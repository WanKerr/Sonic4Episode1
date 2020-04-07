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
    public static void GmGmkBridgeBuild()
    {
        AppMain.gm_gmk_bridge_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(878), AppMain.GmGameDatGetGimmickData(879), 0U);
    }

    public static void GmGmkBridgeFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(878));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_bridge_obj_3d_list, amsAmbHeader.file_num);
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkBridgeInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_BRIDGE");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work1;
        AppMain.ObjObjectCopyAction3dNNModel(work1, AppMain.gm_gmk_bridge_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work1.user_work = ((int)eve_rec.flag & 1) == 0 ? 2U : 3U;
        work1.user_flag = 0U;
        work1.user_timer = 0;
        work1.pos.z = -131072;
        work1.move_flag |= 8448U;
        work1.disp_flag |= 4194304U;
        work1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBridgeMain);
        work1.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBridgeDrawFunc);
        AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work1;
        colWork.obj_col.diff_data = AppMain.g_gm_default_col;
        colWork.obj_col.flag |= 134217728U;
        colWork.obj_col.width = (ushort)(work1.user_work * 64U);
        colWork.obj_col.height = (ushort)16;
        colWork.obj_col.ofst_x = (short)-96;
        colWork.obj_col.ofst_y = (short)0;
        colWork.obj_col.attr = (ushort)1;
        AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), work1, (ushort)0, "GMK_SPILE");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (AppMain.GMS_EFFECT_3DNN_WORK)work2;
        AppMain.ObjObjectCopyAction3dNNModel(work2, AppMain.gm_gmk_bridge_obj_3d_list[2], gmsEffect3DnnWork.obj_3d);
        work2.user_work = work1.user_work;
        work2.pos.z = -131072;
        work2.move_flag |= 8448U;
        work2.disp_flag |= 4194304U;
        work2.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBridgeDecoDrawFunc);
        work2.obj_3d.drawflag |= 32U;
        return work1;
    }

    public static void gmGmkBridgeMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (gmsEnemy3DWork.ene_com.col_work.obj_col.rider_obj == gmsPlayerWork.obj_work)
        {
            if (obj_work.user_timer < 16)
                ++obj_work.user_timer;
            obj_work.user_flag = (uint)gmsPlayerWork.obj_work.pos.x;
        }
        else if (obj_work.user_timer != 0)
            --obj_work.user_timer;
        if (obj_work.user_timer != 0)
        {
            int denom = (int)obj_work.user_work * 53248 * 5 >> 1;
            int a = AppMain.mtMathCos(AppMain.MTM_MATH_CLIP(AppMain.FX_Div((int)obj_work.user_flag - (obj_work.pos.x - 393216 + denom), denom), -4096, 4096) << 2) * 8 * obj_work.user_timer / 16;
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)(AppMain.MTM_MATH_ABS(a) >> 12);
        }
        else
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)0;
    }

    public static void gmGmkBridgeDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        int num1 = (int)obj_work.user_work * 53248 * 5;
        int num2 = (int)obj_work.user_flag - (obj_work.pos.x - 393216);
        int denom1 = num2;
        int denom2 = num1 - num2;
        int v1 = (int)gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y << 12;
        obj_work.ofst.x = -366592;
        obj_work.ofst.y = 26624;
        if (((int)obj_work.disp_flag & 32) != 0)
            return;
        uint lightColor = AppMain.GmMainGetLightColor();
        AppMain.NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        vecFx32.x = obj_work.pos.x + obj_work.ofst.x;
        vecFx32.y = -(obj_work.pos.y + obj_work.ofst.y);
        vecFx32.z = obj_work.pos.z + obj_work.ofst.z;
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMakeUnitMatrix(nnsMatrix);
        AppMain.nnTranslateMatrix(nnsMatrix, nnsMatrix, AppMain.FX_FX32_TO_F32(vecFx32.x), AppMain.FX_FX32_TO_F32(vecFx32.y), AppMain.FX_FX32_TO_F32(vecFx32.z));
        AppMain.AMS_PARAM_DRAW_PRIMITIVE prim = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        prim.type = 0;
        prim.count = 30 * (int)obj_work.user_work;
        prim.ablend = 0;
        prim.bldSrc = 770;
        prim.bldDst = 771;
        prim.bldMode = 32774;
        prim.aTest = (short)1;
        prim.zMask = (short)0;
        prim.zTest = (short)1;
        prim.noSort = (short)1;
        prim.uwrap = 1;
        prim.vwrap = 1;
        prim.texlist = texlist;
        prim.texId = 0;
        prim.vtxPCT3D = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(prim.count);
        AppMain.NNS_PRIM3D_PCT[] buffer = prim.vtxPCT3D.buffer;
        int offset = prim.vtxPCT3D.offset;
        prim.format3D = 4;
        for (int index1 = 0; index1 < (int)(short)obj_work.user_work; ++index1)
        {
            for (int index2 = 0; index2 < 5; ++index2)
            {
                if (obj_work.user_timer != 0)
                {
                    int numer = index1 * 53248 * 5 + index2 * 53248;
                    int v2 = numer >= num2 ? (numer <= num2 ? 4096 : AppMain.FX_Div(num1 - numer, denom2)) : AppMain.FX_Div(numer, denom1);
                    obj_work.ofst.y = 26624 + AppMain.FX_Mul(v1, v2) * obj_work.user_timer / 16;
                }
                int num3 = index2 + 5 * index1;
                float f32_1 = AppMain.FX_FX32_TO_F32(53248 * num3);
                float f32_2 = AppMain.FX_FX32_TO_F32(obj_work.ofst.y);
                int index3 = offset + num3 * 6;
                buffer[index3].Tex.u = buffer[index3 + 1].Tex.u = 0.1f;
                buffer[index3 + 2].Tex.u = buffer[index3 + 3].Tex.u = 0.9f;
                buffer[index3].Tex.v = buffer[index3 + 2].Tex.v = 0.1f;
                buffer[index3 + 1].Tex.v = buffer[index3 + 3].Tex.v = 0.9f;
                buffer[index3].Col = lightColor;
                buffer[index3 + 1].Col = buffer[index3 + 2].Col = buffer[index3 + 3].Col = buffer[index3].Col;
                buffer[index3].Pos.x = buffer[index3 + 1].Pos.x = 6.5f + f32_1;
                buffer[index3 + 2].Pos.x = buffer[index3 + 3].Pos.x = f32_1 - 6.5f;
                buffer[index3].Pos.y = buffer[index3 + 2].Pos.y = 13f - f32_2;
                buffer[index3 + 1].Pos.y = buffer[index3 + 3].Pos.y = -0.0f - f32_2;
                buffer[index3].Pos.z = buffer[index3 + 1].Pos.z = buffer[index3 + 2].Pos.z = buffer[index3 + 3].Pos.z = -1f;
                buffer[index3 + 4] = buffer[index3 + 2];
                buffer[index3 + 5] = buffer[index3 + 3];
                buffer[index3 + 3] = buffer[index3 + 1];
            }
        }
        AppMain.amMatrixPush(nnsMatrix);
        AppMain.ObjDraw3DNNDrawPrimitive(prim);
        AppMain.amMatrixPop();
        AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(prim);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    public static void gmGmkBridgeDecoDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.scale.x = -AppMain.MTM_MATH_ABS(obj_work.scale.x);
        obj_work.ofst.x = -393216;
        AppMain.ObjDrawActionSummary(obj_work);
        obj_work.scale.x = -obj_work.scale.x;
        obj_work.ofst.x = (int)(-393216L + (long)(uint)((int)obj_work.user_work * 53248 * 5));
        AppMain.ObjDrawActionSummary(obj_work);
    }

}