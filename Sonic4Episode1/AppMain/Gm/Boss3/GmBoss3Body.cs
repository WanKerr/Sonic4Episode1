using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static void gmBoss3BodyExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_BOSS3_BODY_WORK tcbWork = (AppMain.GMS_BOSS3_BODY_WORK)AppMain.mtTaskGetTcbWork(tcb);
        AppMain.OBS_OBJECT_WORK objWork = tcbWork.ene_3d.ene_com.obj_work;
        AppMain.GmBsCmnClearBossMotionCBSystem(objWork);
        AppMain.GmBsCmnDeleteSNMWork(tcbWork.snm_work);
        AppMain.GmBsCmnClearCNMCb(objWork);
        AppMain.gmBoss3ExitFunc(tcb);
    }

    private static void gmBoss3BodyReactionPlayer(
      AppMain.OBS_OBJECT_WORK obj_work_player,
      AppMain.OBS_OBJECT_WORK obj_work_body)
    {
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)obj_work_player;
        AppMain.GmPlySeqAtkReactionInit(ply_work);
        AppMain.GmPlySeqSetJumpState(ply_work, 0, 5U);
        int num1 = 2048;
        int num2 = 1228;
        if (ply_work.seq_state == 20)
        {
            num1 = 12288;
            num2 = 12288;
        }
        obj_work_player.spd_m = 0;
        obj_work_player.spd.x = obj_work_player.move.x < 0 ? num1 : -num1;
        obj_work_player.spd.y = obj_work_player.pos.y > obj_work_body.pos.y ? num2 : -num2;
        AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, 102400);
    }

    private static void gmBoss3BodySetRectNormal(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], (short)-8, (short)-8, (short)8, (short)8);
        body_work.ene_3d.ene_com.rect_work[0].flag |= 4U;
    }

    private static void gmBoss3BodySetActionAllParts(
      AppMain.GMS_BOSS3_BODY_WORK body_work,
      int action_id)
    {
        AppMain.gmBoss3BodySetActionAllParts(body_work, action_id, 0);
    }

    private static void gmBoss3BodySetActionAllParts(
      AppMain.GMS_BOSS3_BODY_WORK body_work,
      int action_id,
      int force_change)
    {
        if (force_change == 0 && body_work.action_id == action_id)
            return;
        body_work.action_id = action_id;
        for (int index = 0; 2 > index; ++index)
        {
            AppMain.OBS_OBJECT_WORK partsObj = body_work.parts_objs[index];
            if (partsObj != null)
            {
                AppMain.GMS_BOSS3_PART_ACT_INFO bosS3PartActInfo = AppMain.gm_boss3_act_info_tbl[action_id][index];
                if (index != 1 || ((int)((AppMain.GMS_BOSS3_EGG_WORK)partsObj).flag & 1) == 0)
                {
                    if (bosS3PartActInfo.is_maintain != (byte)0)
                    {
                        if (bosS3PartActInfo.is_repeat != (byte)0)
                            partsObj.disp_flag |= 4U;
                    }
                    else
                        AppMain.GmBsCmnSetAction(partsObj, (int)bosS3PartActInfo.mtn_id, (int)bosS3PartActInfo.is_repeat, bosS3PartActInfo.is_blend);
                    partsObj.obj_3d.speed[0] = bosS3PartActInfo.mtn_spd;
                    partsObj.obj_3d.blend_spd = bosS3PartActInfo.blend_spd;
                }
            }
        }
    }

    private static void gmBoss3BodyOutFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void gmBoss3BodyChaseMoveFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int x = obj_work.spd.x;
        int y = obj_work.spd.y;
        AppMain.gmBoss3BodyChaseAdjustMoveSpeed((AppMain.GMS_BOSS3_BODY_WORK)obj_work);
        AppMain.ObjObjectMove(obj_work);
        obj_work.spd.x = x;
        obj_work.spd.y = y;
    }

    private static void gmBoss3BodyDefFunc(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = target_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = own_rect.parent_obj;
        AppMain.GMS_BOSS3_BODY_WORK body_work = (AppMain.GMS_BOSS3_BODY_WORK)parentObj2;
        if (parentObj1 == null || (ushort)1 != parentObj1.obj_type)
            return;
        AppMain.gmBoss3BodyReactionPlayer(parentObj1, parentObj2);
        AppMain.gmBoss3BodySetNoHitTime(body_work, 10U);
        AppMain.gmBoss3BodyDamage(body_work);
    }

    private static void gmBoss3BodySetNoHitTime(AppMain.GMS_BOSS3_BODY_WORK body_work, uint time)
    {
        AppMain.GMS_ENEMY_COM_WORK eneCom = body_work.ene_3d.ene_com;
        body_work.counter_no_hit = time;
        eneCom.rect_work[0].flag |= 2048U;
    }

    private static void gmBoss3BodyUpdateNoHitTime(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        if (body_work.counter_no_hit > 0U)
            --body_work.counter_no_hit;
        else
            body_work.ene_3d.ene_com.rect_work[0].flag &= 4294965247U;
    }

    private static void gmBoss3BodySetInvincibleTime(AppMain.GMS_BOSS3_BODY_WORK body_work, uint time)
    {
        body_work.counter_invincible = time;
        body_work.flag |= 1U;
    }

    private static void gmBoss3BodyUpdateInvincibleTime(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        if (body_work.counter_invincible > 0U)
            --body_work.counter_invincible;
        else
            body_work.flag &= 4294967294U;
    }

    private static void gmBoss3BodySetDirection(AppMain.GMS_BOSS3_BODY_WORK body_work, short deg)
    {
        body_work.angle_current = deg;
    }

    private static void gmBoss3BodySetDirectionNormal(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) != 0)
            AppMain.gmBoss3BodySetDirection(body_work, AppMain.GMD_BOSS3_ANGLE_LEFT);
        else
            AppMain.gmBoss3BodySetDirection(body_work, AppMain.GMD_BOSS3_ANGLE_RIGHT);
    }

    private static void gmBoss3BodyUpdateDirection(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)body_work).dir.y = (ushort)body_work.angle_current;
    }

    private static float gmBoss3BodyCalcMoveXNormalFrame(
      AppMain.GMS_BOSS3_BODY_WORK body_work,
      int x,
      int speed)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        return (float)AppMain.MTM_MATH_ABS(x - obsObjectWork.pos.x) / (float)speed;
    }

    private static void gmBoss3BodyInitMoveNormal(
      AppMain.GMS_BOSS3_BODY_WORK body_work,
      AppMain.VecFx32 dest_pos,
      float frame)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        body_work.start_pos.x = obsObjectWork.pos.x;
        body_work.start_pos.y = obsObjectWork.pos.y;
        body_work.start_pos.z = obsObjectWork.pos.z;
        body_work.end_pos.x = dest_pos.x;
        body_work.end_pos.y = dest_pos.y;
        body_work.end_pos.z = body_work.start_pos.z;
        body_work.move_counter = 0.0f;
        if ((double)frame > 0.0)
            body_work.move_frame = frame;
        else
            body_work.move_frame = 1f;
    }

    private static float gmBoss3BodyUpdateMoveNormal(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.VecFx32 vecFx32_1 = new AppMain.VecFx32();
        ++body_work.move_counter;
        if ((double)body_work.move_counter >= (double)body_work.move_frame)
        {
            vecFx32_1.x = body_work.end_pos.x;
            vecFx32_1.y = body_work.end_pos.y;
            vecFx32_1.z = body_work.end_pos.z;
        }
        else
        {
            float num = (float)(0.5 * (1.0 - (double)AppMain.nnCos(AppMain.AKM_DEGtoA32(180f * (body_work.move_counter / body_work.move_frame)))));
            AppMain.VecFx32 vecFx32_2 = new AppMain.VecFx32();
            vecFx32_2.x = (int)((double)(body_work.end_pos.x - body_work.start_pos.x) * (double)num);
            vecFx32_2.y = (int)((double)(body_work.end_pos.y - body_work.start_pos.y) * (double)num);
            vecFx32_2.z = (int)((double)(body_work.end_pos.z - body_work.start_pos.z) * (double)num);
            vecFx32_1.x = body_work.start_pos.x + vecFx32_2.x;
            vecFx32_1.y = body_work.start_pos.y + vecFx32_2.y;
            vecFx32_1.z = body_work.start_pos.z + vecFx32_2.z;
        }
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.pos.x = vecFx32_1.x;
        obsObjectWork.pos.y = vecFx32_1.y;
        obsObjectWork.pos.z = vecFx32_1.z;
        return body_work.move_frame - body_work.move_counter;
    }

    private static void gmBoss3BodyInitTurn(
      AppMain.GMS_BOSS3_BODY_WORK body_work,
      short dest_angle,
      float frame,
      int flag_positive)
    {
        body_work.turn_counter = 0.0f;
        body_work.turn_frame = frame;
        body_work.turn_start = body_work.angle_current;
        ushort num1 = (ushort)((uint)dest_angle - (uint)body_work.angle_current);
        body_work.turn_amount = (int)num1;
        if (flag_positive != 0)
            return;
        ushort num2 = (ushort)(body_work.turn_amount - AppMain.AKM_DEGtoA32(360));
        body_work.turn_amount = (int)num2 - AppMain.AKM_DEGtoA32(360);
    }

    private static float gmBoss3BodyUpdateTurn(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        if ((double)body_work.turn_frame < 1.0)
            return 0.0f;
        ++body_work.turn_counter;
        short deg;
        if ((double)body_work.turn_counter >= (double)body_work.turn_frame)
        {
            deg = (short)((int)body_work.turn_start + body_work.turn_amount);
        }
        else
        {
            int ang = AppMain.AKM_DEGtoA32(180f * (body_work.turn_counter / body_work.turn_frame));
            float num = (float)((double)body_work.turn_amount * 0.5 * (1.0 - (double)AppMain.nnCos(ang)));
            deg = (short)((double)body_work.turn_start + (double)num);
        }
        AppMain.gmBoss3BodySetDirection(body_work, deg);
        return body_work.turn_frame - body_work.turn_counter;
    }

    private static int gmBoss3BodyChaseCheckTurn(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.spd.x < 0)
                return 0;
        }
        else if (obsObjectWork.spd.x > 0)
            return 0;
        return 1;
    }

    private static void gmBoss3BodyChaseAdjustMoveSpeed(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work_parts = AppMain.GMM_BS_OBJ((object)body_work);
        int num = AppMain.FX_F32_TO_FX32((float)(1.0 + (double)(8 - AppMain.gmBoss3MgrGetMgrWork(obj_work_parts).life) * 0.200000002980232));
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (gmsPlayerWork.obj_work.pos.y < obj_work_parts.pos.y)
        {
            if (obj_work_parts.spd.x > 0 && obj_work_parts.pos.x < gmsPlayerWork.obj_work.pos.x)
                num = AppMain.FX_Mul(num, 8192);
            if (obj_work_parts.spd.x < 0 && gmsPlayerWork.obj_work.pos.x < obj_work_parts.pos.x)
                num = AppMain.FX_Mul(num, 8192);
        }
        obj_work_parts.spd.x = AppMain.FX_Mul(obj_work_parts.spd.x, num);
        obj_work_parts.spd.y = AppMain.FX_Mul(obj_work_parts.spd.y, num);
        if (body_work.is_move != 0)
            return;
        obj_work_parts.spd.x = 0;
        obj_work_parts.spd.y = 0;
    }

    private static int gmBoss3BodyBattleCalcPattern(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.GMS_BOSS3_MGR_WORK mgrWork = AppMain.gmBoss3MgrGetMgrWork(AppMain.GMM_BS_OBJ((object)body_work));
        int num1 = (int)AppMain.mtMathRand() % 100;
        int num2 = 0;
        for (int index = 0; 7 > index; ++index)
        {
            num2 += AppMain.g_gm_boss3_battle_pattern_per[mgrWork.life - 1][index];
            if (num1 < num2)
                return index;
        }
        return 0;
    }

    private static int gmBoss3BodyBattleInitMovePattern(
      AppMain.GMS_BOSS3_BODY_WORK body_work,
      int pattern_no,
      int pos_index,
      int move_speed)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.g_gm_boss3_battle_move_x[pattern_no][pos_index] == 0)
            return 0;
        AppMain.VecFx32 dest_pos = new AppMain.VecFx32(obsObjectWork.pos.x + AppMain.g_gm_boss3_battle_move_x[pattern_no][pos_index] * 4096, obsObjectWork.pos.y, obsObjectWork.pos.z);
        float frame = AppMain.gmBoss3BodyCalcMoveXNormalFrame(body_work, dest_pos.x, move_speed);
        AppMain.gmBoss3BodyInitMoveNormal(body_work, dest_pos, frame);
        return 1;
    }

    private static int gmBoss3BodyBattleCheckTurn(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) != 0)
        {
            if (body_work.end_pos.x <= body_work.start_pos.x)
                return 0;
        }
        else if (body_work.start_pos.x <= body_work.end_pos.x)
            return 0;
        return 1;
    }

    private static AppMain.OBS_OBJECT_WORK gmBoss3BodyBattleSearchPillar()
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)3);
        while (obj_work != null && ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id != (ushort)279)
            obj_work = AppMain.ObjObjectSearchRegistObject(obj_work, (ushort)3);
        return obj_work;
    }

    private static void gmBoss3BodyDamage(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work_parts = AppMain.GMM_BS_OBJ((object)body_work);
        if (((int)body_work.flag & 1) != 0)
            return;
        AppMain.GMS_BOSS3_MGR_WORK mgrWork = AppMain.gmBoss3MgrGetMgrWork(obj_work_parts);
        --mgrWork.life;
        if (mgrWork.life > 0)
            body_work.flag |= 1073741824U;
        else
            body_work.flag |= 2147483648U;
        AppMain.GmSoundPlaySE("Boss0_01");
        AppMain.gmBoss3EffDamageInit(body_work);
        AppMain.GmPadVibSet(1, 30f, (ushort)8192, (ushort)8192, 0.0f, 0.0f, 0.0f, 8191U);
        AppMain.gmBoss3BodySetInvincibleTime(body_work, 120U);
    }

    private static int gmBoss3BodyEscapeCheckScreenOut(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        return AppMain.GMM_BS_OBJ((object)body_work).pos.x >= (AppMain.g_gm_main_system.map_fcol.right + 64) * 4096 ? 1 : 0;
    }

    private static void gmBoss3BodyEscapeAddjustSpeed(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.MTM_MATH_ABS(obsObjectWork.spd.x) <= -1228)
            return;
        obsObjectWork.spd.x = 6144;
        obsObjectWork.spd.y = -1228;
        obsObjectWork.spd_add.x = 0;
        obsObjectWork.spd_add.y = 0;
    }

    private static void gmBoss3BodyChangeState(AppMain.GMS_BOSS3_BODY_WORK body_work, int state)
    {
        AppMain.GMF_BOSS3_BODY_STATE_FUNC bosS3BodyStateFunc1 = AppMain.gm_boss3_body_state_func_tbl_leave[body_work.state];
        if (bosS3BodyStateFunc1 != null)
            bosS3BodyStateFunc1(body_work);
        body_work.prev_state = body_work.state;
        body_work.state = state;
        AppMain.GMF_BOSS3_BODY_STATE_FUNC bosS3BodyStateFunc2 = AppMain.gm_boss3_body_state_func_tbl_enter[body_work.state];
        if (bosS3BodyStateFunc2 == null)
            return;
        bosS3BodyStateFunc2(body_work);
    }

    private static void gmBoss3BodyStateStartEnter(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        obj_work.flag |= 2U;
        AppMain.gmBoss3BodySetActionAllParts(body_work, 0, 1);
        AppMain.gmBoss3BodySetDirectionNormal(body_work);
        body_work.proc_update = AppMain.GmBsCmnIsFinalZoneType(obj_work) == 0 ? new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateStartUpdateWaitScrLimit) : new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateStartUpdateWait);
        obj_work.user_timer = 180;
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
    }

    private static void gmBoss3BodyStateStartLeave(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)body_work).flag &= 4294967293U;
        body_work.flag &= 4294967279U;
        body_work.ene_3d.ene_com.enemy_flag &= 4294934527U;
    }

    private static void gmBoss3BodyStateStartUpdateWaitScrLimit(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        if (((int)AppMain.g_gm_main_system.game_flag & 32768) == 0)
            return;
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateStartUpdateWait);
    }

    private static void gmBoss3BodyStateStartUpdateWait(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss3BodySetDirectionNormal(body_work);
        if (obsObjectWork.user_timer > 0)
        {
            --obsObjectWork.user_timer;
        }
        else
        {
            AppMain.gmBoss3BodySetActionAllParts(body_work, 1);
            body_work.is_move = 1;
            AppMain.gmBoss3EffAfterburnerRequestCreate(body_work);
            obsObjectWork.disp_flag &= 4294967294U;
            AppMain.gmBoss3BodyInitTurn(body_work, AppMain.GMD_BOSS3_ANGLE_RIGHT, 60f, 1);
            body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateStartUpdateEnd);
        }
    }

    private static void gmBoss3BodyStateStartUpdateEnd(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        double num = (double)AppMain.gmBoss3BodyUpdateTurn(body_work);
        if (AppMain.ObjViewOutCheck(obsObjectWork.pos.x, obsObjectWork.pos.y, (short)64, (short)0, (short)0, (short)0, (short)0) == 0)
            return;
        AppMain.GmGmkCamScrLimitRelease((byte)4);
        AppMain.gmBoss3BodyChangeState(body_work, 2);
    }

    private static void gmBoss3BodyStateChaseMoveEnter(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.gmBoss3BodySetActionAllParts(body_work, 1);
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateChaseMoveUpdate);
        AppMain.gmBoss3EffAfterburnerRequestCreate(body_work);
    }

    private static void gmBoss3BodyStateChaseMoveLeave(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStateChaseMoveUpdate(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.gmBoss3BodyChaseCheckTurn(body_work) != 0)
        {
            short dest_angle;
            int flag_positive;
            if (((int)obsObjectWork.disp_flag & 1) != 0)
            {
                obsObjectWork.disp_flag &= 4294967294U;
                dest_angle = AppMain.GMD_BOSS3_ANGLE_RIGHT;
                flag_positive = 1;
            }
            else
            {
                obsObjectWork.disp_flag |= 1U;
                dest_angle = AppMain.GMD_BOSS3_ANGLE_LEFT;
                flag_positive = 0;
            }
            AppMain.gmBoss3BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
        }
        double num = (double)AppMain.gmBoss3BodyUpdateTurn(body_work);
        if (obsObjectWork.user_flag == 0U)
            return;
        AppMain.gmBoss3BodyChangeState(body_work, 3);
    }

    private static void gmBoss3BodyStatePreBattleEnter(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss3BodySetActionAllParts(body_work, 1);
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStatePreBattleUpdateStart);
        obsObjectWork.user_timer = 120;
        obsObjectWork.ppMove = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.ObjObjectMove);
        AppMain.gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStatePreBattleLeave(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStatePreBattleUpdateStart(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.ObjViewOutCheck(obj_work.pos.x, obj_work.pos.y, (short)0, (short)0, (short)0, (short)0, (short)0) != 0)
            return;
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            if (AppMain._am_draw_video.wide_screen)
            {
                AppMain.GmCameraScaleSet(0.85f, 0.0025f);
                AppMain.GmMapSetDrawMarginMag();
            }
            obj_work.disp_flag |= 1U;
            AppMain.gmBoss3BodyInitTurn(body_work, AppMain.GMD_BOSS3_ANGLE_LEFT, 60f, 0);
            AppMain.gmBoss3BodySetActionAllParts(body_work, 1);
            body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStatePreBattleUpdateTurn);
            if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
                return;
            AppMain.GmSoundChangeAngryBossBGM();
        }
    }

    private static void gmBoss3BodyStatePreBattleUpdateTurn(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        if ((double)AppMain.gmBoss3BodyUpdateTurn(body_work) > 0.0)
            return;
        AppMain.gmBoss3BodySetActionAllParts(body_work, 2);
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStatePreBattleUpdateLaugh);
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmBoss3BodyBattleSearchPillar();
        if (obj_work != null)
            AppMain.GmGmkBoss3PillarWallChangeModeActive(obj_work);
        AppMain.GmMapSetMapDrawSize(5);
        AppMain.GmWaterSurfaceSetFlagDraw(false);
    }

    private static void gmBoss3BodyStatePreBattleUpdateLaugh(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmBoss3BodyBattleSearchPillar();
        if (obj_work != null)
            AppMain.GmGmkBoss3PillarWallClearFlagNoPressDie(obj_work);
        AppMain.gmBoss3BodyChangeState(body_work, 4);
        AppMain.GmCameraScaleSet(1f, 0.0025f);
        AppMain.GmMapSetDrawMarginNormal();
    }

    private static void gmBoss3BodyStateBattleEnter(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss3BodySetActionAllParts(body_work, 1);
        int num = AppMain.g_gm_main_system.map_fcol.right - AppMain.g_gm_main_system.map_fcol.left;
        AppMain.VecFx32 dest_pos = new AppMain.VecFx32((AppMain.g_gm_main_system.map_fcol.left + num / 2) * 4096, obsObjectWork.pos.y, obsObjectWork.pos.z);
        float frame = AppMain.gmBoss3BodyCalcMoveXNormalFrame(body_work, dest_pos.x, 4096);
        AppMain.gmBoss3BodyInitMoveNormal(body_work, dest_pos, frame);
        if (AppMain.gmBoss3BodyBattleCheckTurn(body_work) != 0)
        {
            short dest_angle;
            int flag_positive;
            if (((int)obsObjectWork.disp_flag & 1) != 0)
            {
                obsObjectWork.disp_flag &= 4294967294U;
                dest_angle = AppMain.GMD_BOSS3_ANGLE_RIGHT;
                flag_positive = 1;
            }
            else
            {
                obsObjectWork.disp_flag |= 1U;
                dest_angle = AppMain.GMD_BOSS3_ANGLE_LEFT;
                flag_positive = 0;
            }
            AppMain.gmBoss3BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
        }
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateBattleUpdateMoveCenter);
        AppMain.gmBoss3EffAfterburnerRequestCreate(body_work);
    }

    private static void gmBoss3BodyStateBattleLeave(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStateBattleUpdateMoveCenter(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        float num1 = AppMain.gmBoss3BodyUpdateTurn(body_work);
        float num2 = AppMain.gmBoss3BodyUpdateMoveNormal(body_work);
        if ((double)num1 > 0.0 || (double)num2 > 0.0)
            return;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            AppMain.gmBoss3BodySetActionAllParts(body_work, 3);
        else
            AppMain.gmBoss3BodySetActionAllParts(body_work, 4);
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateBattleUpdateSearch);
        AppMain.gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStateBattleUpdateSearch(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        body_work.pattern_no = AppMain.gmBoss3BodyBattleCalcPattern(body_work);
        if (AppMain.gmBoss3BodyBattleInitMovePattern(body_work, body_work.pattern_no, 0, 4096) != 0)
        {
            if (AppMain.gmBoss3BodyBattleCheckTurn(body_work) != 0)
            {
                short dest_angle;
                int flag_positive;
                if (((int)obj_work.disp_flag & 1) != 0)
                {
                    obj_work.disp_flag &= 4294967294U;
                    dest_angle = AppMain.GMD_BOSS3_ANGLE_RIGHT;
                    flag_positive = 1;
                }
                else
                {
                    obj_work.disp_flag |= 1U;
                    dest_angle = AppMain.GMD_BOSS3_ANGLE_LEFT;
                    flag_positive = 0;
                }
                AppMain.gmBoss3BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
            }
            body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateBattleUpdateMoveFirst);
            AppMain.gmBoss3EffAfterburnerRequestCreate(body_work);
            AppMain.gmBoss3BodySetActionAllParts(body_work, 1);
        }
        else
        {
            AppMain.gmBoss3BodySetActionAllParts(body_work, 5);
            body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateBattleUpdateSign);
        }
    }

    private static void gmBoss3BodyStateBattleUpdateMoveFirst(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        float num1 = AppMain.gmBoss3BodyUpdateTurn(body_work);
        float num2 = AppMain.gmBoss3BodyUpdateMoveNormal(body_work);
        if ((double)num1 > 0.0 || (double)num2 > 0.0)
            return;
        AppMain.gmBoss3BodySetActionAllParts(body_work, 5);
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateBattleUpdateSign);
        AppMain.gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStateBattleUpdateSign(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work1 = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.GmBsCmnIsActionEnd(obj_work1) == 0)
            return;
        AppMain.OBS_OBJECT_WORK obj_work2 = AppMain.gmBoss3BodyBattleSearchPillar();
        if (obj_work2 != null)
            AppMain.GmGmkBoss3PillarChangeModeActive(obj_work2, body_work.pattern_no);
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateBattleUpdateWaitPillar);
        obj_work1.user_timer = AppMain.GmBsCmnIsFinalZoneType(obj_work1) == 0 ? 240 : 150;
        if (((int)obj_work1.disp_flag & 1) != 0)
            AppMain.gmBoss3BodySetActionAllParts(body_work, 6);
        else
            AppMain.gmBoss3BodySetActionAllParts(body_work, 7);
    }

    private static void gmBoss3BodyStateBattleUpdateWaitPillar(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            int move_speed = 4096;
            AppMain.GmBsCmnIsFinalZoneType(obj_work);
            if (AppMain.gmBoss3BodyBattleInitMovePattern(body_work, body_work.pattern_no, 1, move_speed) != 0)
            {
                if (AppMain.gmBoss3BodyBattleCheckTurn(body_work) != 0)
                {
                    short dest_angle;
                    int flag_positive;
                    if (((int)obj_work.disp_flag & 1) != 0)
                    {
                        obj_work.disp_flag &= 4294967294U;
                        dest_angle = AppMain.GMD_BOSS3_ANGLE_RIGHT;
                        flag_positive = 1;
                    }
                    else
                    {
                        obj_work.disp_flag |= 1U;
                        dest_angle = AppMain.GMD_BOSS3_ANGLE_LEFT;
                        flag_positive = 0;
                    }
                    AppMain.gmBoss3BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
                }
                AppMain.gmBoss3EffAfterburnerRequestCreate(body_work);
                body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateBattleUpdateMoveSecond);
                AppMain.gmBoss3BodySetActionAllParts(body_work, 1);
            }
            else
            {
                body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateBattleUpdateWaitActive);
                obj_work.user_timer = AppMain.GmGmkBoss3PillarGetActiveTime(body_work.pattern_no) - 240 + 120;
                if (((int)obj_work.disp_flag & 1) != 0)
                    AppMain.gmBoss3BodySetActionAllParts(body_work, 6);
                else
                    AppMain.gmBoss3BodySetActionAllParts(body_work, 7);
            }
        }
    }

    private static void gmBoss3BodyStateBattleUpdateMoveSecond(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        float num1 = AppMain.gmBoss3BodyUpdateTurn(body_work);
        float num2 = AppMain.gmBoss3BodyUpdateMoveNormal(body_work);
        if ((double)num1 > 0.0 || (double)num2 > 0.0)
            return;
        if (((int)obj_work.disp_flag & 1) != 0)
            AppMain.gmBoss3BodySetActionAllParts(body_work, 6);
        else
            AppMain.gmBoss3BodySetActionAllParts(body_work, 7);
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateBattleUpdateWaitActive);
        int num3 = 240;
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
            num3 = 150;
        obj_work.user_timer = AppMain.GmGmkBoss3PillarGetActiveTime(body_work.pattern_no) - num3 - (int)body_work.move_frame + 120;
        AppMain.gmBoss3EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss3BodyStateBattleUpdateWaitActive(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (obsObjectWork.user_timer > 0)
        {
            --obsObjectWork.user_timer;
        }
        else
        {
            body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateBattleUpdateWaitReturn);
            obsObjectWork.user_timer = 30;
            AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmBoss3BodyBattleSearchPillar();
            if (obj_work == null)
                return;
            AppMain.GmGmkBoss3PillarChangeModeReturn(obj_work);
        }
    }

    private static void gmBoss3BodyStateBattleUpdateWaitReturn(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (obsObjectWork.user_timer > 0)
        {
            --obsObjectWork.user_timer;
        }
        else
        {
            AppMain.gmBoss3BodyChangeState(body_work, 4);
            AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmBoss3BodyBattleSearchPillar();
            if (obj_work == null)
                return;
            AppMain.GmGmkBoss3PillarChangeModeDelete(obj_work);
        }
    }

    private static void gmBoss3BodyStateDefeatEnter(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work1 = AppMain.GMM_BS_OBJ((object)body_work);
        obj_work1.flag |= 2U;
        obj_work1.disp_flag |= 16U;
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
        AppMain.GmBsCmnSetObjSpdZero(obj_work1);
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateDefeatUpdateStart);
        obj_work1.user_timer = 40;
        AppMain.OBS_OBJECT_WORK obj_work2 = AppMain.gmBoss3BodyBattleSearchPillar();
        if (obj_work2 != null)
            AppMain.GmGmkBoss3PillarChangeModeReturn(obj_work2);
        AppMain.GmSoundChangeWinBossBGM();
    }

    private static void gmBoss3BodyStateDefeatLeave(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.disp_flag &= 4294967279U;
        obsObjectWork.flag &= 4294967293U;
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmBoss3BodyBattleSearchPillar();
        if (obj_work == null)
            return;
        AppMain.GmGmkBoss3PillarChangeModeDelete(obj_work);
    }

    private static void gmBoss3BodyStateDefeatUpdateStart(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (obsObjectWork.user_timer > 0)
        {
            --obsObjectWork.user_timer;
        }
        else
        {
            AppMain.OBS_OBJECT_WORK parent_obj = AppMain.GMM_BS_OBJ((object)body_work);
            AppMain.gmBoss3EffBombsInit(body_work.bomb_work, parent_obj, parent_obj.pos.x, parent_obj.pos.y, 327680, 327680, 10U, 30U);
            body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateDefeatUpdateFall);
            obsObjectWork.user_timer = 120;
        }
    }

    private static void gmBoss3BodyStateDefeatUpdateFall(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (obsObjectWork.user_timer > 0)
        {
            --obsObjectWork.user_timer;
            AppMain.gmBoss3EffBombsUpdate(body_work.bomb_work);
        }
        else
            body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateDefeatUpdateExplode);
    }

    private static void gmBoss3BodyStateDefeatUpdateExplode(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        body_work.flag |= 134217728U;
        AppMain.GmSoundPlaySE("Boss0_03");
        AppMain.GMM_PAD_VIB_MID_TIME(120f);
        AppMain.GmBsCmnInitFlashScreen(body_work.flash_work, 4f, 5f, 30f);
        AppMain.OBS_OBJECT_WORK parent_obj = AppMain.GMM_BS_OBJ((object)body_work);
        ((AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate(parent_obj, 8)).pos.z = parent_obj.pos.z + 131072;
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateDefeatUpdateScatter);
        AppMain.GmPlayerAddScoreNoDisp((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), 1000);
        obsObjectWork.user_timer = 40;
    }

    private static void gmBoss3BodyStateDefeatUpdateScatter(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBsCmnUpdateFlashScreen(body_work.flash_work);
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.gmBoss3ChangeTextureBurnt(obj_work);
            body_work.flag |= 16777216U;
            AppMain.gmBoss3EffAfterburnerSmokeInit(body_work);
            AppMain.gmBoss3EffBodySmokeInit(body_work);
            body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateDefeatUpdateEnd);
            obj_work.user_timer = 120;
        }
    }

    private static void gmBoss3BodyStateDefeatUpdateEnd(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (obsObjectWork.user_timer > 0)
            --obsObjectWork.user_timer;
        else
            AppMain.gmBoss3BodyChangeState(body_work, 6);
    }

    private static void gmBoss3BodyStateEscapeEnter(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        obj_work.spd_add.x = 327;
        obj_work.spd_add.y = 40;
        if (((int)obj_work.disp_flag & 1) != 0)
        {
            obj_work.disp_flag &= 4294967294U;
            AppMain.gmBoss3BodyInitTurn(body_work, AppMain.GMD_BOSS3_ANGLE_RIGHT, 60f, 1);
        }
        obj_work.flag |= 2U;
        obj_work.move_flag |= 4352U;
        AppMain.gmBoss3BodySetDirectionNormal(body_work);
        AppMain.gmBoss3BodySetActionAllParts(body_work, 8, 1);
        body_work.flag |= 8388608U;
        AppMain.GmMapSetMapDrawSize(1);
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
            body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateEscapeUpdateFinalZone);
        else
            body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateEscapeUpdateScrollLock);
    }

    private static void gmBoss3BodyStateEscapeLeave(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)body_work).flag &= 4294967293U;
    }

    private static void gmBoss3BodyStateEscapeUpdateScrollLock(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.gmBoss3BodyEscapeAddjustSpeed(body_work);
        if ((double)AppMain.gmBoss3BodyUpdateTurn(body_work) > 0.0)
            return;
        AppMain.GmGmkCamScrLimitRelease((byte)4);
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.gmBoss3BodyBattleSearchPillar();
        if (obj_work != null)
            AppMain.GmGmkBoss3PillarWallChangeModeReturn(obj_work);
        AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 1U);
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateEscapeUpdateWaitScreenOut);
    }

    private static void gmBoss3BodyStateEscapeUpdateWaitScreenOut(
      AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.gmBoss3BodyEscapeAddjustSpeed(body_work);
        if (AppMain.gmBoss3BodyEscapeCheckScreenOut(body_work) == 0)
            return;
        AppMain.gmBoss3MgrGetMgrWork(AppMain.GMM_BS_OBJ((object)body_work)).flag |= 2U;
        body_work.proc_update = (AppMain.GMF_BOSS3_BODY_STATE_FUNC)null;
    }

    private static void gmBoss3BodyStateEscapeUpdateFinalZone(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.gmBoss3BodyEscapeAddjustSpeed(body_work);
        if ((double)AppMain.gmBoss3BodyUpdateTurn(body_work) > 0.0)
            return;
        AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 1U);
        body_work.proc_update = new AppMain.GMF_BOSS3_BODY_STATE_FUNC(AppMain.gmBoss3BodyStateEscapeUpdateWaitScreenOut);
    }

    private static void gmBoss3BodyMainFuncWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS3_BODY_WORK body_work = (AppMain.GMS_BOSS3_BODY_WORK)obj_work;
        if (AppMain.gmBoss3MgrCheckSetupComplete(AppMain.gmBoss3MgrGetMgrWork(obj_work)) == 0)
            return;
        AppMain.GmBsCmnInitBossMotionCBSystem(obj_work, body_work.bmcb_mgr);
        AppMain.GmBsCmnCreateSNMWork(body_work.snm_work, obj_work.obj_3d._object, (ushort)1);
        AppMain.GmBsCmnAppendBossMotionCallback(body_work.bmcb_mgr, body_work.snm_work.bmcb_link);
        for (int index = 0; 1 > index; ++index)
            body_work.snm_reg_id[index] = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.g_boss3_node_index_list[index]);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3BodyMainFunc);
        AppMain.gmBoss3BodyChangeState(body_work, 1);
    }

    private static void gmBoss3BodyMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS3_BODY_WORK body_work = (AppMain.GMS_BOSS3_BODY_WORK)obj_work;
        AppMain.gmBoss3BodyUpdateNoHitTime(body_work);
        AppMain.gmBoss3BodyUpdateInvincibleTime(body_work);
        if (body_work.proc_update != null)
            body_work.proc_update(body_work);
        if (((int)body_work.flag & 33554432) != 0)
            AppMain.gmBoss3EffAfterburnerInit(body_work);
        if (((int)body_work.flag & int.MinValue) != 0)
        {
            body_work.flag &= 1073741823U;
            AppMain.gmBoss3BodyChangeState(body_work, 5);
        }
        else
        {
            if (((int)body_work.flag & 1073741824) != 0)
            {
                body_work.flag &= 3221225471U;
                body_work.flag |= 536870912U;
                AppMain.GmBsCmnInitObject3DNNDamageFlicker(obj_work, body_work.flk_work, 32f);
            }
            AppMain.GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, body_work.flk_work);
            AppMain.gmBoss3BodyUpdateDirection(body_work);
        }
    }

}