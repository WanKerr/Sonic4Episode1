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
    private static void HgTrophyTryAcquisition(int timing)
    {
        AppMain.HGS_TROPHY_CHECK_TIMING_INFO trophyCheckTimingInfo = AppMain.hg_trophy_check_timing_info_tbl[timing];
        if (AppMain.GsTrialIsTrial() || AppMain.hgTrophyIsPlayDemo())
            return;
        for (int index = 0; index < trophyCheckTimingInfo.num; ++index)
        {
            AppMain.HGS_TROPHY_CHECK_INFO check_info = trophyCheckTimingInfo.check_info_tbl[index];
            if (check_info.trophy_type == 0)
                AppMain.hgTrophyTryAcquisitionNormal(check_info);
            else
                AppMain.MTM_ASSERT(false);
        }
    }

    private static void HgTrophyIncEnemyKillCount(AppMain.OBS_OBJECT_WORK ene_obj)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        if (AppMain.hgTrophyIsPlayDemo() || ene_obj.obj_type != (ushort)2)
            return;
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)ene_obj;
        if (gmsEnemyComWork.eve_rec == null || gmsEnemyComWork.eve_rec.id != (ushort)0 && gmsEnemyComWork.eve_rec.id <= (ushort)0 || gmsEnemyComWork.eve_rec.id >= (ushort)39)
            return;
        if (mainSysInfo.ene_kill_count < 1000U)
            ++mainSysInfo.ene_kill_count;
        else
            mainSysInfo.ene_kill_count = 1000U;
        AppMain.HgTrophyTryAcquisition(2);
    }

    private static void HgTrophyIncPlayerDamageCount(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (AppMain.hgTrophyIsPlayDemo())
            return;
        if (AppMain.g_gm_main_system.ply_dmg_count < 1U)
            ++AppMain.g_gm_main_system.ply_dmg_count;
        else
            AppMain.g_gm_main_system.ply_dmg_count = 1U;
    }

    private static void HgTrophyIncFinalClearCount()
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        if (mainSysInfo.final_clear_count < 2U)
            ++mainSysInfo.final_clear_count;
        else
            mainSysInfo.final_clear_count = 2U;
    }

    private static bool hgTrophyIsPlayDemo()
    {
        return AppMain.GsTrialIsTrial();
    }

    private static uint hgTrophyGetClearTime()
    {
        return AppMain.GsGetMainSysInfo().clear_time >= 35999 ? 35999U : (uint)AppMain.GsGetMainSysInfo().clear_time;
    }

    private static void hgTrophyTryAcquisitionNormal(AppMain.HGS_TROPHY_CHECK_INFO check_info)
    {
        if (AppMain.GsTrophyIsAcquired(check_info.trophy_id) || !check_info.acquire_check_func())
            return;
        AppMain.GsTrophyUpdateAcquisition(check_info.trophy_id);
    }

    private static void hgTrophyTryAcquisitionAvatar(AppMain.HGS_TROPHY_CHECK_INFO check_info)
    {
        if (AppMain.GsTrophyAvatarIsAcquired(check_info.trophy_id) || !check_info.acquire_check_func())
            return;
        AppMain.GsTrophyAvatarUpdateAcquisition(check_info.trophy_id);
    }

    private static bool hgTrophyCheckAcquireStage11Clear()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        return AppMain.GsGetMainSysInfo().stage_id == (ushort)0;
    }

    private static bool hgTrophyCheckAcquireDefeatBoss1()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        if (AppMain.GMM_MAIN_STAGE_IS_BOSS())
            return AppMain.g_gs_main_sys_info.stage_id == (ushort)3;
        AppMain.MTM_ASSERT((object)"hgTrophy.cpp::hgTrophyCheckAcquireDefeatBoss() Error! Not supposed to be called in this stage\n");
        return false;
    }

    private static bool hgTrophyCheckAcquireDefeatBoss2()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        if (AppMain.GMM_MAIN_STAGE_IS_BOSS())
            return AppMain.g_gs_main_sys_info.stage_id == (ushort)7;
        AppMain.MTM_ASSERT((object)"hgTrophy.cpp::hgTrophyCheckAcquireDefeatBoss() Error! Not supposed to be called in this stage\n");
        return false;
    }

    private static bool hgTrophyCheckAcquireDefeatBoss3()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        if (AppMain.GMM_MAIN_STAGE_IS_BOSS())
            return AppMain.g_gs_main_sys_info.stage_id == (ushort)11;
        AppMain.MTM_ASSERT((object)"hgTrophy.cpp::hgTrophyCheckAcquireDefeatBoss() Error! Not supposed to be called in this stage\n");
        return false;
    }

    private static bool hgTrophyCheckAcquireDefeatBoss4()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        if (AppMain.GMM_MAIN_STAGE_IS_BOSS())
            return AppMain.g_gs_main_sys_info.stage_id == (ushort)15;
        AppMain.MTM_ASSERT((object)"hgTrophy.cpp::hgTrophyCheckAcquireDefeatBoss() Error! Not supposed to be called in this stage\n");
        return false;
    }

    private static bool hgTrophyCheckAcquireFirstChaosEmerald()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        return ((int)AppMain.g_gm_main_system.game_flag & 65536) != 0;
    }

    private static bool hgTrophyCheckAcquire1000EnemyKill()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        return AppMain.GsGetMainSysInfo().ene_kill_count >= 1000U;
    }

    private static bool hgTrophyCheckAcquireSsonicInAllAct()
    {
        bool flag = true;
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        if (AppMain.g_gm_gamedat_stage_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] != AppMain.GSE_MAIN_STAGE_TYPE.GSD_MAIN_STAGE_TYPE_ACT || AppMain.g_gs_main_sys_info.stage_id == (ushort)28 || !AppMain.GMM_MAIN_GOAL_AS_SUPER_SONIC())
            return false;
        for (uint index = 0; index < 29U; ++index)
        {
            if (index != 28U && AppMain.g_gm_gamedat_stage_type_tbl[(int)index] == AppMain.GSE_MAIN_STAGE_TYPE.GSD_MAIN_STAGE_TYPE_ACT)
            {
                AppMain.MTM_ASSERT(index < 16U);
                if ((int)index == (int)AppMain.g_gs_main_sys_info.stage_id)
                {
                    if (!AppMain.GMM_MAIN_GOAL_AS_SUPER_SONIC())
                        flag = false;
                }
                else if (!AppMain.GsMainSysIsStageGoalAsSuperSonic((int)index))
                    flag = false;
            }
        }
        return flag;
    }

    private static bool hgTrophyCheckAcquireReachEnd()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)10);
        return true;
    }

    private static bool hgTrophyCheckAcquireUploadAllRecords()
    {
        bool flag = true;
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)7 || AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        for (uint index = 0; index < 29U; ++index)
        {
            if (index != 28U && !AppMain.GsMainSysIsStageTimeUploadOnce((int)index))
            {
                flag = false;
                break;
            }
        }
        return flag;
    }

    private static bool hgTrophyCheckAcquireStageS1AllRings()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        if (AppMain.GsGetMainSysInfo().stage_id != (ushort)21 || ((int)AppMain.g_gm_main_system.game_flag & 196608) == 0 || AppMain.GsGetMainSysInfo().clear_ring < AppMain.GmEventMgrGetRingNum())
            return false;
        AppMain.MTM_ASSERT((int)AppMain.GsGetMainSysInfo().clear_ring == (int)AppMain.GmEventMgrGetRingNum());
        return true;
    }

    private static bool hgTrophyCheckAcquireStage100Rings()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        return AppMain.GsGetMainSysInfo().clear_ring >= 120U && AppMain.GsGetMainSysInfo().stage_id == (ushort)12;
    }

    private static bool hgTrophyCheckAcquire99Challenge()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        return AppMain.g_gm_main_system.player_rest_num[0] >= 100U;
    }

    private static bool hgTrophyCheckAcquireAllChaosEmeralds()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        return ((int)AppMain.g_gm_main_system.game_flag & 65536) != 0 && ((int)AppMain.GsGetMainSysInfo().game_flag & 32) != 0;
    }

    private static bool hgTrophyCheckAcquireStage11ClearIn1Min()
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        if (mainSysInfo.stage_id == (ushort)0 && (mainSysInfo.game_mode == 1 || ((int)mainSysInfo.game_flag & 256) == 0))
        {
            ushort min = 0;
            ushort sec = 0;
            ushort msec = 0;
            AppMain.AkUtilFrame60ToTime(AppMain.hgTrophyGetClearTime(), ref min, ref sec, ref msec);
            switch (min)
            {
                case 0:
                    return true;
                case 1:
                    if (sec != (ushort)0 || msec != (ushort)0)
                        break;
                    goto case 0;
            }
        }
        return false;
    }

    private static bool hgTrophyCheckAcquireStageFClearNoDamage()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        return AppMain.GsGetMainSysInfo().stage_id == (ushort)16 && AppMain.g_gm_main_system.ply_dmg_count == 0U && ((int)AppMain.g_gm_main_system.game_flag & 67108864) == 0;
    }

    private static bool hgTrophyCheckAcquireStageEndingAllRings()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)9);
        if (AppMain.GsGetMainSysInfo().stage_id != (ushort)28 || AppMain.GsGetMainSysInfo().clear_ring < AppMain.GmEventMgrGetRingNum())
            return false;
        AppMain.MTM_ASSERT((int)AppMain.GsGetMainSysInfo().clear_ring == (int)AppMain.GmEventMgrGetRingNum());
        return true;
    }

    private static bool hgTrophyCheckAcquireStageFClearAllEmeralds()
    {
        AppMain.MTM_ASSERT(AppMain.SyGetEvtInfo().cur_evt_id == (short)6);
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        return mainSysInfo.stage_id == (ushort)16 && ((int)AppMain.GsGetMainSysInfo().game_flag & 32) != 0 && mainSysInfo.final_clear_count >= 2U;
    }

}