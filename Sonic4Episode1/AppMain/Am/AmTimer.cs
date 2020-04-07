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
    private float amTimerGetFrame(ref AppMain.AMS_TIMER timer)
    {
        AppMain.mppAssertNotImpl();
        return timer.frame;
    }

    private float amTimerGetMSec(ref AppMain.AMS_TIMER timer)
    {
        AppMain.mppAssertNotImpl();
        return timer.msec;
    }

    private float amTimerGetuSec(ref AppMain.AMS_TIMER timer)
    {
        AppMain.mppAssertNotImpl();
        return timer.usec;
    }

    private AppMain.AMS_TIMER amTimerCreate(ref AppMain.AMS_TIMER timer)
    {
        AppMain.mppAssertNotImpl();
        if (timer == null)
        {
            timer = new AppMain.AMS_TIMER();
            timer.delete_flag = 1;
        }
        else
            timer = (AppMain.AMS_TIMER)null;
        timer.count_freq = 1000000f;
        return timer;
    }

    private void amTimerDelete(ref AppMain.AMS_TIMER timer)
    {
        AppMain.mppAssertNotImpl();
        if (timer.delete_flag == 1)
            timer = (AppMain.AMS_TIMER)null;
        else
            timer = (AppMain.AMS_TIMER)null;
    }

    private void amTimerStart(ref AppMain.AMS_TIMER timer)
    {
        AppMain.mppAssertNotImpl();
    }

    private void amTimerEnd(ref AppMain.AMS_TIMER timer, int reset)
    {
        AppMain.mppAssertNotImpl();
    }

    private float amTimerCalcFrame(ulong count_start, ulong count_end, ref AppMain.AMS_TIMER timer)
    {
        AppMain.mppAssertNotImpl();
        return (float)(count_end - count_start) * 60f / timer.count_freq;
    }

    private AppMain.AMS_ALARM amAlarmCreateTimer(AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
        if (alarm == null)
            alarm = new AppMain.AMS_ALARM();
        return alarm;
    }

    private static AppMain.AMS_ALARM amAlarmCreate(AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
        return alarm;
    }

    private static void amAlarmDelete(AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
        if (alarm.delete_flag == 1)
            alarm = (AppMain.AMS_ALARM)null;
        else
            alarm = (AppMain.AMS_ALARM)null;
    }

    private void amAlarmSetTimerVSync(ref AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
    }

    private void amAlarmSetTimer(AppMain.AMS_ALARM alarm, uint interval)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void amAlarmSet(AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
    }

    private void amAlarmClear(ref AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
    }

    private void amAlarmWaitTimer(ref AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
    }

    private void amAlarmWaitVSync(ref AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
    }

    private void amAlarmWait(ref AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
    }

    private void amAlarmUpdateTimer(ref AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
    }

    private int amAlarmCheckTimer(ref AppMain.AMS_ALARM alarm)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private static int amAlarmCheck(AppMain.AMS_ALARM alarm)
    {
        int num = 1;
        AppMain.mppAssertNotImpl();
        return num;
    }

    private void amAlarmHandler(int signal_id)
    {
        AppMain.mppAssertNotImpl();
    }

}