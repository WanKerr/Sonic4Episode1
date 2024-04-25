public partial class AppMain
{
    public static int GMM_BOSS1_AREA_LEFT()
    {
        return g_gm_main_system.map_fcol.left << 12;
    }

    public static int GMM_BOSS1_AREA_TOP()
    {
        return g_gm_main_system.map_fcol.top << 12;
    }

    public static int GMM_BOSS1_AREA_RIGHT()
    {
        return g_gm_main_system.map_fcol.right << 12;
    }

    public static int GMM_BOSS1_AREA_BOTTOM()
    {
        return g_gm_main_system.map_fcol.bottom << 12;
    }

    public static int GMM_BOSS1_AREA_CENTER_X()
    {
        return GMM_BOSS1_AREA_LEFT() + (GMM_BOSS1_AREA_RIGHT() - GMM_BOSS1_AREA_LEFT()) / 2;
    }

    public static int GMM_BOSS1_AREA_CENTER_Y()
    {
        return GMM_BOSS1_AREA_TOP() + (GMM_BOSS1_AREA_BOTTOM() - GMM_BOSS1_AREA_TOP()) / 2;
    }

    public static OBS_OBJECT_WORK GMM_BS_OBJ(object obj)
    {
        return obj is IOBS_OBJECT_WORK ? ((IOBS_OBJECT_WORK)obj).Cast() : (OBS_OBJECT_WORK)obj;
    }

    private static void GmBoss1Build()
    {
        gm_boss1_obj_3d_list = GmGameDBuildRegBuildModel((AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, GMD_BOSS1_ARC), (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 1, GMD_BOSS1_ARC), 0U);
        ObjDataLoadAmbIndex(ObjDataGet(703), 2, GMD_BOSS1_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(704), 3, GMD_BOSS1_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(705), 4, GMD_BOSS1_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(706), 5, GMD_BOSS1_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(707), 6, GMD_BOSS1_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(708), 7, GMD_BOSS1_ARC);
        GmEfctBossBuildSingleDataReg(8, ObjDataGet(709), ObjDataGet(710), 0, null, null, GMD_BOSS1_ARC);
        GmEfctBossBuildSingleDataReg(8, ObjDataGet(709), ObjDataGet(710), 0, null, null, GMD_BOSS1_ARC);
        GmEfctBossBuildSingleDataReg(8, ObjDataGet(709), ObjDataGet(710), 0, null, null, GMD_BOSS1_ARC);
    }

    private static void GmBoss1Flush()
    {
        GmEfctBossFlushSingleDataInit();
        ObjDataRelease(ObjDataGet(708));
        ObjDataRelease(ObjDataGet(707));
        ObjDataRelease(ObjDataGet(706));
        ObjDataRelease(ObjDataGet(705));
        ObjDataRelease(ObjDataGet(704));
        ObjDataRelease(ObjDataGet(703));
        AMS_AMB_HEADER amsAmbHeader = (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, GMD_BOSS1_ARC);
        GmGameDBuildRegFlushModel(gm_boss1_obj_3d_list, amsAmbHeader.file_num);
        gm_boss1_obj_3d_list = null;
    }

    private static OBS_OBJECT_WORK GmBoss1Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS1_MGR_WORK(), "BOSS1_MGR");
        GMS_BOSS1_MGR_WORK gmsBosS1MgrWork = (GMS_BOSS1_MGR_WORK)work;
        work.flag |= 16U;
        work.disp_flag |= 32U;
        work.move_flag |= 8448U;
        gmsBosS1MgrWork.life = GmBsCmnIsFinalZoneType(work) == 0 ? 8 : 4;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1MgrWaitLoad);
        return work;
    }

    private static void gmBoss1SetPartTextureBurnt(OBS_OBJECT_WORK obj_work)
    {
        MTM_ASSERT(obj_work);
        MTM_ASSERT(obj_work.obj_3d);
        MTM_ASSERT(obj_work.disp_flag & 134217728U);
        obj_work.obj_3d.drawflag |= 268435456U;
        obj_work.obj_3d.draw_state.texoffset[0].mode = 2;
        obj_work.obj_3d.draw_state.texoffset[0].u = 0.5f;
    }

    private static bool gmBoss1IsScrollLockBusy()
    {
        return ((int)g_gm_main_system.game_flag & 32768) != 0;
    }

    private static void gmBoss1Init1ShotTimer(
      GMS_BOSS1_1SHOT_TIMER one_shot_timer,
      uint frame)
    {
        MTM_ASSERT(one_shot_timer);
        one_shot_timer.timer = frame;
        one_shot_timer.is_active = true;
    }

    private static bool gmBoss1Update1ShotTimer(GMS_BOSS1_1SHOT_TIMER one_shot_timer)
    {
        MTM_ASSERT(one_shot_timer);
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
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_BOSS1_FLASH_SCREEN_WORK(), null, 0, "boss1_flash_scr");
        GMS_BOSS1_FLASH_SCREEN_WORK s1FlashScreenWork = (GMS_BOSS1_FLASH_SCREEN_WORK)work;
        work.disp_flag |= 4128U;
        work.flag |= 16U;
        GmBsCmnInitFlashScreen(s1FlashScreenWork.flash_work, 4f, 5f, 30f);
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1FlashScreenMain);
    }

    private static void gmBoss1FlashScreenMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_FLASH_SCREEN_WORK s1FlashScreenWork = (GMS_BOSS1_FLASH_SCREEN_WORK)obj_work;
        if (GmBsCmnUpdateFlashScreen(s1FlashScreenWork.flash_work) == 0)
            return;
        GmBsCmnClearFlashScreen(s1FlashScreenWork.flash_work);
        obj_work.flag |= 4U;
    }
}