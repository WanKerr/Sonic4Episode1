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
    private static void gmGmkMoveGearFwInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
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
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkMoveGearFwMain);
    }

    private static void gmGmkMoveGearFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.rect_ret_timer == 0)
            return;
        gmsGmkGearWork.rect_ret_timer = AppMain.ObjTimeCountDown(gmsGmkGearWork.rect_ret_timer);
        if (gmsGmkGearWork.rect_ret_timer != 0)
            return;
        gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag &= 4294965247U;
        gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag &= 4294965247U;
    }

    private static void gmGmkMoveGearMoveInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        obj_work.user_flag &= 4294967294U;
        gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag |= 2048U;
        gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag &= 4294965247U;
        obj_work.flag |= 16U;
        obj_work.move_flag &= 4294959103U;
        gmsGmkGearWork.stop_timer = 737280;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkMoveGearMoveMain);
    }

    private static void gmGmkMoveGearMoveMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK targetObj = (AppMain.GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.target_obj;
        if (targetObj == null || targetObj.gmk_obj != obj_work)
        {
            gmsGmkGearWork.gmk_work.ene_com.target_obj = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.gmGmkMoveGearRetWaitInit(obj_work);
        }
        else
        {
            AppMain.gmGmkMoveGearSetSpd(obj_work, -targetObj.obj_work.spd_m);
            if (obj_work.spd_m != 0 && !gmsGmkGearWork.vib_end)
            {
                AppMain.GMM_PAD_VIB_SMALL_TIME(60f);
                gmsGmkGearWork.vib_end = true;
            }
            gmsGmkGearWork.stop_timer = targetObj.obj_work.spd_m != 0 ? 737280 : AppMain.ObjTimeCountDown(gmsGmkGearWork.stop_timer);
            if (gmsGmkGearWork.stop_timer != 0)
                return;
            gmsGmkGearWork.gmk_work.ene_com.enemy_flag |= 1U;
            gmsGmkGearWork.gmk_work.ene_com.target_obj = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.gmGmkMoveGearRetInit(obj_work);
        }
    }

    private static void gmGmkMoveGearRetWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        ((AppMain.GMS_GMK_GEAR_WORK)obj_work).rect_ret_timer = 65536;
        obj_work.flag &= 4294967293U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkMoveGearRetWaitMain);
    }

    private static void gmGmkMoveGearRetWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.rect_ret_timer != 0)
        {
            gmsGmkGearWork.rect_ret_timer = AppMain.ObjTimeCountDown(gmsGmkGearWork.rect_ret_timer);
            if (gmsGmkGearWork.rect_ret_timer == 0)
            {
                gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag &= 4294965247U;
                gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag &= 4294965247U;
            }
        }
        AppMain.gmGmkMoveGearSetSpd(obj_work, AppMain.ObjSpdDownSet(obj_work.spd_m, 1024));
        if (obj_work.spd_m != 0)
            return;
        gmsGmkGearWork.stop_timer = AppMain.ObjTimeCountDown(gmsGmkGearWork.stop_timer);
        if (gmsGmkGearWork.stop_timer != 0)
            return;
        AppMain.gmGmkMoveGearRetInit(obj_work);
    }

    private static void gmGmkMoveGearRetInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        gmsGmkGearWork.rect_ret_timer = 0;
        if (((int)gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag & 2048) != 0)
            gmsGmkGearWork.rect_ret_timer = 65536;
        if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 && obj_work.pos.x == gmsGmkGearWork.gmk_work.ene_com.born_pos_x || ((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0 && obj_work.pos.y == gmsGmkGearWork.gmk_work.ene_com.born_pos_y)
        {
            AppMain.gmGmkMoveGearFwInit(obj_work);
        }
        else
        {
            gmsGmkGearWork.ret_max_speed = 0;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkMoveGearRetMain);
        }
    }

    private static void gmGmkMoveGearRetMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        bool flag = false;
        int spd_m = obj_work.spd_m;
        if (gmsGmkGearWork.rect_ret_timer != 0)
        {
            gmsGmkGearWork.rect_ret_timer = AppMain.ObjTimeCountDown(gmsGmkGearWork.rect_ret_timer);
            if (gmsGmkGearWork.rect_ret_timer == 0)
            {
                gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag &= 4294965247U;
                gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag &= 4294965247U;
            }
        }
        if (((int)gmsEnemyComWork.eve_rec.flag & 2) != 0)
        {
            if (((int)gmsEnemyComWork.eve_rec.flag & 1) != 0)
            {
                if (obj_work.pos.y < gmsEnemyComWork.born_pos_y)
                    spd_m = AppMain.ObjSpdUpSet(-obj_work.spd_m, -512, 16384);
                else
                    flag = true;
                if (((int)obj_work.move_flag & 4) != 0)
                    flag = true;
            }
            else
            {
                if (obj_work.pos.x < gmsEnemyComWork.born_pos_x)
                    spd_m = AppMain.ObjSpdUpSet(obj_work.spd_m, 512, 16384);
                else
                    flag = true;
                if (((int)obj_work.move_flag & 4) != 0)
                    flag = true;
            }
        }
        else if (((int)gmsEnemyComWork.eve_rec.flag & 1) != 0)
        {
            if (obj_work.pos.y > gmsEnemyComWork.born_pos_y)
                spd_m = AppMain.ObjSpdUpSet(-obj_work.spd_m, 512, 16384);
            else
                flag = true;
            if (((int)obj_work.move_flag & 8) != 0)
                flag = true;
        }
        else
        {
            if (obj_work.pos.x > gmsEnemyComWork.born_pos_x)
                spd_m = AppMain.ObjSpdUpSet(obj_work.spd_m, -512, 16384);
            else
                flag = true;
            if (((int)obj_work.move_flag & 8) != 0)
                flag = true;
        }
        if (AppMain.MTM_MATH_ABS(obj_work.spd_m) > gmsGmkGearWork.ret_max_speed)
            gmsGmkGearWork.ret_max_speed = AppMain.MTM_MATH_ABS(obj_work.spd_m);
        if (flag)
        {
            if (gmsGmkGearWork.ret_max_speed >= 14336)
                obj_work.vib_timer = 65536;
            AppMain.gmGmkMoveGearSetSpd(obj_work, 0);
            AppMain.gmGmkMoveGearFwInit(obj_work);
        }
        else
            AppMain.gmGmkMoveGearSetSpd(obj_work, spd_m);
    }

    private static void gmGmkMoveGearSwitchExeInit(
      AppMain.OBS_OBJECT_WORK obj_work,
      short cam_ofst_x,
      short cam_ofst_y)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        obj_work.user_flag |= 1U;
        obj_work.flag |= 2U;
        if (gmsEnemyComWork.target_obj != null && gmsEnemyComWork.target_obj.obj_type == (ushort)1)
        {
            AppMain.GMS_PLAYER_WORK targetObj = (AppMain.GMS_PLAYER_WORK)gmsEnemyComWork.target_obj;
            AppMain.GmPlayerCameraOffsetSet(targetObj, (short)((int)targetObj.gmk_camera_center_ofst_x + (int)cam_ofst_x), (short)((int)targetObj.gmk_camera_center_ofst_y + (int)cam_ofst_y));
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkMoveGearSwitchExeMain);
    }

    private static void gmGmkMoveGearSwitchExeMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        if (gmsGmkGearWork.sw_gear_obj == null)
        {
            AppMain.gmGmkMoveGearEndInit(obj_work);
        }
        else
        {
            AppMain.GMS_GMK_GEAR_WORK swGearObj = (AppMain.GMS_GMK_GEAR_WORK)gmsGmkGearWork.sw_gear_obj;
            if (swGearObj.open_rot_dist <= 0)
            {
                AppMain.gmGmkMoveGearSetSpd(obj_work, 0);
                if (swGearObj.gmk_work.ene_com.eve_rec.height == (byte)0)
                    gmsEnemyComWork.eve_rec.byte_param[1] = (byte)1;
                AppMain.gmGmkMoveGearEndStaggerInit(obj_work);
            }
            else
            {
                AppMain.GMS_PLAYER_WORK targetObj = (AppMain.GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.target_obj;
                if (targetObj == null || ((int)targetObj.player_flag & 1024) != 0)
                {
                    gmsGmkGearWork.gmk_work.ene_com.target_obj = (AppMain.OBS_OBJECT_WORK)null;
                    AppMain.gmGmkMoveGearRetWaitInit(obj_work);
                }
                else
                {
                    int sSpd = 0;
                    if (((int)gmsEnemyComWork.eve_rec.flag & 1) != 0)
                    {
                        if (((int)gmsEnemyComWork.eve_rec.flag & 2) == 0 && targetObj.obj_work.spd_m < 16384)
                            sSpd = 256;
                        else if (((int)gmsEnemyComWork.eve_rec.flag & 2) != 0 && targetObj.obj_work.spd_m > -16384)
                            sSpd = -256;
                    }
                    else if (((int)gmsEnemyComWork.eve_rec.flag & 2) == 0 && targetObj.obj_work.spd_m > -16384)
                        sSpd = -256;
                    else if (((int)gmsEnemyComWork.eve_rec.flag & 2) != 0 && targetObj.obj_work.spd_m < 16384)
                        sSpd = 256;
                    if (sSpd != 0)
                        targetObj.obj_work.spd_m = AppMain.ObjSpdUpSet(targetObj.obj_work.spd_m, sSpd, 16384);
                    AppMain.gmGmkMoveGearSetSpd(obj_work, -targetObj.obj_work.spd_m);
                }
            }
        }
    }

    private static void gmGmkMoveGearEndStaggerInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK targetObj = (AppMain.GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.target_obj;
        obj_work.user_flag &= 4294967294U;
        obj_work.user_flag |= 2U;
        obj_work.flag |= 2U;
        obj_work.move_flag |= 8448U;
        gmsGmkGearWork.move_stagger_dir_cnt = (ushort)0;
        gmsGmkGearWork.move_stagger_dir_spd = targetObj.obj_work.spd_m;
        obj_work.user_timer = gmsGmkGearWork.move_stagger_dir_spd;
        gmsGmkGearWork.move_stagger_step = (ushort)0;
        obj_work.user_work = (uint)gmsGmkGearWork.move_stagger_step;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkMoveGearEndStaggerMain);
    }

    private static void gmGmkMoveGearEndStaggerMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK targetObj = (AppMain.GMS_PLAYER_WORK)gmsGmkGearWork.gmk_work.ene_com.target_obj;
        if (targetObj == null || targetObj.gmk_obj != obj_work)
        {
            obj_work.user_flag &= 4294967293U;
            if (((AppMain.GMS_GMK_GEAR_WORK)gmsGmkGearWork.sw_gear_obj).gmk_work.ene_com.eve_rec.height == (byte)0)
            {
                gmsGmkGearWork.gmk_work.ene_com.eve_rec.byte_param[1] = (byte)1;
                AppMain.gmGmkMoveGearEndInit(obj_work);
            }
            else
                AppMain.gmGmkMoveGearSwitchRetWaitInit(obj_work);
        }
        else
        {
            int num = ((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 ? 96 : -96;
            switch (gmsGmkGearWork.move_stagger_step)
            {
                case 0:
                    gmsGmkGearWork.move_stagger_dir_spd = gmsGmkGearWork.move_stagger_dir_spd * 9 / 10;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (AppMain.MTM_MATH_ABS(gmsGmkGearWork.move_stagger_dir_spd) <= 128)
                    {
                        ++gmsGmkGearWork.move_stagger_step;
                        gmsGmkGearWork.move_stagger_dir_spd = 0;
                        gmsGmkGearWork.move_stagger_dir_cnt = (ushort)0;
                        gmsGmkGearWork.move_draw_dir_limit = gmsGmkGearWork.move_draw_dir_ofst;
                        break;
                    }
                    break;
                case 1:
                    gmsGmkGearWork.move_stagger_dir_spd += num * 3 / 2;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (gmsGmkGearWork.move_stagger_dir_cnt == (ushort)0)
                    {
                        if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
                        {
                            if ((int)gmsGmkGearWork.move_draw_dir_ofst < (int)gmsGmkGearWork.move_draw_dir_limit / 2)
                            {
                                gmsGmkGearWork.move_stagger_dir_cnt = (ushort)1;
                                break;
                            }
                            break;
                        }
                        if ((int)gmsGmkGearWork.move_draw_dir_ofst > (int)gmsGmkGearWork.move_draw_dir_limit / 2)
                        {
                            gmsGmkGearWork.move_stagger_dir_cnt = (ushort)1;
                            break;
                        }
                        break;
                    }
                    ++gmsGmkGearWork.move_stagger_dir_cnt;
                    if (gmsGmkGearWork.move_stagger_dir_cnt == (ushort)3)
                    {
                        ++gmsGmkGearWork.move_stagger_step;
                        break;
                    }
                    break;
                case 2:
                    gmsGmkGearWork.move_stagger_dir_spd -= num * 3 / 2;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (AppMain.MTM_MATH_ABS(gmsGmkGearWork.move_stagger_dir_spd) <= 128)
                    {
                        gmsGmkGearWork.move_stagger_dir_spd = 0;
                        if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0 && AppMain.GmPlayerKeyCheckWalkRight(targetObj) || ((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 && AppMain.GmPlayerKeyCheckWalkLeft(targetObj))
                        {
                            ++gmsGmkGearWork.move_stagger_step;
                            break;
                        }
                        gmsGmkGearWork.move_stagger_step = (ushort)5;
                        break;
                    }
                    break;
                case 3:
                    gmsGmkGearWork.move_stagger_dir_spd -= num;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (AppMain.MTM_MATH_ABS((int)gmsGmkGearWork.move_draw_dir_ofst) > 1820)
                    {
                        ++gmsGmkGearWork.move_stagger_step;
                        break;
                    }
                    break;
                case 4:
                    gmsGmkGearWork.move_stagger_dir_spd += num;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (AppMain.MTM_MATH_ABS(gmsGmkGearWork.move_stagger_dir_spd) <= 128)
                    {
                        gmsGmkGearWork.move_stagger_step = (ushort)1;
                        gmsGmkGearWork.move_stagger_dir_spd = 0;
                        gmsGmkGearWork.move_stagger_dir_cnt = (ushort)0;
                        gmsGmkGearWork.move_draw_dir_limit = gmsGmkGearWork.move_draw_dir_ofst;
                        break;
                    }
                    break;
                case 5:
                    gmsGmkGearWork.move_stagger_dir_spd -= num;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (AppMain.MTM_MATH_ABS((int)gmsGmkGearWork.move_draw_dir_ofst) <= 309)
                    {
                        ++gmsGmkGearWork.move_stagger_step;
                        break;
                    }
                    break;
                case 6:
                    gmsGmkGearWork.move_stagger_dir_spd += num;
                    gmsGmkGearWork.move_draw_dir_ofst += (short)(gmsGmkGearWork.move_stagger_dir_spd >> 5);
                    if (AppMain.MTM_MATH_ABS(gmsGmkGearWork.move_stagger_dir_spd) <= 128)
                    {
                        gmsGmkGearWork.move_stagger_dir_spd = 0;
                        gmsGmkGearWork.move_draw_dir_ofst /= (short)2;
                        ++gmsGmkGearWork.move_stagger_step;
                        break;
                    }
                    break;
                case 7:
                    if (gmsGmkGearWork.move_draw_dir_ofst != (short)0)
                        gmsGmkGearWork.move_draw_dir_ofst /= (short)2;
                    if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0 && AppMain.GmPlayerKeyCheckWalkRight(targetObj) || ((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 && AppMain.GmPlayerKeyCheckWalkLeft(targetObj))
                    {
                        gmsGmkGearWork.move_stagger_step = (ushort)3;
                        gmsGmkGearWork.move_draw_dir_ofst = (short)0;
                        break;
                    }
                    break;
            }
            obj_work.user_work = (uint)gmsGmkGearWork.move_stagger_step;
            obj_work.user_timer = gmsGmkGearWork.move_stagger_dir_spd;
            AppMain.gmGmkMoveGearSetSpd(obj_work, 0);
        }
    }

    private static void gmGmkMoveGearEndInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        obj_work.flag |= 2U;
        obj_work.move_flag |= 8448U;
        gmsGmkGearWork.gmk_work.ene_com.enemy_flag |= 1U;
        gmsGmkGearWork.gmk_work.ene_com.target_obj = (AppMain.OBS_OBJECT_WORK)null;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkMoveGearEndMain);
    }

    private static void gmGmkMoveGearEndMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        int moveDrawDirOfst = (int)gmsGmkGearWork.move_draw_dir_ofst;
        gmsGmkGearWork.move_draw_dir_ofst = (short)AppMain.ObjSpdDownSet((int)gmsGmkGearWork.move_draw_dir_ofst, 64);
        AppMain.gmGmkMoveGearSetSpd(obj_work, 0);
    }

    private static void gmGmkMoveGearSwitchRetWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        obj_work.flag |= 18U;
        obj_work.move_flag |= 8448U;
        gmsGmkGearWork.gmk_work.ene_com.enemy_flag |= 1U;
        gmsGmkGearWork.gmk_work.ene_com.target_obj = (AppMain.OBS_OBJECT_WORK)null;
        obj_work.user_timer = 245760;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkMoveGearSwitchRetWaitMain);
    }

    private static void gmGmkMoveGearSwitchRetWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.sw_gear_obj == null)
        {
            obj_work.flag &= 4294967279U;
            AppMain.gmGmkMoveGearEndInit(obj_work);
        }
        else
        {
            obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
            if (obj_work.col_work.obj_col.rider_obj == null)
            {
                if (obj_work.user_timer == 0)
                    AppMain.gmGmkMoveGearSwitchRetInit(obj_work);
            }
            else
                obj_work.user_timer = 245760;
            if (gmsGmkGearWork.move_draw_dir_ofst == (short)0)
                return;
            gmsGmkGearWork.move_draw_dir_ofst = (short)AppMain.ObjSpdDownSet((int)gmsGmkGearWork.move_draw_dir_ofst, 64);
            AppMain.gmGmkMoveGearSetSpd(obj_work, 0);
        }
    }

    private static void gmGmkMoveGearSwitchRetInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        obj_work.flag &= 4294967293U;
        obj_work.move_flag &= 4294958847U;
        gmsGmkGearWork.gmk_work.ene_com.enemy_flag &= 4294967280U;
        obj_work.user_flag &= 4294967294U;
        gmsGmkGearWork.gmk_work.ene_com.rect_work[0].flag |= 2048U;
        gmsGmkGearWork.rect_ret_timer = 0;
        if (((int)gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag & 2048) != 0)
            gmsGmkGearWork.rect_ret_timer = 65536;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkMoveGearSwitchRetMain);
    }

    private static void gmGmkMoveGearSwitchRetMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.sw_gear_obj == null)
        {
            obj_work.flag &= 4294967279U;
            AppMain.gmGmkMoveGearEndInit(obj_work);
        }
        else
        {
            if (gmsGmkGearWork.rect_ret_timer != 0)
            {
                gmsGmkGearWork.rect_ret_timer = AppMain.ObjTimeCountDown(gmsGmkGearWork.rect_ret_timer);
                if (gmsGmkGearWork.rect_ret_timer == 0)
                    gmsGmkGearWork.gmk_work.ene_com.rect_work[2].flag &= 4294965247U;
            }
            AppMain.GMS_GMK_GEAR_WORK swGearObj = (AppMain.GMS_GMK_GEAR_WORK)gmsGmkGearWork.sw_gear_obj;
            if (swGearObj.open_rot_dist >= 65536)
            {
                AppMain.gmGmkMoveGearSetSpd(obj_work, 0);
                AppMain.gmGmkMoveGearRetInit(obj_work);
            }
            else
            {
                int num = swGearObj.close_rot_spd;
                if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
                {
                    if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 2) == 0)
                        num = -num;
                }
                else if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 2) != 0)
                    num = -num;
                AppMain.gmGmkMoveGearSetSpd(obj_work, -num << 5);
                obj_work.spd_m = 0;
            }
        }
    }

    private static bool gmGmkMoveGearCheckSwitchMove(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_type != (ushort)3 || ((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec.id != (ushort)182)
            return false;
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
        {
            if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 2) != 0)
            {
                if (obj_work.pos.y <= gmsGmkGearWork.move_end_y)
                    return true;
            }
            else if (obj_work.pos.y >= gmsGmkGearWork.move_end_y)
                return true;
        }
        else if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 2) != 0)
        {
            if (obj_work.pos.x <= gmsGmkGearWork.move_end_x)
                return true;
        }
        else if (obj_work.pos.x >= gmsGmkGearWork.move_end_x)
            return true;
        return false;
    }

    private static void gmGmkMoveGearSetSpd(AppMain.OBS_OBJECT_WORK obj_work, int spd_m)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        int num1 = spd_m;
        int num2;
        int num3;
        if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
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
        if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 2) != 0)
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
        if (spd_m != 0 && ((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) != 0)
            obj_work.spd.x += 4096;
        gmsGmkGearWork.old_move_draw_dir = gmsGmkGearWork.move_draw_dir;
        gmsGmkGearWork.move_draw_dir_spd = (short)(-num1 >> 5);
        gmsGmkGearWork.move_draw_dir += (ushort)gmsGmkGearWork.move_draw_dir_spd;
        AppMain.nnMakeRotateZMatrix(obj_work.obj_3d.user_obj_mtx_r, (int)gmsGmkGearWork.move_draw_dir * 2 + (int)gmsGmkGearWork.move_draw_dir_ofst);
        if ((int)gmsGmkGearWork.old_move_draw_dir == (int)gmsGmkGearWork.move_draw_dir || (obj_work.col_work.obj_col.rider_obj == null || obj_work.col_work.obj_col.rider_obj.obj_type != (ushort)1 || ((AppMain.GMS_PLAYER_WORK)obj_work.col_work.obj_col.rider_obj).gmk_obj != null) && (obj_work.col_work.obj_col.toucher_obj == null || obj_work.col_work.obj_col.toucher_obj.obj_type != (ushort)1 || ((AppMain.GMS_PLAYER_WORK)obj_work.col_work.obj_col.toucher_obj).gmk_obj != null) || gmsGmkGearWork.gmk_work.ene_com.target_obj != null)
            return;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)obj_work.col_work.obj_col.rider_obj ?? (AppMain.GMS_PLAYER_WORK)obj_work.col_work.obj_col.toucher_obj;
        if (ply_work == null || ply_work.gmk_obj == obj_work)
            return;
        AppMain.gmGmkGearSetRotFlow(obj_work, ply_work, ((int)(short)gmsGmkGearWork.move_draw_dir - (int)(short)gmsGmkGearWork.old_move_draw_dir) * 2);
    }

    private static void gmGmkMoveGearBodyDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_PLAYER_WORK parentObj1 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        AppMain.GMS_GMK_GEAR_WORK parentObj2 = (AppMain.GMS_GMK_GEAR_WORK)mine_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj3 = mine_rect.parent_obj;
        if (parentObj1.obj_work.obj_type != (ushort)1 || parentObj1.gmk_obj == parentObj3 || (((int)parentObj1.obj_work.move_flag & 1) == 0 || parentObj1.seq_state == 0) || 3 <= parentObj1.seq_state && parentObj1.seq_state <= 8 || (parentObj1.obj_work.pos.x <= parentObj3.pos.x - 16384 || parentObj1.obj_work.pos.x >= parentObj3.pos.x + 16384) && (parentObj1.obj_work.prev_pos.x > parentObj3.pos.x || parentObj1.obj_work.pos.x < parentObj3.pos.x) && (parentObj1.obj_work.prev_pos.x < parentObj3.pos.x || parentObj1.obj_work.pos.x > parentObj3.pos.x))
            return;
        parentObj2.gmk_work.ene_com.target_dp_pos.x = 0;
        parentObj2.gmk_work.ene_com.target_dp_pos.y = -262144 - (int)parentObj1.obj_work.field_rect[3] * 4096;
        parentObj2.gmk_work.ene_com.target_dp_pos.z = parentObj1.obj_work.pos.z - parentObj3.pos.z;
        parentObj2.gmk_work.ene_com.enemy_flag &= 4294967280U;
        AppMain.GmPlySeqInitMoveGear(parentObj1, parentObj3, ((int)parentObj2.gmk_work.ene_com.eve_rec.flag & 4) == 0);
        parentObj2.gmk_work.ene_com.target_obj = (AppMain.OBS_OBJECT_WORK)parentObj1;
        AppMain.gmGmkMoveGearMoveInit(parentObj3);
    }

    private static void gmGmkMoveGearDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_GMK_GEAR_WORK parentObj1 = (AppMain.GMS_GMK_GEAR_WORK)mine_rect.parent_obj;
        if (match_rect.parent_obj.obj_type != (ushort)3)
            return;
        AppMain.GMS_ENEMY_COM_WORK parentObj2 = (AppMain.GMS_ENEMY_COM_WORK)match_rect.parent_obj;
        if (parentObj2.eve_rec.id == (ushort)183)
        {
            parentObj1.move_end_x = match_rect.parent_obj.pos.x;
            parentObj1.move_end_y = match_rect.parent_obj.pos.y;
            if (((int)parentObj2.eve_rec.flag & 1) == 0)
                return;
            parentObj1.gear_end_obj = match_rect.parent_obj;
        }
        else
        {
            if (((AppMain.GMS_ENEMY_COM_WORK)match_rect.parent_obj).eve_rec.id != (ushort)184)
                return;
            parentObj1.sw_gear_obj = match_rect.parent_obj;
        }
    }

    private static void gmGmkMoveGearEndSwitchFwInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        if (((int)gmsGmkGearWork.gmk_work.ene_com.eve_rec.flag & 1) == 0 || gmsGmkGearWork.gmk_work.ene_com.eve_rec.byte_param[1] != (byte)0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkMoveGearEndSwitchFwMain);
    }

    private static void gmGmkMoveGearEndSwitchFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        if (gmsGmkGearWork.move_gear_obj == null || ((AppMain.GMS_ENEMY_COM_WORK)gmsGmkGearWork.move_gear_obj).eve_rec.byte_param[1] == (byte)0)
            return;
        gmsGmkGearWork.gmk_work.ene_com.eve_rec.byte_param[1] = (byte)1;
        obj_work.parent_obj = gmsGmkGearWork.move_gear_obj;
        obj_work.flag |= 16U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmGmkMoveGearEndAtkHitFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        if (match_rect.parent_obj.obj_type != (ushort)3)
            AppMain.ObjRectFuncNoHit(mine_rect, match_rect);
        else if (((AppMain.GMS_ENEMY_COM_WORK)match_rect.parent_obj).eve_rec.id != (ushort)182)
        {
            AppMain.ObjRectFuncNoHit(mine_rect, match_rect);
        }
        else
        {
            if (((int)((AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj).eve_rec.flag & 1) == 0)
                return;
            ((AppMain.GMS_GMK_GEAR_WORK)mine_rect.parent_obj).move_gear_obj = match_rect.parent_obj;
        }
    }

    private static void gmGmkGearMoveLastFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_GEAR_WORK gmsGmkGearWork = (AppMain.GMS_GMK_GEAR_WORK)obj_work;
        float val = 0.0f;
        if (obj_work.flag != 0U || gmsGmkGearWork.h_snd_gear == null)
            return;
        int num1 = gmsGmkGearWork.gmk_work.ene_com.eve_rec.id != (ushort)182 ? (gmsGmkGearWork.move_gear_obj == null ? 0 : AppMain.MTM_MATH_ABS((int)((AppMain.GMS_GMK_GEAR_WORK)gmsGmkGearWork.move_gear_obj).move_draw_dir_spd)) : AppMain.MTM_MATH_ABS((int)gmsGmkGearWork.move_draw_dir_spd) + AppMain.MTM_MATH_ABS((int)(short)((int)gmsGmkGearWork.move_draw_dir_ofst >> 3));
        if (num1 >= 4)
        {
            if (num1 >= 864)
            {
                val = 1f;
            }
            else
            {
                val = AppMain.FXM_FX32_TO_FLOAT(AppMain.FX_Div(num1 - 4, 860));
                if ((double)val > 1.0)
                    val = 1f;
            }
        }
        gmsGmkGearWork.h_snd_gear.au_player.SetAisac("Speed", val);
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        float num2 = AppMain.FXM_FX32_TO_FLOAT(gmsGmkGearWork.gmk_work.ene_com.obj_work.pos.x) - obsCamera.disp_pos.x;
        float num3 = AppMain.FXM_FX32_TO_FLOAT(gmsGmkGearWork.gmk_work.ene_com.obj_work.pos.y) - -obsCamera.disp_pos.y;
        float num4;
        if ((double)num2 < 600.0 && (double)num3 < 600.0)
        {
            float num5 = (float)((double)num2 * (double)num2 + (double)num3 * (double)num3);
            if ((double)num5 <= 90000.0)
                num4 = 1f;
            else if ((double)num5 <= 360000.0)
            {
                num4 = (float)((360000.0 - (double)num5) / 90000.0);
                if ((double)num4 > 1.0)
                    num4 = 1f;
                else if ((double)num4 < 0.0)
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