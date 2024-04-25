public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkScrewInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_COM_WORK(), "GMK_SCREW");
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)work;
        work.move_flag |= 8480U;
        gmsEnemyComWork.rect_work[0].flag &= 4294967291U;
        gmsEnemyComWork.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsEnemyComWork.rect_work[2];
        pRec.ppHit = null;
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkScrewDefFunc);
        ObjRectAtkSet(pRec, 0, 0);
        ObjRectDefSet(pRec, 65534, 0);
        if ((eve_rec.flag & GMD_GMK_SCREW_EVE_FLAG_LEFT) != 0)
            ObjRectWorkSet(pRec, -4, -8, -16, 0);
        else
            ObjRectWorkSet(pRec, 4, -8, 16, 0);
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkScrewMain);
        return work;
    }

    private static void gmGmkScrewMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (gmsPlayerWork.seq_state == GME_PLY_SEQ_STATE_GMK_SCREW || gmsPlayerWork.gmk_obj == obj_work)
            return;
        obj_work.flag &= 4294967279U;
    }

    private static void gmGmkScrewDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != 1 || parentObj2.seq_state == GME_PLY_SEQ_STATE_GMK_SCREW))
            return;
        int spd3 = parentObj2.spd3;
        ushort flag = parentObj1.eve_rec.flag;
        if ((parentObj2.obj_work.spd_m < spd3 || (parentObj1.eve_rec.flag & GMD_GMK_SCREW_EVE_FLAG_LEFT) != 0) && (parentObj2.obj_work.spd_m > -spd3 || (parentObj1.eve_rec.flag & GMD_GMK_SCREW_EVE_FLAG_LEFT) == 0) || ((int)parentObj2.obj_work.move_flag & 1) == 0)
            return;
        GmPlySeqInitScrew(parentObj2, parentObj1, parentObj1.obj_work.pos.x, parentObj1.obj_work.pos.y, flag);
        parentObj1.obj_work.flag |= 16U;
    }

}