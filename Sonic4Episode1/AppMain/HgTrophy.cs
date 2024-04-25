public partial class AppMain
{
    private static void HgTrophyTryAcquisition(int timing)
    {
        HGS_TROPHY_CHECK_TIMING_INFO trophyCheckTimingInfo = hg_trophy_check_timing_info_tbl[timing];
        if (GsTrialIsTrial() || hgTrophyIsPlayDemo())
            return;
        for (int index = 0; index < trophyCheckTimingInfo.num; ++index)
        {
            HGS_TROPHY_CHECK_INFO check_info = trophyCheckTimingInfo.check_info_tbl[index];
            if (check_info.trophy_type == 0)
                hgTrophyTryAcquisitionNormal(check_info);
            else
                MTM_ASSERT(false);
        }
    }

    private static void HgTrophyIncEnemyKillCount(OBS_OBJECT_WORK ene_obj)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        if (hgTrophyIsPlayDemo() || ene_obj.obj_type != 2)
            return;
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)ene_obj;
        if (gmsEnemyComWork.eve_rec == null || gmsEnemyComWork.eve_rec.id != 0 && gmsEnemyComWork.eve_rec.id <= 0 || gmsEnemyComWork.eve_rec.id >= 39)
            return;
        if (mainSysInfo.ene_kill_count < 1000U)
            ++mainSysInfo.ene_kill_count;
        else
            mainSysInfo.ene_kill_count = 1000U;
        HgTrophyTryAcquisition(2);
    }

    private static void HgTrophyIncPlayerDamageCount(GMS_PLAYER_WORK ply_work)
    {
        if (hgTrophyIsPlayDemo())
            return;
        if (g_gm_main_system.ply_dmg_count < 1U)
            ++g_gm_main_system.ply_dmg_count;
        else
            g_gm_main_system.ply_dmg_count = 1U;
    }

    private static void HgTrophyIncFinalClearCount()
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        if (mainSysInfo.final_clear_count < 2U)
            ++mainSysInfo.final_clear_count;
        else
            mainSysInfo.final_clear_count = 2U;
    }

    private static bool hgTrophyIsPlayDemo()
    {
        return GsTrialIsTrial();
    }

    private static uint hgTrophyGetClearTime()
    {
        return GsGetMainSysInfo().clear_time >= 35999 ? 35999U : (uint)GsGetMainSysInfo().clear_time;
    }

    private static void hgTrophyTryAcquisitionNormal(HGS_TROPHY_CHECK_INFO check_info)
    {
        if (GsTrophyIsAcquired(check_info.trophy_id) || !check_info.acquire_check_func())
            return;
        GsTrophyUpdateAcquisition(check_info.trophy_id);
    }

    private static void hgTrophyTryAcquisitionAvatar(HGS_TROPHY_CHECK_INFO check_info)
    {
        if (GsTrophyAvatarIsAcquired(check_info.trophy_id) || !check_info.acquire_check_func())
            return;
        GsTrophyAvatarUpdateAcquisition(check_info.trophy_id);
    }

    private static bool hgTrophyCheckAcquireStage11Clear()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        return GsGetMainSysInfo().stage_id == 0;
    }

    private static bool hgTrophyCheckAcquireDefeatBoss1()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        if (GMM_MAIN_STAGE_IS_BOSS())
            return g_gs_main_sys_info.stage_id == 3;
        MTM_ASSERT("hgTrophy.cpp::hgTrophyCheckAcquireDefeatBoss() Error! Not supposed to be called in this stage\n");
        return false;
    }

    private static bool hgTrophyCheckAcquireDefeatBoss2()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        if (GMM_MAIN_STAGE_IS_BOSS())
            return g_gs_main_sys_info.stage_id == 7;
        MTM_ASSERT("hgTrophy.cpp::hgTrophyCheckAcquireDefeatBoss() Error! Not supposed to be called in this stage\n");
        return false;
    }

    private static bool hgTrophyCheckAcquireDefeatBoss3()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        if (GMM_MAIN_STAGE_IS_BOSS())
            return g_gs_main_sys_info.stage_id == 11;
        MTM_ASSERT("hgTrophy.cpp::hgTrophyCheckAcquireDefeatBoss() Error! Not supposed to be called in this stage\n");
        return false;
    }

    private static bool hgTrophyCheckAcquireDefeatBoss4()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        if (GMM_MAIN_STAGE_IS_BOSS())
            return g_gs_main_sys_info.stage_id == 15;
        MTM_ASSERT("hgTrophy.cpp::hgTrophyCheckAcquireDefeatBoss() Error! Not supposed to be called in this stage\n");
        return false;
    }

    private static bool hgTrophyCheckAcquireFirstChaosEmerald()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        return ((int)g_gm_main_system.game_flag & 65536) != 0;
    }

    private static bool hgTrophyCheckAcquire1000EnemyKill()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        return GsGetMainSysInfo().ene_kill_count >= 1000U;
    }

    private static bool hgTrophyCheckAcquireSsonicInAllAct()
    {
        bool flag = true;
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        if (g_gm_gamedat_stage_type_tbl[g_gs_main_sys_info.stage_id] != GSE_MAIN_STAGE_TYPE.GSD_MAIN_STAGE_TYPE_ACT || g_gs_main_sys_info.stage_id == 28 || !GMM_MAIN_GOAL_AS_SUPER_SONIC())
            return false;
        for (uint index = 0; index < 29U; ++index)
        {
            if (index != 28U && g_gm_gamedat_stage_type_tbl[(int)index] == GSE_MAIN_STAGE_TYPE.GSD_MAIN_STAGE_TYPE_ACT)
            {
                MTM_ASSERT(index < 16U);
                if ((int)index == g_gs_main_sys_info.stage_id)
                {
                    if (!GMM_MAIN_GOAL_AS_SUPER_SONIC())
                        flag = false;
                }
                else if (!GsMainSysIsStageGoalAsSuperSonic((int)index))
                    flag = false;
            }
        }
        return flag;
    }

    private static bool hgTrophyCheckAcquireReachEnd()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 10);
        return true;
    }

    private static bool hgTrophyCheckAcquireUploadAllRecords()
    {
        bool flag = true;
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 7 || SyGetEvtInfo().cur_evt_id == 6);
        for (uint index = 0; index < 29U; ++index)
        {
            if (index != 28U && !GsMainSysIsStageTimeUploadOnce((int)index))
            {
                flag = false;
                break;
            }
        }
        return flag;
    }

    private static bool hgTrophyCheckAcquireStageS1AllRings()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        if (GsGetMainSysInfo().stage_id != 21 || ((int)g_gm_main_system.game_flag & 196608) == 0 || GsGetMainSysInfo().clear_ring < GmEventMgrGetRingNum())
            return false;
        MTM_ASSERT((int)GsGetMainSysInfo().clear_ring == (int)GmEventMgrGetRingNum());
        return true;
    }

    private static bool hgTrophyCheckAcquireStage100Rings()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        return GsGetMainSysInfo().clear_ring >= 120U && GsGetMainSysInfo().stage_id == 12;
    }

    private static bool hgTrophyCheckAcquire99Challenge()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        return g_gm_main_system.player_rest_num[0] >= 100U;
    }

    private static bool hgTrophyCheckAcquireAllChaosEmeralds()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        return ((int)g_gm_main_system.game_flag & 65536) != 0 && ((int)GsGetMainSysInfo().game_flag & 32) != 0;
    }

    private static bool hgTrophyCheckAcquireStage11ClearIn1Min()
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        if (mainSysInfo.stage_id == 0 && (mainSysInfo.game_mode == 1 || ((int)mainSysInfo.game_flag & 256) == 0))
        {
            ushort min = 0;
            ushort sec = 0;
            ushort msec = 0;
            AkUtilFrame60ToTime(hgTrophyGetClearTime(), ref min, ref sec, ref msec);
            switch (min)
            {
                case 0:
                    return true;
                case 1:
                    if (sec != 0 || msec != 0)
                        break;
                    goto case 0;
            }
        }
        return false;
    }

    private static bool hgTrophyCheckAcquireStageFClearNoDamage()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        return GsGetMainSysInfo().stage_id == 16 && g_gm_main_system.ply_dmg_count == 0U && ((int)g_gm_main_system.game_flag & 67108864) == 0;
    }

    private static bool hgTrophyCheckAcquireStageEndingAllRings()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 9);
        if (GsGetMainSysInfo().stage_id != 28 || GsGetMainSysInfo().clear_ring < GmEventMgrGetRingNum())
            return false;
        MTM_ASSERT((int)GsGetMainSysInfo().clear_ring == (int)GmEventMgrGetRingNum());
        return true;
    }

    private static bool hgTrophyCheckAcquireStageFClearAllEmeralds()
    {
        MTM_ASSERT(SyGetEvtInfo().cur_evt_id == 6);
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        return mainSysInfo.stage_id == 16 && ((int)GsGetMainSysInfo().game_flag & 32) != 0 && mainSysInfo.final_clear_count >= 2U;
    }

}