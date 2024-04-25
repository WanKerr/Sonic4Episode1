public partial class AppMain
{
    public static void GmPauseInit()
    {
        ObjObjectPause(2);
        g_gm_main_system.game_flag |= 64U;
        g_gm_main_system.game_flag &= 4294967167U;
        uint num = g_gm_main_system.game_flag & 3072U;
        g_gm_main_system.game_flag &= 4294964223U;
        gm_pause_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmPauseMain), new GSF_TASK_PROCEDURE(gmPauseDest), 0U, 3, 28928U, 6, () => new GMS_PAUSE_WORK(), "GM_PAUSE");
        GMS_PAUSE_WORK work = (GMS_PAUSE_WORK)gm_pause_tcb.work;
        work.Clear();
        work.time_count_flag_save = num;
        gmPauseProcUpdateInit(work);
    }

    private static void GmPauseExit()
    {
        if (!GmPauseMenuIsFinished())
            GmPauseMenuCancel();
        if (gm_pause_tcb == null)
            return;
        mtTaskClearTcb(gm_pause_tcb);
        gm_pause_tcb = null;
    }

    public static bool GmPauseCheckExecutable()
    {
        return gm_pause_tcb == null && ((int)g_gm_main_system.game_flag & 32968936) == 0 && (g_gm_main_system.ply_work[0] != null && ((int)g_gm_main_system.ply_work[0].player_flag & GMD_PLF_DIE) == 0);
    }

    private static void gmPauseDest(MTS_TASK_TCB tcb)
    {
        GMS_PAUSE_WORK work = (GMS_PAUSE_WORK)tcb.work;
        g_gm_main_system.game_flag |= work.time_count_flag_save & 3072U;
        g_gm_main_system.game_flag &= 4294967231U;
        gm_pause_tcb = null;
    }

    private static void gmPauseMain(MTS_TASK_TCB tcb)
    {
        GMS_PAUSE_WORK work = (GMS_PAUSE_WORK)tcb.work;
        if (work.proc_update != null)
            work.proc_update(work);
        if (((int)work.flag & 1) == 0)
            return;
        mtTaskClearTcb(tcb);
    }

    private static void gmPauseExecRecoverRoutine(GMS_PAUSE_WORK pause_work, bool b_rec_snd)
    {
        ObjObjectPauseOut();
        pause_work.flag |= 1U;
        if (!b_rec_snd)
            return;
        GmSoundAllResume();
    }

    private static void gmPauseProcUpdateInit(GMS_PAUSE_WORK pause_work)
    {
        pause_work.proc_update = new GMS_PAUSE_WORK._proc_update_(gmPauseProcUpdatePauseMenuStart);
    }

    private static void gmPauseProcUpdateReinit(GMS_PAUSE_WORK pause_work)
    {
        GmPauseMenuStart(28928U);
        pause_work.proc_update = new GMS_PAUSE_WORK._proc_update_(gmPauseProcUpdateWaitDecision);
    }

    private static void gmPauseProcUpdatePauseMenuStart(GMS_PAUSE_WORK pause_work)
    {
        GmSoundAllPause();
        GmPauseMenuStart(28928U);
        pause_work.proc_update = new GMS_PAUSE_WORK._proc_update_(gmPauseProcUpdateWaitDecision);
    }

    private static void gmPauseProcUpdateWaitDecision(GMS_PAUSE_WORK pause_work)
    {
        if (!GmPauseMenuIsFinished())
            return;
        int result = GmPauseMenuGetResult();
        bool flag = false;
        switch (result)
        {
            case 0:
            case 2:
            case 3:
                SaveState.deleteSave();
                flag = true;
                pause_work.proc_update = new GMS_PAUSE_WORK._proc_update_(gmPauseProcUpdateFadeOutToExitGame);
                break;
            case 1:
                flag = true;
                pause_work.proc_update = new GMS_PAUSE_WORK._proc_update_(gmPauseProcUpdateFadeOutToOption);
                break;
            default:
                gmPauseExecRecoverRoutine(pause_work, true);
                pause_work.proc_update = null;
                break;
        }
        if (!flag)
            return;
        IzFadeInitEasyColor(0, (ushort)short.MaxValue, IZD_FADE_DT_PRIO_DEF, 18U, 0U, 1U, 20f, true);
    }

    private static void gmPauseProcUpdateFadeOutToOption(GMS_PAUSE_WORK pause_work)
    {
        if (!IzFadeIsEnd())
            return;
        DmOptionStart(null);
        pause_work.proc_update = new GMS_PAUSE_WORK._proc_update_(gmPauseProcUpdateWaitRecover);
    }

    private static void gmPauseProcUpdateWaitRecover(GMS_PAUSE_WORK pause_work)
    {
        bool flag = true;
        if (!DmOptionIsExit())
            flag = false;
        if (!flag)
            return;
        IzFadeInitEasyColor(0, (ushort)short.MaxValue, IZD_FADE_DT_PRIO_DEF, 18U, 1U, 0U, 20f, true);
        pause_work.proc_update = new GMS_PAUSE_WORK._proc_update_(gmPauseProcUpdateFadeInFromDemo);
    }

    private static void gmPauseProcUpdateFadeInFromDemo(GMS_PAUSE_WORK pause_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        gmPauseProcUpdateReinit(pause_work);
    }

    private static void gmPauseProcUpdateFadeOutToExitGame(GMS_PAUSE_WORK pause_work)
    {
        if (!IzFadeIsEnd())
            return;
        g_gm_main_system.game_flag |= 128U;
        gmPauseExecRecoverRoutine(pause_work, false);
    }
}