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
    private static AppMain.OBS_OBJECT_WORK GmGmkRockRideInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkRockRideLoadObj(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        AppMain.gmGmkRockRideWaitInit(objWork);
        return objWork;
    }

    public static void GmGmkRockRideBuild()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(817));
        AppMain.TXB_HEADER txb = AppMain.readTXBfile(AppMain.amBindGet(amsAmbHeader, 0));
        AppMain.g_gm_gmk_rock_ride_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(816)), amsAmbHeader, 0U, txb);
    }

    public static void GmGmkRockRideFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(816));
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_rock_ride_obj_3d_list, amsAmbHeader.file_num);
        AppMain.g_gm_gmk_rock_ride_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkRockRideLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_ROCK_WORK work = (AppMain.GMS_GMK_ROCK_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_ROCK_WORK()), "GMK_ROCK_RIDE");
        AppMain.GMS_ENEMY_3D_WORK enemyWork = work.enemy_work;
        AppMain.OBS_OBJECT_WORK objWork = work.enemy_work.ene_com.obj_work;
        enemyWork.ene_com.rect_work[0].flag &= 4294967291U;
        enemyWork.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.g_gm_gmk_rock_ride_obj_3d_list[0], enemyWork.obj_3d);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return enemyWork;
    }

    private static void gmGmkRockRideMoveFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjObjectMove(obj_work);
    }

    private static void gmGmkRockRideDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.VecU16 vecU16 = new AppMain.VecU16(obj_work.dir);
        ushort userTimerAngleZ = AppMain.gmGmkRockRideGetUserTimerAngleZ(obj_work);
        obj_work.dir.z = obj_work.spd_m >= 0 ? userTimerAngleZ : userTimerAngleZ;
        ushort userWork = (ushort)obj_work.user_work;
        obj_work.dir.x = userWork;
        AppMain.ObjDrawActionSummary(obj_work);
        obj_work.dir.Assign(vecU16);
    }

    private static void gmGmkRockRideTcbDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GMK_ROCK_WORK tcbWork = (AppMain.GMS_GMK_ROCK_WORK)AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.se_handle != null)
        {
            AppMain.GmSoundStopSE(tcbWork.se_handle);
            AppMain.GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkRockRideSetUserTimerAngleZ(
      AppMain.OBS_OBJECT_WORK obj_work,
      ushort angle_z)
    {
        obj_work.user_timer = (int)angle_z;
    }

    private static void gmGmkRockRideAddUserTimerAngleZ(
      AppMain.OBS_OBJECT_WORK obj_work,
      short angle_z)
    {
        obj_work.user_timer += (int)angle_z;
    }

    private static ushort gmGmkRockRideGetUserTimerAngleZ(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return (ushort)obj_work.user_timer;
    }

    private static void gmGmkRockRideWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gimmick_work = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.gmGmkRockRideWaitSetRect(gimmick_work);
        obj_work.flag |= 1U;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        obj_work.spd_m = 0;
        gimmick_work.ene_com.target_obj = AppMain.g_gm_main_system.ply_work[0].obj_work;
        ushort angle_z = AppMain.mtMathRand();
        AppMain.gmGmkRockRideSetUserTimerAngleZ(obj_work, angle_z);
        obj_work.user_work = (uint)AppMain.mtMathRand();
        ((AppMain.GMS_GMK_ROCK_WORK)obj_work).se_handle = AppMain.GsSoundAllocSeHandle();
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockRideWaitMain);
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockRideDrawFunc);
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkRockRideTcbDest));
    }

    private static void gmGmkRockRideWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkRockRideWaitDefFunc(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = own_rect.parent_obj;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)parentObj1;
        AppMain.GMS_ENEMY_COM_WORK eneCom = gmsEnemy3DWork.ene_com;
        AppMain.OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != (ushort)1)
            return;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)parentObj2;
        if (((int)parentObj2.move_flag & 1) != 0)
        {
            if (gmsEnemy3DWork.ene_com.eve_rec.flag != (ushort)0)
            {
                if (parentObj1.pos.x >= parentObj2.pos.x)
                    return;
            }
            else if (parentObj1.pos.x <= parentObj2.pos.x)
                return;
            AppMain.GmPlySeqInitRockRideStart(ply_work, eneCom);
            AppMain.gmGmkRockRideStartInit(parentObj1);
        }
        else
        {
            int num1 = parentObj1.pos.x - parentObj2.pos.x;
            int num2 = parentObj1.pos.y - parentObj2.pos.y;
            if (AppMain.FX_Mul(num1, num1) + AppMain.FX_Mul(num2, num2) > 12845056 || ply_work.seq_state != 17 && ply_work.seq_state != 21 && (ply_work.seq_state != 16 && ply_work.seq_state != 29))
                return;
            int spd_x = -(229376 - AppMain.MTM_MATH_ABS(num1)) / 30;
            int spd_y = 0;
            if (parentObj1.pos.x < parentObj2.pos.x)
                spd_x = -spd_x;
            AppMain.GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, 60, 1, 0);
        }
    }

    private static void gmGmkRockRideWaitSetRect(AppMain.GMS_ENEMY_3D_WORK gimmick_work)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        AppMain.ObjRectWorkZSet(pRec, (short)-48, (short)-56, (short)-500, (short)48, (short)56, (short)500);
        pRec.flag &= 4294966271U;
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkRockRideWaitDefFunc);
    }

    private static void gmGmkRockRideStartInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkRockRideStartSetRect((AppMain.GMS_ENEMY_3D_WORK)obj_work);
        AppMain.ObjObjectFieldRectSet(obj_work, (short)-16, (short)-16, (short)16, (short)16);
        obj_work.flag &= 4294967294U;
        obj_work.move_flag &= 4294958847U;
        obj_work.move_flag |= 192U;
        obj_work.move_flag &= 4294836223U;
        obj_work.spd_m = 0;
        AppMain.GmSoundPlaySE("BigRock3", ((AppMain.GMS_GMK_ROCK_WORK)obj_work).se_handle);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockRideStartMain);
        obj_work.ppMove = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockRideMoveFunc);
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockRideDrawFunc);
    }

    private static void gmGmkRockRideStartMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK targetObj = ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.target_obj;
        AppMain.gmGmkRockRideAddUserTimerAngleZ(obj_work, (short)(obj_work.spd_m >> 4));
        int num = 224;
        if (obj_work.pos.x < targetObj.pos.x)
            num = -num;
        obj_work.spd_m += num;
        if (AppMain.MTM_MATH_ABS(obj_work.spd_m) > 12288)
            AppMain.gmGmkRockRideRollInit(obj_work);
        float val = AppMain.FX_FX32_TO_F32(AppMain.FX_Div(AppMain.MTM_MATH_ABS(obj_work.spd_m), 6));
        if ((double)val > 1.0)
            val = 1f;
        AppMain.GMS_GMK_ROCK_WORK gmsGmkRockWork = (AppMain.GMS_GMK_ROCK_WORK)obj_work;
        if (gmsGmkRockWork.se_handle == null)
            return;
        gmsGmkRockWork.se_handle.au_player.SetAisac("Speed", val);
    }

    private static void gmGmkRockRideStartSetRect(AppMain.GMS_ENEMY_3D_WORK gimmick_work)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        AppMain.ObjRectDefSet(pRec, ushort.MaxValue, (short)3);
        pRec.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
    }

    private static void gmGmkRockRideRollInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkRockRideRollSetRect((AppMain.GMS_ENEMY_3D_WORK)obj_work);
        AppMain.ObjObjectFieldRectSet(obj_work, (short)-16, (short)-16, (short)16, (short)16);
        obj_work.flag &= 4294967294U;
        obj_work.move_flag &= 4294958847U;
        obj_work.move_flag |= 131264U;
        obj_work.spd_slope = 192;
        obj_work.spd_slope_max = 61440;
        obj_work.pos.z = 131072;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockRideRollMainNoPlayer);
        obj_work.ppMove = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockRideMoveFunc);
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockRideDrawFunc);
        AppMain.GMS_GMK_ROCK_WORK gmsGmkRockWork = (AppMain.GMS_GMK_ROCK_WORK)obj_work;
        if (gmsGmkRockWork.effect_work != null)
            return;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate(obj_work, 2, 18);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        gmsEffect3DesWork.efct_com.obj_work.parent_ofst.z = 98304;
        gmsEffect3DesWork.efct_com.obj_work.parent_ofst.y = 131072;
        gmsGmkRockWork.effect_work = gmsEffect3DesWork;
    }

    private static void gmGmkRockRideRollMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkRockRideAddUserTimerAngleZ(obj_work, (short)(obj_work.spd_m >> 4));
        float val = AppMain.FX_FX32_TO_F32(AppMain.FX_Div(AppMain.MTM_MATH_ABS(obj_work.spd_m), 6));
        if ((double)val > 1.0)
            val = 1f;
        AppMain.GMS_GMK_ROCK_WORK gmsGmkRockWork = (AppMain.GMS_GMK_ROCK_WORK)obj_work;
        if (gmsGmkRockWork.se_handle != null)
            gmsGmkRockWork.se_handle.au_player.SetAisac("Speed", val);
        if (((int)obj_work.move_flag & 4) != 0 || ((int)obj_work.move_flag & 8) != 0)
            AppMain.gmGmkRockRideStopInit(obj_work);
        if (gmsGmkRockWork.vib_timer % 30 == 0)
            AppMain.GMM_PAD_VIB_SMALL_TIME(10f);
        ++gmsGmkRockWork.vib_timer;
        if (AppMain.g_gm_main_system.ply_work[0].seq_state == 31)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockRideRollMainNoPlayer);
        AppMain.GMM_PAD_VIB_STOP();
        gmsGmkRockWork.vib_timer = 0;
        obj_work.pos.z = -262144;
    }

    private static void gmGmkRockRideRollMainNoPlayer(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkRockRideAddUserTimerAngleZ(obj_work, (short)(obj_work.spd_m >> 4));
        float val = AppMain.FX_FX32_TO_F32(AppMain.FX_Div(AppMain.MTM_MATH_ABS(obj_work.spd_m), 6));
        if ((double)val > 1.0)
            val = 1f;
        AppMain.GMS_GMK_ROCK_WORK gmsGmkRockWork = (AppMain.GMS_GMK_ROCK_WORK)obj_work;
        if (gmsGmkRockWork.se_handle != null)
            gmsGmkRockWork.se_handle.au_player.SetAisac("Speed", val);
        if (((int)obj_work.move_flag & 4) == 0 && ((int)obj_work.move_flag & 8) == 0)
            return;
        AppMain.gmGmkRockRideStopInit(obj_work);
    }

    private static void gmGmkRockRideRollDefFunc(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = own_rect.parent_obj;
        AppMain.GMS_ENEMY_COM_WORK eneCom = ((AppMain.GMS_ENEMY_3D_WORK)parentObj1).ene_com;
        AppMain.OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != (ushort)1)
            return;
        AppMain.GmPlySeqInitRockRide((AppMain.GMS_PLAYER_WORK)parentObj2, eneCom);
        own_rect.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkRockRideRollDefFunc);
        parentObj1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkRockRideRollMain);
        parentObj1.pos.z = 131072;
    }

    private static void gmGmkRockRideRollSetRect(AppMain.GMS_ENEMY_3D_WORK gimmick_work)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        AppMain.ObjRectWorkZSet(pRec, (short)-48, (short)-48, (short)-500, (short)48, (short)48, (short)500);
        pRec.flag |= 1024U;
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkRockRideRollDefFunc);
    }

    private static void gmGmkRockRideStopInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkRockRideStopSetRect((AppMain.GMS_ENEMY_3D_WORK)obj_work);
        AppMain.ObjObjectFieldRectSet(obj_work, (short)-16, (short)-16, (short)16, (short)16);
        obj_work.flag &= 4294967294U;
        obj_work.move_flag |= 256U;
        obj_work.move_flag &= 4294967294U;
        obj_work.spd_slope = 0;
        obj_work.spd_slope_max = 0;
        obj_work.spd_m = 0;
        AppMain.GMS_GMK_ROCK_WORK gmsGmkRockWork = (AppMain.GMS_GMK_ROCK_WORK)obj_work;
        if (gmsGmkRockWork.se_handle != null)
            AppMain.GmSoundStopSE(gmsGmkRockWork.se_handle);
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        if (gmsGmkRockWork.effect_work != null)
            AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)gmsGmkRockWork.effect_work);
        AppMain.GMM_PAD_VIB_STOP();
    }

    private static void gmGmkRockRideStopSetRect(AppMain.GMS_ENEMY_3D_WORK gimmick_work)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        pRec.flag |= 1024U;
        AppMain.ObjRectDefSet(pRec, ushort.MaxValue, (short)3);
        pRec.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
    }


}