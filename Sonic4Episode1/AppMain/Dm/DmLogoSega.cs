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
    public static int dm_logo_sega_com_file_num
    {
        get
        {
            return AppMain.dm_logo_sega_com_fileinfo_list.Length;
        }
    }

    private static int dm_logo_sega_localize_file_num
    {
        get
        {
            return AppMain.dm_logo_sega_localize_fileinfo_list_region_jp.Length;
        }
    }

    private void DmLogoSegaInit(object arg)
    {
        AppMain.AoAccountClearCurrentId();
        if (this.DmLogoSegaBuildCheck())
        {
            this.dmLogoSegaStart();
        }
        else
        {
            AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSegaLoadWait), (AppMain.GSF_TASK_PROCEDURE)null, 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "DM_LSEGA_LW");
            this.DmLogoSegaLoad();
        }
    }

    private void DmLogoSegaLoad()
    {
        AppMain.GSE_REGION region = AppMain.GsEnvGetRegion();
        this.DmLogoComLoadFileCreate(AppMain.dm_logo_sega_load_tcb);
        this.DmLogoComLoadFileReg(AppMain.dm_logo_sega_load_tcb, AppMain.dm_logo_sega_com_fileinfo_list, AppMain.dm_logo_sega_com_file_num);
        this.DmLogoComLoadFileReg(AppMain.dm_logo_sega_load_tcb, AppMain.dm_logo_sega_localize_fileinfo_list_tbl[(int)region], AppMain.dm_logo_sega_localize_file_num);
        this.DmLogoComLoadFileStart(AppMain.dm_logo_sega_load_tcb);
    }

    private bool DmLogoSegaLoadCheck()
    {
        return AppMain.dm_logo_sega_load_tcb.Target == null && AppMain.dm_logo_sega_data[0] != null;
    }

    private void DmLogoSegaBuild()
    {
        AppMain.AMS_AMB_HEADER[] amsAmbHeaderArray = new AppMain.AMS_AMB_HEADER[2];
        AppMain.dm_logo_sega_build_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSegaDataBuildMain), new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSegaDataBuildDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "DM_LSEGA_BUILD");
        AppMain.g_obj.def_user_light_flag = 1U;
        AppMain.GmGameDBuildModelBuildInit();
        this.dm_logo_sega_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.dm_logo_sega_data[0], AppMain.dm_logo_sega_data[1], 0U);
        AppMain.dm_logo_sega_aos_tex = AppMain.New<AppMain.AOS_TEXTURE>(2);
        string sPath;
        amsAmbHeaderArray[0] = AppMain.readAMBFile(AppMain.amBindGet(AppMain.dm_logo_sega_data[4], 1, out sPath));
        amsAmbHeaderArray[0].dir = sPath;
        amsAmbHeaderArray[1] = AppMain.readAMBFile(AppMain.amBindGet(AppMain.dm_logo_sega_data[3], 105, out sPath));
        amsAmbHeaderArray[1].dir = sPath;
        AppMain.AOS_TEXTURE[] dmLogoSegaAosTex = AppMain.dm_logo_sega_aos_tex;
        for (int index = 0; index < 2; ++index)
        {
            if (amsAmbHeaderArray[index] != null)
            {
                AppMain.AoTexBuild(dmLogoSegaAosTex[index], amsAmbHeaderArray[index]);
                AppMain.AoTexLoad(dmLogoSegaAosTex[index]);
            }
        }
        for (int index = 0; index < 2; ++index)
            AppMain.dm_logo_sega_efct_mdl_state[index] = AppMain.ObjAction3dESModelLoadToDwork(AppMain.dm_logo_sega_efct_mdl_data_work[index], (object)(AppMain.AmbChunk)AppMain.amBindGet(AppMain.dm_logo_sega_data[3], AppMain.dm_logo_sega_efct_mdl_id_tbl[index]), 0U);
    }

    private bool DmLogoSegaBuildCheck()
    {
        return AppMain.dm_logo_sega_build_state;
    }

    private void DmLogoSegaFlush()
    {
        AppMain.dm_logo_sega_flush_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSegaDataFlushMain), new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSegaDataFlushDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "DM_TOP_FLUSH");
        AppMain.GmGameDBuildModelFlushInit();
        AppMain.GmGameDBuildRegFlushModel(this.dm_logo_sega_obj_3d_list, AppMain.dm_logo_sega_data[0].file_num);
        AppMain.ArrayPointer<AppMain.AOS_TEXTURE> dmLogoSegaAosTex = (AppMain.ArrayPointer<AppMain.AOS_TEXTURE>)AppMain.dm_logo_sega_aos_tex;
        int num = 0;
        while (num < 2)
        {
            AppMain.AoTexRelease((AppMain.AOS_TEXTURE)dmLogoSegaAosTex);
            ++num;
            ++dmLogoSegaAosTex;
        }
        for (int index = 0; index < 2; ++index)
            AppMain.dm_logo_sega_efct_mdl_state[index] = AppMain.ObjAction3dESModelReleaseDwork(AppMain.dm_logo_sega_efct_mdl_data_work[index]);
    }

    private bool DmLogoSegaFlushCheck()
    {
        return !AppMain.dm_logo_sega_build_state;
    }

    private void DmLogoSegaRelease()
    {
        for (int index = 0; index < 5; ++index)
        {
            AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.dm_logo_sega_data[index];
            AppMain.dm_logo_sega_data[index] = (AppMain.AMS_AMB_HEADER)null;
        }
    }

    private bool DmLogoSegaReleaseCheck()
    {
        return AppMain.dm_logo_sega_load_tcb.Target == null && AppMain.dm_logo_sega_data[0] == null;
    }

    private void dmLogoSegaStart()
    {
        AppMain.NNS_RGBA diffuse = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_RGB ambient = new AppMain.NNS_RGB(1f, 1f, 1f);
        this.dmLogoSegaObjSysytemInit();
        AppMain.GsSoundReset();
        AppMain.GsSoundBegin((ushort)3, (uint)short.MaxValue, 0);
        AppMain.DMS_LOGO_SEGA_WORK work = (AppMain.DMS_LOGO_SEGA_WORK)AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSegaMainFunc), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_LOGO_SEGA_WORK()), "DM_LSEGA_MAIN").work;
        AppMain.nnSetPrimitive3DMaterial(ref diffuse, ref ambient, 1f);
        AppMain.AoActSysSetDrawStateEnable(false);
        this.dmLogoSegaActionCreate(work);
        work.ply_obj = this.dmLogoSegaCreatePlayer();
        work.timer = 0;
        work.func = new AppMain.DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaStartWaitFunc);
        work.h_se = AppMain.GsSoundAllocSeHandle();
    }

    private void dmLogoSegaObjSysytemInit()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR pos = new AppMain.NNS_VECTOR(0.0f, 0.0f, 50f);
        AppMain.ObjInit((byte)4, (ushort)61435, (byte)0, AppMain.GMD_OBJ_LCD_X, AppMain.GMD_OBJ_LCD_Y, (float)AppMain.GSD_DISP_WIDTH, (float)AppMain.GSD_DISP_HEIGHT);
        AppMain.ObjDataAlloc(10);
        AppMain.ObjDrawESEffectSystemInit((ushort)0, 20480U, 0U);
        AppMain.ObjDrawSetNNCommandStateTbl(0U, 0U, true);
        AppMain.AoActSysClearPeak();
        AppMain.g_obj.flag = 4259848U;
        AppMain.g_obj.ppPre = (AppMain.OBJECT_Delegate)null;
        AppMain.g_obj.ppPost = (AppMain.OBJECT_Delegate)null;
        AppMain.g_obj.ppCollision = (AppMain.OBJECT_WORK_Delegate)null;
        AppMain.g_obj.ppObjPre = (AppMain.OBJECT_WORK_Delegate)null;
        AppMain.g_obj.ppObjPost = (AppMain.OBJECT_WORK_Delegate)null;
        AppMain.g_obj.ppRegRecAuto = (AppMain.OBJECT_WORK_Delegate)null;
        AppMain.g_obj.draw_scale.x = AppMain.g_obj.draw_scale.y = AppMain.g_obj.draw_scale.z = 29127;
        AppMain.g_obj.inv_draw_scale.x = AppMain.g_obj.inv_draw_scale.y = AppMain.g_obj.inv_draw_scale.z = AppMain.FX_Div(4096, AppMain.g_obj.draw_scale.x);
        AppMain.g_obj.depth = 128;
        AppMain.g_obj.ambient_color.r = 0.8f;
        AppMain.g_obj.ambient_color.g = 0.8f;
        AppMain.g_obj.ambient_color.b = 0.8f;
        nnsVector.x = -1f;
        nnsVector.y = -1f;
        nnsVector.z = -1f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_0, ref col, 1f, nnsVector);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        AppMain.ObjCameraInit(0, pos, 0, (ushort)0, 6144);
        AppMain.ObjCamera3dInit(0);
        AppMain.g_obj.glb_camera_id = 0;
        AppMain.g_obj.glb_camera_type = 1;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        obsCamera.user_func = new AppMain.OBJF_CAMERA_USER_FUNC(this.dmLogoSegaCamera);
        obsCamera.command_state = 0U;
        obsCamera.scale = 0.9f;
        obsCamera.ofst.z = 1000f;
        this.amTrailEFInitialize();
    }

    private void dmLogoSegaPreEnd(AppMain.DMS_LOGO_SEGA_WORK logo_work)
    {
        AppMain.amTrailEFDeleteGroup((ushort)1);
        for (AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, ushort.MaxValue); obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
            obj_work.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private void dmLogoSegaEnd(AppMain.DMS_LOGO_SEGA_WORK logo_work)
    {
        this.dmLogoSegaActionDelete(logo_work);
        AppMain.g_obj.ppPre = (AppMain.OBJECT_Delegate)null;
        AppMain.ObjObjectClearAllObject();
        AppMain.ObjPreExit();
        AppMain.ObjDrawESEffectSystemExit();
        AppMain.ObjExit();
        AppMain.GsSoundStopSeHandle(logo_work.h_se);
        AppMain.GsSoundFreeSeHandle(logo_work.h_se);
        AppMain.GsSoundHalt();
        AppMain.GsSoundEnd();
        AppMain.GsSoundReset();
    }

    private void dmLogoSegaMainFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_LOGO_SEGA_WORK work = (AppMain.DMS_LOGO_SEGA_WORK)tcb.work;
        work.h_se.snd_ctrl_param.volume = !AppMain.GsSystemBgmIsPlay() ? 1f : 0.0f;
        if (AppMain.AoSysIsShowPlatformUI())
        {
            if (AppMain.IzFadeIsExe())
                AppMain.IzFadeSetStopUpdate1Frame((AppMain.IZS_FADE_WORK)null);
        }
        else
        {
            if (work.func != null)
                work.func(work);
            if (((int)work.flag & 1) != 0)
            {
                this.dmLogoSegaPreEnd(work);
                AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.gmLogoSegaPreEndWaitFunc));
                work.timer = 0;
                return;
            }
        }
        float frame = 0.0f;
        if (!AppMain.AoSysIsShowPlatformUI() && ((int)work.flag & 2) != 0)
            frame = 1f;
        AppMain.AoActSysSetDrawTaskPrio();
        for (int index = 0; index < 8; ++index)
        {
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_logo_sega_aos_tex[(int)AppMain.dm_logo_sega_tex_id_tbl[index]]));
            AppMain.AoActUpdate(work.act[index], frame);
            AppMain.AoActDraw(work.act[index]);
        }
        AppMain.NNS_RGBA diffuse = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_RGB ambient = new AppMain.NNS_RGB(1f, 1f, 1f);
        if (!AppMain.AoSysIsShowPlatformUI())
            AppMain.amTrailEFUpdate((ushort)1);
        if (AppMain.g_obj.glb_camera_id != -1)
        {
            AppMain.SNNS_VECTOR disp_pos = new AppMain.SNNS_VECTOR();
            AppMain.SNNS_VECTOR snnsVector = new AppMain.SNNS_VECTOR();
            AppMain.SNNS_MATRIX dst = new AppMain.SNNS_MATRIX();
            AppMain.nnMakeUnitMatrix(ref dst);
            AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, AppMain.g_obj.glb_camera_type, 0U);
            AppMain.ObjCameraDispPosGet(AppMain.g_obj.glb_camera_id, out disp_pos);
            AppMain.amVectorSet(ref snnsVector, -dst.M03, -dst.M13, -dst.M23);
            AppMain.nnAddVector(ref disp_pos, ref snnsVector, ref disp_pos);
            AppMain.amEffectSetCameraPos(ref disp_pos);
        }
        AppMain.nnSetPrimitive3DMaterial(ref diffuse, ref ambient, 1f);
        AppMain.amTrailEFDraw((ushort)1, AppMain.dm_logo_sega_aos_tex[0].texlist, 0U);
    }

    private void gmLogoSegaPreEndWaitFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_LOGO_SEGA_WORK work = (AppMain.DMS_LOGO_SEGA_WORK)tcb.work;
        ++work.timer;
        if (work.timer <= 2)
            return;
        this.dmLogoSegaEnd(work);
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.gmLogoSegaEndWaitFunc));
    }

    private void gmLogoSegaEndWaitFunc(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.ObjObjectCheckClearAllObject() || AppMain.ObjIsExitWait())
            return;
        this.DmLogoSegaFlush();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.gmLogoSegaFlushWaitFunc));
    }

    private void gmLogoSegaFlushWaitFunc(AppMain.MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSegaFlushCheck())
            return;
        this.DmLogoSegaRelease();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.gmLogoSegaRelesehWaitFunc));
    }

    private void gmLogoSegaRelesehWaitFunc(AppMain.MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSegaReleaseCheck())
            return;
        AppMain.mtTaskClearTcb(tcb);
        AppMain.SyChangeNextEvt();
    }

    public static bool mppSountInitializationDelayExpired()
    {
        return true;
    }

    private void dmLogoSegaStartWaitFunc(AppMain.DMS_LOGO_SEGA_WORK logo_work)
    {
        bool flag = true;
        ++logo_work.timer;
        if (!AppMain.mppSountInitializationDelayExpired())
            flag = false;
        if (logo_work.timer < 0 || !flag)
            return;
        AppMain.IzFadeExit();
        logo_work.timer = 4;
        logo_work.func = new AppMain.DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaRunLeftFunc);
        this.dmLogoSegaPlayerInitSeqRunLeft(logo_work.ply_obj);
        logo_work.flag |= 2U;
        logo_work.efct_obj = this.dmLogoSegaCreateDashEffect(logo_work.ply_obj, 0);
    }

    private void dmLogoSegaRunLeftFunc(AppMain.DMS_LOGO_SEGA_WORK logo_work)
    {
        ++logo_work.timer;
        if (logo_work.timer <= 10)
            return;
        logo_work.timer = 0;
        logo_work.func = new AppMain.DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaRunRightWaitFunc);
        logo_work.efct_obj.flag |= 8U;
        logo_work.efct_obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private void dmLogoSegaRunRightWaitFunc(AppMain.DMS_LOGO_SEGA_WORK logo_work)
    {
        ++logo_work.timer;
        if (logo_work.timer <= 25)
            return;
        logo_work.timer = 0;
        logo_work.func = new AppMain.DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaRunRightFunc);
        this.dmLogoSegaPlayerInitSeqRunRight(logo_work.ply_obj);
        logo_work.efct_obj = this.dmLogoSegaCreateDashEffect(logo_work.ply_obj, 1);
    }

    private void dmLogoSegaRunRightFunc(AppMain.DMS_LOGO_SEGA_WORK logo_work)
    {
        ++logo_work.timer;
        if (logo_work.timer <= 10)
            return;
        logo_work.timer = 0;
        logo_work.func = new AppMain.DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaDispWaitFunc);
        AppMain.GsSoundPlaySe("Sega_Logo", logo_work.h_se);
        if (!AppMain.GsSystemBgmIsPlay())
            return;
        logo_work.h_se.snd_ctrl_param.volume = 0.0f;
    }

    private void dmLogoSegaDispWaitFunc(AppMain.DMS_LOGO_SEGA_WORK logo_work)
    {
        ++logo_work.timer;
        if (logo_work.timer < 180)
            return;
        logo_work.func = new AppMain.DMS_LOGO_SEGA_WORK_Delegate(this.dmLogoSegaFadeOutWaitFunc);
        AppMain.IzFadeInitEasy(0U, 3U, 60f, true);
    }

    private void dmLogoSegaFadeOutWaitFunc(AppMain.DMS_LOGO_SEGA_WORK logo_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        logo_work.flag |= 1U;
    }

    private AppMain.OBS_OBJECT_WORK dmLogoSegaCreatePlayer()
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)8192, (byte)0, (byte)0, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_LOGO_SEGA_OBJ_3DNN_WORK()), "DM_LSEGA_PLY");
        AppMain.DMS_LOGO_SEGA_OBJ_3DNN_WORK logoSegaObj3DnnWork = (AppMain.DMS_LOGO_SEGA_OBJ_3DNN_WORK)obj_work;
        obj_work.obj_type = (ushort)1;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.ObjDrawActionSummary);
        obj_work.ppOutSub = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppIn = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppActCall = (AppMain.OBS_OBJECT_WORK_Delegate2)null;
        obj_work.ppRec = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppLast = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.OBS_ACTION3D_NN_WORK obj3d = logoSegaObj3DnnWork.obj_3d;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, this.dm_logo_sega_obj_3d_list[0], obj3d);
        AppMain.ObjDrawSetToon(obj3d);
        obj3d.command_state = 0U;
        AppMain.ObjDataSet(logoSegaObj3DnnWork.data_work, (object)AppMain.dm_logo_sega_data[2]);
        logoSegaObj3DnnWork.data_work.num |= (ushort)32768;
        AppMain.ObjObjectAction3dNNMotionLoad(obj_work, 0, false, logoSegaObj3DnnWork.data_work, (string)null, 0, (AppMain.AMS_AMB_HEADER)null, 136, 16);
        obj_work.disp_flag |= 16777728U;
        obj_work.scale.x = obj_work.scale.y = obj_work.scale.z = 8192;
        AppMain.nnMakeUnitMatrix(obj3d.user_obj_mtx_r);
        AppMain.nnTranslateMatrix(obj3d.user_obj_mtx_r, obj3d.user_obj_mtx_r, 0.0f, -36f / AppMain.FXM_FX32_TO_FLOAT(AppMain.g_obj.draw_scale.y), 0.0f);
        obj_work.pos.x = 2490368;
        obj_work.pos.y = 0;
        obj_work.pos.z = 0;
        return obj_work;
    }

    private void dmLogoSegaPlayerInitSeqRunLeft(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x = 2490368;
        AppMain.ObjDrawObjectActionSet(obj_work, 77);
        obj_work.disp_flag |= 5U;
        this.dmLogoSegaCreateTrail(obj_work);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(this.dmLogoSegaPlayerSeqRunLeft);
    }

    private void dmLogoSegaPlayerSeqRunLeft(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.AoSysIsShowPlatformUI())
        {
            obj_work.disp_flag |= 4096U;
        }
        else
        {
            obj_work.disp_flag &= 4294963199U;
            obj_work.pos.x += -495616;
        }
    }

    private void dmLogoSegaPlayerInitSeqRunRight(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x = -2490368;
        AppMain.ObjDrawObjectActionSet(obj_work, 9);
        obj_work.disp_flag &= 4294967294U;
        obj_work.disp_flag |= 4U;
        this.dmLogoSegaCreateTrail(obj_work);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(this.dmLogoSegaPlayerSeqRunRight);
    }

    private void dmLogoSegaPlayerSeqRunRight(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.AoSysIsShowPlatformUI())
        {
            obj_work.disp_flag |= 4096U;
        }
        else
        {
            obj_work.disp_flag &= 4294963199U;
            obj_work.pos.x += 495616;
        }
    }

    private AppMain.OBS_OBJECT_WORK dmLogoSegaCreateDashEffect(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int type)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)12288, (byte)0, (byte)0, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_LOGO_SEGA_OBJ_ES_WORK()), "DM_LSEGA_EFCT");
        AppMain.DMS_LOGO_SEGA_OBJ_ES_WORK logoSegaObjEsWork = (AppMain.DMS_LOGO_SEGA_OBJ_ES_WORK)obj_work;
        obj_work.obj_type = (ushort)2;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.ObjDrawActionSummary);
        obj_work.ppOutSub = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppIn = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppActCall = (AppMain.OBS_OBJECT_WORK_Delegate2)null;
        obj_work.ppRec = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppLast = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(this.dmLogoSegaEffectMain);
        obj_work.parent_obj = parent_obj;
        obj_work.pos.Assign(parent_obj.pos);
        int index = type != 1 ? 52 : 53;
        AppMain.ObjObjectAction3dESEffectLoad(obj_work, logoSegaObjEsWork.obj_3des, (AppMain.OBS_DATA_WORK)null, (string)null, index, AppMain.dm_logo_sega_data[3], 0, 0);
        logoSegaObjEsWork.obj_3des.command_state = 0U;
        AppMain.ObjDataSet(logoSegaObjEsWork.data_work_texamb, AppMain.amBindGet(AppMain.dm_logo_sega_data[3], 97));
        logoSegaObjEsWork.data_work_texamb.num |= (ushort)32768;
        AppMain.ObjDataSet(logoSegaObjEsWork.data_work_texlist, (object)AppMain.dm_logo_sega_aos_tex[1].texlist);
        logoSegaObjEsWork.data_work_texlist.num |= (ushort)32768;
        AppMain.ObjObjectAction3dESTextureLoad(obj_work, logoSegaObjEsWork.obj_3des, logoSegaObjEsWork.data_work_texamb, (string)null, 0, (AppMain.AMS_AMB_HEADER)null, false);
        AppMain.ObjObjectAction3dESTextureSetByDwork(obj_work, logoSegaObjEsWork.data_work_texlist);
        AppMain.ObjDataSet(logoSegaObjEsWork.data_work_model, AppMain.amBindGet(AppMain.dm_logo_sega_data[3], AppMain.dm_logo_sega_efct_mdl_id_tbl[type]));
        logoSegaObjEsWork.data_work_texlist.num |= (ushort)32768;
        AppMain.ObjObjectAction3dESModelLoad(obj_work, logoSegaObjEsWork.obj_3des, logoSegaObjEsWork.data_work_model, (string)null, 0, (AppMain.AMS_AMB_HEADER)null, 0U, false);
        AppMain.ObjObjectAction3dESModelSetByDwork(obj_work, AppMain.dm_logo_sega_efct_mdl_data_work[type]);
        logoSegaObjEsWork.obj_3des.disp_rot.x = (ushort)0;
        logoSegaObjEsWork.obj_3des.disp_rot.y = (ushort)0;
        logoSegaObjEsWork.obj_3des.disp_rot.z = (ushort)0;
        obj_work.scale.x = AppMain.FX_Mul(parent_obj.scale.x, AppMain.g_obj.draw_scale.x);
        obj_work.scale.y = AppMain.FX_Mul(parent_obj.scale.y, AppMain.g_obj.draw_scale.y);
        obj_work.scale.z = AppMain.FX_Mul(parent_obj.scale.z, AppMain.g_obj.draw_scale.z);
        obj_work.disp_flag |= 4194304U;
        logoSegaObjEsWork.obj_3des.flag |= 8U;
        return obj_work;
    }

    private void dmLogoSegaEffectMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        if (AppMain.AoSysIsShowPlatformUI())
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

    private void dmLogoSegaCreateTrail(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.NNS_TEXLIST texlist = AppMain.dm_logo_sega_aos_tex[0].texlist;
        AppMain.amTrailMakeEffect(new AppMain.AMS_TRAIL_PARAM()
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
            partsNum = (short)63,
            zBias = -65536f,
            texId = texlist.nTex - 1,
            blendType = (short)0,
            zTest = (short)1
        }, (ushort)1, (short)1);
    }

    private void dmLogoSegaCamera(AppMain.OBS_CAMERA camera)
    {
        camera.disp_pos.x = 0.0f;
        camera.disp_pos.y = 0.0f;
        camera.disp_pos.z = 200f;
        camera.target_pos.x = 0.0f;
        camera.target_pos.y = 0.0f;
        camera.target_pos.z = 0.0f;
    }

    private static void dmLogoSegaLoadPostFunc(AppMain.DMS_LOGO_COM_LOAD_CONTEXT context)
    {
        AppMain.dm_logo_sega_data[context.no] = AppMain.readAMBFile(context.fs_req);
    }

    private void dmLogoSegaDataBuildMain(AppMain.MTS_TASK_TCB tcb)
    {
        bool flag = true;
        if (!AppMain.GmGameDBuildCheckBuildModel())
            flag = false;
        AppMain.ArrayPointer<AppMain.AOS_TEXTURE> dmLogoSegaAosTex = (AppMain.ArrayPointer<AppMain.AOS_TEXTURE>)AppMain.dm_logo_sega_aos_tex;
        int num = 0;
        while (num < 2)
        {
            if (!AppMain.AoTexIsLoaded((AppMain.AOS_TEXTURE)dmLogoSegaAosTex))
                flag = false;
            ++num;
            ++dmLogoSegaAosTex;
        }
        for (int index = 0; index < 2; ++index)
        {
            if (AppMain.dm_logo_sega_efct_mdl_state[index] != -1 && !AppMain.amDrawIsRegistComplete(AppMain.dm_logo_sega_efct_mdl_state[index]))
            {
                flag = false;
                AppMain.dm_logo_sega_efct_mdl_state[index] = -1;
            }
        }
        if (!flag)
            return;
        AppMain.mtTaskClearTcb(tcb);
        AppMain.dm_logo_sega_build_state = true;
    }

    private void dmLogoSegaDataBuildDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.dm_logo_sega_build_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private void dmLogoSegaDataFlushMain(AppMain.MTS_TASK_TCB tcb)
    {
        bool flag = true;
        if (!AppMain.GmGameDBuildCheckFlushModel())
            flag = false;
        AppMain.ArrayPointer<AppMain.AOS_TEXTURE> dmLogoSegaAosTex = (AppMain.ArrayPointer<AppMain.AOS_TEXTURE>)AppMain.dm_logo_sega_aos_tex;
        int num = 0;
        while (num < 2)
        {
            if (!AppMain.AoTexIsReleased((AppMain.AOS_TEXTURE)dmLogoSegaAosTex))
                flag = false;
            ++num;
            ++dmLogoSegaAosTex;
        }
        for (int index = 0; index < 2; ++index)
        {
            if (AppMain.dm_logo_sega_efct_mdl_state[index] != -1)
            {
                if (AppMain.ObjAction3dESModelReleaseDworkCheck(AppMain.dm_logo_sega_efct_mdl_data_work[index], AppMain.dm_logo_sega_efct_mdl_state[index]))
                    AppMain.dm_logo_sega_efct_mdl_state[index] = -1;
                else
                    flag = false;
            }
        }
        if (!flag)
            return;
        AppMain.dm_logo_sega_aos_tex = (AppMain.AOS_TEXTURE[])null;
        this.dm_logo_sega_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        AppMain.mtTaskClearTcb(tcb);
        AppMain.dm_logo_sega_build_state = false;
    }

    private void dmLogoSegaDataFlushDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.dm_logo_sega_flush_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private void dmLogoSegaLoadWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSegaLoadCheck())
            return;
        this.DmLogoSegaBuild();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.dmLogoSegaBuildWait));
    }

    private void dmLogoSegaBuildWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!this.DmLogoSegaBuildCheck())
            return;
        AppMain.mtTaskClearTcb(tcb);
        this.dmLogoSegaStart();
    }

    private void dmLogoSegaActionCreate(AppMain.DMS_LOGO_SEGA_WORK logo_work)
    {
        AppMain.A2S_AMA_HEADER ama = AppMain.readAMAFile(AppMain.amBindGet(AppMain.dm_logo_sega_data[4], 0));
        for (uint id = 0; id < 8U; ++id)
        {
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_logo_sega_aos_tex[(int)AppMain.dm_logo_sega_tex_id_tbl[(int)id]]));
            logo_work.act[(int)id] = AppMain.AoActCreate(ama, id);
        }
    }

    private void dmLogoSegaActionDelete(AppMain.DMS_LOGO_SEGA_WORK logo_work)
    {
        for (int index = 0; index < 8; ++index)
            AppMain.AoActDelete(logo_work.act[index]);
    }

}