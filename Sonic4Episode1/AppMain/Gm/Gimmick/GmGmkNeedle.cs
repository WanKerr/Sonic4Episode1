public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkNeedleInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_NEEDLE_WORK(), "GMK_NEEDLE_MAIN");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        gmsGmkNeedleWork.needle_type = GmGmkNeedleGetType(eve_rec.id);
        ObjObjectCopyAction3dNNModel(work, gm_gmk_needle_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleDrawFunc);
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)gm_gmk_col_rect_tbl[gmsGmkNeedleWork.needle_type][0];
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)gm_gmk_col_rect_tbl[gmsGmkNeedleWork.needle_type][1];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)gm_gmk_col_rect_tbl[gmsGmkNeedleWork.needle_type][2];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)gm_gmk_col_rect_tbl[gmsGmkNeedleWork.needle_type][3];
        gmsEnemy3DWork.ene_com.col_work.obj_col.dir = (ushort)(16384U * gmsGmkNeedleWork.needle_type);
        work.pos.z = -4096;
        OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkZSet(pRec, (short)gm_gmk_atk_rect_tbl[gmsGmkNeedleWork.needle_type][0], (short)gm_gmk_atk_rect_tbl[gmsGmkNeedleWork.needle_type][1], -500, (short)gm_gmk_atk_rect_tbl[gmsGmkNeedleWork.needle_type][2], (short)gm_gmk_atk_rect_tbl[gmsGmkNeedleWork.needle_type][3], 500);
        if (g_gs_main_sys_info.stage_id == 9)
        {
            if (gmsGmkNeedleWork.needle_type == 1)
                pRec.rect.left -= 16;
            else if (gmsGmkNeedleWork.needle_type == 3)
                pRec.rect.right += 16;
        }
        pRec.flag |= 4U;
        pRec.flag |= 1024U;
        if (g_gs_main_sys_info.stage_id == 14)
        {
            ObjDrawObjectActionSet(work, 0);
            work.obj_3d.use_light_flag &= 4294967294U;
            work.obj_3d.use_light_flag |= 4U;
        }
        work.move_flag |= 8449U;
        work.disp_flag |= 4194304U;
        gmsGmkNeedleWork.state = 0U;
        gmGmkNeedleFwInit(work);
        work.flag |= 1073741824U;
        gmsGmkNeedleWork.color = uint.MaxValue;
        if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
            gmsGmkNeedleWork.color = 4288717055U;
        else if (g_gs_main_sys_info.stage_id == 14)
            gmsGmkNeedleWork.color = 2694881535U;
        return work;
    }

    private static OBS_OBJECT_WORK GmGmkActNeedleInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_NEEDLE_WORK(), "GMK_NEEDLE_ACT_MAIN");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        gmsGmkNeedleWork.needle_type = GmGmkNeedleGetType(eve_rec.id);
        ObjObjectCopyAction3dNNModel(work, gm_gmk_needle_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleDrawFunc);
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)gm_gmk_col_rect_tbl[gmsGmkNeedleWork.needle_type][0];
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)gm_gmk_col_rect_tbl[gmsGmkNeedleWork.needle_type][1];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)gm_gmk_col_rect_tbl[gmsGmkNeedleWork.needle_type][2];
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)gm_gmk_col_rect_tbl[gmsGmkNeedleWork.needle_type][3];
        gmsEnemy3DWork.ene_com.col_work.obj_col.dir = (ushort)(32768 * (gmsGmkNeedleWork.needle_type - 4));
        work.pos.z = -4096;
        OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkZSet(pRec, (short)gm_gmk_atk_rect_tbl[gmsGmkNeedleWork.needle_type][0], (short)gm_gmk_atk_rect_tbl[gmsGmkNeedleWork.needle_type][1], -500, (short)gm_gmk_atk_rect_tbl[gmsGmkNeedleWork.needle_type][2], (short)gm_gmk_atk_rect_tbl[gmsGmkNeedleWork.needle_type][3], 500);
        pRec.flag |= 4U;
        pRec.flag |= 1024U;
        work.move_flag |= 8449U;
        work.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        work.scale.y = 4096;
        amFlagOn(ref work.flag, 2U);
        amFlagOff(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
        gmsGmkNeedleWork.state = 0U;
        gmsGmkNeedleWork.is_first_disp = 1;
        gmsGmkNeedleWork.timer = -30;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkActNeedleFwMain);
        if (g_gs_main_sys_info.stage_id == 14)
        {
            ObjDrawObjectActionSet(work, 0);
            work.obj_3d.use_light_flag &= 4294967294U;
            work.obj_3d.use_light_flag |= 4U;
        }
        gmsGmkNeedleWork.color = uint.MaxValue;
        if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
            gmsGmkNeedleWork.color = 4288717055U;
        else if (g_gs_main_sys_info.stage_id == 14)
            gmsGmkNeedleWork.color = 2694881535U;
        return work;
    }

    private static OBS_OBJECT_WORK GmGmkBackNeedleInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return null;
    }

    private static OBS_OBJECT_WORK GmGmkStandNeedleInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return null;
    }

    public static void GmGmkNeedleBuild()
    {
        gm_gmk_needle_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(822), GmGameDatGetGimmickData(823), 0U);
        gm_gmk_needle_obj_tvx_list = GmGameDatGetGimmickData(824);
        tvx_needle = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_needle_obj_tvx_list, 0));
        tvx_stand = new TVX_FILE((AmbChunk)amBindGet(gm_gmk_needle_obj_tvx_list, 1));
    }

    public static void GmGmkNeedleFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(822));
        GmGameDBuildRegFlushModel(gm_gmk_needle_obj_3d_list, amsAmbHeader.file_num);
        gm_gmk_needle_obj_3d_list = null;
        gm_gmk_needle_obj_tvx_list = null;
    }

    private static void gmGmkNeedleFwInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)(GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkNeedleFwMain);
    }

    private static void gmGmkNeedleFwMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        OBS_RECT_WORK obsRectWork = gmsEnemy3DWork.ene_com.rect_work[1];
        if (gmsGmkNeedleWork.needle_type != 0)
            return;
        OBS_OBJECT_WORK riderObj = gmsEnemy3DWork.ene_com.col_work.obj_col.rider_obj;
        if (riderObj != null && riderObj.ride_obj == (OBS_OBJECT_WORK)gmsEnemy3DWork)
        {
            if (riderObj.obj_type != 1)
                return;
            obsRectWork.flag |= 4U;
        }
        else
            obsRectWork.flag &= 4294967291U;
    }

    private static void gmGmkActNeedleFwInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        if (((GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork).state == 0U)
            amFlagOff(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
        else
            amFlagOn(ref obj_work.flag, 2U);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkActNeedleFwMain);
    }

    private static void gmGmkActNeedleFwMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        OBS_RECT_WORK obsRectWork = gmsEnemy3DWork.ene_com.rect_work[1];
        if (gmsGmkNeedleWork.needle_type == 4)
        {
            OBS_OBJECT_WORK riderObj = gmsEnemy3DWork.ene_com.col_work.obj_col.rider_obj;
            if (riderObj != null && riderObj.ride_obj == (OBS_OBJECT_WORK)gmsEnemy3DWork)
            {
                if (riderObj.obj_type == 1)
                    amFlagOff(ref obj_work.flag, 2U);
            }
            else
                amFlagOn(ref obj_work.flag, 2U);
        }
        if (gmsGmkNeedleWork.timer >= 60)
        {
            gmsGmkNeedleWork.timer = 0;
            gmGmkActNeedleScalingInit(obj_work);
        }
        else
            ++gmsGmkNeedleWork.timer;
        if (((int)gmsGmkNeedleWork.scale_flag & 1) == 0)
            return;
        gmGmkActNeedleSetScaleColRect(obj_work);
    }

    private static void gmGmkActNeedleScalingInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        if (gmsGmkNeedleWork.state == 1U)
        {
            amFlagOn(ref obj_work.flag, 2U);
            amFlagOn(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
            gmsGmkNeedleWork.scale_flag |= 1U;
            gmsGmkNeedleWork.scale_flag |= 4U;
        }
        else if (gmsGmkNeedleWork.is_first_disp == 0)
        {
            obj_work.scale.y = 256;
            gmsGmkNeedleWork.scale_flag |= 1U;
            gmsGmkNeedleWork.scale_flag |= 2U;
        }
        else
        {
            gmsGmkNeedleWork.scale_flag |= 1U;
            gmsGmkNeedleWork.scale_flag |= 4U;
        }
        if (gmsGmkNeedleWork.is_first_disp != 0)
        {
            amFlagOn(ref obj_work.flag, 2U);
            amFlagOn(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
            gmsGmkNeedleWork.state = 1U;
            gmsGmkNeedleWork.is_first_disp = 0;
        }
        amFlagOff(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
        if (gmsGmkNeedleWork.needle_type == 4)
        {
            OBS_OBJECT_WORK riderObj = gmsEnemy3DWork.ene_com.col_work.obj_col.rider_obj;
            if (riderObj != null && riderObj.ride_obj == (OBS_OBJECT_WORK)gmsEnemy3DWork)
            {
                if (riderObj.obj_type == 1)
                    amFlagOff(ref obj_work.flag, 2U);
            }
            else
                amFlagOn(ref obj_work.flag, 2U);
        }
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkActNeedleScalingMain);
    }

    private static void gmGmkActNeedleScalingMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        if (gmsGmkNeedleWork.timer >= 6)
        {
            gmsGmkNeedleWork.timer = 0;
            gmGmkActNeedleRectWaitInit(obj_work);
        }
        else
        {
            amFlagOn(ref obj_work.flag, 2U);
            if (gmsGmkNeedleWork.state == 0U)
                obj_work.scale.y += 682;
            else
                obj_work.scale.y -= 682;
            if (obj_work.scale.y > 4096)
                obj_work.scale.y = 4096;
            else if (obj_work.scale.y <= 0)
                obj_work.scale.y = 0;
            amFlagOff(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
            obj_work.scale.y = MTM_MATH_CLIP(obj_work.scale.y, 0, 4096);
            if (((int)gmsGmkNeedleWork.scale_flag & 1) != 0)
                gmGmkActNeedleSetScaleColRect(obj_work);
            if (gmsGmkNeedleWork.needle_type == 4)
            {
                OBS_OBJECT_WORK riderObj = gmsEnemy3DWork.ene_com.col_work.obj_col.rider_obj;
                if (riderObj != null && riderObj.ride_obj == (OBS_OBJECT_WORK)gmsEnemy3DWork)
                {
                    if (riderObj.obj_type == 1)
                        amFlagOff(ref obj_work.flag, 2U);
                }
                else
                    amFlagOn(ref obj_work.flag, 2U);
            }
            ++gmsGmkNeedleWork.timer;
        }
    }

    private static void gmGmkActNeedleRectWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)(GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkActNeedleRectWaitMain);
    }

    private static void gmGmkActNeedleRectWaitMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        OBS_RECT_WORK obsRectWork = gmsEnemy3DWork.ene_com.rect_work[1];
        amFlagOff(ref gmsEnemy3DWork.ene_com.col_work.obj_col.flag, 256U);
        gmGmkActNeedleFwInit(obj_work);
        gmsGmkNeedleWork.state ^= 1U;
        if (gmsGmkNeedleWork.state == 1U)
            GmSoundPlaySE("Spine");
        if (((int)gmsGmkNeedleWork.scale_flag & 1) == 0)
            return;
        gmGmkActNeedleSetScaleColRect(obj_work);
    }

    private static ushort GmGmkNeedleGetType(ushort type)
    {
        ushort num1;
        if (type < 97)
        {
            ushort num2 = (ushort)(type - 91U);
            num1 = (ushort)gm_gmk_ndl_type_tbl[num2];
        }
        else
        {
            ushort num2 = (ushort)(4U + (ushort)(type - 97U));
            num1 = (ushort)gm_gmk_ndl_type_tbl[num2];
        }
        return num1;
    }

    private static void gmGmkActNeedleSetScaleColRect(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)gmsEnemy3DWork;
        ++gmsGmkNeedleWork.scale_timer;
        if (((int)gmsGmkNeedleWork.scale_flag & 4) != 0)
        {
            int num = gmsEnemy3DWork.ene_com.col_work.obj_col.height - 3;
            if (num < 0)
            {
                num = 0;
                gmsGmkNeedleWork.scale_timer = 0;
                gmsGmkNeedleWork.scale_flag &= 4294967294U;
                gmsGmkNeedleWork.scale_flag &= 4294967291U;
            }
            gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)num;
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)(-1 * gmsEnemy3DWork.ene_com.col_work.obj_col.height);
        }
        else
        {
            if (((int)gmsGmkNeedleWork.scale_flag & 2) == 0)
                return;
            int num = gmsEnemy3DWork.ene_com.col_work.obj_col.height + 4;
            if (num > 32)
            {
                num = 32;
                gmsGmkNeedleWork.scale_timer = 0;
                gmsGmkNeedleWork.scale_flag &= 4294967294U;
                gmsGmkNeedleWork.scale_flag &= 4294967293U;
            }
            gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)num;
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)(-1 * gmsEnemy3DWork.ene_com.col_work.obj_col.height);
        }
    }

    private static void gmGmkNeedleDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        if (!GmMainIsDrawEnable())
            return;
        GMS_GMK_NEEDLE_WORK gmsGmkNeedleWork = (GMS_GMK_NEEDLE_WORK)(GMS_ENEMY_3D_WORK)obj_work;
        if (((int)obj_work.disp_flag & 32) != 0)
            return;
        VecFx32 pos = new VecFx32();
        VecFx32 scale = new VecFx32(4096, 4096, 4096);
        NNS_TEXLIST texlist = obj_work.obj_3d.texlist;
        GMS_TVX_EX_WORK ex_work = new GMS_TVX_EX_WORK();
        short rotate_z = (short)(-49152 * gmsGmkNeedleWork.needle_type);
        if (gmsGmkNeedleWork.needle_type == 5)
            rotate_z = short.MinValue;
        obj_work.dir.z = (ushort)-rotate_z;
        ex_work.u_wrap = 1;
        ex_work.v_wrap = 1;
        ex_work.coord.u = 0.0f;
        ex_work.coord.v = 0.0f;
        ex_work.color = gmsGmkNeedleWork.color;
        for (int index = 0; index < 5; ++index)
        {
            pos.z = obj_work.pos.z;
            if (index >= 3)
                pos.z -= 8192;
            switch (gmsGmkNeedleWork.needle_type)
            {
                case 0:
                case 4:
                    pos.x = obj_work.pos.x + gm_gmk_disp_ofst_tbl_u[index][0];
                    pos.y = obj_work.pos.y + gm_gmk_disp_ofst_tbl_u[index][1];
                    break;
                case 1:
                case 5:
                    pos.x = obj_work.pos.x + gm_gmk_disp_ofst_tbl_l[index][0];
                    pos.y = obj_work.pos.y + gm_gmk_disp_ofst_tbl_l[index][1];
                    break;
                case 2:
                    pos.x = obj_work.pos.x + gm_gmk_disp_ofst_tbl_d[index][0];
                    pos.y = obj_work.pos.y + gm_gmk_disp_ofst_tbl_d[index][1];
                    break;
                case 3:
                    pos.x = obj_work.pos.x + gm_gmk_disp_ofst_tbl_r[index][0];
                    pos.y = obj_work.pos.y + gm_gmk_disp_ofst_tbl_r[index][1];
                    break;
            }
            GmTvxSetModelEx(tvx_needle, texlist, ref pos, ref obj_work.scale, GMD_TVX_DISP_ROTATE | GMD_TVX_DISP_SCALE | GMD_TVX_DISP_LIGHT_DISABLE, rotate_z, ref ex_work);
            GmTvxSetModel(tvx_stand, texlist, ref pos, ref scale, GMD_TVX_DISP_ROTATE | GMD_TVX_DISP_SCALE, rotate_z);
        }
    }

    public static void GmGmkNeedleSetLight()
    {
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        if (g_gs_main_sys_info.stage_id == 14)
        {
            nnsVector.x = -0.1f;
            nnsVector.y = -0.09f;
            nnsVector.z = -0.93f;
        }
        else
        {
            nnsVector.x = -1f;
            nnsVector.y = -1f;
            nnsVector.z = -1f;
        }
        col.r = 1f;
        col.g = 1f;
        col.b = 1f;
        nnNormalizeVector(nnsVector, nnsVector);
        float intensity = g_gs_main_sys_info.stage_id != 14 ? 1f : 0.8f;
        ObjDrawSetParallelLight(NNE_LIGHT_2, ref col, intensity, nnsVector);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

}