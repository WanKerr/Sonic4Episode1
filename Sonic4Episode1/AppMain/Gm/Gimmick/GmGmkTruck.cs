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
    public static void GmGmkTruckBuild()
    {
        AppMain.gm_gmk_truck_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(928), AppMain.GmGameDatGetGimmickData(929), 0U);
    }

    public static void GmGmkTruckFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(928);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_truck_obj_3d_list, gimmickData.file_num);
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkTruckInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_TRUCK_WORK()), "GMK_TRUCK");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_GMK_TRUCK_WORK truck_work = (AppMain.GMS_GMK_TRUCK_WORK)work;
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkTruckDest));
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_truck_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(930), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectActionSet(work, 0);
        AppMain.ObjCopyAction3dNNModel(AppMain.gm_gmk_truck_obj_3d_list[1], truck_work.obj_3d_tire);
        gmsEnemy3DWork.obj_3d.mtn_cb_func = new AppMain.mtn_cb_func_delegate(AppMain.gmGmkTruckMotionCallback);
        gmsEnemy3DWork.obj_3d.mtn_cb_param = (object)truck_work;
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTruckDispFunc);
        work.flag |= 1U;
        work.move_flag |= 128U;
        work.disp_flag |= 16777220U;
        work.disp_flag |= 16U;
        work.obj_3d.blend_spd = 0.125f;
        truck_work.trans_r.x = 0.0f;
        truck_work.trans_r.y = 0.0f;
        truck_work.trans_r.z = 4f / AppMain.FXM_FX32_TO_FLOAT(AppMain.g_obj.draw_scale.x);
        AppMain.nnMakeUnitMatrix(gmsEnemy3DWork.obj_3d.user_obj_mtx_r);
        AppMain.nnTranslateMatrix(gmsEnemy3DWork.obj_3d.user_obj_mtx_r, gmsEnemy3DWork.obj_3d.user_obj_mtx_r, truck_work.trans_r.x, truck_work.trans_r.y, truck_work.trans_r.z);
        AppMain.ObjObjectFieldRectSet(work, (short)AppMain.GMD_GMK_TRUCK_FIELD_RECT_LEFT, (short)AppMain.GMD_GMK_TRUCK_FIELD_RECT_TOP, (short)AppMain.GMD_GMK_TRUCK_FIELD_RECT_RIGHT, (short)AppMain.GMD_GMK_TRUCK_FIELD_RECT_BOTTOM);
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkTruckBodyDefFunc);
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectWorkSet(pRec, (short)-64, (short)-64, (short)64, (short)64);
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.x = -0.85f;
        nnsVector.y = -0.45f;
        nnsVector.z = -3.05f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_1, ref col, 1f, nnsVector);
        gmsEnemy3DWork.obj_3d.use_light_flag &= 4294967294U;
        truck_work.obj_3d_tire.use_light_flag &= 4294967294U;
        gmsEnemy3DWork.obj_3d.use_light_flag |= 2U;
        truck_work.obj_3d_tire.use_light_flag |= 2U;
        AppMain.gmGmkTruckCreateLightEfct(truck_work);
        gmsEnemy3DWork.ene_com.enemy_flag |= 65536U;
        work.obj_3d.command_state = 16U;
        return work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkTruckGravityInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_COM_WORK()), "GMK_T_GRAVITY");
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)work;
        work.move_flag |= 8448U;
        work.disp_flag |= 32U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemyComWork.rect_work[2];
        AppMain.ObjRectGroupSet(pRec, (byte)1, (byte)1);
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)1);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        if ((ushort)239 <= eve_rec.id && eve_rec.id <= (ushort)246)
        {
            short[] numArray = (ushort)239 > eve_rec.id || eve_rec.id > (ushort)242 ? AppMain.gm_gmk_t_gravity_rr_rect_tbl[(int)eve_rec.id - 243] : AppMain.gm_gmk_t_gravity_r_rect_tbl;
            AppMain.ObjRectSet(pRec.rect, numArray[0], numArray[1], numArray[2], numArray[3]);
        }
        else
            AppMain.ObjRectSet(pRec.rect, (short)((int)eve_rec.left << 1), (short)((int)eve_rec.top << 1), (short)((int)eve_rec.width + (int)eve_rec.left << 1), (short)((int)eve_rec.height + (int)eve_rec.top << 1));
        pRec.parent_obj = work;
        pRec.flag |= 192U;
        pRec.ppDef = (ushort)268 > eve_rec.id || eve_rec.id > (ushort)271 ? new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkTGravityChangeDefFunc) : new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkTGravityForceChangeDefFunc);
        gmsEnemyComWork.rect_work[1].flag &= 4294967291U;
        gmsEnemyComWork.rect_work[0].flag &= 4294967291U;
        return work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkTruckNoLandingInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_COM_WORK()), "GMK_T_NOLANDING");
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)work;
        work.move_flag |= 8448U;
        work.disp_flag |= 32U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemyComWork.rect_work[2];
        AppMain.ObjRectGroupSet(pRec, (byte)1, (byte)1);
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)1);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectSet(pRec.rect, (short)((int)eve_rec.left << 1), (short)((int)eve_rec.top << 1), (short)((int)eve_rec.width + (int)eve_rec.left << 1), (short)((int)eve_rec.height + (int)eve_rec.top << 1));
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkTNoLandingDefFunc);
        pRec.parent_obj = work;
        pRec.flag |= 192U;
        gmsEnemyComWork.rect_work[1].flag &= 4294967291U;
        gmsEnemyComWork.rect_work[0].flag &= 4294967291U;
        return work;
    }

    public static void gmGmkTruckDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GMK_TRUCK_WORK tcbWork = (AppMain.GMS_GMK_TRUCK_WORK)AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.h_snd_lorry != null)
        {
            AppMain.GmSoundStopSE(tcbWork.h_snd_lorry);
            AppMain.GsSoundFreeSeHandle(tcbWork.h_snd_lorry);
            tcbWork.h_snd_lorry = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.GmEnemyDefaultExit(tcb);
    }

    public static void gmGmkTruckInitMain(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)ply_work);
        AppMain.GMS_GMK_TRUCK_WORK truck_work = (AppMain.GMS_GMK_TRUCK_WORK)obj_work;
        obj_work.flag |= 2U;
        obj_work.move_flag |= 8448U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTruckMain);
        AppMain.gmGmkTruckCreateSparkEfct(truck_work, 27);
        truck_work.h_snd_lorry = AppMain.GsSoundAllocSeHandle();
    }

    public static void gmGmkTruckMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_TRUCK_WORK truck_work = (AppMain.GMS_GMK_TRUCK_WORK)obj_work;
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        if (truck_work.target_player == null)
        {
            AppMain.gmGmkTruckInitDeathFall(obj_work, (AppMain.GMS_PLAYER_WORK)null);
            obj_work.ppFunc(obj_work);
        }
        else if (((int)truck_work.target_player.player_flag & 262144) == 0)
        {
            AppMain.gmGmkTruckInitFree(obj_work, truck_work.target_player);
            obj_work.ppFunc(obj_work);
        }
        else if (((int)truck_work.target_player.gmk_flag2 & 64) != 0)
        {
            AppMain.gmGmkTruckInitDeathFall(obj_work, truck_work.target_player);
            obj_work.ppFunc(obj_work);
        }
        else
        {
            if (((int)truck_work.target_player.player_flag & 1024) != 0)
                obj_work.pos.z = 983040;
            AppMain.GMS_PLAYER_WORK targetPlayer = truck_work.target_player;
            obj_work.prev_pos = obj_work.pos;
            obj_work.pos.x = targetPlayer.obj_work.pos.x;
            obj_work.pos.y = targetPlayer.obj_work.pos.y;
            obj_work.move.x = obj_work.pos.x - obj_work.prev_pos.x;
            obj_work.move.y = obj_work.pos.y - obj_work.prev_pos.y;
            obj_work.move.z = obj_work.pos.z - obj_work.prev_pos.z;
            obj_work.dir = targetPlayer.obj_work.dir;
            obj_work.dir.z += targetPlayer.obj_work.dir_fall;
            obj_work.vib_timer = targetPlayer.obj_work.vib_timer;
            obj_work.disp_flag &= 4294967279U;
            truck_work.tire_dir_spd = ((int)targetPlayer.obj_work.move_flag & 1) == 0 ? AppMain.ObjSpdDownSet(truck_work.tire_dir_spd, 128) : targetPlayer.obj_work.spd_m;
            truck_work.tire_dir += (ushort)AppMain.FX_Div(truck_work.tire_dir_spd, 65536);
            int id = -1;
            uint num = 0;
            if (0 <= targetPlayer.act_state && targetPlayer.act_state <= 7 || (targetPlayer.act_state == 69 || targetPlayer.act_state == 70) || (targetPlayer.act_state == 74 || targetPlayer.act_state == 76 || targetPlayer.act_state == 75))
            {
                id = 3;
                num = 4U;
            }
            else if (71 <= targetPlayer.act_state && targetPlayer.act_state <= 72)
            {
                id = 0;
                num = 4U;
            }
            else if (((int)targetPlayer.obj_work.move_flag & 1) == 0)
            {
                id = 1;
                num = 4U;
            }
            else if (((int)targetPlayer.obj_work.move_flag & 1) != 0 && ((int)targetPlayer.obj_work.move_flag & 4194304) == 0)
                id = 2;
            else if (obj_work.obj_3d.act_id[0] == 2 && ((int)obj_work.disp_flag & 8) != 0)
            {
                id = targetPlayer.obj_work.spd_m == 0 ? 3 : 0;
                num = 4U;
            }
            else if (11 <= targetPlayer.act_state && targetPlayer.act_state <= 16 && (obj_work.obj_3d.act_id[0] != 2 || ((int)obj_work.disp_flag & 8) != 0))
            {
                id = 3;
                num = 4U;
            }
            if (id != -1 && obj_work.obj_3d.act_id[0] != id)
            {
                AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, id);
                obj_work.disp_flag |= num;
            }
            if (obj_work.obj_3d.act_id[0] != 3 && (11 > targetPlayer.act_state || targetPlayer.act_state > 16 || obj_work.obj_3d.act_id[0] != 2))
                obj_work.obj_3d.frame[0] = targetPlayer.obj_work.obj_3d.frame[0];
            truck_work.slope_f_y_dir = (ushort)0;
            truck_work.slope_f_z_dir = (ushort)0;
            truck_work.slope_z_dir = (ushort)0;
            float x;
            float y;
            float z;
            if (((int)targetPlayer.player_flag & 4) == 0)
            {
                x = 0.0f;
                y = 8f;
                z = -5f;
            }
            else
            {
                x = 0.0f;
                y = 8f;
                z = 5f;
            }
            AppMain.nnMakeUnitMatrix(obj_work.obj_3d.user_obj_mtx_r);
            AppMain.nnTranslateMatrix(obj_work.obj_3d.user_obj_mtx_r, obj_work.obj_3d.user_obj_mtx_r, truck_work.trans_r.x, truck_work.trans_r.y, truck_work.trans_r.z);
            if (((int)targetPlayer.gmk_flag & 262144) != 0 && targetPlayer.gmk_work3 != 0)
            {
                AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
                truck_work.slope_z_dir = (ushort)targetPlayer.gmk_work3;
                truck_work.slope_f_z_dir = (ushort)(AppMain.MTM_MATH_ABS(targetPlayer.gmk_work3) >> 2);
                truck_work.slope_f_y_dir = (ushort)(targetPlayer.gmk_work3 >> 2);
                AppMain.nnMakeUnitMatrix(nnsMatrix);
                AppMain.nnTranslateMatrix(nnsMatrix, nnsMatrix, -x, -y, -z);
                AppMain.nnRotateXMatrix(nnsMatrix, nnsMatrix, (int)truck_work.slope_z_dir);
                AppMain.nnRotateYMatrix(nnsMatrix, nnsMatrix, (int)truck_work.slope_f_y_dir);
                AppMain.nnRotateZMatrix(nnsMatrix, nnsMatrix, (int)truck_work.slope_f_z_dir);
                AppMain.nnTranslateMatrix(nnsMatrix, nnsMatrix, x, y, z);
                AppMain.nnMultiplyMatrix(obj_work.obj_3d.user_obj_mtx_r, obj_work.obj_3d.user_obj_mtx_r, nnsMatrix);
                AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
            }
            if (((int)targetPlayer.obj_work.move_flag & 1) != 0 && AppMain.MTM_MATH_ABS(targetPlayer.obj_work.spd_m) >= AppMain.GMD_GMK_TRUCK_SPARK_EFCT_SMALL_MIN_SPD && (truck_work.efct_f_spark == null || truck_work.efct_b_spark == null))
                AppMain.gmGmkTruckCreateSparkEfct(truck_work, 27);
            if (truck_work.h_snd_lorry.au_player.sound == null || truck_work.h_snd_lorry.au_player.sound[0] == null)
            {
                truck_work.h_snd_lorry = AppMain.GsSoundAllocSeHandle();
                truck_work.h_snd_lorry.au_player.SetAisac("Speed", 0.0f);
                AppMain.GmSoundPlaySEForce("Lorry", truck_work.h_snd_lorry, true);
            }
            AppMain.gmGmkTruckSetMoveSeParam(obj_work, truck_work.h_snd_lorry, targetPlayer, ((int)targetPlayer.player_flag & 16777216) != 0 ? 1 : 0);
        }
    }

    public static void gmGmkTruckInitFree(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_PLAYER_WORK ply_work)
    {
        ((AppMain.GMS_GMK_TRUCK_WORK)obj_work).target_player = (AppMain.GMS_PLAYER_WORK)null;
        uint num1;
        uint num2;
        if (ply_work != null)
        {
            obj_work.spd = ply_work.obj_work.spd;
            obj_work.spd_m = ply_work.obj_work.spd_m;
            num1 = ply_work.obj_work.flag;
            num2 = ply_work.obj_work.move_flag;
        }
        else
        {
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
            obj_work.spd.z = 0;
            num1 = 0U;
            num2 = 0U;
        }
        obj_work.flag &= 4294967294U;
        obj_work.flag |= (uint)(2 | (int)num1 & 1);
        obj_work.flag &= 4294967279U;
        obj_work.move_flag |= 192U;
        obj_work.move_flag &= 4294401791U;
        if (((int)num2 & 16) != 0)
        {
            obj_work.move_flag |= 16U;
        }
        else
        {
            if (obj_work.spd.x > obj_work.spd_m)
                obj_work.spd_m = obj_work.spd.x;
            obj_work.spd.x = 0;
            if (obj_work.obj_3d.act_id[0] != 0)
            {
                AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 0);
                obj_work.disp_flag |= 4U;
            }
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTruckFreeMain);
    }

    public static void gmGmkTruckFreeMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_TRUCK_WORK truck_work = (AppMain.GMS_GMK_TRUCK_WORK)obj_work;
        if (((int)obj_work.move_flag & 1) != 0 && ((int)obj_work.move_flag & 16) != 0)
        {
            if (obj_work.spd.x > obj_work.spd_m)
                obj_work.spd_m = obj_work.spd.x;
            obj_work.spd.x = 0;
            obj_work.move_flag &= 4294967279U;
            if (obj_work.obj_3d.act_id[0] != 0)
            {
                AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 0);
                obj_work.disp_flag |= 4U;
            }
            AppMain.GmSoundPlaySE("Lorry4");
        }
        if (((int)obj_work.move_flag & 1) != 0)
            obj_work.spd_m = AppMain.ObjSpdUpSet(obj_work.spd_m, 128, 40960);
        else
            obj_work.spd.x = AppMain.ObjSpdUpSet(obj_work.spd.x, 128, 40960);
        truck_work.tire_dir_spd = ((int)obj_work.move_flag & 1) == 0 ? AppMain.ObjSpdDownSet(truck_work.tire_dir_spd, 128) : obj_work.spd_m;
        truck_work.tire_dir += (ushort)AppMain.FX_Div(truck_work.tire_dir_spd, 16384);
        if (((int)obj_work.move_flag & 1) != 0 && AppMain.MTM_MATH_ABS(obj_work.spd_m) >= AppMain.GMD_GMK_TRUCK_SPARK_EFCT_SMALL_MIN_SPD && (truck_work.efct_f_spark == null || truck_work.efct_b_spark == null))
            AppMain.gmGmkTruckCreateSparkEfct(truck_work, 27);
        AppMain.gmGmkTruckSetMoveSeParam(obj_work, truck_work.h_snd_lorry, (AppMain.GMS_PLAYER_WORK)null, 1);
    }

    public static void gmGmkTruckInitDeathFall(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_PLAYER_WORK ply_work)
    {
        ((AppMain.GMS_GMK_TRUCK_WORK)obj_work).target_player = (AppMain.GMS_PLAYER_WORK)null;
        if (ply_work != null)
        {
            obj_work.spd = ply_work.obj_work.spd;
            obj_work.spd_m = ply_work.obj_work.spd_m;
        }
        else
        {
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
            obj_work.spd.z = 0;
        }
        AppMain.ObjObjectSpdDirFall(ref obj_work.spd.x, ref obj_work.spd.y, AppMain.g_gm_main_system.pseudofall_dir);
        obj_work.spd.x = AppMain.FX_Mul(obj_work.spd.x, 4608);
        obj_work.spd.y = AppMain.FX_Mul(obj_work.spd.y, 4608);
        obj_work.spd_add.x = 0;
        obj_work.spd_add.y = obj_work.spd_fall;
        AppMain.ObjObjectSpdDirFall(ref obj_work.spd_add.x, ref obj_work.spd_add.y, AppMain.g_gm_main_system.pseudofall_dir);
        obj_work.flag |= 2U;
        obj_work.flag &= 4294967279U;
        obj_work.move_flag |= 272U;
        obj_work.move_flag &= 4294958975U;
        obj_work.pos.z = 983040;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTruckDeathFallMain);
    }

    public static void gmGmkTruckDeathFallMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.dir.z += (ushort)1024;
    }

    public static void gmGmkTruckDispFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_TRUCK_WORK gmsGmkTruckWork = (AppMain.GMS_GMK_TRUCK_WORK)obj_work;
        AppMain.ObjDrawActionSummary(obj_work);
        AppMain.VecU16 dir = obj_work.dir;
        uint p_disp_flag = obj_work.disp_flag | 16777216U;
        AppMain.nnMakeRotateXYZMatrix(gmsGmkTruckWork.obj_3d_tire.user_obj_mtx_r, (int)gmsGmkTruckWork.tire_dir, (int)gmsGmkTruckWork.slope_f_y_dir, (int)gmsGmkTruckWork.slope_f_z_dir);
        AppMain.VecFx32 vecFx32;
        vecFx32.x = AppMain.FXM_FLOAT_TO_FX32(gmsGmkTruckWork.tire_pos_f.M03);
        vecFx32.y = AppMain.FXM_FLOAT_TO_FX32(-gmsGmkTruckWork.tire_pos_f.M13);
        vecFx32.z = AppMain.FXM_FLOAT_TO_FX32(gmsGmkTruckWork.tire_pos_f.M23);
        AppMain.ObjDrawAction3DNN(gmsGmkTruckWork.obj_3d_tire, new AppMain.VecFx32?(vecFx32), new AppMain.VecU16?(dir), obj_work.scale, ref p_disp_flag);
        vecFx32.x = AppMain.FXM_FLOAT_TO_FX32(gmsGmkTruckWork.tire_pos_b.M03);
        vecFx32.y = AppMain.FXM_FLOAT_TO_FX32(-gmsGmkTruckWork.tire_pos_b.M13);
        vecFx32.z = AppMain.FXM_FLOAT_TO_FX32(gmsGmkTruckWork.tire_pos_b.M23);
        AppMain.ObjDrawAction3DNN(gmsGmkTruckWork.obj_3d_tire, new AppMain.VecFx32?(vecFx32), new AppMain.VecU16?(dir), obj_work.scale, ref p_disp_flag);
    }

    public static void gmGmkTruckBodyDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != (ushort)1)
            return;
        AppMain.GMS_GMK_TRUCK_WORK gmsGmkTruckWork = (AppMain.GMS_GMK_TRUCK_WORK)parentObj1;
        parentObj1.obj_work.flag |= 16U;
        AppMain.GmPlayerSetTruckRide(parentObj2, parentObj1.obj_work, parentObj1.obj_work.field_rect[0], parentObj1.obj_work.field_rect[1], parentObj1.obj_work.field_rect[2], parentObj1.obj_work.field_rect[3]);
        gmsGmkTruckWork.target_player = parentObj2;
        AppMain.gmGmkTruckInitMain(parentObj1.obj_work, parentObj2);
    }

    public static void gmGmkTruckMotionCallback(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_OBJECT _object,
      object param)
    {
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.GMS_GMK_TRUCK_WORK gmsGmkTruckWork = (AppMain.GMS_GMK_TRUCK_WORK)param;
        AppMain.nnMakeUnitMatrix(nnsMatrix2);
        AppMain.nnMultiplyMatrix(nnsMatrix2, nnsMatrix2, AppMain.amMatrixGetCurrent());
        AppMain.nnCalcNodeMatrixTRSList(nnsMatrix1, _object, AppMain.GMD_GMK_TRUCK_NODE_ID_TIRE_POS_F, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, nnsMatrix2);
        gmsGmkTruckWork.tire_pos_f.Assign(nnsMatrix1);
        AppMain.nnCalcNodeMatrixTRSList(nnsMatrix1, _object, AppMain.GMD_GMK_TRUCK_NODE_ID_TIRE_POS_B, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, nnsMatrix2);
        gmsGmkTruckWork.tire_pos_b.Assign(nnsMatrix1);
        AppMain.nnCalcNodeMatrixTRSList(nnsMatrix1, _object, AppMain.GMD_GMK_TRUCK_NODE_ID_LIGHT_POS, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, nnsMatrix2);
        gmsGmkTruckWork.light_pos.Assign(nnsMatrix1);
    }

    public static void gmGmkTruckSetMoveSeParam(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GSS_SND_SE_HANDLE h_snd,
      AppMain.GMS_PLAYER_WORK ply_work,
      int b_goal)
    {
        float val = 0.0f;
        if (h_snd == null)
            return;
        AppMain.OBS_OBJECT_WORK obsObjectWork = ply_work == null ? obj_work : ply_work.obj_work;
        int num1 = AppMain.MTM_MATH_ABS(obsObjectWork.spd_m);
        if (((int)obsObjectWork.move_flag & 1) != 0 && num1 >= AppMain.GMD_GMK_TRUCK_SE_MIN_SPD)
        {
            if (num1 >= AppMain.GMD_GMK_TRUCK_SE_MAX_SPD)
            {
                val = 1f;
            }
            else
            {
                val = AppMain.FXM_FX32_TO_FLOAT(AppMain.FX_Div(num1 - AppMain.GMD_GMK_TRUCK_SE_MIN_SPD, AppMain.GMD_GMK_TRUCK_SE_MAX_SPD - AppMain.GMD_GMK_TRUCK_SE_MIN_SPD));
                if ((double)val > 1.0)
                    val = 1f;
            }
        }
        h_snd.au_player.SetAisac("Speed", val);
        if (b_goal == 0)
            return;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        float num2 = AppMain.FXM_FX32_TO_FLOAT(obsObjectWork.pos.x) - obsCamera.disp_pos.x;
        float num3 = AppMain.FXM_FX32_TO_FLOAT(obsObjectWork.pos.y) - -obsCamera.disp_pos.y;
        float num4;
        if ((double)num2 < (double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MAX_DIST && (double)num3 < (double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MAX_DIST)
        {
            float num5 = (float)((double)num2 * (double)num2 + (double)num3 * (double)num3);
            if ((double)num5 <= (double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MIN_DIST * (double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MIN_DIST)
                num4 = 1f;
            else if ((double)num5 <= (double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MAX_DIST * (double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MAX_DIST)
            {
                num4 = (float)(((double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MAX_DIST * (double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MAX_DIST - (double)num5) / (((double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MAX_DIST - (double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MIN_DIST) * ((double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MAX_DIST - (double)AppMain.GMD_GMK_TRUCK_SE_GOAL_MIN_DIST)));
                if ((double)num4 > 1.0)
                    num4 = 1f;
                else if ((double)num4 < 0.0)
                    num4 = 0.0f;
            }
            else
                num4 = 0.0f;
        }
        else
            num4 = 0.0f;
        h_snd.snd_ctrl_param.volume = num4;
    }

    public static void gmGmkTGravityChangeDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != (ushort)1 || (((int)parentObj1.eve_rec.flag & AppMain.GMD_GMK_T_GRAVITY_A) != 0 && ((int)parentObj2.obj_work.flag & 1) != 0 || ((int)parentObj1.eve_rec.flag & AppMain.GMD_GMK_T_GRAVITY_B) != 0 && ((int)parentObj2.obj_work.flag & 1) == 0))
            return;
        ushort num1;
        if ((ushort)239 <= parentObj1.eve_rec.id && parentObj1.eve_rec.id <= (ushort)246)
        {
            int num2;
            int num3;
            int num4;
            if ((ushort)239 <= parentObj1.eve_rec.id && parentObj1.eve_rec.id <= (ushort)242)
            {
                num2 = (int)parentObj1.eve_rec.id - 239;
                num3 = (num2 & 2) == 0 ? parentObj1.obj_work.pos.x - 524288 : parentObj1.obj_work.pos.x + 524288;
                num4 = (num2 + 1 & 2) == 0 ? parentObj1.obj_work.pos.y + 524288 : parentObj1.obj_work.pos.y - 524288;
                parentObj2.gmk_flag2 |= 8U;
            }
            else
            {
                num2 = (int)parentObj1.eve_rec.id - 243;
                num3 = (num2 & 2) == 0 ? parentObj1.obj_work.pos.x + 524288 : parentObj1.obj_work.pos.x - 524288;
                num4 = (num2 + 1 & 2) == 0 ? parentObj1.obj_work.pos.y - 524288 : parentObj1.obj_work.pos.y + 524288;
            }
            float num5 = AppMain.FXM_FX32_TO_FLOAT(num3 - parentObj2.obj_work.pos.x);
            float num6 = AppMain.FXM_FX32_TO_FLOAT(num4 - parentObj2.obj_work.pos.y);
            if ((ushort)239 <= parentObj1.eve_rec.id && parentObj1.eve_rec.id <= (ushort)242)
            {
                if ((num2 & 2) != 0)
                {
                    if ((double)num5 < 0.0)
                        return;
                }
                else if ((double)num5 > 0.0)
                    return;
                if ((num2 + 1 & 2) != 0)
                {
                    if ((double)num6 > 0.0)
                        return;
                }
                else if ((double)num6 < 0.0)
                    return;
            }
            else
            {
                if ((num2 & 2) != 0)
                {
                    if ((double)num5 > 0.0)
                        return;
                }
                else if ((double)num5 < 0.0)
                    return;
                if ((num2 + 1 & 2) != 0)
                {
                    if ((double)num6 < 0.0)
                        return;
                }
                else if ((double)num6 > 0.0)
                    return;
            }
            num1 = (ushort)(65536U - (uint)(ushort)(AppMain.nnArcTan2(-(double)num6, (double)num5) - 16384));
            if ((ushort)239 <= parentObj1.eve_rec.id && parentObj1.eve_rec.id <= (ushort)242)
                num1 -= (ushort)32768;
        }
        else
            num1 = AppMain.gm_gmk_t_gravity_flat_dir_tbl[(int)parentObj1.eve_rec.id - 223];
        if ((int)parentObj2.jump_pseudofall_eve_id_cur != (int)parentObj1.eve_rec.id)
        {
            parentObj2.jump_pseudofall_eve_id_wait = parentObj1.eve_rec.id;
        }
        else
        {
            if (((int)parentObj1.eve_rec.flag & AppMain.GMD_GMK_T_CLEAR_PSEUDOFALL_DIR_FIX) != 0)
                parentObj2.gmk_flag &= 4278190079U;
            if (((int)parentObj2.gmk_flag & 16777216) != 0)
                return;
            AppMain.ObjObjectSpdDirFall(ref parentObj2.obj_work.spd.x, ref parentObj2.obj_work.spd.y, (ushort)-((int)num1 - (int)parentObj2.jump_pseudofall_dir));
            parentObj2.jump_pseudofall_dir = num1;
            parentObj2.jump_pseudofall_eve_id_set = parentObj1.eve_rec.id;
        }
    }

    public static void gmGmkTGravityForceChangeDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != (ushort)1 || ((int)parentObj2.obj_work.move_flag & 1) != 0) || (((int)parentObj1.eve_rec.flag & AppMain.GMD_GMK_T_GRAVITY_A) != 0 && ((int)parentObj2.obj_work.flag & 1) != 0 || ((int)parentObj1.eve_rec.flag & AppMain.GMD_GMK_T_GRAVITY_B) != 0 && ((int)parentObj2.obj_work.flag & 1) == 0))
            return;
        int num1 = ((int)parentObj1.eve_rec.id - 268) * 16384;
        ushort num2 = (ushort)((uint)num1 - (uint)parentObj2.jump_pseudofall_dir);
        parentObj2.jump_pseudofall_dir = (ushort)num1;
        int a = num1 - parentObj2.ply_pseudofall_dir;
        if ((ushort)AppMain.MTM_MATH_ABS(a) > (ushort)32768)
        {
            if (a < 0)
                parentObj2.ply_pseudofall_dir += 65536 + a;
            else
                parentObj2.ply_pseudofall_dir += a - 65536;
        }
        else
            parentObj2.ply_pseudofall_dir = num1;
        AppMain.ObjObjectSpdDirFall(ref parentObj2.obj_work.spd.x, ref parentObj2.obj_work.spd.y, (ushort)-num2);
        parentObj2.gmk_flag |= 16777216U;
    }

    public static void gmGmkTNoLandingDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != (ushort)1)
            return;
        parentObj2.obj_work.sys_flag |= (uint)(1 << (int)parentObj1.eve_rec.id - 264);
    }

    public static void gmGmkTruckCreateLightEfct(AppMain.GMS_GMK_TRUCK_WORK truck_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)truck_work, 2, 10);
        gmsEffect3DesWork.efct_com.obj_work.user_work_OBJECT = (object)truck_work.light_pos;
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTruckLightEfctMain);
        gmsEffect3DesWork.efct_com.obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTruckLightEfctDispFunc);
    }

    public static void gmGmkTruckLightEfctMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.parent_obj == null)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            AppMain.GMS_GMK_TRUCK_WORK parentObj = (AppMain.GMS_GMK_TRUCK_WORK)obj_work.parent_obj;
            obj_work.dir.z = (ushort)((uint)obj_work.parent_obj.dir.z + (uint)parentObj.slope_z_dir);
            if (((int)obj_work.disp_flag & 8) == 0)
                return;
            obj_work.flag |= 4U;
        }
    }

    public static void gmGmkTruckLightEfctDispFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.NNS_MATRIX userWorkObject = (AppMain.NNS_MATRIX)obj_work.user_work_OBJECT;
        if (obj_work.parent_obj == null)
            return;
        obj_work.pos.x = AppMain.FXM_FLOAT_TO_FX32(userWorkObject.M03);
        obj_work.pos.y = AppMain.FXM_FLOAT_TO_FX32(-userWorkObject.M13);
        obj_work.pos.z = AppMain.FXM_FLOAT_TO_FX32(userWorkObject.M23);
        AppMain.ObjDrawActionSummary(obj_work);
    }

    public static void gmGmkTruckCreateSparkEfct(AppMain.GMS_GMK_TRUCK_WORK truck_work, int efct_type)
    {
        if (truck_work.efct_f_spark == null)
        {
            truck_work.efct_f_spark = AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)truck_work, 2, efct_type);
            truck_work.efct_f_spark.efct_com.obj_work.flag |= 524304U;
            truck_work.efct_f_spark.efct_com.obj_work.user_work_OBJECT = (object)truck_work.tire_pos_f;
            truck_work.efct_f_spark.efct_com.obj_work.user_timer = efct_type;
            truck_work.efct_f_spark.efct_com.obj_work.user_flag = 0U;
            truck_work.efct_f_spark.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTruckSparkEfctMain);
            truck_work.efct_f_spark.efct_com.obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTruckSparkEfctDispFunc);
        }
        if (truck_work.efct_b_spark != null)
            return;
        truck_work.efct_b_spark = AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)truck_work, 2, efct_type);
        truck_work.efct_b_spark.efct_com.obj_work.flag |= 524304U;
        truck_work.efct_b_spark.efct_com.obj_work.user_work_OBJECT = (object)truck_work.tire_pos_b;
        truck_work.efct_b_spark.efct_com.obj_work.user_timer = efct_type;
        truck_work.efct_b_spark.efct_com.obj_work.user_flag = 1U;
        truck_work.efct_b_spark.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTruckSparkEfctMain);
        truck_work.efct_b_spark.efct_com.obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTruckSparkEfctDispFunc);
    }

    public static void gmGmkTruckSparkEfctMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_TRUCK_WORK parentObj = (AppMain.GMS_GMK_TRUCK_WORK)obj_work.parent_obj;
        if (obj_work.parent_obj == null)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)parentObj.target_player ?? (AppMain.OBS_OBJECT_WORK)parentObj;
            uint dispFlag = obj_work.disp_flag;
            if (((int)obsObjectWork.move_flag & 1) == 0)
            {
                obj_work.disp_flag |= 32U;
            }
            else
            {
                obj_work.disp_flag &= 4294967263U;
                if (AppMain.MTM_MATH_ABS(obsObjectWork.spd_m) < AppMain.GMD_GMK_TRUCK_SPARK_EFCT_SMALL_MIN_SPD)
                {
                    if (((int)dispFlag & 32) != 0)
                    {
                        obj_work.flag |= 8U;
                    }
                    else
                    {
                        AppMain.ObjDrawKillAction3DES(obj_work);
                        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd);
                    }
                    if (obj_work.user_flag == 0U)
                        parentObj.efct_f_spark = (AppMain.GMS_EFFECT_3DES_WORK)null;
                    else
                        parentObj.efct_b_spark = (AppMain.GMS_EFFECT_3DES_WORK)null;
                }
                AppMain.GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
            }
        }
    }

    public static void gmGmkTruckSparkEfctDispFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_TRUCK_WORK parentObj = (AppMain.GMS_GMK_TRUCK_WORK)obj_work.parent_obj;
        AppMain.NNS_MATRIX userWorkObject = (AppMain.NNS_MATRIX)obj_work.user_work_OBJECT;
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        if (obj_work.parent_obj == null)
        {
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        }
        else
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)parentObj.target_player ?? (AppMain.OBS_OBJECT_WORK)parentObj;
            AppMain.VecFx32 vecFx32;
            vecFx32.x = AppMain.FXM_FLOAT_TO_FX32(userWorkObject.M03);
            vecFx32.y = AppMain.FXM_FLOAT_TO_FX32(-userWorkObject.M13);
            vecFx32.z = AppMain.FXM_FLOAT_TO_FX32(userWorkObject.M23);
            ushort dir_z;
            ushort num;
            if (obsObjectWork.spd_m >= 0)
            {
                obj_work.disp_flag &= 4294967294U;
                dir_z = (ushort)(8192U - (uint)obj_work.parent_obj.dir.z);
                num = (ushort)((uint)-obj_work.parent_obj.dir.z - 2048U);
            }
            else
            {
                obj_work.disp_flag |= 1U;
                dir_z = (ushort)(8192U + (uint)obj_work.parent_obj.dir.z);
                num = (ushort)((uint)obj_work.parent_obj.dir.z + 2048U);
            }
            obj_work.pos = vecFx32;
            nnsVector.x = AppMain.nnSin((int)num) * AppMain.GMD_GMK_TRUCK_EFCT_SPRAK_OFST_DIST;
            nnsVector.y = AppMain.nnCos((int)num) * AppMain.GMD_GMK_TRUCK_EFCT_SPRAK_OFST_DIST;
            nnsVector.z = 0.0f;
            AppMain.GmComEfctSetDispOffsetF((AppMain.GMS_EFFECT_3DES_WORK)obj_work, nnsVector.x, nnsVector.y, nnsVector.z);
            AppMain.GmComEfctSetDispRotation((AppMain.GMS_EFFECT_3DES_WORK)obj_work, (ushort)0, (ushort)0, dir_z);
            AppMain.ObjDrawActionSummary(obj_work);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        }
    }

}