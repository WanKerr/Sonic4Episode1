using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    public static void GMM_PAD_VIB_LARGE()
    {
        AppMain.GmPadVibSet(1, 60f, (ushort)32768, (ushort)32768, 0.0f, 0.0f, 0.0f, 32768U);
    }

    public static void GMM_PAD_VIB_MID()
    {
        AppMain.GmPadVibSet(1, 30f, (ushort)16384, (ushort)16384, 0.0f, 0.0f, 0.0f, 16384U);
    }

    public static void GMM_PAD_VIB_SMALL()
    {
        AppMain.GmPadVibSet(1, 30f, (ushort)8192, (ushort)8192, 0.0f, 0.0f, 0.0f, 8192U);
    }

    public static void GMM_PAD_VIB_LARGE_TIME(float time)
    {
        AppMain.GmPadVibSet(1, time, (ushort)32768, (ushort)32768, 0.0f, 0.0f, 0.0f, 32768U);
    }

    public static void GMM_PAD_VIB_MID_TIME(float time)
    {
        AppMain.GmPadVibSet(1, time, (ushort)16384, (ushort)16384, 0.0f, 0.0f, 0.0f, 16384U);
    }

    public static void GMM_PAD_VIB_SMALL_TIME(float time)
    {
        AppMain.GmPadVibSet(1, time, (ushort)8192, (ushort)8192, 0.0f, 0.0f, 0.0f, 8192U);
    }

    public static void GMM_PAD_VIB_LARGE_NOEND()
    {
        AppMain.GmPadVibSet(1, -1f, (ushort)32768, (ushort)32768, 0.0f, 0.0f, 0.0f, 32768U);
    }

    public static void GMM_PAD_VIB_MID_NOEND()
    {
        AppMain.GmPadVibSet(1, -1f, (ushort)16384, (ushort)16384, 0.0f, 0.0f, 0.0f, 16384U);
    }

    public static void GMM_PAD_VIB_SMALL_NOEND()
    {
        AppMain.GmPadVibSet(1, -1f, (ushort)8192, (ushort)8192, 0.0f, 0.0f, 0.0f, 8192U);
    }

    public static void GMM_PAD_VIB_STOP()
    {
        AppMain.GmPadVibSet(0, 0.0f, (ushort)0, (ushort)0, 0.0f, 0.0f, 0.0f);
    }

    private static void GmPadVibInit()
    {
    }

    private static void GmPadVibExit()
    {
    }

    private static void GmPadVibSet(
      int vib_type,
      float time,
      ushort left_vib,
      ushort right_vib,
      float add_dec_time,
      float int_vib_time,
      float int_stop_time)
    {
        AppMain.GmPadVibSet(vib_type, time, left_vib, right_vib, add_dec_time, int_vib_time, int_stop_time, 0U);
    }

    private static void GmPadVibSet(
      int vib_type,
      float time,
      ushort left_vib,
      ushort right_vib,
      float add_dec_time,
      float int_vib_time,
      float int_stop_time,
      uint prio)
    {
    }

    private void gmPadVibDMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_PAD_VIB_WORK work = (AppMain.GMS_PAD_VIB_WORK)tcb.work;
        if (AppMain.ObjObjectPauseCheck(0U) != 0U)
        {
            if (((int)work.flag & 2) != 0)
                return;
            work.flag |= 2U;
        }
        else
        {
            work.flag &= 4294967293U;
            if ((double)work.time > 0.0)
            {
                work.time_count = AppMain.ObjTimeCountUpF(work.time_count);
                if ((double)work.time_count >= (double)work.time)
                {
                    work.vib_type = 0;
                    work.prio = 0U;
                    work.time = -1f;
                }
            }
            switch (work.vib_type)
            {
                case 2:
                    if ((double)work.time - (double)work.time_count >= (double)work.add_dec_time)
                        break;
                    float num1 = (work.time - work.time_count) / work.add_dec_time;
                    double num2 = (double)AppMain.nnRoundOff((float)((double)work.left_vib * (double)num1 + 0.5));
                    double num3 = (double)AppMain.nnRoundOff((float)((double)work.right_vib * (double)num1 + 0.5));
                    break;
                case 3:
                    if ((double)work.time_count >= (double)work.add_dec_time)
                        break;
                    float num4 = work.time_count / work.add_dec_time;
                    double num5 = (double)AppMain.nnRoundOff((float)((double)work.left_vib * (double)num4 + 0.5));
                    double num6 = (double)AppMain.nnRoundOff((float)((double)work.right_vib * (double)num4 + 0.5));
                    break;
                case 4:
                    work.int_count = AppMain.ObjTimeCountUpF(work.int_count);
                    if (((int)work.flag & 1) != 0)
                    {
                        if ((double)work.int_count < (double)work.int_stop_time)
                            break;
                        work.int_count = 0.0f;
                        work.flag &= 4294967294U;
                        break;
                    }
                    if ((double)work.int_count < (double)work.int_stop_time)
                        break;
                    work.int_count = 0.0f;
                    work.flag |= 1U;
                    break;
            }
        }
    }

    private void gmPadVibDest(AppMain.MTS_TASK_TCB tcb)
    {
        AoPad.AoPadSetVibration((ushort)0, (ushort)0);
        AppMain.gm_pad_vib_tcb = (AppMain.MTS_TASK_TCB)null;
    }
}