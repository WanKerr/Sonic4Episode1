using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    private static void gmGmkSlot_ReelControl(AppMain.GMS_GMK_SLOT_REEL_STATUS_WORK preel)
    {
        if (preel.reel_time > 0)
            --preel.reel_time;
        if (preel.reel_time <= 0 && preel.reel_acc != 0)
        {
            preel.reel_spd += preel.reel_acc;
            if (preel.reel_spd > (int)AppMain.GMD_GMK_SLOT_REEL_MAX_SPEED)
            {
                preel.reel_spd = (int)AppMain.GMD_GMK_SLOT_REEL_MAX_SPEED;
                preel.reel_acc = 0;
            }
            else if (preel.reel_spd < 0)
            {
                preel.reel_spd = 0;
                preel.reel_acc = 0;
            }
        }
        preel.reel += (short)preel.reel_spd;
    }

    private static void gmGmkSlotStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SLOT_WORK gmsGmkSlotWork = (AppMain.GMS_GMK_SLOT_WORK)obj_work;
        gmsGmkSlotWork.reel_status[0].reel = (short)((int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT * 15);
        AppMain.GMS_GMK_SLOT_REEL_STATUS_WORK reelStatu = gmsGmkSlotWork.reel_status[1];
        int slotReeL1KomaHeight = (int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT;
        reelStatu.reel = (short)0;
        gmsGmkSlotWork.reel_status[2].reel = AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT;
        gmsGmkSlotWork.reel_status[0].reel_spd = 0;
        gmsGmkSlotWork.reel_status[1].reel_spd = 0;
        gmsGmkSlotWork.reel_status[2].reel_spd = 0;
        gmsGmkSlotWork.reel_status[0].reel_acc = 0;
        gmsGmkSlotWork.reel_status[1].reel_acc = 0;
        gmsGmkSlotWork.reel_status[2].reel_acc = 0;
        gmsGmkSlotWork.reel_status[0].reel_time = 0;
        gmsGmkSlotWork.reel_status[1].reel_time = 0;
        gmsGmkSlotWork.reel_status[2].reel_time = 0;
        gmsGmkSlotWork.prob[0] = AppMain.GMD_GMK_SLOT_PROB_JJJ;
        gmsGmkSlotWork.prob[1] = AppMain.GMD_GMK_SLOT_PROB_EEE;
        gmsGmkSlotWork.prob[2] = AppMain.GMD_GMK_SLOT_PROB_SSS;
        gmsGmkSlotWork.prob[4] = AppMain.GMD_GMK_SLOT_PROB_BBB;
        gmsGmkSlotWork.prob[3] = AppMain.GMD_GMK_SLOT_PROB_RRR;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotStay);
    }

    private static void gmGmkSlotStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SLOT_WORK gmsGmkSlotWork = (AppMain.GMS_GMK_SLOT_WORK)obj_work;
        AppMain.rand_result <<= 16;
        AppMain.rand_result += (uint)AppMain.mtMathRand();
        if (AppMain.slot_start_call != gmsGmkSlotWork.slot_id)
            return;
        AppMain.gmGmkSlotGameStart(obj_work);
    }


    private static void gmGmkSlotGameStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SLOT_WORK gmsGmkSlotWork = (AppMain.GMS_GMK_SLOT_WORK)obj_work;
        gmsGmkSlotWork.slot_step = 0;
        gmsGmkSlotWork.current_reel = 0;
        gmsGmkSlotWork.slot_se_timer = 0;
        int num1 = (int)(AppMain.rand_result % 100U) - (int)AppMain.GMD_GMK_SLOT_1ST_LOT;
        if (num1 >= 0)
        {
            int num2 = num1 * 100 / (100 - (int)AppMain.GMD_GMK_SLOT_1ST_LOT);
            int index1;
            for (index1 = 0; index1 < 5; ++index1)
            {
                num2 -= (int)gmsGmkSlotWork.prob[index1];
                if (num2 <= 0)
                {
                    gmsGmkSlotWork.prob[index1] = AppMain.tbl_gmk_slot_prob[index1];
                    break;
                }
                if ((int)gmsGmkSlotWork.prob[index1] < (int)AppMain.tbl_gmk_slot_prob_max[index1])
                {
                    gmsGmkSlotWork.prob[index1] += AppMain.tbl_gmk_slot_prob[index1];
                    if ((int)gmsGmkSlotWork.prob[index1] > (int)AppMain.tbl_gmk_slot_prob_max[index1])
                        gmsGmkSlotWork.prob[index1] = AppMain.tbl_gmk_slot_prob_max[index1];
                }
            }
            gmsGmkSlotWork.lotresult = (short)index1;
            if (index1 != 5)
            {
                gmsGmkSlotWork.reel_status[0].reel_target_mark = gmsGmkSlotWork.reel_status[1].reel_target_mark = gmsGmkSlotWork.reel_status[2].reel_target_mark = index1;
            }
            else
            {
                int num3 = 0;
                for (int index2 = 0; index2 < 3; ++index2)
                {
                    gmsGmkSlotWork.reel_status[index2].reel_target_mark = (int)(AppMain.getRand() % 5U);
                    if (gmsGmkSlotWork.reel_status[index2].reel_target_mark == 4)
                        ++num3;
                }
                int index3 = (int)(AppMain.getRand() % 3U);
                if (num3 <= 1)
                {
                    gmsGmkSlotWork.lotresult = (short)5;
                    if (num3 == 0)
                        gmsGmkSlotWork.reel_status[index3].reel_target_mark = 4;
                }
                else if (num3 >= 2)
                {
                    gmsGmkSlotWork.lotresult = (short)6;
                    if (num3 == 3)
                        gmsGmkSlotWork.reel_status[index3].reel_target_mark = (int)(AppMain.getRand() % 4U);
                }
            }
        }
        else if (num1 < (int)-AppMain.GMD_GMK_SLOT_EGG_LOT)
        {
            gmsGmkSlotWork.lotresult = (short)-1;
            for (int index = 0; index < 3; ++index)
                gmsGmkSlotWork.reel_status[index].reel_target_mark = (int)(AppMain.getRand() % 4U);
            if (gmsGmkSlotWork.reel_status[0].reel_target_mark == gmsGmkSlotWork.reel_status[1].reel_target_mark && gmsGmkSlotWork.reel_status[1].reel_target_mark == gmsGmkSlotWork.reel_status[2].reel_target_mark)
            {
                if (gmsGmkSlotWork.reel_status[0].reel_target_mark == 1)
                {
                    gmsGmkSlotWork.lotresult = (short)8;
                }
                else
                {
                    int num2;
                    do
                    {
                        num2 = (int)(AppMain.getRand() % 4U);
                    }
                    while (gmsGmkSlotWork.reel_status[0].reel_target_mark == num2);
                    gmsGmkSlotWork.reel_status[2].reel_target_mark = num2;
                }
            }
        }
        else
        {
            gmsGmkSlotWork.reel_status[0].reel_target_mark = gmsGmkSlotWork.reel_status[1].reel_target_mark = gmsGmkSlotWork.reel_status[2].reel_target_mark = 1;
            gmsGmkSlotWork.lotresult = (short)8;
        }
        for (int index = 0; index < 3; ++index)
        {
            ushort num2 = (ushort)AppMain.getRand();
            do
            {
                num2 = (ushort)(((uint)num2 + 1U) % (uint)AppMain.GMD_GMK_SLOT_REEL_ALLMARK);
                gmsGmkSlotWork.reel_status[index].reel_target_pos = (int)num2;
            }
            while ((int)AppMain.tbl_gmk_reel_mark[index][(int)AppMain.GMD_GMK_SLOT_REEL_ALLMARK - (int)num2 & 15] != gmsGmkSlotWork.reel_status[index].reel_target_mark);
        }
        gmsGmkSlotWork.freestop = 0;
        AppMain.gmGmkSlotGameStart_100(obj_work);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotGameStart_100);
    }

    private static void gmGmkSlotGameStart_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SLOT_WORK gmsGmkSlotWork = (AppMain.GMS_GMK_SLOT_WORK)obj_work;
        if (gmsGmkSlotWork.timer <= 0)
        {
            AppMain.GMS_GMK_SLOT_REEL_STATUS_WORK reelStatu = gmsGmkSlotWork.reel_status[gmsGmkSlotWork.current_reel];
            switch (gmsGmkSlotWork.slot_step)
            {
                case 0:
                    gmsGmkSlotWork.reel_status[0].reel_time = 0;
                    gmsGmkSlotWork.reel_status[1].reel_time = 15;
                    gmsGmkSlotWork.reel_status[2].reel_time = 30;
                    gmsGmkSlotWork.reel_status[0].reel_acc = (int)AppMain.GMD_GMK_SLOT_REEL_ACC;
                    gmsGmkSlotWork.reel_status[1].reel_acc = (int)AppMain.GMD_GMK_SLOT_REEL_ACC;
                    gmsGmkSlotWork.reel_status[2].reel_acc = (int)AppMain.GMD_GMK_SLOT_REEL_ACC;
                    gmsGmkSlotWork.timer = 10;
                    gmsGmkSlotWork.timer += (int)AppMain.mtMathRand() % 1;
                    gmsGmkSlotWork.slot_step = 10;
                    break;
                case 10:
                    gmsGmkSlotWork.current_reel = 0;
                    gmsGmkSlotWork.slot_step = 40;
                    gmsGmkSlotWork.timer_next = 0;
                    gmsGmkSlotWork.freestop = 1;
                    gmsGmkSlotWork.timer = 0;
                    break;
                case 20:
                    gmsGmkSlotWork.current_reel = 1;
                    gmsGmkSlotWork.slot_step = 40;
                    gmsGmkSlotWork.timer_next = 0;
                    gmsGmkSlotWork.freestop = 1;
                    gmsGmkSlotWork.timer = 0;
                    break;
                case 30:
                    gmsGmkSlotWork.current_reel = 2;
                    if (gmsGmkSlotWork.timer < 0)
                    {
                        gmsGmkSlotWork.slot_step = 80;
                        break;
                    }
                    if (gmsGmkSlotWork.freestop != 0)
                    {
                        if (gmsGmkSlotWork.lotresult < (short)0 || gmsGmkSlotWork.lotresult == (short)5 || gmsGmkSlotWork.lotresult == (short)6)
                        {
                            gmsGmkSlotWork.timer = 0;
                            gmsGmkSlotWork.slot_step = 40;
                            break;
                        }
                        gmsGmkSlotWork.timer = 60;
                        gmsGmkSlotWork.slot_step = gmsGmkSlotWork.lotresult == (short)8 ? 60 : 50;
                        break;
                    }
                    gmsGmkSlotWork.timer = 0;
                    gmsGmkSlotWork.slot_step = 40;
                    break;
                case 40:
                    if (gmsGmkSlotWork.freestop == 0)
                    {
                        reelStatu.reel_spd = (int)AppMain.GMD_GMK_SLOT_REEL_MIN_SPEED;
                        reelStatu.reel_acc = 0;
                        gmsGmkSlotWork.timer = 0;
                        gmsGmkSlotWork.slot_step = 71;
                        break;
                    }
                    reelStatu.reel_acc = (int)-AppMain.GMD_GMK_SLOT_REEL_BRAKE;
                    ++gmsGmkSlotWork.slot_step;
                    goto case 41;
                case 41:
                    if (reelStatu.reel_spd <= (int)AppMain.GMD_GMK_SLOT_REEL_MIN_SPEED + reelStatu.reel_acc)
                    {
                        reelStatu.reel_spd = (int)AppMain.GMD_GMK_SLOT_REEL_MIN_SPEED;
                        reelStatu.reel_acc = 0;
                        gmsGmkSlotWork.timer = 0;
                        gmsGmkSlotWork.slot_step = 70;
                        break;
                    }
                    break;
                case 50:
                    reelStatu.reel_acc = (int)-AppMain.GMD_GMK_SLOT_REEL_BRAKE / 2;
                    ++gmsGmkSlotWork.slot_step;
                    goto case 51;
                case 51:
                    if (reelStatu.reel_spd <= (int)AppMain.GMD_GMK_SLOT_REEL_MIN_SPEED + reelStatu.reel_acc)
                    {
                        reelStatu.reel_spd = (int)AppMain.GMD_GMK_SLOT_REEL_MIN_SPEED;
                        reelStatu.reel_acc = 0;
                        gmsGmkSlotWork.timer = 30;
                        gmsGmkSlotWork.slot_step = 70;
                        break;
                    }
                    break;
                case 60:
                    reelStatu.reel_acc = (int)-AppMain.GMD_GMK_SLOT_REEL_BRAKE / 2;
                    ++gmsGmkSlotWork.slot_step;
                    goto case 61;
                case 61:
                    if (reelStatu.reel_spd <= (int)AppMain.GMD_GMK_SLOT_REEL_EGG_SPEED + reelStatu.reel_acc)
                    {
                        reelStatu.reel_spd = (int)AppMain.GMD_GMK_SLOT_REEL_EGG_SPEED;
                        reelStatu.reel_acc = 0;
                        ++gmsGmkSlotWork.slot_step;
                        gmsGmkSlotWork.suberi_cnt = 0;
                        gmsGmkSlotWork.suberi_input = 0;
                        break;
                    }
                    break;
                case 62:
                    int num1 = (int)(ushort)reelStatu.reel / (int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT % (int)AppMain.GMD_GMK_SLOT_REEL_ALLMARK;
                    if (num1 == reelStatu.reel_target_pos)
                    {
                        gmsGmkSlotWork.slot_step = 69;
                        reelStatu.reel = (short)((int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT * num1);
                        gmsGmkSlotWork.lotresult = (short)1;
                        break;
                    }
                    break;
                case 69:
                    reelStatu.reel_extime = 8;
                    AppMain.GmSoundPlaySE("Casino5");
                    gmsGmkSlotWork.slot_step = 72;
                    goto case 72;
                case 70:
                    if ((int)(ushort)reelStatu.reel / (int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT == (int)(ushort)((uint)reelStatu.reel - (uint)reelStatu.reel_spd) / (int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT)
                        break;
                    goto case 71;
                case 71:
                    int num2 = (int)(ushort)reelStatu.reel / (int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT % (int)AppMain.GMD_GMK_SLOT_REEL_ALLMARK;
                    if (num2 == reelStatu.reel_target_pos)
                    {
                        reelStatu.reel = (short)((int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT * num2);
                        goto case 69;
                    }
                    else
                        break;
                case 72:
                    if ((reelStatu.reel_extime & 1) == 0)
                    {
                        if (reelStatu.reel_extime == 4)
                            reelStatu.reel_spd /= 2;
                        reelStatu.reel_spd = -reelStatu.reel_spd;
                    }
                    --reelStatu.reel_extime;
                    if (reelStatu.reel_extime == 0)
                    {
                        reelStatu.reel = (short)((int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT * reelStatu.reel_target_pos);
                        reelStatu.reel_spd = 0;
                        if (gmsGmkSlotWork.current_reel == 0)
                        {
                            gmsGmkSlotWork.timer = gmsGmkSlotWork.timer_next;
                            gmsGmkSlotWork.timer += (int)AppMain.mtMathRand() % 1;
                            gmsGmkSlotWork.slot_step = 20;
                            break;
                        }
                        if (gmsGmkSlotWork.current_reel == 1)
                        {
                            gmsGmkSlotWork.timer = gmsGmkSlotWork.timer_next;
                            gmsGmkSlotWork.timer += (int)AppMain.mtMathRand() % 1;
                            gmsGmkSlotWork.slot_step = 30;
                            break;
                        }
                        if (gmsGmkSlotWork.current_reel == 2)
                        {
                            if (gmsGmkSlotWork.lotresult == (short)9)
                            {
                                if (gmsGmkSlotWork.reel_status[0].reel_target_mark == gmsGmkSlotWork.reel_status[1].reel_target_mark && gmsGmkSlotWork.reel_status[0].reel_target_mark == gmsGmkSlotWork.reel_status[2].reel_target_mark)
                                {
                                    gmsGmkSlotWork.lotresult = (short)gmsGmkSlotWork.reel_status[0].reel_target_mark;
                                }
                                else
                                {
                                    int num3 = 5;
                                    for (int index = 0; index < 3; ++index)
                                    {
                                        if (gmsGmkSlotWork.reel_status[index].reel_target_mark == 4)
                                        {
                                            gmsGmkSlotWork.lotresult = (short)num3;
                                            ++num3;
                                        }
                                    }
                                }
                            }
                            obj_work.ppFunc = gmsGmkSlotWork.lotresult < (short)0 || gmsGmkSlotWork.lotresult == (short)9 ? new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotGameLose) : new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotGameHit);
                            break;
                        }
                        break;
                    }
                    break;
                case 80:
                    if ((int)(ushort)reelStatu.reel / (int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT != (int)(ushort)((uint)reelStatu.reel - (uint)reelStatu.reel_spd) / (int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT)
                    {
                        reelStatu.reel &= (short)-4096;
                        reelStatu.reel_target_pos = (int)reelStatu.reel;
                        int index = 16 - (int)(ushort)reelStatu.reel / (int)AppMain.GMD_GMK_SLOT_REEL1KOMA_HEIGHT & 15;
                        reelStatu.reel_target_mark = (int)AppMain.tbl_gmk_reel_mark[gmsGmkSlotWork.current_reel][index];
                        reelStatu.reel_extime = 8;
                        reelStatu.reel_spd = (int)AppMain.GMD_GMK_SLOT_REEL_MIN_SPEED;
                        AppMain.GmSoundPlaySE("Casino5");
                        gmsGmkSlotWork.slot_step = 81;
                        goto case 81;
                    }
                    else
                        break;
                case 81:
                    if ((reelStatu.reel_extime & 1) == 0)
                    {
                        if (reelStatu.reel_extime == 4)
                            reelStatu.reel_spd /= 2;
                        reelStatu.reel_spd = -reelStatu.reel_spd;
                    }
                    --reelStatu.reel_extime;
                    if (reelStatu.reel_extime == 0)
                    {
                        reelStatu.reel = (short)reelStatu.reel_target_pos;
                        reelStatu.reel_spd = 0;
                        if (gmsGmkSlotWork.current_reel == 0)
                        {
                            gmsGmkSlotWork.timer = gmsGmkSlotWork.timer_next;
                            gmsGmkSlotWork.timer += (int)AppMain.mtMathRand() % 1;
                            gmsGmkSlotWork.slot_step = 20;
                            break;
                        }
                        if (gmsGmkSlotWork.current_reel == 1)
                        {
                            gmsGmkSlotWork.timer = gmsGmkSlotWork.timer_next;
                            gmsGmkSlotWork.timer += (int)AppMain.mtMathRand() % 1;
                            gmsGmkSlotWork.slot_step = 30;
                            break;
                        }
                        if (gmsGmkSlotWork.current_reel == 2)
                        {
                            if (gmsGmkSlotWork.lotresult == (short)9)
                            {
                                if (gmsGmkSlotWork.reel_status[0].reel_target_mark == gmsGmkSlotWork.reel_status[1].reel_target_mark && gmsGmkSlotWork.reel_status[0].reel_target_mark == gmsGmkSlotWork.reel_status[2].reel_target_mark)
                                {
                                    gmsGmkSlotWork.lotresult = (short)gmsGmkSlotWork.reel_status[0].reel_target_mark;
                                }
                                else
                                {
                                    int num3 = 5;
                                    for (int index = 0; index < 3; ++index)
                                    {
                                        if (gmsGmkSlotWork.reel_status[index].reel_target_mark == 4)
                                        {
                                            gmsGmkSlotWork.lotresult = (short)num3;
                                            ++num3;
                                        }
                                    }
                                }
                            }
                            obj_work.ppFunc = gmsGmkSlotWork.lotresult < (short)0 || gmsGmkSlotWork.lotresult == (short)9 ? new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotGameLose) : new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotGameHit);
                            break;
                        }
                        break;
                    }
                    break;
            }
        }
        else
            --gmsGmkSlotWork.timer;
        if (gmsGmkSlotWork.slot_se_timer > 0)
            --gmsGmkSlotWork.slot_se_timer;
        for (int index = 0; index < 3; ++index)
        {
            AppMain.GMS_GMK_SLOT_REEL_STATUS_WORK reelStatu = gmsGmkSlotWork.reel_status[index];
            if (reelStatu.reel_spd != 0)
            {
                short num = (short)((int)reelStatu.reel + reelStatu.reel_spd);
                if ((int)num / 4096 != (int)reelStatu.reel / 4096 && ((int)num & -4096) != reelStatu.reel_se && gmsGmkSlotWork.slot_se_timer <= 0)
                {
                    AppMain.GmSoundPlaySE("Casino4");
                    gmsGmkSlotWork.slot_se_timer = 3;
                }
                reelStatu.reel_se = (int)num & -4096;
            }
        }
        AppMain.gmGmkSlot_ReelControl(gmsGmkSlotWork.reel_status[0]);
        AppMain.gmGmkSlot_ReelControl(gmsGmkSlotWork.reel_status[1]);
        AppMain.gmGmkSlot_ReelControl(gmsGmkSlotWork.reel_status[2]);
    }

    private static void gmGmkSlotGameHit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SLOT_WORK gmsGmkSlotWork = (AppMain.GMS_GMK_SLOT_WORK)obj_work;
        if (gmsGmkSlotWork.lotresult != (short)1)
        {
            AppMain.GmRingSlotSetNum(AppMain.slot_start_player, AppMain.tbl_slot_bonus_ring[(int)gmsGmkSlotWork.lotresult]);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotGameHit_100);
        }
        else
        {
            AppMain.GmEfctCmnEsCreate(AppMain.slot_start_player.obj_work, 96);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotGameHit_200);
            gmsGmkSlotWork.timer = 30;
        }
    }

    private static void gmGmkSlotGameHit_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.GmRingCheckRestSlotRing() != 0)
            return;
        AppMain.GMS_GMK_SLOT_WORK gmsGmkSlotWork = (AppMain.GMS_GMK_SLOT_WORK)obj_work;
        AppMain.GmPlayerAddScore(AppMain.slot_start_player, AppMain.tbl_slot_bonus_score[(int)gmsGmkSlotWork.lotresult], AppMain.slot_start_player.obj_work.pos.x, AppMain.slot_start_player.obj_work.pos.y);
        AppMain.slot_start_call = -1;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotStay);
    }

    private static void gmGmkSlotGameHit_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SLOT_WORK gmsGmkSlotWork = (AppMain.GMS_GMK_SLOT_WORK)obj_work;
        --gmsGmkSlotWork.timer;
        if (gmsGmkSlotWork.timer > 0)
            return;
        gmsGmkSlotWork.ppos_x = AppMain.slot_start_player.obj_work.pos.x;
        gmsGmkSlotWork.ppos_y = AppMain.slot_start_player.obj_work.pos.y;
        gmsGmkSlotWork.timer = 100;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotGameHit_210);
    }

    private static void gmGmkSlotGameHit_210(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SLOT_WORK gmsGmkSlotWork = (AppMain.GMS_GMK_SLOT_WORK)obj_work;
        AppMain.slot_start_player.obj_work.pos.x = gmsGmkSlotWork.ppos_x + ((int)AppMain.tbl_dam_ofst_xy[gmsGmkSlotWork.timer % 8][0] << 12);
        AppMain.slot_start_player.obj_work.pos.y = gmsGmkSlotWork.ppos_y + ((int)AppMain.tbl_dam_ofst_xy[gmsGmkSlotWork.timer % 8][1] << 12);
        AppMain.GmPlayerRingDec(AppMain.slot_start_player, (short)1);
        if (gmsGmkSlotWork.timer % 12 == 0)
            AppMain.GmSoundPlaySE("Damage2");
        --gmsGmkSlotWork.timer;
        if (gmsGmkSlotWork.timer > 0)
            return;
        AppMain.slot_start_player.obj_work.pos.x = gmsGmkSlotWork.ppos_x;
        AppMain.slot_start_player.obj_work.pos.y = gmsGmkSlotWork.ppos_y;
        AppMain.slot_start_call = -1;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotStay);
    }

    private static void gmGmkSlotGameLose(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SLOT_WORK gmsGmkSlotWork = (AppMain.GMS_GMK_SLOT_WORK)obj_work;
        AppMain.slot_start_call = -1;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotStay);
    }

    private static void gmGmkSlot_CreateReel(AppMain.GMS_GMK_SLOT_WORK pwork)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)pwork;
        for (int index = 0; index < 3; ++index)
        {
            AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SLOTPARTS_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "Gmk_SlotReel");
            AppMain.GMS_GMK_SLOTPARTS_WORK gmkSlotpartsWork = (AppMain.GMS_GMK_SLOTPARTS_WORK)work;
            AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_slot_obj_3d_list[(int)AppMain.tbl_gmk_slot_reelmodel_id[index]], gmkSlotpartsWork.eff_work.obj_3d);
            work.parent_obj = obsObjectWork;
            work.pos.x = obsObjectWork.pos.x + (48 * index - 48) * 4096;
            work.pos.y = obsObjectWork.pos.y;
            work.pos.z = obsObjectWork.pos.z;
            work.obj_3d.drawflag |= 268435456U;
            work.flag &= 4294966271U;
            work.flag |= 2U;
            work.move_flag |= 256U;
            work.disp_flag &= 4294967039U;
            work.disp_flag |= 138412032U;
            work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSlotReel);
            gmkSlotpartsWork.reel_id = index;
            gmkSlotpartsWork.slot_work = pwork;
        }
    }

    private static void gmGmkSlotReel(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SLOTPARTS_WORK gmkSlotpartsWork = (AppMain.GMS_GMK_SLOTPARTS_WORK)obj_work;
        ushort num1 = (ushort)((uint)(ushort)gmkSlotpartsWork.slot_work.reel_status[gmkSlotpartsWork.reel_id].reel >> 12);
        int index = (int)AppMain.tbl_reel_tex_u[(int)num1 / 5];
        float num2 = (float)(((int)AppMain.tbl_reel_tex_v[index][(int)num1 % 5] << 12) + ((int)gmkSlotpartsWork.slot_work.reel_status[gmkSlotpartsWork.reel_id].reel & 4095)) / 32768f;
        float num3 = (float)index / 8f;
        gmkSlotpartsWork.eff_work.obj_3d.draw_state.texoffset[0].v = -num2;
        gmkSlotpartsWork.eff_work.obj_3d.draw_state.texoffset[0].u = num3;
    }

    private static int GmGmkSlotStartRequest(int slot_id, AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (AppMain.slot_start_call != -1)
            return 0;
        AppMain.slot_start_call = slot_id;
        AppMain.slot_start_player = ply_work;
        return 1;
    }

    private static int GmGmkSlotIsStatus(int slot_id)
    {
        if (AppMain.slot_start_call == -1)
            return 1;
        AppMain.UNREFERENCED_PARAMETER((object)slot_id);
        return 0;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSlotInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_SLOT_WORK work = (AppMain.GMS_GMK_SLOT_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SLOT_WORK()), "Gmk_Slot");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_slot_obj_3d_list[3], gmsEnemy3DWork.obj_3d);
        AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(875).pData, 1, 1);
        AppMain.ObjDrawAction3dActionSet3DNNMaterial(gmsEnemy3DWork.obj_3d, 0);
        obj_work.pos.z = -135168;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194308U;
        work.slot_id = (int)eve_rec.left;
        if (AppMain.slot_start_call == 0)
            AppMain.slot_start_call = -1;
        AppMain.gmGmkSlot_CreateReel(work);
        AppMain.gmGmkSlotStart(obj_work);
        return obj_work;
    }

    private static void GmGmkSlotBuild()
    {
        AppMain.gm_gmk_slot_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(873), AppMain.GmGameDatGetGimmickData(874), 0U);
        AppMain.slot_start_call = 0;
    }

    private static void GmGmkSlotFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(873);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_slot_obj_3d_list, gimmickData.file_num);
    }


}