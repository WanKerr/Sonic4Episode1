using System;

public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkCamScrLimitInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_CAM_SCR_LIMIT_WORK(), "GMK_CAM_SCRLMT");
        GMS_GMK_CAM_SCR_LIMIT_WORK gmkCamScrLimitWork = (GMS_GMK_CAM_SCR_LIMIT_WORK)work;
        ((GMS_ENEMY_COM_WORK)work).enemy_flag |= 65536U;
        work.user_flag = eve_rec.flag;
        work.move_flag |= 8480U;
        work.flag |= 16U;
        GMS_GMK_CAM_SCR_LIMIT_SETTING limitSetting = gmkCamScrLimitWork.limit_setting;
        limitSetting.limit_rect[0] = (work.pos.x >> 12) + eve_rec.left * 2;
        limitSetting.limit_rect[2] = (work.pos.x >> 12) + eve_rec.left * 2 + eve_rec.width * 2;
        limitSetting.limit_rect[1] = (work.pos.y >> 12) + eve_rec.top * 2;
        limitSetting.limit_rect[3] = (work.pos.y >> 12) + eve_rec.top * 2 + eve_rec.height * 2;
        if (eve_rec.id == 302)
        {
            work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCamScrLimitSetting);
            g_gm_main_system.game_flag |= 32768U;
        }
        else
            work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCamScrLimitMain);
        return work;
    }

    private static void gmGmkCamScrLimitMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (obj_work.pos.x > gmsPlayerWork.obj_work.pos.x)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCamScrLimitSetting);
        g_gm_main_system.game_flag |= 32768U;
    }

    private static OBS_OBJECT_WORK GmGmkCamScrLimitReleaseInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_COM_WORK(), "GMK_SCRLMT_RELEASE");
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)work;
        work.user_flag = eve_rec.flag;
        work.user_timer = 0;
        work.move_flag |= 8480U;
        work.flag |= 16U;
        if (eve_rec.id == 303)
        {
            work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCamScrLimitRelease);
            g_gm_main_system.game_flag |= 32768U;
        }
        else
            work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCamScrLimitReleaseMain);
        work.user_work = 3U;
        return work;
    }

    private static void gmGmkCamScrLimitRelease(OBS_OBJECT_WORK obj_work)
    {
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        int num1 = FXM_FLOAT_TO_FX32(obsCamera.pos.x) >> 12;
        int num2 = -FXM_FLOAT_TO_FX32(obsCamera.pos.y) >> 12;
        int num3 = FXM_FLOAT_TO_FX32(AMD_SCREEN_2D_WIDTH / 2f * obsCamera.scale) >> 12;
        int num4 = FXM_FLOAT_TO_FX32(AMD_SCREEN_2D_HEIGHT / 2f * obsCamera.scale) >> 12;
        int userWork1 = (int)obj_work.user_work;
        int userWork2 = (int)obj_work.user_work;
        byte num5 = 1;
        if (((ushort)obj_work.user_flag & 1) != 0 && g_gm_main_system.map_fcol.left != 0)
        {
            if (num1 - num3 > g_gm_main_system.map_fcol.left)
            {
                g_gm_main_system.map_fcol.left = 0;
            }
            else
            {
                g_gm_main_system.map_fcol.left -= userWork1;
                if (g_gm_main_system.map_fcol.left < 0)
                    g_gm_main_system.map_fcol.left = 0;
                num5 = 0;
            }
        }
        if (((ushort)obj_work.user_flag & 4) != 0 && g_gm_main_system.map_fcol.right < g_gm_main_system.map_fcol.map_block_num_x * 64)
        {
            if (num1 + num3 < g_gm_main_system.map_fcol.right)
            {
                g_gm_main_system.map_fcol.right = g_gm_main_system.map_fcol.map_block_num_x * 64;
            }
            else
            {
                g_gm_main_system.map_fcol.right += userWork1;
                if (g_gm_main_system.map_fcol.right > g_gm_main_system.map_fcol.map_block_num_x * 64)
                    g_gm_main_system.map_fcol.right = g_gm_main_system.map_fcol.map_block_num_x * 64;
                num5 = 0;
            }
        }
        if (((ushort)obj_work.user_flag & 2) != 0 && g_gm_main_system.map_fcol.top != 0)
        {
            if (num2 - num4 > g_gm_main_system.map_fcol.top)
            {
                g_gm_main_system.map_fcol.top = 0;
            }
            else
            {
                g_gm_main_system.map_fcol.top -= userWork2;
                if (g_gm_main_system.map_fcol.top < 0)
                    g_gm_main_system.map_fcol.top = 0;
                num5 = 0;
            }
        }
        if (((ushort)obj_work.user_flag & 8) != 0 && g_gm_main_system.map_fcol.bottom < g_gm_main_system.map_fcol.map_block_num_y * 64)
        {
            if (num2 + num4 < g_gm_main_system.map_fcol.bottom)
            {
                g_gm_main_system.map_fcol.bottom = g_gm_main_system.map_fcol.map_block_num_y * 64;
            }
            else
            {
                g_gm_main_system.map_fcol.bottom += userWork2;
                if (g_gm_main_system.map_fcol.bottom > g_gm_main_system.map_fcol.map_block_num_y * 64)
                    g_gm_main_system.map_fcol.bottom = g_gm_main_system.map_fcol.map_block_num_y * 64;
                num5 = 0;
            }
        }
        if (num5 != 0 && obj_work.user_timer != 0)
        {
            obj_work.flag |= 8U;
            g_gm_main_system.game_flag &= 4294934527U;
        }
        ++obj_work.user_timer;
    }

    private static void gmGmkCamScrLimitReleaseMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (obj_work.pos.x > gmsPlayerWork.obj_work.pos.x)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCamScrLimitRelease);
        g_gm_main_system.game_flag |= 32768U;
    }

    private static void GmCamScrLimitSetDirect(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y)
    {
        if ((eve_rec.flag & 1) != 0)
            g_gm_main_system.map_fcol.left = (pos_x >> 12) + eve_rec.left * 2;
        if ((eve_rec.flag & 4) != 0)
            g_gm_main_system.map_fcol.right = (pos_x >> 12) + eve_rec.left * 2 + eve_rec.width * 2;
        if ((eve_rec.flag & 2) != 0)
            g_gm_main_system.map_fcol.top = (pos_y >> 12) + eve_rec.top * 2;
        if ((eve_rec.flag & 8) == 0)
            return;
        g_gm_main_system.map_fcol.bottom = (pos_y >> 12) + eve_rec.top * 2 + eve_rec.height * 2;
    }

    private static void gmGmkCamScrLimitSetting(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
        GMS_GMK_CAM_SCR_LIMIT_WORK gmkCamScrLimitWork = (GMS_GMK_CAM_SCR_LIMIT_WORK)obj_work;
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        int num1 = FXM_FLOAT_TO_FX32(obsCamera.pos.x) >> 12;
        int num2 = -FXM_FLOAT_TO_FX32(obsCamera.pos.y) >> 12;
        int num3 = 1;
        int num4 = 1;
        int num5 = FXM_FLOAT_TO_FX32(AMD_SCREEN_2D_WIDTH / 2f * obsCamera.scale) >> 12;
        int num6 = FXM_FLOAT_TO_FX32(AMD_SCREEN_2D_HEIGHT / 2f * obsCamera.scale) >> 12;
        byte num7 = 1;
        if ((Convert.ToInt32(obj_work.user_flag) & 1) != 0 && g_gm_main_system.map_fcol.left != gmkCamScrLimitWork.limit_setting.limit_rect[0])
        {
            int num8 = num1 - num5;
            if (num8 > gmkCamScrLimitWork.limit_setting.limit_rect[0])
            {
                g_gm_main_system.map_fcol.left = gmkCamScrLimitWork.limit_setting.limit_rect[0];
            }
            else
            {
                if (num8 > g_gm_main_system.map_fcol.left)
                    g_gm_main_system.map_fcol.left = num8 + num3;
                else
                    g_gm_main_system.map_fcol.left += num3;
                if (g_gm_main_system.map_fcol.left > gmkCamScrLimitWork.limit_setting.limit_rect[0])
                    g_gm_main_system.map_fcol.left = gmkCamScrLimitWork.limit_setting.limit_rect[0];
            }
            num7 = 0;
        }
        if ((Convert.ToInt32(obj_work.user_flag) & 4) != 0 && g_gm_main_system.map_fcol.right != gmkCamScrLimitWork.limit_setting.limit_rect[2])
        {
            int num8 = num1 + num5;
            if (num8 < gmkCamScrLimitWork.limit_setting.limit_rect[2])
            {
                g_gm_main_system.map_fcol.right = gmkCamScrLimitWork.limit_setting.limit_rect[2];
            }
            else
            {
                if (num8 < g_gm_main_system.map_fcol.right)
                    g_gm_main_system.map_fcol.right = num8 - num3;
                else
                    g_gm_main_system.map_fcol.right -= num3;
                if (g_gm_main_system.map_fcol.right < gmkCamScrLimitWork.limit_setting.limit_rect[2])
                    g_gm_main_system.map_fcol.right = gmkCamScrLimitWork.limit_setting.limit_rect[2];
            }
            num7 = 0;
        }
        if ((Convert.ToInt32(obj_work.user_flag) & 2) != 0 && g_gm_main_system.map_fcol.top != gmkCamScrLimitWork.limit_setting.limit_rect[1])
        {
            int num8 = num2 - num6;
            if (num8 > gmkCamScrLimitWork.limit_setting.limit_rect[1])
            {
                g_gm_main_system.map_fcol.top = gmkCamScrLimitWork.limit_setting.limit_rect[1];
            }
            else
            {
                if (num8 > g_gm_main_system.map_fcol.top)
                    g_gm_main_system.map_fcol.top = num8 + num4;
                else
                    g_gm_main_system.map_fcol.top += num4;
                if (g_gm_main_system.map_fcol.top > gmkCamScrLimitWork.limit_setting.limit_rect[1])
                    g_gm_main_system.map_fcol.top = gmkCamScrLimitWork.limit_setting.limit_rect[1];
            }
            num7 = 0;
        }
        if ((Convert.ToInt32(obj_work.user_flag) & 8) != 0 && g_gm_main_system.map_fcol.bottom != gmkCamScrLimitWork.limit_setting.limit_rect[3])
        {
            int num8 = num2 + num6;
            if (num8 < gmkCamScrLimitWork.limit_setting.limit_rect[3])
            {
                g_gm_main_system.map_fcol.bottom = gmkCamScrLimitWork.limit_setting.limit_rect[3];
            }
            else
            {
                if (num8 < g_gm_main_system.map_fcol.bottom)
                    g_gm_main_system.map_fcol.bottom = num8 - num4;
                else
                    g_gm_main_system.map_fcol.bottom -= num4;
                if (g_gm_main_system.map_fcol.bottom < gmkCamScrLimitWork.limit_setting.limit_rect[3])
                    g_gm_main_system.map_fcol.bottom = gmkCamScrLimitWork.limit_setting.limit_rect[3];
            }
            num7 = 0;
        }
        if (num7 == 0)
            return;
        obj_work.flag |= 8U;
        g_gm_main_system.game_flag &= 4294934527U;
    }

    private static OBS_OBJECT_WORK GmGmkCamScrLimitRelease(byte flag)
    {
        return GmEventMgrLocalEventBirth(303, 0, 0, (ushort)(flag & 15U), 0, 0, 0, 0, 0);
    }

}