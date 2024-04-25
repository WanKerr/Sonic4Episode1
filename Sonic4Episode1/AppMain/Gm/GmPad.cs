public partial class AppMain
{
    private const int GME_PAD_VIB_TYPE_STOP = 0;
    private const int GME_PAD_VIB_TYPE_NORMAL = 1;
    private const int GME_PAD_VIB_TYPE_DEC = 2;
    private const int GME_PAD_VIB_TYPE_ACC = 3;
    private const int GME_PAD_VIB_TYPE_INT = 4;
    private const int GME_PAD_VIB_TYPE_MAX = 5;
    public const int GMD_PAD_VIB_DEF_INT_VIB_TIME = 122880;
    public const int GMD_PAD_VIB_DEF_INT_STOP_TIME = 122880;
    public const int GMD_PAD_VIB_LARGE_TYPE = 1;
    public const float GMD_PAD_VIB_LARGE_TIME = 60f;
    public const int GMD_PAD_VIB_LARGE_LEFT_VIB = 32768;
    public const int GMD_PAD_VIB_LARGE_RIGHT_VIB = 32768;
    public const float GMD_PAD_VIB_LARGE_ADD_DEC_TIME = 0.0f;
    public const float GMD_PAD_VIB_LARGE_INT_VIB_TIME = 0.0f;
    public const float GMD_PAD_VIB_LARGE_INT_STOP_TIME = 0.0f;
    public const int GMD_PAD_VIB_LARGE_PRIO = 32768;
    public const int GMD_PAD_VIB_MID_TYPE = 1;
    public const float GMD_PAD_VIB_MID_TIME = 30f;
    public const int GMD_PAD_VIB_MID_LEFT_VIB = 16384;
    public const int GMD_PAD_VIB_MID_RIGHT_VIB = 16384;
    public const float GMD_PAD_VIB_MID_ADD_DEC_TIME = 0.0f;
    public const float GMD_PAD_VIB_MID_INT_VIB_TIME = 0.0f;
    public const float GMD_PAD_VIB_MID_INT_STOP_TIME = 0.0f;
    public const int GMD_PAD_VIB_MID_PRIO = 16384;
    public const int GMD_PAD_VIB_SMALL_TYPE = 1;
    public const float GMD_PAD_VIB_SMALL_TIME = 30f;
    public const int GMD_PAD_VIB_SMALL_LEFT_VIB = 8192;
    public const int GMD_PAD_VIB_SMALL_RIGHT_VIB = 8192;
    public const float GMD_PAD_VIB_SMALL_ADD_DEC_TIME = 0.0f;
    public const float GMD_PAD_VIB_SMALL_INT_VIB_TIME = 0.0f;
    public const float GMD_PAD_VIB_SMALL_INT_STOP_TIME = 0.0f;
    public const int GMD_PAD_VIB_SMALL_PRIO = 8192;
    public const uint GMD_PAD_VIB_FLAG_INT_STOP = 1;
    public const uint GMD_PAD_VIB_FLAG_PAUSE = 2;

    public static void GMM_PAD_VIB_LARGE()
    {
        GmPadVibSet(1, 60f, 32768, 32768, 0.0f, 0.0f, 0.0f, 32768U);
    }

    public static void GMM_PAD_VIB_MID()
    {
        GmPadVibSet(1, 30f, 16384, 16384, 0.0f, 0.0f, 0.0f, 16384U);
    }

    public static void GMM_PAD_VIB_SMALL()
    {
        GmPadVibSet(1, 30f, 8192, 8192, 0.0f, 0.0f, 0.0f, 8192U);
    }

    public static void GMM_PAD_VIB_LARGE_TIME(float time)
    {
        GmPadVibSet(1, time, 32768, 32768, 0.0f, 0.0f, 0.0f, 32768U);
    }

    public static void GMM_PAD_VIB_MID_TIME(float time)
    {
        GmPadVibSet(1, time, 16384, 16384, 0.0f, 0.0f, 0.0f, 16384U);
    }

    public static void GMM_PAD_VIB_SMALL_TIME(float time)
    {
        GmPadVibSet(1, time, 8192, 8192, 0.0f, 0.0f, 0.0f, 8192U);
    }

    public static void GMM_PAD_VIB_LARGE_NOEND()
    {
        GmPadVibSet(1, -1f, 32768, 32768, 0.0f, 0.0f, 0.0f, 32768U);
    }

    public static void GMM_PAD_VIB_MID_NOEND()
    {
        GmPadVibSet(1, -1f, 16384, 16384, 0.0f, 0.0f, 0.0f, 16384U);
    }

    public static void GMM_PAD_VIB_SMALL_NOEND()
    {
        GmPadVibSet(1, -1f, 8192, 8192, 0.0f, 0.0f, 0.0f, 8192U);
    }

    public static void GMM_PAD_VIB_STOP()
    {
        GmPadVibSet(0, 0.0f, 0, 0, 0.0f, 0.0f, 0.0f);
    }


    private static void GmPadVibInit()
    {
        gm_pad_vib_tcb = MTM_TASK_MAKE_TCB(gmPadVibDMain, gmPadVibDest, 0x1080, 0x20, 2, 0,
            () => new GMS_PAD_VIB_WORK(), "GM_PAD_VIB");
        
        GMS_PAD_VIB_WORK work = (GMS_PAD_VIB_WORK) gm_pad_vib_tcb.work;
        work.flag = work.flag & 0xfffffffe;
        work.add_dec_time = 0.0f;
        work.int_vib_time = 0.0f;
        work.int_stop_time = 0.0f;
        work.vib_type = 0;
        work.time_count = 0.0f;
        work.left_vib = 0;
        work.right_vib = 0;
        work.prio = 0;
        work.time = -1.0f;

        AoPad.AoPadSetVibration(0, 0);
    }

    private static void GmPadVibExit()
    {
        amTaskDelete(gm_pad_vib_tcb.am_tcb);
        gm_pad_vib_tcb = null;
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
        GmPadVibSet(vib_type, time, left_vib, right_vib, add_dec_time, int_vib_time, int_stop_time, 0U);
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
        if (gm_pad_vib_tcb == null) return;

        GMS_PAD_VIB_WORK work = (GMS_PAD_VIB_WORK) gm_pad_vib_tcb.work;
        if (work.prio < (prio + 1))
        {
            work.flag = work.flag & 0xfffffffe;
            work.time = time;
            work.vib_type = vib_type;
            work.left_vib = left_vib;
            work.right_vib = right_vib;
            work.add_dec_time = add_dec_time;
            work.int_vib_time = int_vib_time;
            work.int_vib_time = int_stop_time;
            work.prio = prio;
            work.time_count = 0;
            
            AoPad.AoPadSetVibration(left_vib, right_vib);
        }
    }

    private static void gmPadVibDMain(MTS_TASK_TCB tcb)
    {
        GMS_PAD_VIB_WORK work = (GMS_PAD_VIB_WORK) gm_pad_vib_tcb.work;
        if (ObjObjectPauseCheck(0U) != 0U)
        {
            if (((int) work.flag & 2) != 0)
                return;
            work.flag |= 2U;
        }
        else
        {
            work.flag &= 4294967293U;
            if (work.time > 0.0)
            {
                work.time_count = ObjTimeCountUpF(work.time_count);
                if (work.time_count >= (double) work.time)
                {
                    work.vib_type = GME_PAD_VIB_TYPE_STOP;
                    work.prio = 0U;
                    work.time = -1f;
                }
            }

            ushort left = 0;
            ushort right = 0;

            switch (work.vib_type)
            {
                case GME_PAD_VIB_TYPE_NORMAL:
                    left = work.left_vib;
                    right = work.right_vib;
                    break;
                case GME_PAD_VIB_TYPE_DEC:
                    if (work.time - (double) work.time_count >= work.add_dec_time)
                        break;
                    float num1 = (work.time - work.time_count) / work.add_dec_time;
                    double num2 = nnRoundOff((float) (work.left_vib * (double) num1 + 0.5));
                    double num3 = nnRoundOff((float) (work.right_vib * (double) num1 + 0.5));
                    left = (ushort) (int) num2;
                    right = (ushort) (int) num3;
                    break;
                case GME_PAD_VIB_TYPE_ACC:
                    if (work.time_count >= (double) work.add_dec_time)
                        break;
                    float num4 = work.time_count / work.add_dec_time;
                    double num5 = nnRoundOff((float) (work.left_vib * (double) num4 + 0.5));
                    double num6 = nnRoundOff((float) (work.right_vib * (double) num4 + 0.5));
                    left = (ushort) (int) num5;
                    right = (ushort) (int) num6;
                    break;

                case GME_PAD_VIB_TYPE_INT:
                    work.int_count = ObjTimeCountUpF(work.int_count);
                    if (((int) work.flag & 1) != 0)
                    {
                        if (work.int_count < (double) work.int_stop_time)
                            break;
                        work.int_count = 0.0f;
                        work.flag &= 4294967294U;
                        break;
                    }

                    if (work.int_count < (double) work.int_stop_time)
                        break;
                    work.int_count = 0.0f;
                    work.flag |= 1U;

                    left = work.left_vib;
                    right = work.right_vib;
                    break;
            }

            AoPad.AoPadSetVibration(left, right);
        }
    }

    private static void gmPadVibDest(MTS_TASK_TCB tcb)
    {
        AoPad.AoPadSetVibration(0, 0);
        gm_pad_vib_tcb = null;
    }
}