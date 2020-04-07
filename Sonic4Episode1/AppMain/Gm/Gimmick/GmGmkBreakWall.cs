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

    private static int GMM_GMK_TYPE_CHECK(int gmk)
    {
        return gmk >= 2 ? 1 : 0;
    }

    private static bool GMM_GMK_TYPE_IS_WALL(int gmk)
    {
        return AppMain.GMM_GMK_TYPE_CHECK(gmk) == 0;
    }

    private static int GMM_GMK_TYPE_IS_VECT(int gmk)
    {
        return gmk != 0 ? 0 : 1;
    }

    private static void gmGmkBreakWallStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BWALL_WORK gmsGmkBwallWork = (AppMain.GMS_GMK_BWALL_WORK)obj_work;
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)AppMain.tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][0];
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)AppMain.tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][1];
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.ofst_x = AppMain.tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][2];
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.ofst_y = AppMain.tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][3];
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.dir = (ushort)0;
        gmsGmkBwallWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsGmkBwallWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsGmkBwallWork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkBreakWallHit);
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec, AppMain.tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][4], AppMain.tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][5], AppMain.tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][6], AppMain.tbl_gm_gmk_bwall_col_rect[gmsGmkBwallWork.obj_type][7]);
        gmsGmkBwallWork.hitpass = 0;
        gmsGmkBwallWork.hitcheck = (short)0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBreakWallStay);
    }

    private static void gmGmkBreakWallStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BWALL_WORK gmsGmkBwallWork = (AppMain.GMS_GMK_BWALL_WORK)obj_work;
        if (gmsGmkBwallWork.hitcheck < (short)0)
        {
            ushort vect = ((int)gmsGmkBwallWork.hitcheck & 1) != 0 ? (ushort)0 : (ushort)32768;
            AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)null;
            obj_work.flag |= 10U;
            AppMain.GmSoundPlaySE("BreakWall");
            AppMain.GMM_PAD_VIB_SMALL();
            AppMain.gmGmkBreakWall_CreateParts(obj_work, gmsGmkBwallWork.wall_type, gmsGmkBwallWork.obj_type, vect);
            if (AppMain.gmk_bwall_effect_y > 196608)
            {
                while (AppMain.gmk_bwall_effect_y > 65536)
                    AppMain.gmk_bwall_effect_y -= 53248;
            }
            int num = obj_work.pos.z;
            switch (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id])
            {
                case 0:
                    obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, 0, 8);
                    break;
                case 1:
                    obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, 1, 1);
                    break;
                case 2:
                    obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, 2, 33);
                    if (AppMain.g_gs_main_sys_info.stage_id == (ushort)9)
                    {
                        num = 655360;
                        obsObjectWork.obj_3des.command_state = 15U;
                        break;
                    }
                    break;
                case 3:
                    obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, 3, 3);
                    break;
            }
            if (obsObjectWork == null)
                return;
            obsObjectWork.pos.x = obj_work.pos.x;
            obsObjectWork.pos.y = obj_work.pos.y - AppMain.gmk_bwall_effect_y;
            obsObjectWork.pos.z = num;
            AppMain.gmk_bwall_effect_y += 126976;
            obsObjectWork.dir.z = vect;
            obsObjectWork.disp_flag &= 4294967039U;
        }
        else
        {
            if (gmsGmkBwallWork.hitpass == 0 && gmsGmkBwallWork.hitcheck != (short)0)
                AppMain.gmGmkBreakWallStart(obj_work);
            gmsGmkBwallWork.hitpass = 0;
        }
    }

    private static void gmGmkBreakWallHit(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 != AppMain.g_gm_main_system.ply_work[0])
            return;
        AppMain.GMS_GMK_BWALL_WORK gmsGmkBwallWork = (AppMain.GMS_GMK_BWALL_WORK)parentObj1;
        AppMain.OBS_RECT_WORK obsRectWork = gmsGmkBwallWork.gmk_work.ene_com.rect_work[2];
        switch (AppMain.GMM_GMK_TYPE_CHECK(gmsGmkBwallWork.obj_type))
        {
            case 0:
                if (((int)parentObj2.player_flag & 262144) != 0)
                {
                    if (AppMain.MTM_MATH_ABS(parentObj2.obj_work.spd_m) < parentObj2.spd3 && AppMain.MTM_MATH_ABS(parentObj2.obj_work.spd.x) < parentObj2.spd3)
                        break;
                }
                else if (parentObj2.act_state == 30 || parentObj2.act_state == 29 || (parentObj2.act_state == 26 || parentObj2.act_state == 27))
                {
                    if (((int)gmsGmkBwallWork.broketype & (int)AppMain.GMD_GMK_BWALL_HARD_SPIN_D) != 0)
                        break;
                    if (parentObj2.act_state != 26 && parentObj2.act_state != 27)
                    {
                        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
                        gmsGmkBwallWork.hitcheck = (short)1;
                        gmsGmkBwallWork.hitpass = 1;
                    }
                    else if (AppMain.MTM_MATH_ABS(parentObj2.obj_work.spd_m) < AppMain.g_gm_player_parameter[0].spd_max_spin / 4)
                        break;
                }
                else if (parentObj2.act_state == 39)
                {
                    if (((int)gmsGmkBwallWork.broketype & (int)AppMain.GMD_GMK_BWALL_HARD_SPIN_J) != 0)
                        break;
                }
                else if (parentObj2.act_state != 22 && parentObj2.act_state != 22 && parentObj2.act_state != 21 || ((int)gmsGmkBwallWork.broketype & (int)AppMain.GMD_GMK_BWALL_HARD_DASH) != 0)
                    break;
                if (AppMain.GMM_GMK_TYPE_IS_VECT(gmsGmkBwallWork.obj_type) != 0)
                {
                    if (parentObj1.pos.x >= parentObj2.obj_work.pos.x)
                    {
                        short num1 = (short)((parentObj1.pos.x >> 12) + (int)obsRectWork.rect.left - (int)match_rect.rect.right);
                        short num2 = (short)((parentObj2.obj_work.pos.x >> 12) - (int)num1);
                        obsRectWork.rect.left += num2;
                        gmsGmkBwallWork.hitcheck = AppMain.GMD_GMK_BWALL_HIT_LEFT;
                    }
                    else
                    {
                        short num = (short)((parentObj1.pos.x >> 12) + (int)obsRectWork.rect.right - (int)match_rect.rect.left - (parentObj2.obj_work.pos.x >> 12));
                        obsRectWork.rect.right -= num;
                        gmsGmkBwallWork.hitcheck = AppMain.GMD_GMK_BWALL_HIT_RIGHT;
                    }
                    gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
                    gmsGmkBwallWork.hitpass = 1;
                    if (obsRectWork.rect.left < (short)-16 && obsRectWork.rect.right > (short)16)
                        break;
                    gmsGmkBwallWork.hitcheck = (short)-gmsGmkBwallWork.hitcheck;
                    break;
                }
                if (parentObj1.pos.y >= parentObj2.obj_work.pos.y)
                {
                    short num1 = (short)((parentObj1.pos.y >> 12) + (int)obsRectWork.rect.top - (int)match_rect.rect.bottom);
                    short num2 = (short)((parentObj2.obj_work.pos.y >> 12) - (int)num1);
                    obsRectWork.rect.top += num2;
                    gmsGmkBwallWork.hitcheck = AppMain.GMD_GMK_BFLOOR_HIT_TOP;
                }
                else
                {
                    short num = (short)((int)(short)((parentObj1.pos.y >> 12) + (int)obsRectWork.rect.bottom - (int)match_rect.rect.top) - (int)(short)(parentObj2.obj_work.pos.y >> 12));
                    obsRectWork.rect.bottom -= num;
                    gmsGmkBwallWork.hitcheck = AppMain.GMD_GMK_BFLOOR_HIT_BOTTOM;
                }
                gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
                gmsGmkBwallWork.hitpass = 1;
                if (obsRectWork.rect.top < (short)-16 && obsRectWork.rect.bottom > (short)16)
                    break;
                gmsGmkBwallWork.hitcheck = (short)-gmsGmkBwallWork.hitcheck;
                break;
            case 1:
                if (((int)gmsGmkBwallWork.broketype & (int)AppMain.GMD_GMK_BFLOOR_HARD_CANNON) != 0 && parentObj2.act_state != 67 || ((int)gmsGmkBwallWork.broketype & (int)AppMain.GMD_GMK_BFLOOR_HARD_CANNON) != 0 && parentObj2.act_state == 67 && parentObj2.obj_work.spd.y > 0 || (parentObj2.act_state != 39 && ((int)gmsGmkBwallWork.broketype & (int)AppMain.GMD_GMK_BFLOOR_HARD_CANNON) == 0 || parentObj2.obj_work.pos.y <= parentObj1.pos.y && parentObj2.obj_work.spd.y <= 0 || parentObj2.obj_work.pos.y >= parentObj1.pos.y && parentObj2.obj_work.spd.y >= 0))
                    break;
                if (parentObj1.pos.y >= parentObj2.obj_work.pos.y)
                {
                    short num1 = (short)((parentObj1.pos.y >> 12) + (int)obsRectWork.rect.top - (int)match_rect.rect.bottom);
                    short num2 = (short)((parentObj2.obj_work.pos.y >> 12) - (int)num1);
                    obsRectWork.rect.top += num2;
                    gmsGmkBwallWork.hitcheck = AppMain.GMD_GMK_BFLOOR_HIT_TOP;
                }
                else
                {
                    short num = (short)((int)(short)((parentObj1.pos.y >> 12) + (int)obsRectWork.rect.bottom - (int)match_rect.rect.top) - (int)(short)(parentObj2.obj_work.pos.y >> 12));
                    obsRectWork.rect.bottom -= num;
                    gmsGmkBwallWork.hitcheck = AppMain.GMD_GMK_BFLOOR_HIT_BOTTOM;
                }
                gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
                gmsGmkBwallWork.hitpass = 1;
                if (obsRectWork.rect.top < (short)-16 && obsRectWork.rect.bottom > (short)16)
                    break;
                gmsGmkBwallWork.hitcheck = (short)-gmsGmkBwallWork.hitcheck;
                break;
        }
    }

    private static void gmGmkBreakLandParts_Main(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BWALL_PARTS gmsGmkBwallParts = (AppMain.GMS_GMK_BWALL_PARTS)obj_work;
        if (gmsGmkBwallParts.vect > (ushort)32768)
        {
            obj_work.dir.x += (ushort)1024;
            obj_work.dir.y += (ushort)768;
        }
        else
        {
            obj_work.dir.x -= (ushort)1024;
            obj_work.dir.y -= (ushort)768;
        }
        --gmsGmkBwallParts.falltimer;
        if (gmsGmkBwallParts.falltimer > (short)0)
            return;
        obj_work.flag |= 8U;
    }

    private static void gmGmkBreakWall_CreateParts(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int type,
      int obj_type,
      ushort vect)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        ushort num1 = (ushort)AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        ushort num2 = (ushort)((uint)AppMain.mtMathRand() % 8192U);
        for (int index = 0; index < (int)AppMain.tbl_gmk_bwall_parts[(int)num1][type].num; ++index)
        {
            ushort[] numArray = AppMain.tbl_gmk_bwall_parts[(int)num1][type]._params[index];
            AppMain.GMS_GMK_BWALL_PARTS work = (AppMain.GMS_GMK_BWALL_PARTS)AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BWALL_PARTS()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "BreakWall_Parts");
            AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_breakwall_obj_3d_list[(int)numArray[0]], work.eff_work.obj_3d);
            ushort num3 = numArray[5];
            ushort num4 = numArray[3];
            ushort num5 = (ushort)((uint)numArray[4] + (uint)num2 / 2U);
            if (AppMain.GMM_GMK_TYPE_IS_VECT(obj_type) != 0)
            {
                ushort num6 = num4 < (ushort)32768 ? (ushort)((uint)num4 - (uint)num2) : (ushort)((uint)num4 + (uint)num2);
                if (vect == (ushort)0)
                {
                    obj_work.pos.x = parent_obj.pos.x + (int)(short)numArray[1] * 4096;
                    obj_work.dir.z = (ushort)0;
                }
                else
                {
                    obj_work.pos.x = parent_obj.pos.x - (int)(short)numArray[1] * 4096;
                    obj_work.dir.z = (ushort)32768;
                    num6 = (ushort)(32768U - (uint)num6);
                }
                work.vect = num6;
                obj_work.pos.y = parent_obj.pos.y + (int)(short)numArray[2] * 4096;
                ushort num7 = (ushort)((uint)numArray[4] + (uint)num2 / 2U);
                int a = AppMain.mtMathCos((int)num6) * (int)num3;
                obj_work.spd.y = AppMain.mtMathSin((int)num6) * (int)num3;
                obj_work.spd.x = AppMain.mtMathCos((int)num7) * a >> 12;
                obj_work.spd.z = -(AppMain.mtMathSin((int)num7) * AppMain.MTM_MATH_ABS(a) >> 12);
                obj_work.pos.z = parent_obj.pos.z + AppMain.mtMathSin((int)num7) * 8;
                obj_work.spd.x += gmsPlayerWork.obj_work.move.x >> 1;
            }
            else
            {
                ushort num6 = num4 < (ushort)49152 ? (ushort)((uint)num4 - (uint)num2) : (ushort)((uint)num4 + (uint)num2);
                obj_work.pos.x = parent_obj.pos.x + (int)(short)numArray[1] * 4096;
                if (vect == (ushort)0)
                {
                    obj_work.pos.y = parent_obj.pos.y + (int)(short)numArray[2] * 4096;
                    obj_work.dir.z = (ushort)0;
                }
                else
                {
                    obj_work.pos.y = parent_obj.pos.y - (int)(short)numArray[2] * 4096;
                    obj_work.dir.z = (ushort)32768;
                    num6 = (ushort)(65536U - (uint)num6);
                }
                work.vect = num6;
                int a = AppMain.mtMathCos((int)num6) * (int)num3;
                obj_work.spd.y = AppMain.mtMathSin((int)num6) * (int)num3;
                obj_work.spd.x = AppMain.mtMathCos((int)num5) * a >> 12;
                obj_work.spd.z = -(AppMain.mtMathSin((int)num5) * AppMain.MTM_MATH_ABS(a) >> 12);
                obj_work.pos.z = parent_obj.pos.z + AppMain.mtMathSin((int)num5) * 8;
            }
            obj_work.spd_add.y = 384;
            obj_work.dir.x = (ushort)0;
            obj_work.dir.z = (ushort)0;
            obj_work.move_flag |= 256U;
            obj_work.disp_flag |= 4194304U;
            obj_work.disp_flag &= 4294967039U;
            obj_work.flag |= 2U;
            work.falltimer = obj_work.spd.y >= 0 ? (short)120 : (short)90;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBreakLandParts_Main);
            obj_work.obj_3d.use_light_flag &= 4294967294U;
            obj_work.obj_3d.use_light_flag |= 2U;
            obj_work.obj_3d.use_light_flag |= 65536U;
        }
    }

    private static AppMain.OBS_OBJECT_WORK gmGmkBreakWallInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      int wall)
    {
        AppMain.GMS_GMK_BWALL_WORK work = (AppMain.GMS_GMK_BWALL_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BWALL_WORK()), "GMK_BREAK_LAND_MAIN");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        ushort num = AppMain.tbl_breakwall_mdl[AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id]][wall];
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_breakwall_obj_3d_list[(int)num], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -131072;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        work.broketype = (ushort)((uint)eve_rec.flag & 7U);
        work.obj_type = eve_rec.id != (ushort)272 ? 0 : 1;
        work.wall_type = wall;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
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

    private static AppMain.OBS_OBJECT_WORK GmGmkBreakWall_L1Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_BWALL_WORK gmsGmkBwallWork = (AppMain.GMS_GMK_BWALL_WORK)AppMain.gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 0);
        AppMain.gmGmkBreakWallStart((AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork);
        return (AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBreakWall_L2Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_BWALL_WORK gmsGmkBwallWork = (AppMain.GMS_GMK_BWALL_WORK)AppMain.gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 1);
        AppMain.gmGmkBreakWallStart((AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork);
        return (AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBreakWall_R1Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_BWALL_WORK gmsGmkBwallWork = (AppMain.GMS_GMK_BWALL_WORK)AppMain.gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 2);
        AppMain.gmGmkBreakWallStart((AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork);
        return (AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBreakWall_R2Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_BWALL_WORK gmsGmkBwallWork = (AppMain.GMS_GMK_BWALL_WORK)AppMain.gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 3);
        AppMain.gmGmkBreakWallStart((AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork);
        return (AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBreakWall_C1Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_BWALL_WORK gmsGmkBwallWork = (AppMain.GMS_GMK_BWALL_WORK)AppMain.gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 4);
        AppMain.gmGmkBreakWallStart((AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork);
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.disp_flag |= 4194304U;
        return (AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBreakWall_C2Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_BWALL_WORK gmsGmkBwallWork = (AppMain.GMS_GMK_BWALL_WORK)AppMain.gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 5);
        AppMain.gmGmkBreakWallStart((AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork);
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.disp_flag |= 4194304U;
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.obj_3d.drawflag |= 32U;
        return (AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBreakWall_C1_H_Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_BWALL_WORK gmsGmkBwallWork = (AppMain.GMS_GMK_BWALL_WORK)AppMain.gmGmkBreakWallInit(eve_rec, pos_x, pos_y, type, 7);
        AppMain.gmGmkBreakWallStart((AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork);
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.disp_flag |= 4194304U;
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.disp_flag &= 4294967039U;
        gmsGmkBwallWork.gmk_work.ene_com.obj_work.dir.z = (ushort)49152;
        gmsGmkBwallWork.gmk_work.ene_com.col_work.obj_col.flag |= 32U;
        return (AppMain.OBS_OBJECT_WORK)gmsGmkBwallWork;
    }

}