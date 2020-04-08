using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

public partial class AppMain
{
    private static int ObjCameraInit(
     int cam_id,
     AppMain.NNS_VECTOR pos,
     int group,
     ushort pause_level,
     int prio)
    {
        AppMain.OBS_CAMERA_SYS obsCameraSys1 = new AppMain.OBS_CAMERA_SYS();
        AppMain.OBS_CAMERA_SYS obsCameraSys2;
        if (AppMain.obj_camera_tcb == null)
        {
            AppMain.obj_camera_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.objCameraMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.objCameraDest), 0U, pause_level, (uint)prio, group, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.OBS_CAMERA_SYS()), "objCamera");
            obsCameraSys2 = (AppMain.OBS_CAMERA_SYS)AppMain.obj_camera_tcb.work;
            obsCameraSys2.Clear();
            AppMain.obj_camera_sys = obsCameraSys2;
        }
        else
            obsCameraSys2 = AppMain.obj_camera_sys;
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
        obsCameraSys2.obj_camera[index] = new AppMain.OBS_CAMERA();
        ++obsCameraSys2.camera_num;
        AppMain.OBS_CAMERA obsCamera = obsCameraSys2.obj_camera[index];
        obsCamera.camera_id = index;
        obsCamera.command_state = 0U;
        obsCamera.spd_max.x = 16f;
        obsCamera.spd_max.y = 16f;
        obsCamera.spd_max.z = 4f;
        obsCamera.spd_add.x = 3f;
        obsCamera.spd_add.y = 3f;
        obsCamera.spd_add.z = 0.5f;
        obsCamera.shift = (ushort)1;
        obsCamera.limit[0] = 8;
        obsCamera.limit[1] = 8;
        obsCamera.limit[2] = obsCamera.limit[0] + (int)AppMain.OBD_LCD_X;
        obsCamera.limit[3] = obsCamera.limit[1] + (int)AppMain.OBD_LCD_Y;
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
        if (AppMain.obj_camera_sys == null)
            return;
        if (AppMain.obj_camera_sys.obj_camera[cam_id] == null)
        {
            AppMain.NNS_VECTOR pos = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            if (AppMain.ObjCameraInit(cam_id, pos, 0, (ushort)0, 61438) == -1)
                return;
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(pos);
        }
        AppMain.OBS_CAMERA obj_camera = AppMain.obj_camera_sys.obj_camera[cam_id];
        obj_camera.flag |= 16U;
        obj_camera.znear = 1f;
        obj_camera.zfar = 60000f;
        obj_camera.aspect = AppMain.AMD_SCREEN_ASPECT;
        obj_camera.fovy = AppMain.NNM_DEGtoA32(45f);
        AppMain.nnMakePerspectiveMatrix(obj_camera.prj_pers_mtx, obj_camera.fovy, obj_camera.aspect, obj_camera.znear, obj_camera.zfar);
        obj_camera.scale = 5f / 64f;
        float num1 = (float)((double)AppMain.g_obj.disp_height * (double)obj_camera.scale * 0.5 * 1.0);
        float num2 = num1 * obj_camera.aspect;
        obj_camera.left = -num2;
        obj_camera.right = num2;
        obj_camera.bottom = -num1;
        obj_camera.top = num1;
        AppMain.nnMakeOrthoMatrix(obj_camera.prj_ortho_mtx, obj_camera.left, obj_camera.right, obj_camera.bottom, obj_camera.top, obj_camera.znear, obj_camera.zfar);
        switch (obj_camera.camera_type)
        {
            case AppMain.OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_ROLL:
                AppMain.NNS_CAMERA_TARGET_ROLL cameraTargetRoll = new AppMain.NNS_CAMERA_TARGET_ROLL();
                AppMain.ObjCameraGetTargetRollCamera(obj_camera, cameraTargetRoll);
                AppMain.nnMakeTargetRollCameraViewMatrix(obj_camera.view_mtx, cameraTargetRoll);
                break;
            case AppMain.OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_UP_TARGET:
                AppMain.NNS_CAMERA_TARGET_UPTARGET cameraTargetUptarget = new AppMain.NNS_CAMERA_TARGET_UPTARGET();
                AppMain.ObjCameraGetTargetUpTargetCamera(obj_camera, cameraTargetUptarget);
                AppMain.nnMakeTargetUpTargetCameraViewMatrix(obj_camera.view_mtx, cameraTargetUptarget);
                break;
            case AppMain.OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_UP_VEC:
                AppMain.NNS_CAMERA_TARGET_UPVECTOR cameraTargetUpvector = new AppMain.NNS_CAMERA_TARGET_UPVECTOR();
                AppMain.ObjCameraGetTargetUpVecCamera(obj_camera, cameraTargetUpvector);
                AppMain.nnMakeTargetUpVectorCameraViewMatrix(obj_camera.view_mtx, cameraTargetUpvector);
                break;
        }
    }

    private static void ObjCameraExit()
    {
        if (AppMain.obj_camera_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.obj_camera_tcb);
    }

    private static void ObjCameraGetTargetRollCamera(
      AppMain.OBS_CAMERA obj_camera,
      AppMain.NNS_CAMERA_TARGET_ROLL troll_camera)
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
            troll_camera.Target.x = AppMain.FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.x);
            troll_camera.Target.y = AppMain.FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.y);
            troll_camera.Target.z = AppMain.FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.z);
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
      AppMain.OBS_CAMERA obj_camera,
      AppMain.NNS_CAMERA_TARGET_UPTARGET tupt_camera)
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
            tupt_camera.Target.x = AppMain.FXM_FX32_TO_FLOAT(obj_camera.camup_obj.pos.x);
            tupt_camera.Target.y = AppMain.FXM_FX32_TO_FLOAT(obj_camera.camup_obj.pos.y);
            tupt_camera.Target.z = AppMain.FXM_FX32_TO_FLOAT(obj_camera.camup_obj.pos.z);
        }
        else
        {
            tupt_camera.Target.x = obj_camera.camup_pos.x;
            tupt_camera.Target.y = obj_camera.camup_pos.y;
            tupt_camera.Target.z = obj_camera.camup_pos.z;
        }
    }

    private static void ObjCameraGetTargetUpVecCamera(
      AppMain.OBS_CAMERA obj_camera,
      AppMain.NNS_CAMERA_TARGET_UPVECTOR tupvec_camera)
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
            tupvec_camera.Target.x = AppMain.FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.x);
            tupvec_camera.Target.y = AppMain.FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.y);
            tupvec_camera.Target.z = AppMain.FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.z);
        }
        else
        {
            tupvec_camera.Target.x = obj_camera.target_pos.x;
            tupvec_camera.Target.y = obj_camera.target_pos.y;
            tupvec_camera.Target.z = obj_camera.target_pos.z;
        }
        tupvec_camera.UpVector.Assign(obj_camera.up_vec);
    }

    private static void ObjCameraPlaySet(int cam_id, AppMain.NNS_VECTOR ofst)
    {
        AppMain.obj_camera_sys.obj_camera[cam_id].play_ofst_max.Assign(ofst);
    }

    private static void ObjCameraAllowSet(int cam_id, AppMain.NNS_VECTOR allow)
    {
        AppMain.obj_camera_sys.obj_camera[cam_id].allow.x = 0.0f;
        AppMain.obj_camera_sys.obj_camera[cam_id].allow.y = 0.0f;
        AppMain.obj_camera_sys.obj_camera[cam_id].allow.z = 0.0f;
        AppMain.obj_camera_sys.obj_camera[cam_id].allow_limit.Assign(allow);
    }

    public static void ObjCameraDispPosGet(int cam_id, out AppMain.SNNS_VECTOR disp_pos)
    {
        disp_pos = new AppMain.SNNS_VECTOR(AppMain.obj_camera_sys.obj_camera[cam_id].disp_pos);
    }

    private static float ObjCameraDispScaleGet(int cam_id)
    {
        AppMain.mppAssertNotImpl();
        AppMain.OBS_CAMERA obsCamera = AppMain.obj_camera_sys.obj_camera[cam_id];
        return (double)obsCamera.disp_pos.z >= 0.0 ? (float)(1.0 - (double)obsCamera.disp_pos.z / 2.0) : 1f - obsCamera.disp_pos.z;
    }

    public static AppMain.OBS_CAMERA ObjCameraGet(int cam_id)
    {
        return AppMain.obj_camera_sys != null ? AppMain.obj_camera_sys.obj_camera[cam_id] : (AppMain.OBS_CAMERA)null;
    }

    public static void ObjCameraSetUserFunc(int cam_id, AppMain.OBJF_CAMERA_USER_FUNC user_func)
    {
        if (AppMain.obj_camera_sys == null || AppMain.obj_camera_sys.obj_camera[cam_id] == null)
            return;
        AppMain.obj_camera_sys.obj_camera[cam_id].user_func = user_func;
    }

    private static void objCameraDest(AppMain.MTS_TASK_TCB pTcb)
    {
        AppMain.obj_camera_tcb = (AppMain.MTS_TASK_TCB)null;
        AppMain.obj_camera_sys = (AppMain.OBS_CAMERA_SYS)null;
        AppMain.OBS_CAMERA_SYS work = (AppMain.OBS_CAMERA_SYS)pTcb.work;
        for (int index = 0; index < 8; ++index)
        {
            if (work.obj_camera[index] != null)
            {
                object userWork = work.obj_camera[index].user_work;
            }
        }
    }

    private static void objCameraMain(AppMain.MTS_TASK_TCB tcb)
    {
        if (AppMain.ObjObjectPauseCheck(0U) != 0U)
            return;
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        for (int index = 0; index < 8; ++index)
        {
            AppMain.OBS_CAMERA obsCamera = AppMain.obj_camera_sys.obj_camera[index];
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
                        AppMain.objCameraMove(obsCamera);
                    nnsVector.x = obsCamera.pos.x;
                    nnsVector.y = obsCamera.pos.y;
                    nnsVector.z = obsCamera.pos.z;
                    if (((int)obsCamera.flag & 8) != 0)
                    {
                        nnsVector.x -= (float)(((double)nnsVector.x - (double)obsCamera.work.x) * 2.0);
                        nnsVector.y -= (float)(((double)nnsVector.y - (double)obsCamera.work.y) * 2.0);
                        nnsVector.z -= (float)(((double)nnsVector.z - (double)obsCamera.work.z) * 2.0);
                    }
                    obsCamera.disp_pos.x = nnsVector.x + obsCamera.ofst.x;
                    obsCamera.disp_pos.y = nnsVector.y + obsCamera.ofst.y;
                    obsCamera.disp_pos.z = nnsVector.z + obsCamera.ofst.z;
                }
                if (((int)obsCamera.flag & 32) != 0)
                    AppMain.objCameraLimitCheck(obsCamera);
                obsCamera.disp_pos.x += obsCamera.disp_ofst.x;
                obsCamera.disp_pos.y += obsCamera.disp_ofst.y;
                obsCamera.disp_pos.z += obsCamera.disp_ofst.z;
                obsCamera.disp_ofst.x = 0.0f;
                obsCamera.disp_ofst.y = 0.0f;
                obsCamera.disp_ofst.z = 0.0f;
                if (((int)obsCamera.flag & 16) != 0)
                {
                    AppMain.nnMakePerspectiveMatrix(obsCamera.prj_pers_mtx, obsCamera.fovy, obsCamera.aspect, obsCamera.znear, obsCamera.zfar);
                    float num1 = (float)((double)AppMain.AMD_SCREEN_2D_WIDTH * (double)obsCamera.scale * 0.5 * 1.0);
                    float num2 = num1 * obsCamera.aspect;
                    obsCamera.left = -num2;
                    obsCamera.right = num2;
                    obsCamera.bottom = -num1;
                    obsCamera.top = num1;
                    AppMain.nnMakeOrthoMatrix(obsCamera.prj_ortho_mtx, obsCamera.left, obsCamera.right, obsCamera.bottom, obsCamera.top, obsCamera.znear, obsCamera.zfar);
                    switch (obsCamera.camera_type)
                    {
                        case AppMain.OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_ROLL:
                            AppMain.NNS_CAMERA_TARGET_ROLL cameraTargetRoll = AppMain.GlobalPool<AppMain.NNS_CAMERA_TARGET_ROLL>.Alloc();
                            int roll = obsCamera.roll;
                            if (((int)obsCamera.flag & 1073741824) != 0)
                                obsCamera.roll = 0;
                            AppMain.ObjCameraGetTargetRollCamera(obsCamera, cameraTargetRoll);
                            AppMain.nnMakeTargetRollCameraViewMatrix(obsCamera.view_mtx, cameraTargetRoll);
                            obsCamera.roll = roll;
                            AppMain.GlobalPool<AppMain.NNS_CAMERA_TARGET_ROLL>.Release(cameraTargetRoll);
                            continue;
                        case AppMain.OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_UP_TARGET:
                            AppMain.NNS_CAMERA_TARGET_UPTARGET cameraTargetUptarget = new AppMain.NNS_CAMERA_TARGET_UPTARGET();
                            AppMain.ObjCameraGetTargetUpTargetCamera(obsCamera, cameraTargetUptarget);
                            AppMain.nnMakeTargetUpTargetCameraViewMatrix(obsCamera.view_mtx, cameraTargetUptarget);
                            continue;
                        case AppMain.OBE_CAMERA_TYPE.OBE_CAMERA_TYPE_TARGET_UP_VEC:
                            AppMain.NNS_CAMERA_TARGET_UPVECTOR cameraTargetUpvector = new AppMain.NNS_CAMERA_TARGET_UPVECTOR();
                            AppMain.ObjCameraGetTargetUpVecCamera(obsCamera, cameraTargetUpvector);
                            AppMain.nnMakeTargetUpVectorCameraViewMatrix(obsCamera.view_mtx, cameraTargetUpvector);
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    private static void objCameraMove(AppMain.OBS_CAMERA obj_camera)
    {
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        if (obj_camera.target_obj != null)
        {
            nnsVector.x = AppMain.FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.x) + obj_camera.target_ofst.x;
            nnsVector.y = AppMain.FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.y - (((int)AppMain.OBD_LCD_Y << 1) + 200 << 12)) + obj_camera.target_ofst.y;
            nnsVector.z = AppMain.FXM_FX32_TO_FLOAT(obj_camera.target_obj.pos.z) + obj_camera.target_ofst.z;
        }
        else
        {
            nnsVector.x = obj_camera.target_pos.x - AppMain.FXM_FX32_TO_FLOAT((int)AppMain.OBD_LCD_X >> 1 << 12) + obj_camera.target_ofst.x;
            nnsVector.y = obj_camera.target_pos.y - AppMain.FXM_FX32_TO_FLOAT((int)AppMain.OBD_LCD_Y >> 1 << 12) + obj_camera.target_ofst.y;
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
                obj_camera.pos.x = AppMain.ObjShiftSetF(obj_camera.pos.x, nnsVector.x, (int)obj_camera.shift, obj_camera.spd_max.x, obj_camera.spd_add.x);
                obj_camera.pos.y = AppMain.ObjShiftSetF(obj_camera.pos.y, nnsVector.x, (int)obj_camera.shift, obj_camera.spd_max.y, obj_camera.spd_add.y);
                obj_camera.pos.z = AppMain.ObjShiftSetF(obj_camera.pos.z, nnsVector.x, (int)obj_camera.shift, obj_camera.spd_max.z, obj_camera.spd_add.z);
            }
            else
            {
                obj_camera.spd.x = (double)nnsVector.x == (double)obj_camera.pos.x ? AppMain.ObjSpdDownSetF(obj_camera.spd.x, obj_camera.spd_add.x) : AppMain.ObjSpdUpSetF(obj_camera.spd.x, obj_camera.spd_add.x, obj_camera.spd_max.x);
                obj_camera.spd.y = (double)nnsVector.y == (double)obj_camera.pos.y ? AppMain.ObjSpdDownSetF(obj_camera.spd.y, obj_camera.spd_add.y) : AppMain.ObjSpdUpSetF(obj_camera.spd.y, obj_camera.spd_add.y, obj_camera.spd_max.y);
                obj_camera.spd.z = (double)nnsVector.z == (double)obj_camera.pos.z ? AppMain.ObjSpdDownSetF(obj_camera.spd.z, obj_camera.spd_add.z) : AppMain.ObjSpdUpSetF(obj_camera.spd.z, obj_camera.spd_add.z, obj_camera.spd_max.z);
                if ((double)nnsVector.x > (double)obj_camera.pos.x)
                {
                    obj_camera.pos.x += obj_camera.spd.x;
                    if ((double)obj_camera.pos.x > (double)nnsVector.x)
                        obj_camera.pos.x = nnsVector.x;
                }
                else
                {
                    obj_camera.pos.x -= obj_camera.spd.x;
                    if ((double)obj_camera.pos.x < (double)nnsVector.x)
                        obj_camera.pos.x = nnsVector.x;
                }
                if ((double)nnsVector.y > (double)obj_camera.pos.y)
                {
                    obj_camera.pos.y += obj_camera.spd.y;
                    if ((double)obj_camera.pos.y > (double)nnsVector.y)
                        obj_camera.pos.y = nnsVector.y;
                }
                else
                {
                    obj_camera.pos.y -= obj_camera.spd.y;
                    if ((double)obj_camera.pos.y < (double)nnsVector.y)
                        obj_camera.pos.y = nnsVector.y;
                }
                if ((double)nnsVector.z > (double)obj_camera.pos.z)
                {
                    obj_camera.pos.z += obj_camera.spd.z;
                    if ((double)obj_camera.pos.z > (double)nnsVector.z)
                        obj_camera.pos.z = nnsVector.z;
                }
                else
                {
                    obj_camera.pos.z -= obj_camera.spd.z;
                    if ((double)obj_camera.pos.z < (double)nnsVector.z)
                        obj_camera.pos.z = nnsVector.z;
                }
            }
            if ((double)Math.Abs(nnsVector.x - obj_camera.pos.x) > (double)obj_camera.play_ofst_max.x)
                obj_camera.pos.x = (double)nnsVector.x <= (double)obj_camera.pos.x ? nnsVector.x + obj_camera.play_ofst_max.x : nnsVector.x - obj_camera.play_ofst_max.x;
            if ((double)Math.Abs(nnsVector.y - obj_camera.pos.y) > (double)obj_camera.play_ofst_max.y)
                obj_camera.pos.y = (double)nnsVector.y <= (double)obj_camera.pos.y ? nnsVector.y + obj_camera.play_ofst_max.y : nnsVector.y - obj_camera.play_ofst_max.y;
            if ((double)Math.Abs(nnsVector.z - obj_camera.pos.z) > (double)obj_camera.play_ofst_max.z)
                obj_camera.pos.z = (double)nnsVector.z <= (double)obj_camera.pos.z ? nnsVector.z + obj_camera.play_ofst_max.z : nnsVector.z - obj_camera.play_ofst_max.z;
            obj_camera.pos.z += obj_camera.ofst.z;
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        }
    }

    private static void objCameraLimitCheck(AppMain.OBS_CAMERA obj_camera)
    {
        AppMain.objCameraPosLimitCheck(obj_camera, obj_camera.disp_pos);
    }

    private static void objCameraPosLimitCheck(AppMain.OBS_CAMERA obj_camera, AppMain.NNS_VECTOR pPos)
    {
        int num1 = 0;
        if ((double)pPos.z != 0.0)
        {
            if ((double)obj_camera.limit[4] > (double)pPos.z)
                pPos.z = (float)obj_camera.limit[4];
            if ((double)obj_camera.limit[5] < (double)pPos.z)
                pPos.z = (float)obj_camera.limit[5];
            if (1.0 / (double)AppMain.ObjCameraDispScaleGet((int)obj_camera.index) * (double)AppMain.OBD_LCD_X > (double)(obj_camera.limit[2] - obj_camera.limit[0]))
            {
                float num2 = 1f / ((float)(obj_camera.limit[2] - obj_camera.limit[0]) / (float)AppMain.OBD_LCD_X);
                pPos.z = (double)pPos.z < 0.0 ? (float)(((double)num2 - 1.0) * -1.0) : (float)(((double)num2 - 1.0) * -2.0);
            }
            if (1.0 / (double)AppMain.ObjCameraDispScaleGet((int)obj_camera.index) * (double)AppMain.OBD_LCD_Y > (double)(obj_camera.limit[3] - obj_camera.limit[1]))
            {
                float num2 = 1f / ((float)(obj_camera.limit[3] - obj_camera.limit[1]) / (float)AppMain.OBD_LCD_X);
                pPos.z = (double)pPos.z < 0.0 ? (float)(((double)num2 - 1.0) * -1.0) : (float)(((double)num2 - 1.0) * -2.0);
            }
        }
        if ((double)pPos.z > 0.0)
            num1 = AppMain.FXM_FLOAT_TO_FX32(1f / AppMain.ObjCameraDispScaleGet((int)obj_camera.index) * (float)AppMain.OBD_LCD_X) - ((int)AppMain.OBD_LCD_X << 12) >> 13;
        if ((double)(obj_camera.limit[0] + num1) > (double)pPos.x)
            pPos.x = (float)(obj_camera.limit[0] + num1);
        if ((double)(obj_camera.limit[2] - (int)AppMain.OBD_LCD_X - num1) < (double)pPos.x)
            pPos.x = (float)(obj_camera.limit[2] - (int)AppMain.OBD_LCD_X - num1);
        if ((double)pPos.z > 0.0)
            num1 = AppMain.FXM_FLOAT_TO_FX32(1f / AppMain.ObjCameraDispScaleGet((int)obj_camera.index) * (float)AppMain.OBD_LCD_Y) - ((int)AppMain.OBD_LCD_Y << 12) >> 13;
        if ((double)(obj_camera.limit[1] + num1) > (double)pPos.y)
            pPos.y = (float)(obj_camera.limit[1] + num1);
        if ((double)(obj_camera.limit[3] - (int)AppMain.OBD_LCD_Y - num1) >= (double)pPos.y)
            return;
        pPos.y = (float)(obj_camera.limit[3] - (int)AppMain.OBD_LCD_Y - num1);
    }
}