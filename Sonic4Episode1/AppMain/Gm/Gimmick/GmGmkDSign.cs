public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkDSignInit(
         GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_DSIGN");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_dsign_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -917504;
        work.flag |= 2U;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        int num = MTM_MATH_CLIP(eve_rec.id - 287, 0, 3);
        work.dir.z = (ushort)(num * 16384);
        return work;
    }

    public static void GmGmkDSignBuild()
    {
        gm_gmk_dsign_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(954), GmGameDatGetGimmickData(955), 0U);
    }

    public static void GmGmkDSignFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(954);
        GmGameDBuildRegFlushModel(gm_gmk_dsign_obj_3d_list, gimmickData.file_num);
    }

}