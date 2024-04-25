public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkBobbinInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkBobbinLoadObj(eve_rec, pos_x, pos_y).ene_com.obj_work;
        gmGmkBobbinInit(objWork);
        return objWork;
    }

    public static void GmGmkBobbinBuild()
    {
        g_gm_gmk_bobbin_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(863)), readAMBFile(GmGameDatGetGimmickData(864)), 0U);
    }

    public static void GmGmkBobbinFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(863));
        GmGameDBuildRegFlushModel(g_gm_gmk_bobbin_obj_3d_list, amsAmbHeader.file_num);
        g_gm_gmk_bobbin_obj_3d_list = null;
    }

    private static GMS_ENEMY_3D_WORK gmGmkBobbinLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_BOBBIN");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkBobbinLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkBobbinLoadObjNoModel(eve_rec, pos_x, pos_y);
        OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index = 0;
        ObjObjectCopyAction3dNNModel(objWork, g_gm_gmk_bobbin_obj_3d_list[index], gmsEnemy3DWork.obj_3d);
        OBS_DATA_WORK data_work1 = ObjDataGet(865);
        ObjObjectAction3dNNMotionLoad(objWork, 0, false, data_work1, null, 0, null);
        OBS_DATA_WORK data_work2 = ObjDataGet(866);
        ObjObjectAction3dNNMaterialMotionLoad(objWork, 0, data_work2, null, 0, null);
        return gmsEnemy3DWork;
    }

    private static void gmGmkBobbinInit(OBS_OBJECT_WORK obj_work)
    {
        gmGmkBobbinSetRect((GMS_ENEMY_3D_WORK)obj_work);
        obj_work.move_flag = 8448U;
        obj_work.disp_flag |= 4194308U;
        obj_work.pos.z = -131072;
        if (GSM_MAIN_STAGE_IS_SPSTAGE())
            obj_work.pos.z = -65536;
        obj_work.ppFunc = null;
        obj_work.ppMove = null;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBobbinDrawFunc);
        gmGmkBobbinChangeModeWait(obj_work);
    }

    private static void gmGmkBobbinSetRect(GMS_ENEMY_3D_WORK gimmick_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)gimmick_work;
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        short cLeft = -24;
        short cRight = 24;
        short cTop = -24;
        short cBottom = 24;
        ObjRectWorkZSet(pRec, cLeft, cTop, -500, cRight, cBottom, 500);
        pRec.flag |= 1024U;
        ObjRectGroupSet(pRec, 1, 1);
        ObjRectDefSet(pRec, 65534, 0);
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkBobbinDefFunc);
        if (!GSM_MAIN_STAGE_IS_SPSTAGE())
            return;
        OBS_COLLISION_WORK colWork = ((GMS_ENEMY_3D_WORK)obsObjectWork).ene_com.col_work;
        colWork.obj_col.obj = obsObjectWork;
        colWork.obj_col.diff_data = g_gm_default_col;
        colWork.obj_col.width = 16;
        colWork.obj_col.height = 16;
        colWork.obj_col.ofst_x = (short)-(colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-(colWork.obj_col.height / 2);
        colWork.obj_col.attr = 2;
        colWork.obj_col.flag |= 134217760U;
    }

    private static void gmGmkBobbinDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawActionSummary(obj_work);
    }

    private static VecFx32 gmGmkBobbinNormalizeVectorXY(VecFx32 vec)
    {
        VecFx32 vecFx32 = new VecFx32();
        int x = FX_Mul(vec.x, vec.x) + FX_Mul(vec.y, vec.y);
        if (x == 0)
        {
            vecFx32.x = 4096;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = FX_Div(4096, FX_Sqrt(x));
            vecFx32.x = FX_Mul(vec.x, v2);
            vecFx32.y = FX_Mul(vec.y, v2);
        }
        vecFx32.z = 0;
        if (GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            int dest_x = 0;
            int dest_y = 0;
            ObjUtilGetRotPosXY(vecFx32.x, vecFx32.y, ref dest_x, ref dest_y, (ushort)-g_gm_main_system.pseudofall_dir);
            vecFx32.x = dest_x;
            vecFx32.y = dest_y;
        }
        return vecFx32;
    }

    private static void gmGmkBobbinDefPlayer(
      GMS_ENEMY_3D_WORK gimmick_work,
      GMS_PLAYER_WORK player_work,
      int speed_x,
      int speed_y)
    {
        bool flag_no_recover_homing = false;
        if ((gimmick_work.ene_com.eve_rec.flag & 1) != 0)
            flag_no_recover_homing = true;
        GmPlySeqInitPinballAir(player_work, speed_x, speed_y, 5, flag_no_recover_homing);
        if (GMM_MAIN_STAGE_IS_SS())
            return;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)gimmick_work;
        GmPlayerAddScore(player_work, 10, obsObjectWork.pos.x, obsObjectWork.pos.y);
    }

    private static void gmGmkBobbinDefEnemy(
      OBS_OBJECT_WORK obj_work,
      int speed_x,
      int speed_y)
    {
        if (((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id != 316)
            return;
        obj_work.spd.x = speed_x;
        obj_work.spd.y = speed_y;
        obj_work.spd_add.x = 0;
        obj_work.spd_add.y = 0;
        if (MTM_MATH_ABS(obj_work.spd.x) < 256)
        {
            obj_work.spd.x = 256;
        }
        else
        {
            if (MTM_MATH_ABS(obj_work.spd.y) >= 256)
                return;
            obj_work.spd.y = 256;
        }
    }

    private static void gmGmkBobbinDefFunc(
      OBS_RECT_WORK gimmick_rect,
      OBS_RECT_WORK player_rect)
    {
        OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        GMS_ENEMY_3D_WORK gimmick_work = (GMS_ENEMY_3D_WORK)parentObj1;
        OBS_OBJECT_WORK parentObj2 = player_rect.parent_obj;
        VecFx32 vec = new VecFx32();
        vec.x = parentObj2.prev_pos.x - parentObj1.pos.x;
        vec.y = (int)(parentObj2.prev_pos.y + -12288L - parentObj1.pos.y);
        vec.z = 0;
        if (FX_Mul(114688, 114688) < FX_Mul(vec.x, vec.x) + FX_Mul(vec.y, vec.y))
        {
            gimmick_rect.flag &= 4294966271U;
        }
        else
        {
            gimmick_rect.flag |= 1024U;
            VecFx32 vecFx32 = gmGmkBobbinNormalizeVectorXY(vec);
            parentObj2.dir.z = 0;
            int v1_1 = FX_Mul(vecFx32.x, 24576);
            int v1_2 = FX_Mul(vecFx32.y, 24576);
            int v2_1 = FX_F32_TO_FX32((float)((100.0 + gimmick_work.ene_com.eve_rec.left) * 0.00999999977648258));
            if (v2_1 < 0)
                v2_1 = 0;
            int v2_2 = FX_F32_TO_FX32((float)((100.0 + gimmick_work.ene_com.eve_rec.top) * 0.00999999977648258));
            if (v2_2 < 0)
                v2_2 = 0;
            int num1 = FX_Mul(v1_1, v2_1);
            int num2 = FX_Mul(v1_2, v2_2);
            if (parentObj2.obj_type == 1)
                gmGmkBobbinDefPlayer(gimmick_work, (GMS_PLAYER_WORK)parentObj2, num1, num2);
            else if (parentObj2.obj_type == 2)
                gmGmkBobbinDefEnemy(parentObj2, num1, num2);
            gmGmkBobbinChangeModeHit(parentObj1);
            GmSoundPlaySE("Casino1");
            GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(parentObj1, 16);
            gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj2.pos.x;
            gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj2.pos.y;
            gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
            gmsEffect3DesWork.efct_com.obj_work.dir.z = (ushort)(nnArcTan2(FX_FX32_TO_F32(num2), FX_FX32_TO_F32(num1)) - 16384);
            if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            {
                OBS_CAMERA obsCamera = ObjCameraGet(g_obj.glb_camera_id);
                if (obsCamera != null)
                    gmsEffect3DesWork.efct_com.obj_work.dir.z -= (ushort)obsCamera.roll;
            }
            GMM_PAD_VIB_SMALL();
        }
    }

    private static void gmGmkBobbinChangeModeWait(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawObjectActionSet3DNN(obj_work, 0, 0);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBobbinMainWait);
    }

    private static void gmGmkBobbinChangeModeHit(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        ObjDrawObjectActionSet3DNN(obj_work, 1, 0);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBobbinMainHit);
    }

    private static void gmGmkBobbinMainWait(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkBobbinMainHit(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        gmGmkBobbinChangeModeWait(obj_work);
    }

}