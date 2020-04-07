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
    public static void AkUtilFrame60ToTime(uint frame)
    {
        ushort? nullable = new ushort?();
        AppMain.AkUtilFrame60ToTime(frame, ref nullable, ref nullable, ref nullable);
    }

    public static void AkUtilFrame60ToTime(uint frame, ref ushort min)
    {
        ushort? nullable = new ushort?();
        ushort? min1 = new ushort?(min);
        AppMain.AkUtilFrame60ToTime(frame, ref min1, ref nullable, ref nullable);
        min = min1.Value;
    }

    public static void AkUtilFrame60ToTime(uint frame, ref ushort min, ref ushort sec)
    {
        ushort? msec = new ushort?();
        ushort? min1 = new ushort?(min);
        ushort? sec1 = new ushort?(sec);
        AppMain.AkUtilFrame60ToTime(frame, ref min1, ref sec1, ref msec);
        min = min1.Value;
        sec = sec1.Value;
    }

    public static void AkUtilFrame60ToTime(
      uint frame,
      ref ushort min,
      ref ushort sec,
      ref ushort msec)
    {
        ushort? min1 = new ushort?(min);
        ushort? sec1 = new ushort?(sec);
        ushort? msec1 = new ushort?(msec);
        AppMain.AkUtilFrame60ToTime(frame, ref min1, ref sec1, ref msec1);
        min = min1.Value;
        sec = sec1.Value;
        msec = msec1.Value;
    }

    public static void AkUtilFrame60ToTime(uint frame, ushort? min, ref ushort sec, ushort? msec)
    {
        ushort? sec1 = new ushort?(sec);
        ushort? nullable = new ushort?();
        AppMain.AkUtilFrame60ToTime(frame, ref nullable, ref sec1, ref nullable);
        sec = sec1.Value;
    }

    public static void AkUtilFrame60ToTime(uint frame, ref ushort? min)
    {
        ushort? nullable = new ushort?();
        AppMain.AkUtilFrame60ToTime(frame, ref min, ref nullable, ref nullable);
    }

    public static void AkUtilFrame60ToTime(uint frame, ref ushort? min, ref ushort? sec)
    {
        ushort? msec = new ushort?();
        AppMain.AkUtilFrame60ToTime(frame, ref min, ref sec, ref msec);
    }

    public static void AkUtilFrame60ToTime(
      uint frame,
      ref ushort? min,
      ref ushort? sec,
      ref ushort? msec)
    {
        if ((long)frame > (long)AppMain.AKD_UTIL_FRAME60_TO_TIME_MAX_FRAME)
            frame = (uint)AppMain.AKD_UTIL_FRAME60_TO_TIME_MAX_FRAME;
        ushort num1 = (ushort)(frame / 3600U);
        frame -= (uint)((int)num1 * 60 * 60);
        ushort num2 = (ushort)(frame / 60U);
        frame -= (uint)num2 * 60U;
        ushort num3 = (ushort)(frame * 433U >> 8);
        if (num3 >= (ushort)100)
            num3 = (ushort)99;
        ushort? nullable1 = min;
        if ((nullable1.HasValue ? new int?((int)nullable1.GetValueOrDefault()) : new int?()).HasValue)
            min = new ushort?(num1);
        ushort? nullable2 = sec;
        if ((nullable2.HasValue ? new int?((int)nullable2.GetValueOrDefault()) : new int?()).HasValue)
            sec = new ushort?(num2);
        ushort? nullable3 = msec;
        if (!(nullable3.HasValue ? new int?((int)nullable3.GetValueOrDefault()) : new int?()).HasValue)
            return;
        msec = new ushort?(num3);
    }

    public static int AkUtilNumValueToDigits(int val, int[] digit_list, int digit_num)
    {
        return AppMain.AkUtilNumValueToDigits(val, digit_list, digit_num, 10);
    }

    public static int AkUtilNumValueToDigits(int val, int[] digit_list, int digit_num, int radix)
    {
        int num1 = val;
        int num2 = 1;
        for (int index = 0; index < digit_num; ++index)
        {
            int num3 = num1 % (num2 * radix);
            digit_list[index] = num3 / num2;
            num1 -= num3;
            num2 *= radix;
        }
        return num1;
    }

    public static int AkUtilNumValueToDigits(
      int val,
      AppMain.ArrayPointer<int> digit_list,
      int digit_num)
    {
        return AppMain.AkUtilNumValueToDigits(val, digit_list, digit_num, 10);
    }

    public static int AkUtilNumValueToDigits(
      int val,
      AppMain.ArrayPointer<int> digit_list,
      int digit_num,
      int radix)
    {
        int num1 = val;
        int num2 = 1;
        for (int index = 0; index < digit_num; ++index)
        {
            int num3 = num1 % (num2 * radix);
            digit_list.SetPrimitive(index, num3 / num2);
            num1 -= num3;
            num2 *= radix;
        }
        return num1;
    }

}