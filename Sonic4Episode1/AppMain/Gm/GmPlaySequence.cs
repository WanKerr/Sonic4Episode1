using System;

public partial class AppMain
{
    private static void GmPlySeqSetSeqState(GMS_PLAYER_WORK ply_work)
    {
        ply_work.seq_init_tbl = g_gm_ply_seq_init_tbl_list[ply_work.char_id];
        ply_work.seq_state_data_tbl = g_gm_ply_seq_state_data_tbl[ply_work.char_id];
    }

    private static void GmPlySeqMain(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        GMS_PLY_SEQ_STATE_DATA[] seqStateDataTbl = ply_work.seq_state_data_tbl;
        if (ply_work.no_spddown_timer != 0)
            ply_work.no_spddown_timer = ObjTimeCountDown(ply_work.no_spddown_timer);
        if (ply_work.maxdash_timer != 0)
            ply_work.maxdash_timer = ObjTimeCountDown(ply_work.maxdash_timer);
        if ((ply_work.player_flag & GMD_PLF_ACT_GOAL) != 0)
            gmPlySeqActGoal(ply_work);
        if ((ply_work.player_flag & GMD_PLF_BOSS_GOAL_PRE) != 0)
            gmPlySeqBossGoalPre(ply_work);
        if ((ply_work.player_flag & GMD_PLF_BOSS5_DEMO) != 0)
            gmPlySeqBoss5DemoPre(ply_work);
        if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            gmPlySeqSplStgRollCtrl(ply_work);
        gmPlySeqCheckChangeSequence(ply_work);
        if (ply_work.seq_func != null)
            ply_work.seq_func(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 8388608) != 0)
        {
            GmPlayerAnimeSpeedSetWalk(ply_work, ply_work.obj_work.spd_m);
        }
        else if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 4194304) == 0)
        {
            ply_work.obj_work.obj_3d.speed[0] = 1f;
            ply_work.obj_work.obj_3d.speed[1] = 1f;
        }

        if ((ply_work.player_flag & GMD_PLF_PGM_TURN) == 0)
            return;
        int pgmTurnDir = ply_work.pgm_turn_dir;
        int num;
        if (ply_work.pgm_turn_dir_tbl != null)
        {
            num = ply_work.pgm_turn_dir_tbl[ply_work.pgm_turn_tbl_cnt];
            ++ply_work.pgm_turn_tbl_cnt;
            if (ply_work.pgm_turn_tbl_cnt >= ply_work.pgm_turn_tbl_num)
            {
                ply_work.pgm_turn_tbl_cnt = ply_work.pgm_turn_tbl_num - 1;
                ply_work.player_flag &= 4294967279U;
                if ((ply_work.player_flag & GMD_PLF_PGM_TURN_RDM) == 0)
                    num = 0;
            }
        }
        else if (((int) ply_work.obj_work.disp_flag & 1) != 0)
        {
            num = pgmTurnDir - ply_work.pgm_turn_spd;
            if (num <= 0)
            {
                num = 0;
                ply_work.player_flag &= 4294967279U;
            }
        }
        else
        {
            num = pgmTurnDir + ply_work.pgm_turn_spd;
            if (num >= 65536)
            {
                num = 0;
                ply_work.player_flag &= 4294967279U;
            }
        }

        ply_work.pgm_turn_dir = (ushort) num;
        if ((ply_work.player_flag & 0x80000000u) == 0 || (ply_work.player_flag & GMD_PLF_PGM_TURN_RDM) != 0)
            return;
        GmPlayerActionChange(ply_work, ply_work.fall_act_state);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.player_flag &= int.MaxValue;
    }

    private static bool GmPlySeqChangeSequence(GMS_PLAYER_WORK ply_work, int seq_state)
    {
        GmPlySeqChangeSequenceState(ply_work, seq_state);
        if (ply_work.seq_init_tbl[seq_state] == null)
            return false;
        ply_work.seq_init_tbl[seq_state](ply_work);
        return true;
    }

    private static void GmPlySeqChangeSequenceState(GMS_PLAYER_WORK ply_work, int seq_state)
    {
        if (ply_work.gmk_obj != null)
            GmPlayerStateGimmickInit(ply_work);
        ply_work.prev_seq_state = ply_work.seq_state;
        ply_work.seq_state = seq_state;
        ply_work.rect_work[1].flag &= 4294967291U;
        if ((ply_work.player_flag & GMD_PLF_PGM_TURN_RDM) != 0)
            GmPlayerSetReverseOnlyState(ply_work);
        if ((ply_work.player_flag & 0x80000000u) == 0)
            return;
        ply_work.player_flag &= 2147483375U;
        ply_work.pgm_turn_dir_tbl = null;
        ply_work.pgm_turn_dir = 0;
        ply_work.pgm_turn_spd = 0;
    }

    private static void GmPlySeqSetProgramTurn(GMS_PLAYER_WORK ply_work, ushort turn_spd)
    {
        if ((ply_work.player_flag & GMD_PLF_PGM_TURN) == 0)
            ply_work.pgm_turn_dir = 0;
        GmPlayerSetReverse(ply_work);
        ply_work.player_flag |= GMD_PLF_PGM_TURN;
        ply_work.pgm_turn_spd = turn_spd;
        ply_work.pgm_turn_dir += 32768;
        ply_work.pgm_turn_dir_tbl = null;
    }

    private static void GmPlySeqSetProgramTurnTbl(
        GMS_PLAYER_WORK ply_work,
        ushort[] turn_tbl,
        int tbl_num,
        bool rev_depend_mtn)
    {
        if ((ply_work.player_flag & GMD_PLF_PGM_TURN) == 0)
            ply_work.pgm_turn_dir = 0;
        if (!rev_depend_mtn)
            GmPlayerSetReverse(ply_work);
        else
            ply_work.player_flag |= GMD_PLF_PGM_TURN_RDM;
        ply_work.player_flag |= GMD_PLF_PGM_TURN;
        ply_work.pgm_turn_dir_tbl = turn_tbl;
        ply_work.pgm_turn_tbl_num = tbl_num;
        ply_work.pgm_turn_tbl_cnt = 0;
    }

    private static void GmPlySeqSetProgramTurnFwTurn(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.disp_flag & 1) != 0)
            GmPlySeqSetProgramTurnTbl(ply_work, gm_ply_seq_turn_l_dir_tbl, 10, true);
        else
            GmPlySeqSetProgramTurnTbl(ply_work, gm_ply_seq_turn_dir_tbl, 10, true);
    }

    private static void GmPlySeqSetFallTurn(GMS_PLAYER_WORK ply_work)
    {
        int num = 0;
        if ((ply_work.player_flag & 0x80000000u) != 0)
            num = ply_work.pgm_turn_tbl_cnt;
        else
            ply_work.fall_act_state = ply_work.act_state;
        if (((int) ply_work.obj_work.disp_flag & 1) != 0)
            GmPlySeqSetProgramTurnTbl(ply_work, gm_ply_seq_fall_turn_l_dir_tbl, 10, false);
        else
            GmPlySeqSetProgramTurnTbl(ply_work, gm_ply_seq_fall_turn_dir_tbl, 10, false);
        ply_work.player_flag |= GMD_PLF_PGM_FALL_TURN;
        if (ply_work.act_state == 42 || ply_work.act_state == 43)
            GmPlayerActionChange(ply_work, 43);
        else
            GmPlayerActionChange(ply_work, 41);
        if (num == 0)
            return;
        ply_work.pgm_turn_tbl_cnt = 10 - num;
        ply_work.obj_work.obj_3d.frame[0] = ply_work.pgm_turn_tbl_cnt;
    }

    private static void GmPlySeqChangeFw(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqInitFw(GMS_PLAYER_WORK ply_work)
    {
        if (!GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            if (ply_work.obj_work.spd_m != 0)
            {
                if (ply_work.prev_seq_state == 2)
                    GmPlayerActionChange(ply_work, 0);
                GmPlySeqChangeSequence(ply_work, 1);
                return;
            }

            if ((ply_work.player_flag & GMD_PLF_PINBALL_SONIC) == 0)
                GmPlayerActionChange(ply_work, 0);
            else
                GmPlyEfctCreateSpinJumpBlur(ply_work);
        }
        else
            GmPlyEfctCreateSpinJumpBlur(ply_work);

        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqFwMain);
    }

    private static void gmPlySeqFwMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state == 0)
        {
            if (((int) ply_work.obj_work.disp_flag & 8) == 0)
                return;
            int num = (int) ply_work.obj_work.user_work + 1;
            ply_work.obj_work.user_work = (uint) num;
            if ((int) ply_work.obj_work.user_work < 8)
                return;
            if ((ply_work.player_flag & GMD_PLF_SUPER_SONIC) != 0)
                GmPlayerActionChange(ply_work, 4);
            else
                GmPlayerActionChange(ply_work, 2);
            if (((int) ply_work.obj_work.disp_flag & 1) != 0)
                GmPlySeqSetProgramTurn(ply_work, 4096);
            ply_work.obj_work.user_work = 0U;
        }
        else if (ply_work.act_state == 2 || ply_work.act_state == 4 || ply_work.act_state == 6)
        {
            if (((int) ply_work.obj_work.disp_flag & 8) == 0)
                return;
            GmPlayerActionChange(ply_work, ply_work.act_state + 1);
            ply_work.obj_work.disp_flag |= 4U;
            ply_work.obj_work.user_work = 0U;
        }
        else if (ply_work.act_state == 3)
        {
            if (((int) ply_work.obj_work.disp_flag & 8) == 0)
                return;
            int num = (int) ply_work.obj_work.user_work + 1;
            ply_work.obj_work.user_work = (uint) num;
            if ((int) ply_work.obj_work.user_work < 10)
                return;
            GmPlayerActionChange(ply_work, 4);
            ply_work.obj_work.user_work = 0U;
        }
        else
        {
            if (ply_work.act_state != 5 || ((int) ply_work.obj_work.disp_flag & 8) == 0)
                return;
            int num = (int) ply_work.obj_work.user_work + 1;
            ply_work.obj_work.user_work = (uint) num;
            if ((int) ply_work.obj_work.user_work < 3 || (ply_work.player_flag & GMD_PLF_SUPER_SONIC) != 0)
                return;
            GmPlayerActionChange(ply_work, 6);
            ply_work.obj_work.user_work = 0U;
        }
    }

    private static void GmPlySeqInitWalk(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & GMD_PLF_PINBALL_SONIC) == 0)
            GmPlayerWalkActionSet(ply_work);
        else
            GmPlyEfctCreateSpinJumpBlur(ply_work);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqWalkMain);
        ply_work.obj_work.user_timer = 0;
    }

    private static void gmPlySeqWalkMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m > 0 && GmPlayerKeyCheckWalkRight(ply_work) &&
            ((int) ply_work.obj_work.disp_flag & 1) != 0 || ply_work.obj_work.spd_m < 0 &&
            GmPlayerKeyCheckWalkLeft(ply_work) && ((int) ply_work.obj_work.disp_flag & 1) == 0)
            GmPlySeqSetProgramTurn(ply_work, 4096);
        GmPlayerWalkActionCheck(ply_work);
        if ((ply_work.obj_work.user_timer & 63) == 1 && ply_work.obj_work.ride_obj == null)
            GmPlyEfctCreateFootSmoke(ply_work);
        ++ply_work.obj_work.user_timer;
    }

    private static void GmPlySeqInitTurn(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.prev_seq_state == 2)
        {
            ply_work.player_flag &= 2147483375U;
            GmPlayerSetReverse(ply_work);
            GmPlySeqChangeSequence(ply_work, 0);
        }
        else
        {
            if (23 <= ply_work.act_state && ply_work.act_state <= 25)
                GmPlayerActionChange(ply_work, 10);
            else if (20 <= ply_work.act_state && ply_work.act_state <= 22)
            {
                GmPlayerActionChange(ply_work, 9);
            }
            else
            {
                GmPlayerActionChange(ply_work, 8);
                GmPlySeqSetProgramTurnFwTurn(ply_work);
            }

            ply_work.obj_work.move_flag &= 4294967279U;
            ply_work.seq_func = new seq_func_delegate(gmPlySeqTurnMain);
        }
    }

    private static void gmPlySeqTurnMain(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.disp_flag & 8) == 0)
            return;
        GmPlayerSetReverseOnlyState(ply_work);
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqInitLookupStart(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 11);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqLookupMain);
    }

    private static void GmPlySeqInitLookupMiddle(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 12);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqLookupMain);
    }

    private static void GmPlySeqInitLookupEnd(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 13);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqLookupEndMain);
    }

    private static void gmPlySeqLookupMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m != 0)
        {
            GmPlySeqChangeSequence(ply_work, 1);
        }
        else
        {
            if (ply_work.act_state != 11 || ((int) ply_work.obj_work.disp_flag & 8) == 0)
                return;
            GmPlySeqChangeSequence(ply_work, 4);
        }
    }

    private static void gmPlySeqLookupEndMain(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.disp_flag & 8) == 0)
            return;
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqInitSquatStart(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 14);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqSquatMain);
    }

    private static void GmPlySeqInitSquatMiddle(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 15);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < 4096)
            ply_work.obj_work.spd_m = 0;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqSquatMain);
    }

    private static void GmPlySeqInitSquatEnd(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 16);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqSquatEndMain);
    }

    private static void gmPlySeqSquatMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state == 14 && ((int) ply_work.obj_work.disp_flag & 8) != 0)
            GmPlySeqChangeSequence(ply_work, 7);
        if (ply_work.seq_state != 7 || ply_work.obj_work.spd_m == 0)
            return;
        ply_work.obj_work.move_flag |= 16384U;
        GmPlySeqChangeSequence(ply_work, 10);
    }

    private static void gmPlySeqSquatEndMain(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.disp_flag & 8) == 0)
            return;
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqInitBrake(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 23);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqBrakeMain);
        GmSoundPlaySE("Brake");
        GmPlyEfctCreateBrakeImpact(ply_work);
        GmPlyEfctCreateBrakeDust(ply_work);
    }

    private static void gmPlySeqBrakeMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 25 &&
            (((int) ply_work.obj_work.disp_flag & 1) != 0 && !GmPlayerKeyCheckWalkRight(ply_work) ||
             ((int) ply_work.obj_work.disp_flag & 1) == 0 && !GmPlayerKeyCheckWalkLeft(ply_work)))
            GmPlayerActionChange(ply_work, 25);
        switch (ply_work.act_state)
        {
            case 23:
                if (((int) ply_work.obj_work.disp_flag & 8) == 0)
                    break;
                GmPlayerActionChange(ply_work, 24);
                ply_work.obj_work.disp_flag |= 4U;
                break;
            case 24:
                if (ply_work.obj_work.spd_m > 0 && GmPlayerKeyCheckWalkLeft(ply_work) ||
                    ply_work.obj_work.spd_m < 0 && GmPlayerKeyCheckWalkRight(ply_work))
                    break;
                GmPlySeqChangeSequence(ply_work, 2);
                break;
            case 25:
                if (((int) ply_work.obj_work.disp_flag & 8) == 0)
                    break;
                if (ply_work.obj_work.spd_m != 0)
                {
                    GmPlySeqChangeSequence(ply_work, 1);
                    break;
                }

                GmPlySeqChangeSequence(ply_work, 0);
                break;
        }
    }

    private static void GmPlySeqInitSpin(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 27);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqSpinMain);
        GmPlayerSetAtk(ply_work);
        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        if (ply_work.prev_seq_state != 37 && (ply_work.player_flag & GMD_PLF_PINBALL_SONIC) == 0)
        {
            if (betterSfx)
            {
                GmSoundPlaySE(ply_work.prev_seq_state == 11 ? "Dash1" : "Dash2");
            }
            else
            {
                GmSoundPlaySE("Spin");
            }
        }

        GmPlyEfctCreateSpinDashDust(ply_work);
        GmPlyEfctCreateSuperAuraSpin(ply_work);
        GmPlyEfctCreateSpinDashBlur(ply_work, 1U);
        GmPlyEfctCreateSpinDashCircleBlur(ply_work);
        GmPlyEfctCreateTrail(ply_work, GME_PLY_EFCT_TRAIL_TYPE_SPINDASH);
    }

    private static void gmPlySeqSpinMain(GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlySeqInitSpinDashAcc(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 29 && ply_work.act_state != 30 && ply_work.act_state != 28)
            GmPlyEfctCreateSpinStartBlur(ply_work);
        if (ply_work.efct_spin_start_blur != null)
            GmPlayerActionChange(ply_work, 28);
        else
            GmPlayerActionChange(ply_work, 29);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.dash_power = ply_work.dash_power == 0
            ? ply_work.spd_spin
            : ObjSpdUpSet(ply_work.dash_power, ply_work.spd_add_spin, ply_work.spd_max_spin);
        ply_work.seq_func = new seq_func_delegate(gmPlySeqSpinDashMain);
        GmPlayerSetAtk(ply_work);

        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        if (betterSfx)
        {
            ply_work.spinHandle.snd_ctrl_param.pitch =
                Math.Min(((float) (ply_work.dash_power - ply_work.spd_spin) / ply_work.spd_max_spin), 1f);
            GmSoundPlaySEForce("Spin", ply_work.spinHandle);
        }
        else
        {
            if (ply_work.spin_se_timer <= 0)
            {
                GmSoundPlaySE("Dash1");
                ply_work.spin_se_timer = 25;
            }

            if (ply_work.spin_back_se_timer <= 0)
            {
                GmSoundPlaySE("Dash2");
                ply_work.spin_se_timer = 50;
            }
        }

        if (ply_work.prev_seq_state == 11)
            return;
        GmPlyEfctCreateSpinAddDust(ply_work);
    }

    private static void GmPlySeqInitSpinDash(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 29 && ply_work.act_state != 30 && ply_work.act_state != 28)
            GmPlyEfctCreateSpinStartBlur(ply_work);
        GmPlayerActionChange(ply_work, 30);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqSpinDashMain);
        GmPlayerSetAtk(ply_work);
        GmPlyEfctCreateSpinDust(ply_work);
    }

    private static void gmPlySeqSpinDashMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state == 28 && ply_work.efct_spin_start_blur == null)
        {
            if (ply_work.seq_state == 11)
            {
                GmPlySeqChangeSequence(ply_work, 12);
                return;
            }

            GmPlayerActionChange(ply_work, 30);
        }

        if (ply_work.act_state == 29 && ((int) ply_work.obj_work.disp_flag & 8) != 0)
            GmPlySeqChangeSequence(ply_work, 12);
        else if ((ply_work.key_on & 2) == 0 || ply_work.obj_work.spd_m != 0)
        {
            ply_work.no_spddown_timer = 72;
            ply_work.camera_stop_timer = 32768;
            int a = 48128 + FX_Mul(ply_work.dash_power, 512);
            if (((int) ply_work.obj_work.disp_flag & 1) != 0)
                a = -a;
            if (MTM_MATH_ABS(a) > MTM_MATH_ABS(ply_work.obj_work.spd_m))
                ply_work.obj_work.spd_m = a;
            ply_work.dash_power = 0;
            ply_work.obj_work.move_flag |= 16384U;
            GmPlySeqChangeSequence(ply_work, 10);
            GmPlyEfctCreateSpinDashImpact(ply_work);
            GMM_PAD_VIB_SMALL();
        }
        else
        {
            if ((ply_work.key_on & 1) == 0)
                return;
            GmPlySeqChangeSequence(ply_work, 0);
        }
    }

    private static void GmPlySeqInitStaggerFront(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 33);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqStaggerMain);
        GmPlyEfctCreateSweat(ply_work);
    }

    private static void GmPlySeqInitStaggerBack(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 34);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqStaggerMain);
        GmPlyEfctCreateSweat(ply_work);
    }

    private static void GmPlySeqInitStaggerDanger(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 35);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqStaggerMain);
        GmPlyEfctCreateSweat(ply_work);
    }

    private static void gmPlySeqStaggerMain(GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlySeqInitFall(GMS_PLAYER_WORK ply_work)
    {
        if (!GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY() &&
            ((ply_work.player_flag & GMD_PLF_SUPER_SONIC) == 0 || ply_work.act_state != 21 && ply_work.act_state != 22) &&
            ((ply_work.player_flag & GMD_PLF_PINBALL_SONIC) == 0 && ply_work.prev_seq_state != 40))
        {
            if ((ushort) (ply_work.obj_work.dir.z - 8192U) <= 49152)
                GmPlayerActionChange(ply_work, 42);
            else
                GmPlayerActionChange(ply_work, 40);
        }

        GmPlySeqInitFallState(ply_work);
    }

    private static void GmPlySeqInitFallState(GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 32912U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqJumpMain);
        ply_work.obj_work.spd.x = FX_Mul(ply_work.obj_work.spd_m,
            mtMathCos(ply_work.obj_work.dir.z -
                      (g_gm_main_system.pseudofall_dir - ply_work.prev_dir_fall2)));
        ply_work.obj_work.spd.y = FX_Mul(ply_work.obj_work.spd_m,
            mtMathSin(ply_work.obj_work.dir.z -
                      (g_gm_main_system.pseudofall_dir - ply_work.prev_dir_fall2)));
        ply_work.obj_work.spd_m = 0;
        ply_work.player_flag &= 4294967280U;
        ply_work.player_flag |= GMD_PLF_USER1;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
    }

    private static void GmPlySeqInitJump(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & GMD_PLF_PINBALL_SONIC) == 0)
            GmPlayerActionChange(ply_work, 39);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4290772990U;
        ushort z = ply_work.obj_work.dir.z;
        if ((z + 256 & 8192) != 0 && (z + 256 & 4095) <= 1024)
        {
            if (ply_work.obj_work.spd_m > 0 && z < 32768)
                z -= 1152;
            else if (ply_work.obj_work.spd_m < 0 && z > 32768)
                z += 1152;
        }

        ply_work.obj_work.spd.x = FX_Mul(ply_work.obj_work.spd_m, mtMathCos(z));
        ply_work.obj_work.spd.y = FX_Mul(ply_work.obj_work.spd_m, mtMathSin(z));
        ply_work.obj_work.spd.x += FX_Mul(ply_work.spd_jump, mtMathSin(ply_work.obj_work.dir.z));
        ply_work.obj_work.spd.y += FX_Mul(-ply_work.spd_jump, mtMathCos(ply_work.obj_work.dir.z));
        if (((int) ply_work.gmk_flag & 4096) != 0)
        {
            ply_work.obj_work.spd.z = ply_work.obj_work.spd.y;
            ply_work.obj_work.spd.y = 0;
            if (ply_work.obj_work.pos.z < 0)
                ply_work.obj_work.spd.z = -ply_work.obj_work.spd.z;
        }

        ply_work.player_flag &= 4294967280U;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
        GmPlySeqSetJumpState(ply_work, 0, 0U);
        if (ply_work.prev_seq_state == 10 && ply_work.no_spddown_timer >= 20)
            ply_work.no_spddown_timer = 20;
        GmPlayerSetAtk(ply_work);
        if (gm_ply_seq_jump_call_se_jump)
        {
            var modernSfx = gs.backup.SSave.CreateInstance().GetRemaster().ModernSoundEffects;
            GmSoundPlaySE("Jump");
            //if (modernSfx)
            //    AppMain.GmSoundPlaySE("JumpSpin");
        }

        GmPlyEfctCreateJumpDust(ply_work);
        GmPlyEfctCreateSpinJumpBlur(ply_work);
    }

    private static void GmPlySeqSetJumpState(
        GMS_PLAYER_WORK ply_work,
        int nofall_timer,
        uint flag)
    {
        ply_work.obj_work.user_timer = nofall_timer;
        if (ply_work.no_jump_move_timer == 0)
            ply_work.player_flag &= 4294967263U;
        ply_work.player_flag &= 4294967152U;
        if (((int) flag & 1) != 0)
            ply_work.player_flag |= GMD_PLF_USER1;
        if (((int) flag & 2) != 0)
            ply_work.player_flag |= GMD_PLF_USER2;
        if (((int) flag & 4) != 0)
            ply_work.player_flag |= GMD_PLF_NOHOMING;
        if (((int) flag & 8) != 0)
            ply_work.player_flag |= GMD_PLF_NOJUMPMOVE;
        if ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) != 0)
            ply_work.seq_func = new seq_func_delegate(gmPlySeqTruckJumpMain);
        else
            ply_work.seq_func = new seq_func_delegate(gmPlySeqJumpMain);
    }

    private static void GmPlySeqInitJumpEX(GMS_PLAYER_WORK ply_work, int spd_x, int spd_y)
    {
        GmPlySeqInitJump(ply_work);
        ply_work.obj_work.spd.x = spd_x;
        ply_work.obj_work.spd.y = spd_y;
        ply_work.obj_work.spd_m = 0;
        if (ply_work.obj_work.spd.x < 0)
        {
            if (ply_work.obj_work.spd_m > 0)
                ply_work.obj_work.spd_m = 0;
            if (((int) ply_work.obj_work.disp_flag & 1) != 0)
                return;
            GmPlayerSetReverse(ply_work);
        }
        else
        {
            if (ply_work.obj_work.spd_m < 0)
                ply_work.obj_work.spd_m = 0;
            if (((int) ply_work.obj_work.disp_flag & 1) == 0)
                return;
            GmPlayerSetReverse(ply_work);
        }
    }

    private static void GmPlySeqAtkReactionInit(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.seq_state == 19)
        {
            GmPlySeqChangeSequence(ply_work, 20);
        }
        else
        {
            if (((int) ply_work.obj_work.move_flag & 16) == 0)
                return;
            int x = ply_work.obj_work.spd.x;
            int spdM = ply_work.obj_work.spd_m;
            GmPlayerStateInit(ply_work);
            gm_ply_seq_jump_call_se_jump = false;
            GmPlySeqChangeSequence(ply_work, 17);
            gm_ply_seq_jump_call_se_jump = true;
            GmPlySeqSetJumpState(ply_work, 0, 1U);
            ply_work.obj_work.spd.y = -16384;
            ply_work.obj_work.spd.x = x;
            ply_work.obj_work.spd_m = spdM;
        }
    }

    private static void GmPlySeqAtkReactionSpdInit(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int no_spddown_timer)
    {
        ply_work.obj_work.spd.x = spd_x;
        ply_work.no_spddown_timer = no_spddown_timer;
        GmPlySeqAtkReactionInit(ply_work);
    }

    private static void gmPlySeqJumpMain(GMS_PLAYER_WORK ply_work)
    {
        int num = ply_work.obj_work.spd.y;
        if (((int) ply_work.gmk_flag & 4096) != 0)
        {
            num = ply_work.obj_work.spd.z;
            if (ply_work.obj_work.dir.x > 32768)
                num = -num;
        }

        if (ply_work.obj_work.user_timer != 0)
        {
            --ply_work.obj_work.user_timer;
            if (ply_work.obj_work.user_timer == 0)
                ply_work.obj_work.move_flag |= 128U;
        }

        if ((ply_work.player_flag & 5) == 0 && !GmPlayerKeyCheckJumpKeyOn(ply_work) && num < -16384)
            ply_work.player_flag |= 4U;
        if ((ply_work.player_flag & 4) != 0 && ply_work.obj_work.spd.y < 0)
            ply_work.obj_work.spd.y += ply_work.obj_work.spd_fall;
        switch (ply_work.act_state)
        {
            case 44:
                if (num > 1024)
                {
                    GmPlayerActionChange(ply_work, 45);
                    break;
                }

                break;
            case 45:
                if (((int) ply_work.obj_work.disp_flag & 8) != 0)
                {
                    GmPlayerActionChange(ply_work, 46);
                    ply_work.obj_work.disp_flag |= 4U;
                    break;
                }

                break;
            case 47:
                if (((int) ply_work.obj_work.disp_flag & 8) != 0)
                {
                    ply_work.obj_work.disp_flag |= 1024U;
                    GmPlayerActionChange(ply_work, 48);
                    ply_work.obj_work.disp_flag |= 4U;
                    break;
                }

                break;
        }

        if (((int) ply_work.obj_work.move_flag & 1) == 0)
            return;
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqInitWallPush(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 17);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqWallPushMain);
    }

    private static void gmPlySeqWallPushMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 17 || ((int) ply_work.obj_work.disp_flag & 8) == 0)
            return;
        GmPlayerActionChange(ply_work, 18);
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlySeqInitHoming(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.enemy_obj == null)
        {
            GmPlySeqChangeSequence(ply_work, 21);
        }
        else
        {
            if ((ply_work.player_flag & 131072) == 0)
            {
                GmPlayerActionChange(ply_work, 31);
                ply_work.obj_work.disp_flag |= 4U;
            }

            ply_work.obj_work.move_flag |= 32784U;
            ply_work.obj_work.move_flag &= 4294967166U;
            ply_work.player_flag |= 128U;
            ply_work.obj_work.dir.z = 0;
            ply_work.gmk_flag &= 4261410812U;
            ply_work.seq_func = new seq_func_delegate(gmPlySeqHomingMain);
            ply_work.obj_work.user_timer = 131072;
            ply_work.homing_timer = 98304;
            ply_work.homing_boost_timer = 262144;
            GmPlayerSetAtk(ply_work);
            GmPlyEfctCreateHomingImpact(ply_work);
            GmPlyEfctCreateTrail(ply_work, GME_PLY_EFCT_TRAIL_TYPE_HOMING); // added from iOS+win32
            GmSoundPlaySE("Homing");
        }
    }

    private static void GmPlySeqSetNoJumpMoveTime(GMS_PLAYER_WORK ply_work, int time)
    {
        ply_work.no_jump_move_timer = time;
        ply_work.player_flag |= 32U;
    }

    private static void gmPlySeqHomingMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.user_timer == 0)
        {
            GmPlySeqChangeSequence(ply_work, 16);
        }
        else
        {
            ply_work.obj_work.user_timer = ObjTimeCountDown(ply_work.obj_work.user_timer);
            if (((int) ply_work.obj_work.move_flag & 1) != 0)
            {
                GmPlySeqLandingSet(ply_work, 0);
                GmPlySeqChangeSequence(ply_work, 0);
            }
            else
            {
                if (ply_work.enemy_obj == null)
                    return;
                OBS_RECT_WORK obsRectWork = ((GMS_ENEMY_COM_WORK) ply_work.enemy_obj).rect_work[2];
                int x = ply_work.enemy_obj.pos.x;
                int num1 = ((int) ply_work.enemy_obj.disp_flag & 2) == 0
                    ? ply_work.enemy_obj.pos.y + (obsRectWork.rect.top + obsRectWork.rect.bottom << 11)
                    : ply_work.enemy_obj.pos.y - (obsRectWork.rect.top + obsRectWork.rect.bottom << 11);
                float num2 = FXM_FX32_TO_FLOAT(x - ply_work.obj_work.pos.x);
                int ang = nnArcTan2(FXM_FX32_TO_FLOAT(num1 - ply_work.obj_work.pos.y),
                    num2) + ply_work.obj_work.dir_fall;
                ply_work.obj_work.spd.x = (int) (nnCos(ang) * 61440.0);
                ply_work.obj_work.spd.y = (int) (nnSin(ang) * 61440.0);
                if (ply_work.obj_work.spd.x < 0)
                    ply_work.obj_work.disp_flag |= 1U;
                else if (ply_work.obj_work.spd.x > 0)
                    ply_work.obj_work.disp_flag &= 4294967294U;
                if (MTM_MATH_ABS(ply_work.obj_work.spd.x) <= 256 ||
                    ((int) ply_work.obj_work.move_flag & 4) == 0)
                    return;
                GmPlySeqLandingSet(ply_work, 0);
                GmPlySeqChangeSequence(ply_work, 0);
            }
        }
    }

    private static void GmPlySeqInitHomingRef(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 131072) == 0)
            GmPlayerActionChange(ply_work, 32);
        ply_work.player_flag &= 4294967167U;
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 32912U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqHomingRefMain);
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = (ply_work.player_flag & 67108864) == 0
            ? -20480
            : GMD_PLAYER_WATERJUMP_GET(-20480);
        ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.player_flag &= 4294967280U;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
        GmPlyEfctCreateJumpDust(ply_work);
    }

    private static void gmPlySeqHomingRefMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd.y >= 0)
        {
            GmPlySeqChangeSequence(ply_work, 16);
        }
        else
        {
            if (((int) ply_work.obj_work.move_flag & 1) == 0)
                return;
            GmPlySeqLandingSet(ply_work, 0);
            GmPlySeqChangeSequence(ply_work, 0);
        }
    }

    private static void GmPlySeqInitJumpDash(GMS_PLAYER_WORK ply_work)
    {
        var modernSfx = gs.backup.SSave.CreateInstance().GetRemaster().ModernSoundEffects;
        
        if ((ply_work.player_flag & 131072) == 0)
        {
            GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.player_flag |= 160U;
        ply_work.obj_work.dir.z = 0;
        ply_work.gmk_flag &= 4261410812U;
        int ang = ((int) ply_work.obj_work.disp_flag & 1) == 0 ? 63488 : -30720;
        if ((ply_work.player_flag & 32768) != 0)
        {
            ply_work.obj_work.spd.y = 0;
            ply_work.obj_work.spd.x += (int) (4096.0 * nnCos(ang));
            ply_work.obj_work.spd.y += -(int) (4096.0 * nnSin(ang));
            ply_work.no_spddown_timer = 8;
            ply_work.obj_work.user_timer = 20;
        }
        else
        {
            ply_work.obj_work.spd.y = 0;
            ply_work.obj_work.spd.x += (int) (16384.0 * nnCos(ang));
            ply_work.obj_work.spd.y += -(int) (16384.0 * nnSin(ang));
            ply_work.no_spddown_timer = 8;
            ply_work.obj_work.user_timer = 20;
        }

        // if (modernSfx)
        //     GmSoundPlaySE("Dash");

        GmPlayerSetAtk(ply_work);
        GmPlyEfctCreateJumpDash(ply_work);
        ply_work.seq_func = new seq_func_delegate(gmPlySeqJumpDashMain);
    }

    private static void gmPlySeqJumpDashMain(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.move_flag & 1) != 0)
        {
            ply_work.player_flag &= 4294967263U;
            GmPlySeqLandingSet(ply_work, 0);
            GmPlySeqChangeSequence(ply_work, 0);
        }
        else if (ply_work.obj_work.user_timer == 0)
        {
            int x = ply_work.obj_work.spd.x;
            int y = ply_work.obj_work.spd.y;
            GmPlySeqChangeSequence(ply_work, 16);
            ply_work.obj_work.spd.x = x;
            ply_work.obj_work.spd.y = y;
            ply_work.player_flag &= 4294967263U;
        }
        else
            --ply_work.obj_work.user_timer;
    }

    private static void GmPlySeqChangeDamage(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequence(ply_work, 22);
    }

    private static void GmPlySeqChangeDamageSetSpd(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        GmPlySeqChangeSequence(ply_work, 22);
        ply_work.obj_work.spd.x = spd_x;
        ply_work.obj_work.spd.y = spd_y;
        if (spd_x < 0)
            ply_work.obj_work.disp_flag &= 4294967294U;
        else
            ply_work.obj_work.disp_flag |= 1U;
    }

    private static void GmPlySeqInitDamage(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerStateInit(ply_work);
        if ((ply_work.player_flag & 32768) != 0)
        {
            ply_work.obj_work.spd.x = 24576;
            ply_work.obj_work.spd.y = -12288;
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.disp_flag &= 4294967294U;
        }
        else
        {
            ply_work.obj_work.spd.x = -6144;
            ply_work.obj_work.spd.y = -12288;
            ply_work.obj_work.spd_m = 0;
            if (((int) ply_work.obj_work.disp_flag & 1) != 0)
                ply_work.obj_work.spd.x = -ply_work.obj_work.spd.x;
        }

        GmPlayerActionChange(ply_work, 36);
        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.invincible_timer = ply_work.time_damage;
        GmPlayerSetDefInvincible(ply_work);
        ply_work.seq_func = new seq_func_delegate(gmPlySeqDamageMain);
        ply_work.obj_work.disp_flag |= 4U;
        GMM_PAD_VIB_LARGE_TIME(60f);
    }

    private static void gmPlySeqDamageMain(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.move_flag & 1) == 0)
            return;
        ply_work.rect_work[0].flag &= 4294967039U;
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqChangeDeath(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequence(ply_work, 23);
    }

    private static void GmPlySeqInitDeath(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 1024) != 0 || (ply_work.player_flag & 16777216) != 0)
            return;
        if ((ply_work.player_flag & 16384) != 0)
            GmPlayerSetEndSuperSonic(ply_work);
        GmPlayerStateInit(ply_work);
        ply_work.obj_work.disp_flag &= 4294967294U;
        ply_work.obj_work.move_flag |= 768U;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.y = -ply_work.spd_jump;
        ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.dir.z = 0;
        ply_work.obj_work.pos.z = 983040;
        if ((ply_work.player_flag & 262144) != 0)
        {
            ply_work.jump_pseudofall_dir = g_gm_main_system.pseudofall_dir;
            ply_work.gmk_flag |= 16777216U;
            ply_work.obj_work.dir.x = ply_work.obj_work.dir.y = 0;
            ply_work.obj_work.dir.z = 0;
            ply_work.obj_work.move_flag &= 4294967231U;
        }

        ply_work.player_flag &= 3489660927U;
        ply_work.player_flag |= 1024U;
        ply_work.obj_work.flag |= 2U;
        if ((ply_work.player_flag & 67108864) != 0)
            GmSoundPlaySE("Damage3");
        else
            GmSoundPlaySE("Damage1");
        GmPlayerActionChange(ply_work, 37);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqDeathMain);
        ply_work.obj_work.user_timer = 0;
        ply_work.water_timer = 0;
        GMM_PAD_VIB_LARGE_TIME(90f);
    }

    private static void gmPlySeqDeathMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state == 37 && ((int) ply_work.obj_work.disp_flag & 8) != 0)
        {
            GmPlayerActionChange(ply_work, 38);
            ply_work.obj_work.disp_flag |= 4U;
        }

        if ((ply_work.player_flag & 262144) == 0)
            return;
        ply_work.obj_work.dir.z += 1024;
    }

    private static void GmPlySeqChangeTransformSuper(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequence(ply_work, 24);
    }

    private static void GmPlySeqInitTransformSuper(GMS_PLAYER_WORK ply_work)
    {
        int num1 = 0;
        int num2 = 0;
        if ((ply_work.player_flag & 1024) != 0 || (ply_work.player_flag & 16777216) != 0)
            return;
        ushort z = ply_work.obj_work.dir.z;
        if (((int) ply_work.obj_work.move_flag & 16) == 0)
        {
            num1 = FXM_FLOAT_TO_FX32(nnCos(81920 - ply_work.obj_work.dir.z) * 3f);
            num2 = -FXM_FLOAT_TO_FX32(nnSin(81920 - ply_work.obj_work.dir.z) * 3f);
        }

        GmPlayerStateInit(ply_work);
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.flag |= 2U;
        if ((ply_work.player_flag & 262144) != 0)
            ply_work.obj_work.move_flag |= 8448U;
        if (((int) ply_work.obj_work.move_flag & 16) == 0)
        {
            ply_work.obj_work.move_flag |= 16U;
            ply_work.obj_work.move_flag &= 4294967280U;
            ply_work.obj_work.pos.x += num1;
            ply_work.obj_work.pos.y += num2;
        }

        ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.dir.z = 0;
        if ((ply_work.player_flag & 262144) != 0)
            ply_work.obj_work.dir.z = z;
        if ((ply_work.player_flag & 262144) != 0)
        {
            ply_work.obj_work.pos.z = short.MinValue;
            ply_work.gmk_flag |= 536870912U;
        }

        GmPlayerActionChange(ply_work, 50);
        ply_work.seq_func = new seq_func_delegate(gmPlySeqTransformSuperMain);
        ply_work.obj_work.user_timer = 593920;
        ply_work.obj_work.user_work = 0U;
        GmPlyEfctCreateSuperStart(ply_work);
    }

    public static void gmPlySeqTransformSuperMain(GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.user_timer = ObjTimeCountDown(ply_work.obj_work.user_timer);
        if (ply_work.act_state == 50)
        {
            if (((int) ply_work.obj_work.disp_flag & 8) != 0)
            {
                GmPlayerActionChange(ply_work, 51);
                ply_work.obj_work.disp_flag |= 4U;
            }
        }
        else if (ply_work.act_state != 52 && (ply_work.obj_work.user_timer & 4294963200L) == 286720L)
            GmPlayerActionChange(ply_work, 52);

        if ((ply_work.obj_work.user_timer & 4294963200L) == 245760L && (ply_work.player_flag & 16384) == 0)
        {
            GMS_PLAYER_RESET_ACT_WORK reset_act_work = new GMS_PLAYER_RESET_ACT_WORK();
            ushort z = ply_work.obj_work.dir.z;
            GmPlayerActionChange(ply_work, 53);
            ply_work.obj_work.disp_flag |= 4U;
            GmPlayerSaveResetAction(ply_work, reset_act_work);
            GmPlayerSetSuperSonic(ply_work);
            GmPlayerResetAction(ply_work, reset_act_work);
            ply_work.obj_work.move_flag &= 4294967167U;
            ply_work.obj_work.flag |= 2U;
            if ((ply_work.player_flag & 262144) != 0)
                ply_work.obj_work.move_flag |= 8448U;
            if ((ply_work.player_flag & 262144) != 0)
                ply_work.obj_work.dir.z = z;
        }

        if (ply_work.obj_work.user_timer != 0)
            return;
        ply_work.obj_work.move_flag |= 128U;
        ply_work.obj_work.flag &= 4294967293U;
        ply_work.obj_work.move_flag &= 4294958847U;
        ply_work.super_sonic_ring_timer = 245760;
        if ((ply_work.player_flag & 262144) != 0)
        {
            ply_work.obj_work.pos.z = 0;
            ply_work.gmk_flag &= 3758096383U;
        }

        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqChangeActGoal(GMS_PLAYER_WORK ply_work)
    {
        SaveState.deleteSave();
        if ((ply_work.player_flag & 1024) != 0 || ((int) g_gm_main_system.game_flag & 16384) != 0)
            return;
        uint moveFlag = ply_work.obj_work.move_flag;
        GmPlayerStateInit(ply_work);
        if (ply_work.seq_state == 11 || ply_work.seq_state == 12)
            GmPlySeqChangeSequence(ply_work, 0);
        ply_work.obj_work.move_flag |= moveFlag & 1U;
        ply_work.obj_work.move_flag &= 4294441983U;
        ply_work.player_flag |= 22020096U;
        GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
    }

    private static void gmPlySeqActGoal(GMS_PLAYER_WORK ply_work)
    {
        if (((int) g_gm_main_system.game_flag & 16384) != 0)
            return;
        ply_work.player_flag |= 4194304U;
        GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
        ply_work.water_timer = 0;
        if (FXM_FLOAT_TO_FX32(ObjCameraGet(g_obj.glb_camera_id).disp_pos.x) +
            (OBD_LCD_X >> 1) + 128 <= ply_work.obj_work.pos.x >> 12)
            return;
        ply_work.key_on |= 8;
        ply_work.key_walk_rot_z = short.MaxValue;
    }

    private static void GmPlySeqChangeBossGoal(
        GMS_PLAYER_WORK ply_work,
        int capsule_pos_x,
        int capsule_pos_y)
    {
        SaveState.deleteSave();
        if ((ply_work.player_flag & 1024) != 0)
            return;
        GmPlayerStateInit(ply_work);
        ply_work.player_flag |= 23068672U;
        ply_work.rect_work[0].def_power = 3;
        ply_work.gmk_work0 = capsule_pos_x;
        ply_work.gmk_work1 = capsule_pos_y;
        ply_work.gmk_work2 = ply_work.obj_work.pos.x < capsule_pos_x ? 1 : 0;
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void gmPlySeqBossGoalPre(GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag |= 4194304U;
        ply_work.rect_work[0].def_power = 3;
        ply_work.water_timer = 0;
        if (((int) ply_work.obj_work.move_flag & 1) == 0 || ((int) ply_work.obj_work.move_flag & 1) != 0 &&
            ply_work.obj_work.pos.y < ply_work.gmk_work1 - 98304)
        {
            if (((int) ply_work.obj_work.move_flag & 1) == 0 &&
                MTM_MATH_ABS(ply_work.obj_work.pos.x - ply_work.gmk_work0) > 262144)
                return;
            if (ply_work.gmk_work2 != 0)
            {
                ply_work.key_on |= 4;
                ply_work.key_walk_rot_z = -32767;
            }
            else
            {
                ply_work.key_on |= 8;
                ply_work.key_walk_rot_z = short.MaxValue;
            }
        }
        else
        {
            ply_work.player_flag &= 4292870143U;
            GmPlySeqChangeSequence(ply_work, 25);
        }
    }

    private static void GmPlySeqInitBossGaol(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 1024) != 0)
            return;
        GmPlayerStateInit(ply_work);
        ply_work.player_flag |= 4194304U;
        ply_work.rect_work[0].def_power = 3;
        ply_work.water_timer = 0;
        GmPlayerActionChange(ply_work, 0);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.user_timer = 245760;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqBossGoalMain);
    }

    private static void gmPlySeqBossGoalMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state == 0)
        {
            ply_work.obj_work.user_timer = ObjTimeCountDown(ply_work.obj_work.user_timer);
            if (ply_work.obj_work.user_timer != 0)
                return;
            if (((int) ply_work.obj_work.disp_flag & 1) != 0)
                GmPlySeqSetProgramTurn(ply_work, 4096);
            GmPlayerActionChange(ply_work, 54);
        }
        else
        {
            if (((int) ply_work.obj_work.disp_flag & 8) == 0 || ply_work.act_state != 54)
                return;
            GmPlayerActionChange(ply_work, 55);
            ply_work.obj_work.disp_flag |= 4U;
        }
    }

    private static void GmPlySeqChangeBoss5Demo(
        GMS_PLAYER_WORK ply_work,
        int dest_pos_x,
        bool is_goal)
    {
        if ((ply_work.player_flag & 1024) != 0)
            return;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        GmPlySeqLandingSet(ply_work, 0);
        ply_work.player_flag |= 1077936128U;
        if (is_goal)
        {
            SaveState.deleteSave();
            ply_work.player_flag |= 16777216U;
            ply_work.rect_work[0].def_power = 3;
        }

        ply_work.gmk_work0 = dest_pos_x;
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void gmPlySeqBoss5DemoPre(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.pos.x >= ply_work.gmk_work0)
        {
            if (ply_work.obj_work.spd.x != 0)
                return;
            ply_work.player_flag &= 3221225471U;
            GmPlySeqChangeSequence(ply_work, 26);
        }
        else
        {
            ply_work.key_on |= 8;
            ply_work.key_walk_rot_z = short.MaxValue;
        }
    }

    private static void GmPlySeqInitBoss5Demo(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 1024) != 0)
            return;
        GmPlayerStateInit(ply_work);
        ply_work.player_flag |= 4194304U;
        if (ply_work.act_state != 0)
        {
            GmPlayerActionChange(ply_work, 0);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.seq_func = new seq_func_delegate(gmPlySeqBoss5DemoMain);
    }

    private static void gmPlySeqBoss5DemoMain(GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlySeqChangeBoss5DemoEnd(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 1024) != 0)
            return;
        ply_work.player_flag &= 3217031167U;
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqChangeTRetryFw(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequence(ply_work, 27);
    }

    private static void GmPlySeqInitTRetryFw(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 131072) != 0)
            GmPlayerSetEndPinballSonic(ply_work);
        if ((ply_work.player_flag & 262144) != 0)
            GmPlayerSetEndTruckRide(ply_work);
        GmPlayerSpdParameterSet(ply_work);
        ply_work.obj_work.dir.x = 0;
        ply_work.obj_work.dir.y = 0;
        ply_work.obj_work.dir.z = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd.z = 0;
        ply_work.obj_work.disp_flag &= 4294967292U;
        GmPlayerActionChange(ply_work, 4);
        ply_work.player_flag &= 4293918719U;
        ply_work.obj_work.spd_m = 0;
        ply_work.water_timer = 0;
        ply_work.rect_work[0].def_power = 3;
        ply_work.invincible_timer = 0;
        ply_work.obj_work.move_flag = 16192U;
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.flag |= 2U;
        ply_work.obj_work.dir_fall = 0;
        ply_work.ply_pseudofall_dir = 0;
        ply_work.jump_pseudofall_dir = 0;
        g_gm_main_system.pseudofall_dir = 0;
        ply_work.player_flag |= 4194304U;
        ply_work.player_flag &= 3220176895U;
        ply_work.obj_work.dir_slope = 192;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqTRetryFw);
    }

    private static void gmPlySeqTRetryFw(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.disp_flag & 8) != 0)
            GmPlayerActionChange(ply_work, 5);
        ply_work.water_timer = 0;
        ply_work.rect_work[0].def_power = 3;
    }

    private static void GmPlySeqChangeTRetryAcc(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequence(ply_work, 28);
    }

    private static void GmPlySeqInitTRetryAcc(GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag |= 512U;
        GmPlySeqMoveWalk(ply_work);
        GmPlayerWalkActionSet(ply_work);
        ply_work.obj_work.user_timer = 0;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqTRetryAcc);
    }

    private static void gmPlySeqTRetryAcc(GMS_PLAYER_WORK ply_work)
    {
        ++ply_work.obj_work.user_timer;
        ply_work.obj_work.spd_m += 512;
        if (ply_work.obj_work.user_timer > 100)
        {
            ply_work.seq_func = null;
            ply_work.obj_work.user_timer = 0;
            ply_work.obj_work.move_flag &= 4294959103U;
            ply_work.obj_work.move_flag &= 4294966271U;
            ply_work.obj_work.flag |= 16U;
        }

        if (ply_work.obj_work.spd_m > ply_work.spd4 - 512 && (ply_work.player_flag & 1048576) == 0)
        {
            ply_work.obj_work.dir.z = 4097;
            GmPlayerWalkActionSet(ply_work);
            GmPlayerWalkActionCheck(ply_work);
            ply_work.obj_work.dir.z = 0;
            GmPlySeqChangeTRetryRun(ply_work);
        }

        ply_work.water_timer = 0;
        ply_work.rect_work[0].def_power = 3;
    }

    private static void GmPlySeqChangeTRetryRun(GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag |= 1048576U;
    }

    private static void GmPlySeqInitTruckFw(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m != 0)
        {
            GmPlySeqChangeSequence(ply_work, 1);
        }
        else
        {
            if (((int) ply_work.obj_work.move_flag & 4194304) == 0)
            {
                ply_work.gmk_flag &= 4293918719U;
                GmPlayerActionChange(ply_work, 73);
            }
            else if (ply_work.obj_3d[g_gm_player_model_tbl[ply_work.char_id][0]].act_id[0] !=
                     g_gm_player_motion_right_tbl[ply_work.char_id][0] &&
                     ply_work.obj_3d[g_gm_player_model_tbl[ply_work.char_id][73]].act_id[0] !=
                     g_gm_player_motion_right_tbl[ply_work.char_id][73])
            {
                if (((int) ply_work.gmk_flag & 1048576) != 0)
                    GmPlayerActionChange(ply_work, 70);
                else
                    GmPlayerActionChange(ply_work, 69);
                ply_work.obj_work.disp_flag |= 4U;
            }

            ply_work.obj_work.move_flag &= 4294967279U;
            ply_work.obj_work.user_timer = 0;
            ply_work.obj_work.user_work = 0U;
            ply_work.seq_func = new seq_func_delegate(gmPlySeqTruckFwMain);
        }
    }

    private static void gmPlySeqTruckFwMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_3d[g_gm_player_model_tbl[ply_work.char_id][73]].act_id[0] ==
            g_gm_player_motion_right_tbl[ply_work.char_id][73] &&
            ((int) ply_work.obj_work.disp_flag & 8) != 0)
        {
            if (((int) ply_work.gmk_flag & 1048576) != 0)
                GmPlayerActionChange(ply_work, 70);
            else
                GmPlayerActionChange(ply_work, 69);
            ply_work.obj_work.disp_flag |= 4U;
        }

        if (ply_work.act_state == 69 || ply_work.act_state == 70)
        {
            if (((int) ply_work.obj_work.disp_flag & 8) == 0)
                return;
            int userWork = (int) ply_work.obj_work.user_work;
            ++ply_work.obj_work.user_work;
            if ((int) ply_work.obj_work.user_work < 8)
                return;
            GmPlayerActionChange(ply_work, 2);
            ply_work.obj_work.user_work = 0U;
        }
        else if (ply_work.act_state == 2 || ply_work.act_state == 4 || ply_work.act_state == 6)
        {
            if (((int) ply_work.obj_work.disp_flag & 8) == 0)
                return;
            GmPlayerActionChange(ply_work, ply_work.act_state + 1);
            ply_work.obj_work.disp_flag |= 4U;
            ply_work.obj_work.user_work = 0U;
        }
        else
        {
            if (ply_work.act_state != 3 || ((int) ply_work.obj_work.disp_flag & 8) == 0)
                return;
            int num = (int) ply_work.obj_work.user_work + 1;
            ply_work.obj_work.user_work = (uint) num;
            if ((int) ply_work.obj_work.user_work < 10)
                return;
            GmPlayerActionChange(ply_work, 4);
            ply_work.obj_work.user_work = 0U;
        }
    }

    private static void GmPlySeqInitTruckWalk(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.move_flag & 4194304) == 0)
            GmPlayerActionChange(ply_work, 73);
        else if (ply_work.act_state != 71 && ply_work.act_state != 72)
        {
            if (((int) ply_work.gmk_flag & 1048576) != 0)
                GmPlayerActionChange(ply_work, 72);
            else
                GmPlayerActionChange(ply_work, 71);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqTruckWalkMain);
        ply_work.obj_work.user_timer = 0;
        ply_work.truck_left_flip_timer = 245760;
    }

    private static void gmPlySeqTruckWalkMain(GMS_PLAYER_WORK ply_work)
    {
        bool flag = false;
        if (ply_work.obj_work.spd_m < 0 && ply_work.act_state == 71)
        {
            ply_work.truck_left_flip_timer = ObjTimeCountDown(ply_work.truck_left_flip_timer);
            if (ply_work.truck_left_flip_timer == 0)
            {
                ply_work.gmk_flag |= 1048576U;
                flag = true;
            }
        }
        else
        {
            ply_work.truck_left_flip_timer = 245760;
            if (ply_work.obj_work.spd_m >= 0 && ply_work.act_state == 72)
            {
                ply_work.gmk_flag &= 4293918719U;
                flag = true;
            }
        }

        if (flag)
        {
            float num = ply_work.obj_work.obj_3d.frame[0];
            if (((int) ply_work.gmk_flag & 1048576) != 0)
                GmPlayerActionChange(ply_work, 72);
            else
                GmPlayerActionChange(ply_work, 71);
            ply_work.obj_work.disp_flag |= 4U;
            ply_work.obj_work.obj_3d.frame[0] = ply_work.obj_work.obj_3d.frame[1] = num;
        }

        if (ply_work.obj_3d[g_gm_player_model_tbl[ply_work.char_id][73]].act_id[0] !=
            g_gm_player_motion_right_tbl[ply_work.char_id][73] ||
            ((int) ply_work.obj_work.disp_flag & 8) == 0)
            return;
        if (ply_work.obj_work.spd_m >= 0)
        {
            ply_work.gmk_flag &= 4293918719U;
            GmPlayerActionChange(ply_work, 71);
        }
        else if (ply_work.obj_work.spd_m < 0)
        {
            ply_work.gmk_flag |= 1048576U;
            GmPlayerActionChange(ply_work, 72);
        }

        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlySeqInitTruckFall(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_3d[g_gm_player_model_tbl[ply_work.char_id][40]].act_id[0] !=
            g_gm_player_motion_right_tbl[ply_work.char_id][40])
        {
            GmPlayerActionChange(ply_work, 40);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 49296U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.gmk_flag &= 1073479679U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqTruckJumpMain);
        ushort num = (ushort) (ply_work.obj_work.dir.z + (uint) ply_work.obj_work.dir_fall -
                               ply_work.jump_pseudofall_dir);
        ply_work.obj_work.spd.x = FX_Mul(ply_work.obj_work.spd_m, mtMathCos(num));
        ply_work.obj_work.spd.y = FX_Mul(ply_work.obj_work.spd_m, mtMathSin(num));
        ply_work.player_flag &= 4294967280U;
        ply_work.player_flag |= 1U;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
    }

    private static void GmPlySeqInitTruckJump(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_3d[g_gm_player_model_tbl[ply_work.char_id][40]].act_id[0] !=
            g_gm_player_motion_right_tbl[ply_work.char_id][40])
        {
            GmPlayerActionChange(ply_work, 40);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag |= 49168U;
        ply_work.obj_work.move_flag &= 4290772990U;
        ushort z = ply_work.obj_work.dir.z;
        if ((z + 256 & 8192) != 0 && (z + 256 & 4095) <= 1024)
        {
            if (ply_work.obj_work.spd_m > 0 && z < 32768)
                z -= 1152;
            else if (ply_work.obj_work.spd_m < 0 && z > 32768)
                z += 1152;
        }

        ushort num1 = (ushort) (z + (uint) ply_work.obj_work.dir_fall - ply_work.jump_pseudofall_dir);
        ply_work.obj_work.spd.x = FX_Mul(ply_work.obj_work.spd_m, mtMathCos(num1));
        ply_work.obj_work.spd.y = FX_Mul(ply_work.obj_work.spd_m, mtMathSin(num1));
        ushort num2 = (ushort) (ply_work.obj_work.dir.z + (uint) ply_work.obj_work.dir_fall -
                                ply_work.jump_pseudofall_dir);
        ply_work.obj_work.spd.x += FX_Mul(ply_work.spd_jump, mtMathSin(num2));
        ply_work.obj_work.spd.y += FX_Mul(-ply_work.spd_jump, mtMathCos(num2));
        ply_work.player_flag &= 4294967280U;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
        GmPlySeqSetJumpState(ply_work, 0, 0U);
        ply_work.seq_func = new seq_func_delegate(gmPlySeqTruckJumpMain);
        GmPlayerSetAtk(ply_work);
        GmSoundPlaySE("Lorry3");
    }

    private static void gmPlySeqTruckJumpMain(GMS_PLAYER_WORK ply_work)
    {
        int y = ply_work.obj_work.spd.y;
        if (ply_work.obj_work.user_timer != 0)
        {
            --ply_work.obj_work.user_timer;
            if (ply_work.obj_work.user_timer == 0)
                ply_work.obj_work.move_flag |= 128U;
        }

        if ((ply_work.player_flag & 5) == 0 && !GmPlayerKeyCheckJumpKeyOn(ply_work) && y < -16384)
            ply_work.player_flag |= 4U;
        if ((ply_work.player_flag & 4) != 0 && ply_work.obj_work.spd.y < 0)
            ply_work.obj_work.spd.y += ply_work.obj_work.spd_fall;
        if (((int) ply_work.obj_work.move_flag & 6) == 0)
        {
            bool flag = false;
            if (MTM_MATH_ABS(ply_work.obj_work.spd.x) < 4096)
                flag = true;
            if (flag)
            {
                ushort num = (ushort) (ply_work.obj_work.dir_fall -
                                       (uint) g_gm_main_system.pseudofall_dir);
                ply_work.obj_work.spd.x += mtMathSin(num);
            }
        }

        if (((int) ply_work.obj_work.move_flag & 2) != 0)
            ply_work.obj_work.spd.y += ply_work.obj_work.spd_fall * 5;
        if (((int) ply_work.obj_work.move_flag & 1) == 0)
            return;
        GmPlySeqLandingSet(ply_work, 0);
        GmSoundPlaySE("Lorry4");
        GmPlySeqChangeSequence(ply_work, 0);
        GMM_PAD_VIB_MID();
    }

    private static void GmPlySeqInitTruckSquatStart(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 14);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqTruckSquatMain);
    }

    private static void GmPlySeqInitTruckSquatMiddle(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 15);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < 4096)
            ply_work.obj_work.spd_m = 0;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqTruckSquatMain);
    }

    private static void GmPlySeqInitTruckSquatEnd(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 16);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqSquatEndMain);
    }

    private static void gmPlySeqTruckSquatMain(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m != 0)
        {
            GmPlySeqChangeSequence(ply_work, 1);
        }
        else
        {
            if (ply_work.act_state != 14 || ((int) ply_work.obj_work.disp_flag & 8) == 0)
                return;
            GmPlySeqChangeSequence(ply_work, 7);
        }
    }

    private static void GmPlySeqInitTruckStaggerFront(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 33);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqTruckStaggerMain);
        ply_work.obj_work.user_timer = 0;
        GmPlyEfctCreateSweat(ply_work);
    }

    private static void GmPlySeqInitTruckStaggerBack(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerActionChange(ply_work, 34);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqTruckStaggerMain);
        ply_work.obj_work.user_timer = 0;
        GmPlyEfctCreateSweat(ply_work);
    }

    private static void gmPlySeqTruckStaggerMain(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.gmk_flag & 262144) == 0)
            GmPlySeqChangeSequence(ply_work, 0);
        else if (ply_work.seq_state == 13)
        {
            ply_work.obj_work.user_timer += 1024;
            if (ply_work.obj_work.user_timer <= 8192)
                return;
            ply_work.obj_work.user_timer = 8192;
        }
        else
        {
            ply_work.obj_work.user_timer -= 1024;
            if (ply_work.obj_work.user_timer >= -8192)
                return;
            ply_work.obj_work.user_timer = -8192;
        }
    }

    private static void GmPlySeqLandingSet(GMS_PLAYER_WORK ply_work, ushort dir_z)
    {
        GmPlayerSpdParameterSet(ply_work);
        ply_work.obj_work.move_flag &= 4294934511U;
        ply_work.obj_work.move_flag |= 128U;
        ply_work.obj_work.disp_flag &= 4294967263U;
        ply_work.gmk_flag &= 4278190079U;
        ply_work.gmk_flag2 &= 4294967007U;
        ply_work.gmk_flag2 &= 4294966783U;
        ply_work.player_flag &= 4294967135U;
        ply_work.no_jump_move_timer = 0;
        ply_work.score_combo_cnt = 0U;
        if (((int) ply_work.gmk_flag & 1) == 0 && ((int) ply_work.obj_work.col_flag & 1) != 0)
            ply_work.obj_work.dir.z = 0;
        if (dir_z > 0)
        {
            if ((ply_work.player_flag & 262144) != 0)
            {
                if (((int) ply_work.gmk_flag2 & 1) != 0)
                {
                    ply_work.obj_work.spd_m += FX_Mul(FX_Mul(ply_work.obj_work.move.x, 2048),
                        mtMathCos(
                            (ushort) (dir_z - (uint) g_gm_main_system.pseudofall_dir)));
                    ply_work.obj_work.spd_m += FX_Mul(FX_Mul(ply_work.obj_work.move.y, 2048),
                        mtMathSin(
                            (ushort) (dir_z - (uint) g_gm_main_system.pseudofall_dir)));
                }
                else
                {
                    ply_work.obj_work.spd_m += FX_Mul(ply_work.obj_work.move.x,
                        mtMathCos(
                            (ushort) (dir_z - (uint) g_gm_main_system.pseudofall_dir)));
                    ply_work.obj_work.spd_m += FX_Mul(ply_work.obj_work.move.y,
                        mtMathSin(
                            (ushort) (dir_z - (uint) g_gm_main_system.pseudofall_dir)));
                }
            }
            else
            {
                ply_work.obj_work.spd_m += FX_Mul(ply_work.obj_work.move.x, mtMathCos(dir_z));
                ply_work.obj_work.spd_m += FX_Mul(ply_work.obj_work.move.y, mtMathSin(dir_z));
                if (ObjObjectDirFallReverseCheck(ply_work.obj_work.dir_fall) != 0U)
                    ply_work.obj_work.spd_m = -ply_work.obj_work.spd_m;
            }
        }
        else if ((ply_work.player_flag & 262144) != 0)
        {
            if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < MTM_MATH_ABS(ply_work.obj_work.spd.x))
                ply_work.obj_work.spd_m = ply_work.obj_work.spd.x;
            ply_work.obj_work.spd_m += FX_Mul(MTM_MATH_ABS(ply_work.obj_work.spd.x),
                mtMathSin((ushort) (ply_work.obj_work.dir.z +
                                    (ply_work.obj_work.dir_fall -
                                     (uint) g_gm_main_system.pseudofall_dir))));
        }
        else
        {
            if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < MTM_MATH_ABS(ply_work.obj_work.spd.x))
                ply_work.obj_work.spd_m = ply_work.obj_work.spd.x;
            ply_work.obj_work.spd_m += FX_Mul(MTM_MATH_ABS(ply_work.obj_work.spd.x),
                mtMathSin(ply_work.obj_work.dir.z));
        }

        ply_work.gmk_flag2 &= 4294967294U;
        ply_work.spd_work_max = MTM_MATH_ABS(ply_work.obj_work.spd_m);
        if (ply_work.spd_work_max > ply_work.obj_work.spd_slope_max)
            ply_work.spd_work_max = ply_work.obj_work.spd_slope_max;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        if (dir_z != 0)
            ply_work.obj_work.dir.z = dir_z;
        if (((int) ply_work.gmk_flag & 4096) == 0)
            ply_work.obj_work.dir.x = 0;
        if ((ply_work.player_flag & 262144) == 0)
            return;
        ply_work.obj_work.move_flag &= 4294950911U;
    }

    private static void GmPlySeqMoveFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK) obj_work;
        GMS_PLY_SEQ_STATE_DATA[] seqStateDataTbl = ply_work.seq_state_data_tbl;
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & int.MinValue) != 0)
        {
            if ((ply_work.player_flag & 32768) != 0)
                GmPlySeqMoveWalkAutoRun(ply_work);
            else if ((ply_work.player_flag & 262144) != 0)
                GmPlySeqMoveWalkTruck(ply_work);
            else
                GmPlySeqMoveWalk(ply_work);
        }

        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 1073741824) != 0 &&
            (ply_work.player_flag & 32) == 0)
        {
            if ((ply_work.player_flag & 32768) != 0)
                GmPlySeqMoveJumpAutoRun(ply_work);
            else if ((ply_work.player_flag & 262144) != 0)
                GmPlySeqMoveJumpTruck(ply_work);
            else
                GmPlySeqMoveJump(ply_work);
        }

        if (ply_work.no_jump_move_timer != 0)
        {
            ply_work.no_jump_move_timer = ObjTimeCountDown(ply_work.no_jump_move_timer);
            if (ply_work.no_jump_move_timer == 0)
                ply_work.player_flag &= 4294967263U;
        }

        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 536870912) != 0)
            GmPlySeqMoveSpin(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 134217728) != 0)
            GmPlySeqMoveSpinNoDec(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 67108864) != 0)
            GmPlySeqMoveSpinPinball(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 268435456) != 0)
        {
            if ((ply_work.player_flag & 262144) != 0)
                GmPlySeqTruckJumpDirec(ply_work);
            else if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
                gmPlySeqSplJumpDirec(ply_work);
            else
                GmPlySeqJumpDirec(ply_work);
        }

        if ((ply_work.player_flag & 32768) != 0 && (ply_work.player_flag & 1024) == 0 &&
            (obj_work.pos.x <= g_obj.camera[0][0] + 65536 &&
             obj_work.pos.x > g_obj.camera[0][0] + 65536 - 4194304))
        {
            if (((int) obj_work.move_flag & 16) != 0)
            {
                if (obj_work.spd.x < ply_work.scroll_spd_x)
                {
                    obj_work.spd.x = ply_work.scroll_spd_x;
                    if (((int) obj_work.disp_flag & 1) != 0)
                        GmPlySeqSetProgramTurn(ply_work, 4096);
                }
            }
            else if (obj_work.spd_m < ply_work.scroll_spd_x)
            {
                obj_work.spd_m = ply_work.scroll_spd_x;
                if (3 <= ply_work.seq_state && ply_work.seq_state <= 5)
                    GmPlySeqChangeSequence(ply_work, 0);
                if (((int) obj_work.disp_flag & 1) != 0)
                {
                    if (ply_work.seq_state == 10)
                    {
                        GmPlayerSetReverse(ply_work);
                        GmPlySeqChangeSequence(ply_work, 0);
                    }
                    else if (6 <= ply_work.seq_state && ply_work.seq_state <= 8)
                        GmPlySeqSetProgramTurn(ply_work, 4096);
                    else
                        GmPlySeqChangeSequence(ply_work, 2);
                }
            }
        }

        if ((ply_work.player_flag & 262144) != 0)
        {
            if (((int) obj_work.move_flag & 1) != 0 && ((int) ply_work.gmk_flag & 262144) != 0)
                return;
            gmPlySeqTruckMove(obj_work);
        }
        else if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            gmPlySeqSplMove(obj_work);
        else
            ObjObjectMove(obj_work);
    }

    private static void GmPlySeqMoveWalk(GMS_PLAYER_WORK ply_work)
    {
        int spdAdd = ply_work.spd_add;
        int fSpd = ply_work.spd_dec;
        int num1 = ply_work.spd_max;
        if (GmPlayerKeyCheckWalkRight(ply_work) || GmPlayerKeyCheckWalkLeft(ply_work))
        {
            int num2 = MTM_MATH_ABS(ply_work.key_walk_rot_z);
            if (num2 > 24576)
                num2 = 24576;
            num1 = num1 * num2 / 24576;
        }

        if (num1 < ply_work.prev_walk_roll_spd_max)
        {
            num1 = ply_work.prev_walk_roll_spd_max - fSpd;
            if (num1 < 0)
                num1 = 0;
        }

        ply_work.prev_walk_roll_spd_max = num1;
        if (ply_work.obj_work.dir.z > 0)
        {
            int num2 = FX_Mul(ply_work.spd_max_add_slope, mtMathSin(ply_work.obj_work.dir.z));
            if (num2 > 0)
                num1 += num2;
        }

        if (ply_work.no_spddown_timer != 0)
        {
            fSpd = 0;
        }
        else
        {
            int v2 = 0;
            if (MTM_MATH_ABS(ply_work.obj_work.spd_m) > ply_work.spd3)
            {
                int num2;
                if (num1 - ply_work.spd3 != 0)
                {
                    num2 = FX_Div(MTM_MATH_ABS(ply_work.obj_work.spd_m) - ply_work.spd3,
                        num1 - ply_work.spd3);
                    if (num2 > 4096)
                        num2 = 4096;
                }
                else
                    num2 = 4096;

                v2 = num2 * 3968 >> 12;
            }

            spdAdd -= FX_Mul(spdAdd, v2);
        }

        if ((ply_work.player_flag & 67108864) != 0)
        {
            GMD_PLAYER_WATER_SET(ref spdAdd);
            GMD_PLAYER_WATER_SET(ref fSpd);
        }

        if (ply_work.spd_work_max >= num1 && MTM_MATH_ABS(ply_work.obj_work.spd_m) >= num1)
        {
            if (ply_work.spd_work_max > ply_work.obj_work.spd_m)
                ply_work.spd_work_max = MTM_MATH_ABS(ply_work.obj_work.spd_m);
            num1 = ply_work.spd_work_max;
        }

        if ((ply_work.player_flag & 32768) != 0 && GmPlayerKeyCheckWalkRight(ply_work) &&
            num1 > ply_work.scroll_spd_x + 8192)
            num1 = ply_work.scroll_spd_x + 8192;
        if (GmPlayerKeyCheckWalkLeft(ply_work) | GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = ObjSpdUpSet(ply_work.obj_work.spd_m, spdAdd, num1);
            }
            else
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = ObjSpdUpSet(ply_work.obj_work.spd_m, -spdAdd, num1);
            }
        }
        else
        {
            ply_work.spd_pool = 0;
            ply_work.obj_work.spd.x = MTM_MATH_CLIP(ply_work.obj_work.spd.x, -num1, num1);
            ply_work.obj_work.spd_m = MTM_MATH_CLIP(ply_work.obj_work.spd_m, -num1, num1);
            if ((ply_work.obj_work.dir.z + 8192 & 65280) > 16384)
                return;
            if ((ply_work.player_flag & 134217728) != 0)
                ply_work.player_flag &= 4160749567U;
            else if ((ply_work.player_flag & 32768) != 0)
            {
                if (((int) ply_work.obj_work.disp_flag & 1) == 0 && ply_work.seq_state == 1)
                {
                    int num2 = ply_work.scroll_spd_x - 4096;
                    if (num2 < 0)
                        num2 = 0;
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                    if (ply_work.obj_work.spd_m >= num2)
                        return;
                    ply_work.obj_work.spd_m = num2;
                }
                else
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
            }
            else
                ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
        }
    }

    private static void GmPlySeqMoveWalkTruck(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.gmk_flag & 262144) != 0)
            return;
        int spdAdd = ply_work.spd_add;
        int fSpd = ply_work.spd_dec;
        int num1 = ply_work.spd_max;
        ushort num2 = (ushort) (ply_work.obj_work.dir.z +
                                (ply_work.obj_work.dir_fall - (uint) g_gm_main_system.pseudofall_dir));
        if (num2 != 0)
        {
            int num3 = FX_Mul(ply_work.spd_max_add_slope, mtMathSin(num2));
            if (num3 > 0)
                num1 += num3;
        }

        if (ply_work.no_spddown_timer != 0)
        {
            fSpd = 0;
        }
        else
        {
            int v2 = 0;
            if (MTM_MATH_ABS(ply_work.obj_work.spd_m) > ply_work.spd3)
            {
                int num3;
                if (num1 - ply_work.spd3 != 0)
                {
                    num3 = FX_Div(MTM_MATH_ABS(ply_work.obj_work.spd_m) - ply_work.spd3,
                        num1 - ply_work.spd3);
                    if (num3 > 4096)
                        num3 = 4096;
                }
                else
                    num3 = 4096;

                v2 = num3 * 3968 >> 12;
            }

            spdAdd -= FX_Mul(spdAdd, v2);
        }

        if ((ply_work.player_flag & 67108864) != 0)
        {
            GMD_PLAYER_WATER_SET(ref spdAdd);
            GMD_PLAYER_WATER_SET(ref fSpd);
        }

        if (ply_work.spd_work_max >= num1 && MTM_MATH_ABS(ply_work.obj_work.spd_m) >= num1)
        {
            if (ply_work.spd_work_max > ply_work.obj_work.spd_m)
                ply_work.spd_work_max = MTM_MATH_ABS(ply_work.obj_work.spd_m);
            num1 = ply_work.spd_work_max;
        }

        if ((((int) g_gm_main_system.game_flag & 1048576) != 0 ||
             (ply_work.player_flag & 16777216) != 0) && GmPlayerKeyCheckWalkLeft(ply_work) |
            GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = ObjSpdUpSet(ply_work.obj_work.spd_m, spdAdd, num1);
            }
            else
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = ObjSpdUpSet(ply_work.obj_work.spd_m, -spdAdd, num1);
            }
        }
        else
        {
            if ((num2 + ply_work.obj_work.dir_slope & ushort.MaxValue) >=
                ply_work.obj_work.dir_slope << 1)
                return;
            ply_work.spd_pool = 0;
            ply_work.obj_work.spd.x = MTM_MATH_CLIP(ply_work.obj_work.spd.x, -num1, num1);
            ply_work.obj_work.spd_m = MTM_MATH_CLIP(ply_work.obj_work.spd_m, -num1, num1);
            if ((num2 + 2048 & 65280) > 4096)
                return;
            if ((ply_work.player_flag & 134217728) != 0)
                ply_work.player_flag &= 4160749567U;
            else
                ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
        }
    }

    private static void GmPlySeqMoveWalkAutoRun(GMS_PLAYER_WORK ply_work)
    {
        int spdAdd = ply_work.spd_add;
        int fSpd = ply_work.spd_dec;
        int spdMax = ply_work.spd_max;
        int num1 = FX_F32_TO_FX32(9.5f);
        if (GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (ply_work.obj_work.spd_m <= ply_work.spd3)
                spdAdd >>= 2;
            if (ply_work.obj_work.spd_m < ply_work.scroll_spd_x + FX_F32_TO_FX32(0.25f))
                ply_work.obj_work.spd_m = ply_work.scroll_spd_x + FX_F32_TO_FX32(0.25f);
            if (ply_work.obj_work.spd_m < FX_F32_TO_FX32(8.4f))
                ply_work.obj_work.spd_m = FX_F32_TO_FX32(8.4f);
            if (ply_work.obj_work.spd_m > FX_F32_TO_FX32(8.7f))
                ply_work.obj_work.spd_m = FX_F32_TO_FX32(8.7f);
        }

        if (GmPlayerKeyCheckWalkRight(ply_work) || GmPlayerKeyCheckWalkLeft(ply_work))
        {
            int num2 = MTM_MATH_ABS(ply_work.key_walk_rot_z);
            if (num2 > 24576)
                num2 = 24576;
            num1 = num1 * num2 / 24576;
        }

        if (num1 < ply_work.prev_walk_roll_spd_max)
        {
            num1 = ply_work.prev_walk_roll_spd_max - fSpd;
            if (num1 < 0)
                num1 = 0;
        }

        ply_work.prev_walk_roll_spd_max = num1;
        if (ply_work.obj_work.dir.z != 0)
        {
            int num2 = FX_Mul(ply_work.spd_max_add_slope, mtMathSin(ply_work.obj_work.dir.z));
            if (num2 > 0)
                num1 += num2;
        }

        if (ply_work.no_spddown_timer != 0)
        {
            fSpd = 0;
        }
        else
        {
            int v2 = 0;
            if (MTM_MATH_ABS(ply_work.obj_work.spd_m) > ply_work.spd3)
            {
                int num2;
                if (num1 - ply_work.spd3 != 0)
                {
                    num2 = FX_Div(MTM_MATH_ABS(ply_work.obj_work.spd_m) - ply_work.spd3,
                        num1 - ply_work.spd3);
                    if (num2 > 4096)
                        num2 = 4096;
                }
                else
                    num2 = 4096;

                v2 = num2 * 3968 >> 12;
            }

            spdAdd -= FX_Mul(spdAdd, v2);
        }

        if ((ply_work.player_flag & 67108864) != 0)
        {
            GMD_PLAYER_WATER_SET(ref spdAdd);
            GMD_PLAYER_WATER_SET(ref fSpd);
        }

        if (ply_work.spd_work_max >= num1 && MTM_MATH_ABS(ply_work.obj_work.spd_m) >= num1)
        {
            if (ply_work.spd_work_max > ply_work.obj_work.spd_m)
                ply_work.spd_work_max = MTM_MATH_ABS(ply_work.obj_work.spd_m);
            num1 = ply_work.spd_work_max;
        }

        if ((ply_work.player_flag & 32768) != 0)
        {
            ply_work.spd_work_max += 8192;
            num1 = ply_work.spd_work_max + 8192;
        }

        if (GmPlayerKeyCheckWalkLeft(ply_work) | GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = ObjSpdUpSet(ply_work.obj_work.spd_m, spdAdd, num1);
            }
            else
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = ObjSpdUpSet(ply_work.obj_work.spd_m, -spdAdd, num1);
            }
        }
        else
        {
            ply_work.spd_pool = 0;
            ply_work.obj_work.spd.x = MTM_MATH_CLIP(ply_work.obj_work.spd.x, -num1, num1);
            ply_work.obj_work.spd_m = MTM_MATH_CLIP(ply_work.obj_work.spd_m, -num1, num1);
            if ((ply_work.obj_work.dir.z + 8192 & 65280) > 16384)
                return;
            if ((ply_work.player_flag & 134217728) != 0)
                ply_work.player_flag &= 4160749567U;
            else if ((ply_work.player_flag & 32768) != 0)
            {
                if (((int) ply_work.obj_work.disp_flag & 1) == 0 && ply_work.seq_state == 1)
                {
                    int num2 = ply_work.scroll_spd_x - 4096;
                    if (num2 < 0)
                        num2 = 0;
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                    if (ply_work.obj_work.spd_m >= num2)
                        return;
                    ply_work.obj_work.spd_m = num2;
                }
                else
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
            }
            else
                ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
        }
    }

    private static void GmPlySeqMoveJump(GMS_PLAYER_WORK ply_work)
    {
        int spdJumpAdd = ply_work.spd_jump_add;
        int fSpd1 = ply_work.spd_jump_dec;
        int spdJumpDec = ply_work.spd_jump_dec;
        int spdJumpMax = ply_work.spd_jump_max;
        ply_work.spd_work_max = 0;
        if ((ply_work.obj_work.dir.z + 8192 & 49152) != 0 || ply_work.obj_work.dir.z == 57344)
            fSpd1 >>= 2;
        int fSpd2;
        if (ply_work.no_spddown_timer != 0)
        {
            fSpd1 = 0;
            fSpd2 = spdJumpAdd >> 2;
        }
        else
        {
            int v2 = 0;
            if (MTM_MATH_ABS(ply_work.obj_work.spd.x) > ply_work.spd2)
            {
                int num;
                if (spdJumpMax - ply_work.spd2 != 0)
                {
                    num = FX_Div(MTM_MATH_ABS(ply_work.obj_work.spd.x) - ply_work.spd2,
                        spdJumpMax - ply_work.spd2);
                    if (num > 4096)
                        num = 4096;
                }
                else
                    num = 4096;

                v2 = num * 3968 >> 12;
            }

            fSpd2 = spdJumpAdd - FX_Mul(spdJumpAdd, v2);
        }

        if ((ply_work.player_flag & 67108864) != 0)
        {
            GMD_PLAYER_WATER_SET(ref fSpd2);
            GMD_PLAYER_WATER_SET(ref fSpd1);
        }

        int sSpd = FX_Mul(fSpd1, 4096);
        FX_Mul(spdJumpDec, 4096);
        if (GmPlayerKeyCheckWalkLeft(ply_work) | GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd.x < 0)
                    ply_work.obj_work.spd.x = ObjSpdDownSet(ply_work.obj_work.spd.x, sSpd);
                ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = ObjSpdUpSet(ply_work.obj_work.spd.x, fSpd2, spdJumpMax);
            }
            else
            {
                if (ply_work.obj_work.spd.x > 0)
                    ply_work.obj_work.spd.x = ObjSpdDownSet(ply_work.obj_work.spd.x, sSpd);
                ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = ObjSpdUpSet(ply_work.obj_work.spd.x, -fSpd2, spdJumpMax);
            }
        }
        else
        {
            ply_work.obj_work.spd.x = MTM_MATH_CLIP(ply_work.obj_work.spd.x, -spdJumpMax, spdJumpMax);
            ply_work.obj_work.spd_m = MTM_MATH_CLIP(ply_work.obj_work.spd_m, -spdJumpMax, spdJumpMax);
            ply_work.spd_pool = 0;
            ply_work.obj_work.spd.x = ObjSpdDownSet(ply_work.obj_work.spd.x, fSpd1);
            ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
        }
    }

    private static void GmPlySeqMoveJumpTruck(GMS_PLAYER_WORK ply_work)
    {
        int spdJumpAdd = ply_work.spd_jump_add;
        int fSpd1 = ply_work.spd_jump_dec;
        int spdJumpDec = ply_work.spd_jump_dec;
        int spdJumpMax = ply_work.spd_jump_max;
        ply_work.spd_work_max = 0;
        if ((ply_work.obj_work.dir.z + 8192 & 49152) != 0 || ply_work.obj_work.dir.z == 57344)
            fSpd1 >>= 2;
        int fSpd2;
        if (ply_work.no_spddown_timer != 0)
        {
            fSpd1 = 0;
            fSpd2 = spdJumpAdd >> 2;
        }
        else
        {
            int v2 = 0;
            if (MTM_MATH_ABS(ply_work.obj_work.spd.x) > ply_work.spd2)
            {
                int num;
                if (spdJumpMax - ply_work.spd2 != 0)
                {
                    num = FX_Div(MTM_MATH_ABS(ply_work.obj_work.spd.x) - ply_work.spd2,
                        spdJumpMax - ply_work.spd2);
                    if (num > 4096)
                        num = 4096;
                }
                else
                    num = 4096;

                v2 = num * 3968 >> 12;
            }

            fSpd2 = spdJumpAdd - FX_Mul(spdJumpAdd, v2);
        }

        if ((ply_work.player_flag & 67108864) != 0)
        {
            GMD_PLAYER_WATER_SET(ref fSpd2);
            GMD_PLAYER_WATER_SET(ref fSpd1);
        }

        int num1 = FX_Mul(fSpd1, 4096);
        int v1 = FX_Mul(spdJumpDec, 4096);
        int num2 = 12288;
        if (((int) ply_work.gmk_flag2 & 512) != 0)
        {
            ushort num3 = (ushort) (ply_work.obj_work.dir_fall - (uint) g_gm_main_system.pseudofall_dir);
            ply_work.obj_work.spd.x += mtMathSin(num3) / 3;
            if (ply_work.obj_work.spd.x > num2)
            {
                ply_work.obj_work.spd.x = num2;
            }
            else
            {
                if (ply_work.obj_work.spd.x >= -num2)
                    return;
                ply_work.obj_work.spd.x = -num2;
            }
        }
        else if (((int) ply_work.gmk_flag2 & 1) != 0)
        {
            int sSpd1 = FX_Mul(num1, 3072);
            int sSpd2 = FX_Mul(v1, 3072);
            ply_work.obj_work.spd.x = MTM_MATH_CLIP(ply_work.obj_work.spd.x, -spdJumpMax, spdJumpMax);
            ply_work.obj_work.spd_m = MTM_MATH_CLIP(ply_work.obj_work.spd_m, -spdJumpMax, spdJumpMax);
            ply_work.spd_pool = 0;
            ply_work.obj_work.spd.x = ObjSpdDownSet(ply_work.obj_work.spd.x, sSpd1);
            ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd2);
        }
        else if (GmPlayerKeyCheckWalkLeft(ply_work) | GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd.x < 0)
                    ply_work.obj_work.spd.x = ObjSpdDownSet(ply_work.obj_work.spd.x, num1);
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = ObjSpdUpSet(ply_work.obj_work.spd.x, fSpd2, spdJumpMax);
            }
            else
            {
                if (ply_work.obj_work.spd.x > 0)
                    ply_work.obj_work.spd.x = ObjSpdDownSet(ply_work.obj_work.spd.x, num1);
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = ObjSpdUpSet(ply_work.obj_work.spd.x, -fSpd2, spdJumpMax);
            }
        }
        else
        {
            ply_work.obj_work.spd.x = MTM_MATH_CLIP(ply_work.obj_work.spd.x, -spdJumpMax, spdJumpMax);
            ply_work.obj_work.spd_m = MTM_MATH_CLIP(ply_work.obj_work.spd_m, -spdJumpMax, spdJumpMax);
            ply_work.spd_pool = 0;
            ply_work.obj_work.spd.x = ObjSpdDownSet(ply_work.obj_work.spd.x, fSpd1);
            ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
        }
    }

    private static void GmPlySeqMoveJumpAutoRun(GMS_PLAYER_WORK ply_work)
    {
        int v1 = ply_work.spd_jump_add;
        int fSpd1 = ply_work.spd_jump_dec;
        int spdJumpDec = ply_work.spd_jump_dec;
        int spdJumpMax = ply_work.spd_jump_max;
        ply_work.spd_work_max = 0;
        if (GmPlayerKeyCheckWalkRight(ply_work))
            v1 = 0;
        if ((ply_work.obj_work.dir.z + 8192 & 49152) != 0 || ply_work.obj_work.dir.z == 57344)
            fSpd1 >>= 2;
        int fSpd2;
        if (ply_work.no_spddown_timer != 0)
        {
            fSpd1 = 0;
            fSpd2 = v1 >> 2;
        }
        else
        {
            int v2 = 0;
            if (MTM_MATH_ABS(ply_work.obj_work.spd.x) > ply_work.spd2)
            {
                int num;
                if (spdJumpMax - ply_work.spd2 != 0)
                {
                    num = FX_Div(MTM_MATH_ABS(ply_work.obj_work.spd.x) - ply_work.spd2,
                        spdJumpMax - ply_work.spd2);
                    if (num > 4096)
                        num = 4096;
                }
                else
                    num = 4096;

                v2 = num * 3968 >> 12;
            }

            fSpd2 = v1 - FX_Mul(v1, v2);
        }

        if ((ply_work.player_flag & 67108864) != 0)
        {
            GMD_PLAYER_WATER_SET(ref fSpd2);
            GMD_PLAYER_WATER_SET(ref fSpd1);
        }

        int sSpd = FX_Mul(fSpd1, 4096);
        FX_Mul(spdJumpDec, 4096);
        if (GmPlayerKeyCheckWalkLeft(ply_work) | GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd.x < 0)
                    ply_work.obj_work.spd.x = ObjSpdDownSet(ply_work.obj_work.spd.x, sSpd);
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = ObjSpdUpSet(ply_work.obj_work.spd.x, fSpd2, spdJumpMax);
            }
            else
            {
                if (ply_work.obj_work.spd.x > 0)
                    ply_work.obj_work.spd.x = ObjSpdDownSet(ply_work.obj_work.spd.x, sSpd);
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = ObjSpdUpSet(ply_work.obj_work.spd.x, -fSpd2, spdJumpMax);
            }
        }
        else
        {
            ply_work.obj_work.spd.x = MTM_MATH_CLIP(ply_work.obj_work.spd.x, -spdJumpMax, spdJumpMax);
            ply_work.obj_work.spd_m = MTM_MATH_CLIP(ply_work.obj_work.spd_m, -spdJumpMax, spdJumpMax);
            ply_work.spd_pool = 0;
            ply_work.obj_work.spd.x = ObjSpdDownSet(ply_work.obj_work.spd.x, fSpd1);
            ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
        }
    }

    private static void GmPlySeqMoveSpin(GMS_PLAYER_WORK ply_work)
    {
        int sSpd = ply_work.spd_dec_spin;
        if (ply_work.no_spddown_timer != 0)
        {
            ply_work.obj_work.spd_slope = 0;
            sSpd = 0;
        }
        else
        {
            ply_work.obj_work.spd_slope = ply_work.seq_state == 37
                ? g_gm_player_parameter[ply_work.char_id].spd_slope_spin_spipe
                : g_gm_player_parameter[ply_work.char_id].spd_slope_spin;
            ply_work.obj_work.dir_slope = 640;
        }

        if (ply_work.obj_work.spd_m > 0 && (ply_work.key_on & 8) != 0 ||
            ply_work.obj_work.spd_m < 0 && (ply_work.key_on & 4) != 0)
            ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd >> 1);
        else if ((ply_work.obj_work.spd_m <= 0 || (ply_work.key_on & 8) == 0) &&
                 (ply_work.obj_work.spd_m >= 0 || (ply_work.key_on & 4) == 0))
            ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd << 1);
        else
            ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
    }

    private static void GmPlySeqMoveSpinNoDec(GMS_PLAYER_WORK ply_work)
    {
        int spdDecSpin = ply_work.spd_dec_spin;
        if (ply_work.no_spddown_timer != 0)
        {
            ply_work.obj_work.spd_slope = 0;
        }
        else
        {
            ply_work.obj_work.spd_slope = ply_work.seq_state == 37
                ? g_gm_player_parameter[ply_work.char_id].spd_slope_spin_spipe
                : g_gm_player_parameter[ply_work.char_id].spd_slope_spin;
            ply_work.obj_work.dir_slope = 640;
        }
    }

    private static void GmPlySeqMoveSpinPinball(GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.spd_slope = g_gm_player_parameter[ply_work.char_id].spd_slope_spin_pinball;
        ply_work.obj_work.dir_slope = 256;
        int spdAddSpinPinball = ply_work.spd_add_spin_pinball;
        int sSpd = ply_work.spd_dec_spin_pinball;
        int spdMaxSpinPinball = ply_work.spd_max_spin_pinball;
        int num1 = MTM_MATH_ABS(ply_work.key_walk_rot_z);
        if (num1 > 24576)
            num1 = 24576;
        int num2 = spdMaxSpinPinball * num1 / 24576;
        if (num2 < ply_work.prev_walk_roll_spd_max)
        {
            num2 = ply_work.prev_walk_roll_spd_max - sSpd;
            if (num2 < 0)
                num2 = 0;
        }

        ply_work.prev_walk_roll_spd_max = num2;
        if (ply_work.obj_work.dir.z != 0)
        {
            int num3 = FX_Mul(ply_work.spd_max_add_slope_spin_pinball,
                mtMathSin(ply_work.obj_work.dir.z));
            if (num3 > 0)
                num2 += num3;
        }

        if (ply_work.no_spddown_timer != 0)
            sSpd = 0;
        if (ply_work.spd_work_max >= num2 && MTM_MATH_ABS(ply_work.obj_work.spd_m) >= num2)
        {
            if (ply_work.spd_work_max > ply_work.obj_work.spd_m)
                ply_work.spd_work_max = MTM_MATH_ABS(ply_work.obj_work.spd_m);
            num2 = ply_work.spd_work_max;
        }

        if (GmPlayerKeyCheckWalkLeft(ply_work) | GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
                ply_work.obj_work.spd_m = ObjSpdUpSet(ply_work.obj_work.spd_m, spdAddSpinPinball, num2);
            }
            else
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
                ply_work.obj_work.spd_m = ObjSpdUpSet(ply_work.obj_work.spd_m, -spdAddSpinPinball, num2);
            }
        }
        else
        {
            ply_work.spd_pool = 0;
            ply_work.obj_work.spd.x = MTM_MATH_CLIP(ply_work.obj_work.spd.x, -num2, num2);
            ply_work.obj_work.spd_m = MTM_MATH_CLIP(ply_work.obj_work.spd_m, -num2, num2);
            if ((ply_work.obj_work.dir.z + 8192 & 65280) > 16384)
                return;
            if ((ply_work.player_flag & 134217728) != 0)
                ply_work.player_flag &= 4160749567U;
            else
                ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
        }
    }

    private static void gmPlySeqTruckMove(OBS_OBJECT_WORK obj_work)
    {
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        GMS_PLAYER_WORK gmsPlayerWork = (GMS_PLAYER_WORK) obj_work;
        ushort num4 = (ushort) (obj_work.dir.z +
                                (obj_work.dir_fall - (uint) g_gm_main_system.pseudofall_dir));
        obj_work.prev_pos.x = obj_work.pos.x;
        obj_work.prev_pos.y = obj_work.pos.y;
        obj_work.prev_pos.z = obj_work.pos.z;
        if (((int) obj_work.move_flag & 134217728) != 0)
        {
            obj_work.flow.x = 0;
            obj_work.flow.y = 0;
            obj_work.flow.z = 0;
        }

        int x = obj_work.flow.x;
        int y = obj_work.flow.y;
        if ((x != 0 || y != 0) && obj_work.dir_fall != 0)
            ObjObjectSpdDirFall(ref x, ref y, obj_work.dir_fall);
        if (obj_work.hitstop_timer != 0)
        {
            obj_work.move.x = FX_Mul(x, g_obj.speed);
            obj_work.move.y = FX_Mul(y, g_obj.speed);
            obj_work.move.z = FX_Mul(obj_work.flow.z, g_obj.speed);
        }
        else
        {
            if (((int) obj_work.move_flag & 1) == 0)
            {
                if (((int) obj_work.move_flag & 128) != 0 && ((int) obj_work.move_flag & 1) == 0)
                    obj_work.spd.y += FX_Mul(obj_work.spd_fall, g_obj.speed);
                if (((int) obj_work.move_flag & 128) != 0 && obj_work.spd.y > obj_work.spd_fall_max)
                    obj_work.spd.y = obj_work.spd_fall_max;
            }

            if (((int) obj_work.move_flag & 64) != 0)
            {
                if (((int) obj_work.move_flag & 131072) != 0 &&
                    (obj_work.spd_m != 0 || ((int) obj_work.move_flag & 262144) == 0) &&
                    (num4 + obj_work.dir_slope & ushort.MaxValue) >= obj_work.dir_slope << 1)
                {
                    int v1 = MTM_MATH_ABS(obj_work.spd_m) >= 16384
                        ? obj_work.spd_slope << 1
                        : obj_work.spd_slope;
                    if (obj_work.spd_m > 0 && num4 > 32768 || obj_work.spd_m < 0 && num4 < 32768)
                        v1 <<= 1;
                    int sSpd = FX_Mul(v1, mtMathSin(num4));
                    if (sSpd > 0 || num4 < 32768)
                    {
                        if (sSpd < 256)
                            sSpd = 256;
                    }
                    else if (sSpd > -256)
                        sSpd = -256;

                    obj_work.spd_m = ObjSpdUpSet(obj_work.spd_m, sSpd, obj_work.spd_slope_max);
                }

                if (((int) obj_work.move_flag & 32768) == 0)
                {
                    num1 = FX_Mul(obj_work.spd_m, mtMathCos(obj_work.dir.z));
                    num2 = FX_Mul(obj_work.spd_m, mtMathSin(obj_work.dir.z));
                }
            }

            if (((int) obj_work.move_flag & 67108864) != 0)
            {
                obj_work.move.x = FX_Mul(obj_work.spd.x + num1 + x, g_obj.speed);
                obj_work.move.y = FX_Mul(obj_work.spd.y + num2 + y, g_obj.speed);
            }
            else
            {
                obj_work.move.x = FX_Mul(obj_work.spd.x + num1 + x + g_obj.scroll[0],
                    g_obj.speed);
                obj_work.move.y = FX_Mul(obj_work.spd.y + num2 + y + g_obj.scroll[1],
                    g_obj.speed);
            }

            obj_work.move.z = FX_Mul(obj_work.spd.z + num3 + obj_work.flow.z, g_obj.speed);
            if (((int) obj_work.move_flag & 1) != 0)
                ObjObjectSpdDirFall(ref obj_work.move.x, ref obj_work.move.y, obj_work.dir_fall);
            else
                ObjObjectSpdDirFall(ref obj_work.move.x, ref obj_work.move.y,
                    gmsPlayerWork.jump_pseudofall_dir);
        }

        obj_work.pos.x += obj_work.move.x;
        obj_work.pos.y += obj_work.move.y;
        obj_work.pos.z += obj_work.move.z;
        obj_work.spd.x += obj_work.spd_add.x;
        obj_work.spd.y += obj_work.spd_add.y;
        obj_work.spd.z += obj_work.spd_add.z;
        obj_work.flow.x = 0;
        obj_work.flow.y = 0;
        obj_work.flow.z = 0;
    }

    private static void gmPlySeqSplMove(OBS_OBJECT_WORK obj_work)
    {
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        GMS_PLAYER_WORK gmsPlayerWork = (GMS_PLAYER_WORK) obj_work;
        ushort num4 = (ushort) (obj_work.dir.z +
                                (obj_work.dir_fall - (uint) g_gm_main_system.pseudofall_dir));
        obj_work.prev_pos.x = obj_work.pos.x;
        obj_work.prev_pos.y = obj_work.pos.y;
        obj_work.prev_pos.z = obj_work.pos.z;
        if (((int) obj_work.move_flag & 134217728) != 0)
        {
            obj_work.flow.x = 0;
            obj_work.flow.y = 0;
            obj_work.flow.z = 0;
        }

        int x = obj_work.flow.x;
        int y = obj_work.flow.y;
        if ((x != 0 || y != 0) && obj_work.dir_fall != 0)
            ObjObjectSpdDirFall(ref x, ref y, obj_work.dir_fall);
        if (obj_work.hitstop_timer != 0)
        {
            obj_work.move.x = FX_Mul(x, g_obj.speed);
            obj_work.move.y = FX_Mul(y, g_obj.speed);
            obj_work.move.z = FX_Mul(obj_work.flow.z, g_obj.speed);
        }
        else
        {
            if (((int) obj_work.move_flag & 1) == 0)
            {
                if (((int) obj_work.move_flag & 128) != 0 && ((int) obj_work.move_flag & 1) == 0)
                    obj_work.spd.y += FX_Mul(obj_work.spd_fall, g_obj.speed);
                if (((int) obj_work.move_flag & 128) != 0 && obj_work.spd.y > obj_work.spd_fall_max)
                    obj_work.spd.y = obj_work.spd_fall_max;
            }

            if (((int) obj_work.move_flag & 64) != 0)
            {
                if (((int) obj_work.move_flag & 131072) != 0 &&
                    (obj_work.spd_m != 0 || ((int) obj_work.move_flag & 262144) == 0))
                    obj_work.spd_m =
                        (num4 + obj_work.dir_slope & ushort.MaxValue) <
                        obj_work.dir_slope << 1 || ((int) obj_work.move_flag & 1) == 0
                            ? 0
                            : (MTM_MATH_ABS(obj_work.spd_m) >= 8192
                                ? (MTM_MATH_ABS(obj_work.spd_m) >= 16384
                                    ? ObjSpdUpSet(obj_work.spd_m,
                                        FX_Mul(obj_work.spd_slope << 1, mtMathSin(num4)),
                                        obj_work.spd_slope_max)
                                    : ObjSpdUpSet(obj_work.spd_m,
                                        FX_Mul(obj_work.spd_slope, mtMathSin(num4)),
                                        obj_work.spd_slope_max))
                                : ObjSpdUpSet(obj_work.spd_m,
                                    FX_Mul(obj_work.spd_slope >> 1, mtMathSin(num4)),
                                    obj_work.spd_slope_max));
                if (((int) obj_work.move_flag & 32768) == 0)
                {
                    num1 = FX_Mul(obj_work.spd_m, mtMathCos(obj_work.dir.z));
                    num2 = FX_Mul(obj_work.spd_m, mtMathSin(obj_work.dir.z));
                }
            }

            if (((int) obj_work.move_flag & 67108864) != 0)
            {
                obj_work.move.x = FX_Mul(obj_work.spd.x + num1 + x, g_obj.speed);
                obj_work.move.y = FX_Mul(obj_work.spd.y + num2 + y, g_obj.speed);
            }
            else
            {
                obj_work.move.x = FX_Mul(obj_work.spd.x + num1 + x + g_obj.scroll[0],
                    g_obj.speed);
                obj_work.move.y = FX_Mul(obj_work.spd.y + num2 + y + g_obj.scroll[1],
                    g_obj.speed);
            }

            obj_work.move.z = FX_Mul(obj_work.spd.z + num3 + obj_work.flow.z, g_obj.speed);
            if (((int) obj_work.move_flag & 1) != 0)
                ObjObjectSpdDirFall(ref obj_work.move.x, ref obj_work.move.y, obj_work.dir_fall);
            else
                ObjObjectSpdDirFall(ref obj_work.move.x, ref obj_work.move.y,
                    gmsPlayerWork.jump_pseudofall_dir);
        }

        obj_work.pos.x += obj_work.move.x;
        obj_work.pos.y += obj_work.move.y;
        obj_work.pos.z += obj_work.move.z;
        obj_work.spd.x += obj_work.spd_add.x;
        obj_work.spd.y += obj_work.spd_add.y;
        obj_work.spd.z += obj_work.spd_add.z;
        obj_work.flow.x = 0;
        obj_work.flow.y = 0;
        obj_work.flow.z = 0;
    }

    private static void gmPlySeqSplJumpDirec(GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.dir.z = ObjRoopMove16(ply_work.obj_work.dir.z,
            (ushort) (ply_work.jump_pseudofall_dir - (uint) ply_work.obj_work.dir_fall), 512);
        if (((int) ply_work.gmk_flag & 536875264) != 0)
            return;
        ply_work.obj_work.pos.z = (int) (ObjSpdDownSet(ply_work.obj_work.pos.z, 16384) & 4294963200L);
        ply_work.obj_work.spd.z = ObjSpdDownSet(ply_work.obj_work.spd.z, 512);
        ply_work.obj_work.dir.x = ObjRoopMove16(ply_work.obj_work.dir.x, 0, 1024);
    }

    private static void GmPlySeqJumpDirec(GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.dir.z = ObjRoopMove16(ply_work.obj_work.dir.z, 0, 512);
        if (((int) ply_work.gmk_flag & 536875264) != 0)
            return;
        ply_work.obj_work.pos.z = (int) (ObjSpdDownSet(ply_work.obj_work.pos.z, 16384) & 4294963200L);
        ply_work.obj_work.spd.z = ObjSpdDownSet(ply_work.obj_work.spd.z, 512);
        ply_work.obj_work.dir.x = ObjRoopMove16(ply_work.obj_work.dir.x, 0, 1024);
    }

    private static void GmPlySeqTruckJumpDirec(GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.dir.z = ObjRoopMove16(ply_work.obj_work.dir.z,
            (ushort) (ply_work.jump_pseudofall_dir - (uint) ply_work.obj_work.dir_fall), 512);
        if (((int) ply_work.gmk_flag & 536875264) != 0)
            return;
        ply_work.obj_work.pos.z = (int) (ObjSpdDownSet(ply_work.obj_work.pos.z, 16384) & 4294963200L);
        ply_work.obj_work.spd.z = ObjSpdDownSet(ply_work.obj_work.spd.z, 512);
        ply_work.obj_work.dir.x = ObjRoopMove16(ply_work.obj_work.dir.x, 0, 1024);
    }

    private static bool GmPlySeqCheckAcceptHoming(GMS_PLAYER_WORK ply_work)
    {
        return ((int) ply_work.seq_state_data_tbl[ply_work.seq_state].accept_attr & 16) != 0 &&
               (ply_work.player_flag & 128) == 0;
    }

    private static void gmPlySeqCheckChangeSequence(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        if (GmPlayerIsTransformSuperSonic(ply_work) && GmPlayerKeyCheckTransformKeyPush(ply_work) &&
            (0 <= ply_work.seq_state && ply_work.seq_state <= 21))
            GmPlySeqChangeTransformSuper(ply_work);
        gmPlySeqCheckChangeSequenceUserInput(ply_work);
    }

    private static bool gmPlySeqCheckChangeSequenceUserInput(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        GMS_PLY_SEQ_STATE_DATA[] seqStateDataTbl = ply_work.seq_state_data_tbl;
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 4) != 0)
        {
            if ((ply_work.player_flag & 262144) != 0)
                gmPlySeqCheckEndTruckWalk(ply_work);
            else
                gmPlySeqCheckEndWalk(ply_work);
        }

        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 1) != 0)
            gmPlySeqCheckTurn(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 2) != 0)
            gmPlySeqCheckDirectTurn(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 8) != 0)
            gmPlySeqCheckFall(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 32768) != 0)
            gmPlySeqCheckStagger(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 16) != 0)
            gmPlySeqCheckEndLookup(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 32) != 0)
            gmPlySeqCheckEndSquat(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 128) != 0)
            gmPlySeqCheckEndSpin(ply_work);
        if (((int) seqStateDataTbl[ply_work.seq_state].check_attr & 1024) != 0)
            gmPlySeqCheckEndWallPush(ply_work);
        if (GmPlySeqCheckAcceptHoming(ply_work))
        {
            if (ply_work.cursol_enemy_obj == null)
            {
                GmPlyEfctCreateHomingCursol(ply_work);
                ply_work.cursol_enemy_obj = ply_work.enemy_obj;
            }

            if (gmPlySeqCheckHoming(ply_work))
                return true;
        }

        if (
            ((int) seqStateDataTbl[ply_work.seq_state].accept_attr & 4096) != 0 &&
            gmPlySeqCheckSquatSpin(ply_work) ||
            ((int) seqStateDataTbl[ply_work.seq_state].accept_attr & 256) != 0 && gmPlySeqCheckSpin(ply_work) ||
            (((int) seqStateDataTbl[ply_work.seq_state].accept_attr & 512) != 0 &&
             gmPlySeqCheckSpinDashAcc(ply_work) ||
             ((int) seqStateDataTbl[ply_work.seq_state].accept_attr & 2048) != 0 &&
             gmPlySeqCheckPinballSpinDashAcc(ply_work)) ||
            (((int) seqStateDataTbl[ply_work.seq_state].accept_attr & 8) != 0 && gmPlySeqCheckJump(ply_work) ||
             ((int) seqStateDataTbl[ply_work.seq_state].accept_attr & 4) != 0 && gmPlySeqCheckBrake(ply_work) ||
             ((int) seqStateDataTbl[ply_work.seq_state].accept_attr & 1024) != 0 &&
             gmPlySeqCheckWallPush(ply_work)))
            return true;
        if (((int) seqStateDataTbl[ply_work.seq_state].accept_attr & 2) != 0)
        {
            if ((ply_work.player_flag & 262144) != 0)
            {
                if (gmPlySeqCheckTruckWalk(ply_work))
                    return true;
            }
            else if (gmPlySeqCheckWalk(ply_work))
                return true;
        }

        return ((int) seqStateDataTbl[ply_work.seq_state].accept_attr & 64) != 0 &&
               gmPlySeqCheckLookup(ply_work) ||
               ((int) seqStateDataTbl[ply_work.seq_state].accept_attr & 128) != 0 &&
               gmPlySeqCheckSquat(ply_work);
    }

    private static bool gmPlySeqCheckEndWalk(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m != 0 || ply_work.obj_work.spd.z != 0)
            return false;
        GmPlySeqChangeSequence(ply_work, 0);
        return true;
    }

    private static bool gmPlySeqCheckTurn(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.seq_state == 2)
        {
            if (((int) ply_work.obj_work.disp_flag & 1) == 0 && GmPlayerKeyCheckWalkRight(ply_work) ||
                ((int) ply_work.obj_work.disp_flag & 1) != 0 && GmPlayerKeyCheckWalkLeft(ply_work))
                return GmPlySeqChangeSequence(ply_work, 2);
        }
        else if ((((int) ply_work.obj_work.disp_flag & 1) != 0 && GmPlayerKeyCheckWalkRight(ply_work) ||
                  ((int) ply_work.obj_work.disp_flag & 1) == 0 && GmPlayerKeyCheckWalkLeft(ply_work)) &&
                 MTM_MATH_ABS(ply_work.obj_work.spd_m) < 16384)
            return GmPlySeqChangeSequence(ply_work, 2);

        return false;
    }

    private static bool gmPlySeqCheckDirectTurn(GMS_PLAYER_WORK ply_work)
    {
        if ((((int) ply_work.obj_work.disp_flag & 1) == 0 || !GmPlayerKeyCheckWalkRight(ply_work)) &&
            (((int) ply_work.obj_work.disp_flag & 1) != 0 || !GmPlayerKeyCheckWalkLeft(ply_work)) ||
            ((int) ply_work.obj_work.move_flag & 16) == 0 &&
            (((int) ply_work.obj_work.move_flag & 16) != 0 || MTM_MATH_ABS(ply_work.obj_work.spd_m) >= 16384))
            return false;
        if (ply_work.act_state == 40 || ply_work.act_state == 48 ||
            (ply_work.act_state == 41 || ply_work.act_state == 42) || ply_work.act_state == 43)
            GmPlySeqSetFallTurn(ply_work);
        else
            GmPlySeqSetProgramTurn(ply_work, 4096);
        return true;
    }

    private static bool gmPlySeqCheckFall(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.move_flag & 1) == 0)
            return GmPlySeqChangeSequence(ply_work, 16);
        if ((ply_work.player_flag & 262144) != 0)
        {
            if (((int) ply_work.gmk_flag & 262144) != 0)
            {
                if (((int) ply_work.gmk_flag & 1073741824) != 0)
                {
                    if (ply_work.fall_timer != 0)
                    {
                        ply_work.fall_timer = ObjTimeCountDown(ply_work.fall_timer);
                        return false;
                    }

                    ply_work.gmk_flag &= 3220963327U;
                    GmPlayerSpdParameterSet(ply_work);
                    ply_work.jump_pseudofall_dir = g_gm_main_system.pseudofall_dir;
                    ply_work.obj_work.pos.x = FXM_FLOAT_TO_FX32(ply_work.truck_mtx_ply_mtn_pos.M03);
                    ply_work.obj_work.pos.y = FXM_FLOAT_TO_FX32(-ply_work.truck_mtx_ply_mtn_pos.M13);
                    ply_work.obj_work.pos.z = FXM_FLOAT_TO_FX32(ply_work.truck_mtx_ply_mtn_pos.M23);
                    GmPlySeqChangeDeath(ply_work);
                    ply_work.gmk_flag2 |= 64U;
                    return true;
                }
            }
            else
            {
                ushort num = (ushort) (ply_work.obj_work.dir.z +
                                       (ply_work.obj_work.dir_fall -
                                        (uint) g_gm_main_system.pseudofall_dir));
                if (((num + 16384 & 32768) != 0 || num == 49152) &&
                    MTM_MATH_ABS(ply_work.obj_work.spd_m) < 8192)
                {
                    if (ply_work.fall_timer != 0)
                    {
                        ply_work.fall_timer = ObjTimeCountDown(ply_work.fall_timer);
                        return false;
                    }

                    GmPlayerSpdParameterSet(ply_work);
                    return GmPlySeqChangeSequence(ply_work, 16);
                }

                GmPlayerSpdParameterSet(ply_work);
            }
        }
        else if (ply_work.fall_timer != 0)
            ply_work.fall_timer = ObjTimeCountDown(ply_work.fall_timer);
        else if (((ply_work.obj_work.dir.z + 16384 & 32768) != 0 || ply_work.obj_work.dir.z == 49152) &&
                 MTM_MATH_ABS(ply_work.obj_work.spd_m) < 8192)
        {
            GmPlayerSpdParameterSet(ply_work);
            return GmPlySeqChangeSequence(ply_work, 16);
        }

        return false;
    }

    private static bool gmPlySeqCheckStagger(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.obj_work.dir.z & short.MaxValue) != 0 || ply_work.obj_work.ride_obj != null)
            return false;
        OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        pData.pos_x = ply_work.obj_work.pos.x >> 12;
        pData.pos_y = (ply_work.obj_work.pos.y >> 12) + ply_work.obj_work.field_rect[3];
        pData.flag = (ushort) (ply_work.obj_work.flag & 1U);
        pData.vec = 2;
        pData.dir = null;
        pData.attr = null;
        if (ObjDiffCollision(pData) > 0)
        {
            pData.pos_x = (ply_work.obj_work.pos.x >> 12) + ply_work.obj_work.field_rect[0] - 2;
            int num1 = ObjDiffCollision(pData);
            pData.pos_x = (ply_work.obj_work.pos.x >> 12) + ply_work.obj_work.field_rect[2] + 2;
            int num2 = ObjDiffCollision(pData);
            if (num1 <= 0 && num2 >= 16)
            {
                pData.pos_x = (ply_work.obj_work.pos.x >> 12) + ply_work.obj_work.field_rect[0] - -4;
                int num3 = ObjDiffCollision(pData);
                if (((int) ply_work.obj_work.disp_flag & 1) != 0)
                {
                    GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
                    return GmPlySeqChangeSequence(ply_work, 14);
                }

                GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
                return num3 > 0
                    ? GmPlySeqChangeSequence(ply_work, 15)
                    : GmPlySeqChangeSequence(ply_work, 13);
            }

            if (num1 >= 16 && num2 <= 0)
            {
                pData.pos_x = (ply_work.obj_work.pos.x >> 12) + ply_work.obj_work.field_rect[2] - 4;
                int num3 = ObjDiffCollision(pData);
                if (((int) ply_work.obj_work.disp_flag & 1) != 0)
                {
                    GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
                    return num3 > 0
                        ? GmPlySeqChangeSequence(ply_work, 15)
                        : GmPlySeqChangeSequence(ply_work, 13);
                }

                GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
                return GmPlySeqChangeSequence(ply_work, 14);
            }
        }

        GlobalPool<OBS_COL_CHK_DATA>.Release(pData);
        return false;
    }

    private static bool gmPlySeqCheckEndLookup(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.move_flag & 1) == 0)
            return GmPlySeqChangeSequence(ply_work, 16);
        return (ply_work.key_on & 1) == 0 && GmPlySeqChangeSequence(ply_work, 5);
    }

    private static bool gmPlySeqCheckEndSquat(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.move_flag & 1) == 0)
            return GmPlySeqChangeSequence(ply_work, 16);
        return (ply_work.key_on & 2) == 0 && GmPlySeqChangeSequence(ply_work, 8);
    }

    private static bool gmPlySeqCheckEndSpin(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m >= 2048 || ply_work.obj_work.spd_m <= -2048)
            return false;
        ply_work.obj_work.spd_m = 0;
        GmPlayerSpdParameterSet(ply_work);
        if ((ply_work.player_flag & 131072) != 0)
        {
            GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
        }

        return GmPlySeqChangeSequence(ply_work, 0);
    }

    private static bool gmPlySeqCheckEndWallPush(GMS_PLAYER_WORK ply_work)
    {
        return (((int) ply_work.obj_work.move_flag & 4) == 0 ||
                ((int) ply_work.obj_work.disp_flag & 1) != 0 && !GmPlayerKeyCheckWalkLeft(ply_work) ||
                ((int) ply_work.obj_work.disp_flag & 1) == 0 && !GmPlayerKeyCheckWalkRight(ply_work)) &&
               GmPlySeqChangeSequence(ply_work, 0);
    }

    private static bool gmPlySeqCheckHoming(GMS_PLAYER_WORK ply_work)
    {
        if (!GmPlayerKeyCheckJumpKeyPush(ply_work) || ply_work.homing_timer != 0 ||
            ((ply_work.player_flag & 128) != 0 || GMM_MAIN_STAGE_IS_ENDING()))
            return false;
        return ply_work.enemy_obj != null
            ? GmPlySeqChangeSequence(ply_work, 19)
            : GmPlySeqChangeSequence(ply_work, 21);
    }

    private static bool gmPlySeqCheckSquatSpin(GMS_PLAYER_WORK ply_work)
    {
        return (ply_work.key_on & 2) != 0 &&
               (ply_work.obj_work.spd_m > 2048 || ply_work.obj_work.spd_m < -2048) &&
               GmPlySeqChangeSequence(ply_work, 10);
    }

    private static bool gmPlySeqCheckSpin(GMS_PLAYER_WORK ply_work)
    {
        return (ply_work.obj_work.spd_m > 2048 || ply_work.obj_work.spd_m < -2048) &&
               GmPlySeqChangeSequence(ply_work, 10);
    }

    private static bool gmPlySeqCheckSpinDashAcc(GMS_PLAYER_WORK ply_work)
    {
        return GmPlayerKeyCheckJumpKeyPush(ply_work) && GmPlySeqChangeSequence(ply_work, 11);
    }

    private static bool gmPlySeqCheckPinballSpinDashAcc(GMS_PLAYER_WORK ply_work)
    {
        return (ply_work.key_on & 2) != 0 && GmPlayerKeyCheckJumpKeyPush(ply_work) &&
               GmPlySeqChangeSequence(ply_work, 11);
    }

    private static bool gmPlySeqCheckJump(GMS_PLAYER_WORK ply_work)
    {
        return GmPlayerKeyCheckJumpKeyPush(ply_work) &&
               (((int) ply_work.obj_work.move_flag & 1) != 0 ||
                ply_work.gmk_obj != null && ((int) ply_work.gmk_flag & 16384) != 0) &&
               GmPlySeqChangeSequence(ply_work, 17);
    }

    private static bool gmPlySeqCheckBrake(GMS_PLAYER_WORK ply_work)
    {
        return ply_work.seq_state != 9 &&
               (GmPlayerKeyCheckWalkLeft(ply_work) && ply_work.obj_work.spd_m >= 16384 ||
                GmPlayerKeyCheckWalkRight(ply_work) && ply_work.obj_work.spd_m <= -16384) &&
               GmPlySeqChangeSequence(ply_work, 9);
    }

    private static bool gmPlySeqCheckWalk(GMS_PLAYER_WORK ply_work)
    {
        return
            (!GmObjCheckMapLeftLimit(ply_work.obj_work, 14) || !GmPlayerKeyCheckWalkLeft(ply_work)) &&
            (!GmObjCheckMapRightLimit(ply_work.obj_work, 14) || !GmPlayerKeyCheckWalkRight(ply_work)) &&
            (13 > ply_work.seq_state || ply_work.seq_state > 15 || ((int) ply_work.obj_work.move_flag & 4) == 0 ||
             (((int) ply_work.obj_work.disp_flag & 1) == 0 || !GmPlayerKeyCheckWalkLeft(ply_work)) &&
             (((int) ply_work.obj_work.disp_flag & 1) != 0 || !GmPlayerKeyCheckWalkRight(ply_work))) &&
            (ply_work.obj_work.spd_m != 0 || GmPlayerKeyCheckWalkLeft(ply_work) ||
             GmPlayerKeyCheckWalkRight(ply_work)) && GmPlySeqChangeSequence(ply_work, 1);
    }

    private static bool gmPlySeqCheckLookup(GMS_PLAYER_WORK ply_work)
    {
        return ply_work.obj_work.spd_m == 0 && ((int) ply_work.obj_work.move_flag & 1) != 0 &&
               (ply_work.key_on & 1) != 0 && GmPlySeqChangeSequence(ply_work, 3);
    }

    private static bool gmPlySeqCheckSquat(GMS_PLAYER_WORK ply_work)
    {
        return (ply_work.key_on & 2) != 0 && GmPlySeqChangeSequence(ply_work, 7);
    }

    private static bool gmPlySeqCheckWallPush(GMS_PLAYER_WORK ply_work)
    {
        return ((int) ply_work.obj_work.move_flag & 4) != 0 && (ply_work.player_flag & 32768) == 0 &&
               (((int) ply_work.obj_work.disp_flag & 1) != 0 && GmPlayerKeyCheckWalkLeft(ply_work) ||
                ((int) ply_work.obj_work.disp_flag & 1) == 0 && GmPlayerKeyCheckWalkRight(ply_work)) &&
               (ply_work.obj_work.pos.x >> 12 > g_gm_main_system.map_fcol.left + 14 &&
                ply_work.obj_work.pos.x >> 12 < g_gm_main_system.map_fcol.right - 14) &&
               GmPlySeqChangeSequence(ply_work, 18);
    }

    private static bool gmPlySeqCheckTruckWalk(GMS_PLAYER_WORK ply_work)
    {
        if (GmObjCheckMapLeftLimit(ply_work.obj_work, 14) && GmPlayerKeyCheckWalkLeft(ply_work) ||
            GmObjCheckMapRightLimit(ply_work.obj_work, 14) && GmPlayerKeyCheckWalkRight(ply_work))
            return false;
        if (MTM_MATH_ABS(ply_work.obj_work.spd_m) >= 64 || GmPlayerKeyCheckWalkLeft(ply_work) ||
            GmPlayerKeyCheckWalkRight(ply_work))
            return GmPlySeqChangeSequence(ply_work, 1);
        ply_work.obj_work.spd_m = 0;
        return false;
    }

    private static bool gmPlySeqCheckEndTruckWalk(GMS_PLAYER_WORK ply_work)
    {
        if (MTM_MATH_ABS(ply_work.obj_work.spd_m) >= 64 || ply_work.obj_work.spd.z != 0)
            return false;
        ply_work.obj_work.spd_m = 0;
        GmPlySeqChangeSequence(ply_work, 0);
        return true;
    }

    private static void gmPlySeqSplStgRollCtrl(GMS_PLAYER_WORK ply_work)
    {
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        if (((int) g_gm_main_system.game_flag & 17240600) != 0)
            return;
        int keyRotZ = ply_work.key_rot_z;
        if (((int) g_gs_main_sys_info.game_flag & 512) != 0)
        {
            int num = keyRotZ * 26 / 10;
            obsCamera.roll += num;
        }
        else
        {
            int num1 = 66;
            --ply_work.accel_counter;
            if (ply_work.accel_counter <= 0)
            {
                ply_work.accel_counter = 0;
                int a = (keyRotZ - ply_work.prev_key_rot_z) * 3 / 4;
                int num2;
                if (MTM_MATH_ABS(a) < 16384)
                    num2 = 0;
                else if (a < 0 && keyRotZ > 0 || a > 0 && keyRotZ < 0)
                {
                    num2 = 0;
                }
                else
                {
                    int num3 = a / 64;
                    ply_work.dir_vec_add = num3;
                    ply_work.accel_counter = 8;
                }
            }

            int num4 = keyRotZ / num1 + ply_work.dir_vec_add * ply_work.accel_counter;
            obsCamera.roll += num4;
        }

        if (ply_work.nudge_di_timer != 0)
        {
            --ply_work.nudge_di_timer;
        }
        else
        {
            bool flag = false;
            if ((ply_work.key_push & 160) != 0)
                flag = true;
            if (!flag)
                return;
            GMS_SPL_STG_WORK work = GmSplStageGetWork();
            ply_work.nudge_di_timer = 30;
            ply_work.nudge_timer = 30;
            work.flag &= 4294967293U;
        }
    }

    private static void GmPlySeqInitSpringJump(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        bool spd_clear,
        int no_jump_move_time,
        int fall_dir,
        bool t_cam_slow)
    {
        bool set_act = true;
        GmPlySeqChangeSequenceState(ply_work, 29);
        if (spd_clear)
        {
            ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
            ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
            ply_work.obj_work.spd_m = 0;
        }

        if ((ply_work.player_flag & 262144) != 0)
        {
            if (fall_dir != -1)
            {
                ply_work.jump_pseudofall_dir = (ushort) fall_dir;
                int a = fall_dir - ply_work.ply_pseudofall_dir;
                if ((ushort) MTM_MATH_ABS(a) > 32768)
                {
                    if (a < 0)
                        ply_work.ply_pseudofall_dir += 65536 + a;
                    else
                        ply_work.ply_pseudofall_dir += a - 65536;
                }
                else
                    ply_work.ply_pseudofall_dir = fall_dir;

                g_gm_main_system.pseudofall_dir = (ushort) ply_work.ply_pseudofall_dir;
            }

            GmPlayerSetAtk(ply_work);
            set_act = false;
            GmPlayerActionChange(ply_work, 40);
            ply_work.obj_work.disp_flag |= 4U;
        }

        GmPlySeqGmkInitGmkJump(ply_work, spd_x, spd_y, set_act);
        if ((ply_work.player_flag & 262144) != 0 && fall_dir != -1)
            ply_work.gmk_flag2 |= 256U;
        if ((ply_work.player_flag & 262144) != 0)
        {
            ply_work.obj_work.disp_flag &= 4294967294U;
            if (t_cam_slow)
                ply_work.gmk_flag2 |= 32U;
        }

        if ((ply_work.player_flag & 262144) != 0 && fall_dir != -1)
            ply_work.gmk_flag |= 16777216U;
        if (no_jump_move_time > 0)
            GmPlySeqSetNoJumpMoveTime(ply_work, no_jump_move_time);
        if ((ply_work.player_flag & 262144) != 0)
            GmSoundPlaySE("Lorry5");
        else
            GmSoundPlaySE("Spring");
        GMM_PAD_VIB_SMALL();
    }

    private static void GmPlySeqInitRockRideStart(
        GMS_PLAYER_WORK ply_work,
        GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (OBS_OBJECT_WORK) com_work)
            return;
        GmPlySeqChangeSequenceState(ply_work, 30);
        GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = com_work.obj_work;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainGimmickRockRidePush);
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        OBS_OBJECT_WORK gmkObj = ply_work.gmk_obj;
        GmPlayerActionChange(ply_work, 17);
        objWork.spd_m = 0;
        objWork.spd.x = 0;
        objWork.spd.y = 0;
        objWork.spd.z = 0;
        objWork.spd_add.x = 0;
        objWork.spd_add.y = 0;
        objWork.spd_add.z = 0;
        if (objWork.pos.x < gmkObj.pos.x)
            objWork.disp_flag &= 4294967294U;
        else
            objWork.disp_flag |= 1U;
    }

    private static void GmPlySeqInitRockRide(
        GMS_PLAYER_WORK ply_work,
        GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (OBS_OBJECT_WORK) com_work)
            return;
        GmPlySeqChangeSequenceState(ply_work, 31);
        GmPlySeqGmkInitGimmickDependInit(ply_work, com_work.obj_work, 0, 0, 0);
        ply_work.gmk_obj = com_work.obj_work;
        com_work.target_dp_dist = 229376;
        ply_work.player_flag |= 12U;
        ply_work.obj_work.move_flag |= 256U;
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        OBS_OBJECT_WORK gmkObj = ply_work.gmk_obj;
        if (objWork.pos.y > gmkObj.pos.y)
        {
            GmPlySeqChangeSequence(ply_work, 16);
            ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainGimmickRockRideStop);
        }
        else
        {
            ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainGimmickRockRide);
            GmPlayerCameraOffsetSet(ply_work, 0, -48);
            GmCameraAllowSet(10f, 30f, 0.0f);
        }

        GmPlayerActionChange(ply_work, 60);
        objWork.disp_flag |= 4U;
        ply_work.gmk_flag |= 16384U;
        int v1 = FX_Div(gmkObj.pos.x - objWork.pos.x, 229376);
        objWork.spd_m = FX_Mul(v1, 15360) + gmkObj.spd_m;
        if (gmkObj.spd_m > 0)
            objWork.disp_flag &= 4294967294U;
        else
            objWork.disp_flag |= 1U;
    }

    private static void GmPlySeqInitPulley(
        GMS_PLAYER_WORK ply_work,
        GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (OBS_OBJECT_WORK) com_work)
            return;
        GmPlySeqChangeSequenceState(ply_work, 32);
        com_work.obj_work.spd.x = ply_work.obj_work.spd.x;
        if (((int) ply_work.obj_work.move_flag & 16) == 0)
            com_work.obj_work.spd.x =
                FX_Mul(ply_work.obj_work.spd_m, mtMathCos(ply_work.obj_work.dir.z));
        com_work.obj_work.move_flag &= 4294967231U;
        GmPlySeqGmkInitGimmickDependInit(ply_work, com_work.obj_work, 0, 163840, 0);
        com_work.target_dp_pos.x = 0;
        com_work.target_dp_pos.y = 163840;
        com_work.target_dp_pos.z = 0;
        ply_work.player_flag |= 12U;
        com_work.target_dp_dist = -163840;
        ply_work.obj_work.move_flag |= 256U;
        ply_work.obj_work.move_flag &= 4294967278U;
        ply_work.gmk_flag |= 16384U;
        GmPlayerActionChange(ply_work, 66);
        ply_work.obj_work.pos.Assign(com_work.obj_work.pos);
        ply_work.obj_work.pos.y += 163840;
    }

    private static void GmPlySeqInitBreathing(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 33);
        GmPlayerStateGimmickInit(ply_work);
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainGimmickBreathing);
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        objWork.spd_m = 0;
        objWork.spd.x = 0;
        objWork.spd.y = 0;
        objWork.spd_add.x = 0;
        objWork.spd_add.y = 0;
        if (((int) objWork.move_flag & 1) != 0)
            GmPlayerActionChange(ply_work, 62);
        else
            GmPlayerActionChange(ply_work, 68);
        GmSoundPlaySE("Breathe");
    }

    private static void GmPlySeqInitDashPanel(GMS_PLAYER_WORK ply_work, uint type)
    {
        int[][] numArray = new int[4][]
        {
            new int[2] {55296, 0},
            new int[2] {-55296, 0},
            new int[2] {0, -55296},
            new int[2] {0, -55296}
        };
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequenceState(ply_work, 34);
        if ((ply_work.player_flag & 262144) == 0)
            GmPlayerActionChange(ply_work, 27);
        else if (type == 1U || type == 3U)
        {
            ply_work.gmk_flag |= 1048576U;
            GmPlayerActionChange(ply_work, 72);
        }
        else
        {
            ply_work.gmk_flag &= 1048576U;
            GmPlayerActionChange(ply_work, 71);
        }

        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.obj_work.user_timer = 60;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainDashPanel);
        if ((ply_work.player_flag & 262144) == 0)
            GmPlySeqGmkSpdSet(ply_work, numArray[(int) type][0], numArray[(int) type][1]);
        else
            GmPlySeqGmkTruckSpdSet(ply_work, numArray[(int) type][0], numArray[(int) type][1]);
        ply_work.no_spddown_timer = 49152;
        ply_work.spd_work_max = ply_work.obj_work.spd_m;
        GmPlayerSetAtk(ply_work);

        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        if (!betterSfx)
            GmSoundPlaySE("Spin");

        if ((ply_work.player_flag & 262144) == 0)
        {
            GmPlyEfctCreateSpinDashBlur(ply_work, 1U);
            GmPlyEfctCreateSpinDashCircleBlur(ply_work);
            GmPlyEfctCreateTrail(ply_work, GME_PLY_EFCT_TRAIL_TYPE_SPINDASH);
        }

        GMM_PAD_VIB_SMALL();
    }

    private static void GmPlySeqInitTarzanRope(
        GMS_PLAYER_WORK ply_work,
        GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (OBS_OBJECT_WORK) com_work)
            return;
        GmPlySeqChangeSequenceState(ply_work, 35);
        GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = com_work.obj_work;
        ply_work.seq_func = null;
        ply_work.obj_work.move_flag &= 4294967102U;
        GmPlayerActionChange(ply_work, 63);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.gmk_flag |= 16384U;
    }

    private static void GmPlySeqInitWaterSlider(
        GMS_PLAYER_WORK ply_work,
        GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (OBS_OBJECT_WORK) com_work)
            return;
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequenceState(ply_work, 36);
        GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = com_work.obj_work;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainWaterSlider);
        ply_work.obj_work.move_flag &= 4294967279U;
        GmPlayerActionChange(ply_work, 65);
        ply_work.obj_work.disp_flag |= 4U;
        GmGmkWaterSliderCreateEffect();
    }

    private static void GmPlySeqInitSpipe(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 37);
        if (ply_work.act_state != 26 && ply_work.act_state != 27)
            GmSoundPlaySE("Spin");
        if (ply_work.act_state != 27)
        {
            GmPlayerActionChange(ply_work, 27);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainSpipe);
        GmPlyEfctCreateSpinDashBlur(ply_work, 1U);
        GmPlyEfctCreateSpinDashCircleBlur(ply_work);
    }

    private static ushort GmPlySeqScrewCheck(GMS_PLAYER_WORK ply_work)
    {
        return ply_work.seq_func == new seq_func_delegate(gmPlySeqGmkScrewMain)
            ? (ushort) 1
            : (ushort) 0;
    }

    private static void GmPlySeqInitScrew(
        GMS_PLAYER_WORK ply_work,
        GMS_ENEMY_COM_WORK gmk_work,
        int pos_x,
        int pos_y,
        ushort flag)
    {
        if (GmPlySeqScrewCheck(ply_work) != 0)
            return;
        GmPlySeqChangeSequenceState(ply_work, 38);
        GmPlayerWalkActionSet(ply_work);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 8208U;
        ply_work.gmk_flag |= 147456U;
        ply_work.gmk_obj = gmk_work.obj_work;
        ply_work.gmk_work0 = pos_x;
        ply_work.gmk_work1 = pos_y;
        ply_work.obj_work.user_work = flag;
        ply_work.obj_work.user_timer = 0;
        if (((int) ply_work.obj_work.user_work & GMD_GMK_SCREW_EVE_FLAG_LEFT) != 0)
        {
            if (ply_work.gmk_work0 > ply_work.obj_work.pos.x)
                ply_work.obj_work.user_timer = ply_work.gmk_work0 - ply_work.obj_work.pos.x;
        }
        else if (ply_work.gmk_work0 < ply_work.obj_work.pos.x)
            ply_work.obj_work.user_timer = ply_work.obj_work.pos.x - ply_work.gmk_work0;

        ply_work.gmk_work1 -= ply_work.obj_work.field_rect[3] << 12;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkScrewMain);
        ply_work.timer = 16;
    }

    private static void GmPlySeqInitDemoFw(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 39);
        GmPlayerStateGimmickInit(ply_work);
        ply_work.seq_func = null;
        if ((ply_work.player_flag & 262144) != 0)
        {
            GmPlayerActionChange(ply_work, 69);
            ply_work.obj_work.disp_flag |= 4U;
        }
        else
        {
            GmPlayerActionChange(ply_work, 0);
            ply_work.obj_work.disp_flag |= 4U;
        }
    }

    private static void GmPlySeqInitCannon(
        GMS_PLAYER_WORK ply_work,
        GMS_ENEMY_COM_WORK gmk_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 41);
        GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.obj_work.move_flag |= 512U;
        ply_work.obj_work.pos.x = gmk_work.obj_work.pos.x;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.x = 0;
        if (ply_work.obj_work.spd_add.y <= 0)
            ply_work.obj_work.spd_add.y = 672;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkCannonWait);
        ply_work.gmk_obj = gmk_work.obj_work;
        ply_work.gmk_flag2 |= 134U;
        GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
    }

    private static void GmPlySeqInitCannonShoot(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        GmPlySeqChangeSequenceState(ply_work, 42);
        GmPlySeqGmkInitGmkJump(ply_work, spd_x, spd_y);
        GmPlayerActionChange(ply_work, 67);
        ply_work.obj_work.disp_flag |= 4U;
        GmPlayerSetDefNormal(ply_work);
        GmPlayerSetAtk(ply_work);
        GmPlyEfctCreateSpinJumpBlur(ply_work);
    }

    private static void GmPlySeqInitStopper(
        GMS_PLAYER_WORK ply_work,
        GMS_ENEMY_COM_WORK gmk_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 40);
        if (ply_work.act_state != 26)
            GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 16U;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.flag |= 2U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkStopperMove);
        ply_work.gmk_obj = gmk_work.obj_work;
    }

    private static void GmPlySeqInitStopperEnd(GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.move_flag |= 144U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkStopperEnd);
    }

    private static void GmPlySeqGmkInitUpBumper(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        GmPlySeqChangeSequenceState(ply_work, 43);
        GmPlySeqGmkInitGmkJump(ply_work, spd_x, spd_y);
        GmSoundPlaySE("Spring");
    }

    private static void GmPlySeqGmkInitSeesaw(
        GMS_PLAYER_WORK ply_work,
        GMS_ENEMY_COM_WORK gmk_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 44);
        if (ply_work.act_state != 27)
        {
            GmPlayerActionChange(ply_work, 27);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.dir.z = 0;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkSeesaw);
        ply_work.gmk_obj = gmk_work.obj_work;
        GmPlyEfctCreateSpinDashBlur(ply_work, 1U);
        GmPlyEfctCreateSpinDashCircleBlur(ply_work);
    }

    private static void GmPlySeqGmkInitSeesawEnd(
        GMS_PLAYER_WORK ply_work,
        int spdx,
        int spdy)
    {
        GmPlySeqChangeSequence(ply_work, 16);
        GmPlySeqGmkInitGmkJump(ply_work, spdx, spdy, false);
        ply_work.no_spddown_timer = 0;
        ply_work.gmk_obj = null;
        GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        GmPlayerSetAtk(ply_work);
    }

    private static void GmPlySeqGmkInitSpinFall(GMS_PLAYER_WORK ply_work, int spdx, int spdy)
    {
        ply_work.gmk_obj = null;
        GmPlySeqChangeSequenceState(ply_work, 66);
        GmPlySeqInitFallState(ply_work);
        GmPlySeqGmkInitGmkJump(ply_work, spdx, spdy, false);
        ply_work.no_spddown_timer = 0;
        if (ply_work.act_state != 26)
        {
            GmPlayerActionChange(ply_work, 26);
            ply_work.obj_work.disp_flag |= 4U;
        }

        GmPlayerSetAtk(ply_work);
        GmPlyEfctCreateSpinJumpBlur(ply_work);
    }

    private static void GmPlySeqInitPinball(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_spddown_timer)
    {
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequenceState(ply_work, 45);
        GmPlayerStateGimmickInit(ply_work);
        if (ply_work.act_state != 39)
        {
            GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
            GmPlyEfctCreateSpinJumpBlur(ply_work);
        }

        ply_work.obj_work.move_flag &= 4294967279U;
        GmPlySeqGmkSpdSet(ply_work, spd_x, spd_y);
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.z = 0;
        ply_work.obj_work.user_timer = 60;
        ply_work.no_spddown_timer = no_spddown_timer * 4096;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainPinball);
        GmPlayerSetAtk(ply_work);
        GmSoundPlaySE("Spin");
    }

    private static void GmPlySeqInitPinballAir(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, 5, false, 0);
    }

    private static void GmPlySeqInitPinballAir(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_move_time)
    {
        GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, no_move_time, false, 0);
    }

    private static void GmPlySeqInitPinballAir(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_move_time,
        bool flag_no_recover_homing)
    {
        GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, no_move_time, flag_no_recover_homing ? 1 : 0, 0);
    }

    private static void GmPlySeqInitPinballAir(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_move_time,
        bool flag_no_recover_homing,
        int no_spddown_timer)
    {
        GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, no_move_time, flag_no_recover_homing ? 1 : 0,
            no_spddown_timer);
    }

    private static void GmPlySeqInitPinballAir(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_move_time,
        int flag_no_recover_homing,
        int no_spddown_timer)
    {
        uint num1 = 0;
        if (((int) ply_work.rect_work[1].flag & 4) != 0)
            num1 = 1U;
        uint num2 = 0;
        if ((ply_work.player_flag & 128) != 0)
            num2 = 1U;
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequenceState(ply_work, 46);
        GmPlayerStateGimmickInit(ply_work);
        if (((uint)flag_no_recover_homing & num2) != 0)
            ply_work.player_flag |= 128U;
        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.obj_work.move_flag |= 128U;
        ply_work.obj_work.flag &= 4294967293U;
        ply_work.player_flag |= 32U;
        ply_work.obj_work.spd_fall = FX_Mul(ply_work.obj_work.spd_fall, FX_F32_TO_FX32(1.1f));
        ply_work.obj_work.dir.y = 0;
        GmPlySeqGmkSpdSet(ply_work, spd_x, spd_y);
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.z = 0;
        ply_work.obj_work.spd_m = 0;
        bool flag = false;
        if (((int) ply_work.obj_work.disp_flag & 4) != 0)
            flag = true;
        int act_state = ply_work.act_state;
        switch (act_state)
        {
            case 0:
            case 1:
            case 8:
            case 9:
            case 10:
            case 19:
            case 20:
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
                act_state = 40;
                flag = true;
                break;
            case 41:
                act_state = 40;
                flag = true;
                break;
            case 43:
                act_state = 42;
                flag = true;
                break;
        }

        GmPlayerActionChange(ply_work, act_state);
        if (flag)
            ply_work.obj_work.disp_flag |= 4U;
        ply_work.no_spddown_timer = no_spddown_timer * 4096;
        ply_work.obj_work.user_timer = no_move_time;
        ply_work.obj_work.user_flag = 1U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainPinballAir);
        if (num1 != 0U)
            GmPlayerSetAtk(ply_work);
        if (((int) ply_work.gmk_flag & 4096) == 0)
            return;
        ply_work.obj_work.spd.z = ply_work.obj_work.spd.y;
        ply_work.obj_work.spd.y = 0;
        if (ply_work.obj_work.pos.z >= 0)
            return;
        ply_work.obj_work.spd.z = -ply_work.obj_work.spd.z;
    }

    private static void GmPlySeqInitFlipper(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (OBS_OBJECT_WORK) com_work)
            return;
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequenceState(ply_work, 47);
        GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = com_work.obj_work;
        if (ply_work.act_state != 39)
        {
            GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
            GmPlyEfctCreateSpinJumpBlur(ply_work);
        }

        ply_work.obj_work.spd.x = spd_x;
        ply_work.obj_work.spd.y = spd_y;
        ply_work.obj_work.spd.z = 0;
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.z = 0;
        ply_work.obj_work.move_flag &= 4294967166U;
        ply_work.obj_work.move_flag |= 33040U;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.dir.z = 0;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainFlipper);
        GmSoundPlaySE("Spin");
        if (ply_work.obj_work.spd.x > 0)
        {
            ply_work.obj_work.disp_flag &= 4294967294U;
        }
        else
        {
            if (ply_work.obj_work.spd.x >= 0)
                return;
            ply_work.obj_work.disp_flag |= 1U;
        }
    }

    private static void GmPlySeqGmkInitForceSpin(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequenceState(ply_work, 51);
        GmPlayerStateGimmickInit(ply_work);
        if (ply_work.act_state != 26 && ply_work.act_state != 27)
            GmSoundPlaySE("Spin");
        if (ply_work.act_state != 26)
        {
            GmPlayerActionChange(ply_work, 26);
            ply_work.obj_work.disp_flag |= 4U;
            GmPlyEfctCreateSpinDashBlur(ply_work, 0U);
        }

        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainForceSpin);
        ply_work.obj_work.user_timer = ply_work.obj_work.spd_m;
        ply_work.obj_work.user_flag = 0U;
        GmPlayerSetAtk(ply_work);
        ply_work.obj_work.move_flag |= 193U;
        ply_work.gmk_obj = null;
    }

    private static void GmPlySeqGmkInitForceSpinDec(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequenceState(ply_work, 52);
        GmPlayerStateGimmickInit(ply_work);
        if (ply_work.act_state != 26 && ply_work.act_state != 27)
            GmSoundPlaySE("Spin");
        if (ply_work.act_state != 26)
        {
            GmPlayerActionChange(ply_work, 26);
            ply_work.obj_work.disp_flag |= 4U;
            GmPlyEfctCreateSpinDashBlur(ply_work, 0U);
        }

        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainForceSpinDec);
        ply_work.obj_work.user_timer = ply_work.obj_work.spd_m;
        ply_work.obj_work.user_flag = 1U;
        GmPlayerSetAtk(ply_work);
        ply_work.obj_work.move_flag |= 193U;
        ply_work.gmk_obj = null;
    }

    private static void GmPlySeqGmkInitForceSpinFall(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 53);
        ply_work.obj_work.move_flag |= 32912U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainForceSpinFall);
        ply_work.obj_work.spd.x =
            FX_Mul(ply_work.obj_work.spd_m, mtMathCos(ply_work.obj_work.dir.z));
        ply_work.obj_work.spd.y =
            FX_Mul(ply_work.obj_work.spd_m, mtMathSin(ply_work.obj_work.dir.z));
        if (((int) ply_work.obj_work.user_flag & 1) == 0 || MTM_MATH_ABS(ply_work.obj_work.spd.x) <=
            MTM_MATH_ABS(ply_work.obj_work.spd.y))
            return;
        ply_work.obj_work.spd.y = ply_work.obj_work.spd.x >> 1;
        if (ply_work.obj_work.spd.y < 0)
            ply_work.obj_work.spd.y = -ply_work.obj_work.spd.y;
        ply_work.obj_work.spd.x >>= 1;
    }

    private static void GmPlySeqInitPinballCtpltHold(
        GMS_PLAYER_WORK ply_work,
        GMS_ENEMY_COM_WORK gmk_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 48);
        if (ply_work.prev_seq_state != 51 && ply_work.prev_seq_state != 52)
        {
            GmPlayerActionChange(ply_work, 26);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.dir.z = 0;
        GmPlyEfctCreateSpinDashBlur(ply_work, 0U);
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainPinballCtpltHold);
        ply_work.gmk_obj = gmk_work.obj_work;
    }

    private static void GmPlySeqInitPinballCtplt(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequenceState(ply_work, spd_x == 0 ? 49 : 50);
        GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        if (spd_x != 0)
        {
            ply_work.obj_work.move_flag &= 4294967279U;
            ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainPinballCtplt);
            if (spd_x > 0)
                ply_work.obj_work.disp_flag &= 4294967294U;
            else
                ply_work.obj_work.disp_flag |= 1U;
        }
        else
        {
            ply_work.obj_work.move_flag |= 144U;
            GmPlySeqGmkSpdSet(ply_work, spd_x, spd_y);
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.dir.z = 0;
            ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainPinballAir);
        }

        ply_work.obj_work.flag &= 4294967293U;
        GmPlayerSetAtk(ply_work);
        ply_work.no_spddown_timer = 2457600;
        GmSoundPlaySE("Catapult");
        GmPlyEfctCreateSpinJumpBlur(ply_work);
    }

    private static void GmPlySeqInitMoveGear(
        GMS_PLAYER_WORK ply_work,
        OBS_OBJECT_WORK gmk_obj,
        bool cam_adjust)
    {
        GmPlySeqChangeSequenceState(ply_work, 54);
        GmPlySeqLandingSet(ply_work, 0);
        ply_work.obj_work.dir.z = 0;
        GmPlySeqGmkInitGimmickDependInit(ply_work, gmk_obj, 0, 0, 0);
        ply_work.player_flag |= 514U;
        ply_work.obj_work.user_flag = 1U;
        ply_work.obj_work.move_flag |= 257U;
        if (ply_work.obj_work.spd_m != 0)
            GmPlayerWalkActionSet(ply_work);
        else if (ply_work.act_state != 0)
        {
            GmPlayerActionChange(ply_work, 0);
            ply_work.obj_work.disp_flag |= 4U;
        }

        if (cam_adjust)
        {
            GmPlayerCameraOffsetSet(ply_work, 0, -48);
            GmCameraAllowSet(0.0f, 0.0f, 0.0f);
        }

        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainMoveGear);
    }

    private static void GmPlySeqInitSteamPipeIn(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequenceState(ply_work, 57);
        GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 256U;
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_m = 0;
        GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainSteamPipe);
        ply_work.gmk_obj = null;
        ply_work.obj_work.user_timer = 0;
        GmPlyEfctCreateSteamPipe(ply_work);
        GmPlyEfctCreateSpinDashBlur(ply_work, 0U);
    }

    private static void GmPlySeqInitSteamPipeOut(GMS_PLAYER_WORK ply_work, int spd_x)
    {
        ply_work.obj_work.move_flag &= 4294967039U;
        ply_work.obj_work.move_flag |= 128U;
        GmPlayerSetDefNormal(ply_work);
        ply_work.obj_work.user_timer = 60;
        GmPlySeqGmkInitGmkJump(ply_work, spd_x, 0);
        GmPlayerActionChange(ply_work, 26);
        GmPlayerSetAtk(ply_work);
    }

    private static void GmPlySeqGmkInitPopSteamJump(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_jump_move_time)
    {
        if (ply_work.seq_state != 58)
            GmPlySeqChangeSequenceState(ply_work, 58);
        ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_m = 0;
        GmPlySeqGmkInitGmkJump(ply_work, spd_x, spd_y);
        if (no_jump_move_time <= 0)
            return;
        GmPlySeqSetNoJumpMoveTime(ply_work, no_jump_move_time);
    }

    private static void GmPlySeqInitDrainTank(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 55);
        GmPlayerStateGimmickInit(ply_work);
        GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4294967166U;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd.z = 0;
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.z = 0;
        ply_work.seq_func = null;
    }

    private static void GmPlySeqInitDrainTankFall(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 56);
        GmPlayerStateGimmickInit(ply_work);
        GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 32912U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.z = 0;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainDrainTank);
    }

    private static void GmPlySeqInitSplIn(GMS_PLAYER_WORK ply_work, VecFx32 pos)
    {
        GmPlySeqChangeSequenceState(ply_work, 59);
        GmPlayerStateGimmickInit(ply_work);
        if (ply_work.act_state != 26)
        {
            GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag |= 41232U;
        ply_work.obj_work.move_flag &= 4294967166U;
        ply_work.obj_work.pos.x = pos.x;
        ply_work.obj_work.pos.y = pos.y;
        ply_work.seq_func = null;
        g_gm_main_system.game_flag |= 16384U;
    }

    private static void GmPlySeqGmkInitBoss2Catch(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 60);
        if (ply_work.act_state != 39)
        {
            GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag |= 41232U;
        ply_work.obj_work.move_flag &= 4294967166U;
        ply_work.seq_func = null;
    }

    private static void GmPlySeqGmkInitBoss5Quake(GMS_PLAYER_WORK ply_work, int no_move_time)
    {
        GmPlySeqChangeSequenceState(ply_work, 61);
        if (ply_work.act_state != 34)
        {
            GmPlayerActionChange(ply_work, 34);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.move_flag |= 40960U;
        ply_work.obj_work.user_timer = no_move_time;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainBoss5Quake);
    }

    private static void GmPlySeqGmkInitEndingDemo1(GMS_PLAYER_WORK ply_work)
    {
        GmPlySeqChangeSequenceState(ply_work, 62);
        GmPlayerActionChange(ply_work, 77);
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainEndingFrontSide);
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
    }

    private static void GmPlySeqGmkInitEndingDemo2(GMS_PLAYER_WORK ply_work, bool type2)
    {
        GmPlySeqChangeSequenceState(ply_work, 63);
        GmPlayerActionChange(ply_work, 39);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainEndingFinish);
        ply_work.obj_work.spd.y = -10240;
        ply_work.obj_work.spd_add.y = 168;
        ply_work.obj_work.dir.y = 16384;
        ply_work.obj_work.user_work = 0U;
        ply_work.obj_work.user_flag = 0U;
        if (type2)
            ply_work.obj_work.user_flag = 1U;
        ply_work.obj_work.move_flag |= 33040U;
    }

    private static void GmPlySeqGmkInitTruckDanger(
        GMS_PLAYER_WORK ply_work,
        OBS_OBJECT_WORK gmk_obj)
    {
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequenceState(ply_work, 64);
        GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = gmk_obj;
        GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
        ply_work.player_flag &= 4294967280U;
        ply_work.gmk_flag &= 4293918719U;
        ply_work.gmk_flag |= 32768U;
        nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
        int num1 = 32768 - ply_work.obj_work.dir.z +
                   (short) (g_gm_main_system.pseudofall_dir - ply_work.obj_work.dir_fall);
        ply_work.gmk_work1 = 0;
        ply_work.gmk_work2 = 69632;
        ply_work.gmk_work3 = 0;
        ply_work.obj_work.user_work = 0U;
        uint num2 = gmPlayerCheckTruckAirFoot(ply_work);
        if (ply_work.obj_work.dir.z <= 32768)
        {
            if (((int) num2 & 1) != 0)
            {
                num1 -= 6144;
                ply_work.obj_work.user_timer = 1024;
            }
            else
            {
                ply_work.player_flag |= 2U;
                GmPlayerActionChange(ply_work, 74);
            }
        }
        else if (((int) num2 & 2) != 0)
        {
            num1 += 6144;
            ply_work.player_flag |= 4U;
            ply_work.obj_work.user_timer = -1024;
        }
        else
        {
            ply_work.player_flag |= 2U;
            GmPlayerActionChange(ply_work, 74);
        }

        ply_work.gmk_work0 = (ushort) (num1 / 17);
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainTruckDanger);
        GmSoundPlaySE("Lorry2");
    }

    private static void GmPlySeqGmkInitTruckDangerRet(
        GMS_PLAYER_WORK ply_work,
        OBS_OBJECT_WORK gmk_obj)
    {
        int gmkWork3 = ply_work.gmk_work3;
        uint num1 = ply_work.player_flag & 13U;
        GmPlySeqChangeSequenceState(ply_work, 64);
        GmPlayerStateGimmickInit(ply_work);
        ply_work.player_flag |= num1;
        ply_work.gmk_obj = gmk_obj;
        GmPlayerActionChange(ply_work, 76);
        ply_work.gmk_flag |= 32768U;
        int num2 = 32768 - ply_work.obj_work.dir.z - gmkWork3 +
                   (short) (g_gm_main_system.pseudofall_dir - ply_work.obj_work.dir_fall);
        if (num2 > 0)
            num2 = (ushort) num2;
        ply_work.gmk_work0 = -num2 / 14;
        ply_work.gmk_work1 = num2;
        ply_work.gmk_work2 = 0;
        ply_work.gmk_work3 = gmkWork3;
        ply_work.obj_work.vib_timer = 0;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainTruckDangerRet);
    }

    private static void GmPlySeqGmkInitGmkJump(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        GmPlySeqGmkInitGmkJump(ply_work, spd_x, spd_y, true);
    }

    private static void GmPlySeqGmkInitGmkJump(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        bool set_act)
    {
        if ((ply_work.player_flag & 1024) != 0)
            return;
        GmPlayerStateGimmickInit(ply_work);
        if (((int) ply_work.obj_work.move_flag & 1) != 0)
            ply_work.obj_work.spd.x = ply_work.obj_work.spd_m;
        GmPlySeqLandingSet(ply_work, 0);
        if ((ply_work.player_flag & 262144) != 0)
            ObjObjectSpdDirFall(ref spd_x, ref spd_y, (ushort) -ply_work.jump_pseudofall_dir);
        else
            ObjObjectSpdDirFall(ref spd_x, ref spd_y, ply_work.obj_work.dir_fall);
        if (((int) ply_work.obj_work.move_flag & 16) == 0)
            ply_work.camera_jump_pos_y = ply_work.obj_work.pos.y;
        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4294967294U;
        if (spd_x != 0)
        {
            if (set_act)
                GmPlayerActionChange(ply_work, 47);
            ply_work.obj_work.spd.x = spd_x;
            if (ply_work.obj_work.spd.x < 0)
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = 0;
                ply_work.obj_work.disp_flag |= 1U;
            }
            else
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = 0;
                ply_work.obj_work.disp_flag &= 4294967294U;
            }

            ply_work.no_spddown_timer = 262144;
        }
        else
        {
            if (set_act)
            {
                GmPlayerActionChange(ply_work, 44);
                ply_work.obj_work.disp_flag |= 4U;
            }

            ply_work.obj_work.spd.x =
                FX_Mul(ply_work.obj_work.spd_m, mtMathCos(ply_work.obj_work.dir.z));
        }

        ply_work.obj_work.spd.y = spd_y == 0
            ? FX_Mul(ply_work.obj_work.spd_m, mtMathSin(ply_work.obj_work.dir.z))
            : spd_y;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
        GmPlySeqSetJumpState(ply_work, 0, 3U);
        if ((ply_work.player_flag & 67108864) == 0)
            return;
        GMD_PLAYER_WATERJUMP_SET(ref ply_work.obj_work.spd.x);
        GMD_PLAYER_WATERJUMP_SET(ref ply_work.obj_work.spd.y);
    }

    private static void GmPlySeqGmkInitGimmickDependInit(
        GMS_PLAYER_WORK ply_work,
        OBS_OBJECT_WORK gmk_obj,
        int ofst_x,
        int ofst_y,
        int ofst_z)
    {
        if (ply_work.gmk_obj == gmk_obj)
            return;
        GmPlayerSpdParameterSet(ply_work);
        GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = gmk_obj;
        ply_work.obj_work.move_flag |= 40976U;
        ply_work.obj_work.move_flag &= 4294967103U;
        ply_work.obj_work.user_flag = 0U;
        ply_work.player_flag &= 4294967280U;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.obj_work.user_timer = 0;
        ply_work.gmk_work0 = ofst_x;
        ply_work.gmk_work1 = ofst_y;
        ply_work.gmk_work2 = ofst_z;
        ply_work.seq_func = new seq_func_delegate(GmPlySeqGmkMainGimmickDepend);
    }

    private static void GmPlySeqGmkMainGimmickDepend(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK) ply_work;
        OBS_OBJECT_WORK gmkObj1 = ply_work.gmk_obj;
        if (gmkObj1 != null)
        {
            GMS_ENEMY_COM_WORK gmkObj2 = (GMS_ENEMY_COM_WORK) ply_work.gmk_obj;
            if (((int) gmkObj2.enemy_flag & 15) != 0)
            {
                ply_work.gmk_obj = null;
                if (((int) gmkObj2.enemy_flag & 2) != 0)
                {
                    obsObjectWork.spd.x = gmkObj1.spd.x;
                    obsObjectWork.spd.y = gmkObj1.spd.y;
                }
                else if (((int) gmkObj2.enemy_flag & 4) != 0)
                    obsObjectWork.spd.x = gmkObj1.spd_m;
                else if (((int) gmkObj2.enemy_flag & 8) != 0)
                {
                    obsObjectWork.spd.x = obsObjectWork.move.x;
                    obsObjectWork.spd.y = obsObjectWork.move.y;
                }
            }
            else
            {
                obsObjectWork.prev_pos.x = obsObjectWork.pos.x;
                obsObjectWork.prev_pos.y = obsObjectWork.pos.y;
                obsObjectWork.prev_pos.z = obsObjectWork.pos.z;
                if ((ply_work.player_flag & 1) != 0)
                    obsObjectWork.pos.Assign(gmkObj1.pos);
                else if ((ply_work.player_flag & 2) != 0)
                {
                    obsObjectWork.pos.x = gmkObj1.pos.x + gmkObj2.target_dp_pos.x;
                    obsObjectWork.pos.y = gmkObj1.pos.y + gmkObj2.target_dp_pos.y;
                    obsObjectWork.pos.z = gmkObj1.pos.z + gmkObj2.target_dp_pos.z;
                }
                else if ((ply_work.player_flag & 4) != 0)
                {
                    NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
                    NNS_VECTOR nnsVector = new NNS_VECTOR(0.0f, -1f, 0.0f);
                    nnMakeUnitMatrix(nnsMatrix);
                    nnRotateXYZMatrix(nnsMatrix, nnsMatrix, -gmkObj2.target_dp_dir.x,
                        gmkObj2.target_dp_dir.y, gmkObj2.target_dp_dir.z);
                    nnTransformVector(nnsVector, nnsMatrix, nnsVector);
                    nnScaleVector(nnsVector, nnsVector, FXM_FX32_TO_FLOAT(gmkObj2.target_dp_dist));
                    obsObjectWork.pos.x = gmkObj1.pos.x + FXM_FLOAT_TO_FX32(nnsVector.x);
                    obsObjectWork.pos.y = gmkObj1.pos.y + FXM_FLOAT_TO_FX32(nnsVector.y);
                    obsObjectWork.pos.z = gmkObj1.pos.z + FXM_FLOAT_TO_FX32(nnsVector.z);
                    GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
                }

                if ((ply_work.player_flag & 8) != 0)
                    obsObjectWork.dir.Assign(gmkObj2.target_dp_dir);
                obsObjectWork.move.x = obsObjectWork.pos.x - obsObjectWork.prev_pos.x;
                obsObjectWork.move.y = obsObjectWork.pos.y - obsObjectWork.prev_pos.y;
                obsObjectWork.move.z = obsObjectWork.pos.z - obsObjectWork.prev_pos.z;
                if (((int) obsObjectWork.user_flag & 1) != 0 && gmkObj1.vib_timer != 0)
                    obsObjectWork.vib_timer = gmkObj1.vib_timer + 4096;
                if (((int) obsObjectWork.move_flag & 8192) != 0)
                    obsObjectWork.flow.x = obsObjectWork.flow.y = obsObjectWork.flow.z = 0;
            }
        }

        if (ply_work.gmk_obj != null)
            return;
        GmPlayerStateGimmickInit(ply_work);
    }

    private static void GmPlySeqGmkSpdSet(GMS_PLAYER_WORK ply_work, int spd_x, int spd_y)
    {
        if (spd_x < 0)
            ply_work.obj_work.disp_flag |= 1U;
        else if (spd_x > 0)
            ply_work.obj_work.disp_flag &= 4294967294U;
        if (((int) ply_work.obj_work.move_flag & 16) != 0)
        {
            if (((int) ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd.x > spd_x ||
                ((int) ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd.x < spd_x)
                ply_work.obj_work.spd.x = spd_x;
            if (MTM_MATH_ABS(ply_work.obj_work.spd.y) >= MTM_MATH_ABS(spd_y))
                return;
            ply_work.obj_work.spd.y = spd_y;
        }
        else
        {
            switch ((ply_work.obj_work.dir.z + 8192 & 49152) >> 14)
            {
                case 0:
                case 2:
                    if (((int) ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd_m > spd_x ||
                        ((int) ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd_m < spd_x)
                        ply_work.obj_work.spd_m = spd_x;
                    if (MTM_MATH_ABS(ply_work.obj_work.spd.y) >= MTM_MATH_ABS(spd_y))
                        break;
                    ply_work.obj_work.spd.y = spd_y;
                    if (ply_work.obj_work.spd.y >= 0)
                        break;
                    ply_work.obj_work.move_flag |= 16U;
                    break;
                case 1:
                    if (spd_y > 0 && ply_work.obj_work.spd_m < spd_y || spd_y < 0 && ply_work.obj_work.spd_m > spd_y)
                        ply_work.obj_work.spd_m = spd_y;
                    if (ply_work.obj_work.spd_m > 0)
                        ply_work.obj_work.disp_flag &= 4294967294U;
                    else
                        ply_work.obj_work.disp_flag |= 1U;
                    if (MTM_MATH_ABS(ply_work.obj_work.spd.x) >= MTM_MATH_ABS(spd_x))
                        break;
                    ply_work.obj_work.spd.x = spd_x;
                    break;
                case 3:
                    if (spd_y > 0 && ply_work.obj_work.spd_m > -spd_y || spd_y < 0 && ply_work.obj_work.spd_m < -spd_y)
                        ply_work.obj_work.spd_m = -spd_y;
                    if (ply_work.obj_work.spd_m > 0)
                        ply_work.obj_work.disp_flag &= 4294967294U;
                    else
                        ply_work.obj_work.disp_flag |= 1U;
                    if (MTM_MATH_ABS(ply_work.obj_work.spd.x) >= MTM_MATH_ABS(spd_x))
                        break;
                    ply_work.obj_work.spd.x = spd_x;
                    break;
            }
        }
    }

    private static void GmPlySeqGmkTruckSpdSet(
        GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        if (spd_x < 0)
            ply_work.gmk_flag |= 1048576U;
        else if (spd_x > 0)
            ply_work.gmk_flag &= 4293918719U;
        if (((int) ply_work.obj_work.move_flag & 16) != 0)
        {
            if (((int) ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd.x > spd_x ||
                ((int) ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd.x < spd_x)
                ply_work.obj_work.spd.x = spd_x;
            if (MTM_MATH_ABS(ply_work.obj_work.spd.y) >= MTM_MATH_ABS(spd_y))
                return;
            ply_work.obj_work.spd.y = spd_y;
        }
        else
        {
            switch (((ushort) (ply_work.obj_work.dir.z + (uint) ply_work.obj_work.dir_fall) + 8192 &
                     49152) >> 14)
            {
                case 0:
                case 2:
                    if (((int) ply_work.gmk_flag & 1048576) != 0 && ply_work.obj_work.spd_m > spd_x ||
                        ((int) ply_work.gmk_flag & 1048576) == 0 && ply_work.obj_work.spd_m < spd_x)
                        ply_work.obj_work.spd_m = spd_x;
                    if (MTM_MATH_ABS(ply_work.obj_work.spd.y) >= MTM_MATH_ABS(spd_y))
                        break;
                    ply_work.obj_work.spd.y = spd_y;
                    if (ply_work.obj_work.spd.y >= 0)
                        break;
                    ply_work.obj_work.move_flag |= 16U;
                    break;
                case 1:
                    if (spd_y > 0 && ply_work.obj_work.spd_m < spd_y || spd_y < 0 && ply_work.obj_work.spd_m > spd_y)
                        ply_work.obj_work.spd_m = spd_y;
                    if (ply_work.obj_work.spd_m > 0)
                        ply_work.gmk_flag &= 4293918719U;
                    else
                        ply_work.gmk_flag |= 1048576U;
                    if (MTM_MATH_ABS(ply_work.obj_work.spd.x) >= MTM_MATH_ABS(spd_x))
                        break;
                    ply_work.obj_work.spd.x = spd_x;
                    break;
                case 3:
                    if (spd_y > 0 && ply_work.obj_work.spd_m > -spd_y || spd_y < 0 && ply_work.obj_work.spd_m < -spd_y)
                        ply_work.obj_work.spd_m = -spd_y;
                    if (ply_work.obj_work.spd_m > 0)
                        ply_work.gmk_flag &= 4293918719U;
                    else
                        ply_work.gmk_flag |= 1048576U;
                    if (MTM_MATH_ABS(ply_work.obj_work.spd.x) >= MTM_MATH_ABS(spd_x))
                        break;
                    ply_work.obj_work.spd.x = spd_x;
                    break;
            }
        }
    }

    private static void gmPlySeqGmkMainGimmickRockRidePush(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        objWork.obj_3d.speed[0] -= 0.02f;
        if (objWork.obj_3d.speed[0] <= 0.5)
            objWork.obj_3d.speed[0] = 0.5f;
        if (((int) objWork.disp_flag & 8) == 0)
            return;
        objWork.user_timer = 5;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainGimmickRockRideStartWait);
        objWork.obj_3d.speed[0] = 1f;
    }

    private static void gmPlySeqGmkMainGimmickRockRideStartWait(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        --objWork.user_timer;
        if (objWork.user_timer > 0)
            return;
        objWork.user_timer = 0;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainGimmickRockRideStartJump);
    }

    private static void gmPlySeqGmkMainGimmickRockRideStartJump(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        if (((int) objWork.disp_flag & 8) == 0)
            return;
        OBS_OBJECT_WORK gmkObj = ply_work.gmk_obj;
        int spd_x = 13824;
        if (gmkObj.pos.x < objWork.pos.x)
            spd_x = -spd_x;
        GmPlySeqGmkInitGmkJump(ply_work, spd_x, -24576);
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainGimmickRockRideStartFall);
    }

    private static void gmPlySeqGmkMainGimmickRockRideStartFall(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.move_flag & 1) == 0)
            return;
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void gmPlySeqGmkMainGimmickRockRide(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        OBS_OBJECT_WORK gmkObj = ply_work.gmk_obj;
        if (gmkObj.spd_m == 0)
        {
            ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkMainGimmickRockRideStop);
            GmPlayerCameraOffsetSet(ply_work, 0, 0);
            GmCameraAllowReset();
        }
        else
        {
            int num1 = FX_Mul(61440,
                FX_F32_TO_FX32(-GmPlayerKeyGetGimmickRotZ(ply_work) /
                               GMD_GMK_ROCK_RIDE_KEY_ANGLE_LIMIT));
            int num2 = (gmkObj.spd_m <= 0 ? num1 - short.MinValue : num1 - 32768) - objWork.spd_m;
            if (num2 > 0)
                objWork.spd_m += 384;
            else if (num2 < 0)
                objWork.spd_m -= 384;
            int a = gmkObj.spd_m - objWork.spd_m;
            int num3 = MTM_MATH_ABS(a);
            if (num3 >= 15360)
            {
                int spd_x = 16384;
                if (a < 0)
                    spd_x = -spd_x;
                GmPlySeqChangeSequence(ply_work, 16);
                GmPlySeqGmkInitGmkJump(ply_work, spd_x, 12288);
                GmPlayerCameraOffsetSet(ply_work, 0, 0);
                GmCameraAllowReset();
            }
            else
            {
                if (num3 >= 2816)
                {
                    if (ply_work.act_state != 61)
                    {
                        GmPlayerActionChange(ply_work, 61);
                        objWork.disp_flag |= 4U;
                    }
                }
                else if (ply_work.act_state != 60)
                {
                    GmPlayerActionChange(ply_work, 60);
                    objWork.disp_flag |= 4U;
                }

                ((GMS_ENEMY_COM_WORK) gmkObj).target_dp_dir.z = (ushort) (a * 4 / 5);
                GmPlySeqGmkMainGimmickDepend(ply_work);
            }
        }
    }

    private static void gmPlySeqGmkMainGimmickRockRideStop(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        GmPlySeqGmkInitGmkJump(ply_work, objWork.spd.x, objWork.spd.y);
    }

    private static void gmPlySeqGmkMainGimmickBreathing(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        GmPlySeqLandingSet(ply_work, 0);
        if (((int) objWork.disp_flag & 8) == 0)
            return;
        if (((int) objWork.move_flag & 1) != 0)
            GmPlySeqChangeSequence(ply_work, 0);
        else
            GmPlySeqChangeSequence(ply_work, 16);
    }

    private static void gmPlySeqGmkMainDashPanel(GMS_PLAYER_WORK ply_work)
    {
        --ply_work.obj_work.user_timer;
        if (ply_work.obj_work.user_timer > 0 && ply_work.obj_work.spd_m != 0)
            return;
        GmPlayerSpdParameterSet(ply_work);
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void gmPlySeqGmkMainWaterSlider(GMS_PLAYER_WORK ply_work)
    {
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        if (((int) objWork.move_flag & 1) == 0)
        {
            nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
            ply_work.gmk_flag &= 4294934527U;
            GmPlySeqChangeSequence(ply_work, 0);
        }
        else
        {
            if (!GmPlayerKeyCheckJumpKeyPush(ply_work))
                return;
            nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
            ply_work.gmk_flag &= 4294934527U;
            objWork.spd_m /= 2;
            GmPlySeqChangeSequence(ply_work, 17);
        }
    }

    private static void gmPlySeqGmkMainSpipe(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.gmk_flag & 65536) != 0)
        {
            if (((int) ply_work.obj_work.move_flag & 1) == 0)
            {
                int num1 = FX_Mul(40960, mtMathCos(ply_work.obj_work.dir.z - 16384));
                ply_work.obj_work.pos.x -= num1;
                int num2 = FX_Mul(40960, mtMathSin(ply_work.obj_work.dir.z - 16384));
                ply_work.obj_work.pos.y -= num2;
                ply_work.obj_work.spd.x = 0;
                ply_work.obj_work.spd.y = 0;
            }

            ply_work.obj_work.move_flag &= 4294934511U;
            if (ply_work.obj_work.spd_m == 0)
            {
                ply_work.obj_work.spd_m = 8192;
                GmSoundPlaySE("Spin");
            }
        }
        else
            GmPlySeqChangeSequence(ply_work, 10);

        ply_work.gmk_flag &= 4294901759U;
    }

    private static void gmPlySeqGmkScrewMain(GMS_PLAYER_WORK ply_work)
    {
        GmPlayerWalkActionCheck(ply_work);
        GMS_PLY_SEQ_STATE_DATA[] seqStateDataTbl = ply_work.seq_state_data_tbl;
        if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < ply_work.spd2 &&
            ((int) ply_work.obj_work.move_flag & 1) == 0)
        {
            ply_work.obj_work.dir.x = ply_work.obj_work.dir.y = ply_work.obj_work.dir.z = 0;
            ply_work.obj_work.move_flag &= 4294959103U;
            GmPlySeqChangeSequence(ply_work, 16);
        }
        else
        {
            if (ply_work.timer != 0)
                --ply_work.timer;
            else if (((int) ply_work.obj_work.move_flag & 13) != 0)
            {
                ply_work.obj_work.dir.x = ply_work.obj_work.dir.y = ply_work.obj_work.dir.z = 0;
                ply_work.obj_work.move_flag &= 4294959103U;
                ply_work.obj_work.spd.x = ply_work.obj_work.spd_m;
                GmPlySeqLandingSet(ply_work, 0);
                GmPlySeqChangeSequence(ply_work, 0);
                return;
            }

            GmPlySeqMoveWalk(ply_work);
            gmPlySeqGmkMoveScrew(ply_work, 1530320, 288, 38);
        }
    }

    private static void gmPlySeqGmkMoveScrew(
        GMS_PLAYER_WORK ply_work,
        int screw_length,
        short screw_width,
        short screw_height)
    {
        ply_work.obj_work.user_timer += MTM_MATH_ABS(ply_work.obj_work.spd_m);
        int userTimer = ply_work.obj_work.user_timer;
        sbyte num1 = (sbyte) (userTimer / screw_length);
        int num2 = (userTimer - screw_length * num1 << 8) / screw_length << 4;
        ushort num3 = (ushort) (num2 << 4 & ushort.MaxValue);
        ply_work.obj_work.dir.x = num3;
        if (((int) ply_work.obj_work.user_work & GMD_GMK_SCREW_EVE_FLAG_LEFT) != 0)
            ply_work.obj_work.dir.x = (ushort) -ply_work.obj_work.dir.x;
        ply_work.obj_work.dir.z = ply_work.obj_work.dir.x >= 16384
            ? (ply_work.obj_work.dir.x >= 32768
                ? (ply_work.obj_work.dir.x >= 49152
                    ? (ushort) (65536U - ply_work.obj_work.dir.x)
                    : (ushort) (ply_work.obj_work.dir.x - 32768U))
                : (ushort) (16384 - (ply_work.obj_work.dir.x - 16384)))
            : ply_work.obj_work.dir.x;
        ply_work.obj_work.dir.z >>= 1;
        if (ply_work.obj_work.dir.x < 32768)
            ply_work.obj_work.dir.z = (ushort) -ply_work.obj_work.dir.z;
        ply_work.obj_work.dir.x = (ushort) -ply_work.obj_work.dir.x;
        int num4 = mtMathCos(num3);
        screw_height -= ply_work.obj_work.field_rect[3];
        screw_height = (short) -screw_height;
        ply_work.obj_work.prev_pos.x = ply_work.obj_work.pos.x;
        ply_work.obj_work.prev_pos.y = ply_work.obj_work.pos.y;
        ply_work.obj_work.pos.x = ((int) ply_work.obj_work.user_work & GMD_GMK_SCREW_EVE_FLAG_LEFT) == 0
            ? ply_work.gmk_work0 + (num1 * screw_width << 12) + screw_width * num2
            : ply_work.gmk_work0 - (num1 * screw_width << 12) - screw_width * num2;
        ply_work.obj_work.pos.y = ply_work.gmk_work1 + (screw_height << 12) - num4 * screw_height;
        ply_work.obj_work.move.x = ply_work.obj_work.pos.x - ply_work.obj_work.prev_pos.x;
        ply_work.obj_work.move.y = ply_work.obj_work.pos.y - ply_work.obj_work.prev_pos.y;
    }

    private static void gmPlySeqGmkCannonWait(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.pos.y < ply_work.gmk_obj.pos.y)
            return;
        ply_work.obj_work.pos.y = ply_work.gmk_obj.pos.y;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_fall = 0;
        ply_work.seq_func = null;
        ply_work.obj_work.move_flag &= 4294967167U;
    }

    private static void gmPlySeqGmkStopperMove(GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.pos.x = (ply_work.obj_work.pos.x + ply_work.gmk_obj.pos.x) / 2;
        if (MTM_MATH_ABS(ply_work.obj_work.pos.x - ply_work.gmk_obj.pos.x) < 1024)
            ply_work.obj_work.pos.x = ply_work.gmk_obj.pos.x;
        if (ply_work.obj_work.pos.y > ply_work.gmk_obj.pos.y)
        {
            ply_work.obj_work.pos.y -= 32768;
            if (ply_work.obj_work.pos.y < ply_work.gmk_obj.pos.y)
                ply_work.obj_work.pos.y = ply_work.gmk_obj.pos.y;
        }
        else
        {
            ply_work.obj_work.pos.y += 32768;
            if (ply_work.obj_work.pos.y > ply_work.gmk_obj.pos.y)
                ply_work.obj_work.pos.y = ply_work.gmk_obj.pos.y;
        }

        if (ply_work.obj_work.pos.x != ply_work.gmk_obj.pos.x || ply_work.obj_work.pos.y != ply_work.gmk_obj.pos.y)
            return;
        ply_work.seq_func = new seq_func_delegate(gmPlySeqGmkStopperWait);
    }

    private static void gmPlySeqGmkStopperWait(GMS_PLAYER_WORK ply_work)
    {
    }

    private static void gmPlySeqGmkStopperEnd(GMS_PLAYER_WORK ply_work)
    {
        bool flag = false;
        if (ply_work.gmk_obj == null)
            flag = true;
        else if (ply_work.gmk_obj.user_timer < ply_work.obj_work.pos.y >> 12)
            flag = true;
        if (!flag)
            return;
        int y = ply_work.obj_work.spd.y;
        GmPlySeqChangeSequence(ply_work, 16);
        GmPlySeqGmkInitGmkJump(ply_work, 0, y, false);
        ply_work.gmk_obj = null;
        if (ply_work.act_state != 26)
        {
            GmPlayerActionChange(ply_work, 26);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.flag &= 4294967293U;
    }

    private static void gmPlySeqGmkSeesaw(GMS_PLAYER_WORK ply_work)
    {
    }

    private static void gmPlySeqGmkMainPinball(GMS_PLAYER_WORK ply_work)
    {
        --ply_work.obj_work.user_timer;
        if (ply_work.obj_work.user_timer <= 0 || ply_work.obj_work.spd_m == 0)
        {
            GmPlySeqChangeSequence(ply_work, 0);
        }
        else
        {
            if (((int) ply_work.obj_work.move_flag & 1) != 0)
                return;
            int spd_x = FX_Mul(ply_work.obj_work.spd_m, mtMathCos(ply_work.obj_work.dir.z));
            int spd_y = FX_Mul(ply_work.obj_work.spd_m, mtMathSin(ply_work.obj_work.dir.z));
            GmPlySeqInitPinballAir(ply_work, spd_x, spd_y);
        }
    }

    private static void gmPlySeqGmkMainPinballAir(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.user_timer > 0)
            --ply_work.obj_work.user_timer;
        if (ply_work.obj_work.user_timer <= 0 && ply_work.obj_work.user_flag != 0U)
            ply_work.player_flag &= 4294967263U;
        if (((int) ply_work.obj_work.move_flag & 1) == 0)
            return;
        ply_work.no_spddown_timer = 0;
        GmPlySeqLandingSet(ply_work, 0);
        GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void gmPlySeqGmkMainPinballCtpltHold(GMS_PLAYER_WORK ply_work)
    {
    }

    private static void gmPlySeqGmkMainPinballCtplt(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.move_flag & 1) != 0)
            return;
        int spd_x = FX_Mul(ply_work.obj_work.spd_m, mtMathCos(ply_work.obj_work.dir.z));
        int spd_y = FX_Mul(ply_work.obj_work.spd_m, mtMathSin(ply_work.obj_work.dir.z));
        GmPlySeqInitPinballAir(ply_work, spd_x, spd_y);
    }

    private static void gmPlySeqGmkMainFlipper(GMS_PLAYER_WORK ply_work)
    {
        if (MTM_MATH_ABS(ply_work.gmk_obj.pos.x - ply_work.obj_work.pos.x) <= 221184 &&
            MTM_MATH_ABS(ply_work.gmk_obj.pos.y - ply_work.obj_work.pos.y) <= 131072)
            return;
        GmPlySeqChangeSequence(ply_work, 0);
        ply_work.obj_work.move_flag |= 128U;
        ply_work.obj_work.move_flag &= 4294934271U;
    }

    private static void gmPlySeqGmkMainForceSpin(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m == 0)
        {
            ply_work.obj_work.spd_m = ((int) ply_work.obj_work.disp_flag & 1) == 0 ? 8192 : -8192;
            GmSoundPlaySE("Spin");
        }

        if (((int) ply_work.obj_work.move_flag & 1) != 0)
            return;
        GmPlySeqGmkInitForceSpinFall(ply_work);
    }

    private static void gmPlySeqGmkMainForceSpinDec(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m == 0)
        {
            ply_work.obj_work.spd_m = ((int) ply_work.obj_work.disp_flag & 1) == 0 ? 8192 : -8192;
            GmSoundPlaySE("Spin");
        }

        if (((int) ply_work.obj_work.disp_flag & 1) != 0)
        {
            if (ply_work.obj_work.spd_m < -8192)
                ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, -2048);
        }
        else if (ply_work.obj_work.spd_m > 8192)
            ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, 2048);

        if (((int) ply_work.obj_work.move_flag & 1) != 0)
            return;
        GmPlySeqGmkInitForceSpinFall(ply_work);
        ply_work.obj_work.spd_m = 0;
    }

    private static void gmPlySeqGmkMainForceSpinFall(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.move_flag & 1) == 0)
            return;
        if (((int) ply_work.obj_work.user_flag & 1) != 0)
            GmPlySeqGmkInitForceSpinDec(ply_work);
        else
            GmPlySeqGmkInitForceSpin(ply_work);
    }

    private static void gmPlySeqGmkMainMoveGear(GMS_PLAYER_WORK ply_work)
    {
        bool flag = true;
        OBS_OBJECT_WORK gmkObj = ply_work.gmk_obj;
        if (gmkObj == null)
            GmPlySeqChangeFw(ply_work);
        else if (((int) gmkObj.user_flag & 1) == 0 && GmPlayerKeyCheckJumpKeyPush(ply_work))
        {
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
            GmPlySeqChangeSequence(ply_work, 17);
        }
        else
        {
            ply_work.obj_work.move_flag |= 1U;
            if (((int) gmkObj.user_flag & 2) != 0)
            {
                ply_work.obj_work.spd_m = 0;
                ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
            }

            if (ply_work.act_state != 8 &&
                (((int) gmkObj.user_flag & 2) == 0 &&
                 (GmPlayerKeyCheckWalkLeft(ply_work) && ((int) ply_work.obj_work.disp_flag & 1) == 0 &&
                  ply_work.obj_work.spd_m <= 0 ||
                  GmPlayerKeyCheckWalkRight(ply_work) && ((int) ply_work.obj_work.disp_flag & 1) != 0 &&
                  ply_work.obj_work.spd_m >= 0) ||
                 ((int) gmkObj.user_flag & 1) != 0 &&
                 (((int) ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd_m <= 0 ||
                  ((int) ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd_m >= 0) ||
                 ((int) gmkObj.user_flag & 2) != 0 && gmkObj.user_work == 7U &&
                 (GmPlayerKeyCheckWalkLeft(ply_work) && ((int) ply_work.obj_work.disp_flag & 1) == 0 &&
                     gmkObj.user_timer <= 0 || GmPlayerKeyCheckWalkRight(ply_work) &&
                     ((int) ply_work.obj_work.disp_flag & 1) != 0 && gmkObj.user_timer >= 0)))
            {
                GmPlayerActionChange(ply_work, 8);
                GmPlySeqSetProgramTurnFwTurn(ply_work);
            }
            else if (ply_work.act_state == 8)
            {
                if (((int) ply_work.obj_work.disp_flag & 8) != 0)
                {
                    GmPlayerSetReverseOnlyState(ply_work);
                    GmPlayerActionChange(ply_work, 0);
                    ply_work.obj_work.disp_flag |= 4U;
                }
            }
            else if (((int) gmkObj.user_flag & 2) != 0 &&
                     (GmPlayerKeyCheckWalkLeft(ply_work) && ((int) ply_work.obj_work.disp_flag & 1) != 0 ||
                      GmPlayerKeyCheckWalkRight(ply_work) && ((int) ply_work.obj_work.disp_flag & 1) == 0) ||
                     gmkObj.user_timer != 0)
            {
                if (ply_work.ring_num == 0 && (gmkObj.user_work == 0U || gmkObj.user_work == 4U))
                {
                    if (ply_work.act_state != 33)
                    {
                        GmPlayerActionChange(ply_work, 33);
                        ply_work.obj_work.obj_3d.blend_spd = 1f / 16f;
                        ply_work.obj_work.disp_flag |= 4U;
                    }

                    ply_work.obj_work.obj_3d.speed[0] = 0.5f;
                    ply_work.obj_work.obj_3d.speed[1] = 0.5f;
                    flag = false;
                }
                else
                {
                    if ((((int) gmkObj.user_flag & 8) == 0 && GmPlayerKeyCheckWalkRight(ply_work) ||
                         ((int) gmkObj.user_flag & 8) != 0 && GmPlayerKeyCheckWalkLeft(ply_work)) &&
                        (gmkObj.user_work == 7U && gmkObj.user_timer == 0))
                    {
                        GmPlySeqChangeFw(ply_work);
                        return;
                    }

                    if (gmkObj.user_work == 1U || gmkObj.user_work == 2U)
                    {
                        if (ply_work.act_state != 60)
                        {
                            GmPlayerActionChange(ply_work, 60);
                            ply_work.obj_work.obj_3d.blend_spd = 1f / 16f;
                            ply_work.obj_work.disp_flag |= 4U;
                        }
                    }
                    else if (ply_work.act_state != 20)
                    {
                        GmPlayerActionChange(ply_work, 20);
                        ply_work.obj_work.disp_flag |= 4U;
                    }
                }

                if (ply_work.act_state != 33)
                {
                    flag = false;
                    int num = gmkObj.user_timer * 3;
                    int a = MTM_MATH_ABS((num >> 3) + (num >> 2));
                    if (a <= 1024)
                        a = 1024;
                    if (a >= 32768)
                        a = 32768;
                    ply_work.obj_work.obj_3d.speed[0] = FXM_FX32_TO_FLOAT(a);
                    ply_work.obj_work.obj_3d.speed[1] = FXM_FX32_TO_FLOAT(a);
                }
            }
            else if (ply_work.obj_work.spd_m != 0)
                GmPlayerWalkActionCheck(ply_work);
            else if (ply_work.act_state != 0)
            {
                GmPlayerActionChange(ply_work, 0);
                ply_work.obj_work.disp_flag |= 4U;
            }

            if (((int) gmkObj.user_flag & 3) == 0)
            {
                if (((int) gmkObj.user_flag & 4) != 0)
                    gmPlySeqGmkMoveGearMove(ply_work, true);
                else
                    gmPlySeqGmkMoveGearMove(ply_work, false);
            }

            if (flag)
                gmPlySeqGmkMoveGearAnimeSpeedSetWalk(ply_work, ply_work.obj_work.spd_m);
            GmPlySeqGmkMainGimmickDepend(ply_work);
        }
    }

    private static void gmPlySeqGmkMoveGearMove(GMS_PLAYER_WORK ply_work, bool spd_up_type)
    {
        int num1;
        int sSpd;
        if (!spd_up_type)
        {
            if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < 1024)
            {
                num1 = ply_work.spd_add >> 3;
                sSpd = ply_work.spd_dec;
            }
            else if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < 2048)
            {
                num1 = ply_work.spd_add >> 2;
                sSpd = (ply_work.spd_dec >> 1) + (ply_work.spd_dec >> 2);
            }
            else if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < 3072)
            {
                num1 = ply_work.spd_add >> 1;
                sSpd = ply_work.spd_dec >> 1;
            }
            else if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < 4096)
            {
                num1 = (ply_work.spd_add >> 1) + (ply_work.spd_add >> 2);
                sSpd = ply_work.spd_dec >> 2;
            }
            else
            {
                num1 = ply_work.spd_add;
                sSpd = ply_work.spd_dec >> 3;
            }
        }
        else if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < 3072)
        {
            num1 = ply_work.spd_add >> 1;
            sSpd = ply_work.spd_dec;
        }
        else if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < 4096)
        {
            num1 = (ply_work.spd_add >> 1) + (ply_work.spd_add >> 2);
            sSpd = (ply_work.spd_dec >> 1) + (ply_work.spd_dec >> 2);
        }
        else
        {
            num1 = ply_work.spd_add;
            sSpd = ply_work.spd_dec >> 1;
        }

        int num2 = (ply_work.spd_max >> 1) + (ply_work.spd_max >> 2);
        if (GmPlayerKeyCheckWalkRight(ply_work) || GmPlayerKeyCheckWalkLeft(ply_work))
        {
            int num3 = MTM_MATH_ABS(ply_work.key_walk_rot_z);
            if (num3 > 24576)
                num3 = 24576;
            num2 = num2 * num3 / 24576;
        }

        if (num2 < ply_work.prev_walk_roll_spd_max)
        {
            num2 = ply_work.prev_walk_roll_spd_max - sSpd;
            if (num2 < 0)
                num2 = 0;
        }

        ply_work.prev_walk_roll_spd_max = num2;
        if (ply_work.obj_work.dir.z != 0)
        {
            int num3 = FX_Mul(ply_work.spd_max_add_slope, mtMathSin(ply_work.obj_work.dir.z));
            if (num3 > 0)
                num2 += num3;
        }

        if (ply_work.no_spddown_timer != 0)
            sSpd = 0;
        else if (MTM_MATH_ABS(ply_work.obj_work.spd_m) <= ply_work.spd1)
            num1 = num1 * 5 / 8;
        else if (MTM_MATH_ABS(ply_work.obj_work.spd_m) <= ply_work.spd2)
            num1 >>= 1;
        else if (MTM_MATH_ABS(ply_work.obj_work.spd_m) > ply_work.spd3)
        {
            int num3;
            if (num2 - ply_work.spd3 != 0)
            {
                num3 = FX_Div(MTM_MATH_ABS(ply_work.obj_work.spd_m) - ply_work.spd3,
                    num2 - ply_work.spd3);
                if (num3 > 4096)
                    num3 = 4096;
            }
            else
                num3 = 4096;

            int v2 = num3 * 3968 >> 12;
            num1 -= FX_Mul(num1, v2);
        }

        if (ply_work.spd_work_max >= num2 && MTM_MATH_ABS(ply_work.obj_work.spd_m) >= num2)
        {
            if (ply_work.spd_work_max > ply_work.obj_work.spd_m)
                ply_work.spd_work_max = MTM_MATH_ABS(ply_work.obj_work.spd_m);
            num2 = ply_work.spd_work_max;
        }

        if ((ply_work.player_flag & 32768) != 0 && GmPlayerKeyCheckWalkRight(ply_work) &&
            num2 > ply_work.scroll_spd_x + 8192)
            num2 = ply_work.scroll_spd_x + 8192;
        if (GmPlayerKeyCheckWalkLeft(ply_work) | GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
                ply_work.obj_work.spd_m = ObjSpdUpSet(ply_work.obj_work.spd_m, num1, num2);
            }
            else
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
                ply_work.obj_work.spd_m = ObjSpdUpSet(ply_work.obj_work.spd_m, -num1, num2);
            }
        }
        else
        {
            ply_work.spd_pool = 0;
            ply_work.obj_work.spd.x = MTM_MATH_CLIP(ply_work.obj_work.spd.x, -num2, num2);
            ply_work.obj_work.spd_m = MTM_MATH_CLIP(ply_work.obj_work.spd_m, -num2, num2);
            if ((ply_work.obj_work.dir.z + 8192 & 65280) > 16384)
                return;
            if ((ply_work.player_flag & 134217728) != 0)
                ply_work.player_flag &= 4160749567U;
            else if ((ply_work.player_flag & 32768) != 0)
            {
                if (((int) ply_work.obj_work.disp_flag & 1) == 0 && ply_work.seq_state == 1)
                {
                    int num3 = ply_work.scroll_spd_x - 4096;
                    if (num3 < 0)
                        num3 = 0;
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
                    if (ply_work.obj_work.spd_m >= num3)
                        return;
                    ply_work.obj_work.spd_m = num3;
                }
                else
                    ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
            }
            else
                ply_work.obj_work.spd_m = ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
        }
    }

    private static void gmPlySeqGmkMoveGearAnimeSpeedSetWalk(
        GMS_PLAYER_WORK ply_work,
        int spd_set)
    {
        int a;
        if (19 <= ply_work.act_state && ply_work.act_state <= 21)
        {
            a = MTM_MATH_ABS((spd_set >> 3) + (spd_set >> 2));
            if (a <= 2048)
                a = 2048;
            if (a >= 32768)
                a = 32768;
        }
        else
            a = 4096;

        if (ply_work.obj_work.obj_3d == null)
            return;
        ply_work.obj_work.obj_3d.speed[0] = FXM_FX32_TO_FLOAT(a);
        ply_work.obj_work.obj_3d.speed[1] = FXM_FX32_TO_FLOAT(a);
    }

    private static void gmPlySeqGmkMainSteamPipe(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.user_timer >= 245760)
            return;
        ply_work.obj_work.user_timer = ObjTimeCountUp(ply_work.obj_work.user_timer);
        float num = (float) (FXM_FX32_TO_FLOAT(ply_work.obj_work.user_timer) / 60.0 * 2.0);
        ply_work.obj_work.obj_3d.speed[0] = 1f + num;
        ply_work.obj_work.obj_3d.speed[1] = 1f + num;
    }

    private static void gmPlySeqGmkMainDrainTank(GMS_PLAYER_WORK ply_work)
    {
        if (((int) ply_work.obj_work.move_flag & 1) != 0)
        {
            GmPlySeqLandingSet(ply_work, 0);
            GmPlySeqChangeSequence(ply_work, 0);
        }
        else if (ply_work.obj_work.spd_add.x > 0)
        {
            if (ply_work.obj_work.spd.x < 0)
                return;
            ply_work.obj_work.spd_add.x = 0;
            ply_work.obj_work.spd.x = 0;
        }
        else
        {
            if (ply_work.obj_work.spd_add.x >= 0 || ply_work.obj_work.spd.x > 0)
                return;
            ply_work.obj_work.spd_add.x = 0;
            ply_work.obj_work.spd.x = 0;
        }
    }

    private static void gmPlySeqGmkMainBoss5Quake(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.user_timer > 0)
        {
            --ply_work.obj_work.user_timer;
        }
        else
        {
            GmPlySeqLandingSet(ply_work, 0);
            ply_work.obj_work.move_flag &= 4294959103U;
            GmPlySeqChangeSequence(ply_work, 0);
        }
    }

    private static void gmPlySeqGmkMainEndingFrontSide(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 77 || ((int) ply_work.obj_work.disp_flag & 8) == 0)
            return;
        GmPlayerActionChange(ply_work, 78);
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void gmPlySeqGmkMainEndingFinish(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 80 && ply_work.act_state != 82 && ply_work.act_state != 84)
        {
            int num = (int) ply_work.obj_work.user_work + 4;
            ply_work.obj_work.user_work = (uint) num;
            ply_work.obj_work.scale.x += (int) ply_work.obj_work.user_work;
            ply_work.obj_work.scale.y += (int) ply_work.obj_work.user_work;
            ply_work.obj_work.scale.z += (int) ply_work.obj_work.user_work;
            ply_work.obj_work.pos.z += 1024;
        }

        if (ply_work.act_state == 39 && ply_work.obj_work.spd.y > -4096)
        {
            if ((ply_work.player_flag & 16384) != 0)
                GmPlayerActionChange(ply_work, 83);
            else if (ply_work.obj_work.user_flag != 0U)
                GmPlayerActionChange(ply_work, 81);
            else
                GmPlayerActionChange(ply_work, 79);
        }
        else
        {
            if (ply_work.act_state != 79 && ply_work.act_state != 81 && ply_work.act_state != 83 ||
                ((int) ply_work.obj_work.disp_flag & 8) == 0)
                return;
            if ((ply_work.player_flag & 16384) != 0)
                GmPlayerActionChange(ply_work, 84);
            else if (ply_work.obj_work.user_flag != 0U)
                GmPlayerActionChange(ply_work, 82);
            else
                GmPlayerActionChange(ply_work, 80);
            ply_work.obj_work.move_flag |= 8192U;
            ply_work.obj_work.disp_flag |= 32U;
            GmEndingTrophySet();
        }
    }

    private static uint gmPlayerCheckTruckAirFoot(GMS_PLAYER_WORK ply_work)
    {
        OBS_COL_CHK_DATA pData = GlobalPool<OBS_COL_CHK_DATA>.Alloc();
        uint num1 = 0;
        int num2 = 0;
        int num3 = 0;
        if (ply_work.obj_work.ride_obj != null)
            return num1;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK) ply_work;
        ushort num4 = (ushort) (obsObjectWork.dir.z + (uint) obsObjectWork.dir_fall);
        pData.flag = (ushort) (obsObjectWork.flag & 1U);
        pData.vec = 2;
        pData.dir = null;
        pData.attr = null;
        switch ((ushort) ((ushort) ((ushort) ((obsObjectWork.dir.z + 8192 & 49152) >> 14) +
                                    (uint) (ushort) ((obsObjectWork.dir_fall + 8192 & 49152) >> 14)) & 3U))
        {
            case 0:
                pData.vec = 2;
                break;
            case 1:
                pData.vec = 1;
                break;
            case 2:
                pData.vec = 3;
                break;
            case 3:
                pData.vec = 0;
                break;
        }

        if ((num4 & 16383) != 0)
        {
            NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
            NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
            NNS_VECTOR nnsVector3 = GlobalPool<NNS_VECTOR>.Alloc();
            NNS_VECTOR nnsVector4 = GlobalPool<NNS_VECTOR>.Alloc();
            NNS_VECTOR nnsVector5 = GlobalPool<NNS_VECTOR>.Alloc();
            NNS_VECTOR vec2 = GlobalPool<NNS_VECTOR>.Alloc();
            switch ((num4 & 49152) >> 14)
            {
                case 0:
                    num2 = obsObjectWork.field_rect[0] - 2;
                    num3 = obsObjectWork.field_rect[3] + 2;
                    break;
                case 1:
                    num2 = obsObjectWork.field_rect[0] - 2;
                    num3 = obsObjectWork.field_rect[1] - 2;
                    break;
                case 2:
                    num2 = obsObjectWork.field_rect[2] + 2;
                    num3 = obsObjectWork.field_rect[1] - 2;
                    break;
                case 3:
                    num2 = obsObjectWork.field_rect[2] + 2;
                    num3 = obsObjectWork.field_rect[3] + 2;
                    break;
            }

            nnsVector1.x = num2;
            nnsVector1.y = -num3;
            nnsVector1.z = 0.0f;
            nnsVector2.x = num2 + 10f * nnCos(-num4);
            nnsVector2.y = -num3 + 10f * nnSin(-num4);
            nnsVector2.z = 0.0f;
            nnsVector3.x = nnsVector3.y = nnsVector3.z = 0.0f;
            nnsVector5.x = nnsVector2.x - nnsVector1.x;
            nnsVector5.y = nnsVector2.y - nnsVector1.y;
            nnsVector5.z = nnsVector2.z - nnsVector1.z;
            vec2.x = nnsVector3.x - nnsVector1.x;
            vec2.y = nnsVector3.y - nnsVector1.y;
            vec2.z = nnsVector3.z - nnsVector1.z;
            float num5 = nnDotProductVector(nnsVector5, vec2) /
                         nnDotProductVector(nnsVector5, nnsVector5);
            nnsVector4.x = nnsVector1.x + nnsVector5.x * num5;
            nnsVector4.y = nnsVector1.y + nnsVector5.y * num5;
            nnsVector4.z = nnsVector1.z + nnsVector5.z * num5;
            num2 = FXM_FLOAT_TO_FX32(nnsVector4.x);
            num3 = FXM_FLOAT_TO_FX32(-nnsVector4.y);
            GlobalPool<NNS_VECTOR>.Release(nnsVector1);
            GlobalPool<NNS_VECTOR>.Release(nnsVector2);
            GlobalPool<NNS_VECTOR>.Release(nnsVector3);
            GlobalPool<NNS_VECTOR>.Release(nnsVector4);
            GlobalPool<NNS_VECTOR>.Release(nnsVector5);
            GlobalPool<NNS_VECTOR>.Release(vec2);
        }
        else
        {
            switch ((num4 & 49152) >> 14)
            {
                case 0:
                    num2 = 0;
                    num3 = obsObjectWork.field_rect[3] << 12;
                    break;
                case 1:
                    num2 = -obsObjectWork.field_rect[3] << 12;
                    num3 = 0;
                    break;
                case 2:
                    num2 = 0;
                    num3 = -obsObjectWork.field_rect[3] << 12;
                    break;
                case 3:
                    num2 = obsObjectWork.field_rect[3] << 12;
                    num3 = 0;
                    break;
            }
        }

        int fx32_1 = FXM_FLOAT_TO_FX32(obsObjectWork.field_rect[2] * nnCos(num4));
        int fx32_2 = FXM_FLOAT_TO_FX32(obsObjectWork.field_rect[2] * nnSin(num4));
        pData.pos_x = num2 + fx32_1 + obsObjectWork.pos.x >> 12;
        pData.pos_y = num3 + fx32_2 + obsObjectWork.pos.y >> 12;
        if (ObjDiffCollision(pData) <= 2)
            num1 |= 1U;
        int fx32_3 = FXM_FLOAT_TO_FX32(obsObjectWork.field_rect[0] * nnCos(num4));
        int fx32_4 = FXM_FLOAT_TO_FX32(obsObjectWork.field_rect[0] * nnSin(num4));
        pData.pos_x = num2 + fx32_3 + obsObjectWork.pos.x >> 12;
        pData.pos_y = num3 + fx32_4 + obsObjectWork.pos.y >> 12;
        if (ObjDiffCollision(pData) <= 2)
            num1 |= 2U;
        return num1;
    }

    private static void gmPlySeqGmkMainTruckDanger(GMS_PLAYER_WORK ply_work)
    {
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        if ((ply_work.player_flag & 2) == 0)
        {
            if (MTM_MATH_ABS(ply_work.gmk_work3) < 6144)
            {
                ply_work.gmk_work3 += ply_work.obj_work.user_timer;
                if ((ply_work.player_flag & 4) == 0)
                {
                    if (ply_work.gmk_work3 > 6144)
                        ply_work.gmk_work3 = 6144;
                    ply_work.obj_work.user_timer = ObjSpdUpSet(ply_work.obj_work.user_timer, 32, 1024);
                }
                else
                {
                    if (ply_work.gmk_work3 < -6144)
                        ply_work.gmk_work3 = -6144;
                    ply_work.obj_work.user_timer = ObjSpdUpSet(ply_work.obj_work.user_timer, -32, 1024);
                }
            }
            else
            {
                if (ply_work.act_state != 74 && ply_work.act_state != 75)
                    GmPlayerActionChange(ply_work, 74);
                if ((int) ply_work.obj_work.user_work < 3)
                {
                    int num = (int) ply_work.obj_work.user_work + 1;
                    ply_work.obj_work.user_work = (uint) num;
                    ply_work.obj_work.user_timer = -ply_work.obj_work.user_timer >> 1;
                    if (ply_work.gmk_work3 < 0)
                        ++ply_work.gmk_work3;
                    else
                        --ply_work.gmk_work3;
                }
                else
                    ply_work.player_flag |= 2U;
            }
        }

        if (ply_work.act_state == 74)
        {
            ply_work.gmk_work2 = ObjTimeCountDown(ply_work.gmk_work2);
            if (ply_work.gmk_work2 != 0)
                ply_work.gmk_work1 = (ushort) (ply_work.gmk_work1 + ply_work.gmk_work0);
            if (((int) ply_work.obj_work.disp_flag & 8) != 0)
            {
                GmPlayerActionChange(ply_work, 75);
                ply_work.obj_work.disp_flag |= 4U;
                ply_work.gmk_flag |= 1073741824U;
                ply_work.obj_work.vib_timer = ply_work.fall_timer;
            }
        }
        else if (((int) ply_work.gmk_flag & 1073741824) != 0)
        {
            ply_work.gmk_work1 = (ushort) ((uint) (32768 - ply_work.obj_work.dir.z - ply_work.gmk_work3) +
                                           (uint) (short) (g_gm_main_system.pseudofall_dir -
                                                           ply_work.obj_work.dir_fall));
            if ((ply_work.player_flag & 1) != 0)
                GmPlySeqGmkInitTruckDangerRet(ply_work, ply_work.truck_obj);
        }

        if (((int) ply_work.gmk_flag & 1073741824) == 0 && ply_work.act_state == 75 &&
            (ply_work.player_flag & 2) != 0)
            ply_work.gmk_flag |= 1073741824U;
        nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
        nnTranslateMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, 0.0f, 5f, 9f);
        nnRotateXMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, ply_work.gmk_work1);
        nnTranslateMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, -0.0f, -5f, -9f);
        float x;
        float y;
        float z;
        if ((ply_work.player_flag & 4) == 0)
        {
            x = 0.0f;
            y = 8f;
            z = -5f;
        }
        else
        {
            x = 0.0f;
            y = 8f;
            z = 5f;
        }

        nnMakeUnitMatrix(nnsMatrix);
        nnTranslateMatrix(nnsMatrix, nnsMatrix, -x, -y, -z);
        nnRotateXMatrix(nnsMatrix, nnsMatrix, ply_work.gmk_work3);
        nnRotateYMatrix(nnsMatrix, nnsMatrix, MTM_MATH_ABS(ply_work.gmk_work3) >> 2);
        nnRotateZMatrix(nnsMatrix, nnsMatrix, MTM_MATH_ABS(ply_work.gmk_work3) >> 2);
        nnTranslateMatrix(nnsMatrix, nnsMatrix, x, y, z);
        nnMultiplyMatrix(ply_work.ex_obj_mtx_r, nnsMatrix, ply_work.ex_obj_mtx_r);
    }

    private static void gmPlySeqGmkMainTruckDangerRet(GMS_PLAYER_WORK ply_work)
    {
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        ply_work.gmk_work2 = ObjTimeCountUp(ply_work.gmk_work2);
        if (73728 <= ply_work.gmk_work2 && ply_work.gmk_work2 <= 131072)
        {
            ply_work.gmk_work1 += ply_work.gmk_work0;
            if (ply_work.gmk_work0 < 0)
            {
                if (ply_work.gmk_work1 < 0)
                    ply_work.gmk_work1 = 0;
            }
            else if (ply_work.gmk_work1 > 0)
                ply_work.gmk_work1 = 0;
        }

        if ((ply_work.player_flag & 2) != 0)
        {
            ply_work.gmk_work3 = ObjSpdDownSet(ply_work.gmk_work3, 1024);
            if (MTM_MATH_ABS(ply_work.gmk_work3) == 0)
            {
                ply_work.gmk_work3 = 0;
                ply_work.gmk_flag |= 2147483648U;
                GmPlySeqChangeFw(ply_work);
                return;
            }
        }
        else if (((int) ply_work.obj_work.disp_flag & 8) != 0)
        {
            ply_work.gmk_work1 = 0;
            if (ply_work.gmk_work3 != 0)
            {
                ply_work.player_flag |= 2U;
                ply_work.gmk_flag &= 4293918719U;
                GmPlayerActionChange(ply_work, 69);
                ply_work.obj_work.disp_flag |= 4U;
            }
            else
            {
                ply_work.gmk_flag |= 2147483648U;
                GmPlySeqChangeFw(ply_work);
                return;
            }
        }

        nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
        nnTranslateMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, 0.0f, 5f, 9f);
        nnRotateXMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, (ushort) ply_work.gmk_work1);
        nnTranslateMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, -0.0f, -5f, -9f);
        float x;
        float y;
        float z;
        if ((ply_work.player_flag & 4) == 0)
        {
            x = 0.0f;
            y = 8f;
            z = -5f;
        }
        else
        {
            x = 0.0f;
            y = 8f;
            z = 5f;
        }

        nnMakeUnitMatrix(nnsMatrix);
        nnTranslateMatrix(nnsMatrix, nnsMatrix, -x, -y, -z);
        nnRotateXMatrix(nnsMatrix, nnsMatrix, ply_work.gmk_work3);
        nnRotateZMatrix(nnsMatrix, nnsMatrix, MTM_MATH_ABS(ply_work.gmk_work3) >> 2);
        nnTranslateMatrix(nnsMatrix, nnsMatrix, x, y, z);
        nnMultiplyMatrix(ply_work.ex_obj_mtx_r, nnsMatrix, ply_work.ex_obj_mtx_r);
    }
}