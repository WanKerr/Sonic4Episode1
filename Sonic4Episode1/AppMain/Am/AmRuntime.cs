public partial class AppMain
{
    public static AMS_AME_RUNTIME _amAllocRuntime()
    {
        AMS_AME_RUNTIME amsAmeRuntime = _am_runtime_ref[_am_runtime_alloc];
        ++_am_runtime_alloc;
        if (_am_runtime_alloc >= 512)
            _am_runtime_alloc = 0;
        amsAmeRuntime.Clear();
        return amsAmeRuntime;
    }

    public static void _amFreeRuntime(AMS_AME_RUNTIME runtime)
    {
        if (runtime.work != null)
            amEffectFreeRuntimeWork(runtime.work);
        AMS_AME_RUNTIME_WORK next1 = (AMS_AME_RUNTIME_WORK)runtime.work_head.next;
        for (AMS_AME_RUNTIME_WORK workTail = (AMS_AME_RUNTIME_WORK)runtime.work_tail; next1 != workTail; next1 = (AMS_AME_RUNTIME_WORK)next1.next)
            amEffectFreeRuntimeWork(next1);
        AMS_AME_RUNTIME_WORK next2 = (AMS_AME_RUNTIME_WORK)runtime.active_head.next;
        for (AMS_AME_RUNTIME_WORK activeTail = (AMS_AME_RUNTIME_WORK)runtime.active_tail; next2 != activeTail; next2 = (AMS_AME_RUNTIME_WORK)next2.next)
            amEffectFreeRuntimeWork(next2);
        _am_runtime_ref[_am_runtime_free] = runtime;
        ++_am_runtime_free;
        if (_am_runtime_free < 512)
            return;
        _am_runtime_free = 0;
    }

    public static AMS_AME_RUNTIME_WORK _amAllocRuntimeWork()
    {
        AMS_AME_RUNTIME_WORK amsAmeRuntimeWork = _am_work_ref[_am_work_alloc];
        ++_am_work_alloc;
        if (_am_work_alloc >= 1024)
            _am_work_alloc = 0;
        amsAmeRuntimeWork.Clear();
        return amsAmeRuntimeWork;
    }

    public static void _amAddEntry(AMS_AME_ECB ecb, AMS_AME_RUNTIME runtime)
    {
        AMS_AME_ENTRY amsAmeEntry = _am_entry_ref[_am_entry_alloc];
        ++_am_entry_alloc;
        if (_am_entry_alloc >= 512)
            _am_entry_alloc = 0;
        amsAmeEntry.runtime = runtime;
        if (ecb.entry_head == null)
        {
            ecb.entry_head = amsAmeEntry;
            amsAmeEntry.prev = null;
        }
        if (ecb.entry_tail != null)
        {
            amsAmeEntry.prev = ecb.entry_tail;
            ecb.entry_tail.next = amsAmeEntry;
        }
        ecb.entry_tail = amsAmeEntry;
        amsAmeEntry.next = null;
        ++ecb.entry_num;
    }

    public static void _amDelEntry(AMS_AME_ECB ecb, AMS_AME_ENTRY entry)
    {
        if (entry.prev == null)
            ecb.entry_head = (AMS_AME_ENTRY)entry.next;
        else
            entry.prev.next = entry.next;
        if (entry.next == null)
            ecb.entry_tail = (AMS_AME_ENTRY)entry.prev;
        else
            entry.next.prev = entry.prev;
        _am_entry_ref[_am_entry_free] = entry;
        ++_am_entry_free;
        if (_am_entry_free >= 512)
            _am_entry_free = 0;
        --ecb.entry_num;
    }
}