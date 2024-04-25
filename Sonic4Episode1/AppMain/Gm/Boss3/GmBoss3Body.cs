public partial class AppMain
{
    private static void gmBoss3BodyExit(MTS_TASK_TCB tcb)
    {
        GMS_BOSS3_BODY_WORK tcbWork = (GMS_BOSS3_BODY_WORK)mtTaskGetTcbWork(tcb);
        OBS_OBJECT_WORK objWork = tcbWork.ene_3d.ene_com.obj_work;
        GmBsCmnClearBossMotionCBSystem(objWork);
        GmBsCmnDeleteSNMWork(tcbWork.snm_work);
        GmBsCmnClearCNMCb(objWork);
        gmBoss3ExitFunc(tcb);
    }

    private static void gmBoss3BodyReactionPlayer(
      OBS_OBJECT_WORK obj_work_player,
      OBS_OBJECT_WORK obj_work_body)
    {
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)obj_work_player;
        GmPlySeqAtkReactionInit(ply_work);
        GmPlySeqSetJumpState(ply_work, 0, 5U);
        int num1 = 2048;
        int num2 = 1228;
        if (ply_work.seq_state == GME_PLY_SEQ_STATE_HOMING_REF)
        {
            num1 = 12288;
            num2 = 12288;
        }
        obj_work_player.spd_m = 0;
        obj_work_player.spd.x = obj_work_player.move.x < 0 ? num1 : -num1;
        obj_work_player.spd.y = obj_work_player.pos.y > obj_work_body.pos.y ? num2 : -num2;
        GmPlySeqSetNoJumpMoveTime(ply_work, 102400);
    }

    private static void gmBoss3BodySetRectNormal(GMS_BOSS3_BODY_WORK body_work)
    {
        ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], -8, -8, 8, 8);
        body_work.ene_3d.ene_com.rect_work[0].flag |= 4U;
    }

    private static void gmBoss3BodySetActionAllParts(
      GMS_BOSS3_BODY_WORK body_work,
      int action_id)
    {
        gmBoss3BodySetActionAllParts(body_work, action_id, 0);
    }

    private static void gmBoss3BodySetActionAllParts(
      GMS_BOSS3_BODY_WORK body_work,
      int action_id,
      int force_change)
    {
        if (force_change == 0 && body_work.action_id == action_id)
            return;
        body_work.action_id = action_id;
        for (int index = 0; 2 > index; ++index)
        {
            OBS_OBJECT_WORK partsObj = body_work.parts_objs[index];
            if (partsObj != null)
            {
                GMS_BOSS3_PART_ACT_INFO bosS3PartActInfo = gm_boss3_act_info_tbl[action_id][index];
                if (index != 1 || ((int)((GMS_BOSS3_EGG_WORK)partsObj).flag & 1) == 0)
                {
                    if (bosS3PartActInfo.is_maintain != 0)
                    {
                        if (bosS3PartActInfo.is_repeat != 0)
                            partsObj.disp_flag |= 4U;
                    }
                    else
                        GmBsCmnSetAction(partsObj, bosS3PartActInfo.mtn_id, bosS3PartActInfo.is_repeat, bosS3PartActInfo.is_blend);
                    partsObj.obj_3d.speed[0] = bosS3PartActInfo.mtn_spd;
                    partsObj.obj_3d.blend_spd = bosS3PartActInfo.blend_spd;
                }
            }
        }
    }

    private static void gmBoss3BodyOutFunc(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawActionSummary(obj_work);
    }

    private static void gmBoss3BodyChaseMoveFunc(OBS_OBJECT_WORK obj_work)
    {
        int x = obj_work.spd.x;
        int y = obj_work.spd.y;
        gmBoss3BodyChaseAdjustMoveSpeed((GMS_BOSS3_BODY_WORK)obj_work);
        ObjObjectMove(obj_work);
        obj_work.spd.x = x;
        obj_work.spd.y = y;
    }

    private static void gmBoss3BodyDefFunc(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj1 = target_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = own_rect.parent_obj;
        GMS_BOSS3_BODY_WORK body_work = (GMS_BOSS3_BODY_WORK)parentObj2;
        if (parentObj1 == null || 1 != parentObj1.obj_type)
            return;
        gmBoss3BodyReactionPlayer(parentObj1, parentObj2);
        gmBoss3BodySetNoHitTime(body_work, 10U);
        gmBoss3BodyDamage(body_work);
    }

    private static void gmBoss3BodySetNoHitTime(GMS_BOSS3_BODY_WORK body_work, uint time)
    {
        GMS_ENEMY_COM_WORK eneCom = body_work.ene_3d.ene_com;
        body_work.counter_no_hit = time;
        eneCom.rect_work[0].flag |= 2048U;
    }

    private static void gmBoss3BodyUpdateNoHitTime(GMS_BOSS3_BODY_WORK body_work)
    {
        if (body_work.counter_no_hit > 0U)
            --body_work.counter_no_hit;
        else
            body_work.ene_3d.ene_com.rect_work[0].flag &= 4294965247U;
    }

    private static void gmBoss3BodySetInvincibleTime(GMS_BOSS3_BODY_WORK body_work, uint time)
    {
        body_work.counter_invincible = time;
        body_work.flag |= 1U;
    }

    private static void gmBoss3BodyUpdateInvincibleTime(GMS_BOSS3_BODY_WORK body_work)
    {
        if (body_work.counter_invincible > 0U)
            --body_work.counter_invincible;
        else
            body_work.flag &= 4294967294U;
    }

    private static void gmBoss3BodySetDirection(GMS_BOSS3_BODY_WORK body_work, short deg)
    {
        body_work.angle_current = deg;
    }

    private static void gmBoss3BodySetDirectionNormal(GMS_BOSS3_BODY_WORK body_work)
    {
        if (((int)GMM_BS_OBJ(body_work).disp_flag & 1) != 0)
            gmBoss3BodySetDirection(body_work, GMD_BOSS3_ANGLE_LEFT);
        else
            gmBoss3BodySetDirection(body_work, GMD_BOSS3_ANGLE_RIGHT);
    }

    private static void gmBoss3BodyUpdateDirection(GMS_BOSS3_BODY_WORK body_work)
    {
        GMM_BS_OBJ(body_work).dir.y = (ushort)body_work.angle_current;
    }

    private static float gmBoss3BodyCalcMoveXNormalFrame(
      GMS_BOSS3_BODY_WORK body_work,
      int x,
      int speed)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        return MTM_MATH_ABS(x - obsObjectWork.pos.x) / (float)speed;
    }

    private static void gmBoss3BodyInitMoveNormal(
      GMS_BOSS3_BODY_WORK body_work,
      VecFx32 dest_pos,
      float frame)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        body_work.start_pos.x = obsObjectWork.pos.x;
        body_work.start_pos.y = obsObjectWork.pos.y;
        body_work.start_pos.z = obsObjectWork.pos.z;
        body_work.end_pos.x = dest_pos.x;
        body_work.end_pos.y = dest_pos.y;
        body_work.end_pos.z = body_work.start_pos.z;
        body_work.move_counter = 0.0f;
        if (frame > 0.0)
            body_work.move_frame = frame;
        else
            body_work.move_frame = 1f;
    }

    private static float gmBoss3BodyUpdateMoveNormal(GMS_BOSS3_BODY_WORK body_work)
    {
        VecFx32 vecFx32_1 = new VecFx32();
        ++body_work.move_counter;
        if (body_work.move_counter >= (double)body_work.move_frame)
        {
            vecFx32_1.x = body_work.end_pos.x;
            vecFx32_1.y = body_work.end_pos.y;
            vecFx32_1.z = body_work.end_pos.z;
        }
        else
        {
            float num = (float)(0.5 * (1.0 - nnCos(AKM_DEGtoA32(180f * (body_work.move_counter / body_work.move_frame)))));
            VecFx32 vecFx32_2 = new VecFx32();
            vecFx32_2.x = (int)((body_work.end_pos.x - body_work.start_pos.x) * (double)num);
            vecFx32_2.y = (int)((body_work.end_pos.y - body_work.start_pos.y) * (double)num);
            vecFx32_2.z = (int)((body_work.end_pos.z - body_work.start_pos.z) * (double)num);
            vecFx32_1.x = body_work.start_pos.x + vecFx32_2.x;
            vecFx32_1.y = body_work.start_pos.y + vecFx32_2.y;
            vecFx32_1.z = body_work.start_pos.z + vecFx32_2.z;
        }
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.pos.x = vecFx32_1.x;
        obsObjectWork.pos.y = vecFx32_1.y;
        obsObjectWork.pos.z = vecFx32_1.z;
        return body_work.move_frame - body_work.move_counter;
    }

    private static void gmBoss3BodyInitTurn(
      GMS_BOSS3_BODY_WORK body_work,
      short dest_angle,
      float frame,
      int flag_positive)
    {
        body_work.turn_counter = 0.0f;
        body_work.turn_frame = frame;
        body_work.turn_start = body_work.angle_current;
        ushort num1 = (ushort)((uint)dest_angle - (uint)body_work.angle_current);
        body_work.turn_amount = num1;
        if (flag_positive != 0)
            return;
        ushort num2 = (ushort)(body_work.turn_amount - AKM_DEGtoA32(360));
        body_work.turn_amount = num2 - AKM_DEGtoA32(360);
    }

    private static float gmBoss3BodyUpdateTurn(GMS_BOSS3_BODY_WORK body_work)
    {
        if (body_work.turn_frame < 1.0)
            return 0.0f;
        ++body_work.turn_counter;
        short deg;
        if (body_work.turn_counter >= (double)body_work.turn_frame)
        {
            deg = (short)(body_work.turn_start + body_work.turn_amount);
        }
        else
        {
            int ang = AKM_DEGtoA32(180f * (body_work.turn_counter / body_work.turn_frame));
            float num = (float)(body_work.turn_amount * 0.5 * (1.0 - nnCos(ang)));
            deg = (short)(body_work.turn_start + (double)num);
        }
        gmBoss3BodySetDirection(body_work, deg);
        return body_work.turn_frame - body_work.turn_counter;
    }

    private static int gmBoss3BodyChaseCheckTurn(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.spd.x < 0)
                return 0;
        }
        else if (obsObjectWork.spd.x > 0)
            return 0;
        return 1;
    }

    private static void gmBoss3BodyChaseAdjustMoveSpeed(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work_parts = GMM_BS_OBJ(body_work);
        int num = FX_F32_TO_FX32((float)(1.0 + (8 - gmBoss3MgrGetMgrWork(obj_work_parts).life) * 0.200000002980232));
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (gmsPlayerWork.obj_work.pos.y < obj_work_parts.pos.y)
        {
            if (obj_work_parts.spd.x > 0 && obj_work_parts.pos.x < gmsPlayerWork.obj_work.pos.x)
                num = FX_Mul(num, 8192);
            if (obj_work_parts.spd.x < 0 && gmsPlayerWork.obj_work.pos.x < obj_work_parts.pos.x)
                num = FX_Mul(num, 8192);
        }
        obj_work_parts.spd.x = FX_Mul(obj_work_parts.spd.x, num);
        obj_work_parts.spd.y = FX_Mul(obj_work_parts.spd.y, num);
        if (body_work.is_move != 0)
            return;
        obj_work_parts.spd.x = 0;
        obj_work_parts.spd.y = 0;
    }

    private static int gmBoss3BodyBattleCalcPattern(GMS_BOSS3_BODY_WORK body_work)
    {
        GMS_BOSS3_MGR_WORK mgrWork = gmBoss3MgrGetMgrWork(GMM_BS_OBJ(body_work));
        int num1 = mtMathRand() % 100;
        int num2 = 0;
        for (int index = 0; 7 > index; ++index)
        {
            num2 += g_gm_boss3_battle_pattern_per[mgrWork.life - 1][index];
            if (num1 < num2)
                return index;
        }
        return 0;
    }

    private static int gmBoss3BodyBattleInitMovePattern(
      GMS_BOSS3_BODY_WORK body_work,
      int pattern_no,
      int pos_index,
      int move_speed)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (g_gm_boss3_battle_move_x[pattern_no][pos_index] == 0)
            return 0;
        VecFx32 dest_pos = new VecFx32(obsObjectWork.pos.x + g_gm_boss3_battle_move_x[pattern_no][pos_index] * 4096, obsObjectWork.pos.y, obsObjectWork.pos.z);
        float frame = gmBoss3BodyCalcMoveXNormalFrame(body_work, dest_pos.x, move_speed);
        gmBoss3BodyInitMoveNormal(body_work, dest_pos, frame);
        return 1;
    }

    private static int gmBoss3BodyBattleCheckTurn(GMS_BOSS3_BODY_WORK body_work)
    {
        if (((int)GMM_BS_OBJ(body_work).disp_flag & 1) != 0)
        {
            if (body_work.end_pos.x <= body_work.start_pos.x)
                return 0;
        }
        else if (body_work.start_pos.x <= body_work.end_pos.x)
            return 0;
        return 1;
    }

    private static OBS_OBJECT_WORK gmBoss3BodyBattleSearchPillar()
    {
        OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, 3);
        while (obj_work != null && ((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id != 279)
            obj_work = ObjObjectSearchRegistObject(obj_work, 3);
        return obj_work;
    }

    private static void gmBoss3BodyDamage(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work_parts = GMM_BS_OBJ(body_work);
        if (((int)body_work.flag & 1) != 0)
            return;
        GMS_BOSS3_MGR_WORK mgrWork = gmBoss3MgrGetMgrWork(obj_work_parts);
        --mgrWork.life;
        if (mgrWork.life > 0)
            body_work.flag |= 1073741824U;
        else
            body_work.flag |= 2147483648U;
        GmSoundPlaySE("Boss0_01");
        gmBoss3EffDamageInit(body_work);
        GmPadVibSet(1, 30f, 8192, 8192, 0.0f, 0.0f, 0.0f, 8191U);
        gmBoss3BodySetInvincibleTime(body_work, 120U);
    }

    private static int gmBoss3BodyEscapeCheckScreenOut(GMS_BOSS3_BODY_WORK body_work)
    {
        return GMM_BS_OBJ(body_work).pos.x >= (g_gm_main_system.map_fcol.right + 64) * 4096 ? 1 : 0;
    }

    private static void gmBoss3BodyEscapeAddjustSpeed(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (MTM_MATH_ABS(obsObjectWork.spd.x) <= -1228)
            return;
        obsObjectWork.spd.x = 6144;
        obsObjectWork.spd.y = -1228;
        obsObjectWork.spd_add.x = 0;
        obsObjectWork.spd_add.y = 0;
    }

    private static void gmBoss3BodyChangeState(GMS_BOSS3_BODY_WORK body_work, int state)
    {
        GMF_BOSS3_BODY_STATE_FUNC bosS3BodyStateFunc1 = gm_boss3_body_state_func_tbl_leave[body_work.state];
        if (bosS3BodyStateFunc1 != null)
            bosS3BodyStateFunc1(body_work);
        body_work.prev_state = body_work.state;
        body_work.state = state;
        GMF_BOSS3_BODY_STATE_FUNC bosS3BodyStateFunc2 = gm_boss3_body_state_func_tbl_enter[body_work.state];
        if (bosS3BodyStateFunc2 == null)
            return;
        bosS3BodyStateFunc2(body_work);
    }

    private static void gmBoss3BodyStateStartEnter(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        obj_work.flag |= 2U;
        gmBoss3BodySetActionAllParts(body_work, 0, 1);
        gmBoss3BodySetDirectionNormal(body_work);
        body_work.proc_update = GmBsCmnIsFinalZoneType(obj_work) == 0 ? new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateStartUpdateWaitScrLimit) : new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateStartUpdateWait);
        obj_work.user_timer = 180;
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
    }

    private static void gmBoss3BodyStateStartLeave(GMS_BOSS3_BODY_WORK body_work)
    {
        GMM_BS_OBJ(body_work).flag &= 4294967293U;
        body_work.flag &= 4294967279U;
        body_work.ene_3d.ene_com.enemy_flag &= 4294934527U;
    }

    private static void gmBoss3BodyStateStartUpdateWaitScrLimit(GMS_BOSS3_BODY_WORK body_work)
    {
        if (((int)g_gm_main_system.game_flag & 32768) == 0)
            return;
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateStartUpdateWait);
    }

    private static void gmBoss3BodyStateStartUpdateWait(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss3BodySetDirectionNormal(body_work);
        if (obsObjectWork.user_timer > 0)
        {
            --obsObjectWork.user_timer;
        }
        else
        {
            gmBoss3BodySetActionAllParts(body_work, 1);
            body_work.is_move = 1;
            gmBoss3EffAfterburnerRequestCreate(body_work);
            obsObjectWork.disp_flag &= 4294967294U;
            gmBoss3BodyInitTurn(body_work, GMD_BOSS3_ANGLE_RIGHT, 60f, 1);
            body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateStartUpdateEnd);
        }
    }

    private static void gmBoss3BodyStateStartUpdateEnd(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        double num = gmBoss3BodyUpdateTurn(body_work);
        if (ObjViewOutCheck(obsObjectWork.pos.x, obsObjectWork.pos.y, 64, 0, 0, 0, 0) == 0)
            return;
        GmGmkCamScrLimitRelease(4);
        gmBoss3BodyChangeState(body_work, 2);
    }

    private static void gmBoss3BodyStateChaseMoveEnter(GMS_BOSS3_BODY_WORK body_work)
    {
        gmBoss3BodySetActionAllParts(body_work, 1);
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateChaseMoveUpdate);
        gmBoss3EffAfterburnerRequestCreate(body_work);
    }

    private static void gmBoss3BodyStateChaseMoveLeave(GMS_BOSS3_BODY_WORK body_work)
    {
        gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStateChaseMoveUpdate(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (gmBoss3BodyChaseCheckTurn(body_work) != 0)
        {
            short dest_angle;
            int flag_positive;
            if (((int)obsObjectWork.disp_flag & 1) != 0)
            {
                obsObjectWork.disp_flag &= 4294967294U;
                dest_angle = GMD_BOSS3_ANGLE_RIGHT;
                flag_positive = 1;
            }
            else
            {
                obsObjectWork.disp_flag |= 1U;
                dest_angle = GMD_BOSS3_ANGLE_LEFT;
                flag_positive = 0;
            }
            gmBoss3BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
        }
        double num = gmBoss3BodyUpdateTurn(body_work);
        if (obsObjectWork.user_flag == 0U)
            return;
        gmBoss3BodyChangeState(body_work, 3);
    }

    private static void gmBoss3BodyStatePreBattleEnter(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss3BodySetActionAllParts(body_work, 1);
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStatePreBattleUpdateStart);
        obsObjectWork.user_timer = 120;
        obsObjectWork.ppMove = new MPP_VOID_OBS_OBJECT_WORK(ObjObjectMove);
        gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStatePreBattleLeave(GMS_BOSS3_BODY_WORK body_work)
    {
        gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStatePreBattleUpdateStart(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (ObjViewOutCheck(obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0) != 0)
            return;
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            if (_am_draw_video.wide_screen)
            {
                GmCameraScaleSet(0.85f, 0.0025f);
                GmMapSetDrawMarginMag();
            }
            obj_work.disp_flag |= 1U;
            gmBoss3BodyInitTurn(body_work, GMD_BOSS3_ANGLE_LEFT, 60f, 0);
            gmBoss3BodySetActionAllParts(body_work, 1);
            body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStatePreBattleUpdateTurn);
            if (GmBsCmnIsFinalZoneType(obj_work) != 0)
                return;
            GmSoundChangeAngryBossBGM();
        }
    }

    private static void gmBoss3BodyStatePreBattleUpdateTurn(GMS_BOSS3_BODY_WORK body_work)
    {
        if (gmBoss3BodyUpdateTurn(body_work) > 0.0)
            return;
        gmBoss3BodySetActionAllParts(body_work, 2);
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStatePreBattleUpdateLaugh);
        OBS_OBJECT_WORK obj_work = gmBoss3BodyBattleSearchPillar();
        if (obj_work != null)
            GmGmkBoss3PillarWallChangeModeActive(obj_work);
        GmMapSetMapDrawSize(5);
        GmWaterSurfaceSetFlagDraw(false);
    }

    private static void gmBoss3BodyStatePreBattleUpdateLaugh(GMS_BOSS3_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        OBS_OBJECT_WORK obj_work = gmBoss3BodyBattleSearchPillar();
        if (obj_work != null)
            GmGmkBoss3PillarWallClearFlagNoPressDie(obj_work);
        gmBoss3BodyChangeState(body_work, 4);
        GmCameraScaleSet(1f, 0.0025f);
        GmMapSetDrawMarginNormal();
    }

    private static void gmBoss3BodyStateBattleEnter(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss3BodySetActionAllParts(body_work, 1);
        int num = g_gm_main_system.map_fcol.right - g_gm_main_system.map_fcol.left;
        VecFx32 dest_pos = new VecFx32((g_gm_main_system.map_fcol.left + num / 2) * 4096, obsObjectWork.pos.y, obsObjectWork.pos.z);
        float frame = gmBoss3BodyCalcMoveXNormalFrame(body_work, dest_pos.x, 4096);
        gmBoss3BodyInitMoveNormal(body_work, dest_pos, frame);
        if (gmBoss3BodyBattleCheckTurn(body_work) != 0)
        {
            short dest_angle;
            int flag_positive;
            if (((int)obsObjectWork.disp_flag & 1) != 0)
            {
                obsObjectWork.disp_flag &= 4294967294U;
                dest_angle = GMD_BOSS3_ANGLE_RIGHT;
                flag_positive = 1;
            }
            else
            {
                obsObjectWork.disp_flag |= 1U;
                dest_angle = GMD_BOSS3_ANGLE_LEFT;
                flag_positive = 0;
            }
            gmBoss3BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
        }
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateBattleUpdateMoveCenter);
        gmBoss3EffAfterburnerRequestCreate(body_work);
    }

    private static void gmBoss3BodyStateBattleLeave(GMS_BOSS3_BODY_WORK body_work)
    {
        gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStateBattleUpdateMoveCenter(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        float num1 = gmBoss3BodyUpdateTurn(body_work);
        float num2 = gmBoss3BodyUpdateMoveNormal(body_work);
        if (num1 > 0.0 || num2 > 0.0)
            return;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            gmBoss3BodySetActionAllParts(body_work, 3);
        else
            gmBoss3BodySetActionAllParts(body_work, 4);
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateBattleUpdateSearch);
        gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStateBattleUpdateSearch(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        body_work.pattern_no = gmBoss3BodyBattleCalcPattern(body_work);
        if (gmBoss3BodyBattleInitMovePattern(body_work, body_work.pattern_no, 0, 4096) != 0)
        {
            if (gmBoss3BodyBattleCheckTurn(body_work) != 0)
            {
                short dest_angle;
                int flag_positive;
                if (((int)obj_work.disp_flag & 1) != 0)
                {
                    obj_work.disp_flag &= 4294967294U;
                    dest_angle = GMD_BOSS3_ANGLE_RIGHT;
                    flag_positive = 1;
                }
                else
                {
                    obj_work.disp_flag |= 1U;
                    dest_angle = GMD_BOSS3_ANGLE_LEFT;
                    flag_positive = 0;
                }
                gmBoss3BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
            }
            body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateBattleUpdateMoveFirst);
            gmBoss3EffAfterburnerRequestCreate(body_work);
            gmBoss3BodySetActionAllParts(body_work, 1);
        }
        else
        {
            gmBoss3BodySetActionAllParts(body_work, 5);
            body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateBattleUpdateSign);
        }
    }

    private static void gmBoss3BodyStateBattleUpdateMoveFirst(GMS_BOSS3_BODY_WORK body_work)
    {
        float num1 = gmBoss3BodyUpdateTurn(body_work);
        float num2 = gmBoss3BodyUpdateMoveNormal(body_work);
        if (num1 > 0.0 || num2 > 0.0)
            return;
        gmBoss3BodySetActionAllParts(body_work, 5);
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateBattleUpdateSign);
        gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStateBattleUpdateSign(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work1 = GMM_BS_OBJ(body_work);
        if (GmBsCmnIsActionEnd(obj_work1) == 0)
            return;
        OBS_OBJECT_WORK obj_work2 = gmBoss3BodyBattleSearchPillar();
        if (obj_work2 != null)
            GmGmkBoss3PillarChangeModeActive(obj_work2, body_work.pattern_no);
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateBattleUpdateWaitPillar);
        obj_work1.user_timer = GmBsCmnIsFinalZoneType(obj_work1) == 0 ? 240 : 150;
        if (((int)obj_work1.disp_flag & 1) != 0)
            gmBoss3BodySetActionAllParts(body_work, 6);
        else
            gmBoss3BodySetActionAllParts(body_work, 7);
    }

    private static void gmBoss3BodyStateBattleUpdateWaitPillar(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            int move_speed = 4096;
            GmBsCmnIsFinalZoneType(obj_work);
            if (gmBoss3BodyBattleInitMovePattern(body_work, body_work.pattern_no, 1, move_speed) != 0)
            {
                if (gmBoss3BodyBattleCheckTurn(body_work) != 0)
                {
                    short dest_angle;
                    int flag_positive;
                    if (((int)obj_work.disp_flag & 1) != 0)
                    {
                        obj_work.disp_flag &= 4294967294U;
                        dest_angle = GMD_BOSS3_ANGLE_RIGHT;
                        flag_positive = 1;
                    }
                    else
                    {
                        obj_work.disp_flag |= 1U;
                        dest_angle = GMD_BOSS3_ANGLE_LEFT;
                        flag_positive = 0;
                    }
                    gmBoss3BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
                }
                gmBoss3EffAfterburnerRequestCreate(body_work);
                body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateBattleUpdateMoveSecond);
                gmBoss3BodySetActionAllParts(body_work, 1);
            }
            else
            {
                body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateBattleUpdateWaitActive);
                obj_work.user_timer = GmGmkBoss3PillarGetActiveTime(body_work.pattern_no) - 240 + 120;
                if (((int)obj_work.disp_flag & 1) != 0)
                    gmBoss3BodySetActionAllParts(body_work, 6);
                else
                    gmBoss3BodySetActionAllParts(body_work, 7);
            }
        }
    }

    private static void gmBoss3BodyStateBattleUpdateMoveSecond(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        float num1 = gmBoss3BodyUpdateTurn(body_work);
        float num2 = gmBoss3BodyUpdateMoveNormal(body_work);
        if (num1 > 0.0 || num2 > 0.0)
            return;
        if (((int)obj_work.disp_flag & 1) != 0)
            gmBoss3BodySetActionAllParts(body_work, 6);
        else
            gmBoss3BodySetActionAllParts(body_work, 7);
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateBattleUpdateWaitActive);
        int num3 = 240;
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
            num3 = 150;
        obj_work.user_timer = GmGmkBoss3PillarGetActiveTime(body_work.pattern_no) - num3 - (int)body_work.move_frame + 120;
        gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStateBattleUpdateWaitActive(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (obsObjectWork.user_timer > 0)
        {
            --obsObjectWork.user_timer;
        }
        else
        {
            body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateBattleUpdateWaitReturn);
            obsObjectWork.user_timer = 30;
            OBS_OBJECT_WORK obj_work = gmBoss3BodyBattleSearchPillar();
            if (obj_work == null)
                return;
            GmGmkBoss3PillarChangeModeReturn(obj_work);
        }
    }

    private static void gmBoss3BodyStateBattleUpdateWaitReturn(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (obsObjectWork.user_timer > 0)
        {
            --obsObjectWork.user_timer;
        }
        else
        {
            gmBoss3BodyChangeState(body_work, 4);
            OBS_OBJECT_WORK obj_work = gmBoss3BodyBattleSearchPillar();
            if (obj_work == null)
                return;
            GmGmkBoss3PillarChangeModeDelete(obj_work);
        }
    }

    private static void gmBoss3BodyStateDefeatEnter(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work1 = GMM_BS_OBJ(body_work);
        obj_work1.flag |= 2U;
        obj_work1.disp_flag |= 16U;
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
        GmBsCmnSetObjSpdZero(obj_work1);
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateDefeatUpdateStart);
        obj_work1.user_timer = 40;
        OBS_OBJECT_WORK obj_work2 = gmBoss3BodyBattleSearchPillar();
        if (obj_work2 != null)
            GmGmkBoss3PillarChangeModeReturn(obj_work2);
        GmSoundChangeWinBossBGM();
    }

    private static void gmBoss3BodyStateDefeatLeave(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.disp_flag &= 4294967279U;
        obsObjectWork.flag &= 4294967293U;
        OBS_OBJECT_WORK obj_work = gmBoss3BodyBattleSearchPillar();
        if (obj_work == null)
            return;
        GmGmkBoss3PillarChangeModeDelete(obj_work);
    }

    private static void gmBoss3BodyStateDefeatUpdateStart(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (obsObjectWork.user_timer > 0)
        {
            --obsObjectWork.user_timer;
        }
        else
        {
            OBS_OBJECT_WORK parent_obj = GMM_BS_OBJ(body_work);
            gmBoss3EffBombsInit(body_work.bomb_work, parent_obj, parent_obj.pos.x, parent_obj.pos.y, 327680, 327680, 10U, 30U);
            body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateDefeatUpdateFall);
            obsObjectWork.user_timer = 120;
        }
    }

    private static void gmBoss3BodyStateDefeatUpdateFall(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (obsObjectWork.user_timer > 0)
        {
            --obsObjectWork.user_timer;
            gmBoss3EffBombsUpdate(body_work.bomb_work);
        }
        else
            body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateDefeatUpdateExplode);
    }

    private static void gmBoss3BodyStateDefeatUpdateExplode(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        body_work.flag |= 134217728U;
        GmSoundPlaySE("Boss0_03");
        GMM_PAD_VIB_MID_TIME(120f);
        GmBsCmnInitFlashScreen(body_work.flash_work, 4f, 5f, 30f);
        OBS_OBJECT_WORK parent_obj = GMM_BS_OBJ(body_work);
        ((OBS_OBJECT_WORK)GmEfctCmnEsCreate(parent_obj, 8)).pos.z = parent_obj.pos.z + 131072;
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateDefeatUpdateScatter);
        GmPlayerAddScoreNoDisp((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), 1000);
        obsObjectWork.user_timer = 40;
    }

    private static void gmBoss3BodyStateDefeatUpdateScatter(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBsCmnUpdateFlashScreen(body_work.flash_work);
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            gmBoss3ChangeTextureBurnt(obj_work);
            body_work.flag |= 16777216U;
            gmBoss3EffAfterburnerSmokeInit(body_work);
            gmBoss3EffBodySmokeInit(body_work);
            body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateDefeatUpdateEnd);
            obj_work.user_timer = 120;
        }
    }

    private static void gmBoss3BodyStateDefeatUpdateEnd(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (obsObjectWork.user_timer > 0)
            --obsObjectWork.user_timer;
        else
            gmBoss3BodyChangeState(body_work, 6);
    }

    private static void gmBoss3BodyStateEscapeEnter(GMS_BOSS3_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        obj_work.spd_add.x = 327;
        obj_work.spd_add.y = 40;
        if (((int)obj_work.disp_flag & 1) != 0)
        {
            obj_work.disp_flag &= 4294967294U;
            gmBoss3BodyInitTurn(body_work, GMD_BOSS3_ANGLE_RIGHT, 60f, 1);
        }
        obj_work.flag |= 2U;
        obj_work.move_flag |= 4352U;
        gmBoss3BodySetDirectionNormal(body_work);
        gmBoss3BodySetActionAllParts(body_work, 8, 1);
        body_work.flag |= 8388608U;
        GmMapSetMapDrawSize(1);
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
            body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateEscapeUpdateFinalZone);
        else
            body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateEscapeUpdateScrollLock);
    }

    private static void gmBoss3BodyStateEscapeLeave(GMS_BOSS3_BODY_WORK body_work)
    {
        GMM_BS_OBJ(body_work).flag &= 4294967293U;
    }

    private static void gmBoss3BodyStateEscapeUpdateScrollLock(GMS_BOSS3_BODY_WORK body_work)
    {
        gmBoss3BodyEscapeAddjustSpeed(body_work);
        if (gmBoss3BodyUpdateTurn(body_work) > 0.0)
            return;
        GmGmkCamScrLimitRelease(4);
        OBS_OBJECT_WORK obj_work = gmBoss3BodyBattleSearchPillar();
        if (obj_work != null)
            GmGmkBoss3PillarWallChangeModeReturn(obj_work);
        GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 1U);
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateEscapeUpdateWaitScreenOut);
    }

    private static void gmBoss3BodyStateEscapeUpdateWaitScreenOut(
      GMS_BOSS3_BODY_WORK body_work)
    {
        gmBoss3BodyEscapeAddjustSpeed(body_work);
        if (gmBoss3BodyEscapeCheckScreenOut(body_work) == 0)
            return;
        gmBoss3MgrGetMgrWork(GMM_BS_OBJ(body_work)).flag |= 2U;
        body_work.proc_update = null;
    }

    private static void gmBoss3BodyStateEscapeUpdateFinalZone(GMS_BOSS3_BODY_WORK body_work)
    {
        gmBoss3BodyEscapeAddjustSpeed(body_work);
        if (gmBoss3BodyUpdateTurn(body_work) > 0.0)
            return;
        GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 1U);
        body_work.proc_update = new GMF_BOSS3_BODY_STATE_FUNC(gmBoss3BodyStateEscapeUpdateWaitScreenOut);
    }

    private static void gmBoss3BodyMainFuncWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS3_BODY_WORK body_work = (GMS_BOSS3_BODY_WORK)obj_work;
        if (gmBoss3MgrCheckSetupComplete(gmBoss3MgrGetMgrWork(obj_work)) == 0)
            return;
        GmBsCmnInitBossMotionCBSystem(obj_work, body_work.bmcb_mgr);
        GmBsCmnCreateSNMWork(body_work.snm_work, obj_work.obj_3d._object, 1);
        GmBsCmnAppendBossMotionCallback(body_work.bmcb_mgr, body_work.snm_work.bmcb_link);
        for (int index = 0; 1 > index; ++index)
            body_work.snm_reg_id[index] = GmBsCmnRegisterSNMNode(body_work.snm_work, g_boss3_node_index_list[index]);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3BodyMainFunc);
        gmBoss3BodyChangeState(body_work, 1);
    }

    private static void gmBoss3BodyMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS3_BODY_WORK body_work = (GMS_BOSS3_BODY_WORK)obj_work;
        gmBoss3BodyUpdateNoHitTime(body_work);
        gmBoss3BodyUpdateInvincibleTime(body_work);
        if (body_work.proc_update != null)
            body_work.proc_update(body_work);
        if (((int)body_work.flag & 33554432) != 0)
            gmBoss3EffAfterburnerInit(body_work);
        if (((int)body_work.flag & int.MinValue) != 0)
        {
            body_work.flag &= 1073741823U;
            gmBoss3BodyChangeState(body_work, 5);
        }
        else
        {
            if (((int)body_work.flag & 1073741824) != 0)
            {
                body_work.flag &= 3221225471U;
                body_work.flag |= 536870912U;
                GmBsCmnInitObject3DNNDamageFlicker(obj_work, body_work.flk_work, 32f);
            }
            GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, body_work.flk_work);
            gmBoss3BodyUpdateDirection(body_work);
        }
    }

}