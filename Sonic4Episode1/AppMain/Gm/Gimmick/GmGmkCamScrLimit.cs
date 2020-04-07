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
    private static AppMain.OBS_OBJECT_WORK GmGmkCamScrLimitInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_CAM_SCR_LIMIT_WORK()), "GMK_CAM_SCRLMT");
        AppMain.GMS_GMK_CAM_SCR_LIMIT_WORK gmkCamScrLimitWork = (AppMain.GMS_GMK_CAM_SCR_LIMIT_WORK)work;
        ((AppMain.GMS_ENEMY_COM_WORK)work).enemy_flag |= 65536U;
        work.user_flag = (uint)eve_rec.flag;
        work.move_flag |= 8480U;
        work.flag |= 16U;
        AppMain.GMS_GMK_CAM_SCR_LIMIT_SETTING limitSetting = gmkCamScrLimitWork.limit_setting;
        limitSetting.limit_rect[0] = (work.pos.x >> 12) + (int)eve_rec.left * 2;
        limitSetting.limit_rect[2] = (work.pos.x >> 12) + (int)eve_rec.left * 2 + (int)eve_rec.width * 2;
        limitSetting.limit_rect[1] = (work.pos.y >> 12) + (int)eve_rec.top * 2;
        limitSetting.limit_rect[3] = (work.pos.y >> 12) + (int)eve_rec.top * 2 + (int)eve_rec.height * 2;
        if (eve_rec.id == (ushort)302)
        {
            work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCamScrLimitSetting);
            AppMain.g_gm_main_system.game_flag |= 32768U;
        }
        else
            work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCamScrLimitMain);
        return work;
    }

    private static void gmGmkCamScrLimitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (obj_work.pos.x > gmsPlayerWork.obj_work.pos.x)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCamScrLimitSetting);
        AppMain.g_gm_main_system.game_flag |= 32768U;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkCamScrLimitReleaseInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_COM_WORK()), "GMK_SCRLMT_RELEASE");
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)work;
        work.user_flag = (uint)eve_rec.flag;
        work.user_timer = 0;
        work.move_flag |= 8480U;
        work.flag |= 16U;
        if (eve_rec.id == (ushort)303)
        {
            work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCamScrLimitRelease);
            AppMain.g_gm_main_system.game_flag |= 32768U;
        }
        else
            work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCamScrLimitReleaseMain);
        work.user_work = 3U;
        return work;
    }

    private static void gmGmkCamScrLimitRelease(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        int num1 = AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.x) >> 12;
        int num2 = -AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.y) >> 12;
        int num3 = AppMain.FXM_FLOAT_TO_FX32(AppMain.AMD_SCREEN_2D_WIDTH / 2f * obsCamera.scale) >> 12;
        int num4 = AppMain.FXM_FLOAT_TO_FX32(AppMain.AMD_SCREEN_2D_HEIGHT / 2f * obsCamera.scale) >> 12;
        int userWork1 = (int)obj_work.user_work;
        int userWork2 = (int)obj_work.user_work;
        byte num5 = 1;
        if (((int)(ushort)obj_work.user_flag & 1) != 0 && AppMain.g_gm_main_system.map_fcol.left != 0)
        {
            if (num1 - num3 > AppMain.g_gm_main_system.map_fcol.left)
            {
                AppMain.g_gm_main_system.map_fcol.left = 0;
            }
            else
            {
                AppMain.g_gm_main_system.map_fcol.left -= userWork1;
                if (AppMain.g_gm_main_system.map_fcol.left < 0)
                    AppMain.g_gm_main_system.map_fcol.left = 0;
                num5 = (byte)0;
            }
        }
        if (((int)(ushort)obj_work.user_flag & 4) != 0 && AppMain.g_gm_main_system.map_fcol.right < (int)AppMain.g_gm_main_system.map_fcol.map_block_num_x * 64)
        {
            if (num1 + num3 < AppMain.g_gm_main_system.map_fcol.right)
            {
                AppMain.g_gm_main_system.map_fcol.right = (int)AppMain.g_gm_main_system.map_fcol.map_block_num_x * 64;
            }
            else
            {
                AppMain.g_gm_main_system.map_fcol.right += userWork1;
                if (AppMain.g_gm_main_system.map_fcol.right > (int)AppMain.g_gm_main_system.map_fcol.map_block_num_x * 64)
                    AppMain.g_gm_main_system.map_fcol.right = (int)AppMain.g_gm_main_system.map_fcol.map_block_num_x * 64;
                num5 = (byte)0;
            }
        }
        if (((int)(ushort)obj_work.user_flag & 2) != 0 && AppMain.g_gm_main_system.map_fcol.top != 0)
        {
            if (num2 - num4 > AppMain.g_gm_main_system.map_fcol.top)
            {
                AppMain.g_gm_main_system.map_fcol.top = 0;
            }
            else
            {
                AppMain.g_gm_main_system.map_fcol.top -= userWork2;
                if (AppMain.g_gm_main_system.map_fcol.top < 0)
                    AppMain.g_gm_main_system.map_fcol.top = 0;
                num5 = (byte)0;
            }
        }
        if (((int)(ushort)obj_work.user_flag & 8) != 0 && AppMain.g_gm_main_system.map_fcol.bottom < (int)AppMain.g_gm_main_system.map_fcol.map_block_num_y * 64)
        {
            if (num2 + num4 < AppMain.g_gm_main_system.map_fcol.bottom)
            {
                AppMain.g_gm_main_system.map_fcol.bottom = (int)AppMain.g_gm_main_system.map_fcol.map_block_num_y * 64;
            }
            else
            {
                AppMain.g_gm_main_system.map_fcol.bottom += userWork2;
                if (AppMain.g_gm_main_system.map_fcol.bottom > (int)AppMain.g_gm_main_system.map_fcol.map_block_num_y * 64)
                    AppMain.g_gm_main_system.map_fcol.bottom = (int)AppMain.g_gm_main_system.map_fcol.map_block_num_y * 64;
                num5 = (byte)0;
            }
        }
        if (num5 != (byte)0 && obj_work.user_timer != 0)
        {
            obj_work.flag |= 8U;
            AppMain.g_gm_main_system.game_flag &= 4294934527U;
        }
        ++obj_work.user_timer;
    }

    private static void gmGmkCamScrLimitReleaseMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (obj_work.pos.x > gmsPlayerWork.obj_work.pos.x)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkCamScrLimitRelease);
        AppMain.g_gm_main_system.game_flag |= 32768U;
    }

    private static void GmCamScrLimitSetDirect(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y)
    {
        if (((int)eve_rec.flag & 1) != 0)
            AppMain.g_gm_main_system.map_fcol.left = (pos_x >> 12) + (int)eve_rec.left * 2;
        if (((int)eve_rec.flag & 4) != 0)
            AppMain.g_gm_main_system.map_fcol.right = (pos_x >> 12) + (int)eve_rec.left * 2 + (int)eve_rec.width * 2;
        if (((int)eve_rec.flag & 2) != 0)
            AppMain.g_gm_main_system.map_fcol.top = (pos_y >> 12) + (int)eve_rec.top * 2;
        if (((int)eve_rec.flag & 8) == 0)
            return;
        AppMain.g_gm_main_system.map_fcol.bottom = (pos_y >> 12) + (int)eve_rec.top * 2 + (int)eve_rec.height * 2;
    }

    private static void gmGmkCamScrLimitSetting(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        AppMain.GMS_GMK_CAM_SCR_LIMIT_WORK gmkCamScrLimitWork = (AppMain.GMS_GMK_CAM_SCR_LIMIT_WORK)obj_work;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        int num1 = AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.x) >> 12;
        int num2 = -AppMain.FXM_FLOAT_TO_FX32(obsCamera.pos.y) >> 12;
        int num3 = 1;
        int num4 = 1;
        int num5 = AppMain.FXM_FLOAT_TO_FX32(AppMain.AMD_SCREEN_2D_WIDTH / 2f * obsCamera.scale) >> 12;
        int num6 = AppMain.FXM_FLOAT_TO_FX32(AppMain.AMD_SCREEN_2D_HEIGHT / 2f * obsCamera.scale) >> 12;
        byte num7 = 1;
        if ((Convert.ToInt32(obj_work.user_flag) & 1) != 0 && AppMain.g_gm_main_system.map_fcol.left != gmkCamScrLimitWork.limit_setting.limit_rect[0])
        {
            int num8 = num1 - num5;
            if (num8 > gmkCamScrLimitWork.limit_setting.limit_rect[0])
            {
                AppMain.g_gm_main_system.map_fcol.left = gmkCamScrLimitWork.limit_setting.limit_rect[0];
            }
            else
            {
                if (num8 > AppMain.g_gm_main_system.map_fcol.left)
                    AppMain.g_gm_main_system.map_fcol.left = num8 + num3;
                else
                    AppMain.g_gm_main_system.map_fcol.left += num3;
                if (AppMain.g_gm_main_system.map_fcol.left > gmkCamScrLimitWork.limit_setting.limit_rect[0])
                    AppMain.g_gm_main_system.map_fcol.left = gmkCamScrLimitWork.limit_setting.limit_rect[0];
            }
            num7 = (byte)0;
        }
        if ((Convert.ToInt32(obj_work.user_flag) & 4) != 0 && AppMain.g_gm_main_system.map_fcol.right != gmkCamScrLimitWork.limit_setting.limit_rect[2])
        {
            int num8 = num1 + num5;
            if (num8 < gmkCamScrLimitWork.limit_setting.limit_rect[2])
            {
                AppMain.g_gm_main_system.map_fcol.right = gmkCamScrLimitWork.limit_setting.limit_rect[2];
            }
            else
            {
                if (num8 < AppMain.g_gm_main_system.map_fcol.right)
                    AppMain.g_gm_main_system.map_fcol.right = num8 - num3;
                else
                    AppMain.g_gm_main_system.map_fcol.right -= num3;
                if (AppMain.g_gm_main_system.map_fcol.right < gmkCamScrLimitWork.limit_setting.limit_rect[2])
                    AppMain.g_gm_main_system.map_fcol.right = gmkCamScrLimitWork.limit_setting.limit_rect[2];
            }
            num7 = (byte)0;
        }
        if ((Convert.ToInt32(obj_work.user_flag) & 2) != 0 && AppMain.g_gm_main_system.map_fcol.top != gmkCamScrLimitWork.limit_setting.limit_rect[1])
        {
            int num8 = num2 - num6;
            if (num8 > gmkCamScrLimitWork.limit_setting.limit_rect[1])
            {
                AppMain.g_gm_main_system.map_fcol.top = gmkCamScrLimitWork.limit_setting.limit_rect[1];
            }
            else
            {
                if (num8 > AppMain.g_gm_main_system.map_fcol.top)
                    AppMain.g_gm_main_system.map_fcol.top = num8 + num4;
                else
                    AppMain.g_gm_main_system.map_fcol.top += num4;
                if (AppMain.g_gm_main_system.map_fcol.top > gmkCamScrLimitWork.limit_setting.limit_rect[1])
                    AppMain.g_gm_main_system.map_fcol.top = gmkCamScrLimitWork.limit_setting.limit_rect[1];
            }
            num7 = (byte)0;
        }
        if ((Convert.ToInt32(obj_work.user_flag) & 8) != 0 && AppMain.g_gm_main_system.map_fcol.bottom != gmkCamScrLimitWork.limit_setting.limit_rect[3])
        {
            int num8 = num2 + num6;
            if (num8 < gmkCamScrLimitWork.limit_setting.limit_rect[3])
            {
                AppMain.g_gm_main_system.map_fcol.bottom = gmkCamScrLimitWork.limit_setting.limit_rect[3];
            }
            else
            {
                if (num8 < AppMain.g_gm_main_system.map_fcol.bottom)
                    AppMain.g_gm_main_system.map_fcol.bottom = num8 - num4;
                else
                    AppMain.g_gm_main_system.map_fcol.bottom -= num4;
                if (AppMain.g_gm_main_system.map_fcol.bottom < gmkCamScrLimitWork.limit_setting.limit_rect[3])
                    AppMain.g_gm_main_system.map_fcol.bottom = gmkCamScrLimitWork.limit_setting.limit_rect[3];
            }
            num7 = (byte)0;
        }
        if (num7 == (byte)0)
            return;
        obj_work.flag |= 8U;
        AppMain.g_gm_main_system.game_flag &= 4294934527U;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkCamScrLimitRelease(byte flag)
    {
        return AppMain.GmEventMgrLocalEventBirth((ushort)303, 0, 0, (ushort)((uint)flag & 15U), (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
    }

}