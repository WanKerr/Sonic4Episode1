using System;
using System.Runtime.CompilerServices;
using mpp;

using static AppMain.nncalcmatrixpalettemotion;

public partial class AppMain
{
#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static float NNM_TAYLOR_SIN(float f, float f2)
    {
        return f * (float)(1.0 + f2 * (-0.16666667163372 - f2 * -0.00833333376795053));
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    private static float NNM_TAYLOR_COS(float f, float f2)
    {
        return (float)(1.0 + f2 * (f2 * (0.0416666679084301 + f2 * (-1.0 / 720.0)) - 0.5));
    }

    private static float nnSin(int ang)
    {
        float num = 0.0f;
        int n = ang & ushort.MaxValue;
        switch (ang & 57344)
        {
            case 0:
                float f1 = NNM_A32toRAD(n);
                float f2_1 = f1 * f1;
                num = NNM_TAYLOR_SIN(f1, f2_1);
                break;
            case 8192:
            case 16384:
                float f2 = NNM_A32toRAD(n - 16384);
                float f2_2 = f2 * f2;
                num = NNM_TAYLOR_COS(f2, f2_2);
                break;
            case 24576:
            case 32768:
                float f3 = NNM_A32toRAD(n - 32768);
                float f2_3 = f3 * f3;
                num = -NNM_TAYLOR_SIN(f3, f2_3);
                break;
            case 40960:
            case 49152:
                float f4 = NNM_A32toRAD(n - 49152);
                float f2_4 = f4 * f4;
                num = -NNM_TAYLOR_COS(f4, f2_4);
                break;
            case 57344:
                float f5 = NNM_A32toRAD(n - 65536);
                float f2_5 = f5 * f5;
                num = NNM_TAYLOR_SIN(f5, f2_5);
                break;
        }
        return num;
    }

    private static float nnCos(int ang)
    {
        return nnSin(ang + 16384);
    }

    private static void nnCalcSinCosTable()
    {
        for (int index = 0; index < 36000; ++index)
            _nnSinCos((int)(index * 1.82044434547424), out nnSinTable[index], out nnCosTable[index]);
    }

    private static void nnSinCos(int ang, out float s, out float c)
    {
        int index = (int)Math.Round(ang * 0.54931640625);
        while (index > 35999)
            index -= 36000;
        while (index < 0)
            index += 36000;
        s = nnSinTable[index];
        c = nnCosTable[index];
    }

    private static void _nnSinCos(int ang, out float s, out float c)
    {
        if (ang == 0)
        {
            s = 0.0f;
            c = 1f;
        }
        int n = ang & ushort.MaxValue;
        c = 0.0f;
        s = 0.0f;
        switch (ang & 57344)
        {
            case 0:
                float f1 = NNM_A32toRAD(n);
                float f2_1 = f1 * f1;
                s = NNM_TAYLOR_SIN(f1, f2_1);
                c = NNM_TAYLOR_COS(f1, f2_1);
                break;
            case 8192:
            case 16384:
                float f2 = NNM_A32toRAD(n - 16384);
                float f2_2 = f2 * f2;
                s = NNM_TAYLOR_COS(f2, f2_2);
                c = -NNM_TAYLOR_SIN(f2, f2_2);
                break;
            case 24576:
            case 32768:
                float f3 = NNM_A32toRAD(n - 32768);
                float f2_3 = f3 * f3;
                s = -NNM_TAYLOR_SIN(f3, f2_3);
                c = -NNM_TAYLOR_COS(f3, f2_3);
                break;
            case 40960:
            case 49152:
                float f4 = NNM_A32toRAD(n - 49152);
                float f2_4 = f4 * f4;
                s = -NNM_TAYLOR_COS(f4, f2_4);
                c = NNM_TAYLOR_SIN(f4, f2_4);
                break;
            case 57344:
                float f5 = NNM_A32toRAD(n - 65536);
                float f2_5 = f5 * f5;
                s = NNM_TAYLOR_SIN(f5, f2_5);
                c = NNM_TAYLOR_COS(f5, f2_5);
                break;
        }
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void nnmSetUpVectorFast(out NNS_VECTORFAST dst, float x, float y, float z)
    {
        dst.x = x;
        dst.y = y;
        dst.z = z;
        dst.w = 1f;
    }

#if !WINDOWSPHONE7_5 && !NET40
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    public static void nnSetUpVectorFast(out NNS_VECTORFAST dst, float x, float y, float z)
    {
        dst.x = x;
        dst.y = y;
        dst.z = z;
        dst.w = 1f;
    }

    public static void nnAddVector(
      ref SNNS_VECTOR dst,
      ref SNNS_VECTOR vec1,
      ref SNNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnAddVector(
      NNS_VECTOR dst,
      NNS_VECTOR vec1,
      NNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    //public static void nnAddVector(
    //  ref SNNS_VECTOR dst,
    //  ref SNNS_VECTOR vec1,
    //  ref SNNS_VECTOR vec2)
    //{
    //    dst.x = vec1.x + vec2.x;
    //    dst.y = vec1.y + vec2.y;
    //    dst.z = vec1.z + vec2.z;
    //}

    public static void nnAddVector(
      ref SNNS_VECTOR dst,
      ref SNNS_VECTOR vec1,
      NNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnAddVector(
      ref SNNS_VECTOR dst,
      NNS_VECTOR vec1,
      ref SNNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnAddVector(
      ref SNNS_VECTOR dst,
      NNS_VECTOR vec1,
      NNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    //public static void nnAddVector(
    //  ref SNNS_VECTOR dst,
    //  ref SNNS_VECTOR vec1,
    //  NNS_VECTOR vec2)
    //{
    //    dst.x = vec1.x + vec2.x;
    //    dst.y = vec1.y + vec2.y;
    //    dst.z = vec1.z + vec2.z;
    //}

    //public static void nnAddVector(
    //  ref SNNS_VECTOR dst,
    //  ref SNNS_VECTOR vec1,
    //  ref SNNS_VECTOR vec2)
    //{
    //    dst.x = vec1.x + vec2.x;
    //    dst.y = vec1.y + vec2.y;
    //    dst.z = vec1.z + vec2.z;
    //}

    public static void nnCrossProductVector(
      ref SNNS_VECTOR dst,
      ref SNNS_VECTOR vec1,
      ref SNNS_VECTOR vec2)
    {
        float num1 = (float)(vec1.y * (double)vec2.z - vec1.z * (double)vec2.y);
        float num2 = (float)(vec1.z * (double)vec2.x - vec1.x * (double)vec2.z);
        float num3 = (float)(vec1.x * (double)vec2.y - vec1.y * (double)vec2.x);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    public static void nnCrossProductVector(
      NNS_VECTOR dst,
      NNS_VECTOR vec1,
      NNS_VECTOR vec2)
    {
        float num1 = (float)(vec1.y * (double)vec2.z - vec1.z * (double)vec2.y);
        float num2 = (float)(vec1.z * (double)vec2.x - vec1.x * (double)vec2.z);
        float num3 = (float)(vec1.x * (double)vec2.y - vec1.y * (double)vec2.x);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    public static void nnCopyVector(NNS_VECTOR dst, ref SNNS_VECTOR src)
    {
        dst.Assign(ref src);
    }

    public static void nnCopyVector(NNS_VECTOR dst, ref SNNS_VECTOR4D src)
    {
        dst.Assign(ref src);
    }

    public static void nnCopyVector(NNS_VECTOR dst, NNS_VECTOR src)
    {
        dst.Assign(src);
    }

    public static float nnDotProductVector(NNS_VECTOR vec1, NNS_VECTOR vec2)
    {
        return (float)(vec1.x * (double)vec2.x + vec1.y * (double)vec2.y + vec1.z * (double)vec2.z);
    }

    public static float nnLengthVector(NNS_VECTOR vec)
    {
        return nnSqrt((float)(vec.x * (double)vec.x + vec.y * (double)vec.y + vec.z * (double)vec.z));
    }

    public static float nnDistanceVector(NNS_VECTOR vec1, ref SNNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return nnSqrt((float)(num1 * (double)num1 + num2 * (double)num2 + num3 * (double)num3));
    }

    public static float nnDistanceVector(ref SNNS_VECTOR vec1, NNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return nnSqrt((float)(num1 * (double)num1 + num2 * (double)num2 + num3 * (double)num3));
    }

    public static float nnDistanceVector(NNS_VECTOR vec1, NNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return nnSqrt((float)(num1 * (double)num1 + num2 * (double)num2 + num3 * (double)num3));
    }

    public static float nnDistanceVector(ref SNNS_VECTOR vec1, ref SNNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return nnSqrt((float)(num1 * (double)num1 + num2 * (double)num2 + num3 * (double)num3));
    }

    //public static float nnDistanceVector(ref SNNS_VECTOR vec1, NNS_VECTOR vec2)
    //{
    //    float num1 = vec2.x - vec1.x;
    //    float num2 = vec2.y - vec1.y;
    //    float num3 = vec2.z - vec1.z;
    //    return nnSqrt((float)(num1 * (double)num1 + num2 * (double)num2 + num3 * (double)num3));
    //}

    public static float nnLengthSqVector(NNS_VECTOR vec)
    {
        return (float)(vec.x * (double)vec.x + vec.y * (double)vec.y + vec.z * (double)vec.z);
    }

    public static float nnLengthSqVector(ref SNNS_VECTOR vec)
    {
        return (float)(vec.x * (double)vec.x + vec.y * (double)vec.y + vec.z * (double)vec.z);
    }

    public static float nnLengthSqVector(float[] vec)
    {
        return (float)(vec[0] * (double)vec[0] + vec[1] * (double)vec[1] + vec[2] * (double)vec[2]);
    }

    public static float nnLengthSqVector(ref OpenGL.glArray4f vec)
    {
        return (float)(vec.f0 * (double)vec.f0 + vec.f1 * (double)vec.f1 + vec.f2 * (double)vec.f2);
    }

    public static float nnDistanceSqVector(NNS_VECTOR vec1, NNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return (float)(num1 * (double)num1 + num2 * (double)num2 + num3 * (double)num3);
    }

    public static int nnNormalizeVector(NNS_VECTOR dst, NNS_VECTOR src)
    {
        float n = nnLengthSqVector(src);
        if (n == 0.0)
        {
            dst.x = 0.0f;
            dst.y = 0.0f;
            dst.z = 0.0f;
            return 0;
        }
        float num = nnInvertSqrt(n);
        dst.x = src.x * num;
        dst.y = src.y * num;
        dst.z = src.z * num;
        return 1;
    }

    public static int nnNormalizeVector(ref SNNS_VECTOR dst, ref SNNS_VECTOR src)
    {
        float n = nnLengthSqVector(ref src);
        if (n == 0.0)
        {
            dst.x = 0.0f;
            dst.y = 0.0f;
            dst.z = 0.0f;
            return 0;
        }
        float num = nnInvertSqrt(n);
        dst.x = src.x * num;
        dst.y = src.y * num;
        dst.z = src.z * num;
        return 1;
    }

    public static int nnNormalizeVector(float[] dst, float[] src)
    {
        float n = nnLengthSqVector(src);
        if (n == 0.0)
        {
            dst[0] = 0.0f;
            dst[1] = 0.0f;
            dst[2] = 0.0f;
            return 0;
        }
        float num = nnInvertSqrt(n);
        dst[0] = src[0] * num;
        dst[1] = src[1] * num;
        dst[2] = src[2] * num;
        return 1;
    }

    public static int nnNormalizeVector(ref OpenGL.glArray4f dst, ref OpenGL.glArray4f src)
    {
        float n = nnLengthSqVector(ref src);
        if (n == 0.0)
        {
            dst.f0 = 0.0f;
            dst.f1 = 0.0f;
            dst.f2 = 0.0f;
            return 0;
        }
        float num = nnInvertSqrt(n);
        dst.f0 = src.f0 * num;
        dst.f1 = src.f1 * num;
        dst.f2 = src.f2 * num;
        return 1;
    }

    public static void nnScaleVector(
      ref SNNS_VECTOR dst,
      ref SNNS_VECTOR src,
      float scale)
    {
        dst.x = src.x * scale;
        dst.y = src.y * scale;
        dst.z = src.z * scale;
    }

    public static void nnScaleVector(NNS_VECTOR dst, NNS_VECTOR src, float scale)
    {
        dst.x = src.x * scale;
        dst.y = src.y * scale;
        dst.z = src.z * scale;
    }

    public static void nnScaleAddVector(
      NNS_VECTOR dst,
      NNS_VECTOR vec1,
      NNS_VECTOR vec2,
      float scale)
    {
        float num1 = vec2.x * scale;
        float num2 = vec2.y * scale;
        float num3 = vec2.z * scale;
        dst.x = vec1.x + num1;
        dst.y = vec1.y + num2;
        dst.z = vec1.z + num3;
    }

    public static void nnSubtractVector(
      NNS_VECTOR dst,
      NNS_VECTOR vec1,
      NNS_VECTOR vec2)
    {
        dst.x = vec1.x - vec2.x;
        dst.y = vec1.y - vec2.y;
        dst.z = vec1.z - vec2.z;
    }

    public static void nnSubtractVector(
      ref SNNS_VECTOR dst,
      NNS_VECTOR vec1,
      NNS_VECTOR vec2)
    {
        dst.x = vec1.x - vec2.x;
        dst.y = vec1.y - vec2.y;
        dst.z = vec1.z - vec2.z;
    }

    public static void nnSubtractVector(
      ref SNNS_VECTOR dst,
      ref SNNS_VECTOR vec1,
      NNS_VECTOR vec2)
    {
        dst.x = vec1.x - vec2.x;
        dst.y = vec1.y - vec2.y;
        dst.z = vec1.z - vec2.z;
    }

    public static void nnSubtractVector(
      ref SNNS_VECTOR dst,
      ref SNNS_VECTOR vec1,
      ref SNNS_VECTOR vec2)
    {
        dst.x = vec1.x - vec2.x;
        dst.y = vec1.y - vec2.y;
        dst.z = vec1.z - vec2.z;
    }

    //public static void nnSubtractVector(
    //  ref SNNS_VECTOR dst,
    //  ref SNNS_VECTOR vec1,
    //  NNS_VECTOR vec2)
    //{
    //    dst.x = vec1.x - vec2.x;
    //    dst.y = vec1.y - vec2.y;
    //    dst.z = vec1.z - vec2.z;
    //}

    //public static void nnSubtractVector(
    //  ref SNNS_VECTOR dst,
    //  ref SNNS_VECTOR vec1,
    //  ref SNNS_VECTOR vec2)
    //{
    //    dst.x = vec1.x - vec2.x;
    //    dst.y = vec1.y - vec2.y;
    //    dst.z = vec1.z - vec2.z;
    //}

    private static void nnTransformVector(
      ref SNNS_VECTOR dst,
      ref SNNS_MATRIX mtx,
      ref SNNS_VECTOR src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformVector(
      NNS_VECTOR dst,
      NNS_MATRIX mtx,
      NNS_VECTOR src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformVector(
      ref SNNS_VECTOR dst,
      NNS_MATRIX mtx,
      NNS_VECTOR src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    //private static void nnTransformVector(
    //  ref SNNS_VECTOR dst,
    //  ref SNNS_MATRIX mtx,
    //  ref SNNS_VECTOR src)
    //{
    //    float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z) + mtx.M03;
    //    float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z) + mtx.M13;
    //    float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z) + mtx.M23;
    //    dst.x = num1;
    //    dst.y = num2;
    //    dst.z = num3;
    //}

    private static void nnTransformVector(
      ref SNNS_VECTOR dst,
      NNS_MATRIX mtx,
      ref SNNS_VECTOR src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    //private static void nnTransformVector(
    //  ref SNNS_VECTOR dst,
    //  NNS_MATRIX mtx,
    //  ref SNNS_VECTOR src)
    //{
    //    float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z) + mtx.M03;
    //    float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z) + mtx.M13;
    //    float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z) + mtx.M23;
    //    dst.x = num1;
    //    dst.y = num2;
    //    dst.z = num3;
    //}

    private static void nnTransformVector(
      NNS_VECTOR4D dst,
      NNS_MATRIX mtx,
      NNS_VECTOR4D src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformVector(
      ref SNNS_VECTOR4D dst,
      NNS_MATRIX mtx,
      ref SNNS_VECTOR4D src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformNormalVector(
      NNS_VECTOR dst,
      NNS_MATRIX mtx,
      NNS_VECTOR src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z);
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z);
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformNormalVector(
      NNS_VECTOR4D dst,
      NNS_MATRIX mtx,
      NNS_VECTOR4D src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z);
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z);
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformNormalVector(
      ref SNNS_VECTOR4D dst,
      NNS_MATRIX mtx,
      NNS_VECTOR4D src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z);
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z);
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformNormalVector(
      ref SNNS_VECTOR4D dst,
      NNS_MATRIX mtx,
      ref SNNS_VECTOR4D src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z);
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z);
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformNormalVector(
      NNS_VECTOR4D dst,
      NNS_MATRIX mtx,
      ref SNNS_VECTOR4D src)
    {
        float num1 = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z);
        float num2 = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z);
        float num3 = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnCopyMatrixTranslationVector(NNS_VECTOR dst, NNS_MATRIX mtx)
    {
        dst.x = mtx.M03;
        dst.y = mtx.M13;
        dst.z = mtx.M23;
    }

    private static void nnCopyMatrixTranslationVector(
      out SNNS_VECTOR dst,
      NNS_MATRIX mtx)
    {
        dst.x = mtx.M03;
        dst.y = mtx.M13;
        dst.z = mtx.M23;
    }

    private static void nnCopyMatrixTranslationVector(
      out SNNS_VECTOR dst,
      ref SNNS_MATRIX mtx)
    {
        dst.x = mtx.M03;
        dst.y = mtx.M13;
        dst.z = mtx.M23;
    }

    private static void nnCopyMatrixTranslationVector(
      NNS_VECTOR dst,
      ref SNNS_MATRIX mtx)
    {
        dst.x = mtx.M03;
        dst.y = mtx.M13;
        dst.z = mtx.M23;
    }

    private static void nnSubtractVectorFast(
      out NNS_VECTORFAST dst,
      NNS_VECTORFAST vec1,
      NNS_VECTORFAST vec2)
    {
        float x = vec1.x - vec2.x;
        float y = vec1.y - vec2.y;
        float z = vec1.z - vec2.z;
        nnmSetUpVectorFast(out dst, x, y, z);
    }

    private static void nnTransformVectorFast(
      out NNS_VECTORFAST dst,
      NNS_MATRIX mtx,
      NNS_VECTORFAST src)
    {
        float x = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z) + mtx.M03;
        float y = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z) + mtx.M13;
        float z = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z) + mtx.M23;
        nnmSetUpVectorFast(out dst, x, y, z);
    }

    private static void nnTransformNormalVectorFast(
      out NNS_VECTORFAST dst,
      NNS_MATRIX mtx,
      NNS_VECTORFAST src)
    {
        float x = (float)(mtx.M00 * (double)src.x + mtx.M01 * (double)src.y + mtx.M02 * (double)src.z);
        float y = (float)(mtx.M10 * (double)src.x + mtx.M11 * (double)src.y + mtx.M12 * (double)src.z);
        float z = (float)(mtx.M20 * (double)src.x + mtx.M21 * (double)src.y + mtx.M22 * (double)src.z);
        nnmSetUpVectorFast(out dst, x, y, z);
    }

    private static void nnCopyMatrixTranslationVectorFast(
      out NNS_VECTORFAST dst,
      NNS_MATRIX mtx)
    {
        dst.x = mtx.M03;
        dst.y = mtx.M13;
        dst.z = mtx.M23;
        dst.w = 1f;
    }

    private static float nnLengthSqVectorFast(ref NNS_VECTORFAST vec)
    {
        return (float)(vec.x * (double)vec.x + vec.y * (double)vec.y + vec.z * (double)vec.z);
    }

    private static float nnDotProductVectorFast(
      ref NNS_VECTORFAST vec1,
      ref NNS_VECTORFAST vec2)
    {
        return (float)(vec1.x * (double)vec2.x + vec1.y * (double)vec2.y + vec1.z * (double)vec2.z);
    }
    private void nnMakeCameraPointerViewMatrix(NNS_MATRIX mtx, NNS_CAMERAPTR camptr)
    {
        mppAssertNotImpl();
        switch (camptr.fType)
        {
            case byte.MaxValue:
                nnMakeTargetRollCameraViewMatrix(mtx, (NNS_CAMERA_TARGET_ROLL)camptr.pCamera);
                break;
            case 383:
                nnMakeTargetUpVectorCameraViewMatrix(mtx, (NNS_CAMERA_TARGET_UPVECTOR)camptr.pCamera);
                break;
            case 639:
                nnMakeTargetUpTargetCameraViewMatrix(mtx, (NNS_CAMERA_TARGET_UPTARGET)camptr.pCamera);
                break;
            case 3135:
                this.nnMakeRotationCameraViewMatrix(mtx, (NNS_CAMERA_ROTATION)camptr.pCamera);
                break;
        }
    }

    private void nnMakeCameraPointerPerspectiveMatrix(
      NNS_MATRIX dst,
      NNS_CAMERAPTR camptr)
    {
        mppAssertNotImpl();
    }

    private static void nnMakeTargetRollCameraViewMatrix(
      NNS_MATRIX mtx,
      NNS_CAMERA_TARGET_ROLL cam)
    {
        SNNS_VECTOR snnsVector1 = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector2 = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector3 = new SNNS_VECTOR();
        snnsVector3.x = cam.Position.x - cam.Target.x;
        snnsVector3.y = cam.Position.y - cam.Target.y;
        snnsVector3.z = cam.Position.z - cam.Target.z;
        nnNormalizeVector(ref snnsVector3, ref snnsVector3);
        snnsVector1.x = snnsVector3.z;
        snnsVector1.y = 0.0f;
        snnsVector1.z = -snnsVector3.x;
        nnNormalizeVector(ref snnsVector1, ref snnsVector1);
        nnCrossProductVector(ref snnsVector2, ref snnsVector3, ref snnsVector1);
        nnMakeVectorCameraViewMatrix(mtx, cam.Position, ref snnsVector1, ref snnsVector2, ref snnsVector3);
        SNNS_MATRIX dst;
        nnMakeRotateZMatrix(out dst, -cam.Roll);
        nnMultiplyMatrix(mtx, ref dst, mtx);
    }

    private static void nnMakeTargetUpVectorCameraViewMatrix(
      NNS_MATRIX mtx,
      NNS_CAMERA_TARGET_UPVECTOR cam)
    {
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector3 = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector1.x = cam.Position.x - cam.Target.x;
        nnsVector1.y = cam.Position.y - cam.Target.y;
        nnsVector1.z = cam.Position.z - cam.Target.z;
        nnNormalizeVector(nnsVector1, nnsVector1);
        nnCrossProductVector(nnsVector2, cam.UpVector, nnsVector1);
        nnNormalizeVector(nnsVector2, nnsVector2);
        nnCrossProductVector(nnsVector3, nnsVector1, nnsVector2);
        nnMakeVectorCameraViewMatrix(mtx, cam.Position, nnsVector2, nnsVector3, nnsVector1);
    }

    private static void nnMakeTargetUpTargetCameraViewMatrix(
      NNS_MATRIX mtx,
      NNS_CAMERA_TARGET_UPTARGET cam)
    {
        mppAssertNotImpl();
    }

    private void nnMakeRotationCameraViewMatrix(
      NNS_MATRIX mtx,
      NNS_CAMERA_ROTATION cam)
    {
        mppAssertNotImpl();
    }


    public static void nnmRotateMatrixFast(NNS_MATRIX mtx, int ang, int ma, int mb)
    {
        if (ang == 0)
            return;
        float s;
        float c;
        nnSinCos(ang, out s, out c);
        float num1 = s;
        float num2 = c;
        float num3 = mtx.M(0, ma);
        float num4 = mtx.M(0, mb);
        mtx.SetM(0, ma, (float)(num3 * (double)num2 + num4 * (double)num1));
        mtx.SetM(0, mb, (float)(num3 * -(double)num1 + num4 * (double)num2));
        float num5 = mtx.M(1, ma);
        float num6 = mtx.M(1, mb);
        mtx.SetM(1, ma, (float)(num5 * (double)num2 + num6 * (double)num1));
        mtx.SetM(1, mb, (float)(num5 * -(double)num1 + num6 * (double)num2));
        float num7 = mtx.M(2, ma);
        float num8 = mtx.M(2, mb);
        mtx.SetM(2, ma, (float)(num7 * (double)num2 + num8 * (double)num1));
        mtx.SetM(2, mb, (float)(num7 * -(double)num1 + num8 * (double)num2));
    }

    private static void nnMakeVectorCameraViewMatrix(
      NNS_MATRIX mtx,
      NNS_VECTOR pos,
      NNS_VECTOR right,
      NNS_VECTOR up,
      NNS_VECTOR ilook)
    {
        mtx.M00 = right.x;
        mtx.M01 = right.y;
        mtx.M02 = right.z;
        mtx.M03 = (float)-(right.x * (double)pos.x + right.y * (double)pos.y + right.z * (double)pos.z);
        mtx.M10 = up.x;
        mtx.M11 = up.y;
        mtx.M12 = up.z;
        mtx.M13 = (float)-(up.x * (double)pos.x + up.y * (double)pos.y + up.z * (double)pos.z);
        mtx.M20 = ilook.x;
        mtx.M21 = ilook.y;
        mtx.M22 = ilook.z;
        mtx.M23 = (float)-(ilook.x * (double)pos.x + ilook.y * (double)pos.y + ilook.z * (double)pos.z);
        mtx.M30 = mtx.M31 = mtx.M32 = 0.0f;
        mtx.M33 = 1f;
    }

    private static void nnMakeVectorCameraViewMatrix(
      NNS_MATRIX mtx,
      NNS_VECTOR pos,
      ref SNNS_VECTOR right,
      ref SNNS_VECTOR up,
      ref SNNS_VECTOR ilook)
    {
        mtx.M00 = right.x;
        mtx.M01 = right.y;
        mtx.M02 = right.z;
        mtx.M03 = (float)-(right.x * (double)pos.x + right.y * (double)pos.y + right.z * (double)pos.z);
        mtx.M10 = up.x;
        mtx.M11 = up.y;
        mtx.M12 = up.z;
        mtx.M13 = (float)-(up.x * (double)pos.x + up.y * (double)pos.y + up.z * (double)pos.z);
        mtx.M20 = ilook.x;
        mtx.M21 = ilook.y;
        mtx.M22 = ilook.z;
        mtx.M23 = (float)-(ilook.x * (double)pos.x + ilook.y * (double)pos.y + ilook.z * (double)pos.z);
        mtx.M30 = mtx.M31 = mtx.M32 = 0.0f;
        mtx.M33 = 1f;
    }

    private void nnCalcNodeStatusListMatrixList(
      uint[] nodestatlist,
      NNS_OBJECT obj,
      NNS_MATRIX mtxlist,
      NNS_MATRIX basemtx,
      uint flag)
    {
        mppAssertNotImpl();
    }

    private static float NNM_DEGtoRAD(float n)
    {
        return n * ((float)Math.PI / 180f);
    }

    private static float NNM_DEGtoRAD(int n)
    {
        return n * ((float)Math.PI / 180f);
    }

    private static int NNM_DEGtoA32(float n)
    {
        return (int)(n * 182.04443359375);
    }

    private static int NNM_RADtoA32(float n)
    {
        return (int)(n * 10430.3779296875);
    }

    private static float NNM_RADtoDEG(float n)
    {
        return n * 57.29578f;
    }

    private static float NNM_A32toDEG(float n)
    {
        return n * 0.005493164f;
    }

    private static float NNM_A32toRAD(float n)
    {
        return n * 9.58738E-05f;
    }

    private static int NNM_DEGtoA32(int n)
    {
        return (int)(n * 182.04443359375);
    }

    private static int NNM_RADtoA32(int n)
    {
        return (int)(n * 10430.3779296875);
    }

    private static float NNM_RADtoDEG(int n)
    {
        return n * 57.29578f;
    }

    private static float NNM_A32toDEG(int n)
    {
        return n * 0.005493164f;
    }

    private static float NNM_A32toRAD(int n)
    {
        return n * 9.58738E-05f;
    }

    private static int NNM_DEGtoA16(float n)
    {
        return (short)(int)(n * 182.04443359375);
    }

    public static int NNM_MAX(int a, int b)
    {
        return Math.Max(a, b);
    }

    public static float NNM_MAX(float a, float b)
    {
        return Math.Max(a, b);
    }

    public static int NNM_MIN(int a, int b)
    {
        return Math.Min(a, b);
    }

    public static float NNM_MIN(float a, float b)
    {
        return Math.Min(a, b);
    }
    private static void nnInvertTransposeMatrix33(NNS_MATRIX dst, NNS_MATRIX src)
    {
        float m00 = src.M00;
        float m01 = src.M01;
        float m02 = src.M02;
        float m10 = src.M10;
        float m11 = src.M11;
        float m12 = src.M12;
        float m20 = src.M20;
        float m21 = src.M21;
        float m22 = src.M22;
        float num1 = (float)(m11 * (double)m22 - m12 * (double)m21);
        float num2 = (float)(m12 * (double)m20 - m10 * (double)m22);
        float num3 = (float)(m10 * (double)m21 - m11 * (double)m20);
        float num4 = (float)(m00 * (double)num1 + m01 * (double)num2 + m02 * (double)num3);
        if (num4 == 0.0)
        {
            dst.M00 = 0.0f;
            dst.M01 = 0.0f;
            dst.M02 = 0.0f;
            dst.M10 = 0.0f;
            dst.M11 = 0.0f;
            dst.M12 = 0.0f;
            dst.M20 = 0.0f;
            dst.M21 = 0.0f;
            dst.M22 = 0.0f;
        }
        else
        {
            float num5 = 1f / num4;
            dst.M00 = num1 * num5;
            dst.M10 = (float)-(m01 * (double)m22 - m21 * (double)m02) * num5;
            dst.M20 = (float)(m01 * (double)m12 - m11 * (double)m02) * num5;
            dst.M01 = num2 * num5;
            dst.M11 = (float)(m00 * (double)m22 - m20 * (double)m02) * num5;
            dst.M21 = (float)-(m00 * (double)m12 - m10 * (double)m02) * num5;
            dst.M02 = num3 * num5;
            dst.M12 = (float)-(m00 * (double)m21 - m20 * (double)m01) * num5;
            dst.M22 = (float)(m00 * (double)m11 - m10 * (double)m01) * num5;
        }
    }

    private static void nnInvertTransposeMatrix33NotNormalized(
      NNS_MATRIX dst,
      NNS_MATRIX src)
    {
        float m00 = src.M00;
        float m01 = src.M01;
        float m02 = src.M02;
        float m10 = src.M10;
        float m11 = src.M11;
        float m12 = src.M12;
        float m20 = src.M20;
        float m21 = src.M21;
        float m22 = src.M22;
        dst.M00 = (float)(m11 * (double)m22 - m21 * (double)m12);
        dst.M10 = (float)-(m01 * (double)m22 - m21 * (double)m02);
        dst.M20 = (float)(m01 * (double)m12 - m11 * (double)m02);
        dst.M01 = (float)-(m10 * (double)m22 - m20 * (double)m12);
        dst.M11 = (float)(m00 * (double)m22 - m20 * (double)m02);
        dst.M21 = (float)-(m00 * (double)m12 - m10 * (double)m02);
        dst.M02 = (float)(m10 * (double)m21 - m20 * (double)m11);
        dst.M12 = (float)-(m00 * (double)m21 - m20 * (double)m01);
        dst.M22 = (float)(m00 * (double)m11 - m10 * (double)m01);
    }
    public static void nnCopyMatrix(NNS_MATRIX dst, NNS_MATRIX src)
    {
        dst.Assign(src);
    }

    public static void nnCopyMatrix(NNS_MATRIX dst, ref SNNS_MATRIX src)
    {
        dst.Assign(ref src);
    }

    public static void nnCopyMatrix(ref SNNS_MATRIX dst, ref SNNS_MATRIX src)
    {
        dst.Assign(ref src);
    }

    public static void nnCopyMatrix(ref SNNS_MATRIX dst, NNS_MATRIX src)
    {
        dst.Assign(src);
    }


    public static bool nnInvertMatrix(NNS_MATRIX dst, NNS_MATRIX src)
    {
        float m00 = src.M00;
        float m01 = src.M01;
        float m02 = src.M02;
        float m03 = src.M03;
        float m10 = src.M10;
        float m11 = src.M11;
        float m12 = src.M12;
        float m13 = src.M13;
        float m20 = src.M20;
        float m21 = src.M21;
        float m22 = src.M22;
        float m23 = src.M23;
        float num1 = (float)(m11 * (double)m22 - m12 * (double)m21);
        float num2 = (float)(m12 * (double)m20 - m10 * (double)m22);
        float num3 = (float)(m10 * (double)m21 - m11 * (double)m20);
        float num4 = (float)(m00 * (double)num1 + m01 * (double)num2 + m02 * (double)num3);
        if (num4 == 0.0)
        {
            dst.Clear();
            return false;
        }
        float num5 = 1f / num4;
        dst.M00 = num1 * num5;
        dst.M01 = (float)-(m01 * (double)m22 - m21 * (double)m02) * num5;
        dst.M02 = (float)(m01 * (double)m12 - m11 * (double)m02) * num5;
        dst.M10 = num2 * num5;
        dst.M11 = (float)(m00 * (double)m22 - m20 * (double)m02) * num5;
        dst.M12 = (float)-(m00 * (double)m12 - m10 * (double)m02) * num5;
        dst.M20 = num3 * num5;
        dst.M21 = (float)-(m00 * (double)m21 - m20 * (double)m01) * num5;
        dst.M22 = (float)(m00 * (double)m11 - m10 * (double)m01) * num5;
        dst.M03 = (float)(-dst.M00 * (double)m03 - dst.M01 * (double)m13 - dst.M02 * (double)m23);
        dst.M13 = (float)(-dst.M10 * (double)m03 - dst.M11 * (double)m13 - dst.M12 * (double)m23);
        dst.M23 = (float)(-dst.M20 * (double)m03 - dst.M21 * (double)m13 - dst.M22 * (double)m23);
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
        return true;
    }

    public static void nnInvertOrthoMatrix(NNS_MATRIX dst, NNS_MATRIX src)
    {
        float m00 = src.M00;
        float m01 = src.M01;
        float m02 = src.M02;
        float m03 = src.M03;
        float m10 = src.M10;
        float m11 = src.M11;
        float m12 = src.M12;
        float m13 = src.M13;
        float m20 = src.M20;
        float m21 = src.M21;
        float m22 = src.M22;
        float m23 = src.M23;
        dst.M01 = m10;
        dst.M02 = m20;
        dst.M10 = m01;
        dst.M12 = m21;
        dst.M20 = m02;
        dst.M21 = m12;
        dst.M03 = (float)-(m00 * (double)m03 + m10 * (double)m13 + m20 * (double)m23);
        dst.M13 = (float)-(m01 * (double)m03 + m11 * (double)m13 + m21 * (double)m23);
        dst.M23 = (float)-(m02 * (double)m03 + m12 * (double)m13 + m22 * (double)m23);
        if (src != dst)
        {
            dst.M00 = m00;
            dst.M11 = m11;
            dst.M22 = m22;
        }
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      NNS_MATRIX dst,
      ref SNNS_MATRIX mtx1,
      NNS_MATRIX mtx2)
    {
        float m00_1 = mtx1.M00;
        float m10_1 = mtx1.M10;
        float m20_1 = mtx1.M20;
        float m01_1 = mtx1.M01;
        float m11_1 = mtx1.M11;
        float m21_1 = mtx1.M21;
        float m02_1 = mtx1.M02;
        float m12_1 = mtx1.M12;
        float m22_1 = mtx1.M22;
        float m03_1 = mtx1.M03;
        float m13_1 = mtx1.M13;
        float m23_1 = mtx1.M23;
        float m00_2 = mtx2.M00;
        float m10_2 = mtx2.M10;
        float m20_2 = mtx2.M20;
        dst.M00 = (float)(m00_1 * (double)m00_2 + m01_1 * (double)m10_2 + m02_1 * (double)m20_2);
        dst.M10 = (float)(m10_1 * (double)m00_2 + m11_1 * (double)m10_2 + m12_1 * (double)m20_2);
        dst.M20 = (float)(m20_1 * (double)m00_2 + m21_1 * (double)m10_2 + m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)(m00_1 * (double)m01_2 + m01_1 * (double)m11_2 + m02_1 * (double)m21_2);
        dst.M11 = (float)(m10_1 * (double)m01_2 + m11_1 * (double)m11_2 + m12_1 * (double)m21_2);
        dst.M21 = (float)(m20_1 * (double)m01_2 + m21_1 * (double)m11_2 + m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)(m00_1 * (double)m02_2 + m01_1 * (double)m12_2 + m02_1 * (double)m22_2);
        dst.M12 = (float)(m10_1 * (double)m02_2 + m11_1 * (double)m12_2 + m12_1 * (double)m22_2);
        dst.M22 = (float)(m20_1 * (double)m02_2 + m21_1 * (double)m12_2 + m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)(m00_1 * (double)m03_2 + m01_1 * (double)m13_2 + m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)(m10_1 * (double)m03_2 + m11_1 * (double)m13_2 + m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)(m20_1 * (double)m03_2 + m21_1 * (double)m13_2 + m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      NNS_MATRIX dst,
      NNS_MATRIX mtx1,
      ref SNNS_MATRIX mtx2)
    {
        float m00_1 = mtx1.M00;
        float m10_1 = mtx1.M10;
        float m20_1 = mtx1.M20;
        float m01_1 = mtx1.M01;
        float m11_1 = mtx1.M11;
        float m21_1 = mtx1.M21;
        float m02_1 = mtx1.M02;
        float m12_1 = mtx1.M12;
        float m22_1 = mtx1.M22;
        float m03_1 = mtx1.M03;
        float m13_1 = mtx1.M13;
        float m23_1 = mtx1.M23;
        float m00_2 = mtx2.M00;
        float m10_2 = mtx2.M10;
        float m20_2 = mtx2.M20;
        dst.M00 = (float)(m00_1 * (double)m00_2 + m01_1 * (double)m10_2 + m02_1 * (double)m20_2);
        dst.M10 = (float)(m10_1 * (double)m00_2 + m11_1 * (double)m10_2 + m12_1 * (double)m20_2);
        dst.M20 = (float)(m20_1 * (double)m00_2 + m21_1 * (double)m10_2 + m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)(m00_1 * (double)m01_2 + m01_1 * (double)m11_2 + m02_1 * (double)m21_2);
        dst.M11 = (float)(m10_1 * (double)m01_2 + m11_1 * (double)m11_2 + m12_1 * (double)m21_2);
        dst.M21 = (float)(m20_1 * (double)m01_2 + m21_1 * (double)m11_2 + m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)(m00_1 * (double)m02_2 + m01_1 * (double)m12_2 + m02_1 * (double)m22_2);
        dst.M12 = (float)(m10_1 * (double)m02_2 + m11_1 * (double)m12_2 + m12_1 * (double)m22_2);
        dst.M22 = (float)(m20_1 * (double)m02_2 + m21_1 * (double)m12_2 + m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)(m00_1 * (double)m03_2 + m01_1 * (double)m13_2 + m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)(m10_1 * (double)m03_2 + m11_1 * (double)m13_2 + m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)(m20_1 * (double)m03_2 + m21_1 * (double)m13_2 + m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      NNS_MATRIX dst,
      NNS_MATRIX mtx1,
      NNS_MATRIX mtx2)
    {
        float m00_1 = mtx1.M00;
        float m10_1 = mtx1.M10;
        float m20_1 = mtx1.M20;
        float m01_1 = mtx1.M01;
        float m11_1 = mtx1.M11;
        float m21_1 = mtx1.M21;
        float m02_1 = mtx1.M02;
        float m12_1 = mtx1.M12;
        float m22_1 = mtx1.M22;
        float m03_1 = mtx1.M03;
        float m13_1 = mtx1.M13;
        float m23_1 = mtx1.M23;
        float m00_2 = mtx2.M00;
        float m10_2 = mtx2.M10;
        float m20_2 = mtx2.M20;
        dst.M00 = (float)(m00_1 * (double)m00_2 + m01_1 * (double)m10_2 + m02_1 * (double)m20_2);
        dst.M10 = (float)(m10_1 * (double)m00_2 + m11_1 * (double)m10_2 + m12_1 * (double)m20_2);
        dst.M20 = (float)(m20_1 * (double)m00_2 + m21_1 * (double)m10_2 + m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)(m00_1 * (double)m01_2 + m01_1 * (double)m11_2 + m02_1 * (double)m21_2);
        dst.M11 = (float)(m10_1 * (double)m01_2 + m11_1 * (double)m11_2 + m12_1 * (double)m21_2);
        dst.M21 = (float)(m20_1 * (double)m01_2 + m21_1 * (double)m11_2 + m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)(m00_1 * (double)m02_2 + m01_1 * (double)m12_2 + m02_1 * (double)m22_2);
        dst.M12 = (float)(m10_1 * (double)m02_2 + m11_1 * (double)m12_2 + m12_1 * (double)m22_2);
        dst.M22 = (float)(m20_1 * (double)m02_2 + m21_1 * (double)m12_2 + m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)(m00_1 * (double)m03_2 + m01_1 * (double)m13_2 + m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)(m10_1 * (double)m03_2 + m11_1 * (double)m13_2 + m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)(m20_1 * (double)m03_2 + m21_1 * (double)m13_2 + m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      ref SNNS_MATRIX dst,
      NNS_MATRIX mtx1,
      ref SNNS_MATRIX mtx2)
    {
        float m00_1 = mtx1.M00;
        float m10_1 = mtx1.M10;
        float m20_1 = mtx1.M20;
        float m01_1 = mtx1.M01;
        float m11_1 = mtx1.M11;
        float m21_1 = mtx1.M21;
        float m02_1 = mtx1.M02;
        float m12_1 = mtx1.M12;
        float m22_1 = mtx1.M22;
        float m03_1 = mtx1.M03;
        float m13_1 = mtx1.M13;
        float m23_1 = mtx1.M23;
        float m00_2 = mtx2.M00;
        float m10_2 = mtx2.M10;
        float m20_2 = mtx2.M20;
        dst.M00 = (float)(m00_1 * (double)m00_2 + m01_1 * (double)m10_2 + m02_1 * (double)m20_2);
        dst.M10 = (float)(m10_1 * (double)m00_2 + m11_1 * (double)m10_2 + m12_1 * (double)m20_2);
        dst.M20 = (float)(m20_1 * (double)m00_2 + m21_1 * (double)m10_2 + m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)(m00_1 * (double)m01_2 + m01_1 * (double)m11_2 + m02_1 * (double)m21_2);
        dst.M11 = (float)(m10_1 * (double)m01_2 + m11_1 * (double)m11_2 + m12_1 * (double)m21_2);
        dst.M21 = (float)(m20_1 * (double)m01_2 + m21_1 * (double)m11_2 + m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)(m00_1 * (double)m02_2 + m01_1 * (double)m12_2 + m02_1 * (double)m22_2);
        dst.M12 = (float)(m10_1 * (double)m02_2 + m11_1 * (double)m12_2 + m12_1 * (double)m22_2);
        dst.M22 = (float)(m20_1 * (double)m02_2 + m21_1 * (double)m12_2 + m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)(m00_1 * (double)m03_2 + m01_1 * (double)m13_2 + m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)(m10_1 * (double)m03_2 + m11_1 * (double)m13_2 + m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)(m20_1 * (double)m03_2 + m21_1 * (double)m13_2 + m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      out SNNS_MATRIX dst,
      ref SNNS_MATRIX mtx1,
      ref SNNS_MATRIX mtx2)
    {
        float m00_1 = mtx1.M00;
        float m10_1 = mtx1.M10;
        float m20_1 = mtx1.M20;
        float m01_1 = mtx1.M01;
        float m11_1 = mtx1.M11;
        float m21_1 = mtx1.M21;
        float m02_1 = mtx1.M02;
        float m12_1 = mtx1.M12;
        float m22_1 = mtx1.M22;
        float m03_1 = mtx1.M03;
        float m13_1 = mtx1.M13;
        float m23_1 = mtx1.M23;
        float m00_2 = mtx2.M00;
        float m10_2 = mtx2.M10;
        float m20_2 = mtx2.M20;
        dst.M00 = (float)(m00_1 * (double)m00_2 + m01_1 * (double)m10_2 + m02_1 * (double)m20_2);
        dst.M10 = (float)(m10_1 * (double)m00_2 + m11_1 * (double)m10_2 + m12_1 * (double)m20_2);
        dst.M20 = (float)(m20_1 * (double)m00_2 + m21_1 * (double)m10_2 + m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)(m00_1 * (double)m01_2 + m01_1 * (double)m11_2 + m02_1 * (double)m21_2);
        dst.M11 = (float)(m10_1 * (double)m01_2 + m11_1 * (double)m11_2 + m12_1 * (double)m21_2);
        dst.M21 = (float)(m20_1 * (double)m01_2 + m21_1 * (double)m11_2 + m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)(m00_1 * (double)m02_2 + m01_1 * (double)m12_2 + m02_1 * (double)m22_2);
        dst.M12 = (float)(m10_1 * (double)m02_2 + m11_1 * (double)m12_2 + m12_1 * (double)m22_2);
        dst.M22 = (float)(m20_1 * (double)m02_2 + m21_1 * (double)m12_2 + m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)(m00_1 * (double)m03_2 + m01_1 * (double)m13_2 + m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)(m10_1 * (double)m03_2 + m11_1 * (double)m13_2 + m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)(m20_1 * (double)m03_2 + m21_1 * (double)m13_2 + m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      ref SNNS_MATRIX dst,
      NNS_MATRIX mtx1,
      NNS_MATRIX mtx2)
    {
        float m00_1 = mtx1.M00;
        float m10_1 = mtx1.M10;
        float m20_1 = mtx1.M20;
        float m01_1 = mtx1.M01;
        float m11_1 = mtx1.M11;
        float m21_1 = mtx1.M21;
        float m02_1 = mtx1.M02;
        float m12_1 = mtx1.M12;
        float m22_1 = mtx1.M22;
        float m03_1 = mtx1.M03;
        float m13_1 = mtx1.M13;
        float m23_1 = mtx1.M23;
        float m00_2 = mtx2.M00;
        float m10_2 = mtx2.M10;
        float m20_2 = mtx2.M20;
        dst.M00 = (float)(m00_1 * (double)m00_2 + m01_1 * (double)m10_2 + m02_1 * (double)m20_2);
        dst.M10 = (float)(m10_1 * (double)m00_2 + m11_1 * (double)m10_2 + m12_1 * (double)m20_2);
        dst.M20 = (float)(m20_1 * (double)m00_2 + m21_1 * (double)m10_2 + m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)(m00_1 * (double)m01_2 + m01_1 * (double)m11_2 + m02_1 * (double)m21_2);
        dst.M11 = (float)(m10_1 * (double)m01_2 + m11_1 * (double)m11_2 + m12_1 * (double)m21_2);
        dst.M21 = (float)(m20_1 * (double)m01_2 + m21_1 * (double)m11_2 + m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)(m00_1 * (double)m02_2 + m01_1 * (double)m12_2 + m02_1 * (double)m22_2);
        dst.M12 = (float)(m10_1 * (double)m02_2 + m11_1 * (double)m12_2 + m12_1 * (double)m22_2);
        dst.M22 = (float)(m20_1 * (double)m02_2 + m21_1 * (double)m12_2 + m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)(m00_1 * (double)m03_2 + m01_1 * (double)m13_2 + m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)(m10_1 * (double)m03_2 + m11_1 * (double)m13_2 + m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)(m20_1 * (double)m03_2 + m21_1 * (double)m13_2 + m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnTransposeMatrix(NNS_MATRIX dst, NNS_MATRIX src)
    {
        dst.M00 = src.M00;
        dst.M01 = src.M10;
        dst.M02 = src.M20;
        dst.M03 = 0.0f;
        dst.M10 = src.M01;
        dst.M11 = src.M11;
        dst.M12 = src.M21;
        dst.M13 = 0.0f;
        dst.M20 = src.M02;
        dst.M21 = src.M12;
        dst.M22 = src.M22;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnQuaternionMatrix(
      NNS_MATRIX dst,
      NNS_MATRIX src,
      ref NNS_QUATERNION quat)
    {
        SNNS_MATRIX dst1;
        nnMakeQuaternionMatrix(out dst1, ref quat);
        nnMultiplyMatrix(dst, src, ref dst1);
    }

    public static void nnRotateXMatrix(NNS_MATRIX dst, NNS_MATRIX src, int ax)
    {
        if (ax == 0)
        {
            if (dst == src)
                return;
            nnCopyMatrix(dst, src);
        }
        else
        {
            float s;
            float c;
            nnSinCos(ax, out s, out c);
            float num1 = s;
            float num2 = c;
            float m01 = src.M01;
            float m02 = src.M02;
            dst.M01 = (float)(m01 * (double)num2 + m02 * (double)num1);
            dst.M02 = (float)(m01 * -(double)num1 + m02 * (double)num2);
            float m11 = src.M11;
            float m12 = src.M12;
            dst.M11 = (float)(m11 * (double)num2 + m12 * (double)num1);
            dst.M12 = (float)(m11 * -(double)num1 + m12 * (double)num2);
            float m21 = src.M21;
            float m22 = src.M22;
            dst.M21 = (float)(m21 * (double)num2 + m22 * (double)num1);
            dst.M22 = (float)(m21 * -(double)num1 + m22 * (double)num2);
            if (dst == src)
                return;
            dst.M00 = src.M00;
            dst.M03 = src.M03;
            dst.M10 = src.M10;
            dst.M13 = src.M13;
            dst.M20 = src.M20;
            dst.M23 = src.M23;
            dst.M30 = 0.0f;
            dst.M31 = 0.0f;
            dst.M32 = 0.0f;
            dst.M33 = 1f;
        }
    }

    public static void nnRotateYMatrix(NNS_MATRIX dst, NNS_MATRIX src, int ay)
    {
        if (ay == 0)
        {
            if (dst == src)
                return;
            nnCopyMatrix(dst, src);
        }
        else
        {
            float s;
            float c;
            nnSinCos(ay, out s, out c);
            float num1 = s;
            float num2 = c;
            float m00 = src.M00;
            float m02 = src.M02;
            dst.M00 = (float)(m00 * (double)num2 + m02 * -(double)num1);
            dst.M02 = (float)(m00 * (double)num1 + m02 * (double)num2);
            float m10 = src.M10;
            float m12 = src.M12;
            dst.M10 = (float)(m10 * (double)num2 + m12 * -(double)num1);
            dst.M12 = (float)(m10 * (double)num1 + m12 * (double)num2);
            float m20 = src.M20;
            float m22 = src.M22;
            dst.M20 = (float)(m20 * (double)num2 + m22 * -(double)num1);
            dst.M22 = (float)(m20 * (double)num1 + m22 * (double)num2);
            if (dst == src)
                return;
            dst.M01 = src.M01;
            dst.M03 = src.M03;
            dst.M11 = src.M11;
            dst.M13 = src.M13;
            dst.M21 = src.M21;
            dst.M23 = src.M23;
            dst.M30 = 0.0f;
            dst.M31 = 0.0f;
            dst.M32 = 0.0f;
            dst.M33 = 1f;
        }
    }

    public static void nnRotateYMatrix(
      ref SNNS_MATRIX dst,
      ref SNNS_MATRIX src,
      int ay)
    {
        if (ay == 0)
            return;
        float s;
        float c;
        nnSinCos(ay, out s, out c);
        float num1 = s;
        float num2 = c;
        float m00 = src.M00;
        float m02 = src.M02;
        dst.M00 = (float)(m00 * (double)num2 + m02 * -(double)num1);
        dst.M02 = (float)(m00 * (double)num1 + m02 * (double)num2);
        float m10 = src.M10;
        float m12 = src.M12;
        dst.M10 = (float)(m10 * (double)num2 + m12 * -(double)num1);
        dst.M12 = (float)(m10 * (double)num1 + m12 * (double)num2);
        float m20 = src.M20;
        float m22 = src.M22;
        dst.M20 = (float)(m20 * (double)num2 + m22 * -(double)num1);
        dst.M22 = (float)(m20 * (double)num1 + m22 * (double)num2);
        dst.M01 = src.M01;
        dst.M03 = src.M03;
        dst.M11 = src.M11;
        dst.M13 = src.M13;
        dst.M21 = src.M21;
        dst.M23 = src.M23;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnRotateZMatrix(
      ref SNNS_MATRIX dst,
      ref SNNS_MATRIX src,
      int az)
    {
        if (az == 0)
        {
            dst = src;
        }
        else
        {
            float s;
            float c;
            nnSinCos(az, out s, out c);
            float num1 = s;
            float num2 = c;
            float m00 = src.M00;
            float m01 = src.M01;
            dst.M00 = (float)(m00 * (double)num2 + m01 * (double)num1);
            dst.M01 = (float)(m00 * -(double)num1 + m01 * (double)num2);
            float m10 = src.M10;
            float m11 = src.M11;
            dst.M10 = (float)(m10 * (double)num2 + m11 * (double)num1);
            dst.M11 = (float)(m10 * -(double)num1 + m11 * (double)num2);
            float m20 = src.M20;
            float m21 = src.M21;
            dst.M20 = (float)(m20 * (double)num2 + m21 * (double)num1);
            dst.M21 = (float)(m20 * -(double)num1 + m21 * (double)num2);
            dst.M02 = src.M02;
            dst.M03 = src.M03;
            dst.M12 = src.M12;
            dst.M13 = src.M13;
            dst.M22 = src.M22;
            dst.M23 = src.M23;
            dst.M30 = 0.0f;
            dst.M31 = 0.0f;
            dst.M32 = 0.0f;
            dst.M33 = 1f;
        }
    }

    public static void nnRotateZMatrix(NNS_MATRIX dst, NNS_MATRIX src, int az)
    {
        if (az == 0)
        {
            if (dst == src)
                return;
            dst.Assign(src);
        }
        else
        {
            float s;
            float c;
            nnSinCos(az, out s, out c);
            float num1 = s;
            float num2 = c;
            float m00 = src.M00;
            float m01 = src.M01;
            dst.M00 = (float)(m00 * (double)num2 + m01 * (double)num1);
            dst.M01 = (float)(m00 * -(double)num1 + m01 * (double)num2);
            float m10 = src.M10;
            float m11 = src.M11;
            dst.M10 = (float)(m10 * (double)num2 + m11 * (double)num1);
            dst.M11 = (float)(m10 * -(double)num1 + m11 * (double)num2);
            float m20 = src.M20;
            float m21 = src.M21;
            dst.M20 = (float)(m20 * (double)num2 + m21 * (double)num1);
            dst.M21 = (float)(m20 * -(double)num1 + m21 * (double)num2);
            if (dst == src)
                return;
            dst.M02 = src.M02;
            dst.M03 = src.M03;
            dst.M12 = src.M12;
            dst.M13 = src.M13;
            dst.M22 = src.M22;
            dst.M23 = src.M23;
            dst.M30 = 0.0f;
            dst.M31 = 0.0f;
            dst.M32 = 0.0f;
            dst.M33 = 1f;
        }
    }

    public static void nnRotateXYZMatrix(
      NNS_MATRIX dst,
      NNS_MATRIX src,
      int ax,
      int ay,
      int az)
    {
        if (az != 0 || dst != src)
            nnRotateZMatrix(dst, src, az);
        if (ay != 0)
            nnRotateYMatrix(dst, dst, ay);
        if (ax == 0)
            return;
        nnRotateXMatrix(dst, dst, ax);
    }

    public static void nnRotateXZYMatrix(
      NNS_MATRIX dst,
      NNS_MATRIX src,
      int ax,
      int ay,
      int az)
    {
        if (ay != 0 || dst != src)
            nnRotateYMatrix(dst, src, ay);
        if (az != 0)
            nnRotateZMatrix(dst, dst, az);
        if (ax == 0)
            return;
        nnRotateXMatrix(dst, dst, ax);
    }

    public static void nnRotateZXYMatrix(
      NNS_MATRIX dst,
      NNS_MATRIX src,
      int ax,
      int ay,
      int az)
    {
        if (ay != 0 || dst != src)
            nnRotateYMatrix(dst, src, ay);
        if (ax != 0)
            nnRotateXMatrix(dst, dst, ax);
        if (az == 0)
            return;
        nnRotateZMatrix(dst, dst, az);
    }

    public static void nnScaleMatrix(
      ref SNNS_MATRIX dst,
      ref SNNS_MATRIX src,
      float x,
      float y,
      float z)
    {
        dst.M03 = src.M03;
        dst.M13 = src.M13;
        dst.M23 = src.M23;
        dst.M33 = 1f;
        dst.M00 = src.M00 * x;
        dst.M10 = src.M10 * x;
        dst.M20 = src.M20 * x;
        dst.M30 = 0.0f;
        dst.M01 = src.M01 * y;
        dst.M11 = src.M11 * y;
        dst.M21 = src.M21 * y;
        dst.M31 = 0.0f;
        dst.M02 = src.M02 * z;
        dst.M12 = src.M12 * z;
        dst.M22 = src.M22 * z;
        dst.M32 = 0.0f;
    }

    public static void nnScaleMatrix(
      NNS_MATRIX dst,
      NNS_MATRIX src,
      float x,
      float y,
      float z)
    {
        if (dst != src)
        {
            dst.M03 = src.M03;
            dst.M13 = src.M13;
            dst.M23 = src.M23;
            dst.M33 = 1f;
        }
        dst.M00 = src.M00 * x;
        dst.M10 = src.M10 * x;
        dst.M20 = src.M20 * x;
        dst.M30 = 0.0f;
        dst.M01 = src.M01 * y;
        dst.M11 = src.M11 * y;
        dst.M21 = src.M21 * y;
        dst.M31 = 0.0f;
        dst.M02 = src.M02 * z;
        dst.M12 = src.M12 * z;
        dst.M22 = src.M22 * z;
        dst.M32 = 0.0f;
    }

    public static void nnTranslateMatrix(
      ref SNNS_MATRIX dst,
      ref SNNS_MATRIX src,
      float x,
      float y,
      float z)
    {
        nnCopyMatrix33(ref dst, ref src);
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M03 = (float)(src.M00 * (double)x + src.M01 * (double)y + src.M02 * (double)z) + src.M03;
        dst.M13 = (float)(src.M10 * (double)x + src.M11 * (double)y + src.M12 * (double)z) + src.M13;
        dst.M23 = (float)(src.M20 * (double)x + src.M21 * (double)y + src.M22 * (double)z) + src.M23;
        dst.M33 = 1f;
    }

    public static void nnTranslateMatrix(
      NNS_MATRIX dst,
      NNS_MATRIX src,
      float x,
      float y,
      float z)
    {
        if (dst != src)
        {
            nnCopyMatrix33(dst, src);
            dst.M30 = 0.0f;
            dst.M31 = 0.0f;
            dst.M32 = 0.0f;
        }
        dst.M03 = (float)(src.M00 * (double)x + src.M01 * (double)y + src.M02 * (double)z) + src.M03;
        dst.M13 = (float)(src.M10 * (double)x + src.M11 * (double)y + src.M12 * (double)z) + src.M13;
        dst.M23 = (float)(src.M20 * (double)x + src.M21 * (double)y + src.M22 * (double)z) + src.M23;
        dst.M33 = 1f;
    }

    public static void nnCopyVectorMatrixTranslation(
      NNS_MATRIX mtx,
      ref SNNS_VECTOR vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(
      ref SNNS_MATRIX mtx,
      ref SNNS_VECTOR vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(
      ref SNNS_MATRIX mtx,
      NNS_VECTOR vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(
      ref SNNS_MATRIX mtx,
      NNS_VECTOR4D vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(
      ref SNNS_MATRIX mtx,
      ref SNNS_VECTOR4D vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(NNS_MATRIX mtx, NNS_VECTOR vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(NNS_MATRIX mtx, NNS_VECTOR4D vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    private static void nnCopyVectorFastMatrixTranslation(
      NNS_MATRIX mtx,
      ref NNS_VECTORFAST vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    private int nnCheckCollisionSC(ref NNS_SPHERE sphere, ref NNS_CAPSULE capsule)
    {
        mppAssertNotImpl();
        return 0;
    }

    private int nnCheckCollisionBC(ref NNS_BOX box, ref NNS_CAPSULE capsule)
    {
        mppAssertNotImpl();
        return 0;
    }
    private int nnCalcMorphMotionWeight(
      ref NNS_SUBMOTION submot,
      float frame,
      ref float weight)
    {
        mppAssertNotImpl();
        return 0;
    }

    private void nnCalcMorphMotion(
      float[] mwpal,
      ref NNS_MORPHTARGETLIST mtgt,
      ref NNS_MOTION mot,
      float frame)
    {
        mppAssertNotImpl();
    }

    private void nnBlendMorphWeightPalette(
      float[] dstmwpal,
      float[] srcmwpal0,
      float ratio0,
      float[] srcmwpal1,
      float ratio1,
      ref NNS_MORPHTARGETLIST mtgt)
    {
        mppAssertNotImpl();
    }

    public static void nnCopyQuaternion(
      ref NNS_QUATERNION dst,
      ref NNS_QUATERNION src)
    {
        dst = src;
    }

    public static void nnMultiplyQuaternion(
      ref NNS_QUATERNION dst,
      ref NNS_QUATERNION quat1,
      ref NNS_QUATERNION quat2)
    {
        float x1 = quat1.x;
        float y1 = quat1.y;
        float z1 = quat1.z;
        float w1 = quat1.w;
        float x2 = quat2.x;
        float y2 = quat2.y;
        float z2 = quat2.z;
        float w2 = quat2.w;
        dst.x = (float)(w1 * (double)x2 + x1 * (double)w2 + y1 * (double)z2 - z1 * (double)y2);
        dst.y = (float)(w1 * (double)y2 - x1 * (double)z2 + y1 * (double)w2 + z1 * (double)x2);
        dst.z = (float)(w1 * (double)z2 + x1 * (double)y2 - y1 * (double)x2 + z1 * (double)w2);
        dst.w = (float)(w1 * (double)w2 - x1 * (double)x2 - y1 * (double)y2 - z1 * (double)z2);
    }

    public static int nnNormalizeQuaternion(
      ref NNS_QUATERNION dst,
      ref NNS_QUATERNION src)
    {
        float n = (float)(src.x * (double)src.x + src.y * (double)src.y + src.z * (double)src.z + src.w * (double)src.w);
        if (n == 0.0)
        {
            dst.x = 0.0f;
            dst.y = 0.0f;
            dst.z = 0.0f;
            dst.w = 0.0f;
            return 0;
        }
        float num = nnInvertSqrt(n);
        dst.x = src.x * num;
        dst.y = src.y * num;
        dst.z = src.z * num;
        dst.w = src.w * num;
        return 1;
    }

    public static int nnInvertQuaternion(
      ref NNS_QUATERNION dst,
      ref NNS_QUATERNION src)
    {
        float num1 = (float)(src.x * (double)src.x + src.y * (double)src.y + src.z * (double)src.z + src.w * (double)src.w);
        if (num1 == 0.0)
        {
            dst.x = 0.0f;
            dst.y = 0.0f;
            dst.z = 0.0f;
            dst.w = 0.0f;
            return 0;
        }
        float num2 = 1f / num1;
        dst.x = -src.x * num2;
        dst.y = -src.y * num2;
        dst.z = -src.z * num2;
        dst.w = src.w * num2;
        return 1;
    }

    private static void nnLogQuaternion(
      ref NNS_QUATERNION dst,
      ref NNS_QUATERNION src)
    {
        int num1 = nnArcCos(src.w);
        float num2 = nnSin(num1);
        if (num2 > 0.0)
        {
            float num3 = 1f / num2;
            dst.x = NNM_A32toRAD(num1) * src.x * num3;
            dst.y = NNM_A32toRAD(num1) * src.y * num3;
            dst.z = NNM_A32toRAD(num1) * src.z * num3;
            dst.w = 0.0f;
        }
        else
        {
            dst.x = 0.0f;
            dst.y = 0.0f;
            dst.z = 0.0f;
            dst.w = 0.0f;
        }
    }

    public static void nnExpQuaternion(ref NNS_QUATERNION dst, ref NNS_QUATERNION src)
    {
        float n = nnSqrt((float)(src.x * (double)src.x + src.y * (double)src.y + src.z * (double)src.z));
        float s;
        float c;
        nnSinCos(NNM_RADtoA32(n), out s, out c);
        if (n > 0.0)
        {
            float num = 1f / n;
            dst.x = s * src.x * num;
            dst.y = s * src.y * num;
            dst.z = s * src.z * num;
            dst.w = c;
        }
        else
        {
            dst.x = 0.0f;
            dst.y = 0.0f;
            dst.z = 0.0f;
            dst.w = c;
        }
    }

    private static void nnSplineQuaternion(
      ref NNS_QUATERNION dst,
      ref NNS_QUATERNION quatprev,
      ref NNS_QUATERNION quat,
      ref NNS_QUATERNION quatnext)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion3 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion4 = new NNS_QUATERNION();
        nnInvertQuaternion(ref nnsQuaternion2, ref quat);
        nnMultiplyQuaternion(ref nnsQuaternion3, ref nnsQuaternion2, ref quatprev);
        nnMultiplyQuaternion(ref nnsQuaternion4, ref nnsQuaternion2, ref quatnext);
        nnLogQuaternion(ref nnsQuaternion3, ref nnsQuaternion3);
        nnLogQuaternion(ref nnsQuaternion4, ref nnsQuaternion4);
        nnsQuaternion1.x = (float)((nnsQuaternion3.x + (double)nnsQuaternion4.x) / -4.0);
        nnsQuaternion1.y = (float)((nnsQuaternion3.y + (double)nnsQuaternion4.y) / -4.0);
        nnsQuaternion1.z = (float)((nnsQuaternion3.z + (double)nnsQuaternion4.z) / -4.0);
        nnsQuaternion1.w = (float)((nnsQuaternion3.w + (double)nnsQuaternion4.w) / -4.0);
        nnExpQuaternion(ref nnsQuaternion1, ref nnsQuaternion1);
        nnMultiplyQuaternion(ref dst, ref quat, ref nnsQuaternion1);
    }

    private static void nnLerpQuaternion(
      ref NNS_QUATERNION dst,
      ref NNS_QUATERNION quat1,
      ref NNS_QUATERNION quat2,
      float t)
    {
        NNS_QUATERNION src = new NNS_QUATERNION();
        float num = 1f - t;
        if (quat1.x * (double)quat2.x + quat1.y * (double)quat2.y + quat1.z * (double)quat2.z + quat1.w * (double)quat2.w > 0.0)
        {
            src.x = (float)(quat1.x * (double)num + quat2.x * (double)t);
            src.y = (float)(quat1.y * (double)num + quat2.y * (double)t);
            src.z = (float)(quat1.z * (double)num + quat2.z * (double)t);
            src.w = (float)(quat1.w * (double)num + quat2.w * (double)t);
        }
        else
        {
            src.x = (float)(quat1.x * (double)num - quat2.x * (double)t);
            src.y = (float)(quat1.y * (double)num - quat2.y * (double)t);
            src.z = (float)(quat1.z * (double)num - quat2.z * (double)t);
            src.w = (float)(quat1.w * (double)num - quat2.w * (double)t);
        }
        nnNormalizeQuaternion(ref dst, ref src);
    }

    private static void nnSlerpQuaternion(
      out NNS_QUATERNION dst,
      ref NNS_QUATERNION quat1,
      ref NNS_QUATERNION quat2,
      float t)
    {
        float num1 = 1f;
        float num2 = 1f - t;
        float num3 = (float)(quat1.x * (double)quat2.x + quat1.y * (double)quat2.y + quat1.z * (double)quat2.z + quat1.w * (double)quat2.w);
        if (num3 < 0.0)
        {
            num3 = -num3;
            num1 = -num1;
        }
        float num4;
        float num5;
        if (num3 < 0.999989986419678)
        {
            float num6 = (float)(2.0 - 2.82842707633972 * nnInvertSqrt(1f + num3)) * t;
            float num7 = num2 * (1f - num6);
            float num8 = -num6 * num2 + t;
            float num9 = nnInvertSqrt((float)(num7 * (double)num7 + num8 * (double)num8 + 2.0 * num7 * num8 * num3));
            num4 = num7 * num9;
            num5 = num8 * (num9 * num1);
        }
        else
        {
            num4 = num2;
            num5 = t * num1;
        }
        dst.x = (float)(quat1.x * (double)num4 + quat2.x * (double)num5);
        dst.y = (float)(quat1.y * (double)num4 + quat2.y * (double)num5);
        dst.z = (float)(quat1.z * (double)num4 + quat2.z * (double)num5);
        dst.w = (float)(quat1.w * (double)num4 + quat2.w * (double)num5);
    }

    private void nnSlerpNoInvQuaternion(
      ref NNS_QUATERNION dst,
      ref NNS_QUATERNION q1,
      ref NNS_QUATERNION q2,
      float t)
    {
        float num1 = (float)(q1.x * (double)q2.x + q1.y * (double)q2.y + q1.z * (double)q2.z + q1.w * (double)q2.w);
        float num2;
        float num3;
        if (num1 >= -0.980000019073486 && num1 <= 0.980000019073486)
        {
            int ang = nnArcCos(num1);
            float num4 = 1f / nnSin(ang);
            num2 = nnSin((int)(ang * (1.0 - t))) * num4;
            num3 = nnSin((int)(ang * (double)t)) * num4;
        }
        else
        {
            num2 = 1f - t;
            num3 = t;
        }
        dst.x = (float)(q1.x * (double)num2 + q2.x * (double)num3);
        dst.y = (float)(q1.y * (double)num2 + q2.y * (double)num3);
        dst.z = (float)(q1.z * (double)num2 + q2.z * (double)num3);
        dst.w = (float)(q1.w * (double)num2 + q2.w * (double)num3);
    }

    private static void nnSquadQuaternion(
      ref NNS_QUATERNION dst,
      ref NNS_QUATERNION quat1,
      ref NNS_QUATERNION quata,
      ref NNS_QUATERNION quatb,
      ref NNS_QUATERNION quat2,
      float t)
    {
        NNS_QUATERNION dst1 = new NNS_QUATERNION();
        NNS_QUATERNION dst2 = new NNS_QUATERNION();
        float t1 = (float)(2.0 * t * (1.0 - t));
        nnSlerpQuaternion(out dst1, ref quat1, ref quat2, t);
        nnSlerpQuaternion(out dst2, ref quata, ref quatb, t);
        nnSlerpQuaternion(out dst, ref dst1, ref dst2, t1);
    }

    public static void nnMakeUnitQuaternion(ref NNS_QUATERNION dst)
    {
        dst.x = 0.0f;
        dst.y = 0.0f;
        dst.z = 0.0f;
        dst.w = 1f;
    }

    private static void nnMakeRotateAxisQuaternion(
      out NNS_QUATERNION dst,
      float vx,
      float vy,
      float vz,
      int ang)
    {
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.x = vx;
        nnsVector.y = vy;
        nnsVector.z = vz;
        nnNormalizeVector(nnsVector, nnsVector);
        ang >>= 1;
        float s;
        float c;
        nnSinCos(ang, out s, out c);
        dst.x = nnsVector.x * s;
        dst.y = nnsVector.y * s;
        dst.z = nnsVector.z * s;
        dst.w = c;
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    public static void nnMakeRotateMatrixQuaternion(
      out NNS_QUATERNION dst,
      NNS_MATRIX mtx)
    {
        int[] nxt = _nnMakeRotateMatrixQuaternion.nxt;
        nxt[0] = 1;
        nxt[1] = 2;
        nxt[2] = 0;
        float[] q = _nnMakeRotateMatrixQuaternion.q;
        float num1 = mtx.M00 + mtx.M11 + mtx.M22;
        if (num1 > 0.0)
        {
            float num2 = nnSqrt(num1 + 1f);
            dst.w = num2 * 0.5f;
            float num3 = 0.5f / num2;
            dst.x = (mtx.M21 - mtx.M12) * num3;
            dst.y = (mtx.M02 - mtx.M20) * num3;
            dst.z = (mtx.M10 - mtx.M01) * num3;
        }
        else
        {
            int index1 = 0;
            if (mtx.M11 > (double)mtx.M00)
                index1 = 1;
            if (mtx.M22 > (double)mtx.M(index1, index1))
                index1 = 2;
            int index2 = nxt[index1];
            int index3 = nxt[index2];
            float num2 = nnSqrt((float)(mtx.M(index1, index1) - (mtx.M(index2, index2) + (double)mtx.M(index3, index3)) + 1.0));
            q[index1] = num2 * 0.5f;
            if (num2 != 0.0)
                num2 = 0.5f / num2;
            dst.w = (mtx.M(index3, index2) - mtx.M(index2, index3)) * num2;
            q[index2] = (mtx.M(index1, index2) + mtx.M(index2, index1)) * num2;
            q[index3] = (mtx.M(index1, index3) + mtx.M(index3, index1)) * num2;
            dst.x = q[0];
            dst.y = q[1];
            dst.z = q[2];
        }
    }

    private static void nnMakeRotateXYZQuaternion(
      out NNS_QUATERNION dst,
      int rx,
      int ry,
      int rz)
    {
        float num1;
        float num2;
        float s;
        float c;
        if (rx == 0)
        {
            num1 = 0.0f;
            num2 = 1f;
        }
        else
        {
            nnSinCos(rx >> 1, out s, out c);
            num1 = s;
            num2 = c;
        }
        float num3;
        float num4;
        if (ry == 0)
        {
            num3 = 0.0f;
            num4 = 1f;
        }
        else
        {
            nnSinCos(ry >> 1, out s, out c);
            num3 = s;
            num4 = c;
        }
        float num5;
        float num6;
        if (rz == 0)
        {
            num5 = 0.0f;
            num6 = 1f;
        }
        else
        {
            nnSinCos(rz >> 1, out s, out c);
            num5 = s;
            num6 = c;
        }
        float num7 = num6 * num4;
        float num8 = num6 * num3;
        float num9 = num5 * num3;
        float num10 = num5 * num4;
        dst.x = (float)(num7 * (double)num1 - num9 * (double)num2);
        dst.y = (float)(num8 * (double)num2 + num10 * (double)num1);
        dst.z = (float)(-num8 * (double)num1 + num10 * (double)num2);
        dst.w = (float)(num7 * (double)num2 + num9 * (double)num1);
    }

    public static void nnMakeRotateXZYQuaternion(
      out NNS_QUATERNION dst,
      int rx,
      int ry,
      int rz)
    {
        float num1;
        float num2;
        float s;
        float c;
        if (rx == 0)
        {
            num1 = 0.0f;
            num2 = 1f;
        }
        else
        {
            nnSinCos(rx >> 1, out s, out c);
            num1 = s;
            num2 = c;
        }
        float num3;
        float num4;
        if (ry == 0)
        {
            num3 = 0.0f;
            num4 = 1f;
        }
        else
        {
            nnSinCos(ry >> 1, out s, out c);
            num3 = s;
            num4 = c;
        }
        float num5;
        float num6;
        if (rz == 0)
        {
            num5 = 0.0f;
            num6 = 1f;
        }
        else
        {
            nnSinCos(rz >> 1, out s, out c);
            num5 = s;
            num6 = c;
        }
        float num7 = num4 * num6;
        float num8 = num3 * num6;
        float num9 = num3 * num5;
        float num10 = num4 * num5;
        dst.x = (float)(num7 * (double)num1 + num9 * (double)num2);
        dst.y = (float)(num8 * (double)num2 + num10 * (double)num1);
        dst.z = (float)(-num8 * (double)num1 + num10 * (double)num2);
        dst.w = (float)(num7 * (double)num2 - num9 * (double)num1);
    }

    public static void nnMakeRotateZXYQuaternion(
      out NNS_QUATERNION dst,
      int rx,
      int ry,
      int rz)
    {
        float num1;
        float num2;
        float s;
        float c;
        if (rx == 0)
        {
            num1 = 0.0f;
            num2 = 1f;
        }
        else
        {
            nnSinCos(rx >> 1, out s, out c);
            num1 = s;
            num2 = c;
        }
        float num3;
        float num4;
        if (ry == 0)
        {
            num3 = 0.0f;
            num4 = 1f;
        }
        else
        {
            nnSinCos(ry >> 1, out s, out c);
            num3 = s;
            num4 = c;
        }
        float num5;
        float num6;
        if (rz == 0)
        {
            num5 = 0.0f;
            num6 = 1f;
        }
        else
        {
            nnSinCos(rz >> 1, out s, out c);
            num5 = s;
            num6 = c;
        }
        dst.x = (float)(num4 * (double)num1 * num6 + num3 * (double)num2 * num5);
        dst.y = (float)(-num4 * (double)num1 * num5 + num3 * (double)num2 * num6);
        dst.z = (float)(num4 * (double)num2 * num5 - num3 * (double)num1 * num6);
        dst.w = (float)(num4 * (double)num2 * num6 + num3 * (double)num1 * num5);
    }

    private void nnCalcMatrixList(
      NNS_MATRIX mtxlist,
      NNS_OBJECT obj,
      NNS_MATRIX basemtx)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotionNode(
      NNS_MATRIX mtxlist,
      NNS_OBJECT obj,
      NNS_MATRIX basemtx,
      NNS_MOTION mot,
      float frame)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotion(
      NNS_MATRIX mtxlist,
      NNS_OBJECT obj,
      NNS_MOTION mot,
      float frame,
      NNS_MATRIX basemtx)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixListTRSList(
      NNS_MATRIX mtxlist,
      NNS_OBJECT obj,
      NNS_TRS trslist,
      NNS_MATRIX basemtx)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixListMultiplyMatrix(
      ArrayPointer<NNS_MATRIX> dstlist,
      NNS_MATRIX src,
      ArrayPointer<NNS_MATRIX> srclist,
      int num)
    {
        this.nnCalcMultiplyMatrices(dstlist, src, srclist, num);
    }

    private void nnCalcMatrixPaletteMatrixList(
      NNS_MATRIX mtxpal,
      NNS_OBJECT obj,
      NNS_MATRIX mtxlist,
      NNS_MATRIX basemtx)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotionNode1BoneSIIK(
      NNS_MATRIX mtxlist,
      NNS_OBJECT obj,
      NNS_MATRIX basemtx,
      int jnt1idx,
      int submotidx,
      NNS_MOTION mot,
      float frame)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotionNode2BoneSIIK(
      NNS_MATRIX mtxlist,
      NNS_OBJECT obj,
      NNS_MATRIX basemtx,
      int jnt1idx,
      int submotidx,
      NNS_MOTION mot,
      float frame)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotionNode1BoneXSIIK(
      NNS_MATRIX mtxlist,
      NNS_OBJECT obj,
      NNS_MATRIX basemtx,
      int rootidx,
      int submotidx,
      NNS_MOTION mot,
      float frame)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotionNode2BoneXSIIK(
      NNS_MATRIX mtxlist,
      NNS_OBJECT obj,
      NNS_MATRIX basemtx,
      int rootidx,
      int submotidx,
      NNS_MOTION mot,
      float frame)
    {
        mppAssertNotImpl();
    }

    private void nnConvertPosition4sTo3f(float[] dst, short[] src, int nVertex)
    {
        int num1 = 0;
        int index1 = 0;
        for (int index2 = 0; index2 < nVertex; ++index2)
        {
            float[] numArray1 = dst;
            int index3 = num1;
            int num2 = index3 + 1;
            double num3 = src[index1] / (double)src[index1 + 3];
            numArray1[index3] = (float)num3;
            float[] numArray2 = dst;
            int index4 = num2;
            int num4 = index4 + 1;
            double num5 = src[index1 + 1] / (double)src[index1 + 3];
            numArray2[index4] = (float)num5;
            float[] numArray3 = dst;
            int index5 = num4;
            num1 = index5 + 1;
            double num6 = src[index1 + 2] / (double)src[index1 + 3];
            numArray3[index5] = (float)num6;
            index1 += 4;
        }
    }

    private void nnConvertNormal3bTo3f(float[] dst, sbyte[] src, int nVertex)
    {
        int num1 = 0;
        int num2 = 0;
        for (int index1 = 0; index1 < nVertex; ++index1)
        {
            float[] numArray1 = dst;
            int index2 = num1;
            int num3 = index2 + 1;
            sbyte[] numArray2 = src;
            int index3 = num2;
            int num4 = index3 + 1;
            double num5 = (numArray2[index3] + 0.5) * 0.00784313771873713;
            numArray1[index2] = (float)num5;
            float[] numArray3 = dst;
            int index4 = num3;
            int num6 = index4 + 1;
            sbyte[] numArray4 = src;
            int index5 = num4;
            int num7 = index5 + 1;
            double num8 = (numArray4[index5] + 0.5) * 0.00784313771873713;
            numArray3[index4] = (float)num8;
            float[] numArray5 = dst;
            int index6 = num6;
            num1 = index6 + 1;
            sbyte[] numArray6 = src;
            int index7 = num7;
            num2 = index7 + 1;
            double num9 = (numArray6[index7] + 0.5) * 0.00784313771873713;
            numArray5[index6] = (float)num9;
        }
    }

    private void nnConvertNormal3sTo3f(float[] dst, short[] src, int nVertex)
    {
        int num1 = 0;
        int num2 = 0;
        for (int index1 = 0; index1 < nVertex; ++index1)
        {
            float[] numArray1 = dst;
            int index2 = num1;
            int num3 = index2 + 1;
            short[] numArray2 = src;
            int index3 = num2;
            int num4 = index3 + 1;
            double num5 = (numArray2[index3] + 0.5) * 3.05180437862873E-05;
            numArray1[index2] = (float)num5;
            float[] numArray3 = dst;
            int index4 = num3;
            int num6 = index4 + 1;
            short[] numArray4 = src;
            int index5 = num4;
            int num7 = index5 + 1;
            double num8 = (numArray4[index5] + 0.5) * 3.05180437862873E-05;
            numArray3[index4] = (float)num8;
            float[] numArray5 = dst;
            int index6 = num6;
            num1 = index6 + 1;
            short[] numArray6 = src;
            int index7 = num7;
            num2 = index7 + 1;
            double num9 = (numArray6[index7] + 0.5) * 3.05180437862873E-05;
            numArray5[index6] = (float)num9;
        }
    }

    private static void nnCalcMorphPositon(
      float[] pPosBuf,
      NNS_VTXARRAY_GL[] pMorphArray,
      NNS_VTXARRAY_GL[] pObjArray,
      NNS_VTXARRAY_GL[] pMtgtArray,
      int nVertex,
      float weight)
    {
        mppAssertNotImpl();
    }

    private static void nnCalcMorphNormal(
      float[] pNrmBuf,
      NNS_VTXARRAY_GL[] pMorphArray,
      NNS_VTXARRAY_GL[] pObjArray,
      NNS_VTXARRAY_GL[] pMtgtArray,
      int nVertex,
      float weight)
    {
        mppAssertNotImpl();
    }

    private static void nnCalcMorphGeneral(
      float[] pBuf,
      NNS_VTXARRAY_GL[] pMorphArray,
      NNS_VTXARRAY_GL[] pObjArray,
      NNS_VTXARRAY_GL[] pMtgtArray,
      int nVertex,
      float weight)
    {
        mppAssertNotImpl();
    }

    private static void nnNormalizeNormalArray(
      float[] pNrmBuf,
      NNS_VTXARRAY_GL[] pNrmArray,
      int nVertex)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMorphObject(
      ref NNS_OBJECT mobj,
      ref NNS_OBJECT obj,
      ref NNS_MORPHTARGETLIST mtgt,
      float[] mwpal,
      uint flag)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMotionLightScalar(NNS_SUBMOTION submot, float frame, ref float val)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMotionLightAngle(NNS_SUBMOTION submot, float frame, ref int ang)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMotionLightXYZ(
      NNS_SUBMOTION submot,
      float frame,
      NNS_VECTOR xyz)
    {
        mppAssertNotImpl();
    }

    private void nnCalcMotionLightRGB(NNS_SUBMOTION submot, float frame, NNS_RGB col)
    {
        mppAssertNotImpl();
    }

    private void nnCalcLightMotionCore(
      NNS_LIGHTPTR dstptr,
      NNS_LIGHTPTR litptr,
      NNS_MOTION mot,
      float frame)
    {
        mppAssertNotImpl();
    }

    private void nnCalcLightMotion(
      NNS_LIGHTPTR dstptr,
      NNS_LIGHTPTR litptr,
      NNS_MOTION mot,
      float frame)
    {
        mppAssertNotImpl();
    }

    private static int nnCalcMotionNodeHide(NNS_SUBMOTION submot, float frame)
    {
        int val = 0;
        NNS_MOTION_KEY_Class11[] pKeyList = (NNS_MOTION_KEY_Class11[])submot.pKeyList;
        int nKeyFrame = submot.nKeyFrame;
        if ((submot.fIPType & 3703U) == 4U)
            nnInterpolateConstantS32_1(pKeyList, nKeyFrame, frame, out val);
        return val;
    }

    private static void nnCalcNodeHideMotion(
      ArrayPointer<uint> nodestatlist,
      NNS_MOTION mot,
      float frame)
    {
        float dstframe1;
        if (((int)mot.fType & 1) == 0 || nnCalcMotionFrame(out dstframe1, mot.fType, mot.StartFrame, mot.EndFrame, frame) == 0)
            return;
        ArrayPointer<NNS_SUBMOTION> arrayPointer = new ArrayPointer<NNS_SUBMOTION>(mot.pSubmotion);
        for (int index = 0; index < mot.nSubmotion; ++index)
        {
            NNS_SUBMOTION submot = arrayPointer + index;
            if (((int)submot.fType & 1048576) != 0 && submot.StartFrame <= (double)dstframe1 && dstframe1 <= (double)submot.EndFrame)
            {
                uint fType = submot.fIPType;
                if (((int)mot.fType & 131072) != 0 && dstframe1 == (double)submot.EndFrame)
                    fType = 131072U;
                float dstframe2;
                if (nnCalcMotionFrame(out dstframe2, fType, submot.StartKeyFrame, submot.EndKeyFrame, dstframe1) != 0 && nnCalcMotionNodeHide(submot, dstframe2) != 0)
                {
                    int num = (int)(nodestatlist + submot.Id).SetPrimitive((nodestatlist + submot.Id)[0] | 1U);
                }
            }
        }
    }

    private static void nnInterpolateConstantF1(
      NNS_MOTION_KEY_Class1[] vk,
      int nKey,
      float frame,
      out float val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class1 nnsMotionKeyClass1 = vk[(int)num1];
        val = nnsMotionKeyClass1.Value;
    }

    private static void nnInterpolateConstantF3(
      NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      NNS_VECTOR val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class5 nnsMotionKeyClass5 = vk[(int)num1];
        val.x = nnsMotionKeyClass5.Value.x;
        val.y = nnsMotionKeyClass5.Value.y;
        val.z = nnsMotionKeyClass5.Value.z;
    }

    private static void nnInterpolateConstantF3(
      NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      out SNNS_VECTOR val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class5 nnsMotionKeyClass5 = vk[(int)num1];
        val.x = nnsMotionKeyClass5.Value.x;
        val.y = nnsMotionKeyClass5.Value.y;
        val.z = nnsMotionKeyClass5.Value.z;
    }

    private static void nnInterpolateConstantF3(
      NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      ref NNS_RGBA val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class5 nnsMotionKeyClass5 = vk[(int)num1];
        val.r = nnsMotionKeyClass5.Value.x;
        val.g = nnsMotionKeyClass5.Value.y;
        val.b = nnsMotionKeyClass5.Value.z;
    }

    private static void nnInterpolateConstantA32_1(
      NNS_MOTION_KEY_Class8[] vk,
      int nKey,
      float frame,
      out int val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class8 nnsMotionKeyClass8 = vk[(int)num1];
        val = nnsMotionKeyClass8.Value;
    }

    private static void nnInterpolateConstantA32_3(
      NNS_MOTION_KEY_Class13[] vk,
      int nKey,
      float frame,
      ref NNS_ROTATE_A32 val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class13 motionKeyClass13 = vk[(int)num1];
        val.x = motionKeyClass13.Value.x;
        val.y = motionKeyClass13.Value.y;
        val.z = motionKeyClass13.Value.z;
    }

    private static void nnInterpolateConstantA16_1(
      NNS_MOTION_KEY_Class14[] vk,
      int nKey,
      float frame,
      out short val)
    {
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if (num1 >= vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        NNS_MOTION_KEY_Class14 motionKeyClass14 = vk[(int)num2];
        val = motionKeyClass14.Value;
    }

    private static void nnInterpolateConstantA16_3(
      NNS_MOTION_KEY_Class16[] vk,
      int nKey,
      float frame,
      ref NNS_ROTATE_A16 val)
    {
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if (num1 >= vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        NNS_MOTION_KEY_Class16 motionKeyClass16 = vk[(int)num2];
        val.x = motionKeyClass16.Value.x;
        val.y = motionKeyClass16.Value.y;
        val.z = motionKeyClass16.Value.z;
    }

    private static void nnInterpolateLinearF1(
      NNS_MOTION_KEY_Class1[] vk,
      int nKey,
      float frame,
      out float val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        int index1 = (int)num1;
        if ((int)num1 >= nKey - 1)
        {
            val = vk[index1].Value;
        }
        else
        {
            int index2 = index1 + 1;
            float num3 = (float)((frame - (double)vk[index2].Frame) / (vk[index1].Frame - (double)vk[index2].Frame));
            val = vk[index2].Value + (vk[index1].Value - vk[index2].Value) * num3;
        }
    }

    private static void nnInterpolateLinearF3(
      NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      NNS_VECTOR val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_1 = vk[(int)num1];
        if (num1 >= nKey - 1)
        {
            val.x = nnsMotionKeyClass5_1.Value.x;
            val.y = nnsMotionKeyClass5_1.Value.y;
            val.z = nnsMotionKeyClass5_1.Value.z;
        }
        else
        {
            NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_2 = vk[(int)(num1 + 1U)];
            float num3 = (float)((frame - (double)nnsMotionKeyClass5_2.Frame) / (nnsMotionKeyClass5_1.Frame - (double)nnsMotionKeyClass5_2.Frame));
            val.x = nnsMotionKeyClass5_2.Value.x + (nnsMotionKeyClass5_1.Value.x - nnsMotionKeyClass5_2.Value.x) * num3;
            val.y = nnsMotionKeyClass5_2.Value.y + (nnsMotionKeyClass5_1.Value.y - nnsMotionKeyClass5_2.Value.y) * num3;
            val.z = nnsMotionKeyClass5_2.Value.z + (nnsMotionKeyClass5_1.Value.z - nnsMotionKeyClass5_2.Value.z) * num3;
        }
    }

    private static void nnInterpolateLinearF3(
      NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      out SNNS_VECTOR val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_1 = vk[(int)num1];
        if (num1 >= nKey - 1)
        {
            val.x = nnsMotionKeyClass5_1.Value.x;
            val.y = nnsMotionKeyClass5_1.Value.y;
            val.z = nnsMotionKeyClass5_1.Value.z;
        }
        else
        {
            NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_2 = vk[(int)(num1 + 1U)];
            float num3 = (float)((frame - (double)nnsMotionKeyClass5_2.Frame) / (nnsMotionKeyClass5_1.Frame - (double)nnsMotionKeyClass5_2.Frame));
            val.x = nnsMotionKeyClass5_2.Value.x + (nnsMotionKeyClass5_1.Value.x - nnsMotionKeyClass5_2.Value.x) * num3;
            val.y = nnsMotionKeyClass5_2.Value.y + (nnsMotionKeyClass5_1.Value.y - nnsMotionKeyClass5_2.Value.y) * num3;
            val.z = nnsMotionKeyClass5_2.Value.z + (nnsMotionKeyClass5_1.Value.z - nnsMotionKeyClass5_2.Value.z) * num3;
        }
    }

    private static void nnInterpolateLinearF3(
      NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      ref NNS_RGBA val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_1 = vk[(int)num1];
        if ((int)num1 >= nKey - 1)
        {
            val.r = nnsMotionKeyClass5_1.Value.x;
            val.g = nnsMotionKeyClass5_1.Value.y;
            val.b = nnsMotionKeyClass5_1.Value.z;
        }
        else
        {
            NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_2 = vk[(int)num1 + 1];
            float num3 = (float)((frame - (double)nnsMotionKeyClass5_2.Frame) / (nnsMotionKeyClass5_1.Frame - (double)nnsMotionKeyClass5_2.Frame));
            val.r = nnsMotionKeyClass5_2.Value.x + (nnsMotionKeyClass5_1.Value.x - nnsMotionKeyClass5_2.Value.x) * num3;
            val.g = nnsMotionKeyClass5_2.Value.y + (nnsMotionKeyClass5_1.Value.y - nnsMotionKeyClass5_2.Value.y) * num3;
            val.b = nnsMotionKeyClass5_2.Value.z + (nnsMotionKeyClass5_1.Value.z - nnsMotionKeyClass5_2.Value.z) * num3;
        }
    }

    private static void nnInterpolateLinearA32_1(
      NNS_MOTION_KEY_Class8[] vk,
      int nKey,
      float frame,
      out int val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class8> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class8>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = (~arrayPointer1).Value;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class8> arrayPointer2 = arrayPointer1 + 1;
            float num3 = (float)((frame - (double)(~arrayPointer2).Frame) / ((~arrayPointer1).Frame - (double)(~arrayPointer2).Frame));
            val = (~arrayPointer2).Value + (int)(((~arrayPointer1).Value - (~arrayPointer2).Value) * (double)num3);
        }
    }

    private static void nnInterpolateLinearA32_3(
      NNS_MOTION_KEY_Class13[] vk,
      int nKey,
      float frame,
      ref NNS_ROTATE_A32 val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class13> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class13>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = (~arrayPointer1).Value;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class13> arrayPointer2 = arrayPointer1 + 1;
            float num3 = (float)((frame - (double)(~arrayPointer2).Frame) / ((~arrayPointer1).Frame - (double)(~arrayPointer2).Frame));
            val.x = (~arrayPointer2).Value.x + (int)(((~arrayPointer1).Value.x - (~arrayPointer2).Value.x) * (double)num3);
            val.y = (~arrayPointer2).Value.y + (int)(((~arrayPointer1).Value.y - (~arrayPointer2).Value.y) * (double)num3);
            val.z = (~arrayPointer2).Value.z + (int)(((~arrayPointer1).Value.z - (~arrayPointer2).Value.z) * (double)num3);
        }
    }

    private static void nnInterpolateLinearA16_1(
      NNS_MOTION_KEY_Class14[] vk,
      int nKey,
      float frame,
      out short val)
    {
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if (num1 >= vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        NNS_MOTION_KEY_Class14 motionKeyClass14_1 = vk[(int)num2];
        if ((int)num2 >= nKey - 1)
        {
            val = motionKeyClass14_1.Value;
        }
        else
        {
            NNS_MOTION_KEY_Class14 motionKeyClass14_2 = vk[(int)num2 + 1];
            int num4 = (int)(65536.0 * (frame - (double)motionKeyClass14_2.Frame) / (motionKeyClass14_1.Frame - motionKeyClass14_2.Frame));
            val = (short)(motionKeyClass14_2.Value + ((short)(motionKeyClass14_1.Value - motionKeyClass14_2.Value) * num4 >> 16));
        }
    }

    private static void nnInterpolateLinearA16_3(
      NNS_MOTION_KEY_Class16[] vk,
      int nKey,
      float frame,
      ref NNS_ROTATE_A16 val)
    {
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if (num1 >= vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        ArrayPointer<NNS_MOTION_KEY_Class16> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class16>(vk, (int)num2);
        if ((int)num2 >= nKey - 1)
        {
            val.x = (~arrayPointer1).Value.x;
            val.y = (~arrayPointer1).Value.y;
            val.z = (~arrayPointer1).Value.z;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class16> arrayPointer2 = arrayPointer1 + 1;
            int num4 = (int)(65536.0 * (frame - (double)(~arrayPointer2).Frame) / ((~arrayPointer1).Frame - (~arrayPointer2).Frame));
            val.x = (short)((~arrayPointer2).Value.x + ((short)((~arrayPointer1).Value.x - (~arrayPointer2).Value.x) * num4 >> 16));
            val.y = (short)((~arrayPointer2).Value.y + ((short)((~arrayPointer1).Value.y - (~arrayPointer2).Value.y) * num4 >> 16));
            val.z = (short)((~arrayPointer2).Value.z + ((short)((~arrayPointer1).Value.z - (~arrayPointer2).Value.z) * num4 >> 16));
        }
    }

    private static float nnSolveBezier(float f0, float h0, float f1, float h1, float frame)
    {
        float num1 = h1 - h0;
        float num2 = f1 - f0 + num1;
        float num3 = -2f * num2 - num1;
        float num4 = (float)(3.0 * (num2 - (double)h0));
        float num5 = 3f * h0;
        float num6 = f0 - frame;
        float num7 = 0.0f;
        float num8 = 0.5f;
        float num9 = 0.5f;
        for (int index = 0; index < 16; ++index)
        {
            if (((num3 * num9 + num4) * num9 + num5) * num9 + num6 < 0.0)
                num7 = num9;
            num8 *= 0.5f;
            num9 = num7 + num8;
        }
        return num9;
    }

    private static void nnInterpolateBezierF1(
      NNS_MOTION_KEY_Class2[] vk,
      int nKey,
      float frame,
      out float val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class2 nnsMotionKeyClass2_1 = vk[(int)num1];
        if ((int)num1 >= nKey - 1)
        {
            val = nnsMotionKeyClass2_1.Value;
        }
        else
        {
            NNS_MOTION_KEY_Class2 nnsMotionKeyClass2_2 = vk[(int)num1 + 1];
            float num3 = nnSolveBezier(nnsMotionKeyClass2_1.Frame, nnsMotionKeyClass2_1.Bhandle.Out.x, nnsMotionKeyClass2_2.Frame, nnsMotionKeyClass2_2.Bhandle.In.x, frame);
            float num4 = nnsMotionKeyClass2_1.Value;
            float y = nnsMotionKeyClass2_1.Bhandle.Out.y;
            float num5 = nnsMotionKeyClass2_2.Value;
            float num6 = nnsMotionKeyClass2_2.Bhandle.In.y - y;
            float num7 = num5 - num4 + num6;
            float num8 = -2f * num7 - num6;
            float num9 = (float)(3.0 * (num7 - (double)y));
            float num10 = 3f * y;
            float num11 = num4;
            val = ((num8 * num3 + num9) * num3 + num10) * num3 + num11;
        }
    }

    private static void nnInterpolateBezierA32_1(
      NNS_MOTION_KEY_Class9[] vk,
      int nKey,
      float frame,
      out int val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class9> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class9>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = (~arrayPointer1).Value;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class9> arrayPointer2 = arrayPointer1 + 1;
            float num3 = nnSolveBezier((~arrayPointer1).Frame, (~arrayPointer1).Bhandle.Out.x, (~arrayPointer2).Frame, (~arrayPointer2).Bhandle.In.x, frame);
            float num4 = (~arrayPointer1).Value;
            float y = (~arrayPointer1).Bhandle.Out.y;
            float num5 = (~arrayPointer2).Value;
            float num6 = (~arrayPointer2).Bhandle.In.y - y;
            float num7 = num5 - num4 + num6;
            float num8 = -2f * num7 - num6;
            float num9 = (float)(3.0 * (num7 - (double)y));
            float num10 = 3f * y;
            float num11 = num4;
            val = (int)(((num8 * (double)num3 + num9) * num3 + num10) * num3 + num11);
        }
    }

    private static void nnRotXYZtoQuat(
      ref NNS_QUATERNION dst,
      int rx,
      int ry,
      int rz,
      uint rtype)
    {
        switch (rtype)
        {
            case 256:
                nnMakeRotateXZYQuaternion(out dst, rx, ry, rz);
                break;
            case 1024:
                nnMakeRotateZXYQuaternion(out dst, rx, ry, rz);
                break;
            default:
                nnMakeRotateXYZQuaternion(out dst, rx, ry, rz);
                break;
        }
    }

    private static void nnInterpolateLerpA16_3(
      NNS_MOTION_KEY_Class16[] vk,
      int nKey,
      float frame,
      ref NNS_QUATERNION val,
      uint rtype)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if (num1 >= vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        ArrayPointer<NNS_MOTION_KEY_Class16> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class16>(vk, (int)num2);
        if ((int)num2 >= nKey - 1)
        {
            nnRotXYZtoQuat(ref val, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class16> arrayPointer2 = arrayPointer1 + 1;
            float t = (frame - (~arrayPointer1).Frame) / ((~arrayPointer2).Frame - (~arrayPointer1).Frame);
            nnRotXYZtoQuat(ref nnsQuaternion1, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
            nnRotXYZtoQuat(ref nnsQuaternion2, (~arrayPointer2).Value.x, (~arrayPointer2).Value.y, (~arrayPointer2).Value.z, rtype);
            nnLerpQuaternion(ref val, ref nnsQuaternion1, ref nnsQuaternion2, t);
        }
    }

    private static void nnInterpolateLerpA32_3(
      NNS_MOTION_KEY_Class13[] vk,
      int nKey,
      float frame,
      ref NNS_QUATERNION val,
      uint rtype)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class13> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class13>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            nnRotXYZtoQuat(ref val, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class13> arrayPointer2 = arrayPointer1 + 1;
            float t = (float)((frame - (double)(~arrayPointer1).Frame) / ((~arrayPointer2).Frame - (double)(~arrayPointer1).Frame));
            nnRotXYZtoQuat(ref nnsQuaternion1, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
            nnRotXYZtoQuat(ref nnsQuaternion2, (~arrayPointer2).Value.x, (~arrayPointer2).Value.y, (~arrayPointer2).Value.z, rtype);
            nnLerpQuaternion(ref val, ref nnsQuaternion1, ref nnsQuaternion2, t);
        }
    }

    private static void nnInterpolateLerpQuat_4(
      NNS_MOTION_KEY_Class7[] vk,
      int nKey,
      float frame,
      ref NNS_QUATERNION val)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class7> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class7>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = (~arrayPointer1).Value;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class7> arrayPointer2 = arrayPointer1 + 1;
            float t = (float)((frame - (double)(~arrayPointer1).Frame) / ((~arrayPointer2).Frame - (double)(~arrayPointer1).Frame));
            NNS_QUATERNION quat1 = (~arrayPointer1).Value;
            NNS_QUATERNION quat2 = (~arrayPointer2).Value;
            nnLerpQuaternion(ref val, ref quat1, ref quat2, t);
        }
    }

    private static void nnInterpolateSlerpA16_3(
      NNS_MOTION_KEY_Class16[] vk,
      int nKey,
      float frame,
      ref NNS_QUATERNION val,
      uint rtype)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if (num1 >= vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        ArrayPointer<NNS_MOTION_KEY_Class16> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class16>(vk, (int)num2);
        if ((int)num2 >= nKey - 1)
        {
            nnRotXYZtoQuat(ref val, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class16> arrayPointer2 = arrayPointer1 + 1;
            float t = (frame - (~arrayPointer1).Frame) / ((~arrayPointer2).Frame - (~arrayPointer1).Frame);
            nnRotXYZtoQuat(ref nnsQuaternion1, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
            nnRotXYZtoQuat(ref nnsQuaternion2, (~arrayPointer2).Value.x, (~arrayPointer2).Value.y, (~arrayPointer2).Value.z, rtype);
            nnSlerpQuaternion(out val, ref nnsQuaternion1, ref nnsQuaternion2, t);
        }
    }

    private static void nnInterpolateSlerpA32_3(
      NNS_MOTION_KEY_Class13[] vk,
      int nKey,
      float frame,
      ref NNS_QUATERNION val,
      uint rtype)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class13> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class13>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            nnRotXYZtoQuat(ref val, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class13> arrayPointer2 = arrayPointer1 + 1;
            float t = (float)((frame - (double)(~arrayPointer1).Frame) / ((~arrayPointer2).Frame - (double)(~arrayPointer1).Frame));
            nnRotXYZtoQuat(ref nnsQuaternion1, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
            nnRotXYZtoQuat(ref nnsQuaternion2, (~arrayPointer2).Value.x, (~arrayPointer2).Value.y, (~arrayPointer2).Value.z, rtype);
            nnSlerpQuaternion(out val, ref nnsQuaternion1, ref nnsQuaternion2, t);
        }
    }

    private static void nnInterpolateSlerpQuat_4(
      NNS_MOTION_KEY_Class7[] vk,
      int nKey,
      float frame,
      ref NNS_QUATERNION val)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class7> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class7>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = (~arrayPointer1).Value;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class7> arrayPointer2 = arrayPointer1 + 1;
            float t = (float)((frame - (double)(~arrayPointer1).Frame) / ((~arrayPointer2).Frame - (double)(~arrayPointer1).Frame));
            NNS_QUATERNION quat1 = (~arrayPointer1).Value;
            NNS_QUATERNION quat2 = (~arrayPointer2).Value;
            nnSlerpQuaternion(out val, ref quat1, ref quat2, t);
        }
    }

    private static void nnInterpolateSquadA16_3(
      NNS_MOTION_KEY_Class16[] vk,
      int nKey,
      float frame,
      ref NNS_QUATERNION val,
      uint rtype)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion3 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion4 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion5 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion6 = new NNS_QUATERNION();
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if (num1 >= vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        ArrayPointer<NNS_MOTION_KEY_Class16> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class16>(vk, (int)num2);
        if ((int)num2 >= nKey - 1)
        {
            nnRotXYZtoQuat(ref val, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class16> arrayPointer2 = arrayPointer1 + 1;
            if (num2 > 0U && (int)num2 < nKey - 2)
            {
                ArrayPointer<NNS_MOTION_KEY_Class16> arrayPointer3 = arrayPointer1 - 1;
                ArrayPointer<NNS_MOTION_KEY_Class16> arrayPointer4 = arrayPointer1 + 2;
                float t = (frame - (~arrayPointer1).Frame) / ((~arrayPointer2).Frame - (~arrayPointer1).Frame);
                nnRotXYZtoQuat(ref nnsQuaternion1, (~arrayPointer3).Value.x, (~arrayPointer3).Value.y, (~arrayPointer3).Value.z, rtype);
                nnRotXYZtoQuat(ref nnsQuaternion2, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
                nnRotXYZtoQuat(ref nnsQuaternion3, (~arrayPointer2).Value.x, (~arrayPointer2).Value.y, (~arrayPointer2).Value.z, rtype);
                nnRotXYZtoQuat(ref nnsQuaternion4, (~arrayPointer4).Value.x, (~arrayPointer4).Value.y, (~arrayPointer4).Value.z, rtype);
                nnSplineQuaternion(ref nnsQuaternion5, ref nnsQuaternion1, ref nnsQuaternion2, ref nnsQuaternion3);
                nnSplineQuaternion(ref nnsQuaternion6, ref nnsQuaternion2, ref nnsQuaternion3, ref nnsQuaternion4);
                nnSquadQuaternion(ref val, ref nnsQuaternion2, ref nnsQuaternion5, ref nnsQuaternion6, ref nnsQuaternion3, t);
            }
            else
                nnInterpolateSlerpA16_3(vk, nKey, frame, ref val, rtype);
        }
    }

    private static void nnInterpolateSquadA32_3(
      NNS_MOTION_KEY_Class13[] vk,
      int nKey,
      float frame,
      ref NNS_QUATERNION val,
      uint rtype)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion3 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion4 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion5 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion6 = new NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class13> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class13>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            nnRotXYZtoQuat(ref val, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class13> arrayPointer2 = arrayPointer1 + 1;
            if (num1 > 0U && (int)num1 < nKey - 2)
            {
                ArrayPointer<NNS_MOTION_KEY_Class13> arrayPointer3 = arrayPointer1 - 1;
                ArrayPointer<NNS_MOTION_KEY_Class13> arrayPointer4 = arrayPointer1 + 2;
                float t = (float)((frame - (double)(~arrayPointer1).Frame) / ((~arrayPointer2).Frame - (double)(~arrayPointer1).Frame));
                nnRotXYZtoQuat(ref nnsQuaternion1, (~arrayPointer3).Value.x, (~arrayPointer3).Value.y, (~arrayPointer3).Value.z, rtype);
                nnRotXYZtoQuat(ref nnsQuaternion2, (~arrayPointer1).Value.x, (~arrayPointer1).Value.y, (~arrayPointer1).Value.z, rtype);
                nnRotXYZtoQuat(ref nnsQuaternion3, (~arrayPointer2).Value.x, (~arrayPointer2).Value.y, (~arrayPointer2).Value.z, rtype);
                nnRotXYZtoQuat(ref nnsQuaternion4, (~arrayPointer4).Value.x, (~arrayPointer4).Value.y, (~arrayPointer4).Value.z, rtype);
                nnSplineQuaternion(ref nnsQuaternion5, ref nnsQuaternion1, ref nnsQuaternion2, ref nnsQuaternion3);
                nnSplineQuaternion(ref nnsQuaternion6, ref nnsQuaternion2, ref nnsQuaternion3, ref nnsQuaternion4);
                nnSquadQuaternion(ref val, ref nnsQuaternion2, ref nnsQuaternion5, ref nnsQuaternion6, ref nnsQuaternion3, t);
            }
            else
                nnInterpolateSlerpA32_3(vk, nKey, frame, ref val, rtype);
        }
    }

    private static void nnInterpolateSquadQuat_4(
      NNS_MOTION_KEY_Class7[] vk,
      int nKey,
      float frame,
      ref NNS_QUATERNION val)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion3 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion4 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion5 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion6 = new NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class7> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class7>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = (~arrayPointer1).Value;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class7> arrayPointer2 = arrayPointer1 + 1;
            if (num1 > 0U && (int)num1 < nKey - 2)
            {
                ArrayPointer<NNS_MOTION_KEY_Class7> arrayPointer3 = arrayPointer1 - 1;
                ArrayPointer<NNS_MOTION_KEY_Class7> arrayPointer4 = arrayPointer1 + 2;
                float t = (float)((frame - (double)(~arrayPointer1).Frame) / ((~arrayPointer2).Frame - (double)(~arrayPointer1).Frame));
                NNS_QUATERNION quatprev = (~arrayPointer3).Value;
                NNS_QUATERNION nnsQuaternion7 = (~arrayPointer1).Value;
                NNS_QUATERNION nnsQuaternion8 = (~arrayPointer2).Value;
                NNS_QUATERNION quatnext = (~arrayPointer4).Value;
                nnSplineQuaternion(ref nnsQuaternion5, ref quatprev, ref nnsQuaternion7, ref nnsQuaternion8);
                nnSplineQuaternion(ref nnsQuaternion6, ref nnsQuaternion7, ref nnsQuaternion8, ref quatnext);
                nnSquadQuaternion(ref val, ref nnsQuaternion7, ref nnsQuaternion5, ref nnsQuaternion6, ref nnsQuaternion8, t);
            }
            else
                nnInterpolateSlerpQuat_4(vk, nKey, frame, ref val);
        }
    }

    private static void nnInterpolateConstantQuat_4(
      NNS_MOTION_KEY_Class7[] vk,
      int nKey,
      float frame,
      ref NNS_QUATERNION val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class7 nnsMotionKeyClass7 = vk[(int)num1];
        val.x = nnsMotionKeyClass7.Value.x;
        val.y = nnsMotionKeyClass7.Value.y;
        val.z = nnsMotionKeyClass7.Value.z;
        val.w = nnsMotionKeyClass7.Value.w;
    }

    private static void nnInterpolateSISplineF1(
      NNS_MOTION_KEY_Class3[] vk,
      int nKey,
      float frame,
      out float val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class3> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class3>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = (~arrayPointer1).Value;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class3> arrayPointer2 = arrayPointer1 + 1;
            float num3 = (~arrayPointer2).Frame - (~arrayPointer1).Frame;
            float num4 = (~arrayPointer2).Value - (~arrayPointer1).Value;
            float num5 = (frame - (~arrayPointer1).Frame) / num3;
            float num6 = (float)(-2.0 * num4 + num3 * ((~arrayPointer1).Shandle.Out + (double)(~arrayPointer2).Shandle.In));
            float num7 = (float)(3.0 * num4 - num3 * (2.0 * (~arrayPointer1).Shandle.Out + (~arrayPointer2).Shandle.In));
            float num8 = num3 * (~arrayPointer1).Shandle.Out;
            val = (~arrayPointer1).Value + num5 * (num5 * (num6 * num5 + num7) + num8);
        }
    }

    private static void nnInterpolateSISplineA32_1(
      NNS_MOTION_KEY_Class10[] vk,
      int nKey,
      float frame,
      out int val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class10> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class10>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = (~arrayPointer1).Value;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class10> arrayPointer2 = arrayPointer1 + 1;
            float num3 = (~arrayPointer2).Frame - (~arrayPointer1).Frame;
            int num4 = (~arrayPointer2).Value - (~arrayPointer1).Value;
            float num5 = (frame - (~arrayPointer1).Frame) / num3;
            float num6 = -2 * num4 + num3 * ((~arrayPointer1).Shandle.Out + (~arrayPointer2).Shandle.In);
            float num7 = 3 * num4 - num3 * (2f * (~arrayPointer1).Shandle.Out + (~arrayPointer2).Shandle.In);
            float num8 = num3 * (~arrayPointer1).Shandle.Out;
            val = (int)((~arrayPointer1).Value + num5 * (num5 * (num6 * (double)num5 + num7) + num8));
        }
    }

    private static void nnInterpolateSISplineA16_1(
      NNS_MOTION_KEY_Class15[] vk,
      int nKey,
      float frame,
      out short val)
    {
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if (num1 >= vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        ArrayPointer<NNS_MOTION_KEY_Class15> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class15>(vk, (int)num2);
        if ((int)num2 >= nKey - 1)
        {
            val = (~arrayPointer1).Value;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class15> arrayPointer2 = arrayPointer1 + 1;
            float num4 = (~arrayPointer2).Frame - (~arrayPointer1).Frame;
            short num5 = (short)((~arrayPointer2).Value - (~arrayPointer1).Value);
            float num6 = (frame - (~arrayPointer1).Frame) / num4;
            float num7 = -2 * num5 + num4 * ((~arrayPointer1).Shandle.Out + (~arrayPointer2).Shandle.In);
            float num8 = 3 * num5 - num4 * (2f * (~arrayPointer1).Shandle.Out + (~arrayPointer2).Shandle.In);
            float num9 = num4 * (~arrayPointer1).Shandle.Out;
            val = (short)((~arrayPointer1).Value + num6 * (num6 * (num7 * (double)num6 + num8) + num9));
        }
    }

    private static void nnInterpolateConstantU1(
      NNS_MOTION_KEY_Class12[] vk,
      int nKey,
      float frame,
      out uint val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class12 motionKeyClass12 = vk[(int)num1];
        val = motionKeyClass12.Value;
    }

    private static void nnInterpolateLinearU1(
      NNS_MOTION_KEY_Class12[] vk,
      int nKey,
      float frame,
      out uint val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class12> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class12>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = (~arrayPointer1).Value;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class12> arrayPointer2 = arrayPointer1 + 1;
            float num3 = (float)((frame - (double)(~arrayPointer2).Frame) / ((~arrayPointer1).Frame - (double)(~arrayPointer2).Frame));
            val = (uint)((~arrayPointer1).Value * (double)num3 + (~arrayPointer2).Value * (1.0 - num3));
        }
    }

    private static int nnInterpolateTriggerU1(
      NNS_MOTION_KEY_Class12[] vk,
      int nKey,
      float frame,
      out uint val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class12 motionKeyClass12 = vk[(int)num1];
        if (frame > motionKeyClass12.Frame + (double)nngNodeUserMotionTriggerTime)
        {
            val = 0U;
            return 0;
        }
        val = motionKeyClass12.Value;
        return 1;
    }

    private static void nnSearchTriggerU1(
      NNS_MOTION_KEY_Class12[] vk,
      int nKey,
      float frame,
      float interval,
      NNS_NODEUSRMOT_CALLBACK_FUNC func,
      NNS_NODEUSRMOT_CALLBACK_VAL val)
    {
        if (nKey == 0)
            return;
        int _offset1;
        int _offset2;
        if (vk[0].Frame < (double)frame && frame < (double)vk[nKey - 1].Frame)
        {
            uint num1 = 0;
            uint num2 = (uint)nKey;
            while (num2 - num1 > 1U)
            {
                uint num3 = num1 + num2 >> 1;
                if (frame > (double)vk[(int)num3].Frame)
                    num1 = num3;
                else
                    num2 = num3;
            }
            if (vk[(int)num1].Frame == (double)frame)
                _offset2 = _offset1 = (int)num1;
            else if (vk[(int)num2].Frame == (double)frame)
            {
                _offset2 = _offset1 = (int)num2;
            }
            else
            {
                _offset2 = (int)num1;
                _offset1 = (int)num2;
            }
        }
        else
        {
            _offset2 = _offset1 = -1;
            if (frame < (double)vk[0].Frame)
                _offset1 = 0;
            if (vk[nKey - 1].Frame < (double)frame)
                _offset2 = nKey - 1;
            if (vk[0].Frame == (double)frame)
                _offset2 = _offset1 = 0;
            if (vk[nKey - 1].Frame == (double)frame)
                _offset2 = _offset1 = nKey - 1;
        }
        if (interval > 0.0 && _offset2 != -1)
        {
            ArrayPointer<NNS_MOTION_KEY_Class12> arrayPointer = new ArrayPointer<NNS_MOTION_KEY_Class12>(vk, _offset2);
            while ((~arrayPointer).Frame + (double)interval > frame && _offset2 >= 0)
            {
                val.IValue = (~arrayPointer).Value;
                val.Frame = (~arrayPointer).Frame;
                func(val);
                --_offset2;
                --arrayPointer;
            }
        }
        else
        {
            if (interval >= 0.0 || _offset1 == -1)
                return;
            ArrayPointer<NNS_MOTION_KEY_Class12> arrayPointer = new ArrayPointer<NNS_MOTION_KEY_Class12>(vk, _offset1);
            while ((~arrayPointer).Frame + (double)interval < frame && _offset1 < nKey)
            {
                val.IValue = (~arrayPointer).Value;
                val.Frame = (~arrayPointer).Frame;
                func(val);
                ++_offset1;
                ++arrayPointer;
            }
        }
    }

    private static void nnInterpolateConstantS32_1(
      NNS_MOTION_KEY_Class11[] vk,
      int nKey,
      float frame,
      out int val)
    {
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if (num1 >= (double)vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        NNS_MOTION_KEY_Class11 motionKeyClass11 = vk[(int)num2];
        val = motionKeyClass11.Value;
    }

    private static void nnInterpolateConstantF2(
      NNS_MOTION_KEY_Class4[] vk,
      int nKey,
      float frame,
      out NNS_TEXCOORD val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        NNS_MOTION_KEY_Class4 nnsMotionKeyClass4 = vk[(int)num1];
        val.u = nnsMotionKeyClass4.Value.u;
        val.v = nnsMotionKeyClass4.Value.v;
    }

    private static void nnInterpolateLinearF2(
      NNS_MOTION_KEY_Class4[] vk,
      int nKey,
      float frame,
      out NNS_TEXCOORD val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if (frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        ArrayPointer<NNS_MOTION_KEY_Class4> arrayPointer1 = new ArrayPointer<NNS_MOTION_KEY_Class4>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val.u = (~arrayPointer1).Value.u;
            val.v = (~arrayPointer1).Value.v;
        }
        else
        {
            ArrayPointer<NNS_MOTION_KEY_Class4> arrayPointer2 = arrayPointer1 + 1;
            float num3 = (float)((frame - (double)(~arrayPointer2).Frame) / ((~arrayPointer1).Frame - (double)(~arrayPointer2).Frame));
            float num4 = 1f - num3;
            val.u = (float)((~arrayPointer1).Value.u * (double)num3 + (~arrayPointer2).Value.u * (double)num4);
            val.v = (float)((~arrayPointer1).Value.v * (double)num3 + (~arrayPointer2).Value.v * (double)num4);
        }
    }


    private static void nnCalc1BoneSIIK(
      NNS_MATRIX jnt1mtx,
      NNS_MATRIX jnt1motmtx,
      NNS_MATRIX effmtx,
      float lbone1)
    {
        GlobalPool<NNS_MATRIX>.Alloc();
        GlobalPool<NNS_MATRIX>.Alloc();
        mppAssertNotImpl();
    }

    private static void nnCalc2BoneSIIK(
      NNS_MATRIX jnt1mtx,
      NNS_MATRIX jnt1motmtx,
      NNS_MATRIX jnt2mtx,
      NNS_MATRIX jnt2motmtx,
      NNS_MATRIX effmtx,
      float lbone1,
      float lbone2,
      int zpref)
    {
        mppAssertNotImpl();
    }

    private void nnAdjustMatrixXaxis(NNS_MATRIX mtx, NNS_VECTORFAST pos)
    {
        mppAssertNotImpl();
    }

    private void nnCalcCosineTheorem(out float sin, out float cos, float a, float b, float c)
    {
        mppAssertNotImpl();
        sin = cos = 0.0f;
    }

    private void nnCalcCosineTheorem2(
      out float sin0,
      out float cos0,
      out float sin1,
      out float cos1,
      float a,
      float b,
      float c)
    {
        mppAssertNotImpl();
        sin0 = cos0 = sin1 = cos1 = 0.0f;
    }

    private void nnCalcNodeStatusListInitialPose(
      uint[] nodestatlist,
      NNS_OBJECT obj,
      NNS_MATRIX basemtx,
      uint flag)
    {
        mppAssertNotImpl();
    }

    private static void nnmSetUpVector(NNS_VECTOR vec, float x, float y, float z)
    {
        mppAssertNotImpl();
    }

    private static void nnmAddScaleVector(NNS_VECTOR dst, NNS_VECTOR add, float scl)
    {
        mppAssertNotImpl();
    }

    private static void nnmBlendVector(
      NNS_VECTOR dst,
      NNS_VECTOR vec1,
      float blend1,
      NNS_VECTOR vec2,
      float blend2)
    {
        mppAssertNotImpl();
    }

    private static void nnmTransformVector(
      NNS_VECTOR dst,
      NNS_MATRIX mtx,
      NNS_VECTOR src)
    {
        mppAssertNotImpl();
    }

    private static void nnmTransformNormalVector(
      NNS_VECTOR dst,
      NNS_MATRIX mtx,
      NNS_VECTOR src)
    {
        mppAssertNotImpl();
    }

    private static void nnCalcPliablePosNrm(
      object pPliablePositions,
      object pPliableNormals,
      int nVtx,
      object pObjPositions,
      uint PosType,
      object pObjNormals,
      uint NrmType,
      object pWeights,
      int nWeight,
      object pMtxIndices,
      int nMtxIdx,
      NNS_MATRIX[] posmtx,
      NNS_MATRIX[] nrmmtx)
    {
        mppAssertNotImpl();
    }

    private static void nnCalcPliableObjectSpaceNormal(
      object pPliablePositions,
      object pPliableNormals,
      object pPliableTangents,
      object pPliableBinormals,
      int nVtx,
      object pObjPositions,
      uint PosType,
      object pWeights,
      int nWeight,
      object pMtxIndices,
      int nMtxIdx,
      NNS_MATRIX[] posmtx,
      NNS_MATRIX[] nrmmtx)
    {
        mppAssertNotImpl();
    }

    private static void nnCalcPliableTangentSpaceNormal(
      object pPliablePositions,
      object pPliableNormals,
      object pPliableTangents,
      object pPliableBinormals,
      int nVtx,
      object pObjPositions,
      uint PosType,
      object pObjNormals,
      uint NrmType,
      object pObjTangents,
      uint TanType,
      object pObjBinormals,
      uint BnrmType,
      object pWeights,
      int nWeight,
      object pMtxIndices,
      int nMtxIdx,
      NNS_MATRIX[] posmtx,
      NNS_MATRIX[] nrmmtx)
    {
        mppAssertNotImpl();
    }

    private static void nnCalcPliableVerticesGeneral(
      object pPliablePositions,
      object pPliableNormals,
      object pPliableTangents,
      object pPliableBinormals,
      int nVtx,
      object pObjPositions,
      uint PosType,
      object pObjNormals,
      uint NrmType,
      object pObjTangents,
      uint TanType,
      object pObjBinormals,
      uint BnrmType,
      object pWeights,
      int nWeight,
      object pMtxIndices,
      int nMtxIdx,
      NNS_MATRIX[] posmtx,
      NNS_MATRIX[] nrmmtx)
    {
        mppAssertNotImpl();
    }

    private void nnCalcPliableObject(
      NNS_PLIABLEOBJ pobj,
      NNS_MATRIX mtxpal,
      uint flag)
    {
        mppAssertNotImpl();
    }

    private void nnCalcPliableObjectNodeStatusList(
      NNS_PLIABLEOBJ pobj,
      NNS_MATRIX mtxpal,
      uint nodestatlist,
      uint flag)
    {
        mppAssertNotImpl();
    }

    private void nnTransformUpVectorCameraLocal(
      NNS_VECTOR vec,
      NNS_CAMERA_TARGET_UPVECTOR cam,
      float x,
      float y,
      float z)
    {
        mppAssertNotImpl();
    }

    private void nnPitchUpVectorCamera(NNS_CAMERA_TARGET_UPVECTOR cam, int pitch)
    {
        mppAssertNotImpl();
    }

    private void nnRollUpVectorCamera(NNS_CAMERA_TARGET_UPVECTOR cam, int roll)
    {
        mppAssertNotImpl();
    }

    private void nnYawUpVectorCamera(NNS_CAMERA_TARGET_UPVECTOR cam, int yaw)
    {
        mppAssertNotImpl();
    }

    private void nnApproachTargetUpVectorCamera(NNS_CAMERA_TARGET_UPVECTOR cam, float d)
    {
        mppAssertNotImpl();
    }

    private void nnApproachTargetUpVectorCameraLevel(NNS_CAMERA_TARGET_UPVECTOR cam, float d)
    {
        mppAssertNotImpl();
    }

    private void nnMoveTargetUpVectorCamera(NNS_CAMERA_TARGET_UPVECTOR cam, float d)
    {
        mppAssertNotImpl();
    }

    private void nnRotateUpVectorCameraAroundTargetH(NNS_CAMERA_TARGET_UPVECTOR cam, int ang)
    {
        mppAssertNotImpl();
    }

    private void nnRotateUpVectorCameraAroundTargetV(NNS_CAMERA_TARGET_UPVECTOR cam, int ang)
    {
        mppAssertNotImpl();
    }

    private void nnRotateUpVectorCameraLevelAroundTarget(
      NNS_CAMERA_TARGET_UPVECTOR cam,
      int ang)
    {
        mppAssertNotImpl();
    }

    private void nnMoveUpVectorCameraLocal(
      NNS_CAMERA_TARGET_UPVECTOR cam,
      float x,
      float y,
      float z)
    {
        mppAssertNotImpl();
    }

    private void nnConvertCameraPointerUpVectorCamera(
      NNS_CAMERA_TARGET_UPVECTOR cam,
      NNS_CAMERAPTR camptr)
    {
        mppAssertNotImpl();
    }


    private void nnDrawMultiObject(
      NNS_OBJECT obj,
      NNS_MATRIX[] mtxpalptrlist,
      uint[] nodestatlistptrlist,
      uint subobjtype,
      uint flag,
      int num)
    {
        mppAssertNotImpl();
    }

    private uint nnCalcObjectSize(NNS_OBJECT obj)
    {
        return 0;
    }

    private static uint nnCopyMaterialList(
      NNS_MATERIALPTR[] dstmatptr,
      NNS_MATERIALPTR[] srcmatptr,
      int nMaterial,
      uint flag)
    {
        for (int index1 = 0; index1 < nMaterial; ++index1)
        {
            if (((int)srcmatptr[index1].fType & 2) != 0)
            {
                NNS_MATERIAL_STDSHADER_DESC materialStdshaderDesc = null;
                NNS_MATERIAL_STDSHADER_DESC pMaterial = (NNS_MATERIAL_STDSHADER_DESC)srcmatptr[index1].pMaterial;
                if (((int)srcmatptr[index1].fType & 4) != 0)
                {
                    if (dstmatptr != null)
                    {
                        dstmatptr[index1].fType = 6U;
                        dstmatptr[index1].pMaterial = (NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)(materialStdshaderDesc = new NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE());
                        ((NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)materialStdshaderDesc).Assign((NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)pMaterial);
                    }
                }
                else if (dstmatptr != null)
                {
                    dstmatptr[index1].fType = 2U;
                    dstmatptr[index1].pMaterial = materialStdshaderDesc = new NNS_MATERIAL_STDSHADER_DESC();
                    materialStdshaderDesc.Assign(pMaterial);
                }
                NNS_MATERIAL_STDSHADER_COLOR pColor1 = pMaterial.pColor;
                int num1 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    NNS_MATERIAL_STDSHADER_COLOR pColor2 = ((NNS_MATERIAL_STDSHADER_DESC)srcmatptr[index2].pMaterial).pColor;
                    if (pColor1 == pColor2)
                    {
                        if (dstmatptr != null)
                            materialStdshaderDesc.pColor = ((NNS_MATERIAL_STDSHADER_DESC)dstmatptr[index2].pMaterial).pColor;
                        num1 = 0;
                        break;
                    }
                }
                if (num1 != 0 && dstmatptr != null)
                {
                    materialStdshaderDesc.pColor = new NNS_MATERIAL_STDSHADER_COLOR();
                    materialStdshaderDesc.pColor.Assign(pColor1);
                }
                NNS_MATERIAL_LOGIC pLogic1 = pMaterial.pLogic;
                int num2 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    NNS_MATERIAL_LOGIC pLogic2 = ((NNS_MATERIAL_STDSHADER_DESC)srcmatptr[index2].pMaterial).pLogic;
                    if (pLogic1 == pLogic2)
                    {
                        if (dstmatptr != null)
                            materialStdshaderDesc.pLogic = ((NNS_MATERIAL_STDSHADER_DESC)dstmatptr[index2].pMaterial).pLogic;
                        num2 = 0;
                        break;
                    }
                }
                if (num2 != 0 && dstmatptr != null)
                {
                    materialStdshaderDesc.pLogic = new NNS_MATERIAL_LOGIC();
                    materialStdshaderDesc.pLogic.Assign(pLogic1);
                }
                if (pMaterial.nTex > 0)
                {
                    if (dstmatptr != null)
                    {
                        materialStdshaderDesc.pTexDesc = new NNS_MATERIAL_STDSHADER_TEXMAP_DESC[pMaterial.nTex];
                        int index2 = 0;
                        for (; index1 < pMaterial.nTex; ++index1)
                            materialStdshaderDesc.pTexDesc[index2] = new NNS_MATERIAL_STDSHADER_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                    }
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                    {
                        NNS_MATERIAL_STDSHADER_TEXMAP_DESC stdshaderTexmapDesc1 = pMaterial.pTexDesc[index2];
                        NNS_MATERIAL_STDSHADER_TEXMAP_DESC stdshaderTexmapDesc2 = null;
                        if (dstmatptr != null)
                            stdshaderTexmapDesc2 = materialStdshaderDesc.pTexDesc[index2];
                        if (stdshaderTexmapDesc1.pBorderColor.HasValue && dstmatptr != null)
                        {
                            stdshaderTexmapDesc2.pBorderColor = new NNS_RGBA?(new NNS_RGBA());
                            stdshaderTexmapDesc2.pBorderColor = stdshaderTexmapDesc1.pBorderColor;
                        }
                        if (stdshaderTexmapDesc1.pFilterMode != null && dstmatptr != null)
                        {
                            stdshaderTexmapDesc2.pFilterMode = new NNS_TEXTURE_FILTERMODE();
                            stdshaderTexmapDesc2.pFilterMode.Assign(stdshaderTexmapDesc1.pFilterMode);
                        }
                        if (stdshaderTexmapDesc1.pLODParam != null && dstmatptr != null)
                        {
                            stdshaderTexmapDesc2.pLODParam = new NNS_TEXTURE_LOD_PARAM();
                            stdshaderTexmapDesc2.pLODParam.Assign(stdshaderTexmapDesc1.pLODParam);
                        }
                    }
                }
            }
            else if (((int)srcmatptr[index1].fType & 8) != 0)
            {
                NNS_MATERIAL_GLES11_DESC materialGleS11Desc = null;
                NNS_MATERIAL_GLES11_DESC pMaterial = (NNS_MATERIAL_GLES11_DESC)srcmatptr[index1].pMaterial;
                if (dstmatptr != null)
                {
                    dstmatptr[index1].fType = 8U;
                    dstmatptr[index1].pMaterial = materialGleS11Desc = new NNS_MATERIAL_GLES11_DESC();
                    materialGleS11Desc.Assign(pMaterial);
                }
                NNS_MATERIAL_STDSHADER_COLOR pColor1 = pMaterial.pColor;
                int num1 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    NNS_MATERIAL_STDSHADER_COLOR pColor2 = ((NNS_MATERIAL_GLES11_DESC)srcmatptr[index2].pMaterial).pColor;
                    if (pColor1 == pColor2)
                    {
                        if (dstmatptr != null)
                            materialGleS11Desc.pColor = ((NNS_MATERIAL_GLES11_DESC)dstmatptr[index2].pMaterial).pColor;
                        num1 = 0;
                        break;
                    }
                }
                if (num1 != 0 && dstmatptr != null)
                {
                    materialGleS11Desc.pColor = new NNS_MATERIAL_STDSHADER_COLOR();
                    materialGleS11Desc.pColor.Assign(pColor1);
                }
                NNS_MATERIAL_GLES11_LOGIC pLogic1 = pMaterial.pLogic;
                int num2 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    NNS_MATERIAL_GLES11_LOGIC pLogic2 = ((NNS_MATERIAL_GLES11_DESC)srcmatptr[index2].pMaterial).pLogic;
                    if (pLogic1 == pLogic2)
                    {
                        if (dstmatptr != null)
                            materialGleS11Desc.pLogic = ((NNS_MATERIAL_GLES11_DESC)dstmatptr[index2].pMaterial).pLogic;
                        num2 = 0;
                        break;
                    }
                }
                if (num2 != 0 && dstmatptr != null)
                {
                    materialGleS11Desc.pLogic = new NNS_MATERIAL_GLES11_LOGIC();
                    materialGleS11Desc.pLogic.Assign(pLogic1);
                }
                if (pMaterial.nTex > 0)
                {
                    if (dstmatptr != null)
                    {
                        materialGleS11Desc.pTexDesc = new NNS_MATERIAL_GLES11_TEXMAP_DESC[pMaterial.nTex];
                        for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                            materialGleS11Desc.pTexDesc[index2] = new NNS_MATERIAL_GLES11_TEXMAP_DESC(ref pMaterial.pTexDesc[index2]);
                    }
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                    {
                        NNS_MATERIAL_GLES11_TEXMAP_DESC gleS11TexmapDesc1 = pMaterial.pTexDesc[index2];
                        NNS_MATERIAL_GLES11_TEXMAP_DESC gleS11TexmapDesc2;
                        if (dstmatptr != null)
                            gleS11TexmapDesc2 = materialGleS11Desc.pTexDesc[index2];
                        if (gleS11TexmapDesc1.pCombine != null && dstmatptr != null)
                        {
                            gleS11TexmapDesc2.pCombine = new NNS_TEXTURE_GLES11_COMBINE();
                            gleS11TexmapDesc2.pCombine.Assign(gleS11TexmapDesc1.pCombine);
                        }
                        if (gleS11TexmapDesc1.pFilterMode != null && dstmatptr != null)
                        {
                            gleS11TexmapDesc2.pFilterMode = new NNS_TEXTURE_FILTERMODE();
                            gleS11TexmapDesc2.pFilterMode.Assign(gleS11TexmapDesc1.pFilterMode);
                        }
                    }
                }
            }
            else if (((int)srcmatptr[index1].fType & 1) != 0)
            {
                NNS_MATERIAL_DESC nnsMaterialDesc = null;
                NNS_MATERIAL_DESC pMaterial1 = (NNS_MATERIAL_DESC)srcmatptr[index1].pMaterial;
                if (dstmatptr != null)
                {
                    dstmatptr[index1].fType = 1U;
                    dstmatptr[index1].pMaterial = nnsMaterialDesc = new NNS_MATERIAL_DESC();
                    nnsMaterialDesc.Assign(pMaterial1);
                }
                NNS_MATERIAL_COLOR pColor1 = pMaterial1.pColor;
                int num1 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    NNS_MATERIAL_DESC pMaterial2 = (NNS_MATERIAL_DESC)srcmatptr[index2].pMaterial;
                    NNS_MATERIAL_COLOR pColor2 = pMaterial2.pColor;
                    NNS_MATERIAL_COLOR pBackColor = pMaterial2.pBackColor;
                    if (pColor1 == pColor2)
                    {
                        if (dstmatptr != null)
                            nnsMaterialDesc.pColor = ((NNS_MATERIAL_DESC)dstmatptr[index2].pMaterial).pColor;
                        num1 = 0;
                        break;
                    }
                    if (pColor1 == pBackColor)
                    {
                        if (dstmatptr != null)
                            nnsMaterialDesc.pColor = ((NNS_MATERIAL_DESC)dstmatptr[index2].pMaterial).pBackColor;
                        num1 = 0;
                        break;
                    }
                }
                if (num1 != 0 && dstmatptr != null)
                {
                    nnsMaterialDesc.pColor = new NNS_MATERIAL_COLOR();
                    nnsMaterialDesc.pColor.Assign(pColor1);
                }
                NNS_MATERIAL_COLOR pBackColor1 = pMaterial1.pBackColor;
                if (pBackColor1 != null)
                {
                    int num2 = 1;
                    for (int index2 = 0; index2 < index1; ++index2)
                    {
                        NNS_MATERIAL_DESC pMaterial2 = (NNS_MATERIAL_DESC)srcmatptr[index2].pMaterial;
                        NNS_MATERIAL_COLOR pColor2 = pMaterial2.pColor;
                        NNS_MATERIAL_COLOR pBackColor2 = pMaterial2.pBackColor;
                        if (pBackColor1 == pColor2)
                        {
                            if (dstmatptr != null)
                                nnsMaterialDesc.pBackColor = ((NNS_MATERIAL_DESC)dstmatptr[index2].pMaterial).pColor;
                            num2 = 0;
                            break;
                        }
                        if (pBackColor1 == pBackColor2)
                        {
                            if (dstmatptr != null)
                                nnsMaterialDesc.pBackColor = ((NNS_MATERIAL_DESC)dstmatptr[index2].pMaterial).pBackColor;
                            num2 = 0;
                            break;
                        }
                    }
                    if (num2 != 0 && dstmatptr != null)
                    {
                        nnsMaterialDesc.pBackColor = new NNS_MATERIAL_COLOR();
                        nnsMaterialDesc.pBackColor.Assign(pBackColor1);
                    }
                }
                NNS_MATERIAL_LOGIC pLogic1 = pMaterial1.pLogic;
                int num3 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    NNS_MATERIAL_LOGIC pLogic2 = ((NNS_MATERIAL_DESC)srcmatptr[index2].pMaterial).pLogic;
                    if (pLogic1 == pLogic2)
                    {
                        if (dstmatptr != null)
                            nnsMaterialDesc.pLogic = ((NNS_MATERIAL_DESC)dstmatptr[index2].pMaterial).pLogic;
                        num3 = 0;
                        break;
                    }
                }
                if (num3 != 0 && dstmatptr != null)
                {
                    nnsMaterialDesc.pLogic = new NNS_MATERIAL_LOGIC();
                    nnsMaterialDesc.pLogic.Assign(pLogic1);
                }
                if (pMaterial1.nTex > 0)
                {
                    if (dstmatptr != null)
                    {
                        nnsMaterialDesc.pTexDesc = new NNS_MATERIAL_TEXMAP_DESC[pMaterial1.nTex];
                        int index2 = 0;
                        for (; index1 < pMaterial1.nTex; ++index1)
                            nnsMaterialDesc.pTexDesc[index2] = new NNS_MATERIAL_TEXMAP_DESC(pMaterial1.pTexDesc[index2]);
                    }
                    for (int index2 = 0; index2 < pMaterial1.nTex; ++index2)
                    {
                        NNS_MATERIAL_TEXMAP_DESC materialTexmapDesc1 = pMaterial1.pTexDesc[index2];
                        NNS_MATERIAL_TEXMAP_DESC materialTexmapDesc2 = null;
                        if (dstmatptr != null)
                            materialTexmapDesc2 = nnsMaterialDesc.pTexDesc[index2];
                        if (materialTexmapDesc1.pCombine != null && dstmatptr != null)
                        {
                            materialTexmapDesc2.pCombine = new NNS_TEXTURE_COMBINE();
                            materialTexmapDesc2.pCombine.Assign(materialTexmapDesc1.pCombine);
                        }
                        if (materialTexmapDesc1.pBorderColor.HasValue && dstmatptr != null)
                        {
                            materialTexmapDesc2.pBorderColor = new NNS_RGBA?(new NNS_RGBA());
                            materialTexmapDesc2.pBorderColor = materialTexmapDesc1.pBorderColor;
                        }
                        if (materialTexmapDesc1.pFilterMode != null && dstmatptr != null)
                        {
                            materialTexmapDesc2.pFilterMode = new NNS_TEXTURE_FILTERMODE();
                            materialTexmapDesc2.pFilterMode.Assign(materialTexmapDesc1.pFilterMode);
                        }
                        if (materialTexmapDesc1.pLODParam != null && dstmatptr != null)
                        {
                            materialTexmapDesc2.pLODParam = new NNS_TEXTURE_LOD_PARAM();
                            materialTexmapDesc2.pLODParam.Assign(materialTexmapDesc1.pLODParam);
                        }
                    }
                }
            }
        }
        return 0;
    }

    private static uint nnCopyVertexList(
      NNS_VTXLISTPTR[] dstvlist,
      NNS_VTXLISTPTR[] srcvlist,
      int nVtxList,
      uint flag)
    {
        for (int index1 = 0; index1 < nVtxList; ++index1)
        {
            if (dstvlist != null)
                dstvlist[index1].fType = srcvlist[index1].fType;
            if (((int)srcvlist[index1].fType & 1) != 0)
            {
                NNS_VTXLIST_GL_DESC pVtxList = (NNS_VTXLIST_GL_DESC)srcvlist[index1].pVtxList;
                NNS_VTXLIST_GL_DESC nnsVtxlistGlDesc = null;
                if (dstvlist != null)
                {
                    dstvlist[index1].pVtxList = nnsVtxlistGlDesc = new NNS_VTXLIST_GL_DESC();
                    nnsVtxlistGlDesc.Assign(pVtxList);
                }
                if (dstvlist != null)
                {
                    nnsVtxlistGlDesc.pArray = new NNS_VTXARRAY_GL[pVtxList.nArray];
                    for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
                        nnsVtxlistGlDesc.pArray[index2] = new NNS_VTXARRAY_GL(pVtxList.pArray[index1]);
                }
                if (((int)srcvlist[index1].fType & 16) != 0)
                {
                    if (dstvlist == null)
                        ;
                }
                else if (((int)pVtxList.Type & 65536) == 0)
                {
                    if (dstvlist != null)
                    {
                        byte[] data = new byte[pVtxList.VertexBufferSize];
                        Array.Copy(pVtxList.pVertexBuffer.Data, data, pVtxList.VertexBufferSize);
                        nnsVtxlistGlDesc.pVertexBuffer = ByteBuffer.Wrap(data);
                        for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
                            nnsVtxlistGlDesc.pArray[index2].Pointer = nnsVtxlistGlDesc.pVertexBuffer + pVtxList.pArray[index2].Pointer.Offset;
                        nnsVtxlistGlDesc.VertexBufferSize = pVtxList.VertexBufferSize;
                    }
                    else
                    {
                        int num = 0;
                        while (num < pVtxList.nArray)
                            ++num;
                    }
                }
                else if (dstvlist != null)
                {
                    byte[] data = new byte[pVtxList.VertexBufferSize];
                    Array.Copy(pVtxList.pVertexBuffer.Data, data, pVtxList.VertexBufferSize);
                    nnsVtxlistGlDesc.pVertexBuffer = ByteBuffer.Wrap(data);
                    for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
                        nnsVtxlistGlDesc.pArray[index2].Pointer = nnsVtxlistGlDesc.pVertexBuffer + pVtxList.pArray[index2].Pointer.Offset;
                }
                if (dstvlist != null && pVtxList.nMatrix != 0)
                {
                    nnsVtxlistGlDesc.pMatrixIndices = new ushort[pVtxList.nMatrix];
                    Array.Copy(pVtxList.pMatrixIndices, nnsVtxlistGlDesc.pMatrixIndices, pVtxList.nMatrix);
                }
            }
            else if (((int)srcvlist[index1].fType & 16711680) != 0)
            {
                NNS_VTXLIST_COMMON_DESC pVtxList = (NNS_VTXLIST_COMMON_DESC)srcvlist[index1].pVtxList;
                NNS_VTXLIST_COMMON_DESC vtxlistCommonDesc = null;
                if (dstvlist != null)
                {
                    dstvlist[index1].pVtxList = vtxlistCommonDesc = new NNS_VTXLIST_COMMON_DESC();
                    vtxlistCommonDesc.Assign(pVtxList);
                }
                for (int index2 = 0; index2 < 4; ++index2)
                {
                    NNS_VTXLIST_COMMON_ARRAY vtxlistCommonArray = null;
                    switch (index2)
                    {
                        case 0:
                            vtxlistCommonArray = pVtxList.List0;
                            break;
                        case 1:
                            vtxlistCommonArray = pVtxList.List1;
                            break;
                        case 2:
                            vtxlistCommonArray = pVtxList.List2;
                            break;
                        case 3:
                            vtxlistCommonArray = pVtxList.List3;
                            break;
                    }
                    if (vtxlistCommonArray.pList != null && dstvlist != null)
                    {
                        switch (index2)
                        {
                            case 0:
                                NNS_VTXLIST_COMMON_ARRAY list0 = vtxlistCommonDesc.List0;
                                break;
                            case 1:
                                NNS_VTXLIST_COMMON_ARRAY list1 = vtxlistCommonDesc.List1;
                                break;
                            case 2:
                                NNS_VTXLIST_COMMON_ARRAY list2 = vtxlistCommonDesc.List2;
                                break;
                            case 3:
                                NNS_VTXLIST_COMMON_ARRAY list3 = vtxlistCommonDesc.List3;
                                break;
                        }
                        mppAssertNotImpl();
                    }
                }
            }
            else
                NNM_ASSERT(0, "Unknown vertex foramt.\n");
        }
        return 0;
    }

    private static uint nnCopyPrimitiveList(
      NNS_PRIMLISTPTR[] dstplist,
      NNS_PRIMLISTPTR[] srcplist,
      int nPrimList,
      uint flag)
    {
        for (int index1 = 0; index1 < nPrimList; ++index1)
        {
            if (dstplist != null)
                dstplist[index1].fType = srcplist[index1].fType;
            if (((int)srcplist[index1].fType & 1) != 0)
            {
                NNS_PRIMLIST_GL_DESC pPrimList = (NNS_PRIMLIST_GL_DESC)srcplist[index1].pPrimList;
                NNS_PRIMLIST_GL_DESC nnsPrimlistGlDesc = null;
                if (dstplist != null)
                {
                    dstplist[index1].pPrimList = nnsPrimlistGlDesc = new NNS_PRIMLIST_GL_DESC();
                    nnsPrimlistGlDesc.Assign(pPrimList);
                }
                if (dstplist != null)
                {
                    nnsPrimlistGlDesc.pCounts = new int[pPrimList.nPrim];
                    Array.Copy(pPrimList.pCounts, nnsPrimlistGlDesc.pCounts, pPrimList.nPrim);
                }
                if (dstplist != null)
                    nnsPrimlistGlDesc.pIndices = new UShortBuffer[pPrimList.nPrim];
                if (((int)srcplist[index1].fType & 2) != 0)
                {
                    if (dstplist == null)
                        ;
                }
                else if (dstplist != null)
                {
                    byte[] data = new byte[pPrimList.IndexBufferSize];
                    Array.Copy(pPrimList.pIndexBuffer.Data, data, pPrimList.IndexBufferSize);
                    nnsPrimlistGlDesc.pIndexBuffer = ByteBuffer.Wrap(data);
                    for (int index2 = 0; index2 < pPrimList.nPrim; ++index2)
                        nnsPrimlistGlDesc.pIndices[index2] = (nnsPrimlistGlDesc.pIndexBuffer + pPrimList.pIndices[index2].Offset).AsUShortBuffer();
                }
            }
            else if (((int)srcplist[index1].fType & 16711680) != 0)
            {
                switch (srcplist[index1].fType)
                {
                    case 65536:
                    case 131072:
                        NNS_PRIMLIST_COMMON_TRIANGLE_STRIP pPrimList1 = (NNS_PRIMLIST_COMMON_TRIANGLE_STRIP)srcplist[index1].pPrimList;
                        NNS_PRIMLIST_COMMON_TRIANGLE_STRIP commonTriangleStrip = null;
                        if (dstplist != null)
                        {
                            dstplist[index1].pPrimList = commonTriangleStrip = new NNS_PRIMLIST_COMMON_TRIANGLE_STRIP();
                            commonTriangleStrip.Assign(pPrimList1);
                        }
                        int nStrip = pPrimList1.nStrip;
                        if (dstplist != null)
                        {
                            commonTriangleStrip.pLengthList = new ushort[pPrimList1.nStrip];
                            Array.Copy(pPrimList1.pLengthList, commonTriangleStrip.pLengthList, pPrimList1.nStrip);
                        }
                        int num = 0;
                        for (int index2 = 0; index2 < nStrip; ++index2)
                            num += pPrimList1.pLengthList[index2];
                        if (dstplist != null)
                        {
                            commonTriangleStrip.pStripList = new ushort[pPrimList1.nIndexSetSize * num];
                            Array.Copy(pPrimList1.pStripList, commonTriangleStrip.pStripList, pPrimList1.nIndexSetSize * num);
                            continue;
                        }
                        continue;
                    case 262144:
                        NNS_PRIMLIST_COMMON_TRIANGLE_LIST pPrimList2 = (NNS_PRIMLIST_COMMON_TRIANGLE_LIST)srcplist[index1].pPrimList;
                        NNS_PRIMLIST_COMMON_TRIANGLE_LIST commonTriangleList = null;
                        if (dstplist != null)
                        {
                            dstplist[index1].pPrimList = commonTriangleList = new NNS_PRIMLIST_COMMON_TRIANGLE_LIST();
                            commonTriangleList.Assign(pPrimList2);
                        }
                        if (dstplist != null)
                        {
                            commonTriangleList.pTriangleList = new ushort[pPrimList2.nIndexSetSize * pPrimList2.nTriangle];
                            Array.Copy(pPrimList2.pTriangleList, commonTriangleList.pTriangleList, pPrimList2.nIndexSetSize * pPrimList2.nTriangle);
                            continue;
                        }
                        continue;
                    case 524288:
                        NNS_PRIMLIST_COMMON_QUAD_LIST pPrimList3 = (NNS_PRIMLIST_COMMON_QUAD_LIST)srcplist[index1].pPrimList;
                        NNS_PRIMLIST_COMMON_QUAD_LIST primlistCommonQuadList = null;
                        if (dstplist != null)
                        {
                            dstplist[index1].pPrimList = primlistCommonQuadList = new NNS_PRIMLIST_COMMON_QUAD_LIST();
                            primlistCommonQuadList.Assign(pPrimList3);
                        }
                        if (dstplist != null)
                        {
                            primlistCommonQuadList.pQuadList = new ushort[pPrimList3.nIndexSetSize * pPrimList3.nQuad];
                            Array.Copy(pPrimList3.pQuadList, primlistCommonQuadList.pQuadList, pPrimList3.nIndexSetSize * pPrimList3.nQuad);
                            continue;
                        }
                        continue;
                    case 1048576:
                        NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST pPrimList4 = (NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST)srcplist[index1].pPrimList;
                        NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST triangleQuadList = null;
                        if (dstplist != null)
                        {
                            dstplist[index1].pPrimList = triangleQuadList = new NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST();
                            triangleQuadList.Assign(pPrimList4);
                        }
                        if (dstplist != null)
                        {
                            triangleQuadList.pTriangleList = new ushort[pPrimList4.nIndexSetSize * pPrimList4.nTriangle];
                            Array.Copy(pPrimList4.pTriangleList, triangleQuadList.pTriangleList, pPrimList4.nIndexSetSize * pPrimList4.nTriangle);
                        }
                        if (dstplist != null)
                        {
                            triangleQuadList.pQuadList = new ushort[pPrimList4.nIndexSetSize * pPrimList4.nQuad];
                            Array.Copy(pPrimList4.pQuadList, triangleQuadList.pQuadList, pPrimList4.nIndexSetSize * pPrimList4.nQuad);
                            continue;
                        }
                        continue;
                    default:
                        continue;
                }
            }
            else
                NNM_ASSERT(0, "Unknown primitive foramt.\n");
        }
        return 0;
    }

    private static uint nnCopySubobjList(
      NNS_SUBOBJ[] pSubobjListDst,
      NNS_SUBOBJ[] pSubobjListSrc,
      int nSubobj,
      uint flag)
    {
        if (pSubobjListDst != null)
        {
            for (int index = 0; index < nSubobj; ++index)
                pSubobjListDst[index].Assign(pSubobjListSrc[index]);
        }
        for (int index1 = 0; index1 < nSubobj; ++index1)
        {
            NNS_SUBOBJ nnsSubobj1 = pSubobjListSrc[index1];
            NNS_SUBOBJ nnsSubobj2 = null;
            if (pSubobjListDst != null)
            {
                nnsSubobj2 = pSubobjListDst[index1];
                nnsSubobj2.pMeshsetList = new NNS_MESHSET[nnsSubobj1.nMeshset];
                for (int index2 = 0; index2 < nnsSubobj1.nMeshset; ++index2)
                    nnsSubobj2.pMeshsetList[index2] = new NNS_MESHSET(nnsSubobj1.pMeshsetList[index2]);
            }
            if (pSubobjListDst != null && nnsSubobj1.nTex != 0)
            {
                nnsSubobj2.pTexNumList = new int[nnsSubobj1.nTex];
                Array.Copy(nnsSubobj1.pTexNumList, nnsSubobj2.pTexNumList, nnsSubobj1.nTex);
            }
        }
        return 0;
    }

    private static uint nnCopyObject(NNS_OBJECT dstobj, NNS_OBJECT srcobj, uint flag)
    {
        if (dstobj != null)
        {
            dstobj.Assign(srcobj);
            dstobj.fType |= 128U;
        }
        if (dstobj != null)
        {
            dstobj.pMatPtrList = New<NNS_MATERIALPTR>(srcobj.pMatPtrList.Length);
            int num = (int)nnCopyMaterialList(dstobj.pMatPtrList, srcobj.pMatPtrList, srcobj.nMaterial, flag);
        }
        else
        {
            int num1 = (int)nnCopyMaterialList(null, srcobj.pMatPtrList, srcobj.nMaterial, flag);
        }
        if (dstobj != null)
        {
            dstobj.pVtxListPtrList = New<NNS_VTXLISTPTR>(srcobj.pVtxListPtrList.Length);
            int num2 = (int)nnCopyVertexList(dstobj.pVtxListPtrList, srcobj.pVtxListPtrList, srcobj.nVtxList, flag);
        }
        else
        {
            int num3 = (int)nnCopyVertexList(null, srcobj.pVtxListPtrList, srcobj.nVtxList, flag);
        }
        if (dstobj != null)
        {
            dstobj.pPrimListPtrList = New<NNS_PRIMLISTPTR>(srcobj.pPrimListPtrList.Length);
            int num2 = (int)nnCopyPrimitiveList(dstobj.pPrimListPtrList, srcobj.pPrimListPtrList, srcobj.nPrimList, flag);
        }
        else
        {
            int num4 = (int)nnCopyPrimitiveList(null, srcobj.pPrimListPtrList, srcobj.nPrimList, flag);
        }
        if (dstobj != null)
        {
            dstobj.pNodeList = New<NNS_NODE>(srcobj.nNode);
            for (int index = 0; index < srcobj.nNode; ++index)
                dstobj.pNodeList[index].Assign(srcobj.pNodeList[index]);
        }
        if (dstobj != null)
        {
            dstobj.pSubobjList = New<NNS_SUBOBJ>(srcobj.pSubobjList.Length);
            int num2 = (int)nnCopySubobjList(dstobj.pSubobjList, srcobj.pSubobjList, srcobj.nSubobj, flag);
        }
        else
        {
            int num5 = (int)nnCopySubobjList(null, srcobj.pSubobjList, srcobj.nSubobj, flag);
        }
        return 0;
    }

    private uint nnCalcMorphObjectBufferSize(
      ref NNS_OBJECT obj,
      ref NNS_MORPHTARGETLIST mtgt,
      uint flag)
    {
        mppAssertNotImpl();
        return 0;
    }

    private uint nnInitMorphObject(
      ref NNS_OBJECT mobj,
      ref NNS_OBJECT obj,
      ref NNS_MORPHTARGETLIST mtgt,
      uint flag)
    {
        mppAssertNotImpl();
        return 0;
    }

    private void nnDrawMorphObject(
      ref NNS_OBJECT mobj,
      ref NNS_MATRIX mtxpal,
      ref uint nodestatlist,
      uint subobjtype,
      uint flag)
    {
        mppAssertNotImpl();
    }

    private static void nnMakeUnitMatrix(ref SNNS_MATRIX dst)
    {
        dst.M00 = 1f;
        dst.M01 = 0.0f;
        dst.M02 = 0.0f;
        dst.M03 = 0.0f;
        dst.M10 = 0.0f;
        dst.M11 = 1f;
        dst.M12 = 0.0f;
        dst.M13 = 0.0f;
        dst.M20 = 0.0f;
        dst.M21 = 0.0f;
        dst.M22 = 1f;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    private static void nnMakeUnitMatrix(NNS_MATRIX dst)
    {
        dst.M00 = 1f;
        dst.M01 = 0.0f;
        dst.M02 = 0.0f;
        dst.M03 = 0.0f;
        dst.M10 = 0.0f;
        dst.M11 = 1f;
        dst.M12 = 0.0f;
        dst.M13 = 0.0f;
        dst.M20 = 0.0f;
        dst.M21 = 0.0f;
        dst.M22 = 1f;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    private static void nnMakeQuaternionMatrix(
      out SNNS_MATRIX dst,
      ref NNS_QUATERNION quat)
    {
        float x = quat.x;
        float y = quat.y;
        float z = quat.z;
        float w = quat.w;
        float num1 = x * x;
        float num2 = y * y;
        float num3 = z * z;
        float num4 = w * w;
        float num5 = x * y;
        float num6 = y * z;
        float num7 = x * z;
        float num8 = x * w;
        float num9 = y * w;
        float num10 = z * w;
        float num11 = num4 - num1;
        float num12 = num2 - num3;
        dst.M00 = num4 + num1 - num2 - num3;
        dst.M01 = (float)(2.0 * (num5 - (double)num10));
        dst.M02 = (float)(2.0 * (num7 + (double)num9));
        dst.M10 = (float)(2.0 * (num5 + (double)num10));
        dst.M11 = num11 + num12;
        dst.M12 = (float)(2.0 * (num6 - (double)num8));
        dst.M20 = (float)(2.0 * (num7 - (double)num9));
        dst.M21 = (float)(2.0 * (num6 + (double)num8));
        dst.M22 = num11 - num12;
        dst.M03 = 0.0f;
        dst.M13 = 0.0f;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    private static void nnMakeQuaternionMatrix(
      NNS_MATRIX dst,
      ref NNS_QUATERNION quat)
    {
        float x = quat.x;
        float y = quat.y;
        float z = quat.z;
        float w = quat.w;
        float num1 = x * x;
        float num2 = y * y;
        float num3 = z * z;
        float num4 = w * w;
        float num5 = x * y;
        float num6 = y * z;
        float num7 = x * z;
        float num8 = x * w;
        float num9 = y * w;
        float num10 = z * w;
        float num11 = num4 - num1;
        float num12 = num2 - num3;
        dst.M00 = num4 + num1 - num2 - num3;
        dst.M01 = (float)(2.0 * (num5 - (double)num10));
        dst.M02 = (float)(2.0 * (num7 + (double)num9));
        dst.M10 = (float)(2.0 * (num5 + (double)num10));
        dst.M11 = num11 + num12;
        dst.M12 = (float)(2.0 * (num6 - (double)num8));
        dst.M20 = (float)(2.0 * (num7 - (double)num9));
        dst.M21 = (float)(2.0 * (num6 + (double)num8));
        dst.M22 = num11 - num12;
        dst.M03 = 0.0f;
        dst.M13 = 0.0f;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMakeRotateXMatrix(NNS_MATRIX dst, int ax)
    {
        float s;
        float c;
        nnSinCos(ax, out s, out c);
        dst.M00 = 1f;
        dst.M01 = 0.0f;
        dst.M02 = 0.0f;
        dst.M03 = 0.0f;
        dst.M10 = 0.0f;
        dst.M11 = c;
        dst.M12 = -s;
        dst.M13 = 0.0f;
        dst.M20 = 0.0f;
        dst.M21 = s;
        dst.M22 = c;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMakeRotateXMatrix(out SNNS_MATRIX dst, int ax)
    {
        float s;
        float c;
        nnSinCos(ax, out s, out c);
        dst.M00 = 1f;
        dst.M01 = 0.0f;
        dst.M02 = 0.0f;
        dst.M03 = 0.0f;
        dst.M10 = 0.0f;
        dst.M11 = c;
        dst.M12 = -s;
        dst.M13 = 0.0f;
        dst.M20 = 0.0f;
        dst.M21 = s;
        dst.M22 = c;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMakeRotateYMatrix(NNS_MATRIX dst, int ay)
    {
        float s;
        float c;
        nnSinCos(ay, out s, out c);
        dst.M00 = c;
        dst.M01 = 0.0f;
        dst.M02 = s;
        dst.M03 = 0.0f;
        dst.M10 = 0.0f;
        dst.M11 = 1f;
        dst.M12 = 0.0f;
        dst.M13 = 0.0f;
        dst.M20 = -s;
        dst.M21 = 0.0f;
        dst.M22 = c;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMakeRotateZMatrix(out SNNS_MATRIX dst, int az)
    {
        float s;
        float c;
        nnSinCos(az, out s, out c);
        dst.M00 = c;
        dst.M01 = -s;
        dst.M02 = 0.0f;
        dst.M03 = 0.0f;
        dst.M10 = s;
        dst.M11 = c;
        dst.M12 = 0.0f;
        dst.M13 = 0.0f;
        dst.M20 = 0.0f;
        dst.M21 = 0.0f;
        dst.M22 = 1f;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMakeRotateZMatrix(NNS_MATRIX dst, int az)
    {
        float s;
        float c;
        nnSinCos(az, out s, out c);
        dst.M00 = c;
        dst.M01 = -s;
        dst.M02 = 0.0f;
        dst.M03 = 0.0f;
        dst.M10 = s;
        dst.M11 = c;
        dst.M12 = 0.0f;
        dst.M13 = 0.0f;
        dst.M20 = 0.0f;
        dst.M21 = 0.0f;
        dst.M22 = 1f;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMakeRotateXYZMatrix(NNS_MATRIX dst, int ax, int ay, int az)
    {
        nnMakeRotateZMatrix(dst, az);
        nnRotateYMatrix(dst, dst, ay);
        nnRotateXMatrix(dst, dst, ax);
    }

    public static void nnMakeRotateXZYMatrix(NNS_MATRIX dst, int ax, int ay, int az)
    {
        nnMakeRotateYMatrix(dst, ay);
        nnRotateZMatrix(dst, dst, az);
        nnRotateXMatrix(dst, dst, ax);
    }

    public static void nnMakeRotateZXYMatrix(NNS_MATRIX dst, int ax, int ay, int az)
    {
        nnMakeRotateYMatrix(dst, ay);
        nnRotateXMatrix(dst, dst, ax);
        nnRotateZMatrix(dst, dst, az);
    }

    public static void nnMakeScaleMatrix(out SNNS_MATRIX dst, float x, float y, float z)
    {
        dst.M00 = x;
        dst.M01 = 0.0f;
        dst.M02 = 0.0f;
        dst.M03 = 0.0f;
        dst.M10 = 0.0f;
        dst.M11 = y;
        dst.M12 = 0.0f;
        dst.M13 = 0.0f;
        dst.M20 = 0.0f;
        dst.M21 = 0.0f;
        dst.M22 = z;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMakeScaleMatrix(NNS_MATRIX dst, float x, float y, float z)
    {
        dst.M00 = x;
        dst.M01 = 0.0f;
        dst.M02 = 0.0f;
        dst.M03 = 0.0f;
        dst.M10 = 0.0f;
        dst.M11 = y;
        dst.M12 = 0.0f;
        dst.M13 = 0.0f;
        dst.M20 = 0.0f;
        dst.M21 = 0.0f;
        dst.M22 = z;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    private static void nnMakeTranslateMatrix(
      out SNNS_MATRIX dst,
      float x,
      float y,
      float z)
    {
        dst.M00 = 1f;
        dst.M01 = 0.0f;
        dst.M02 = 0.0f;
        dst.M03 = x;
        dst.M10 = 0.0f;
        dst.M11 = 1f;
        dst.M12 = 0.0f;
        dst.M13 = y;
        dst.M20 = 0.0f;
        dst.M21 = 0.0f;
        dst.M22 = 1f;
        dst.M23 = z;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    private static void nnMakeTranslateMatrix(NNS_MATRIX dst, float x, float y, float z)
    {
        dst.M00 = 1f;
        dst.M01 = 0.0f;
        dst.M02 = 0.0f;
        dst.M03 = x;
        dst.M10 = 0.0f;
        dst.M11 = 1f;
        dst.M12 = 0.0f;
        dst.M13 = y;
        dst.M20 = 0.0f;
        dst.M21 = 0.0f;
        dst.M22 = 1f;
        dst.M23 = z;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMakePerspectiveMatrix(
      NNS_MATRIX mtx,
      int fovy,
      float aspect,
      float znear,
      float zfar)
    {
        double num = 1.0 / nnTan(fovy >> 1);
        mtx.M00 = (float)num / aspect;
        mtx.M01 = 0.0f;
        mtx.M02 = 0.0f;
        mtx.M03 = 0.0f;
        mtx.M10 = 0.0f;
        mtx.M11 = (float)num;
        mtx.M12 = 0.0f;
        mtx.M13 = 0.0f;
        mtx.M20 = 0.0f;
        mtx.M21 = 0.0f;
        mtx.M22 = (float)(-znear / (zfar - (double)znear));
        mtx.M23 = (float)(-(zfar * (double)znear) / (zfar - (double)znear));
        mtx.M30 = 0.0f;
        mtx.M31 = 0.0f;
        mtx.M32 = -1f;
        mtx.M33 = 0.0f;
    }

    public static void nnMakePerspectiveOffCenterMatrix(
      NNS_MATRIX mtx,
      float left,
      float right,
      float bottom,
      float top,
      float znear,
      float zfar)
    {
        mtx.M00 = (float)(2.0 * znear / (right - (double)left));
        mtx.M01 = 0.0f;
        mtx.M02 = (float)((right + (double)left) / (right - (double)left));
        mtx.M03 = 0.0f;
        mtx.M10 = 0.0f;
        mtx.M11 = (float)(2.0 * znear / (top - (double)bottom));
        mtx.M12 = (float)((top + (double)bottom) / (top - (double)bottom));
        mtx.M13 = 0.0f;
        mtx.M20 = 0.0f;
        mtx.M21 = 0.0f;
        mtx.M22 = (float)(-znear / (zfar - (double)znear));
        mtx.M23 = (float)(-(zfar * (double)znear) / (zfar - (double)znear));
        mtx.M30 = 0.0f;
        mtx.M31 = 0.0f;
        mtx.M32 = -1f;
        mtx.M33 = 0.0f;
    }

    public static void nnMakeOrthoMatrix(
      NNS_MATRIX mtx,
      float left,
      float right,
      float bottom,
      float top,
      float znear,
      float zfar)
    {
        mtx.M00 = (float)(2.0 / (right - (double)left));
        mtx.M01 = 0.0f;
        mtx.M02 = 0.0f;
        mtx.M03 = (float)(-(right + (double)left) / (right - (double)left));
        mtx.M10 = 0.0f;
        mtx.M11 = (float)(2.0 / (top - (double)bottom));
        mtx.M12 = 0.0f;
        mtx.M13 = (float)(-(top + (double)bottom) / (top - (double)bottom));
        mtx.M20 = 0.0f;
        mtx.M21 = 0.0f;
        mtx.M22 = (float)(-1.0 / (zfar - (double)znear));
        mtx.M23 = (float)(-zfar / (zfar - (double)znear));
        mtx.M30 = 0.0f;
        mtx.M31 = 0.0f;
        mtx.M32 = 0.0f;
        mtx.M33 = 1f;
    }
    private static void nnCalcMatrixPaletteMotion(
     NNS_MATRIX[] mtxpal,
     uint[] nodestatlist,
     NNS_OBJECT obj,
     NNS_MOTION mot,
     float frame,
     NNS_MATRIX basemtx,
     NNS_MATRIXSTACK mstk,
     uint flag)
    {
        if (((int)mot.fType & 1) == 0)
            return;
        nnsSubMotIdx = 0;
        float dstframe;
        int num = nnCalcMotionFrame(out dstframe, mot.fType, mot.StartFrame, mot.EndFrame, frame);
        nnsMtxPal = mtxpal;
        nnsNodeStatList = nodestatlist;
        nnsNSFlag = flag;
        nnsObj = obj;
        nnsNodeList = obj.pNodeList;
        nnsMstk = mstk;
        nnsMot = mot;
        nnsFrame = dstframe;
        if (num != 0)
        {
            nnsBaseMtx = basemtx == null ? nngUnitMatrix : basemtx;
            nnSetCurrentMatrix(mstk, nnsBaseMtx);
            nnCalcMatrixPaletteMotionNode(0);
        }
        else
            nnCalcMatrixPalette(mtxpal, nodestatlist, obj, basemtx, mstk, nnsNSFlag);
    }

    private static int nnCalcMotionFrame(
      out float dstframe,
      uint fType,
      float startframe,
      float endframe,
      float frame)
    {
        if (((int)fType & 64) != 0)
        {
            dstframe = frame;
            return 1;
        }
        switch ((uint)((int)fType & 2031680 & -65))
        {
            case 65536:
                if (startframe <= (double)frame && frame < (double)endframe)
                {
                    dstframe = frame;
                    return 1;
                }
                dstframe = frame;
                return 0;
            case 131072:
                dstframe = startframe <= (double)frame ? (endframe > (double)frame ? frame : endframe) : startframe;
                return 1;
            case 262144:
                if (startframe <= (double)frame && frame < (double)endframe)
                {
                    dstframe = frame;
                }
                else
                {
                    float num1 = frame - startframe;
                    float num2 = endframe - startframe;
                    float num3 = num1 / num2;
                    int num4 = (int)num3;
                    if (num3 < 0.0)
                        --num4;
                    float num5 = num4;
                    dstframe = frame - num2 * num5;
                }
                return 1;
            case 524288:
                float num6 = frame - startframe;
                float num7 = endframe - startframe;
                float num8 = num6 / num7;
                int num9 = (int)num8;
                if (num8 < 0.0)
                    --num9;
                float num10 = num9;
                dstframe = (num9 & 1) == 0 ? frame - num7 * num10 : (float)(startframe - (double)num6 + num7 * (num10 + 1.0));
                return 1;
            default:
                dstframe = frame;
                return 0;
        }
    }

    private static void nnCalcMatrixPaletteMotionNode(int nodeIdx)
    {
        int? pHideFlag = new int?(0);
        NNS_NODE nnsNode;
        do
        {
            nnsNode = nncalcmatrixpalettemotion.nnsNodeList[nodeIdx];
            if (((int)nnsNode.fType & 134217728) != 0)
            {
                if (((int)nnsNode.fType & 100663296) != 0)
                {
                    nnPushMatrix(nncalcmatrixpalettemotion.nnsMstk, null);
                    if (((int)nnsNode.fType & 33554432) != 0)
                        nnCalcMatrixPaletteMotionNode1BoneXSIIK(nodeIdx);
                    else if (((int)nnsNode.fType & 67108864) != 0)
                        nnCalcMatrixPaletteMotionNode2BoneXSIIK(nodeIdx);
                    nnPopMatrix(nncalcmatrixpalettemotion.nnsMstk);
                    nodeIdx = nnsNode.iSibling;
                    goto label_25;
                }
            }
            else
            {
                if (((int)nnsNode.fType & 16384) != 0)
                {
                    nnPushMatrix(nncalcmatrixpalettemotion.nnsMstk, null);
                    nnCalcMatrixPaletteMotionNode1BoneSIIK(nodeIdx);
                    nnPopMatrix(nncalcmatrixpalettemotion.nnsMstk);
                    break;
                }
                if (((int)nnsNode.fType & 32768) != 0)
                {
                    nnPushMatrix(nncalcmatrixpalettemotion.nnsMstk, null);
                    nnCalcMatrixPaletteMotionNode2BoneSIIK(nodeIdx);
                    nnPopMatrix(nncalcmatrixpalettemotion.nnsMstk);
                    break;
                }
            }
            nnPushMatrix(nncalcmatrixpalettemotion.nnsMstk, null);
            NNS_MATRIX currentMatrix = nnGetCurrentMatrix(nncalcmatrixpalettemotion.nnsMstk);
            nncalcmatrixpalettemotion.nnsSubMotIdx = nnCalcNodeMotionCore(currentMatrix, ref pHideFlag, nncalcmatrixpalettemotion.nnsBaseMtx, nnsNode, nodeIdx, nncalcmatrixpalettemotion.nnsObj, nncalcmatrixpalettemotion.nnsMot, nncalcmatrixpalettemotion.nnsSubMotIdx, nncalcmatrixpalettemotion.nnsFrame);
            if (nnsNode.iMatrix != -1)
            {
                if (((int)nnsNode.fType & 8) != 0)
                    nnCopyMatrix(nncalcmatrixpalettemotion.nnsMtxPal[nnsNode.iMatrix], currentMatrix);
                else
                    nnMultiplyMatrix(nncalcmatrixpalettemotion.nnsMtxPal[nnsNode.iMatrix], currentMatrix, nnsNode.InvInitMtx);
            }
            if (nncalcmatrixpalettemotion.nnsNodeStatList != null)
            {
                if (nodeIdx == 0 && nncalcmatrixpalettemotion.nnsNSFlag != 0U)
                    nncalcmatrixpalettemotion.nnsRootScale = nnEstimateMatrixScaling(currentMatrix);
                int? nullable = pHideFlag;
                if ((nullable.GetValueOrDefault() != 0 ? 1 : (!nullable.HasValue ? 1 : 0)) != 0)
                    nncalcmatrixpalettemotion.nnsNodeStatList[nodeIdx] |= 1U;
                nnCalcClipSetNodeStatus(nncalcmatrixpalettemotion.nnsNodeStatList, nncalcmatrixpalettemotion.nnsNodeList, nodeIdx, currentMatrix, nncalcmatrixpalettemotion.nnsRootScale, nncalcmatrixpalettemotion.nnsNSFlag);
            }
            if (nnsNode.iChild != -1)
                nnCalcMatrixPaletteMotionNode(nnsNode.iChild);
            nnPopMatrix(nncalcmatrixpalettemotion.nnsMstk);
            nodeIdx = nnsNode.iSibling;
        label_25:;
        }
        while (nnsNode.iSibling != -1);
    }

    private static int nnCalcNodeMotionCore(
      NNS_MATRIX pNodeMtx,
      ref int? pHideFlag,
      NNS_MATRIX pBaseMtx,
      NNS_NODE pNode,
      int NodeIdx,
      NNS_OBJECT pObj,
      NNS_MOTION pMot,
      int SubMotIdx,
      float frame)
    {
        NNS_VECTOR tv = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_ROTATE_A32 rv = new NNS_ROTATE_A32();
        NNS_QUATERNION quat = new NNS_QUATERNION();
        NNS_VECTOR sv = GlobalPool<NNS_VECTOR>.Alloc();
        uint fType1 = pNode.fType;
        uint rtype = pNode.fType & 3840U;
        tv.Assign(pNode.Translation);
        sv.Assign(pNode.Scaling);
        if (((int)fType1 & 114688) != 0)
        {
            rv.x = rv.y = rv.z = 0;
        }
        else
        {
            rv.x = pNode.Rotation.x;
            rv.y = pNode.Rotation.y;
            rv.z = pNode.Rotation.z;
        }
        int num1 = ((int)fType1 & 1) != 0 ? 0 : 1;
        int num2 = ((int)fType1 & 2) != 0 ? 0 : 1;
        int num3 = ((int)fType1 & 4) != 0 ? 0 : 1;
        if (pHideFlag.HasValue)
            pHideFlag = new int?(0);
        for (int SubMotIdx1 = SubMotIdx; SubMotIdx1 < pMot.nSubmotion; ++SubMotIdx1)
        {
            NNS_SUBMOTION submot = pMot.pSubmotion[SubMotIdx1];
            if (NodeIdx < submot.Id)
            {
                SubMotIdx = SubMotIdx1;
                break;
            }
            if (NodeIdx == submot.Id && submot.fType != 0U && (submot.StartFrame <= (double)frame && frame <= (double)submot.EndFrame))
            {
                uint fType2 = submot.fIPType;
                if (((int)pMot.fType & 131072) != 0 && frame == (double)submot.EndFrame)
                    fType2 = 131072U;
                float dstframe;
                if (nnCalcMotionFrame(out dstframe, fType2, submot.StartKeyFrame, submot.EndKeyFrame, frame) != 0)
                {
                    if (((int)submot.fType & 30720) != 0)
                        num2 |= nnCalcMotionRotate(submot, dstframe, ref rv, quat, rtype);
                    else if (((int)submot.fType & 1792) != 0)
                        num1 |= nnCalcMotionTranslate(submot, dstframe, tv);
                    else if (((int)submot.fType & 229376) != 0)
                        num3 |= nnCalcMotionScale(submot, dstframe, sv);
                    else if (((int)submot.fType & 786432) != 0)
                        nnCallbackMotionUserData(pObj, pMot, SubMotIdx1, NodeIdx, dstframe, frame);
                    else if (((int)submot.fType & 1048576) != 0 && pHideFlag.HasValue)
                        pHideFlag = new int?(nnCalcMotionNodeHide(submot, dstframe));
                }
            }
        }
        if (num1 == 1)
        {
            if (((int)pNode.fType & 8192) != 0)
            {
                NNS_VECTORFAST dst = new NNS_VECTORFAST();
                nnmSetUpVectorFast(out dst, tv.x, tv.y, tv.z);
                nnTransformVectorFast(out dst, pBaseMtx, dst);
                nnCopyVectorFastMatrixTranslation(pNodeMtx, ref dst);
            }
            else
                nnTranslateMatrixFast(pNodeMtx, tv.x, tv.y, tv.z);
        }
        if (((int)pNode.fType & 1839104) != 0)
        {
            if (((int)pNode.fType & 4096) != 0)
                nnCopyMatrix33(pNodeMtx, pBaseMtx);
            if (((int)pNode.fType & 262144) != 0)
                nnNormalizeColumn0(pNodeMtx);
            if (((int)pNode.fType & 524288) != 0)
                nnNormalizeColumn1(pNodeMtx);
            if (((int)pNode.fType & 1048576) != 0)
                nnNormalizeColumn2(pNodeMtx);
        }
        if (num2 == 1)
        {
            switch (rtype)
            {
                case 0:
                    nnRotateXYZMatrix(pNodeMtx, pNodeMtx, rv.x, rv.y, rv.z);
                    break;
                case 256:
                    nnRotateXZYMatrixFast(pNodeMtx, rv.x, rv.y, rv.z);
                    break;
                case 1024:
                    nnRotateZXYMatrixFast(pNodeMtx, rv.x, rv.y, rv.z);
                    break;
                default:
                    nnRotateXYZMatrix(pNodeMtx, pNodeMtx, rv.x, rv.y, rv.z);
                    break;
            }
        }
        else if ((num2 & 2) != 0)
            nnQuaternionMatrix(pNodeMtx, pNodeMtx, ref quat);
        if (num3 == 1)
            nnScaleMatrixFast(pNodeMtx, sv.x, sv.y, sv.z);
        GlobalPool<NNS_VECTOR>.Release(tv);
        GlobalPool<NNS_VECTOR>.Release(sv);
        return SubMotIdx;
    }

    private static int nnCalcMotionRotate(
      NNS_SUBMOTION submot,
      float frame,
      ref NNS_ROTATE_A32 rv,
      NNS_QUATERNION rq,
      uint rtype)
    {
        int num1 = 0;
        NNS_ROTATE_A16 val = new NNS_ROTATE_A16();
        int[] arv = _nnCalcMotionRotate.arv;
        arv[0] = rv.x;
        arv[1] = rv.y;
        arv[2] = rv.z;
        short[] arsv = _nnCalcMotionRotate.arsv;
        arsv[0] = val.x;
        arsv[1] = val.y;
        arsv[2] = val.z;
        object pKeyList = submot.pKeyList;
        int nKeyFrame = submot.nKeyFrame;
        uint num2 = submot.fType & 30720U;
        uint num3 = submot.fType & 28U;
        int index = 0;
        switch (num2)
        {
            case 14336:
                if (num3 == 16U || pKeyList is NNS_MOTION_KEY_Class16[])
                {
                    switch (submot.fIPType & 3703U)
                    {
                        case 2:
                            nnInterpolateLinearA16_3((NNS_MOTION_KEY_Class16[])pKeyList, nKeyFrame, frame, ref val);
                            rv.x = val.x;
                            rv.y = val.y;
                            rv.z = val.z;
                            num1 = 1;
                            break;
                        case 4:
                            nnInterpolateConstantA16_3((NNS_MOTION_KEY_Class16[])pKeyList, nKeyFrame, frame, ref val);
                            rv.x = val.x;
                            rv.y = val.y;
                            rv.z = val.z;
                            num1 = 1;
                            break;
                        case 512:
                            nnInterpolateLerpA16_3((NNS_MOTION_KEY_Class16[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                        case 1024:
                            nnInterpolateSlerpA16_3((NNS_MOTION_KEY_Class16[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                        case 2048:
                            nnInterpolateSquadA16_3((NNS_MOTION_KEY_Class16[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                    }
                }
                else
                {
                    switch (submot.fIPType & 3703U)
                    {
                        case 2:
                            nnInterpolateLinearA32_3((NNS_MOTION_KEY_Class13[])pKeyList, nKeyFrame, frame, ref rv);
                            num1 = 1;
                            break;
                        case 4:
                            nnInterpolateConstantA32_3((NNS_MOTION_KEY_Class13[])pKeyList, nKeyFrame, frame, ref rv);
                            num1 = 1;
                            break;
                        case 512:
                            nnInterpolateLerpA32_3((NNS_MOTION_KEY_Class13[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                        case 1024:
                            nnInterpolateSlerpA32_3((NNS_MOTION_KEY_Class13[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                        case 2048:
                            nnInterpolateSquadA32_3((NNS_MOTION_KEY_Class13[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                    }
                }
                break;
            case 16384:
                switch (submot.fIPType & 3703U)
                {
                    case 4:
                        nnInterpolateConstantQuat_4((NNS_MOTION_KEY_Class7[])pKeyList, nKeyFrame, frame, ref rq);
                        num1 = 2;
                        break;
                    case 512:
                        nnInterpolateLerpQuat_4((NNS_MOTION_KEY_Class7[])pKeyList, nKeyFrame, frame, ref rq);
                        num1 = 2;
                        break;
                    case 1024:
                        nnInterpolateSlerpQuat_4((NNS_MOTION_KEY_Class7[])pKeyList, nKeyFrame, frame, ref rq);
                        num1 = 2;
                        break;
                    case 2048:
                        nnInterpolateSquadQuat_4((NNS_MOTION_KEY_Class7[])pKeyList, nKeyFrame, frame, ref rq);
                        num1 = 2;
                        break;
                }
                break;
            default:
                switch (num2)
                {
                    case 4096:
                        ++index;
                        num2 = 2048U;
                        break;
                    case 8192:
                        index += 2;
                        num2 = 2048U;
                        break;
                }
                if (2048U == num2)
                {
                    if (num3 == 16U)
                    {
                        num1 = 1;
                        switch (submot.fIPType & 3703U)
                        {
                            case 2:
                                nnInterpolateLinearA16_1((NNS_MOTION_KEY_Class14[])pKeyList, nKeyFrame, frame, out arsv[index]);
                                arv[index] = arsv[index];
                                break;
                            case 4:
                                nnInterpolateConstantA16_1((NNS_MOTION_KEY_Class14[])pKeyList, nKeyFrame, frame, out arsv[index]);
                                arv[index] = arsv[index];
                                break;
                            case 32:
                                nnInterpolateSISplineA16_1((NNS_MOTION_KEY_Class15[])pKeyList, nKeyFrame, frame, out arsv[index]);
                                arv[index] = arsv[index];
                                break;
                        }
                    }
                    else
                    {
                        switch (submot.fIPType & 3703U)
                        {
                            case 2:
                                nnInterpolateLinearA32_1((NNS_MOTION_KEY_Class8[])pKeyList, nKeyFrame, frame, out arv[index]);
                                num1 = 1;
                                break;
                            case 4:
                                nnInterpolateConstantA32_1((NNS_MOTION_KEY_Class8[])pKeyList, nKeyFrame, frame, out arv[index]);
                                num1 = 1;
                                break;
                            case 16:
                                nnInterpolateBezierA32_1((NNS_MOTION_KEY_Class9[])pKeyList, nKeyFrame, frame, out arv[index]);
                                num1 = 1;
                                break;
                            case 32:
                                nnInterpolateSISplineA32_1((NNS_MOTION_KEY_Class10[])pKeyList, nKeyFrame, frame, out arv[index]);
                                num1 = 1;
                                break;
                        }
                    }
                }
                rv.x = arv[0];
                rv.y = arv[1];
                rv.z = arv[2];
                break;
        }
        return num1;
    }

    private static void nnRotateXYZMatrixFast(NNS_MATRIX mtx, int ax, int ay, int az)
    {
        nnmRotateMatrixFast(mtx, az, 0, 1);
        nnmRotateMatrixFast(mtx, ay, 2, 0);
        nnmRotateMatrixFast(mtx, ax, 1, 2);
    }

    private static void nnRotateXZYMatrixFast(NNS_MATRIX mtx, int ax, int ay, int az)
    {
        nnmRotateMatrixFast(mtx, ay, 2, 0);
        nnmRotateMatrixFast(mtx, az, 0, 1);
        nnmRotateMatrixFast(mtx, ax, 1, 2);
    }

    private static void nnRotateZXYMatrixFast(NNS_MATRIX mtx, int ax, int ay, int az)
    {
        nnmRotateMatrixFast(mtx, ay, 2, 0);
        nnmRotateMatrixFast(mtx, ax, 1, 2);
        nnmRotateMatrixFast(mtx, az, 0, 1);
    }

    private static int nnCalcMotionTranslate(
      NNS_SUBMOTION submot,
      float frame,
      NNS_VECTOR tv)
    {
        int num1 = 0;
        float val = 0.0f;
        bool flag = false;
        uint num2 = submot.fType & 1792U;
        object pKeyList = submot.pKeyList;
        int nKeyFrame = submot.nKeyFrame;
        if (num2 == 1792U)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    nnInterpolateLinearF3((NNS_MOTION_KEY_Class5[])pKeyList, nKeyFrame, frame, tv);
                    num1 = 1;
                    break;
                case 4:
                    nnInterpolateConstantF3((NNS_MOTION_KEY_Class5[])pKeyList, nKeyFrame, frame, tv);
                    num1 = 1;
                    break;
            }
        }
        else
        {
            switch (num2)
            {
                case 256:
                    flag = true;
                    val = tv.x;
                    break;
                case 512:
                    flag = true;
                    val = tv.y;
                    break;
                case 1024:
                    flag = true;
                    val = tv.z;
                    break;
            }
            if (flag)
            {
                switch (submot.fIPType & 3703U)
                {
                    case 2:
                        nnInterpolateLinearF1((NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 4:
                        nnInterpolateConstantF1((NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 16:
                        nnInterpolateBezierF1((NNS_MOTION_KEY_Class2[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 32:
                        nnInterpolateSISplineF1((NNS_MOTION_KEY_Class3[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                }
                switch (num2)
                {
                    case 256:
                        tv.x = val;
                        break;
                    case 512:
                        tv.y = val;
                        break;
                    case 1024:
                        tv.z = val;
                        break;
                }
            }
        }
        return num1;
    }

    private static void nnTranslateMatrixFast(NNS_MATRIX mtx, float x, float y, float z)
    {
        mtx.M03 = (float)(mtx.M00 * (double)x + mtx.M01 * (double)y + mtx.M02 * (double)z) + mtx.M03;
        mtx.M13 = (float)(mtx.M10 * (double)x + mtx.M11 * (double)y + mtx.M12 * (double)z) + mtx.M13;
        mtx.M23 = (float)(mtx.M20 * (double)x + mtx.M21 * (double)y + mtx.M22 * (double)z) + mtx.M23;
    }

    private static int nnCalcMotionScale(
      NNS_SUBMOTION submot,
      float frame,
      NNS_VECTOR sv)
    {
        int num1 = 0;
        float val = 0.0f;
        bool flag = false;
        uint num2 = submot.fType & 229376U;
        object pKeyList = submot.pKeyList;
        int nKeyFrame = submot.nKeyFrame;
        if (num2 == 229376U)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    nnInterpolateLinearF3((NNS_MOTION_KEY_Class5[])pKeyList, nKeyFrame, frame, sv);
                    num1 = 1;
                    break;
                case 4:
                    nnInterpolateConstantF3((NNS_MOTION_KEY_Class5[])pKeyList, nKeyFrame, frame, sv);
                    num1 = 1;
                    break;
            }
        }
        else
        {
            switch (num2)
            {
                case 32768:
                    flag = true;
                    val = sv.x;
                    break;
                case 65536:
                    flag = true;
                    val = sv.y;
                    break;
                case 131072:
                    flag = true;
                    val = sv.z;
                    break;
            }
            if (flag)
            {
                switch (submot.fIPType & 3703U)
                {
                    case 2:
                        nnInterpolateLinearF1((NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 4:
                        nnInterpolateConstantF1((NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 16:
                        nnInterpolateBezierF1((NNS_MOTION_KEY_Class2[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 32:
                        nnInterpolateSISplineF1((NNS_MOTION_KEY_Class3[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                }
                switch (num2)
                {
                    case 32768:
                        sv.x = val;
                        break;
                    case 65536:
                        sv.y = val;
                        break;
                    case 131072:
                        sv.z = val;
                        break;
                }
            }
        }
        return num1;
    }

    private static void nnScaleMatrixFast(NNS_MATRIX mtx, float x, float y, float z)
    {
        mtx.M00 *= x;
        mtx.M01 *= y;
        mtx.M02 *= z;
        mtx.M10 *= x;
        mtx.M11 *= y;
        mtx.M12 *= z;
        mtx.M20 *= x;
        mtx.M21 *= y;
        mtx.M22 *= z;
    }

    private static void nnCalcMatrixPaletteMotionNode2BoneSIIK(int jnt1nodeIdx)
    {
        mppAssertNotImpl();

        NNS_NODE[] iVar1;
        NNS_MATRIX uVar2;
        float uVar3;
        NNS_NODE iVar4;
        int iVar5;
        NNS_NODE iVar6;
        float uVar7;
        int iVar8;
        NNS_NODE iVar9;

        NNS_MATRIX NStack_170 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX NStack_130 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX auStack_f0 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX NStack_b0 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX auStack_70 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_VECTORFAST auStack_30 = default;

        int? x = null;

        iVar1 = nnsNodeList;
        iVar4 = nnsNodeList[jnt1nodeIdx];
        iVar5 = iVar4.iChild;
        iVar6 = nnsNodeList[iVar5];
        iVar8 = iVar6.iChild;
        iVar9 = nnsNodeList[iVar8];

        // educated guess, currently never assigned (figure out how this is read)
        uVar3 = iVar4.SIIKBoneLength;
        uVar7 = iVar6.SIIKBoneLength;
        uVar2 = nnGetCurrentMatrix(nnsMstk);
        nnCopyMatrix(auStack_70, uVar2);
        nnMakeUnitMatrix(NStack_b0);
        nnsSubMotIdx = nnCalcNodeMotionCore(NStack_b0, ref x, NStack_b0, iVar4, jnt1nodeIdx, nnsObj, nnsMot, nnsSubMotIdx, nnsFrame);
        nnMakeUnitMatrix(NStack_130);
        nnsSubMotIdx = nnCalcNodeMotionCore(NStack_130, ref x, NStack_130, iVar6, iVar5, nnsObj, nnsMot, nnsSubMotIdx, nnsFrame);
        nnMakeUnitMatrix(NStack_170);
        nnsSubMotIdx = nnCalcNodeMotionCore(NStack_170, ref x, NStack_170, iVar9, iVar8, nnsObj, nnsMot, nnsSubMotIdx, nnsFrame);
        nnCopyMatrixTranslationVectorFast(out auStack_30, NStack_170);
        nnTransformVectorFast(out auStack_30, nnsBaseMtx, auStack_30);
        nnCopyVectorFastMatrixTranslation(NStack_170, ref auStack_30);
        nnCalc2BoneSIIK(auStack_70, NStack_b0, auStack_f0, NStack_130, NStack_170, uVar3, uVar7, (int)(iVar6.fType >> 0x11 & 1));
        if ((iVar9.fType & 0x1000) != 0)
        {
            nnCopyMatrix33(NStack_170, nnsBaseMtx);
        }

        // InvInitMtx is a guess.
        if (iVar4.iMatrix != -1)
        {
            nnMultiplyMatrix(nnsMtxPal[iVar4.iMatrix], auStack_70, iVar4.InvInitMtx);
        }
        if (iVar6.iMatrix != -1)
        {
            nnMultiplyMatrix(nnsMtxPal[iVar6.iMatrix], auStack_f0, iVar6.InvInitMtx);
        }
        if (iVar9.iMatrix != -1)
        {
            nnMultiplyMatrix(nnsMtxPal[iVar9.iMatrix], NStack_170, iVar9.InvInitMtx);
        }
        if (nnsNodeStatList != null)
        {
            nnCalcClipSetNodeStatus
                      (nnsNodeStatList, nnsNodeList, jnt1nodeIdx, auStack_70, nnsRootScale, nnsNSFlag);
            nnCalcClipSetNodeStatus
                      (nnsNodeStatList, nnsNodeList, iVar5, auStack_f0, nnsRootScale, nnsNSFlag);
            nnCalcClipSetNodeStatus
                      (nnsNodeStatList, nnsNodeList, iVar8, NStack_170, nnsRootScale, nnsNSFlag);
        }
        if (iVar9.iChild != -1)
        {
            nnPushMatrix(nnsMstk, NStack_170);
            nnCalcMatrixPaletteMotionNode(iVar9.iChild);
            nnPopMatrix(nnsMstk);
        }
        if (iVar9.iSibling != -1)
        {
            nnPushMatrix(nnsMstk, auStack_f0);
            nnCalcMatrixPaletteMotionNode(iVar9.iSibling);
            nnPopMatrix(nnsMstk);
        }
        if (iVar6.iSibling != -1)
        {
            nnPushMatrix(nnsMstk, auStack_70);
            nnCalcMatrixPaletteMotionNode(iVar6.iSibling);
            nnPopMatrix(nnsMstk);
        }
        if (iVar6.iSibling != -1)
        {
            nnPushMatrix(nnsMstk, null);
            nnCalcMatrixPaletteMotionNode(iVar6.iSibling);
            nnPopMatrix(nnsMstk);
        }
        return;
    }

    private static void nnCalcMatrixPaletteMotionNode1BoneSIIK(int jnt1nodeIdx)
    {
        mppAssertNotImpl();
    }

    private static void nnCalcMatrixPaletteMotionNode2BoneXSIIK(int rootidx)
    {
        mppAssertNotImpl();
    }

    private static void nnCalcMatrixPaletteMotionNode1BoneXSIIK(int rootidx)
    {
        mppAssertNotImpl();
    }

    private static void nnCallbackMotionUserData(
      NNS_OBJECT obj,
      NNS_MOTION mot,
      int SubMotIdx,
      int NodeIdx,
      float nframe,
      float origframe)
    {
        NNS_NODEUSRMOT_CALLBACK_VAL val = new NNS_NODEUSRMOT_CALLBACK_VAL();
        val.iNode = NodeIdx;
        val.Frame = origframe;
        val.pMotion = mot;
        val.iSubmot = SubMotIdx;
        NNS_SUBMOTION submot = mot.pSubmotion[SubMotIdx];
        val.fSubmotType = submot.fType;
        val.fSubmotIPType = submot.fIPType;
        val.pObject = obj;
        nnCalcMotionUserData(val, submot, nframe);
    }

    private static void nnCalcMotionUserData(
      NNS_NODEUSRMOT_CALLBACK_VAL val,
      NNS_SUBMOTION submot,
      float frame)
    {
        if (nngNodeUserMotionCallbackFunc == null)
            return;
        uint num = submot.fType & 786432U;
        object pKeyList = submot.pKeyList;
        int nKeyFrame = submot.nKeyFrame;
        uint val1 = 0;
        float val2 = 0.0f;
        if (((int)num & 262144) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    nnInterpolateLinearU1((NNS_MOTION_KEY_Class12[])pKeyList, nKeyFrame, frame, out val1);
                    break;
                case 4:
                    nnInterpolateConstantU1((NNS_MOTION_KEY_Class12[])pKeyList, nKeyFrame, frame, out val1);
                    break;
                case 64:
                    nnSearchTriggerU1((NNS_MOTION_KEY_Class12[])pKeyList, nKeyFrame, frame, nngNodeUserMotionTriggerTime, nngNodeUserMotionCallbackFunc, val);
                    return;
            }
            val.IValue = val1;
        }
        else if (((int)num & 524288) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    nnInterpolateLinearF1((NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val2);
                    break;
                case 4:
                    nnInterpolateConstantF1((NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val2);
                    break;
                case 16:
                    nnInterpolateBezierF1((NNS_MOTION_KEY_Class2[])pKeyList, nKeyFrame, frame, out val2);
                    break;
                case 32:
                    nnInterpolateSISplineF1((NNS_MOTION_KEY_Class3[])pKeyList, nKeyFrame, frame, out val2);
                    break;
            }
            val.FValue = val2;
        }
        nngNodeUserMotionCallbackFunc(val);
    }

    private static void nnNormalizeColumn0(NNS_MATRIX mtx)
    {
        float m00 = mtx.M00;
        float num1 = m00 * m00;
        float m10 = mtx.M10;
        float num2 = num1 + m10 * m10;
        float m20 = mtx.M20;
        float num3 = nnInvertSqrt(num2 + m20 * m20);
        mtx.M00 *= num3;
        mtx.M10 *= num3;
        mtx.M20 *= num3;
    }

    private static void nnNormalizeColumn1(NNS_MATRIX mtx)
    {
        float m01 = mtx.M01;
        float num1 = m01 * m01;
        float m11 = mtx.M11;
        float num2 = num1 + m11 * m11;
        float m21 = mtx.M21;
        float num3 = nnInvertSqrt(num2 + m21 * m21);
        mtx.M01 *= num3;
        mtx.M11 *= num3;
        mtx.M21 *= num3;
    }

    private static void nnNormalizeColumn2(NNS_MATRIX mtx)
    {
        float m02 = mtx.M02;
        float num1 = m02 * m02;
        float m12 = mtx.M12;
        float num2 = num1 + m12 * m12;
        float m22 = mtx.M22;
        float num3 = nnInvertSqrt(num2 + m22 * m22);
        mtx.M02 *= num3;
        mtx.M12 *= num3;
        mtx.M22 *= num3;
    }

    private static void nnNormalizeColumn3(NNS_MATRIX mtx)
    {
        float m03 = mtx.M03;
        float num1 = m03 * m03;
        float m13 = mtx.M13;
        float num2 = num1 + m13 * m13;
        float m23 = mtx.M23;
        float num3 = nnInvertSqrt(num2 + m23 * m23);
        mtx.M03 *= num3;
        mtx.M13 *= num3;
        mtx.M23 *= num3;
    }

    private void nnCalcNodeMatrixNode(NNS_MATRIX mtx, NNS_OBJECT obj, int nodeidx)
    {
        NNS_NODE pNode = obj.pNodeList[nodeidx];
        if (pNode.iParent != -1)
            this.nnCalcNodeMatrixNode(mtx, obj, pNode.iParent);
        if (((int)pNode.fType & 1) == 0)
        {
            if (((int)pNode.fType & 8192) != 0)
            {
                nnTranslateMatrixFast(mtx, pNode.Translation.x, pNode.Translation.y, pNode.Translation.z);
            }
            else
            {
                NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
                nnTransformVector(nnsVector, nncalcnodematrix.nnsBaseMtx, pNode.Translation);
                nnCopyVectorMatrixTranslation(mtx, nnsVector);
            }
        }
        if (((int)pNode.fType & 4096) != 0)
            nnCopyMatrix33(mtx, nncalcnodematrix.nnsBaseMtx);
        else if (((int)pNode.fType & 1835008) != 0)
        {
            if (((int)pNode.fType & 262144) != 0)
                nnNormalizeColumn0(mtx);
            if (((int)pNode.fType & 524288) != 0)
                nnNormalizeColumn1(mtx);
            if (((int)pNode.fType & 1048576) != 0)
                nnNormalizeColumn2(mtx);
        }
        if (((int)pNode.fType & 2) == 0)
        {
            switch (pNode.fType & 3840U)
            {
                case 0:
                    nnRotateXYZMatrixFast(mtx, pNode.Rotation.x, pNode.Rotation.y, pNode.Rotation.z);
                    break;
                case 256:
                    nnRotateXZYMatrixFast(mtx, pNode.Rotation.x, pNode.Rotation.y, pNode.Rotation.z);
                    break;
                case 1024:
                    nnRotateZXYMatrixFast(mtx, pNode.Rotation.x, pNode.Rotation.y, pNode.Rotation.z);
                    break;
                default:
                    nnRotateXYZMatrixFast(mtx, pNode.Rotation.x, pNode.Rotation.y, pNode.Rotation.z);
                    break;
            }
        }
        if (((int)pNode.fType & 4) != 0)
            return;
        nnScaleMatrixFast(mtx, pNode.Scaling.x, pNode.Scaling.y, pNode.Scaling.z);
    }

    private void nnCalcNodeMatrix(
      NNS_MATRIX mtx,
      NNS_OBJECT obj,
      int nodeidx,
      NNS_MATRIX basemtx)
    {
        nncalcnodematrix.nnsBaseMtx = basemtx == null ? nngUnitMatrix : basemtx;
        nnCopyMatrix(mtx, nncalcnodematrix.nnsBaseMtx);
        this.nnCalcNodeMatrixNode(mtx, obj, nodeidx);
    }

    private void nnCalcNodeMatrixMotionNode(NNS_MATRIX mtx, int nodeidx)
    {
        NNS_MATRIX src = null;
        int num = 0;
        NNS_NODE nnsNode = nncalcnodematrix.nnsNodeList[nodeidx];
        if (((int)nnsNode.fType & 122880) == 0)
        {
            if (nnsNode.iParent != -1)
                this.nnCalcNodeMatrixMotionNode(mtx, nnsNode.iParent);
            int? pHideFlag = new int?();
            nncalcnodematrix.nnsSubMotIdx = nnCalcNodeMotionCore(mtx, ref pHideFlag, nncalcnodematrix.nnsBaseMtx, nnsNode, nodeidx, nncalcnodematrix.nnsObj, nncalcnodematrix.nnsMot, nncalcnodematrix.nnsSubMotIdx, nncalcnodematrix.nnsFrame);
        }
        else
        {
            NNS_NODE pNode1 = null;
            NNS_NODE pNode2 = null;
            NNS_NODE pNode3 = null;
            NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_MATRIX jnt2mtx = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_MATRIX nnsMatrix3 = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_MATRIX nnsMatrix4 = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_VECTORFAST dst = new NNS_VECTORFAST();
            int nodeidx1 = 0;
            int NodeIdx1 = 0;
            int NodeIdx2 = 0;
            int NodeIdx3 = 0;
            if (((int)nnsNode.fType & 8192) != 0)
            {
                src = nnsMatrix2;
                NodeIdx3 = nodeidx;
                pNode3 = nnsNode;
                if (((int)nncalcnodematrix.nnsNodeList[nnsNode.iParent].fType & 16384) != 0)
                {
                    num = 1;
                    NodeIdx1 = pNode3.iParent;
                    pNode1 = nncalcnodematrix.nnsNodeList[NodeIdx1];
                    nodeidx1 = pNode1.iParent;
                }
                else if (((int)nncalcnodematrix.nnsNodeList[nnsNode.iParent].fType & 65536) != 0)
                {
                    num = 2;
                    NodeIdx2 = pNode3.iParent;
                    pNode2 = nncalcnodematrix.nnsNodeList[NodeIdx2];
                    NodeIdx1 = pNode2.iParent;
                    pNode1 = nncalcnodematrix.nnsNodeList[NodeIdx1];
                    nodeidx1 = pNode1.iParent;
                }
            }
            else if (((int)nnsNode.fType & 16384) != 0)
            {
                src = nnsMatrix1;
                num = 1;
                NodeIdx1 = nodeidx;
                pNode1 = nnsNode;
                nodeidx1 = pNode1.iParent;
                NodeIdx3 = pNode1.iChild;
                pNode3 = nncalcnodematrix.nnsNodeList[NodeIdx3];
            }
            else if (((int)nnsNode.fType & 32768) != 0)
            {
                src = nnsMatrix1;
                num = 2;
                NodeIdx1 = nodeidx;
                pNode1 = nnsNode;
                nodeidx1 = pNode1.iParent;
                NodeIdx2 = pNode1.iChild;
                pNode2 = nncalcnodematrix.nnsNodeList[NodeIdx2];
                NodeIdx3 = pNode2.iChild;
                pNode3 = nncalcnodematrix.nnsNodeList[NodeIdx3];
            }
            else if (((int)nnsNode.fType & 65536) != 0)
            {
                src = jnt2mtx;
                num = 2;
                NodeIdx2 = nodeidx;
                pNode2 = nnsNode;
                NodeIdx3 = pNode2.iChild;
                pNode3 = nncalcnodematrix.nnsNodeList[NodeIdx3];
                NodeIdx1 = pNode2.iParent;
                pNode1 = nncalcnodematrix.nnsNodeList[NodeIdx1];
                nodeidx1 = pNode1.iParent;
            }
            nnCopyMatrix(nnsMatrix1, nncalcnodematrix.nnsBaseMtx);
            this.nnCalcNodeMatrixMotionNode(nnsMatrix1, nodeidx1);
            nnMakeUnitMatrix(nnsMatrix3);
            int? pHideFlag = new int?();
            nncalcnodematrix.nnsSubMotIdx = nnCalcNodeMotionCore(nnsMatrix3, ref pHideFlag, nnsMatrix3, pNode1, NodeIdx1, nncalcnodematrix.nnsObj, nncalcnodematrix.nnsMot, nncalcnodematrix.nnsSubMotIdx, nncalcnodematrix.nnsFrame);
            float siikBoneLength1 = pNode1.SIIKBoneLength;
            switch (num)
            {
                case 1:
                    nnMakeUnitMatrix(nnsMatrix2);
                    pHideFlag = new int?();
                    nncalcnodematrix.nnsSubMotIdx = nnCalcNodeMotionCore(nnsMatrix2, ref pHideFlag, nnsMatrix2, pNode3, NodeIdx3, nncalcnodematrix.nnsObj, nncalcnodematrix.nnsMot, nncalcnodematrix.nnsSubMotIdx, nncalcnodematrix.nnsFrame);
                    nnCopyMatrixTranslationVectorFast(out dst, nnsMatrix2);
                    nnTransformVectorFast(out dst, nncalcnodematrix.nnsBaseMtx, dst);
                    nnCopyVectorFastMatrixTranslation(nnsMatrix2, ref dst);
                    nnCalc1BoneSIIK(nnsMatrix1, nnsMatrix3, nnsMatrix2, siikBoneLength1);
                    if (((int)pNode3.fType & 4096) != 0)
                    {
                        nnCopyMatrix33(nnsMatrix2, nncalcnodematrix.nnsBaseMtx);
                        break;
                    }
                    break;
                case 2:
                    nnMakeUnitMatrix(nnsMatrix4);
                    pHideFlag = new int?();
                    nncalcnodematrix.nnsSubMotIdx = nnCalcNodeMotionCore(nnsMatrix4, ref pHideFlag, nnsMatrix4, pNode2, NodeIdx2, nncalcnodematrix.nnsObj, nncalcnodematrix.nnsMot, nncalcnodematrix.nnsSubMotIdx, nncalcnodematrix.nnsFrame);
                    nnMakeUnitMatrix(nnsMatrix2);
                    nncalcnodematrix.nnsSubMotIdx = nnCalcNodeMotionCore(nnsMatrix2, ref pHideFlag, nnsMatrix2, pNode3, NodeIdx3, nncalcnodematrix.nnsObj, nncalcnodematrix.nnsMot, nncalcnodematrix.nnsSubMotIdx, nncalcnodematrix.nnsFrame);
                    nnCopyMatrixTranslationVectorFast(out dst, nnsMatrix2);
                    nnTransformVectorFast(out dst, nncalcnodematrix.nnsBaseMtx, dst);
                    nnCopyVectorFastMatrixTranslation(nnsMatrix2, ref dst);
                    int zpref = ((int)pNode2.fType & 131072) == 0 ? 0 : 1;
                    float siikBoneLength2 = pNode2.SIIKBoneLength;
                    nnCalc2BoneSIIK(nnsMatrix1, nnsMatrix3, jnt2mtx, nnsMatrix4, nnsMatrix2, siikBoneLength1, siikBoneLength2, zpref);
                    if (((int)pNode3.fType & 4096) != 0)
                    {
                        nnCopyMatrix33(nnsMatrix2, nncalcnodematrix.nnsBaseMtx);
                        break;
                    }
                    break;
            }
            nnCopyMatrix(mtx, src);
        }
    }

    private void nnCalcNodeMatrixMotion(
      NNS_MATRIX mtx,
      NNS_OBJECT obj,
      int nodeidx,
      NNS_MOTION mot,
      float frame,
      NNS_MATRIX basemtx)
    {
        if (((int)mot.fType & 1) == 0)
            return;
        float dstframe;
        if (nnCalcMotionFrame(out dstframe, mot.fType, mot.StartFrame, mot.EndFrame, frame) != 0)
        {
            nncalcnodematrix.nnsSubMotIdx = 0;
            if (basemtx != null)
            {
                nnCopyMatrix(mtx, basemtx);
                nncalcnodematrix.nnsBaseMtx = basemtx;
            }
            else
            {
                nnCopyMatrix(mtx, nngUnitMatrix);
                nncalcnodematrix.nnsBaseMtx = nngUnitMatrix;
            }
            nncalcnodematrix.nnsFrame = dstframe;
            nncalcnodematrix.nnsObj = obj;
            nncalcnodematrix.nnsMot = mot;
            nncalcnodematrix.nnsNodeList = obj.pNodeList;
            this.nnCalcNodeMatrixMotionNode(mtx, nodeidx);
        }
        else
            this.nnCalcNodeMatrix(mtx, obj, nodeidx, basemtx);
    }

    private static void nnCalcNodeMatrixTRSListNode(
      NNS_MATRIX mtx,
      NNS_OBJECT obj,
      int nodeidx,
      ArrayPointer<NNS_TRS> trslist)
    {
        NNS_NODE nnsNode1 = null;
        NNS_NODE nnsNode2 = null;
        NNS_MATRIX nnsMatrix1 = nnCalcNode_mtx_pool.Alloc();
        NNS_MATRIX nnsMatrix2 = nnCalcNode_mtx_pool.Alloc();
        NNS_MATRIX nnsMatrix3 = nnCalcNode_mtx_pool.Alloc();
        int nodeidx1 = 0;
        int index1 = 0;
        int index2 = 0;
        int num = 0;
        NNS_NODE pNode = obj.pNodeList[nodeidx];
        NNS_TRS nnsTrs1 = trslist[nodeidx];
        NNS_MATRIX src;
        int index3;
        NNS_NODE nnsNode3;
        if (((int)pNode.fType & 8192) != 0)
        {
            src = nnsMatrix3;
            index3 = nodeidx;
            nnsNode3 = pNode;
            if (((int)obj.pNodeList[pNode.iParent].fType & 16384) != 0)
            {
                num = 1;
                index1 = nnsNode3.iParent;
                nnsNode1 = obj.pNodeList[index1];
                nodeidx1 = nnsNode1.iParent;
            }
            else if (((int)obj.pNodeList[pNode.iParent].fType & 65536) != 0)
            {
                num = 2;
                index2 = nnsNode3.iParent;
                nnsNode2 = obj.pNodeList[index2];
                index1 = nnsNode2.iParent;
                nnsNode1 = obj.pNodeList[index1];
                nodeidx1 = nnsNode1.iParent;
            }
        }
        else if (((int)pNode.fType & 16384) != 0)
        {
            src = nnsMatrix1;
            num = 1;
            index1 = nodeidx;
            nnsNode1 = pNode;
            nodeidx1 = nnsNode1.iParent;
            index3 = nnsNode1.iChild;
            nnsNode3 = obj.pNodeList[index3];
        }
        else if (((int)pNode.fType & 32768) != 0)
        {
            src = nnsMatrix1;
            num = 2;
            index1 = nodeidx;
            nnsNode1 = pNode;
            nodeidx1 = nnsNode1.iParent;
            index2 = nnsNode1.iChild;
            nnsNode2 = obj.pNodeList[index2];
            index3 = nnsNode2.iChild;
            nnsNode3 = obj.pNodeList[index3];
        }
        else if (((int)pNode.fType & 65536) != 0)
        {
            src = nnsMatrix2;
            num = 2;
            index2 = nodeidx;
            nnsNode2 = pNode;
            index3 = nnsNode2.iChild;
            nnsNode3 = obj.pNodeList[index3];
            index1 = nnsNode2.iParent;
            nnsNode1 = obj.pNodeList[index1];
            nodeidx1 = nnsNode1.iParent;
        }
        else
        {
            nnCalcNode_mtx_pool.Release(nnsMatrix1);
            nnCalcNode_mtx_pool.Release(nnsMatrix2);
            nnCalcNode_mtx_pool.Release(nnsMatrix3);
            if (pNode.iParent != -1)
                nnCalcNodeMatrixTRSListNode(mtx, obj, pNode.iParent, trslist);
            else
                nnCopyMatrix(mtx, nncalcnodematrix.nnsBaseMtx);
            nnTranslateMatrix(mtx, mtx, nnsTrs1.Translation.x, nnsTrs1.Translation.y, nnsTrs1.Translation.z);
            if (((int)pNode.fType & 4096) != 0)
                nnCopyMatrix33(mtx, nncalcnodematrix.nnsBaseMtx);
            else if (((int)pNode.fType & 1835008) != 0)
            {
                if (((int)pNode.fType & 262144) != 0)
                    nnNormalizeColumn0(mtx);
                if (((int)pNode.fType & 524288) != 0)
                    nnNormalizeColumn1(mtx);
                if (((int)pNode.fType & 1048576) != 0)
                    nnNormalizeColumn2(mtx);
            }
            nnQuaternionMatrix(mtx, mtx, ref nnsTrs1.Rotation);
            nnScaleMatrix(mtx, mtx, nnsTrs1.Scaling.x, nnsTrs1.Scaling.y, nnsTrs1.Scaling.z);
            return;
        }
        NNS_MATRIX nnsMatrix4 = nnCalcNode_mtx_pool.Alloc();
        NNS_MATRIX nnsMatrix5 = nnCalcNode_mtx_pool.Alloc();
        NNS_VECTORFAST dst = new NNS_VECTORFAST();
        NNS_TRS nnsTrs2 = trslist + index1;
        NNS_TRS nnsTrs3 = trslist + index2;
        NNS_TRS nnsTrs4 = trslist + index3;
        nnCalcNodeMatrixTRSListNode(nnsMatrix1, obj, nodeidx1, trslist);
        nnMakeUnitMatrix(nnsMatrix4);
        nnTranslateMatrix(nnsMatrix4, nnsMatrix4, nnsTrs2.Translation.x, nnsTrs2.Translation.y, nnsTrs2.Translation.z);
        nnQuaternionMatrix(nnsMatrix4, nnsMatrix4, ref nnsTrs2.Rotation);
        nnScaleMatrix(nnsMatrix4, nnsMatrix4, nnsTrs2.Scaling.x, nnsTrs2.Scaling.y, nnsTrs2.Scaling.z);
        float siikBoneLength1 = nnsNode1.SIIKBoneLength;
        switch (num)
        {
            case 1:
                nnMakeUnitMatrix(nnsMatrix3);
                nnTranslateMatrix(nnsMatrix3, nnsMatrix3, nnsTrs4.Translation.x, nnsTrs4.Translation.y, nnsTrs4.Translation.z);
                nnQuaternionMatrix(nnsMatrix3, nnsMatrix3, ref nnsTrs4.Rotation);
                nnScaleMatrix(nnsMatrix3, nnsMatrix3, nnsTrs4.Scaling.x, nnsTrs4.Scaling.y, nnsTrs4.Scaling.z);
                nnCopyMatrixTranslationVectorFast(out dst, nnsMatrix3);
                nnTransformVectorFast(out dst, nncalcnodematrix.nnsBaseMtx, dst);
                nnCopyVectorFastMatrixTranslation(nnsMatrix3, ref dst);
                nnCalc1BoneSIIK(nnsMatrix1, nnsMatrix4, nnsMatrix3, siikBoneLength1);
                if (((int)nnsNode3.fType & 4096) != 0)
                {
                    nnCopyMatrix33(nnsMatrix3, nncalcnodematrix.nnsBaseMtx);
                    break;
                }
                break;
            case 2:
                nnMakeUnitMatrix(nnsMatrix5);
                nnTranslateMatrix(nnsMatrix2, nnsMatrix2, nnsTrs3.Translation.x, nnsTrs3.Translation.y, nnsTrs3.Translation.z);
                nnQuaternionMatrix(nnsMatrix2, nnsMatrix2, ref nnsTrs3.Rotation);
                nnScaleMatrix(nnsMatrix2, nnsMatrix2, nnsTrs3.Scaling.x, nnsTrs3.Scaling.y, nnsTrs3.Scaling.z);
                nnMakeUnitMatrix(nnsMatrix3);
                nnTranslateMatrix(nnsMatrix3, nnsMatrix3, nnsTrs4.Translation.x, nnsTrs4.Translation.y, nnsTrs4.Translation.z);
                nnQuaternionMatrix(nnsMatrix3, nnsMatrix3, ref nnsTrs4.Rotation);
                nnScaleMatrix(nnsMatrix3, nnsMatrix3, nnsTrs4.Scaling.x, nnsTrs4.Scaling.y, nnsTrs4.Scaling.z);
                nnCopyMatrixTranslationVectorFast(out dst, nnsMatrix3);
                nnTransformVectorFast(out dst, nncalcnodematrix.nnsBaseMtx, dst);
                nnCopyVectorFastMatrixTranslation(nnsMatrix3, ref dst);
                int zpref = ((int)nnsNode2.fType & 131072) != 0 ? 0 : 1;
                float siikBoneLength2 = nnsNode2.SIIKBoneLength;
                nnCalc2BoneSIIK(nnsMatrix1, nnsMatrix4, nnsMatrix2, nnsMatrix5, nnsMatrix3, siikBoneLength1, siikBoneLength2, zpref);
                if (((int)nnsNode3.fType & 4096) != 0)
                {
                    nnCopyMatrix33(nnsMatrix3, nncalcnodematrix.nnsBaseMtx);
                    break;
                }
                break;
        }
        nnCopyMatrix(mtx, src);
        nnCalcNode_mtx_pool.Release(nnsMatrix4);
        nnCalcNode_mtx_pool.Release(nnsMatrix5);
        nnCalcNode_mtx_pool.Release(nnsMatrix1);
        nnCalcNode_mtx_pool.Release(nnsMatrix2);
        nnCalcNode_mtx_pool.Release(nnsMatrix3);
    }

    private static void nnCalcNodeMatrixTRSList(
      NNS_MATRIX mtx,
      NNS_OBJECT obj,
      int nodeidx,
      ArrayPointer<NNS_TRS> trslist,
      NNS_MATRIX basemtx)
    {
        nncalcnodematrix.nnsBaseMtx = basemtx == null ? nngUnitMatrix : basemtx;
        nnCalcNodeMatrixTRSListNode(mtx, obj, nodeidx, trslist);
    }

    public static void nnCopyMatrix33(NNS_MATRIX dst, NNS_MATRIX src)
    {
        dst.M00 = src.M00;
        dst.M01 = src.M01;
        dst.M02 = src.M02;
        dst.M10 = src.M10;
        dst.M11 = src.M11;
        dst.M12 = src.M12;
        dst.M20 = src.M20;
        dst.M21 = src.M21;
        dst.M22 = src.M22;
    }

    public static void nnCopyMatrix33(ref SNNS_MATRIX dst, ref SNNS_MATRIX src)
    {
        dst.M00 = src.M00;
        dst.M01 = src.M01;
        dst.M02 = src.M02;
        dst.M10 = src.M10;
        dst.M11 = src.M11;
        dst.M12 = src.M12;
        dst.M20 = src.M20;
        dst.M21 = src.M21;
        dst.M22 = src.M22;
    }

    private static void nnCalcMatrixPaletteNode(int nodeIdx)
    {
        NNS_NODE nnsNode;
        do
        {
            nnsNode = nncalcmatrixpalette.nnsNodeList[nodeIdx];
            nnPushMatrix(nncalcmatrixpalette.nnsMstk, null);
            NNS_MATRIX currentMatrix = nnGetCurrentMatrix(nncalcmatrixpalette.nnsMstk);
            if (((int)nnsNode.fType & 1) == 0)
            {
                if (((int)nnsNode.fType & 8192) == 0)
                {
                    nnTranslateMatrixFast(currentMatrix, nnsNode.Translation.x, nnsNode.Translation.y, nnsNode.Translation.z);
                }
                else
                {
                    NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
                    nnTransformVector(nnsVector, nncalcmatrixpalette.nnsBaseMtx, nnsNode.Translation);
                    nnCopyVectorMatrixTranslation(currentMatrix, nnsVector);
                    GlobalPool<NNS_VECTOR>.Release(nnsVector);
                }
            }
            if (((int)nnsNode.fType & 4096) != 0)
                nnCopyMatrix33(currentMatrix, nncalcmatrixpalette.nnsBaseMtx);
            else if (((int)nnsNode.fType & 1835008) != 0)
            {
                if (((int)nnsNode.fType & 262144) != 0)
                    nnNormalizeColumn0(currentMatrix);
                if (((int)nnsNode.fType & 524288) != 0)
                    nnNormalizeColumn1(currentMatrix);
                if (((int)nnsNode.fType & 1048576) != 0)
                    nnNormalizeColumn2(currentMatrix);
            }
            if (((int)nnsNode.fType & 2) == 0)
            {
                switch (nnsNode.fType & 3840U)
                {
                    case 0:
                        nnRotateXYZMatrixFast(currentMatrix, nnsNode.Rotation.x, nnsNode.Rotation.y, nnsNode.Rotation.z);
                        break;
                    case 256:
                        nnRotateXZYMatrixFast(currentMatrix, nnsNode.Rotation.x, nnsNode.Rotation.y, nnsNode.Rotation.z);
                        break;
                    case 1024:
                        nnRotateZXYMatrixFast(currentMatrix, nnsNode.Rotation.x, nnsNode.Rotation.y, nnsNode.Rotation.z);
                        break;
                    default:
                        nnRotateXYZMatrixFast(currentMatrix, nnsNode.Rotation.x, nnsNode.Rotation.y, nnsNode.Rotation.z);
                        break;
                }
            }
            if (((int)nnsNode.fType & 4) == 0)
                nnScaleMatrixFast(currentMatrix, nnsNode.Scaling.x, nnsNode.Scaling.y, nnsNode.Scaling.z);
            if (nnsNode.iMatrix != -1)
            {
                if (((int)nnsNode.fType & 8) != 0)
                    nnCopyMatrix(nncalcmatrixpalette.nnsMtxPal[nnsNode.iMatrix], currentMatrix);
                else
                    nnMultiplyMatrix(nncalcmatrixpalette.nnsMtxPal[nnsNode.iMatrix], currentMatrix, nnsNode.InvInitMtx);
            }
            if (nncalcmatrixpalette.nnsNodeStatList != null)
            {
                if (nodeIdx == 0 && nncalcmatrixpalette.nnsNSFlag != 0U)
                    nncalcmatrixpalette.nnsRootScale = nnEstimateMatrixScaling(currentMatrix);
                nnCalcClipSetNodeStatus(nncalcmatrixpalette.nnsNodeStatList, nncalcmatrixpalette.nnsNodeList, nodeIdx, currentMatrix, nncalcmatrixpalette.nnsRootScale, nncalcmatrixpalette.nnsNSFlag);
            }
            if (nnsNode.iChild != -1)
                nnCalcMatrixPaletteNode(nnsNode.iChild);
            nnPopMatrix(nncalcmatrixpalette.nnsMstk);
            nodeIdx = nnsNode.iSibling;
        }
        while (nnsNode.iSibling != -1);
    }

    private static void nnCalcMatrixPalette(
      NNS_MATRIX[] mtxpal,
      uint[] nodestatlist,
      NNS_OBJECT obj,
      NNS_MATRIX basemtx,
      NNS_MATRIXSTACK mstk,
      uint flag)
    {
        nncalcmatrixpalette.nnsBaseMtx = basemtx == null ? nngUnitMatrix : basemtx;
        nnSetCurrentMatrix(mstk, nncalcmatrixpalette.nnsBaseMtx);
        nncalcmatrixpalette.nnsMtxPal = mtxpal;
        nncalcmatrixpalette.nnsNodeStatList = nodestatlist;
        nncalcmatrixpalette.nnsNSFlag = flag;
        nncalcmatrixpalette.nnsNodeList = obj.pNodeList;
        nncalcmatrixpalette.nnsMstk = mstk;
        nnCalcMatrixPaletteNode(0);
    }

    private static void nnPutCommonVertex(
      NNS_VTXLIST_COMMON_DESC vdesc,
      int nIndexSetSize,
      ushort indices,
      NNS_MATRIX mtxpal,
      uint flag)
    {
        mppAssertNotImpl();
    }

    private static void nnPutNormalVector(NNS_VECTOR pos, NNS_VECTOR nrm)
    {
        mppAssertNotImpl();
    }

    private static void nnPutCommonVertexNormal(
      NNS_VTXLIST_COMMON_DESC vdesc,
      int nIndexSetSize,
      ushort indices,
      NNS_MATRIX mtxpal)
    {
        mppAssertNotImpl();
    }

    private void nnDrawObjectCommonVertex(
      NNS_VTXLISTPTR vlistptr,
      NNS_PRIMLISTPTR plistptr,
      NNS_MATRIX mtxpal,
      uint flag)
    {
        mppAssertNotImpl();
    }

    private void nnDrawObjectCommonVertexNormal(
      NNS_VTXLISTPTR vlistptr,
      NNS_PRIMLISTPTR plistptr,
      NNS_MATRIX mtxpal)
    {
        mppAssertNotImpl();
    }

    public static uint NNM_GL_TEXTURE(int _slot)
    {
        return (uint)(33984 + _slot);
    }

    public static uint NNM_GL_LIGHT(int _idx)
    {
        return (uint)(16384 + _idx);
    }

    private int nnCheckCollisionSS(ref NNS_SPHERE sphere1, ref NNS_SPHERE sphere2)
    {
        mppAssertNotImpl();
        return 0;
    }

    private int nnCheckCollisionCC(ref NNS_CAPSULE capsule1, ref NNS_CAPSULE capsule2)
    {
        mppAssertNotImpl();
        return 1;
    }

    private int nnCheckCollisionBS(ref NNS_BOX box, ref NNS_SPHERE sphere)
    {
        mppAssertNotImpl();
        return 0;
    }

    private int nnCheckCollisionBB(ref NNS_BOX box1, ref NNS_BOX box2)
    {
        mppAssertNotImpl();
        return 1;
    }

    public static void nnInitMaterialMotionObject(
      NNS_OBJECT mmobj,
      NNS_OBJECT obj,
      NNS_MOTION mmot)
    {
        int nMaterial = obj.nMaterial;
        mmobj.Assign(obj);
        mmobj.pMatPtrList = new NNS_MATERIALPTR[nMaterial];
        for (int index = 0; index < nMaterial; ++index)
            mmobj.pMatPtrList[index] = new NNS_MATERIALPTR(obj.pMatPtrList[index]);
        if (mmot != null)
        {
            if (((int)mmot.fType & 31) != 16)
                return;
            ArrayPointer<NNS_SUBMOTION> pSubmotion = mmot.pSubmotion;
            int nSubmotion = mmot.nSubmotion;
            for (int index1 = 0; index1 < nMaterial; ++index1)
            {
                bool flag1 = false;
                bool flag2 = false;
                bool flag3 = false;
                bool[] objectBTexOffsetMot = nnInitMaterialMotionObject_bTexOffsetMot;
                Array.Clear(objectBTexOffsetMot, 0, 8);
                for (; nSubmotion > 0 && (~pSubmotion).Id0 < index1; --nSubmotion)
                    ++pSubmotion;
                for (; nSubmotion > 0 && (~pSubmotion).Id0 == index1; --nSubmotion)
                {
                    switch ((~pSubmotion).fType & 4294967040U)
                    {
                        case 512:
                        case 1024:
                        case 2048:
                        case 3584:
                        case 4096:
                        case 8192:
                        case 16384:
                        case 32768:
                        case 57344:
                        case 65536:
                        case 131072:
                        case 262144:
                        case 524288:
                        case 1048576:
                        case 1835008:
                            flag1 = true;
                            flag2 = true;
                            break;
                        case 2097152:
                        case 4194304:
                            flag1 = true;
                            flag3 = true;
                            break;
                        case 8388608:
                        case 16777216:
                        case 25165824:
                            flag1 = true;
                            flag3 = true;
                            objectBTexOffsetMot[(~pSubmotion).Id1] = true;
                            break;
                        case 33554432:
                            flag1 = true;
                            break;
                    }
                    ++pSubmotion;
                }
                if (flag1)
                {
                    uint fType = obj.pMatPtrList[index1].fType;
                    if (((int)fType & 2) != 0)
                    {
                        NNS_MATERIAL_STDSHADER_DESC pMaterial = (NNS_MATERIAL_STDSHADER_DESC)obj.pMatPtrList[index1].pMaterial;
                        NNS_MATERIAL_STDSHADER_DESC materialStdshaderDesc;
                        mmobj.pMatPtrList[index1].pMaterial = ((int)fType & 4) == 0 ? (materialStdshaderDesc = new NNS_MATERIAL_STDSHADER_DESC(pMaterial)) : (materialStdshaderDesc = new NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE((NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)pMaterial));
                        if (flag2)
                            materialStdshaderDesc.pColor = new NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            materialStdshaderDesc.pTexDesc = new NNS_MATERIAL_STDSHADER_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                materialStdshaderDesc.pTexDesc[index2] = new NNS_MATERIAL_STDSHADER_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                                if (objectBTexOffsetMot[index2])
                                    materialStdshaderDesc.pTexDesc[index2].fType &= 3221225471U;
                            }
                        }
                    }
                    else if (((int)fType & 8) != 0)
                    {
                        NNS_MATERIAL_GLES11_DESC pMaterial = (NNS_MATERIAL_GLES11_DESC)obj.pMatPtrList[index1].pMaterial;
                        NNS_MATERIAL_GLES11_DESC materialGleS11Desc;
                        mmobj.pMatPtrList[index1].pMaterial = materialGleS11Desc = new NNS_MATERIAL_GLES11_DESC(pMaterial);
                        if (flag2)
                            materialGleS11Desc.pColor = new NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            materialGleS11Desc.pTexDesc = new NNS_MATERIAL_GLES11_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                materialGleS11Desc.pTexDesc[index2] = new NNS_MATERIAL_GLES11_TEXMAP_DESC(ref pMaterial.pTexDesc[index2]);
                                if (objectBTexOffsetMot[index2])
                                    materialGleS11Desc.pTexDesc[index2].fType &= 3221225471U;
                            }
                        }
                    }
                    else if (((int)fType & 1) != 0)
                    {
                        NNS_MATERIAL_DESC pMaterial = (NNS_MATERIAL_DESC)obj.pMatPtrList[index1].pMaterial;
                        NNS_MATERIAL_DESC nnsMaterialDesc;
                        mmobj.pMatPtrList[index1].pMaterial = nnsMaterialDesc = new NNS_MATERIAL_DESC(pMaterial);
                        if (flag2)
                            nnsMaterialDesc.pColor = new NNS_MATERIAL_COLOR(pMaterial.pColor);
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            nnsMaterialDesc.pTexDesc = new NNS_MATERIAL_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                nnsMaterialDesc.pTexDesc[index2] = new NNS_MATERIAL_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                                if (objectBTexOffsetMot[index2])
                                    nnsMaterialDesc.pTexDesc[index2].fType &= 3221225471U;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int index1 = 0; index1 < nMaterial; ++index1)
            {
                uint fType = obj.pMatPtrList[index1].fType;
                if (((int)fType & 2) != 0)
                {
                    NNS_MATERIAL_STDSHADER_DESC pMaterial = (NNS_MATERIAL_STDSHADER_DESC)obj.pMatPtrList[index1].pMaterial;
                    NNS_MATERIAL_STDSHADER_DESC materialStdshaderDesc;
                    mmobj.pMatPtrList[index1].pMaterial = ((int)fType & 4) == 0 ? (materialStdshaderDesc = new NNS_MATERIAL_STDSHADER_DESC(pMaterial)) : (materialStdshaderDesc = new NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE((NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)pMaterial));
                    materialStdshaderDesc.pColor = new NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                    materialStdshaderDesc.pTexDesc = new NNS_MATERIAL_STDSHADER_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        materialStdshaderDesc.pTexDesc[index2] = new NNS_MATERIAL_STDSHADER_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                }
                else if (((int)fType & 8) != 0)
                {
                    NNS_MATERIAL_GLES11_DESC pMaterial = (NNS_MATERIAL_GLES11_DESC)obj.pMatPtrList[index1].pMaterial;
                    NNS_MATERIAL_GLES11_DESC materialGleS11Desc;
                    mmobj.pMatPtrList[index1].pMaterial = materialGleS11Desc = new NNS_MATERIAL_GLES11_DESC(pMaterial);
                    materialGleS11Desc.pColor = new NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                    materialGleS11Desc.pTexDesc = new NNS_MATERIAL_GLES11_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        materialGleS11Desc.pTexDesc[index2] = new NNS_MATERIAL_GLES11_TEXMAP_DESC(ref pMaterial.pTexDesc[index2]);
                }
                else if (((int)fType & 1) != 0)
                {
                    NNS_MATERIAL_DESC pMaterial = (NNS_MATERIAL_DESC)obj.pMatPtrList[index1].pMaterial;
                    NNS_MATERIAL_DESC nnsMaterialDesc;
                    mmobj.pMatPtrList[index1].pMaterial = nnsMaterialDesc = new NNS_MATERIAL_DESC(pMaterial);
                    nnsMaterialDesc.pColor = new NNS_MATERIAL_COLOR(pMaterial.pColor);
                    nnsMaterialDesc.pTexDesc = new NNS_MATERIAL_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        nnsMaterialDesc.pTexDesc[index2] = new NNS_MATERIAL_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                }
            }
        }
    }

    public static void nnInitMaterialMotionObject_fast(
      NNS_OBJECT mmobj,
      NNS_OBJECT obj,
      NNS_MOTION mmot)
    {
        int nMaterial = obj.nMaterial;
        mmobj.Assign(obj);
        mmobj.pMatPtrList = amDrawAlloc_NNS_MATERIALPTR(nMaterial);
        for (int index = 0; index < nMaterial; ++index)
        {
            mmobj.pMatPtrList[index] = amDrawAlloc_NNS_MATERIALPTR();
            mmobj.pMatPtrList[index].Assign(obj.pMatPtrList[index]);
        }
        if (mmot != null)
        {
            if (((int)mmot.fType & 31) != 16)
                return;
            ArrayPointer<NNS_SUBMOTION> pSubmotion = mmot.pSubmotion;
            int nSubmotion = mmot.nSubmotion;
            for (int index1 = 0; index1 < nMaterial; ++index1)
            {
                bool flag1 = false;
                bool flag2 = false;
                bool flag3 = false;
                bool[] objectBTexOffsetMot = nnInitMaterialMotionObject_bTexOffsetMot;
                Array.Clear(objectBTexOffsetMot, 0, 8);
                for (; nSubmotion > 0 && (~pSubmotion).Id0 < index1; --nSubmotion)
                    ++pSubmotion;
                for (; nSubmotion > 0 && (~pSubmotion).Id0 == index1; --nSubmotion)
                {
                    switch ((~pSubmotion).fType & 4294967040U)
                    {
                        case 512:
                        case 1024:
                        case 2048:
                        case 3584:
                        case 4096:
                        case 8192:
                        case 16384:
                        case 32768:
                        case 57344:
                        case 65536:
                        case 131072:
                        case 262144:
                        case 524288:
                        case 1048576:
                        case 1835008:
                            flag1 = true;
                            flag2 = true;
                            break;
                        case 2097152:
                        case 4194304:
                            flag1 = true;
                            flag3 = true;
                            break;
                        case 8388608:
                        case 16777216:
                        case 25165824:
                            flag1 = true;
                            flag3 = true;
                            objectBTexOffsetMot[(~pSubmotion).Id1] = true;
                            break;
                        case 33554432:
                            flag1 = true;
                            break;
                    }
                    ++pSubmotion;
                }
                if (flag1)
                {
                    uint fType = obj.pMatPtrList[index1].fType;
                    if (((int)fType & 2) != 0)
                    {
                        NNS_MATERIAL_STDSHADER_DESC pMaterial = (NNS_MATERIAL_STDSHADER_DESC)obj.pMatPtrList[index1].pMaterial;
                        NNS_MATERIAL_STDSHADER_DESC materialStdshaderDesc;
                        mmobj.pMatPtrList[index1].pMaterial = ((int)fType & 4) == 0 ? (materialStdshaderDesc = new NNS_MATERIAL_STDSHADER_DESC(pMaterial)) : (materialStdshaderDesc = new NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE((NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)pMaterial));
                        if (flag2)
                            materialStdshaderDesc.pColor = new NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            materialStdshaderDesc.pTexDesc = new NNS_MATERIAL_STDSHADER_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                materialStdshaderDesc.pTexDesc[index2] = new NNS_MATERIAL_STDSHADER_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                                if (objectBTexOffsetMot[index2])
                                    materialStdshaderDesc.pTexDesc[index2].fType &= 3221225471U;
                            }
                        }
                    }
                    else if (((int)fType & 8) != 0)
                    {
                        NNS_MATERIAL_GLES11_DESC pMaterial = (NNS_MATERIAL_GLES11_DESC)obj.pMatPtrList[index1].pMaterial;
                        NNS_MATERIAL_GLES11_DESC materialGleS11Desc;
                        mmobj.pMatPtrList[index1].pMaterial = materialGleS11Desc = amDrawAlloc_NNS_MATERIAL_GLES11_DESC();
                        materialGleS11Desc.Assign(pMaterial);
                        if (flag2)
                        {
                            materialGleS11Desc.pColor = amDrawAlloc_NNS_MATERIAL_STDSHADER_COLOR();
                            materialGleS11Desc.pColor.Assign(pMaterial.pColor);
                        }
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            materialGleS11Desc.pTexDesc = new NNS_MATERIAL_GLES11_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                materialGleS11Desc.pTexDesc[index2] = new NNS_MATERIAL_GLES11_TEXMAP_DESC(ref pMaterial.pTexDesc[index2]);
                                if (objectBTexOffsetMot[index2])
                                    materialGleS11Desc.pTexDesc[index2].fType &= 3221225471U;
                            }
                        }
                    }
                    else if (((int)fType & 1) != 0)
                    {
                        NNS_MATERIAL_DESC pMaterial = (NNS_MATERIAL_DESC)obj.pMatPtrList[index1].pMaterial;
                        NNS_MATERIAL_DESC nnsMaterialDesc;
                        mmobj.pMatPtrList[index1].pMaterial = nnsMaterialDesc = new NNS_MATERIAL_DESC(pMaterial);
                        if (flag2)
                            nnsMaterialDesc.pColor = new NNS_MATERIAL_COLOR(pMaterial.pColor);
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            nnsMaterialDesc.pTexDesc = new NNS_MATERIAL_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                nnsMaterialDesc.pTexDesc[index2] = new NNS_MATERIAL_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                                if (objectBTexOffsetMot[index2])
                                    nnsMaterialDesc.pTexDesc[index2].fType &= 3221225471U;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int index1 = 0; index1 < nMaterial; ++index1)
            {
                uint fType = obj.pMatPtrList[index1].fType;
                if (((int)fType & 2) != 0)
                {
                    NNS_MATERIAL_STDSHADER_DESC pMaterial = (NNS_MATERIAL_STDSHADER_DESC)obj.pMatPtrList[index1].pMaterial;
                    NNS_MATERIAL_STDSHADER_DESC materialStdshaderDesc;
                    mmobj.pMatPtrList[index1].pMaterial = ((int)fType & 4) == 0 ? (materialStdshaderDesc = new NNS_MATERIAL_STDSHADER_DESC(pMaterial)) : (materialStdshaderDesc = new NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE((NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)pMaterial));
                    materialStdshaderDesc.pColor = new NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                    materialStdshaderDesc.pTexDesc = new NNS_MATERIAL_STDSHADER_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        materialStdshaderDesc.pTexDesc[index2] = new NNS_MATERIAL_STDSHADER_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                }
                else if (((int)fType & 8) != 0)
                {
                    NNS_MATERIAL_GLES11_DESC pMaterial = (NNS_MATERIAL_GLES11_DESC)obj.pMatPtrList[index1].pMaterial;
                    NNS_MATERIAL_GLES11_DESC materialGleS11Desc;
                    mmobj.pMatPtrList[index1].pMaterial = materialGleS11Desc = new NNS_MATERIAL_GLES11_DESC(pMaterial);
                    materialGleS11Desc.pColor = new NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                    materialGleS11Desc.pTexDesc = new NNS_MATERIAL_GLES11_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        materialGleS11Desc.pTexDesc[index2] = new NNS_MATERIAL_GLES11_TEXMAP_DESC(ref pMaterial.pTexDesc[index2]);
                }
                else if (((int)fType & 1) != 0)
                {
                    NNS_MATERIAL_DESC pMaterial = (NNS_MATERIAL_DESC)obj.pMatPtrList[index1].pMaterial;
                    NNS_MATERIAL_DESC nnsMaterialDesc;
                    mmobj.pMatPtrList[index1].pMaterial = nnsMaterialDesc = new NNS_MATERIAL_DESC(pMaterial);
                    nnsMaterialDesc.pColor = new NNS_MATERIAL_COLOR(pMaterial.pColor);
                    nnsMaterialDesc.pTexDesc = new NNS_MATERIAL_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        nnsMaterialDesc.pTexDesc[index2] = new NNS_MATERIAL_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                }
            }
        }
    }

    public static int nnInterpolateFloat(ref float val, NNS_SUBMOTION submot, float frame)
    {
        if (nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    nnInterpolateLinearF1((NNS_MOTION_KEY_Class1[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 4:
                    nnInterpolateConstantF1((NNS_MOTION_KEY_Class1[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 16:
                    nnInterpolateBezierF1((NNS_MOTION_KEY_Class2[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 32:
                    nnInterpolateSISplineF1((NNS_MOTION_KEY_Class3[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
            }
        }
        return 0;
    }

    public static int nnInterpolateFloat2(
      NNS_TEXCOORD val,
      NNS_SUBMOTION submot,
      float frame)
    {
        if (nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    nnInterpolateLinearF2((NNS_MOTION_KEY_Class4[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 4:
                    nnInterpolateConstantF2((NNS_MOTION_KEY_Class4[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
            }
        }
        return 0;
    }

    public static int nnInterpolateFloat3(
      NNS_VECTOR val,
      NNS_SUBMOTION submot,
      float frame)
    {
        if (nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    nnInterpolateLinearF3((NNS_MOTION_KEY_Class5[])submot.pKeyList, submot.nKeyFrame, frame, val);
                    return 1;
                case 4:
                    nnInterpolateConstantF3((NNS_MOTION_KEY_Class5[])submot.pKeyList, submot.nKeyFrame, frame, val);
                    return 1;
            }
        }
        return 0;
    }

    public static int nnInterpolateFloat3(
      ref SNNS_VECTOR val,
      NNS_SUBMOTION submot,
      float frame)
    {
        if (nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    nnInterpolateLinearF3((NNS_MOTION_KEY_Class5[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 4:
                    nnInterpolateConstantF3((NNS_MOTION_KEY_Class5[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
            }
        }
        return 0;
    }

    public static int nnInterpolateFloat3(
      ref NNS_RGBA _val,
      NNS_SUBMOTION submot,
      float frame)
    {
        SNNS_VECTOR val;
        val.x = _val.r;
        val.y = _val.g;
        val.z = _val.b;
        int num = nnInterpolateFloat3(ref val, submot, frame);
        _val.r = val.x;
        _val.g = val.y;
        _val.b = val.z;
        return num;
    }

    public static int nnInterpolateUint32(ref uint val, NNS_SUBMOTION submot, float frame)
    {
        if (nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    nnInterpolateLinearU1((NNS_MOTION_KEY_Class12[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 4:
                    nnInterpolateConstantU1((NNS_MOTION_KEY_Class12[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 64:
                    return nnInterpolateTriggerU1((NNS_MOTION_KEY_Class12[])submot.pKeyList, submot.nKeyFrame, frame, out val);
            }
        }
        return 0;
    }

    public static int nnInterpolateSint32(ref int val, NNS_SUBMOTION submot, float frame)
    {
        if (nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) == 0 || (submot.fIPType & 3703U) != 4U)
            return 0;
        nnInterpolateConstantS32_1((NNS_MOTION_KEY_Class11[])submot.pKeyList, submot.nKeyFrame, frame, out val);
        return 1;
    }

    public static void nnCalcMaterialMotion(
      NNS_OBJECT mmobj,
      NNS_OBJECT obj,
      NNS_MOTION mmot,
      float frame)
    {
        int nMaterial = obj.nMaterial;
        if (((int)mmot.fType & 31) != 16)
            return;
        for (int index1 = 0; index1 < nMaterial; ++index1)
        {
            uint fType = obj.pMatPtrList[index1].fType;
            if (((int)fType & 2) != 0)
            {
                NNS_MATERIAL_STDSHADER_DESC pMaterial1 = (NNS_MATERIAL_STDSHADER_DESC)obj.pMatPtrList[index1].pMaterial;
                NNS_MATERIAL_STDSHADER_DESC pMaterial2 = (NNS_MATERIAL_STDSHADER_DESC)mmobj.pMatPtrList[index1].pMaterial;
                if (pMaterial1 != pMaterial2)
                {
                    NNS_MATERIAL_STDSHADER_COLOR pColor1 = pMaterial1.pColor;
                    NNS_MATERIAL_STDSHADER_COLOR pColor2 = pMaterial2.pColor;
                    NNS_MATERIAL_STDSHADER_TEXMAP_DESC[] pTexDesc1 = pMaterial1.pTexDesc;
                    NNS_MATERIAL_STDSHADER_TEXMAP_DESC[] pTexDesc2 = pMaterial2.pTexDesc;
                    pMaterial2.User = pMaterial1.User;
                    if (pColor1 != pColor2)
                    {
                        pColor2.Ambient = pColor1.Ambient;
                        pColor2.Diffuse = pColor1.Diffuse;
                        pColor2.Specular = pColor1.Specular;
                        pColor2.Shininess = pColor1.Shininess;
                        pColor2.SpecularIntensity = pColor1.SpecularIntensity;
                    }
                    if (pTexDesc1 != pTexDesc2)
                    {
                        int nTex = pMaterial1.nTex;
                        for (int index2 = 0; index2 < nTex; ++index2)
                        {
                            pTexDesc2[index2].iTexIdx = pTexDesc1[index2].iTexIdx;
                            pTexDesc2[index2].Blend = pTexDesc1[index2].Blend;
                            pTexDesc2[index2].Offset = pTexDesc1[index2].Offset;
                        }
                    }
                }
            }
            else if (((int)fType & 8) != 0)
            {
                NNS_MATERIAL_GLES11_DESC pMaterial1 = (NNS_MATERIAL_GLES11_DESC)obj.pMatPtrList[index1].pMaterial;
                NNS_MATERIAL_GLES11_DESC pMaterial2 = (NNS_MATERIAL_GLES11_DESC)mmobj.pMatPtrList[index1].pMaterial;
                if (pMaterial1 != pMaterial2)
                {
                    NNS_MATERIAL_STDSHADER_COLOR pColor1 = pMaterial1.pColor;
                    NNS_MATERIAL_STDSHADER_COLOR pColor2 = pMaterial2.pColor;
                    NNS_MATERIAL_GLES11_TEXMAP_DESC[] pTexDesc1 = pMaterial1.pTexDesc;
                    NNS_MATERIAL_GLES11_TEXMAP_DESC[] pTexDesc2 = pMaterial2.pTexDesc;
                    pMaterial2.User = pMaterial1.User;
                    if (pColor1 != pColor2)
                    {
                        pColor2.Ambient = pColor1.Ambient;
                        pColor2.Diffuse = pColor1.Diffuse;
                        pColor2.Specular = pColor1.Specular;
                        pColor2.Shininess = pColor1.Shininess;
                        pColor2.SpecularIntensity = pColor1.SpecularIntensity;
                    }
                    if (pTexDesc1 != pTexDesc2)
                    {
                        int nTex = pMaterial1.nTex;
                        for (int index2 = 0; index2 < nTex; ++index2)
                        {
                            pTexDesc2[index2].iTexIdx = pTexDesc1[index2].iTexIdx;
                            pTexDesc2[index2].Offset = pTexDesc1[index2].Offset;
                        }
                    }
                }
            }
            else if (((int)fType & 1) != 0)
            {
                NNS_MATERIAL_DESC pMaterial1 = (NNS_MATERIAL_DESC)obj.pMatPtrList[index1].pMaterial;
                NNS_MATERIAL_DESC pMaterial2 = (NNS_MATERIAL_DESC)mmobj.pMatPtrList[index1].pMaterial;
                if (pMaterial1 != pMaterial2)
                {
                    NNS_MATERIAL_COLOR pColor1 = pMaterial1.pColor;
                    NNS_MATERIAL_COLOR pColor2 = pMaterial2.pColor;
                    NNS_MATERIAL_TEXMAP_DESC[] pTexDesc1 = pMaterial1.pTexDesc;
                    NNS_MATERIAL_TEXMAP_DESC[] pTexDesc2 = pMaterial2.pTexDesc;
                    pMaterial2.User = pMaterial1.User;
                    if (pColor1 != pColor2)
                    {
                        pColor2.Ambient = pColor1.Ambient;
                        pColor2.Diffuse = pColor1.Diffuse;
                        pColor2.Specular = pColor1.Specular;
                        pColor2.Shininess = pColor1.Shininess;
                    }
                    if (pTexDesc1 != pTexDesc2)
                    {
                        int nTex = pMaterial1.nTex;
                        for (int index2 = 0; index2 < nTex; ++index2)
                        {
                            pTexDesc2[index2].iTexIdx = pTexDesc1[index2].iTexIdx;
                            pTexDesc2[index2].EnvColor = pTexDesc1[index2].EnvColor;
                            pTexDesc2[index2].Offset = pTexDesc1[index2].Offset;
                        }
                    }
                }
            }
        }
        if (nnCalcMotionFrame(out frame, mmot.fType, mmot.StartFrame, mmot.EndFrame, frame) == 0)
            return;
        ArrayPointer<NNS_SUBMOTION> pSubmotion = mmot.pSubmotion;
        int nSubmotion = mmot.nSubmotion;
        for (int index = 0; index < nMaterial; ++index)
        {
            uint fType = obj.pMatPtrList[index].fType;
            if (((int)fType & 2) != 0)
            {
                NNS_MATERIAL_STDSHADER_DESC pMaterial = (NNS_MATERIAL_STDSHADER_DESC)mmobj.pMatPtrList[index].pMaterial;
                NNS_MATERIAL_STDSHADER_COLOR pColor = pMaterial.pColor;
                NNS_MATERIAL_STDSHADER_TEXMAP_DESC[] pTexDesc = pMaterial.pTexDesc;
                for (; nSubmotion > 0 && (~pSubmotion).Id0 < index; --nSubmotion)
                    ++pSubmotion;
                for (; nSubmotion > 0 && (~pSubmotion).Id0 == index; --nSubmotion)
                {
                    switch ((~pSubmotion).fType & 4294967040U)
                    {
                        case 512:
                            nnInterpolateFloat(ref pColor.Diffuse.r, pSubmotion, frame);
                            break;
                        case 1024:
                            nnInterpolateFloat(ref pColor.Diffuse.g, pSubmotion, frame);
                            break;
                        case 2048:
                            nnInterpolateFloat(ref pColor.Diffuse.b, pSubmotion, frame);
                            break;
                        case 3584:
                            nnInterpolateFloat3(ref pColor.Diffuse, pSubmotion, frame);
                            break;
                        case 4096:
                            nnInterpolateFloat(ref pColor.Diffuse.a, pSubmotion, frame);
                            break;
                        case 8192:
                            nnInterpolateFloat(ref pColor.Specular.r, pSubmotion, frame);
                            break;
                        case 16384:
                            nnInterpolateFloat(ref pColor.Specular.g, pSubmotion, frame);
                            break;
                        case 32768:
                            nnInterpolateFloat(ref pColor.Specular.b, pSubmotion, frame);
                            break;
                        case 57344:
                            nnInterpolateFloat3(ref pColor.Specular, pSubmotion, frame);
                            break;
                        case 65536:
                            nnInterpolateFloat(ref pColor.SpecularIntensity, pSubmotion, frame);
                            break;
                        case 131072:
                            float val = 0.0f;
                            if (nnInterpolateFloat(ref val, pSubmotion, frame) != 0)
                            {
                                pColor.Shininess = nnPow(2.0, 10.0 * val + 2.0);
                                break;
                            }
                            break;
                        case 262144:
                            nnInterpolateFloat(ref pColor.Ambient.r, pSubmotion, frame);
                            break;
                        case 524288:
                            nnInterpolateFloat(ref pColor.Ambient.g, pSubmotion, frame);
                            break;
                        case 1048576:
                            nnInterpolateFloat(ref pColor.Ambient.b, pSubmotion, frame);
                            break;
                        case 1835008:
                            nnInterpolateFloat3(ref pColor.Ambient, pSubmotion, frame);
                            break;
                        case 2097152:
                            nnInterpolateSint32(ref pTexDesc[(~pSubmotion).Id1].iTexIdx, pSubmotion, frame);
                            break;
                        case 4194304:
                            nnInterpolateFloat(ref pTexDesc[(~pSubmotion).Id1].Blend, pSubmotion, frame);
                            break;
                        case 8388608:
                            nnInterpolateFloat(ref pTexDesc[(~pSubmotion).Id1].Offset.u, pSubmotion, frame);
                            break;
                        case 16777216:
                            nnInterpolateFloat(ref pTexDesc[(~pSubmotion).Id1].Offset.v, pSubmotion, frame);
                            break;
                        case 25165824:
                            nnInterpolateFloat2(pTexDesc[(~pSubmotion).Id1].Offset, pSubmotion, frame);
                            break;
                        case 33554432:
                            nnInterpolateUint32(ref pMaterial.User, pSubmotion, frame);
                            break;
                    }
                    ++pSubmotion;
                }
            }
            else if (((int)fType & 8) != 0)
            {
                NNS_MATERIAL_GLES11_DESC pMaterial = (NNS_MATERIAL_GLES11_DESC)mmobj.pMatPtrList[index].pMaterial;
                NNS_MATERIAL_STDSHADER_COLOR pColor = pMaterial.pColor;
                NNS_MATERIAL_GLES11_TEXMAP_DESC[] pTexDesc = pMaterial.pTexDesc;
                for (; nSubmotion > 0 && (~pSubmotion).Id0 < index; --nSubmotion)
                    ++pSubmotion;
                for (; nSubmotion > 0 && (~pSubmotion).Id0 == index; --nSubmotion)
                {
                    switch ((~pSubmotion).fType & 4294967040U)
                    {
                        case 512:
                            nnInterpolateFloat(ref pColor.Diffuse.r, pSubmotion, frame);
                            break;
                        case 1024:
                            nnInterpolateFloat(ref pColor.Diffuse.g, pSubmotion, frame);
                            break;
                        case 2048:
                            nnInterpolateFloat(ref pColor.Diffuse.b, pSubmotion, frame);
                            break;
                        case 3584:
                            nnInterpolateFloat3(ref pColor.Diffuse, pSubmotion, frame);
                            break;
                        case 4096:
                            nnInterpolateFloat(ref pColor.Diffuse.a, pSubmotion, frame);
                            break;
                        case 8192:
                            nnInterpolateFloat(ref pColor.Specular.r, pSubmotion, frame);
                            break;
                        case 16384:
                            nnInterpolateFloat(ref pColor.Specular.g, pSubmotion, frame);
                            break;
                        case 32768:
                            nnInterpolateFloat(ref pColor.Specular.b, pSubmotion, frame);
                            break;
                        case 57344:
                            nnInterpolateFloat3(ref pColor.Specular, pSubmotion, frame);
                            break;
                        case 65536:
                            nnInterpolateFloat(ref pColor.SpecularIntensity, pSubmotion, frame);
                            break;
                        case 131072:
                            float val = 0.0f;
                            if (nnInterpolateFloat(ref val, pSubmotion, frame) != 0)
                            {
                                pColor.Shininess = nnPow(2.0, 10.0 * val + 2.0);
                                break;
                            }
                            break;
                        case 262144:
                            nnInterpolateFloat(ref pColor.Ambient.r, pSubmotion, frame);
                            break;
                        case 524288:
                            nnInterpolateFloat(ref pColor.Ambient.g, pSubmotion, frame);
                            break;
                        case 1048576:
                            nnInterpolateFloat(ref pColor.Ambient.b, pSubmotion, frame);
                            break;
                        case 1835008:
                            nnInterpolateFloat3(ref pColor.Ambient, pSubmotion, frame);
                            break;
                        case 2097152:
                            nnInterpolateSint32(ref pTexDesc[(~pSubmotion).Id1].iTexIdx, pSubmotion, frame);
                            break;
                        case 8388608:
                            nnInterpolateFloat(ref pTexDesc[(~pSubmotion).Id1].Offset.u, pSubmotion, frame);
                            break;
                        case 16777216:
                            nnInterpolateFloat(ref pTexDesc[(~pSubmotion).Id1].Offset.v, pSubmotion, frame);
                            break;
                        case 25165824:
                            nnInterpolateFloat2(pTexDesc[(~pSubmotion).Id1].Offset, pSubmotion, frame);
                            break;
                        case 33554432:
                            nnInterpolateUint32(ref pMaterial.User, pSubmotion, frame);
                            break;
                    }
                    ++pSubmotion;
                }
            }
            else if (((int)fType & 1) != 0)
            {
                NNS_MATERIAL_DESC pMaterial = (NNS_MATERIAL_DESC)mmobj.pMatPtrList[index].pMaterial;
                NNS_MATERIAL_COLOR pColor = pMaterial.pColor;
                NNS_MATERIAL_TEXMAP_DESC[] pTexDesc = pMaterial.pTexDesc;
                for (; nSubmotion > 0 && (~pSubmotion).Id0 < index; --nSubmotion)
                    ++pSubmotion;
                for (; nSubmotion > 0 && (~pSubmotion).Id0 == index; --nSubmotion)
                {
                    switch ((~pSubmotion).fType & 4294967040U)
                    {
                        case 512:
                            nnInterpolateFloat(ref pColor.Diffuse.r, pSubmotion, frame);
                            break;
                        case 1024:
                            nnInterpolateFloat(ref pColor.Diffuse.g, pSubmotion, frame);
                            break;
                        case 2048:
                            nnInterpolateFloat(ref pColor.Diffuse.b, pSubmotion, frame);
                            break;
                        case 3584:
                            nnInterpolateFloat3(ref pColor.Diffuse, pSubmotion, frame);
                            break;
                        case 4096:
                            nnInterpolateFloat(ref pColor.Diffuse.a, pSubmotion, frame);
                            break;
                        case 8192:
                            nnInterpolateFloat(ref pColor.Specular.r, pSubmotion, frame);
                            break;
                        case 16384:
                            nnInterpolateFloat(ref pColor.Specular.g, pSubmotion, frame);
                            break;
                        case 32768:
                            nnInterpolateFloat(ref pColor.Specular.b, pSubmotion, frame);
                            break;
                        case 57344:
                            nnInterpolateFloat3(ref pColor.Specular, pSubmotion, frame);
                            break;
                        case 65536:
                            float val1 = 0.0f;
                            if (nnInterpolateFloat(ref val1, pSubmotion, frame) != 0)
                            {
                                pColor.Specular.r *= val1;
                                pColor.Specular.g *= val1;
                                pColor.Specular.b *= val1;
                                break;
                            }
                            break;
                        case 131072:
                            float val2 = 0.0f;
                            if (nnInterpolateFloat(ref val2, pSubmotion, frame) != 0)
                            {
                                pColor.Shininess = nnPow(2.0, 10.0 * val2 + 2.0);
                                break;
                            }
                            break;
                        case 262144:
                            nnInterpolateFloat(ref pColor.Ambient.r, pSubmotion, frame);
                            break;
                        case 524288:
                            nnInterpolateFloat(ref pColor.Ambient.g, pSubmotion, frame);
                            break;
                        case 1048576:
                            nnInterpolateFloat(ref pColor.Ambient.b, pSubmotion, frame);
                            break;
                        case 1835008:
                            nnInterpolateFloat3(ref pColor.Ambient, pSubmotion, frame);
                            break;
                        case 2097152:
                            nnInterpolateSint32(ref pTexDesc[(~pSubmotion).Id1].iTexIdx, pSubmotion, frame);
                            break;
                        case 4194304:
                            float val3 = 0.0f;
                            if (nnInterpolateFloat(ref val3, pSubmotion, frame) != 0)
                            {
                                pTexDesc[(~pSubmotion).Id1].EnvColor.r = val3;
                                pTexDesc[(~pSubmotion).Id1].EnvColor.g = val3;
                                pTexDesc[(~pSubmotion).Id1].EnvColor.b = val3;
                                break;
                            }
                            break;
                        case 8388608:
                            nnInterpolateFloat(ref pTexDesc[(~pSubmotion).Id1].Offset.u, pSubmotion, frame);
                            break;
                        case 16777216:
                            nnInterpolateFloat(ref pTexDesc[(~pSubmotion).Id1].Offset.v, pSubmotion, frame);
                            break;
                        case 25165824:
                            nnInterpolateFloat2(pTexDesc[(~pSubmotion).Id1].Offset, pSubmotion, frame);
                            break;
                        case 33554432:
                            nnInterpolateUint32(ref pMaterial.User, pSubmotion, frame);
                            break;
                    }
                    ++pSubmotion;
                }
            }
        }
    }

    public static void nnDrawMaterialMotionObject(
      NNS_OBJECT mmobj,
      NNS_MATRIX[] mtxpal,
      uint[] nodestatlist,
      uint subobjtype,
      uint flag)
    {
        nnDrawObject(mmobj, mtxpal, nodestatlist, subobjtype, flag, 0U);
    }

    private static uint nnCalcClipBoxNode(NNS_NODE node, NNS_MATRIX mtx)
    {
        float boundingBoxX = node.BoundingBoxX;
        float boundingBoxY = node.BoundingBoxY;
        float boundingBoxZ = node.BoundingBoxZ;
        return nnCalcClipBox(node.Center, boundingBoxX, boundingBoxY, boundingBoxZ, mtx);
    }

    private static uint nnCalcClipBox(
      NNS_VECTOR center,
      float sx,
      float sy,
      float sz,
      NNS_MATRIX mtx)
    {
        uint num1 = 0;
        float nClip = nngClip3d.n_clip;
        float fClip = nngClip3d.f_clip;
        NNS_VECTORFAST dst1;
        nnmSetUpVectorFast(out dst1, center.x, center.y, center.z);
        NNS_VECTORFAST dst2;
        nnTransformVectorFast(out dst2, mtx, dst1);
        float num2 = sx * mtx.M20;
        float num3 = sy * mtx.M21;
        float num4 = sz * mtx.M22;
        float num5 = nnAbs(num2) + nnAbs(num3) + nnAbs(num4);
        if (dst2.z > -nClip + (double)num5 || dst2.z < -fClip - (double)num5)
            return 16;
        if (dst2.z > -nClip - (double)num5)
            num1 |= 260U;
        if (dst2.z < -fClip + (double)num5)
            num1 |= 520U;
        float num6 = sx * mtx.M00;
        float num7 = sy * mtx.M01;
        float num8 = sz * mtx.M02;
        if (nngProjectionType != 1)
        {
            float num9 = nnAbs(num6 * (double)nngClipPlane.Right.nx + num2 * (double)nngClipPlane.Right.nz) + nnAbs(num7 * (double)nngClipPlane.Right.nx + num3 * (double)nngClipPlane.Right.nz) + nnAbs(num8 * (double)nngClipPlane.Right.nx + num4 * (double)nngClipPlane.Right.nz);
            float num10 = (float)(dst2.x * (double)nngClipPlane.Right.nx + dst2.z * (double)nngClipPlane.Right.nz);
            if (num10 > (double)num9)
                return 16;
            if (num10 > -(double)num9)
                num1 |= 4096U;
            float num11 = nnAbs(num6 * (double)nngClipPlane.Left.nx + num2 * (double)nngClipPlane.Left.nz) + nnAbs(num7 * (double)nngClipPlane.Left.nx + num3 * (double)nngClipPlane.Left.nz) + nnAbs(num8 * (double)nngClipPlane.Left.nx + num4 * (double)nngClipPlane.Left.nz);
            float num12 = (float)(dst2.x * (double)nngClipPlane.Left.nx + dst2.z * (double)nngClipPlane.Left.nz);
            if (num12 > (double)num11)
                return 16;
            if (num12 > -(double)num11)
                num1 |= 8192U;
            float num13 = sx * mtx.M10;
            float num14 = sy * mtx.M11;
            float num15 = sz * mtx.M12;
            float num16 = nnAbs(num13 * (double)nngClipPlane.Top.ny + num2 * (double)nngClipPlane.Top.nz) + nnAbs(num14 * (double)nngClipPlane.Top.ny + num3 * (double)nngClipPlane.Top.nz) + nnAbs(num15 * (double)nngClipPlane.Top.ny + num4 * (double)nngClipPlane.Top.nz);
            float num17 = (float)(dst2.y * (double)nngClipPlane.Top.ny + dst2.z * (double)nngClipPlane.Top.nz);
            if (num17 > (double)num16)
                return 16;
            if (num17 > -(double)num16)
                num1 |= 16384U;
            float num18 = nnAbs(num13 * (double)nngClipPlane.Bottom.ny + num2 * (double)nngClipPlane.Bottom.nz) + nnAbs(num14 * (double)nngClipPlane.Bottom.ny + num3 * (double)nngClipPlane.Bottom.nz) + nnAbs(num15 * (double)nngClipPlane.Bottom.ny + num4 * (double)nngClipPlane.Bottom.nz);
            float num19 = (float)(dst2.y * (double)nngClipPlane.Bottom.ny + dst2.z * (double)nngClipPlane.Bottom.nz);
            if (num19 > (double)num18)
                return 16;
            if (num19 > -(double)num18)
                num1 |= 32768U;
            if (num1 == 0U)
                return 2;
        }
        else
        {
            float num9 = nnAbs(num6) + nnAbs(num7) + nnAbs(num8);
            float num10 = dst2.x - nngClipPlane.Right.mul - nngClipPlane.Right.ofs;
            if (num10 > (double)num9)
                return 16;
            if (num10 > -(double)num9)
                num1 |= 4096U;
            float num11 = dst2.x - nngClipPlane.Left.mul - nngClipPlane.Left.ofs;
            if (num11 < -(double)num9)
                return 16;
            if (num11 < (double)num9)
                num1 |= 8192U;
            float num12 = nnAbs(sx * mtx.M10) + nnAbs(sy * mtx.M11) + nnAbs(sz * mtx.M12);
            float num13 = dst2.y - nngClipPlane.Top.mul - nngClipPlane.Top.ofs;
            if (num13 > (double)num12)
                return 16;
            if (num13 > -(double)num12)
                num1 |= 16384U;
            float num14 = dst2.y - nngClipPlane.Bottom.mul - nngClipPlane.Bottom.ofs;
            if (num14 < -(double)num12)
                return 16;
            if (num14 < (double)num12)
                num1 |= 32768U;
            if (num1 == 0U)
                return 2;
        }
        return num1 & 62U;
    }

    private static uint nnCalcClipCore(
      NNS_VECTOR center,
      float radius,
      NNS_MATRIX mtx)
    {
        uint num1 = 0;
        float nClip = nngClip3d.n_clip;
        float fClip = nngClip3d.f_clip;
        NNS_VECTORFAST dst1;
        nnmSetUpVectorFast(out dst1, center.x, center.y, center.z);
        NNS_VECTORFAST dst2;
        nnTransformVectorFast(out dst2, mtx, dst1);
        if (dst2.z > -nClip + (double)radius || dst2.z < -fClip - (double)radius)
            return 16;
        if (dst2.z > -nClip - (double)radius)
            num1 |= 260U;
        if (dst2.z < -fClip + (double)radius)
            num1 |= 520U;
        if (nngProjectionType != 1)
        {
            float num2 = (float)(dst2.x * (double)nngClipPlane.Right.nx + dst2.z * (double)nngClipPlane.Right.nz);
            if (num2 > (double)radius)
                return 16;
            if (num2 > -(double)radius)
                num1 |= 4096U;
            float num3 = (float)(dst2.x * (double)nngClipPlane.Left.nx + dst2.z * (double)nngClipPlane.Left.nz);
            if (num3 > (double)radius)
                return 16;
            if (num3 > -(double)radius)
                num1 |= 8192U;
            float num4 = (float)(dst2.y * (double)nngClipPlane.Top.ny + dst2.z * (double)nngClipPlane.Top.nz);
            if (num4 > (double)radius)
                return 16;
            if (num4 > -(double)radius)
                num1 |= 16384U;
            float num5 = (float)(dst2.y * (double)nngClipPlane.Bottom.ny + dst2.z * (double)nngClipPlane.Bottom.nz);
            if (num5 > (double)radius)
                return 16;
            if (num5 > -(double)radius)
                num1 |= 32768U;
            if (num1 == 0U)
                return 2;
        }
        else
        {
            float num2 = dst2.y - nngClipPlane.Top.mul - nngClipPlane.Top.ofs;
            if (num2 > (double)radius)
                return 16;
            if (num2 > -(double)radius)
                num1 |= 16384U;
            float num3 = dst2.y - nngClipPlane.Bottom.mul - nngClipPlane.Bottom.ofs;
            if (num3 < -(double)radius)
                return 16;
            if (num3 < (double)radius)
                num1 |= 32768U;
            float num4 = dst2.x - nngClipPlane.Right.mul - nngClipPlane.Right.ofs;
            if (num4 > (double)radius)
                return 16;
            if (num4 > -(double)radius)
                num1 |= 4096U;
            float num5 = dst2.x - nngClipPlane.Left.mul - nngClipPlane.Left.ofs;
            if (num5 < -(double)radius)
                return 16;
            if (num5 < (double)radius)
                num1 |= 8192U;
            if (num1 == 0U)
                return 2;
        }
        return num1 & 62U;
    }

    private static uint nnCalcClip(NNS_VECTOR center, float radius, NNS_MATRIX mtx)
    {
        if (radius == 0.0)
            return 0;
        radius *= nnEstimateMatrixScaling(mtx);
        return nnCalcClipCore(center, radius, mtx);
    }

    private static uint nnCalcClipUniformScale(
      NNS_VECTOR center,
      float radius,
      NNS_MATRIX mtx,
      float factor)
    {
        if (radius == 0.0)
            return 0;
        radius *= factor;
        return nnCalcClipCore(center, radius, mtx);
    }

    private static void nnCalcClipSetNodeStatus(
      uint[] pNodeStatList,
      NNS_NODE[] pNodeList,
      int nodeIdx,
      NNS_MATRIX pNodeMtx,
      float rootscale,
      uint flag)
    {
        nnclip.nnsNodeStatList = pNodeStatList;
        nnclip.nnsNodeList = pNodeList;
        NNS_NODE pNode = pNodeList[nodeIdx];
        if (((int)pNodeStatList[nodeIdx] & 1025) != 0)
            return;
        if (((int)pNode.fType & 16) != 0)
            pNodeStatList[nodeIdx] |= 1U;
        if (((int)pNode.fType & 32) != 0)
        {
            pNodeStatList[nodeIdx] |= 1U;
            if (pNode.iChild == -1)
                return;
            nnSetUpNodeStatusListFlag(pNode.iChild, 1U);
        }
        else
        {
            if (flag == 0U || pNode.iMatrix == -1 && ((int)flag & 8) == 0)
                return;
            if (((int)pNode.fType & 2097152) != 0 && ((int)pNode.fType & 4194304) == 0 && ((int)flag & 32) == 0)
                pNodeStatList[nodeIdx] |= nnCalcClipBoxNode(pNode, pNodeMtx);
            else if (((int)flag & 16) != 0)
                pNodeStatList[nodeIdx] |= nnCalcClip(pNode.Center, pNode.Radius, pNodeMtx);
            else
                pNodeStatList[nodeIdx] |= nnCalcClipUniformScale(pNode.Center, pNode.Radius, pNodeMtx, rootscale);
            if (((int)pNodeStatList[nodeIdx] & 16) == 0)
                return;
            if (((int)flag & 2) != 0)
                pNodeStatList[nodeIdx] |= 1024U;
            else
                pNodeStatList[nodeIdx] |= 1U;
            if (((int)flag & 8) == 0 || pNode.iChild == -1)
                return;
            nnSetUpNodeStatusListFlag(pNode.iChild, pNodeStatList[nodeIdx]);
        }
    }

    private static void nnSetUpNodeStatusListFlag(int nodeidx, uint flag)
    {
        mppAssertNotImpl();
    }

    private uint nnCheckObjectClip(NNS_OBJECT obj, NNS_MATRIX basemtx)
    {
        mppAssertNotImpl();
        return 0;
    }

    private uint nnCheckObjectClipMotionCore(
      NNS_OBJECT obj,
      NNS_MOTION mot,
      float frame,
      NNS_MATRIX basemtx)
    {
        mppAssertNotImpl();
        return 0;
    }

    private uint nnCheckObjectClipMotion(
      NNS_OBJECT obj,
      NNS_MOTION mot,
      float frame,
      NNS_MATRIX basemtx)
    {
        mppAssertNotImpl();
        return uint.MaxValue;
    }

    private static float nnEstimateMatrixScaling(NNS_MATRIX mtx)
    {
        NNS_VECTORFAST dst1;
        nnSetUpVectorFast(out dst1, mtx.M00, mtx.M10, mtx.M20);
        NNS_VECTORFAST dst2;
        nnSetUpVectorFast(out dst2, mtx.M01, mtx.M11, mtx.M21);
        NNS_VECTORFAST dst3;
        nnSetUpVectorFast(out dst3, mtx.M02, mtx.M12, mtx.M22);
        return nnSqrt(NNM_MAX(NNM_MAX(nnLengthSqVectorFast(ref dst1), nnLengthSqVectorFast(ref dst2)), nnLengthSqVectorFast(ref dst3)) + 2f * NNM_MAX(NNM_MAX(nnAbs(nnDotProductVectorFast(ref dst1, ref dst2)), nnAbs(nnDotProductVectorFast(ref dst1, ref dst3))), nnAbs(nnDotProductVectorFast(ref dst2, ref dst3))));
    }

    public static uint NND_DRAWOBJ_SHADER_USER_PROFILE(byte _n)
    {
        return (uint)((_n & byte.MaxValue) << 2);
    }

    private static void nnSetMaterialCallback(NNS_MATERIALCALLBACK_FUNC func)
    {
        nngMaterialCallbackFunc = func;
    }

    private static NNS_MATERIALCALLBACK_FUNC nnGetMaterialCallback()
    {
        return nngMaterialCallbackFunc;
    }

    private static int nnPutMaterial(NNS_DRAWCALLBACK_VAL val)
    {
        return nngMaterialCallbackFunc != null ? nngMaterialCallbackFunc(val) : nnPutMaterialCore(val);
    }

}