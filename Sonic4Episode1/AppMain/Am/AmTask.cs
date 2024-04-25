public partial class AppMain
{
    public static AMS_TCB amTaskMake(
      TaskProc proc,
      TaskProc dest,
      uint prio,
      uint user,
      uint attr,
      string name)
    {
        return amTaskMake(_am_default_taskp, proc, dest, prio, user, attr, name, 1U, 0, uint.MaxValue);
    }

    public static AMS_TCB amTaskMake(
      AMS_TASK taskp,
      TaskProc proc,
      TaskProc dest,
      uint prio,
      uint user,
      uint attr,
      string name)
    {
        return amTaskMake(taskp, proc, dest, prio, user, attr, name, 1U, 0, uint.MaxValue);
    }

    public static AMS_TCB amTaskMake(
      TaskProc proc,
      TaskProc dest,
      uint prio,
      uint user,
      uint attr,
      string name,
      uint stall,
      int group,
      uint run)
    {
        return amTaskMake(_am_default_taskp, proc, dest, prio, user, attr, name, stall, group, run);
    }

    private static void amTaskSetProcedure(AMS_TCB tcb, TaskProc proc)
    {
        tcb.procedure = proc;
        tcb.proc_addr = int.MaxValue;
    }

    private static void amTaskSetDestructor(AMS_TCB tcb, TaskProc dest)
    {
        tcb.destructor = dest;
    }

    public static void amTaskDeleteGroup(uint user, uint attr, uint flag)
    {
        amTaskDeleteGroup(_am_default_taskp, user, attr, flag);
    }

    public static void amTaskDeletePriority(uint prio_begin, uint prio_end, uint user)
    {
        amTaskDeletePriority(_am_default_taskp, prio_begin, prio_end, user);
    }

    public static void amTaskSleepGroup(uint user, uint attr, uint flag)
    {
        amTaskSleepGroup(_am_default_taskp, user, attr, flag);
    }

    public static void amTaskSleepPriority(uint prio_begin, uint prio_end, uint user)
    {
        amTaskSleepPriority(_am_default_taskp, prio_begin, prio_end, user);
    }

    public static void amTaskWakeupGroup(uint user, uint attr, uint flag)
    {
        amTaskWakeupGroup(_am_default_taskp, user, attr, flag);
    }

    public static void amTaskWakeupPriority(uint prio_begin, uint prio_end, uint user)
    {
        amTaskWakeupPriority(_am_default_taskp, prio_begin, prio_end, user);
    }

    public static object amTaskGetWork(AMS_TCB tcb)
    {
        return tcb.work;
    }

    private static AMS_TCB amTaskNextTcb(AMS_TCB tcbp)
    {
        return tcbp.next = GlobalPool<AMS_TCB>.Alloc();
    }

    private static AMS_TCB amPrevNextTcb(AMS_TCB tcbp)
    {
        return tcbp.prev = GlobalPool<AMS_TCB>.Alloc();
    }

    private static AMS_TCB_FOOTER amTaskGetTcbFooter(AMS_TCB tcbp)
    {
        return tcbp.footer;
    }

    private static AMS_TASK amTaskInitSystem()
    {
        return amTaskInitSystem(256, 64, 1);
    }

    private static AMS_TASK amTaskInitSystem(
      int max_tcb,
      int work_size,
      int thread_num)
    {
        AMS_TASK amsTask = new AMS_TASK();
        if (_am_default_taskp == null)
            _am_default_taskp = amsTask;
        amsTask.tcb_max = max_tcb;
        amsTask.tcb_work_size = work_size + 63 & -64;
        amsTask.tcb_head.name = "TCB Head";
        amsTask.tcb_head.priority = 0U;
        amsTask.tcb_head.prev = null;
        amsTask.tcb_head.next = amsTask.tcb_tail;
        amsTask.tcb_head.user_id = 0U;
        amsTask.tcb_head.attribute = 1U;
        amsTask.tcb_head.wkbegin = 218237452;
        amTaskSetProcedure(amsTask.tcb_head, null);
        amTaskSetDestructor(amsTask.tcb_head, null);
        amsTask.tcb_tail.name = "TCB Tail";
        amsTask.tcb_tail.priority = uint.MaxValue;
        amsTask.tcb_tail.prev = amsTask.tcb_head;
        amsTask.tcb_tail.next = null;
        amsTask.tcb_tail.user_id = 0U;
        amsTask.tcb_tail.attribute = 1U;
        amsTask.tcb_tail.wkbegin = 218237452;
        amTaskSetProcedure(amsTask.tcb_tail, null);
        amTaskSetDestructor(amsTask.tcb_tail, null);
        return amsTask;
    }

    private static void amTaskExitSystem()
    {
        amTaskExitSystem(_am_default_taskp);
    }

    private static void amTaskExitSystem(AMS_TASK taskp)
    {
        amMemFreeSystem(taskp);
    }

    private static void amTaskReset(AMS_TASK taskp)
    {
        taskp.tcb_head.next = taskp.tcb_tail;
        taskp.tcb_tail.prev = taskp.tcb_head;
    }

    private static void amTaskExecute()
    {
        amTaskExecute(_am_default_taskp);
    }

    private static void amTaskExecute(AMS_TASK taskp)
    {
        _amTaskExecuteInOrder(taskp);
    }

    private static AMS_TCB amTaskMake(
      AMS_TASK taskp,
      TaskProc proc,
      TaskProc dest,
      uint prio,
      uint user,
      uint attr,
      string name,
      uint stall,
      int group,
      uint run)
    {
        AMS_TCB amsTcb = GlobalPool<AMS_TCB>.Alloc();
        amsTcb.name = name;
        AMS_TCB_FOOTER tcbFooter = amTaskGetTcbFooter(amsTcb);
        tcbFooter.cpu_cnt = 0U;
        tcbFooter.cpu_cnt_max = 0U;
        amsTcb.priority = prio;
        amsTcb.priority = prio;
        amsTcb.user_id = user;
        amsTcb.attribute = attr;
        amTaskSetProcedure(amsTcb, proc);
        amTaskSetDestructor(amsTcb, dest);
        AMS_TCB next = taskp.tcb_head.next;
        while (next != taskp.tcb_tail && next.priority <= prio)
            next = next.next;
        next.prev.next = amsTcb;
        amsTcb.prev = next.prev;
        next.prev = amsTcb;
        amsTcb.next = next;

        //System.Diagnostics.Debug.WriteLine($"Task {name}, prio {prio}, next {next.name}, prev {amsTcb.prev.name}");

        return amsTcb;
    }

    private int amTaskPending(AMS_TCB tcbp)
    {
        return 0;
    }

    public static int amTaskStart(AMS_TCB tcbp)
    {
        return 0;
    }

    public static void amTaskDelete(AMS_TCB tcb)
    {
        if (tcb.proc_addr == -1)
            return;
        if (tcb.destructor != null)
            tcb.destructor(tcb);
        tcb.proc_addr = -1;
    }

    public static void amTaskDeleteGroup(AMS_TASK taskp, uint user, uint attr, uint flag)
    {
        if (user == 0U)
            user = uint.MaxValue;
        AMS_TCB next = taskp.tcb_head.next;
        switch (flag)
        {
            case 0:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != 0)
                        amTaskDelete(next);
                }
                break;
            case 1:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == (int)attr)
                        amTaskDelete(next);
                }
                break;
            case 2:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == 0)
                        amTaskDelete(next);
                }
                break;
            case 3:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != (int)attr)
                        amTaskDelete(next);
                }
                break;
        }
    }

    public static void amTaskDeletePriority(
      AMS_TASK taskp,
      uint prio_begin,
      uint prio_end,
      uint user)
    {
        AMS_TCB next = taskp.tcb_head.next;
        while (next != taskp.tcb_tail && next.priority < prio_begin)
            next = next.next;
        for (; next != taskp.tcb_tail && next.priority <= prio_end; next = next.next)
        {
            if (user == 0U || ((int)next.user_id & (int)user) != 0)
                amTaskDelete(next);
        }
    }

    public static void amTaskSleep(AMS_TCB tcb)
    {
        tcb.proc_addr |= 1;
    }

    public static void amTaskSleepGroup(AMS_TASK taskp, uint user, uint attr, uint flag)
    {
        mppAssertNotImpl();
        if (user == 0U)
            user = uint.MaxValue;
        AMS_TCB next = taskp.tcb_head.next;
        switch (flag)
        {
            case 0:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != 0)
                        amTaskSleep(next);
                }
                break;
            case 1:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == (int)attr)
                        amTaskSleep(next);
                }
                break;
            case 2:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == 0)
                        amTaskSleep(next);
                }
                break;
            case 3:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != (int)attr)
                        amTaskSleep(next);
                }
                break;
        }
    }

    public static void amTaskSleepPriority(
      AMS_TASK taskp,
      uint prio_begin,
      uint prio_end,
      uint user)
    {
        mppAssertNotImpl();
        AMS_TCB next = taskp.tcb_head.next;
        while (next != taskp.tcb_tail && next.priority < prio_begin)
            next = next.next;
        for (; next != taskp.tcb_tail && next.priority <= prio_end; next = next.next)
        {
            if (user == 0U || ((int)next.user_id & (int)user) != 0)
                amTaskSleep(next);
        }
    }

    public static void amTaskWakeup(AMS_TCB tcb)
    {
        mppAssertNotImpl();
        tcb.proc_addr &= -2;
    }

    public static void amTaskWakeupGroup(AMS_TASK taskp, uint user, uint attr, uint flag)
    {
        mppAssertNotImpl();
        if (user == 0U)
            user = uint.MaxValue;
        AMS_TCB next = taskp.tcb_head.next;
        switch (flag)
        {
            case 0:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != 0)
                        amTaskWakeup(next);
                }
                break;
            case 1:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == (int)attr)
                        amTaskWakeup(next);
                }
                break;
            case 2:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) == 0)
                        amTaskWakeup(next);
                }
                break;
            case 3:
                for (; next != taskp.tcb_tail; next = next.next)
                {
                    if (((int)next.user_id & (int)user) != 0 && ((int)next.attribute & (int)attr) != (int)attr)
                        amTaskWakeup(next);
                }
                break;
        }
    }

    public static void amTaskWakeupPriority(
      AMS_TASK taskp,
      uint prio_begin,
      uint prio_end,
      uint user)
    {
        mppAssertNotImpl();
        AMS_TCB next = taskp.tcb_head.next;
        while (next != taskp.tcb_tail && next.priority < prio_begin)
            next = next.next;
        for (; next != taskp.tcb_tail && next.priority <= prio_end; next = next.next)
        {
            if (user == 0U || ((int)next.user_id & (int)user) != 0)
                amTaskWakeup(next);
        }
    }

    private static void amTaskSetOwnerName(AMS_TASKLIST_OWNER[] pList, uint listSize)
    {
        mppAssertNotImpl();
        _am_owner_list = pList;
        _am_szOwnerList = _am_owner_list != null ? (int)listSize : 0;
    }

    private static void amTaskDisplayList(AMS_TASK taskp, int locx, int locy)
    {
        mppAssertNotImpl();
    }

    private static void amTaskDisplayThread(AMS_TASK taskp)
    {
        mppAssertNotImpl();
    }

    private static void _amTaskExecuteInOrder(AMS_TASK taskp)
    {
        for (AMS_TCB next = taskp.tcb_head.next; next != taskp.tcb_tail; next = next.next)
        {
            if (next.procedure != null && next.proc_addr > 0)
                next.procedure(next);
        }
        for (AMS_TCB next = taskp.tcb_head.next; next != taskp.tcb_tail; next = next.next)
        {
            if (next.proc_addr == -1)
                _amTaskDeleteReal(next);
        }
    }

    private static void _amTaskDeleteReal(AMS_TCB tcb)
    {
        AMS_TASK taskp = tcb.taskp;
        tcb.prev.next = tcb.next;
        tcb.next.prev = tcb.prev;
        GlobalPool<AMS_TCB>.Release(tcb);
    }

    private static void _amTaskCheckWork(AMS_TASK taskp)
    {
        mppAssertNotImpl();
    }

}