public partial class AppMain
{
    private static OBS_OBJECT_WORK GmBoss5LandInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_BOSS5_LAND_PLACEMENT_INFO place_info = new GMS_BOSS5_LAND_PLACEMENT_INFO();
        if (gmBoss5LandGetPlacementInfo(place_info) == 0)
            return null;
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, place_info.pos_x, place_info.pos_y, () => new GMS_BOSS5_LAND_WORK(), "BOSS5_LAND");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS5_LAND_WORK land_work = (GMS_BOSS5_LAND_WORK)work;
        work.pos.z = -524288;
        work.flag |= 16U;
        work.flag &= 4294966271U;
        work.disp_flag &= 4294967263U;
        work.disp_flag |= 4194304U;
        work.move_flag |= 8448U;
        work.move_flag &= 4294967167U;
        for (int part_index = 0; part_index < place_info.part_num; ++part_index)
        {
            uint num = (uint)((ulong)part_index % 3UL);
            gmBoss5LandCreateLdPart(land_work, gm_boss5_land_place_pattern_tbl[(int)num], part_index);
        }
        gmBoss5LandSetObjCollisionRect(land_work, place_info.part_num);
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5LandMain);
        gmBoss5LandProcInit(land_work);
        return work;
    }

    private static GMS_BOSS5_LAND_WORK GmBoss5LandCreate(
      GMS_BOSS5_MGR_WORK mgr_work)
    {
        OBS_OBJECT_WORK obsObjectWork1 = GMM_BS_OBJ(mgr_work);
        OBS_OBJECT_WORK obsObjectWork2 = GmEventMgrLocalEventBirth(344, obsObjectWork1.pos.x, obsObjectWork1.pos.y, 0, 0, 0, 0, 0, 0);
        if (obsObjectWork2 == null)
            return null;
        ((GMS_BOSS5_LAND_WORK)obsObjectWork2).mgr_work = mgr_work;
        return (GMS_BOSS5_LAND_WORK)obsObjectWork2;
    }

    private static int gmBoss5LandGetPlacementInfo(GMS_BOSS5_LAND_PLACEMENT_INFO place_info)
    {
        GMS_EVE_RECORD_EVENT gmsEveRecordEvent = null;
        OBS_OBJECT_WORK obj_work;
        for (obj_work = ObjObjectSearchRegistObject(null, 3); obj_work != null; obj_work = ObjObjectSearchRegistObject(obj_work, 3))
        {
            GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
            if (gmsEnemyComWork.eve_rec != null && gmsEnemyComWork.eve_rec.id == 282)
            {
                gmsEveRecordEvent = gmsEnemyComWork.eve_rec;
                break;
            }
        }
        if (obj_work == null)
        {
            mppAssertNotImpl();
            return 0;
        }
        place_info.pos_x = obj_work.pos.x;
        place_info.pos_y = obj_work.pos.y;
        int num = gmsEveRecordEvent.left + gmsEveRecordEvent.width << 3;
        place_info.part_num = (int)(num / GMD_BOSS5_LAND_LDPART_WIDTH_INT);
        return 1;
    }

    private static void gmBoss5LandSetObjCollisionRect(
      GMS_BOSS5_LAND_WORK land_work,
      int part_num)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(land_work);
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)land_work;
        gmsEnemyComWork.col_work.obj_col.obj = GMM_BS_OBJ(land_work);
        gmsEnemyComWork.col_work.obj_col.width = (ushort)((ulong)part_num * GMD_BOSS5_LAND_LDPART_WIDTH_INT);
        gmsEnemyComWork.col_work.obj_col.height = GMD_BOSS5_LAND_LDPART_HEIGHT_INT;
        gmsEnemyComWork.col_work.obj_col.ofst_x = 0;
        gmsEnemyComWork.col_work.obj_col.ofst_y = 0;
        obsObjectWork.view_out_ofst_plus[0] = (short)(gmsEnemyComWork.col_work.obj_col.ofst_x - (short)gmsEnemyComWork.col_work.obj_col.width);
        obsObjectWork.view_out_ofst_plus[2] = (short)(gmsEnemyComWork.col_work.obj_col.ofst_x + (short)gmsEnemyComWork.col_work.obj_col.width);
    }

    private static void gmBoss5LandDisableObjCollision(GMS_BOSS5_LAND_WORK land_work)
    {
        ((GMS_ENEMY_COM_WORK)land_work).col_work.obj_col.obj = null;
    }

    private static void gmBoss5LandMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_LAND_WORK wrk = (GMS_BOSS5_LAND_WORK)obj_work;
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    private static void gmBoss5LandProcInit(GMS_BOSS5_LAND_WORK land_work)
    {
        land_work.proc_update = new MPP_VOID_GMS_BOSS5_LAND_WORK(gmBoss5LandProcUpdateIdle);
    }

    private static void gmBoss5LandProcUpdateIdle(GMS_BOSS5_LAND_WORK land_work)
    {
        if (((int)land_work.mgr_work.flag & 536870912) == 0)
            return;
        land_work.flag |= GMD_BOSS5_LAND_FLAG_SHAKE_ACTIVE;
        land_work.proc_update = new MPP_VOID_GMS_BOSS5_LAND_WORK(gmBoss5LandProcUpdateShake);
    }

    private static void gmBoss5LandProcUpdateShake(GMS_BOSS5_LAND_WORK land_work)
    {
        if (((int)land_work.mgr_work.flag & 1073741824) == 0)
            return;
        land_work.flag |= GMD_BOSS5_LAND_FLAG_BREAK_ACTIVE;
        gmBoss5LandDisableObjCollision(land_work);
        land_work.proc_update = null;
    }

    private static GMS_BOSS5_LDPART_WORK gmBoss5LandCreateLdPart(
      GMS_BOSS5_LAND_WORK land_work,
      int land_type,
      int part_index)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_BOSS5_LDPART_WORK(), GMM_BS_OBJ(land_work), 0, "BOSS5_LAND_PART");
        GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (GMS_EFFECT_3DNN_WORK)work;
        GMS_BOSS5_LDPART_WORK ldpart_work = (GMS_BOSS5_LDPART_WORK)work;
        ldpart_work.part_index = part_index;
        ObjObjectCopyAction3dNNModel(work, GmBoss5GetObject3dList()[gm_boss5_land_mdl_amb_idx_tbl[land_type]], gmsEffect3DnnWork.obj_3d);
        work.obj_3d.drawflag = (uint)(work.obj_3d.drawflag & ulong.MaxValue);
        ObjObjectAction3dNNMaterialMotionLoad(work, 0, ObjDataGet(gm_boss5_land_mat_mtn_dwork_no_tbl[land_type]), null, gm_boss5_land_mat_mtn_data_tbl[land_type], null);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.flag |= 1024U;
        work.flag |= 18U;
        work.disp_flag |= 4194304U;
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        work.parent_ofst.x = GMD_BOSS5_LAND_LDPART_WIDTH_FX / 2 + part_index * GMD_BOSS5_LAND_LDPART_WIDTH_FX + GMD_BOSS5_LAND_LDPART_CENTER_OFST_X_FX;
        work.parent_ofst.y = GMD_BOSS5_LAND_LDPART_HEIGHT_FX / 2 + GMD_BOSS5_LAND_LDPART_CENTER_OFST_Y_FX;
        work.parent_ofst.z = 0;
        ldpart_work.pivot_parent_ofst[0] = work.parent_ofst.x;
        ldpart_work.pivot_parent_ofst[1] = work.parent_ofst.y;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5LdPartMain);
        gmBoss5LdPartProcInit(ldpart_work);
        return ldpart_work;
    }

    private static void gmBoss5LdPartInitSpin(GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        nnMakeUnitQuaternion(ref ldpart_work.cur_rot_quat);
        nnMakeUnitQuaternion(ref ldpart_work.rot_diff_quat);
        for (int index = 0; index < GMD_BOSS5_LAND_LDPART_SPIN_ROT_AXIS_NUM; ++index)
        {
            NNS_VECTOR dst_vec = GlobalPool<NNS_VECTOR>.Alloc();
            float rand_z = MTM_MATH_CLIP((float)(FX_FX32_TO_F32(AkMathRandFx()) * 2.0 - 1.0), -1f, 1f);
            short rand_angle = AKM_DEGtoA16(360f * FX_FX32_TO_F32(AkMathRandFx()));
            AkMathGetRandomUnitVector(dst_vec, rand_z, rand_angle);
            NNS_QUATERNION dst;
            nnMakeRotateAxisQuaternion(out dst, dst_vec.x, dst_vec.y, dst_vec.z, GMD_BOSS5_LAND_LDPART_SPIN_ROT_SPD_DEG);
            nnMultiplyQuaternion(ref ldpart_work.rot_diff_quat, ref dst, ref ldpart_work.rot_diff_quat);
        }
    }

    private static void gmBoss5LdPartUpdateSpin(GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(ldpart_work);
        nnMultiplyQuaternion(ref ldpart_work.cur_rot_quat, ref ldpart_work.rot_diff_quat, ref ldpart_work.cur_rot_quat);
        nnMakeQuaternionMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, ref ldpart_work.cur_rot_quat);
        obsObjectWork.disp_flag |= 16777216U;
    }

    private static void gmBoss5LdPartInitFall(GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(ldpart_work);
        int ang = AKM_DEGtoA32(mtMathRand() % GMD_BOSS5_LAND_LDPART_FALL_XY_DIR_RANGE_DEG + (270 - GMD_BOSS5_LAND_LDPART_FALL_XY_DIR_RANGE_DEG / 2));
        int num = FX_Mul(AkMathRandFx(), GMD_BOSS5_LAND_LDPART_FALL_Z_SPD_MAX * 2) - GMD_BOSS5_LAND_LDPART_FALL_Z_SPD_MAX;
        obsObjectWork.spd.y = (int)(4096.0 * GMD_BOSS5_LAND_LDPART_FALL_XY_SPD_FL * nnSin(ang));
        obsObjectWork.spd.x = (int)(4096.0 * GMD_BOSS5_LAND_LDPART_FALL_XY_SPD_FL * nnCos(ang));
        obsObjectWork.spd.z = num;
        obsObjectWork.flag &= 4294966271U;
        obsObjectWork.move_flag |= 128U;
    }

    private static void gmBoss5LdPartInitVib(GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        ldpart_work.vib_cnt = mtMathRand() % 40;
        ldpart_work.vib_ofst[0] = ldpart_work.vib_ofst[1] = 0;
    }

    private static void gmBoss5LdPartUpdateVib(GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        ldpart_work.vib_ofst[0] = FX_Mul(gm_boss5_land_vib_tbl[ldpart_work.vib_cnt][0], GMD_BOSS5_LAND_LDPART_VIB_AMPLITUDE);
        ldpart_work.vib_ofst[1] = FX_Mul(gm_boss5_land_vib_tbl[ldpart_work.vib_cnt][1], GMD_BOSS5_LAND_LDPART_VIB_AMPLITUDE);
        ++ldpart_work.vib_cnt;
        if (ldpart_work.vib_cnt < 40)
            return;
        ldpart_work.vib_cnt = 0;
    }

    private static void gmBoss5LdPartClearVib(GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        ldpart_work.vib_cnt = 0;
        ldpart_work.vib_ofst[0] = ldpart_work.vib_ofst[1] = 0;
    }

    private static void gmBoss5LdPartMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_LDPART_WORK wrk = (GMS_BOSS5_LDPART_WORK)obj_work;
        obj_work.parent_ofst.x = wrk.pivot_parent_ofst[0] + wrk.vib_ofst[0];
        obj_work.parent_ofst.y = wrk.pivot_parent_ofst[1] + wrk.vib_ofst[1];
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    private static void gmBoss5LdPartProcInit(GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(ldpart_work);
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag |= 4U;
        ldpart_work.proc_update = new MPP_VOID_GMS_BOSS5_LDPART_WORK(gmBoss5LdPartProcUpdateIdle);
    }

    private static void gmBoss5LdPartProcUpdateIdle(GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        if (((int)((GMS_BOSS5_LAND_WORK)GMM_BS_OBJ(ldpart_work).parent_obj).flag & (int)GMD_BOSS5_LAND_FLAG_SHAKE_ACTIVE) == 0)
            return;
        gmBoss5LdPartInitVib(ldpart_work);
        ldpart_work.proc_update = new MPP_VOID_GMS_BOSS5_LDPART_WORK(gmBoss5LdPartProcUpdateShake);
    }

    private static void gmBoss5LdPartProcUpdateShake(GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        GMS_BOSS5_LAND_WORK parentObj = (GMS_BOSS5_LAND_WORK)GMM_BS_OBJ(ldpart_work).parent_obj;
        gmBoss5LdPartUpdateVib(ldpart_work);
        if (((int)parentObj.flag & (int)GMD_BOSS5_LAND_FLAG_BREAK_ACTIVE) == 0)
            return;
        gmBoss5LdPartClearVib(ldpart_work);
        gmBoss5LdPartInitSpin(ldpart_work);
        gmBoss5LdPartInitFall(ldpart_work);
        ldpart_work.wait_timer = (uint)(ldpart_work.part_index & 1);
        ldpart_work.proc_update = new MPP_VOID_GMS_BOSS5_LDPART_WORK(gmBoss5LdPartProcUpdateFall);
    }

    private static void gmBoss5LdPartProcUpdateFall(GMS_BOSS5_LDPART_WORK ldpart_work)
    {
        if (ldpart_work.wait_timer != 0U)
            --ldpart_work.wait_timer;
        else if (ldpart_work.brk_glass_cnt == 0U)
        {
            GmBoss5EfctCreateBreakingGlass(GMM_BS_OBJ(ldpart_work));
            ++ldpart_work.brk_glass_cnt;
        }
        gmBoss5LdPartUpdateSpin(ldpart_work);
    }

    private static void GmBoss5LandSetLight()
    {
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.x = 0.0f;
        nnsVector.y = -0.2f;
        nnsVector.z = -1f;
        nnNormalizeVector(nnsVector, nnsVector);
        ObjDrawSetParallelLight(NNE_LIGHT_1, ref col, 1f, nnsVector);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }


    private static OBS_OBJECT_WORK GmGmkBoss5LandPlaceInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_BOSS5_LAND_PLACE_WORK(), "BOSS5_LAND_PLACE");
        work.flag |= 16U;
        work.disp_flag &= 4294967263U;
        work.move_flag |= 8448U;
        work.move_flag &= 4294967167U;
        return work;
    }
}