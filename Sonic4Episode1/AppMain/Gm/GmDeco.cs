using System;

public partial class AppMain
{
    private static void GmDecoInit()
    {
        gmDecoInitMgr();
        ClearArray(g_deco_fall_manager, 0, 16);
    }

    public static void GmDecoExit()
    {
        gmDecoExitMgr();
        gmDeco_motionHeader = null;
        gmDeco_matMotionHeader = null;
    }

    public static void gmDecoDrawServerMain(MTS_TASK_TCB tcb)
    {
        if (!GmMainIsDrawEnable())
            return;
        uint command = 0;
        float syncTime = gmDecoGameSystemGetSyncTime();
        for (int index1 = 0; index1 < 16; ++index1)
        {
            GMS_DECO_FALL_MANAGER gmsDecoFallManager = g_deco_fall_manager[index1];
            if (gmsDecoFallManager.dec_id != 0U)
            {
                switch (gmsDecoFallManager.dec_id)
                {
                    case 20:
                    case 40:
                    case 41:
                    case 42:
                        command = 9U;
                        break;
                    case 27:
                    case 47:
                    case 48:
                    case 49:
                        command = 12U;
                        ObjDraw3DNNSetCameraEx(0, 1, 12U);
                        break;
                }
                VecFx32 vecFx32_1 = new VecFx32();
                vecFx32_1.Assign(gmsDecoFallManager.reg[0].vec);
                NNS_TEXLIST texlist = gmsDecoFallManager.texlist;
                NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
                nnMakeUnitMatrix(nnsMatrix);
                nnTranslateMatrix(nnsMatrix, nnsMatrix, FX_FX32_TO_F32(vecFx32_1.x), FX_FX32_TO_F32(vecFx32_1.y), FX_FX32_TO_F32(vecFx32_1.z));
                AMS_PARAM_DRAW_PRIMITIVE prim = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
                prim.type = 0;
                prim.count = 6 * gmsDecoFallManager.reg_num;
                prim.ablend = 1;
                prim.bldSrc = 770;
                prim.bldDst = 771;
                prim.bldMode = 32774;
                prim.aTest = 0;
                prim.zMask = 0;
                prim.zTest = 1;
                prim.noSort = 1;
                prim.uwrap = 1;
                prim.vwrap = 0;
                prim.texlist = texlist;
                prim.texId = texlist.nTex - 1;
                prim.vtxPCT3D = amDrawAlloc_NNS_PRIM3D_PCT(prim.count);
                NNS_PRIM3D_PCT[] buffer = prim.vtxPCT3D.buffer;
                int offset = prim.vtxPCT3D.offset;
                prim.format3D = 4;
                uint num1 = 0;
                float num2 = (float)(fmod(syncTime, gmsDecoFallManager.frame) / (double)gmsDecoFallManager.frame * 5.02799081802368);
                NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
                VecFx32 vecFx32_2 = new VecFx32();
                for (uint index2 = 0; index2 < 8U; ++index2)
                {
                    vecFx32_2.Assign(gmsDecoFallManager.reg[(int)index2].vec);
                    if (vecFx32_2.x != 0)
                    {
                        int index3 = offset + 6 * (int)num1;
                        buffer[index3].Tex.u = buffer[index3 + 1].Tex.u = 1f;
                        buffer[index3 + 2].Tex.u = buffer[index3 + 3].Tex.u = 0.0f;
                        buffer[index3].Tex.v = buffer[index3 + 2].Tex.v = -1f * gmsDecoFallManager.reg[(int)index2].num - num2;
                        buffer[index3 + 1].Tex.v = buffer[index3 + 3].Tex.v = 0.0f - num2;
                        buffer[index3].Col = uint.MaxValue;
                        buffer[index3 + 1].Col = buffer[index3 + 2].Col = buffer[index3 + 3].Col = buffer[index3].Col;
                        nnsVector.x = FX_FX32_TO_F32(vecFx32_2.x - vecFx32_1.x);
                        nnsVector.y = FX_FX32_TO_F32(vecFx32_2.y - vecFx32_1.y);
                        nnsVector.z = FX_FX32_TO_F32(vecFx32_2.z - vecFx32_1.z);
                        buffer[index3].Pos.x = buffer[index3 + 1].Pos.x = 32f + nnsVector.x;
                        buffer[index3 + 2].Pos.x = buffer[index3 + 3].Pos.x = nnsVector.x - 32f;
                        buffer[index3].Pos.y = buffer[index3 + 2].Pos.y = 32f + nnsVector.y;
                        buffer[index3 + 1].Pos.y = buffer[index3 + 3].Pos.y = (float)(nnsVector.y - 32.0 + -64.0 * (gmsDecoFallManager.reg[(int)index2].num - 1U));
                        buffer[index3].Pos.z = buffer[index3 + 1].Pos.z = buffer[index3 + 2].Pos.z = buffer[index3 + 3].Pos.z = 1f + nnsVector.z;
                        buffer[index3 + 4] = buffer[index3 + 2];
                        buffer[index3 + 5] = buffer[index3 + 3];
                        buffer[index3 + 3] = buffer[index3 + 1];
                        gmsDecoFallManager.reg[(int)index2].vec.y = 0;
                        ++num1;
                    }
                }
                GlobalPool<NNS_VECTOR>.Release(nnsVector);
                amMatrixPush(nnsMatrix);
                GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
                ObjDraw3DNNDrawPrimitive(prim, command, 0, 0);
                amMatrixPop();
                GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(prim);
            }
        }
        gmDecoExecuteDrawPrimitive();
        gmDecoInitDrawPrimitive();
    }

    private static uint gmDecoGameSystemGetSyncTime()
    {
        return g_gm_main_system.sync_time;
    }

    private static void gmDecoExecuteDrawPrimitive()
    {
        GMS_DECO_PRIM_DRAW_WORK[] gDecoTvxWork = g_deco_tvx_work;
        if (gDecoTvxWork[0].tex == null)
            return;
        SNNS_MATRIX dst1 = new SNNS_MATRIX();
        SNNS_MATRIX dst2 = new SNNS_MATRIX();
        SNNS_MATRIX snnsMatrix = new SNNS_MATRIX();
        nnMakeUnitMatrix(ref snnsMatrix);
        nnMakeScaleMatrix(out dst1, -1f, 1f, 1f);
        nnMakeScaleMatrix(out dst2, 1f, -1f, 1f);
        uint lightColor = GmMainGetLightColor();
        _AMS_PARAM_DRAW_PRIMITIVE.ablend = 0;
        _AMS_PARAM_DRAW_PRIMITIVE.bldSrc = 770;
        _AMS_PARAM_DRAW_PRIMITIVE.bldDst = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.bldMode = 32774;
        _AMS_PARAM_DRAW_PRIMITIVE.aTest = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.zMask = 0;
        _AMS_PARAM_DRAW_PRIMITIVE.zTest = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.noSort = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.uwrap = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.vwrap = 1;
        _AMS_PARAM_DRAW_PRIMITIVE.format3D = 4;
        SNNS_VECTOR src = new SNNS_VECTOR();
        for (uint index1 = 0; index1 < 16U && gDecoTvxWork[(int)index1].tex_id != -1; ++index1)
        {
            if (gDecoTvxWork[(int)index1].tex_id != 0)
            {
                _AMS_PARAM_DRAW_PRIMITIVE.ablend = 1;
                _AMS_PARAM_DRAW_PRIMITIVE.aTest = 0;
            }
            else
            {
                _AMS_PARAM_DRAW_PRIMITIVE.ablend = 0;
                _AMS_PARAM_DRAW_PRIMITIVE.aTest = 1;
            }
            _AMS_PARAM_DRAW_PRIMITIVE.texlist = gDecoTvxWork[(int)index1].tex.texlist;
            _AMS_PARAM_DRAW_PRIMITIVE.type = 1;
            _AMS_PARAM_DRAW_PRIMITIVE.count = gDecoTvxWork[(int)index1].all_vtx_num + gDecoTvxWork[(int)index1].stack_num * 2 - 2;
            if (_gmDecoExecuteDrawPrimitive.v_tbl[(int)index1] == null || _gmDecoExecuteDrawPrimitive.v_tbl[(int)index1].Length < _AMS_PARAM_DRAW_PRIMITIVE.count)
                _gmDecoExecuteDrawPrimitive.v_tbl[(int)index1] = new NNS_PRIM3D_PCT[_AMS_PARAM_DRAW_PRIMITIVE.count];
            _gmDecoExecuteDrawPrimitive.v_tbl_array[(int)index1].buffer = _gmDecoExecuteDrawPrimitive.v_tbl[(int)index1];
            NNS_PRIM3D_PCT[] buffer = _gmDecoExecuteDrawPrimitive.v_tbl_array[(int)index1].buffer;
            int num1 = 0;
            _AMS_PARAM_DRAW_PRIMITIVE.vtxPCT3D = _gmDecoExecuteDrawPrimitive.v_tbl_array[(int)index1];
            _AMS_PARAM_DRAW_PRIMITIVE.texId = gDecoTvxWork[(int)index1].tex_id;
            for (uint index2 = 0; index2 < gDecoTvxWork[(int)index1].stack_num; ++index2)
            {
                GMS_DECO_PRIM_DRAW_STACK decoPrimDrawStack = gDecoTvxWork[(int)index1].stack[(int)index2];
                int num2 = (int)decoPrimDrawStack.vtx_num / 3;
                float num3 = FXM_FX32_TO_FLOAT(decoPrimDrawStack.pos.x);
                float num4 = -FXM_FX32_TO_FLOAT(decoPrimDrawStack.pos.y) + decoPrimDrawStack.off_y;
                float num5 = FXM_FX32_TO_FLOAT(decoPrimDrawStack.pos.z);
                int num6 = num1;
                AOS_TVX_VERTEX[] vtx = decoPrimDrawStack.vtx;
                for (int index3 = 0; index3 < decoPrimDrawStack.vtx_num; ++index3)
                {
                    src.x = vtx[index3].x;
                    src.y = vtx[index3].y;
                    src.z = vtx[index3].z;
                    int index4 = num6 + index3;
                    if (((int)decoPrimDrawStack.disp_flag & 1) != 0)
                        nnTransformVector(ref buffer[index4].Pos, ref dst1, ref src);
                    else if (((int)decoPrimDrawStack.disp_flag & 2) != 0)
                        nnTransformVector(ref buffer[index4].Pos, ref dst2, ref src);
                    else
                        buffer[index4].Pos.Assign(src.x, src.y, src.z);
                    buffer[index4].Pos.x += num3;
                    buffer[index4].Pos.y += num4;
                    buffer[index4].Pos.z += num5;
                    buffer[index4].Tex.u = vtx[index3].u;
                    buffer[index4].Tex.v = vtx[index3].v;
                    buffer[index4].Col = vtx[index3].c & lightColor;
                }
                num1 = num1 + (int)decoPrimDrawStack.vtx_num + 2;
                if (index2 != 0U)
                {
                    int index3 = num6 - 1;
                    buffer[index3] = buffer[index3 + 1];
                }
                if (index2 != gDecoTvxWork[(int)index1].stack_num - 1)
                {
                    int index3 = num6 + (int)decoPrimDrawStack.vtx_num - 1;
                    buffer[index3 + 1] = buffer[index3];
                }
            }
            ObjDraw3DNNSetCameraEx(0, 1, gDecoTvxWork[(int)index1].command);
            amMatrixPush(ref snnsMatrix);
            ObjDraw3DNNDrawPrimitive(_AMS_PARAM_DRAW_PRIMITIVE, gDecoTvxWork[(int)index1].command, 0, 0);
            amMatrixPop();
        }
    }

