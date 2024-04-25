public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkSsEnduranceInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_SS_ENDURANCE");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= 128;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_ss_endurance_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.user_flag = (uint)((~eve_rec.flag & 3) + 1) - eve_rec.byte_param[1];
        work.user_work = gmGmkSsEnduranceColorSet(work.user_flag);
        gmGmkSsEnduranceScaleSet(work);
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsEnduranceWait);
        work.user_timer = 0;
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsEnduranceDrawFunc);
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

    public static void GmGmkSsEnduranceBuild()
    {
        gm_gmk_ss_endurance_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(903), GmGameDatGetGimmickData(904), 0U);
        gm_gmk_ss_endurance_obj_tvx_list = GmGameDatGetGimmickData(906);
    }

    public static void GmGmkSsEnduranceFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(903);
        GmGameDBuildRegFlushModel(gm_gmk_ss_endurance_obj_3d_list, gimmickData.file_num);
        gm_gmk_ss_endurance_obj_tvx_list = null;
    }

    private static void gmGmkSsEnduranceWait(OBS_OBJECT_WORK obj_work)
    {
        OBS_COLLISION_OBJ objCol = obj_work.col_work.obj_col;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (((int)GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            if (objCol.toucher_obj == gmsPlayerWork.obj_work)
            {
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsEnduranceDamage);
                obj_work.user_timer |= 30;
                GmSoundPlaySE("Special3");
            }
            gmGmkSsEnduranceUpdateUVTimer(obj_work);
            GmGmkSsSquareBounce(obj_work);
        }
    }

    private static void gmGmkSsEnduranceDamage(OBS_OBJECT_WORK obj_work)
    {
        if (((int)GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            uint num = (uint)(obj_work.user_timer & byte.MaxValue) - 1U;
            obj_work.user_timer = (int)(obj_work.user_timer & 4294967040L | num & byte.MaxValue);
            if (num == 0U)
            {
                --obj_work.user_flag;
                ++((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.byte_param[1];
                if (((int)obj_work.user_flag & int.MaxValue) != 0)
                {
                    gmGmkSsEnduranceScaleSet(obj_work);
                    obj_work.user_work = gmGmkSsEnduranceColorSet(obj_work.user_flag & int.MaxValue);
                    obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsEnduranceWait);
                }
                else
                {
                    obj_work.flag |= 4U;
                    ((GMS_ENEMY_3D_WORK)obj_work).ene_com.enemy_flag |= 65536U;
                    GmEfctZoneEsCreate(obj_work, 5, 8).efct_com.obj_work.flag |= 512U;
                }
            }
            else
            {
                uint color_num = (uint)(((obj_work.user_timer & 12) >> 2) + 1);
                obj_work.user_work = gmGmkSsEnduranceColorSet(color_num);
            }
            gmGmkSsEnduranceUpdateUVTimer(obj_work);
            GmGmkSsSquareBounce(obj_work);
        }
    }

    private static void gmGmkSsEnduranceDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        TVX_FILE model_tvx;
        if (gm_gmk_ss_endurance_obj_tvx_list.buf[0] == null)
        {
            model_tvx = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_ss_endurance_obj_tvx_list, 0));
            gm_gmk_ss_endurance_obj_tvx_list.buf[0] = model_tvx;
        }
        else
            model_tvx = (TVX_FILE)gm_gmk_ss_endurance_obj_tvx_list.buf[0];
        NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        GMS_TVX_EX_WORK ex_work = new GMS_TVX_EX_WORK();
        uint num = (uint)obj_work.user_timer >> 10 & 31U;
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.u = 0.125f * (gm_gmk_ss_endurance_uv_parameter[(int)num] % 4) + gm_gmk_ss_endurance_mat_color[(int)obj_work.user_work].u;
        ex_work.coord.v = 0.125f * (gm_gmk_ss_endurance_uv_parameter[(int)num] / 4) + gm_gmk_ss_endurance_mat_color[(int)obj_work.user_work].v;
        ex_work.color = uint.MaxValue;
        GmTvxSetModelEx(model_tvx, texlist, ref obj_work.pos, ref obj_work.scale, GMD_TVX_DISP_SCALE | GMD_TVX_DISP_LIGHT_DISABLE, 0, ref ex_work);
    }

    private static void gmGmkSsEnduranceUpdateUVTimer(OBS_OBJECT_WORK obj_work)
    {
        uint num = ((uint)obj_work.user_timer >> 8 & (uint)sbyte.MaxValue) + 1U;
        if (num >= 120U)
            num = 0U;
        obj_work.user_timer = (int)((long)(obj_work.user_timer & byte.MaxValue) | num << 8);
    }

    private static uint gmGmkSsEnduranceColorSet(uint color_num)
    {
        return gm_gmk_ss_endurance_color[(int)color_num];
    }

    private static void gmGmkSsEnduranceScaleSet(OBS_OBJECT_WORK obj_work)
    {
        int num = 4096;
        switch (obj_work.user_flag & int.MaxValue)
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
        gm_gmk_ss_circle_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(900)), readAMBFile(GmGameDatGetGimmickData(901)), 0U);
        gm_gmk_ss_circle_obj_tvx_list = readAMBFile(GmGameDatGetGimmickData(902));
    }

    private static void GmGmkSsCircleFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(900));
        GmGameDBuildRegFlushModel(gm_gmk_ss_circle_obj_3d_list, amsAmbHeader.file_num);
        gm_gmk_ss_circle_obj_tvx_list = null;
    }

    private static OBS_OBJECT_WORK GmGmkSsCircleInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_SS_CIRCLE");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= 128;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_ss_circle_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.user_flag = 0U;
        if (eve_rec.id == 194)
        {
            work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsOnewayMain);
            work.disp_flag |= 134217728U;
            work.obj_3d.drawflag |= 8388608U;
            work.obj_3d.draw_state.alpha.alpha = 0.5f;
        }
        else
        {
            work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsCircleMain);
            OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
            colWork.obj_col.obj = work;
            colWork.obj_col.diff_data = g_gm_default_col;
            colWork.obj_col.width = 24;
            colWork.obj_col.height = 24;
            colWork.obj_col.ofst_x = (short)-(colWork.obj_col.width / 2);
            colWork.obj_col.ofst_y = (short)-(colWork.obj_col.height / 2);
            colWork.obj_col.attr = 2;
            colWork.obj_col.flag |= 134217760U;
        }
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsCircleDrawFunc);
        return work;
    }

    private static void GmGmkSsOnewayThrough(uint sw_no)
    {
        GmSplStageSwSet(sw_no);
    }

    private static void gmGmkSsCircleMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            if (((int)g_gm_main_system.sync_time & 15) == 0)
                obj_work.dir.z += 8192;
            GmGmkSsSquareBounce(obj_work);
        }
    }

    private static void gmGmkSsOnewayMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (((int)GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            if (GmSplStageSwCheck(gmsEnemy3DWork.ene_com.eve_rec.flag))
            {
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSsCircleMain);
                OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
                colWork.obj_col.obj = obj_work;
                colWork.obj_col.diff_data = g_gm_default_col;
                colWork.obj_col.width = 24;
                colWork.obj_col.height = 24;
                colWork.obj_col.ofst_x = (short)-(colWork.obj_col.width / 2);
                colWork.obj_col.ofst_y = (short)-(colWork.obj_col.height / 2);
                colWork.obj_col.attr = 2;
                colWork.obj_col.flag |= 134217760U;
                obj_work.disp_flag &= 4160749567U;
                obj_work.obj_3d.drawflag &= 4286578687U;
                obj_work.obj_3d.draw_state.alpha.alpha = 1f;
            }
            GmGmkSsSquareBounce(obj_work);
        }
    }

    private static void gmGmkSsCircleDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        TVX_FILE model_tvx;
        if (gm_gmk_ss_circle_obj_tvx_list.buf[0] == null)
        {
            model_tvx = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_ss_circle_obj_tvx_list, 0));
            gm_gmk_ss_circle_obj_tvx_list.buf[0] = model_tvx;
        }
        else
            model_tvx = (TVX_FILE)gm_gmk_ss_circle_obj_tvx_list.buf[0];
        NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        uint flag = GMD_TVX_DISP_LIGHT_DISABLE | GMD_TVX_DISP_ROTATE;
        GMS_TVX_EX_WORK ex_work = new GMS_TVX_EX_WORK();
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.u = 0.0f;
        ex_work.coord.v = 0.0f;
        ex_work.color = uint.MaxValue;
        if (obj_work.obj_3d.draw_state.alpha.alpha == 0.5)
        {
            ex_work.color = 4294967176U;
            flag |= GMD_TVX_DISP_BLEND;
        }
        GmTvxSetModelEx(model_tvx, texlist, ref obj_work.pos, ref obj_work.scale, flag, (short)-obj_work.dir.z, ref ex_work);
    }

}