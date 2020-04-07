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
    public static void GmMapFarInitData(AppMain.AMS_AMB_HEADER amb)
    {
        AppMain.gmMapFarDataInit();
        AppMain.gmMapFarDataSetAmbHeader(amb);
    }

    private static void GmMapFarBuildData()
    {
        switch (AppMain.gmMapFarGetZoneType(AppMain.gmMapFarGetStageId()))
        {
            case 0:
                AppMain.gmMapFarZone1Build();
                break;
            case 1:
                AppMain.gmMapFarZone2Build();
                break;
            case 2:
                AppMain.gmMapFarZone3Build();
                break;
            case 3:
                AppMain.gmMapFarZone4Build();
                break;
            case 4:
                AppMain.gmMapFarZoneFinalBuild();
                break;
            case 5:
                AppMain.gmMapFarZoneSSBuild();
                break;
        }
    }

    private static bool GmMapFarCheckLoading()
    {
        switch (AppMain.gmMapFarGetZoneType(AppMain.gmMapFarGetStageId()))
        {
            case 0:
                return AppMain.gmMapFarZone1CheckLoading();
            case 1:
                return AppMain.gmMapFarZone2CheckLoading();
            case 2:
                return AppMain.gmMapFarZone3CheckLoading();
            case 3:
                return AppMain.gmMapFarZone4CheckLoading();
            case 4:
                return AppMain.gmMapFarZoneFinalCheckLoading();
            case 5:
                return AppMain.gmMapFarZoneSSCheckLoading();
            default:
                return true;
        }
    }

    private static void GmMapFarInit()
    {
        int zoneType = AppMain.gmMapFarGetZoneType(AppMain.gmMapFarGetStageId());
        AppMain.gmMapFarInitMgr();
        AppMain.gmMapFarCreateTcbPreDraw();
        AppMain.gmMapFarCreateTcbDraw();
        AppMain.gmMapFarCreateTcbPostDraw();
        switch (zoneType)
        {
            case 0:
                AppMain.gmMapFarZone1Init();
                break;
            case 1:
                AppMain.gmMapFarZone2Init();
                break;
            case 2:
                AppMain.gmMapFarZone3Init();
                break;
            case 3:
                AppMain.gmMapFarZone4Init();
                break;
            case 4:
                AppMain.gmMapFarZoneFinalInit();
                break;
            case 5:
                AppMain.gmMapFarZoneSSInit();
                break;
        }
    }

    private static void GmMapFarExit()
    {
        AppMain.gmMapFarDataClearObjWork();
        AppMain.gmMapFarExitMgr();
    }

    private static void GmMapFarFlushData()
    {
        switch (AppMain.gmMapFarGetZoneType(AppMain.gmMapFarGetStageId()))
        {
            case 0:
                AppMain.gmMapFarZone1Flush();
                break;
            case 1:
                AppMain.gmMapFarZone2Flush();
                break;
            case 2:
                AppMain.gmMapFarZone3Flush();
                break;
            case 3:
                AppMain.gmMapFarZone4Flush();
                break;
            case 4:
                AppMain.gmMapFarZoneFinalFlush();
                break;
            case 5:
                AppMain.gmMapFarZoneSSFlush();
                break;
        }
    }

    public static void GmMapFarRelease()
    {
        switch (AppMain.gmMapFarGetZoneType(AppMain.gmMapFarGetStageId()))
        {
            case 0:
                AppMain.gmMapFarZone1Release();
                break;
            case 1:
                AppMain.gmMapFarZone2Release();
                break;
            case 2:
                AppMain.gmMapFarZone3Release();
                break;
            case 3:
                AppMain.gmMapFarZone4Release();
                break;
            case 4:
                AppMain.gmMapFarZoneFinalRelease();
                break;
            case 5:
                AppMain.gmMapFarZoneSSRelease();
                break;
        }
        AppMain.gmMapFarDataRelease();
        AppMain.gmMapFarReleaseMgr();
    }

    private static AppMain.NNS_VECTOR GmMapFarGetCameraPos(AppMain.NNS_VECTOR player_camera_pos)
    {
        switch (AppMain.gmMapFarGetZoneType(AppMain.gmMapFarGetStageId()))
        {
            case 0:
                return AppMain.gmMapFarZone1GetCameraPos(player_camera_pos);
            case 1:
                return AppMain.gmMapFarZone2GetCameraPos(player_camera_pos);
            case 2:
                return AppMain.gmMapFarZone3GetCameraPos(player_camera_pos);
            case 3:
                return player_camera_pos;
            case 4:
                return AppMain.gmMapFarZoneFinalGetCameraPos(player_camera_pos);
            case 5:
                return AppMain.gmMapFarZoneSSGetCameraPos(player_camera_pos);
            default:
                return player_camera_pos;
        }
    }

    private static AppMain.NNS_VECTOR GmMapFarGetCameraTarget(AppMain.NNS_VECTOR camera_pos)
    {
        int zoneType = AppMain.gmMapFarGetZoneType(AppMain.gmMapFarGetStageId());
        AppMain.NNS_VECTOR cameraTargetResult = AppMain.GmMapFarGetCameraTarget_result;
        AppMain.nnAddVector(cameraTargetResult, camera_pos, AppMain.g_gm_map_far_camera_target_offset[zoneType]);
        return cameraTargetResult;
    }

    private static int gmMapFarGetStageId()
    {
        return (int)AppMain.g_gs_main_sys_info.stage_id;
    }

    private static int gmMapFarGetZoneType(int stage_id)
    {
        return AppMain.g_gm_gamedat_zone_type_tbl[stage_id];
    }

    private static AppMain.MP_HEADER gmMapFarGetMapsetMpA()
    {
        return (AppMain.MP_HEADER)AppMain.g_gm_gamedat_map_set[0];
    }

    private static AppMain.OBS_OBJECT gmMapFarGetObject()
    {
        return AppMain.g_obj;
    }

    private static void gmMapFarClearColor()
    {
        int zoneType = AppMain.gmMapFarGetZoneType(AppMain.gmMapFarGetStageId());
        AppMain.amDrawSetBGColor(AppMain.g_gm_map_far_clear_color[zoneType]);
    }

    public static void gmMapFarDataInit()
    {
        int num1 = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.g_map_far_data_real.Clear();
        AppMain.g_map_far_data = AppMain.g_map_far_data_real;
        if (num1 == 0)
        {
            AppMain.g_map_far_data.degSky = AppMain.nnRandom() * 340f;
            AppMain.g_map_far_data.degSky2 = AppMain.nnRandom() * 340f;
        }
        else if (4 == num1)
        {
            float num2 = AppMain.nnRandom();
            AppMain.g_map_far_data.degSky = (double)num2 >= 0.25 ? ((double)num2 >= 0.5 ? ((double)num2 >= 0.75 ? AppMain.nnRandom() * 355f : (float)(20.0 + (double)AppMain.nnRandom() * 50.0)) : (float)(130.0 + (double)AppMain.nnRandom() * 50.0)) : (float)(260.0 + (double)AppMain.nnRandom() * 50.0);
            AppMain.g_map_far_data.degSky2 = AppMain.nnRandom() * 355f;
        }
        else
        {
            AppMain.g_map_far_data.degSky = 90f;
            AppMain.g_map_far_data.degSky2 = 90f;
        }
    }

    private static void gmMapFarDataRelease()
    {
        if (AppMain.g_map_far_data == null)
            return;
        AppMain.gmMapFarDataFreeNNModelWork();
        AppMain.gmMapFarDataReleaseAmbHeader();
        AppMain.g_map_far_data = (AppMain.GMS_MAP_FAR_DATA)null;
    }

    private static AppMain.GMS_MAP_FAR_DATA gmMapFarDataGetInfo()
    {
        return AppMain.g_map_far_data;
    }

    public static void gmMapFarDataSetAmbHeader(AppMain.AMS_AMB_HEADER amb)
    {
        AppMain.gmMapFarDataGetInfo().amb_header = amb;
    }

    private static AppMain.AMS_AMB_HEADER gmMapFarDataGetAmbHeader()
    {
        return AppMain.gmMapFarDataGetInfo().amb_header;
    }

    private static void gmMapFarDataReleaseAmbHeader()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        if (info.amb_header == null)
            return;
        info.amb_header = (AppMain.AMS_AMB_HEADER)null;
    }

    private static AppMain.GMS_MAP_FAR_OBJ_WORK gmMapFarDataLoadObj(
      AppMain.GMD_MAP_FAR_OBJ_INDEX obj_index,
      AppMain.OBS_ACTION3D_NN_WORK obj_3d_work,
      AppMain.MPP_VOID_OBS_OBJECT_WORK main_func,
      AppMain.MPP_VOID_OBS_OBJECT_WORK out_func)
    {
        AppMain.GMS_MAP_FAR_OBJ_WORK gmsMapFarObjWork = (AppMain.GMS_MAP_FAR_OBJ_WORK)AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)12544, (byte)5, (byte)0, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_MAP_FAR_OBJ_WORK()), "MAP FAR OBJ");
        AppMain.OBS_OBJECT_WORK objWork = gmsMapFarObjWork.obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, obj_3d_work, gmsMapFarObjWork.obj_3d);
        objWork.obj_type = (ushort)8;
        objWork.flag |= 18U;
        objWork.move_flag |= 768U;
        objWork.user_flag = (uint)obj_index;
        objWork.ppFunc = main_func;
        objWork.ppOut = out_func;
        AppMain.mtTaskChangeTcbDestructor(objWork.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.ObjObjectExit));
        AppMain.gmMapFarDataSetObjWork(objWork, obj_index);
        return gmsMapFarObjWork;
    }

    private static void gmMapFarDataClearObjWork()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        for (int index = 0; index < 16; ++index)
            info.obj_work[index] = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void gmMapFarDataSetObjWork(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMD_MAP_FAR_OBJ_INDEX obj_index)
    {
        AppMain.gmMapFarDataGetInfo().obj_work[(int)obj_index] = obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK gmMapFarDataGetObjWork(
      AppMain.GMD_MAP_FAR_OBJ_INDEX obj_index)
    {
        return (AppMain.GMD_MAP_FAR_OBJ_INDEX)16 <= obj_index ? (AppMain.OBS_OBJECT_WORK)null : AppMain.gmMapFarDataGetInfo().obj_work[(int)obj_index];
    }

    private static void gmMapFarDataFreeNNModelWork()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        if (info.nn_work != null)
            info.nn_work = (AppMain.OBS_ACTION3D_NN_WORK)null;
        info.nn_work_num = 0;
        info.nn_regist_num = 0;
        info.mp_header = (AppMain.MP_HEADER)null;
        info.md_header = (AppMain.MD_HEADER)null;
    }

    private static AppMain.GMS_MAP_FAR_MGR gmMapFarGetMgr()
    {
        return AppMain.g_map_far_mgr;
    }

    private static void gmMapFarInitMgr()
    {
        AppMain.g_map_far_mgr_real.Clear();
        AppMain.g_map_far_mgr = AppMain.g_map_far_mgr_real;
    }

    private static void gmMapFarExitMgr()
    {
        if (AppMain.g_map_far_mgr == null)
            return;
        AppMain.gmMapFarDeleteTcbPreDraw();
        AppMain.gmMapFarDeleteTcbDraw();
        AppMain.gmMapFarDeleteTcbPostDraw();
        AppMain.g_map_far_mgr = (AppMain.GMS_MAP_FAR_MGR)null;
    }

    private static void gmMapFarReleaseMgr()
    {
    }

    private static AppMain.MTS_TASK_TCB gmMapFarCreateTcbPreDraw()
    {
        AppMain.GMS_MAP_FAR_MGR mgr = AppMain.gmMapFarGetMgr();
        mgr.tcb_pre_draw = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMapFarTcbProcPreDraw), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 12544U, 5, (AppMain.TaskWorkFactoryDelegate)null, "GM MAP FAR PRE DRAW");
        return mgr.tcb_pre_draw;
    }

    private static void gmMapFarDeleteTcbPreDraw()
    {
        AppMain.GMS_MAP_FAR_MGR mgr = AppMain.gmMapFarGetMgr();
        if (mgr.tcb_pre_draw == null)
            return;
        AppMain.mtTaskClearTcb(mgr.tcb_pre_draw);
        mgr.tcb_pre_draw = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmMapFarTcbProcPreDraw(AppMain.MTS_TASK_TCB tcb)
    {
        int num = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.ObjDraw3DNNUserFunc(AppMain._gmMapFarTcbProcPreDrawDT, (object)null, 0, 1U);
        AppMain.gmMapFarCameraApply();
        if (num == 0)
        {
            AppMain.amDrawSetFog(1U, 1);
            if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
            {
                AppMain.amDrawSetFogColor(1U, 0.85f, 0.5f, 0.25f);
                if (AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
                    AppMain.amDrawSetFog(1U, 0);
            }
            else
                AppMain.amDrawSetFogColor(1U, 0.7f, 0.95f, 1f);
            AppMain.amDrawSetFogRange(1U, 100f, 500f);
        }
        else if (1 == num)
        {
            AppMain.amDrawSetFog(1U, 0);
            AppMain.amDrawSetFogColor(1U, 0.0f, 0.0f, 0.3f);
            AppMain.amDrawSetFogRange(1U, 450f, 650f);
        }
        else if (2 == num)
        {
            AppMain.amDrawSetFog(1U, 0);
            AppMain.amDrawSetFogColor(1U, 0.3333333f, 0.4627451f, 0.427451f);
            AppMain.amDrawSetFogRange(1U, 300f, 1300f);
        }
        else if (4 == num)
        {
            AppMain.amDrawSetFog(1U, 0);
            AppMain.amDrawSetFogColor(1U, 0.1f, 0.08f, 0.22f);
            AppMain.amDrawSetFogRange(1U, 160f, 1100f);
        }
        else if (5 == num)
        {
            AppMain.amDrawSetFog(1U, 1);
            AppMain.amDrawSetFogColor(1U, 0.0f, 0.0f, 0.0f);
            AppMain.amDrawSetFogRange(1U, 100f, 1000f);
        }
        AppMain.amDrawSetFog(0U, 0);
        AppMain.amDrawSetFog(3U, 0);
    }

    private static AppMain.MTS_TASK_TCB gmMapFarCreateTcbDraw()
    {
        AppMain.GMS_MAP_FAR_MGR mgr = AppMain.gmMapFarGetMgr();
        mgr.tcb_draw = AppMain.MTM_TASK_MAKE_TCB((AppMain.GSF_TASK_PROCEDURE)null, (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 12544U, 5, (AppMain.TaskWorkFactoryDelegate)null, "GM_MAP_FAR_DRAW");
        return mgr.tcb_draw;
    }

    private static void gmMapFarDeleteTcbDraw()
    {
        AppMain.GMS_MAP_FAR_MGR mgr = AppMain.gmMapFarGetMgr();
        if (mgr.tcb_draw == null)
            return;
        AppMain.mtTaskClearTcb(mgr.tcb_draw);
        mgr.tcb_draw = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmMapFarChangeTcbProcDraw(AppMain.GSF_TASK_PROCEDURE proc)
    {
        AppMain.mtTaskChangeTcbProcedure(AppMain.gmMapFarGetMgr().tcb_draw, proc);
    }

    private static void gmMapFarTcbProcPreDrawDT(object data)
    {
        AppMain.gmMapFarClearColor();
    }

    private static AppMain.MTS_TASK_TCB gmMapFarCreateTcbPostDraw()
    {
        AppMain.GMS_MAP_FAR_MGR mgr = AppMain.gmMapFarGetMgr();
        mgr.tcb_post_draw = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMapFarTcbProcPostDraw), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 12544U, 5, (AppMain.TaskWorkFactoryDelegate)null, "GM MAP FAR POST DRAW");
        return mgr.tcb_post_draw;
    }

    private static void gmMapFarDeleteTcbPostDraw()
    {
        AppMain.GMS_MAP_FAR_MGR mgr = AppMain.gmMapFarGetMgr();
        if (mgr.tcb_post_draw == null)
            return;
        AppMain.mtTaskClearTcb(mgr.tcb_post_draw);
        mgr.tcb_post_draw = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmMapFarTcbProcPostDraw(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.ObjDraw3DNNUserFunc(AppMain._gmMapFarTcbProcPostDrawDT, (object)null, 0, 3U);
        AppMain.OBS_OBJECT obsObject = AppMain.gmMapFarGetObject();
        AppMain.ObjDraw3DNNSetCamera(obsObject.glb_camera_id, obsObject.glb_camera_type);
    }

    private static void gmMapFarTcbProcPostDrawDT(object data)
    {
    }

    private static AppMain.GMS_MAP_FAR_CAMERA gmMapFarCameraGetInfo()
    {
        return AppMain.gmMapFarGetMgr().camera;
    }

    private static void gmMapFarCameraSetInfo(int camear_id, int camera_type)
    {
        AppMain.GMS_MAP_FAR_CAMERA info = AppMain.gmMapFarCameraGetInfo();
        info.camera_id = camear_id;
        info.camera_type = camera_type;
    }

    private static void gmMapFarCameraApply()
    {
        AppMain.GMS_MAP_FAR_CAMERA info = AppMain.gmMapFarCameraGetInfo();
        AppMain.ObjDraw3DNNSetCamera(info.camera_id, info.camera_type);
    }

    private static void gmMapFarCameraSetSpeed(float speed_x, float speed_y)
    {
        AppMain.GMS_MAP_FAR_CAMERA info = AppMain.gmMapFarCameraGetInfo();
        info.camera_speed_x = speed_x;
        info.camera_speed_y = speed_y;
    }

    private static float gmMapFarCameraGetSpeedX()
    {
        return AppMain.gmMapFarCameraGetInfo().camera_speed_x;
    }

    private static float gmMapFarCameraGetSpeedY()
    {
        return AppMain.gmMapFarCameraGetInfo().camera_speed_y;
    }

    private static int gmMapFarCameraGetScrollDistance(
      AppMain.GMS_MAP_FAR_SCROLL[] scroll_list,
      uint scroll_info_num)
    {
        if (scroll_list == null)
            return 0;
        int num = 0;
        for (uint index = 0; scroll_info_num > index; ++index)
        {
            AppMain.GMS_MAP_FAR_SCROLL scroll = scroll_list[(int)index];
            num += scroll.width * scroll.loop_num;
        }
        return num;
    }

    private static float gmMapFarCameraGetPos(
      float player_camera_pos,
      AppMain.GMS_MAP_FAR_SCROLL[] scroll_list,
      uint scroll_info_num,
      float scroll_speed)
    {
        float num1 = scroll_speed * player_camera_pos;
        float num2 = num1;
        if (scroll_list == null)
            return num1;
        for (uint index = 0; scroll_info_num > index; ++index)
        {
            AppMain.GMS_MAP_FAR_SCROLL scroll = scroll_list[(int)index];
            if (scroll.loop_num != 0)
            {
                int num3 = scroll.width * scroll.loop_num;
                if ((double)num3 <= (double)num1)
                {
                    num1 -= (float)num3;
                    num2 = (float)(scroll.pos + scroll.width);
                }
                else
                {
                    int num4 = (int)num1 / scroll.width;
                    float num5 = num1 - (float)(num4 * scroll.width);
                    num2 = (float)scroll.pos + num5;
                    break;
                }
            }
        }
        return num2;
    }

    private static AppMain.NNS_VECTOR gmMapFarCameraGetPos(
      AppMain.NNS_VECTOR player_camera_pos,
      AppMain.GMS_MAP_FAR_SCROLL[] scroll_list_x,
      uint scroll_info_num_x,
      AppMain.GMS_MAP_FAR_SCROLL[] scroll_list_y,
      uint scroll_info_num_y)
    {
        AppMain.NNS_VECTOR cameraGetPosResult = AppMain.gmMapFarCameraGetPos_result;
        float x = player_camera_pos.x;
        cameraGetPosResult.x = AppMain.gmMapFarCameraGetPos(x, scroll_list_x, scroll_info_num_x, AppMain.gmMapFarCameraGetSpeedX());
        float y = player_camera_pos.y;
        cameraGetPosResult.y = AppMain.gmMapFarCameraGetPos(y, scroll_list_y, scroll_info_num_y, AppMain.gmMapFarCameraGetSpeedY());
        cameraGetPosResult.z = 160f;
        return cameraGetPosResult;
    }

    private static void gmMapFarSceneLoadObj(
      AppMain.GMD_MAP_FAR_OBJ_INDEX obj_index,
      AppMain.OBS_ACTION3D_NN_WORK obj_3d_work,
      uint mat_motion_index,
      AppMain.AMS_AMB_HEADER mat_amb_header,
      uint motion_index,
      AppMain.AMS_AMB_HEADER mtn_amb_header,
      AppMain.MPP_VOID_OBS_OBJECT_WORK main_func,
      AppMain.MPP_VOID_OBS_OBJECT_WORK out_func,
      uint command_state)
    {
        AppMain.GMS_MAP_FAR_OBJ_WORK gmsMapFarObjWork = AppMain.gmMapFarDataLoadObj(obj_index, obj_3d_work, main_func, out_func);
        gmsMapFarObjWork.obj_3d.command_state = command_state;
        if (mat_amb_header != null)
        {
            AppMain.ObjAction3dNNMaterialMotionLoad(gmsMapFarObjWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, (int)mat_motion_index, mat_amb_header);
            if (AppMain.gmMapFarGetZoneType(AppMain.gmMapFarGetStageId()) == 5)
            {
                AppMain.AMS_MOTION motion = gmsMapFarObjWork.obj_3d.motion;
                motion.mmtn[motion.mmotion_id].StartFrame = 2400f;
            }
        }
        if (mtn_amb_header == null)
            return;
        AppMain.ObjAction3dNNMotionLoad(gmsMapFarObjWork.obj_3d, 0, false, (AppMain.OBS_DATA_WORK)null, (string)null, (int)motion_index, mtn_amb_header, 8, 8);
    }

    private static void gmMapFarSceneObjFuncDraw2DBG(AppMain.OBS_OBJECT_WORK pWork)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        int stageId = AppMain.gmMapFarGetStageId();
        if (stageId <= 2)
            return;
        if (stageId >= 4 && stageId <= 7)
        {
            if (AppMain.gmMapFarDrawCheckYakei() == 0)
                return;
        }
        else
        {
            switch (stageId)
            {
                case 8:
                    if ((double)AppMain.g_map_far_data.pos.z >= -33.3300018310547 && (double)AppMain.g_map_far_data.pos.z <= -26.3700008392334 || (double)AppMain.g_map_far_data.pos.z >= -97.6399993896484 && (double)AppMain.g_map_far_data.pos.z <= -88.9800033569336 || ((double)AppMain.g_map_far_data.pos.z >= -283.790008544922 && (double)AppMain.g_map_far_data.pos.z <= -243.600006103516 || (double)AppMain.g_map_far_data.pos.z >= -376.929992675781 && (double)AppMain.g_map_far_data.pos.z <= -352.829986572266) || (double)AppMain.g_map_far_data.pos.z >= -467.010009765625 && (double)AppMain.g_map_far_data.pos.z <= -454.160003662109)
                        return;
                    break;
                case 10:
                    if ((double)AppMain.g_map_far_data.pos.z >= -117.26000213623 || (double)AppMain.g_map_far_data.pos.z >= -183.139999389648 && (double)AppMain.g_map_far_data.pos.z <= -117.26000213623 && (double)AppMain.g_map_far_data.pos.y <= 26.7299995422363 || ((double)AppMain.g_map_far_data.pos.z >= -444.0 && (double)AppMain.g_map_far_data.pos.z <= -200.0 || (double)AppMain.g_map_far_data.pos.z >= -636.400024414063 && (double)AppMain.g_map_far_data.pos.z <= -486.140014648438))
                        return;
                    break;
                case 11:
                    if (((double)AppMain.g_map_far_data.pos.z < -115.0 || (double)AppMain.g_map_far_data.pos.y >= 15.0) && (double)AppMain.g_map_far_data.pos.z > -245.0 && ((double)AppMain.g_map_far_data.pos.z > -105.0 || (double)AppMain.g_map_far_data.pos.z < -220.0 || (double)AppMain.g_map_far_data.pos.y < 40.0))
                        return;
                    break;
                default:
                    if (stageId == 16 && ((double)AppMain.g_map_far_data.pos.z >= -5.59999990463257 || (double)AppMain.g_map_far_data.pos.z >= -13.3000001907349 && (double)AppMain.g_map_far_data.pos.z <= -7.15000009536743 || (double)AppMain.g_map_far_data.pos.z >= -21.2999992370605 && (double)AppMain.g_map_far_data.pos.z <= -15.6499996185303))
                        return;
                    break;
            }
        }
        pWork.disp_flag |= 9502720U;
        AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
        if (stageId == 3)
        {
            pWork.disp_flag |= 4U;
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 250f, -97f, -20f);
        }
        else if (stageId >= 4 && stageId <= 7)
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 250f, -70f, -50f);
        else if (stageId >= 8 && stageId <= 11)
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 10f, -30f, -50f);
        else if (stageId >= 16 && stageId <= 20)
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 250f, -70f, -50f);
        AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
    }

    private static void gmMapFarSceneObjFuncDrawRockA(AppMain.OBS_OBJECT_WORK pWork)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -10f;
        float num = -100f;
        int stageId = AppMain.gmMapFarGetStageId();
        pWork.disp_flag |= 9502720U;
        if (AppMain.gmMapFarDrawCheckRock() == 0)
            return;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2)
        {
            if ((double)AppMain.g_map_far_data.pos.z <= -18.0 && (double)AppMain.g_map_far_data.pos.z >= -295.0)
            {
                AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num - 40f, y, -90f);
                AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if ((double)AppMain.g_map_far_data.pos.z <= -112.0 && (double)AppMain.g_map_far_data.pos.z >= -366.0)
            {
                AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 100f, y, -240f);
                AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
                AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if ((double)AppMain.g_map_far_data.pos.z > -238.0 || (double)AppMain.g_map_far_data.pos.z < -559.0)
                return;
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 30f, y, -400f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else if (stageId == 3)
        {
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num - 30f, y, -100f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 3f, 3f, 3f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else
        {
            if ((double)AppMain.g_map_far_data.pos.z <= -10.0 && (double)AppMain.g_map_far_data.pos.z >= -260.0)
            {
                AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 100f, y, -135f);
                AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
                AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if ((double)AppMain.g_map_far_data.pos.z <= -84.0 && (double)AppMain.g_map_far_data.pos.z >= -500.0)
            {
                AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num - 40f, y, -290f);
                AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if ((double)AppMain.g_map_far_data.pos.z > -182.0 || (double)AppMain.g_map_far_data.pos.z < -475.0)
                return;
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 80f, y, -330f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
    }

    private static void gmMapFarSceneObjFuncDrawRockB(AppMain.OBS_OBJECT_WORK pWork)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -10f;
        float x = -100f;
        int stageId = AppMain.gmMapFarGetStageId();
        pWork.disp_flag |= 9502720U;
        if (AppMain.gmMapFarDrawCheckRock() == 0)
            return;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2)
        {
            if ((double)AppMain.g_map_far_data.pos.z <= -18.0 && (double)AppMain.g_map_far_data.pos.z >= -256.0)
            {
                AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 60f, y, -100f);
                AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2.5f, 2.5f, 2.5f);
                AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if ((double)AppMain.g_map_far_data.pos.z <= -112.0 && (double)AppMain.g_map_far_data.pos.z >= -406.0)
            {
                AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 20f, y, -280f);
                AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if ((double)AppMain.g_map_far_data.pos.z > -275.0 || (double)AppMain.g_map_far_data.pos.z < -526.0)
                return;
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 100f, y, -400f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else if (stageId == 3)
        {
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y, 75f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 5f, 5f, 5f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x - 130f, y, -180f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 3f, 3f, 3f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else
        {
            if ((double)AppMain.g_map_far_data.pos.z <= -6.0 && (double)AppMain.g_map_far_data.pos.z >= -317.0)
            {
                AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 40f, y, -160f);
                AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if ((double)AppMain.g_map_far_data.pos.z <= -100.0 && (double)AppMain.g_map_far_data.pos.z >= -397.0)
            {
                AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 70f, y, -250f);
                AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
                AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if ((double)AppMain.g_map_far_data.pos.z > -314.0 || (double)AppMain.g_map_far_data.pos.z < -574.0)
                return;
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x + 100f, y, -440f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2.5f, 2.5f, 2.5f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
    }

    private static void gmMapFarSceneObjFuncDrawRockC(AppMain.OBS_OBJECT_WORK pWork)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -10f;
        float num = -100f;
        int stageId = AppMain.gmMapFarGetStageId();
        pWork.disp_flag |= 9502720U;
        if (AppMain.gmMapFarDrawCheckRock() == 0)
            return;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2)
        {
            if ((double)AppMain.g_map_far_data.pos.z <= -18.0 && (double)AppMain.g_map_far_data.pos.z >= -387.0)
            {
                AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 20f, y, -180f);
                AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
                AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if ((double)AppMain.g_map_far_data.pos.z > -167.0 || (double)AppMain.g_map_far_data.pos.z < -582.0)
                return;
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 80f, y, -360f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 4f, 4f, 4f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else if (stageId == 3)
        {
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 160f, y, 10f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else
        {
            if ((double)AppMain.g_map_far_data.pos.z <= -6.0 && (double)AppMain.g_map_far_data.pos.z >= -526.0)
            {
                AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 30f, y, -250f);
                AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 5f, 5f, 5f);
                AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
            }
            if ((double)AppMain.g_map_far_data.pos.z > -202.0 || (double)AppMain.g_map_far_data.pos.z < -577.0)
                return;
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, num + 40f, y, -380f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 2f, 2f, 2f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
    }

    private static int gmMapFarDrawCheckRock()
    {
        switch (AppMain.gmMapFarGetStageId())
        {
            case 0:
                if ((double)AppMain.g_map_far_data.pos.y <= 5.19999980926514 && (double)AppMain.g_map_far_data.pos.z >= -436.0 || (double)AppMain.g_map_far_data.pos.y <= 4.1399998664856 && (double)AppMain.g_map_far_data.pos.z < -436.0 && (double)AppMain.g_map_far_data.pos.z >= -471.399993896484 || ((double)AppMain.g_map_far_data.pos.y <= 10.0 && (double)AppMain.g_map_far_data.pos.z < -182.0 && (double)AppMain.g_map_far_data.pos.z >= -192.0 || (double)AppMain.g_map_far_data.pos.y <= 5.40000009536743 && (double)AppMain.g_map_far_data.pos.z <= -62.0 && (double)AppMain.g_map_far_data.pos.z >= -110.0) || ((double)AppMain.g_map_far_data.pos.y <= 8.85000038146973 && (double)AppMain.g_map_far_data.pos.z <= -267.0 && (double)AppMain.g_map_far_data.pos.z >= -308.0 || (double)AppMain.g_map_far_data.pos.y <= 9.97000026702881 && (double)AppMain.g_map_far_data.pos.z <= -337.0 && (double)AppMain.g_map_far_data.pos.z >= -358.0 || ((double)AppMain.g_map_far_data.pos.y >= 7.0 && (double)AppMain.g_map_far_data.pos.z <= -347.0 && (double)AppMain.g_map_far_data.pos.z >= -359.0 || (double)AppMain.g_map_far_data.pos.z <= -495.0 && (double)AppMain.g_map_far_data.pos.z >= -503.0)))
                    return 0;
                break;
            case 1:
                if ((double)AppMain.g_map_far_data.pos.y <= 2.95000004768372 || (double)AppMain.g_map_far_data.pos.y <= 6.46999979019165 && (double)AppMain.g_map_far_data.pos.z <= -135.399993896484 && (double)AppMain.g_map_far_data.pos.z >= -156.0 || ((double)AppMain.g_map_far_data.pos.y <= 4.5 && (double)AppMain.g_map_far_data.pos.z <= -135.399993896484 && (double)AppMain.g_map_far_data.pos.z >= -170.0 || (double)AppMain.g_map_far_data.pos.z <= -419.0))
                    return 0;
                break;
            case 3:
                return 0;
        }
        return 1;
    }

    private static int gmMapFarDrawCheckYakei()
    {
        switch (AppMain.gmMapFarGetStageId())
        {
            case 4:
                if ((double)AppMain.g_map_far_data.pos.y >= 26.5 && (double)AppMain.g_map_far_data.pos.z <= -36.7000007629395 && (double)AppMain.g_map_far_data.pos.z >= -41.9000015258789 || (double)AppMain.g_map_far_data.pos.y <= 25.4599990844727 && (double)AppMain.g_map_far_data.pos.z <= -50.8800010681152 && (double)AppMain.g_map_far_data.pos.z >= -58.560001373291 || (double)AppMain.g_map_far_data.pos.y >= 7.05999994277954 && (double)AppMain.g_map_far_data.pos.y <= 31.6700000762939 && ((double)AppMain.g_map_far_data.pos.z <= -62.560001373291 && (double)AppMain.g_map_far_data.pos.z >= -64.9700012207031))
                    return 0;
                break;
            case 6:
                if ((double)AppMain.g_map_far_data.pos.y <= 9.55000019073486 || (double)AppMain.g_map_far_data.pos.y <= 15.3100004196167 && (double)AppMain.g_map_far_data.pos.y >= 13.0 && (double)AppMain.g_map_far_data.pos.z <= -56.4799995422363 || ((double)AppMain.g_map_far_data.pos.y <= 19.3899993896484 && (double)AppMain.g_map_far_data.pos.y >= 16.6299991607666 && (double)AppMain.g_map_far_data.pos.z <= -58.8800010681152 || (double)AppMain.g_map_far_data.pos.y <= 19.3899993896484 && (double)AppMain.g_map_far_data.pos.y >= 16.4200000762939 && (double)AppMain.g_map_far_data.pos.z >= -37.0) || (double)AppMain.g_map_far_data.pos.y <= 32.4900016784668 && (double)AppMain.g_map_far_data.pos.y >= 29.0100002288818 && (double)AppMain.g_map_far_data.pos.z >= -29.9799995422363)
                    return 0;
                break;
        }
        return 1;
    }

    private static void gmMapFarSceneObjFuncDrawBg(AppMain.OBS_OBJECT_WORK pWork)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -30f;
        float x = 50f;
        int num = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        int stageId = AppMain.gmMapFarGetStageId();
        pWork.disp_flag |= 9502720U;
        if (AppMain.ObjObjectPauseCheck(obj3d.flag) > 0U)
            pWork.disp_flag |= 4096U;
        else
            pWork.disp_flag &= 4294963199U;
        if (1 == num)
        {
            if (AppMain.gmMapFarDrawCheckYakei() == 0)
                return;
            pWork.disp_flag |= 4U;
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            if (stageId == 7)
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y + 10f, 0.0f);
            else
                AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y, -135f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 1f, 1f, 1f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else if (2 == num)
        {
            switch (stageId)
            {
                case 8:
                    if ((double)AppMain.g_map_far_data.pos.z <= -312.5 && (double)AppMain.g_map_far_data.pos.z >= -333.0)
                        return;
                    break;
                case 10:
                    if ((double)AppMain.g_map_far_data.pos.z >= -102.0 || (double)AppMain.g_map_far_data.pos.z <= -102.0 && (double)AppMain.g_map_far_data.pos.z >= -160.0 && (double)AppMain.g_map_far_data.pos.y <= 54.0 || ((double)AppMain.g_map_far_data.pos.z <= -177.559997558594 && (double)AppMain.g_map_far_data.pos.z >= -392.0 || (double)AppMain.g_map_far_data.pos.z <= -410.0 && (double)AppMain.g_map_far_data.pos.z >= -431.0 && (double)AppMain.g_map_far_data.pos.y <= 64.0) || ((double)AppMain.g_map_far_data.pos.z <= -431.0 && (double)AppMain.g_map_far_data.pos.z >= -550.0 || (double)AppMain.g_map_far_data.pos.z >= -550.0 && (double)AppMain.g_map_far_data.pos.y < 50.0))
                        return;
                    break;
                case 11:
                    if (((double)AppMain.g_map_far_data.pos.z < -103.0 || (double)AppMain.g_map_far_data.pos.y >= 24.0) && (double)AppMain.g_map_far_data.pos.z > -216.0 && ((double)AppMain.g_map_far_data.pos.z > -95.0 || (double)AppMain.g_map_far_data.pos.z < -190.0 || (double)AppMain.g_map_far_data.pos.y < 84.0))
                        return;
                    break;
            }
            pWork.disp_flag |= 4U;
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y, -160f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 1f, 1f, 1f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else if (4 == num)
        {
            pWork.disp_flag |= 13705472U;
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, AppMain.g_map_far_data.pos.x, -10f, AppMain.g_map_far_data.pos.z);
            AppMain.nnRotateYMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, (int)(ushort)AppMain.NNM_DEGtoA16(90f));
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
        else
        {
            if (5 != num || ((int)AppMain.g_gm_main_system.game_flag & 4) != 0)
                return;
            float obdLcdX = (float)AppMain.OBD_LCD_X;
            float obdLcdY = (float)AppMain.OBD_LCD_Y;
            pWork.dir.y = (ushort)49152;
            pWork.disp_flag |= 4U;
            AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, obdLcdX, obdLcdY, -160f);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 3f, 3f, 3f);
            AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
        }
    }

    private static void gmMapFarSceneObjFuncDrawRotate(AppMain.OBS_OBJECT_WORK pWork)
    {
        int num = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        int stageId = AppMain.gmMapFarGetStageId();
        if (AppMain.ObjObjectPauseCheck(obj3d.flag) == 0U)
        {
            if (num == 0)
            {
                AppMain.g_map_far_data.degSky += AppMain.amSystemGetFrameRateMain() * 0.005f;
                AppMain.g_map_far_data.degSky2 += AppMain.amSystemGetFrameRateMain() * 0.01f;
            }
            else if (4 == num)
            {
                AppMain.g_map_far_data.degSky += AppMain.amSystemGetFrameRateMain() * 0.01f;
                AppMain.g_map_far_data.degSky2 += AppMain.amSystemGetFrameRateMain() * 0.02f;
            }
        }
        else
            pWork.disp_flag |= 4096U;
        if ((double)AppMain.g_map_far_data.degSky > 360.0)
            AppMain.g_map_far_data.degSky = 0.0f;
        if ((double)AppMain.g_map_far_data.degSky2 > 360.0)
            AppMain.g_map_far_data.degSky2 = 0.0f;
        AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
        if (1 == num)
        {
            pWork.disp_flag |= 13705472U;
            if (AppMain.gmMapFarDrawCheckYakei() == 0)
                return;
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, AppMain.g_map_far_data.pos.x, -30f, AppMain.g_map_far_data.pos.z);
            AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 1.4f, 1.4f, 1.4f);
        }
        else if (num == 0)
        {
            pWork.disp_flag |= 4U;
            pWork.disp_flag |= 13705472U;
            switch (stageId)
            {
                case 0:
                    if ((double)AppMain.g_map_far_data.pos.y <= 10.0 && (double)AppMain.g_map_far_data.pos.z <= -182.0 && (double)AppMain.g_map_far_data.pos.z >= -191.800003051758 || (double)AppMain.g_map_far_data.pos.y <= 7.84999990463257 && (double)AppMain.g_map_far_data.pos.z <= -269.0 && (double)AppMain.g_map_far_data.pos.z >= -304.0 || ((double)AppMain.g_map_far_data.pos.y <= 9.97000026702881 && (double)AppMain.g_map_far_data.pos.z <= -337.0 && (double)AppMain.g_map_far_data.pos.z >= -358.0 || (double)AppMain.g_map_far_data.pos.y >= 7.0 && (double)AppMain.g_map_far_data.pos.z <= -347.0 && (double)AppMain.g_map_far_data.pos.z >= -359.0) || (double)AppMain.g_map_far_data.pos.z <= -495.0 && (double)AppMain.g_map_far_data.pos.z >= -503.0)
                        return;
                    break;
                case 1:
                    if ((double)AppMain.g_map_far_data.pos.y <= 6.46999979019165 && (double)AppMain.g_map_far_data.pos.z <= -135.399993896484 && (double)AppMain.g_map_far_data.pos.z >= -156.0 || (double)AppMain.g_map_far_data.pos.y <= 3.04999995231628 && (double)AppMain.g_map_far_data.pos.z <= -130.399993896484 && (double)AppMain.g_map_far_data.pos.z >= -190.0 || (double)AppMain.g_map_far_data.pos.z <= -419.0)
                        return;
                    break;
                case 3:
                    return;
            }
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, AppMain.g_map_far_data.pos.x, -10f, AppMain.g_map_far_data.pos.z);
        }
        else if (4 == num)
            pWork.disp_flag |= 13705472U;
        AppMain.nnRotateYMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, (int)(ushort)AppMain.NNM_DEGtoA16(AppMain.g_map_far_data.degSky));
        AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
    }

    private static void gmMapFarSceneObjFuncDrawWheel(AppMain.OBS_OBJECT_WORK pWork)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -30f;
        float x = 50f;
        int stageId = AppMain.gmMapFarGetStageId();
        if (7 == stageId)
            return;
        pWork.disp_flag |= 4U;
        pWork.disp_flag |= 9502720U;
        switch (stageId)
        {
            case 4:
                if ((double)AppMain.g_map_far_data.pos.z >= -119.400001525879 || AppMain.gmMapFarDrawCheckYakei() == 0)
                    return;
                break;
            case 5:
                return;
            case 6:
                if ((double)AppMain.g_map_far_data.pos.z >= -119.400001525879)
                    return;
                break;
        }
        if (AppMain.ObjObjectPauseCheck(obj3d.flag) > 0U)
            pWork.disp_flag |= 4096U;
        else
            pWork.disp_flag &= 4294963199U;
        AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
        if (stageId == 7)
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y + 10f, 0.0f);
        else
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y, -135f);
        AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 1f, 1f, 1f);
        AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
    }

    private static void gmMapFarSceneObjFuncDrawSLight(AppMain.OBS_OBJECT_WORK pWork)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        float y = -30f;
        float x = 50f;
        int stageId = AppMain.gmMapFarGetStageId();
        if (7 == stageId)
            return;
        pWork.disp_flag |= 4U;
        pWork.disp_flag |= 9502720U;
        if (AppMain.gmMapFarDrawCheckYakei() == 0)
            return;
        if (AppMain.ObjObjectPauseCheck(obj3d.flag) > 0U)
            pWork.disp_flag |= 4096U;
        else
            pWork.disp_flag &= 4294963199U;
        AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
        if (stageId == 7)
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y + 10f, 0.0f);
        else
            AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, y, -135f);
        AppMain.nnScaleMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, 1f, 1f, 1f);
        AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
    }

    private static void gmMapFarSceneObjFuncDrawSea(AppMain.OBS_OBJECT_WORK pWork)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = pWork.obj_3d;
        int stageId = AppMain.gmMapFarGetStageId();
        pWork.disp_flag |= 4U;
        pWork.disp_flag |= 13705472U;
        if (stageId != 0 && stageId != 1 && (stageId != 2 && stageId == 3))
            return;
        AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx);
        float x = 40f;
        AppMain.nnTranslateMatrix(obj3d.user_obj_mtx, obj3d.user_obj_mtx, x, -10f, AppMain.g_map_far_data.pos.z);
        AppMain.ObjDrawAction3DNN(pWork.obj_3d, new AppMain.VecFx32?(pWork.pos), new AppMain.VecU16?(pWork.dir), pWork.scale, ref pWork.disp_flag);
    }

    private static void gmMapFarDrawSeaUserFunc(object data)
    {
        AppMain.NNS_RGBA_U8 color = new AppMain.NNS_RGBA_U8((byte)0, (byte)0, (byte)0, byte.MaxValue);
        AppMain.amDrawGetProjectionMatrix();
        AppMain.amDrawEndScene();
        AppMain.amRenderSetTarget(AppMain._am_draw_target, AppMain.AMD_RENDER_CLEAR_COLOR | AppMain.AMD_RENDER_CLEAR_DEPTH, color);
    }

    private static void gmMapFarZone1Build()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        AppMain.AMS_AMB_HEADER ambHeader = AppMain.gmMapFarDataGetAmbHeader();
        string sPath;
        AppMain.AMS_AMB_HEADER mdl_amb1 = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 0, out sPath));
        mdl_amb1.dir = sPath;
        AppMain.AMS_AMB_HEADER tex_amb = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 1, out sPath));
        tex_amb.dir = sPath;
        info.obj_3d_list = AppMain.GmGameDBuildRegBuildModel(mdl_amb1, tex_amb, 0U);
        uint draw_flag = 0;
        AppMain.AMS_AMB_HEADER mdl_amb2 = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 3, out sPath));
        mdl_amb2.dir = sPath;
        info.obj_3d_list_render = AppMain.GmGameDBuildRegBuildModel(mdl_amb2, tex_amb, draw_flag);
    }

    private static bool gmMapFarZone1CheckLoading()
    {
        return true;
    }

    private static void gmMapFarZone1Flush()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        AppMain.AMS_AMB_HEADER ambHeader = AppMain.gmMapFarDataGetAmbHeader();
        string sPath;
        AppMain.AMS_AMB_HEADER amsAmbHeader1 = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 0, out sPath));
        amsAmbHeader1.dir = sPath;
        AppMain.GmGameDBuildRegFlushModel(info.obj_3d_list, amsAmbHeader1.file_num);
        info.obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        AppMain.AMS_AMB_HEADER amsAmbHeader2 = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 3, out sPath));
        amsAmbHeader2.dir = sPath;
        AppMain.GmGameDBuildRegFlushModel(info.obj_3d_list_render, amsAmbHeader2.file_num);
        info.obj_3d_list_render = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static void gmMapFarZone1Init()
    {
        int stageId = AppMain.gmMapFarGetStageId();
        AppMain.gmMapFarChangeTcbProcDraw(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMpaFarZone1TcbProcDraw));
        AppMain.gmMapFarCameraSetInfo(1, 0);
        AppMain.GMS_MAP_FAR_SCROLL[] scroll_list1;
        int num1;
        AppMain.GMS_MAP_FAR_SCROLL[] scroll_list2;
        int num2;
        if (stageId == 3)
        {
            scroll_list1 = AppMain.g_map_far_zone_1_boss_scroll_x;
            num1 = AppMain.g_map_far_zone_1_boss_scroll_x.Length;
            scroll_list2 = AppMain.g_map_far_zone_1_boss_scroll_y;
            num2 = AppMain.g_map_far_zone_1_boss_scroll_y.Length;
        }
        else
        {
            scroll_list1 = AppMain.g_map_far_zone_1_scroll_x;
            num1 = AppMain.g_map_far_zone_1_scroll_num_x;
            scroll_list2 = AppMain.g_map_far_zone_1_scroll_y;
            num2 = AppMain.g_map_far_zone_1_scroll_num_y;
        }
        AppMain.MP_HEADER mapsetMpA = AppMain.gmMapFarGetMapsetMpA();
        int scrollDistance1 = AppMain.gmMapFarCameraGetScrollDistance(scroll_list1, (uint)num1);
        int scrollDistance2 = AppMain.gmMapFarCameraGetScrollDistance(scroll_list2, (uint)num2);
        AppMain.gmMapFarCameraSetSpeed((float)scrollDistance1 / (float)((int)mapsetMpA.map_w * 64), (float)scrollDistance2 / (float)((int)mapsetMpA.map_h * 64));
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        for (int index = 0; 6 > index; ++index)
        {
            uint num3 = AppMain.g_map_far_zone_1_scene_obj_data[index];
            AppMain.OBS_ACTION3D_NN_WORK obj_3d_work = index != 0 ? info.obj_3d_list[(int)num3] : info.obj_3d_list_render[(int)num3];
            uint command_state = index < 0 ? 1U : 2U;
            uint mat_motion_index = AppMain.g_map_far_zone_1_scene_obj_data_mat_motion[index];
            AppMain.AMS_AMB_HEADER mat_amb_header = (AppMain.AMS_AMB_HEADER)null;
            if (uint.MaxValue != mat_motion_index)
                mat_amb_header = AppMain.readAMBFile(AppMain.amBindGet(AppMain.gmMapFarDataGetAmbHeader(), 2));
            AppMain.gmMapFarSceneLoadObj((AppMain.GMD_MAP_FAR_OBJ_INDEX)index, obj_3d_work, mat_motion_index, mat_amb_header, 0U, (AppMain.AMS_AMB_HEADER)null, AppMain.g_map_far_zone_1_scene_obj_func_main[index], AppMain.g_map_far_zone_1_scene_obj_func_out[index], command_state);
        }
        AppMain.gmMapFarDataGetObjWork(AppMain.GMD_MAP_FAR_OBJ_INDEX.GMD_MAP_FAR_OBJ_INDEX_ZONE_1_SKY).scale.y = 8192;
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmMapFarDataGetObjWork(AppMain.GMD_MAP_FAR_OBJ_INDEX.GMD_MAP_FAR_OBJ_INDEX_ZONE_1_SEA);
        objWork.disp_flag |= 4U;
        AppMain.ObjDrawObjectActionSet3DNNMaterial(objWork, 0);
        objWork.obj_3d.mat_speed = 0.2f;
        for (int index = 0; 6 > index; ++index)
            AppMain.gmMapFarDataGetObjWork((AppMain.GMD_MAP_FAR_OBJ_INDEX)index).user_work = (uint)index;
    }

    private static void gmMapFarZone1Release()
    {
    }

    private static void gmMpaFarZone1TcbProcDraw(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static AppMain.NNS_VECTOR gmMapFarZone1GetCameraPos(
      AppMain.NNS_VECTOR player_camera_pos)
    {
        AppMain.MP_HEADER mapsetMpA = AppMain.gmMapFarGetMapsetMpA();
        AppMain.NNS_VECTOR getCameraPosResult = AppMain.gmMapFarZone1GetCameraPos_result;
        getCameraPosResult.Assign(player_camera_pos);
        getCameraPosResult.y += (float)((int)mapsetMpA.map_h * 64);
        AppMain.NNS_VECTOR pos = AppMain.gmMapFarCameraGetPos(getCameraPosResult, AppMain.g_map_far_zone_1_scroll_x, (uint)AppMain.g_map_far_zone_1_scroll_num_x, AppMain.g_map_far_zone_1_scroll_y, (uint)AppMain.g_map_far_zone_1_scroll_num_y);
        float z = pos.z;
        pos.z = -pos.x;
        pos.y = pos.y;
        pos.x = z;
        AppMain.g_map_far_data.pos.Assign(pos);
        return pos;
    }

    private static void gmMapFarZone2Build()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        AppMain.AMS_AMB_HEADER ambHeader = AppMain.gmMapFarDataGetAmbHeader();
        AppMain.AMS_AMB_HEADER mdl_amb = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 0));
        AppMain.AMS_AMB_HEADER tex_amb = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 3));
        info.obj_3d_list = AppMain.GmGameDBuildRegBuildModel(mdl_amb, tex_amb, 0U);
    }

    private static bool gmMapFarZone2CheckLoading()
    {
        return true;
    }

    private static void gmMapFarZone2Flush()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.amBindGet(AppMain.gmMapFarDataGetAmbHeader(), 0));
        AppMain.GmGameDBuildRegFlushModel(info.obj_3d_list, amsAmbHeader.file_num);
        info.obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static void gmMapFarZone2Init()
    {
        int stageId = AppMain.gmMapFarGetStageId();
        AppMain.gmMapFarChangeTcbProcDraw(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMpaFarZone2TcbProcDraw));
        AppMain.gmMapFarCameraSetInfo(1, 0);
        AppMain.GMS_MAP_FAR_SCROLL[] scroll_list1;
        int num1;
        AppMain.GMS_MAP_FAR_SCROLL[] scroll_list2;
        int num2;
        if (stageId == 7)
        {
            scroll_list1 = AppMain.g_map_far_zone_2_boss_scroll_x;
            num1 = AppMain.g_map_far_zone_2_boss_scroll_x.Length;
            scroll_list2 = AppMain.g_map_far_zone_2_boss_scroll_y;
            num2 = AppMain.g_map_far_zone_2_boss_scroll_y.Length;
        }
        else
        {
            scroll_list1 = AppMain.g_map_far_zone_2_scroll_x;
            num1 = AppMain.g_map_far_zone_2_scroll_num_x;
            scroll_list2 = AppMain.g_map_far_zone_2_scroll_y;
            num2 = AppMain.g_map_far_zone_2_scroll_num_y;
        }
        AppMain.MP_HEADER mapsetMpA = AppMain.gmMapFarGetMapsetMpA();
        int scrollDistance1 = AppMain.gmMapFarCameraGetScrollDistance(scroll_list1, (uint)num1);
        int scrollDistance2 = AppMain.gmMapFarCameraGetScrollDistance(scroll_list2, (uint)num2);
        AppMain.gmMapFarCameraSetSpeed((float)scrollDistance1 / (float)((int)mapsetMpA.map_w * 64), (float)scrollDistance2 / (float)((int)mapsetMpA.map_h * 64));
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        for (int index = 0; 3 > index; ++index)
        {
            uint num3 = AppMain.g_map_far_zone_2_scene_obj_data[index];
            AppMain.OBS_ACTION3D_NN_WORK obj3d = info.obj_3d_list[(int)num3];
            uint command_state = 1;
            uint motion_index = AppMain.g_map_far_zone_2_scene_obj_data_motion[index];
            AppMain.AMS_AMB_HEADER mtn_amb_header = (AppMain.AMS_AMB_HEADER)null;
            if (uint.MaxValue != motion_index)
                mtn_amb_header = AppMain.readAMBFile(AppMain.amBindGet(AppMain.gmMapFarDataGetAmbHeader(), 1));
            uint mat_motion_index = AppMain.g_map_far_zone_2_scene_obj_data_mat_motion[index];
            AppMain.AMS_AMB_HEADER mat_amb_header = (AppMain.AMS_AMB_HEADER)null;
            if (uint.MaxValue != mat_motion_index)
                mat_amb_header = AppMain.readAMBFile(AppMain.amBindGet(AppMain.gmMapFarDataGetAmbHeader(), 2));
            AppMain.gmMapFarSceneLoadObj((AppMain.GMD_MAP_FAR_OBJ_INDEX)index, obj3d, mat_motion_index, mat_amb_header, motion_index, mtn_amb_header, AppMain.g_map_far_zone_2_scene_obj_func_main[index], AppMain.g_map_far_zone_2_scene_obj_func_out[index], command_state);
        }
        for (int index = 0; 3 > index; ++index)
            AppMain.gmMapFarDataGetObjWork((AppMain.GMD_MAP_FAR_OBJ_INDEX)index).user_work = (uint)index;
    }

    private static void gmMapFarZone2Release()
    {
    }

    private static void gmMpaFarZone2TcbProcDraw(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static AppMain.NNS_VECTOR gmMapFarZone2GetCameraPos(
      AppMain.NNS_VECTOR player_camera_pos)
    {
        AppMain.MP_HEADER mapsetMpA = AppMain.gmMapFarGetMapsetMpA();
        AppMain.NNS_VECTOR getCameraPosResult = AppMain.gmMapFarZone2GetCameraPos_result;
        getCameraPosResult.Assign(player_camera_pos);
        getCameraPosResult.y += (float)((int)mapsetMpA.map_h * 64);
        AppMain.NNS_VECTOR pos = AppMain.gmMapFarCameraGetPos(getCameraPosResult, AppMain.g_map_far_zone_2_scroll_x, (uint)AppMain.g_map_far_zone_2_scroll_num_x, AppMain.g_map_far_zone_2_scroll_y, (uint)AppMain.g_map_far_zone_2_scroll_num_y);
        float z = pos.z;
        pos.z = -pos.x;
        pos.y = pos.y;
        pos.x = z;
        AppMain.g_map_far_data.pos.Assign(pos);
        return pos;
    }

    private static void gmMapFarZone3Build()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        AppMain.AMS_AMB_HEADER ambHeader = AppMain.gmMapFarDataGetAmbHeader();
        AppMain.AMS_AMB_HEADER mdl_amb = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 0));
        AppMain.AMS_AMB_HEADER tex_amb = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 1));
        info.obj_3d_list = AppMain.GmGameDBuildRegBuildModel(mdl_amb, tex_amb, 0U);
    }

    private static bool gmMapFarZone3CheckLoading()
    {
        return true;
    }

    private static void gmMapFarZone3Flush()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.amBindGet(AppMain.gmMapFarDataGetAmbHeader(), 0));
        AppMain.GmGameDBuildRegFlushModel(info.obj_3d_list, amsAmbHeader.file_num);
        info.obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static void gmMapFarZone3Init()
    {
        AppMain.gmMapFarChangeTcbProcDraw(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMpaFarZone3TcbProcDraw));
        AppMain.gmMapFarCameraSetInfo(1, 0);
        AppMain.GMS_MAP_FAR_SCROLL[] mapFarZone3ScrollX = AppMain.g_map_far_zone_3_scroll_x;
        int farZone3ScrollNumX = AppMain.g_map_far_zone_3_scroll_num_x;
        AppMain.GMS_MAP_FAR_SCROLL[] mapFarZone3ScrollY = AppMain.g_map_far_zone_3_scroll_y;
        int farZone3ScrollNumY = AppMain.g_map_far_zone_3_scroll_num_y;
        AppMain.MP_HEADER mapsetMpA = AppMain.gmMapFarGetMapsetMpA();
        int scrollDistance1 = AppMain.gmMapFarCameraGetScrollDistance(mapFarZone3ScrollX, (uint)farZone3ScrollNumX);
        int scrollDistance2 = AppMain.gmMapFarCameraGetScrollDistance(mapFarZone3ScrollY, (uint)farZone3ScrollNumY);
        AppMain.gmMapFarCameraSetSpeed((float)scrollDistance1 / (float)((int)mapsetMpA.map_w * 64), (float)scrollDistance2 / (float)((int)mapsetMpA.map_h * 64));
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        for (int index = 0; 1 > index; ++index)
        {
            uint num = AppMain.g_map_far_zone_3_scene_obj_data[index];
            AppMain.OBS_ACTION3D_NN_WORK obj3d = info.obj_3d_list[(int)num];
            uint command_state = 2;
            uint mat_motion_index = AppMain.g_map_far_zone_3_scene_obj_data_mat_motion[index];
            AppMain.AMS_AMB_HEADER mat_amb_header = (AppMain.AMS_AMB_HEADER)null;
            if (uint.MaxValue != mat_motion_index)
                mat_amb_header = AppMain.readAMBFile(AppMain.amBindGet(AppMain.gmMapFarDataGetAmbHeader(), 2));
            AppMain.gmMapFarSceneLoadObj((AppMain.GMD_MAP_FAR_OBJ_INDEX)index, obj3d, mat_motion_index, mat_amb_header, 0U, (AppMain.AMS_AMB_HEADER)null, AppMain.g_map_far_zone_3_scene_obj_func_main[index], AppMain.g_map_far_zone_3_scene_obj_func_out[index], command_state);
        }
        for (int index = 0; 1 > index; ++index)
            AppMain.gmMapFarDataGetObjWork((AppMain.GMD_MAP_FAR_OBJ_INDEX)index).user_work = (uint)index;
    }

    private static void gmMapFarZone3Release()
    {
    }

    private static void gmMpaFarZone3TcbProcDraw(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static AppMain.NNS_VECTOR gmMapFarZone3GetCameraPos(
      AppMain.NNS_VECTOR player_camera_pos)
    {
        AppMain.MP_HEADER mapsetMpA = AppMain.gmMapFarGetMapsetMpA();
        AppMain.NNS_VECTOR getCameraPosResult = AppMain.gmMapFarZone3GetCameraPos_result;
        getCameraPosResult.Assign(player_camera_pos);
        getCameraPosResult.y += (float)((int)mapsetMpA.map_h * 64);
        AppMain.NNS_VECTOR pos = AppMain.gmMapFarCameraGetPos(getCameraPosResult, AppMain.g_map_far_zone_3_scroll_x, (uint)AppMain.g_map_far_zone_3_scroll_num_x, AppMain.g_map_far_zone_3_scroll_y, (uint)AppMain.g_map_far_zone_3_scroll_num_y);
        float z = pos.z;
        pos.z = -pos.x;
        pos.y = pos.y;
        pos.x = z;
        AppMain.g_map_far_data.pos.Assign(pos);
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
        AppMain.gmMapFarChangeTcbProcDraw(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMpaFarZone4TcbProcDraw));
    }

    private static void gmMapFarZone4Release()
    {
    }

    private static void gmMpaFarZone4TcbProcDraw(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static void gmMapFarZoneFinalBuild()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        AppMain.AMS_AMB_HEADER ambHeader = AppMain.gmMapFarDataGetAmbHeader();
        AppMain.AMS_AMB_HEADER mdl_amb = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 0));
        AppMain.AMS_AMB_HEADER tex_amb = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 1));
        info.obj_3d_list = AppMain.GmGameDBuildRegBuildModel(mdl_amb, tex_amb, 0U);
    }

    private static bool gmMapFarZoneFinalCheckLoading()
    {
        return true;
    }

    private static void gmMapFarZoneFinalFlush()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.amBindGet(AppMain.gmMapFarDataGetAmbHeader(), 0));
        AppMain.GmGameDBuildRegFlushModel(info.obj_3d_list, amsAmbHeader.file_num);
        info.obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static void gmMapFarZoneFinalInit()
    {
        AppMain.gmMapFarChangeTcbProcDraw(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMpaFarZoneFinalTcbProcDraw));
        AppMain.gmMapFarCameraSetInfo(1, 0);
        AppMain.GMS_MAP_FAR_SCROLL[] zoneFinalScrollX = AppMain.g_map_far_zone_final_scroll_x;
        int zoneFinalScrollNumX = AppMain.g_map_far_zone_final_scroll_num_x;
        AppMain.GMS_MAP_FAR_SCROLL[] zoneFinalScrollY = AppMain.g_map_far_zone_final_scroll_y;
        int zoneFinalScrollNumY = AppMain.g_map_far_zone_final_scroll_num_y;
        AppMain.MP_HEADER mapsetMpA = AppMain.gmMapFarGetMapsetMpA();
        int scrollDistance1 = AppMain.gmMapFarCameraGetScrollDistance(zoneFinalScrollX, (uint)zoneFinalScrollNumX);
        int scrollDistance2 = AppMain.gmMapFarCameraGetScrollDistance(zoneFinalScrollY, (uint)zoneFinalScrollNumY);
        AppMain.gmMapFarCameraSetSpeed((float)scrollDistance1 / (float)((int)mapsetMpA.map_w * 64), (float)scrollDistance2 / (float)((int)mapsetMpA.map_h * 64));
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        for (int index = 0; 1 > index; ++index)
        {
            uint num = AppMain.g_map_far_zone_final_scene_obj_data[index];
            AppMain.OBS_ACTION3D_NN_WORK obj3d = info.obj_3d_list[(int)num];
            AppMain.gmMapFarSceneLoadObj((AppMain.GMD_MAP_FAR_OBJ_INDEX)index, obj3d, uint.MaxValue, (AppMain.AMS_AMB_HEADER)null, 0U, (AppMain.AMS_AMB_HEADER)null, AppMain.g_map_far_zone_final_scene_obj_func_main[index], AppMain.g_map_far_zone_final_scene_obj_func_out[index], 2U);
        }
        for (int index = 0; 1 > index; ++index)
            AppMain.gmMapFarDataGetObjWork((AppMain.GMD_MAP_FAR_OBJ_INDEX)index).user_work = (uint)index;
    }

    private static void gmMapFarZoneFinalRelease()
    {
    }

    private static void gmMpaFarZoneFinalTcbProcDraw(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static AppMain.NNS_VECTOR gmMapFarZoneFinalGetCameraPos(
      AppMain.NNS_VECTOR player_camera_pos)
    {
        AppMain.MP_HEADER mapsetMpA = AppMain.gmMapFarGetMapsetMpA();
        AppMain.NNS_VECTOR getCameraPosResult = AppMain.gmMapFarZoneFinalGetCameraPos_result;
        getCameraPosResult.Assign(player_camera_pos);
        getCameraPosResult.y += (float)((int)mapsetMpA.map_h * 64);
        AppMain.NNS_VECTOR pos = AppMain.gmMapFarCameraGetPos(getCameraPosResult, AppMain.g_map_far_zone_final_scroll_x, (uint)AppMain.g_map_far_zone_final_scroll_num_x, AppMain.g_map_far_zone_final_scroll_y, (uint)AppMain.g_map_far_zone_final_scroll_num_y);
        float z = pos.z;
        pos.z = -pos.x;
        pos.y = pos.y;
        pos.x = z;
        AppMain.g_map_far_data.pos.Assign(pos);
        return pos;
    }

    private static void gmMapFarZoneSSBuild()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        AppMain.AMS_AMB_HEADER ambHeader = AppMain.gmMapFarDataGetAmbHeader();
        AppMain.AMS_AMB_HEADER mdl_amb = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 0));
        AppMain.AMS_AMB_HEADER tex_amb = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 1));
        info.obj_3d_list = AppMain.GmGameDBuildRegBuildModel(mdl_amb, tex_amb, 0U);
    }

    private static bool gmMapFarZoneSSCheckLoading()
    {
        return true;
    }

    private static void gmMapFarZoneSSFlush()
    {
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.amBindGet(AppMain.gmMapFarDataGetAmbHeader(), 0));
        AppMain.GmGameDBuildRegFlushModel(info.obj_3d_list, amsAmbHeader.file_num);
        info.obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static void gmMapFarZoneSSInit()
    {
        AppMain.gmMapFarChangeTcbProcDraw(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMpaFarZoneSSTcbProcDraw));
        AppMain.gmMapFarCameraSetInfo(1, 0);
        AppMain.GMS_MAP_FAR_SCROLL[] farZoneSsScrollX = AppMain.g_map_far_zone_ss_scroll_x;
        int zoneSsScrollNumX = AppMain.g_map_far_zone_ss_scroll_num_x;
        AppMain.GMS_MAP_FAR_SCROLL[] farZoneSsScrollY = AppMain.g_map_far_zone_ss_scroll_y;
        int zoneSsScrollNumY = AppMain.g_map_far_zone_ss_scroll_num_y;
        AppMain.MP_HEADER mapsetMpA = AppMain.gmMapFarGetMapsetMpA();
        int scrollDistance1 = AppMain.gmMapFarCameraGetScrollDistance(farZoneSsScrollX, (uint)zoneSsScrollNumX);
        int scrollDistance2 = AppMain.gmMapFarCameraGetScrollDistance(farZoneSsScrollY, (uint)zoneSsScrollNumY);
        AppMain.gmMapFarCameraSetSpeed((float)scrollDistance1 / (float)((int)mapsetMpA.map_w * 64), (float)scrollDistance2 / (float)((int)mapsetMpA.map_h * 64));
        AppMain.GMS_MAP_FAR_DATA info = AppMain.gmMapFarDataGetInfo();
        for (int index = 0; 1 > index; ++index)
        {
            uint num = AppMain.g_map_far_zone_ss_scene_obj_data[index];
            AppMain.OBS_ACTION3D_NN_WORK obj3d = info.obj_3d_list[(int)num];
            uint mat_motion_index = AppMain.g_map_far_zone_ss_scene_obj_data_mat_motion[index];
            AppMain.AMS_AMB_HEADER mat_amb_header = (AppMain.AMS_AMB_HEADER)null;
            if (uint.MaxValue != mat_motion_index)
                mat_amb_header = AppMain.readAMBFile(AppMain.amBindGet(AppMain.gmMapFarDataGetAmbHeader(), 2));
            AppMain.gmMapFarSceneLoadObj((AppMain.GMD_MAP_FAR_OBJ_INDEX)index, obj3d, mat_motion_index, mat_amb_header, 0U, (AppMain.AMS_AMB_HEADER)null, AppMain.g_map_far_zone_ss_scene_obj_func_main[index], AppMain.g_map_far_zone_ss_scene_obj_func_out[index], 1U);
        }
    }

    private static void gmMapFarZoneSSRelease()
    {
    }

    private static void gmMpaFarZoneSSTcbProcDraw(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static AppMain.NNS_VECTOR gmMapFarZoneSSGetCameraPos(
      AppMain.NNS_VECTOR player_camera_pos)
    {
        AppMain.NNS_VECTOR getCameraPosResult = AppMain.gmMapFarZoneSSGetCameraPos_result;
        getCameraPosResult.Assign(player_camera_pos);
        getCameraPosResult.x = (float)AppMain.OBD_LCD_X;
        getCameraPosResult.y = (float)AppMain.OBD_LCD_Y;
        getCameraPosResult.z = 50f;
        return getCameraPosResult;
    }

}