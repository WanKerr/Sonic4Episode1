public partial class AppMain
{
    private float amTimerGetFrame(ref AMS_TIMER timer)
    {
        mppAssertNotImpl();
        return timer.frame;
    }

    private float amTimerGetMSec(ref AMS_TIMER timer)
    {
        mppAssertNotImpl();
        return timer.msec;
    }

    private float amTimerGetuSec(ref AMS_TIMER timer)
    {
        mppAssertNotImpl();
        return timer.usec;
    }

    private AMS_TIMER amTimerCreate(ref AMS_TIMER timer)
    {
        mppAssertNotImpl();
        if (timer == null)
        {
            timer = new AMS_TIMER();
            timer.delete_flag = 1;
        }
        else
            timer = null;
        timer.count_freq = 1000000f;
        return timer;
    }

    private void amTimerDelete(ref AMS_TIMER timer)
    {
        mppAssertNotImpl();
        if (timer.delete_flag == 1)
            timer = null;
        else
            timer = null;
    }

    private void amTimerStart(ref AMS_TIMER timer)
    {
        mppAssertNotImpl();
    }

    private void amTimerEnd(ref AMS_TIMER timer, int reset)
    {
        mppAssertNotImpl();
    }

    private float amTimerCalcFrame(ulong count_start, ulong count_end, ref AMS_TIMER timer)
    {
        mppAssertNotImpl();
        return (count_end - count_start) * 60f / timer.count_freq;
    }

    private AMS_ALARM amAlarmCreateTimer(AMS_ALARM alarm)
    {
        mppAssertNotImpl();
        if (alarm == null)
            alarm = new AMS_ALARM();
        return alarm;
    }

    private static AMS_ALARM amAlarmCreate(AMS_ALARM alarm)
    {
        mppAssertNotImpl();
        return alarm;
    }

    private static void amAlarmDelete(AMS_ALARM alarm)
    {
        mppAssertNotImpl();
        if (alarm.delete_flag == 1)
            alarm = null;
        else
            alarm = null;
    }

    private void amAlarmSetTimerVSync(ref AMS_ALARM alarm)
    {
        mppAssertNotImpl();
    }

    private void amAlarmSetTimer(AMS_ALARM alarm, uint interval)
    {
        mppAssertNotImpl();
    }

    private static void amAlarmSet(AMS_ALARM alarm)
    {
        mppAssertNotImpl();
    }

    private void amAlarmClear(ref AMS_ALARM alarm)
    {
        mppAssertNotImpl();
    }

    private void amAlarmWaitTimer(ref AMS_ALARM alarm)
    {
        mppAssertNotImpl();
    }

    private void amAlarmWaitVSync(ref AMS_ALARM alarm)
    {
        mppAssertNotImpl();
    }

    private void amAlarmWait(ref AMS_ALARM alarm)
    {
        mppAssertNotImpl();
    }

    private void amAlarmUpdateTimer(ref AMS_ALARM alarm)
    {
        mppAssertNotImpl();
    }

    private int amAlarmCheckTimer(ref AMS_ALARM alarm)
    {
        mppAssertNotImpl();
        return 0;
    }

    private static int amAlarmCheck(AMS_ALARM alarm)
    {
        int num = 1;
        mppAssertNotImpl();
        return num;
    }

    private void amAlarmHandler(int signal_id)
    {
        mppAssertNotImpl();
    }

}