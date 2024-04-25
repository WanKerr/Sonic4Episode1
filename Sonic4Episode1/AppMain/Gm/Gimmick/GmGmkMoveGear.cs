public partial class AppMain
{
    private static void gmGmkMoveGearFwInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.rect_ret_timer == 0)
        {
            gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag &= 4294965247U;
            gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag &= 4294965247U;
        }
        obj_work.flag &= 4294967279U;
        obj_work.spd_m = 0;
        obj_work.spd.x = obj_work.spd.y = 0;
        obj_work.spd_add.x = obj_work.spd_add.y = 0;
        gmsGmkGearWork.vib_end = false;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkMoveGearFwMain);
    }

    private static void gmGmkMoveGearFwMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.rect_ret_timer == 0)
            return;
        gmsGmkGearWork.rect_ret_timer = ObjTimeCountDown(gmsGmkGearWork.rect_ret_timer);
        if (gmsGmkGearWork.rect_ret_timer != 0)
            return;
        gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag &= 4294965247U;
        gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag &= 4294965247U;
    }

    private static void gmGmkMoveGearMoveInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        obj_work.user_flag &= 4294967294U;
        gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag |= 2048U;
        gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag &= 4294965247U;
        obj_work.flag |= 16U;
        obj_work.move_flag &= 4294959103U;
        gmsGmkGearWork.stop_timer = 737280;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkMoveGearMoveMain);
    }

    private static void gmGmkMoveGearMoveMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        GMS_PLAYER_WORK targetObj = (GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.target_obj;
        if (targetObj == null || targetObj.gmk_obj != obj_work)
        {
            gmsGmkGearWork.gmk_work.ene_com.target_obj = null;
            gmGmkMoveGearRetWaitInit(obj_work);
        }
        else
        {
            var multiplier = 1;
            var options = gs.backup.SSave.CreateInstance();
            if (options.GetRemaster().BetterGears)
            {
                multiplier = 2;
            }

            gmGmkMoveGearSetSpd(obj_work, -targetObj.obj_work.spd_m * multiplier);
            if (obj_work.spd_m != 0 && !gmsGmkGearWork.vib_end)
            {
                GMM_PAD_VIB_SMALL_TIME(60f);
                gmsGmkGearWork.vib_end = true;
            }
            gmsGmkGearWork.stop_timer = targetObj.obj_work.spd_m != 0 ? 737280 : ObjTimeCountDown(gmsGmkGearWork.stop_timer);
            if (gmsGmkGearWork.stop_timer != 0)
                return;
            gmsGmkGearWork.gmk_work.ene_com.enemy_flag |= 1U;
            gmsGmkGearWork.gmk_work.ene_com.target_obj = null;
            gmGmkMoveGearRetInit(obj_work);
        }
    }

    private static void gmGmkMoveGearRetWaitInit(OBS_OBJECT_WORK obj_work)
    {
        ((GMS_GMK_GEAR_WORK)obj_work).rect_ret_timer = 65536;
        obj_work.flag &= 4294967293U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkMoveGearRetWaitMain);
    }

    private static void gmGmkMoveGearRetWaitMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.rect_ret_timer != 0)
        {
            gmsGmkGearWork.rect_ret_timer = ObjTimeCountDown(gmsGmkGearWork.rect_ret_timer);
            if (gmsGmkGearWork.rect_ret_timer == 0)
            {
                gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag &= 4294965247U;
                gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag &= 4294965247U;
            }
        }
        gmGmkMoveGearSetSpd(obj_work, ObjSpdDownSet(obj_work.spd_m, 2024));
        if (obj_work.spd_m != 0)
            return;
        gmsGmkGearWork.stop_timer = ObjTimeCountDown(gmsGmkGearWork.stop_timer);
        if (gmsGmkGearWork.stop_timer != 0)
            return;
        gmGmkMoveGearRetInit(obj_work);
    }

    private static void gmGmkMoveGearRetInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        gmsGmkGearWork.rect_ret_timer = 0;
        if (((int)gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag & 2048) != 0)
            gmsGmkGearWork.rect_ret_timer = 65536;
        if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 && obj_work.pos.x == gmsGmkGearWork.gmk_work.ene_com.born_pos_x || (gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0 && obj_work.pos.y == gmsGmkGearWork.gmk_work.ene_com.born_pos_y)
        {
            gmGmkMoveGearFwInit(obj_work);
        }
        else
        {
            gmsGmkGearWork.ret_max_speed = 0;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkMoveGearRetMain);
        }
    }

    private static void gmGmkMoveGearRetMain(OBS_OBJECT_WORK obj_work)
    {
        var spdAdd = 512;
        var spdMax = 16384;

        var options = gs.backup.SSave.CreateInstance();
        if (options.GetRemaster().BetterGears)
        {
            spdAdd *= 2;
            spdMax *= 2;
        }

        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
        bool flag = false;
        int spd_m = obj_work.spd_m;
        if (gmsGmkGearWork.rect_ret_timer != 0)
        {
            gmsGmkGearWork.rect_ret_timer = ObjTimeCountDown(gmsGmkGearWork.rect_ret_timer);
            if (gmsGmkGearWork.rect_ret_timer == 0)
            {
                gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag &= 4294965247U;
                gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag &= 4294965247U;
            }
        }
        if ((gmsEnemyComWork.eve_rec.flag & 2) != 0)
        {
            if ((gmsEnemyComWork.eve_rec.flag & 1) != 0)
            {
                if (obj_work.pos.y < gmsEnemyComWork.born_pos_y)
                    spd_m = ObjSpdUpSet(-obj_work.spd_m, -spdAdd, spdMax);
                else
                    flag = true;
                if (((int)obj_work.move_flag & 4) != 0)
                    flag = true;
            }
            else
            {
                if (obj_work.pos.x < gmsEnemyComWork.born_pos_x)
                    spd_m = ObjSpdUpSet(obj_work.spd_m, spdAdd, spdMax);
                else
                    flag = true;
                if (((int)obj_work.move_flag & 4) != 0)
                    flag = true;
            }
        }
        else if ((gmsEnemyComWork.eve_rec.flag & 1) != 0)
        {
            if (obj_work.pos.y > gmsEnemyComWork.born_pos_y)
                spd_m = ObjSpdUpSet(-obj_work.spd_m, spdAdd, spdMax);
            else
                flag = true;
            if (((int)obj_work.move_flag & 8) != 0)
                flag = true;
        }
        else
        {
            if (obj_work.pos.x > gmsEnemyComWork.born_pos_x)
                spd_m = ObjSpdUpSet(obj_work.spd_m, -spdAdd, spdMax);
            else
                flag = true;
            if (((int)obj_work.move_flag & 8) != 0)
                flag = true;
        }
        if (MTM_MATH_ABS(obj_work.spd_m) > gmsGmkGearWork.ret_max_speed)
            gmsGmkGearWork.ret_max_speed = MTM_MATH_ABS(obj_work.spd_m);
        if (flag)
        {
            if (gmsGmkGearWork.ret_max_speed >= 14336)
                obj_work.vib_timer = 65536;
            gmGmkMoveGearSetSpd(obj_work, 0);
            gmGmkMoveGearFwInit(obj_work);
        }
        else
            gmGmkMoveGearSetSpd(obj_work, spd_m);
    }

    private static void gmGmkMoveGearSwitchExeInit(
      OBS_OBJECT_WORK obj_work,
      short cam_ofst_x,
      short cam_ofst_y)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
        obj_work.user_flag |= 1U;
        obj_work.flag |= 2U;
        if (gmsEnemyComWork.target_obj != null && gmsEnemyComWork.target_obj.obj_type == 1)
        {
            GMS_PLAYER_WORK targetObj = (GMS_PLAYER_WORK)gmsEnemyComWork.target_obj;
            GmPlayerCameraOffsetSet(targetObj, (short)(targetObj.gmk_camera_center_ofst_x + cam_ofst_x), (short)(targetObj.gmk_camera_center_ofst_y + cam_ofst_y));
        }
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkMoveGearSwitchExeMain);
    }

    private static void gmGmkMoveGearSwitchExeMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
        if (gmsGmkGearWork.sw_gear_obj == null)
        {
            gmGmkMoveGearEndInit(obj_work);
        }
        else
        {
            GMS_GMK_GEAR_WORK swGearObj = (GMS_GMK_GEAR_WORK)gmsGmkGearWork.sw_gear_obj;
            if (swGearObj.open_rot_dist <= 0)
            {
                gmGmkMoveGearSetSpd(obj_work, 0);
                if (swGearObj.gmk_work.ene_com.eve_rec.height == 0)
                    gmsEnemyComWork.eve_rec.byte_param[1] = 1;
                gmGmkMoveGearEndStaggerInit(obj_work);
            }
            else
            {
                GMS_PLAYER_WORK targetObj = (GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.target_obj;
                if (targetObj == null || ((int)targetObj.player_flag & 1024) != 0)
                {
                    gmsGmkGearWork.gmk_work.ene_com.target_obj = null;
                    gmGmkMoveGearRetWaitInit(obj_work);
                }
                else
                {
                    var spdAdd = 256;
                    var spdMax = 16384;

                    var options = gs.backup.SSave.CreateInstance();
                    if (options.GetRemaster().BetterGears)
                    {
                        spdAdd *= 2;
                        spdMax *= 2;
                    }

                    int sSpd = 0;
                    if ((gmsEnemyComWork.eve_rec.flag & 1) != 0)
                    {
                        if ((gmsEnemyComWork.eve_rec.flag & 2) == 0 && targetObj.obj_work.spd_m < spdMax)
                            sSpd = spdAdd;
                        else if ((gmsEnemyComWork.eve_rec.flag & 2) != 0 && targetObj.obj_work.spd_m > -spdMax)
                            sSpd = -spdAdd;
                    }
                    else if ((gmsEnemyComWork.eve_rec.flag & 2) == 0 && targetObj.obj_work.spd_m > -spdMax)
                        sSpd = -spdAdd;
                    else if ((gmsEnemyComWork.eve_rec.flag & 2) != 0 && targetObj.obj_work.spd_m < spdMax)
                        sSpd = spdAdd;

                    if (sSpd != 0)
                        targetObj.obj_work.spd_m = ObjSpdUpSet(targetObj.obj_work.spd_m, sSpd, spdMax);
                    gmGmkMoveGearSetSpd(obj_work, -targetObj.obj_work.spd_m);
                }
            }
        }
    }

    private static void gmGmkMoveGearEndStaggerInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        GMS_PLAYER_WORK targetObj = (GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.target_obj;
        obj_work.user_flag &= 4294967294U;
        obj_work.user_flag |= 2U;
        obj_work.flag |= 2U;
        obj_work.move_flag |= 8448U;
        gmsGmkGearWork.move_stagger_dir_cnt = 0;
        gmsGmkGearWork.move_stagger_dir_spd = targetObj.obj_work.spd_m;
        obj_work.user_timer = gmsGmkGearWork.move_stagger_dir_spd;
        gmsGmkGearWork.move_stagger_step = 0;
        obj_work.user_work = gmsGmkGearWork.move_stagger_step;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkMoveGearEndStaggerMain);
    }

    private static void gmGmkMoveGearEndStaggerMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        GMS_PLAYER_WORK targetObj = (GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.target_obj;
        if (targetObj == null || targetObj.gmk_obj != obj_work)
        {
            obj_work.user_flag &= 4294967293U;
            if (((GMS_GMK_GEAR_WORK)gmsGmkGearWork.sw_gear_obj).gmk_work.ene_com.eve_rec.height == 0)
            {
                gmsGmkGearWork.gmk_work.ene_com.eve_rec.byte_param[1] = 1;
                gmGmkMoveGearEndInit(obj_work);
            }
            else
                gmGmkMoveGearSwitchRetWaitInit(obj_work);
        }
        else
        {
            int num = (gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 ? 96 : -96;
            switch (gmsGmkGearWork.move_stagger_step)
            {
                case 0:
                    gmsGmkGearWork.move_stagger_dir_spd = gmsGmkGearWork.move_stagger_dir_spd * 9 / 10;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (MTM_MATH_ABS(gmsGmkGearWork.move_stagger_dir_spd) <= 128)
                    {
                        ++gmsGmkGearWork.move_stagger_step;
                        gmsGmkGearWork.move_stagger_dir_spd = 0;
                        gmsGmkGearWork.move_stagger_dir_cnt = 0;
                        gmsGmkGearWork.move_draw_dir_limit = gmsGmkGearWork.move_draw_dir_ofst;
                        break;
                    }
                    break;
                case 1:
                    gmsGmkGearWork.move_stagger_dir_spd += num * 3 / 2;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (gmsGmkGearWork.move_stagger_dir_cnt == 0)
                    {
                        if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
                        {
                            if (gmsGmkGearWork.move_draw_dir_ofst < gmsGmkGearWork.move_draw_dir_limit / 2)
                            {
                                gmsGmkGearWork.move_stagger_dir_cnt = 1;
                                break;
                            }
                            break;
                        }
                        if (gmsGmkGearWork.move_draw_dir_ofst > gmsGmkGearWork.move_draw_dir_limit / 2)
                        {
                            gmsGmkGearWork.move_stagger_dir_cnt = 1;
                            break;
                        }
                        break;
                    }
                    ++gmsGmkGearWork.move_stagger_dir_cnt;
                    if (gmsGmkGearWork.move_stagger_dir_cnt == 3)
                    {
                        ++gmsGmkGearWork.move_stagger_step;
                        break;
                    }
                    break;
                case 2:
                    gmsGmkGearWork.move_stagger_dir_spd -= num * 3 / 2;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (MTM_MATH_ABS(gmsGmkGearWork.move_stagger_dir_spd) <= 128)
                    {
                        gmsGmkGearWork.move_stagger_dir_spd = 0;
                        if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0 && GmPlayerKeyCheckWalkRight(targetObj) || (gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 && GmPlayerKeyCheckWalkLeft(targetObj))
                        {
                            ++gmsGmkGearWork.move_stagger_step;
                            break;
                        }
                        gmsGmkGearWork.move_stagger_step = 5;
                        break;
                    }
                    break;
                case 3:
                    gmsGmkGearWork.move_stagger_dir_spd -= num;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (MTM_MATH_ABS(gmsGmkGearWork.move_draw_dir_ofst) > 1820)
                    {
                        ++gmsGmkGearWork.move_stagger_step;
                        break;
                    }
                    break;
                case 4:
                    gmsGmkGearWork.move_stagger_dir_spd += num;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (MTM_MATH_ABS(gmsGmkGearWork.move_stagger_dir_spd) <= 128)
                    {
                        gmsGmkGearWork.move_stagger_step = 1;
                        gmsGmkGearWork.move_stagger_dir_spd = 0;
                        gmsGmkGearWork.move_stagger_dir_cnt = 0;
                        gmsGmkGearWork.move_draw_dir_limit = gmsGmkGearWork.move_draw_dir_ofst;
                        break;
                    }
                    break;
                case 5:
                    gmsGmkGearWork.move_stagger_dir_spd -= num;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (MTM_MATH_ABS(gmsGmkGearWork.move_draw_dir_ofst) <= 309)
                    {
                        ++gmsGmkGearWork.move_stagger_step;
                        break;
                    }
                    break;
                case 6:
                    gmsGmkGearWork.move_stagger_dir_spd += num;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (MTM_MATH_ABS(gmsGmkGearWork.move_stagger_dir_spd) <= 128)
                    {
                        gmsGmkGearWork.move_stagger_dir_spd = 0;
                        gmsGmkGearWork.move_draw_dir_ofst /= 2;
                        ++gmsGmkGearWork.move_stagger_step;
                        break;
                    }
                    break;
                case 7:
                    if (gmsGmkGearWork.move_draw_dir_ofst != 0)
                        gmsGmkGearWork.move_draw_dir_ofst /= 2;
                    if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0 && GmPlayerKeyCheckWalkRight(targetObj) || (gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 && GmPlayerKeyCheckWalkLeft(targetObj))
                    {
                        gmsGmkGearWork.move_stagger_step = 3;
                        gmsGmkGearWork.move_draw_dir_ofst = 0;
                        break;
                    }
                    break;
            }
            obj_work.user_work = gmsGmkGearWork.move_stagger_step;
            obj_work.user_timer = gmsGmkGearWork.move_stagger_dir_spd;
            gmGmkMoveGearSetSpd(obj_work, 0);
        }
    }

    private static void gmGmkMoveGearEndInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        obj_work.flag |= 2U;
        obj_work.move_flag |= 8448U;
        gmsGmkGearWork.gmk_work.ene_com.enemy_flag |= 1U;
        gmsGmkGearWork.gmk_work.ene_com.target_obj = null;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkMoveGearEndMain);
    }

    private static void gmGmkMoveGearEndMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        int moveDrawDirOfst = gmsGmkGearWork.move_draw_dir_ofst;
        gmsGmkGearWork.move_draw_dir_ofst = (short)ObjSpdDownSet(gmsGmkGearWork.move_draw_dir_ofst, 64);
        gmGmkMoveGearSetSpd(obj_work, 0);
    }

    private static void gmGmkMoveGearSwitchRetWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        obj_work.flag |= 18U;
        obj_work.move_flag |= 8448U;
        gmsGmkGearWork.gmk_work.ene_com.enemy_flag |= 1U;
        gmsGmkGearWork.gmk_work.ene_com.target_obj = null;
        obj_work.user_timer = 245760;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkMoveGearSwitchRetWaitMain);
    }

    private static void gmGmkMoveGearSwitchRetWaitMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.sw_gear_obj == null)
        {
            obj_work.flag &= 4294967279U;
            gmGmkMoveGearEndInit(obj_work);
        }
        else
        {
            obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
            if (obj_work.col_work.obj_col.rider_obj == null)
            {
                if (obj_work.user_timer == 0)
                    gmGmkMoveGearSwitchRetInit(obj_work);
            }
            else
                obj_work.user_timer = 245760;
            if (gmsGmkGearWork.move_draw_dir_ofst == 0)
                return;
            gmsGmkGearWork.move_draw_dir_ofst = (short)ObjSpdDownSet(gmsGmkGearWork.move_draw_dir_ofst, 64);
            gmGmkMoveGearSetSpd(obj_work, 0);
        }
    }

    private static void gmGmkMoveGearSwitchRetInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        obj_work.flag &= 4294967293U;
        obj_work.move_flag &= 4294958847U;
        gmsGmkGearWork.gmk_work.ene_com.enemy_flag &= 4294967280U;
        obj_work.user_flag &= 4294967294U;
        gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag |= 2048U;
        gmsGmkGearWork.rect_ret_timer = 0;
        if (((int)gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag & 2048) != 0)
            gmsGmkGearWork.rect_ret_timer = 65536;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkMoveGearSwitchRetMain);
    }

    private static void gmGmkMoveGearSwitchRetMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.sw_gear_obj == null)
        {
            obj_work.flag &= 4294967279U;
            gmGmkMoveGearEndInit(obj_work);
        }
        else
        {
            if (gmsGmkGearWork.rect_ret_timer != 0)
            {
                gmsGmkGearWork.rect_ret_timer = ObjTimeCountDown(gmsGmkGearWork.rect_ret_timer);
                if (gmsGmkGearWork.rect_ret_timer == 0)
                    gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag &= 4294965247U;
            }
            GMS_GMK_GEAR_WORK swGearObj = (GMS_GMK_GEAR_WORK)gmsGmkGearWork.sw_gear_obj;
            if (swGearObj.open_rot_dist >= 65536)
            {
                gmGmkMoveGearSetSpd(obj_work, 0);
                gmGmkMoveGearRetInit(obj_work);
            }
            else
            {
                int num = swGearObj.close_rot_spd;
                if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
                {
                    if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 2) == 0)
                        num = -num;
                }
                else if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 2) != 0)
                    num = -num;
                gmGmkMoveGearSetSpd(obj_work, -num << 5);
                obj_work.spd_m = 0;
            }
        }
    }

    private static bool gmGmkMoveGearCheckSwitchMove(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_type != 3 || ((GMS_ENEMY_COM_WORK)obj_work).eve_rec.id != 182)
            return false;
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
        {
            if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 2) != 0)
            {
                if (obj_work.pos.y <= gmsGmkGearWork.move_end_y)
                    return true;
            }
            else if (obj_work.pos.y >= gmsGmkGearWork.move_end_y)
                return true;
        }
        else if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 2) != 0)
        {
            if (obj_work.pos.x <= gmsGmkGearWork.move_end_x)
                return true;
        }
        else if (obj_work.pos.x >= gmsGmkGearWork.move_end_x)
            return true;
        return false;
    }

    private static void gmGmkMoveGearSetSpd(OBS_OBJECT_WORK obj_work, int spd_m)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        int num1 = spd_m;
        int num2;
        int num3;
        if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
        {
            num2 = gmsGmkGearWork.gmk_work.ene_com.born_pos_y - obj_work.pos.y;
            num3 = gmsGmkGearWork.move_end_y - obj_work.pos.y;
            spd_m = -spd_m;
        }
        else
        {
            num2 = gmsGmkGearWork.gmk_work.ene_com.born_pos_x - obj_work.pos.x;
            num3 = gmsGmkGearWork.move_end_x - obj_work.pos.x;
        }
        if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 2) != 0)
        {
            if (spd_m < 0)
            {
                if (num3 > spd_m)
                    spd_m = num3;
            }
            else if (num2 < spd_m)
                spd_m = num2;
        }
        else if (spd_m > 0)
        {
            if (num3 < spd_m)
                spd_m = num3;
        }
        else if (num2 > spd_m)
            spd_m = num2;
        obj_work.spd_m = spd_m;
        if (spd_m != 0 && (gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
            obj_work.spd.x += 4096;
        gmsGmkGearWork.old_move_draw_dir = gmsGmkGearWork.move_draw_dir;
        gmsGmkGearWork.move_draw_dir_spd = (short)(-num1 >> 5);
        gmsGmkGearWork.move_draw_dir += (ushort)gmsGmkGearWork.move_draw_dir_spd;
        nnMakeRotateZMatrix(obj_work.obj_3d.user_obj_mtx_r, gmsGmkGearWork.move_draw_dir * 2 + gmsGmkGearWork.move_draw_dir_ofst);
        if (gmsGmkGearWork.old_move_draw_dir == gmsGmkGearWork.move_draw_dir || (obj_work.col_work.obj_col.rider_obj == null || obj_work.col_work.obj_col.rider_obj.obj_type != 1 || ((GMS_PLAYER_WORK)obj_work.col_work.obj_col.rider_obj).gmk_obj != null) && (obj_work.col_work.obj_col.toucher_obj == null || obj_work.col_work.obj_col.toucher_obj.obj_type != 1 || ((GMS_PLAYER_WORK)obj_work.col_work.obj_col.toucher_obj).gmk_obj != null) || gmsGmkGearWork.gmk_work.ene_com.target_obj != null)
            return;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)obj_work.col_work.obj_col.rider_obj ?? (GMS_PLAYER_WORK)obj_work.col_work.obj_col.toucher_obj;
        if (ply_work == null || ply_work.gmk_obj == obj_work)
            return;
        gmGmkGearSetRotFlow(obj_work, ply_work, ((short)gmsGmkGearWork.move_draw_dir - (short)gmsGmkGearWork.old_move_draw_dir) * 2);
    }

    private static void gmGmkMoveGearBodyDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_PLAYER_WORK parentObj1 = (GMS_PLAYER_WORK)match_rect.parent_obj;
        GMS_GMK_GEAR_WORK parentObj2 = (GMS_GMK_GEAR_WORK)mine_rect.parent_obj;
        OBS_OBJECT_WORK parentObj3 = mine_rect.parent_obj;
        if (parentObj1.obj_work.obj_type != 1 || parentObj1.gmk_obj == parentObj3 || (((int)parentObj1.obj_work.move_flag & 1) == 0 || parentObj1.seq_state == 0) || 3 <= parentObj1.seq_state && parentObj1.seq_state <= 8 || (parentObj1.obj_work.pos.x <= parentObj3.pos.x - 16384 || parentObj1.obj_work.pos.x >= parentObj3.pos.x + 16384) && (parentObj1.obj_work.prev_pos.x > parentObj3.pos.x || parentObj1.obj_work.pos.x < parentObj3.pos.x) && (parentObj1.obj_work.prev_pos.x < parentObj3.pos.x || parentObj1.obj_work.pos.x > parentObj3.pos.x))
            return;
        parentObj2.gmk_work.ene_com.target_dp_pos.x = 0;
        parentObj2.gmk_work.ene_com.target_dp_pos.y = -262144 - parentObj1.obj_work.field_rect[3] * 4096;
        parentObj2.gmk_work.ene_com.target_dp_pos.z = parentObj1.obj_work.pos.z - parentObj3.pos.z;
        parentObj2.gmk_work.ene_com.enemy_flag &= 4294967280U;
        GmPlySeqInitMoveGear(parentObj1, parentObj3, (parentObj2.gmk_work.ene_com.eve_rec.flag & 4) == 0);
        parentObj2.gmk_work.ene_com.target_obj = (OBS_OBJECT_WORK)parentObj1;
        gmGmkMoveGearMoveInit(parentObj3);
    }

    private static void gmGmkMoveGearDefFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        GMS_GMK_GEAR_WORK parentObj1 = (GMS_GMK_GEAR_WORK)mine_rect.parent_obj;
        if (match_rect.parent_obj.obj_type != 3)
            return;
        GMS_ENEMY_COM_WORK parentObj2 = (GMS_ENEMY_COM_WORK)match_rect.parent_obj;
        if (parentObj2.eve_rec.id == 183)
        {
            parentObj1.move_end_x = match_rect.parent_obj.pos.x;
            parentObj1.move_end_y = match_rect.parent_obj.pos.y;
            if ((parentObj2.eve_rec.flag & 1) == 0)
                return;
            parentObj1.gear_end_obj = match_rect.parent_obj;
        }
        else
        {
            if (((GMS_ENEMY_COM_WORK)match_rect.parent_obj).eve_rec.id != 184)
                return;
            parentObj1.sw_gear_obj = match_rect.parent_obj;
        }
    }

    private static void gmGmkMoveGearEndSwitchFwInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if ((gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 || gmsGmkGearWork.gmk_work.ene_com.eve_rec.byte_param[1] != 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkMoveGearEndSwitchFwMain);
    }

    private static void gmGmkMoveGearEndSwitchFwMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.move_gear_obj == null || ((GMS_ENEMY_COM_WORK)gmsGmkGearWork.move_gear_obj).eve_rec.byte_param[1] == 0)
            return;
        gmsGmkGearWork.gmk_work.ene_com.eve_rec.byte_param[1] = 1;
        obj_work.parent_obj = gmsGmkGearWork.move_gear_obj;
        obj_work.flag |= 16U;
        obj_work.ppFunc = null;
    }

    private static void gmGmkMoveGearEndAtkHitFunc(
      OBS_RECT_WORK mine_rect,
      OBS_RECT_WORK match_rect)
    {
        if (match_rect.parent_obj.obj_type != 3)
            ObjRectFuncNoHit(mine_rect, match_rect);
        else if (((GMS_ENEMY_COM_WORK)match_rect.parent_obj).eve_rec.id != 182)
        {
            ObjRectFuncNoHit(mine_rect, match_rect);
        }
        else
        {
            if ((((GMS_ENEMY_COM_WORK)mine_rect.parent_obj).eve_rec.flag & 1) == 0)
                return;
            ((GMS_GMK_GEAR_WORK)mine_rect.parent_obj).move_gear_obj = match_rect.parent_obj;
        }
    }

    private static void gmGmkGearMoveLastFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_GEAR_WORK gmsGmkGearWork = (GMS_GMK_GEAR_WORK)obj_work;
        float val = 0.0f;
        if (obj_work.flag != 0U || gmsGmkGearWork.h_snd_gear == null)
            return;
        int num1 = gmsGmkGearWork.gmk_work.ene_com.eve_rec.id != 182 ? (gmsGmkGearWork.move_gear_obj == null ? 0 : MTM_MATH_ABS(((GMS_GMK_GEAR_WORK)gmsGmkGearWork.move_gear_obj).move_draw_dir_spd)) : MTM_MATH_ABS(gmsGmkGearWork.move_draw_dir_spd) + MTM_MATH_ABS((short)(gmsGmkGearWork.move_draw_dir_ofst >> 3));
        if (num1 >= 4)
        {
            if (num1 >= 864)
            {
                val = 1f;
            }
            else
            {
                val = FXM_FX32_TO_FLOAT(FX_Div(num1 - 4, 860));
                if (val > 1.0)
                    val = 1f;
            }
        }
        gmsGmkGearWork.h_snd_gear.au_player.SetAisac("Speed", val);
        OBS_CAMERA obsCamera = ObjCameraGet(g_obj.glb_camera_id);
        float num2 = FXM_FX32_TO_FLOAT(gmsGmkGearWork.gmk_work.ene_com.obj_work.pos.x) - obsCamera.disp_pos.x;
        float num3 = FXM_FX32_TO_FLOAT(gmsGmkGearWork.gmk_work.ene_com.obj_work.pos.y) - -obsCamera.disp_pos.y;
        float num4;
        if (num2 < 600.0 && num3 < 600.0)
        {
            float num5 = (float)(num2 * (double)num2 + num3 * (double)num3);
            if (num5 <= 90000.0)
                num4 = 1f;
            else if (num5 <= 360000.0)
            {
                num4 = (float)((360000.0 - num5) / 90000.0);
                if (num4 > 1.0)
                    num4 = 1f;
                else if (num4 < 0.0)
                    num4 = 0.0f;
            }
            else
                num4 = 0.0f;
        }
        else
            num4 = 0.0f;
        gmsGmkGearWork.h_snd_gear.snd_ctrl_param.volume = num4;
    }


}