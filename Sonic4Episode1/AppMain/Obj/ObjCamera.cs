using System;

public partial class AppMain
{
    private static int ObjCameraInit(
     int cam_id,
     NNS_VECTOR pos,
     int group,
     ushort pause_level,
     int prio)
    {
        OBS_CAMERA_SYS obsCameraSys1 = new OBS_CAMERA_SYS();
        OBS_CAMERA_SYS obsCameraSys2;
        if (obj_camera_tcb == null)
        {
            obj_camera_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(objCameraMain), new GSF_TASK_PROCEDURE(objCameraDest), 0U, pause_level, (uint)prio, group, () => new OBS_CAMERA_SYS(), "objCamera");
            obsCameraSys2 = (OBS_CAMERA_SYS)obj_camera_tcb.work;
            obsCameraSys2.Clear();
            obj_camera_sys = obsCameraSys2;
        }
        else
            obsCameraSys2 = obj_camera_sys;
        if (obsCameraSys2.camera_num >= 8)
            return -1;
        int index;
        if (cam_id < 0)
        {
            index = 0;
            while (index < 8 && obsCameraSys2.obj_camera[index] != null)
                ++index;
        }
        else
        {
            if (obsCameraSys2.obj_camera[cam_id] != null)
                return -1;
            index = cam_id;
        }
        if (index >= 8 || obsCameraSys2.obj_camera[index] != null)
            return -1;
        obsCameraSys2.obj_camera[index] = new OBS_CAMERA();
        ++obsCameraSys2.camera_num;
        OBS_CAMERA obsCamera = obsCameraSys2.obj_camera[index];
        obsCamera.camera_id = index;
        obsCamera.command_state = 0U;
        obsCamera.spd_max.x = 16f;
        obsCamera.spd_max.y = 16f;
        obsCamera.spd_max.z = 4f;
        obsCamera.spd_add.x = 3f;
        obsCamera.spd_add.y = 3f;
        obsCamera.spd_add.z = 0.5f;
        obsCamera.shift = 1;
        obsCamera.limit[0] = 8;
        obsCamera.limit[1] = 8;
        obsCamera.limit[2] = obsCamera.limit[0] + OBD_LCD_X;
        obsCamera.limit[3] = obsCamera.limit[1] + OBD_LCD_Y;
        obsCamera.limit[4] = -4096;
        obsCamera.limit[5] = 4096;
        obsCamera.pos.Assign(pos);
        obsCamera.prev_pos.Assign(pos);
        obsCamera.disp_pos.Assign(pos);
        obsCamera.prev_disp_pos.Assign(pos);
        return index;
    }

    private static void ObjCamera3dInit(int cam_id)
    {
        if (obj_camera_sys == null)
            return;
        if (obj_camera_sys.obj_camera[cam_id] == null)
        {
            NNS_VECTOR pos = GlobalPool<NNS_VECTOR>.Alloc();
            if (ObjCameraInit(cam_id, pos, 0, 0, 61438) == -1)
                return;
            GlobalPool<NNS_VECTOR>.Release(pos);
        }
        OBS_CAMERA obj_camera = obj_camera_sys.obj_camera[cam_id];
        obj_camera.flag |= 16U;
        obj_camera.znear = 1f;
        obj_camera.zfar = 60000f;
        obj_camera.aspect = AMD_SCREEN_ASPECT;
        obj_camera.fovy = NNM_DEGtoA32(45f);
        nnMakePerspectiveMatrix(obj_camera.prj_pers_mtx, obj_camera.fovy, obj_camera.aspect, obj_camera.znear, obj_camera.zfar);
        obj_camera.scale = 5f / 64f;
        float num1 = (float)(g_obj.disp_height * (double)obj_camera.scale * 0.5 * 1.0);
        float num2 = num1 * obj_camera.aspect;
        obj_camera.left = -num2;
        obj_camera.right = num2;
        obj_camera.bottom = -num1;
        obj_camera.top = num1;
        nnMakeOrthoMatrix(obj_camera.prj_ortho_mtx, obj_camera.left, obj_camera.right, obj_camera.bottom, obj_camera.top, obj_camera.znear, obj_camera.zfar);
        switch (obj_camera.camera_type)
        {
            case OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_ROLL:
                NNS_CAMERA_TARGET_ROLL cameraTargetRoll = new NNS_CAMERA_TARGET_ROLL();
                ObjCameraGetTargetRollCamera(obj_camera, cameraTargetRoll);
                nnMakeTargetRollCameraViewMatrix(obj_camera.view_mtx, cameraTargetRoll);
                break;
            case OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_UP_TARGET:
                NNS_CAMERA_TARGET_UPTARGET cameraTargetUptarget = new NNS_CAMERA_TARGET_UPTARGET();
                ObjCameraGetTargetUpTargetCamera(obj_camera, cameraTargetUptarget);
                nnMakeTargetUpTargetCameraViewMatrix(obj_camera.view_mtx, cameraTargetUptarget);
                break;
            case OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_UP_VEC:
                NNS_CAMERA_TARGET_UPVECTOR cameraTargetUpvector = new NNS_CAMERA_TARGET_UPVECTOR();
                ObjCameraGetTargetUpVecCamera(obj_camera, cameraTargetUpvector);
                nnMakeTargetUpVectorCameraViewMatrix(obj_camera.view_mtx, cameraTargetUpvector);
                break;
        }
    }

    private static void ObjCameraExit()
    {
        if (obj_camera_tcb == null)
            return;
        mtTaskClearTcb(obj_camera_tcb);
    }

    private static void ObjCameraGetTargetRollCamera(
      OBS_CAMERA obj_camera,
      NNS_CAMERA_TARGET_ROLL troll_camera)
    {
        troll_camera.User = 0U;
        troll_camera.Fovy = obj_camera.fovy;
        troll_camera.Aspect = obj_camera.aspect;
        troll_camera.ZNear = obj_camera.znear;
        troll_camera.ZFar = obj_camera.zfar;
        troll_camera.Position.x = obj_camera.disp_pos.x;
        troll_camera.Position.y = obj_camera.disp_pos.y;
        troll_camera.Position.z = obj_camera.disp_pos.z;
        if (obj_camera.target_obj != null)
        {
            troll_camera.Target.x = FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.x);
            troll_camera.Target.y = FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.y);
            troll_camera.Target.z = FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.z);
        }
        else
        {
            troll_camera.Target.x = obj_camera.target_pos.x;
            troll_camera.Target.y = obj_camera.target_pos.y;
            troll_camera.Target.z = obj_camera.target_pos.z;
        }
        troll_camera.Roll = obj_camera.roll + 16384;
    }

    private static void ObjCameraGetTargetUpTargetCamera(
      OBS_CAMERA obj_camera,
      NNS_CAMERA_TARGET_UPTARGET tupt_camera)
    {
        tupt_camera.User = 0U;
        tupt_camera.Fovy = obj_camera.fovy;
        tupt_camera.Aspect = obj_camera.aspect;
        tupt_camera.ZNear = obj_camera.znear;
        tupt_camera.ZFar = obj_camera.zfar;
        tupt_camera.Position.x = obj_camera.disp_pos.x;
        tupt_camera.Position.y = obj_camera.disp_pos.y;
        tupt_camera.Position.z = obj_camera.disp_pos.z;
        if (obj_camera.camup_obj != null)
        {
            tupt_camera.Target.x = FXM_FX32_TO_FLOAT(obj_camera.camup_obj.pos.x);
            tupt_camera.Target.y = FXM_FX32_TO_FLOAT(obj_camera.camup_obj.pos.y);
            tupt_camera.Target.z = FXM_FX32_TO_FLOAT(obj_camera.camup_obj.pos.z);
        }
        else
        {
            tupt_camera.Target.x = obj_camera.camup_pos.x;
            tupt_camera.Target.y = obj_camera.camup_pos.y;
            tupt_camera.Target.z = obj_camera.camup_pos.z;
        }
    }

    private static void ObjCameraGetTargetUpVecCamera(
      OBS_CAMERA obj_camera,
      NNS_CAMERA_TARGET_UPVECTOR tupvec_camera)
    {
        tupvec_camera.User = 0U;
        tupvec_camera.Fovy = obj_camera.fovy;
        tupvec_camera.Aspect = obj_camera.aspect;
        tupvec_camera.ZNear = obj_camera.znear;
        tupvec_camera.ZFar = obj_camera.zfar;
        tupvec_camera.Position.x = obj_camera.disp_pos.x;
        tupvec_camera.Position.y = obj_camera.disp_pos.y;
        tupvec_camera.Position.z = obj_camera.disp_pos.z;
        if (obj_camera.target_obj != null)
        {
            tupvec_camera.Target.x = FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.x);
            tupvec_camera.Target.y = FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.y);
            tupvec_camera.Target.z = FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.z);
        }
        else
        {
            tupvec_camera.Target.x = obj_camera.target_pos.x;
            tupvec_camera.Target.y = obj_camera.target_pos.y;
            tupvec_camera.Target.z = obj_camera.target_pos.z;
        }
        tupvec_camera.UpVector.Assign(obj_camera.up_vec);
    }

    private static void ObjCameraPlaySet(int cam_id, NNS_VECTOR ofst)
    {
        obj_camera_sys.obj_camera[cam_id].play_ofst_max.Assign(ofst);
    }

    private static void ObjCameraAllowSet(int cam_id, NNS_VECTOR allow)
    {
        obj_camera_sys.obj_camera[cam_id].allow.x = 0.0f;
        obj_camera_sys.obj_camera[cam_id].allow.y = 0.0f;
        obj_camera_sys.obj_camera[cam_id].allow.z = 0.0f;
        obj_camera_sys.obj_camera[cam_id].allow_limit.Assign(allow);
    }

    public static void ObjCameraDispPosGet(int cam_id, out SNNS_VECTOR disp_pos)
    {
        disp_pos = new SNNS_VECTOR(obj_camera_sys.obj_camera[cam_id].disp_pos);
    }

    private static float ObjCameraDispScaleGet(int cam_id)
    {
        mppAssertNotImpl();
        OBS_CAMERA obsCamera = obj_camera_sys.obj_camera[cam_id];
        return obsCamera.disp_pos.z >= 0.0 ? (float)(1.0 - obsCamera.disp_pos.z / 2.0) : 1f - obsCamera.disp_pos.z;
    }

    public static OBS_CAMERA ObjCameraGet(int cam_id)
    {
        return obj_camera_sys != null ? obj_camera_sys.obj_camera[cam_id] : null;
    }

    public static void ObjCameraSetUserFunc(int cam_id, OBJF_CAMERA_USER_FUNC user_func)
    {
        if (obj_camera_sys == null || obj_camera_sys.obj_camera[cam_id] == null)
            return;
        obj_camera_sys.obj_camera[cam_id].user_func = user_func;
    }

    private static void objCameraDest(MTS_TASK_TCB pTcb)
    {
        obj_camera_tcb = null;
        obj_camera_sys = null;
        OBS_CAMERA_SYS work = (OBS_CAMERA_SYS)pTcb.work;
        for (int index = 0; index < 8; ++index)
        {
            if (work.obj_camera[index] != null)
            {
                object userWork = work.obj_camera[index].user_work;
            }
        }
    }

    private static void objCameraMain(MTS_TASK_TCB tcb)
    {
        if (ObjObjectPauseCheck(0U) != 0U)
            return;
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        for (int index = 0; index < 8; ++index)
        {
            OBS_CAMERA obsCamera = obj_camera_sys.obj_camera[index];
            if (obsCamera != null)
            {
                obsCamera.prev_disp_pos.x = obsCamera.disp_pos.x;
                obsCamera.prev_disp_pos.y = obsCamera.disp_pos.y;
                obsCamera.prev_disp_pos.z = obsCamera.disp_pos.z;
                if (obsCamera.user_func != null)
                {
                    obsCamera.user_func(obsCamera);
                }
                else
                {
                    if (((int)obsCamera.flag & 4) != 0)
                        objCameraMove(obsCamera);
                    nnsVector.x = obsCamera.pos.x;
                    nnsVector.y = obsCamera.pos.y;
                    nnsVector.z = obsCamera.pos.z;
                    if (((int)obsCamera.flag & 8) != 0)
                    {
                        nnsVector.x -= (float)((nnsVector.x - (double)obsCamera.work.x) * 2.0);
                        nnsVector.y -= (float)((nnsVector.y - (double)obsCamera.work.y) * 2.0);
                        nnsVector.z -= (float)((nnsVector.z - (double)obsCamera.work.z) * 2.0);
                    }
                    obsCamera.disp_pos.x = nnsVector.x + obsCamera.ofst.x;
                    obsCamera.disp_pos.y = nnsVector.y + obsCamera.ofst.y;
                    obsCamera.disp_pos.z = nnsVector.z + obsCamera.ofst.z;
                }
                if (((int)obsCamera.flag & 32) != 0)
                    objCameraLimitCheck(obsCamera);
                obsCamera.disp_pos.x += obsCamera.disp_ofst.x;
                obsCamera.disp_pos.y += obsCamera.disp_ofst.y;
                obsCamera.disp_pos.z += obsCamera.disp_ofst.z;
                obsCamera.disp_ofst.x = 0.0f;
                obsCamera.disp_ofst.y = 0.0f;
                obsCamera.disp_ofst.z = 0.0f;
                if (((int)obsCamera.flag & 16) != 0)
                {
                    nnMakePerspectiveMatrix(obsCamera.prj_pers_mtx, obsCamera.fovy, obsCamera.aspect, obsCamera.znear, obsCamera.zfar);
                    float num1 = (float)(AMD_SCREEN_2D_WIDTH * (double)obsCamera.scale * 0.5 * 1.0);
                    float num2 = num1 * obsCamera.aspect;
                    obsCamera.left = -num2;
                    obsCamera.right = num2;
                    obsCamera.bottom = -num1;
                    obsCamera.top = num1;
                    nnMakeOrthoMatrix(obsCamera.prj_ortho_mtx, obsCamera.left, obsCamera.right, obsCamera.bottom, obsCamera.top, obsCamera.znear, obsCamera.zfar);
                    switch (obsCamera.camera_type)
                    {
                        case OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_ROLL:
                            NNS_CAMERA_TARGET_ROLL cameraTargetRoll = GlobalPool<NNS_CAMERA_TARGET_ROLL>.Alloc();
                            int roll = obsCamera.roll;
                            if (((int)obsCamera.flag & 1073741824) != 0)
                                obsCamera.roll = 0;
                            ObjCameraGetTargetRollCamera(obsCamera, cameraTargetRoll);
                            nnMakeTargetRollCameraViewMatrix(obsCamera.view_mtx, cameraTargetRoll);
                            obsCamera.roll = roll;
                            GlobalPool<NNS_CAMERA_TARGET_ROLL>.Release(cameraTargetRoll);
                            continue;
                        case OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_UP_TARGET:
                            NNS_CAMERA_TARGET_UPTARGET cameraTargetUptarget = new NNS_CAMERA_TARGET_UPTARGET();
                            ObjCameraGetTargetUpTargetCamera(obsCamera, cameraTargetUptarget);
                            nnMakeTargetUpTargetCameraViewMatrix(obsCamera.view_mtx, cameraTargetUptarget);
                            continue;
                        case OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_UP_VEC:
                            NNS_CAMERA_TARGET_UPVECTOR cameraTargetUpvector = new NNS_CAMERA_TARGET_UPVECTOR();
                            ObjCameraGetTargetUpVecCamera(obsCamera, cameraTargetUpvector);
                            nnMakeTargetUpVectorCameraViewMatrix(obsCamera.view_mtx, cameraTargetUpvector);
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    private static void objCameraMove(OBS_CAMERA obj_camera)
    {
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        if (obj_camera.target_obj != null)
        {
            nnsVector.x = FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.x) + obj_camera.target_ofst.x;
            nnsVector.y = FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.y - ((OBD_LCD_Y << 1) + 200 << 12)) + obj_camera.target_ofst.y;
            nnsVector.z = FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.z) + obj_camera.target_ofst.z;
        }
        else
        {
            nnsVector.x = obj_camera.target_pos.x - FXM_FX32_TO_FLOAT(OBD_LCD_X >> 1 << 12) + obj_camera.target_ofst.x;
            nnsVector.y = obj_camera.target_pos.y - FXM_FX32_TO_FLOAT(OBD_LCD_Y >> 1 << 12) + obj_camera.target_ofst.y;
            nnsVector.z = obj_camera.target_pos.z + obj_camera.target_ofst.z;
        }
        obj_camera.work.x = nnsVector.x;
        obj_camera.work.y = nnsVector.y;
        obj_camera.work.z = nnsVector.z;
        obj_camera.prev_pos.x = obj_camera.pos.x;
        obj_camera.prev_pos.y = obj_camera.pos.y;
        obj_camera.prev_pos.z = obj_camera.pos.z;
        if (((int)obj_camera.flag & 1) != 0)
        {
            if (obj_camera.target_obj == null)
                return;
            obj_camera.pos.x = nnsVector.x;
            obj_camera.pos.y = nnsVector.y;
            obj_camera.pos.z = nnsVector.z;
        }
        else
        {
            if (((int)obj_camera.flag & 2) != 0)
            {
                obj_camera.pos.x = ObjShiftSetF(obj_camera.pos.x, nnsVector.x, obj_camera.shift, obj_camera.spd_max.x, obj_camera.spd_add.x);
                obj_camera.pos.y = ObjShiftSetF(obj_camera.pos.y, nnsVector.x, obj_camera.shift, obj_camera.spd_max.y, obj_camera.spd_add.y);
                obj_camera.pos.z = ObjShiftSetF(obj_camera.pos.z, nnsVector.x, obj_camera.shift, obj_camera.spd_max.z, obj_camera.spd_add.z);
            }
            else
            {
                obj_camera.spd.x = nnsVector.x == (double)obj_camera.pos.x ? ObjSpdDownSetF(obj_camera.spd.x, obj_camera.spd_add.x) : ObjSpdUpSetF(obj_camera.spd.x, obj_camera.spd_add.x, obj_camera.spd_max.x);
                obj_camera.spd.y = nnsVector.y == (double)obj_camera.pos.y ? ObjSpdDownSetF(obj_camera.spd.y, obj_camera.spd_add.y) : ObjSpdUpSetF(obj_camera.spd.y, obj_camera.spd_add.y, obj_camera.spd_max.y);
                obj_camera.spd.z = nnsVector.z == (double)obj_camera.pos.z ? ObjSpdDownSetF(obj_camera.spd.z, obj_camera.spd_add.z) : ObjSpdUpSetF(obj_camera.spd.z, obj_camera.spd_add.z, obj_camera.spd_max.z);
                if (nnsVector.x > (double)obj_camera.pos.x)
                {
                    obj_camera.pos.x += obj_camera.spd.x;
                    if (obj_camera.pos.x > (double)nnsVector.x)
                        obj_camera.pos.x = nnsVector.x;
                }
                else
                {
                    obj_camera.pos.x -= obj_camera.spd.x;
                    if (obj_camera.pos.x < (double)nnsVector.x)
                        obj_camera.pos.x = nnsVector.x;
                }
                if (nnsVector.y > (double)obj_camera.pos.y)
                {
                    obj_camera.pos.y += obj_camera.spd.y;
                    if (obj_camera.pos.y > (double)nnsVector.y)
                        obj_camera.pos.y = nnsVector.y;
                }
                else
                {
                    obj_camera.pos.y -= obj_camera.spd.y;
                    if (obj_camera.pos.y < (double)nnsVector.y)
                        obj_camera.pos.y = nnsVector.y;
                }
                if (nnsVector.z > (double)obj_camera.pos.z)
                {
                    obj_camera.pos.z += obj_camera.spd.z;
                    if (obj_camera.pos.z > (double)nnsVector.z)
                        obj_camera.pos.z = nnsVector.z;
                }
                else
                {
                    obj_camera.pos.z -= obj_camera.spd.z;
                    if (obj_camera.pos.z < (double)nnsVector.z)
                        obj_camera.pos.z = nnsVector.z;
                }
            }
            if (Math.Abs(nnsVector.x - obj_camera.pos.x) > (double)obj_camera.play_ofst_max.x)
                obj_camera.pos.x = nnsVector.x <= (double)obj_camera.pos.x ? nnsVector.x + obj_camera.play_ofst_max.x : nnsVector.x - obj_camera.play_ofst_max.x;
            if (Math.Abs(nnsVector.y - obj_camera.pos.y) > (double)obj_camera.play_ofst_max.y)
                obj_camera.pos.y = nnsVector.y <= (double)obj_camera.pos.y ? nnsVector.y + obj_camera.play_ofst_max.y : nnsVector.y - obj_camera.play_ofst_max.y;
            if (Math.Abs(nnsVector.z - obj_camera.pos.z) > (double)obj_camera.play_ofst_max.z)
                obj_camera.pos.z = nnsVector.z <= (double)obj_camera.pos.z ? nnsVector.z + obj_camera.play_ofst_max.z : nnsVector.z - obj_camera.play_ofst_max.z;
            obj_camera.pos.z += obj_camera.ofst.z;
            GlobalPool<NNS_VECTOR>.Release(nnsVector);
        }
    }

    private static void objCameraLimitCheck(OBS_CAMERA obj_camera)
    {
        objCameraPosLimitCheck(obj_camera, obj_camera.disp_pos);
    }

    private static void objCameraPosLimitCheck(OBS_CAMERA obj_camera, NNS_VECTOR pPos)
    {
        int num1 = 0;
        if (pPos.z != 0.0)
        {
            if (obj_camera.limit[4] > (double)pPos.z)
                pPos.z = obj_camera.limit[4];
            if (obj_camera.limit[5] < (double)pPos.z)
                pPos.z = obj_camera.limit[5];
            if (1.0 / ObjCameraDispScaleGet(obj_camera.index) * OBD_LCD_X > obj_camera.limit[2] - obj_camera.limit[0])
            {
                float num2 = 1f / ((obj_camera.limit[2] - obj_camera.limit[0]) / (float)OBD_LCD_X);
                pPos.z = pPos.z < 0.0 ? (float)((num2 - 1.0) * -1.0) : (float)((num2 - 1.0) * -2.0);
            }
            if (1.0 / ObjCameraDispScaleGet(obj_camera.index) * OBD_LCD_Y > obj_camera.limit[3] - obj_camera.limit[1])
            {
                float num2 = 1f / ((obj_camera.limit[3] - obj_camera.limit[1]) / (float)OBD_LCD_X);
                pPos.z = pPos.z < 0.0 ? (float)((num2 - 1.0) * -1.0) : (float)((num2 - 1.0) * -2.0);
            }
        }
        if (pPos.z > 0.0)
            num1 = FXM_FLOAT_TO_FX32(1f / ObjCameraDispScaleGet(obj_camera.index) * OBD_LCD_X) - (OBD_LCD_X << 12) >> 13;
        if (obj_camera.limit[0] + num1 > (double)pPos.x)
            pPos.x = obj_camera.limit[0] + num1;
        if (obj_camera.limit[2] - OBD_LCD_X - num1 < (double)pPos.x)
            pPos.x = obj_camera.limit[2] - OBD_LCD_X - num1;
        if (pPos.z > 0.0)
            num1 = FXM_FLOAT_TO_FX32(1f / ObjCameraDispScaleGet(obj_camera.index) * OBD_LCD_Y) - (OBD_LCD_Y << 12) >> 13;
        if (obj_camera.limit[1] + num1 > (double)pPos.y)
            pPos.y = obj_camera.limit[1] + num1;
        if (obj_camera.limit[3] - OBD_LCD_Y - num1 >= (double)pPos.y)
            return;
        pPos.y = obj_camera.limit[3] - OBD_LCD_Y - num1;
    }
}