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
    public static void GmGmkLandBuild()
    {
        int index = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.gm_gmk_land_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(AppMain.gm_gmk_land_obj_data[index][0]), AppMain.GmGameDatGetGimmickData(AppMain.gm_gmk_land_obj_data[index][1]), 0U);
        if (index != 2)
            return;
        AppMain.gm_gmk_land_3_obj_tvx_list = AppMain.GmGameDatGetGimmickData(810);
    }

    public static void GmGmkLandFlush()
    {
        int index = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(AppMain.gm_gmk_land_obj_data[index][0]);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_land_obj_3d_list, gimmickData.file_num);
        AppMain.gm_gmk_land_3_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkLandInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int index1 = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK rideWork = AppMain.GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_LAND");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)rideWork;
        int index2;
        ushort num;
        if (eve_rec.id == (ushort)82)
        {
            index2 = 1;
            num = (ushort)1;
        }
        else if (eve_rec.id == (ushort)83)
        {
            index2 = 2;
            num = (ushort)2;
        }
        else
        {
            index2 = 0;
            num = (ushort)0;
        }
        int index3 = AppMain.gm_gmk_land_mdl_data[index1][(int)num];
        AppMain.ObjObjectCopyAction3dNNModel(rideWork, AppMain.gm_gmk_land_obj_3d_list[index3], gmsEnemy3DWork.obj_3d);
        switch (index1)
        {
            case 1:
                int id = index3;
                int index4 = index3;
                AppMain.ObjObjectAction3dNNMotionLoad(rideWork, 0, false, AppMain.ObjDataGet(805), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
                AppMain.ObjDrawObjectActionSet(rideWork, id);
                AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, index4, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(806).pData);
                AppMain.ObjDrawObjectActionSet3DNNMaterial(rideWork, 0);
                rideWork.disp_flag |= 4U;
                break;
            case 4:
                int index5 = index3;
                AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, index5, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(815).pData);
                AppMain.ObjDrawObjectActionSet3DNNMaterial(rideWork, 0);
                rideWork.disp_flag |= 16U;
                ((AppMain.NNS_MOTION_KEY_Class5[])rideWork.obj_3d.motion.mmtn[0].pSubmotion[0].pKeyList)[0].Value.y = 1f;
                break;
            default:
                if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
                {
                    gmsEnemy3DWork.obj_3d.use_light_flag &= 4294967294U;
                    gmsEnemy3DWork.obj_3d.use_light_flag |= 2U;
                    gmsEnemy3DWork.obj_3d.use_light_flag |= 65536U;
                    break;
                }
                break;
        }
        if (index1 == 2)
            rideWork.ppOut = index3 != 0 ? new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkLand3TvxRDrawFunc) : new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkLand3TvxDrawFunc);
        rideWork.pos.z = -131072;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = rideWork;
        gmsEnemy3DWork.ene_com.col_work.obj_col.diff_data = AppMain.g_gm_default_col;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        if (((int)eve_rec.flag & 128) == 0 && eve_rec.id != (ushort)83)
            gmsEnemy3DWork.ene_com.col_work.obj_col.attr = (ushort)1;
        switch (AppMain.gm_gmk_land_col_type_tbl[index2])
        {
            case 1:
                gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)80;
                gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)24;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)((int)-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)-17;
                if (((int)gmsEnemy3DWork.ene_com.col_work.obj_col.attr & 1) != 0)
                {
                    gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)8;
                    ++gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y;
                    break;
                }
                break;
            case 2:
                if (index1 != 2)
                {
                    gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)64;
                    gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)64;
                    gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)((int)-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
                    gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)-31;
                }
                else
                {
                    gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)24;
                    gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)32;
                    gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)((int)-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
                    gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)-15;
                }
                rideWork.field_rect[0] = gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x;
                rideWork.field_rect[1] = gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y;
                rideWork.field_rect[2] = (short)((int)gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x + (int)gmsEnemy3DWork.ene_com.col_work.obj_col.width);
                rideWork.field_rect[3] = (short)((int)gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y + (int)gmsEnemy3DWork.ene_com.col_work.obj_col.height);
                break;
            default:
                gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)48;
                gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)24;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)((int)-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)-17;
                if (((int)gmsEnemy3DWork.ene_com.col_work.obj_col.attr & 1) != 0)
                {
                    gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)8;
                    ++gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y;
                    break;
                }
                break;
        }
        rideWork.move_flag |= 8448U;
        rideWork.disp_flag |= 4194304U;
        rideWork.flag |= 2U;
        AppMain.gmGmkLandMoveInit(rideWork);
        return rideWork;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkZ3LandPulleyInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK rideWork = AppMain.GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_LAND_PULLEY");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)rideWork;
        AppMain.ObjObjectCopyAction3dNNModel(rideWork, AppMain.gm_gmk_land_obj_3d_list[2], gmsEnemy3DWork.obj_3d);
        rideWork.pos.z = -163840;
        rideWork.move_flag |= 8448U;
        rideWork.disp_flag |= 4194304U;
        rideWork.flag |= 2U;
        rideWork.user_work = (uint)(short)((int)eve_rec.left << 8) / 10U;
        rideWork.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkLand3TvxPulleyDrawFunc);
        return rideWork;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkZ3LandRopeInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK rideWork = AppMain.GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_LAND_ROPE");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)rideWork;
        AppMain.ObjObjectCopyAction3dNNModel(rideWork, AppMain.gm_gmk_land_obj_3d_list[3], gmsEnemy3DWork.obj_3d);
        rideWork.pos.z = -196608;
        rideWork.move_flag |= 8448U;
        rideWork.disp_flag |= 4194304U;
        rideWork.flag |= 2U;
        if (eve_rec.id == (ushort)275)
            rideWork.dir.z = (ushort)49152;
        if (((int)eve_rec.flag & 1) != 0)
            rideWork.dir.z += (ushort)32768;
        if (eve_rec.left != (sbyte)0)
            gmsEnemy3DWork.obj_3d.mat_speed = (float)eve_rec.left / 10f;
        rideWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkLand3TvxRopeMain);
        float num1 = 120f;
        float num2 = (float)AppMain.g_gm_main_system.sync_time / (num1 / gmsEnemy3DWork.obj_3d.mat_speed);
        gmsEnemy3DWork.obj_3d.mat_frame = (num2 - (float)(int)num2) * num1;
        rideWork.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkLand3TvxRopeDrawFunc);
        return rideWork;
    }

    public static void gmGmkLandMoveInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.prev_pos.x = (obj_work.pos.x >> 12) + (int)gmsEnemy3DWork.ene_com.eve_rec.left + ((int)gmsEnemy3DWork.ene_com.eve_rec.width >> 1);
        obj_work.prev_pos.y = (obj_work.pos.y >> 12) + (int)gmsEnemy3DWork.ene_com.eve_rec.top + ((int)gmsEnemy3DWork.ene_com.eve_rec.height >> 1);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkLandMain);
        if (((int)gmsEnemy3DWork.ene_com.eve_rec.width | (int)gmsEnemy3DWork.ene_com.eve_rec.height) == 0)
            return;
        if (gmsEnemy3DWork.ene_com.eve_rec.id != (ushort)128)
        {
            int num1;
            int num2;
            int num3;
            if ((int)gmsEnemy3DWork.ene_com.eve_rec.height < (int)gmsEnemy3DWork.ene_com.eve_rec.width)
            {
                num1 = (int)gmsEnemy3DWork.ene_com.eve_rec.width >> 1;
                num2 = obj_work.pos.x >> 12;
                num3 = obj_work.prev_pos.x;
            }
            else
            {
                num1 = (int)gmsEnemy3DWork.ene_com.eve_rec.height >> 1;
                num2 = obj_work.pos.y >> 12;
                num3 = obj_work.prev_pos.y;
            }
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 4) == 0)
            {
                ushort num4 = 768;
                while (num4 > (ushort)256 && num3 + (num1 * AppMain.mtMathSin((int)(ushort)((uint)num4 << 6)) >> 12) <= num2)
                    num4 -= (ushort)4;
                obj_work.user_timer = (int)num4;
            }
            else
            {
                obj_work.user_timer = 0;
                obj_work.user_flag = 0U;
            }
            short num5 = (short)(((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 48) >> 4 << 8);
            obj_work.user_timer -= (int)num5;
            obj_work.user_timer &= 16383;
        }
        else
        {
            short num1 = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.left * 2);
            short num2 = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.top * 2);
            short num3 = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.width * 2);
            short num4 = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.height * 2);
            int num5 = (int)num3 * 2 + (int)num4 * 2;
            obj_work.user_timer = num2 != (short)0 ? (num1 != (short)0 ? ((int)num1 + (int)num3 != 0 ? (num5 - (int)num4 - AppMain.MTM_MATH_ABS((int)num1)) * 4096 / num5 : ((int)num3 + AppMain.MTM_MATH_ABS((int)num2)) * 4096 / num5) : (num5 - AppMain.MTM_MATH_ABS((int)num2)) * 4096 / num5) : AppMain.MTM_MATH_ABS((int)num1) * 4096 / num5;
            obj_work.view_out_ofst += (short)256;
        }
    }

    public static void gmGmkLandMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.OBS_COLLISION_OBJ objCol = obj_work.col_work.obj_col;
        if (obj_work.user_work < 30U)
            AppMain.gmGmkLandMove(obj_work);
        if (objCol.rider_obj != null && objCol.rider_obj.ride_obj == obj_work)
        {
            gmsEnemy3DWork.ene_com.enemy_flag |= 1U;
            obj_work.ofst.y = ((int)obj_work.disp_flag & 2) == 0 ? 4096 : -4096;
        }
        if (((int)gmsEnemy3DWork.ene_com.enemy_flag & 1) == 0)
            return;
        if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 64) != 0)
        {
            ++obj_work.user_work;
            if (obj_work.user_work == 30U)
            {
                obj_work.move_flag &= 4294959103U;
                obj_work.move_flag |= 128U;
                obj_work.prev_pos.x = obj_work.pos.x;
                obj_work.prev_pos.y = obj_work.pos.y;
                obj_work.spd_fall_max = 30720;
                if (gmsEnemy3DWork.ene_com.eve_rec.id == (ushort)83)
                {
                    obj_work.move_flag &= 4294967039U;
                    obj_work.move_flag |= 1024U;
                    obj_work.ppFunc = AppMain._gmGmkLandColFall;
                }
                else
                    obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
            }
        }
        if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 4) == 0)
            return;
        obj_work.user_flag |= 65536U;
    }

    public static void gmGmkLandMove(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        byte num1 = AppMain.gm_gmk_land_spd_tbl[(int)(byte)((uint)gmsEnemy3DWork.ene_com.eve_rec.flag & 3U)];
        ushort userTimer = (ushort)obj_work.user_timer;
        int num2;
        int num3;
        if (gmsEnemy3DWork.ene_com.eve_rec.id != (ushort)128)
        {
            int x = obj_work.prev_pos.x;
            int y = obj_work.prev_pos.y;
            short num4 = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.width >> 1);
            short num5 = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.height >> 1);
            ushort num6;
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 4) == 0)
                num6 = (ushort)((int)AppMain.g_gm_main_system.sync_time * (int)num1 + (int)userTimer & 1023);
            else if (obj_work.user_flag == 0U)
            {
                num6 = (ushort)(obj_work.user_timer & 1023);
            }
            else
            {
                num6 = (ushort)((int)num1 * ((int)obj_work.user_flag & 1023) + (int)userTimer & 1023);
                obj_work.user_flag = (uint)((int)obj_work.user_flag & 65536 | (int)obj_work.user_flag + 1 & 1023);
            }
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 8) != 0)
            {
                num2 = (x << 12) + (int)num4 * AppMain.mtMathSin((int)(ushort)(((int)num6 << 6) + 32768));
                num3 = (y << 12) + (int)num5 * AppMain.mtMathSin((int)(ushort)((uint)num6 << 6));
            }
            else
            {
                num2 = (x << 12) + (int)num4 * AppMain.mtMathSin((int)(ushort)((uint)num6 << 6));
                num3 = (y << 12) + (int)num5 * AppMain.mtMathSin((int)(ushort)((uint)num6 << 6));
            }
        }
        else
        {
            short num4 = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.left * 2);
            short num5 = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.top * 2);
            short num6 = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.width * 2);
            short num7 = (short)((int)gmsEnemy3DWork.ene_com.eve_rec.height * 2);
            int num8 = ((int)num6 * 2 + (int)num7 * 2) * (num1 == (byte)0 ? (int)(ushort)(((long)AppMain.g_gm_main_system.sync_time + (long)((int)userTimer >> 2) & 1023L) << 2) : (int)(ushort)((int)AppMain.g_gm_main_system.sync_time * (int)num1 + (int)userTimer & 4095)) / 4096;
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 8) == 0)
            {
                if (num8 <= (int)num6)
                {
                    num2 = gmsEnemy3DWork.ene_com.born_pos_x + ((int)num4 + (int)(short)num8 << 12);
                    num3 = gmsEnemy3DWork.ene_com.born_pos_y + ((int)num5 << 12);
                }
                else if (num8 <= (int)num6 + (int)num7)
                {
                    num2 = gmsEnemy3DWork.ene_com.born_pos_x + ((int)num4 + (int)num6 << 12);
                    num3 = gmsEnemy3DWork.ene_com.born_pos_y + ((int)num5 + (int)(short)num8 - (int)num6 << 12);
                }
                else if (num8 <= (int)num6 * 2 + (int)num7)
                {
                    num2 = gmsEnemy3DWork.ene_com.born_pos_x + ((int)num4 + (int)num6 - ((int)(short)num8 - (int)num6 - (int)num7) << 12);
                    num3 = gmsEnemy3DWork.ene_com.born_pos_y + ((int)num5 + (int)num7 << 12);
                }
                else
                {
                    num2 = gmsEnemy3DWork.ene_com.born_pos_x + ((int)num4 << 12);
                    num3 = gmsEnemy3DWork.ene_com.born_pos_y + ((int)num5 + ((int)num7 - ((int)(short)num8 - (int)num6 * 2 - (int)num7)) << 12);
                }
            }
            else if (num8 <= (int)num6)
            {
                num2 = gmsEnemy3DWork.ene_com.born_pos_x + ((int)num4 + (int)num6 - (int)(short)num8 << 12);
                num3 = gmsEnemy3DWork.ene_com.born_pos_y + ((int)num5 << 12);
            }
            else if (num8 <= (int)num6 + (int)num7)
            {
                num2 = gmsEnemy3DWork.ene_com.born_pos_x + ((int)num4 << 12);
                num3 = gmsEnemy3DWork.ene_com.born_pos_y + ((int)num5 + (int)(short)num8 - (int)num6 << 12);
            }
            else if (num8 <= (int)num6 * 2 + (int)num7)
            {
                num2 = gmsEnemy3DWork.ene_com.born_pos_x + ((int)num4 + ((int)(short)num8 - (int)num6 - (int)num7) << 12);
                num3 = gmsEnemy3DWork.ene_com.born_pos_y + ((int)num5 + (int)num7 << 12);
            }
            else
            {
                num2 = gmsEnemy3DWork.ene_com.born_pos_x + ((int)num4 + (int)num6 << 12);
                num3 = gmsEnemy3DWork.ene_com.born_pos_y + ((int)num5 + ((int)num7 - ((int)(short)num8 - (int)num6 * 2 - (int)num7)) << 12);
            }
        }
        obj_work.move.x = num2 - obj_work.pos.x;
        obj_work.move.y = num3 - obj_work.pos.y;
        obj_work.pos.x = num2;
        obj_work.pos.y = num3;
    }

    public static void gmGmkLandColFall(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.move_flag & 1) == 0)
            return;
        obj_work.move_flag |= 256U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    public static void gmGmkZ3LandPulleyMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.dir.z += (ushort)obj_work.user_work;
    }

    public static void gmGmkLand3TvxRopeMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        obj3d.mat_frame += obj3d.mat_speed;
        if ((double)obj3d.mat_frame < 120.0)
            return;
        obj3d.mat_frame -= 120f;
    }

    public static void gmGmkLand3TvxDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.NNS_TEXCOORD uv = new AppMain.NNS_TEXCOORD(0.0f, 0.0f);
        AppMain.gmGmkLand3TvxDrawFuncEx(0U, obj_work.obj_3d.texlist, ref obj_work.pos, ref obj_work.scale, AppMain.GMD_TVX_DISP_LIGHT_DISABLE, (short)0, ref uv);
    }

    public static void gmGmkLand3TvxRDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.NNS_TEXCOORD uv = new AppMain.NNS_TEXCOORD(0.0f, 0.0f);
        AppMain.gmGmkLand3TvxDrawFuncEx(1U, obj_work.obj_3d.texlist, ref obj_work.pos, ref obj_work.scale, AppMain.GMD_TVX_DISP_LIGHT_DISABLE, (short)0, ref uv);
    }

    public static void gmGmkLand3TvxPulleyDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.NNS_TEXCOORD uv = new AppMain.NNS_TEXCOORD(0.0f, 0.0f);
        AppMain.gmGmkLand3TvxDrawFuncEx(2U, obj_work.obj_3d.texlist, ref obj_work.pos, ref obj_work.scale, AppMain.GMD_TVX_DISP_LIGHT_DISABLE | AppMain.GMD_TVX_DISP_ROTATE, (short)-obj_work.dir.z, ref uv);
    }

    public static void gmGmkLand3TvxRopeDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        var coord = new AppMain.NNS_TEXCOORD(0.0f, 0.0f)
        {
            v = (float)(-0.25 * (double)obj_work.obj_3d.mat_frame / 120.0)
        };
        AppMain.gmGmkLand3TvxDrawFuncEx(3U, obj_work.obj_3d.texlist, ref obj_work.pos, ref obj_work.scale, AppMain.GMD_TVX_DISP_LIGHT_DISABLE | AppMain.GMD_TVX_DISP_ROTATE, (short)-obj_work.dir.z, ref coord);
    }

    public static void gmGmkLand3TvxDrawFuncEx(
      uint tvx_index,
      AppMain.NNS_TEXLIST texlist,
      ref AppMain.VecFx32 pos,
      ref AppMain.VecFx32 scale,
      uint disp_flag,
      short dir_z,
      ref AppMain.NNS_TEXCOORD uv)
    {
        int index = (int)tvx_index;
        AppMain.TVX_FILE model_tvx;
        if (AppMain.gm_gmk_land_3_obj_tvx_list.buf[index] == null)
        {
            model_tvx = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_land_3_obj_tvx_list, index));
            AppMain.gm_gmk_land_3_obj_tvx_list.buf[index] = (object)model_tvx;
        }
        else
            model_tvx = (AppMain.TVX_FILE)AppMain.gm_gmk_land_3_obj_tvx_list.buf[index];

        var work = new AppMain.GMS_TVX_EX_WORK()
        {
            u_wrap = 1,
            v_wrap = 1,
            coord = {
        u = uv.u,
        v = uv.v
      },
            color = uint.MaxValue
        };
        AppMain.GmTvxSetModelEx(model_tvx, texlist, ref pos, ref scale, disp_flag, dir_z, ref work);
    }


}