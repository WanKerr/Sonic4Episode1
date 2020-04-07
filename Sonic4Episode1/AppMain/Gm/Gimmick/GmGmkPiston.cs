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
    private static AppMain.OBS_OBJECT_WORK GmGmkPistonUpInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.gmGmkPistonInit(eve_rec, pos_x, pos_y, type);
        AppMain.GMS_GMK_PISTON_WORK gmsGmkPistonWork = (AppMain.GMS_GMK_PISTON_WORK)obsObjectWork;
        gmsGmkPistonWork.obj_type = 0U;
        gmsGmkPistonWork.piston_vect = (ushort)32768;
        obsObjectWork.dir.z = (ushort)0;
        gmsGmkPistonWork.gmk_work.ene_com.col_work.obj_col.flag |= 32U;
        if (eve_rec.top < (sbyte)0)
            gmsGmkPistonWork.timer_set_move = (int)-eve_rec.top * 2 << 12;
        else if (eve_rec.top > (sbyte)0)
            gmsGmkPistonWork.timer_set_move = (int)eve_rec.top * 2 << 12;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonStart);
        return obsObjectWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPistonDownInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.gmGmkPistonInit(eve_rec, pos_x, pos_y, type);
        AppMain.GMS_GMK_PISTON_WORK gmsGmkPistonWork = (AppMain.GMS_GMK_PISTON_WORK)obsObjectWork;
        gmsGmkPistonWork.obj_type = 1U;
        gmsGmkPistonWork.piston_vect = (ushort)0;
        obsObjectWork.dir.z = (ushort)32768;
        gmsGmkPistonWork.gmk_work.ene_com.col_work.obj_col.flag |= 32U;
        if (eve_rec.top > (sbyte)0)
            gmsGmkPistonWork.timer_set_move = (int)eve_rec.top * 2 << 12;
        else if (eve_rec.top < (sbyte)0)
            gmsGmkPistonWork.timer_set_move = (int)-eve_rec.top * 2 << 12;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonStart);
        return obsObjectWork;
    }

    public static void GmGmkPistonBuild()
    {
        AppMain.gm_gmk_piston_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(842), AppMain.GmGameDatGetGimmickData(843), 0U);
    }

    public static void GmGmkPistonFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(842);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_piston_obj_3d_list, gimmickData.file_num);
    }

    private static uint gmGmkPistonSyncTimeGet(AppMain.GMS_GMK_PISTON_WORK pwork)
    {
        return AppMain.g_gm_main_system.sync_time % ((uint)((pwork.timer_set_move + (pwork.stroke_spd - 1)) / pwork.stroke_spd) * 2U + (uint)pwork.timer_set_wait_upper + (uint)pwork.timer_set_wait_lower);
    }

    private static void gmGmkPistonStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK pwork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        pwork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        pwork.gmk_work.ene_com.col_work.obj_col.width = (ushort)AppMain.GmkPistonData.tbl_gm_gmk_piston_col_rect[(int)pwork.obj_type][0];
        pwork.gmk_work.ene_com.col_work.obj_col.height = (ushort)AppMain.GmkPistonData.tbl_gm_gmk_piston_col_rect[(int)pwork.obj_type][1];
        pwork.gmk_work.ene_com.col_work.obj_col.ofst_x = AppMain.GmkPistonData.tbl_gm_gmk_piston_col_rect[(int)pwork.obj_type][2];
        pwork.gmk_work.ene_com.col_work.obj_col.ofst_y = AppMain.GmkPistonData.tbl_gm_gmk_piston_col_rect[(int)pwork.obj_type][3];
        pwork.gmk_work.ene_com.col_work.obj_col.dir = (ushort)0;
        pwork.gmk_work.ene_com.col_work.obj_col.diff_data = AppMain.g_gm_default_col;
        pwork.gmk_work.ene_com.col_work.obj_col.flag |= 134217760U;
        pwork.gmk_work.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.gmGmkPistonRod_Create(obj_work);
        uint num1 = (uint)((pwork.timer_set_move + (pwork.stroke_spd - 1)) / pwork.stroke_spd);
        uint num2 = AppMain.gmGmkPistonSyncTimeGet(pwork);
        if (num2 <= (uint)pwork.timer_set_wait_lower)
        {
            pwork.timer_dec = (int)((long)pwork.timer_set_wait_lower - (long)(num2 - 1U));
            AppMain.gmGmkPistonStay(obj_work);
        }
        else
        {
            uint num3 = num2 - (uint)pwork.timer_set_wait_lower;
            int num4 = pwork.piston_vect == (ushort)0 ? pwork.stroke_spd : -pwork.stroke_spd;
            if (num3 < num1)
            {
                pwork.timer_dec = pwork.timer_set_move;
                for (; num3 > 1U; --num3)
                {
                    pwork.timer_dec -= pwork.stroke_spd;
                    obj_work.pos.y += num4;
                }
                AppMain.gmGmkPistonStroke(obj_work);
            }
            else
            {
                uint num5 = num3 - num1;
                obj_work.pos.y += pwork.piston_vect == (ushort)0 ? pwork.timer_set_move : -pwork.timer_set_move;
                if (num5 <= (uint)pwork.timer_set_wait_upper)
                {
                    pwork.timer_dec = (int)((long)pwork.timer_set_wait_upper - (long)(num5 - 1U));
                    AppMain.gmGmkPistonTopDeadWait(obj_work);
                }
                else
                {
                    uint num6 = num5 - (uint)pwork.timer_set_wait_upper;
                    pwork.timer_dec = pwork.timer_set_move;
                    for (; num6 > 1U; --num6)
                    {
                        pwork.timer_dec -= pwork.stroke_spd;
                        obj_work.pos.y -= num4;
                    }
                    AppMain.gmGmkPistonShrink(obj_work);
                }
            }
        }
    }

    private static void gmGmkPistonStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.y = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonStay_100);
        AppMain.gmGmkPistonStay_100(obj_work);
    }

    private static void gmGmkPistonStay_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK gmsGmkPistonWork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        --gmsGmkPistonWork.timer_dec;
        if (gmsGmkPistonWork.timer_dec > 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonStay_200);
    }

    private static void gmGmkPistonStay_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK gmsGmkPistonWork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        gmsGmkPistonWork.timer_dec = gmsGmkPistonWork.timer_set_move;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonStroke);
        AppMain.gmGmkPistonStroke(obj_work);
    }

    private static void gmGmkPistonStroke(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK gmsGmkPistonWork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        obj_work.spd.y = gmsGmkPistonWork.piston_vect == (ushort)0 ? gmsGmkPistonWork.stroke_spd : -gmsGmkPistonWork.stroke_spd;
        obj_work.pos.y += obj_work.spd.y;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonStroke_100);
        AppMain.gmGmkPistonStroke_100(obj_work);
        AppMain.GmSoundPlaySE("Piston1");
    }

    private static void gmGmkPistonStroke_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK gmsGmkPistonWork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        gmsGmkPistonWork.timer_dec -= gmsGmkPistonWork.stroke_spd;
        if (gmsGmkPistonWork.timer_dec > 0)
            return;
        obj_work.spd.y = 0;
        if (gmsGmkPistonWork.timer_dec < 0)
        {
            obj_work.spd.y = gmsGmkPistonWork.timer_dec;
            if (gmsGmkPistonWork.piston_vect == (ushort)32768)
                obj_work.spd.y = -obj_work.spd.y;
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonStroke_200);
    }

    private static void gmGmkPistonStroke_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK pwork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        uint num1 = (uint)((pwork.timer_set_move + (pwork.stroke_spd - 1)) / pwork.stroke_spd);
        uint num2 = AppMain.gmGmkPistonSyncTimeGet(pwork) - (uint)pwork.timer_set_wait_lower - num1;
        pwork.timer_dec = num2 > (uint)pwork.timer_set_wait_upper ? 0 : (int)((long)pwork.timer_set_wait_upper - (long)(num2 - 1U));
        AppMain.gmGmkPistonTopDeadWait(obj_work);
    }

    private static void gmGmkPistonTopDeadWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!((AppMain.GMS_GMK_PISTON_WORK)obj_work).efct_di)
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 48);
            obsObjectWork.pos.x = obj_work.pos.x;
            obsObjectWork.pos.y = obj_work.pos.y;
            obsObjectWork.pos.z = obj_work.pos.z + 65536;
            obsObjectWork.dir.z = obj_work.dir.z;
            AppMain.GmSoundPlaySE("Piston2");
        }
        obj_work.spd.y = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonTopDeadWait_100);
        AppMain.gmGmkPistonTopDeadWait_100(obj_work);
    }

    private static void gmGmkPistonTopDeadWait_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK gmsGmkPistonWork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        --gmsGmkPistonWork.timer_dec;
        if (gmsGmkPistonWork.timer_dec > 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonTopDeadWait_200);
    }

    private static void gmGmkPistonTopDeadWait_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK gmsGmkPistonWork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        gmsGmkPistonWork.timer_dec = gmsGmkPistonWork.timer_set_move;
        AppMain.gmGmkPistonShrink(obj_work);
    }

    private static void gmGmkPistonShrink(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK gmsGmkPistonWork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        obj_work.spd.y = gmsGmkPistonWork.piston_vect == (ushort)0 ? -gmsGmkPistonWork.stroke_spd : gmsGmkPistonWork.stroke_spd;
        obj_work.pos.y += obj_work.spd.y;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonShrink_100);
        AppMain.gmGmkPistonShrink_100(obj_work);
        AppMain.GmSoundPlaySE("Piston1");
    }

    private static void gmGmkPistonShrink_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK gmsGmkPistonWork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        gmsGmkPistonWork.timer_dec -= gmsGmkPistonWork.stroke_spd;
        if (gmsGmkPistonWork.timer_dec > 0)
            return;
        obj_work.spd.y = 0;
        if (gmsGmkPistonWork.timer_dec < 0)
        {
            obj_work.spd.y = gmsGmkPistonWork.timer_dec;
            if (gmsGmkPistonWork.piston_vect != (ushort)32768)
                obj_work.spd.y = -obj_work.spd.y;
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonShrink_200);
    }

    private static void gmGmkPistonShrink_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTON_WORK pwork = (AppMain.GMS_GMK_PISTON_WORK)obj_work;
        uint num = AppMain.gmGmkPistonSyncTimeGet(pwork);
        pwork.timer_dec = num > (uint)pwork.timer_set_wait_lower ? 0 : (int)((long)pwork.timer_set_wait_lower - (long)(num - 1U));
        AppMain.gmGmkPistonStay(obj_work);
    }

    private static void gmGmkPistonRodStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PISTONROD_WORK gmkPistonrodWork = (AppMain.GMS_GMK_PISTONROD_WORK)obj_work;
        int num = AppMain.MTM_MATH_ABS(obj_work.parent_obj.pos.y - gmkPistonrodWork.fulcrum) / 8;
        obj_work.scale.y = num;
    }

    private static void gmGmkPistonRod_Create(AppMain.OBS_OBJECT_WORK parent_obj)
    {
        AppMain.GMS_GMK_PISTONROD_WORK work = (AppMain.GMS_GMK_PISTONROD_WORK)AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_PISTONROD_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "Gmk_PistonRod");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_piston_obj_3d_list[1], work.eff_work.obj_3d);
        obj_work.parent_obj = parent_obj;
        obj_work.parent_ofst.x = 0;
        obj_work.parent_ofst.y = 65536;
        obj_work.parent_ofst.z = -524288;
        obj_work.dir.z = (ushort)((uint)parent_obj.dir.z ^ 32768U);
        if (obj_work.dir.z == (ushort)0)
            obj_work.parent_ofst.y = -obj_work.parent_ofst.y;
        obj_work.flag |= 1024U;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        obj_work.disp_flag &= 4294967039U;
        obj_work.flag |= 2U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPistonRodStay);
        work.fulcrum = parent_obj.pos.y + obj_work.parent_ofst.y;
    }

    private static AppMain.OBS_OBJECT_WORK gmGmkPistonInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_PISTON_WORK work = (AppMain.GMS_GMK_PISTON_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_PISTON_WORK()), "Gmk_PistonRod");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_piston_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -131072;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        obj_work.disp_flag &= 4294967039U;
        obj_work.flag |= 2U;
        work.stroke_spd = 16384;
        work.timer_set_move = 524288;
        if (((int)eve_rec.flag & 31) != 0)
        {
            int num = ((int)eve_rec.flag & 31) > 16 ? -((int)eve_rec.flag & 15) << 10 : ((int)eve_rec.flag & 31) << 10;
            work.stroke_spd += num;
        }
        work.efct_di = ((int)eve_rec.flag & 128) != 0;
        work.timer_set_wait_upper = (int)eve_rec.left * 2;
        work.timer_set_wait_lower = (int)eve_rec.height * 2;
        return obj_work;
    }

}