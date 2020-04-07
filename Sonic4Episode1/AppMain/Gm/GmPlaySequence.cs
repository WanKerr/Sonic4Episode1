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
    private static void GmPlySeqSetSeqState(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.seq_init_tbl = AppMain.g_gm_ply_seq_init_tbl_list[(int)ply_work.char_id];
        ply_work.seq_state_data_tbl = AppMain.g_gm_ply_seq_state_data_tbl[(int)ply_work.char_id];
    }

    private static void GmPlySeqMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        AppMain.GMS_PLY_SEQ_STATE_DATA[] seqStateDataTbl = ply_work.seq_state_data_tbl;
        if (ply_work.no_spddown_timer != 0)
            ply_work.no_spddown_timer = AppMain.ObjTimeCountDown(ply_work.no_spddown_timer);
        if (ply_work.maxdash_timer != 0)
            ply_work.maxdash_timer = AppMain.ObjTimeCountDown(ply_work.maxdash_timer);
        if (((int)ply_work.player_flag & 1048576) != 0)
            AppMain.gmPlySeqActGoal(ply_work);
        if (((int)ply_work.player_flag & 2097152) != 0)
            AppMain.gmPlySeqBossGoalPre(ply_work);
        if (((int)ply_work.player_flag & 1073741824) != 0)
            AppMain.gmPlySeqBoss5DemoPre(ply_work);
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            AppMain.gmPlySeqSplStgRollCtrl(ply_work);
        AppMain.gmPlySeqCheckChangeSequence(ply_work);
        if (ply_work.seq_func != null)
            ply_work.seq_func(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 8388608) != 0)
            AppMain.GmPlayerAnimeSpeedSetWalk(ply_work, ply_work.obj_work.spd_m);
        else if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 4194304) == 0)
        {
            ply_work.obj_work.obj_3d.speed[0] = 1f;
            ply_work.obj_work.obj_3d.speed[1] = 1f;
        }

        if (((int)ply_work.player_flag & 16) == 0)
            return;
        int pgmTurnDir = (int)ply_work.pgm_turn_dir;
        int num;
        if (ply_work.pgm_turn_dir_tbl != null)
        {
            num = (int)ply_work.pgm_turn_dir_tbl[ply_work.pgm_turn_tbl_cnt];
            ++ply_work.pgm_turn_tbl_cnt;
            if (ply_work.pgm_turn_tbl_cnt >= ply_work.pgm_turn_tbl_num)
            {
                ply_work.pgm_turn_tbl_cnt = ply_work.pgm_turn_tbl_num - 1;
                ply_work.player_flag &= 4294967279U;
                if (((int)ply_work.player_flag & 256) == 0)
                    num = 0;
            }
        }
        else if (((int)ply_work.obj_work.disp_flag & 1) != 0)
        {
            num = pgmTurnDir - (int)ply_work.pgm_turn_spd;
            if (num <= 0)
            {
                num = 0;
                ply_work.player_flag &= 4294967279U;
            }
        }
        else
        {
            num = pgmTurnDir + (int)ply_work.pgm_turn_spd;
            if (num >= 65536)
            {
                num = 0;
                ply_work.player_flag &= 4294967279U;
            }
        }

        ply_work.pgm_turn_dir = (ushort)num;
        if (((int)ply_work.player_flag & int.MinValue) == 0 || ((int)ply_work.player_flag & 16) != 0)
            return;
        AppMain.GmPlayerActionChange(ply_work, ply_work.fall_act_state);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.player_flag &= (uint)int.MaxValue;
    }

    private static bool GmPlySeqChangeSequence(AppMain.GMS_PLAYER_WORK ply_work, int seq_state)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, seq_state);
        if (ply_work.seq_init_tbl[seq_state] == null)
            return false;
        ply_work.seq_init_tbl[seq_state](ply_work);
        return true;
    }

    private static void GmPlySeqChangeSequenceState(AppMain.GMS_PLAYER_WORK ply_work, int seq_state)
    {
        if (ply_work.gmk_obj != null)
            AppMain.GmPlayerStateGimmickInit(ply_work);
        ply_work.prev_seq_state = ply_work.seq_state;
        ply_work.seq_state = seq_state;
        ply_work.rect_work[1].flag &= 4294967291U;
        if (((int)ply_work.player_flag & 256) != 0)
            AppMain.GmPlayerSetReverseOnlyState(ply_work);
        if (((int)ply_work.player_flag & int.MinValue) == 0)
            return;
        ply_work.player_flag &= 2147483375U;
        ply_work.pgm_turn_dir_tbl = (ushort[])null;
        ply_work.pgm_turn_dir = (ushort)0;
        ply_work.pgm_turn_spd = (ushort)0;
    }

    private static void GmPlySeqSetProgramTurn(AppMain.GMS_PLAYER_WORK ply_work, ushort turn_spd)
    {
        if (((int)ply_work.player_flag & 16) == 0)
            ply_work.pgm_turn_dir = (ushort)0;
        AppMain.GmPlayerSetReverse(ply_work);
        ply_work.player_flag |= 16U;
        ply_work.pgm_turn_spd = turn_spd;
        ply_work.pgm_turn_dir += (ushort)32768;
        ply_work.pgm_turn_dir_tbl = (ushort[])null;
    }

    private static void GmPlySeqSetProgramTurnTbl(
        AppMain.GMS_PLAYER_WORK ply_work,
        ushort[] turn_tbl,
        int tbl_num,
        bool rev_depend_mtn)
    {
        if (((int)ply_work.player_flag & 16) == 0)
            ply_work.pgm_turn_dir = (ushort)0;
        if (!rev_depend_mtn)
            AppMain.GmPlayerSetReverse(ply_work);
        else
            ply_work.player_flag |= 256U;
        ply_work.player_flag |= 16U;
        ply_work.pgm_turn_dir_tbl = turn_tbl;
        ply_work.pgm_turn_tbl_num = tbl_num;
        ply_work.pgm_turn_tbl_cnt = 0;
    }

    private static void GmPlySeqSetProgramTurnFwTurn(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.disp_flag & 1) != 0)
            AppMain.GmPlySeqSetProgramTurnTbl(ply_work, AppMain.gm_ply_seq_turn_l_dir_tbl, 10, true);
        else
            AppMain.GmPlySeqSetProgramTurnTbl(ply_work, AppMain.gm_ply_seq_turn_dir_tbl, 10, true);
    }

    private static void GmPlySeqSetFallTurn(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int num = 0;
        if (((int)ply_work.player_flag & int.MinValue) != 0)
            num = ply_work.pgm_turn_tbl_cnt;
        else
            ply_work.fall_act_state = ply_work.act_state;
        if (((int)ply_work.obj_work.disp_flag & 1) != 0)
            AppMain.GmPlySeqSetProgramTurnTbl(ply_work, AppMain.gm_ply_seq_fall_turn_l_dir_tbl, 10, false);
        else
            AppMain.GmPlySeqSetProgramTurnTbl(ply_work, AppMain.gm_ply_seq_fall_turn_dir_tbl, 10, false);
        ply_work.player_flag |= 2147483648U;
        if (ply_work.act_state == 42 || ply_work.act_state == 43)
            AppMain.GmPlayerActionChange(ply_work, 43);
        else
            AppMain.GmPlayerActionChange(ply_work, 41);
        if (num == 0)
            return;
        ply_work.pgm_turn_tbl_cnt = 10 - num;
        ply_work.obj_work.obj_3d.frame[0] = (float)ply_work.pgm_turn_tbl_cnt;
    }

    private static void GmPlySeqChangeFw(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqInitFw(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (!AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            if (ply_work.obj_work.spd_m != 0)
            {
                if (ply_work.prev_seq_state == 2)
                    AppMain.GmPlayerActionChange(ply_work, 0);
                AppMain.GmPlySeqChangeSequence(ply_work, 1);
                return;
            }

            if (((int)ply_work.player_flag & 131072) == 0)
                AppMain.GmPlayerActionChange(ply_work, 0);
            else
                AppMain.GmPlyEfctCreateSpinJumpBlur(ply_work);
        }
        else
            AppMain.GmPlyEfctCreateSpinJumpBlur(ply_work);

        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqFwMain);
    }

    private static void gmPlySeqFwMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state == 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 8) == 0)
                return;
            int num = (int)ply_work.obj_work.user_work + 1;
            ply_work.obj_work.user_work = (uint)num;
            if ((int)ply_work.obj_work.user_work < 8)
                return;
            if (((int)ply_work.player_flag & 16384) != 0)
                AppMain.GmPlayerActionChange(ply_work, 4);
            else
                AppMain.GmPlayerActionChange(ply_work, 2);
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                AppMain.GmPlySeqSetProgramTurn(ply_work, (ushort)4096);
            ply_work.obj_work.user_work = 0U;
        }
        else if (ply_work.act_state == 2 || ply_work.act_state == 4 || ply_work.act_state == 6)
        {
            if (((int)ply_work.obj_work.disp_flag & 8) == 0)
                return;
            AppMain.GmPlayerActionChange(ply_work, ply_work.act_state + 1);
            ply_work.obj_work.disp_flag |= 4U;
            ply_work.obj_work.user_work = 0U;
        }
        else if (ply_work.act_state == 3)
        {
            if (((int)ply_work.obj_work.disp_flag & 8) == 0)
                return;
            int num = (int)ply_work.obj_work.user_work + 1;
            ply_work.obj_work.user_work = (uint)num;
            if ((int)ply_work.obj_work.user_work < 10)
                return;
            AppMain.GmPlayerActionChange(ply_work, 4);
            ply_work.obj_work.user_work = 0U;
        }
        else
        {
            if (ply_work.act_state != 5 || ((int)ply_work.obj_work.disp_flag & 8) == 0)
                return;
            int num = (int)ply_work.obj_work.user_work + 1;
            ply_work.obj_work.user_work = (uint)num;
            if ((int)ply_work.obj_work.user_work < 3 || ((int)ply_work.player_flag & 16384) != 0)
                return;
            AppMain.GmPlayerActionChange(ply_work, 6);
            ply_work.obj_work.user_work = 0U;
        }
    }

    private static void GmPlySeqInitWalk(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 131072) == 0)
            AppMain.GmPlayerWalkActionSet(ply_work);
        else
            AppMain.GmPlyEfctCreateSpinJumpBlur(ply_work);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqWalkMain);
        ply_work.obj_work.user_timer = 0;
    }

    private static void gmPlySeqWalkMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m > 0 && AppMain.GmPlayerKeyCheckWalkRight(ply_work) &&
            ((int)ply_work.obj_work.disp_flag & 1) != 0 || ply_work.obj_work.spd_m < 0 &&
            AppMain.GmPlayerKeyCheckWalkLeft(ply_work) && ((int)ply_work.obj_work.disp_flag & 1) == 0)
            AppMain.GmPlySeqSetProgramTurn(ply_work, (ushort)4096);
        AppMain.GmPlayerWalkActionCheck(ply_work);
        if ((ply_work.obj_work.user_timer & 63) == 1 && ply_work.obj_work.ride_obj == null)
            AppMain.GmPlyEfctCreateFootSmoke(ply_work);
        ++ply_work.obj_work.user_timer;
    }

    private static void GmPlySeqInitTurn(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.prev_seq_state == 2)
        {
            ply_work.player_flag &= 2147483375U;
            AppMain.GmPlayerSetReverse(ply_work);
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
        }
        else
        {
            if (23 <= ply_work.act_state && ply_work.act_state <= 25)
                AppMain.GmPlayerActionChange(ply_work, 10);
            else if (20 <= ply_work.act_state && ply_work.act_state <= 22)
            {
                AppMain.GmPlayerActionChange(ply_work, 9);
            }
            else
            {
                AppMain.GmPlayerActionChange(ply_work, 8);
                AppMain.GmPlySeqSetProgramTurnFwTurn(ply_work);
            }

            ply_work.obj_work.move_flag &= 4294967279U;
            ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTurnMain);
        }
    }

    private static void gmPlySeqTurnMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.disp_flag & 8) == 0)
            return;
        AppMain.GmPlayerSetReverseOnlyState(ply_work);
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqInitLookupStart(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 11);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqLookupMain);
    }

    private static void GmPlySeqInitLookupMiddle(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 12);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqLookupMain);
    }

    private static void GmPlySeqInitLookupEnd(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 13);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqLookupEndMain);
    }

    private static void gmPlySeqLookupMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m != 0)
        {
            AppMain.GmPlySeqChangeSequence(ply_work, 1);
        }
        else
        {
            if (ply_work.act_state != 11 || ((int)ply_work.obj_work.disp_flag & 8) == 0)
                return;
            AppMain.GmPlySeqChangeSequence(ply_work, 4);
        }
    }

    private static void gmPlySeqLookupEndMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.disp_flag & 8) == 0)
            return;
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqInitSquatStart(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 14);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqSquatMain);
    }

    private static void GmPlySeqInitSquatMiddle(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 15);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 4096)
            ply_work.obj_work.spd_m = 0;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqSquatMain);
    }

    private static void GmPlySeqInitSquatEnd(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 16);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqSquatEndMain);
    }

    private static void gmPlySeqSquatMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state == 14 && ((int)ply_work.obj_work.disp_flag & 8) != 0)
            AppMain.GmPlySeqChangeSequence(ply_work, 7);
        if (ply_work.seq_state != 7 || ply_work.obj_work.spd_m == 0)
            return;
        ply_work.obj_work.move_flag |= 16384U;
        AppMain.GmPlySeqChangeSequence(ply_work, 10);
    }

    private static void gmPlySeqSquatEndMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.disp_flag & 8) == 0)
            return;
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqInitBrake(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 23);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqBrakeMain);
        AppMain.GmSoundPlaySE("Brake");
        AppMain.GmPlyEfctCreateBrakeImpact(ply_work);
        AppMain.GmPlyEfctCreateBrakeDust(ply_work);
    }

    private static void gmPlySeqBrakeMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 25 &&
            (((int)ply_work.obj_work.disp_flag & 1) != 0 && !AppMain.GmPlayerKeyCheckWalkRight(ply_work) ||
             ((int)ply_work.obj_work.disp_flag & 1) == 0 && !AppMain.GmPlayerKeyCheckWalkLeft(ply_work)))
            AppMain.GmPlayerActionChange(ply_work, 25);
        switch (ply_work.act_state)
        {
            case 23:
                if (((int)ply_work.obj_work.disp_flag & 8) == 0)
                    break;
                AppMain.GmPlayerActionChange(ply_work, 24);
                ply_work.obj_work.disp_flag |= 4U;
                break;
            case 24:
                if (ply_work.obj_work.spd_m > 0 && AppMain.GmPlayerKeyCheckWalkLeft(ply_work) ||
                    ply_work.obj_work.spd_m < 0 && AppMain.GmPlayerKeyCheckWalkRight(ply_work))
                    break;
                AppMain.GmPlySeqChangeSequence(ply_work, 2);
                break;
            case 25:
                if (((int)ply_work.obj_work.disp_flag & 8) == 0)
                    break;
                if (ply_work.obj_work.spd_m != 0)
                {
                    AppMain.GmPlySeqChangeSequence(ply_work, 1);
                    break;
                }

                AppMain.GmPlySeqChangeSequence(ply_work, 0);
                break;
        }
    }

    private static void GmPlySeqInitSpin(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 27);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqSpinMain);
        AppMain.GmPlayerSetAtk(ply_work);
        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        if (ply_work.prev_seq_state != 37 && ((int)ply_work.player_flag & 131072) == 0)
        {
            if (betterSfx)
            {
                AppMain.GmSoundPlaySE(ply_work.prev_seq_state == 11 ? "Dash1" : "Dash2");
            }
            else
            {
                AppMain.GmSoundPlaySE("Spin");
            }
        }

        AppMain.GmPlyEfctCreateSpinDashDust(ply_work);
        AppMain.GmPlyEfctCreateSuperAuraSpin(ply_work);
        AppMain.GmPlyEfctCreateSpinDashBlur(ply_work, 1U);
        AppMain.GmPlyEfctCreateSpinDashCircleBlur(ply_work);
        AppMain.GmPlyEfctCreateTrail(ply_work, 1);
    }

    private static void gmPlySeqSpinMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlySeqInitSpinDashAcc(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 29 && ply_work.act_state != 30 && ply_work.act_state != 28)
            AppMain.GmPlyEfctCreateSpinStartBlur(ply_work);
        if (ply_work.efct_spin_start_blur != null)
            AppMain.GmPlayerActionChange(ply_work, 28);
        else
            AppMain.GmPlayerActionChange(ply_work, 29);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.dash_power = ply_work.dash_power == 0
            ? ply_work.spd_spin
            : AppMain.ObjSpdUpSet(ply_work.dash_power, ply_work.spd_add_spin, ply_work.spd_max_spin);
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqSpinDashMain);
        AppMain.GmPlayerSetAtk(ply_work);

        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        if (betterSfx)
        {
            ply_work.spinHandle.snd_ctrl_param.pitch = Math.Min(((float)(ply_work.dash_power - ply_work.spd_spin) / ply_work.spd_max_spin), 1f);
            AppMain.GmSoundPlaySEForce("Spin", ply_work.spinHandle);
        }
        else
        {
            if (ply_work.spin_se_timer <= (short)0)
            {
                AppMain.GmSoundPlaySE("Dash1");
                ply_work.spin_se_timer = (short)25;
            }

            if (ply_work.spin_back_se_timer <= (short)0)
            {
                AppMain.GmSoundPlaySE("Dash2");
                ply_work.spin_se_timer = (short)50;
            }
        }

        if (ply_work.prev_seq_state == 11)
            return;
        AppMain.GmPlyEfctCreateSpinAddDust(ply_work);
    }

    private static void GmPlySeqInitSpinDash(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 29 && ply_work.act_state != 30 && ply_work.act_state != 28)
            AppMain.GmPlyEfctCreateSpinStartBlur(ply_work);
        AppMain.GmPlayerActionChange(ply_work, 30);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqSpinDashMain);
        AppMain.GmPlayerSetAtk(ply_work);
        AppMain.GmPlyEfctCreateSpinDust(ply_work);
    }

    private static void gmPlySeqSpinDashMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state == 28 && ply_work.efct_spin_start_blur == null)
        {
            if (ply_work.seq_state == 11)
            {
                AppMain.GmPlySeqChangeSequence(ply_work, 12);
                return;
            }

            AppMain.GmPlayerActionChange(ply_work, 30);
        }

        if (ply_work.act_state == 29 && ((int)ply_work.obj_work.disp_flag & 8) != 0)
            AppMain.GmPlySeqChangeSequence(ply_work, 12);
        else if (((int)ply_work.key_on & 2) == 0 || ply_work.obj_work.spd_m != 0)
        {
            ply_work.no_spddown_timer = 72;
            ply_work.camera_stop_timer = 32768;
            int a = 48128 + AppMain.FX_Mul(ply_work.dash_power, 512);
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                a = -a;
            if (AppMain.MTM_MATH_ABS(a) > AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m))
                ply_work.obj_work.spd_m = a;
            ply_work.dash_power = 0;
            ply_work.obj_work.move_flag |= 16384U;
            AppMain.GmPlySeqChangeSequence(ply_work, 10);
            AppMain.GmPlyEfctCreateSpinDashImpact(ply_work);
            AppMain.GMM_PAD_VIB_SMALL();
        }
        else
        {
            if (((int)ply_work.key_on & 1) == 0)
                return;
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
        }
    }

    private static void GmPlySeqInitStaggerFront(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 33);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqStaggerMain);
        AppMain.GmPlyEfctCreateSweat(ply_work);
    }

    private static void GmPlySeqInitStaggerBack(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 34);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqStaggerMain);
        AppMain.GmPlyEfctCreateSweat(ply_work);
    }

    private static void GmPlySeqInitStaggerDanger(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 35);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqStaggerMain);
        AppMain.GmPlyEfctCreateSweat(ply_work);
    }

    private static void gmPlySeqStaggerMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlySeqInitFall(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (!AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY() &&
            (((int)ply_work.player_flag & 16384) == 0 || ply_work.act_state != 21 && ply_work.act_state != 22) &&
            (((int)ply_work.player_flag & 131072) == 0 && ply_work.prev_seq_state != 40))
        {
            if ((ushort)((uint)ply_work.obj_work.dir.z - 8192U) <= (ushort)49152)
                AppMain.GmPlayerActionChange(ply_work, 42);
            else
                AppMain.GmPlayerActionChange(ply_work, 40);
        }

        AppMain.GmPlySeqInitFallState(ply_work);
    }

    private static void GmPlySeqInitFallState(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 32912U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqJumpMain);
        ply_work.obj_work.spd.x = AppMain.FX_Mul(ply_work.obj_work.spd_m,
            AppMain.mtMathCos((int)ply_work.obj_work.dir.z -
                              ((int)AppMain.g_gm_main_system.pseudofall_dir - (int)ply_work.prev_dir_fall2)));
        ply_work.obj_work.spd.y = AppMain.FX_Mul(ply_work.obj_work.spd_m,
            AppMain.mtMathSin((int)ply_work.obj_work.dir.z -
                              ((int)AppMain.g_gm_main_system.pseudofall_dir - (int)ply_work.prev_dir_fall2)));
        ply_work.obj_work.spd_m = 0;
        ply_work.player_flag &= 4294967280U;
        ply_work.player_flag |= 1U;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
    }

    private static void GmPlySeqInitJump(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 131072) == 0)
            AppMain.GmPlayerActionChange(ply_work, 39);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4290772990U;
        ushort z = ply_work.obj_work.dir.z;
        if (((int)z + 256 & 8192) != 0 && ((int)z + 256 & 4095) <= 1024)
        {
            if (ply_work.obj_work.spd_m > 0 && z < (ushort)32768)
                z -= (ushort)1152;
            else if (ply_work.obj_work.spd_m < 0 && z > (ushort)32768)
                z += (ushort)1152;
        }

        ply_work.obj_work.spd.x = AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathCos((int)z));
        ply_work.obj_work.spd.y = AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathSin((int)z));
        ply_work.obj_work.spd.x += AppMain.FX_Mul(ply_work.spd_jump, AppMain.mtMathSin((int)ply_work.obj_work.dir.z));
        ply_work.obj_work.spd.y += AppMain.FX_Mul(-ply_work.spd_jump, AppMain.mtMathCos((int)ply_work.obj_work.dir.z));
        if (((int)ply_work.gmk_flag & 4096) != 0)
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
        AppMain.GmPlySeqSetJumpState(ply_work, 0, 0U);
        if (ply_work.prev_seq_state == 10 && ply_work.no_spddown_timer >= 20)
            ply_work.no_spddown_timer = 20;
        AppMain.GmPlayerSetAtk(ply_work);
        if (AppMain.gm_ply_seq_jump_call_se_jump)
        {
            var modernSfx = gs.backup.SSave.CreateInstance().GetRemaster().ModernSoundEffects;
            AppMain.GmSoundPlaySE("Jump");
            //if (modernSfx)
            //    AppMain.GmSoundPlaySE("JumpSpin");
        }
        AppMain.GmPlyEfctCreateJumpDust(ply_work);
        AppMain.GmPlyEfctCreateSpinJumpBlur(ply_work);
    }

    private static void GmPlySeqSetJumpState(
        AppMain.GMS_PLAYER_WORK ply_work,
        int nofall_timer,
        uint flag)
    {
        ply_work.obj_work.user_timer = nofall_timer;
        if (ply_work.no_jump_move_timer == 0)
            ply_work.player_flag &= 4294967263U;
        ply_work.player_flag &= 4294967152U;
        if (((int)flag & 1) != 0)
            ply_work.player_flag |= 1U;
        if (((int)flag & 2) != 0)
            ply_work.player_flag |= 2U;
        if (((int)flag & 4) != 0)
            ply_work.player_flag |= 128U;
        if (((int)flag & 8) != 0)
            ply_work.player_flag |= 32U;
        if (((int)ply_work.player_flag & 262144) != 0)
            ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTruckJumpMain);
        else
            ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqJumpMain);
    }

    private static void GmPlySeqInitJumpEX(AppMain.GMS_PLAYER_WORK ply_work, int spd_x, int spd_y)
    {
        AppMain.GmPlySeqInitJump(ply_work);
        ply_work.obj_work.spd.x = spd_x;
        ply_work.obj_work.spd.y = spd_y;
        ply_work.obj_work.spd_m = 0;
        if (ply_work.obj_work.spd.x < 0)
        {
            if (ply_work.obj_work.spd_m > 0)
                ply_work.obj_work.spd_m = 0;
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                return;
            AppMain.GmPlayerSetReverse(ply_work);
        }
        else
        {
            if (ply_work.obj_work.spd_m < 0)
                ply_work.obj_work.spd_m = 0;
            if (((int)ply_work.obj_work.disp_flag & 1) == 0)
                return;
            AppMain.GmPlayerSetReverse(ply_work);
        }
    }

    private static void GmPlySeqAtkReactionInit(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.seq_state == 19)
        {
            AppMain.GmPlySeqChangeSequence(ply_work, 20);
        }
        else
        {
            if (((int)ply_work.obj_work.move_flag & 16) == 0)
                return;
            int x = ply_work.obj_work.spd.x;
            int spdM = ply_work.obj_work.spd_m;
            AppMain.GmPlayerStateInit(ply_work);
            AppMain.gm_ply_seq_jump_call_se_jump = false;
            AppMain.GmPlySeqChangeSequence(ply_work, 17);
            AppMain.gm_ply_seq_jump_call_se_jump = true;
            AppMain.GmPlySeqSetJumpState(ply_work, 0, 1U);
            ply_work.obj_work.spd.y = -16384;
            ply_work.obj_work.spd.x = x;
            ply_work.obj_work.spd_m = spdM;
        }
    }

    private static void GmPlySeqAtkReactionSpdInit(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int no_spddown_timer)
    {
        ply_work.obj_work.spd.x = spd_x;
        ply_work.no_spddown_timer = no_spddown_timer;
        AppMain.GmPlySeqAtkReactionInit(ply_work);
    }

    private static void gmPlySeqJumpMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int num = ply_work.obj_work.spd.y;
        if (((int)ply_work.gmk_flag & 4096) != 0)
        {
            num = ply_work.obj_work.spd.z;
            if (ply_work.obj_work.dir.x > (ushort)32768)
                num = -num;
        }

        if (ply_work.obj_work.user_timer != 0)
        {
            --ply_work.obj_work.user_timer;
            if (ply_work.obj_work.user_timer == 0)
                ply_work.obj_work.move_flag |= 128U;
        }

        if (((int)ply_work.player_flag & 5) == 0 && !AppMain.GmPlayerKeyCheckJumpKeyOn(ply_work) && num < -16384)
            ply_work.player_flag |= 4U;
        if (((int)ply_work.player_flag & 4) != 0 && ply_work.obj_work.spd.y < 0)
            ply_work.obj_work.spd.y += ply_work.obj_work.spd_fall;
        switch (ply_work.act_state)
        {
            case 44:
                if (num > 1024)
                {
                    AppMain.GmPlayerActionChange(ply_work, 45);
                    break;
                }

                break;
            case 45:
                if (((int)ply_work.obj_work.disp_flag & 8) != 0)
                {
                    AppMain.GmPlayerActionChange(ply_work, 46);
                    ply_work.obj_work.disp_flag |= 4U;
                    break;
                }

                break;
            case 47:
                if (((int)ply_work.obj_work.disp_flag & 8) != 0)
                {
                    ply_work.obj_work.disp_flag |= 1024U;
                    AppMain.GmPlayerActionChange(ply_work, 48);
                    ply_work.obj_work.disp_flag |= 4U;
                    break;
                }

                break;
        }

        if (((int)ply_work.obj_work.move_flag & 1) == 0)
            return;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqInitWallPush(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 17);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqWallPushMain);
    }

    private static void gmPlySeqWallPushMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 17 || ((int)ply_work.obj_work.disp_flag & 8) == 0)
            return;
        AppMain.GmPlayerActionChange(ply_work, 18);
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlySeqInitHoming(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.enemy_obj == null)
        {
            AppMain.GmPlySeqChangeSequence(ply_work, 21);
        }
        else
        {
            if (((int)ply_work.player_flag & 131072) == 0)
            {
                AppMain.GmPlayerActionChange(ply_work, 31);
                ply_work.obj_work.disp_flag |= 4U;
            }

            ply_work.obj_work.move_flag |= 32784U;
            ply_work.obj_work.move_flag &= 4294967166U;
            ply_work.player_flag |= 128U;
            ply_work.obj_work.dir.z = (ushort)0;
            ply_work.gmk_flag &= 4261410812U;
            ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqHomingMain);
            ply_work.obj_work.user_timer = 131072;
            ply_work.homing_timer = 98304;
            ply_work.homing_boost_timer = 262144;
            AppMain.GmPlayerSetAtk(ply_work);
            AppMain.GmPlyEfctCreateHomingImpact(ply_work);
            AppMain.GmSoundPlaySE("Homing");
        }
    }

    private static void GmPlySeqSetNoJumpMoveTime(AppMain.GMS_PLAYER_WORK ply_work, int time)
    {
        ply_work.no_jump_move_timer = time;
        ply_work.player_flag |= 32U;
    }

    private static void gmPlySeqHomingMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.user_timer == 0)
        {
            AppMain.GmPlySeqChangeSequence(ply_work, 16);
        }
        else
        {
            ply_work.obj_work.user_timer = AppMain.ObjTimeCountDown(ply_work.obj_work.user_timer);
            if (((int)ply_work.obj_work.move_flag & 1) != 0)
            {
                AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
                AppMain.GmPlySeqChangeSequence(ply_work, 0);
            }
            else
            {
                if (ply_work.enemy_obj == null)
                    return;
                AppMain.OBS_RECT_WORK obsRectWork = ((AppMain.GMS_ENEMY_COM_WORK)ply_work.enemy_obj).rect_work[2];
                int x = ply_work.enemy_obj.pos.x;
                int num1 = ((int)ply_work.enemy_obj.disp_flag & 2) == 0
                    ? ply_work.enemy_obj.pos.y + ((int)obsRectWork.rect.top + (int)obsRectWork.rect.bottom << 11)
                    : ply_work.enemy_obj.pos.y - ((int)obsRectWork.rect.top + (int)obsRectWork.rect.bottom << 11);
                float num2 = AppMain.FXM_FX32_TO_FLOAT(x - ply_work.obj_work.pos.x);
                int ang = AppMain.nnArcTan2((double)AppMain.FXM_FX32_TO_FLOAT(num1 - ply_work.obj_work.pos.y),
                    (double)num2) + (int)ply_work.obj_work.dir_fall;
                ply_work.obj_work.spd.x = (int)((double)AppMain.nnCos(ang) * 61440.0);
                ply_work.obj_work.spd.y = (int)((double)AppMain.nnSin(ang) * 61440.0);
                if (ply_work.obj_work.spd.x < 0)
                    ply_work.obj_work.disp_flag |= 1U;
                else if (ply_work.obj_work.spd.x > 0)
                    ply_work.obj_work.disp_flag &= 4294967294U;
                if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) <= 256 ||
                    ((int)ply_work.obj_work.move_flag & 4) == 0)
                    return;
                AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
                AppMain.GmPlySeqChangeSequence(ply_work, 0);
            }
        }
    }

    private static void GmPlySeqInitHomingRef(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 131072) == 0)
            AppMain.GmPlayerActionChange(ply_work, 32);
        ply_work.player_flag &= 4294967167U;
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 32912U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqHomingRefMain);
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = ((int)ply_work.player_flag & 67108864) == 0
            ? -20480
            : AppMain.GMD_PLAYER_WATERJUMP_GET(-20480);
        ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.player_flag &= 4294967280U;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
        AppMain.GmPlyEfctCreateJumpDust(ply_work);
    }

    private static void gmPlySeqHomingRefMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd.y >= 0)
        {
            AppMain.GmPlySeqChangeSequence(ply_work, 16);
        }
        else
        {
            if (((int)ply_work.obj_work.move_flag & 1) == 0)
                return;
            AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
        }
    }

    private static void GmPlySeqInitJumpDash(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 131072) == 0)
        {
            AppMain.GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.player_flag |= 160U;
        ply_work.obj_work.dir.z = (ushort)0;
        ply_work.gmk_flag &= 4261410812U;
        int ang = ((int)ply_work.obj_work.disp_flag & 1) == 0 ? 63488 : -30720;
        if (((int)ply_work.player_flag & 32768) != 0)
        {
            ply_work.obj_work.spd.y = 0;
            ply_work.obj_work.spd.x += (int)(4096.0 * (double)AppMain.nnCos(ang));
            ply_work.obj_work.spd.y += -(int)(4096.0 * (double)AppMain.nnSin(ang));
            ply_work.no_spddown_timer = 8;
            ply_work.obj_work.user_timer = 20;
        }
        else
        {
            ply_work.obj_work.spd.y = 0;
            ply_work.obj_work.spd.x += (int)(16384.0 * (double)AppMain.nnCos(ang));
            ply_work.obj_work.spd.y += -(int)(16384.0 * (double)AppMain.nnSin(ang));
            ply_work.no_spddown_timer = 8;
            ply_work.obj_work.user_timer = 20;
        }

        AppMain.GmPlayerSetAtk(ply_work);
        AppMain.GmPlyEfctCreateJumpDash(ply_work);
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqJumpDashMain);
    }

    private static void gmPlySeqJumpDashMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
        {
            ply_work.player_flag &= 4294967263U;
            AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
        }
        else if (ply_work.obj_work.user_timer == 0)
        {
            int x = ply_work.obj_work.spd.x;
            int y = ply_work.obj_work.spd.y;
            AppMain.GmPlySeqChangeSequence(ply_work, 16);
            ply_work.obj_work.spd.x = x;
            ply_work.obj_work.spd.y = y;
            ply_work.player_flag &= 4294967263U;
        }
        else
            --ply_work.obj_work.user_timer;
    }

    private static void GmPlySeqChangeDamage(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequence(ply_work, 22);
    }

    private static void GmPlySeqChangeDamageSetSpd(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        AppMain.GmPlySeqChangeSequence(ply_work, 22);
        ply_work.obj_work.spd.x = spd_x;
        ply_work.obj_work.spd.y = spd_y;
        if (spd_x < 0)
            ply_work.obj_work.disp_flag &= 4294967294U;
        else
            ply_work.obj_work.disp_flag |= 1U;
    }

    private static void GmPlySeqInitDamage(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerStateInit(ply_work);
        if (((int)ply_work.player_flag & 32768) != 0)
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
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                ply_work.obj_work.spd.x = -ply_work.obj_work.spd.x;
        }

        AppMain.GmPlayerActionChange(ply_work, 36);
        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.invincible_timer = ply_work.time_damage;
        AppMain.GmPlayerSetDefInvincible(ply_work);
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqDamageMain);
        ply_work.obj_work.disp_flag |= 4U;
        AppMain.GMM_PAD_VIB_LARGE_TIME(60f);
    }

    private static void gmPlySeqDamageMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.move_flag & 1) == 0)
            return;
        ply_work.rect_work[0].flag &= 4294967039U;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqChangeDeath(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequence(ply_work, 23);
    }

    private static void GmPlySeqInitDeath(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 1024) != 0 || ((int)ply_work.player_flag & 16777216) != 0)
            return;
        if (((int)ply_work.player_flag & 16384) != 0)
            AppMain.GmPlayerSetEndSuperSonic(ply_work);
        AppMain.GmPlayerStateInit(ply_work);
        ply_work.obj_work.disp_flag &= 4294967294U;
        ply_work.obj_work.move_flag |= 768U;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.y = -ply_work.spd_jump;
        ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.dir.z = (ushort)0;
        ply_work.obj_work.pos.z = 983040;
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            ply_work.jump_pseudofall_dir = AppMain.g_gm_main_system.pseudofall_dir;
            ply_work.gmk_flag |= 16777216U;
            ply_work.obj_work.dir.x = ply_work.obj_work.dir.y = (ushort)0;
            ply_work.obj_work.dir.z = (ushort)0;
            ply_work.obj_work.move_flag &= 4294967231U;
        }

        ply_work.player_flag &= 3489660927U;
        ply_work.player_flag |= 1024U;
        ply_work.obj_work.flag |= 2U;
        if (((int)ply_work.player_flag & 67108864) != 0)
            AppMain.GmSoundPlaySE("Damage3");
        else
            AppMain.GmSoundPlaySE("Damage1");
        AppMain.GmPlayerActionChange(ply_work, 37);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqDeathMain);
        ply_work.obj_work.user_timer = 0;
        ply_work.water_timer = 0;
        AppMain.GMM_PAD_VIB_LARGE_TIME(90f);
    }

    private static void gmPlySeqDeathMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state == 37 && ((int)ply_work.obj_work.disp_flag & 8) != 0)
        {
            AppMain.GmPlayerActionChange(ply_work, 38);
            ply_work.obj_work.disp_flag |= 4U;
        }

        if (((int)ply_work.player_flag & 262144) == 0)
            return;
        ply_work.obj_work.dir.z += (ushort)1024;
    }

    private static void GmPlySeqChangeTransformSuper(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequence(ply_work, 24);
    }

    private static void GmPlySeqInitTransformSuper(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int num1 = 0;
        int num2 = 0;
        if (((int)ply_work.player_flag & 1024) != 0 || ((int)ply_work.player_flag & 16777216) != 0)
            return;
        ushort z = ply_work.obj_work.dir.z;
        if (((int)ply_work.obj_work.move_flag & 16) == 0)
        {
            num1 = AppMain.FXM_FLOAT_TO_FX32(AppMain.nnCos(81920 - (int)ply_work.obj_work.dir.z) * 3f);
            num2 = -AppMain.FXM_FLOAT_TO_FX32(AppMain.nnSin(81920 - (int)ply_work.obj_work.dir.z) * 3f);
        }

        AppMain.GmPlayerStateInit(ply_work);
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.flag |= 2U;
        if (((int)ply_work.player_flag & 262144) != 0)
            ply_work.obj_work.move_flag |= 8448U;
        if (((int)ply_work.obj_work.move_flag & 16) == 0)
        {
            ply_work.obj_work.move_flag |= 16U;
            ply_work.obj_work.move_flag &= 4294967280U;
            ply_work.obj_work.pos.x += num1;
            ply_work.obj_work.pos.y += num2;
        }

        ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.dir.z = (ushort)0;
        if (((int)ply_work.player_flag & 262144) != 0)
            ply_work.obj_work.dir.z = z;
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            ply_work.obj_work.pos.z = (int)short.MinValue;
            ply_work.gmk_flag |= 536870912U;
        }

        AppMain.GmPlayerActionChange(ply_work, 50);
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTransformSuperMain);
        ply_work.obj_work.user_timer = 593920;
        ply_work.obj_work.user_work = 0U;
        AppMain.GmPlyEfctCreateSuperStart(ply_work);
    }

    public static void gmPlySeqTransformSuperMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.user_timer = AppMain.ObjTimeCountDown(ply_work.obj_work.user_timer);
        if (ply_work.act_state == 50)
        {
            if (((int)ply_work.obj_work.disp_flag & 8) != 0)
            {
                AppMain.GmPlayerActionChange(ply_work, 51);
                ply_work.obj_work.disp_flag |= 4U;
            }
        }
        else if (ply_work.act_state != 52 && ((long)ply_work.obj_work.user_timer & 4294963200L) == 286720L)
            AppMain.GmPlayerActionChange(ply_work, 52);

        if (((long)ply_work.obj_work.user_timer & 4294963200L) == 245760L && ((int)ply_work.player_flag & 16384) == 0)
        {
            AppMain.GMS_PLAYER_RESET_ACT_WORK reset_act_work = new AppMain.GMS_PLAYER_RESET_ACT_WORK();
            ushort z = ply_work.obj_work.dir.z;
            AppMain.GmPlayerActionChange(ply_work, 53);
            ply_work.obj_work.disp_flag |= 4U;
            AppMain.GmPlayerSaveResetAction(ply_work, reset_act_work);
            AppMain.GmPlayerSetSuperSonic(ply_work);
            AppMain.GmPlayerResetAction(ply_work, reset_act_work);
            ply_work.obj_work.move_flag &= 4294967167U;
            ply_work.obj_work.flag |= 2U;
            if (((int)ply_work.player_flag & 262144) != 0)
                ply_work.obj_work.move_flag |= 8448U;
            if (((int)ply_work.player_flag & 262144) != 0)
                ply_work.obj_work.dir.z = z;
        }

        if (ply_work.obj_work.user_timer != 0)
            return;
        ply_work.obj_work.move_flag |= 128U;
        ply_work.obj_work.flag &= 4294967293U;
        ply_work.obj_work.move_flag &= 4294958847U;
        ply_work.super_sonic_ring_timer = 245760;
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            ply_work.obj_work.pos.z = 0;
            ply_work.gmk_flag &= 3758096383U;
        }

        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqChangeActGoal(AppMain.GMS_PLAYER_WORK ply_work)
    {
        SaveState.deleteSave();
        if (((int)ply_work.player_flag & 1024) != 0 || ((int)AppMain.g_gm_main_system.game_flag & 16384) != 0)
            return;
        uint moveFlag = ply_work.obj_work.move_flag;
        AppMain.GmPlayerStateInit(ply_work);
        if (ply_work.seq_state == 11 || ply_work.seq_state == 12)
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
        ply_work.obj_work.move_flag |= moveFlag & 1U;
        ply_work.obj_work.move_flag &= 4294441983U;
        ply_work.player_flag |= 22020096U;
        AppMain.GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
    }

    private static void gmPlySeqActGoal(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)AppMain.g_gm_main_system.game_flag & 16384) != 0)
            return;
        ply_work.player_flag |= 4194304U;
        AppMain.GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
        ply_work.water_timer = 0;
        if (AppMain.FXM_FLOAT_TO_FX32(AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id).disp_pos.x) +
            ((int)AppMain.OBD_LCD_X >> 1) + 128 <= ply_work.obj_work.pos.x >> 12)
            return;
        ply_work.key_on |= (ushort)8;
        ply_work.key_walk_rot_z = (int)short.MaxValue;
    }

    private static void GmPlySeqChangeBossGoal(
        AppMain.GMS_PLAYER_WORK ply_work,
        int capsule_pos_x,
        int capsule_pos_y)
    {
        SaveState.deleteSave();
        if (((int)ply_work.player_flag & 1024) != 0)
            return;
        AppMain.GmPlayerStateInit(ply_work);
        ply_work.player_flag |= 23068672U;
        ply_work.rect_work[0].def_power = (short)3;
        ply_work.gmk_work0 = capsule_pos_x;
        ply_work.gmk_work1 = capsule_pos_y;
        ply_work.gmk_work2 = ply_work.obj_work.pos.x < capsule_pos_x ? 1 : 0;
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void gmPlySeqBossGoalPre(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag |= 4194304U;
        ply_work.rect_work[0].def_power = (short)3;
        ply_work.water_timer = 0;
        if (((int)ply_work.obj_work.move_flag & 1) == 0 || ((int)ply_work.obj_work.move_flag & 1) != 0 &&
            ply_work.obj_work.pos.y < ply_work.gmk_work1 - 98304)
        {
            if (((int)ply_work.obj_work.move_flag & 1) == 0 &&
                AppMain.MTM_MATH_ABS(ply_work.obj_work.pos.x - ply_work.gmk_work0) > 262144)
                return;
            if (ply_work.gmk_work2 != 0)
            {
                ply_work.key_on |= (ushort)4;
                ply_work.key_walk_rot_z = -32767;
            }
            else
            {
                ply_work.key_on |= (ushort)8;
                ply_work.key_walk_rot_z = (int)short.MaxValue;
            }
        }
        else
        {
            ply_work.player_flag &= 4292870143U;
            AppMain.GmPlySeqChangeSequence(ply_work, 25);
        }
    }

    private static void GmPlySeqInitBossGaol(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 1024) != 0)
            return;
        AppMain.GmPlayerStateInit(ply_work);
        ply_work.player_flag |= 4194304U;
        ply_work.rect_work[0].def_power = (short)3;
        ply_work.water_timer = 0;
        AppMain.GmPlayerActionChange(ply_work, 0);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.user_timer = 245760;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqBossGoalMain);
    }

    private static void gmPlySeqBossGoalMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state == 0)
        {
            ply_work.obj_work.user_timer = AppMain.ObjTimeCountDown(ply_work.obj_work.user_timer);
            if (ply_work.obj_work.user_timer != 0)
                return;
            if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                AppMain.GmPlySeqSetProgramTurn(ply_work, (ushort)4096);
            AppMain.GmPlayerActionChange(ply_work, 54);
        }
        else
        {
            if (((int)ply_work.obj_work.disp_flag & 8) == 0 || ply_work.act_state != 54)
                return;
            AppMain.GmPlayerActionChange(ply_work, 55);
            ply_work.obj_work.disp_flag |= 4U;
        }
    }

    private static void GmPlySeqChangeBoss5Demo(
        AppMain.GMS_PLAYER_WORK ply_work,
        int dest_pos_x,
        bool is_goal)
    {
        if (((int)ply_work.player_flag & 1024) != 0)
            return;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        ply_work.player_flag |= 1077936128U;
        if (is_goal)
        {
            SaveState.deleteSave();
            ply_work.player_flag |= 16777216U;
            ply_work.rect_work[0].def_power = (short)3;
        }

        ply_work.gmk_work0 = dest_pos_x;
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void gmPlySeqBoss5DemoPre(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.pos.x >= ply_work.gmk_work0)
        {
            if (ply_work.obj_work.spd.x != 0)
                return;
            ply_work.player_flag &= 3221225471U;
            AppMain.GmPlySeqChangeSequence(ply_work, 26);
        }
        else
        {
            ply_work.key_on |= (ushort)8;
            ply_work.key_walk_rot_z = (int)short.MaxValue;
        }
    }

    private static void GmPlySeqInitBoss5Demo(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 1024) != 0)
            return;
        AppMain.GmPlayerStateInit(ply_work);
        ply_work.player_flag |= 4194304U;
        if (ply_work.act_state != 0)
        {
            AppMain.GmPlayerActionChange(ply_work, 0);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqBoss5DemoMain);
    }

    private static void gmPlySeqBoss5DemoMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlySeqChangeBoss5DemoEnd(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 1024) != 0)
            return;
        ply_work.player_flag &= 3217031167U;
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void GmPlySeqChangeTRetryFw(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequence(ply_work, 27);
    }

    private static void GmPlySeqInitTRetryFw(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 131072) != 0)
            AppMain.GmPlayerSetEndPinballSonic(ply_work);
        if (((int)ply_work.player_flag & 262144) != 0)
            AppMain.GmPlayerSetEndTruckRide(ply_work);
        AppMain.GmPlayerSpdParameterSet(ply_work);
        ply_work.obj_work.dir.x = (ushort)0;
        ply_work.obj_work.dir.y = (ushort)0;
        ply_work.obj_work.dir.z = (ushort)0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd.z = 0;
        ply_work.obj_work.disp_flag &= 4294967292U;
        AppMain.GmPlayerActionChange(ply_work, 4);
        ply_work.player_flag &= 4293918719U;
        ply_work.obj_work.spd_m = 0;
        ply_work.water_timer = 0;
        ply_work.rect_work[0].def_power = (short)3;
        ply_work.invincible_timer = 0;
        ply_work.obj_work.move_flag = 16192U;
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.flag |= 2U;
        ply_work.obj_work.dir_fall = (ushort)0;
        ply_work.ply_pseudofall_dir = 0;
        ply_work.jump_pseudofall_dir = (ushort)0;
        AppMain.g_gm_main_system.pseudofall_dir = (ushort)0;
        ply_work.player_flag |= 4194304U;
        ply_work.player_flag &= 3220176895U;
        ply_work.obj_work.dir_slope = (ushort)192;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTRetryFw);
    }

    private static void gmPlySeqTRetryFw(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.disp_flag & 8) != 0)
            AppMain.GmPlayerActionChange(ply_work, 5);
        ply_work.water_timer = 0;
        ply_work.rect_work[0].def_power = (short)3;
    }

    private static void GmPlySeqChangeTRetryAcc(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequence(ply_work, 28);
    }

    private static void GmPlySeqInitTRetryAcc(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag |= 512U;
        AppMain.GmPlySeqMoveWalk(ply_work);
        AppMain.GmPlayerWalkActionSet(ply_work);
        ply_work.obj_work.user_timer = 0;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTRetryAcc);
    }

    private static void gmPlySeqTRetryAcc(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ++ply_work.obj_work.user_timer;
        ply_work.obj_work.spd_m += 512;
        if (ply_work.obj_work.user_timer > 100)
        {
            ply_work.seq_func = (AppMain.seq_func_delegate)null;
            ply_work.obj_work.user_timer = 0;
            ply_work.obj_work.move_flag &= 4294959103U;
            ply_work.obj_work.move_flag &= 4294966271U;
            ply_work.obj_work.flag |= 16U;
        }

        if (ply_work.obj_work.spd_m > ply_work.spd4 - 512 && ((int)ply_work.player_flag & 1048576) == 0)
        {
            ply_work.obj_work.dir.z = (ushort)4097;
            AppMain.GmPlayerWalkActionSet(ply_work);
            AppMain.GmPlayerWalkActionCheck(ply_work);
            ply_work.obj_work.dir.z = (ushort)0;
            AppMain.GmPlySeqChangeTRetryRun(ply_work);
        }

        ply_work.water_timer = 0;
        ply_work.rect_work[0].def_power = (short)3;
    }

    private static void GmPlySeqChangeTRetryRun(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.player_flag |= 1048576U;
    }

    private static void GmPlySeqInitTruckFw(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m != 0)
        {
            AppMain.GmPlySeqChangeSequence(ply_work, 1);
        }
        else
        {
            if (((int)ply_work.obj_work.move_flag & 4194304) == 0)
            {
                ply_work.gmk_flag &= 4293918719U;
                AppMain.GmPlayerActionChange(ply_work, 73);
            }
            else if (ply_work.obj_3d[(int)AppMain.g_gm_player_model_tbl[(int)ply_work.char_id][0]].act_id[0] !=
                     (int)AppMain.g_gm_player_motion_right_tbl[(int)ply_work.char_id][0] &&
                     ply_work.obj_3d[(int)AppMain.g_gm_player_model_tbl[(int)ply_work.char_id][73]].act_id[0] !=
                     (int)AppMain.g_gm_player_motion_right_tbl[(int)ply_work.char_id][73])
            {
                if (((int)ply_work.gmk_flag & 1048576) != 0)
                    AppMain.GmPlayerActionChange(ply_work, 70);
                else
                    AppMain.GmPlayerActionChange(ply_work, 69);
                ply_work.obj_work.disp_flag |= 4U;
            }

            ply_work.obj_work.move_flag &= 4294967279U;
            ply_work.obj_work.user_timer = 0;
            ply_work.obj_work.user_work = 0U;
            ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTruckFwMain);
        }
    }

    private static void gmPlySeqTruckFwMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_3d[(int)AppMain.g_gm_player_model_tbl[(int)ply_work.char_id][73]].act_id[0] ==
            (int)AppMain.g_gm_player_motion_right_tbl[(int)ply_work.char_id][73] &&
            ((int)ply_work.obj_work.disp_flag & 8) != 0)
        {
            if (((int)ply_work.gmk_flag & 1048576) != 0)
                AppMain.GmPlayerActionChange(ply_work, 70);
            else
                AppMain.GmPlayerActionChange(ply_work, 69);
            ply_work.obj_work.disp_flag |= 4U;
        }

        if (ply_work.act_state == 69 || ply_work.act_state == 70)
        {
            if (((int)ply_work.obj_work.disp_flag & 8) == 0)
                return;
            int userWork = (int)ply_work.obj_work.user_work;
            ++ply_work.obj_work.user_work;
            if ((int)ply_work.obj_work.user_work < 8)
                return;
            AppMain.GmPlayerActionChange(ply_work, 2);
            ply_work.obj_work.user_work = 0U;
        }
        else if (ply_work.act_state == 2 || ply_work.act_state == 4 || ply_work.act_state == 6)
        {
            if (((int)ply_work.obj_work.disp_flag & 8) == 0)
                return;
            AppMain.GmPlayerActionChange(ply_work, ply_work.act_state + 1);
            ply_work.obj_work.disp_flag |= 4U;
            ply_work.obj_work.user_work = 0U;
        }
        else
        {
            if (ply_work.act_state != 3 || ((int)ply_work.obj_work.disp_flag & 8) == 0)
                return;
            int num = (int)ply_work.obj_work.user_work + 1;
            ply_work.obj_work.user_work = (uint)num;
            if ((int)ply_work.obj_work.user_work < 10)
                return;
            AppMain.GmPlayerActionChange(ply_work, 4);
            ply_work.obj_work.user_work = 0U;
        }
    }

    private static void GmPlySeqInitTruckWalk(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.move_flag & 4194304) == 0)
            AppMain.GmPlayerActionChange(ply_work, 73);
        else if (ply_work.act_state != 71 && ply_work.act_state != 72)
        {
            if (((int)ply_work.gmk_flag & 1048576) != 0)
                AppMain.GmPlayerActionChange(ply_work, 72);
            else
                AppMain.GmPlayerActionChange(ply_work, 71);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTruckWalkMain);
        ply_work.obj_work.user_timer = 0;
        ply_work.truck_left_flip_timer = 245760;
    }

    private static void gmPlySeqTruckWalkMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        bool flag = false;
        if (ply_work.obj_work.spd_m < 0 && ply_work.act_state == 71)
        {
            ply_work.truck_left_flip_timer = AppMain.ObjTimeCountDown(ply_work.truck_left_flip_timer);
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
            if (((int)ply_work.gmk_flag & 1048576) != 0)
                AppMain.GmPlayerActionChange(ply_work, 72);
            else
                AppMain.GmPlayerActionChange(ply_work, 71);
            ply_work.obj_work.disp_flag |= 4U;
            ply_work.obj_work.obj_3d.frame[0] = ply_work.obj_work.obj_3d.frame[1] = num;
        }

        if (ply_work.obj_3d[(int)AppMain.g_gm_player_model_tbl[(int)ply_work.char_id][73]].act_id[0] !=
            (int)AppMain.g_gm_player_motion_right_tbl[(int)ply_work.char_id][73] ||
            ((int)ply_work.obj_work.disp_flag & 8) == 0)
            return;
        if (ply_work.obj_work.spd_m >= 0)
        {
            ply_work.gmk_flag &= 4293918719U;
            AppMain.GmPlayerActionChange(ply_work, 71);
        }
        else if (ply_work.obj_work.spd_m < 0)
        {
            ply_work.gmk_flag |= 1048576U;
            AppMain.GmPlayerActionChange(ply_work, 72);
        }

        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void GmPlySeqInitTruckFall(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_3d[(int)AppMain.g_gm_player_model_tbl[(int)ply_work.char_id][40]].act_id[0] !=
            (int)AppMain.g_gm_player_motion_right_tbl[(int)ply_work.char_id][40])
        {
            AppMain.GmPlayerActionChange(ply_work, 40);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 49296U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.gmk_flag &= 1073479679U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTruckJumpMain);
        ushort num = (ushort)((uint)ply_work.obj_work.dir.z + (uint)ply_work.obj_work.dir_fall -
                               (uint)ply_work.jump_pseudofall_dir);
        ply_work.obj_work.spd.x = AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathCos((int)num));
        ply_work.obj_work.spd.y = AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathSin((int)num));
        ply_work.player_flag &= 4294967280U;
        ply_work.player_flag |= 1U;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
    }

    private static void GmPlySeqInitTruckJump(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_3d[(int)AppMain.g_gm_player_model_tbl[(int)ply_work.char_id][40]].act_id[0] !=
            (int)AppMain.g_gm_player_motion_right_tbl[(int)ply_work.char_id][40])
        {
            AppMain.GmPlayerActionChange(ply_work, 40);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag |= 49168U;
        ply_work.obj_work.move_flag &= 4290772990U;
        ushort z = ply_work.obj_work.dir.z;
        if (((int)z + 256 & 8192) != 0 && ((int)z + 256 & 4095) <= 1024)
        {
            if (ply_work.obj_work.spd_m > 0 && z < (ushort)32768)
                z -= (ushort)1152;
            else if (ply_work.obj_work.spd_m < 0 && z > (ushort)32768)
                z += (ushort)1152;
        }

        ushort num1 = (ushort)((uint)z + (uint)ply_work.obj_work.dir_fall - (uint)ply_work.jump_pseudofall_dir);
        ply_work.obj_work.spd.x = AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathCos((int)num1));
        ply_work.obj_work.spd.y = AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathSin((int)num1));
        ushort num2 = (ushort)((uint)ply_work.obj_work.dir.z + (uint)ply_work.obj_work.dir_fall -
                                (uint)ply_work.jump_pseudofall_dir);
        ply_work.obj_work.spd.x += AppMain.FX_Mul(ply_work.spd_jump, AppMain.mtMathSin((int)num2));
        ply_work.obj_work.spd.y += AppMain.FX_Mul(-ply_work.spd_jump, AppMain.mtMathCos((int)num2));
        ply_work.player_flag &= 4294967280U;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
        AppMain.GmPlySeqSetJumpState(ply_work, 0, 0U);
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTruckJumpMain);
        AppMain.GmPlayerSetAtk(ply_work);
        AppMain.GmSoundPlaySE("Lorry3");
    }

    private static void gmPlySeqTruckJumpMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int y = ply_work.obj_work.spd.y;
        if (ply_work.obj_work.user_timer != 0)
        {
            --ply_work.obj_work.user_timer;
            if (ply_work.obj_work.user_timer == 0)
                ply_work.obj_work.move_flag |= 128U;
        }

        if (((int)ply_work.player_flag & 5) == 0 && !AppMain.GmPlayerKeyCheckJumpKeyOn(ply_work) && y < -16384)
            ply_work.player_flag |= 4U;
        if (((int)ply_work.player_flag & 4) != 0 && ply_work.obj_work.spd.y < 0)
            ply_work.obj_work.spd.y += ply_work.obj_work.spd_fall;
        if (((int)ply_work.obj_work.move_flag & 6) == 0)
        {
            bool flag = false;
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) < 4096)
                flag = true;
            if (flag)
            {
                ushort num = (ushort)((uint)ply_work.obj_work.dir_fall -
                                       (uint)AppMain.g_gm_main_system.pseudofall_dir);
                ply_work.obj_work.spd.x += AppMain.mtMathSin((int)num);
            }
        }

        if (((int)ply_work.obj_work.move_flag & 2) != 0)
            ply_work.obj_work.spd.y += ply_work.obj_work.spd_fall * 5;
        if (((int)ply_work.obj_work.move_flag & 1) == 0)
            return;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmSoundPlaySE("Lorry4");
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
        AppMain.GMM_PAD_VIB_MID();
    }

    private static void GmPlySeqInitTruckSquatStart(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 14);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTruckSquatMain);
    }

    private static void GmPlySeqInitTruckSquatMiddle(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 15);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 4096)
            ply_work.obj_work.spd_m = 0;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTruckSquatMain);
    }

    private static void GmPlySeqInitTruckSquatEnd(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 16);
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqSquatEndMain);
    }

    private static void gmPlySeqTruckSquatMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m != 0)
        {
            AppMain.GmPlySeqChangeSequence(ply_work, 1);
        }
        else
        {
            if (ply_work.act_state != 14 || ((int)ply_work.obj_work.disp_flag & 8) == 0)
                return;
            AppMain.GmPlySeqChangeSequence(ply_work, 7);
        }
    }

    private static void GmPlySeqInitTruckStaggerFront(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 33);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTruckStaggerMain);
        ply_work.obj_work.user_timer = 0;
        AppMain.GmPlyEfctCreateSweat(ply_work);
    }

    private static void GmPlySeqInitTruckStaggerBack(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerActionChange(ply_work, 34);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqTruckStaggerMain);
        ply_work.obj_work.user_timer = 0;
        AppMain.GmPlyEfctCreateSweat(ply_work);
    }

    private static void gmPlySeqTruckStaggerMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.gmk_flag & 262144) == 0)
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
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

    private static void GmPlySeqLandingSet(AppMain.GMS_PLAYER_WORK ply_work, ushort dir_z)
    {
        AppMain.GmPlayerSpdParameterSet(ply_work);
        ply_work.obj_work.move_flag &= 4294934511U;
        ply_work.obj_work.move_flag |= 128U;
        ply_work.obj_work.disp_flag &= 4294967263U;
        ply_work.gmk_flag &= 4278190079U;
        ply_work.gmk_flag2 &= 4294967007U;
        ply_work.gmk_flag2 &= 4294966783U;
        ply_work.player_flag &= 4294967135U;
        ply_work.no_jump_move_timer = 0;
        ply_work.score_combo_cnt = 0U;
        if (((int)ply_work.gmk_flag & 1) == 0 && ((int)ply_work.obj_work.col_flag & 1) != 0)
            ply_work.obj_work.dir.z = (ushort)0;
        if (dir_z > (ushort)0)
        {
            if (((int)ply_work.player_flag & 262144) != 0)
            {
                if (((int)ply_work.gmk_flag2 & 1) != 0)
                {
                    ply_work.obj_work.spd_m += AppMain.FX_Mul(AppMain.FX_Mul(ply_work.obj_work.move.x, 2048),
                        AppMain.mtMathCos(
                            (int)(ushort)((uint)dir_z - (uint)AppMain.g_gm_main_system.pseudofall_dir)));
                    ply_work.obj_work.spd_m += AppMain.FX_Mul(AppMain.FX_Mul(ply_work.obj_work.move.y, 2048),
                        AppMain.mtMathSin(
                            (int)(ushort)((uint)dir_z - (uint)AppMain.g_gm_main_system.pseudofall_dir)));
                }
                else
                {
                    ply_work.obj_work.spd_m += AppMain.FX_Mul(ply_work.obj_work.move.x,
                        AppMain.mtMathCos(
                            (int)(ushort)((uint)dir_z - (uint)AppMain.g_gm_main_system.pseudofall_dir)));
                    ply_work.obj_work.spd_m += AppMain.FX_Mul(ply_work.obj_work.move.y,
                        AppMain.mtMathSin(
                            (int)(ushort)((uint)dir_z - (uint)AppMain.g_gm_main_system.pseudofall_dir)));
                }
            }
            else
            {
                ply_work.obj_work.spd_m += AppMain.FX_Mul(ply_work.obj_work.move.x, AppMain.mtMathCos((int)dir_z));
                ply_work.obj_work.spd_m += AppMain.FX_Mul(ply_work.obj_work.move.y, AppMain.mtMathSin((int)dir_z));
                if (AppMain.ObjObjectDirFallReverseCheck(ply_work.obj_work.dir_fall) != 0U)
                    ply_work.obj_work.spd_m = -ply_work.obj_work.spd_m;
            }
        }
        else if (((int)ply_work.player_flag & 262144) != 0)
        {
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x))
                ply_work.obj_work.spd_m = ply_work.obj_work.spd.x;
            ply_work.obj_work.spd_m += AppMain.FX_Mul(AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x),
                AppMain.mtMathSin((int)(ushort)((uint)ply_work.obj_work.dir.z +
                                                  ((uint)ply_work.obj_work.dir_fall -
                                                   (uint)AppMain.g_gm_main_system.pseudofall_dir))));
        }
        else
        {
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x))
                ply_work.obj_work.spd_m = ply_work.obj_work.spd.x;
            ply_work.obj_work.spd_m += AppMain.FX_Mul(AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x),
                AppMain.mtMathSin((int)ply_work.obj_work.dir.z));
        }

        ply_work.gmk_flag2 &= 4294967294U;
        ply_work.spd_work_max = AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
        if (ply_work.spd_work_max > ply_work.obj_work.spd_slope_max)
            ply_work.spd_work_max = ply_work.obj_work.spd_slope_max;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        if (dir_z != (ushort)0)
            ply_work.obj_work.dir.z = dir_z;
        if (((int)ply_work.gmk_flag & 4096) == 0)
            ply_work.obj_work.dir.x = (ushort)0;
        if (((int)ply_work.player_flag & 262144) == 0)
            return;
        ply_work.obj_work.move_flag &= 4294950911U;
    }

    private static void GmPlySeqMoveFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)obj_work;
        AppMain.GMS_PLY_SEQ_STATE_DATA[] seqStateDataTbl = ply_work.seq_state_data_tbl;
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & int.MinValue) != 0)
        {
            if (((int)ply_work.player_flag & 32768) != 0)
                AppMain.GmPlySeqMoveWalkAutoRun(ply_work);
            else if (((int)ply_work.player_flag & 262144) != 0)
                AppMain.GmPlySeqMoveWalkTruck(ply_work);
            else
                AppMain.GmPlySeqMoveWalk(ply_work);
        }

        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 1073741824) != 0 &&
            ((int)ply_work.player_flag & 32) == 0)
        {
            if (((int)ply_work.player_flag & 32768) != 0)
                AppMain.GmPlySeqMoveJumpAutoRun(ply_work);
            else if (((int)ply_work.player_flag & 262144) != 0)
                AppMain.GmPlySeqMoveJumpTruck(ply_work);
            else
                AppMain.GmPlySeqMoveJump(ply_work);
        }

        if (ply_work.no_jump_move_timer != 0)
        {
            ply_work.no_jump_move_timer = AppMain.ObjTimeCountDown(ply_work.no_jump_move_timer);
            if (ply_work.no_jump_move_timer == 0)
                ply_work.player_flag &= 4294967263U;
        }

        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 536870912) != 0)
            AppMain.GmPlySeqMoveSpin(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 134217728) != 0)
            AppMain.GmPlySeqMoveSpinNoDec(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 67108864) != 0)
            AppMain.GmPlySeqMoveSpinPinball(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 268435456) != 0)
        {
            if (((int)ply_work.player_flag & 262144) != 0)
                AppMain.GmPlySeqTruckJumpDirec(ply_work);
            else if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
                AppMain.gmPlySeqSplJumpDirec(ply_work);
            else
                AppMain.GmPlySeqJumpDirec(ply_work);
        }

        if (((int)ply_work.player_flag & 32768) != 0 && ((int)ply_work.player_flag & 1024) == 0 &&
            (obj_work.pos.x <= AppMain.g_obj.camera[0][0] + 65536 &&
             obj_work.pos.x > AppMain.g_obj.camera[0][0] + 65536 - 4194304))
        {
            if (((int)obj_work.move_flag & 16) != 0)
            {
                if (obj_work.spd.x < ply_work.scroll_spd_x)
                {
                    obj_work.spd.x = ply_work.scroll_spd_x;
                    if (((int)obj_work.disp_flag & 1) != 0)
                        AppMain.GmPlySeqSetProgramTurn(ply_work, (ushort)4096);
                }
            }
            else if (obj_work.spd_m < ply_work.scroll_spd_x)
            {
                obj_work.spd_m = ply_work.scroll_spd_x;
                if (3 <= ply_work.seq_state && ply_work.seq_state <= 5)
                    AppMain.GmPlySeqChangeSequence(ply_work, 0);
                if (((int)obj_work.disp_flag & 1) != 0)
                {
                    if (ply_work.seq_state == 10)
                    {
                        AppMain.GmPlayerSetReverse(ply_work);
                        AppMain.GmPlySeqChangeSequence(ply_work, 0);
                    }
                    else if (6 <= ply_work.seq_state && ply_work.seq_state <= 8)
                        AppMain.GmPlySeqSetProgramTurn(ply_work, (ushort)4096);
                    else
                        AppMain.GmPlySeqChangeSequence(ply_work, 2);
                }
            }
        }

        if (((int)ply_work.player_flag & 262144) != 0)
        {
            if (((int)obj_work.move_flag & 1) != 0 && ((int)ply_work.gmk_flag & 262144) != 0)
                return;
            AppMain.gmPlySeqTruckMove(obj_work);
        }
        else if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            AppMain.gmPlySeqSplMove(obj_work);
        else
            AppMain.ObjObjectMove(obj_work);
    }

    private static void GmPlySeqMoveWalk(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int spdAdd = ply_work.spd_add;
        int fSpd = ply_work.spd_dec;
        int num1 = ply_work.spd_max;
        if (AppMain.GmPlayerKeyCheckWalkRight(ply_work) || AppMain.GmPlayerKeyCheckWalkLeft(ply_work))
        {
            int num2 = AppMain.MTM_MATH_ABS(ply_work.key_walk_rot_z);
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
        if (ply_work.obj_work.dir.z > (ushort)0)
        {
            int num2 = AppMain.FX_Mul(ply_work.spd_max_add_slope, AppMain.mtMathSin((int)ply_work.obj_work.dir.z));
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
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) > ply_work.spd3)
            {
                int num2;
                if (num1 - ply_work.spd3 != 0)
                {
                    num2 = AppMain.FX_Div(AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) - ply_work.spd3,
                        num1 - ply_work.spd3);
                    if (num2 > 4096)
                        num2 = 4096;
                }
                else
                    num2 = 4096;

                v2 = num2 * 3968 >> 12;
            }

            spdAdd -= AppMain.FX_Mul(spdAdd, v2);
        }

        if (((int)ply_work.player_flag & 67108864) != 0)
        {
            AppMain.GMD_PLAYER_WATER_SET(ref spdAdd);
            AppMain.GMD_PLAYER_WATER_SET(ref fSpd);
        }

        if (ply_work.spd_work_max >= num1 && AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) >= num1)
        {
            if (ply_work.spd_work_max > ply_work.obj_work.spd_m)
                ply_work.spd_work_max = AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
            num1 = ply_work.spd_work_max;
        }

        if (((int)ply_work.player_flag & 32768) != 0 && AppMain.GmPlayerKeyCheckWalkRight(ply_work) &&
            num1 > ply_work.scroll_spd_x + 8192)
            num1 = ply_work.scroll_spd_x + 8192;
        if (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) | AppMain.GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdUpSet(ply_work.obj_work.spd_m, spdAdd, num1);
            }
            else
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdUpSet(ply_work.obj_work.spd_m, -spdAdd, num1);
            }
        }
        else
        {
            ply_work.spd_pool = (short)0;
            ply_work.obj_work.spd.x = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd.x, -num1, num1);
            ply_work.obj_work.spd_m = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd_m, -num1, num1);
            if (((int)ply_work.obj_work.dir.z + 8192 & 65280) > 16384)
                return;
            if (((int)ply_work.player_flag & 134217728) != 0)
                ply_work.player_flag &= 4160749567U;
            else if (((int)ply_work.player_flag & 32768) != 0)
            {
                if (((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.seq_state == 1)
                {
                    int num2 = ply_work.scroll_spd_x - 4096;
                    if (num2 < 0)
                        num2 = 0;
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                    if (ply_work.obj_work.spd_m >= num2)
                        return;
                    ply_work.obj_work.spd_m = num2;
                }
                else
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
            }
            else
                ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
        }
    }

    private static void GmPlySeqMoveWalkTruck(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.gmk_flag & 262144) != 0)
            return;
        int spdAdd = ply_work.spd_add;
        int fSpd = ply_work.spd_dec;
        int num1 = ply_work.spd_max;
        ushort num2 = (ushort)((uint)ply_work.obj_work.dir.z +
                                ((uint)ply_work.obj_work.dir_fall - (uint)AppMain.g_gm_main_system.pseudofall_dir));
        if (num2 != (ushort)0)
        {
            int num3 = AppMain.FX_Mul(ply_work.spd_max_add_slope, AppMain.mtMathSin((int)num2));
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
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) > ply_work.spd3)
            {
                int num3;
                if (num1 - ply_work.spd3 != 0)
                {
                    num3 = AppMain.FX_Div(AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) - ply_work.spd3,
                        num1 - ply_work.spd3);
                    if (num3 > 4096)
                        num3 = 4096;
                }
                else
                    num3 = 4096;

                v2 = num3 * 3968 >> 12;
            }

            spdAdd -= AppMain.FX_Mul(spdAdd, v2);
        }

        if (((int)ply_work.player_flag & 67108864) != 0)
        {
            AppMain.GMD_PLAYER_WATER_SET(ref spdAdd);
            AppMain.GMD_PLAYER_WATER_SET(ref fSpd);
        }

        if (ply_work.spd_work_max >= num1 && AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) >= num1)
        {
            if (ply_work.spd_work_max > ply_work.obj_work.spd_m)
                ply_work.spd_work_max = AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
            num1 = ply_work.spd_work_max;
        }

        if ((((int)AppMain.g_gm_main_system.game_flag & 1048576) != 0 ||
             ((int)ply_work.player_flag & 16777216) != 0) && AppMain.GmPlayerKeyCheckWalkLeft(ply_work) |
            AppMain.GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdUpSet(ply_work.obj_work.spd_m, spdAdd, num1);
            }
            else
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdUpSet(ply_work.obj_work.spd_m, -spdAdd, num1);
            }
        }
        else
        {
            if (((int)num2 + (int)ply_work.obj_work.dir_slope & (int)ushort.MaxValue) >=
                (int)ply_work.obj_work.dir_slope << 1)
                return;
            ply_work.spd_pool = (short)0;
            ply_work.obj_work.spd.x = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd.x, -num1, num1);
            ply_work.obj_work.spd_m = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd_m, -num1, num1);
            if (((int)num2 + 2048 & 65280) > 4096)
                return;
            if (((int)ply_work.player_flag & 134217728) != 0)
                ply_work.player_flag &= 4160749567U;
            else
                ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
        }
    }

    private static void GmPlySeqMoveWalkAutoRun(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int spdAdd = ply_work.spd_add;
        int fSpd = ply_work.spd_dec;
        int spdMax = ply_work.spd_max;
        int num1 = AppMain.FX_F32_TO_FX32(9.5f);
        if (AppMain.GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (ply_work.obj_work.spd_m <= ply_work.spd3)
                spdAdd >>= 2;
            if (ply_work.obj_work.spd_m < ply_work.scroll_spd_x + AppMain.FX_F32_TO_FX32(0.25f))
                ply_work.obj_work.spd_m = ply_work.scroll_spd_x + AppMain.FX_F32_TO_FX32(0.25f);
            if (ply_work.obj_work.spd_m < AppMain.FX_F32_TO_FX32(8.4f))
                ply_work.obj_work.spd_m = AppMain.FX_F32_TO_FX32(8.4f);
            if (ply_work.obj_work.spd_m > AppMain.FX_F32_TO_FX32(8.7f))
                ply_work.obj_work.spd_m = AppMain.FX_F32_TO_FX32(8.7f);
        }

        if (AppMain.GmPlayerKeyCheckWalkRight(ply_work) || AppMain.GmPlayerKeyCheckWalkLeft(ply_work))
        {
            int num2 = AppMain.MTM_MATH_ABS(ply_work.key_walk_rot_z);
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
        if (ply_work.obj_work.dir.z != (ushort)0)
        {
            int num2 = AppMain.FX_Mul(ply_work.spd_max_add_slope, AppMain.mtMathSin((int)ply_work.obj_work.dir.z));
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
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) > ply_work.spd3)
            {
                int num2;
                if (num1 - ply_work.spd3 != 0)
                {
                    num2 = AppMain.FX_Div(AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) - ply_work.spd3,
                        num1 - ply_work.spd3);
                    if (num2 > 4096)
                        num2 = 4096;
                }
                else
                    num2 = 4096;

                v2 = num2 * 3968 >> 12;
            }

            spdAdd -= AppMain.FX_Mul(spdAdd, v2);
        }

        if (((int)ply_work.player_flag & 67108864) != 0)
        {
            AppMain.GMD_PLAYER_WATER_SET(ref spdAdd);
            AppMain.GMD_PLAYER_WATER_SET(ref fSpd);
        }

        if (ply_work.spd_work_max >= num1 && AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) >= num1)
        {
            if (ply_work.spd_work_max > ply_work.obj_work.spd_m)
                ply_work.spd_work_max = AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
            num1 = ply_work.spd_work_max;
        }

        if (((int)ply_work.player_flag & 32768) != 0)
        {
            ply_work.spd_work_max += 8192;
            num1 = ply_work.spd_work_max + 8192;
        }

        if (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) | AppMain.GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdUpSet(ply_work.obj_work.spd_m, spdAdd, num1);
            }
            else
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdUpSet(ply_work.obj_work.spd_m, -spdAdd, num1);
            }
        }
        else
        {
            ply_work.spd_pool = (short)0;
            ply_work.obj_work.spd.x = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd.x, -num1, num1);
            ply_work.obj_work.spd_m = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd_m, -num1, num1);
            if (((int)ply_work.obj_work.dir.z + 8192 & 65280) > 16384)
                return;
            if (((int)ply_work.player_flag & 134217728) != 0)
                ply_work.player_flag &= 4160749567U;
            else if (((int)ply_work.player_flag & 32768) != 0)
            {
                if (((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.seq_state == 1)
                {
                    int num2 = ply_work.scroll_spd_x - 4096;
                    if (num2 < 0)
                        num2 = 0;
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
                    if (ply_work.obj_work.spd_m >= num2)
                        return;
                    ply_work.obj_work.spd_m = num2;
                }
                else
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
            }
            else
                ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, fSpd);
        }
    }

    private static void GmPlySeqMoveJump(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int spdJumpAdd = ply_work.spd_jump_add;
        int fSpd1 = ply_work.spd_jump_dec;
        int spdJumpDec = ply_work.spd_jump_dec;
        int spdJumpMax = ply_work.spd_jump_max;
        ply_work.spd_work_max = 0;
        if (((int)ply_work.obj_work.dir.z + 8192 & 49152) != 0 || ply_work.obj_work.dir.z == (ushort)57344)
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
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) > ply_work.spd2)
            {
                int num;
                if (spdJumpMax - ply_work.spd2 != 0)
                {
                    num = AppMain.FX_Div(AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) - ply_work.spd2,
                        spdJumpMax - ply_work.spd2);
                    if (num > 4096)
                        num = 4096;
                }
                else
                    num = 4096;

                v2 = num * 3968 >> 12;
            }

            fSpd2 = spdJumpAdd - AppMain.FX_Mul(spdJumpAdd, v2);
        }

        if (((int)ply_work.player_flag & 67108864) != 0)
        {
            AppMain.GMD_PLAYER_WATER_SET(ref fSpd2);
            AppMain.GMD_PLAYER_WATER_SET(ref fSpd1);
        }

        int sSpd = AppMain.FX_Mul(fSpd1, 4096);
        AppMain.FX_Mul(spdJumpDec, 4096);
        if (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) | AppMain.GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd.x < 0)
                    ply_work.obj_work.spd.x = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.x, sSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = AppMain.ObjSpdUpSet(ply_work.obj_work.spd.x, fSpd2, spdJumpMax);
            }
            else
            {
                if (ply_work.obj_work.spd.x > 0)
                    ply_work.obj_work.spd.x = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.x, sSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = AppMain.ObjSpdUpSet(ply_work.obj_work.spd.x, -fSpd2, spdJumpMax);
            }
        }
        else
        {
            ply_work.obj_work.spd.x = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd.x, -spdJumpMax, spdJumpMax);
            ply_work.obj_work.spd_m = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd_m, -spdJumpMax, spdJumpMax);
            ply_work.spd_pool = (short)0;
            ply_work.obj_work.spd.x = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.x, fSpd1);
            ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
        }
    }

    private static void GmPlySeqMoveJumpTruck(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int spdJumpAdd = ply_work.spd_jump_add;
        int fSpd1 = ply_work.spd_jump_dec;
        int spdJumpDec = ply_work.spd_jump_dec;
        int spdJumpMax = ply_work.spd_jump_max;
        ply_work.spd_work_max = 0;
        if (((int)ply_work.obj_work.dir.z + 8192 & 49152) != 0 || ply_work.obj_work.dir.z == (ushort)57344)
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
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) > ply_work.spd2)
            {
                int num;
                if (spdJumpMax - ply_work.spd2 != 0)
                {
                    num = AppMain.FX_Div(AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) - ply_work.spd2,
                        spdJumpMax - ply_work.spd2);
                    if (num > 4096)
                        num = 4096;
                }
                else
                    num = 4096;

                v2 = num * 3968 >> 12;
            }

            fSpd2 = spdJumpAdd - AppMain.FX_Mul(spdJumpAdd, v2);
        }

        if (((int)ply_work.player_flag & 67108864) != 0)
        {
            AppMain.GMD_PLAYER_WATER_SET(ref fSpd2);
            AppMain.GMD_PLAYER_WATER_SET(ref fSpd1);
        }

        int num1 = AppMain.FX_Mul(fSpd1, 4096);
        int v1 = AppMain.FX_Mul(spdJumpDec, 4096);
        int num2 = 12288;
        if (((int)ply_work.gmk_flag2 & 512) != 0)
        {
            ushort num3 = (ushort)((uint)ply_work.obj_work.dir_fall - (uint)AppMain.g_gm_main_system.pseudofall_dir);
            ply_work.obj_work.spd.x += AppMain.mtMathSin((int)num3) / 3;
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
        else if (((int)ply_work.gmk_flag2 & 1) != 0)
        {
            int sSpd1 = AppMain.FX_Mul(num1, 3072);
            int sSpd2 = AppMain.FX_Mul(v1, 3072);
            ply_work.obj_work.spd.x = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd.x, -spdJumpMax, spdJumpMax);
            ply_work.obj_work.spd_m = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd_m, -spdJumpMax, spdJumpMax);
            ply_work.spd_pool = (short)0;
            ply_work.obj_work.spd.x = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.x, sSpd1);
            ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd2);
        }
        else if (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) | AppMain.GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd.x < 0)
                    ply_work.obj_work.spd.x = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.x, num1);
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = AppMain.ObjSpdUpSet(ply_work.obj_work.spd.x, fSpd2, spdJumpMax);
            }
            else
            {
                if (ply_work.obj_work.spd.x > 0)
                    ply_work.obj_work.spd.x = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.x, num1);
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = AppMain.ObjSpdUpSet(ply_work.obj_work.spd.x, -fSpd2, spdJumpMax);
            }
        }
        else
        {
            ply_work.obj_work.spd.x = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd.x, -spdJumpMax, spdJumpMax);
            ply_work.obj_work.spd_m = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd_m, -spdJumpMax, spdJumpMax);
            ply_work.spd_pool = (short)0;
            ply_work.obj_work.spd.x = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.x, fSpd1);
            ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
        }
    }

    private static void GmPlySeqMoveJumpAutoRun(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int v1 = ply_work.spd_jump_add;
        int fSpd1 = ply_work.spd_jump_dec;
        int spdJumpDec = ply_work.spd_jump_dec;
        int spdJumpMax = ply_work.spd_jump_max;
        ply_work.spd_work_max = 0;
        if (AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            v1 = 0;
        if (((int)ply_work.obj_work.dir.z + 8192 & 49152) != 0 || ply_work.obj_work.dir.z == (ushort)57344)
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
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) > ply_work.spd2)
            {
                int num;
                if (spdJumpMax - ply_work.spd2 != 0)
                {
                    num = AppMain.FX_Div(AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) - ply_work.spd2,
                        spdJumpMax - ply_work.spd2);
                    if (num > 4096)
                        num = 4096;
                }
                else
                    num = 4096;

                v2 = num * 3968 >> 12;
            }

            fSpd2 = v1 - AppMain.FX_Mul(v1, v2);
        }

        if (((int)ply_work.player_flag & 67108864) != 0)
        {
            AppMain.GMD_PLAYER_WATER_SET(ref fSpd2);
            AppMain.GMD_PLAYER_WATER_SET(ref fSpd1);
        }

        int sSpd = AppMain.FX_Mul(fSpd1, 4096);
        AppMain.FX_Mul(spdJumpDec, 4096);
        if (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) | AppMain.GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd.x < 0)
                    ply_work.obj_work.spd.x = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.x, sSpd);
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = AppMain.ObjSpdUpSet(ply_work.obj_work.spd.x, fSpd2, spdJumpMax);
            }
            else
            {
                if (ply_work.obj_work.spd.x > 0)
                    ply_work.obj_work.spd.x = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.x, sSpd);
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
                ply_work.obj_work.spd.x = AppMain.ObjSpdUpSet(ply_work.obj_work.spd.x, -fSpd2, spdJumpMax);
            }
        }
        else
        {
            ply_work.obj_work.spd.x = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd.x, -spdJumpMax, spdJumpMax);
            ply_work.obj_work.spd_m = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd_m, -spdJumpMax, spdJumpMax);
            ply_work.spd_pool = (short)0;
            ply_work.obj_work.spd.x = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.x, fSpd1);
            ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, spdJumpDec);
        }
    }

    private static void GmPlySeqMoveSpin(AppMain.GMS_PLAYER_WORK ply_work)
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
                ? AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_slope_spin_spipe
                : AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_slope_spin;
            ply_work.obj_work.dir_slope = (ushort)640;
        }

        if (ply_work.obj_work.spd_m > 0 && ((int)ply_work.key_on & 8) != 0 ||
            ply_work.obj_work.spd_m < 0 && ((int)ply_work.key_on & 4) != 0)
            ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd >> 1);
        else if ((ply_work.obj_work.spd_m <= 0 || ((int)ply_work.key_on & 8) == 0) &&
                 (ply_work.obj_work.spd_m >= 0 || ((int)ply_work.key_on & 4) == 0))
            ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd << 1);
        else
            ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
    }

    private static void GmPlySeqMoveSpinNoDec(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int spdDecSpin = ply_work.spd_dec_spin;
        if (ply_work.no_spddown_timer != 0)
        {
            ply_work.obj_work.spd_slope = 0;
        }
        else
        {
            ply_work.obj_work.spd_slope = ply_work.seq_state == 37
                ? AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_slope_spin_spipe
                : AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_slope_spin;
            ply_work.obj_work.dir_slope = (ushort)640;
        }
    }

    private static void GmPlySeqMoveSpinPinball(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.spd_slope = AppMain.g_gm_player_parameter[(int)ply_work.char_id].spd_slope_spin_pinball;
        ply_work.obj_work.dir_slope = (ushort)256;
        int spdAddSpinPinball = ply_work.spd_add_spin_pinball;
        int sSpd = ply_work.spd_dec_spin_pinball;
        int spdMaxSpinPinball = ply_work.spd_max_spin_pinball;
        int num1 = AppMain.MTM_MATH_ABS(ply_work.key_walk_rot_z);
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
        if (ply_work.obj_work.dir.z != (ushort)0)
        {
            int num3 = AppMain.FX_Mul(ply_work.spd_max_add_slope_spin_pinball,
                AppMain.mtMathSin((int)ply_work.obj_work.dir.z));
            if (num3 > 0)
                num2 += num3;
        }

        if (ply_work.no_spddown_timer != 0)
            sSpd = 0;
        if (ply_work.spd_work_max >= num2 && AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) >= num2)
        {
            if (ply_work.spd_work_max > ply_work.obj_work.spd_m)
                ply_work.spd_work_max = AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
            num2 = ply_work.spd_work_max;
        }

        if (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) | AppMain.GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdUpSet(ply_work.obj_work.spd_m, spdAddSpinPinball, num2);
            }
            else
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdUpSet(ply_work.obj_work.spd_m, -spdAddSpinPinball, num2);
            }
        }
        else
        {
            ply_work.spd_pool = (short)0;
            ply_work.obj_work.spd.x = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd.x, -num2, num2);
            ply_work.obj_work.spd_m = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd_m, -num2, num2);
            if (((int)ply_work.obj_work.dir.z + 8192 & 65280) > 16384)
                return;
            if (((int)ply_work.player_flag & 134217728) != 0)
                ply_work.player_flag &= 4160749567U;
            else
                ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
        }
    }

    private static void gmPlySeqTruckMove(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = (AppMain.GMS_PLAYER_WORK)obj_work;
        ushort num4 = (ushort)((uint)obj_work.dir.z +
                                ((uint)obj_work.dir_fall - (uint)AppMain.g_gm_main_system.pseudofall_dir));
        obj_work.prev_pos.x = obj_work.pos.x;
        obj_work.prev_pos.y = obj_work.pos.y;
        obj_work.prev_pos.z = obj_work.pos.z;
        if (((int)obj_work.move_flag & 134217728) != 0)
        {
            obj_work.flow.x = 0;
            obj_work.flow.y = 0;
            obj_work.flow.z = 0;
        }

        int x = obj_work.flow.x;
        int y = obj_work.flow.y;
        if ((x != 0 || y != 0) && obj_work.dir_fall != (ushort)0)
            AppMain.ObjObjectSpdDirFall(ref x, ref y, obj_work.dir_fall);
        if (obj_work.hitstop_timer != 0)
        {
            obj_work.move.x = AppMain.FX_Mul(x, AppMain.g_obj.speed);
            obj_work.move.y = AppMain.FX_Mul(y, AppMain.g_obj.speed);
            obj_work.move.z = AppMain.FX_Mul(obj_work.flow.z, AppMain.g_obj.speed);
        }
        else
        {
            if (((int)obj_work.move_flag & 1) == 0)
            {
                if (((int)obj_work.move_flag & 128) != 0 && ((int)obj_work.move_flag & 1) == 0)
                    obj_work.spd.y += AppMain.FX_Mul(obj_work.spd_fall, AppMain.g_obj.speed);
                if (((int)obj_work.move_flag & 128) != 0 && obj_work.spd.y > obj_work.spd_fall_max)
                    obj_work.spd.y = obj_work.spd_fall_max;
            }

            if (((int)obj_work.move_flag & 64) != 0)
            {
                if (((int)obj_work.move_flag & 131072) != 0 &&
                    (obj_work.spd_m != 0 || ((int)obj_work.move_flag & 262144) == 0) &&
                    ((int)num4 + (int)obj_work.dir_slope & (int)ushort.MaxValue) >= (int)obj_work.dir_slope << 1)
                {
                    int v1 = AppMain.MTM_MATH_ABS(obj_work.spd_m) >= 16384
                        ? obj_work.spd_slope << 1
                        : obj_work.spd_slope;
                    if (obj_work.spd_m > 0 && num4 > (ushort)32768 || obj_work.spd_m < 0 && num4 < (ushort)32768)
                        v1 <<= 1;
                    int sSpd = AppMain.FX_Mul(v1, AppMain.mtMathSin((int)num4));
                    if (sSpd > 0 || num4 < (ushort)32768)
                    {
                        if (sSpd < 256)
                            sSpd = 256;
                    }
                    else if (sSpd > -256)
                        sSpd = -256;

                    obj_work.spd_m = AppMain.ObjSpdUpSet(obj_work.spd_m, sSpd, obj_work.spd_slope_max);
                }

                if (((int)obj_work.move_flag & 32768) == 0)
                {
                    num1 = AppMain.FX_Mul(obj_work.spd_m, AppMain.mtMathCos((int)obj_work.dir.z));
                    num2 = AppMain.FX_Mul(obj_work.spd_m, AppMain.mtMathSin((int)obj_work.dir.z));
                }
            }

            if (((int)obj_work.move_flag & 67108864) != 0)
            {
                obj_work.move.x = AppMain.FX_Mul(obj_work.spd.x + num1 + x, AppMain.g_obj.speed);
                obj_work.move.y = AppMain.FX_Mul(obj_work.spd.y + num2 + y, AppMain.g_obj.speed);
            }
            else
            {
                obj_work.move.x = AppMain.FX_Mul(obj_work.spd.x + num1 + x + AppMain.g_obj.scroll[0],
                    AppMain.g_obj.speed);
                obj_work.move.y = AppMain.FX_Mul(obj_work.spd.y + num2 + y + AppMain.g_obj.scroll[1],
                    AppMain.g_obj.speed);
            }

            obj_work.move.z = AppMain.FX_Mul(obj_work.spd.z + num3 + obj_work.flow.z, AppMain.g_obj.speed);
            if (((int)obj_work.move_flag & 1) != 0)
                AppMain.ObjObjectSpdDirFall(ref obj_work.move.x, ref obj_work.move.y, obj_work.dir_fall);
            else
                AppMain.ObjObjectSpdDirFall(ref obj_work.move.x, ref obj_work.move.y,
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

    private static void gmPlySeqSplMove(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int num1 = 0;
        int num2 = 0;
        int num3 = 0;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = (AppMain.GMS_PLAYER_WORK)obj_work;
        ushort num4 = (ushort)((uint)obj_work.dir.z +
                                ((uint)obj_work.dir_fall - (uint)AppMain.g_gm_main_system.pseudofall_dir));
        obj_work.prev_pos.x = obj_work.pos.x;
        obj_work.prev_pos.y = obj_work.pos.y;
        obj_work.prev_pos.z = obj_work.pos.z;
        if (((int)obj_work.move_flag & 134217728) != 0)
        {
            obj_work.flow.x = 0;
            obj_work.flow.y = 0;
            obj_work.flow.z = 0;
        }

        int x = obj_work.flow.x;
        int y = obj_work.flow.y;
        if ((x != 0 || y != 0) && obj_work.dir_fall != (ushort)0)
            AppMain.ObjObjectSpdDirFall(ref x, ref y, obj_work.dir_fall);
        if (obj_work.hitstop_timer != 0)
        {
            obj_work.move.x = AppMain.FX_Mul(x, AppMain.g_obj.speed);
            obj_work.move.y = AppMain.FX_Mul(y, AppMain.g_obj.speed);
            obj_work.move.z = AppMain.FX_Mul(obj_work.flow.z, AppMain.g_obj.speed);
        }
        else
        {
            if (((int)obj_work.move_flag & 1) == 0)
            {
                if (((int)obj_work.move_flag & 128) != 0 && ((int)obj_work.move_flag & 1) == 0)
                    obj_work.spd.y += AppMain.FX_Mul(obj_work.spd_fall, AppMain.g_obj.speed);
                if (((int)obj_work.move_flag & 128) != 0 && obj_work.spd.y > obj_work.spd_fall_max)
                    obj_work.spd.y = obj_work.spd_fall_max;
            }

            if (((int)obj_work.move_flag & 64) != 0)
            {
                if (((int)obj_work.move_flag & 131072) != 0 &&
                    (obj_work.spd_m != 0 || ((int)obj_work.move_flag & 262144) == 0))
                    obj_work.spd_m =
                        ((int)num4 + (int)obj_work.dir_slope & (int)ushort.MaxValue) <
                        (int)obj_work.dir_slope << 1 || ((int)obj_work.move_flag & 1) == 0
                            ? 0
                            : (AppMain.MTM_MATH_ABS(obj_work.spd_m) >= 8192
                                ? (AppMain.MTM_MATH_ABS(obj_work.spd_m) >= 16384
                                    ? AppMain.ObjSpdUpSet(obj_work.spd_m,
                                        AppMain.FX_Mul(obj_work.spd_slope << 1, AppMain.mtMathSin((int)num4)),
                                        obj_work.spd_slope_max)
                                    : AppMain.ObjSpdUpSet(obj_work.spd_m,
                                        AppMain.FX_Mul(obj_work.spd_slope, AppMain.mtMathSin((int)num4)),
                                        obj_work.spd_slope_max))
                                : AppMain.ObjSpdUpSet(obj_work.spd_m,
                                    AppMain.FX_Mul(obj_work.spd_slope >> 1, AppMain.mtMathSin((int)num4)),
                                    obj_work.spd_slope_max));
                if (((int)obj_work.move_flag & 32768) == 0)
                {
                    num1 = AppMain.FX_Mul(obj_work.spd_m, AppMain.mtMathCos((int)obj_work.dir.z));
                    num2 = AppMain.FX_Mul(obj_work.spd_m, AppMain.mtMathSin((int)obj_work.dir.z));
                }
            }

            if (((int)obj_work.move_flag & 67108864) != 0)
            {
                obj_work.move.x = AppMain.FX_Mul(obj_work.spd.x + num1 + x, AppMain.g_obj.speed);
                obj_work.move.y = AppMain.FX_Mul(obj_work.spd.y + num2 + y, AppMain.g_obj.speed);
            }
            else
            {
                obj_work.move.x = AppMain.FX_Mul(obj_work.spd.x + num1 + x + AppMain.g_obj.scroll[0],
                    AppMain.g_obj.speed);
                obj_work.move.y = AppMain.FX_Mul(obj_work.spd.y + num2 + y + AppMain.g_obj.scroll[1],
                    AppMain.g_obj.speed);
            }

            obj_work.move.z = AppMain.FX_Mul(obj_work.spd.z + num3 + obj_work.flow.z, AppMain.g_obj.speed);
            if (((int)obj_work.move_flag & 1) != 0)
                AppMain.ObjObjectSpdDirFall(ref obj_work.move.x, ref obj_work.move.y, obj_work.dir_fall);
            else
                AppMain.ObjObjectSpdDirFall(ref obj_work.move.x, ref obj_work.move.y,
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

    private static void gmPlySeqSplJumpDirec(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.dir.z = AppMain.ObjRoopMove16(ply_work.obj_work.dir.z,
            (ushort)((uint)ply_work.jump_pseudofall_dir - (uint)ply_work.obj_work.dir_fall), (short)512);
        if (((int)ply_work.gmk_flag & 536875264) != 0)
            return;
        ply_work.obj_work.pos.z = (int)((long)AppMain.ObjSpdDownSet(ply_work.obj_work.pos.z, 16384) & 4294963200L);
        ply_work.obj_work.spd.z = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.z, 512);
        ply_work.obj_work.dir.x = AppMain.ObjRoopMove16(ply_work.obj_work.dir.x, (ushort)0, (short)1024);
    }

    private static void GmPlySeqJumpDirec(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.dir.z = AppMain.ObjRoopMove16(ply_work.obj_work.dir.z, (ushort)0, (short)512);
        if (((int)ply_work.gmk_flag & 536875264) != 0)
            return;
        ply_work.obj_work.pos.z = (int)((long)AppMain.ObjSpdDownSet(ply_work.obj_work.pos.z, 16384) & 4294963200L);
        ply_work.obj_work.spd.z = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.z, 512);
        ply_work.obj_work.dir.x = AppMain.ObjRoopMove16(ply_work.obj_work.dir.x, (ushort)0, (short)1024);
    }

    private static void GmPlySeqTruckJumpDirec(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.dir.z = AppMain.ObjRoopMove16(ply_work.obj_work.dir.z,
            (ushort)((uint)ply_work.jump_pseudofall_dir - (uint)ply_work.obj_work.dir_fall), (short)512);
        if (((int)ply_work.gmk_flag & 536875264) != 0)
            return;
        ply_work.obj_work.pos.z = (int)((long)AppMain.ObjSpdDownSet(ply_work.obj_work.pos.z, 16384) & 4294963200L);
        ply_work.obj_work.spd.z = AppMain.ObjSpdDownSet(ply_work.obj_work.spd.z, 512);
        ply_work.obj_work.dir.x = AppMain.ObjRoopMove16(ply_work.obj_work.dir.x, (ushort)0, (short)1024);
    }

    private static bool GmPlySeqCheckAcceptHoming(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ((int)ply_work.seq_state_data_tbl[ply_work.seq_state].accept_attr & 16) != 0 &&
               ((int)ply_work.player_flag & 128) == 0;
    }

    private static void gmPlySeqCheckChangeSequence(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        if (AppMain.GmPlayerIsTransformSuperSonic(ply_work) && AppMain.GmPlayerKeyCheckTransformKeyPush(ply_work) &&
            (0 <= ply_work.seq_state && ply_work.seq_state <= 21))
            AppMain.GmPlySeqChangeTransformSuper(ply_work);
        AppMain.gmPlySeqCheckChangeSequenceUserInput(ply_work);
    }

    private static bool gmPlySeqCheckChangeSequenceUserInput(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        AppMain.GMS_PLY_SEQ_STATE_DATA[] seqStateDataTbl = ply_work.seq_state_data_tbl;
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 4) != 0)
        {
            if (((int)ply_work.player_flag & 262144) != 0)
                AppMain.gmPlySeqCheckEndTruckWalk(ply_work);
            else
                AppMain.gmPlySeqCheckEndWalk(ply_work);
        }

        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 1) != 0)
            AppMain.gmPlySeqCheckTurn(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 2) != 0)
            AppMain.gmPlySeqCheckDirectTurn(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 8) != 0)
            AppMain.gmPlySeqCheckFall(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 32768) != 0)
            AppMain.gmPlySeqCheckStagger(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 16) != 0)
            AppMain.gmPlySeqCheckEndLookup(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 32) != 0)
            AppMain.gmPlySeqCheckEndSquat(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 128) != 0)
            AppMain.gmPlySeqCheckEndSpin(ply_work);
        if (((int)seqStateDataTbl[ply_work.seq_state].check_attr & 1024) != 0)
            AppMain.gmPlySeqCheckEndWallPush(ply_work);
        if (AppMain.GmPlySeqCheckAcceptHoming(ply_work))
        {
            if (ply_work.cursol_enemy_obj == null)
            {
                AppMain.GmPlyEfctCreateHomingCursol(ply_work);
                ply_work.cursol_enemy_obj = ply_work.enemy_obj;
            }

            if (AppMain.gmPlySeqCheckHoming(ply_work))
                return true;
        }

        if (
            ((int)seqStateDataTbl[ply_work.seq_state].accept_attr & 4096) != 0 &&
            AppMain.gmPlySeqCheckSquatSpin(ply_work) ||
            ((int)seqStateDataTbl[ply_work.seq_state].accept_attr & 256) != 0 && AppMain.gmPlySeqCheckSpin(ply_work) ||
            (((int)seqStateDataTbl[ply_work.seq_state].accept_attr & 512) != 0 &&
             AppMain.gmPlySeqCheckSpinDashAcc(ply_work) ||
             ((int)seqStateDataTbl[ply_work.seq_state].accept_attr & 2048) != 0 &&
             AppMain.gmPlySeqCheckPinballSpinDashAcc(ply_work)) ||
            (((int)seqStateDataTbl[ply_work.seq_state].accept_attr & 8) != 0 && AppMain.gmPlySeqCheckJump(ply_work) ||
             ((int)seqStateDataTbl[ply_work.seq_state].accept_attr & 4) != 0 && AppMain.gmPlySeqCheckBrake(ply_work) ||
             ((int)seqStateDataTbl[ply_work.seq_state].accept_attr & 1024) != 0 &&
             AppMain.gmPlySeqCheckWallPush(ply_work)))
            return true;
        if (((int)seqStateDataTbl[ply_work.seq_state].accept_attr & 2) != 0)
        {
            if (((int)ply_work.player_flag & 262144) != 0)
            {
                if (AppMain.gmPlySeqCheckTruckWalk(ply_work))
                    return true;
            }
            else if (AppMain.gmPlySeqCheckWalk(ply_work))
                return true;
        }

        return ((int)seqStateDataTbl[ply_work.seq_state].accept_attr & 64) != 0 &&
               AppMain.gmPlySeqCheckLookup(ply_work) ||
               ((int)seqStateDataTbl[ply_work.seq_state].accept_attr & 128) != 0 &&
               AppMain.gmPlySeqCheckSquat(ply_work);
    }

    private static bool gmPlySeqCheckEndWalk(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m != 0 || ply_work.obj_work.spd.z != 0)
            return false;
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
        return true;
    }

    private static bool gmPlySeqCheckTurn(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.seq_state == 2)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) == 0 && AppMain.GmPlayerKeyCheckWalkRight(ply_work) ||
                ((int)ply_work.obj_work.disp_flag & 1) != 0 && AppMain.GmPlayerKeyCheckWalkLeft(ply_work))
                return AppMain.GmPlySeqChangeSequence(ply_work, 2);
        }
        else if ((((int)ply_work.obj_work.disp_flag & 1) != 0 && AppMain.GmPlayerKeyCheckWalkRight(ply_work) ||
                  ((int)ply_work.obj_work.disp_flag & 1) == 0 && AppMain.GmPlayerKeyCheckWalkLeft(ply_work)) &&
                 AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 16384)
            return AppMain.GmPlySeqChangeSequence(ply_work, 2);

        return false;
    }

    private static bool gmPlySeqCheckDirectTurn(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if ((((int)ply_work.obj_work.disp_flag & 1) == 0 || !AppMain.GmPlayerKeyCheckWalkRight(ply_work)) &&
            (((int)ply_work.obj_work.disp_flag & 1) != 0 || !AppMain.GmPlayerKeyCheckWalkLeft(ply_work)) ||
            ((int)ply_work.obj_work.move_flag & 16) == 0 &&
            (((int)ply_work.obj_work.move_flag & 16) != 0 || AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) >= 16384))
            return false;
        if (ply_work.act_state == 40 || ply_work.act_state == 48 ||
            (ply_work.act_state == 41 || ply_work.act_state == 42) || ply_work.act_state == 43)
            AppMain.GmPlySeqSetFallTurn(ply_work);
        else
            AppMain.GmPlySeqSetProgramTurn(ply_work, (ushort)4096);
        return true;
    }

    private static bool gmPlySeqCheckFall(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.move_flag & 1) == 0)
            return AppMain.GmPlySeqChangeSequence(ply_work, 16);
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            if (((int)ply_work.gmk_flag & 262144) != 0)
            {
                if (((int)ply_work.gmk_flag & 1073741824) != 0)
                {
                    if (ply_work.fall_timer != 0)
                    {
                        ply_work.fall_timer = AppMain.ObjTimeCountDown(ply_work.fall_timer);
                        return false;
                    }

                    ply_work.gmk_flag &= 3220963327U;
                    AppMain.GmPlayerSpdParameterSet(ply_work);
                    ply_work.jump_pseudofall_dir = AppMain.g_gm_main_system.pseudofall_dir;
                    ply_work.obj_work.pos.x = AppMain.FXM_FLOAT_TO_FX32(ply_work.truck_mtx_ply_mtn_pos.M03);
                    ply_work.obj_work.pos.y = AppMain.FXM_FLOAT_TO_FX32(-ply_work.truck_mtx_ply_mtn_pos.M13);
                    ply_work.obj_work.pos.z = AppMain.FXM_FLOAT_TO_FX32(ply_work.truck_mtx_ply_mtn_pos.M23);
                    AppMain.GmPlySeqChangeDeath(ply_work);
                    ply_work.gmk_flag2 |= 64U;
                    return true;
                }
            }
            else
            {
                ushort num = (ushort)((uint)ply_work.obj_work.dir.z +
                                       ((uint)ply_work.obj_work.dir_fall -
                                        (uint)AppMain.g_gm_main_system.pseudofall_dir));
                if ((((int)num + 16384 & 32768) != 0 || num == (ushort)49152) &&
                    AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 8192)
                {
                    if (ply_work.fall_timer != 0)
                    {
                        ply_work.fall_timer = AppMain.ObjTimeCountDown(ply_work.fall_timer);
                        return false;
                    }

                    AppMain.GmPlayerSpdParameterSet(ply_work);
                    return AppMain.GmPlySeqChangeSequence(ply_work, 16);
                }

                AppMain.GmPlayerSpdParameterSet(ply_work);
            }
        }
        else if (ply_work.fall_timer != 0)
            ply_work.fall_timer = AppMain.ObjTimeCountDown(ply_work.fall_timer);
        else if ((((int)ply_work.obj_work.dir.z + 16384 & 32768) != 0 || ply_work.obj_work.dir.z == (ushort)49152) &&
                 AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 8192)
        {
            AppMain.GmPlayerSpdParameterSet(ply_work);
            return AppMain.GmPlySeqChangeSequence(ply_work, 16);
        }

        return false;
    }

    private static bool gmPlySeqCheckStagger(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.dir.z & (int)short.MaxValue) != 0 || ply_work.obj_work.ride_obj != null)
            return false;
        AppMain.OBS_COL_CHK_DATA pData = AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Alloc();
        pData.pos_x = ply_work.obj_work.pos.x >> 12;
        pData.pos_y = (ply_work.obj_work.pos.y >> 12) + (int)ply_work.obj_work.field_rect[3];
        pData.flag = (ushort)(ply_work.obj_work.flag & 1U);
        pData.vec = (ushort)2;
        pData.dir = (ushort[])null;
        pData.attr = (uint[])null;
        if (AppMain.ObjDiffCollision(pData) > 0)
        {
            pData.pos_x = (ply_work.obj_work.pos.x >> 12) + (int)ply_work.obj_work.field_rect[0] - 2;
            int num1 = AppMain.ObjDiffCollision(pData);
            pData.pos_x = (ply_work.obj_work.pos.x >> 12) + (int)ply_work.obj_work.field_rect[2] + 2;
            int num2 = AppMain.ObjDiffCollision(pData);
            if (num1 <= 0 && num2 >= 16)
            {
                pData.pos_x = (ply_work.obj_work.pos.x >> 12) + (int)ply_work.obj_work.field_rect[0] - -4;
                int num3 = AppMain.ObjDiffCollision(pData);
                if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                {
                    AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Release(pData);
                    return AppMain.GmPlySeqChangeSequence(ply_work, 14);
                }

                AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Release(pData);
                return num3 > 0
                    ? AppMain.GmPlySeqChangeSequence(ply_work, 15)
                    : AppMain.GmPlySeqChangeSequence(ply_work, 13);
            }

            if (num1 >= 16 && num2 <= 0)
            {
                pData.pos_x = (ply_work.obj_work.pos.x >> 12) + (int)ply_work.obj_work.field_rect[2] - 4;
                int num3 = AppMain.ObjDiffCollision(pData);
                if (((int)ply_work.obj_work.disp_flag & 1) != 0)
                {
                    AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Release(pData);
                    return num3 > 0
                        ? AppMain.GmPlySeqChangeSequence(ply_work, 15)
                        : AppMain.GmPlySeqChangeSequence(ply_work, 13);
                }

                AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Release(pData);
                return AppMain.GmPlySeqChangeSequence(ply_work, 14);
            }
        }

        AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Release(pData);
        return false;
    }

    private static bool gmPlySeqCheckEndLookup(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.move_flag & 1) == 0)
            return AppMain.GmPlySeqChangeSequence(ply_work, 16);
        return ((int)ply_work.key_on & 1) == 0 && AppMain.GmPlySeqChangeSequence(ply_work, 5);
    }

    private static bool gmPlySeqCheckEndSquat(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.move_flag & 1) == 0)
            return AppMain.GmPlySeqChangeSequence(ply_work, 16);
        return ((int)ply_work.key_on & 2) == 0 && AppMain.GmPlySeqChangeSequence(ply_work, 8);
    }

    private static bool gmPlySeqCheckEndSpin(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m >= 2048 || ply_work.obj_work.spd_m <= -2048)
            return false;
        ply_work.obj_work.spd_m = 0;
        AppMain.GmPlayerSpdParameterSet(ply_work);
        if (((int)ply_work.player_flag & 131072) != 0)
        {
            AppMain.GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
        }

        return AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static bool gmPlySeqCheckEndWallPush(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return (((int)ply_work.obj_work.move_flag & 4) == 0 ||
                ((int)ply_work.obj_work.disp_flag & 1) != 0 && !AppMain.GmPlayerKeyCheckWalkLeft(ply_work) ||
                ((int)ply_work.obj_work.disp_flag & 1) == 0 && !AppMain.GmPlayerKeyCheckWalkRight(ply_work)) &&
               AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static bool gmPlySeqCheckHoming(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (!AppMain.GmPlayerKeyCheckJumpKeyPush(ply_work) || ply_work.homing_timer != 0 ||
            (((int)ply_work.player_flag & 128) != 0 || AppMain.GMM_MAIN_STAGE_IS_ENDING()))
            return false;
        return ply_work.enemy_obj != null
            ? AppMain.GmPlySeqChangeSequence(ply_work, 19)
            : AppMain.GmPlySeqChangeSequence(ply_work, 21);
    }

    private static bool gmPlySeqCheckSquatSpin(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ((int)ply_work.key_on & 2) != 0 &&
               (ply_work.obj_work.spd_m > 2048 || ply_work.obj_work.spd_m < -2048) &&
               AppMain.GmPlySeqChangeSequence(ply_work, 10);
    }

    private static bool gmPlySeqCheckSpin(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return (ply_work.obj_work.spd_m > 2048 || ply_work.obj_work.spd_m < -2048) &&
               AppMain.GmPlySeqChangeSequence(ply_work, 10);
    }

    private static bool gmPlySeqCheckSpinDashAcc(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return AppMain.GmPlayerKeyCheckJumpKeyPush(ply_work) && AppMain.GmPlySeqChangeSequence(ply_work, 11);
    }

    private static bool gmPlySeqCheckPinballSpinDashAcc(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ((int)ply_work.key_on & 2) != 0 && AppMain.GmPlayerKeyCheckJumpKeyPush(ply_work) &&
               AppMain.GmPlySeqChangeSequence(ply_work, 11);
    }

    private static bool gmPlySeqCheckJump(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return AppMain.GmPlayerKeyCheckJumpKeyPush(ply_work) &&
               (((int)ply_work.obj_work.move_flag & 1) != 0 ||
                ply_work.gmk_obj != null && ((int)ply_work.gmk_flag & 16384) != 0) &&
               AppMain.GmPlySeqChangeSequence(ply_work, 17);
    }

    private static bool gmPlySeqCheckBrake(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ply_work.seq_state != 9 &&
               (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) && ply_work.obj_work.spd_m >= 16384 ||
                AppMain.GmPlayerKeyCheckWalkRight(ply_work) && ply_work.obj_work.spd_m <= -16384) &&
               AppMain.GmPlySeqChangeSequence(ply_work, 9);
    }

    private static bool gmPlySeqCheckWalk(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return
            (!AppMain.GmObjCheckMapLeftLimit(ply_work.obj_work, 14) || !AppMain.GmPlayerKeyCheckWalkLeft(ply_work)) &&
            (!AppMain.GmObjCheckMapRightLimit(ply_work.obj_work, 14) || !AppMain.GmPlayerKeyCheckWalkRight(ply_work)) &&
            (13 > ply_work.seq_state || ply_work.seq_state > 15 || ((int)ply_work.obj_work.move_flag & 4) == 0 ||
             (((int)ply_work.obj_work.disp_flag & 1) == 0 || !AppMain.GmPlayerKeyCheckWalkLeft(ply_work)) &&
             (((int)ply_work.obj_work.disp_flag & 1) != 0 || !AppMain.GmPlayerKeyCheckWalkRight(ply_work))) &&
            (ply_work.obj_work.spd_m != 0 || AppMain.GmPlayerKeyCheckWalkLeft(ply_work) ||
             AppMain.GmPlayerKeyCheckWalkRight(ply_work)) && AppMain.GmPlySeqChangeSequence(ply_work, 1);
    }

    private static bool gmPlySeqCheckLookup(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ply_work.obj_work.spd_m == 0 && ((int)ply_work.obj_work.move_flag & 1) != 0 &&
               ((int)ply_work.key_on & 1) != 0 && AppMain.GmPlySeqChangeSequence(ply_work, 3);
    }

    private static bool gmPlySeqCheckSquat(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ((int)ply_work.key_on & 2) != 0 && AppMain.GmPlySeqChangeSequence(ply_work, 7);
    }

    private static bool gmPlySeqCheckWallPush(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ((int)ply_work.obj_work.move_flag & 4) != 0 && ((int)ply_work.player_flag & 32768) == 0 &&
               (((int)ply_work.obj_work.disp_flag & 1) != 0 && AppMain.GmPlayerKeyCheckWalkLeft(ply_work) ||
                ((int)ply_work.obj_work.disp_flag & 1) == 0 && AppMain.GmPlayerKeyCheckWalkRight(ply_work)) &&
               (ply_work.obj_work.pos.x >> 12 > AppMain.g_gm_main_system.map_fcol.left + 14 &&
                ply_work.obj_work.pos.x >> 12 < AppMain.g_gm_main_system.map_fcol.right - 14) &&
               AppMain.GmPlySeqChangeSequence(ply_work, 18);
    }

    private static bool gmPlySeqCheckTruckWalk(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (AppMain.GmObjCheckMapLeftLimit(ply_work.obj_work, 14) && AppMain.GmPlayerKeyCheckWalkLeft(ply_work) ||
            AppMain.GmObjCheckMapRightLimit(ply_work.obj_work, 14) && AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            return false;
        if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) >= 64 || AppMain.GmPlayerKeyCheckWalkLeft(ply_work) ||
            AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            return AppMain.GmPlySeqChangeSequence(ply_work, 1);
        ply_work.obj_work.spd_m = 0;
        return false;
    }

    private static bool gmPlySeqCheckEndTruckWalk(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) >= 64 || ply_work.obj_work.spd.z != 0)
            return false;
        ply_work.obj_work.spd_m = 0;
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
        return true;
    }

    private static void gmPlySeqSplStgRollCtrl(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        if (((int)AppMain.g_gm_main_system.game_flag & 17240600) != 0)
            return;
        int keyRotZ = ply_work.key_rot_z;
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 512) != 0)
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
                if (AppMain.MTM_MATH_ABS(a) < 16384)
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

        if (ply_work.nudge_di_timer != (short)0)
        {
            --ply_work.nudge_di_timer;
        }
        else
        {
            bool flag = false;
            if (((int)ply_work.key_push & 160) != 0)
                flag = true;
            if (!flag)
                return;
            AppMain.GMS_SPL_STG_WORK work = AppMain.GmSplStageGetWork();
            ply_work.nudge_di_timer = (short)30;
            ply_work.nudge_timer = (short)30;
            work.flag &= 4294967293U;
        }
    }

    private static void GmPlySeqInitSpringJump(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        bool spd_clear,
        int no_jump_move_time,
        int fall_dir,
        bool t_cam_slow)
    {
        bool set_act = true;
        AppMain.GmPlySeqChangeSequenceState(ply_work, 29);
        if (spd_clear)
        {
            ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
            ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
            ply_work.obj_work.spd_m = 0;
        }

        if (((int)ply_work.player_flag & 262144) != 0)
        {
            if (fall_dir != -1)
            {
                ply_work.jump_pseudofall_dir = (ushort)fall_dir;
                int a = fall_dir - ply_work.ply_pseudofall_dir;
                if ((ushort)AppMain.MTM_MATH_ABS(a) > (ushort)32768)
                {
                    if (a < 0)
                        ply_work.ply_pseudofall_dir += 65536 + a;
                    else
                        ply_work.ply_pseudofall_dir += a - 65536;
                }
                else
                    ply_work.ply_pseudofall_dir = fall_dir;

                AppMain.g_gm_main_system.pseudofall_dir = (ushort)ply_work.ply_pseudofall_dir;
            }

            AppMain.GmPlayerSetAtk(ply_work);
            set_act = false;
            AppMain.GmPlayerActionChange(ply_work, 40);
            ply_work.obj_work.disp_flag |= 4U;
        }

        AppMain.GmPlySeqGmkInitGmkJump(ply_work, spd_x, spd_y, set_act);
        if (((int)ply_work.player_flag & 262144) != 0 && fall_dir != -1)
            ply_work.gmk_flag2 |= 256U;
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            ply_work.obj_work.disp_flag &= 4294967294U;
            if (t_cam_slow)
                ply_work.gmk_flag2 |= 32U;
        }

        if (((int)ply_work.player_flag & 262144) != 0 && fall_dir != -1)
            ply_work.gmk_flag |= 16777216U;
        if (no_jump_move_time > 0)
            AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, no_jump_move_time);
        if (((int)ply_work.player_flag & 262144) != 0)
            AppMain.GmSoundPlaySE("Lorry5");
        else
            AppMain.GmSoundPlaySE("Spring");
        AppMain.GMM_PAD_VIB_SMALL();
    }

    private static void GmPlySeqInitRockRideStart(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (AppMain.OBS_OBJECT_WORK)com_work)
            return;
        AppMain.GmPlySeqChangeSequenceState(ply_work, 30);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = com_work.obj_work;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainGimmickRockRidePush);
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        AppMain.OBS_OBJECT_WORK gmkObj = ply_work.gmk_obj;
        AppMain.GmPlayerActionChange(ply_work, 17);
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
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (AppMain.OBS_OBJECT_WORK)com_work)
            return;
        AppMain.GmPlySeqChangeSequenceState(ply_work, 31);
        AppMain.GmPlySeqGmkInitGimmickDependInit(ply_work, com_work.obj_work, 0, 0, 0);
        ply_work.gmk_obj = com_work.obj_work;
        com_work.target_dp_dist = 229376;
        ply_work.player_flag |= 12U;
        ply_work.obj_work.move_flag |= 256U;
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        AppMain.OBS_OBJECT_WORK gmkObj = ply_work.gmk_obj;
        if (objWork.pos.y > gmkObj.pos.y)
        {
            AppMain.GmPlySeqChangeSequence(ply_work, 16);
            ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainGimmickRockRideStop);
        }
        else
        {
            ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainGimmickRockRide);
            AppMain.GmPlayerCameraOffsetSet(ply_work, (short)0, (short)-48);
            AppMain.GmCameraAllowSet(10f, 30f, 0.0f);
        }

        AppMain.GmPlayerActionChange(ply_work, 60);
        objWork.disp_flag |= 4U;
        ply_work.gmk_flag |= 16384U;
        int v1 = AppMain.FX_Div(gmkObj.pos.x - objWork.pos.x, 229376);
        objWork.spd_m = AppMain.FX_Mul(v1, 15360) + gmkObj.spd_m;
        if (gmkObj.spd_m > 0)
            objWork.disp_flag &= 4294967294U;
        else
            objWork.disp_flag |= 1U;
    }

    private static void GmPlySeqInitPulley(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (AppMain.OBS_OBJECT_WORK)com_work)
            return;
        AppMain.GmPlySeqChangeSequenceState(ply_work, 32);
        com_work.obj_work.spd.x = ply_work.obj_work.spd.x;
        if (((int)ply_work.obj_work.move_flag & 16) == 0)
            com_work.obj_work.spd.x =
                AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathCos((int)ply_work.obj_work.dir.z));
        com_work.obj_work.move_flag &= 4294967231U;
        AppMain.GmPlySeqGmkInitGimmickDependInit(ply_work, com_work.obj_work, 0, 163840, 0);
        com_work.target_dp_pos.x = 0;
        com_work.target_dp_pos.y = 163840;
        com_work.target_dp_pos.z = 0;
        ply_work.player_flag |= 12U;
        com_work.target_dp_dist = -163840;
        ply_work.obj_work.move_flag |= 256U;
        ply_work.obj_work.move_flag &= 4294967278U;
        ply_work.gmk_flag |= 16384U;
        AppMain.GmPlayerActionChange(ply_work, 66);
        ply_work.obj_work.pos.Assign(com_work.obj_work.pos);
        ply_work.obj_work.pos.y += 163840;
    }

    private static void GmPlySeqInitBreathing(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 33);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainGimmickBreathing);
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        objWork.spd_m = 0;
        objWork.spd.x = 0;
        objWork.spd.y = 0;
        objWork.spd_add.x = 0;
        objWork.spd_add.y = 0;
        if (((int)objWork.move_flag & 1) != 0)
            AppMain.GmPlayerActionChange(ply_work, 62);
        else
            AppMain.GmPlayerActionChange(ply_work, 68);
        AppMain.GmSoundPlaySE("Breathe");
    }

    private static void GmPlySeqInitDashPanel(AppMain.GMS_PLAYER_WORK ply_work, uint type)
    {
        int[][] numArray = new int[4][]
        {
            new int[2] {55296, 0},
            new int[2] {-55296, 0},
            new int[2] {0, -55296},
            new int[2] {0, -55296}
        };
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequenceState(ply_work, 34);
        if (((int)ply_work.player_flag & 262144) == 0)
            AppMain.GmPlayerActionChange(ply_work, 27);
        else if (type == 1U || type == 3U)
        {
            ply_work.gmk_flag |= 1048576U;
            AppMain.GmPlayerActionChange(ply_work, 72);
        }
        else
        {
            ply_work.gmk_flag &= 1048576U;
            AppMain.GmPlayerActionChange(ply_work, 71);
        }

        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.obj_work.user_timer = 60;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainDashPanel);
        if (((int)ply_work.player_flag & 262144) == 0)
            AppMain.GmPlySeqGmkSpdSet(ply_work, numArray[(int)type][0], numArray[(int)type][1]);
        else
            AppMain.GmPlySeqGmkTruckSpdSet(ply_work, numArray[(int)type][0], numArray[(int)type][1]);
        ply_work.no_spddown_timer = 49152;
        ply_work.spd_work_max = ply_work.obj_work.spd_m;
        AppMain.GmPlayerSetAtk(ply_work);

        var betterSfx = gs.backup.SSave.CreateInstance().GetRemaster().BetterSoundEffects;
        if (!betterSfx)
            AppMain.GmSoundPlaySE("Spin");

        if (((int)ply_work.player_flag & 262144) == 0)
        {
            AppMain.GmPlyEfctCreateSpinDashBlur(ply_work, 1U);
            AppMain.GmPlyEfctCreateSpinDashCircleBlur(ply_work);
            AppMain.GmPlyEfctCreateTrail(ply_work, 1);
        }

        AppMain.GMM_PAD_VIB_SMALL();
    }

    private static void GmPlySeqInitTarzanRope(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (AppMain.OBS_OBJECT_WORK)com_work)
            return;
        AppMain.GmPlySeqChangeSequenceState(ply_work, 35);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = com_work.obj_work;
        ply_work.seq_func = (AppMain.seq_func_delegate)null;
        ply_work.obj_work.move_flag &= 4294967102U;
        AppMain.GmPlayerActionChange(ply_work, 63);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.gmk_flag |= 16384U;
    }

    private static void GmPlySeqInitWaterSlider(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (AppMain.OBS_OBJECT_WORK)com_work)
            return;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequenceState(ply_work, 36);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = com_work.obj_work;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainWaterSlider);
        ply_work.obj_work.move_flag &= 4294967279U;
        AppMain.GmPlayerActionChange(ply_work, 65);
        ply_work.obj_work.disp_flag |= 4U;
        AppMain.GmGmkWaterSliderCreateEffect();
    }

    private static void GmPlySeqInitSpipe(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 37);
        if (ply_work.act_state != 26 && ply_work.act_state != 27)
            AppMain.GmSoundPlaySE("Spin");
        if (ply_work.act_state != 27)
        {
            AppMain.GmPlayerActionChange(ply_work, 27);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainSpipe);
        AppMain.GmPlyEfctCreateSpinDashBlur(ply_work, 1U);
        AppMain.GmPlyEfctCreateSpinDashCircleBlur(ply_work);
    }

    private static ushort GmPlySeqScrewCheck(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return ply_work.seq_func == new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkScrewMain)
            ? (ushort)1
            : (ushort)0;
    }

    private static void GmPlySeqInitScrew(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.GMS_ENEMY_COM_WORK gmk_work,
        int pos_x,
        int pos_y,
        ushort flag)
    {
        if (AppMain.GmPlySeqScrewCheck(ply_work) != (ushort)0)
            return;
        AppMain.GmPlySeqChangeSequenceState(ply_work, 38);
        AppMain.GmPlayerWalkActionSet(ply_work);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 8208U;
        ply_work.gmk_flag |= 147456U;
        ply_work.gmk_obj = gmk_work.obj_work;
        ply_work.gmk_work0 = pos_x;
        ply_work.gmk_work1 = pos_y;
        ply_work.obj_work.user_work = (uint)flag;
        ply_work.obj_work.user_timer = 0;
        if (((int)ply_work.obj_work.user_work & AppMain.GMD_GMK_SCREW_EVE_FLAG_LEFT) != 0)
        {
            if (ply_work.gmk_work0 > ply_work.obj_work.pos.x)
                ply_work.obj_work.user_timer = ply_work.gmk_work0 - ply_work.obj_work.pos.x;
        }
        else if (ply_work.gmk_work0 < ply_work.obj_work.pos.x)
            ply_work.obj_work.user_timer = ply_work.obj_work.pos.x - ply_work.gmk_work0;

        ply_work.gmk_work1 -= (int)ply_work.obj_work.field_rect[3] << 12;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkScrewMain);
        ply_work.timer = 16;
    }

    private static void GmPlySeqInitDemoFw(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 39);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        ply_work.seq_func = (AppMain.seq_func_delegate)null;
        if (((int)ply_work.player_flag & 262144) != 0)
        {
            AppMain.GmPlayerActionChange(ply_work, 69);
            ply_work.obj_work.disp_flag |= 4U;
        }
        else
        {
            AppMain.GmPlayerActionChange(ply_work, 0);
            ply_work.obj_work.disp_flag |= 4U;
        }
    }

    private static void GmPlySeqInitCannon(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.GMS_ENEMY_COM_WORK gmk_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 41);
        AppMain.GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.obj_work.move_flag |= 512U;
        ply_work.obj_work.pos.x = gmk_work.obj_work.pos.x;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.x = 0;
        if (ply_work.obj_work.spd_add.y <= 0)
            ply_work.obj_work.spd_add.y = 672;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkCannonWait);
        ply_work.gmk_obj = gmk_work.obj_work;
        ply_work.gmk_flag2 |= 134U;
        AppMain.GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
    }

    private static void GmPlySeqInitCannonShoot(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 42);
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, spd_x, spd_y);
        AppMain.GmPlayerActionChange(ply_work, 67);
        ply_work.obj_work.disp_flag |= 4U;
        AppMain.GmPlayerSetDefNormal(ply_work);
        AppMain.GmPlayerSetAtk(ply_work);
        AppMain.GmPlyEfctCreateSpinJumpBlur(ply_work);
    }

    private static void GmPlySeqInitStopper(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.GMS_ENEMY_COM_WORK gmk_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 40);
        if (ply_work.act_state != 26)
            AppMain.GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 16U;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.flag |= 2U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkStopperMove);
        ply_work.gmk_obj = gmk_work.obj_work;
    }

    private static void GmPlySeqInitStopperEnd(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.move_flag |= 144U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkStopperEnd);
    }

    private static void GmPlySeqGmkInitUpBumper(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 43);
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, spd_x, spd_y);
        AppMain.GmSoundPlaySE("Spring");
    }

    private static void GmPlySeqGmkInitSeesaw(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.GMS_ENEMY_COM_WORK gmk_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 44);
        if (ply_work.act_state != 27)
        {
            AppMain.GmPlayerActionChange(ply_work, 27);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.dir.z = (ushort)0;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkSeesaw);
        ply_work.gmk_obj = gmk_work.obj_work;
        AppMain.GmPlyEfctCreateSpinDashBlur(ply_work, 1U);
        AppMain.GmPlyEfctCreateSpinDashCircleBlur(ply_work);
    }

    private static void GmPlySeqGmkInitSeesawEnd(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spdx,
        int spdy)
    {
        AppMain.GmPlySeqChangeSequence(ply_work, 16);
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, spdx, spdy, false);
        ply_work.no_spddown_timer = 0;
        ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        AppMain.GmPlayerSetAtk(ply_work);
    }

    private static void GmPlySeqGmkInitSpinFall(AppMain.GMS_PLAYER_WORK ply_work, int spdx, int spdy)
    {
        ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.GmPlySeqChangeSequenceState(ply_work, 66);
        AppMain.GmPlySeqInitFallState(ply_work);
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, spdx, spdy, false);
        ply_work.no_spddown_timer = 0;
        if (ply_work.act_state != 26)
        {
            AppMain.GmPlayerActionChange(ply_work, 26);
            ply_work.obj_work.disp_flag |= 4U;
        }

        AppMain.GmPlayerSetAtk(ply_work);
        AppMain.GmPlyEfctCreateSpinJumpBlur(ply_work);
    }

    private static void GmPlySeqInitPinball(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_spddown_timer)
    {
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequenceState(ply_work, 45);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        if (ply_work.act_state != 39)
        {
            AppMain.GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
            AppMain.GmPlyEfctCreateSpinJumpBlur(ply_work);
        }

        ply_work.obj_work.move_flag &= 4294967279U;
        AppMain.GmPlySeqGmkSpdSet(ply_work, spd_x, spd_y);
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.z = 0;
        ply_work.obj_work.user_timer = 60;
        ply_work.no_spddown_timer = no_spddown_timer * 4096;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainPinball);
        AppMain.GmPlayerSetAtk(ply_work);
        AppMain.GmSoundPlaySE("Spin");
    }

    private static void GmPlySeqInitPinballAir(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        AppMain.GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, 5, false, 0);
    }

    private static void GmPlySeqInitPinballAir(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_move_time)
    {
        AppMain.GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, no_move_time, false, 0);
    }

    private static void GmPlySeqInitPinballAir(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_move_time,
        bool flag_no_recover_homing)
    {
        AppMain.GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, no_move_time, flag_no_recover_homing ? 1 : 0, 0);
    }

    private static void GmPlySeqInitPinballAir(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_move_time,
        bool flag_no_recover_homing,
        int no_spddown_timer)
    {
        AppMain.GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, no_move_time, flag_no_recover_homing ? 1 : 0,
            no_spddown_timer);
    }

    private static void GmPlySeqInitPinballAir(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_move_time,
        int flag_no_recover_homing,
        int no_spddown_timer)
    {
        uint num1 = 0;
        if (((int)ply_work.rect_work[1].flag & 4) != 0)
            num1 = 1U;
        uint num2 = 0;
        if (((int)ply_work.player_flag & 128) != 0)
            num2 = 1U;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequenceState(ply_work, 46);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        if (((long)flag_no_recover_homing & (long)num2) != 0L)
            ply_work.player_flag |= 128U;
        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.obj_work.move_flag |= 128U;
        ply_work.obj_work.flag &= 4294967293U;
        ply_work.player_flag |= 32U;
        ply_work.obj_work.spd_fall = AppMain.FX_Mul(ply_work.obj_work.spd_fall, AppMain.FX_F32_TO_FX32(1.1f));
        ply_work.obj_work.dir.y = (ushort)0;
        AppMain.GmPlySeqGmkSpdSet(ply_work, spd_x, spd_y);
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.z = 0;
        ply_work.obj_work.spd_m = 0;
        bool flag = false;
        if (((int)ply_work.obj_work.disp_flag & 4) != 0)
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

        AppMain.GmPlayerActionChange(ply_work, act_state);
        if (flag)
            ply_work.obj_work.disp_flag |= 4U;
        ply_work.no_spddown_timer = no_spddown_timer * 4096;
        ply_work.obj_work.user_timer = no_move_time;
        ply_work.obj_work.user_flag = 1U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainPinballAir);
        if (num1 != 0U)
            AppMain.GmPlayerSetAtk(ply_work);
        if (((int)ply_work.gmk_flag & 4096) == 0)
            return;
        ply_work.obj_work.spd.z = ply_work.obj_work.spd.y;
        ply_work.obj_work.spd.y = 0;
        if (ply_work.obj_work.pos.z >= 0)
            return;
        ply_work.obj_work.spd.z = -ply_work.obj_work.spd.z;
    }

    private static void GmPlySeqInitFlipper(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        AppMain.GMS_ENEMY_COM_WORK com_work)
    {
        if (ply_work.gmk_obj == (AppMain.OBS_OBJECT_WORK)com_work)
            return;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequenceState(ply_work, 47);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = com_work.obj_work;
        if (ply_work.act_state != 39)
        {
            AppMain.GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
            AppMain.GmPlyEfctCreateSpinJumpBlur(ply_work);
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
        ply_work.obj_work.dir.z = (ushort)0;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainFlipper);
        AppMain.GmSoundPlaySE("Spin");
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

    private static void GmPlySeqGmkInitForceSpin(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequenceState(ply_work, 51);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        if (ply_work.act_state != 26 && ply_work.act_state != 27)
            AppMain.GmSoundPlaySE("Spin");
        if (ply_work.act_state != 26)
        {
            AppMain.GmPlayerActionChange(ply_work, 26);
            ply_work.obj_work.disp_flag |= 4U;
            AppMain.GmPlyEfctCreateSpinDashBlur(ply_work, 0U);
        }

        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainForceSpin);
        ply_work.obj_work.user_timer = ply_work.obj_work.spd_m;
        ply_work.obj_work.user_flag = 0U;
        AppMain.GmPlayerSetAtk(ply_work);
        ply_work.obj_work.move_flag |= 193U;
        ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void GmPlySeqGmkInitForceSpinDec(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequenceState(ply_work, 52);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        if (ply_work.act_state != 26 && ply_work.act_state != 27)
            AppMain.GmSoundPlaySE("Spin");
        if (ply_work.act_state != 26)
        {
            AppMain.GmPlayerActionChange(ply_work, 26);
            ply_work.obj_work.disp_flag |= 4U;
            AppMain.GmPlyEfctCreateSpinDashBlur(ply_work, 0U);
        }

        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainForceSpinDec);
        ply_work.obj_work.user_timer = ply_work.obj_work.spd_m;
        ply_work.obj_work.user_flag = 1U;
        AppMain.GmPlayerSetAtk(ply_work);
        ply_work.obj_work.move_flag |= 193U;
        ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void GmPlySeqGmkInitForceSpinFall(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 53);
        ply_work.obj_work.move_flag |= 32912U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainForceSpinFall);
        ply_work.obj_work.spd.x =
            AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathCos((int)ply_work.obj_work.dir.z));
        ply_work.obj_work.spd.y =
            AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathSin((int)ply_work.obj_work.dir.z));
        if (((int)ply_work.obj_work.user_flag & 1) == 0 || AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) <=
            AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.y))
            return;
        ply_work.obj_work.spd.y = ply_work.obj_work.spd.x >> 1;
        if (ply_work.obj_work.spd.y < 0)
            ply_work.obj_work.spd.y = -ply_work.obj_work.spd.y;
        ply_work.obj_work.spd.x >>= 1;
    }

    private static void GmPlySeqInitPinballCtpltHold(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.GMS_ENEMY_COM_WORK gmk_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 48);
        if (ply_work.prev_seq_state != 51 && ply_work.prev_seq_state != 52)
        {
            AppMain.GmPlayerActionChange(ply_work, 26);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag &= 4294967279U;
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.dir.z = (ushort)0;
        AppMain.GmPlyEfctCreateSpinDashBlur(ply_work, 0U);
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainPinballCtpltHold);
        ply_work.gmk_obj = gmk_work.obj_work;
    }

    private static void GmPlySeqInitPinballCtplt(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequenceState(ply_work, spd_x == 0 ? 49 : 50);
        AppMain.GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        if (spd_x != 0)
        {
            ply_work.obj_work.move_flag &= 4294967279U;
            ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainPinballCtplt);
            if (spd_x > 0)
                ply_work.obj_work.disp_flag &= 4294967294U;
            else
                ply_work.obj_work.disp_flag |= 1U;
        }
        else
        {
            ply_work.obj_work.move_flag |= 144U;
            AppMain.GmPlySeqGmkSpdSet(ply_work, spd_x, spd_y);
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.dir.z = (ushort)0;
            ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainPinballAir);
        }

        ply_work.obj_work.flag &= 4294967293U;
        AppMain.GmPlayerSetAtk(ply_work);
        ply_work.no_spddown_timer = 2457600;
        AppMain.GmSoundPlaySE("Catapult");
        AppMain.GmPlyEfctCreateSpinJumpBlur(ply_work);
    }

    private static void GmPlySeqInitMoveGear(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.OBS_OBJECT_WORK gmk_obj,
        bool cam_adjust)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 54);
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        ply_work.obj_work.dir.z = (ushort)0;
        AppMain.GmPlySeqGmkInitGimmickDependInit(ply_work, gmk_obj, 0, 0, 0);
        ply_work.player_flag |= 514U;
        ply_work.obj_work.user_flag = 1U;
        ply_work.obj_work.move_flag |= 257U;
        if (ply_work.obj_work.spd_m != 0)
            AppMain.GmPlayerWalkActionSet(ply_work);
        else if (ply_work.act_state != 0)
        {
            AppMain.GmPlayerActionChange(ply_work, 0);
            ply_work.obj_work.disp_flag |= 4U;
        }

        if (cam_adjust)
        {
            AppMain.GmPlayerCameraOffsetSet(ply_work, (short)0, (short)-48);
            AppMain.GmCameraAllowSet(0.0f, 0.0f, 0.0f);
        }

        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainMoveGear);
    }

    private static void GmPlySeqInitSteamPipeIn(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequenceState(ply_work, 57);
        AppMain.GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 256U;
        ply_work.obj_work.move_flag &= 4294967167U;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_m = 0;
        AppMain.GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainSteamPipe);
        ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
        ply_work.obj_work.user_timer = 0;
        AppMain.GmPlyEfctCreateSteamPipe(ply_work);
        AppMain.GmPlyEfctCreateSpinDashBlur(ply_work, 0U);
    }

    private static void GmPlySeqInitSteamPipeOut(AppMain.GMS_PLAYER_WORK ply_work, int spd_x)
    {
        ply_work.obj_work.move_flag &= 4294967039U;
        ply_work.obj_work.move_flag |= 128U;
        AppMain.GmPlayerSetDefNormal(ply_work);
        ply_work.obj_work.user_timer = 60;
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, spd_x, 0);
        AppMain.GmPlayerActionChange(ply_work, 26);
        AppMain.GmPlayerSetAtk(ply_work);
    }

    private static void GmPlySeqGmkInitPopSteamJump(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        int no_jump_move_time)
    {
        if (ply_work.seq_state != 58)
            AppMain.GmPlySeqChangeSequenceState(ply_work, 58);
        ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_m = 0;
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, spd_x, spd_y);
        if (no_jump_move_time <= 0)
            return;
        AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, no_jump_move_time);
    }

    private static void GmPlySeqInitDrainTank(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 55);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        AppMain.GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4294967166U;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd.z = 0;
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.z = 0;
        ply_work.seq_func = (AppMain.seq_func_delegate)null;
    }

    private static void GmPlySeqInitDrainTankFall(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 56);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        AppMain.GmPlayerActionChange(ply_work, 26);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.obj_work.move_flag |= 32912U;
        ply_work.obj_work.move_flag &= 4294967294U;
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.z = 0;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainDrainTank);
    }

    private static void GmPlySeqInitSplIn(AppMain.GMS_PLAYER_WORK ply_work, AppMain.VecFx32 pos)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 59);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        if (ply_work.act_state != 26)
        {
            AppMain.GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag |= 41232U;
        ply_work.obj_work.move_flag &= 4294967166U;
        ply_work.obj_work.pos.x = pos.x;
        ply_work.obj_work.pos.y = pos.y;
        ply_work.seq_func = (AppMain.seq_func_delegate)null;
        AppMain.g_gm_main_system.game_flag |= 16384U;
    }

    private static void GmPlySeqGmkInitBoss2Catch(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 60);
        if (ply_work.act_state != 39)
        {
            AppMain.GmPlayerActionChange(ply_work, 39);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.move_flag |= 41232U;
        ply_work.obj_work.move_flag &= 4294967166U;
        ply_work.seq_func = (AppMain.seq_func_delegate)null;
    }

    private static void GmPlySeqGmkInitBoss5Quake(AppMain.GMS_PLAYER_WORK ply_work, int no_move_time)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 61);
        if (ply_work.act_state != 34)
        {
            AppMain.GmPlayerActionChange(ply_work, 34);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_add.x = ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.move_flag |= 40960U;
        ply_work.obj_work.user_timer = no_move_time;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainBoss5Quake);
    }

    private static void GmPlySeqGmkInitEndingDemo1(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 62);
        AppMain.GmPlayerActionChange(ply_work, 77);
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainEndingFrontSide);
        ply_work.obj_work.spd_m = 0;
        ply_work.obj_work.spd.x = 0;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_add.x = 0;
        ply_work.obj_work.spd_add.y = 0;
    }

    private static void GmPlySeqGmkInitEndingDemo2(AppMain.GMS_PLAYER_WORK ply_work, bool type2)
    {
        AppMain.GmPlySeqChangeSequenceState(ply_work, 63);
        AppMain.GmPlayerActionChange(ply_work, 39);
        ply_work.obj_work.disp_flag |= 4U;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainEndingFinish);
        ply_work.obj_work.spd.y = -10240;
        ply_work.obj_work.spd_add.y = 168;
        ply_work.obj_work.dir.y = (ushort)16384;
        ply_work.obj_work.user_work = 0U;
        ply_work.obj_work.user_flag = 0U;
        if (type2)
            ply_work.obj_work.user_flag = 1U;
        ply_work.obj_work.move_flag |= 33040U;
    }

    private static void GmPlySeqGmkInitTruckDanger(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.OBS_OBJECT_WORK gmk_obj)
    {
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequenceState(ply_work, 64);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        ply_work.gmk_obj = gmk_obj;
        AppMain.GmPlayerSetDefInvincible(ply_work);
        ply_work.invincible_timer = 0;
        ply_work.player_flag &= 4294967280U;
        ply_work.gmk_flag &= 4293918719U;
        ply_work.gmk_flag |= 32768U;
        AppMain.nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
        int num1 = 32768 - (int)ply_work.obj_work.dir.z +
                   (int)(short)((int)AppMain.g_gm_main_system.pseudofall_dir - (int)ply_work.obj_work.dir_fall);
        ply_work.gmk_work1 = 0;
        ply_work.gmk_work2 = 69632;
        ply_work.gmk_work3 = 0;
        ply_work.obj_work.user_work = 0U;
        uint num2 = AppMain.gmPlayerCheckTruckAirFoot(ply_work);
        if (ply_work.obj_work.dir.z <= (ushort)32768)
        {
            if (((int)num2 & 1) != 0)
            {
                num1 -= 6144;
                ply_work.obj_work.user_timer = 1024;
            }
            else
            {
                ply_work.player_flag |= 2U;
                AppMain.GmPlayerActionChange(ply_work, 74);
            }
        }
        else if (((int)num2 & 2) != 0)
        {
            num1 += 6144;
            ply_work.player_flag |= 4U;
            ply_work.obj_work.user_timer = -1024;
        }
        else
        {
            ply_work.player_flag |= 2U;
            AppMain.GmPlayerActionChange(ply_work, 74);
        }

        ply_work.gmk_work0 = (int)(ushort)(num1 / 17);
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainTruckDanger);
        AppMain.GmSoundPlaySE("Lorry2");
    }

    private static void GmPlySeqGmkInitTruckDangerRet(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.OBS_OBJECT_WORK gmk_obj)
    {
        int gmkWork3 = ply_work.gmk_work3;
        uint num1 = ply_work.player_flag & 13U;
        AppMain.GmPlySeqChangeSequenceState(ply_work, 64);
        AppMain.GmPlayerStateGimmickInit(ply_work);
        ply_work.player_flag |= num1;
        ply_work.gmk_obj = gmk_obj;
        AppMain.GmPlayerActionChange(ply_work, 76);
        ply_work.gmk_flag |= 32768U;
        int num2 = 32768 - (int)ply_work.obj_work.dir.z - gmkWork3 +
                   (int)(short)((int)AppMain.g_gm_main_system.pseudofall_dir - (int)ply_work.obj_work.dir_fall);
        if (num2 > 0)
            num2 = (int)(ushort)num2;
        ply_work.gmk_work0 = -num2 / 14;
        ply_work.gmk_work1 = num2;
        ply_work.gmk_work2 = 0;
        ply_work.gmk_work3 = gmkWork3;
        ply_work.obj_work.vib_timer = 0;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainTruckDangerRet);
    }

    private static void GmPlySeqGmkInitGmkJump(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, spd_x, spd_y, true);
    }

    private static void GmPlySeqGmkInitGmkJump(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y,
        bool set_act)
    {
        if (((int)ply_work.player_flag & 1024) != 0)
            return;
        AppMain.GmPlayerStateGimmickInit(ply_work);
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
            ply_work.obj_work.spd.x = ply_work.obj_work.spd_m;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        if (((int)ply_work.player_flag & 262144) != 0)
            AppMain.ObjObjectSpdDirFall(ref spd_x, ref spd_y, (ushort)-ply_work.jump_pseudofall_dir);
        else
            AppMain.ObjObjectSpdDirFall(ref spd_x, ref spd_y, ply_work.obj_work.dir_fall);
        if (((int)ply_work.obj_work.move_flag & 16) == 0)
            ply_work.camera_jump_pos_y = ply_work.obj_work.pos.y;
        ply_work.obj_work.move_flag |= 32784U;
        ply_work.obj_work.move_flag &= 4294967294U;
        if (spd_x != 0)
        {
            if (set_act)
                AppMain.GmPlayerActionChange(ply_work, 47);
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
                AppMain.GmPlayerActionChange(ply_work, 44);
                ply_work.obj_work.disp_flag |= 4U;
            }

            ply_work.obj_work.spd.x =
                AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathCos((int)ply_work.obj_work.dir.z));
        }

        ply_work.obj_work.spd.y = spd_y == 0
            ? AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathSin((int)ply_work.obj_work.dir.z))
            : spd_y;
        ply_work.obj_work.user_timer = 0;
        ply_work.obj_work.user_work = 0U;
        ply_work.timer = 0;
        AppMain.GmPlySeqSetJumpState(ply_work, 0, 3U);
        if (((int)ply_work.player_flag & 67108864) == 0)
            return;
        AppMain.GMD_PLAYER_WATERJUMP_SET(ref ply_work.obj_work.spd.x);
        AppMain.GMD_PLAYER_WATERJUMP_SET(ref ply_work.obj_work.spd.y);
    }

    private static void GmPlySeqGmkInitGimmickDependInit(
        AppMain.GMS_PLAYER_WORK ply_work,
        AppMain.OBS_OBJECT_WORK gmk_obj,
        int ofst_x,
        int ofst_y,
        int ofst_z)
    {
        if (ply_work.gmk_obj == gmk_obj)
            return;
        AppMain.GmPlayerSpdParameterSet(ply_work);
        AppMain.GmPlayerStateGimmickInit(ply_work);
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
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.GmPlySeqGmkMainGimmickDepend);
    }

    private static void GmPlySeqGmkMainGimmickDepend(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)ply_work;
        AppMain.OBS_OBJECT_WORK gmkObj1 = ply_work.gmk_obj;
        if (gmkObj1 != null)
        {
            AppMain.GMS_ENEMY_COM_WORK gmkObj2 = (AppMain.GMS_ENEMY_COM_WORK)ply_work.gmk_obj;
            if (((int)gmkObj2.enemy_flag & 15) != 0)
            {
                ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
                if (((int)gmkObj2.enemy_flag & 2) != 0)
                {
                    obsObjectWork.spd.x = gmkObj1.spd.x;
                    obsObjectWork.spd.y = gmkObj1.spd.y;
                }
                else if (((int)gmkObj2.enemy_flag & 4) != 0)
                    obsObjectWork.spd.x = gmkObj1.spd_m;
                else if (((int)gmkObj2.enemy_flag & 8) != 0)
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
                if (((int)ply_work.player_flag & 1) != 0)
                    obsObjectWork.pos.Assign(gmkObj1.pos);
                else if (((int)ply_work.player_flag & 2) != 0)
                {
                    obsObjectWork.pos.x = gmkObj1.pos.x + gmkObj2.target_dp_pos.x;
                    obsObjectWork.pos.y = gmkObj1.pos.y + gmkObj2.target_dp_pos.y;
                    obsObjectWork.pos.z = gmkObj1.pos.z + gmkObj2.target_dp_pos.z;
                }
                else if (((int)ply_work.player_flag & 4) != 0)
                {
                    AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
                    AppMain.NNS_VECTOR nnsVector = new AppMain.NNS_VECTOR(0.0f, -1f, 0.0f);
                    AppMain.nnMakeUnitMatrix(nnsMatrix);
                    AppMain.nnRotateXYZMatrix(nnsMatrix, nnsMatrix, (int)-gmkObj2.target_dp_dir.x,
                        (int)gmkObj2.target_dp_dir.y, (int)gmkObj2.target_dp_dir.z);
                    AppMain.nnTransformVector(nnsVector, nnsMatrix, nnsVector);
                    AppMain.nnScaleVector(nnsVector, nnsVector, AppMain.FXM_FX32_TO_FLOAT(gmkObj2.target_dp_dist));
                    obsObjectWork.pos.x = gmkObj1.pos.x + AppMain.FXM_FLOAT_TO_FX32(nnsVector.x);
                    obsObjectWork.pos.y = gmkObj1.pos.y + AppMain.FXM_FLOAT_TO_FX32(nnsVector.y);
                    obsObjectWork.pos.z = gmkObj1.pos.z + AppMain.FXM_FLOAT_TO_FX32(nnsVector.z);
                    AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
                }

                if (((int)ply_work.player_flag & 8) != 0)
                    obsObjectWork.dir.Assign(gmkObj2.target_dp_dir);
                obsObjectWork.move.x = obsObjectWork.pos.x - obsObjectWork.prev_pos.x;
                obsObjectWork.move.y = obsObjectWork.pos.y - obsObjectWork.prev_pos.y;
                obsObjectWork.move.z = obsObjectWork.pos.z - obsObjectWork.prev_pos.z;
                if (((int)obsObjectWork.user_flag & 1) != 0 && gmkObj1.vib_timer != 0)
                    obsObjectWork.vib_timer = gmkObj1.vib_timer + 4096;
                if (((int)obsObjectWork.move_flag & 8192) != 0)
                    obsObjectWork.flow.x = obsObjectWork.flow.y = obsObjectWork.flow.z = 0;
            }
        }

        if (ply_work.gmk_obj != null)
            return;
        AppMain.GmPlayerStateGimmickInit(ply_work);
    }

    private static void GmPlySeqGmkSpdSet(AppMain.GMS_PLAYER_WORK ply_work, int spd_x, int spd_y)
    {
        if (spd_x < 0)
            ply_work.obj_work.disp_flag |= 1U;
        else if (spd_x > 0)
            ply_work.obj_work.disp_flag &= 4294967294U;
        if (((int)ply_work.obj_work.move_flag & 16) != 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd.x > spd_x ||
                ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd.x < spd_x)
                ply_work.obj_work.spd.x = spd_x;
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.y) >= AppMain.MTM_MATH_ABS(spd_y))
                return;
            ply_work.obj_work.spd.y = spd_y;
        }
        else
        {
            switch (((int)ply_work.obj_work.dir.z + 8192 & 49152) >> 14)
            {
                case 0:
                case 2:
                    if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd_m > spd_x ||
                        ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd_m < spd_x)
                        ply_work.obj_work.spd_m = spd_x;
                    if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.y) >= AppMain.MTM_MATH_ABS(spd_y))
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
                    if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) >= AppMain.MTM_MATH_ABS(spd_x))
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
                    if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) >= AppMain.MTM_MATH_ABS(spd_x))
                        break;
                    ply_work.obj_work.spd.x = spd_x;
                    break;
            }
        }
    }

    private static void GmPlySeqGmkTruckSpdSet(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_x,
        int spd_y)
    {
        if (spd_x < 0)
            ply_work.gmk_flag |= 1048576U;
        else if (spd_x > 0)
            ply_work.gmk_flag &= 4293918719U;
        if (((int)ply_work.obj_work.move_flag & 16) != 0)
        {
            if (((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd.x > spd_x ||
                ((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd.x < spd_x)
                ply_work.obj_work.spd.x = spd_x;
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.y) >= AppMain.MTM_MATH_ABS(spd_y))
                return;
            ply_work.obj_work.spd.y = spd_y;
        }
        else
        {
            switch (((int)(ushort)((uint)ply_work.obj_work.dir.z + (uint)ply_work.obj_work.dir_fall) + 8192 &
                     49152) >> 14)
            {
                case 0:
                case 2:
                    if (((int)ply_work.gmk_flag & 1048576) != 0 && ply_work.obj_work.spd_m > spd_x ||
                        ((int)ply_work.gmk_flag & 1048576) == 0 && ply_work.obj_work.spd_m < spd_x)
                        ply_work.obj_work.spd_m = spd_x;
                    if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.y) >= AppMain.MTM_MATH_ABS(spd_y))
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
                    if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) >= AppMain.MTM_MATH_ABS(spd_x))
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
                    if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd.x) >= AppMain.MTM_MATH_ABS(spd_x))
                        break;
                    ply_work.obj_work.spd.x = spd_x;
                    break;
            }
        }
    }

    private static void gmPlySeqGmkMainGimmickRockRidePush(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        objWork.obj_3d.speed[0] -= 0.02f;
        if ((double)objWork.obj_3d.speed[0] <= 0.5)
            objWork.obj_3d.speed[0] = 0.5f;
        if (((int)objWork.disp_flag & 8) == 0)
            return;
        objWork.user_timer = 5;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainGimmickRockRideStartWait);
        objWork.obj_3d.speed[0] = 1f;
    }

    private static void gmPlySeqGmkMainGimmickRockRideStartWait(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        --objWork.user_timer;
        if (objWork.user_timer > 0)
            return;
        objWork.user_timer = 0;
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainGimmickRockRideStartJump);
    }

    private static void gmPlySeqGmkMainGimmickRockRideStartJump(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        if (((int)objWork.disp_flag & 8) == 0)
            return;
        AppMain.OBS_OBJECT_WORK gmkObj = ply_work.gmk_obj;
        int spd_x = 13824;
        if (gmkObj.pos.x < objWork.pos.x)
            spd_x = -spd_x;
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, spd_x, -24576);
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainGimmickRockRideStartFall);
    }

    private static void gmPlySeqGmkMainGimmickRockRideStartFall(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.move_flag & 1) == 0)
            return;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void gmPlySeqGmkMainGimmickRockRide(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        AppMain.OBS_OBJECT_WORK gmkObj = ply_work.gmk_obj;
        if (gmkObj.spd_m == 0)
        {
            ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkMainGimmickRockRideStop);
            AppMain.GmPlayerCameraOffsetSet(ply_work, (short)0, (short)0);
            AppMain.GmCameraAllowReset();
        }
        else
        {
            int num1 = AppMain.FX_Mul(61440,
                AppMain.FX_F32_TO_FX32((float)-AppMain.GmPlayerKeyGetGimmickRotZ(ply_work) /
                                       AppMain.GMD_GMK_ROCK_RIDE_KEY_ANGLE_LIMIT));
            int num2 = (gmkObj.spd_m <= 0 ? num1 - (int)short.MinValue : num1 - 32768) - objWork.spd_m;
            if (num2 > 0)
                objWork.spd_m += 384;
            else if (num2 < 0)
                objWork.spd_m -= 384;
            int a = gmkObj.spd_m - objWork.spd_m;
            int num3 = AppMain.MTM_MATH_ABS(a);
            if (num3 >= 15360)
            {
                int spd_x = 16384;
                if (a < 0)
                    spd_x = -spd_x;
                AppMain.GmPlySeqChangeSequence(ply_work, 16);
                AppMain.GmPlySeqGmkInitGmkJump(ply_work, spd_x, 12288);
                AppMain.GmPlayerCameraOffsetSet(ply_work, (short)0, (short)0);
                AppMain.GmCameraAllowReset();
            }
            else
            {
                if (num3 >= 2816)
                {
                    if (ply_work.act_state != 61)
                    {
                        AppMain.GmPlayerActionChange(ply_work, 61);
                        objWork.disp_flag |= 4U;
                    }
                }
                else if (ply_work.act_state != 60)
                {
                    AppMain.GmPlayerActionChange(ply_work, 60);
                    objWork.disp_flag |= 4U;
                }

                ((AppMain.GMS_ENEMY_COM_WORK)gmkObj).target_dp_dir.z = (ushort)(a * 4 / 5);
                AppMain.GmPlySeqGmkMainGimmickDepend(ply_work);
            }
        }
    }

    private static void gmPlySeqGmkMainGimmickRockRideStop(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, objWork.spd.x, objWork.spd.y);
    }

    private static void gmPlySeqGmkMainGimmickBreathing(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        if (((int)objWork.disp_flag & 8) == 0)
            return;
        if (((int)objWork.move_flag & 1) != 0)
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
        else
            AppMain.GmPlySeqChangeSequence(ply_work, 16);
    }

    private static void gmPlySeqGmkMainDashPanel(AppMain.GMS_PLAYER_WORK ply_work)
    {
        --ply_work.obj_work.user_timer;
        if (ply_work.obj_work.user_timer > 0 && ply_work.obj_work.spd_m != 0)
            return;
        AppMain.GmPlayerSpdParameterSet(ply_work);
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void gmPlySeqGmkMainWaterSlider(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_OBJECT_WORK objWork = ply_work.obj_work;
        if (((int)objWork.move_flag & 1) == 0)
        {
            AppMain.nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
            ply_work.gmk_flag &= 4294934527U;
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
        }
        else
        {
            if (!AppMain.GmPlayerKeyCheckJumpKeyPush(ply_work))
                return;
            AppMain.nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
            ply_work.gmk_flag &= 4294934527U;
            objWork.spd_m /= 2;
            AppMain.GmPlySeqChangeSequence(ply_work, 17);
        }
    }

    private static void gmPlySeqGmkMainSpipe(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.gmk_flag & 65536) != 0)
        {
            if (((int)ply_work.obj_work.move_flag & 1) == 0)
            {
                int num1 = AppMain.FX_Mul(40960, AppMain.mtMathCos((int)ply_work.obj_work.dir.z - 16384));
                ply_work.obj_work.pos.x -= num1;
                int num2 = AppMain.FX_Mul(40960, AppMain.mtMathSin((int)ply_work.obj_work.dir.z - 16384));
                ply_work.obj_work.pos.y -= num2;
                ply_work.obj_work.spd.x = 0;
                ply_work.obj_work.spd.y = 0;
            }

            ply_work.obj_work.move_flag &= 4294934511U;
            if (ply_work.obj_work.spd_m == 0)
            {
                ply_work.obj_work.spd_m = 8192;
                AppMain.GmSoundPlaySE("Spin");
            }
        }
        else
            AppMain.GmPlySeqChangeSequence(ply_work, 10);

        ply_work.gmk_flag &= 4294901759U;
    }

    private static void gmPlySeqGmkScrewMain(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmPlayerWalkActionCheck(ply_work);
        AppMain.GMS_PLY_SEQ_STATE_DATA[] seqStateDataTbl = ply_work.seq_state_data_tbl;
        if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < ply_work.spd2 &&
            ((int)ply_work.obj_work.move_flag & 1) == 0)
        {
            ply_work.obj_work.dir.x = ply_work.obj_work.dir.y = ply_work.obj_work.dir.z = (ushort)0;
            ply_work.obj_work.move_flag &= 4294959103U;
            AppMain.GmPlySeqChangeSequence(ply_work, 16);
        }
        else
        {
            if (ply_work.timer != 0)
                --ply_work.timer;
            else if (((int)ply_work.obj_work.move_flag & 13) != 0)
            {
                ply_work.obj_work.dir.x = ply_work.obj_work.dir.y = ply_work.obj_work.dir.z = (ushort)0;
                ply_work.obj_work.move_flag &= 4294959103U;
                ply_work.obj_work.spd.x = ply_work.obj_work.spd_m;
                AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
                AppMain.GmPlySeqChangeSequence(ply_work, 0);
                return;
            }

            AppMain.GmPlySeqMoveWalk(ply_work);
            AppMain.gmPlySeqGmkMoveScrew(ply_work, 1530320, (short)288, (short)38);
        }
    }

    private static void gmPlySeqGmkMoveScrew(
        AppMain.GMS_PLAYER_WORK ply_work,
        int screw_length,
        short screw_width,
        short screw_height)
    {
        ply_work.obj_work.user_timer += AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
        int userTimer = ply_work.obj_work.user_timer;
        sbyte num1 = (sbyte)(userTimer / screw_length);
        int num2 = (userTimer - screw_length * (int)num1 << 8) / screw_length << 4;
        ushort num3 = (ushort)(num2 << 4 & (int)ushort.MaxValue);
        ply_work.obj_work.dir.x = num3;
        if (((int)ply_work.obj_work.user_work & AppMain.GMD_GMK_SCREW_EVE_FLAG_LEFT) != 0)
            ply_work.obj_work.dir.x = (ushort)-ply_work.obj_work.dir.x;
        ply_work.obj_work.dir.z = ply_work.obj_work.dir.x >= (ushort)16384
            ? (ply_work.obj_work.dir.x >= (ushort)32768
                ? (ply_work.obj_work.dir.x >= (ushort)49152
                    ? (ushort)(65536U - (uint)ply_work.obj_work.dir.x)
                    : (ushort)((uint)ply_work.obj_work.dir.x - 32768U))
                : (ushort)(16384 - ((int)ply_work.obj_work.dir.x - 16384)))
            : ply_work.obj_work.dir.x;
        ply_work.obj_work.dir.z >>= 1;
        if (ply_work.obj_work.dir.x < (ushort)32768)
            ply_work.obj_work.dir.z = (ushort)-ply_work.obj_work.dir.z;
        ply_work.obj_work.dir.x = (ushort)-ply_work.obj_work.dir.x;
        int num4 = AppMain.mtMathCos((int)num3);
        screw_height -= ply_work.obj_work.field_rect[3];
        screw_height = (short)-screw_height;
        ply_work.obj_work.prev_pos.x = ply_work.obj_work.pos.x;
        ply_work.obj_work.prev_pos.y = ply_work.obj_work.pos.y;
        ply_work.obj_work.pos.x = ((int)ply_work.obj_work.user_work & AppMain.GMD_GMK_SCREW_EVE_FLAG_LEFT) == 0
            ? ply_work.gmk_work0 + ((int)num1 * (int)screw_width << 12) + (int)screw_width * num2
            : ply_work.gmk_work0 - ((int)num1 * (int)screw_width << 12) - (int)screw_width * num2;
        ply_work.obj_work.pos.y = ply_work.gmk_work1 + ((int)screw_height << 12) - num4 * (int)screw_height;
        ply_work.obj_work.move.x = ply_work.obj_work.pos.x - ply_work.obj_work.prev_pos.x;
        ply_work.obj_work.move.y = ply_work.obj_work.pos.y - ply_work.obj_work.prev_pos.y;
    }

    private static void gmPlySeqGmkCannonWait(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.pos.y < ply_work.gmk_obj.pos.y)
            return;
        ply_work.obj_work.pos.y = ply_work.gmk_obj.pos.y;
        ply_work.obj_work.spd.y = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_add.y = 0;
        ply_work.obj_work.spd_fall = 0;
        ply_work.seq_func = (AppMain.seq_func_delegate)null;
        ply_work.obj_work.move_flag &= 4294967167U;
    }

    private static void gmPlySeqGmkStopperMove(AppMain.GMS_PLAYER_WORK ply_work)
    {
        ply_work.obj_work.pos.x = (ply_work.obj_work.pos.x + ply_work.gmk_obj.pos.x) / 2;
        if (AppMain.MTM_MATH_ABS(ply_work.obj_work.pos.x - ply_work.gmk_obj.pos.x) < 1024)
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
        ply_work.seq_func = new AppMain.seq_func_delegate(AppMain.gmPlySeqGmkStopperWait);
    }

    private static void gmPlySeqGmkStopperWait(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void gmPlySeqGmkStopperEnd(AppMain.GMS_PLAYER_WORK ply_work)
    {
        bool flag = false;
        if (ply_work.gmk_obj == null)
            flag = true;
        else if (ply_work.gmk_obj.user_timer < ply_work.obj_work.pos.y >> 12)
            flag = true;
        if (!flag)
            return;
        int y = ply_work.obj_work.spd.y;
        AppMain.GmPlySeqChangeSequence(ply_work, 16);
        AppMain.GmPlySeqGmkInitGmkJump(ply_work, 0, y, false);
        ply_work.gmk_obj = (AppMain.OBS_OBJECT_WORK)null;
        if (ply_work.act_state != 26)
        {
            AppMain.GmPlayerActionChange(ply_work, 26);
            ply_work.obj_work.disp_flag |= 4U;
        }

        ply_work.obj_work.flag &= 4294967293U;
    }

    private static void gmPlySeqGmkSeesaw(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void gmPlySeqGmkMainPinball(AppMain.GMS_PLAYER_WORK ply_work)
    {
        --ply_work.obj_work.user_timer;
        if (ply_work.obj_work.user_timer <= 0 || ply_work.obj_work.spd_m == 0)
        {
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
        }
        else
        {
            if (((int)ply_work.obj_work.move_flag & 1) != 0)
                return;
            int spd_x = AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathCos((int)ply_work.obj_work.dir.z));
            int spd_y = AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathSin((int)ply_work.obj_work.dir.z));
            AppMain.GmPlySeqInitPinballAir(ply_work, spd_x, spd_y);
        }
    }

    private static void gmPlySeqGmkMainPinballAir(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.user_timer > 0)
            --ply_work.obj_work.user_timer;
        if (ply_work.obj_work.user_timer <= 0 && ply_work.obj_work.user_flag != 0U)
            ply_work.player_flag &= 4294967263U;
        if (((int)ply_work.obj_work.move_flag & 1) == 0)
            return;
        ply_work.no_spddown_timer = 0;
        AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
    }

    private static void gmPlySeqGmkMainPinballCtpltHold(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void gmPlySeqGmkMainPinballCtplt(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
            return;
        int spd_x = AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathCos((int)ply_work.obj_work.dir.z));
        int spd_y = AppMain.FX_Mul(ply_work.obj_work.spd_m, AppMain.mtMathSin((int)ply_work.obj_work.dir.z));
        AppMain.GmPlySeqInitPinballAir(ply_work, spd_x, spd_y);
    }

    private static void gmPlySeqGmkMainFlipper(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (AppMain.MTM_MATH_ABS(ply_work.gmk_obj.pos.x - ply_work.obj_work.pos.x) <= 221184 &&
            AppMain.MTM_MATH_ABS(ply_work.gmk_obj.pos.y - ply_work.obj_work.pos.y) <= 131072)
            return;
        AppMain.GmPlySeqChangeSequence(ply_work, 0);
        ply_work.obj_work.move_flag |= 128U;
        ply_work.obj_work.move_flag &= 4294934271U;
    }

    private static void gmPlySeqGmkMainForceSpin(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m == 0)
        {
            ply_work.obj_work.spd_m = ((int)ply_work.obj_work.disp_flag & 1) == 0 ? 8192 : -8192;
            AppMain.GmSoundPlaySE("Spin");
        }

        if (((int)ply_work.obj_work.move_flag & 1) != 0)
            return;
        AppMain.GmPlySeqGmkInitForceSpinFall(ply_work);
    }

    private static void gmPlySeqGmkMainForceSpinDec(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.spd_m == 0)
        {
            ply_work.obj_work.spd_m = ((int)ply_work.obj_work.disp_flag & 1) == 0 ? 8192 : -8192;
            AppMain.GmSoundPlaySE("Spin");
        }

        if (((int)ply_work.obj_work.disp_flag & 1) != 0)
        {
            if (ply_work.obj_work.spd_m < -8192)
                ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, -2048);
        }
        else if (ply_work.obj_work.spd_m > 8192)
            ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, 2048);

        if (((int)ply_work.obj_work.move_flag & 1) != 0)
            return;
        AppMain.GmPlySeqGmkInitForceSpinFall(ply_work);
        ply_work.obj_work.spd_m = 0;
    }

    private static void gmPlySeqGmkMainForceSpinFall(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.move_flag & 1) == 0)
            return;
        if (((int)ply_work.obj_work.user_flag & 1) != 0)
            AppMain.GmPlySeqGmkInitForceSpinDec(ply_work);
        else
            AppMain.GmPlySeqGmkInitForceSpin(ply_work);
    }

    private static void gmPlySeqGmkMainMoveGear(AppMain.GMS_PLAYER_WORK ply_work)
    {
        bool flag = true;
        AppMain.OBS_OBJECT_WORK gmkObj = ply_work.gmk_obj;
        if (gmkObj == null)
            AppMain.GmPlySeqChangeFw(ply_work);
        else if (((int)gmkObj.user_flag & 1) == 0 && AppMain.GmPlayerKeyCheckJumpKeyPush(ply_work))
        {
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
            AppMain.GmPlySeqChangeSequence(ply_work, 17);
        }
        else
        {
            ply_work.obj_work.move_flag |= 1U;
            if (((int)gmkObj.user_flag & 2) != 0)
            {
                ply_work.obj_work.spd_m = 0;
                ply_work.obj_work.spd.x = ply_work.obj_work.spd.y = 0;
            }

            if (ply_work.act_state != 8 &&
                (((int)gmkObj.user_flag & 2) == 0 &&
                 (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) && ((int)ply_work.obj_work.disp_flag & 1) == 0 &&
                  ply_work.obj_work.spd_m <= 0 ||
                  AppMain.GmPlayerKeyCheckWalkRight(ply_work) && ((int)ply_work.obj_work.disp_flag & 1) != 0 &&
                  ply_work.obj_work.spd_m >= 0) ||
                 ((int)gmkObj.user_flag & 1) != 0 &&
                 (((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.obj_work.spd_m <= 0 ||
                  ((int)ply_work.obj_work.disp_flag & 1) != 0 && ply_work.obj_work.spd_m >= 0) ||
                 ((int)gmkObj.user_flag & 2) != 0 && gmkObj.user_work == 7U &&
                 (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) && ((int)ply_work.obj_work.disp_flag & 1) == 0 &&
                     gmkObj.user_timer <= 0 || AppMain.GmPlayerKeyCheckWalkRight(ply_work) &&
                     ((int)ply_work.obj_work.disp_flag & 1) != 0 && gmkObj.user_timer >= 0)))
            {
                AppMain.GmPlayerActionChange(ply_work, 8);
                AppMain.GmPlySeqSetProgramTurnFwTurn(ply_work);
            }
            else if (ply_work.act_state == 8)
            {
                if (((int)ply_work.obj_work.disp_flag & 8) != 0)
                {
                    AppMain.GmPlayerSetReverseOnlyState(ply_work);
                    AppMain.GmPlayerActionChange(ply_work, 0);
                    ply_work.obj_work.disp_flag |= 4U;
                }
            }
            else if (((int)gmkObj.user_flag & 2) != 0 &&
                     (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) && ((int)ply_work.obj_work.disp_flag & 1) != 0 ||
                      AppMain.GmPlayerKeyCheckWalkRight(ply_work) && ((int)ply_work.obj_work.disp_flag & 1) == 0) ||
                     gmkObj.user_timer != 0)
            {
                if (ply_work.ring_num == (short)0 && (gmkObj.user_work == 0U || gmkObj.user_work == 4U))
                {
                    if (ply_work.act_state != 33)
                    {
                        AppMain.GmPlayerActionChange(ply_work, 33);
                        ply_work.obj_work.obj_3d.blend_spd = 1f / 16f;
                        ply_work.obj_work.disp_flag |= 4U;
                    }

                    ply_work.obj_work.obj_3d.speed[0] = 0.5f;
                    ply_work.obj_work.obj_3d.speed[1] = 0.5f;
                    flag = false;
                }
                else
                {
                    if ((((int)gmkObj.user_flag & 8) == 0 && AppMain.GmPlayerKeyCheckWalkRight(ply_work) ||
                         ((int)gmkObj.user_flag & 8) != 0 && AppMain.GmPlayerKeyCheckWalkLeft(ply_work)) &&
                        (gmkObj.user_work == 7U && gmkObj.user_timer == 0))
                    {
                        AppMain.GmPlySeqChangeFw(ply_work);
                        return;
                    }

                    if (gmkObj.user_work == 1U || gmkObj.user_work == 2U)
                    {
                        if (ply_work.act_state != 60)
                        {
                            AppMain.GmPlayerActionChange(ply_work, 60);
                            ply_work.obj_work.obj_3d.blend_spd = 1f / 16f;
                            ply_work.obj_work.disp_flag |= 4U;
                        }
                    }
                    else if (ply_work.act_state != 20)
                    {
                        AppMain.GmPlayerActionChange(ply_work, 20);
                        ply_work.obj_work.disp_flag |= 4U;
                    }
                }

                if (ply_work.act_state != 33)
                {
                    flag = false;
                    int num = gmkObj.user_timer * 3;
                    int a = AppMain.MTM_MATH_ABS((num >> 3) + (num >> 2));
                    if (a <= 1024)
                        a = 1024;
                    if (a >= 32768)
                        a = 32768;
                    ply_work.obj_work.obj_3d.speed[0] = AppMain.FXM_FX32_TO_FLOAT(a);
                    ply_work.obj_work.obj_3d.speed[1] = AppMain.FXM_FX32_TO_FLOAT(a);
                }
            }
            else if (ply_work.obj_work.spd_m != 0)
                AppMain.GmPlayerWalkActionCheck(ply_work);
            else if (ply_work.act_state != 0)
            {
                AppMain.GmPlayerActionChange(ply_work, 0);
                ply_work.obj_work.disp_flag |= 4U;
            }

            if (((int)gmkObj.user_flag & 3) == 0)
            {
                if (((int)gmkObj.user_flag & 4) != 0)
                    AppMain.gmPlySeqGmkMoveGearMove(ply_work, true);
                else
                    AppMain.gmPlySeqGmkMoveGearMove(ply_work, false);
            }

            if (flag)
                AppMain.gmPlySeqGmkMoveGearAnimeSpeedSetWalk(ply_work, ply_work.obj_work.spd_m);
            AppMain.GmPlySeqGmkMainGimmickDepend(ply_work);
        }
    }

    private static void gmPlySeqGmkMoveGearMove(AppMain.GMS_PLAYER_WORK ply_work, bool spd_up_type)
    {
        int num1;
        int sSpd;
        if (!spd_up_type)
        {
            if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 1024)
            {
                num1 = ply_work.spd_add >> 3;
                sSpd = ply_work.spd_dec;
            }
            else if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 2048)
            {
                num1 = ply_work.spd_add >> 2;
                sSpd = (ply_work.spd_dec >> 1) + (ply_work.spd_dec >> 2);
            }
            else if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 3072)
            {
                num1 = ply_work.spd_add >> 1;
                sSpd = ply_work.spd_dec >> 1;
            }
            else if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 4096)
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
        else if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 3072)
        {
            num1 = ply_work.spd_add >> 1;
            sSpd = ply_work.spd_dec;
        }
        else if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 4096)
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
        if (AppMain.GmPlayerKeyCheckWalkRight(ply_work) || AppMain.GmPlayerKeyCheckWalkLeft(ply_work))
        {
            int num3 = AppMain.MTM_MATH_ABS(ply_work.key_walk_rot_z);
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
        if (ply_work.obj_work.dir.z != (ushort)0)
        {
            int num3 = AppMain.FX_Mul(ply_work.spd_max_add_slope, AppMain.mtMathSin((int)ply_work.obj_work.dir.z));
            if (num3 > 0)
                num2 += num3;
        }

        if (ply_work.no_spddown_timer != 0)
            sSpd = 0;
        else if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) <= ply_work.spd1)
            num1 = num1 * 5 / 8;
        else if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) <= ply_work.spd2)
            num1 >>= 1;
        else if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) > ply_work.spd3)
        {
            int num3;
            if (num2 - ply_work.spd3 != 0)
            {
                num3 = AppMain.FX_Div(AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) - ply_work.spd3,
                    num2 - ply_work.spd3);
                if (num3 > 4096)
                    num3 = 4096;
            }
            else
                num3 = 4096;

            int v2 = num3 * 3968 >> 12;
            num1 -= AppMain.FX_Mul(num1, v2);
        }

        if (ply_work.spd_work_max >= num2 && AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) >= num2)
        {
            if (ply_work.spd_work_max > ply_work.obj_work.spd_m)
                ply_work.spd_work_max = AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m);
            num2 = ply_work.spd_work_max;
        }

        if (((int)ply_work.player_flag & 32768) != 0 && AppMain.GmPlayerKeyCheckWalkRight(ply_work) &&
            num2 > ply_work.scroll_spd_x + 8192)
            num2 = ply_work.scroll_spd_x + 8192;
        if (AppMain.GmPlayerKeyCheckWalkLeft(ply_work) | AppMain.GmPlayerKeyCheckWalkRight(ply_work))
        {
            if (AppMain.GmPlayerKeyCheckWalkRight(ply_work))
            {
                if (ply_work.obj_work.spd_m < 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdUpSet(ply_work.obj_work.spd_m, num1, num2);
            }
            else
            {
                if (ply_work.obj_work.spd_m > 0)
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
                ply_work.obj_work.spd_m = AppMain.ObjSpdUpSet(ply_work.obj_work.spd_m, -num1, num2);
            }
        }
        else
        {
            ply_work.spd_pool = (short)0;
            ply_work.obj_work.spd.x = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd.x, -num2, num2);
            ply_work.obj_work.spd_m = AppMain.MTM_MATH_CLIP(ply_work.obj_work.spd_m, -num2, num2);
            if (((int)ply_work.obj_work.dir.z + 8192 & 65280) > 16384)
                return;
            if (((int)ply_work.player_flag & 134217728) != 0)
                ply_work.player_flag &= 4160749567U;
            else if (((int)ply_work.player_flag & 32768) != 0)
            {
                if (((int)ply_work.obj_work.disp_flag & 1) == 0 && ply_work.seq_state == 1)
                {
                    int num3 = ply_work.scroll_spd_x - 4096;
                    if (num3 < 0)
                        num3 = 0;
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
                    if (ply_work.obj_work.spd_m >= num3)
                        return;
                    ply_work.obj_work.spd_m = num3;
                }
                else
                    ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
            }
            else
                ply_work.obj_work.spd_m = AppMain.ObjSpdDownSet(ply_work.obj_work.spd_m, sSpd);
        }
    }

    private static void gmPlySeqGmkMoveGearAnimeSpeedSetWalk(
        AppMain.GMS_PLAYER_WORK ply_work,
        int spd_set)
    {
        int a;
        if (19 <= ply_work.act_state && ply_work.act_state <= 21)
        {
            a = AppMain.MTM_MATH_ABS((spd_set >> 3) + (spd_set >> 2));
            if (a <= 2048)
                a = 2048;
            if (a >= 32768)
                a = 32768;
        }
        else
            a = 4096;

        if (ply_work.obj_work.obj_3d == null)
            return;
        ply_work.obj_work.obj_3d.speed[0] = AppMain.FXM_FX32_TO_FLOAT(a);
        ply_work.obj_work.obj_3d.speed[1] = AppMain.FXM_FX32_TO_FLOAT(a);
    }

    private static void gmPlySeqGmkMainSteamPipe(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.user_timer >= 245760)
            return;
        ply_work.obj_work.user_timer = AppMain.ObjTimeCountUp(ply_work.obj_work.user_timer);
        float num = (float)((double)AppMain.FXM_FX32_TO_FLOAT(ply_work.obj_work.user_timer) / 60.0 * 2.0);
        ply_work.obj_work.obj_3d.speed[0] = 1f + num;
        ply_work.obj_work.obj_3d.speed[1] = 1f + num;
    }

    private static void gmPlySeqGmkMainDrainTank(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
        {
            AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
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

    private static void gmPlySeqGmkMainBoss5Quake(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.obj_work.user_timer > 0)
        {
            --ply_work.obj_work.user_timer;
        }
        else
        {
            AppMain.GmPlySeqLandingSet(ply_work, (ushort)0);
            ply_work.obj_work.move_flag &= 4294959103U;
            AppMain.GmPlySeqChangeSequence(ply_work, 0);
        }
    }

    private static void gmPlySeqGmkMainEndingFrontSide(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 77 || ((int)ply_work.obj_work.disp_flag & 8) == 0)
            return;
        AppMain.GmPlayerActionChange(ply_work, 78);
        ply_work.obj_work.disp_flag |= 4U;
    }

    private static void gmPlySeqGmkMainEndingFinish(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.act_state != 80 && ply_work.act_state != 82 && ply_work.act_state != 84)
        {
            int num = (int)ply_work.obj_work.user_work + 4;
            ply_work.obj_work.user_work = (uint)num;
            ply_work.obj_work.scale.x += (int)ply_work.obj_work.user_work;
            ply_work.obj_work.scale.y += (int)ply_work.obj_work.user_work;
            ply_work.obj_work.scale.z += (int)ply_work.obj_work.user_work;
            ply_work.obj_work.pos.z += 1024;
        }

        if (ply_work.act_state == 39 && ply_work.obj_work.spd.y > -4096)
        {
            if (((int)ply_work.player_flag & 16384) != 0)
                AppMain.GmPlayerActionChange(ply_work, 83);
            else if (ply_work.obj_work.user_flag != 0U)
                AppMain.GmPlayerActionChange(ply_work, 81);
            else
                AppMain.GmPlayerActionChange(ply_work, 79);
        }
        else
        {
            if (ply_work.act_state != 79 && ply_work.act_state != 81 && ply_work.act_state != 83 ||
                ((int)ply_work.obj_work.disp_flag & 8) == 0)
                return;
            if (((int)ply_work.player_flag & 16384) != 0)
                AppMain.GmPlayerActionChange(ply_work, 84);
            else if (ply_work.obj_work.user_flag != 0U)
                AppMain.GmPlayerActionChange(ply_work, 82);
            else
                AppMain.GmPlayerActionChange(ply_work, 80);
            ply_work.obj_work.move_flag |= 8192U;
            ply_work.obj_work.disp_flag |= 32U;
            AppMain.GmEndingTrophySet();
        }
    }

    private static uint gmPlayerCheckTruckAirFoot(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_COL_CHK_DATA pData = AppMain.GlobalPool<AppMain.OBS_COL_CHK_DATA>.Alloc();
        uint num1 = 0;
        int num2 = 0;
        int num3 = 0;
        if (ply_work.obj_work.ride_obj != null)
            return num1;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)ply_work;
        ushort num4 = (ushort)((uint)obsObjectWork.dir.z + (uint)obsObjectWork.dir_fall);
        pData.flag = (ushort)(obsObjectWork.flag & 1U);
        pData.vec = (ushort)2;
        pData.dir = (ushort[])null;
        pData.attr = (uint[])null;
        switch ((ushort)((uint)(ushort)((uint)(ushort)(((int)obsObjectWork.dir.z + 8192 & 49152) >> 14) +
                                           (uint)(ushort)(((int)obsObjectWork.dir_fall + 8192 & 49152) >> 14)) & 3U))
        {
            case 0:
                pData.vec = (ushort)2;
                break;
            case 1:
                pData.vec = (ushort)1;
                break;
            case 2:
                pData.vec = (ushort)3;
                break;
            case 3:
                pData.vec = (ushort)0;
                break;
        }

        if (((int)num4 & 16383) != 0)
        {
            AppMain.NNS_VECTOR nnsVector1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            AppMain.NNS_VECTOR nnsVector2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            AppMain.NNS_VECTOR nnsVector3 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            AppMain.NNS_VECTOR nnsVector4 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            AppMain.NNS_VECTOR nnsVector5 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            AppMain.NNS_VECTOR vec2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            switch (((int)num4 & 49152) >> 14)
            {
                case 0:
                    num2 = (int)obsObjectWork.field_rect[0] - 2;
                    num3 = (int)obsObjectWork.field_rect[3] + 2;
                    break;
                case 1:
                    num2 = (int)obsObjectWork.field_rect[0] - 2;
                    num3 = (int)obsObjectWork.field_rect[1] - 2;
                    break;
                case 2:
                    num2 = (int)obsObjectWork.field_rect[2] + 2;
                    num3 = (int)obsObjectWork.field_rect[1] - 2;
                    break;
                case 3:
                    num2 = (int)obsObjectWork.field_rect[2] + 2;
                    num3 = (int)obsObjectWork.field_rect[3] + 2;
                    break;
            }

            nnsVector1.x = (float)num2;
            nnsVector1.y = (float)-num3;
            nnsVector1.z = 0.0f;
            nnsVector2.x = (float)num2 + 10f * AppMain.nnCos((int)-num4);
            nnsVector2.y = (float)-num3 + 10f * AppMain.nnSin((int)-num4);
            nnsVector2.z = 0.0f;
            nnsVector3.x = nnsVector3.y = nnsVector3.z = 0.0f;
            nnsVector5.x = nnsVector2.x - nnsVector1.x;
            nnsVector5.y = nnsVector2.y - nnsVector1.y;
            nnsVector5.z = nnsVector2.z - nnsVector1.z;
            vec2.x = nnsVector3.x - nnsVector1.x;
            vec2.y = nnsVector3.y - nnsVector1.y;
            vec2.z = nnsVector3.z - nnsVector1.z;
            float num5 = AppMain.nnDotProductVector(nnsVector5, vec2) /
                         AppMain.nnDotProductVector(nnsVector5, nnsVector5);
            nnsVector4.x = nnsVector1.x + nnsVector5.x * num5;
            nnsVector4.y = nnsVector1.y + nnsVector5.y * num5;
            nnsVector4.z = nnsVector1.z + nnsVector5.z * num5;
            num2 = AppMain.FXM_FLOAT_TO_FX32(nnsVector4.x);
            num3 = AppMain.FXM_FLOAT_TO_FX32(-nnsVector4.y);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector1);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector2);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector3);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector4);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector5);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(vec2);
        }
        else
        {
            switch (((int)num4 & 49152) >> 14)
            {
                case 0:
                    num2 = 0;
                    num3 = (int)obsObjectWork.field_rect[3] << 12;
                    break;
                case 1:
                    num2 = (int)-obsObjectWork.field_rect[3] << 12;
                    num3 = 0;
                    break;
                case 2:
                    num2 = 0;
                    num3 = (int)-obsObjectWork.field_rect[3] << 12;
                    break;
                case 3:
                    num2 = (int)obsObjectWork.field_rect[3] << 12;
                    num3 = 0;
                    break;
            }
        }

        int fx32_1 = AppMain.FXM_FLOAT_TO_FX32((float)obsObjectWork.field_rect[2] * AppMain.nnCos((int)num4));
        int fx32_2 = AppMain.FXM_FLOAT_TO_FX32((float)obsObjectWork.field_rect[2] * AppMain.nnSin((int)num4));
        pData.pos_x = num2 + fx32_1 + obsObjectWork.pos.x >> 12;
        pData.pos_y = num3 + fx32_2 + obsObjectWork.pos.y >> 12;
        if (AppMain.ObjDiffCollision(pData) <= 2)
            num1 |= 1U;
        int fx32_3 = AppMain.FXM_FLOAT_TO_FX32((float)obsObjectWork.field_rect[0] * AppMain.nnCos((int)num4));
        int fx32_4 = AppMain.FXM_FLOAT_TO_FX32((float)obsObjectWork.field_rect[0] * AppMain.nnSin((int)num4));
        pData.pos_x = num2 + fx32_3 + obsObjectWork.pos.x >> 12;
        pData.pos_y = num3 + fx32_4 + obsObjectWork.pos.y >> 12;
        if (AppMain.ObjDiffCollision(pData) <= 2)
            num1 |= 2U;
        return num1;
    }

    private static void gmPlySeqGmkMainTruckDanger(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        if (((int)ply_work.player_flag & 2) == 0)
        {
            if (AppMain.MTM_MATH_ABS(ply_work.gmk_work3) < 6144)
            {
                ply_work.gmk_work3 += ply_work.obj_work.user_timer;
                if (((int)ply_work.player_flag & 4) == 0)
                {
                    if (ply_work.gmk_work3 > 6144)
                        ply_work.gmk_work3 = 6144;
                    ply_work.obj_work.user_timer = AppMain.ObjSpdUpSet(ply_work.obj_work.user_timer, 32, 1024);
                }
                else
                {
                    if (ply_work.gmk_work3 < -6144)
                        ply_work.gmk_work3 = -6144;
                    ply_work.obj_work.user_timer = AppMain.ObjSpdUpSet(ply_work.obj_work.user_timer, -32, 1024);
                }
            }
            else
            {
                if (ply_work.act_state != 74 && ply_work.act_state != 75)
                    AppMain.GmPlayerActionChange(ply_work, 74);
                if ((int)ply_work.obj_work.user_work < 3)
                {
                    int num = (int)ply_work.obj_work.user_work + 1;
                    ply_work.obj_work.user_work = (uint)num;
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
            ply_work.gmk_work2 = AppMain.ObjTimeCountDown(ply_work.gmk_work2);
            if (ply_work.gmk_work2 != 0)
                ply_work.gmk_work1 = (int)(ushort)(ply_work.gmk_work1 + ply_work.gmk_work0);
            if (((int)ply_work.obj_work.disp_flag & 8) != 0)
            {
                AppMain.GmPlayerActionChange(ply_work, 75);
                ply_work.obj_work.disp_flag |= 4U;
                ply_work.gmk_flag |= 1073741824U;
                ply_work.obj_work.vib_timer = ply_work.fall_timer;
            }
        }
        else if (((int)ply_work.gmk_flag & 1073741824) != 0)
        {
            ply_work.gmk_work1 = (int)(ushort)((uint)(32768 - (int)ply_work.obj_work.dir.z - ply_work.gmk_work3) +
                                                 (uint)(short)((int)AppMain.g_gm_main_system.pseudofall_dir -
                                                                 (int)ply_work.obj_work.dir_fall));
            if (((int)ply_work.player_flag & 1) != 0)
                AppMain.GmPlySeqGmkInitTruckDangerRet(ply_work, ply_work.truck_obj);
        }

        if (((int)ply_work.gmk_flag & 1073741824) == 0 && ply_work.act_state == 75 &&
            ((int)ply_work.player_flag & 2) != 0)
            ply_work.gmk_flag |= 1073741824U;
        AppMain.nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
        AppMain.nnTranslateMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, 0.0f, 5f, 9f);
        AppMain.nnRotateXMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, ply_work.gmk_work1);
        AppMain.nnTranslateMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, -0.0f, -5f, -9f);
        float x;
        float y;
        float z;
        if (((int)ply_work.player_flag & 4) == 0)
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

        AppMain.nnMakeUnitMatrix(nnsMatrix);
        AppMain.nnTranslateMatrix(nnsMatrix, nnsMatrix, -x, -y, -z);
        AppMain.nnRotateXMatrix(nnsMatrix, nnsMatrix, ply_work.gmk_work3);
        AppMain.nnRotateYMatrix(nnsMatrix, nnsMatrix, AppMain.MTM_MATH_ABS(ply_work.gmk_work3) >> 2);
        AppMain.nnRotateZMatrix(nnsMatrix, nnsMatrix, AppMain.MTM_MATH_ABS(ply_work.gmk_work3) >> 2);
        AppMain.nnTranslateMatrix(nnsMatrix, nnsMatrix, x, y, z);
        AppMain.nnMultiplyMatrix(ply_work.ex_obj_mtx_r, nnsMatrix, ply_work.ex_obj_mtx_r);
    }

    private static void gmPlySeqGmkMainTruckDangerRet(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        ply_work.gmk_work2 = AppMain.ObjTimeCountUp(ply_work.gmk_work2);
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

        if (((int)ply_work.player_flag & 2) != 0)
        {
            ply_work.gmk_work3 = AppMain.ObjSpdDownSet(ply_work.gmk_work3, 1024);
            if (AppMain.MTM_MATH_ABS(ply_work.gmk_work3) == 0)
            {
                ply_work.gmk_work3 = 0;
                ply_work.gmk_flag |= 2147483648U;
                AppMain.GmPlySeqChangeFw(ply_work);
                return;
            }
        }
        else if (((int)ply_work.obj_work.disp_flag & 8) != 0)
        {
            ply_work.gmk_work1 = 0;
            if (ply_work.gmk_work3 != 0)
            {
                ply_work.player_flag |= 2U;
                ply_work.gmk_flag &= 4293918719U;
                AppMain.GmPlayerActionChange(ply_work, 69);
                ply_work.obj_work.disp_flag |= 4U;
            }
            else
            {
                ply_work.gmk_flag |= 2147483648U;
                AppMain.GmPlySeqChangeFw(ply_work);
                return;
            }
        }

        AppMain.nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
        AppMain.nnTranslateMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, 0.0f, 5f, 9f);
        AppMain.nnRotateXMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, (int)(ushort)ply_work.gmk_work1);
        AppMain.nnTranslateMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, -0.0f, -5f, -9f);
        float x;
        float y;
        float z;
        if (((int)ply_work.player_flag & 4) == 0)
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

        AppMain.nnMakeUnitMatrix(nnsMatrix);
        AppMain.nnTranslateMatrix(nnsMatrix, nnsMatrix, -x, -y, -z);
        AppMain.nnRotateXMatrix(nnsMatrix, nnsMatrix, ply_work.gmk_work3);
        AppMain.nnRotateZMatrix(nnsMatrix, nnsMatrix, AppMain.MTM_MATH_ABS(ply_work.gmk_work3) >> 2);
        AppMain.nnTranslateMatrix(nnsMatrix, nnsMatrix, x, y, z);
        AppMain.nnMultiplyMatrix(ply_work.ex_obj_mtx_r, nnsMatrix, ply_work.ex_obj_mtx_r);
    }
}