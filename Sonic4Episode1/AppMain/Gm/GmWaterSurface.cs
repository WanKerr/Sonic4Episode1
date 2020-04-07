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
    private static void GmWaterSurfaceInitData(AppMain.AMS_AMB_HEADER amb)
    {
        AppMain.gmWaterSurfaceDataInit();
        AppMain.gmWaterSurfaceDataSetAmbHeader(amb);
    }

    private static void GmWaterSurfaceBuildData()
    {
        if (AppMain.gmWaterSurfaceGameSystemGetZoneType(AppMain.gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        AppMain.dwaterInit();
        AppMain.AMS_AMB_HEADER ambHeader = AppMain.gmWaterSurfaceDataGetAmbHeader();
        AppMain.AoTexBuild(AppMain._dmap_water.tex_color, AppMain.readAMBFile(AppMain.amBindGet(ambHeader, ambHeader.file_num - 1)));
        AppMain.AoTexLoad(AppMain._dmap_water.tex_color);
        AppMain.dwaterSetObjectAMB(ambHeader, ambHeader);
        AppMain.dwaterLoadObject(0U);
    }

    private static bool GmWaterSurfaceCheckLoading()
    {
        bool flag = true;
        if (AppMain.GsMainSysGetDisplayListRegistNum() >= 192)
            return false;
        if (AppMain.gmWaterSurfaceGameSystemGetZoneType(AppMain.gmWaterSurfaceGameSystemGetStageId()) == 2)
        {
            AppMain.GMS_WATER_SURFACE_DATA info = AppMain.gmWaterSurfaceDataGetInfo();
            if (!AppMain.AoTexIsLoaded(AppMain._dmap_water.tex_color))
                flag = false;
            else if (!info.flag_load_object)
            {
                if (AppMain.dwaterLoadObject(0U) == 0)
                {
                    int num = flag ? 1 : 0;
                    flag = false;
                    info.flag_load_object = false;
                }
                else
                    info.flag_load_object = true;
            }
        }
        return flag;
    }

    private static void GmWaterSurfaceInit()
    {
        AppMain.gmWaterSurfaceGameSystemSetWaterLevel(ushort.MaxValue);
        if (AppMain.gmWaterSurfaceGameSystemGetZoneType(AppMain.gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        AppMain.gmWaterSurfaceInitMgr();
        AppMain.gmWaterSurfaceCreateTcb();
    }

    private static void GmWaterSurfaceExit()
    {
        AppMain.gmWaterSurfaceExitMgr();
    }

    private static void GmWaterSurfaceFlushData()
    {
        if (AppMain.gmWaterSurfaceGameSystemGetZoneType(AppMain.gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        AppMain.AoTexRelease(AppMain._dmap_water.tex_color);
    }

    private static void GmWaterSurfaceRelease()
    {
        if (AppMain.gmWaterSurfaceGameSystemGetZoneType(AppMain.gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        AppMain.gmWaterSurfaceDataRelease();
    }

    private static bool GmWaterSurfaceCheckFlush()
    {
        if (AppMain.GsMainSysGetDisplayListRegistNum() >= 192)
            return false;
        bool flag = true;
        if (AppMain.gmWaterSurfaceGameSystemGetZoneType(AppMain.gmWaterSurfaceGameSystemGetStageId()) == 2 && AppMain._dmap_water != null)
        {
            if (!AppMain.AoTexIsReleased(AppMain._dmap_water.tex_color))
                flag = false;
            else if (AppMain._dmap_water.regist_index == -1)
            {
                AppMain._dmap_water.regist_index = AppMain.dwaterRelease();
                int num = flag ? 1 : 0;
                flag = false;
            }
            else if (!AppMain.amDrawIsRegistComplete(AppMain._dmap_water.regist_index))
            {
                int num = flag ? 1 : 0;
                flag = false;
            }
            else
                AppMain.dwaterExit();
        }
        return flag;
    }

    private static void GmWaterSurfaceRequestChangeWaterLevel(
      ushort water_level,
      ushort time,
      bool flag_add_time)
    {
        if (AppMain.gmWaterSurfaceGameSystemGetZoneType(AppMain.gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        AppMain.GMS_WATER_SURFACE_MGR mgr = AppMain.gmWaterSurfaceGetMgr();
        if (mgr == null)
            return;
        AppMain.GMS_WATER_SURFACE_INFO work = (AppMain.GMS_WATER_SURFACE_INFO)mgr.tcb_water.work;
        if (flag_add_time)
        {
            work.water_time += time;
        }
        else
        {
            work.water_time = time;
            work.water_counter = (ushort)0;
        }
        work.next_water_level = (float)water_level;
        work.now_water_level = (float)AppMain.gmWaterSurfaceGameSystemGetWaterLevel();
    }

    private static void GmWaterSurfaceRequestAddWatarLevel(
      float water_level,
      ushort time,
      bool flag_add_time)
    {
        if (AppMain.gmWaterSurfaceGameSystemGetZoneType(AppMain.gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        AppMain.GMS_WATER_SURFACE_MGR mgr = AppMain.gmWaterSurfaceGetMgr();
        if (mgr == null)
            return;
        AppMain.GMS_WATER_SURFACE_INFO work = (AppMain.GMS_WATER_SURFACE_INFO)mgr.tcb_water.work;
        if (flag_add_time)
        {
            work.water_time += time;
        }
        else
        {
            work.water_time = time;
            work.water_counter = (ushort)0;
        }
        work.next_water_level += water_level;
        work.now_water_level = (float)AppMain.gmWaterSurfaceGameSystemGetWaterLevel();
    }

    private static void GmWaterSurfaceSetFlagDraw(bool flag_draw)
    {
        if (AppMain.gmWaterSurfaceGameSystemGetZoneType(AppMain.gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        AppMain.GMS_WATER_SURFACE_MGR mgr = AppMain.gmWaterSurfaceGetMgr();
        if (mgr == null)
            return;
        ((AppMain.GMS_WATER_SURFACE_INFO)mgr.tcb_water.work).flag_draw = flag_draw;
    }

    private static void GmWaterSurfaceSetFlagEnableRef(bool flag_enable_ref)
    {
        if (AppMain.gmWaterSurfaceGameSystemGetZoneType(AppMain.gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        AppMain.GMS_WATER_SURFACE_MGR mgr = AppMain.gmWaterSurfaceGetMgr();
        if (mgr == null)
            return;
        ((AppMain.GMS_WATER_SURFACE_INFO)mgr.tcb_water.work).flag_enable_ref = flag_enable_ref;
    }

    private static AppMain.AMS_RENDER_TARGET GmWaterSurfaceGetRenderTarget()
    {
        return AppMain.gmWaterSurfaceGetMgr()?.render_target;
    }

    private static void GmWaterSurfaceDrawNoWaterField(
      float left,
      float top,
      float right,
      float bottom)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.aTest = (short)0;
        setParam.zMask = (short)0;
        setParam.zTest = (short)1;
        setParam.ablend = 1;
        setParam.noSort = (short)1;
        AppMain.NNS_PRIM3D_PC[] nnsPriM3DPcArray = AppMain.amDrawAlloc_NNS_PRIM3D_PC(6);
        float f32 = AppMain.FX_FX32_TO_F32(1310720);
        nnsPriM3DPcArray[0].Pos.Assign(left, top, f32);
        nnsPriM3DPcArray[1].Pos.Assign(right, top, f32);
        nnsPriM3DPcArray[2].Pos.Assign(left, bottom, f32);
        nnsPriM3DPcArray[5].Pos.Assign(right, bottom, f32);
        uint num = AppMain.AMD_RGBA8888(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)0);
        nnsPriM3DPcArray[0].Col = num;
        nnsPriM3DPcArray[1].Col = num;
        nnsPriM3DPcArray[2].Col = num;
        nnsPriM3DPcArray[5].Col = num;
        nnsPriM3DPcArray[3] = nnsPriM3DPcArray[1];
        nnsPriM3DPcArray[4] = nnsPriM3DPcArray[2];
        setParam.format3D = 2;
        setParam.type = 0;
        setParam.vtxPC3D = nnsPriM3DPcArray;
        setParam.texlist = (AppMain.NNS_TEXLIST)null;
        setParam.texId = 0;
        setParam.count = 6;
        setParam.sortZ = -1f;
        AppMain.gmWaterSurfaceMatrixPush(9U);
        AppMain.amDrawPrimitive3D(9U, setParam);
        AppMain.gmWaterSurfaceMatrixPop(9U);
        AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    private static int gmWaterSurfaceGameSystemGetStageId()
    {
        return (int)AppMain.g_gs_main_sys_info.stage_id;
    }

    private static int gmWaterSurfaceGameSystemGetZoneType(int stage_id)
    {
        return AppMain.g_gm_gamedat_zone_type_tbl[stage_id];
    }

    private static ushort gmWaterSurfaceGameSystemGetWaterLevel()
    {
        return AppMain.g_gm_main_system.water_level;
    }

    private static void gmWaterSurfaceGameSystemSetWaterLevel(ushort water_level)
    {
        AppMain.g_gm_main_system.water_level = water_level;
    }

    private static void gmWaterSurfaceDataInit()
    {
        AppMain.g_water_surface_data_real.Clear();
        AppMain.g_water_surface_data = AppMain.g_water_surface_data_real;
    }

    private static void gmWaterSurfaceDataRelease()
    {
        if (AppMain.g_water_surface_data == null)
            return;
        AppMain.gmWaterSurfaceDataReleaseAmbHeader();
        AppMain.g_water_surface_data = (AppMain.GMS_WATER_SURFACE_DATA)null;
    }

    private static AppMain.GMS_WATER_SURFACE_DATA gmWaterSurfaceDataGetInfo()
    {
        return AppMain.g_water_surface_data;
    }

    private static void gmWaterSurfaceDataSetAmbHeader(AppMain.AMS_AMB_HEADER amb)
    {
        AppMain.gmWaterSurfaceDataGetInfo().amb_header = amb;
    }

    private static AppMain.AMS_AMB_HEADER gmWaterSurfaceDataGetAmbHeader()
    {
        return AppMain.gmWaterSurfaceDataGetInfo().amb_header;
    }

    private static void gmWaterSurfaceDataReleaseAmbHeader()
    {
        AppMain.GMS_WATER_SURFACE_DATA info = AppMain.gmWaterSurfaceDataGetInfo();
        if (info.amb_header == null)
            return;
        info.amb_header = (AppMain.AMS_AMB_HEADER)null;
    }

    private static AppMain.GMS_WATER_SURFACE_MGR gmWaterSurfaceGetMgr()
    {
        return AppMain.g_water_surface_mgr;
    }

    private static void gmWaterSurfaceInitMgr()
    {
        AppMain.g_water_surface_mgr_real.Clear();
        AppMain.g_water_surface_mgr = AppMain.g_water_surface_mgr_real;
    }

    private static void gmWaterSurfaceExitMgr()
    {
        if (AppMain.g_water_surface_mgr == null)
            return;
        AppMain.gmWaterSurfaceDeleteTcb();
        AppMain.g_water_surface_mgr = (AppMain.GMS_WATER_SURFACE_MGR)null;
    }

    private static AppMain.MTS_TASK_TCB gmWaterSurfaceCreateTcb()
    {
        AppMain.GMS_WATER_SURFACE_MGR mgr = AppMain.gmWaterSurfaceGetMgr();
        mgr.tcb_water = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmWaterSurfaceProc), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 8202U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_WATER_SURFACE_INFO()), "GM WATER SURFACE");
        AppMain.GMS_WATER_SURFACE_INFO work = (AppMain.GMS_WATER_SURFACE_INFO)mgr.tcb_water.work;
        work.now_water_level = (float)AppMain.gmWaterSurfaceGameSystemGetWaterLevel();
        work.next_water_level = (float)ushort.MaxValue;
        work.water_time = (ushort)0;
        work.water_counter = (ushort)0;
        work.flag_draw = true;
        work.flag_enable_ref = true;
        return mgr.tcb_water;
    }

    private static void gmWaterSurfaceDeleteTcb()
    {
        AppMain.GMS_WATER_SURFACE_MGR mgr = AppMain.gmWaterSurfaceGetMgr();
        if (mgr.tcb_water == null)
            return;
        AppMain.mtTaskClearTcb(mgr.tcb_water);
        mgr.tcb_water = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmWaterSurfaceTcbProcPreDrawDT(object data)
    {
        AppMain.AMS_RENDER_TARGET target = AppMain._am_render_manager.targetp != AppMain._gm_mapFar_render_work ? AppMain._gm_mapFar_render_work : AppMain._am_draw_target;
        if (target.width == 0)
            return;
        AppMain.amDrawEndScene();
        AppMain.NNS_RGBA_U8 color = new AppMain.NNS_RGBA_U8((byte)0, (byte)0, (byte)0, byte.MaxValue);
        AppMain.amRenderCopyTarget(target, color);
        AppMain.GMS_WATER_SURFACE_MGR mgr = AppMain.gmWaterSurfaceGetMgr();
        if (mgr == null)
            return;
        mgr.render_target = target;
    }

    private static void gmWaterSurfaceTcbProcDrawDT(object data)
    {
        AppMain.GMS_WATER_SURFACE_MGR mgr = AppMain.gmWaterSurfaceGetMgr();
        if (mgr == null || mgr.render_target == null || mgr.render_target.width == 0)
            return;
        AppMain.dwaterDrawWater(mgr.render_target);
    }

    private static void gmWaterSurfaceTcbProcPostDrawDT(object data)
    {
    }

    private static void gmWaterSurfaceProc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_WATER_SURFACE_INFO work = (AppMain.GMS_WATER_SURFACE_INFO)tcb.work;
        float speed = 0.0f;
        if (AppMain.ObjObjectPauseCheck(0U) == 0U)
        {
            float num1 = (float)((int)work.water_time - (int)work.water_counter);
            if ((double)num1 != 0.0)
            {
                float num2 = (work.next_water_level - work.now_water_level) / num1;
                work.now_water_level += num2;
                if ((ushort)1 > (ushort)AppMain.MTM_MATH_ABS(work.now_water_level - work.next_water_level))
                {
                    work.water_time = (ushort)0;
                    work.water_counter = (ushort)0;
                }
                else
                    ++work.water_counter;
            }
            else
                work.now_water_level = work.next_water_level;
            if (work.flag_draw)
            {
                AppMain.gmWaterSurfaceGameSystemSetWaterLevel((ushort)work.now_water_level);
                speed = AppMain.amSystemGetFrameRateMain();
            }
            else
            {
                AppMain.gmWaterSurfaceGameSystemSetWaterLevel(ushort.MaxValue);
                return;
            }
        }
        AppMain.OBS_CAMERA obsCamera1 = AppMain.ObjCameraGet(6);
        float x1 = obsCamera1.disp_pos.x;
        float y = obsCamera1.disp_pos.y;
        float dy = -work.now_water_level - y;
        if ((double)dy < (double)-((int)AppMain.OBD_LCD_Y / 2 + 32))
        {
            AppMain.GMS_WATER_SURFACE_MGR mgr = AppMain.gmWaterSurfaceGetMgr();
            if (mgr == null)
                return;
            mgr.render_target = (AppMain.AMS_RENDER_TARGET)null;
        }
        else
        {
            bool flag = false;
            if ((double)dy > (double)AppMain.OBD_LCD_Y * 0.800000011920929)
            {
                y = obsCamera1.disp_pos.y;
                dy = (float)AppMain.OBD_LCD_Y * 0.8f;
                flag = true;
            }
            int roll = obsCamera1.roll;
            float scale = 1f / obsCamera1.scale;
            AppMain.dwaterUpdate(speed, x1, y, dy, roll, scale);
            if (!AppMain.GmMainIsDrawEnable())
                return;
            AppMain.dwaterSetParam();
            uint drawflag = 0;
            AppMain.ObjDraw3DNNSetCameraEx(6, 1, 9U);
            if (work.flag_enable_ref && !flag)
                AppMain.dwaterDrawReflection(9U, drawflag);
            AppMain.ObjDraw3DNNUserFunc(AppMain._gmWaterSurfaceTcbProcPreDrawDT, (object)null, 0, 9U);
            AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, 1, 9U);
            AppMain.ObjDraw3DNNSetCameraEx(6, 1, 4U);
            AppMain.AMS_PARAM_DRAW_PRIMITIVE prim = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
            prim.aTest = (short)0;
            prim.zMask = (short)1;
            prim.zTest = (short)1;
            prim.ablend = 1;
            prim.bldSrc = 770;
            prim.bldDst = 1;
            prim.bldMode = 32774;
            AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(18);
            AppMain.NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            AppMain.OBS_CAMERA obsCamera2 = AppMain.ObjCameraGet(0);
            float x2 = obsCamera2.disp_pos.x;
            float Y = -work.now_water_level;
            float f32 = AppMain.FX_FX32_TO_F32(1310720);
            float num1 = obsCamera2.znear + 1f;
            float num2 = 300f;
            float num3 = Y - (obsCamera2.disp_pos.y - 300f);
            uint num4 = AppMain.AMD_RGBA8888(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)96);
            for (int index1 = 0; index1 < 3; ++index1)
            {
                float num5 = num1 * (float)(index1 + 1);
                int index2 = offset + index1 * 6;
                buffer[index2].Pos.Assign(x2 - num2, Y, f32 - num5);
                buffer[1 + index2].Pos.Assign(x2 + num2, Y, f32 - num5);
                buffer[2 + index2].Pos.Assign(x2 - num2, Y - num3, f32 - num5);
                buffer[5 + index2].Pos.Assign(x2 + num2, Y - num3, f32 - num5);
                buffer[index2].Col = num4;
                buffer[1 + index2].Col = num4;
                buffer[2 + index2].Col = num4;
                buffer[5 + index2].Col = num4;
                float s;
                float c;
                AppMain.nnSinCos(AppMain.NNM_DEGtoA32(AppMain._dmap_water.speed_surface * 360f), out s, out c);
                float num6 = (float)((double)num2 * 2.0 / 128.0 * 0.5);
                float num7;
                float num8;
                float num9;
                float num10;
                if (index1 == 0)
                {
                    float num11 = x2 / 270f;
                    num7 = num11 - ((float)(int)num11 - AppMain._dmap_water.speed_surface);
                    num8 = num7 + num6;
                    num9 = 0.3f;
                    num10 = (float)((double)num9 + (double)num3 / 128.0 * 0.5 + (double)c / 5.0);
                }
                else if (index1 == 1)
                {
                    float num11 = x2 / 270f;
                    num8 = num11 - ((float)(int)num11 + AppMain._dmap_water.speed_surface * 2f);
                    num7 = num8 - num6 * 0.75f;
                    num9 = 1f;
                    num10 = (float)((double)num9 - (double)num3 * 0.75 / 128.0 * 0.5 + (double)s / 5.0);
                }
                else
                {
                    num7 = 11f / 64f;
                    num8 = num7 + 1f / 128f;
                    num9 = 0.5f;
                    num10 = num9 + 1f / 128f;
                    buffer[index2].Col = AppMain.AMD_RGBA8888((byte)32, (byte)176, (byte)64, (byte)112);
                    buffer[1 + index2].Col = buffer[index2].Col;
                    buffer[2 + index2].Col = buffer[index2].Col;
                    buffer[5 + index2].Col = buffer[index2].Col;
                }
                buffer[index2].Tex.u = num7;
                buffer[index2].Tex.v = num9;
                buffer[1 + index2].Tex.u = num8;
                buffer[1 + index2].Tex.v = num9;
                buffer[2 + index2].Tex.u = num7;
                buffer[2 + index2].Tex.v = num10;
                buffer[5 + index2].Tex.u = num8;
                buffer[5 + index2].Tex.v = num10;
                buffer[3 + index2] = buffer[1 + index2];
                buffer[4 + index2] = buffer[2 + index2];
            }
            prim.format3D = 4;
            prim.type = 0;
            prim.vtxPCT3D = nnsPriM3DPctArray;
            prim.count = 18;
            prim.sortZ = -num1;
            prim.texId = 0;
            prim.texlist = AppMain._dmap_water.tex_color.texlist;
            prim.uwrap = 0;
            prim.vwrap = 0;
            AppMain.gmWaterSurfaceMatrixPush(4U);
            AppMain.ObjDraw3DNNDrawPrimitive(prim, 4U);
            AppMain.gmWaterSurfaceMatrixPop(4U);
            if (!flag)
                AppMain.dwaterDrawSurface(4U, drawflag);
            AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, 1, 4U);
            AppMain.ObjDraw3DNNUserFunc(new AppMain.OBF_DRAW_USER_DT_FUNC(AppMain.gmWaterSurfaceTcbProcPostDrawDT), (object)null, 0, 8U);
            AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(prim);
        }
    }

    private static void gmWaterSurfaceMatrixPush(uint command_state)
    {
        AppMain.ObjDraw3DNNUserFunc(AppMain._gmWaterSurfaceUserFuncMatrixPush, (object)null, 0, command_state);
    }

    private static void gmWaterSurfaceMatrixPop(uint command_state)
    {
        AppMain.ObjDraw3DNNUserFunc(AppMain._gmWaterSurfaceUserFuncPop, (object)null, 0, command_state);
    }

    private static void gmWaterSurfaceUserFuncMatrixPush(object param)
    {
        AppMain.amMatrixPush();
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMultiplyMatrix(nnsMatrix, AppMain.amDrawGetWorldViewMatrix(), current);
        AppMain.nnSetPrimitive3DMatrix(nnsMatrix);
    }

    private static void gmWaterSurfaceUserFuncPop(object param)
    {
        AppMain.amMatrixPop();
    }


}