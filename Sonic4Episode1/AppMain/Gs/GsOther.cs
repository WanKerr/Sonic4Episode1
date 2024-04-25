public partial class AppMain
{
    private static void GsInitOtherStart()
    {
        GsGetMainSysInfo().is_save_run = 0U;
        g_gs_init_tcb = amTaskMake(new TaskProc(gsInitTaskProcedure), new TaskProc(gsInitTaskDestructor), 0U, 0U, 0U, "gsInit");
        g_gs_init_tcb.work = new GSS_INIT_WORK();
        GSS_INIT_WORK work = (GSS_INIT_WORK)amTaskGetWork(g_gs_init_tcb);
        work.count = 0;
        work.fs = null;
        work.proc = new GSS_INIT_WORK.ProcDelegate(gsInitProcSysFirst);
        amTaskStart(g_gs_init_tcb);
    }

    private static bool GsInitOtherIsInitialized()
    {
        return g_gs_init_tcb == null;
    }

    private static void GsOtherExit()
    {
    }
}