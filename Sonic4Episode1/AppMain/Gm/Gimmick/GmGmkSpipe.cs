public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSpipeInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_COM_WORK(), "GMK_S_PIPE");
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)work;
        work.move_flag |= 8480U;
        work.pos.z = -131072;
        OBS_RECT_WORK pRec = gmsEnemyComWork.rect_work[2];
        ObjRectGroupSet(pRec, 1, 1);
        ObjRectAtkSet(pRec, 0, 1);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectSet(pRec.rect, (short)(eve_rec.left * 2), (short)(eve_rec.top * 2), (short)((ushort)(eve_rec.width * 2U) + (short)(eve_rec.left * 2)), (short)((ushort)(eve_rec.height * 2U) + (short)(eve_rec.top * 2)));
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkSpipeDefFunc);
        pRec.parent_obj = work;
        pRec.flag |= 192U;
        return work;
    }

    private static void gmGmkSpipeDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != 1)
            return;
        if (parentObj2.seq_state != GME_PLY_SEQ_STATE_GMK_SPIPE)
            GmPlySeqInitSpipe(parentObj2);
        parentObj2.gmk_flag |= 65536U;
    }

}