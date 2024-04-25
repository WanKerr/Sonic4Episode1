public partial class AppMain
{
    private static void GmOverBuildDataInit()
    {
        for (int index = 0; index < 2; ++index)
        {
            gm_over_textures[index].Clear();
            gm_over_texamb_list[index] = (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, gm_over_tex_amb_idx_tbl[GsEnvGetLanguage()][index], GmGameDatGetCockpitData());
            AoTexBuild(gm_over_textures[index], gm_over_texamb_list[index]);
            AoTexLoad(gm_over_textures[index]);
        }
    }

    private static bool GmOverBuildDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < 2; ++index)
        {
            if (!AoTexIsLoaded(gm_over_textures[index]))
                flag = false;
        }
        return flag;
    }

    private static void GmOverFlushDataInit()
    {
        for (int index = 0; index < 2; ++index)
            AoTexRelease(gm_over_textures[index]);
    }

    private static bool GmOverFlushDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < 2; ++index)
        {
            if (gm_over_texamb_list[index] != null)
            {
                if (!AoTexIsReleased(gm_over_textures[index]))
                {
                    flag = false;
                }
                else
                {
                    gm_over_texamb_list[index] = null;
                    gm_over_textures[index].Clear();
                }
            }
        }
        return flag;
    }

    private static void GmOverStart(int type)
    {
        SaveState.deleteSave();
        gm_over_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmOverMain), new GSF_TASK_PROCEDURE(gmOverDest), 0U, 0, 18464U, 5, () => new GMS_OVER_MGR_WORK(), "GM_OVER_MGR");
        GMS_OVER_MGR_WORK work1 = (GMS_OVER_MGR_WORK)gm_over_tcb.work;
        work1.Clear();
        for (int index = 0; index < 4; ++index)
        {
            OBS_OBJECT_WORK work2 = GMM_COCKPIT_CREATE_WORK(() => new GMS_COCKPIT_2D_WORK(), null, 0, "GAME_OVER");
            GMS_COCKPIT_2D_WORK cpit_2d = (GMS_COCKPIT_2D_WORK)work2;
            ObjObjectAction2dAMALoadSetTexlist(work2, cpit_2d.obj_2d, null, null, gm_over_ama_amb_idx_tbl[GsEnvGetLanguage()][1], GmGameDatGetCockpitData(), AoTexGetTexList(gm_over_textures[1]), gm_over_string_act_id_tbl[GsEnvGetLanguage()][index], 0);
            work1.string_sub_parts[index] = cpit_2d;
            gmOverSetActionHide(cpit_2d);
        }
        for (int index = 0; index < 2; ++index)
        {
            OBS_OBJECT_WORK work2 = GMM_COCKPIT_CREATE_WORK(() => new GMS_COCKPIT_2D_WORK(), null, 0, "GAME_OVER");
            GMS_COCKPIT_2D_WORK cpit_2d = (GMS_COCKPIT_2D_WORK)work2;
            ObjObjectAction2dAMALoadSetTexlist(work2, cpit_2d.obj_2d, null, null, gm_over_ama_amb_idx_tbl[GsEnvGetLanguage()][0], GmGameDatGetCockpitData(), AoTexGetTexList(gm_over_textures[0]), gm_over_fadeout_act_id_tbl[index], 0);
            work1.fadeout_sub_parts[index] = cpit_2d;
            work2.pos.z = -65536;
            work2.disp_flag &= 4294967291U;
            gmOverSetActionHide(cpit_2d);
        }
        switch (type)
        {
            case 0:
                gmOverProcUpdateGOInit(work1);
                break;
            case 1:
                gmOverProcUpdateTOInit(work1);
                break;
        }
        work1.proc_disp = new _GMS_OVER_MGR_WORK_UD_(gmOverProcDispLoop);
    }

    private static void GmOverExit()
    {
        if (gm_over_tcb == null)
            return;
        mtTaskClearTcb(gm_over_tcb);
        gm_over_tcb = null;
    }

    private static bool gmOverIsSkipKeyOn()
    {
        if ((AoPad.AoPadDirect() & ControllerConsts.CONFIRM) == 0 && !isBackKeyPressed())
            return false;
        setBackKeyRequest(false);
        return true;
    }

    private static void gmOverSetActionHide(GMS_COCKPIT_2D_WORK cpit_2d)
    {
        ((OBS_OBJECT_WORK)cpit_2d).disp_flag |= 4128U;
    }

    private static void gmOverSetActionPlay(GMS_COCKPIT_2D_WORK cpit_2d)
    {
        ((OBS_OBJECT_WORK)cpit_2d).disp_flag &= 4294963167U;
    }

    private static void gmOverSetActionPause(GMS_COCKPIT_2D_WORK cpit_2d)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)cpit_2d;
        obsObjectWork.disp_flag &= 4294967263U;
        obsObjectWork.disp_flag |= 4096U;
    }

    private static void gmOverDest(MTS_TASK_TCB tcb)
    {
    }

    private static void gmOverMain(MTS_TASK_TCB tcb)
    {
        GMS_OVER_MGR_WORK work = (GMS_OVER_MGR_WORK)tcb.work;
        if (work.proc_update != null)
            work.proc_update(work);
        if (work.proc_disp == null)
            return;
        work.proc_disp(work);
    }

    private static void gmOverProcUpdateGOInit(GMS_OVER_MGR_WORK mgr_work)
    {
        mgr_work.wait_timer = 30U;
        mgr_work.proc_update = new _GMS_OVER_MGR_WORK_UD_(gmOverProcUpdateGOWaitStart);
    }

    private static void gmOverProcUpdateGOWaitStart(GMS_OVER_MGR_WORK mgr_work)
    {
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            gmOverSetActionPlay(mgr_work.string_sub_parts[0]);
            gmOverSetActionPlay(mgr_work.string_sub_parts[1]);
            mgr_work.wait_timer = 480U;
            mgr_work.proc_update = new _GMS_OVER_MGR_WORK_UD_(gmOverProcUpdateGOLoop);
        }
    }

    private static void gmOverProcUpdateGOLoop(GMS_OVER_MGR_WORK mgr_work)
    {
        if (gmOverIsSkipKeyOn())
            mgr_work.wait_timer = 0U;
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            gmOverSetActionPlay(mgr_work.fadeout_sub_parts[0]);
            mgr_work.proc_update = new _GMS_OVER_MGR_WORK_UD_(gmOverProcUpdateGOWaitFadeEnd);
        }
    }

    private static void gmOverProcUpdateGOWaitFadeEnd(GMS_OVER_MGR_WORK mgr_work)
    {
        if (((int)((OBS_OBJECT_WORK)mgr_work.fadeout_sub_parts[0]).disp_flag & 8) == 0)
            return;
        IzFadeInitEasy(0U, 1U, 1f);
        mgr_work.proc_update = new _GMS_OVER_MGR_WORK_UD_(gmOverProcUpdateGOWaitFinalizeFade);
    }

    private static void gmOverProcUpdateGOWaitFinalizeFade(GMS_OVER_MGR_WORK mgr_work)
    {
        if (!IzFadeIsEnd())
            return;
        g_gm_main_system.game_flag |= 256U;
        mgr_work.proc_update = null;
    }

    private static void gmOverProcUpdateTOInit(GMS_OVER_MGR_WORK mgr_work)
    {
        mgr_work.wait_timer = 30U;
        mgr_work.proc_update = new _GMS_OVER_MGR_WORK_UD_(gmOverProcUpdateTOWaitStart);
    }

    private static void gmOverProcUpdateTOWaitStart(GMS_OVER_MGR_WORK mgr_work)
    {
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            gmOverSetActionPlay(mgr_work.string_sub_parts[2]);
            gmOverSetActionPlay(mgr_work.string_sub_parts[3]);
            gmOverSetActionPlay(mgr_work.fadeout_sub_parts[1]);
            mgr_work.proc_update = new _GMS_OVER_MGR_WORK_UD_(gmOverProcUpdateTOWaitFadeEnd);
        }
    }

    private static void gmOverProcUpdateTOWaitFadeEnd(GMS_OVER_MGR_WORK mgr_work)
    {
        if (((int)((OBS_OBJECT_WORK)mgr_work.fadeout_sub_parts[1]).disp_flag & 8) == 0)
            return;
        IzFadeInitEasy(0U, 1U, 1f);
        mgr_work.proc_update = new _GMS_OVER_MGR_WORK_UD_(gmOverProcUpdateTOWaitFinalizeFade);
    }

    private static void gmOverProcUpdateTOWaitFinalizeFade(GMS_OVER_MGR_WORK mgr_work)
    {
        if (!IzFadeIsEnd())
            return;
        g_gm_main_system.game_flag |= 256U;
        mgr_work.proc_update = null;
    }

    private static void gmOverProcDispLoop(GMS_OVER_MGR_WORK mgr_work)
    {
    }

}