public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkItemInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_ITEM");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        if (eve_rec.byte_param[1] != 0)
            ObjObjectCopyAction3dNNModel(work, gm_gmk_item_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        else
            ObjObjectCopyAction3dNNModel(work, gm_gmk_item_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        gmsEnemy3DWork.obj_3d.material_cb_func = new MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE(gmGmkItemMaterialCallback);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkItemOut);
        work.user_work = gm_gmk_item_matrial_user_data_tbl[gmGmkItemConvEvtId(eve_rec.id) - 63];
        work.disp_flag |= 16777216U;
        nnMakeUnitMatrix(work.obj_3d.user_obj_mtx_r);
        nnTranslateMatrix(work.obj_3d.user_obj_mtx_r, work.obj_3d.user_obj_mtx_r, 0.0f, -1f / FXM_FX32_TO_FLOAT(g_obj.draw_scale.y), 0.0f);
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec1.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkItemBodyDefFunc);
        pRec1.ppHit = null;
        ObjRectAtkSet(pRec1, 0, 0);
        ObjRectDefSet(pRec1, 65534, 0);
        ObjRectWorkSet(pRec1, -30, -50, 30, 10);
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        pRec2.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkItemDamageDefFunc);
        pRec2.ppHit = null;
        ObjRectAtkSet(pRec2, 0, 0);
        ObjRectWorkSet(pRec2, -30, -48, 30, 0);
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = 40;
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = 32;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = -20;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = -32;
        ObjObjectFieldRectSet(work, -8, -8, 8, 0);
        work.disp_flag |= 4194304U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 512U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        if (eve_rec.byte_param[1] != 0)
        {
            gmsEnemy3DWork.ene_com.col_work.obj_col.obj = null;
            work.flag |= 2U;
            work.move_flag &= 4294967151U;
            if ((eve_rec.flag & 1) != 0)
                work.move_flag |= 8448U;
            work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkItemFallCheckMain);
        }
        else
        {
            work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkItemMain);
            GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(work, 40);
            GmComEfctAddDispOffsetF(efct_work, 0.0f, -12.5f, 10f);
            efct_work.efct_com.obj_work.flag |= 16U;
            work.user_flag_OBJECT = efct_work;
            if ((eve_rec.flag & 1) != 0)
            {
                work.move_flag &= 4294967151U;
                work.move_flag |= 8448U;
                gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
            }
        }
        work.flag |= 1073741824U;
        return work;
    }

    public static void GmGmkItemBuild()
    {
        gm_gmk_item_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(789), GmGameDatGetGimmickData(790), 0U);
    }

    public static void GmGmkItemFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(789));
        GmGameDBuildRegFlushModel(gm_gmk_item_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkItemMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (gmsEnemy3DWork.ene_com.eve_rec.byte_param[1] != 0)
        {
            ObjObjectAction3dNNModelReleaseCopy(obj_work);
            ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_item_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
            obj_work.disp_flag |= 16777216U;
            nnMakeUnitMatrix(obj_work.obj_3d.user_obj_mtx_r);
            nnTranslateMatrix(obj_work.obj_3d.user_obj_mtx_r, obj_work.obj_3d.user_obj_mtx_r, 0.0f, -1f / FXM_FX32_TO_FLOAT(g_obj.draw_scale.y), 0.0f);
            if (g_gs_main_sys_info.stage_id == 2)
                GmEfctZoneEsCreate(obj_work, 0, 1);
            else if (GMM_MAIN_GET_ZONE_TYPE() == 2)
            {
                if (GmMainIsWaterLevel() && obj_work.pos.y - 196608 >> 12 > g_gm_main_system.water_level)
                    GmEfctZoneEsCreate(obj_work, 2, 9);
                else
                    GmEfctCmnEsCreate(obj_work, 39);
            }
            else
                GmEfctCmnEsCreate(obj_work, 39);
            gmGmkItemCreatePopUpEffect(obj_work, (int)obj_work.user_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkItemEffectWatiMain);
            obj_work.user_timer = 245760;
            if (gmGmkItemConvEvtId(gmsEnemy3DWork.ene_com.eve_rec.id) == 67)
                obj_work.user_timer = 1;
            obj_work.flag |= 16U;
            if (obj_work.user_flag != 0U)
                ((GMS_EFFECT_3DES_WORK)obj_work.user_flag_OBJECT).efct_com.obj_work.flag |= 8U;
        }
        if (((int)obj_work.move_flag & 128) == 0 || ((int)obj_work.move_flag & 1) == 0)
            return;
        obj_work.move_flag &= 4294967151U;
        obj_work.spd.y = obj_work.spd_add.y = 0;
        obj_work.flag &= 4294967293U;
    }

    private static void gmGmkItemEffectWatiMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer <= 0)
        {
            GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
            if (gmsEnemyComWork.target_obj != null)
            {
                switch (gmGmkItemConvEvtId(gmsEnemyComWork.eve_rec.id))
                {
                    case 63:
                        GmPlayerItemHiSpeedSet((GMS_PLAYER_WORK)gmsEnemyComWork.target_obj);
                        break;
                    case 64:
                        GmPlayerItemInvincibleSet((GMS_PLAYER_WORK)gmsEnemyComWork.target_obj);
                        break;
                    case 65:
                        GmPlayerItemRing10Set((GMS_PLAYER_WORK)gmsEnemyComWork.target_obj);
                        break;
                    case 66:
                        GmPlayerItemBarrierSet((GMS_PLAYER_WORK)gmsEnemyComWork.target_obj);
                        break;
                    case 67:
                        GmPlayerItem1UPSet((GMS_PLAYER_WORK)gmsEnemyComWork.target_obj);
                        break;
                }
            }
            gmsEnemyComWork.target_obj = null;
            obj_work.flag &= 4294967279U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkItemFallCheckMain);
        }
        gmGmkItemFallCheckMain(obj_work);
    }

    private static void gmGmkItemFallCheckMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.move_flag & 128) == 0 || ((int)obj_work.move_flag & 1) == 0)
            return;
        obj_work.move_flag &= 4294967151U;
        obj_work.spd.y = obj_work.spd_add.y = 0;
    }

    private static ushort gmGmkItemConvEvtId(ushort eve_id)
    {
        if (g_gs_main_sys_info.game_mode == 1 && eve_id == 67)
            eve_id = 65;
        return eve_id;
    }

    private static void gmGmkItemOut(OBS_OBJECT_WORK obj_work)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        GMS_GMK_ITEM_MAT_CB_PARAM gmkItemMatCbParam = amDrawAlloc_GMS_GMK_ITEM_MAT_CB_PARAM();
        gmkItemMatCbParam.draw_id = (ushort)obj_work.user_work;
        obj3d.material_cb_param = gmkItemMatCbParam;
        ObjDrawActionSummary(obj_work);
    }

    private static void gmGmkItemBodyDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != 1 || parentObj2.obj_work.touch_obj != parentObj1.obj_work))
            return;
        short num1 = parentObj2.obj_work.field_rect[1];
        int num2 = parentObj1.col_work.obj_col.height + parentObj1.col_work.obj_col.ofst_y << 12;
        if (parentObj2.obj_work.pos.y + num1 < parentObj1.obj_work.pos.y + num2 || parentObj2.obj_work.move.y > 0)
            return;
        parentObj1.obj_work.spd.y = -8192;
        parentObj1.obj_work.move_flag |= 144U;
        parentObj1.obj_work.move_flag &= 4294967294U;
        parentObj1.obj_work.flag |= 2U;
    }

    private static void gmGmkItemDamageDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_ENEMY_COM_WORK parentObj1 = (GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != 1 || parentObj2.seq_state != GME_PLY_SEQ_STATE_HOMING && parentObj2.seq_state != GME_PLY_SEQ_STATE_SPIN && parentObj2.obj_work.touch_obj == parentObj1.obj_work || parentObj2.seq_state != GME_PLY_SEQ_STATE_HOMING && parentObj2.seq_state != GME_PLY_SEQ_STATE_SPIN && (parentObj2.obj_work.move.y <= 0 || parentObj2.obj_work.pos.x + (parentObj2.obj_work.field_rect[2] << 12) < parentObj1.obj_work.pos.x - 81920 || parentObj2.obj_work.pos.x + (parentObj2.obj_work.field_rect[0] << 12) > parentObj1.obj_work.pos.x + 81920))
            return;
        parentObj1.eve_rec.byte_param[1] = 1;
        parentObj1.obj_work.flag |= 2U;
        parentObj1.col_work.obj_col.obj = null;
        parentObj1.obj_work.move_flag |= 144U;
        parentObj1.obj_work.move_flag &= 4294967294U;
        parentObj1.target_obj = parentObj2.obj_work;
        GmPlySeqAtkReactionInit(parentObj2);
        GmSoundPlaySE("Enemy");
        GMM_PAD_VIB_SMALL();
    }

    private static bool gmGmkItemMaterialCallback(NNS_DRAWCALLBACK_VAL val, object param)
    {
        if (param != null)
        {
            GMS_GMK_ITEM_MAT_CB_PARAM gmkItemMatCbParam = (GMS_GMK_ITEM_MAT_CB_PARAM)param;
            uint materialUserData = ObjDraw3DNNGetMaterialUserData(val);
            switch (materialUserData)
            {
                case 0:
                case 64:
                    return nnPutMaterialCore(val) == 1;
                default:
                    if ((int)materialUserData != (int)gmkItemMatCbParam.draw_id)
                        break;
                    goto case 0;
            }
        }
        return false;
    }

    private static void gmGmkItemCreatePopUpEffect(OBS_OBJECT_WORK parent_obj, int mat_id)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DNN_WORK(), null, 0, "GMK_ITEM_POP");
        GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (GMS_EFFECT_3DNN_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_item_obj_3d_list[2], gmsEffect3DnnWork.obj_3d);
        work.pos.x = parent_obj.pos.x;
        work.pos.y = parent_obj.pos.y - 86016;
        work.pos.z = -524288;
        gmsEffect3DnnWork.obj_3d.material_cb_func = new MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE(gmGmkItemMaterialCallback);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkItemOut);
        work.user_work = (uint)mat_id;
        work.flag |= 18U;
        work.disp_flag |= 4194304U;
        work.move_flag &= 4294967167U;
        work.scale.x = work.scale.y = 6144;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkItemPopUpEffectMain);
        work.spd.y = -10240;
        work.spd_add.y = 320;
    }

    private static void gmGmkItemPopUpEffectMain(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.spd.y == 0)
        {
            obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
            if (obj_work.user_timer > 0)
                return;
            obj_work.flag |= 4U;
        }
        else
        {
            if (obj_work.spd.y + obj_work.spd_add.y < 0)
                return;
            obj_work.spd.y = obj_work.spd_add.y = 0;
            obj_work.user_timer = 122880;
            obj_work.move_flag |= 8192U;
        }
    }
}