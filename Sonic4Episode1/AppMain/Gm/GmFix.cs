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
    private static void GmFixBuildDataInit()
    {
        for (int index = 0; index < 3; ++index)
            AppMain.gm_fix_textures[index].Clear();
        for (int index = 0; index < 3; ++index)
        {
            AppMain.gm_fix_texamb_list[index] = AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, AppMain.gm_fix_tex_amb_idx_tbl[AppMain.GsEnvGetLanguage()][index], AppMain.GmGameDatGetCockpitData());
            AppMain.AoTexBuild(AppMain.gm_fix_textures[index], (AppMain.AMS_AMB_HEADER)AppMain.gm_fix_texamb_list[index]);
            AppMain.AoTexLoad(AppMain.gm_fix_textures[index]);
        }
    }

    private static bool GmFixBuildDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < 3; ++index)
        {
            if (!AppMain.AoTexIsLoaded(AppMain.gm_fix_textures[index]))
                flag = false;
        }
        return flag;
    }

    private static void GmFixFlushDataInit()
    {
        for (int index = 0; index < 3; ++index)
            AppMain.AoTexRelease(AppMain.gm_fix_textures[index]);
    }

    private static bool GmFixFlushDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < 3; ++index)
        {
            if (AppMain.gm_fix_texamb_list[index] != null)
            {
                if (!AppMain.AoTexIsReleased(AppMain.gm_fix_textures[index]))
                {
                    flag = false;
                }
                else
                {
                    AppMain.gm_fix_texamb_list[index] = (object)null;
                    AppMain.gm_fix_textures[index].Clear();
                }
            }
        }
        return flag;
    }

    private static void GmFixInit()
    {
        AppMain.gm_fix_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmFixProcMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmFixDest), 0U, (ushort)0, 18432U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_FIX_MGR_WORK()), "GM_FIX_MGR");
        AppMain.GMS_FIX_MGR_WORK work = (AppMain.GMS_FIX_MGR_WORK)AppMain.gm_fix_tcb.work;
        work.Clear();
        AppMain.GMF_FIX_PART_INIT_FUNC[] gmfFixPartInitFuncArray;
        if (AppMain.gmFixIsSpecialStage())
        {
            gmfFixPartInitFuncArray = AppMain.gm_fix_ss_part_init_func_tbl;
            work.flag |= 4U;
        }
        else
            gmfFixPartInitFuncArray = !AppMain.gmFixIsTimeAttack() ? AppMain.gm_fix_part_init_func_tbl : (!AppMain.gmFixIsStage22() ? AppMain.gm_fix_ta_part_init_func_tbl : AppMain.gm_fix_ta_s22_part_init_func_tbl);
        for (int index = 0; index < 5; ++index)
        {
            if (gmfFixPartInitFuncArray[index] != null)
                gmfFixPartInitFuncArray[index](work);
        }
        work.proc_update = (AppMain.MPP_VOID_GMS_FIX_PART_WORK)null;
    }

    private static void GmFixExit()
    {
        if (AppMain.gm_fix_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_fix_tcb);
        AppMain.gm_fix_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void GmFixSetDisp(bool enable)
    {
        AppMain.GmFixSetDispEx(enable, enable, enable, enable, enable);
    }

    private static void GmFixSetDispEx(
      bool enable,
      bool enable_ss,
      bool enable_pause,
      bool enable_action,
      bool enable_move)
    {
        if (AppMain.gm_fix_tcb == null)
            return;
        AppMain.GMS_FIX_MGR_WORK work = (AppMain.GMS_FIX_MGR_WORK)AppMain.gm_fix_tcb.work;
        if (work == null)
            return;
        int num1 = (enable ? 2 : 0) | (enable_ss ? 512 : 0) | (enable_pause ? 1024 : 0) | (enable_action ? 2048 : 0) | (enable_move ? 4096 : 0);
        int num2 = (enable ? 0 : 2) | (enable_ss ? 0 : 512) | (enable_pause ? 0 : 1024) | (enable_action ? 0 : 2048) | (enable_move ? 0 : 4096);
        work.flag &= (uint)~num1;
        work.flag |= (uint)num2;
    }

    private static bool GmFixIsDisp()
    {
        if (AppMain.gm_fix_tcb != null)
        {
            AppMain.GMS_FIX_MGR_WORK work = (AppMain.GMS_FIX_MGR_WORK)AppMain.gm_fix_tcb.work;
            if (work != null && ((int)work.flag & 2) == 0)
                return true;
        }
        return false;
    }

    private static void GmFixRequestTimerFlash()
    {
        if (AppMain.gm_fix_tcb == null)
            return;
        AppMain.GMS_FIX_MGR_WORK work = (AppMain.GMS_FIX_MGR_WORK)AppMain.gm_fix_tcb.work;
        if (work == null)
            return;
        work.req_flag |= 1U;
    }

    private static void gmFixSubpartOutFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.gm_fix_tcb == null || ((int)((AppMain.GMS_FIX_MGR_WORK)AppMain.gm_fix_tcb.work).flag & 2) != 0)
            return;
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void gmFixSetFrameStatic(AppMain.OBS_OBJECT_WORK obj_work, float frame)
    {
        obj_work.obj_2d.frame = frame;
        obj_work.obj_2d.speed = 0.0f;
    }

    private static void gmFixUpdatePart(AppMain.GMS_FIX_PART_WORK part_work)
    {
        if (((int)part_work.flag & 1) == 0)
            return;
        if (AppMain.gmFixCheckClear(part_work))
        {
            AppMain.gmFixUnregisterPart(part_work);
        }
        else
        {
            if (part_work.proc_update != null)
                part_work.proc_update(part_work);
            if (part_work.proc_disp == null || ((int)part_work.flag & 2) != 0)
                return;
            part_work.proc_disp(part_work);
        }
    }

    private static bool gmFixCheckClear(AppMain.GMS_FIX_PART_WORK part_work)
    {
        return AppMain.gm_fix_tcb == null || part_work.parent_mgr == null || ((int)part_work.parent_mgr.flag & 1) != 0;
    }

    private static bool gmFixIsSpecialStage()
    {
        switch (AppMain.GsGetMainSysInfo().stage_id)
        {
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
                return true;
            default:
                return false;
        }
    }

    private static bool gmFixIsTimeAttack()
    {
        return AppMain.GsGetMainSysInfo().game_mode == 1;
    }

    private static bool gmFixIsStage22()
    {
        return AppMain.GsGetMainSysInfo().stage_id == (ushort)5;
    }

    private static bool gmFixIsStageTruck()
    {
        return AppMain.GsGetMainSysInfo().stage_id == (ushort)9;
    }

    private static int gmFixGetPlan()
    {
        int num = 0;
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        if (AppMain.gmFixIsStageTruck())
        {
            if ((512 & (int)mainSysInfo.game_flag) != 0)
                num = 1;
        }
        else if (AppMain.gmFixIsSpecialStage())
        {
            if ((512 & (int)mainSysInfo.game_flag) != 0)
                num = 1;
        }
        else if ((1 & (int)mainSysInfo.game_flag) != 0)
            num = 2;
        return num;
    }

    private static short gmFixGetRingNum()
    {
        return AppMain.g_gm_main_system.ply_work[0] != null ? AppMain.g_gm_main_system.ply_work[0].ring_num : (short)0;
    }

    private static uint gmFixGetScore()
    {
        return AppMain.g_gm_main_system.ply_work[0] != null ? AppMain.g_gm_main_system.ply_work[0].score : 0U;
    }

    private static uint gmFixGetGameTime()
    {
        return AppMain.g_gm_main_system.game_time >= 35999U ? 35999U : AppMain.g_gm_main_system.game_time;
    }

    private static uint gmFixGetChallengeNum()
    {
        uint num = 0;
        if (AppMain.g_gm_main_system.player_rest_num[0] > 0U)
            num = AppMain.g_gm_main_system.player_rest_num[0] - 1U;
        return (uint)AppMain.MTM_MATH_CLIP((int)num, 0, 999);
    }

    private static void gmFixInitBlink(
      AppMain.GMS_FIX_PART_WORK part_work,
      uint on_time,
      uint off_time)
    {
        part_work.blink_timer = on_time + off_time;
        part_work.blink_on_time = on_time;
        part_work.blink_off_time = off_time;
    }

    private static bool gmFixUpdateBlink(AppMain.GMS_FIX_PART_WORK part_work)
    {
        if (part_work.blink_timer != 0U)
            --part_work.blink_timer;
        bool flag = part_work.blink_timer >= part_work.blink_off_time;
        if (part_work.blink_timer == 0U)
            part_work.blink_timer = part_work.blink_on_time + part_work.blink_off_time;
        return flag;
    }

    private static void gmFixDest(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static void gmFixProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_FIX_MGR_WORK work = (AppMain.GMS_FIX_MGR_WORK)tcb.work;
        if (work.proc_update != null)
            AppMain.mppAssertNotImpl();
        for (int index = 0; index < 5; ++index)
        {
            if (work.part_work[index] != null)
                AppMain.gmFixUpdatePart(work.part_work[index]);
        }
    }

    private static void gmFixRegisterPart(
      AppMain.GMS_FIX_MGR_WORK mgr_work,
      AppMain.GMS_FIX_PART_WORK part_work,
      int part_type)
    {
        mgr_work.part_work[part_type] = part_work;
        part_work.part_type = part_type;
        part_work.parent_mgr = mgr_work;
        part_work.flag |= 1U;
    }

    private static void gmFixUnregisterPart(AppMain.GMS_FIX_PART_WORK part_work)
    {
        AppMain.GMS_FIX_MGR_WORK parentMgr = part_work.parent_mgr;
        if (parentMgr != null)
            parentMgr.part_work[part_work.part_type] = (AppMain.GMS_FIX_PART_WORK)null;
        part_work.part_type = -1;
        part_work.parent_mgr = (AppMain.GMS_FIX_MGR_WORK)null;
        part_work.flag &= 4294967294U;
    }

    private static bool gmFixProcessRequest(AppMain.GMS_FIX_MGR_WORK mgr_work, uint req_flag_bit)
    {
        if (((int)mgr_work.req_flag & (int)req_flag_bit) == 0)
            return false;
        mgr_work.req_flag &= ~req_flag_bit;
        return true;
    }

    private static void gmFixRingCountPartInit(AppMain.GMS_FIX_MGR_WORK mgr_work)
    {
        AppMain.GMS_FIX_PART_WORK partRingcount = (AppMain.GMS_FIX_PART_WORK)mgr_work.part_ringcount;
        AppMain.gmFixRegisterPart(mgr_work, partRingcount, 0);
        partRingcount.proc_update = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixRingCountPartProcUpdateMain);
        partRingcount.proc_disp = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixRingCountPartProcDispMain);
        for (int index = 0; index < 4; ++index)
        {
            AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "FIX_RING");
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (AppMain.GMS_COCKPIT_2D_WORK)work;
            work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmFixSubpartOutFunc);
            AppMain.ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.gm_fix_ama_amb_idx_tbl[AppMain.GsEnvGetLanguage()][0], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(AppMain.gm_fix_textures[0]), (uint)AppMain.gm_fix_ringcount_act_id_tbl[index], 0);
            AppMain.gmFixSetFrameStatic(work, 0.0f);
            if (index != 0)
                work.pos.y += AppMain.FX_F32_TO_FX32(5f);
            ((AppMain.GMS_FIX_PART_RINGCOUNT)partRingcount).sub_parts[index] = (AppMain.GMS_COCKPIT_2D_WORK)work;
        }
    }

    private static void gmFixRingCountPartProcUpdateMain(AppMain.GMS_FIX_PART_WORK part_work)
    {
        AppMain.gmFixRingCountPartUpdateDigitList((AppMain.GMS_FIX_PART_RINGCOUNT)part_work);
        if (AppMain.gmFixGetRingNum() == (short)0)
        {
            if (((int)part_work.flag & 4) != 0)
                return;
            part_work.flag |= 4U;
            AppMain.gmFixInitBlink(part_work, 10U, 10U);
        }
        else
            part_work.flag &= 4294967291U;
    }

    private static void gmFixRingCountPartProcDispMain(AppMain.GMS_FIX_PART_WORK part_work)
    {
        AppMain.GMS_FIX_PART_RINGCOUNT part_ringcount = (AppMain.GMS_FIX_PART_RINGCOUNT)part_work;
        if (((int)part_work.flag & 4) != 0)
        {
            if (AppMain.gmFixUpdateBlink(part_work))
                part_work.flag &= 4294967287U;
            else
                part_work.flag |= 8U;
        }
        else
            part_work.flag &= 4294967287U;
        if (((int)part_work.flag & 8) != 0)
            AppMain.gmFixRingCountPartSetDispDigits(part_ringcount, false);
        else
            AppMain.gmFixRingCountPartSetDispDigits(part_ringcount, true);
        AppMain.gmFixRingCountPartUpdateActionDigitsType(part_ringcount);
    }

    private static void gmFixRingCountPartUpdateDigitList(
      AppMain.GMS_FIX_PART_RINGCOUNT part_ringcount)
    {
        int ringNum = (int)AppMain.gmFixGetRingNum();
        AppMain.AkUtilNumValueToDigits(ringNum, part_ringcount.digit_list, 3);
        if (ringNum != 0)
            return;
        for (int index = 0; index < 3; ++index)
            part_ringcount.digit_list[index] = 10;
    }

    private static void gmFixRingCountPartUpdateActionDigitsType(
      AppMain.GMS_FIX_PART_RINGCOUNT part_ringcount)
    {
        for (int index = 0; index < 3; ++index)
            AppMain.gmFixSetFrameStatic((AppMain.OBS_OBJECT_WORK)part_ringcount.sub_parts[AppMain.gm_fix_part_ring_count_digit_subpart_idx_tbl[index]], AppMain.gm_fix_part_ring_count_digit_type_frame_tbl[part_ringcount.digit_list[index]]);
    }

    private static void gmFixRingCountPartSetDispDigits(
      AppMain.GMS_FIX_PART_RINGCOUNT part_ringcount,
      bool enable)
    {
        for (int index = 0; index < 4; ++index)
        {
            if (index != 0)
            {
                if (enable)
                    ((AppMain.OBS_OBJECT_WORK)part_ringcount.sub_parts[index]).disp_flag &= 4294967263U;
                else
                    ((AppMain.OBS_OBJECT_WORK)part_ringcount.sub_parts[index]).disp_flag |= 32U;
            }
        }
    }

    private static void gmFixScorePartInit(AppMain.GMS_FIX_MGR_WORK mgr_work)
    {
        int num = 0;
        AppMain.GMS_FIX_PART_WORK partScore = (AppMain.GMS_FIX_PART_WORK)mgr_work.part_score;
        AppMain.gmFixRegisterPart(mgr_work, partScore, 1);
        partScore.proc_update = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixScorePartProcUpdateMain);
        partScore.proc_disp = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixScorePartProcDispMain);
        if (AppMain.gmFixIsStage22())
        {
            AppMain.act_id_tblgmFixScorePartInit = AppMain.gm_fix_score_stage22_act_id_tbl[AppMain.gmFixGetPlan()];
        }
        else
        {
            AppMain.act_id_tblgmFixScorePartInit = AppMain.gm_fix_score_act_id_tbl;
            num = 0;
        }
        for (int index = 0; index < 10; ++index)
        {
            if (AppMain.act_id_tblgmFixScorePartInit[index] < 0)
            {
                ((AppMain.GMS_FIX_PART_SCORE)partScore).sub_parts[index] = (AppMain.GMS_COCKPIT_2D_WORK)null;
            }
            else
            {
                AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "FIX_SCORE");
                AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (AppMain.GMS_COCKPIT_2D_WORK)work;
                work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmFixSubpartOutFunc);
                AppMain.ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.gm_fix_ama_amb_idx_tbl[AppMain.GsEnvGetLanguage()][0], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(AppMain.gm_fix_textures[0]), (uint)AppMain.act_id_tblgmFixScorePartInit[index], 0);
                AppMain.gmFixSetFrameStatic(work, 0.0f);
                work.pos.x += num;
                if (index != 0)
                    work.pos.y += AppMain.FX_F32_TO_FX32(2f);
                ((AppMain.GMS_FIX_PART_SCORE)partScore).sub_parts[index] = (AppMain.GMS_COCKPIT_2D_WORK)work;
            }
        }
    }

    private static void gmFixScorePartProcUpdateMain(AppMain.GMS_FIX_PART_WORK part_work)
    {
        AppMain.gmFixScorePartUpdateDigitList((AppMain.GMS_FIX_PART_SCORE)part_work);
    }

    private static void gmFixScorePartProcDispMain(AppMain.GMS_FIX_PART_WORK part_work)
    {
        AppMain.gmFixScorePartUpdateActionDigitsType((AppMain.GMS_FIX_PART_SCORE)part_work);
    }

    private static void gmFixScorePartUpdateDigitList(AppMain.GMS_FIX_PART_SCORE part_score)
    {
        AppMain.AkUtilNumValueToDigits((int)AppMain.gmFixGetScore(), part_score.digit_list, 9);
    }

    private static void gmFixScorePartUpdateActionDigitsType(AppMain.GMS_FIX_PART_SCORE part_score)
    {
        for (int index = 0; index < 9; ++index)
            AppMain.gmFixSetFrameStatic((AppMain.OBS_OBJECT_WORK)part_score.sub_parts[AppMain.gm_fix_part_score_digit_subpart_idx_tbl[index]], AppMain.gm_fix_part_common_digit_type_frame_tbl[part_score.digit_list[index]]);
    }

    private static void gmFixTimerPartInit(AppMain.GMS_FIX_MGR_WORK mgr_work)
    {
        AppMain.GMS_FIX_PART_WORK partTimer1 = (AppMain.GMS_FIX_PART_WORK)mgr_work.part_timer;
        AppMain.GMS_FIX_PART_TIMER partTimer2 = mgr_work.part_timer;
        AppMain.gmFixRegisterPart(mgr_work, partTimer1, 2);
        partTimer1.proc_update = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixTimerPartProcUpdateMain);
        partTimer1.proc_disp = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixTimerPartProcDispMain);
        if (AppMain.gmFixIsTimeAttack())
        {
            AppMain.act_id_tblgmFixTimerPartInit = AppMain.gm_fix_timer_timeattack_act_id_tbl;
            partTimer2.flag |= 1U;
        }
        else
            AppMain.act_id_tblgmFixTimerPartInit = AppMain.gm_fix_timer_act_id_tbl;
        for (int index = 0; index < 8; ++index)
        {
            AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "FIX_TIMER");
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (AppMain.GMS_COCKPIT_2D_WORK)work;
            work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmFixSubpartOutFunc);
            AppMain.ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.gm_fix_ama_amb_idx_tbl[AppMain.GsEnvGetLanguage()][0], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(AppMain.gm_fix_textures[0]), (uint)AppMain.act_id_tblgmFixTimerPartInit[index], 0);
            AppMain.gmFixSetFrameStatic(work, 0.0f);
            if (AppMain.gmFixIsTimeAttack())
            {
                work.pos.x += AppMain.FX_F32_TO_FX32(-98f);
                if (index != 0)
                    work.pos.y += AppMain.FX_F32_TO_FX32(5f);
            }
            else if (index != 0)
                work.pos.y += AppMain.FX_F32_TO_FX32(5f);
            ((AppMain.GMS_FIX_PART_TIMER)partTimer1).sub_parts[index] = (AppMain.GMS_COCKPIT_2D_WORK)work;
        }
    }

    private static void gmFixTimerSSPartInit(AppMain.GMS_FIX_MGR_WORK mgr_work)
    {
        AppMain.GMS_FIX_PART_WORK partTimer1 = (AppMain.GMS_FIX_PART_WORK)mgr_work.part_timer;
        AppMain.GMS_FIX_PART_TIMER partTimer2 = mgr_work.part_timer;
        AppMain.gmFixRegisterPart(mgr_work, partTimer1, 2);
        partTimer1.proc_update = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixTimerPartProcUpdateMain);
        partTimer1.proc_disp = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixTimerPartProcDispMain);
        partTimer2.flag |= 1U;
        for (int index1 = 0; index1 < 8; ++index1)
        {
            AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "FIX_TIMER_SS");
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (AppMain.GMS_COCKPIT_2D_WORK)work;
            int index2;
            int index3;
            if (index1 == 0)
            {
                index2 = 2;
                index3 = 2;
            }
            else
            {
                index2 = 1;
                index3 = 1;
            }
            work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmFixSubpartOutFunc);
            AppMain.ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.gm_fix_ama_amb_idx_tbl[AppMain.GsEnvGetLanguage()][index2], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(AppMain.gm_fix_textures[index3]), (uint)AppMain.gm_fix_timer_ss_act_id_tbl[AppMain.GsEnvGetLanguage()][index1], 0);
            AppMain.gmFixSetFrameStatic(work, 0.0f);
            ((AppMain.GMS_FIX_PART_TIMER)partTimer1).sub_parts[index1] = (AppMain.GMS_COCKPIT_2D_WORK)work;
        }
    }

    private static void gmFixTimerPartProcUpdateMain(AppMain.GMS_FIX_PART_WORK part_work)
    {
        AppMain.GMS_FIX_PART_TIMER part_timer = (AppMain.GMS_FIX_PART_TIMER)part_work;
        AppMain.gmFixTimerPartUpdateDigitList(part_timer);
        if (AppMain.gmFixProcessRequest(part_work.parent_mgr, 1U))
            AppMain.gmFixTimerPartInitFlashAction(part_timer);
        else
            AppMain.gmFixTimerPartUpdateFlashAction(part_timer);
        if (AppMain.gmFixTimerPartIsTimeRunningOut(part_work.parent_mgr))
        {
            ushort? sec = new ushort?((ushort)0);
            ushort? nullable1 = new ushort?();
            AppMain.AkUtilFrame60ToTime(AppMain.gmFixGetGameTime(), ref nullable1, ref sec, ref nullable1);
            if (((int)part_work.flag & 4) == 0)
            {
                part_work.flag |= 4U;
                AppMain.gmFixInitBlink(part_work, 10U, 10U);
                AppMain.GMS_FIX_PART_TIMER gmsFixPartTimer = part_timer;
                ushort? nullable2 = sec;
                int num = (int)(ushort)(nullable2.HasValue ? new int?((int)nullable2.GetValueOrDefault() - 1) : new int?()).Value;
                gmsFixPartTimer.cur_sec = (ushort)num;
            }
            int curSec = (int)part_timer.cur_sec;
            ushort? nullable3 = sec;
            if ((curSec != (int)nullable3.GetValueOrDefault() ? 1 : (!nullable3.HasValue ? 1 : 0)) == 0)
                return;
            part_timer.cur_sec = sec.Value;
            AppMain.GmSoundPlaySE("Countdown");
        }
        else
            part_work.flag &= 4294967291U;
    }

    private static void gmFixTimerPartProcDispMain(AppMain.GMS_FIX_PART_WORK part_work)
    {
        AppMain.GMS_FIX_PART_TIMER part_timer = (AppMain.GMS_FIX_PART_TIMER)part_work;
        if (((int)part_work.flag & 4) != 0)
        {
            AppMain.gmFixTimerPartSetDigitsRed(part_timer, true);
            if (AppMain.gmFixUpdateBlink(part_work))
                part_work.flag &= 4294967287U;
            else
                part_work.flag |= 8U;
        }
        else
        {
            AppMain.gmFixTimerPartSetDigitsRed(part_timer, false);
            part_work.flag &= 4294967287U;
        }
        if (((int)part_work.flag & 8) != 0)
            AppMain.gmFixTimerPartSetDispDigits(part_timer, false);
        else
            AppMain.gmFixTimerPartSetDispDigits(part_timer, true);
        AppMain.gmFixTimerPartUpdateActionDigitsType(part_timer);
    }

    private static void gmFixTimerPartUpdateDigitList(AppMain.GMS_FIX_PART_TIMER part_timer)
    {
        ushort msec = 0;
        ushort sec = 0;
        ushort min = 0;
        AppMain.AkUtilFrame60ToTime(AppMain.gmFixGetGameTime(), ref min, ref sec, ref msec);
        int _offset1 = 0;
        AppMain.AkUtilNumValueToDigits((int)msec, new AppMain.ArrayPointer<int>(part_timer.digit_list, _offset1), AppMain.digit_num_listgmFixTimerPartUpdateDigitList[0]);
        int _offset2 = _offset1 + AppMain.digit_num_listgmFixTimerPartUpdateDigitList[0];
        AppMain.AkUtilNumValueToDigits((int)sec, new AppMain.ArrayPointer<int>(part_timer.digit_list, _offset2), AppMain.digit_num_listgmFixTimerPartUpdateDigitList[1]);
        int _offset3 = _offset2 + AppMain.digit_num_listgmFixTimerPartUpdateDigitList[1];
        AppMain.AkUtilNumValueToDigits((int)min, new AppMain.ArrayPointer<int>(part_timer.digit_list, _offset3), AppMain.digit_num_listgmFixTimerPartUpdateDigitList[2]);
        int num = _offset3 + AppMain.digit_num_listgmFixTimerPartUpdateDigitList[2];
    }

    private static void gmFixTimerPartUpdateActionDigitsType(AppMain.GMS_FIX_PART_TIMER part_timer)
    {
        for (int index = 0; index < 5; ++index)
            AppMain.gmFixSetFrameStatic((AppMain.OBS_OBJECT_WORK)part_timer.sub_parts[AppMain.gm_fix_part_timer_digit_subpart_idx_tbl[index]], AppMain.gm_fix_part_common_digit_type_frame_tbl[part_timer.digit_list[index]] + part_timer.digit_frame_ofst);
        for (int index = 0; index < 2; ++index)
            AppMain.gmFixSetFrameStatic((AppMain.OBS_OBJECT_WORK)part_timer.sub_parts[AppMain.gm_fix_part_timer_deco_char_subpart_idx_tbl[index]], 0.0f + part_timer.deco_char_frame_ofst);
    }

    private static void gmFixTimerPartSetDispDigits(
      AppMain.GMS_FIX_PART_TIMER part_timer,
      bool enable)
    {
        for (int index = 0; index < 8; ++index)
        {
            if (index != 0)
            {
                if (enable)
                    ((AppMain.OBS_OBJECT_WORK)part_timer.sub_parts[index]).disp_flag &= 4294967263U;
                else
                    ((AppMain.OBS_OBJECT_WORK)part_timer.sub_parts[index]).disp_flag |= 32U;
            }
        }
    }

    private static void gmFixTimerPartSetDigitsRed(AppMain.GMS_FIX_PART_TIMER part_timer, bool enable)
    {
        if (((int)part_timer.flag & 1) != 0)
            AppMain.gmFixTimerPartSetTexRedDigits(part_timer, enable);
        else
            AppMain.gmFixTimerPartSetColorRedDigits(part_timer, enable);
    }

    private static void gmFixTimerPartSetColorRedDigits(
      AppMain.GMS_FIX_PART_TIMER part_timer,
      bool enable)
    {
        for (int index = 0; index < 8; ++index)
        {
            AppMain.OBS_OBJECT_WORK subPart = (AppMain.OBS_OBJECT_WORK)part_timer.sub_parts[index];
            if (index != 0)
                subPart.obj_2d.color.c = !enable ? uint.MaxValue : 4278190335U;
        }
    }

    private static void gmFixTimerPartSetTexRedDigits(
      AppMain.GMS_FIX_PART_TIMER part_timer,
      bool enable)
    {
        if (enable)
        {
            part_timer.digit_frame_ofst = 10f;
            part_timer.deco_char_frame_ofst = 1f;
        }
        else
        {
            part_timer.digit_frame_ofst = 0.0f;
            part_timer.deco_char_frame_ofst = 0.0f;
        }
    }

    private static bool gmFixTimerPartIsTimeRunningOut(AppMain.GMS_FIX_MGR_WORK mgr_work)
    {
        ushort min = 0;
        ushort sec = 0;
        AppMain.AkUtilFrame60ToTime(AppMain.gmFixGetGameTime(), ref min, ref sec);
        return ((int)mgr_work.flag & 4) != 0 ? min <= (ushort)0 && sec < (ushort)20 : min >= (ushort)9 && (uint)sec + 20U >= 60U;
    }

    private static void gmFixTimerPartInitFlashAction(AppMain.GMS_FIX_PART_TIMER part_timer)
    {
        part_timer.flag |= 2U;
        part_timer.fade_ratio = 0.0f;
        part_timer.scale_ratio = 0.0f;
        part_timer.flash_act_phase = 0U;
    }

    private static void gmFixTimerPartUpdateFlashAction(AppMain.GMS_FIX_PART_TIMER part_timer)
    {
        bool flag = false;
        if (((int)part_timer.flag & 2) == 0)
            return;
        switch (part_timer.flash_act_phase)
        {
            case 0:
                part_timer.fade_ratio += 0.5f;
                part_timer.scale_ratio += 0.25f;
                part_timer.fade_ratio = AppMain.MTM_MATH_CLIP(part_timer.fade_ratio, 0.0f, 1f);
                part_timer.scale_ratio = AppMain.MTM_MATH_CLIP(part_timer.fade_ratio, 0.0f, 1f);
                if ((double)part_timer.scale_ratio >= 1.0)
                {
                    ++part_timer.flash_act_phase;
                    break;
                }
                break;
            case 1:
                part_timer.fade_ratio -= 0.02f;
                part_timer.scale_ratio -= 0.05f;
                part_timer.fade_ratio = AppMain.MTM_MATH_CLIP(part_timer.fade_ratio, 0.0f, 1f);
                part_timer.scale_ratio = AppMain.MTM_MATH_CLIP(part_timer.scale_ratio, 0.0f, 1f);
                if ((double)part_timer.fade_ratio <= 0.0)
                {
                    ++part_timer.flash_act_phase;
                    break;
                }
                break;
            default:
                flag = true;
                break;
        }
        uint[] numArray = new uint[7]
        {
      1U,
      2U,
      3U,
      4U,
      5U,
      6U,
      7U
        };
        uint length = (uint)numArray.Length;
        if (!flag)
        {
            int num1 = 4096 + (int)((double)part_timer.scale_ratio * 2048.0);
            byte num2 = (byte)AppMain.MTM_MATH_CLIP((float)byte.MaxValue * part_timer.fade_ratio, 0.0f, (float)byte.MaxValue);
            for (uint index = 0; index < length; ++index)
            {
                AppMain.OBS_OBJECT_WORK subPart = (AppMain.OBS_OBJECT_WORK)part_timer.sub_parts[(int)numArray[(int)index]];
                subPart.scale.x = subPart.scale.y = num1;
                subPart.obj_2d.fade.b = subPart.obj_2d.fade.g = subPart.obj_2d.fade.r = byte.MaxValue;
                subPart.obj_2d.fade.a = num2;
            }
        }
        else
        {
            part_timer.fade_ratio = 0.0f;
            part_timer.scale_ratio = 0.0f;
            part_timer.flash_act_phase = 0U;
            for (uint index = 0; index < length; ++index)
            {
                AppMain.OBS_OBJECT_WORK subPart = (AppMain.OBS_OBJECT_WORK)part_timer.sub_parts[(int)numArray[(int)index]];
                subPart.scale.x = subPart.scale.y = 4096;
                subPart.obj_2d.fade.a = subPart.obj_2d.fade.b = subPart.obj_2d.fade.g = subPart.obj_2d.fade.r = (byte)0;
            }
            part_timer.flag &= 4294967293U;
        }
    }

    private static void gmFixChallengePartInit(AppMain.GMS_FIX_MGR_WORK mgr_work)
    {
        AppMain.GMS_FIX_PART_WORK partChallenge = (AppMain.GMS_FIX_PART_WORK)mgr_work.part_challenge;
        AppMain.gmFixRegisterPart(mgr_work, partChallenge, 3);
        partChallenge.proc_update = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixChallengePartProcUpdateMain);
        partChallenge.proc_disp = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixChallengePartProcDispMain);
        for (int index = 0; index < 5; ++index)
        {
            AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "FIX_CHALLENGE");
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (AppMain.GMS_COCKPIT_2D_WORK)work;
            work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmFixSubpartOutFunc);
            AppMain.ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.gm_fix_ama_amb_idx_tbl[AppMain.GsEnvGetLanguage()][0], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(AppMain.gm_fix_textures[0]), (uint)AppMain.gm_fix_challenge_act_id_tbl[AppMain.gmFixGetPlan()][index], 0);
            AppMain.gmFixSetFrameStatic(work, 0.0f);
            ((AppMain.GMS_FIX_PART_CHALLENGE)partChallenge).sub_parts[index] = (AppMain.GMS_COCKPIT_2D_WORK)work;
        }
    }

    private static void gmFixChallengePartProcUpdateMain(AppMain.GMS_FIX_PART_WORK part_work)
    {
        AppMain.GMS_FIX_PART_CHALLENGE part_challenge = (AppMain.GMS_FIX_PART_CHALLENGE)part_work;
        float frame = 0.0f;
        if (AppMain.g_gm_main_system.ply_work[0] != null)
            frame = ((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 16384) == 0 ? 0.0f : 1f;
        AppMain.gmFixSetFrameStatic((AppMain.OBS_OBJECT_WORK)part_challenge.sub_parts[0], frame);
        AppMain.gmFixChallengePartUpdateDigitList(part_challenge);
    }

    private static void gmFixChallengePartProcDispMain(AppMain.GMS_FIX_PART_WORK part_work)
    {
        AppMain.gmFixChallengePartUpdateActionDigitsType((AppMain.GMS_FIX_PART_CHALLENGE)part_work);
    }

    private static void gmFixChallengePartUpdateDigitList(
      AppMain.GMS_FIX_PART_CHALLENGE part_challenge)
    {
        AppMain.AkUtilNumValueToDigits((int)AppMain.gmFixGetChallengeNum(), part_challenge.digit_list, 3);
    }

    private static void gmFixChallengePartUpdateActionDigitsType(
      AppMain.GMS_FIX_PART_CHALLENGE part_challenge)
    {
        for (int index = 0; index < 3; ++index)
            AppMain.gmFixSetFrameStatic((AppMain.OBS_OBJECT_WORK)part_challenge.sub_parts[AppMain.gm_fix_part_challenge_digit_subpart_idx_tbl[index]], AppMain.gm_fix_part_common_digit_type_frame_tbl[part_challenge.digit_list[index]]);
    }

    private static void gmFixVirtualPadPartInit(AppMain.GMS_FIX_MGR_WORK mgr_work)
    {
        CPadVirtualPad cpadVirtualPad = CPadVirtualPad.CreateInstance();
        if (!cpadVirtualPad.IsValid())
            return;

        AppMain.MPP_VOID_OBS_OBJECT_WORK[] voidObsObjectWorkArray = new AppMain.MPP_VOID_OBS_OBJECT_WORK[4]
        {
              new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmFixVirtualPadOutClassGMD_FIX_MGR_FLAG_HIDE_VIRTUAL_PAD_PART_SUPER_SONIC.OutFunc),
              new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmFixVirtualPadOutClassGMD_FIX_MGR_FLAG_HIDE_VIRTUAL_PAD_PART_PAUSE.OutFunc),
              new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmFixVirtualPadOutClassGMD_FIX_MGR_FLAG_HIDE_VIRTUAL_PAD_PART_ACTION.OutFunc),
              new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmFixVirtualPadOutClassGMD_FIX_MGR_FLAG_HIDE_VIRTUAL_PAD_PART_MOVE_PAD.OutFunc)
        };
        AppMain.GMS_FIX_PART_WORK partVirtualPad = (AppMain.GMS_FIX_PART_WORK)mgr_work.part_virtual_pad;
        AppMain.gmFixRegisterPart(mgr_work, partVirtualPad, 4);
        partVirtualPad.proc_update = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixVirtualPadPartProcUpdateMain);
        partVirtualPad.proc_disp = new AppMain.MPP_VOID_GMS_FIX_PART_WORK(AppMain.gmFixVirtualPadPartProcDispMain);
        int index1 = 0;
        for (int index2 = 4; index1 < index2; ++index1)
        {
            if (AppMain.gm_fix_virtual_pad_act_id_tbl[AppMain.gmFixGetPlan()][index1] < 0)
            {
                ((AppMain.GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).sub_parts[index1] = (AppMain.GMS_COCKPIT_2D_WORK)null;
            }
            else
            {
                AppMain.OBS_OBJECT_WORK work = AppMain.GMM_COCKPIT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "FIX_VIRTUAL_PAD");
                AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (AppMain.GMS_COCKPIT_2D_WORK)work;
                work.ppOut = voidObsObjectWorkArray[index1];
                AppMain.ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, AppMain.gm_fix_ama_amb_idx_tbl[AppMain.GsEnvGetLanguage()][0], AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(AppMain.gm_fix_textures[0]), (uint)AppMain.gm_fix_virtual_pad_act_id_tbl[AppMain.gmFixGetPlan()][index1], 0);
                AppMain.amFlagOff(ref work.disp_flag, 32U);
                AppMain.gmFixSetFrameStatic(work, 0.0f);
                if (1 == index1)
                {
                    if (AppMain.gmFixIsSpecialStage())
                        work.pos.x += AppMain.FX_F32_TO_FX32(400f);
                    else if (AppMain.gmFixIsTimeAttack())
                        work.pos.x += AppMain.FX_F32_TO_FX32(200f);
                }
              ((AppMain.GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).sub_parts[index1] = (AppMain.GMS_COCKPIT_2D_WORK)work;
            }
        }
        switch (AppMain.GsEnvGetLanguage())
        {
            case 3:
            case 5:
                ((AppMain.GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).pause_icon_frame[0] = 2f;
                ((AppMain.GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).pause_icon_frame[1] = 3f;
                break;
            default:
                ((AppMain.GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).pause_icon_frame[0] = 0.0f;
                ((AppMain.GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).pause_icon_frame[1] = 1f;
                break;
        }
        AppMain.amFlagOff(ref partVirtualPad.flag, 2U);
    }

    private static void gmFixVirtualPadPartProcUpdateMain(AppMain.GMS_FIX_PART_WORK part_work)
    {
        AppMain.GMS_FIX_PART_VIRTUAL_PAD pArg = (AppMain.GMS_FIX_PART_VIRTUAL_PAD)part_work;
        AppMain.OBS_OBJECT_WORK subPart1 = (AppMain.OBS_OBJECT_WORK)pArg.sub_parts[0];
        if (AppMain.gmFixVirtualPadPartIsDispSuperSonicIcon(pArg))
            AppMain.amFlagOff(ref subPart1.disp_flag, 32U);
        else
            AppMain.amFlagOn(ref subPart1.disp_flag, 32U);
        AppMain.OBS_OBJECT_WORK subPart2 = (AppMain.OBS_OBJECT_WORK)pArg.sub_parts[1];
        if (AppMain.gmFixVirtualPadPartIsDispPauseIcon(pArg))
        {
            AppMain.amFlagOff(ref subPart2.disp_flag, 32U);
            if (AppMain.gmFixVirtualPadPartIsOnPauseIcon(pArg))
                AppMain.gmFixSetFrameStatic(subPart2, pArg.pause_icon_frame[1]);
            else
                AppMain.gmFixSetFrameStatic(subPart2, pArg.pause_icon_frame[0]);
        }
        else
            AppMain.amFlagOn(ref subPart2.disp_flag, 32U);
        switch (AppMain.gmFixGetPlan())
        {
            case 1:
                AppMain.OBS_OBJECT_WORK subPart3 = (AppMain.OBS_OBJECT_WORK)pArg.sub_parts[2];
                if (AppMain.gmFixVirtualPadPartIsOnActionIcon(pArg))
                {
                    AppMain.gmFixSetFrameStatic(subPart3, 1f);
                    break;
                }
                AppMain.gmFixSetFrameStatic(subPart3, 0.0f);
                break;
            case 2:
                AppMain.gmFixSetFrameStatic((AppMain.OBS_OBJECT_WORK)pArg.sub_parts[3], AppMain.gmFixVirtualPadPartGetMovePadFrame(pArg));
                goto case 1;
        }
    }

    private static void gmFixVirtualPadPartProcDispMain(AppMain.GMS_FIX_PART_WORK pArg)
    {
    }

    private static bool gmFixVirtualPadPartIsDispSuperSonicIcon(AppMain.GMS_FIX_PART_VIRTUAL_PAD pArg)
    {
        return AppMain.GmPlayerIsTransformSuperSonic(AppMain.g_gm_main_system.ply_work[0]);
    }

    private static bool gmFixVirtualPadPartIsDispPauseIcon(AppMain.GMS_FIX_PART_VIRTUAL_PAD pArg)
    {
        return true;
    }

    private static bool gmFixVirtualPadPartIsOnPauseIcon(AppMain.GMS_FIX_PART_VIRTUAL_PAD pArg)
    {
        bool flag = false;
        if ((AppMain.GmPauseCheckExecutable() || ((int)AppMain.g_gm_main_system.game_flag & 192) != 0) && 0 <= AppMain.GmMainKeyCheckPauseKeyOn())
            flag = true;
        return flag;
    }

    private static float gmFixVirtualPadPartGetMovePadFrame(AppMain.GMS_FIX_PART_VIRTUAL_PAD pArg)
    {
        ushort num1 = CPadVirtualPad.CreateInstance().GetValue();
        float num2 = 0.0f;
        for (int index = 0; index < AppMain.c_key_to_frame_table.Length; ++index)
        {
            AppMain.SKeyToFrame skeyToFrame = AppMain.c_key_to_frame_table[index];
            if ((skeyToFrame.key & (int)num1) != 0)
            {
                num2 = skeyToFrame.frame;
                break;
            }
        }
        return num2;
    }

    private static bool gmFixVirtualPadPartIsOnActionIcon(AppMain.GMS_FIX_PART_VIRTUAL_PAD pArg)
    {
        return AppMain.GmPlayerKeyCheckJumpKeyOn(AppMain.g_gm_main_system.ply_work[0]);
    }


}