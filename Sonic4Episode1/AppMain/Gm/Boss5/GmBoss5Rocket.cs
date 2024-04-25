public partial class AppMain
{
    public static OBS_OBJECT_WORK GmBoss5RocketInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS5_ROCKET_WORK(), "BOSS5_RKT");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS5_ROCKET_WORK rkt_work = (GMS_BOSS5_ROCKET_WORK)work;
        ObjObjectFieldRectSet(work, -16, -16, 16, 16);
        work.move_flag |= 256U;
        work.move_flag &= 4294443007U;
        ObjObjectCopyAction3dNNModel(work, GmBoss5GetObject3dList()[3], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(747), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = GMD_BOSS5_DEFAULT_BLEND_SPD;
        work.flag |= 16U;
        work.disp_flag |= 4194304U;
        work.move_flag |= 16U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        gmBoss5RocketSetRectSize(rkt_work, 1);
        gmsEnemy3DWork.ene_com.rect_work[1].ppHit = new OBS_RECT_WORK_Delegate1(gmBoss5RocketAtkPlyHitFunc);
        gmsEnemy3DWork.ene_com.rect_work[0].ppDef = new OBS_RECT_WORK_Delegate1(gmBoss5RocketDamageDefFunc);
        gmsEnemy3DWork.ene_com.rect_work[0].flag |= 2048U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag |= 2048U;
        GmBsCmnSetAction(work, 0, 1, 0);
        gmBoss5RocketInitCallbacks(rkt_work);
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5RocketMain);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5RocketOutFunc);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss5RocketExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static GMS_BOSS5_ROCKET_WORK GmBoss5RocketLaunchNormal(
      GMS_BOSS5_BODY_WORK body_work,
      int rkt_type)
    {
        GMS_BOSS5_ROCKET_WORK rkt_work = gmBoss5RocketCreate(body_work, rkt_type);
        gmBoss5RocketNmlProcInit(rkt_work);
        return rkt_work;
    }

    public static GMS_BOSS5_ROCKET_WORK GmBoss5RocketLaunchStrong(
      GMS_BOSS5_BODY_WORK body_work,
      int rkt_type)
    {
        GMS_BOSS5_ROCKET_WORK rkt_work = gmBoss5RocketCreate(body_work, rkt_type);
        gmBoss5RocketStrProcInit(rkt_work);
        return rkt_work;
    }

    public static GMS_BOSS5_ROCKET_WORK GmBoss5RocketSpawnConnected(
      GMS_BOSS5_BODY_WORK body_work,
      int rkt_type)
    {
        GMS_BOSS5_ROCKET_WORK rkt_work = gmBoss5RocketCreate(body_work, rkt_type);
        gmBoss5RocketCnctProcInit(rkt_work);
        return rkt_work;
    }

    public static void gmBoss5RocketExit(MTS_TASK_TCB tcb)
    {
        gmBoss5RocketReleaseCallbacks((GMS_BOSS5_ROCKET_WORK)mtTaskGetTcbWork(tcb));
        GmEnemyDefaultExit(tcb);
    }

    public static GMS_BOSS5_ROCKET_WORK gmBoss5RocketCreate(
      GMS_BOSS5_BODY_WORK body_work,
      int rkt_type)
    {
        OBS_OBJECT_WORK obsObjectWork1 = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK obsObjectWork2 = GmEventMgrLocalEventBirth(332, obsObjectWork1.pos.x, obsObjectWork1.pos.y, 0, 0, 0, 0, 0, 0);
        GMS_BOSS5_ROCKET_WORK gmsBosS5RocketWork = (GMS_BOSS5_ROCKET_WORK)obsObjectWork2;
        obsObjectWork2.parent_obj = obsObjectWork1;
        gmsBosS5RocketWork.rkt_type = rkt_type;
        gmsBosS5RocketWork.arm_snm_id = 1 != rkt_type ? body_work.armpt_snm_reg_ids[0][2] : body_work.armpt_snm_reg_ids[1][2];
        obsObjectWork2.disp_flag |= 16777216U;
        if (((int)obsObjectWork1.disp_flag & 1) != 0)
        {
            nnMakeRotateXMatrix(obsObjectWork2.obj_3d.user_obj_mtx_r, AKM_DEGtoA32(180));
            obsObjectWork2.disp_flag |= 1U;
        }
        else
        {
            nnMakeRotateXMatrix(obsObjectWork2.obj_3d.user_obj_mtx_r, AKM_DEGtoA32(0));
            obsObjectWork2.disp_flag &= 4294967294U;
        }
        return (GMS_BOSS5_ROCKET_WORK)obsObjectWork2;
    }

    public static void gmBoss5RocketSetAtkBodyRect(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_RECT_WORK pRec = ((GMS_ENEMY_COM_WORK)rkt_work).rect_work[1];
        ObjRectGroupSet(pRec, 0, 2);
        ObjRectAtkSet(pRec, 2, 1);
        ObjRectDefSet(pRec, ushort.MaxValue, 0);
        pRec.flag |= 4U;
        pRec.ppHit = new OBS_RECT_WORK_Delegate1(gmBoss5RocketAtkBossHitFunc);
        pRec.ppDef = null;
    }

    public static void gmBoss5RocketSetNoHitTime(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)rkt_work;
        rkt_work.no_hit_timer = 10U;
        gmsEnemyComWork.rect_work[0].flag |= 2048U;
        gmsEnemyComWork.rect_work[1].flag |= 2048U;
    }

    public static void gmBoss5RocketUpdateNoHitTime(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (rkt_work.no_hit_timer != 0U)
        {
            --rkt_work.no_hit_timer;
        }
        else
        {
            GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)rkt_work;
            gmsEnemyComWork.rect_work[0].flag &= 4294965247U;
            gmsEnemyComWork.rect_work[1].flag &= 4294965247U;
        }
    }

    public static void gmBoss5RocketUpdateMainRectPosition(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(rkt_work.snm_work, rkt_work.drill_snm_reg_id);
        int x = FX_F32_TO_FX32(snmMtx.M03) - rkt_work.pivot_prev_pos.x;
        int y = -FX_F32_TO_FX32(snmMtx.M13) - rkt_work.pivot_prev_pos.y;
        for (int index = 0; index < 3; ++index)
            VEC_Set(ref rkt_work.ene_3d.ene_com.rect_work[index].rect.pos, x, y, 0);
    }

    public static void gmBoss5RocketSetAtkEnable(GMS_BOSS5_ROCKET_WORK rkt_work, int enable)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)rkt_work;
        if (enable != 0)
            gmsEnemyComWork.rect_work[1].flag &= 4294965247U;
        else
            gmsEnemyComWork.rect_work[1].flag |= 2048U;
    }

    public static void gmBoss5RocketSetDmgEnable(GMS_BOSS5_ROCKET_WORK rkt_work, int enable)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)rkt_work;
        if (enable != 0)
            gmsEnemyComWork.rect_work[0].flag &= 4294965247U;
        else
            gmsEnemyComWork.rect_work[0].flag |= 2048U;
    }

    public static void gmBoss5RocketSetRectSize(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      int sides_type)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)rkt_work;
        switch (sides_type)
        {
            case 1:
                ObjRectWorkSet(gmsEnemyComWork.rect_work[1], -20, -20, 20, 20);
                ObjRectWorkSet(gmsEnemyComWork.rect_work[0], -10, -10, 10, 10);
                break;
            default:
                ObjRectWorkSet(gmsEnemyComWork.rect_work[0], -20, -20, 20, 20);
                ObjRectWorkSet(gmsEnemyComWork.rect_work[1], -10, -10, 10, 10);
                break;
        }
    }

    public static void gmBoss5RocketInitPlySearch(GMS_BOSS5_ROCKET_WORK rkt_work, int delay)
    {
        rkt_work.ply_search_delay = delay;
        GmBsCmnInitDelaySearch(rkt_work.dsearch_work, GmBsCmnGetPlayerObj(), rkt_work.search_hist_buf, 21);
    }

    public static void gmBoss5RocketUpdatePlySearch(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        GmBsCmnUpdateDelaySearch(rkt_work.dsearch_work);
    }

    public static void gmBoss5RocketGetPlySearchPos(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      out VecFx32 pos)
    {
        GmBsCmnGetDelaySearchPos(rkt_work.dsearch_work, rkt_work.ply_search_delay, out pos);
    }

    public static int gmBoss5RocketSetPlyRebound(
      OBS_RECT_WORK rkt_rect,
      OBS_RECT_WORK ply_rect)
    {
        OBS_OBJECT_WORK parentObj1 = ply_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = rkt_rect.parent_obj;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)parentObj1;
        OBS_OBJECT_WORK parentObj3 = parentObj2.parent_obj;
        GmPlySeqAtkReactionInit(ply_work);
        if (ply_work.seq_state == GME_PLY_SEQ_STATE_HOMING_REF)
        {
            GmPlySeqSetJumpState(ply_work, 0, 5U);
            uint num = GmBsCmnCheckRectHitSideVFirst(rkt_rect, ply_rect);
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ((int)num & (int)GMD_BS_CMN_RECT_HIT_SIDE_H_MASK) == 0 ? (ply_work.obj_work.move.x <= 0 ? (ply_work.obj_work.move.x >= 0 ? (ply_work.obj_work.pos.x >= parentObj3.pos.x ? (parentObj3.pos.x >= ply_work.obj_work.pos.x ? (((int)ply_work.obj_work.disp_flag & 1) == 0 ? -16384 : 16384) : -16384) : 16384) : 16384) : -16384) : (((int)num & (int)GMD_BS_CMN_RECT_HIT_SIDE_LEFT) == 0 ? 16384 : -16384);
            ply_work.obj_work.spd.y = -16384;
            GmPlySeqSetNoJumpMoveTime(ply_work, 49152);
        }
        else
        {
            uint num = GmBsCmnCheckRectHitSideVFirst(rkt_rect, ply_rect);
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ((int)num & (int)GMD_BS_CMN_RECT_HIT_SIDE_H_MASK) == 0 ? (ply_work.obj_work.move.x <= 0 ? (ply_work.obj_work.move.x >= 0 ? (ply_work.obj_work.pos.x >= parentObj3.pos.x ? (parentObj3.pos.x >= ply_work.obj_work.pos.x ? (((int)ply_work.obj_work.disp_flag & 1) == 0 ? -12288 : 12288) : -12288) : 12288) : 12288) : -12288) : (((int)num & (int)GMD_BS_CMN_RECT_HIT_SIDE_LEFT) == 0 ? 12288 : -12288);
            ply_work.obj_work.spd.y = -16384;
            GmPlySeqSetNoJumpMoveTime(ply_work, 32768);
            ply_work.homing_timer = 40960;
        }
        return 1;
    }

    public static void gmBoss5RocketUpdateRocketStuckWithArm(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      int b_rotation)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        NNS_MATRIX ofst_mtx;
        if (((int)parentObj.flag & 16) != 0)
        {
            int index = rkt_work.rkt_type != 1 ? 0 : 1;
            ofst_mtx = parentObj.rkt_ofst_mtx[index];
        }
        else
            ofst_mtx = null;
        if (parentObj.adj_hgap_is_active != 0)
            GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, rkt_work.arm_snm_id, b_rotation, ofst_mtx);
        else
            GmBsCmnUpdateObject3DNNStuckWithNodeRelative(obj_work, parentObj.snm_work, rkt_work.arm_snm_id, b_rotation, obj_work.parent_obj.pos, parentObj.pivot_prev_pos, ofst_mtx);
        if (b_rotation == 0 || rkt_work.rkt_type != 1)
            return;
        obj_work.disp_flag |= 16777216U;
        nnRotateYMatrix(obj_work.obj_3d.user_obj_mtx_r, obj_work.obj_3d.user_obj_mtx_r, AKM_DEGtoA32(180));
    }

    public static void gmBoss5RocketInitRocketStuckWithArmLerpRot(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      float ratio_spd)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(rkt_work.snm_work, rkt_work.drill_snm_reg_id);
        AkMathNormalizeMtx(nnsMatrix, snmMtx);
        nnMakeRotateMatrixQuaternion(out rkt_work.stuck_lerp_src_quat, nnsMatrix);
        gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 0);
        obsObjectWork.dir.x = obsObjectWork.dir.y = obsObjectWork.dir.z = 0;
        rkt_work.stuck_lerp_ratio = 0.0f;
        rkt_work.stuck_lerp_ratio_spd = ratio_spd;
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    public static int gmBoss5RocketUpdateRocketStuckWithArmLerpRot(
      GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj;
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        int num = 0;
        rkt_work.stuck_lerp_ratio += rkt_work.stuck_lerp_ratio_spd;
        if (rkt_work.stuck_lerp_ratio >= 1.0)
        {
            rkt_work.stuck_lerp_ratio = 1f;
            num = 1;
        }
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(parentObj.snm_work, rkt_work.arm_snm_id);
        AkMathNormalizeMtx(nnsMatrix, snmMtx);
        NNS_QUATERNION dst1;
        nnMakeRotateMatrixQuaternion(out dst1, nnsMatrix);
        if (rkt_work.rkt_type == 1)
        {
            NNS_QUATERNION dst2;
            nnMakeRotateXYZQuaternion(out dst2, 0, AKM_DEGtoA32(180), 0);
            nnMultiplyQuaternion(ref dst1, ref dst1, ref dst2);
        }
        NNS_QUATERNION dst3;
        nnSlerpQuaternion(out dst3, ref rkt_work.stuck_lerp_src_quat, ref dst1, rkt_work.stuck_lerp_ratio);
        gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 0);
        obsObjectWork.disp_flag |= 16777216U;
        nnQuaternionMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, obsObjectWork.obj_3d.user_obj_mtx_r, ref dst3);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
        return num != 0 ? 1 : 0;
    }

    public static void gmBoss5RocketGetArmNodePosFx(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      out VecFx32 pos_out)
    {
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(((GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(rkt_work).parent_obj).snm_work, rkt_work.arm_snm_id);
        pos_out.x = FX_F32_TO_FX32(snmMtx.M03);
        pos_out.y = -FX_F32_TO_FX32(snmMtx.M13);
        pos_out.z = FX_F32_TO_FX32(snmMtx.M23);
    }

    public static void gmBoss5RocketSetDispOfst(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      float disp_ofst,
      int b_pos_slide)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        obsObjectWork.disp_flag |= 16777216U;
        nnTranslateMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, obsObjectWork.obj_3d.user_obj_mtx_r, disp_ofst, 0.0f, 0.0f);
        if (b_pos_slide == 0)
            return;
        obsObjectWork.pos.x -= FX_Mul(FX_F32_TO_FX32(nnCos(obsObjectWork.dir.z) * disp_ofst), g_obj.draw_scale.x);
        obsObjectWork.pos.y -= FX_Mul(FX_F32_TO_FX32(nnSin(obsObjectWork.dir.z) * disp_ofst), g_obj.draw_scale.y);
    }

    public static void gmBoss5RocketGetDispOfst(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      ref VecFx32 ofst_pos_out)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        amVectorSet(nnsVector, 0.0f, 0.0f, 0.0f);
        nnMakeRotateXYZMatrix(nnsMatrix, (int)(ushort.MaxValue & (long)-obsObjectWork.dir.x), ushort.MaxValue & obsObjectWork.dir.y, (int)(ushort.MaxValue & (long)-obsObjectWork.dir.z));
        nnScaleMatrix(nnsMatrix, nnsMatrix, FX_FX32_TO_F32(g_obj.draw_scale.x), FX_FX32_TO_F32(g_obj.draw_scale.y), FX_FX32_TO_F32(g_obj.draw_scale.z));
        nnMultiplyMatrix(nnsMatrix, nnsMatrix, obsObjectWork.obj_3d.user_obj_mtx_r);
        nnTransformVector(nnsVector, nnsMatrix, nnsVector);
        VEC_Set(ref ofst_pos_out, FX_F32_TO_FX32(nnsVector.x), FX_F32_TO_FX32(-nnsVector.y), FX_F32_TO_FX32(nnsVector.z));
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    public static int gmBoss5RocketInitFlyDestPos(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      int init_acc,
      int init_spd,
      int max_spd,
      VecFx32 launch_pos,
      ref VecFx32 dest_pos)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        rkt_work.acc = init_acc;
        rkt_work.max_spd = max_spd;
        int num = nnArcTan2(FX_FX32_TO_F32(dest_pos.y - launch_pos.y), FX_FX32_TO_F32(dest_pos.x - launch_pos.x));
        rkt_work.move_dir = num;
        int fx32_1 = FX_F32_TO_FX32(nnCos(rkt_work.move_dir));
        int fx32_2 = FX_F32_TO_FX32(nnSin(rkt_work.move_dir));
        obsObjectWork.spd_add.x = FX_Mul(rkt_work.acc, fx32_1);
        obsObjectWork.spd_add.y = FX_Mul(rkt_work.acc, fx32_2);
        obsObjectWork.spd_add.z = 0;
        obsObjectWork.spd.x = FX_Mul(init_spd, fx32_1);
        obsObjectWork.spd.y = FX_Mul(init_spd, fx32_2);
        rkt_work.launch_pos = launch_pos;
        rkt_work.dest_pos = dest_pos;
        return num;
    }

    public static void gmBoss5RocketInitFlyDestDistance(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      int init_acc,
      int init_spd,
      int max_spd,
      ref VecFx32 launch_pos,
      int angle,
      int distance)
    {
        VecFx32 dest_pos;
        dest_pos.x = launch_pos.x + FX_Mul(distance, FX_F32_TO_FX32(nnCos(angle)));
        dest_pos.y = launch_pos.y + FX_Mul(distance, FX_F32_TO_FX32(nnSin(angle)));
        dest_pos.z = 0;
        gmBoss5RocketInitFlyDestPos(rkt_work, init_acc, init_spd, max_spd, launch_pos, ref dest_pos);
    }

    public static void gmBoss5RocketRedirectFlyDestPos(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      int init_acc,
      ref VecFx32 dest_pos)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        VecFx32 spd = obsObjectWork.spd;
        gmBoss5RocketInitFlyDestPos(rkt_work, init_acc, 4096, rkt_work.max_spd, obsObjectWork.pos, ref dest_pos);
        obsObjectWork.spd.Assign(spd);
    }

    public static int gmBoss5RocketUpdateFlyDest(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        return gmBoss5RocketUpdateFlyDest(rkt_work, 0);
    }

    public static int gmBoss5RocketUpdateFlyDest(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      int b_mdl_center)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
        VecFx32 ofst_pos_out = new VecFx32();
        if (FX_Sqrt(FX_Mul(obsObjectWork.spd.x, obsObjectWork.spd.x) + FX_Mul(obsObjectWork.spd.y, obsObjectWork.spd.y)) >= rkt_work.max_spd)
            obsObjectWork.spd_add.x = obsObjectWork.spd_add.y = obsObjectWork.spd_add.z = 0;
        amVectorSet(nnsVector1, FX_FX32_TO_F32(obsObjectWork.spd.x), FX_FX32_TO_F32(obsObjectWork.spd.y), 0.0f);
        if (b_mdl_center != 0)
            gmBoss5RocketGetDispOfst(rkt_work, ref ofst_pos_out);
        amVectorSet(nnsVector2, FX_FX32_TO_F32(rkt_work.dest_pos.x - (obsObjectWork.pos.x + ofst_pos_out.x)), FX_FX32_TO_F32(rkt_work.dest_pos.y - (obsObjectWork.pos.y + ofst_pos_out.y)), 0.0f);
        int num = nnDotProductVector(nnsVector1, nnsVector2) <= 0.0 ? 1 : 0;
        GlobalPool<NNS_VECTOR>.Release(nnsVector1);
        GlobalPool<NNS_VECTOR>.Release(nnsVector2);
        return num;
    }

    public static void gmBoss5RocketInitFlyReverse(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      int acc_scalar)
    {
        gmBoss5RocketInitFlyReverse(rkt_work, acc_scalar, 0);
    }

    public static void gmBoss5RocketInitFlyReverse(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      int acc_scalar,
      int is_add)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        amVectorSet(nnsVector, -FX_FX32_TO_F32(obsObjectWork.spd.x), -FX_FX32_TO_F32(obsObjectWork.spd.y), 0.0f);
        nnNormalizeVector(nnsVector, nnsVector);
        VecFx32 acc_vec = new VecFx32(FX_Mul(FX_F32_TO_FX32(nnsVector.x), acc_scalar), FX_Mul(FX_F32_TO_FX32(nnsVector.y), acc_scalar), 0);
        gmBoss5RocketInitFlyReverseVec(rkt_work, ref acc_vec, is_add);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    public static void gmBoss5RocketInitFlyReverseVec(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      ref VecFx32 acc_vec)
    {
        gmBoss5RocketInitFlyReverseVec(rkt_work, ref acc_vec, 0);
    }

    public static void gmBoss5RocketInitFlyReverseVec(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      ref VecFx32 acc_vec,
      int is_add)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        rkt_work.rvs_acc.x = acc_vec.x;
        rkt_work.rvs_acc.y = acc_vec.y;
        rkt_work.rvs_acc.z = 0;
        if (is_add != 0)
        {
            obsObjectWork.spd_add.x += rkt_work.rvs_acc.x;
            obsObjectWork.spd_add.y += rkt_work.rvs_acc.y;
            obsObjectWork.spd_add.z = 0;
        }
        else
            obsObjectWork.spd_add.Assign(rkt_work.rvs_acc);
    }

    public static int gmBoss5RocketUpdateFlyReverse(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
        amVectorSet(nnsVector1, FX_FX32_TO_F32(rkt_work.rvs_acc.x), FX_FX32_TO_F32(rkt_work.rvs_acc.y), 0.0f);
        amVectorSet(nnsVector2, FX_FX32_TO_F32(obsObjectWork.spd.x), FX_FX32_TO_F32(obsObjectWork.spd.y), 0.0f);
        int num = nnDotProductVector(nnsVector1, nnsVector2) >= 0.0 ? 1 : 0;
        GlobalPool<NNS_VECTOR>.Release(nnsVector1);
        GlobalPool<NNS_VECTOR>.Release(nnsVector2);
        return num;
    }

    public static void gmBoss5RocketSetInitialDir(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        obsObjectWork.dir.z = ((int)obsObjectWork.parent_obj.disp_flag & 1) == 0 ? (ushort)GMD_BOSS5_RKT_SEARCH_INITIAL_DIR_Z_R : (ushort)GMD_BOSS5_RKT_SEARCH_INITIAL_DIR_Z_L;
        if (rkt_work.rkt_type == 1)
            obsObjectWork.dir.x = (ushort)(ushort.MaxValue & (ulong)(obsObjectWork.dir.x + GMD_BOSS5_RKT_SEARCH_INITIAL_ADJ_DIR_X_RA));
        else
            obsObjectWork.dir.x = (ushort)(ushort.MaxValue & (ulong)(obsObjectWork.dir.x + GMD_BOSS5_RKT_SEARCH_INITIAL_ADJ_DIR_X_LA));
    }

    public static void gmBoss5RocketUpdateDirFollowingPos(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      ref VecFx32 targ_pos,
      float deg,
      int is_reverse)
    {
        gmBoss5RocketUpdateDirFollowingPos(rkt_work, targ_pos, deg, is_reverse, 0);
    }

    public static void gmBoss5RocketUpdateDirFollowingPos(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      VecFx32 targ_pos,
      float deg,
      int is_reverse,
      int force_rot_spd)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        if (is_reverse != 0)
            obsObjectWork.dir.z -= (ushort)AKM_DEGtoA32(180);
        int a1 = (int)(ushort.MaxValue & (long)((int)(ushort.MaxValue & (long)nnArcTan2(FX_FX32_TO_F32(targ_pos.y - obsObjectWork.pos.y), FX_FX32_TO_F32(targ_pos.x - obsObjectWork.pos.x))) - obsObjectWork.dir.z));
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
        if (is_reverse != 0)
            a2 += AKM_DEGtoA32(180);
        if (force_rot_spd != 0)
            a2 = force_rot_spd;
        obsObjectWork.dir.z = (ushort)(short)(ushort.MaxValue & (long)(obsObjectWork.dir.z + a2));
    }

    public static void gmBoss5RocketUpdateDirPlyLockOn(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      ref VecFx32 lock_pos)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        OBS_OBJECT_WORK parentObj = obsObjectWork.parent_obj;
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        if (((int)parentObj.disp_flag & 1) != 0)
        {
            if (playerObj.pos.x >= obsObjectWork.pos.x)
                gmBoss5RocketUpdateDirFollowingPos(rkt_work, lock_pos, 1f, 0, AKM_DEGtoA32(1f));
            else
                gmBoss5RocketUpdateDirFollowingPos(rkt_work, ref lock_pos, 1f, 0);
            gmBoss5RocketLimitDir(rkt_work, GMD_BOSS5_RKT_LOCKON_DIR_LIMIT_L_START, GMD_BOSS5_RKT_LOCKON_DIR_LIMIT_L_END);
        }
        else
        {
            if (playerObj.pos.x <= obsObjectWork.pos.x)
                gmBoss5RocketUpdateDirFollowingPos(rkt_work, lock_pos, 1f, 0, AKM_DEGtoA32(-1f));
            else
                gmBoss5RocketUpdateDirFollowingPos(rkt_work, ref lock_pos, 1f, 0);
            gmBoss5RocketLimitDir(rkt_work, GMD_BOSS5_RKT_LOCKON_DIR_LIMIT_R_START, GMD_BOSS5_RKT_LOCKON_DIR_LIMIT_R_END);
        }
    }

    public static void gmBoss5RocketUpdateDirFollowingAccSpd(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      float deg,
      int is_reverse)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        if (is_reverse != 0)
            obsObjectWork.dir.z -= (ushort)AKM_DEGtoA32(180);
        int num1 = (int)(ushort.MaxValue & (long)(nnArcTan2(FX_FX32_TO_F32(obsObjectWork.spd.y), FX_FX32_TO_F32(obsObjectWork.spd.x)) - obsObjectWork.dir.z));
        if (num1 >= AKM_DEGtoA32(180))
            num1 = -(AKM_DEGtoA32(360) - num1);
        int num2;
        if (obsObjectWork.spd_add.x != 0 || obsObjectWork.spd_add.y != 0)
        {
            num2 = (int)(ushort.MaxValue & (long)(nnArcTan2(FX_FX32_TO_F32(obsObjectWork.spd_add.y), FX_FX32_TO_F32(obsObjectWork.spd_add.x)) - obsObjectWork.dir.z));
            if (num2 >= AKM_DEGtoA32(180))
                num2 = -(AKM_DEGtoA32(360) - num2);
        }
        else
            num2 = num1;
        int a1 = (int)(ushort.MaxValue & (long)((num2 + num1) / 2));
        int a2;
        if (a1 >= AKM_DEGtoA32(180))
        {
            a1 = -(AKM_DEGtoA32(360) - a1);
            a2 = AKM_DEGtoA32(-deg);
        }
        else
            a2 = AKM_DEGtoA32(deg);
        if (MTM_MATH_ABS(a1) < MTM_MATH_ABS(a2))
            a2 = a1;
        if (is_reverse != 0)
            a2 += AKM_DEGtoA32(180);
        obsObjectWork.dir.z = (ushort)(short)(ushort.MaxValue & (long)(obsObjectWork.dir.z + a2));
    }

    public static void gmBoss5RocketLimitDir(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      int start_angle,
      int end_angle)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        int num1 = (int)(ushort.MaxValue & (long)((int)(ushort.MaxValue & (long)obsObjectWork.dir.z) - start_angle));
        int num2 = (int)(ushort.MaxValue & (long)(end_angle - start_angle));
        if (num1 <= num2)
            return;
        int num3 = num2 / 2 + AKM_DEGtoA32(180);
        if (num1 >= num3)
            obsObjectWork.dir.z = (ushort)(short)(ushort.MaxValue & (long)start_angle);
        else
            obsObjectWork.dir.z = (ushort)(short)(ushort.MaxValue & (long)end_angle);
    }

    public static void gmBoss5RocketInitDirFalling(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        rkt_work.pivot_fall_angle = AKM_DEGtoA32(90);
        rkt_work.wobble_sin_param_angle = AKM_DEGtoA32(0);
        obsObjectWork.dir.z = (ushort)(ushort.MaxValue & (ulong)rkt_work.pivot_fall_angle);
    }

    public static void gmBoss5RocketUpdateDirFalling(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        obsObjectWork.dir.x = (ushort)(ushort.MaxValue & (ulong)(obsObjectWork.dir.x + GMD_BOSS5_RKT_FALL_SPIN_ANGLE_SPD));
        rkt_work.wobble_sin_param_angle += GMD_BOSS5_RKT_FALL_WOBBLE_SIN_PARAM_DEG_SPD;
        rkt_work.wobble_sin_param_angle = (int)(ushort.MaxValue & (long)rkt_work.wobble_sin_param_angle);
        obsObjectWork.dir.z = (ushort)(ushort.MaxValue & (ulong)(rkt_work.pivot_fall_angle + AKM_DEGtoA32(15f * nnSin(rkt_work.wobble_sin_param_angle))));
    }

    public static void gmBoss5RocketEndDirFalling(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        rkt_work.pivot_fall_angle = 0;
        rkt_work.wobble_sin_param_angle = 0;
        obsObjectWork.dir.z = (ushort)AKM_DEGtoA32(90);
    }

    public static void gmBoss5RocketInitLeakageFlicker(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.wfall_atk_toggle_timer = 0U;
        rkt_work.flag &= 4294967287U;
    }

    public static int gmBoss5RocketUpdateLeakageFlicker(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (rkt_work.wfall_atk_toggle_timer != 0U)
            --rkt_work.wfall_atk_toggle_timer;
        else
            rkt_work.wfall_atk_toggle_timer = 39U;
        if (rkt_work.wfall_atk_toggle_timer < 20U)
        {
            rkt_work.flag &= 4294967287U;
            return 0;
        }
        rkt_work.flag |= 8U;
        return 1;
    }

    public static void gmBoss5RocketClearLeakageFlicker(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.wfall_atk_toggle_timer = 0U;
        rkt_work.flag &= 4294967287U;
    }

    public static void gmBoss5RocketInitFlyBlow(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj;
        obsObjectWork.move_flag |= 16U;
        obsObjectWork.move_flag &= 4294967166U;
        rkt_work.launch_pos.Assign(obsObjectWork.pos);
        rkt_work.dest_pos.Assign(parentObj.part_obj_core.pos);
        int ang = nnArcTan2(FX_FX32_TO_F32(rkt_work.dest_pos.y - obsObjectWork.pos.y), FX_FX32_TO_F32(rkt_work.dest_pos.x - obsObjectWork.pos.x));
        int fx32_1 = FX_F32_TO_FX32(nnCos(ang));
        int fx32_2 = FX_F32_TO_FX32(nnSin(ang));
        obsObjectWork.spd.x = FX_Mul(49152, fx32_1);
        obsObjectWork.spd.y = FX_Mul(49152, fx32_2);
        obsObjectWork.spd.z = 0;
        if (obsObjectWork.spd.x < 0)
            rkt_work.rot_spd = -GMD_BOSS5_RKT_BLOW_FLY_ROT_SPD;
        else
            rkt_work.rot_spd = GMD_BOSS5_RKT_BLOW_FLY_ROT_SPD;
    }

    public static int gmBoss5RocketUpdateFlyBlow(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
        obsObjectWork.dir.z = (ushort)(short)(ushort.MaxValue & (long)(obsObjectWork.dir.z + rkt_work.rot_spd));
        amVectorSet(nnsVector1, FX_FX32_TO_F32(rkt_work.dest_pos.x - rkt_work.launch_pos.x), FX_FX32_TO_F32(rkt_work.dest_pos.y - rkt_work.launch_pos.y), 0.0f);
        amVectorSet(nnsVector2, FX_FX32_TO_F32(rkt_work.dest_pos.x - obsObjectWork.pos.x), FX_FX32_TO_F32(rkt_work.dest_pos.y - obsObjectWork.pos.y), 0.0f);
        int num = nnDotProductVector(nnsVector1, nnsVector2) <= 0.0 ? 1 : 0;
        GlobalPool<NNS_VECTOR>.Release(nnsVector1);
        GlobalPool<NNS_VECTOR>.Release(nnsVector2);
        return num;
    }

    public static void gmBoss5RocketInitFlyBounce(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        obsObjectWork.move_flag |= 16U;
        obsObjectWork.move_flag &= 4294967166U;
        int fx32_1 = FX_F32_TO_FX32(nnCos(GMD_BOSS5_RKT_BOUNCE_DIR_ANGLE));
        int fx32_2 = FX_F32_TO_FX32(nnSin(GMD_BOSS5_RKT_BOUNCE_DIR_ANGLE));
        int num = 0;
        if (obsObjectWork.spd.x < 0)
            num = 1;
        obsObjectWork.spd.x = FX_Mul(16384, fx32_1);
        obsObjectWork.spd.y = FX_Mul(16384, fx32_2);
        obsObjectWork.spd.z = 0;
        if (num != 0)
            obsObjectWork.spd.x = -obsObjectWork.spd.x;
        if (obsObjectWork.spd.x < 0)
            rkt_work.rot_spd = -GMD_BOSS5_RKT_BOUNCE_FLY_ROT_SPD;
        else
            rkt_work.rot_spd = GMD_BOSS5_RKT_BOUNCE_FLY_ROT_SPD;
    }

    public static int gmBoss5RocketUpdateFlyBounce(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        obsObjectWork.dir.z = (ushort)(short)(ushort.MaxValue & (long)(obsObjectWork.dir.z + rkt_work.rot_spd));
        return obsObjectWork.pos.y <= GMM_BOSS5_AREA_TOP() - 196608 ? 1 : 0;
    }

    public static void gmBoss5RocketInitStuckLean(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        rkt_work.stuck_dir = obsObjectWork.dir.z;
        rkt_work.stuck_lean_ratio = 0.0f;
    }

    public static int gmBoss5RocketUpdateStuckLean(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK pObj = GMM_BS_OBJ(rkt_work);
        OBS_OBJECT_WORK parentObj = pObj.parent_obj;
        int num1 = 1;
        int num2 = 0;
        rkt_work.hit_vib_amp_deg -= 0.5f;
        if (rkt_work.hit_vib_amp_deg <= 0.0)
            rkt_work.hit_vib_amp_deg = 0.0f;
        rkt_work.hit_vib_sin_angle = (int)(ushort.MaxValue & (long)(rkt_work.hit_vib_sin_angle + GMD_BOSS5_RKT_GRD_STUCK_LEAN_HIT_VIB_SIN_ANGLE_ADD));
        float num3 = nnSin(rkt_work.hit_vib_sin_angle) * rkt_work.hit_vib_amp_deg;
        float n;
        if (((int)rkt_work.flag & 128) != 0)
        {
            float stuckLeanRatio = rkt_work.stuck_lean_ratio;
            if (rkt_work.stuck_lean_ratio < 1.0)
            {
                rkt_work.stuck_lean_ratio += 0.075f;
                num1 = 0;
            }
            rkt_work.stuck_lean_ratio = MTM_MATH_CLIP(rkt_work.stuck_lean_ratio, 0.0f, 1f);
            n = 180f * rkt_work.stuck_lean_ratio - num3;
            num2 = (int)(131072.0 * (rkt_work.stuck_lean_ratio - (double)stuckLeanRatio));
            ObjObjectFieldRectSet(pObj, -16, -16, 16, (short)(16 + (short)(rkt_work.stuck_lean_ratio * 8.0)));
        }
        else
            n = (int)rkt_work.hit_count * -10f + num3;
        if (pObj.pos.x < parentObj.pos.x)
        {
            n = -n;
            num2 = -num2;
        }
        pObj.dir.z = (ushort)(ushort.MaxValue & (ulong)(rkt_work.stuck_dir + AKM_DEGtoA32(n)));
        pObj.pos.x += num2;
        return num1;
    }

    public static void gmBoss5RocketSetStuckLeanHitVib(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.hit_vib_amp_deg = 10f;
        rkt_work.hit_vib_sin_angle = AKM_DEGtoA32(0);
    }

    public static void gmBoss5RocketInitScatter(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        GmBoss5ScatterSetFlyParam(obj_work);
        AkMathNormalizeMtx(nnsMatrix, obj_work.obj_3d.user_obj_mtx_r);
        nnMakeRotateMatrixQuaternion(out rkt_work.sct_cur_quat, nnsMatrix);
        nnMakeUnitQuaternion(ref rkt_work.sct_spin_quat);
        for (int index = 0; index < 2; ++index)
        {
            NNS_VECTOR dst_vec = GlobalPool<NNS_VECTOR>.Alloc();
            float rand_z = MTM_MATH_CLIP((float)(FX_FX32_TO_F32(AkMathRandFx()) * 2.0 - 1.0), -1f, 1f);
            short rand_angle = AKM_DEGtoA16(360f * FX_FX32_TO_F32(AkMathRandFx()));
            AkMathGetRandomUnitVector(dst_vec, rand_z, rand_angle);
            NNS_QUATERNION dst;
            nnMakeRotateAxisQuaternion(out dst, dst_vec.x, dst_vec.y, dst_vec.z, GMD_BOSS5_SCT_SPIN_SPD_ANGLE);
            nnMultiplyQuaternion(ref rkt_work.sct_spin_quat, ref dst, ref rkt_work.sct_spin_quat);
            GlobalPool<NNS_VECTOR>.Release(dst_vec);
        }
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    public static void gmBoss5RocketUpdateScatter(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        nnMultiplyQuaternion(ref rkt_work.sct_cur_quat, ref rkt_work.sct_spin_quat, ref rkt_work.sct_cur_quat);
        nnMakeQuaternionMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, ref rkt_work.sct_cur_quat);
        obsObjectWork.disp_flag |= 16777216U;
    }

    public static int gmBoss5RocketReceiveSignalLaunch(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(rkt_work).parent_obj;
        if (rkt_work.rkt_type == 0 && ((int)parentObj.flag & 268435456) != 0)
        {
            parentObj.flag &= 4026531839U;
            return 1;
        }
        if (rkt_work.rkt_type != 1 || ((int)parentObj.flag & 134217728) == 0)
            return 0;
        parentObj.flag &= 4160749567U;
        return 1;
    }

    public static int gmBoss5RocketReceiveSignalReturn(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(rkt_work).parent_obj;
        if (rkt_work.rkt_type == 0 && ((int)parentObj.flag & 1073741824) != 0)
        {
            parentObj.flag &= 3221225471U;
            return 1;
        }
        if (rkt_work.rkt_type != 1 || ((int)parentObj.flag & 536870912) == 0)
            return 0;
        parentObj.flag &= 3758096383U;
        return 1;
    }

    public static void gmBoss5RocketDispatchSignalReturned(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(rkt_work).parent_obj;
        if (rkt_work.rkt_type == 0)
        {
            parentObj.flag |= 67108864U;
        }
        else
        {
            if (rkt_work.rkt_type != 1)
                return;
            parentObj.flag |= 33554432U;
        }
    }

    public static void gmBoss5RocketInitCallbacks(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        GmBsCmnInitBossMotionCBSystem(obj_work, rkt_work.bmcb_mgr);
        GmBsCmnCreateSNMWork(rkt_work.snm_work, obj_work.obj_3d._object, 1);
        GmBsCmnAppendBossMotionCallback(rkt_work.bmcb_mgr, rkt_work.snm_work.bmcb_link);
        rkt_work.drill_snm_reg_id = GmBsCmnRegisterSNMNode(rkt_work.snm_work, 3);
    }

    public static void gmBoss5RocketReleaseCallbacks(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        GmBsCmnClearBossMotionCBSystem(GMM_BS_OBJ(rkt_work));
        GmBsCmnDeleteSNMWork(rkt_work.snm_work);
    }

    public static void gmBoss5RocketAtkPlyHitFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        ((GMS_BOSS5_ROCKET_WORK)my_rect.parent_obj).flag |= 1U;
        GmEnemyDefaultAtkFunc(my_rect, your_rect);
    }

    public static void gmBoss5RocketAtkBossHitFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        GMS_BOSS5_ROCKET_WORK rkt_work = (GMS_BOSS5_ROCKET_WORK)parentObj1;
        if (((GMS_ENEMY_COM_WORK)parentObj2).eve_rec.id == 330)
        {
            parentObj1.flag |= 2U;
            gmBoss5RocketBounceProcInit(rkt_work);
        }
        else
            ObjRectFuncNoHit(my_rect, your_rect);
    }

    public static void gmBoss5RocketDamageDefFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        GMS_BOSS5_ROCKET_WORK parentObj1 = (GMS_BOSS5_ROCKET_WORK)my_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        if (parentObj2 == null || 1 != parentObj2.obj_type || ((int)((GMS_PLAYER_WORK)parentObj2).obj_work.move_flag & 1) != 0)
            return;
        int num = gmBoss5RocketSetPlyRebound(my_rect, your_rect);
        if (((int)parentObj1.flag & 2) != 0)
        {
            if (num != 0)
            {
                ++parentObj1.hit_count;
                if (parentObj1.hit_count >= 3U)
                {
                    gmBoss5RocketClearLeakageFlicker(parentObj1);
                    gmBoss5RocketBlowProcInit(parentObj1);
                }
                else
                {
                    gmBoss5RocketSetNoHitTime(parentObj1);
                    gmBoss5RocketSetStuckLeanHitVib(parentObj1);
                }
            }
            else
            {
                gmBoss5RocketSetDmgEnable(parentObj1, 0);
                GmBoss5EfctCreateRocketRollSpark(parentObj1);
                parentObj1.flag |= 128U;
            }
        }
        else
        {
            gmBoss5RocketClearLeakageFlicker(parentObj1);
            gmBoss5RocketBlowProcInit(parentObj1);
        }
        GmSoundPlaySE("Boss0_01");
    }

    public static void gmBoss5RocketOutFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_ROCKET_WORK gmsBosS5RocketWork = (GMS_BOSS5_ROCKET_WORK)obj_work;
        ObjDrawActionSummary(obj_work);
        gmsBosS5RocketWork.pivot_prev_pos.Assign(obj_work.pos);
    }

    public static void gmBoss5RocketMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_ROCKET_WORK gmsBosS5RocketWork = (GMS_BOSS5_ROCKET_WORK)obj_work;
        if (((int)gmsBosS5RocketWork.flag & 2) != 0 && ((int)gmsBosS5RocketWork.flag & 128) == 0)
            gmBoss5RocketUpdateNoHitTime(gmsBosS5RocketWork);
        if (gmsBosS5RocketWork.proc_update != null)
            gmsBosS5RocketWork.proc_update(gmsBosS5RocketWork);
        if (((int)gmsBosS5RocketWork.flag & 8) != 0)
            GmBoss5EfctTryStartRocketLeakage(gmsBosS5RocketWork);
        else
            GmBoss5EfctEndRocketLeakage(gmsBosS5RocketWork);
        gmBoss5RocketUpdateMainRectPosition(gmsBosS5RocketWork);
    }

    public static void gmBoss5RocketNmlProcInit(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        gmBoss5RocketSetInitialDir(rkt_work);
        gmBoss5RocketSetRectSize(rkt_work, 1);
        gmBoss5RocketSetAtkEnable(rkt_work, 0);
        gmBoss5RocketSetDmgEnable(rkt_work, 0);
        gmBoss5RocketInitPlySearch(rkt_work, 20);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketNmlProcUpdateFace);
    }

    public static void gmBoss5RocketNmlProcUpdateFace(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 0);
        gmBoss5RocketUpdatePlySearch(rkt_work);
        VecFx32 pos;
        gmBoss5RocketGetPlySearchPos(rkt_work, out pos);
        gmBoss5RocketUpdateDirPlyLockOn(rkt_work, ref pos);
        if (gmBoss5RocketReceiveSignalLaunch(rkt_work) == 0)
            return;
        gmBoss5RocketInitFlyDestDistance(rkt_work, 4096, 40960, 49152, ref obsObjectWork.pos, obsObjectWork.dir.z, 786432);
        gmBoss5RocketSetAtkEnable(rkt_work, 1);
        GmBoss5EfctCreateRocketLaunch(rkt_work);
        GmSoundPlaySE("FinalBoss07");
        GmBoss5EfctStartRocketJet(rkt_work);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketNmlProcUpdateFly);
    }

    public static void gmBoss5RocketNmlProcUpdateFly(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (gmBoss5RocketUpdateFlyDest(rkt_work) == 0)
            return;
        gmBoss5RocketInitFlyReverse(rkt_work, 4096, 0);
        GmBoss5EfctEndRocketJet(rkt_work);
        GmBoss5EfctStartRocketJetReverse(rkt_work);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketNmlProcUpdateWaitDecel);
    }

    public static void gmBoss5RocketNmlProcUpdateWaitDecel(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        if (gmBoss5RocketUpdateFlyReverse(rkt_work) == 0)
            return;
        GmBsCmnSetObjSpdZero(obj_work);
        VecFx32 pos_out;
        gmBoss5RocketGetArmNodePosFx(rkt_work, out pos_out);
        int num = gmBoss5RocketInitFlyDestPos(rkt_work, 2048, 0, 40960, obj_work.pos, ref pos_out);
        obj_work.dir.z = (ushort)(short)(ushort.MaxValue & (long)(num + AKM_DEGtoA32(180)));
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketNmlProcUpdateWaitReturn);
    }

    public static void gmBoss5RocketNmlProcUpdateWaitReturn(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        if (gmBoss5RocketUpdateFlyDest(rkt_work, 1) == 0)
            return;
        gmBoss5RocketDispatchSignalReturned(rkt_work);
        GmBoss5EfctEndRocketJet(rkt_work);
        GmBoss5EfctCreateRocketDock((GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj, rkt_work.rkt_type);
        gmBoss5RocketSetAtkEnable(rkt_work, 0);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketNmlProcUpdateFinalize);
    }

    public static void gmBoss5RocketNmlProcUpdateFinalize(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5RocketStrProcInit(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        gmBoss5RocketSetInitialDir(rkt_work);
        gmBoss5RocketSetRectSize(rkt_work, 1);
        gmBoss5RocketSetAtkEnable(rkt_work, 0);
        gmBoss5RocketSetDmgEnable(rkt_work, 0);
        gmBoss5RocketInitPlySearch(rkt_work, 20);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketStrProcUpdateFace);
    }

    public static void gmBoss5RocketStrProcUpdateFace(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 0);
        gmBoss5RocketUpdatePlySearch(rkt_work);
        VecFx32 pos;
        gmBoss5RocketGetPlySearchPos(rkt_work, out pos);
        gmBoss5RocketUpdateDirPlyLockOn(rkt_work, ref pos);
        if (gmBoss5RocketReceiveSignalReturn(rkt_work) != 0)
        {
            gmBoss5RocketInitRocketStuckWithArmLerpRot(rkt_work, 0.1f);
            gmBoss5RocketUpdateRocketStuckWithArmLerpRot(rkt_work);
            rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketStrProcUpdateRecover);
        }
        else
        {
            if (gmBoss5RocketReceiveSignalLaunch(rkt_work) == 0)
                return;
            gmBoss5RocketSetDispOfst(rkt_work, -10f, 1);
            gmBoss5RocketInitFlyDestDistance(rkt_work, 6144, 40960, 61440, ref obsObjectWork.pos, obsObjectWork.dir.z, 786432);
            gmBoss5RocketSetAtkEnable(rkt_work, 1);
            GmBoss5EfctCreateRocketLaunch(rkt_work);
            GmSoundPlaySE("FinalBoss07");
            GmBoss5EfctStartRocketJet(rkt_work);
            rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketStrProcUpdateFlyTarget);
        }
    }

    public static void gmBoss5RocketStrProcUpdateFlyTarget(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (gmBoss5RocketUpdateFlyDest(rkt_work) == 0)
            return;
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        VecFx32 dest_pos = new VecFx32(obsObjectWork.pos.x, GMM_BOSS5_AREA_TOP() - 196608, 0);
        VecFx32 acc_vec = new VecFx32(-2048, 0, 0);
        gmBoss5RocketRedirectFlyDestPos(rkt_work, 2048, ref dest_pos);
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            acc_vec.x = -acc_vec.x;
        gmBoss5RocketInitFlyReverseVec(rkt_work, ref acc_vec, 1);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketStrProcUpdateFlyDecel);
    }

    public static void gmBoss5RocketStrProcUpdateFlyDecel(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        gmBoss5RocketUpdateDirFollowingAccSpd(rkt_work, 5f, 0);
        if (gmBoss5RocketUpdateFlyReverse(rkt_work) == 0)
            return;
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        VecFx32 dest_pos = new VecFx32(obsObjectWork.pos.x, GMM_BOSS5_AREA_TOP() - 196608, 0);
        gmBoss5RocketInitFlyDestPos(rkt_work, 2048, MTM_MATH_ABS(obsObjectWork.spd.y), 61440, obsObjectWork.pos, ref dest_pos);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketStrProcUpdateFlyAbove);
    }

    public static void gmBoss5RocketStrProcUpdateFlyAbove(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        gmBoss5RocketUpdateDirFollowingAccSpd(rkt_work, 1f, 0);
        if (gmBoss5RocketUpdateFlyDest(rkt_work) == 0)
            return;
        GmBsCmnSetObjSpdZero(obj_work);
        gmBoss5RocketSetAtkEnable(rkt_work, 0);
        rkt_work.wait_timer = gmBoss5RocketSeqGetWaitFallTime(rkt_work);
        GmBoss5EfctEndRocketJet(rkt_work);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketStrProcUpdateWaitFall);
    }

    public static void gmBoss5RocketStrProcUpdateWaitFall(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        if (rkt_work.wait_timer >= 10U)
            obsObjectWork.pos.x = GmBsCmnGetPlayerObj().pos.x;
        if (rkt_work.wait_timer != 0U)
        {
            --rkt_work.wait_timer;
        }
        else
        {
            GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obsObjectWork;
            obsObjectWork.spd.y = 8192;
            gmBoss5RocketSetRectSize(rkt_work, 1);
            gmBoss5RocketSetAtkEnable(rkt_work, 1);
            gmBoss5RocketSetDmgEnable(rkt_work, 1);
            gmBoss5RocketInitLeakageFlicker(rkt_work);
            GmBoss5EfctStartRocketSmoke(rkt_work);
            gmBoss5RocketInitDirFalling(rkt_work);
            gmsEnemyComWork.enemy_flag &= 4294934527U;
            rkt_work.flag &= 4294967294U;
            rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketStrProcUpdateFall);
        }
    }

    public static void gmBoss5RocketStrProcUpdateFall(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        gmBoss5RocketUpdateDirFalling(rkt_work);
        if (gmBoss5RocketUpdateLeakageFlicker(rkt_work) != 0)
            gmBoss5RocketSetRectSize(rkt_work, 1);
        else
            gmBoss5RocketSetRectSize(rkt_work, 0);
        if (obj_work.pos.y >= GMM_BOSS5_AREA_TOP())
            obj_work.move_flag &= 4294967039U;
        if (((int)obj_work.move_flag & 1) == 0)
            return;
        GmBsCmnSetObjSpdZero(obj_work);
        rkt_work.flag |= 2U;
        gmBoss5RocketSetRectSize(rkt_work, 0);
        gmBoss5RocketSetAtkEnable(rkt_work, 1);
        gmBoss5RocketSetDmgEnable(rkt_work, 1);
        rkt_work.wait_timer = ((int)rkt_work.flag & 1) == 0 ? 300U : 30U;
        gmBoss5RocketClearLeakageFlicker(rkt_work);
        GmBoss5EfctCreateRocketLandingShockwave(rkt_work);
        GmSoundPlaySE("FinalBoss13");
        gmBoss5RocketEndDirFalling(rkt_work);
        gmBoss5RocketInitStuckLean(rkt_work);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketStrProcUpdateStuck);
    }

    public static void gmBoss5RocketStrProcUpdateStuck(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        int num = gmBoss5RocketUpdateStuckLean(rkt_work);
        if (num != 0 && ((int)rkt_work.flag & 128) != 0 && ((int)rkt_work.flag & 256) == 0)
        {
            gmBoss5RocketSetStuckLeanHitVib(rkt_work);
            rkt_work.flag |= 256U;
        }
        if (((int)rkt_work.flag & 128) != 0)
            rkt_work.flag |= 8U;
        else
            rkt_work.flag &= 4294967287U;
        if (rkt_work.wait_timer != 0U)
        {
            --rkt_work.wait_timer;
        }
        else
        {
            if (num == 0)
                return;
            GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obsObjectWork;
            rkt_work.flag &= 4294967287U;
            GmBoss5EfctEndRocketSmoke(rkt_work);
            VecFx32 pos_out;
            gmBoss5RocketGetArmNodePosFx(rkt_work, out pos_out);
            gmBoss5RocketInitFlyDestPos(rkt_work, 1228, 0, 61440, obsObjectWork.pos, ref pos_out);
            rkt_work.flag &= 4294967293U;
            gmBoss5RocketSetRectSize(rkt_work, 1);
            gmBoss5RocketSetAtkEnable(rkt_work, 1);
            gmBoss5RocketSetDmgEnable(rkt_work, 0);
            gmsEnemyComWork.enemy_flag |= 32768U;
            rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketStrProcUpdateReturn);
        }
    }

    public static void gmBoss5RocketStrProcUpdateReturn(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        if (((int)rkt_work.flag & 128) != 0)
            gmBoss5RocketUpdateDirFollowingAccSpd(rkt_work, 5f, 1);
        else
            gmBoss5RocketUpdateDirFollowingAccSpd(rkt_work, 2f, 1);
        if (gmBoss5RocketUpdateFlyDest(rkt_work, 1) == 0)
            return;
        GmBoss5EfctCreateRocketDock((GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj, rkt_work.rkt_type);
        gmBoss5RocketDispatchSignalReturned(rkt_work);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketStrProcUpdateFinalize);
    }

    public static void gmBoss5RocketStrProcUpdateFinalize(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5RocketStrProcUpdateRecover(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj;
        gmBoss5RocketUpdateRocketStuckWithArmLerpRot(rkt_work);
        if ((rkt_work.rkt_type != 0 || ((int)parentObj.flag & 4) != 0) && (rkt_work.rkt_type != 1 || ((int)parentObj.flag & 4) != 0))
            return;
        obsObjectWork.flag |= 4U;
    }

    public static void gmBoss5RocketBlowProcInit(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obsObjectWork;
        GmPlayerAddScore((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), 100, obsObjectWork.pos.x, obsObjectWork.pos.y);
        GmBoss5EfctEndRocketSmoke(rkt_work);
        rkt_work.flag &= 4294967293U;
        obsObjectWork.move_flag |= 16U;
        obsObjectWork.move_flag &= 4294967294U;
        gmsEnemyComWork.enemy_flag |= 32768U;
        gmBoss5RocketSetAtkBodyRect(rkt_work);
        gmBoss5RocketSetRectSize(rkt_work, 1);
        gmBoss5RocketSetAtkEnable(rkt_work, 1);
        gmBoss5RocketSetDmgEnable(rkt_work, 0);
        gmBoss5RocketInitFlyBlow(rkt_work);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketBlowProcUpdateFly);
    }

    public static void gmBoss5RocketBlowProcUpdateFly(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (gmBoss5RocketUpdateFlyBlow(rkt_work) == 0)
            return;
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketBlowProcUpdateWaitHit);
    }

    public static void gmBoss5RocketBlowProcUpdateWaitHit(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        gmBoss5RocketUpdateFlyBlow(rkt_work);
        obj_work.pos.x = rkt_work.dest_pos.x;
        obj_work.pos.y = rkt_work.dest_pos.y;
        GmBsCmnSetObjSpdZero(obj_work);
    }

    public static void gmBoss5RocketBounceProcInit(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obsObjectWork;
        obsObjectWork.move_flag |= 16U;
        obsObjectWork.move_flag &= 4294967294U;
        gmsEnemyComWork.enemy_flag |= 32768U;
        gmBoss5RocketSetAtkEnable(rkt_work, 0);
        gmBoss5RocketSetDmgEnable(rkt_work, 0);
        gmBoss5RocketInitFlyBounce(rkt_work);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketBounceProcUpdateFlyUp);
    }

    public static void gmBoss5RocketBounceProcUpdateFlyUp(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        if (gmBoss5RocketUpdateFlyBounce(rkt_work) == 0)
            return;
        GmBsCmnSetObjSpdZero(obj_work);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketBounceProcUpdateWait);
    }

    public static void gmBoss5RocketBounceProcUpdateWait(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        if (gmBoss5RocketReceiveSignalReturn(rkt_work) == 0)
            return;
        VecFx32 pos_out;
        gmBoss5RocketGetArmNodePosFx(rkt_work, out pos_out);
        gmBoss5RocketInitFlyDestPos(rkt_work, 1228, 0, 61440, obsObjectWork.pos, ref pos_out);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketBounceProcUpdateReturn);
    }

    public static void gmBoss5RocketBounceProcUpdateReturn(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        gmBoss5RocketUpdateDirFollowingAccSpd(rkt_work, 5f, 1);
        if (gmBoss5RocketUpdateFlyDest(rkt_work, 1) == 0)
            return;
        GmBoss5EfctCreateRocketDock((GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj, rkt_work.rkt_type);
        gmBoss5RocketDispatchSignalReturned(rkt_work);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketBounceProcUpdateFinalize);
    }

    public static void gmBoss5RocketBounceProcUpdateFinalize(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(rkt_work);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5RocketCnctProcInit(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 1);
        gmBoss5RocketSetRectSize(rkt_work, 1);
        gmBoss5RocketSetAtkEnable(rkt_work, 0);
        gmBoss5RocketSetDmgEnable(rkt_work, 0);
        rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketCnctProcUpdateIdle);
    }

    public static void gmBoss5RocketCnctProcUpdateIdle(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj;
        gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 1);
        if (rkt_work.rkt_type == 0 && ((int)parentObj.flag & 4) != 0 || rkt_work.rkt_type == 1 && ((int)parentObj.flag & 8) != 0)
        {
            obsObjectWork.disp_flag |= 32U;
            gmBoss5RocketSetAtkEnable(rkt_work, 0);
        }
        else
        {
            obsObjectWork.disp_flag &= 4294967263U;
            if (((int)parentObj.flag & 256) != 0)
                gmBoss5RocketSetAtkEnable(rkt_work, 1);
            else
                gmBoss5RocketSetAtkEnable(rkt_work, 0);
            if (((int)parentObj.flag & 262144) == 0)
                return;
            rkt_work.wait_timer = rkt_work.rkt_type != 0 ? 30U : 10U;
            rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketCnctProcUpdateWaitScatterStart);
        }
    }

    public static void gmBoss5RocketCnctProcUpdateWaitScatterStart(
      GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 1);
        if (rkt_work.wait_timer != 0U)
        {
            --rkt_work.wait_timer;
        }
        else
        {
            gmBoss5RocketInitScatter(rkt_work);
            rkt_work.wait_timer = 180U;
            rkt_work.proc_update = new MPP_VOID_GMS_BOSS5_ROCKET_WORK(gmBoss5RocketCnctProcUpdateScatter);
        }
    }

    public static void gmBoss5RocketCnctProcUpdateScatter(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(rkt_work);
        gmBoss5RocketUpdateScatter(rkt_work);
        if (rkt_work.wait_timer != 0U)
            --rkt_work.wait_timer;
        else
            obsObjectWork.flag |= 4U;
    }

    public static uint gmBoss5RocketSeqGetWaitFallTime(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        int life = ((GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(rkt_work).parent_obj).mgr_work.life;
        GMS_BOSS5_RKT_SEQ_WAITFALL_INFO rktSeqWaitfallInfo = null;
        int index1 = 0;
        int num1 = 0;
        for (int index2 = 0; index2 < 3; ++index2)
        {
            if (life <= gm_boss5_rkt_seq_wait_fall_time_tbl[index2].life_threshold)
            {
                rktSeqWaitfallInfo = gm_boss5_rkt_seq_wait_fall_time_tbl[index2];
                break;
            }
        }
        if (rktSeqWaitfallInfo == null)
            return 0;
        int num2 = AkMathRandFx();
        for (int index2 = 0; index2 < 3; ++index2)
        {
            int num3 = rktSeqWaitfallInfo.probability[index2];
            if (num3 > 0)
            {
                index1 = index2;
                num1 += num3;
                if (num2 <= num1)
                    return rktSeqWaitfallInfo.frame[index2];
            }
        }
        return rktSeqWaitfallInfo.frame[index1];
    }

}