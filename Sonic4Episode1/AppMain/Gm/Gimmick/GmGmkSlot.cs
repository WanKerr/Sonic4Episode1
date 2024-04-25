public partial class AppMain
{
    private static void gmGmkSlot_ReelControl(GMS_GMK_SLOT_REEL_STATUS_WORK preel)
    {
        if (preel.reel_time > 0)
            --preel.reel_time;
        if (preel.reel_time <= 0 && preel.reel_acc != 0)
        {
            preel.reel_spd += preel.reel_acc;
            if (preel.reel_spd > GMD_GMK_SLOT_REEL_MAX_SPEED)
            {
                preel.reel_spd = GMD_GMK_SLOT_REEL_MAX_SPEED;
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

    private static void gmGmkSlotStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SLOT_WORK gmsGmkSlotWork = (GMS_GMK_SLOT_WORK)obj_work;
        gmsGmkSlotWork.reel_status[0].reel = (short)(GMD_GMK_SLOT_REEL1KOMA_HEIGHT * 15);
        GMS_GMK_SLOT_REEL_STATUS_WORK reelStatu = gmsGmkSlotWork.reel_status[1];
        int slotReeL1KomaHeight = GMD_GMK_SLOT_REEL1KOMA_HEIGHT;
        reelStatu.reel = 0;
        gmsGmkSlotWork.reel_status[2].reel = GMD_GMK_SLOT_REEL1KOMA_HEIGHT;
        gmsGmkSlotWork.reel_status[0].reel_spd = 0;
        gmsGmkSlotWork.reel_status[1].reel_spd = 0;
        gmsGmkSlotWork.reel_status[2].reel_spd = 0;
        gmsGmkSlotWork.reel_status[0].reel_acc = 0;
        gmsGmkSlotWork.reel_status[1].reel_acc = 0;
        gmsGmkSlotWork.reel_status[2].reel_acc = 0;
        gmsGmkSlotWork.reel_status[0].reel_time = 0;
        gmsGmkSlotWork.reel_status[1].reel_time = 0;
        gmsGmkSlotWork.reel_status[2].reel_time = 0;
        gmsGmkSlotWork.prob[0] = GMD_GMK_SLOT_PROB_JJJ;
        gmsGmkSlotWork.prob[1] = GMD_GMK_SLOT_PROB_EEE;
        gmsGmkSlotWork.prob[2] = GMD_GMK_SLOT_PROB_SSS;
        gmsGmkSlotWork.prob[4] = GMD_GMK_SLOT_PROB_BBB;
        gmsGmkSlotWork.prob[3] = GMD_GMK_SLOT_PROB_RRR;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotStay);
    }

    private static void gmGmkSlotStay(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SLOT_WORK gmsGmkSlotWork = (GMS_GMK_SLOT_WORK)obj_work;
        rand_result <<= 16;
        rand_result += mtMathRand();
        if (slot_start_call != gmsGmkSlotWork.slot_id)
            return;
        gmGmkSlotGameStart(obj_work);
    }


    private static void gmGmkSlotGameStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SLOT_WORK gmsGmkSlotWork = (GMS_GMK_SLOT_WORK)obj_work;
        gmsGmkSlotWork.slot_step = 0;
        gmsGmkSlotWork.current_reel = 0;
        gmsGmkSlotWork.slot_se_timer = 0;
        int num1 = (int)(rand_result % 100U) - GMD_GMK_SLOT_1ST_LOT;
        if (num1 >= 0)
        {
            int num2 = num1 * 100 / (100 - GMD_GMK_SLOT_1ST_LOT);
            int index1;
            for (index1 = 0; index1 < 5; ++index1)
            {
                num2 -= gmsGmkSlotWork.prob[index1];
                if (num2 <= 0)
                {
                    gmsGmkSlotWork.prob[index1] = tbl_gmk_slot_prob[index1];
                    break;
                }
                if (gmsGmkSlotWork.prob[index1] < tbl_gmk_slot_prob_max[index1])
                {
                    gmsGmkSlotWork.prob[index1] += tbl_gmk_slot_prob[index1];
                    if (gmsGmkSlotWork.prob[index1] > tbl_gmk_slot_prob_max[index1])
                        gmsGmkSlotWork.prob[index1] = tbl_gmk_slot_prob_max[index1];
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
                    gmsGmkSlotWork.reel_status[index2].reel_target_mark = (int)(getRand() % 5U);
                    if (gmsGmkSlotWork.reel_status[index2].reel_target_mark == 4)
                        ++num3;
                }
                int index3 = (int)(getRand() % 3U);
                if (num3 <= 1)
                {
                    gmsGmkSlotWork.lotresult = 5;
                    if (num3 == 0)
                        gmsGmkSlotWork.reel_status[index3].reel_target_mark = 4;
                }
                else if (num3 >= 2)
                {
                    gmsGmkSlotWork.lotresult = 6;
                    if (num3 == 3)
                        gmsGmkSlotWork.reel_status[index3].reel_target_mark = (int)(getRand() % 4U);
                }
            }
        }
        else if (num1 < -GMD_GMK_SLOT_EGG_LOT)
        {
            gmsGmkSlotWork.lotresult = -1;
            for (int index = 0; index < 3; ++index)
                gmsGmkSlotWork.reel_status[index].reel_target_mark = (int)(getRand() % 4U);
            if (gmsGmkSlotWork.reel_status[0].reel_target_mark == gmsGmkSlotWork.reel_status[1].reel_target_mark && gmsGmkSlotWork.reel_status[1].reel_target_mark == gmsGmkSlotWork.reel_status[2].reel_target_mark)
            {
                if (gmsGmkSlotWork.reel_status[0].reel_target_mark == 1)
                {
                    gmsGmkSlotWork.lotresult = 8;
                }
                else
                {
                    int num2;
                    do
                    {
                        num2 = (int)(getRand() % 4U);
                    }
                    while (gmsGmkSlotWork.reel_status[0].reel_target_mark == num2);
                    gmsGmkSlotWork.reel_status[2].reel_target_mark = num2;
                }
            }
        }
        else
        {
            gmsGmkSlotWork.reel_status[0].reel_target_mark = gmsGmkSlotWork.reel_status[1].reel_target_mark = gmsGmkSlotWork.reel_status[2].reel_target_mark = 1;
            gmsGmkSlotWork.lotresult = 8;
        }
        for (int index = 0; index < 3; ++index)
        {
            ushort num2 = (ushort)getRand();
            do
            {
                num2 = (ushort)((num2 + 1U) % (uint)GMD_GMK_SLOT_REEL_ALLMARK);
                gmsGmkSlotWork.reel_status[index].reel_target_pos = num2;
            }
            while (tbl_gmk_reel_mark[index][GMD_GMK_SLOT_REEL_ALLMARK - num2 & 15] != gmsGmkSlotWork.reel_status[index].reel_target_mark);
        }
        gmsGmkSlotWork.freestop = 0;
        gmGmkSlotGameStart_100(obj_work);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotGameStart_100);
    }

    private static void gmGmkSlotGameStart_100(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SLOT_WORK gmsGmkSlotWork = (GMS_GMK_SLOT_WORK)obj_work;
        if (gmsGmkSlotWork.timer <= 0)
        {
            GMS_GMK_SLOT_REEL_STATUS_WORK reelStatu = gmsGmkSlotWork.reel_status[gmsGmkSlotWork.current_reel];
            switch (gmsGmkSlotWork.slot_step)
            {
                case 0:
                    gmsGmkSlotWork.reel_status[0].reel_time = 0;
                    gmsGmkSlotWork.reel_status[1].reel_time = 15;
                    gmsGmkSlotWork.reel_status[2].reel_time = 30;
                    gmsGmkSlotWork.reel_status[0].reel_acc = GMD_GMK_SLOT_REEL_ACC;
                    gmsGmkSlotWork.reel_status[1].reel_acc = GMD_GMK_SLOT_REEL_ACC;
                    gmsGmkSlotWork.reel_status[2].reel_acc = GMD_GMK_SLOT_REEL_ACC;
                    gmsGmkSlotWork.timer = 10;
                    gmsGmkSlotWork.timer += mtMathRand() % 1;
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
                        if (gmsGmkSlotWork.lotresult < 0 || gmsGmkSlotWork.lotresult == 5 || gmsGmkSlotWork.lotresult == 6)
                        {
                            gmsGmkSlotWork.timer = 0;
                            gmsGmkSlotWork.slot_step = 40;
                            break;
                        }
                        gmsGmkSlotWork.timer = 60;
                        gmsGmkSlotWork.slot_step = gmsGmkSlotWork.lotresult == 8 ? 60 : 50;
                        break;
                    }
                    gmsGmkSlotWork.timer = 0;
                    gmsGmkSlotWork.slot_step = 40;
                    break;
                case 40:
                    if (gmsGmkSlotWork.freestop == 0)
                    {
                        reelStatu.reel_spd = GMD_GMK_SLOT_REEL_MIN_SPEED;
                        reelStatu.reel_acc = 0;
                        gmsGmkSlotWork.timer = 0;
                        gmsGmkSlotWork.slot_step = 71;
                        break;
                    }
                    reelStatu.reel_acc = -GMD_GMK_SLOT_REEL_BRAKE;
                    ++gmsGmkSlotWork.slot_step;
                    goto case 41;
                case 41:
                    if (reelStatu.reel_spd <= GMD_GMK_SLOT_REEL_MIN_SPEED + reelStatu.reel_acc)
                    {
                        reelStatu.reel_spd = GMD_GMK_SLOT_REEL_MIN_SPEED;
                        reelStatu.reel_acc = 0;
                        gmsGmkSlotWork.timer = 0;
                        gmsGmkSlotWork.slot_step = 70;
                        break;
                    }
                    break;
                case 50:
                    reelStatu.reel_acc = -GMD_GMK_SLOT_REEL_BRAKE / 2;
                    ++gmsGmkSlotWork.slot_step;
                    goto case 51;
                case 51:
                    if (reelStatu.reel_spd <= GMD_GMK_SLOT_REEL_MIN_SPEED + reelStatu.reel_acc)
                    {
                        reelStatu.reel_spd = GMD_GMK_SLOT_REEL_MIN_SPEED;
                        reelStatu.reel_acc = 0;
                        gmsGmkSlotWork.timer = 30;
                        gmsGmkSlotWork.slot_step = 70;
                        break;
                    }
                    break;
                case 60:
                    reelStatu.reel_acc = -GMD_GMK_SLOT_REEL_BRAKE / 2;
                    ++gmsGmkSlotWork.slot_step;
                    goto case 61;
                case 61:
                    if (reelStatu.reel_spd <= GMD_GMK_SLOT_REEL_EGG_SPEED + reelStatu.reel_acc)
                    {
                        reelStatu.reel_spd = GMD_GMK_SLOT_REEL_EGG_SPEED;
                        reelStatu.reel_acc = 0;
                        ++gmsGmkSlotWork.slot_step;
                        gmsGmkSlotWork.suberi_cnt = 0;
                        gmsGmkSlotWork.suberi_input = 0;
                        break;
                    }
                    break;
                case 62:
                    int num1 = (ushort)reelStatu.reel / GMD_GMK_SLOT_REEL1KOMA_HEIGHT % GMD_GMK_SLOT_REEL_ALLMARK;
                    if (num1 == reelStatu.reel_target_pos)
                    {
                        gmsGmkSlotWork.slot_step = 69;
                        reelStatu.reel = (short)(GMD_GMK_SLOT_REEL1KOMA_HEIGHT * num1);
                        gmsGmkSlotWork.lotresult = 1;
                        break;
                    }
                    break;
                case 69:
                    reelStatu.reel_extime = 8;
                    GmSoundPlaySE("Casino5");
                    gmsGmkSlotWork.slot_step = 72;
                    goto case 72;
                case 70:
                    if ((ushort)reelStatu.reel / GMD_GMK_SLOT_REEL1KOMA_HEIGHT == (ushort)((uint)reelStatu.reel - (uint)reelStatu.reel_spd) / GMD_GMK_SLOT_REEL1KOMA_HEIGHT)
                        break;
                    goto case 71;
                case 71:
                    int num2 = (ushort)reelStatu.reel / GMD_GMK_SLOT_REEL1KOMA_HEIGHT % GMD_GMK_SLOT_REEL_ALLMARK;
                    if (num2 == reelStatu.reel_target_pos)
                    {
                        reelStatu.reel = (short)(GMD_GMK_SLOT_REEL1KOMA_HEIGHT * num2);
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
                        reelStatu.reel = (short)(GMD_GMK_SLOT_REEL1KOMA_HEIGHT * reelStatu.reel_target_pos);
                        reelStatu.reel_spd = 0;
                        if (gmsGmkSlotWork.current_reel == 0)
                        {
                            gmsGmkSlotWork.timer = gmsGmkSlotWork.timer_next;
                            gmsGmkSlotWork.timer += mtMathRand() % 1;
                            gmsGmkSlotWork.slot_step = 20;
                            break;
                        }
                        if (gmsGmkSlotWork.current_reel == 1)
                        {
                            gmsGmkSlotWork.timer = gmsGmkSlotWork.timer_next;
                            gmsGmkSlotWork.timer += mtMathRand() % 1;
                            gmsGmkSlotWork.slot_step = 30;
                            break;
                        }
                        if (gmsGmkSlotWork.current_reel == 2)
                        {
                            if (gmsGmkSlotWork.lotresult == 9)
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
                            obj_work.ppFunc = gmsGmkSlotWork.lotresult < 0 || gmsGmkSlotWork.lotresult == 9 ? new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotGameLose) : new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotGameHit);
                            break;
                        }
                        break;
                    }
                    break;
                case 80:
                    if ((ushort)reelStatu.reel / GMD_GMK_SLOT_REEL1KOMA_HEIGHT != (ushort)((uint)reelStatu.reel - (uint)reelStatu.reel_spd) / GMD_GMK_SLOT_REEL1KOMA_HEIGHT)
                    {
                        reelStatu.reel &= -4096;
                        reelStatu.reel_target_pos = reelStatu.reel;
                        int index = 16 - (ushort)reelStatu.reel / GMD_GMK_SLOT_REEL1KOMA_HEIGHT & 15;
                        reelStatu.reel_target_mark = tbl_gmk_reel_mark[gmsGmkSlotWork.current_reel][index];
                        reelStatu.reel_extime = 8;
                        reelStatu.reel_spd = GMD_GMK_SLOT_REEL_MIN_SPEED;
                        GmSoundPlaySE("Casino5");
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
                            gmsGmkSlotWork.timer += mtMathRand() % 1;
                            gmsGmkSlotWork.slot_step = 20;
                            break;
                        }
                        if (gmsGmkSlotWork.current_reel == 1)
                        {
                            gmsGmkSlotWork.timer = gmsGmkSlotWork.timer_next;
                            gmsGmkSlotWork.timer += mtMathRand() % 1;
                            gmsGmkSlotWork.slot_step = 30;
                            break;
                        }
                        if (gmsGmkSlotWork.current_reel == 2)
                        {
                            if (gmsGmkSlotWork.lotresult == 9)
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
                            obj_work.ppFunc = gmsGmkSlotWork.lotresult < 0 || gmsGmkSlotWork.lotresult == 9 ? new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotGameLose) : new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotGameHit);
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
            GMS_GMK_SLOT_REEL_STATUS_WORK reelStatu = gmsGmkSlotWork.reel_status[index];
            if (reelStatu.reel_spd != 0)
            {
                short num = (short)(reelStatu.reel + reelStatu.reel_spd);
                if (num / 4096 != reelStatu.reel / 4096 && (num & -4096) != reelStatu.reel_se && gmsGmkSlotWork.slot_se_timer <= 0)
                {
                    GmSoundPlaySE("Casino4");
                    gmsGmkSlotWork.slot_se_timer = 3;
                }
                reelStatu.reel_se = num & -4096;
            }
        }
        gmGmkSlot_ReelControl(gmsGmkSlotWork.reel_status[0]);
        gmGmkSlot_ReelControl(gmsGmkSlotWork.reel_status[1]);
        gmGmkSlot_ReelControl(gmsGmkSlotWork.reel_status[2]);
    }

    private static void gmGmkSlotGameHit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SLOT_WORK gmsGmkSlotWork = (GMS_GMK_SLOT_WORK)obj_work;
        if (gmsGmkSlotWork.lotresult != 1)
        {
            GmRingSlotSetNum(slot_start_player, tbl_slot_bonus_ring[gmsGmkSlotWork.lotresult]);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotGameHit_100);
        }
        else
        {
            GmEfctCmnEsCreate(slot_start_player.obj_work, 96);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotGameHit_200);
            gmsGmkSlotWork.timer = 30;
        }
    }

    private static void gmGmkSlotGameHit_100(OBS_OBJECT_WORK obj_work)
    {
        if (GmRingCheckRestSlotRing() != 0)
            return;
        GMS_GMK_SLOT_WORK gmsGmkSlotWork = (GMS_GMK_SLOT_WORK)obj_work;
        GmPlayerAddScore(slot_start_player, tbl_slot_bonus_score[gmsGmkSlotWork.lotresult], slot_start_player.obj_work.pos.x, slot_start_player.obj_work.pos.y);
        slot_start_call = -1;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotStay);
    }

    private static void gmGmkSlotGameHit_200(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SLOT_WORK gmsGmkSlotWork = (GMS_GMK_SLOT_WORK)obj_work;
        --gmsGmkSlotWork.timer;
        if (gmsGmkSlotWork.timer > 0)
            return;
        gmsGmkSlotWork.ppos_x = slot_start_player.obj_work.pos.x;
        gmsGmkSlotWork.ppos_y = slot_start_player.obj_work.pos.y;
        gmsGmkSlotWork.timer = 100;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotGameHit_210);
    }

    private static void gmGmkSlotGameHit_210(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SLOT_WORK gmsGmkSlotWork = (GMS_GMK_SLOT_WORK)obj_work;
        slot_start_player.obj_work.pos.x = gmsGmkSlotWork.ppos_x + (tbl_dam_ofst_xy[gmsGmkSlotWork.timer % 8][0] << 12);
        slot_start_player.obj_work.pos.y = gmsGmkSlotWork.ppos_y + (tbl_dam_ofst_xy[gmsGmkSlotWork.timer % 8][1] << 12);
        GmPlayerRingDec(slot_start_player, 1);
        if (gmsGmkSlotWork.timer % 12 == 0)
            GmSoundPlaySE("Damage2");
        --gmsGmkSlotWork.timer;
        if (gmsGmkSlotWork.timer > 0)
            return;
        slot_start_player.obj_work.pos.x = gmsGmkSlotWork.ppos_x;
        slot_start_player.obj_work.pos.y = gmsGmkSlotWork.ppos_y;
        slot_start_call = -1;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotStay);
    }

    private static void gmGmkSlotGameLose(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SLOT_WORK gmsGmkSlotWork = (GMS_GMK_SLOT_WORK)obj_work;
        slot_start_call = -1;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotStay);
    }

    private static void gmGmkSlot_CreateReel(GMS_GMK_SLOT_WORK pwork)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)pwork;
        for (int index = 0; index < 3; ++index)
        {
            OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_GMK_SLOTPARTS_WORK(), null, 0, "Gmk_SlotReel");
            GMS_GMK_SLOTPARTS_WORK gmkSlotpartsWork = (GMS_GMK_SLOTPARTS_WORK)work;
            ObjObjectCopyAction3dNNModel(work, gm_gmk_slot_obj_3d_list[tbl_gmk_slot_reelmodel_id[index]], gmkSlotpartsWork.eff_work.obj_3d);
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
            work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkSlotReel);
            gmkSlotpartsWork.reel_id = index;
            gmkSlotpartsWork.slot_work = pwork;
        }
    }

    private static void gmGmkSlotReel(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_SLOTPARTS_WORK gmkSlotpartsWork = (GMS_GMK_SLOTPARTS_WORK)obj_work;
        ushort num1 = (ushort)((uint)(ushort)gmkSlotpartsWork.slot_work.reel_status[gmkSlotpartsWork.reel_id].reel >> 12);
        int index = tbl_reel_tex_u[num1 / 5];
        float num2 = ((tbl_reel_tex_v[index][num1 % 5] << 12) + (gmkSlotpartsWork.slot_work.reel_status[gmkSlotpartsWork.reel_id].reel & 4095)) / 32768f;
        float num3 = index / 8f;
        gmkSlotpartsWork.eff_work.obj_3d.draw_state.texoffset[0].v = -num2;
        gmkSlotpartsWork.eff_work.obj_3d.draw_state.texoffset[0].u = num3;
    }

    private static int GmGmkSlotStartRequest(int slot_id, GMS_PLAYER_WORK ply_work)
    {
        if (slot_start_call != -1)
            return 0;
        slot_start_call = slot_id;
        slot_start_player = ply_work;
        return 1;
    }

    private static int GmGmkSlotIsStatus(int slot_id)
    {
        if (slot_start_call == -1)
            return 1;
        UNREFERENCED_PARAMETER(slot_id);
        return 0;
    }

    private static OBS_OBJECT_WORK GmGmkSlotInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_SLOT_WORK work = (GMS_GMK_SLOT_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_SLOT_WORK(), "Gmk_Slot");
        OBS_OBJECT_WORK obj_work = (OBS_OBJECT_WORK)work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(obj_work, gm_gmk_slot_obj_3d_list[3], gmsEnemy3DWork.obj_3d);
        ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, null, null, 0, (AMS_AMB_HEADER)ObjDataGet(875).pData, 1, 1);
        ObjDrawAction3dActionSet3DNNMaterial(gmsEnemy3DWork.obj_3d, 0);
        obj_work.pos.z = -135168;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194308U;
        work.slot_id = eve_rec.left;
        if (slot_start_call == 0)
            slot_start_call = -1;
        gmGmkSlot_CreateReel(work);
        gmGmkSlotStart(obj_work);
        return obj_work;
    }

    private static void GmGmkSlotBuild()
    {
        gm_gmk_slot_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(873), GmGameDatGetGimmickData(874), 0U);
        slot_start_call = 0;
    }

    private static void GmGmkSlotFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(873);
        GmGameDBuildRegFlushModel(gm_gmk_slot_obj_3d_list, gimmickData.file_num);
    }


}