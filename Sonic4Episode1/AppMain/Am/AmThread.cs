using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private Thread amThreadGetCurrentID()
    {
        return Thread.CurrentThread;
    }

    private static Thread amThreadCreate(
      ref AppMain.AMS_THREAD thread,
      ParameterizedThreadStart proc,
      object arg,
      AppMain.AMD_CORE core,
      int prio,
      uint stack_size,
      string name)
    {
        AppMain.mppAssertNotImpl();
        AppMain.amAssert((object)thread);
        AppMain.amAssert((object)proc);
        thread.thread_id = new Thread(new ParameterizedThreadStart(proc.Invoke));
        if (thread.thread_id != null)
        {
            AppMain.amAlarmCreate(thread.alarm_exit);
            AppMain.amMutexCreate(thread.mutex);
            thread.thread_id.Start(arg);
        }
        return thread.thread_id;
    }

    private static void amThreadCheckSafe(int i, string s)
    {
    }

    private static void amThreadOpen(AppMain.AMS_THREAD thread)
    {
        AppMain.mppAssertNotImpl();
    }

    private static int amThreadExit(AppMain.AMS_THREAD thread)
    {
        AppMain.amAssert((object)thread);
        AppMain.amAlarmSet(thread.alarm_exit);
        return 1;
    }

    private static int amThreadCheckExit(AppMain.AMS_THREAD thread)
    {
        AppMain.amAssert((object)thread);
        return AppMain.amAlarmCheck(thread.alarm_exit);
    }

    private static void amThreadQuit(AppMain.AMS_THREAD thread)
    {
        AppMain.amAssert((object)thread);
        thread.thread_id.Abort();
        thread.thread_id = (Thread)null;
    }

    private static int amThreadCheckQuit(AppMain.AMS_THREAD thread)
    {
        AppMain.amAssert((object)thread);
        return !Monitor.Wait((object)thread.thread_id, 0) ? 0 : 1;
    }

    private static void amThreadWaitQuit(AppMain.AMS_THREAD thread)
    {
        AppMain.amAssert((object)thread);
        Monitor.Wait((object)thread.thread_id);
    }

    private static void amThreadDelete(AppMain.AMS_THREAD thread)
    {
        AppMain.amAssert((object)thread);
        AppMain.amMutexDelete(thread.mutex);
        AppMain.amAlarmDelete(thread.alarm_exit);
    }

    public static bool amThreadCheckDraw()
    {
        return AppMain.amThreadCheckDraw(false);
    }

    public static bool amThreadCheckDraw(bool default_thread)
    {
        return true;
    }

    private static void amThreadSleep(int msec)
    {
        Thread.Sleep(msec);
    }
}