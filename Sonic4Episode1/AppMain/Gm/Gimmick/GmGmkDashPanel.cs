public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkDashPanelInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_DASH_PANEL");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_dash_panel_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, false, ObjDataGet(827), null, 0, null);
        ObjDrawObjectActionSet(work, 0);
        ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, null, null, 0, (AMS_AMB_HEADER)ObjDataGet(828).pData);
        ObjDrawObjectActionSet3DNNMaterial(work, 0);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppHit = null;
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkDashPanelDefFunc);
        ObjRectAtkSet(pRec, 0, 0);
        ObjRectDefSet(pRec, 65534, 0);
        if (g_gs_main_sys_info.stage_id == 9 && (eve_rec.id == 109 || eve_rec.id == 110))
            ObjRectWorkSet(pRec, -16, -8, 16, 8);
        else
            ObjRectWorkSet(pRec, -8, -8, 8, 8);
        pRec.flag |= 1024U;
        if (eve_rec.id == 108)
            work.dir.y = 32768;
        else if (eve_rec.id == 109)
            work.dir.z = 49152;
        else if (eve_rec.id == 110)
        {
            work.dir.z = 16384;
            work.dir.y = 32768;
        }
        else
        {
            work.dir.z = 0;
            work.dir.y = 0;
        }
        work.ppFunc = null;
        return work;
    }

    public static void GmGmkDashPanelBuild()
    {
        gm_gmk_dash_panel_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(825), GmGameDatGetGimmickData(826), 0U);
    }

    public static void GmGmkDashPanelFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(825));
        GmGameDBuildRegFlushModel(gm_gmk_dash_panel_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkDashPanelDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != 1)
            return;
        int id = parentObj1.eve_rec.id;
        if (((int)parentObj2.obj_work.move_flag & 1) == 0)
        {
            parentObj1.rect_work[2].flag &= 4294573823U;
        }
        else
        {
            GmPlySeqInitDashPanel(parentObj2, parentObj1.eve_rec.id - 107U);
            GmSoundPlaySE("DashPanel");
        }
    }

}