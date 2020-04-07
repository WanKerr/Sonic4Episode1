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
    private static void GmEndingTrophySet()
    {
        AppMain.GMS_ENDING_WORK work = AppMain.gmEndingGetWork();
        if (AppMain.g_gm_main_system.ply_work[0] != null)
            AppMain.GsGetMainSysInfo().clear_ring = work.get_ring;
        else
            AppMain.GsGetMainSysInfo().clear_ring = 0U;
    }

    public static void GmEndingExit()
    {
        if (AppMain.gm_ending_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_ending_tcb);
        AppMain.g_gm_main_system.game_flag &= 4286578687U;
        AppMain.gm_ending_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    public static void GmEndingStart()
    {
        AppMain.g_gm_main_system.game_flag |= 8388608U;
        AppMain.GmFixSetDispEx(false, false, false, true, false);
        AppMain.gm_ending_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmEndingCtrl), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 18448U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENDING_WORK()), "ENDING_CTRL");
        AppMain.GMS_ENDING_WORK work = (AppMain.GMS_ENDING_WORK)AppMain.gm_ending_tcb.work;
        work.step = 0;
        work.flag = 1U;
        work.timer = 16U;
        AppMain.GmCameraAllowSet(0.0f, 50f, 0.0f);
        AppMain.g_gm_main_system.map_fcol.bottom -= 32;
    }

    public static void GmEndingBuild()
    {
        AppMain.gm_ending_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(949), AppMain.GmGameDatGetGimmickData(950), 0U);
    }

    public static void GmEndingFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(949);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ending_obj_3d_list, gimmickData.file_num);
    }

    public static AppMain.GMS_ENDING_WORK gmEndingGetWork()
    {
        return (AppMain.GMS_ENDING_WORK)AppMain.gm_ending_tcb.work;
    }

    public static bool GmEndingAnimalForwardChk()
    {
        AppMain.GMS_ENDING_WORK work = AppMain.gmEndingGetWork();
        return work.type != 0 && work.step >= 7;
    }

    public static void GmEndingPlyKeyCustom(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GMS_ENDING_WORK work = AppMain.gmEndingGetWork();
        if (((int)work.flag & 1) != 0)
        {
            ply_work.key_on = (ushort)0;
            ply_work.key_push = (ushort)0;
            ply_work.key_release = (ushort)0;
        }
        else
        {
            ply_work.key_on &= (ushort)160;
            ply_work.key_push &= (ushort)160;
            ply_work.key_release &= (ushort)160;
        }
        ply_work.key_rot_z = ply_work.key_walk_rot_z = 0;
        if (((int)work.flag & 2) != 0)
        {
            ply_work.key_on |= (ushort)4;
            ply_work.key_rot_z = ply_work.key_walk_rot_z = -32767;
        }
        else
        {
            if (((int)work.flag & 4) == 0)
                return;
            ply_work.key_on |= (ushort)8;
            ply_work.key_rot_z = ply_work.key_walk_rot_z = (int)short.MaxValue;
        }
    }

    private static void GmEndingPlyNopSet()
    {
        AppMain.GMS_ENDING_WORK work = AppMain.gmEndingGetWork();
        if (work.step != 1)
            return;
        work.step = 2;
        AppMain.g_gm_main_system.ply_work[0].obj_work.spd_m = -36864;
    }

    private static void GmEndingPlyBrakeSet()
    {
        AppMain.GMS_ENDING_WORK work = AppMain.gmEndingGetWork();
        if (work.step != 2)
            return;
        work.step = 3;
        AppMain.g_gm_main_system.ply_work[0].obj_work.pos.x = 2080768;
        AppMain.g_gm_main_system.ply_work[0].obj_work.spd_m = -36864;
    }

    private static void gmEndingLastPic(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        obj_work.disp_flag |= 32U;
        if (gmsPlayerWork.act_state != 80 && gmsPlayerWork.act_state != 82 && gmsPlayerWork.act_state != 84)
            return;
        obj_work.disp_flag &= 4294967263U;
        gmsPlayerWork.obj_work.disp_flag |= 32U;
    }

    private static void gmEndingLastPicInit()
    {
        AppMain.GMS_ENDING_WORK work1 = AppMain.gmEndingGetWork();
        AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), AppMain.g_gm_main_system.ply_work[0].obj_work, (ushort)0, "END_PIC");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (AppMain.GMS_EFFECT_3DNN_WORK)work2;
        work2.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEndingLastPic);
        AppMain.ObjObjectCopyAction3dNNModel(work2, AppMain.gm_ending_obj_3d_list[work1.type], gmsEffect3DnnWork.obj_3d);
        work2.move_flag |= 16128U;
        work2.disp_flag |= 4194336U;
        work2.flag |= 1026U;
        work2.scale.x = work2.scale.y = work2.scale.z = 5120;
        work2.parent_ofst.y = AppMain.gm_ending_obj_offset[work1.type];
    }

    private static void gmEndingCtrl(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_ENDING_WORK work = (AppMain.GMS_ENDING_WORK)tcb.work;
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        switch (work.step)
        {
            case 0:
                if (work.timer != 0U)
                {
                    --work.timer;
                    break;
                }
                AppMain.g_gm_main_system.game_flag |= 1024U;
                AppMain.IzFadeInitEasy(1U, 2U, 60f);
                work.step = 1;
                work.flag = 2U;
                AppMain.GmSoundPlayStageBGM(0);
                break;
            case 2:
                work.flag |= 1U;
                ply_work.spd_jump_add = ply_work.spd_add;
                break;
            case 3:
                ply_work.obj_work.disp_flag |= 1U;
                if (((int)ply_work.obj_work.move_flag & 1) != 0 && ply_work.obj_work.spd_m > -4096)
                {
                    if (AppMain.amTpIsTouchOn(0))
                    {
                        ply_work.ring_num = (short)AppMain.GmEventMgrGetRingNum();
                        if (AppMain.amTpIsTouchOn(1))
                            AppMain.g_gs_main_sys_info.game_flag |= 32U;
                        else
                            AppMain.g_gs_main_sys_info.game_flag &= 4294967263U;
                    }
                    work.get_ring = (uint)ply_work.ring_num;
                    if (work.get_ring < AppMain.GmEventMgrGetRingNum())
                    {
                        work.step = 4;
                        work.type = 0;
                    }
                    else if (((int)AppMain.g_gs_main_sys_info.game_flag & 32) != 0)
                    {
                        work.step = 5;
                        work.type = 2;
                    }
                    else
                    {
                        work.step = 4;
                        work.type = 1;
                    }
                    work.flag &= 4294967291U;
                    AppMain.gmEndingLastPicInit();
                    break;
                }
                work.flag &= 4294967293U;
                work.flag |= 4U;
                break;
            case 4:
                if (AppMain.g_gm_main_system.game_time <= 720U || ply_work.seq_state == 62)
                    break;
                AppMain.GmPlySeqGmkInitEndingDemo1(ply_work);
                work.step = 6;
                break;
            case 5:
                if (AppMain.g_gm_main_system.game_time <= 720U || ply_work.seq_state == 24)
                    break;
                AppMain.GmPlySeqChangeTransformSuper(ply_work);
                work.step = 6;
                break;
            case 6:
                if (AppMain.g_gm_main_system.game_time <= 900U || ply_work.seq_state == 63)
                    break;
                bool type2 = false;
                if (work.type == 1)
                    type2 = true;
                AppMain.GmPlySeqGmkInitEndingDemo2(ply_work, type2);
                work.step = 7;
                break;
            case 7:
                if (AppMain.g_gm_main_system.game_time <= 1140U)
                    break;
                AppMain.IzFadeInitEasy(0U, 1U, 32f);
                work.step = 8;
                break;
            case 8:
                if (!AppMain.IzFadeIsEnd())
                    break;
                AppMain.GmMainEnd();
                AppMain.SyDecideEvtCase((short)0);
                AppMain.SyChangeNextEvt();
                break;
        }
    }

}