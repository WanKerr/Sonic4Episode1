public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkNeedleNeonInitStand(
     GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkNeedleNeonLoadObj(eve_rec, pos_x, pos_y, type, 2U).ene_com.obj_work;
        gmGmkNeedleNeonStandInit(objWork);
        OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(337, objWork.pos.x, objWork.pos.y, eve_rec.flag, eve_rec.left, eve_rec.top, eve_rec.width, eve_rec.height, type);
        obsObjectWork.parent_obj = objWork;
        obsObjectWork.user_work = (uint)(objWork.pos.y + 131072);
        return objWork;
    }

    private static OBS_OBJECT_WORK GmGmkNeedleNeonInitNeedle(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkNeedleNeonLoadObj(eve_rec, pos_x, pos_y, type, 0U).ene_com.obj_work;
        gmGmkNeedleNeonNeedleInit(objWork);
        return objWork;
    }

    private static OBS_OBJECT_WORK GmGmkNeedleNeonInitGlaer(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(eve_rec);
        UNREFERENCED_PARAMETER(pos_x);
        UNREFERENCED_PARAMETER(pos_y);
        UNREFERENCED_PARAMETER(type);
        return null;
    }

    public static void GmGmkNeedleNeonBuild()
    {
        g_gm_gmk_needle_neon_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(822), GmGameDatGetGimmickData(823), 0U);
        g_gm_gmk_needle_neon_obj_tvx_list = GmGameDatGetGimmickData(824);
    }

    public static void GmGmkNeedleNeonFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(822);
        GmGameDBuildRegFlushModel(g_gm_gmk_needle_neon_obj_3d_list, gimmickData.file_num);
        g_gm_gmk_needle_neon_obj_3d_list = null;
        g_gm_gmk_needle_neon_obj_tvx_list = null;
    }

    private static void GmGmkNeedleNeonChangeModeActive(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_flag &= 4294967294U;
        if (obj_work.ppFunc == new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonNeedleMainOn) || obj_work.ppFunc == new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonNeedleMainActive))
            return;
        gmGmkNeedleNeonNeedleChangeModeOn(obj_work);
    }

    private static void GmGmkNeedleNeonChangeModeWait(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_flag &= 4294967294U;
        if (obj_work.ppFunc == new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonNeedleMainWait) || obj_work.ppFunc == new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonNeedleMainOff))
            return;
        gmGmkNeedleNeonNeedleChangeModeOff(obj_work);
    }

    private static void GmGmkNeedleNeonChangeModeTimer(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_flag |= 1U;
    }

    private static GMS_ENEMY_3D_WORK gmGmkNeedleNeonLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_NEEDLE_NEON");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkNeedleNeonLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      uint model_index)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkNeedleNeonLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        ObjObjectCopyAction3dNNModel(gmsEnemy3DWork.ene_com.obj_work, g_gm_gmk_needle_neon_obj_3d_list[(int)model_index], gmsEnemy3DWork.obj_3d);
        return gmsEnemy3DWork;
    }

    private static void gmGmkNeedleNeonDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || obj_work.ppFunc == new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonNeedleMainWait) || ((int)obj_work.disp_flag & 32) != 0)
            return;
        gmGmkNeedleNeonTvxDrawFunc(new TVX_FILE((AmbChunk)amBindGet(g_gm_gmk_needle_neon_obj_tvx_list, 0)), obj_work.obj_3d.texlist, obj_work.pos);
    }

    private static void gmGmkNeedleNeonStandDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        TVX_FILE tvx;
        if (g_gm_gmk_needle_neon_obj_tvx_list.buf[1] == null)
        {
            tvx = new TVX_FILE((AmbChunk)amBindGet(g_gm_gmk_needle_neon_obj_tvx_list, 1));
            g_gm_gmk_needle_neon_obj_tvx_list.buf[1] = tvx;
        }
        else
            tvx = (TVX_FILE)g_gm_gmk_needle_neon_obj_tvx_list.buf[1];
        gmGmkNeedleNeonTvxDrawFunc(tvx, obj_work.obj_3d.texlist, obj_work.pos);
    }

    private static void gmGmkNeedleNeonTvxDrawFunc(
      TVX_FILE tvx,
      NNS_TEXLIST texlist,
      VecFx32 base_pos)
    {
        VecFx32 scale = new VecFx32(4096, 4096, 4096);
        for (int index = 0; 5 > index; ++index)
        {
            VecFx32 pos;
            pos.x = base_pos.x + g_gm_gmk_disp_offset[index].x;
            pos.y = base_pos.y + g_gm_gmk_disp_offset[index].y;
            pos.z = base_pos.z + g_gm_gmk_disp_offset[index].z;
            GmTvxSetModel(tvx, texlist, ref pos, ref scale, 0U, 0);
        }
    }

    private static void gmGmkNeedleNeonStandInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 8449U;
        obj_work.disp_flag |= 4194304U;
        obj_work.pos.z = -655360;
        obj_work.ppFunc = null;
        obj_work.ppMove = null;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonStandDrawFunc);
    }

    private static void gmGmkNeedleNeonNeedleInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = 24;
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = 30;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = -12;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = -32;
        OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkZSet(pRec, -16, -33, -500, 16, -8, 500);
        pRec.flag |= 1024U;
        obj_work.flag |= 2U;
        obj_work.move_flag |= 8449U;
        obj_work.disp_flag |= 4194304U;
        obj_work.flag |= 16U;
        obj_work.pos.z = -655360;
        obj_work.ppMove = null;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonDrawFunc);
        gmGmkNeedleNeonNeedleChangeModeWait(obj_work);
    }

    private static void gmGmkNeedleNeonNeedleChangeModeWait(OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonNeedleMainWait);
    }

    private static void gmGmkNeedleNeonNeedleChangeModeOn(OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonNeedleMainOn);
        GmSoundPlaySE("Boss2_06");
    }

    private static void gmGmkNeedleNeonNeedleChangeModeActive(OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonNeedleMainActive);
    }

    private static void gmGmkNeedleNeonNeedleChangeModeOff(OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag |= 2U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleNeonNeedleMainOff);
    }

    private static void gmGmkNeedleNeonNeedleMainWait(OBS_OBJECT_WORK obj_work)
    {
        int userWork = (int)obj_work.user_work;
        obj_work.pos.y = userWork;
        if (((int)obj_work.user_flag & 1) == 0)
            return;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 480)
            return;
        obj_work.user_timer = 0;
        gmGmkNeedleNeonNeedleChangeModeOn(obj_work);
    }

    private static void gmGmkNeedleNeonNeedleMainOn(OBS_OBJECT_WORK obj_work)
    {
        gmGmkNeedleNeonNeedleUpdateHitRect(obj_work);
        int userWork = (int)obj_work.user_work;
        int fx32 = FX_F32_TO_FX32(32 * obj_work.user_timer / 10);
        obj_work.pos.y = userWork - fx32;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 10)
            return;
        obj_work.user_timer = 0;
        gmGmkNeedleNeonNeedleChangeModeActive(obj_work);
    }

    private static void gmGmkNeedleNeonNeedleMainActive(OBS_OBJECT_WORK obj_work)
    {
        gmGmkNeedleNeonNeedleUpdateHitRect(obj_work);
        int userWork = (int)obj_work.user_work;
        obj_work.pos.y = userWork - 131072;
        if (((int)obj_work.user_flag & 1) == 0)
            return;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 180)
            return;
        obj_work.user_timer = 0;
        gmGmkNeedleNeonNeedleChangeModeOff(obj_work);
    }

    private static void gmGmkNeedleNeonNeedleMainOff(OBS_OBJECT_WORK obj_work)
    {
        int userWork = (int)obj_work.user_work;
        int fx32 = FX_F32_TO_FX32(32 - 32 * obj_work.user_timer / 10);
        obj_work.pos.y = userWork - fx32;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 10)
            return;
        obj_work.user_timer = 0;
        gmGmkNeedleNeonNeedleChangeModeWait(obj_work);
    }

    private static void gmGmkNeedleNeonNeedleUpdateHitRect(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_ENEMY_3D_WORK)obj_work).ene_com.col_work.obj_col.rider_obj != null)
            obj_work.flag &= 4294967293U;
        else
            obj_work.flag |= 2U;
    }

}