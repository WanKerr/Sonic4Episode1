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
    private static void GmOverBuildDataInit()
    {
        for (int index = 0; index < 2; ++index)
        {
            AppMain.gm_over_textures[index].Clear();
            AppMain.gm_over_texamb_list[index] = (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, AppMain.gm_over_tex_amb_idx_tbl[AppMain.GsEnvGetLanguage()][index], AppMain.GmGameDatGetCockpitData());
            AppMain.AoTexBuild(AppMain.gm_over_textures[index], AppMain.gm_over_texamb_list[index]);
            AppMain.AoTexLoad(AppMain.gm_over_textures[index]);
        }
    }

    private static bool GmOverBuildDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.AoTexIsLoaded(AppMain.gm_over_textures[index]))
                flag = false;
        }
        return flag;
    }

    private static void GmOverFlushDataInit()
    {
        for (int index = 0; index < 2; ++index)
            AppMain.AoTexRelease(AppMain.gm_over_textures[index]);
    }

    private static bool GmOverFlushDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < 2; ++index)
        {
            if (AppMain.gm_over_texamb_list[index] != null)
            {
                if (!AppMain.AoTexIsReleased(AppMain.gm_over_textures[index]))
                {
                    flag = false;
                }
                else
                {
                    AppMain.gm_over_texamb_list[index] = (AppMain.AMS_AMB_HEADER)null;
                    AppMain.gm_over_textures[index].Clear();
                }
            }
        }
        return flag;
    }

    private static void GmOverStart(int type)
    {
        SaveState.deleteSave();
        AppMain.gm_over_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmOverMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmOverDest), 0U, (ushort)0, 18464U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_OVER_MGR_WORK()), "GM_OVER_MGR");
        AppMain.GMS_OVER_MGR_WORK work1 = (AppMain.GMS_OVER_MGR_WORK)AppMain.gm_over_tcb.work;
        work1.Clear();
        for (int index = 0; index < 4; ++index)
        {
            AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_COCKPIT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "GAME_OVER");
            AppMain.GMS_COCKPIT_2D_WORK cpit_2d = (AppMain.GMS_COCKPIT_2D_WORK)work2;
            AppMain.ObjObjectAction2dAMALoadSetTexlist(work2, cpit_2d.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.gm_over_ama_amb_idx_tbl[AppMain.GsEnvGetLanguage()][1], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(AppMain.gm_over_textures[1]), AppMain.gm_over_string_act_id_tbl[AppMain.GsEnvGetLanguage()][index], 0);
            work1.string_sub_parts[index] = cpit_2d;
            AppMain.gmOverSetActionHide(cpit_2d);
        }
        for (int index = 0; index < 2; ++index)
        {
            AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_COCKPIT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "GAME_OVER");
            AppMain.GMS_COCKPIT_2D_WORK cpit_2d = (AppMain.GMS_COCKPIT_2D_WORK)work2;
            AppMain.ObjObjectAction2dAMALoadSetTexlist(work2, cpit_2d.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.gm_over_ama_amb_idx_tbl[AppMain.GsEnvGetLanguage()][0], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(AppMain.gm_over_textures[0]), AppMain.gm_over_fadeout_act_id_tbl[index], 0);
            work1.fadeout_sub_parts[index] = cpit_2d;
            work2.pos.z = -65536;
            work2.disp_flag &= 4294967291U;
            AppMain.gmOverSetActionHide(cpit_2d);
        }
        switch (type)
        {
            case 0:
                AppMain.gmOverProcUpdateGOInit(work1);
                break;
            case 1:
                AppMain.gmOverProcUpdateTOInit(work1);
                break;
        }
        work1.proc_disp = new AppMain._GMS_OVER_MGR_WORK_UD_(AppMain.gmOverProcDispLoop);
    }

    private static void GmOverExit()
    {
        if (AppMain.gm_over_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_over_tcb);
        AppMain.gm_over_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static bool gmOverIsSkipKeyOn()
    {
        if ((AoPad.AoPadDirect() & ControllerConsts.CONFIRM) == 0 && !AppMain.isBackKeyPressed())
            return false;
        AppMain.setBackKeyRequest(false);
        return true;
    }

    private static void gmOverSetActionHide(AppMain.GMS_COCKPIT_2D_WORK cpit_2d)
    {
        ((AppMain.OBS_OBJECT_WORK)cpit_2d).disp_flag |= 4128U;
    }

    private static void gmOverSetActionPlay(AppMain.GMS_COCKPIT_2D_WORK cpit_2d)
    {
        ((AppMain.OBS_OBJECT_WORK)cpit_2d).disp_flag &= 4294963167U;
    }

    private static void gmOverSetActionPause(AppMain.GMS_COCKPIT_2D_WORK cpit_2d)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)cpit_2d;
        obsObjectWork.disp_flag &= 4294967263U;
        obsObjectWork.disp_flag |= 4096U;
    }

    private static void gmOverDest(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static void gmOverMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_OVER_MGR_WORK work = (AppMain.GMS_OVER_MGR_WORK)tcb.work;
        if (work.proc_update != null)
            work.proc_update(work);
        if (work.proc_disp == null)
            return;
        work.proc_disp(work);
    }

    private static void gmOverProcUpdateGOInit(AppMain.GMS_OVER_MGR_WORK mgr_work)
    {
        mgr_work.wait_timer = 30U;
        mgr_work.proc_update = new AppMain._GMS_OVER_MGR_WORK_UD_(AppMain.gmOverProcUpdateGOWaitStart);
    }

    private static void gmOverProcUpdateGOWaitStart(AppMain.GMS_OVER_MGR_WORK mgr_work)
    {
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            AppMain.gmOverSetActionPlay(mgr_work.string_sub_parts[0]);
            AppMain.gmOverSetActionPlay(mgr_work.string_sub_parts[1]);
            mgr_work.wait_timer = 480U;
            mgr_work.proc_update = new AppMain._GMS_OVER_MGR_WORK_UD_(AppMain.gmOverProcUpdateGOLoop);
        }
    }

    private static void gmOverProcUpdateGOLoop(AppMain.GMS_OVER_MGR_WORK mgr_work)
    {
        if (AppMain.gmOverIsSkipKeyOn())
            mgr_work.wait_timer = 0U;
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            AppMain.gmOverSetActionPlay(mgr_work.fadeout_sub_parts[0]);
            mgr_work.proc_update = new AppMain._GMS_OVER_MGR_WORK_UD_(AppMain.gmOverProcUpdateGOWaitFadeEnd);
        }
    }

    private static void gmOverProcUpdateGOWaitFadeEnd(AppMain.GMS_OVER_MGR_WORK mgr_work)
    {
        if (((int)((AppMain.OBS_OBJECT_WORK)mgr_work.fadeout_sub_parts[0]).disp_flag & 8) == 0)
            return;
        AppMain.IzFadeInitEasy(0U, 1U, 1f);
        mgr_work.proc_update = new AppMain._GMS_OVER_MGR_WORK_UD_(AppMain.gmOverProcUpdateGOWaitFinalizeFade);
    }

    private static void gmOverProcUpdateGOWaitFinalizeFade(AppMain.GMS_OVER_MGR_WORK mgr_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.g_gm_main_system.game_flag |= 256U;
        mgr_work.proc_update = (AppMain._GMS_OVER_MGR_WORK_UD_)null;
    }

    private static void gmOverProcUpdateTOInit(AppMain.GMS_OVER_MGR_WORK mgr_work)
    {
        mgr_work.wait_timer = 30U;
        mgr_work.proc_update = new AppMain._GMS_OVER_MGR_WORK_UD_(AppMain.gmOverProcUpdateTOWaitStart);
    }

    private static void gmOverProcUpdateTOWaitStart(AppMain.GMS_OVER_MGR_WORK mgr_work)
    {
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            AppMain.gmOverSetActionPlay(mgr_work.string_sub_parts[2]);
            AppMain.gmOverSetActionPlay(mgr_work.string_sub_parts[3]);
            AppMain.gmOverSetActionPlay(mgr_work.fadeout_sub_parts[1]);
            mgr_work.proc_update = new AppMain._GMS_OVER_MGR_WORK_UD_(AppMain.gmOverProcUpdateTOWaitFadeEnd);
        }
    }

    private static void gmOverProcUpdateTOWaitFadeEnd(AppMain.GMS_OVER_MGR_WORK mgr_work)
    {
        if (((int)((AppMain.OBS_OBJECT_WORK)mgr_work.fadeout_sub_parts[1]).disp_flag & 8) == 0)
            return;
        AppMain.IzFadeInitEasy(0U, 1U, 1f);
        mgr_work.proc_update = new AppMain._GMS_OVER_MGR_WORK_UD_(AppMain.gmOverProcUpdateTOWaitFinalizeFade);
    }

    private static void gmOverProcUpdateTOWaitFinalizeFade(AppMain.GMS_OVER_MGR_WORK mgr_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.g_gm_main_system.game_flag |= 256U;
        mgr_work.proc_update = (AppMain._GMS_OVER_MGR_WORK_UD_)null;
    }

    private static void gmOverProcDispLoop(AppMain.GMS_OVER_MGR_WORK mgr_work)
    {
    }

}