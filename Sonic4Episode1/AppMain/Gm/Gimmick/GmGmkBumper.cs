public partial class AppMain
{
    private static void GmGmkBumperBuild()
    {
        g_gm_gmk_bumper_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(852), GmGameDatGetGimmickData(853), 0U);
    }

    private static void GmGmkBumperFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(852);
        GmGameDBuildRegFlushModel(g_gm_gmk_bumper_obj_3d_list, gimmickData.file_num);
        g_gm_gmk_bumper_obj_3d_list = null;
    }

    private static OBS_OBJECT_WORK GmGmkBumperInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int num = eve_rec.id - 146;
        OBS_OBJECT_WORK objWork = gmGmkBumperLoadObj(eve_rec, pos_x, pos_y, num).ene_com.obj_work;
        gmGmkBumperInit(objWork, num);
        return objWork;
    }

    private static uint gmGmkBumpereGameSystemGetSyncTime()
    {
        return g_gm_main_system.sync_time;
    }

    private static GMS_ENEMY_3D_WORK gmGmkBumperLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        UNREFERENCED_PARAMETER(type);
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_BUMPER_WORK(), "GMK_BUMPER");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkBumperLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkBumperLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index1 = g_gm_gmk_bumper_model_id[type];
        ObjObjectCopyAction3dNNModel(objWork, g_gm_gmk_bumper_obj_3d_list[index1], gmsEnemy3DWork.obj_3d);
        int index2 = g_gm_gmk_bumper_motion_id[type];
        object pData = ObjDataGet(855).pData;
        ObjObjectAction3dNNMaterialMotionLoad(objWork, 0, null, null, index2, pData);
        return gmsEnemy3DWork;
    }

    private static void gmGmkBumperInit(OBS_OBJECT_WORK obj_work, int bumper_type)
    {
        GMS_ENEMY_3D_WORK gimmick_work = (GMS_ENEMY_3D_WORK)obj_work;
        gmGmkBumperSetRect(gimmick_work, bumper_type);
        obj_work.move_flag = 8448U;
        obj_work.dir.z = g_gm_gmk_bumper_angle_z[bumper_type];
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag |= 4194324U;
        obj_work.pos.z = -122880;
        ((GMS_GMK_BUMPER_WORK)gimmick_work).se_handle = GsSoundAllocSeHandle();
        obj_work.ppFunc = null;
        obj_work.ppMove = null;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBumperDrawFunc);
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkBumperDestFunc));
    }

    private static void gmGmkBumperSetRect(GMS_ENEMY_3D_WORK gimmick_work, int bumper_type)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        short cLeft = g_gmk_bumper_rect[bumper_type][0];
        short cRight = g_gmk_bumper_rect[bumper_type][2];
        short cTop = g_gmk_bumper_rect[bumper_type][1];
        short cBottom = g_gmk_bumper_rect[bumper_type][3];
        ObjRectWorkZSet(pRec, cLeft, cTop, -500, cRight, cBottom, 500);
        ObjRectDefSet(pRec, 65534, 0);
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkBumperDefFunc);
    }

    private static void gmGmkBumperDestFunc(MTS_TASK_TCB tcb)
    {
        GMS_GMK_BUMPER_WORK gmsGmkBumperWork = tcb.work is GMS_GMK_BUMPER_WORK ? (GMS_GMK_BUMPER_WORK)tcb.work : (GMS_GMK_BUMPER_WORK)mtTaskGetTcbWork(tcb);
        if (gmsGmkBumperWork.se_handle != null)
        {
            GmSoundStopSE(gmsGmkBumperWork.se_handle);
            GsSoundFreeSeHandle(gmsGmkBumperWork.se_handle);
            gmsGmkBumperWork.se_handle = null;
        }
        GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkBumperDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (obj3d.motion != null)
        {
            float startFrame = amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
            float num = amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - startFrame;
            float syncTime = gmGmkBumpereGameSystemGetSyncTime();
            obj3d.mat_frame = syncTime % num;
        }
        ObjDrawActionSummary(obj_work);
    }

    private static int gmGmkBumperCalcType(int id)
    {
        return id - 146;
    }

    private static void gmGmkBumperDefFunc(
      OBS_RECT_WORK gimmick_rect,
      OBS_RECT_WORK player_rect)
    {
        OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)parentObj1;
        OBS_OBJECT_WORK parentObj2 = player_rect.parent_obj;
        if ((parentObj2.holder == null || !(parentObj2.holder is GMS_ENEMY_COM_WORK)) && ((GMS_PLAYER_WORK)parentObj2).seq_state == 40)
            return;
        int num1 = 32768;
        int num2 = 32768;
        int num3 = -12288;
        int type = gmGmkBumperCalcType(gmsEnemy3DWork.ene_com.eve_rec.id);
        if (parentObj2.obj_type == 1 && !gmGmkBumperCheckHit(parentObj1.pos, parentObj2.pos, type))
            return;
        parentObj2.dir.z = 0;
        VecFx32 vecFx32 = new VecFx32();
        vecFx32.z = 0;
        vecFx32.x = parentObj2.spd.x;
        vecFx32.y = parentObj2.spd.y;
        int num4 = parentObj2.pos.x - parentObj1.pos.x;
        int num5 = parentObj2.pos.y + num3 - parentObj1.pos.y;
        int num6 = MTM_MATH_ABS(vecFx32.x);
        int num7 = MTM_MATH_ABS(vecFx32.y);
        int no_move_time = 0;
        switch (type)
        {
            case 0:
                if (num4 > g_gmk_bumper_rect[0][2] * 4096 + num1)
                {
                    vecFx32.x = 16384;
                    no_move_time = 15;
                    break;
                }
                if (num4 < g_gmk_bumper_rect[0][0] * 4096 - num1)
                {
                    vecFx32.x = -16384;
                    no_move_time = 15;
                    break;
                }
                vecFx32.y = 24576;
                if (num4 < -num1)
                    vecFx32.x -= 12288;
                else if (num4 > num1)
                    vecFx32.x += 12288;
                no_move_time = 5;
                break;
            case 1:
                if (num4 > g_gmk_bumper_rect[1][2] * 4096)
                {
                    vecFx32.x = 16384;
                    no_move_time = 15;
                    break;
                }
                if (num4 < g_gmk_bumper_rect[1][0] * 4096)
                {
                    vecFx32.x = -16384;
                    no_move_time = 15;
                    break;
                }
                vecFx32.y = -24576;
                if (num4 < -num1)
                    vecFx32.x -= 12288;
                else if (num4 > num1)
                    vecFx32.x += 12288;
                no_move_time = 5;
                break;
            case 2:
                if (num5 < g_gmk_bumper_rect[2][1] * 4096)
                {
                    vecFx32.y = -24576;
                    no_move_time = 5;
                    break;
                }
                if (num5 > g_gmk_bumper_rect[2][3] * 4096)
                {
                    vecFx32.y = 24576;
                    no_move_time = 5;
                    break;
                }
                vecFx32.x = 16384;
                if (num5 < -num2)
                    vecFx32.y -= 12288;
                else if (num5 > num2)
                    vecFx32.y += 12288;
                no_move_time = 15;
                break;
            case 3:
                if (num5 < g_gmk_bumper_rect[3][1] * 4096)
                {
                    vecFx32.y = -24576;
                    no_move_time = 5;
                    break;
                }
                if (num5 > g_gmk_bumper_rect[3][3] * 4096)
                {
                    vecFx32.y = 24576;
                    no_move_time = 5;
                    break;
                }
                vecFx32.x = -16384;
                if (num5 < -num2)
                    vecFx32.y -= 12288;
                else if (num5 > num2)
                    vecFx32.y += 12288;
                no_move_time = 15;
                break;
            case 4:
                vecFx32.x = num6;
                vecFx32.y = num7;
                vecFx32.x = 20480;
                vecFx32.y = 20480;
                no_move_time = 5;
                break;
            case 5:
                vecFx32.x = num6;
                vecFx32.y = -num7;
                vecFx32.x = 20480;
                vecFx32.y = -20480;
                no_move_time = 5;
                break;
            case 6:
                vecFx32.x = -num6;
                vecFx32.y = num7;
                vecFx32.x = -20480;
                vecFx32.y = 20480;
                no_move_time = 5;
                break;
            case 7:
                vecFx32.x = -num6;
                vecFx32.y = -num7;
                vecFx32.x = -20480;
                vecFx32.y = -20480;
                no_move_time = 5;
                break;
            case 8:
                if (num4 > g_gmk_bumper_rect[8][2] * 4096)
                {
                    vecFx32.x = 16384;
                    no_move_time = 15;
                    break;
                }
                if (num4 < g_gmk_bumper_rect[8][0] * 4096)
                {
                    vecFx32.x = -16384;
                    no_move_time = 15;
                    break;
                }
                if (num5 < -num2)
                {
                    vecFx32.y = -24576;
                    if (num4 < -num1)
                        vecFx32.x -= 12288;
                    else if (num4 > num1)
                        vecFx32.x += 12288;
                    no_move_time = 5;
                    break;
                }
                vecFx32.y = 24576;
                if (num4 < -num1)
                    vecFx32.x -= 12288;
                else if (num4 > num1)
                    vecFx32.x += 12288;
                no_move_time = 5;
                break;
            case 9:
                if (num5 < g_gmk_bumper_rect[9][1] * 4096)
                {
                    vecFx32.y = -24576;
                    no_move_time = 5;
                    break;
                }
                if (num5 > g_gmk_bumper_rect[9][3] * 4096)
                {
                    vecFx32.y = 24576;
                    no_move_time = 5;
                    break;
                }
                if (num4 < -num1)
                {
                    vecFx32.x = -16384;
                    if (num5 < -num2)
                        vecFx32.y -= 12288;
                    else if (num4 > num1)
                        vecFx32.y += 12288;
                    no_move_time = 15;
                    break;
                }
                vecFx32.x = 16384;
                if (num5 < -num2)
                    vecFx32.y -= 12288;
                else if (num4 > num1)
                    vecFx32.y += 12288;
                no_move_time = 15;
                break;
        }
        vecFx32.x = MTM_MATH_CLIP(vecFx32.x, -16384, 16384);
        vecFx32.y = MTM_MATH_CLIP(vecFx32.y, -24576, 24576);
        if (parentObj2.obj_type == 1)
        {
            bool flag_no_recover_homing = false;
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
                flag_no_recover_homing = true;
            GmPlySeqInitPinballAir((GMS_PLAYER_WORK)parentObj2, vecFx32.x, vecFx32.y, no_move_time, flag_no_recover_homing);
        }
        else if (parentObj2.obj_type == 2)
        {
            if (((GMS_ENEMY_3D_WORK)parentObj2).ene_com.eve_rec.id != 316)
                return;
            parentObj2.spd.x = vecFx32.x;
            parentObj2.spd.y = vecFx32.y;
            parentObj2.spd_add.x = 0;
            parentObj2.spd_add.y = 0;
            if (MTM_MATH_ABS(parentObj2.spd.x) < 256)
                parentObj2.spd.x = 256;
            else if (MTM_MATH_ABS(parentObj2.spd.y) < 256)
                parentObj2.spd.y = 256;
        }
        GMS_GMK_BUMPER_WORK bumper_work = (GMS_GMK_BUMPER_WORK)parentObj1;
        if (!gmGmkBumperCheckHitEffect(bumper_work))
            return;
        if (bumper_work.se_handle != null)
            GmSoundPlaySE("Casino6", bumper_work.se_handle);
        int efct_cmn_idx = g_gmk_bumper_effect_id_flush[type];
        if (efct_cmn_idx != -1)
        {
            GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(parentObj1, efct_cmn_idx);
            int num8 = g_gmk_bumper_effect_offset_flush[type][0] * 4096;
            int num9 = g_gmk_bumper_effect_offset_flush[type][1] * 4096;
            gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj1.pos.x + num8;
            gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj1.pos.y + num9;
            gmsEffect3DesWork.efct_com.obj_work.pos.z = 655360;
            gmsEffect3DesWork.efct_com.obj_work.dir.z = parentObj1.dir.z;
        }
        GMM_PAD_VIB_SMALL();
    }

    private static bool gmGmkBumperCheckHit(
      VecFx32 gimmick_pos,
      VecFx32 target_pos,
      int type)
    {
        switch (type)
        {
            case 0:
                VecFx32 vecFx32_1 = new VecFx32(gimmick_pos);
                vecFx32_1.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][3]);
                VecFx32 line_end1 = new VecFx32();
                line_end1.Assign(gimmick_pos);
                line_end1.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][0]);
                line_end1.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][3] * 0.5f);
                VecFx32 line_start1 = new VecFx32();
                line_start1.Assign(gimmick_pos);
                line_start1.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][2]);
                line_start1.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][3] * 0.5f);
                if (gmGmkBumperCheckLeft(line_start1, vecFx32_1, target_pos) || gmGmkBumperCheckLeft(vecFx32_1, line_end1, target_pos))
                    return false;
                break;
            case 1:
                VecFx32 vecFx32_2 = new VecFx32();
                vecFx32_2.Assign(gimmick_pos);
                vecFx32_2.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][1]);
                VecFx32 line_start2 = new VecFx32();
                line_start2.Assign(gimmick_pos);
                line_start2.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][0]);
                line_start2.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][1] * 0.4f);
                VecFx32 line_end2 = new VecFx32();
                line_end2.Assign(gimmick_pos);
                line_end2.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][2]);
                line_end2.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][1] * 0.4f);
                if (gmGmkBumperCheckLeft(line_start2, vecFx32_2, target_pos) || gmGmkBumperCheckLeft(vecFx32_2, line_end2, target_pos))
                    return false;
                break;
            case 2:
                VecFx32 vecFx32_3 = new VecFx32();
                vecFx32_3.Assign(gimmick_pos);
                vecFx32_3.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][2]);
                VecFx32 line_start3 = new VecFx32();
                line_start3.Assign(gimmick_pos);
                line_start3.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][2] * 0.4f);
                line_start3.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][1]);
                VecFx32 line_end3 = new VecFx32();
                line_end3.Assign(gimmick_pos);
                line_end3.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][2] * 0.4f);
                line_end3.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][3]);
                if (gmGmkBumperCheckLeft(line_start3, vecFx32_3, target_pos) || gmGmkBumperCheckLeft(vecFx32_3, line_end3, target_pos))
                    return false;
                break;
            case 3:
                VecFx32 vecFx32_4 = new VecFx32();
                vecFx32_4.Assign(gimmick_pos);
                vecFx32_4.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][0]);
                VecFx32 line_end4 = new VecFx32();
                line_end4.Assign(gimmick_pos);
                line_end4.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][0] * 0.4f);
                line_end4.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][1]);
                VecFx32 line_start4 = new VecFx32();
                line_start4.Assign(gimmick_pos);
                line_start4.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][0] * 0.4f);
                line_start4.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][3]);
                if (gmGmkBumperCheckLeft(line_start4, vecFx32_4, target_pos) || gmGmkBumperCheckLeft(vecFx32_4, line_end4, target_pos))
                    return false;
                break;
            case 4:
                VecFx32 line_start5 = new VecFx32(gimmick_pos);
                line_start5.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][2]);
                line_start5.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][3] * 0.2f);
                VecFx32 line_end5 = new VecFx32(gimmick_pos);
                line_end5.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][2] * 0.2f);
                line_end5.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][3]);
                if (gmGmkBumperCheckLeft(line_start5, line_end5, target_pos))
                    return false;
                break;
            case 5:
                VecFx32 line_start6 = new VecFx32(gimmick_pos);
                line_start6.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][2] * 0.2f);
                line_start6.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][1]);
                VecFx32 line_end6 = new VecFx32(gimmick_pos);
                line_end6.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][2]);
                line_end6.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][1] * 0.2f);
                if (gmGmkBumperCheckLeft(line_start6, line_end6, target_pos))
                    return false;
                break;
            case 6:
                VecFx32 line_end7 = new VecFx32(gimmick_pos);
                line_end7.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][0]);
                line_end7.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][3] * 0.2f);
                VecFx32 line_start7 = new VecFx32(gimmick_pos);
                line_start7.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][0] * 0.2f);
                line_start7.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][3]);
                if (gmGmkBumperCheckLeft(line_start7, line_end7, target_pos))
                    return false;
                break;
            case 7:
                VecFx32 line_end8 = new VecFx32(gimmick_pos);
                line_end8.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][2] * 0.2f);
                line_end8.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][1]);
                VecFx32 line_start8 = new VecFx32(gimmick_pos);
                line_start8.x += FX_F32_TO_FX32(g_gmk_bumper_rect[type][0]);
                line_start8.y += FX_F32_TO_FX32(g_gmk_bumper_rect[type][1] * 0.2f);
                if (gmGmkBumperCheckLeft(line_start8, line_end8, target_pos))
                    return false;
                break;
        }
        return true;
    }

    private static bool gmGmkBumperCheckLeft(
      VecFx32 line_start,
      VecFx32 line_end,
      VecFx32 point)
    {
        int v1_1 = line_end.x - line_start.x;
        int v1_2 = line_end.y - line_start.y;
        int v2_1 = point.x - line_start.x;
        int v2_2 = point.y - line_start.y;
        return FX_Mul(v1_1, v2_2) - FX_Mul(v1_2, v2_1) <= 0;
    }

    private static bool gmGmkBumperCheckHitEffect(GMS_GMK_BUMPER_WORK bumper_work)
    {
        return bumper_work.se_handle != null && bumper_work.se_handle.au_player != null && (2 != bumper_work.se_handle.au_player.GetStatus() && 1 != bumper_work.se_handle.au_player.GetStatus());
    }

}