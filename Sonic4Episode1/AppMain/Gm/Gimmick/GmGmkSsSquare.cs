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
    private static void GmGmkSsSquareBuild()
    {
        AppMain.gm_gmk_ss_square_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(896), AppMain.GmGameDatGetGimmickData(897), 0U);
        AppMain.gm_gmk_ss_square_obj_tvx_list = AppMain.GmGameDatGetGimmickData(899);
    }

    private static void GmGmkSsSquareFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(896);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_ss_square_obj_3d_list, gimmickData.file_num);
        AppMain.gm_gmk_ss_square_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSsSquareInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SS_SQUARE");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= (short)128;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_ss_square_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.user_flag = work.user_work = (uint)(((int)eve_rec.flag & 3) + 1);
        work.user_timer = AppMain.MTM_MATH_CLIP((int)eve_rec.left, 0, 8);
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsSquareMain);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsSquareDrawFunc);
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

    private static void GmGmkSsSquareBounce(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_COLLISION_OBJ objCol = obj_work.col_work.obj_col;
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        AppMain.GMS_SPL_STG_WORK work = AppMain.GmSplStageGetWork();
        if (objCol.toucher_obj == ply_work.obj_work)
        {
            if (ply_work.nudge_timer != (short)0 && ((int)work.flag & 2) == 0)
            {
                AppMain.GmPlySeqInitPinballAir(ply_work, 0, -17408, 5, false);
                work.flag |= 1U;
                work.flag |= 2U;
            }
            else if (((int)obj_work.user_flag & int.MinValue) == 0 && ((int)work.flag & 1) == 0 && (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) > 4096 || AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.y) > 4096))
            {
                AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
                AppMain.VecFx32 vecFx32 = AppMain.gmGmkSsSquareNormalizeVectorXY(new AppMain.VecFx32()
                {
                    x = objWork.prev_pos.x - obj_work.pos.x,
                    y = objWork.prev_pos.y - obj_work.pos.y,
                    z = 0
                });
                objWork.dir.z = (ushort)0;
                int num1 = AppMain.MTM_MATH_ABS(objWork.spd.x);
                int num2 = AppMain.MTM_MATH_ABS(objWork.spd.y);
                int v2 = AppMain.FX_Sqrt(AppMain.FX_Mul(num1, num1) + AppMain.FX_Mul(num2, num2)) / 2;
                int spd_x = AppMain.FX_Mul(vecFx32.x, v2);
                int spd_y = AppMain.FX_Mul(vecFx32.y, v2);
                AppMain.GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, 5, false);
                work.flag |= 1U;
            }
            obj_work.user_flag |= 2147483648U;
        }
        else
            obj_work.user_flag &= (uint)int.MaxValue;
    }

    private static void gmGmkSsSquareMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)AppMain.GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            uint num1 = (uint)((obj_work.user_timer >> 3 & (int)sbyte.MaxValue) + 1);
            if (num1 >= 120U)
                num1 = 0U;
            obj_work.user_timer = (int)((long)(obj_work.user_timer & 7) | ((long)num1 & (long)sbyte.MaxValue) << 3);
            if (obj_work.user_timer != 0)
            {
                uint num2 = (uint)(obj_work.user_timer - 1 & 7);
                uint num3 = (uint)((int)num2 + 2 & 7);
                uint num4 = AppMain.g_gm_main_system.sync_time >> 3 & 7U;
                obj_work.user_work = (int)num2 == (int)num4 || (int)num3 == (int)num4 ? (uint)(((int)obj_work.user_flag - 2 & 3) + 1) : obj_work.user_flag & 7U;
            }
            AppMain.GmGmkSsSquareBounce(obj_work);
        }
    }

    private static AppMain.VecFx32 gmGmkSsSquareNormalizeVectorXY(AppMain.VecFx32 vec)
    {
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        int denom = AppMain.FX_Sqrt(AppMain.FX_Mul(vec.x, vec.x) + AppMain.FX_Mul(vec.y, vec.y));
        if (denom == 0)
        {
            vecFx32.x = 4096;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = AppMain.FX_Div(4096, denom);
            vecFx32.x = AppMain.FX_Mul(vec.x, v2);
            vecFx32.y = AppMain.FX_Mul(vec.y, v2);
        }
        vecFx32.z = 0;
        int dest_x = 0;
        int dest_y = 0;
        AppMain.ObjUtilGetRotPosXY(vecFx32.x, vecFx32.y, ref dest_x, ref dest_y, (ushort)-AppMain.g_gm_main_system.pseudofall_dir);
        vecFx32.x = dest_x;
        vecFx32.y = dest_y;
        return vecFx32;
    }

    private static void gmGmkSsSquareDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.TVX_FILE model_tvx;
        if (AppMain.gm_gmk_ss_square_obj_tvx_list.buf[0] == null)
        {
            model_tvx = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_ss_square_obj_tvx_list, 0));
            AppMain.gm_gmk_ss_square_obj_tvx_list.buf[0] = (object)model_tvx;
        }
        else
            model_tvx = (AppMain.TVX_FILE)AppMain.gm_gmk_ss_square_obj_tvx_list.buf[0];
        AppMain.NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        AppMain.GMS_TVX_EX_WORK ex_work = new AppMain.GMS_TVX_EX_WORK();
        uint num = (uint)(obj_work.user_timer >> 5 & 31);
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.u = 0.125f * (float)((int)AppMain.gm_gmk_ss_square_uv_parameter[(int)num] % 4) + AppMain.gm_gmk_ss_square_mat_color[(int)(obj_work.user_work - 1U)].u;
        ex_work.coord.v = 0.125f * (float)((int)AppMain.gm_gmk_ss_square_uv_parameter[(int)num] / 4) + AppMain.gm_gmk_ss_square_mat_color[(int)(obj_work.user_work - 1U)].v;
        ex_work.color = uint.MaxValue;
        AppMain.GmTvxSetModelEx(model_tvx, texlist, ref obj_work.pos, ref obj_work.scale, AppMain.GMD_TVX_DISP_LIGHT_DISABLE, (short)0, ref ex_work);
    }
}