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


    private static void gsInitTaskProcedure(AppMain.AMS_TCB tcb)
    {
        AppMain.GSS_INIT_WORK work = (AppMain.GSS_INIT_WORK)AppMain.amTaskGetWork(tcb);
        if (work.proc == null)
        {
            AppMain.amTaskDelete(tcb);
        }
        else
        {
            Delegate proc = (Delegate)work.proc;
            work.proc(work);
            if ((object)work.proc != (object)proc)
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

    private static void gsInitTaskDestructor(AppMain.AMS_TCB tcb)
    {
        AppMain.g_gs_init_tcb = (AppMain.AMS_TCB)null;
    }

    private static void gsInitProcSysFirst(AppMain.GSS_INIT_WORK work)
    {
        AppMain.GsEnvInit();
        AppMain.GsFontInit();
        AppMain.AoSysInit();
        AppMain.AoActSysInit(256U, 256U, 256U, 32U);
        AppMain.GsSystemBgmInit();
        AppMain.GsSystemBgmSetEnable(true);
        AppMain.GsRebootInit();
        bool flag = true;
        AppMain.GsRebootIsTitleReboot();
        if (flag)
            AppMain.IzFadeInitEasyTask(0U, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 1f);
        else
            AppMain.IzFadeInitEasyTask(0U, (byte)0, (byte)0, (byte)0, byte.MaxValue, (byte)0, (byte)0, (byte)0, byte.MaxValue, 1f);
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcStrapLoad);
    }

    private static void gsInitProcStrapLoad(AppMain.GSS_INIT_WORK work)
    {
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcLoadLoadingFile);
    }

    private static void gsInitProcLoadLoadingFile(AppMain.GSS_INIT_WORK work)
    {
        if (work.fs == null)
            work.fs = AppMain.amFsReadBackground(AppMain.g_gs_init_loading_file_path);
        if (!AppMain.amFsIsComplete(work.fs))
            return;
        AppMain.DmLoadingBuild(work.fs);
        AppMain.amFsClearRequest(work.fs);
        work.fs = (AppMain.AMS_FS)null;
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcBuildLoadingFile);
    }

    private static void gsInitProcBuildLoadingFile(AppMain.GSS_INIT_WORK work)
    {
        if (!AppMain.DmLoadingBuildCheck())
            return;
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcLoadSysMsgFile);
    }

    private static void gsInitProcLoadSysMsgFile(AppMain.GSS_INIT_WORK work)
    {
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcLoadSaveMsgFile);
    }

    private static void gsInitProcLoadSaveMsgFile(AppMain.GSS_INIT_WORK work)
    {
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcSysLast);
    }

    private static void gsInitProcSysLast(AppMain.GSS_INIT_WORK work)
    {
        AppMain.AoAccountInit();
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcCheckTrial);
    }

    private static void gsInitProcCheckTrial(AppMain.GSS_INIT_WORK work)
    {
        if (work.count == 0)
            AppMain.GsTrialInitStart();
        if (!AppMain.GsTrialInitIsFinished())
            return;
        AppMain.amSystemLog("\n================================================\n");
        if (AppMain.GsTrialIsTrial())
            AppMain.amSystemLog("gsInit - product mode : Trial\n");
        else
            AppMain.amSystemLog("gsInit - product mode : Full\n");
        AppMain.amSystemLog("================================================\n\n");
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcInitTorphy);
    }

    private static void gsInitProcInitTorphy(AppMain.GSS_INIT_WORK work)
    {
        AppMain.GsTrophyInit();
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcInstallTorphy);
    }

    private static void gsInitProcInstallTorphy(AppMain.GSS_INIT_WORK work)
    {
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcPresence);
    }

    private static void gsInitProcPresence(AppMain.GSS_INIT_WORK work)
    {
        if (work.count == 0)
            AppMain.AoPresenceInit();
        if (!AppMain.AoPresenceInitialized())
            return;
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcWaitPadEnable);
    }

    private static void gsInitProcWaitPadEnable(AppMain.GSS_INIT_WORK work)
    {
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcEnd);
    }

    private static void gsInitProcEnd(AppMain.GSS_INIT_WORK work)
    {
        work.proc = (AppMain.GSS_INIT_WORK.ProcDelegate)null;
    }

}