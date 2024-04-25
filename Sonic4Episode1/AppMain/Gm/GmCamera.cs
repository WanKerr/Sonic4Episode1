using System;

public partial class AppMain
{
    private static void GmCameraInit()
    {
        NNS_VECTOR pos1 = new NNS_VECTOR(177f, -15800f, 50f);
        ObjCameraInit(0, pos1, 4, 0, 8192);
        ObjCamera3dInit(0);
        g_obj.glb_camera_id = 0;
        g_obj.glb_camera_type = 1;
        GmCameraDelayReset();
        GmCameraAllowReset();
        ObjCameraSetUserFunc(0, new OBJF_CAMERA_USER_FUNC(GmCameraFunc));
        OBS_CAMERA obsCamera1 = ObjCameraGet(0);
        obsCamera1.scale = GMD_CAMERA_SCALE;
        obsCamera1.ofst.z = 1000f;
        gm_camera_vibration.x = 0.0f;
        gm_camera_vibration.y = 0.0f;
        gm_camera_vibration.z = 0.0f;
        gm_camera_work.flag = 0;
        gm_camera_work.timer = 0;
        gm_camera_work.offset = 0;
        gm_camera_work.scale_now = 1f;
        gm_camera_work.scale_target = 1f;
        gm_camera_work.scale_spd = 0.1f;
        int num = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        switch (num)
        {
            case 0:
            case 1:
            case 2:
            case 4:
            case 5:
                ObjCameraInit(1, new NNS_VECTOR(0.0f, 0.0f, 60f), 4, 0, 8192);
                ObjCamera3dInit(1);
                ObjCameraSetUserFunc(1, new OBJF_CAMERA_USER_FUNC(gmCameraFuncMapFar));
                OBS_CAMERA obsCamera2 = ObjCameraGet(1);
                obsCamera2.command_state = 1U;
                obsCamera2.fovy = 2 != num ? NNM_DEGtoA32(40f) : NNM_DEGtoA32(40f);
                obsCamera2.znear = 0.1f;
                obsCamera2.zfar = 32768f;
                break;
        }
        NNS_VECTOR pos2 = new NNS_VECTOR(177f, -15800f, 50f);
        int[] numArray1 = new int[4] { 2, 3, 4, 5 };
        int[] numArray2 = new int[4] { 0, 4, 6, 8 };
        uint[] numArray3 = new uint[4] { 7U, 0U, 0U, 0U };
        for (int index = 0; index < 4; ++index)
        {
            if (g_gm_gamedat_map_set_add[numArray2[index]] != null)
            {
                ObjCameraInit(numArray1[index], pos2, 4, 0, 8192);
                ObjCamera3dInit(numArray1[index]);
                ObjCameraSetUserFunc(numArray1[index], new OBJF_CAMERA_USER_FUNC(gmCameraFuncAddMap));
                OBS_CAMERA obsCamera3 = ObjCameraGet(numArray1[index]);
                obsCamera3.scale = GMD_CAMERA_SCALE;
                obsCamera3.ofst.z = 1000f;
                obsCamera3.command_state = numArray3[index];
            }
        }
        ObjCameraInit(6, new NNS_VECTOR(pos1.x, pos1.y, pos1.z), 4, 0, 8192);
        ObjCamera3dInit(6);
        ObjCameraSetUserFunc(6, new OBJF_CAMERA_USER_FUNC(gmCameraFuncWater));
        OBS_CAMERA obsCamera4 = ObjCameraGet(6);
        obsCamera4.command_state = 9U;
        obsCamera4.scale = GMD_CAMERA_SCALE;
        obsCamera4.ofst.z = 1000f;
    }

    private static void GmCameraExit()
    {
    }

