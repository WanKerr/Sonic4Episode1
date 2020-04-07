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
    public static AppMain.AMS_TCB amTaskMake(
      AppMain.TaskProc proc,
      AppMain.TaskProc dest,
      uint prio,
      uint user,
      uint attr,
      string name)
    {
        return AppMain.amTaskMake(AppMain._am_default_taskp, proc, dest, prio, user, attr, name, 1U, 0, uint.MaxValue);
    }

    public static AppMain.AMS_TCB amTaskMake(
      AppMain.AMS_TASK taskp,
      AppMain.TaskProc proc,
      AppMain.TaskProc dest,
      uint prio,
      uint user,
      uint attr,
      string name)
    {
        return AppMain.amTaskMake(taskp, proc, dest, prio, user, attr, name, 1U, 0, uint.MaxValue);
    }

    public static AppMain.AMS_TCB amTaskMake(
      AppMain.TaskProc proc,
      AppMain.TaskProc dest,
      uint prio,
      uint user,
      uint attr,
      string name,
      uint stall,
      int group,
      uint run)
    {
        return AppMain.amTaskMake(AppMain._am_default_taskp, proc, dest, prio, user, attr, name, stall, group, run);
    }

    private static void amTaskSetProcedure(AppMain.AMS_TCB tcb, AppMain.TaskProc proc)
    {
        tcb.procedure = proc;
        tcb.proc_addr = int.MaxValue;
    }

    private static void amTaskSetDestructor(AppMain.AMS_TCB tcb, AppMain.TaskProc dest)
    {
        tcb.destructor = dest;
    }

    public static void amTaskDeleteGroup(uint user, uint attr, uint flag)
    {
        AppMain.amTaskDeleteGroup(AppMain._am_default_taskp, user, attr, flag);
    }

    public static void amTaskDeletePriority(uint prio_begin, uint prio_end, uint user)
    {
        AppMain.amTaskDeletePriority(AppMain._am_default_taskp, prio_begin, prio_end, user);
    }

    public static void amTaskSleepGroup(uint user, uint attr, uint flag)
    {
        AppMain.amTaskSleepGroup(AppMain._am_default_taskp, user, attr, flag);
    }

    public static void amTaskSleepPriority(uint prio_begin, uint prio_end, uint user)
    {
        AppMain.amTaskSleepPriority(AppMain._am_default_taskp, prio_begin, prio_end, user);
    }

    public static void amTaskWakeupGroup(uint user, uint attr, uint flag)
    {
        AppMain.amTaskWakeupGroup(AppMain._am_default_taskp, user, attr, flag);
    }

    public static void amTaskWakeupPriority(uint prio_begin, uint prio_end, uint user)
    {
        AppMain.amTaskWakeupPriority(AppMain._am_default_taskp, prio_begin, prio_end, user);
    }

    public static object amTaskGetWork(AppMain.AMS_TCB tcb)
    {
        return tcb.work;
    }

    private static AppMain.AMS_TCB amTaskNextTcb(AppMain.AMS_TCB tcbp)
    {
        return tcbp.next = AppMain.GlobalPool<AppMain.AMS_TCB>.Alloc();
    }

    private static AppMain.AMS_TCB amPrevNextTcb(AppMain.AMS_TCB tcbp)
    {
        return tcbp.prev = AppMain.GlobalPool<AppMain.AMS_TCB>.Alloc();
    }

    private static AppMain.AMS_TCB_FOOTER amTaskGetTcbFooter(AppMain.AMS_TCB tcbp)
    {
        return tcbp.footer;
    }

    private static AppMain.AMS_TASK amTaskInitSystem()
    {
        return AppMain.amTaskInitSystem(256, 64, 1);
    }

    private static AppMain.AMS_TASK amTaskInitSystem(
      int max_tcb,
      int work_size,
      int thread_num)
    {
        AppMain.AMS_TASK amsTask = new AppMain.AMS_TASK();
        if (AppMain._am_default_taskp == null)
            AppMain._am_default_taskp = amsTask;
        amsTask.tcb_max = max_tcb;
        amsTask.tcb_work_size = work_size + 63 & -64;
        amsTask.tcb_head.name = "TCB Head";
        amsTask.tcb_head.priority = 0U;
        amsTask.tcb_head.prev = (AppMain.AMS_TCB)null;
        amsTask.tcb_head.next = amsTask.tcb_tail;
        amsTask.tcb_head.user_id = 0U;
        amsTask.tcb_head.attribute = 1U;
        amsTask.tcb_head.wkbegin = 218237452;
        AppMain.amTaskSetProcedure(amsTask.tcb_head, (AppMain.TaskProc)null);
        AppMain.amTaskSetDestructor(amsTask.tcb_head, (AppMain.TaskProc)null);
        amsTask.tcb_tail.name = "TCB Tail";
        amsTask.tcb_tail.priority = uint.MaxValue;
        amsTask.tcb_tail.prev = amsTask.tcb_head;
        amsTask.tcb_tail.next = (AppMain.AMS_TCB)null;
        amsTask.tcb_tail.user_id = 0U;
        amsTask.tcb_tail.attribute = 1U;
        amsTask.tcb_tail.wkbegin = 218237452;
        AppMain.amTaskSetProcedure(amsTask.tcb_tail, (AppMain.TaskProc)null);
        AppMain.amTaskSetDestructor(amsTask.tcb_tail, (AppMain.TaskProc)null);
        return amsTask;
    }

    private static void amTaskExitSystem()
    {
        AppMain.amTaskExitSystem(AppMain._am_default_taskp);
    }

    private static void amTaskExitSystem(AppMain.AMS_TASK taskp)
    {
        AppMain.amMemFreeSystem((object)taskp);
    }

    private static void amTaskReset(AppMain.AMS_TASK taskp)
    {
        taskp.tcb_head.next = taskp.tcb_tail;
        taskp.tcb_tail.prev = taskp.tcb_head;
    }

    private static void amTaskExecute()
    {
        AppMain.amTaskExecute(AppMain._am_default_taskp);
    }

    private static void amTaskExecute(AppMain.AMS_TASK taskp)
    {
        AppMain._amTaskExecuteInOrder(taskp);
    }

    private static AppMain.AMS_TCB amTaskMake(
      AppMain.AMS_TASK taskp,
      AppMain.TaskProc proc,
      AppMain.TaskProc dest,
      uint prio,
      uint user,
      uint attr,
      string name,
      uint stall,
      int group,
      uint run)
    {
        AppMain.AMS_TCB amsTcb = AppMain.GlobalPool<AppMain.AMS_TCB>.Alloc();
        amsTcb.name = name;
        AppMain.AMS_TCB_FOOTER tcbFooter = AppMain.amTaskGetTcbFooter(amsTcb);
        tcbFooter.cpu_cnt = 0U;
        tcbFooter.cpu_cnt_max = 0U;
        amsTcb.priority = prio;
        amsTcb.priority = prio;
        amsTcb.user_id = user;
        amsTcb.attribute = attr;
        AppMain.amTaskSetProcedure(amsTcb, proc);
        AppMain.amTaskSetDestructor(amsTcb, dest);
        AppMain.AMS_TCB next = taskp.tcb_head.next;
        while (next != taskp.tcb_tail && next.priority <= prio)
            next = next.next;
        next.prev.next = amsTcb;
        amsTcb.prev = next.prev;
        next.prev = amsTcb;
        amsTcb.next = next;
        return amsTcb;
    }

    private int amTaskPending(AppMain.AMS_TCB tcbp)
    {
        return 0;
    }

    public static int amTaskStart(AppMain.AMS_TCB tcbp)
    {
        return 0;
    }

    public static void amTaskDelete(AppMain.AMS_TCB tcb)
    {
        if (tcb.proc_addr == -1)
            return;
        if (tcb.destructor != null)
            tcb.destructor(tcb);
        tcb.proc_addr = -1;
    }

    public static void amTaskDeleteGroup(AppMain.AMS_TASK taskp, uint user, uint attr, uint flag)
    {
        if (user == 0U)
            user = uint.MaxValue;
        AppMain.AMS_TCB next = taskp.tcb_head.next;
        switch (flag)
        {
            case 0:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != 0)
                        AppMain.amTaskDelete(next);
                }
                break;
            case 1:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == (int)attr)
                        AppMain.amTaskDelete(next);
                }
                break;
            case 2:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == 0)
                        AppMain.amTaskDelete(next);
                }
                break;
            case 3:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != (int)attr)
                        AppMain.amTaskDelete(next);
                }
                break;
        }
    }

    public static void amTaskDeletePriority(
      AppMain.AMS_TASK taskp,
      uint prio_begin,
      uint prio_end,
      uint user)
    {
        AppMain.AMS_TCB next = taskp.tcb_head.next;
        while (next != taskp.tcb_tail && next.priority < prio_begin)
            next = next.next;
        for (; next != taskp.tcb_tail && next.priority <= prio_end; next = next.next)
        {
            if (user == 0U || ((int)next.user_id & (int)user) != 0)
                AppMain.amTaskDelete(next);
        }
    }

    public static void amTaskSleep(AppMain.AMS_TCB tcb)
    {
        tcb.proc_addr |= 1;
    }

    public static void amTaskSleepGroup(AppMain.AMS_TASK taskp, uint user, uint attr, uint flag)
    {
        AppMain.mppAssertNotImpl();
        if (user == 0U)
            user = uint.MaxValue;
        AppMain.AMS_TCB next = taskp.tcb_head.next;
        switch (flag)
        {
            case 0:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != 0)
                        AppMain.amTaskSleep(next);
                }
                break;
            case 1:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == (int)attr)
                        AppMain.amTaskSleep(next);
                }
                break;
            case 2:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == 0)
                        AppMain.amTaskSleep(next);
                }
                break;
            case 3:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != (int)attr)
                        AppMain.amTaskSleep(next);
                }
                break;
        }
    }

    public static void amTaskSleepPriority(
      AppMain.AMS_TASK taskp,
      uint prio_begin,
      uint prio_end,
      uint user)
    {
        AppMain.mppAssertNotImpl();
        AppMain.AMS_TCB next = taskp.tcb_head.next;
        while (next != taskp.tcb_tail && next.priority < prio_begin)
            next = next.next;
        for (; next != taskp.tcb_tail && next.priority <= prio_end; next = next.next)
        {
            if (user == 0U || ((int)next.user_id & (int)user) != 0)
                AppMain.amTaskSleep(next);
        }
    }

    public static void amTaskWakeup(AppMain.AMS_TCB tcb)
    {
        AppMain.mppAssertNotImpl();
        tcb.proc_addr &= -2;
    }

    public static void amTaskWakeupGroup(AppMain.AMS_TASK taskp, uint user, uint attr, uint flag)
    {
        AppMain.mppAssertNotImpl();
        if (user == 0U)
            user = uint.MaxValue;
        AppMain.AMS_TCB next = taskp.tcb_head.next;
        switch (flag)
        {
            case 0:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != 0)
                        AppMain.amTaskWakeup(next);
                }
                break;
            case 1:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == (int)attr)
                        AppMain.amTaskWakeup(next);
                }
                break;
            case 2:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == 0)
                        AppMain.amTaskWakeup(next);
                }
                break;
            case 3:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != (int)attr)
                        AppMain.amTaskWakeup(next);
                }
                break;
        }
    }

    public static void amTaskWakeupPriority(
      AppMain.AMS_TASK taskp,
      uint prio_begin,
      uint prio_end,
      uint user)
    {
        AppMain.mppAssertNotImpl();
        AppMain.AMS_TCB next = taskp.tcb_head.next;
        while (next != taskp.tcb_tail && next.priority < prio_begin)
            next = next.next;
        for (; next != taskp.tcb_tail && next.priority <= prio_end; next = next.next)
        {
            if (user == 0U || ((int)next.user_id & (int)user) != 0)
                AppMain.amTaskWakeup(next);
        }
    }

    private static void amTaskSetOwnerName(AppMain.AMS_TASKLIST_OWNER[] pList, uint listSize)
    {
        AppMain.mppAssertNotImpl();
        AppMain._am_owner_list = pList;
        AppMain._am_szOwnerList = AppMain._am_owner_list != null ? (int)listSize : 0;
    }

    private static void amTaskDisplayList(AppMain.AMS_TASK taskp, int locx, int locy)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amTaskDisplayThread(AppMain.AMS_TASK taskp)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void _amTaskExecuteInOrder(AppMain.AMS_TASK taskp)
    {
        for (AppMain.AMS_TCB next = taskp.tcb_head.next; next != taskp.tcb_tail; next = next.next)
        {
            if (next.procedure != null && next.proc_addr > 0)
                next.procedure(next);
        }
        for (AppMain.AMS_TCB next = taskp.tcb_head.next; next != taskp.tcb_tail; next = next.next)
        {
            if (next.proc_addr == -1)
                AppMain._amTaskDeleteReal(next);
        }
    }

    private static void _amTaskDeleteReal(AppMain.AMS_TCB tcb)
    {
        AppMain.AMS_TASK taskp = tcb.taskp;
        tcb.prev.next = tcb.next;
        tcb.next.prev = tcb.prev;
        AppMain.GlobalPool<AppMain.AMS_TCB>.Release(tcb);
    }

    private static void _amTaskCheckWork(AppMain.AMS_TASK taskp)
    {
        AppMain.mppAssertNotImpl();
    }

}