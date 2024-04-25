using er;
using gs.backup;

public partial class AppMain
{
    private static bool GmClearDemoBuildCheck()
    {
        return gmClearDemoIsTexLoad();
    }

    private static void GmClearDemoBuild()
    {
        int language = GsEnvGetLanguage();
        int num = language < 1 ? 0 : 2 * language;
        gm_clrdm_mgr.Clear();
        gm_clrdm_mgr_p = gm_clrdm_mgr;
        for (int index = 0; index < 2; ++index)
            gm_clrdm_tex[index].Clear();
        gm_clrdm_amb[0] = readAMBFile(ObjDataLoadAmbIndex(null, 31, g_gm_gamedat_cockpit_main_arc));
        gm_clrdm_amb[1] = readAMBFile(ObjDataLoadAmbIndex(null, g_gm_clear_demo_data_amb_id[GsEnvGetLanguage()], g_gm_gamedat_cockpit_main_arc));
        for (int index = 0; index < 2; ++index)
        {
            AoTexBuild(gm_clrdm_tex[index], gm_clrdm_amb[index]);
            AoTexLoad(gm_clrdm_tex[index]);
        }
    }

    private static void GmClearDemoFlush()
    {
        for (int index = 0; index < 2; ++index)
            AoTexRelease(gm_clrdm_tex[index]);
    }

    private static bool GmClearDemoFlushCheck()
    {
        return gmClearDemoIsTexRelease();
    }

    private static void GmClearDemoStart()
    {
        gmClearDemoInit();
    }

    private static void GmClearDemoExit()
    {
        if (gm_clrdm_mgr_p.tcb == null)
            return;
        mtTaskClearTcb(gm_clrdm_mgr_p.tcb);
        gm_clrdm_mgr_p.tcb = null;
    }

    private static bool GmClearDemoIsExit()
    {
        return gm_clrdm_mgr_p.tcb == null;
    }

    private static void GmClearDemoRetryStart()
    {
        gmClearDemoRetryInit();
    }

