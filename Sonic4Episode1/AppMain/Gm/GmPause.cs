using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    public static void GmPauseInit()
    {
        AppMain.ObjObjectPause((ushort)2);
        AppMain.g_gm_main_system.game_flag |= 64U;
        AppMain.g_gm_main_system.game_flag &= 4294967167U;
        uint num = AppMain.g_gm_main_system.game_flag & 3072U;
        AppMain.g_gm_main_system.game_flag &= 4294964223U;
        AppMain.gm_pause_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmPauseMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmPauseDest), 0U, (ushort)3, 28928U, 6, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_PAUSE_WORK()), "GM_PAUSE");
        AppMain.GMS_PAUSE_WORK work = (AppMain.GMS_PAUSE_WORK)AppMain.gm_pause_tcb.work;
        work.Clear();
        work.time_count_flag_save = num;
        AppMain.gmPauseProcUpdateInit(work);
    }

    private static void GmPauseExit()
    {
        if (!AppMain.GmPauseMenuIsFinished())
            AppMain.GmPauseMenuCancel();
        if (AppMain.gm_pause_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_pause_tcb);
        AppMain.gm_pause_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    public static bool GmPauseCheckExecutable()
    {
        return AppMain.gm_pause_tcb == null && ((int)AppMain.g_gm_main_system.game_flag & 32968936) == 0 && (AppMain.g_gm_main_system.ply_work[0] != null && ((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 1024) == 0);
    }

    private static void gmPauseDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_PAUSE_WORK work = (AppMain.GMS_PAUSE_WORK)tcb.work;
        AppMain.g_gm_main_system.game_flag |= work.time_count_flag_save & 3072U;
        AppMain.g_gm_main_system.game_flag &= 4294967231U;
        AppMain.gm_pause_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmPauseMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_PAUSE_WORK work = (AppMain.GMS_PAUSE_WORK)tcb.work;
        if (work.proc_update != null)
            work.proc_update(work);
        if (((int)work.flag & 1) == 0)
            return;
        AppMain.mtTaskClearTcb(tcb);
    }

    private static void gmPauseExecRecoverRoutine(AppMain.GMS_PAUSE_WORK pause_work, bool b_rec_snd)
    {
        AppMain.ObjObjectPauseOut();
        pause_work.flag |= 1U;
        if (!b_rec_snd)
            return;
        AppMain.GmSoundAllResume();
    }

    private static void gmPauseProcUpdateInit(AppMain.GMS_PAUSE_WORK pause_work)
    {
        pause_work.proc_update = new AppMain.GMS_PAUSE_WORK._proc_update_(AppMain.gmPauseProcUpdatePauseMenuStart);
    }

    private static void gmPauseProcUpdateReinit(AppMain.GMS_PAUSE_WORK pause_work)
    {
        AppMain.GmPauseMenuStart(28928U);
        pause_work.proc_update = new AppMain.GMS_PAUSE_WORK._proc_update_(AppMain.gmPauseProcUpdateWaitDecision);
    }

    private static void gmPauseProcUpdatePauseMenuStart(AppMain.GMS_PAUSE_WORK pause_work)
    {
        AppMain.GmSoundAllPause();
        AppMain.GmPauseMenuStart(28928U);
        pause_work.proc_update = new AppMain.GMS_PAUSE_WORK._proc_update_(AppMain.gmPauseProcUpdateWaitDecision);
    }

    private static void gmPauseProcUpdateWaitDecision(AppMain.GMS_PAUSE_WORK pause_work)
    {
        if (!AppMain.GmPauseMenuIsFinished())
            return;
        int result = AppMain.GmPauseMenuGetResult();
        bool flag = false;
        switch (result)
        {
            case 0:
            case 2:
            case 3:
                SaveState.deleteSave();
                flag = true;
                pause_work.proc_update = new AppMain.GMS_PAUSE_WORK._proc_update_(AppMain.gmPauseProcUpdateFadeOutToExitGame);
                break;
            case 1:
                flag = true;
                pause_work.proc_update = new AppMain.GMS_PAUSE_WORK._proc_update_(AppMain.gmPauseProcUpdateFadeOutToOption);
                break;
            default:
                AppMain.gmPauseExecRecoverRoutine(pause_work, true);
                pause_work.proc_update = (AppMain.GMS_PAUSE_WORK._proc_update_)null;
                break;
        }
        if (!flag)
            return;
        AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 1U, 20f, true);
    }

    private static void gmPauseProcUpdateFadeOutToOption(AppMain.GMS_PAUSE_WORK pause_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.DmOptionStart((object)null);
        pause_work.proc_update = new AppMain.GMS_PAUSE_WORK._proc_update_(AppMain.gmPauseProcUpdateWaitRecover);
    }

    private static void gmPauseProcUpdateWaitRecover(AppMain.GMS_PAUSE_WORK pause_work)
    {
        bool flag = true;
        if (!AppMain.DmOptionIsExit())
            flag = false;
        if (!flag)
            return;
        AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 1U, 0U, 20f, true);
        pause_work.proc_update = new AppMain.GMS_PAUSE_WORK._proc_update_(AppMain.gmPauseProcUpdateFadeInFromDemo);
    }

    private static void gmPauseProcUpdateFadeInFromDemo(AppMain.GMS_PAUSE_WORK pause_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        AppMain.gmPauseProcUpdateReinit(pause_work);
    }

    private static void gmPauseProcUpdateFadeOutToExitGame(AppMain.GMS_PAUSE_WORK pause_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.g_gm_main_system.game_flag |= 128U;
        AppMain.gmPauseExecRecoverRoutine(pause_work, false);
    }
}