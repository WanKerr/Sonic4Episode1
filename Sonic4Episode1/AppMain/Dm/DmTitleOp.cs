public partial class AppMain
{

    public DMS_TITLEOP_OBJ_3DNN_WORK DMM_TITLEOP_CREATE_3D_OBJ(
      ushort prio,
      byte group,
      TaskWorkFactoryDelegate work_size,
      string name)
    {
        return dmTitleOpCreate3DObj(prio, group, work_size);
    }

    private void DmTitleOpLoad()
    {
        DmLogoComLoadFileCreate(dm_titleop_load_tcb);
        DmLogoComLoadFileReg(dm_titleop_load_tcb, dm_titleop_com_fileinfo_list, 2);
        DmLogoComLoadFileStart(dm_titleop_load_tcb);
    }

    private bool DmTitleOpLoadCheck()
    {
        return dm_titleop_load_tcb.Target == null && dm_titleop_data[0] != null;
    }

    private void DmTitleOpBuild()
    {
        AMS_AMB_HEADER[] amsAmbHeaderArray = new AMS_AMB_HEADER[1];
        dm_titleop_build_tcb = MTM_TASK_MAKE_TCB(dmTitleOpDataBuildMain, dmTitleOpDataBuildDest, 0U, ushort.MaxValue, 4096U, 0, null, "DM_TOP_BUILD");
        dm_titleop_aos_tex = New<AOS_TEXTURE>(1);
        string sPath = null;
        amsAmbHeaderArray[0] = readAMBFile(amBindGet(dm_titleop_data[1], 1, out sPath));
        amsAmbHeaderArray[0].dir = sPath;
        AOS_TEXTURE[] dmTitleopAosTex = dm_titleop_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            AoTexBuild(dmTitleopAosTex[index], amsAmbHeaderArray[index]);
            AoTexLoad(dmTitleopAosTex[index]);
        }
        ObjInit(4, GMD_TASK_PRIO_OBJSYS, 0, GMD_OBJ_LCD_X, GMD_OBJ_LCD_Y, GSD_DISP_WIDTH, GSD_DISP_HEIGHT);
        ObjDataAlloc(10);
        ObjDrawSetNNCommandStateTbl(0U, 0U, true);
        ObjDrawSetNNCommandStateTbl(1U, 1U, true);
        ObjDrawSetNNCommandStateTbl(2U, 2U, true);
        ObjDrawSetNNCommandStateTbl(3U, 3U, true);
        ObjDrawSetNNCommandStateTbl(4U, 4U, true);
        ObjDrawSetNNCommandStateTbl(5U, 5U, true);
        AoActSysClearPeak();
        GmGameDBuildModelBuildInit();
        dm_titleop_obj_3d_list = GmGameDBuildRegBuildModel((AMS_AMB_HEADER)dm_titleop_mapfar_data[0], (AMS_AMB_HEADER)dm_titleop_mapfar_data[1], 0U);
        dm_titleop_water_obj_3d_list = GmGameDBuildRegBuildModel((AMS_AMB_HEADER)dm_titleop_mapfar_data[3], (AMS_AMB_HEADER)dm_titleop_mapfar_data[1], 0U);
    }

    private bool DmTitleOpBuildCheck()
    {
        return dm_titleop_build_state;
    }

    private void DmTitleOpFlush()
    {
        dm_titleop_flush_tcb = MTM_TASK_MAKE_TCB(dmTitleOpDataFlushMain, dmTitleOpDataFlushDest, 0U, ushort.MaxValue, 4096U, 0, null, "DM_TOP_FLUSH");
        AOS_TEXTURE[] dmTitleopAosTex = dm_titleop_aos_tex;
        for (int index = 0; index < 1; ++index)
            AoTexRelease(dmTitleopAosTex[index]);
        GmGameDBuildModelFlushInit();
        GmGameDBuildRegFlushModel(dm_titleop_obj_3d_list, ((AMS_AMB_HEADER)dm_titleop_mapfar_data[0]).file_num);
        GmGameDBuildRegFlushModel(dm_titleop_water_obj_3d_list, ((AMS_AMB_HEADER)dm_titleop_mapfar_data[3]).file_num);
    }

    private bool DmTitleOpFlushCheck()
    {
        return !dm_titleop_build_state;
    }

    private void DmTitleOpRelease()
    {
        for (int index = 0; index < 2; ++index)
            dm_titleop_data[index] = null;
        for (int index = 0; index < 4; ++index)
            dm_titleop_mapfar_data[index] = null;
    }

    private bool DmTitleOpReleaseCheck()
    {
        return dm_titleop_load_tcb.Target == null && dm_titleop_data[0] == null;
    }

    private void DmTitleOpInit()
    {
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.Clear();
        NNS_VECTOR pos = new NNS_VECTOR(0.0f, 0.0f, 50f);
        g_obj.flag = 4259848U;
        g_obj.ppPre = null;
        g_obj.ppPost = null;
        g_obj.ppCollision = null;
        g_obj.ppObjPre = null;
        g_obj.ppObjPost = null;
        g_obj.ppRegRecAuto = null;
        g_obj.draw_scale.x = g_obj.draw_scale.y = g_obj.draw_scale.z = 13107;
        g_obj.inv_draw_scale.x = g_obj.inv_draw_scale.y = g_obj.inv_draw_scale.z = FX_Div(4096, g_obj.draw_scale.x);
        g_obj.depth = 128;
        g_obj.ambient_color.r = 0.8f;
        g_obj.ambient_color.g = 0.8f;
        g_obj.ambient_color.b = 0.8f;
        nnsVector.x = -1f;
        nnsVector.y = -1f;
        nnsVector.z = -1f;
        nnNormalizeVector(nnsVector, nnsVector);
        ObjDrawSetParallelLight(NNE_LIGHT_0, ref col, 1f, nnsVector);
        ObjCameraInit(0, pos, 0, 0, 12288);
        ObjCamera3dInit(0);
        g_obj.glb_camera_id = 0;
        g_obj.glb_camera_type = 0;
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        obsCamera.user_func = dmTitleOpCamera;
        obsCamera.command_state = 0U;
        obsCamera.fovy = NNM_DEGtoA32(40f);
        obsCamera.znear = 0.1f;
        obsCamera.zfar = 32768f;
        dmTitleOpMgrInit();
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    private void DmTitleOpExit()
    {
        if (dm_titleop_mgr_tcb == null)
            return;
        ((DMS_TITLEOP_MGR_WORK)dm_titleop_mgr_tcb.work).flag |= 536870912U;
    }

    private bool DmTitleOpExitEndCheck()
    {
        return dm_titleop_mgr_tcb == null;
    }

    private void DmTitleOpDraw2D()
    {
        if (dm_titleop_mgr_tcb == null)
            return;
        DMS_TITLEOP_MGR_WORK work = (DMS_TITLEOP_MGR_WORK)dm_titleop_mgr_tcb.work;
        if (((int)work.flag & int.MinValue) == 0)
            return;
        AoActSetTexture(AoTexGetTexList(dm_titleop_aos_tex[0]));
        int num = 5;
        for (int index = 0; index < num; ++index)
        {
            if (((int)work.flag & 1 << index) != 0 && ((int)work.flag & 256 << index) == 0)
            {
                AoActAcmPush();
                AoActAcmInit();
                switch (index)
                {
                    case 0:
                        AoActAcmApplyTrans(0.0f, -25f, 0.0f);
                        goto default;
                    case 1:
                        AoActSetFrame(work.act[index], work.finger_frame);
                        AoActAcmApplyTrans(-10f, -15f, 0.0f);
                        AoActAcmApplyScale(0.9f, 0.9f);
                        AoActUpdate(work.act[index], 0.0f);
                        ++work.finger_frame;
                        if (work.finger_frame > 84.0)
                        {
                            work.finger_frame = 60f;
                        }
                        break;
                    case 3:
                        AoActAcmApplyTrans(0.0f, -10f, 0.0f);
                        goto default;
                    default:
                        AoActAcmApplyScale(0.9f, 0.9f);
                        AoActUpdate(work.act[index]);
                        break;
                }
                AoActSortRegAction(work.act[index]);
                AoActAcmPop(1U);
            }
        }
    }

    private bool DmTitleOpIsLogoActFinish()
    {
        return dm_titleop_mgr_tcb != null && ((int)((DMS_TITLEOP_MGR_WORK)dm_titleop_mgr_tcb.work).flag & 1073741824) != 0;
    }

    private void DmTitleOpDispRightEnable(bool disp)
    {
        if (dm_titleop_mgr_tcb == null)
            return;
        DMS_TITLEOP_MGR_WORK work = (DMS_TITLEOP_MGR_WORK)dm_titleop_mgr_tcb.work;
        if (disp)
            work.flag &= 4294963199U;
        else
            work.flag |= 4096U;
    }

    private void DmTitleOpSetRetOptionState()
    {
        if (dm_titleop_mgr_tcb == null)
            return;
        DMS_TITLEOP_MGR_WORK work = (DMS_TITLEOP_MGR_WORK)dm_titleop_mgr_tcb.work;
        work.flag |= 1073741951U;
        work.frame = 105;
        work.finger_frame = 75f;
        AoActSetTexture(AoTexGetTexList(dm_titleop_aos_tex[0]));
        int num = 5;
        for (int index = 0; index < num; ++index)
        {
            AoActAcmPush();
            AoActAcmInit();
            AoActSetFrame(work.act[index], 75f);
            AoActUpdate(work.act[index], 0.0f);
            AoActAcmPop(1U);
        }
    }

    private static void dmTitleOpLoadPostFuncMapFar(DMS_LOGO_COM_LOAD_CONTEXT context)
    {
        dm_titleop_data[context.no] = context.fs_req;
        AMS_AMB_HEADER header = readAMBFile(dm_titleop_data[context.no]);
        for (int index = 0; index < header.file_num; ++index)
        {
            string sPath;
            AMS_AMB_HEADER amsAmbHeader = readAMBFile(amBindGet(header, index, out sPath));
            amsAmbHeader.dir = sPath;
            dm_titleop_mapfar_data[index] = amsAmbHeader;
        }
    }

    private static void dmTitleOpLoadPostFuncTitleLogo(DMS_LOGO_COM_LOAD_CONTEXT context)
    {
        dm_titleop_data[context.no] = context.fs_req;
        context.fs_req = null;
    }

    private void dmTitleOpDataBuildMain(MTS_TASK_TCB tcb)
    {
        bool flag = true;
        AOS_TEXTURE[] dmTitleopAosTex = dm_titleop_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            if (!AoTexIsLoaded(dmTitleopAosTex[index]))
                flag = false;
        }
        if (!GmGameDBuildCheckBuildModel())
            flag = false;
        if (!flag)
            return;
        mtTaskClearTcb(tcb);
        dm_titleop_build_state = true;
    }

    private void dmTitleOpDataBuildDest(MTS_TASK_TCB tcb)
    {
        dm_titleop_build_tcb = null;
    }

    private void dmTitleOpDataFlushMain(MTS_TASK_TCB tcb)
    {
        bool flag = true;
        AOS_TEXTURE[] dmTitleopAosTex = dm_titleop_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            if (!AoTexIsReleased(dmTitleopAosTex[index]))
                flag = false;
        }
        if (!GmGameDBuildCheckFlushModel())
            flag = false;
        if (!flag)
            return;
        for (int index = 0; index < dm_titleop_aos_tex.Length; ++index)
            dm_titleop_aos_tex[index] = null;
        dm_titleop_aos_tex = null;
        dm_titleop_obj_3d_list = null;
        dm_titleop_water_obj_3d_list = null;
        mtTaskClearTcb(tcb);
        dm_titleop_build_state = false;
    }

    private void dmTitleOpDataFlushDest(MTS_TASK_TCB tcb)
    {
        dm_titleop_flush_tcb = null;
    }

    private void dmTitleOpMgrInit()
    {
        dm_titleop_mgr_tcb = MTM_TASK_MAKE_TCB(dmTitleOpMgrMain, dmTitleOpMgrDest, 0U, ushort.MaxValue, 12288U, 0, () => new DMS_TITLEOP_MGR_WORK(), "DM_TOP_MGR");
        DMS_TITLEOP_MGR_WORK work = (DMS_TITLEOP_MGR_WORK)dm_titleop_mgr_tcb.work;
        work.Clear();
        dmTitleOpCreateObjFarSky();
        dmTitleOpCreateObjFarRock(0U);
        dmTitleOpCreateObjFarRock(1U);
        dmTitleOpCreateObjFarRock(2U);
        dmTitleOpCreateObjFarSea();
        dmTitleOpCreateAction(work);
        work.flag |= 268435456U;
        dm_titleop_scrl_x_ofst = 0;
    }

    private void dmTitleOpMgrDest(MTS_TASK_TCB tcb)
    {
        dm_titleop_mgr_tcb = null;
    }

    private void dmTitleOpMgrMain(MTS_TASK_TCB tcb)
    {
        DMS_TITLEOP_MGR_WORK work = (DMS_TITLEOP_MGR_WORK)tcb.work;
        if (((int)work.flag & 536870912) != 0)
        {
            dmTitleOpEndStart(tcb);
        }
        else
        {
            ++work.frame;
            if (work.frame == 30)
                work.flag |= 111U;
            if (work.frame == 75)
                work.flag |= 16U;
            else if (work.frame >= 105)
                work.flag |= 1073741824U;
            if (((int)work.flag & 268435456) != 0)
            {
                dm_titleop_scrl_x_ofst += sbyte.MinValue;
                int num = dm_titleop_scrl_x_ofst / 3145728;
                dm_titleop_scrl_x_ofst -= num * 3145728;
            }
            ObjDraw3DNNUserFunc(_dmTitleOpPreDrawDT, null, 0, 0U);
            ObjDraw3DNNUserFunc(_dmTitleOpFallShaderPreRenderUserFunc, null, 0, 2U);
            amDrawSetFog(0U, 1);
            amDrawSetFogColor(0U, 0.7f, 0.95f, 1f);
            amDrawSetFogRange(0U, 1f, 500f);
            amDrawSetFog(4U, 0);
        }
    }

    private void dmTitleOpCreateObjFarSky()
    {
        DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork1 = DMM_TITLEOP_CREATE_3D_OBJ(24576, 0, () => new DMS_TITLEOP_OBJ_3DNN_WORK(), "DM_TOP_SKYT");
        OBS_OBJECT_WORK obj_work1 = (OBS_OBJECT_WORK)titleopObj3DnnWork1;
        obj_work1.obj_type = 1;
        ObjObjectCopyAction3dNNModel(obj_work1, dm_titleop_obj_3d_list[0], titleopObj3DnnWork1.obj_3d);
        obj_work1.obj_3d.command_state = 3U;
        ObjAction3dNNMaterialMotionLoad(titleopObj3DnnWork1.obj_3d, 0, null, null, 0, (AMS_AMB_HEADER)dm_titleop_mapfar_data[2]);
        ObjDrawObjectActionSet3DNNMaterial(obj_work1, 0);
        obj_work1.disp_flag |= 13697028U;
        obj_work1.ppFunc = dmTitleOpFarSkyFunc;
        DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork2 = DMM_TITLEOP_CREATE_3D_OBJ(16384, 0, () => new DMS_TITLEOP_OBJ_3DNN_WORK(), "DM_TOP_SKYB");
        OBS_OBJECT_WORK obj_work2 = (OBS_OBJECT_WORK)titleopObj3DnnWork2;
        obj_work2.obj_type = 1;
        ObjObjectCopyAction3dNNModel(obj_work2, dm_titleop_obj_3d_list[0], titleopObj3DnnWork2.obj_3d);
        obj_work2.obj_3d.command_state = 1U;
        ObjAction3dNNMaterialMotionLoad(titleopObj3DnnWork2.obj_3d, 0, null, null, 0, (AMS_AMB_HEADER)dm_titleop_mapfar_data[2]);
        ObjDrawObjectActionSet3DNNMaterial(obj_work2, 0);
        obj_work2.disp_flag |= 13697028U;
        obj_work2.ppFunc = dmTitleOpFarSkyFunc;
    }

    private void dmTitleOpFarSkyFunc(OBS_OBJECT_WORK obj_work)
    {
        DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork = (DMS_TITLEOP_OBJ_3DNN_WORK)obj_work;
        titleopObj3DnnWork.sky_rot += 0.01f;
        if (titleopObj3DnnWork.sky_rot > 360.0)
            titleopObj3DnnWork.sky_rot -= 360f;
        nnMakeUnitMatrix(obj_work.obj_3d.user_obj_mtx);
        nnRotateYMatrix(obj_work.obj_3d.user_obj_mtx, obj_work.obj_3d.user_obj_mtx, (ushort)NNM_DEGtoA16(titleopObj3DnnWork.sky_rot));
    }

    private void dmTitleOpCreateObjFarRock(uint type)
    {
        DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork = DMM_TITLEOP_CREATE_3D_OBJ(24832, 0, () => new DMS_TITLEOP_OBJ_3DNN_WORK(), "DM_TOP_ROCKT");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)titleopObj3DnnWork;
        obj_work.obj_type = 1;
        ObjObjectCopyAction3dNNModel(obj_work, dm_titleop_obj_3d_list[(int)(1U + type)], titleopObj3DnnWork.obj_3d);
        obj_work.obj_3d.command_state = 3U;
        obj_work.disp_flag |= 5242880U;
        obj_work.disp_flag |= 268435456U;
        obj_work.ppOut = dmTitleOpObjRockDraw;
        titleopObj3DnnWork.rock_setting = dm_titleop_rock_setting[(int)type];
        titleopObj3DnnWork.rock_setting_num = dm_titleop_rock_setting_num[(int)type];
    }

    private void dmTitleOpCreateObjFarSea()
    {
        DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork = DMM_TITLEOP_CREATE_3D_OBJ(20480, 0, () => new DMS_TITLEOP_OBJ_3DNN_WORK(), "DM_TOP_SEA");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)titleopObj3DnnWork;
        obj_work.obj_type = 1;
        ObjObjectCopyAction3dNNModel(obj_work, dm_titleop_water_obj_3d_list[0], titleopObj3DnnWork.obj_3d);
        obj_work.obj_3d.command_state = 2U;
        ObjAction3dNNMaterialMotionLoad(titleopObj3DnnWork.obj_3d, 0, null, null, 1, (AMS_AMB_HEADER)dm_titleop_mapfar_data[2]);
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.obj_3d.mat_speed = 0.2f;
        obj_work.dir.y = 49152;
        obj_work.disp_flag |= 5308420U;
    }

    private DMS_TITLEOP_OBJ_3DNN_WORK dmTitleOpCreate3DObj(
      ushort prio,
      byte group,
      TaskWorkFactoryDelegate work_size)
    {
        OBS_OBJECT_WORK obsObjectWork = OBM_OBJECT_TASK_DETAIL_INIT(prio, group, 0, 0, work_size, null);
        DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork = (DMS_TITLEOP_OBJ_3DNN_WORK)obsObjectWork;
        obsObjectWork.ppOut = dmTitleOpObjDraw;
        obsObjectWork.ppOutSub = null;
        obsObjectWork.ppIn = null;
        obsObjectWork.ppMove = null;
        obsObjectWork.ppActCall = null;
        obsObjectWork.ppRec = null;
        obsObjectWork.ppLast = null;
        obsObjectWork.ppFunc = null;
        return titleopObj3DnnWork;
    }

    private void dmTitleOpCreateAction(DMS_TITLEOP_MGR_WORK top_mgr_work)
    {
        string sPath = null;
        A2S_AMA_HEADER ama = readAMAFile(amBindGet(dm_titleop_data[1], 0, out sPath));
        for (uint id = 0; id < 7U; ++id)
        {
            AoActSetTexture(AoTexGetTexList(dm_titleop_aos_tex[0]));
            top_mgr_work.act[(int)id] = AoActCreate(ama, id);
        }
        top_mgr_work.flag |= 2147483648U;
    }

    private void dmTitleOpDeleteAction(ref DMS_TITLEOP_MGR_WORK top_mgr_work)
    {
        for (int index = 0; index < 7; ++index)
            AoActDelete(top_mgr_work.act[index]);
    }

    private void dmTitleOpEndStart(MTS_TASK_TCB tcb)
    {
        DMS_TITLEOP_MGR_WORK work = (DMS_TITLEOP_MGR_WORK)tcb.work;
        dmTitleOpPreEnd(ref work);
        work.frame = 0;
        mtTaskChangeTcbProcedure(tcb, dmTitleOpPreEndWait);
    }

    private void dmTitleOpPreEnd(ref DMS_TITLEOP_MGR_WORK top_mgr_work)
    {
        for (OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, ushort.MaxValue); obj_work != null; obj_work = ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
            obj_work.ppOut = null;
    }

    private void dmTitleOpPreEndWait(MTS_TASK_TCB tcb)
    {
        DMS_TITLEOP_MGR_WORK work = (DMS_TITLEOP_MGR_WORK)tcb.work;
        ++work.frame;
        if (work.frame <= 2)
            return;
        dmTitleOpEnd(ref work);
        mtTaskChangeTcbProcedure(tcb, dmTitleOpEndWait);
    }

    private void dmTitleOpEnd(ref DMS_TITLEOP_MGR_WORK top_mgr_work)
    {
        dmTitleOpDeleteAction(ref top_mgr_work);
        g_obj.ppPre = null;
        ObjObjectClearAllObject();
        ObjPreExit();
        ObjExit();
    }

    private void dmTitleOpEndWait(MTS_TASK_TCB tcb)
    {
        if (!ObjObjectCheckClearAllObject() || ObjIsExitWait())
            return;
        mtTaskClearTcb(tcb);
    }

    private static void dmTitleOpPreDrawDT(object data)
    {
        amDrawSetBGColor(dm_titleop_clear_color);
    }

    private void dmTitleOpObjDraw(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawActionSummary(obj_work);
    }

    private void dmTitleOpObjRockDraw(OBS_OBJECT_WORK obj_work)
    {
        DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork = (DMS_TITLEOP_OBJ_3DNN_WORK)obj_work;
        for (int index = 0; index < titleopObj3DnnWork.rock_setting_num; ++index)
        {
            obj_work.pos.Assign(titleopObj3DnnWork.rock_setting[index].pos);
            obj_work.pos.x += dm_titleop_scrl_x_ofst;
            if (obj_work.pos.x < -1966080)
                obj_work.pos.x += 3145728;
            else if (obj_work.pos.x > 1966080)
                obj_work.pos.x -= 3145728;
            if (obj_work.pos.x >= -1179648 && obj_work.pos.x <= 1179648)
            {
                obj_work.scale.Assign(titleopObj3DnnWork.rock_setting[index].scale);
                ObjDrawActionSummary(obj_work);
            }
        }
    }

    private static void dmTitleOpFallShaderPreRenderUserFunc(object data)
    {
        NNS_RGBA_U8 color = new NNS_RGBA_U8(0, 0, 0, byte.MaxValue);
        AMS_RENDER_TARGET target = _am_render_manager.targetp != _gm_mapFar_render_work ? _gm_mapFar_render_work : _am_draw_target;
        if (target.width == 0)
            return;
        amRenderCopyTarget(target, color);
    }

    private void dmTitleOpDrawFallShaderPreSettingUserFunc(ref object data)
    {
        if ((_am_render_manager.targetp != _gm_mapFar_render_work ? _gm_mapFar_render_work : _am_draw_target).width == 0)
            return;
        amDrawGetProjectionMatrix();
    }

    private void dmTitleOpCamera(OBS_CAMERA camera)
    {
        camera.disp_pos.x = 0.0f;
        camera.disp_pos.y = 5f;
        camera.disp_pos.z = 160f;
        camera.target_pos.x = 0.0f;
        camera.target_pos.y = 0.0f;
        camera.target_pos.z = 10f;
    }

}