    private static void gmClearDemoInit()
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        MTS_TASK_TCB mtsTaskTcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmClearDemoProcMain), new GSF_TASK_PROCEDURE(gmClearDemoDest), 0U, 0, 18448U, 5, () => new GMS_CLRDM_MAIN_WORK(), "CLRDM_MAIN");
        gm_clrdm_mgr_p.tcb = mtsTaskTcb;
        GMS_CLRDM_MAIN_WORK work = (GMS_CLRDM_MAIN_WORK) mtsTaskTcb.work;
        for (int index = 0; index < 2; ++index)
            work.tex[index] = gm_clrdm_tex[index];
        work.trg_retry.Constructor();
        work.trg_back.Constructor();
        work.count = 1;
        work.stage_id = mainSysInfo.stage_id;
        work.game_mode = mainSysInfo.game_mode;
        if (work.stage_id <= 20)
        {
            work.is_clear_spe_stg = false;
            gmClearDemoSetPlayGameScore(work);
        }
        else
        {
            work.is_clear_spe_stg = true;
            gmClearDemoSetSpecialStageScore(work);
            gmClearDemoSetSpecialStageClearInfo(work);
        }

        gmClearDemoSetSaveScoreData(work);
        if (work.stage_id >= 21 && work.stage_id < 29)
        {
            if (work.game_mode == 0)
            {
                gmClearDemoCreateObjSpeScore(work);
                gmClearDemoCreateObjSpecialScoreAtk(work);
                gmClearDemoSetInitDispAct(work);
            }
            else
            {
                gmClearDemoCreateObjSpeTime(work);
                gmClearDemoCreateObjSpecialTimeAtk(work);
            }
        }
        else if (work.game_mode == 0)
        {
            gmClearDemoCreateObjActScore(work);
            gmClearDemoCreateObjAct(work);
            gmClearDemoSetInitDispAct(work);
        }
        else
        {
            gmClearDemoCreateObjActTime(work);
            gmClearDemoCreateObjNormalTimeAtk(work);
        }

        if (mainSysInfo.stage_id >= 16 && mainSysInfo.stage_id <= 20)
            GmSoundPlayClearFinal();
        else
            GmSoundPlayClear();
        gmClearDemoSetMainUpdateProc(work);
    }

    private static void gmClearDemoRetryInit()
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        MTS_TASK_TCB mtsTaskTcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmClearDemoProcMain), new GSF_TASK_PROCEDURE(gmClearDemoDest), 0U, 0, 18448U, 5, () => new GMS_CLRDM_MAIN_WORK(), "CLRDM_MAIN");
        gm_clrdm_mgr_p.tcb = mtsTaskTcb;
        GMS_CLRDM_MAIN_WORK work = (GMS_CLRDM_MAIN_WORK) mtsTaskTcb.work;
        for (int index = 0; index < 2; ++index)
            work.tex[index] = gm_clrdm_tex[index];
        work.trg_retry.Constructor();
        work.trg_back.Constructor();
        work.count = 1;
        work.stage_id = mainSysInfo.stage_id;
        work.is_clear_spe_stg = work.stage_id >= 21;
        work.game_mode = mainSysInfo.game_mode;
        gmClearDemoCreateObjActTime(work);
        gmClearDemoCreateObjNormalTimeAtk(work);
        gmClearDemoSetRetryInitAct(work);
        work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcRetryStart);
    }

    private static void gmClearDemoSetMainUpdateProc(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.stage_id >= 21)
        {
            if (main_work.game_mode == 0)
            {
                main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeScoreMoveEfct);
            }
            else
            {
                main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeTimeMoveEfct);
                gmClearDemoSetClearTimeRecord(main_work);
                gmClearDemoSetSortBufTimeAct(main_work);
                main_work.tex_new_record_act.obj_2d.speed = 0.0f;
            }
        }
        else if (main_work.game_mode == 0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcMoveEfct);
        }
        else
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcTimeMoveEfct);
            gmClearDemoSetClearTimeRecord(main_work);
            gmClearDemoSetSortBufTimeAct(main_work);
            main_work.tex_new_record_act.obj_2d.speed = 0.0f;
        }
    }

    private static void gmClearDemoSetPlayGameScore(GMS_CLRDM_MAIN_WORK main_work)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        mainSysInfo.clear_score = gmClearDemoGetScore();
        mainSysInfo.clear_time = (int) gmClearDemoGetGameTime();
        mainSysInfo.clear_ring = (uint) gmClearDemoGetRingNum();
        main_work.time_score[0] = 0U;
        main_work.time_score[1] = gmClearDemoGetTimeScore(mainSysInfo.clear_time);
        main_work.ring_score[0] = 0U;
        main_work.ring_score[1] = mainSysInfo.clear_ring * 100U;
        if (main_work.ring_score[1] > 99900U)
            main_work.ring_score[1] = 99900U;
        main_work.total_score[1] = mainSysInfo.clear_score;
        main_work.total_score[0] = main_work.total_score[1] + main_work.ring_score[1] + main_work.time_score[1];
        if (main_work.total_score[0] <= 999999990U)
            return;
        main_work.total_score[0] = 999999990U;
    }

    private static void gmClearDemoSetSaveScoreData(GMS_CLRDM_MAIN_WORK main_work)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        uint high_score = main_work.total_score[0] < 10U ? 0U : main_work.total_score[0];
        SSystem instance1 = SSystem.CreateInstance();
        SStage stage = SStage.CreateInstance();
        SSpecial special = SSpecial.CreateInstance();
        if (main_work.stage_id >= 21)
        {
            uint num = main_work.stage_id - 21U;
            GMM_MAIN_GOAL_AS_SUPER_SONIC();
            if (mainSysInfo.game_mode == 0)
            {
                if (((int) g_gm_main_system.game_flag & 65536) != 0 && !main_work.is_first_spe_clear)
                    GmPlayerStockGet(g_gm_main_system.ply_work[0], 1);
                bool is_uploaded = LiveFeature.getInstance().saveLeaderBoardScore(main_work.stage_id - 4, (int) high_score);
                special[(int) num].SetHighScoreUploaded(is_uploaded);
                if (is_uploaded && !special[(int) num].IsScoreUploadedOnce())
                    special[(int) num].SetScoreUploadedOnce(true);
            }
            else if (mainSysInfo.game_mode == 1)
            {
                if (((int) g_gm_main_system.game_flag & 65536) != 0 && !main_work.is_first_spe_clear)
                    GmPlayerStockGet(g_gm_main_system.ply_work[0], 1);
                bool is_uploaded = false;
                if (((int) g_gm_main_system.game_flag & 65536) != 0)
                {
                    is_uploaded = LiveFeature.getInstance().saveLeaderBoardScore(main_work.stage_id - 4 + 24, mainSysInfo.clear_time);
                    special[(int) num].SetFastTimeUploaded(is_uploaded);
                }

                if (is_uploaded && !special[(int) num].IsTimeUploadedOnce())
                    special[(int) num].SetTimeUploadedOnce(true);
            }

            if ((special[(int) num].GetHighScore() < high_score || special[(int) num].GetHighScore() == 0) && ((int) g_gm_main_system.game_flag & 65536) != 0)
            {
                special[(int) num].SetHighScore(high_score);
                main_work.flag |= 32768U;
            }

            if ((special[(int) num].GetFastTime() < (uint) mainSysInfo.clear_time || special[(int) num].GetFastTime() == 0) && ((int) g_gm_main_system.game_flag & 65536) != 0)
            {
                special[(int) num].SetFastTime((uint) mainSysInfo.clear_time);
                main_work.flag |= 4096U;
            }
        }
        else
        {
            uint num = main_work.stage_id;
            if (num > 16U && num <= 20U)
                num = 16U;
            bool flag = GMM_MAIN_USE_SUPER_SONIC();

            if (GMM_MAIN_GOAL_AS_SUPER_SONIC())
                stage[(int) num].SetUseSuperSonicOnce(true);
            if (mainSysInfo.game_mode == 0)
            {
                high_score = main_work.total_score[0] < 10U ? 0U : main_work.total_score[0];
                g_gs_main_sys_info.is_first_play = stage[(int) num].GetHighScore(false) == 0;
                bool is_uploaded = LiveFeature.getInstance().saveLeaderBoardScore((int) num, (int) high_score);
                stage[(int) num].SetHighScoreUploaded(flag, is_uploaded);
                if (is_uploaded && !stage[(int) num].IsScoreUploadedOnce())
                    stage[(int) num].SetScoreUploadedOnce(true);
                g_gs_main_sys_info.prev_stage_id = ((int) g_gm_main_system.game_flag & 16384) == 0 ? ushort.MaxValue : g_gs_main_sys_info.stage_id;
            }
            else if (mainSysInfo.game_mode == 1 && (stage[(int) num].GetFastTime(flag) > (uint) mainSysInfo.clear_time || stage[(int) num].GetFastTime(flag) == 0))
            {
                bool is_uploaded = LiveFeature.getInstance().saveLeaderBoardScore((int) num + 24, mainSysInfo.clear_time);
                stage[(int) num].SetFastTimeUploaded(flag, is_uploaded);
                if (is_uploaded && !stage[(int) num].IsTimeUploadedOnce())
                    stage[(int) num].SetTimeUploadedOnce(true);
            }

            if (flag)
            {
                if (stage[(int) num].GetFastTime(false) > (uint) mainSysInfo.clear_time || stage[(int) num].GetFastTime(false) == 0)
                    main_work.flag |= 4096U;
            }
            else if (stage[(int) num].GetFastTime(true) > (uint) mainSysInfo.clear_time || stage[(int) num].GetFastTime(true) == 0)
                main_work.flag |= 4096U;

            stage[(int) num].SetFastTime((uint) mainSysInfo.clear_time, flag);

            if (stage[(int) num].GetHighScore(flag) < high_score || stage[(int) num].GetHighScore(flag) == 0)
                stage[(int) num].SetHighScore(high_score, flag);
        }


        instance1.Killed = mainSysInfo.ene_kill_count;
        instance1.PlayerStock = mainSysInfo.rest_player_num;
        if (mainSysInfo.stage_id >= 16 && mainSysInfo.stage_id <= 20)
        {
            HgTrophyIncFinalClearCount();
            instance1.ClearCount = mainSysInfo.final_clear_count;
        }

        HgTrophyTryAcquisition(4);
    }

    private static void gmClearDemoSetClearTimeRecord(GMS_CLRDM_MAIN_WORK main_work)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        main_work.clear_time = (uint) mainSysInfo.clear_time;
        AkUtilFrame60ToTime(main_work.clear_time, ref main_work.time_min, ref main_work.time_sec, ref main_work.time_msec);
        main_work.record_time_num_act[0].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[2].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[3].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[5].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[6].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[1].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[4].obj_2d.speed = 0.0f;
        for (int index = 0; index < 7; ++index)
            ((OBS_OBJECT_WORK) main_work.record_time_num_act[index]).disp_flag &= 4294967263U;
    }

    private static void gmClearDemoSetSpecialStageScore(GMS_CLRDM_MAIN_WORK main_work)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        mainSysInfo.clear_score = gmClearDemoGetScore();
        mainSysInfo.clear_time = (int) gmClearDemoGetGameTime();
        mainSysInfo.clear_ring = (uint) gmClearDemoGetRingNum();
        main_work.time_score[0] = 0U;
        main_work.time_score[1] = gmClearDemoGetTimeSpeScore(mainSysInfo.clear_time);
        main_work.ring_score[0] = 0U;
        main_work.ring_score[1] = mainSysInfo.clear_ring * 100U;
        if (main_work.ring_score[1] > 99900U)
            main_work.ring_score[1] = 99900U;
        main_work.total_score[1] = mainSysInfo.clear_score;
        main_work.total_score[0] = main_work.total_score[1] + main_work.ring_score[1] + main_work.time_score[1];
        if (main_work.total_score[0] <= 999999990U)
            return;
        main_work.total_score[0] = 999999990U;
    }

    private static void gmClearDemoSetSpecialStageClearInfo(GMS_CLRDM_MAIN_WORK main_work)
    {
        ushort num1 = g_gs_main_sys_info.prev_stage_id == ushort.MaxValue ? ushort.MaxValue : g_gs_main_sys_info.prev_stage_id;
        SSpecial instance = SSpecial.CreateInstance();
        uint num2 = main_work.stage_id - 21U;
        if (((int) g_gs_main_sys_info.game_flag & 32) != 0)
            main_work.is_full_eme = true;
        if (((int) g_gm_main_system.game_flag & 65536) != 0)
        {
            main_work.is_get_eme = true;
            if (!instance[(int) num2].IsGetEmerald() && num1 != ushort.MaxValue)
            {
                instance[(int) num2].SetEmeraldStage((EEmeraldStage.Type) dm_clrdm_spe_stg_get_act_no[num1]);
                main_work.is_first_spe_clear = true;
            }

            main_work.get_eme_no = (int) num2;
            if (g_gs_main_sys_info.stage_id == 27)
                g_gs_main_sys_info.game_flag |= 32U;
            main_work.has_eme_num = (int) num2 + 1;
            for (int index = 7; index > 0; --index)
            {
                if (instance[index - 1].IsGetEmerald())
                {
                    main_work.has_eme_num = index;
                    break;
                }
            }
        }
        else
        {
            main_work.is_get_eme = false;
            main_work.get_eme_no = -1;
            for (int index = 7; index > 0; --index)
            {
                if (instance[index - 1].IsGetEmerald())
                {
                    main_work.has_eme_num = index;
                    break;
                }
            }
        }
    }

    private static uint gmClearDemoGetTimeScore(int clear_time)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        uint num1 = 0;
        ushort min = 0;
        ushort sec = 0;
        ushort msec = 0;
        AkUtilFrame60ToTime((uint) clear_time, ref min, ref sec, ref msec);
        int num2 = min * 60 + sec;
        for (int index = 0; index < 9; ++index)
        {
            if (num2 < dm_clrdm_clear_time_sec_tbl[mainSysInfo.stage_id][index])
            {
                num1 = dm_clrdm_clear_time_score_tbl[index];
                break;
            }
        }

        if (((int) g_gs_main_sys_info.game_flag & 8) != 0)
            num1 = 0U;
        return num1;
    }

    private static uint gmClearDemoGetTimeSpeScore(int clear_time)
    {
        ushort min = 0;
        ushort sec = 0;
        ushort msec = 0;
        AkUtilFrame60ToTime((uint) clear_time, ref min, ref sec, ref msec);
        uint num = (uint) ((min * 60 + sec) * 100);
        if (((int) g_gm_main_system.game_flag & 131072) != 0)
            num = 0U;
        if (((int) g_gm_main_system.game_flag & 262144) != 0)
            num = 0U;
        return num;
    }

    private static void gmClearDemoSetInitDispAct(GMS_CLRDM_MAIN_WORK main_work)
    {
        for (int index = 0; index < 5; ++index)
        {
            ((OBS_OBJECT_WORK) main_work.time_num_act[index]).disp_flag |= 32U;
            ((OBS_OBJECT_WORK) main_work.ring_num_act[index]).disp_flag |= 32U;
        }

        for (int index = 0; index < 9; ++index)
            ((OBS_OBJECT_WORK) main_work.total_num_act[index]).disp_flag |= 32U;
        for (int index = 0; index < 3; ++index)
            ((OBS_OBJECT_WORK) main_work.line_act[index]).disp_flag |= 32U;
        ((OBS_OBJECT_WORK) main_work.sonic_icon_act).disp_flag |= 32U;
        ((OBS_OBJECT_WORK) main_work.sonic_icon_act2).disp_flag |= 32U;
    }

    private static void gmClearDemoProcMain(MTS_TASK_TCB tcb)
    {
        GMS_CLRDM_MAIN_WORK work = (GMS_CLRDM_MAIN_WORK) tcb.work;
        if (((int) work.flag & 1) != 0)
        {
            if (gm_clrdm_mgr_p.tcb != null)
            {
                mtTaskClearTcb(gm_clrdm_mgr_p.tcb);
                gm_clrdm_mgr_p.tcb = null;
            }

            if (work.stage_id < 21)
            {
                if (((int) g_gm_main_system.game_flag & 16384) != 0)
                {
                    g_gs_main_sys_info.prev_stage_id = g_gs_main_sys_info.stage_id;
                    DmCmnBackupSave(false, false, false);
                }
                else
                    g_gs_main_sys_info.prev_stage_id = ushort.MaxValue;
            }

            if (((int) work.flag & 16384) != 0)
                work.flag &= 4294950911U;
            g_gm_main_system.game_flag |= 16U;
            if (((int) work.flag & 8192) == 0)
                return;
            g_gm_main_system.game_flag |= 2U;
            work.flag &= 4294959103U;
        }
        else
        {
            if (work.proc_update == null)
                return;
            work.proc_update(work);
        }
    }

    private static void gmClearDemoDest(MTS_TASK_TCB tcb)
    {
    }

    private static void gmClearDemoProcMoveEfct(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AoActIsEndTrs(main_work.tex_total_act.obj_2d.act))
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcPrevCalcScore);
            amFlagOn(ref main_work.flag, 32U);
            amFlagOn(ref main_work.flag, 512U);
        }

        if (AoActIsEndTrs(main_work.tex_time_act.obj_2d.act))
            amFlagOn(ref main_work.flag, 128U);
        if (AoActIsEndTrs(main_work.tex_ring_act.obj_2d.act))
            amFlagOn(ref main_work.flag, 256U);
        gmClearDemoSetSortBufAct(main_work);
        gmClearDemoSetScoreData(main_work);
        gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcPrevCalcScore(GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += main_work.count;
        if (main_work.timer > 150.0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcCalcScore);
            main_work.proc_calc_score = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoSetCalcScore);
            main_work.timer = 0.0f;
        }

        if (amTpIsTouchPush(0) || isBackKeyPressed())
        {
            setBackKeyRequest(false);
            amFlagOn(ref main_work.flag, 4U);
        }

        gmClearDemoSetSortBufAct(main_work);
        gmClearDemoSetScoreData(main_work);
        gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcCalcScore(GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += main_work.count;
        if (((int) main_work.flag & 4) != 0 || ((int) main_work.flag & 8) != 0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcWaitDispSonic);
            main_work.time_score[1] = main_work.time_score[0];
            main_work.ring_score[1] = main_work.ring_score[0];
            main_work.total_score[1] = main_work.total_score[0];
            main_work.proc_calc_score = null;
            amFlagOff(ref main_work.flag, 4U);
            amFlagOff(ref main_work.flag, 8U);
            if (main_work.total_score[1] >= 10000U)
            {
                amFlagOn(ref main_work.flag, 16U);
                main_work.idle_time = 270;
            }
            else
                main_work.idle_time = 120;

            HgTrophyTryAcquisition(0);
            main_work.timer = 0.0f;
        }

        if (amTpIsTouchPush(0) || isBackKeyPressed() || AoPad.AoPadADirect() != 0)
        {
            setBackKeyRequest(false);
            amFlagOn(ref main_work.flag, 4U);
        }

        if (main_work.proc_calc_score != null && main_work.timer > 1.0)
        {
            main_work.proc_calc_score(main_work);
            main_work.timer = 0.0f;
        }

        gmClearDemoSetSortBufAct(main_work);
        gmClearDemoSetScoreData(main_work);
        gmClearDemoUpdateAct(main_work);
        if (main_work.proc_calc_score == null)
        {
            GmSoundPlaySE("Result2");
        }
        else
        {
            if (0.0 != main_work.timer)
                return;
            GmSoundPlaySE("Result1");
        }
    }

    private static void gmClearDemoProcWaitDispSonic(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.timer >= 60.0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcDispIdle);
            main_work.timer = 0.0f;
            if (((int) main_work.flag & 16) == 0)
                return;
            GmSoundPlayJingle(0U);
            GmPlayerStockGet(g_gm_main_system.ply_work[0], 1);
        }
        else
        {
            gmClearDemoSetSortBufAct(main_work);
            gmClearDemoSetScoreData(main_work);
            gmClearDemoUpdateAct(main_work);
            main_work.timer += main_work.count;
        }
    }

    private static void gmClearDemoProcDispIdle(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.timer >= (double) main_work.idle_time)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcFadeOut);
            IzFadeInitEasy(0U, 1U, 32f);
        }

        if (((int) main_work.flag & 16) != 0)
            gmClearDemoSetFlashSonic(main_work);
        gmClearDemoSetSortBufAct(main_work);
        gmClearDemoSetScoreData(main_work);
        gmClearDemoUpdateAct(main_work);
        main_work.timer += main_work.count;
    }

    private static void gmClearDemoProcFadeOut(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (IzFadeIsEnd())
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcFinish);
            main_work.timer = 0.0f;
        }
        else
        {
            if (((int) main_work.flag & 16) != 0)
                gmClearDemoSetFlashSonic(main_work);
            if (main_work.game_mode == 0)
            {
                gmClearDemoSetSortBufAct(main_work);
                gmClearDemoSetScoreData(main_work);
                gmClearDemoUpdateAct(main_work);
            }

            main_work.timer += main_work.count;
        }
    }

    private static void gmClearDemoProcFinish(GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.proc_update = null;
        main_work.trg_retry.Release();
        main_work.trg_retry.Destructor();
        main_work.trg_back.Release();
        main_work.trg_back.Destructor();
        amFlagOn(ref main_work.flag, 1U);
    }

    private static void gmClearDemoProcTimeMoveEfct(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AoActIsEndTrs(main_work.tex_big_time_act.obj_2d.act))
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcTimeWaitTimeEfct);
            for (int index = 0; index < 7; ++index)
            {
                ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.record_time_num_act[index], main_work.record_time_num_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (49 + index), 0);
                ((OBS_OBJECT_WORK) main_work.record_time_num_act[index]).scale.x = 4096;
                ((OBS_OBJECT_WORK) main_work.record_time_num_act[index]).scale.y = 4096;
            }

            gmClearDemoSetClearTimeRecord(main_work);
            gmClearDemoSetSortBufTimeAct(main_work);
            amFlagOn(ref main_work.flag, 1024U);
            amFlagOn(ref main_work.flag, 2048U);
        }

        gmClearDemoSetTimeAtkSortBufAct(main_work);
        gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcTimeWaitTimeEfct(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.timer > 60.0)
        {
            if (((int) main_work.flag & 4096) != 0)
            {
                main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcTimeMoveNewRecord);
                main_work.tex_new_record_act.obj_2d.speed = 1f;
                main_work.idle_time = 180;
            }
            else
            {
                main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcTimeDispEffect);
                main_work.idle_time = 120;
            }

            HgTrophyTryAcquisition(0);
            main_work.timer = 0.0f;
        }

        gmClearDemoSetTimeAtkSortBufAct(main_work);
        gmClearDemoUpdateAct(main_work);
        if (main_work.timer > 120.0)
            return;
        main_work.timer += main_work.count;
    }

    private static void gmClearDemoProcTimeMoveNewRecord(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AoActIsEndTrs(main_work.tex_new_record_act.obj_2d.act))
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcTimeDispEffect);
            GmSoundPlayJingle(5U);
        }

        gmClearDemoSetTimeAtkSortBufAct(main_work);
        gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcTimeDispEffect(GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += main_work.count;
        if (amTpIsTouchPush(0))
            main_work.flag |= 4U;
        if (main_work.timer > (double) main_work.idle_time || ((int) main_work.flag & 4) != 0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcChangeRetryOut);
            IzFadeInitEasy(0U, 1U, 32f);
            main_work.timer = 0.0f;
            main_work.flag &= 4294967291U;
        }

        gmClearDemoTimeFlushEffect(main_work);
        gmClearDemoSetTimeAtkSortBufAct(main_work);
    }

    private static void gmClearDemoProcRetryStart(GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcChangeRetryOut);
        IzFadeInitEasy(0U, 1U, 32f);
        main_work.timer = 0.0f;
    }

    private static void gmClearDemoProcChangeRetryOut(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (IzFadeIsEnd())
        {
            if (main_work.nodisp_check)
            {
                IzFadeInitEasy(0U, 0U, 32f);
                main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcChangeRetryIn);
                main_work.nodisp_check = false;
                return;
            }

            gmClearDemoSetRetryDispInfo(main_work);
            gmClearDemoSetRetrySortBufAct(main_work);
            main_work.nodisp_check = true;
        }

        gmClearDemoSetTimeAtkSortBufAct(main_work);
    }

    private static void gmClearDemoProcChangeRetryIn(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcWaitSelectRetry);
    }

    private static void gmClearDemoProcWaitSelectRetry(GMS_CLRDM_MAIN_WORK main_work)
    {
        gmClearDemoSetBgColorBlack();
        gmClearDemoSetRetryInput(main_work);
        if (((int) main_work.flag & 4) != 0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcWaitRetrySonicRunEfct);
            GmPlySeqChangeTRetryAcc(g_gm_main_system.ply_work[0]);
        }
        else
        {
            if (((int) main_work.flag & 2) == 0)
                return;
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcFadeOut);
            IzFadeInitEasy(0U, 1U, 32f);
        }
    }

    private static void gmClearDemoProcWaitRetrySonicRunEfct(GMS_CLRDM_MAIN_WORK main_work)
    {
        gmClearDemoSetBgColorBlack();
        if (ObjObjectViewOutCheck(g_gm_main_system.ply_work[0].obj_work) == 0)
            return;
        main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcFadeOut);
        main_work.flag |= 8192U;
        IzFadeInitEasy(0U, 1U, 32f);
    }

    private static void gmClearDemoSetRetryInput(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (isBackKeyPressed())
        {
            main_work.flag |= 2U;
            setBackKeyRequest(false);
            GmSoundPlaySE("Ok");
        }
        else
        {
            CTrgAoAction trgRetry = main_work.trg_retry;
            trgRetry.Update();
            if (trgRetry.GetState(0U)[10] && trgRetry.GetState(0U)[1])
            {
                main_work.flag |= 4U;
                int index1 = 0;
                for (int index2 = arrayof(main_work.btn_retry); index1 < index2; ++index1)
                {
                    main_work.btn_retry[index1].obj_2d.frame = 2f;
                    main_work.btn_retry[index1].obj_2d.speed = 1f;
                }

                GmSoundPlaySE("Ok");
            }
            else if (trgRetry.GetState(0U)[0])
            {
                int index1 = 0;
                for (int index2 = arrayof(main_work.btn_retry); index1 < index2; ++index1)
                    main_work.btn_retry[index1].obj_2d.frame = 1f;
            }
            else if (trgRetry.GetState(0U)[13])
            {
                int index1 = 0;
                for (int index2 = arrayof(main_work.btn_retry); index1 < index2; ++index1)
                    main_work.btn_retry[index1].obj_2d.frame = 0.0f;
            }

            CTrgAoAction trgBack = main_work.trg_back;
            trgBack.Update();
            if (trgBack.GetState(0U)[10] && trgBack.GetState(0U)[1])
            {
                main_work.flag |= 2U;
                int index1 = 0;
                for (int index2 = arrayof(main_work.btn_back); index1 < index2; ++index1)
                {
                    main_work.btn_back[index1].obj_2d.frame = 2f;
                    main_work.btn_back[index1].obj_2d.speed = 1f;
                }

                GmSoundPlaySE("Ok");
            }
            else if (trgBack.GetState(0U)[2])
            {
                int index1 = 0;
                for (int index2 = arrayof(main_work.btn_back); index1 < index2; ++index1)
                    main_work.btn_back[index1].obj_2d.frame = 1f;
            }
            else
            {
                if (!trgBack.GetState(0U)[13])
                    return;
                int index1 = 0;
                for (int index2 = arrayof(main_work.btn_back); index1 < index2; ++index1)
                    main_work.btn_back[index1].obj_2d.frame = 0.0f;
            }
        }
    }

    private static void gmClearDemoSetRetryDispInfo(GMS_CLRDM_MAIN_WORK main_work)
    {
        g_gm_main_system.ply_work[0].player_flag |= GMD_PLF_TATK_RETRY;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        GmSoundStopStageBGM(64);
        GmSoundStopJingle(64);
        GmSoundStopBGMJingle(64);
        GsSoundStopSe();
        GmMapSetDisp(false);
        GmFixSetDisp(false);
        GmObjSetAllObjectNoDisp();
        GmRingGetWork().flag |= 1U;
        g_gm_main_system.game_flag |= 8192U;
        g_gm_main_system.water_level = ushort.MaxValue;
        g_gm_main_system.game_flag &= 4294964223U;
        gmsPlayerWork.obj_work.flag &= 4294967167U;
        gmsPlayerWork.obj_work.disp_flag &= 4294967263U;
        OBS_CAMERA obsCamera = ObjCameraGet(g_obj.glb_camera_id);
        ObjCameraSetUserFunc(0, new OBJF_CAMERA_USER_FUNC(GmCameraFunc));
        GmCameraScaleSet(1f, 1f);
        obsCamera.roll = 0;
        gmsPlayerWork.gmk_flag &= 4294934527U;
        gmsPlayerWork.obj_work.pos.x = FXM_FLOAT_TO_FX32(obsCamera.pos.x);
        gmsPlayerWork.obj_work.pos.y = FXM_FLOAT_TO_FX32(obsCamera.pos.y * -1f);
        GmCamScrLimitSetDirect(new GMS_EVE_RECORD_EVENT()
        {
            flag = 15,
            left = -96,
            top = -85,
            width = 192,
            height = 112
        }, FXM_FLOAT_TO_FX32(obsCamera.pos.x), FXM_FLOAT_TO_FX32(obsCamera.pos.y * -1f));
        GmPlySeqChangeTRetryFw(g_gm_main_system.ply_work[0]);
        GmObjSetObjectNoFunc(4U);
        GmObjSetObjectNoFunc(8U);
        GmObjSetObjectNoFunc(16U);
        GmObjSetObjectNoFunc(32U);
        GmObjSetObjectNoFunc(64U);
        GmObjSetObjectNoFunc(128U);
        GmObjSetObjectNoFunc(256U);
        GMM_PAD_VIB_STOP();
        ((OBS_OBJECT_WORK) main_work.tex_retry_act).disp_flag &= 4294967263U;
        ((OBS_OBJECT_WORK) main_work.tex_back_slct_act).disp_flag &= 4294967263U;
        ((OBS_OBJECT_WORK) main_work.bg_retry).disp_flag &= 4294967263U;
        amFlagOff(ref main_work.flag, 1024U);
        main_work.flag &= 4294966271U;
        for (int index = 0; index < arrayof(main_work.record_time_num_act); ++index)
            ((OBS_OBJECT_WORK) main_work.record_time_num_act[index]).disp_flag |= 32U;
        int index1 = 0;
        for (int index2 = arrayof(main_work.btn_retry); index1 < index2; ++index1)
            ((OBS_OBJECT_WORK) main_work.btn_retry[index1]).disp_flag &= 4294967263U;
        int index3 = 0;
        for (int index2 = arrayof(main_work.btn_back); index3 < index2; ++index3)
            ((OBS_OBJECT_WORK) main_work.btn_back[index3]).disp_flag &= 4294967263U;
        ObjDrawClearNNCommandStateTbl();
        ObjDrawSetNNCommandStateTbl(0U, 1U, false);
        ObjDrawSetNNCommandStateTbl(1U, 2U, false);
        ObjDrawSetNNCommandStateTbl(2U, 3U, true);
        ObjDrawSetNNCommandStateTbl(3U, 5U, true);
        ObjDrawSetNNCommandStateTbl(4U, 11U, true);
        ObjDrawSetNNCommandStateTbl(5U, 12U, true);
        ObjDrawSetNNCommandStateTbl(6U, 9U, true);
        ObjDrawSetNNCommandStateTbl(7U, 4U, true);
        ObjDrawSetNNCommandStateTbl(8U, 8U, true);
        ObjDrawSetNNCommandStateTbl(9U, 7U, true);
        ObjDrawSetNNCommandStateTbl(10U, 10U, true);
        ObjDrawSetNNCommandStateTbl(11U, 6U, true);
        ObjDrawSetNNCommandStateTbl(12U, 0U, true);
    }

    private static void gmClearDemoProcSpeScoreMoveEfct(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AoActIsEndTrs(main_work.tex_total_act.obj_2d.act))
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeScorePrevCalcScore);
            amFlagOn(ref main_work.flag, 32U);
            amFlagOn(ref main_work.flag, 512U);
        }

        if (AoActIsEndTrs(main_work.tex_time_act.obj_2d.act))
            amFlagOn(ref main_work.flag, 128U);
        if (AoActIsEndTrs(main_work.tex_ring_act.obj_2d.act))
            amFlagOn(ref main_work.flag, 256U);
        gmClearDemoSetSortBufAct(main_work);
        gmClearDemoSetScoreData(main_work);
        gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcSpeScorePrevCalcScore(GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += main_work.count;
        if (main_work.timer > 150.0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeScoreCalcScore);
            main_work.proc_calc_score = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoSetCalcScore);
            main_work.timer = 0.0f;
        }

        if (amTpIsTouchPush(0) || isBackKeyPressed())
        {
            setBackKeyRequest(false);
            amFlagOn(ref main_work.flag, 4U);
        }

        gmClearDemoSetSortBufAct(main_work);
        gmClearDemoSetScoreData(main_work);
        gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcSpeScoreCalcScore(GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += main_work.count;
        if (((int) main_work.flag & 4) != 0 || ((int) main_work.flag & 8) != 0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeScoreWaitDispSonic);
            main_work.time_score[1] = main_work.time_score[0];
            main_work.ring_score[1] = main_work.ring_score[0];
            main_work.total_score[1] = main_work.total_score[0];
            main_work.proc_calc_score = null;
            amFlagOff(ref main_work.flag, 4U);
            amFlagOff(ref main_work.flag, 8U);
            if (main_work.total_score[1] >= 10000U)
            {
                amFlagOn(ref main_work.flag, 16U);
                main_work.idle_time = 270;
            }
            else
                main_work.idle_time = 120;

            HgTrophyTryAcquisition(0);
            main_work.timer = 0.0f;
        }

        if (amTpIsTouchPush(0) || isBackKeyPressed())
        {
            setBackKeyRequest(false);
            amFlagOn(ref main_work.flag, 4U);
        }

        if (main_work.proc_calc_score != null && main_work.timer > 1.0)
        {
            main_work.proc_calc_score(main_work);
            main_work.timer = 0.0f;
        }

        gmClearDemoSetSortBufAct(main_work);
        gmClearDemoSetScoreData(main_work);
        gmClearDemoUpdateAct(main_work);
        if (main_work.proc_calc_score == null)
        {
            GmSoundPlaySE("Result2");
        }
        else
        {
            if (0.0 != main_work.timer)
                return;
            GmSoundPlaySE("Result1");
        }
    }

    private static void gmClearDemoProcSpeScoreWaitDispSonic(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.timer >= 60.0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeScoreDispIdle);
            main_work.timer = 0.0f;
            if (((int) main_work.flag & 16) == 0)
                return;
            GmSoundPlayJingle(0U);
            GmPlayerStockGet(g_gm_main_system.ply_work[0], 1);
        }
        else
        {
            gmClearDemoSetSortBufAct(main_work);
            gmClearDemoSetScoreData(main_work);
            gmClearDemoUpdateAct(main_work);
            main_work.timer += main_work.count;
        }
    }

    private static void gmClearDemoProcSpeScoreDispIdle(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.timer >= (double) main_work.idle_time)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeScoreFadeOut);
            IzFadeInitEasy(0U, 3U, 32f);
        }

        if (((int) main_work.flag & 16) != 0)
            gmClearDemoSetFlashSonic(main_work);
        gmClearDemoSetSortBufAct(main_work);
        gmClearDemoSetScoreData(main_work);
        gmClearDemoUpdateAct(main_work);
        main_work.timer += main_work.count;
    }

    private static void gmClearDemoProcSpeScoreFadeOut(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (IzFadeIsEnd())
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeScoreFinish);
            main_work.timer = 0.0f;
        }
        else
        {
            if (((int) main_work.flag & 16) != 0)
                gmClearDemoSetFlashSonic(main_work);
            gmClearDemoSetSortBufAct(main_work);
            gmClearDemoSetScoreData(main_work);
            gmClearDemoUpdateAct(main_work);
            main_work.timer += main_work.count;
        }
    }

    private static void gmClearDemoProcSpeScoreFinish(GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.proc_update = null;
        amFlagOn(ref main_work.flag, 1U);
    }

    private static void gmClearDemoProcSpeTimeMoveEfct(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AoActIsEndTrs(main_work.tex_big_time_act.obj_2d.act))
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeTimeWaitTimeEfct);
            for (int index = 0; index < 7; ++index)
            {
                ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.record_time_num_act[index], main_work.record_time_num_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (49 + index), 0);
                ((OBS_OBJECT_WORK) main_work.record_time_num_act[index]).scale.x = 4096;
                ((OBS_OBJECT_WORK) main_work.record_time_num_act[index]).scale.y = 4096;
            }

            gmClearDemoSetClearTimeRecord(main_work);
            gmClearDemoSetSortBufTimeAct(main_work);
            amFlagOn(ref main_work.flag, 1024U);
            amFlagOn(ref main_work.flag, 2048U);
        }

        gmClearDemoSetTimeAtkSortBufAct(main_work);
        gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcSpeTimeWaitTimeEfct(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.timer > 60.0)
        {
            if (((int) main_work.flag & 4096) != 0)
            {
                main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeTimeTimeMoveNewRecord);
                main_work.tex_new_record_act.obj_2d.speed = 1f;
                main_work.idle_time = 180;
            }
            else
            {
                main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeTimeDispEffect);
                main_work.idle_time = 120;
            }

            HgTrophyTryAcquisition(0);
            main_work.timer = 0.0f;
        }

        gmClearDemoSetTimeAtkSortBufAct(main_work);
        gmClearDemoUpdateAct(main_work);
        if (main_work.timer > 120.0)
            return;
        main_work.timer += main_work.count;
    }

    private static void gmClearDemoProcSpeTimeTimeMoveNewRecord(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AoActIsEndTrs(main_work.tex_new_record_act.obj_2d.act))
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeTimeDispEffect);
            GmSoundPlayJingle(5U);
        }

        gmClearDemoSetTimeAtkSortBufAct(main_work);
        gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcSpeTimeDispEffect(GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += main_work.count;
        if ((AoPad.AoPadStand() & ControllerConsts.CONFIRM) != 0)
            main_work.flag |= 4U;
        if (main_work.timer > (double) main_work.idle_time || ((int) main_work.flag & 4) != 0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeTimeChangeRetryOut);
            IzFadeInitEasy(0U, 1U, 32f);
            main_work.timer = 0.0f;
            main_work.flag &= 4294967291U;
        }

        gmClearDemoTimeFlushEffect(main_work);
        gmClearDemoSetTimeAtkSortBufAct(main_work);
    }

    private static void gmClearDemoProcSpeTimeChangeRetryOut(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (IzFadeIsEnd())
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeTimeChangeRetryIn);
            IzFadeInitEasy(0U, 0U, 32f);
            gmClearDemoSetRetryDispInfo(main_work);
            gmClearDemoSetRetrySortBufAct(main_work);
            main_work.timer = 0.0f;
        }
        else
            gmClearDemoSetTimeAtkSortBufAct(main_work);
    }

    private static void gmClearDemoProcSpeTimeChangeRetryIn(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcSpeTimeWaitSelectRetry);
    }

    private static void gmClearDemoProcSpeTimeWaitSelectRetry(GMS_CLRDM_MAIN_WORK main_work)
    {
        gmClearDemoSetBgColorBlack();
        gmClearDemoSetRetryInput(main_work);
        if (((int) main_work.flag & 4) != 0)
        {
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcWaitRetrySonicRunEfct);
            GmPlySeqChangeTRetryAcc(g_gm_main_system.ply_work[0]);
        }
        else
        {
            if (((int) main_work.flag & 2) == 0)
                return;
            main_work.proc_update = new GMS_CLRDM_MAIN_WORK._WorkDelegate(gmClearDemoProcFadeOut);
            IzFadeInitEasy(0U, 1U, 32f);
        }
    }

    private static void gmClearDemoCreateObjActScore(GMS_CLRDM_MAIN_WORK main_work)
    {
        TaskWorkFactoryDelegate work_size = () => new GMS_COCKPIT_2D_WORK();
        OBS_OBJECT_WORK obsObjectWork;
        for (int index = 0; index < 5; ++index)
        {
            OBS_OBJECT_WORK work1 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_UPTEXT");
            main_work.tex_up_act[index] = (GMS_COCKPIT_2D_WORK) work1;
            obsObjectWork = null;
            OBS_OBJECT_WORK work2 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TIMENUM");
            main_work.time_num_act[index] = (GMS_COCKPIT_2D_WORK) work2;
            obsObjectWork = null;
            OBS_OBJECT_WORK work3 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_RINGNUM");
            main_work.ring_num_act[index] = (GMS_COCKPIT_2D_WORK) work3;
            obsObjectWork = null;
        }

        for (int index = 0; index < 9; ++index)
        {
            OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TOTALNUM");
            main_work.total_num_act[index] = (GMS_COCKPIT_2D_WORK) work;
            obsObjectWork = null;
        }

        for (int index = 0; index < 3; ++index)
        {
            OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_LINE");
            main_work.line_act[index] = (GMS_COCKPIT_2D_WORK) work;
            obsObjectWork = null;
        }

        OBS_OBJECT_WORK work4 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TIMETEXT");
        main_work.tex_time_act = (GMS_COCKPIT_2D_WORK) work4;
        obsObjectWork = null;
        OBS_OBJECT_WORK work5 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_RINGTEXT");
        main_work.tex_ring_act = (GMS_COCKPIT_2D_WORK) work5;
        obsObjectWork = null;
        OBS_OBJECT_WORK work6 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TOTALTEXT");
        main_work.tex_total_act = (GMS_COCKPIT_2D_WORK) work6;
        obsObjectWork = null;
        OBS_OBJECT_WORK work7 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_SONICICON");
        main_work.sonic_icon_act = (GMS_COCKPIT_2D_WORK) work7;
        obsObjectWork = null;
        OBS_OBJECT_WORK work8 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_SONICICON2");
        main_work.sonic_icon_act2 = (GMS_COCKPIT_2D_WORK) work8;
        obsObjectWork = null;
    }

    private static void gmClearDemoCreateObjActTime(GMS_CLRDM_MAIN_WORK main_work)
    {
        TaskWorkFactoryDelegate work_size = () => new GMS_COCKPIT_2D_WORK();
        OBS_OBJECT_WORK obsObjectWork;
        for (int index = 0; index < 5; ++index)
        {
            OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_UPTEXT");
            main_work.tex_up_act[index] = (GMS_COCKPIT_2D_WORK) work;
            obsObjectWork = null;
        }

        OBS_OBJECT_WORK work1 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TIMEATK_TEXT");
        main_work.tex_big_time_act = (GMS_COCKPIT_2D_WORK) work1;
        obsObjectWork = null;
        for (int index = 0; index < 7; ++index)
        {
            OBS_OBJECT_WORK work2 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TIME_DIGIT_NUM");
            main_work.record_time_num_act[index] = (GMS_COCKPIT_2D_WORK) work2;
            obsObjectWork = null;
        }

        OBS_OBJECT_WORK work3 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TIMEATK_SONIC");
        main_work.time_sonic_icon_act = (GMS_COCKPIT_2D_WORK) work3;
        obsObjectWork = null;
        OBS_OBJECT_WORK work4 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_NEWRECORD_TEXT");
        main_work.tex_new_record_act = (GMS_COCKPIT_2D_WORK) work4;
        obsObjectWork = null;
        OBS_OBJECT_WORK work5 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_BG_RETRY");
        main_work.bg_retry = (GMS_COCKPIT_2D_WORK) work5;
        obsObjectWork = null;
        OBS_OBJECT_WORK work6 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_RETRY_TEXT");
        main_work.tex_retry_act = (GMS_COCKPIT_2D_WORK) work6;
        obsObjectWork = null;
        OBS_OBJECT_WORK work7 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_ACT_BACK_TEXT");
        main_work.tex_back_slct_act = (GMS_COCKPIT_2D_WORK) work7;
        obsObjectWork = null;
        int index1 = 0;
        for (int index2 = arrayof(main_work.btn_retry); index1 < index2; ++index1)
        {
            OBS_OBJECT_WORK work2 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_RETRY_BTN");
            main_work.btn_retry[index1] = (GMS_COCKPIT_2D_WORK) work2;
            obsObjectWork = null;
        }

        int index3 = 0;
        for (int index2 = arrayof(main_work.btn_back); index3 < index2; ++index3)
        {
            OBS_OBJECT_WORK work2 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_ACT_BACK_BTN");
            main_work.btn_back[index3] = (GMS_COCKPIT_2D_WORK) work2;
            obsObjectWork = null;
        }
    }

    private static void gmClearDemoCreateObjSpeScore(GMS_CLRDM_MAIN_WORK main_work)
    {
        TaskWorkFactoryDelegate work_size = () => new GMS_COCKPIT_2D_WORK();
        OBS_OBJECT_WORK obsObjectWork;
        for (int index = 0; index < 3; ++index)
        {
            OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_UP_SPST_TEXT");
            main_work.tex_spst_up_act[index] = (GMS_COCKPIT_2D_WORK) work;
            obsObjectWork = null;
        }

        for (int index = 0; index < 7; ++index)
        {
            OBS_OBJECT_WORK work1 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_SPE_STAGE_NO");
            main_work.spst_num_act[index] = (GMS_COCKPIT_2D_WORK) work1;
            obsObjectWork = null;
            OBS_OBJECT_WORK work2 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_EMER_UP_ICON");
            main_work.icon_emer_up_act[index] = (GMS_COCKPIT_2D_WORK) work2;
            obsObjectWork = null;
            OBS_OBJECT_WORK work3 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_EMER_DOWN_ICON");
            main_work.icon_emer_down_act[index] = (GMS_COCKPIT_2D_WORK) work3;
            obsObjectWork = null;
        }

        OBS_OBJECT_WORK work4 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_LIGHT_EMER");
        main_work.icon_emer_light_act = (GMS_COCKPIT_2D_WORK) work4;
        obsObjectWork = null;
        OBS_OBJECT_WORK work5 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_EXTEND_TEXT");
        main_work.tex_extend_act = (GMS_COCKPIT_2D_WORK) work5;
        obsObjectWork = null;
        for (int index = 0; index < 5; ++index)
        {
            OBS_OBJECT_WORK work1 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TIMENUM");
            main_work.time_num_act[index] = (GMS_COCKPIT_2D_WORK) work1;
            obsObjectWork = null;
            OBS_OBJECT_WORK work2 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_RINGNUM");
            main_work.ring_num_act[index] = (GMS_COCKPIT_2D_WORK) work2;
            obsObjectWork = null;
        }

        for (int index = 0; index < 9; ++index)
        {
            OBS_OBJECT_WORK work1 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TOTALNUM");
            main_work.total_num_act[index] = (GMS_COCKPIT_2D_WORK) work1;
            obsObjectWork = null;
        }

        for (int index = 0; index < 3; ++index)
        {
            OBS_OBJECT_WORK work1 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_LINE");
            main_work.line_act[index] = (GMS_COCKPIT_2D_WORK) work1;
            obsObjectWork = null;
        }

        OBS_OBJECT_WORK work6 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TIMETEXT");
        main_work.tex_time_act = (GMS_COCKPIT_2D_WORK) work6;
        obsObjectWork = null;
        OBS_OBJECT_WORK work7 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_RINGTEXT");
        main_work.tex_ring_act = (GMS_COCKPIT_2D_WORK) work7;
        obsObjectWork = null;
        OBS_OBJECT_WORK work8 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TOTALTEXT");
        main_work.tex_total_act = (GMS_COCKPIT_2D_WORK) work8;
        obsObjectWork = null;
        OBS_OBJECT_WORK work9 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_SONICICON");
        main_work.sonic_icon_act = (GMS_COCKPIT_2D_WORK) work9;
        obsObjectWork = null;
        OBS_OBJECT_WORK work10 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_SONICICON2");
        main_work.sonic_icon_act2 = (GMS_COCKPIT_2D_WORK) work10;
        obsObjectWork = null;
    }

    private static void gmClearDemoCreateObjSpeTime(GMS_CLRDM_MAIN_WORK main_work)
    {
        TaskWorkFactoryDelegate work_size = () => new GMS_COCKPIT_2D_WORK();
        OBS_OBJECT_WORK obsObjectWork;
        for (int index = 0; index < 3; ++index)
        {
            OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_UP_SPST_TEXT");
            main_work.tex_spst_up_act[index] = (GMS_COCKPIT_2D_WORK) work;
            obsObjectWork = null;
        }

        for (int index = 0; index < 7; ++index)
        {
            OBS_OBJECT_WORK work1 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_SPE_STAGE_NO");
            main_work.spst_num_act[index] = (GMS_COCKPIT_2D_WORK) work1;
            obsObjectWork = null;
            OBS_OBJECT_WORK work2 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_EMER_UP_ICON");
            main_work.icon_emer_up_act[index] = (GMS_COCKPIT_2D_WORK) work2;
            obsObjectWork = null;
            OBS_OBJECT_WORK work3 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_EMER_DOWN_ICON");
            main_work.icon_emer_down_act[index] = (GMS_COCKPIT_2D_WORK) work3;
            obsObjectWork = null;
        }

        OBS_OBJECT_WORK work4 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_LIGHT_EMER");
        main_work.icon_emer_light_act = (GMS_COCKPIT_2D_WORK) work4;
        obsObjectWork = null;
        OBS_OBJECT_WORK work5 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TIMEATK_TEXT");
        main_work.tex_big_time_act = (GMS_COCKPIT_2D_WORK) work5;
        obsObjectWork = null;
        for (int index = 0; index < 7; ++index)
        {
            OBS_OBJECT_WORK work1 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TIME_DIGIT_NUM");
            main_work.record_time_num_act[index] = (GMS_COCKPIT_2D_WORK) work1;
            obsObjectWork = null;
        }

        OBS_OBJECT_WORK work6 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_TIMEATK_SONIC");
        main_work.time_sonic_icon_act = (GMS_COCKPIT_2D_WORK) work6;
        obsObjectWork = null;
        OBS_OBJECT_WORK work7 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_NEWRECORD_TEXT");
        main_work.tex_new_record_act = (GMS_COCKPIT_2D_WORK) work7;
        obsObjectWork = null;
        OBS_OBJECT_WORK work8 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_BG_RETRY");
        main_work.bg_retry = (GMS_COCKPIT_2D_WORK) work8;
        obsObjectWork = null;
        OBS_OBJECT_WORK work9 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_RETRY_TEXT");
        main_work.tex_retry_act = (GMS_COCKPIT_2D_WORK) work9;
        obsObjectWork = null;
        OBS_OBJECT_WORK work10 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_ACT_BACK_TEXT");
        main_work.tex_back_slct_act = (GMS_COCKPIT_2D_WORK) work10;
        obsObjectWork = null;
        int index1 = 0;
        for (int index2 = arrayof(main_work.btn_retry); index1 < index2; ++index1)
        {
            OBS_OBJECT_WORK work1 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_RETRY_BTN");
            main_work.btn_retry[index1] = (GMS_COCKPIT_2D_WORK) work1;
            obsObjectWork = null;
        }

        int index3 = 0;
        for (int index2 = arrayof(main_work.btn_back); index3 < index2; ++index3)
        {
            OBS_OBJECT_WORK work1 = GMM_COCKPIT_CREATE_WORK(work_size, null, 0, "CLRDM_ACT_BACK_BTN");
            main_work.btn_back[index3] = (GMS_COCKPIT_2D_WORK) work1;
            obsObjectWork = null;
        }
    }

    private static void gmClearDemoCreateObjAct(GMS_CLRDM_MAIN_WORK main_work)
    {
        gmClearDemoCreateObjActForStage(main_work);
        for (int index = 0; index < 5; ++index)
        {
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.time_num_act[index], main_work.time_num_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (4 + index), 0);
            ((OBS_OBJECT_WORK) main_work.time_num_act[index]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.time_num_act[index]).scale.y = 4096;
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.ring_num_act[index], main_work.ring_num_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (9 + index), 0);
            ((OBS_OBJECT_WORK) main_work.ring_num_act[index]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.ring_num_act[index]).scale.y = 4096;
        }

        for (int index = 0; index < 9; ++index)
        {
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.total_num_act[index], main_work.total_num_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (18 + index), 0);
            ((OBS_OBJECT_WORK) main_work.total_num_act[index]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.total_num_act[index]).scale.y = 4096;
        }

        for (int index = 0; index < 3; ++index)
        {
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.line_act[index], main_work.line_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (14 + index), 0);
            ((OBS_OBJECT_WORK) main_work.line_act[index]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.line_act[index]).scale.y = 4096;
        }

        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_time_act, main_work.tex_time_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 7U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_time_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_time_act).scale.y = 4096;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_ring_act, main_work.tex_ring_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 8U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_ring_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_ring_act).scale.y = 4096;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_total_act, main_work.tex_total_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 9U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_total_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_total_act).scale.y = 4096;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.sonic_icon_act, main_work.sonic_icon_act.obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), 17U, 0);
        ((OBS_OBJECT_WORK) main_work.sonic_icon_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.sonic_icon_act).scale.y = 4096;
    }

    private static void gmClearDemoCreateObjNormalTimeAtk(GMS_CLRDM_MAIN_WORK main_work)
    {
        gmClearDemoCreateObjActForStage(main_work);
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_big_time_act, main_work.tex_big_time_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 15U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_big_time_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_big_time_act).scale.y = 4096;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.time_sonic_icon_act, main_work.time_sonic_icon_act.obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), 56U, 0);
        ((OBS_OBJECT_WORK) main_work.time_sonic_icon_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.time_sonic_icon_act).scale.y = 4096;
        main_work.time_sonic_icon_act.obj_2d.frame = GMM_MAIN_USE_SUPER_SONIC() ? 1f : 0.0f;
        main_work.time_sonic_icon_act.obj_2d.speed = 0.0f;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_new_record_act, main_work.tex_new_record_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 14U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_new_record_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_new_record_act).scale.y = 4096;
    }

    private static void gmClearDemoCreateObjSpecialScoreAtk(GMS_CLRDM_MAIN_WORK main_work)
    {
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_spst_up_act[0], main_work.tex_spst_up_act[0].obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 11U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[0]).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[0]).scale.y = 4096;
        if (main_work.is_get_eme && main_work.is_first_spe_clear)
        {
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_spst_up_act[1], main_work.tex_spst_up_act[1].obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 12U, 0);
            ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[1]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[1]).scale.y = 4096;
        }

        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_spst_up_act[2], main_work.tex_spst_up_act[2].obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 13U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[2]).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[2]).scale.y = 4096;
        gmClearDemoCreateObjSpeActForStage(main_work);
        for (int index = 0; index < main_work.has_eme_num; ++index)
        {
            if (main_work.is_get_eme && main_work.is_first_spe_clear)
            {
                ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.icon_emer_down_act[index], main_work.icon_emer_down_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (34 + index), 0);
                ((OBS_OBJECT_WORK) main_work.icon_emer_down_act[index]).scale.x = 4096;
                ((OBS_OBJECT_WORK) main_work.icon_emer_down_act[index]).scale.y = 4096;
            }
            else
            {
                ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.icon_emer_up_act[index], main_work.icon_emer_up_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (42 + index), 0);
                ((OBS_OBJECT_WORK) main_work.icon_emer_up_act[index]).scale.x = 4096;
                ((OBS_OBJECT_WORK) main_work.icon_emer_up_act[index]).scale.y = 4096;
            }
        }

        for (int index = 0; index < 5; ++index)
        {
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.time_num_act[index], main_work.time_num_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (57 + index), 0);
            main_work.time_num_act[index].obj_2d.ama.act_tbl[(int) main_work.time_num_act[index].obj_2d.act_id].mtn.trs_tbl[0].trs_y -= 15f;
            ((OBS_OBJECT_WORK) main_work.time_num_act[index]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.time_num_act[index]).scale.y = 4096;
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.ring_num_act[index], main_work.ring_num_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (62 + index), 0);
            ((OBS_OBJECT_WORK) main_work.ring_num_act[index]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.ring_num_act[index]).scale.y = 4096;
        }

        for (int index = 0; index < 9; ++index)
        {
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.total_num_act[index], main_work.total_num_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (72 + index), 0);
            ((OBS_OBJECT_WORK) main_work.total_num_act[index]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.total_num_act[index]).scale.y = 4096;
        }

        for (int index = 0; index < 3; ++index)
        {
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.line_act[index], main_work.line_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (67 + index), 0);
            ((OBS_OBJECT_WORK) main_work.line_act[index]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.line_act[index]).scale.y = 4096;
        }

        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_time_act, main_work.tex_time_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 18U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_time_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_time_act).scale.y = 4096;
        main_work.tex_time_act.obj_2d.ama.act_tbl[(int) main_work.tex_time_act.obj_2d.act_id].mtn.trs_tbl[0].trs_y -= 15f;
        main_work.tex_time_act.obj_2d.ama.act_tbl[(int) main_work.tex_time_act.obj_2d.act_id].mtn.trs_tbl[1].trs_y -= 15f;
        main_work.tex_time_act.obj_2d.ama.act_tbl[(int) main_work.tex_time_act.obj_2d.act_id].mtn.trs_tbl[2].trs_y -= 15f;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_ring_act, main_work.tex_ring_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 19U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_ring_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_ring_act).scale.y = 4096;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_total_act, main_work.tex_total_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 20U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_total_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_total_act).scale.y = 4096;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.sonic_icon_act, main_work.sonic_icon_act.obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), 70U, 0);
        ((OBS_OBJECT_WORK) main_work.sonic_icon_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.sonic_icon_act).scale.y = 4096;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.sonic_icon_act2, main_work.sonic_icon_act2.obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), 71U, 0);
        ((OBS_OBJECT_WORK) main_work.sonic_icon_act2).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.sonic_icon_act2).scale.y = 4096;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_extend_act, main_work.tex_extend_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 10U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_extend_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_extend_act).scale.y = 4096;
    }

    private static void gmClearDemoCreateObjSpecialTimeAtk(GMS_CLRDM_MAIN_WORK main_work)
    {
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_spst_up_act[0], main_work.tex_spst_up_act[0].obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 11U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[0]).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[0]).scale.y = 4096;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_spst_up_act[2], main_work.tex_spst_up_act[2].obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 13U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[2]).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[2]).scale.y = 4096;
        gmClearDemoCreateObjSpeActForStage(main_work);
        for (int index = 0; index < main_work.has_eme_num; ++index)
        {
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.icon_emer_up_act[index], main_work.icon_emer_up_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (42 + index), 0);
            ((OBS_OBJECT_WORK) main_work.icon_emer_up_act[index]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.icon_emer_up_act[index]).scale.y = 4096;
        }

        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_big_time_act, main_work.tex_big_time_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 15U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_big_time_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_big_time_act).scale.y = 4096;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.time_sonic_icon_act, main_work.time_sonic_icon_act.obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), 56U, 0);
        ((OBS_OBJECT_WORK) main_work.time_sonic_icon_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.time_sonic_icon_act).scale.y = 4096;
        main_work.time_sonic_icon_act.obj_2d.frame = GMM_MAIN_USE_SUPER_SONIC() ? 1f : 0.0f;
        main_work.time_sonic_icon_act.obj_2d.speed = 0.0f;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_new_record_act, main_work.tex_new_record_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 14U, 0);
        ((OBS_OBJECT_WORK) main_work.tex_new_record_act).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_new_record_act).scale.y = 4096;
    }

    private static void gmClearDemoCreateObjActForStage(GMS_CLRDM_MAIN_WORK main_work)
    {
        int language = GsEnvGetLanguage();
        ushort num1;
        switch (language)
        {
            case 3:
                num1 = 84;
                break;
            case 5:
                num1 = 85;
                break;
            default:
                num1 = 0;
                break;
        }

        if (language != 4)
        {
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_up_act[0], main_work.tex_up_act[0].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), num1, 0);
            ((OBS_OBJECT_WORK) main_work.tex_up_act[0]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.tex_up_act[0]).scale.y = 4096;
        }

        ushort num2 = language == 4 ? dm_clrdm_stage_ge_tex_act_id[main_work.stage_id] : dm_clrdm_stage_tex_act_id[main_work.stage_id];
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_up_act[1], main_work.tex_up_act[1].obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), num2, 0);
        ((OBS_OBJECT_WORK) main_work.tex_up_act[1]).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_up_act[1]).scale.y = 4096;
        ushort num3;
        int index;
        if (dm_clrdm_stage_text_amb_id[main_work.stage_id] == 0)
        {
            num3 = 0;
            index = 30;
        }
        else
        {
            num3 = 1;
            index = g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()];
        }

        if (num3 == 0 && (language == 0 || language == 1))
        {
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_up_act[2], main_work.tex_up_act[2].obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 3U, 0);
            ((OBS_OBJECT_WORK) main_work.tex_up_act[2]).scale.x = 4096;
            ((OBS_OBJECT_WORK) main_work.tex_up_act[2]).scale.y = 4096;
        }

        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_up_act[3], main_work.tex_up_act[3].obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), dm_clrdm_stage_text_act_id[main_work.stage_id], 0);
        ((OBS_OBJECT_WORK) main_work.tex_up_act[3]).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_up_act[3]).scale.y = 4096;
        ushort num4 = language != 4 ? (language != 2 ? (language != 5 ? (language != 3 ? dm_clrdm_stage_num_act_id[main_work.stage_id] : dm_clrdm_stage_sp_num_act_id[main_work.stage_id]) : dm_clrdm_stage_sp_num_act_id[main_work.stage_id]) : dm_clrdm_stage_fr_num_act_id[main_work.stage_id]) : dm_clrdm_stage_ge_num_act_id[main_work.stage_id];
        if (num4 == ushort.MaxValue)
            return;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_up_act[4], main_work.tex_up_act[4].obj_2d, null, null, index, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[num3]), num4, 0);
        ((OBS_OBJECT_WORK) main_work.tex_up_act[4]).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.tex_up_act[4]).scale.y = 4096;
    }

    private static void gmClearDemoCreateObjSpeActForStage(GMS_CLRDM_MAIN_WORK main_work)
    {
        int language = GsEnvGetLanguage();
        int index = main_work.stage_id - 21;
        int num = language == 3 ? 86 : 27;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.spst_num_act[index], main_work.spst_num_act[index].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (num + index), 0);
        ((OBS_OBJECT_WORK) main_work.spst_num_act[index]).scale.x = 4096;
        ((OBS_OBJECT_WORK) main_work.spst_num_act[index]).scale.y = 4096;
    }

    private static void gmClearDemoSetSortBufAct(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (((int) main_work.flag & 32) == 0)
        {
            for (int index = 0; index < 3; ++index)
                ((OBS_OBJECT_WORK) main_work.line_act[index]).disp_flag |= 32U;
        }
        else
        {
            for (int index = 0; index < 3; ++index)
                ((OBS_OBJECT_WORK) main_work.line_act[index]).disp_flag &= 4294967263U;
        }

        if (((int) main_work.flag & 64) == 0)
            ((OBS_OBJECT_WORK) main_work.sonic_icon_act).disp_flag |= 32U;
        else
            ((OBS_OBJECT_WORK) main_work.sonic_icon_act).disp_flag &= 4294967263U;
        gmClearDemoSetSortBufScore(main_work);
    }

    private static void gmClearDemoSetTimeAtkSortBufAct(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (((int) main_work.flag & 1024) != 0)
        {
            for (int index = 0; index < 7; ++index)
                ((OBS_OBJECT_WORK) main_work.record_time_num_act[index]).disp_flag &= 4294967263U;
        }

        gmClearDemoSetSortBufScore(main_work);
    }

    private static void gmClearDemoSetRetryInitAct(GMS_CLRDM_MAIN_WORK main_work)
    {
        for (int index = 0; index < 5; ++index)
            ((OBS_OBJECT_WORK) main_work.tex_up_act[index]).disp_flag |= 32U;
        for (int index = 0; index < 7; ++index)
            ((OBS_OBJECT_WORK) main_work.record_time_num_act[index]).disp_flag |= 32U;
        ((OBS_OBJECT_WORK) main_work.tex_big_time_act).disp_flag |= 32U;
        ((OBS_OBJECT_WORK) main_work.time_sonic_icon_act).disp_flag |= 32U;
        ((OBS_OBJECT_WORK) main_work.tex_new_record_act).disp_flag |= 32U;
        main_work.tex_retry_act.obj_2d.frame = 0.0f;
        main_work.tex_retry_act.obj_2d.speed = 0.0f;
        main_work.tex_back_slct_act.obj_2d.frame = 0.0f;
        main_work.tex_back_slct_act.obj_2d.speed = 0.0f;
        int index1 = 0;
        for (int index2 = arrayof(main_work.btn_retry); index1 < index2; ++index1)
        {
            main_work.btn_retry[index1].obj_2d.frame = 0.0f;
            main_work.btn_retry[index1].obj_2d.speed = 0.0f;
        }

        int index3 = 0;
        for (int index2 = arrayof(main_work.btn_back); index3 < index2; ++index3)
        {
            main_work.btn_back[index3].obj_2d.frame = 0.0f;
            main_work.btn_back[index3].obj_2d.speed = 0.0f;
        }
    }

    private static void gmClearDemoSetRetrySortBufAct(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.is_clear_spe_stg)
        {
            for (int index = 0; index < 3; ++index)
                ((OBS_OBJECT_WORK) main_work.tex_spst_up_act[index]).disp_flag |= 32U;
        }
        else
        {
            for (int index = 0; index < 5; ++index)
                ((OBS_OBJECT_WORK) main_work.tex_up_act[index]).disp_flag |= 32U;
        }

        for (int index = 0; index < 7; ++index)
            ((OBS_OBJECT_WORK) main_work.record_time_num_act[index]).disp_flag |= 32U;
        ((OBS_OBJECT_WORK) main_work.tex_big_time_act).disp_flag |= 32U;
        ((OBS_OBJECT_WORK) main_work.time_sonic_icon_act).disp_flag |= 32U;
        ((OBS_OBJECT_WORK) main_work.tex_new_record_act).disp_flag |= 32U;
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.bg_retry, main_work.bg_retry.obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), 99U, 0);
        int index1 = 0;
        for (int index2 = arrayof(main_work.btn_retry); index1 < index2; ++index1)
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.btn_retry[index1], main_work.btn_retry[index1].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (100 + index1), 0);
        int index3 = 0;
        for (int index2 = arrayof(main_work.btn_back); index3 < index2; ++index3)
            ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.btn_back[index3], main_work.btn_back[index3].obj_2d, null, null, 30, GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[0]), (uint) (103 + index3), 0);
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_retry_act, main_work.tex_retry_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 16U, 0);
        ObjObjectAction2dAMALoadSetTexlist((OBS_OBJECT_WORK) main_work.tex_back_slct_act, main_work.tex_back_slct_act.obj_2d, null, null, g_gm_clear_demo_data_ama_id[GsEnvGetLanguage()], GmGameDatGetCockpitData(), AoTexGetTexList(main_work.tex[1]), 17U, 0);
        main_work.tex_retry_act.obj_2d.frame = 0.0f;
        main_work.tex_retry_act.obj_2d.speed = 0.0f;
        main_work.tex_back_slct_act.obj_2d.frame = 0.0f;
        main_work.tex_back_slct_act.obj_2d.speed = 0.0f;
        int index4 = 0;
        for (int index2 = arrayof(main_work.btn_retry); index4 < index2; ++index4)
        {
            main_work.btn_retry[index4].obj_2d.frame = 0.0f;
            main_work.btn_retry[index4].obj_2d.speed = 0.0f;
        }

        int index5 = 0;
        for (int index2 = arrayof(main_work.btn_back); index5 < index2; ++index5)
        {
            main_work.btn_back[index5].obj_2d.frame = 0.0f;
            main_work.btn_back[index5].obj_2d.speed = 0.0f;
        }

        main_work.trg_retry.Create(main_work.btn_retry[1].obj_2d.act);
        main_work.trg_back.Create(main_work.btn_back[1].obj_2d.act);
    }

    private static void gmClearDemoSetSortBufScore(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (((int) main_work.flag & 128) != 0)
            gmClearDemoSetSortBufScoreAct(main_work.time_num_act, main_work.time_score[1], 5U);
        if (((int) main_work.flag & 256) != 0)
            gmClearDemoSetSortBufScoreAct(main_work.ring_num_act, main_work.ring_score[1], 5U);
        if (((int) main_work.flag & 512) == 0)
            return;
        gmClearDemoSetSortBufScoreAct(main_work.total_num_act, main_work.total_score[1], 9U);
    }

    private static void gmClearDemoSetSortBufScoreAct(
        GMS_COCKPIT_2D_WORK[] score_act,
        uint score,
        uint digits)
    {
        int num = 1;
        if (score < 10U)
        {
            for (int index = 0; index < (int) digits - 1; ++index)
                ((OBS_OBJECT_WORK) score_act[index]).disp_flag |= 32U;
            ((OBS_OBJECT_WORK) score_act[(int) (digits - 1U)]).disp_flag &= 4294967263U;
        }
        else
        {
            if (score < 10U)
                return;
            for (int index1 = 0; index1 < (int) digits; ++index1)
            {
                for (int index2 = 0; index2 < (int) (digits - index1 - 1L); ++index2)
                    num *= 10;
                if (score < (uint) num)
                    ((OBS_OBJECT_WORK) score_act[index1]).disp_flag |= 32U;
                else
                    ((OBS_OBJECT_WORK) score_act[index1]).disp_flag &= 4294967263U;
                num = 1;
            }
        }
    }

    private static void gmClearDemoSetSortBufTimeAct(GMS_CLRDM_MAIN_WORK main_work)
    {
        int num1 = main_work.time_sec < 10 ? 0 : main_work.time_sec / 10;
        int num2 = main_work.time_msec < 10 ? 0 : main_work.time_msec / 10;
        int num3 = main_work.time_sec % 10;
        int num4 = main_work.time_msec % 10;
        main_work.record_time_num_act[0].obj_2d.frame = main_work.time_min;
        main_work.record_time_num_act[2].obj_2d.frame = num1;
        main_work.record_time_num_act[3].obj_2d.frame = num3;
        main_work.record_time_num_act[5].obj_2d.frame = num2;
        main_work.record_time_num_act[6].obj_2d.frame = num4;
        main_work.record_time_num_act[1].obj_2d.frame = 0.0f;
        main_work.record_time_num_act[4].obj_2d.frame = 0.0f;
    }

    private static void gmClearDemoUpdateAct(GMS_CLRDM_MAIN_WORK main_work)
    {
    }

    private static void gmClearDemoTimeFlushEffect(GMS_CLRDM_MAIN_WORK main_work)
    {
    }

    private static void gmClearDemoSetCalcScore(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.ring_score[1] <= 0U && main_work.time_score[1] <= 0U)
        {
            main_work.proc_calc_score = null;
            amFlagOn(ref main_work.flag, 8U);
        }
        else
        {
            if (main_work.time_score[1] > 0U)
            {
                main_work.time_score[1] -= 100U;
                main_work.total_score[1] += 100U;
            }

            if (main_work.ring_score[1] <= 0U)
                return;
            main_work.ring_score[1] -= 100U;
            main_work.total_score[1] += 100U;
        }
    }

    private static void gmClearDemoSetScoreData(GMS_CLRDM_MAIN_WORK main_work)
    {
        gmClearDemoSetDispScore(main_work.time_num_act, main_work.time_score[1], 5U);
        gmClearDemoSetDispScore(main_work.ring_num_act, main_work.ring_score[1], 5U);
        gmClearDemoSetDispScore(main_work.total_num_act, main_work.total_score[1], 9U);
    }

    private static void gmClearDemoSetDispScore(
        GMS_COCKPIT_2D_WORK[] score_act,
        uint score,
        uint digits)
    {
        int num1 = 1;
        for (int index = 0; index < 1; ++index)
        {
            score_act[digits - 1U - index].obj_2d.frame = 0.0f;
            score_act[digits - 1U - index].obj_2d.speed = 0.0f;
        }

        for (int index1 = 1; index1 < (int) digits; ++index1)
        {
            for (int index2 = 0; index2 < index1; ++index2)
                num1 *= 10;
            if (score >= (uint) num1)
            {
                float num2 = (int) (float) (score / num1) % 10;
                score_act[digits - 1U - index1].obj_2d.frame = num2;
                score_act[digits - 1U - index1].obj_2d.speed = 0.0f;
            }
            else
            {
                score_act[digits - 1U - index1].obj_2d.frame = 0.0f;
                score_act[digits - 1U - index1].obj_2d.speed = 0.0f;
            }

            num1 = 1;
        }
    }

    private static void gmClearDemoSetFlashSonic(GMS_CLRDM_MAIN_WORK main_work)
    {
        if (((int) main_work.flag & 64) != 0)
        {
            if (main_work.flash_timer >= 30.0)
            {
                amFlagOff(ref main_work.flag, 64U);
                main_work.flash_timer = 0.0f;
            }
        }
        else if (main_work.flash_timer >= 10.0)
        {
            amFlagOn(ref main_work.flag, 64U);
            main_work.flash_timer = 0.0f;
        }

        main_work.flash_timer += main_work.count;
    }

    private static bool gmClearDemoIsTexLoad()
    {
        int num = 0;
        for (int index = 0; index < 2; ++index)
        {
            if (AoTexIsLoaded(gm_clrdm_tex[index]))
                num |= 1 << index;
        }

        return num == 3;
    }

    private static bool gmClearDemoIsTexRelease()
    {
        int num = 0;
        for (int index = 0; index < 2; ++index)
        {
            if (AoTexIsReleased(gm_clrdm_tex[index]))
                num |= 1 << index;
        }

        return num == 3;
    }

    private static short gmClearDemoGetRingNum()
    {
        return g_gm_main_system.ply_work[0] != null ? g_gm_main_system.ply_work[0].ring_num : (short) 0;
    }

    private static uint gmClearDemoGetScore()
    {
        return g_gm_main_system.ply_work[0] != null ? g_gm_main_system.ply_work[0].score : 0U;
    }

    private static uint gmClearDemoGetGameTime()
    {
        if (g_gs_main_sys_info.stage_id >= 21 && ((int) g_gm_main_system.game_flag & 65536) == 0)
            return 0;
        return g_gm_main_system.game_time >= 35999U ? 35999U : g_gm_main_system.game_time;
    }

    private static uint gmClearDemoGetChallengeNum()
    {
        return g_gm_main_system.player_rest_num[0];
    }

    private static void gmClearDemoSetBgColorBlack()
    {
        gmClearDemoSetBgColorBlack(null);
    }

    private static void gmClearDemoSetBgColorBlack(AMS_TCB tcb)
    {
        if (tcb != null)
            amDrawSetBGColor(new NNS_RGBA_U8(0, 0, 0, byte.MaxValue));
        else
            amDrawMakeTask(new TaskProc(gmClearDemoSetBgColorBlack), 65280);
    }
}