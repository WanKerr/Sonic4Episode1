using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static AppMain.OBS_OBJECT_WORK GmGmkItemInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_ITEM");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        if (eve_rec.byte_param[1] != (byte)0)
            AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_item_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        else
            AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_item_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        gmsEnemy3DWork.obj_3d.material_cb_func = new AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE(AppMain.gmGmkItemMaterialCallback);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkItemOut);
        work.user_work = AppMain.gm_gmk_item_matrial_user_data_tbl[(int)AppMain.gmGmkItemConvEvtId(eve_rec.id) - 63];
        work.disp_flag |= 16777216U;
        AppMain.nnMakeUnitMatrix(work.obj_3d.user_obj_mtx_r);
        AppMain.nnTranslateMatrix(work.obj_3d.user_obj_mtx_r, work.obj_3d.user_obj_mtx_r, 0.0f, -1f / AppMain.FXM_FX32_TO_FLOAT(AppMain.g_obj.draw_scale.y), 0.0f);
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec1.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkItemBodyDefFunc);
        pRec1.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec1, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec1, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec1, (short)-30, (short)-50, (short)30, (short)10);
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        pRec2.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkItemDamageDefFunc);
        pRec2.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec2, (ushort)0, (short)0);
        AppMain.ObjRectWorkSet(pRec2, (short)-30, (short)-48, (short)30, (short)0);
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)40;
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)32;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)-20;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)-32;
        AppMain.ObjObjectFieldRectSet(work, (short)-8, (short)-8, (short)8, (short)0);
        work.disp_flag |= 4194304U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 512U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        if (eve_rec.byte_param[1] != (byte)0)
        {
            gmsEnemy3DWork.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
            work.flag |= 2U;
            work.move_flag &= 4294967151U;
            if (((int)eve_rec.flag & 1) != 0)
                work.move_flag |= 8448U;
            work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkItemFallCheckMain);
        }
        else
        {
            work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkItemMain);
            AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(work, 40);
            AppMain.GmComEfctAddDispOffsetF(efct_work, 0.0f, -12.5f, 10f);
            efct_work.efct_com.obj_work.flag |= 16U;
            work.user_flag_OBJECT = (object)efct_work;
            if (((int)eve_rec.flag & 1) != 0)
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
        AppMain.gm_gmk_item_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(789), AppMain.GmGameDatGetGimmickData(790), 0U);
    }

    public static void GmGmkItemFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(789));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_item_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkItemMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (gmsEnemy3DWork.ene_com.eve_rec.byte_param[1] != (byte)0)
        {
            AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_item_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
            obj_work.disp_flag |= 16777216U;
            AppMain.nnMakeUnitMatrix(obj_work.obj_3d.user_obj_mtx_r);
            AppMain.nnTranslateMatrix(obj_work.obj_3d.user_obj_mtx_r, obj_work.obj_3d.user_obj_mtx_r, 0.0f, -1f / AppMain.FXM_FX32_TO_FLOAT(AppMain.g_obj.draw_scale.y), 0.0f);
            if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2)
                AppMain.GmEfctZoneEsCreate(obj_work, 0, 1);
            else if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 2)
            {
                if (AppMain.GmMainIsWaterLevel() && obj_work.pos.y - 196608 >> 12 > (int)AppMain.g_gm_main_system.water_level)
                    AppMain.GmEfctZoneEsCreate(obj_work, 2, 9);
                else
                    AppMain.GmEfctCmnEsCreate(obj_work, 39);
            }
            else
                AppMain.GmEfctCmnEsCreate(obj_work, 39);
            AppMain.gmGmkItemCreatePopUpEffect(obj_work, (int)obj_work.user_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkItemEffectWatiMain);
            obj_work.user_timer = 245760;
            if (AppMain.gmGmkItemConvEvtId(gmsEnemy3DWork.ene_com.eve_rec.id) == (ushort)67)
                obj_work.user_timer = 1;
            obj_work.flag |= 16U;
            if (obj_work.user_flag != 0U)
                ((AppMain.GMS_EFFECT_3DES_WORK)obj_work.user_flag_OBJECT).efct_com.obj_work.flag |= 8U;
        }
        if (((int)obj_work.move_flag & 128) == 0 || ((int)obj_work.move_flag & 1) == 0)
            return;
        obj_work.move_flag &= 4294967151U;
        obj_work.spd.y = obj_work.spd_add.y = 0;
        obj_work.flag &= 4294967293U;
    }

    private static void gmGmkItemEffectWatiMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer <= 0)
        {
            AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
            if (gmsEnemyComWork.target_obj != null)
            {
                switch (AppMain.gmGmkItemConvEvtId(gmsEnemyComWork.eve_rec.id))
                {
                    case 63:
                        AppMain.GmPlayerItemHiSpeedSet((AppMain.GMS_PLAYER_WORK)gmsEnemyComWork.target_obj);
                        break;
                    case 64:
                        AppMain.GmPlayerItemInvincibleSet((AppMain.GMS_PLAYER_WORK)gmsEnemyComWork.target_obj);
                        break;
                    case 65:
                        AppMain.GmPlayerItemRing10Set((AppMain.GMS_PLAYER_WORK)gmsEnemyComWork.target_obj);
                        break;
                    case 66:
                        AppMain.GmPlayerItemBarrierSet((AppMain.GMS_PLAYER_WORK)gmsEnemyComWork.target_obj);
                        break;
                    case 67:
                        AppMain.GmPlayerItem1UPSet((AppMain.GMS_PLAYER_WORK)gmsEnemyComWork.target_obj);
                        break;
                }
            }
            gmsEnemyComWork.target_obj = (AppMain.OBS_OBJECT_WORK)null;
            obj_work.flag &= 4294967279U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkItemFallCheckMain);
        }
        AppMain.gmGmkItemFallCheckMain(obj_work);
    }

    private static void gmGmkItemFallCheckMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.move_flag & 128) == 0 || ((int)obj_work.move_flag & 1) == 0)
            return;
        obj_work.move_flag &= 4294967151U;
        obj_work.spd.y = obj_work.spd_add.y = 0;
    }

    private static ushort gmGmkItemConvEvtId(ushort eve_id)
    {
        if (AppMain.g_gs_main_sys_info.game_mode == 1 && eve_id == (ushort)67)
            eve_id = (ushort)65;
        return eve_id;
    }

    private static void gmGmkItemOut(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        AppMain.GMS_GMK_ITEM_MAT_CB_PARAM gmkItemMatCbParam = AppMain.amDrawAlloc_GMS_GMK_ITEM_MAT_CB_PARAM();
        gmkItemMatCbParam.draw_id = (uint)(ushort)obj_work.user_work;
        obj3d.material_cb_param = (object)gmkItemMatCbParam;
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void gmGmkItemBodyDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != (ushort)1 || parentObj2.obj_work.touch_obj != parentObj1.obj_work))
            return;
        short num1 = parentObj2.obj_work.field_rect[1];
        int num2 = (int)parentObj1.col_work.obj_col.height + (int)parentObj1.col_work.obj_col.ofst_y << 12;
        if (parentObj2.obj_work.pos.y + (int)num1 < parentObj1.obj_work.pos.y + num2 || parentObj2.obj_work.move.y > 0)
            return;
        parentObj1.obj_work.spd.y = -8192;
        parentObj1.obj_work.move_flag |= 144U;
        parentObj1.obj_work.move_flag &= 4294967294U;
        parentObj1.obj_work.flag |= 2U;
    }

    private static void gmGmkItemDamageDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != (ushort)1 || parentObj2.seq_state != 19 && parentObj2.seq_state != 10 && parentObj2.obj_work.touch_obj == parentObj1.obj_work || parentObj2.seq_state != 19 && parentObj2.seq_state != 10 && (parentObj2.obj_work.move.y <= 0 || parentObj2.obj_work.pos.x + ((int)parentObj2.obj_work.field_rect[2] << 12) < parentObj1.obj_work.pos.x - 81920 || parentObj2.obj_work.pos.x + ((int)parentObj2.obj_work.field_rect[0] << 12) > parentObj1.obj_work.pos.x + 81920))
            return;
        parentObj1.eve_rec.byte_param[1] = (byte)1;
        parentObj1.obj_work.flag |= 2U;
        parentObj1.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
        parentObj1.obj_work.move_flag |= 144U;
        parentObj1.obj_work.move_flag &= 4294967294U;
        parentObj1.target_obj = parentObj2.obj_work;
        AppMain.GmPlySeqAtkReactionInit(parentObj2);
        AppMain.GmSoundPlaySE("Enemy");
        AppMain.GMM_PAD_VIB_SMALL();
    }

    private static bool gmGmkItemMaterialCallback(AppMain.NNS_DRAWCALLBACK_VAL val, object param)
    {
        if (param != null)
        {
            AppMain.GMS_GMK_ITEM_MAT_CB_PARAM gmkItemMatCbParam = (AppMain.GMS_GMK_ITEM_MAT_CB_PARAM)param;
            uint materialUserData = AppMain.ObjDraw3DNNGetMaterialUserData(val);
            switch (materialUserData)
            {
                case 0:
                case 64:
                    return AppMain.nnPutMaterialCore(val) == 1;
                default:
                    if ((int)materialUserData != (int)gmkItemMatCbParam.draw_id)
                        break;
                    goto case 0;
            }
        }
        return false;
    }

    private static void gmGmkItemCreatePopUpEffect(AppMain.OBS_OBJECT_WORK parent_obj, int mat_id)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "GMK_ITEM_POP");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (AppMain.GMS_EFFECT_3DNN_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_item_obj_3d_list[2], gmsEffect3DnnWork.obj_3d);
        work.pos.x = parent_obj.pos.x;
        work.pos.y = parent_obj.pos.y - 86016;
        work.pos.z = -524288;
        gmsEffect3DnnWork.obj_3d.material_cb_func = new AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE(AppMain.gmGmkItemMaterialCallback);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkItemOut);
        work.user_work = (uint)mat_id;
        work.flag |= 18U;
        work.disp_flag |= 4194304U;
        work.move_flag &= 4294967167U;
        work.scale.x = work.scale.y = 6144;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkItemPopUpEffectMain);
        work.spd.y = -10240;
        work.spd_add.y = 320;
    }

    private static void gmGmkItemPopUpEffectMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.spd.y == 0)
        {
            obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
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