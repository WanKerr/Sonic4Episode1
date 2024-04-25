public partial class AppMain
{
    private static void AoActSysInit(
     uint spr_buf_num,
     uint act_buf_num,
     uint sort_buf_num,
     uint acm_stack_num)
    {
        if (g_ao_act_master_buf != null)
            return;
        if (spr_buf_num == 0U)
            spr_buf_num = 1U;
        if (act_buf_num == 0U)
            act_buf_num = 1U;
        if (sort_buf_num == 0U)
            sort_buf_num = 1U;
        if (acm_stack_num == 0U)
            acm_stack_num = 1U;
        g_ao_act_spr_buf_size = spr_buf_num;
        if (spr_buf_num > 0U)
        {
            g_ao_act_spr_buf = New<AOS_SPRITE>((int)spr_buf_num);
            g_ao_act_spr_ref = new AOS_SPRITE[(int)spr_buf_num];
        }
        else
        {
            g_ao_act_spr_buf = null;
            g_ao_act_spr_ref = null;
        }
        g_ao_act_buf_size = act_buf_num;
        if (act_buf_num > 0U)
        {
            g_ao_act_buf = New<AOS_ACTION>((int)act_buf_num);
            g_ao_act_ref = new AOS_ACTION[(int)act_buf_num];
        }
        else
        {
            g_ao_act_buf = null;
            g_ao_act_ref = null;
        }
        g_ao_act_sort_buf_size = sort_buf_num;
        g_ao_act_sort_buf = sort_buf_num <= 0U ? null : (ArrayPointer<AOS_ACT_SORT>)New<AOS_ACT_SORT>((int)sort_buf_num);
        g_ao_act_acm_buf_size = acm_stack_num;
        g_ao_act_acm_buf = acm_stack_num <= 0U ? null : (ArrayPointer<AOS_ACT_ACM>)New<AOS_ACT_ACM>((int)acm_stack_num);
        g_ao_act_acm_flag_buf_size = acm_stack_num;
        g_ao_act_acm_flag_buf = acm_stack_num <= 0U ? null : (ArrayPointer<uint>)new uint[(int)acm_stack_num];
        AoActSysReset();
    }

    private static void AoActSysReset()
    {
        g_ao_act_sys_draw_prio = 4096U;
        g_ao_act_sys_frame_rate = 1f;
        g_ao_act_sys_adjust_x = 0.0f;
        g_ao_act_sys_adjust_y = 0.0f;
        if (g_ao_act_spr_buf_size > 0U)
        {
            for (int index = 0; index < g_ao_act_spr_buf.Length; ++index)
                g_ao_act_spr_buf[index].Clear();
            for (uint index = 0; index < g_ao_act_spr_buf_size; ++index)
                g_ao_act_spr_ref[(int)index] = g_ao_act_spr_buf[(int)index];
        }
        g_ao_act_spr_alloc = 0U;
        g_ao_act_spr_free = 0U;
        g_ao_act_spr_num = 0U;
        if (g_ao_act_buf_size > 0U)
        {
            for (int index = 0; index < g_ao_act_buf.Length; ++index)
                g_ao_act_buf[index].Clear();
            for (uint index = 0; index < g_ao_act_buf_size; ++index)
                g_ao_act_ref[(int)index] = g_ao_act_buf[(int)index];
        }
        g_ao_act_alloc = 0U;
        g_ao_act_free = 0U;
        g_ao_act_num = 0U;
        if (g_ao_act_sort_buf_size > 0U)
        {
            ArrayPointer<AOS_ACT_SORT> gAoActSortBuf = g_ao_act_sort_buf;
            for (int index = 0; index < g_ao_act_sort_buf_size; ++index)
            {
                (~gAoActSortBuf).Clear();
                ++gAoActSortBuf;
            }
        }
        g_ao_act_sort_num = 0U;
        g_ao_act_acm_cur = g_ao_act_acm_buf;
        g_ao_act_acm_num = 1U;
        if (g_ao_act_acm_buf_size > 0U)
        {
            ArrayPointer<AOS_ACT_ACM> gAoActAcmBuf = g_ao_act_acm_buf;
            for (int index = 0; index < g_ao_act_acm_buf_size; ++index)
            {
                (~gAoActAcmBuf).Clear();
                ++gAoActAcmBuf;
            }
            AoActAcmInit();
        }
        g_ao_act_acm_flag_cur = g_ao_act_acm_flag_buf;
        g_ao_act_acm_flag_num = 1U;
        if (g_ao_act_acm_flag_buf_size > 0U)
        {
            ArrayPointer<uint> gAoActAcmFlagBuf = g_ao_act_acm_flag_buf;
            for (int index = 0; index < g_ao_act_acm_flag_buf_size; ++index)
            {
                int num = (int)gAoActAcmFlagBuf.SetPrimitive(0U);
                ++gAoActAcmFlagBuf;
            }
            AoActAcmSetFlag();
        }
        AoActSysClearPeak();
        g_ao_act_texlist = null;
    }

    private static void AoActSysExit()
    {
        if (g_ao_act_master_buf != null)
            g_ao_act_master_buf = null;
        g_ao_act_sys_frame_rate = 1f;
        g_ao_act_sys_adjust_x = 0.0f;
        g_ao_act_sys_adjust_y = 0.0f;
        g_ao_act_master_buf = null;
        g_ao_act_spr_buf = null;
        g_ao_act_spr_ref = null;
        g_ao_act_spr_alloc = 0U;
        g_ao_act_spr_free = 0U;
        g_ao_act_spr_buf_size = 0U;
        g_ao_act_spr_num = 0U;
        g_ao_act_spr_peak = 0U;
        g_ao_act_buf = null;
        g_ao_act_ref = null;
        g_ao_act_alloc = 0U;
        g_ao_act_free = 0U;
        g_ao_act_buf_size = 0U;
        g_ao_act_num = 0U;
        g_ao_act_peak = 0U;
        g_ao_act_sort_buf = null;
        g_ao_act_sort_buf_size = 0U;
        g_ao_act_sort_num = 0U;
        g_ao_act_sort_peak = 0U;
        g_ao_act_acm_buf = null;
        g_ao_act_acm_cur = null;
        g_ao_act_acm_buf_size = 0U;
        g_ao_act_acm_num = 0U;
        g_ao_act_acm_peak = 0U;
        g_ao_act_acm_flag_buf = null;
        g_ao_act_acm_flag_cur = null;
        g_ao_act_acm_flag_buf_size = 0U;
        g_ao_act_acm_flag_num = 0U;
        g_ao_act_acm_flag_peak = 0U;
        g_ao_act_texlist = null;
    }

    private static uint AoActSysGetSprBufferSize()
    {
        return g_ao_act_spr_buf_size;
    }

    private static uint AoActSysGetSprBufferRemain()
    {
        return g_ao_act_spr_buf_size - g_ao_act_spr_num;
    }

    private static uint AoActSysGetSprBufferPeak()
    {
        return g_ao_act_spr_peak;
    }

    private static uint AoActSysGetActBufferSize()
    {
        return g_ao_act_buf_size;
    }

    private static uint AoActSysGetActBufferRemain()
    {
        return g_ao_act_buf_size - g_ao_act_num;
    }

    private static uint AoActSysGetActBufferPeak()
    {
        return g_ao_act_peak;
    }

    private static uint AoActSysGetSortBufferSize()
    {
        return g_ao_act_sort_buf_size;
    }

    private static uint AoActSysGetSortBufferRemain()
    {
        return g_ao_act_sort_buf_size - g_ao_act_sort_num;
    }

    private static uint AoActSysGetSortBufferPeak()
    {
        return g_ao_act_sort_peak;
    }

    public static uint AoActSysGetAcmStackSize()
    {
        return g_ao_act_acm_buf_size;
    }

    public static uint AoActSysGetAcmStackRemain()
    {
        return g_ao_act_acm_buf_size - g_ao_act_acm_num;
    }

    public static uint AoActSysGetAcmBufferPeak()
    {
        return g_ao_act_acm_peak;
    }

    public static uint AoActSysGetAcmFlagStackSize()
    {
        return g_ao_act_acm_flag_buf_size;
    }

    public static uint AoActSysGetAcmFlagStackRemain()
    {
        return g_ao_act_acm_flag_buf_size - g_ao_act_acm_flag_num;
    }

    public static uint AoActSysGetAcmFlagBufferPeak()
    {
        return g_ao_act_acm_flag_peak;
    }

    public static void AoActSysClearPeak()
    {
        g_ao_act_spr_peak = 0U;
        g_ao_act_peak = 0U;
        g_ao_act_sort_peak = 0U;
        g_ao_act_acm_peak = 0U;
        g_ao_act_acm_flag_peak = 0U;
    }

    public static void AoActSysSetDrawTaskPrio()
    {
        AoActSysSetDrawTaskPrio(4096U);
    }

    public static void AoActSysSetDrawTaskPrio(uint prio)
    {
        g_ao_act_sys_draw_prio = prio;
    }

    public static uint AoActSysGetDrawTaskPrio()
    {
        return g_ao_act_sys_draw_prio;
    }

    public static void AoActSysSetDrawStateEnable(bool enable)
    {
        g_ao_act_sys_draw_state_enable = enable;
    }

    public static bool AoActSysGetDrawStateEnable()
    {
        return g_ao_act_sys_draw_state_enable;
    }

    public static void AoActSysSetDrawState(uint state)
    {
        g_ao_act_sys_draw_state = state;
    }

    public static uint AoActSysGetDrawState()
    {
        return g_ao_act_sys_draw_state;
    }

    public static void AoActSysSetFrameRate(float rate)
    {
        g_ao_act_sys_frame_rate = rate;
    }

    public static float AoActSysGetFrameRate()
    {
        return g_ao_act_sys_frame_rate;
    }

    public static void AoActSysSetAdjust(float x, float y)
    {
        g_ao_act_sys_adjust_x = x;
        g_ao_act_sys_adjust_y = y;
    }

    public static void AoActSysAddAdjust(float x, float y)
    {
        g_ao_act_sys_adjust_x += x;
        g_ao_act_sys_adjust_y += y;
    }

    public static float AoActSysGetAdjustX()
    {
        return g_ao_act_sys_adjust_x;
    }

    public static float AoActSysGetAdjustY()
    {
        return g_ao_act_sys_adjust_y;
    }

}