    private static void gmDecoInitDrawPrimitive()
    {
        GMS_DECO_PRIM_DRAW_WORK[] gDecoTvxWork = g_deco_tvx_work;
        for (int index = 0; index < 16; ++index)
        {
            gDecoTvxWork[index].tex = null;
            gDecoTvxWork[index].tex_id = -1;
            gDecoTvxWork[index].command = uint.MaxValue;
            gDecoTvxWork[index].all_vtx_num = 0;
            gDecoTvxWork[index].stack_num = 0;
        }
    }

    private static void gmDecoExitMgr()
    {
        if (g_deco_mgr == null)
            return;
        if (g_deco_mgr.se_handle != null)
        {
            GmSoundStopSE(g_deco_mgr.se_handle);
            GsSoundFreeSeHandle(g_deco_mgr.se_handle);
            g_deco_mgr.se_handle = null;
        }
        gmDecoDeleteTcbPost();
        g_deco_mgr = null;
    }

    private static void GmDecoSetLightFinalZone()
    {
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.x = 0.0f;
        nnsVector.y = -0.3f;
        nnsVector.z = -0.4f;
        nnNormalizeVector(nnsVector, nnsVector);
        ObjDrawSetParallelLight(NNE_LIGHT_2, ref col, 0.8f, nnsVector);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    private static void gmDecoDeleteTcbPost()
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr.tcb_post == null)
            return;
        mtTaskClearTcb(mgr.tcb_post);
        mgr.tcb_post = null;
    }

    private static GMS_DECO_MGR gmDecoGetMgr()
    {
        return g_deco_mgr;
    }

    private static OBS_OBJECT_WORK GmDecoInitModel(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return gmDecoInitModel(dec_rec, x, y, type)?.obj_work;
    }

    private static OBS_OBJECT_WORK GmDecoInitModelMaterial(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return gmDecoInitModelMaterial(dec_rec, x, y, type).obj_work;
    }

    private static OBS_OBJECT_WORK GmDecoInitModelMotionMaterial(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return gmDecoInitModelMotioinMaterial(dec_rec, x, y, type).obj_work;
    }

    private static OBS_OBJECT_WORK GmDecoInitPrimitive3D(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return null;
    }

    private static OBS_OBJECT_WORK GmDecoInitModelMotionTouch(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return gmDecoInitModel(dec_rec, x, y, type)?.obj_work;
    }

    private static OBS_OBJECT_WORK GmDecoInitFall(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        GMS_DECO_WORK gmsDecoWork = gmDecoInitFall(dec_rec, x, y, type);
        OBS_OBJECT_WORK objWork = gmsDecoWork.obj_work;
        OBS_ACTION3D_NN_WORK obj3d = gmsDecoWork.obj_3d;
        float frame = amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
        gmDecoAddFallEvent(dec_rec, objWork.pos.x + objWork.ofst.x, obj3d.texlist, frame);
        return gmsDecoWork.obj_work;
    }

    private static OBS_OBJECT_WORK GmDecoInitEffect(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return gmDecoInitEffect(dec_rec, x, y, type).obj_work;
    }

    private static OBS_OBJECT_WORK GmDecoInitEffectBlockAndNext(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return gmDecoInitEffectBlockAndNext(dec_rec, x, y, type).obj_work;
    }

    private static OBS_OBJECT_WORK GmDecoInitEffectBlock(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return gmDecoInitEffectBlock(dec_rec, x, y, type).obj_work;
    }

    private static OBS_OBJECT_WORK GmDecoInitModelEffect(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return gmDecoInitModelEffect(dec_rec, x, y, type).obj_work;
    }

    private static OBS_OBJECT_WORK GmDecoInitModelMotionMaterialTouch(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        GMS_DECO_WORK deco_work = gmDecoInitModelMotioinMaterial(dec_rec, x, y, type);
        if (deco_work == null)
            return null;
        ((OBS_OBJECT_WORK)deco_work).flag &= 4294967293U;
        int id = dec_rec.id;
        short num1 = g_gm_deco_rect_size[id][0];
        short num2 = g_gm_deco_rect_size[id][1];
        short left = (short)(-num1 / 2);
        short top = (short)(-num2 / 2);
        short right = (short)(num1 / 2);
        short bottom = (short)(num2 / 2);
        gmDecoSetRect(deco_work, left, top, -500, right, bottom, 500, g_gm_deco_func_rect[id]);
        return (OBS_OBJECT_WORK)deco_work;
    }

    private static OBS_OBJECT_WORK GmDecoInitModelLoop(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return gmDecoInitModelLoop(dec_rec, x, y, type).obj_work;
    }

    private static void GmDecoInitData(AMS_AMB_HEADER amb)
    {
        gmDecoDataInit();
        gmDecoDataSetAmbHeader(amb);
        gmDecoInitDrawPrimitive();
    }

    private static void GmDecoBuildData()
    {
        int num = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        GMS_DECO_DATA info = gmDecoDataGetInfo();
        AMS_AMB_HEADER header = null;
        if (info != null)
            header = gmDecoDataGetAmbHeader();
        string sPath;
        if (gmDecoIsUseModel(0))
        {
            info.tvx_model = readAMBFile(amBindGet(header, 0, out sPath));
            info.tvx_model_data = new TVX_FILE[info.tvx_model.file_num];
            info.tvx_model.dir = sPath;
            AoTexBuild(info.tvx_tex, readAMBFile(amBindGet(header, 1)));
            AoTexLoad(info.tvx_tex);
        }
        if (!gmDecoIsUseModel(1))
            return;
        AMS_AMB_HEADER mdl_amb = readAMBFile(amBindGet(header, 4, out sPath));
        mdl_amb.dir = sPath;
        AMS_AMB_HEADER tex_amb = readAMBFile(amBindGet(header, 5, out sPath));
        tex_amb.dir = sPath;
        uint draw_flag = 0;
        info.obj_3d_list_fall = GmGameDBuildRegBuildModel(mdl_amb, tex_amb, draw_flag);
    }

    private static bool GmDecoCheckLoading()
    {
        GMS_DECO_DATA info = gmDecoDataGetInfo();
        if (gmDecoIsUseModel(0) && !AoTexIsLoaded(info.tvx_tex))
            return false;
        gmDecoIsUseModel(1);
        return true;
    }

    private static void GmDecoRelease()
    {
        GmDecoGlareDataRelease();
        gmDecoDataRelease();
        gmDecoReleaseMgr();
    }

    private static void GmDecoFlushData()
    {
        int num = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        GMS_DECO_DATA info = gmDecoDataGetInfo();
        AMS_AMB_HEADER header = null;
        if (info != null)
            header = gmDecoDataGetAmbHeader();
        if (gmDecoIsUseModel(0))
            AoTexRelease(info.tvx_tex);
        if (!gmDecoIsUseModel(1))
            return;
        string sPath;
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(amBindGet(header, 4, out sPath));
        amsAmbHeader.dir = sPath;
        GmGameDBuildRegFlushModel(info.obj_3d_list_fall, amsAmbHeader.file_num);
        info.obj_3d_list_fall = null;
    }

    private static bool GmDecoCheckFlushing()
    {
        GMS_DECO_DATA info = gmDecoDataGetInfo();
        return !gmDecoIsUseModel(0) || AoTexIsReleased(info.tvx_tex);
    }

    private static OBS_OBJECT_WORK GmDecoInitModelMotion(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return null;
    }

    private static void GmDecoSetFrameMotion(int frame, int index)
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.common_frame_motion[index] = frame;
    }

    private static void GmDecoStartEffectFinalBossLight()
    {
        GmDecoSetFrameMotion(1, 2);
    }

    private static AMS_RENDER_TARGET GmDecoGetFallRenderTarget()
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return null;
        return !mgr.flag_render_front ? (mgr.render_target_back == null ? gmDecoDrawFallCopyRenderFront() : mgr.render_target_back) : mgr.render_target_front;
    }

    private static void GmDecoSetLoopState()
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        int index = 0;
        OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, 4);
        while (obj_work != null)
        {
            if (obj_work.ppFunc == new MPP_VOID_OBS_OBJECT_WORK(gmDecoMainFuncLoop))
            {
                if (index < 2)
                {
                    mgr.motion_frame_loop[index] = 1;
                    ++index;
                    continue;
                }
                if (index < 12)
                {
                    mgr.motion_frame_loop[index] = obj_work.user_timer;
                    ++index;
                }
                obj_work.flag |= 8U;
                obj_work.disp_flag |= 32U;
                GMS_DECO_WORK gmsDecoWork = (GMS_DECO_WORK)obj_work;
                GMS_EVE_RECORD_DECORATE eventRecord = gmsDecoWork.event_record;
                if (eventRecord != null)
                {
                    if (eventRecord.pos_x == byte.MaxValue)
                    {
                        eventRecord.pos_x = gmsDecoWork.event_x;
                        gmsDecoWork.event_x = 0;
                    }
                    gmsDecoWork.event_record = null;
                }
            }
            obj_work = ObjObjectSearchRegistObject(obj_work, 4);
        }
    }

    private static void GmDecoClearLoopState()
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        for (int index = 0; index < mgr.motion_frame_loop.Length; ++index)
            mgr.motion_frame_loop[index] = 0;
    }

    private static void GmDecoStartLoop()
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.state_loop = 1;
    }

    private static void GmDecoEndLoop()
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.state_loop = 2;
    }

    private static void gmDecoDataInit()
    {
        g_deco_data_real.Clear();
        g_deco_data = g_deco_data_real;
    }

    private static void gmDecoDataRelease()
    {
        if (g_deco_data == null)
            return;
        gmDecoDataReleaseAmbHeader();
        g_deco_data = null;
    }

    private static GMS_DECO_DATA gmDecoDataGetInfo()
    {
        return g_deco_data;
    }

    private static void gmDecoDataSetAmbHeader(AMS_AMB_HEADER amb)
    {
        gmDecoDataGetInfo().amb_header = amb;
    }

    private static AMS_AMB_HEADER gmDecoDataGetAmbHeader()
    {
        return gmDecoDataGetInfo().amb_header;
    }

    private static void gmDecoDataReleaseAmbHeader()
    {
        GMS_DECO_DATA info = gmDecoDataGetInfo();
        if (info.amb_header == null)
            return;
        info.amb_header = null;
    }

    private static OBS_ACTION3D_NN_WORK[] gmDecoDataGetObj3DList(int id)
    {
        GMS_DECO_DATA info = gmDecoDataGetInfo();
        OBS_ACTION3D_NN_WORK[] obsActioN3DNnWorkArray = null;
        switch (id)
        {
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
                obsActioN3DNnWorkArray = GmGmkWaterSliderGetObj3DList();
                break;
            case 20:
            case 27:
            case 40:
            case 41:
            case 42:
            case 43:
            case 44:
            case 45:
            case 46:
            case 47:
            case 48:
            case 49:
            case 50:
            case 51:
            case 52:
            case 53:
            case 70:
            case 71:
            case 72:
            case 73:
                obsActioN3DNnWorkArray = info.obj_3d_list_fall;
                break;
        }
        return obsActioN3DNnWorkArray;
    }

    private static AMS_AMB_HEADER gmDecoDataGetMotionHeader(int id)
    {
        AMS_AMB_HEADER amsAmbHeader = null;
        switch (id)
        {
            case 7:
            case 8:
            case 9:
            case 11:
            case 12:
            case 13:
                amsAmbHeader = (AMS_AMB_HEADER)ObjDataGet(834).pData;
                break;
            case 17:
            case 26:
            case 111:
            case 114:
            case 170:
            case 171:
            case 172:
            case 174:
            case 175:
            case 176:
            case 178:
            case 179:
            case 183:
                AMS_AMB_HEADER ambHeader = gmDecoDataGetAmbHeader();
                if (gmDeco_matMotionHeader == null)
                {
                    string sPath;
                    gmDeco_matMotionHeader = readAMBFile(amBindGet(ambHeader, 2, out sPath));
                    gmDeco_matMotionHeader.dir = sPath;
                }
                amsAmbHeader = gmDeco_matMotionHeader;
                break;
        }
        return amsAmbHeader;
    }

    private static AMS_AMB_HEADER gmDecoDataGetMatMotionHeader(int id)
    {
        AMS_AMB_HEADER amsAmbHeader = null;
        switch (id)
        {
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
                amsAmbHeader = (AMS_AMB_HEADER)ObjDataGet(835).pData;
                break;
            case 20:
            case 27:
            case 40:
            case 41:
            case 42:
            case 43:
            case 44:
            case 45:
            case 46:
            case 47:
            case 48:
            case 49:
            case 50:
            case 51:
            case 52:
            case 53:
            case 70:
            case 71:
            case 72:
            case 73:
            case 170:
            case 171:
            case 172:
            case 174:
            case 175:
            case 176:
            case 178:
            case 179:
            case 183:
                AMS_AMB_HEADER ambHeader = gmDecoDataGetAmbHeader();
                if (gmDeco_motionHeader == null)
                {
                    string sPath;
                    gmDeco_motionHeader = readAMBFile(amBindGet(ambHeader, 3, out sPath));
                    gmDeco_motionHeader.dir = sPath;
                }
                amsAmbHeader = gmDeco_motionHeader;
                break;
        }
        return amsAmbHeader;
    }

    private static void gmDecoCopySetRenderTargetForFront(AMS_RENDER_TARGET target)
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.flag_render_front = true;
        mgr.render_target_front = target;
    }

    private static void gmDecoCopySetRenderTargetForBack(AMS_RENDER_TARGET target)
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.flag_render_back = true;
        mgr.render_target_back = target;
    }

    private static AMS_RENDER_TARGET gmDecoGetRenderWorkFront()
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return null;
        return mgr.render_target_front == null ? null : mgr.render_target_front;
    }

    private static AMS_RENDER_TARGET gmDecoGetRenderWorkBack()
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return null;
        return mgr.render_target_back == null ? null : mgr.render_target_back;
    }

    private static void gmDecoInitMgr()
    {
        g_deco_mgr_real.Clear();
        g_deco_mgr = g_deco_mgr_real;
        gmDecoCreateTcbPost();
    }

    private static void gmDecoReleaseMgr()
    {
    }

    private static MTS_TASK_TCB gmDecoCreateTcbPost()
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        mgr.tcb_post = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmDecoTcbProcPost), null, 0U, 0, 5376U, 5, null, "GM DECO POST");
        return mgr.tcb_post;
    }

    private static void gmDecoTcbProcPost(MTS_TASK_TCB tcb)
    {
        ObjDraw3DNNUserFunc(_gmDecoTcbProcPostDT, null, 0, 8U);
    }

    private static void gmDecoTcbProcPostDT(object data)
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.flag_render_front = false;
        mgr.flag_render_back = false;
        mgr.render_target_front = null;
        mgr.render_target_back = null;
    }

    private static void gmDecoDraw(OBS_OBJECT_WORK obj_work)
    {
        uint dispFlag = obj_work.disp_flag;
        GMS_DECO_WORK gmsDecoWork = (GMS_DECO_WORK)obj_work;
        if (gmsDecoWork.model_tex != null)
        {
            if (!GmMainIsDrawEnable() || ((int)dispFlag & 32) != 0)
                return;
            if (((int)obj_work.user_flag & 2) == 0)
                gmDecoSetDrawPrimitive(gmsDecoWork.model_index, gmsDecoWork.model_tex, obj_work.pos, 0.0f, gmsDecoWork.obj_3d.command_state, dispFlag);
            if (((int)obj_work.user_flag & 4) == 0)
                return;
            gmDecoSetDrawPrimitive(((GMS_DECO_SUBMODEL_WORK)gmsDecoWork).sub_model_index, gmsDecoWork.model_tex, obj_work.pos, 0.0f, gmsDecoWork.obj_3d.command_state, dispFlag);
        }
        else
        {
            if (((int)obj_work.user_flag & 2) == 0)
            {
                OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
                if (obj3d != null)
                {
                    if (((int)obj_work.user_flag & 8) != 0 && obj3d != null && (obj3d.motion != null && obj3d.motion.mmobject != null))
                    {
                        float startFrame = amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
                        float divisor = amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - startFrame;
                        float syncTime = gmDecoGameSystemGetSyncTime();
                        obj3d.mat_frame = fmod(syncTime, divisor);
                        obj_work.disp_flag |= 16U;
                    }
                    if (obj3d.command_state != 0U)
                        ObjDraw3DNNSetCameraEx(0, 1, obj_work.obj_3d.command_state);
                    ObjDrawActionSummary(obj_work);
                    obj_work.disp_flag = dispFlag;
                }
            }
            if (((int)obj_work.user_flag & 4) == 0)
                return;
            if (ObjObjectPauseCheck(0U) != 0U)
                dispFlag |= 4096U;
            GMS_DECO_SUBMODEL_WORK decoSubmodelWork = (GMS_DECO_SUBMODEL_WORK)obj_work;
            decoSubmodelWork.obj_3d_sub.command_state = 17U;
            if (decoSubmodelWork.obj_3d_sub.command_state != 17U)
                ObjDraw3DNNSetCameraEx(0, 1, decoSubmodelWork.obj_3d_sub.command_state);
            ObjDrawAction3DNN(decoSubmodelWork.obj_3d_sub, new VecFx32?(obj_work.pos), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref dispFlag);
        }
    }

    private static float fmod(float dividend, float divisor)
    {
        return (Math.Abs(dividend) - Math.Abs(divisor) * (float)Math.Floor(Math.Abs(dividend) / (double)Math.Abs(divisor))) * Math.Sign(dividend);
    }

    private static void gmDecoDrawFinalShutter3Line(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.user_flag & 2) == 0)
            ObjDrawActionSummary(obj_work);
        if (((int)obj_work.user_flag & 4) == 0)
            return;
        uint dispFlag = obj_work.disp_flag;
        if (ObjObjectPauseCheck(0U) != 0U)
            dispFlag |= 4096U;
        VecFx32 vecFx32 = new VecFx32(obj_work.pos);
        vecFx32.z = -1703936;
        GMS_DECO_SUBMODEL_WORK decoSubmodelWork = (GMS_DECO_SUBMODEL_WORK)obj_work;
        if (decoSubmodelWork.obj_3d_sub.command_state != 15U)
            ObjDraw3DNNSetCameraEx(0, 1, decoSubmodelWork.obj_3d_sub.command_state);
        ObjDrawAction3DNN(decoSubmodelWork.obj_3d_sub, new VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref dispFlag);
    }

    private static void gmDecoDrawFinalShutter5Line(OBS_OBJECT_WORK obj_work)
    {
        GMS_DECO_WORK gmsDecoWork = (GMS_DECO_WORK)obj_work;
        uint disp_flag = obj_work.disp_flag | 4096U;
        if (!GmMainIsDrawEnable() || ((int)disp_flag & 32) != 0)
            return;
        if (((int)obj_work.user_flag & 2) == 0)
            gmDecoSetDrawPrimitive(gmsDecoWork.model_index, gmsDecoWork.model_tex, obj_work.pos, obj_work.user_timer, gmsDecoWork.obj_3d.command_state, disp_flag);
        if (((int)obj_work.user_flag & 4) == 0)
            return;
        gmDecoSetDrawPrimitive(((GMS_DECO_SUBMODEL_WORK)gmsDecoWork).sub_model_index, gmsDecoWork.model_tex, new VecFx32(obj_work.pos)
        {
            x = obj_work.pos.x,
            y = obj_work.pos.y,
            z = -2228224
        }, -obj_work.user_timer / 2, gmsDecoWork.obj_3d.command_state, disp_flag);
    }

    private static AMS_RENDER_TARGET gmDecoDrawFallCopyRenderFront()
    {
        AMS_RENDER_TARGET target;
        switch (g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id])
        {
            case 0:
                target = gmDecoGetRenderWorkFront();
                if (target == null)
                {
                    target = _gm_mapFar_render_work;
                    if (target.width == 0)
                        return null;
                    break;
                }
                break;
            case 2:
                AMS_RENDER_TARGET renderTarget = GmWaterSurfaceGetRenderTarget();
                if (renderTarget != null)
                {
                    gmDecoCopySetRenderTargetForFront(renderTarget);
                    return renderTarget;
                }
                target = _am_render_manager.targetp != _gm_mapFar_render_work ? _gm_mapFar_render_work : _am_draw_target;
                break;
            default:
                return null;
        }
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr != null && !mgr.flag_render_front)
        {
            NNS_RGBA_U8 rendaerTargetColor = g_deco_rendaer_target_color;
            amRenderCopyTarget(target, rendaerTargetColor);
            gmDecoCopySetRenderTargetForFront(target);
        }
        return target;
    }

    private static AMS_RENDER_TARGET gmDecoDrawFallCopyRenderBack()
    {
        int num = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        AMS_RENDER_TARGET target = gmDecoGetRenderWorkBack();
        switch (num)
        {
            case 0:
                if (target == null)
                {
                    target = _gm_mapFar_render_work;
                    if (target.width == 0)
                        return null;
                    break;
                }
                break;
            case 2:
                target = _am_render_manager.targetp != _gm_mapFar_render_work ? _gm_mapFar_render_work : _am_draw_target;
                break;
            default:
                return null;
        }
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr != null && !mgr.flag_render_back)
        {
            NNS_RGBA_U8 rendaerTargetColor = g_deco_rendaer_target_color;
            amRenderCopyTarget(target, rendaerTargetColor);
            gmDecoCopySetRenderTargetForBack(target);
        }
        return target;
    }

    private static void gmDecoDrawFallRender(
      AMS_RENDER_TARGET render_target,
      NNS_MATRIX proj_mtx)
    {
    }

    private static void gmDecoDrawFallFrontUserFunc(object data)
    {
        amDrawGetProjectionMatrix();
    }

    private static void gmDecoDrawFallBackUserFunc(object data)
    {
        NNS_MATRIX projectionMatrix = amDrawGetProjectionMatrix();
        AMS_RENDER_TARGET render_target = gmDecoDrawFallCopyRenderBack();
        if (render_target == null)
            return;
        gmDecoDrawFallRender(render_target, projectionMatrix);
    }

    private static void gmDecoDrawFallFront(OBS_OBJECT_WORK obj_work)
    {
        ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, 1, obj_work.obj_3d.command_state);
        ObjDraw3DNNUserFunc(_gmDecoDrawFallFrontUserFunc, null, 0, obj_work.obj_3d.command_state);
        gmDecoDraw(obj_work);
    }

    public static void gmDecoDrawFallBack(OBS_OBJECT_WORK obj_work)
    {
        ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, 1, obj_work.obj_3d.command_state);
        ObjDraw3DNNUserFunc(_gmDecoDrawFallBackUserFunc, null, 0, obj_work.obj_3d.command_state);
        gmDecoDraw(obj_work);
    }

    private static GMS_DECO_WORK gmDecoLoadObj(
      GMS_EVE_RECORD_DECORATE dec_rec,
      TaskWorkFactoryDelegate work_size,
      int pos_x,
      int pos_y,
      MPP_VOID_OBS_OBJECT_WORK main_func,
      MPP_VOID_OBS_OBJECT_WORK move_func,
      MPP_VOID_OBS_OBJECT_WORK out_func,
      GSF_TASK_PROCEDURE dest_func)
    {
        GMS_DECO_WORK gmsDecoWork = (GMS_DECO_WORK)OBM_OBJECT_TASK_DETAIL_INIT(5376, 5, 0, 0, work_size, "DECO OBJ");
        OBS_OBJECT_WORK objWork = gmsDecoWork.obj_work;
        mtTaskChangeTcbDestructor(objWork.tcb, dest_func);
        objWork.obj_type = 4;
        objWork.pos.x = pos_x + g_gm_deco_pos[dec_rec.id][0];
        objWork.pos.y = pos_y + g_gm_deco_pos[dec_rec.id][1];
        objWork.pos.z = g_gm_deco_pos[dec_rec.id][2];
        objWork.dir.z = g_gm_deco_rot_z[dec_rec.id];
        objWork.move_flag |= 8448U;
        objWork.ppFunc = main_func;
        objWork.ppMove = move_func;
        objWork.ppOut = out_func;
        objWork.view_out_ofst = (short)(g_gm_decorate_size_tbl[dec_rec.id] + 16 + 32 + 16);
        objWork.ppViewCheck = new OBS_OBJECT_WORK_Delegate3(ObjObjectViewOutCheck);
        gmsDecoWork.event_record = dec_rec;
        gmsDecoWork.event_x = dec_rec.pos_x;
        dec_rec.pos_x = byte.MaxValue;
        return gmsDecoWork;
    }

    private static GMS_DECO_WORK gmDecoLoadModel(
      GMS_DECO_WORK deco_work,
      OBS_ACTION3D_NN_WORK obj_3d_work)
    {
        ObjObjectCopyAction3dNNModel(deco_work.obj_work, obj_3d_work, deco_work.obj_3d);
        deco_work.obj_3d.drawflag |= 32U;
        return deco_work;
    }

    private static void gmDecoLoadMotion(
      GMS_DECO_WORK deco_work,
      int motion_index,
      AMS_AMB_HEADER motion_amb)
    {
        ObjObjectAction3dNNMotionLoad(deco_work.obj_work, 0, false, null, null, motion_index, motion_amb);
    }

    private static void gmDecoLoadMatMotion(
      GMS_DECO_WORK deco_work,
      int motion_index,
      AMS_AMB_HEADER motion_amb)
    {
        ObjObjectAction3dNNMaterialMotionLoad(deco_work.obj_work, 0, null, null, motion_index, motion_amb);
    }

    private static void gmDecoSetRect(
      GMS_DECO_WORK deco_work,
      short left,
      short top,
      short back,
      short right,
      short bottom,
      short front,
      OBS_RECT_WORK_Delegate1 func)
    {
        OBS_OBJECT_WORK objWork = deco_work.obj_work;
        ArrayPointer<OBS_RECT_WORK> rect_work = new ArrayPointer<OBS_RECT_WORK>(deco_work.rect_work, 0);
        ObjObjectGetRectBuf(objWork, rect_work, 1);
        ObjRectGroupSet(rect_work, 1, 1);
        ObjRectAtkSet(rect_work, 0, 0);
        ObjRectDefSet(rect_work, 65534, 0);
        (~rect_work).parent_obj = objWork;
        (~rect_work).ppDef = func;
        (~rect_work).ppHit = null;
        (~rect_work).flag |= 1152U;
        ObjRectWorkZSet(rect_work, left, top, back, right, bottom, front);
    }

    private static void gmDecoMainFuncMotionCount(OBS_OBJECT_WORK obj_work)
    {
        --obj_work.user_timer;
        if (0 < obj_work.user_timer)
            return;
        obj_work.obj_3d.frame[0] = 0.0f;
        obj_work.user_timer = 0;
        obj_work.disp_flag |= 16U;
        obj_work.ppFunc = null;
    }

    private static void gmDecoMainFuncDecreaseMotionSpeed(OBS_OBJECT_WORK obj_work)
    {
        --obj_work.user_timer;
        if (obj_work.user_timer > 0)
        {
            float userTimer = obj_work.user_timer;
            float num = obj_work.obj_3d.speed[0] / userTimer;
            obj_work.obj_3d.speed[0] -= num;
            obj_work.obj_3d.frame[0] += obj_work.obj_3d.speed[0];
            float startFrame = amMotionGetStartFrame(obj_work.obj_3d.motion, obj_work.obj_3d.act_id[0]);
            float endFrame = amMotionGetEndFrame(obj_work.obj_3d.motion, obj_work.obj_3d.act_id[0]);
            if (obj_work.obj_3d.frame[0] < (double)startFrame)
            {
                obj_work.obj_3d.frame[0] = endFrame;
            }
            else
            {
                if (obj_work.obj_3d.frame[0] <= (double)endFrame)
                    return;
                obj_work.obj_3d.frame[0] = startFrame;
            }
        }
        else
        {
            obj_work.user_timer = 0;
            obj_work.obj_3d.speed[0] = 0.0f;
            obj_work.ppFunc = null;
        }
    }

    private static void gmDecoMainFuncMotionApplyCommonFrame(OBS_OBJECT_WORK obj_work)
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        OBS_ACTION3D_NN_WORK[] obsActioN3DNnWorkArray = new OBS_ACTION3D_NN_WORK[2]
        {
      obj_work.obj_3d,
      null
        };
        if (((int)obj_work.user_flag & 4) != 0)
        {
            GMS_DECO_SUBMODEL_WORK decoSubmodelWork = (GMS_DECO_SUBMODEL_WORK)obj_work;
            obsActioN3DNnWorkArray[1] = decoSubmodelWork.obj_3d_sub;
        }
        int int32 = Convert.ToInt32(obj_work.user_work);
        int index1 = 0;
        if (((int)obj_work.user_flag & 32) != 0)
            index1 = 1;
        for (int index2 = 0; 2 > index2; ++index2)
        {
            OBS_ACTION3D_NN_WORK obsActioN3DNnWork = obsActioN3DNnWorkArray[index2];
            if (obsActioN3DNnWork != null && obsActioN3DNnWork.motion != null)
            {
                if (obsActioN3DNnWork.motion.mtnbuf[0] != null)
                {
                    obsActioN3DNnWork.frame[0] = mgr.common_frame_motion[index1] + int32;
                    float startFrame = amMotionGetStartFrame(obsActioN3DNnWork.motion, obsActioN3DNnWork.act_id[0]);
                    float endFrame = amMotionGetEndFrame(obsActioN3DNnWork.motion, obsActioN3DNnWork.act_id[0]);
                    if (obsActioN3DNnWork.frame[0] < (double)startFrame)
                        obsActioN3DNnWork.frame[0] = startFrame;
                    else if (obsActioN3DNnWork.frame[0] >= (double)endFrame)
                        obsActioN3DNnWork.frame[0] = endFrame - 1f;
                }
                if (obsActioN3DNnWork.motion.mmobject != null)
                {
                    obsActioN3DNnWork.mat_frame = mgr.common_frame_motion[index1] + int32;
                    float startFrame = amMotionMaterialGetStartFrame(obsActioN3DNnWork.motion, obsActioN3DNnWork.mat_act_id);
                    float endFrame = amMotionMaterialGetEndFrame(obsActioN3DNnWork.motion, obsActioN3DNnWork.mat_act_id);
                    if (obsActioN3DNnWork.mat_frame < (double)startFrame)
                        obsActioN3DNnWork.mat_frame = startFrame;
                    else if (obsActioN3DNnWork.mat_frame >= (double)endFrame)
                        obsActioN3DNnWork.mat_frame = endFrame - 1f;
                }
            }
        }
    }

    private static void gmDecoMainFuncMotionCheckCommonFrame(OBS_OBJECT_WORK obj_work)
    {
        if (((int)g_gm_main_system.game_flag & 4096) != 0)
            return;
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        if (obj_work.user_timer < 254)
        {
            ++obj_work.user_timer;
            obj_work.disp_flag &= 4294967279U;
        }
        int index = 0;
        if (((int)obj_work.user_flag & 32) != 0)
            index = 1;
        if (mgr.common_frame_motion[index] == 0)
        {
            obj_work.flag &= 4294967279U;
            obj_work.user_timer = 0;
        }
        else
            obj_work.flag |= 16U;
    }

    private static void gmDecoMainFuncLoop(OBS_OBJECT_WORK obj_work)
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        int index = 0;
        if (((int)obj_work.user_flag & 32) != 0)
            index = 1;
        if (mgr.state_loop == 0)
            obj_work.flag |= 16U;
        else if (mgr.state_loop == 1)
        {
            gmDecoMainFuncMotionCheckCommonFrame(obj_work);
            obj_work.flag &= 4294967279U;
        }
        else
            gmDecoMainFuncMotionCheckCommonFrame(obj_work);
        if (obj_work.user_timer != 0 && mgr.common_frame_motion[index] != 0 && mgr.common_frame_motion[index] + 1 >= obj_work.user_timer)
            mgr.common_frame_motion[index] = obj_work.user_timer;
        int id = ((GMS_DECO_WORK)obj_work).event_record.id;
        if (mgr.se_handle != null)
        {
            if (mgr.common_frame_motion[index] > Convert.ToInt32(obj_work.user_work))
            {
                GmSoundStopSE(mgr.se_handle);
                GsSoundFreeSeHandle(mgr.se_handle);
                mgr.se_handle = null;
            }
        }
        else if (mgr.state_loop == 1 && mgr.common_frame_motion[index] < Convert.ToInt32(obj_work.user_work) && id != 0)
        {
            mgr.se_handle = GsSoundAllocSeHandle();
            if (id == 183)
                GmSoundPlaySE("Shutter1", mgr.se_handle);
        }
        if (obj_work.user_timer != Convert.ToInt32(obj_work.user_work) || id == 0 || id != 183)
            return;
        GmSoundPlaySE("Shutter2");
    }

    private static void gmDecoMainFuncEffectCheckCommonFrame(OBS_OBJECT_WORK obj_work)
    {
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr == null)
            return;
        int index = 0;
        if (((int)obj_work.user_flag & 32) != 0)
            index = 1;
        else if (((int)obj_work.user_flag & 64) != 0)
            index = 2;
        if (mgr.common_frame_motion[index] == 0)
            obj_work.disp_flag |= 4128U;
        else
            obj_work.disp_flag &= 4294963167U;
    }

    private static void gmDecoTcbDest(MTS_TASK_TCB tcb)
    {
        GMS_DECO_WORK tcbWork = (GMS_DECO_WORK)mtTaskGetTcbWork(tcb);
        gmDecoDelFallEvent(tcbWork.event_record, tcbWork.obj_work.pos.x + tcbWork.obj_work.ofst.x);
        if (((int)tcbWork.obj_work.user_flag & 4) != 0 && tcbWork.model_tex == null)
            ObjAction3dNNMotionRelease(((GMS_DECO_SUBMODEL_WORK)tcbWork).obj_3d_sub);
        GMS_EVE_RECORD_DECORATE eventRecord = tcbWork.event_record;
        if (eventRecord != null)
        {
            if (eventRecord.pos_x == byte.MaxValue)
            {
                eventRecord.pos_x = tcbWork.event_x;
                tcbWork.event_x = 0;
            }
            tcbWork.event_record = null;
        }
        ObjObjectExit(tcb);
    }

    private static void gmDecoRectFuncChangeMotionCount(
      OBS_RECT_WORK own_rect_work,
      OBS_RECT_WORK target_rect_work)
    {
        OBS_OBJECT_WORK parentObj = own_rect_work.parent_obj;
        uint id = ((GMS_DECO_WORK)parentObj).event_record.id;
        parentObj.user_timer = g_gm_deco_user_timer[(int)id];
        float startFrame = amMotionGetStartFrame(parentObj.obj_3d.motion, parentObj.obj_3d.act_id[0]);
        float num = (amMotionGetEndFrame(parentObj.obj_3d.motion, parentObj.obj_3d.act_id[0]) - startFrame) * Convert.ToInt32(parentObj.user_work);
        parentObj.obj_3d.speed[0] = num / parentObj.user_timer;
        parentObj.disp_flag &= 4294967279U;
        parentObj.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmDecoMainFuncMotionCount);
    }

    private static void gmDecoRectFuncChangeDecreaseMotionSpeed(
      OBS_RECT_WORK own_rect_work,
      OBS_RECT_WORK target_rect_work)
    {
        OBS_OBJECT_WORK parentObj1 = own_rect_work.parent_obj;
        OBS_OBJECT_WORK parentObj2 = target_rect_work.parent_obj;
        float num = (float)((((int)parentObj2.move_flag & 32768) == 0 ? FX_FX32_TO_F32(parentObj2.spd_m) : FX_FX32_TO_F32(parentObj2.spd.x)) * (double)Convert.ToInt32(parentObj1.user_work) / 10.0);
        parentObj1.obj_3d.speed[0] = num;
        if (((int)parentObj1.user_flag & 16) == 0)
            parentObj1.obj_3d.speed[0] = MTM_MATH_ABS(parentObj1.obj_3d.speed[0]);
        parentObj1.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmDecoMainFuncDecreaseMotionSpeed);
        uint id = ((GMS_DECO_WORK)parentObj1).event_record.id;
        parentObj1.user_timer = g_gm_deco_user_timer[(int)id];
    }

    private static void gmDecoRectFuncChangeMotionCommonFrame(
      OBS_RECT_WORK own_rect_work,
      OBS_RECT_WORK target_rect_work)
    {
        OBS_OBJECT_WORK parentObj = own_rect_work.parent_obj;
        parentObj.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmDecoMainFuncMotionCheckCommonFrame);
        parentObj.flag |= 2U;
    }

    private static void gmDecoSetLightSpecial(OBS_OBJECT_WORK obj_work, int id)
    {
    }

    private static void gmDecoAdjustIPhone(OBS_OBJECT_WORK obj_work, int id)
    {
        if (obj_work == null || id != 71)
            return;
        if (g_gs_main_sys_info.stage_id != 8)
            return;
        float num1;
        float num2;
        if (obj_work.pos.x > FX_F32_TO_FX32(1024f) && obj_work.pos.x < FX_F32_TO_FX32(1280f))
        {
            num1 = 576f;
            num2 = 54.4f;
        }
        else if (obj_work.pos.x > FX_F32_TO_FX32(3008f) && obj_work.pos.x < FX_F32_TO_FX32(3072f))
        {
            num1 = 768f;
            num2 = 40f;
        }
        else
        {
            if (obj_work.pos.x <= FX_F32_TO_FX32(6848f) || obj_work.pos.x >= FX_F32_TO_FX32(6976f) || obj_work.pos.y <= FX_F32_TO_FX32(1213f))
                return;
            num1 = 1213f;
            num2 = 61f;
        }
        obj_work.scale.y = FX_F32_TO_FX32(num2 / 64f);
        float num3 = (FX_FX32_TO_F32(obj_work.pos.y) - num1) / 64f;
        obj_work.pos.y = FX_F32_TO_FX32(num1 + num3 * num2);
    }

    private static GMS_DECO_WORK gmDecoInitNomodel(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type,
      TaskWorkFactoryDelegate work_size)
    {
        int id = dec_rec.id;
        if (((int)g_gm_deco_user_flag[id] & 1) != 0 && ((int)g_gs_main_sys_info.game_flag & 32) == 0)
        {
            dec_rec.pos_x = byte.MaxValue;
            return null;
        }
        GMS_DECO_WORK gmsDecoWork = gmDecoLoadObj(dec_rec, work_size, x, y, g_gm_deco_func_main[id], g_gm_deco_func_move[id], g_gm_deco_func_out[id], g_gm_deco_func_dest[id]);
        gmsDecoWork.obj_work.disp_flag |= g_gm_deco_disp_flag[id];
        gmsDecoWork.obj_work.user_work = g_gm_deco_user_work[id];
        gmsDecoWork.obj_work.user_timer = g_gm_deco_user_timer[id];
        gmsDecoWork.obj_work.user_flag = g_gm_deco_user_flag[id];
        gmsDecoWork.obj_work.flag |= 2U;
        if (((int)gmsDecoWork.obj_work.disp_flag & 1) != 0)
        {
            gmsDecoWork.obj_work.scale.x = -4096;
            gmsDecoWork.obj_work.disp_flag &= 4294967294U;
        }
        if (((int)gmsDecoWork.obj_work.disp_flag & 2) != 0)
        {
            gmsDecoWork.obj_work.scale.y = -4096;
            gmsDecoWork.obj_work.disp_flag &= 4294967293U;
        }
        if (((int)gmsDecoWork.obj_work.user_flag & 512) != 0)
            gmsDecoWork.obj_work.flag |= 16U;
        return gmsDecoWork;
    }

    private static GMS_DECO_WORK gmDecoInitModel(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        int id = dec_rec.id;
        int index1 = -1;
        TaskWorkFactoryDelegate work_size;
        if (((int)g_gm_deco_user_flag[id] & 4) != 0)
        {
            work_size = () => new GMS_DECO_SUBMODEL_WORK();
            index1 = g_gm_deco_model_index[id][1];
        }
        else
            work_size = () => new GMS_DECO_WORK();
        GMS_DECO_WORK deco_work = gmDecoInitNomodel(dec_rec, x, y, type, work_size);
        if (deco_work == null)
            return null;
        OBS_ACTION3D_NN_WORK[] obj3Dlist = gmDecoDataGetObj3DList(id);
        if (obj3Dlist == null)
        {
            deco_work.model_tex = gmDecoDataGetInfo().tvx_tex;
            int num = g_gm_deco_model_index[id][0];
            deco_work.obj_3d.command_state = g_gm_deco_command_state[id];
            deco_work.model_index = num;
            if (index1 != -1)
            {
                GMS_DECO_SUBMODEL_WORK decoSubmodelWork = (GMS_DECO_SUBMODEL_WORK)deco_work;
                decoSubmodelWork.obj_3d_sub.command_state = g_gm_deco_command_state[id];
                decoSubmodelWork.sub_model_index = index1;
            }
            deco_work.obj_work.disp_flag |= g_gm_deco_disp_flag[id];
        }
        else
        {
            int index2 = g_gm_deco_model_index[id][0];
            if (index2 != -1)
            {
                gmDecoLoadModel(deco_work, obj3Dlist[index2]);
                deco_work.obj_3d.command_state = g_gm_deco_command_state[id];
            }
            if (index1 != -1)
            {
                GMS_DECO_SUBMODEL_WORK decoSubmodelWork = (GMS_DECO_SUBMODEL_WORK)deco_work;
                ObjCopyAction3dNNModel(obj3Dlist[index1], decoSubmodelWork.obj_3d_sub);
                decoSubmodelWork.obj_3d_sub.drawflag |= 32U;
                if (((int)g_gm_deco_user_flag[id] & 128) != 0)
                {
                    decoSubmodelWork.obj_3d_sub.use_light_flag &= 4294967294U;
                    decoSubmodelWork.obj_3d_sub.use_light_flag |= 4U;
                }
                decoSubmodelWork.obj_3d_sub.command_state = g_gm_deco_command_state[id];
            }
            deco_work.obj_work.disp_flag |= g_gm_deco_disp_flag[id];
            if (((int)deco_work.obj_work.disp_flag & 1) != 0)
            {
                deco_work.obj_work.scale.x = -4096;
                deco_work.obj_work.disp_flag &= 4294967294U;
            }
            if (((int)deco_work.obj_work.disp_flag & 2) != 0)
            {
                deco_work.obj_work.scale.y = -4096;
                deco_work.obj_work.disp_flag &= 4294967293U;
            }
        }
        gmDecoSetLightSpecial(deco_work.obj_work, id);
        return deco_work;
    }

    private static void gmDecoInitMotion(GMS_DECO_WORK deco_work)
    {
        int id = deco_work.event_record.id;
        AMS_AMB_HEADER motionHeader = gmDecoDataGetMotionHeader(id);
        int motion_index = g_gm_deco_node_motion_index[id][0];
        if (motion_index != -1)
        {
            gmDecoLoadMotion(deco_work, motion_index, motionHeader);
            OBS_OBJECT_WORK objWork = deco_work.obj_work;
            uint dispFlag = objWork.disp_flag;
            ObjDrawObjectActionSet(objWork, 0);
            objWork.disp_flag = dispFlag;
        }
        if (((int)g_gm_deco_user_flag[id] & 4) == 0)
            return;
        int index = g_gm_deco_node_motion_index[id][1];
        if (index == -1)
            return;
        ObjAction3dNNMotionLoad(((GMS_DECO_SUBMODEL_WORK)deco_work).obj_3d_sub, 0, false, null, null, index, motionHeader);
    }

    private static GMS_DECO_WORK gmDecoInitModelMotion(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        GMS_DECO_WORK deco_work = gmDecoInitModel(dec_rec, x, y, type);
        if (deco_work == null)
            return null;
        gmDecoInitMotion(deco_work);
        return deco_work;
    }

    private static GMS_DECO_WORK gmDecoInitModelMotionTouch(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        GMS_DECO_WORK deco_work = gmDecoInitModelMotion(dec_rec, x, y, type);
        if (deco_work == null)
            return null;
        ((OBS_OBJECT_WORK)deco_work).flag &= 4294967293U;
        int id = dec_rec.id;
        short num1 = g_gm_deco_rect_size[id][0];
        short num2 = g_gm_deco_rect_size[id][1];
        short left = (short)(-num1 / 2);
        short top = (short)(-num2 / 2);
        short right = (short)(num1 / 2);
        short bottom = (short)(num2 / 2);
        gmDecoSetRect(deco_work, left, top, -500, right, bottom, 500, g_gm_deco_func_rect[id]);
        return deco_work;
    }

    private static void gmDecoInitMaterial(GMS_DECO_WORK deco_work)
    {
        int id = deco_work.event_record.id;
        AMS_AMB_HEADER matMotionHeader = gmDecoDataGetMatMotionHeader(id);
        int motion_index = g_gm_deco_mat_motion_index[id][0];
        if (motion_index != -1)
        {
            gmDecoLoadMatMotion(deco_work, motion_index, matMotionHeader);
            OBS_OBJECT_WORK objWork = deco_work.obj_work;
            uint dispFlag = objWork.disp_flag;
            ObjDrawObjectActionSet3DNNMaterial(objWork, 0);
            objWork.disp_flag = dispFlag;
        }
        if (((int)g_gm_deco_user_flag[id] & 4) == 0)
            return;
        int index = g_gm_deco_mat_motion_index[id][1];
        if (index == -1)
            return;
        ObjAction3dNNMaterialMotionLoad(((GMS_DECO_SUBMODEL_WORK)deco_work).obj_3d_sub, 0, null, null, index, matMotionHeader);
    }

    private static GMS_DECO_WORK gmDecoInitModelMaterial(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        GMS_DECO_WORK deco_work = gmDecoInitModel(dec_rec, x, y, type);
        if (deco_work == null)
            return null;
        gmDecoInitMaterial(deco_work);
        return deco_work;
    }

    private static GMS_DECO_WORK gmDecoInitModelMotioinMaterial(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        GMS_DECO_WORK deco_work = gmDecoInitModel(dec_rec, x, y, type);
        if (deco_work == null)
            return null;
        gmDecoInitMotion(deco_work);
        gmDecoInitMaterial(deco_work);
        return deco_work;
    }

    private static GMS_DECO_WORK gmDecoInitModelMotionMaterialTouch(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        GMS_DECO_WORK deco_work = gmDecoInitModelMotioinMaterial(dec_rec, x, y, type);
        if (deco_work == null)
            return null;
        ((OBS_OBJECT_WORK)deco_work).flag &= 4294967293U;
        int id = dec_rec.id;
        short num1 = g_gm_deco_rect_size[id][0];
        short num2 = g_gm_deco_rect_size[id][1];
        short left = (short)(-num1 / 2);
        short top = (short)(-num2 / 2);
        short right = (short)(num1 / 2);
        short bottom = (short)(num2 / 2);
        gmDecoSetRect(deco_work, left, top, -500, right, bottom, 500, g_gm_deco_func_rect[id]);
        return deco_work;
    }

    private static GMS_DECO_WORK gmDecoInitModelLoop(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        GMS_DECO_WORK gmsDecoWork = gmDecoInitModel(dec_rec, x, y, type);
        if (gmsDecoWork == null)
            return null;
        GMS_DECO_MGR mgr = gmDecoGetMgr();
        if (mgr != null)
        {
            if (mgr.state_loop == 0)
                gmDecoMainFuncLoop(gmsDecoWork.obj_work);
            else if (mgr.state_loop == 1)
            {
                for (int index = 0; 12 > index; ++index)
                {
                    if (mgr.motion_frame_loop[index] != 0)
                    {
                        gmsDecoWork.obj_work.user_timer = mgr.motion_frame_loop[index];
                        mgr.motion_frame_loop[index] = 0;
                        gmDecoMainFuncLoop(gmsDecoWork.obj_work);
                        break;
                    }
                }
            }
            else
            {
                GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
                if (gmsDecoWork.obj_work.pos.x < gmsPlayerWork.obj_work.pos.x)
                {
                    gmsDecoWork.obj_work.user_timer = 254;
                }
                else
                {
                    int index = 0;
                    if (((int)gmsDecoWork.obj_work.user_flag & 32) != 0)
                        index = 1;
                    if (16 < mgr.common_frame_motion[index])
                        gmsDecoWork.obj_work.user_timer = mgr.common_frame_motion[index] - 16;
                }
                gmDecoMainFuncLoop(gmsDecoWork.obj_work);
            }
        }
        return gmsDecoWork;
    }

    private static GMS_DECO_WORK gmDecoInitModelEffect(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        GMS_DECO_WORK gmsDecoWork = gmDecoInitModel(dec_rec, x, y, type);
        if (gmsDecoWork == null)
            return null;
        int id = dec_rec.id;
        int efct_zone_idx = g_gm_deco_model_index[id][1];
        int zone_no = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctZoneEsCreate(gmsDecoWork.obj_work, zone_no, efct_zone_idx);
        gmsEffect3DesWork.efct_com.obj_work.dir.z = g_gm_deco_rot_z[dec_rec.id];
        int num1 = (int)g_gm_deco_user_work[id] << 16 >> 16;
        int num2 = (int)g_gm_deco_user_work[id] >> 16;
        gmsEffect3DesWork.efct_com.obj_work.pos.x = gmsDecoWork.obj_work.pos.x + num1 * 4096;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = gmsDecoWork.obj_work.pos.y + num2 * 4096;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = gmsDecoWork.obj_work.pos.z + 131072;
        if (gmsDecoWork.obj_3d.command_state == 17U)
            gmsEffect3DesWork.obj_3des.command_state = 17U;
        return gmsDecoWork;
    }

    private static GMS_DECO_WORK gmDecoInitPrimitive3D(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        int id = dec_rec.id;
        GMS_DECO_WORK gmsDecoWork = gmDecoInitNomodel(dec_rec, x, y, type, () => new GMS_DECO_WORK());
        if (gmsDecoWork == null)
            return null;
        gmsDecoWork.obj_work.disp_flag |= g_gm_deco_disp_flag[id];
        return gmsDecoWork;
    }

    private static GMS_DECO_WORK gmDecoInitFall(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        GMS_DECO_WORK gmsDecoWork = gmDecoInitModelMaterial(dec_rec, x, y, type);
        if (gmsDecoWork == null)
            return null;
        int id = gmsDecoWork.event_record.id;
        gmDecoAdjustIPhone((OBS_OBJECT_WORK)gmsDecoWork, id);
        return gmsDecoWork;
    }

    private static GMS_DECO_WORK gmDecoInitEffect(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        int id = dec_rec.id;
        GMS_DECO_WORK gmsDecoWork = gmDecoInitNomodel(dec_rec, x, y, type, () => new GMS_DECO_WORK());
        if (gmsDecoWork == null)
            return null;
        int efct_zone_idx = g_gm_deco_model_index[id][0];
        int zone_no = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctZoneEsCreate(gmsDecoWork.obj_work, zone_no, efct_zone_idx);
        gmsEffect3DesWork.efct_com.obj_work.dir.z = g_gm_deco_rot_z[dec_rec.id];
        gmsEffect3DesWork.efct_com.obj_work.disp_flag |= g_gm_deco_disp_flag[id];
        int num1 = (int)gmsEffect3DesWork.efct_com.obj_work.user_flag | (int)g_gm_deco_user_flag[id];
        gmsEffect3DesWork.efct_com.obj_work.user_flag = (uint)num1;
        int num2 = (int)gmsEffect3DesWork.efct_com.obj_work.user_work | (int)g_gm_deco_user_work[id];
        gmsEffect3DesWork.efct_com.obj_work.user_work = (uint)num2;
        gmsEffect3DesWork.obj_3des.command_state = g_gm_deco_command_state[id];
        if (gmsDecoWork.obj_work.ppFunc != null)
        {
            gmsEffect3DesWork.efct_com.obj_work.ppFunc = gmsDecoWork.obj_work.ppFunc;
            gmsDecoWork.obj_work.ppFunc = null;
        }
        return gmsDecoWork;
    }

    private static GMS_DECO_WORK gmDecoInitEffectBlock(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        int id = dec_rec.id;
        int x1 = x >> 18 << 18;
        int y1 = y >> 18 << 18;
        GMS_DECO_WORK gmsDecoWork = gmDecoInitNomodel(dec_rec, x1, y1, type, () => new GMS_DECO_WORK());
        if (gmsDecoWork == null)
            return null;
        int efct_zone_idx = g_gm_deco_model_index[id][0];
        int zone_no = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctZoneEsCreate(gmsDecoWork.obj_work, zone_no, efct_zone_idx);
        efct_3des.efct_com.obj_work.dir.z = g_gm_deco_rot_z[dec_rec.id];
        efct_3des.efct_com.obj_work.disp_flag |= g_gm_deco_disp_flag[id];
        efct_3des.efct_com.obj_work.user_flag |= g_gm_deco_user_flag[id];
        efct_3des.efct_com.obj_work.user_work |= g_gm_deco_user_work[id];
        efct_3des.obj_3des.command_state = g_gm_deco_command_state[id];
        efct_3des.efct_com.obj_work.user_flag |= gmsDecoWork.obj_work.user_flag;
        if (gmsDecoWork.obj_work.ppFunc != null)
        {
            efct_3des.efct_com.obj_work.ppFunc = gmsDecoWork.obj_work.ppFunc;
            gmsDecoWork.obj_work.ppFunc = null;
        }
        if (((int)g_gm_deco_user_flag[id] & 256) != 0)
        {
            int num1 = (int)g_gm_deco_user_work[id] << 16 >> 16;
            int num2 = (int)g_gm_deco_user_work[id] >> 16;
            GmEffect3DESSetDuplicateDraw(efct_3des, num1, num2, 0.0f);
        }
        return gmsDecoWork;
    }

    private static GMS_DECO_WORK gmDecoInitEffectBlockAndNext(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        int id = dec_rec.id;
        int x1 = x >> 18 << 18;
        int y1 = y >> 18 << 18;
        GMS_DECO_WORK gmsDecoWork = gmDecoInitNomodel(dec_rec, x1, y1, type, () => new GMS_DECO_WORK());
        if (gmsDecoWork == null)
            return null;
        int efct_zone_idx = g_gm_deco_model_index[id][0];
        int zone_no = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork1 = GmEfctZoneEsCreate(gmsDecoWork.obj_work, zone_no, efct_zone_idx);
        gmsEffect3DesWork1.efct_com.obj_work.dir.z = g_gm_deco_rot_z[dec_rec.id];
        gmsEffect3DesWork1.efct_com.obj_work.disp_flag |= g_gm_deco_disp_flag[id];
        gmsEffect3DesWork1.efct_com.obj_work.user_flag |= g_gm_deco_user_flag[id];
        gmsEffect3DesWork1.efct_com.obj_work.user_work |= g_gm_deco_user_work[id];
        gmsEffect3DesWork1.obj_3des.command_state = g_gm_deco_command_state[id];
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork2 = GmEfctZoneEsCreate(gmsDecoWork.obj_work, zone_no, efct_zone_idx + 1);
        gmsEffect3DesWork2.efct_com.obj_work.dir.z = g_gm_deco_rot_z[dec_rec.id];
        gmsEffect3DesWork2.efct_com.obj_work.disp_flag |= g_gm_deco_disp_flag[id];
        gmsEffect3DesWork2.efct_com.obj_work.user_flag |= g_gm_deco_user_flag[id];
        gmsEffect3DesWork2.efct_com.obj_work.user_work |= g_gm_deco_user_work[id];
        gmsEffect3DesWork2.obj_3des.command_state = g_gm_deco_command_state[id];
        if (gmsDecoWork.obj_work.ppFunc != null)
        {
            gmsEffect3DesWork1.efct_com.obj_work.ppFunc = gmsDecoWork.obj_work.ppFunc;
            gmsEffect3DesWork2.efct_com.obj_work.ppFunc = gmsDecoWork.obj_work.ppFunc;
            gmsDecoWork.obj_work.ppFunc = null;
        }
        return gmsDecoWork;
    }

    private static void gmDecoAddFallEvent(
      GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      NNS_TEXLIST texlist,
      float frame)
    {
        if (dec_rec == null)
            return;
        ushort id = dec_rec.id;
        GMS_DECO_FALL_MANAGER gmsDecoFallManager1 = null;
        GMS_DECO_FALL_REGISTER decoFallRegister1 = null;
        switch (id)
        {
            case 20:
            case 27:
            case 40:
            case 41:
            case 42:
            case 47:
            case 48:
            case 49:
                for (int index = 0; index < 16; ++index)
                {
                    GMS_DECO_FALL_MANAGER gmsDecoFallManager2 = g_deco_fall_manager[index];
                    if (gmsDecoFallManager2.dec_id == 0U)
                    {
                        if (gmsDecoFallManager1 == null)
                            gmsDecoFallManager1 = gmsDecoFallManager2;
                    }
                    else if ((int)gmsDecoFallManager2.dec_id == id)
                    {
                        gmsDecoFallManager1 = gmsDecoFallManager2;
                        break;
                    }
                }
                gmsDecoFallManager1.dec_id = id;
                gmsDecoFallManager1.texlist = texlist;
                gmsDecoFallManager1.frame = frame;
                ++gmsDecoFallManager1.all_num;
                for (int index = 0; index < 8; ++index)
                {
                    GMS_DECO_FALL_REGISTER decoFallRegister2 = gmsDecoFallManager1.reg[index];
                    if (decoFallRegister2.num == 0U)
                    {
                        if (decoFallRegister1 == null)
                            decoFallRegister1 = decoFallRegister2;
                    }
                    else if (decoFallRegister2.vec.x == x)
                    {
                        decoFallRegister1 = decoFallRegister2;
                        break;
                    }
                }
                if (decoFallRegister1.num == 0U)
                    ++gmsDecoFallManager1.reg_num;
                ++decoFallRegister1.num;
                decoFallRegister1.vec.x = x;
                break;
        }
    }

    private static void gmDecoDelFallEvent(GMS_EVE_RECORD_DECORATE dec_rec, int x)
    {
        if (dec_rec == null)
            return;
        ushort id = dec_rec.id;
        switch (id)
        {
            case 20:
            case 27:
            case 40:
            case 41:
            case 42:
            case 47:
            case 48:
            case 49:
                for (int index1 = 0; index1 < 16; ++index1)
                {
                    GMS_DECO_FALL_MANAGER gmsDecoFallManager = g_deco_fall_manager[index1];
                    if ((int)gmsDecoFallManager.dec_id == id)
                    {
                        --gmsDecoFallManager.all_num;
                        if (gmsDecoFallManager.all_num == 0)
                        {
                            gmsDecoFallManager.dec_id = 0U;
                            gmsDecoFallManager.texlist = null;
                        }
                        for (int index2 = 0; index2 < 8; ++index2)
                        {
                            GMS_DECO_FALL_REGISTER decoFallRegister = gmsDecoFallManager.reg[index2];
                            if (decoFallRegister.vec.x == x)
                            {
                                --decoFallRegister.num;
                                if (decoFallRegister.num != 0U)
                                    break;
                                decoFallRegister.vec.x = 0;
                                --gmsDecoFallManager.reg_num;
                                break;
                            }
                        }
                        break;
                    }
                }
                break;
        }
    }

    private static void gmDecoSetDrawFall(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable())
            return;
        GMS_DECO_WORK gmsDecoWork = (GMS_DECO_WORK)obj_work;
        VecFx32 vecFx32 = new VecFx32();
        vecFx32.x = obj_work.pos.x + obj_work.ofst.x;
        vecFx32.y = -(obj_work.pos.y + obj_work.ofst.y);
        vecFx32.z = obj_work.pos.z + obj_work.ofst.z;
        GMS_EVE_RECORD_DECORATE eventRecord = gmsDecoWork.event_record;
        GMS_DECO_FALL_REGISTER decoFallRegister = null;
        for (int index1 = 0; index1 < 16; ++index1)
        {
            if ((int)g_deco_fall_manager[index1].dec_id == eventRecord.id)
            {
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    if (g_deco_fall_manager[index1].reg[index2].vec.x == vecFx32.x)
                    {
                        decoFallRegister = g_deco_fall_manager[index1].reg[index2];
                        break;
                    }
                }
            }
        }
        if (decoFallRegister == null)
            return;
        decoFallRegister.vec.z = vecFx32.z;
        if (decoFallRegister.vec.y != 0 && decoFallRegister.vec.y >= vecFx32.y)
            return;
        decoFallRegister.vec.y = vecFx32.y;
    }

    private static void gmDecoSetDrawPrimitive(
      int model_index,
      AOS_TEXTURE model_tex,
      VecFx32 pos,
      float off_y,
      uint command,
      uint disp_flag)
    {
        if (!GmMainIsDrawEnable())
            return;
        if (gmDecoDataGetInfo().tvx_model_data[model_index] == null)
        {
            AmbChunk data = (AmbChunk)amBindGet(gmDecoDataGetInfo().tvx_model, model_index);
            gmDecoDataGetInfo().tvx_model_data[model_index] = new TVX_FILE(data);
        }
        TVX_FILE file = gmDecoDataGetInfo().tvx_model_data[model_index];
        uint textureNum = AoTvxGetTextureNum(file);
        GMS_DECO_PRIM_DRAW_WORK[] gDecoTvxWork = g_deco_tvx_work;
        for (uint tex_no = 0; tex_no < textureNum; ++tex_no)
        {
            uint vertexNum = AoTvxGetVertexNum(file, tex_no);
            int textureId = AoTvxGetTextureId(file, tex_no);
            for (int index = 0; index < 16; ++index)
            {
                if (gDecoTvxWork[index].tex == null && gDecoTvxWork[index].tex_id == -1 && gDecoTvxWork[index].command == uint.MaxValue || gDecoTvxWork[index].tex == model_tex && gDecoTvxWork[index].tex_id == textureId && (int)gDecoTvxWork[index].command == (int)command)
                {
                    gDecoTvxWork[index].tex = model_tex;
                    gDecoTvxWork[index].tex_id = textureId;
                    gDecoTvxWork[index].command = command;
                    gDecoTvxWork[index].all_vtx_num += (ushort)vertexNum;
                    GMS_DECO_PRIM_DRAW_STACK decoPrimDrawStack = gDecoTvxWork[index].stack[gDecoTvxWork[index].stack_num];
                    decoPrimDrawStack.vtx = AoTvxGetVertex(file, tex_no);
                    decoPrimDrawStack.vtx_num = vertexNum;
                    decoPrimDrawStack.pos.Assign(pos);
                    decoPrimDrawStack.off_y = off_y;
                    decoPrimDrawStack.disp_flag = disp_flag;
                    ++gDecoTvxWork[index].stack_num;
                    break;
                }
            }
        }
    }

    private static bool gmDecoIsUseModel(int type)
    {
        bool flag = false;
        int num = g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
        if (type == 0 && 5 != num)
            flag = true;
        if (1 == type && (num == 0 || 2 == num))
            flag = true;
        return flag;
    }
}