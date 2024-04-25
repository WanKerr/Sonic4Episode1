public partial class AppMain
{
    public static void ObjDrawObjectSetToon(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawSetToon(obj_work.obj_3d);
    }

    public static void objDraw3DNNModelCommandFunc(AMS_COMMAND_HEADER command, uint drawflag)
    {
        obj_draw_user_command_func_tbl[command.command_id](command, drawflag);
    }

    public static void objDrawResetCache()
    {
        _objDraw3DNNSetCamera_Pool.ReleaseUsedObjects();
        _ObjDraw3DNNModel_Pool.ReleaseUsedObjects();
        OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE_Pool.ReleaseUsedObjects();
        OBS_DRAW_PARAM_3DNN_SORT_MODEL_Pool.ReleaseUsedObjects();
    }

    public static void ObjDrawInit()
    {
        amDrawSetDrawCommandFunc(new _am_draw_command_delegate(objDraw3DNNModelCommandFunc), new _am_draw_command_delegate(objDraw3DNNModelCommandSortFunc));
        ObjDrawClearNNCommandStateTbl();
        g_obj_draw_3dnn_draw_state.drawflag = 0U;
        g_obj_draw_3dnn_draw_state.diffuse.mode = 3;
        g_obj_draw_3dnn_draw_state.diffuse.r = 1f;
        g_obj_draw_3dnn_draw_state.diffuse.g = 1f;
        g_obj_draw_3dnn_draw_state.diffuse.b = 1f;
        g_obj_draw_3dnn_draw_state.ambient.mode = 3;
        g_obj_draw_3dnn_draw_state.ambient.r = 1f;
        g_obj_draw_3dnn_draw_state.ambient.g = 1f;
        g_obj_draw_3dnn_draw_state.ambient.b = 1f;
        g_obj_draw_3dnn_draw_state.alpha.mode = 3;
        g_obj_draw_3dnn_draw_state.alpha.alpha = 1f;
        g_obj_draw_3dnn_draw_state.specular.mode = 3;
        g_obj_draw_3dnn_draw_state.specular.r = 1f;
        g_obj_draw_3dnn_draw_state.specular.g = 1f;
        g_obj_draw_3dnn_draw_state.specular.b = 1f;
        g_obj_draw_3dnn_draw_state.blend.mode = 0;
        g_obj_draw_3dnn_draw_state.envmap.texsrc = 1;
        g_obj_draw_3dnn_draw_state.zmode.compare = 1U;
        g_obj_draw_3dnn_draw_state.zmode.func = 515;
        g_obj_draw_3dnn_draw_state.zmode.update = 1U;
        NNS_MATRIX texmtx = g_obj_draw_3dnn_draw_state.envmap.texmtx;
        nnMakeUnitMatrix(texmtx);
        nnTranslateMatrix(texmtx, texmtx, 0.5f, 0.5f, 0.0f);
        nnScaleMatrix(texmtx, texmtx, 0.5f, 0.5f, 0.0f);
        for (int index = 0; index < 4; ++index)
        {
            g_obj_draw_3dnn_draw_state.texoffset[index].mode = 2;
            g_obj_draw_3dnn_draw_state.texoffset[index].u = 0.0f;
            g_obj_draw_3dnn_draw_state.texoffset[index].v = 0.0f;
        }
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_VECTOR nnsVector = new NNS_VECTOR(-1f, -1f, -1f);
        g_obj.def_user_light_flag = 1U;
        g_obj.ambient_color.r = 0.8f;
        g_obj.ambient_color.g = 0.8f;
        g_obj.ambient_color.b = 0.8f;
        nnNormalizeVector(nnsVector, nnsVector);
        int light_no = 0;
        OBS_LIGHT obsLight = g_obj.light[0];
        for (; light_no < NNE_LIGHT_MAX; ++light_no)
            ObjDrawSetParallelLight(light_no, ref col, 1f, nnsVector);
        AoActSysSetDrawStateEnable(true);
        AoActSysSetDrawState(6U);
        AoActSysSetDrawTaskPrio(8192U);
    }

    public static void ObjDrawESEffectSystemInit(ushort pause_level, uint task_prio, uint group)
    {
        amEffectSystemInit();
        obj_draw_effect_server_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(objDraw3DESEffectServerMain), null, 0U, pause_level, task_prio, (int)group, null, "ES_EFFECT_SERVER");
    }

    public static bool ObjDrawESEffectSystemIsActive()
    {
        return obj_draw_effect_server_tcb != null;
    }

    public static void ObjDrawExit()
    {
        nnSetMaterialCallback(null);
        AoActSysSetDrawStateEnable(false);
    }

    public static void ObjDrawESEffectSystemExit()
    {
        mtTaskClearTcb(obj_draw_effect_server_tcb);
        obj_draw_effect_server_tcb = null;
    }

    public static void ObjDrawPrioritySet(OBS_OBJECT_WORK obj_work, uint prio)
    {
    }

    public static void ObjDrawObjectActionSet(OBS_OBJECT_WORK obj_work, int id)
    {
        obj_work.disp_flag &= 4294967287U;
        obj_work.disp_flag &= 4294967291U;
        if (obj_work.obj_3d == null || obj_work.obj_3d.motion == null)
            return;
        obj_work.obj_3d.act_id[0] = id;
        amMotionSet(obj_work.obj_3d.motion, 0, id);
        obj_work.obj_3d.frame[0] = 0.0f;
    }

    public static void ObjDrawObjectActionSet3DNN(
      OBS_OBJECT_WORK obj_work,
      int id,
      int mbuf_id)
    {
        obj_work.disp_flag &= 4294967287U;
        obj_work.disp_flag &= 4294967291U;
        ObjDrawAction3dActionSet3DNN(obj_work.obj_3d, id, mbuf_id);
    }

    public static void ObjDrawAction3dActionSet3DNN(
      OBS_ACTION3D_NN_WORK obj_3d,
      int id,
      int mbuf_id)
    {
        if ((uint)mbuf_id >= 2U)
            return;
        obj_3d.act_id[mbuf_id] = id;
        amMotionSet(obj_3d.motion, mbuf_id, id);
        obj_3d.frame[mbuf_id] = 0.0f;
    }

    public static void ObjDrawObjectActionSet3DNNBlend(OBS_OBJECT_WORK obj_work, int id)
    {
        obj_work.disp_flag &= 4294967287U;
        obj_work.disp_flag &= 4294967291U;
        ObjDrawAction3dActionSet3DNNBlend(obj_work.obj_3d, id);
    }

    public static void ObjDrawAction3dActionSet3DNNBlend(OBS_ACTION3D_NN_WORK obj_3d, int id)
    {
        ObjDrawAction3dActionSet3DNN(obj_3d, obj_3d.act_id[0], 1);
        obj_3d.frame[1] = obj_3d.frame[0];
        ObjDrawAction3dActionSet3DNN(obj_3d, id, 0);
        obj_3d.marge = 1f;
        obj_3d.flag |= 1U;
    }

    public static void ObjDrawObjectActionSet3DNNMaterial(OBS_OBJECT_WORK obj_work, int id)
    {
        obj_work.disp_flag &= 4294967287U;
        obj_work.disp_flag &= 4294967291U;
        ObjDrawAction3dActionSet3DNNMaterial(obj_work.obj_3d, id);
    }

    public static void ObjDrawAction3dActionSet3DNNMaterial(
      OBS_ACTION3D_NN_WORK obj_3d,
      int id)
    {
        obj_3d.mat_act_id = id;
        amMotionMaterialSet(obj_3d.motion, id);
        obj_3d.mat_frame = 0.0f;
    }

    public static int ObjDrawActionGet(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_3d != null && obj_work.obj_3d.motion != null)
            return obj_work.obj_3d.act_id[0];
        return obj_work.obj_2d != null ? (int)obj_work.obj_2d.act_id : 0;
    }

    public static int ObjDrawActionGet3DNN(OBS_OBJECT_WORK obj_work, int mbuf_id)
    {
        return mbuf_id >= 2 || obj_work.obj_3d == null || obj_work.obj_3d.motion == null ? 0 : obj_work.obj_3d.act_id[mbuf_id];
    }

    public static void ObjDrawActionSummary(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_3d != null && ObjAction3dNNModelLoadCheck(obj_work.obj_3d))
            ObjDrawObjectAction3DNN(obj_work, obj_work.obj_3d);
        if (obj_work.obj_3des != null && ObjAction3dESEffectLoadCheck(obj_work.obj_3des))
            ObjDrawObjectAction3DES(obj_work, obj_work.obj_3des);
        if (obj_work.obj_2d == null || !ObjAction2dAMALoadCheck(obj_work.obj_2d))
            return;
        ObjDrawObjectAction2DAMA(obj_work, obj_work.obj_2d);
    }

    public static void ObjDrawClearNNCommandStateTbl()
    {
        for (int index = 0; index < 18; ++index)
        {
            obj_draw_3dnn_command_state_tbl[index] = uint.MaxValue;
            obj_draw_3dnn_command_state_exe_end_scene_tbl[index] = false;
        }
        obj_draw_3dnn_command_state_tbl[0] = 0U;
        obj_draw_3dnn_command_state_exe_end_scene_tbl[0] = true;
    }

    public static void ObjDrawSetNNCommandStateTbl(uint tbl_no, uint command_state, bool end_scene)
    {
        obj_draw_3dnn_command_state_tbl[(int)tbl_no] = command_state;
        obj_draw_3dnn_command_state_exe_end_scene_tbl[(int)tbl_no] = end_scene;
    }

    public static void ObjDrawNNStart()
    {
        amDrawMakeTask(_objDrawStart_DT, 4096, null);
    }

    public static void ObjDraw3DNNSetCamera(int camera_id, int proj_type)
    {
        OBS_CAMERA obj_camera = null;
        if (!GmMainIsDrawEnable())
            return;
        if (camera_id >= 0)
            obj_camera = ObjCameraGet(camera_id);
        objDraw3DNNSetCamera(obj_camera, proj_type, obj_camera.command_state);
    }

    public static void ObjDraw3DNNSetCameraEx(int camera_id, int proj_type, uint command_state)
    {
        OBS_CAMERA obj_camera = null;
        if (!GmMainIsDrawEnable())
            return;
        if (camera_id >= 0)
            obj_camera = ObjCameraGet(camera_id);
        objDraw3DNNSetCamera(obj_camera, proj_type, command_state);
    }

    public static void ObjDraw3DNNUserFunc(
      OBF_DRAW_USER_DT_FUNC user_func,
      object param,
      int param_size,
      uint command_state)
    {
        _ObjDraw3DNNUserFunc(user_func, null, command_state);
    }

    public static void _ObjDraw3DNNUserFunc(
      OBF_DRAW_USER_DT_FUNC user_func,
      object param,
      uint command_state)
    {
        if (!GmMainIsDrawEnable())
            return;
        OBS_DRAW_PARAM_3DNN_USER_FUNC param3DnnUserFunc = amDrawAlloc_OBS_DRAW_PARAM_3DNN_USER_FUNC();
        param3DnnUserFunc.param = param;
        param3DnnUserFunc.func = user_func;
        amDrawRegistCommand(command_state, OBD_DRAW_USER_COMMAND_3DNN_USER_FUNC, param3DnnUserFunc);
    }

    public static void ObjDrawObjectAction3DNN(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION3D_NN_WORK obj_3d)
    {
        VecFx32 pos = obj_work.pos;
        VecU16 vecU16 = new VecU16(obj_work.dir);
        pos.x += obj_work.ofst.x;
        pos.y += obj_work.ofst.y;
        pos.z += obj_work.ofst.z;
        if (obj_work.dir_fall != 0)
            vecU16.z += obj_work.dir_fall;
        ObjDrawAction3DNN(obj_3d, new VecFx32?(pos), new AppMain.VecU16?(vecU16), obj_work.scale, ref obj_work.disp_flag);
    }

    public static void ObjDrawAction3DNN(
      OBS_ACTION3D_NN_WORK obj_3d,
      VecFx32? pos,
      VecU16? dir,
      VecFx32 scale,
      ref uint p_disp_flag)
    {
        uint? p_disp_flag1 = new uint?(p_disp_flag);
        ObjDrawAction3DNN(obj_3d, pos, dir, new VecFx32?(scale), ref p_disp_flag1);
        p_disp_flag = p_disp_flag1.Value;
    }

    public static void ObjDrawAction3DNN(
      OBS_ACTION3D_NN_WORK obj_3d,
      VecFx32? pos,
      VecU16? dir,
      VecFx32? scale,
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
                OBS_CAMERA obsCamera = ObjCameraGet(_g_obj.glb_camera_id);
                float num1 = obsCamera.disp_pos.x + obsCamera.bottom;
                float num2 = obsCamera.disp_pos.x + obsCamera.top;
                float num3 = (float)-(obsCamera.disp_pos.y + (double)obsCamera.top);
                float num4 = (float)-(obsCamera.disp_pos.y + (double)obsCamera.bottom);
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
                    num5 = FX_FX32_TO_F32(x);
                }
                float num6 = num5 * 3.2f;
                float num7 = FX_FX32_TO_F32(pos.Value.x) - obj_3d._object.Radius * 1.2f * num6;
                float num8 = FX_FX32_TO_F32(pos.Value.x) + obj_3d._object.Radius * 1.2f * num6;
                float num9 = FX_FX32_TO_F32(pos.Value.y) - obj_3d._object.Radius * 1.2f * num6;
                float num10 = FX_FX32_TO_F32(pos.Value.y) + obj_3d._object.Radius * 1.2f * num6;
                if (num1 > (double)num8)
                    flag = true;
                if (num2 < (double)num7)
                    flag = true;
                if (num3 > (double)num10)
                    flag = true;
                if (num4 < (double)num9)
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
            NNS_MATRIX action3DnnObjMtx = ObjDrawAction3DNN_obj_mtx;
            VecFx32 vecFx32_1 = new VecFx32(4096, 4096, 4096);
            nnMakeUnitMatrix(action3DnnObjMtx);
            if (pos.HasValue && ((int)p_disp_flag1 & 8192) == 0)
            {
                VecFx32 vecFx32_2 = pos.Value;
                if (((int)p_disp_flag1 & 2097152) == 0)
                    vecFx32_2.y = -vecFx32_2.y;
                if (((int)p_disp_flag1 & 524288) == 0)
                {
                    if (_g_obj.glb_scale.x != 4096)
                        vecFx32_2.x = FX_Mul(vecFx32_2.x, _g_obj.glb_scale.x);
                    if (_g_obj.glb_scale.y != 4096)
                        vecFx32_2.y = FX_Mul(vecFx32_2.y, _g_obj.glb_scale.y);
                    if (_g_obj.glb_scale.z != 4096)
                        vecFx32_2.z = FX_Mul(vecFx32_2.z, _g_obj.glb_scale.z);
                }
                nnTranslateMatrix(action3DnnObjMtx, action3DnnObjMtx, FX_FX32_TO_F32(vecFx32_2.x), FX_FX32_TO_F32(vecFx32_2.y), FX_FX32_TO_F32(vecFx32_2.z));
            }
            if (dir.HasValue && ((int)p_disp_flag1 & 256) == 0)
            {
                if (((int)p_disp_flag1 & 2097152) == 0)
                    nnRotateXYZMatrix(action3DnnObjMtx, action3DnnObjMtx, -dir.Value.x, dir.Value.y, -dir.Value.z);
                else
                    nnRotateXYZMatrix(action3DnnObjMtx, action3DnnObjMtx, dir.Value.x, dir.Value.y, dir.Value.z);
            }
            if (((int)p_disp_flag1 & 4194304) == 0)
            {
                if (((int)p_disp_flag1 & 67108864) == 0)
                {
                    if (((int)p_disp_flag1 & 1) != 0)
                        nnRotateYMatrix(action3DnnObjMtx, action3DnnObjMtx, 49152);
                    else
                        nnRotateYMatrix(action3DnnObjMtx, action3DnnObjMtx, 16384);
                }
                else
                {
                    if (((int)p_disp_flag1 & 2) != 0)
                        nnRotateXMatrix(action3DnnObjMtx, action3DnnObjMtx, 32768);
                    if (((int)p_disp_flag1 & 1) != 0)
                        nnRotateYMatrix(action3DnnObjMtx, action3DnnObjMtx, 32768);
                }
            }
            if (scale.HasValue && ((int)p_disp_flag1 & 65536) == 0)
                vecFx32_1 = scale.Value;
            if (((int)p_disp_flag1 & 1048576) == 0)
            {
                vecFx32_1.x = FX_Mul(vecFx32_1.x, _g_obj.draw_scale.x);
                vecFx32_1.y = FX_Mul(vecFx32_1.y, _g_obj.draw_scale.y);
                vecFx32_1.z = FX_Mul(vecFx32_1.z, _g_obj.draw_scale.z);
            }
            if (((int)p_disp_flag1 & 524288) == 0)
            {
                vecFx32_1.x = FX_Mul(vecFx32_1.x, _g_obj.glb_scale.x);
                vecFx32_1.y = FX_Mul(vecFx32_1.y, _g_obj.glb_scale.y);
                vecFx32_1.z = FX_Mul(vecFx32_1.z, _g_obj.glb_scale.z);
            }
            nnScaleMatrix(action3DnnObjMtx, action3DnnObjMtx, FX_FX32_TO_F32(vecFx32_1.x), FX_FX32_TO_F32(vecFx32_1.y), FX_FX32_TO_F32(vecFx32_1.z));
            if (((int)p_disp_flag1 & 8388608) != 0)
                nnMultiplyMatrix(action3DnnObjMtx, obj_3d.user_obj_mtx, action3DnnObjMtx);
            if (((int)p_disp_flag1 & 16777216) != 0)
                nnMultiplyMatrix(action3DnnObjMtx, action3DnnObjMtx, obj_3d.user_obj_mtx_r);
            amMatrixPush(action3DnnObjMtx);
            if (obj_3d.motion != null && obj_3d.motion.mmobject != null)
                ObjDrawAction3DNNMaterialUpdate(obj_3d, ref p_disp_flag1);
            if (obj_3d.motion != null && obj_3d.motion.mtnbuf[0] != null)
            {
                ObjDrawAction3DNNMotionUpdate(obj_3d, ref p_disp_flag1);
                if (obj_3d.mtn_cb_func != null)
                    obj_3d.mtn_cb_func(obj_3d.motion, obj_3d._object, obj_3d.mtn_cb_param);
                if (((int)p_disp_flag1 & 32) == 0)
                {
                    AMS_DRAWSTATE draw_state = null;
                    if (((int)p_disp_flag1 & 134217728) != 0)
                        draw_state = obj_3d.draw_state;
                    if (obj_3d.marge == 0.0 || obj_3d.marge == 1.0)
                    {
                        int motion_id;
                        float num;
                        if (obj_3d.marge == 0.0)
                        {
                            motion_id = obj_3d.act_id[0];
                            num = obj_3d.frame[0];
                        }
                        else
                        {
                            motion_id = obj_3d.act_id[1];
                            num = obj_3d.frame[1];
                        }
                        NNS_MOTION motion = obj_3d.motion.mtnbuf[motion_id & ushort.MaxValue];
                        float frame = num + amMotionGetStartFrame(obj_3d.motion, motion_id);
                        if (obj_3d.motion != null && obj_3d.motion.mmobject != null)
                            ObjDraw3DNNMotionMaterialMotion(obj_3d.motion, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, obj_3d.mplt_cb_func, obj_3d.mplt_cb_param, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                        else
                            ObjDraw3DNNDrawMotion(motion, frame, obj_3d._object, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, obj_3d.mplt_cb_func, obj_3d.mplt_cb_param, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                    }
                    else if (obj_3d.motion != null && obj_3d.motion.mmobject != null)
                        ObjDraw3DNNMotionMaterialMotion(obj_3d.motion, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, obj_3d.mplt_cb_func, obj_3d.mplt_cb_param, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                    else
                        ObjDraw3DNNMotion(obj_3d.motion, obj_3d.motion._object, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, obj_3d.mplt_cb_func, obj_3d.mplt_cb_param, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                }
                obj_3d.user_param = null;
                obj_3d.mplt_cb_param = null;
                obj_3d.material_cb_param = null;
            }
            else
            {
                if (((int)p_disp_flag1 & 32) == 0)
                {
                    AMS_DRAWSTATE draw_state = null;
                    if (((int)p_disp_flag1 & 134217728) != 0)
                        draw_state = obj_3d.draw_state;
                    if (obj_3d.motion != null && obj_3d.motion.mmobject != null)
                        ObjDraw3DNNMotionMaterialMotion(obj_3d.motion, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, null, null, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                    else
                        ObjDraw3DNNModel(obj_3d._object, obj_3d.texlist, obj_3d.drawflag, obj_3d.sub_obj_type, obj_3d.user_func, obj_3d.user_param, obj_3d.command_state, obj_3d.material_cb_func, obj_3d.material_cb_param, draw_state, obj_3d.use_light_flag);
                }
                obj_3d.user_param = null;
                obj_3d.mplt_cb_param = null;
                obj_3d.material_cb_param = null;
            }
            amMatrixPop();
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
      OBS_ACTION3D_NN_WORK obj_3d,
      ref uint p_disp_flag)
    {
        uint? p_disp_flag1 = new uint?(p_disp_flag);
        ObjDrawAction3DNNMotionUpdate(obj_3d, ref p_disp_flag1);
        p_disp_flag = p_disp_flag1.Value;
    }

    public static void ObjDrawAction3DNNMotionUpdate(
      OBS_ACTION3D_NN_WORK obj_3d,
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
                    if (obj_3d.marge <= 0.0)
                    {
                        obj_3d.marge = 0.0f;
                        obj_3d.flag &= 4294967294U;
                    }
                }
                if (((int)num1 & 4112) == 0)
                {
                    num2 = obj_3d.speed[0] * FX_FX32_TO_F32(g_obj.speed);
                    num3 = obj_3d.speed[1] * FX_FX32_TO_F32(g_obj.speed);
                }
                if (obj_3d.marge < 1.0 || ((int)obj_3d.flag & 3) != 0)
                    obj_3d.frame[0] += num2;
                if (((int)obj_3d.flag & 1) == 0 && (obj_3d.marge > 0.0 || ((int)obj_3d.flag & 3) != 0))
                    obj_3d.frame[1] += num3;
                if (((int)num1 & 4) != 0)
                {
                    for (int index = 0; index < 2; ++index)
                    {
                        float num4 = amMotionGetEndFrame(obj_3d.motion, obj_3d.act_id[index]) - amMotionGetStartFrame(obj_3d.motion, obj_3d.act_id[index]);
                        while (obj_3d.frame[index] >= (double)num4)
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
                        float num4 = amMotionGetEndFrame(obj_3d.motion, obj_3d.act_id[index]) - amMotionGetStartFrame(obj_3d.motion, obj_3d.act_id[index]);
                        if (obj_3d.frame[index] >= num4 - 1.0)
                        {
                            obj_3d.frame[index] = num4 - 1f;
                            if (index == 0)
                                num1 |= 8U;
                        }
                    }
                }
            }
            if (obj_3d.marge < 1.0 || ((int)obj_3d.flag & 3) != 0)
                amMotionSetFrame(obj_3d.motion, 0, obj_3d.frame[0] + amMotionGetStartFrame(obj_3d.motion, obj_3d.act_id[0]));
            if (obj_3d.marge > 0.0 || ((int)obj_3d.flag & 3) != 0)
                amMotionSetFrame(obj_3d.motion, 1, obj_3d.frame[1] + amMotionGetStartFrame(obj_3d.motion, obj_3d.act_id[1]));
            amMotionGet(obj_3d.motion, obj_3d.marge, obj_3d.per);
        }
        if (!p_disp_flag.HasValue)
            return;
        ref uint? local = ref p_disp_flag;
        uint? nullable = local;
        uint num5 = num1 & 8U;
        local = nullable.HasValue ? new uint?(nullable.GetValueOrDefault() | num5) : new uint?();
    }

    public static void ObjDrawAction3DNNMaterialUpdate(
      OBS_ACTION3D_NN_WORK obj_3d,
      ref uint p_disp_flag)
    {
        uint? p_disp_flag1 = new uint?(p_disp_flag);
        ObjDrawAction3DNNMaterialUpdate(obj_3d, ref p_disp_flag1);
        p_disp_flag = p_disp_flag1.Value;
    }

    public static void ObjDrawAction3DNNMaterialUpdate(
      OBS_ACTION3D_NN_WORK obj_3d,
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
                    num2 = obj_3d.mat_speed * FX_FX32_TO_F32(g_obj.speed);
                obj_3d.mat_frame += num2;
                if (((int)num1 & 4) != 0)
                {
                    float num3 = amMotionMaterialGetEndFrame(obj_3d.motion, obj_3d.mat_act_id) - amMotionMaterialGetStartFrame(obj_3d.motion, obj_3d.mat_act_id);
                    while (obj_3d.mat_frame >= (double)num3)
                    {
                        obj_3d.mat_frame -= num3;
                        num1 |= 33554432U;
                    }
                }
                else
                {
                    float num3 = amMotionMaterialGetEndFrame(obj_3d.motion, obj_3d.mat_act_id) - amMotionMaterialGetStartFrame(obj_3d.motion, obj_3d.mat_act_id);
                    if (obj_3d.mat_frame >= num3 - 1.0)
                    {
                        obj_3d.mat_frame = num3 - 1f;
                        num1 |= 33554432U;
                    }
                }
            }
            amMotionMaterialSetFrame(obj_3d.motion, obj_3d.mat_frame + amMotionMaterialGetStartFrame(obj_3d.motion, obj_3d.mat_act_id));
            amMotionMaterialCalc(obj_3d.motion);
        }
        if (!p_disp_flag.HasValue)
            return;
        ref uint? local = ref p_disp_flag;
        uint? nullable = local;
        uint num4 = num1 & 33554432U;
        local = nullable.HasValue ? new uint?(nullable.GetValueOrDefault() | num4) : new uint?();
    }

    public static void ObjDraw3DNNModel(
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!GmMainIsDrawEnable())
            return;
        OBS_DRAW_PARAM_3DNN_MODEL drawParam3DnnModel = _ObjDraw3DNNModel_Pool.Alloc();
        NNS_MATRIX mtx = drawParam3DnnModel.mtx;
        nnCopyMatrix(mtx, amMatrixGetCurrent());
        AMS_PARAM_DRAW_OBJECT amsParamDrawObject = drawParam3DnnModel.param;
        amsParamDrawObject._object = _object;
        if (gmMapTransX != 0.0)
            nnScaleMatrix(mtx, mtx, 1.005f, 1.005f, 1f);
        amsParamDrawObject.mtx = mtx;
        amsParamDrawObject.sub_obj_type = sub_obj_type;
        amsParamDrawObject.flag = drawflag;
        amsParamDrawObject.texlist = texlist;
        amsParamDrawObject.material_func = null;
        amsParamDrawObject.scaleZ = 1f;
        drawParam3DnnModel.state = null;
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
        amDrawRegistCommand(command_state, OBD_DRAW_USER_COMMAND_3DNN_MODEL, amsParamDrawObject);
    }

    public static void ObjDraw3DNNModelMaterialMotion(
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!GmMainIsDrawEnable())
            return;
        OBS_DRAW_PARAM_3DNN_MODEL drawParam3DnnModel = new OBS_DRAW_PARAM_3DNN_MODEL();
        NNS_MATRIX mtx = drawParam3DnnModel.mtx;
        nnCopyMatrix(mtx, amMatrixGetCurrent());
        AMS_PARAM_DRAW_OBJECT amsParamDrawObject = drawParam3DnnModel.param;
        amsParamDrawObject._object = _object;
        amsParamDrawObject.mtx = mtx;
        amsParamDrawObject.sub_obj_type = sub_obj_type;
        amsParamDrawObject.flag = drawflag;
        amsParamDrawObject.texlist = texlist;
        amsParamDrawObject.material_func = null;
        amsParamDrawObject.scaleZ = 1f;
        drawParam3DnnModel.state = null;
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
        amDrawRegistCommand(command_state, OBD_DRAW_USER_COMMAND_3DNN_MODEL_MATMTN, amsParamDrawObject);
    }

    public static void ObjDraw3DNNMotion(
      AMS_MOTION motion,
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT mplt_cb_func,
      object mplt_cb_param,
      MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!GmMainIsDrawEnable())
            return;
        int nodeNum = motion.node_num;
        OBS_DRAW_PARAM_3DNN_MOTION drawParam3DnnMotion = amDrawAlloc_OBS_DRAW_PARAM_3DNN_MOTION();
        NNS_MATRIX mtx = drawParam3DnnMotion.mtx;
        nnCopyMatrix(mtx, amMatrixGetCurrent());
        AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs = drawParam3DnnMotion.param;
        paramDrawMotionTrs._object = _object;
        paramDrawMotionTrs.mtx = mtx;
        paramDrawMotionTrs.sub_obj_type = sub_obj_type;
        paramDrawMotionTrs.flag = drawflag;
        paramDrawMotionTrs.texlist = texlist;
        paramDrawMotionTrs.trslist = amDrawAlloc_NNS_TRS(nodeNum);
        paramDrawMotionTrs.material_func = null;
        for (int index = 0; index < nodeNum; ++index)
            paramDrawMotionTrs.trslist[index] = motion.data[index];
        int motionId = motion.mbuf[0].motion_id;
        paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & ushort.MaxValue];
        paramDrawMotionTrs.frame = motion.mbuf[0].frame;
        drawParam3DnnMotion.state = null;
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
        amDrawRegistCommand(command_state, OBD_DRAW_USER_COMMAND_3DNN_MOTION, paramDrawMotionTrs);
    }

    public static void ObjDraw3DNNMotionMaterialMotion(
      AMS_MOTION motion,
      NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT mplt_cb_func,
      object mplt_cb_param,
      MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!GmMainIsDrawEnable())
            return;
        if (motion.mmobject == null)
        {
            ObjDraw3DNNMotion(motion, motion._object, texlist, drawflag, sub_obj_type, user_func, user_param, command_state, mplt_cb_func, mplt_cb_param, material_cb_func, material_cb_param, draw_state, use_light_flag);
        }
        else
        {
            int nodeNum = motion.node_num;
            OBS_DRAW_PARAM_3DNN_MOTION drawParam3DnnMotion = amDrawAlloc_OBS_DRAW_PARAM_3DNN_MOTION();
            NNS_MATRIX mtx = drawParam3DnnMotion.mtx;
            mtx.Assign(amMatrixGetCurrent());
            AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs = drawParam3DnnMotion.param;
            paramDrawMotionTrs.mtx = mtx;
            paramDrawMotionTrs.sub_obj_type = sub_obj_type;
            paramDrawMotionTrs.flag = drawflag;
            paramDrawMotionTrs.texlist = texlist;
            paramDrawMotionTrs.trslist = amDrawAlloc_NNS_TRS(nodeNum);
            paramDrawMotionTrs.material_func = null;
            for (int index = 0; index < nodeNum; ++index)
            {
                paramDrawMotionTrs.trslist[index] = amDraw_NNS_TRS_Pool.Alloc();
                paramDrawMotionTrs.trslist[index].Assign(motion.data[index]);
            }
            paramDrawMotionTrs._object = motion._object;
            paramDrawMotionTrs.mmotion = motion.mmtn[motion.mmotion_id];
            paramDrawMotionTrs.mframe = motion.mmotion_frame;
            int motionId = motion.mbuf[0].motion_id;
            if (motion.mtnfile[motionId >> 16].file != null)
            {
                paramDrawMotionTrs.motion = motion.mtnfile[motionId >> 16].motion[motionId & ushort.MaxValue];
                paramDrawMotionTrs.frame = motion.mbuf[0].frame;
            }
            else
            {
                paramDrawMotionTrs.motion = null;
                paramDrawMotionTrs.frame = 0.0f;
            }
            drawParam3DnnMotion.state = null;
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
            amDrawRegistCommand(command_state, OBD_DRAW_USER_COMMAND_3DNN_MOTION_MATMTN, paramDrawMotionTrs);
        }
    }

    public static void ObjDraw3DNNDrawMotion(
      NNS_MOTION motion,
      float frame,
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT mplt_cb_func,
      object mplt_cb_param,
      MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!GmMainIsDrawEnable())
            return;
        OBS_DRAW_PARAM_3DNN_DRAW_MOTION param3DnnDrawMotion = amDrawAlloc_OBS_DRAW_PARAM_3DNN_DRAW_MOTION();
        NNS_MATRIX mtx = param3DnnDrawMotion.mtx;
        nnCopyMatrix(mtx, amMatrixGetCurrent());
        AMS_PARAM_DRAW_MOTION amsParamDrawMotion = param3DnnDrawMotion.param;
        amsParamDrawMotion._object = _object;
        amsParamDrawMotion.mtx = mtx;
        amsParamDrawMotion.sub_obj_type = sub_obj_type;
        amsParamDrawMotion.flag = drawflag;
        amsParamDrawMotion.texlist = texlist;
        amsParamDrawMotion.motion = motion;
        amsParamDrawMotion.frame = frame;
        amsParamDrawMotion.material_func = null;
        param3DnnDrawMotion.state = null;
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
                amDrawRegistCommand(command_state, OBD_DRAW_USER_COMMAND_3DNN_DRAW_MOTION, amsParamDrawMotion);
                break;
            default:
                param3DnnDrawMotion.state = param3DnnDrawMotion.draw_state;
                param3DnnDrawMotion.draw_state.Assign(draw_state);
                goto case null;
        }
    }

    public static void ObjDraw3DNNDrawMotionMaterialMotion(
      NNS_MOTION motion,
      float frame,
      NNS_OBJECT _object,
      NNS_TEXLIST texlist,
      uint drawflag,
      uint sub_obj_type,
      MPP_VOID_OBJECT_DELEGATE user_func,
      object user_param,
      uint command_state,
      MPP_VOID_ARRAYNNSMATRIX_NNSOBJECT_OBJECT mplt_cb_func,
      object mplt_cb_param,
      MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func,
      object material_cb_param,
      AMS_DRAWSTATE draw_state,
      uint use_light_flag)
    {
        if (!GmMainIsDrawEnable())
            return;
        OBS_DRAW_PARAM_3DNN_DRAW_MOTION param3DnnDrawMotion = new OBS_DRAW_PARAM_3DNN_DRAW_MOTION();
        NNS_MATRIX mtx = param3DnnDrawMotion.mtx;
        nnCopyMatrix(mtx, amMatrixGetCurrent());
        AMS_PARAM_DRAW_MOTION amsParamDrawMotion = param3DnnDrawMotion.param;
        amsParamDrawMotion._object = _object;
        amsParamDrawMotion.mtx = mtx;
        amsParamDrawMotion.sub_obj_type = sub_obj_type;
        amsParamDrawMotion.flag = drawflag;
        amsParamDrawMotion.texlist = texlist;
        amsParamDrawMotion.motion = motion;
        amsParamDrawMotion.frame = frame;
        amsParamDrawMotion.material_func = null;
        param3DnnDrawMotion.state = null;
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
        amDrawRegistCommand(command_state, OBD_DRAW_USER_COMMAND_3DNN_DRAW_MOTION_MATMTN, amsParamDrawMotion);
    }

    public static void ObjDraw3DNNDrawPrimitive(AMS_PARAM_DRAW_PRIMITIVE prim)
    {
        ObjDraw3DNNDrawPrimitive(prim, 0U, 0, 0);
    }

    public static void ObjDraw3DNNDrawPrimitive(AMS_PARAM_DRAW_PRIMITIVE prim, uint command)
    {
        ObjDraw3DNNDrawPrimitive(prim, command, 0, 0);
    }

    public static void ObjDraw3DNNDrawPrimitive(
      AMS_PARAM_DRAW_PRIMITIVE prim,
      uint command,
      int light,
      int cull)
    {
        if (!GmMainIsDrawEnable())
            return;
        OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE dnnDrawPrimitive = OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE_Pool.Alloc();
        dnnDrawPrimitive.dat.Assign(prim);
        nnCopyMatrix(dnnDrawPrimitive.mtx, amMatrixGetCurrent());
        dnnDrawPrimitive.light = light;
        dnnDrawPrimitive.cull = cull;
        _ObjDraw3DNNUserFunc(_objDraw3DNNDrawPrimitive_DT, dnnDrawPrimitive, command);
    }

    public static uint ObjDraw3DNNGetMaterialUserData(NNS_DRAWCALLBACK_VAL val)
    {
        uint num = 0;
        switch (val.pMaterial.fType)
        {
            case 1:
                mppAssertNotImpl();
                break;
            case 2:
                mppAssertNotImpl();
                break;
            case 4:
                mppAssertNotImpl();
                break;
            case 8:
                num = ((NNS_MATERIAL_GLES11_DESC)val.pMaterial.pMaterial).User;
                break;
            default:
                num = 0U;
                break;
        }
        return num;
    }

    public static void ObjDrawSetToon(OBS_ACTION3D_NN_WORK obj_3d)
    {
    }

    public static int ObjDrawToonMaterialCallback(NNS_DRAWCALLBACK_VAL val, object param)
    {
        return nnPutMaterialCore(val);
    }

    public static void ObjDrawObjectAction3DES(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION3D_ES_WORK obj_3des)
    {
        VecFx32 vecFx32 = new VecFx32();
        VecU16 dir = new VecU16();
        vecFx32.Assign(obj_work.pos);
        dir.Assign(obj_work.dir);
        vecFx32.x += obj_work.ofst.x;
        vecFx32.y += obj_work.ofst.y;
        vecFx32.z += obj_work.ofst.z;
        if (obj_work.dir_fall != 0)
            dir.z += obj_work.dir_fall;
        ObjDrawAction3DES(obj_3des, new VecFx32?(vecFx32), ref dir, new VecFx32?(obj_work.scale), ref obj_work.disp_flag);
    }

    public static void ObjDrawAction3DES(
      OBS_ACTION3D_ES_WORK obj_3des,
      VecFx32? pos,
      ref VecU16 dir,
      VecFx32? scale,
      ref uint p_disp_flag)
    {
        uint? p_disp_flag1 = new uint?(p_disp_flag);
        ObjDrawAction3DES(obj_3des, pos, new AppMain.VecU16?(dir), scale, ref p_disp_flag1);
        p_disp_flag = p_disp_flag1.Value;
    }

    public static void ObjDrawAction3DES(
      OBS_ACTION3D_ES_WORK obj_3des,
      VecFx32? pos,
      VecU16? dir,
      VecFx32? scale,
      ref uint? p_disp_flag)
    {
        if (obj_3des.ecb == null)
            return;
        uint num = 0;
        NNS_MATRIX nnsMatrix1 = amDrawAlloc_NNS_MATRIX();
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion3 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion4 = new NNS_QUATERNION();
        vec_posObjDrawAction3DES.w = 0.0f;
        amQuatInit(ref nnsQuaternion1);
        amVectorInit(ref vecObjDrawAction3DES);
        nnMakeUnitMatrix(nnsMatrix1);
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
        amQuatEulerToQuatXYZ(ref nnsQuaternion2, obj_3des.disp_rot.x, obj_3des.disp_rot.y, obj_3des.disp_rot.z);
        if (((int)num & 4194304) == 0)
        {
            if (((int)num & 1) != 0)
            {
                if (((int)obj_3des.flag & 4) != 0)
                    amQuatEulerToQuatXYZ(ref nnsQuaternion3, NNM_DEGtoA32(0.0f), NNM_DEGtoA32(0.0f), NNM_DEGtoA32(270f));
                else
                    amQuatEulerToQuatXYZ(ref nnsQuaternion3, NNM_DEGtoA32(0.0f), NNM_DEGtoA32(270f), NNM_DEGtoA32(0.0f));
            }
            else if (((int)obj_3des.flag & 4) != 0)
                amQuatEulerToQuatXYZ(ref nnsQuaternion3, NNM_DEGtoA32(0.0f), NNM_DEGtoA32(0.0f), NNM_DEGtoA32(90f));
            else
                amQuatEulerToQuatXYZ(ref nnsQuaternion3, NNM_DEGtoA32(0.0f), NNM_DEGtoA32(90f), NNM_DEGtoA32(0.0f));
        }
        else
            amQuatInit(ref nnsQuaternion3);
        if (dir.HasValue && ((int)num & 256) == 0)
        {
            if (((int)num & 2097152) == 0)
                amQuatEulerToQuatXYZ(ref nnsQuaternion4, -dir.Value.x, dir.Value.y, -dir.Value.z);
            else
                amQuatEulerToQuatXYZ(ref nnsQuaternion4, dir.Value.x, dir.Value.y, dir.Value.z);
            if (((int)obj_3des.flag & 32) != 0)
                nnMultiplyQuaternion(ref nnsQuaternion4, ref nnsQuaternion4, ref obj_3des.user_dir_quat);
        }
        else
            amQuatInit(ref nnsQuaternion4);
        vec_dispObjDrawAction3DES.w = 0.0f;
        amVectorSet(out vec_dispObjDrawAction3DES, obj_3des.disp_ofst.x, obj_3des.disp_ofst.y, obj_3des.disp_ofst.z);
        if (pos.HasValue)
        {
            VecFx32 vecFx32 = pos.Value;
            if (((int)num & 2097152) == 0)
                vecFx32.y = -vecFx32.y;
            amVectorSet(out vec_posObjDrawAction3DES, FX_FX32_TO_F32(vecFx32.x), FX_FX32_TO_F32(vecFx32.y), FX_FX32_TO_F32(vecFx32.z));
        }
        else
            amVectorInit(ref vec_posObjDrawAction3DES);
        amVectorOne(ref vec_scaleObjDrawAction3DES);
        if (scale.HasValue && ((int)num & 65536) == 0)
        {
            if (((int)obj_3des.flag & 8) != 0)
            {
                amVectorSet(out vec_scaleObjDrawAction3DES, FX_FX32_TO_F32(scale.Value.x), FX_FX32_TO_F32(scale.Value.y), FX_FX32_TO_F32(scale.Value.z));
                amEffectSetSizeRate(obj_3des.ecb, 1f);
            }
            else
                amEffectSetSizeRate(obj_3des.ecb, FX_FX32_TO_F32(scale.Value.x));
        }
        if (((int)obj_3des.flag & 1) != 0)
        {
            amVectorAdd(ref vecObjDrawAction3DES, ref vec_dispObjDrawAction3DES, ref vecObjDrawAction3DES);
            amQuatMulti(ref nnsQuaternion1, ref nnsQuaternion3, ref nnsQuaternion1);
            amQuatMulti(ref nnsQuaternion1, ref nnsQuaternion4, ref nnsQuaternion1);
            SNNS_MATRIX dst1;
            nnMakeQuaternionMatrix(out dst1, ref nnsQuaternion1);
            SNNS_VECTOR snnsVector;
            snnsVector.x = vecObjDrawAction3DES.x;
            snnsVector.y = vecObjDrawAction3DES.y;
            snnsVector.z = vecObjDrawAction3DES.z;
            nnTransformVector(ref snnsVector, ref dst1, ref snnsVector);
            vecObjDrawAction3DES.x = snnsVector.x;
            vecObjDrawAction3DES.y = snnsVector.y;
            vecObjDrawAction3DES.z = snnsVector.z;
            amQuatMulti(ref nnsQuaternion1, ref nnsQuaternion1, ref nnsQuaternion2);
            if (((int)obj_3des.flag & 16) != 0)
                amEffectSetRotate(obj_3des.ecb, ref nnsQuaternion1, 1);
            else
                amEffectSetRotate(obj_3des.ecb, ref nnsQuaternion1);
            SNNS_MATRIX dst2;
            nnMakeScaleMatrix(out dst2, vec_scaleObjDrawAction3DES.x, vec_scaleObjDrawAction3DES.y, vec_scaleObjDrawAction3DES.z);
            nnMultiplyMatrix(nnsMatrix1, ref dst2, nnsMatrix1);
            if (((int)obj_3des.flag & 2) != 0)
            {
                SNNS_MATRIX dst3;
                nnMakeTranslateMatrix(out dst3, vec_posObjDrawAction3DES.x, vec_posObjDrawAction3DES.y, vec_posObjDrawAction3DES.z);
                nnMultiplyMatrix(nnsMatrix1, ref dst3, nnsMatrix1);
            }
            else
                amVectorAdd(ref vecObjDrawAction3DES, ref vec_posObjDrawAction3DES, ref vecObjDrawAction3DES);
            amEffectSetTranslate(obj_3des.ecb, ref vecObjDrawAction3DES);
        }
        else
        {
            amQuatMulti(ref nnsQuaternion1, ref nnsQuaternion3, ref nnsQuaternion1);
            amQuatMulti(ref nnsQuaternion1, ref nnsQuaternion4, ref nnsQuaternion1);
            nnMakeQuaternionMatrix(nnsMatrix1, ref nnsQuaternion1);
            SNNS_VECTOR vec;
            vec.x = vec_posObjDrawAction3DES.x;
            vec.y = vec_posObjDrawAction3DES.y;
            vec.z = vec_posObjDrawAction3DES.z;
            nnCopyVectorMatrixTranslation(nnsMatrix1, ref vec);
            SNNS_MATRIX dst1;
            nnMakeScaleMatrix(out dst1, vec_scaleObjDrawAction3DES.x, vec_scaleObjDrawAction3DES.y, vec_scaleObjDrawAction3DES.z);
            nnMultiplyMatrix(nnsMatrix1, nnsMatrix1, ref dst1);
            SNNS_MATRIX dst2;
            nnMakeTranslateMatrix(out dst2, vec_dispObjDrawAction3DES.x, vec_dispObjDrawAction3DES.y, vec_dispObjDrawAction3DES.z);
            nnMultiplyMatrix(nnsMatrix1, nnsMatrix1, ref dst2);
            SNNS_MATRIX dst3;
            nnMakeQuaternionMatrix(out dst3, ref nnsQuaternion2);
            nnMultiplyMatrix(nnsMatrix1, nnsMatrix1, ref dst3);
        }
        ObjDraw3DESSetCamera(obj_3des, nnsMatrix1);
        ObjDraw3DESMatrixPush(nnsMatrix1, obj_3des.command_state);
        float unitFrame = amEffectGetUnitFrame();
        if (((int)num & 4096) == 0)
        {
            if (((int)num & 16) != 0)
                amEffectSetUnitTime(0.0f, 60);
            else
                amEffectSetUnitTime(obj_3des.speed * FX_FX32_TO_F32(_g_obj.speed), 60);
            amEffectUpdate(obj_3des.ecb);
            if (amEffectIsDelete(obj_3des.ecb) != 0)
            {
                obj_3des.ecb = null;
                if (p_disp_flag.HasValue)
                {
                    ref uint? local = ref p_disp_flag;
                    uint? nullable = local;
                    local = nullable.HasValue ? new uint?(nullable.GetValueOrDefault() | 8U) : new uint?();
                }
            }
        }
        amEffectSetUnitTime(unitFrame, 60);
        if (((int)num & 32) == 0 && obj_3des.ecb != null)
        {
            ObjDraw3DESEffect(obj_3des.ecb, obj_3des.texlist, obj_3des.command_state);
            if (((int)obj_3des.flag & 64) != 0)
            {
                NNS_MATRIX nnsMatrix2 = amDrawAlloc_NNS_MATRIX();
                nnMakeTranslateMatrix(nnsMatrix2, obj_3des.dup_draw_ofst.x, obj_3des.dup_draw_ofst.y, obj_3des.dup_draw_ofst.z);
                nnMultiplyMatrix(nnsMatrix2, nnsMatrix2, nnsMatrix1);
                ObjDraw3DESMatrixPop(obj_3des.command_state);
                ObjDraw3DESSetCamera(obj_3des, nnsMatrix2);
                ObjDraw3DESMatrixPush(nnsMatrix2, obj_3des.command_state);
                ObjDraw3DESEffect(obj_3des.ecb, obj_3des.texlist, obj_3des.command_state);
            }
        }
        ObjDraw3DESMatrixPop(obj_3des.command_state);
    }

    public static void ObjDraw3DESEffect(
      AMS_AME_ECB ecb,
      NNS_TEXLIST texlist,
      uint command_state)
    {
        amEffectDraw(ecb, texlist, command_state);
    }

    public static void ObjDraw3DESMatrixPush(NNS_MATRIX mtx, uint command_state)
    {
        _ObjDraw3DNNUserFunc(_objDraw3DESMatrixPush_UserFunc, mtx, command_state);
    }

    public static void ObjDraw3DESMatrixPop(uint command_state)
    {
        ObjDraw3DNNUserFunc(_objDraw3DESMatrixPop_UserFunc, null, 0, command_state);
    }

    public static void ObjDrawKillAction3DES(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_3des.ecb == null)
            return;
        amEffectKill(obj_work.obj_3des.ecb);
    }

    public static void ObjDraw3DESSetCamera(
      OBS_ACTION3D_ES_WORK obj_3des,
      NNS_MATRIX obj_mtx)
    {
        SNNS_VECTOR disp_pos = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector = new SNNS_VECTOR();
        ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, g_obj.glb_camera_type, obj_3des.command_state);
        ObjCameraDispPosGet(g_obj.glb_camera_id, out disp_pos);
        amVectorSet(ref snnsVector, -obj_mtx.M03, -obj_mtx.M13, -obj_mtx.M23);
        nnAddVector(ref disp_pos, ref snnsVector, ref disp_pos);
        amEffectSetCameraPos(ref disp_pos);
    }

    public static void ObjDrawObjectAction2DAMA(
      OBS_OBJECT_WORK obj_work,
      OBS_ACTION2D_AMA_WORK obj_2d)
    {
        if (!GmMainIsDrawEnable())
            return;
        VecFx32 vecFx32 = new VecFx32(obj_work.pos);
        VecU16 vecU16 = new VecU16(obj_work.dir);
        vecFx32.x += obj_work.ofst.x;
        vecFx32.y += obj_work.ofst.y;
        vecFx32.z += obj_work.ofst.z;
        ObjDrawAction2DAMA(obj_2d, new VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), new VecFx32?(obj_work.scale), ref obj_work.disp_flag);
    }

    public static void ObjDrawAction2DAMA(
      OBS_ACTION2D_AMA_WORK obj_2d,
      VecFx32? pos,
      VecU16? dir,
      VecFx32? scale,
      ref uint p_disp_flag)
    {
        uint num = 0;
        if (!GmMainIsDrawEnable())
            return;
        if (p_disp_flag != 0U)
        {
            if (((int)p_disp_flag & 16) == 0)
                p_disp_flag &= 4294967287U;
            num = p_disp_flag;
        }
        AoActSetTexture(obj_2d.texlist);
        AOS_ACT_ACM drawAction2DamaAcm = ObjDrawAction2DAMA_acm;
        AoActAcmInit(drawAction2DamaAcm);
        if (pos.HasValue && ((int)num & 8192) == 0)
        {
            drawAction2DamaAcm.trans_x = FXM_FX32_TO_FLOAT(pos.Value.x);
            drawAction2DamaAcm.trans_y = FXM_FX32_TO_FLOAT(pos.Value.y);
            drawAction2DamaAcm.trans_z = FXM_FX32_TO_FLOAT(pos.Value.z);
        }
        if (dir.HasValue && ((int)num & 256) == 0)
            drawAction2DamaAcm.rotate = NNM_A32toDEG(dir.Value.z);
        if (scale.HasValue && ((int)num & 65536) == 0)
        {
            drawAction2DamaAcm.scale_x = FXM_FX32_TO_FLOAT(scale.Value.x);
            drawAction2DamaAcm.scale_y = FXM_FX32_TO_FLOAT(scale.Value.y);
        }
        drawAction2DamaAcm.color = obj_2d.color;
        drawAction2DamaAcm.fade = obj_2d.fade;
        AoActAcmPush(drawAction2DamaAcm);
        if (((int)num & 4096) == 0)
        {
            float frame = obj_2d.speed * FX_FX32_TO_F32(g_obj.speed) * GmMainGetDrawMotionSpeed();
            if (obj_2d.act.frame == (double)obj_2d.frame)
            {
                AoActUpdate(obj_2d.act, frame);
                obj_2d.frame = obj_2d.act.frame;
            }
            else
            {
                AoActSetFrame(obj_2d.act, obj_2d.frame);
                AoActUpdate(obj_2d.act, 0.0f);
            }
            if (((int)num & 4) != 0)
            {
                if (AoActIsEnd(obj_2d.act))
                {
                    obj_2d.frame = 0.0f;
                    num |= 8U;
                }
            }
            else if (AoActIsEnd(obj_2d.act))
                num |= 8U;
        }
        if (((int)num & 32) == 0)
            AoActSortRegAction(obj_2d.act);
        AoActAcmPop(1U);
        p_disp_flag |= num & 8U;
    }

    public static void ObjDrawAction2DAMADrawStart()
    {
        if (!GmMainIsDrawEnable())
            return;
        ObjDraw3DNNUserFunc(_objDraw2DAMAPre_DT, null, 0, 6U);
        AoActSortExecuteFix();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    public static void ObjDrawSetParallelLight(
      int light_no,
      ref NNS_RGBA col,
      float intensity,
      NNS_VECTOR vec)
    {
        nnSetUpParallelLight(g_obj.light[light_no].parallel, ref col, intensity, vec);
        g_obj.light[light_no].light_type = 1U;
    }

    private void ObjDrawSetPointLight(
      int light_no,
      ref NNS_RGBA col,
      float intensity,
      NNS_VECTOR pos,
      float falloffstart,
      float falloffend)
    {
        mppAssertNotImpl();
        nnSetUpPointLight(g_obj.light[light_no].point, ref col, intensity, pos, falloffstart, falloffend);
        g_obj.light[light_no].light_type = 2U;
    }

    private void ObjDrawSetTargetSpotLight(
      int light_no,
      ref NNS_RGBA col,
      float intensity,
      NNS_VECTOR pos,
      NNS_VECTOR target,
      int innerangle,
      int outerangle,
      float falloffstart,
      float falloffend)
    {
        mppAssertNotImpl();
        nnSetUpTargetSpotLight(g_obj.light[light_no].target_spot, ref col, intensity, pos, target, innerangle, outerangle, falloffstart, falloffend);
        g_obj.light[light_no].light_type = 4U;
    }

    private void ObjDrawSetRotationSpotLight(
      int light_no,
      ref NNS_RGBA col,
      float intensity,
      NNS_VECTOR pos,
      int rottype,
      NNS_ROTATE_A32 rotation,
      int innerangle,
      int outerangle,
      float falloffstart,
      float falloffend)
    {
        mppAssertNotImpl();
        nnSetUpRotationSpotLight(g_obj.light[light_no].rotation_spot, ref col, intensity, pos, rottype, rotation, innerangle, outerangle, falloffstart, falloffend);
        g_obj.light[light_no].light_type = 8U;
    }

    public static void objDrawStart_DT(AMS_TCB tcb)
    {
        nnSetAmbientColor(g_obj.ambient_color.r, g_obj.ambient_color.g, g_obj.ambient_color.b);

        for (int no = 0; no < NNE_LIGHT_MAX; ++no)
        {
            if ((g_obj.def_user_light_flag & (uint)(1 << no)) != 0)
            {
                nnSetLight(no, g_obj.light[no].light_param, g_obj.light[no].light_type);
                nnSetLightSwitch(no, 1);
            }
            else
            {
                nnSetLightSwitch(no, 0);
            }
        }

        nnPutLightSettings();
        for (int index = 0; index < 18; ++index)
        {
            if (obj_draw_3dnn_command_state_tbl[index] != uint.MaxValue)
            {
                amDrawExecCommand(obj_draw_3dnn_command_state_tbl[index], g_obj.drawflag);
                if (obj_draw_3dnn_command_state_exe_end_scene_tbl[index])
                    amDrawEndScene();
            }
        }
    }

    public static void objDraw3DNNSetCamera(
      OBS_CAMERA obj_camera,
      int proj_type,
      uint command_state)
    {
        OBS_DRAW_PARAM_3DNN_SET_CAMERA param3DnnSetCamera = _objDraw3DNNSetCamera_Pool.Alloc();
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
            NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_CAMERAPTR nnsCameraptr = new NNS_CAMERAPTR();
            NNS_CAMERA_TARGET_ROLL cam = new NNS_CAMERA_TARGET_ROLL();
            NNS_VECTOR vec = new NNS_VECTOR(0.0f, 0.0f, 0.0f);
            nnsCameraptr.fType = byte.MaxValue;
            nnsCameraptr.pCamera = cam;
            cam.Target.Assign(vec);
            cam.Position.Assign(cam.Target);
            cam.Position.z += 50f;
            cam.Position.x += 0.0f;
            cam.Roll = NNM_DEGtoA32(0.0f);
            cam.Fovy = NNM_DEGtoA32(45f);
            cam.Aspect = AMD_SCREEN_ASPECT;
            cam.ZNear = 1f;
            cam.ZFar = 60000f;
            switch (proj_type)
            {
                case 1:
                    float top = (float)(g_obj.disp_height * (5.0 / 64.0) * 0.5);
                    float right = top * cam.Aspect;
                    nnMakeOrthoMatrix(nnsMatrix2, -right, right, -top, top, cam.ZNear, cam.ZFar);
                    param3DnnSetCamera.prj_mtx.Assign(nnsMatrix2);
                    break;
                default:
                    nnMakePerspectiveMatrix(nnsMatrix2, cam.Fovy, cam.Aspect, cam.ZNear, cam.ZFar);
                    param3DnnSetCamera.prj_mtx.Assign(nnsMatrix2);
                    break;
            }
            nnMakeTargetRollCameraViewMatrix(nnsMatrix1, cam);
            param3DnnSetCamera.view_mtx.Assign(nnsMatrix1);
        }
        amEffectSetWorldViewMatrix(param3DnnSetCamera.view_mtx);
        amDrawRegistCommand(command_state, OBD_DRAW_USER_COMMAND_3DNN_SET_CAMERA, param3DnnSetCamera);
    }

    public static void objDraw3DNNModel_DT(AMS_COMMAND_HEADER command, uint drawflag)
    {
        NNS_MATRIX nnsMatrix = amDrawAlloc_NNS_MATRIX();
        amMatrixPush();
        OBS_DRAW_PARAM_3DNN_MODEL drawParam3DnnModel = !(command.param is AMS_PARAM_DRAW_OBJECT) ? (OBS_DRAW_PARAM_3DNN_MODEL)command.param : (OBS_DRAW_PARAM_3DNN_MODEL)(AMS_PARAM_DRAW_OBJECT)command.param;
        uint alternativeLighting = drawParam3DnnModel.use_light_flag & 98304U;
        if (((int)g_obj.def_user_light_flag ^ (int)drawParam3DnnModel.use_light_flag) != 0)
            objDrawSetDrawLight(drawParam3DnnModel.use_light_flag);
        if (drawParam3DnnModel.user_func != null)
            drawParam3DnnModel.user_func(drawParam3DnnModel.user_param);
        AMS_PARAM_DRAW_OBJECT amsParamDrawObject = drawParam3DnnModel.param;
        int nNode = amsParamDrawObject._object.nNode;
        OBS_DRAW_PARAM_3DNN_SORT_MODEL_Pool.Alloc();
        if (_objDraw3DNNModel_DT.plt_mtx == null || _objDraw3DNNModel_DT.plt_mtx.Length < nNode)
        {
            _objDraw3DNNModel_DT.plt_mtx = new NNS_MATRIX[nNode];
            for (int index = 0; index < nNode; ++index)
                _objDraw3DNNModel_DT.plt_mtx[index] = GlobalPool<NNS_MATRIX>.Alloc();
        }
        NNS_MATRIX[] pltMtx = _objDraw3DNNModel_DT.plt_mtx;
        if (_objDraw3DNNModel_DT.nstat == null || _objDraw3DNNModel_DT.nstat.Length < (nNode + 3 & -4))
            _objDraw3DNNModel_DT.nstat = new uint[nNode + 3 & -4];
        uint[] nstat = _objDraw3DNNModel_DT.nstat;
        if (amsParamDrawObject.mtx != null)
        {
            nnMultiplyMatrix(nnsMatrix, amMatrixGetCurrent(), amsParamDrawObject.mtx);
            nnMultiplyMatrix(nnsMatrix, _am_draw_world_view_matrix, nnsMatrix);
        }
        else
            nnMultiplyMatrix(nnsMatrix, _am_draw_world_view_matrix, amMatrixGetCurrent());
        nnSetUpNodeStatusList(nstat, nNode, 0U);
        nnCalcMatrixPalette(pltMtx, nstat, amsParamDrawObject._object, nnsMatrix, _am_default_stack, 1U);
        if (amsParamDrawObject.texlist != null)
            nnSetTextureList(amsParamDrawObject.texlist);
        if (drawParam3DnnModel.state != null)
        {
            amDrawPushState();
            amDrawSetState(drawParam3DnnModel.state);
        }
        if (drawParam3DnnModel.material_cb_func != null)
            objDraw3DNNSetMaterialCallback(drawParam3DnnModel.material_cb_func, drawParam3DnnModel.material_cb_param);
        if (command.command_id == OBD_DRAW_USER_COMMAND_3DNN_MODEL)
            nnDrawObject(amsParamDrawObject._object, pltMtx, nstat, (uint)((int)amsParamDrawObject.sub_obj_type | 256 | 512 | 7), amsParamDrawObject.flag | drawflag | amDrawGetState().drawflag, alternativeLighting);
        else
            nnDrawMaterialMotionObject(amsParamDrawObject._object, pltMtx, nstat, (uint)((int)amsParamDrawObject.sub_obj_type | 256 | 512 | 7), amsParamDrawObject.flag | drawflag | amDrawGetState().drawflag);
        if (drawParam3DnnModel.material_cb_func != null)
            objDraw3DNNSetMaterialCallback(null, null);
        if (drawParam3DnnModel.state != null)
            amDrawPopState();
        if (((int)g_obj.def_user_light_flag ^ (int)drawParam3DnnModel.use_light_flag) != 0)
            objDrawSetDefaultLight();
        amMatrixPop();
    }

    private static void objDraw3DNNMotion_DT(AMS_COMMAND_HEADER command, uint drawflag)
    {
        SNNS_MATRIX snnsMatrix = new SNNS_MATRIX();
        amMatrixPush();
        OBS_DRAW_PARAM_3DNN_MOTION drawParam3DnnMotion = !(command.param is OBS_DRAW_PARAM_3DNN_MOTION) ? (OBS_DRAW_PARAM_3DNN_MOTION)(AMS_PARAM_DRAW_MOTION_TRS)command.param : (OBS_DRAW_PARAM_3DNN_MOTION)command.param;
        if (((int)g_obj.def_user_light_flag ^ (int)drawParam3DnnMotion.use_light_flag) != 0)
            objDrawSetDrawLight(drawParam3DnnMotion.use_light_flag);
        if (drawParam3DnnMotion.user_func != null)
            drawParam3DnnMotion.user_func(drawParam3DnnMotion.user_param);
        AMS_PARAM_DRAW_MOTION_TRS paramDrawMotionTrs = drawParam3DnnMotion.param;
        int nNode = paramDrawMotionTrs._object.nNode;
        int nMtxPal = paramDrawMotionTrs._object.nMtxPal;
        if (command.command_id == OBD_DRAW_USER_COMMAND_3DNN_MOTION_MATMTN && paramDrawMotionTrs.mmotion != null)
        {
            NNS_OBJECT mmobj = amDrawAlloc_NNS_OBJECT();
            nnInitMaterialMotionObject_fast(mmobj, paramDrawMotionTrs._object, paramDrawMotionTrs.mmotion);
            nnCalcMaterialMotion(mmobj, paramDrawMotionTrs._object, paramDrawMotionTrs.mmotion, paramDrawMotionTrs.mframe);
            paramDrawMotionTrs._object = mmobj;
        }
        OBS_DRAW_PARAM_3DNN_SORT_MODEL_Pool.Alloc();
        if (_objDraw3DNNMotion_DT.plt_mtx == null || _objDraw3DNNMotion_DT.plt_mtx.Length < nMtxPal)
        {
            _objDraw3DNNMotion_DT.plt_mtx = new NNS_MATRIX[nMtxPal];
            for (int index = 0; index < nMtxPal; ++index)
                _objDraw3DNNMotion_DT.plt_mtx[index] = GlobalPool<NNS_MATRIX>.Alloc();
        }
        NNS_MATRIX[] pltMtx = _objDraw3DNNMotion_DT.plt_mtx;
        if (_objDraw3DNNMotion_DT.nstat == null || _objDraw3DNNMotion_DT.nstat.Length < nNode)
            _objDraw3DNNMotion_DT.nstat = new uint[nNode];
        uint[] nstat = _objDraw3DNNMotion_DT.nstat;
        if (paramDrawMotionTrs.mtx != null)
        {
            nnMultiplyMatrix(ref snnsMatrix, amMatrixGetCurrent(), paramDrawMotionTrs.mtx);
            nnMultiplyMatrix(ref snnsMatrix, _am_draw_world_view_matrix, ref snnsMatrix);
        }
        else
            nnMultiplyMatrix(ref snnsMatrix, _am_draw_world_view_matrix, amMatrixGetCurrent());
        nnSetUpNodeStatusList(nstat, nNode, 0U);
        nnCalcMatrixPaletteTRSList(pltMtx, nstat, paramDrawMotionTrs._object, paramDrawMotionTrs.trslist, ref snnsMatrix, _am_default_stack, 1U);
        if (paramDrawMotionTrs.motion != null)
            nnCalcNodeHideMotion(nstat, paramDrawMotionTrs.motion, paramDrawMotionTrs.frame);
        if (drawParam3DnnMotion.mplt_cb_func != null)
            drawParam3DnnMotion.mplt_cb_func(pltMtx, paramDrawMotionTrs._object, drawParam3DnnMotion.mplt_cb_param);
        if (paramDrawMotionTrs.texlist != null)
            nnSetTextureList(paramDrawMotionTrs.texlist);
        if (drawParam3DnnMotion.state != null)
        {
            amDrawPushState();
            amDrawSetState(drawParam3DnnMotion.state);
        }
        if (drawParam3DnnMotion.material_cb_func != null)
            objDraw3DNNSetMaterialCallback(drawParam3DnnMotion.material_cb_func, drawParam3DnnMotion.material_cb_param);
        if (command.command_id == OBD_DRAW_USER_COMMAND_3DNN_MOTION)
            nnDrawObject(paramDrawMotionTrs._object, pltMtx, nstat, (uint)((int)paramDrawMotionTrs.sub_obj_type | 256 | 512 | 7), paramDrawMotionTrs.flag | drawflag | amDrawGetState().drawflag, 0U);
        else
            nnDrawMaterialMotionObject(paramDrawMotionTrs._object, pltMtx, nstat, (uint)((int)paramDrawMotionTrs.sub_obj_type | 256 | 512 | 7), paramDrawMotionTrs.flag | drawflag | amDrawGetState().drawflag);
        if (drawParam3DnnMotion.material_cb_func != null)
            objDraw3DNNSetMaterialCallback(null, null);
        if (drawParam3DnnMotion.state != null)
            amDrawPopState();
        if (((int)g_obj.def_user_light_flag ^ (int)drawParam3DnnMotion.use_light_flag) != 0)
            objDrawSetDefaultLight();
        amMatrixPop();
    }

    public static void objDraw3DNNDrawPrimitive_DT(object param)
    {
        OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE dnnDrawPrimitive = (OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE)param;
        AMS_PARAM_DRAW_PRIMITIVE dat = dnnDrawPrimitive.dat;
        amMatrixPush();
        nnMultiplyMatrix(ref tempSNNS_MATRIX0, amMatrixGetCurrent(), dnnDrawPrimitive.mtx);
        nnMultiplyMatrix(ref tempSNNS_MATRIX0, _am_draw_world_view_matrix, ref tempSNNS_MATRIX0);
        nnSetPrimitive3DMatrix(ref tempSNNS_MATRIX0);
        nnSetPrimitiveTexNum(dat.texlist, dat.texId);
        nnSetPrimitiveTexState(0, 0, dat.uwrap, dat.vwrap);
        if (dat.aTest != 0)
            nnSetPrimitive3DAlphaFuncGL(516U, 0.5f);
        else
            nnSetPrimitive3DAlphaFuncGL(519U, 0.5f);
        nnSetPrimitive3DDepthMaskGL(dat.zMask == 0);
        if (dat.zTest != 0)
            nnSetPrimitive3DDepthFuncGL(515U);
        else
            nnSetPrimitive3DDepthFuncGL(519U);
        if (dat.ablend != 0 && dat.bldMode == 32774)
        {
            switch (dat.bldDst)
            {
                case 1:
                    nnSetPrimitiveBlend(0);
                    break;
                case 771:
                    nnSetPrimitiveBlend(1);
                    break;
                default:
                    nnSetPrimitiveBlend(1);
                    break;
            }
        }
        nnBeginDrawPrimitive3D(dat.format3D, dat.ablend, dnnDrawPrimitive.light, dnnDrawPrimitive.cull);
        switch (dat.format3D)
        {
            case 2:
                nnDrawPrimitive3D(dat.type, dat.vtxPC3D, dat.count);
                break;
            case 4:
                nnDrawPrimitive3D(dat.type, dat.vtxPCT3D, dat.count);
                break;
        }
        nnEndDrawPrimitive3D();
        amMatrixPop();
    }

    private static void objDraw3DNNSetCamera_DT(AMS_COMMAND_HEADER command, uint drawflag)
    {
        OBS_DRAW_PARAM_3DNN_SET_CAMERA param3DnnSetCamera = (OBS_DRAW_PARAM_3DNN_SET_CAMERA)command.param;
        if (param3DnnSetCamera.proj_type == 1)
            amDrawSetProjection(param3DnnSetCamera.prj_mtx, 1);
        else
            amDrawSetProjection(param3DnnSetCamera.prj_mtx, 0);
        amDrawSetWorldViewMatrix(param3DnnSetCamera.view_mtx);
        nnSetLightMatrix(param3DnnSetCamera.view_mtx);
        nnPutLightSettings();
    }

    public static void objDraw3DNNUserFunc_DT(AMS_COMMAND_HEADER command, uint drawflag)
    {
        OBS_DRAW_PARAM_3DNN_USER_FUNC param3DnnUserFunc = (OBS_DRAW_PARAM_3DNN_USER_FUNC)command.param;
        param3DnnUserFunc.func(param3DnnUserFunc.param);
    }

    public static void objDraw3DNNDrawMotion_DT(AMS_COMMAND_HEADER command, uint drawflag)
    {
        NNS_MATRIX nnsMatrix = amDrawAlloc_NNS_MATRIX();
        amMatrixPush();
        OBS_DRAW_PARAM_3DNN_DRAW_MOTION param3DnnDrawMotion = !(command.param is AMS_PARAM_DRAW_MOTION) ? (OBS_DRAW_PARAM_3DNN_DRAW_MOTION)command.param : (OBS_DRAW_PARAM_3DNN_DRAW_MOTION)(AMS_PARAM_DRAW_MOTION)command.param;
        if (((int)g_obj.def_user_light_flag ^ (int)param3DnnDrawMotion.use_light_flag) != 0)
            objDrawSetDrawLight(param3DnnDrawMotion.use_light_flag);
        if (param3DnnDrawMotion.user_func != null)
            param3DnnDrawMotion.user_func(param3DnnDrawMotion.user_param);
        AMS_PARAM_DRAW_MOTION amsParamDrawMotion = param3DnnDrawMotion.param;
        int nNode = amsParamDrawMotion._object.nNode;
        if (_objDraw3DNNDrawMotion_DT.plt_mtx == null || _objDraw3DNNDrawMotion_DT.plt_mtx.Length < nNode)
        {
            _objDraw3DNNDrawMotion_DT.plt_mtx = new NNS_MATRIX[nNode];
            _objDraw3DNNDrawMotion_DT.nstat = new uint[nNode];
            for (int index = 0; index < nNode; ++index)
                _objDraw3DNNDrawMotion_DT.plt_mtx[index] = GlobalPool<NNS_MATRIX>.Alloc();
        }
        NNS_MATRIX[] pltMtx = _objDraw3DNNDrawMotion_DT.plt_mtx;
        uint[] nstat = _objDraw3DNNDrawMotion_DT.nstat;
        if (amsParamDrawMotion.mtx != null)
        {
            nnMultiplyMatrix(nnsMatrix, amMatrixGetCurrent(), amsParamDrawMotion.mtx);
            nnMultiplyMatrix(nnsMatrix, _am_draw_world_view_matrix, nnsMatrix);
        }
        else
            nnMultiplyMatrix(nnsMatrix, _am_draw_world_view_matrix, amMatrixGetCurrent());
        nnSetUpNodeStatusList(nstat, nNode, 0U);
        nnCalcMatrixPaletteMotion(pltMtx, nstat, amsParamDrawMotion._object, amsParamDrawMotion.motion, amsParamDrawMotion.frame, nnsMatrix, _am_default_stack, 1U);
        nnCalcNodeHideMotion(nstat, amsParamDrawMotion.motion, amsParamDrawMotion.frame);
        if (param3DnnDrawMotion.mplt_cb_func != null)
            param3DnnDrawMotion.mplt_cb_func(pltMtx, amsParamDrawMotion._object, param3DnnDrawMotion.mplt_cb_param);
        if (amsParamDrawMotion.texlist != null)
            nnSetTextureList(amsParamDrawMotion.texlist);
        if (param3DnnDrawMotion.state != null)
        {
            amDrawPushState();
            amDrawSetState(param3DnnDrawMotion.state);
        }
        if (param3DnnDrawMotion.material_cb_func != null)
            objDraw3DNNSetMaterialCallback(param3DnnDrawMotion.material_cb_func, param3DnnDrawMotion.material_cb_param);
        uint alternativeLighting = param3DnnDrawMotion.use_light_flag & 98304U;
        if (command.command_id == OBD_DRAW_USER_COMMAND_3DNN_DRAW_MOTION)
            nnDrawObject(amsParamDrawMotion._object, pltMtx, nstat, (uint)((int)amsParamDrawMotion.sub_obj_type | 256 | 512 | 7), amsParamDrawMotion.flag | drawflag | amDrawGetState().drawflag, alternativeLighting);
        else
            nnDrawMaterialMotionObject(amsParamDrawMotion._object, pltMtx, nstat, (uint)((int)amsParamDrawMotion.sub_obj_type | 256 | 512 | 7), amsParamDrawMotion.flag | drawflag | amDrawGetState().drawflag);
        if (param3DnnDrawMotion.material_cb_func != null)
            objDraw3DNNSetMaterialCallback(null, null);
        if (param3DnnDrawMotion.state != null)
            amDrawPopState();
        if (((int)g_obj.def_user_light_flag ^ (int)param3DnnDrawMotion.use_light_flag) != 0)
            objDrawSetDefaultLight();
        amMatrixPop();
    }

    private static void objDraw3DNNModelCommandSortFunc(
      AMS_COMMAND_HEADER command,
      uint drawflag)
    {
        obj_draw_user_command_sort_func_tbl[command.command_id](command, drawflag);
    }

    private static void objDraw3DNNSortModel_DT(AMS_COMMAND_HEADER command, uint drawflag)
    {
        amMatrixPush();
        OBS_DRAW_PARAM_3DNN_SORT_MODEL param3DnnSortModel = (OBS_DRAW_PARAM_3DNN_SORT_MODEL)command.param;
        if (((int)g_obj.def_user_light_flag ^ (int)param3DnnSortModel.use_light_flag) != 0)
            objDrawSetDrawLight(param3DnnSortModel.use_light_flag);
        if (param3DnnSortModel.user_func != null)
            param3DnnSortModel.user_func(param3DnnSortModel.user_param);
        AMS_PARAM_SORT_DRAW_OBJECT paramSortDrawObject = param3DnnSortModel.param;
        AMS_PARAM_DRAW_OBJECT drawObject = paramSortDrawObject.draw_object;
        if (drawObject.texlist != null)
            nnSetTextureList(drawObject.texlist);
        if (paramSortDrawObject.draw_state != null)
        {
            amDrawPushState();
            amDrawSetState(paramSortDrawObject.draw_state);
        }
        objDraw3DNNSetMaterialCallback(param3DnnSortModel.material_cb_func, param3DnnSortModel.material_cb_param);
        if (command.command_id == OBD_DRAW_USER_COMMAND_SORT_3DNN_MODEL)
            nnDrawObject(drawObject._object, paramSortDrawObject.mtx, paramSortDrawObject.nstat_list, (uint)((int)drawObject.sub_obj_type | 256 | 512 | 2), drawObject.flag | paramSortDrawObject.drawflag | amDrawGetState().drawflag, 0U);
        else
            nnDrawMaterialMotionObject(drawObject._object, paramSortDrawObject.mtx, paramSortDrawObject.nstat_list, (uint)((int)drawObject.sub_obj_type | 256 | 512 | 2), drawObject.flag | paramSortDrawObject.drawflag | amDrawGetState().drawflag);
        objDraw3DNNSetMaterialCallback(null, null);
        if (paramSortDrawObject.draw_state != null)
            amDrawPopState();
        if (((int)g_obj.def_user_light_flag ^ (int)param3DnnSortModel.use_light_flag) != 0)
            objDrawSetDefaultLight();
        amMatrixPop();
    }

    private static void objDraw3DNNSetMaterialCallback(
      MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE cb_func,
      object cb_param)
    {
        obj_draw_material_cb_func = cb_func;
        obj_draw_material_cb_param = cb_param;
        if (cb_func != null)
            nnSetMaterialCallback(_objDraw3DNNMaterialCallback);
        else
            nnSetMaterialCallback(null);
    }

    public static int objDraw3DNNMaterialCallback(NNS_DRAWCALLBACK_VAL draw_cb_val)
    {
        if (obj_draw_material_cb_func == null)
            return nnPutMaterialCore(draw_cb_val);
        return !obj_draw_material_cb_func(draw_cb_val, obj_draw_material_cb_param) ? 0 : 1;
    }

    public static void objDrawSetDrawLight(uint use_light_flag)
    {
        for (int no = 0; no < NNE_LIGHT_MAX; ++no)
        {
            if ((use_light_flag & ((uint)(1 << no))) != 0)
            {
                nnSetLight(no, g_obj.light[no].light_param, g_obj.light[no].light_type);
                nnSetLightSwitch(no, 1);
            }
            else
                nnSetLightSwitch(no, 0);
        }
        nnPutLightSettings();
    }

    public static void objDrawSetDefaultLight()
    {
        for (int no = 0; no < NNE_LIGHT_MAX; ++no)
        {
            if ((g_obj.def_user_light_flag & (uint)(1 << no)) != 0)
            {
                nnSetLight(no, g_obj.light[no].light_param, g_obj.light[no].light_type);
                nnSetLightSwitch(no, 1);
            }
            else
                nnSetLightSwitch(no, 0);
        }
        nnPutLightSettings();
    }

    public static void objDraw3DESEffectServerMain(MTS_TASK_TCB tcb)
    {
        amEffectExecute();
    }

    public static void objDraw3DESMatrixPush_UserFunc(object param)
    {
        amMatrixPush();
        NNS_MATRIX current = amMatrixGetCurrent();
        nnMultiplyMatrix(current, current, (NNS_MATRIX)param);
        nnMultiplyMatrix(ref tempSNNS_MATRIX0, amDrawGetWorldViewMatrix(), current);
        nnSetPrimitive3DMatrix(ref tempSNNS_MATRIX0);
    }

    public static void objDraw3DESMatrixPop_UserFunc(object param)
    {
        amMatrixPop();
    }

    public static void objDraw2DAMAPre_DT(object param)
    {
        AoActDrawPre();
    }

}