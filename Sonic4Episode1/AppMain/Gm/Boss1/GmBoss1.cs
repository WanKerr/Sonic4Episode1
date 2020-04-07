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
    public static int GMM_BOSS1_AREA_LEFT()
    {
        return AppMain.g_gm_main_system.map_fcol.left << 12;
    }

    public static int GMM_BOSS1_AREA_TOP()
    {
        return AppMain.g_gm_main_system.map_fcol.top << 12;
    }

    public static int GMM_BOSS1_AREA_RIGHT()
    {
        return AppMain.g_gm_main_system.map_fcol.right << 12;
    }

    public static int GMM_BOSS1_AREA_BOTTOM()
    {
        return AppMain.g_gm_main_system.map_fcol.bottom << 12;
    }

    public static int GMM_BOSS1_AREA_CENTER_X()
    {
        return AppMain.GMM_BOSS1_AREA_LEFT() + (AppMain.GMM_BOSS1_AREA_RIGHT() - AppMain.GMM_BOSS1_AREA_LEFT()) / 2;
    }

    public static int GMM_BOSS1_AREA_CENTER_Y()
    {
        return AppMain.GMM_BOSS1_AREA_TOP() + (AppMain.GMM_BOSS1_AREA_BOTTOM() - AppMain.GMM_BOSS1_AREA_TOP()) / 2;
    }

    public static AppMain.OBS_OBJECT_WORK GMM_BS_OBJ(object obj)
    {
        return obj is AppMain.IOBS_OBJECT_WORK ? ((AppMain.IOBS_OBJECT_WORK)obj).Cast() : (AppMain.OBS_OBJECT_WORK)obj;
    }

    private static void GmBoss1Build()
    {
        AppMain.gm_boss1_obj_3d_list = AppMain.GmGameDBuildRegBuildModel((AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, AppMain.GMD_BOSS1_ARC), (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 1, AppMain.GMD_BOSS1_ARC), 0U);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(703), 2, AppMain.GMD_BOSS1_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(704), 3, AppMain.GMD_BOSS1_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(705), 4, AppMain.GMD_BOSS1_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(706), 5, AppMain.GMD_BOSS1_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(707), 6, AppMain.GMD_BOSS1_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(708), 7, AppMain.GMD_BOSS1_ARC);
        AppMain.GmEfctBossBuildSingleDataReg(8, AppMain.ObjDataGet(709), AppMain.ObjDataGet(710), 0, (AppMain.OBS_DATA_WORK)null, (AppMain.OBS_DATA_WORK)null, AppMain.GMD_BOSS1_ARC);
        AppMain.GmEfctBossBuildSingleDataReg(8, AppMain.ObjDataGet(709), AppMain.ObjDataGet(710), 0, (AppMain.OBS_DATA_WORK)null, (AppMain.OBS_DATA_WORK)null, AppMain.GMD_BOSS1_ARC);
        AppMain.GmEfctBossBuildSingleDataReg(8, AppMain.ObjDataGet(709), AppMain.ObjDataGet(710), 0, (AppMain.OBS_DATA_WORK)null, (AppMain.OBS_DATA_WORK)null, AppMain.GMD_BOSS1_ARC);
    }

    private static void GmBoss1Flush()
    {
        AppMain.GmEfctBossFlushSingleDataInit();
        AppMain.ObjDataRelease(AppMain.ObjDataGet(708));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(707));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(706));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(705));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(704));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(703));
        AppMain.AMS_AMB_HEADER amsAmbHeader = (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, AppMain.GMD_BOSS1_ARC);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_boss1_obj_3d_list, amsAmbHeader.file_num);
        AppMain.gm_boss1_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss1Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS1_MGR_WORK()), "BOSS1_MGR");
        AppMain.GMS_BOSS1_MGR_WORK gmsBosS1MgrWork = (AppMain.GMS_BOSS1_MGR_WORK)work;
        work.flag |= 16U;
        work.disp_flag |= 32U;
        work.move_flag |= 8448U;
        gmsBosS1MgrWork.life = AppMain.GmBsCmnIsFinalZoneType(work) == 0 ? 8 : 4;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1MgrWaitLoad);
        return work;
    }

    private static void gmBoss1SetPartTextureBurnt(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.MTM_ASSERT((object)obj_work);
        AppMain.MTM_ASSERT((object)obj_work.obj_3d);
        AppMain.MTM_ASSERT(obj_work.disp_flag & 134217728U);
        obj_work.obj_3d.drawflag |= 268435456U;
        obj_work.obj_3d.draw_state.texoffset[0].mode = 2;
        obj_work.obj_3d.draw_state.texoffset[0].u = 0.5f;
    }

    private static bool gmBoss1IsScrollLockBusy()
    {
        return ((int)AppMain.g_gm_main_system.game_flag & 32768) != 0;
    }

    private static void gmBoss1Init1ShotTimer(
      AppMain.GMS_BOSS1_1SHOT_TIMER one_shot_timer,
      uint frame)
    {
        AppMain.MTM_ASSERT((object)one_shot_timer);
        one_shot_timer.timer = frame;
        one_shot_timer.is_active = true;
    }

    private static bool gmBoss1Update1ShotTimer(AppMain.GMS_BOSS1_1SHOT_TIMER one_shot_timer)
    {
        AppMain.MTM_ASSERT((object)one_shot_timer);
        if (!one_shot_timer.is_active)
            return false;
        if (one_shot_timer.timer != 0U)
        {
            --one_shot_timer.timer;
            return false;
        }
        one_shot_timer.is_active = false;
        return true;
    }

    private static void gmBoss1InitFlashScreen()
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS1_FLASH_SCREEN_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "boss1_flash_scr");
        AppMain.GMS_BOSS1_FLASH_SCREEN_WORK s1FlashScreenWork = (AppMain.GMS_BOSS1_FLASH_SCREEN_WORK)work;
        work.disp_flag |= 4128U;
        work.flag |= 16U;
        AppMain.GmBsCmnInitFlashScreen(s1FlashScreenWork.flash_work, 4f, 5f, 30f);
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1FlashScreenMain);
    }

    private static void gmBoss1FlashScreenMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_FLASH_SCREEN_WORK s1FlashScreenWork = (AppMain.GMS_BOSS1_FLASH_SCREEN_WORK)obj_work;
        if (AppMain.GmBsCmnUpdateFlashScreen(s1FlashScreenWork.flash_work) == 0)
            return;
        AppMain.GmBsCmnClearFlashScreen(s1FlashScreenWork.flash_work);
        obj_work.flag |= 4U;
    }
}