public partial class AppMain
{
    public static AMS_AMB_HEADER GMD_BOSS1_ARC => g_gm_gamedat_enemy_arc;

    public static int GMM_BOSS1_STAGE_MAP_POS_OFST_Y()
    {
        return GMM_MAIN_GET_ZONE_TYPE() != 4 ? 0 : 14155776;
    }

    public static int GMD_BOSS1_GROUND_POS_Y => 1286144 + GMM_BOSS1_STAGE_MAP_POS_OFST_Y();

    public static int GMD_BOSS1_BODY_DEFAULT_ALTITUDE => 712704 + GMM_BOSS1_STAGE_MAP_POS_OFST_Y();

    public static int GMD_BOSS1_BODY_START_POS_Y => GMM_BOSS1_STAGE_MAP_POS_OFST_Y() - 245760;

    public static int GMD_BOSS1_BODY_ATKBASH_TARG_Y => GMD_BOSS1_GROUND_POS_Y - 360448;

    private static OBS_OBJECT_WORK GmBoss1BodyInit(
     GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS1_BODY_WORK(), "BOSS1_BODY");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS1_BODY_WORK body_work = (GMS_BOSS1_BODY_WORK)work;
        work.pos.y = GMD_BOSS1_BODY_START_POS_Y;
        work.pos.z = 0;
        body_work.atk_nml_alt = GMD_BOSS1_BODY_DEFAULT_ALTITUDE;
        work.flag |= 16U;
        work.disp_flag |= 4194309U;
        work.move_flag |= 4096U;
        work.move_flag &= 4294967167U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        gmsEnemy3DWork.ene_com.vit = 1;
        gmBoss1BodySetDmgRectSizeToDefault(body_work);
        gmsEnemy3DWork.ene_com.rect_work[0].ppDef = new OBS_RECT_WORK_Delegate1(gmBoss1BodyDamageDefFunc);
        gmsEnemy3DWork.ene_com.rect_work[1].flag |= 2048U;
        ObjObjectCopyAction3dNNModel(work, gm_boss1_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(703), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = 0.125f;
        work.disp_flag |= 134217728U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1BodyWaitSetup);
        gmBoss1BodyChangeState(body_work, 0);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1BodyOutFunc);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss1BodyExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss1BodyExit(MTS_TASK_TCB tcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        GMS_BOSS1_BODY_WORK gmsBosS1BodyWork = (GMS_BOSS1_BODY_WORK)tcbWork;
        gmBoss1MgrDecObjCreateCount(gmsBosS1BodyWork.mgr_work);
        GmBsCmnClearBossMotionCBSystem(tcbWork);
        GmBsCmnDeleteSNMWork(gmsBosS1BodyWork.snm_work);
        GmBsCmnClearCNMCb(tcbWork);
        GmBsCmnDeleteCNMMgrWork(gmsBosS1BodyWork.cnm_mgr_work);
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss1BodySetActionWhole(GMS_BOSS1_BODY_WORK body_work, int act_id)
    {
        gmBoss1BodySetActionWhole(body_work, act_id, false);
    }

    private static void gmBoss1BodySetActionWhole(
      GMS_BOSS1_BODY_WORK body_work,
      int act_id,
      bool force_change)
    {
        GMS_BOSS1_PART_ACT_INFO[] bosS1PartActInfoArray = gm_boss1_act_id_tbl[act_id];
        if (!force_change && body_work.whole_act_id == act_id)
            return;
        body_work.whole_act_id = act_id;
        for (int index = 0; index < 3; ++index)
        {
            if (body_work.parts_objs[index] != null)
            {
                if (index == 2)
                {
                    GMS_BOSS1_EGG_WORK partsObj = (GMS_BOSS1_EGG_WORK)body_work.parts_objs[index];
                    body_work.egg_revert_mtn_id = bosS1PartActInfoArray[index].act_id;
                    if (((int)partsObj.flag & 1) != 0)
                        continue;
                }
                if (bosS1PartActInfoArray[index].is_maintain == 0)
                    GmBsCmnSetAction(body_work.parts_objs[index], bosS1PartActInfoArray[index].act_id, bosS1PartActInfoArray[index].is_repeat, bosS1PartActInfoArray[index].is_blend ? 1 : 0);
                else if (bosS1PartActInfoArray[index].is_repeat != 0)
                    GMM_BS_OBJ(body_work).disp_flag |= 4U;
                if (bosS1PartActInfoArray[index].is_blend && bosS1PartActInfoArray[index].is_merge_manual)
                {
                    if (index == 1)
                    {
                        GMS_BOSS1_CHAIN_WORK partsObj = (GMS_BOSS1_CHAIN_WORK)body_work.parts_objs[1];
                        partsObj.flag |= 1U;
                        GMM_BS_OBJ(partsObj).disp_flag |= 4U;
                    }
                    else
                        MTM_ASSERT(false);
                }
                body_work.parts_objs[index].obj_3d.speed[0] = bosS1PartActInfoArray[index].mtn_spd;
                body_work.parts_objs[index].obj_3d.blend_spd = bosS1PartActInfoArray[index].blend_spd;
            }
        }
    }

    private static void gmBoss1BodySetSuspendAction(
      GMS_BOSS1_BODY_WORK body_work,
      int part_idx,
      uint suspend_time)
    {
        OBS_ACTION3D_NN_WORK obj3d = body_work.parts_objs[part_idx].obj_3d;
        MTM_ASSERT(part_idx == 1);
        obj3d.speed[0] = 0.0f;
        body_work.mtn_suspend[part_idx].is_suspended = true;
        body_work.mtn_suspend[part_idx].suspend_timer = suspend_time;
    }

    private static void gmBoss1BodyUpdateSuspendAction(GMS_BOSS1_BODY_WORK body_work)
    {
        for (int index = 0; index < 3; ++index)
        {
            GMS_BOSS1_MTN_SUSPEND_WORK s1MtnSuspendWork = body_work.mtn_suspend[index];
            if (s1MtnSuspendWork.is_suspended)
            {
                MTM_ASSERT(index == 1);
                if (s1MtnSuspendWork.suspend_timer != 0U)
                {
                    --s1MtnSuspendWork.suspend_timer;
                }
                else
                {
                    body_work.parts_objs[index].obj_3d.speed[0] = gm_boss1_act_id_tbl[body_work.whole_act_id][index].mtn_spd;
                    s1MtnSuspendWork.is_suspended = false;
                }
            }
        }
    }

    private static bool gmBoss1BodyCheckChainMotionMergeEnd(GMS_BOSS1_BODY_WORK body_work)
    {
        return ((int)((GMS_BOSS1_CHAIN_WORK)body_work.parts_objs[1]).flag & 1) == 0;
    }

    private static void gmBoss1BodySetNoHitTime(GMS_BOSS1_BODY_WORK body_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)body_work;
        body_work.no_hit_timer = 10U;
        gmsEnemyComWork.rect_work[0].flag |= 2048U;
    }

    private static void gmBoss1BodyUpdateNoHitTime(GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.no_hit_timer != 0U)
            --body_work.no_hit_timer;
        else
            ((GMS_ENEMY_COM_WORK)body_work).rect_work[0].flag &= 4294965247U;
    }

    private static void gmBoss1BodyExecDamageRoutine(GMS_BOSS1_BODY_WORK body_work)
    {
        GMS_BOSS1_MGR_WORK mgrWork = body_work.mgr_work;
        MTM_ASSERT(mgrWork);
        if (mgrWork.life != 0)
            --mgrWork.life;
        if (0 < mgrWork.life)
        {
            body_work.flag |= 1073741824U;
        }
        else
        {
            body_work.flag |= 2147483648U;
            GMM_BS_OBJ(body_work).flag |= 2U;
        }
    }

    private static void gmBoss1BodySetDmgRectSizeForAtkNml(GMS_BOSS1_BODY_WORK body_work)
    {
        ObjRectWorkSet(((GMS_ENEMY_COM_WORK)body_work).rect_work[0], -24, -24, 24, 16);
    }

    private static void gmBoss1BodySetDmgRectSizeToDefault(GMS_BOSS1_BODY_WORK body_work)
    {
        ObjRectWorkSet(((GMS_ENEMY_COM_WORK)body_work).rect_work[0], -24, -24, 24, 24);
    }

    private static void gmBoss1BodySetAtkRectToWeakAttacker(GMS_BOSS1_BODY_WORK body_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)body_work;
        ObjRectWorkSet(gmsEnemyComWork.rect_work[1], -16, -16, 16, 16);
        gmsEnemyComWork.rect_work[1].flag &= 4294965247U;
    }

    private static void gmBoss1BodySetAtkRectToNormal(GMS_BOSS1_BODY_WORK body_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)body_work;
        ObjRectWorkSet(gmsEnemyComWork.rect_work[1], 0, 0, 0, 0);
        gmsEnemyComWork.rect_work[1].flag |= 2048U;
    }

    private static bool gmBoss1BodyIsExtraAttack(GMS_BOSS1_BODY_WORK body_work)
    {
        return GMM_BOSS1_MGR(body_work).life <= 3;
    }

    private static bool gmBoss1BodyIsEscapeScrUnlock(GMS_BOSS1_BODY_WORK body_work)
    {
        return GMM_BS_OBJ(body_work).pos.x >= GMM_BOSS1_AREA_RIGHT() - 131072;
    }

    private static bool gmBoss1BodyIsEscapeOutFinalZone(GMS_BOSS1_BODY_WORK body_work)
    {
        return GMM_BS_OBJ(body_work).pos.x >= GMM_BOSS1_AREA_RIGHT() + 393216;
    }

    private static bool gmBoss1BodyIsDirectionPositiveFromCurrent(
      GMS_BOSS1_BODY_WORK body_work,
      short target_angle)
    {
        return (int)(ushort.MaxValue & (long)(body_work.cur_angle - target_angle)) >= AKM_DEGtoA32(180);
    }

    private static void gmBoss1BodyUpdateDirection(GMS_BOSS1_BODY_WORK body_work)
    {
        GMM_BS_OBJ(body_work).dir.y = (ushort)body_work.cur_angle;
    }

    private static void gmBoss1BodySetDirectionNormal(GMS_BOSS1_BODY_WORK body_work)
    {
        if (((int)GMM_BS_OBJ(body_work).disp_flag & 1) != 0)
            gmBoss1BodySetDirection(body_work, GMD_BOSS1_LEFTWARD_ANGLE);
        else
            gmBoss1BodySetDirection(body_work, GMD_BOSS1_RIGHTWARD_ANGLE);
        body_work.orig_angle = 0;
        body_work.turn_angle = 0;
    }

    private static void gmBoss1BodySetDirection(GMS_BOSS1_BODY_WORK body_work, short deg)
    {
        body_work.cur_angle = deg;
    }

    private static void gmBoss1BodyInitTurnGently(
      GMS_BOSS1_BODY_WORK body_work,
      short dest_angle,
      int frame,
      bool is_positive)
    {
        MTM_ASSERT(frame > 0);
        body_work.orig_angle = body_work.cur_angle;
        body_work.turn_angle = 0;
        body_work.turn_spd = 0;
        if (is_positive)
        {
            ushort num = (ushort)((uint)dest_angle - (uint)body_work.cur_angle);
            body_work.turn_amount = num;
        }
        else
        {
            ushort num = (ushort)(dest_angle - AKM_DEGtoA32(360) - (body_work.cur_angle - AKM_DEGtoA32(360)));
            body_work.turn_amount = num - AKM_DEGtoA32(360);
        }
        body_work.turn_gen_var = 0;
        float num1 = 180f / frame;
        MTM_ASSERT(MTM_MATH_ABS(num1) <= 2147483648.0);
        body_work.turn_gen_factor = AKM_DEGtoA32(num1);
        gmBoss1BodySetDirection(body_work, (short)(body_work.orig_angle + body_work.turn_angle));
    }

    private static bool gmBoss1BodyUpdateTurnGently(GMS_BOSS1_BODY_WORK body_work)
    {
        bool flag = false;
        MTM_ASSERT(body_work.turn_gen_factor > 0);
        body_work.turn_gen_var += body_work.turn_gen_factor;
        if (body_work.turn_gen_var >= AKM_DEGtoA32(180))
        {
            body_work.turn_gen_var = AKM_DEGtoA32(180);
            flag = true;
        }
        float a = (float)(body_work.turn_amount * 0.5 * (1.0 - nnCos(body_work.turn_gen_var)));
        MTM_ASSERT(MTM_MATH_ABS(a) <= 2147483648.0);
        body_work.turn_angle = (int)a;
        if (flag)
            body_work.turn_angle = body_work.turn_amount;
        gmBoss1BodySetDirection(body_work, (short)(body_work.orig_angle + body_work.turn_angle));
        return flag;
    }

    private static void gmBoss1BodyInitPreANChainMotion(GMS_BOSS1_BODY_WORK body_work)
    {
        MTM_ASSERT(body_work.parts_objs[1]);
        body_work.parts_objs[1].obj_3d.frame[0] = 160f;
    }

    private static void gmBoss1BodyInitPreANMove(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.spd.x = 0;
        obsObjectWork.spd_add.x = -81;
    }

    private static bool gmBoss1BodyUpdatePreANMove(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        bool flag = false;
        if (MTM_MATH_ABS(obj_work.spd.x) >= 7372)
        {
            obj_work.spd.x = -7372;
            obj_work.spd_add.x = 0;
        }
        if (obj_work.pos.x <= GMM_BOSS1_AREA_LEFT() + 589824)
        {
            obj_work.pos.x = GMM_BOSS1_AREA_LEFT() + 589824;
            flag = true;
        }
        if (flag)
            GmBsCmnSetObjSpdZero(obj_work);
        return flag;
    }

    private static void gmBoss1BodySetANChainInitialBlendSpd(GMS_BOSS1_BODY_WORK body_work)
    {
        MTM_ASSERT(body_work.parts_objs[1]);
        body_work.parts_objs[1].obj_3d.blend_spd = 0.01f;
    }

    private static void gmBoss1BodyInitAtkNmlMove(GMS_BOSS1_BODY_WORK body_work, int frame)
    {
        body_work.move_time = frame;
        body_work.move_cnt = 0;
        gmBoss1BodyUpdateAtkNmlMove(body_work);
    }

    private static bool gmBoss1BodyUpdateAtkNmlMove(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num1 = GMM_BOSS1_AREA_LEFT() + 589824;
        int num2 = GMM_BOSS1_AREA_LEFT() + 983040;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            int num3 = num1;
            num1 = num2;
            num2 = num3;
        }
        bool flag;
        if (body_work.move_cnt < body_work.move_time)
        {
            int moveCurveAngleWidth = GMD_BOSS1_BODY_ATKNML_MOVE_CURVE_ANGLE_WIDTH;
            int moveCurveStartAngle = GMD_BOSS1_BODY_ATKNML_MOVE_CURVE_START_ANGLE;
            ++body_work.move_cnt;
            int num3 = (int)(moveCurveAngleWidth / (double)body_work.move_time);
            float num4 = nnCos(moveCurveStartAngle) - nnCos(moveCurveStartAngle + moveCurveAngleWidth);
            float num5 = (nnCos(moveCurveStartAngle) - nnCos(moveCurveStartAngle + num3 * body_work.move_cnt)) / num4;
            obsObjectWork.pos.x = num1 + (int)((num2 - num1) * (double)num5);
            flag = false;
        }
        else
        {
            obsObjectWork.pos.x = num2;
            flag = true;
        }
        return flag;
    }

    private static void gmBoss1BodySetFlipForAtkNmlMove(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num = GMM_BOSS1_AREA_CENTER_X();
        if (obsObjectWork.pos.x < num)
            obsObjectWork.disp_flag &= 4294967294U;
        else
            obsObjectWork.disp_flag |= 1U;
    }

    private static void gmBoss1BodyInitAtkNmlFlipAndTurn(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int frame = 40;
        MTM_ASSERT(frame < 72);
        gmBoss1BodySetFlipForAtkNmlMove(body_work);
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            gmBoss1BodyInitTurnGently(body_work, GMD_BOSS1_LEFTWARD_ANGLE, frame, true);
        else
            gmBoss1BodyInitTurnGently(body_work, GMD_BOSS1_RIGHTWARD_ANGLE, frame, false);
    }

    private static bool gmBoss1BodyUpdateAtkNmlFlipAndTurn(GMS_BOSS1_BODY_WORK body_work)
    {
        return gmBoss1BodyUpdateTurnGently(body_work);
    }

    private static void gmBoss1BodyInitAtkNmlDrift(GMS_BOSS1_BODY_WORK body_work, int frame)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        MTM_ASSERT(frame > 0);
        GmBsCmnSetObjSpdZero(obj_work);
        body_work.drift_angle = 0;
        body_work.drift_ang_spd = (int)nnRoundOff((float)(AKM_DEGtoA32(180f) / (double)frame + 0.5));
        body_work.drift_timer = frame;
        body_work.drift_pivot_x = ((int)obj_work.disp_flag & 1) == 0 ? GMM_BOSS1_AREA_LEFT() + 589824 : GMM_BOSS1_AREA_LEFT() + 983040;
        gmBoss1BodyUpdateAtkNmlDrift(body_work);
    }

    private static bool gmBoss1BodyUpdateAtkNmlDrift(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num;
        bool flag;
        if (body_work.drift_timer != 0)
        {
            --body_work.drift_timer;
            body_work.drift_angle = (int)(ushort.MaxValue & (long)(body_work.drift_angle + body_work.drift_ang_spd));
            num = FX_Mul(FX_Sin(body_work.drift_angle), 131072);
            flag = false;
        }
        else
        {
            num = 0;
            flag = true;
        }
        if (((int)obsObjectWork.disp_flag & 1) == 0)
            num = -num;
        obsObjectWork.pos.x = body_work.drift_pivot_x + num;
        return flag;
    }

    private static void gmBoss1BodyInitRush(GMS_BOSS1_BODY_WORK body_work, bool is_left)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (is_left)
        {
            body_work.bash_targ_pos.x = GMM_BOSS1_AREA_LEFT() + 720896;
            body_work.bash_targ_pos.y = GMD_BOSS1_BODY_ATKBASH_TARG_Y;
            body_work.bash_targ_pos.z = 0;
        }
        else
        {
            body_work.bash_targ_pos.x = GMM_BOSS1_AREA_LEFT() + 851968;
            body_work.bash_targ_pos.y = GMD_BOSS1_BODY_ATKBASH_TARG_Y;
            body_work.bash_targ_pos.z = 0;
        }
        int num1 = (body_work.bash_targ_pos.x - obsObjectWork.pos.x) / 39;
        int num2 = (body_work.bash_targ_pos.y - obsObjectWork.pos.y) / 39;
        obsObjectWork.spd_add.x = (int)(num1 * (1.0 / 32.0));
        obsObjectWork.spd_add.y = (int)(num2 * (1.0 / 32.0));
        obsObjectWork.spd.x = 0;
        obsObjectWork.spd.y = 0;
    }

    private static bool gmBoss1BodyUpdateRush(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
        amVectorSet(nnsVector1, FX_FX32_TO_F32(body_work.bash_targ_pos.x) - FX_FX32_TO_F32(obj_work.pos.x), FX_FX32_TO_F32(body_work.bash_targ_pos.y) - FX_FX32_TO_F32(obj_work.pos.y), 0.0f);
        amVectorSet(nnsVector2, FX_FX32_TO_F32(obj_work.spd.x), FX_FX32_TO_F32(obj_work.spd.y), 0.0f);
        if (0.0 >= nnDotProductVector(nnsVector2, nnsVector1))
        {
            GmBsCmnSetObjSpdZero(obj_work);
            VEC_Set(ref obj_work.pos, body_work.bash_targ_pos.x, body_work.bash_targ_pos.y, body_work.bash_targ_pos.z);
            GlobalPool<NNS_VECTOR>.Release(nnsVector1);
            GlobalPool<NNS_VECTOR>.Release(nnsVector2);
            return true;
        }
        GlobalPool<NNS_VECTOR>.Release(nnsVector1);
        GlobalPool<NNS_VECTOR>.Release(nnsVector2);
        return false;
    }

    private static void gmBoss1BodyInitBashReturn(GMS_BOSS1_BODY_WORK body_work, bool is_left)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (is_left)
        {
            body_work.bash_ret_pos.x = GMM_BOSS1_AREA_LEFT() + 524288;
            body_work.bash_ret_pos.y = body_work.atk_nml_alt;
            body_work.bash_ret_pos.z = 0;
        }
        else
        {
            body_work.bash_ret_pos.x = GMM_BOSS1_AREA_LEFT() + 1048576;
            body_work.bash_ret_pos.y = body_work.atk_nml_alt;
            body_work.bash_ret_pos.z = 0;
        }
        VEC_Set(ref body_work.bash_orig_pos, obsObjectWork.pos.x, obsObjectWork.pos.y, obsObjectWork.pos.z);
        body_work.bash_homing_deg = 0;
    }

    private static bool gmBoss1BodyUpdateBashReturn(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        body_work.bash_homing_deg += AKM_DEGtoA32(3);
        if (body_work.bash_homing_deg >= AKM_DEGtoA32(180))
        {
            body_work.bash_homing_deg = AKM_DEGtoA32(180);
            obsObjectWork.pos.x = body_work.bash_ret_pos.x;
            obsObjectWork.pos.y = body_work.bash_ret_pos.y;
            return true;
        }
        obsObjectWork.pos.x = body_work.bash_orig_pos.x + FX_Mul(body_work.bash_ret_pos.x - body_work.bash_orig_pos.x, 4096 - mtMathCos(body_work.bash_homing_deg) >> 1);
        obsObjectWork.pos.y = body_work.bash_orig_pos.y + FX_Mul(body_work.bash_ret_pos.y - body_work.bash_orig_pos.y, 4096 - mtMathCos(body_work.bash_homing_deg) >> 1);
        return false;
    }

    private static void gmBoss1BodyInitEscapeMove(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.spd_add.x = ((int)obj_work.disp_flag & 1) == 0 ? 409 : -409;
        obj_work.spd_add.y = -256;
    }

    private static bool gmBoss1BodyUpdateEscapeMove(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        bool flag = false;
        if (MTM_MATH_ABS(obj_work.spd.x) >= 4915)
        {
            obj_work.spd.x = 4915;
            obj_work.spd.y = -3072;
            obj_work.spd_add.x = 0;
            obj_work.spd_add.y = 0;
        }
        if (obj_work.pos.y < 0)
            flag = true;
        else if (obj_work.pos.x >= (g_gm_main_system.map_size[0] << 12) + 262144)
            flag = true;
        if (flag)
            GmBsCmnSetObjSpdZero(obj_work);
        return flag;
    }

    private static void gmBoss1BodyInitDefeatState(GMS_BOSS1_BODY_WORK body_work)
    {
        bool flag = false;
        if (((int)body_work.flag & 1) != 0)
            flag = true;
        gmBoss1BodyChangeState(body_work, 7, true);
        if (flag)
            body_work.flag |= 1U;
        else
            body_work.flag &= 4294967294U;
    }

    private static void gmBoss1BodyUpdateChainTopDirection(GMS_BOSS1_BODY_WORK body_work)
    {
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        if (((int)body_work.flag & 1) == 0)
        {
            NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(body_work.snm_work, body_work.chaintop_snm_reg_id);
            nnRotateYMatrix(nnsMatrix, snmMtx, -GMM_BS_OBJ(body_work).dir.y + AKM_DEGtoA16(90));
            GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix, body_work.chaintop_cnm_reg_id);
            GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, body_work.chaintop_cnm_reg_id, 1);
        }
        else
            GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, body_work.chaintop_cnm_reg_id, 0);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss1BodyDamageDefFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        GMS_BOSS1_BODY_WORK body_work = (GMS_BOSS1_BODY_WORK)parentObj1;
        if (parentObj2 == null || 1 != parentObj2.obj_type)
            return;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)parentObj2;
        GmPlySeqAtkReactionInit(ply_work);
        GmPlySeqSetJumpState(ply_work, 0, 5U);
        if (ply_work.seq_state == GME_PLY_SEQ_STATE_HOMING_REF)
        {
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ply_work.obj_work.move.x < 0 ? 20480 : -20480;
            ply_work.obj_work.spd.y = parentObj2.pos.y > parentObj1.pos.y ? 16384 : -16384;
            GmPlySeqSetNoJumpMoveTime(ply_work, 102400);
        }
        else
        {
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ply_work.obj_work.move.x < 0 ? 16384 : -16384;
            ply_work.obj_work.spd.y = parentObj2.pos.y > parentObj1.pos.y ? 12288 : -12288;
            GmPlySeqSetNoJumpMoveTime(ply_work, 102400);
        }
        gmBoss1BodySetNoHitTime(body_work);
        GmSoundPlaySE("Boss0_01");
        GMM_PAD_VIB_SMALL_TIME(30f);
        gmBoss1EffDamageInit(body_work);
        if (((int)body_work.flag & 4) != 0)
            return;
        gmBoss1BodyExecDamageRoutine(body_work);
    }

    private static void gmBoss1BodyOutFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_BODY_WORK gmsBosS1BodyWork = (GMS_BOSS1_BODY_WORK)obj_work;
        GmBsCmnUpdateCNMParam(obj_work, gmsBosS1BodyWork.cnm_mgr_work);
        ObjDrawActionSummary(obj_work);
    }

    private static void gmBoss1BodyChangeState(GMS_BOSS1_BODY_WORK body_work, int state)
    {
        gmBoss1BodyChangeState(body_work, state, false);
    }

    private static void gmBoss1BodyChangeState(
      GMS_BOSS1_BODY_WORK body_work,
      int state,
      bool is_wrapped)
    {
        UNREFERENCED_PARAMETER(is_wrapped);
        GMF_BOSS1_BODY_STATE_LEAVE_FUNC bodyStateLeaveFunc = gm_boss1_body_state_leave_func_tbl[body_work.state];
        if (bodyStateLeaveFunc != null)
            bodyStateLeaveFunc(body_work);
        body_work.prev_state = body_work.state;
        body_work.state = state;
        GMS_BOSS1_BODY_STATE_ENTER_INFO bodyStateEnterInfo = gm_boss1_body_state_enter_info_tbl[body_work.state];
        if (bodyStateEnterInfo.enter_func == null)
            return;
        bodyStateEnterInfo.enter_func(body_work);
    }

    private static void gmBoss1BodyWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_BODY_WORK body_work = (GMS_BOSS1_BODY_WORK)obj_work;
        if (((int)body_work.mgr_work.flag & 1) == 0)
            return;
        GmBsCmnInitBossMotionCBSystem(obj_work, body_work.bmcb_mgr);
        GmBsCmnCreateSNMWork(body_work.snm_work, obj_work.obj_3d._object, 4);
        GmBsCmnAppendBossMotionCallback(body_work.bmcb_mgr, body_work.snm_work.bmcb_link);
        body_work.chain_snm_reg_id = GmBsCmnRegisterSNMNode(body_work.snm_work, 13);
        body_work.egg_snm_reg_id = GmBsCmnRegisterSNMNode(body_work.snm_work, 11);
        body_work.body_snm_reg_id = GmBsCmnRegisterSNMNode(body_work.snm_work, 2);
        body_work.chaintop_snm_reg_id = GmBsCmnRegisterSNMNode(body_work.snm_work, 9);
        GmBsCmnCreateCNMMgrWork(body_work.cnm_mgr_work, obj_work.obj_3d._object, 1);
        GmBsCmnInitCNMCb(obj_work, body_work.cnm_mgr_work);
        body_work.chaintop_cnm_reg_id = GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, 9);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1BodyMain);
        gmBoss1BodyChangeState(body_work, 1);
    }

    private static void gmBoss1BodyMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_BODY_WORK gmsBosS1BodyWork = (GMS_BOSS1_BODY_WORK)obj_work;
        gmBoss1BodyUpdateNoHitTime(gmsBosS1BodyWork);
        if (((int)gmsBosS1BodyWork.flag & int.MinValue) != 0)
        {
            gmsBosS1BodyWork.flag &= 1073741823U;
            gmBoss1BodyInitDefeatState(gmsBosS1BodyWork);
        }
        else if (gmsBosS1BodyWork.proc_update != null)
            gmsBosS1BodyWork.proc_update(gmsBosS1BodyWork);
        gmBoss1BodyUpdateSuspendAction(gmsBosS1BodyWork);
        gmBoss1EffAfterburnerUpdateCreate(gmsBosS1BodyWork);
        if (((int)gmsBosS1BodyWork.flag & 1073741824) != 0)
        {
            gmsBosS1BodyWork.flag &= 3221225471U;
            gmsBosS1BodyWork.flag |= 536870912U;
            GmBsCmnInitObject3DNNDamageFlicker(obj_work, gmsBosS1BodyWork.flk_work, 32f);
        }
        GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, gmsBosS1BodyWork.flk_work);
        gmBoss1BodyUpdateDirection(gmsBosS1BodyWork);
        gmBoss1BodyUpdateChainTopDirection(gmsBosS1BodyWork);
    }

    private static void gmBoss1BodyStateEnterStart(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.flag |= 2U;
        body_work.flag |= 64U;
        gmBoss1BodySetActionWhole(body_work, 0, true);
        body_work.flag |= 32U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        GmBsCmnSetObjSpdZero(obj_work);
        gmBoss1BodySetDirectionNormal(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateStartWithWaitLockBegin);
    }

    private static void gmBoss1BodyStateLeaveStart(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obsObjectWork;
        gmBoss1EffAfterburnerSetEnable(body_work, false);
        gmsEnemy3DWork.ene_com.enemy_flag &= 4294934527U;
        body_work.flag &= 4294967231U;
        obsObjectWork.flag &= 4294967293U;
        body_work.flag &= 4294967263U;
    }

    private static void gmBoss1BodyStateUpdateStartWithWaitLockBegin(
      GMS_BOSS1_BODY_WORK body_work)
    {
        if (!gmBoss1IsScrollLockBusy())
            return;
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateStartWithWaitLockComplete);
    }

    private static void gmBoss1BodyStateUpdateStartWithWaitLockComplete(
      GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (gmBoss1IsScrollLockBusy())
            return;
        GmMapSetMapDrawSize(3);
        GmBsCmnSetObjSpd(obj_work, 0, 4096, 0);
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateStartWithFall);
    }

    private static void gmBoss1BodyStateUpdateStartWithFall(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (obj_work.pos.y < body_work.atk_nml_alt)
            return;
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.pos.y = body_work.atk_nml_alt;
        GmBsCmnSetObjSpd(obj_work, -4096, 0, 0);
        gmBoss1EffAfterburnerSetEnable(body_work, true);
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateStartWithMove);
    }

    private static void gmBoss1BodyStateUpdateStartWithMove(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        int num = GMM_BOSS1_AREA_LEFT() + 786432;
        if (obj_work.pos.x > num)
            return;
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.pos.x = num;
        body_work.wait_timer = 10U;
        gmBoss1EffAfterburnerSetEnable(body_work, false);
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateStartWithWaitEnd);
    }

    private static void gmBoss1BodyStateUpdateStartWithWaitEnd(GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else
            gmBoss1BodyChangeState(body_work, 2);
    }

    private static void gmBoss1BodyStateEnterPrep(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.flag |= 2U;
        body_work.flag |= 64U;
        gmBoss1BodySetActionWhole(body_work, 2);
        body_work.flag &= 4294967263U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        GmBsCmnSetObjSpdZero(obj_work);
        body_work.wait_timer = 95U;
        GmSoundPlaySE("Boss1_01");
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdatePrepWithWait);
    }

    private static void gmBoss1BodyStateLeavePrep(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        ((GMS_ENEMY_3D_WORK)obsObjectWork).ene_com.enemy_flag &= 4294934527U;
        body_work.flag &= 4294967231U;
        obsObjectWork.flag &= 4294967293U;
    }

    private static void gmBoss1BodyStateUpdatePrepWithWait(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else
            body_work.flag &= 4294967231U;
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        gmBoss1BodyChangeState(body_work, 3);
    }

    private static void gmBoss1BodyStateEnterPreAtkNml(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss1BodySetDmgRectSizeForAtkNml(body_work);
        obsObjectWork.flag &= 4294967293U;
        gmBoss1BodySetActionWhole(body_work, 3);
        gmBoss1BodyInitPreANChainMotion(body_work);
        gmBoss1BodySetFlipForAtkNmlMove(body_work);
        gmBoss1BodyInitPreANMove(body_work);
        gmBoss1EffAfterburnerSetEnable(body_work, true);
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdatePreAtkNmlWithMove);
    }

    private static void gmBoss1BodyStateLeavePreAtkNml(GMS_BOSS1_BODY_WORK body_work)
    {
        gmBoss1EffAfterburnerSetEnable(body_work, false);
        gmBoss1BodySetDmgRectSizeToDefault(body_work);
    }

    private static void gmBoss1BodyStateUpdatePreAtkNmlWithMove(GMS_BOSS1_BODY_WORK body_work)
    {
        gmBoss1BodySetDirectionNormal(body_work);
        if (!gmBoss1BodyUpdatePreANMove(body_work))
            return;
        gmBoss1BodyChangeState(body_work, 4);
        gmBoss1BodySetANChainInitialBlendSpd(body_work);
    }

    private static void gmBoss1BodyStateEnterAtkNml(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        bool force_change = false;
        gmBoss1BodySetDmgRectSizeForAtkNml(body_work);
        obsObjectWork.flag &= 4294967293U;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            force_change = true;
        gmBoss1BodySetActionWhole(body_work, 4, force_change);
        gmBoss1BodyInitAtkNmlFlipAndTurn(body_work);
        gmBoss1BodyInitAtkNmlDrift(body_work, 72);
        gmBoss1EffAfterburnerSetEnable(body_work, false);
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateAtkNmlWithTurn);
    }

    private static void gmBoss1BodyStateLeaveAtkNml(GMS_BOSS1_BODY_WORK body_work)
    {
        gmBoss1EffAfterburnerSetEnable(body_work, false);
        gmBoss1BodySetDmgRectSizeToDefault(body_work);
    }

    private static void gmBoss1BodyStateUpdateAtkNmlWithTurn(GMS_BOSS1_BODY_WORK body_work)
    {
        bool flag = gmBoss1BodyUpdateAtkNmlDrift(body_work);
        if (!gmBoss1BodyUpdateAtkNmlFlipAndTurn(body_work) || !flag)
            return;
        gmBoss1BodySetFlipForAtkNmlMove(body_work);
        gmBoss1BodyInitAtkNmlMove(body_work, 56);
        gmBoss1EffAfterburnerSetEnable(body_work, true);
        GmSoundPlaySE("Boss1_02");
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateAtkNmlWithMove);
    }

    private static void gmBoss1BodyStateUpdateAtkNmlWithMove(GMS_BOSS1_BODY_WORK body_work)
    {
        gmBoss1BodySetDirectionNormal(body_work);
        if (gmBoss1BodyIsExtraAttack(body_work))
        {
            if (GmBsCmnIsFinalZoneType(GMM_BS_OBJ(body_work)) == 0)
                GmSoundChangeAngryBossBGM();
            gmBoss1BodyChangeState(body_work, 5);
        }
        else
        {
            if (!gmBoss1BodyUpdateAtkNmlMove(body_work))
                return;
            gmBoss1BodyChangeState(body_work, 4);
        }
    }

    private static void gmBoss1BodyStateEnterAtkBash(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        gmBoss1BodySetActionWhole(body_work, 5);
        GmBsCmnSetObjSpdZero(obj_work);
        gmBoss1BodySetDirectionNormal(body_work);
        gmBoss1BodySetDmgRectSizeForAtkNml(body_work);
        if (GmBsCmnGetPlayerObj().pos.x < obj_work.pos.x)
        {
            obj_work.disp_flag |= 1U;
            gmBoss1BodyInitTurnGently(body_work, AKM_DEGtoA16(270f), 30, false);
        }
        else
        {
            obj_work.disp_flag &= 4294967294U;
            gmBoss1BodyInitTurnGently(body_work, AKM_DEGtoA16(90f), 30, true);
        }
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateAtkBashWithLock);
    }

    private static void gmBoss1BodyStateLeaveAtkBash(GMS_BOSS1_BODY_WORK body_work)
    {
        body_work.flag &= 4294967294U;
        body_work.flag &= 4294967291U;
        gmBoss1BodySetAtkRectToNormal(body_work);
        gmBoss1BodySetDmgRectSizeToDefault(body_work);
    }

    private static void gmBoss1BodyStateUpdateAtkBashWithLock(GMS_BOSS1_BODY_WORK body_work)
    {
        if (!gmBoss1BodyUpdateTurnGently(body_work))
            return;
        gmBoss1BodySetActionWhole(body_work, 6);
        gmBoss1Init1ShotTimer(body_work.se_timer, 110U);
        body_work.se_cnt = 3U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateAtkBashWithPrep);
    }

    private static void gmBoss1BodyStateUpdateAtkBashWithPrep(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (gmBoss1Update1ShotTimer(body_work.se_timer) && body_work.se_cnt != 0U)
        {
            --body_work.se_cnt;
            GmSoundPlaySE("Boss1_03");
            gmBoss1Init1ShotTimer(body_work.se_timer, 15U);
        }
        if (gmBoss1BodyCheckChainMotionMergeEnd(body_work))
            body_work.flag |= 1U;
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        gmBoss1BodySetDmgRectSizeToDefault(body_work);
        gmBoss1BodySetAtkRectToWeakAttacker(body_work);
        body_work.flag |= 4U;
        body_work.flag |= 1U;
        gmBoss1BodySetActionWhole(body_work, 7);
        if (((int)obj_work.disp_flag & 1) != 0)
            gmBoss1BodyInitRush(body_work, true);
        else
            gmBoss1BodyInitRush(body_work, false);
        GmSoundPlaySE("Boss1_04");
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateAtkBashWithSwing);
    }

    private static void gmBoss1BodyStateUpdateAtkBashWithSwing(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (!gmBoss1BodyUpdateRush(body_work) || GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        gmBoss1BodySetDmgRectSizeToDefault(body_work);
        gmBoss1BodySetAtkRectToWeakAttacker(body_work);
        body_work.flag &= 4294967291U;
        body_work.flag |= 134217728U;
        GmCameraVibrationSet(0, 65536, 0);
        GmSoundPlaySE("Boss1_05");
        GMM_PAD_VIB_MID_TIME(30f);
        gmBoss1BodySetActionWhole(body_work, 8);
        gmBoss1BodySetSuspendAction(body_work, 1, 1U);
        gmBoss1Init1ShotTimer(body_work.se_timer, 70U);
        body_work.se_cnt = 2U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateAtkBashWithFinish);
    }

    private static void gmBoss1BodyStateUpdateAtkBashWithFinish(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (gmBoss1Update1ShotTimer(body_work.se_timer) && body_work.se_cnt != 0U)
        {
            --body_work.se_cnt;
            GmSoundPlaySE("Boss1_03");
            gmBoss1Init1ShotTimer(body_work.se_timer, 15U);
        }
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        gmBoss1BodySetAtkRectToNormal(body_work);
        gmBoss1BodySetActionWhole(body_work, 9);
        if (((int)obj_work.disp_flag & 1) != 0)
        {
            gmBoss1BodyInitBashReturn(body_work, true);
            obj_work.disp_flag &= 4294967294U;
            gmBoss1BodyInitTurnGently(body_work, AKM_DEGtoA16(90), 90, false);
        }
        else
        {
            gmBoss1BodyInitBashReturn(body_work, false);
            obj_work.disp_flag |= 1U;
            gmBoss1BodyInitTurnGently(body_work, AKM_DEGtoA16(270), 90, true);
        }
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateAtkBashWithHoming);
    }

    private static void gmBoss1BodyStateUpdateAtkBashWithHoming(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        bool flag = true;
        if (!gmBoss1BodyUpdateTurnGently(body_work))
            flag = false;
        if (!gmBoss1BodyUpdateBashReturn(body_work))
            flag = false;
        if (!flag)
            return;
        gmBoss1BodySetDmgRectSizeToDefault(body_work);
        gmBoss1BodySetAtkRectToWeakAttacker(body_work);
        body_work.flag |= 4U;
        body_work.flag |= 1U;
        gmBoss1BodySetActionWhole(body_work, 7);
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            gmBoss1BodyInitRush(body_work, true);
        else
            gmBoss1BodyInitRush(body_work, false);
        GmSoundPlaySE("Boss1_04");
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateAtkBashWithSwing);
    }

    private static void gmBoss1BodyStateEnterDmgNml(GMS_BOSS1_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss1BodyStateLeaveDmgNml(GMS_BOSS1_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss1BodyStateEnterDefeat(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        obj_work.flag |= 2U;
        obj_work.disp_flag |= 16U;
        body_work.flag |= 8U;
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
        GmBsCmnSetObjSpdZero(obj_work);
        body_work.wait_timer = 40U;
        GmSoundChangeWinBossBGM();
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateDefeatWithWaitStart);
    }

    private static void gmBoss1BodyStateLeaveDefeat(GMS_BOSS1_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss1BodyStateUpdateDefeatWithWaitStart(
      GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            gmBoss1EffBombInitCreate(body_work.bomb_work, 0, GMM_BS_OBJ(body_work), GMM_BS_OBJ(body_work).pos.x, GMM_BS_OBJ(body_work).pos.y, 327680, 327680, 10U, 10U);
            body_work.wait_timer = 120U;
            body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateDefeatWithExplode);
        }
    }

    private static void gmBoss1BodyStateUpdateDefeatWithExplode(GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
            gmBoss1EffBombUpdateCreate(body_work.bomb_work);
        }
        else
        {
            body_work.flag |= 67108864U;
            GmSoundPlaySE("Boss0_03");
            gmBoss1InitFlashScreen();
            GMM_PAD_VIB_MID_TIME(120f);
            OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(GMM_BS_OBJ(body_work), 8);
            obsObjectWork.pos.z = obsObjectWork.parent_obj.pos.z + 131072;
            body_work.wait_timer = 40U;
            GmPlayerAddScoreNoDisp((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), 1000);
            body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateDefeatWithScatter);
        }
    }

    private static void gmBoss1BodyStateUpdateDefeatWithScatter(GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            gmBoss1SetPartTextureBurnt(GMM_BS_OBJ(body_work));
            body_work.flag |= 16777216U;
            gmBoss1EffABSmokeInit(body_work);
            gmBoss1EffBodySmokeInit(body_work);
            gmBoss1EffBodySmallSmokeInit(body_work);
            body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateDefeatWithWaitEnd);
        }
    }

    private static void gmBoss1BodyStateUpdateDefeatWithWaitEnd(GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
            --body_work.wait_timer;
        else
            gmBoss1BodyChangeState(body_work, 8);
    }

    private static void gmBoss1BodyStateEnterEscape(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.flag |= 2U;
        obsObjectWork.disp_flag &= 4294967279U;
        gmBoss1BodySetActionWhole(body_work, 12);
        body_work.flag |= 8388608U;
        bool is_positive = gmBoss1BodyIsDirectionPositiveFromCurrent(body_work, GMD_BOSS1_RIGHTWARD_ANGLE);
        gmBoss1BodyInitTurnGently(body_work, GMD_BOSS1_RIGHTWARD_ANGLE, 90, is_positive);
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateEscapeWithTurn);
    }

    private static void gmBoss1BodyStateLeaveEscape(GMS_BOSS1_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss1BodyStateUpdateEscapeWithTurn(GMS_BOSS1_BODY_WORK body_work)
    {
        if (!gmBoss1BodyUpdateTurnGently(body_work))
            return;
        gmBoss1BodyInitEscapeMove(body_work);
        if (GmBsCmnIsFinalZoneType(GMM_BS_OBJ(GMM_BOSS1_MGR(body_work))) != 0)
            body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateEscapeWithMoveMoveFinalZone);
        else
            body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateEscapeWithMoveLocked);
    }

    private static void gmBoss1BodyStateUpdateEscapeWithMoveLocked(
      GMS_BOSS1_BODY_WORK body_work)
    {
        gmBoss1BodyUpdateEscapeMove(body_work);
        if (!gmBoss1BodyIsEscapeScrUnlock(body_work))
            return;
        GmMapSetMapDrawSize(1);
        gmBoss1EffBodyDebrisInit(body_work);
        body_work.flag |= 256U;
        GmGmkCamScrLimitRelease(4);
        body_work.proc_update = new MPP_VOID_GMS_BOSS1_BODY_WORK(gmBoss1BodyStateUpdateEscapeWithMoveUnlocked);
    }

    private static void gmBoss1BodyStateUpdateEscapeWithMoveUnlocked(
      GMS_BOSS1_BODY_WORK body_work)
    {
        if (!gmBoss1BodyUpdateEscapeMove(body_work))
            return;
        GMM_BOSS1_MGR(body_work).flag |= 2U;
        body_work.proc_update = null;
    }

    private static void gmBoss1BodyStateUpdateEscapeWithMoveMoveFinalZone(
      GMS_BOSS1_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 256) == 0 && gmBoss1BodyIsEscapeScrUnlock(body_work))
        {
            gmBoss1EffBodyDebrisInit(body_work);
            body_work.flag |= 256U;
        }
        if (!gmBoss1BodyUpdateEscapeMove(body_work) && !gmBoss1BodyIsEscapeOutFinalZone(body_work))
            return;
        GMM_BOSS1_MGR(body_work).flag |= 2U;
        body_work.proc_update = null;
    }

}