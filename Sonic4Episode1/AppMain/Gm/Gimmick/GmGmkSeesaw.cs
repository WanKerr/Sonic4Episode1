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
    private static AppMain.OBS_OBJECT_WORK GmGmkSeesaw0Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = AppMain.gmGmkSeesawInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSeesawWork.tilt = (short)0;
        AppMain.gmGmkSeesawStart(gmsGmkSeesawWork.gmk_work.ene_com.obj_work);
        return gmsGmkSeesawWork.gmk_work.ene_com.obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSeesaw30Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = AppMain.gmGmkSeesawInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSeesawWork.tilt = (short)4608;
        AppMain.gmGmkSeesawStart(gmsGmkSeesawWork.gmk_work.ene_com.obj_work);
        return gmsGmkSeesawWork.gmk_work.ene_com.obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSeesaw330Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = AppMain.gmGmkSeesawInit(eve_rec, pos_x, pos_y, type);
        gmsGmkSeesawWork.tilt = (short)-4608;
        AppMain.gmGmkSeesawStart(gmsGmkSeesawWork.gmk_work.ene_com.obj_work);
        return gmsGmkSeesawWork.gmk_work.ene_com.obj_work;
    }

    public static void GmGmkSeesawBuild()
    {
        AppMain.gm_gmk_seesaw_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(876), AppMain.GmGameDatGetGimmickData(877), 0U);
        for (int index = 0; index < 16; ++index)
            AppMain.seesaw_alive[index] = (short)0;
        AppMain.control_right = (AppMain.GMS_GMK_SEESAW_WORK)null;
    }

    public static void GmGmkSeesawFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(876);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_seesaw_obj_3d_list, gimmickData.file_num);
    }

    private static void gmGmkSeesawExitTCB(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GMK_SEESAW_WORK tcbWork = (AppMain.GMS_GMK_SEESAW_WORK)AppMain.mtTaskGetTcbWork(tcb);
        --AppMain.seesaw_alive[(int)tcbWork.seesaw_id];
        if (AppMain.control_right == tcbWork)
        {
            AppMain.control_right = (AppMain.GMS_GMK_SEESAW_WORK)null;
            AppMain.lock_seesaw_id = (ushort)0;
        }
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkSeesawStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = (AppMain.GMS_GMK_SEESAW_WORK)obj_work;
        gmsGmkSeesawWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec1 = gmsGmkSeesawWork.gmk_work.ene_com.rect_work[2];
        pRec1.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
        pRec1.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec1, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec1, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec1, (short)-48, (short)-24, (short)48, (short)0);
        AppMain.OBS_RECT_WORK pRec2 = gmsGmkSeesawWork.gmk_work.ene_com.rect_work[0];
        pRec2.flag |= 4U;
        pRec2.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
        pRec2.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectWorkSet(pRec2, (short)-2, (short)-2, (short)2, (short)2);
        obj_work.flag &= 4294967293U;
        gmsGmkSeesawWork.initial_tilt = gmsGmkSeesawWork.tilt;
        gmsGmkSeesawWork.tilt_d = (short)0;
        gmsGmkSeesawWork.tilt_se_timer = (short)0;
        if (AppMain.seesaw_alive[(int)gmsGmkSeesawWork.seesaw_id] == (short)0)
            AppMain.seesaw_tilt[(int)gmsGmkSeesawWork.seesaw_id] = gmsGmkSeesawWork.tilt;
        else
            gmsGmkSeesawWork.tilt = AppMain.seesaw_tilt[(int)gmsGmkSeesawWork.seesaw_id];
        obj_work.dir.z = (ushort)gmsGmkSeesawWork.tilt;
        ++AppMain.seesaw_alive[(int)gmsGmkSeesawWork.seesaw_id];
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkSeesawExitTCB));
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSeesawStay);
    }

    private static void gmGmkSeesawStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = (AppMain.GMS_GMK_SEESAW_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if ((int)AppMain.lock_seesaw_id == (int)gmsGmkSeesawWork.seesaw_id)
        {
            gmsGmkSeesawWork.tilt = AppMain.seesaw_tilt[(int)gmsGmkSeesawWork.seesaw_id];
            obj_work.dir.z = (ushort)gmsGmkSeesawWork.tilt;
            gmsGmkSeesawWork.tilt_timer = (short)60;
        }
        else if ((int)gmsGmkSeesawWork.tilt != (int)gmsGmkSeesawWork.initial_tilt)
        {
            if (gmsGmkSeesawWork.tilt_timer <= (short)0)
            {
                if ((int)gmsGmkSeesawWork.tilt > (int)gmsGmkSeesawWork.initial_tilt)
                {
                    gmsGmkSeesawWork.tilt -= (short)256;
                    if ((int)gmsGmkSeesawWork.tilt < (int)gmsGmkSeesawWork.initial_tilt)
                        gmsGmkSeesawWork.tilt = gmsGmkSeesawWork.initial_tilt;
                }
                else if ((int)gmsGmkSeesawWork.tilt < (int)gmsGmkSeesawWork.initial_tilt)
                {
                    gmsGmkSeesawWork.tilt += (short)256;
                    if ((int)gmsGmkSeesawWork.tilt > (int)gmsGmkSeesawWork.initial_tilt)
                        gmsGmkSeesawWork.tilt = gmsGmkSeesawWork.initial_tilt;
                }
            }
            else
                --gmsGmkSeesawWork.tilt_timer;
        }
        obj_work.dir.z = (ushort)gmsGmkSeesawWork.tilt;
        if (gmsGmkSeesawWork.tilt == (short)0)
            gmsGmkSeesawWork.tilt = (short)0;
        if (((int)gmsPlayerWork.player_flag & 1024) != 0 || ((int)gmsPlayerWork.obj_work.flag & 2) != 0 || ((int)gmsPlayerWork.obj_work.move_flag & 16) != 0 && gmsPlayerWork.obj_work.spd.y < 0)
            return;
        int num1 = AppMain.mtMathSin((int)(ushort)gmsGmkSeesawWork.tilt) * 29;
        int num2 = AppMain.mtMathCos((int)(ushort)gmsGmkSeesawWork.tilt) * 29;
        int num3 = AppMain.mtMathCos((int)(ushort)gmsGmkSeesawWork.tilt) * 112 / 2;
        int num4 = AppMain.MTM_MATH_ABS(AppMain.mtMathSin((int)(ushort)gmsGmkSeesawWork.tilt) * 112 / 2);
        int num5 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        if (num5 < -(num3 + 131072 - num1) || num5 > num3 + 131072 + num1)
            return;
        int num6 = 0;
        int num7 = 0;
        long num8;
        long num9;
        if (gmsPlayerWork.obj_work.move.x == 0)
        {
            if (gmsPlayerWork.obj_work.move.y == 0)
                return;
            num8 = (long)gmsPlayerWork.obj_work.pos.x;
            num9 = 0L;
        }
        else if (gmsPlayerWork.obj_work.move.y == 0)
        {
            num8 = 0L;
            num9 = (long)gmsPlayerWork.obj_work.pos.y;
        }
        else
        {
            num8 = (long)(gmsPlayerWork.obj_work.move.y << 12) / (long)gmsPlayerWork.obj_work.move.x;
            long num10 = num8 * (long)gmsPlayerWork.obj_work.pos.x >> 12;
            num9 = (long)gmsPlayerWork.obj_work.pos.y - num10;
        }
        long num11 = (long)AppMain.mtMathCos((int)(ushort)gmsGmkSeesawWork.tilt);
        long num12 = ((long)AppMain.mtMathSin((int)(ushort)gmsGmkSeesawWork.tilt) << 12) / num11;
        long num13 = num12 * (long)(obj_work.pos.x + num1) >> 12;
        long num14 = (long)(obj_work.pos.y - num2) - num13;
        if (num8 != 0L && num8 == num12)
        {
            num6 = gmsPlayerWork.obj_work.pos.x;
            num7 = (int)((num12 * (long)num6 >> 12) + num14);
        }
        else if (num8 != 0L && num9 != 0L)
        {
            num6 = (int)((num14 - num9 << 12) / (num8 - num12));
            num7 = (int)((num12 * (long)num6 >> 12) + num14);
        }
        else if (num8 == 0L)
        {
            num7 = (int)num9;
            num6 = num12 == 0L ? gmsPlayerWork.obj_work.pos.x : (int)(((long)num7 - num14 << 12) / num12);
        }
        else if (num9 == 0L)
        {
            num6 = (int)num8;
            num7 = (int)((num12 * num8 >> 12) + num14);
        }
        if (obj_work.pos.x - (num3 - num1) > num6 || num6 > obj_work.pos.x + (num3 + num1) || (obj_work.pos.y + (num4 - num2) + 2 < num7 || num7 < obj_work.pos.y - (num4 + num2) - 2))
            return;
        int a1 = gmsPlayerWork.obj_work.pos.x - gmsPlayerWork.obj_work.move.x;
        int a2 = gmsPlayerWork.obj_work.pos.y - gmsPlayerWork.obj_work.move.y;
        int x = gmsPlayerWork.obj_work.pos.x;
        int y = gmsPlayerWork.obj_work.pos.y;
        if (a1 < x)
            AppMain.MTM_MATH_SWAP<int>(ref a1, ref x);
        if (a2 < y)
            AppMain.MTM_MATH_SWAP<int>(ref a2, ref y);
        int num15 = a2 + 1280;
        int num16 = y - 1280;
        if (x > num6 || num6 > a1 || (num15 < num7 || num7 < num16))
            return;
        gmsGmkSeesawWork.ply_work = gmsPlayerWork;
        gmsGmkSeesawWork.hold_x = num6 - num1;
        gmsGmkSeesawWork.hold_y = num7;
        if (AppMain.control_right == null)
            AppMain.gmGmkSeesaw_PlayerHold(obj_work);
        else
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSeesaw_PlayerHold);
    }

    private static void gmGmkSeesaw_PlayerHold(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = (AppMain.GMS_GMK_SEESAW_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK plyWork = gmsGmkSeesawWork.ply_work;
        if (AppMain.control_right != null)
        {
            if (AppMain.control_right.gmk_work.ene_com.obj_work.pos.x >= obj_work.pos.x && plyWork.obj_work.spd.x >= 0 || AppMain.control_right.gmk_work.ene_com.obj_work.pos.x <= obj_work.pos.x && plyWork.obj_work.spd.x <= 0)
            {
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSeesawStay);
                AppMain.gmGmkSeesawStay(obj_work);
                return;
            }
            gmsGmkSeesawWork.player_speed = AppMain.control_right.player_speed;
        }
        else
            gmsGmkSeesawWork.player_speed = plyWork.obj_work.move.x / 2;
        AppMain.GmPlySeqGmkInitSeesaw(plyWork, gmsGmkSeesawWork.gmk_work.ene_com);
        long num = (long)(gmsGmkSeesawWork.hold_x - obj_work.pos.x << 12) / (long)AppMain.mtMathCos((int)gmsGmkSeesawWork.tilt);
        gmsGmkSeesawWork.player_distance = num;
        AppMain.control_right = gmsGmkSeesawWork;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSeesaw_PlayerHold_100);
        AppMain.gmGmkSeesaw_PlayerHold_100(obj_work);
    }

    private static void gmGmkSeesaw_PlayerHold_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SEESAW_WORK gmsGmkSeesawWork = (AppMain.GMS_GMK_SEESAW_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK plyWork = gmsGmkSeesawWork.ply_work;
        AppMain.lock_seesaw_id = (ushort)0;
        if (plyWork.seq_state != 44 || AppMain.control_right != gmsGmkSeesawWork || ((int)plyWork.obj_work.move_flag & 256) != 0)
        {
            gmsGmkSeesawWork.tilt_d = (short)0;
            gmsGmkSeesawWork.tilt_se_timer = (short)0;
            if (AppMain.control_right == gmsGmkSeesawWork)
                AppMain.control_right = (AppMain.GMS_GMK_SEESAW_WORK)null;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSeesawStay);
            AppMain.gmGmkSeesawStay(obj_work);
        }
        else
        {
            AppMain.lock_seesaw_id = gmsGmkSeesawWork.seesaw_id;
            gmsGmkSeesawWork.tilt_timer = (short)60;
            AppMain.control_right = gmsGmkSeesawWork;
            int num1 = AppMain.GmPlayerKeyGetGimmickRotZ(gmsGmkSeesawWork.ply_work);
            if (num1 > 256)
                num1 = 256;
            if (num1 < -256)
                num1 = -256;
            gmsGmkSeesawWork.tilt_d = (short)num1;
            if (gmsGmkSeesawWork.tilt_d != (short)0)
            {
                gmsGmkSeesawWork.tilt += gmsGmkSeesawWork.tilt_d;
                if (gmsGmkSeesawWork.tilt_se_timer == (short)0)
                {
                    AppMain.GmSoundPlaySE("Seesaw");
                    gmsGmkSeesawWork.tilt_se_timer = (short)8;
                }
                --gmsGmkSeesawWork.tilt_se_timer;
                if (gmsGmkSeesawWork.tilt >= (short)4608)
                {
                    gmsGmkSeesawWork.tilt = (short)4608;
                    gmsGmkSeesawWork.tilt_d = (short)0;
                }
                if (gmsGmkSeesawWork.tilt <= (short)-4608)
                {
                    gmsGmkSeesawWork.tilt = (short)-4608;
                    gmsGmkSeesawWork.tilt_d = (short)0;
                }
            }
            obj_work.dir.z = (ushort)gmsGmkSeesawWork.tilt;
            AppMain.seesaw_tilt[(int)gmsGmkSeesawWork.seesaw_id] = (short)obj_work.dir.z;
            if (((int)plyWork.obj_work.move_flag & 4) != 0)
            {
                int num2 = AppMain.mtMathSin((int)(ushort)gmsGmkSeesawWork.tilt) * 29;
                int num3 = obj_work.pos.x + num2;
                long num4 = (long)(plyWork.obj_work.pos.x - num3 << 12) / (long)AppMain.mtMathCos((int)gmsGmkSeesawWork.tilt);
                gmsGmkSeesawWork.player_distance = num4;
                if (((int)plyWork.obj_work.disp_flag & 1) == 0 && gmsGmkSeesawWork.player_speed > 0 || ((int)plyWork.obj_work.disp_flag & 1) != 0 && gmsGmkSeesawWork.player_speed < 0)
                    gmsGmkSeesawWork.player_speed = 0;
            }
            long playerDistance = gmsGmkSeesawWork.player_distance;
            gmsGmkSeesawWork.player_distance += (long)gmsGmkSeesawWork.player_speed;
            long num5 = gmsGmkSeesawWork.player_distance - playerDistance;
            gmsGmkSeesawWork.player_speed += 256 * AppMain.mtMathSin((int)(ushort)gmsGmkSeesawWork.tilt) >> 12;
            int num6 = AppMain.mtMathSin((int)(ushort)gmsGmkSeesawWork.tilt) * 29;
            int num7 = obj_work.pos.x + num6;
            int num8 = AppMain.mtMathCos((int)(ushort)gmsGmkSeesawWork.tilt) * 29;
            int num9 = obj_work.pos.y - num8;
            int num10 = (int)(gmsGmkSeesawWork.player_distance * (long)AppMain.mtMathCos((int)(ushort)gmsGmkSeesawWork.tilt) >> 12);
            int num11 = num7 + num10;
            int num12 = (int)(gmsGmkSeesawWork.player_distance * (long)AppMain.mtMathSin((int)(ushort)gmsGmkSeesawWork.tilt) >> 12);
            int num13 = num9 + num12;
            if (gmsGmkSeesawWork.player_speed < 0 && ((int)plyWork.obj_work.disp_flag & 1) == 0)
                plyWork.obj_work.disp_flag |= 1U;
            else if (gmsGmkSeesawWork.player_speed > 0 && ((int)plyWork.obj_work.disp_flag & 1) != 0)
                plyWork.obj_work.disp_flag &= 4294967294U;
            if (gmsGmkSeesawWork.player_distance <= 231424L && gmsGmkSeesawWork.player_distance >= -231424L)
            {
                plyWork.obj_work.spd.x = num11 - plyWork.obj_work.pos.x;
                plyWork.obj_work.spd.y = num13 - plyWork.obj_work.pos.y;
            }
            else
            {
                int num2 = (int)(num5 * (long)AppMain.mtMathCos((int)(ushort)gmsGmkSeesawWork.tilt) >> 12);
                int spdy = (int)(num5 * (long)AppMain.mtMathSin((int)(ushort)gmsGmkSeesawWork.tilt) >> 12);
                if (AppMain.MTM_MATH_ABS(num2) < 256)
                    num2 = num2 >= 0 ? 1024 : -1024;
                AppMain.GmPlySeqGmkInitSeesawEnd(plyWork, num2, spdy);
                gmsGmkSeesawWork.tilt_d = (short)0;
                AppMain.control_right = (AppMain.GMS_GMK_SEESAW_WORK)null;
                AppMain.lock_seesaw_id = (ushort)0;
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSeesawStay);
            }
        }
    }

    private static void gmGmkSeesaw_CreateParts(AppMain.GMS_GMK_SEESAW_WORK pwork)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)pwork;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SEESAWPARTS_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "Gmk_SeesawParts");
        AppMain.GMS_GMK_SEESAWPARTS_WORK gmkSeesawpartsWork = (AppMain.GMS_GMK_SEESAWPARTS_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_seesaw_obj_3d_list[1], gmkSeesawpartsWork.eff_work.obj_3d);
        work.parent_obj = obsObjectWork;
        work.flag &= 4294966271U;
        work.pos.x = obsObjectWork.pos.x;
        work.pos.y = obsObjectWork.pos.y;
        work.pos.z = obsObjectWork.pos.z + 4096;
        work.disp_flag |= 4194304U;
        work.disp_flag |= 256U;
        work.flag |= 2U;
        work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static AppMain.GMS_GMK_SEESAW_WORK gmGmkSeesawInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.GMS_GMK_SEESAW_WORK work = (AppMain.GMS_GMK_SEESAW_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SEESAW_WORK()), "Gmk_Seesaw");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_seesaw_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -4096;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        work.seesaw_id = (ushort)eve_rec.left;
        AppMain.gmGmkSeesaw_CreateParts(work);
        return work;
    }

}