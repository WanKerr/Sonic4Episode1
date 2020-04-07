using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    public static void DmSaveStart(uint disp_flag, bool is_first_save, bool is_task_draw)
    {
        switch (AppMain.SyGetEvtInfo().cur_evt_id)
        {
            case 6:
                break;
            case 9:
                break;
            case 11:
                break;
            default:
                if (((int)disp_flag & 4) != 0 && AppMain.GsTrialIsTrial())
                    break;
                AppMain.dm_save_mgr.Clear();
                AppMain.dm_save_mgr_p = AppMain.dm_save_mgr;
                AppMain.dm_save_msg_flag = disp_flag;
                AppMain.dm_save_first_save = is_first_save;
                AppMain.dm_save_is_task_draw = is_task_draw;
                AppMain.dm_save_is_snd_build = false;
                AppMain.dmSaveInit();
                break;
        }
    }

    private static void DmSaveAttenMsgStart()
    {
        int curEvtId = (int)AppMain.SyGetEvtInfo().cur_evt_id;
        AppMain.dm_save_mgr.Clear();
        AppMain.dm_save_mgr_p = AppMain.dm_save_mgr;
        AppMain.dm_save_msg_flag = 2U;
        AppMain.dm_save_first_save = false;
        AppMain.dm_save_is_task_draw = false;
        AppMain.dm_save_is_snd_build = true;
        AppMain.dmSaveInit();
    }

    private static void DmSaveMenuStart(bool is_task_draw, bool is_snd_build)
    {
        switch (AppMain.SyGetEvtInfo().cur_evt_id)
        {
            case 6:
                break;
            case 9:
                break;
            case 11:
                break;
            default:
                if (!AppMain.AoAccountIsCurrentEnable())
                    break;
                AppMain.dmSaveSetSysDataForBackup();
                if (AppMain.GsTrialIsTrial() || AppMain.dmSaveIsSaveNecessary())
                    break;
                AppMain.dm_save_mgr.Clear();
                AppMain.dm_save_mgr_p = AppMain.dm_save_mgr;
                AppMain.dm_save_msg_flag = 4U;
                AppMain.dm_save_first_save = false;
                AppMain.dm_save_is_task_draw = is_task_draw;
                AppMain.dm_save_is_snd_build = is_snd_build;
                AppMain.dmSaveInit();
                break;
        }
    }

    public static bool DmSaveIsExit()
    {
        return AppMain.dm_save_mgr_p == null || AppMain.dm_save_mgr_p.tcb == null;
    }

    private static bool DmSaveIsDraw()
    {
        return AppMain.dm_save_draw_reserve;
    }

    private static void dmSaveInit()
    {
        AppMain.dm_save_mgr_p.tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.dmSaveProcMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.dmSaveDest), 0U, (ushort)short.MaxValue, 8192U, 0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_SAVE_MAIN_WORK()), "SAVE_TASK");
        AppMain.DMS_SAVE_MAIN_WORK work = (AppMain.DMS_SAVE_MAIN_WORK)AppMain.dm_save_mgr_p.tcb.work;
        AppMain.dm_save_disp_flag = 0U;
        AppMain.dm_save_is_draw_state = 0U;
        AppMain.dm_save_win_mode = 0;
        AppMain.dm_save_draw_reserve = false;
        for (int index = 0; index < 2; ++index)
        {
            AppMain.dm_save_win_size_rate[index] = 0.0f;
            AppMain.dm_save_cmn_tex[index] = (AppMain.AOS_TEXTURE)null;
        }
        for (int index = 0; index < 6; ++index)
            AppMain.dm_save_act[index] = (AppMain.AOS_ACTION)null;
        work.announce_flag = AppMain.dm_save_msg_flag;
        work.draw_state = AppMain.AoActSysGetDrawStateEnable() ? 1U : 0U;
        AppMain.dm_save_draw_state = work.draw_state == 0U ? 0U : AppMain.AoActSysGetDrawState();
        for (int index = 0; index < 2; ++index)
            AppMain.dm_save_cmn_tex[index] = (AppMain.AOS_TEXTURE)null;
        AppMain.dm_save_is_draw_state = work.draw_state;
        work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveLoadFontData);
    }

    private static void dmSaveSetSysDataForBackup()
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        SSystem instance = SSystem.CreateInstance();
        instance.SetKilled(mainSysInfo.ene_kill_count);
        instance.SetPlayerStock(mainSysInfo.rest_player_num);
    }

    private static bool dmSaveIsSaveNecessary()
    {
        return false;
    }

    private static void dmSaveProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_SAVE_MAIN_WORK work = (AppMain.DMS_SAVE_MAIN_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
        {
            AppMain.mtTaskClearTcb(tcb);
            AppMain.dm_save_disp_flag = 0U;
            AppMain.dm_save_is_draw_state = 0U;
            AppMain.dm_save_win_mode = 0;
            AppMain.dm_save_is_task_draw = false;
            for (int index = 0; index < 2; ++index)
                AppMain.dm_save_win_size_rate[index] = 0.0f;
            AppMain.dm_save_mgr_p = (AppMain.DMS_SAVE_MGR)null;
        }
        if (((int)work.flag & int.MinValue) != 0 && !AppMain.AoAccountIsCurrentEnable())
        {
            work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcStopDraw);
            work.proc_input = (AppMain._saveproc_input_update)null;
            work.proc_draw = (AppMain._saveproc_draw)null;
            AppMain.dm_save_draw_reserve = false;
            work.flag &= (uint)int.MaxValue;
        }
        else
        {
            if (work.proc_menu_update != null)
                work.proc_menu_update(work);
            if (work.proc_draw == null || AppMain.AoSysMsgIsShow())
                return;
            work.proc_draw();
        }
    }

    private static void dmSaveDest(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static void dmSaveLoadFontData(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (AppMain.SyGetEvtInfo().cur_evt_id == (short)10)
            AppMain.GsFontBuild(false);
        else
            AppMain.GsFontBuild();
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveIsLoadFontData);
    }

    private static void dmSaveIsLoadFontData(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (!AppMain.GsFontIsBuilded())
            return;
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveLoadRequest);
    }

    private static void dmSaveLoadRequest(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        main_work.arc_cmn_amb_fs[0] = AppMain.amFsReadBackground("DEMO/CMN/D_CMN_WIN.AMB");
        main_work.arc_cmn_amb_fs[1] = AppMain.amFsReadBackground(AppMain.dm_save_menu_cmn_lng_amb_name_tbl[AppMain.GsEnvGetLanguage()]);
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcLoadWait);
    }

    private static void dmSaveProcLoadWait(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (AppMain.dmSaveIsDataLoad(main_work) == 0)
            return;
        for (int index = 0; index < 2; ++index)
        {
            main_work.arc_cmn_amb[index] = AppMain.readAMBFile(main_work.arc_cmn_amb_fs[index]);
            main_work.arc_cmn_amb_fs[index] = (AppMain.AMS_FS)null;
            main_work.cmn_ama[index] = AppMain.readAMAFile(AppMain.amBindGet(main_work.arc_cmn_amb[index], 0));
            string sPath;
            main_work.cmn_amb[index] = AppMain.readAMBFile(AppMain.amBindGet(main_work.arc_cmn_amb[index], 1, out sPath));
            main_work.cmn_amb[index].dir = sPath;
            AppMain.amFsClearRequest(main_work.arc_cmn_amb_fs[index]);
            main_work.arc_cmn_amb_fs[index] = (AppMain.AMS_FS)null;
            AppMain.AoTexBuild(main_work.cmn_tex[index], main_work.cmn_amb[index]);
            AppMain.AoTexLoad(main_work.cmn_tex[index]);
        }
        if (AppMain.dm_save_is_snd_build)
            AppMain.DmSoundBuild();
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcTexBuildWait);
    }

    private static void dmSaveProcTexBuildWait(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (AppMain.dmSaveIsTexLoad(main_work) != 1)
            return;
        for (int index = 0; index < 2; ++index)
            AppMain.dm_save_cmn_tex[index] = main_work.cmn_tex[index];
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcCreateAct);
    }

    private static void dmSaveProcCreateAct(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 6U; ++index)
        {
            AppMain.A2S_AMA_HEADER ama;
            AppMain.AOS_TEXTURE tex;
            if (index >= 1U)
            {
                ama = main_work.cmn_ama[1];
                tex = main_work.cmn_tex[1];
            }
            else
            {
                ama = main_work.cmn_ama[0];
                tex = main_work.cmn_tex[0];
            }
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(tex));
            main_work.act[(int)index] = AppMain.AoActCreate(ama, AppMain.g_dm_act_id_tbl[(int)index]);
            AppMain.dm_save_act[(int)index] = main_work.act[(int)index];
        }
        if (((int)AppMain.dm_save_msg_flag & 4) != 0)
            main_work.flag |= 2147483648U;
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowNodispIdle);
    }

    private static void dmSaveProcWindowNodispIdle(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 8) != 0 || main_work.announce_flag != 0U)
        {
            main_work.win_timer = 0;
            for (uint index = 0; index < 4U; ++index)
            {
                if (((int)main_work.announce_flag & 1 << (int)index) != 0)
                {
                    AppMain.dm_save_win_mode = main_work.win_mode = (int)index;
                    break;
                }
            }
            if (main_work.win_mode == 0)
            {
                AppMain.DmCmnBackupLoad();
                main_work.proc_input = (AppMain._saveproc_input_update)null;
                main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowOpenWaitLoadIdle);
            }
            else if (main_work.win_mode == 1)
            {
                main_work.proc_input = new AppMain._saveproc_input_update(AppMain.dmSaveInputProcWindow);
                main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowOpenEfct);
                main_work.proc_draw = new AppMain._saveproc_draw(AppMain.DmSaveWinSelectDraw);
                AppMain.dm_save_draw_reserve = true;
                AppMain.DmSoundPlaySE("Window");
            }
            else if (main_work.win_mode == 2)
            {
                AppMain.DmCmnBackupSave(AppMain.dm_save_first_save, false, false);
                main_work.proc_input = (AppMain._saveproc_input_update)null;
                main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowOpenWaitIdle);
                main_work.proc_draw = (AppMain._saveproc_draw)null;
                AppMain.dm_save_draw_reserve = false;
            }
            else if (main_work.win_mode == 3)
            {
                AppMain.DmCmnBackupSave(AppMain.dm_save_first_save, true, false);
                main_work.proc_input = (AppMain._saveproc_input_update)null;
                main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowOpenWaitIdle);
                main_work.proc_draw = (AppMain._saveproc_draw)null;
                AppMain.dm_save_draw_reserve = false;
            }
            main_work.flag &= 4294967287U;
        }
        else
        {
            main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcStopDraw);
            AppMain.dm_save_draw_reserve = false;
        }
    }

    private static void dmSaveProcWindowOpenWaitLoadIdle(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowOpenEfct);
        main_work.proc_draw = new AppMain._saveproc_draw(AppMain.DmSaveWinSelectDraw);
        AppMain.DmSoundPlaySE("Window");
        AppMain.dm_save_draw_reserve = true;
    }

    private static void dmSaveProcWindowOpenWaitIdle(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowOpenEfct);
        main_work.proc_draw = new AppMain._saveproc_draw(AppMain.DmSaveWinSelectDraw);
        AppMain.DmSoundPlaySE("Window");
        AppMain.dm_save_draw_reserve = true;
    }

    private static void dmSaveProcWindowOpenEfct(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 16) != 0)
        {
            main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowAnnounceIdle);
            main_work.disp_flag |= 1U;
            AppMain.dm_save_disp_flag = main_work.disp_flag;
            main_work.flag &= 4294967279U;
        }
        else
            AppMain.dmSaveSetWinOpenEfct(main_work);
    }

    private static void dmSaveProcWindowAnnounceIdle(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (main_work.win_mode == 0)
        {
            if (AppMain.DmCmnBackupIsLoadFinished())
            {
                main_work.win_timer = 8;
                if (main_work.timer >= 60)
                {
                    main_work.disp_flag &= 4294967294U;
                    AppMain.dm_save_disp_flag = main_work.disp_flag;
                    main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowCloseEfct);
                    main_work.timer = 0;
                }
            }
            else if (AppMain.AoSysMsgIsShow())
            {
                main_work.win_timer = 8;
                main_work.disp_flag &= 4294967294U;
                AppMain.dm_save_disp_flag = main_work.disp_flag;
                main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowCloseEfct);
                main_work.timer = 0;
            }
            else 
                main_work.timer = 0;
        }
        else if (main_work.win_mode == 1)
        {
            if (((int)main_work.flag & 4) != 0)
            {
                main_work.proc_input = (AppMain._saveproc_input_update)null;
                main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowCloseEfct);
                main_work.win_timer = 8;
                main_work.disp_flag &= 4294967294U;
                AppMain.DmSoundPlaySE("Ok");
                main_work.flag &= 4294967291U;
                main_work.timer = 0;
            }
        }
        else if (main_work.win_mode == 2)
        {
            if (AppMain.DmCmnBackupIsSaveFinished())
            {
                main_work.win_timer = 8;
                if (main_work.timer >= 60)
                {
                    main_work.proc_input = (AppMain._saveproc_input_update)null;
                    main_work.disp_flag &= 4294967294U;
                    main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowCloseEfct);
                    main_work.timer = 0;
                }
            }
            else 
                main_work.timer = 0;
        }
        else if (main_work.win_mode == 3)
        {
            if (AppMain.DmCmnBackupIsSaveFinished())
            {
                main_work.win_timer = 8;
                if (main_work.timer >= 60)
                {
                    main_work.proc_input = (AppMain._saveproc_input_update)null;
                    main_work.disp_flag &= 4294967294U;
                    main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowCloseEfct);
                    main_work.timer = 0;
                }
            }
            else
                main_work.timer = 0;
        }
        else
        {
            main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowCloseEfct);
            main_work.disp_flag &= 4294967294U;
        }
        AppMain.dm_save_disp_flag = main_work.disp_flag;
        ++main_work.timer;
    }

    private static void dmSaveProcWindowCloseEfct(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 16) != 0)
        {
            main_work.proc_menu_update = main_work.win_mode != 1 ? (main_work.win_mode != 0 ? new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowNodispIdle) : new AppMain._saveproc_input_update(AppMain.dmSaveProcWaitLoadEnd)) : new AppMain._saveproc_input_update(AppMain.dmSaveProcWaitSeStop);
            main_work.announce_flag &= (uint)~(1 << main_work.win_mode);
            main_work.flag &= 4294967279U;
            main_work.proc_draw = (AppMain._saveproc_draw)null;
            AppMain.dm_save_draw_reserve = false;
        }
        AppMain.dmSaveSetWinCloseEfct(main_work);
    }

    private static void dmSaveProcWaitSeStop(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (main_work.timer > 60)
        {
            main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowNodispIdle);
            main_work.timer = 0;
        }
        else
            ++main_work.timer;
    }

    private static void dmSaveProcWaitLoadEnd(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (!AppMain.DmCmnBackupIsLoadFinished())
            return;
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcWindowNodispIdle);
        main_work.timer = 0;
    }

    private static void dmSaveProcStopDraw(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcDataRelease);
    }

    private static void dmSaveProcDataRelease(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
            AppMain.AoTexRelease(main_work.cmn_tex[index]);
        if (AppMain.dm_save_is_snd_build)
        {
            AppMain.DmSoundExit();
            AppMain.DmSoundFlush();
        }
        main_work.proc_menu_update = new AppMain._saveproc_input_update(AppMain.dmSaveProcFinish);
    }

    private static void dmSaveProcFinish(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (AppMain.dmSaveIsTexRelease(main_work) != 1)
            return;
        for (int index = 0; index < 2; ++index)
            AppMain.dm_save_cmn_tex[index] = (AppMain.AOS_TEXTURE)null;
        for (int index = 0; index < 6; ++index)
        {
            if (main_work.act[index] != null)
            {
                AppMain.AoActDelete(main_work.act[index]);
                main_work.act[index] = (AppMain.AOS_ACTION)null;
            }
            AppMain.dm_save_act[index] = (AppMain.AOS_ACTION)null;
        }
        for (int index = 0; index < 2; ++index)
        {
            if (main_work.arc_cmn_amb[index] != null)
                main_work.arc_cmn_amb[index] = (AppMain.AMS_AMB_HEADER)null;
        }
        main_work.flag |= 1U;
        main_work.proc_menu_update = (AppMain._saveproc_input_update)null;
    }

    private static void dmSaveInputProcWindow(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (AppMain.AoAccountGetCurrentId() < 0)
        {
            if (AoPad.AoPadSomeoneStand(ControllerConsts.CONFIRM) < 0)
                return;
            main_work.flag |= 4U;
        }
        else
        {
            if ((AoPad.AoPadStand() & ControllerConsts.CONFIRM) == 0)
                return;
            main_work.flag |= 4U;
        }
    }

    private static void DmSaveWinSelectDraw()
    {
        AppMain.AoActSysSetDrawTaskPrio(61439U);
        int num1;
        int num2;
        if (((int)AppMain.dm_save_msg_flag & 2) != 0)
        {
            num1 = 749;
            num2 = (int)((180.0 + (double)AppMain.dm_save_win_size_y_tbl[AppMain.GsEnvGetLanguage()]) * (27.0 / 16.0));
        }
        else
        {
            if (AppMain.GsEnvGetLanguage() == 4)
            {
                num1 = 749;
                num2 = 303;
            }
            else
            {
                num1 = 641;
                num2 = 303;
            }
        }
        uint tex_id = !AppMain.dm_save_is_task_draw ? (((int)AppMain.dm_save_msg_flag & 2) == 0 ? 0U : 1U) : 1U;
        if (AppMain.dm_save_is_draw_state != 0U)
            AppMain.AoWinSysDrawState(0, AppMain.AoTexGetTexList(AppMain.dm_save_cmn_tex[0]), tex_id, 480f, 356f, (float)num1 * AppMain.dm_save_win_size_rate[0], (float)num2 * AppMain.dm_save_win_size_rate[1], AppMain.dm_save_draw_state);
        else
            AppMain.AoWinSysDrawTask(0, AppMain.AoTexGetTexList(AppMain.dm_save_cmn_tex[0]), tex_id, 480f, 356f, (float)num1 * AppMain.dm_save_win_size_rate[0], (float)((double)num2 * (double)AppMain.dm_save_win_size_rate[1] * 0.899999976158142), (ushort)61439);
        if (((int)AppMain.dm_save_disp_flag & 1) != 0)
        {
            switch (AppMain.dm_save_win_mode)
            {
                case 0:
                    AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_save_cmn_tex[1]));
                    AppMain.AoActSortRegAction(AppMain.dm_save_act[2]);
                    break;
                case 1:
                    AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_save_cmn_tex[1]));
                    AppMain.AoActSortRegAction(AppMain.dm_save_act[3]);
                    AppMain.AoActSortRegAction(AppMain.dm_save_act[5]);
                    break;
                case 2:
                    AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_save_cmn_tex[1]));
                    AppMain.AoActSortRegAction(AppMain.dm_save_act[4]);
                    break;
                case 3:
                    AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_save_cmn_tex[1]));
                    AppMain.AoActSortRegAction(AppMain.dm_save_act[4]);
                    break;
            }
            AppMain.AoActAcmPush();
            int num3 = AppMain.GsEnvGetLanguage() == 0 ? 0 : AppMain.dm_save_win_size_y_tbl[AppMain.GsEnvGetLanguage()] / 2;
            for (int index = 0; index < 6; ++index)
            {
                AppMain.AOS_TEXTURE tex = index < 1 ? AppMain.dm_save_cmn_tex[0] : AppMain.dm_save_cmn_tex[1];
                AppMain.AoActAcmInit();
                AppMain.AoActAcmApplyTrans(AppMain.dm_save_win_act_pos_tbl[index][0], AppMain.dm_save_win_act_pos_tbl[index][1], 0.0f);
                if (((int)AppMain.dm_save_msg_flag & 2) != 0)
                {
                    switch (index)
                    {
                        case 0:
                        case 1:
                            AppMain.AoActAcmApplyTrans(-32f, (float)(num3 * -1), 0.0f);
                            break;
                        case 5:
                            AppMain.AoActAcmApplyTrans(0.0f, 16f + (float)num3, 0.0f);
                            break;
                    }
                }
                AppMain.AoActAcmApplyScale(27f / 16f, 27f / 16f);
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(tex));
                AppMain.AoActUpdate(AppMain.dm_save_act[index], 0.0f);
            }
            AppMain.AoActAcmPop();
            AppMain.AoActSortExecute();
            AppMain.AoActSortDraw();
            AppMain.AoActSortUnregAll();
        }
        if (AppMain.dm_save_is_draw_state == 0U || !AppMain.dm_save_is_task_draw)
            return;
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.dmSaveTaskDraw), (ushort)61439, 0U);
    }

    private static void dmSaveTaskDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(AppMain.dm_save_draw_state);
        AppMain.amDrawEndScene();
    }

    private static int dmSaveIsDataLoad(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.amFsIsComplete(main_work.arc_cmn_amb_fs[index]))
                return 0;
        }
        return 1;
    }

    private static int dmSaveIsTexLoad(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.AoTexIsLoaded(main_work.cmn_tex[index]))
                return 0;
        }
        return AppMain.dm_save_is_snd_build && !AppMain.DmSoundBuildCheck() ? 0 : 1;
    }

    private static int dmSaveIsTexRelease(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.AoTexIsReleased(main_work.cmn_tex[index]))
                return 0;
        }
        return 1;
    }

    private static void dmSaveSetWinOpenEfct(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        if (main_work.win_timer > 8)
        {
            main_work.flag |= 16U;
            main_work.win_timer = 0;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int)index] = 1f;
        }
        else
            ++main_work.win_timer;
        for (uint index = 0; index < 2U; ++index)
        {
            main_work.win_size_rate[(int)index] = main_work.win_timer == 0 ? 1f : (float)main_work.win_timer / 8f;
            if ((double)main_work.win_size_rate[(int)index] > 1.0)
                main_work.win_size_rate[(int)index] = 1f;
            AppMain.dm_save_win_size_rate[(int)index] = main_work.win_size_rate[(int)index];
        }
    }

    private static void dmSaveSetWinCloseEfct(AppMain.DMS_SAVE_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
        {
            main_work.win_size_rate[(int)index] = main_work.win_timer == 0 ? 0.0f : (float)main_work.win_timer / 8f;
            AppMain.dm_save_win_size_rate[(int)index] = main_work.win_size_rate[(int)index];
        }
        if (main_work.win_timer < 0)
        {
            main_work.flag |= 16U;
            main_work.win_timer = 0;
            for (uint index = 0; index < 2U; ++index)
            {
                main_work.win_size_rate[(int)index] = 0.0f;
                AppMain.dm_save_win_size_rate[(int)index] = main_work.win_size_rate[(int)index];
            }
        }
        else
            --main_work.win_timer;
    }

}