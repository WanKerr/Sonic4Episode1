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
    private static void GmSplStageStart()
    {
        ushort num = (ushort)((uint)AppMain.g_gs_main_sys_info.stage_id - 21U);
        AppMain.gm_spl_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSplStageFadeInWait), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 4176U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_SPL_STG_WORK()), "SPL_STG_CTRL");
        AppMain.GMS_SPL_STG_WORK work = (AppMain.GMS_SPL_STG_WORK)AppMain.gm_spl_tcb.work;
        work.counter = 0U;
        work.light_vec.x = -1f;
        work.light_vec.y = -1f;
        work.light_vec.z = -1f;
        work.get_ring = (ushort)0;
        AppMain.g_gm_main_system.game_flag &= 4294508543U;
        AppMain.IzFadeInitEasy(1U, 3U, 8f);
        AppMain.g_gm_main_system.game_time = AppMain.gm_spl_stage_init_time[(int)num];
        AppMain.g_gm_main_system.game_flag |= 4096U;
        Array.Clear((Array)AppMain.gm_gmk_ss_switch, 0, AppMain.gm_gmk_ss_switch.Length);
    }

    private static void GmSplStageExit()
    {
        if (AppMain.gm_spl_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_spl_tcb);
        AppMain.gm_spl_tcb = (AppMain.MTS_TASK_TCB)null;
        AppMain.g_gm_main_system.game_flag &= 4294508543U;
    }

    private static void GmSplStageSetLight()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.x = -0.4f;
        nnsVector.y = -0.4f;
        nnsVector.z = -1f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_1, ref col, 1f, nnsVector);
    }

    private static AppMain.GMS_SPL_STG_WORK GmSplStageGetWork()
    {
        return (AppMain.GMS_SPL_STG_WORK)AppMain.gm_spl_tcb.work;
    }

    private static void GmSplStageSwSet(uint sw_no)
    {
        sw_no &= 15U;
        AppMain.gm_gmk_ss_switch[(int)sw_no] = (byte)1;
    }

    private static bool GmSplStageSwCheck(uint sw_no)
    {
        sw_no &= 15U;
        return AppMain.gm_gmk_ss_switch[(int)sw_no] != (byte)0;
    }

    private static ushort GmSplStageRingGateNumGet(ushort gate_id)
    {
        ushort num = (ushort)((uint)AppMain.g_gs_main_sys_info.stage_id - 21U);
        return AppMain.gm_spl_stage_ringgate_num[(int)num][(int)gate_id];
    }

    private static void gmSplStageFadeInWait(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_SPL_STG_WORK work = (AppMain.GMS_SPL_STG_WORK)tcb.work;
        ++work.counter;
        if (work.counter <= 30U)
            return;
        work.counter = 0U;
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSplStageFadeInWait2));
        AppMain.IzFadeInitEasy(0U, 2U, 30f);
    }

    private static void gmSplStageFadeInWait2(AppMain.MTS_TASK_TCB tcb)
    {
        if (AppMain.ObjObjectPauseCheck(0U) != 0U || !AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        gmsPlayerWork.obj_work.move_flag &= 4294958847U;
        gmsPlayerWork.nudge_di_timer = (short)0;
        gmsPlayerWork.nudge_timer = (short)0;
        gmsPlayerWork.nudge_ofst_x = 0;
        AppMain.g_gm_main_system.game_flag |= 1024U;
        AppMain.g_gm_main_system.game_flag &= 4294963199U;
        if (AppMain.GmStartMsgIsExe())
            AppMain.GmStartMsgInit();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSplStagePlayEndChk));
    }

    private static void gmSplStagePlayEndChk(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_SPL_STG_WORK work = (AppMain.GMS_SPL_STG_WORK)tcb.work;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        if (AppMain.ObjObjectPauseCheck(0U) != 0U)
            return;
        AppMain.gmSplStageLightCtrl(work);
        work.flag &= 4294967294U;
        AppMain.gmSplStageNudgeCtrl();
        AppMain.gmSplStageRingGateChk(work);
        if (((int)AppMain.g_gm_main_system.game_flag & 458752) == 0)
            return;
        AppMain.g_gm_main_system.game_flag &= 4294966271U;
        gmsPlayerWork.obj_work.flag |= 130U;
        gmsPlayerWork.obj_work.move_flag |= 8448U;
        gmsPlayerWork.obj_work.disp_flag &= 4294967294U;
        gmsPlayerWork.player_flag |= 4194304U;
        if (((int)AppMain.g_gm_main_system.game_flag & 393216) != 0)
            AppMain.GMM_PAD_VIB_MID_TIME(90f);
        work.roll = obsCamera.roll;
        work.roll_spd = 256;
        work.counter = 0U;
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSplStageRolling));
    }

    private static void gmSplStageRolling(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_SPL_STG_WORK work = (AppMain.GMS_SPL_STG_WORK)tcb.work;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        if (AppMain.ObjObjectPauseCheck(0U) != 0U || obsCamera == null)
            return;
        work.roll_spd += 56;
        work.roll += work.roll_spd;
        obsCamera.roll = work.roll;
        ++work.counter;
        if (work.counter == 90U)
            AppMain.IzFadeInitEasy(0U, 3U, 30f);
        if (work.counter <= 90U || !AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        AppMain.IzFadeRestoreDrawSetting();
        AppMain.GmObjSetAllObjectNoDisp();
        AppMain.GmRingGetWork().flag |= 1U;
        AppMain.GmFixSetDisp(false);
        work.flag |= 4U;
        work.counter = 1U;
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSplStageGotoEnd));
        obsCamera.roll = 0;
        AppMain.g_gm_main_system.pseudofall_dir = (ushort)0;
    }

    private static void gmSplStageGotoEnd(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_SPL_STG_WORK work = (AppMain.GMS_SPL_STG_WORK)tcb.work;
        if (work.counter != 0U)
        {
            --work.counter;
        }
        else
        {
            AppMain.g_gm_main_system.game_flag |= 4U;
            AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmSplStageEnd));
        }
    }

    private static void gmSplStageEnd(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.UNREFERENCED_PARAMETER((object)tcb);
    }

    private static void gmSplStageLightCtrl(AppMain.GMS_SPL_STG_WORK tcb_work)
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.UNREFERENCED_PARAMETER((object)tcb_work);
        AppMain.NNS_VECTOR nnsVector1 = AppMain.gmSplStageLightRot(-1f, -1f, -1f);
        AppMain.nnNormalizeVector(nnsVector1, nnsVector1);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_0, ref col, 1f, nnsVector1);
        AppMain.NNS_VECTOR nnsVector2 = AppMain.gmSplStageLightRot(-0.4f, -0.4f, -1f);
        AppMain.nnNormalizeVector(nnsVector2, nnsVector2);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_1, ref col, 1f, nnsVector2);
    }

    private static AppMain.NNS_VECTOR gmSplStageLightRot(
      float pos_x,
      float pos_y,
      float pos_z)
    {
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        float num1 = pos_x * AppMain.nnSin((int)-AppMain.g_gm_main_system.pseudofall_dir);
        float num2 = pos_x * AppMain.nnCos((int)-AppMain.g_gm_main_system.pseudofall_dir);
        float num3 = pos_y * AppMain.nnSin((int)-AppMain.g_gm_main_system.pseudofall_dir);
        float num4 = pos_y * AppMain.nnCos((int)-AppMain.g_gm_main_system.pseudofall_dir);
        nnsVector.x = num2 - num3;
        nnsVector.y = num1 + num4;
        nnsVector.z = pos_z;
        return nnsVector;
    }

    private static void gmSplStageNudgeCtrl()
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        if (gmsPlayerWork.nudge_timer != (short)0)
        {
            int dest_x = ((int)gmsPlayerWork.nudge_timer * 8 << 12) / 30;
            if (((int)gmsPlayerWork.nudge_timer & 2) != 0)
                dest_x = -dest_x;
            int dest_y = 0;
            AppMain.GmObjGetRotPosXY(dest_x, dest_y, ref gmsPlayerWork.obj_work.ofst.x, ref gmsPlayerWork.obj_work.ofst.y, AppMain.g_gm_main_system.pseudofall_dir);
            AppMain.GmObjGetRotPosXY(dest_x, dest_y, ref dest_x, ref dest_y, (ushort)-AppMain.g_gm_main_system.pseudofall_dir);
            obsCamera.ofst.x = AppMain.FXM_FX32_TO_FLOAT(dest_x);
            obsCamera.ofst.y = AppMain.FXM_FX32_TO_FLOAT(dest_y);
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

    private static void gmSplStageRingGateChk(AppMain.GMS_SPL_STG_WORK tcb_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        ushort getRing = tcb_work.get_ring;
        for (ushort gate_id = 0; gate_id < (ushort)9; ++gate_id)
        {
            if (((int)tcb_work.get_ring & 1 << (int)gate_id) == 0)
            {
                ushort num = AppMain.GmSplStageRingGateNumGet(gate_id);
                if (num != (ushort)byte.MaxValue)
                {
                    if ((int)num <= (int)(ushort)gmsPlayerWork.ring_num)
                        tcb_work.get_ring |= (ushort)(1U << (int)gate_id);
                }
                else
                    break;
            }
        }
        if ((int)getRing == (int)tcb_work.get_ring)
            return;
        AppMain.GmSoundPlaySE("Special7");
    }

    public static void GmSpStageBranchInit(object arg)
    {
        AppMain.UNREFERENCED_PARAMETER(arg);
        if (AppMain.SyGetEvtInfo().old_evt_id == (short)5)
        {
            if (AppMain.g_gs_main_sys_info.stage_id < (ushort)21 || AppMain.g_gs_main_sys_info.stage_id > (ushort)27)
                AppMain.g_gs_main_sys_info.stage_id = (ushort)21;
        }
        else
            AppMain.g_gs_main_sys_info.stage_id = AppMain.GsMainSysIsStageClear(21) ? (AppMain.GsMainSysIsStageClear(22) ? (AppMain.GsMainSysIsStageClear(23) ? (AppMain.GsMainSysIsStageClear(24) ? (AppMain.GsMainSysIsStageClear(25) ? (AppMain.GsMainSysIsStageClear(26) ? (ushort)27 : (ushort)26) : (ushort)25) : (ushort)24) : (ushort)23) : (ushort)22) : (ushort)21;
        AppMain.g_gs_main_sys_info.char_id[0] = 2;
        AppMain.g_gs_main_sys_info.game_flag |= 128U;
        AppMain.GmMainGSInit();
        AppMain.SyChangeNextEvt();
    }

}