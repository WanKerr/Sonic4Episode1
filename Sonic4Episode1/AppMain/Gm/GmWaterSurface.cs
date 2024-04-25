public partial class AppMain
{
    private static void GmWaterSurfaceInitData(AMS_AMB_HEADER amb)
    {
        gmWaterSurfaceDataInit();
        gmWaterSurfaceDataSetAmbHeader(amb);
    }

    private static void GmWaterSurfaceBuildData()
    {
        if (gmWaterSurfaceGameSystemGetZoneType(gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        dwaterInit();
        AMS_AMB_HEADER ambHeader = gmWaterSurfaceDataGetAmbHeader();
        AoTexBuild(_dmap_water.tex_color, readAMBFile(amBindGet(ambHeader, ambHeader.file_num - 1)));
        AoTexLoad(_dmap_water.tex_color);
        dwaterSetObjectAMB(ambHeader, ambHeader);
        dwaterLoadObject(0U);
    }

    private static bool GmWaterSurfaceCheckLoading()
    {
        bool flag = true;
        if (GsMainSysGetDisplayListRegistNum() >= 192)
            return false;
        if (gmWaterSurfaceGameSystemGetZoneType(gmWaterSurfaceGameSystemGetStageId()) == 2)
        {
            GMS_WATER_SURFACE_DATA info = gmWaterSurfaceDataGetInfo();
            if (!AoTexIsLoaded(_dmap_water.tex_color))
                flag = false;
            else if (!info.flag_load_object)
            {
                if (dwaterLoadObject(0U) == 0)
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
        gmWaterSurfaceGameSystemSetWaterLevel(ushort.MaxValue);
        if (gmWaterSurfaceGameSystemGetZoneType(gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        gmWaterSurfaceInitMgr();
        gmWaterSurfaceCreateTcb();
    }

    private static void GmWaterSurfaceExit()
    {
        gmWaterSurfaceExitMgr();
    }

    private static void GmWaterSurfaceFlushData()
    {
        if (gmWaterSurfaceGameSystemGetZoneType(gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        AoTexRelease(_dmap_water.tex_color);
    }

    private static void GmWaterSurfaceRelease()
    {
        if (gmWaterSurfaceGameSystemGetZoneType(gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        gmWaterSurfaceDataRelease();
    }

    private static bool GmWaterSurfaceCheckFlush()
    {
        if (GsMainSysGetDisplayListRegistNum() >= 192)
            return false;
        bool flag = true;
        if (gmWaterSurfaceGameSystemGetZoneType(gmWaterSurfaceGameSystemGetStageId()) == 2 && _dmap_water != null)
        {
            if (!AoTexIsReleased(_dmap_water.tex_color))
                flag = false;
            else if (_dmap_water.regist_index == -1)
            {
                _dmap_water.regist_index = dwaterRelease();
                int num = flag ? 1 : 0;
                flag = false;
            }
            else if (!amDrawIsRegistComplete(_dmap_water.regist_index))
            {
                int num = flag ? 1 : 0;
                flag = false;
            }
            else
                dwaterExit();
        }
        return flag;
    }

    private static void GmWaterSurfaceRequestChangeWaterLevel(
      ushort water_level,
      ushort time,
      bool flag_add_time)
    {
        if (gmWaterSurfaceGameSystemGetZoneType(gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        GMS_WATER_SURFACE_MGR mgr = gmWaterSurfaceGetMgr();
        if (mgr == null)
            return;
        GMS_WATER_SURFACE_INFO work = (GMS_WATER_SURFACE_INFO)mgr.tcb_water.work;
        if (flag_add_time)
        {
            work.water_time += time;
        }
        else
        {
            work.water_time = time;
            work.water_counter = 0;
        }
        work.next_water_level = water_level;
        work.now_water_level = gmWaterSurfaceGameSystemGetWaterLevel();
    }

    private static void GmWaterSurfaceRequestAddWatarLevel(
      float water_level,
      ushort time,
      bool flag_add_time)
    {
        if (gmWaterSurfaceGameSystemGetZoneType(gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        GMS_WATER_SURFACE_MGR mgr = gmWaterSurfaceGetMgr();
        if (mgr == null)
            return;
        GMS_WATER_SURFACE_INFO work = (GMS_WATER_SURFACE_INFO)mgr.tcb_water.work;
        if (flag_add_time)
        {
            work.water_time += time;
        }
        else
        {
            work.water_time = time;
            work.water_counter = 0;
        }
        work.next_water_level += water_level;
        work.now_water_level = gmWaterSurfaceGameSystemGetWaterLevel();
    }

    private static void GmWaterSurfaceSetFlagDraw(bool flag_draw)
    {
        if (gmWaterSurfaceGameSystemGetZoneType(gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        GMS_WATER_SURFACE_MGR mgr = gmWaterSurfaceGetMgr();
        if (mgr == null)
            return;
        ((GMS_WATER_SURFACE_INFO)mgr.tcb_water.work).flag_draw = flag_draw;
    }

    private static void GmWaterSurfaceSetFlagEnableRef(bool flag_enable_ref)
    {
        if (gmWaterSurfaceGameSystemGetZoneType(gmWaterSurfaceGameSystemGetStageId()) != 2)
            return;
        GMS_WATER_SURFACE_MGR mgr = gmWaterSurfaceGetMgr();
        if (mgr == null)
            return;
        ((GMS_WATER_SURFACE_INFO)mgr.tcb_water.work).flag_enable_ref = flag_enable_ref;
    }

    private static AMS_RENDER_TARGET GmWaterSurfaceGetRenderTarget()
    {
        return gmWaterSurfaceGetMgr()?.render_target;
    }

    private static void GmWaterSurfaceDrawNoWaterField(
      float left,
      float top,
      float right,
      float bottom)
    {
        AMS_PARAM_DRAW_PRIMITIVE setParam = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.aTest = 0;
        setParam.zMask = 0;
        setParam.zTest = 1;
        setParam.ablend = 1;
        setParam.noSort = 1;
        NNS_PRIM3D_PC[] nnsPriM3DPcArray = amDrawAlloc_NNS_PRIM3D_PC(6);
        float f32 = FX_FX32_TO_F32(1310720);
        nnsPriM3DPcArray[0].Pos.Assign(left, top, f32);
        nnsPriM3DPcArray[1].Pos.Assign(right, top, f32);
        nnsPriM3DPcArray[2].Pos.Assign(left, bottom, f32);
        nnsPriM3DPcArray[5].Pos.Assign(right, bottom, f32);
        uint num = AMD_RGBA8888(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
        nnsPriM3DPcArray[0].Col = num;
        nnsPriM3DPcArray[1].Col = num;
        nnsPriM3DPcArray[2].Col = num;
        nnsPriM3DPcArray[5].Col = num;
        nnsPriM3DPcArray[3] = nnsPriM3DPcArray[1];
        nnsPriM3DPcArray[4] = nnsPriM3DPcArray[2];
        setParam.format3D = 2;
        setParam.type = 0;
        setParam.vtxPC3D = nnsPriM3DPcArray;
        setParam.texlist = null;
        setParam.texId = 0;
        setParam.count = 6;
        setParam.sortZ = -1f;
        gmWaterSurfaceMatrixPush(9U);
        amDrawPrimitive3D(9U, setParam);
        gmWaterSurfaceMatrixPop(9U);
        GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    private static int gmWaterSurfaceGameSystemGetStageId()
    {
        return g_gs_main_sys_info.stage_id;
    }

    private static int gmWaterSurfaceGameSystemGetZoneType(int stage_id)
    {
        return g_gm_gamedat_zone_type_tbl[stage_id];
    }

    private static ushort gmWaterSurfaceGameSystemGetWaterLevel()
    {
        return g_gm_main_system.water_level;
    }

    private static void gmWaterSurfaceGameSystemSetWaterLevel(ushort water_level)
    {
        g_gm_main_system.water_level = water_level;
    }

    private static void gmWaterSurfaceDataInit()
    {
        g_water_surface_data_real.Clear();
        g_water_surface_data = g_water_surface_data_real;
    }

    private static void gmWaterSurfaceDataRelease()
    {
        if (g_water_surface_data == null)
            return;
        gmWaterSurfaceDataReleaseAmbHeader();
        g_water_surface_data = null;
    }

    private static GMS_WATER_SURFACE_DATA gmWaterSurfaceDataGetInfo()
    {
        return g_water_surface_data;
    }

    private static void gmWaterSurfaceDataSetAmbHeader(AMS_AMB_HEADER amb)
    {
        gmWaterSurfaceDataGetInfo().amb_header = amb;
    }

    private static AMS_AMB_HEADER gmWaterSurfaceDataGetAmbHeader()
    {
        return gmWaterSurfaceDataGetInfo().amb_header;
    }

    private static void gmWaterSurfaceDataReleaseAmbHeader()
    {
        GMS_WATER_SURFACE_DATA info = gmWaterSurfaceDataGetInfo();
        if (info.amb_header == null)
            return;
        info.amb_header = null;
    }

    private static GMS_WATER_SURFACE_MGR gmWaterSurfaceGetMgr()
    {
        return g_water_surface_mgr;
    }

    private static void gmWaterSurfaceInitMgr()
    {
        g_water_surface_mgr_real.Clear();
        g_water_surface_mgr = g_water_surface_mgr_real;
    }

    private static void gmWaterSurfaceExitMgr()
    {
        if (g_water_surface_mgr == null)
            return;
        gmWaterSurfaceDeleteTcb();
        g_water_surface_mgr = null;
    }

    private static MTS_TASK_TCB gmWaterSurfaceCreateTcb()
    {
        GMS_WATER_SURFACE_MGR mgr = gmWaterSurfaceGetMgr();
        mgr.tcb_water = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmWaterSurfaceProc), null, 0U, 0, 8202U, 5, () => new GMS_WATER_SURFACE_INFO(), "GM WATER SURFACE");
        GMS_WATER_SURFACE_INFO work = (GMS_WATER_SURFACE_INFO)mgr.tcb_water.work;
        work.now_water_level = gmWaterSurfaceGameSystemGetWaterLevel();
        work.next_water_level = ushort.MaxValue;
        work.water_time = 0;
        work.water_counter = 0;
        work.flag_draw = true;
        work.flag_enable_ref = true;
        return mgr.tcb_water;
    }

    private static void gmWaterSurfaceDeleteTcb()
    {
        GMS_WATER_SURFACE_MGR mgr = gmWaterSurfaceGetMgr();
        if (mgr.tcb_water == null)
            return;
        mtTaskClearTcb(mgr.tcb_water);
        mgr.tcb_water = null;
    }

    private static void gmWaterSurfaceTcbProcPreDrawDT(object data)
    {
        AMS_RENDER_TARGET target = _am_render_manager.targetp != _gm_mapFar_render_work ? _gm_mapFar_render_work : _am_draw_target;
        if (target.width == 0)
            return;
        amDrawEndScene();
        NNS_RGBA_U8 color = new NNS_RGBA_U8(0, 0, 0, byte.MaxValue);
        amRenderCopyTarget(target, color);
        GMS_WATER_SURFACE_MGR mgr = gmWaterSurfaceGetMgr();
        if (mgr == null)
            return;
        mgr.render_target = target;
    }

    private static void gmWaterSurfaceTcbProcDrawDT(object data)
    {
        GMS_WATER_SURFACE_MGR mgr = gmWaterSurfaceGetMgr();
        if (mgr == null || mgr.render_target == null || mgr.render_target.width == 0)
            return;
        dwaterDrawWater(mgr.render_target);
    }

    private static void gmWaterSurfaceTcbProcPostDrawDT(object data)
    {
    }

    private static void gmWaterSurfaceProc(MTS_TASK_TCB tcb)
    {
        GMS_WATER_SURFACE_INFO work = (GMS_WATER_SURFACE_INFO)tcb.work;
        float speed = 0.0f;
        if (ObjObjectPauseCheck(0U) == 0U)
        {
            float num1 = work.water_time - work.water_counter;
            if (num1 != 0.0)
            {
                float num2 = (work.next_water_level - work.now_water_level) / num1;
                work.now_water_level += num2;
                if (1 > (ushort)MTM_MATH_ABS(work.now_water_level - work.next_water_level))
                {
                    work.water_time = 0;
                    work.water_counter = 0;
                }
                else
                    ++work.water_counter;
            }
            else
                work.now_water_level = work.next_water_level;
            if (work.flag_draw)
            {
                gmWaterSurfaceGameSystemSetWaterLevel((ushort)work.now_water_level);
                speed = amSystemGetFrameRateMain();
            }
            else
            {
                gmWaterSurfaceGameSystemSetWaterLevel(ushort.MaxValue);
                return;
            }
        }
        OBS_CAMERA obsCamera1 = ObjCameraGet(6);
        float x1 = obsCamera1.disp_pos.x;
        float y = obsCamera1.disp_pos.y;
        float dy = -work.now_water_level - y;
        if (dy < (double)-(OBD_LCD_Y / 2 + 32))
        {
            GMS_WATER_SURFACE_MGR mgr = gmWaterSurfaceGetMgr();
            if (mgr == null)
                return;
            mgr.render_target = null;
        }
        else
        {
            bool flag = false;
            if (dy > OBD_LCD_Y * 0.800000011920929)
            {
                y = obsCamera1.disp_pos.y;
                dy = OBD_LCD_Y * 0.8f;
                flag = true;
            }
            int roll = obsCamera1.roll;
            float scale = 1f / obsCamera1.scale;
            dwaterUpdate(speed, x1, y, dy, roll, scale);
            if (!GmMainIsDrawEnable())
                return;
            dwaterSetParam();
            uint drawflag = 0;
            ObjDraw3DNNSetCameraEx(6, 1, 9U);

            if (work.flag_enable_ref && !flag)
                dwaterDrawReflection(9U, drawflag);

            ObjDraw3DNNUserFunc(_gmWaterSurfaceTcbProcPreDrawDT, null, 0, 9U);
            ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, 1, 9U);
            ObjDraw3DNNSetCameraEx(6, 1, 4U);
            AMS_PARAM_DRAW_PRIMITIVE prim = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
            prim.aTest = 0;
            prim.zMask = 1;
            prim.zTest = 1;
            prim.ablend = 1;
            prim.bldSrc = 770;
            prim.bldDst = 1;
            prim.bldMode = 32774;
            NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = amDrawAlloc_NNS_PRIM3D_PCT(18);
            NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            OBS_CAMERA obsCamera2 = ObjCameraGet(0);
            float x2 = obsCamera2.disp_pos.x;
            float Y = -work.now_water_level;
            float f32 = FX_FX32_TO_F32(1310720);
            float num1 = obsCamera2.znear + 1f;
            float num2 = 300f;
            float num3 = Y - (obsCamera2.disp_pos.y - 300f);
            uint num4 = AMD_RGBA8888(byte.MaxValue, byte.MaxValue, byte.MaxValue, 96);
            for (int index1 = 0; index1 < 3; ++index1)
            {
                float num5 = num1 * (index1 + 1);
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
                nnSinCos(NNM_DEGtoA32(_dmap_water.speed_surface * 360f), out s, out c);
                float num6 = (float)(num2 * 2.0 / 128.0 * 0.5);
                float num7;
                float num8;
                float num9;
                float num10;
                if (index1 == 0)
                {
                    float num11 = x2 / 270f;
                    num7 = num11 - ((int)num11 - _dmap_water.speed_surface);
                    num8 = num7 + num6;
                    num9 = 0.3f;
                    num10 = (float)(num9 + num3 / 128.0 * 0.5 + c / 5.0);
                }
                else if (index1 == 1)
                {
                    float num11 = x2 / 270f;
                    num8 = num11 - ((int)num11 + _dmap_water.speed_surface * 2f);
                    num7 = num8 - num6 * 0.75f;
                    num9 = 1f;
                    num10 = (float)(num9 - num3 * 0.75 / 128.0 * 0.5 + s / 5.0);
                }
                else
                {
                    num7 = 11f / 64f;
                    num8 = num7 + 1f / 128f;
                    num9 = 0.5f;
                    num10 = num9 + 1f / 128f;
                    buffer[index2].Col = AMD_RGBA8888(32, 176, 64, 112);
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
            prim.texlist = _dmap_water.tex_color.texlist;
            prim.uwrap = 0;
            prim.vwrap = 0;
            gmWaterSurfaceMatrixPush(4U);
            ObjDraw3DNNDrawPrimitive(prim, 4U);
            gmWaterSurfaceMatrixPop(4U);
            if (!flag)
                dwaterDrawSurface(4U, drawflag);
            ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, 1, 4U);
            ObjDraw3DNNUserFunc(new OBF_DRAW_USER_DT_FUNC(gmWaterSurfaceTcbProcPostDrawDT), null, 0, 8U);
            GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(prim);
        }
    }

    private static void gmWaterSurfaceMatrixPush(uint command_state)
    {
        ObjDraw3DNNUserFunc(_gmWaterSurfaceUserFuncMatrixPush, null, 0, command_state);
    }

    private static void gmWaterSurfaceMatrixPop(uint command_state)
    {
        ObjDraw3DNNUserFunc(_gmWaterSurfaceUserFuncPop, null, 0, command_state);
    }

    private static void gmWaterSurfaceUserFuncMatrixPush(object param)
    {
        amMatrixPush();
        NNS_MATRIX current = amMatrixGetCurrent();
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        nnMultiplyMatrix(nnsMatrix, amDrawGetWorldViewMatrix(), current);
        nnSetPrimitive3DMatrix(nnsMatrix);
    }

    private static void gmWaterSurfaceUserFuncPop(object param)
    {
        amMatrixPop();
    }


}