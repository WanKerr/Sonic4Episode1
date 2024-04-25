public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkBreakObjInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_BOBJ_WORK work = (GMS_GMK_BOBJ_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_BOBJ_WORK(), "GMK_BREAK_LAND_MAIN");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_breakobj_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -4096;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        work.zone_type = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        gmGmkBreakObjStart(obj_work);
        return obj_work;
    }

    public static void GmGmkBreakObjBuild()
    {
        gm_gmk_breakobj_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(799)), readAMBFile(GmGameDatGetGimmickData(800)), 0U);
    }

    public static void GmGmkBreakObjFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(799));
        GmGameDBuildRegFlushModel(gm_gmk_breakobj_obj_3d_list, amsAmbHeader.file_num);
    }

    public static void gmGmkBreakObjStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOBJ_WORK gmsGmkBobjWork = (GMS_GMK_BOBJ_WORK)obj_work;
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][0];
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][1];
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.ofst_x = tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][2];
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.ofst_y = tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][3];
        gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.dir = 0;
        gmsGmkBobjWork.breakrect_left = (short)(gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.ofst_x - 4);
        gmsGmkBobjWork.breakrect_right = (short)(gmsGmkBobjWork.breakrect_left + gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.width + 4);
        gmsGmkBobjWork.breakrect_top = (short)(gmsGmkBobjWork.gmk_work.ene_com.col_work.obj_col.ofst_y - 4);
        gmsGmkBobjWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec1 = gmsGmkBobjWork.gmk_work.ene_com.rect_work[0];
        pRec1.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkBreakObjHit);
        pRec1.ppHit = null;
        ObjRectAtkSet(pRec1, 0, 0);
        ObjRectDefSet(pRec1, 65534, 0);
        ObjRectWorkSet(pRec1, tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][4], tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][5], tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][6], tbl_gm_gmk_bobj_col_rect[gmsGmkBobjWork.zone_type][7]);
        OBS_RECT_WORK pRec2 = gmsGmkBobjWork.gmk_work.ene_com.rect_work[2];
        pRec2.ppDef = null;
        pRec2.ppHit = null;
        ObjRectAtkSet(pRec2, 0, 0);
        ObjRectDefSet(pRec2, 65534, 0);
        ObjRectWorkSet(pRec2, gmsGmkBobjWork.breakrect_left, gmsGmkBobjWork.breakrect_top, gmsGmkBobjWork.breakrect_right, 0);
        gmsGmkBobjWork.hitpass = false;
        gmsGmkBobjWork.hitcheck = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBreakObjStay);
    }

    public static void gmGmkBreakObjStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOBJ_WORK gmsGmkBobjWork = (GMS_GMK_BOBJ_WORK)obj_work;
        if (!gmsGmkBobjWork.hitpass && gmsGmkBobjWork.hitcheck != 0)
            gmGmkBreakObjStart(obj_work);
        gmsGmkBobjWork.hitpass = false;
    }

    private static void gmGmkBreakObjHit(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 != g_gm_main_system.ply_work[0])
            return;
        GMS_GMK_BOBJ_WORK gmsGmkBobjWork = (GMS_GMK_BOBJ_WORK)parentObj1;
        OBS_RECT_WORK obsRectWork = gmsGmkBobjWork.gmk_work.ene_com.obj_work.rect_work[0];
        if (parentObj2.act_state != 39 && parentObj2.act_state != 31 && ((int)parentObj2.player_flag & 262144) == 0)
            return;
        int num1;
        short num2 = (short)(num1 = 0);
        short num3 = (short)num1;
        short num4 = (short)num1;
        ushort vect = 0;
        if (parentObj1.pos.x >= parentObj2.obj_work.pos.x)
        {
            short num5 = (short)((parentObj1.pos.x >> 12) + obsRectWork.rect.left - match_rect.rect.right);
            num4 = (short)((parentObj2.obj_work.pos.x >> 12) - num5);
            gmsGmkBobjWork.hitcheck = 1;
        }
        else
        {
            num3 = (short)((parentObj1.pos.x >> 12) + obsRectWork.rect.right - match_rect.rect.left - (parentObj2.obj_work.pos.x >> 12));
            gmsGmkBobjWork.hitcheck = 1;
            vect = 32768;
        }
        if (parentObj1.pos.y >= parentObj2.obj_work.pos.y)
        {
            short num5 = (short)((parentObj1.pos.y >> 12) + obsRectWork.rect.top - match_rect.rect.bottom);
            num2 = (short)((parentObj2.obj_work.pos.y >> 12) - num5);
            gmsGmkBobjWork.hitcheck = 1;
        }
        if (obsRectWork.rect.right > gmsGmkBobjWork.breakrect_right)
        {
            obsRectWork.rect.right -= num3;
            if (obsRectWork.rect.right < gmsGmkBobjWork.breakrect_right)
                obsRectWork.rect.right = gmsGmkBobjWork.breakrect_right;
        }
        if (obsRectWork.rect.left < gmsGmkBobjWork.breakrect_left)
        {
            obsRectWork.rect.left += num4;
            if (obsRectWork.rect.left > gmsGmkBobjWork.breakrect_left)
                obsRectWork.rect.left = gmsGmkBobjWork.breakrect_left;
        }
        if (obsRectWork.rect.top < gmsGmkBobjWork.breakrect_top)
        {
            obsRectWork.rect.top += num2;
            if (obsRectWork.rect.top > gmsGmkBobjWork.breakrect_top)
                obsRectWork.rect.top = gmsGmkBobjWork.breakrect_top;
        }
        gmsGmkBobjWork.hitpass = true;
        short num6 = (short)(parentObj2.obj_work.spd.x >> 12);
        short num7 = (short)(parentObj2.obj_work.spd.y >> 12);
        if (obsRectWork.rect.right + num6 > gmsGmkBobjWork.breakrect_right && obsRectWork.rect.left + num6 < gmsGmkBobjWork.breakrect_left || obsRectWork.rect.top + num7 < gmsGmkBobjWork.breakrect_top)
            return;
        ushort num8 = tbl_gmk_breakobj_effect[gmsGmkBobjWork.zone_type];
        switch (gmsGmkBobjWork.zone_type)
        {
            case 0:
                if (g_gs_main_sys_info.stage_id == 2)
                {
                    num8 = 3;
                    break;
                }
                break;
            case 2:
                if (g_gm_main_system.water_level != ushort.MaxValue && g_gm_main_system.water_level < (parentObj1.pos.y >> 12) + gmsGmkBobjWork.breakrect_top)
                {
                    num8 = 11;
                    break;
                }
                break;
        }
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctZoneEsCreate(null, gmsGmkBobjWork.zone_type, num8);
        obsObjectWork.pos.x = parentObj1.pos.x;
        obsObjectWork.pos.y = parentObj1.pos.y;
        obsObjectWork.pos.z = parentObj1.pos.z;
        gmGmkBreakObj_CreateParts(parentObj1, gmsGmkBobjWork.zone_type, vect);
        gmsGmkBobjWork.gmk_work.ene_com.obj_work.col_work.obj_col.obj = null;
        GmEnemyDefaultDefFunc(mine_rect, match_rect);
        GmSoundPlaySE("BreakOBJ");
        GmPlayerAddScore(parentObj2, 100, parentObj1.pos.x, parentObj1.pos.y);
    }

    public static void gmGmkBreakObj_CreateParts(
      OBS_OBJECT_WORK parent_obj,
      int type,
      ushort vect)
    {
        int index1 = 0;
        for (int index2 = 0; index2 < GMS_GMK_BOBJ_PARTS_PARAM_TABLE[type].num; ++index2)
        {
            ushort[] numArray = GMS_GMK_BOBJ_PARTS_PARAM_TABLE[type].Params[index1];
            GMS_GMK_BOBJ_PARTS work = (GMS_GMK_BOBJ_PARTS)GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_BOBJ_PARTS(), null, 0, "BreakObj_Parts");
            OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
            ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_breakobj_obj_3d_list[numArray[0]], work.eff_work.obj_3d);
            ((NNS_MATERIAL_GLES11_DESC)obj_work.obj_3d._object.pMatPtrList[0].pMaterial).fFlag = 2U;
            obj_work.pos.x = parent_obj.pos.x + (short)numArray[1] * 4096;
            obj_work.pos.y = parent_obj.pos.y - (short)numArray[2] * 4096;
            obj_work.pos.z = parent_obj.pos.z + 1;
            vect = numArray[3];
            int num = -numArray[4];
            obj_work.spd.x = mtMathCos(vect) * num >> 12;
            obj_work.spd.y = -(mtMathSin(vect) * num >> 12);
            obj_work.spd_add.y = 1024;
            obj_work.dir.x = 0;
            obj_work.dir.y = 0;
            obj_work.dir.z = vect;
            obj_work.move_flag |= 256U;
            obj_work.disp_flag |= 4194304U;
            obj_work.disp_flag &= 4294967039U;
            obj_work.flag |= 2U;
            work.falltimer = 60;
            ++index1;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBreakObjParts_Main);
        }
    }

    public static void gmGmkBreakObjParts_Main(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BOBJ_PARTS gmsGmkBobjParts = (GMS_GMK_BOBJ_PARTS)obj_work;
        obj_work.dir.z += 512;
        --gmsGmkBobjParts.falltimer;
        if (gmsGmkBobjParts.falltimer > 0)
            return;
        obj_work.flag |= 8U;
    }

}