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
    private static AppMain.OBS_OBJECT_WORK GmGmkBreakObjInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_BOBJ_WORK work = (AppMain.GMS_GMK_BOBJ_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BOBJ_WORK()), "GMK_BREAK_LAND_MAIN");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_breakobj_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -4096;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        work.zone_type = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.gmGmkBreakObjStart(obj_work);
        return obj_work;
    }

    public static void GmGmkBreakObjBuild()
    {
        AppMain.gm_gmk_breakobj_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(799)), AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(800)), 0U);
    }

    public static void GmGmkBreakObjFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(799));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_breakobj_obj_3d_list, amsAmbHeader.file_num);
    }

    public static void gmGmkBreakObjStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOBJ_WORK gmsGmkBobjWork = (AppMain.GMS_GMK_BOBJ_WORK)obj_work;
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)AppMain.tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][0];
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)AppMain.tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][1];
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.ofst_x = AppMain.tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][2];
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.ofst_y = AppMain.tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][3];
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.dir = (ushort)0;
        gmsGmkBobjWork.breakrect_left = (short)((int)gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.ofst_x - 4);
        gmsGmkBobjWork.breakrect_right = (short)((int)gmsGmkBobjWork.breakrect_left + (int)gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.width + 4);
        gmsGmkBobjWork.breakrect_top = (short)((int)gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.ofst_y - 4);
        gmsGmkBobjWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec1 = gmsGmkBobjWork.gmk_work.ene_com.rect_work[0];
        pRec1.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkBreakObjHit);
        pRec1.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec1, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec1, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec1, AppMain.tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][4], AppMain.tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][5], AppMain.tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][6], AppMain.tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][7]);
        AppMain.OBS_RECT_WORK pRec2 = gmsGmkBobjWork.gmk_work.ene_com.rect_work[2];
        pRec2.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
        pRec2.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec2, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec2, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec2, gmsGmkBobjWork.breakrect_left, gmsGmkBobjWork.breakrect_top, gmsGmkBobjWork.breakrect_right, (short)0);
        gmsGmkBobjWork.hitpass = false;
        gmsGmkBobjWork.hitcheck = (short)0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBreakObjStay);
    }

    public static void gmGmkBreakObjStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOBJ_WORK gmsGmkBobjWork = (AppMain.GMS_GMK_BOBJ_WORK)obj_work;
        if (!gmsGmkBobjWork.hitpass && gmsGmkBobjWork.hitcheck != (short)0)
            AppMain.gmGmkBreakObjStart(obj_work);
        gmsGmkBobjWork.hitpass = false;
    }

    private static void gmGmkBreakObjHit(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 != AppMain.g_gm_main_system.ply_work[0])
            return;
        AppMain.GMS_GMK_BOBJ_WORK gmsGmkBobjWork = (AppMain.GMS_GMK_BOBJ_WORK)parentObj1;
        AppMain.OBS_RECT_WORK obsRectWork = gmsGmkBobjWork.gmk_work.ene_com.obj_work.rect_work[0];
        if (parentObj2.act_state != 39 && parentObj2.act_state != 31 && ((int)parentObj2.player_flag & 262144) == 0)
            return;
        int num1;
        short num2 = (short)(num1 = 0);
        short num3 = (short)num1;
        short num4 = (short)num1;
        ushort vect = 0;
        if (parentObj1.pos.x >= parentObj2.obj_work.pos.x)
        {
            short num5 = (short)((parentObj1.pos.x >> 12) + (int)obsRectWork.rect.left - (int)match_rect.rect.right);
            num4 = (short)((parentObj2.obj_work.pos.x >> 12) - (int)num5);
            gmsGmkBobjWork.hitcheck = (short)1;
        }
        else
        {
            num3 = (short)((parentObj1.pos.x >> 12) + (int)obsRectWork.rect.right - (int)match_rect.rect.left - (parentObj2.obj_work.pos.x >> 12));
            gmsGmkBobjWork.hitcheck = (short)1;
            vect = (ushort)32768;
        }
        if (parentObj1.pos.y >= parentObj2.obj_work.pos.y)
        {
            short num5 = (short)((parentObj1.pos.y >> 12) + (int)obsRectWork.rect.top - (int)match_rect.rect.bottom);
            num2 = (short)((parentObj2.obj_work.pos.y >> 12) - (int)num5);
            gmsGmkBobjWork.hitcheck = (short)1;
        }
        if ((int)obsRectWork.rect.right > (int)gmsGmkBobjWork.breakrect_right)
        {
            obsRectWork.rect.right -= num3;
            if ((int)obsRectWork.rect.right < (int)gmsGmkBobjWork.breakrect_right)
                obsRectWork.rect.right = gmsGmkBobjWork.breakrect_right;
        }
        if ((int)obsRectWork.rect.left < (int)gmsGmkBobjWork.breakrect_left)
        {
            obsRectWork.rect.left += num4;
            if ((int)obsRectWork.rect.left > (int)gmsGmkBobjWork.breakrect_left)
                obsRectWork.rect.left = gmsGmkBobjWork.breakrect_left;
        }
        if ((int)obsRectWork.rect.top < (int)gmsGmkBobjWork.breakrect_top)
        {
            obsRectWork.rect.top += num2;
            if ((int)obsRectWork.rect.top > (int)gmsGmkBobjWork.breakrect_top)
                obsRectWork.rect.top = gmsGmkBobjWork.breakrect_top;
        }
        gmsGmkBobjWork.hitpass = true;
        short num6 = (short)(parentObj2.obj_work.spd.x >> 12);
        short num7 = (short)(parentObj2.obj_work.spd.y >> 12);
        if ((int)obsRectWork.rect.right + (int)num6 > (int)gmsGmkBobjWork.breakrect_right && (int)obsRectWork.rect.left + (int)num6 < (int)gmsGmkBobjWork.breakrect_left || (int)obsRectWork.rect.top + (int)num7 < (int)gmsGmkBobjWork.breakrect_top)
            return;
        ushort num8 = AppMain.tbl_gmk_breakobj_effect[gmsGmkBobjWork.zone_type];
        switch (gmsGmkBobjWork.zone_type)
        {
            case 0:
                if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2)
                {
                    num8 = (ushort)3;
                    break;
                }
                break;
            case 2:
                if (AppMain.g_gm_main_system.water_level != ushort.MaxValue && (int)AppMain.g_gm_main_system.water_level < (parentObj1.pos.y >> 12) + (int)gmsGmkBobjWork.breakrect_top)
                {
                    num8 = (ushort)11;
                    break;
                }
                break;
        }
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, gmsGmkBobjWork.zone_type, (int)num8);
        obsObjectWork.pos.x = parentObj1.pos.x;
        obsObjectWork.pos.y = parentObj1.pos.y;
        obsObjectWork.pos.z = parentObj1.pos.z;
        AppMain.gmGmkBreakObj_CreateParts(parentObj1, gmsGmkBobjWork.zone_type, vect);
        gmsGmkBobjWork.gmk_work.ene_com.obj_work.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.GmEnemyDefaultDefFunc(mine_rect, match_rect);
        AppMain.GmSoundPlaySE("BreakOBJ");
        AppMain.GmPlayerAddScore(parentObj2, 100, parentObj1.pos.x, parentObj1.pos.y);
    }

    public static void gmGmkBreakObj_CreateParts(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int type,
      ushort vect)
    {
        int index1 = 0;
        for (int index2 = 0; index2 < (int)AppMain.GMS_GMK_BOBJ_PARTS_PARAM_TABLE[type].num; ++index2)
        {
            ushort[] numArray = AppMain.GMS_GMK_BOBJ_PARTS_PARAM_TABLE[type].Params[index1];
            AppMain.GMS_GMK_BOBJ_PARTS work = (AppMain.GMS_GMK_BOBJ_PARTS)AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BOBJ_PARTS()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "BreakObj_Parts");
            AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_breakobj_obj_3d_list[(int)numArray[0]], work.eff_work.obj_3d);
            ((AppMain.NNS_MATERIAL_GLES11_DESC)obj_work.obj_3d._object.pMatPtrList[0].pMaterial).fFlag = 2U;
            obj_work.pos.x = parent_obj.pos.x + (int)(short)numArray[1] * 4096;
            obj_work.pos.y = parent_obj.pos.y - (int)(short)numArray[2] * 4096;
            obj_work.pos.z = parent_obj.pos.z + 1;
            vect = numArray[3];
            int num = (int)-numArray[4];
            obj_work.spd.x = AppMain.mtMathCos((int)vect) * num >> 12;
            obj_work.spd.y = -(AppMain.mtMathSin((int)vect) * num >> 12);
            obj_work.spd_add.y = 1024;
            obj_work.dir.x = (ushort)0;
            obj_work.dir.y = (ushort)0;
            obj_work.dir.z = vect;
            obj_work.move_flag |= 256U;
            obj_work.disp_flag |= 4194304U;
            obj_work.disp_flag &= 4294967039U;
            obj_work.flag |= 2U;
            work.falltimer = (short)60;
            ++index1;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBreakObjParts_Main);
        }
    }

    public static void gmGmkBreakObjParts_Main(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOBJ_PARTS gmsGmkBobjParts = (AppMain.GMS_GMK_BOBJ_PARTS)obj_work;
        obj_work.dir.z += (ushort)512;
        --gmsGmkBobjParts.falltimer;
        if (gmsGmkBobjParts.falltimer > (short)0)
            return;
        obj_work.flag |= 8U;
    }

}