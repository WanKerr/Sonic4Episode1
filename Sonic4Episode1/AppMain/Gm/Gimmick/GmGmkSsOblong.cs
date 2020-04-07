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
    private static AppMain.OBS_OBJECT_WORK GmGmkSsOblongInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SS_OBLONG");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= (short)128;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_ss_oblong_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.user_flag = 0U;
        work.user_work = 0U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsOblongMain);
        work.user_timer = 0;
        if (((int)eve_rec.flag & 1) != 0)
            work.dir.z = (ushort)16384;
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsOblongDrawFunc);
        AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work;
        colWork.obj_col.diff_data = AppMain.g_gm_default_col;
        if (((int)eve_rec.flag & 1) != 0)
        {
            colWork.obj_col.width = (ushort)24;
            colWork.obj_col.height = (ushort)48;
        }
        else
        {
            colWork.obj_col.width = (ushort)48;
            colWork.obj_col.height = (ushort)24;
        }
        colWork.obj_col.ofst_x = (short)-((int)colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-((int)colWork.obj_col.height / 2);
        colWork.obj_col.attr = (ushort)2;
        colWork.obj_col.flag |= 134217760U;
        return work;
    }

    public static void GmGmkSsOblongBuild()
    {
        AppMain.gm_gmk_ss_oblong_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(987), AppMain.GmGameDatGetGimmickData(988), 0U);
        AppMain.gm_gmk_ss_oblong_obj_tvx_list = AppMain.GmGameDatGetGimmickData(990);
    }

    public static void GmGmkSsOblongFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(987);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_ss_oblong_obj_3d_list, gimmickData.file_num);
        AppMain.gm_gmk_ss_oblong_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
    }

    private static void gmGmkSsOblongMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)AppMain.GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            obj_work.user_work = AppMain.g_gm_main_system.sync_time + 1U >> 5 & 3U;
            ++obj_work.user_timer;
            if (obj_work.user_timer >= 48)
                obj_work.user_timer = 0;
            AppMain.GmGmkSsSquareBounce(obj_work);
        }
    }

    private static void gmGmkSsOblongDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        if (AppMain.gmGmkSsOblongDrawFunctvx == null)
            AppMain.gmGmkSsOblongDrawFunctvx = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_ss_oblong_obj_tvx_list, 0));
        AppMain.NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        uint dispLightDisable = AppMain.GMD_TVX_DISP_LIGHT_DISABLE;
        uint num1 = 0;
        if (obj_work.dir.z != (ushort)0)
        {
            dispLightDisable |= AppMain.GMD_TVX_DISP_ROTATE;
            num1 = (uint)obj_work.dir.z;
        }
        AppMain.GMS_TVX_EX_WORK ex_work = new AppMain.GMS_TVX_EX_WORK();
        uint num2 = (uint)(obj_work.user_timer / 3);
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.u = 0.125f * (float)(num2 % 8U) + AppMain.gm_gmk_ss_oblong_mat_color[(int)obj_work.user_work].u;
        ex_work.coord.v = 0.125f * (float)(num2 / 8U) + AppMain.gm_gmk_ss_oblong_mat_color[(int)obj_work.user_work].v;
        ex_work.color = uint.MaxValue;
        AppMain.GmTvxSetModelEx(AppMain.gmGmkSsOblongDrawFunctvx, texlist, ref obj_work.pos, ref obj_work.scale, dispLightDisable, (short)num1, ref ex_work);
    }

}