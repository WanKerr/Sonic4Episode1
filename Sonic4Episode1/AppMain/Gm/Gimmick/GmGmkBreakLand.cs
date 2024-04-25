public partial class AppMain
{
    private static void gmGmkBreakLandStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BLAND_WORK gmsGmkBlandWork = (GMS_GMK_BLAND_WORK)obj_work;
        OBS_OBJECT_WORK objWork = g_gm_main_system.ply_work[0].obj_work;
        if (objWork.ride_obj != obj_work)
            return;
        int num1 = obj_work.pos.x + gmsGmkBlandWork.colrect_left;
        int num2 = obj_work.pos.x + gmsGmkBlandWork.colrect_right;
        int num3 = num1 - (objWork.field_rect[0] << 12);
        int num4 = num2 - (objWork.field_rect[2] << 12);
        if (num3 > objWork.pos.x || objWork.pos.x > num4)
            return;
        if (objWork.pos.x - obj_work.pos.x <= gmsGmkBlandWork.gmk_work.ene_com.col_work.obj_col.width * 4096 / 2)
        {
            int num5 = -(gmsGmkBlandWork.gmk_work.ene_com.col_work.obj_col.width * 4096 / 2);
        }
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctZoneEsCreate(null, 0, 0);
        obsObjectWork.pos.x = obj_work.pos.x;
        obsObjectWork.pos.y = obj_work.pos.y;
        obsObjectWork.pos.z = obj_work.pos.z + 131072;
        if (gmsGmkBlandWork.vect == 0)
            obsObjectWork.pos.x += 262144;
        else
            obsObjectWork.pos.x -= 262144;
        gmsGmkBlandWork.broken_timer = 45;
        gmsGmkBlandWork.quake_timer = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBreakLandStay_100);
    }

    private static void gmGmkBreakLandStay_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BLAND_WORK gmsGmkBlandWork = (GMS_GMK_BLAND_WORK)obj_work;
        --gmsGmkBlandWork.broken_timer;
        if (gmsGmkBlandWork.broken_timer <= 0)
        {
            ObjObjectAction3dNNModelReleaseCopy(obj_work);
            ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_breakland_obj_3d_list[gmsGmkBlandWork.vect == 0 ? 2 : 3], gmsGmkBlandWork.gmk_work.obj_3d);
            ObjObjectAction3dNNMotionLoad(obj_work, 0, false, ObjDataGet(796), null, 0, null);
            if (gmsGmkBlandWork.vect == 32768)
            {
                ObjDrawObjectActionSet(obj_work, 0);
                obj_work.obj_3d.use_light_flag &= 4294967294U;
                obj_work.obj_3d.use_light_flag |= 4U;
                obj_work.obj_3d.use_light_flag |= 65536U;
            }
            else
            {
                ObjDrawObjectActionSet(obj_work, 1);
                obj_work.obj_3d.use_light_flag &= 4294967294U;
                obj_work.obj_3d.use_light_flag |= 2U;
                obj_work.obj_3d.use_light_flag |= 65536U;
            }
            obj_work.disp_flag &= 4294967279U;
            obj_work.disp_flag &= 4294967291U;
            gmsGmkBlandWork.gmk_work.ene_com.col_work.obj_col.obj = null;
            GmSoundPlaySE("BreakGround");
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBreakLandBroken);
        }
        else
        {
            gmsGmkBlandWork.quake_timer &= 3;
            obj_work.pos.y += tbl_breaklandquake[gmsGmkBlandWork.quake_timer] * 4096;
            ++gmsGmkBlandWork.quake_timer;
        }
    }

    private static void gmGmkBreakLandBroken(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 8U;
    }

    private static OBS_OBJECT_WORK gmGmkBreakLandInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      ushort vect)
    {
        GMS_GMK_BLAND_WORK work = (GMS_GMK_BLAND_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_BLAND_WORK(), "GMK_BREAK_LAND_MAIN");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_breakland_obj_3d_list[vect == 0 ? 0 : 1], gmsEnemy3DWork.obj_3d);
        if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
        {
            obj_work.obj_3d.use_light_flag &= 4294967294U;
            obj_work.obj_3d.use_light_flag |= 32U;
            obj_work.obj_3d.use_light_flag |= 65536U;
        }
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.diff_data = g_gm_breakland_col;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)gm_gmk_breakland_col_rect_tbl[0];
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)gm_gmk_breakland_col_rect_tbl[1];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = gm_gmk_breakland_col_rect_tbl[2];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = gm_gmk_breakland_col_rect_tbl[3];
        work.colrect_left = 278528;
        work.colrect_right = 524288;
        if (vect == 32768)
        {
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)(-gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x - gmsEnemy3DWork.ene_com.col_work.obj_col.width);
            work.colrect_right = -278528;
            work.colrect_left = -524288;
        }
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 268435456U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.dir = 0;
        if ((eve_rec.flag & 128) == 0)
            gmsEnemy3DWork.ene_com.col_work.obj_col.attr = 1;
        obj_work.pos.z = -4096;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 272629760U;
        obj_work.flag |= 2U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBreakLandStay);
        work.vect = vect;
        return obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkBreakLandRInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return gmGmkBreakLandInit(eve_rec, pos_x, pos_y, type, 0);
    }

    private static OBS_OBJECT_WORK GmGmkBreakLandLInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return gmGmkBreakLandInit(eve_rec, pos_x, pos_y, type, 32768);
    }

    private static void GmGmkBreakLandBuild()
    {
        gm_gmk_breakland_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(794)), readAMBFile(GmGameDatGetGimmickData(795)), 0U);
    }

    private static void GmGmkBreakLandFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(794));
        GmGameDBuildRegFlushModel(gm_gmk_breakland_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void GmGmkBreakLandSetLight()
    {
        NNS_RGBA col = new NNS_RGBA();
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.x = -0.5f;
        nnsVector.y = -0.05f;
        nnsVector.z = -1f;
        col.r = 0.65f;
        col.g = 0.65f;
        col.b = 0.65f;
        nnNormalizeVector(nnsVector, nnsVector);
        ObjDrawSetParallelLight(NNE_LIGHT_1, ref col, 1f, nnsVector);
        nnsVector.x = 0.4f;
        nnsVector.y = -0.05f;
        nnsVector.z = -1f;
        col.r = 0.65f;
        col.g = 0.65f;
        col.b = 0.65f;
        nnNormalizeVector(nnsVector, nnsVector);
        ObjDrawSetParallelLight(NNE_LIGHT_2, ref col, 1f, nnsVector);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }


}