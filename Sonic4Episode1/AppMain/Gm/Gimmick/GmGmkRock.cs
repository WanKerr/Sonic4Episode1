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
    private static AppMain.OBS_OBJECT_WORK GmGmkRockChaseManagerInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmGmkRockHookInit(eve_rec, pos_x, pos_y, type);
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obsObjectWork;
        ((AppMain.GMS_GMK_ROCK_CHASE_WORK)AppMain.GmEventMgrLocalEventBirth((ushort)307, pos_x, pos_y, eve_rec.flag, eve_rec.left, eve_rec.top, eve_rec.width, eve_rec.height, (byte)0)).hook_work = gmsEnemy3DWork;
        return obsObjectWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkRockFallManagerInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_ROCK_FALL_MGR_WORK mgr_work = (AppMain.GMS_GMK_ROCK_FALL_MGR_WORK)AppMain.gmGmkRockLoadObjNoModel(eve_rec, pos_x, pos_y, type, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_ROCK_FALL_MGR_WORK()));
        AppMain.OBS_OBJECT_WORK objWork = mgr_work.enemy_work.ene_com.obj_work;
        AppMain.gmGmkRockManagerInit(objWork);
        AppMain.gmGmkRockFallMgrSetInterval(mgr_work, (int)eve_rec.left * 60);
        AppMain.gmGmkRockFallMgrSetUserTimer(objWork, (int)eve_rec.left * 60);
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)306, objWork.pos.x, objWork.pos.y - (int)eve_rec.top * 2 * 4096 + 262144, eve_rec.flag, eve_rec.left, eve_rec.top, eve_rec.width, eve_rec.height, (byte)0);
        obsObjectWork.flag |= 16U;
        obsObjectWork.parent_obj = objWork;
        mgr_work.hook_work = (AppMain.GMS_ENEMY_3D_WORK)obsObjectWork;
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkRockFallInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_ROCK_FALL_WORK gmsGmkRockFallWork = (AppMain.GMS_GMK_ROCK_FALL_WORK)AppMain.gmGmkRockLoadObj(eve_rec, pos_x, pos_y, type, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_ROCK_FALL_WORK()));
        AppMain.OBS_OBJECT_WORK objWork = gmsGmkRockFallWork.enemy_work.ene_com.obj_work;
        AppMain.gmGmkRockFallInit(objWork);
        gmsGmkRockFallWork.wait_time = type == (byte)0 ? 0 : 30;
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkRockHookInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkRockLoadObjHook(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        AppMain.gmGmkRockHookInit(objWork);
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkRockChaseInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work = (AppMain.GMS_GMK_ROCK_CHASE_WORK)AppMain.gmGmkRockLoadObj(eve_rec, pos_x, pos_y, type, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_ROCK_CHASE_WORK()));
        AppMain.OBS_OBJECT_WORK objWork = rock_work.enemy_work.ene_com.obj_work;
        AppMain.gmGmkRockChaseInit(objWork);
        AppMain.gmGmkRockChaseSetLength(rock_work, (int)eve_rec.left * 2 * 4096);
        AppMain.gmGmkRockChaseSetSpeed(rock_work, (int)eve_rec.top * 2 * 4096);
        return objWork;
    }

    public static void GmGmkRockBuild()
    {
        AppMain.g_gm_gmk_rock_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(816)), AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(817)), 0U);
    }

    public static void GmGmkRockFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(816));
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_rock_obj_3d_list, amsAmbHeader.file_num);
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkRockLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      AppMain.TaskWorkFactoryDelegate work_size)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, work_size, "GMK_ROCK");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkRockLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      AppMain.TaskWorkFactoryDelegate work_size)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkRockLoadObjNoModel(eve_rec, pos_x, pos_y, type, work_size);
        AppMain.OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.g_gm_gmk_rock_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return gmsEnemy3DWork;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkRockLoadObjHook(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int pos_y1 = pos_y >> 17 << 17;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkRockLoadObjNoModel(eve_rec, pos_x, pos_y1, type, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()));
        AppMain.OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.g_gm_gmk_rock_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(objWork, 0, false, AppMain.ObjDataGet(818), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        return gmsEnemy3DWork;
    }

    private static void gmGmkRockMoveFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjObjectMove(obj_work);
    }

    private static void gmGmkRockFallDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.VecU16 vecU16 = new AppMain.VecU16(obj_work.dir);
        ushort roll = ((AppMain.GMS_GMK_ROCK_FALL_WORK)obj_work).roll;
        obj_work.dir.y = roll;
        ushort userWork = (ushort)obj_work.user_work;
        obj_work.dir.z = userWork;
        obj_work.dir.z += roll;
        AppMain.ObjDrawActionSummary(obj_work);
        AppMain.ObjDrawActionSummary(obj_work);
        obj_work.dir.Assign(vecU16);
    }

    private static void gmGmkRockChaseDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work = (AppMain.GMS_GMK_ROCK_CHASE_WORK)obj_work;
        AppMain.VecU16 vecU16 = new AppMain.VecU16(obj_work.dir);
        ushort angleZ = AppMain.gmGmkRockChaseGetAngleZ(rock_work);
        obj_work.dir.z = obj_work.spd_m >= 0 ? angleZ : angleZ;
        ushort userWork = (ushort)obj_work.user_work;
        obj_work.dir.x = userWork;
        obj_work.pos.y += rock_work.current_bound;
        AppMain.ObjDrawActionSummary(obj_work);
        AppMain.ObjDrawActionSummary(obj_work);
        obj_work.dir.Assign(vecU16);
        obj_work.pos.y -= rock_work.current_bound;
    }

    private static void gmGmkRockChaseTcbDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GMK_ROCK_CHASE_WORK tcbWork = (AppMain.GMS_GMK_ROCK_CHASE_WORK)AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.se_handle != null)
        {
            AppMain.GmSoundStopSE(tcbWork.se_handle);
            AppMain.GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkRockWaitDefFunc(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj = own_rect.parent_obj;
        AppMain.GMS_GMK_ROCK_CHASE_WORK gmkRockChaseWork = (AppMain.GMS_GMK_ROCK_CHASE_WORK)parentObj;
        AppMain.gmGmkRockHookkChangeModeActive((AppMain.OBS_OBJECT_WORK)gmkRockChaseWork.hook_work);
        gmkRockChaseWork.hook_work = (AppMain.GMS_ENEMY_3D_WORK)null;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate(parentObj, 2, 32);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
        gmsEffect3DesWork.efct_com.obj_work.parent_ofst.y = 204800;
        AppMain.gmGmkRockChaseChangeModeFall(parentObj);
    }

    private static void gmGmkRockSetRectActive(AppMain.GMS_ENEMY_3D_WORK gimmick_work)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        AppMain.ObjRectWorkZSet(pRec, (short)-40, (short)-40, (short)-500, (short)40, (short)40, (short)500);
        pRec.flag |= 1024U;
        AppMain.ObjRectAtkSet(pRec, (ushort)2, (short)1);
        AppMain.ObjRectDefSet(pRec, (ushort)0, (short)0);
        pRec.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
    }

    private static void gmGmkRockSetRectWait(AppMain.GMS_ENEMY_3D_WORK gimmick_work)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        AppMain.ObjRectWorkZSet(pRec, (short)-40, (short)-40, (short)-500, (short)40, (short)500, (short)500);
        pRec.flag |= 1024U;
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkRockWaitDefFunc);
    }

    private static void gmGmkRockChaseInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gimmick_work = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.gmGmkRockSetRectWait(gimmick_work);
        AppMain.ObjObjectFieldRectSet(obj_work, (short)-28, (short)-28, (short)28, (short)42);
        obj_work.disp_flag |= 4194304U;
        gimmick_work.ene_com.target_obj = AppMain.g_gm_main_system.ply_work[0].obj_work;
        obj_work.pos.z = -131072;
        AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work = (AppMain.GMS_GMK_ROCK_CHASE_WORK)obj_work;
        ushort angle_z = AppMain.mtMathRand();
        AppMain.gmGmkRockChaseSetAngleZ(rock_work, angle_z);
        obj_work.user_work = (uint)AppMain.mtMathRand();
        rock_work.se_handle = AppMain.GsSoundAllocSeHandle();
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockMoveFunc);
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockChaseDrawFunc);
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkRockChaseTcbDest));
    }

    private static void gmGmkRockChaseChangeModeFall(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_ROCK_CHASE_WORK gmkRockChaseWork = (AppMain.GMS_GMK_ROCK_CHASE_WORK)obj_work;
        AppMain.gmGmkRockSetRectActive((AppMain.GMS_ENEMY_3D_WORK)obj_work);
        obj_work.spd_m = 0;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        obj_work.dir.z = (ushort)0;
        obj_work.flag |= 16U;
        obj_work.move_flag |= 192U;
        obj_work.move_flag &= 4294443007U;
        if (gmkRockChaseWork.se_handle != null)
            AppMain.GmSoundStopSE(gmkRockChaseWork.se_handle);
        if (gmkRockChaseWork.flag_vib)
        {
            AppMain.GMM_PAD_VIB_STOP();
            gmkRockChaseWork.flag_vib = false;
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockChaseMainFall);
    }

    private static void gmGmkRockChaseChangeModeChase(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work = (AppMain.GMS_GMK_ROCK_CHASE_WORK)obj_work;
        obj_work.spd_m = 0;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        AppMain.gmGmkRockChaseSetDirType(rock_work, 0U);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockChaseMainChase);
        if (rock_work.effect_work == null)
        {
            AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate(obj_work, 2, 24);
            gmsEffect3DesWork.efct_com.obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
            gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
            gmsEffect3DesWork.efct_com.obj_work.parent_ofst.y = 204800;
            rock_work.effect_work = gmsEffect3DesWork;
        }
        AppMain.GmSoundPlaySE("BigRock2", rock_work.se_handle);
    }

    private static void gmGmkRockChaseChangeModeEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_ROCK_CHASE_WORK gmkRockChaseWork = (AppMain.GMS_GMK_ROCK_CHASE_WORK)obj_work;
        if (gmkRockChaseWork.flag_vib)
        {
            AppMain.GMM_PAD_VIB_STOP();
            gmkRockChaseWork.flag_vib = false;
        }
        obj_work.flag &= 4294967279U;
        obj_work.move_flag |= 256U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmGmkRockChaseMainFall(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.move_flag & 1) != 0)
        {
            AppMain.GmSoundPlaySE("BigRock1");
            AppMain.GmCameraVibrationSet(0, 12288, 0);
            AppMain.gmGmkRockChaseChangeModeChase(obj_work);
        }
        else
        {
            if (AppMain.g_gm_main_system.ply_work[0].obj_work.pos.y >= obj_work.pos.y - 2097152)
                return;
            AppMain.gmGmkRockChaseChangeModeEnd(obj_work);
        }
    }

    private static void gmGmkRockChaseMainChase(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work = (AppMain.GMS_GMK_ROCK_CHASE_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK targetObj = ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.target_obj;
        int a = targetObj.pos.x - obj_work.pos.x;
        uint type = AppMain.gmGmkRockChaseGetDirType(rock_work);
        if (type == 0U)
        {
            type = (int)obj_work.dir.z > AppMain.NNM_DEGtoA16(180f) ? 2U : 1U;
            AppMain.gmGmkRockChaseSetDirType(rock_work, type);
        }
        int num1;
        int num2;
        if (type == 1U && a < 0 || type == 2U && a >= 0)
        {
            num1 = 768;
            num2 = 32768;
            if (rock_work.flag_vib)
            {
                AppMain.GMM_PAD_VIB_STOP();
                rock_work.flag_vib = false;
            }
        }
        else
        {
            int num3 = AppMain.MTM_MATH_ABS(targetObj.spd_m);
            if (num3 < AppMain.gmGmkRockChaseGetSpeed(rock_work))
            {
                num1 = 512;
                num2 = 65536;
            }
            else
            {
                int num4 = AppMain.MTM_MATH_ABS(a);
                int length = AppMain.gmGmkRockChaseGetLength(rock_work);
                int num5 = AppMain.FX_Mul(length, 8192);
                if (num5 < 1228800)
                    num5 = 1228800;
                if (num4 > num5)
                {
                    num1 = 3840;
                    num2 = (int)((long)num3 + 32768L);
                    if (rock_work.flag_vib)
                    {
                        AppMain.GMM_PAD_VIB_STOP();
                        rock_work.flag_vib = false;
                    }
                }
                else if (num4 > length)
                {
                    num1 = 768;
                    num2 = (int)((long)num3 + 10240L);
                    if (!rock_work.flag_vib)
                    {
                        AppMain.GMM_PAD_VIB_MID_NOEND();
                        rock_work.flag_vib = true;
                    }
                }
                else
                {
                    num1 = -768;
                    num2 = (int)((long)num3 + -6144L);
                    if (!rock_work.flag_vib)
                    {
                        AppMain.GMM_PAD_VIB_MID_NOEND();
                        rock_work.flag_vib = true;
                    }
                }
            }
        }
        if (type == 1U)
        {
            obj_work.spd_m += num1;
            AppMain.gmGmkRockChaseAddAngleZ(rock_work, (short)1000);
            if (obj_work.spd_m > num2)
                obj_work.spd_m = num2;
        }
        else
        {
            int num3 = -num1;
            int num4 = -num2;
            obj_work.spd_m += num3;
            AppMain.gmGmkRockChaseAddAngleZ(rock_work, (short)-1000);
            if (obj_work.spd_m < num4)
                obj_work.spd_m = num4;
        }
        if (((int)obj_work.move_flag & 1) == 0)
            AppMain.gmGmkRockChaseChangeModeFall(obj_work);
        else if (rock_work.current_bound >= 0)
        {
            rock_work.current_bound = 0;
            if ((int)AppMain.mtMathRand() % 10 != 0)
                return;
            int num3 = 32 + (int)AppMain.mtMathRand() % 16;
            rock_work.target_bound = -num3 * 4096;
            rock_work.current_bound -= 8192;
            if (rock_work.se_handle == null)
                return;
            AppMain.GmSoundStopSE(rock_work.se_handle);
        }
        else if (rock_work.target_bound > rock_work.current_bound)
        {
            rock_work.target_bound = 0;
            rock_work.current_bound += 8192;
            if (rock_work.current_bound < 0)
                return;
            AppMain.GmSoundPlaySE("BigRock1");
            AppMain.GmSoundPlaySE("BigRock2", rock_work.se_handle);
            AppMain.GmCameraVibrationSet(0, 12288, 0);
        }
        else
            rock_work.current_bound -= 8192;
    }

    private static void gmGmkRockManagerInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 8448U;
        AppMain.gmGmkRockFallMgrSetUserTimer(obj_work, 0);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockManagerMain);
    }

    private static void gmGmkRockManagerMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_ROCK_FALL_MGR_WORK mgr_work = (AppMain.GMS_GMK_ROCK_FALL_MGR_WORK)obj_work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        int interval = AppMain.gmGmkRockFallMgrGetInterval(mgr_work);
        if (AppMain.gmGmkRockFallMgrGetUserTimer(obj_work) >= interval)
        {
            AppMain.gmGmkRockFallMgrSetUserTimer(obj_work, 0);
            byte type = 0;
            if (interval >= 120)
                type = (byte)1;
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)300, obj_work.pos.x, obj_work.pos.y - (int)gmsEnemy3DWork.ene_com.eve_rec.top * 2 * 4096, gmsEnemy3DWork.ene_com.eve_rec.flag, gmsEnemy3DWork.ene_com.eve_rec.left, gmsEnemy3DWork.ene_com.eve_rec.top, gmsEnemy3DWork.ene_com.eve_rec.width, gmsEnemy3DWork.ene_com.eve_rec.height, type);
            obsObjectWork.spd_fall = 336;
            obsObjectWork.spd_fall_max = 32768;
            ((AppMain.GMS_GMK_ROCK_FALL_WORK)obsObjectWork).hook_work = mgr_work.hook_work;
        }
        AppMain.gmGmkRockFallMgrAddUserTimer(obj_work, 1);
    }

    private static void gmGmkRockFallInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkRockSetRectActive((AppMain.GMS_ENEMY_3D_WORK)obj_work);
        obj_work.move_flag |= 384U;
        obj_work.disp_flag |= 4194304U;
        obj_work.pos.z = -131072;
        obj_work.user_work = (uint)AppMain.mtMathRand();
        AppMain.GMS_GMK_ROCK_FALL_WORK gmsGmkRockFallWork = (AppMain.GMS_GMK_ROCK_FALL_WORK)obj_work;
        gmsGmkRockFallWork.roll = AppMain.mtMathRand();
        gmsGmkRockFallWork.roll_d = (ushort)128;
        if ((int)gmsGmkRockFallWork.roll % 2 != 0)
            gmsGmkRockFallWork.roll_d = (ushort)-gmsGmkRockFallWork.roll_d;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockFallMainStart);
        obj_work.ppMove = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockMoveFunc);
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockFallDrawFunc);
    }

    private static void gmGmkRockFallMainStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK hookWork = (AppMain.OBS_OBJECT_WORK)((AppMain.GMS_GMK_ROCK_FALL_WORK)obj_work).hook_work;
        if (hookWork.pos.y + 98304 > obj_work.pos.y)
            return;
        AppMain.GmEfctZoneEsCreate(obj_work, 2, 17).efct_com.obj_work.pos.z = 131072;
        AppMain.GmSoundPlaySE("BigRock4");
        obj_work.pos.y = hookWork.pos.y + 98304;
        AppMain.GmCameraVibrationSet(0, 4096, 0);
        obj_work.move_flag &= 4294967167U;
        obj_work.spd.y = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockFallMainWait);
    }

    private static void gmGmkRockFallMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_ROCK_FALL_WORK gmsGmkRockFallWork = (AppMain.GMS_GMK_ROCK_FALL_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK hookWork = (AppMain.OBS_OBJECT_WORK)gmsGmkRockFallWork.hook_work;
        ++obj_work.user_timer;
        if (obj_work.user_timer < gmsGmkRockFallWork.wait_time)
            return;
        obj_work.user_timer = 0;
        obj_work.move_flag |= 128U;
        AppMain.gmGmkRockHookkChangeModeActive(hookWork);
        gmsGmkRockFallWork.hook_work = (AppMain.GMS_ENEMY_3D_WORK)null;
        AppMain.GmSoundPlaySE("BigRock5");
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockFallMainFallWaitEffect);
    }

    private static void gmGmkRockFallMainFallWaitEffect(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_ROCK_FALL_WORK gmsGmkRockFallWork = (AppMain.GMS_GMK_ROCK_FALL_WORK)obj_work;
        gmsGmkRockFallWork.roll += gmsGmkRockFallWork.roll_d;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 30)
            return;
        obj_work.user_timer = 0;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate(obj_work, 2, 32);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        gmsEffect3DesWork.efct_com.obj_work.pos.y -= 262144;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
        gmsEffect3DesWork.efct_com.obj_work.parent_ofst.y = 204800;
        gmsGmkRockFallWork.effect_work = gmsEffect3DesWork;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockFallMainFall);
    }

    private static void gmGmkRockFallMainFall(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 128U;
        AppMain.GMS_GMK_ROCK_FALL_WORK gmsGmkRockFallWork = (AppMain.GMS_GMK_ROCK_FALL_WORK)obj_work;
        gmsGmkRockFallWork.roll += gmsGmkRockFallWork.roll_d;
    }

    private static void gmGmkRockHookInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        obj_work.pos.z = 0;
        AppMain.gmGmkRockHookChangeModeWait(obj_work);
    }

    private static void gmGmkRockHookChangeModeWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, 0, 0);
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmGmkRockHookkChangeModeActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, 1, 0);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockHookMainActive);
    }

    private static void gmGmkRockHookMainActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        AppMain.gmGmkRockHookChangeModeWait(obj_work);
    }

    private static void gmGmkRockChaseSetLength(AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work, int length)
    {
        rock_work.length = length;
    }

    private static int gmGmkRockChaseGetLength(AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work)
    {
        return rock_work.length;
    }

    private static void gmGmkRockChaseSetSpeed(AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work, int speed)
    {
        rock_work.speed = speed;
    }

    private static int gmGmkRockChaseGetSpeed(AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work)
    {
        return rock_work.speed;
    }

    private static void gmGmkRockChaseSetAngleZ(
      AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work,
      ushort angle_z)
    {
        rock_work.angle_z = angle_z;
    }

    private static void gmGmkRockChaseAddAngleZ(
      AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work,
      short angle_z)
    {
        rock_work.angle_z += (ushort)angle_z;
    }

    private static ushort gmGmkRockChaseGetAngleZ(AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work)
    {
        return rock_work.angle_z;
    }

    private static void gmGmkRockChaseSetDirType(AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work, uint type)
    {
        rock_work.dir_type = type;
    }

    private static uint gmGmkRockChaseGetDirType(AppMain.GMS_GMK_ROCK_CHASE_WORK rock_work)
    {
        return rock_work.dir_type;
    }

    private static void gmGmkRockFallMgrSetInterval(
      AppMain.GMS_GMK_ROCK_FALL_MGR_WORK mgr_work,
      int interval)
    {
        mgr_work.interval = interval;
    }

    private static int gmGmkRockFallMgrGetInterval(AppMain.GMS_GMK_ROCK_FALL_MGR_WORK mgr_work)
    {
        return mgr_work.interval;
    }

    private static void gmGmkRockFallMgrSetUserTimer(AppMain.OBS_OBJECT_WORK obj_work, int count)
    {
        obj_work.user_timer = count;
    }

    private static void gmGmkRockFallMgrAddUserTimer(AppMain.OBS_OBJECT_WORK obj_work, int count)
    {
        obj_work.user_timer += count;
    }

    private static int gmGmkRockFallMgrGetUserTimer(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_timer;
    }

}