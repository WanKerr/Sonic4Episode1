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
    private static void gmGmkBreakLandStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BLAND_WORK gmsGmkBlandWork = (AppMain.GMS_GMK_BLAND_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK objWork = AppMain.g_gm_main_system.ply_work[0].obj_work;
        if (objWork.ride_obj != obj_work)
            return;
        int num1 = obj_work.pos.x + gmsGmkBlandWork.colrect_left;
        int num2 = obj_work.pos.x + gmsGmkBlandWork.colrect_right;
        int num3 = num1 - ((int)objWork.field_rect[0] << 12);
        int num4 = num2 - ((int)objWork.field_rect[2] << 12);
        if (num3 > objWork.pos.x || objWork.pos.x > num4)
            return;
        if (objWork.pos.x - obj_work.pos.x <= (int)gmsGmkBlandWork.gmk_work.ene_com.col_work.obj_col.width * 4096 / 2)
        {
            int num5 = -((int)gmsGmkBlandWork.gmk_work.ene_com.col_work.obj_col.width * 4096 / 2);
        }
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, 0, 0);
        obsObjectWork.pos.x = obj_work.pos.x;
        obsObjectWork.pos.y = obj_work.pos.y;
        obsObjectWork.pos.z = obj_work.pos.z + 131072;
        if (gmsGmkBlandWork.vect == (ushort)0)
            obsObjectWork.pos.x += 262144;
        else
            obsObjectWork.pos.x -= 262144;
        gmsGmkBlandWork.broken_timer = 45;
        gmsGmkBlandWork.quake_timer = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBreakLandStay_100);
    }

    private static void gmGmkBreakLandStay_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BLAND_WORK gmsGmkBlandWork = (AppMain.GMS_GMK_BLAND_WORK)obj_work;
        --gmsGmkBlandWork.broken_timer;
        if (gmsGmkBlandWork.broken_timer <= 0)
        {
            AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_breakland_obj_3d_list[gmsGmkBlandWork.vect == (ushort)0 ? 2 : 3], gmsGmkBlandWork.gmk_work.obj_3d);
            AppMain.ObjObjectAction3dNNMotionLoad(obj_work, 0, false, AppMain.ObjDataGet(796), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
            if (gmsGmkBlandWork.vect == (ushort)32768)
            {
                AppMain.ObjDrawObjectActionSet(obj_work, 0);
                obj_work.obj_3d.use_light_flag &= 4294967294U;
                obj_work.obj_3d.use_light_flag |= 4U;
                obj_work.obj_3d.use_light_flag |= 65536U;
            }
            else
            {
                AppMain.ObjDrawObjectActionSet(obj_work, 1);
                obj_work.obj_3d.use_light_flag &= 4294967294U;
                obj_work.obj_3d.use_light_flag |= 2U;
                obj_work.obj_3d.use_light_flag |= 65536U;
            }
            obj_work.disp_flag &= 4294967279U;
            obj_work.disp_flag &= 4294967291U;
            gmsGmkBlandWork.gmk_work.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.GmSoundPlaySE("BreakGround");
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBreakLandBroken);
        }
        else
        {
            gmsGmkBlandWork.quake_timer &= 3;
            obj_work.pos.y += (int)AppMain.tbl_breaklandquake[gmsGmkBlandWork.quake_timer] * 4096;
            ++gmsGmkBlandWork.quake_timer;
        }
    }

    private static void gmGmkBreakLandBroken(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 8U;
    }

    private static AppMain.OBS_OBJECT_WORK gmGmkBreakLandInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      ushort vect)
    {
        AppMain.GMS_GMK_BLAND_WORK work = (AppMain.GMS_GMK_BLAND_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BLAND_WORK()), "GMK_BREAK_LAND_MAIN");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_breakland_obj_3d_list[vect == (ushort)0 ? 0 : 1], gmsEnemy3DWork.obj_3d);
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
        {
            obj_work.obj_3d.use_light_flag &= 4294967294U;
            obj_work.obj_3d.use_light_flag |= 32U;
            obj_work.obj_3d.use_light_flag |= 65536U;
        }
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.diff_data = AppMain.g_gm_breakland_col;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)AppMain.gm_gmk_breakland_col_rect_tbl[0];
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)AppMain.gm_gmk_breakland_col_rect_tbl[1];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = AppMain.gm_gmk_breakland_col_rect_tbl[2];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = AppMain.gm_gmk_breakland_col_rect_tbl[3];
        work.colrect_left = 278528;
        work.colrect_right = 524288;
        if (vect == (ushort)32768)
        {
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)((int)-gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x - (int)gmsEnemy3DWork.ene_com.col_work.obj_col.width);
            work.colrect_right = -278528;
            work.colrect_left = -524288;
        }
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 268435456U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.dir = (ushort)0;
        if (((int)eve_rec.flag & 128) == 0)
            gmsEnemy3DWork.ene_com.col_work.obj_col.attr = (ushort)1;
        obj_work.pos.z = -4096;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 272629760U;
        obj_work.flag |= 2U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBreakLandStay);
        work.vect = vect;
        return obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBreakLandRInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return AppMain.gmGmkBreakLandInit(eve_rec, pos_x, pos_y, type, (ushort)0);
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBreakLandLInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return AppMain.gmGmkBreakLandInit(eve_rec, pos_x, pos_y, type, (ushort)32768);
    }

    private static void GmGmkBreakLandBuild()
    {
        AppMain.gm_gmk_breakland_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(794)), AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(795)), 0U);
    }

    private static void GmGmkBreakLandFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(794));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_breakland_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void GmGmkBreakLandSetLight()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA();
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.x = -0.5f;
        nnsVector.y = -0.05f;
        nnsVector.z = -1f;
        col.r = 0.65f;
        col.g = 0.65f;
        col.b = 0.65f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_1, ref col, 1f, nnsVector);
        nnsVector.x = 0.4f;
        nnsVector.y = -0.05f;
        nnsVector.z = -1f;
        col.r = 0.65f;
        col.g = 0.65f;
        col.b = 0.65f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_2, ref col, 1f, nnsVector);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }


}