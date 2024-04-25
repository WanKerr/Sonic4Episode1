public partial class AppMain
{
    private static void GmGmkTarzanRopeBuild()
    {
        g_gm_gmk_tarzan_rope_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(829)), readAMBFile(GmGameDatGetGimmickData(830)), 0U);
    }

    private static void GmGmkTarzanRopeFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(829));
        GmGameDBuildRegFlushModel(g_gm_gmk_tarzan_rope_obj_3d_list, amsAmbHeader.file_num);
        g_gm_gmk_tarzan_rope_obj_3d_list = null;
    }

    private static OBS_OBJECT_WORK GmGmkTarzanRopeInit(
      GMS_EVE_RECORD_EVENT eve_rec,
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
                return null;
        }
        float length = (float)(1.0 + eve_rec.left / 100.0);
        OBS_OBJECT_WORK objWork = gmGmkTarzanRopeLoadObj(eve_rec, pos_x, pos_y, type1).ene_com.obj_work;
        gmGmkTarzanRopeInit(objWork, type1, length);
        return objWork;
    }

    private static void gmGmkTarzanRopeMotionCallback(
      AMS_MOTION motion,
      NNS_OBJECT obj,
      object val)
    {
        if (((GMS_ENEMY_3D_WORK)(OBS_OBJECT_WORK)val).ene_com.target_obj == null)
            return;
        NNS_MATRIX nnsMatrix = new NNS_MATRIX();
        nnMakeUnitMatrix(nnsMatrix);
        nnMultiplyMatrix(nnsMatrix, nnsMatrix, amMatrixGetCurrent());
        nnCalcNodeMatrixTRSList(g_gm_gmk_tarzan_rope_active_matrix, obj, 13, motion.data, nnsMatrix);
    }

    private static GMS_ENEMY_3D_WORK gmGmkTarzanRopeLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_T_ROPE");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkTarzanRopeLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkTarzanRopeLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index1 = eve_rec.left < 50 ? (eve_rec.left < 20 ? 0 : 1) : 2;
        int index2 = g_gm_gmk_tarzan_rope_model_id[index1];
        ObjObjectCopyAction3dNNModel(objWork, g_gm_gmk_tarzan_rope_obj_3d_list[index2], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(objWork, 0, false, ObjDataGet(831), null, 0, null);
        objWork.obj_3d.mtn_cb_func = new mtn_cb_func_delegate(gmGmkTarzanRopeMotionCallback);
        objWork.obj_3d.mtn_cb_param = objWork;
        int fx32 = FX_F32_TO_FX32(objWork.obj_3d._object.pNodeList[0].Translation.y * (eve_rec.left / 30f));
        objWork.pos.y -= fx32;
        return gmsEnemy3DWork;
    }

    private static void gmGmkTarzanRopeInit(OBS_OBJECT_WORK obj_work, int type, float length)
    {
        gmGmkTarzanRopeSetRect((GMS_ENEMY_3D_WORK)obj_work, type);
        obj_work.move_flag = 8448U;
        obj_work.disp_flag |= 4194304U;
        obj_work.obj_3d.drawflag |= 32U;
        int id = g_gm_gmk_tarzan_rope_motion_id[type];
        ObjDrawObjectActionSet3DNN(obj_work, id, 0);
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
        gmGmkTarzanRopeSetUserWorkTargetAngle(obj_work, angle);
        gmGmkTarzanRopeSetUserTimerCurrentAngle(obj_work, angle);
        gmGmkTarzanRopeeSetUserFlagType(obj_work, type);
        obj_work.ppFunc = null;
        obj_work.ppMove = null;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkTarzanRopeDrawFunc);
    }

    private static void gmGmkTarzanRopeSetRect(GMS_ENEMY_3D_WORK gimmick_work, int type)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        short cRight = (short)(64.0 * (1.0 + gimmick_work.ene_com.eve_rec.left / 100.0));
        switch (type)
        {
            case 0:
                ObjRectWorkZSet(pRec, -32, -32, -500, 32, (short)(cRight - 32), 500);
                break;
            case 1:
                ObjRectWorkZSet(pRec, (short)-cRight, -48, -500, 0, -16, 500);
                break;
            case 2:
                ObjRectWorkZSet(pRec, 0, -48, -500, cRight, -16, 500);
                break;
        }
        pRec.flag |= 1024U;
        ObjRectDefSet(pRec, 65534, 0);
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkTarzanRopeDefFunc);
    }

    private static float gmGmkTarzanRopeCalcFlame(OBS_OBJECT_WORK obj_work, int angle)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        int num1 = 16384;
        int num2 = -16384;
        if (angle > num1)
            angle = num1;
        else if (angle < num2)
            angle = num2;
        float startFrame = amMotionGetStartFrame(obj3d.motion, obj3d.act_id[0]);
        float endFrame = amMotionGetEndFrame(obj3d.motion, obj3d.act_id[0]);
        return (float)((endFrame - (double)startFrame) / 2.0) - (float)(angle / (float)num1 * (endFrame - (double)startFrame) / 4.0);
    }

    private static void gmGmkTarzanRopeDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (obj3d.motion == null)
            return;
        obj3d.frame[0] = gmGmkTarzanRopeCalcFlame(obj_work, gmGmkTarzanRopeGetUserTimerCurrentAngle(obj_work));
        ObjDrawActionSummary(obj_work);
    }

    private static void gmGmkTarzanRopeDefFunc(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj1 = own_rect.parent_obj;
        GMS_ENEMY_3D_WORK gimmick_work = (GMS_ENEMY_3D_WORK)parentObj1;
        OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != 1)
            return;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)parentObj2;
        if (ply_work.seq_state == GME_PLY_SEQ_STATE_GMK_TARZAN_ROPE)
            return;
        if (ply_work.seq_state == GME_PLY_SEQ_STATE_HOMING)
            GMM_PAD_VIB_SMALL();
        GmPlySeqInitTarzanRope(ply_work, gimmick_work.ene_com);
        parentObj1.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkTarzanRopeMainWait);
        gimmick_work.ene_com.target_obj = parentObj2;
        int userFlagType = gmGmkTarzanRopeGetUserFlagType(parentObj1);
        int angle;
        if (userFlagType == 0)
        {
            angle = parentObj2.spd.x >> 2;
            if (angle > 240)
                angle += MTM_MATH_ABS(parentObj2.spd.y >> 2);
            else if (angle < -240)
                angle -= MTM_MATH_ABS(parentObj2.spd.y >> 2);
            if (angle > 16384)
                angle = 16384;
            else if (angle < -16384)
                angle = -16384;
        }
        else
            angle = gmGmkTarzanRopeGetUserTimerCurrentAngle(parentObj1);
        switch (userFlagType)
        {
            case 0:
                ushort num1 = (ushort)((parentObj1.pos.y >> 12) - 32);
                ushort num2 = (ushort)(parentObj2.pos.y >> 12);
                gimmick_work.ene_com.enemy_flag &= 4294901760U;
                if (num2 > num1)
                {
                    gimmick_work.ene_com.enemy_flag |= (ushort)(num2 - (uint)num1);
                    break;
                }
                break;
            case 1:
                ushort num3 = (ushort)(parentObj1.pos.x >> 12);
                ushort num4 = (ushort)(parentObj2.pos.x >> 12);
                gimmick_work.ene_com.enemy_flag &= 4294901760U;
                if (num4 < num3)
                {
                    gimmick_work.ene_com.enemy_flag |= (ushort)(num3 - (uint)num4);
                    break;
                }
                break;
            case 2:
                ushort num5 = (ushort)(parentObj1.pos.x >> 12);
                ushort num6 = (ushort)(parentObj2.pos.x >> 12);
                gimmick_work.ene_com.enemy_flag &= 4294901760U;
                if (num6 > num5)
                {
                    gimmick_work.ene_com.enemy_flag |= (ushort)(num6 - (uint)num5);
                    break;
                }
                break;
        }
        gmGmkTarzanRopeSetUserWorkTargetAngle(parentObj1, angle);
        int type = 0;
        gmGmkTarzanRopeeSetUserFlagType(parentObj1, type);
        gmGmkTarzanRopeSetRect(gimmick_work, type);
        parentObj2.spd_m = 0;
        parentObj2.spd.x = 0;
        parentObj2.spd.y = 0;
        parentObj2.dir.z = 0;
    }

    private static int gmGmkTarzanRopeUpdateAngleCurrent(
      OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        int num1 = MTM_MATH_ABS(angle_target);
        int num2 = MTM_MATH_ABS(angle_current);
        int num3 = 1920;
        float a = (num1 - num2) / 20480f;
        if (a > 0.550000011920929)
            a = 0.55f;
        else if (a < -0.550000011920929)
            a = -0.55f;
        int angle = (int)(num3 * (MTM_MATH_ABS(a) + 0.0500000007450581));
        if (angle_target < angle_current)
            angle = -angle;
        return gmGmkTarzanRopeAddUserTimerCurrentAngle(obj_work, angle);
    }

    private static int gmGmkTarzanRopeUpdateAngleTarget(
      OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current,
      bool flag_motion_change)
    {
        if (angle_target > 0)
        {
            angle_target = gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, -48);
            if (angle_target > 16384)
                angle_target = 16384;
            else if (angle_target < 0)
                angle_target = 0;
            if (angle_current >= angle_target)
            {
                angle_target = -angle_target;
                gmGmkTarzanRopeSetUserWorkTargetAngle(obj_work, angle_target);
                if (flag_motion_change)
                    gmGmkTarzanRopeChangeDirMotion(obj_work, angle_current);
            }
        }
        else if (angle_target < 0)
        {
            angle_target = gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, 48);
            if (angle_target < -16384)
                angle_target = -16384;
            else if (angle_target > 0)
                angle_target = 0;
            if (angle_current <= angle_target)
            {
                angle_target = -angle_target;
                gmGmkTarzanRopeSetUserWorkTargetAngle(obj_work, angle_target);
                if (flag_motion_change)
                    gmGmkTarzanRopeChangeDirMotion(obj_work, angle_current);
            }
        }
        return angle_target;
    }

    private static void gmGmkTarzanRopeChangeDirMotion(
      OBS_OBJECT_WORK obj_work,
      int angle_current)
    {
        OBS_OBJECT_WORK objWork = g_gm_main_system.ply_work[0].obj_work;
        OBS_ACTION3D_NN_WORK obj3d = objWork.obj_3d;
        float startFrame = amMotionGetStartFrame(obj3d.motion, obj3d.act_id[0]);
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
      OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        UNREFERENCED_PARAMETER(obj_work);
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        OBS_ACTION3D_NN_WORK obj3d = objWork.obj_3d;
        float num1 = 34f;
        int num2 = MTM_MATH_ABS(angle_target);
        int num3 = MTM_MATH_ABS(angle_current);
        if (num2 < 1792 && num3 < 1792)
        {
            int gimmickRotZ = gmGmkTarzanRopeGetGimmickRotZ(ply_work);
            if (ply_work.act_state == 64 || MTM_MATH_ABS(gimmickRotZ) >= 208)
                return;
            GmPlayerActionChange(ply_work, 64);
            objWork.disp_flag |= 4U;
        }
        else if (ply_work.act_state == 64)
        {
            GmPlayerActionChange(ply_work, 63);
            objWork.disp_flag |= 4U;
        }
        else if (((int)objWork.disp_flag & 1) != 0)
        {
            if (angle_target > 0 && angle_current > 0)
            {
                if (obj3d.frame[0] <= (double)num1)
                    return;
                objWork.disp_flag |= 16U;
            }
            else
            {
                if (angle_target >= 0 || angle_current >= 0 || obj3d.frame[0] >= (double)num1)
                    return;
                objWork.disp_flag |= 16U;
            }
        }
        else if (angle_target > 0 && angle_current > 0)
        {
            if (obj3d.frame[0] >= (double)num1)
                return;
            objWork.disp_flag |= 16U;
        }
        else
        {
            if (angle_target >= 0 || angle_current >= 0 || obj3d.frame[0] <= (double)num1)
                return;
            objWork.disp_flag |= 16U;
        }
    }

    private static int gmGmkTarzanRopeApplyKeyLeft(
      OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        GMS_PLAYER_WORK targetObj = (GMS_PLAYER_WORK)((GMS_ENEMY_3D_WORK)obj_work).ene_com.target_obj;
        int num1 = MTM_MATH_ABS(angle_target);
        int num2 = MTM_MATH_ABS(angle_current);
        int gimmickRotZ = gmGmkTarzanRopeGetGimmickRotZ(targetObj);
        if (num1 < 1792 && num2 < 1792)
        {
            if (gimmickRotZ < 0)
                angle_target = gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, -1792);
        }
        else if (angle_target < 0)
        {
            if (gimmickRotZ < 0)
                angle_target = gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, -132);
            else if (gimmickRotZ > 0)
                angle_target = gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, 31);
        }
        return angle_target;
    }

    private static int gmGmkTarzanRopeApplyKeyRight(
      OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        GMS_PLAYER_WORK targetObj = (GMS_PLAYER_WORK)((GMS_ENEMY_3D_WORK)obj_work).ene_com.target_obj;
        int num1 = MTM_MATH_ABS(angle_target);
        int num2 = MTM_MATH_ABS(angle_current);
        int gimmickRotZ = gmGmkTarzanRopeGetGimmickRotZ(targetObj);
        if (num1 < 1792 && num2 < 1792)
        {
            if (gimmickRotZ > 0)
                angle_target = gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, 1792);
        }
        else if (angle_target > 0)
        {
            if (gimmickRotZ > 0)
                angle_target = gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, 132);
            else if (gimmickRotZ < 0)
                angle_target = gmGmkTarzanRopeAddUserWorkTargetAngle(obj_work, -31);
        }
        return angle_target;
    }

    private static void gmGmkTarzanRopeCheckStop(
      OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        int num1 = MTM_MATH_ABS(angle_target);
        int num2 = MTM_MATH_ABS(angle_current);
        if (num1 >= 208 || num2 >= 208)
            return;
        gmGmkTarzanRopeSetUserTimerCurrentAngle(obj_work, 0);
        gmGmkTarzanRopeSetUserWorkTargetAngle(obj_work, 0);
    }

    private static bool gmGmkTarzanRopeCheckPlayerJump(
      OBS_OBJECT_WORK obj_work,
      int angle_target,
      int angle_current)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)targetObj;
        if (gmGmkTarzanRopeGetCatchWait(obj_work) > 0U || !GmPlayerKeyCheckJumpKeyPush(ply_work))
            return false;
        targetObj.disp_flag &= 4294967279U;
        targetObj.spd_m = 0;
        targetObj.spd.x = 0;
        targetObj.spd.y = 0;
        targetObj.dir.z = 0;
        targetObj.spd_add.x = 0;
        targetObj.spd_add.y = 0;
        targetObj.spd_add.z = 0;
        float num1 = (float)(1.0 + gmsEnemy3DWork.ene_com.eve_rec.left / 10000.0);
        int spd_x = (int)(angle_target * 0.800000011920929 * num1);
        int num2 = (int)(MTM_MATH_ABS(angle_current) * 2.29999995231628 * num1);
        if (angle_target == 0 && angle_current == 0)
        {
            spd_x = 0;
            num2 = 16384;
        }
        else if (angle_target < 0 && 0 < angle_current || 0 < angle_target && angle_current < 0)
        {
            if (MTM_MATH_ABS(angle_target + angle_current) < 1792)
            {
                spd_x = -spd_x;
            }
            else
            {
                spd_x = 0;
                num2 = 16384;
            }
        }
        GmPlySeqGmkInitGmkJump(ply_work, spd_x, -num2);
        GmPlySeqChangeSequenceState(ply_work, 17);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkTarzanRopeMainEnd);
        gmsEnemy3DWork.ene_com.target_obj = null;
        return true;
    }

    private static void gmGmkTarzanRopeUpdatePlayerPos(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        GMS_PLAYER_WORK gmsPlayerWork = (GMS_PLAYER_WORK)targetObj;
        NNS_MATRIX nnsMatrix = new NNS_MATRIX();
        nnMakeUnitMatrix(nnsMatrix);
        nnsMatrix.M11 = g_gm_gmk_tarzan_rope_active_matrix.M11;
        nnsMatrix.M22 = g_gm_gmk_tarzan_rope_active_matrix.M00;
        nnsMatrix.M21 = g_gm_gmk_tarzan_rope_active_matrix.M01;
        nnsMatrix.M12 = g_gm_gmk_tarzan_rope_active_matrix.M10;
        nnsMatrix.M03 = -5f;
        AkMathNormalizeMtx(gmsPlayerWork.ex_obj_mtx_r, nnsMatrix);
        if (((int)targetObj.disp_flag & 1) != 0)
        {
            gmsPlayerWork.ex_obj_mtx_r.M21 = -gmsPlayerWork.ex_obj_mtx_r.M21;
            gmsPlayerWork.ex_obj_mtx_r.M12 = -gmsPlayerWork.ex_obj_mtx_r.M12;
            nnsMatrix.M03 = -nnsMatrix.M03;
        }
        int num = (((int)gmsEnemy3DWork.ene_com.enemy_flag & ushort.MaxValue) << 12) + 24576;
        if (num > 393216)
            num = 393216;
        gmsEnemy3DWork.ene_com.enemy_flag &= 4294901760U;
        gmsEnemy3DWork.ene_com.enemy_flag |= (uint)(num >> 12);
        NNS_VECTOR nnsVector = new NNS_VECTOR(0.0f, (float)(-(num / 393216f) * 20.0) + 15f, 0.0f);
        nnTransformVector(nnsVector, nnsMatrix, nnsVector);
        targetObj.pos.x = FX_F32_TO_FX32(g_gm_gmk_tarzan_rope_active_matrix.M03 + nnsVector.z);
        targetObj.pos.y = -FX_F32_TO_FX32(g_gm_gmk_tarzan_rope_active_matrix.M13 + nnsVector.y);
        targetObj.pos.z = FX_F32_TO_FX32(g_gm_gmk_tarzan_rope_active_matrix.M23 + nnsVector.x);
        gmsPlayerWork.gmk_flag |= 32768U;
    }

    private static void gmGmkTarzanRopeMainWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (((int)gmsPlayerWork.player_flag & 1024) != 0 || gmsPlayerWork.seq_state == 22)
        {
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkTarzanRopeMainEnd);
            gmsEnemy3DWork.ene_com.target_obj = null;
            gmsPlayerWork.gmk_flag &= 4294934527U;
        }
        else
        {
            int userWorkTargetAngle = gmGmkTarzanRopeGetUserWorkTargetAngle(obj_work);
            int timerCurrentAngle = gmGmkTarzanRopeGetUserTimerCurrentAngle(obj_work);
            gmGmkTarzanRopeInitCatchWait(obj_work);
            if (gmGmkTarzanRopeCheckPlayerJump(obj_work, userWorkTargetAngle, timerCurrentAngle))
                return;
            int angle_current = gmGmkTarzanRopeUpdateAngleCurrent(obj_work, userWorkTargetAngle, timerCurrentAngle);
            int angle_target = gmGmkTarzanRopeUpdateAngleTarget(obj_work, userWorkTargetAngle, angle_current, true);
            gmGmkTarzanRopeCheckStop(obj_work, angle_target, angle_current);
            gmGmkTarzanRopeUpdatePlayerPos(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkTarzanRopeMainKey);
        }
    }

    private static void gmGmkTarzanRopeMainKey(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (((int)gmsPlayerWork.player_flag & 1024) != 0 || gmsPlayerWork.seq_state == 22)
        {
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkTarzanRopeMainEnd);
            gmsEnemy3DWork.ene_com.target_obj = null;
            gmsPlayerWork.gmk_flag &= 4294934527U;
        }
        else
        {
            int angle_target1 = gmGmkTarzanRopeGetUserWorkTargetAngle(obj_work);
            int timerCurrentAngle = gmGmkTarzanRopeGetUserTimerCurrentAngle(obj_work);
            gmGmkTarzanRopeUpdateCatchWait(obj_work);
            if (gmGmkTarzanRopeCheckPlayerJump(obj_work, angle_target1, timerCurrentAngle))
                return;
            if (angle_target1 <= 0)
                angle_target1 = gmGmkTarzanRopeApplyKeyLeft(obj_work, angle_target1, timerCurrentAngle);
            if (angle_target1 >= 0)
                angle_target1 = gmGmkTarzanRopeApplyKeyRight(obj_work, angle_target1, timerCurrentAngle);
            int angle_current = gmGmkTarzanRopeUpdateAngleCurrent(obj_work, angle_target1, timerCurrentAngle);
            int angle_target2 = gmGmkTarzanRopeUpdateAngleTarget(obj_work, angle_target1, angle_current, true);
            gmGmkTarzanRopeUpdatePlayerMotion(obj_work, angle_target2, angle_current);
            gmGmkTarzanRopeCheckStop(obj_work, angle_target2, angle_current);
            gmGmkTarzanRopeUpdatePlayerPos(obj_work);
        }
    }

    private static void gmGmkTarzanRopeMainEnd(OBS_OBJECT_WORK obj_work)
    {
        int userWorkTargetAngle = gmGmkTarzanRopeGetUserWorkTargetAngle(obj_work);
        int timerCurrentAngle = gmGmkTarzanRopeGetUserTimerCurrentAngle(obj_work);
        gmGmkTarzanRopeUpdateAngleCurrent(obj_work, userWorkTargetAngle, timerCurrentAngle);
        gmGmkTarzanRopeUpdateAngleTarget(obj_work, userWorkTargetAngle, timerCurrentAngle, false);
        gmGmkTarzanRopeCheckStop(obj_work, userWorkTargetAngle, timerCurrentAngle);
    }

    private static int gmGmkTarzanRopeGetGimmickRotZ(GMS_PLAYER_WORK ply_work)
    {
        int num = GmPlayerKeyGetGimmickRotZ(ply_work);
        if (((int)g_gs_main_sys_info.game_flag & 1) == 0)
            num = num;
        return num;
    }

    private static void gmGmkTarzanRopeSetUserWorkTargetAngle(
      OBS_OBJECT_WORK obj_work,
      int angle)
    {
        obj_work.user_work = (uint)angle;
    }

    private static int gmGmkTarzanRopeGetUserWorkTargetAngle(OBS_OBJECT_WORK obj_work)
    {
        return (int)obj_work.user_work;
    }

    private static int gmGmkTarzanRopeAddUserWorkTargetAngle(
      OBS_OBJECT_WORK obj_work,
      int angle)
    {
        obj_work.user_work += (uint)angle;
        return (int)obj_work.user_work;
    }

    private static void gmGmkTarzanRopeSetUserTimerCurrentAngle(
      OBS_OBJECT_WORK obj_work,
      int angle)
    {
        obj_work.user_timer = angle;
    }

    private static int gmGmkTarzanRopeGetUserTimerCurrentAngle(OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_timer;
    }

    private static int gmGmkTarzanRopeAddUserTimerCurrentAngle(
      OBS_OBJECT_WORK obj_work,
      int angle)
    {
        obj_work.user_timer += angle;
        return obj_work.user_timer;
    }

    private static void gmGmkTarzanRopeeSetUserFlagType(OBS_OBJECT_WORK obj_work, int type)
    {
        obj_work.user_flag |= (uint)(ushort)type << 16;
    }

    private static int gmGmkTarzanRopeGetUserFlagType(OBS_OBJECT_WORK obj_work)
    {
        return (int)(obj_work.user_flag >> 16);
    }

    private static void gmGmkTarzanRopeInitCatchWait(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_flag |= 10U;
    }

    private static void gmGmkTarzanRopeUpdateCatchWait(OBS_OBJECT_WORK obj_work)
    {
        uint num = obj_work.user_flag & byte.MaxValue;
        if (num > 0U)
            --num;
        obj_work.user_flag = (uint)(obj_work.user_flag & 18446744073709551360UL | num);
    }

    private static uint gmGmkTarzanRopeGetCatchWait(OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_flag & byte.MaxValue;
    }
}