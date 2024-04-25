public partial class AppMain
{
    private static void GmGmkSsSquareBuild()
    {
        gm_gmk_ss_square_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(896), GmGameDatGetGimmickData(897), 0U);
        gm_gmk_ss_square_obj_tvx_list = GmGameDatGetGimmickData(899);
    }

    private static void GmGmkSsSquareFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(896);
        GmGameDBuildRegFlushModel(gm_gmk_ss_square_obj_3d_list, gimmickData.file_num);
        gm_gmk_ss_square_obj_tvx_list = null;
    }

    private static OBS_OBJECT_WORK GmGmkSsSquareInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_SS_SQUARE");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= 128;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_ss_square_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.user_flag = work.user_work = (uint)((eve_rec.flag & 3) + 1);
        work.user_timer = MTM_MATH_CLIP(eve_rec.left, 0, 8);
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsSquareMain);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsSquareDrawFunc);
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

    private static void GmGmkSsSquareBounce(OBS_OBJECT_WORK obj_work)
    {
        OBS_COLLISION_OBJ objCol = obj_work.col_work.obj_col;
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        GMS_SPL_STG_WORK work = GmSplStageGetWork();
        if (objCol.toucher_obj == ply_work.obj_work)
        {
            if (ply_work.nudge_timer != 0 && ((int)work.flag & 2) == 0)
            {
                GmPlySeqInitPinballAir(ply_work, 0, -17408, 5, false);
                work.flag |= 1U;
                work.flag |= 2U;
            }
            else if (((int)obj_work.user_flag & int.MinValue) == 0 && ((int)work.flag & 1) == 0 && (MTM_MATH_ABS(ply_work.obj_work.spd.x) > 4096 || MTM_MATH_ABS(ply_work.obj_work.spd.y) > 4096))
            {
                OBS_OBJECT_WORK objWork = ply_work.obj_work;
                VecFx32 vecFx32 = gmGmkSsSquareNormalizeVectorXY(new VecFx32()
                {
                    x = objWork.prev_pos.x - obj_work.pos.x,
                    y = objWork.prev_pos.y - obj_work.pos.y,
                    z = 0
                });
                objWork.dir.z = 0;
                int num1 = MTM_MATH_ABS(objWork.spd.x);
                int num2 = MTM_MATH_ABS(objWork.spd.y);
                int v2 = FX_Sqrt(FX_Mul(num1, num1) + FX_Mul(num2, num2)) / 2;
                int spd_x = FX_Mul(vecFx32.x, v2);
                int spd_y = FX_Mul(vecFx32.y, v2);
                GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, 5, false);
                work.flag |= 1U;
            }
            obj_work.user_flag |= 2147483648U;
        }
        else
            obj_work.user_flag &= int.MaxValue;
    }

    private static void gmGmkSsSquareMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            uint num1 = (uint)((obj_work.user_timer >> 3 & sbyte.MaxValue) + 1);
            if (num1 >= 120U)
                num1 = 0U;
            obj_work.user_timer = (int)((long)(obj_work.user_timer & 7) | (num1 & sbyte.MaxValue) << 3);
            if (obj_work.user_timer != 0)
            {
                uint num2 = (uint)(obj_work.user_timer - 1 & 7);
                uint num3 = (uint)((int)num2 + 2 & 7);
                uint num4 = g_gm_main_system.sync_time >> 3 & 7U;
                obj_work.user_work = (int)num2 == (int)num4 || (int)num3 == (int)num4 ? (uint)(((int)obj_work.user_flag - 2 & 3) + 1) : obj_work.user_flag & 7U;
            }
            GmGmkSsSquareBounce(obj_work);
        }
    }

    private static VecFx32 gmGmkSsSquareNormalizeVectorXY(VecFx32 vec)
    {
        VecFx32 vecFx32 = new VecFx32();
        int denom = FX_Sqrt(FX_Mul(vec.x, vec.x) + FX_Mul(vec.y, vec.y));
        if (denom == 0)
        {
            vecFx32.x = 4096;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = FX_Div(4096, denom);
            vecFx32.x = FX_Mul(vec.x, v2);
            vecFx32.y = FX_Mul(vec.y, v2);
        }
        vecFx32.z = 0;
        int dest_x = 0;
        int dest_y = 0;
        ObjUtilGetRotPosXY(vecFx32.x, vecFx32.y, ref dest_x, ref dest_y, (ushort)-g_gm_main_system.pseudofall_dir);
        vecFx32.x = dest_x;
        vecFx32.y = dest_y;
        return vecFx32;
    }

    private static void gmGmkSsSquareDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        TVX_FILE model_tvx;
        if (gm_gmk_ss_square_obj_tvx_list.buf[0] == null)
        {
            model_tvx = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_ss_square_obj_tvx_list, 0));
            gm_gmk_ss_square_obj_tvx_list.buf[0] = model_tvx;
        }
        else
            model_tvx = (TVX_FILE)gm_gmk_ss_square_obj_tvx_list.buf[0];
        NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        GMS_TVX_EX_WORK ex_work = new GMS_TVX_EX_WORK();
        uint num = (uint)(obj_work.user_timer >> 5 & 31);
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.u = 0.125f * (gm_gmk_ss_square_uv_parameter[(int)num] % 4) + gm_gmk_ss_square_mat_color[(int)(obj_work.user_work - 1U)].u;
        ex_work.coord.v = 0.125f * (gm_gmk_ss_square_uv_parameter[(int)num] / 4) + gm_gmk_ss_square_mat_color[(int)(obj_work.user_work - 1U)].v;
        ex_work.color = uint.MaxValue;
        GmTvxSetModelEx(model_tvx, texlist, ref obj_work.pos, ref obj_work.scale, GMD_TVX_DISP_LIGHT_DISABLE, 0, ref ex_work);
    }
}