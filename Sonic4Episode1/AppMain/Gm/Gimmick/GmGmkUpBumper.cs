public partial class AppMain
{
    public static OBS_OBJECT_WORK GmGmkUpBumperLInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_UPBUMPER_WORK gmsGmkUpbumperWork = (GMS_GMK_UPBUMPER_WORK)gmGmkUpBumperInit(eve_rec, pos_x, pos_y, type);
        gmsGmkUpbumperWork.obj_type = 0;
        gmGmkUpBumperStart(gmsGmkUpbumperWork.gmk_work.ene_com.obj_work);
        return gmsGmkUpbumperWork.gmk_work.ene_com.obj_work;
    }

    public static OBS_OBJECT_WORK GmGmkUpBumperRInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obsObjectWork = gmGmkUpBumperInit(eve_rec, pos_x, pos_y, type);
        GMS_GMK_UPBUMPER_WORK gmsGmkUpbumperWork = (GMS_GMK_UPBUMPER_WORK)obsObjectWork;
        obsObjectWork.disp_flag &= 4290772991U;
        obsObjectWork.obj_3d.drawflag |= 32U;
        obsObjectWork.dir.y = 16384;
        gmsGmkUpbumperWork.obj_type = 1;
        gmGmkUpBumperStart(gmsGmkUpbumperWork.gmk_work.ene_com.obj_work);
        return obsObjectWork;
    }

    public static void GmGmkUpBumperBuild()
    {
        gm_gmk_upbumper_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(850), GmGameDatGetGimmickData(851), 0U);
    }

    public static void GmGmkUpBumperFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(850);
        GmGameDBuildRegFlushModel(gm_gmk_upbumper_obj_3d_list, gimmickData.file_num);
    }

    private static void gmGmkUpBumperStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_UPBUMPER_WORK gmsGmkUpbumperWork = (GMS_GMK_UPBUMPER_WORK)obj_work;
        gmsGmkUpbumperWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsGmkUpbumperWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsGmkUpbumperWork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkUpBumperHit);
        pRec.ppHit = null;
        ObjRectAtkSet(pRec, 0, 0);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectWorkSet(pRec, GmkUpBumperData.tbl_gm_gmk_upbumper_rect[gmsGmkUpbumperWork.obj_type][0], GmkUpBumperData.tbl_gm_gmk_upbumper_rect[gmsGmkUpbumperWork.obj_type][1], GmkUpBumperData.tbl_gm_gmk_upbumper_rect[gmsGmkUpbumperWork.obj_type][2], GmkUpBumperData.tbl_gm_gmk_upbumper_rect[gmsGmkUpbumperWork.obj_type][3]);
        obj_work.flag &= 4294967293U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkUpBumperStay);
    }

    private static void gmGmkUpBumperStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_UPBUMPER_WORK gmsGmkUpbumperWork = (GMS_GMK_UPBUMPER_WORK)obj_work;
        if (gmsGmkUpbumperWork.player_spd_keep_timer_mine <= 0)
            return;
        if (gmsGmkUpbumperWork.player_spd_keep_timer_mine > player_spd_keep_timer)
        {
            gmsGmkUpbumperWork.player_spd_keep_timer_mine = player_spd_keep_timer;
            --player_spd_keep_timer;
            if (player_spd_keep_timer > 0)
                return;
            gmsGmkUpbumperWork.player_spd_keep_timer_mine = 0;
            player_spd_keep_timer = 0;
            player_spd_x = player_spd_y = 0;
        }
        else
            gmsGmkUpbumperWork.player_spd_keep_timer_mine = 0;
    }

    private static void gmGmkUpBumperHit(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_GMK_UPBUMPER_WORK parentObj1 = (GMS_GMK_UPBUMPER_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 == g_gm_main_system.ply_work[0])
        {
            int spd_y = 0;
            int spd_x;
            if (player_spd_keep_timer <= 0)
            {
                spd_x = 0;
                for (uint index = 0; index < GMD_GMK_UPBUMPER_REBOUND_DATA_NUM; ++index)
                {
                    if (parentObj2.act_state == tbl_upbmper_rebound_data[(int)index].act_state)
                    {
                        spd_x = tbl_upbmper_rebound_data[(int)index].spd_x;
                        spd_y = tbl_upbmper_rebound_data[(int)index].spd_y;
                        player_spd_x = spd_x;
                        player_spd_y = spd_y;
                        player_spd_keep_timer = 60;
                        parentObj1.player_spd_keep_timer_mine = (short)(player_spd_keep_timer + 1);
                        break;
                    }
                }
                if (spd_x == 0)
                {
                    int num = MTM_MATH_ABS(parentObj2.obj_work.spd.x);
                    spd_x = num + (num >> 3);
                    if (spd_x > 32768)
                        spd_x = 32768;
                    if (spd_x < 16384)
                        spd_x = 16384;
                    spd_y = -16384;
                }
            }
            else
            {
                spd_x = player_spd_x;
                spd_y = player_spd_y;
                player_spd_keep_timer = 60;
                parentObj1.player_spd_keep_timer_mine = (short)(player_spd_keep_timer + 1);
            }
            if (parentObj1.obj_type == 1)
                spd_x = -spd_x;
            GmPlySeqGmkInitUpBumper(parentObj2, spd_x, spd_y);
            GMM_PAD_VIB_SMALL();
        }
        mine_rect.flag &= 4294573823U;
    }

    private static OBS_OBJECT_WORK gmGmkUpBumperInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_UPBUMPER_WORK work = (GMS_GMK_UPBUMPER_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_UPBUMPER_WORK(), "Gmk_UpBumper");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_upbumper_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -4096;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        return obj_work;
    }

}