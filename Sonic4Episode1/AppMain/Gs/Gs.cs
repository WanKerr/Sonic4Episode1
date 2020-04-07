using System;
using System.Collections.Generic;
using System.Text;

partial class AppMain
{
    public static int GSD_DISP_WIDTH
    {
        get
        {
            return (int)AppMain.g_gs_main_sys_info.sys_disp_width;
        }
    }

    public static int GSD_DISP_HEIGHT
    {
        get
        {
            return (int)AppMain.g_gs_main_sys_info.sys_disp_height;
        }
    }

    public static bool GSM_MAIN_STAGE_IS_SPSTAGE()
    {
        return ((int)AppMain.g_gs_main_sys_info.game_flag & 128) != 0;
    }

    public static bool GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY()
    {
        return ((int)AppMain.g_gs_main_sys_info.game_flag & 128) != 0 && ((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 65536) == 0;
    }

    public static AppMain.GSS_MAIN_SYS_INFO GsGetMainSysInfo()
    {
        return g_gs_main_sys_info;
    }

    public static int GsMainSysGetDisplayListRegistNum()
    {
        return AppMain._am_displaylist_manager.regist_num + AppMain._am_displaylist_manager.reg_write_num;
    }

    public static int GsGetGameLevel()
    {
        return AppMain.g_gs_main_sys_info.level;
    }

    private static void GsSetGameLevel(int level)
    {
        AppMain.g_gs_main_sys_info.level = level;
    }

    private static void GsReqExit()
    {
    }

    private static void GsInitUser()
    {
        AppMain.GsMainSysInfoInit(AppMain.GsGetMainSysInfo());
        AppMain.mtTaskInitSystem();
        AppMain.SyInitEvtSys(AppMain._gs_evt_data, 15, (short)1, true, (ushort)256, (byte)14);
        AppMain.GsSoundInit();
    }

    private static void GsExitUser()
    {
        AppMain.GsOtherExit();
        AppMain.GsSoundExit();
    }

    private static void GsTrophyInit()
    {
    }

    private static void GsTrophyExit()
    {
    }

    private static void GsTrophyResetForAccount()
    {
        AppMain.gsTrophyResetAcquisitionTable();
    }

    private static void GsTrophyUpdateAcquisition(uint trophy_id)
    {
        if (AppMain.gs_trophy_acquisition_tbl[(int)trophy_id] != 0)
            return;
        AppMain.gs_trophy_acquisition_tbl[(int)trophy_id] = 1;
        AppMain.gsTrophyTriggerAquisition(trophy_id);
    }

    private static bool GsTrophyIsAcquired(uint trophy_id)
    {
        return AppMain.gs_trophy_acquisition_tbl[(int)trophy_id] != 0;
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
        Array.Clear((Array)AppMain.gs_trophy_acquisition_tbl, 0, 16);
    }

    private static void gsTrophyTriggerAquisition(uint trophy_no)
    {
        LiveFeature.getInstance().GotAchievment(AppMain.achievements[(int)trophy_no].id);
    }

    private static void GsRebootInit()
    {
        AppMain.amSystemLog("gsReboot Initializing...\n");
        AppMain.amSystemLog("gsReboot Initialized.\n");
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
