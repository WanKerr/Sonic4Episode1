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
    public static void ObjDrawObjectSetToon(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawSetToon(obj_work.obj_3d);
    }

    public static void objDraw3DNNModelCommandFunc(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.obj_draw_user_command_func_tbl[command.command_id](command, drawflag);
    }

    public static void objDrawResetCache()
    {
        AppMain._objDraw3DNNSetCamera_Pool.ReleaseUsedObjects();
        AppMain._ObjDraw3DNNModel_Pool.ReleaseUsedObjects();
        AppMain.OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE_Pool.ReleaseUsedObjects();
        AppMain.OBS_DRAW_PARAM_3DNN_SORT_MODEL_Pool.ReleaseUsedObjects();
    }

    public static void ObjDrawInit()
    {
        AppMain.amDrawSetDrawCommandFunc(new AppMain._am_draw_command_delegate(AppMain.objDraw3DNNModelCommandFunc), new AppMain._am_draw_command_delegate(AppMain.objDraw3DNNModelCommandSortFunc));
        AppMain.ObjDrawClearNNCommandStateTbl();
        AppMain.g_obj_draw_3dnn_draw_state.drawflag = 0U;
        AppMain.g_obj_draw_3dnn_draw_state.diffuse.mode = 3;
        AppMain.g_obj_draw_3dnn_draw_state.diffuse.r = 1f;
        AppMain.g_obj_draw_3dnn_draw_state.diffuse.g = 1f;
        AppMain.g_obj_draw_3dnn_draw_state.diffuse.b = 1f;
        AppMain.g_obj_draw_3dnn_draw_state.ambient.mode = 3;
        AppMain.g_obj_draw_3dnn_draw_state.ambient.r = 1f;
        AppMain.g_obj_draw_3dnn_draw_state.ambient.g = 1f;
        AppMain.g_obj_draw_3dnn_draw_state.ambient.b = 1f;
        AppMain.g_obj_draw_3dnn_draw_state.alpha.mode = 3;
        AppMain.g_obj_draw_3dnn_draw_state.alpha.alpha = 1f;
        AppMain.g_obj_draw_3dnn_draw_state.specular.mode = 3;
        AppMain.g_obj_draw_3dnn_draw_state.specular.r = 1f;
        AppMain.g_obj_draw_3dnn_draw_state.specular.g = 1f;
        AppMain.g_obj_draw_3dnn_draw_state.specular.b = 1f;
        AppMain.g_obj_draw_3dnn_draw_state.blend.mode = 0;
        AppMain.g_obj_draw_3dnn_draw_state.envmap.texsrc = 1;
        AppMain.g_obj_draw_3dnn_draw_state.zmode.compare = 1U;
        AppMain.g_obj_draw_3dnn_draw_state.zmode.func = 515;
        AppMain.g_obj_draw_3dnn_draw_state.zmode.update = 1U;
        AppMain.NNS_MATRIX texmtx = AppMain.g_obj_draw_3dnn_draw_state.envmap.texmtx;
        AppMain.nnMakeUnitMatrix(texmtx);
        AppMain.nnTranslateMatrix(texmtx, texmtx, 0.5f, 0.5f, 0.0f);
        AppMain.nnScaleMatrix(texmtx, texmtx, 0.5f, 0.5f, 0.0f);
        for (int index = 0; index < 4; ++index)
        {
            AppMain.g_obj_draw_3dnn_draw_state.texoffset[index].mode = 2;
            AppMain.g_obj_draw_3dnn_draw_state.texoffset[index].u = 0.0f;
            AppMain.g_obj_draw_3dnn_draw_state.texoffset[index].v = 0.0f;
        }
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = new AppMain.NNS_VECTOR(-1f, -1f, -1f);
        AppMain.g_obj.def_user_light_flag = 1U;
        AppMain.g_obj.ambient_color.r = 0.8f;
        AppMain.g_obj.ambient_color.g = 0.8f;
        AppMain.g_obj.ambient_color.b = 0.8f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        int light_no = 0;
        AppMain.OBS_LIGHT obsLight = AppMain.g_obj.light[0];
        for (; light_no < AppMain.NNE_LIGHT_MAX; ++light_no)
            AppMain.ObjDrawSetParallelLight(light_no, ref col, 1f, nnsVector);
        AppMain.AoActSysSetDrawStateEnable(true);
        AppMain.AoActSysSetDrawState(6U);
        AppMain.AoActSysSetDrawTaskPrio(8192U);
    }

    public static void ObjDrawESEffectSystemInit(ushort pause_level, uint task_prio, uint group)
    {
        AppMain.amEffectSystemInit();
        AppMain.obj_draw_effect_server_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.objDraw3DESEffectServerMain), (AppMain.GSF_TASK_PROCEDURE)null, 0U, pause_level, task_prio, (int)group, (AppMain.TaskWorkFactoryDelegate)null, "ES_EFFECT_SERVER");
    }

    public static bool ObjDrawESEffectSystemIsActive()
    {
        return AppMain.obj_draw_effect_server_tcb != null;
    }

    public static void ObjDrawExit()
    {
        AppMain.nnSetMaterialCallback((AppMain.NNS_MATERIALCALLBACK_FUNC)null);
        AppMain.AoActSysSetDrawStateEnable(false);
    }

    public static void ObjDrawESEffectSystemExit()
    {
        AppMain.mtTaskClearTcb(AppMain.obj_draw_effect_server_tcb);
        AppMain.obj_draw_effect_server_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    public static void ObjDrawPrioritySet(AppMain.OBS_OBJECT_WORK obj_work, uint prio)
    {
    }

    public static void ObjDrawObjectActionSet(AppMain.OBS_OBJECT_WORK obj_work, int id)
    {
        obj_work.disp_flag &= 4294967287U;
        obj_work.disp_flag &= 4294967291U;
        if (obj_work.obj_3d == null || obj_work.obj_3d.motion == null)
            return;
        obj_work.obj_3d.act_id[0] = id;
        AppMain.amMotionSet(obj_work.obj_3d.motion, 0, id);
        obj_work.obj_3d.frame[0] = 0.0f;
    }

    public static void ObjDrawObjectActionSet3DNN(
      AppMain.OBS_OBJECT_WORK obj_work,
      int id,
      int mbuf_id)
    {
        obj_work.disp_flag &= 4294967287U;
        obj_work.disp_flag &= 4294967291U;
        AppMain.ObjDrawAction3dActionSet3DNN(obj_work.obj_3d, id, mbuf_id);
    }

    public static void ObjDrawAction3dActionSet3DNN(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      int id,
      int mbuf_id)
    {
        if ((uint)mbuf_id >= 2U)
            return;
        obj_3d.act_id[mbuf_id] = id;
        AppMain.amMotionSet(obj_3d.motion, mbuf_id, id);
        obj_3d.frame[mbuf_id] = 0.0f;
    }

    public static void ObjDrawObjectActionSet3DNNBlend(AppMain.OBS_OBJECT_WORK obj_work, int id)
    {
        obj_work.disp_flag &= 4294967287U;
        obj_work.disp_flag &= 4294967291U;
        AppMain.ObjDrawAction3dActionSet3DNNBlend(obj_work.obj_3d, id);
    }

    public static void ObjDrawAction3dActionSet3DNNBlend(AppMain.OBS_ACTION3D_NN_WORK obj_3d, int id)
    {
        AppMain.ObjDrawAction3dActionSet3DNN(obj_3d, obj_3d.act_id[0], 1);
        obj_3d.frame[1] = obj_3d.frame[0];
        AppMain.ObjDrawAction3dActionSet3DNN(obj_3d, id, 0);
        obj_3d.marge = 1f;
        obj_3d.flag |= 1U;
    }

    public static void ObjDrawObjectActionSet3DNNMaterial(AppMain.OBS_OBJECT_WORK obj_work, int id)
    {
        obj_work.disp_flag &= 4294967287U;
        obj_work.disp_flag &= 4294967291U;
        AppMain.ObjDrawAction3dActionSet3DNNMaterial(obj_work.obj_3d, id);
    }

    public static void ObjDrawAction3dActionSet3DNNMaterial(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      int id)
    {
        obj_3d.mat_act_id = id;
        AppMain.amMotionMaterialSet(obj_3d.motion, id);
        obj_3d.mat_frame = 0.0f;
    }

    public static int ObjDrawActionGet(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_3d != null && obj_work.obj_3d.motion != null)
            return obj_work.obj_3d.act_id[0];
        return obj_work.obj_2d != null ? (int)obj_work.obj_2d.act_id : 0;
    }

    public static int ObjDrawActionGet3DNN(AppMain.OBS_OBJECT_WORK obj_work, int mbuf_id)
    {
        return mbuf_id >= 2 || obj_work.obj_3d == null || obj_work.obj_3d.motion == null ? 0 : obj_work.obj_3d.act_id[mbuf_id];
    }

    public static void ObjDrawActionSummary(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_3d != null && AppMain.ObjAction3dNNModelLoadCheck(obj_work.obj_3d))
            AppMain.ObjDrawObjectAction3DNN(obj_work, obj_work.obj_3d);
        if (obj_work.obj_3des != null && AppMain.ObjAction3dESEffectLoadCheck(obj_work.obj_3des))
            AppMain.ObjDrawObjectAction3DES(obj_work, obj_work.obj_3des);
        if (obj_work.obj_2d == null || !AppMain.ObjAction2dAMALoadCheck(obj_work.obj_2d))
            return;
        AppMain.ObjDrawObjectAction2DAMA(obj_work, obj_work.obj_2d);
    }

    public static void ObjDrawClearNNCommandStateTbl()
    {
        for (int index = 0; index < 18; ++index)
        {
            AppMain.obj_draw_3dnn_command_state_tbl[index] = uint.MaxValue;
            AppMain.obj_draw_3dnn_command_state_exe_end_scene_tbl[index] = false;
        }
        AppMain.obj_draw_3dnn_command_state_tbl[0] = 0U;
        AppMain.obj_draw_3dnn_command_state_exe_end_scene_tbl[0] = true;
    }

    public static void ObjDrawSetNNCommandStateTbl(uint tbl_no, uint command_state, bool end_scene)
    {
        AppMain.obj_draw_3dnn_command_state_tbl[(int)tbl_no] = command_state;
        AppMain.obj_draw_3dnn_command_state_exe_end_scene_tbl[(int)tbl_no] = end_scene;
    }

    public static void ObjDrawNNStart()
    {
        AppMain.amDrawMakeTask(AppMain._objDrawStart_DT, (ushort)4096, (object)null);
    }

    public static void ObjDraw3DNNSetCamera(int camera_id, int proj_type)
    {
        AppMain.OBS_CAMERA obj_camera = (AppMain.OBS_CAMERA)null;
        if (!AppMain.GmMainIsDrawEnable())
            return;
        if (camera_id >= 0)
            obj_camera = AppMain.ObjCameraGet(camera_id);
        AppMain.objDraw3DNNSetCamera(obj_camera, proj_type, obj_camera.command_state);
    }

    public static void ObjDraw3DNNSetCameraEx(int camera_id, int proj_type, uint command_state)
    {
        AppMain.OBS_CAMERA obj_camera = (AppMain.OBS_CAMERA)null;
        if (!AppMain.GmMainIsDrawEnable())
            return;
        if (camera_id >= 0)
            obj_camera = AppMain.ObjCameraGet(camera_id);
        AppMain.objDraw3DNNSetCamera(obj_camera, proj_type, command_state);
    }

    public static void ObjDraw3DNNUserFunc(
      AppMain.OBF_DRAW_USER_DT_FUNC user_func,
      object param,
      int param_size,
      uint command_state)
    {
        AppMain._ObjDraw3DNNUserFunc(user_func, (object)null, command_state);
    }

    public static void _ObjDraw3DNNUserFunc(
      AppMain.OBF_DRAW_USER_DT_FUNC user_func,
      object param,
      uint command_state)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.OBS_DRAW_PARAM_3DNN_USER_FUNC param3DnnUserFunc = AppMain.amDrawAlloc_OBS_DRAW_PARAM_3DNN_USER_FUNC();
        param3DnnUserFunc.param = param;
        param3DnnUserFunc.func = user_func;
        AppMain.amDrawRegistCommand(command_state, AppMain.OBD_DRAW_USER_COMMAND_3DNN_USER_FUNC, (object)param3DnnUserFunc);
    }

    public static void ObjDrawObjectAction3DNN(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION3D_NN_WORK obj_3d)
    {
        AppMain.VecFx32 pos = obj_work.pos;
        AppMain.VecU16 vecU16 = new AppMain.VecU16(obj_work.dir);
        pos.x += obj_work.ofst.x;
        pos.y += obj_work.ofst.y;
        pos.z += obj_work.ofst.z;
        if (obj_work.dir_fall != (ushort)0)
            vecU16.z += obj_work.dir_fall;
        AppMain.ObjDrawAction3DNN(obj_3d, new AppMain.VecFx32?(pos), new AppMain.VecU16?(vecU16), obj_work.scale, ref obj_work.disp_flag);
    }

    public static void ObjDrawAction3DNN(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      AppMain.VecFx32? pos,
      AppMain.VecU16? dir,
      AppMain.VecFx32 scale,
      ref uint p_disp_flag)
    {
        uint? p_disp_flag1 = new uint?(p_disp_flag);
        AppMain.ObjDrawAction3DNN(obj_3d, pos, dir, new AppMain.VecFx32?(scale), ref p_disp_flag1);
        p_disp_flag = p_disp_flag1.Value;
    }

    public static void ObjDrawAction3DNN(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      AppMain.VecFx32? pos,
      AppMain.VecU16? dir,
      AppMain.VecFx32? scale,
      ref uint? p_disp_flag)
    {
        uint p_disp_flag1 = 0;
        if (p_disp_flag.HasValue)
        {
            uint? nullable1 = p_disp_flag;
            uint? nullable2 = nullable1.HasValue ? new uint?(nullable1.GetValueOrDefault() & 16U) : new uint?();
            if ((nullable2.GetValueOrDefault() != 0U ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
            {
                ref uint? local = ref p_disp_flag;
                uint? nullable3 = local;
                local = nullable3.HasValue ? new uint?(nullable3.GetValueOrDefault() & 4261412855U) : new uint?();
            }
            p_disp_flag1 = p_disp_flag.Value;
        }
        if (obj_3d != null && obj_3d._object != null)
        {
            bool flag = false;
            p_disp_flag1 &= 3758096383U;
            if (pos.HasValue && ((int)p_disp_flag1 & 293609472) == 0)
            {
                AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain._g_obj.glb_camera_id);
                float num1 = obsCamera.disp_pos.x + obsCamera.bottom;
                float num2 = obsCamera.disp_pos.x + obsCamera.top;
                float num3 = (float)-((double)obsCamera.disp_pos.y + (double)obsCamera.top);
                float num4 = (float)-((double)obsCamera.disp_pos.y + (double)obsCamera.bottom);
                float num5 = 1f;
                if (((int)p_disp_flag1 & 65536) == 0)
                {
                    int x = scale.Value.x;
                    int y = scale.Value.y;
                    if (x < y)
                        x = y;
                    int z = scale.Value.z;
                    if (x < z)
                        x = z;
                    num5 = AppMain.FX_FX32_TO_F32(x);
                }
                float num6 = num5 * 3.2f;
                float num7 = AppMain.FX_FX32_TO_F32(pos.Value.x) - obj_3d._object.Radius * 1.2f * num6;
                float num8 = AppMain.FX_FX32_TO_F32(pos.Value.x) + obj_3d._object.Radius * 1.2f * num6;
                float num9 = AppMain.FX_FX32_TO_F32(pos.Value.y) - obj_3d._object.Radius * 1.2f * num6;
                float num10 = AppMain.FX_FX32_TO_F32(pos.Value.y) + obj_3d._object.Radius * 1.2f * num6;
                if ((double)num1 > (double)num8)
                    flag = true;
                if ((double)num2 < (double)num7)
                    flag = true;
                if ((double)num3 > (double)num10)
                    flag = true;
                if ((double)num4 < (double)num9)
                    flag = true;
            }
            if (flag)
                p_disp_flag1 |= 536870944U;
        }
        if (((int)p_disp_flag1 & 1073741824) != 0)
        {
            if (!p_disp_flag.HasValue)
                return;
            ref uint? local1 = ref p_disp_flag;
            uint? nullable1 = local1;
            local1 = nullable1.HasValue ? new uint?(nullable1.GetValueOrDefault() & 3758096383U) : new uint?();
            ref uint? local2 = ref p_disp_flag;
            uint? nullable2 = local2;
            uint num = p_disp_flag1 & 570425352U;
            local2 = nullable2.HasValue ? new uint?(nullable2.GetValueOrDefault() | num) : new uint?();
        }
        else
        {
            AppMain.NNS_MATRIX action3DnnObjMtx = AppMain.ObjDrawAction3DNN_obj_mtx;
            AppMain.VecFx32 vecFx32_1 = new AppMain.VecFx32(4096, 4096, 4096);
            AppMain.nnMakeUnitMatrix(action3DnnObjMtx);
            if (pos.HasValue && ((int)p_disp_flag1 & 8192) == 0)
            {
                AppMain.VecFx32 vecFx32_2 = pos.Value;
                if (((int)p_disp_flag1 & 2097152) == 0)
                    vecFx32_2.y = -vecFx32_2.y;
                if (((int)p_disp_flag1 & 524288) == 0)
                {
                    if (AppMain._g_obj.glb_scale.x != 4096)
                        vecFx32_2.x = AppMain.FX_Mul(vecFx32_2.x, AppMain._g_obj.glb_scale.x);
                    if (AppMain._g_obj.glb_scale.y != 4096)
                        vecFx32_2.y = AppMain.FX_Mul(vecFx32_2.y, AppMain._g_obj.glb_scale.y);
                    if (AppMain._g_obj.glb_scale.z != 4096)
                        vecFx32_2.z = AppMain.FX_Mul(vecFx32_2.z, AppMain._g_obj.glb_scale.z);
                }
                AppMain.nnTranslateMatrix(action3DnnObjMtx, action3DnnObjMtx, AppMain.FX_FX32_TO_F32(vecFx32_2.x), AppMain.FX_FX32_TO_F32(vecFx32_2.y), AppMain.FX_FX32_TO_F32(vecFx32_2.z));
            }
            if (dir.HasValue && ((int)p_disp_flag1 & 256) == 0)
            {
                if (((int)p_disp_flag1 & 2097152) == 0)
                    AppMain.nnRotateXYZMatrix(action3DnnObjMtx, action3DnnObjMtx, (int)-dir.Value.x, (int)dir.Value.y, (int)-dir.Value.z);
                else
                    AppMain.nnRotateXYZMatrix(action3DnnObjMtx, action3DnnObjMtx, (int)dir.Value.x, (int)dir.Value.y, (int)dir.Value.z);
            }
            if (((int)p_disp_flag1 & 4194304) == 0)
            {
                if (((int)p_disp_flag1 & 67108864) == 0)
                {
                    if (((int)p_disp_flag1 & 1) != 0)
                        AppMain.nnRotateYMatrix(action3DnnObjMtx, action3DnnObjMtx, 49152);
                    else
                        AppMain.nnRotateYMatrix(action3DnnObjMtx, action3DnnObjMtx, 16384);
                }
                else
                {
                    if (((int)p_disp_flag1 & 2) != 0)
                        AppMain.nnRotateXMatrix(action3DnnObjMtx, action3DnnObjMtx, 32768);
                    if (((int)p_disp_flag1 & 1) != 0)
                        AppMain.nnRotateYMatrix(action3DnnObjMtx, action3DnnObjMtx, 32768);
                }
            }
            if (scale.HasValue && ((int)p_disp_flag1 & 65536) == 0)
                vecFx32_1 = scale.Value;
            if (((int)p_disp_flag1 & 1048576) == 0)
            {
                vecFx32_1.x = AppMain.FX_Mul(vecFx32_1.x, AppMain._g_obj.draw_scale.x);
                vecFx32_1.y = AppMain.FX_Mul(vecFx32_1.y, AppMain._g_obj.draw_scale.y);
                vecFx32_1.z = AppMain.FX_Mul(vecFx32_1.z, AppMain._g_obj.draw_scale.z);
            }
            if (((int)p_disp_flag1 & 524288) == 0)
            {
                vecFx32_1.x = AppMain.FX_Mul(vecFx32_1.x, AppMain._g_obj.glb_scale.x);
                vecFx32_1.y = AppMain.FX_Mul(vecFx32_1.y, AppMain._g_obj.glb_scale.y);
                vecFx32_1.z = AppMain.FX_Mul(vecFx32_1.z, AppMain._g_obj.glb_scale.z);
            }
            AppMain.nnScaleMatrix(action3DnnObjMtx, action3DnnObjMtx, AppMain.FX_FX32_TO_F32(vecFx32_1.x), AppMain.FX_FX32_TO_F32(vecFx32_1.y), AppMain.FX_FX32_TO_F32(vecFx32_1.z));
            if (((int)p_disp_flag1 & 8388608) != 0)
                AppMain.nnMultiplyMatrix(action3DnnObjMtx, obj_3d.user_obj_mtx, action3DnnObjMtx);
            if (((int)p_disp_flag1 & 16777216) != 0)
                AppMain.nnMultiplyMatrix(action3DnnObjMtx, action3DnnObjMtx, obj_3d.user_obj_mtx_r);
            AppMain.amMatrixPush(action3DnnObjMtx);
            if (obj_3d.motion != null && obj_3d.motion.mmobject != null)
                AppMain.ObjDrawAction3DNNMaterialUpdate(obj_3d, ref p_disp_flag1);
            if (obj_3d.motion != null && obj_3d.motion.mtnbuf[0] != null)
            {
                AppMain.ObjDrawAction3DNNMotionUpdate(obj_3d, ref p_disp_flag1);
                if (obj_3d.mtn_cb_func != null)
                    obj_3d.mtn_cb_func(obj_3d.motion, obj_3d._object, obj_3d.mtn_cb_param);
                if (((int)p_disp_flag1 & 32) == 0)
                {
                    AppMain.AMS_DRAWSTATE draw_state = (AppMain.AMS_DRAWSTATE)null;
                    if (((int)p_disp_flag1 & 134217728) != 0)
                        draw_state = obj_3d.draw_state;
                    if ((double)obj_3d.marge == 0.0 || (double)obj_3d.marge == 1.0)
                    {
                        int motion_id;
                        float num;
                        if ((double)obj_3d.marge == 0.0)
                        {
                            motion_id = obj_3d.act_id[0];
                            num = obj_3d.frame[0];
                        }
                        else
                        {
                            motion_id = obj_3d.act_id[1];
                            num = obj_3d.frame[1];
                        }
                        AppMain.NNS_MOTION motion = obj_3d.motion.mtnbuf[motion_id & (int)ushort.MaxValue];
                        float frame = num + AppMain.amMotionGetStartFrame(obj_3d.motion, motion_id);
                        if (obj_3d.motion != null && obj_3d.motion.mmobject != null)
                            AppMain.ObjDraw3DNNMotionMaterialMotion(obj_3d.motion, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, obj_3d.mplt_cb_func, obj_3d.mplt_cb_param, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                        else
                            AppMain.ObjDraw3DNNDrawMotion(motion, frame, obj_3d._object, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, obj_3d.mplt_cb_func, obj_3d.mplt_cb_param, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                    }
                    else if (obj_3d.motion != null && obj_3d.motion.mmobject != null)
                        AppMain.ObjDraw3DNNMotionMaterialMotion(obj_3d.motion, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, obj_3d.mplt_cb_func, obj_3d.mplt_cb_param, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                    else
                        AppMain.ObjDraw3DNNMotion(obj_3d.motion, obj_3d.motion._object, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, obj_3d.mplt_cb_func, obj_3d.mplt_cb_param, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                }
                obj_3d.user_param = (object)null;
                obj_3d.mplt_cb_param = (object)null;
                obj_3d.material_cb_param = (object)null;
            }
            else
            {
                if (((int)p_disp_flag1 & 32) == 0)
                {
                    AppMain.AMS_DRAWSTATE draw_state = (AppMain.AMS_DRAWSTATE)null;
                    if (((int)p_disp_flag1 & 134217728) != 0)
                        draw_state = obj_3d.draw_state;
                    if (obj_3d.motion != null && obj_3d.motion.mmobject != null)
                        AppMain.ObjDraw3DNNMotionMaterialMotion(obj_3d.motion, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, (AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT)null, (object)null, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                    else
                        AppMain.ObjDraw3DNNModel(obj_3d._object, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                }
                obj_3d.user_param = (object)null;
                obj_3d.mplt_cb_param = (object)null;
                obj_3d.material_cb_param = (object)null;
            }
            AppMain.amMatrixPop();
            if (!p_disp_flag.HasValue)
                return;
            ref uint? local1 = ref p_disp_flag;
            uint? nullable1 = local1;
            local1 = nullable1.HasValue ? new uint?(nullable1.GetValueOrDefault() & 3758096383U) : new uint?();
            ref uint? local2 = ref p_disp_flag;
            uint? nullable2 = local2;
            uint num1 = p_disp_flag1 & 570425352U;
            local2 = nullable2.HasValue ? new uint?(nullable2.GetValueOrDefault() | num1) : new uint?();
        }
    }

    public static void ObjDrawAction3DNNMotionUpdate(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      ref uint p_disp_flag)
    {
        uint? p_disp_flag1 = new uint?(p_disp_flag);
        AppMain.ObjDrawAction3DNNMotionUpdate(obj_3d, ref p_disp_flag1);
        p_disp_flag = p_disp_flag1.Value;
    }

    public static void ObjDrawAction3DNNMotionUpdate(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      ref uint? p_disp_flag)
    {
        uint num1 = 0;
        float num2 = 0.0f;
        float num3 = 0.0f;
        if (p_disp_flag.HasValue)
            num1 = p_disp_flag.Value;
        if (((int)num1 & 4096) == 0)
        {
            if (((int)num1 & 16) == 0)
            {
                if (((int)obj_3d.flag & 1) != 0)
                {
                    obj_3d.marge -= obj_3d.blend_spd;
                    if ((double)obj_3d.marge <= 0.0)
                    {
                        obj_3d.marge = 0.0f;
                        obj_3d.flag &= 4294967294U;
                    }
                }
                if (((int)num1 & 4112) == 0)
                {
                    num2 = obj_3d.speed[0] * AppMain.FX_FX32_TO_F32(AppMain.g_obj.speed);
                    num3 = obj_3d.speed[1] * AppMain.FX_FX32_TO_F32(AppMain.g_obj.speed);
                }
                if ((double)obj_3d.marge < 1.0 || ((int)obj_3d.flag & 3) != 0)
                    obj_3d.frame[0] += num2;
                if (((int)obj_3d.flag & 1) == 0 && ((double)obj_3d.marge > 0.0 || ((int)obj_3d.flag & 3) != 0))
                    obj_3d.frame[1] += num3;
                if (((int)num1 & 4) != 0)
                {
                    for (int index = 0; index < 2; ++index)
                    {
                        float num4 = AppMain.amMotionGetEndFrame(obj_3d.motion, obj_3d.act_id[index]) - AppMain.amMotionGetStartFrame(obj_3d.motion, obj_3d.act_id[index]);
                        while ((double)obj_3d.frame[index] >= (double)num4)
                        {
                            obj_3d.frame[index] = obj_3d.frame[index] - num4;
                            if (index == 0)
                                num1 |= 8U;
                        }
                    }
                }
                else
                {
                    for (int index = 0; index < 2; ++index)
                    {
                        float num4 = AppMain.amMotionGetEndFrame(obj_3d.motion, obj_3d.act_id[index]) - AppMain.amMotionGetStartFrame(obj_3d.motion, obj_3d.act_id[index]);
                        if ((double)obj_3d.frame[index] >= (double)num4 - 1.0)
                        {
                            obj_3d.frame[index] = num4 - 1f;
                            if (index == 0)
                                num1 |= 8U;
                        }
                    }
                }
            }
            if ((double)obj_3d.marge < 1.0 || ((int)obj_3d.flag & 3) != 0)
                AppMain.amMotionSetFrame(obj_3d.motion, 0, obj_3d.frame[0] + AppMain.amMotionGetStartFrame(obj_3d.motion, obj_3d.act_id[0]));
            if ((double)obj_3d.marge > 0.0 || ((int)obj_3d.flag & 3) != 0)
                AppMain.amMotionSetFrame(obj_3d.motion, 1, obj_3d.frame[1] + AppMain.amMotionGetStartFrame(obj_3d.motion, obj_3d.act_id[1]));
            AppMain.amMotionGet(obj_3d.motion, obj_3d.marge, obj_3d.per);
        }
        if (!p_disp_flag.HasValue)
            return;
        ref uint? local = ref p_disp_flag;
        uint? nullable = local;
        uint num5 = num1 & 8U;
        local = nullable.HasValue ? new uint?(nullable.GetValueOrDefault() | num5) : new uint?();
    }

    public static void ObjDrawAction3DNNMaterialUpdate(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      ref uint p_disp_flag)
    {
        uint? p_disp_flag1 = new uint?(p_disp_flag);
        AppMain.ObjDrawAction3DNNMaterialUpdate(obj_3d, ref p_disp_flag1);
        p_disp_flag = p_disp_flag1.Value;
    }

    public static void ObjDrawAction3DNNMaterialUpdate(
      AppMain.OBS_ACTION3D_NN_WORK obj_3d,
      ref uint? p_disp_flag)
    {
        uint num1 = 0;
        float num2 = 0.0f;
        if (p_disp_flag.HasValue)
            num1 = p_disp_flag.Value;
        if (((int)num1 & 4096) == 0)
        {
            if (((int)num1 & 16) == 0)
            {
                if (((int)num1 & 4112) == 0)
                    num2 = obj_3d.mat_speed * AppMain.FX_FX32_TO_F32(AppMain.g_obj.speed);
                obj_3d.mat_frame += num2;
                if (((int)num1 & 4) != 0)
                {
                    float num3 = AppMain.amMotionMaterialGetEndFrame(obj_3d.motion, obj_3d.mat_act_id) - AppMain.amMotionMaterialGetStartFrame(obj_3d.motion, obj_3d.mat_act_id);
                    while ((double)obj_3d.mat_frame >= (double)num3)
                    {
                        obj_3d.mat_frame -= num3;
                        num1 |= 33554432U;
                    }
                }
                else
                {
                    float num3 = AppMain.amMotionMaterialGetEndFrame(obj_3d.motion, obj_3d.mat_act_id) - AppMain.amMotionMaterialGetStartFrame(obj_3d.motion, obj_3d.mat_act_id);
                    if ((double)obj_3d.mat_frame >= (double)num3 - 1.0)
                    {
                        obj_3d.mat_frame = num3 - 1f;
                        num1 |= 33554432U;
                    }
                }
            }
            AppMain.amMotionMaterialSetFrame(obj_3d.motion, obj_3d.mat_frame + AppMain.amMotionMaterialGetStartFrame(obj_3d.motion, obj_3d.mat_act_id));
            AppMain.amMotionMaterialCalc(obj_3d.motion);
        }
        if (!p_disp_flag.HasValue)
            return;
        ref uint? local = ref p_disp_flag;
        uint? nullable = local;
        uint num4 = num1 & 33554432U;
        local = nullable.HasValue ? new uint?(nullable.GetValueOrDefault() | num4) : new uint?();
    }

    public static void ObjDraw3DNNModel(
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      AppMain.MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AppMain.AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.OBS_DRAW_PARAM_3DNN_MODEL drawParam3DnnModel = AppMain._ObjDraw3DNNModel_Pool.Alloc();
        AppMain.NNS_MATRIX mtx = drawParam3DnnModel.mtx;
        AppMain.nnCopyMatrix(mtx, AppMain.amMatrixGetCurrent());
        AppMain.AMS_PARAM_DRAW_OBJECT amsParamDrawObject = drawParam3DnnModel.param;
        amsParamDrawObject._object = _object;
        if ((double)AppMain.gmMapTransX != 0.0)
            AppMain.nnScaleMatrix(mtx, mtx, 1.005f, 1.005f, 1f);
        amsParamDrawObject.mtx = mtx;
        amsParamDrawObject.sub_obj_type = sub_obj_type;
        amsParamDrawObject.flag = drawflag;
        amsParamDrawObject.texlist = texlist;
        amsParamDrawObject.material_func = (AppMain.NNS_MATERIALCALLBACK_FUNC)null;
        amsParamDrawObject.scaleZ = 1f;
        drawParam3DnnModel.state = (AppMain.AMS_DRAWSTATE)null;
        if (draw_state != null)
        {
            drawParam3DnnModel.state = drawParam3DnnModel.draw_state;
            drawParam3DnnModel.draw_state.Assign(draw_state);
        }
        drawParam3DnnModel.use_light_flag = use_light_flag;
        drawParam3DnnModel.user_func = user_func;
        drawParam3DnnModel.user_param = user_param;
        drawParam3DnnModel.material_cb_func = material_cb_func;
        drawParam3DnnModel.material_cb_param = material_cb_param;
        AppMain.amDrawRegistCommand(command_state, AppMain.OBD_DRAW_USER_COMMAND_3DNN_MODEL, (object)amsParamDrawObject);
    }

    public static void ObjDraw3DNNModelMaterialMotion(
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      AppMain.MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AppMain.AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.OBS_DRAW_PARAM_3DNN_MODEL drawParam3DnnModel = new AppMain.OBS_DRAW_PARAM_3DNN_MODEL();
        AppMain.NNS_MATRIX mtx = drawParam3DnnModel.mtx;
        AppMain.nnCopyMatrix(mtx, AppMain.amMatrixGetCurrent());
        AppMain.AMS_PARAM_DRAW_OBJECT amsParamDrawObject = drawParam3DnnModel.param;
        amsParamDrawObject._object = _object;
        amsParamDrawObject.mtx = mtx;
        amsParamDrawObject.sub_obj_type = sub_obj_type;
        amsParamDrawObject.flag = drawflag;
        amsParamDrawObject.texlist = texlist;
        amsParamDrawObject.material_func = (AppMain.NNS_MATERIALCALLBACK_FUNC)null;
        amsParamDrawObject.scaleZ = 1f;
        drawParam3DnnModel.state = (AppMain.AMS_DRAWSTATE)null;
        if (draw_state != null)
        {
            drawParam3DnnModel.state = drawParam3DnnModel.draw_state;
            drawParam3DnnModel.draw_state.Assign(draw_state);
        }
        drawParam3DnnModel.use_light_flag = use_light_flag;
        drawParam3DnnModel.user_func = user_func;
        drawParam3DnnModel.user_param = user_param;
        drawParam3DnnModel.material_cb_func = material_cb_func;
        drawParam3DnnModel.material_cb_param = material_cb_param;
        AppMain.amDrawRegistCommand(command_state, AppMain.OBD_DRAW_USER_COMMAND_3DNN_MODEL_MATMTN, (object)amsParamDrawObject);
    }

    public static void ObjDraw3DNNMotion(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      AppMain.MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT mplt_cb_func,
      object mplt_cb_param,
      AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AppMain.AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        int nodeNum = motion.node_num;
        AppMain.OBS_DRAW_PARAM_3DNN_MOTION drawParam3DnnMotion = AppMain.amDrawAlloc_OBS_DRAW_PARAM_3DNN_MOTION();
        AppMain.NNS_MATRIX mtx = drawParam3DnnMotion.mtx;
        AppMain.nnCopyMatrix(mtx, AppMain.amMatrixGetCurrent());
        AppMain.AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs = drawParam3DnnMotion.param;
        paramDrawMotionTrs._object = _object;
        paramDrawMotionTrs.mtx = mtx;
        paramDrawMotionTrs.sub_obj_type = sub_obj_type;
        paramDrawMotionTrs.flag = drawflag;
        paramDrawMotionTrs.texlist = texlist;
        paramDrawMotionTrs.trslist = AppMain.amDrawAlloc_NNS_TRS(nodeNum);
        paramDrawMotionTrs.material_func = (AppMain.NNS_MATERIALCALLBACK_FUNC)null;
        for (int index = 0; index < nodeNum; ++index)
            paramDrawMotionTrs.trslist[index] = motion.data[index];
        int motionId = motion.mbuf[0].motion_id;
        paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & (int)ushort.MaxValue];
        paramDrawMotionTrs.frame = motion.mbuf[0].frame;
        drawParam3DnnMotion.state = (AppMain.AMS_DRAWSTATE)null;
        if (draw_state != null)
        {
            drawParam3DnnMotion.state = drawParam3DnnMotion.draw_state;
            drawParam3DnnMotion.draw_state.Assign(draw_state);
        }
        drawParam3DnnMotion.use_light_flag = use_light_flag;
        drawParam3DnnMotion.user_func = user_func;
        drawParam3DnnMotion.user_param = user_param;
        drawParam3DnnMotion.mplt_cb_func = mplt_cb_func;
        drawParam3DnnMotion.mplt_cb_param = mplt_cb_param;
        drawParam3DnnMotion.material_cb_func = material_cb_func;
        drawParam3DnnMotion.material_cb_param = material_cb_param;
        AppMain.amDrawRegistCommand(command_state, AppMain.OBD_DRAW_USER_COMMAND_3DNN_MOTION, (object)paramDrawMotionTrs);
    }

    public static void ObjDraw3DNNMotionMaterialMotion(
      AppMain.AMS_MOTION motion,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      AppMain.MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT mplt_cb_func,
      object mplt_cb_param,
      AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AppMain.AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        if (motion.mmobject == null)
        {
            AppMain.ObjDraw3DNNMotion(motion, motion._object, texlist, drawflag, sub_obj_type, user_func, user_param, command_state, mplt_cb_func, mplt_cb_param, material_cb_func, material_cb_param, draw_state, use_light_flag);
        }
        else
        {
            int nodeNum = motion.node_num;
            AppMain.OBS_DRAW_PARAM_3DNN_MOTION drawParam3DnnMotion = AppMain.amDrawAlloc_OBS_DRAW_PARAM_3DNN_MOTION();
            AppMain.NNS_MATRIX mtx = drawParam3DnnMotion.mtx;
            mtx.Assign(AppMain.amMatrixGetCurrent());
            AppMain.AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs = drawParam3DnnMotion.param;
            paramDrawMotionTrs.mtx = mtx;
            paramDrawMotionTrs.sub_obj_type = sub_obj_type;
            paramDrawMotionTrs.flag = drawflag;
            paramDrawMotionTrs.texlist = texlist;
            paramDrawMotionTrs.trslist = AppMain.amDrawAlloc_NNS_TRS(nodeNum);
            paramDrawMotionTrs.material_func = (AppMain.NNS_MATERIALCALLBACK_FUNC)null;
            for (int index = 0; index < nodeNum; ++index)
            {
                paramDrawMotionTrs.trslist[index] = AppMain.amDraw_NNS_TRS_Pool.Alloc();
                paramDrawMotionTrs.trslist[index].Assign(motion.data[index]);
            }
            paramDrawMotionTrs._object = motion._object;
            paramDrawMotionTrs.mmotion = motion.mmtn[motion.mmotion_id];
            paramDrawMotionTrs.mframe = motion.mmotion_frame;
            int motionId = motion.mbuf[0].motion_id;
            if (motion.mtnfile[motionId >> 16].file != null)
            {
                paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & (int)ushort.MaxValue];
                paramDrawMotionTrs.frame = motion.mbuf[0].frame;
            }
            else
            {
                paramDrawMotionTrs.motion = (AppMain.NNS_MOTION)null;
                paramDrawMotionTrs.frame = 0.0f;
            }
            drawParam3DnnMotion.state = (AppMain.AMS_DRAWSTATE)null;
            if (draw_state != null)
            {
                drawParam3DnnMotion.state = drawParam3DnnMotion.draw_state;
                drawParam3DnnMotion.draw_state.Assign(draw_state);
            }
            drawParam3DnnMotion.use_light_flag = use_light_flag;
            drawParam3DnnMotion.user_func = user_func;
            drawParam3DnnMotion.user_param = user_param;
            drawParam3DnnMotion.mplt_cb_func = mplt_cb_func;
            drawParam3DnnMotion.mplt_cb_param = mplt_cb_param;
            drawParam3DnnMotion.material_cb_func = material_cb_func;
            drawParam3DnnMotion.material_cb_param = material_cb_param;
            AppMain.amDrawRegistCommand(command_state, AppMain.OBD_DRAW_USER_COMMAND_3DNN_MOTION_MATMTN, (object)paramDrawMotionTrs);
        }
    }

    public static void ObjDraw3DNNDrawMotion(
      AppMain.NNS_MOTION motion,
      float frame,
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      AppMain.MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT mplt_cb_func,
      object mplt_cb_param,
      AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AppMain.AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.OBS_DRAW_PARAM_3DNN_DRAW_MOTION param3DnnDrawMotion = AppMain.amDrawAlloc_OBS_DRAW_PARAM_3DNN_DRAW_MOTION();
        AppMain.NNS_MATRIX mtx = param3DnnDrawMotion.mtx;
        AppMain.nnCopyMatrix(mtx, AppMain.amMatrixGetCurrent());
        AppMain.AMS_PARAM_DRAW_MOTION amsParamDrawMotion = param3DnnDrawMotion.param;
        amsParamDrawMotion._object = _object;
        amsParamDrawMotion.mtx = mtx;
        amsParamDrawMotion.sub_obj_type = sub_obj_type;
        amsParamDrawMotion.flag = drawflag;
        amsParamDrawMotion.texlist = texlist;
        amsParamDrawMotion.motion = motion;
        amsParamDrawMotion.frame = frame;
        amsParamDrawMotion.material_func = (AppMain.NNS_MATERIALCALLBACK_FUNC)null;
        param3DnnDrawMotion.state = (AppMain.AMS_DRAWSTATE)null;
        switch (draw_state)
        {
            case null:
                param3DnnDrawMotion.use_light_flag = use_light_flag;
                param3DnnDrawMotion.user_func = user_func;
                param3DnnDrawMotion.user_param = user_param;
                param3DnnDrawMotion.mplt_cb_func = mplt_cb_func;
                param3DnnDrawMotion.mplt_cb_param = mplt_cb_param;
                param3DnnDrawMotion.material_cb_func = material_cb_func;
                param3DnnDrawMotion.material_cb_param = material_cb_param;
                AppMain.amDrawRegistCommand(command_state, AppMain.OBD_DRAW_USER_COMMAND_3DNN_DRAW_MOTION, (object)amsParamDrawMotion);
                break;
            default:
                param3DnnDrawMotion.state = param3DnnDrawMotion.draw_state;
                param3DnnDrawMotion.draw_state.Assign(draw_state);
                goto case null;
        }
    }

    public static void ObjDraw3DNNDrawMotionMaterialMotion(
      AppMain.NNS_MOTION motion,
      float frame,
      AppMain.NNS_OBJECT _object,
      AppMain.NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      AppMain.MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      AppMain.MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT mplt_cb_func,
      object mplt_cb_param,
      AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AppMain.AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.OBS_DRAW_PARAM_3DNN_DRAW_MOTION param3DnnDrawMotion = new AppMain.OBS_DRAW_PARAM_3DNN_DRAW_MOTION();
        AppMain.NNS_MATRIX mtx = param3DnnDrawMotion.mtx;
        AppMain.nnCopyMatrix(mtx, AppMain.amMatrixGetCurrent());
        AppMain.AMS_PARAM_DRAW_MOTION amsParamDrawMotion = param3DnnDrawMotion.param;
        amsParamDrawMotion._object = _object;
        amsParamDrawMotion.mtx = mtx;
        amsParamDrawMotion.sub_obj_type = sub_obj_type;
        amsParamDrawMotion.flag = drawflag;
        amsParamDrawMotion.texlist = texlist;
        amsParamDrawMotion.motion = motion;
        amsParamDrawMotion.frame = frame;
        amsParamDrawMotion.material_func = (AppMain.NNS_MATERIALCALLBACK_FUNC)null;
        param3DnnDrawMotion.state = (AppMain.AMS_DRAWSTATE)null;
        if (draw_state != null)
        {
            param3DnnDrawMotion.state = param3DnnDrawMotion.draw_state;
            param3DnnDrawMotion.draw_state.Assign(draw_state);
        }
        param3DnnDrawMotion.use_light_flag = use_light_flag;
        param3DnnDrawMotion.user_func = user_func;
        param3DnnDrawMotion.user_param = user_param;
        param3DnnDrawMotion.mplt_cb_func = mplt_cb_func;
        param3DnnDrawMotion.mplt_cb_param = mplt_cb_param;
        param3DnnDrawMotion.material_cb_func = material_cb_func;
        param3DnnDrawMotion.material_cb_param = material_cb_param;
        AppMain.amDrawRegistCommand(command_state, AppMain.OBD_DRAW_USER_COMMAND_3DNN_DRAW_MOTION_MATMTN, (object)amsParamDrawMotion);
    }

    public static void ObjDraw3DNNDrawPrimitive(AppMain.AMS_PARAM_DRAW_PRIMITIVE prim)
    {
        AppMain.ObjDraw3DNNDrawPrimitive(prim, 0U, 0, 0);
    }

    public static void ObjDraw3DNNDrawPrimitive(AppMain.AMS_PARAM_DRAW_PRIMITIVE prim, uint command)
    {
        AppMain.ObjDraw3DNNDrawPrimitive(prim, command, 0, 0);
    }

    public static void ObjDraw3DNNDrawPrimitive(
      AppMain.AMS_PARAM_DRAW_PRIMITIVE prim,
      uint command,
      int light,
      int cull)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE dnnDrawPrimitive = AppMain.OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE_Pool.Alloc();
        dnnDrawPrimitive.dat.Assign(prim);
        AppMain.nnCopyMatrix(dnnDrawPrimitive.mtx, AppMain.amMatrixGetCurrent());
        dnnDrawPrimitive.light = light;
        dnnDrawPrimitive.cull = cull;
        AppMain._ObjDraw3DNNUserFunc(AppMain._objDraw3DNNDrawPrimitive_DT, (object)dnnDrawPrimitive, command);
    }

    public static uint ObjDraw3DNNGetMaterialUserData(AppMain.NNS_DRAWCALLBACK_VAL val)
    {
        uint num = 0;
        switch (val.pMaterial.fType)
        {
            case 1:
                AppMain.mppAssertNotImpl();
                break;
            case 2:
                AppMain.mppAssertNotImpl();
                break;
            case 4:
                AppMain.mppAssertNotImpl();
                break;
            case 8:
                num = ((AppMain.NNS_MATERIAL_GLES11_DESC)val.pMaterial.pMaterial).User;
                break;
            default:
                num = 0U;
                break;
        }
        return num;
    }

    public static void ObjDrawSetToon(AppMain.OBS_ACTION3D_NN_WORK obj_3d)
    {
    }

    public static int ObjDrawToonMaterialCallback(AppMain.NNS_DRAWCALLBACK_VAL val, object param)
    {
        return AppMain.nnPutMaterialCore(val);
    }

    public static void ObjDrawObjectAction3DES(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION3D_ES_WORK obj_3des)
    {
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        AppMain.VecU16 dir = new AppMain.VecU16();
        vecFx32.Assign(obj_work.pos);
        dir.Assign(obj_work.dir);
        vecFx32.x += obj_work.ofst.x;
        vecFx32.y += obj_work.ofst.y;
        vecFx32.z += obj_work.ofst.z;
        if (obj_work.dir_fall != (ushort)0)
            dir.z += obj_work.dir_fall;
        AppMain.ObjDrawAction3DES(obj_3des, new AppMain.VecFx32?(vecFx32), ref dir, new AppMain.VecFx32?(obj_work.scale), ref obj_work.disp_flag);
    }

    public static void ObjDrawAction3DES(
      AppMain.OBS_ACTION3D_ES_WORK obj_3des,
      AppMain.VecFx32? pos,
      ref AppMain.VecU16 dir,
      AppMain.VecFx32? scale,
      ref uint p_disp_flag)
    {
        uint? p_disp_flag1 = new uint?(p_disp_flag);
        AppMain.ObjDrawAction3DES(obj_3des, pos, new AppMain.VecU16?(dir), scale, ref p_disp_flag1);
        p_disp_flag = p_disp_flag1.Value;
    }

    public static void ObjDrawAction3DES(
      AppMain.OBS_ACTION3D_ES_WORK obj_3des,
      AppMain.VecFx32? pos,
      AppMain.VecU16? dir,
      AppMain.VecFx32? scale,
      ref uint? p_disp_flag)
    {
        if (obj_3des.ecb == null)
            return;
        uint num = 0;
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.amDrawAlloc_NNS_MATRIX();
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion3 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion4 = new AppMain.NNS_QUATERNION();
        AppMain.vec_posObjDrawAction3DES.w = 0.0f;
        AppMain.amQuatInit(ref nnsQuaternion1);
        AppMain.amVectorInit(ref AppMain.vecObjDrawAction3DES);
        AppMain.nnMakeUnitMatrix(nnsMatrix1);
        if (p_disp_flag.HasValue)
        {
            num = p_disp_flag.Value;
            if (((int)p_disp_flag.Value & 16) == 0)
            {
                ref uint? local = ref p_disp_flag;
                uint? nullable = local;
                local = nullable.HasValue ? new uint?(nullable.GetValueOrDefault() & 4294967287U) : new uint?();
            }
        }
        AppMain.amQuatEulerToQuatXYZ(ref nnsQuaternion2, (int)obj_3des.disp_rot.x, (int)obj_3des.disp_rot.y, (int)obj_3des.disp_rot.z);
        if (((int)num & 4194304) == 0)
        {
            if (((int)num & 1) != 0)
            {
                if (((int)obj_3des.flag & 4) != 0)
                    AppMain.amQuatEulerToQuatXYZ(ref nnsQuaternion3, AppMain.NNM_DEGtoA32(0.0f), AppMain.NNM_DEGtoA32(0.0f), AppMain.NNM_DEGtoA32(270f));
                else
                    AppMain.amQuatEulerToQuatXYZ(ref nnsQuaternion3, AppMain.NNM_DEGtoA32(0.0f), AppMain.NNM_DEGtoA32(270f), AppMain.NNM_DEGtoA32(0.0f));
            }
            else if (((int)obj_3des.flag & 4) != 0)
                AppMain.amQuatEulerToQuatXYZ(ref nnsQuaternion3, AppMain.NNM_DEGtoA32(0.0f), AppMain.NNM_DEGtoA32(0.0f), AppMain.NNM_DEGtoA32(90f));
            else
                AppMain.amQuatEulerToQuatXYZ(ref nnsQuaternion3, AppMain.NNM_DEGtoA32(0.0f), AppMain.NNM_DEGtoA32(90f), AppMain.NNM_DEGtoA32(0.0f));
        }
        else
            AppMain.amQuatInit(ref nnsQuaternion3);
        if (dir.HasValue && ((int)num & 256) == 0)
        {
            if (((int)num & 2097152) == 0)
                AppMain.amQuatEulerToQuatXYZ(ref nnsQuaternion4, (int)-dir.Value.x, (int)dir.Value.y, (int)-dir.Value.z);
            else
                AppMain.amQuatEulerToQuatXYZ(ref nnsQuaternion4, (int)dir.Value.x, (int)dir.Value.y, (int)dir.Value.z);
            if (((int)obj_3des.flag & 32) != 0)
                AppMain.nnMultiplyQuaternion(ref nnsQuaternion4, ref nnsQuaternion4, ref obj_3des.user_dir_quat);
        }
        else
            AppMain.amQuatInit(ref nnsQuaternion4);
        AppMain.vec_dispObjDrawAction3DES.w = 0.0f;
        AppMain.amVectorSet(out AppMain.vec_dispObjDrawAction3DES, obj_3des.disp_ofst.x, obj_3des.disp_ofst.y, obj_3des.disp_ofst.z);
        if (pos.HasValue)
        {
            AppMain.VecFx32 vecFx32 = pos.Value;
            if (((int)num & 2097152) == 0)
                vecFx32.y = -vecFx32.y;
            AppMain.amVectorSet(out AppMain.vec_posObjDrawAction3DES, AppMain.FX_FX32_TO_F32(vecFx32.x), AppMain.FX_FX32_TO_F32(vecFx32.y), AppMain.FX_FX32_TO_F32(vecFx32.z));
        }
        else
            AppMain.amVectorInit(ref AppMain.vec_posObjDrawAction3DES);
        AppMain.amVectorOne(ref AppMain.vec_scaleObjDrawAction3DES);
        if (scale.HasValue && ((int)num & 65536) == 0)
        {
            if (((int)obj_3des.flag & 8) != 0)
            {
                AppMain.amVectorSet(out AppMain.vec_scaleObjDrawAction3DES, AppMain.FX_FX32_TO_F32(scale.Value.x), AppMain.FX_FX32_TO_F32(scale.Value.y), AppMain.FX_FX32_TO_F32(scale.Value.z));
                AppMain.amEffectSetSizeRate(obj_3des.ecb, 1f);
            }
            else
                AppMain.amEffectSetSizeRate(obj_3des.ecb, AppMain.FX_FX32_TO_F32(scale.Value.x));
        }
        if (((int)obj_3des.flag & 1) != 0)
        {
            AppMain.amVectorAdd(ref AppMain.vecObjDrawAction3DES, ref AppMain.vec_dispObjDrawAction3DES, ref AppMain.vecObjDrawAction3DES);
            AppMain.amQuatMulti(ref nnsQuaternion1, ref nnsQuaternion3, ref nnsQuaternion1);
            AppMain.amQuatMulti(ref nnsQuaternion1, ref nnsQuaternion4, ref nnsQuaternion1);
            AppMain.SNNS_MATRIX dst1;
            AppMain.nnMakeQuaternionMatrix(out dst1, ref nnsQuaternion1);
            AppMain.SNNS_VECTOR snnsVector;
            snnsVector.x = AppMain.vecObjDrawAction3DES.x;
            snnsVector.y = AppMain.vecObjDrawAction3DES.y;
            snnsVector.z = AppMain.vecObjDrawAction3DES.z;
            AppMain.nnTransformVector(ref snnsVector, ref dst1, ref snnsVector);
            AppMain.vecObjDrawAction3DES.x = snnsVector.x;
            AppMain.vecObjDrawAction3DES.y = snnsVector.y;
            AppMain.vecObjDrawAction3DES.z = snnsVector.z;
            AppMain.amQuatMulti(ref nnsQuaternion1, ref nnsQuaternion1, ref nnsQuaternion2);
            if (((int)obj_3des.flag & 16) != 0)
                AppMain.amEffectSetRotate(obj_3des.ecb, ref nnsQuaternion1, 1);
            else
                AppMain.amEffectSetRotate(obj_3des.ecb, ref nnsQuaternion1);
            AppMain.SNNS_MATRIX dst2;
            AppMain.nnMakeScaleMatrix(out dst2, AppMain.vec_scaleObjDrawAction3DES.x, AppMain.vec_scaleObjDrawAction3DES.y, AppMain.vec_scaleObjDrawAction3DES.z);
            AppMain.nnMultiplyMatrix(nnsMatrix1, ref dst2, nnsMatrix1);
            if (((int)obj_3des.flag & 2) != 0)
            {
                AppMain.SNNS_MATRIX dst3;
                AppMain.nnMakeTranslateMatrix(out dst3, AppMain.vec_posObjDrawAction3DES.x, AppMain.vec_posObjDrawAction3DES.y, AppMain.vec_posObjDrawAction3DES.z);
                AppMain.nnMultiplyMatrix(nnsMatrix1, ref dst3, nnsMatrix1);
            }
            else
                AppMain.amVectorAdd(ref AppMain.vecObjDrawAction3DES, ref AppMain.vec_posObjDrawAction3DES, ref AppMain.vecObjDrawAction3DES);
            AppMain.amEffectSetTranslate(obj_3des.ecb, ref AppMain.vecObjDrawAction3DES);
        }
        else
        {
            AppMain.amQuatMulti(ref nnsQuaternion1, ref nnsQuaternion3, ref nnsQuaternion1);
            AppMain.amQuatMulti(ref nnsQuaternion1, ref nnsQuaternion4, ref nnsQuaternion1);
            AppMain.nnMakeQuaternionMatrix(nnsMatrix1, ref nnsQuaternion1);
            AppMain.SNNS_VECTOR vec;
            vec.x = AppMain.vec_posObjDrawAction3DES.x;
            vec.y = AppMain.vec_posObjDrawAction3DES.y;
            vec.z = AppMain.vec_posObjDrawAction3DES.z;
            AppMain.nnCopyVectorMatrixTranslation(nnsMatrix1, ref vec);
            AppMain.SNNS_MATRIX dst1;
            AppMain.nnMakeScaleMatrix(out dst1, AppMain.vec_scaleObjDrawAction3DES.x, AppMain.vec_scaleObjDrawAction3DES.y, AppMain.vec_scaleObjDrawAction3DES.z);
            AppMain.nnMultiplyMatrix(nnsMatrix1, nnsMatrix1, ref dst1);
            AppMain.SNNS_MATRIX dst2;
            AppMain.nnMakeTranslateMatrix(out dst2, AppMain.vec_dispObjDrawAction3DES.x, AppMain.vec_dispObjDrawAction3DES.y, AppMain.vec_dispObjDrawAction3DES.z);
            AppMain.nnMultiplyMatrix(nnsMatrix1, nnsMatrix1, ref dst2);
            AppMain.SNNS_MATRIX dst3;
            AppMain.nnMakeQuaternionMatrix(out dst3, ref nnsQuaternion2);
            AppMain.nnMultiplyMatrix(nnsMatrix1, nnsMatrix1, ref dst3);
        }
        AppMain.ObjDraw3DESSetCamera(obj_3des, nnsMatrix1);
        AppMain.ObjDraw3DESMatrixPush(nnsMatrix1, obj_3des.command_state);
        float unitFrame = AppMain.amEffectGetUnitFrame();
        if (((int)num & 4096) == 0)
        {
            if (((int)num & 16) != 0)
                AppMain.amEffectSetUnitTime(0.0f, 60);
            else
                AppMain.amEffectSetUnitTime(obj_3des.speed * AppMain.FX_FX32_TO_F32(AppMain._g_obj.speed), 60);
            AppMain.amEffectUpdate(obj_3des.ecb);
            if (AppMain.amEffectIsDelete(obj_3des.ecb) != 0)
            {
                obj_3des.ecb = (AppMain.AMS_AME_ECB)null;
                if (p_disp_flag.HasValue)
                {
                    ref uint? local = ref p_disp_flag;
                    uint? nullable = local;
                    local = nullable.HasValue ? new uint?(nullable.GetValueOrDefault() | 8U) : new uint?();
                }
            }
        }
        AppMain.amEffectSetUnitTime(unitFrame, 60);
        if (((int)num & 32) == 0 && obj_3des.ecb != null)
        {
            AppMain.ObjDraw3DESEffect(obj_3des.ecb, obj_3des.texlist, obj_3des.command_state);
            if (((int)obj_3des.flag & 64) != 0)
            {
                AppMain.NNS_MATRIX nnsMatrix2 = AppMain.amDrawAlloc_NNS_MATRIX();
                AppMain.nnMakeTranslateMatrix(nnsMatrix2, obj_3des.dup_draw_ofst.x, obj_3des.dup_draw_ofst.y, obj_3des.dup_draw_ofst.z);
                AppMain.nnMultiplyMatrix(nnsMatrix2, nnsMatrix2, nnsMatrix1);
                AppMain.ObjDraw3DESMatrixPop(obj_3des.command_state);
                AppMain.ObjDraw3DESSetCamera(obj_3des, nnsMatrix2);
                AppMain.ObjDraw3DESMatrixPush(nnsMatrix2, obj_3des.command_state);
                AppMain.ObjDraw3DESEffect(obj_3des.ecb, obj_3des.texlist, obj_3des.command_state);
            }
        }
        AppMain.ObjDraw3DESMatrixPop(obj_3des.command_state);
    }

    public static void ObjDraw3DESEffect(
      AppMain.AMS_AME_ECB ecb,
      AppMain.NNS_TEXLIST texlist,
      uint command_state)
    {
        AppMain.amEffectDraw(ecb, texlist, command_state);
    }

    public static void ObjDraw3DESMatrixPush(AppMain.NNS_MATRIX mtx, uint command_state)
    {
        AppMain._ObjDraw3DNNUserFunc(AppMain._objDraw3DESMatrixPush_UserFunc, (object)mtx, command_state);
    }

    public static void ObjDraw3DESMatrixPop(uint command_state)
    {
        AppMain.ObjDraw3DNNUserFunc(AppMain._objDraw3DESMatrixPop_UserFunc, (object)null, 0, command_state);
    }

    public static void ObjDrawKillAction3DES(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_3des.ecb == null)
            return;
        AppMain.amEffectKill(obj_work.obj_3des.ecb);
    }

    public static void ObjDraw3DESSetCamera(
      AppMain.OBS_ACTION3D_ES_WORK obj_3des,
      AppMain.NNS_MATRIX obj_mtx)
    {
        AppMain.SNNS_VECTOR disp_pos = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector = new AppMain.SNNS_VECTOR();
        AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, AppMain.g_obj.glb_camera_type, obj_3des.command_state);
        AppMain.ObjCameraDispPosGet(AppMain.g_obj.glb_camera_id, out disp_pos);
        AppMain.amVectorSet(ref snnsVector, -obj_mtx.M03, -obj_mtx.M13, -obj_mtx.M23);
        AppMain.nnAddVector(ref disp_pos, ref snnsVector, ref disp_pos);
        AppMain.amEffectSetCameraPos(ref disp_pos);
    }

    public static void ObjDrawObjectAction2DAMA(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.OBS_ACTION2D_AMA_WORK obj_2d)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32(obj_work.pos);
        AppMain.VecU16 vecU16 = new AppMain.VecU16(obj_work.dir);
        vecFx32.x += obj_work.ofst.x;
        vecFx32.y += obj_work.ofst.y;
        vecFx32.z += obj_work.ofst.z;
        AppMain.ObjDrawAction2DAMA(obj_2d, new AppMain.VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), new AppMain.VecFx32?(obj_work.scale), ref obj_work.disp_flag);
    }

    public static void ObjDrawAction2DAMA(
      AppMain.OBS_ACTION2D_AMA_WORK obj_2d,
      AppMain.VecFx32? pos,
      AppMain.VecU16? dir,
      AppMain.VecFx32? scale,
      ref uint p_disp_flag)
    {
        uint num = 0;
        if (!AppMain.GmMainIsDrawEnable())
            return;
        if (p_disp_flag != 0U)
        {
            if (((int)p_disp_flag & 16) == 0)
                p_disp_flag &= 4294967287U;
            num = p_disp_flag;
        }
        AppMain.AoActSetTexture(obj_2d.texlist);
        AppMain.AOS_ACT_ACM drawAction2DamaAcm = AppMain.ObjDrawAction2DAMA_acm;
        AppMain.AoActAcmInit(drawAction2DamaAcm);
        if (pos.HasValue && ((int)num & 8192) == 0)
        {
            drawAction2DamaAcm.trans_x = AppMain.FXM_FX32_TO_FLOAT(pos.Value.x);
            drawAction2DamaAcm.trans_y = AppMain.FXM_FX32_TO_FLOAT(pos.Value.y);
            drawAction2DamaAcm.trans_z = AppMain.FXM_FX32_TO_FLOAT(pos.Value.z);
        }
        if (dir.HasValue && ((int)num & 256) == 0)
            drawAction2DamaAcm.rotate = AppMain.NNM_A32toDEG((int)dir.Value.z);
        if (scale.HasValue && ((int)num & 65536) == 0)
        {
            drawAction2DamaAcm.scale_x = AppMain.FXM_FX32_TO_FLOAT(scale.Value.x);
            drawAction2DamaAcm.scale_y = AppMain.FXM_FX32_TO_FLOAT(scale.Value.y);
        }
        drawAction2DamaAcm.color = obj_2d.color;
        drawAction2DamaAcm.fade = obj_2d.fade;
        AppMain.AoActAcmPush(drawAction2DamaAcm);
        if (((int)num & 4096) == 0)
        {
            float frame = obj_2d.speed * AppMain.FX_FX32_TO_F32(AppMain.g_obj.speed) * AppMain.GmMainGetDrawMotionSpeed();
            if ((double)obj_2d.act.frame == (double)obj_2d.frame)
            {
                AppMain.AoActUpdate(obj_2d.act, frame);
                obj_2d.frame = obj_2d.act.frame;
            }
            else
            {
                AppMain.AoActSetFrame(obj_2d.act, obj_2d.frame);
                AppMain.AoActUpdate(obj_2d.act, 0.0f);
            }
            if (((int)num & 4) != 0)
            {
                if (AppMain.AoActIsEnd(obj_2d.act))
                {
                    obj_2d.frame = 0.0f;
                    num |= 8U;
                }
            }
            else if (AppMain.AoActIsEnd(obj_2d.act))
                num |= 8U;
        }
        if (((int)num & 32) == 0)
            AppMain.AoActSortRegAction(obj_2d.act);
        AppMain.AoActAcmPop(1U);
        p_disp_flag |= num & 8U;
    }

    public static void ObjDrawAction2DAMADrawStart()
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.ObjDraw3DNNUserFunc(AppMain._objDraw2DAMAPre_DT, (object)null, 0, 6U);
        AppMain.AoActSortExecuteFix();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    public static void ObjDrawSetParallelLight(
      int light_no,
      ref AppMain.NNS_RGBA col,
      float intensity,
      AppMain.NNS_VECTOR vec)
    {
        AppMain.nnSetUpParallelLight(AppMain.g_obj.light[light_no].parallel, ref col, intensity, vec);
        AppMain.g_obj.light[light_no].light_type = 1U;
    }

    private void ObjDrawSetPointLight(
      int light_no,
      ref AppMain.NNS_RGBA col,
      float intensity,
      AppMain.NNS_VECTOR pos,
      float falloffstart,
      float falloffend)
    {
        AppMain.mppAssertNotImpl();
        AppMain.nnSetUpPointLight(AppMain.g_obj.light[light_no].point, ref col, intensity, pos, falloffstart, falloffend);
        AppMain.g_obj.light[light_no].light_type = 2U;
    }

    private void ObjDrawSetTargetSpotLight(
      int light_no,
      ref AppMain.NNS_RGBA col,
      float intensity,
      AppMain.NNS_VECTOR pos,
      AppMain.NNS_VECTOR target,
      int innerangle,
      int outerangle,
      float falloffstart,
      float falloffend)
    {
        AppMain.mppAssertNotImpl();
        AppMain.nnSetUpTargetSpotLight(AppMain.g_obj.light[light_no].target_spot, ref col, intensity, pos, target, innerangle, outerangle, falloffstart, falloffend);
        AppMain.g_obj.light[light_no].light_type = 4U;
    }

    private void ObjDrawSetRotationSpotLight(
      int light_no,
      ref AppMain.NNS_RGBA col,
      float intensity,
      AppMain.NNS_VECTOR pos,
      int rottype,
      AppMain.NNS_ROTATE_A32 rotation,
      int innerangle,
      int outerangle,
      float falloffstart,
      float falloffend)
    {
        AppMain.mppAssertNotImpl();
        AppMain.nnSetUpRotationSpotLight(AppMain.g_obj.light[light_no].rotation_spot, ref col, intensity, pos, rottype, rotation, innerangle, outerangle, falloffstart, falloffend);
        AppMain.g_obj.light[light_no].light_type = 8U;
    }

    public static void objDrawStart_DT(AppMain.AMS_TCB tcb)
    {
        AppMain.nnSetAmbientColor(AppMain.g_obj.ambient_color.r, AppMain.g_obj.ambient_color.g, AppMain.g_obj.ambient_color.b);
        for (int no = 0; no < AppMain.NNE_LIGHT_MAX; ++no)
        {
            if (((long)AppMain.g_obj.def_user_light_flag & (long)(1 << no)) != 0L)
            {
                AppMain.nnSetLight(no, (object)AppMain.g_obj.light[no].light_param, AppMain.g_obj.light[no].light_type);
                AppMain.nnSetLightSwitch(no, 1);
            }
            else
                AppMain.nnSetLightSwitch(no, 0);
        }
        AppMain.nnPutLightSettings();
        for (int index = 0; index < 18; ++index)
        {
            if (AppMain.obj_draw_3dnn_command_state_tbl[index] != uint.MaxValue)
            {
                AppMain.amDrawExecCommand(AppMain.obj_draw_3dnn_command_state_tbl[index], AppMain.g_obj.drawflag);
                if (AppMain.obj_draw_3dnn_command_state_exe_end_scene_tbl[index])
                    AppMain.amDrawEndScene();
            }
        }
    }

    public static void objDraw3DNNSetCamera(
      AppMain.OBS_CAMERA obj_camera,
      int proj_type,
      uint command_state)
    {
        AppMain.OBS_DRAW_PARAM_3DNN_SET_CAMERA param3DnnSetCamera = AppMain._objDraw3DNNSetCamera_Pool.Alloc();
        param3DnnSetCamera.proj_type = proj_type;
        if (obj_camera != null)
        {
            switch (proj_type)
            {
                case 1:
                    param3DnnSetCamera.prj_mtx.Assign(obj_camera.prj_ortho_mtx);
                    break;
                default:
                    param3DnnSetCamera.prj_mtx.Assign(obj_camera.prj_pers_mtx);
                    break;
            }
            param3DnnSetCamera.view_mtx.Assign(obj_camera.view_mtx);
        }
        else
        {
            AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_CAMERAPTR nnsCameraptr = new AppMain.NNS_CAMERAPTR();
            AppMain.NNS_CAMERA_TARGET_ROLL cam = new AppMain.NNS_CAMERA_TARGET_ROLL();
            AppMain.NNS_VECTOR vec = new AppMain.NNS_VECTOR(0.0f, 0.0f, 0.0f);
            nnsCameraptr.fType = (uint)byte.MaxValue;
            nnsCameraptr.pCamera = (object)cam;
            cam.Target.Assign(vec);
            cam.Position.Assign(cam.Target);
            cam.Position.z += 50f;
            cam.Position.x += 0.0f;
            cam.Roll = AppMain.NNM_DEGtoA32(0.0f);
            cam.Fovy = AppMain.NNM_DEGtoA32(45f);
            cam.Aspect = AppMain.AMD_SCREEN_ASPECT;
            cam.ZNear = 1f;
            cam.ZFar = 60000f;
            switch (proj_type)
            {
                case 1:
                    float top = (float)((double)AppMain.g_obj.disp_height * (5.0 / 64.0) * 0.5);
                    float right = top * cam.Aspect;
                    AppMain.nnMakeOrthoMatrix(nnsMatrix2, -right, right, -top, top, cam.ZNear, cam.ZFar);
                    param3DnnSetCamera.prj_mtx.Assign(nnsMatrix2);
                    break;
                default:
                    AppMain.nnMakePerspectiveMatrix(nnsMatrix2, cam.Fovy, cam.Aspect, cam.ZNear, cam.ZFar);
                    param3DnnSetCamera.prj_mtx.Assign(nnsMatrix2);
                    break;
            }
            AppMain.nnMakeTargetRollCameraViewMatrix(nnsMatrix1, cam);
            param3DnnSetCamera.view_mtx.Assign(nnsMatrix1);
        }
        AppMain.amEffectSetWorldViewMatrix(param3DnnSetCamera.view_mtx);
        AppMain.amDrawRegistCommand(command_state, AppMain.OBD_DRAW_USER_COMMAND_3DNN_SET_CAMERA, (object)param3DnnSetCamera);
    }

    public static void objDraw3DNNModel_DT(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.amDrawAlloc_NNS_MATRIX();
        AppMain.amMatrixPush();
        AppMain.OBS_DRAW_PARAM_3DNN_MODEL drawParam3DnnModel = !(command.param is AppMain.AMS_PARAM_DRAW_OBJECT) ? (AppMain.OBS_DRAW_PARAM_3DNN_MODEL)command.param : (AppMain.OBS_DRAW_PARAM_3DNN_MODEL)(AppMain.AMS_PARAM_DRAW_OBJECT)command.param;
        uint alternativeLighting = drawParam3DnnModel.use_light_flag & 98304U;
        if (((int)AppMain.g_obj.def_user_light_flag ^ (int)drawParam3DnnModel.use_light_flag) != 0)
            AppMain.objDrawSetDrawLight(drawParam3DnnModel.use_light_flag);
        if (drawParam3DnnModel.user_func != null)
            drawParam3DnnModel.user_func(drawParam3DnnModel.user_param);
        AppMain.AMS_PARAM_DRAW_OBJECT amsParamDrawObject = drawParam3DnnModel.param;
        int nNode = amsParamDrawObject._object.nNode;
        AppMain.OBS_DRAW_PARAM_3DNN_SORT_MODEL_Pool.Alloc();
        if (AppMain._objDraw3DNNModel_DT.plt_mtx == null || AppMain._objDraw3DNNModel_DT.plt_mtx.Length < nNode)
        {
            AppMain._objDraw3DNNModel_DT.plt_mtx = new AppMain.NNS_MATRIX[nNode];
            for (int index = 0; index < nNode; ++index)
                AppMain._objDraw3DNNModel_DT.plt_mtx[index] = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        }
        AppMain.NNS_MATRIX[] pltMtx = AppMain._objDraw3DNNModel_DT.plt_mtx;
        if (AppMain._objDraw3DNNModel_DT.nstat == null || AppMain._objDraw3DNNModel_DT.nstat.Length < (nNode + 3 & -4))
            AppMain._objDraw3DNNModel_DT.nstat = new uint[nNode + 3 & -4];
        uint[] nstat = AppMain._objDraw3DNNModel_DT.nstat;
        if (amsParamDrawObject.mtx != null)
        {
            AppMain.nnMultiplyMatrix(nnsMatrix, AppMain.amMatrixGetCurrent(), amsParamDrawObject.mtx);
            AppMain.nnMultiplyMatrix(nnsMatrix, AppMain._am_draw_world_view_matrix, nnsMatrix);
        }
        else
            AppMain.nnMultiplyMatrix(nnsMatrix, AppMain._am_draw_world_view_matrix, AppMain.amMatrixGetCurrent());
        AppMain.nnSetUpNodeStatusList(nstat, nNode, 0U);
        AppMain.nnCalcMatrixPalette(pltMtx, nstat, amsParamDrawObject._object, nnsMatrix, AppMain._am_default_stack, 1U);
        if (amsParamDrawObject.texlist != null)
            AppMain.nnSetTextureList(amsParamDrawObject.texlist);
        if (drawParam3DnnModel.state != null)
        {
            AppMain.amDrawPushState();
            AppMain.amDrawSetState(drawParam3DnnModel.state);
        }
        if (drawParam3DnnModel.material_cb_func != null)
            AppMain.objDraw3DNNSetMaterialCallback(drawParam3DnnModel.material_cb_func, drawParam3DnnModel.material_cb_param);
        if (command.command_id == AppMain.OBD_DRAW_USER_COMMAND_3DNN_MODEL)
            AppMain.nnDrawObject(amsParamDrawObject._object, pltMtx, nstat, (uint)((int)amsParamDrawObject.sub_obj_type | 256 | 512 | 7), amsParamDrawObject.flag | drawflag | AppMain.amDrawGetState().drawflag, alternativeLighting);
        else
            AppMain.nnDrawMaterialMotionObject(amsParamDrawObject._object, pltMtx, nstat, (uint)((int)amsParamDrawObject.sub_obj_type | 256 | 512 | 7), amsParamDrawObject.flag | drawflag | AppMain.amDrawGetState().drawflag);
        if (drawParam3DnnModel.material_cb_func != null)
            AppMain.objDraw3DNNSetMaterialCallback((AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE)null, (object)null);
        if (drawParam3DnnModel.state != null)
            AppMain.amDrawPopState();
        if (((int)AppMain.g_obj.def_user_light_flag ^ (int)drawParam3DnnModel.use_light_flag) != 0)
            AppMain.objDrawSetDefaultLight();
        AppMain.amMatrixPop();
    }

    private static void objDraw3DNNMotion_DT(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.SNNS_MATRIX snnsMatrix = new AppMain.SNNS_MATRIX();
        AppMain.amMatrixPush();
        AppMain.OBS_DRAW_PARAM_3DNN_MOTION drawParam3DnnMotion = !(command.param is AppMain.OBS_DRAW_PARAM_3DNN_MOTION) ? (AppMain.OBS_DRAW_PARAM_3DNN_MOTION)(AppMain.AMS_PARAM_DRAW_MOTION_TRS)command.param : (AppMain.OBS_DRAW_PARAM_3DNN_MOTION)command.param;
        if (((int)AppMain.g_obj.def_user_light_flag ^ (int)drawParam3DnnMotion.use_light_flag) != 0)
            AppMain.objDrawSetDrawLight(drawParam3DnnMotion.use_light_flag);
        if (drawParam3DnnMotion.user_func != null)
            drawParam3DnnMotion.user_func(drawParam3DnnMotion.user_param);
        AppMain.AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs = drawParam3DnnMotion.param;
        int nNode = paramDrawMotionTrs._object.nNode;
        int nMtxPal = paramDrawMotionTrs._object.nMtxPal;
        if (command.command_id == AppMain.OBD_DRAW_USER_COMMAND_3DNN_MOTION_MATMTN && paramDrawMotionTrs.mmotion != null)
        {
            AppMain.NNS_OBJECT mmobj = AppMain.amDrawAlloc_NNS_OBJECT();
            AppMain.nnInitMaterialMotionObject_fast(mmobj, paramDrawMotionTrs._object, paramDrawMotionTrs.mmotion);
            AppMain.nnCalcMaterialMotion(mmobj, paramDrawMotionTrs._object, paramDrawMotionTrs.mmotion, paramDrawMotionTrs.mframe);
            paramDrawMotionTrs._object = mmobj;
        }
        AppMain.OBS_DRAW_PARAM_3DNN_SORT_MODEL_Pool.Alloc();
        if (AppMain._objDraw3DNNMotion_DT.plt_mtx == null || AppMain._objDraw3DNNMotion_DT.plt_mtx.Length < nMtxPal)
        {
            AppMain._objDraw3DNNMotion_DT.plt_mtx = new AppMain.NNS_MATRIX[nMtxPal];
            for (int index = 0; index < nMtxPal; ++index)
                AppMain._objDraw3DNNMotion_DT.plt_mtx[index] = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        }
        AppMain.NNS_MATRIX[] pltMtx = AppMain._objDraw3DNNMotion_DT.plt_mtx;
        if (AppMain._objDraw3DNNMotion_DT.nstat == null || AppMain._objDraw3DNNMotion_DT.nstat.Length < nNode)
            AppMain._objDraw3DNNMotion_DT.nstat = new uint[nNode];
        uint[] nstat = AppMain._objDraw3DNNMotion_DT.nstat;
        if (paramDrawMotionTrs.mtx != null)
        {
            AppMain.nnMultiplyMatrix(ref snnsMatrix, AppMain.amMatrixGetCurrent(), paramDrawMotionTrs.mtx);
            AppMain.nnMultiplyMatrix(ref snnsMatrix, AppMain._am_draw_world_view_matrix, ref snnsMatrix);
        }
        else
            AppMain.nnMultiplyMatrix(ref snnsMatrix, AppMain._am_draw_world_view_matrix, AppMain.amMatrixGetCurrent());
        AppMain.nnSetUpNodeStatusList(nstat, nNode, 0U);
        AppMain.nnCalcMatrixPaletteTRSList(pltMtx, nstat, paramDrawMotionTrs._object, paramDrawMotionTrs.trslist, ref snnsMatrix, AppMain._am_default_stack, 1U);
        if (paramDrawMotionTrs.motion != null)
            AppMain.nnCalcNodeHideMotion((AppMain.ArrayPointer<uint>)nstat, paramDrawMotionTrs.motion, paramDrawMotionTrs.frame);
        if (drawParam3DnnMotion.mplt_cb_func != null)
            drawParam3DnnMotion.mplt_cb_func(pltMtx, paramDrawMotionTrs._object, drawParam3DnnMotion.mplt_cb_param);
        if (paramDrawMotionTrs.texlist != null)
            AppMain.nnSetTextureList(paramDrawMotionTrs.texlist);
        if (drawParam3DnnMotion.state != null)
        {
            AppMain.amDrawPushState();
            AppMain.amDrawSetState(drawParam3DnnMotion.state);
        }
        if (drawParam3DnnMotion.material_cb_func != null)
            AppMain.objDraw3DNNSetMaterialCallback(drawParam3DnnMotion.material_cb_func, drawParam3DnnMotion.material_cb_param);
        if (command.command_id == AppMain.OBD_DRAW_USER_COMMAND_3DNN_MOTION)
            AppMain.nnDrawObject(paramDrawMotionTrs._object, pltMtx, nstat, (uint)((int)paramDrawMotionTrs.sub_obj_type | 256 | 512 | 7), paramDrawMotionTrs.flag | drawflag | AppMain.amDrawGetState().drawflag, 0U);
        else
            AppMain.nnDrawMaterialMotionObject(paramDrawMotionTrs._object, pltMtx, nstat, (uint)((int)paramDrawMotionTrs.sub_obj_type | 256 | 512 | 7), paramDrawMotionTrs.flag | drawflag | AppMain.amDrawGetState().drawflag);
        if (drawParam3DnnMotion.material_cb_func != null)
            AppMain.objDraw3DNNSetMaterialCallback((AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE)null, (object)null);
        if (drawParam3DnnMotion.state != null)
            AppMain.amDrawPopState();
        if (((int)AppMain.g_obj.def_user_light_flag ^ (int)drawParam3DnnMotion.use_light_flag) != 0)
            AppMain.objDrawSetDefaultLight();
        AppMain.amMatrixPop();
    }

    public static void objDraw3DNNDrawPrimitive_DT(object param)
    {
        AppMain.OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE dnnDrawPrimitive = (AppMain.OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE)param;
        AppMain.AMS_PARAM_DRAW_PRIMITIVE dat = dnnDrawPrimitive.dat;
        AppMain.amMatrixPush();
        AppMain.nnMultiplyMatrix(ref AppMain.tempSNNS_MATRIX0, AppMain.amMatrixGetCurrent(), dnnDrawPrimitive.mtx);
        AppMain.nnMultiplyMatrix(ref AppMain.tempSNNS_MATRIX0, AppMain._am_draw_world_view_matrix, ref AppMain.tempSNNS_MATRIX0);
        AppMain.nnSetPrimitive3DMatrix(ref AppMain.tempSNNS_MATRIX0);
        AppMain.nnSetPrimitiveTexNum(dat.texlist, dat.texId);
        AppMain.nnSetPrimitiveTexState(0, 0, dat.uwrap, dat.vwrap);
        if (dat.aTest != (short)0)
            AppMain.nnSetPrimitive3DAlphaFuncGL(516U, 0.5f);
        else
            AppMain.nnSetPrimitive3DAlphaFuncGL(519U, 0.5f);
        AppMain.nnSetPrimitive3DDepthMaskGL(dat.zMask == (short)0);
        if (dat.zTest != (short)0)
            AppMain.nnSetPrimitive3DDepthFuncGL(515U);
        else
            AppMain.nnSetPrimitive3DDepthFuncGL(519U);
        if (dat.ablend != 0 && dat.bldMode == 32774)
        {
            switch (dat.bldDst)
            {
                case 1:
                    AppMain.nnSetPrimitiveBlend(0);
                    break;
                case 771:
                    AppMain.nnSetPrimitiveBlend(1);
                    break;
                default:
                    AppMain.nnSetPrimitiveBlend(1);
                    break;
            }
        }
        AppMain.nnBeginDrawPrimitive3D(dat.format3D, dat.ablend, dnnDrawPrimitive.light, dnnDrawPrimitive.cull);
        switch (dat.format3D)
        {
            case 2:
                AppMain.nnDrawPrimitive3D(dat.type, (object)dat.vtxPC3D, dat.count);
                break;
            case 4:
                AppMain.nnDrawPrimitive3D(dat.type, (object)dat.vtxPCT3D, dat.count);
                break;
        }
        AppMain.nnEndDrawPrimitive3D();
        AppMain.amMatrixPop();
    }

    private static void objDraw3DNNSetCamera_DT(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.OBS_DRAW_PARAM_3DNN_SET_CAMERA param3DnnSetCamera = (AppMain.OBS_DRAW_PARAM_3DNN_SET_CAMERA)command.param;
        if (param3DnnSetCamera.proj_type == 1)
            AppMain.amDrawSetProjection(param3DnnSetCamera.prj_mtx, 1);
        else
            AppMain.amDrawSetProjection(param3DnnSetCamera.prj_mtx, 0);
        AppMain.amDrawSetWorldViewMatrix(param3DnnSetCamera.view_mtx);
        AppMain.nnSetLightMatrix(param3DnnSetCamera.view_mtx);
        AppMain.nnPutLightSettings();
    }

    public static void objDraw3DNNUserFunc_DT(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.OBS_DRAW_PARAM_3DNN_USER_FUNC param3DnnUserFunc = (AppMain.OBS_DRAW_PARAM_3DNN_USER_FUNC)command.param;
        param3DnnUserFunc.func(param3DnnUserFunc.param);
    }

    public static void objDraw3DNNDrawMotion_DT(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.amDrawAlloc_NNS_MATRIX();
        AppMain.amMatrixPush();
        AppMain.OBS_DRAW_PARAM_3DNN_DRAW_MOTION param3DnnDrawMotion = !(command.param is AppMain.AMS_PARAM_DRAW_MOTION) ? (AppMain.OBS_DRAW_PARAM_3DNN_DRAW_MOTION)command.param : (AppMain.OBS_DRAW_PARAM_3DNN_DRAW_MOTION)(AppMain.AMS_PARAM_DRAW_MOTION)command.param;
        if (((int)AppMain.g_obj.def_user_light_flag ^ (int)param3DnnDrawMotion.use_light_flag) != 0)
            AppMain.objDrawSetDrawLight(param3DnnDrawMotion.use_light_flag);
        if (param3DnnDrawMotion.user_func != null)
            param3DnnDrawMotion.user_func(param3DnnDrawMotion.user_param);
        AppMain.AMS_PARAM_DRAW_MOTION amsParamDrawMotion = param3DnnDrawMotion.param;
        int nNode = amsParamDrawMotion._object.nNode;
        if (AppMain._objDraw3DNNDrawMotion_DT.plt_mtx == null || AppMain._objDraw3DNNDrawMotion_DT.plt_mtx.Length < nNode)
        {
            AppMain._objDraw3DNNDrawMotion_DT.plt_mtx = new AppMain.NNS_MATRIX[nNode];
            AppMain._objDraw3DNNDrawMotion_DT.nstat = new uint[nNode];
            for (int index = 0; index < nNode; ++index)
                AppMain._objDraw3DNNDrawMotion_DT.plt_mtx[index] = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        }
        AppMain.NNS_MATRIX[] pltMtx = AppMain._objDraw3DNNDrawMotion_DT.plt_mtx;
        uint[] nstat = AppMain._objDraw3DNNDrawMotion_DT.nstat;
        if (amsParamDrawMotion.mtx != null)
        {
            AppMain.nnMultiplyMatrix(nnsMatrix, AppMain.amMatrixGetCurrent(), amsParamDrawMotion.mtx);
            AppMain.nnMultiplyMatrix(nnsMatrix, AppMain._am_draw_world_view_matrix, nnsMatrix);
        }
        else
            AppMain.nnMultiplyMatrix(nnsMatrix, AppMain._am_draw_world_view_matrix, AppMain.amMatrixGetCurrent());
        AppMain.nnSetUpNodeStatusList(nstat, nNode, 0U);
        AppMain.nnCalcMatrixPaletteMotion(pltMtx, nstat, amsParamDrawMotion._object, amsParamDrawMotion.motion, amsParamDrawMotion.frame, nnsMatrix, AppMain._am_default_stack, 1U);
        AppMain.nnCalcNodeHideMotion((AppMain.ArrayPointer<uint>)nstat, amsParamDrawMotion.motion, amsParamDrawMotion.frame);
        if (param3DnnDrawMotion.mplt_cb_func != null)
            param3DnnDrawMotion.mplt_cb_func(pltMtx, amsParamDrawMotion._object, param3DnnDrawMotion.mplt_cb_param);
        if (amsParamDrawMotion.texlist != null)
            AppMain.nnSetTextureList(amsParamDrawMotion.texlist);
        if (param3DnnDrawMotion.state != null)
        {
            AppMain.amDrawPushState();
            AppMain.amDrawSetState(param3DnnDrawMotion.state);
        }
        if (param3DnnDrawMotion.material_cb_func != null)
            AppMain.objDraw3DNNSetMaterialCallback(param3DnnDrawMotion.material_cb_func, param3DnnDrawMotion.material_cb_param);
        uint alternativeLighting = param3DnnDrawMotion.use_light_flag & 98304U;
        if (command.command_id == AppMain.OBD_DRAW_USER_COMMAND_3DNN_DRAW_MOTION)
            AppMain.nnDrawObject(amsParamDrawMotion._object, pltMtx, nstat, (uint)((int)amsParamDrawMotion.sub_obj_type | 256 | 512 | 7), amsParamDrawMotion.flag | drawflag | AppMain.amDrawGetState().drawflag, alternativeLighting);
        else
            AppMain.nnDrawMaterialMotionObject(amsParamDrawMotion._object, pltMtx, nstat, (uint)((int)amsParamDrawMotion.sub_obj_type | 256 | 512 | 7), amsParamDrawMotion.flag | drawflag | AppMain.amDrawGetState().drawflag);
        if (param3DnnDrawMotion.material_cb_func != null)
            AppMain.objDraw3DNNSetMaterialCallback((AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE)null, (object)null);
        if (param3DnnDrawMotion.state != null)
            AppMain.amDrawPopState();
        if (((int)AppMain.g_obj.def_user_light_flag ^ (int)param3DnnDrawMotion.use_light_flag) != 0)
            AppMain.objDrawSetDefaultLight();
        AppMain.amMatrixPop();
    }

    private static void objDraw3DNNModelCommandSortFunc(
      AppMain.AMS_COMMAND_HEADER command,
      uint drawflag)
    {
        AppMain.obj_draw_user_command_sort_func_tbl[command.command_id](command, drawflag);
    }

    private static void objDraw3DNNSortModel_DT(AppMain.AMS_COMMAND_HEADER command, uint drawflag)
    {
        AppMain.amMatrixPush();
        AppMain.OBS_DRAW_PARAM_3DNN_SORT_MODEL param3DnnSortModel = (AppMain.OBS_DRAW_PARAM_3DNN_SORT_MODEL)command.param;
        if (((int)AppMain.g_obj.def_user_light_flag ^ (int)param3DnnSortModel.use_light_flag) != 0)
            AppMain.objDrawSetDrawLight(param3DnnSortModel.use_light_flag);
        if (param3DnnSortModel.user_func != null)
            param3DnnSortModel.user_func(param3DnnSortModel.user_param);
        AppMain.AMS_PARAM_SORT_DRAW_OBJECT paramSortDrawObject = param3DnnSortModel.param;
        AppMain.AMS_PARAM_DRAW_OBJECT drawObject = paramSortDrawObject.draw_object;
        if (drawObject.texlist != null)
            AppMain.nnSetTextureList(drawObject.texlist);
        if (paramSortDrawObject.draw_state != null)
        {
            AppMain.amDrawPushState();
            AppMain.amDrawSetState(paramSortDrawObject.draw_state);
        }
        AppMain.objDraw3DNNSetMaterialCallback(param3DnnSortModel.material_cb_func, param3DnnSortModel.material_cb_param);
        if (command.command_id == AppMain.OBD_DRAW_USER_COMMAND_SORT_3DNN_MODEL)
            AppMain.nnDrawObject(drawObject._object, paramSortDrawObject.mtx, paramSortDrawObject.nstat_list, (uint)((int)drawObject.sub_obj_type | 256 | 512 | 2), drawObject.flag | paramSortDrawObject.drawflag | AppMain.amDrawGetState().drawflag, 0U);
        else
            AppMain.nnDrawMaterialMotionObject(drawObject._object, paramSortDrawObject.mtx, paramSortDrawObject.nstat_list, (uint)((int)drawObject.sub_obj_type | 256 | 512 | 2), drawObject.flag | paramSortDrawObject.drawflag | AppMain.amDrawGetState().drawflag);
        AppMain.objDraw3DNNSetMaterialCallback((AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE)null, (object)null);
        if (paramSortDrawObject.draw_state != null)
            AppMain.amDrawPopState();
        if (((int)AppMain.g_obj.def_user_light_flag ^ (int)param3DnnSortModel.use_light_flag) != 0)
            AppMain.objDrawSetDefaultLight();
        AppMain.amMatrixPop();
    }

    private static void objDraw3DNNSetMaterialCallback(
      AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE cb_func,
      object cb_param)
    {
        AppMain.obj_draw_material_cb_func = cb_func;
        AppMain.obj_draw_material_cb_param = cb_param;
        if (cb_func != null)
            AppMain.nnSetMaterialCallback(AppMain._objDraw3DNNMaterialCallback);
        else
            AppMain.nnSetMaterialCallback((AppMain.NNS_MATERIALCALLBACK_FUNC)null);
    }

    public static int objDraw3DNNMaterialCallback(AppMain.NNS_DRAWCALLBACK_VAL draw_cb_val)
    {
        if (AppMain.obj_draw_material_cb_func == null)
            return AppMain.nnPutMaterialCore(draw_cb_val);
        return !AppMain.obj_draw_material_cb_func(draw_cb_val, AppMain.obj_draw_material_cb_param) ? 0 : 1;
    }

    public static void objDrawSetDrawLight(uint use_light_flag)
    {
        for (int no = 0; no < AppMain.NNE_LIGHT_MAX; ++no)
        {
            if (((long)use_light_flag & (long)(1 << no)) != 0L)
            {
                AppMain.nnSetLight(no, (object)AppMain.g_obj.light[no].light_param, AppMain.g_obj.light[no].light_type);
                AppMain.nnSetLightSwitch(no, 1);
            }
            else
                AppMain.nnSetLightSwitch(no, 0);
        }
        AppMain.nnPutLightSettings();
    }

    public static void objDrawSetDefaultLight()
    {
        for (int no = 0; no < AppMain.NNE_LIGHT_MAX; ++no)
        {
            if (((long)AppMain.g_obj.def_user_light_flag & (long)(1 << no)) != 0L)
            {
                AppMain.nnSetLight(no, (object)AppMain.g_obj.light[no].light_param, AppMain.g_obj.light[no].light_type);
                AppMain.nnSetLightSwitch(no, 1);
            }
            else
                AppMain.nnSetLightSwitch(no, 0);
        }
        AppMain.nnPutLightSettings();
    }

    public static void objDraw3DESEffectServerMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.amEffectExecute();
    }

    public static void objDraw3DESMatrixPush_UserFunc(object param)
    {
        AppMain.amMatrixPush();
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.nnMultiplyMatrix(current, current, (AppMain.NNS_MATRIX)param);
        AppMain.nnMultiplyMatrix(ref AppMain.tempSNNS_MATRIX0, AppMain.amDrawGetWorldViewMatrix(), current);
        AppMain.nnSetPrimitive3DMatrix(ref AppMain.tempSNNS_MATRIX0);
    }

    public static void objDraw3DESMatrixPop_UserFunc(object param)
    {
        AppMain.amMatrixPop();
    }

    public static void objDraw2DAMAPre_DT(object param)
    {
        AppMain.AoActDrawPre();
    }

}