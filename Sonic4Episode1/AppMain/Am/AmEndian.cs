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
    private static uint _amConvertEndian32(uint data)
    {
        return (uint)((int)((data & 4278190080U) >> 24) | (int)((data & 16711680U) >> 8) | ((int)data & 65280) << 8 | ((int)data & (int)byte.MaxValue) << 24);
    }

    private static ushort _amConvertEndian16(ushort data)
    {
        return (ushort)((int)data >> 8 & (int)byte.MaxValue | (int)data << 8);
    }

    private static ulong _amConvertEndian64(ulong data)
    {
        return (ulong)((long)(data >> 56) & (long)byte.MaxValue | ((long)(data >> 48) & (long)byte.MaxValue) << 8 | ((long)(data >> 40) & (long)byte.MaxValue) << 16 | ((long)(data >> 32) & (long)byte.MaxValue) << 24 | ((long)(data >> 24) & (long)byte.MaxValue) << 32 | ((long)(data >> 16) & (long)byte.MaxValue) << 40 | ((long)(data >> 8) & (long)byte.MaxValue) << 48 | ((long)data & (long)byte.MaxValue) << 56);
    }

    private static uint _amConvertEndian(uint data)
    {
        return AppMain._amConvertEndian32(data);
    }

    private static int _amConvertEndian(int data)
    {
        return (int)AppMain._amConvertEndian32((uint)data);
    }

    private static ushort _amConvertEndian(ushort data)
    {
        return AppMain._amConvertEndian16(data);
    }

    private static short _amConvertEndian(short data)
    {
        AppMain.mppAssertNotImpl();
        return (short)AppMain._amConvertEndian16((ushort)data);
    }

    private static ulong _amConvertEndian(ulong data)
    {
        return AppMain._amConvertEndian64(data);
    }

    private static long _amConvertEndian(long data)
    {
        return (long)AppMain._amConvertEndian64((ulong)data);
    }

    private static void _amConvertEndian(ref uint data)
    {
        data = AppMain._amConvertEndian32(data);
    }

    private static void _amConvertEndian(ref int data)
    {
        data = (int)AppMain._amConvertEndian32((uint)data);
    }

    private static void _amConvertEndian(ref ushort data)
    {
        data = AppMain._amConvertEndian16(data);
    }

    private static void _amConvertEndian(ref short data)
    {
        data = (short)AppMain._amConvertEndian16((ushort)data);
    }

    private static void _amConvertEndian(ref ulong data)
    {
        data = AppMain._amConvertEndian64(data);
    }

    private static void _amConvertEndian(ref long data)
    {
        data = (long)AppMain._amConvertEndian64((ulong)data);
    }
}