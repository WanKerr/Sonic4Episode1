public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSsOblongInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_SS_OBLONG");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= 128;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_ss_oblong_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.user_flag = 0U;
        work.user_work = 0U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsOblongMain);
        work.user_timer = 0;
        if ((eve_rec.flag & 1) != 0)
            work.dir.z = 16384;
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsOblongDrawFunc);
        OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work;
        colWork.obj_col.diff_data = g_gm_default_col;
        if ((eve_rec.flag & 1) != 0)
        {
            colWork.obj_col.width = 24;
            colWork.obj_col.height = 48;
        }
        else
        {
            colWork.obj_col.width = 48;
            colWork.obj_col.height = 24;
        }
        colWork.obj_col.ofst_x = (short)-(colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-(colWork.obj_col.height / 2);
        colWork.obj_col.attr = 2;
        colWork.obj_col.flag |= 134217760U;
        return work;
    }

    public static void GmGmkSsOblongBuild()
    {
        gm_gmk_ss_oblong_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(987), GmGameDatGetGimmickData(988), 0U);
        gm_gmk_ss_oblong_obj_tvx_list = GmGameDatGetGimmickData(990);
    }

    public static void GmGmkSsOblongFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(987);
        GmGameDBuildRegFlushModel(gm_gmk_ss_oblong_obj_3d_list, gimmickData.file_num);
        gm_gmk_ss_oblong_obj_tvx_list = null;
    }

    private static void gmGmkSsOblongMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            obj_work.user_work = g_gm_main_system.sync_time + 1U >> 5 & 3U;
            ++obj_work.user_timer;
            if (obj_work.user_timer >= 48)
                obj_work.user_timer = 0;
            GmGmkSsSquareBounce(obj_work);
        }
    }

    private static void gmGmkSsOblongDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        if (gmGmkSsOblongDrawFunctvx == null)
            gmGmkSsOblongDrawFunctvx = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_ss_oblong_obj_tvx_list, 0));
        NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        uint dispLightDisable = GMD_TVX_DISP_LIGHT_DISABLE;
        uint num1 = 0;
        if (obj_work.dir.z != 0)
        {
            dispLightDisable |= GMD_TVX_DISP_ROTATE;
            num1 = obj_work.dir.z;
        }
        GMS_TVX_EX_WORK ex_work = new GMS_TVX_EX_WORK();
        uint num2 = (uint)(obj_work.user_timer / 3);
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.u = 0.125f * (num2 % 8U) + gm_gmk_ss_oblong_mat_color[(int)obj_work.user_work].u;
        ex_work.coord.v = 0.125f * (num2 / 8U) + gm_gmk_ss_oblong_mat_color[(int)obj_work.user_work].v;
        ex_work.color = uint.MaxValue;
        GmTvxSetModelEx(gmGmkSsOblongDrawFunctvx, texlist, ref obj_work.pos, ref obj_work.scale, dispLightDisable, (short)num1, ref ex_work);
    }

}