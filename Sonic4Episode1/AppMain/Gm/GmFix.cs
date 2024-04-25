public partial class AppMain
{
    private static void GmFixBuildDataInit()
    {
        for (int index = 0; index < 3; ++index)
            gm_fix_textures[index].Clear();
        for (int index = 0; index < 3; ++index)
        {
            gm_fix_texamb_list[index] = ObjDataLoadAmbIndex(null, gm_fix_tex_amb_idx_tbl[GsEnvGetLanguage()][index], GmGameDatGetCockpitData());
            AoTexBuild(gm_fix_textures[index], (AMS_AMB_HEADER)gm_fix_texamb_list[index]);
            AoTexLoad(gm_fix_textures[index]);
        }
    }

    private static bool GmFixBuildDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < 3; ++index)
        {
            if (!AoTexIsLoaded(gm_fix_textures[index]))
                flag = false;
        }
        return flag;
    }

    private static void GmFixFlushDataInit()
    {
        for (int index = 0; index < 3; ++index)
            AoTexRelease(gm_fix_textures[index]);
    }

    private static bool GmFixFlushDataLoop()
    {
        bool flag = true;
        for (int index = 0; index < 3; ++index)
        {
            if (gm_fix_texamb_list[index] != null)
            {
                if (!AoTexIsReleased(gm_fix_textures[index]))
                {
                    flag = false;
                }
                else
                {
                    gm_fix_texamb_list[index] = null;
                    gm_fix_textures[index].Clear();
                }
            }
        }
        return flag;
    }

    private static void GmFixInit()
    {
        gm_fix_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmFixProcMain), new GSF_TASK_PROCEDURE(gmFixDest), 0U, 0, 18432U, 5, () => new GMS_FIX_MGR_WORK(), "GM_FIX_MGR");
        GMS_FIX_MGR_WORK work = (GMS_FIX_MGR_WORK)gm_fix_tcb.work;
        work.Clear();
        GMF_FIX_PART_INIT_FUNC[] gmfFixPartInitFuncArray;
        if (gmFixIsSpecialStage())
        {
            gmfFixPartInitFuncArray = gm_fix_ss_part_init_func_tbl;
            work.flag |= 4U;
        }
        else
            gmfFixPartInitFuncArray = !gmFixIsTimeAttack() ? gm_fix_part_init_func_tbl : (!gmFixIsStage22() ? gm_fix_ta_part_init_func_tbl : gm_fix_ta_s22_part_init_func_tbl);
        for (int index = 0; index < 5; ++index)
        {
            if (gmfFixPartInitFuncArray[index] != null)
                gmfFixPartInitFuncArray[index](work);
        }
        work.proc_update = null;
    }

    private static void GmFixExit()
    {
        if (gm_fix_tcb == null)
            return;
        mtTaskClearTcb(gm_fix_tcb);
        gm_fix_tcb = null;
    }

    private static void GmFixSetDisp(bool enable)
    {
        GmFixSetDispEx(enable, enable, enable, enable, enable);
    }

    private static void GmFixSetDispEx(
      bool enable,
      bool enable_ss,
      bool enable_pause,
      bool enable_action,
      bool enable_move)
    {
        if (gm_fix_tcb == null)
            return;
        GMS_FIX_MGR_WORK work = (GMS_FIX_MGR_WORK)gm_fix_tcb.work;
        if (work == null)
            return;
        int num1 = (enable ? 2 : 0) | (enable_ss ? 512 : 0) | (enable_pause ? 1024 : 0) | (enable_action ? 2048 : 0) | (enable_move ? 4096 : 0);
        int num2 = (enable ? 0 : 2) | (enable_ss ? 0 : 512) | (enable_pause ? 0 : 1024) | (enable_action ? 0 : 2048) | (enable_move ? 0 : 4096);
        work.flag &= (uint)~num1;
        work.flag |= (uint)num2;
    }

    private static bool GmFixIsDisp()
    {
        if (gm_fix_tcb != null)
        {
            GMS_FIX_MGR_WORK work = (GMS_FIX_MGR_WORK)gm_fix_tcb.work;
            if (work != null && ((int)work.flag & 2) == 0)
                return true;
        }
        return false;
    }

    private static void GmFixRequestTimerFlash()
    {
        if (gm_fix_tcb == null)
            return;
        GMS_FIX_MGR_WORK work = (GMS_FIX_MGR_WORK)gm_fix_tcb.work;
        if (work == null)
            return;
        work.req_flag |= 1U;
    }

    private static void gmFixSubpartOutFunc(OBS_OBJECT_WORK obj_work)
    {
        if (gm_fix_tcb == null || ((int)((GMS_FIX_MGR_WORK)gm_fix_tcb.work).flag & 2) != 0)
            return;
        ObjDrawActionSummary(obj_work);
    }

    private static void gmFixSetFrameStatic(OBS_OBJECT_WORK obj_work, float frame)
    {
        obj_work.obj_2d.frame = frame;
        obj_work.obj_2d.speed = 0.0f;
    }

    private static void gmFixUpdatePart(GMS_FIX_PART_WORK part_work)
    {
        if (((int)part_work.flag & 1) == 0)
            return;
        if (gmFixCheckClear(part_work))
        {
            gmFixUnregisterPart(part_work);
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

    private static bool gmFixCheckClear(GMS_FIX_PART_WORK part_work)
    {
        return gm_fix_tcb == null || part_work.parent_mgr == null || ((int)part_work.parent_mgr.flag & 1) != 0;
    }

    private static bool gmFixIsSpecialStage()
    {
        switch (GsGetMainSysInfo().stage_id)
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
        return GsGetMainSysInfo().game_mode == 1;
    }

    private static bool gmFixIsStage22()
    {
        return GsGetMainSysInfo().stage_id == 5;
    }

    private static bool gmFixIsStageTruck()
    {
        return GsGetMainSysInfo().stage_id == 9;
    }

    private static int gmFixGetPlan()
    {
        int num = 0;
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        if (gmFixIsStageTruck())
        {
            if ((512 & (int)mainSysInfo.game_flag) != 0)
                num = 1;
        }
        else if (gmFixIsSpecialStage())
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
        return g_gm_main_system.ply_work[0] != null ? g_gm_main_system.ply_work[0].ring_num : (short)0;
    }

    private static uint gmFixGetScore()
    {
        return g_gm_main_system.ply_work[0] != null ? g_gm_main_system.ply_work[0].score : 0U;
    }

    private static uint gmFixGetGameTime()
    {
        return g_gm_main_system.game_time >= 35999U ? 35999U : g_gm_main_system.game_time;
    }

    private static uint gmFixGetChallengeNum()
    {
        uint num = 0;
        if (g_gm_main_system.player_rest_num[0] > 0U)
            num = g_gm_main_system.player_rest_num[0] - 1U;
        return (uint)MTM_MATH_CLIP((int)num, 0, 999);
    }

    private static void gmFixInitBlink(
      GMS_FIX_PART_WORK part_work,
      uint on_time,
      uint off_time)
    {
        part_work.blink_timer = on_time + off_time;
        part_work.blink_on_time = on_time;
        part_work.blink_off_time = off_time;
    }

    private static bool gmFixUpdateBlink(GMS_FIX_PART_WORK part_work)
    {
        if (part_work.blink_timer != 0U)
            --part_work.blink_timer;
        bool flag = part_work.blink_timer >= part_work.blink_off_time;
        if (part_work.blink_timer == 0U)
            part_work.blink_timer = part_work.blink_on_time + part_work.blink_off_time;
        return flag;
    }

    private static void gmFixDest(MTS_TASK_TCB tcb)
    {
    }

    private static void gmFixProcMain(MTS_TASK_TCB tcb)
    {
        GMS_FIX_MGR_WORK work = (GMS_FIX_MGR_WORK)tcb.work;
        if (work.proc_update != null)
            mppAssertNotImpl();
        for (int index = 0; index < 5; ++index)
        {
            if (work.part_work[index] != null)
                gmFixUpdatePart(work.part_work[index]);
        }
    }

    private static void gmFixRegisterPart(
      GMS_FIX_MGR_WORK mgr_work,
      GMS_FIX_PART_WORK part_work,
      int part_type)
    {
        mgr_work.part_work[part_type] = part_work;
        part_work.part_type = part_type;
        part_work.parent_mgr = mgr_work;
        part_work.flag |= 1U;
    }

    private static void gmFixUnregisterPart(GMS_FIX_PART_WORK part_work)
    {
        GMS_FIX_MGR_WORK parentMgr = part_work.parent_mgr;
        if (parentMgr != null)
            parentMgr.part_work[part_work.part_type] = null;
        part_work.part_type = -1;
        part_work.parent_mgr = null;
        part_work.flag &= 4294967294U;
    }

    private static bool gmFixProcessRequest(GMS_FIX_MGR_WORK mgr_work, uint req_flag_bit)
    {
        if (((int)mgr_work.req_flag & (int)req_flag_bit) == 0)
            return false;
        mgr_work.req_flag &= ~req_flag_bit;
        return true;
    }

    private static void gmFixRingCountPartInit(GMS_FIX_MGR_WORK mgr_work)
    {
        GMS_FIX_PART_WORK partRingcount = (GMS_FIX_PART_WORK)mgr_work.part_ringcount;
        gmFixRegisterPart(mgr_work, partRingcount, 0);
        partRingcount.proc_update = new MPP_VOID_GMS_FIX_PART_WORK(gmFixRingCountPartProcUpdateMain);
        partRingcount.proc_disp = new MPP_VOID_GMS_FIX_PART_WORK(gmFixRingCountPartProcDispMain);
        for (int index = 0; index < 4; ++index)
        {
            OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(() => new GMS_COCKPIT_2D_WORK(), null, 0, "FIX_RING");
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (GMS_COCKPIT_2D_WORK)work;
            work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmFixSubpartOutFunc);
            ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, null, null, gm_fix_ama_amb_idx_tbl[GsEnvGetLanguage()][0], GmGameDatGetCockpitData(), AoTexGetTexList(gm_fix_textures[0]), (uint)gm_fix_ringcount_act_id_tbl[index], 0);
            gmFixSetFrameStatic(work, 0.0f);
            if (index != 0)
                work.pos.y += FX_F32_TO_FX32(5f);
            ((GMS_FIX_PART_RINGCOUNT)partRingcount).sub_parts[index] = (GMS_COCKPIT_2D_WORK)work;
        }
    }

    private static void gmFixRingCountPartProcUpdateMain(GMS_FIX_PART_WORK part_work)
    {
        gmFixRingCountPartUpdateDigitList((GMS_FIX_PART_RINGCOUNT)part_work);
        if (gmFixGetRingNum() == 0)
        {
            if (((int)part_work.flag & 4) != 0)
                return;
            part_work.flag |= 4U;
            gmFixInitBlink(part_work, 10U, 10U);
        }
        else
            part_work.flag &= 4294967291U;
    }

    private static void gmFixRingCountPartProcDispMain(GMS_FIX_PART_WORK part_work)
    {
        GMS_FIX_PART_RINGCOUNT part_ringcount = (GMS_FIX_PART_RINGCOUNT)part_work;
        if (((int)part_work.flag & 4) != 0)
        {
            if (gmFixUpdateBlink(part_work))
                part_work.flag &= 4294967287U;
            else
                part_work.flag |= 8U;
        }
        else
            part_work.flag &= 4294967287U;
        if (((int)part_work.flag & 8) != 0)
            gmFixRingCountPartSetDispDigits(part_ringcount, false);
        else
            gmFixRingCountPartSetDispDigits(part_ringcount, true);
        gmFixRingCountPartUpdateActionDigitsType(part_ringcount);
    }

    private static void gmFixRingCountPartUpdateDigitList(
      GMS_FIX_PART_RINGCOUNT part_ringcount)
    {
        int ringNum = gmFixGetRingNum();
        AkUtilNumValueToDigits(ringNum, part_ringcount.digit_list, 3);
        if (ringNum != 0)
            return;
        for (int index = 0; index < 3; ++index)
            part_ringcount.digit_list[index] = 10;
    }

    private static void gmFixRingCountPartUpdateActionDigitsType(
      GMS_FIX_PART_RINGCOUNT part_ringcount)
    {
        for (int index = 0; index < 3; ++index)
            gmFixSetFrameStatic((OBS_OBJECT_WORK)part_ringcount.sub_parts[gm_fix_part_ring_count_digit_subpart_idx_tbl[index]], gm_fix_part_ring_count_digit_type_frame_tbl[part_ringcount.digit_list[index]]);
    }

    private static void gmFixRingCountPartSetDispDigits(
      GMS_FIX_PART_RINGCOUNT part_ringcount,
      bool enable)
    {
        for (int index = 0; index < 4; ++index)
        {
            if (index != 0)
            {
                if (enable)
                    ((OBS_OBJECT_WORK)part_ringcount.sub_parts[index]).disp_flag &= 0xFFFFFFDFU;
                else
                    ((OBS_OBJECT_WORK)part_ringcount.sub_parts[index]).disp_flag |= 0x20U;
            }
        }
    }

    private static void gmFixScorePartInit(GMS_FIX_MGR_WORK mgr_work)
    {
        int num = 0;
        GMS_FIX_PART_WORK partScore = (GMS_FIX_PART_WORK)mgr_work.part_score;
        gmFixRegisterPart(mgr_work, partScore, 1);
        partScore.proc_update = new MPP_VOID_GMS_FIX_PART_WORK(gmFixScorePartProcUpdateMain);
        partScore.proc_disp = new MPP_VOID_GMS_FIX_PART_WORK(gmFixScorePartProcDispMain);
        if (gmFixIsStage22())
        {
            act_id_tblgmFixScorePartInit = gm_fix_score_stage22_act_id_tbl[gmFixGetPlan()];
        }
        else
        {
            act_id_tblgmFixScorePartInit = gm_fix_score_act_id_tbl;
            num = 0;
        }
        for (int index = 0; index < 10; ++index)
        {
            if (act_id_tblgmFixScorePartInit[index] < 0)
            {
                ((GMS_FIX_PART_SCORE)partScore).sub_parts[index] = null;
            }
            else
            {
                OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(() => new GMS_COCKPIT_2D_WORK(), null, 0, "FIX_SCORE");
                GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (GMS_COCKPIT_2D_WORK)work;
                work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmFixSubpartOutFunc);
                ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, null, null, gm_fix_ama_amb_idx_tbl[GsEnvGetLanguage()][0], GmGameDatGetCockpitData(), AoTexGetTexList(gm_fix_textures[0]), (uint)act_id_tblgmFixScorePartInit[index], 0);
                gmFixSetFrameStatic(work, 0.0f);
                work.pos.x += num;
                if (index != 0)
                    work.pos.y += FX_F32_TO_FX32(2f);
                ((GMS_FIX_PART_SCORE)partScore).sub_parts[index] = (GMS_COCKPIT_2D_WORK)work;
            }
        }
    }

    private static void gmFixScorePartProcUpdateMain(GMS_FIX_PART_WORK part_work)
    {
        gmFixScorePartUpdateDigitList((GMS_FIX_PART_SCORE)part_work);
    }

    private static void gmFixScorePartProcDispMain(GMS_FIX_PART_WORK part_work)
    {
        gmFixScorePartUpdateActionDigitsType((GMS_FIX_PART_SCORE)part_work);
    }

    private static void gmFixScorePartUpdateDigitList(GMS_FIX_PART_SCORE part_score)
    {
        AkUtilNumValueToDigits((int)gmFixGetScore(), part_score.digit_list, 9);
    }

    private static void gmFixScorePartUpdateActionDigitsType(GMS_FIX_PART_SCORE part_score)
    {
        for (int index = 0; index < 9; ++index)
            gmFixSetFrameStatic((OBS_OBJECT_WORK)part_score.sub_parts[gm_fix_part_score_digit_subpart_idx_tbl[index]], gm_fix_part_common_digit_type_frame_tbl[part_score.digit_list[index]]);
    }

    private static void gmFixTimerPartInit(GMS_FIX_MGR_WORK mgr_work)
    {
        GMS_FIX_PART_WORK partTimer1 = (GMS_FIX_PART_WORK)mgr_work.part_timer;
        GMS_FIX_PART_TIMER partTimer2 = mgr_work.part_timer;
        gmFixRegisterPart(mgr_work, partTimer1, 2);
        partTimer1.proc_update = new MPP_VOID_GMS_FIX_PART_WORK(gmFixTimerPartProcUpdateMain);
        partTimer1.proc_disp = new MPP_VOID_GMS_FIX_PART_WORK(gmFixTimerPartProcDispMain);
        if (gmFixIsTimeAttack())
        {
            act_id_tblgmFixTimerPartInit = gm_fix_timer_timeattack_act_id_tbl;
            partTimer2.flag |= 1U;
        }
        else
            act_id_tblgmFixTimerPartInit = gm_fix_timer_act_id_tbl;
        for (int index = 0; index < 8; ++index)
        {
            OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(() => new GMS_COCKPIT_2D_WORK(), null, 0, "FIX_TIMER");
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (GMS_COCKPIT_2D_WORK)work;
            work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmFixSubpartOutFunc);
            ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, null, null, gm_fix_ama_amb_idx_tbl[GsEnvGetLanguage()][0], GmGameDatGetCockpitData(), AoTexGetTexList(gm_fix_textures[0]), (uint)act_id_tblgmFixTimerPartInit[index], 0);
            gmFixSetFrameStatic(work, 0.0f);
            if (gmFixIsTimeAttack())
            {
                work.pos.x += FX_F32_TO_FX32(-98f);
                if (index != 0)
                    work.pos.y += FX_F32_TO_FX32(5f);
            }
            else if (index != 0)
                work.pos.y += FX_F32_TO_FX32(5f);
            ((GMS_FIX_PART_TIMER)partTimer1).sub_parts[index] = (GMS_COCKPIT_2D_WORK)work;
        }
    }

    private static void gmFixTimerSSPartInit(GMS_FIX_MGR_WORK mgr_work)
    {
        GMS_FIX_PART_WORK partTimer1 = (GMS_FIX_PART_WORK)mgr_work.part_timer;
        GMS_FIX_PART_TIMER partTimer2 = mgr_work.part_timer;
        gmFixRegisterPart(mgr_work, partTimer1, 2);
        partTimer1.proc_update = new MPP_VOID_GMS_FIX_PART_WORK(gmFixTimerPartProcUpdateMain);
        partTimer1.proc_disp = new MPP_VOID_GMS_FIX_PART_WORK(gmFixTimerPartProcDispMain);
        partTimer2.flag |= 1U;
        for (int index1 = 0; index1 < 8; ++index1)
        {
            OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(() => new GMS_COCKPIT_2D_WORK(), null, 0, "FIX_TIMER_SS");
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (GMS_COCKPIT_2D_WORK)work;
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
            work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmFixSubpartOutFunc);
            ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, null, null, gm_fix_ama_amb_idx_tbl[GsEnvGetLanguage()][index2], GmGameDatGetCockpitData(), AoTexGetTexList(gm_fix_textures[index3]), (uint)gm_fix_timer_ss_act_id_tbl[GsEnvGetLanguage()][index1], 0);
            gmFixSetFrameStatic(work, 0.0f);
            ((GMS_FIX_PART_TIMER)partTimer1).sub_parts[index1] = (GMS_COCKPIT_2D_WORK)work;
        }
    }

    private static void gmFixTimerPartProcUpdateMain(GMS_FIX_PART_WORK part_work)
    {
        GMS_FIX_PART_TIMER part_timer = (GMS_FIX_PART_TIMER)part_work;
        gmFixTimerPartUpdateDigitList(part_timer);
        if (gmFixProcessRequest(part_work.parent_mgr, 1U))
            gmFixTimerPartInitFlashAction(part_timer);
        else
            gmFixTimerPartUpdateFlashAction(part_timer);
        if (gmFixTimerPartIsTimeRunningOut(part_work.parent_mgr))
        {
            ushort? sec = 0;
            ushort? ignore = null;
            AkUtilFrame60ToTime(gmFixGetGameTime(), ref ignore, ref sec, ref ignore);
            if (((int)part_work.flag & 4) == 0)
            {
                part_work.flag |= 4U;
                gmFixInitBlink(part_work, 10U, 10U);
                GMS_FIX_PART_TIMER gmsFixPartTimer = part_timer;
                int num = sec.Value - 1;
                gmsFixPartTimer.cur_sec = (ushort)num;
            }
            int curSec = part_timer.cur_sec;
            ushort? nullable3 = sec;
            if ((curSec != nullable3.GetValueOrDefault() ? 1 : (!nullable3.HasValue ? 1 : 0)) == 0)
                return;
            part_timer.cur_sec = sec.Value;
            GmSoundPlaySE("Countdown");
        }
        else
            part_work.flag &= 4294967291U;
    }

    private static void gmFixTimerPartProcDispMain(GMS_FIX_PART_WORK part_work)
    {
        GMS_FIX_PART_TIMER part_timer = (GMS_FIX_PART_TIMER)part_work;
        if (((int)part_work.flag & 4) != 0)
        {
            gmFixTimerPartSetDigitsRed(part_timer, true);
            if (gmFixUpdateBlink(part_work))
                part_work.flag &= 4294967287U;
            else
                part_work.flag |= 8U;
        }
        else
        {
            gmFixTimerPartSetDigitsRed(part_timer, false);
            part_work.flag &= 4294967287U;
        }
        if (((int)part_work.flag & 8) != 0)
            gmFixTimerPartSetDispDigits(part_timer, false);
        else
            gmFixTimerPartSetDispDigits(part_timer, true);
        gmFixTimerPartUpdateActionDigitsType(part_timer);
    }

    private static void gmFixTimerPartUpdateDigitList(GMS_FIX_PART_TIMER part_timer)
    {
        ushort msec = 0;
        ushort sec = 0;
        ushort min = 0;
        AkUtilFrame60ToTime(gmFixGetGameTime(), ref min, ref sec, ref msec);
        int _offset1 = 0;
        AkUtilNumValueToDigits(msec, new ArrayPointer<int>(part_timer.digit_list, _offset1), digit_num_listgmFixTimerPartUpdateDigitList[0]);
        int _offset2 = _offset1 + digit_num_listgmFixTimerPartUpdateDigitList[0];
        AkUtilNumValueToDigits(sec, new ArrayPointer<int>(part_timer.digit_list, _offset2), digit_num_listgmFixTimerPartUpdateDigitList[1]);
        int _offset3 = _offset2 + digit_num_listgmFixTimerPartUpdateDigitList[1];
        AkUtilNumValueToDigits(min, new ArrayPointer<int>(part_timer.digit_list, _offset3), digit_num_listgmFixTimerPartUpdateDigitList[2]);
        int num = _offset3 + digit_num_listgmFixTimerPartUpdateDigitList[2];
    }

    private static void gmFixTimerPartUpdateActionDigitsType(GMS_FIX_PART_TIMER part_timer)
    {
        for (int index = 0; index < 5; ++index)
            gmFixSetFrameStatic((OBS_OBJECT_WORK)part_timer.sub_parts[gm_fix_part_timer_digit_subpart_idx_tbl[index]], gm_fix_part_common_digit_type_frame_tbl[part_timer.digit_list[index]] + part_timer.digit_frame_ofst);
        for (int index = 0; index < 2; ++index)
            gmFixSetFrameStatic((OBS_OBJECT_WORK)part_timer.sub_parts[gm_fix_part_timer_deco_char_subpart_idx_tbl[index]], 0.0f + part_timer.deco_char_frame_ofst);
    }

    private static void gmFixTimerPartSetDispDigits(
      GMS_FIX_PART_TIMER part_timer,
      bool enable)
    {
        for (int index = 0; index < 8; ++index)
        {
            if (index != 0)
            {
                if (enable)
                    ((OBS_OBJECT_WORK)part_timer.sub_parts[index]).disp_flag &= 4294967263U;
                else
                    ((OBS_OBJECT_WORK)part_timer.sub_parts[index]).disp_flag |= 32U;
            }
        }
    }

    private static void gmFixTimerPartSetDigitsRed(GMS_FIX_PART_TIMER part_timer, bool enable)
    {
        if (((int)part_timer.flag & 1) != 0)
            gmFixTimerPartSetTexRedDigits(part_timer, enable);
        else
            gmFixTimerPartSetColorRedDigits(part_timer, enable);
    }

    private static void gmFixTimerPartSetColorRedDigits(
      GMS_FIX_PART_TIMER part_timer,
      bool enable)
    {
        for (int index = 0; index < 8; ++index)
        {
            OBS_OBJECT_WORK subPart = (OBS_OBJECT_WORK)part_timer.sub_parts[index];
            if (index != 0)
                subPart.obj_2d.color.c = !enable ? uint.MaxValue : 0xFF0000FFU;
        }
    }

    private static void gmFixTimerPartSetTexRedDigits(
      GMS_FIX_PART_TIMER part_timer,
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

    private static bool gmFixTimerPartIsTimeRunningOut(GMS_FIX_MGR_WORK mgr_work)
    {
        ushort min = 0;
        ushort sec = 0;
        AkUtilFrame60ToTime(gmFixGetGameTime(), ref min, ref sec);
        return ((int)mgr_work.flag & 4) != 0 ? min <= 0 && sec < 20 : min >= 9 && sec + 20U >= 60U;
    }

    private static void gmFixTimerPartInitFlashAction(GMS_FIX_PART_TIMER part_timer)
    {
        part_timer.flag |= 2U;
        part_timer.fade_ratio = 0.0f;
        part_timer.scale_ratio = 0.0f;
        part_timer.flash_act_phase = 0U;
    }

    private static void gmFixTimerPartUpdateFlashAction(GMS_FIX_PART_TIMER part_timer)
    {
        bool flag = false;
        if (((int)part_timer.flag & 2) == 0)
            return;
        switch (part_timer.flash_act_phase)
        {
            case 0:
                part_timer.fade_ratio += 0.5f;
                part_timer.scale_ratio += 0.25f;
                part_timer.fade_ratio = MTM_MATH_CLIP(part_timer.fade_ratio, 0.0f, 1f);
                part_timer.scale_ratio = MTM_MATH_CLIP(part_timer.fade_ratio, 0.0f, 1f);
                if (part_timer.scale_ratio >= 1.0)
                {
                    ++part_timer.flash_act_phase;
                    break;
                }
                break;
            case 1:
                part_timer.fade_ratio -= 0.02f;
                part_timer.scale_ratio -= 0.05f;
                part_timer.fade_ratio = MTM_MATH_CLIP(part_timer.fade_ratio, 0.0f, 1f);
                part_timer.scale_ratio = MTM_MATH_CLIP(part_timer.scale_ratio, 0.0f, 1f);
                if (part_timer.fade_ratio <= 0.0)
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
            int num1 = 4096 + (int)(part_timer.scale_ratio * 2048.0);
            byte num2 = (byte)MTM_MATH_CLIP(byte.MaxValue * part_timer.fade_ratio, 0.0f, byte.MaxValue);
            for (uint index = 0; index < length; ++index)
            {
                OBS_OBJECT_WORK subPart = (OBS_OBJECT_WORK)part_timer.sub_parts[(int)numArray[(int)index]];
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
                OBS_OBJECT_WORK subPart = (OBS_OBJECT_WORK)part_timer.sub_parts[(int)numArray[(int)index]];
                subPart.scale.x = subPart.scale.y = 4096;
                subPart.obj_2d.fade.a = subPart.obj_2d.fade.b = subPart.obj_2d.fade.g = subPart.obj_2d.fade.r = 0;
            }
            part_timer.flag &= 4294967293U;
        }
    }

    private static void gmFixChallengePartInit(GMS_FIX_MGR_WORK mgr_work)
    {
        GMS_FIX_PART_WORK partChallenge = (GMS_FIX_PART_WORK)mgr_work.part_challenge;
        gmFixRegisterPart(mgr_work, partChallenge, 3);
        partChallenge.proc_update = new MPP_VOID_GMS_FIX_PART_WORK(gmFixChallengePartProcUpdateMain);
        partChallenge.proc_disp = new MPP_VOID_GMS_FIX_PART_WORK(gmFixChallengePartProcDispMain);
        for (int index = 0; index < 5; ++index)
        {
            OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(() => new GMS_COCKPIT_2D_WORK(), null, 0, "FIX_CHALLENGE");
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (GMS_COCKPIT_2D_WORK)work;
            work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmFixSubpartOutFunc);
            ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, null, null, gm_fix_ama_amb_idx_tbl[GsEnvGetLanguage()][0], GmGameDatGetCockpitData(), AoTexGetTexList(gm_fix_textures[0]), (uint)gm_fix_challenge_act_id_tbl[gmFixGetPlan()][index], 0);
            gmFixSetFrameStatic(work, 0.0f);
            ((GMS_FIX_PART_CHALLENGE)partChallenge).sub_parts[index] = (GMS_COCKPIT_2D_WORK)work;
        }
    }

    private static void gmFixChallengePartProcUpdateMain(GMS_FIX_PART_WORK part_work)
    {
        GMS_FIX_PART_CHALLENGE part_challenge = (GMS_FIX_PART_CHALLENGE)part_work;
        float frame = 0.0f;
        if (g_gm_main_system.ply_work[0] != null)
            frame = (g_gm_main_system.ply_work[0].player_flag & GMD_PLF_SUPER_SONIC) == 0 ? 0.0f : 1f;
        gmFixSetFrameStatic((OBS_OBJECT_WORK)part_challenge.sub_parts[0], frame);
        gmFixChallengePartUpdateDigitList(part_challenge);
    }

    private static void gmFixChallengePartProcDispMain(GMS_FIX_PART_WORK part_work)
    {
        gmFixChallengePartUpdateActionDigitsType((GMS_FIX_PART_CHALLENGE)part_work);
    }

    private static void gmFixChallengePartUpdateDigitList(
      GMS_FIX_PART_CHALLENGE part_challenge)
    {
        AkUtilNumValueToDigits((int)gmFixGetChallengeNum(), part_challenge.digit_list, 3);
    }

    private static void gmFixChallengePartUpdateActionDigitsType(
      GMS_FIX_PART_CHALLENGE part_challenge)
    {
        for (int index = 0; index < 3; ++index)
            gmFixSetFrameStatic((OBS_OBJECT_WORK)part_challenge.sub_parts[gm_fix_part_challenge_digit_subpart_idx_tbl[index]], gm_fix_part_common_digit_type_frame_tbl[part_challenge.digit_list[index]]);
    }

    private static void gmFixVirtualPadPartInit(GMS_FIX_MGR_WORK mgr_work)
    {
        CPadVirtualPad cpadVirtualPad = CPadVirtualPad.CreateInstance();
        if (!cpadVirtualPad.IsValid())
            return;

        MPP_VOID_OBS_OBJECT_WORK[] voidObsObjectWorkArray = new MPP_VOID_OBS_OBJECT_WORK[4]
        {
              new MPP_VOID_OBS_OBJECT_WORK(gmFixVirtualPadOutClassGMD_FIX_MGR_FLAG_HIDE_VIRTUAL_PAD_PART_SUPER_SONIC.OutFunc),
              new MPP_VOID_OBS_OBJECT_WORK(gmFixVirtualPadOutClassGMD_FIX_MGR_FLAG_HIDE_VIRTUAL_PAD_PART_PAUSE.OutFunc),
              new MPP_VOID_OBS_OBJECT_WORK(gmFixVirtualPadOutClassGMD_FIX_MGR_FLAG_HIDE_VIRTUAL_PAD_PART_ACTION.OutFunc),
              new MPP_VOID_OBS_OBJECT_WORK(gmFixVirtualPadOutClassGMD_FIX_MGR_FLAG_HIDE_VIRTUAL_PAD_PART_MOVE_PAD.OutFunc)
        };
        GMS_FIX_PART_WORK partVirtualPad = (GMS_FIX_PART_WORK)mgr_work.part_virtual_pad;
        gmFixRegisterPart(mgr_work, partVirtualPad, 4);
        partVirtualPad.proc_update = new MPP_VOID_GMS_FIX_PART_WORK(gmFixVirtualPadPartProcUpdateMain);
        partVirtualPad.proc_disp = new MPP_VOID_GMS_FIX_PART_WORK(gmFixVirtualPadPartProcDispMain);
        int index1 = 0;
        for (int index2 = 4; index1 < index2; ++index1)
        {
            if (gm_fix_virtual_pad_act_id_tbl[gmFixGetPlan()][index1] < 0)
            {
                ((GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).sub_parts[index1] = null;
            }
            else
            {
                OBS_OBJECT_WORK work = GMM_COCKPIT_CREATE_WORK(() => new GMS_COCKPIT_2D_WORK(), null, 0, "FIX_VIRTUAL_PAD");
                GMS_COCKPIT_2D_WORK gmsCockpit2DWork = (GMS_COCKPIT_2D_WORK)work;
                work.ppOut = voidObsObjectWorkArray[index1];
                ObjObjectAction2dAMALoadSetTexlist(work, gmsCockpit2DWork.obj_2d, null, null, gm_fix_ama_amb_idx_tbl[GsEnvGetLanguage()][0], GmGameDatGetCockpitData(), AoTexGetTexList(gm_fix_textures[0]), (uint)gm_fix_virtual_pad_act_id_tbl[gmFixGetPlan()][index1], 0);
                amFlagOff(ref work.disp_flag, 32U);
                gmFixSetFrameStatic(work, 0.0f);
                if (1 == index1)
                {
                    if (gmFixIsSpecialStage())
                        work.pos.x += FX_F32_TO_FX32(400f);
                    else if (gmFixIsTimeAttack())
                        work.pos.x += FX_F32_TO_FX32(200f);
                }
              ((GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).sub_parts[index1] = (GMS_COCKPIT_2D_WORK)work;
            }
        }
        switch (GsEnvGetLanguage())
        {
            case 3:
            case 5:
                ((GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).pause_icon_frame[0] = 2f;
                ((GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).pause_icon_frame[1] = 3f;
                break;
            default:
                ((GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).pause_icon_frame[0] = 0.0f;
                ((GMS_FIX_PART_VIRTUAL_PAD)partVirtualPad).pause_icon_frame[1] = 1f;
                break;
        }
        amFlagOff(ref partVirtualPad.flag, 2U);
    }

    private static void gmFixVirtualPadPartProcUpdateMain(GMS_FIX_PART_WORK part_work)
    {
        GMS_FIX_PART_VIRTUAL_PAD pArg = (GMS_FIX_PART_VIRTUAL_PAD)part_work;
        OBS_OBJECT_WORK subPart1 = (OBS_OBJECT_WORK)pArg.sub_parts[0];
        if (gmFixVirtualPadPartIsDispSuperSonicIcon(pArg))
            amFlagOff(ref subPart1.disp_flag, 32U);
        else
            amFlagOn(ref subPart1.disp_flag, 32U);
        OBS_OBJECT_WORK subPart2 = (OBS_OBJECT_WORK)pArg.sub_parts[1];
        if (gmFixVirtualPadPartIsDispPauseIcon(pArg))
        {
            amFlagOff(ref subPart2.disp_flag, 32U);
            if (gmFixVirtualPadPartIsOnPauseIcon(pArg))
                gmFixSetFrameStatic(subPart2, pArg.pause_icon_frame[1]);
            else
                gmFixSetFrameStatic(subPart2, pArg.pause_icon_frame[0]);
        }
        else
            amFlagOn(ref subPart2.disp_flag, 32U);
        switch (gmFixGetPlan())
        {
            case 1:
                OBS_OBJECT_WORK subPart3 = (OBS_OBJECT_WORK)pArg.sub_parts[2];
                if (gmFixVirtualPadPartIsOnActionIcon(pArg))
                {
                    gmFixSetFrameStatic(subPart3, 1f);
                    break;
                }
                gmFixSetFrameStatic(subPart3, 0.0f);
                break;
            case 2:
                gmFixSetFrameStatic((OBS_OBJECT_WORK)pArg.sub_parts[3], gmFixVirtualPadPartGetMovePadFrame(pArg));
                goto case 1;
        }
    }

    private static void gmFixVirtualPadPartProcDispMain(GMS_FIX_PART_WORK pArg)
    {
    }

    private static bool gmFixVirtualPadPartIsDispSuperSonicIcon(GMS_FIX_PART_VIRTUAL_PAD pArg)
    {
        return GmPlayerIsTransformSuperSonic(g_gm_main_system.ply_work[0]);
    }

    private static bool gmFixVirtualPadPartIsDispPauseIcon(GMS_FIX_PART_VIRTUAL_PAD pArg)
    {
        return true;
    }

    private static bool gmFixVirtualPadPartIsOnPauseIcon(GMS_FIX_PART_VIRTUAL_PAD pArg)
    {
        bool flag = false;
        if ((GmPauseCheckExecutable() || ((int)g_gm_main_system.game_flag & 192) != 0) && 0 <= GmMainKeyCheckPauseKeyOn())
            flag = true;
        return flag;
    }

    private static float gmFixVirtualPadPartGetMovePadFrame(GMS_FIX_PART_VIRTUAL_PAD pArg)
    {
        ushort num1 = CPadVirtualPad.CreateInstance().GetValue();
        float num2 = 0.0f;
        for (int index = 0; index < c_key_to_frame_table.Length; ++index)
        {
            SKeyToFrame skeyToFrame = c_key_to_frame_table[index];
            if ((skeyToFrame.key & num1) != 0)
            {
                num2 = skeyToFrame.frame;
                break;
            }
        }
        return num2;
    }

    private static bool gmFixVirtualPadPartIsOnActionIcon(GMS_FIX_PART_VIRTUAL_PAD pArg)
    {
        return GmPlayerKeyCheckJumpKeyOn(g_gm_main_system.ply_work[0]);
    }


}