    public static void GmCameraPosSet(int pos_x, int pos_y, int pos_z)
    {
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        float num1 = GSD_DISP_WIDTH / 2 * obsCamera.scale;
        float num2 = GSD_DISP_HEIGHT / 2 * obsCamera.scale;
        float num3 = g_gm_main_system.map_fcol.bottom - GSD_DISP_HEIGHT / 2 * obsCamera.scale;
        obsCamera.pos.x = FXM_FX32_TO_FLOAT(pos_x);
        if (obsCamera.pos.x < (double)num1)
            obsCamera.pos.x = num1;
        obsCamera.pos.y = -FXM_FX32_TO_FLOAT(pos_y);
        if (obsCamera.pos.y > -(double)num2)
            obsCamera.pos.y = -num2;
        if (obsCamera.pos.y > (double)num3)
            obsCamera.pos.y = num3;
        obsCamera.pos.z = FXM_FX32_TO_FLOAT(pos_z) + 50f;
        obsCamera.disp_pos.x = obsCamera.pos.x;
        obsCamera.disp_pos.y = obsCamera.pos.y;
        obsCamera.disp_pos.z = obsCamera.pos.z;
    }

    private static void GmCameraVibrationSet(int vib_x, int vib_y, int vib_z)
    {
        gm_camera_vibration.x = FXM_FX32_TO_FLOAT(vib_x);
        gm_camera_vibration.y = FXM_FX32_TO_FLOAT(vib_y);
        gm_camera_vibration.z = FXM_FX32_TO_FLOAT(vib_z);
    }

    private static void GmCameraAllowSet(float alw_x, float alw_y, float alw_z)
    {
        gm_camera_option_allow_pos.x = alw_x;
        gm_camera_option_allow_pos.y = alw_y;
        gm_camera_option_allow_pos.z = alw_z;
        ObjCameraAllowSet(0, gm_camera_option_allow_pos);
    }

    private static void GmCameraAllowReset()
    {
        if (!GSM_MAIN_STAGE_IS_SPSTAGE())
            ObjCameraAllowSet(0, gm_camera_common_allow_pos);
        else
            ObjCameraAllowSet(0, gm_camera_splstg_allow_pos);
    }

    private static void GmCameraDelaySet(float dly_x, float dly_y, float dly_z)
    {
        NNS_VECTOR ofst = GlobalPool<NNS_VECTOR>.Alloc();
        ofst.x = dly_x;
        ofst.y = dly_y;
        ofst.z = dly_z;
        ObjCameraPlaySet(0, ofst);
        GlobalPool<NNS_VECTOR>.Release(ofst);
    }

    private static void GmCameraDelayReset()
    {
        NNS_VECTOR ofst1 = new NNS_VECTOR(50f, 50f, 0.0f);
        NNS_VECTOR ofst2 = new NNS_VECTOR(0.0f, 0.0f, 0.0f);
        if (!GSM_MAIN_STAGE_IS_SPSTAGE())
            ObjCameraPlaySet(0, ofst1);
        else
            ObjCameraPlaySet(0, ofst2);
    }

    private static void GmCameraScaleSet(float scale_target, float scale_spd)
    {
        gm_camera_work.scale_target = scale_target;
        gm_camera_work.scale_spd = scale_spd;
    }

    public static void GmCameraSetClipCamera(OBS_CAMERA obj_camera)
    {
        ObjObjectClipCameraSet(g_obj.camera[0][0], g_obj.camera[0][1]);
    }

