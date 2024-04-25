using System;

public partial class AppMain
{
    private static void GmSplStageStart()
    {
        ushort num = (ushort)(g_gs_main_sys_info.stage_id - 21U);
        gm_spl_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmSplStageFadeInWait), null, 0U, 0, 4176U, 5, () => new GMS_SPL_STG_WORK(), "SPL_STG_CTRL");
        GMS_SPL_STG_WORK work = (GMS_SPL_STG_WORK)gm_spl_tcb.work;
        work.counter = 0U;
        work.light_vec.x = -1f;
        work.light_vec.y = -1f;
        work.light_vec.z = -1f;
        work.get_ring = 0;
        g_gm_main_system.game_flag &= 4294508543U;
        IzFadeInitEasy(1U, 3U, 8f);
        g_gm_main_system.game_time = gm_spl_stage_init_time[num];
        g_gm_main_system.game_flag |= 4096U;
        Array.Clear(gm_gmk_ss_switch, 0, gm_gmk_ss_switch.Length);
    }

    private static void GmSplStageExit()
    {
        if (gm_spl_tcb == null)
            return;
        mtTaskClearTcb(gm_spl_tcb);
        gm_spl_tcb = null;
        g_gm_main_system.game_flag &= 4294508543U;
    }

    private static void GmSplStageSetLight()
    {
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.x = -0.4f;
        nnsVector.y = -0.4f;
        nnsVector.z = -1f;
        nnNormalizeVector(nnsVector, nnsVector);
        ObjDrawSetParallelLight(NNE_LIGHT_1, ref col, 1f, nnsVector);
    }

    private static GMS_SPL_STG_WORK GmSplStageGetWork()
    {
        return (GMS_SPL_STG_WORK)gm_spl_tcb.work;
    }

    private static void GmSplStageSwSet(uint sw_no)
    {
        sw_no &= 15U;
        gm_gmk_ss_switch[(int)sw_no] = 1;
    }

    private static bool GmSplStageSwCheck(uint sw_no)
    {
        sw_no &= 15U;
        return gm_gmk_ss_switch[(int)sw_no] != 0;
    }

    private static ushort GmSplStageRingGateNumGet(ushort gate_id)
    {
        ushort num = (ushort)(g_gs_main_sys_info.stage_id - 21U);
        return gm_spl_stage_ringgate_num[num][gate_id];
    }

    private static void gmSplStageFadeInWait(MTS_TASK_TCB tcb)
    {
        GMS_SPL_STG_WORK work = (GMS_SPL_STG_WORK)tcb.work;
        ++work.counter;
        if (work.counter <= 30U)
            return;
        work.counter = 0U;
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmSplStageFadeInWait2));
        IzFadeInitEasy(0U, 2U, 30f);
    }

    private static void gmSplStageFadeInWait2(MTS_TASK_TCB tcb)
    {
        if (ObjObjectPauseCheck(0U) != 0U || !IzFadeIsEnd())
            return;
        IzFadeExit();
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        gmsPlayerWork.obj_work.move_flag &= 4294958847U;
        gmsPlayerWork.nudge_di_timer = 0;
        gmsPlayerWork.nudge_timer = 0;
        gmsPlayerWork.nudge_ofst_x = 0;
        g_gm_main_system.game_flag |= 1024U;
        g_gm_main_system.game_flag &= 4294963199U;
        if (GmStartMsgIsExe())
            GmStartMsgInit();
        AoPresenceSet();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmSplStagePlayEndChk));
    }

    private static void gmSplStagePlayEndChk(MTS_TASK_TCB tcb)
    {
        GMS_SPL_STG_WORK work = (GMS_SPL_STG_WORK)tcb.work;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        if (ObjObjectPauseCheck(0U) != 0U)
            return;
        gmSplStageLightCtrl(work);
        work.flag &= 4294967294U;
        gmSplStageNudgeCtrl();
        gmSplStageRingGateChk(work);
        if (((int)g_gm_main_system.game_flag & 458752) == 0)
            return;
        g_gm_main_system.game_flag &= 4294966271U;
        gmsPlayerWork.obj_work.flag |= 130U;
        gmsPlayerWork.obj_work.move_flag |= 8448U;
        gmsPlayerWork.obj_work.disp_flag &= 4294967294U;
        gmsPlayerWork.player_flag |= GMD_PLF_NOKEY;
        if (((int)g_gm_main_system.game_flag & 393216) != 0)
            GMM_PAD_VIB_MID_TIME(90f);
        work.roll = obsCamera.roll;
        work.roll_spd = 256;
        work.counter = 0U;
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmSplStageRolling));
    }

    private static void gmSplStageRolling(MTS_TASK_TCB tcb)
    {
        GMS_SPL_STG_WORK work = (GMS_SPL_STG_WORK)tcb.work;
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        if (ObjObjectPauseCheck(0U) != 0U || obsCamera == null)
            return;
        work.roll_spd += 56;
        work.roll += work.roll_spd;
        obsCamera.roll = work.roll;
        ++work.counter;
        if (work.counter == 90U)
            IzFadeInitEasy(0U, 3U, 30f);
        if (work.counter <= 90U || !IzFadeIsEnd())
            return;
        IzFadeExit();
        IzFadeRestoreDrawSetting();
        GmObjSetAllObjectNoDisp();
        GmRingGetWork().flag |= 1U;
        GmFixSetDisp(false);
        work.flag |= 4U;
        work.counter = 1U;
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmSplStageGotoEnd));
        obsCamera.roll = 0;
        g_gm_main_system.pseudofall_dir = 0;
    }

    private static void gmSplStageGotoEnd(MTS_TASK_TCB tcb)
    {
        GMS_SPL_STG_WORK work = (GMS_SPL_STG_WORK)tcb.work;
        if (work.counter != 0U)
        {
            --work.counter;
        }
        else
        {
            g_gm_main_system.game_flag |= 4U;
            mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmSplStageEnd));
        }
    }

    private static void gmSplStageEnd(MTS_TASK_TCB tcb)
    {
        UNREFERENCED_PARAMETER(tcb);
    }

    private static void gmSplStageLightCtrl(GMS_SPL_STG_WORK tcb_work)
    {
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        GlobalPool<NNS_VECTOR>.Alloc();
        UNREFERENCED_PARAMETER(tcb_work);
        NNS_VECTOR nnsVector1 = gmSplStageLightRot(-1f, -1f, -1f);
        nnNormalizeVector(nnsVector1, nnsVector1);
        ObjDrawSetParallelLight(NNE_LIGHT_0, ref col, 1f, nnsVector1);
        NNS_VECTOR nnsVector2 = gmSplStageLightRot(-0.4f, -0.4f, -1f);
        nnNormalizeVector(nnsVector2, nnsVector2);
        ObjDrawSetParallelLight(NNE_LIGHT_1, ref col, 1f, nnsVector2);
    }

    private static NNS_VECTOR gmSplStageLightRot(
      float pos_x,
      float pos_y,
      float pos_z)
    {
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        float num1 = pos_x * nnSin(-g_gm_main_system.pseudofall_dir);
        float num2 = pos_x * nnCos(-g_gm_main_system.pseudofall_dir);
        float num3 = pos_y * nnSin(-g_gm_main_system.pseudofall_dir);
        float num4 = pos_y * nnCos(-g_gm_main_system.pseudofall_dir);
        nnsVector.x = num2 - num3;
        nnsVector.y = num1 + num4;
        nnsVector.z = pos_z;
        return nnsVector;
    }

    private static void gmSplStageNudgeCtrl()
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        if (gmsPlayerWork.nudge_timer != 0)
        {
            int dest_x = (gmsPlayerWork.nudge_timer * 8 << 12) / 30;
            if ((gmsPlayerWork.nudge_timer & 2) != 0)
                dest_x = -dest_x;
            int dest_y = 0;
            GmObjGetRotPosXY(dest_x, dest_y, ref gmsPlayerWork.obj_work.ofst.x, ref gmsPlayerWork.obj_work.ofst.y, g_gm_main_system.pseudofall_dir);
            GmObjGetRotPosXY(dest_x, dest_y, ref dest_x, ref dest_y, (ushort)-g_gm_main_system.pseudofall_dir);
            obsCamera.ofst.x = FXM_FX32_TO_FLOAT(dest_x);
            obsCamera.ofst.y = FXM_FX32_TO_FLOAT(dest_y);
            --gmsPlayerWork.nudge_timer;
        }
        else
        {
            obsCamera.ofst.x = 0.0f;
            obsCamera.ofst.y = 0.0f;
            gmsPlayerWork.obj_work.ofst.x = 0;
            gmsPlayerWork.obj_work.ofst.y = 0;
            gmsPlayerWork.nudge_ofst_x = 0;
        }
    }

    private static void gmSplStageRingGateChk(GMS_SPL_STG_WORK tcb_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        ushort getRing = tcb_work.get_ring;
        for (ushort gate_id = 0; gate_id < 9; ++gate_id)
        {
            if ((tcb_work.get_ring & 1 << gate_id) == 0)
            {
                ushort num = GmSplStageRingGateNumGet(gate_id);
                if (num != byte.MaxValue)
                {
                    if (num <= (ushort)gmsPlayerWork.ring_num)
                        tcb_work.get_ring |= (ushort)(1U << gate_id);
                }
                else
                    break;
            }
        }
        if (getRing == tcb_work.get_ring)
            return;
        GmSoundPlaySE("Special7");
    }

    public static void GmSpStageBranchInit(object arg)
    {
        UNREFERENCED_PARAMETER(arg);
        if (SyGetEvtInfo().old_evt_id == 5)
        {
            if (g_gs_main_sys_info.stage_id < 21 || g_gs_main_sys_info.stage_id > 27)
                g_gs_main_sys_info.stage_id = 21;
        }
        else
            g_gs_main_sys_info.stage_id = GsMainSysIsStageClear(21) ? (GsMainSysIsStageClear(22) ? (GsMainSysIsStageClear(23) ? (GsMainSysIsStageClear(24) ? (GsMainSysIsStageClear(25) ? (GsMainSysIsStageClear(26) ? (ushort)27 : (ushort)26) : (ushort)25) : (ushort)24) : (ushort)23) : (ushort)22) : (ushort)21;
        g_gs_main_sys_info.char_id[0] = 2;
        g_gs_main_sys_info.game_flag |= 128U;
        GmMainGSInit();
        SyChangeNextEvt();
    }

}