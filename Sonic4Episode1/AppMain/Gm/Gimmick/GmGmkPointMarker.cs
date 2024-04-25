public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkPointMarkerInit(
         GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        if (g_gs_main_sys_info.game_mode == 1)
        {
            eve_rec.pos_x = byte.MaxValue;
            return null;
        }
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_PMARKER_WORK(), "GMK_POINT_MARKER");
        GMS_GMK_PMARKER_WORK gmsGmkPmarkerWork = (GMS_GMK_PMARKER_WORK)work;
        work.pos.y += 4096;
        work.pos.z = g_gs_main_sys_info.stage_id != 9 ? -65536 : -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        gmsGmkPmarkerWork.marker_prty = (ushort)eve_rec.left;
        gmGmkPointMarkerStart(work);
        return work;
    }

    public static void GmGmkPointMarkerBuild()
    {
        gm_gmk_pmarker_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(838), GmGameDatGetGimmickData(839), 0U);
    }

    public static void GmGmkPointMarkerFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(838));
        GmGameDBuildRegFlushModel(gm_gmk_pmarker_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkPointMarkerHit(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_GMK_PMARKER_WORK parentObj1 = (GMS_GMK_PMARKER_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        parentObj1.markerdist = parentObj1.OBJWORK.pos.x - parentObj2.obj_work.pos.x;
        if (parentObj1.markerdist <= 16384 && parentObj1.markerdist >= -16384 || parentObj1.markerdist < 16384 && parentObj1.markerdistlast >= 16384 || parentObj1.markerdist > -16384 && parentObj1.markerdistlast <= -16384)
        {
            if (g_gm_main_system.marker_pri < parentObj1.marker_prty)
            {
                GmPlayerSetMarkerPoint(parentObj2, parentObj1.OBJWORK.pos.x, parentObj1.OBJWORK.pos.y);
                g_gm_main_system.marker_pri = parentObj1.marker_prty;
                parentObj1.marker_prty = 0;
                parentObj1.hitcounter = 2;
                SaveState.saveCurrentState(1);
            }
            parentObj1.OBJWORK.flag |= 2U;
        }
        else
            mine_rect.flag &= 4294573823U;
    }

    private static void gmGmkPointMarkerStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PMARKER_WORK gmsGmkPmarkerWork = (GMS_GMK_PMARKER_WORK)obj_work;
        ObjDrawObjectActionSet(obj_work, 0);
        gmsGmkPmarkerWork.markerdist = 0;
        gmsGmkPmarkerWork.hitcounter = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPointMarkerStay_100);
    }

    private static void gmGmkPointMarkerStay_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PMARKER_WORK gmsGmkPmarkerWork = (GMS_GMK_PMARKER_WORK)obj_work;
        gmsGmkPmarkerWork.markerdistlast = gmsGmkPmarkerWork.markerdist;
        gmsGmkPmarkerWork.markerdist = 0;
        if (gmsGmkPmarkerWork.hitcounter > 0)
        {
            GmSoundPlaySE("Marker");
            gmGmkPointMarkerStay_200(obj_work);
        }
        else
        {
            if (g_gm_main_system.marker_pri < gmsGmkPmarkerWork.marker_prty)
                return;
            gmsGmkPmarkerWork.marker_prty = 0;
            gmGmkPointMarkerStay_400(obj_work);
        }
    }

    private static void gmGmkPointMarkerStay_200(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawObjectActionSet(obj_work, 1);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPointMarkerStay_210);
    }

    private static void gmGmkPointMarkerStay_210(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PMARKER_WORK gmsGmkPmarkerWork = (GMS_GMK_PMARKER_WORK)obj_work;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        --gmsGmkPmarkerWork.hitcounter;
        if (gmsGmkPmarkerWork.hitcounter == 0)
        {
            ObjDrawObjectActionSet(obj_work, 0);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkPointMarkerStay_300);
        }
        else
            gmGmkPointMarkerStay_200(obj_work);
    }

    private static void gmGmkPointMarkerStay_300(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        gmGmkPointMarkerStay_400(obj_work);
    }

    private static void gmGmkPointMarkerStay_400(OBS_OBJECT_WORK obj_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctCmnEsCreate(obj_work, 49);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 34f, 0.0f);
        efct_3des.efct_com.obj_work.pos.z = g_gs_main_sys_info.stage_id != 9 ? obj_work.pos.z + 65536 : obj_work.pos.z + 40960;
        ObjAction3dNNMaterialMotionLoad(obj_work.obj_3d, 0, null, null, 0, (AMS_AMB_HEADER)ObjDataGet(841).pData);
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.obj_3d.mat_speed = 1f;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = null;
    }

    private static void gmGmkPointMarkerStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_PMARKER_WORK gmsGmkPmarkerWork = (GMS_GMK_PMARKER_WORK)obj_work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_pmarker_obj_3d_list[0], gmsGmkPmarkerWork.OBJ_3D);
        ObjObjectAction3dNNMotionLoad(obj_work, 0, false, ObjDataGet(840), null, 0, null);
        ObjDrawObjectActionSet(obj_work, 0);
        if (g_gm_main_system.marker_pri < gmsGmkPmarkerWork.marker_prty)
        {
            gmsGmkPmarkerWork.COMWORK.rect_work[0].flag &= 4294967291U;
            gmsGmkPmarkerWork.COMWORK.rect_work[1].flag &= 4294967291U;
            OBS_RECT_WORK pRec = gmsGmkPmarkerWork.COMWORK.rect_work[2];
            pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkPointMarkerHit);
            pRec.ppHit = null;
            ObjRectAtkSet(pRec, 0, 0);
            ObjRectDefSet(pRec, 65534, 0);
            ObjRectWorkSet(pRec, -16, -64, 16, 0);
        }
        else
            gmsGmkPmarkerWork.marker_prty = 0;
        gmGmkPointMarkerStay(obj_work);
    }


}