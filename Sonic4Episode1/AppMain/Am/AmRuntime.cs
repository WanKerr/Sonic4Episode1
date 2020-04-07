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
    public static AppMain.AMS_AME_RUNTIME _amAllocRuntime()
    {
        AppMain.AMS_AME_RUNTIME amsAmeRuntime = AppMain._am_runtime_ref[AppMain._am_runtime_alloc];
        ++AppMain._am_runtime_alloc;
        if (AppMain._am_runtime_alloc >= 512)
            AppMain._am_runtime_alloc = 0;
        amsAmeRuntime.Clear();
        return amsAmeRuntime;
    }

    public static void _amFreeRuntime(AppMain.AMS_AME_RUNTIME runtime)
    {
        if (runtime.work != null)
            AppMain.amEffectFreeRuntimeWork(runtime.work);
        AppMain.AMS_AME_RUNTIME_WORK next1 = (AppMain.AMS_AME_RUNTIME_WORK)runtime.work_head.next;
        for (AppMain.AMS_AME_RUNTIME_WORK workTail = (AppMain.AMS_AME_RUNTIME_WORK)runtime.work_tail; next1 != workTail; next1 = (AppMain.AMS_AME_RUNTIME_WORK)next1.next)
            AppMain.amEffectFreeRuntimeWork(next1);
        AppMain.AMS_AME_RUNTIME_WORK next2 = (AppMain.AMS_AME_RUNTIME_WORK)runtime.active_head.next;
        for (AppMain.AMS_AME_RUNTIME_WORK activeTail = (AppMain.AMS_AME_RUNTIME_WORK)runtime.active_tail; next2 != activeTail; next2 = (AppMain.AMS_AME_RUNTIME_WORK)next2.next)
            AppMain.amEffectFreeRuntimeWork(next2);
        AppMain._am_runtime_ref[AppMain._am_runtime_free] = runtime;
        ++AppMain._am_runtime_free;
        if (AppMain._am_runtime_free < 512)
            return;
        AppMain._am_runtime_free = 0;
    }

    public static AppMain.AMS_AME_RUNTIME_WORK _amAllocRuntimeWork()
    {
        AppMain.AMS_AME_RUNTIME_WORK amsAmeRuntimeWork = AppMain._am_work_ref[AppMain._am_work_alloc];
        ++AppMain._am_work_alloc;
        if (AppMain._am_work_alloc >= 1024)
            AppMain._am_work_alloc = 0;
        amsAmeRuntimeWork.Clear();
        return amsAmeRuntimeWork;
    }

    public static void _amAddEntry(AppMain.AMS_AME_ECB ecb, AppMain.AMS_AME_RUNTIME runtime)
    {
        AppMain.AMS_AME_ENTRY amsAmeEntry = AppMain._am_entry_ref[AppMain._am_entry_alloc];
        ++AppMain._am_entry_alloc;
        if (AppMain._am_entry_alloc >= 512)
            AppMain._am_entry_alloc = 0;
        amsAmeEntry.runtime = runtime;
        if (ecb.entry_head == null)
        {
            ecb.entry_head = amsAmeEntry;
            amsAmeEntry.prev = (AppMain.AMS_AME_LIST)null;
        }
        if (ecb.entry_tail != null)
        {
            amsAmeEntry.prev = (AppMain.AMS_AME_LIST)ecb.entry_tail;
            ecb.entry_tail.next = (AppMain.AMS_AME_LIST)amsAmeEntry;
        }
        ecb.entry_tail = amsAmeEntry;
        amsAmeEntry.next = (AppMain.AMS_AME_LIST)null;
        ++ecb.entry_num;
    }

    public static void _amDelEntry(AppMain.AMS_AME_ECB ecb, AppMain.AMS_AME_ENTRY entry)
    {
        if (entry.prev == null)
            ecb.entry_head = (AppMain.AMS_AME_ENTRY)entry.next;
        else
            entry.prev.next = entry.next;
        if (entry.next == null)
            ecb.entry_tail = (AppMain.AMS_AME_ENTRY)entry.prev;
        else
            entry.next.prev = entry.prev;
        AppMain._am_entry_ref[AppMain._am_entry_free] = entry;
        ++AppMain._am_entry_free;
        if (AppMain._am_entry_free >= 512)
            AppMain._am_entry_free = 0;
        --ecb.entry_num;
    }
}