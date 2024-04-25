public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSsArrowInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_SS_ARROW");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= 128;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_ss_arrow_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsArrowMain);
        ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, null, null, 0, readAMBFile(ObjDataGet(986).pData));
        ObjDrawObjectActionSet3DNNMaterial(work, 0);
        work.dir.z = (ushort)((uint)eve_rec.width << 8);
        int num = (int)(g_gm_main_system.sync_time % 24U) - (MTM_MATH_CLIP(eve_rec.left, 0, 3) << 3);
        if (num < 0)
            num += 24;
        work.user_timer = num;
        gmsEnemy3DWork.obj_3d.mat_frame = num;
        return work;
    }

    public static void GmGmkSsArrowBuild()
    {
        gm_gmk_ss_arrow_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(984)), readAMBFile(GmGameDatGetGimmickData(985)), 0U);
    }

    public static void GmGmkSsArrowFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(984));
        GmGameDBuildRegFlushModel(gm_gmk_ss_arrow_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkSsArrowMain(OBS_OBJECT_WORK obj_work)
    {
        if (GSM_MAIN_STAGE_IS_SPSTAGE() && ((int)GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            ++obj_work.user_timer;
            if (obj_work.user_timer < 24)
                return;
            obj_work.user_timer = 0;
            ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        }
    }

}