public partial class AppMain
{
    public static OBS_OBJECT_WORK GmBoss5TurretInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS5_TURRET_WORK(), "BOSS5_TRT");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS5_TURRET_WORK trt_work = (GMS_BOSS5_TURRET_WORK)work;
        ObjObjectCopyAction3dNNModel(work, GmBoss5GetObject3dList()[0], gmsEnemy3DWork.obj_3d);
        ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = GMD_BOSS5_DEFAULT_BLEND_SPD;
        work.flag |= 18U;
        work.disp_flag |= 4194304U;
        work.move_flag |= 256U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        gmBoss5TurretInitDispRot(trt_work);
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5TurretMain);
        gmBoss5TurretProcInit(trt_work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static GMS_BOSS5_TURRET_WORK GmBoss5TurretStartUp(
      GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork1 = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK obsObjectWork2 = GmEventMgrLocalEventBirth(333, obsObjectWork1.pos.x, obsObjectWork1.pos.y, 0, 0, 0, 0, 0, 0);
        obsObjectWork2.parent_obj = obsObjectWork1;
        return (GMS_BOSS5_TURRET_WORK)obsObjectWork2;
    }

    public static void gmBoss5TurretGetDispRotatedOfstPos(
      GMS_BOSS5_TURRET_WORK trt_work,
      ref VecFx32 src_ofst_pos,
      out VecFx32 dest_ofst_pos)
    {
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        amVectorSet(nnsVector, FX_FX32_TO_F32(src_ofst_pos.x), FX_FX32_TO_F32(-src_ofst_pos.y), FX_FX32_TO_F32(src_ofst_pos.z));
        nnMakeQuaternionMatrix(nnsMatrix, ref trt_work.disp_quat);
        nnTransformVector(nnsVector, nnsMatrix, nnsVector);
        dest_ofst_pos = new VecFx32(FX_F32_TO_FX32(nnsVector.x), FX_F32_TO_FX32(-nnsVector.y), FX_F32_TO_FX32(nnsVector.z));
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    public static void gmBoss5TurretGetVulcanFirePos(
      GMS_BOSS5_TURRET_WORK trt_work,
      ref VecFx32 out_pos)
    {
        VecFx32 src_ofst_pos = new VecFx32(0, 0, GMD_BOSS5_TURRET_VULCAN_FIRE_OFST_FORWARD);
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(trt_work);
        VecFx32 dest_ofst_pos;
        gmBoss5TurretGetDispRotatedOfstPos(trt_work, ref src_ofst_pos, out dest_ofst_pos);
        VEC_Set(ref out_pos, obsObjectWork.pos.x + dest_ofst_pos.x, obsObjectWork.pos.y + dest_ofst_pos.y, obsObjectWork.pos.z + GMD_BOSS5_TURRET_VULCAN_FIRE_OFST_Z);
    }

    public static void gmBoss5TurretGetVulcanBulletPos(
      GMS_BOSS5_TURRET_WORK trt_work,
      ref VecFx32 out_pos)
    {
        VecFx32 src_ofst_pos = new VecFx32(0, 0, GMD_BOSS5_TURRET_VULCAN_BULLET_OFST_FORWARD);
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(trt_work);
        VecFx32 dest_ofst_pos;
        gmBoss5TurretGetDispRotatedOfstPos(trt_work, ref src_ofst_pos, out dest_ofst_pos);
        VEC_Set(ref out_pos, obsObjectWork.pos.x + dest_ofst_pos.x, obsObjectWork.pos.y + dest_ofst_pos.y, obsObjectWork.pos.z + GMD_BOSS5_TURRET_VULCAN_BULLET_OFST_Z);
    }

    public static void gmBoss5TurretInitDispRot(GMS_BOSS5_TURRET_WORK trt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(trt_work);
        obsObjectWork.disp_flag &= 4278190079U;
        nnMakeUnitQuaternion(ref trt_work.disp_quat);
        nnMakeUnitMatrix(obsObjectWork.obj_3d.user_obj_mtx_r);
    }

    public static void gmBoss5TurretUpdateDispRot(GMS_BOSS5_TURRET_WORK trt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(trt_work);
        obsObjectWork.disp_flag |= 16777216U;
        nnMakeQuaternionMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, ref trt_work.disp_quat);
    }

    public static void gmBoss5TurretUpdateDirFollowingPos(
      GMS_BOSS5_TURRET_WORK trt_work,
      ref VecFx32 targ_pos,
      float deg)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(trt_work);
        int a1 = (int)(ushort.MaxValue & (long)((int)(ushort.MaxValue & (long)nnArcTan2(FX_FX32_TO_F32(targ_pos.y - obsObjectWork.pos.y), FX_FX32_TO_F32(targ_pos.x - obsObjectWork.pos.x))) - trt_work.fire_dir_z));
        int a2;
        if (a1 >= AKM_DEGtoA32(180))
        {
            a1 = -(AKM_DEGtoA32(360) - a1);
            a2 = AKM_DEGtoA32(-deg);
        }
        else
            a2 = AKM_DEGtoA32(deg);
        if (MTM_MATH_ABS(a1) <= MTM_MATH_ABS(a2))
            a2 = a1;
        trt_work.fire_dir_z = (short)(ushort.MaxValue & (long)(trt_work.fire_dir_z + a2));
    }

    public static void gmBoss5TurretSetRoundFaceRot(
      GMS_BOSS5_TURRET_WORK trt_work,
      int dir_z_angle,
      int tilt_near_angle)
    {
        int rz = (int)(ushort.MaxValue & (long)-dir_z_angle);
        nnMakeRotateXZYQuaternion(out trt_work.disp_quat, AKM_DEGtoA32(90), -tilt_near_angle, AKM_DEGtoA32(90));
        NNS_QUATERNION dst;
        nnMakeRotateXYZQuaternion(out dst, 0, 0, rz);
        nnMultiplyQuaternion(ref trt_work.disp_quat, ref dst, ref trt_work.disp_quat);
    }

    public static void gmBoss5TurretUpdateDirFacePly(GMS_BOSS5_TURRET_WORK trt_work)
    {
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        gmBoss5TurretUpdateDirFollowingPos(trt_work, ref playerObj.pos, GMD_BOSS5_TURRET_FACE_PLY_SPD_DEG);
        float num = nnSin(trt_work.fire_dir_z);
        if (num < 0.0)
            num = 0.0f;
        int tilt_near_angle = (int)(num * (double)(ushort.MaxValue & (long)GMD_BOSS5_TURRET_TILT_NEAR_ANGLE));
        gmBoss5TurretSetRoundFaceRot(trt_work, trt_work.fire_dir_z, tilt_near_angle);
    }

    public static void gmBoss5TurretInitVulcanBurstShot(
      GMS_BOSS5_TURRET_WORK trt_work,
      int shot_num)
    {
        trt_work.vul_shot_remain = shot_num;
        trt_work.vul_burst_timer = GMD_BOSS5_TURRET_VULCAN_SHOT_INTERVAL;
        trt_work.vul_shot_angle = trt_work.fire_dir_z;
        gmBoss5TurretGetVulcanFirePos(trt_work, ref trt_work.vul_fire_pos);
        gmBoss5TurretGetVulcanBulletPos(trt_work, ref trt_work.vul_bullet_pos);
    }

    public static int gmBoss5TurretUpdateVulcanBurstShot(GMS_BOSS5_TURRET_WORK trt_work)
    {
        if (trt_work.vul_shot_remain == 0)
            return 1;
        if (trt_work.vul_burst_timer != 0)
        {
            --trt_work.vul_burst_timer;
        }
        else
        {
            gmBoss5TurretGetVulcanFirePos(trt_work, ref trt_work.vul_fire_pos);
            gmBoss5TurretGetVulcanBulletPos(trt_work, ref trt_work.vul_bullet_pos);
            GmBoss5EfctCreateVulcanFire(trt_work, trt_work.vul_fire_pos, trt_work.vul_shot_angle);
            GmBoss5EfctCreateVulcanBullet(trt_work, trt_work.vul_bullet_pos, trt_work.vul_shot_angle, GMD_BOSS5_TURRET_VULCAN_BULLET_SPD);
            --trt_work.vul_shot_remain;
            trt_work.vul_burst_timer = GMD_BOSS5_TURRET_VULCAN_SHOT_INTERVAL;
        }
        return 0;
    }

    public static void gmBoss5TurretClearVulcanBurstShot(GMS_BOSS5_TURRET_WORK trt_work)
    {
        trt_work.vul_shot_remain = 0;
        trt_work.vul_burst_timer = 0;
    }

    public static void gmBoss5TurretInitPartsPose(GMS_BOSS5_TURRET_WORK trt_work)
    {
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(trt_work).parent_obj;
        int[] numArray = new int[2]
        {
      parentObj.pole_cnm_reg_id,
      parentObj.cover_cnm_reg_id
        };
        int num = 2;
        nnMakeUnitMatrix(nnsMatrix);
        for (int index = 0; index < num; ++index)
        {
            GmBsCmnChangeCNMModeNode(parentObj.cnm_mgr_work, numArray[index], 1U);
            GmBsCmnEnableCNMLocalCoordinate(parentObj.cnm_mgr_work, numArray[index], 1);
            GmBsCmnEnableCNMMtxNode(parentObj.cnm_mgr_work, numArray[index], 1);
            GmBsCmnSetCNMMtx(parentObj.cnm_mgr_work, nnsMatrix, numArray[index]);
        }
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    public static void gmBoss5TurretEndPartsPose(GMS_BOSS5_TURRET_WORK trt_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(trt_work).parent_obj;
        int[] numArray = new int[2]
        {
      parentObj.pole_cnm_reg_id,
      parentObj.cover_cnm_reg_id
        };
        int num = 2;
        for (int index = 0; index < num; ++index)
            GmBsCmnEnableCNMMtxNode(parentObj.cnm_mgr_work, numArray[index], 0);
    }

    public static void gmBoss5TurretInitSlideTurret(
      GMS_BOSS5_TURRET_WORK trt_work,
      int slide_type)
    {
        trt_work.trt_slide_type = slide_type;
        if (slide_type == 0)
            trt_work.trt_slide_length = 0.0f;
        else
            trt_work.trt_slide_length = GMD_BOSS5_TURRET_SLIDE_LENGTH_MAX;
    }

    public static int gmBoss5TurretUpdateSlideTurret(GMS_BOSS5_TURRET_WORK trt_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(trt_work).parent_obj;
        int num;
        if (trt_work.trt_slide_type == 0)
        {
            if (trt_work.trt_slide_length < (double)GMD_BOSS5_TURRET_SLIDE_LENGTH_MAX)
            {
                trt_work.trt_slide_length += GMD_BOSS5_TURRET_SLIDE_RAISE_SPD_F;
                num = 0;
            }
            else
            {
                trt_work.trt_slide_length = GMD_BOSS5_TURRET_SLIDE_LENGTH_MAX;
                num = 1;
            }
        }
        else if (trt_work.trt_slide_length > 0.0)
        {
            trt_work.trt_slide_length -= GMD_BOSS5_TURRET_SLIDE_LOWER_SPD_F;
            num = 0;
        }
        else
        {
            trt_work.trt_slide_length = 0.0f;
            num = 1;
        }
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        nnMakeTranslateMatrix(nnsMatrix, 0.0f, trt_work.trt_slide_length + GMD_BOSS5_TURRET_SLIDE_POLE_DISP_OFST_Y, 0.0f);
        GmBsCmnSetCNMMtx(parentObj.cnm_mgr_work, nnsMatrix, parentObj.pole_cnm_reg_id);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
        return num;
    }

    public static void gmBoss5TurretInitSlideCover(
      GMS_BOSS5_TURRET_WORK trt_work,
      int slide_type)
    {
        trt_work.cvr_slide_type = slide_type;
        if (slide_type == 0)
            trt_work.cvr_slide_ratio = 0.0f;
        else
            trt_work.cvr_slide_ratio = 1f;
    }

    public static int gmBoss5TurretUpdateSlideCover(GMS_BOSS5_TURRET_WORK trt_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(trt_work).parent_obj;
        int num1;
        if (trt_work.cvr_slide_type == 0)
        {
            if (trt_work.cvr_slide_ratio < 1.0)
            {
                trt_work.cvr_slide_ratio += GMD_BOSS5_TURRET_COVER_SLIDE_OPEN_RATIO_SPD_F;
                num1 = 0;
            }
            else
            {
                trt_work.cvr_slide_ratio = 1f;
                num1 = 1;
            }
        }
        else if (trt_work.cvr_slide_ratio > 0.0)
        {
            trt_work.cvr_slide_ratio -= GMD_BOSS5_TURRET_COVER_SLIDE_CLOSE_RATIO_SPD_F;
            num1 = 0;
        }
        else
        {
            trt_work.cvr_slide_ratio = 0.0f;
            num1 = 1;
        }
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        float num2 = (float)(1.0 + trt_work.cvr_slide_ratio * (GMD_BOSS5_TURRET_COVER_SLIDE_SCALE_MAX - 1.0));
        nnMakeRotateXMatrix(nnsMatrix, AKM_DEGtoA32(trt_work.cvr_slide_ratio * GMD_BOSS5_TURRET_COVER_SLIDE_DEG_MAX));
        nnScaleMatrix(nnsMatrix, nnsMatrix, num2, num2, num2);
        GmBsCmnSetCNMMtx(parentObj.cnm_mgr_work, nnsMatrix, parentObj.cover_cnm_reg_id);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
        return num1;
    }

    public static void gmBoss5TurretMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_TURRET_WORK gmsBosS5TurretWork = (GMS_BOSS5_TURRET_WORK)obj_work;
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        NNS_MATRIX turretMainTrtOfst = gmBoss5TurretMain_trt_ofst;
        if (gmsBosS5TurretWork.proc_update != null)
            gmsBosS5TurretWork.proc_update(gmsBosS5TurretWork);
        nnMakeTranslateMatrix(turretMainTrtOfst, 0.0f, gmsBosS5TurretWork.trt_slide_length, 0.0f);
        GmBsCmnUpdateObject3DNNStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.pole_snm_reg_id, 0, obj_work.parent_obj.pos, parentObj.pivot_prev_pos, turretMainTrtOfst);
        gmBoss5TurretUpdateDispRot(gmsBosS5TurretWork);
    }

    public static void gmBoss5TurretProcInit(GMS_BOSS5_TURRET_WORK trt_work)
    {
        trt_work.proc_update = new MPP_VOID_GMS_BOSS5_TURRET_WORK(gmBoss5TurretProcUpdateStandby);
    }

    public static void gmBoss5TurretProcUpdateStandby(GMS_BOSS5_TURRET_WORK trt_work)
    {
        if (((int)((GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(trt_work).parent_obj).flag & 512) != 0 || gmBoss5TurretSeqGetVulcanShotNum(trt_work) <= 0)
            return;
        if (trt_work.wait_timer != 0U)
        {
            --trt_work.wait_timer;
        }
        else
        {
            gmBoss5TurretInitPartsPose(trt_work);
            gmBoss5TurretInitSlideCover(trt_work, 0);
            gmBoss5TurretUpdateDirFollowingPos(trt_work, ref GmBsCmnGetPlayerObj().pos, 360f);
            trt_work.proc_update = new MPP_VOID_GMS_BOSS5_TURRET_WORK(gmBoss5TurretProcUpdateOpen);
        }
    }

    public static void gmBoss5TurretProcUpdateOpen(GMS_BOSS5_TURRET_WORK trt_work)
    {
        if (gmBoss5TurretUpdateSlideCover(trt_work) == 0)
            return;
        gmBoss5TurretInitSlideTurret(trt_work, 0);
        trt_work.proc_update = new MPP_VOID_GMS_BOSS5_TURRET_WORK(gmBoss5TurretProcUpdateAppear);
    }

    public static void gmBoss5TurretProcUpdateAppear(GMS_BOSS5_TURRET_WORK trt_work)
    {
        gmBoss5TurretUpdateDirFacePly(trt_work);
        if (gmBoss5TurretUpdateSlideTurret(trt_work) == 0)
            return;
        trt_work.wait_timer = GMD_BOSS5_TURRET_FACE_TIME;
        trt_work.proc_update = new MPP_VOID_GMS_BOSS5_TURRET_WORK(gmBoss5TurretProcUpdateFace);
    }

    public static void gmBoss5TurretProcUpdateFace(GMS_BOSS5_TURRET_WORK trt_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(trt_work).parent_obj;
        gmBoss5TurretUpdateDirFacePly(trt_work);
        if (((int)parentObj.flag & 512) != 0)
        {
            trt_work.wait_timer = 0U;
            trt_work.proc_update = new MPP_VOID_GMS_BOSS5_TURRET_WORK(gmBoss5TurretProcUpdateDisappear);
        }
        else if (trt_work.wait_timer != 0U)
        {
            --trt_work.wait_timer;
        }
        else
        {
            gmBoss5TurretInitVulcanBurstShot(trt_work, gmBoss5TurretSeqGetVulcanShotNum(trt_work));
            trt_work.proc_update = new MPP_VOID_GMS_BOSS5_TURRET_WORK(gmBoss5TurretProcUpdateFire);
        }
    }

    public static void gmBoss5TurretProcUpdateFire(GMS_BOSS5_TURRET_WORK trt_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(trt_work).parent_obj;
        if (gmBoss5TurretUpdateVulcanBurstShot(trt_work) == 0 && ((int)parentObj.flag & 512) == 0)
            return;
        gmBoss5TurretClearVulcanBurstShot(trt_work);
        gmBoss5TurretInitSlideTurret(trt_work, 1);
        trt_work.proc_update = new MPP_VOID_GMS_BOSS5_TURRET_WORK(gmBoss5TurretProcUpdateDisappear);
    }

    public static void gmBoss5TurretProcUpdateDisappear(GMS_BOSS5_TURRET_WORK trt_work)
    {
        if (gmBoss5TurretUpdateSlideTurret(trt_work) == 0)
            return;
        trt_work.wait_timer = gmBoss5TurretSeqGetVulcanWaitTime(trt_work);
        gmBoss5TurretInitSlideCover(trt_work, 1);
        trt_work.proc_update = new MPP_VOID_GMS_BOSS5_TURRET_WORK(gmBoss5TurretProcUpdateClose);
    }

    public static void gmBoss5TurretProcUpdateClose(GMS_BOSS5_TURRET_WORK trt_work)
    {
        if (gmBoss5TurretUpdateSlideCover(trt_work) == 0)
            return;
        gmBoss5TurretEndPartsPose(trt_work);
        trt_work.proc_update = new MPP_VOID_GMS_BOSS5_TURRET_WORK(gmBoss5TurretProcUpdateStandby);
    }

    public static uint gmBoss5TurretSeqGetVulcanWaitTime(GMS_BOSS5_TURRET_WORK trt_work)
    {
        int life = ((GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(trt_work).parent_obj).mgr_work.life;
        GMS_BOSS5_TURRET_SEQ_VUL_SHOT_INFO turretSeqVulShotInfo = null;
        for (int index = 0; index < 5; ++index)
        {
            if (life <= gm_boss5_trt_seq_vul_shot_info_tbl[index].life_threshold)
            {
                turretSeqVulShotInfo = gm_boss5_trt_seq_vul_shot_info_tbl[index];
                break;
            }
        }
        return turretSeqVulShotInfo == null ? 0U : turretSeqVulShotInfo.wait_time;
    }

    public static int gmBoss5TurretSeqGetVulcanShotNum(GMS_BOSS5_TURRET_WORK trt_work)
    {
        int life = ((GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(trt_work).parent_obj).mgr_work.life;
        GMS_BOSS5_TURRET_SEQ_VUL_SHOT_INFO turretSeqVulShotInfo = null;
        for (int index = 0; index < 5; ++index)
        {
            if (life <= gm_boss5_trt_seq_vul_shot_info_tbl[index].life_threshold)
            {
                turretSeqVulShotInfo = gm_boss5_trt_seq_vul_shot_info_tbl[index];
                break;
            }
        }
        return turretSeqVulShotInfo == null ? 0 : turretSeqVulShotInfo.shot_num;
    }

}