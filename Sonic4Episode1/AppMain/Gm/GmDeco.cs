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
    private static void GmDecoInit()
    {
        AppMain.gmDecoInitMgr();
        AppMain.ClearArray<AppMain.GMS_DECO_FALL_MANAGER>(AppMain.g_deco_fall_manager, 0, 16);
    }

    public static void GmDecoExit()
    {
        AppMain.gmDecoExitMgr();
        AppMain.gmDeco_motionHeader = (AppMain.AMS_AMB_HEADER)null;
        AppMain.gmDeco_matMotionHeader = (AppMain.AMS_AMB_HEADER)null;
    }

    public static void gmDecoDrawServerMain(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        uint command = 0;
        float syncTime = (float)AppMain.gmDecoGameSystemGetSyncTime();
        for (int index1 = 0; index1 < 16; ++index1)
        {
            AppMain.GMS_DECO_FALL_MANAGER gmsDecoFallManager = AppMain.g_deco_fall_manager[index1];
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
                        AppMain.ObjDraw3DNNSetCameraEx(0, 1, 12U);
                        break;
                }
                AppMain.VecFx32 vecFx32_1 = new AppMain.VecFx32();
                vecFx32_1.Assign(gmsDecoFallManager.reg[0].vec);
                AppMain.NNS_TEXLIST texlist = gmsDecoFallManager.texlist;
                AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
                AppMain.nnMakeUnitMatrix(nnsMatrix);
                AppMain.nnTranslateMatrix(nnsMatrix, nnsMatrix, AppMain.FX_FX32_TO_F32(vecFx32_1.x), AppMain.FX_FX32_TO_F32(vecFx32_1.y), AppMain.FX_FX32_TO_F32(vecFx32_1.z));
                AppMain.AMS_PARAM_DRAW_PRIMITIVE prim = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
                prim.type = 0;
                prim.count = 6 * (int)gmsDecoFallManager.reg_num;
                prim.ablend = 1;
                prim.bldSrc = 770;
                prim.bldDst = 771;
                prim.bldMode = 32774;
                prim.aTest = (short)0;
                prim.zMask = (short)0;
                prim.zTest = (short)1;
                prim.noSort = (short)1;
                prim.uwrap = 1;
                prim.vwrap = 0;
                prim.texlist = texlist;
                prim.texId = texlist.nTex - 1;
                prim.vtxPCT3D = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(prim.count);
                AppMain.NNS_PRIM3D_PCT[] buffer = prim.vtxPCT3D.buffer;
                int offset = prim.vtxPCT3D.offset;
                prim.format3D = 4;
                uint num1 = 0;
                float num2 = (float)((double)AppMain.fmod(syncTime, gmsDecoFallManager.frame) / (double)gmsDecoFallManager.frame * 5.02799081802368);
                AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
                AppMain.VecFx32 vecFx32_2 = new AppMain.VecFx32();
                for (uint index2 = 0; index2 < 8U; ++index2)
                {
                    vecFx32_2.Assign(gmsDecoFallManager.reg[(int)index2].vec);
                    if (vecFx32_2.x != 0)
                    {
                        int index3 = offset + 6 * (int)num1;
                        buffer[index3].Tex.u = buffer[index3 + 1].Tex.u = 1f;
                        buffer[index3 + 2].Tex.u = buffer[index3 + 3].Tex.u = 0.0f;
                        buffer[index3].Tex.v = buffer[index3 + 2].Tex.v = -1f * (float)gmsDecoFallManager.reg[(int)index2].num - num2;
                        buffer[index3 + 1].Tex.v = buffer[index3 + 3].Tex.v = 0.0f - num2;
                        buffer[index3].Col = uint.MaxValue;
                        buffer[index3 + 1].Col = buffer[index3 + 2].Col = buffer[index3 + 3].Col = buffer[index3].Col;
                        nnsVector.x = AppMain.FX_FX32_TO_F32(vecFx32_2.x - vecFx32_1.x);
                        nnsVector.y = AppMain.FX_FX32_TO_F32(vecFx32_2.y - vecFx32_1.y);
                        nnsVector.z = AppMain.FX_FX32_TO_F32(vecFx32_2.z - vecFx32_1.z);
                        buffer[index3].Pos.x = buffer[index3 + 1].Pos.x = 32f + nnsVector.x;
                        buffer[index3 + 2].Pos.x = buffer[index3 + 3].Pos.x = nnsVector.x - 32f;
                        buffer[index3].Pos.y = buffer[index3 + 2].Pos.y = 32f + nnsVector.y;
                        buffer[index3 + 1].Pos.y = buffer[index3 + 3].Pos.y = (float)((double)nnsVector.y - 32.0 + -64.0 * (double)(gmsDecoFallManager.reg[(int)index2].num - 1U));
                        buffer[index3].Pos.z = buffer[index3 + 1].Pos.z = buffer[index3 + 2].Pos.z = buffer[index3 + 3].Pos.z = 1f + nnsVector.z;
                        buffer[index3 + 4] = buffer[index3 + 2];
                        buffer[index3 + 5] = buffer[index3 + 3];
                        buffer[index3 + 3] = buffer[index3 + 1];
                        gmsDecoFallManager.reg[(int)index2].vec.y = 0;
                        ++num1;
                    }
                }
                AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
                AppMain.amMatrixPush(nnsMatrix);
                AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
                AppMain.ObjDraw3DNNDrawPrimitive(prim, command, 0, 0);
                AppMain.amMatrixPop();
                AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(prim);
            }
        }
        AppMain.gmDecoExecuteDrawPrimitive();
        AppMain.gmDecoInitDrawPrimitive();
    }

    private static uint gmDecoGameSystemGetSyncTime()
    {
        return AppMain.g_gm_main_system.sync_time;
    }

    private static void gmDecoExecuteDrawPrimitive()
    {
        AppMain.GMS_DECO_PRIM_DRAW_WORK[] gDecoTvxWork = AppMain.g_deco_tvx_work;
        if (gDecoTvxWork[0].tex == null)
            return;
        AppMain.SNNS_MATRIX dst1 = new AppMain.SNNS_MATRIX();
        AppMain.SNNS_MATRIX dst2 = new AppMain.SNNS_MATRIX();
        AppMain.SNNS_MATRIX snnsMatrix = new AppMain.SNNS_MATRIX();
        AppMain.nnMakeUnitMatrix(ref snnsMatrix);
        AppMain.nnMakeScaleMatrix(out dst1, -1f, 1f, 1f);
        AppMain.nnMakeScaleMatrix(out dst2, 1f, -1f, 1f);
        uint lightColor = AppMain.GmMainGetLightColor();
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.ablend = 0;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.bldSrc = 770;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.bldDst = 1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.bldMode = 32774;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.aTest = (short)1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.zMask = (short)0;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.zTest = (short)1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.noSort = (short)1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.uwrap = 1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.vwrap = 1;
        AppMain._AMS_PARAM_DRAW_PRIMITIVE.format3D = 4;
        AppMain.SNNS_VECTOR src = new AppMain.SNNS_VECTOR();
        for (uint index1 = 0; index1 < 16U && gDecoTvxWork[(int)index1].tex_id != -1; ++index1)
        {
            if (gDecoTvxWork[(int)index1].tex_id != 0)
            {
                AppMain._AMS_PARAM_DRAW_PRIMITIVE.ablend = 1;
                AppMain._AMS_PARAM_DRAW_PRIMITIVE.aTest = (short)0;
            }
            else
            {
                AppMain._AMS_PARAM_DRAW_PRIMITIVE.ablend = 0;
                AppMain._AMS_PARAM_DRAW_PRIMITIVE.aTest = (short)1;
            }
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.texlist = gDecoTvxWork[(int)index1].tex.texlist;
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.type = 1;
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.count = (int)gDecoTvxWork[(int)index1].all_vtx_num + (int)gDecoTvxWork[(int)index1].stack_num * 2 - 2;
            if (AppMain._gmDecoExecuteDrawPrimitive.v_tbl[(int)index1] == null || AppMain._gmDecoExecuteDrawPrimitive.v_tbl[(int)index1].Length < AppMain._AMS_PARAM_DRAW_PRIMITIVE.count)
                AppMain._gmDecoExecuteDrawPrimitive.v_tbl[(int)index1] = new AppMain.NNS_PRIM3D_PCT[AppMain._AMS_PARAM_DRAW_PRIMITIVE.count];
            AppMain._gmDecoExecuteDrawPrimitive.v_tbl_array[(int)index1].buffer = AppMain._gmDecoExecuteDrawPrimitive.v_tbl[(int)index1];
            AppMain.NNS_PRIM3D_PCT[] buffer = AppMain._gmDecoExecuteDrawPrimitive.v_tbl_array[(int)index1].buffer;
            int num1 = 0;
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.vtxPCT3D = AppMain._gmDecoExecuteDrawPrimitive.v_tbl_array[(int)index1];
            AppMain._AMS_PARAM_DRAW_PRIMITIVE.texId = gDecoTvxWork[(int)index1].tex_id;
            for (uint index2 = 0; index2 < (uint)gDecoTvxWork[(int)index1].stack_num; ++index2)
            {
                AppMain.GMS_DECO_PRIM_DRAW_STACK decoPrimDrawStack = gDecoTvxWork[(int)index1].stack[(int)index2];
                int num2 = (int)decoPrimDrawStack.vtx_num / 3;
                float num3 = AppMain.FXM_FX32_TO_FLOAT(decoPrimDrawStack.pos.x);
                float num4 = -AppMain.FXM_FX32_TO_FLOAT(decoPrimDrawStack.pos.y) + decoPrimDrawStack.off_y;
                float num5 = AppMain.FXM_FX32_TO_FLOAT(decoPrimDrawStack.pos.z);
                int num6 = num1;
                AppMain.AOS_TVX_VERTEX[] vtx = decoPrimDrawStack.vtx;
                for (int index3 = 0; (long)index3 < (long)decoPrimDrawStack.vtx_num; ++index3)
                {
                    src.x = vtx[index3].x;
                    src.y = vtx[index3].y;
                    src.z = vtx[index3].z;
                    int index4 = num6 + index3;
                    if (((int)decoPrimDrawStack.disp_flag & 1) != 0)
                        AppMain.nnTransformVector(ref buffer[index4].Pos, ref dst1, ref src);
                    else if (((int)decoPrimDrawStack.disp_flag & 2) != 0)
                        AppMain.nnTransformVector(ref buffer[index4].Pos, ref dst2, ref src);
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
                if ((long)index2 != (long)((int)gDecoTvxWork[(int)index1].stack_num - 1))
                {
                    int index3 = num6 + (int)decoPrimDrawStack.vtx_num - 1;
                    buffer[index3 + 1] = buffer[index3];
                }
            }
            AppMain.ObjDraw3DNNSetCameraEx(0, 1, gDecoTvxWork[(int)index1].command);
            AppMain.amMatrixPush(ref snnsMatrix);
            AppMain.ObjDraw3DNNDrawPrimitive(AppMain._AMS_PARAM_DRAW_PRIMITIVE, gDecoTvxWork[(int)index1].command, 0, 0);
            AppMain.amMatrixPop();
        }
    }

    private static void gmDecoInitDrawPrimitive()
    {
        AppMain.GMS_DECO_PRIM_DRAW_WORK[] gDecoTvxWork = AppMain.g_deco_tvx_work;
        for (int index = 0; index < 16; ++index)
        {
            gDecoTvxWork[index].tex = (AppMain.AOS_TEXTURE)null;
            gDecoTvxWork[index].tex_id = -1;
            gDecoTvxWork[index].command = uint.MaxValue;
            gDecoTvxWork[index].all_vtx_num = (ushort)0;
            gDecoTvxWork[index].stack_num = (ushort)0;
        }
    }

    private static void gmDecoExitMgr()
    {
        if (AppMain.g_deco_mgr == null)
            return;
        if (AppMain.g_deco_mgr.se_handle != null)
        {
            AppMain.GmSoundStopSE(AppMain.g_deco_mgr.se_handle);
            AppMain.GsSoundFreeSeHandle(AppMain.g_deco_mgr.se_handle);
            AppMain.g_deco_mgr.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.gmDecoDeleteTcbPost();
        AppMain.g_deco_mgr = (AppMain.GMS_DECO_MGR)null;
    }

    private static void GmDecoSetLightFinalZone()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.x = 0.0f;
        nnsVector.y = -0.3f;
        nnsVector.z = -0.4f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_2, ref col, 0.8f, nnsVector);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    private static void gmDecoDeleteTcbPost()
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr.tcb_post == null)
            return;
        AppMain.mtTaskClearTcb(mgr.tcb_post);
        mgr.tcb_post = (AppMain.MTS_TASK_TCB)null;
    }

    private static AppMain.GMS_DECO_MGR gmDecoGetMgr()
    {
        return AppMain.g_deco_mgr;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitModel(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return AppMain.gmDecoInitModel(dec_rec, x, y, type)?.obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitModelMaterial(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return AppMain.gmDecoInitModelMaterial(dec_rec, x, y, type).obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitModelMotionMaterial(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return AppMain.gmDecoInitModelMotioinMaterial(dec_rec, x, y, type).obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitPrimitive3D(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return (AppMain.OBS_OBJECT_WORK)null;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitModelMotionTouch(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return AppMain.gmDecoInitModel(dec_rec, x, y, type)?.obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitFall(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        AppMain.GMS_DECO_WORK gmsDecoWork = AppMain.gmDecoInitFall(dec_rec, x, y, type);
        AppMain.OBS_OBJECT_WORK objWork = gmsDecoWork.obj_work;
        AppMain.OBS_ACTION3D_NN_WORK obj3d = gmsDecoWork.obj_3d;
        float frame = AppMain.amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - AppMain.amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
        AppMain.gmDecoAddFallEvent(dec_rec, objWork.pos.x + objWork.ofst.x, obj3d.texlist, frame);
        return gmsDecoWork.obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitEffect(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return AppMain.gmDecoInitEffect(dec_rec, x, y, type).obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitEffectBlockAndNext(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return AppMain.gmDecoInitEffectBlockAndNext(dec_rec, x, y, type).obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitEffectBlock(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return AppMain.gmDecoInitEffectBlock(dec_rec, x, y, type).obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitModelEffect(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return AppMain.gmDecoInitModelEffect(dec_rec, x, y, type).obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitModelMotionMaterialTouch(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        AppMain.GMS_DECO_WORK deco_work = AppMain.gmDecoInitModelMotioinMaterial(dec_rec, x, y, type);
        if (deco_work == null)
            return (AppMain.OBS_OBJECT_WORK)null;
        ((AppMain.OBS_OBJECT_WORK)deco_work).flag &= 4294967293U;
        int id = (int)dec_rec.id;
        short num1 = AppMain.g_gm_deco_rect_size[id][0];
        short num2 = AppMain.g_gm_deco_rect_size[id][1];
        short left = (short)((int)-num1 / 2);
        short top = (short)((int)-num2 / 2);
        short right = (short)((int)num1 / 2);
        short bottom = (short)((int)num2 / 2);
        AppMain.gmDecoSetRect(deco_work, left, top, (short)-500, right, bottom, (short)500, AppMain.g_gm_deco_func_rect[id]);
        return (AppMain.OBS_OBJECT_WORK)deco_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitModelLoop(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return AppMain.gmDecoInitModelLoop(dec_rec, x, y, type).obj_work;
    }

    private static void GmDecoInitData(AppMain.AMS_AMB_HEADER amb)
    {
        AppMain.gmDecoDataInit();
        AppMain.gmDecoDataSetAmbHeader(amb);
        AppMain.gmDecoInitDrawPrimitive();
    }

    private static void GmDecoBuildData()
    {
        int num = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.GMS_DECO_DATA info = AppMain.gmDecoDataGetInfo();
        AppMain.AMS_AMB_HEADER header = (AppMain.AMS_AMB_HEADER)null;
        if (info != null)
            header = AppMain.gmDecoDataGetAmbHeader();
        string sPath;
        if (AppMain.gmDecoIsUseModel(0))
        {
            info.tvx_model = AppMain.readAMBFile(AppMain.amBindGet(header, 0, out sPath));
            info.tvx_model_data = new AppMain.TVX_FILE[info.tvx_model.file_num];
            info.tvx_model.dir = sPath;
            AppMain.AoTexBuild(info.tvx_tex, AppMain.readAMBFile(AppMain.amBindGet(header, 1)));
            AppMain.AoTexLoad(info.tvx_tex);
        }
        if (!AppMain.gmDecoIsUseModel(1))
            return;
        AppMain.AMS_AMB_HEADER mdl_amb = AppMain.readAMBFile(AppMain.amBindGet(header, 4, out sPath));
        mdl_amb.dir = sPath;
        AppMain.AMS_AMB_HEADER tex_amb = AppMain.readAMBFile(AppMain.amBindGet(header, 5, out sPath));
        tex_amb.dir = sPath;
        uint draw_flag = 0;
        info.obj_3d_list_fall = AppMain.GmGameDBuildRegBuildModel(mdl_amb, tex_amb, draw_flag);
    }

    private static bool GmDecoCheckLoading()
    {
        AppMain.GMS_DECO_DATA info = AppMain.gmDecoDataGetInfo();
        if (AppMain.gmDecoIsUseModel(0) && !AppMain.AoTexIsLoaded(info.tvx_tex))
            return false;
        AppMain.gmDecoIsUseModel(1);
        return true;
    }

    private static void GmDecoRelease()
    {
        AppMain.GmDecoGlareDataRelease();
        AppMain.gmDecoDataRelease();
        AppMain.gmDecoReleaseMgr();
    }

    private static void GmDecoFlushData()
    {
        int num = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.GMS_DECO_DATA info = AppMain.gmDecoDataGetInfo();
        AppMain.AMS_AMB_HEADER header = (AppMain.AMS_AMB_HEADER)null;
        if (info != null)
            header = AppMain.gmDecoDataGetAmbHeader();
        if (AppMain.gmDecoIsUseModel(0))
            AppMain.AoTexRelease(info.tvx_tex);
        if (!AppMain.gmDecoIsUseModel(1))
            return;
        string sPath;
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.amBindGet(header, 4, out sPath));
        amsAmbHeader.dir = sPath;
        AppMain.GmGameDBuildRegFlushModel(info.obj_3d_list_fall, amsAmbHeader.file_num);
        info.obj_3d_list_fall = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static bool GmDecoCheckFlushing()
    {
        AppMain.GMS_DECO_DATA info = AppMain.gmDecoDataGetInfo();
        return !AppMain.gmDecoIsUseModel(0) || AppMain.AoTexIsReleased(info.tvx_tex);
    }

    private static AppMain.OBS_OBJECT_WORK GmDecoInitModelMotion(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        return (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void GmDecoSetFrameMotion(int frame, int index)
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.common_frame_motion[index] = frame;
    }

    private static void GmDecoStartEffectFinalBossLight()
    {
        AppMain.GmDecoSetFrameMotion(1, 2);
    }

    private static AppMain.AMS_RENDER_TARGET GmDecoGetFallRenderTarget()
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return (AppMain.AMS_RENDER_TARGET)null;
        return !mgr.flag_render_front ? (mgr.render_target_back == null ? AppMain.gmDecoDrawFallCopyRenderFront() : mgr.render_target_back) : mgr.render_target_front;
    }

    private static void GmDecoSetLoopState()
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return;
        int index = 0;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)4);
        while (obj_work != null)
        {
            if (obj_work.ppFunc == new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmDecoMainFuncLoop))
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
                AppMain.GMS_DECO_WORK gmsDecoWork = (AppMain.GMS_DECO_WORK)obj_work;
                AppMain.GMS_EVE_RECORD_DECORATE eventRecord = gmsDecoWork.event_record;
                if (eventRecord != null)
                {
                    if (eventRecord.pos_x == byte.MaxValue)
                    {
                        eventRecord.pos_x = gmsDecoWork.event_x;
                        gmsDecoWork.event_x = (byte)0;
                    }
                    gmsDecoWork.event_record = (AppMain.GMS_EVE_RECORD_DECORATE)null;
                }
            }
            obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, (ushort)4);
        }
    }

    private static void GmDecoClearLoopState()
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return;
        for (int index = 0; index < mgr.motion_frame_loop.Length; ++index)
            mgr.motion_frame_loop[index] = 0;
    }

    private static void GmDecoStartLoop()
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.state_loop = 1;
    }

    private static void GmDecoEndLoop()
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.state_loop = 2;
    }

    private static void gmDecoDataInit()
    {
        AppMain.g_deco_data_real.Clear();
        AppMain.g_deco_data = AppMain.g_deco_data_real;
    }

    private static void gmDecoDataRelease()
    {
        if (AppMain.g_deco_data == null)
            return;
        AppMain.gmDecoDataReleaseAmbHeader();
        AppMain.g_deco_data = (AppMain.GMS_DECO_DATA)null;
    }

    private static AppMain.GMS_DECO_DATA gmDecoDataGetInfo()
    {
        return AppMain.g_deco_data;
    }

    private static void gmDecoDataSetAmbHeader(AppMain.AMS_AMB_HEADER amb)
    {
        AppMain.gmDecoDataGetInfo().amb_header = amb;
    }

    private static AppMain.AMS_AMB_HEADER gmDecoDataGetAmbHeader()
    {
        return AppMain.gmDecoDataGetInfo().amb_header;
    }

    private static void gmDecoDataReleaseAmbHeader()
    {
        AppMain.GMS_DECO_DATA info = AppMain.gmDecoDataGetInfo();
        if (info.amb_header == null)
            return;
        info.amb_header = (AppMain.AMS_AMB_HEADER)null;
    }

    private static AppMain.OBS_ACTION3D_NN_WORK[] gmDecoDataGetObj3DList(int id)
    {
        AppMain.GMS_DECO_DATA info = AppMain.gmDecoDataGetInfo();
        AppMain.OBS_ACTION3D_NN_WORK[] obsActioN3DNnWorkArray = (AppMain.OBS_ACTION3D_NN_WORK[])null;
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
                obsActioN3DNnWorkArray = AppMain.GmGmkWaterSliderGetObj3DList();
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

    private static AppMain.AMS_AMB_HEADER gmDecoDataGetMotionHeader(int id)
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = (AppMain.AMS_AMB_HEADER)null;
        switch (id)
        {
            case 7:
            case 8:
            case 9:
            case 11:
            case 12:
            case 13:
                amsAmbHeader = (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(834).pData;
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
                AppMain.AMS_AMB_HEADER ambHeader = AppMain.gmDecoDataGetAmbHeader();
                if (AppMain.gmDeco_matMotionHeader == null)
                {
                    string sPath;
                    AppMain.gmDeco_matMotionHeader = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 2, out sPath));
                    AppMain.gmDeco_matMotionHeader.dir = sPath;
                }
                amsAmbHeader = AppMain.gmDeco_matMotionHeader;
                break;
        }
        return amsAmbHeader;
    }

    private static AppMain.AMS_AMB_HEADER gmDecoDataGetMatMotionHeader(int id)
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = (AppMain.AMS_AMB_HEADER)null;
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
                amsAmbHeader = (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(835).pData;
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
                AppMain.AMS_AMB_HEADER ambHeader = AppMain.gmDecoDataGetAmbHeader();
                if (AppMain.gmDeco_motionHeader == null)
                {
                    string sPath;
                    AppMain.gmDeco_motionHeader = AppMain.readAMBFile(AppMain.amBindGet(ambHeader, 3, out sPath));
                    AppMain.gmDeco_motionHeader.dir = sPath;
                }
                amsAmbHeader = AppMain.gmDeco_motionHeader;
                break;
        }
        return amsAmbHeader;
    }

    private static void gmDecoCopySetRenderTargetForFront(AppMain.AMS_RENDER_TARGET target)
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.flag_render_front = true;
        mgr.render_target_front = target;
    }

    private static void gmDecoCopySetRenderTargetForBack(AppMain.AMS_RENDER_TARGET target)
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.flag_render_back = true;
        mgr.render_target_back = target;
    }

    private static AppMain.AMS_RENDER_TARGET gmDecoGetRenderWorkFront()
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return (AppMain.AMS_RENDER_TARGET)null;
        return mgr.render_target_front == null ? (AppMain.AMS_RENDER_TARGET)null : mgr.render_target_front;
    }

    private static AppMain.AMS_RENDER_TARGET gmDecoGetRenderWorkBack()
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return (AppMain.AMS_RENDER_TARGET)null;
        return mgr.render_target_back == null ? (AppMain.AMS_RENDER_TARGET)null : mgr.render_target_back;
    }

    private static void gmDecoInitMgr()
    {
        AppMain.g_deco_mgr_real.Clear();
        AppMain.g_deco_mgr = AppMain.g_deco_mgr_real;
        AppMain.gmDecoCreateTcbPost();
    }

    private static void gmDecoReleaseMgr()
    {
    }

    private static AppMain.MTS_TASK_TCB gmDecoCreateTcbPost()
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        mgr.tcb_post = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmDecoTcbProcPost), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 5376U, 5, (AppMain.TaskWorkFactoryDelegate)null, "GM DECO POST");
        return mgr.tcb_post;
    }

    private static void gmDecoTcbProcPost(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.ObjDraw3DNNUserFunc(AppMain._gmDecoTcbProcPostDT, (object)null, 0, 8U);
    }

    private static void gmDecoTcbProcPostDT(object data)
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return;
        mgr.flag_render_front = false;
        mgr.flag_render_back = false;
        mgr.render_target_front = (AppMain.AMS_RENDER_TARGET)null;
        mgr.render_target_back = (AppMain.AMS_RENDER_TARGET)null;
    }

    private static void gmDecoDraw(AppMain.OBS_OBJECT_WORK obj_work)
    {
        uint dispFlag = obj_work.disp_flag;
        AppMain.GMS_DECO_WORK gmsDecoWork = (AppMain.GMS_DECO_WORK)obj_work;
        if (gmsDecoWork.model_tex != null)
        {
            if (!AppMain.GmMainIsDrawEnable() || ((int)dispFlag & 32) != 0)
                return;
            if (((int)obj_work.user_flag & 2) == 0)
                AppMain.gmDecoSetDrawPrimitive(gmsDecoWork.model_index, gmsDecoWork.model_tex, obj_work.pos, 0.0f, gmsDecoWork.obj_3d.command_state, dispFlag);
            if (((int)obj_work.user_flag & 4) == 0)
                return;
            AppMain.gmDecoSetDrawPrimitive(((AppMain.GMS_DECO_SUBMODEL_WORK)gmsDecoWork).sub_model_index, gmsDecoWork.model_tex, obj_work.pos, 0.0f, gmsDecoWork.obj_3d.command_state, dispFlag);
        }
        else
        {
            if (((int)obj_work.user_flag & 2) == 0)
            {
                AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
                if (obj3d != null)
                {
                    if (((int)obj_work.user_flag & 8) != 0 && obj3d != null && (obj3d.motion != null && obj3d.motion.mmobject != null))
                    {
                        float startFrame = AppMain.amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
                        float divisor = AppMain.amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - startFrame;
                        float syncTime = (float)AppMain.gmDecoGameSystemGetSyncTime();
                        obj3d.mat_frame = AppMain.fmod(syncTime, divisor);
                        obj_work.disp_flag |= 16U;
                    }
                    if (obj3d.command_state != 0U)
                        AppMain.ObjDraw3DNNSetCameraEx(0, 1, obj_work.obj_3d.command_state);
                    AppMain.ObjDrawActionSummary(obj_work);
                    obj_work.disp_flag = dispFlag;
                }
            }
            if (((int)obj_work.user_flag & 4) == 0)
                return;
            if (AppMain.ObjObjectPauseCheck(0U) != 0U)
                dispFlag |= 4096U;
            AppMain.GMS_DECO_SUBMODEL_WORK decoSubmodelWork = (AppMain.GMS_DECO_SUBMODEL_WORK)obj_work;
            decoSubmodelWork.obj_3d_sub.command_state = 17U;
            if (decoSubmodelWork.obj_3d_sub.command_state != 17U)
                AppMain.ObjDraw3DNNSetCameraEx(0, 1, decoSubmodelWork.obj_3d_sub.command_state);
            AppMain.ObjDrawAction3DNN(decoSubmodelWork.obj_3d_sub, new AppMain.VecFx32?(obj_work.pos), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref dispFlag);
        }
    }

    private static float fmod(float dividend, float divisor)
    {
        return (Math.Abs(dividend) - Math.Abs(divisor) * (float)Math.Floor((double)Math.Abs(dividend) / (double)Math.Abs(divisor))) * (float)Math.Sign(dividend);
    }

    private static void gmDecoDrawFinalShutter3Line(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.user_flag & 2) == 0)
            AppMain.ObjDrawActionSummary(obj_work);
        if (((int)obj_work.user_flag & 4) == 0)
            return;
        uint dispFlag = obj_work.disp_flag;
        if (AppMain.ObjObjectPauseCheck(0U) != 0U)
            dispFlag |= 4096U;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32(obj_work.pos);
        vecFx32.z = -1703936;
        AppMain.GMS_DECO_SUBMODEL_WORK decoSubmodelWork = (AppMain.GMS_DECO_SUBMODEL_WORK)obj_work;
        if (decoSubmodelWork.obj_3d_sub.command_state != 15U)
            AppMain.ObjDraw3DNNSetCameraEx(0, 1, decoSubmodelWork.obj_3d_sub.command_state);
        AppMain.ObjDrawAction3DNN(decoSubmodelWork.obj_3d_sub, new AppMain.VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref dispFlag);
    }

    private static void gmDecoDrawFinalShutter5Line(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_DECO_WORK gmsDecoWork = (AppMain.GMS_DECO_WORK)obj_work;
        uint disp_flag = obj_work.disp_flag | 4096U;
        if (!AppMain.GmMainIsDrawEnable() || ((int)disp_flag & 32) != 0)
            return;
        if (((int)obj_work.user_flag & 2) == 0)
            AppMain.gmDecoSetDrawPrimitive(gmsDecoWork.model_index, gmsDecoWork.model_tex, obj_work.pos, (float)obj_work.user_timer, gmsDecoWork.obj_3d.command_state, disp_flag);
        if (((int)obj_work.user_flag & 4) == 0)
            return;
        AppMain.gmDecoSetDrawPrimitive(((AppMain.GMS_DECO_SUBMODEL_WORK)gmsDecoWork).sub_model_index, gmsDecoWork.model_tex, new AppMain.VecFx32(obj_work.pos)
        {
            x = obj_work.pos.x,
            y = obj_work.pos.y,
            z = -2228224
        }, (float)(-obj_work.user_timer / 2), gmsDecoWork.obj_3d.command_state, disp_flag);
    }

    private static AppMain.AMS_RENDER_TARGET gmDecoDrawFallCopyRenderFront()
    {
        AppMain.AMS_RENDER_TARGET target;
        switch (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id])
        {
            case 0:
                target = AppMain.gmDecoGetRenderWorkFront();
                if (target == null)
                {
                    target = AppMain._gm_mapFar_render_work;
                    if (target.width == 0)
                        return (AppMain.AMS_RENDER_TARGET)null;
                    break;
                }
                break;
            case 2:
                AppMain.AMS_RENDER_TARGET renderTarget = AppMain.GmWaterSurfaceGetRenderTarget();
                if (renderTarget != null)
                {
                    AppMain.gmDecoCopySetRenderTargetForFront(renderTarget);
                    return renderTarget;
                }
                target = AppMain._am_render_manager.targetp != AppMain._gm_mapFar_render_work ? AppMain._gm_mapFar_render_work : AppMain._am_draw_target;
                break;
            default:
                return (AppMain.AMS_RENDER_TARGET)null;
        }
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr != null && !mgr.flag_render_front)
        {
            AppMain.NNS_RGBA_U8 rendaerTargetColor = AppMain.g_deco_rendaer_target_color;
            AppMain.amRenderCopyTarget(target, rendaerTargetColor);
            AppMain.gmDecoCopySetRenderTargetForFront(target);
        }
        return target;
    }

    private static AppMain.AMS_RENDER_TARGET gmDecoDrawFallCopyRenderBack()
    {
        int num = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.AMS_RENDER_TARGET target = AppMain.gmDecoGetRenderWorkBack();
        switch (num)
        {
            case 0:
                if (target == null)
                {
                    target = AppMain._gm_mapFar_render_work;
                    if (target.width == 0)
                        return (AppMain.AMS_RENDER_TARGET)null;
                    break;
                }
                break;
            case 2:
                target = AppMain._am_render_manager.targetp != AppMain._gm_mapFar_render_work ? AppMain._gm_mapFar_render_work : AppMain._am_draw_target;
                break;
            default:
                return (AppMain.AMS_RENDER_TARGET)null;
        }
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr != null && !mgr.flag_render_back)
        {
            AppMain.NNS_RGBA_U8 rendaerTargetColor = AppMain.g_deco_rendaer_target_color;
            AppMain.amRenderCopyTarget(target, rendaerTargetColor);
            AppMain.gmDecoCopySetRenderTargetForBack(target);
        }
        return target;
    }

    private static void gmDecoDrawFallRender(
      AppMain.AMS_RENDER_TARGET render_target,
      AppMain.NNS_MATRIX proj_mtx)
    {
    }

    private static void gmDecoDrawFallFrontUserFunc(object data)
    {
        AppMain.amDrawGetProjectionMatrix();
    }

    private static void gmDecoDrawFallBackUserFunc(object data)
    {
        AppMain.NNS_MATRIX projectionMatrix = AppMain.amDrawGetProjectionMatrix();
        AppMain.AMS_RENDER_TARGET render_target = AppMain.gmDecoDrawFallCopyRenderBack();
        if (render_target == null)
            return;
        AppMain.gmDecoDrawFallRender(render_target, projectionMatrix);
    }

    private static void gmDecoDrawFallFront(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, 1, obj_work.obj_3d.command_state);
        AppMain.ObjDraw3DNNUserFunc(AppMain._gmDecoDrawFallFrontUserFunc, (object)null, 0, obj_work.obj_3d.command_state);
        AppMain.gmDecoDraw(obj_work);
    }

    public static void gmDecoDrawFallBack(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, 1, obj_work.obj_3d.command_state);
        AppMain.ObjDraw3DNNUserFunc(AppMain._gmDecoDrawFallBackUserFunc, (object)null, 0, obj_work.obj_3d.command_state);
        AppMain.gmDecoDraw(obj_work);
    }

    private static AppMain.GMS_DECO_WORK gmDecoLoadObj(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      AppMain.TaskWorkFactoryDelegate work_size,
      int pos_x,
      int pos_y,
      AppMain.MPP_VOID_OBS_OBJECT_WORK main_func,
      AppMain.MPP_VOID_OBS_OBJECT_WORK move_func,
      AppMain.MPP_VOID_OBS_OBJECT_WORK out_func,
      AppMain.GSF_TASK_PROCEDURE dest_func)
    {
        AppMain.GMS_DECO_WORK gmsDecoWork = (AppMain.GMS_DECO_WORK)AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)5376, (byte)5, (byte)0, (byte)0, work_size, "DECO OBJ");
        AppMain.OBS_OBJECT_WORK objWork = gmsDecoWork.obj_work;
        AppMain.mtTaskChangeTcbDestructor(objWork.tcb, dest_func);
        objWork.obj_type = (ushort)4;
        objWork.pos.x = pos_x + AppMain.g_gm_deco_pos[(int)dec_rec.id][0];
        objWork.pos.y = pos_y + AppMain.g_gm_deco_pos[(int)dec_rec.id][1];
        objWork.pos.z = AppMain.g_gm_deco_pos[(int)dec_rec.id][2];
        objWork.dir.z = AppMain.g_gm_deco_rot_z[(int)dec_rec.id];
        objWork.move_flag |= 8448U;
        objWork.ppFunc = main_func;
        objWork.ppMove = move_func;
        objWork.ppOut = out_func;
        objWork.view_out_ofst = (short)((int)AppMain.g_gm_decorate_size_tbl[(int)dec_rec.id] + 16 + 32 + 16);
        objWork.ppViewCheck = new AppMain.OBS_OBJECT_WORK_Delegate3(AppMain.ObjObjectViewOutCheck);
        gmsDecoWork.event_record = dec_rec;
        gmsDecoWork.event_x = dec_rec.pos_x;
        dec_rec.pos_x = byte.MaxValue;
        return gmsDecoWork;
    }

    private static AppMain.GMS_DECO_WORK gmDecoLoadModel(
      AppMain.GMS_DECO_WORK deco_work,
      AppMain.OBS_ACTION3D_NN_WORK obj_3d_work)
    {
        AppMain.ObjObjectCopyAction3dNNModel(deco_work.obj_work, obj_3d_work, deco_work.obj_3d);
        deco_work.obj_3d.drawflag |= 32U;
        return deco_work;
    }

    private static void gmDecoLoadMotion(
      AppMain.GMS_DECO_WORK deco_work,
      int motion_index,
      AppMain.AMS_AMB_HEADER motion_amb)
    {
        AppMain.ObjObjectAction3dNNMotionLoad(deco_work.obj_work, 0, false, (AppMain.OBS_DATA_WORK)null, (string)null, motion_index, motion_amb);
    }

    private static void gmDecoLoadMatMotion(
      AppMain.GMS_DECO_WORK deco_work,
      int motion_index,
      AppMain.AMS_AMB_HEADER motion_amb)
    {
        AppMain.ObjObjectAction3dNNMaterialMotionLoad(deco_work.obj_work, 0, (AppMain.OBS_DATA_WORK)null, (string)null, motion_index, (object)motion_amb);
    }

    private static void gmDecoSetRect(
      AppMain.GMS_DECO_WORK deco_work,
      short left,
      short top,
      short back,
      short right,
      short bottom,
      short front,
      AppMain.OBS_RECT_WORK_Delegate1 func)
    {
        AppMain.OBS_OBJECT_WORK objWork = deco_work.obj_work;
        AppMain.ArrayPointer<AppMain.OBS_RECT_WORK> rect_work = new AppMain.ArrayPointer<AppMain.OBS_RECT_WORK>(deco_work.rect_work, 0);
        AppMain.ObjObjectGetRectBuf(objWork, rect_work, (ushort)1);
        AppMain.ObjRectGroupSet((AppMain.OBS_RECT_WORK)rect_work, (byte)1, (byte)1);
        AppMain.ObjRectAtkSet((AppMain.OBS_RECT_WORK)rect_work, (ushort)0, (short)0);
        AppMain.ObjRectDefSet((AppMain.OBS_RECT_WORK)rect_work, (ushort)65534, (short)0);
        ((AppMain.OBS_RECT_WORK)~rect_work).parent_obj = objWork;
        ((AppMain.OBS_RECT_WORK)~rect_work).ppDef = func;
        ((AppMain.OBS_RECT_WORK)~rect_work).ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        ((AppMain.OBS_RECT_WORK)~rect_work).flag |= 1152U;
        AppMain.ObjRectWorkZSet((AppMain.OBS_RECT_WORK)rect_work, left, top, back, right, bottom, front);
    }

    private static void gmDecoMainFuncMotionCount(AppMain.OBS_OBJECT_WORK obj_work)
    {
        --obj_work.user_timer;
        if (0 < obj_work.user_timer)
            return;
        obj_work.obj_3d.frame[0] = 0.0f;
        obj_work.user_timer = 0;
        obj_work.disp_flag |= 16U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmDecoMainFuncDecreaseMotionSpeed(AppMain.OBS_OBJECT_WORK obj_work)
    {
        --obj_work.user_timer;
        if (obj_work.user_timer > 0)
        {
            float userTimer = (float)obj_work.user_timer;
            float num = obj_work.obj_3d.speed[0] / userTimer;
            obj_work.obj_3d.speed[0] -= num;
            obj_work.obj_3d.frame[0] += obj_work.obj_3d.speed[0];
            float startFrame = AppMain.amMotionGetStartFrame(obj_work.obj_3d.motion, obj_work.obj_3d.act_id[0]);
            float endFrame = AppMain.amMotionGetEndFrame(obj_work.obj_3d.motion, obj_work.obj_3d.act_id[0]);
            if ((double)obj_work.obj_3d.frame[0] < (double)startFrame)
            {
                obj_work.obj_3d.frame[0] = endFrame;
            }
            else
            {
                if ((double)obj_work.obj_3d.frame[0] <= (double)endFrame)
                    return;
                obj_work.obj_3d.frame[0] = startFrame;
            }
        }
        else
        {
            obj_work.user_timer = 0;
            obj_work.obj_3d.speed[0] = 0.0f;
            obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        }
    }

    private static void gmDecoMainFuncMotionApplyCommonFrame(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return;
        AppMain.OBS_ACTION3D_NN_WORK[] obsActioN3DNnWorkArray = new AppMain.OBS_ACTION3D_NN_WORK[2]
        {
      obj_work.obj_3d,
      null
        };
        if (((int)obj_work.user_flag & 4) != 0)
        {
            AppMain.GMS_DECO_SUBMODEL_WORK decoSubmodelWork = (AppMain.GMS_DECO_SUBMODEL_WORK)obj_work;
            obsActioN3DNnWorkArray[1] = decoSubmodelWork.obj_3d_sub;
        }
        int int32 = Convert.ToInt32(obj_work.user_work);
        int index1 = 0;
        if (((int)obj_work.user_flag & 32) != 0)
            index1 = 1;
        for (int index2 = 0; 2 > index2; ++index2)
        {
            AppMain.OBS_ACTION3D_NN_WORK obsActioN3DNnWork = obsActioN3DNnWorkArray[index2];
            if (obsActioN3DNnWork != null && obsActioN3DNnWork.motion != null)
            {
                if (obsActioN3DNnWork.motion.mtnbuf[0] != null)
                {
                    obsActioN3DNnWork.frame[0] = (float)(mgr.common_frame_motion[index1] + int32);
                    float startFrame = AppMain.amMotionGetStartFrame(obsActioN3DNnWork.motion, obsActioN3DNnWork.act_id[0]);
                    float endFrame = AppMain.amMotionGetEndFrame(obsActioN3DNnWork.motion, obsActioN3DNnWork.act_id[0]);
                    if ((double)obsActioN3DNnWork.frame[0] < (double)startFrame)
                        obsActioN3DNnWork.frame[0] = startFrame;
                    else if ((double)obsActioN3DNnWork.frame[0] >= (double)endFrame)
                        obsActioN3DNnWork.frame[0] = endFrame - 1f;
                }
                if (obsActioN3DNnWork.motion.mmobject != null)
                {
                    obsActioN3DNnWork.mat_frame = (float)(mgr.common_frame_motion[index1] + int32);
                    float startFrame = AppMain.amMotionMaterialGetStartFrame(obsActioN3DNnWork.motion, obsActioN3DNnWork.mat_act_id);
                    float endFrame = AppMain.amMotionMaterialGetEndFrame(obsActioN3DNnWork.motion, obsActioN3DNnWork.mat_act_id);
                    if ((double)obsActioN3DNnWork.mat_frame < (double)startFrame)
                        obsActioN3DNnWork.mat_frame = startFrame;
                    else if ((double)obsActioN3DNnWork.mat_frame >= (double)endFrame)
                        obsActioN3DNnWork.mat_frame = endFrame - 1f;
                }
            }
        }
    }

    private static void gmDecoMainFuncMotionCheckCommonFrame(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)AppMain.g_gm_main_system.game_flag & 4096) != 0)
            return;
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
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

    private static void gmDecoMainFuncLoop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr == null)
            return;
        int index = 0;
        if (((int)obj_work.user_flag & 32) != 0)
            index = 1;
        if (mgr.state_loop == 0)
            obj_work.flag |= 16U;
        else if (mgr.state_loop == 1)
        {
            AppMain.gmDecoMainFuncMotionCheckCommonFrame(obj_work);
            obj_work.flag &= 4294967279U;
        }
        else
            AppMain.gmDecoMainFuncMotionCheckCommonFrame(obj_work);
        if (obj_work.user_timer != 0 && mgr.common_frame_motion[index] != 0 && mgr.common_frame_motion[index] + 1 >= obj_work.user_timer)
            mgr.common_frame_motion[index] = obj_work.user_timer;
        int id = (int)((AppMain.GMS_DECO_WORK)obj_work).event_record.id;
        if (mgr.se_handle != null)
        {
            if (mgr.common_frame_motion[index] > Convert.ToInt32(obj_work.user_work))
            {
                AppMain.GmSoundStopSE(mgr.se_handle);
                AppMain.GsSoundFreeSeHandle(mgr.se_handle);
                mgr.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
            }
        }
        else if (mgr.state_loop == 1 && mgr.common_frame_motion[index] < Convert.ToInt32(obj_work.user_work) && id != 0)
        {
            mgr.se_handle = AppMain.GsSoundAllocSeHandle();
            if (id == 183)
                AppMain.GmSoundPlaySE("Shutter1", mgr.se_handle);
        }
        if (obj_work.user_timer != Convert.ToInt32(obj_work.user_work) || id == 0 || id != 183)
            return;
        AppMain.GmSoundPlaySE("Shutter2");
    }

    private static void gmDecoMainFuncEffectCheckCommonFrame(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
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

    private static void gmDecoTcbDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_DECO_WORK tcbWork = (AppMain.GMS_DECO_WORK)AppMain.mtTaskGetTcbWork(tcb);
        AppMain.gmDecoDelFallEvent(tcbWork.event_record, tcbWork.obj_work.pos.x + tcbWork.obj_work.ofst.x);
        if (((int)tcbWork.obj_work.user_flag & 4) != 0 && tcbWork.model_tex == null)
            AppMain.ObjAction3dNNMotionRelease(((AppMain.GMS_DECO_SUBMODEL_WORK)tcbWork).obj_3d_sub);
        AppMain.GMS_EVE_RECORD_DECORATE eventRecord = tcbWork.event_record;
        if (eventRecord != null)
        {
            if (eventRecord.pos_x == byte.MaxValue)
            {
                eventRecord.pos_x = tcbWork.event_x;
                tcbWork.event_x = (byte)0;
            }
            tcbWork.event_record = (AppMain.GMS_EVE_RECORD_DECORATE)null;
        }
        AppMain.ObjObjectExit(tcb);
    }

    private static void gmDecoRectFuncChangeMotionCount(
      AppMain.OBS_RECT_WORK own_rect_work,
      AppMain.OBS_RECT_WORK target_rect_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = own_rect_work.parent_obj;
        uint id = (uint)((AppMain.GMS_DECO_WORK)parentObj).event_record.id;
        parentObj.user_timer = AppMain.g_gm_deco_user_timer[(int)id];
        float startFrame = AppMain.amMotionGetStartFrame(parentObj.obj_3d.motion, parentObj.obj_3d.act_id[0]);
        float num = (AppMain.amMotionGetEndFrame(parentObj.obj_3d.motion, parentObj.obj_3d.act_id[0]) - startFrame) * (float)Convert.ToInt32(parentObj.user_work);
        parentObj.obj_3d.speed[0] = num / (float)parentObj.user_timer;
        parentObj.disp_flag &= 4294967279U;
        parentObj.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmDecoMainFuncMotionCount);
    }

    private static void gmDecoRectFuncChangeDecreaseMotionSpeed(
      AppMain.OBS_RECT_WORK own_rect_work,
      AppMain.OBS_RECT_WORK target_rect_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = own_rect_work.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = target_rect_work.parent_obj;
        float num = (float)((((int)parentObj2.move_flag & 32768) == 0 ? (double)AppMain.FX_FX32_TO_F32(parentObj2.spd_m) : (double)AppMain.FX_FX32_TO_F32(parentObj2.spd.x)) * (double)Convert.ToInt32(parentObj1.user_work) / 10.0);
        parentObj1.obj_3d.speed[0] = num;
        if (((int)parentObj1.user_flag & 16) == 0)
            parentObj1.obj_3d.speed[0] = AppMain.MTM_MATH_ABS(parentObj1.obj_3d.speed[0]);
        parentObj1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmDecoMainFuncDecreaseMotionSpeed);
        uint id = (uint)((AppMain.GMS_DECO_WORK)parentObj1).event_record.id;
        parentObj1.user_timer = AppMain.g_gm_deco_user_timer[(int)id];
    }

    private static void gmDecoRectFuncChangeMotionCommonFrame(
      AppMain.OBS_RECT_WORK own_rect_work,
      AppMain.OBS_RECT_WORK target_rect_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = own_rect_work.parent_obj;
        parentObj.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmDecoMainFuncMotionCheckCommonFrame);
        parentObj.flag |= 2U;
    }

    private static void gmDecoSetLightSpecial(AppMain.OBS_OBJECT_WORK obj_work, int id)
    {
    }

    private static void gmDecoAdjustIPhone(AppMain.OBS_OBJECT_WORK obj_work, int id)
    {
        if (obj_work == null || id != 71)
            return;
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)8)
            return;
        float num1;
        float num2;
        if (obj_work.pos.x > AppMain.FX_F32_TO_FX32(1024f) && obj_work.pos.x < AppMain.FX_F32_TO_FX32(1280f))
        {
            num1 = 576f;
            num2 = 54.4f;
        }
        else if (obj_work.pos.x > AppMain.FX_F32_TO_FX32(3008f) && obj_work.pos.x < AppMain.FX_F32_TO_FX32(3072f))
        {
            num1 = 768f;
            num2 = 40f;
        }
        else
        {
            if (obj_work.pos.x <= AppMain.FX_F32_TO_FX32(6848f) || obj_work.pos.x >= AppMain.FX_F32_TO_FX32(6976f) || obj_work.pos.y <= AppMain.FX_F32_TO_FX32(1213f))
                return;
            num1 = 1213f;
            num2 = 61f;
        }
        obj_work.scale.y = AppMain.FX_F32_TO_FX32(num2 / 64f);
        float num3 = (AppMain.FX_FX32_TO_F32(obj_work.pos.y) - num1) / 64f;
        obj_work.pos.y = AppMain.FX_F32_TO_FX32(num1 + num3 * num2);
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitNomodel(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type,
      AppMain.TaskWorkFactoryDelegate work_size)
    {
        int id = (int)dec_rec.id;
        if (((int)AppMain.g_gm_deco_user_flag[id] & 1) != 0 && ((int)AppMain.g_gs_main_sys_info.game_flag & 32) == 0)
        {
            dec_rec.pos_x = byte.MaxValue;
            return (AppMain.GMS_DECO_WORK)null;
        }
        AppMain.GMS_DECO_WORK gmsDecoWork = AppMain.gmDecoLoadObj(dec_rec, work_size, x, y, AppMain.g_gm_deco_func_main[id], AppMain.g_gm_deco_func_move[id], AppMain.g_gm_deco_func_out[id], AppMain.g_gm_deco_func_dest[id]);
        gmsDecoWork.obj_work.disp_flag |= AppMain.g_gm_deco_disp_flag[id];
        gmsDecoWork.obj_work.user_work = AppMain.g_gm_deco_user_work[id];
        gmsDecoWork.obj_work.user_timer = AppMain.g_gm_deco_user_timer[id];
        gmsDecoWork.obj_work.user_flag = AppMain.g_gm_deco_user_flag[id];
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

    private static AppMain.GMS_DECO_WORK gmDecoInitModel(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        int id = (int)dec_rec.id;
        int index1 = -1;
        AppMain.TaskWorkFactoryDelegate work_size;
        if (((int)AppMain.g_gm_deco_user_flag[id] & 4) != 0)
        {
            work_size = (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_DECO_SUBMODEL_WORK());
            index1 = AppMain.g_gm_deco_model_index[id][1];
        }
        else
            work_size = (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_DECO_WORK());
        AppMain.GMS_DECO_WORK deco_work = AppMain.gmDecoInitNomodel(dec_rec, x, y, type, work_size);
        if (deco_work == null)
            return (AppMain.GMS_DECO_WORK)null;
        AppMain.OBS_ACTION3D_NN_WORK[] obj3Dlist = AppMain.gmDecoDataGetObj3DList(id);
        if (obj3Dlist == null)
        {
            deco_work.model_tex = AppMain.gmDecoDataGetInfo().tvx_tex;
            int num = AppMain.g_gm_deco_model_index[id][0];
            deco_work.obj_3d.command_state = AppMain.g_gm_deco_command_state[id];
            deco_work.model_index = num;
            if (index1 != -1)
            {
                AppMain.GMS_DECO_SUBMODEL_WORK decoSubmodelWork = (AppMain.GMS_DECO_SUBMODEL_WORK)deco_work;
                decoSubmodelWork.obj_3d_sub.command_state = AppMain.g_gm_deco_command_state[id];
                decoSubmodelWork.sub_model_index = index1;
            }
            deco_work.obj_work.disp_flag |= AppMain.g_gm_deco_disp_flag[id];
        }
        else
        {
            int index2 = AppMain.g_gm_deco_model_index[id][0];
            if (index2 != -1)
            {
                AppMain.gmDecoLoadModel(deco_work, obj3Dlist[index2]);
                deco_work.obj_3d.command_state = AppMain.g_gm_deco_command_state[id];
            }
            if (index1 != -1)
            {
                AppMain.GMS_DECO_SUBMODEL_WORK decoSubmodelWork = (AppMain.GMS_DECO_SUBMODEL_WORK)deco_work;
                AppMain.ObjCopyAction3dNNModel(obj3Dlist[index1], decoSubmodelWork.obj_3d_sub);
                decoSubmodelWork.obj_3d_sub.drawflag |= 32U;
                if (((int)AppMain.g_gm_deco_user_flag[id] & 128) != 0)
                {
                    decoSubmodelWork.obj_3d_sub.use_light_flag &= 4294967294U;
                    decoSubmodelWork.obj_3d_sub.use_light_flag |= 4U;
                }
                decoSubmodelWork.obj_3d_sub.command_state = AppMain.g_gm_deco_command_state[id];
            }
            deco_work.obj_work.disp_flag |= AppMain.g_gm_deco_disp_flag[id];
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
        AppMain.gmDecoSetLightSpecial(deco_work.obj_work, id);
        return deco_work;
    }

    private static void gmDecoInitMotion(AppMain.GMS_DECO_WORK deco_work)
    {
        int id = (int)deco_work.event_record.id;
        AppMain.AMS_AMB_HEADER motionHeader = AppMain.gmDecoDataGetMotionHeader(id);
        int motion_index = AppMain.g_gm_deco_node_motion_index[id][0];
        if (motion_index != -1)
        {
            AppMain.gmDecoLoadMotion(deco_work, motion_index, motionHeader);
            AppMain.OBS_OBJECT_WORK objWork = deco_work.obj_work;
            uint dispFlag = objWork.disp_flag;
            AppMain.ObjDrawObjectActionSet(objWork, 0);
            objWork.disp_flag = dispFlag;
        }
        if (((int)AppMain.g_gm_deco_user_flag[id] & 4) == 0)
            return;
        int index = AppMain.g_gm_deco_node_motion_index[id][1];
        if (index == -1)
            return;
        AppMain.ObjAction3dNNMotionLoad(((AppMain.GMS_DECO_SUBMODEL_WORK)deco_work).obj_3d_sub, 0, false, (AppMain.OBS_DATA_WORK)null, (string)null, index, motionHeader);
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitModelMotion(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        AppMain.GMS_DECO_WORK deco_work = AppMain.gmDecoInitModel(dec_rec, x, y, type);
        if (deco_work == null)
            return (AppMain.GMS_DECO_WORK)null;
        AppMain.gmDecoInitMotion(deco_work);
        return deco_work;
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitModelMotionTouch(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        AppMain.GMS_DECO_WORK deco_work = AppMain.gmDecoInitModelMotion(dec_rec, x, y, type);
        if (deco_work == null)
            return (AppMain.GMS_DECO_WORK)null;
        ((AppMain.OBS_OBJECT_WORK)deco_work).flag &= 4294967293U;
        int id = (int)dec_rec.id;
        short num1 = AppMain.g_gm_deco_rect_size[id][0];
        short num2 = AppMain.g_gm_deco_rect_size[id][1];
        short left = (short)((int)-num1 / 2);
        short top = (short)((int)-num2 / 2);
        short right = (short)((int)num1 / 2);
        short bottom = (short)((int)num2 / 2);
        AppMain.gmDecoSetRect(deco_work, left, top, (short)-500, right, bottom, (short)500, AppMain.g_gm_deco_func_rect[id]);
        return deco_work;
    }

    private static void gmDecoInitMaterial(AppMain.GMS_DECO_WORK deco_work)
    {
        int id = (int)deco_work.event_record.id;
        AppMain.AMS_AMB_HEADER matMotionHeader = AppMain.gmDecoDataGetMatMotionHeader(id);
        int motion_index = AppMain.g_gm_deco_mat_motion_index[id][0];
        if (motion_index != -1)
        {
            AppMain.gmDecoLoadMatMotion(deco_work, motion_index, matMotionHeader);
            AppMain.OBS_OBJECT_WORK objWork = deco_work.obj_work;
            uint dispFlag = objWork.disp_flag;
            AppMain.ObjDrawObjectActionSet3DNNMaterial(objWork, 0);
            objWork.disp_flag = dispFlag;
        }
        if (((int)AppMain.g_gm_deco_user_flag[id] & 4) == 0)
            return;
        int index = AppMain.g_gm_deco_mat_motion_index[id][1];
        if (index == -1)
            return;
        AppMain.ObjAction3dNNMaterialMotionLoad(((AppMain.GMS_DECO_SUBMODEL_WORK)deco_work).obj_3d_sub, 0, (AppMain.OBS_DATA_WORK)null, (string)null, index, matMotionHeader);
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitModelMaterial(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        AppMain.GMS_DECO_WORK deco_work = AppMain.gmDecoInitModel(dec_rec, x, y, type);
        if (deco_work == null)
            return (AppMain.GMS_DECO_WORK)null;
        AppMain.gmDecoInitMaterial(deco_work);
        return deco_work;
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitModelMotioinMaterial(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        AppMain.GMS_DECO_WORK deco_work = AppMain.gmDecoInitModel(dec_rec, x, y, type);
        if (deco_work == null)
            return (AppMain.GMS_DECO_WORK)null;
        AppMain.gmDecoInitMotion(deco_work);
        AppMain.gmDecoInitMaterial(deco_work);
        return deco_work;
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitModelMotionMaterialTouch(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        AppMain.GMS_DECO_WORK deco_work = AppMain.gmDecoInitModelMotioinMaterial(dec_rec, x, y, type);
        if (deco_work == null)
            return (AppMain.GMS_DECO_WORK)null;
        ((AppMain.OBS_OBJECT_WORK)deco_work).flag &= 4294967293U;
        int id = (int)dec_rec.id;
        short num1 = AppMain.g_gm_deco_rect_size[id][0];
        short num2 = AppMain.g_gm_deco_rect_size[id][1];
        short left = (short)((int)-num1 / 2);
        short top = (short)((int)-num2 / 2);
        short right = (short)((int)num1 / 2);
        short bottom = (short)((int)num2 / 2);
        AppMain.gmDecoSetRect(deco_work, left, top, (short)-500, right, bottom, (short)500, AppMain.g_gm_deco_func_rect[id]);
        return deco_work;
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitModelLoop(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        AppMain.GMS_DECO_WORK gmsDecoWork = AppMain.gmDecoInitModel(dec_rec, x, y, type);
        if (gmsDecoWork == null)
            return (AppMain.GMS_DECO_WORK)null;
        AppMain.GMS_DECO_MGR mgr = AppMain.gmDecoGetMgr();
        if (mgr != null)
        {
            if (mgr.state_loop == 0)
                AppMain.gmDecoMainFuncLoop(gmsDecoWork.obj_work);
            else if (mgr.state_loop == 1)
            {
                for (int index = 0; 12 > index; ++index)
                {
                    if (mgr.motion_frame_loop[index] != 0)
                    {
                        gmsDecoWork.obj_work.user_timer = mgr.motion_frame_loop[index];
                        mgr.motion_frame_loop[index] = 0;
                        AppMain.gmDecoMainFuncLoop(gmsDecoWork.obj_work);
                        break;
                    }
                }
            }
            else
            {
                AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
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
                AppMain.gmDecoMainFuncLoop(gmsDecoWork.obj_work);
            }
        }
        return gmsDecoWork;
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitModelEffect(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        AppMain.GMS_DECO_WORK gmsDecoWork = AppMain.gmDecoInitModel(dec_rec, x, y, type);
        if (gmsDecoWork == null)
            return (AppMain.GMS_DECO_WORK)null;
        int id = (int)dec_rec.id;
        int efct_zone_idx = AppMain.g_gm_deco_model_index[id][1];
        int zone_no = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate(gmsDecoWork.obj_work, zone_no, efct_zone_idx);
        gmsEffect3DesWork.efct_com.obj_work.dir.z = AppMain.g_gm_deco_rot_z[(int)dec_rec.id];
        int num1 = (int)AppMain.g_gm_deco_user_work[id] << 16 >> 16;
        int num2 = (int)AppMain.g_gm_deco_user_work[id] >> 16;
        gmsEffect3DesWork.efct_com.obj_work.pos.x = gmsDecoWork.obj_work.pos.x + num1 * 4096;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = gmsDecoWork.obj_work.pos.y + num2 * 4096;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = gmsDecoWork.obj_work.pos.z + 131072;
        if (gmsDecoWork.obj_3d.command_state == 17U)
            gmsEffect3DesWork.obj_3des.command_state = 17U;
        return gmsDecoWork;
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitPrimitive3D(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        int id = (int)dec_rec.id;
        AppMain.GMS_DECO_WORK gmsDecoWork = AppMain.gmDecoInitNomodel(dec_rec, x, y, type, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_DECO_WORK()));
        if (gmsDecoWork == null)
            return (AppMain.GMS_DECO_WORK)null;
        gmsDecoWork.obj_work.disp_flag |= AppMain.g_gm_deco_disp_flag[id];
        return gmsDecoWork;
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitFall(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        AppMain.GMS_DECO_WORK gmsDecoWork = AppMain.gmDecoInitModelMaterial(dec_rec, x, y, type);
        if (gmsDecoWork == null)
            return (AppMain.GMS_DECO_WORK)null;
        int id = (int)gmsDecoWork.event_record.id;
        AppMain.gmDecoAdjustIPhone((AppMain.OBS_OBJECT_WORK)gmsDecoWork, id);
        return gmsDecoWork;
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitEffect(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        int id = (int)dec_rec.id;
        AppMain.GMS_DECO_WORK gmsDecoWork = AppMain.gmDecoInitNomodel(dec_rec, x, y, type, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_DECO_WORK()));
        if (gmsDecoWork == null)
            return (AppMain.GMS_DECO_WORK)null;
        int efct_zone_idx = AppMain.g_gm_deco_model_index[id][0];
        int zone_no = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate(gmsDecoWork.obj_work, zone_no, efct_zone_idx);
        gmsEffect3DesWork.efct_com.obj_work.dir.z = AppMain.g_gm_deco_rot_z[(int)dec_rec.id];
        gmsEffect3DesWork.efct_com.obj_work.disp_flag |= AppMain.g_gm_deco_disp_flag[id];
        int num1 = (int)gmsEffect3DesWork.efct_com.obj_work.user_flag | (int)AppMain.g_gm_deco_user_flag[id];
        gmsEffect3DesWork.efct_com.obj_work.user_flag = (uint)num1;
        int num2 = (int)gmsEffect3DesWork.efct_com.obj_work.user_work | (int)AppMain.g_gm_deco_user_work[id];
        gmsEffect3DesWork.efct_com.obj_work.user_work = (uint)num2;
        gmsEffect3DesWork.obj_3des.command_state = AppMain.g_gm_deco_command_state[id];
        if (gmsDecoWork.obj_work.ppFunc != null)
        {
            gmsEffect3DesWork.efct_com.obj_work.ppFunc = gmsDecoWork.obj_work.ppFunc;
            gmsDecoWork.obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        }
        return gmsDecoWork;
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitEffectBlock(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        int id = (int)dec_rec.id;
        int x1 = x >> 18 << 18;
        int y1 = y >> 18 << 18;
        AppMain.GMS_DECO_WORK gmsDecoWork = AppMain.gmDecoInitNomodel(dec_rec, x1, y1, type, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_DECO_WORK()));
        if (gmsDecoWork == null)
            return (AppMain.GMS_DECO_WORK)null;
        int efct_zone_idx = AppMain.g_gm_deco_model_index[id][0];
        int zone_no = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctZoneEsCreate(gmsDecoWork.obj_work, zone_no, efct_zone_idx);
        efct_3des.efct_com.obj_work.dir.z = AppMain.g_gm_deco_rot_z[(int)dec_rec.id];
        efct_3des.efct_com.obj_work.disp_flag |= AppMain.g_gm_deco_disp_flag[id];
        efct_3des.efct_com.obj_work.user_flag |= AppMain.g_gm_deco_user_flag[id];
        efct_3des.efct_com.obj_work.user_work |= AppMain.g_gm_deco_user_work[id];
        efct_3des.obj_3des.command_state = AppMain.g_gm_deco_command_state[id];
        efct_3des.efct_com.obj_work.user_flag |= gmsDecoWork.obj_work.user_flag;
        if (gmsDecoWork.obj_work.ppFunc != null)
        {
            efct_3des.efct_com.obj_work.ppFunc = gmsDecoWork.obj_work.ppFunc;
            gmsDecoWork.obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        }
        if (((int)AppMain.g_gm_deco_user_flag[id] & 256) != 0)
        {
            int num1 = (int)AppMain.g_gm_deco_user_work[id] << 16 >> 16;
            int num2 = (int)AppMain.g_gm_deco_user_work[id] >> 16;
            AppMain.GmEffect3DESSetDuplicateDraw(efct_3des, (float)num1, (float)num2, 0.0f);
        }
        return gmsDecoWork;
    }

    private static AppMain.GMS_DECO_WORK gmDecoInitEffectBlockAndNext(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      int y,
      byte type)
    {
        int id = (int)dec_rec.id;
        int x1 = x >> 18 << 18;
        int y1 = y >> 18 << 18;
        AppMain.GMS_DECO_WORK gmsDecoWork = AppMain.gmDecoInitNomodel(dec_rec, x1, y1, type, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_DECO_WORK()));
        if (gmsDecoWork == null)
            return (AppMain.GMS_DECO_WORK)null;
        int efct_zone_idx = AppMain.g_gm_deco_model_index[id][0];
        int zone_no = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork1 = AppMain.GmEfctZoneEsCreate(gmsDecoWork.obj_work, zone_no, efct_zone_idx);
        gmsEffect3DesWork1.efct_com.obj_work.dir.z = AppMain.g_gm_deco_rot_z[(int)dec_rec.id];
        gmsEffect3DesWork1.efct_com.obj_work.disp_flag |= AppMain.g_gm_deco_disp_flag[id];
        gmsEffect3DesWork1.efct_com.obj_work.user_flag |= AppMain.g_gm_deco_user_flag[id];
        gmsEffect3DesWork1.efct_com.obj_work.user_work |= AppMain.g_gm_deco_user_work[id];
        gmsEffect3DesWork1.obj_3des.command_state = AppMain.g_gm_deco_command_state[id];
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork2 = AppMain.GmEfctZoneEsCreate(gmsDecoWork.obj_work, zone_no, efct_zone_idx + 1);
        gmsEffect3DesWork2.efct_com.obj_work.dir.z = AppMain.g_gm_deco_rot_z[(int)dec_rec.id];
        gmsEffect3DesWork2.efct_com.obj_work.disp_flag |= AppMain.g_gm_deco_disp_flag[id];
        gmsEffect3DesWork2.efct_com.obj_work.user_flag |= AppMain.g_gm_deco_user_flag[id];
        gmsEffect3DesWork2.efct_com.obj_work.user_work |= AppMain.g_gm_deco_user_work[id];
        gmsEffect3DesWork2.obj_3des.command_state = AppMain.g_gm_deco_command_state[id];
        if (gmsDecoWork.obj_work.ppFunc != null)
        {
            gmsEffect3DesWork1.efct_com.obj_work.ppFunc = gmsDecoWork.obj_work.ppFunc;
            gmsEffect3DesWork2.efct_com.obj_work.ppFunc = gmsDecoWork.obj_work.ppFunc;
            gmsDecoWork.obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        }
        return gmsDecoWork;
    }

    private static void gmDecoAddFallEvent(
      AppMain.GMS_EVE_RECORD_DECORATE dec_rec,
      int x,
      AppMain.NNS_TEXLIST texlist,
      float frame)
    {
        if (dec_rec == null)
            return;
        ushort id = dec_rec.id;
        AppMain.GMS_DECO_FALL_MANAGER gmsDecoFallManager1 = (AppMain.GMS_DECO_FALL_MANAGER)null;
        AppMain.GMS_DECO_FALL_REGISTER decoFallRegister1 = (AppMain.GMS_DECO_FALL_REGISTER)null;
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
                    AppMain.GMS_DECO_FALL_MANAGER gmsDecoFallManager2 = AppMain.g_deco_fall_manager[index];
                    if (gmsDecoFallManager2.dec_id == 0U)
                    {
                        if (gmsDecoFallManager1 == null)
                            gmsDecoFallManager1 = gmsDecoFallManager2;
                    }
                    else if ((int)gmsDecoFallManager2.dec_id == (int)id)
                    {
                        gmsDecoFallManager1 = gmsDecoFallManager2;
                        break;
                    }
                }
                gmsDecoFallManager1.dec_id = (uint)id;
                gmsDecoFallManager1.texlist = texlist;
                gmsDecoFallManager1.frame = frame;
                ++gmsDecoFallManager1.all_num;
                for (int index = 0; index < 8; ++index)
                {
                    AppMain.GMS_DECO_FALL_REGISTER decoFallRegister2 = gmsDecoFallManager1.reg[index];
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

    private static void gmDecoDelFallEvent(AppMain.GMS_EVE_RECORD_DECORATE dec_rec, int x)
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
                    AppMain.GMS_DECO_FALL_MANAGER gmsDecoFallManager = AppMain.g_deco_fall_manager[index1];
                    if ((int)gmsDecoFallManager.dec_id == (int)id)
                    {
                        --gmsDecoFallManager.all_num;
                        if (gmsDecoFallManager.all_num == (ushort)0)
                        {
                            gmsDecoFallManager.dec_id = 0U;
                            gmsDecoFallManager.texlist = (AppMain.NNS_TEXLIST)null;
                        }
                        for (int index2 = 0; index2 < 8; ++index2)
                        {
                            AppMain.GMS_DECO_FALL_REGISTER decoFallRegister = gmsDecoFallManager.reg[index2];
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

    private static void gmDecoSetDrawFall(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        AppMain.GMS_DECO_WORK gmsDecoWork = (AppMain.GMS_DECO_WORK)obj_work;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        vecFx32.x = obj_work.pos.x + obj_work.ofst.x;
        vecFx32.y = -(obj_work.pos.y + obj_work.ofst.y);
        vecFx32.z = obj_work.pos.z + obj_work.ofst.z;
        AppMain.GMS_EVE_RECORD_DECORATE eventRecord = gmsDecoWork.event_record;
        AppMain.GMS_DECO_FALL_REGISTER decoFallRegister = (AppMain.GMS_DECO_FALL_REGISTER)null;
        for (int index1 = 0; index1 < 16; ++index1)
        {
            if ((int)AppMain.g_deco_fall_manager[index1].dec_id == (int)eventRecord.id)
            {
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    if (AppMain.g_deco_fall_manager[index1].reg[index2].vec.x == vecFx32.x)
                    {
                        decoFallRegister = AppMain.g_deco_fall_manager[index1].reg[index2];
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
      AppMain.AOS_TEXTURE model_tex,
      AppMain.VecFx32 pos,
      float off_y,
      uint command,
      uint disp_flag)
    {
        if (!AppMain.GmMainIsDrawEnable())
            return;
        if (AppMain.gmDecoDataGetInfo().tvx_model_data[model_index] == null)
        {
            AppMain.AmbChunk data = (AppMain.AmbChunk)AppMain.amBindGet(AppMain.gmDecoDataGetInfo().tvx_model, model_index);
            AppMain.gmDecoDataGetInfo().tvx_model_data[model_index] = new AppMain.TVX_FILE(data);
        }
        AppMain.TVX_FILE file = AppMain.gmDecoDataGetInfo().tvx_model_data[model_index];
        uint textureNum = AppMain.AoTvxGetTextureNum(file);
        AppMain.GMS_DECO_PRIM_DRAW_WORK[] gDecoTvxWork = AppMain.g_deco_tvx_work;
        for (uint tex_no = 0; tex_no < textureNum; ++tex_no)
        {
            uint vertexNum = AppMain.AoTvxGetVertexNum(file, tex_no);
            int textureId = AppMain.AoTvxGetTextureId(file, tex_no);
            for (int index = 0; index < 16; ++index)
            {
                if (gDecoTvxWork[index].tex == null && gDecoTvxWork[index].tex_id == -1 && gDecoTvxWork[index].command == uint.MaxValue || gDecoTvxWork[index].tex == model_tex && gDecoTvxWork[index].tex_id == textureId && (int)gDecoTvxWork[index].command == (int)command)
                {
                    gDecoTvxWork[index].tex = model_tex;
                    gDecoTvxWork[index].tex_id = textureId;
                    gDecoTvxWork[index].command = command;
                    gDecoTvxWork[index].all_vtx_num += (ushort)vertexNum;
                    AppMain.GMS_DECO_PRIM_DRAW_STACK decoPrimDrawStack = gDecoTvxWork[index].stack[(int)gDecoTvxWork[index].stack_num];
                    decoPrimDrawStack.vtx = AppMain.AoTvxGetVertex(file, tex_no);
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
        int num = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        if (type == 0 && 5 != num)
            flag = true;
        if (1 == type && (num == 0 || 2 == num))
            flag = true;
        return flag;
    }
}