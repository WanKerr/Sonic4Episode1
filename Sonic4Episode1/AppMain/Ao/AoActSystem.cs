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
    private static void AoActSysInit(
     uint spr_buf_num,
     uint act_buf_num,
     uint sort_buf_num,
     uint acm_stack_num)
    {
        if (AppMain.g_ao_act_master_buf != null)
            return;
        if (spr_buf_num == 0U)
            spr_buf_num = 1U;
        if (act_buf_num == 0U)
            act_buf_num = 1U;
        if (sort_buf_num == 0U)
            sort_buf_num = 1U;
        if (acm_stack_num == 0U)
            acm_stack_num = 1U;
        AppMain.g_ao_act_spr_buf_size = spr_buf_num;
        if (spr_buf_num > 0U)
        {
            AppMain.g_ao_act_spr_buf = AppMain.New<AppMain.AOS_SPRITE>((int)spr_buf_num);
            AppMain.g_ao_act_spr_ref = new AppMain.AOS_SPRITE[(int)spr_buf_num];
        }
        else
        {
            AppMain.g_ao_act_spr_buf = (AppMain.AOS_SPRITE[])null;
            AppMain.g_ao_act_spr_ref = (AppMain.AOS_SPRITE[])null;
        }
        AppMain.g_ao_act_buf_size = act_buf_num;
        if (act_buf_num > 0U)
        {
            AppMain.g_ao_act_buf = AppMain.New<AppMain.AOS_ACTION>((int)act_buf_num);
            AppMain.g_ao_act_ref = new AppMain.AOS_ACTION[(int)act_buf_num];
        }
        else
        {
            AppMain.g_ao_act_buf = (AppMain.AOS_ACTION[])null;
            AppMain.g_ao_act_ref = (AppMain.AOS_ACTION[])null;
        }
        AppMain.g_ao_act_sort_buf_size = sort_buf_num;
        AppMain.g_ao_act_sort_buf = sort_buf_num <= 0U ? (AppMain.ArrayPointer<AppMain.AOS_ACT_SORT>)(AppMain.AOS_ACT_SORT[])null : (AppMain.ArrayPointer<AppMain.AOS_ACT_SORT>)AppMain.New<AppMain.AOS_ACT_SORT>((int)sort_buf_num);
        AppMain.g_ao_act_acm_buf_size = acm_stack_num;
        AppMain.g_ao_act_acm_buf = acm_stack_num <= 0U ? (AppMain.ArrayPointer<AppMain.AOS_ACT_ACM>)(AppMain.AOS_ACT_ACM[])null : (AppMain.ArrayPointer<AppMain.AOS_ACT_ACM>)AppMain.New<AppMain.AOS_ACT_ACM>((int)acm_stack_num);
        AppMain.g_ao_act_acm_flag_buf_size = acm_stack_num;
        AppMain.g_ao_act_acm_flag_buf = acm_stack_num <= 0U ? (AppMain.ArrayPointer<uint>)(uint[])null : (AppMain.ArrayPointer<uint>)new uint[(int)acm_stack_num];
        AppMain.AoActSysReset();
    }

    private static void AoActSysReset()
    {
        AppMain.g_ao_act_sys_draw_prio = 4096U;
        AppMain.g_ao_act_sys_frame_rate = 1f;
        AppMain.g_ao_act_sys_adjust_x = 0.0f;
        AppMain.g_ao_act_sys_adjust_y = 0.0f;
        if (AppMain.g_ao_act_spr_buf_size > 0U)
        {
            for (int index = 0; index < AppMain.g_ao_act_spr_buf.Length; ++index)
                AppMain.g_ao_act_spr_buf[index].Clear();
            for (uint index = 0; index < AppMain.g_ao_act_spr_buf_size; ++index)
                AppMain.g_ao_act_spr_ref[(int)index] = AppMain.g_ao_act_spr_buf[(int)index];
        }
        AppMain.g_ao_act_spr_alloc = 0U;
        AppMain.g_ao_act_spr_free = 0U;
        AppMain.g_ao_act_spr_num = 0U;
        if (AppMain.g_ao_act_buf_size > 0U)
        {
            for (int index = 0; index < AppMain.g_ao_act_buf.Length; ++index)
                AppMain.g_ao_act_buf[index].Clear();
            for (uint index = 0; index < AppMain.g_ao_act_buf_size; ++index)
                AppMain.g_ao_act_ref[(int)index] = AppMain.g_ao_act_buf[(int)index];
        }
        AppMain.g_ao_act_alloc = 0U;
        AppMain.g_ao_act_free = 0U;
        AppMain.g_ao_act_num = 0U;
        if (AppMain.g_ao_act_sort_buf_size > 0U)
        {
            AppMain.ArrayPointer<AppMain.AOS_ACT_SORT> gAoActSortBuf = AppMain.g_ao_act_sort_buf;
            for (int index = 0; (long)index < (long)AppMain.g_ao_act_sort_buf_size; ++index)
            {
                ((AppMain.AOS_ACT_SORT)~gAoActSortBuf).Clear();
                ++gAoActSortBuf;
            }
        }
        AppMain.g_ao_act_sort_num = 0U;
        AppMain.g_ao_act_acm_cur = AppMain.g_ao_act_acm_buf;
        AppMain.g_ao_act_acm_num = 1U;
        if (AppMain.g_ao_act_acm_buf_size > 0U)
        {
            AppMain.ArrayPointer<AppMain.AOS_ACT_ACM> gAoActAcmBuf = AppMain.g_ao_act_acm_buf;
            for (int index = 0; (long)index < (long)AppMain.g_ao_act_acm_buf_size; ++index)
            {
                ((AppMain.AOS_ACT_ACM)~gAoActAcmBuf).Clear();
                ++gAoActAcmBuf;
            }
            AppMain.AoActAcmInit();
        }
        AppMain.g_ao_act_acm_flag_cur = AppMain.g_ao_act_acm_flag_buf;
        AppMain.g_ao_act_acm_flag_num = 1U;
        if (AppMain.g_ao_act_acm_flag_buf_size > 0U)
        {
            AppMain.ArrayPointer<uint> gAoActAcmFlagBuf = AppMain.g_ao_act_acm_flag_buf;
            for (int index = 0; (long)index < (long)AppMain.g_ao_act_acm_flag_buf_size; ++index)
            {
                int num = (int)gAoActAcmFlagBuf.SetPrimitive(0U);
                ++gAoActAcmFlagBuf;
            }
            AppMain.AoActAcmSetFlag();
        }
        AppMain.AoActSysClearPeak();
        AppMain.g_ao_act_texlist = (AppMain.NNS_TEXLIST)null;
    }

    private static void AoActSysExit()
    {
        if (AppMain.g_ao_act_master_buf != null)
            AppMain.g_ao_act_master_buf = (byte[])null;
        AppMain.g_ao_act_sys_frame_rate = 1f;
        AppMain.g_ao_act_sys_adjust_x = 0.0f;
        AppMain.g_ao_act_sys_adjust_y = 0.0f;
        AppMain.g_ao_act_master_buf = (byte[])null;
        AppMain.g_ao_act_spr_buf = (AppMain.AOS_SPRITE[])null;
        AppMain.g_ao_act_spr_ref = (AppMain.AOS_SPRITE[])null;
        AppMain.g_ao_act_spr_alloc = 0U;
        AppMain.g_ao_act_spr_free = 0U;
        AppMain.g_ao_act_spr_buf_size = 0U;
        AppMain.g_ao_act_spr_num = 0U;
        AppMain.g_ao_act_spr_peak = 0U;
        AppMain.g_ao_act_buf = (AppMain.AOS_ACTION[])null;
        AppMain.g_ao_act_ref = (AppMain.AOS_ACTION[])null;
        AppMain.g_ao_act_alloc = 0U;
        AppMain.g_ao_act_free = 0U;
        AppMain.g_ao_act_buf_size = 0U;
        AppMain.g_ao_act_num = 0U;
        AppMain.g_ao_act_peak = 0U;
        AppMain.g_ao_act_sort_buf = (AppMain.ArrayPointer<AppMain.AOS_ACT_SORT>)(AppMain.AOS_ACT_SORT[])null;
        AppMain.g_ao_act_sort_buf_size = 0U;
        AppMain.g_ao_act_sort_num = 0U;
        AppMain.g_ao_act_sort_peak = 0U;
        AppMain.g_ao_act_acm_buf = (AppMain.ArrayPointer<AppMain.AOS_ACT_ACM>)(AppMain.AOS_ACT_ACM[])null;
        AppMain.g_ao_act_acm_cur = (AppMain.ArrayPointer<AppMain.AOS_ACT_ACM>)(AppMain.AOS_ACT_ACM[])null;
        AppMain.g_ao_act_acm_buf_size = 0U;
        AppMain.g_ao_act_acm_num = 0U;
        AppMain.g_ao_act_acm_peak = 0U;
        AppMain.g_ao_act_acm_flag_buf = (AppMain.ArrayPointer<uint>)(uint[])null;
        AppMain.g_ao_act_acm_flag_cur = (AppMain.ArrayPointer<uint>)(uint[])null;
        AppMain.g_ao_act_acm_flag_buf_size = 0U;
        AppMain.g_ao_act_acm_flag_num = 0U;
        AppMain.g_ao_act_acm_flag_peak = 0U;
        AppMain.g_ao_act_texlist = (AppMain.NNS_TEXLIST)null;
    }

    private static uint AoActSysGetSprBufferSize()
    {
        return AppMain.g_ao_act_spr_buf_size;
    }

    private static uint AoActSysGetSprBufferRemain()
    {
        return AppMain.g_ao_act_spr_buf_size - AppMain.g_ao_act_spr_num;
    }

    private static uint AoActSysGetSprBufferPeak()
    {
        return AppMain.g_ao_act_spr_peak;
    }

    private static uint AoActSysGetActBufferSize()
    {
        return AppMain.g_ao_act_buf_size;
    }

    private static uint AoActSysGetActBufferRemain()
    {
        return AppMain.g_ao_act_buf_size - AppMain.g_ao_act_num;
    }

    private static uint AoActSysGetActBufferPeak()
    {
        return AppMain.g_ao_act_peak;
    }

    private static uint AoActSysGetSortBufferSize()
    {
        return AppMain.g_ao_act_sort_buf_size;
    }

    private static uint AoActSysGetSortBufferRemain()
    {
        return AppMain.g_ao_act_sort_buf_size - AppMain.g_ao_act_sort_num;
    }

    private static uint AoActSysGetSortBufferPeak()
    {
        return AppMain.g_ao_act_sort_peak;
    }

    public static uint AoActSysGetAcmStackSize()
    {
        return AppMain.g_ao_act_acm_buf_size;
    }

    public static uint AoActSysGetAcmStackRemain()
    {
        return AppMain.g_ao_act_acm_buf_size - AppMain.g_ao_act_acm_num;
    }

    public static uint AoActSysGetAcmBufferPeak()
    {
        return AppMain.g_ao_act_acm_peak;
    }

    public static uint AoActSysGetAcmFlagStackSize()
    {
        return AppMain.g_ao_act_acm_flag_buf_size;
    }

    public static uint AoActSysGetAcmFlagStackRemain()
    {
        return AppMain.g_ao_act_acm_flag_buf_size - AppMain.g_ao_act_acm_flag_num;
    }

    public static uint AoActSysGetAcmFlagBufferPeak()
    {
        return AppMain.g_ao_act_acm_flag_peak;
    }

    public static void AoActSysClearPeak()
    {
        AppMain.g_ao_act_spr_peak = 0U;
        AppMain.g_ao_act_peak = 0U;
        AppMain.g_ao_act_sort_peak = 0U;
        AppMain.g_ao_act_acm_peak = 0U;
        AppMain.g_ao_act_acm_flag_peak = 0U;
    }

    public static void AoActSysSetDrawTaskPrio()
    {
        AppMain.AoActSysSetDrawTaskPrio(4096U);
    }

    public static void AoActSysSetDrawTaskPrio(uint prio)
    {
        AppMain.g_ao_act_sys_draw_prio = prio;
    }

    public static uint AoActSysGetDrawTaskPrio()
    {
        return AppMain.g_ao_act_sys_draw_prio;
    }

    public static void AoActSysSetDrawStateEnable(bool enable)
    {
        AppMain.g_ao_act_sys_draw_state_enable = enable;
    }

    public static bool AoActSysGetDrawStateEnable()
    {
        return AppMain.g_ao_act_sys_draw_state_enable;
    }

    public static void AoActSysSetDrawState(uint state)
    {
        AppMain.g_ao_act_sys_draw_state = state;
    }

    public static uint AoActSysGetDrawState()
    {
        return AppMain.g_ao_act_sys_draw_state;
    }

    public static void AoActSysSetFrameRate(float rate)
    {
        AppMain.g_ao_act_sys_frame_rate = rate;
    }

    public static float AoActSysGetFrameRate()
    {
        return AppMain.g_ao_act_sys_frame_rate;
    }

    public static void AoActSysSetAdjust(float x, float y)
    {
        AppMain.g_ao_act_sys_adjust_x = x;
        AppMain.g_ao_act_sys_adjust_y = y;
    }

    public static void AoActSysAddAdjust(float x, float y)
    {
        AppMain.g_ao_act_sys_adjust_x += x;
        AppMain.g_ao_act_sys_adjust_y += y;
    }

    public static float AoActSysGetAdjustX()
    {
        return AppMain.g_ao_act_sys_adjust_x;
    }

    public static float AoActSysGetAdjustY()
    {
        return AppMain.g_ao_act_sys_adjust_y;
    }

}