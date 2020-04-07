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
    private static void GmGmkTarzanRopeBuild()
    {
        AppMain.g_gm_gmk_tarzan_rope_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(829)), AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(830)), 0U);
    }

    private static void GmGmkTarzanRopeFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(829));
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_tarzan_rope_obj_3d_list, amsAmbHeader.file_num);
        AppMain.g_gm_gmk_tarzan_rope_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkTarzanRopeInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int type1;
        switch (eve_rec.id)
        {
            case 112:
                type1 = 0;
                break;
            case 113:
                type1 = 1;
                break;
            case 114:
                type1 = 2;
                break;
            default:
                return (AppMain.OBS_OBJECT_WORK)null;
        }
        float length = (float)(1.0 + (double)eve_rec.left / 100.0);
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkTarzanRopeLoadObj(eve_rec, pos_x, pos_y, type1).ene_com.obj_work;
        AppMain.gmGmkTarzanRopeInit(objWork, type1, length);
        return objWork;
    }

    private static void gmGmkTarzanRopeMotionCallback(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_OBJECT obj,
      object val)
    {
        if (((AppMain.GMS_ENEMY_3D_WORK)(AppMain.OBS_OBJECT_WORK)val).ene_com.target_obj == null)
            return;
        AppMain.NNS_MATRIX nnsMatrix = new AppMain.NNS_MATRIX();
        AppMain.nnMakeUnitMatrix(nnsMatrix);
        AppMain.nnMultiplyMatrix(nnsMatrix, nnsMatrix, AppMain.amMatrixGetCurrent());
        AppMain.nnCalcNodeMatrixTRSList(AppMain.g_gm_gmk_tarzan_rope_active_matrix, obj, 13, (AppMain.ArrayPointer<AppMain.NNS_TRS>)motion.data, nnsMatrix);
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkTarzanRopeLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_T_ROPE");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkTarzanRopeLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkTarzanRopeLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        AppMain.OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index1 = eve_rec.left < (sbyte)50 ? (eve_rec.left < (sbyte)20 ? 0 : 1) : 2;
        int index2 = AppMain.g_gm_gmk_tarzan_rope_model_id[index1];
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.g_gm_gmk_tarzan_rope_obj_3d_list[index2], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(objWork, 0, false, AppMain.ObjDataGet(831), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        objWork.obj_3d.mtn_cb_func = new AppMain.mtn_cb_func_delegate(AppMain.gmGmkTarzanRopeMotionCallback);
        objWork.obj_3d.mtn_cb_param = (object)objWork;
        int fx32 = AppMain.FX_F32_TO_FX32(objWork.obj_3d._object.pNodeList[0].Translation.y * ((float)eve_rec.left / 30f));
        objWork.pos.y -= fx32;
        return gmsEnemy3DWork;
    }

    private static void gmGmkTarzanRopeInit(AppMain.OBS_OBJECT_WORK obj_work, int type, float length)
    {
        AppMain.gmGmkTarzanRopeSetRect((AppMain.GMS_ENEMY_3D_WORK)obj_work, type);
        obj_work.move_flag = 8448U;
        obj_work.disp_flag |= 4194304U;
        obj_work.obj_3d.drawflag |= 32U;
        int id = AppMain.g_gm_gmk_tarzan_rope_motion_id[type];
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, id, 0);
        obj_work.disp_flag |= 16U;
        int angle = 0;
        switch (type)
        {
            case 0:
                angle = 0;
                break;
            case 1:
                angle = -16384;
                break;
            case 2:
                angle = 16384;
                break;
        }
        AppMain.gmGmkTarzanRopeSetUserWorkTargetAngle(obj_work, angle);
        AppMain.gmGmkTarzanRopeSetUserTimerCurrentAngle(obj_work, angle);
        AppMain.gmGmkTarzanRopeeSetUserFlagType(obj_work, type);
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTarzanRopeDrawFunc);
    }

    private static void gmGmkTarzanRopeSetRect(AppMain.GMS_ENEMY_3D_WORK gimmick_work, int type)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        short cRight = (short)(64.0 * (1.0 + (double)gimmick_work.ene_com.eve_rec.left / 100.0));
        switch (type)
        {
            case 0:
                AppMain.ObjRectWorkZSet(pRec, (short)-32, (short)-32, (short)-500, (short)32, (short)((int)cRight - 32), (short)500);
                break;
            case 1:
                AppMain.ObjRectWorkZSet(pRec, (short)-cRight, (short)-48, (short)-500, (short)0, (short)-16, (short)500);
                break;
            case 2:
                AppMain.ObjRectWorkZSet(pRec, (short)0, (short)-48, (short)-500, cRight, (short)-16, (short)500);
                break;
        }
        pRec.flag |= 1024U;
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkTarzanRopeDefFunc);
    }

    private static float gmGmkTarzanRopeCalcFlame(AppMain.OBS_OBJECT_WORK obj_work, int angle)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        int num1 = 16384;
        int num2 = -16384;
        if (angle > num1)
            angle = num1;
        else if (angle < num2)
            angle = num2;
        float startFrame = AppMain.amMotionGetStartFrame(obj3d.motion, obj3d.act_id[0]);
        float endFrame = AppMain.amMotionGetEndFrame(obj3d.motion, obj3d.act_id[0]);
        return (float)(((double)endFrame - (double)startFrame) / 2.0) - (float)((double)((float)angle / (float)num1) * ((double)endFrame - (double)startFrame) / 4.0);
    }

    private static void gmGmkTarzanRopeDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (obj3d.motion == null)
            return;
        obj3d.frame[0] = AppMain.gmGmkTarzanRopeCalcFlame(obj_work, AppMain.gmGmkTarzanRopeGetUserTimerCurrentAngle(obj_work));
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void gmGmkTarzanRopeDefFunc(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = own_rect.parent_obj;
        AppMain.GMS_ENEMY_3D_WORK gimmick_work = (AppMain.GMS_ENEMY_3D_WORK)parentObj1;
        AppMain.OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != (ushort)1)
            return;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)parentObj2;
        if (ply_work.seq_state == 35)
            return;
        if (ply_work.seq_state == 19)
            AppMain.GMM_PAD_VIB_SMALL();
        AppMain.GmPlySeqInitTarzanRope(ply_work, gimmick_work.ene_com);
        parentObj1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTarzanRopeMainWait);
        gimmick_work.ene_com.target_obj = parentObj2;
        int userFlagType = AppMain.gmGmkTarzanRopeGetUserFlagType(parentObj1);
        int angle;
        if (userFlagType == 0)
        {
            angle = parentObj2.spd.x >> 2;
            if (angle > 240)
                angle += AppMain.MTM_MATH_ABS(parentObj2.spd.y >> 2);
            else if (angle < -240)
                angle -= AppMain.MTM_MATH_ABS(parentObj2.spd.y >> 2);
            if (angle > 16384)
                angle = 16384;
            else if (angle < -16384)
                angle = -16384;
        }
        else
            angle = AppMain.gmGmkTarzanRopeGetUserTimerCurrentAngle(parentObj1);
        switch (userFlagType)
        {
            case 0:
                ushort num1 = (ushort)((parentObj1.pos.y >> 12) - 32);
                ushort num2 = (ushort)(parentObj2.pos.y >> 12);
                gimmick_work.ene_com.enemy_flag &= 4294901760U;
                if ((int)num2 > (int)num1)
                {
                    gimmick_work.ene_com.enemy_flag |= (uint)(ushort)((uint)num2 - (uint)num1);
                    break;
                }
                break;
            case 1:
                ushort num3 = (ushort)(parentObj1.pos.x >> 12);
                ushort num4 = (ushort)(parentObj2.pos.x >> 12);
                gimmick_work.ene_com.enemy_flag &= 4294901760U;
                if ((int)num4 < (int)num3)
                {
                    gimmick_work.ene_com.enemy_flag |= (uint)(ushort)((uint)num3 - (uint)num4);
                    break;
                }
                break;
            case 2:
                ushort num5 = (ushort)(parentObj1.pos.x >> 12);
                ushort num6 = (ushort)(parentObj2.pos.x >> 12);
                gimmick_work.ene_com.enemy_flag &= 4294901760U;
                if ((int)num6 > (int)num5)
                {
                    gimmick_work.ene_com.enemy_flag |= (uint)(ushort)((uint)num6 - (uint)num5);
                    break;
                }
                break;
        }
        AppMain.gmGmkTarzanRopeSetUserWorkTargetAngle(parentObj1, angle);
        int type = 0;
        AppMain.gmGmkTarzanRopeeSetUserFlagType(parentObj1, type);
        AppMain.gmGmkTarzanRopeSetRect(gimmick_work, type);
        parentObj2.spd_m = 0;
        parentObj2.spd.x = 0;
        parentObj2.spd.y = 0;
        parentObj2.dir.z = (ushort)0;
    }

    private static int gmGmkTarzanRopeUpdateAngleCurrent(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        int num1 = AppMain.MTM_MATH_ABS(angle_target);
        int num2 = AppMain.MTM_MATH_ABS(angle_current);
        int num3 = 1920;
        float a = (float)(num1 - num2) / 20480f;
        if ((double)a > 0.550000011920929)
            a = 0.55f;
        else if ((double)a < -0.550000011920929)
            a = -0.55f;
        int angle = (int)((double)num3 * ((double)AppMain.MTM_MATH_ABS(a) + 0.0500000007450581));
        if (angle_target < angle_current)
            angle = -angle;
        return AppMain.gmGmkTarzanRopeAddUserTimerCurrentAngle(obj_work, angle);
    }

    private static int gmGmkTarzanRopeUpdateAngleTarget(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current,
      bool flag_motion_change)
    {
        if (angle_target > 0)
        {
            angle_target = AppMain.gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, -48);
            if (angle_target > 16384)
                angle_target = 16384;
            else if (angle_target < 0)
                angle_target = 0;
            if (angle_current >= angle_target)
            {
                angle_target = -angle_target;
                AppMain.gmGmkTarzanRopeSetUserWorkTargetAngle(obj_work, angle_target);
                if (flag_motion_change)
                    AppMain.gmGmkTarzanRopeChangeDirMotion(obj_work, angle_current);
            }
        }
        else if (angle_target < 0)
        {
            angle_target = AppMain.gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, 48);
            if (angle_target < -16384)
                angle_target = -16384;
            else if (angle_target > 0)
                angle_target = 0;
            if (angle_current <= angle_target)
            {
                angle_target = -angle_target;
                AppMain.gmGmkTarzanRopeSetUserWorkTargetAngle(obj_work, angle_target);
                if (flag_motion_change)
                    AppMain.gmGmkTarzanRopeChangeDirMotion(obj_work, angle_current);
            }
        }
        return angle_target;
    }

    private static void gmGmkTarzanRopeChangeDirMotion(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle_current)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.g_gm_main_system.ply_work[0].obj_work;
        AppMain.OBS_ACTION3D_NN_WORK obj3d = objWork.obj_3d;
        float startFrame = AppMain.amMotionGetStartFrame(obj3d.motion, obj3d.act_id[0]);
        float num = 34f;
        if (((int)objWork.disp_flag & 1) != 0)
        {
            if (angle_current < 0)
                obj3d.frame[0] = startFrame;
            else if (angle_current > 0)
                obj3d.frame[0] = num;
        }
        else if (angle_current < 0)
            obj3d.frame[0] = num;
        else if (angle_current > 0)
            obj3d.frame[0] = startFrame;
        objWork.disp_flag &= 4294967279U;
    }

    private static void gmGmkTarzanRopeUpdatePlayerMotion(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        AppMain.UNREFERENCED_PARAMETER((object)obj_work);
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        AppMain.OBS_ACTION3D_NN_WORK obj3d = objWork.obj_3d;
        float num1 = 34f;
        int num2 = AppMain.MTM_MATH_ABS(angle_target);
        int num3 = AppMain.MTM_MATH_ABS(angle_current);
        if (num2 < 1792 && num3 < 1792)
        {
            int gimmickRotZ = AppMain.gmGmkTarzanRopeGetGimmickRotZ(ply_work);
            if (ply_work.act_state == 64 || AppMain.MTM_MATH_ABS(gimmickRotZ) >= 208)
                return;
            AppMain.GmPlayerActionChange(ply_work, 64);
            objWork.disp_flag |= 4U;
        }
        else if (ply_work.act_state == 64)
        {
            AppMain.GmPlayerActionChange(ply_work, 63);
            objWork.disp_flag |= 4U;
        }
        else if (((int)objWork.disp_flag & 1) != 0)
        {
            if (angle_target > 0 && angle_current > 0)
            {
                if ((double)obj3d.frame[0] <= (double)num1)
                    return;
                objWork.disp_flag |= 16U;
            }
            else
            {
                if (angle_target >= 0 || angle_current >= 0 || (double)obj3d.frame[0] >= (double)num1)
                    return;
                objWork.disp_flag |= 16U;
            }
        }
        else if (angle_target > 0 && angle_current > 0)
        {
            if ((double)obj3d.frame[0] >= (double)num1)
                return;
            objWork.disp_flag |= 16U;
        }
        else
        {
            if (angle_target >= 0 || angle_current >= 0 || (double)obj3d.frame[0] <= (double)num1)
                return;
            objWork.disp_flag |= 16U;
        }
    }

    private static int gmGmkTarzanRopeApplyKeyLeft(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        AppMain.GMS_PLAYER_WORK targetObj = (AppMain.GMS_PLAYER_WORK)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.target_obj;
        int num1 = AppMain.MTM_MATH_ABS(angle_target);
        int num2 = AppMain.MTM_MATH_ABS(angle_current);
        int gimmickRotZ = AppMain.gmGmkTarzanRopeGetGimmickRotZ(targetObj);
        if (num1 < 1792 && num2 < 1792)
        {
            if (gimmickRotZ < 0)
                angle_target = AppMain.gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, -1792);
        }
        else if (angle_target < 0)
        {
            if (gimmickRotZ < 0)
                angle_target = AppMain.gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, -132);
            else if (gimmickRotZ > 0)
                angle_target = AppMain.gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, 31);
        }
        return angle_target;
    }

    private static int gmGmkTarzanRopeApplyKeyRight(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        AppMain.GMS_PLAYER_WORK targetObj = (AppMain.GMS_PLAYER_WORK)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.target_obj;
        int num1 = AppMain.MTM_MATH_ABS(angle_target);
        int num2 = AppMain.MTM_MATH_ABS(angle_current);
        int gimmickRotZ = AppMain.gmGmkTarzanRopeGetGimmickRotZ(targetObj);
        if (num1 < 1792 && num2 < 1792)
        {
            if (gimmickRotZ > 0)
                angle_target = AppMain.gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, 1792);
        }
        else if (angle_target > 0)
        {
            if (gimmickRotZ > 0)
                angle_target = AppMain.gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, 132);
            else if (gimmickRotZ < 0)
                angle_target = AppMain.gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, -31);
        }
        return angle_target;
    }

    private static void gmGmkTarzanRopeCheckStop(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        int num1 = AppMain.MTM_MATH_ABS(angle_target);
        int num2 = AppMain.MTM_MATH_ABS(angle_current);
        if (num1 >= 208 || num2 >= 208)
            return;
        AppMain.gmGmkTarzanRopeSetUserTimerCurrentAngle(obj_work, 0);
        AppMain.gmGmkTarzanRopeSetUserWorkTargetAngle(obj_work, 0);
    }

    private static bool gmGmkTarzanRopeCheckPlayerJump(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)targetObj;
        if (AppMain.gmGmkTarzanRopeGetCatchWait(obj_work) > 0U || !AppMain.GmPlayerKeyCheckJumpKeyPush(ply_work))
            return false;
        targetObj.disp_flag &= 4294967279U;
        targetObj.spd_m = 0;
        targetObj.spd.x = 0;
        targetObj.spd.y = 0;
        targetObj.dir.z = (ushort)0;
        targetObj.spd_add.x = 0;
        targetObj.spd_add.y = 0;
        targetObj.spd_add.z = 0;
        float num1 = (float)(1.0 + (double)gmsEnemy3DWork.ene_com.eve_rec.left / 10000.0);
        int spd_x = (int)((double)angle_target * 0.800000011920929 * (double)num1);
        int num2 = (int)((double)AppMain.MTM_MATH_ABS(angle_current) * 2.29999995231628 * (double)num1);
        if (angle_target == 0 && angle_current == 0)
        {
            spd_x = 0;
            num2 = 16384;
        }
        else if (angle_target < 0 && 0 < angle_current || 0 < angle_target && angle_current < 0)
        {
            if (AppMain.MTM_MATH_ABS(angle_target + angle_current) < 1792)
            {
                spd_x = -spd_x;
            }
            else
            {
                spd_x = 0;
                num2 = 16384;
            }
        }
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, spd_x, -num2);
        AppMain.GmPlySeqChangeSequenceState(ply_work, 17);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTarzanRopeMainEnd);
        gmsEnemy3DWork.ene_com.target_obj = (AppMain.OBS_OBJECT_WORK)null;
        return true;
    }

    private static void gmGmkTarzanRopeUpdatePlayerPos(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = (AppMain.GMS_PLAYER_WORK)targetObj;
        AppMain.NNS_MATRIX nnsMatrix = new AppMain.NNS_MATRIX();
        AppMain.nnMakeUnitMatrix(nnsMatrix);
        nnsMatrix.M11 = AppMain.g_gm_gmk_tarzan_rope_active_matrix.M11;
        nnsMatrix.M22 = AppMain.g_gm_gmk_tarzan_rope_active_matrix.M00;
        nnsMatrix.M21 = AppMain.g_gm_gmk_tarzan_rope_active_matrix.M01;
        nnsMatrix.M12 = AppMain.g_gm_gmk_tarzan_rope_active_matrix.M10;
        nnsMatrix.M03 = -5f;
        AppMain.AkMathNormalizeMtx(gmsPlayerWork.ex_obj_mtx_r, nnsMatrix);
        if (((int)targetObj.disp_flag & 1) != 0)
        {
            gmsPlayerWork.ex_obj_mtx_r.M21 = -gmsPlayerWork.ex_obj_mtx_r.M21;
            gmsPlayerWork.ex_obj_mtx_r.M12 = -gmsPlayerWork.ex_obj_mtx_r.M12;
            nnsMatrix.M03 = -nnsMatrix.M03;
        }
        int num = (((int)gmsEnemy3DWork.ene_com.enemy_flag & (int)ushort.MaxValue) << 12) + 24576;
        if (num > 393216)
            num = 393216;
        gmsEnemy3DWork.ene_com.enemy_flag &= 4294901760U;
        gmsEnemy3DWork.ene_com.enemy_flag |= (uint)(num >> 12);
        AppMain.NNS_VECTOR nnsVector = new AppMain.NNS_VECTOR(0.0f, (float)(-(double)((float)num / 393216f) * 20.0) + 15f, 0.0f);
        AppMain.nnTransformVector(nnsVector, nnsMatrix, nnsVector);
        targetObj.pos.x = AppMain.FX_F32_TO_FX32(AppMain.g_gm_gmk_tarzan_rope_active_matrix.M03 + nnsVector.z);
        targetObj.pos.y = -AppMain.FX_F32_TO_FX32(AppMain.g_gm_gmk_tarzan_rope_active_matrix.M13 + nnsVector.y);
        targetObj.pos.z = AppMain.FX_F32_TO_FX32(AppMain.g_gm_gmk_tarzan_rope_active_matrix.M23 + nnsVector.x);
        gmsPlayerWork.gmk_flag |= 32768U;
    }

    private static void gmGmkTarzanRopeMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (((int)gmsPlayerWork.player_flag & 1024) != 0 || gmsPlayerWork.seq_state == 22)
        {
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTarzanRopeMainEnd);
            gmsEnemy3DWork.ene_com.target_obj = (AppMain.OBS_OBJECT_WORK)null;
            gmsPlayerWork.gmk_flag &= 4294934527U;
        }
        else
        {
            int userWorkTargetAngle = AppMain.gmGmkTarzanRopeGetUserWorkTargetAngle(obj_work);
            int timerCurrentAngle = AppMain.gmGmkTarzanRopeGetUserTimerCurrentAngle(obj_work);
            AppMain.gmGmkTarzanRopeInitCatchWait(obj_work);
            if (AppMain.gmGmkTarzanRopeCheckPlayerJump(obj_work, userWorkTargetAngle, timerCurrentAngle))
                return;
            int angle_current = AppMain.gmGmkTarzanRopeUpdateAngleCurrent(obj_work, userWorkTargetAngle, timerCurrentAngle);
            int angle_target = AppMain.gmGmkTarzanRopeUpdateAngleTarget(obj_work, userWorkTargetAngle, angle_current, true);
            AppMain.gmGmkTarzanRopeCheckStop(obj_work, angle_target, angle_current);
            AppMain.gmGmkTarzanRopeUpdatePlayerPos(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTarzanRopeMainKey);
        }
    }

    private static void gmGmkTarzanRopeMainKey(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (((int)gmsPlayerWork.player_flag & 1024) != 0 || gmsPlayerWork.seq_state == 22)
        {
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkTarzanRopeMainEnd);
            gmsEnemy3DWork.ene_com.target_obj = (AppMain.OBS_OBJECT_WORK)null;
            gmsPlayerWork.gmk_flag &= 4294934527U;
        }
        else
        {
            int angle_target1 = AppMain.gmGmkTarzanRopeGetUserWorkTargetAngle(obj_work);
            int timerCurrentAngle = AppMain.gmGmkTarzanRopeGetUserTimerCurrentAngle(obj_work);
            AppMain.gmGmkTarzanRopeUpdateCatchWait(obj_work);
            if (AppMain.gmGmkTarzanRopeCheckPlayerJump(obj_work, angle_target1, timerCurrentAngle))
                return;
            if (angle_target1 <= 0)
                angle_target1 = AppMain.gmGmkTarzanRopeApplyKeyLeft(obj_work, angle_target1, timerCurrentAngle);
            if (angle_target1 >= 0)
                angle_target1 = AppMain.gmGmkTarzanRopeApplyKeyRight(obj_work, angle_target1, timerCurrentAngle);
            int angle_current = AppMain.gmGmkTarzanRopeUpdateAngleCurrent(obj_work, angle_target1, timerCurrentAngle);
            int angle_target2 = AppMain.gmGmkTarzanRopeUpdateAngleTarget(obj_work, angle_target1, angle_current, true);
            AppMain.gmGmkTarzanRopeUpdatePlayerMotion(obj_work, angle_target2, angle_current);
            AppMain.gmGmkTarzanRopeCheckStop(obj_work, angle_target2, angle_current);
            AppMain.gmGmkTarzanRopeUpdatePlayerPos(obj_work);
        }
    }

    private static void gmGmkTarzanRopeMainEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int userWorkTargetAngle = AppMain.gmGmkTarzanRopeGetUserWorkTargetAngle(obj_work);
        int timerCurrentAngle = AppMain.gmGmkTarzanRopeGetUserTimerCurrentAngle(obj_work);
        AppMain.gmGmkTarzanRopeUpdateAngleCurrent(obj_work, userWorkTargetAngle, timerCurrentAngle);
        AppMain.gmGmkTarzanRopeUpdateAngleTarget(obj_work, userWorkTargetAngle, timerCurrentAngle, false);
        AppMain.gmGmkTarzanRopeCheckStop(obj_work, userWorkTargetAngle, timerCurrentAngle);
    }

    private static int gmGmkTarzanRopeGetGimmickRotZ(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int num = AppMain.GmPlayerKeyGetGimmickRotZ(ply_work);
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 1) == 0)
            num = num;
        return num;
    }

    private static void gmGmkTarzanRopeSetUserWorkTargetAngle(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle)
    {
        obj_work.user_work = (uint)angle;
    }

    private static int gmGmkTarzanRopeGetUserWorkTargetAngle(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return (int)obj_work.user_work;
    }

    private static int gmGmkTarzanRopeAddUserWorkTargetAngle(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle)
    {
        obj_work.user_work += (uint)angle;
        return (int)obj_work.user_work;
    }

    private static void gmGmkTarzanRopeSetUserTimerCurrentAngle(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle)
    {
        obj_work.user_timer = angle;
    }

    private static int gmGmkTarzanRopeGetUserTimerCurrentAngle(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_timer;
    }

    private static int gmGmkTarzanRopeAddUserTimerCurrentAngle(
      AppMain.OBS_OBJECT_WORK obj_work,
      int angle)
    {
        obj_work.user_timer += angle;
        return obj_work.user_timer;
    }

    private static void gmGmkTarzanRopeeSetUserFlagType(AppMain.OBS_OBJECT_WORK obj_work, int type)
    {
        obj_work.user_flag |= (uint)(ushort)type << 16;
    }

    private static int gmGmkTarzanRopeGetUserFlagType(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return (int)(obj_work.user_flag >> 16);
    }

    private static void gmGmkTarzanRopeInitCatchWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_flag |= 10U;
    }

    private static void gmGmkTarzanRopeUpdateCatchWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        uint num = obj_work.user_flag & (uint)byte.MaxValue;
        if (num > 0U)
            --num;
        obj_work.user_flag = (uint)((ulong)obj_work.user_flag & 18446744073709551360UL | (ulong)num);
    }

    private static uint gmGmkTarzanRopeGetCatchWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_flag & (uint)byte.MaxValue;
    }
}