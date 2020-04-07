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

    public AppMain.DMS_TITLEOP_OBJ_3DNN_WORK DMM_TITLEOP_CREATE_3D_OBJ(
      ushort prio,
      byte group,
      AppMain.TaskWorkFactoryDelegate work_size,
      string name)
    {
        return this.dmTitleOpCreate3DObj(prio, group, work_size);
    }

    private void DmTitleOpLoad()
    {
        this.DmLogoComLoadFileCreate(AppMain.dm_titleop_load_tcb);
        this.DmLogoComLoadFileReg(AppMain.dm_titleop_load_tcb, this.dm_titleop_com_fileinfo_list, 2);
        this.DmLogoComLoadFileStart(AppMain.dm_titleop_load_tcb);
    }

    private bool DmTitleOpLoadCheck()
    {
        return AppMain.dm_titleop_load_tcb.Target == null && AppMain.dm_titleop_data[0] != null;
    }

    private void DmTitleOpBuild()
    {
        AppMain.AMS_AMB_HEADER[] amsAmbHeaderArray = new AppMain.AMS_AMB_HEADER[1];
        AppMain.dm_titleop_build_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmTitleOpDataBuildMain), new AppMain.GSF_TASK_PROCEDURE(this.dmTitleOpDataBuildDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "DM_TOP_BUILD");
        AppMain.dm_titleop_aos_tex = AppMain.New<AppMain.AOS_TEXTURE>(1);
        string sPath = (string)null;
        amsAmbHeaderArray[0] = AppMain.readAMBFile(AppMain.amBindGet(AppMain.dm_titleop_data[1], 1, out sPath));
        amsAmbHeaderArray[0].dir = sPath;
        AppMain.AOS_TEXTURE[] dmTitleopAosTex = AppMain.dm_titleop_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            AppMain.AoTexBuild(dmTitleopAosTex[index], amsAmbHeaderArray[index]);
            AppMain.AoTexLoad(dmTitleopAosTex[index]);
        }
        AppMain.ObjInit((byte)4, (ushort)61435, (byte)0, AppMain.GMD_OBJ_LCD_X, AppMain.GMD_OBJ_LCD_Y, (float)AppMain.GSD_DISP_WIDTH, (float)AppMain.GSD_DISP_HEIGHT);
        AppMain.ObjDataAlloc(10);
        AppMain.ObjDrawSetNNCommandStateTbl(0U, 0U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(1U, 1U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(2U, 2U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(3U, 3U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(4U, 4U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(5U, 5U, true);
        AppMain.AoActSysClearPeak();
        AppMain.GmGameDBuildModelBuildInit();
        this.dm_titleop_obj_3d_list = AppMain.GmGameDBuildRegBuildModel((AppMain.AMS_AMB_HEADER)AppMain.dm_titleop_mapfar_data[0], (AppMain.AMS_AMB_HEADER)AppMain.dm_titleop_mapfar_data[1], 0U);
        this.dm_titleop_water_obj_3d_list = AppMain.GmGameDBuildRegBuildModel((AppMain.AMS_AMB_HEADER)AppMain.dm_titleop_mapfar_data[3], (AppMain.AMS_AMB_HEADER)AppMain.dm_titleop_mapfar_data[1], 0U);
    }

    private bool DmTitleOpBuildCheck()
    {
        return AppMain.dm_titleop_build_state;
    }

    private void DmTitleOpFlush()
    {
        AppMain.dm_titleop_flush_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmTitleOpDataFlushMain), new AppMain.GSF_TASK_PROCEDURE(this.dmTitleOpDataFlushDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "DM_TOP_FLUSH");
        AppMain.AOS_TEXTURE[] dmTitleopAosTex = AppMain.dm_titleop_aos_tex;
        for (int index = 0; index < 1; ++index)
            AppMain.AoTexRelease(dmTitleopAosTex[index]);
        AppMain.GmGameDBuildModelFlushInit();
        AppMain.GmGameDBuildRegFlushModel(this.dm_titleop_obj_3d_list, ((AppMain.AMS_AMB_HEADER)AppMain.dm_titleop_mapfar_data[0]).file_num);
        AppMain.GmGameDBuildRegFlushModel(this.dm_titleop_water_obj_3d_list, ((AppMain.AMS_AMB_HEADER)AppMain.dm_titleop_mapfar_data[3]).file_num);
    }

    private bool DmTitleOpFlushCheck()
    {
        return !AppMain.dm_titleop_build_state;
    }

    private void DmTitleOpRelease()
    {
        for (int index = 0; index < 2; ++index)
            AppMain.dm_titleop_data[index] = (AppMain.AMS_FS)null;
        for (int index = 0; index < 4; ++index)
            AppMain.dm_titleop_mapfar_data[index] = (object)null;
    }

    private bool DmTitleOpReleaseCheck()
    {
        return AppMain.dm_titleop_load_tcb.Target == null && AppMain.dm_titleop_data[0] == null;
    }

    private void DmTitleOpInit()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.Clear();
        AppMain.NNS_VECTOR pos = new AppMain.NNS_VECTOR(0.0f, 0.0f, 50f);
        AppMain.g_obj.flag = 4259848U;
        AppMain.g_obj.ppPre = (AppMain.OBJECT_Delegate)null;
        AppMain.g_obj.ppPost = (AppMain.OBJECT_Delegate)null;
        AppMain.g_obj.ppCollision = (AppMain.OBJECT_WORK_Delegate)null;
        AppMain.g_obj.ppObjPre = (AppMain.OBJECT_WORK_Delegate)null;
        AppMain.g_obj.ppObjPost = (AppMain.OBJECT_WORK_Delegate)null;
        AppMain.g_obj.ppRegRecAuto = (AppMain.OBJECT_WORK_Delegate)null;
        AppMain.g_obj.draw_scale.x = AppMain.g_obj.draw_scale.y = AppMain.g_obj.draw_scale.z = 13107;
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
        AppMain.ObjCameraInit(0, pos, 0, (ushort)0, 12288);
        AppMain.ObjCamera3dInit(0);
        AppMain.g_obj.glb_camera_id = 0;
        AppMain.g_obj.glb_camera_type = 0;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        obsCamera.user_func = new AppMain.OBJF_CAMERA_USER_FUNC(this.dmTitleOpCamera);
        obsCamera.command_state = 0U;
        obsCamera.fovy = AppMain.NNM_DEGtoA32(40f);
        obsCamera.znear = 0.1f;
        obsCamera.zfar = 32768f;
        this.dmTitleOpMgrInit();
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    private void DmTitleOpExit()
    {
        if (AppMain.dm_titleop_mgr_tcb == null)
            return;
        ((AppMain.DMS_TITLEOP_MGR_WORK)AppMain.dm_titleop_mgr_tcb.work).flag |= 536870912U;
    }

    private bool DmTitleOpExitEndCheck()
    {
        return AppMain.dm_titleop_mgr_tcb == null;
    }

    private void DmTitleOpDraw2D()
    {
        if (AppMain.dm_titleop_mgr_tcb == null)
            return;
        AppMain.DMS_TITLEOP_MGR_WORK work = (AppMain.DMS_TITLEOP_MGR_WORK)AppMain.dm_titleop_mgr_tcb.work;
        if (((int)work.flag & int.MinValue) == 0)
            return;
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_titleop_aos_tex[0]));
        int num = 5;
        for (int index = 0; index < num; ++index)
        {
            if (((int)work.flag & 1 << index) != 0 && ((int)work.flag & 256 << index) == 0)
            {
                AppMain.AoActAcmPush();
                AppMain.AoActAcmInit();
                switch (index)
                {
                    case 0:
                        AppMain.AoActAcmApplyTrans(0.0f, -25f, 0.0f);
                        goto default;
                    case 1:
                        AppMain.AoActSetFrame(work.act[index], work.finger_frame);
                        AppMain.AoActAcmApplyTrans(-10f, -15f, 0.0f);
                        AppMain.AoActAcmApplyScale(0.9f, 0.9f);
                        AppMain.AoActUpdate(work.act[index], 0.0f);
                        ++work.finger_frame;
                        if ((double)work.finger_frame > 84.0)
                        {
                            work.finger_frame = 60f;
                            break;
                        }
                        break;
                    case 3:
                        AppMain.AoActAcmApplyTrans(0.0f, -10f, 0.0f);
                        goto default;
                    default:
                        AppMain.AoActAcmApplyScale(0.9f, 0.9f);
                        AppMain.AoActUpdate(work.act[index]);
                        break;
                }
                AppMain.AoActSortRegAction(work.act[index]);
                AppMain.AoActAcmPop(1U);
            }
        }
    }

    private bool DmTitleOpIsLogoActFinish()
    {
        return AppMain.dm_titleop_mgr_tcb != null && ((int)((AppMain.DMS_TITLEOP_MGR_WORK)AppMain.dm_titleop_mgr_tcb.work).flag & 1073741824) != 0;
    }

    private void DmTitleOpDispRightEnable(bool disp)
    {
        if (AppMain.dm_titleop_mgr_tcb == null)
            return;
        AppMain.DMS_TITLEOP_MGR_WORK work = (AppMain.DMS_TITLEOP_MGR_WORK)AppMain.dm_titleop_mgr_tcb.work;
        if (disp)
            work.flag &= 4294963199U;
        else
            work.flag |= 4096U;
    }

    private void DmTitleOpSetRetOptionState()
    {
        if (AppMain.dm_titleop_mgr_tcb == null)
            return;
        AppMain.DMS_TITLEOP_MGR_WORK work = (AppMain.DMS_TITLEOP_MGR_WORK)AppMain.dm_titleop_mgr_tcb.work;
        work.flag |= 1073741951U;
        work.frame = 105;
        work.finger_frame = 75f;
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_titleop_aos_tex[0]));
        int num = 5;
        for (int index = 0; index < num; ++index)
        {
            AppMain.AoActAcmPush();
            AppMain.AoActAcmInit();
            AppMain.AoActSetFrame(work.act[index], 75f);
            AppMain.AoActUpdate(work.act[index], 0.0f);
            AppMain.AoActAcmPop(1U);
        }
    }

    private static void dmTitleOpLoadPostFuncMapFar(AppMain.DMS_LOGO_COM_LOAD_CONTEXT context)
    {
        AppMain.dm_titleop_data[context.no] = context.fs_req;
        AppMain.AMS_AMB_HEADER header = AppMain.readAMBFile(AppMain.dm_titleop_data[context.no]);
        for (int index = 0; index < header.file_num; ++index)
        {
            string sPath;
            AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.amBindGet(header, index, out sPath));
            amsAmbHeader.dir = sPath;
            AppMain.dm_titleop_mapfar_data[index] = (object)amsAmbHeader;
        }
    }

    private static void dmTitleOpLoadPostFuncTitleLogo(AppMain.DMS_LOGO_COM_LOAD_CONTEXT context)
    {
        AppMain.dm_titleop_data[context.no] = context.fs_req;
        context.fs_req = (AppMain.AMS_FS)null;
    }

    private void dmTitleOpDataBuildMain(AppMain.MTS_TASK_TCB tcb)
    {
        bool flag = true;
        AppMain.AOS_TEXTURE[] dmTitleopAosTex = AppMain.dm_titleop_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            if (!AppMain.AoTexIsLoaded(dmTitleopAosTex[index]))
                flag = false;
        }
        if (!AppMain.GmGameDBuildCheckBuildModel())
            flag = false;
        if (!flag)
            return;
        AppMain.mtTaskClearTcb(tcb);
        AppMain.dm_titleop_build_state = true;
    }

    private void dmTitleOpDataBuildDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.dm_titleop_build_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private void dmTitleOpDataFlushMain(AppMain.MTS_TASK_TCB tcb)
    {
        bool flag = true;
        AppMain.AOS_TEXTURE[] dmTitleopAosTex = AppMain.dm_titleop_aos_tex;
        for (int index = 0; index < 1; ++index)
        {
            if (!AppMain.AoTexIsReleased(dmTitleopAosTex[index]))
                flag = false;
        }
        if (!AppMain.GmGameDBuildCheckFlushModel())
            flag = false;
        if (!flag)
            return;
        for (int index = 0; index < AppMain.dm_titleop_aos_tex.Length; ++index)
            AppMain.dm_titleop_aos_tex[index] = (AppMain.AOS_TEXTURE)null;
        AppMain.dm_titleop_aos_tex = (AppMain.AOS_TEXTURE[])null;
        this.dm_titleop_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        this.dm_titleop_water_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        AppMain.mtTaskClearTcb(tcb);
        AppMain.dm_titleop_build_state = false;
    }

    private void dmTitleOpDataFlushDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.dm_titleop_flush_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private void dmTitleOpMgrInit()
    {
        AppMain.dm_titleop_mgr_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmTitleOpMgrMain), new AppMain.GSF_TASK_PROCEDURE(this.dmTitleOpMgrDest), 0U, ushort.MaxValue, 12288U, 0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_TITLEOP_MGR_WORK()), "DM_TOP_MGR");
        AppMain.DMS_TITLEOP_MGR_WORK work = (AppMain.DMS_TITLEOP_MGR_WORK)AppMain.dm_titleop_mgr_tcb.work;
        work.Clear();
        this.dmTitleOpCreateObjFarSky();
        this.dmTitleOpCreateObjFarRock(0U);
        this.dmTitleOpCreateObjFarRock(1U);
        this.dmTitleOpCreateObjFarRock(2U);
        this.dmTitleOpCreateObjFarSea();
        this.dmTitleOpCreateAction(work);
        work.flag |= 268435456U;
        AppMain.dm_titleop_scrl_x_ofst = 0;
    }

    private void dmTitleOpMgrDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.dm_titleop_mgr_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private void dmTitleOpMgrMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_TITLEOP_MGR_WORK work = (AppMain.DMS_TITLEOP_MGR_WORK)tcb.work;
        if (((int)work.flag & 536870912) != 0)
        {
            this.dmTitleOpEndStart(tcb);
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
                AppMain.dm_titleop_scrl_x_ofst += (int)sbyte.MinValue;
                int num = AppMain.dm_titleop_scrl_x_ofst / 3145728;
                AppMain.dm_titleop_scrl_x_ofst -= num * 3145728;
            }
            AppMain.ObjDraw3DNNUserFunc(AppMain._dmTitleOpPreDrawDT, (object)null, 0, 0U);
            AppMain.ObjDraw3DNNUserFunc(AppMain._dmTitleOpFallShaderPreRenderUserFunc, (object)null, 0, 2U);
            AppMain.amDrawSetFog(0U, 1);
            AppMain.amDrawSetFogColor(0U, 0.7f, 0.95f, 1f);
            AppMain.amDrawSetFogRange(0U, 1f, 500f);
            AppMain.amDrawSetFog(4U, 0);
        }
    }

    private void dmTitleOpCreateObjFarSky()
    {
        AppMain.DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork1 = this.DMM_TITLEOP_CREATE_3D_OBJ((ushort)24576, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_TITLEOP_OBJ_3DNN_WORK()), "DM_TOP_SKYT");
        AppMain.OBS_OBJECT_WORK obj_work1 = (AppMain.OBS_OBJECT_WORK)titleopObj3DnnWork1;
        obj_work1.obj_type = (ushort)1;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work1, this.dm_titleop_obj_3d_list[0], titleopObj3DnnWork1.obj_3d);
        obj_work1.obj_3d.command_state = 3U;
        AppMain.ObjAction3dNNMaterialMotionLoad(titleopObj3DnnWork1.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, (AppMain.AMS_AMB_HEADER)AppMain.dm_titleop_mapfar_data[2]);
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work1, 0);
        obj_work1.disp_flag |= 13697028U;
        obj_work1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(this.dmTitleOpFarSkyFunc);
        AppMain.DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork2 = this.DMM_TITLEOP_CREATE_3D_OBJ((ushort)16384, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_TITLEOP_OBJ_3DNN_WORK()), "DM_TOP_SKYB");
        AppMain.OBS_OBJECT_WORK obj_work2 = (AppMain.OBS_OBJECT_WORK)titleopObj3DnnWork2;
        obj_work2.obj_type = (ushort)1;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work2, this.dm_titleop_obj_3d_list[0], titleopObj3DnnWork2.obj_3d);
        obj_work2.obj_3d.command_state = 1U;
        AppMain.ObjAction3dNNMaterialMotionLoad(titleopObj3DnnWork2.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, (AppMain.AMS_AMB_HEADER)AppMain.dm_titleop_mapfar_data[2]);
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work2, 0);
        obj_work2.disp_flag |= 13697028U;
        obj_work2.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(this.dmTitleOpFarSkyFunc);
    }

    private void dmTitleOpFarSkyFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork = (AppMain.DMS_TITLEOP_OBJ_3DNN_WORK)obj_work;
        titleopObj3DnnWork.sky_rot += 0.01f;
        if ((double)titleopObj3DnnWork.sky_rot > 360.0)
            titleopObj3DnnWork.sky_rot -= 360f;
        AppMain.nnMakeUnitMatrix(obj_work.obj_3d.user_obj_mtx);
        AppMain.nnRotateYMatrix(obj_work.obj_3d.user_obj_mtx, obj_work.obj_3d.user_obj_mtx, (int)(ushort)AppMain.NNM_DEGtoA16(titleopObj3DnnWork.sky_rot));
    }

    private void dmTitleOpCreateObjFarRock(uint type)
    {
        AppMain.DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork = this.DMM_TITLEOP_CREATE_3D_OBJ((ushort)24832, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_TITLEOP_OBJ_3DNN_WORK()), "DM_TOP_ROCKT");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)titleopObj3DnnWork;
        obj_work.obj_type = (ushort)1;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, this.dm_titleop_obj_3d_list[(int)(1U + type)], titleopObj3DnnWork.obj_3d);
        obj_work.obj_3d.command_state = 3U;
        obj_work.disp_flag |= 5242880U;
        obj_work.disp_flag |= 268435456U;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(this.dmTitleOpObjRockDraw);
        titleopObj3DnnWork.rock_setting = AppMain.dm_titleop_rock_setting[(int)type];
        titleopObj3DnnWork.rock_setting_num = AppMain.dm_titleop_rock_setting_num[(int)type];
    }

    private void dmTitleOpCreateObjFarSea()
    {
        AppMain.DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork = this.DMM_TITLEOP_CREATE_3D_OBJ((ushort)20480, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_TITLEOP_OBJ_3DNN_WORK()), "DM_TOP_SEA");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)titleopObj3DnnWork;
        obj_work.obj_type = (ushort)1;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, this.dm_titleop_water_obj_3d_list[0], titleopObj3DnnWork.obj_3d);
        obj_work.obj_3d.command_state = 2U;
        AppMain.ObjAction3dNNMaterialMotionLoad(titleopObj3DnnWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 1, (AppMain.AMS_AMB_HEADER)AppMain.dm_titleop_mapfar_data[2]);
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.obj_3d.mat_speed = 0.2f;
        obj_work.dir.y = (ushort)49152;
        obj_work.disp_flag |= 5308420U;
    }

    private AppMain.DMS_TITLEOP_OBJ_3DNN_WORK dmTitleOpCreate3DObj(
      ushort prio,
      byte group,
      AppMain.TaskWorkFactoryDelegate work_size)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.OBM_OBJECT_TASK_DETAIL_INIT(prio, group, (byte)0, (byte)0, work_size, (string)null);
        AppMain.DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork = (AppMain.DMS_TITLEOP_OBJ_3DNN_WORK)obsObjectWork;
        obsObjectWork.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(this.dmTitleOpObjDraw);
        obsObjectWork.ppOutSub = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppIn = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppActCall = (AppMain.OBS_OBJECT_WORK_Delegate2)null;
        obsObjectWork.ppRec = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppLast = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obsObjectWork.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        return titleopObj3DnnWork;
    }

    private void dmTitleOpCreateAction(AppMain.DMS_TITLEOP_MGR_WORK top_mgr_work)
    {
        string sPath = (string)null;
        AppMain.A2S_AMA_HEADER ama = AppMain.readAMAFile((object)AppMain.amBindGet(AppMain.dm_titleop_data[1], 0, out sPath));
        for (uint id = 0; id < 7U; ++id)
        {
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_titleop_aos_tex[0]));
            top_mgr_work.act[(int)id] = AppMain.AoActCreate(ama, id);
        }
        top_mgr_work.flag |= 2147483648U;
    }

    private void dmTitleOpDeleteAction(ref AppMain.DMS_TITLEOP_MGR_WORK top_mgr_work)
    {
        for (int index = 0; index < 7; ++index)
            AppMain.AoActDelete(top_mgr_work.act[index]);
    }

    private void dmTitleOpEndStart(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_TITLEOP_MGR_WORK work = (AppMain.DMS_TITLEOP_MGR_WORK)tcb.work;
        this.dmTitleOpPreEnd(ref work);
        work.frame = 0;
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.dmTitleOpPreEndWait));
    }

    private void dmTitleOpPreEnd(ref AppMain.DMS_TITLEOP_MGR_WORK top_mgr_work)
    {
        for (AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, ushort.MaxValue); obj_work != null; obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, ushort.MaxValue))
            obj_work.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private void dmTitleOpPreEndWait(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_TITLEOP_MGR_WORK work = (AppMain.DMS_TITLEOP_MGR_WORK)tcb.work;
        ++work.frame;
        if (work.frame <= 2)
            return;
        this.dmTitleOpEnd(ref work);
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(this.dmTitleOpEndWait));
    }

    private void dmTitleOpEnd(ref AppMain.DMS_TITLEOP_MGR_WORK top_mgr_work)
    {
        this.dmTitleOpDeleteAction(ref top_mgr_work);
        AppMain.g_obj.ppPre = (AppMain.OBJECT_Delegate)null;
        AppMain.ObjObjectClearAllObject();
        AppMain.ObjPreExit();
        AppMain.ObjExit();
    }

    private void dmTitleOpEndWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.ObjObjectCheckClearAllObject() || AppMain.ObjIsExitWait())
            return;
        AppMain.mtTaskClearTcb(tcb);
    }

    private static void dmTitleOpPreDrawDT(object data)
    {
        AppMain.amDrawSetBGColor(AppMain.dm_titleop_clear_color);
    }

    private void dmTitleOpObjDraw(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private void dmTitleOpObjRockDraw(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_TITLEOP_OBJ_3DNN_WORK titleopObj3DnnWork = (AppMain.DMS_TITLEOP_OBJ_3DNN_WORK)obj_work;
        for (int index = 0; index < titleopObj3DnnWork.rock_setting_num; ++index)
        {
            obj_work.pos.Assign(titleopObj3DnnWork.rock_setting[index].pos);
            obj_work.pos.x += AppMain.dm_titleop_scrl_x_ofst;
            if (obj_work.pos.x < -1966080)
                obj_work.pos.x += 3145728;
            else if (obj_work.pos.x > 1966080)
                obj_work.pos.x -= 3145728;
            if (obj_work.pos.x >= -1179648 && obj_work.pos.x <= 1179648)
            {
                obj_work.scale.Assign(titleopObj3DnnWork.rock_setting[index].scale);
                AppMain.ObjDrawActionSummary(obj_work);
            }
        }
    }

    private static void dmTitleOpFallShaderPreRenderUserFunc(object data)
    {
        AppMain.NNS_RGBA_U8 color = new AppMain.NNS_RGBA_U8((byte)0, (byte)0, (byte)0, byte.MaxValue);
        AppMain.AMS_RENDER_TARGET target = AppMain._am_render_manager.targetp != AppMain._gm_mapFar_render_work ? AppMain._gm_mapFar_render_work : AppMain._am_draw_target;
        if (target.width == 0)
            return;
        AppMain.amRenderCopyTarget(target, color);
    }

    private void dmTitleOpDrawFallShaderPreSettingUserFunc(ref object data)
    {
        if ((AppMain._am_render_manager.targetp != AppMain._gm_mapFar_render_work ? AppMain._gm_mapFar_render_work : AppMain._am_draw_target).width == 0)
            return;
        AppMain.amDrawGetProjectionMatrix();
    }

    private void dmTitleOpCamera(AppMain.OBS_CAMERA camera)
    {
        camera.disp_pos.x = 0.0f;
        camera.disp_pos.y = 5f;
        camera.disp_pos.z = 160f;
        camera.target_pos.x = 0.0f;
        camera.target_pos.y = 0.0f;
        camera.target_pos.z = 10f;
    }

}