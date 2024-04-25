public partial class AppMain
{
    public static void GmRingBuild()
    {
        gm_ring_obj_3d = new OBS_ACTION3D_NN_WORK();
        ObjAction3dNNModelLoad(gm_ring_obj_3d, null, null, 0, readAMBFile((AMS_FS)ObjDataGet(2).pData), null, readAMBFile((AMS_FS)ObjDataGet(3).pData), 0U);
    }

    public static GMS_RING_SYS_WORK GmRingGetWork()
    {
        return gm_ring_sys_work;
    }

    public static int GmRingFlushCheck()
    {
        if (gm_ring_obj_3d == null)
            return 1;
        if (!ObjAction3dNNModelReleaseCheck(gm_ring_obj_3d))
            return 0;
        gm_ring_obj_3d = null;
        return 1;
    }

    public static void GmRingFlush()
    {
        ObjAction3dNNMotionRelease(gm_ring_obj_3d);
        ObjAction3dNNModelRelease(gm_ring_obj_3d);
    }

    public static int GmRingBuildCheck()
    {
        if (!ObjAction3dNNModelLoadCheck(gm_ring_obj_3d))
            return 0;
        if (gm_ring_obj_3d.mat_mtn[0] == null)
            ObjAction3dNNMaterialMotionLoad(gm_ring_obj_3d, 0, null, null, 0, readAMBFile(ObjDataGet(4).pData));
        return 1;
    }

    public static void GmRingExit()
    {
        if (gm_ring_tcb == null)
            return;
        mtTaskClearTcb(gm_ring_tcb);
    }

    public static GMS_RING_WORK GmRingCreate(
      GMS_EVE_RECORD_RING eve_rec,
      int pos_x,
      int pos_y,
      int pos_z)
    {
        if (gm_ring_sys_work == null)
            return null;
        GMS_RING_WORK ring_work = gmRingAllocRingWork();
        if (ring_work == null)
            return null;
        ring_work.pos.x = pos_x;
        ring_work.pos.y = pos_y;
        ring_work.pos.z = pos_z;
        ring_work.spd_x = 0;
        ring_work.spd_y = 0;
        ring_work.scale.x = ring_work.scale.y = ring_work.scale.z = gm_ring_scale;
        ring_work.timer = 0;
        ring_work.flag = 0;
        if (eve_rec != null)
            eve_rec.pos_x = byte.MaxValue;
        ring_work.eve_rec = eve_rec;
        ring_work.duct_obj = null;
        gmRingAttachRingList(ring_work);
        return ring_work;
    }

    public static GMS_RING_WORK gmRingAllocRingWork()
    {
        if (gm_ring_sys_work.ring_list_cnt >= 96)
            return null;
        GMS_RING_WORK ring = gm_ring_sys_work.ring_list[gm_ring_sys_work.ring_list_cnt];
        ++gm_ring_sys_work.ring_list_cnt;
        return ring;
    }

    public static void gmRingAttachRingList(GMS_RING_WORK ring_work)
    {
        if (gm_ring_sys_work.ring_list_end != null)
        {
            gm_ring_sys_work.ring_list_end.post_ring = ring_work;
            ring_work.pre_ring = gm_ring_sys_work.ring_list_end;
            ring_work.post_ring = null;
            gm_ring_sys_work.ring_list_end = ring_work;
        }
        else
        {
            gm_ring_sys_work.ring_list_start = gm_ring_sys_work.ring_list_end = ring_work;
            ring_work.pre_ring = ring_work.post_ring = null;
        }
    }

    public static void GmRingInit()
    {
        MTS_TASK_TCB mtsTaskTcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmRingMain), new GSF_TASK_PROCEDURE(gmRingDest), 0U, 0, 7680U, 5, () => new GMS_RING_SYS_WORK(), "GM RING MAIN");
        if (mtsTaskTcb == null)
            return;
        gm_ring_tcb = mtsTaskTcb;
        gm_ring_sys_work = (GMS_RING_SYS_WORK)mtsTaskTcb.work;
        gm_ring_sys_work.Clear();
        for (int i = 0; i < 8; i++)
        {
            gm_ring_sys_work.h_snd_ring[i] = GsSoundAllocSeHandle();
            gm_ring_sys_work.h_snd_ring[i].flag |= 2147483648U;
        }
        gm_ring_sys_work.player_num = 1;
        for (int index = 0; index < 96; ++index)
        {
            GMS_RING_WORK gmsRingWork = gm_ring_sys_work.ring_list_buf[index];
            gm_ring_sys_work.ring_list[index] = gmsRingWork;
        }
        gm_ring_scale = 4096;
        gm_ring_fall_acc_x = 0;
        gm_ring_fall_acc_y = 288;
        if (21 <= g_gs_main_sys_info.stage_id && g_gs_main_sys_info.stage_id <= 27)
        {
            g_gm_ring_size = (short)((OBD_LCD_X - OBD_LCD_Y) * 1.4 + 20.0);
            gm_ring_die_offset = (short)((OBD_LCD_X - OBD_LCD_Y) * 1.4 + 72.0);
        }
        else
        {
            g_gm_ring_size = 20;
            gm_ring_die_offset = 72;
        }
        gm_ring_sys_work.rec_func = new _rec_func_(gmRingHitFuncNormal);
        gm_ring_sys_work.col_func = new _ring_work_func_delegate_(gmRingMoveCollsion);
        gm_ring_sys_work.ref_spd_base = 8192;
        gm_ring_sys_work.ring_draw_func = new _ring_work_func_delegate_(gmRingDrawFuncRing3D);
        gm_ring_sys_work.se_wait = 0;
        gm_ring_sys_work.color = uint.MaxValue;
        if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
            gm_ring_sys_work.color = 4292927743U;
        else if (g_gs_main_sys_info.stage_id == 14)
            gm_ring_sys_work.color = 3233857791U;
        gm_ring_sys_work.dir = 49152;
        ObjDrawAction3dActionSet3DNNMaterial(gm_ring_obj_3d, 0);
    }

    private static void gmRingMain(MTS_TASK_TCB tcb)
    {
        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        var modernSfx = gs.backup.SSave.CreateInstance().GetRemaster().ModernSoundEffects;
        if (((int)gm_ring_sys_work.flag & 1) != 0 && !(betterSfx || modernSfx))
        {
            gm_ring_sys_work.ring_se_cnt = 0;
        }
        else
        {
            if (g_obj.glb_camera_id >= 0)
                ObjDraw3DNNSetCamera(g_obj.glb_camera_id, g_obj.glb_camera_type);
            if (gm_ring_sys_work.se_wait > 0)
                --gm_ring_sys_work.se_wait;
            gmRingDrawBegin();
            if (ObjObjectPauseCheck(0U) != 0U)
            {
                GMS_RING_WORK gmsRingWork1 = gm_ring_sys_work.ring_list_start;
                while (gmsRingWork1 != null)
                {
                    GMS_RING_WORK gms_ring_work = gmsRingWork1;
                    gmsRingWork1 = gms_ring_work.post_ring;
                    gm_ring_sys_work.ring_draw_func(gms_ring_work);
                }
                GMS_RING_WORK gmsRingWork2 = gm_ring_sys_work.damage_ring_list_start;
                while (gmsRingWork2 != null)
                {
                    GMS_RING_WORK gms_ring_work = gmsRingWork2;
                    gmsRingWork2 = gms_ring_work.post_ring;
                    if (gms_ring_work.timer > 32 || (gms_ring_work.timer & 2) != 0)
                        gm_ring_sys_work.ring_draw_func(gms_ring_work);
                }
                GMS_RING_WORK gmsRingWork3 = gm_ring_sys_work.slot_ring_list_start;
                while (gmsRingWork3 != null)
                {
                    GMS_RING_WORK gms_ring_work = gmsRingWork3;
                    gmsRingWork3 = gms_ring_work.post_ring;
                    gm_ring_sys_work.ring_draw_func(gms_ring_work);
                }
                gmRingDrawEnd();

                if (!(betterSfx || modernSfx))
                    gm_ring_sys_work.ring_se_cnt = 0;
            }
            else
            {
                for (int index = 0; index < gm_ring_sys_work.player_num; ++index)
                {
                    GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[index];
                    OBS_RECT_WORK obsRectWork = gmsPlayerWork.rect_work[2];
                    ply_rect[index].pos.x = gmsPlayerWork.obj_work.pos.x;
                    ply_rect[index].pos.y = gmsPlayerWork.obj_work.pos.y;
                    ply_rect[index].pos.z = gmsPlayerWork.obj_work.pos.z;
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
                        num1 = (short)FX_Mul(num1, gmsPlayerWork.obj_work.scale.x);
                        num2 = (short)FX_Mul(num2, gmsPlayerWork.obj_work.scale.x);
                    }
                    ply_rect[index].left = num1;
                    ply_rect[index].right = num2;
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
                        num3 = (short)FX_Mul(num3, gmsPlayerWork.obj_work.scale.y);
                        num4 = (short)FX_Mul(num4, gmsPlayerWork.obj_work.scale.y);
                    }
                    ply_rect[index].top = num3;
                    ply_rect[index].bottom = num4;
                    ply_rect[index].back = 0;
                    ply_rect[index].front = 0;
                }
                ring_rect.left = -9;
                ring_rect.top = -9;
                ring_rect.right = 9;
                ring_rect.bottom = 9;
                ring_rect.back = -8;
                ring_rect.front = 8;
                if (++gm_ring_sys_work.draw_ring_uv_frame >= 64)
                    gm_ring_sys_work.draw_ring_uv_frame = 0;
                if (((int)g_gm_main_system.ply_work[0].player_flag & 262144) != 0)
                {
                    gm_ring_fall_acc_x = (short)FX_Mul(-288, mtMathSin(g_gm_main_system.ply_work[0].obj_work.dir_fall));
                    gm_ring_fall_acc_y = (short)FX_Mul(288, mtMathCos(g_gm_main_system.ply_work[0].obj_work.dir_fall));
                }
                if (gm_ring_sys_work.wait_slot_ring_num != 0)
                {
                    --gm_ring_sys_work.slot_ring_timer;
                    if (gm_ring_sys_work.slot_ring_timer <= 0)
                    {
                        if (gm_ring_sys_work.wait_slot_ring_num >= 2)
                        {
                            GmRingCreateSlotRing(gm_ring_sys_work.slot_target_obj, 1048576, gm_ring_sys_work.slot_ring_create_dir);
                            GmRingCreateSlotRing(gm_ring_sys_work.slot_target_obj, 1048576, (ushort)(gm_ring_sys_work.slot_ring_create_dir + 32768 - 2048));
                            gm_ring_sys_work.wait_slot_ring_num -= 2;
                        }
                        else
                        {
                            GmRingCreateSlotRing(gm_ring_sys_work.slot_target_obj, 1048576, gm_ring_sys_work.slot_ring_create_dir);
                            --gm_ring_sys_work.wait_slot_ring_num;
                        }
                        gm_ring_sys_work.slot_ring_create_dir -= 2048;
                        gm_ring_sys_work.slot_ring_timer = 4;
                    }
                }
                GMS_RING_WORK gmsRingWork1 = gm_ring_sys_work.ring_list_start;
                while (gmsRingWork1 != null)
                {
                    GMS_RING_WORK gmsRingWork2 = gmsRingWork1;
                    gmsRingWork1 = gmsRingWork2.post_ring;
                    if (ObjViewOutCheck(gmsRingWork2.pos.x, gmsRingWork2.pos.y, gm_ring_die_offset, 0, 0, 0, 0) != 0)
                    {
                        if (gmsRingWork2.eve_rec != null)
                            gmsRingWork2.eve_rec.pos_x = (byte)(gmsRingWork2.pos.x >> 12 & byte.MaxValue);
                        gmRingDetachRingList(gmsRingWork2);
                        gmRingFreeRingWork(gmsRingWork2);
                    }
                    else
                    {
                        gm_ring_sys_work.ring_draw_func(gmsRingWork2);
                        ring_rect.pos.z = 0;
                        int index = 0;
                        int num = 0;
                        for (; index < gm_ring_sys_work.player_num; ++index)
                        {
                            GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[index];
                            if ((ply_work.player_flag & 1024) == 0)
                            {
                                ring_rect.pos.x = gmsRingWork2.pos.x;
                                ring_rect.pos.y = gmsRingWork2.pos.y;
                                if (gm_ring_sys_work.rec_func(ply_rect[index], ring_rect) != 0)
                                {
                                    num = 1;
                                    GmPlayerRingGet(ply_work, 1);
                                    GmComEfctCreateRing(ring_rect.pos.x, ring_rect.pos.y);
                                }
                            }
                        }
                        if (num != 0)
                        {
                            gmsRingWork2.timer = 0;
                            gmRingDetachRingList(gmsRingWork2);
                            gmRingFreeRingWork(gmsRingWork2);
                        }
                    }
                }
                GMS_RING_WORK gmsRingWork3 = gm_ring_sys_work.damage_ring_list_start;
                while (gmsRingWork3 != null)
                {
                    GMS_RING_WORK gmsRingWork2 = gmsRingWork3;
                    gmsRingWork3 = gmsRingWork2.post_ring;
                    if (ObjViewOutCheck(gmsRingWork2.pos.x, gmsRingWork2.pos.y, 196, 0, 0, 0, 0) != 0)
                    {
                        gmRingDetachDamageRingList(gmsRingWork2);
                        gmRingFreeRingWork(gmsRingWork2);
                    }
                    else
                    {
                        gmsRingWork2.pos.x += gmsRingWork2.spd_x;
                        if ((gmsRingWork2.flag & 4) != 0)
                            gmsRingWork2.pos.y -= gmsRingWork2.spd_y;
                        else
                            gmsRingWork2.pos.y += gmsRingWork2.spd_y;
                        gmsRingWork2.spd_x += gm_ring_fall_acc_x;
                        gmsRingWork2.spd_y += gm_ring_fall_acc_y;
                        gm_ring_sys_work.col_func(gmsRingWork2);
                        --gmsRingWork2.timer;
                        if (gmsRingWork2.timer == 0)
                        {
                            gmRingDetachDamageRingList(gmsRingWork2);
                            gmRingFreeRingWork(gmsRingWork2);
                        }
                        else
                        {
                            if (gmsRingWork2.timer <= 216)
                            {
                                ring_rect.pos.z = 0;
                                int index = 0;
                                int num = 0;
                                for (; index < gm_ring_sys_work.player_num; ++index)
                                {
                                    GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[index];
                                    if ((ply_work.player_flag & 1024) == 0)
                                    {
                                        ring_rect.pos.x = gmsRingWork2.pos.x;
                                        ring_rect.pos.y = gmsRingWork2.pos.y;
                                        if (gm_ring_sys_work.rec_func(ply_rect[index], ring_rect) != 0)
                                        {
                                            num = 1;
                                            short ringStageNum = ply_work.ring_stage_num;
                                            GmPlayerRingGet(ply_work, 1);
                                            if (ringStageNum < 999)
                                                --ply_work.ring_stage_num;
                                            GmComEfctCreateRing(ring_rect.pos.x, ring_rect.pos.y);
                                            if (((int)gm_ring_sys_work.flag & 16777216 << index) != 0)
                                                gm_ring_sys_work.flag &= (uint)~(16777216 << index);
                                        }
                                    }
                                }
                                if (num != 0)
                                {
                                    gmsRingWork2.timer = 0;
                                    gmRingDetachDamageRingList(gmsRingWork2);
                                    gmRingFreeRingWork(gmsRingWork2);
                                }
                            }
                            if (gmsRingWork2.timer > 32 || (gmsRingWork2.timer & 2) != 0)
                                gm_ring_sys_work.ring_draw_func(gmsRingWork2);
                        }
                    }
                }
                if (gm_ring_sys_work.damage_ring_list_start == null)
                {
                    for (int index = 0; index < gm_ring_sys_work.player_num; ++index)
                    {
                        if (((int)gm_ring_sys_work.flag & 16777216 << index) != 0)
                        {
                            gm_ring_sys_work.damage_num[index] = 0;
                            gm_ring_sys_work.flag &= (uint)~(16777216 << index);
                        }
                    }
                }
                GMS_RING_WORK gmsRingWork4 = gm_ring_sys_work.slot_ring_list_start;
                while (gmsRingWork4 != null)
                {
                    GMS_RING_WORK gmsRingWork2 = gmsRingWork4;
                    gmsRingWork4 = gmsRingWork2.post_ring;
                    if (ObjViewOutCheck(gmsRingWork2.pos.x, gmsRingWork2.pos.y, 512, 0, 0, 0, 0) != 0)
                    {
                        gmRingDetachSlotRingList(gmsRingWork2);
                        gmRingFreeRingWork(gmsRingWork2);
                    }
                    else
                    {
                        gmsRingWork2.pos.x += gmsRingWork2.spd_x;
                        gmsRingWork2.pos.y += gmsRingWork2.spd_y;
                        gm_ring_sys_work.ring_draw_func(gmsRingWork2);
                        ring_rect.pos.z = 0;
                        int index = 0;
                        int num = 0;
                        for (; index < gm_ring_sys_work.player_num; ++index)
                        {
                            GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[index];
                            if ((ply_work.player_flag & 1024) == 0)
                            {
                                ring_rect.pos.x = gmsRingWork2.pos.x;
                                ring_rect.pos.y = gmsRingWork2.pos.y;
                                if (gm_ring_sys_work.rec_func(ply_rect[index], ring_rect) != 0)
                                {
                                    num = 1;
                                    GmPlayerRingGet(ply_work, 1);
                                    GmComEfctCreateRing(ring_rect.pos.x, ring_rect.pos.y);
                                }
                            }
                        }
                        if (num != 0)
                        {
                            gmsRingWork2.timer = 0;
                            gmRingDetachSlotRingList(gmsRingWork2);
                            gmRingFreeRingWork(gmsRingWork2);
                        }
                    }
                }
                gmRingDrawEnd();
                if (!(betterSfx || modernSfx))
                    gm_ring_sys_work.ring_se_cnt = 0;
            }
        }
    }

    private static void gmRingFreeRingWork(GMS_RING_WORK ring_work)
    {
        --gm_ring_sys_work.ring_list_cnt;
        gm_ring_sys_work.ring_list[gm_ring_sys_work.ring_list_cnt] = ring_work;
    }

    private static ushort gmRingHitFuncNormal(OBS_RECT ply_rect, OBS_RECT ring_rect)
    {
        return ObjRectCheck(ply_rect, ring_rect);
    }

    private static void gmRingDrawBegin()
    {
        gm_ring_sys_work.draw_ring_count = 0;
    }

    private static void gmRingDrawEnd()
    {
        if (gm_ring_sys_work.draw_ring_count <= 0 || !GmMainIsDrawEnable())
            return;
        VecU16 vecU16 = new VecU16();
        vecU16.x = 0;
        vecU16.y = gm_ring_sys_work.dir;
        vecU16.z = GmMainGetObjectRotation();
        GMS_RING_SYS_WORK gmRingSysWork = gm_ring_sys_work;
        uint drawRingCount = gm_ring_sys_work.draw_ring_count;
        SNNS_VECTOR disp_pos;
        ObjCameraDispPosGet(0, out disp_pos);
        float s1;
        float c1;
        nnSinCos(vecU16.z - 8192, out s1, out c1);
        s1 *= 13.57638f;
        float num1 = c1 * 13.57638f;
        float s2;
        float c2;
        nnSinCos(vecU16.z - 24576, out s2, out c2);
        float num2 = s2 * 13.57638f;
        float num3 = c2 * 13.57638f;
        VecFx32 vecFx32 = new VecFx32(gmRingSysWork.draw_ring_pos[0]);
        SNNS_MATRIX snnsMatrix = new SNNS_MATRIX();
        nnMakeUnitMatrix(ref snnsMatrix);
        nnTranslateMatrix(ref snnsMatrix, ref snnsMatrix, FX_FX32_TO_F32(vecFx32.x), -FX_FX32_TO_F32(vecFx32.y), FX_FX32_TO_F32(vecFx32.z));
        _AMS_PARAM_DRAW_PRIMITIVE.type = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.count = 6 * (int)drawRingCount - 2;
        _AMS_PARAM_DRAW_PRIMITIVE.ablend = 0;
        _AMS_PARAM_DRAW_PRIMITIVE.bldSrc = 768;
        _AMS_PARAM_DRAW_PRIMITIVE.bldDst = 774;
        _AMS_PARAM_DRAW_PRIMITIVE.bldMode = 32774;
        _AMS_PARAM_DRAW_PRIMITIVE.aTest = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.zMask = 0;
        _AMS_PARAM_DRAW_PRIMITIVE.zTest = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.noSort = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.texlist = gm_ring_obj_3d.texlist;
        _AMS_PARAM_DRAW_PRIMITIVE.texId = 0;
        _AMS_PARAM_DRAW_PRIMITIVE.uwrap = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.vwrap = 1;
        uint color = gm_ring_sys_work.color;
        _AMS_PARAM_DRAW_PRIMITIVE.vtxPCT3D = amDrawAlloc_NNS_PRIM3D_PCT(_AMS_PARAM_DRAW_PRIMITIVE.count);
        NNS_PRIM3D_PCT[] buffer = _AMS_PARAM_DRAW_PRIMITIVE.vtxPCT3D.buffer;
        int offset = _AMS_PARAM_DRAW_PRIMITIVE.vtxPCT3D.offset;
        _AMS_PARAM_DRAW_PRIMITIVE.format3D = 4;
        for (int index1 = 0; index1 < drawRingCount; ++index1)
        {
            int index2 = offset + 6 * index1;
            ushort num4 = (ushort)(gm_ring_sys_work.draw_ring_uv_frame / 4U);
            buffer[index2].Tex.u = buffer[index2 + 1].Tex.u = gm_ring_roll_uv[0][num4];
            buffer[index2 + 2].Tex.u = buffer[index2 + 3].Tex.u = gm_ring_roll_uv[0][num4] + 0.25f;
            buffer[index2].Tex.v = buffer[index2 + 2].Tex.v = gm_ring_roll_uv[1][num4];
            buffer[index2 + 1].Tex.v = buffer[index2 + 3].Tex.v = gm_ring_roll_uv[1][num4] + 0.25f;
            buffer[index2].Col = color;
            buffer[index2 + 1].Col = buffer[index2 + 2].Col = buffer[index2 + 3].Col = buffer[index2].Col;
            buffer[index2].Pos.x = s1 + FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].x - vecFx32.x);
            buffer[index2 + 1].Pos.x = num2 + FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].x - vecFx32.x);
            buffer[index2 + 2].Pos.x = -num2 + FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].x - vecFx32.x);
            buffer[index2 + 3].Pos.x = -s1 + FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].x - vecFx32.x);
            buffer[index2].Pos.y = num1 - FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].y - vecFx32.y);
            buffer[index2 + 1].Pos.y = num3 - FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].y - vecFx32.y);
            buffer[index2 + 2].Pos.y = -num3 - FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].y - vecFx32.y);
            buffer[index2 + 3].Pos.y = -num1 - FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].y - vecFx32.y);
            buffer[index2].Pos.z = buffer[index2 + 1].Pos.z = buffer[index2 + 2].Pos.z = buffer[index2 + 3].Pos.z = FX_FX32_TO_F32(gmRingSysWork.draw_ring_pos[index1].z - vecFx32.z) - 1f;
            if (index1 != 0)
                buffer[index2 - 1] = buffer[index2];
            if (index1 + 1 < drawRingCount)
                buffer[index2 + 4] = buffer[index2 + 3];
        }
        _AMS_PARAM_DRAW_PRIMITIVE.sortZ = nnDistanceVector(ref buffer[offset].Pos, ref disp_pos);
        amMatrixPush(ref snnsMatrix);
        ObjDraw3DNNDrawPrimitive(_AMS_PARAM_DRAW_PRIMITIVE);
        amMatrixPop();
    }

    private static void gmRingDest(MTS_TASK_TCB tcb)
    {
        for (GMS_RING_WORK gmsRingWork = gm_ring_sys_work.ring_list_start; gmsRingWork != null; gmsRingWork = gmsRingWork.post_ring)
        {
            if (gmsRingWork.eve_rec != null)
                gmsRingWork.eve_rec.pos_x = (byte)(gmsRingWork.pos.x >> 12 & byte.MaxValue);
        }
        for (int index = 0; index < 2; ++index)
        {
            if (gm_ring_sys_work.h_snd_ring[index] != null)
            {
                GmSoundStopSE(gm_ring_sys_work.h_snd_ring[index]);
                GsSoundFreeSeHandle(gm_ring_sys_work.h_snd_ring[index]);
                gm_ring_sys_work.h_snd_ring[index] = null;
            }
        }
        gm_ring_tcb = null;
        gm_ring_sys_work = null;
    }

    private static void gmRingMoveCollsion(GMS_RING_WORK ring_work)
    {
        int num1 = 0;
        int spdY = ring_work.spd_y;
        OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        if ((ring_work.flag & 4) != 0)
        {
            int num2 = -spdY;
        }
        pData.flag = (ring_work.flag & 2) == 0 ? (ushort)0 : (ushort)1;
        pData.dir = null;
        pData.attr = null;
        pData.pos_x = ring_work.pos.x >> 12;
        pData.pos_y = ring_work.pos.y >> 12;
        if (ring_work.spd_y > 0)
        {
            pData.pos_y += 9;
            pData.vec = 2;
            num1 = ObjDiffCollisionFast(pData);
            if (num1 < 0)
            {
                if ((ring_work.flag & 4) != 0)
                    ring_work.pos.y -= num1 << 12;
                else
                    ring_work.pos.y += num1 << 12;
            }
        }
        else if (ring_work.spd_y < 0)
        {
            pData.pos_y += -9;
            pData.vec = 3;
            num1 = ObjDiffCollisionFast(pData);
            if (num1 < 0)
            {
                if ((ring_work.flag & 4) != 0)
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
            pData.vec = 0;
            num3 = ObjDiffCollisionFast(pData);
            if (num3 < 0)
                ring_work.pos.x += num3 << 12;
        }
        else if (ring_work.spd_x < 0)
        {
            pData.pos_x += -9;
            pData.vec = 1;
            num3 = ObjDiffCollisionFast(pData);
            if (num3 < 0)
                ring_work.pos.x -= num3 << 12;
        }
        if (num3 < 0)
        {
            ring_work.spd_x -= ring_work.spd_x >> 2;
            ring_work.spd_x = -ring_work.spd_x;
        }
        GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
    }

    private static void gmRingDetachSlotRingList(GMS_RING_WORK ring_work)
    {
        if (ring_work.pre_ring == null)
            gm_ring_sys_work.slot_ring_list_start = ring_work.post_ring;
        else
            ring_work.pre_ring.post_ring = ring_work.post_ring;
        if (ring_work.post_ring == null)
            gm_ring_sys_work.slot_ring_list_end = ring_work.pre_ring;
        else
            ring_work.post_ring.pre_ring = ring_work.pre_ring;
    }

    private static void gmRingDetachRingList(GMS_RING_WORK ring_work)
    {
        if (ring_work.pre_ring == null)
            gm_ring_sys_work.ring_list_start = ring_work.post_ring;
        else
            ring_work.pre_ring.post_ring = ring_work.post_ring;
        if (ring_work.post_ring == null)
            gm_ring_sys_work.ring_list_end = ring_work.pre_ring;
        else
            ring_work.post_ring.pre_ring = ring_work.pre_ring;
    }

    private static GMS_RING_WORK GmRingCreateSlotRing(
      OBS_OBJECT_WORK target_obj,
      int dist,
      ushort dir)
    {
        if (gm_ring_sys_work == null)
            return null;
        GMS_RING_WORK ring_work = gmRingAllocRingWork();
        if (ring_work == null)
            return null;
        ring_work.pos.x = target_obj.pos.x + FX_Mul(dist, mtMathCos(dir));
        ring_work.pos.y = target_obj.pos.y + FX_Mul(-dist, mtMathSin(dir));
        ring_work.pos.z = target_obj.pos.z;
        ring_work.spd_x = FX_Mul(24576, mtMathCos((ushort)(dir + 32768U)));
        ring_work.spd_y = FX_Mul(-24576, mtMathSin((ushort)(dir + 32768U)));
        ring_work.scale.x = ring_work.scale.y = ring_work.scale.z = 4096;
        ring_work.timer = 0;
        ring_work.flag = 0;
        ring_work.eve_rec = null;
        ring_work.duct_obj = null;
        gmRingAttachSlotRingList(ring_work);
        return ring_work;
    }

    private static void gmRingDetachDamageRingList(GMS_RING_WORK ring_work)
    {
        if (ring_work.pre_ring == null)
            gm_ring_sys_work.damage_ring_list_start = ring_work.post_ring;
        else
            ring_work.pre_ring.post_ring = ring_work.post_ring;
        if (ring_work.post_ring == null)
            gm_ring_sys_work.damage_ring_list_end = ring_work.pre_ring;
        else
            ring_work.post_ring.pre_ring = ring_work.pre_ring;
    }

    private static void gmRingAttachSlotRingList(GMS_RING_WORK ring_work)
    {
        if (gm_ring_sys_work.slot_ring_list_end != null)
        {
            gm_ring_sys_work.slot_ring_list_end.post_ring = ring_work;
            ring_work.pre_ring = gm_ring_sys_work.slot_ring_list_end;
            ring_work.post_ring = null;
            gm_ring_sys_work.slot_ring_list_end = ring_work;
        }
        else
        {
            gm_ring_sys_work.slot_ring_list_start = gm_ring_sys_work.slot_ring_list_end = ring_work;
            ring_work.pre_ring = ring_work.post_ring = null;
        }
    }

    private static void gmRingDrawFuncRing3D(GMS_RING_WORK ring_work)
    {
        VecU16 vecU16 = new VecU16();
        vecU16.x = 0;
        vecU16.y = gm_ring_sys_work.dir;
        OBS_CAMERA obsCamera = ObjCameraGet(g_obj.glb_camera_id);
        vecU16.z = (ushort)-obsCamera.roll;
        int drawRingCount = gm_ring_sys_work.draw_ring_count;
        gm_ring_sys_work.draw_ring_pos[drawRingCount].x = ring_work.pos.x;
        gm_ring_sys_work.draw_ring_pos[drawRingCount].y = ring_work.pos.y;
        gm_ring_sys_work.draw_ring_pos[drawRingCount].z = ring_work.pos.z;
        ++gm_ring_sys_work.draw_ring_count;
    }

    private static void GmRingGetSE()
    {
        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        var modernSfx = gs.backup.SSave.CreateInstance().GetRemaster().ModernSoundEffects;

        if (betterSfx || modernSfx)
        {
            var i = gm_ring_sys_work.ring_se_cnt % 4;
            var j = i + 4;

            bool flag = true;
            // makes sure we can only play 1 ring sound per frame
            //if (gm_ring_sys_work.se_wait > 0)
            //    return;

            if (gm_ring_sys_work != null)
            {
                if (((int)gm_ring_sys_work.flag & 8) != 0)
                    flag = false;
                gm_ring_sys_work.flag ^= 8U;
                //gm_ring_sys_work.se_wait = 1;
            }

            gm_ring_sys_work.h_snd_ring[i].snd_ctrl_param.pan = flag ? -1.0f : 1.0f;
            gm_ring_sys_work.h_snd_ring[i].flag |= 1U;

            GmSoundPlaySE("Ring1", gm_ring_sys_work.h_snd_ring[i]);
            if (modernSfx)
                GmSoundPlaySE("RingSparkle" + random.Next(1, 4), gm_ring_sys_work.h_snd_ring[j]);
        }
        else
        {
            bool flag = true;
            if (gm_ring_sys_work.ring_se_cnt >= 1 || gm_ring_sys_work.se_wait > 0)
                return;
            if (gm_ring_sys_work != null)
            {
                if (((int)gm_ring_sys_work.flag & 8) != 0)
                    flag = false;
                gm_ring_sys_work.flag ^= 8U;
                gm_ring_sys_work.se_wait = 3;
            }
            if (flag)
                GmSoundPlaySE("Ring1L", gm_ring_sys_work.h_snd_ring[0]);
            else
                GmSoundPlaySE("Ring1R", gm_ring_sys_work.h_snd_ring[1]);
        }

        ++gm_ring_sys_work.ring_se_cnt;
    }

    private static void GmRingDamageSet(GMS_PLAYER_WORK ply_obj)
    {
        GmRingDamageSetNum(ply_obj, ply_obj.ring_num);
    }

    private static void GmRingDamageSetNum(GMS_PLAYER_WORK ply_work, short ring_num)
    {
        int spd_x = 0;
        int spd_y = 0;
        int num1 = 1160;
        byte playerId = ply_work.player_id;
        ushort flag = (ushort)(8 | playerId << 4 & 16);
        if (gm_ring_sys_work == null)
            return;
        if (ring_num > ply_work.ring_num)
            ring_num = ply_work.ring_num;
        else if (ring_num < 0)
        {
            mppAssertNotImpl();
            return;
        }
        ply_work.ring_num -= ring_num;
        if (ring_num > 32)
            ring_num = 32;
        gm_ring_sys_work.flag |= 16777216U << playerId;
        if (((int)ply_work.obj_work.flag & 1) != 0)
            flag |= 2;
        int num2 = num1 + (gm_ring_sys_work.damage_num[playerId] << 8);
        ushort dirFall = ply_work.obj_work.dir_fall;
        for (int index = 0; index < ring_num; ++index)
        {
            if (num2 >= 0)
            {
                int num3 = num2 >> 8;
                int num4 = num3 >= 6 ? -num3 + 9 : num3;
                int num5 = mtMathSin((ushort)((num2 + dirFall & byte.MaxValue) << 8)) << 4 >> num4;
                int num6 = mtMathCos((ushort)((num2 + dirFall & byte.MaxValue) << 8)) << 4 >> num4;
                spd_x = num5 - (num5 >> 2);
                spd_y = num6 - (num6 >> 2);
                num2 = num2 + 16 | 128;
            }
            if (GmRingCreateDamageRing(ply_work.obj_work.pos.x, ply_work.obj_work.pos.y, 0, spd_x, spd_y, flag) != null)
            {
                num2 = -num2;
                spd_x = -spd_x;
            }
            else
                break;
        }
        if (gm_ring_sys_work.damage_num[playerId] >= gm_ring_damege_num_tbl[g_gs_main_sys_info.level])
            return;
        ++gm_ring_sys_work.damage_num[playerId];
    }

    private static GMS_RING_WORK GmRingCreateDamageRing(
      int pos_x,
      int pos_y,
      int pos_z,
      int spd_x,
      int spd_y,
      ushort flag)
    {
        if (gm_ring_sys_work == null)
            return null;
        GMS_RING_WORK ring_work = gmRingAllocRingWork();
        if (ring_work == null)
            return null;
        ring_work.pos.x = pos_x;
        ring_work.pos.y = pos_y;
        ring_work.pos.z = pos_z;
        ring_work.spd_x = spd_x;
        ring_work.spd_y = spd_y;
        ring_work.scale.x = ring_work.scale.y = ring_work.scale.z = 4096;
        ring_work.timer = (short)(256 + (mtMathRand() & 31));
        ring_work.flag = flag;
        ring_work.eve_rec = null;
        ring_work.duct_obj = null;
        gmRingAttachDamageRingList(ring_work);
        return ring_work;
    }

    private static void gmRingAttachDamageRingList(GMS_RING_WORK ring_work)
    {
        if (gm_ring_sys_work.damage_ring_list_end != null)
        {
            gm_ring_sys_work.damage_ring_list_end.post_ring = ring_work;
            ring_work.pre_ring = gm_ring_sys_work.damage_ring_list_end;
            ring_work.post_ring = null;
            gm_ring_sys_work.damage_ring_list_end = ring_work;
        }
        else
        {
            gm_ring_sys_work.damage_ring_list_start = gm_ring_sys_work.damage_ring_list_end = ring_work;
            ring_work.pre_ring = ring_work.post_ring = null;
        }
    }

    private static void GmRingSlotSetNum(GMS_PLAYER_WORK ply_work, int ring_num)
    {
        if (gm_ring_sys_work == null || ring_num <= 0)
            return;
        gm_ring_sys_work.wait_slot_ring_num = ring_num;
        gm_ring_sys_work.slot_ring_create_dir = 0;
        gm_ring_sys_work.slot_target_obj = (OBS_OBJECT_WORK)ply_work;
    }

    private static int GmRingCheckRestSlotRing()
    {
        return gm_ring_sys_work == null || gm_ring_sys_work.wait_slot_ring_num == 0 && gm_ring_sys_work.slot_ring_list_start == null ? 0 : 1;
    }

}