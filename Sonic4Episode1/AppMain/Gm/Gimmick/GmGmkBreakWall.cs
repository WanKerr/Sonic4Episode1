public partial class AppMain
{

    private static int GMM_GMK_TYPE_CHECK(int gmk)
    {
        return gmk >= 2 ? 1 : 0;
    }

    private static bool GMM_GMK_TYPE_IS_WALL(int gmk)
    {
        return GMM_GMK_TYPE_CHECK(gmk) == 0;
    }

    private static int GMM_GMK_TYPE_IS_VECT(int gmk)
    {
        return gmk != 0 ? 0 : 1;
    }

    private static void gmGmkBreakWallStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BWALL_WORK gmsGmkBwallWork = (GMS_GMK_BWALL_WORK)obj_work;
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][0];
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][1];
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.ofst_x = tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][2];
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.ofst_y = tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][3];
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.dir = 0;
        gmsGmkBwallWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsGmkBwallWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsGmkBwallWork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkBreakWallHit);
        pRec.ppHit = null;
        ObjRectAtkSet(pRec, 0, 0);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectWorkSet(pRec, tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][4], tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][5], tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][6], tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][7]);
        gmsGmkBwallWork.hitpass = 0;
        gmsGmkBwallWork.hitcheck = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBreakWallStay);
    }

    private static void gmGmkBreakWallStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BWALL_WORK gmsGmkBwallWork = (GMS_GMK_BWALL_WORK)obj_work;
        if (gmsGmkBwallWork.hitcheck < 0)
        {
            ushort vect = (gmsGmkBwallWork.hitcheck & 1) != 0 ? (ushort)0 : (ushort)32768;
            OBS_OBJECT_WORK obsObjectWork = null;
            obj_work.flag |= 10U;
            GmSoundPlaySE("BreakWall");
            GMM_PAD_VIB_SMALL();
            gmGmkBreakWall_CreateParts(obj_work, gmsGmkBwallWork.wall_type, gmsGmkBwallWork.obj_type, vect);
            if (gmk_bwall_effect_y > 196608)
            {
                while (gmk_bwall_effect_y > 65536)
                    gmk_bwall_effect_y -= 53248;
            }
            int num = obj_work.pos.z;
            switch (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id])
            {
                case 0:
                    obsObjectWork = (OBS_OBJECT_WORK)GmEfctZoneEsCreate(null, 0, 8);
                    break;
                case 1:
                    obsObjectWork = (OBS_OBJECT_WORK)GmEfctZoneEsCreate(null, 1, 1);
                    break;
                case 2:
                    obsObjectWork = (OBS_OBJECT_WORK)GmEfctZoneEsCreate(null, 2, 33);
                    if (g_gs_main_sys_info.stage_id == 9)
                    {
                        num = 655360;
                        obsObjectWork.obj_3des.command_state = 15U;
                        break;
                    }
                    break;
                case 3:
                    obsObjectWork = (OBS_OBJECT_WORK)GmEfctZoneEsCreate(null, 3, 3);
                    break;
            }
            if (obsObjectWork == null)
                return;
            obsObjectWork.pos.x = obj_work.pos.x;
            obsObjectWork.pos.y = obj_work.pos.y - gmk_bwall_effect_y;
            obsObjectWork.pos.z = num;
            gmk_bwall_effect_y += 126976;
            obsObjectWork.dir.z = vect;
            obsObjectWork.disp_flag &= 4294967039U;
        }
        else
        {
            if (gmsGmkBwallWork.hitpass == 0 && gmsGmkBwallWork.hitcheck != 0)
                gmGmkBreakWallStart(obj_work);
            gmsGmkBwallWork.hitpass = 0;
        }
    }

    private static void gmGmkBreakWallHit(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        GMS_PLAYER_WORK parentObj2 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 != g_gm_main_system.ply_work[0])
            return;
        GMS_GMK_BWALL_WORK gmsGmkBwallWork = (GMS_GMK_BWALL_WORK)parentObj1;
        OBS_RECT_WORK obsRectWork = gmsGmkBwallWork.gmk_work.ene_com.rect_work[2];
        switch (GMM_GMK_TYPE_CHECK(gmsGmkBwallWork.obj_type))
        {
            case 0:
                if (((int)parentObj2.player_flag & 262144) != 0)
                {
                    if (MTM_MATH_ABS(parentObj2.obj_work.spd_m) < parentObj2.spd3 && MTM_MATH_ABS(parentObj2.obj_work.spd.x) < parentObj2.spd3)
                        break;
                }
                else if (parentObj2.act_state == 30 || parentObj2.act_state == 29 || (parentObj2.act_state == 26 || parentObj2.act_state == 27))
                {
                    if ((gmsGmkBwallWork.broketype & (int)GMD_GMK_BWALL_HARD_SPIN_D) != 0)
                        break;
                    if (parentObj2.act_state != 26 && parentObj2.act_state != 27)
                    {
                        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.obj = null;
                        gmsGmkBwallWork.hitcheck = 1;
                        gmsGmkBwallWork.hitpass = 1;
                    }
                    else if (MTM_MATH_ABS(parentObj2.obj_work.spd_m) < g_gm_player_parameter[0].spd_max_spin / 4)
                        break;
                }
                else if (parentObj2.act_state == 39)
                {
                    if ((gmsGmkBwallWork.broketype & (int)GMD_GMK_BWALL_HARD_SPIN_J) != 0)
                        break;
                }
                else if (parentObj2.act_state != 22 && parentObj2.act_state != 22 && parentObj2.act_state != 21 || (gmsGmkBwallWork.broketype & (int)GMD_GMK_BWALL_HARD_DASH) != 0)
                    break;
                if (GMM_GMK_TYPE_IS_VECT(gmsGmkBwallWork.obj_type) != 0)
                {
                    if (parentObj1.pos.x >= parentObj2.obj_work.pos.x)
                    {
                        short num1 = (short)((parentObj1.pos.x >> 12) + obsRectWork.rect.left - match_rect.rect.right);
                        short num2 = (short)((parentObj2.obj_work.pos.x >> 12) - num1);
                        obsRectWork.rect.left += num2;
                        gmsGmkBwallWork.hitcheck = GMD_GMK_BWALL_HIT_LEFT;
                    }
                    else
                    {
                        short num = (short)((parentObj1.pos.x >> 12) + obsRectWork.rect.right - match_rect.rect.left - (parentObj2.obj_work.pos.x >> 12));
                        obsRectWork.rect.right -= num;
                        gmsGmkBwallWork.hitcheck = GMD_GMK_BWALL_HIT_RIGHT;
                    }
                    gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.obj = null;
                    gmsGmkBwallWork.hitpass = 1;
                    if (obsRectWork.rect.left < -16 && obsRectWork.rect.right > 16)
                        break;
                    gmsGmkBwallWork.hitcheck = (short)-gmsGmkBwallWork.hitcheck;
                    break;
                }
                if (parentObj1.pos.y >= parentObj2.obj_work.pos.y)
                {
                    short num1 = (short)((parentObj1.pos.y >> 12) + obsRectWork.rect.top - match_rect.rect.bottom);
                    short num2 = (short)((parentObj2.obj_work.pos.y >> 12) - num1);
                    obsRectWork.rect.top += num2;
                    gmsGmkBwallWork.hitcheck = GMD_GMK_BFLOOR_HIT_TOP;
                }
                else
                {
                    short num = (short)((short)((parentObj1.pos.y >> 12) + obsRectWork.rect.bottom - match_rect.rect.top) - (short)(parentObj2.obj_work.pos.y >> 12));
                    obsRectWork.rect.bottom -= num;
                    gmsGmkBwallWork.hitcheck = GMD_GMK_BFLOOR_HIT_BOTTOM;
                }
                gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.obj = null;
                gmsGmkBwallWork.hitpass = 1;
                if (obsRectWork.rect.top < -16 && obsRectWork.rect.bottom > 16)
                    break;
                gmsGmkBwallWork.hitcheck = (short)-gmsGmkBwallWork.hitcheck;
                break;
            case 1:
                if ((gmsGmkBwallWork.broketype & (int)GMD_GMK_BFLOOR_HARD_CANNON) != 0 && parentObj2.act_state != 67 || (gmsGmkBwallWork.broketype & (int)GMD_GMK_BFLOOR_HARD_CANNON) != 0 && parentObj2.act_state == 67 && parentObj2.obj_work.spd.y > 0 || (parentObj2.act_state != 39 && (gmsGmkBwallWork.broketype & (int)GMD_GMK_BFLOOR_HARD_CANNON) == 0 || parentObj2.obj_work.pos.y <= parentObj1.pos.y && parentObj2.obj_work.spd.y <= 0 || parentObj2.obj_work.pos.y >= parentObj1.pos.y && parentObj2.obj_work.spd.y >= 0))
                    break;
                if (parentObj1.pos.y >= parentObj2.obj_work.pos.y)
                {
                    short num1 = (short)((parentObj1.pos.y >> 12) + obsRectWork.rect.top - match_rect.rect.bottom);
                    short num2 = (short)((parentObj2.obj_work.pos.y >> 12) - num1);
                    obsRectWork.rect.top += num2;
                    gmsGmkBwallWork.hitcheck = GMD_GMK_BFLOOR_HIT_TOP;
                }
                else
                {
                    short num = (short)((short)((parentObj1.pos.y >> 12) + obsRectWork.rect.bottom - match_rect.rect.top) - (short)(parentObj2.obj_work.pos.y >> 12));
                    obsRectWork.rect.bottom -= num;
                    gmsGmkBwallWork.hitcheck = GMD_GMK_BFLOOR_HIT_BOTTOM;
                }
                gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.obj = null;
                gmsGmkBwallWork.hitpass = 1;
                if (obsRectWork.rect.top < -16 && obsRectWork.rect.bottom > 16)
                    break;
                gmsGmkBwallWork.hitcheck = (short)-gmsGmkBwallWork.hitcheck;
                break;
        }
    }

    private static void gmGmkBreakLandParts_Main(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_BWALL_PARTS gmsGmkBwallParts = (GMS_GMK_BWALL_PARTS)obj_work;
        if (gmsGmkBwallParts.vect > 32768)
        {
            obj_work.dir.x += 1024;
            obj_work.dir.y += 768;
        }
        else
        {
            obj_work.dir.x -= 1024;
            obj_work.dir.y -= 768;
        }
        --gmsGmkBwallParts.falltimer;
        if (gmsGmkBwallParts.falltimer > 0)
            return;
        obj_work.flag |= 8U;
    }

    private static void gmGmkBreakWall_CreateParts(
      OBS_OBJECT_WORK parent_obj,
      int type,
      int obj_type,
      ushort vect)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        ushort num1 = (ushort)g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        ushort num2 = (ushort)(mtMathRand() % 8192U);
        for (int index = 0; index < tbl_gmk_bwall_parts[num1][type].num; ++index)
        {
            ushort[] numArray = tbl_gmk_bwall_parts[num1][type]._params[index];
            GMS_GMK_BWALL_PARTS work = (GMS_GMK_BWALL_PARTS)GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_BWALL_PARTS(), null, 0, "BreakWall_Parts");
            OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
            ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_breakwall_obj_3d_list[numArray[0]], work.eff_work.obj_3d);
            ushort num3 = numArray[5];
            ushort num4 = numArray[3];
            ushort num5 = (ushort)(numArray[4] + num2 / 2U);
            if (GMM_GMK_TYPE_IS_VECT(obj_type) != 0)
            {
                ushort num6 = num4 < 32768 ? (ushort)(num4 - (uint)num2) : (ushort)(num4 + (uint)num2);
                if (vect == 0)
                {
                    obj_work.pos.x = parent_obj.pos.x + (short)numArray[1] * 4096;
                    obj_work.dir.z = 0;
                }
                else
                {
                    obj_work.pos.x = parent_obj.pos.x - (short)numArray[1] * 4096;
                    obj_work.dir.z = 32768;
                    num6 = (ushort)(32768U - num6);
                }
                work.vect = num6;
                obj_work.pos.y = parent_obj.pos.y + (short)numArray[2] * 4096;
                ushort num7 = (ushort)(numArray[4] + num2 / 2U);
                int a = mtMathCos(num6) * num3;
                obj_work.spd.y = mtMathSin(num6) * num3;
                obj_work.spd.x = mtMathCos(num7) * a >> 12;
                obj_work.spd.z = -(mtMathSin(num7) * MTM_MATH_ABS(a) >> 12);
                obj_work.pos.z = parent_obj.pos.z + mtMathSin(num7) * 8;
                obj_work.spd.x += gmsPlayerWork.obj_work.move.x >> 1;
            }
            else
            {
                ushort num6 = num4 < 49152 ? (ushort)(num4 - (uint)num2) : (ushort)(num4 + (uint)num2);
                obj_work.pos.x = parent_obj.pos.x + (short)numArray[1] * 4096;
                if (vect == 0)
                {
                    obj_work.pos.y = parent_obj.pos.y + (short)numArray[2] * 4096;
                    obj_work.dir.z = 0;
                }
                else
                {
                    obj_work.pos.y = parent_obj.pos.y - (short)numArray[2] * 4096;
                    obj_work.dir.z = 32768;
                    num6 = (ushort)(65536U - num6);
                }
                work.vect = num6;
                int a = mtMathCos(num6) * num3;
                obj_work.spd.y = mtMathSin(num6) * num3;
                obj_work.spd.x = mtMathCos(num5) * a >> 12;
                obj_work.spd.z = -(mtMathSin(num5) * MTM_MATH_ABS(a) >> 12);
                obj_work.pos.z = parent_obj.pos.z + mtMathSin(num5) * 8;
            }
            obj_work.spd_add.y = 384;
            obj_work.dir.x = 0;
            obj_work.dir.z = 0;
            obj_work.move_flag |= 256U;
            obj_work.disp_flag |= 4194304U;
            obj_work.disp_flag &= 4294967039U;
            obj_work.flag |= 2U;
            work.falltimer = obj_work.spd.y >= 0 ? (short)120 : (short)90;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBreakLandParts_Main);
            obj_work.obj_3d.use_light_flag &= 4294967294U;
            obj_work.obj_3d.use_light_flag |= 2U;
            obj_work.obj_3d.use_light_flag |= 65536U;
        }
    }

    private static OBS_OBJECT_WORK gmGmkBreakWallInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      int wall)
    {
        GMS_GMK_BWALL_WORK work = (GMS_GMK_BWALL_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_BWALL_WORK(), "GMK_BREAK_LAND_MAIN");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ushort num = tbl_breakwall_mdl[g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id]][wall];
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_breakwall_obj_3d_list[num], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -131072;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        work.broketype = (ushort)(eve_rec.flag & 7U);
        work.obj_type = eve_rec.id != 272 ? 0 : 1;
        work.wall_type = wall;
        if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
        {
            gmsEnemy3DWork.obj_3d.use_light_flag &= 4294967294U;
            gmsEnemy3DWork.obj_3d.use_light_flag |= 2U;
            gmsEnemy3DWork.obj_3d.use_light_flag |= 98304U;
        }
        else
        {
            gmsEnemy3DWork.obj_3d.use_light_flag &= 4294967294U;
            gmsEnemy3DWork.obj_3d.use_light_flag |= 2U;
            gmsEnemy3DWork.obj_3d.use_light_flag |= 65536U;
        }
        return obj_work;
    }

    private static OBS_OBJECT_WORK GmGmkBreakWall_L1Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_BWALL_WORK gmsGmkBwallWork = (GMS_GMK_BWALL_WORK)gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 0);
        gmGmkBreakWallStart((OBS_OBJECT_WORK)gmsGmkBwallWork);
        return (OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static OBS_OBJECT_WORK GmGmkBreakWall_L2Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_BWALL_WORK gmsGmkBwallWork = (GMS_GMK_BWALL_WORK)gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 1);
        gmGmkBreakWallStart((OBS_OBJECT_WORK)gmsGmkBwallWork);
        return (OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static OBS_OBJECT_WORK GmGmkBreakWall_R1Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_BWALL_WORK gmsGmkBwallWork = (GMS_GMK_BWALL_WORK)gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 2);
        gmGmkBreakWallStart((OBS_OBJECT_WORK)gmsGmkBwallWork);
        return (OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static OBS_OBJECT_WORK GmGmkBreakWall_R2Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_BWALL_WORK gmsGmkBwallWork = (GMS_GMK_BWALL_WORK)gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 3);
        gmGmkBreakWallStart((OBS_OBJECT_WORK)gmsGmkBwallWork);
        return (OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static OBS_OBJECT_WORK GmGmkBreakWall_C1Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_BWALL_WORK gmsGmkBwallWork = (GMS_GMK_BWALL_WORK)gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 4);
        gmGmkBreakWallStart((OBS_OBJECT_WORK)gmsGmkBwallWork);
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.disp_flag |= 4194304U;
        return (OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static OBS_OBJECT_WORK GmGmkBreakWall_C2Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_BWALL_WORK gmsGmkBwallWork = (GMS_GMK_BWALL_WORK)gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 5);
        gmGmkBreakWallStart((OBS_OBJECT_WORK)gmsGmkBwallWork);
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.disp_flag |= 4194304U;
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.obj_3d.drawflag |= 32U;
        return (OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static OBS_OBJECT_WORK GmGmkBreakWall_C1_H_Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_BWALL_WORK gmsGmkBwallWork = (GMS_GMK_BWALL_WORK)gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 7);
        gmGmkBreakWallStart((OBS_OBJECT_WORK)gmsGmkBwallWork);
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.disp_flag |= 4194304U;
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.disp_flag &= 4294967039U;
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.dir.z = 49152;
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.flag |= 32U;
        return (OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

}