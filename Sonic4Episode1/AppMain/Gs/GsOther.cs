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
    private static void GsInitOtherStart()
    {
        AppMain.GsGetMainSysInfo().is_save_run = 0U;
        AppMain.g_gs_init_tcb = AppMain.amTaskMake(new AppMain.TaskProc(AppMain.gsInitTaskProcedure), new AppMain.TaskProc(AppMain.gsInitTaskDestructor), 0U, 0U, 0U, "gsInit");
        AppMain.g_gs_init_tcb.work = (object)new AppMain.GSS_INIT_WORK();
        AppMain.GSS_INIT_WORK work = (AppMain.GSS_INIT_WORK)AppMain.amTaskGetWork(AppMain.g_gs_init_tcb);
        work.count = 0;
        work.fs = (AppMain.AMS_FS)null;
        work.proc = new AppMain.GSS_INIT_WORK.ProcDelegate(AppMain.gsInitProcSysFirst);
        AppMain.amTaskStart(AppMain.g_gs_init_tcb);
    }

    private static bool GsInitOtherIsInitialized()
    {
        return AppMain.g_gs_init_tcb == null;
    }

    private static void GsOtherExit()
    {
    }
}