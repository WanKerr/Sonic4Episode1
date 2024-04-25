public partial class AppMain
{
    private static void DmLoadingBuild(AMS_FS arc_amb)
    {
        dm_loading_mgr_p = new DMS_LOADING_MGR();
        dm_loading_tex = new AOS_TEXTURE[1];
        for (int index = 0; index < 1; ++index)
            dm_loading_tex[index] = new AOS_TEXTURE();
        for (int index = 0; index < 1; ++index)
        {
            string sPath = null;
            dm_loading_ama[index] = readAMAFile(amBindGet(arc_amb, 0, out sPath));
            sPath = null;
            dm_loading_amb[index] = readAMBFile(amBindGet(arc_amb, 1, out sPath));
            dm_loading_amb[index].dir = sPath;
        }
        for (int index = 0; index < 1; ++index)
        {
            AoTexBuild(dm_loading_tex[index], dm_loading_amb[index]);
            AoTexLoad(dm_loading_tex[index]);
        }
    }

    private static bool DmLoadingBuildCheck()
    {
        return dmLoadingIsTexLoad() != 0;
    }

    private static void DmLoadingFlush()
    {
        for (int index = 0; index < 1; ++index)
            AoTexRelease(dm_loading_tex[index]);
    }

    private static bool DmLoadingFlushCheck()
    {
        if (dmLoadingIsTexRelease() == 0)
            return false;
        if (dm_loading_mgr_p != null)
            dm_loading_mgr_p = null;
        return true;
    }

    private static void DmLoadingStart()
    {
        dmLoadingInit();
    }

    private static bool DmLoadingIsExit()
    {
        return dm_loading_mgr_p == null || dm_loading_mgr_p.tcb == null;
    }

    private static void DmLoadingExit()
    {
        if (dm_loading_mgr_p.tcb == null)
            return;
        mtTaskClearTcb(dm_loading_mgr_p.tcb);
        dm_loading_mgr_p.tcb = null;
    }

    private static void DmLoadingSetLoadComplete()
    {
        dm_loading_check_load_comp = true;
    }

    private static void dmLoadingInit()
    {
        dm_loading_mgr_p.tcb = mtTaskMake(new GSF_TASK_PROCEDURE(dmLoadingProcMain), new GSF_TASK_PROCEDURE(dmLoadingDest), 0U, (ushort)short.MaxValue, 8192U, 10, () => new DMS_LOADING_MAIN_WORK(), "LOADING_MAIN");
        DMS_LOADING_MAIN_WORK work = (DMS_LOADING_MAIN_WORK)dm_loading_mgr_p.tcb.work;
        work.draw_state = AoActSysGetDrawStateEnable() ? 1U : 0U;
        AoActSysSetDrawStateEnable(work.draw_state == 1U);
        if (work.draw_state != 0U)
            dm_loading_draw_state = AoActSysGetDrawState();
        dmLoadingSetInitData(work);
        work.proc_update = new DMS_LOADING_MAIN_WORK._proc_update_(dmLoadingProcInit);
    }

    private static void dmLoadingSetInitData(DMS_LOADING_MAIN_WORK main_work)
    {
        dm_loading_check_load_comp = false;
        switch (SyGetEvtInfo().cur_evt_id)
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
        main_work.lang_id = GsEnvGetLanguage();
    }

    private static void dmLoadingProcMain(MTS_TASK_TCB tcb)
    {
        DMS_LOADING_MAIN_WORK work = (DMS_LOADING_MAIN_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
            DmLoadingExit();
        if (work.proc_update != null)
            work.proc_update(work);
        if (work.proc_draw == null)
            return;
        work.proc_draw(work);
    }

    private static void dmLoadingDest(MTS_TASK_TCB tcb)
    {
    }

    private static void dmLoadingProcInit(DMS_LOADING_MAIN_WORK main_work)
    {
        main_work.proc_update = new DMS_LOADING_MAIN_WORK._proc_update_(dmLoadingProcCreateAct);
    }

    private static void dmLoadingProcCreateAct(DMS_LOADING_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 8U; ++index)
        {
            A2S_AMA_HEADER ama = dm_loading_ama[0];
            AoActSetTexture(AoTexGetTexList(dm_loading_tex[0]));
            main_work.act[(int)index] = AoActCreate(ama, g_dm_act_id_tbl_loading[(int)index]);
        }
        main_work.proc_update = new DMS_LOADING_MAIN_WORK._proc_update_(dmLoadingProcFadeIn);
        main_work.proc_draw = new DMS_LOADING_MAIN_WORK._proc_draw_(dmLoadingProcActDraw);
        if (main_work.is_maingame_load)
            IzFadeInitEasy(0U, 0U, 32f);
        else
            IzFadeInitEasy(0U, 0U, 32f);
    }

    private static void dmLoadingProcFadeIn(DMS_LOADING_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        main_work.proc_update = new DMS_LOADING_MAIN_WORK._proc_update_(dmLoadingProcNowLoading);
    }

    private static void dmLoadingProcNowLoading(DMS_LOADING_MAIN_WORK main_work)
    {
        if (dm_loading_check_load_comp && main_work.timer > 60.0)
        {
            main_work.proc_update = new DMS_LOADING_MAIN_WORK._proc_update_(dmLoadingProcAlreadyLoaded);
            main_work.timer = 0.0f;
            if (main_work.is_maingame_load)
                IzFadeInitEasyColor(0, (ushort)short.MaxValue, IZD_FADE_DT_PRIO_DEF, 18U, 0U, 3U, 32f, true);
            else
                IzFadeInitEasy(0U, 3U, 32f);
        }
        if (main_work.sonic_set_frame >= 12.0)
            main_work.sonic_set_frame = 0.0f;
        ++main_work.sonic_set_frame;
        ++main_work.timer;
    }

    private static void dmLoadingProcAlreadyLoaded(DMS_LOADING_MAIN_WORK main_work)
    {
        if (main_work.timer > 32.0)
        {
            main_work.proc_update = new DMS_LOADING_MAIN_WORK._proc_update_(dmLoadingProcFadeOut);
            main_work.timer = 0.0f;
        }
        if (main_work.sonic_set_frame >= 12.0)
            main_work.sonic_set_frame = 0.0f;
        main_work.sonic_pos_x += 50f;
        ++main_work.sonic_set_frame;
        ++main_work.timer;
    }

    private static void dmLoadingProcFadeOut(DMS_LOADING_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        main_work.proc_update = new DMS_LOADING_MAIN_WORK._proc_update_(dmLoadingProcStopDraw);
        main_work.proc_draw = null;
    }

    private static void dmLoadingProcStopDraw(DMS_LOADING_MAIN_WORK main_work)
    {
        for (int index = 0; index < 8; ++index)
        {
            if (main_work.act[index] != null)
            {
                AoActDelete(main_work.act[index]);
                main_work.act[index] = null;
            }
        }
        main_work.proc_update = null;
        main_work.flag |= 1U;
    }

    private static void dmLoadingProcActDraw(DMS_LOADING_MAIN_WORK main_work)
    {
        dmLoadingCommonDraw(main_work);
        if (main_work.draw_state == 0U)
            return;
        amDrawMakeTask(new TaskProc(dmLoadingTaskDraw), 32768, null);
    }

    private static void dmLoadingTaskDraw(AMS_TCB tcb)
    {
        AoActDrawPre();
        amDrawExecCommand(dm_loading_draw_state);
        amDrawEndScene();
    }

    private static void dmLoadingCommonDraw(DMS_LOADING_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(8192U);
        AoActSetTexture(AoTexGetTexList(dm_loading_tex[0]));
        for (int index = 0; index < 8; ++index)
            AoActSortRegAction(main_work.act[index]);
        AoActSetFrame(main_work.act[3], main_work.sonic_set_frame);
        AoActSetFrame(main_work.act[7], main_work.lang_id);
        AoActSetTexture(AoTexGetTexList(dm_loading_tex[0]));
        for (int index = 0; index <= 2; ++index)
            AoActUpdate(main_work.act[index], 0.0f);
        AoActAcmPush();
        AoActAcmInit();
        AoActAcmApplyTrans(main_work.sonic_pos_x, -10f, 0.0f);
        AoActUpdate(main_work.act[3], 0.0f);
        AoActAcmPop();
        for (int index = 4; index <= 6; ++index)
            AoActUpdate(main_work.act[index], 1f);
        AoActUpdate(main_work.act[7], 0.0f);
        if (main_work.lang_id >= 6)
            main_work.act[7].sprite.tex_id = 9 + main_work.lang_id - 6;
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private static short dmLoadingIsTexLoad()
    {
        for (int index = 0; index < 1; ++index)
        {
            if (!AoTexIsLoaded(dm_loading_tex[index]))
                return 0;
        }
        return 1;
    }

    private static short dmLoadingIsTexRelease()
    {
        for (int index = 0; index < 1; ++index)
        {
            if (!AoTexIsReleased(dm_loading_tex[index]))
                return 0;
        }
        return 1;
    }

}