public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkBreakFloorInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_BWALL_WORK work = (GMS_GMK_BWALL_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_BWALL_WORK(), "GMK_BREAK_LAND_MAIN");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ushort num = tbl_breakwall_mdl[g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id]][6];
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_breakwall_obj_3d_list[num], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -131072;
        if ((eve_rec.flag & 2) != 0)
            obj_work.pos.z -= 4096;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        work.broketype = (ushort)(eve_rec.flag & 1U);
        if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
        {
            gmsEnemy3DWork.obj_3d.use_light_flag &= 4294967294U;
            gmsEnemy3DWork.obj_3d.use_light_flag |= 2U;
        }
        work.obj_type = 2;
        work.wall_type = 6;
        gmGmkBreakWallStart(obj_work);
        return obj_work;
    }

    private static void GmGmkBreakWallBuild()
    {
        gm_gmk_breakwall_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(797), GmGameDatGetGimmickData(798), 0U);
    }

    private static void GmGmkBreakWallFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(797));
        GmGameDBuildRegFlushModel(gm_gmk_breakwall_obj_3d_list, amsAmbHeader.file_num);
    }
}