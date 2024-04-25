public partial class AppMain
{
    public static void GmMapFarInitData(AMS_AMB_HEADER amb)
    {
        gmMapFarDataInit();
        gmMapFarDataSetAmbHeader(amb);
    }

    private static void GmMapFarBuildData()
    {
        switch (gmMapFarGetZoneType(gmMapFarGetStageId()))
        {
            case 0:
                gmMapFarZone1Build();
                break;
            case 1:
                gmMapFarZone2Build();
                break;
            case 2:
                gmMapFarZone3Build();
                break;
            case 3:
                gmMapFarZone4Build();
                break;
            case 4:
                gmMapFarZoneFinalBuild();
                break;
            case 5:
                gmMapFarZoneSSBuild();
                break;
        }
    }

    private static bool GmMapFarCheckLoading()
    {
        switch (gmMapFarGetZoneType(gmMapFarGetStageId()))
        {
            case 0:
                return gmMapFarZone1CheckLoading();
            case 1:
                return gmMapFarZone2CheckLoading();
            case 2:
                return gmMapFarZone3CheckLoading();
            case 3:
                return gmMapFarZone4CheckLoading();
            case 4:
                return gmMapFarZoneFinalCheckLoading();
            case 5:
                return gmMapFarZoneSSCheckLoading();
            default:
                return true;
        }
    }

    private static void GmMapFarInit()
    {
        int zoneType = gmMapFarGetZoneType(gmMapFarGetStageId());
        gmMapFarInitMgr();
        gmMapFarCreateTcbPreDraw();
        gmMapFarCreateTcbDraw();
        gmMapFarCreateTcbPostDraw();
        switch (zoneType)
        {
            case 0:
                gmMapFarZone1Init();
                break;
            case 1:
                gmMapFarZone2Init();
                break;
            case 2:
                gmMapFarZone3Init();
                break;
            case 3:
                gmMapFarZone4Init();
                break;
            case 4:
                gmMapFarZoneFinalInit();
                break;
            case 5:
                gmMapFarZoneSSInit();
                break;
        }
    }

    private static void GmMapFarExit()
    {
        gmMapFarDataClearObjWork();
        gmMapFarExitMgr();
    }

    private static void GmMapFarFlushData()
    {
        switch (gmMapFarGetZoneType(gmMapFarGetStageId()))
        {
            case 0:
                gmMapFarZone1Flush();
                break;
            case 1:
                gmMapFarZone2Flush();
                break;
            case 2:
                gmMapFarZone3Flush();
                break;
            case 3:
                gmMapFarZone4Flush();
                break;
            case 4:
                gmMapFarZoneFinalFlush();
                break;
            case 5:
                gmMapFarZoneSSFlush();
                break;
        }
    }

    public static void GmMapFarRelease()
    {
        switch (gmMapFarGetZoneType(gmMapFarGetStageId()))
        {
            case 0:
                gmMapFarZone1Release();
                break;
            case 1:
                gmMapFarZone2Release();
                break;
            case 2:
                gmMapFarZone3Release();
                break;
            case 3:
                gmMapFarZone4Release();
                break;
            case 4:
                gmMapFarZoneFinalRelease();
                break;
            case 5:
                gmMapFarZoneSSRelease();
                break;
        }
        gmMapFarDataRelease();
        gmMapFarReleaseMgr();
    }

    private static NNS_VECTOR GmMapFarGetCameraPos(NNS_VECTOR player_camera_pos)
    {
        switch (gmMapFarGetZoneType(gmMapFarGetStageId()))
        {
            case 0:
                return gmMapFarZone1GetCameraPos(player_camera_pos);
            case 1:
                return gmMapFarZone2GetCameraPos(player_camera_pos);
            case 2:
                return gmMapFarZone3GetCameraPos(player_camera_pos);
            case 3:
                return player_camera_pos;
            case 4:
                return gmMapFarZoneFinalGetCameraPos(player_camera_pos);
            case 5:
                return gmMapFarZoneSSGetCameraPos(player_camera_pos);
            default:
                return player_camera_pos;
        }
    }

    private static NNS_VECTOR GmMapFarGetCameraTarget(NNS_VECTOR camera_pos)
    {
        int zoneType = gmMapFarGetZoneType(gmMapFarGetStageId());
        NNS_VECTOR cameraTargetResult = GmMapFarGetCameraTarget_result;
        nnAddVector(cameraTargetResult, camera_pos, g_gm_map_far_camera_target_offset[zoneType]);
        return cameraTargetResult;
    }

    private static int gmMapFarGetStageId()
    {
        return g_gs_main_sys_info.stage_id;
    }

    private static int gmMapFarGetZoneType(int stage_id)
    {
        return g_gm_gamedat_zone_type_tbl[stage_id];
    }

    private static MP_HEADER gmMapFarGetMapsetMpA()
    {
        return (MP_HEADER)g_gm_gamedat_map_set[0];
    }

    private static OBS_OBJECT gmMapFarGetObject()
    {
        return g_obj;
    }

    private static void gmMapFarClearColor()
    {
        int zoneType = gmMapFarGetZoneType(gmMapFarGetStageId());
        amDrawSetBGColor(g_gm_map_far_clear_color[zoneType]);
    }

    public static void gmMapFarDataInit()
    {
        int num1 = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        g_map_far_data_real.Clear();
        g_map_far_data = g_map_far_data_real;
        if (num1 == 0)
        {
            g_map_far_data.degSky = nnRandom() * 340f;
            g_map_far_data.degSky2 = nnRandom() * 340f;
        }
        else if (4 == num1)
        {
            float num2 = nnRandom();
            g_map_far_data.degSky = num2 >= 0.25 ? (num2 >= 0.5 ? (num2 >= 0.75 ? nnRandom() * 355f : (float)(20.0 + nnRandom() * 50.0)) : (float)(130.0 + nnRandom() * 50.0)) : (float)(260.0 + nnRandom() * 50.0);
            g_map_far_data.degSky2 = nnRandom() * 355f;
        }
        else
        {
            g_map_far_data.degSky = 90f;
            g_map_far_data.degSky2 = 90f;
        }
    }

    private static void gmMapFarDataRelease()
    {
        if (g_map_far_data == null)
            return;
        gmMapFarDataFreeNNModelWork();
        gmMapFarDataReleaseAmbHeader();
        g_map_far_data = null;
    }

    private static GMS_MAP_FAR_DATA gmMapFarDataGetInfo()
    {
        return g_map_far_data;
    }

    public static void gmMapFarDataSetAmbHeader(AMS_AMB_HEADER amb)
    {
        gmMapFarDataGetInfo().amb_header = amb;
    }

    private static AMS_AMB_HEADER gmMapFarDataGetAmbHeader()
    {
        return gmMapFarDataGetInfo().amb_header;
    }

    private static void gmMapFarDataReleaseAmbHeader()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        if (info.amb_header == null)
            return;
        info.amb_header = null;
    }

    private static GMS_MAP_FAR_OBJ_WORK gmMapFarDataLoadObj(
      GMD_MAP_FAR_OBJ_INDEX obj_index,
      OBS_ACTION3D_NN_WORK obj_3d_work,
      MPP_VOID_OBS_OBJECT_WORK main_func,
      MPP_VOID_OBS_OBJECT_WORK out_func)
    {
        GMS_MAP_FAR_OBJ_WORK gmsMapFarObjWork = (GMS_MAP_FAR_OBJ_WORK)OBM_OBJECT_TASK_DETAIL_INIT(12544, 5, 0, 0, () => new GMS_MAP_FAR_OBJ_WORK(), "MAP FAR OBJ");
        OBS_OBJECT_WORK objWork = gmsMapFarObjWork.obj_work;
        ObjObjectCopyAction3dNNModel(objWork, obj_3d_work, gmsMapFarObjWork.obj_3d);
        objWork.obj_type = 8;
        objWork.flag |= 18U;
        objWork.move_flag |= 768U;
        objWork.user_flag = (uint)obj_index;
        objWork.ppFunc = main_func;
        objWork.ppOut = out_func;
        mtTaskChangeTcbDestructor(objWork.tcb, new GSF_TASK_PROCEDURE(ObjObjectExit));
        gmMapFarDataSetObjWork(objWork, obj_index);
        return gmsMapFarObjWork;
    }

    private static void gmMapFarDataClearObjWork()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        for (int index = 0; index < 16; ++index)
            info.obj_work[index] = null;
    }

    private static void gmMapFarDataSetObjWork(
      OBS_OBJECT_WORK obj_work,
      GMD_MAP_FAR_OBJ_INDEX obj_index)
    {
        gmMapFarDataGetInfo().obj_work[(int)obj_index] = obj_work;
    }

    private static OBS_OBJECT_WORK gmMapFarDataGetObjWork(
      GMD_MAP_FAR_OBJ_INDEX obj_index)
    {
        return (GMD_MAP_FAR_OBJ_INDEX)16 <= obj_index ? null : gmMapFarDataGetInfo().obj_work[(int)obj_index];
    }

    private static void gmMapFarDataFreeNNModelWork()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        if (info.nn_work != null)
            info.nn_work = null;
        info.nn_work_num = 0;
        info.nn_regist_num = 0;
        info.mp_header = null;
        info.md_header = null;
    }

    private static GMS_MAP_FAR_MGR gmMapFarGetMgr()
    {
        return g_map_far_mgr;
    }

    private static void gmMapFarInitMgr()
    {
        g_map_far_mgr_real.Clear();
        g_map_far_mgr = g_map_far_mgr_real;
    }

    private static void gmMapFarExitMgr()
    {
        if (g_map_far_mgr == null)
            return;
        gmMapFarDeleteTcbPreDraw();
        gmMapFarDeleteTcbDraw();
        gmMapFarDeleteTcbPostDraw();
        g_map_far_mgr = null;
    }

    private static void gmMapFarReleaseMgr()
    {
    }

    private static MTS_TASK_TCB gmMapFarCreateTcbPreDraw()
    {
        GMS_MAP_FAR_MGR mgr = gmMapFarGetMgr();
        mgr.tcb_pre_draw = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmMapFarTcbProcPreDraw), null, 0U, 0, 12544U, 5, null, "GM MAP FAR PRE DRAW");
        return mgr.tcb_pre_draw;
    }

    private static void gmMapFarDeleteTcbPreDraw()
    {
        GMS_MAP_FAR_MGR mgr = gmMapFarGetMgr();
        if (mgr.tcb_pre_draw == null)
            return;
        mtTaskClearTcb(mgr.tcb_pre_draw);
        mgr.tcb_pre_draw = null;
    }

    private static void gmMapFarTcbProcPreDraw(MTS_TASK_TCB tcb)
    {
        int num = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        ObjDraw3DNNUserFunc(_gmMapFarTcbProcPreDrawDT, null, 0, 1U);
        gmMapFarCameraApply();
        if (num == 0)
        {
            amDrawSetFog(1U, 1);
            if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
            {
                amDrawSetFogColor(1U, 0.85f, 0.5f, 0.25f);
                if (g_gs_main_sys_info.stage_id == 3)
                    amDrawSetFog(1U, 0);
            }
            else
                amDrawSetFogColor(1U, 0.7f, 0.95f, 1f);
            amDrawSetFogRange(1U, 100f, 500f);
        }
        else if (1 == num)
        {
            amDrawSetFog(1U, 0);
            amDrawSetFogColor(1U, 0.0f, 0.0f, 0.3f);
            amDrawSetFogRange(1U, 450f, 650f);
        }
        else if (2 == num)
        {
            amDrawSetFog(1U, 0);
            amDrawSetFogColor(1U, 0.3333333f, 0.4627451f, 0.427451f);
            amDrawSetFogRange(1U, 300f, 1300f);
        }
        else if (4 == num)
        {
            amDrawSetFog(1U, 0);
            amDrawSetFogColor(1U, 0.1f, 0.08f, 0.22f);
            amDrawSetFogRange(1U, 160f, 1100f);
        }
        else if (5 == num)
        {
            amDrawSetFog(1U, 1);
            amDrawSetFogColor(1U, 0.0f, 0.0f, 0.0f);
            amDrawSetFogRange(1U, 100f, 1000f);
        }
        amDrawSetFog(0U, 0);
        amDrawSetFog(3U, 0);
    }

    private static MTS_TASK_TCB gmMapFarCreateTcbDraw()
    {
        GMS_MAP_FAR_MGR mgr = gmMapFarGetMgr();
        mgr.tcb_draw = MTM_TASK_MAKE_TCB(null, null, 0U, 0, 12544U, 5, null, "GM_MAP_FAR_DRAW");
        return mgr.tcb_draw;
    }

    private static void gmMapFarDeleteTcbDraw()
    {
        GMS_MAP_FAR_MGR mgr = gmMapFarGetMgr();
        if (mgr.tcb_draw == null)
            return;
        mtTaskClearTcb(mgr.tcb_draw);
        mgr.tcb_draw = null;
    }

    private static void gmMapFarChangeTcbProcDraw(GSF_TASK_PROCEDURE proc)
    {
        mtTaskChangeTcbProcedure(gmMapFarGetMgr().tcb_draw, proc);
    }

    private static void gmMapFarTcbProcPreDrawDT(object data)
    {
        gmMapFarClearColor();
    }

    private static MTS_TASK_TCB gmMapFarCreateTcbPostDraw()
    {
        GMS_MAP_FAR_MGR mgr = gmMapFarGetMgr();
        mgr.tcb_post_draw = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmMapFarTcbProcPostDraw), null, 0U, 0, 12544U, 5, null, "GM MAP FAR POST DRAW");
        return mgr.tcb_post_draw;
    }

    private static void gmMapFarDeleteTcbPostDraw()
    {
        GMS_MAP_FAR_MGR mgr = gmMapFarGetMgr();
        if (mgr.tcb_post_draw == null)
            return;
        mtTaskClearTcb(mgr.tcb_post_draw);
        mgr.tcb_post_draw = null;
    }

    private static void gmMapFarTcbProcPostDraw(MTS_TASK_TCB tcb)
    {
        ObjDraw3DNNUserFunc(_gmMapFarTcbProcPostDrawDT, null, 0, 3U);
        OBS_OBJECT obsObject = gmMapFarGetObject();
        ObjDraw3DNNSetCamera(obsObject.glb_camera_id, obsObject.glb_camera_type);
    }

    private static void gmMapFarTcbProcPostDrawDT(object data)
    {
    }

    private static GMS_MAP_FAR_CAMERA gmMapFarCameraGetInfo()
    {
        return gmMapFarGetMgr().camera;
    }

    private static void gmMapFarCameraSetInfo(int camear_id, int camera_type)
    {
        GMS_MAP_FAR_CAMERA info = gmMapFarCameraGetInfo();
        info.camera_id = camear_id;
        info.camera_type = camera_type;
    }

    private static void gmMapFarCameraApply()
    {
        GMS_MAP_FAR_CAMERA info = gmMapFarCameraGetInfo();
        ObjDraw3DNNSetCamera(info.camera_id, info.camera_type);
    }

    private static void gmMapFarCameraSetSpeed(float speed_x, float speed_y)
    {
        GMS_MAP_FAR_CAMERA info = gmMapFarCameraGetInfo();
        info.camera_speed_x = speed_x;
        info.camera_speed_y = speed_y;
    }

    private static float gmMapFarCameraGetSpeedX()
    {
        return gmMapFarCameraGetInfo().camera_speed_x;
    }

    private static float gmMapFarCameraGetSpeedY()
    {
        return gmMapFarCameraGetInfo().camera_speed_y;
    }

    private static int gmMapFarCameraGetScrollDistance(
      GMS_MAP_FAR_SCROLL[] scroll_list,
      uint scroll_info_num)
    {
        if (scroll_list == null)
            return 0;
        int num = 0;
        for (uint index = 0; scroll_info_num > index; ++index)
        {
            GMS_MAP_FAR_SCROLL scroll = scroll_list[(int)index];
            num += scroll.width * scroll.loop_num;
        }
        return num;
    }

    private static float gmMapFarCameraGetPos(
      float player_camera_pos,
      GMS_MAP_FAR_SCROLL[] scroll_list,
      uint scroll_info_num,
      float scroll_speed)
    {
        float num1 = scroll_speed * player_camera_pos;
        float num2 = num1;
        if (scroll_list == null)
            return num1;
        for (uint index = 0; scroll_info_num > index; ++index)
        {
            GMS_MAP_FAR_SCROLL scroll = scroll_list[(int)index];
            if (scroll.loop_num != 0)
            {
                int num3 = scroll.width * scroll.loop_num;
                if (num3 <= (double)num1)
                {
                    num1 -= num3;
                    num2 = scroll.pos + scroll.width;
                }
                else
                {
                    int num4 = (int)num1 / scroll.width;
                    float num5 = num1 - num4 * scroll.width;
                    num2 = scroll.pos + num5;
                    break;
                }
            }
        }
        return num2;
    }

    private static NNS_VECTOR gmMapFarCameraGetPos(
      NNS_VECTOR player_camera_pos,
      GMS_MAP_FAR_SCROLL[] scroll_list_x,
      uint scroll_info_num_x,
      GMS_MAP_FAR_SCROLL[] scroll_list_y,
      uint scroll_info_num_y)
    {
        NNS_VECTOR cameraGetPosResult = gmMapFarCameraGetPos_result;
        float x = player_camera_pos.x;
        cameraGetPosResult.x = gmMapFarCameraGetPos(x, scroll_list_x, scroll_info_num_x, gmMapFarCameraGetSpeedX());
        float y = player_camera_pos.y;
        cameraGetPosResult.y = gmMapFarCameraGetPos(y, scroll_list_y, scroll_info_num_y, gmMapFarCameraGetSpeedY());
        cameraGetPosResult.z = 160f;
        return cameraGetPosResult;
    }

    private static void gmMapFarSceneLoadObj(
      GMD_MAP_FAR_OBJ_INDEX obj_index,
      OBS_ACTION3D_NN_WORK obj_3d_work,
      uint mat_motion_index,
      AMS_AMB_HEADER mat_amb_header,
      uint motion_index,
      AMS_AMB_HEADER mtn_amb_header,
      MPP_VOID_OBS_OBJECT_WORK main_func,
      MPP_VOID_OBS_OBJECT_WORK out_func,
      uint command_state)
    {
        GMS_MAP_FAR_OBJ_WORK gmsMapFarObjWork = gmMapFarDataLoadObj(obj_index, obj_3d_work, main_func, out_func);
        gmsMapFarObjWork.obj_3d.command_state = command_state;
        if (mat_amb_header != null)
        {
            ObjAction3dNNMaterialMotionLoad(gmsMapFarObjWork.obj_3d, 0, null, null, (int)mat_motion_index, mat_amb_header);
            if (gmMapFarGetZoneType(gmMapFarGetStageId()) == 5)
            {
                AMS_MOTION motion = gmsMapFarObjWork.obj_3d.motion;
                motion.mmtn[motion.mmotion_id].StartFrame = 2400f;
            }
        }
        if (mtn_amb_header == null)
            return;
        ObjAction3dNNMotionLoad(gmsMapFarObjWork.obj_3d, 0, false, null, null, (int)motion_index, mtn_amb_header, 8, 8);
    }

    private static void gmMapFarSceneObjFuncDraw2DBG(OBS_OBJECT_WORK pWork)
    {
        OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        int stageId = gmMapFarGetStageId();
        if (stageId <= 2)
            return;
        if (stageId >= 4 && stageId <= 7)
        {
            if (gmMapFarDrawCheckYakei() == 0)
                return;
        }
        else
        {
            switch (stageId)
            {
                case 8:
                    if (g_map_far_data.pos.z >= -33.3300018310547 && g_map_far_data.pos.z <= -26.3700008392334 || g_map_far_data.pos.z >= -97.6399993896484 && g_map_far_data.pos.z <= -88.9800033569336 || (g_map_far_data.pos.z >= -283.790008544922 && g_map_far_data.pos.z <= -243.600006103516 || g_map_far_data.pos.z >= -376.929992675781 && g_map_far_data.pos.z <= -352.829986572266) || g_map_far_data.pos.z >= -467.010009765625 && g_map_far_data.pos.z <= -454.160003662109)
                        return;
                    break;
                case 10:
                    if (g_map_far_data.pos.z >= -117.26000213623 || g_map_far_data.pos.z >= -183.139999389648 && g_map_far_data.pos.z <= -117.26000213623 && g_map_far_data.pos.y <= 26.7299995422363 || (g_map_far_data.pos.z >= -444.0 && g_map_far_data.pos.z <= -200.0 || g_map_far_data.pos.z >= -636.400024414063 && g_map_far_data.pos.z <= -486.140014648438))
                        return;
                    break;
                case 11:
                    if ((g_map_far_data.pos.z < -115.0 || g_map_far_data.pos.y >= 15.0) && g_map_far_data.pos.z > -245.0 && (g_map_far_data.pos.z > -105.0 || g_map_far_data.pos.z < -220.0 || g_map_far_data.pos.y < 40.0))
                        return;
                    break;
                default:
                    if (stageId == 16 && (g_map_far_data.pos.z >= -5.59999990463257 || g_map_far_data.pos.z >= -13.3000001907349 && g_map_far_data.pos.z <= -7.15000009536743 || g_map_far_data.pos.z >= -21.2999992370605 && g_map_far_data.pos.z <= -15.6499996185303))
                        return;
                    break;
            }
        }
        pWork.disp_flag |= 9502720U;
        nnMakeUnitMatrix(obj3d.user_obj_mtx);
        if (stageId == 3)
        {
            pWork.disp_flag |= 4U;
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 250f, -97f, -20f);
        }
        else if (stageId >= 4 && stageId <= 7)
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 250f, -70f, -50f);
        else if (stageId >= 8 && stageId <= 11)
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 10f, -30f, -50f);
        else if (stageId >= 16 && stageId <= 20)
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 250f, -70f, -50f);
        ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
    }

    private static void gmMapFarSceneObjFuncDrawRockA(OBS_OBJECT_WORK pWork)
    {
        OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -10f;
        float num = -100f;
        int stageId = gmMapFarGetStageId();
        pWork.disp_flag |= 9502720U;
        if (gmMapFarDrawCheckRock() == 0)
            return;
        if (g_gs_main_sys_info.stage_id == 2)
        {
            if (g_map_far_data.pos.z <= -18.0 && g_map_far_data.pos.z >= -295.0)
            {
                nnMakeUnitMatrix(obj3d.user_obj_mtx);
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num - 40f, y, -90f);
                ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if (g_map_far_data.pos.z <= -112.0 && g_map_far_data.pos.z >= -366.0)
            {
                nnMakeUnitMatrix(obj3d.user_obj_mtx);
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 100f, y, -240f);
                nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
                ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if (g_map_far_data.pos.z > -238.0 || g_map_far_data.pos.z < -559.0)
                return;
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 30f, y, -400f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else if (stageId == 3)
        {
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num - 30f, y, -100f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 3f, 3f, 3f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else
        {
            if (g_map_far_data.pos.z <= -10.0 && g_map_far_data.pos.z >= -260.0)
            {
                nnMakeUnitMatrix(obj3d.user_obj_mtx);
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 100f, y, -135f);
                nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
                ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if (g_map_far_data.pos.z <= -84.0 && g_map_far_data.pos.z >= -500.0)
            {
                nnMakeUnitMatrix(obj3d.user_obj_mtx);
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num - 40f, y, -290f);
                ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if (g_map_far_data.pos.z > -182.0 || g_map_far_data.pos.z < -475.0)
                return;
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 80f, y, -330f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
    }

    private static void gmMapFarSceneObjFuncDrawRockB(OBS_OBJECT_WORK pWork)
    {
        OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -10f;
        float x = -100f;
        int stageId = gmMapFarGetStageId();
        pWork.disp_flag |= 9502720U;
        if (gmMapFarDrawCheckRock() == 0)
            return;
        if (g_gs_main_sys_info.stage_id == 2)
        {
            if (g_map_far_data.pos.z <= -18.0 && g_map_far_data.pos.z >= -256.0)
            {
                nnMakeUnitMatrix(obj3d.user_obj_mtx);
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 60f, y, -100f);
                nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2.5f, 2.5f, 2.5f);
                ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if (g_map_far_data.pos.z <= -112.0 && g_map_far_data.pos.z >= -406.0)
            {
                nnMakeUnitMatrix(obj3d.user_obj_mtx);
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 20f, y, -280f);
                ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if (g_map_far_data.pos.z > -275.0 || g_map_far_data.pos.z < -526.0)
                return;
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 100f, y, -400f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else if (stageId == 3)
        {
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y, 75f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 5f, 5f, 5f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x - 130f, y, -180f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 3f, 3f, 3f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else
        {
            if (g_map_far_data.pos.z <= -6.0 && g_map_far_data.pos.z >= -317.0)
            {
                nnMakeUnitMatrix(obj3d.user_obj_mtx);
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 40f, y, -160f);
                ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if (g_map_far_data.pos.z <= -100.0 && g_map_far_data.pos.z >= -397.0)
            {
                nnMakeUnitMatrix(obj3d.user_obj_mtx);
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 70f, y, -250f);
                nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
                ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if (g_map_far_data.pos.z > -314.0 || g_map_far_data.pos.z < -574.0)
                return;
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 100f, y, -440f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2.5f, 2.5f, 2.5f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
    }

    private static void gmMapFarSceneObjFuncDrawRockC(OBS_OBJECT_WORK pWork)
    {
        OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -10f;
        float num = -100f;
        int stageId = gmMapFarGetStageId();
        pWork.disp_flag |= 9502720U;
        if (gmMapFarDrawCheckRock() == 0)
            return;
        if (g_gs_main_sys_info.stage_id == 2)
        {
            if (g_map_far_data.pos.z <= -18.0 && g_map_far_data.pos.z >= -387.0)
            {
                nnMakeUnitMatrix(obj3d.user_obj_mtx);
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 20f, y, -180f);
                nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
                ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if (g_map_far_data.pos.z > -167.0 || g_map_far_data.pos.z < -582.0)
                return;
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 80f, y, -360f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 4f, 4f, 4f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else if (stageId == 3)
        {
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 160f, y, 10f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else
        {
            if (g_map_far_data.pos.z <= -6.0 && g_map_far_data.pos.z >= -526.0)
            {
                nnMakeUnitMatrix(obj3d.user_obj_mtx);
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 30f, y, -250f);
                nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 5f, 5f, 5f);
                ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if (g_map_far_data.pos.z > -202.0 || g_map_far_data.pos.z < -577.0)
                return;
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 40f, y, -380f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
    }

    private static int gmMapFarDrawCheckRock()
    {
        switch (gmMapFarGetStageId())
        {
            case 0:
                if (g_map_far_data.pos.y <= 5.19999980926514 && g_map_far_data.pos.z >= -436.0 || g_map_far_data.pos.y <= 4.1399998664856 && g_map_far_data.pos.z < -436.0 && g_map_far_data.pos.z >= -471.399993896484 || (g_map_far_data.pos.y <= 10.0 && g_map_far_data.pos.z < -182.0 && g_map_far_data.pos.z >= -192.0 || g_map_far_data.pos.y <= 5.40000009536743 && g_map_far_data.pos.z <= -62.0 && g_map_far_data.pos.z >= -110.0) || (g_map_far_data.pos.y <= 8.85000038146973 && g_map_far_data.pos.z <= -267.0 && g_map_far_data.pos.z >= -308.0 || g_map_far_data.pos.y <= 9.97000026702881 && g_map_far_data.pos.z <= -337.0 && g_map_far_data.pos.z >= -358.0 || (g_map_far_data.pos.y >= 7.0 && g_map_far_data.pos.z <= -347.0 && g_map_far_data.pos.z >= -359.0 || g_map_far_data.pos.z <= -495.0 && g_map_far_data.pos.z >= -503.0)))
                    return 0;
                break;
            case 1:
                if (g_map_far_data.pos.y <= 2.95000004768372 || g_map_far_data.pos.y <= 6.46999979019165 && g_map_far_data.pos.z <= -135.399993896484 && g_map_far_data.pos.z >= -156.0 || (g_map_far_data.pos.y <= 4.5 && g_map_far_data.pos.z <= -135.399993896484 && g_map_far_data.pos.z >= -170.0 || g_map_far_data.pos.z <= -419.0))
                    return 0;
                break;
            case 3:
                return 0;
        }
        return 1;
    }

    private static int gmMapFarDrawCheckYakei()
    {
        switch (gmMapFarGetStageId())
        {
            case 4:
                if (g_map_far_data.pos.y >= 26.5 && g_map_far_data.pos.z <= -36.7000007629395 && g_map_far_data.pos.z >= -41.9000015258789 || g_map_far_data.pos.y <= 25.4599990844727 && g_map_far_data.pos.z <= -50.8800010681152 && g_map_far_data.pos.z >= -58.560001373291 || g_map_far_data.pos.y >= 7.05999994277954 && g_map_far_data.pos.y <= 31.6700000762939 && (g_map_far_data.pos.z <= -62.560001373291 && g_map_far_data.pos.z >= -64.9700012207031))
                    return 0;
                break;
            case 6:
                if (g_map_far_data.pos.y <= 9.55000019073486 || g_map_far_data.pos.y <= 15.3100004196167 && g_map_far_data.pos.y >= 13.0 && g_map_far_data.pos.z <= -56.4799995422363 || (g_map_far_data.pos.y <= 19.3899993896484 && g_map_far_data.pos.y >= 16.6299991607666 && g_map_far_data.pos.z <= -58.8800010681152 || g_map_far_data.pos.y <= 19.3899993896484 && g_map_far_data.pos.y >= 16.4200000762939 && g_map_far_data.pos.z >= -37.0) || g_map_far_data.pos.y <= 32.4900016784668 && g_map_far_data.pos.y >= 29.0100002288818 && g_map_far_data.pos.z >= -29.9799995422363)
                    return 0;
                break;
        }
        return 1;
    }

    private static void gmMapFarSceneObjFuncDrawBg(OBS_OBJECT_WORK pWork)
    {
        OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -30f;
        float x = 50f;
        int num = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        int stageId = gmMapFarGetStageId();
        pWork.disp_flag |= 9502720U;
        if (ObjObjectPauseCheck(obj3d.flag) > 0U)
            pWork.disp_flag |= 4096U;
        else
            pWork.disp_flag &= 4294963199U;
        if (1 == num)
        {
            if (gmMapFarDrawCheckYakei() == 0)
                return;
            pWork.disp_flag |= 4U;
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            if (stageId == 7)
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y + 10f, 0.0f);
            else
                nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y, -135f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 1f, 1f, 1f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else if (2 == num)
        {
            switch (stageId)
            {
                case 8:
                    if (g_map_far_data.pos.z <= -312.5 && g_map_far_data.pos.z >= -333.0)
                        return;
                    break;
                case 10:
                    if (g_map_far_data.pos.z >= -102.0 || g_map_far_data.pos.z <= -102.0 && g_map_far_data.pos.z >= -160.0 && g_map_far_data.pos.y <= 54.0 || (g_map_far_data.pos.z <= -177.559997558594 && g_map_far_data.pos.z >= -392.0 || g_map_far_data.pos.z <= -410.0 && g_map_far_data.pos.z >= -431.0 && g_map_far_data.pos.y <= 64.0) || (g_map_far_data.pos.z <= -431.0 && g_map_far_data.pos.z >= -550.0 || g_map_far_data.pos.z >= -550.0 && g_map_far_data.pos.y < 50.0))
                        return;
                    break;
                case 11:
                    if ((g_map_far_data.pos.z < -103.0 || g_map_far_data.pos.y >= 24.0) && g_map_far_data.pos.z > -216.0 && (g_map_far_data.pos.z > -95.0 || g_map_far_data.pos.z < -190.0 || g_map_far_data.pos.y < 84.0))
                        return;
                    break;
            }
            pWork.disp_flag |= 4U;
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y, -160f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 1f, 1f, 1f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else if (4 == num)
        {
            pWork.disp_flag |= 13705472U;
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, g_map_far_data.pos.x, -10f, g_map_far_data.pos.z);
            nnRotateYMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, (ushort)NNM_DEGtoA16(90f));
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else
        {
            if (5 != num || ((int)g_gm_main_system.game_flag & 4) != 0)
                return;
            float obdLcdX = OBD_LCD_X;
            float obdLcdY = OBD_LCD_Y;
            pWork.dir.y = 49152;
            pWork.disp_flag |= 4U;
            nnMakeUnitMatrix(obj3d.user_obj_mtx);
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, obdLcdX, obdLcdY, -160f);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 3f, 3f, 3f);
            ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
    }

    private static void gmMapFarSceneObjFuncDrawRotate(OBS_OBJECT_WORK pWork)
    {
        int num = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        int stageId = gmMapFarGetStageId();
        if (ObjObjectPauseCheck(obj3d.flag) == 0U)
        {
            if (num == 0)
            {
                g_map_far_data.degSky += amSystemGetFrameRateMain() * 0.005f;
                g_map_far_data.degSky2 += amSystemGetFrameRateMain() * 0.01f;
            }
            else if (4 == num)
            {
                g_map_far_data.degSky += amSystemGetFrameRateMain() * 0.01f;
                g_map_far_data.degSky2 += amSystemGetFrameRateMain() * 0.02f;
            }
        }
        else
            pWork.disp_flag |= 4096U;
        if (g_map_far_data.degSky > 360.0)
            g_map_far_data.degSky = 0.0f;
        if (g_map_far_data.degSky2 > 360.0)
            g_map_far_data.degSky2 = 0.0f;
        nnMakeUnitMatrix(obj3d.user_obj_mtx);
        if (1 == num)
        {
            pWork.disp_flag |= 13705472U;
            if (gmMapFarDrawCheckYakei() == 0)
                return;
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, g_map_far_data.pos.x, -30f, g_map_far_data.pos.z);
            nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 1.4f, 1.4f, 1.4f);
        }
        else if (num == 0)
        {
            pWork.disp_flag |= 4U;
            pWork.disp_flag |= 13705472U;
            switch (stageId)
            {
                case 0:
                    if (g_map_far_data.pos.y <= 10.0 && g_map_far_data.pos.z <= -182.0 && g_map_far_data.pos.z >= -191.800003051758 || g_map_far_data.pos.y <= 7.84999990463257 && g_map_far_data.pos.z <= -269.0 && g_map_far_data.pos.z >= -304.0 || (g_map_far_data.pos.y <= 9.97000026702881 && g_map_far_data.pos.z <= -337.0 && g_map_far_data.pos.z >= -358.0 || g_map_far_data.pos.y >= 7.0 && g_map_far_data.pos.z <= -347.0 && g_map_far_data.pos.z >= -359.0) || g_map_far_data.pos.z <= -495.0 && g_map_far_data.pos.z >= -503.0)
                        return;
                    break;
                case 1:
                    if (g_map_far_data.pos.y <= 6.46999979019165 && g_map_far_data.pos.z <= -135.399993896484 && g_map_far_data.pos.z >= -156.0 || g_map_far_data.pos.y <= 3.04999995231628 && g_map_far_data.pos.z <= -130.399993896484 && g_map_far_data.pos.z >= -190.0 || g_map_far_data.pos.z <= -419.0)
                        return;
                    break;
                case 3:
                    return;
            }
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, g_map_far_data.pos.x, -10f, g_map_far_data.pos.z);
        }
        else if (4 == num)
            pWork.disp_flag |= 13705472U;
        nnRotateYMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, (ushort)NNM_DEGtoA16(g_map_far_data.degSky));
        ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
    }

    private static void gmMapFarSceneObjFuncDrawWheel(OBS_OBJECT_WORK pWork)
    {
        OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -30f;
        float x = 50f;
        int stageId = gmMapFarGetStageId();
        if (7 == stageId)
            return;
        pWork.disp_flag |= 4U;
        pWork.disp_flag |= 9502720U;
        switch (stageId)
        {
            case 4:
                if (g_map_far_data.pos.z >= -119.400001525879 || gmMapFarDrawCheckYakei() == 0)
                    return;
                break;
            case 5:
                return;
            case 6:
                if (g_map_far_data.pos.z >= -119.400001525879)
                    return;
                break;
        }
        if (ObjObjectPauseCheck(obj3d.flag) > 0U)
            pWork.disp_flag |= 4096U;
        else
            pWork.disp_flag &= 4294963199U;
        nnMakeUnitMatrix(obj3d.user_obj_mtx);
        if (stageId == 7)
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y + 10f, 0.0f);
        else
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y, -135f);
        nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 1f, 1f, 1f);
        ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
    }

    private static void gmMapFarSceneObjFuncDrawSLight(OBS_OBJECT_WORK pWork)
    {
        OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -30f;
        float x = 50f;
        int stageId = gmMapFarGetStageId();
        if (7 == stageId)
            return;
        pWork.disp_flag |= 4U;
        pWork.disp_flag |= 9502720U;
        if (gmMapFarDrawCheckYakei() == 0)
            return;
        if (ObjObjectPauseCheck(obj3d.flag) > 0U)
            pWork.disp_flag |= 4096U;
        else
            pWork.disp_flag &= 4294963199U;
        nnMakeUnitMatrix(obj3d.user_obj_mtx);
        if (stageId == 7)
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y + 10f, 0.0f);
        else
            nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y, -135f);
        nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 1f, 1f, 1f);
        ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
    }

    private static void gmMapFarSceneObjFuncDrawSea(OBS_OBJECT_WORK pWork)
    {
        OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        int stageId = gmMapFarGetStageId();
        pWork.disp_flag |= 4U;
        pWork.disp_flag |= 13705472U;
        if (stageId != 0 && stageId != 1 && (stageId != 2 && stageId == 3))
            return;
        nnMakeUnitMatrix(obj3d.user_obj_mtx);
        float x = 40f;
        nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, -10f, g_map_far_data.pos.z);
        ObjDrawAction3DNN(pWork.obj_3d, new VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
    }

    private static void gmMapFarDrawSeaUserFunc(object data)
    {
        NNS_RGBA_U8 color = new NNS_RGBA_U8(0, 0, 0, byte.MaxValue);
        amDrawGetProjectionMatrix();
        amDrawEndScene();
        amRenderSetTarget(_am_draw_target, AMD_RENDER_CLEAR_COLOR | AMD_RENDER_CLEAR_DEPTH, color);
    }

    private static void gmMapFarZone1Build()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        AMS_AMB_HEADER ambHeader = gmMapFarDataGetAmbHeader();
        string sPath;
        AMS_AMB_HEADER mdl_amb1 = readAMBFile(amBindGet(ambHeader, 0, out sPath));
        mdl_amb1.dir = sPath;
        AMS_AMB_HEADER tex_amb = readAMBFile(amBindGet(ambHeader, 1, out sPath));
        tex_amb.dir = sPath;
        info.obj_3d_list = GmGameDBuildRegBuildModel(mdl_amb1, tex_amb, 0U);
        uint draw_flag = 0;
        AMS_AMB_HEADER mdl_amb2 = readAMBFile(amBindGet(ambHeader, 3, out sPath));
        mdl_amb2.dir = sPath;
        info.obj_3d_list_render = GmGameDBuildRegBuildModel(mdl_amb2, tex_amb, draw_flag);
    }

    private static bool gmMapFarZone1CheckLoading()
    {
        return true;
    }

    private static void gmMapFarZone1Flush()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        AMS_AMB_HEADER ambHeader = gmMapFarDataGetAmbHeader();
        string sPath;
        AMS_AMB_HEADER amsAmbHeader1 = readAMBFile(amBindGet(ambHeader, 0, out sPath));
        amsAmbHeader1.dir = sPath;
        GmGameDBuildRegFlushModel(info.obj_3d_list, amsAmbHeader1.file_num);
        info.obj_3d_list = null;
        AMS_AMB_HEADER amsAmbHeader2 = readAMBFile(amBindGet(ambHeader, 3, out sPath));
        amsAmbHeader2.dir = sPath;
        GmGameDBuildRegFlushModel(info.obj_3d_list_render, amsAmbHeader2.file_num);
        info.obj_3d_list_render = null;
    }

    private static void gmMapFarZone1Init()
    {
        int stageId = gmMapFarGetStageId();
        gmMapFarChangeTcbProcDraw(new GSF_TASK_PROCEDURE(gmMpaFarZone1TcbProcDraw));
        gmMapFarCameraSetInfo(1, 0);
        GMS_MAP_FAR_SCROLL[] scroll_list1;
        int num1;
        GMS_MAP_FAR_SCROLL[] scroll_list2;
        int num2;
        if (stageId == 3)
        {
            scroll_list1 = g_map_far_zone_1_boss_scroll_x;
            num1 = g_map_far_zone_1_boss_scroll_x.Length;
            scroll_list2 = g_map_far_zone_1_boss_scroll_y;
            num2 = g_map_far_zone_1_boss_scroll_y.Length;
        }
        else
        {
            scroll_list1 = g_map_far_zone_1_scroll_x;
            num1 = g_map_far_zone_1_scroll_num_x;
            scroll_list2 = g_map_far_zone_1_scroll_y;
            num2 = g_map_far_zone_1_scroll_num_y;
        }
        MP_HEADER mapsetMpA = gmMapFarGetMapsetMpA();
        int scrollDistance1 = gmMapFarCameraGetScrollDistance(scroll_list1, (uint)num1);
        int scrollDistance2 = gmMapFarCameraGetScrollDistance(scroll_list2, (uint)num2);
        gmMapFarCameraSetSpeed(scrollDistance1 / (float)(mapsetMpA.map_w * 64), scrollDistance2 / (float)(mapsetMpA.map_h * 64));
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        for (int index = 0; 6 > index; ++index)
        {
            uint num3 = g_map_far_zone_1_scene_obj_data[index];
            OBS_ACTION3D_NN_WORK obj_3d_work = index != 0 ? info.obj_3d_list[(int)num3] : info.obj_3d_list_render[(int)num3];
            uint command_state = index < 0 ? 1U : 2U;
            uint mat_motion_index = g_map_far_zone_1_scene_obj_data_mat_motion[index];
            AMS_AMB_HEADER mat_amb_header = null;
            if (uint.MaxValue != mat_motion_index)
                mat_amb_header = readAMBFile(amBindGet(gmMapFarDataGetAmbHeader(), 2));
            gmMapFarSceneLoadObj((GMD_MAP_FAR_OBJ_INDEX)index, obj_3d_work, mat_motion_index, mat_amb_header, 0U, null, g_map_far_zone_1_scene_obj_func_main[index], g_map_far_zone_1_scene_obj_func_out[index], command_state);
        }
        gmMapFarDataGetObjWork(GMD_MAP_FAR_OBJ_INDEX.GMD_MAP_FAR_OBJ_INDEX_ZONE_1_SKY).scale.y = 8192;
        OBS_OBJECT_WORK objWork = gmMapFarDataGetObjWork(GMD_MAP_FAR_OBJ_INDEX.GMD_MAP_FAR_OBJ_INDEX_ZONE_1_SEA);
        objWork.disp_flag |= 4U;
        ObjDrawObjectActionSet3DNNMaterial(objWork, 0);
        objWork.obj_3d.mat_speed = 0.2f;
        for (int index = 0; 6 > index; ++index)
            gmMapFarDataGetObjWork((GMD_MAP_FAR_OBJ_INDEX)index).user_work = (uint)index;
    }

    private static void gmMapFarZone1Release()
    {
    }

    private static void gmMpaFarZone1TcbProcDraw(MTS_TASK_TCB tcb)
    {
    }

    private static NNS_VECTOR gmMapFarZone1GetCameraPos(
      NNS_VECTOR player_camera_pos)
    {
        MP_HEADER mapsetMpA = gmMapFarGetMapsetMpA();
        NNS_VECTOR getCameraPosResult = gmMapFarZone1GetCameraPos_result;
        getCameraPosResult.Assign(player_camera_pos);
        getCameraPosResult.y += mapsetMpA.map_h * 64;
        NNS_VECTOR pos = gmMapFarCameraGetPos(getCameraPosResult, g_map_far_zone_1_scroll_x, (uint)g_map_far_zone_1_scroll_num_x, g_map_far_zone_1_scroll_y, (uint)g_map_far_zone_1_scroll_num_y);
        float z = pos.z;
        pos.z = -pos.x;
        pos.y = pos.y;
        pos.x = z;
        g_map_far_data.pos.Assign(pos);
        return pos;
    }

    private static void gmMapFarZone2Build()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        AMS_AMB_HEADER ambHeader = gmMapFarDataGetAmbHeader();
        AMS_AMB_HEADER mdl_amb = readAMBFile(amBindGet(ambHeader, 0));
        AMS_AMB_HEADER tex_amb = readAMBFile(amBindGet(ambHeader, 3));
        info.obj_3d_list = GmGameDBuildRegBuildModel(mdl_amb, tex_amb, 0U);
    }

    private static bool gmMapFarZone2CheckLoading()
    {
        return true;
    }

    private static void gmMapFarZone2Flush()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(amBindGet(gmMapFarDataGetAmbHeader(), 0));
        GmGameDBuildRegFlushModel(info.obj_3d_list, amsAmbHeader.file_num);
        info.obj_3d_list = null;
    }

    private static void gmMapFarZone2Init()
    {
        int stageId = gmMapFarGetStageId();
        gmMapFarChangeTcbProcDraw(new GSF_TASK_PROCEDURE(gmMpaFarZone2TcbProcDraw));
        gmMapFarCameraSetInfo(1, 0);
        GMS_MAP_FAR_SCROLL[] scroll_list1;
        int num1;
        GMS_MAP_FAR_SCROLL[] scroll_list2;
        int num2;
        if (stageId == 7)
        {
            scroll_list1 = g_map_far_zone_2_boss_scroll_x;
            num1 = g_map_far_zone_2_boss_scroll_x.Length;
            scroll_list2 = g_map_far_zone_2_boss_scroll_y;
            num2 = g_map_far_zone_2_boss_scroll_y.Length;
        }
        else
        {
            scroll_list1 = g_map_far_zone_2_scroll_x;
            num1 = g_map_far_zone_2_scroll_num_x;
            scroll_list2 = g_map_far_zone_2_scroll_y;
            num2 = g_map_far_zone_2_scroll_num_y;
        }
        MP_HEADER mapsetMpA = gmMapFarGetMapsetMpA();
        int scrollDistance1 = gmMapFarCameraGetScrollDistance(scroll_list1, (uint)num1);
        int scrollDistance2 = gmMapFarCameraGetScrollDistance(scroll_list2, (uint)num2);
        gmMapFarCameraSetSpeed(scrollDistance1 / (float)(mapsetMpA.map_w * 64), scrollDistance2 / (float)(mapsetMpA.map_h * 64));
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        for (int index = 0; 3 > index; ++index)
        {
            uint num3 = g_map_far_zone_2_scene_obj_data[index];
            OBS_ACTION3D_NN_WORK obj3d = info.obj_3d_list[(int)num3];
            uint command_state = 1;
            uint motion_index = g_map_far_zone_2_scene_obj_data_motion[index];
            AMS_AMB_HEADER mtn_amb_header = null;
            if (uint.MaxValue != motion_index)
                mtn_amb_header = readAMBFile(amBindGet(gmMapFarDataGetAmbHeader(), 1));
            uint mat_motion_index = g_map_far_zone_2_scene_obj_data_mat_motion[index];
            AMS_AMB_HEADER mat_amb_header = null;
            if (uint.MaxValue != mat_motion_index)
                mat_amb_header = readAMBFile(amBindGet(gmMapFarDataGetAmbHeader(), 2));
            gmMapFarSceneLoadObj((GMD_MAP_FAR_OBJ_INDEX)index, obj3d, mat_motion_index, mat_amb_header, motion_index, mtn_amb_header, g_map_far_zone_2_scene_obj_func_main[index], g_map_far_zone_2_scene_obj_func_out[index], command_state);
        }
        for (int index = 0; 3 > index; ++index)
            gmMapFarDataGetObjWork((GMD_MAP_FAR_OBJ_INDEX)index).user_work = (uint)index;
    }

    private static void gmMapFarZone2Release()
    {
    }

    private static void gmMpaFarZone2TcbProcDraw(MTS_TASK_TCB tcb)
    {
    }

    private static NNS_VECTOR gmMapFarZone2GetCameraPos(
      NNS_VECTOR player_camera_pos)
    {
        MP_HEADER mapsetMpA = gmMapFarGetMapsetMpA();
        NNS_VECTOR getCameraPosResult = gmMapFarZone2GetCameraPos_result;
        getCameraPosResult.Assign(player_camera_pos);
        getCameraPosResult.y += mapsetMpA.map_h * 64;
        NNS_VECTOR pos = gmMapFarCameraGetPos(getCameraPosResult, g_map_far_zone_2_scroll_x, (uint)g_map_far_zone_2_scroll_num_x, g_map_far_zone_2_scroll_y, (uint)g_map_far_zone_2_scroll_num_y);
        float z = pos.z;
        pos.z = -pos.x;
        pos.y = pos.y;
        pos.x = z;
        g_map_far_data.pos.Assign(pos);
        return pos;
    }

    private static void gmMapFarZone3Build()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        AMS_AMB_HEADER ambHeader = gmMapFarDataGetAmbHeader();
        AMS_AMB_HEADER mdl_amb = readAMBFile(amBindGet(ambHeader, 0));
        AMS_AMB_HEADER tex_amb = readAMBFile(amBindGet(ambHeader, 1));
        info.obj_3d_list = GmGameDBuildRegBuildModel(mdl_amb, tex_amb, 0U);
    }

    private static bool gmMapFarZone3CheckLoading()
    {
        return true;
    }

    private static void gmMapFarZone3Flush()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(amBindGet(gmMapFarDataGetAmbHeader(), 0));
        GmGameDBuildRegFlushModel(info.obj_3d_list, amsAmbHeader.file_num);
        info.obj_3d_list = null;
    }

    private static void gmMapFarZone3Init()
    {
        gmMapFarChangeTcbProcDraw(new GSF_TASK_PROCEDURE(gmMpaFarZone3TcbProcDraw));
        gmMapFarCameraSetInfo(1, 0);
        GMS_MAP_FAR_SCROLL[] mapFarZone3ScrollX = g_map_far_zone_3_scroll_x;
        int farZone3ScrollNumX = g_map_far_zone_3_scroll_num_x;
        GMS_MAP_FAR_SCROLL[] mapFarZone3ScrollY = g_map_far_zone_3_scroll_y;
        int farZone3ScrollNumY = g_map_far_zone_3_scroll_num_y;
        MP_HEADER mapsetMpA = gmMapFarGetMapsetMpA();
        int scrollDistance1 = gmMapFarCameraGetScrollDistance(mapFarZone3ScrollX, (uint)farZone3ScrollNumX);
        int scrollDistance2 = gmMapFarCameraGetScrollDistance(mapFarZone3ScrollY, (uint)farZone3ScrollNumY);
        gmMapFarCameraSetSpeed(scrollDistance1 / (float)(mapsetMpA.map_w * 64), scrollDistance2 / (float)(mapsetMpA.map_h * 64));
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        for (int index = 0; 1 > index; ++index)
        {
            uint num = g_map_far_zone_3_scene_obj_data[index];
            OBS_ACTION3D_NN_WORK obj3d = info.obj_3d_list[(int)num];
            uint command_state = 2;
            uint mat_motion_index = g_map_far_zone_3_scene_obj_data_mat_motion[index];
            AMS_AMB_HEADER mat_amb_header = null;
            if (uint.MaxValue != mat_motion_index)
                mat_amb_header = readAMBFile(amBindGet(gmMapFarDataGetAmbHeader(), 2));
            gmMapFarSceneLoadObj((GMD_MAP_FAR_OBJ_INDEX)index, obj3d, mat_motion_index, mat_amb_header, 0U, null, g_map_far_zone_3_scene_obj_func_main[index], g_map_far_zone_3_scene_obj_func_out[index], command_state);
        }
        for (int index = 0; 1 > index; ++index)
            gmMapFarDataGetObjWork((GMD_MAP_FAR_OBJ_INDEX)index).user_work = (uint)index;
    }

    private static void gmMapFarZone3Release()
    {
    }

    private static void gmMpaFarZone3TcbProcDraw(MTS_TASK_TCB tcb)
    {
    }

    private static NNS_VECTOR gmMapFarZone3GetCameraPos(
      NNS_VECTOR player_camera_pos)
    {
        MP_HEADER mapsetMpA = gmMapFarGetMapsetMpA();
        NNS_VECTOR getCameraPosResult = gmMapFarZone3GetCameraPos_result;
        getCameraPosResult.Assign(player_camera_pos);
        getCameraPosResult.y += mapsetMpA.map_h * 64;
        NNS_VECTOR pos = gmMapFarCameraGetPos(getCameraPosResult, g_map_far_zone_3_scroll_x, (uint)g_map_far_zone_3_scroll_num_x, g_map_far_zone_3_scroll_y, (uint)g_map_far_zone_3_scroll_num_y);
        float z = pos.z;
        pos.z = -pos.x;
        pos.y = pos.y;
        pos.x = z;
        g_map_far_data.pos.Assign(pos);
        return pos;
    }

    private static void gmMapFarZone4Build()
    {
    }

    private static bool gmMapFarZone4CheckLoading()
    {
        return true;
    }

    private static void gmMapFarZone4Flush()
    {
    }

    private static void gmMapFarZone4Init()
    {
        gmMapFarChangeTcbProcDraw(new GSF_TASK_PROCEDURE(gmMpaFarZone4TcbProcDraw));
    }

    private static void gmMapFarZone4Release()
    {
    }

    private static void gmMpaFarZone4TcbProcDraw(MTS_TASK_TCB tcb)
    {
    }

    private static void gmMapFarZoneFinalBuild()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        AMS_AMB_HEADER ambHeader = gmMapFarDataGetAmbHeader();
        AMS_AMB_HEADER mdl_amb = readAMBFile(amBindGet(ambHeader, 0));
        AMS_AMB_HEADER tex_amb = readAMBFile(amBindGet(ambHeader, 1));
        info.obj_3d_list = GmGameDBuildRegBuildModel(mdl_amb, tex_amb, 0U);
    }

    private static bool gmMapFarZoneFinalCheckLoading()
    {
        return true;
    }

    private static void gmMapFarZoneFinalFlush()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(amBindGet(gmMapFarDataGetAmbHeader(), 0));
        GmGameDBuildRegFlushModel(info.obj_3d_list, amsAmbHeader.file_num);
        info.obj_3d_list = null;
    }

    private static void gmMapFarZoneFinalInit()
    {
        gmMapFarChangeTcbProcDraw(new GSF_TASK_PROCEDURE(gmMpaFarZoneFinalTcbProcDraw));
        gmMapFarCameraSetInfo(1, 0);
        GMS_MAP_FAR_SCROLL[] zoneFinalScrollX = g_map_far_zone_final_scroll_x;
        int zoneFinalScrollNumX = g_map_far_zone_final_scroll_num_x;
        GMS_MAP_FAR_SCROLL[] zoneFinalScrollY = g_map_far_zone_final_scroll_y;
        int zoneFinalScrollNumY = g_map_far_zone_final_scroll_num_y;
        MP_HEADER mapsetMpA = gmMapFarGetMapsetMpA();
        int scrollDistance1 = gmMapFarCameraGetScrollDistance(zoneFinalScrollX, (uint)zoneFinalScrollNumX);
        int scrollDistance2 = gmMapFarCameraGetScrollDistance(zoneFinalScrollY, (uint)zoneFinalScrollNumY);
        gmMapFarCameraSetSpeed(scrollDistance1 / (float)(mapsetMpA.map_w * 64), scrollDistance2 / (float)(mapsetMpA.map_h * 64));
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        for (int index = 0; 1 > index; ++index)
        {
            uint num = g_map_far_zone_final_scene_obj_data[index];
            OBS_ACTION3D_NN_WORK obj3d = info.obj_3d_list[(int)num];
            gmMapFarSceneLoadObj((GMD_MAP_FAR_OBJ_INDEX)index, obj3d, uint.MaxValue, null, 0U, null, g_map_far_zone_final_scene_obj_func_main[index], g_map_far_zone_final_scene_obj_func_out[index], 2U);
        }
        for (int index = 0; 1 > index; ++index)
            gmMapFarDataGetObjWork((GMD_MAP_FAR_OBJ_INDEX)index).user_work = (uint)index;
    }

    private static void gmMapFarZoneFinalRelease()
    {
    }

    private static void gmMpaFarZoneFinalTcbProcDraw(MTS_TASK_TCB tcb)
    {
    }

    private static NNS_VECTOR gmMapFarZoneFinalGetCameraPos(
      NNS_VECTOR player_camera_pos)
    {
        MP_HEADER mapsetMpA = gmMapFarGetMapsetMpA();
        NNS_VECTOR getCameraPosResult = gmMapFarZoneFinalGetCameraPos_result;
        getCameraPosResult.Assign(player_camera_pos);
        getCameraPosResult.y += mapsetMpA.map_h * 64;
        NNS_VECTOR pos = gmMapFarCameraGetPos(getCameraPosResult, g_map_far_zone_final_scroll_x, (uint)g_map_far_zone_final_scroll_num_x, g_map_far_zone_final_scroll_y, (uint)g_map_far_zone_final_scroll_num_y);
        float z = pos.z;
        pos.z = -pos.x;
        pos.y = pos.y;
        pos.x = z;
        g_map_far_data.pos.Assign(pos);
        return pos;
    }

    private static void gmMapFarZoneSSBuild()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        AMS_AMB_HEADER ambHeader = gmMapFarDataGetAmbHeader();
        AMS_AMB_HEADER mdl_amb = readAMBFile(amBindGet(ambHeader, 0));
        AMS_AMB_HEADER tex_amb = readAMBFile(amBindGet(ambHeader, 1));
        info.obj_3d_list = GmGameDBuildRegBuildModel(mdl_amb, tex_amb, 0U);
    }

    private static bool gmMapFarZoneSSCheckLoading()
    {
        return true;
    }

    private static void gmMapFarZoneSSFlush()
    {
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(amBindGet(gmMapFarDataGetAmbHeader(), 0));
        GmGameDBuildRegFlushModel(info.obj_3d_list, amsAmbHeader.file_num);
        info.obj_3d_list = null;
    }

    private static void gmMapFarZoneSSInit()
    {
        gmMapFarChangeTcbProcDraw(new GSF_TASK_PROCEDURE(gmMpaFarZoneSSTcbProcDraw));
        gmMapFarCameraSetInfo(1, 0);
        GMS_MAP_FAR_SCROLL[] farZoneSsScrollX = g_map_far_zone_ss_scroll_x;
        int zoneSsScrollNumX = g_map_far_zone_ss_scroll_num_x;
        GMS_MAP_FAR_SCROLL[] farZoneSsScrollY = g_map_far_zone_ss_scroll_y;
        int zoneSsScrollNumY = g_map_far_zone_ss_scroll_num_y;
        MP_HEADER mapsetMpA = gmMapFarGetMapsetMpA();
        int scrollDistance1 = gmMapFarCameraGetScrollDistance(farZoneSsScrollX, (uint)zoneSsScrollNumX);
        int scrollDistance2 = gmMapFarCameraGetScrollDistance(farZoneSsScrollY, (uint)zoneSsScrollNumY);
        gmMapFarCameraSetSpeed(scrollDistance1 / (float)(mapsetMpA.map_w * 64), scrollDistance2 / (float)(mapsetMpA.map_h * 64));
        GMS_MAP_FAR_DATA info = gmMapFarDataGetInfo();
        for (int index = 0; 1 > index; ++index)
        {
            uint num = g_map_far_zone_ss_scene_obj_data[index];
            OBS_ACTION3D_NN_WORK obj3d = info.obj_3d_list[(int)num];
            uint mat_motion_index = g_map_far_zone_ss_scene_obj_data_mat_motion[index];
            AMS_AMB_HEADER mat_amb_header = null;
            if (uint.MaxValue != mat_motion_index)
                mat_amb_header = readAMBFile(amBindGet(gmMapFarDataGetAmbHeader(), 2));
            gmMapFarSceneLoadObj((GMD_MAP_FAR_OBJ_INDEX)index, obj3d, mat_motion_index, mat_amb_header, 0U, null, g_map_far_zone_ss_scene_obj_func_main[index], g_map_far_zone_ss_scene_obj_func_out[index], 1U);
        }
    }

    private static void gmMapFarZoneSSRelease()
    {
    }

    private static void gmMpaFarZoneSSTcbProcDraw(MTS_TASK_TCB tcb)
    {
    }

    private static NNS_VECTOR gmMapFarZoneSSGetCameraPos(
      NNS_VECTOR player_camera_pos)
    {
        NNS_VECTOR getCameraPosResult = gmMapFarZoneSSGetCameraPos_result;
        getCameraPosResult.Assign(player_camera_pos);
        getCameraPosResult.x = OBD_LCD_X;
        getCameraPosResult.y = OBD_LCD_Y;
        getCameraPosResult.z = 50f;
        return getCameraPosResult;
    }

}