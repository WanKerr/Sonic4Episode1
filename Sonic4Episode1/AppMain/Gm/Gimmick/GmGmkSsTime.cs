public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSsTimeInit(
     GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_SS_TIME");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= 128;
        uint num = MTM_MATH_CLIP(eve_rec.flag & 3U, 0U, 2U);
        work.user_work = num;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_ss_time_obj_3d_list[(int)num], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.scale.x = work.scale.y = work.scale.z = 6144;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsTimeMain);
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkSsTimeDefFunc);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectWorkSet(pRec, -6, -6, 6, 6);
        return work;
    }

    public static void GmGmkSsTimeBuild()
    {
        gm_gmk_ss_time_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(915), GmGameDatGetGimmickData(916), 0U);
    }

    public static void GmGmkSsTimeFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(915);
        GmGameDBuildRegFlushModel(gm_gmk_ss_time_obj_3d_list, gimmickData.file_num);
    }

    private static void gmGmkSsTimeMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)GmSplStageGetWork().flag & 4) != 0)
            obj_work.flag |= 4U;
        else
            obj_work.dir.z = GmMainGetObjectRotation();
    }

    private static void gmGmkSsTimeDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != 1 || parentObj2.gmk_obj == (OBS_OBJECT_WORK)parentObj1))
            return;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork1 = GmEfctZoneEsCreate(parentObj1.obj_work, 5, 17);
        gmsEffect3DesWork1.efct_com.obj_work.flag |= 512U;
        gmsEffect3DesWork1.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsTimeEfctMain);
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork2 = GmEfctZoneEsCreate(parentObj1.obj_work, 5, gm_gmk_ss_time_add_msg[GsEnvGetLanguage()]);
        gmsEffect3DesWork2.efct_com.obj_work.flag |= 512U;
        gmsEffect3DesWork2.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsTimeEfctMain);
        gmsEffect3DesWork2.obj_3des.command_state = 10U;
        parentObj1.enemy_flag |= 65536U;
        GmSoundPlaySE("Special6");
        GmFixRequestTimerFlash();
        g_gm_main_system.game_time += gm_gmk_ss_time_add_subtract[(int)parentObj1.obj_work.user_work];
        parentObj1.obj_work.flag |= 4U;
    }

    private static void gmGmkSsTimeEfctMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_CAMERA obsCamera = ObjCameraGet(g_obj.glb_camera_id);
        if (obsCamera == null)
            return;
        obj_work.dir.z = (ushort)-obsCamera.roll;
    }

}