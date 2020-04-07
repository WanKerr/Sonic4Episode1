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
    private static AppMain.OBS_OBJECT_WORK GmGmkSsRingGateInit(
         AppMain.GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SS_RINGGATE");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work1;
        work1.user_work = (uint)AppMain.GmSplStageRingGateNumGet((ushort)eve_rec.left);
        work1.user_flag = (uint)eve_rec.flag & 1U;
        work1.user_timer = 20;
        if ((int)(ushort)work1.user_work > (int)AppMain.g_gm_main_system.ply_work[0].ring_num)
        {
            AppMain.ObjObjectCopyAction3dNNModel(work1, AppMain.gm_gmk_ss_ringgate_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            uint num = AppMain.g_gm_main_system.sync_time % 128U;
            gmsEnemy3DWork.obj_3d.mat_frame = (float)num;
            work1.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsRingGateDrawFunc);
            work1.user_flag = (uint)((int)work1.user_flag & 1 | ((int)num & (int)sbyte.MaxValue) << 8);
        }
        else
            AppMain.ObjObjectCopyAction3dNNModel(work1, AppMain.gm_gmk_ss_ringgate_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        work1.pos.z = -131072;
        work1.move_flag |= 8448U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work1.disp_flag |= 4194304U;
        work1.obj_3d.use_light_flag &= 4294967294U;
        work1.obj_3d.use_light_flag |= 2U;
        if ((int)(ushort)work1.user_work > (int)AppMain.g_gm_main_system.ply_work[0].ring_num)
        {
            AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), work1, (ushort)0, "GATERING");
            AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork1 = (AppMain.GMS_EFFECT_3DNN_WORK)work2;
            AppMain.ObjObjectCopyAction3dNNModel(work2, AppMain.gm_gmk_ss_ringgate_obj_3d_list[12], gmsEffect3DnnWork1.obj_3d);
            work2.pos.z = -65536;
            work2.move_flag |= 8448U;
            work2.disp_flag &= 4294967039U;
            work2.user_work = 0U;
            work2.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsRingGateNumMain);
            work2.dir.y = (ushort)49152;
            AppMain.OBS_OBJECT_WORK work3 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), work1, (ushort)0, "GATENUM10");
            AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork2 = (AppMain.GMS_EFFECT_3DNN_WORK)work3;
            work3.user_timer = (int)(work1.user_work / 10U);
            AppMain.ObjObjectCopyAction3dNNModel(work3, AppMain.gm_gmk_ss_ringgate_obj_3d_list[2 + work3.user_timer], gmsEffect3DnnWork2.obj_3d);
            work3.pos.z = -65536;
            work3.move_flag |= 8448U;
            work3.disp_flag &= 4294967039U;
            work3.user_work = 1U;
            work3.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsRingGateNumMain);
            work3.dir.y = (ushort)49152;
            AppMain.OBS_OBJECT_WORK work4 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), work1, (ushort)0, "GATENUM1");
            AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork3 = (AppMain.GMS_EFFECT_3DNN_WORK)work4;
            work4.user_timer = (int)(work1.user_work % 10U);
            AppMain.ObjObjectCopyAction3dNNModel(work4, AppMain.gm_gmk_ss_ringgate_obj_3d_list[2 + work4.user_timer], gmsEffect3DnnWork3.obj_3d);
            work4.pos.z = -65536;
            work4.move_flag |= 8448U;
            work4.disp_flag &= 4294967039U;
            work4.user_work = 2U;
            work4.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsRingGateNumMain);
            work4.dir.y = (ushort)49152;
        }
        work1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsRingGateMain);
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkSsRingGateDefFunc);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        if (((int)eve_rec.flag & 1) != 0)
        {
            AppMain.ObjRectWorkSet(pRec, (short)-20, (short)-52, (short)20, (short)52);
            work1.dir.z = (ushort)16384;
        }
        else
            AppMain.ObjRectWorkSet(pRec, (short)-52, (short)-20, (short)52, (short)20);
        pRec.flag |= 1024U;
        AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work1;
        colWork.obj_col.diff_data = AppMain.g_gm_default_col;
        if (((int)eve_rec.flag & 1) != 0)
        {
            colWork.obj_col.width = (ushort)24;
            colWork.obj_col.height = (ushort)96;
        }
        else
        {
            colWork.obj_col.width = (ushort)96;
            colWork.obj_col.height = (ushort)24;
        }
        colWork.obj_col.ofst_x = (short)-((int)colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-((int)colWork.obj_col.height / 2);
        colWork.obj_col.attr = (ushort)2;
        colWork.obj_col.flag |= 134217760U;
        if ((int)(ushort)work1.user_work <= (int)AppMain.g_gm_main_system.ply_work[0].ring_num)
        {
            gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
            gmsEnemy3DWork.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
            work1.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
            work1.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.ObjDrawActionSummary);
        }
        return work1;
    }

    public static void GmGmkSsRingGateBuild()
    {
        AppMain.gm_gmk_ss_ringgate_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(917), AppMain.GmGameDatGetGimmickData(918), 0U);
        AppMain.gm_gmk_ss_ringgate_obj_tvx_list = AppMain.GmGameDatGetGimmickData(920);
    }

    public static void GmGmkSsRingGateFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(917);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_ss_ringgate_obj_3d_list, gimmickData.file_num);
        AppMain.gm_gmk_ss_ringgate_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
    }

    private static void gmGmkSsRingGateMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        uint num = (obj_work.user_flag >> 8 & (uint)sbyte.MaxValue) + 1U;
        obj_work.user_flag = (uint)((int)obj_work.user_flag & 1 | ((int)num & (int)sbyte.MaxValue) << 8);
        if ((int)(ushort)obj_work.user_work > (int)gmsPlayerWork.ring_num)
            return;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = (AppMain.OBS_OBJECT_WORK)null;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsRingGateVanish);
        obj_work.disp_flag |= 134217728U;
        obj_work.obj_3d.drawflag |= 8388608U;
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        obj_work.user_timer = 20;
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctZoneEsCreate(obj_work, 5, 9);
        AppMain.GmEffect3DESSetDispOffset(efct_3des, 0.0f, 0.0f, 8f);
        efct_3des.efct_com.obj_work.dir.z = obj_work.dir.z;
        efct_3des.efct_com.obj_work.flag |= 512U;
    }

    private static void gmGmkSsRingGateVanish(AppMain.OBS_OBJECT_WORK obj_work)
    {
        --obj_work.user_timer;
        if (obj_work.user_timer == 8)
        {
            obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.ObjDrawActionSummary);
            AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_ss_ringgate_obj_3d_list[1], ((AppMain.GMS_ENEMY_3D_WORK)obj_work).obj_3d);
        }
        if ((double)obj_work.user_timer > 4.0)
        {
            obj_work.obj_3d.draw_state.alpha.alpha = (float)obj_work.user_timer / 20f;
        }
        else
        {
            obj_work.obj_3d.draw_state.alpha.alpha = 0.2f;
            obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        }
    }

    private static void gmGmkSsRingGateDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != (ushort)1 || parentObj2.gmk_obj == (AppMain.OBS_OBJECT_WORK)parentObj1))
            return;
        int v1_1 = AppMain.FX_Mul(parentObj2.obj_work.spd.x, 5120);
        int num1 = AppMain.FX_Mul(parentObj2.obj_work.spd.y, 5120);
        short objectRotation = (short)AppMain.GmMainGetObjectRotation();
        if (((int)parentObj1.obj_work.user_flag & 1) != 0)
        {
            if (objectRotation > (short)0)
                objectRotation -= (short)16384;
            else
                objectRotation += (short)16384;
        }
        short num2 = (short)((int)objectRotation * 2);
        int v1_2 = -num1;
        int num3 = AppMain.FX_Mul(v1_1, AppMain.mtMathCos((int)num2)) + AppMain.FX_Mul(v1_2, AppMain.mtMathSin((int)num2));
        int num4 = AppMain.FX_Mul(v1_2, AppMain.mtMathCos((int)num2)) - AppMain.FX_Mul(v1_1, AppMain.mtMathSin((int)num2));
        parentObj2.obj_work.spd.x = num3;
        parentObj2.obj_work.spd.y = num4;
        parentObj2.obj_work.spd_m = AppMain.FX_Mul(-parentObj2.obj_work.spd_m, 5120);
        AppMain.GMM_PAD_VIB_MID_TIME(60f);
        parentObj2.player_flag &= 4294967280U;
        parentObj2.player_flag |= 1U;
    }

    private static void gmGmkSsRingGateNumMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (AppMain.GMS_EFFECT_3DNN_WORK)obj_work;
        if (parentObj.user_timer < 8)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            int num = AppMain.MTM_MATH_CLIP((int)((long)parentObj.user_work - (long)AppMain.g_gm_main_system.ply_work[0].ring_num), 0, 99);
            if (obj_work.user_work == 1U && num < 10)
            {
                obj_work.disp_flag |= 32U;
            }
            else
            {
                bool flag = false;
                switch (obj_work.user_work)
                {
                    case 1:
                        if (obj_work.user_timer != num / 10)
                        {
                            obj_work.user_timer = num / 10;
                            flag = true;
                            break;
                        }
                        break;
                    case 2:
                        if (obj_work.user_timer != num % 10)
                        {
                            obj_work.user_timer = num % 10;
                            flag = true;
                            break;
                        }
                        break;
                }
                if (flag)
                {
                    AppMain.ObjAction3dNNMotionRelease(obj_work.obj_3d);
                    AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
                    AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_ss_ringgate_obj_3d_list[2 + obj_work.user_timer], gmsEffect3DnnWork.obj_3d);
                }
                obj_work.dir.z = AppMain.GmMainGetObjectRotation();
                obj_work.disp_flag |= parentObj.disp_flag & 134217728U;
                obj_work.obj_3d.drawflag |= parentObj.obj_3d.drawflag & 8388608U;
                obj_work.obj_3d.draw_state.alpha.alpha = parentObj.obj_3d.draw_state.alpha.alpha;
                int x = parentObj.pos.x;
                int y = parentObj.pos.y;
                switch (obj_work.user_work)
                {
                    case 0:
                        x += AppMain.FX_Mul(-36864, AppMain.mtMathCos((int)obj_work.dir.z));
                        y += AppMain.FX_Mul(-36864, AppMain.mtMathSin((int)obj_work.dir.z));
                        break;
                    case 2:
                        x += AppMain.FX_Mul(36864, AppMain.mtMathCos((int)obj_work.dir.z));
                        y += AppMain.FX_Mul(36864, AppMain.mtMathSin((int)obj_work.dir.z));
                        break;
                }
                obj_work.pos.x = x;
                obj_work.pos.y = y;
            }
        }
    }

    private static void gmGmkSsRingGateDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.TVX_FILE model_tvx;
        if (AppMain.gm_gmk_ss_ringgate_obj_tvx_list.buf[0] == null)
        {
            model_tvx = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_ss_ringgate_obj_tvx_list, 0));
            AppMain.gm_gmk_ss_ringgate_obj_tvx_list.buf[0] = (object)model_tvx;
        }
        else
            model_tvx = (AppMain.TVX_FILE)AppMain.gm_gmk_ss_ringgate_obj_tvx_list.buf[0];
        AppMain.NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        uint dispLightDisable = AppMain.GMD_TVX_DISP_LIGHT_DISABLE;
        uint num1 = 0;
        if (obj_work.dir.z != (ushort)0)
        {
            dispLightDisable |= AppMain.GMD_TVX_DISP_ROTATE;
            num1 = (uint)obj_work.dir.z;
        }
        AppMain.GMS_TVX_EX_WORK ex_work = new AppMain.GMS_TVX_EX_WORK();
        uint num2 = obj_work.user_flag >> 13 & 3U;
        ex_work.u_wrap = 0;
        ex_work.v_wrap = 0;
        ex_work.coord.u = -0.25f * (float)num2;
        ex_work.coord.v = 0.0f;
        ex_work.color = uint.MaxValue;
        AppMain.GmTvxSetModelEx(model_tvx, texlist, ref obj_work.pos, ref obj_work.scale, dispLightDisable, (short)num1, ref ex_work);
    }
}