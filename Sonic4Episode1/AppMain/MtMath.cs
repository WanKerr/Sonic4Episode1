using System;
using System.Runtime.CompilerServices;

public partial class AppMain
{
    public static uint _mt_math_rand;

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void MTM_MATH_SWAP<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static uint MTM_MATH_MAX(uint a, uint b)
    {
        return Math.Max(a, b);
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static int MTM_MATH_MAX(int a, int b)
    {
        return Math.Max(a, b);
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static float MTM_MATH_CLIP(float a, float low, float high)
    {
        if (a < (double)low)
            return low;
        return a <= (double)high ? a : high;
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static int MTM_MATH_CLIP(int a, int low, int high)
    {
        if (a < low)
            return low;
        return a <= high ? a : high;
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static uint MTM_MATH_CLIP(uint a, uint low, uint high)
    {
        if (a < low)
            return low;
        return a <= high ? a : high;
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static int MTM_MATH_ABS(int a)
    {
        return a >= 0 ? a : -a;
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static float MTM_MATH_ABS(float a)
    {
        return a < 0.0 ? -a : a;
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static int mtMathSin(int angle)
    {
        return FX_Sin(angle);
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static int mtMathCos(int angle)
    {
        return FX_Cos(angle);
    }

    // TODO: is it worth replacing these with System.Random?

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void mtMathSRand(uint seed)
    {
        _mt_math_rand = seed;
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static ushort mtMathRand()
    {
        _mt_math_rand = (uint)(1663525 * (int)_mt_math_rand + 1013904223);
        return (ushort)(_mt_math_rand >> 16);
    }

}