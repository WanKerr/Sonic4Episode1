using System;

public partial class AppMain
{


    private static void gsInitTaskProcedure(AMS_TCB tcb)
    {
        GSS_INIT_WORK work = (GSS_INIT_WORK)amTaskGetWork(tcb);
        if (work.proc == null)
        {
            amTaskDelete(tcb);
        }
        else
        {
            Delegate proc = work.proc;
            work.proc(work);
            if (work.proc != proc)
            {
                work.count = 0;
            }
            else
            {
                if (work.count >= -1)
                    return;
                ++work.count;
            }
        }
    }

    private static void gsInitTaskDestructor(AMS_TCB tcb)
    {
        g_gs_init_tcb = null;
    }

    private static void gsInitProcSysFirst(GSS_INIT_WORK work)
    {
        GsEnvInit();
        GsFontInit();
        AoSysInit();
        AoActSysInit(256U, 256U, 256U, 32U);
        GsSystemBgmInit();
        GsSystemBgmSetEnable(true);
        GsRebootInit();
        bool flag = true;
        GsRebootIsTitleReboot();
        if (flag)
            IzFadeInitEasyTask(0U, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 1f);
        else
            IzFadeInitEasyTask(0U, 0, 0, 0, byte.MaxValue, 0, 0, 0, byte.MaxValue, 1f);
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcStrapLoad);
    }

    private static void gsInitProcStrapLoad(GSS_INIT_WORK work)
    {
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcLoadLoadingFile);
    }

    private static void gsInitProcLoadLoadingFile(GSS_INIT_WORK work)
    {
        if (work.fs == null)
            work.fs = amFsReadBackground(g_gs_init_loading_file_path);
        if (!amFsIsComplete(work.fs))
            return;
        DmLoadingBuild(work.fs);
        amFsClearRequest(work.fs);
        work.fs = null;
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcBuildLoadingFile);
    }

    private static void gsInitProcBuildLoadingFile(GSS_INIT_WORK work)
    {
        if (!DmLoadingBuildCheck())
            return;
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcLoadSysMsgFile);
    }

    private static void gsInitProcLoadSysMsgFile(GSS_INIT_WORK work)
    {
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcLoadSaveMsgFile);
    }

    private static void gsInitProcLoadSaveMsgFile(GSS_INIT_WORK work)
    {
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcSysLast);
    }

    private static void gsInitProcSysLast(GSS_INIT_WORK work)
    {
        AoAccountInit();
        //AoStorageInit();
        //DmRankSysInit();
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcCheckTrial);
    }

    private static void gsInitProcCheckTrial(GSS_INIT_WORK work)
    {
        if (work.count == 0)
            GsTrialInitStart();
        if (!GsTrialInitIsFinished())
            return;
        amSystemLog("\n================================================\n");
        if (GsTrialIsTrial())
            amSystemLog("gsInit - product mode : Trial\n");
        else
            amSystemLog("gsInit - product mode : Full\n");
        amSystemLog("================================================\n\n");
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcInitTorphy);
    }

    private static void gsInitProcInitTorphy(GSS_INIT_WORK work)
    {
        GsTrophyInit();
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcInstallTorphy);
    }

    private static void gsInitProcInstallTorphy(GSS_INIT_WORK work)
    {
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcPresence);
    }

    private static void gsInitProcPresence(GSS_INIT_WORK work)
    {
        if (work.count == 0)
            AoPresenceInit();
        if (!AoPresenceInitialized())
            return;
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcWaitPadEnable);
    }

    private static void gsInitProcWaitPadEnable(GSS_INIT_WORK work)
    {
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcEnd);
    }

    private static void gsInitProcEnd(GSS_INIT_WORK work)
    {
        work.proc = null;
    }

}