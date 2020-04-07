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
    public static void GmRingBuild()
    {
        AppMain.gm_ring_obj_3d = new AppMain.OBS_ACTION3D_NN_WORK();
        AppMain.ObjAction3dNNModelLoad(AppMain.gm_ring_obj_3d, (AppMain.OBS_DATA_WORK)null, (string)null, 0, AppMain.readAMBFile((AppMain.AMS_FS)AppMain.ObjDataGet(2).pData), (string)null, AppMain.readAMBFile((AppMain.AMS_FS)AppMain.ObjDataGet(3).pData), 0U);
    }

    public static AppMain.GMS_RING_SYS_WORK GmRingGetWork()
    {
        return AppMain.gm_ring_sys_work;
    }

    public static int GmRingFlushCheck()
    {
        if (AppMain.gm_ring_obj_3d == null)
            return 1;
        if (!AppMain.ObjAction3dNNModelReleaseCheck(AppMain.gm_ring_obj_3d))
            return 0;
        AppMain.gm_ring_obj_3d = (AppMain.OBS_ACTION3D_NN_WORK)null;
        return 1;
    }

    public static void GmRingFlush()
    {
        AppMain.ObjAction3dNNMotionRelease(AppMain.gm_ring_obj_3d);
        AppMain.ObjAction3dNNModelRelease(AppMain.gm_ring_obj_3d);
    }

    public static int GmRingBuildCheck()
    {
        if (!AppMain.ObjAction3dNNModelLoadCheck(AppMain.gm_ring_obj_3d))
            return 0;
        if (AppMain.gm_ring_obj_3d.mat_mtn[0] == null)
            AppMain.ObjAction3dNNMaterialMotionLoad(AppMain.gm_ring_obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, AppMain.readAMBFile(AppMain.ObjDataGet(4).pData));
        return 1;
    }

    public static void GmRingExit()
    {
        if (AppMain.gm_ring_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_ring_tcb);
    }

    public static AppMain.GMS_RING_WORK GmRingCreate(
      AppMain.GMS_EVE_RECORD_RING eve_rec,
      int pos_x,
      int pos_y,
      int pos_z)
    {
        if (AppMain.gm_ring_sys_work == null)
            return (AppMain.GMS_RING_WORK)null;
        AppMain.GMS_RING_WORK ring_work = AppMain.gmRingAllocRingWork();
        if (ring_work == null)
            return (AppMain.GMS_RING_WORK)null;
        ring_work.pos.x = pos_x;
        ring_work.pos.y = pos_y;
        ring_work.pos.z = pos_z;
        ring_work.spd_x = 0;
        ring_work.spd_y = 0;
        ring_work.scale.x = ring_work.scale.y = ring_work.scale.z = AppMain.gm_ring_scale;
        ring_work.timer = (short)0;
        ring_work.flag = (ushort)0;
        if (eve_rec != null)
            eve_rec.pos_x = byte.MaxValue;
        ring_work.eve_rec = eve_rec;
        ring_work.duct_obj = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.gmRingAttachRingList(ring_work);
        return ring_work;
    }

    public static AppMain.GMS_RING_WORK gmRingAllocRingWork()
    {
        if (AppMain.gm_ring_sys_work.ring_list_cnt >= 96)
            return (AppMain.GMS_RING_WORK)null;
        AppMain.GMS_RING_WORK ring = AppMain.gm_ring_sys_work.ring_list[AppMain.gm_ring_sys_work.ring_list_cnt];
        ++AppMain.gm_ring_sys_work.ring_list_cnt;
        return ring;
    }

    public static void gmRingAttachRingList(AppMain.GMS_RING_WORK ring_work)
    {
        if (AppMain.gm_ring_sys_work.ring_list_end != null)
        {
            AppMain.gm_ring_sys_work.ring_list_end.post_ring = ring_work;
            ring_work.pre_ring = AppMain.gm_ring_sys_work.ring_list_end;
            ring_work.post_ring = (AppMain.GMS_RING_WORK)null;
            AppMain.gm_ring_sys_work.ring_list_end = ring_work;
        }
        else
        {
            AppMain.gm_ring_sys_work.ring_list_start = AppMain.gm_ring_sys_work.ring_list_end = ring_work;
            ring_work.pre_ring = ring_work.post_ring = (AppMain.GMS_RING_WORK)null;
        }
    }

    public static void GmRingInit()
    {
        AppMain.MTS_TASK_TCB mtsTaskTcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmRingMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmRingDest), 0U, (ushort)0, 7680U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_RING_SYS_WORK()), "GM RING MAIN");
        if (mtsTaskTcb == null)
            return;
        AppMain.gm_ring_tcb = mtsTaskTcb;
        AppMain.gm_ring_sys_work = (AppMain.GMS_RING_SYS_WORK)mtsTaskTcb.work;
        AppMain.gm_ring_sys_work.Clear();
        for (int i = 0; i < 8; i++)
        {
            AppMain.gm_ring_sys_work.h_snd_ring[i] = AppMain.GsSoundAllocSeHandle();
            AppMain.gm_ring_sys_work.h_snd_ring[i].flag |= 2147483648U;
        }
        AppMain.gm_ring_sys_work.player_num = (byte)1;
        for (int index = 0; index < 96; ++index)
        {
            AppMain.GMS_RING_WORK gmsRingWork = AppMain.gm_ring_sys_work.ring_list_buf[index];
            AppMain.gm_ring_sys_work.ring_list[index] = gmsRingWork;
        }
        AppMain.gm_ring_scale = 4096;
        AppMain.gm_ring_fall_acc_x = (short)0;
        AppMain.gm_ring_fall_acc_y = (short)288;
        if ((ushort)21 <= AppMain.g_gs_main_sys_info.stage_id && AppMain.g_gs_main_sys_info.stage_id <= (ushort)27)
        {
            AppMain.g_gm_ring_size = (short)((double)((int)AppMain.OBD_LCD_X - (int)AppMain.OBD_LCD_Y) * 1.4 + 20.0);
            AppMain.gm_ring_die_offset = (short)((double)((int)AppMain.OBD_LCD_X - (int)AppMain.OBD_LCD_Y) * 1.4 + 72.0);
        }
        else
        {
            AppMain.g_gm_ring_size = (short)20;
            AppMain.gm_ring_die_offset = (short)72;
        }
        AppMain.gm_ring_sys_work.rec_func = new AppMain._rec_func_(AppMain.gmRingHitFuncNormal);
        AppMain.gm_ring_sys_work.col_func = new AppMain._ring_work_func_delegate_(AppMain.gmRingMoveCollsion);
        AppMain.gm_ring_sys_work.ref_spd_base = (short)8192;
        AppMain.gm_ring_sys_work.ring_draw_func = new AppMain._ring_work_func_delegate_(AppMain.gmRingDrawFuncRing3D);
        AppMain.gm_ring_sys_work.se_wait = 0;
        AppMain.gm_ring_sys_work.color = uint.MaxValue;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
            AppMain.gm_ring_sys_work.color = 4292927743U;
        else if (AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
            AppMain.gm_ring_sys_work.color = 3233857791U;
        AppMain.gm_ring_sys_work.dir = (ushort)49152;
        AppMain.ObjDrawAction3dActionSet3DNNMaterial(AppMain.gm_ring_obj_3d, 0);
    }

    private static void gmRingMain(AppMain.MTS_TASK_TCB tcb)
    {
        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        var modernSfx = gs.backup.SSave.CreateInstance().GetRemaster().ModernSoundEffects;
        if (((int)AppMain.gm_ring_sys_work.flag & 1) != 0 && !(betterSfx || modernSfx))
        {
            AppMain.gm_ring_sys_work.ring_se_cnt = 0;
        }
        else
        {
            if (AppMain.g_obj.glb_camera_id >= 0)
                AppMain.ObjDraw3DNNSetCamera(AppMain.g_obj.glb_camera_id, AppMain.g_obj.glb_camera_type);
            if (AppMain.gm_ring_sys_work.se_wait > 0)
                --AppMain.gm_ring_sys_work.se_wait;
            AppMain.gmRingDrawBegin();
            if (AppMain.ObjObjectPauseCheck(0U) != 0U)
            {
                AppMain.GMS_RING_WORK gmsRingWork1 = AppMain.gm_ring_sys_work.ring_list_start;
                while (gmsRingWork1 != null)
                {
                    AppMain.GMS_RING_WORK gms_ring_work = gmsRingWork1;
                    gmsRingWork1 = gms_ring_work.post_ring;
                    AppMain.gm_ring_sys_work.ring_draw_func(gms_ring_work);
                }
                AppMain.GMS_RING_WORK gmsRingWork2 = AppMain.gm_ring_sys_work.damage_ring_list_start;
                while (gmsRingWork2 != null)
                {
                    AppMain.GMS_RING_WORK gms_ring_work = gmsRingWork2;
                    gmsRingWork2 = gms_ring_work.post_ring;
                    if (gms_ring_work.timer > (short)32 || ((int)gms_ring_work.timer & 2) != 0)
                        AppMain.gm_ring_sys_work.ring_draw_func(gms_ring_work);
                }
                AppMain.GMS_RING_WORK gmsRingWork3 = AppMain.gm_ring_sys_work.slot_ring_list_start;
                while (gmsRingWork3 != null)
                {
                    AppMain.GMS_RING_WORK gms_ring_work = gmsRingWork3;
                    gmsRingWork3 = gms_ring_work.post_ring;
                    AppMain.gm_ring_sys_work.ring_draw_func(gms_ring_work);
                }
                AppMain.gmRingDrawEnd();

                if (!(betterSfx || modernSfx))
                    AppMain.gm_ring_sys_work.ring_se_cnt = 0;
            }
            else
            {
                for (int index = 0; index < (int)AppMain.gm_ring_sys_work.player_num; ++index)
                {
                    AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[index];
                    AppMain.OBS_RECT_WORK obsRectWork = gmsPlayerWork.rect_work[2];
                    AppMain.ply_rect[index].pos.x = gmsPlayerWork.obj_work.pos.x;
                    AppMain.ply_rect[index].pos.y = gmsPlayerWork.obj_work.pos.y;
                    AppMain.ply_rect[index].pos.z = gmsPlayerWork.obj_work.pos.z;
                    short num1;
                    short num2;
                    if (((int)gmsPlayerWork.obj_work.disp_flag & 1 ^ (int)obsRectWork.flag & 1) != 0)
                    {
                        num1 = (short)-obsRectWork.rect.right;
                        num2 = (short)-obsRectWork.rect.left;
                    }
                    else
                    {
                        num1 = obsRectWork.rect.left;
                        num2 = obsRectWork.rect.right;
                    }
                    if (gmsPlayerWork.obj_work.scale.x != 4096)
                    {
                        num1 = (short)AppMain.FX_Mul((int)num1, gmsPlayerWork.obj_work.scale.x);
                        num2 = (short)AppMain.FX_Mul((int)num2, gmsPlayerWork.obj_work.scale.x);
                    }
                    AppMain.ply_rect[index].left = num1;
                    AppMain.ply_rect[index].right = num2;
                    short num3;
                    short num4;
                    if (((int)gmsPlayerWork.obj_work.disp_flag & 2 ^ (int)obsRectWork.flag & 2) != 0)
                    {
                        num3 = (short)-obsRectWork.rect.bottom;
                        num4 = (short)-obsRectWork.rect.top;
                    }
                    else
                    {
                        num3 = obsRectWork.rect.top;
                        num4 = obsRectWork.rect.bottom;
                    }
                    if (gmsPlayerWork.obj_work.scale.y != 4096)
                    {
                        num3 = (short)AppMain.FX_Mul((int)num3, gmsPlayerWork.obj_work.scale.y);
                        num4 = (short)AppMain.FX_Mul((int)num4, gmsPlayerWork.obj_work.scale.y);
                    }
                    AppMain.ply_rect[index].top = num3;
                    AppMain.ply_rect[index].bottom = num4;
                    AppMain.ply_rect[index].back = (short)0;
                    AppMain.ply_rect[index].front = (short)0;
                }
                AppMain.ring_rect.left = (short)-9;
                AppMain.ring_rect.top = (short)-9;
                AppMain.ring_rect.right = (short)9;
                AppMain.ring_rect.bottom = (short)9;
                AppMain.ring_rect.back = (short)-8;
                AppMain.ring_rect.front = (short)8;
                if (++AppMain.gm_ring_sys_work.draw_ring_uv_frame >= (ushort)64)
                    AppMain.gm_ring_sys_work.draw_ring_uv_frame = (ushort)0;
                if (((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 262144) != 0)
                {
                    AppMain.gm_ring_fall_acc_x = (short)AppMain.FX_Mul(-288, AppMain.mtMathSin((int)AppMain.g_gm_main_system.ply_work[0].obj_work.dir_fall));
                    AppMain.gm_ring_fall_acc_y = (short)AppMain.FX_Mul(288, AppMain.mtMathCos((int)AppMain.g_gm_main_system.ply_work[0].obj_work.dir_fall));
                }
                if (AppMain.gm_ring_sys_work.wait_slot_ring_num != 0)
                {
                    --AppMain.gm_ring_sys_work.slot_ring_timer;
                    if (AppMain.gm_ring_sys_work.slot_ring_timer <= 0)
                    {
                        if (AppMain.gm_ring_sys_work.wait_slot_ring_num >= 2)
                        {
                            AppMain.GmRingCreateSlotRing(AppMain.gm_ring_sys_work.slot_target_obj, 1048576, AppMain.gm_ring_sys_work.slot_ring_create_dir);
                            AppMain.GmRingCreateSlotRing(AppMain.gm_ring_sys_work.slot_target_obj, 1048576, (ushort)((int)AppMain.gm_ring_sys_work.slot_ring_create_dir + 32768 - 2048));
                            AppMain.gm_ring_sys_work.wait_slot_ring_num -= 2;
                        }
                        else
                        {
                            AppMain.GmRingCreateSlotRing(AppMain.gm_ring_sys_work.slot_target_obj, 1048576, AppMain.gm_ring_sys_work.slot_ring_create_dir);
                            --AppMain.gm_ring_sys_work.wait_slot_ring_num;
                        }
                        AppMain.gm_ring_sys_work.slot_ring_create_dir -= (ushort)2048;
                        AppMain.gm_ring_sys_work.slot_ring_timer = 4;
                    }
                }
                AppMain.GMS_RING_WORK gmsRingWork1 = AppMain.gm_ring_sys_work.ring_list_start;
                while (gmsRingWork1 != null)
                {
                    AppMain.GMS_RING_WORK gmsRingWork2 = gmsRingWork1;
                    gmsRingWork1 = gmsRingWork2.post_ring;
                    if (AppMain.ObjViewOutCheck(gmsRingWork2.pos.x, gmsRingWork2.pos.y, AppMain.gm_ring_die_offset, (short)0, (short)0, (short)0, (short)0) != 0)
                    {
                        if (gmsRingWork2.eve_rec != null)
                            gmsRingWork2.eve_rec.pos_x = (byte)(gmsRingWork2.pos.x >> 12 & (int)byte.MaxValue);
                        AppMain.gmRingDetachRingList(gmsRingWork2);
                        AppMain.gmRingFreeRingWork(gmsRingWork2);
                    }
                    else
                    {
                        AppMain.gm_ring_sys_work.ring_draw_func(gmsRingWork2);
                        AppMain.ring_rect.pos.z = 0;
                        int index = 0;
                        int num = 0;
                        for (; index < (int)AppMain.gm_ring_sys_work.player_num; ++index)
                        {
                            AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[index];
                            if (((int)ply_work.player_flag & 1024) == 0)
                            {
                                AppMain.ring_rect.pos.x = gmsRingWork2.pos.x;
                                AppMain.ring_rect.pos.y = gmsRingWork2.pos.y;
                                if (AppMain.gm_ring_sys_work.rec_func(AppMain.ply_rect[index], AppMain.ring_rect) != (ushort)0)
                                {
                                    num = 1;
                                    AppMain.GmPlayerRingGet(ply_work, (short)1);
                                    AppMain.GmComEfctCreateRing(AppMain.ring_rect.pos.x, AppMain.ring_rect.pos.y);
                                }
                            }
                        }
                        if (num != 0)
                        {
                            gmsRingWork2.timer = (short)0;
                            AppMain.gmRingDetachRingList(gmsRingWork2);
                            AppMain.gmRingFreeRingWork(gmsRingWork2);
                        }
                    }
                }
                AppMain.GMS_RING_WORK gmsRingWork3 = AppMain.gm_ring_sys_work.damage_ring_list_start;
                while (gmsRingWork3 != null)
                {
                    AppMain.GMS_RING_WORK gmsRingWork2 = gmsRingWork3;
                    gmsRingWork3 = gmsRingWork2.post_ring;
                    if (AppMain.ObjViewOutCheck(gmsRingWork2.pos.x, gmsRingWork2.pos.y, (short)196, (short)0, (short)0, (short)0, (short)0) != 0)
                    {
                        AppMain.gmRingDetachDamageRingList(gmsRingWork2);
                        AppMain.gmRingFreeRingWork(gmsRingWork2);
                    }
                    else
                    {
                        gmsRingWork2.pos.x += gmsRingWork2.spd_x;
                        if (((int)gmsRingWork2.flag & 4) != 0)
                            gmsRingWork2.pos.y -= gmsRingWork2.spd_y;
                        else
                            gmsRingWork2.pos.y += gmsRingWork2.spd_y;
                        gmsRingWork2.spd_x += (int)AppMain.gm_ring_fall_acc_x;
                        gmsRingWork2.spd_y += (int)AppMain.gm_ring_fall_acc_y;
                        AppMain.gm_ring_sys_work.col_func(gmsRingWork2);
                        --gmsRingWork2.timer;
                        if (gmsRingWork2.timer == (short)0)
                        {
                            AppMain.gmRingDetachDamageRingList(gmsRingWork2);
                            AppMain.gmRingFreeRingWork(gmsRingWork2);
                        }
                        else
                        {
                            if (gmsRingWork2.timer <= (short)216)
                            {
                                AppMain.ring_rect.pos.z = 0;
                                int index = 0;
                                int num = 0;
                                for (; index < (int)AppMain.gm_ring_sys_work.player_num; ++index)
                                {
                                    AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[index];
                                    if (((int)ply_work.player_flag & 1024) == 0)
                                    {
                                        AppMain.ring_rect.pos.x = gmsRingWork2.pos.x;
                                        AppMain.ring_rect.pos.y = gmsRingWork2.pos.y;
                                        if (AppMain.gm_ring_sys_work.rec_func(AppMain.ply_rect[index], AppMain.ring_rect) != (ushort)0)
                                        {
                                            num = 1;
                                            short ringStageNum = ply_work.ring_stage_num;
                                            AppMain.GmPlayerRingGet(ply_work, (short)1);
                                            if (ringStageNum < (short)999)
                                                --ply_work.ring_stage_num;
                                            AppMain.GmComEfctCreateRing(AppMain.ring_rect.pos.x, AppMain.ring_rect.pos.y);
                                            if (((int)AppMain.gm_ring_sys_work.flag & 16777216 << index) != 0)
                                                AppMain.gm_ring_sys_work.flag &= (uint)~(16777216 << index);
                                        }
                                    }
                                }
                                if (num != 0)
                                {
                                    gmsRingWork2.timer = (short)0;
                                    AppMain.gmRingDetachDamageRingList(gmsRingWork2);
                                    AppMain.gmRingFreeRingWork(gmsRingWork2);
                                }
                            }
                            if (gmsRingWork2.timer > (short)32 || ((int)gmsRingWork2.timer & 2) != 0)
                                AppMain.gm_ring_sys_work.ring_draw_func(gmsRingWork2);
                        }
                    }
                }
                if (AppMain.gm_ring_sys_work.damage_ring_list_start == null)
                {
                    for (int index = 0; index < (int)AppMain.gm_ring_sys_work.player_num; ++index)
                    {
                        if (((int)AppMain.gm_ring_sys_work.flag & 16777216 << index) != 0)
                        {
                            AppMain.gm_ring_sys_work.damage_num[index] = (byte)0;
                            AppMain.gm_ring_sys_work.flag &= (uint)~(16777216 << index);
                        }
                    }
                }
                AppMain.GMS_RING_WORK gmsRingWork4 = AppMain.gm_ring_sys_work.slot_ring_list_start;
                while (gmsRingWork4 != null)
                {
                    AppMain.GMS_RING_WORK gmsRingWork2 = gmsRingWork4;
                    gmsRingWork4 = gmsRingWork2.post_ring;
                    if (AppMain.ObjViewOutCheck(gmsRingWork2.pos.x, gmsRingWork2.pos.y, (short)512, (short)0, (short)0, (short)0, (short)0) != 0)
                    {
                        AppMain.gmRingDetachSlotRingList(gmsRingWork2);
                        AppMain.gmRingFreeRingWork(gmsRingWork2);
                    }
                    else
                    {
                        gmsRingWork2.pos.x += gmsRingWork2.spd_x;
                        gmsRingWork2.pos.y += gmsRingWork2.spd_y;
                        AppMain.gm_ring_sys_work.ring_draw_func(gmsRingWork2);
                        AppMain.ring_rect.pos.z = 0;
                        int index = 0;
                        int num = 0;
                        for (; index < (int)AppMain.gm_ring_sys_work.player_num; ++index)
                        {
                            AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[index];
                            if (((int)ply_work.player_flag & 1024) == 0)
                            {
                                AppMain.ring_rect.pos.x = gmsRingWork2.pos.x;
                                AppMain.ring_rect.pos.y = gmsRingWork2.pos.y;
                                if (AppMain.gm_ring_sys_work.rec_func(AppMain.ply_rect[index], AppMain.ring_rect) != (ushort)0)
                                {
                                    num = 1;
                                    AppMain.GmPlayerRingGet(ply_work, (short)1);
                                    AppMain.GmComEfctCreateRing(AppMain.ring_rect.pos.x, AppMain.ring_rect.pos.y);
                                }
                            }
                        }
                        if (num != 0)
                        {
                            gmsRingWork2.timer = (short)0;
                            AppMain.gmRingDetachSlotRingList(gmsRingWork2);
                            AppMain.gmRingFreeRingWork(gmsRingWork2);
                        }
                    }
                }
                AppMain.gmRingDrawEnd();
                if (!(betterSfx || modernSfx))
                    AppMain.gm_ring_sys_work.ring_se_cnt = 0;
            }
        }
    }

    private static void gmRingFreeRingWork(AppMain.GMS_RING_WORK ring_work)
    {
        --AppMain.gm_ring_sys_work.ring_list_cnt;
        AppMain.gm_ring_sys_work.ring_list[AppMain.gm_ring_sys_work.ring_list_cnt] = ring_work;
    }

    private static ushort gmRingHitFuncNormal(AppMain.OBS_RECT ply_rect, AppMain.OBS_RECT ring_rect)
    {
        return AppMain.ObjRectCheck(ply_rect, ring_rect);
    }

    private static void gmRingDrawBegin()
    {
        AppMain.gm_ring_sys_work.draw_ring_count = (ushort)0;
    }

    private static void gmRingDrawEnd()
    {
        if (AppMain.gm_ring_sys_work.draw_ring_count <= (ushort)0 || !AppMain.GmMainIsDrawEnable())
            return;
        AppMain.VecU16 vecU16 = new AppMain.VecU16();
        vecU16.x = (ushort)0;
        vecU16.y = AppMain.gm_ring_sys_work.dir;
        vecU16.z = AppMain.GmMainGetObjectRotation();
        AppMain.GMS_RING_SYS_WORK gmRingSysWork = AppMain.gm_ring_sys_work;
        uint drawRingCount = (uint)AppMain.gm_ring_sys_work.draw_ring_count;
        AppMain.SNNS_VECTOR disp_pos;
        AppMain.ObjCameraDispPosGet(0, out disp_pos);
        float s1;
        float c1;
        AppMain.nnSinCos((int)vecU16.z - 8192, out s1, out c1);
        s1 *= 13.57638f;
        float num1 = c1 * 13.57638f;
        float s2;
        float c2;
        AppMain.nnSinCos((int)vecU16.z - 24576, out s2, out c2);
        float num2 = s2 * 13.57638f;
        float num3 = c2 * 13.57638f;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32(gmRingSysWork.draw_ring_pos[0]);
        AppMain.SNNS_MATRIX snnsMatrix = new AppMain.SNNS_MATRIX();
        AppMain.nnMakeUnitMatrix(ref snnsMatrix);
        AppMain.nnTranslateMatrix(ref snnsMatrix, ref snnsMatrix, AppMain.FX_FX32_TO_F32(vecFx32.x), -AppMain.FX_FX32_TO_F32(vecFx32.y), AppMain.FX_FX32_TO_F32(vecFx32.z));
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.type = 1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.count = 6 * (int)drawRingCount - 2;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.ablend = 0;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.bldSrc = 768;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.bldDst = 774;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.bldMode = 32774;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.aTest = (short)1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.zMask = (short)0;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.zTest = (short)1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.noSort = (short)1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.texlist = AppMain.gm_ring_obj_3d.texlist;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.texId = 0;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.uwrap = 1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.vwrap = 1;
        uint color = AppMain.gm_ring_sys_work.color;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.vtxPCT3D = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(AppMain._AMS_PARAM_DRAW_PRIMITIVE.count);
        AppMain.NNS_PRIM3D_PCT[] buffer = AppMain._AMS_PARAM_DRAW_PRIMITIVE.vtxPCT3D.buffer;
        int offset = AppMain._AMS_PARAM_DRAW_PRIMITIVE.vtxPCT3D.offset;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.format3D = 4;
        for (int index1 = 0; (long)index1 < (long)drawRingCount; ++index1)
        {
            int index2 = offset + 6 * index1;
            ushort num4 = (ushort)((uint)AppMain.gm_ring_sys_work.draw_ring_uv_frame / 4U);
            buffer[index2].Tex.u = buffer[index2 + 1].Tex.u = AppMain.gm_ring_roll_uv[0][(int)num4];
            buffer[index2 + 2].Tex.u = buffer[index2 + 3].Tex.u = AppMain.gm_ring_roll_uv[0][(int)num4] + 0.25f;
            buffer[index2].Tex.v = buffer[index2 + 2].Tex.v = AppMain.gm_ring_roll_uv[1][(int)num4];
            buffer[index2 + 1].Tex.v = buffer[index2 + 3].Tex.v = AppMain.gm_ring_roll_uv[1][(int)num4] + 0.25f;
            buffer[index2].Col = color;
            buffer[index2 + 1].Col = buffer[index2 + 2].Col = buffer[index2 + 3].Col = buffer[index2].Col;
            buffer[index2].Pos.x = s1 + AppMain.FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].x - vecFx32.x);
            buffer[index2 + 1].Pos.x = num2 + AppMain.FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].x - vecFx32.x);
            buffer[index2 + 2].Pos.x = -num2 + AppMain.FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].x - vecFx32.x);
            buffer[index2 + 3].Pos.x = -s1 + AppMain.FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].x - vecFx32.x);
            buffer[index2].Pos.y = num1 - AppMain.FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].y - vecFx32.y);
            buffer[index2 + 1].Pos.y = num3 - AppMain.FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].y - vecFx32.y);
            buffer[index2 + 2].Pos.y = -num3 - AppMain.FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].y - vecFx32.y);
            buffer[index2 + 3].Pos.y = -num1 - AppMain.FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].y - vecFx32.y);
            buffer[index2].Pos.z = buffer[index2 + 1].Pos.z = buffer[index2 + 2].Pos.z = buffer[index2 + 3].Pos.z = AppMain.FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].z - vecFx32.z) - 1f;
            if (index1 != 0)
                buffer[index2 - 1] = buffer[index2];
            if ((long)(index1 + 1) < (long)drawRingCount)
                buffer[index2 + 4] = buffer[index2 + 3];
        }
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.sortZ = AppMain.nnDistanceVector(ref buffer[offset].Pos, ref disp_pos);
        AppMain.amMatrixPush(ref snnsMatrix);
        AppMain.ObjDraw3DNNDrawPrimitive(AppMain._AMS_PARAM_DRAW_PRIMITIVE);
        AppMain.amMatrixPop();
    }

    private static void gmRingDest(AppMain.MTS_TASK_TCB tcb)
    {
        for (AppMain.GMS_RING_WORK gmsRingWork = AppMain.gm_ring_sys_work.ring_list_start; gmsRingWork != null; gmsRingWork = gmsRingWork.post_ring)
        {
            if (gmsRingWork.eve_rec != null)
                gmsRingWork.eve_rec.pos_x = (byte)(gmsRingWork.pos.x >> 12 & (int)byte.MaxValue);
        }
        for (int index = 0; index < 2; ++index)
        {
            if (AppMain.gm_ring_sys_work.h_snd_ring[index] != null)
            {
                AppMain.GmSoundStopSE(AppMain.gm_ring_sys_work.h_snd_ring[index]);
                AppMain.GsSoundFreeSeHandle(AppMain.gm_ring_sys_work.h_snd_ring[index]);
                AppMain.gm_ring_sys_work.h_snd_ring[index] = (AppMain.GSS_SND_SE_HANDLE)null;
            }
        }
        AppMain.gm_ring_tcb = (AppMain.MTS_TASK_TCB)null;
        AppMain.gm_ring_sys_work = (AppMain.GMS_RING_SYS_WORK)null;
    }

    private static void gmRingMoveCollsion(AppMain.GMS_RING_WORK ring_work)
    {
        int num1 = 0;
        int spdY = ring_work.spd_y;
        AppMain.OBS_COL_CHK_DATA pData = AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Alloc();
        if (((int)ring_work.flag & 4) != 0)
        {
            int num2 = -spdY;
        }
        pData.flag = ((int)ring_work.flag & 2) == 0 ? (ushort)0 : (ushort)1;
        pData.dir = (ushort[])null;
        pData.attr = (uint[])null;
        pData.pos_x = ring_work.pos.x >> 12;
        pData.pos_y = ring_work.pos.y >> 12;
        if (ring_work.spd_y > 0)
        {
            pData.pos_y += 9;
            pData.vec = (ushort)2;
            num1 = AppMain.ObjDiffCollisionFast(pData);
            if (num1 < 0)
            {
                if (((int)ring_work.flag & 4) != 0)
                    ring_work.pos.y -= num1 << 12;
                else
                    ring_work.pos.y += num1 << 12;
            }
        }
        else if (ring_work.spd_y < 0)
        {
            pData.pos_y += -9;
            pData.vec = (ushort)3;
            num1 = AppMain.ObjDiffCollisionFast(pData);
            if (num1 < 0)
            {
                if (((int)ring_work.flag & 4) != 0)
                    ring_work.pos.y += num1 << 12;
                else
                    ring_work.pos.y -= num1 << 12;
            }
        }
        if (num1 < 0)
        {
            ring_work.spd_y -= ring_work.spd_y >> 2;
            ring_work.spd_y = -ring_work.spd_y;
        }
        int num3 = 0;
        pData.pos_y = ring_work.pos.y >> 12;
        if (ring_work.spd_x > 0)
        {
            pData.pos_x += 9;
            pData.vec = (ushort)0;
            num3 = AppMain.ObjDiffCollisionFast(pData);
            if (num3 < 0)
                ring_work.pos.x += num3 << 12;
        }
        else if (ring_work.spd_x < 0)
        {
            pData.pos_x += -9;
            pData.vec = (ushort)1;
            num3 = AppMain.ObjDiffCollisionFast(pData);
            if (num3 < 0)
                ring_work.pos.x -= num3 << 12;
        }
        if (num3 < 0)
        {
            ring_work.spd_x -= ring_work.spd_x >> 2;
            ring_work.spd_x = -ring_work.spd_x;
        }
        AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Release(pData);
    }

    private static void gmRingDetachSlotRingList(AppMain.GMS_RING_WORK ring_work)
    {
        if (ring_work.pre_ring == null)
            AppMain.gm_ring_sys_work.slot_ring_list_start = ring_work.post_ring;
        else
            ring_work.pre_ring.post_ring = ring_work.post_ring;
        if (ring_work.post_ring == null)
            AppMain.gm_ring_sys_work.slot_ring_list_end = ring_work.pre_ring;
        else
            ring_work.post_ring.pre_ring = ring_work.pre_ring;
    }

    private static void gmRingDetachRingList(AppMain.GMS_RING_WORK ring_work)
    {
        if (ring_work.pre_ring == null)
            AppMain.gm_ring_sys_work.ring_list_start = ring_work.post_ring;
        else
            ring_work.pre_ring.post_ring = ring_work.post_ring;
        if (ring_work.post_ring == null)
            AppMain.gm_ring_sys_work.ring_list_end = ring_work.pre_ring;
        else
            ring_work.post_ring.pre_ring = ring_work.pre_ring;
    }

    private static AppMain.GMS_RING_WORK GmRingCreateSlotRing(
      AppMain.OBS_OBJECT_WORK target_obj,
      int dist,
      ushort dir)
    {
        if (AppMain.gm_ring_sys_work == null)
            return (AppMain.GMS_RING_WORK)null;
        AppMain.GMS_RING_WORK ring_work = AppMain.gmRingAllocRingWork();
        if (ring_work == null)
            return (AppMain.GMS_RING_WORK)null;
        ring_work.pos.x = target_obj.pos.x + AppMain.FX_Mul(dist, AppMain.mtMathCos((int)dir));
        ring_work.pos.y = target_obj.pos.y + AppMain.FX_Mul(-dist, AppMain.mtMathSin((int)dir));
        ring_work.pos.z = target_obj.pos.z;
        ring_work.spd_x = AppMain.FX_Mul(24576, AppMain.mtMathCos((int)(ushort)((uint)dir + 32768U)));
        ring_work.spd_y = AppMain.FX_Mul(-24576, AppMain.mtMathSin((int)(ushort)((uint)dir + 32768U)));
        ring_work.scale.x = ring_work.scale.y = ring_work.scale.z = 4096;
        ring_work.timer = (short)0;
        ring_work.flag = (ushort)0;
        ring_work.eve_rec = (AppMain.GMS_EVE_RECORD_RING)null;
        ring_work.duct_obj = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.gmRingAttachSlotRingList(ring_work);
        return ring_work;
    }

    private static void gmRingDetachDamageRingList(AppMain.GMS_RING_WORK ring_work)
    {
        if (ring_work.pre_ring == null)
            AppMain.gm_ring_sys_work.damage_ring_list_start = ring_work.post_ring;
        else
            ring_work.pre_ring.post_ring = ring_work.post_ring;
        if (ring_work.post_ring == null)
            AppMain.gm_ring_sys_work.damage_ring_list_end = ring_work.pre_ring;
        else
            ring_work.post_ring.pre_ring = ring_work.pre_ring;
    }

    private static void gmRingAttachSlotRingList(AppMain.GMS_RING_WORK ring_work)
    {
        if (AppMain.gm_ring_sys_work.slot_ring_list_end != null)
        {
            AppMain.gm_ring_sys_work.slot_ring_list_end.post_ring = ring_work;
            ring_work.pre_ring = AppMain.gm_ring_sys_work.slot_ring_list_end;
            ring_work.post_ring = (AppMain.GMS_RING_WORK)null;
            AppMain.gm_ring_sys_work.slot_ring_list_end = ring_work;
        }
        else
        {
            AppMain.gm_ring_sys_work.slot_ring_list_start = AppMain.gm_ring_sys_work.slot_ring_list_end = ring_work;
            ring_work.pre_ring = ring_work.post_ring = (AppMain.GMS_RING_WORK)null;
        }
    }

    private static void gmRingDrawFuncRing3D(AppMain.GMS_RING_WORK ring_work)
    {
        AppMain.VecU16 vecU16 = new AppMain.VecU16();
        vecU16.x = (ushort)0;
        vecU16.y = AppMain.gm_ring_sys_work.dir;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        vecU16.z = (ushort)-obsCamera.roll;
        int drawRingCount = (int)AppMain.gm_ring_sys_work.draw_ring_count;
        AppMain.gm_ring_sys_work.draw_ring_pos[drawRingCount].x = ring_work.pos.x;
        AppMain.gm_ring_sys_work.draw_ring_pos[drawRingCount].y = ring_work.pos.y;
        AppMain.gm_ring_sys_work.draw_ring_pos[drawRingCount].z = ring_work.pos.z;
        ++AppMain.gm_ring_sys_work.draw_ring_count;
    }

    private static void GmRingGetSE()
    {
        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        var modernSfx = gs.backup.SSave.CreateInstance().GetRemaster().ModernSoundEffects;

        if (betterSfx || modernSfx)
        {
            var i = AppMain.gm_ring_sys_work.ring_se_cnt % 4;
            var j = i + 4;

            gm_ring_sys_work.h_snd_ring[i].snd_ctrl_param.volume = 0.3f;
            gm_ring_sys_work.h_snd_ring[j].snd_ctrl_param.volume = 0.3f;

            AppMain.GmSoundPlaySE("Ring1", AppMain.gm_ring_sys_work.h_snd_ring[i]);
            if (modernSfx)
                AppMain.GmSoundPlaySE("RingSparkle" + random.Next(1, 4), AppMain.gm_ring_sys_work.h_snd_ring[j]);
        }
        else
        {
            bool flag = true;
            if (AppMain.gm_ring_sys_work.ring_se_cnt >= 1 || AppMain.gm_ring_sys_work.se_wait > 0)
                return;
            if (AppMain.gm_ring_sys_work != null)
            {
                if (((int)AppMain.gm_ring_sys_work.flag & 8) != 0)
                    flag = false;
                AppMain.gm_ring_sys_work.flag ^= 8U;
                AppMain.gm_ring_sys_work.se_wait = 3;
            }
            if (flag)
                AppMain.GmSoundPlaySE("Ring1L", AppMain.gm_ring_sys_work.h_snd_ring[0]);
            else
                AppMain.GmSoundPlaySE("Ring1R", AppMain.gm_ring_sys_work.h_snd_ring[1]);
        }

        ++AppMain.gm_ring_sys_work.ring_se_cnt;
    }

    private static void GmRingDamageSet(AppMain.GMS_PLAYER_WORK ply_obj)
    {
        AppMain.GmRingDamageSetNum(ply_obj, ply_obj.ring_num);
    }

    private static void GmRingDamageSetNum(AppMain.GMS_PLAYER_WORK ply_work, short ring_num)
    {
        int spd_x = 0;
        int spd_y = 0;
        int num1 = 1160;
        byte playerId = ply_work.player_id;
        ushort flag = (ushort)(8 | (int)playerId << 4 & 16);
        if (AppMain.gm_ring_sys_work == null)
            return;
        if ((int)ring_num > (int)ply_work.ring_num)
            ring_num = ply_work.ring_num;
        else if (ring_num < (short)0)
        {
            AppMain.mppAssertNotImpl();
            return;
        }
        ply_work.ring_num -= ring_num;
        if (ring_num > (short)32)
            ring_num = (short)32;
        AppMain.gm_ring_sys_work.flag |= 16777216U << (int)playerId;
        if (((int)ply_work.obj_work.flag & 1) != 0)
            flag |= (ushort)2;
        int num2 = num1 + ((int)AppMain.gm_ring_sys_work.damage_num[(int)playerId] << 8);
        ushort dirFall = ply_work.obj_work.dir_fall;
        for (int index = 0; index < (int)ring_num; ++index)
        {
            if (num2 >= 0)
            {
                int num3 = num2 >> 8;
                int num4 = num3 >= 6 ? -num3 + 9 : num3;
                int num5 = AppMain.mtMathSin((int)(ushort)((num2 + (int)dirFall & (int)byte.MaxValue) << 8)) << 4 >> num4;
                int num6 = AppMain.mtMathCos((int)(ushort)((num2 + (int)dirFall & (int)byte.MaxValue) << 8)) << 4 >> num4;
                spd_x = num5 - (num5 >> 2);
                spd_y = num6 - (num6 >> 2);
                num2 = num2 + 16 | 128;
            }
            if (AppMain.GmRingCreateDamageRing(ply_work.obj_work.pos.x, ply_work.obj_work.pos.y, 0, spd_x, spd_y, flag) != null)
            {
                num2 = -num2;
                spd_x = -spd_x;
            }
            else
                break;
        }
        if ((int)AppMain.gm_ring_sys_work.damage_num[(int)playerId] >= (int)AppMain.gm_ring_damege_num_tbl[AppMain.g_gs_main_sys_info.level])
            return;
        ++AppMain.gm_ring_sys_work.damage_num[(int)playerId];
    }

    private static AppMain.GMS_RING_WORK GmRingCreateDamageRing(
      int pos_x,
      int pos_y,
      int pos_z,
      int spd_x,
      int spd_y,
      ushort flag)
    {
        if (AppMain.gm_ring_sys_work == null)
            return (AppMain.GMS_RING_WORK)null;
        AppMain.GMS_RING_WORK ring_work = AppMain.gmRingAllocRingWork();
        if (ring_work == null)
            return (AppMain.GMS_RING_WORK)null;
        ring_work.pos.x = pos_x;
        ring_work.pos.y = pos_y;
        ring_work.pos.z = pos_z;
        ring_work.spd_x = spd_x;
        ring_work.spd_y = spd_y;
        ring_work.scale.x = ring_work.scale.y = ring_work.scale.z = 4096;
        ring_work.timer = (short)(256 + ((int)AppMain.mtMathRand() & 31));
        ring_work.flag = flag;
        ring_work.eve_rec = (AppMain.GMS_EVE_RECORD_RING)null;
        ring_work.duct_obj = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.gmRingAttachDamageRingList(ring_work);
        return ring_work;
    }

    private static void gmRingAttachDamageRingList(AppMain.GMS_RING_WORK ring_work)
    {
        if (AppMain.gm_ring_sys_work.damage_ring_list_end != null)
        {
            AppMain.gm_ring_sys_work.damage_ring_list_end.post_ring = ring_work;
            ring_work.pre_ring = AppMain.gm_ring_sys_work.damage_ring_list_end;
            ring_work.post_ring = (AppMain.GMS_RING_WORK)null;
            AppMain.gm_ring_sys_work.damage_ring_list_end = ring_work;
        }
        else
        {
            AppMain.gm_ring_sys_work.damage_ring_list_start = AppMain.gm_ring_sys_work.damage_ring_list_end = ring_work;
            ring_work.pre_ring = ring_work.post_ring = (AppMain.GMS_RING_WORK)null;
        }
    }

    private static void GmRingSlotSetNum(AppMain.GMS_PLAYER_WORK ply_work, int ring_num)
    {
        if (AppMain.gm_ring_sys_work == null || ring_num <= 0)
            return;
        AppMain.gm_ring_sys_work.wait_slot_ring_num = ring_num;
        AppMain.gm_ring_sys_work.slot_ring_create_dir = (ushort)0;
        AppMain.gm_ring_sys_work.slot_target_obj = (AppMain.OBS_OBJECT_WORK)ply_work;
    }

    private static int GmRingCheckRestSlotRing()
    {
        return AppMain.gm_ring_sys_work == null || AppMain.gm_ring_sys_work.wait_slot_ring_num == 0 && AppMain.gm_ring_sys_work.slot_ring_list_start == null ? 0 : 1;
    }

}