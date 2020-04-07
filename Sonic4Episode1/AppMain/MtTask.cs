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
    public static AppMain.MTS_TASK_TCB MTM_TASK_MAKE_TCB(
         AppMain.GSF_TASK_PROCEDURE proc,
         AppMain.GSF_TASK_PROCEDURE dest,
         uint flag,
         ushort pause_level,
         uint prio,
         int group,
         AppMain.TaskWorkFactoryDelegate work_size,
         string name)
    {
        return AppMain.mtTaskMake(proc, dest, flag, pause_level, prio, group, work_size, name);
    }

    private static void mtTaskInitSystem()
    {
        if (AppMain.gs_task_mt_system_tcb != null)
            return;
        AppMain.gs_task_mt_system_tcb = AppMain.mtTaskMake(new AppMain.GSF_TASK_PROCEDURE(AppMain.mtTaskSystemMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.mtTaskSystemDest), 2147483648U, ushort.MaxValue, 0U, 15, (AppMain.TaskWorkFactoryDelegate)null, "GS_TASKMT_SYS");
        AppMain.gs_task_mtsys = new AppMain.GSS_TASK_SYS();
        AppMain.gs_task_mtsys.pause_level = -1;
        AppMain.gs_task_mtsys.pause_level_set = -1;
    }

    private static void mtTaskExitSystem()
    {
        AppMain.mtTaskClearTcb(AppMain.gs_task_mt_system_tcb);
    }

    public static AppMain.MTS_TASK_TCB mtTaskMake(
      AppMain.GSF_TASK_PROCEDURE proc,
      AppMain.GSF_TASK_PROCEDURE dest,
      uint flag,
      ushort pause_level,
      uint prio,
      int group,
      AppMain.TaskWorkFactoryDelegate work_size,
      string name)
    {
        if (((int)flag & int.MinValue) == 0 && (uint)group >= 15U)
            group = 14;
        AppMain.AMS_TCB amsTcb = AppMain.amTaskMake(AppMain._mtTaskProcedure, AppMain._mtTaskDestructor, prio, (uint)(1 << group), 2U, name);
        amsTcb.work = (object)new AppMain.MTS_TASK_TCB();
        AppMain.MTS_TASK_TCB work = (AppMain.MTS_TASK_TCB)AppMain.amTaskGetWork(amsTcb);
        work.am_tcb = amsTcb;
        work.proc = proc;
        work.dest = dest;
        work.pause_level = pause_level;
        if (((int)flag & 1) != 0)
            work.pause_level = ushort.MaxValue;
        work.work = (object)null;
        if (work_size != null)
            work.work = work_size();
        AppMain.amTaskStart(amsTcb);
        return work;
    }

    private static AppMain.AMS_TCB mtTaskGetAmTcb(AppMain.MTS_TASK_TCB tcb)
    {
        return tcb.am_tcb;
    }

    private static AppMain.OBS_OBJECT_WORK mtTaskGetTcbWork(AppMain.MTS_TASK_TCB tcb)
    {
        return tcb.work is AppMain.IOBS_OBJECT_WORK ? ((AppMain.IOBS_OBJECT_WORK)tcb.work).Cast() : (AppMain.OBS_OBJECT_WORK)tcb.work;
    }

    private static void mtTaskChangeTcbProcedure(
      AppMain.MTS_TASK_TCB tcb,
      AppMain.GSF_TASK_PROCEDURE proc)
    {
        tcb.proc = proc;
    }

    private static void mtTaskChangeTcbDestructor(
      AppMain.MTS_TASK_TCB tcb,
      AppMain.GSF_TASK_PROCEDURE dest)
    {
        tcb.dest = dest;
    }

    private static void mtTaskClearTcb(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.amTaskDelete(tcb.am_tcb);
    }

    private static void mtTaskClearTcbAll()
    {
        AppMain.amTaskDeleteGroup(uint.MaxValue, 2U, 1U);
    }

    private static void mtTaskClearPriority(uint prio_begin, uint prio_end)
    {
        AppMain.amTaskDeletePriority(prio_begin, prio_end, uint.MaxValue);
    }

    private static void mtTaskClearGroup(int group)
    {
        AppMain.amTaskDeleteGroup((uint)(1 << group), 2U, 1U);
    }

    private static void mtTaskStartPause(ushort pause_level)
    {
        if (pause_level >= ushort.MaxValue)
            pause_level = (ushort)65534;
        AppMain.gs_task_mtsys.pause_level_set = (int)pause_level;
    }

    private static void mtTaskEndPause()
    {
        AppMain.gs_task_mtsys.pause_level_set = -1;
    }

    private static bool mtTaskIsPaused(ref ushort pause_level)
    {
        if (pause_level != (ushort)0)
            pause_level = AppMain.gs_task_mtsys.pause_level_set < 0 ? (ushort)0 : (ushort)AppMain.gs_task_mtsys.pause_level_set;
        return AppMain.gs_task_mtsys.pause_level_set >= 0;
    }

    private static void mtTaskSystemMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gs_task_mtsys.pause_level = AppMain.gs_task_mtsys.pause_level_set;
    }

    private static void mtTaskSystemDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gs_task_mt_system_tcb = (AppMain.MTS_TASK_TCB)null;
        AppMain.gs_task_mtsys = new AppMain.GSS_TASK_SYS();
    }

    public static void mtTaskProcedure(AppMain.AMS_TCB am_tcb)
    {
        AppMain.MTS_TASK_TCB work = (AppMain.MTS_TASK_TCB)AppMain.amTaskGetWork(am_tcb);
        if ((int)work.pause_level <= AppMain.gs_task_mtsys.pause_level || work.proc == null)
            return;
        work.proc(work);
    }

    public static void mtTaskDestructor(AppMain.AMS_TCB am_tcb)
    {
        AppMain.MTS_TASK_TCB work1 = (AppMain.MTS_TASK_TCB)AppMain.amTaskGetWork(am_tcb);
        if (work1.dest != null)
            work1.dest(work1);
        if (work1.work != null && work1.work is AppMain.GMS_EFFECT_3DES_WORK)
        {
            AppMain.GMS_EFFECT_3DES_WORK work2 = (AppMain.GMS_EFFECT_3DES_WORK)work1.work;
            work2.Clear();
            AppMain.GMS_EFFECT_3DES_WORK_Pool.Release(work2);
        }
        work1.work = (object)null;
    }

}