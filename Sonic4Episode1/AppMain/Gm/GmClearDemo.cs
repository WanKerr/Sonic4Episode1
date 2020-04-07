using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using er;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static bool GmClearDemoBuildCheck()
    {
        return AppMain.gmClearDemoIsTexLoad();
    }

    private static void GmClearDemoBuild()
    {
        int language = AppMain.GsEnvGetLanguage();
        int num = language < 1 ? 0 : 2 * language;
        AppMain.gm_clrdm_mgr.Clear();
        AppMain.gm_clrdm_mgr_p = AppMain.gm_clrdm_mgr;
        for (int index = 0; index < 2; ++index)
            AppMain.gm_clrdm_tex[index].Clear();
        AppMain.gm_clrdm_amb[0] = AppMain.readAMBFile(AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 31, AppMain.g_gm_gamedat_cockpit_main_arc));
        AppMain.gm_clrdm_amb[1] = AppMain.readAMBFile(AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, AppMain.g_gm_clear_demo_data_amb_id[AppMain.GsEnvGetLanguage()], AppMain.g_gm_gamedat_cockpit_main_arc));
        for (int index = 0; index < 2; ++index)
        {
            AppMain.AoTexBuild(AppMain.gm_clrdm_tex[index], AppMain.gm_clrdm_amb[index]);
            AppMain.AoTexLoad(AppMain.gm_clrdm_tex[index]);
        }
    }

    private static void GmClearDemoFlush()
    {
        for (int index = 0; index < 2; ++index)
            AppMain.AoTexRelease(AppMain.gm_clrdm_tex[index]);
    }

    private static bool GmClearDemoFlushCheck()
    {
        return AppMain.gmClearDemoIsTexRelease();
    }

    private static void GmClearDemoStart()
    {
        AppMain.gmClearDemoInit();
    }

    private static void GmClearDemoExit()
    {
        if (AppMain.gm_clrdm_mgr_p.tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_clrdm_mgr_p.tcb);
        AppMain.gm_clrdm_mgr_p.tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static bool GmClearDemoIsExit()
    {
        return AppMain.gm_clrdm_mgr_p.tcb == null;
    }

    private static void GmClearDemoRetryStart()
    {
        AppMain.gmClearDemoRetryInit();
    }

    private static void gmClearDemoInit()
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        AppMain.MTS_TASK_TCB mtsTaskTcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmClearDemoProcMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmClearDemoDest), 0U, (ushort)0, 18448U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_CLRDM_MAIN_WORK()), "CLRDM_MAIN");
        AppMain.gm_clrdm_mgr_p.tcb = mtsTaskTcb;
        AppMain.GMS_CLRDM_MAIN_WORK work = (AppMain.GMS_CLRDM_MAIN_WORK)mtsTaskTcb.work;
        for (int index = 0; index < 2; ++index)
            work.tex[index] = AppMain.gm_clrdm_tex[index];
        work.trg_retry.Constructor();
        work.trg_back.Constructor();
        work.count = 1;
        work.stage_id = mainSysInfo.stage_id;
        work.game_mode = mainSysInfo.game_mode;
        if (work.stage_id <= (ushort)20)
        {
            work.is_clear_spe_stg = false;
            AppMain.gmClearDemoSetPlayGameScore(work);
        }
        else
        {
            work.is_clear_spe_stg = true;
            AppMain.gmClearDemoSetSpecialStageScore(work);
            AppMain.gmClearDemoSetSpecialStageClearInfo(work);
        }
        AppMain.gmClearDemoSetSaveScoreData(work);
        if (work.stage_id >= (ushort)21 && work.stage_id < (ushort)29)
        {
            if (work.game_mode == 0)
            {
                AppMain.gmClearDemoCreateObjSpeScore(work);
                AppMain.gmClearDemoCreateObjSpecialScoreAtk(work);
                AppMain.gmClearDemoSetInitDispAct(work);
            }
            else
            {
                AppMain.gmClearDemoCreateObjSpeTime(work);
                AppMain.gmClearDemoCreateObjSpecialTimeAtk(work);
            }
        }
        else if (work.game_mode == 0)
        {
            AppMain.gmClearDemoCreateObjActScore(work);
            AppMain.gmClearDemoCreateObjAct(work);
            AppMain.gmClearDemoSetInitDispAct(work);
        }
        else
        {
            AppMain.gmClearDemoCreateObjActTime(work);
            AppMain.gmClearDemoCreateObjNormalTimeAtk(work);
        }
        if (mainSysInfo.stage_id >= (ushort)16 && mainSysInfo.stage_id <= (ushort)20)
            AppMain.GmSoundPlayClearFinal();
        else
            AppMain.GmSoundPlayClear();
        AppMain.gmClearDemoSetMainUpdateProc(work);
    }

    private static void gmClearDemoRetryInit()
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        AppMain.MTS_TASK_TCB mtsTaskTcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmClearDemoProcMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmClearDemoDest), 0U, (ushort)0, 18448U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_CLRDM_MAIN_WORK()), "CLRDM_MAIN");
        AppMain.gm_clrdm_mgr_p.tcb = mtsTaskTcb;
        AppMain.GMS_CLRDM_MAIN_WORK work = (AppMain.GMS_CLRDM_MAIN_WORK)mtsTaskTcb.work;
        for (int index = 0; index < 2; ++index)
            work.tex[index] = AppMain.gm_clrdm_tex[index];
        work.trg_retry.Constructor();
        work.trg_back.Constructor();
        work.count = 1;
        work.stage_id = mainSysInfo.stage_id;
        work.is_clear_spe_stg = work.stage_id >= (ushort)21;
        work.game_mode = mainSysInfo.game_mode;
        AppMain.gmClearDemoCreateObjActTime(work);
        AppMain.gmClearDemoCreateObjNormalTimeAtk(work);
        AppMain.gmClearDemoSetRetryInitAct(work);
        work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcRetryStart);
    }

    private static void gmClearDemoSetMainUpdateProc(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.stage_id >= (ushort)21)
        {
            if (main_work.game_mode == 0)
            {
                main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeScoreMoveEfct);
            }
            else
            {
                main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeTimeMoveEfct);
                AppMain.gmClearDemoSetClearTimeRecord(main_work);
                AppMain.gmClearDemoSetSortBufTimeAct(main_work);
                main_work.tex_new_record_act.obj_2d.speed = 0.0f;
            }
        }
        else if (main_work.game_mode == 0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcMoveEfct);
        }
        else
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcTimeMoveEfct);
            AppMain.gmClearDemoSetClearTimeRecord(main_work);
            AppMain.gmClearDemoSetSortBufTimeAct(main_work);
            main_work.tex_new_record_act.obj_2d.speed = 0.0f;
        }
    }

    private static void gmClearDemoSetPlayGameScore(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        mainSysInfo.clear_score = AppMain.gmClearDemoGetScore();
        mainSysInfo.clear_time = (int)AppMain.gmClearDemoGetGameTime();
        mainSysInfo.clear_ring = (uint)AppMain.gmClearDemoGetRingNum();
        main_work.time_score[0] = 0U;
        main_work.time_score[1] = AppMain.gmClearDemoGetTimeScore(mainSysInfo.clear_time);
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

    private static void gmClearDemoSetSaveScoreData(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        uint high_score = main_work.total_score[0] < 10U ? 0U : main_work.total_score[0];
        SSystem instance1 = SSystem.CreateInstance();
        SStage stage = SStage.CreateInstance();
        SSpecial special = SSpecial.CreateInstance();
        if (main_work.stage_id >= (ushort)21)
        {
            uint num = (uint)main_work.stage_id - 21U;
            AppMain.GMM_MAIN_GOAL_AS_SUPER_SONIC();
            if (mainSysInfo.game_mode == 0)
            {
                if (((int)AppMain.g_gm_main_system.game_flag & 65536) != 0 && !main_work.is_first_spe_clear)
                    AppMain.GmPlayerStockGet(AppMain.g_gm_main_system.ply_work[0], (short)1);
                bool is_uploaded = LiveFeature.getInstance().saveLeaderBoardScore((int)main_work.stage_id - 4, (int)high_score);
                special[(int)num].SetHighScoreUploaded(is_uploaded);
                if (is_uploaded && !special[(int)num].IsScoreUploadedOnce())
                    special[(int)num].SetScoreUploadedOnce(true);

            }
            else if (mainSysInfo.game_mode == 1)
            {
                if (((int)AppMain.g_gm_main_system.game_flag & 65536) != 0 && !main_work.is_first_spe_clear)
                    AppMain.GmPlayerStockGet(AppMain.g_gm_main_system.ply_work[0], (short)1);
                bool is_uploaded = false;
                if (((int)AppMain.g_gm_main_system.game_flag & 65536) != 0)
                {
                    is_uploaded = LiveFeature.getInstance().saveLeaderBoardScore((int)main_work.stage_id - 4 + 24, mainSysInfo.clear_time);
                    special[(int)num].SetFastTimeUploaded(is_uploaded);
                }
                if (is_uploaded && !special[(int)num].IsTimeUploadedOnce())
                    special[(int)num].SetTimeUploadedOnce(true);
            }

            if ((special[(int)num].GetHighScore() < high_score || special[(int)num].GetHighScore() == 0) && ((int)AppMain.g_gm_main_system.game_flag & 65536) != 0)
            {
                special[(int)num].SetHighScore(high_score);
                main_work.flag |= 32768U;
            }

            if ((special[(int)num].GetFastTime() < (uint)mainSysInfo.clear_time || special[(int)num].GetFastTime() == 0) && ((int)AppMain.g_gm_main_system.game_flag & 65536) != 0)
            {
                special[(int)num].SetFastTime((uint)mainSysInfo.clear_time);
                main_work.flag |= 4096U;
            }
        }
        else
        {
            uint num = (uint)main_work.stage_id;
            if (num > 16U && num <= 20U)
                num = 16U;
            bool flag = AppMain.GMM_MAIN_USE_SUPER_SONIC();

            if (AppMain.GMM_MAIN_GOAL_AS_SUPER_SONIC())
                stage[(int)num].SetUseSuperSonicOnce(true);
            if (mainSysInfo.game_mode == 0)
            {
                high_score = main_work.total_score[0] < 10U ? 0U : main_work.total_score[0];
                AppMain.g_gs_main_sys_info.is_first_play = stage[(int)num].GetHighScore(false) == 0;
                bool is_uploaded = LiveFeature.getInstance().saveLeaderBoardScore((int)num, (int)high_score);
                stage[(int)num].SetHighScoreUploaded(flag, is_uploaded);
                if (is_uploaded && !stage[(int)num].IsScoreUploadedOnce())
                    stage[(int)num].SetScoreUploadedOnce(true);
                AppMain.g_gs_main_sys_info.prev_stage_id = ((int)AppMain.g_gm_main_system.game_flag & 16384) == 0 ? ushort.MaxValue : AppMain.g_gs_main_sys_info.stage_id;
            }
            else if (mainSysInfo.game_mode == 1 && (stage[(int)num].GetFastTime(flag) > (uint)mainSysInfo.clear_time || stage[(int)num].GetFastTime(flag) == 0))
            {
                bool is_uploaded = LiveFeature.getInstance().saveLeaderBoardScore((int)num + 24, mainSysInfo.clear_time);
                stage[(int)num].SetFastTimeUploaded(flag, is_uploaded);
                if (is_uploaded && !stage[(int)num].IsTimeUploadedOnce())
                    stage[(int)num].SetTimeUploadedOnce(true);
            }

            if (flag)
            {
                if (stage[(int)num].GetFastTime(false) > (uint)mainSysInfo.clear_time || stage[(int)num].GetFastTime(false) == 0)
                    main_work.flag |= 4096U;
            }
            else if (stage[(int)num].GetFastTime(true) > (uint)mainSysInfo.clear_time || stage[(int)num].GetFastTime(true) == 0)
                main_work.flag |= 4096U;
            stage[(int)num].SetFastTime((uint)mainSysInfo.clear_time, flag);

            if (stage[(int)num].GetHighScore(flag) < high_score || stage[(int)num].GetHighScore(flag) == 0)
                stage[(int)num].SetHighScore(high_score, flag);
        }


        instance1.SetKilled(mainSysInfo.ene_kill_count);
        instance1.SetPlayerStock(mainSysInfo.rest_player_num);
        if (mainSysInfo.stage_id >= (ushort)16 && mainSysInfo.stage_id <= (ushort)20)
        {
            AppMain.HgTrophyIncFinalClearCount();
            instance1.SetClearCount(mainSysInfo.final_clear_count);
        }
        AppMain.HgTrophyTryAcquisition(4);
    }

    private static void gmClearDemoSetClearTimeRecord(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        main_work.clear_time = (uint)mainSysInfo.clear_time;
        AppMain.AkUtilFrame60ToTime(main_work.clear_time, ref main_work.time_min, ref main_work.time_sec, ref main_work.time_msec);
        main_work.record_time_num_act[0].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[2].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[3].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[5].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[6].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[1].obj_2d.speed = 0.0f;
        main_work.record_time_num_act[4].obj_2d.speed = 0.0f;
        for (int index = 0; index < 7; ++index)
            ((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index]).disp_flag &= 4294967263U;
    }

    private static void gmClearDemoSetSpecialStageScore(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        mainSysInfo.clear_score = AppMain.gmClearDemoGetScore();
        mainSysInfo.clear_time = (int)AppMain.gmClearDemoGetGameTime();
        mainSysInfo.clear_ring = (uint)AppMain.gmClearDemoGetRingNum();
        main_work.time_score[0] = 0U;
        main_work.time_score[1] = AppMain.gmClearDemoGetTimeSpeScore(mainSysInfo.clear_time);
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

    private static void gmClearDemoSetSpecialStageClearInfo(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        ushort num1 = AppMain.g_gs_main_sys_info.prev_stage_id == ushort.MaxValue ? ushort.MaxValue : AppMain.g_gs_main_sys_info.prev_stage_id;
        SSpecial instance = SSpecial.CreateInstance();
        uint num2 = (uint)main_work.stage_id - 21U;
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 32) != 0)
            main_work.is_full_eme = true;
        if (((int)AppMain.g_gm_main_system.game_flag & 65536) != 0)
        {
            main_work.is_get_eme = true;
            if (!instance[(int)num2].IsGetEmerald() && num1 != ushort.MaxValue)
            {
                instance[(int)num2].SetEmeraldStage((EEmeraldStage.Type)AppMain.dm_clrdm_spe_stg_get_act_no[(int)num1]);
                main_work.is_first_spe_clear = true;
            }
            main_work.get_eme_no = (int)num2;
            if (AppMain.g_gs_main_sys_info.stage_id == (ushort)27)
                AppMain.g_gs_main_sys_info.game_flag |= 32U;
            main_work.has_eme_num = (int)num2 + 1;
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
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        uint num1 = 0;
        ushort min = 0;
        ushort sec = 0;
        ushort msec = 0;
        AppMain.AkUtilFrame60ToTime((uint)clear_time, ref min, ref sec, ref msec);
        int num2 = (int)min * 60 + (int)sec;
        for (int index = 0; index < 9; ++index)
        {
            if (num2 < AppMain.dm_clrdm_clear_time_sec_tbl[(int)mainSysInfo.stage_id][index])
            {
                num1 = AppMain.dm_clrdm_clear_time_score_tbl[index];
                break;
            }
        }
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 8) != 0)
            num1 = 0U;
        return num1;
    }

    private static uint gmClearDemoGetTimeSpeScore(int clear_time)
    {
        ushort min = 0;
        ushort sec = 0;
        ushort msec = 0;
        AppMain.AkUtilFrame60ToTime((uint)clear_time, ref min, ref sec, ref msec);
        uint num = (uint)(((int)min * 60 + (int)sec) * 100);
        if (((int)AppMain.g_gm_main_system.game_flag & 131072) != 0)
            num = 0U;
        if (((int)AppMain.g_gm_main_system.game_flag & 262144) != 0)
            num = 0U;
        return num;
    }

    private static void gmClearDemoSetInitDispAct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        for (int index = 0; index < 5; ++index)
        {
            ((AppMain.OBS_OBJECT_WORK)main_work.time_num_act[index]).disp_flag |= 32U;
            ((AppMain.OBS_OBJECT_WORK)main_work.ring_num_act[index]).disp_flag |= 32U;
        }
        for (int index = 0; index < 9; ++index)
            ((AppMain.OBS_OBJECT_WORK)main_work.total_num_act[index]).disp_flag |= 32U;
        for (int index = 0; index < 3; ++index)
            ((AppMain.OBS_OBJECT_WORK)main_work.line_act[index]).disp_flag |= 32U;
        ((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act).disp_flag |= 32U;
        ((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act2).disp_flag |= 32U;
    }

    private static void gmClearDemoProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_CLRDM_MAIN_WORK work = (AppMain.GMS_CLRDM_MAIN_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
        {
            if (AppMain.gm_clrdm_mgr_p.tcb != null)
            {
                AppMain.mtTaskClearTcb(AppMain.gm_clrdm_mgr_p.tcb);
                AppMain.gm_clrdm_mgr_p.tcb = (AppMain.MTS_TASK_TCB)null;
            }
            if (work.stage_id < (ushort)21)
            {
                if (((int)AppMain.g_gm_main_system.game_flag & 16384) != 0)
                {
                    AppMain.g_gs_main_sys_info.prev_stage_id = AppMain.g_gs_main_sys_info.stage_id;
                    AppMain.DmCmnBackupSave(false, false, false);
                }
                else
                    AppMain.g_gs_main_sys_info.prev_stage_id = ushort.MaxValue;
            }
            if (((int)work.flag & 16384) != 0)
                work.flag &= 4294950911U;
            AppMain.g_gm_main_system.game_flag |= 16U;
            if (((int)work.flag & 8192) == 0)
                return;
            AppMain.g_gm_main_system.game_flag |= 2U;
            work.flag &= 4294959103U;
        }
        else
        {
            if (work.proc_update == null)
                return;
            work.proc_update(work);
        }
    }

    private static void gmClearDemoDest(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static void gmClearDemoProcMoveEfct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.AoActIsEndTrs(main_work.tex_total_act.obj_2d.act))
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcPrevCalcScore);
            AppMain.amFlagOn(ref main_work.flag, 32U);
            AppMain.amFlagOn(ref main_work.flag, 512U);
        }
        if (AppMain.AoActIsEndTrs(main_work.tex_time_act.obj_2d.act))
            AppMain.amFlagOn(ref main_work.flag, 128U);
        if (AppMain.AoActIsEndTrs(main_work.tex_ring_act.obj_2d.act))
            AppMain.amFlagOn(ref main_work.flag, 256U);
        AppMain.gmClearDemoSetSortBufAct(main_work);
        AppMain.gmClearDemoSetScoreData(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcPrevCalcScore(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += (float)main_work.count;
        if ((double)main_work.timer > 150.0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcCalcScore);
            main_work.proc_calc_score = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoSetCalcScore);
            main_work.timer = 0.0f;
        }
        if (AppMain.amTpIsTouchPush(0) || AppMain.isBackKeyPressed())
        {
            AppMain.setBackKeyRequest(false);
            AppMain.amFlagOn(ref main_work.flag, 4U);
        }
        AppMain.gmClearDemoSetSortBufAct(main_work);
        AppMain.gmClearDemoSetScoreData(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcCalcScore(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += (float)main_work.count;
        if (((int)main_work.flag & 4) != 0 || ((int)main_work.flag & 8) != 0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcWaitDispSonic);
            main_work.time_score[1] = main_work.time_score[0];
            main_work.ring_score[1] = main_work.ring_score[0];
            main_work.total_score[1] = main_work.total_score[0];
            main_work.proc_calc_score = (AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate)null;
            AppMain.amFlagOff(ref main_work.flag, 4U);
            AppMain.amFlagOff(ref main_work.flag, 8U);
            if (main_work.total_score[1] >= 10000U)
            {
                AppMain.amFlagOn(ref main_work.flag, 16U);
                main_work.idle_time = 270;
            }
            else
                main_work.idle_time = 120;
            AppMain.HgTrophyTryAcquisition(0);
            main_work.timer = 0.0f;
        }
        if (AppMain.amTpIsTouchPush(0) || AppMain.isBackKeyPressed())
        {
            AppMain.setBackKeyRequest(false);
            AppMain.amFlagOn(ref main_work.flag, 4U);
        }
        if (main_work.proc_calc_score != null && (double)main_work.timer > 1.0)
        {
            main_work.proc_calc_score(main_work);
            main_work.timer = 0.0f;
        }
        AppMain.gmClearDemoSetSortBufAct(main_work);
        AppMain.gmClearDemoSetScoreData(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
        if (main_work.proc_calc_score == null)
        {
            AppMain.GmSoundPlaySE("Result2");
        }
        else
        {
            if (0.0 != (double)main_work.timer)
                return;
            AppMain.GmSoundPlaySE("Result1");
        }
    }

    private static void gmClearDemoProcWaitDispSonic(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if ((double)main_work.timer >= 60.0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcDispIdle);
            main_work.timer = 0.0f;
            if (((int)main_work.flag & 16) == 0)
                return;
            AppMain.GmSoundPlayJingle(0U);
            AppMain.GmPlayerStockGet(AppMain.g_gm_main_system.ply_work[0], (short)1);
        }
        else
        {
            AppMain.gmClearDemoSetSortBufAct(main_work);
            AppMain.gmClearDemoSetScoreData(main_work);
            AppMain.gmClearDemoUpdateAct(main_work);
            main_work.timer += (float)main_work.count;
        }
    }

    private static void gmClearDemoProcDispIdle(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if ((double)main_work.timer >= (double)main_work.idle_time)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcFadeOut);
            AppMain.IzFadeInitEasy(0U, 1U, 32f);
        }
        if (((int)main_work.flag & 16) != 0)
            AppMain.gmClearDemoSetFlashSonic(main_work);
        AppMain.gmClearDemoSetSortBufAct(main_work);
        AppMain.gmClearDemoSetScoreData(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
        main_work.timer += (float)main_work.count;
    }

    private static void gmClearDemoProcFadeOut(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.IzFadeIsEnd())
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcFinish);
            main_work.timer = 0.0f;
        }
        else
        {
            if (((int)main_work.flag & 16) != 0)
                AppMain.gmClearDemoSetFlashSonic(main_work);
            if (main_work.game_mode == 0)
            {
                AppMain.gmClearDemoSetSortBufAct(main_work);
                AppMain.gmClearDemoSetScoreData(main_work);
                AppMain.gmClearDemoUpdateAct(main_work);
            }
            main_work.timer += (float)main_work.count;
        }
    }

    private static void gmClearDemoProcFinish(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.proc_update = (AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate)null;
        main_work.trg_retry.Release();
        main_work.trg_retry.Destructor();
        main_work.trg_back.Release();
        main_work.trg_back.Destructor();
        AppMain.amFlagOn(ref main_work.flag, 1U);
    }

    private static void gmClearDemoProcTimeMoveEfct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.AoActIsEndTrs(main_work.tex_big_time_act.obj_2d.act))
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcTimeWaitTimeEfct);
            for (int index = 0; index < 7; ++index)
            {
                AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index], main_work.record_time_num_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(49 + index), 0);
                ((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index]).scale.x = 4096;
                ((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index]).scale.y = 4096;
            }
            AppMain.gmClearDemoSetClearTimeRecord(main_work);
            AppMain.gmClearDemoSetSortBufTimeAct(main_work);
            AppMain.amFlagOn(ref main_work.flag, 1024U);
            AppMain.amFlagOn(ref main_work.flag, 2048U);
        }
        AppMain.gmClearDemoSetTimeAtkSortBufAct(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcTimeWaitTimeEfct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if ((double)main_work.timer > 60.0)
        {
            if (((int)main_work.flag & 4096) != 0)
            {
                main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcTimeMoveNewRecord);
                main_work.tex_new_record_act.obj_2d.speed = 1f;
                main_work.idle_time = 180;
            }
            else
            {
                main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcTimeDispEffect);
                main_work.idle_time = 120;
            }
            AppMain.HgTrophyTryAcquisition(0);
            main_work.timer = 0.0f;
        }
        AppMain.gmClearDemoSetTimeAtkSortBufAct(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
        if ((double)main_work.timer > 120.0)
            return;
        main_work.timer += (float)main_work.count;
    }

    private static void gmClearDemoProcTimeMoveNewRecord(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.AoActIsEndTrs(main_work.tex_new_record_act.obj_2d.act))
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcTimeDispEffect);
            AppMain.GmSoundPlayJingle(5U);
        }
        AppMain.gmClearDemoSetTimeAtkSortBufAct(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcTimeDispEffect(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += (float)main_work.count;
        if (AppMain.amTpIsTouchPush(0))
            main_work.flag |= 4U;
        if ((double)main_work.timer > (double)main_work.idle_time || ((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcChangeRetryOut);
            AppMain.IzFadeInitEasy(0U, 1U, 32f);
            main_work.timer = 0.0f;
            main_work.flag &= 4294967291U;
        }
        AppMain.gmClearDemoTimeFlushEffect(main_work);
        AppMain.gmClearDemoSetTimeAtkSortBufAct(main_work);
    }

    private static void gmClearDemoProcRetryStart(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcChangeRetryOut);
        AppMain.IzFadeInitEasy(0U, 1U, 32f);
        main_work.timer = 0.0f;
    }

    private static void gmClearDemoProcChangeRetryOut(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.IzFadeIsEnd())
        {
            if (main_work.nodisp_check)
            {
                AppMain.IzFadeInitEasy(0U, 0U, 32f);
                main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcChangeRetryIn);
                main_work.nodisp_check = false;
                return;
            }
            AppMain.gmClearDemoSetRetryDispInfo(main_work);
            AppMain.gmClearDemoSetRetrySortBufAct(main_work);
            main_work.nodisp_check = true;
        }
        AppMain.gmClearDemoSetTimeAtkSortBufAct(main_work);
    }

    private static void gmClearDemoProcChangeRetryIn(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcWaitSelectRetry);
    }

    private static void gmClearDemoProcWaitSelectRetry(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.gmClearDemoSetBgColorBlack();
        AppMain.gmClearDemoSetRetryInput(main_work);
        if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcWaitRetrySonicRunEfct);
            AppMain.GmPlySeqChangeTRetryAcc(AppMain.g_gm_main_system.ply_work[0]);
        }
        else
        {
            if (((int)main_work.flag & 2) == 0)
                return;
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcFadeOut);
            AppMain.IzFadeInitEasy(0U, 1U, 32f);
        }
    }

    private static void gmClearDemoProcWaitRetrySonicRunEfct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.gmClearDemoSetBgColorBlack();
        if (AppMain.ObjObjectViewOutCheck(AppMain.g_gm_main_system.ply_work[0].obj_work) == 0)
            return;
        main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcFadeOut);
        main_work.flag |= 8192U;
        AppMain.IzFadeInitEasy(0U, 1U, 32f);
    }

    private static void gmClearDemoSetRetryInput(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.isBackKeyPressed())
        {
            main_work.flag |= 2U;
            AppMain.setBackKeyRequest(false);
            AppMain.GmSoundPlaySE("Ok");
        }
        else
        {
            CTrgAoAction trgRetry = main_work.trg_retry;
            trgRetry.Update();
            if (trgRetry.GetState(0U)[10] && trgRetry.GetState(0U)[1])
            {
                main_work.flag |= 4U;
                int index1 = 0;
                for (int index2 = AppMain.arrayof((Array)main_work.btn_retry); index1 < index2; ++index1)
                {
                    main_work.btn_retry[index1].obj_2d.frame = 2f;
                    main_work.btn_retry[index1].obj_2d.speed = 1f;
                }
                AppMain.GmSoundPlaySE("Ok");
            }
            else if (trgRetry.GetState(0U)[0])
            {
                int index1 = 0;
                for (int index2 = AppMain.arrayof((Array)main_work.btn_retry); index1 < index2; ++index1)
                    main_work.btn_retry[index1].obj_2d.frame = 1f;
            }
            else if (trgRetry.GetState(0U)[13])
            {
                int index1 = 0;
                for (int index2 = AppMain.arrayof((Array)main_work.btn_retry); index1 < index2; ++index1)
                    main_work.btn_retry[index1].obj_2d.frame = 0.0f;
            }
            CTrgAoAction trgBack = main_work.trg_back;
            trgBack.Update();
            if (trgBack.GetState(0U)[10] && trgBack.GetState(0U)[1])
            {
                main_work.flag |= 2U;
                int index1 = 0;
                for (int index2 = AppMain.arrayof((Array)main_work.btn_back); index1 < index2; ++index1)
                {
                    main_work.btn_back[index1].obj_2d.frame = 2f;
                    main_work.btn_back[index1].obj_2d.speed = 1f;
                }
                AppMain.GmSoundPlaySE("Ok");
            }
            else if (trgBack.GetState(0U)[2])
            {
                int index1 = 0;
                for (int index2 = AppMain.arrayof((Array)main_work.btn_back); index1 < index2; ++index1)
                    main_work.btn_back[index1].obj_2d.frame = 1f;
            }
            else
            {
                if (!trgBack.GetState(0U)[13])
                    return;
                int index1 = 0;
                for (int index2 = AppMain.arrayof((Array)main_work.btn_back); index1 < index2; ++index1)
                    main_work.btn_back[index1].obj_2d.frame = 0.0f;
            }
        }
    }

    private static void gmClearDemoSetRetryDispInfo(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.g_gm_main_system.ply_work[0].player_flag |= 65536U;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        AppMain.GmSoundStopStageBGM(64);
        AppMain.GmSoundStopJingle(64);
        AppMain.GmSoundStopBGMJingle(64);
        AppMain.GsSoundStopSe();
        AppMain.GmMapSetDisp(false);
        AppMain.GmFixSetDisp(false);
        AppMain.GmObjSetAllObjectNoDisp();
        AppMain.GmRingGetWork().flag |= 1U;
        AppMain.g_gm_main_system.game_flag |= 8192U;
        AppMain.g_gm_main_system.water_level = ushort.MaxValue;
        AppMain.g_gm_main_system.game_flag &= 4294964223U;
        gmsPlayerWork.obj_work.flag &= 4294967167U;
        gmsPlayerWork.obj_work.disp_flag &= 4294967263U;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        AppMain.ObjCameraSetUserFunc(0, new AppMain.OBJF_CAMERA_USER_FUNC(AppMain.GmCameraFunc));
        AppMain.GmCameraScaleSet(1f, 1f);
        obsCamera.roll = 0;
        gmsPlayerWork.gmk_flag &= 4294934527U;
        gmsPlayerWork.obj_work.pos.x = AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.x);
        gmsPlayerWork.obj_work.pos.y = AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.y * -1f);
        AppMain.GmCamScrLimitSetDirect(new AppMain.GMS_EVE_RECORD_EVENT()
        {
            flag = (ushort)15,
            left = (sbyte)-96,
            top = (sbyte)-85,
            width = (byte)192,
            height = (byte)112
        }, AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.x), AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.y * -1f));
        AppMain.GmPlySeqChangeTRetryFw(AppMain.g_gm_main_system.ply_work[0]);
        AppMain.GmObjSetObjectNoFunc(4U);
        AppMain.GmObjSetObjectNoFunc(8U);
        AppMain.GmObjSetObjectNoFunc(16U);
        AppMain.GmObjSetObjectNoFunc(32U);
        AppMain.GmObjSetObjectNoFunc(64U);
        AppMain.GmObjSetObjectNoFunc(128U);
        AppMain.GmObjSetObjectNoFunc(256U);
        AppMain.GMM_PAD_VIB_STOP();
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_retry_act).disp_flag &= 4294967263U;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_back_slct_act).disp_flag &= 4294967263U;
        ((AppMain.OBS_OBJECT_WORK)main_work.bg_retry).disp_flag &= 4294967263U;
        AppMain.amFlagOff(ref main_work.flag, 1024U);
        main_work.flag &= 4294966271U;
        for (int index = 0; index < AppMain.arrayof((Array)main_work.record_time_num_act); ++index)
            ((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index]).disp_flag |= 32U;
        int index1 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_retry); index1 < index2; ++index1)
            ((AppMain.OBS_OBJECT_WORK)main_work.btn_retry[index1]).disp_flag &= 4294967263U;
        int index3 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_back); index3 < index2; ++index3)
            ((AppMain.OBS_OBJECT_WORK)main_work.btn_back[index3]).disp_flag &= 4294967263U;
        AppMain.ObjDrawClearNNCommandStateTbl();
        AppMain.ObjDrawSetNNCommandStateTbl(0U, 1U, false);
        AppMain.ObjDrawSetNNCommandStateTbl(1U, 2U, false);
        AppMain.ObjDrawSetNNCommandStateTbl(2U, 3U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(3U, 5U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(4U, 11U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(5U, 12U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(6U, 9U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(7U, 4U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(8U, 8U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(9U, 7U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(10U, 10U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(11U, 6U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(12U, 0U, true);
    }

    private static void gmClearDemoProcSpeScoreMoveEfct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.AoActIsEndTrs(main_work.tex_total_act.obj_2d.act))
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeScorePrevCalcScore);
            AppMain.amFlagOn(ref main_work.flag, 32U);
            AppMain.amFlagOn(ref main_work.flag, 512U);
        }
        if (AppMain.AoActIsEndTrs(main_work.tex_time_act.obj_2d.act))
            AppMain.amFlagOn(ref main_work.flag, 128U);
        if (AppMain.AoActIsEndTrs(main_work.tex_ring_act.obj_2d.act))
            AppMain.amFlagOn(ref main_work.flag, 256U);
        AppMain.gmClearDemoSetSortBufAct(main_work);
        AppMain.gmClearDemoSetScoreData(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcSpeScorePrevCalcScore(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += (float)main_work.count;
        if ((double)main_work.timer > 150.0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeScoreCalcScore);
            main_work.proc_calc_score = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoSetCalcScore);
            main_work.timer = 0.0f;
        }
        if (AppMain.amTpIsTouchPush(0) || AppMain.isBackKeyPressed())
        {
            AppMain.setBackKeyRequest(false);
            AppMain.amFlagOn(ref main_work.flag, 4U);
        }
        AppMain.gmClearDemoSetSortBufAct(main_work);
        AppMain.gmClearDemoSetScoreData(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcSpeScoreCalcScore(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += (float)main_work.count;
        if (((int)main_work.flag & 4) != 0 || ((int)main_work.flag & 8) != 0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeScoreWaitDispSonic);
            main_work.time_score[1] = main_work.time_score[0];
            main_work.ring_score[1] = main_work.ring_score[0];
            main_work.total_score[1] = main_work.total_score[0];
            main_work.proc_calc_score = (AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate)null;
            AppMain.amFlagOff(ref main_work.flag, 4U);
            AppMain.amFlagOff(ref main_work.flag, 8U);
            if (main_work.total_score[1] >= 10000U)
            {
                AppMain.amFlagOn(ref main_work.flag, 16U);
                main_work.idle_time = 270;
            }
            else
                main_work.idle_time = 120;
            AppMain.HgTrophyTryAcquisition(0);
            main_work.timer = 0.0f;
        }
        if (AppMain.amTpIsTouchPush(0) || AppMain.isBackKeyPressed())
        {
            AppMain.setBackKeyRequest(false);
            AppMain.amFlagOn(ref main_work.flag, 4U);
        }
        if (main_work.proc_calc_score != null && (double)main_work.timer > 1.0)
        {
            main_work.proc_calc_score(main_work);
            main_work.timer = 0.0f;
        }
        AppMain.gmClearDemoSetSortBufAct(main_work);
        AppMain.gmClearDemoSetScoreData(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
        if (main_work.proc_calc_score == null)
        {
            AppMain.GmSoundPlaySE("Result2");
        }
        else
        {
            if (0.0 != (double)main_work.timer)
                return;
            AppMain.GmSoundPlaySE("Result1");
        }
    }

    private static void gmClearDemoProcSpeScoreWaitDispSonic(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if ((double)main_work.timer >= 60.0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeScoreDispIdle);
            main_work.timer = 0.0f;
            if (((int)main_work.flag & 16) == 0)
                return;
            AppMain.GmSoundPlayJingle(0U);
            AppMain.GmPlayerStockGet(AppMain.g_gm_main_system.ply_work[0], (short)1);
        }
        else
        {
            AppMain.gmClearDemoSetSortBufAct(main_work);
            AppMain.gmClearDemoSetScoreData(main_work);
            AppMain.gmClearDemoUpdateAct(main_work);
            main_work.timer += (float)main_work.count;
        }
    }

    private static void gmClearDemoProcSpeScoreDispIdle(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if ((double)main_work.timer >= (double)main_work.idle_time)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeScoreFadeOut);
            AppMain.IzFadeInitEasy(0U, 3U, 32f);
        }
        if (((int)main_work.flag & 16) != 0)
            AppMain.gmClearDemoSetFlashSonic(main_work);
        AppMain.gmClearDemoSetSortBufAct(main_work);
        AppMain.gmClearDemoSetScoreData(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
        main_work.timer += (float)main_work.count;
    }

    private static void gmClearDemoProcSpeScoreFadeOut(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.IzFadeIsEnd())
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeScoreFinish);
            main_work.timer = 0.0f;
        }
        else
        {
            if (((int)main_work.flag & 16) != 0)
                AppMain.gmClearDemoSetFlashSonic(main_work);
            AppMain.gmClearDemoSetSortBufAct(main_work);
            AppMain.gmClearDemoSetScoreData(main_work);
            AppMain.gmClearDemoUpdateAct(main_work);
            main_work.timer += (float)main_work.count;
        }
    }

    private static void gmClearDemoProcSpeScoreFinish(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.proc_update = (AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate)null;
        AppMain.amFlagOn(ref main_work.flag, 1U);
    }

    private static void gmClearDemoProcSpeTimeMoveEfct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.AoActIsEndTrs(main_work.tex_big_time_act.obj_2d.act))
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeTimeWaitTimeEfct);
            for (int index = 0; index < 7; ++index)
            {
                AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index], main_work.record_time_num_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(49 + index), 0);
                ((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index]).scale.x = 4096;
                ((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index]).scale.y = 4096;
            }
            AppMain.gmClearDemoSetClearTimeRecord(main_work);
            AppMain.gmClearDemoSetSortBufTimeAct(main_work);
            AppMain.amFlagOn(ref main_work.flag, 1024U);
            AppMain.amFlagOn(ref main_work.flag, 2048U);
        }
        AppMain.gmClearDemoSetTimeAtkSortBufAct(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcSpeTimeWaitTimeEfct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if ((double)main_work.timer > 60.0)
        {
            if (((int)main_work.flag & 4096) != 0)
            {
                main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeTimeTimeMoveNewRecord);
                main_work.tex_new_record_act.obj_2d.speed = 1f;
                main_work.idle_time = 180;
            }
            else
            {
                main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeTimeDispEffect);
                main_work.idle_time = 120;
            }
            AppMain.HgTrophyTryAcquisition(0);
            main_work.timer = 0.0f;
        }
        AppMain.gmClearDemoSetTimeAtkSortBufAct(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
        if ((double)main_work.timer > 120.0)
            return;
        main_work.timer += (float)main_work.count;
    }

    private static void gmClearDemoProcSpeTimeTimeMoveNewRecord(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.AoActIsEndTrs(main_work.tex_new_record_act.obj_2d.act))
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeTimeDispEffect);
            AppMain.GmSoundPlayJingle(5U);
        }
        AppMain.gmClearDemoSetTimeAtkSortBufAct(main_work);
        AppMain.gmClearDemoUpdateAct(main_work);
    }

    private static void gmClearDemoProcSpeTimeDispEffect(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        main_work.timer += (float)main_work.count;
        if ((AoPad.AoPadStand() & ControllerConsts.CONFIRM) != 0)
            main_work.flag |= 4U;
        if ((double)main_work.timer > (double)main_work.idle_time || ((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeTimeChangeRetryOut);
            AppMain.IzFadeInitEasy(0U, 1U, 32f);
            main_work.timer = 0.0f;
            main_work.flag &= 4294967291U;
        }
        AppMain.gmClearDemoTimeFlushEffect(main_work);
        AppMain.gmClearDemoSetTimeAtkSortBufAct(main_work);
    }

    private static void gmClearDemoProcSpeTimeChangeRetryOut(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (AppMain.IzFadeIsEnd())
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeTimeChangeRetryIn);
            AppMain.IzFadeInitEasy(0U, 0U, 32f);
            AppMain.gmClearDemoSetRetryDispInfo(main_work);
            AppMain.gmClearDemoSetRetrySortBufAct(main_work);
            main_work.timer = 0.0f;
        }
        else
            AppMain.gmClearDemoSetTimeAtkSortBufAct(main_work);
    }

    private static void gmClearDemoProcSpeTimeChangeRetryIn(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcSpeTimeWaitSelectRetry);
    }

    private static void gmClearDemoProcSpeTimeWaitSelectRetry(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.gmClearDemoSetBgColorBlack();
        AppMain.gmClearDemoSetRetryInput(main_work);
        if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcWaitRetrySonicRunEfct);
            AppMain.GmPlySeqChangeTRetryAcc(AppMain.g_gm_main_system.ply_work[0]);
        }
        else
        {
            if (((int)main_work.flag & 2) == 0)
                return;
            main_work.proc_update = new AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate(AppMain.gmClearDemoProcFadeOut);
            AppMain.IzFadeInitEasy(0U, 1U, 32f);
        }
    }

    private static void gmClearDemoCreateObjActScore(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.TaskWorkFactoryDelegate work_size = (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK());
        AppMain.OBS_OBJECT_WORK obsObjectWork;
        for (int index = 0; index < 5; ++index)
        {
            AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_UPTEXT");
            main_work.tex_up_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work1;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TIMENUM");
            main_work.time_num_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work2;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.OBS_OBJECT_WORK work3 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_RINGNUM");
            main_work.ring_num_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work3;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        for (int index = 0; index < 9; ++index)
        {
            AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TOTALNUM");
            main_work.total_num_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        for (int index = 0; index < 3; ++index)
        {
            AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_LINE");
            main_work.line_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.OBS_OBJECT_WORK work4 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TIMETEXT");
        main_work.tex_time_act = (AppMain.GMS_COCKPIT_2D_WORK)work4;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work5 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_RINGTEXT");
        main_work.tex_ring_act = (AppMain.GMS_COCKPIT_2D_WORK)work5;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work6 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TOTALTEXT");
        main_work.tex_total_act = (AppMain.GMS_COCKPIT_2D_WORK)work6;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work7 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_SONICICON");
        main_work.sonic_icon_act = (AppMain.GMS_COCKPIT_2D_WORK)work7;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work8 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_SONICICON2");
        main_work.sonic_icon_act2 = (AppMain.GMS_COCKPIT_2D_WORK)work8;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmClearDemoCreateObjActTime(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.TaskWorkFactoryDelegate work_size = (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK());
        AppMain.OBS_OBJECT_WORK obsObjectWork;
        for (int index = 0; index < 5; ++index)
        {
            AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_UPTEXT");
            main_work.tex_up_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TIMEATK_TEXT");
        main_work.tex_big_time_act = (AppMain.GMS_COCKPIT_2D_WORK)work1;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        for (int index = 0; index < 7; ++index)
        {
            AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TIME_DIGIT_NUM");
            main_work.record_time_num_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work2;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.OBS_OBJECT_WORK work3 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TIMEATK_SONIC");
        main_work.time_sonic_icon_act = (AppMain.GMS_COCKPIT_2D_WORK)work3;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work4 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_NEWRECORD_TEXT");
        main_work.tex_new_record_act = (AppMain.GMS_COCKPIT_2D_WORK)work4;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work5 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_BG_RETRY");
        main_work.bg_retry = (AppMain.GMS_COCKPIT_2D_WORK)work5;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work6 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_RETRY_TEXT");
        main_work.tex_retry_act = (AppMain.GMS_COCKPIT_2D_WORK)work6;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work7 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_ACT_BACK_TEXT");
        main_work.tex_back_slct_act = (AppMain.GMS_COCKPIT_2D_WORK)work7;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        int index1 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_retry); index1 < index2; ++index1)
        {
            AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_RETRY_BTN");
            main_work.btn_retry[index1] = (AppMain.GMS_COCKPIT_2D_WORK)work2;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        int index3 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_back); index3 < index2; ++index3)
        {
            AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_ACT_BACK_BTN");
            main_work.btn_back[index3] = (AppMain.GMS_COCKPIT_2D_WORK)work2;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
    }

    private static void gmClearDemoCreateObjSpeScore(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.TaskWorkFactoryDelegate work_size = (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK());
        AppMain.OBS_OBJECT_WORK obsObjectWork;
        for (int index = 0; index < 3; ++index)
        {
            AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_UP_SPST_TEXT");
            main_work.tex_spst_up_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        for (int index = 0; index < 7; ++index)
        {
            AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_SPE_STAGE_NO");
            main_work.spst_num_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work1;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_EMER_UP_ICON");
            main_work.icon_emer_up_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work2;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.OBS_OBJECT_WORK work3 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_EMER_DOWN_ICON");
            main_work.icon_emer_down_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work3;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.OBS_OBJECT_WORK work4 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_LIGHT_EMER");
        main_work.icon_emer_light_act = (AppMain.GMS_COCKPIT_2D_WORK)work4;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work5 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_EXTEND_TEXT");
        main_work.tex_extend_act = (AppMain.GMS_COCKPIT_2D_WORK)work5;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        for (int index = 0; index < 5; ++index)
        {
            AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TIMENUM");
            main_work.time_num_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work1;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_RINGNUM");
            main_work.ring_num_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work2;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        for (int index = 0; index < 9; ++index)
        {
            AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TOTALNUM");
            main_work.total_num_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work1;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        for (int index = 0; index < 3; ++index)
        {
            AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_LINE");
            main_work.line_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work1;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.OBS_OBJECT_WORK work6 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TIMETEXT");
        main_work.tex_time_act = (AppMain.GMS_COCKPIT_2D_WORK)work6;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work7 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_RINGTEXT");
        main_work.tex_ring_act = (AppMain.GMS_COCKPIT_2D_WORK)work7;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work8 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TOTALTEXT");
        main_work.tex_total_act = (AppMain.GMS_COCKPIT_2D_WORK)work8;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work9 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_SONICICON");
        main_work.sonic_icon_act = (AppMain.GMS_COCKPIT_2D_WORK)work9;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work10 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_SONICICON2");
        main_work.sonic_icon_act2 = (AppMain.GMS_COCKPIT_2D_WORK)work10;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmClearDemoCreateObjSpeTime(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.TaskWorkFactoryDelegate work_size = (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK());
        AppMain.OBS_OBJECT_WORK obsObjectWork;
        for (int index = 0; index < 3; ++index)
        {
            AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_UP_SPST_TEXT");
            main_work.tex_spst_up_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        for (int index = 0; index < 7; ++index)
        {
            AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_SPE_STAGE_NO");
            main_work.spst_num_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work1;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_EMER_UP_ICON");
            main_work.icon_emer_up_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work2;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.OBS_OBJECT_WORK work3 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_EMER_DOWN_ICON");
            main_work.icon_emer_down_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work3;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.OBS_OBJECT_WORK work4 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_LIGHT_EMER");
        main_work.icon_emer_light_act = (AppMain.GMS_COCKPIT_2D_WORK)work4;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work5 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TIMEATK_TEXT");
        main_work.tex_big_time_act = (AppMain.GMS_COCKPIT_2D_WORK)work5;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        for (int index = 0; index < 7; ++index)
        {
            AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TIME_DIGIT_NUM");
            main_work.record_time_num_act[index] = (AppMain.GMS_COCKPIT_2D_WORK)work1;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.OBS_OBJECT_WORK work6 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_TIMEATK_SONIC");
        main_work.time_sonic_icon_act = (AppMain.GMS_COCKPIT_2D_WORK)work6;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work7 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_NEWRECORD_TEXT");
        main_work.tex_new_record_act = (AppMain.GMS_COCKPIT_2D_WORK)work7;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work8 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_BG_RETRY");
        main_work.bg_retry = (AppMain.GMS_COCKPIT_2D_WORK)work8;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work9 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_RETRY_TEXT");
        main_work.tex_retry_act = (AppMain.GMS_COCKPIT_2D_WORK)work9;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work10 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_ACT_BACK_TEXT");
        main_work.tex_back_slct_act = (AppMain.GMS_COCKPIT_2D_WORK)work10;
        obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        int index1 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_retry); index1 < index2; ++index1)
        {
            AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_RETRY_BTN");
            main_work.btn_retry[index1] = (AppMain.GMS_COCKPIT_2D_WORK)work1;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
        int index3 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_back); index3 < index2; ++index3)
        {
            AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_COCKPIT_CREATE_WORK(work_size, (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "CLRDM_ACT_BACK_BTN");
            main_work.btn_back[index3] = (AppMain.GMS_COCKPIT_2D_WORK)work1;
            obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
        }
    }

    private static void gmClearDemoCreateObjAct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.gmClearDemoCreateObjActForStage(main_work);
        for (int index = 0; index < 5; ++index)
        {
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.time_num_act[index], main_work.time_num_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(4 + index), 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.time_num_act[index]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.time_num_act[index]).scale.y = 4096;
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.ring_num_act[index], main_work.ring_num_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(9 + index), 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.ring_num_act[index]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.ring_num_act[index]).scale.y = 4096;
        }
        for (int index = 0; index < 9; ++index)
        {
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.total_num_act[index], main_work.total_num_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(18 + index), 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.total_num_act[index]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.total_num_act[index]).scale.y = 4096;
        }
        for (int index = 0; index < 3; ++index)
        {
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.line_act[index], main_work.line_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(14 + index), 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.line_act[index]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.line_act[index]).scale.y = 4096;
        }
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_time_act, main_work.tex_time_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 7U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_time_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_time_act).scale.y = 4096;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_ring_act, main_work.tex_ring_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 8U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_ring_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_ring_act).scale.y = 4096;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_total_act, main_work.tex_total_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 9U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_total_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_total_act).scale.y = 4096;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act, main_work.sonic_icon_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), 17U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act).scale.y = 4096;
    }

    private static void gmClearDemoCreateObjNormalTimeAtk(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.gmClearDemoCreateObjActForStage(main_work);
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_big_time_act, main_work.tex_big_time_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 15U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_big_time_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_big_time_act).scale.y = 4096;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.time_sonic_icon_act, main_work.time_sonic_icon_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), 56U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.time_sonic_icon_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.time_sonic_icon_act).scale.y = 4096;
        main_work.time_sonic_icon_act.obj_2d.frame = AppMain.GMM_MAIN_USE_SUPER_SONIC() ? 1f : 0.0f;
        main_work.time_sonic_icon_act.obj_2d.speed = 0.0f;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_new_record_act, main_work.tex_new_record_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 14U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_new_record_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_new_record_act).scale.y = 4096;
    }

    private static void gmClearDemoCreateObjSpecialScoreAtk(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[0], main_work.tex_spst_up_act[0].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 11U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[0]).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[0]).scale.y = 4096;
        if (main_work.is_get_eme && main_work.is_first_spe_clear)
        {
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[1], main_work.tex_spst_up_act[1].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 12U, 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[1]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[1]).scale.y = 4096;
        }
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[2], main_work.tex_spst_up_act[2].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 13U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[2]).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[2]).scale.y = 4096;
        AppMain.gmClearDemoCreateObjSpeActForStage(main_work);
        for (int index = 0; index < main_work.has_eme_num; ++index)
        {
            if (main_work.is_get_eme && main_work.is_first_spe_clear)
            {
                AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.icon_emer_down_act[index], main_work.icon_emer_down_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(34 + index), 0);
                ((AppMain.OBS_OBJECT_WORK)main_work.icon_emer_down_act[index]).scale.x = 4096;
                ((AppMain.OBS_OBJECT_WORK)main_work.icon_emer_down_act[index]).scale.y = 4096;
            }
            else
            {
                AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.icon_emer_up_act[index], main_work.icon_emer_up_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(42 + index), 0);
                ((AppMain.OBS_OBJECT_WORK)main_work.icon_emer_up_act[index]).scale.x = 4096;
                ((AppMain.OBS_OBJECT_WORK)main_work.icon_emer_up_act[index]).scale.y = 4096;
            }
        }
        for (int index = 0; index < 5; ++index)
        {
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.time_num_act[index], main_work.time_num_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(57 + index), 0);
            main_work.time_num_act[index].obj_2d.ama.act_tbl[(int)main_work.time_num_act[index].obj_2d.act_id].mtn.trs_tbl[0].trs_y -= 15f;
            ((AppMain.OBS_OBJECT_WORK)main_work.time_num_act[index]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.time_num_act[index]).scale.y = 4096;
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.ring_num_act[index], main_work.ring_num_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(62 + index), 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.ring_num_act[index]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.ring_num_act[index]).scale.y = 4096;
        }
        for (int index = 0; index < 9; ++index)
        {
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.total_num_act[index], main_work.total_num_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(72 + index), 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.total_num_act[index]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.total_num_act[index]).scale.y = 4096;
        }
        for (int index = 0; index < 3; ++index)
        {
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.line_act[index], main_work.line_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(67 + index), 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.line_act[index]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.line_act[index]).scale.y = 4096;
        }
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_time_act, main_work.tex_time_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 18U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_time_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_time_act).scale.y = 4096;
        main_work.tex_time_act.obj_2d.ama.act_tbl[(int)main_work.tex_time_act.obj_2d.act_id].mtn.trs_tbl[0].trs_y -= 15f;
        main_work.tex_time_act.obj_2d.ama.act_tbl[(int)main_work.tex_time_act.obj_2d.act_id].mtn.trs_tbl[1].trs_y -= 15f;
        main_work.tex_time_act.obj_2d.ama.act_tbl[(int)main_work.tex_time_act.obj_2d.act_id].mtn.trs_tbl[2].trs_y -= 15f;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_ring_act, main_work.tex_ring_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 19U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_ring_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_ring_act).scale.y = 4096;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_total_act, main_work.tex_total_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 20U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_total_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_total_act).scale.y = 4096;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act, main_work.sonic_icon_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), 70U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act).scale.y = 4096;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act2, main_work.sonic_icon_act2.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), 71U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act2).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act2).scale.y = 4096;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_extend_act, main_work.tex_extend_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 10U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_extend_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_extend_act).scale.y = 4096;
    }

    private static void gmClearDemoCreateObjSpecialTimeAtk(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[0], main_work.tex_spst_up_act[0].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 11U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[0]).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[0]).scale.y = 4096;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[2], main_work.tex_spst_up_act[2].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 13U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[2]).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[2]).scale.y = 4096;
        AppMain.gmClearDemoCreateObjSpeActForStage(main_work);
        for (int index = 0; index < main_work.has_eme_num; ++index)
        {
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.icon_emer_up_act[index], main_work.icon_emer_up_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(42 + index), 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.icon_emer_up_act[index]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.icon_emer_up_act[index]).scale.y = 4096;
        }
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_big_time_act, main_work.tex_big_time_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 15U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_big_time_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_big_time_act).scale.y = 4096;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.time_sonic_icon_act, main_work.time_sonic_icon_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), 56U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.time_sonic_icon_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.time_sonic_icon_act).scale.y = 4096;
        main_work.time_sonic_icon_act.obj_2d.frame = AppMain.GMM_MAIN_USE_SUPER_SONIC() ? 1f : 0.0f;
        main_work.time_sonic_icon_act.obj_2d.speed = 0.0f;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_new_record_act, main_work.tex_new_record_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 14U, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_new_record_act).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_new_record_act).scale.y = 4096;
    }

    private static void gmClearDemoCreateObjActForStage(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        int language = AppMain.GsEnvGetLanguage();
        ushort num1;
        switch (language)
        {
            case 3:
                num1 = (ushort)84;
                break;
            case 5:
                num1 = (ushort)85;
                break;
            default:
                num1 = (ushort)0;
                break;
        }
        if (language != 4)
        {
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[0], main_work.tex_up_act[0].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)num1, 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[0]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[0]).scale.y = 4096;
        }
        ushort num2 = language == 4 ? AppMain.dm_clrdm_stage_ge_tex_act_id[(int)main_work.stage_id] : AppMain.dm_clrdm_stage_tex_act_id[(int)main_work.stage_id];
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[1], main_work.tex_up_act[1].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), (uint)num2, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[1]).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[1]).scale.y = 4096;
        ushort num3;
        int index;
        if (AppMain.dm_clrdm_stage_text_amb_id[(int)main_work.stage_id] == 0)
        {
            num3 = (ushort)0;
            index = 30;
        }
        else
        {
            num3 = (ushort)1;
            index = AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()];
        }
        if (num3 == (ushort)0 && (language == 0 || language == 1))
        {
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[2], main_work.tex_up_act[2].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 3U, 0);
            ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[2]).scale.x = 4096;
            ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[2]).scale.y = 4096;
        }
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[3], main_work.tex_up_act[3].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), (uint)AppMain.dm_clrdm_stage_text_act_id[(int)main_work.stage_id], 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[3]).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[3]).scale.y = 4096;
        ushort num4 = language != 4 ? (language != 2 ? (language != 5 ? (language != 3 ? AppMain.dm_clrdm_stage_num_act_id[(int)main_work.stage_id] : AppMain.dm_clrdm_stage_sp_num_act_id[(int)main_work.stage_id]) : AppMain.dm_clrdm_stage_sp_num_act_id[(int)main_work.stage_id]) : AppMain.dm_clrdm_stage_fr_num_act_id[(int)main_work.stage_id]) : AppMain.dm_clrdm_stage_ge_num_act_id[(int)main_work.stage_id];
        if (num4 == ushort.MaxValue)
            return;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[4], main_work.tex_up_act[4].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, index, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[(int)num3]), (uint)num4, 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[4]).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[4]).scale.y = 4096;
    }

    private static void gmClearDemoCreateObjSpeActForStage(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        int language = AppMain.GsEnvGetLanguage();
        int index = (int)main_work.stage_id - 21;
        int num = language == 3 ? 86 : 27;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.spst_num_act[index], main_work.spst_num_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(num + index), 0);
        ((AppMain.OBS_OBJECT_WORK)main_work.spst_num_act[index]).scale.x = 4096;
        ((AppMain.OBS_OBJECT_WORK)main_work.spst_num_act[index]).scale.y = 4096;
    }

    private static void gmClearDemoSetSortBufAct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 32) == 0)
        {
            for (int index = 0; index < 3; ++index)
                ((AppMain.OBS_OBJECT_WORK)main_work.line_act[index]).disp_flag |= 32U;
        }
        else
        {
            for (int index = 0; index < 3; ++index)
                ((AppMain.OBS_OBJECT_WORK)main_work.line_act[index]).disp_flag &= 4294967263U;
        }
        if (((int)main_work.flag & 64) == 0)
            ((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act).disp_flag |= 32U;
        else
            ((AppMain.OBS_OBJECT_WORK)main_work.sonic_icon_act).disp_flag &= 4294967263U;
        AppMain.gmClearDemoSetSortBufScore(main_work);
    }

    private static void gmClearDemoSetTimeAtkSortBufAct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 1024) != 0)
        {
            for (int index = 0; index < 7; ++index)
                ((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index]).disp_flag &= 4294967263U;
        }
        AppMain.gmClearDemoSetSortBufScore(main_work);
    }

    private static void gmClearDemoSetRetryInitAct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        for (int index = 0; index < 5; ++index)
            ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[index]).disp_flag |= 32U;
        for (int index = 0; index < 7; ++index)
            ((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index]).disp_flag |= 32U;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_big_time_act).disp_flag |= 32U;
        ((AppMain.OBS_OBJECT_WORK)main_work.time_sonic_icon_act).disp_flag |= 32U;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_new_record_act).disp_flag |= 32U;
        main_work.tex_retry_act.obj_2d.frame = 0.0f;
        main_work.tex_retry_act.obj_2d.speed = 0.0f;
        main_work.tex_back_slct_act.obj_2d.frame = 0.0f;
        main_work.tex_back_slct_act.obj_2d.speed = 0.0f;
        int index1 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_retry); index1 < index2; ++index1)
        {
            main_work.btn_retry[index1].obj_2d.frame = 0.0f;
            main_work.btn_retry[index1].obj_2d.speed = 0.0f;
        }
        int index3 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_back); index3 < index2; ++index3)
        {
            main_work.btn_back[index3].obj_2d.frame = 0.0f;
            main_work.btn_back[index3].obj_2d.speed = 0.0f;
        }
    }

    private static void gmClearDemoSetRetrySortBufAct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.is_clear_spe_stg)
        {
            for (int index = 0; index < 3; ++index)
                ((AppMain.OBS_OBJECT_WORK)main_work.tex_spst_up_act[index]).disp_flag |= 32U;
        }
        else
        {
            for (int index = 0; index < 5; ++index)
                ((AppMain.OBS_OBJECT_WORK)main_work.tex_up_act[index]).disp_flag |= 32U;
        }
        for (int index = 0; index < 7; ++index)
            ((AppMain.OBS_OBJECT_WORK)main_work.record_time_num_act[index]).disp_flag |= 32U;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_big_time_act).disp_flag |= 32U;
        ((AppMain.OBS_OBJECT_WORK)main_work.time_sonic_icon_act).disp_flag |= 32U;
        ((AppMain.OBS_OBJECT_WORK)main_work.tex_new_record_act).disp_flag |= 32U;
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.bg_retry, main_work.bg_retry.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), 99U, 0);
        int index1 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_retry); index1 < index2; ++index1)
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.btn_retry[index1], main_work.btn_retry[index1].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(100 + index1), 0);
        int index3 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_back); index3 < index2; ++index3)
            AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.btn_back[index3], main_work.btn_back[index3].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, 30, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[0]), (uint)(103 + index3), 0);
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_retry_act, main_work.tex_retry_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 16U, 0);
        AppMain.ObjObjectAction2dAMALoadSetTexlist((AppMain.OBS_OBJECT_WORK)main_work.tex_back_slct_act, main_work.tex_back_slct_act.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.g_gm_clear_demo_data_ama_id[AppMain.GsEnvGetLanguage()], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(main_work.tex[1]), 17U, 0);
        main_work.tex_retry_act.obj_2d.frame = 0.0f;
        main_work.tex_retry_act.obj_2d.speed = 0.0f;
        main_work.tex_back_slct_act.obj_2d.frame = 0.0f;
        main_work.tex_back_slct_act.obj_2d.speed = 0.0f;
        int index4 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_retry); index4 < index2; ++index4)
        {
            main_work.btn_retry[index4].obj_2d.frame = 0.0f;
            main_work.btn_retry[index4].obj_2d.speed = 0.0f;
        }
        int index5 = 0;
        for (int index2 = AppMain.arrayof((Array)main_work.btn_back); index5 < index2; ++index5)
        {
            main_work.btn_back[index5].obj_2d.frame = 0.0f;
            main_work.btn_back[index5].obj_2d.speed = 0.0f;
        }
        main_work.trg_retry.Create(main_work.btn_retry[1].obj_2d.act);
        main_work.trg_back.Create(main_work.btn_back[1].obj_2d.act);
    }

    private static void gmClearDemoSetSortBufScore(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 128) != 0)
            AppMain.gmClearDemoSetSortBufScoreAct(main_work.time_num_act, main_work.time_score[1], 5U);
        if (((int)main_work.flag & 256) != 0)
            AppMain.gmClearDemoSetSortBufScoreAct(main_work.ring_num_act, main_work.ring_score[1], 5U);
        if (((int)main_work.flag & 512) == 0)
            return;
        AppMain.gmClearDemoSetSortBufScoreAct(main_work.total_num_act, main_work.total_score[1], 9U);
    }

    private static void gmClearDemoSetSortBufScoreAct(
      AppMain.GMS_COCKPIT_2D_WORK[] score_act,
      uint score,
      uint digits)
    {
        int num = 1;
        if (score < 10U)
        {
            for (int index = 0; index < (int)digits - 1; ++index)
                ((AppMain.OBS_OBJECT_WORK)score_act[index]).disp_flag |= 32U;
            ((AppMain.OBS_OBJECT_WORK)score_act[(int)(digits - 1U)]).disp_flag &= 4294967263U;
        }
        else
        {
            if (score < 10U)
                return;
            for (int index1 = 0; index1 < (int)digits; ++index1)
            {
                for (int index2 = 0; index2 < (int)((long)digits - (long)index1 - 1L); ++index2)
                    num *= 10;
                if (score < (uint)num)
                    ((AppMain.OBS_OBJECT_WORK)score_act[index1]).disp_flag |= 32U;
                else
                    ((AppMain.OBS_OBJECT_WORK)score_act[index1]).disp_flag &= 4294967263U;
                num = 1;
            }
        }
    }

    private static void gmClearDemoSetSortBufTimeAct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        int num1 = main_work.time_sec < (ushort)10 ? 0 : (int)main_work.time_sec / 10;
        int num2 = main_work.time_msec < (ushort)10 ? 0 : (int)main_work.time_msec / 10;
        int num3 = (int)main_work.time_sec % 10;
        int num4 = (int)main_work.time_msec % 10;
        main_work.record_time_num_act[0].obj_2d.frame = (float)main_work.time_min;
        main_work.record_time_num_act[2].obj_2d.frame = (float)num1;
        main_work.record_time_num_act[3].obj_2d.frame = (float)num3;
        main_work.record_time_num_act[5].obj_2d.frame = (float)num2;
        main_work.record_time_num_act[6].obj_2d.frame = (float)num4;
        main_work.record_time_num_act[1].obj_2d.frame = 0.0f;
        main_work.record_time_num_act[4].obj_2d.frame = 0.0f;
    }

    private static void gmClearDemoUpdateAct(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
    }

    private static void gmClearDemoTimeFlushEffect(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
    }

    private static void gmClearDemoSetCalcScore(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (main_work.ring_score[1] <= 0U && main_work.time_score[1] <= 0U)
        {
            main_work.proc_calc_score = (AppMain.GMS_CLRDM_MAIN_WORK._WorkDelegate)null;
            AppMain.amFlagOn(ref main_work.flag, 8U);
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

    private static void gmClearDemoSetScoreData(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        AppMain.gmClearDemoSetDispScore(main_work.time_num_act, main_work.time_score[1], 5U);
        AppMain.gmClearDemoSetDispScore(main_work.ring_num_act, main_work.ring_score[1], 5U);
        AppMain.gmClearDemoSetDispScore(main_work.total_num_act, main_work.total_score[1], 9U);
    }

    private static void gmClearDemoSetDispScore(
      AppMain.GMS_COCKPIT_2D_WORK[] score_act,
      uint score,
      uint digits)
    {
        int num1 = 1;
        for (int index = 0; index < 1; ++index)
        {
            score_act[(long)(digits - 1U) - (long)index].obj_2d.frame = 0.0f;
            score_act[(long)(digits - 1U) - (long)index].obj_2d.speed = 0.0f;
        }
        for (int index1 = 1; index1 < (int)digits; ++index1)
        {
            for (int index2 = 0; index2 < index1; ++index2)
                num1 *= 10;
            if (score >= (uint)num1)
            {
                float num2 = (float)((int)(float)((long)score / (long)num1) % 10);
                score_act[(long)(digits - 1U) - (long)index1].obj_2d.frame = num2;
                score_act[(long)(digits - 1U) - (long)index1].obj_2d.speed = 0.0f;
            }
            else
            {
                score_act[(long)(digits - 1U) - (long)index1].obj_2d.frame = 0.0f;
                score_act[(long)(digits - 1U) - (long)index1].obj_2d.speed = 0.0f;
            }
            num1 = 1;
        }
    }

    private static void gmClearDemoSetFlashSonic(AppMain.GMS_CLRDM_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 64) != 0)
        {
            if ((double)main_work.flash_timer >= 30.0)
            {
                AppMain.amFlagOff(ref main_work.flag, 64U);
                main_work.flash_timer = 0.0f;
            }
        }
        else if ((double)main_work.flash_timer >= 10.0)
        {
            AppMain.amFlagOn(ref main_work.flag, 64U);
            main_work.flash_timer = 0.0f;
        }
        main_work.flash_timer += (float)main_work.count;
    }

    private static bool gmClearDemoIsTexLoad()
    {
        int num = 0;
        for (int index = 0; index < 2; ++index)
        {
            if (AppMain.AoTexIsLoaded(AppMain.gm_clrdm_tex[index]))
                num |= 1 << index;
        }
        return num == 3;
    }

    private static bool gmClearDemoIsTexRelease()
    {
        int num = 0;
        for (int index = 0; index < 2; ++index)
        {
            if (AppMain.AoTexIsReleased(AppMain.gm_clrdm_tex[index]))
                num |= 1 << index;
        }
        return num == 3;
    }

    private static short gmClearDemoGetRingNum()
    {
        return AppMain.g_gm_main_system.ply_work[0] != null ? AppMain.g_gm_main_system.ply_work[0].ring_num : (short)0;
    }

    private static uint gmClearDemoGetScore()
    {
        return AppMain.g_gm_main_system.ply_work[0] != null ? AppMain.g_gm_main_system.ply_work[0].score : 0U;
    }

    private static uint gmClearDemoGetGameTime()
    {
        if (AppMain.g_gs_main_sys_info.stage_id >= (ushort)21 && ((int)AppMain.g_gm_main_system.game_flag & 65536) == 0)
            return 0;
        return AppMain.g_gm_main_system.game_time >= 35999U ? 35999U : AppMain.g_gm_main_system.game_time;
    }

    private static uint gmClearDemoGetChallengeNum()
    {
        return AppMain.g_gm_main_system.player_rest_num[0];
    }

    private static void gmClearDemoSetBgColorBlack()
    {
        AppMain.gmClearDemoSetBgColorBlack((AppMain.AMS_TCB)null);
    }

    private static void gmClearDemoSetBgColorBlack(AppMain.AMS_TCB tcb)
    {
        if (tcb != null)
            AppMain.amDrawSetBGColor(new AppMain.NNS_RGBA_U8((byte)0, (byte)0, (byte)0, byte.MaxValue));
        else
            AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.gmClearDemoSetBgColorBlack), (ushort)65280);
    }

}