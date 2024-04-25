public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSsGoalInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_SS_GOAL");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= 128;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_ss_goal_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsGoalMain);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsGoalDrawFunc);
        OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work;
        colWork.obj_col.diff_data = g_gm_default_col;
        colWork.obj_col.width = 24;
        colWork.obj_col.height = 24;
        colWork.obj_col.ofst_x = (short)-(colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-(colWork.obj_col.height / 2);
        colWork.obj_col.attr = 2;
        colWork.obj_col.flag |= 134217760U;
        return work;
    }

    public static void GmGmkSsGoalBuild()
    {
        gm_gmk_ss_goal_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(907), GmGameDatGetGimmickData(908), 0U);
        gm_gmk_ss_goal_obj_tvx_list = GmGameDatGetGimmickData(909);
    }

    public static void GmGmkSsGoalFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(907);
        GmGameDBuildRegFlushModel(gm_gmk_ss_goal_obj_3d_list, gimmickData.file_num);
        gm_gmk_ss_goal_obj_tvx_list = null;
    }

    private static void gmGmkSsGoalMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_COLLISION_OBJ objCol = obj_work.col_work.obj_col;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (((int)GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            obj_work.dir.z = GmMainGetObjectRotation();
            if (objCol.toucher_obj != gmsPlayerWork.obj_work ||
                objCol.toucher_obj.touch_obj != obj_work ||
                ((int)g_gm_main_system.game_flag & GMD_GAME_FLAG_SPL_FAILED) != 0)
            {
                return;
            }

            g_gm_main_system.game_flag |= GMD_GAME_FLAG_SPL_FAILED;
            GmSoundPlaySE("Special4");
        }
    }

    private static void gmGmkSsGoalDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        TVX_FILE model_tvx;
        if (gm_gmk_ss_goal_obj_tvx_list.buf[0] == null)
        {
            model_tvx = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_ss_goal_obj_tvx_list, 0));
            gm_gmk_ss_goal_obj_tvx_list.buf[0] = model_tvx;
        }
        else
            model_tvx = (TVX_FILE)gm_gmk_ss_goal_obj_tvx_list.buf[0];
        NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        GmTvxSetModel(model_tvx, texlist, ref obj_work.pos, ref obj_work.scale, GMD_TVX_DISP_LIGHT_DISABLE | GMD_TVX_DISP_ROTATE, (short)-obj_work.dir.z);
    }

}