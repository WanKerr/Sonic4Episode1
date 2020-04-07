using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static void DmLoadingBuild(AppMain.AMS_FS arc_amb)
    {
        AppMain.dm_loading_mgr_p = new AppMain.DMS_LOADING_MGR();
        AppMain.dm_loading_tex = new AppMain.AOS_TEXTURE[1];
        for (int index = 0; index < 1; ++index)
            AppMain.dm_loading_tex[index] = new AppMain.AOS_TEXTURE();
        for (int index = 0; index < 1; ++index)
        {
            string sPath = (string)null;
            AppMain.dm_loading_ama[index] = AppMain.readAMAFile((object)AppMain.amBindGet(arc_amb, 0, out sPath));
            sPath = (string)null;
            AppMain.dm_loading_amb[index] = AppMain.readAMBFile(AppMain.amBindGet(arc_amb, 1, out sPath));
            AppMain.dm_loading_amb[index].dir = sPath;
        }
        for (int index = 0; index < 1; ++index)
        {
            AppMain.AoTexBuild(AppMain.dm_loading_tex[index], AppMain.dm_loading_amb[index]);
            AppMain.AoTexLoad(AppMain.dm_loading_tex[index]);
        }
    }

    private static bool DmLoadingBuildCheck()
    {
        return AppMain.dmLoadingIsTexLoad() != (short)0;
    }

    private static void DmLoadingFlush()
    {
        for (int index = 0; index < 1; ++index)
            AppMain.AoTexRelease(AppMain.dm_loading_tex[index]);
    }

    private static bool DmLoadingFlushCheck()
    {
        if (AppMain.dmLoadingIsTexRelease() == (short)0)
            return false;
        if (AppMain.dm_loading_mgr_p != null)
            AppMain.dm_loading_mgr_p = (AppMain.DMS_LOADING_MGR)null;
        return true;
    }

    private static void DmLoadingStart()
    {
        AppMain.dmLoadingInit();
    }

    private static bool DmLoadingIsExit()
    {
        return AppMain.dm_loading_mgr_p == null || AppMain.dm_loading_mgr_p.tcb == null;
    }

    private static void DmLoadingExit()
    {
        if (AppMain.dm_loading_mgr_p.tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.dm_loading_mgr_p.tcb);
        AppMain.dm_loading_mgr_p.tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void DmLoadingSetLoadComplete()
    {
        AppMain.dm_loading_check_load_comp = true;
    }

    private static void dmLoadingInit()
    {
        AppMain.dm_loading_mgr_p.tcb = AppMain.mtTaskMake(new AppMain.GSF_TASK_PROCEDURE(AppMain.dmLoadingProcMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.dmLoadingDest), 0U, (ushort)short.MaxValue, 8192U, 10, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_LOADING_MAIN_WORK()), "LOADING_MAIN");
        AppMain.DMS_LOADING_MAIN_WORK work = (AppMain.DMS_LOADING_MAIN_WORK)AppMain.dm_loading_mgr_p.tcb.work;
        work.draw_state = AppMain.AoActSysGetDrawStateEnable() ? 1U : 0U;
        AppMain.AoActSysSetDrawStateEnable(work.draw_state == 1U);
        if (work.draw_state != 0U)
            AppMain.dm_loading_draw_state = AppMain.AoActSysGetDrawState();
        AppMain.dmLoadingSetInitData(work);
        work.proc_update = new AppMain.DMS_LOADING_MAIN_WORK._proc_update_(AppMain.dmLoadingProcInit);
    }

    private static void dmLoadingSetInitData(AppMain.DMS_LOADING_MAIN_WORK main_work)
    {
        AppMain.dm_loading_check_load_comp = false;
        switch (AppMain.SyGetEvtInfo().cur_evt_id)
        {
            case 6:
            case 9:
            case 11:
                main_work.is_maingame_load = true;
                main_work.sonic_pos_x = 0.0f;
                break;
            default:
                main_work.is_maingame_load = false;
                main_work.sonic_pos_x = 0.0f;
                break;
        }
        main_work.lang_id = AppMain.GsEnvGetLanguage();
    }

    private static void dmLoadingProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_LOADING_MAIN_WORK work = (AppMain.DMS_LOADING_MAIN_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
            AppMain.DmLoadingExit();
        if (work.proc_update != null)
            work.proc_update(work);
        if (work.proc_draw == null)
            return;
        work.proc_draw(work);
    }

    private static void dmLoadingDest(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static void dmLoadingProcInit(AppMain.DMS_LOADING_MAIN_WORK main_work)
    {
        main_work.proc_update = new AppMain.DMS_LOADING_MAIN_WORK._proc_update_(AppMain.dmLoadingProcCreateAct);
    }

    private static void dmLoadingProcCreateAct(AppMain.DMS_LOADING_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 8U; ++index)
        {
            AppMain.A2S_AMA_HEADER ama = AppMain.dm_loading_ama[0];
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_loading_tex[0]));
            main_work.act[(int)index] = AppMain.AoActCreate(ama, AppMain.g_dm_act_id_tbl_loading[(int)index]);
        }
        main_work.proc_update = new AppMain.DMS_LOADING_MAIN_WORK._proc_update_(AppMain.dmLoadingProcFadeIn);
        main_work.proc_draw = new AppMain.DMS_LOADING_MAIN_WORK._proc_draw_(AppMain.dmLoadingProcActDraw);
        if (main_work.is_maingame_load)
            AppMain.IzFadeInitEasy(0U, 0U, 32f);
        else
            AppMain.IzFadeInitEasy(0U, 0U, 32f);
    }

    private static void dmLoadingProcFadeIn(AppMain.DMS_LOADING_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        main_work.proc_update = new AppMain.DMS_LOADING_MAIN_WORK._proc_update_(AppMain.dmLoadingProcNowLoading);
    }

    private static void dmLoadingProcNowLoading(AppMain.DMS_LOADING_MAIN_WORK main_work)
    {
        if (AppMain.dm_loading_check_load_comp && (double)main_work.timer > 60.0)
        {
            main_work.proc_update = new AppMain.DMS_LOADING_MAIN_WORK._proc_update_(AppMain.dmLoadingProcAlreadyLoaded);
            main_work.timer = 0.0f;
            if (main_work.is_maingame_load)
                AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 3U, 32f, true);
            else
                AppMain.IzFadeInitEasy(0U, 3U, 32f);
        }
        if ((double)main_work.sonic_set_frame >= 12.0)
            main_work.sonic_set_frame = 0.0f;
        ++main_work.sonic_set_frame;
        ++main_work.timer;
    }

    private static void dmLoadingProcAlreadyLoaded(AppMain.DMS_LOADING_MAIN_WORK main_work)
    {
        if ((double)main_work.timer > 32.0)
        {
            main_work.proc_update = new AppMain.DMS_LOADING_MAIN_WORK._proc_update_(AppMain.dmLoadingProcFadeOut);
            main_work.timer = 0.0f;
        }
        if ((double)main_work.sonic_set_frame >= 12.0)
            main_work.sonic_set_frame = 0.0f;
        main_work.sonic_pos_x += 50f;
        ++main_work.sonic_set_frame;
        ++main_work.timer;
    }

    private static void dmLoadingProcFadeOut(AppMain.DMS_LOADING_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        main_work.proc_update = new AppMain.DMS_LOADING_MAIN_WORK._proc_update_(AppMain.dmLoadingProcStopDraw);
        main_work.proc_draw = (AppMain.DMS_LOADING_MAIN_WORK._proc_draw_)null;
    }

    private static void dmLoadingProcStopDraw(AppMain.DMS_LOADING_MAIN_WORK main_work)
    {
        for (int index = 0; index < 8; ++index)
        {
            if (main_work.act[index] != null)
            {
                AppMain.AoActDelete(main_work.act[index]);
                main_work.act[index] = (AppMain.AOS_ACTION)null;
            }
        }
        main_work.proc_update = (AppMain.DMS_LOADING_MAIN_WORK._proc_update_)null;
        main_work.flag |= 1U;
    }

    private static void dmLoadingProcActDraw(AppMain.DMS_LOADING_MAIN_WORK main_work)
    {
        AppMain.dmLoadingCommonDraw(main_work);
        if (main_work.draw_state == 0U)
            return;
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.dmLoadingTaskDraw), (ushort)32768, (object)null);
    }

    private static void dmLoadingTaskDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(AppMain.dm_loading_draw_state);
        AppMain.amDrawEndScene();
    }

    private static void dmLoadingCommonDraw(AppMain.DMS_LOADING_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(8192U);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_loading_tex[0]));
        for (int index = 0; index < 8; ++index)
            AppMain.AoActSortRegAction(main_work.act[index]);
        AppMain.AoActSetFrame(main_work.act[3], main_work.sonic_set_frame);
        AppMain.AoActSetFrame(main_work.act[7], (float)main_work.lang_id);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_loading_tex[0]));
        for (int index = 0; index <= 2; ++index)
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
        AppMain.AoActAcmPush();
        AppMain.AoActAcmInit();
        AppMain.AoActAcmApplyTrans(main_work.sonic_pos_x, -10f, 0.0f);
        AppMain.AoActUpdate(main_work.act[3], 0.0f);
        AppMain.AoActAcmPop();
        for (int index = 4; index <= 6; ++index)
            AppMain.AoActUpdate(main_work.act[index], 1f);
        AppMain.AoActUpdate(main_work.act[7], 0.0f);
        if (main_work.lang_id >= 6)
            main_work.act[7].sprite.tex_id = 9 + main_work.lang_id - 6;
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private static short dmLoadingIsTexLoad()
    {
        for (int index = 0; index < 1; ++index)
        {
            if (!AppMain.AoTexIsLoaded(AppMain.dm_loading_tex[index]))
                return 0;
        }
        return 1;
    }

    private static short dmLoadingIsTexRelease()
    {
        for (int index = 0; index < 1; ++index)
        {
            if (!AppMain.AoTexIsReleased(AppMain.dm_loading_tex[index]))
                return 0;
        }
        return 1;
    }

}