    private static void GmCameraFunc(OBS_CAMERA obj_camera)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        SNNS_VECTOR snnsVector1;
        snnsVector1.x = FXM_FX32_TO_FLOAT(gmsPlayerWork.obj_work.pos.x);
        snnsVector1.z = FXM_FX32_TO_FLOAT(gmsPlayerWork.obj_work.pos.z);
        snnsVector1.y = GSM_MAIN_STAGE_IS_SPSTAGE() ? FXM_FX32_TO_FLOAT(-gmsPlayerWork.obj_work.pos.y + 8192) : FXM_FX32_TO_FLOAT(-gmsPlayerWork.obj_work.pos.y + 24576);
        if ((gmsPlayerWork.player_flag & GMD_PLF_DIE) != 0 && (gmsPlayerWork.player_flag & GMD_PLF_TATK_RETRY) == 0)
        {
            snnsVector1.x = obj_camera.work.x;
            snnsVector1.y = obj_camera.work.y;
            snnsVector1.z = obj_camera.work.z;
        }
        else
        {
            obj_camera.work.x = snnsVector1.x;
            obj_camera.work.y = snnsVector1.y;
            obj_camera.work.z = snnsVector1.z;
        }
        obj_camera.prev_pos.x = obj_camera.pos.x;
        obj_camera.prev_pos.y = obj_camera.pos.y;
        obj_camera.prev_pos.z = obj_camera.pos.z;
        if ((obj_camera.flag & 1) != 0)
        {
            if (obj_camera.target_obj == null)
                return;
            obj_camera.pos.x = snnsVector1.x;
            obj_camera.pos.y = snnsVector1.y;
            obj_camera.pos.z = snnsVector1.z;
        }
        else
        {
            gmCameraScaleChange(obj_camera);
            if ((obj_camera.flag & 2) != 0)
            {
                obj_camera.pos.x = ObjShiftSetF(obj_camera.pos.x, snnsVector1.x, obj_camera.shift, obj_camera.spd_max.x, obj_camera.spd_add.x);
                obj_camera.pos.y = ObjShiftSetF(obj_camera.pos.y, snnsVector1.x, obj_camera.shift, obj_camera.spd_max.y, obj_camera.spd_add.y);
                obj_camera.pos.z = ObjShiftSetF(obj_camera.pos.z, snnsVector1.x, obj_camera.shift, obj_camera.spd_max.z, obj_camera.spd_add.z);
            }
            else
            {
                obj_camera.allow.x = obj_camera.allow_limit.x;
                obj_camera.allow.z = obj_camera.allow_limit.z;
                obj_camera.allow.y = (gmsPlayerWork.obj_work.move_flag & 16) == 0 ? Math.Max(obj_camera.allow.y - 4f, 0.0f) : obj_camera.allow_limit.y;
                if (snnsVector1.x < obj_camera.pos.x - (double)obj_camera.allow.x)
                    snnsVector1.x += obj_camera.allow.x;
                else if (snnsVector1.x > obj_camera.pos.x + (double)obj_camera.allow.x)
                    snnsVector1.x -= obj_camera.allow.x;
                else
                    snnsVector1.x = obj_camera.pos.x;
                if (snnsVector1.y < obj_camera.pos.y - (double)obj_camera.allow.y)
                    snnsVector1.y += obj_camera.allow.y;
                else if (snnsVector1.y > obj_camera.pos.y + (double)obj_camera.allow.y)
                    snnsVector1.y -= obj_camera.allow.y;
                else
                    snnsVector1.y = obj_camera.pos.y;
                obj_camera.spd.x = snnsVector1.x == (double)obj_camera.pos.x ? ObjSpdDownSetF(obj_camera.spd.x, obj_camera.spd_add.x) : ObjSpdUpSetF(obj_camera.spd.x, obj_camera.spd_add.x, obj_camera.spd_max.x);
                obj_camera.spd.y = snnsVector1.y == (double)obj_camera.pos.y ? ObjSpdDownSetF(obj_camera.spd.y, obj_camera.spd_add.y) : ObjSpdUpSetF(obj_camera.spd.y, obj_camera.spd_add.y, obj_camera.spd_max.y);
                obj_camera.spd.z = snnsVector1.z == (double)obj_camera.pos.z ? ObjSpdDownSetF(obj_camera.spd.z, obj_camera.spd_add.z) : ObjSpdUpSetF(obj_camera.spd.z, obj_camera.spd_add.z, obj_camera.spd_max.z);
                if (snnsVector1.x > (double)obj_camera.pos.x)
                {
                    obj_camera.pos.x += obj_camera.spd.x;
                    if (obj_camera.pos.x > (double)snnsVector1.x)
                        obj_camera.pos.x = snnsVector1.x;
                }
                else
                {
                    obj_camera.pos.x -= obj_camera.spd.x;
                    if (obj_camera.pos.x < (double)snnsVector1.x)
                        obj_camera.pos.x = snnsVector1.x;
                }
                if (snnsVector1.y > (double)obj_camera.pos.y)
                {
                    obj_camera.pos.y += obj_camera.spd.y;
                    if (obj_camera.pos.y > (double)snnsVector1.y)
                        obj_camera.pos.y = snnsVector1.y;
                }
                else
                {
                    obj_camera.pos.y -= obj_camera.spd.y;
                    if (obj_camera.pos.y < (double)snnsVector1.y)
                        obj_camera.pos.y = snnsVector1.y;
                }
                if (snnsVector1.z > (double)obj_camera.pos.z)
                {
                    obj_camera.pos.z += obj_camera.spd.z;
                    if (obj_camera.pos.z > (double)snnsVector1.z)
                        obj_camera.pos.z = snnsVector1.z;
                }
                else
                {
                    obj_camera.pos.z -= obj_camera.spd.z;
                    if (obj_camera.pos.z < (double)snnsVector1.z)
                        obj_camera.pos.z = snnsVector1.z;
                }
            }
            if (MTM_MATH_ABS(snnsVector1.z - obj_camera.pos.z) > (double)obj_camera.play_ofst_max.z)
                obj_camera.pos.z = snnsVector1.z <= (double)obj_camera.pos.z ? snnsVector1.z + obj_camera.play_ofst_max.z : snnsVector1.z - obj_camera.play_ofst_max.z;
            obj_camera.disp_ofst.x = gm_camera_vibration.x / 16f;
            obj_camera.disp_ofst.y = gm_camera_vibration.y / 16f;
            obj_camera.disp_ofst.z = gm_camera_vibration.z / 16f;
            gm_camera_vibration.x = gmCameraVibCheck(gm_camera_vibration.x);
            gm_camera_vibration.y = gmCameraVibCheck(gm_camera_vibration.y);
            gm_camera_vibration.z = gmCameraVibCheck(gm_camera_vibration.z);
            obj_camera.pos.z = 50f;
            SNNS_VECTOR snnsVector2;
            snnsVector2.x = obj_camera.ofst.x;
            snnsVector2.y = obj_camera.ofst.y;
            snnsVector2.z = obj_camera.ofst.z;
            if ((double)obj_camera.scale < GMD_CAMERA_SCALE)
            {
                float num = (obj_camera.scale - 0.3371917f) / 0.3371917f;
                snnsVector2.x *= num;
                snnsVector2.y *= num;
            }
            obj_camera.disp_pos.x = obj_camera.pos.x + snnsVector2.x;
            obj_camera.disp_pos.y = obj_camera.pos.y + snnsVector2.y;
            obj_camera.disp_pos.z = obj_camera.pos.z + snnsVector2.z;
            gmCameraLookupCheck(obj_camera);
            float num1 = g_gm_main_system.map_fcol.left + GSD_DISP_WIDTH / 2 * obj_camera.scale;
            float num2 = g_gm_main_system.map_fcol.right - GSD_DISP_WIDTH / 2 * obj_camera.scale;
            float num3 = g_gm_main_system.map_fcol.top + GSD_DISP_HEIGHT / 2 * obj_camera.scale;
            float num4 = g_gm_main_system.map_fcol.bottom - GSD_DISP_HEIGHT / 2 * obj_camera.scale;
            if (obj_camera.disp_pos.x < (double)num1)
            {
                obj_camera.disp_pos.x = num1;
                if (obj_camera.disp_pos.x > (double)num2)
                    obj_camera.disp_pos.x = (float)((num2 + (double)num1) / 2.0);
            }
            else if (obj_camera.disp_pos.x > (double)num2)
            {
                obj_camera.disp_pos.x = num2;
                if (obj_camera.disp_pos.x < (double)num1)
                    obj_camera.disp_pos.x = (float)((num2 + (double)num1) / 2.0);
            }
            if (obj_camera.disp_pos.y > -(double)num3)
                obj_camera.disp_pos.y = -num3;
            if (obj_camera.disp_pos.y < -(double)num4)
                obj_camera.disp_pos.y = -num4;
            obj_camera.target_pos.Assign(obj_camera.disp_pos);
            obj_camera.target_pos.z -= 50f;
            if (!GSM_MAIN_STAGE_IS_SPSTAGE())
            {
                var loopCamera = gs.backup.SSave.CreateInstance().GetRemaster().LoopCamera;
                if (((int)obj_camera.flag & int.MinValue) != 0 && ((int)gmsPlayerWork.obj_work.move_flag & 1) != 0 && loopCamera)
                {
                    short num5 = (short)-gmsPlayerWork.obj_work.dir.z;
                    short roll = (short)obj_camera.roll;
                    short num6 = (ushort)num5 <= 16384 || (ushort)num5 >= 49152 ? (short)((num5 + roll) / 2) : (short)(ushort)(((ushort)num5 + (uint)(ushort)roll) / 2U);
                    obj_camera.roll = num6;
                    obj_camera.flag &= int.MaxValue;
                }
                else
                {
                    obj_camera.roll = obj_camera.roll * 9 / 10;
                    obj_camera.flag &= int.MaxValue;
                }
            }
            ObjObjectCameraSet(FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - OBD_LCD_Y / 2), FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - OBD_LCD_Y / 2));
            GmCameraSetClipCamera(obj_camera);
        }
    }

    private static void gmCameraFuncMapFar(OBS_CAMERA obj_camera)
    {
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        obj_camera.pos.Assign(GmMapFarGetCameraPos(obsCamera.disp_pos));
        obj_camera.target_pos.Assign(GmMapFarGetCameraTarget(obj_camera.pos));
        obj_camera.disp_pos.Assign(obj_camera.pos);
        if (GSM_MAIN_STAGE_IS_SPSTAGE())
            return;
        obj_camera.roll = obsCamera.roll;
    }

    private static void gmCameraFuncAddMap(OBS_CAMERA obj_camera)
    {
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        obj_camera.prev_pos.Assign(obj_camera.pos);
        obj_camera.prev_disp_pos.Assign(obj_camera.disp_pos);
        obj_camera.disp_pos.Assign(obsCamera.disp_pos);
        obj_camera.pos.Assign(obsCamera.pos);
        obj_camera.ofst.Assign(obsCamera.ofst);
        obj_camera.disp_ofst.Assign(obsCamera.disp_ofst);
        obj_camera.target_ofst.Assign(obsCamera.target_ofst);
        obj_camera.play_ofst_max.Assign(obsCamera.play_ofst_max);
        obj_camera.camup_obj = obsCamera.camup_obj;
        obj_camera.camup_pos.Assign(obsCamera.camup_pos);
        obj_camera.roll = obsCamera.roll;
        obj_camera.up_vec.Assign(obsCamera.up_vec);
        obj_camera.scale = obsCamera.scale;
        obj_camera.left = obsCamera.left;
        obj_camera.right = obsCamera.right;
        obj_camera.bottom = obsCamera.bottom;
        obj_camera.top = obsCamera.top;
        obj_camera.znear = obsCamera.znear;
        obj_camera.zfar = obsCamera.zfar;
        obj_camera.aspect = obsCamera.aspect;
        GmMapGetAddMapCameraPos(obsCamera.disp_pos, obsCamera.target_pos, obj_camera.disp_pos, obj_camera.target_pos, obj_camera.camera_id);
    }

    private static void gmCameraFuncWater(OBS_CAMERA obj_camera)
    {
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        nnCopyVector(obj_camera.pos, obsCamera.pos);
        nnCopyVector(obj_camera.target_pos, obsCamera.target_pos);
        nnCopyVector(obj_camera.disp_pos, obsCamera.disp_pos);
    }

    private static void GmCameraTruckFunc(OBS_CAMERA obj_camera)
    {
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if ((gmsPlayerWork.player_flag & GMD_PLF_DIE) != 0 && (gmsPlayerWork.player_flag & GMD_PLF_TATK_RETRY) == 0)
        {
            GlobalPool<NNS_VECTOR>.Release(nnsVector1);
        }
        else
        {
            nnsVector1.x = FXM_FX32_TO_FLOAT(gmsPlayerWork.obj_work.pos.x);
            nnsVector1.y = FXM_FX32_TO_FLOAT(-gmsPlayerWork.obj_work.pos.y + 24576);
            nnsVector1.z = FXM_FX32_TO_FLOAT(gmsPlayerWork.obj_work.pos.z);
            obj_camera.work.x = nnsVector1.x;
            obj_camera.work.y = nnsVector1.y;
            obj_camera.work.z = nnsVector1.z;
            obj_camera.prev_pos.x = obj_camera.pos.x;
            obj_camera.prev_pos.y = obj_camera.pos.y;
            obj_camera.prev_pos.z = obj_camera.pos.z;
            if (((int)obj_camera.flag & 1) != 0)
            {
                if (obj_camera.target_obj != null)
                {
                    obj_camera.pos.x = nnsVector1.x;
                    obj_camera.pos.y = nnsVector1.y;
                    obj_camera.pos.z = nnsVector1.z;
                }
                GlobalPool<NNS_VECTOR>.Release(nnsVector1);
            }
            else
            {
                if (((int)obj_camera.flag & 2) != 0)
                {
                    obj_camera.pos.x = ObjShiftSetF(obj_camera.pos.x, nnsVector1.x, obj_camera.shift, obj_camera.spd_max.x, obj_camera.spd_add.x);
                    obj_camera.pos.y = ObjShiftSetF(obj_camera.pos.y, nnsVector1.x, obj_camera.shift, obj_camera.spd_max.y, obj_camera.spd_add.y);
                    obj_camera.pos.z = ObjShiftSetF(obj_camera.pos.z, nnsVector1.x, obj_camera.shift, obj_camera.spd_max.z, obj_camera.spd_add.z);
                }
                else
                {
                    obj_camera.allow.x = obj_camera.allow_limit.x;
                    obj_camera.allow.z = obj_camera.allow_limit.z;
                    obj_camera.allow.y = ((int)gmsPlayerWork.obj_work.move_flag & 16) == 0 ? Math.Max(obj_camera.allow.y - 4f, 0.0f) : obj_camera.allow_limit.y;
                    if (nnsVector1.x < obj_camera.pos.x - (double)obj_camera.allow.x)
                        nnsVector1.x += obj_camera.allow.x;
                    else if (nnsVector1.x > obj_camera.pos.x + (double)obj_camera.allow.x)
                        nnsVector1.x -= obj_camera.allow.x;
                    else
                        nnsVector1.x = obj_camera.pos.x;
                    if (nnsVector1.y < obj_camera.pos.y - (double)obj_camera.allow.y)
                        nnsVector1.y += obj_camera.allow.y;
                    else if (nnsVector1.y > obj_camera.pos.y + (double)obj_camera.allow.y)
                        nnsVector1.y -= obj_camera.allow.y;
                    else
                        nnsVector1.y = obj_camera.pos.y;
                    obj_camera.spd.x = nnsVector1.x == (double)obj_camera.pos.x ? ObjSpdDownSetF(obj_camera.spd.x, obj_camera.spd_add.x) : ObjSpdUpSetF(obj_camera.spd.x, obj_camera.spd_add.x, obj_camera.spd_max.x);
                    obj_camera.spd.y = nnsVector1.y == (double)obj_camera.pos.y ? ObjSpdDownSetF(obj_camera.spd.y, obj_camera.spd_add.y) : ObjSpdUpSetF(obj_camera.spd.y, obj_camera.spd_add.y, obj_camera.spd_max.y);
                    obj_camera.spd.z = nnsVector1.z == (double)obj_camera.pos.z ? ObjSpdDownSetF(obj_camera.spd.z, obj_camera.spd_add.z) : ObjSpdUpSetF(obj_camera.spd.z, obj_camera.spd_add.z, obj_camera.spd_max.z);
                    if (nnsVector1.x > (double)obj_camera.pos.x)
                    {
                        obj_camera.pos.x += obj_camera.spd.x;
                        if (obj_camera.pos.x > (double)nnsVector1.x)
                            obj_camera.pos.x = nnsVector1.x;
                    }
                    else
                    {
                        obj_camera.pos.x -= obj_camera.spd.x;
                        if (obj_camera.pos.x < (double)nnsVector1.x)
                            obj_camera.pos.x = nnsVector1.x;
                    }
                    if (nnsVector1.y > (double)obj_camera.pos.y)
                    {
                        obj_camera.pos.y += obj_camera.spd.y;
                        if (obj_camera.pos.y > (double)nnsVector1.y)
                            obj_camera.pos.y = nnsVector1.y;
                    }
                    else
                    {
                        obj_camera.pos.y -= obj_camera.spd.y;
                        if (obj_camera.pos.y < (double)nnsVector1.y)
                            obj_camera.pos.y = nnsVector1.y;
                    }
                    if (nnsVector1.z > (double)obj_camera.pos.z)
                    {
                        obj_camera.pos.z += obj_camera.spd.z;
                        if (obj_camera.pos.z > (double)nnsVector1.z)
                            obj_camera.pos.z = nnsVector1.z;
                    }
                    else
                    {
                        obj_camera.pos.z -= obj_camera.spd.z;
                        if (obj_camera.pos.z < (double)nnsVector1.z)
                            obj_camera.pos.z = nnsVector1.z;
                    }
                }
                if (MTM_MATH_ABS(nnsVector1.z - obj_camera.pos.z) > (double)obj_camera.play_ofst_max.z)
                    obj_camera.pos.z = nnsVector1.z <= (double)obj_camera.pos.z ? nnsVector1.z + obj_camera.play_ofst_max.z : nnsVector1.z - obj_camera.play_ofst_max.z;
                obj_camera.disp_ofst.x = gm_camera_vibration.x / 16f;
                obj_camera.disp_ofst.y = gm_camera_vibration.y / 16f;
                obj_camera.disp_ofst.z = gm_camera_vibration.z / 16f;
                gm_camera_vibration.x = gmCameraVibCheck(gm_camera_vibration.x);
                gm_camera_vibration.y = gmCameraVibCheck(gm_camera_vibration.y);
                gm_camera_vibration.z = gmCameraVibCheck(gm_camera_vibration.z);
                obj_camera.pos.z = 50f;
                NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
                nnsVector2.x = obj_camera.ofst.x;
                nnsVector2.y = obj_camera.ofst.y;
                nnsVector2.z = obj_camera.ofst.z;
                if ((double)obj_camera.scale < GMD_CAMERA_SCALE)
                {
                    float num = (obj_camera.scale - 0.3371917f) / 0.3371917f;
                    nnsVector2.x *= num;
                    nnsVector2.y *= num;
                }
                obj_camera.disp_pos.x = obj_camera.pos.x + nnsVector2.x;
                obj_camera.disp_pos.y = obj_camera.pos.y + nnsVector2.y;
                obj_camera.disp_pos.z = obj_camera.pos.z + nnsVector2.z;
                GlobalPool<NNS_VECTOR>.Release(nnsVector2);
                gmCameraLookupCheck(obj_camera);
                float num1 = g_gm_main_system.map_fcol.left + GSD_DISP_WIDTH / 2 * obj_camera.scale;
                float num2 = g_gm_main_system.map_fcol.right - GSD_DISP_WIDTH / 2 * obj_camera.scale;
                float num3 = g_gm_main_system.map_fcol.top + GSD_DISP_HEIGHT / 2 * obj_camera.scale;
                float num4 = g_gm_main_system.map_fcol.bottom - GSD_DISP_HEIGHT / 2 * obj_camera.scale;
                if (obj_camera.disp_pos.x < (double)num1)
                    obj_camera.disp_pos.x = num1;
                if (obj_camera.disp_pos.x > (double)num2)
                    obj_camera.disp_pos.x = num2;
                if (obj_camera.disp_pos.y > -(double)num3)
                    obj_camera.disp_pos.y = -num3;
                if (obj_camera.disp_pos.y < -(double)num4)
                    obj_camera.disp_pos.y = -num4;
                obj_camera.target_pos.Assign(obj_camera.disp_pos);
                obj_camera.target_pos.z -= 50f;
                int num5 = 4608;
                if (((int)gmsPlayerWork.gmk_flag2 & 32) != 0)
                    num5 = 672;
                int a1 = -gmsPlayerWork.ply_pseudofall_dir - obj_camera.roll;
                if (a1 > 32768)
                    a1 -= 65536;
                else if (a1 < short.MinValue)
                    a1 += 65536;
                int a2;
                if (MTM_MATH_ABS(a1) < 128)
                {
                    a2 = a1;
                }
                else
                {
                    a2 = a1 >> 4;
                    if (MTM_MATH_ABS(a2) > 3328)
                        a2 += a2 - 3328;
                    if (MTM_MATH_ABS(a2) > num5)
                        a2 = a2 < 0 ? -num5 : num5;
                    else if (MTM_MATH_ABS(a2) < 128)
                        a2 = a2 < 0 ? sbyte.MinValue : 128;
                }
                obj_camera.roll += a2;
                if (obj_camera.roll > 65536)
                {
                    obj_camera.roll -= 65536;
                    gmsPlayerWork.ply_pseudofall_dir += 65536;
                }
                else if (obj_camera.roll < -65536)
                {
                    obj_camera.roll += 65536;
                    gmsPlayerWork.ply_pseudofall_dir -= 65536;
                }
                ObjObjectCameraSet(FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - OBD_LCD_Y / 2), FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - OBD_LCD_Y / 2));
                GmCameraSetClipCamera(obj_camera);
                GlobalPool<NNS_VECTOR>.Release(nnsVector1);
            }
        }
    }

    private static float gmCameraVibCheck(float vib)
    {
        if (vib != 0.0)
        {
            if (vib > 0.0)
            {
                vib = (float)(0.0 - (vib - 1.0));
                if (vib > 0.0)
                    vib = 0.0f;
            }
            else
            {
                vib = (float)(0.0 - (vib + 1.0));
                if (vib < 0.0)
                    vib = 0.0f;
            }
        }
        return vib;
    }

    private static void gmCameraLookupCheck(OBS_CAMERA obj_camera)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        int num1 = 0;
        if (GSM_MAIN_STAGE_IS_SPSTAGE())
            return;
        if (gmsPlayerWork.seq_state == 4 && (gmsPlayerWork.key_on & 1) != 0)
            num1 = 1;
        if (gmsPlayerWork.seq_state == 7 && (gmsPlayerWork.key_on & 2) != 0)
            num1 = 2;
        if ((gm_camera_work.flag & num1) != 0 && gm_camera_work.timer > 90)
        {
            if ((gm_camera_work.flag & 1) != 0)
                gm_camera_work.offset += 2;
            else
                gm_camera_work.offset -= 2;
            int num2 = gmsPlayerWork.camera_ofst_y >> 12;
            gm_camera_work.offset = MTM_MATH_CLIP(gm_camera_work.offset, -96 - num2, 80 - num2);
        }
        else
        {
            if ((gm_camera_work.flag & num1) != 0)
                ++gm_camera_work.timer;
            else if (num1 != 0)
            {
                gm_camera_work.flag = num1;
                gm_camera_work.timer = 1;
            }
            else
            {
                gm_camera_work.flag = 0;
                gm_camera_work.timer = 0;
            }
            if (gm_camera_work.offset > 0)
            {
                gm_camera_work.offset -= 2;
                if (gm_camera_work.offset < 0)
                    gm_camera_work.offset = 0;
            }
            else if (gm_camera_work.offset < 0)
            {
                gm_camera_work.offset += 2;
                if (gm_camera_work.offset > 0)
                    gm_camera_work.offset = 0;
            }
        }
        switch ((ushort)(obj_camera.roll + 8192 >> 14))
        {
            case 0:
                obj_camera.disp_pos.y += gm_camera_work.offset;
                break;
            case 1:
                obj_camera.disp_pos.x -= gm_camera_work.offset;
                break;
            case 2:
                obj_camera.disp_pos.y -= gm_camera_work.offset;
                break;
            case 3:
                obj_camera.disp_pos.x += gm_camera_work.offset;
                break;
        }
    }

    private static void gmCameraScaleChange(OBS_CAMERA obj_camera)
    {
        if (gm_camera_work.scale_now == (double)gm_camera_work.scale_target)
            return;
        if (gm_camera_work.scale_now < (double)gm_camera_work.scale_target)
        {
            gm_camera_work.scale_now += gm_camera_work.scale_spd;
            if (gm_camera_work.scale_now > (double)gm_camera_work.scale_target)
                gm_camera_work.scale_now = gm_camera_work.scale_target;
        }
        else if (gm_camera_work.scale_now > (double)gm_camera_work.scale_target)
        {
            gm_camera_work.scale_now -= gm_camera_work.scale_spd;
            if (gm_camera_work.scale_now < (double)gm_camera_work.scale_target)
                gm_camera_work.scale_now = gm_camera_work.scale_target;
        }
        obj_camera.scale = GMD_CAMERA_SCALE / gm_camera_work.scale_now;
    }

}