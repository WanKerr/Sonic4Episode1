public partial class AppMain
{
    public static float GMD_BOSS4_BODY_START_POS_Y => GMM_BOSS4_STAGE(-120f, 1480f);

    public static float GMD_BOSS4_BODY_END_POS_Y => GMM_BOSS4_STAGE(280f, 1880f);

    public static int GMD_BOSS4_SPEED_TIMES_IN_DAMAGE => GMM_BOSS4_PAL_TIME(5f);

    public static float GMD_BOSS4_BODY_PRE_ATKNML_SPD_ADD => GMM_BOSS4_PAL_SPEED(0.02f);

    public static int GMD_BOSS4_BODY_PRE_ATKNML_SPD_MAX_ABS => GMM_BOSS4_PAL_TIME(1f);

    public static int GMD_BOSS4_BODY_ATKNML_MOVE_FRAME => GMM_BOSS4_PAL_TIME(720f);

    public static int GMD_BOSS4_BODY_ATKNML_DRIFT_FRAME => GMM_BOSS4_PAL_TIME(30f);

    public static int GMD_BOSS4_BODY_ATKNML_TURN_FRAME => GMM_BOSS4_PAL_TIME(28f);

    public static int GMD_BOSS4_BODY_2ND_POS_X => GMM_BOSS4_STAGE(3500, 12100);

    public static int GMD_BOSS4_BODY_2ND_POS_Y => GMM_BOSS4_STAGE(250, 1850);

    public static uint GMD_BOSS4_BODY_SONIC_CTRL_TIME => (uint)GMM_BOSS4_PAL_TIME(240f);

    public static uint GMD_BOSS4_BODY_CREATE_CAP_FIRST_TIME => (uint)GMM_BOSS4_PAL_TIME(120f);

    public static float GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_1 => GMM_BOSS4_PAL_SPEED(-1f);

    public static float GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_1 => GMM_BOSS4_PAL_SPEED(-2f);

    public static float GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_2 => GMM_BOSS4_PAL_SPEED(-2f);

    public static float GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_2 => GMM_BOSS4_PAL_SPEED(-2f);

    public static uint GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_3 => (uint)GMM_BOSS4_PAL_TIME(300f);

    public static uint GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_2 => (uint)GMM_BOSS4_PAL_TIME(180f);

    public static uint GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_1 => (uint)GMM_BOSS4_PAL_TIME(120f);

    public static uint GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_2_2 => (uint)GMM_BOSS4_PAL_TIME(90f);

    public static uint GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_TIME => (uint)GMM_BOSS4_PAL_TIME(120f);

    public static uint GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_INTERVAL_MIN_TIME => (uint)GMM_BOSS4_PAL_TIME(10f);

    public static uint GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_INTERVAL_MAX_TIME => (uint)GMM_BOSS4_PAL_TIME(30f);

    public static uint GMD_BOSS4_BODY_DEFEAT_BOMB_PARTS_INTERVAL_MIN_TIME => (uint)GMM_BOSS4_PAL_TIME(10f);

    public static uint GMD_BOSS4_BODY_DEFEAT_BOMB_PARTS_INTERVAL_MAX_TIME => (uint)GMM_BOSS4_PAL_TIME(30f);

    public static GMS_BOSS4_MGR_WORK GMM_BOSS4_MGR(GMS_BOSS4_BODY_WORK work)
    {
        return work.mgr_work;
    }

    private static void GmBoss4BodyBuild()
    {
        ObjDataLoadAmbIndex(ObjDataGet(730), 2, GMD_BOSS4_ARC);
    }

    private static void GmBoss4BodyFlush()
    {
        ObjDataRelease(ObjDataGet(730));
    }

    private static OBS_OBJECT_WORK GmBoss4BodyInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS4_BODY_WORK(), "BOSS4_BODY");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS4_BODY_WORK body_work = (GMS_BOSS4_BODY_WORK)work;
        work.pos.y = FX_F32_TO_FX32(GMD_BOSS4_BODY_START_POS_Y);
        work.pos.z = -131072;
        body_work.atk_nml_alt = FX_F32_TO_FX32(GMD_BOSS4_BODY_END_POS_Y);
        work.flag |= 16U;
        work.disp_flag |= 4194309U;
        work.move_flag |= 4096U;
        work.move_flag &= 4294967167U;
        gmsEnemy3DWork.ene_com.vit = 1;
        ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[0], -40, -16, 40, 2);
        gmsEnemy3DWork.ene_com.rect_work[0].ppDef = new OBS_RECT_WORK_Delegate1(gmBoss4BodyDamageDefFunc);
        ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[1], -32, -8, 32, 40);
        gmsEnemy3DWork.ene_com.rect_work[1].ppHit = new OBS_RECT_WORK_Delegate1(gmBoss4BodyAtkHitFunc);
        ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[2], -30, -18, 30, 40);
        gmsEnemy3DWork.ene_com.rect_work[2].ppHit = new OBS_RECT_WORK_Delegate1(gmBoss4BodyDefHitFunc);
        ObjRectGroupSet(gmsEnemy3DWork.ene_com.rect_work[2], 1, 1);
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294965247U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag |= 4U;
        ObjObjectCopyAction3dNNModel(work, GmBoss4GetObj3D(0), gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(730), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.disp_flag |= 134217728U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4BodyWaitLoad);
        gmBoss4BodyChangeState(body_work, 0);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4BodyOutFunc);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss4BodyExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss4BodyExit(MTS_TASK_TCB tcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        GMS_BOSS4_BODY_WORK gmsBosS4BodyWork = (GMS_BOSS4_BODY_WORK)tcbWork;
        GmBoss4DecObjCreateCount();
        GmBoss4UtilExitNodeMatrix(gmsBosS4BodyWork.node_work);
        GmBsCmnClearCNMCb(tcbWork);
        GmBsCmnDeleteCNMMgrWork(gmsBosS4BodyWork.cnm_mgr_work);
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss4BodySetActionWhole(GMS_BOSS4_BODY_WORK body_work, int act_id)
    {
        gmBoss4BodySetActionWhole(body_work, act_id, false);
    }

    private static void gmBoss4BodySetActionWhole(
      GMS_BOSS4_BODY_WORK body_work,
      int act_id,
      bool force_change)
    {
        GMS_BOSS4_PART_ACT_INFO[] bosS4PartActInfoArray = gm_boss4_act_id_tbl[act_id];
        if (!force_change && body_work.whole_act_id == act_id)
            return;
        body_work.whole_act_id = act_id;
        for (int index = 0; index < 2; ++index)
        {
            if (body_work.parts_objs[index] != null)
            {
                if (index == 1)
                {
                    GMS_BOSS4_EGG_WORK partsObj = (GMS_BOSS4_EGG_WORK)body_work.parts_objs[index];
                    body_work.egg_revert_mtn_id = act_id;
                    if (GmBoss4GetActInfo(body_work.egg_revert_mtn_id, 1).act_id == 4)
                        body_work.egg_revert_mtn_id = !GmBoss4Is2ndStage() ? 3 : 5;
                    if (((int)partsObj.flag & 1) != 0)
                        continue;
                }
                if (bosS4PartActInfoArray[index].is_maintain == 0)
                    GmBsCmnSetAction(body_work.parts_objs[index], bosS4PartActInfoArray[index].act_id, bosS4PartActInfoArray[index].is_repeat, bosS4PartActInfoArray[index].is_blend);
                else if (bosS4PartActInfoArray[index].is_repeat != 0)
                    GMM_BS_OBJ(body_work).disp_flag |= 4U;
                body_work.parts_objs[index].obj_3d.speed[0] = bosS4PartActInfoArray[index].mtn_spd;
                body_work.parts_objs[index].obj_3d.blend_spd = bosS4PartActInfoArray[index].blend_spd;
            }
        }
    }

    private static void gmBoss4BodyUpdateSuspendAction(GMS_BOSS4_BODY_WORK body_work)
    {
    }

    private static void gmBoss4BodyExecDamageRoutine(GMS_BOSS4_BODY_WORK body_work)
    {
        GMS_BOSS4_MGR_WORK mgrWork = body_work.mgr_work;
        MTM_ASSERT(mgrWork);
        if (body_work.damage_timer > 0)
            return;
        if (mgrWork.life != 0)
            --mgrWork.life;
        if (0 < mgrWork.life)
        {
            body_work.flag[0] |= 1073741824U;
            if (1 >= mgrWork.life)
                GmDecoStartLoop();
        }
        else
            body_work.flag[0] |= 2147483648U;
        body_work.damage_timer = 60;
        GmBoss4CapsuleSetInvincible(30);
        gmBoss4BodySetActionWhole(body_work, 6, true);
    }

    private static bool gmBoss4BodyIsExtraAttack(GMS_BOSS4_BODY_WORK body_work)
    {
        return GMM_BOSS4_MGR(body_work).life <= GMD_BOSS4_EXTRA_ATK_THRESHOLD_LIFE;
    }

    private static void gmBoss4BodyInitPreANChainMotion(GMS_BOSS4_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss4BodyInitPreANMove(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.spd.x = 0;
        obsObjectWork.spd_add.x = -FX_F32_TO_FX32(GMD_BOSS4_BODY_PRE_ATKNML_SPD_ADD);
    }

    private static bool gmBoss4BodyUpdatePreANMoveLeft(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        bool flag = false;
        if (MTM_MATH_ABS(obj_work.spd.x) >= FX_F32_TO_FX32(GMD_BOSS4_BODY_PRE_ATKNML_SPD_MAX_ABS))
        {
            obj_work.spd.x = -FX_F32_TO_FX32(GMD_BOSS4_BODY_PRE_ATKNML_SPD_MAX_ABS);
            obj_work.spd_add.x = 0;
        }
        if (obj_work.pos.x <= GMM_BOSS4_AREA_LEFT() + FX_F32_TO_FX32(74f))
        {
            obj_work.pos.x = GMM_BOSS4_AREA_LEFT() + FX_F32_TO_FX32(74f);
            flag = true;
        }
        if (flag)
            GmBsCmnSetObjSpdZero(obj_work);
        return flag;
    }

    private static bool gmBoss4BodyUpdatePreANMoveRight(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        bool flag = false;
        if (MTM_MATH_ABS(obj_work.spd.x) >= FX_F32_TO_FX32(GMD_BOSS4_BODY_PRE_ATKNML_SPD_MAX_ABS))
        {
            obj_work.spd.x = FX_F32_TO_FX32(GMD_BOSS4_BODY_PRE_ATKNML_SPD_MAX_ABS);
            obj_work.spd_add.x = 0;
        }
        if (obj_work.pos.x >= GMM_BOSS4_AREA_LEFT() + FX_F32_TO_FX32(310f))
        {
            obj_work.pos.x = GMM_BOSS4_AREA_LEFT() + FX_F32_TO_FX32(310f);
            flag = true;
        }
        if (flag)
            GmBsCmnSetObjSpdZero(obj_work);
        return flag;
    }

    private static void gmBoss4BodySetANChainInitialBlendSpd(GMS_BOSS4_BODY_WORK body_work)
    {
    }

    private static void gmBoss4BodyInitAtkNmlMove(GMS_BOSS4_BODY_WORK body_work, int frame)
    {
        body_work.move_time = frame;
        body_work.move_cnt = 0;
        gmBoss4BodyUpdateAtkNmlMove(body_work);
    }

    private static bool gmBoss4BodyUpdateAtkNmlMove(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num1 = GMM_BOSS4_AREA_LEFT() + FX_F32_TO_FX32(74f);
        int num2 = GMM_BOSS4_AREA_LEFT() + FX_F32_TO_FX32(310f);
        if (body_work.dir.direction == 1)
        {
            int num3 = num1;
            num1 = num2;
            num2 = num3;
        }
        bool flag;
        if (body_work.move_cnt < body_work.move_time)
        {
            int moveCurveAngleWidth = GMD_BOSS4_BODY_ATKNML_MOVE_CURVE_ANGLE_WIDTH;
            int moveCurveStartAngle = GMD_BOSS4_BODY_ATKNML_MOVE_CURVE_START_ANGLE;
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

    private static void gmBoss4BodySetFlipForAtkNmlMove(GMS_BOSS4_BODY_WORK body_work)
    {
        if (GMM_BS_OBJ(body_work).pos.x < GMM_BOSS4_AREA_CENTER_X())
            body_work.dir.direction = 0;
        else
            body_work.dir.direction = 1;
    }

    private static void gmBoss4BodyInitAtkNmlFlipAndTurn(GMS_BOSS4_BODY_WORK body_work)
    {
        int bodyAtknmlTurnFrame = GMD_BOSS4_BODY_ATKNML_TURN_FRAME;
        MTM_ASSERT(bodyAtknmlTurnFrame < GMD_BOSS4_BODY_ATKNML_DRIFT_FRAME);
        gmBoss4BodySetFlipForAtkNmlMove(body_work);
        if (body_work.dir.direction == 1)
            GmBoss4UtilInitTurnGently(body_work.dir, GMD_BOSS4_LEFTWARD_ANGLE, bodyAtknmlTurnFrame, true);
        else
            GmBoss4UtilInitTurnGently(body_work.dir, GMD_BOSS4_RIGHTWARD_ANGLE, bodyAtknmlTurnFrame, false);
    }

    private static bool gmBoss4BodyUpdateAtkNmlFlipAndTurn(GMS_BOSS4_BODY_WORK body_work)
    {
        return GmBoss4UtilUpdateTurnGently(body_work.dir);
    }

    private static void gmBoss4BodyInitAtkNmlDrift(GMS_BOSS4_BODY_WORK body_work, int frame)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        MTM_ASSERT(frame > 0);
        GmBsCmnSetObjSpdZero(obj_work);
        body_work.drift_angle = 0;
        body_work.drift_ang_spd = (int)nnRoundOff((float)(AKM_DEGtoA32(180f) / (double)frame + 0.5));
        body_work.drift_timer = frame;
        body_work.drift_pivot_x = body_work.dir.direction != 1 ? GMM_BOSS4_AREA_LEFT() + FX_F32_TO_FX32(74f) : GMM_BOSS4_AREA_LEFT() + FX_F32_TO_FX32(310f);
        gmBoss4BodyUpdateAtkNmlDrift(body_work);
    }

    private static bool gmBoss4BodyUpdateAtkNmlDrift(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num;
        bool flag;
        if (body_work.drift_timer != 0)
        {
            --body_work.drift_timer;
            body_work.drift_angle = (int)(ushort.MaxValue & (long)(body_work.drift_angle + body_work.drift_ang_spd));
            num = FX_Mul(FX_Sin(body_work.drift_angle), FX_F32_TO_FX32(16f));
            flag = false;
        }
        else
        {
            num = 0;
            flag = true;
        }
        if (body_work.dir.direction == 0)
            num = -num;
        obsObjectWork.pos.x = body_work.drift_pivot_x + num;
        return flag;
    }

    private static void gmBoss4BodyInitEscapeMove(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.spd.x = 0;
        obsObjectWork.spd_add.x = body_work.dir.direction != 1 ? 409 : -409;
        obsObjectWork.spd_add.y = 204;
    }

    private static bool gmBoss4BodyUpdateEscapeMove(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        bool flag = false;
        if (MTM_MATH_ABS(obj_work.spd.x) >= 11264)
        {
            obj_work.spd.x = 11264;
            obj_work.spd.y = -1536;
            obj_work.spd_add.x = 0;
            obj_work.spd_add.y = 0;
        }
        float right = g_gm_main_system.map_fcol.right;
        if (obj_work.pos.x > FX_F32_TO_FX32(right + 100f))
            flag = true;
        if (flag)
            GmBsCmnSetObjSpdZero(obj_work);
        return flag;
    }

    private static void gmBoss4BodyInitDefeatState(GMS_BOSS4_BODY_WORK body_work)
    {
        bool flag = false;
        if (((int)body_work.flag[0] & 1) != 0)
            flag = true;
        gmBoss4BodyChangeState(body_work, 7, true);
        if (flag)
            body_work.flag[0] |= 1U;
        else
            body_work.flag[0] &= 4294967294U;
        GmSoundChangeWinBossBGM();
    }

    private static void gmBoss4BodyUpdateChainTopDirection(GMS_BOSS4_BODY_WORK body_work)
    {
    }

    private static void gmBoss4BodyAtkHitFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        ((GMS_BOSS4_BODY_WORK)my_rect.parent_obj).flag[0] |= 268435456U;
        GmEnemyDefaultAtkFunc(my_rect, your_rect);
    }

    private static void gmBoss4BodyDamageDefFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        GMS_BOSS4_BODY_WORK body_work = (GMS_BOSS4_BODY_WORK)parentObj1;
        if (parentObj2 == null || 1 != parentObj2.obj_type)
            return;
        GmBoss4UtilSetPlayerAttackReaction(parentObj2, parentObj1);
        if (body_work.nohit_work.timer == 0U)
        {
            GmSoundPlaySE("Boss0_01");
            gmBoss4EffDamageInit(body_work);
            gmBoss4BodyExecDamageRoutine(body_work);
            if (GmBoss4Is2ndStage())
            {
                GmBoss4ChibiExplosion();
                body_work.wait_timer = 60U;
                body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate2nd);
            }
            GMM_PAD_VIB_SMALL_TIME(30f);
        }
        GmBoss4UtilInitNoHitTimer(body_work.nohit_work, (GMS_ENEMY_COM_WORK)parentObj1, 10);
    }

    private static void gmBoss4BodyDefHitFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        parentObj2.pos.x -= parentObj2.move.x;
        if (parentObj1.pos.x > parentObj2.pos.x)
        {
            parentObj2.pos.x -= FX_F32_TO_FX32(2f);
            parentObj2.spd.x = -MTM_MATH_ABS(parentObj2.spd.x);
            parentObj2.spd_m = -MTM_MATH_ABS(parentObj2.spd_m);
        }
        if (parentObj1.pos.x >= parentObj2.pos.x)
            return;
        parentObj2.pos.x += FX_F32_TO_FX32(2f);
        parentObj2.spd.x = MTM_MATH_ABS(parentObj2.spd.x);
        parentObj2.spd_m = MTM_MATH_ABS(parentObj2.spd_m);
    }

    private static void gmBoss4BodyOutFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK gmsBosS4BodyWork = (GMS_BOSS4_BODY_WORK)obj_work;
        GmBsCmnUpdateCNMParam(obj_work, gmsBosS4BodyWork.cnm_mgr_work);
        ObjDrawActionSummary(obj_work);
    }

    private static void gmBoss4BodyChangeState(GMS_BOSS4_BODY_WORK body_work, int state)
    {
        gmBoss4BodyChangeState(body_work, state, false);
    }

    private static void gmBoss4BodyChangeState(
      GMS_BOSS4_BODY_WORK body_work,
      int state,
      bool is_wrapped)
    {
        GMF_BOSS4_BODY_STATE_LEAVE_FUNC bodyStateLeaveFunc = gm_boss4_body_state_leave_func_tbl[body_work.state];
        if (bodyStateLeaveFunc != null)
            bodyStateLeaveFunc(body_work);
        body_work.prev_state = body_work.state;
        body_work.state = state;
        GMS_BOSS4_BODY_STATE_ENTER_INFO bodyStateEnterInfo = gm_boss4_body_state_enter_info_tbl[body_work.state];
        if (bodyStateEnterInfo.enter_func == null)
            return;
        bodyStateEnterInfo.enter_func(body_work);
    }

    private static void gmBoss4BodyWaitLoad(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK body_work = (GMS_BOSS4_BODY_WORK)obj_work;
        if (!GmBoss4IsBuilded())
            return;
        GmBoss4UtilInitNodeMatrix(body_work.node_work, obj_work, 6);
        GmBoss4UtilGetNodeMatrix(body_work.node_work, 2);
        GmBoss4UtilGetNodeMatrix(body_work.node_work, 2);
        GmBoss4UtilGetNodeMatrix(body_work.node_work, 9);
        GmBoss4UtilGetNodeMatrix(body_work.node_work, 10);
        GmBoss4UtilGetNodeMatrix(body_work.node_work, 5);
        GmBoss4UtilGetNodeMatrix(body_work.node_work, 8);
        GmBsCmnCreateCNMMgrWork(body_work.cnm_mgr_work, obj_work.obj_3d._object, 1);
        GmBsCmnInitCNMCb(obj_work, body_work.cnm_mgr_work);
        body_work.chaintop_cnm_reg_id = GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, 0);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4BodyMain);
        body_work.damage_timer = 0;
        GmBoss4UtilInitNoHitTimer(body_work.nohit_work, (GMS_ENEMY_COM_WORK)body_work, 0);
        if (GmBoss4CheckBossRush())
            gmBoss4BodyChangeState(body_work, 5);
        else
            gmBoss4BodyChangeState(body_work, 1);
    }

    private static void gmBoss4BodyMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK gmsBosS4BodyWork = (GMS_BOSS4_BODY_WORK)obj_work;
        GmBoss4UtilUpdateNoHitTimer(gmsBosS4BodyWork.nohit_work);
        if (gmsBosS4BodyWork.proc_update != null)
            gmsBosS4BodyWork.proc_update(gmsBosS4BodyWork);
        gmBoss4BodyUpdateSuspendAction(gmsBosS4BodyWork);
        gmBoss4EffAfterburnerUpdateCreate(gmsBosS4BodyWork);
        if (((int)gmsBosS4BodyWork.flag[0] & int.MinValue) != 0)
        {
            gmsBosS4BodyWork.flag[0] &= 1073741823U;
            gmBoss4BodyInitDefeatState(gmsBosS4BodyWork);
        }
        else
        {
            if (gmsBosS4BodyWork.damage_timer > 0)
                --gmsBosS4BodyWork.damage_timer;
            if (((int)gmsBosS4BodyWork.flag[0] & 1073741824) != 0)
            {
                gmsBosS4BodyWork.flag[0] &= 3221225471U;
                gmsBosS4BodyWork.flag[0] |= 536870912U;
                GmBsCmnInitObject3DNNDamageFlicker(obj_work, gmsBosS4BodyWork.flk_work, 32f);
            }
            GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, gmsBosS4BodyWork.flk_work);
            GmBoss4UtilUpdateDirection(gmsBosS4BodyWork.dir, obj_work);
            gmBoss4BodyUpdateChainTopDirection(gmsBosS4BodyWork);
            GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
            GMS_PLAYER_WORK playerObj = (GMS_PLAYER_WORK)GmBsCmnGetPlayerObj();
            if (playerObj.seq_state == GME_PLY_SEQ_STATE_JUMP || playerObj.seq_state == GME_PLY_SEQ_STATE_HOMING)
                gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
            else
                gmsEnemy3DWork.ene_com.rect_work[2].flag |= 4U;
        }
    }

    private static void gmBoss4BodyStateEnterStart(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        obj_work.flag |= 2U;
        body_work.flag[0] |= 64U;
        gmBoss4BodySetActionWhole(body_work, 0, true);
        body_work.flag[0] |= 32U;
        GmBsCmnSetObjSpdZero(obj_work);
        GmBoss4UtilSetDirectionNormal(body_work.dir);
        body_work.wait_timer = 120U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateStartWithWait);
        gmBoss4EffBossLightSetEnable(body_work, true);
    }

    private static void gmBoss4BodyStateLeaveStart(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss4EffAfterburnerSetEnable(body_work, 0);
        body_work.flag[0] &= 4294967231U;
        obsObjectWork.flag &= 4294967293U;
        body_work.flag[0] &= 4294967263U;
    }

    private static void gmBoss4BodyStateUpdateStartWithWait(GMS_BOSS4_BODY_WORK body_work)
    {
        if (!gmBoss4IsScrollLockBusy())
            return;
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateStartWithWaitEnd);
    }

    private static void gmBoss4BodyStateUpdateStartWithWaitEnd(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (gmBoss4IsScrollLockBusy())
            return;
        GmBsCmnSetObjSpd(obj_work, 0, 4096, 0);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateStartWithFall);
        GmBoss4CapsuleSetInvincible(600, false);
        GmBoss4ChibiSetInvincible(false);
        body_work.wait_timer2 = 90U;
        body_work.flag[0] |= 268435456U;
        body_work.wait_timer = 120U;
        GmBoss4UtilLookAtPlayer(body_work.dir, obj_work, 1);
        GmBoss4UtilLookAt(body_work.dir);
        GmMapSetMapDrawSize(6);
    }

    private static void gmBoss4BodyStateUpdateStartWithFall(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        ((GMS_ENEMY_3D_WORK)obj_work).ene_com.enemy_flag |= 32768U;
        if (body_work.wait_timer2 > 0U)
        {
            --body_work.wait_timer2;
            GmBoss4UtilLookAtPlayer(body_work.dir, obj_work, 1);
        }
        if (--body_work.wait_timer <= 0U && obj_work.pos.y <= FX_F32_TO_FX32(235f))
        {
            body_work.flag[0] |= 268435456U;
            body_work.wait_timer = 120U;
        }
        if (obj_work.pos.y < body_work.atk_nml_alt)
            return;
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.pos.y = body_work.atk_nml_alt;
        GmBsCmnSetObjSpd(obj_work, 0, 0, 0);
        body_work.wait_timer = 30U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateStartWithFallWait);
        GmBoss4UtilLookAtPlayer(body_work.dir, obj_work, 28);
    }

    private static void gmBoss4BodyStateUpdateStartWithFallWait(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBoss4UtilLookAt(body_work.dir);
        if (--body_work.wait_timer != 0U)
            return;
        GmBoss4CapsuleSetInvincible(0);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.pos.y = body_work.atk_nml_alt;
        GmBsCmnSetObjSpd(obj_work, -4096, 0, 0);
        gmBoss4EffAfterburnerSetEnable(body_work, 1);
        gmBoss4BodyChangeState(body_work, 2);
        ((GMS_ENEMY_3D_WORK)(GMS_BOSS4_EGG_WORK)body_work.parts_objs[1]).ene_com.enemy_flag &= 4294934527U;
    }

    private static void gmBoss4BodyStateEnterPreAtkNml(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.flag &= 4294967293U;
        gmBoss4BodySetActionWhole(body_work, 2);
        gmBoss4BodyInitPreANChainMotion(body_work);
        gmBoss4BodyInitPreANMove(body_work);
        gmBoss4EffAfterburnerSetEnable(body_work, 1);
        if (body_work.dir.direction == 0)
        {
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdatePreAtkNmlWithMoveRight);
            obsObjectWork.spd_add.x = -obsObjectWork.spd_add.x;
        }
        else
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdatePreAtkNmlWithMoveLeft);
    }

    private static void gmBoss4BodyStateLeavePreAtkNml(GMS_BOSS4_BODY_WORK body_work)
    {
        gmBoss4EffAfterburnerSetEnable(body_work, 0);
    }

    private static void gmBoss4BodyStateUpdatePreAtkNmlWithMoveLeft(
      GMS_BOSS4_BODY_WORK body_work)
    {
        VecFx32 vecFx32_1 = new VecFx32(FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(-30f), FX_F32_TO_FX32(0.0f));
        VecFx32 vecFx32_2 = new VecFx32(FX_F32_TO_FX32(180f), FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f));
        if (((int)body_work.flag[0] & 2048) != 0)
        {
            gmBoss4EffAfterburnerSetEnable(body_work, 0);
            if (((int)body_work.flag[0] & 1024) == 0)
                GmBoss4EffCommonInit(742, new VecFx32?(vecFx32_1), (OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 2, new VecFx32?(vecFx32_2), body_work.flag, 1024U);
            OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
            obsObjectWork.spd.x = 0;
            obsObjectWork.spd_add.x = 0;
            if (body_work.ene_3d.ene_com.obj_work.pos.y > FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd += FX_F32_TO_FX32(0.03f);
            }
            else
            {
                body_work.avoid_yspd -= FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = FX_F32_TO_FX32(1f);
            }
            body_work.ene_3d.ene_com.obj_work.pos.y -= body_work.avoid_yspd;
            if (body_work.ene_3d.ene_com.obj_work.pos.y < FX_F32_TO_FX32(190f))
            {
                body_work.ene_3d.ene_com.obj_work.pos.y = FX_F32_TO_FX32(190f);
                --body_work.avoid_timer;
            }
            if (body_work.avoid_timer < 0)
            {
                body_work.flag[0] &= 4294965247U;
                body_work.avoid_yspd = 0;
            }
            ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], -8, 20, 8, 40);
        }
        else if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
        {
            body_work.flag[0] |= 4096U;
            body_work.ene_3d.ene_com.rect_work[1].flag &= 4294967291U;
            gmBoss4EffAfterburnerSetEnable(body_work, 0);
            body_work.flag[0] &= 4294966271U;
            if (body_work.ene_3d.ene_com.obj_work.pos.y > FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd -= FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = FX_F32_TO_FX32(1f);
            }
            else
                body_work.avoid_yspd += FX_F32_TO_FX32(0.03f);
            body_work.ene_3d.ene_com.obj_work.pos.y += body_work.avoid_yspd;
            if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
                return;
            gmBoss4BodyInitPreANMove(body_work);
            gmBoss4EffAfterburnerSetEnable(body_work, 0);
            gmBoss4EffAfterburnerSetEnable(body_work, 1);
            body_work.flag[0] &= 4294966271U;
            ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], -32, -8, 32, 40);
            body_work.ene_3d.ene_com.rect_work[1].flag |= 4U;
        }
        else
        {
            body_work.ene_3d.ene_com.obj_work.pos.y = body_work.atk_nml_alt;
            body_work.flag[0] &= 4294963199U;
            GmBoss4UtilSetDirectionNormal(body_work.dir);
            if (gmBoss4BodyUpdatePreANMoveLeft(body_work))
            {
                gmBoss4BodyChangeState(body_work, 3);
                gmBoss4BodySetANChainInitialBlendSpd(body_work);
            }
            if (body_work.damage_timer == 0)
                return;
            for (int index = 1; index < GMD_BOSS4_SPEED_TIMES_IN_DAMAGE; ++index)
            {
                if (gmBoss4BodyUpdatePreANMoveLeft(body_work))
                {
                    gmBoss4BodyChangeState(body_work, 3);
                    gmBoss4BodySetANChainInitialBlendSpd(body_work);
                    break;
                }
            }
        }
    }

    private static void gmBoss4BodyStateUpdatePreAtkNmlWithMoveRight(
      GMS_BOSS4_BODY_WORK body_work)
    {
        VecFx32 vecFx32_1 = new VecFx32(FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(-30f), FX_F32_TO_FX32(0.0f));
        VecFx32 vecFx32_2 = new VecFx32(FX_F32_TO_FX32(180f), FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f));
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (((int)body_work.flag[0] & 2048) != 0)
        {
            gmBoss4EffAfterburnerSetEnable(body_work, 0);
            if (((int)body_work.flag[0] & 1024) == 0)
                GmBoss4EffCommonInit(742, new VecFx32?(vecFx32_1), (OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 2, new VecFx32?(vecFx32_2), body_work.flag, 1024U);
            obsObjectWork.spd.x = 0;
            obsObjectWork.spd_add.x = 0;
            if (body_work.ene_3d.ene_com.obj_work.pos.y > FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd += FX_F32_TO_FX32(0.03f);
            }
            else
            {
                body_work.avoid_yspd -= FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = FX_F32_TO_FX32(1f);
            }
            body_work.ene_3d.ene_com.obj_work.pos.y -= body_work.avoid_yspd;
            if (body_work.ene_3d.ene_com.obj_work.pos.y < FX_F32_TO_FX32(190f))
            {
                body_work.ene_3d.ene_com.obj_work.pos.y = FX_F32_TO_FX32(190f);
                --body_work.avoid_timer;
            }
            if (body_work.avoid_timer < 0)
            {
                body_work.flag[0] &= 4294965247U;
                body_work.avoid_yspd = 0;
            }
            ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], -8, 20, 8, 40);
        }
        else if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
        {
            body_work.ene_3d.ene_com.rect_work[1].flag &= 4294967291U;
            body_work.flag[0] |= 4096U;
            gmBoss4EffAfterburnerSetEnable(body_work, 0);
            body_work.flag[0] &= 4294966271U;
            if (body_work.ene_3d.ene_com.obj_work.pos.y > FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd -= FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = FX_F32_TO_FX32(1f);
            }
            else
                body_work.avoid_yspd += FX_F32_TO_FX32(0.03f);
            body_work.ene_3d.ene_com.obj_work.pos.y += body_work.avoid_yspd;
            if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
                return;
            gmBoss4BodyInitPreANMove(body_work);
            obsObjectWork.spd_add.x = -obsObjectWork.spd_add.x;
            gmBoss4EffAfterburnerSetEnable(body_work, 0);
            gmBoss4EffAfterburnerSetEnable(body_work, 1);
            body_work.flag[0] &= 4294966271U;
            ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], -32, -8, 32, 40);
            body_work.ene_3d.ene_com.rect_work[1].flag |= 4U;
        }
        else
        {
            body_work.ene_3d.ene_com.obj_work.pos.y = body_work.atk_nml_alt;
            body_work.flag[0] &= 4294963199U;
            GmBoss4UtilSetDirectionNormal(body_work.dir);
            if (gmBoss4BodyUpdatePreANMoveRight(body_work))
            {
                gmBoss4BodyChangeState(body_work, 3);
                gmBoss4BodySetANChainInitialBlendSpd(body_work);
            }
            if (body_work.damage_timer == 0)
                return;
            for (int index = 1; index < GMD_BOSS4_SPEED_TIMES_IN_DAMAGE; ++index)
            {
                if (gmBoss4BodyUpdatePreANMoveRight(body_work))
                {
                    gmBoss4BodyChangeState(body_work, 3);
                    gmBoss4BodySetANChainInitialBlendSpd(body_work);
                    break;
                }
            }
        }
    }

    private static void gmBoss4BodyStateEnterAtkNml(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        bool force_change = false;
        obsObjectWork.flag &= 4294967293U;
        if (body_work.dir.direction == 1)
            force_change = true;
        gmBoss4BodySetActionWhole(body_work, 3, force_change);
        gmBoss4BodyInitAtkNmlFlipAndTurn(body_work);
        gmBoss4BodyInitAtkNmlDrift(body_work, GMD_BOSS4_BODY_ATKNML_DRIFT_FRAME);
        gmBoss4EffAfterburnerSetEnable(body_work, 0);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateAtkNmlWithTurn);
    }

    private static void gmBoss4BodyStateLeaveAtkNml(GMS_BOSS4_BODY_WORK body_work)
    {
        gmBoss4EffAfterburnerSetEnable(body_work, 0);
    }

    private static void gmBoss4BodyStateUpdateAtkNmlWithTurn(GMS_BOSS4_BODY_WORK body_work)
    {
        bool flag = gmBoss4BodyUpdateAtkNmlDrift(body_work);
        if (!gmBoss4BodyUpdateAtkNmlFlipAndTurn(body_work) || !flag)
            return;
        gmBoss4BodySetFlipForAtkNmlMove(body_work);
        gmBoss4BodyInitAtkNmlMove(body_work, GMD_BOSS4_BODY_ATKNML_MOVE_FRAME);
        gmBoss4EffAfterburnerSetEnable(body_work, 1);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateAtkNmlWithMove);
    }

    private static void gmBoss4BodyStateUpdateAtkNmlWithMove(GMS_BOSS4_BODY_WORK body_work)
    {
        VecFx32 vecFx32_1 = new VecFx32(FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(-30f), FX_F32_TO_FX32(0.0f));
        VecFx32 vecFx32_2 = new VecFx32(FX_F32_TO_FX32(180f), FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f));
        if (((int)body_work.flag[0] & 2048) != 0)
        {
            gmBoss4EffAfterburnerSetEnable(body_work, 0);
            if (((int)body_work.flag[0] & 1024) == 0)
                GmBoss4EffCommonInit(742, new VecFx32?(vecFx32_1), (OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 2, new VecFx32?(vecFx32_2), body_work.flag, 1024U);
            if (body_work.ene_3d.ene_com.obj_work.pos.y > FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd += FX_F32_TO_FX32(0.03f);
            }
            else
            {
                body_work.avoid_yspd -= FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = FX_F32_TO_FX32(1f);
            }
            body_work.ene_3d.ene_com.obj_work.pos.y -= body_work.avoid_yspd;
            if (body_work.ene_3d.ene_com.obj_work.pos.y < FX_F32_TO_FX32(190f))
            {
                body_work.ene_3d.ene_com.obj_work.pos.y = FX_F32_TO_FX32(190f);
                --body_work.avoid_timer;
            }
            if (body_work.avoid_timer < 0)
            {
                body_work.flag[0] &= 4294965247U;
                body_work.avoid_yspd = 0;
            }
            ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], -8, 20, 8, 40);
        }
        else if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
        {
            body_work.ene_3d.ene_com.rect_work[1].flag &= 4294967291U;
            body_work.flag[0] |= 4096U;
            gmBoss4EffAfterburnerSetEnable(body_work, 0);
            body_work.flag[0] &= 4294966271U;
            if (body_work.ene_3d.ene_com.obj_work.pos.y > FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd -= FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = FX_F32_TO_FX32(1f);
            }
            else
                body_work.avoid_yspd += FX_F32_TO_FX32(0.03f);
            body_work.ene_3d.ene_com.obj_work.pos.y += body_work.avoid_yspd;
            if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
                return;
            gmBoss4EffAfterburnerSetEnable(body_work, 0);
            gmBoss4EffAfterburnerSetEnable(body_work, 1);
            body_work.flag[0] &= 4294966271U;
            ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], -32, -8, 32, 40);
            body_work.ene_3d.ene_com.rect_work[1].flag |= 4U;
        }
        else
        {
            body_work.ene_3d.ene_com.obj_work.pos.y = body_work.atk_nml_alt;
            body_work.flag[0] &= 4294963199U;
            GmBoss4UtilSetDirectionNormal(body_work.dir);
            if (gmBoss4BodyIsExtraAttack(body_work))
                gmBoss4BodyChangeState(body_work, 4);
            else if (gmBoss4BodyUpdateAtkNmlMove(body_work))
            {
                gmBoss4BodyChangeState(body_work, 3);
            }
            else
            {
                if (body_work.damage_timer == 0)
                    return;
                for (int index = 1; index < GMD_BOSS4_SPEED_TIMES_IN_DAMAGE; ++index)
                {
                    if (gmBoss4BodyUpdateAtkNmlMove(body_work))
                    {
                        gmBoss4BodyChangeState(body_work, 3);
                        break;
                    }
                }
            }
        }
    }

    private static void gmBoss4BodyStateEnter1stEnd(GMS_BOSS4_BODY_WORK body_work)
    {
        GmBoss4UtilInitNoHitTimer(body_work.nohit_work, (GMS_ENEMY_COM_WORK)body_work, 1200);
        ((GMS_ENEMY_3D_WORK)body_work).ene_com.rect_work[0].flag |= 2048U;
        GmBoss4CapsuleSetInvincible(600, false);
        GmBoss4ChibiSetInvincible(true);
        GmBoss4ChibiExplosion();
        GmBoss4UtilPlayerStop(true);
        GmBoss4UtilTimerStop(true);
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)body_work;
        VecFx32 end = new VecFx32(FX_F32_TO_FX32(ObjCameraGet(0).target_pos.x), FX_F32_TO_FX32(220f), 0);
        GmBoss4UtilInitMove(body_work.move_work, obsObjectWork.pos, end, 180, 1);
        bool is_positive = GmBoss4UtilIsDirectionPositiveFromCurrent(body_work.dir, GMD_BOSS4_LEFTWARD_ANGLE);
        body_work.dir.direction = 1;
        GmBoss4UtilInitTurnGently(body_work.dir, GMD_BOSS4_LEFTWARD_ANGLE, 40, is_positive);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate1stEnd);
    }

    private static void gmBoss4BodyStateUpdate1stEnd(GMS_BOSS4_BODY_WORK body_work)
    {
        GmBoss4UtilPlayerStop(true);
        GmBoss4ChibiExplosion();
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)body_work;
        GmBoss4UtilUpdateTurnGently(body_work.dir);
        if (body_work.move_work.now_count == 30)
        {
            VecFx32 vecFx32_1 = new VecFx32(FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(-30f), FX_F32_TO_FX32(0.0f));
            VecFx32 vecFx32_2 = new VecFx32(FX_F32_TO_FX32(180f), FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f));
            GmBoss4EffCommonInit(742, new VecFx32?(vecFx32_1), (OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 2, new VecFx32?(vecFx32_2), body_work.flag, 1024U);
        }
        if (GmBoss4UtilUpdateMove(body_work.move_work))
            body_work.proc_update = GmBoss4CapsuleGetCount() <= 0 ? new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateInit1stEndAngry) : new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateInit1stEndExplosion);
        GmBoss4UtilUpdateMovePosition(body_work.move_work, obj_work);
    }

    private static void gmBoss4BodyStateInit1stEndExplosion(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)body_work;
        GmBoss4UtilPlayerStop(true);
        GmBoss4CapsuleExplosion();
        body_work.wait_timer = 180U;
        body_work.flag[0] |= 16777216U;
        VecFx32 end = new VecFx32(obsObjectWork.pos.x, obsObjectWork.pos.y + FX_F32_TO_FX32(20f), 0);
        GmBoss4UtilInitMove(body_work.move_work, obsObjectWork.pos, end, 60, 1);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate1stEndExplosion);
    }

    private static void gmBoss4BodyStateUpdate1stEndExplosion(GMS_BOSS4_BODY_WORK body_work)
    {
        GmBoss4UtilPlayerStop(true);
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)body_work;
        GmBoss4UtilUpdateMove(body_work.move_work);
        GmBoss4UtilUpdateMovePosition(body_work.move_work, obj_work);
        if (body_work.wait_timer > 0U)
            --body_work.wait_timer;
        else
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateInit1stEndAngry);
    }

    private static void gmBoss4BodyStateInit1stEndAngry(GMS_BOSS4_BODY_WORK body_work)
    {
        GmBoss4UtilPlayerStop(true);
        gmBoss4BodySetActionWhole(body_work, 8);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate1stEndAngry);
    }

    private static void gmBoss4BodyStateUpdate1stEndAngry(GMS_BOSS4_BODY_WORK body_work)
    {
        GmBoss4UtilPlayerStop(true);
        if (GmBsCmnIsActionEnd((OBS_OBJECT_WORK)body_work) == 0)
            return;
        gmBoss4BodySetActionWhole(body_work, 9);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate1stEndAngryL2);
    }

    private static void gmBoss4BodyStateUpdate1stEndAngryL2(GMS_BOSS4_BODY_WORK body_work)
    {
        GmBoss4UtilPlayerStop(true);
        if (GmBsCmnIsActionEnd((OBS_OBJECT_WORK)body_work) == 0)
            return;
        gmBoss4BodySetActionWhole(body_work, 10);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateInit1stEndEscape);
    }

    private static void gmBoss4BodyStateInit1stEndEscape(GMS_BOSS4_BODY_WORK body_work)
    {
        GmBoss4UtilPlayerStop(true);
        gmBoss4EffAfterburnerSetEnable(body_work, 2);
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)body_work;
        VecFx32 end = new VecFx32(FX_F32_TO_FX32(ObjCameraGet(0).target_pos.x + 200f) + (GMM_BOSS4_AREA_RIGHT() - GMM_BOSS4_AREA_LEFT()) / 2, obsObjectWork.pos.y, 0);
        GmBoss4UtilInitMove(body_work.move_work, obsObjectWork.pos, end, 150, 1);
        GmMapSetMapDrawSize(1);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate1stEndEscape);
    }

    private static void gmBoss4BodyStateUpdate1stEndEscape(GMS_BOSS4_BODY_WORK body_work)
    {
        GmBoss4UtilPlayerStop(true);
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)body_work;
        if (GmBoss4UtilUpdateMove(body_work.move_work))
            gmBoss4BodyChangeState(body_work, 5);
        else
            GmBoss4UtilUpdateMovePosition(body_work.move_work, obj_work);
    }

    private static void gmBoss4BodyStateEnter2nd(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)body_work;
        GmBoss4UtilInitNoHitTimer(body_work.nohit_work, (GMS_ENEMY_COM_WORK)body_work, 0);
        GmBoss4CapsuleSetInvincible(0);
        GmBoss4ChibiSetInvincible(false);
        GmBoss4UtilPlayerStop(false);
        GmBoss4UtilTimerStop(false);
        if (!GmBoss4CheckBossRush())
            GmGmkCamScrLimitRelease(4).user_work = 16U;
        obsObjectWork.pos.x = FX_F32_TO_FX32(GMD_BOSS4_BODY_2ND_POS_X);
        obsObjectWork.pos.y = FX_F32_TO_FX32(GMD_BOSS4_BODY_2ND_POS_Y);
        obsObjectWork.pos.z = -131072;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)body_work;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag |= 2048U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag |= 2048U;
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294965247U;
        ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[0], -38, -24, 38, 32);
        gmBoss4SetPartTextureBurnt((OBS_OBJECT_WORK)(GMS_BOSS4_EGG_WORK)body_work.parts_objs[1], false);
        GmBoss4UtilInitTurnGently(body_work.dir, GMD_BOSS4_LEFTWARD_ANGLE, 1, false);
        GmBoss4UtilUpdateTurnGently(body_work.dir);
        gmBoss4BodySetActionWhole(body_work, 5);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateInit2nd);
        GmBoss4CapsuleExplosion();
        bool is_positive = GmBoss4UtilIsDirectionPositiveFromCurrent(body_work.dir, GMD_BOSS4_LEFTWARD_ANGLE);
        body_work.dir.direction = 1;
        GmBoss4UtilInitTurnGently(body_work.dir, GMD_BOSS4_LEFTWARD_ANGLE, 1, is_positive);
        GmBoss4UtilUpdateTurnGently(body_work.dir);
        gm_boss4_locking = 0;
    }

    private static void gmBoss4BodyStateInit2nd(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)body_work;
        if (GmBsCmnGetPlayerObj().pos.x >= FX_F32_TO_FX32(GMD_BOSS4_SCROLL_INIT_X))
        {
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate2ndWaitBoss);
            GmBoss4ScrollInit(null, 0, 0, 0);
            body_work.wait_timer = GMD_BOSS4_BODY_SONIC_CTRL_TIME;
            if (GmBoss4CheckBossRush())
            {
                gmBoss4BodySetActionWhole(body_work, 5);
                gmBoss4EffAfterburnerSetEnable(body_work, 0);
                gmBoss4EffAfterburnerSetEnable(body_work, 2);
            }
            else
                GmSoundChangeAngryBossBGM();
            GmMapSetMapDrawSize(7);
        }
        obsObjectWork.pos.y = FX_F32_TO_FX32(GMD_BOSS4_BODY_2ND_POS_Y);
    }

    private static void gmBoss4BodyStateUpdate2ndWaitBoss(GMS_BOSS4_BODY_WORK body_work)
    {
        if (((int)((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj()).obj_work.move_flag & 1) != 0)
            GmBoss4UtilPlayerStop(true);
        float right = g_gm_main_system.map_fcol.right;
        if (((OBS_OBJECT_WORK)body_work).pos.x < FX_F32_TO_FX32(right - 10f))
            return;
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate2nd);
            GmBoss4ScrollInit(null, 0, 0, 0);
            body_work.wait_timer = GMD_BOSS4_BODY_CREATE_CAP_FIRST_TIME;
            GmBoss4UtilPlayerStop(false);
        }
    }

    private static void gmBoss4BodyStateUpdate2nd(GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateInit2ndAttack);
            body_work.wait_timer = 34U;
            body_work.flag[0] |= 2097152U;
            GMS_BOSS4_EGG_WORK partsObj = (GMS_BOSS4_EGG_WORK)body_work.parts_objs[1];
            GmBoss4UtilGetNodeMatrix(partsObj.node_work, 9);
            GmBoss4UtilGetNodeMatrix(partsObj.node_work, 6);
        }
    }

    private static void gmBoss4BodyStateInit2ndAttack(GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            OBS_OBJECT_WORK obsObjectWork1 = (OBS_OBJECT_WORK)body_work;
            NNS_MATRIX nodeMatrix1 = GmBoss4UtilGetNodeMatrix(((GMS_BOSS4_EGG_WORK)body_work.parts_objs[1]).node_work, 9);
            NNS_MATRIX nodeMatrix2 = GmBoss4UtilGetNodeMatrix(body_work.node_work, 2);
            NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
            nnsVector.x = (float)(nodeMatrix1.M03 - (double)nodeMatrix2.M03 + obsObjectWork1.pos.x / 4096.0);
            nnsVector.y = nodeMatrix1.M13;
            nnsVector.z = nodeMatrix1.M23;
            OBS_OBJECT_WORK obsObjectWork2 = GmEventMgrLocalEventBirth(324, FX_F32_TO_FX32(nnsVector.x + 0.0f), -FX_F32_TO_FX32(nnsVector.y - 22f), 0, 0, 0, 0, 0, 0);
            obsObjectWork2.parent_obj = obsObjectWork1;
            GmBoss4IncObjCreateCount();
            if (gmBoss4ChibiGetThrowType() != 0)
            {
                obsObjectWork2.spd.x = FX_F32_TO_FX32(GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_1);
                obsObjectWork2.spd.y = FX_F32_TO_FX32(GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_1);
            }
            else
            {
                obsObjectWork2.spd.x = FX_F32_TO_FX32(GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_2);
                obsObjectWork2.spd.y = FX_F32_TO_FX32(GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_2);
            }
            obsObjectWork2.pos.z = 0;
            if (GmBoss4GetLife() < GME_BOSS4_LIFE_H && GmBoss4GetLife() > GME_BOSS4_LIFE_L)
            {
                body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate2ndAttackWait);
                body_work.wait_timer = GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_2_2;
            }
            else
                body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate2ndAttack);
            GlobalPool<NNS_VECTOR>.Release(nnsVector);
        }
    }

    private static void gmBoss4BodyStateUpdate2ndAttackWait(GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateInit2ndAttack2);
            body_work.wait_timer = 34U;
            body_work.flag[0] |= 4194304U;
        }
    }

    private static void gmBoss4BodyStateInit2ndAttack2(GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            OBS_OBJECT_WORK obsObjectWork1 = (OBS_OBJECT_WORK)body_work;
            NNS_MATRIX nodeMatrix1 = GmBoss4UtilGetNodeMatrix(((GMS_BOSS4_EGG_WORK)body_work.parts_objs[1]).node_work, 6);
            NNS_MATRIX nodeMatrix2 = GmBoss4UtilGetNodeMatrix(body_work.node_work, 2);
            NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
            nnsVector.x = (float)(nodeMatrix1.M03 - (double)nodeMatrix2.M03 + obsObjectWork1.pos.x / 4096.0);
            nnsVector.y = nodeMatrix1.M13;
            nnsVector.z = nodeMatrix1.M23;
            OBS_OBJECT_WORK obsObjectWork2 = GmEventMgrLocalEventBirth(324, FX_F32_TO_FX32(nnsVector.x + 0.0f), -FX_F32_TO_FX32(nnsVector.y - 22f), 0, 0, 0, 0, 0, 0);
            obsObjectWork2.parent_obj = obsObjectWork1;
            GmBoss4IncObjCreateCount();
            if (gmBoss4ChibiGetThrowType() != 0)
            {
                obsObjectWork2.spd.x = FX_F32_TO_FX32(GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_1);
                obsObjectWork2.spd.y = FX_F32_TO_FX32(GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_1);
            }
            else
            {
                obsObjectWork2.spd.x = FX_F32_TO_FX32(GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_2);
                obsObjectWork2.spd.y = FX_F32_TO_FX32(GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_2);
            }
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate2ndAttack);
            GlobalPool<NNS_VECTOR>.Release(nnsVector);
        }
    }

    private static void gmBoss4BodyStateUpdate2ndAttack(GMS_BOSS4_BODY_WORK body_work)
    {
        body_work.wait_timer = GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_3;
        if (GmBoss4GetLife() == 1)
            body_work.wait_timer = GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_1;
        if (GmBoss4GetLife() == 2)
            body_work.wait_timer = GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_2;
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdate2nd);
    }

    private static void gmBoss4BodyStateLeave1stEnd(GMS_BOSS4_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss4BodyStateLeave2nd(GMS_BOSS4_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss4BodyStateEnterDmgNml(GMS_BOSS4_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss4BodyStateLeaveDmgNml(GMS_BOSS4_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss4BodyStateEnterDefeat(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        obj_work.flag |= 2U;
        body_work.flag[0] |= 8U;
        GmBsCmnSetObjSpdZero(obj_work);
        body_work.wait_timer = 40U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateDefeatWithWaitStart);
        gmBoss4EffAfterburnerSetEnable(body_work, 0);
        GmBoss4ScrollNext();
        GmBoss4UtilInitNoHitTimer(body_work.nohit_work, (GMS_ENEMY_COM_WORK)body_work, 1200);
        GmBoss4CapsuleSetInvincible(600, false);
        GmBoss4ChibiSetInvincible(true);
    }

    private static void gmBoss4BodyStateLeaveDefeat(GMS_BOSS4_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss4BodyStateUpdateDefeatWithWaitStart(
      GMS_BOSS4_BODY_WORK body_work)
    {
        GmBoss4ChibiExplosion();
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            GmBoss4EffBombInitCreate(body_work.bomb_work, 0, GMM_BS_OBJ(body_work), GMM_BS_OBJ(body_work).pos.x, GMM_BS_OBJ(body_work).pos.y, FX_F32_TO_FX32(80f), FX_F32_TO_FX32(80f), GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_INTERVAL_MIN_TIME, GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_INTERVAL_MAX_TIME);
            GmBoss4EffBombInitCreate(body_work.bomb_work2, 5, GMM_BS_OBJ(body_work), GMM_BS_OBJ(body_work).pos.x, GMM_BS_OBJ(body_work).pos.y, FX_F32_TO_FX32(80f), FX_F32_TO_FX32(80f), GMD_BOSS4_BODY_DEFEAT_BOMB_PARTS_INTERVAL_MIN_TIME, GMD_BOSS4_BODY_DEFEAT_BOMB_PARTS_INTERVAL_MAX_TIME);
            body_work.wait_timer = GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_TIME;
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateDefeatWithExplode);
        }
    }

    private static void gmBoss4BodyStateUpdateDefeatWithExplode(GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
            GmBoss4EffBombUpdateCreate(body_work.bomb_work);
            GmBoss4EffBombUpdateCreate(body_work.bomb_work2);
            body_work.bomb_work.pos[0] -= FX_F32_TO_FX32(GMD_BOSS4_SCROLL_SPD_MAX - GMD_BOSS4_SCROLL_SPD_BOSS_BROKEN);
            body_work.bomb_work2.pos[0] -= FX_F32_TO_FX32(GMD_BOSS4_SCROLL_SPD_MAX - GMD_BOSS4_SCROLL_SPD_BOSS_BROKEN);
        }
        else
        {
            GmSoundPlaySE("Boss0_03");
            GMM_PAD_VIB_MID_TIME(120f);
            GmBsCmnInitFlashScreen(body_work.flash_work, 4f, 5f, 30f);
            GmPlayerAddScoreNoDisp((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), 1000);
            OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(GMM_BS_OBJ(body_work), 8);
            obsObjectWork.pos.z = obsObjectWork.parent_obj.pos.z + 131072;
            GmBoss4EffChangeType((GMS_EFFECT_3DES_WORK)obsObjectWork, 2U, 1U);
            obsObjectWork.spd.x -= FX_F32_TO_FX32(1f);
            gmBoss4BodySetActionWhole(body_work, 7);
            GmBoss4ScrollOut();
            body_work.wait_timer = 40U;
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateDefeatWithScatter);
        }
    }

    private static void gmBoss4BodyStateUpdateDefeatWithScatter(GMS_BOSS4_BODY_WORK body_work)
    {
        GmBsCmnUpdateFlashScreen(body_work.flash_work);
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            gmBoss4SetPartTextureBurnt(GMM_BS_OBJ(body_work));
            body_work.flag[0] |= 16777216U;
            gmBoss4EffABSmokeInit(body_work);
            gmBoss4EffBodySmokeInit(body_work);
            body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateDefeatWithWaitEnd);
        }
    }

    private static void gmBoss4BodyStateUpdateDefeatWithWaitEnd(GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
            --body_work.wait_timer;
        else
            gmBoss4BodyChangeState(body_work, 8);
    }

    private static void gmBoss4BodyStateEnterEscape(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.flag |= 2U;
        obsObjectWork.disp_flag &= 4294967279U;
        gmBoss4BodySetActionWhole(body_work, 7);
        body_work.flag[0] |= 8388608U;
        bool is_positive = GmBoss4UtilIsDirectionPositiveFromCurrent(body_work.dir, GMD_BOSS4_RIGHTWARD_ANGLE);
        GmBoss4UtilInitTurnGently(body_work.dir, GMD_BOSS4_RIGHTWARD_ANGLE, 90, is_positive);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateEscapeWithTurn);
    }

    private static void gmBoss4BodyStateLeaveEscape(GMS_BOSS4_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
    }

    private static void gmBoss4BodyStateUpdateEscapeWithTurn(GMS_BOSS4_BODY_WORK body_work)
    {
        if (!GmBoss4UtilUpdateTurnGently(body_work.dir))
            return;
        gmBoss4BodyInitEscapeMove(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateEscapeWithMoveLocked);
    }

    private static void gmBoss4BodyStateUpdateEscapeWithMoveLocked(
      GMS_BOSS4_BODY_WORK body_work)
    {
        gmBoss4BodyUpdateEscapeMove(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateEscapeWithMoveUnlocked);
    }

    private static void gmBoss4BodyStateUpdateEscapeWithMoveUnlocked(
      GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK parent_obj = (OBS_OBJECT_WORK)body_work;
        float right = g_gm_main_system.map_fcol.right;
        gmBoss4BodyUpdateEscapeMove(body_work);
        if (parent_obj.pos.x <= FX_F32_TO_FX32(right - 100f))
            return;
        GmBoss4EffChangeType(GmEfctBossCmnEsCreate(parent_obj, 4U), 2U, 3U);
        body_work.proc_update = new MPP_VOID_GMS_BOSS4_BODY_WORK(gmBoss4BodyStateUpdateEscapeWithMoveFinish);
        GmMapSetMapDrawSize(1);
    }

    private static void gmBoss4BodyStateUpdateEscapeWithMoveFinish(
      GMS_BOSS4_BODY_WORK body_work)
    {
        if (!gmBoss4BodyUpdateEscapeMove(body_work))
            return;
        GMM_BOSS4_MGR(body_work).flag |= 2U;
        body_work.proc_update = null;
        GmBoss4ScrollNext();
    }

}