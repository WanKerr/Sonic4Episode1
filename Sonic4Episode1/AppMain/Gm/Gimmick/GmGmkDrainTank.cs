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
    public static int GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH
    {
        get
        {
            return (int)AppMain.OBD_LCD_X + 256;
        }
    }

    public static int GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT
    {
        get
        {
            return (int)AppMain.OBD_LCD_Y + 256;
        }
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkDrainTankInitIn(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkDrainTankLoadObj(eve_rec, pos_x, pos_y, 0, 0U).ene_com.obj_work;
        AppMain.gmGmkDrainTankInInit(objWork);
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkDrainTankInitOut(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_main_system.ply_work[0];
        if (pos_x - 524288 < obsObjectWork.pos.x)
            return (AppMain.OBS_OBJECT_WORK)null;
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkDrainTankLoadObj(eve_rec, pos_x, pos_y, 1, 1U).ene_com.obj_work;
        AppMain.gmGmkDrainTankOutInit(objWork);
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkDrainTankSplashInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkDrainTankLoadObj(eve_rec, pos_x, pos_y, 2, 2U).ene_com.obj_work;
        AppMain.gmGmkDrainTankSplashInit(objWork);
        return objWork;
    }

    public static void GmGmkDrainTankBuild()
    {
        AppMain.g_gm_gmk_drain_tank_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(923), AppMain.GmGameDatGetGimmickData(924), 0U);
    }

    public static void GmGmkDrainTankFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(923);
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_drain_tank_obj_3d_list, gimmickData.file_num);
        AppMain.g_gm_gmk_drain_tank_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkDrainTankLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork;
        if (type == 1)
        {
            AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK work = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK()), "GMK_DRAIN_TANK");
            work.base_pos_x = pos_x + 262144;
            work.base_pos_y = pos_y;
            gmsEnemy3DWork = work.enemy_work;
        }
        else
            gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_DRAIN_TANK");
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        return gmsEnemy3DWork;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkDrainTankLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type,
      uint model_id)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkDrainTankLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        AppMain.ObjObjectCopyAction3dNNModel(gmsEnemy3DWork.ene_com.obj_work, AppMain.g_gm_gmk_drain_tank_obj_3d_list[(int)model_id], gmsEnemy3DWork.obj_3d);
        if (type == 2)
        {
            int index = 0;
            object pData = AppMain.ObjDataGet(925).pData;
            AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, index, (AppMain.AMS_AMB_HEADER)pData);
        }
        return gmsEnemy3DWork;
    }

    private static ushort gmGmkDrainTankGameSystemGetWaterLevel()
    {
        return AppMain.g_gm_main_system.water_level;
    }

    private static void gmGmkDrainTankInInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjObjectFieldRectSet(obj_work, (short)-16, (short)0, (short)0, (short)14);
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)16;
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)32;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)-16;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)-32;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankDrawFuncIn);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankInMainReady);
        obj_work.disp_flag |= 4194304U;
        gmsEnemy3DWork.obj_3d.command_state = 8U;
        obj_work.pos.z = 1441792;
    }

    private static void gmGmkDrainTankOutInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjObjectFieldRectSet(obj_work, (short)0, (short)0, (short)32, (short)16);
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankDrawFuncOut);
        AppMain.gmGmkDrainTankOutChangeModeReady(obj_work);
        obj_work.disp_flag |= 4194304U;
        obj_work.move_flag |= 128U;
        obj_work.flag |= 16U;
        gmsEnemy3DWork.obj_3d.command_state = 8U;
        obj_work.pos.z = 1441792;
        AppMain.GmWaterSurfaceRequestChangeWaterLevel((ushort)AppMain.FX_FX32_TO_F32(obj_work.pos.y - 196608), (ushort)0, false);
        AppMain.GmWaterSurfaceSetFlagEnableRef(false);
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkDrainTankTcbDestOut));
    }

    private static void gmGmkDrainTankSplashInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.disp_flag |= 4194304U;
        obj_work.move_flag = 8448U;
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.pos.z = 262144;
        obj_work.disp_flag |= 134217728U;
        obj_work.obj_3d.drawflag |= 8388608U;
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        obj_work.user_timer = 60;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankSplashMainFunc);
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork1 = AppMain.GmEfctZoneEsCreate(obj_work, 2, 34);
        gmsEffect3DesWork1.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankSplashEffectMain);
        gmsEffect3DesWork1.efct_com.obj_work.move_flag = 384U;
        gmsEffect3DesWork1.efct_com.obj_work.pos.y += -131072;
        gmsEffect3DesWork1.efct_com.obj_work.pos.z = obj_work.pos.z + 131072;
        gmsEffect3DesWork1.efct_com.obj_work.spd.x = 49152;
        gmsEffect3DesWork1.efct_com.obj_work.spd.y = 12288;
        gmsEffect3DesWork1.efct_com.obj_work.spd_add.x = -1376;
        gmsEffect3DesWork1.efct_com.obj_work.spd_add.y = 0;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork2 = AppMain.GmEfctZoneEsCreate(obj_work, 2, 35);
        gmsEffect3DesWork2.efct_com.obj_work.pos.y += -65536;
        gmsEffect3DesWork2.efct_com.obj_work.pos.z = obj_work.pos.z + 131072;
    }

    private static void gmGmkDrainTankDrawFuncIn(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, 1, 8U);
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void gmGmkDrainTankDrawFuncOut(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 32) == 0)
        {
            AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
            int num1 = ((-(int)AppMain.FX_FX32_TO_F32(drainTankOutWork.base_pos_y) - 64) / 64 - 1) * 64;
            int num2 = ((int)AppMain.FX_FX32_TO_F32(drainTankOutWork.base_pos_x) - 96) / 64 * 64;
            int num3 = num2 - 152;
            int num4 = num1 + 96;
            int num5 = num2 + 64;
            AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, 1, 9U);
            AppMain.GmWaterSurfaceDrawNoWaterField(AppMain.FX_FX32_TO_F32(obj_work.pos.x), -AppMain.FX_FX32_TO_F32(obj_work.pos.y) + (float)AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT, AppMain.FX_FX32_TO_F32(obj_work.pos.x) + (float)AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH, -AppMain.FX_FX32_TO_F32(obj_work.pos.y) - (float)AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT);
            AppMain.GmWaterSurfaceDrawNoWaterField((float)num2, -AppMain.FX_FX32_TO_F32(drainTankOutWork.base_pos_y) + (float)AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT, AppMain.FX_FX32_TO_F32(drainTankOutWork.base_pos_x) + (float)AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH, (float)num4);
            AppMain.GmWaterSurfaceDrawNoWaterField(AppMain.FX_FX32_TO_F32(drainTankOutWork.base_pos_x) - (float)AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH, (float)num1, AppMain.FX_FX32_TO_F32(drainTankOutWork.base_pos_x) + (float)AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH, (float)(num1 - AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT));
            AppMain.GmWaterSurfaceDrawNoWaterField((float)(num3 - AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH), -AppMain.FX_FX32_TO_F32(drainTankOutWork.base_pos_y) + (float)AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT, (float)num3, (float)num1);
            AppMain.GmWaterSurfaceDrawNoWaterField((float)num5, (float)num4, (float)(num5 + AppMain.GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH), (float)num1);
        }
        AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, 1, 8U);
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void gmGmkDrainTankTcbDestOut(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GmWaterSurfaceSetFlagEnableRef(true);
        AppMain.g_gm_main_system.game_flag &= 4294959103U;
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static bool gmGmkDrainTankInCheckDeleteTask(
      AppMain.OBS_OBJECT_WORK obj_work,
      int cmp_x,
      int cmp_y)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.g_gm_main_system.ply_work[0].obj_work;
        int num1 = AppMain.MTM_MATH_ABS(obj_work.pos.x - objWork.pos.x);
        int num2 = AppMain.MTM_MATH_ABS(obj_work.pos.y - objWork.pos.y);
        return num1 > cmp_x || num2 > cmp_y;
    }

    private static void gmGmkDrainTankInRequestDeleteTask(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag |= 4U;
    }

    private static void gmGmkDrainTankInMainReady(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
            return;
        if (AppMain.gmGmkDrainTankInCheckDeleteTask(obj_work, 1843200, 1228800))
        {
            AppMain.gmGmkDrainTankInRequestDeleteTask(obj_work);
        }
        else
        {
            AppMain.OBS_OBJECT_WORK objWork = gmsPlayerWork.obj_work;
            if (obj_work.pos.x + 65536 >= objWork.pos.x)
                return;
            obj_work.pos.y += 32768;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankInMainWait);
            obj_work.move_flag |= 128U;
        }
    }

    private static void gmGmkDrainTankInMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 1024) != 0 || !AppMain.gmGmkDrainTankInCheckDeleteTask(obj_work, 1843200, 1228800))
            return;
        AppMain.gmGmkDrainTankInRequestDeleteTask(obj_work);
    }

    private static bool gmGmkDrainTankOutCheckDeleteTask(
      AppMain.OBS_OBJECT_WORK obj_work,
      int cmp_x,
      int cmp_y)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.g_gm_main_system.ply_work[0].obj_work;
        AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
        int num1 = AppMain.MTM_MATH_ABS(drainTankOutWork.base_pos_x - objWork.pos.x);
        int num2 = AppMain.MTM_MATH_ABS(drainTankOutWork.base_pos_y - objWork.pos.y);
        return num1 > cmp_x || num2 > cmp_y;
    }

    private static void gmGmkDrainTankOutRequestDeleteTask(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GmPlayerCameraOffsetSet(AppMain.g_gm_main_system.ply_work[0], (short)0, (short)0);
        AppMain.GmCameraAllowReset();
        AppMain.GmWaterSurfaceRequestChangeWaterLevel(ushort.MaxValue, (ushort)0, false);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankOutMainWaitDelete);
    }

    private static void gmGmkDrainTankOutChangeModeReady(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.x = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankOutMainReady);
    }

    private static void gmGmkDrainTankOutChangeModeWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.g_gm_main_system.ply_work[0].obj_work;
        AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
        drainTankOutWork.player_offset_x = objWork.pos.x - drainTankOutWork.base_pos_x;
        drainTankOutWork.player_offset_y = objWork.pos.y - drainTankOutWork.base_pos_y;
        obj_work.spd.x = 0;
        drainTankOutWork.counter_roll_key = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankOutMainWait);
    }

    private static void gmGmkDrainTankOutChangeModeDamage(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.x = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankOutMainDamage);
    }

    private static void gmGmkDrainTankOutChangeModeSplash(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)gmsEnemy3DWork;
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        objWork.spd.x = !drainTankOutWork.flag_dir_left ? 65536 : -65536;
        objWork.pos.y = obj_work.pos.y;
        objWork.move_flag |= 256U;
        AppMain.GmPlayerBreathingSet(ply_work);
        obj_work.move_flag |= 256U;
        AppMain.g_gm_main_system.game_flag |= 8192U;
        AppMain.GmEventMgrLocalEventBirth((ushort)305, obj_work.pos.x, obj_work.pos.y + 65536, gmsEnemy3DWork.ene_com.eve_rec.flag, gmsEnemy3DWork.ene_com.eve_rec.left, gmsEnemy3DWork.ene_com.eve_rec.top, gmsEnemy3DWork.ene_com.eve_rec.width, gmsEnemy3DWork.ene_com.eve_rec.height, (byte)0);
        AppMain.GmSoundPlaySE("Fluid2");
        AppMain.GMM_PAD_VIB_SMALL();
        AppMain.GmPlayerCameraOffsetSet(ply_work, (short)0, (short)0);
        AppMain.GmCameraAllowReset();
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankOutMainSplash);
    }

    private static void gmGmkDrainTankOutChangeModeEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDrainTankOutMainEnd);
    }

    private static void gmGmkDrainTankOutMainReady(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        AppMain.OBS_OBJECT_WORK objWork = gmsPlayerWork.obj_work;
        AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
        {
            AppMain.gmGmkDrainTankOutUpdateDie(obj_work);
            int num = ((int)AppMain.FX_FX32_TO_F32(out_work.base_pos_x) / 64 - 3) * 262144;
            if (objWork.pos.x < num)
                AppMain.g_gm_main_system.game_flag |= 8192U;
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        else if (AppMain.gmGmkDrainTankOutCheckDeleteTask(obj_work, 1843200, 1228800))
        {
            AppMain.gmGmkDrainTankOutRequestDeleteTask(obj_work);
        }
        else
        {
            if ((int)AppMain.gmGmkDrainTankGameSystemGetWaterLevel() * 4096 + 196608 >= objWork.pos.y)
                return;
            AppMain.gmGmkDrainTankOutChangeModeWait(obj_work);
            out_work.flag_dir_left = AppMain.gmGmkDrainTankOutCheckDirLeft(obj_work, objWork);
            int x = objWork.spd.x;
            AppMain.GmPlySeqInitDrainTank(gmsPlayerWork);
            out_work.player_offset_x += x * 5;
            AppMain.gmGmkDrainTankOutUpdateCameraOffset(gmsPlayerWork, out_work);
            AppMain.GmCameraAllowSet(10f, 10f, 10f);
        }
    }

    private static void gmGmkDrainTankOutMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
        {
            AppMain.gmGmkDrainTankOutUpdateDie(obj_work);
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        else if (AppMain.gmGmkDrainTankOutCheckDeleteTask(obj_work, 1843200, 1228800))
        {
            AppMain.gmGmkDrainTankOutRequestDeleteTask(obj_work);
        }
        else
        {
            AppMain.OBS_OBJECT_WORK objWork = gmsPlayerWork.obj_work;
            if (gmsPlayerWork.seq_state == 22)
                AppMain.gmGmkDrainTankOutChangeModeDamage(obj_work);
            else if (((int)obj_work.move_flag & 1) == 0)
            {
                AppMain.gmGmkDrainTankOutChangeModeSplash(obj_work);
            }
            else
            {
                AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
                int gimmickRotZ = AppMain.GmPlayerKeyGetGimmickRotZ(gmsPlayerWork);
                if (AppMain.MTM_MATH_ABS(gimmickRotZ) > 8192)
                    ++out_work.counter_roll_key;
                else
                    out_work.counter_roll_key = 0;
                if (out_work.counter_roll_key >= 0)
                {
                    AppMain.gmGmkDrainTankOutUpdateCameraRoll(out_work, gimmickRotZ);
                    if (19 == out_work.counter_roll_key % 20 && AppMain.MTM_MATH_ABS(out_work.camera_roll) < AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX)
                        AppMain.GmSoundPlaySE("Fluid1");
                }
                if (AppMain.gmGmkDrainTankOutCheckKeyDir(obj_work, out_work.camera_roll))
                {
                    int num = out_work.camera_roll >> 3;
                    obj_work.spd.x = num;
                    AppMain.GmWaterSurfaceRequestAddWatarLevel(AppMain.MTM_MATH_ABS(AppMain.MTM_MATH_ABS(AppMain.FX_FX32_TO_F32(out_work.camera_roll >> 4))), (ushort)0, true);
                }
                AppMain.gmGmkDrainTankOutAdjustPlayerOffsetBuoyancy(out_work);
                AppMain.gmGmkDrainTankOutAdjustPlayerOffsetWave(out_work, objWork);
                AppMain.gmGmkDrainTankOutApplyPlayerOffset(objWork, out_work);
                AppMain.ObjCameraGet(0).roll = out_work.camera_roll;
                AppMain.gmGmkDrainTankOutUpdateCameraOffset(gmsPlayerWork, out_work);
            }
        }
    }

    private static void gmGmkDrainTankOutMainDamage(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
        {
            AppMain.gmGmkDrainTankOutUpdateDie(obj_work);
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        else if (AppMain.gmGmkDrainTankOutCheckDeleteTask(obj_work, 1843200, 1228800))
        {
            AppMain.gmGmkDrainTankOutRequestDeleteTask(obj_work);
        }
        else
        {
            AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
            AppMain.OBS_OBJECT_WORK objWork = gmsPlayerWork.obj_work;
            AppMain.gmGmkDrainTankOutUpdateCameraRollDamage(out_work);
            int num1 = (AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX - AppMain.MTM_MATH_ABS(out_work.camera_roll)) * 4;
            int num2 = num1 >> 3;
            obj_work.spd.x = !out_work.flag_dir_left ? -num2 : num2;
            AppMain.GmWaterSurfaceRequestAddWatarLevel(-AppMain.FX_FX32_TO_F32(num1 >> 4), (ushort)0, true);
            ++obj_work.user_timer;
            if (((int)objWork.move_flag & 1) != 0)
            {
                obj_work.user_timer = 0;
                AppMain.gmGmkDrainTankOutChangeModeWait(obj_work);
                AppMain.GmPlySeqInitDrainTank(gmsPlayerWork);
            }
            else
            {
                AppMain.ObjCameraGet(0).roll = out_work.camera_roll;
                AppMain.gmGmkDrainTankOutUpdateCameraOffset(gmsPlayerWork, out_work);
                AppMain.gmGmkDrainTankOutSinkRing();
            }
        }
    }

    private static void gmGmkDrainTankOutMainSplash(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        if (((int)ply_work.player_flag & 1024) != 0)
            AppMain.gmGmkDrainTankOutUpdateDie(obj_work);
        else if (AppMain.gmGmkDrainTankOutCheckDeleteTask(obj_work, 1638400, 1228800))
        {
            AppMain.gmGmkDrainTankOutRequestDeleteTask(obj_work);
        }
        else
        {
            AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
            AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
            AppMain.GmWaterSurfaceRequestAddWatarLevel(1f / 1000f, (ushort)0, true);
            AppMain.ObjCameraGet(0).roll = drainTankOutWork.camera_roll;
            if (drainTankOutWork.base_pos_x >= objWork.pos.x)
                return;
            objWork.move_flag |= 128U;
            objWork.move_flag &= 4294967039U;
            objWork.spd.x = 65536;
            objWork.spd_add.x = -896;
            AppMain.GmPlayerCameraOffsetSet(ply_work, (short)0, (short)0);
            AppMain.GmCameraAllowReset();
            AppMain.gmGmkDrainTankOutChangeModeEnd(obj_work);
        }
    }

    private static void gmGmkDrainTankOutMainEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        if (((int)ply_work.player_flag & 1024) != 0)
        {
            AppMain.gmGmkDrainTankOutUpdateDie(obj_work);
        }
        else
        {
            if (!AppMain.gmGmkDrainTankOutCheckDeleteTask(obj_work, 1638400, 1228800))
                return;
            AppMain.gmGmkDrainTankOutRequestDeleteTask(obj_work);
            AppMain.GmPlySeqInitDrainTankFall(ply_work);
        }
    }

    private static void gmGmkDrainTankOutMainWaitDelete(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.gmGmkDrainTankGameSystemGetWaterLevel() != ushort.MaxValue)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmGmkDrainTankOutUpdateDie(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
        AppMain.gmGmkDrainTankOutUpdateCameraRollDie(out_work);
        AppMain.ObjCameraGet(0).roll = out_work.camera_roll;
    }

    private static bool gmGmkDrainTankOutCheckDirLeft(
      AppMain.OBS_OBJECT_WORK gimmick_obj_work,
      AppMain.OBS_OBJECT_WORK player_obj_work)
    {
        return player_obj_work.pos.x >= gimmick_obj_work.pos.x;
    }

    private static void gmGmkDrainTankOutAdjustPlayerOffsetBuoyancy(
      AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work)
    {
        int num = (int)AppMain.gmGmkDrainTankGameSystemGetWaterLevel() * 4096 + 131072;
        if (num > out_work.base_pos_y)
            num = out_work.base_pos_y;
        int a = num - (out_work.base_pos_y + out_work.player_offset_y);
        if (AppMain.MTM_MATH_ABS(a) > 2048)
            a = a >= 0 ? 2048 : -2048;
        out_work.player_offset_y += a;
    }

    private static void gmGmkDrainTankOutAdjustPlayerOffsetWave(
      AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work,
      AppMain.OBS_OBJECT_WORK player_obj_work)
    {
        int num = out_work.camera_roll >> 2;
        if (((int)player_obj_work.move_flag & 4) != 0)
        {
            if (((int)player_obj_work.disp_flag & 1) != 0)
                out_work.player_offset_x += AppMain.FX_F32_TO_FX32(0.4f);
            else
                out_work.player_offset_x -= AppMain.FX_F32_TO_FX32(0.4f);
        }
        else
        {
            if (out_work.camera_roll < 0)
                player_obj_work.disp_flag |= 1U;
            else if (out_work.camera_roll > 0)
                player_obj_work.disp_flag &= 4294967294U;
            out_work.player_offset_x += num;
        }
    }

    private static void gmGmkDrainTankOutApplyPlayerOffset(
      AppMain.OBS_OBJECT_WORK player_obj_work,
      AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work)
    {
        int num1 = out_work.base_pos_x + out_work.player_offset_x;
        int num2 = out_work.base_pos_y + out_work.player_offset_y;
        player_obj_work.spd.x = num1 - player_obj_work.pos.x;
        player_obj_work.spd.y = num2 - player_obj_work.pos.y;
    }

    private static void gmGmkDrainTankOutUpdateCameraRoll(
      AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work,
      int rot_z)
    {
        if (rot_z < -8192)
        {
            if (out_work.camera_roll <= -AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX)
                out_work.camera_roll = -AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX;
            else
                out_work.camera_roll -= AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_SPEED;
        }
        else
        {
            if (rot_z <= 8192)
                return;
            if (out_work.camera_roll >= AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX)
                out_work.camera_roll = AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX;
            else
                out_work.camera_roll += AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_SPEED;
        }
    }

    private static void gmGmkDrainTankOutUpdateCameraRollDamage(
      AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work)
    {
        int num = AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_SPEED * 4;
        if (out_work.flag_dir_left)
        {
            out_work.camera_roll += num;
            if (out_work.camera_roll >= -AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX)
                return;
            out_work.camera_roll = -AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX;
        }
        else
        {
            out_work.camera_roll -= num;
            if (out_work.camera_roll <= AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX)
                return;
            out_work.camera_roll = AppMain.GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX;
        }
    }

    private static void gmGmkDrainTankOutUpdateCameraRollDie(
      AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work)
    {
        out_work.camera_roll -= out_work.camera_roll / 5;
    }

    private static void gmGmkDrainTankOutUpdateCameraOffset(
      AppMain.GMS_PLAYER_WORK player_work,
      AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK out_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)player_work;
        float f32 = AppMain.FX_FX32_TO_F32(out_work.base_pos_x - 737280 - obsObjectWork.pos.x);
        AppMain.GmPlayerCameraOffsetSet(player_work, (short)f32, (short)0);
    }

    private static bool gmGmkDrainTankOutCheckKeyDir(
      AppMain.OBS_OBJECT_WORK gimmick_obj_work,
      int camera_roll)
    {
        AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)gimmick_obj_work;
        return camera_roll < 0 && drainTankOutWork.flag_dir_left || camera_roll > 0 && !drainTankOutWork.flag_dir_left;
    }

    private static void gmGmkDrainTankOutSinkRing()
    {
        for (AppMain.GMS_RING_WORK gmsRingWork = AppMain.GmRingGetWork().damage_ring_list_start; gmsRingWork != null; gmsRingWork = gmsRingWork.post_ring)
        {
            gmsRingWork.spd_y = 4096;
            gmsRingWork.spd_x /= 2;
        }
    }

    private static void gmGmkDrainTankSplashMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        --obj_work.user_timer;
        if (obj_work.user_timer > 0)
        {
            float num = (float)obj_work.user_timer / 60f;
            obj_work.obj_3d.draw_state.alpha.alpha = num;
        }
        else
        {
            obj_work.disp_flag |= 16U;
            obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        }
    }

    private static void gmGmkDrainTankSplashEffectMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.dir.z += (ushort)384;
    }

}