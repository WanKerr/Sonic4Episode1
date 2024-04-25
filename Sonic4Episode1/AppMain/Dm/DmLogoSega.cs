public partial class AppMain
{
    public static int dm_logo_sega_com_file_num => dm_logo_sega_com_fileinfo_list.Length;

    private static int dm_logo_sega_localize_file_num => dm_logo_sega_localize_fileinfo_list_region_jp.Length;

    private void DmLogoSegaInit(object arg)
    {
        AoAccountClearCurrentId();
        if (this.DmLogoSegaBuildCheck())
        {
            this.dmLogoSegaStart();
        }
        else
        {
            MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(this.dmLogoSegaLoadWait), null, 0U, ushort.MaxValue, 4096U, 0, null, "DM_LSEGA_LW");
            this.DmLogoSegaLoad();
        }
    }

    private void DmLogoSegaLoad()
    {
        GSE_REGION region = GsEnvGetRegion();
        this.DmLogoComLoadFileCreate(dm_logo_sega_load_tcb);
        this.DmLogoComLoadFileReg(dm_logo_sega_load_tcb, dm_logo_sega_com_fileinfo_list, dm_logo_sega_com_file_num);
        this.DmLogoComLoadFileReg(dm_logo_sega_load_tcb, dm_logo_sega_localize_fileinfo_list_tbl[(int)region], dm_logo_sega_localize_file_num);
        this.DmLogoComLoadFileStart(dm_logo_sega_load_tcb);
    }

    private bool DmLogoSegaLoadCheck()
    {
        return dm_logo_sega_load_tcb.Target == null && dm_logo_sega_data[0] != null;
    }

    private void DmLogoSegaBuild()
    {
        AMS_AMB_HEADER[] amsAmbHeaderArray = new AMS_AMB_HEADER[2];
        dm_logo_sega_build_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(this.dmLogoSegaDataBuildMain), new GSF_TASK_PROCEDURE(this.dmLogoSegaDataBuildDest), 0U, ushort.MaxValue, 4096U, 0, null, "DM_LSEGA_BUILD");
        g_obj.def_user_light_flag = 1U;
        GmGameDBuildModelBuildInit();
        this.dm_logo_sega_obj_3d_list = GmGameDBuildRegBuildModel(dm_logo_sega_data[0], dm_logo_sega_data[1], 0U);
        dm_logo_sega_aos_tex = New<AOS_TEXTURE>(2);
        string sPath;
        amsAmbHeaderArray[0] = readAMBFile(amBindGet(dm_logo_sega_data[4], 1, out sPath));
        amsAmbHeaderArray[0].dir = sPath;
        amsAmbHeaderArray[1] = readAMBFile(amBindGet(dm_logo_sega_data[3], 105, out sPath));
        amsAmbHeaderArray[1].dir = sPath;
        AOS_TEXTURE[] dmLogoSegaAosTex = dm_logo_sega_aos_tex;
        for (int index = 0; index < 2; ++index)
        {
            if (amsAmbHeaderArray[index] != null)
            {
                AoTexBuild(dmLogoSegaAosTex[index], amsAmbHeaderArray[index]);
                AoTexLoad(dmLogoSegaAosTex[index]);
            }
        }
        for (int index = 0; index < 2; ++index)
            dm_logo_sega_efct_mdl_state[index] = ObjAction3dESModelLoadToDwork(dm_logo_sega_efct_mdl_data_work[index], (AmbChunk)amBindGet(dm_logo_sega_data[3], dm_logo_sega_efct_mdl_id_tbl[index]), 0U);
    }

    private bool DmLogoSegaBuildCheck()
    {
        return dm_logo_sega_build_state;
    }

    private void DmLogoSegaFlush()
    {
        dm_logo_sega_flush_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(this.dmLogoSegaDataFlushMain), new GSF_TASK_PROCEDURE(this.dmLogoSegaDataFlushDest), 0U, ushort.MaxValue, 4096U, 0, null, "DM_TOP_FLUSH");
        GmGameDBuildModelFlushInit();
        GmGameDBuildRegFlushModel(this.dm_logo_sega_obj_3d_list, dm_logo_sega_data[0].file_num);
        ArrayPointer<AOS_TEXTURE> dmLogoSegaAosTex = dm_logo_sega_aos_tex;
        int num = 0;
        while (num < 2)
        {
            AoTexRelease(dmLogoSegaAosTex);
            ++num;
            ++dmLogoSegaAosTex;
        }
        for (int index = 0; index < 2; ++index)
            dm_logo_sega_efct_mdl_state[index] = ObjAction3dESModelReleaseDwork(dm_logo_sega_efct_mdl_data_work[index]);
    }

    private bool DmLogoSegaFlushCheck()
    {
        return !dm_logo_sega_build_state;
    }

    private void DmLogoSegaRelease()
    {
        for (int index = 0; index < 5; ++index)
        {
            AMS_AMB_HEADER amsAmbHeader = dm_logo_sega_data[index];
            dm_logo_sega_data[index] = null;
        }
    }

    private bool DmLogoSegaReleaseCheck()
    {
        return dm_logo_sega_load_tcb.Target == null && dm_logo_sega_data[0] == null;
    }

    private void dmLogoSegaStart()
    {
        NNS_RGBA diffuse = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_RGB ambient = new NNS_RGB(1f, 1f, 1f);
        this.dmLogoSegaObjSysytemInit();
        GsSoundReset();
        GsSoundBegin(3, (uint)short.MaxValue, 0);
        DMS_LOGO_SEGA_WORK work = (DMS_LOGO_SEGA_WORK)MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(this.dmLogoSegaMainFunc), null, 0U, 0, 4096U, 0, () => new DMS_LOGO_SEGA_WORK(), "DM_LSEGA_MAIN").work;
        nnSetPrimitive3DMaterial(ref diffuse, ref ambient, 1f);
        AoActSysSetDrawStateEnable(false);
        this.dmLogoSegaActionCreate(work);
        work.ply_obj = this.dmLogoSegaCreatePlayer();
        work.timer = 0;
        work.func = new DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaStartWaitFunc);
        work.h_se = GsSoundAllocSeHandle();
    }

    private void dmLogoSegaObjSysytemInit()
    {
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR pos = new NNS_VECTOR(0.0f, 0.0f, 50f);
        ObjInit(4, GMD_TASK_PRIO_OBJSYS, 0, GMD_OBJ_LCD_X, GMD_OBJ_LCD_Y, GSD_DISP_WIDTH, GSD_DISP_HEIGHT);
        ObjDataAlloc(10);
        ObjDrawESEffectSystemInit(0, 20480U, 0U);
        ObjDrawSetNNCommandStateTbl(0U, 0U, true);
        AoActSysClearPeak();
        g_obj.flag = 4259848U;
        g_obj.ppPre = null;
        g_obj.ppPost = null;
        g_obj.ppCollision = null;
        g_obj.ppObjPre = null;
        g_obj.ppObjPost = null;
        g_obj.ppRegRecAuto = null;
        g_obj.draw_scale.x = g_obj.draw_scale.y = g_obj.draw_scale.z = 29127;
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
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
        ObjCameraInit(0, pos, 0, 0, 6144);
        ObjCamera3dInit(0);
        g_obj.glb_camera_id = 0;
        g_obj.glb_camera_type = 1;
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        obsCamera.user_func = new OBJF_CAMERA_USER_FUNC(this.dmLogoSegaCamera);
        obsCamera.command_state = 0U;
        obsCamera.scale = 0.9f;
        obsCamera.ofst.z = 1000f;
        this.amTrailEFInitialize();
    }

    private void dmLogoSegaPreEnd(DMS_LOGO_SEGA_WORK logo_work)
    {
        amTrailEFDeleteGroup(1);
        for (OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, ushort.MaxValue); obj_work != null; obj_work = ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
            obj_work.ppOut = null;
    }

    private void dmLogoSegaEnd(DMS_LOGO_SEGA_WORK logo_work)
    {
        this.dmLogoSegaActionDelete(logo_work);
        g_obj.ppPre = null;
        ObjObjectClearAllObject();
        ObjPreExit();
        ObjDrawESEffectSystemExit();
        ObjExit();
        GsSoundStopSeHandle(logo_work.h_se);
        GsSoundFreeSeHandle(logo_work.h_se);
        GsSoundHalt();
        GsSoundEnd();
        GsSoundReset();
    }

    private void dmLogoSegaMainFunc(MTS_TASK_TCB tcb)
    {
        DMS_LOGO_SEGA_WORK work = (DMS_LOGO_SEGA_WORK)tcb.work;
        work.h_se.snd_ctrl_param.volume = !GsSystemBgmIsPlay() ? 1f : 0.0f;
        if (AoSysIsShowPlatformUI())
        {
            if (IzFadeIsExe())
                IzFadeSetStopUpdate1Frame(null);
        }
        else
        {
            if (work.func != null)
                work.func(work);
            if (((int)work.flag & 1) != 0)
            {
                this.dmLogoSegaPreEnd(work);
                mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(this.gmLogoSegaPreEndWaitFunc));
                work.timer = 0;
                return;
            }
        }
        float frame = 0.0f;
        if (!AoSysIsShowPlatformUI() && ((int)work.flag & 2) != 0)
            frame = 1f;
        AoActSysSetDrawTaskPrio();
        for (int index = 0; index < 8; ++index)
        {
            AoActSetTexture(AoTexGetTexList(dm_logo_sega_aos_tex[dm_logo_sega_tex_id_tbl[index]]));
            AoActUpdate(work.act[index], frame);
            AoActDraw(work.act[index]);
        }
        NNS_RGBA diffuse = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_RGB ambient = new NNS_RGB(1f, 1f, 1f);
        if (!AoSysIsShowPlatformUI())
            amTrailEFUpdate(1);
        if (g_obj.glb_camera_id != -1)
        {
            SNNS_VECTOR disp_pos = new SNNS_VECTOR();
            SNNS_VECTOR snnsVector = new SNNS_VECTOR();
            SNNS_MATRIX dst = new SNNS_MATRIX();
            nnMakeUnitMatrix(ref dst);
            ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, g_obj.glb_camera_type, 0U);
            ObjCameraDispPosGet(g_obj.glb_camera_id, out disp_pos);
            amVectorSet(ref snnsVector, -dst.M03, -dst.M13, -dst.M23);
            nnAddVector(ref disp_pos, ref snnsVector, ref disp_pos);
            amEffectSetCameraPos(ref disp_pos);
        }
        nnSetPrimitive3DMaterial(ref diffuse, ref ambient, 1f);
        amTrailEFDraw(1, dm_logo_sega_aos_tex[0].texlist, 0U);
    }

    private void gmLogoSegaPreEndWaitFunc(MTS_TASK_TCB tcb)
    {
        DMS_LOGO_SEGA_WORK work = (DMS_LOGO_SEGA_WORK)tcb.work;
        ++work.timer;
        if (work.timer <= 2)
            return;
        this.dmLogoSegaEnd(work);
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(this.gmLogoSegaEndWaitFunc));
    }

    private void gmLogoSegaEndWaitFunc(MTS_TASK_TCB tcb)
    {
        if (!ObjObjectCheckClearAllObject() || ObjIsExitWait())
            return;
        this.DmLogoSegaFlush();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(this.gmLogoSegaFlushWaitFunc));
    }

    private void gmLogoSegaFlushWaitFunc(MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSegaFlushCheck())
            return;
        this.DmLogoSegaRelease();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(this.gmLogoSegaRelesehWaitFunc));
    }

    private void gmLogoSegaRelesehWaitFunc(MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSegaReleaseCheck())
            return;
        mtTaskClearTcb(tcb);
        SyChangeNextEvt();
    }

    public static bool mppSountInitializationDelayExpired()
    {
        return true;
    }

    private void dmLogoSegaStartWaitFunc(DMS_LOGO_SEGA_WORK logo_work)
    {
        bool flag = true;
        ++logo_work.timer;
        if (!mppSountInitializationDelayExpired())
            flag = false;
        if (logo_work.timer < 0 || !flag)
            return;
        IzFadeExit();
        logo_work.timer = 4;
        logo_work.func = new DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaRunLeftFunc);
        this.dmLogoSegaPlayerInitSeqRunLeft(logo_work.ply_obj);
        logo_work.flag |= 2U;
        logo_work.efct_obj = this.dmLogoSegaCreateDashEffect(logo_work.ply_obj, 0);
    }

    private void dmLogoSegaRunLeftFunc(DMS_LOGO_SEGA_WORK logo_work)
    {
        ++logo_work.timer;
        if (logo_work.timer <= 10)
            return;
        logo_work.timer = 0;
        logo_work.func = new DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaRunRightWaitFunc);
        logo_work.efct_obj.flag |= 8U;
        logo_work.efct_obj = null;
    }

    private void dmLogoSegaRunRightWaitFunc(DMS_LOGO_SEGA_WORK logo_work)
    {
        ++logo_work.timer;
        if (logo_work.timer <= 25)
            return;
        logo_work.timer = 0;
        logo_work.func = new DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaRunRightFunc);
        this.dmLogoSegaPlayerInitSeqRunRight(logo_work.ply_obj);
        logo_work.efct_obj = this.dmLogoSegaCreateDashEffect(logo_work.ply_obj, 1);
    }

    private void dmLogoSegaRunRightFunc(DMS_LOGO_SEGA_WORK logo_work)
    {
        ++logo_work.timer;
        if (logo_work.timer <= 10)
            return;
        logo_work.timer = 0;
        logo_work.func = new DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaDispWaitFunc);
        GsSoundPlaySe("Sega_Logo", logo_work.h_se);
        if (!GsSystemBgmIsPlay())
            return;
        logo_work.h_se.snd_ctrl_param.volume = 0.0f;
    }

    private void dmLogoSegaDispWaitFunc(DMS_LOGO_SEGA_WORK logo_work)
    {
        ++logo_work.timer;
        if (logo_work.timer < 180)
            return;
        logo_work.func = new DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaFadeOutWaitFunc);
        IzFadeInitEasy(0U, 3U, 60f, true);
    }

    private void dmLogoSegaFadeOutWaitFunc(DMS_LOGO_SEGA_WORK logo_work)
    {
        if (!IzFadeIsEnd())
            return;
        logo_work.flag |= 1U;
    }

    private OBS_OBJECT_WORK dmLogoSegaCreatePlayer()
    {
        OBS_OBJECT_WORK obj_work = OBM_OBJECT_TASK_DETAIL_INIT(8192, 0, 0, 0, () => new DMS_LOGO_SEGA_OBJ_3DNN_WORK(), "DM_LSEGA_PLY");
        DMS_LOGO_SEGA_OBJ_3DNN_WORK logoSegaObj3DnnWork = (DMS_LOGO_SEGA_OBJ_3DNN_WORK)obj_work;
        obj_work.obj_type = 1;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(ObjDrawActionSummary);
        obj_work.ppOutSub = null;
        obj_work.ppIn = null;
        obj_work.ppMove = null;
        obj_work.ppActCall = null;
        obj_work.ppRec = null;
        obj_work.ppLast = null;
        obj_work.ppFunc = null;
        OBS_ACTION3D_NN_WORK obj3d = logoSegaObj3DnnWork.obj_3d;
        ObjObjectCopyAction3dNNModel(obj_work, this.dm_logo_sega_obj_3d_list[0], obj3d);
        ObjDrawSetToon(obj3d);
        obj3d.command_state = 0U;
        ObjDataSet(logoSegaObj3DnnWork.data_work, dm_logo_sega_data[2]);
        logoSegaObj3DnnWork.data_work.num |= 32768;
        ObjObjectAction3dNNMotionLoad(obj_work, 0, false, logoSegaObj3DnnWork.data_work, null, 0, null, 136, 16);
        obj_work.disp_flag |= 16777728U;
        obj_work.scale.x = obj_work.scale.y = obj_work.scale.z = 8192;
        nnMakeUnitMatrix(obj3d.user_obj_mtx_r);
        nnTranslateMatrix(obj3d.user_obj_mtx_r, obj3d.user_obj_mtx_r, 0.0f, -36f / FXM_FX32_TO_FLOAT(g_obj.draw_scale.y), 0.0f);
        obj_work.pos.x = 2490368;
        obj_work.pos.y = 0;
        obj_work.pos.z = 0;
        return obj_work;
    }

    private void dmLogoSegaPlayerInitSeqRunLeft(OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x = 2490368;
        ObjDrawObjectActionSet(obj_work, 77);
        obj_work.disp_flag |= 5U;
        this.dmLogoSegaCreateTrail(obj_work);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(this.dmLogoSegaPlayerSeqRunLeft);
    }

    private void dmLogoSegaPlayerSeqRunLeft(OBS_OBJECT_WORK obj_work)
    {
        if (AoSysIsShowPlatformUI())
        {
            obj_work.disp_flag |= 4096U;
        }
        else
        {
            obj_work.disp_flag &= 4294963199U;
            obj_work.pos.x += -495616;
        }
    }

    private void dmLogoSegaPlayerInitSeqRunRight(OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x = -2490368;
        ObjDrawObjectActionSet(obj_work, 9);
        obj_work.disp_flag &= 4294967294U;
        obj_work.disp_flag |= 4U;
        this.dmLogoSegaCreateTrail(obj_work);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(this.dmLogoSegaPlayerSeqRunRight);
    }

    private void dmLogoSegaPlayerSeqRunRight(OBS_OBJECT_WORK obj_work)
    {
        if (AoSysIsShowPlatformUI())
        {
            obj_work.disp_flag |= 4096U;
        }
        else
        {
            obj_work.disp_flag &= 4294963199U;
            obj_work.pos.x += 495616;
        }
    }

    private OBS_OBJECT_WORK dmLogoSegaCreateDashEffect(
      OBS_OBJECT_WORK parent_obj,
      int type)
    {
        OBS_OBJECT_WORK obj_work = OBM_OBJECT_TASK_DETAIL_INIT(12288, 0, 0, 0, () => new DMS_LOGO_SEGA_OBJ_ES_WORK(), "DM_LSEGA_EFCT");
        DMS_LOGO_SEGA_OBJ_ES_WORK logoSegaObjEsWork = (DMS_LOGO_SEGA_OBJ_ES_WORK)obj_work;
        obj_work.obj_type = 2;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(ObjDrawActionSummary);
        obj_work.ppOutSub = null;
        obj_work.ppIn = null;
        obj_work.ppMove = null;
        obj_work.ppActCall = null;
        obj_work.ppRec = null;
        obj_work.ppLast = null;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(this.dmLogoSegaEffectMain);
        obj_work.parent_obj = parent_obj;
        obj_work.pos.Assign(parent_obj.pos);
        int index = type != 1 ? 52 : 53;
        ObjObjectAction3dESEffectLoad(obj_work, logoSegaObjEsWork.obj_3des, null, null, index, dm_logo_sega_data[3], 0, 0);
        logoSegaObjEsWork.obj_3des.command_state = 0U;
        ObjDataSet(logoSegaObjEsWork.data_work_texamb, amBindGet(dm_logo_sega_data[3], 97));
        logoSegaObjEsWork.data_work_texamb.num |= 32768;
        ObjDataSet(logoSegaObjEsWork.data_work_texlist, dm_logo_sega_aos_tex[1].texlist);
        logoSegaObjEsWork.data_work_texlist.num |= 32768;
        ObjObjectAction3dESTextureLoad(obj_work, logoSegaObjEsWork.obj_3des, logoSegaObjEsWork.data_work_texamb, null, 0, null, false);
        ObjObjectAction3dESTextureSetByDwork(obj_work, logoSegaObjEsWork.data_work_texlist);
        ObjDataSet(logoSegaObjEsWork.data_work_model, amBindGet(dm_logo_sega_data[3], dm_logo_sega_efct_mdl_id_tbl[type]));
        logoSegaObjEsWork.data_work_texlist.num |= 32768;
        ObjObjectAction3dESModelLoad(obj_work, logoSegaObjEsWork.obj_3des, logoSegaObjEsWork.data_work_model, null, 0, null, 0U, false);
        ObjObjectAction3dESModelSetByDwork(obj_work, dm_logo_sega_efct_mdl_data_work[type]);
        logoSegaObjEsWork.obj_3des.disp_rot.x = 0;
        logoSegaObjEsWork.obj_3des.disp_rot.y = 0;
        logoSegaObjEsWork.obj_3des.disp_rot.z = 0;
        obj_work.scale.x = FX_Mul(parent_obj.scale.x, g_obj.draw_scale.x);
        obj_work.scale.y = FX_Mul(parent_obj.scale.y, g_obj.draw_scale.y);
        obj_work.scale.z = FX_Mul(parent_obj.scale.z, g_obj.draw_scale.z);
        obj_work.disp_flag |= 4194304U;
        logoSegaObjEsWork.obj_3des.flag |= 8U;
        return obj_work;
    }

    private void dmLogoSegaEffectMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        if (AoSysIsShowPlatformUI())
        {
            obj_work.disp_flag |= 4096U;
        }
        else
        {
            obj_work.disp_flag &= 4294963199U;
            obj_work.pos.Assign(parentObj.pos);
            if (((int)obj_work.parent_obj.disp_flag & 1) != 0)
                obj_work.pos.x -= -81920;
            else
                obj_work.pos.x += -81920;
            obj_work.pos.y += 409600;
            obj_work.pos.z += 409600;
            obj_work.disp_flag &= 4294967294U;
            obj_work.disp_flag |= parentObj.disp_flag & 1U;
        }
    }

    private void dmLogoSegaCreateTrail(OBS_OBJECT_WORK obj_work)
    {
        NNS_TEXLIST texlist = dm_logo_sega_aos_tex[0].texlist;
        amTrailMakeEffect(new AMS_TRAIL_PARAM()
        {
            startColor = {
        r = 0.0f,
        g = 0.0f,
        b = 1f,
        a = 1f
      },
            endColor = {
        r = 0.0f,
        g = 0.0f,
        b = 1f,
        a = 0.0f
      },
            startSize = 88f,
            endSize = 88f,
            life = 35f,
            vanish_time = 10f,
            trail_obj_work = obj_work,
            partsNum = 63,
            zBias = -65536f,
            texId = texlist.nTex - 1,
            blendType = 0,
            zTest = 1
        }, 1, 1);
    }

    private void dmLogoSegaCamera(OBS_CAMERA camera)
    {
        camera.disp_pos.x = 0.0f;
        camera.disp_pos.y = 0.0f;
        camera.disp_pos.z = 200f;
        camera.target_pos.x = 0.0f;
        camera.target_pos.y = 0.0f;
        camera.target_pos.z = 0.0f;
    }

    private static void dmLogoSegaLoadPostFunc(DMS_LOGO_COM_LOAD_CONTEXT context)
    {
        dm_logo_sega_data[context.no] = readAMBFile(context.fs_req);
    }

    private void dmLogoSegaDataBuildMain(MTS_TASK_TCB tcb)
    {
        bool flag = true;
        if (!GmGameDBuildCheckBuildModel())
            flag = false;
        ArrayPointer<AOS_TEXTURE> dmLogoSegaAosTex = dm_logo_sega_aos_tex;
        int num = 0;
        while (num < 2)
        {
            if (!AoTexIsLoaded(dmLogoSegaAosTex))
                flag = false;
            ++num;
            ++dmLogoSegaAosTex;
        }
        for (int index = 0; index < 2; ++index)
        {
            if (dm_logo_sega_efct_mdl_state[index] != -1 && !amDrawIsRegistComplete(dm_logo_sega_efct_mdl_state[index]))
            {
                flag = false;
                dm_logo_sega_efct_mdl_state[index] = -1;
            }
        }
        if (!flag)
            return;
        mtTaskClearTcb(tcb);
        dm_logo_sega_build_state = true;
    }

    private void dmLogoSegaDataBuildDest(MTS_TASK_TCB tcb)
    {
        dm_logo_sega_build_tcb = null;
    }

    private void dmLogoSegaDataFlushMain(MTS_TASK_TCB tcb)
    {
        bool flag = true;
        if (!GmGameDBuildCheckFlushModel())
            flag = false;
        ArrayPointer<AOS_TEXTURE> dmLogoSegaAosTex = dm_logo_sega_aos_tex;
        int num = 0;
        while (num < 2)
        {
            if (!AoTexIsReleased(dmLogoSegaAosTex))
                flag = false;
            ++num;
            ++dmLogoSegaAosTex;
        }
        for (int index = 0; index < 2; ++index)
        {
            if (dm_logo_sega_efct_mdl_state[index] != -1)
            {
                if (ObjAction3dESModelReleaseDworkCheck(dm_logo_sega_efct_mdl_data_work[index], dm_logo_sega_efct_mdl_state[index]))
                    dm_logo_sega_efct_mdl_state[index] = -1;
                else
                    flag = false;
            }
        }
        if (!flag)
            return;
        dm_logo_sega_aos_tex = null;
        this.dm_logo_sega_obj_3d_list = null;
        mtTaskClearTcb(tcb);
        dm_logo_sega_build_state = false;
    }

    private void dmLogoSegaDataFlushDest(MTS_TASK_TCB tcb)
    {
        dm_logo_sega_flush_tcb = null;
    }

    private void dmLogoSegaLoadWait(MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSegaLoadCheck())
            return;
        this.DmLogoSegaBuild();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(this.dmLogoSegaBuildWait));
    }

    private void dmLogoSegaBuildWait(MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSegaBuildCheck())
            return;
        mtTaskClearTcb(tcb);
        this.dmLogoSegaStart();
    }

    private void dmLogoSegaActionCreate(DMS_LOGO_SEGA_WORK logo_work)
    {
        A2S_AMA_HEADER ama = readAMAFile(amBindGet(dm_logo_sega_data[4], 0));
        for (uint id = 0; id < 8U; ++id)
        {
            AoActSetTexture(AoTexGetTexList(dm_logo_sega_aos_tex[dm_logo_sega_tex_id_tbl[(int)id]]));
            logo_work.act[(int)id] = AoActCreate(ama, id);
        }
    }

    private void dmLogoSegaActionDelete(DMS_LOGO_SEGA_WORK logo_work)
    {
        for (int index = 0; index < 8; ++index)
            AoActDelete(logo_work.act[index]);
    }

}