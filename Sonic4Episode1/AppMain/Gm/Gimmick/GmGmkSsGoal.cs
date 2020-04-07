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
    private static AppMain.OBS_OBJECT_WORK GmGmkSsGoalInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SS_GOAL");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= (short)128;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_ss_goal_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsGoalMain);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsGoalDrawFunc);
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

    public static void GmGmkSsGoalBuild()
    {
        AppMain.gm_gmk_ss_goal_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(907), AppMain.GmGameDatGetGimmickData(908), 0U);
        AppMain.gm_gmk_ss_goal_obj_tvx_list = AppMain.GmGameDatGetGimmickData(909);
    }

    public static void GmGmkSsGoalFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(907);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_ss_goal_obj_3d_list, gimmickData.file_num);
        AppMain.gm_gmk_ss_goal_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
    }

    private static void gmGmkSsGoalMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_COLLISION_OBJ objCol = obj_work.col_work.obj_col;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (((int)AppMain.GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            obj_work.dir.z = AppMain.GmMainGetObjectRotation();
            if (objCol.toucher_obj != gmsPlayerWork.obj_work || objCol.toucher_obj.touch_obj != obj_work || ((int)AppMain.g_gm_main_system.game_flag & 131072) != 0)
                return;
            AppMain.g_gm_main_system.game_flag |= 131072U;
            AppMain.GmSoundPlaySE("Special4");
        }
    }

    private static void gmGmkSsGoalDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.TVX_FILE model_tvx;
        if (AppMain.gm_gmk_ss_goal_obj_tvx_list.buf[0] == null)
        {
            model_tvx = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.gm_gmk_ss_goal_obj_tvx_list, 0));
            AppMain.gm_gmk_ss_goal_obj_tvx_list.buf[0] = (object)model_tvx;
        }
        else
            model_tvx = (AppMain.TVX_FILE)AppMain.gm_gmk_ss_goal_obj_tvx_list.buf[0];
        AppMain.NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        AppMain.GmTvxSetModel(model_tvx, texlist, ref obj_work.pos, ref obj_work.scale, AppMain.GMD_TVX_DISP_LIGHT_DISABLE | AppMain.GMD_TVX_DISP_ROTATE, (short)-obj_work.dir.z);
    }

}