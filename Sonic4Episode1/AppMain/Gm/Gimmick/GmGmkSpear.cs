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
    private static AppMain.OBS_OBJECT_WORK GmGmkSpearUInit(
         AppMain.GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        AppMain.GMS_GMK_SPEAR_WORK pwork = (AppMain.GMS_GMK_SPEAR_WORK)AppMain.gmGmkSpearInit(eve_rec, pos_x, pos_y, type);
        pwork.obj_type = 0U;
        pwork.vect = (ushort)49152;
        pwork.gmk_work.ene_com.obj_work.dir.z = (ushort)0;
        if (eve_rec.left > (sbyte)0)
            pwork.timer_set_wait_upper = (short)eve_rec.left;
        if (eve_rec.width > (byte)0)
            pwork.timer_set_wait_lower = (short)eve_rec.width;
        if (eve_rec.top < (sbyte)0)
            pwork.timer_set_move = -((int)eve_rec.top << 12);
        AppMain.gmGmkSpear_CreateParts(pwork);
        pwork.gmk_work.ene_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearStart);
        return pwork.gmk_work.ene_com.obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSpearDInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SPEAR_WORK pwork = (AppMain.GMS_GMK_SPEAR_WORK)AppMain.gmGmkSpearInit(eve_rec, pos_x, pos_y, type);
        pwork.obj_type = 1U;
        pwork.gmk_work.ene_com.obj_work.dir.z = (ushort)32768;
        pwork.vect = (ushort)16384;
        if (eve_rec.left > (sbyte)0)
            pwork.timer_set_wait_upper = (short)eve_rec.left;
        if (eve_rec.width > (byte)0)
            pwork.timer_set_wait_lower = (short)eve_rec.width;
        if (eve_rec.top > (sbyte)0)
            pwork.timer_set_move = (int)eve_rec.top << 12;
        AppMain.gmGmkSpear_CreateParts(pwork);
        pwork.gmk_work.ene_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearStart);
        return pwork.gmk_work.ene_com.obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSpearLInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SPEAR_WORK pwork = (AppMain.GMS_GMK_SPEAR_WORK)AppMain.gmGmkSpearInit(eve_rec, pos_x, pos_y, type);
        pwork.obj_type = 2U;
        pwork.gmk_work.ene_com.obj_work.dir.z = (ushort)49152;
        pwork.vect = (ushort)32768;
        if (eve_rec.top > (sbyte)0)
            pwork.timer_set_wait_upper = (short)eve_rec.top;
        if (eve_rec.width > (byte)0)
            pwork.timer_set_wait_lower = (short)eve_rec.width;
        if (eve_rec.left < (sbyte)0)
            pwork.timer_set_move = -((int)eve_rec.left << 12);
        AppMain.gmGmkSpear_CreateParts(pwork);
        pwork.gmk_work.ene_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearStart);
        return pwork.gmk_work.ene_com.obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSpearRInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SPEAR_WORK pwork = (AppMain.GMS_GMK_SPEAR_WORK)AppMain.gmGmkSpearInit(eve_rec, pos_x, pos_y, type);
        pwork.obj_type = 3U;
        pwork.gmk_work.ene_com.obj_work.dir.z = (ushort)16384;
        pwork.vect = (ushort)0;
        if (eve_rec.top > (sbyte)0)
            pwork.timer_set_wait_upper = (short)eve_rec.top;
        if (eve_rec.width > (byte)0)
            pwork.timer_set_wait_lower = (short)eve_rec.width;
        if (eve_rec.left > (sbyte)0)
            pwork.timer_set_move = (int)eve_rec.left << 12;
        AppMain.gmGmkSpear_CreateParts(pwork);
        pwork.gmk_work.ene_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearStart);
        return pwork.gmk_work.ene_com.obj_work;
    }

    public static void GmGmkSpearBuild()
    {
        AppMain.gm_gmk_spear_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(856)), AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(857)), 0U);
    }

    public static void GmGmkSpearFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(856));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_spear_obj_3d_list, amsAmbHeader.file_num);
    }

    private static uint gmGmkSpearSyncTimeGet(AppMain.GMS_GMK_SPEAR_WORK pwork)
    {
        return AppMain.g_gm_main_system.sync_time % ((uint)((pwork.timer_set_move + (pwork.stroke_spd - 1)) / pwork.stroke_spd) * 2U + (uint)pwork.timer_set_wait_upper + (uint)pwork.timer_set_wait_lower);
    }

    private static void gmGmkSpearStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (AppMain.GMS_GMK_SPEAR_WORK)obj_work;
        gmsGmkSpearWork.gmk_work.ene_com.rect_work[2].flag &= 4294967291U;
        gmsGmkSpearWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsGmkSpearWork.gmk_work.ene_com.rect_work[1].flag |= 4U;
        AppMain.OBS_RECT_WORK pRec = gmsGmkSpearWork.gmk_work.ene_com.rect_work[1];
        AppMain.ObjRectWorkZSet(pRec, AppMain.tbl_gm_gmk_spear_rect[(int)gmsGmkSpearWork.obj_type][0], AppMain.tbl_gm_gmk_spear_rect[(int)gmsGmkSpearWork.obj_type][1], (short)-500, AppMain.tbl_gm_gmk_spear_rect[(int)gmsGmkSpearWork.obj_type][2], AppMain.tbl_gm_gmk_spear_rect[(int)gmsGmkSpearWork.obj_type][3], (short)500);
        pRec.flag |= 4U;
        pRec.flag |= 1024U;
        obj_work.flag &= 4294967293U;
        uint syncTime = AppMain.g_gm_main_system.sync_time;
        uint num1 = (uint)((gmsGmkSpearWork.timer_set_move + (gmsGmkSpearWork.stroke_spd - 1)) / gmsGmkSpearWork.stroke_spd);
        uint num2 = num1 * 2U + (uint)gmsGmkSpearWork.timer_set_wait_upper + (uint)gmsGmkSpearWork.timer_set_wait_lower;
        if (syncTime <= (uint)gmsGmkSpearWork.timer_dec)
        {
            gmsGmkSpearWork.timer_dec -= (int)syncTime - 1;
            gmsGmkSpearWork.timer_dec += (int)gmsGmkSpearWork.timer_set_wait_lower;
            AppMain.gmGmkSpearStay(obj_work);
        }
        else
        {
            uint num3 = (syncTime - (uint)gmsGmkSpearWork.timer_dec) % num2;
            if (num3 <= (uint)gmsGmkSpearWork.timer_set_wait_lower)
            {
                gmsGmkSpearWork.timer_dec = (int)((long)gmsGmkSpearWork.timer_set_wait_lower - (long)(num3 - 1U));
                AppMain.gmGmkSpearStay(obj_work);
            }
            else
            {
                uint num4 = num3 - (uint)gmsGmkSpearWork.timer_set_wait_lower;
                int num5 = AppMain.mtMathCos((int)gmsGmkSpearWork.vect);
                int num6 = AppMain.mtMathSin((int)gmsGmkSpearWork.vect);
                int num7 = num5 * gmsGmkSpearWork.stroke_spd >> 12;
                int num8 = num6 * gmsGmkSpearWork.stroke_spd >> 12;
                if (num4 < num1)
                {
                    gmsGmkSpearWork.timer_dec = gmsGmkSpearWork.timer_set_move;
                    for (; num4 > 1U; --num4)
                    {
                        gmsGmkSpearWork.timer_dec -= gmsGmkSpearWork.stroke_spd;
                        obj_work.pos.x += num7;
                        obj_work.pos.y += num8;
                    }
                    obj_work.spd.x = num7;
                    obj_work.spd.y = num8;
                    AppMain.gmGmkSpearStroke(obj_work);
                }
                else
                {
                    uint num9 = num4 - num1;
                    int num10 = AppMain.mtMathCos((int)gmsGmkSpearWork.vect) * gmsGmkSpearWork.timer_set_move >> 12;
                    int num11 = AppMain.mtMathSin((int)gmsGmkSpearWork.vect) * gmsGmkSpearWork.timer_set_move >> 12;
                    obj_work.pos.x += num10;
                    obj_work.pos.y += num11;
                    if (num9 <= (uint)gmsGmkSpearWork.timer_set_wait_upper)
                    {
                        gmsGmkSpearWork.timer_dec = (int)((long)gmsGmkSpearWork.timer_set_wait_upper - (long)(num9 - 1U));
                        AppMain.gmGmkSpearWait(obj_work);
                    }
                    else
                    {
                        uint num12 = num9 - (uint)gmsGmkSpearWork.timer_set_wait_upper;
                        gmsGmkSpearWork.timer_dec = gmsGmkSpearWork.timer_set_move;
                        for (; num12 > 1U; --num12)
                        {
                            gmsGmkSpearWork.timer_dec -= gmsGmkSpearWork.stroke_spd;
                            obj_work.pos.x -= num7;
                            obj_work.pos.y -= num8;
                        }
                        obj_work.spd.x = -num7;
                        obj_work.spd.y = -num8;
                        AppMain.gmGmkSpearShrink(obj_work);
                    }
                }
            }
        }
    }

    private static void gmGmkSpearStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearStay_100);
        AppMain.gmGmkSpearStay_100(obj_work);
    }

    private static void gmGmkSpearStay_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (AppMain.GMS_GMK_SPEAR_WORK)obj_work;
        --gmsGmkSpearWork.timer_dec;
        if (gmsGmkSpearWork.timer_dec > 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearStay_200);
    }

    private static void gmGmkSpearStay_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (AppMain.GMS_GMK_SPEAR_WORK)obj_work;
        obj_work.spd.x = AppMain.mtMathCos((int)gmsGmkSpearWork.vect);
        obj_work.spd.y = AppMain.mtMathSin((int)gmsGmkSpearWork.vect);
        obj_work.spd.x = obj_work.spd.x * gmsGmkSpearWork.stroke_spd >> 12;
        obj_work.spd.y = obj_work.spd.y * gmsGmkSpearWork.stroke_spd >> 12;
        gmsGmkSpearWork.timer_dec = gmsGmkSpearWork.timer_set_move;
        AppMain.GmSoundPlaySE("Spear");
        AppMain.gmGmkSpearStroke(obj_work);
    }

    private static void gmGmkSpearStroke(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x += obj_work.spd.x;
        obj_work.pos.y += obj_work.spd.y;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearStroke_100);
        AppMain.gmGmkSpearStroke_100(obj_work);
    }

    private static void gmGmkSpearStroke_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (AppMain.GMS_GMK_SPEAR_WORK)obj_work;
        gmsGmkSpearWork.timer_dec -= gmsGmkSpearWork.stroke_spd;
        if (gmsGmkSpearWork.timer_dec > 0)
            return;
        if (gmsGmkSpearWork.timer_dec < 0)
        {
            obj_work.spd.x = AppMain.mtMathCos((int)gmsGmkSpearWork.vect);
            obj_work.spd.y = AppMain.mtMathSin((int)gmsGmkSpearWork.vect);
            obj_work.spd.x = obj_work.spd.x * gmsGmkSpearWork.timer_dec >> 12;
            obj_work.spd.y = obj_work.spd.y * gmsGmkSpearWork.timer_dec >> 12;
        }
        else
        {
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearStroke_200);
    }

    private static void gmGmkSpearStroke_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPEAR_WORK pwork = (AppMain.GMS_GMK_SPEAR_WORK)obj_work;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        uint num1 = (uint)((pwork.timer_set_move + (pwork.stroke_spd - 1)) / pwork.stroke_spd);
        uint num2 = AppMain.gmGmkSpearSyncTimeGet(pwork) - (uint)pwork.timer_set_wait_lower - num1;
        pwork.timer_dec = num2 > (uint)pwork.timer_set_wait_upper ? 0 : (int)((long)pwork.timer_set_wait_upper - (long)(num2 - 1U));
        AppMain.gmGmkSpearWait(obj_work);
    }

    private static void gmGmkSpearWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearWait_100);
        AppMain.gmGmkSpearWait_100(obj_work);
    }

    private static void gmGmkSpearWait_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (AppMain.GMS_GMK_SPEAR_WORK)obj_work;
        --gmsGmkSpearWork.timer_dec;
        if (gmsGmkSpearWork.timer_dec > 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearWait_200);
    }

    private static void gmGmkSpearWait_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (AppMain.GMS_GMK_SPEAR_WORK)obj_work;
        obj_work.spd.x = -(AppMain.mtMathCos((int)gmsGmkSpearWork.vect) * gmsGmkSpearWork.stroke_spd) >> 12;
        obj_work.spd.y = -(AppMain.mtMathSin((int)gmsGmkSpearWork.vect) * gmsGmkSpearWork.stroke_spd) >> 12;
        gmsGmkSpearWork.timer_dec = gmsGmkSpearWork.timer_set_move;
        AppMain.gmGmkSpearShrink(obj_work);
    }

    private static void gmGmkSpearShrink(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x += obj_work.spd.x;
        obj_work.pos.y += obj_work.spd.y;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearShrink_100);
        AppMain.gmGmkSpearShrink_100(obj_work);
    }

    private static void gmGmkSpearShrink_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPEAR_WORK gmsGmkSpearWork = (AppMain.GMS_GMK_SPEAR_WORK)obj_work;
        gmsGmkSpearWork.timer_dec -= gmsGmkSpearWork.stroke_spd;
        if (gmsGmkSpearWork.timer_dec > 0)
            return;
        if (gmsGmkSpearWork.timer_dec < 0)
        {
            obj_work.spd.x = AppMain.mtMathCos((int)gmsGmkSpearWork.vect);
            obj_work.spd.y = AppMain.mtMathSin((int)gmsGmkSpearWork.vect);
            obj_work.spd.x = -(obj_work.spd.x * gmsGmkSpearWork.timer_dec >> 12);
            obj_work.spd.y = -(obj_work.spd.y * gmsGmkSpearWork.timer_dec >> 12);
        }
        else
        {
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearShrink_200);
    }

    private static void gmGmkSpearShrink_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPEAR_WORK pwork = (AppMain.GMS_GMK_SPEAR_WORK)obj_work;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        uint num = AppMain.gmGmkSpearSyncTimeGet(pwork);
        pwork.timer_dec = num > (uint)pwork.timer_set_wait_lower ? 0 : (int)((long)pwork.timer_set_wait_lower - (long)(num - 1U));
        AppMain.gmGmkSpearStay(obj_work);
    }

    private static void gmGmkSpearRod(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SPEARPARTS_WORK gmkSpearpartsWork = (AppMain.GMS_GMK_SPEARPARTS_WORK)obj_work;
        switch (gmkSpearpartsWork.connect_type)
        {
            case 0:
                gmkSpearpartsWork.connect = gmkSpearpartsWork.parent_connect.pos.y;
                break;
            case 1:
                gmkSpearpartsWork.connect = gmkSpearpartsWork.parent_connect.pos.y;
                break;
            case 2:
                gmkSpearpartsWork.connect = gmkSpearpartsWork.parent_connect.pos.x;
                break;
            case 3:
                gmkSpearpartsWork.connect = gmkSpearpartsWork.parent_connect.pos.x;
                break;
        }
        int num = AppMain.MTM_MATH_ABS(gmkSpearpartsWork.connect - gmkSpearpartsWork.fulcrum) / 5;
        obj_work.scale.y = num;
    }

    private static void gmGmkSpear_CreateParts(AppMain.GMS_GMK_SPEAR_WORK pwork)
    {
        AppMain.OBS_OBJECT_WORK objWork = pwork.gmk_work.ene_com.obj_work;
        AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SPEARPARTS_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "Gmk_SpearBase");
        AppMain.GMS_GMK_SPEARPARTS_WORK gmkSpearpartsWork1 = (AppMain.GMS_GMK_SPEARPARTS_WORK)work1;
        AppMain.ObjObjectCopyAction3dNNModel(work1, AppMain.gm_gmk_spear_obj_3d_list[2], gmkSpearpartsWork1.eff_work.obj_3d);
        work1.parent_obj = objWork;
        work1.pos.x = objWork.pos.x;
        work1.pos.y = objWork.pos.y;
        work1.pos.z = objWork.pos.z;
        switch (pwork.obj_type)
        {
            case 0:
                work1.pos.y += 16384;
                break;
            case 1:
                work1.pos.y -= 16384;
                break;
            case 2:
                work1.pos.x += 16384;
                break;
            case 3:
                work1.pos.x -= 16384;
                break;
        }
        work1.dir.z = objWork.dir.z;
        work1.flag &= 4294966271U;
        work1.move_flag |= 256U;
        work1.disp_flag &= 4294967039U;
        work1.flag |= 2U;
        work1.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SPEARPARTS_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "Gmk_SpearRod");
        AppMain.GMS_GMK_SPEARPARTS_WORK gmkSpearpartsWork2 = (AppMain.GMS_GMK_SPEARPARTS_WORK)work2;
        AppMain.ObjObjectCopyAction3dNNModel(work2, AppMain.gm_gmk_spear_obj_3d_list[1], gmkSpearpartsWork2.eff_work.obj_3d);
        work2.parent_obj = objWork;
        work2.parent_ofst.x = 0;
        work2.parent_ofst.y = 0;
        work2.parent_ofst.z = -1;
        work2.dir.z = objWork.dir.z;
        work2.flag |= 1024U;
        work2.move_flag |= 256U;
        work2.disp_flag &= 4294967039U;
        work2.flag |= 2U;
        switch (pwork.obj_type)
        {
            case 0:
                gmkSpearpartsWork2.connect = objWork.pos.y;
                break;
            case 1:
                gmkSpearpartsWork2.connect = objWork.pos.y;
                break;
            case 2:
                gmkSpearpartsWork2.connect = objWork.pos.x;
                break;
            case 3:
                gmkSpearpartsWork2.connect = objWork.pos.x;
                break;
        }
        gmkSpearpartsWork2.connect_type = pwork.obj_type;
        gmkSpearpartsWork2.parent_connect = objWork;
        gmkSpearpartsWork2.obj_type = pwork.obj_type;
        gmkSpearpartsWork2.fulcrum = gmkSpearpartsWork2.connect;
        work2.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSpearRod);
    }

    private static AppMain.OBS_OBJECT_WORK gmGmkSpearInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SPEAR_WORK work = (AppMain.GMS_GMK_SPEAR_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SPEAR_WORK()), "Gmk_Spear");
        AppMain.OBS_OBJECT_WORK objWork = work.gmk_work.ene_com.obj_work;
        AppMain.GMS_ENEMY_3D_WORK gmkWork = work.gmk_work;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.gm_gmk_spear_obj_3d_list[0], gmkWork.obj_3d);
        objWork.pos.z = 0;
        objWork.move_flag |= 256U;
        work.timer_set_move = 196608;
        work.stroke_spd = 32768;
        work.timer_set_wait_upper = (short)120;
        work.timer_set_wait_lower = (short)120;
        work.timer_dec = (int)eve_rec.height;
        if (((int)eve_rec.flag & 31) != 0)
        {
            int num;
            if (((int)eve_rec.flag & 16) == 0)
            {
                num = ((int)eve_rec.flag & 15) << 10;
            }
            else
            {
                num = -((int)eve_rec.flag & 15) << 10;
                if (num == 0)
                    num = -16384;
            }
            work.stroke_spd += num;
        }
        return objWork;
    }


}