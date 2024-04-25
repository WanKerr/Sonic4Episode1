public partial class AppMain
{
    private static void GmGmkFlipperBuild()
    {
        g_gm_gmk_flipper_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(867), GmGameDatGetGimmickData(868), 0U);
    }

    private static void GmGmkFlipperFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(867);
        GmGameDBuildRegFlushModel(g_gm_gmk_flipper_obj_3d_list, gimmickData.file_num);
        g_gm_gmk_flipper_obj_3d_list = null;
    }

    private static OBS_OBJECT_WORK GmGmkFlipperInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int num = gmGmkFlipperCalcType(eve_rec.id);
        OBS_OBJECT_WORK objWork = gmGmkFlipperLoadObj(eve_rec, pos_x, pos_y, num).ene_com.obj_work;
        gmGmkFlipperInit(objWork, num);
        return objWork;
    }

    private static uint gmGmkFlipperGameSystemGetSyncTime()
    {
        return g_gm_main_system.sync_time;
    }

    private static GMS_ENEMY_3D_WORK gmGmkFlipperLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = type != 2 ? (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_FLIPPER_WORK(), "GMK_FLIPPER_U") : (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_FLIPPER_WORK(), "GMK_FLIPPER_LR");
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        return gmsEnemy3DWork;
    }

    private static GMS_ENEMY_3D_WORK gmGmkFlipperLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkFlipperLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index = g_gm_gmk_flipper_model_id[type];
        ObjObjectCopyAction3dNNModel(objWork, g_gm_gmk_flipper_obj_3d_list[index], gmsEnemy3DWork.obj_3d);
        OBS_DATA_WORK data_work = ObjDataGet(869);
        ObjObjectAction3dNNMaterialMotionLoad(objWork, 0, data_work, null, 0, null);
        if (type == 2)
        {
            gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
            gmsEnemy3DWork.ene_com.col_work.obj_col.width = 16;
            gmsEnemy3DWork.ene_com.col_work.obj_col.height = 8;
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)(-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = -7;
            gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 32U;
        }
        return gmsEnemy3DWork;
    }

    private static void gmGmkFlipperInit(OBS_OBJECT_WORK obj_work, int flipper_type)
    {
        gmGmkFlipperSetRect((GMS_ENEMY_3D_WORK)obj_work, flipper_type);
        obj_work.move_flag = 8448U;
        obj_work.dir.z = g_gm_gmk_flipper_angle_z[flipper_type];
        if (flipper_type == 0)
            obj_work.user_flag = 1U;
        obj_work.disp_flag |= 4194304U;
        ObjDrawObjectActionSet3DNNMaterial(obj_work, g_gm_gmk_flipper_mat_motion_id[flipper_type]);
        obj_work.disp_flag |= 4194308U;
        obj_work.pos.z = -122880;
        obj_work.ppFunc = null;
        obj_work.ppMove = null;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkFlipperDrawFunc);
        gmGmkFlipperChangeModeWait(obj_work);
    }

    private static void gmGmkFlipperSetRect(GMS_ENEMY_3D_WORK gimmick_work, int flipper_type)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        switch (flipper_type)
        {
            case 0:
                pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkFlipperDefFuncU);
                break;
            case 1:
                pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkFlipperDefFuncU);
                break;
            case 2:
                pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkFlipperDefFuncLR);
                break;
        }
        ObjRectWorkZSet(pRec, g_gmk_flipper_rect[flipper_type][0], g_gmk_flipper_rect[flipper_type][1], -500, g_gmk_flipper_rect[flipper_type][2], g_gmk_flipper_rect[flipper_type][3], 500);
        pRec.flag |= 1024U;
        ObjRectDefSet(pRec, 65534, 0);
    }

    private static void gmGmkFlipperDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (obj3d.motion != null)
        {
            float startFrame = amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
            float num = amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - startFrame;
            float syncTime = gmGmkFlipperGameSystemGetSyncTime();
            obj3d.mat_frame = syncTime % num;
        }
        ObjDrawActionSummary(obj_work);
    }

    private static int gmGmkFlipperCalcType(int id)
    {
        return id - 169;
    }

    private static void gmGmkFlipperDefPlayer(
      OBS_RECT_WORK gimmick_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)parentObj1;
        OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)parentObj2;
        if (ply_work.seq_state >= 60)
            return;
        int num1 = gmGmkFlipperCalcType(gmsEnemy3DWork.ene_com.eve_rec.id);
        gmsEnemy3DWork.ene_com.target_obj = parentObj2;
        int num2 = gmGmkFlipperCalcRideOffsetY(parentObj2.pos.x, parentObj1, num1);
        if (parentObj1.pos.y + num2 < parentObj2.pos.y)
        {
            int num3 = parentObj2.pos.x - parentObj1.pos.x;
            if (num1 == 1)
                num3 = -num3;
            if (num3 < 0)
                parentObj2.spd.x = 0;
            bool flag_no_recover_homing = false;
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
                flag_no_recover_homing = true;
            GmPlySeqInitPinballAir(ply_work, parentObj2.spd.x, 8192, 5, flag_no_recover_homing);
        }
        else if (gmGmkFlipperCheckRect(parentObj1.pos, parentObj2.pos, num1) == 0)
        {
            gimmick_rect.flag &= 4294966271U;
        }
        else
        {
            gmGmkFlipperChangeModeReady(parentObj1);
            gimmick_rect.flag |= 1024U;
            gmGmkFlipperSetRideSpeed(parentObj2, parentObj1, num1);
            GmPlySeqInitFlipper((GMS_PLAYER_WORK)parentObj2, parentObj2.spd.x, parentObj2.spd.y, gmsEnemy3DWork.ene_com);
            int num3 = num2;
            int num4 = (ply_work.player_flag & 131072) == 0 ? num3 - 36864 : num3 - 61440;
            parentObj2.pos.y = parentObj1.pos.y + num4;
        }
    }

    private static void gmGmkFlipperDefEnemy(
      OBS_RECT_WORK gimmick_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (((GMS_ENEMY_3D_WORK)parentObj2).ene_com.eve_rec.id != 316 || parentObj1.pos.y < parentObj2.pos.y)
            return;
        parentObj2.spd.y = -parentObj2.spd.y;
        if (MTM_MATH_ABS(parentObj2.spd.x) >= 256)
            return;
        parentObj2.spd.x = 256;
    }

    private static void gmGmkFlipperDefFuncU(
      OBS_RECT_WORK gimmick_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj = target_rect.parent_obj;
        if (parentObj.obj_type == 1)
        {
            gmGmkFlipperDefPlayer(gimmick_rect, target_rect);
        }
        else
        {
            if (parentObj.obj_type != 2)
                return;
            gmGmkFlipperDefEnemy(gimmick_rect, target_rect);
        }
    }

    private static void gmGmkFlipperDefFuncLR(
      OBS_RECT_WORK gimmick_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)parentObj1;
        OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != 1)
            return;
        gmsEnemy3DWork.ene_com.target_obj = parentObj2;
        int v1_1 = 86016;
        int v1_2 = 0;
        if (gmsEnemy3DWork.ene_com.eve_rec.width * 1000U == 0U)
        {
            if (parentObj2.pos.x < parentObj1.pos.x)
            {
                parentObj1.user_flag = 0U;
                v1_1 *= -1;
            }
            else
                parentObj1.user_flag = 1U;
        }
        else
        {
            parentObj1.user_flag = 0U;
            v1_1 *= -1;
        }
        int v2_1 = FX_F32_TO_FX32((float)((100.0 + gmsEnemy3DWork.ene_com.eve_rec.left) * 0.00999999977648258));
        if (v2_1 < 0)
            v2_1 = 0;
        int v2_2 = FX_F32_TO_FX32((float)((100.0 + gmsEnemy3DWork.ene_com.eve_rec.top) * 0.00999999977648258));
        if (v2_2 < 0)
            v2_2 = 0;
        int num1 = FX_Mul(v1_1, v2_1);
        int num2 = FX_Mul(v1_2, v2_2);
        gmGmkFlipperChangeModeHit(parentObj1);
        int no_spddown_timer = 12;
        if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 2) != 0)
            no_spddown_timer = 180;
        GmPlySeqInitPinball((GMS_PLAYER_WORK)parentObj2, num1, num2, no_spddown_timer);
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(parentObj1, 16);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj2.pos.x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj2.pos.y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
        gmsEffect3DesWork.efct_com.obj_work.dir.z = (ushort)(nnArcTan2(FX_FX32_TO_F32(num2), FX_FX32_TO_F32(num1)) - 16384);
    }

    private static void gmGmkFlipperChangeModeWait(OBS_OBJECT_WORK obj_work)
    {
        int index = gmGmkFlipperCalcType(((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        obj_work.user_work = g_gm_gmk_flipper_angle_z[index];
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkFlipperMainWait);
    }

    private static void gmGmkFlipperChangeModeReady(OBS_OBJECT_WORK obj_work)
    {
        int index = gmGmkFlipperCalcType(((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        obj_work.user_work = g_gm_gmk_flipper_angle_z[index];
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkFlipperMainReady);
    }

    private static void gmGmkFlipperChangeModeHit(OBS_OBJECT_WORK obj_work)
    {
        int index = gmGmkFlipperCalcType(((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        ushort num1 = g_gm_gmk_flipper_angle_z[index];
        ushort num2 = obj_work.user_flag == 0U ? (ushort)(num1 + (uint)(ushort)NNM_DEGtoA16(70f)) : (ushort)(num1 + (uint)(ushort)NNM_DEGtoA16(-70f));
        obj_work.user_work = num2;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkFlipperMainHit);
        GmSoundPlaySE("Casino2");
    }

    private static void gmGmkFlipperChangeModeHook(OBS_OBJECT_WORK obj_work)
    {
        int index = gmGmkFlipperCalcType(((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        ushort num1 = g_gm_gmk_flipper_angle_z[index];
        ushort num2 = obj_work.user_flag == 0U ? (ushort)(num1 + (uint)(ushort)NNM_DEGtoA16(70f)) : (ushort)(num1 + (uint)(ushort)NNM_DEGtoA16(-70f));
        obj_work.user_work = num2;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkFlipperMainHook);
    }

    private static void gmGmkFlipperChangeModeOpen(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        int index = gmGmkFlipperCalcType(gmsEnemy3DWork.ene_com.eve_rec.id);
        obj_work.user_work = g_gm_gmk_flipper_angle_z[index];
        obj_work.ppFunc = null;
        obj_work.dir.z = 0;
        gmsEnemy3DWork.ene_com.rect_work[2].ppDef = null;
    }

    private static void gmGmkFlipperMainWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        gmGmkFlipperUpdateAngle(obj_work);
        if (gmGmkFlipperCalcType(gmsEnemy3DWork.ene_com.eve_rec.id) != 2 || gmGmkFlipperCheckScore(obj_work) == 0)
            return;
        gmGmkFlipperChangeModeOpen(obj_work);
    }

    private static void gmGmkFlipperMainReady(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        gmGmkFlipperUpdateAngle(obj_work);
        if (true)
        {
            GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
            if (gmGmkFlipperCheckControlPlayer() == 0)
            {
                gmGmkFlipperChangeModeWait(obj_work);
                return;
            }
            if (gmGmkFlipperCheckKeyHit(obj_work, gmsPlayerWork) == 0)
                return;
            if (gmGmkFlipperCheckHook(obj_work) != 0)
            {
                targetObj.spd.x = 0;
                targetObj.spd.y = 0;
                gmGmkFlipperChangeModeHook(obj_work);
                return;
            }
            int num1 = 12288;
            int v1_1 = -53248;
            if (gmGmkFlipperCalcType(gmsEnemy3DWork.ene_com.eve_rec.id) == 1)
                num1 = -num1;
            int v1_2 = num1 + (targetObj.pos.x - obj_work.pos.x >> 2);
            int num2 = (102400 - MTM_MATH_ABS(targetObj.pos.x - obj_work.pos.x)) / 10;
            if (num2 > 0)
                v1_1 += num2;
            int v2_1 = FX_F32_TO_FX32((float)((100.0 + gmsEnemy3DWork.ene_com.eve_rec.left) * 0.00999999977648258));
            if (v2_1 < 0)
                v2_1 = 0;
            int v2_2 = FX_F32_TO_FX32((float)((100.0 + gmsEnemy3DWork.ene_com.eve_rec.top) * 0.00999999977648258));
            if (v2_2 < 0)
                v2_2 = 0;
            int num3 = FX_Mul(v1_2, v2_1);
            int num4 = FX_Mul(v1_1, v2_2);
            int flag_no_recover_homing = 0;
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
                flag_no_recover_homing = 1;
            int no_spddown_timer = 0;
            if ((gmsEnemy3DWork.ene_com.eve_rec.flag & 2) != 0)
                no_spddown_timer = 30;
            GmPlayerSetAtk(gmsPlayerWork);
            GmPlySeqInitPinballAir(gmsPlayerWork, num3, num4, 5, flag_no_recover_homing, no_spddown_timer);
            GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(obj_work, 16);
            gmsEffect3DesWork.efct_com.obj_work.pos.x = targetObj.pos.x;
            gmsEffect3DesWork.efct_com.obj_work.pos.y = targetObj.pos.y;
            gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
            gmsEffect3DesWork.efct_com.obj_work.dir.z = (ushort)(nnArcTan2(FX_FX32_TO_F32(num4), FX_FX32_TO_F32(num3)) - 16384);
        }
        gmGmkFlipperChangeModeHit(obj_work);
    }

    private static void gmGmkFlipperMainHit(OBS_OBJECT_WORK obj_work)
    {
        if (gmGmkFlipperUpdateAngle(obj_work) == 0)
            return;
        gmGmkFlipperChangeModeWait(obj_work);
    }

    private static void gmGmkFlipperMainHook(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        gmGmkFlipperUpdateAngle(obj_work);
        if (false)
            return;
        if (gmGmkFlipperCheckControlPlayer() == 0)
        {
            gmGmkFlipperChangeModeWait(obj_work);
        }
        else
        {
            int flipper_type = gmGmkFlipperCalcType(gmsEnemy3DWork.ene_com.eve_rec.id);
            gmGmkFlipperSetRideSpeed(targetObj, obj_work, flipper_type);
            if ((g_gm_main_system.ply_work[0].key_on & 160) == 0)
            {
                gmGmkFlipperChangeModeReady(obj_work);
            }
            else
            {
                int num = targetObj.pos.x - obj_work.pos.x;
                if (flipper_type == 1)
                    num = -num;
                if (num <= 0)
                    return;
                targetObj.spd.x = 0;
                targetObj.spd.y = 0;
            }
        }
    }

    private static int gmGmkFlipperCheckKeyHit(
      OBS_OBJECT_WORK gimmick_obj_work,
      GMS_PLAYER_WORK player_work)
    {
        return GmPlayerKeyCheckJumpKeyPush(player_work) ? 1 : 0;
    }

    private static int gmGmkFlipperCheckControlPlayer()
    {
        return g_gm_main_system.ply_work[0].seq_state != 47 ? 0 : 1;
    }

    private static int gmGmkFlipperCheckScore(OBS_OBJECT_WORK obj_work)
    {
        uint num = ((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.width * 1000U;
        if (num == 0U)
            return 0;
        uint score = g_gm_main_system.ply_work[0].score;
        return num > score ? 0 : 1;
    }

    private static int gmGmkFlipperCheckLeft(
      VecFx32 line_start,
      VecFx32 line_end,
      VecFx32 point)
    {
        int v1_1 = line_end.x - line_start.x;
        int v1_2 = line_end.y - line_start.y;
        int v2_1 = point.x - line_start.x;
        int v2_2 = point.y - line_start.y;
        return FX_Mul(v1_1, v2_2) - FX_Mul(v1_2, v2_1) <= 0 ? 1 : 0;
    }

    private static int gmGmkFlipperCheckRect(
      VecFx32 gimmick_pos,
      VecFx32 target_pos,
      int type)
    {
        switch (type)
        {
            case 0:
                VecFx32 line_start1 = new VecFx32(gimmick_pos);
                line_start1.y += FX_F32_TO_FX32(g_gmk_flipper_rect[type][1] - 12);
                VecFx32 line_end1 = new VecFx32(gimmick_pos);
                line_end1.x += FX_F32_TO_FX32(g_gmk_flipper_rect[type][2]);
                line_end1.y += FX_F32_TO_FX32(g_gmk_flipper_rect[type][3] - 12);
                if (gmGmkFlipperCheckLeft(line_start1, line_end1, target_pos) != 0)
                    return 0;
                break;
            case 1:
                VecFx32 line_end2 = new VecFx32(gimmick_pos);
                line_end2.y += FX_F32_TO_FX32(g_gmk_flipper_rect[type][1] - 12);
                VecFx32 line_start2 = new VecFx32(gimmick_pos);
                line_start2.x += FX_F32_TO_FX32(g_gmk_flipper_rect[type][0]);
                line_start2.y += FX_F32_TO_FX32(g_gmk_flipper_rect[type][3] - 12);
                if (gmGmkFlipperCheckLeft(line_start2, line_end2, target_pos) != 0)
                    return 0;
                break;
        }
        return 1;
    }

    private static int gmGmkFlipperCheckHook(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        int num1 = targetObj.pos.x - obj_work.pos.x;
        int num2 = 16384;
        int flipper_type = gmGmkFlipperCalcType(gmsEnemy3DWork.ene_com.eve_rec.id);
        if (flipper_type == 1)
        {
            num1 = -num1;
            num2 = -num2;
        }
        if (num1 > 16384)
            return 0;
        if (num1 > 0)
        {
            GMS_PLAYER_WORK gmsPlayerWork = (GMS_PLAYER_WORK)targetObj;
            targetObj.pos.x = obj_work.pos.x;
            int num3 = gmGmkFlipperCalcRideOffsetY(obj_work.pos.x + num2, obj_work, flipper_type);
            int num4 = ((int)gmsPlayerWork.player_flag & 131072) == 0 ? num3 - 36864 : num3 - 61440;
            targetObj.pos.y = obj_work.pos.y + num4;
        }
        return 1;
    }

    private static void gmGmkFlipperSetRideSpeed(
      OBS_OBJECT_WORK target_obj_work,
      OBS_OBJECT_WORK gimmick_obj_work,
      int flipper_type)
    {
        UNREFERENCED_PARAMETER(gimmick_obj_work);
        int num = FX_F32_TO_FX32(0.6857143f);
        int fx32 = FX_F32_TO_FX32(0.3714286f);
        if (flipper_type == 1)
            num = -num;
        target_obj_work.spd.x = num;
        target_obj_work.spd.y = fx32;
        target_obj_work.spd.x = FX_Div(target_obj_work.spd.x, 12288);
        target_obj_work.spd.y = FX_Div(target_obj_work.spd.y, 12288);
    }

    private static int gmGmkFlipperCalcRideOffsetY(
      int x,
      OBS_OBJECT_WORK gimmick_obj_work,
      int flipper_type)
    {
        float num = g_gmk_flipper_rect[flipper_type][2] - g_gmk_flipper_rect[flipper_type][0];
        if (flipper_type == 1)
            num = -num;
        return FX_Mul((int)((g_gmk_flipper_rect[flipper_type][3] - g_gmk_flipper_rect[flipper_type][1] - 2f) / (double)num * 4096.0), x - gimmick_obj_work.pos.x);
    }

    private static int gmGmkFlipperUpdateAngle(OBS_OBJECT_WORK obj_work)
    {
        ++obj_work.user_timer;
        ushort userWork = (ushort)obj_work.user_work;
        ushort num = (ushort)((userWork - obj_work.dir.z) / 6);
        obj_work.dir.z += num;
        if (obj_work.user_timer < 6)
            return 0;
        obj_work.dir.z = userWork;
        obj_work.user_timer = 0;
        return 1;
    }

}