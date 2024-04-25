using System;

partial class AppMain
{
    public static int GSD_DISP_WIDTH => (int)g_gs_main_sys_info.sys_disp_width;

    public static int GSD_DISP_HEIGHT => (int)g_gs_main_sys_info.sys_disp_height;

    public static bool GSM_MAIN_STAGE_IS_SPSTAGE()
    {
        return ((int)g_gs_main_sys_info.game_flag & 128) != 0;
    }

    public static bool GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY()
    {
        return ((int)g_gs_main_sys_info.game_flag & 128) != 0 && ((int)g_gm_main_system.ply_work[0].player_flag & 65536) == 0;
    }

    public static GSS_MAIN_SYS_INFO GsGetMainSysInfo()
    {
        return g_gs_main_sys_info;
    }

    public static int GsMainSysGetDisplayListRegistNum()
    {
        return _am_displaylist_manager.regist_num + _am_displaylist_manager.reg_write_num;
    }

    public static int GsGetGameLevel()
    {
        return g_gs_main_sys_info.level;
    }

    private static void GsSetGameLevel(int level)
    {
        g_gs_main_sys_info.level = level;
    }

    private static void GsReqExit()
    {
    }

    private static void GsInitUser()
    {
        GsMainSysInfoInit(GsGetMainSysInfo());
        mtTaskInitSystem();
        SyInitEvtSys(_gs_evt_data, 15, 1, true, 256, 14);
        GsSoundInit();
    }

    private static void GsExitUser()
    {
        GsOtherExit();
        GsSoundExit();
    }

    private static void GsTrophyInit()
    {
    }

    private static void GsTrophyExit()
    {
    }

    private static void GsTrophyResetForAccount()
    {
        gsTrophyResetAcquisitionTable();
    }

    private static void GsTrophyUpdateAcquisition(uint trophy_id)
    {
        if (gs_trophy_acquisition_tbl[(int)trophy_id] != 0)
            return;
        gs_trophy_acquisition_tbl[(int)trophy_id] = 1;
        gsTrophyTriggerAquisition(trophy_id);
    }

    private static bool GsTrophyIsAcquired(uint trophy_id)
    {
        return gs_trophy_acquisition_tbl[(int)trophy_id] != 0;
    }

    private static void GsTrophyAvatarUpdateAcquisition(uint avaw_id)
    {
    }

    private static bool GsTrophyAvatarIsAcquired(uint avaw_id)
    {
        return true;
    }

    private static void gsTrophyResetAcquisitionTable()
    {
        Array.Clear(gs_trophy_acquisition_tbl, 0, 16);
    }

    private static void gsTrophyTriggerAquisition(uint trophy_no)
    {
        LiveFeature.getInstance().GotAchievment(achievements[(int)trophy_no].id);
    }

    private static void GsRebootInit()
    {
        amSystemLog("gsReboot Initializing...\n");
        amSystemLog("gsReboot Initialized.\n");
    }

    private static void GsRebootExit()
    {
    }

    private static bool GsRebootIsReboot()
    {
        return false;
    }

    private static bool GsRebootIsTitleReboot()
    {
        return false;
    }

    private static void GsRebootSetTitle()
    {
    }

    private static bool GsRebootIsTitle()
    {
        return false;
    }

    public static void GsTrialInitStart()
    {
    }

    public static bool GsTrialInitIsFinished()
    {
        return true;
    }

    public static void GsTrialExit()
    {
    }

    public static bool GsTrialIsTrial()
    {
        return XBOXLive.isTrial();
    }

    public static void GsTrialCheckStart()
    {
    }

    public static bool GsTrialCheckIsFinished()
    {
        return true;
    }

    public static void GsTrialStoreStart()
    {
    }

    public static bool GsTrialStoreIsFinished()
    {
        return true;
    }

    public static void GsTrialDebugSetTrial(bool is_trial)
    {
    }
}
