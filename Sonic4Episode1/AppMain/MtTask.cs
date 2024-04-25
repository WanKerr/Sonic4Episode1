public partial class AppMain
{
    public static MTS_TASK_TCB MTM_TASK_MAKE_TCB(
         GSF_TASK_PROCEDURE proc,
         GSF_TASK_PROCEDURE dest,
         uint flag,
         ushort pause_level,
         uint prio,
         int group,
         TaskWorkFactoryDelegate work_size,
         string name)
    {
        return mtTaskMake(proc, dest, flag, pause_level, prio, group, work_size, name);
    }

    private static void mtTaskInitSystem()
    {
        if (gs_task_mt_system_tcb != null)
            return;
        gs_task_mt_system_tcb = mtTaskMake(new GSF_TASK_PROCEDURE(mtTaskSystemMain), new GSF_TASK_PROCEDURE(mtTaskSystemDest), 2147483648U, ushort.MaxValue, 0U, 15, null, "GS_TASKMT_SYS");
        gs_task_mtsys = new GSS_TASK_SYS();
        gs_task_mtsys.pause_level = -1;
        gs_task_mtsys.pause_level_set = -1;
    }

    private static void mtTaskExitSystem()
    {
        mtTaskClearTcb(gs_task_mt_system_tcb);
    }

    public static MTS_TASK_TCB mtTaskMake(
      GSF_TASK_PROCEDURE proc,
      GSF_TASK_PROCEDURE dest,
      uint flag,
      ushort pause_level,
      uint prio,
      int group,
      TaskWorkFactoryDelegate work_size,
      string name)
    {
        if (((int)flag & int.MinValue) == 0 && (uint)group >= 15U)
            group = 14;
        AMS_TCB amsTcb = amTaskMake(_mtTaskProcedure, _mtTaskDestructor, prio, (uint)(1 << group), 2U, name);
        amsTcb.work = new MTS_TASK_TCB();
        MTS_TASK_TCB work = (MTS_TASK_TCB)amTaskGetWork(amsTcb);
        work.am_tcb = amsTcb;
        work.proc = proc;
        work.dest = dest;
        work.pause_level = pause_level;
        if (((int)flag & 1) != 0)
            work.pause_level = ushort.MaxValue;
        work.work = null;
        if (work_size != null)
            work.work = work_size();
        amTaskStart(amsTcb);
        return work;
    }

    private static AMS_TCB mtTaskGetAmTcb(MTS_TASK_TCB tcb)
    {
        return tcb.am_tcb;
    }

    private static OBS_OBJECT_WORK mtTaskGetTcbWork(MTS_TASK_TCB tcb)
    {
        return tcb.work is IOBS_OBJECT_WORK ? ((IOBS_OBJECT_WORK)tcb.work).Cast() : (OBS_OBJECT_WORK)tcb.work;
    }

    private static void mtTaskChangeTcbProcedure(
      MTS_TASK_TCB tcb,
      GSF_TASK_PROCEDURE proc)
    {
        tcb.proc = proc;
    }

    private static void mtTaskChangeTcbDestructor(
      MTS_TASK_TCB tcb,
      GSF_TASK_PROCEDURE dest)
    {
        tcb.dest = dest;
    }

    private static void mtTaskClearTcb(MTS_TASK_TCB tcb)
    {
        amTaskDelete(tcb.am_tcb);
    }

    private static void mtTaskClearTcbAll()
    {
        amTaskDeleteGroup(uint.MaxValue, 2U, 1U);
    }

    private static void mtTaskClearPriority(uint prio_begin, uint prio_end)
    {
        amTaskDeletePriority(prio_begin, prio_end, uint.MaxValue);
    }

    private static void mtTaskClearGroup(int group)
    {
        amTaskDeleteGroup((uint)(1 << group), 2U, 1U);
    }

    private static void mtTaskStartPause(ushort pause_level)
    {
        if (pause_level >= ushort.MaxValue)
            pause_level = 65534;
        gs_task_mtsys.pause_level_set = pause_level;
    }

    private static void mtTaskEndPause()
    {
        gs_task_mtsys.pause_level_set = -1;
    }

    private static bool mtTaskIsPaused(ref ushort pause_level)
    {
        if (pause_level != 0)
            pause_level = gs_task_mtsys.pause_level_set < 0 ? (ushort)0 : (ushort)gs_task_mtsys.pause_level_set;
        return gs_task_mtsys.pause_level_set >= 0;
    }

    private static void mtTaskSystemMain(MTS_TASK_TCB tcb)
    {
        gs_task_mtsys.pause_level = gs_task_mtsys.pause_level_set;
    }

    private static void mtTaskSystemDest(MTS_TASK_TCB tcb)
    {
        gs_task_mt_system_tcb = null;
        gs_task_mtsys = new GSS_TASK_SYS();
    }

    public static void mtTaskProcedure(AMS_TCB am_tcb)
    {
        MTS_TASK_TCB work = (MTS_TASK_TCB)amTaskGetWork(am_tcb);
        if (work.pause_level <= gs_task_mtsys.pause_level || work.proc == null)
            return;
        work.proc(work);
    }

    public static void mtTaskDestructor(AMS_TCB am_tcb)
    {
        MTS_TASK_TCB work1 = (MTS_TASK_TCB)amTaskGetWork(am_tcb);
        if (work1.dest != null)
            work1.dest(work1);
        if (work1.work != null && work1.work is GMS_EFFECT_3DES_WORK)
        {
            GMS_EFFECT_3DES_WORK work2 = (GMS_EFFECT_3DES_WORK)work1.work;
            work2.Clear();
            GMS_EFFECT_3DES_WORK_Pool.Release(work2);
        }
        work1.work = null;
    }

}