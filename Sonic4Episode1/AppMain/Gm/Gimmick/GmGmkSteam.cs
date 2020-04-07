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
    private static void gmGmkSteamGateHit(
     AppMain.OBS_RECT_WORK mine_rect,
     AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 == AppMain.g_gm_main_system.ply_work[0] && ((int)parentObj2.player_flag & 1024) == 0)
        {
            AppMain.GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (AppMain.GMS_GMK_STEAMP_WORK)parentObj1;
            parentObj2.obj_work.pos.y = parentObj1.pos.y;
            AppMain.GmPlySeqInitSteamPipeIn(parentObj2);
            gmsGmkSteampWork.status = (byte)1;
            gmsGmkSteampWork.ply_work = parentObj2;
            parentObj1.flag |= 2U;
            AppMain.GMM_PAD_VIB_SMALL_TIME(60f);
        }
        mine_rect.flag &= 4294573823U;
    }

    private static void gmGmkSteamExitHit(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 != AppMain.g_gm_main_system.ply_work[0] || parentObj2.seq_state != 57 || ((int)parentObj2.player_flag & 1024) != 0)
            return;
        AppMain.GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (AppMain.GMS_GMK_STEAMP_WORK)parentObj1;
        parentObj2.obj_work.spd.x = parentObj2.obj_work.spd.y = 0;
        gmsGmkSteampWork.status = (byte)1;
        gmsGmkSteampWork.ply_work = parentObj2;
        parentObj1.flag |= 2U;
    }

    private static void gmGmkSteamCrankCheck(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (gmsPlayerWork.obj_work.pos.x < obj_work.pos.x + 65536 && gmsPlayerWork.obj_work.pos.x > obj_work.pos.x - 65536 && (gmsPlayerWork.obj_work.pos.y < obj_work.pos.y + 65536 && gmsPlayerWork.obj_work.pos.y > obj_work.pos.y - 65536))
        {
            if (((int)gmsPlayerWork.player_flag & 1024) != 0 || gmsPlayerWork.seq_state != 57 || obj_work.user_flag != 0U)
                return;
            AppMain.GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (AppMain.GMS_GMK_STEAMP_WORK)obj_work;
            if (gmsPlayerWork.obj_work.spd.x != 0)
            {
                int x = gmsPlayerWork.obj_work.spd.x;
                int num = gmsPlayerWork.obj_work.pos.x - x;
                if ((x <= 0 || ((int)AppMain.tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 2) == 0 || (num > obj_work.pos.x || obj_work.pos.x > gmsPlayerWork.obj_work.pos.x)) && (x >= 0 || ((int)AppMain.tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 1) == 0 || (gmsPlayerWork.obj_work.pos.x > obj_work.pos.x || obj_work.pos.x > num)))
                    return;
                int a = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
                gmsPlayerWork.obj_work.pos.x = obj_work.pos.x;
                gmsPlayerWork.obj_work.spd.x = 0;
                gmsPlayerWork.obj_work.pos.y = obj_work.pos.y;
                if (((int)AppMain.tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 8) == 0)
                {
                    gmsPlayerWork.obj_work.spd.y = -61440;
                    gmsPlayerWork.obj_work.pos.y -= AppMain.MTM_MATH_ABS(a);
                }
                else
                {
                    gmsPlayerWork.obj_work.spd.y = 61440;
                    gmsPlayerWork.obj_work.pos.y += AppMain.MTM_MATH_ABS(a);
                }
                obj_work.flag |= 2U;
                AppMain.GmSoundPlaySE("PipeMoving");
                obj_work.user_flag = 1U;
            }
            else
            {
                int y = gmsPlayerWork.obj_work.spd.y;
                int num = gmsPlayerWork.obj_work.pos.y - y;
                if ((y <= 0 || ((int)AppMain.tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 4) == 0 || (num > obj_work.pos.y || obj_work.pos.y > gmsPlayerWork.obj_work.pos.y)) && (y >= 0 || ((int)AppMain.tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 8) == 0 || (gmsPlayerWork.obj_work.pos.y > obj_work.pos.y || obj_work.pos.y > num)))
                    return;
                int a = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
                gmsPlayerWork.obj_work.pos.y = obj_work.pos.y;
                gmsPlayerWork.obj_work.spd.y = 0;
                gmsPlayerWork.obj_work.pos.x = obj_work.pos.x;
                if (((int)AppMain.tbl_gmk_pipe_vect[gmsGmkSteampWork.obj_type] & 2) == 0)
                {
                    gmsPlayerWork.obj_work.spd.x = 61440;
                    gmsPlayerWork.obj_work.pos.x += AppMain.MTM_MATH_ABS(a);
                }
                else
                {
                    gmsPlayerWork.obj_work.spd.x = -61440;
                    gmsPlayerWork.obj_work.pos.x -= AppMain.MTM_MATH_ABS(a);
                }
                obj_work.flag |= 2U;
                AppMain.GmSoundPlaySE("PipeMoving");
                obj_work.user_flag = 1U;
            }
        }
        else
            obj_work.user_flag = 0U;
    }

    private static void gmGmkSteamPipeStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        switch (((AppMain.GMS_GMK_STEAMP_WORK)obj_work).obj_type)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                AppMain.gmGmkSteamCrankCheck(obj_work);
                break;
            case 4:
            case 5:
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSteamPipeStay_100);
                break;
            case 6:
            case 7:
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSteamPipeStay_Exit);
                break;
            default:
                obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
                break;
        }
    }

    private static void gmGmkSteamPipeStay_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (AppMain.GMS_GMK_STEAMP_WORK)obj_work;
        if (gmsGmkSteampWork.status == (byte)0)
            return;
        switch (gmsGmkSteampWork.obj_type)
        {
            case 4:
            case 5:
                gmsGmkSteampWork.timer = (short)60;
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSteamPipe_GateIn);
                AppMain.GmSoundPlaySE("PipeIn");
                break;
            case 6:
            case 7:
                gmsGmkSteampWork.timer = (short)0;
                AppMain.gmGmkSteamPipe_GateOutColClear(obj_work);
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSteamPipe_GateOut);
                break;
        }
    }

    private static void gmGmkSteamPipeStay_Exit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkSteamPipeStay_100(obj_work);
    }

    private static void gmGmkSteamPipe_GateIn(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (AppMain.GMS_GMK_STEAMP_WORK)obj_work;
        --gmsGmkSteampWork.timer;
        if (gmsGmkSteampWork.timer > (short)0)
            return;
        int spd_x = gmsGmkSteampWork.obj_type != 4 ? -61440 : 61440;
        gmsGmkSteampWork.ply_work.obj_work.move_flag |= 16U;
        AppMain.GmPlySeqGmkSpdSet(gmsGmkSteampWork.ply_work, spd_x, 0);
        gmsGmkSteampWork.ply_work.gmk_flag2 |= 6U;
        AppMain.GmSoundPlaySE("PipeMoving");
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmGmkSteamPipe_GateOut(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (AppMain.GMS_GMK_STEAMP_WORK)obj_work;
        --gmsGmkSteampWork.timer;
        if (gmsGmkSteampWork.timer > (short)0)
            return;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate(obj_work, 92);
        obsObjectWork.pos.x = obj_work.pos.x;
        obsObjectWork.pos.y = obj_work.pos.y;
        int spd_x;
        if (((int)obj_work.user_flag & 1) == 0)
        {
            spd_x = 61440;
            obsObjectWork.dir.z = (ushort)16384;
            obsObjectWork.pos.x += 229376;
        }
        else
        {
            spd_x = -61440;
            obsObjectWork.dir.z = (ushort)49152;
            obsObjectWork.pos.x -= 229376;
        }
        AppMain.GmPlySeqInitSteamPipeOut(gmsGmkSteampWork.ply_work, spd_x);
        AppMain.GmSoundPlaySE("PipeOut");
        gmsGmkSteampWork.timer = (short)8;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSteamPipe_GateOut_100);
    }

    private static void gmGmkSteamPipe_GateOut_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (AppMain.GMS_GMK_STEAMP_WORK)obj_work;
        --gmsGmkSteampWork.timer;
        if (gmsGmkSteampWork.timer > (short)0)
            return;
        AppMain.gmGmkSteamPipe_GateOutColSet(obj_work);
        gmsGmkSteampWork.status = (byte)0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSteamPipeStay_Exit);
    }

    private static void gmGmkSteamPipe_GateOutColSet(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 2) != 0)
            return;
        AppMain.GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (AppMain.GMS_GMK_STEAMP_WORK)obj_work;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)64;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)64;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.ofst_x = (short)0;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.ofst_y = (short)-32;
        gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.dir = (ushort)0;
    }

    private static void gmGmkSteamPipe_GateOutColClear(AppMain.OBS_OBJECT_WORK obj_work)
    {
        ((AppMain.GMS_GMK_STEAMP_WORK)obj_work).gmk_work.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmGmkSteamPipeStart(AppMain.OBS_OBJECT_WORK obj_work, int type)
    {
        AppMain.GMS_GMK_STEAMP_WORK gmsGmkSteampWork = (AppMain.GMS_GMK_STEAMP_WORK)obj_work;
        if (type >= 4 && type < 8)
        {
            gmsGmkSteampWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
            gmsGmkSteampWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
            AppMain.OBS_RECT_WORK pRec = gmsGmkSteampWork.gmk_work.ene_com.rect_work[2];
            pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
            if (type < 6)
            {
                pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkSteamGateHit);
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)32;
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)16;
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.ofst_x = (short)-14;
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.ofst_y = (short)-34;
                gmsGmkSteampWork.gmk_work.ene_com.col_work.obj_col.dir = (ushort)0;
            }
            else
                pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkSteamExitHit);
            AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
            AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
            AppMain.ObjRectWorkSet(pRec, AppMain.tbl_gm_gmk_steampipe_rect[type][0], AppMain.tbl_gm_gmk_steampipe_rect[type][1], AppMain.tbl_gm_gmk_steampipe_rect[type][2], AppMain.tbl_gm_gmk_steampipe_rect[type][3]);
            pRec.flag |= 4U;
            obj_work.flag &= 4294967293U;
        }
        gmsGmkSteampWork.obj_type = type;
        gmsGmkSteampWork.status = (byte)0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSteamPipeStay);
    }

    public static AppMain.OBS_OBJECT_WORK gmGmkSteamPipeInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      ushort model)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_STEAMP_WORK()), "Gmk_SteamPipe");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_steampipe_obj_3d_list[(int)model], gmsEnemy3DWork.obj_3d);
        work.pos.z = (int)eve_rec.left * 8 * 4096;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 2U;
        return work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeGateRInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, (ushort)1);
        obj_work.disp_flag |= 4194304U;
        obj_work.pos.z += 65536;
        AppMain.gmGmkSteamPipeStart(obj_work, 4);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeGateLInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, (ushort)0);
        obj_work.pos.z += 65536;
        AppMain.gmGmkSteamPipeStart(obj_work, 5);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeGateEInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model;
        int type1;
        if (((int)eve_rec.flag & 1) == 0)
        {
            model = (ushort)2;
            type1 = 6;
        }
        else
        {
            model = (ushort)3;
            type1 = 7;
        }
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        obj_work.pos.z += 131072;
        obj_work.user_flag = (uint)eve_rec.flag;
        if (((int)obj_work.user_flag & 1) == 0)
            obj_work.pos.x -= 131072;
        else
            obj_work.pos.x += 131072;
        AppMain.gmGmkSteamPipe_GateOutColSet(obj_work);
        AppMain.gmGmkSteamPipeStart(obj_work, type1);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeA1Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 4;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = (ushort)4;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        AppMain.gmGmkSteamPipeStart(obj_work, 9);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeA2Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 5;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = (ushort)5;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        AppMain.gmGmkSteamPipeStart(obj_work, 8);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeA3Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 4;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = (ushort)4;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        AppMain.gmGmkSteamPipeStart(obj_work, 9);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeA4Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 6;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = (ushort)5;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        AppMain.gmGmkSteamPipeStart(obj_work, 8);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeB1Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 7;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = (ushort)6;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        AppMain.gmGmkSteamPipeStart(obj_work, 9);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeB2Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 8;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = (ushort)7;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        AppMain.gmGmkSteamPipeStart(obj_work, 8);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeB3Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 7;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = (ushort)6;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        AppMain.gmGmkSteamPipeStart(obj_work, 9);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeB4Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 9;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = (ushort)7;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        AppMain.gmGmkSteamPipeStart(obj_work, 8);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeJ1Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 10;
        ushort num = 0;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
        {
            model = (ushort)9;
            num = (ushort)16384;
        }
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        obj_work.dir.z = num;
        AppMain.gmGmkSteamPipeStart(obj_work, 0);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeJ2Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 11;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = (ushort)8;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        AppMain.gmGmkSteamPipeStart(obj_work, 1);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeJ3Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 12;
        ushort num = 0;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
        {
            model = (ushort)8;
            num = (ushort)16384;
        }
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        obj_work.dir.z = num;
        AppMain.gmGmkSteamPipeStart(obj_work, 2);
        return obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkSteamPipeJ4Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort model = 13;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
            model = (ushort)9;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmGmkSteamPipeInit(eve_rec, pos_x, pos_y, type, model);
        AppMain.gmGmkSteamPipeStart(obj_work, 3);
        return obj_work;
    }

    public static void GmGmkSteamPipeBuild()
    {
        AppMain.gm_gmk_steampipe_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(921), AppMain.GmGameDatGetGimmickData(922), 0U);
    }

    public static void GmGmkSteamPipeFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(921);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_steampipe_obj_3d_list, gimmickData.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkStartInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 4) == 0)
        {
            AppMain.g_gm_main_system.resume_pos_x = pos_x;
            AppMain.g_gm_main_system.resume_pos_y = pos_y - 4096;
        }
        eve_rec.pos_x = byte.MaxValue;
        AppMain.GmCameraPosSet(AppMain.g_gm_main_system.resume_pos_x, AppMain.g_gm_main_system.resume_pos_y, 0);
        AppMain.OBS_CAMERA obj_camera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        AppMain.ObjObjectCameraSet(AppMain.FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - (float)((int)AppMain.OBD_LCD_X / 2)), AppMain.FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - (float)((int)AppMain.OBD_LCD_Y / 2)), AppMain.FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - (float)((int)AppMain.OBD_LCD_X / 2)), AppMain.FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - (float)((int)AppMain.OBD_LCD_Y / 2)));
        AppMain.GmCameraSetClipCamera(obj_camera);
        return (AppMain.OBS_OBJECT_WORK)null;
    }

}