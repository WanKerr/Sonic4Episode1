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

    public static void MTM_MATH_SWAP<T>(ref T a, ref T b)
    {
        T obj = a;
        a = b;
        b = obj;
    }

    public static uint MTM_MATH_MAX(uint a, uint b)
    {
        return Math.Max(a, b);
    }

    public static int MTM_MATH_MAX(int a, int b)
    {
        return Math.Max(a, b);
    }

    public static float MTM_MATH_CLIP(float a, float low, float high)
    {
        if ((double)a < (double)low)
            return low;
        return (double)a <= (double)high ? a : high;
    }

    public static int MTM_MATH_CLIP(int a, int low, int high)
    {
        if (a < low)
            return low;
        return a <= high ? a : high;
    }

    public static uint MTM_MATH_CLIP(uint a, uint low, uint high)
    {
        if (a < low)
            return low;
        return a <= high ? a : high;
    }

    public static int MTM_MATH_ABS(int a)
    {
        return a >= 0 ? a : -a;
    }

    public static float MTM_MATH_ABS(float a)
    {
        return (double)a < 0.0 ? -a : a;
    }

    public static int mtMathSin(int angle)
    {
        return AppMain.FX_Sin(angle);
    }

    public static int mtMathCos(int angle)
    {
        return AppMain.FX_Cos(angle);
    }

    public static void mtMathSRand(uint seed)
    {
        AppMain._mt_math_rand = seed;
    }

    public static ushort mtMathRand()
    {
        AppMain._mt_math_rand = (uint)(1663525 * (int)AppMain._mt_math_rand + 1013904223);
        return (ushort)(AppMain._mt_math_rand >> 16);
    }

}