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
    private static AppMain.OBS_OBJECT_WORK GmGmkSsEnduranceInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SS_ENDURANCE");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= (short)128;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_ss_endurance_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.user_flag = (uint)(((int)~eve_rec.flag & 3) + 1) - (uint)eve_rec.byte_param[1];
        work.user_work = AppMain.gmGmkSsEnduranceColorSet(work.user_flag);
        AppMain.gmGmkSsEnduranceScaleSet(work);
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsEnduranceWait);
        work.user_timer = 0;
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsEnduranceDrawFunc);
        AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work;
        colWork.obj_col.diff_data = AppMain.g_gm_default_col;
        colWork.obj_col.width = (ushort)24;
        colWork.obj_col.height = (ushort)24;
        colWork.obj_col.ofst_x = (short)-((int)colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-((int)colWork.obj_col.height / 2);
        colWork.obj_col.attr = (ushort)2;
        colWork.obj_col.flag |= 134217760U;
        return work;
    }

    public static void GmGmkSsEnduranceBuild()
    {
        AppMain.gm_gmk_ss_endurance_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(903), AppMain.GmGameDatGetGimmickData(904), 0U);
        AppMain.gm_gmk_ss_endurance_obj_tvx_list = AppMain.GmGameDatGetGimmickData(906);
    }

    public static void GmGmkSsEnduranceFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(903);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_ss_endurance_obj_3d_list, gimmickData.file_num);
        AppMain.gm_gmk_ss_endurance_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
    }

    private static void gmGmkSsEnduranceWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_COLLISION_OBJ objCol = obj_work.col_work.obj_col;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (((int)AppMain.GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            if (objCol.toucher_obj == gmsPlayerWork.obj_work)
            {
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsEnduranceDamage);
                obj_work.user_timer |= 30;
                AppMain.GmSoundPlaySE("Special3");
            }
            AppMain.gmGmkSsEnduranceUpdateUVTimer(obj_work);
            AppMain.GmGmkSsSquareBounce(obj_work);
        }
    }

    private static void gmGmkSsEnduranceDamage(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)AppMain.GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            uint num = (uint)(obj_work.user_timer & (int)byte.MaxValue) - 1U;
            obj_work.user_timer = (int)((long)obj_work.user_timer & 4294967040L | (long)(num & (uint)byte.MaxValue));
            if (num == 0U)
            {
                --obj_work.user_flag;
                ++((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.byte_param[1];
                if (((int)obj_work.user_flag & int.MaxValue) != 0)
                {
                    AppMain.gmGmkSsEnduranceScaleSet(obj_work);
                    obj_work.user_work = AppMain.gmGmkSsEnduranceColorSet(obj_work.user_flag & (uint)int.MaxValue);
                    obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsEnduranceWait);
                }
                else
                {
                    obj_work.flag |= 4U;
                    ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.enemy_flag |= 65536U;
                    AppMain.GmEfctZoneEsCreate(obj_work, 5, 8).efct_com.obj_work.flag |= 512U;
                }
            }
            else
            {
                uint color_num = (uint)(((obj_work.user_timer & 12) >> 2) + 1);
                obj_work.user_work = AppMain.gmGmkSsEnduranceColorSet(color_num);
            }
            AppMain.gmGmkSsEnduranceUpdateUVTimer(obj_work);
            AppMain.GmGmkSsSquareBounce(obj_work);
        }
    }

    private static void gmGmkSsEnduranceDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.TVX_FILE model_tvx;
        if (AppMain.gm_gmk_ss_endurance_obj_tvx_list.buf[0] == null)
        {
            model_tvx = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_ss_endurance_obj_tvx_list, 0));
            AppMain.gm_gmk_ss_endurance_obj_tvx_list.buf[0] = (object)model_tvx;
        }
        else
            model_tvx = (AppMain.TVX_FILE)AppMain.gm_gmk_ss_endurance_obj_tvx_list.buf[0];
        AppMain.NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        AppMain.GMS_TVX_EX_WORK ex_work = new AppMain.GMS_TVX_EX_WORK();
        uint num = (uint)obj_work.user_timer >> 10 & 31U;
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.u = 0.125f * (float)((int)AppMain.gm_gmk_ss_endurance_uv_parameter[(int)num] % 4) + AppMain.gm_gmk_ss_endurance_mat_color[(int)obj_work.user_work].u;
        ex_work.coord.v = 0.125f * (float)((int)AppMain.gm_gmk_ss_endurance_uv_parameter[(int)num] / 4) + AppMain.gm_gmk_ss_endurance_mat_color[(int)obj_work.user_work].v;
        ex_work.color = uint.MaxValue;
        AppMain.GmTvxSetModelEx(model_tvx, texlist, ref obj_work.pos, ref obj_work.scale, AppMain.GMD_TVX_DISP_SCALE | AppMain.GMD_TVX_DISP_LIGHT_DISABLE, (short)0, ref ex_work);
    }

    private static void gmGmkSsEnduranceUpdateUVTimer(AppMain.OBS_OBJECT_WORK obj_work)
    {
        uint num = ((uint)obj_work.user_timer >> 8 & (uint)sbyte.MaxValue) + 1U;
        if (num >= 120U)
            num = 0U;
        obj_work.user_timer = (int)((long)(obj_work.user_timer & (int)byte.MaxValue) | (long)(num << 8));
    }

    private static uint gmGmkSsEnduranceColorSet(uint color_num)
    {
        return (uint)AppMain.gm_gmk_ss_endurance_color[(int)color_num];
    }

    private static void gmGmkSsEnduranceScaleSet(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int num = 4096;
        switch (obj_work.user_flag & (uint)int.MaxValue)
        {
            case 1:
                num = 2867;
                break;
            case 2:
                num = 3276;
                break;
            case 3:
                num = 3686;
                break;
        }
        obj_work.scale.x = obj_work.scale.y = obj_work.scale.z = num;
    }

    private static void GmGmkSsCircleBuild()
    {
        AppMain.gm_gmk_ss_circle_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(900)), AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(901)), 0U);
        AppMain.gm_gmk_ss_circle_obj_tvx_list = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(902));
    }

    private static void GmGmkSsCircleFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(900));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_ss_circle_obj_3d_list, amsAmbHeader.file_num);
        AppMain.gm_gmk_ss_circle_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSsCircleInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SS_CIRCLE");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= (short)128;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_ss_circle_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.user_flag = 0U;
        if (eve_rec.id == (ushort)194)
        {
            work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsOnewayMain);
            work.disp_flag |= 134217728U;
            work.obj_3d.drawflag |= 8388608U;
            work.obj_3d.draw_state.alpha.alpha = 0.5f;
        }
        else
        {
            work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsCircleMain);
            AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
            colWork.obj_col.obj = work;
            colWork.obj_col.diff_data = AppMain.g_gm_default_col;
            colWork.obj_col.width = (ushort)24;
            colWork.obj_col.height = (ushort)24;
            colWork.obj_col.ofst_x = (short)-((int)colWork.obj_col.width / 2);
            colWork.obj_col.ofst_y = (short)-((int)colWork.obj_col.height / 2);
            colWork.obj_col.attr = (ushort)2;
            colWork.obj_col.flag |= 134217760U;
        }
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsCircleDrawFunc);
        return work;
    }

    private static void GmGmkSsOnewayThrough(uint sw_no)
    {
        AppMain.GmSplStageSwSet(sw_no);
    }

    private static void gmGmkSsCircleMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)AppMain.GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            if (((int)AppMain.g_gm_main_system.sync_time & 15) == 0)
                obj_work.dir.z += (ushort)8192;
            AppMain.GmGmkSsSquareBounce(obj_work);
        }
    }

    private static void gmGmkSsOnewayMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (((int)AppMain.GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            if (AppMain.GmSplStageSwCheck((uint)gmsEnemy3DWork.ene_com.eve_rec.flag))
            {
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsCircleMain);
                AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
                colWork.obj_col.obj = obj_work;
                colWork.obj_col.diff_data = AppMain.g_gm_default_col;
                colWork.obj_col.width = (ushort)24;
                colWork.obj_col.height = (ushort)24;
                colWork.obj_col.ofst_x = (short)-((int)colWork.obj_col.width / 2);
                colWork.obj_col.ofst_y = (short)-((int)colWork.obj_col.height / 2);
                colWork.obj_col.attr = (ushort)2;
                colWork.obj_col.flag |= 134217760U;
                obj_work.disp_flag &= 4160749567U;
                obj_work.obj_3d.drawflag &= 4286578687U;
                obj_work.obj_3d.draw_state.alpha.alpha = 1f;
            }
            AppMain.GmGmkSsSquareBounce(obj_work);
        }
    }

    private static void gmGmkSsCircleDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.TVX_FILE model_tvx;
        if (AppMain.gm_gmk_ss_circle_obj_tvx_list.buf[0] == null)
        {
            model_tvx = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_ss_circle_obj_tvx_list, 0));
            AppMain.gm_gmk_ss_circle_obj_tvx_list.buf[0] = (object)model_tvx;
        }
        else
            model_tvx = (AppMain.TVX_FILE)AppMain.gm_gmk_ss_circle_obj_tvx_list.buf[0];
        AppMain.NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        uint flag = AppMain.GMD_TVX_DISP_LIGHT_DISABLE | AppMain.GMD_TVX_DISP_ROTATE;
        AppMain.GMS_TVX_EX_WORK ex_work = new AppMain.GMS_TVX_EX_WORK();
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.u = 0.0f;
        ex_work.coord.v = 0.0f;
        ex_work.color = uint.MaxValue;
        if ((double)obj_work.obj_3d.draw_state.alpha.alpha == 0.5)
        {
            ex_work.color = 4294967176U;
            flag |= AppMain.GMD_TVX_DISP_BLEND;
        }
        AppMain.GmTvxSetModelEx(model_tvx, texlist, ref obj_work.pos, ref obj_work.scale, flag, (short)-obj_work.dir.z, ref ex_work);
    }

}