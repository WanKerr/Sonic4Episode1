using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static float NNM_TAYLOR_SIN(float f, float f2)
    {
        return f * (float)(1.0 + (double)f2 * (-0.16666667163372 - (double)f2 * -0.00833333376795053));
    }

    private static float NNM_TAYLOR_COS(float f, float f2)
    {
        return (float)(1.0 + (double)f2 * ((double)f2 * (0.0416666679084301 + (double)f2 * (-1.0 / 720.0)) - 0.5));
    }

    private static float nnSin(int ang)
    {
        float num = 0.0f;
        int n = ang & (int)ushort.MaxValue;
        switch (ang & 57344)
        {
            case 0:
                float f1 = AppMain.NNM_A32toRAD(n);
                float f2_1 = f1 * f1;
                num = AppMain.NNM_TAYLOR_SIN(f1, f2_1);
                break;
            case 8192:
            case 16384:
                float f2 = AppMain.NNM_A32toRAD(n - 16384);
                float f2_2 = f2 * f2;
                num = AppMain.NNM_TAYLOR_COS(f2, f2_2);
                break;
            case 24576:
            case 32768:
                float f3 = AppMain.NNM_A32toRAD(n - 32768);
                float f2_3 = f3 * f3;
                num = -AppMain.NNM_TAYLOR_SIN(f3, f2_3);
                break;
            case 40960:
            case 49152:
                float f4 = AppMain.NNM_A32toRAD(n - 49152);
                float f2_4 = f4 * f4;
                num = -AppMain.NNM_TAYLOR_COS(f4, f2_4);
                break;
            case 57344:
                float f5 = AppMain.NNM_A32toRAD(n - 65536);
                float f2_5 = f5 * f5;
                num = AppMain.NNM_TAYLOR_SIN(f5, f2_5);
                break;
        }
        return num;
    }

    private static float nnCos(int ang)
    {
        return AppMain.nnSin(ang + 16384);
    }

    private static void nnCalcSinCosTable()
    {
        for (int index = 0; index < 36000; ++index)
            AppMain._nnSinCos((int)((double)index * 1.82044434547424), out AppMain.nnSinTable[index], out AppMain.nnCosTable[index]);
    }

    private static void nnSinCos(int ang, out float s, out float c)
    {
        int index = (int)Math.Round((double)ang * 0.54931640625);
        while (index > 35999)
            index -= 36000;
        while (index < 0)
            index += 36000;
        s = AppMain.nnSinTable[index];
        c = AppMain.nnCosTable[index];
    }

    private static void _nnSinCos(int ang, out float s, out float c)
    {
        if (ang == 0)
        {
            s = 0.0f;
            c = 1f;
        }
        int n = ang & (int)ushort.MaxValue;
        c = 0.0f;
        s = 0.0f;
        switch (ang & 57344)
        {
            case 0:
                float f1 = AppMain.NNM_A32toRAD(n);
                float f2_1 = f1 * f1;
                s = AppMain.NNM_TAYLOR_SIN(f1, f2_1);
                c = AppMain.NNM_TAYLOR_COS(f1, f2_1);
                break;
            case 8192:
            case 16384:
                float f2 = AppMain.NNM_A32toRAD(n - 16384);
                float f2_2 = f2 * f2;
                s = AppMain.NNM_TAYLOR_COS(f2, f2_2);
                c = -AppMain.NNM_TAYLOR_SIN(f2, f2_2);
                break;
            case 24576:
            case 32768:
                float f3 = AppMain.NNM_A32toRAD(n - 32768);
                float f2_3 = f3 * f3;
                s = -AppMain.NNM_TAYLOR_SIN(f3, f2_3);
                c = -AppMain.NNM_TAYLOR_COS(f3, f2_3);
                break;
            case 40960:
            case 49152:
                float f4 = AppMain.NNM_A32toRAD(n - 49152);
                float f2_4 = f4 * f4;
                s = -AppMain.NNM_TAYLOR_COS(f4, f2_4);
                c = AppMain.NNM_TAYLOR_SIN(f4, f2_4);
                break;
            case 57344:
                float f5 = AppMain.NNM_A32toRAD(n - 65536);
                float f2_5 = f5 * f5;
                s = AppMain.NNM_TAYLOR_SIN(f5, f2_5);
                c = AppMain.NNM_TAYLOR_COS(f5, f2_5);
                break;
        }
    }


    public static void nnmSetUpVectorFast(out AppMain.NNS_VECTORFAST dst, float x, float y, float z)
    {
        dst.x = x;
        dst.y = y;
        dst.z = z;
        dst.w = 1f;
    }

    public static void nnSetUpVectorFast(out AppMain.NNS_VECTORFAST dst, float x, float y, float z)
    {
        dst.x = x;
        dst.y = y;
        dst.z = z;
        dst.w = 1f;
    }

    public static void nnAddVector(
      ref AppMain.SNNS_VECTOR dst,
      ref AppMain.SNNS_VECTOR vec1,
      ref AppMain.SNNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnAddVector(
      AppMain.NNS_VECTOR dst,
      AppMain.NNS_VECTOR vec1,
      AppMain.NNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnAddVector(
      ref AppMain.Vector3f dst,
      ref AppMain.SNNS_VECTOR vec1,
      ref AppMain.SNNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnAddVector(
      ref AppMain.Vector3f dst,
      ref AppMain.SNNS_VECTOR vec1,
      AppMain.NNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnAddVector(
      ref AppMain.Vector3f dst,
      AppMain.NNS_VECTOR vec1,
      ref AppMain.SNNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnAddVector(
      ref AppMain.Vector3f dst,
      AppMain.NNS_VECTOR vec1,
      AppMain.NNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnAddVector(
      ref AppMain.Vector3f dst,
      ref AppMain.Vector3f vec1,
      AppMain.NNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnAddVector(
      ref AppMain.Vector3f dst,
      ref AppMain.Vector3f vec1,
      ref AppMain.SNNS_VECTOR vec2)
    {
        dst.x = vec1.x + vec2.x;
        dst.y = vec1.y + vec2.y;
        dst.z = vec1.z + vec2.z;
    }

    public static void nnCrossProductVector(
      ref AppMain.SNNS_VECTOR dst,
      ref AppMain.SNNS_VECTOR vec1,
      ref AppMain.SNNS_VECTOR vec2)
    {
        float num1 = (float)((double)vec1.y * (double)vec2.z - (double)vec1.z * (double)vec2.y);
        float num2 = (float)((double)vec1.z * (double)vec2.x - (double)vec1.x * (double)vec2.z);
        float num3 = (float)((double)vec1.x * (double)vec2.y - (double)vec1.y * (double)vec2.x);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    public static void nnCrossProductVector(
      AppMain.NNS_VECTOR dst,
      AppMain.NNS_VECTOR vec1,
      AppMain.NNS_VECTOR vec2)
    {
        float num1 = (float)((double)vec1.y * (double)vec2.z - (double)vec1.z * (double)vec2.y);
        float num2 = (float)((double)vec1.z * (double)vec2.x - (double)vec1.x * (double)vec2.z);
        float num3 = (float)((double)vec1.x * (double)vec2.y - (double)vec1.y * (double)vec2.x);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    public static void nnCopyVector(AppMain.NNS_VECTOR dst, ref AppMain.SNNS_VECTOR src)
    {
        dst.Assign(ref src);
    }

    public static void nnCopyVector(AppMain.NNS_VECTOR dst, ref AppMain.SNNS_VECTOR4D src)
    {
        dst.Assign(ref src);
    }

    public static void nnCopyVector(AppMain.NNS_VECTOR dst, AppMain.NNS_VECTOR src)
    {
        dst.Assign(src);
    }

    public static float nnDotProductVector(AppMain.NNS_VECTOR vec1, AppMain.NNS_VECTOR vec2)
    {
        return (float)((double)vec1.x * (double)vec2.x + (double)vec1.y * (double)vec2.y + (double)vec1.z * (double)vec2.z);
    }

    public static float nnLengthVector(AppMain.NNS_VECTOR vec)
    {
        return AppMain.nnSqrt((float)((double)vec.x * (double)vec.x + (double)vec.y * (double)vec.y + (double)vec.z * (double)vec.z));
    }

    public static float nnDistanceVector(AppMain.NNS_VECTOR vec1, ref AppMain.SNNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return AppMain.nnSqrt((float)((double)num1 * (double)num1 + (double)num2 * (double)num2 + (double)num3 * (double)num3));
    }

    public static float nnDistanceVector(ref AppMain.SNNS_VECTOR vec1, AppMain.NNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return AppMain.nnSqrt((float)((double)num1 * (double)num1 + (double)num2 * (double)num2 + (double)num3 * (double)num3));
    }

    public static float nnDistanceVector(AppMain.NNS_VECTOR vec1, AppMain.NNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return AppMain.nnSqrt((float)((double)num1 * (double)num1 + (double)num2 * (double)num2 + (double)num3 * (double)num3));
    }

    public static float nnDistanceVector(ref AppMain.Vector3f vec1, ref AppMain.SNNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return AppMain.nnSqrt((float)((double)num1 * (double)num1 + (double)num2 * (double)num2 + (double)num3 * (double)num3));
    }

    public static float nnDistanceVector(ref AppMain.Vector3f vec1, AppMain.NNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return AppMain.nnSqrt((float)((double)num1 * (double)num1 + (double)num2 * (double)num2 + (double)num3 * (double)num3));
    }

    public static float nnLengthSqVector(AppMain.NNS_VECTOR vec)
    {
        return (float)((double)vec.x * (double)vec.x + (double)vec.y * (double)vec.y + (double)vec.z * (double)vec.z);
    }

    public static float nnLengthSqVector(ref AppMain.SNNS_VECTOR vec)
    {
        return (float)((double)vec.x * (double)vec.x + (double)vec.y * (double)vec.y + (double)vec.z * (double)vec.z);
    }

    public static float nnLengthSqVector(float[] vec)
    {
        return (float)((double)vec[0] * (double)vec[0] + (double)vec[1] * (double)vec[1] + (double)vec[2] * (double)vec[2]);
    }

    public static float nnLengthSqVector(ref OpenGL.glArray4f vec)
    {
        return (float)((double)vec.f0 * (double)vec.f0 + (double)vec.f1 * (double)vec.f1 + (double)vec.f2 * (double)vec.f2);
    }

    public static float nnDistanceSqVector(AppMain.NNS_VECTOR vec1, AppMain.NNS_VECTOR vec2)
    {
        float num1 = vec2.x - vec1.x;
        float num2 = vec2.y - vec1.y;
        float num3 = vec2.z - vec1.z;
        return (float)((double)num1 * (double)num1 + (double)num2 * (double)num2 + (double)num3 * (double)num3);
    }

    public static int nnNormalizeVector(AppMain.NNS_VECTOR dst, AppMain.NNS_VECTOR src)
    {
        float n = AppMain.nnLengthSqVector(src);
        if ((double)n == 0.0)
        {
            dst.x = 0.0f;
            dst.y = 0.0f;
            dst.z = 0.0f;
            return 0;
        }
        float num = AppMain.nnInvertSqrt(n);
        dst.x = src.x * num;
        dst.y = src.y * num;
        dst.z = src.z * num;
        return 1;
    }

    public static int nnNormalizeVector(ref AppMain.SNNS_VECTOR dst, ref AppMain.SNNS_VECTOR src)
    {
        float n = AppMain.nnLengthSqVector(ref src);
        if ((double)n == 0.0)
        {
            dst.x = 0.0f;
            dst.y = 0.0f;
            dst.z = 0.0f;
            return 0;
        }
        float num = AppMain.nnInvertSqrt(n);
        dst.x = src.x * num;
        dst.y = src.y * num;
        dst.z = src.z * num;
        return 1;
    }

    public static int nnNormalizeVector(float[] dst, float[] src)
    {
        float n = AppMain.nnLengthSqVector(src);
        if ((double)n == 0.0)
        {
            dst[0] = 0.0f;
            dst[1] = 0.0f;
            dst[2] = 0.0f;
            return 0;
        }
        float num = AppMain.nnInvertSqrt(n);
        dst[0] = src[0] * num;
        dst[1] = src[1] * num;
        dst[2] = src[2] * num;
        return 1;
    }

    public static int nnNormalizeVector(ref OpenGL.glArray4f dst, ref OpenGL.glArray4f src)
    {
        float n = AppMain.nnLengthSqVector(ref src);
        if ((double)n == 0.0)
        {
            dst.f0 = 0.0f;
            dst.f1 = 0.0f;
            dst.f2 = 0.0f;
            return 0;
        }
        float num = AppMain.nnInvertSqrt(n);
        dst.f0 = src.f0 * num;
        dst.f1 = src.f1 * num;
        dst.f2 = src.f2 * num;
        return 1;
    }

    public static void nnScaleVector(
      ref AppMain.SNNS_VECTOR dst,
      ref AppMain.SNNS_VECTOR src,
      float scale)
    {
        dst.x = src.x * scale;
        dst.y = src.y * scale;
        dst.z = src.z * scale;
    }

    public static void nnScaleVector(AppMain.NNS_VECTOR dst, AppMain.NNS_VECTOR src, float scale)
    {
        dst.x = src.x * scale;
        dst.y = src.y * scale;
        dst.z = src.z * scale;
    }

    public static void nnScaleAddVector(
      AppMain.NNS_VECTOR dst,
      AppMain.NNS_VECTOR vec1,
      AppMain.NNS_VECTOR vec2,
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
      AppMain.NNS_VECTOR dst,
      AppMain.NNS_VECTOR vec1,
      AppMain.NNS_VECTOR vec2)
    {
        dst.x = vec1.x - vec2.x;
        dst.y = vec1.y - vec2.y;
        dst.z = vec1.z - vec2.z;
    }

    public static void nnSubtractVector(
      ref AppMain.Vector3f dst,
      AppMain.NNS_VECTOR vec1,
      AppMain.NNS_VECTOR vec2)
    {
        dst.x = vec1.x - vec2.x;
        dst.y = vec1.y - vec2.y;
        dst.z = vec1.z - vec2.z;
    }

    public static void nnSubtractVector(
      ref AppMain.Vector3f dst,
      ref AppMain.SNNS_VECTOR vec1,
      AppMain.NNS_VECTOR vec2)
    {
        dst.x = vec1.x - vec2.x;
        dst.y = vec1.y - vec2.y;
        dst.z = vec1.z - vec2.z;
    }

    public static void nnSubtractVector(
      ref AppMain.Vector3f dst,
      ref AppMain.SNNS_VECTOR vec1,
      ref AppMain.SNNS_VECTOR vec2)
    {
        dst.x = vec1.x - vec2.x;
        dst.y = vec1.y - vec2.y;
        dst.z = vec1.z - vec2.z;
    }

    public static void nnSubtractVector(
      ref AppMain.Vector3f dst,
      ref AppMain.Vector3f vec1,
      AppMain.NNS_VECTOR vec2)
    {
        dst.x = vec1.x - vec2.x;
        dst.y = vec1.y - vec2.y;
        dst.z = vec1.z - vec2.z;
    }

    public static void nnSubtractVector(
      ref AppMain.Vector3f dst,
      ref AppMain.Vector3f vec1,
      ref AppMain.SNNS_VECTOR vec2)
    {
        dst.x = vec1.x - vec2.x;
        dst.y = vec1.y - vec2.y;
        dst.z = vec1.z - vec2.z;
    }

    private static void nnTransformVector(
      ref AppMain.SNNS_VECTOR dst,
      ref AppMain.SNNS_MATRIX mtx,
      ref AppMain.SNNS_VECTOR src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformVector(
      AppMain.NNS_VECTOR dst,
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformVector(
      ref AppMain.Vector3f dst,
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformVector(
      ref AppMain.Vector3f dst,
      ref AppMain.SNNS_MATRIX mtx,
      ref AppMain.SNNS_VECTOR src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformVector(
      ref AppMain.Vector3f dst,
      AppMain.NNS_MATRIX mtx,
      ref AppMain.SNNS_VECTOR src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformVector(
      ref AppMain.Vector3f dst,
      AppMain.NNS_MATRIX mtx,
      ref AppMain.Vector3f src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformVector(
      AppMain.NNS_VECTOR4D dst,
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR4D src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformVector(
      ref AppMain.SNNS_VECTOR4D dst,
      AppMain.NNS_MATRIX mtx,
      ref AppMain.SNNS_VECTOR4D src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z) + mtx.M03;
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z) + mtx.M13;
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z) + mtx.M23;
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformNormalVector(
      AppMain.NNS_VECTOR dst,
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z);
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z);
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformNormalVector(
      AppMain.NNS_VECTOR4D dst,
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR4D src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z);
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z);
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformNormalVector(
      ref AppMain.SNNS_VECTOR4D dst,
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR4D src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z);
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z);
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformNormalVector(
      ref AppMain.SNNS_VECTOR4D dst,
      AppMain.NNS_MATRIX mtx,
      ref AppMain.SNNS_VECTOR4D src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z);
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z);
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnTransformNormalVector(
      AppMain.NNS_VECTOR4D dst,
      AppMain.NNS_MATRIX mtx,
      ref AppMain.SNNS_VECTOR4D src)
    {
        float num1 = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z);
        float num2 = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z);
        float num3 = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z);
        dst.x = num1;
        dst.y = num2;
        dst.z = num3;
    }

    private static void nnCopyMatrixTranslationVector(AppMain.NNS_VECTOR dst, AppMain.NNS_MATRIX mtx)
    {
        dst.x = mtx.M03;
        dst.y = mtx.M13;
        dst.z = mtx.M23;
    }

    private static void nnCopyMatrixTranslationVector(
      out AppMain.SNNS_VECTOR dst,
      AppMain.NNS_MATRIX mtx)
    {
        dst.x = mtx.M03;
        dst.y = mtx.M13;
        dst.z = mtx.M23;
    }

    private static void nnCopyMatrixTranslationVector(
      out AppMain.SNNS_VECTOR dst,
      ref AppMain.SNNS_MATRIX mtx)
    {
        dst.x = mtx.M03;
        dst.y = mtx.M13;
        dst.z = mtx.M23;
    }

    private static void nnCopyMatrixTranslationVector(
      AppMain.NNS_VECTOR dst,
      ref AppMain.SNNS_MATRIX mtx)
    {
        dst.x = mtx.M03;
        dst.y = mtx.M13;
        dst.z = mtx.M23;
    }

    private static void nnSubtractVectorFast(
      out AppMain.NNS_VECTORFAST dst,
      AppMain.NNS_VECTORFAST vec1,
      AppMain.NNS_VECTORFAST vec2)
    {
        float x = vec1.x - vec2.x;
        float y = vec1.y - vec2.y;
        float z = vec1.z - vec2.z;
        AppMain.nnmSetUpVectorFast(out dst, x, y, z);
    }

    private static void nnTransformVectorFast(
      out AppMain.NNS_VECTORFAST dst,
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTORFAST src)
    {
        float x = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z) + mtx.M03;
        float y = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z) + mtx.M13;
        float z = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z) + mtx.M23;
        AppMain.nnmSetUpVectorFast(out dst, x, y, z);
    }

    private static void nnTransformNormalVectorFast(
      out AppMain.NNS_VECTORFAST dst,
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTORFAST src)
    {
        float x = (float)((double)mtx.M00 * (double)src.x + (double)mtx.M01 * (double)src.y + (double)mtx.M02 * (double)src.z);
        float y = (float)((double)mtx.M10 * (double)src.x + (double)mtx.M11 * (double)src.y + (double)mtx.M12 * (double)src.z);
        float z = (float)((double)mtx.M20 * (double)src.x + (double)mtx.M21 * (double)src.y + (double)mtx.M22 * (double)src.z);
        AppMain.nnmSetUpVectorFast(out dst, x, y, z);
    }

    private static void nnCopyMatrixTranslationVectorFast(
      out AppMain.NNS_VECTORFAST dst,
      AppMain.NNS_MATRIX mtx)
    {
        dst.x = mtx.M03;
        dst.y = mtx.M13;
        dst.z = mtx.M23;
        dst.w = 1f;
    }

    private static float nnLengthSqVectorFast(ref AppMain.NNS_VECTORFAST vec)
    {
        return (float)((double)vec.x * (double)vec.x + (double)vec.y * (double)vec.y + (double)vec.z * (double)vec.z);
    }

    private static float nnDotProductVectorFast(
      ref AppMain.NNS_VECTORFAST vec1,
      ref AppMain.NNS_VECTORFAST vec2)
    {
        return (float)((double)vec1.x * (double)vec2.x + (double)vec1.y * (double)vec2.y + (double)vec1.z * (double)vec2.z);
    }
    private void nnMakeCameraPointerViewMatrix(AppMain.NNS_MATRIX mtx, AppMain.NNS_CAMERAPTR camptr)
    {
        AppMain.mppAssertNotImpl();
        switch (camptr.fType)
        {
            case (uint)byte.MaxValue:
                AppMain.nnMakeTargetRollCameraViewMatrix(mtx, (AppMain.NNS_CAMERA_TARGET_ROLL)camptr.pCamera);
                break;
            case 383:
                AppMain.nnMakeTargetUpVectorCameraViewMatrix(mtx, (AppMain.NNS_CAMERA_TARGET_UPVECTOR)camptr.pCamera);
                break;
            case 639:
                AppMain.nnMakeTargetUpTargetCameraViewMatrix(mtx, (AppMain.NNS_CAMERA_TARGET_UPTARGET)camptr.pCamera);
                break;
            case 3135:
                this.nnMakeRotationCameraViewMatrix(mtx, (AppMain.NNS_CAMERA_ROTATION)camptr.pCamera);
                break;
        }
    }

    private void nnMakeCameraPointerPerspectiveMatrix(
      AppMain.NNS_MATRIX dst,
      AppMain.NNS_CAMERAPTR camptr)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnMakeTargetRollCameraViewMatrix(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_CAMERA_TARGET_ROLL cam)
    {
        AppMain.SNNS_VECTOR snnsVector1 = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector2 = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector3 = new AppMain.SNNS_VECTOR();
        snnsVector3.x = cam.Position.x - cam.Target.x;
        snnsVector3.y = cam.Position.y - cam.Target.y;
        snnsVector3.z = cam.Position.z - cam.Target.z;
        AppMain.nnNormalizeVector(ref snnsVector3, ref snnsVector3);
        snnsVector1.x = snnsVector3.z;
        snnsVector1.y = 0.0f;
        snnsVector1.z = -snnsVector3.x;
        AppMain.nnNormalizeVector(ref snnsVector1, ref snnsVector1);
        AppMain.nnCrossProductVector(ref snnsVector2, ref snnsVector3, ref snnsVector1);
        AppMain.nnMakeVectorCameraViewMatrix(mtx, cam.Position, ref snnsVector1, ref snnsVector2, ref snnsVector3);
        AppMain.SNNS_MATRIX dst;
        AppMain.nnMakeRotateZMatrix(out dst, -cam.Roll);
        AppMain.nnMultiplyMatrix(mtx, ref dst, mtx);
    }

    private static void nnMakeTargetUpVectorCameraViewMatrix(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_CAMERA_TARGET_UPVECTOR cam)
    {
        AppMain.NNS_VECTOR nnsVector1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector3 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector1.x = cam.Position.x - cam.Target.x;
        nnsVector1.y = cam.Position.y - cam.Target.y;
        nnsVector1.z = cam.Position.z - cam.Target.z;
        AppMain.nnNormalizeVector(nnsVector1, nnsVector1);
        AppMain.nnCrossProductVector(nnsVector2, cam.UpVector, nnsVector1);
        AppMain.nnNormalizeVector(nnsVector2, nnsVector2);
        AppMain.nnCrossProductVector(nnsVector3, nnsVector1, nnsVector2);
        AppMain.nnMakeVectorCameraViewMatrix(mtx, cam.Position, nnsVector2, nnsVector3, nnsVector1);
    }

    private static void nnMakeTargetUpTargetCameraViewMatrix(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_CAMERA_TARGET_UPTARGET cam)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnMakeRotationCameraViewMatrix(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_CAMERA_ROTATION cam)
    {
        AppMain.mppAssertNotImpl();
    }


    public static void nnmRotateMatrixFast(AppMain.NNS_MATRIX mtx, int ang, int ma, int mb)
    {
        if (ang == 0)
            return;
        float s;
        float c;
        AppMain.nnSinCos(ang, out s, out c);
        float num1 = s;
        float num2 = c;
        float num3 = mtx.M(0, ma);
        float num4 = mtx.M(0, mb);
        mtx.SetM(0, ma, (float)((double)num3 * (double)num2 + (double)num4 * (double)num1));
        mtx.SetM(0, mb, (float)((double)num3 * -(double)num1 + (double)num4 * (double)num2));
        float num5 = mtx.M(1, ma);
        float num6 = mtx.M(1, mb);
        mtx.SetM(1, ma, (float)((double)num5 * (double)num2 + (double)num6 * (double)num1));
        mtx.SetM(1, mb, (float)((double)num5 * -(double)num1 + (double)num6 * (double)num2));
        float num7 = mtx.M(2, ma);
        float num8 = mtx.M(2, mb);
        mtx.SetM(2, ma, (float)((double)num7 * (double)num2 + (double)num8 * (double)num1));
        mtx.SetM(2, mb, (float)((double)num7 * -(double)num1 + (double)num8 * (double)num2));
    }

    private static void nnMakeVectorCameraViewMatrix(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR pos,
      AppMain.NNS_VECTOR right,
      AppMain.NNS_VECTOR up,
      AppMain.NNS_VECTOR ilook)
    {
        mtx.M00 = right.x;
        mtx.M01 = right.y;
        mtx.M02 = right.z;
        mtx.M03 = (float)-((double)right.x * (double)pos.x + (double)right.y * (double)pos.y + (double)right.z * (double)pos.z);
        mtx.M10 = up.x;
        mtx.M11 = up.y;
        mtx.M12 = up.z;
        mtx.M13 = (float)-((double)up.x * (double)pos.x + (double)up.y * (double)pos.y + (double)up.z * (double)pos.z);
        mtx.M20 = ilook.x;
        mtx.M21 = ilook.y;
        mtx.M22 = ilook.z;
        mtx.M23 = (float)-((double)ilook.x * (double)pos.x + (double)ilook.y * (double)pos.y + (double)ilook.z * (double)pos.z);
        mtx.M30 = mtx.M31 = mtx.M32 = 0.0f;
        mtx.M33 = 1f;
    }

    private static void nnMakeVectorCameraViewMatrix(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR pos,
      ref AppMain.SNNS_VECTOR right,
      ref AppMain.SNNS_VECTOR up,
      ref AppMain.SNNS_VECTOR ilook)
    {
        mtx.M00 = right.x;
        mtx.M01 = right.y;
        mtx.M02 = right.z;
        mtx.M03 = (float)-((double)right.x * (double)pos.x + (double)right.y * (double)pos.y + (double)right.z * (double)pos.z);
        mtx.M10 = up.x;
        mtx.M11 = up.y;
        mtx.M12 = up.z;
        mtx.M13 = (float)-((double)up.x * (double)pos.x + (double)up.y * (double)pos.y + (double)up.z * (double)pos.z);
        mtx.M20 = ilook.x;
        mtx.M21 = ilook.y;
        mtx.M22 = ilook.z;
        mtx.M23 = (float)-((double)ilook.x * (double)pos.x + (double)ilook.y * (double)pos.y + (double)ilook.z * (double)pos.z);
        mtx.M30 = mtx.M31 = mtx.M32 = 0.0f;
        mtx.M33 = 1f;
    }

    private void nnCalcNodeStatusListMatrixList(
      uint[] nodestatlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX mtxlist,
      AppMain.NNS_MATRIX basemtx,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static float NNM_DEGtoRAD(float n)
    {
        return n * ((float)Math.PI / 180f);
    }

    private static float NNM_DEGtoRAD(int n)
    {
        return (float)n * ((float)Math.PI / 180f);
    }

    private static int NNM_DEGtoA32(float n)
    {
        return (int)((double)n * 182.04443359375);
    }

    private static int NNM_RADtoA32(float n)
    {
        return (int)((double)n * 10430.3779296875);
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
        return (int)((double)n * 182.04443359375);
    }

    private static int NNM_RADtoA32(int n)
    {
        return (int)((double)n * 10430.3779296875);
    }

    private static float NNM_RADtoDEG(int n)
    {
        return (float)n * 57.29578f;
    }

    private static float NNM_A32toDEG(int n)
    {
        return (float)n * 0.005493164f;
    }

    private static float NNM_A32toRAD(int n)
    {
        return (float)n * 9.58738E-05f;
    }

    private static int NNM_DEGtoA16(float n)
    {
        return (int)(short)(int)((double)n * 182.04443359375);
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
    private static void nnInvertTransposeMatrix33(AppMain.NNS_MATRIX dst, AppMain.NNS_MATRIX src)
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
        float num1 = (float)((double)m11 * (double)m22 - (double)m12 * (double)m21);
        float num2 = (float)((double)m12 * (double)m20 - (double)m10 * (double)m22);
        float num3 = (float)((double)m10 * (double)m21 - (double)m11 * (double)m20);
        float num4 = (float)((double)m00 * (double)num1 + (double)m01 * (double)num2 + (double)m02 * (double)num3);
        if ((double)num4 == 0.0)
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
            dst.M10 = (float)-((double)m01 * (double)m22 - (double)m21 * (double)m02) * num5;
            dst.M20 = (float)((double)m01 * (double)m12 - (double)m11 * (double)m02) * num5;
            dst.M01 = num2 * num5;
            dst.M11 = (float)((double)m00 * (double)m22 - (double)m20 * (double)m02) * num5;
            dst.M21 = (float)-((double)m00 * (double)m12 - (double)m10 * (double)m02) * num5;
            dst.M02 = num3 * num5;
            dst.M12 = (float)-((double)m00 * (double)m21 - (double)m20 * (double)m01) * num5;
            dst.M22 = (float)((double)m00 * (double)m11 - (double)m10 * (double)m01) * num5;
        }
    }

    private static void nnInvertTransposeMatrix33NotNormalized(
      AppMain.NNS_MATRIX dst,
      AppMain.NNS_MATRIX src)
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
        dst.M00 = (float)((double)m11 * (double)m22 - (double)m21 * (double)m12);
        dst.M10 = (float)-((double)m01 * (double)m22 - (double)m21 * (double)m02);
        dst.M20 = (float)((double)m01 * (double)m12 - (double)m11 * (double)m02);
        dst.M01 = (float)-((double)m10 * (double)m22 - (double)m20 * (double)m12);
        dst.M11 = (float)((double)m00 * (double)m22 - (double)m20 * (double)m02);
        dst.M21 = (float)-((double)m00 * (double)m12 - (double)m10 * (double)m02);
        dst.M02 = (float)((double)m10 * (double)m21 - (double)m20 * (double)m11);
        dst.M12 = (float)-((double)m00 * (double)m21 - (double)m20 * (double)m01);
        dst.M22 = (float)((double)m00 * (double)m11 - (double)m10 * (double)m01);
    }
    public static void nnCopyMatrix(AppMain.NNS_MATRIX dst, AppMain.NNS_MATRIX src)
    {
        dst.Assign(src);
    }

    public static void nnCopyMatrix(AppMain.NNS_MATRIX dst, ref AppMain.SNNS_MATRIX src)
    {
        dst.Assign(ref src);
    }

    public static void nnCopyMatrix(ref AppMain.SNNS_MATRIX dst, ref AppMain.SNNS_MATRIX src)
    {
        dst.Assign(ref src);
    }

    public static bool nnInvertMatrix(AppMain.NNS_MATRIX dst, AppMain.NNS_MATRIX src)
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
        float num1 = (float)((double)m11 * (double)m22 - (double)m12 * (double)m21);
        float num2 = (float)((double)m12 * (double)m20 - (double)m10 * (double)m22);
        float num3 = (float)((double)m10 * (double)m21 - (double)m11 * (double)m20);
        float num4 = (float)((double)m00 * (double)num1 + (double)m01 * (double)num2 + (double)m02 * (double)num3);
        if ((double)num4 == 0.0)
        {
            dst.Clear();
            return false;
        }
        float num5 = 1f / num4;
        dst.M00 = num1 * num5;
        dst.M01 = (float)-((double)m01 * (double)m22 - (double)m21 * (double)m02) * num5;
        dst.M02 = (float)((double)m01 * (double)m12 - (double)m11 * (double)m02) * num5;
        dst.M10 = num2 * num5;
        dst.M11 = (float)((double)m00 * (double)m22 - (double)m20 * (double)m02) * num5;
        dst.M12 = (float)-((double)m00 * (double)m12 - (double)m10 * (double)m02) * num5;
        dst.M20 = num3 * num5;
        dst.M21 = (float)-((double)m00 * (double)m21 - (double)m20 * (double)m01) * num5;
        dst.M22 = (float)((double)m00 * (double)m11 - (double)m10 * (double)m01) * num5;
        dst.M03 = (float)(-(double)dst.M00 * (double)m03 - (double)dst.M01 * (double)m13 - (double)dst.M02 * (double)m23);
        dst.M13 = (float)(-(double)dst.M10 * (double)m03 - (double)dst.M11 * (double)m13 - (double)dst.M12 * (double)m23);
        dst.M23 = (float)(-(double)dst.M20 * (double)m03 - (double)dst.M21 * (double)m13 - (double)dst.M22 * (double)m23);
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
        return true;
    }

    public static void nnInvertOrthoMatrix(AppMain.NNS_MATRIX dst, AppMain.NNS_MATRIX src)
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
        dst.M03 = (float)-((double)m00 * (double)m03 + (double)m10 * (double)m13 + (double)m20 * (double)m23);
        dst.M13 = (float)-((double)m01 * (double)m03 + (double)m11 * (double)m13 + (double)m21 * (double)m23);
        dst.M23 = (float)-((double)m02 * (double)m03 + (double)m12 * (double)m13 + (double)m22 * (double)m23);
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
      AppMain.NNS_MATRIX dst,
      ref AppMain.SNNS_MATRIX mtx1,
      AppMain.NNS_MATRIX mtx2)
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
        dst.M00 = (float)((double)m00_1 * (double)m00_2 + (double)m01_1 * (double)m10_2 + (double)m02_1 * (double)m20_2);
        dst.M10 = (float)((double)m10_1 * (double)m00_2 + (double)m11_1 * (double)m10_2 + (double)m12_1 * (double)m20_2);
        dst.M20 = (float)((double)m20_1 * (double)m00_2 + (double)m21_1 * (double)m10_2 + (double)m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)((double)m00_1 * (double)m01_2 + (double)m01_1 * (double)m11_2 + (double)m02_1 * (double)m21_2);
        dst.M11 = (float)((double)m10_1 * (double)m01_2 + (double)m11_1 * (double)m11_2 + (double)m12_1 * (double)m21_2);
        dst.M21 = (float)((double)m20_1 * (double)m01_2 + (double)m21_1 * (double)m11_2 + (double)m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)((double)m00_1 * (double)m02_2 + (double)m01_1 * (double)m12_2 + (double)m02_1 * (double)m22_2);
        dst.M12 = (float)((double)m10_1 * (double)m02_2 + (double)m11_1 * (double)m12_2 + (double)m12_1 * (double)m22_2);
        dst.M22 = (float)((double)m20_1 * (double)m02_2 + (double)m21_1 * (double)m12_2 + (double)m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)((double)m00_1 * (double)m03_2 + (double)m01_1 * (double)m13_2 + (double)m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)((double)m10_1 * (double)m03_2 + (double)m11_1 * (double)m13_2 + (double)m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)((double)m20_1 * (double)m03_2 + (double)m21_1 * (double)m13_2 + (double)m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      AppMain.NNS_MATRIX dst,
      AppMain.NNS_MATRIX mtx1,
      ref AppMain.SNNS_MATRIX mtx2)
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
        dst.M00 = (float)((double)m00_1 * (double)m00_2 + (double)m01_1 * (double)m10_2 + (double)m02_1 * (double)m20_2);
        dst.M10 = (float)((double)m10_1 * (double)m00_2 + (double)m11_1 * (double)m10_2 + (double)m12_1 * (double)m20_2);
        dst.M20 = (float)((double)m20_1 * (double)m00_2 + (double)m21_1 * (double)m10_2 + (double)m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)((double)m00_1 * (double)m01_2 + (double)m01_1 * (double)m11_2 + (double)m02_1 * (double)m21_2);
        dst.M11 = (float)((double)m10_1 * (double)m01_2 + (double)m11_1 * (double)m11_2 + (double)m12_1 * (double)m21_2);
        dst.M21 = (float)((double)m20_1 * (double)m01_2 + (double)m21_1 * (double)m11_2 + (double)m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)((double)m00_1 * (double)m02_2 + (double)m01_1 * (double)m12_2 + (double)m02_1 * (double)m22_2);
        dst.M12 = (float)((double)m10_1 * (double)m02_2 + (double)m11_1 * (double)m12_2 + (double)m12_1 * (double)m22_2);
        dst.M22 = (float)((double)m20_1 * (double)m02_2 + (double)m21_1 * (double)m12_2 + (double)m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)((double)m00_1 * (double)m03_2 + (double)m01_1 * (double)m13_2 + (double)m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)((double)m10_1 * (double)m03_2 + (double)m11_1 * (double)m13_2 + (double)m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)((double)m20_1 * (double)m03_2 + (double)m21_1 * (double)m13_2 + (double)m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      AppMain.NNS_MATRIX dst,
      AppMain.NNS_MATRIX mtx1,
      AppMain.NNS_MATRIX mtx2)
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
        dst.M00 = (float)((double)m00_1 * (double)m00_2 + (double)m01_1 * (double)m10_2 + (double)m02_1 * (double)m20_2);
        dst.M10 = (float)((double)m10_1 * (double)m00_2 + (double)m11_1 * (double)m10_2 + (double)m12_1 * (double)m20_2);
        dst.M20 = (float)((double)m20_1 * (double)m00_2 + (double)m21_1 * (double)m10_2 + (double)m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)((double)m00_1 * (double)m01_2 + (double)m01_1 * (double)m11_2 + (double)m02_1 * (double)m21_2);
        dst.M11 = (float)((double)m10_1 * (double)m01_2 + (double)m11_1 * (double)m11_2 + (double)m12_1 * (double)m21_2);
        dst.M21 = (float)((double)m20_1 * (double)m01_2 + (double)m21_1 * (double)m11_2 + (double)m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)((double)m00_1 * (double)m02_2 + (double)m01_1 * (double)m12_2 + (double)m02_1 * (double)m22_2);
        dst.M12 = (float)((double)m10_1 * (double)m02_2 + (double)m11_1 * (double)m12_2 + (double)m12_1 * (double)m22_2);
        dst.M22 = (float)((double)m20_1 * (double)m02_2 + (double)m21_1 * (double)m12_2 + (double)m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)((double)m00_1 * (double)m03_2 + (double)m01_1 * (double)m13_2 + (double)m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)((double)m10_1 * (double)m03_2 + (double)m11_1 * (double)m13_2 + (double)m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)((double)m20_1 * (double)m03_2 + (double)m21_1 * (double)m13_2 + (double)m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      ref AppMain.SNNS_MATRIX dst,
      AppMain.NNS_MATRIX mtx1,
      ref AppMain.SNNS_MATRIX mtx2)
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
        dst.M00 = (float)((double)m00_1 * (double)m00_2 + (double)m01_1 * (double)m10_2 + (double)m02_1 * (double)m20_2);
        dst.M10 = (float)((double)m10_1 * (double)m00_2 + (double)m11_1 * (double)m10_2 + (double)m12_1 * (double)m20_2);
        dst.M20 = (float)((double)m20_1 * (double)m00_2 + (double)m21_1 * (double)m10_2 + (double)m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)((double)m00_1 * (double)m01_2 + (double)m01_1 * (double)m11_2 + (double)m02_1 * (double)m21_2);
        dst.M11 = (float)((double)m10_1 * (double)m01_2 + (double)m11_1 * (double)m11_2 + (double)m12_1 * (double)m21_2);
        dst.M21 = (float)((double)m20_1 * (double)m01_2 + (double)m21_1 * (double)m11_2 + (double)m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)((double)m00_1 * (double)m02_2 + (double)m01_1 * (double)m12_2 + (double)m02_1 * (double)m22_2);
        dst.M12 = (float)((double)m10_1 * (double)m02_2 + (double)m11_1 * (double)m12_2 + (double)m12_1 * (double)m22_2);
        dst.M22 = (float)((double)m20_1 * (double)m02_2 + (double)m21_1 * (double)m12_2 + (double)m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)((double)m00_1 * (double)m03_2 + (double)m01_1 * (double)m13_2 + (double)m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)((double)m10_1 * (double)m03_2 + (double)m11_1 * (double)m13_2 + (double)m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)((double)m20_1 * (double)m03_2 + (double)m21_1 * (double)m13_2 + (double)m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      out AppMain.SNNS_MATRIX dst,
      ref AppMain.SNNS_MATRIX mtx1,
      ref AppMain.SNNS_MATRIX mtx2)
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
        dst.M00 = (float)((double)m00_1 * (double)m00_2 + (double)m01_1 * (double)m10_2 + (double)m02_1 * (double)m20_2);
        dst.M10 = (float)((double)m10_1 * (double)m00_2 + (double)m11_1 * (double)m10_2 + (double)m12_1 * (double)m20_2);
        dst.M20 = (float)((double)m20_1 * (double)m00_2 + (double)m21_1 * (double)m10_2 + (double)m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)((double)m00_1 * (double)m01_2 + (double)m01_1 * (double)m11_2 + (double)m02_1 * (double)m21_2);
        dst.M11 = (float)((double)m10_1 * (double)m01_2 + (double)m11_1 * (double)m11_2 + (double)m12_1 * (double)m21_2);
        dst.M21 = (float)((double)m20_1 * (double)m01_2 + (double)m21_1 * (double)m11_2 + (double)m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)((double)m00_1 * (double)m02_2 + (double)m01_1 * (double)m12_2 + (double)m02_1 * (double)m22_2);
        dst.M12 = (float)((double)m10_1 * (double)m02_2 + (double)m11_1 * (double)m12_2 + (double)m12_1 * (double)m22_2);
        dst.M22 = (float)((double)m20_1 * (double)m02_2 + (double)m21_1 * (double)m12_2 + (double)m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)((double)m00_1 * (double)m03_2 + (double)m01_1 * (double)m13_2 + (double)m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)((double)m10_1 * (double)m03_2 + (double)m11_1 * (double)m13_2 + (double)m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)((double)m20_1 * (double)m03_2 + (double)m21_1 * (double)m13_2 + (double)m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMultiplyMatrix(
      ref AppMain.SNNS_MATRIX dst,
      AppMain.NNS_MATRIX mtx1,
      AppMain.NNS_MATRIX mtx2)
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
        dst.M00 = (float)((double)m00_1 * (double)m00_2 + (double)m01_1 * (double)m10_2 + (double)m02_1 * (double)m20_2);
        dst.M10 = (float)((double)m10_1 * (double)m00_2 + (double)m11_1 * (double)m10_2 + (double)m12_1 * (double)m20_2);
        dst.M20 = (float)((double)m20_1 * (double)m00_2 + (double)m21_1 * (double)m10_2 + (double)m22_1 * (double)m20_2);
        float m01_2 = mtx2.M01;
        float m11_2 = mtx2.M11;
        float m21_2 = mtx2.M21;
        dst.M01 = (float)((double)m00_1 * (double)m01_2 + (double)m01_1 * (double)m11_2 + (double)m02_1 * (double)m21_2);
        dst.M11 = (float)((double)m10_1 * (double)m01_2 + (double)m11_1 * (double)m11_2 + (double)m12_1 * (double)m21_2);
        dst.M21 = (float)((double)m20_1 * (double)m01_2 + (double)m21_1 * (double)m11_2 + (double)m22_1 * (double)m21_2);
        float m02_2 = mtx2.M02;
        float m12_2 = mtx2.M12;
        float m22_2 = mtx2.M22;
        dst.M02 = (float)((double)m00_1 * (double)m02_2 + (double)m01_1 * (double)m12_2 + (double)m02_1 * (double)m22_2);
        dst.M12 = (float)((double)m10_1 * (double)m02_2 + (double)m11_1 * (double)m12_2 + (double)m12_1 * (double)m22_2);
        dst.M22 = (float)((double)m20_1 * (double)m02_2 + (double)m21_1 * (double)m12_2 + (double)m22_1 * (double)m22_2);
        float m03_2 = mtx2.M03;
        float m13_2 = mtx2.M13;
        float m23_2 = mtx2.M23;
        dst.M03 = (float)((double)m00_1 * (double)m03_2 + (double)m01_1 * (double)m13_2 + (double)m02_1 * (double)m23_2) + m03_1;
        dst.M13 = (float)((double)m10_1 * (double)m03_2 + (double)m11_1 * (double)m13_2 + (double)m12_1 * (double)m23_2) + m13_1;
        dst.M23 = (float)((double)m20_1 * (double)m03_2 + (double)m21_1 * (double)m13_2 + (double)m22_1 * (double)m23_2) + m23_1;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnTransposeMatrix(AppMain.NNS_MATRIX dst, AppMain.NNS_MATRIX src)
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
      AppMain.NNS_MATRIX dst,
      AppMain.NNS_MATRIX src,
      ref AppMain.NNS_QUATERNION quat)
    {
        AppMain.SNNS_MATRIX dst1;
        AppMain.nnMakeQuaternionMatrix(out dst1, ref quat);
        AppMain.nnMultiplyMatrix(dst, src, ref dst1);
    }

    public static void nnRotateXMatrix(AppMain.NNS_MATRIX dst, AppMain.NNS_MATRIX src, int ax)
    {
        if (ax == 0)
        {
            if (dst == src)
                return;
            AppMain.nnCopyMatrix(dst, src);
        }
        else
        {
            float s;
            float c;
            AppMain.nnSinCos(ax, out s, out c);
            float num1 = s;
            float num2 = c;
            float m01 = src.M01;
            float m02 = src.M02;
            dst.M01 = (float)((double)m01 * (double)num2 + (double)m02 * (double)num1);
            dst.M02 = (float)((double)m01 * -(double)num1 + (double)m02 * (double)num2);
            float m11 = src.M11;
            float m12 = src.M12;
            dst.M11 = (float)((double)m11 * (double)num2 + (double)m12 * (double)num1);
            dst.M12 = (float)((double)m11 * -(double)num1 + (double)m12 * (double)num2);
            float m21 = src.M21;
            float m22 = src.M22;
            dst.M21 = (float)((double)m21 * (double)num2 + (double)m22 * (double)num1);
            dst.M22 = (float)((double)m21 * -(double)num1 + (double)m22 * (double)num2);
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

    public static void nnRotateYMatrix(AppMain.NNS_MATRIX dst, AppMain.NNS_MATRIX src, int ay)
    {
        if (ay == 0)
        {
            if (dst == src)
                return;
            AppMain.nnCopyMatrix(dst, src);
        }
        else
        {
            float s;
            float c;
            AppMain.nnSinCos(ay, out s, out c);
            float num1 = s;
            float num2 = c;
            float m00 = src.M00;
            float m02 = src.M02;
            dst.M00 = (float)((double)m00 * (double)num2 + (double)m02 * -(double)num1);
            dst.M02 = (float)((double)m00 * (double)num1 + (double)m02 * (double)num2);
            float m10 = src.M10;
            float m12 = src.M12;
            dst.M10 = (float)((double)m10 * (double)num2 + (double)m12 * -(double)num1);
            dst.M12 = (float)((double)m10 * (double)num1 + (double)m12 * (double)num2);
            float m20 = src.M20;
            float m22 = src.M22;
            dst.M20 = (float)((double)m20 * (double)num2 + (double)m22 * -(double)num1);
            dst.M22 = (float)((double)m20 * (double)num1 + (double)m22 * (double)num2);
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
      ref AppMain.SNNS_MATRIX dst,
      ref AppMain.SNNS_MATRIX src,
      int ay)
    {
        if (ay == 0)
            return;
        float s;
        float c;
        AppMain.nnSinCos(ay, out s, out c);
        float num1 = s;
        float num2 = c;
        float m00 = src.M00;
        float m02 = src.M02;
        dst.M00 = (float)((double)m00 * (double)num2 + (double)m02 * -(double)num1);
        dst.M02 = (float)((double)m00 * (double)num1 + (double)m02 * (double)num2);
        float m10 = src.M10;
        float m12 = src.M12;
        dst.M10 = (float)((double)m10 * (double)num2 + (double)m12 * -(double)num1);
        dst.M12 = (float)((double)m10 * (double)num1 + (double)m12 * (double)num2);
        float m20 = src.M20;
        float m22 = src.M22;
        dst.M20 = (float)((double)m20 * (double)num2 + (double)m22 * -(double)num1);
        dst.M22 = (float)((double)m20 * (double)num1 + (double)m22 * (double)num2);
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
      ref AppMain.SNNS_MATRIX dst,
      ref AppMain.SNNS_MATRIX src,
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
            AppMain.nnSinCos(az, out s, out c);
            float num1 = s;
            float num2 = c;
            float m00 = src.M00;
            float m01 = src.M01;
            dst.M00 = (float)((double)m00 * (double)num2 + (double)m01 * (double)num1);
            dst.M01 = (float)((double)m00 * -(double)num1 + (double)m01 * (double)num2);
            float m10 = src.M10;
            float m11 = src.M11;
            dst.M10 = (float)((double)m10 * (double)num2 + (double)m11 * (double)num1);
            dst.M11 = (float)((double)m10 * -(double)num1 + (double)m11 * (double)num2);
            float m20 = src.M20;
            float m21 = src.M21;
            dst.M20 = (float)((double)m20 * (double)num2 + (double)m21 * (double)num1);
            dst.M21 = (float)((double)m20 * -(double)num1 + (double)m21 * (double)num2);
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

    public static void nnRotateZMatrix(AppMain.NNS_MATRIX dst, AppMain.NNS_MATRIX src, int az)
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
            AppMain.nnSinCos(az, out s, out c);
            float num1 = s;
            float num2 = c;
            float m00 = src.M00;
            float m01 = src.M01;
            dst.M00 = (float)((double)m00 * (double)num2 + (double)m01 * (double)num1);
            dst.M01 = (float)((double)m00 * -(double)num1 + (double)m01 * (double)num2);
            float m10 = src.M10;
            float m11 = src.M11;
            dst.M10 = (float)((double)m10 * (double)num2 + (double)m11 * (double)num1);
            dst.M11 = (float)((double)m10 * -(double)num1 + (double)m11 * (double)num2);
            float m20 = src.M20;
            float m21 = src.M21;
            dst.M20 = (float)((double)m20 * (double)num2 + (double)m21 * (double)num1);
            dst.M21 = (float)((double)m20 * -(double)num1 + (double)m21 * (double)num2);
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
      AppMain.NNS_MATRIX dst,
      AppMain.NNS_MATRIX src,
      int ax,
      int ay,
      int az)
    {
        if (az != 0 || dst != src)
            AppMain.nnRotateZMatrix(dst, src, az);
        if (ay != 0)
            AppMain.nnRotateYMatrix(dst, dst, ay);
        if (ax == 0)
            return;
        AppMain.nnRotateXMatrix(dst, dst, ax);
    }

    public static void nnRotateXZYMatrix(
      AppMain.NNS_MATRIX dst,
      AppMain.NNS_MATRIX src,
      int ax,
      int ay,
      int az)
    {
        if (ay != 0 || dst != src)
            AppMain.nnRotateYMatrix(dst, src, ay);
        if (az != 0)
            AppMain.nnRotateZMatrix(dst, dst, az);
        if (ax == 0)
            return;
        AppMain.nnRotateXMatrix(dst, dst, ax);
    }

    public static void nnRotateZXYMatrix(
      AppMain.NNS_MATRIX dst,
      AppMain.NNS_MATRIX src,
      int ax,
      int ay,
      int az)
    {
        if (ay != 0 || dst != src)
            AppMain.nnRotateYMatrix(dst, src, ay);
        if (ax != 0)
            AppMain.nnRotateXMatrix(dst, dst, ax);
        if (az == 0)
            return;
        AppMain.nnRotateZMatrix(dst, dst, az);
    }

    public static void nnScaleMatrix(
      ref AppMain.SNNS_MATRIX dst,
      ref AppMain.SNNS_MATRIX src,
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
      AppMain.NNS_MATRIX dst,
      AppMain.NNS_MATRIX src,
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
      ref AppMain.SNNS_MATRIX dst,
      ref AppMain.SNNS_MATRIX src,
      float x,
      float y,
      float z)
    {
        AppMain.nnCopyMatrix33(ref dst, ref src);
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M03 = (float)((double)src.M00 * (double)x + (double)src.M01 * (double)y + (double)src.M02 * (double)z) + src.M03;
        dst.M13 = (float)((double)src.M10 * (double)x + (double)src.M11 * (double)y + (double)src.M12 * (double)z) + src.M13;
        dst.M23 = (float)((double)src.M20 * (double)x + (double)src.M21 * (double)y + (double)src.M22 * (double)z) + src.M23;
        dst.M33 = 1f;
    }

    public static void nnTranslateMatrix(
      AppMain.NNS_MATRIX dst,
      AppMain.NNS_MATRIX src,
      float x,
      float y,
      float z)
    {
        if (dst != src)
        {
            AppMain.nnCopyMatrix33(dst, src);
            dst.M30 = 0.0f;
            dst.M31 = 0.0f;
            dst.M32 = 0.0f;
        }
        dst.M03 = (float)((double)src.M00 * (double)x + (double)src.M01 * (double)y + (double)src.M02 * (double)z) + src.M03;
        dst.M13 = (float)((double)src.M10 * (double)x + (double)src.M11 * (double)y + (double)src.M12 * (double)z) + src.M13;
        dst.M23 = (float)((double)src.M20 * (double)x + (double)src.M21 * (double)y + (double)src.M22 * (double)z) + src.M23;
        dst.M33 = 1f;
    }

    public static void nnCopyVectorMatrixTranslation(
      AppMain.NNS_MATRIX mtx,
      ref AppMain.SNNS_VECTOR vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(
      ref AppMain.SNNS_MATRIX mtx,
      ref AppMain.SNNS_VECTOR vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(
      ref AppMain.SNNS_MATRIX mtx,
      AppMain.NNS_VECTOR vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(
      ref AppMain.SNNS_MATRIX mtx,
      AppMain.NNS_VECTOR4D vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(
      ref AppMain.SNNS_MATRIX mtx,
      ref AppMain.SNNS_VECTOR4D vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(AppMain.NNS_MATRIX mtx, AppMain.NNS_VECTOR vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    public static void nnCopyVectorMatrixTranslation(AppMain.NNS_MATRIX mtx, AppMain.NNS_VECTOR4D vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    private static void nnCopyVectorFastMatrixTranslation(
      AppMain.NNS_MATRIX mtx,
      ref AppMain.NNS_VECTORFAST vec)
    {
        mtx.M03 = vec.x;
        mtx.M13 = vec.y;
        mtx.M23 = vec.z;
    }

    private int nnCheckCollisionSC(ref AppMain.NNS_SPHERE sphere, ref AppMain.NNS_CAPSULE capsule)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private int nnCheckCollisionBC(ref AppMain.NNS_BOX box, ref AppMain.NNS_CAPSULE capsule)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }
    private int nnCalcMorphMotionWeight(
      ref AppMain.NNS_SUBMOTION submot,
      float frame,
      ref float weight)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private void nnCalcMorphMotion(
      float[] mwpal,
      ref AppMain.NNS_MORPHTARGETLIST mtgt,
      ref AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnBlendMorphWeightPalette(
      float[] dstmwpal,
      float[] srcmwpal0,
      float ratio0,
      float[] srcmwpal1,
      float ratio1,
      ref AppMain.NNS_MORPHTARGETLIST mtgt)
    {
        AppMain.mppAssertNotImpl();
    }

    public static void nnCopyQuaternion(
      ref AppMain.NNS_QUATERNION dst,
      ref AppMain.NNS_QUATERNION src)
    {
        dst = src;
    }

    public static void nnMultiplyQuaternion(
      ref AppMain.NNS_QUATERNION dst,
      ref AppMain.NNS_QUATERNION quat1,
      ref AppMain.NNS_QUATERNION quat2)
    {
        float x1 = quat1.x;
        float y1 = quat1.y;
        float z1 = quat1.z;
        float w1 = quat1.w;
        float x2 = quat2.x;
        float y2 = quat2.y;
        float z2 = quat2.z;
        float w2 = quat2.w;
        dst.x = (float)((double)w1 * (double)x2 + (double)x1 * (double)w2 + (double)y1 * (double)z2 - (double)z1 * (double)y2);
        dst.y = (float)((double)w1 * (double)y2 - (double)x1 * (double)z2 + (double)y1 * (double)w2 + (double)z1 * (double)x2);
        dst.z = (float)((double)w1 * (double)z2 + (double)x1 * (double)y2 - (double)y1 * (double)x2 + (double)z1 * (double)w2);
        dst.w = (float)((double)w1 * (double)w2 - (double)x1 * (double)x2 - (double)y1 * (double)y2 - (double)z1 * (double)z2);
    }

    public static int nnNormalizeQuaternion(
      ref AppMain.NNS_QUATERNION dst,
      ref AppMain.NNS_QUATERNION src)
    {
        float n = (float)((double)src.x * (double)src.x + (double)src.y * (double)src.y + (double)src.z * (double)src.z + (double)src.w * (double)src.w);
        if ((double)n == 0.0)
        {
            dst.x = 0.0f;
            dst.y = 0.0f;
            dst.z = 0.0f;
            dst.w = 0.0f;
            return 0;
        }
        float num = AppMain.nnInvertSqrt(n);
        dst.x = src.x * num;
        dst.y = src.y * num;
        dst.z = src.z * num;
        dst.w = src.w * num;
        return 1;
    }

    public static int nnInvertQuaternion(
      ref AppMain.NNS_QUATERNION dst,
      ref AppMain.NNS_QUATERNION src)
    {
        float num1 = (float)((double)src.x * (double)src.x + (double)src.y * (double)src.y + (double)src.z * (double)src.z + (double)src.w * (double)src.w);
        if ((double)num1 == 0.0)
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
      ref AppMain.NNS_QUATERNION dst,
      ref AppMain.NNS_QUATERNION src)
    {
        int num1 = AppMain.nnArcCos((double)src.w);
        float num2 = AppMain.nnSin(num1);
        if ((double)num2 > 0.0)
        {
            float num3 = 1f / num2;
            dst.x = AppMain.NNM_A32toRAD(num1) * src.x * num3;
            dst.y = AppMain.NNM_A32toRAD(num1) * src.y * num3;
            dst.z = AppMain.NNM_A32toRAD(num1) * src.z * num3;
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

    public static void nnExpQuaternion(ref AppMain.NNS_QUATERNION dst, ref AppMain.NNS_QUATERNION src)
    {
        float n = AppMain.nnSqrt((float)((double)src.x * (double)src.x + (double)src.y * (double)src.y + (double)src.z * (double)src.z));
        float s;
        float c;
        AppMain.nnSinCos(AppMain.NNM_RADtoA32(n), out s, out c);
        if ((double)n > 0.0)
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
      ref AppMain.NNS_QUATERNION dst,
      ref AppMain.NNS_QUATERNION quatprev,
      ref AppMain.NNS_QUATERNION quat,
      ref AppMain.NNS_QUATERNION quatnext)
    {
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion3 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion4 = new AppMain.NNS_QUATERNION();
        AppMain.nnInvertQuaternion(ref nnsQuaternion2, ref quat);
        AppMain.nnMultiplyQuaternion(ref nnsQuaternion3, ref nnsQuaternion2, ref quatprev);
        AppMain.nnMultiplyQuaternion(ref nnsQuaternion4, ref nnsQuaternion2, ref quatnext);
        AppMain.nnLogQuaternion(ref nnsQuaternion3, ref nnsQuaternion3);
        AppMain.nnLogQuaternion(ref nnsQuaternion4, ref nnsQuaternion4);
        nnsQuaternion1.x = (float)(((double)nnsQuaternion3.x + (double)nnsQuaternion4.x) / -4.0);
        nnsQuaternion1.y = (float)(((double)nnsQuaternion3.y + (double)nnsQuaternion4.y) / -4.0);
        nnsQuaternion1.z = (float)(((double)nnsQuaternion3.z + (double)nnsQuaternion4.z) / -4.0);
        nnsQuaternion1.w = (float)(((double)nnsQuaternion3.w + (double)nnsQuaternion4.w) / -4.0);
        AppMain.nnExpQuaternion(ref nnsQuaternion1, ref nnsQuaternion1);
        AppMain.nnMultiplyQuaternion(ref dst, ref quat, ref nnsQuaternion1);
    }

    private static void nnLerpQuaternion(
      ref AppMain.NNS_QUATERNION dst,
      ref AppMain.NNS_QUATERNION quat1,
      ref AppMain.NNS_QUATERNION quat2,
      float t)
    {
        AppMain.NNS_QUATERNION src = new AppMain.NNS_QUATERNION();
        float num = 1f - t;
        if ((double)quat1.x * (double)quat2.x + (double)quat1.y * (double)quat2.y + (double)quat1.z * (double)quat2.z + (double)quat1.w * (double)quat2.w > 0.0)
        {
            src.x = (float)((double)quat1.x * (double)num + (double)quat2.x * (double)t);
            src.y = (float)((double)quat1.y * (double)num + (double)quat2.y * (double)t);
            src.z = (float)((double)quat1.z * (double)num + (double)quat2.z * (double)t);
            src.w = (float)((double)quat1.w * (double)num + (double)quat2.w * (double)t);
        }
        else
        {
            src.x = (float)((double)quat1.x * (double)num - (double)quat2.x * (double)t);
            src.y = (float)((double)quat1.y * (double)num - (double)quat2.y * (double)t);
            src.z = (float)((double)quat1.z * (double)num - (double)quat2.z * (double)t);
            src.w = (float)((double)quat1.w * (double)num - (double)quat2.w * (double)t);
        }
        AppMain.nnNormalizeQuaternion(ref dst, ref src);
    }

    private static void nnSlerpQuaternion(
      out AppMain.NNS_QUATERNION dst,
      ref AppMain.NNS_QUATERNION quat1,
      ref AppMain.NNS_QUATERNION quat2,
      float t)
    {
        float num1 = 1f;
        float num2 = 1f - t;
        float num3 = (float)((double)quat1.x * (double)quat2.x + (double)quat1.y * (double)quat2.y + (double)quat1.z * (double)quat2.z + (double)quat1.w * (double)quat2.w);
        if ((double)num3 < 0.0)
        {
            num3 = -num3;
            num1 = -num1;
        }
        float num4;
        float num5;
        if ((double)num3 < 0.999989986419678)
        {
            float num6 = (float)(2.0 - 2.82842707633972 * (double)AppMain.nnInvertSqrt(1f + num3)) * t;
            float num7 = num2 * (1f - num6);
            float num8 = -num6 * num2 + t;
            float num9 = AppMain.nnInvertSqrt((float)((double)num7 * (double)num7 + (double)num8 * (double)num8 + 2.0 * (double)num7 * (double)num8 * (double)num3));
            num4 = num7 * num9;
            num5 = num8 * (num9 * num1);
        }
        else
        {
            num4 = num2;
            num5 = t * num1;
        }
        dst.x = (float)((double)quat1.x * (double)num4 + (double)quat2.x * (double)num5);
        dst.y = (float)((double)quat1.y * (double)num4 + (double)quat2.y * (double)num5);
        dst.z = (float)((double)quat1.z * (double)num4 + (double)quat2.z * (double)num5);
        dst.w = (float)((double)quat1.w * (double)num4 + (double)quat2.w * (double)num5);
    }

    private void nnSlerpNoInvQuaternion(
      ref AppMain.NNS_QUATERNION dst,
      ref AppMain.NNS_QUATERNION q1,
      ref AppMain.NNS_QUATERNION q2,
      float t)
    {
        float num1 = (float)((double)q1.x * (double)q2.x + (double)q1.y * (double)q2.y + (double)q1.z * (double)q2.z + (double)q1.w * (double)q2.w);
        float num2;
        float num3;
        if ((double)num1 >= -0.980000019073486 && (double)num1 <= 0.980000019073486)
        {
            int ang = AppMain.nnArcCos((double)num1);
            float num4 = 1f / AppMain.nnSin(ang);
            num2 = AppMain.nnSin((int)((double)ang * (1.0 - (double)t))) * num4;
            num3 = AppMain.nnSin((int)((double)ang * (double)t)) * num4;
        }
        else
        {
            num2 = 1f - t;
            num3 = t;
        }
        dst.x = (float)((double)q1.x * (double)num2 + (double)q2.x * (double)num3);
        dst.y = (float)((double)q1.y * (double)num2 + (double)q2.y * (double)num3);
        dst.z = (float)((double)q1.z * (double)num2 + (double)q2.z * (double)num3);
        dst.w = (float)((double)q1.w * (double)num2 + (double)q2.w * (double)num3);
    }

    private static void nnSquadQuaternion(
      ref AppMain.NNS_QUATERNION dst,
      ref AppMain.NNS_QUATERNION quat1,
      ref AppMain.NNS_QUATERNION quata,
      ref AppMain.NNS_QUATERNION quatb,
      ref AppMain.NNS_QUATERNION quat2,
      float t)
    {
        AppMain.NNS_QUATERNION dst1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION dst2 = new AppMain.NNS_QUATERNION();
        float t1 = (float)(2.0 * (double)t * (1.0 - (double)t));
        AppMain.nnSlerpQuaternion(out dst1, ref quat1, ref quat2, t);
        AppMain.nnSlerpQuaternion(out dst2, ref quata, ref quatb, t);
        AppMain.nnSlerpQuaternion(out dst, ref dst1, ref dst2, t1);
    }

    public static void nnMakeUnitQuaternion(ref AppMain.NNS_QUATERNION dst)
    {
        dst.x = 0.0f;
        dst.y = 0.0f;
        dst.z = 0.0f;
        dst.w = 1f;
    }

    private static void nnMakeRotateAxisQuaternion(
      out AppMain.NNS_QUATERNION dst,
      float vx,
      float vy,
      float vz,
      int ang)
    {
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.x = vx;
        nnsVector.y = vy;
        nnsVector.z = vz;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        ang >>= 1;
        float s;
        float c;
        AppMain.nnSinCos(ang, out s, out c);
        dst.x = nnsVector.x * s;
        dst.y = nnsVector.y * s;
        dst.z = nnsVector.z * s;
        dst.w = c;
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    public static void nnMakeRotateMatrixQuaternion(
      out AppMain.NNS_QUATERNION dst,
      AppMain.NNS_MATRIX mtx)
    {
        int[] nxt = AppMain._nnMakeRotateMatrixQuaternion.nxt;
        nxt[0] = 1;
        nxt[1] = 2;
        nxt[2] = 0;
        float[] q = AppMain._nnMakeRotateMatrixQuaternion.q;
        float num1 = mtx.M00 + mtx.M11 + mtx.M22;
        if ((double)num1 > 0.0)
        {
            float num2 = AppMain.nnSqrt(num1 + 1f);
            dst.w = num2 * 0.5f;
            float num3 = 0.5f / num2;
            dst.x = (mtx.M21 - mtx.M12) * num3;
            dst.y = (mtx.M02 - mtx.M20) * num3;
            dst.z = (mtx.M10 - mtx.M01) * num3;
        }
        else
        {
            int index1 = 0;
            if ((double)mtx.M11 > (double)mtx.M00)
                index1 = 1;
            if ((double)mtx.M22 > (double)mtx.M(index1, index1))
                index1 = 2;
            int index2 = nxt[index1];
            int index3 = nxt[index2];
            float num2 = AppMain.nnSqrt((float)((double)mtx.M(index1, index1) - ((double)mtx.M(index2, index2) + (double)mtx.M(index3, index3)) + 1.0));
            q[index1] = num2 * 0.5f;
            if ((double)num2 != 0.0)
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
      out AppMain.NNS_QUATERNION dst,
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
            AppMain.nnSinCos(rx >> 1, out s, out c);
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
            AppMain.nnSinCos(ry >> 1, out s, out c);
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
            AppMain.nnSinCos(rz >> 1, out s, out c);
            num5 = s;
            num6 = c;
        }
        float num7 = num6 * num4;
        float num8 = num6 * num3;
        float num9 = num5 * num3;
        float num10 = num5 * num4;
        dst.x = (float)((double)num7 * (double)num1 - (double)num9 * (double)num2);
        dst.y = (float)((double)num8 * (double)num2 + (double)num10 * (double)num1);
        dst.z = (float)(-(double)num8 * (double)num1 + (double)num10 * (double)num2);
        dst.w = (float)((double)num7 * (double)num2 + (double)num9 * (double)num1);
    }

    public static void nnMakeRotateXZYQuaternion(
      out AppMain.NNS_QUATERNION dst,
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
            AppMain.nnSinCos(rx >> 1, out s, out c);
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
            AppMain.nnSinCos(ry >> 1, out s, out c);
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
            AppMain.nnSinCos(rz >> 1, out s, out c);
            num5 = s;
            num6 = c;
        }
        float num7 = num4 * num6;
        float num8 = num3 * num6;
        float num9 = num3 * num5;
        float num10 = num4 * num5;
        dst.x = (float)((double)num7 * (double)num1 + (double)num9 * (double)num2);
        dst.y = (float)((double)num8 * (double)num2 + (double)num10 * (double)num1);
        dst.z = (float)(-(double)num8 * (double)num1 + (double)num10 * (double)num2);
        dst.w = (float)((double)num7 * (double)num2 - (double)num9 * (double)num1);
    }

    public static void nnMakeRotateZXYQuaternion(
      out AppMain.NNS_QUATERNION dst,
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
            AppMain.nnSinCos(rx >> 1, out s, out c);
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
            AppMain.nnSinCos(ry >> 1, out s, out c);
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
            AppMain.nnSinCos(rz >> 1, out s, out c);
            num5 = s;
            num6 = c;
        }
        dst.x = (float)((double)num4 * (double)num1 * (double)num6 + (double)num3 * (double)num2 * (double)num5);
        dst.y = (float)(-(double)num4 * (double)num1 * (double)num5 + (double)num3 * (double)num2 * (double)num6);
        dst.z = (float)((double)num4 * (double)num2 * (double)num5 - (double)num3 * (double)num1 * (double)num6);
        dst.w = (float)((double)num4 * (double)num2 * (double)num6 + (double)num3 * (double)num1 * (double)num5);
    }

    private void nnCalcMatrixList(
      AppMain.NNS_MATRIX mtxlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotionNode(
      AppMain.NNS_MATRIX mtxlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotion(
      AppMain.NNS_MATRIX mtxlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mot,
      float frame,
      AppMain.NNS_MATRIX basemtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixListTRSList(
      AppMain.NNS_MATRIX mtxlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_TRS trslist,
      AppMain.NNS_MATRIX basemtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixListMultiplyMatrix(
      AppMain.ArrayPointer<AppMain.NNS_MATRIX> dstlist,
      AppMain.NNS_MATRIX src,
      AppMain.ArrayPointer<AppMain.NNS_MATRIX> srclist,
      int num)
    {
        this.nnCalcMultiplyMatrices(dstlist, src, srclist, num);
    }

    private void nnCalcMatrixPaletteMatrixList(
      AppMain.NNS_MATRIX mtxpal,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX mtxlist,
      AppMain.NNS_MATRIX basemtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotionNode1BoneSIIK(
      AppMain.NNS_MATRIX mtxlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      int jnt1idx,
      int submotidx,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotionNode2BoneSIIK(
      AppMain.NNS_MATRIX mtxlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      int jnt1idx,
      int submotidx,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotionNode1BoneXSIIK(
      AppMain.NNS_MATRIX mtxlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      int rootidx,
      int submotidx,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMatrixListMotionNode2BoneXSIIK(
      AppMain.NNS_MATRIX mtxlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      int rootidx,
      int submotidx,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.mppAssertNotImpl();
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
            double num3 = (double)src[index1] / (double)src[index1 + 3];
            numArray1[index3] = (float)num3;
            float[] numArray2 = dst;
            int index4 = num2;
            int num4 = index4 + 1;
            double num5 = (double)src[index1 + 1] / (double)src[index1 + 3];
            numArray2[index4] = (float)num5;
            float[] numArray3 = dst;
            int index5 = num4;
            num1 = index5 + 1;
            double num6 = (double)src[index1 + 2] / (double)src[index1 + 3];
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
            double num5 = ((double)numArray2[index3] + 0.5) * 0.00784313771873713;
            numArray1[index2] = (float)num5;
            float[] numArray3 = dst;
            int index4 = num3;
            int num6 = index4 + 1;
            sbyte[] numArray4 = src;
            int index5 = num4;
            int num7 = index5 + 1;
            double num8 = ((double)numArray4[index5] + 0.5) * 0.00784313771873713;
            numArray3[index4] = (float)num8;
            float[] numArray5 = dst;
            int index6 = num6;
            num1 = index6 + 1;
            sbyte[] numArray6 = src;
            int index7 = num7;
            num2 = index7 + 1;
            double num9 = ((double)numArray6[index7] + 0.5) * 0.00784313771873713;
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
            double num5 = ((double)numArray2[index3] + 0.5) * 3.05180437862873E-05;
            numArray1[index2] = (float)num5;
            float[] numArray3 = dst;
            int index4 = num3;
            int num6 = index4 + 1;
            short[] numArray4 = src;
            int index5 = num4;
            int num7 = index5 + 1;
            double num8 = ((double)numArray4[index5] + 0.5) * 3.05180437862873E-05;
            numArray3[index4] = (float)num8;
            float[] numArray5 = dst;
            int index6 = num6;
            num1 = index6 + 1;
            short[] numArray6 = src;
            int index7 = num7;
            num2 = index7 + 1;
            double num9 = ((double)numArray6[index7] + 0.5) * 3.05180437862873E-05;
            numArray5[index6] = (float)num9;
        }
    }

    private static void nnCalcMorphPositon(
      float[] pPosBuf,
      AppMain.NNS_VTXARRAY_GL[] pMorphArray,
      AppMain.NNS_VTXARRAY_GL[] pObjArray,
      AppMain.NNS_VTXARRAY_GL[] pMtgtArray,
      int nVertex,
      float weight)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnCalcMorphNormal(
      float[] pNrmBuf,
      AppMain.NNS_VTXARRAY_GL[] pMorphArray,
      AppMain.NNS_VTXARRAY_GL[] pObjArray,
      AppMain.NNS_VTXARRAY_GL[] pMtgtArray,
      int nVertex,
      float weight)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnCalcMorphGeneral(
      float[] pBuf,
      AppMain.NNS_VTXARRAY_GL[] pMorphArray,
      AppMain.NNS_VTXARRAY_GL[] pObjArray,
      AppMain.NNS_VTXARRAY_GL[] pMtgtArray,
      int nVertex,
      float weight)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnNormalizeNormalArray(
      float[] pNrmBuf,
      AppMain.NNS_VTXARRAY_GL[] pNrmArray,
      int nVertex)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMorphObject(
      ref AppMain.NNS_OBJECT mobj,
      ref AppMain.NNS_OBJECT obj,
      ref AppMain.NNS_MORPHTARGETLIST mtgt,
      float[] mwpal,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMotionLightScalar(AppMain.NNS_SUBMOTION submot, float frame, ref float val)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMotionLightAngle(AppMain.NNS_SUBMOTION submot, float frame, ref int ang)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMotionLightXYZ(
      AppMain.NNS_SUBMOTION submot,
      float frame,
      AppMain.NNS_VECTOR xyz)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcMotionLightRGB(AppMain.NNS_SUBMOTION submot, float frame, AppMain.NNS_RGB col)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcLightMotionCore(
      AppMain.NNS_LIGHTPTR dstptr,
      AppMain.NNS_LIGHTPTR litptr,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcLightMotion(
      AppMain.NNS_LIGHTPTR dstptr,
      AppMain.NNS_LIGHTPTR litptr,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        AppMain.mppAssertNotImpl();
    }

    private static int nnCalcMotionNodeHide(AppMain.NNS_SUBMOTION submot, float frame)
    {
        int val = 0;
        AppMain.NNS_MOTION_KEY_Class11[] pKeyList = (AppMain.NNS_MOTION_KEY_Class11[])submot.pKeyList;
        int nKeyFrame = submot.nKeyFrame;
        if ((submot.fIPType & 3703U) == 4U)
            AppMain.nnInterpolateConstantS32_1(pKeyList, nKeyFrame, frame, out val);
        return val;
    }

    private static void nnCalcNodeHideMotion(
      AppMain.ArrayPointer<uint> nodestatlist,
      AppMain.NNS_MOTION mot,
      float frame)
    {
        float dstframe1;
        if (((int)mot.fType & 1) == 0 || AppMain.nnCalcMotionFrame(out dstframe1, mot.fType, mot.StartFrame, mot.EndFrame, frame) == 0)
            return;
        AppMain.ArrayPointer<AppMain.NNS_SUBMOTION> arrayPointer = new AppMain.ArrayPointer<AppMain.NNS_SUBMOTION>(mot.pSubmotion);
        for (int index = 0; index < mot.nSubmotion; ++index)
        {
            AppMain.NNS_SUBMOTION submot = (AppMain.NNS_SUBMOTION)(AppMain.ArrayPointer<AppMain.NNS_SUBMOTION>)(arrayPointer + index);
            if (((int)submot.fType & 1048576) != 0 && (double)submot.StartFrame <= (double)dstframe1 && (double)dstframe1 <= (double)submot.EndFrame)
            {
                uint fType = submot.fIPType;
                if (((int)mot.fType & 131072) != 0 && (double)dstframe1 == (double)submot.EndFrame)
                    fType = 131072U;
                float dstframe2;
                if (AppMain.nnCalcMotionFrame(out dstframe2, fType, submot.StartKeyFrame, submot.EndKeyFrame, dstframe1) != 0 && AppMain.nnCalcMotionNodeHide(submot, dstframe2) != 0)
                {
                    int num = (int)((AppMain.ArrayPointer<uint>)(nodestatlist + submot.Id)).SetPrimitive(((AppMain.ArrayPointer<uint>)(nodestatlist + submot.Id))[0] | 1U);
                }
            }
        }
    }

    private static void nnInterpolateConstantF1(
      AppMain.NNS_MOTION_KEY_Class1[] vk,
      int nKey,
      float frame,
      out float val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class1 nnsMotionKeyClass1 = vk[(int)num1];
        val = nnsMotionKeyClass1.Value;
    }

    private static void nnInterpolateConstantF3(
      AppMain.NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      AppMain.NNS_VECTOR val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class5 nnsMotionKeyClass5 = vk[(int)num1];
        val.x = nnsMotionKeyClass5.Value.x;
        val.y = nnsMotionKeyClass5.Value.y;
        val.z = nnsMotionKeyClass5.Value.z;
    }

    private static void nnInterpolateConstantF3(
      AppMain.NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      out AppMain.SNNS_VECTOR val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class5 nnsMotionKeyClass5 = vk[(int)num1];
        val.x = nnsMotionKeyClass5.Value.x;
        val.y = nnsMotionKeyClass5.Value.y;
        val.z = nnsMotionKeyClass5.Value.z;
    }

    private static void nnInterpolateConstantF3(
      AppMain.NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_RGBA val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class5 nnsMotionKeyClass5 = vk[(int)num1];
        val.r = nnsMotionKeyClass5.Value.x;
        val.g = nnsMotionKeyClass5.Value.y;
        val.b = nnsMotionKeyClass5.Value.z;
    }

    private static void nnInterpolateConstantA32_1(
      AppMain.NNS_MOTION_KEY_Class8[] vk,
      int nKey,
      float frame,
      out int val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class8 nnsMotionKeyClass8 = vk[(int)num1];
        val = nnsMotionKeyClass8.Value;
    }

    private static void nnInterpolateConstantA32_3(
      AppMain.NNS_MOTION_KEY_Class13[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_ROTATE_A32 val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class13 motionKeyClass13 = vk[(int)num1];
        val.x = motionKeyClass13.Value.x;
        val.y = motionKeyClass13.Value.y;
        val.z = motionKeyClass13.Value.z;
    }

    private static void nnInterpolateConstantA16_1(
      AppMain.NNS_MOTION_KEY_Class14[] vk,
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
            if ((int)num1 >= (int)vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        AppMain.NNS_MOTION_KEY_Class14 motionKeyClass14 = vk[(int)num2];
        val = motionKeyClass14.Value;
    }

    private static void nnInterpolateConstantA16_3(
      AppMain.NNS_MOTION_KEY_Class16[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_ROTATE_A16 val)
    {
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if ((int)num1 >= (int)vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        AppMain.NNS_MOTION_KEY_Class16 motionKeyClass16 = vk[(int)num2];
        val.x = motionKeyClass16.Value.x;
        val.y = motionKeyClass16.Value.y;
        val.z = motionKeyClass16.Value.z;
    }

    private static void nnInterpolateLinearF1(
      AppMain.NNS_MOTION_KEY_Class1[] vk,
      int nKey,
      float frame,
      out float val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
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
            float num3 = (float)(((double)frame - (double)vk[index2].Frame) / ((double)vk[index1].Frame - (double)vk[index2].Frame));
            val = vk[index2].Value + (vk[index1].Value - vk[index2].Value) * num3;
        }
    }

    private static void nnInterpolateLinearF3(
      AppMain.NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      AppMain.NNS_VECTOR val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_1 = vk[(int)num1];
        if ((long)num1 >= (long)(nKey - 1))
        {
            val.x = nnsMotionKeyClass5_1.Value.x;
            val.y = nnsMotionKeyClass5_1.Value.y;
            val.z = nnsMotionKeyClass5_1.Value.z;
        }
        else
        {
            AppMain.NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_2 = vk[(int)(num1 + 1U)];
            float num3 = (float)(((double)frame - (double)nnsMotionKeyClass5_2.Frame) / ((double)nnsMotionKeyClass5_1.Frame - (double)nnsMotionKeyClass5_2.Frame));
            val.x = nnsMotionKeyClass5_2.Value.x + (nnsMotionKeyClass5_1.Value.x - nnsMotionKeyClass5_2.Value.x) * num3;
            val.y = nnsMotionKeyClass5_2.Value.y + (nnsMotionKeyClass5_1.Value.y - nnsMotionKeyClass5_2.Value.y) * num3;
            val.z = nnsMotionKeyClass5_2.Value.z + (nnsMotionKeyClass5_1.Value.z - nnsMotionKeyClass5_2.Value.z) * num3;
        }
    }

    private static void nnInterpolateLinearF3(
      AppMain.NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      out AppMain.SNNS_VECTOR val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_1 = vk[(int)num1];
        if ((long)num1 >= (long)(nKey - 1))
        {
            val.x = nnsMotionKeyClass5_1.Value.x;
            val.y = nnsMotionKeyClass5_1.Value.y;
            val.z = nnsMotionKeyClass5_1.Value.z;
        }
        else
        {
            AppMain.NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_2 = vk[(int)(num1 + 1U)];
            float num3 = (float)(((double)frame - (double)nnsMotionKeyClass5_2.Frame) / ((double)nnsMotionKeyClass5_1.Frame - (double)nnsMotionKeyClass5_2.Frame));
            val.x = nnsMotionKeyClass5_2.Value.x + (nnsMotionKeyClass5_1.Value.x - nnsMotionKeyClass5_2.Value.x) * num3;
            val.y = nnsMotionKeyClass5_2.Value.y + (nnsMotionKeyClass5_1.Value.y - nnsMotionKeyClass5_2.Value.y) * num3;
            val.z = nnsMotionKeyClass5_2.Value.z + (nnsMotionKeyClass5_1.Value.z - nnsMotionKeyClass5_2.Value.z) * num3;
        }
    }

    private static void nnInterpolateLinearF3(
      AppMain.NNS_MOTION_KEY_Class5[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_RGBA val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_1 = vk[(int)num1];
        if ((int)num1 >= nKey - 1)
        {
            val.r = nnsMotionKeyClass5_1.Value.x;
            val.g = nnsMotionKeyClass5_1.Value.y;
            val.b = nnsMotionKeyClass5_1.Value.z;
        }
        else
        {
            AppMain.NNS_MOTION_KEY_Class5 nnsMotionKeyClass5_2 = vk[(int)num1 + 1];
            float num3 = (float)(((double)frame - (double)nnsMotionKeyClass5_2.Frame) / ((double)nnsMotionKeyClass5_1.Frame - (double)nnsMotionKeyClass5_2.Frame));
            val.r = nnsMotionKeyClass5_2.Value.x + (nnsMotionKeyClass5_1.Value.x - nnsMotionKeyClass5_2.Value.x) * num3;
            val.g = nnsMotionKeyClass5_2.Value.y + (nnsMotionKeyClass5_1.Value.y - nnsMotionKeyClass5_2.Value.y) * num3;
            val.b = nnsMotionKeyClass5_2.Value.z + (nnsMotionKeyClass5_1.Value.z - nnsMotionKeyClass5_2.Value.z) * num3;
        }
    }

    private static void nnInterpolateLinearA32_1(
      AppMain.NNS_MOTION_KEY_Class8[] vk,
      int nKey,
      float frame,
      out int val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class8> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class8>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = ((AppMain.NNS_MOTION_KEY_Class8)~arrayPointer1).Value;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class8> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class8>)(arrayPointer1 + 1);
            float num3 = (float)(((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class8)~arrayPointer2).Frame) / ((double)((AppMain.NNS_MOTION_KEY_Class8)~arrayPointer1).Frame - (double)((AppMain.NNS_MOTION_KEY_Class8)~arrayPointer2).Frame));
            val = ((AppMain.NNS_MOTION_KEY_Class8)~arrayPointer2).Value + (int)((double)(((AppMain.NNS_MOTION_KEY_Class8)~arrayPointer1).Value - ((AppMain.NNS_MOTION_KEY_Class8)~arrayPointer2).Value) * (double)num3);
        }
    }

    private static void nnInterpolateLinearA32_3(
      AppMain.NNS_MOTION_KEY_Class13[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_ROTATE_A32 val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13>)(arrayPointer1 + 1);
            float num3 = (float)(((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Frame) / ((double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Frame - (double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Frame));
            val.x = ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.x + (int)((double)(((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.x - ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.x) * (double)num3);
            val.y = ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.y + (int)((double)(((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.y - ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.y) * (double)num3);
            val.z = ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.z + (int)((double)(((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.z - ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.z) * (double)num3);
        }
    }

    private static void nnInterpolateLinearA16_1(
      AppMain.NNS_MOTION_KEY_Class14[] vk,
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
            if ((int)num1 >= (int)vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        AppMain.NNS_MOTION_KEY_Class14 motionKeyClass14_1 = vk[(int)num2];
        if ((int)num2 >= nKey - 1)
        {
            val = motionKeyClass14_1.Value;
        }
        else
        {
            AppMain.NNS_MOTION_KEY_Class14 motionKeyClass14_2 = vk[(int)num2 + 1];
            int num4 = (int)(65536.0 * ((double)frame - (double)motionKeyClass14_2.Frame) / (double)((int)motionKeyClass14_1.Frame - (int)motionKeyClass14_2.Frame));
            val = (short)((int)motionKeyClass14_2.Value + ((int)(short)((int)motionKeyClass14_1.Value - (int)motionKeyClass14_2.Value) * num4 >> 16));
        }
    }

    private static void nnInterpolateLinearA16_3(
      AppMain.NNS_MOTION_KEY_Class16[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_ROTATE_A16 val)
    {
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if ((int)num1 >= (int)vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16>(vk, (int)num2);
        if ((int)num2 >= nKey - 1)
        {
            val.x = ((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.x;
            val.y = ((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.y;
            val.z = ((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.z;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16>)(arrayPointer1 + 1);
            int num4 = (int)(65536.0 * ((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Frame) / (double)((int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Frame - (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Frame));
            val.x = (short)((int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.x + ((int)(short)((int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.x - (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.x) * num4 >> 16));
            val.y = (short)((int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.y + ((int)(short)((int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.y - (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.y) * num4 >> 16));
            val.z = (short)((int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.z + ((int)(short)((int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.z - (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.z) * num4 >> 16));
        }
    }

    private static float nnSolveBezier(float f0, float h0, float f1, float h1, float frame)
    {
        float num1 = h1 - h0;
        float num2 = f1 - f0 + num1;
        float num3 = -2f * num2 - num1;
        float num4 = (float)(3.0 * ((double)num2 - (double)h0));
        float num5 = 3f * h0;
        float num6 = f0 - frame;
        float num7 = 0.0f;
        float num8 = 0.5f;
        float num9 = 0.5f;
        for (int index = 0; index < 16; ++index)
        {
            if ((double)(((num3 * num9 + num4) * num9 + num5) * num9 + num6) < 0.0)
                num7 = num9;
            num8 *= 0.5f;
            num9 = num7 + num8;
        }
        return num9;
    }

    private static void nnInterpolateBezierF1(
      AppMain.NNS_MOTION_KEY_Class2[] vk,
      int nKey,
      float frame,
      out float val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class2 nnsMotionKeyClass2_1 = vk[(int)num1];
        if ((int)num1 >= nKey - 1)
        {
            val = nnsMotionKeyClass2_1.Value;
        }
        else
        {
            AppMain.NNS_MOTION_KEY_Class2 nnsMotionKeyClass2_2 = vk[(int)num1 + 1];
            float num3 = AppMain.nnSolveBezier(nnsMotionKeyClass2_1.Frame, nnsMotionKeyClass2_1.Bhandle.Out.x, nnsMotionKeyClass2_2.Frame, nnsMotionKeyClass2_2.Bhandle.In.x, frame);
            float num4 = nnsMotionKeyClass2_1.Value;
            float y = nnsMotionKeyClass2_1.Bhandle.Out.y;
            float num5 = nnsMotionKeyClass2_2.Value;
            float num6 = nnsMotionKeyClass2_2.Bhandle.In.y - y;
            float num7 = num5 - num4 + num6;
            float num8 = -2f * num7 - num6;
            float num9 = (float)(3.0 * ((double)num7 - (double)y));
            float num10 = 3f * y;
            float num11 = num4;
            val = ((num8 * num3 + num9) * num3 + num10) * num3 + num11;
        }
    }

    private static void nnInterpolateBezierA32_1(
      AppMain.NNS_MOTION_KEY_Class9[] vk,
      int nKey,
      float frame,
      out int val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class9> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class9>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = ((AppMain.NNS_MOTION_KEY_Class9)~arrayPointer1).Value;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class9> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class9>)(arrayPointer1 + 1);
            float num3 = AppMain.nnSolveBezier(((AppMain.NNS_MOTION_KEY_Class9)~arrayPointer1).Frame, ((AppMain.NNS_MOTION_KEY_Class9)~arrayPointer1).Bhandle.Out.x, ((AppMain.NNS_MOTION_KEY_Class9)~arrayPointer2).Frame, ((AppMain.NNS_MOTION_KEY_Class9)~arrayPointer2).Bhandle.In.x, frame);
            float num4 = (float)((AppMain.NNS_MOTION_KEY_Class9)~arrayPointer1).Value;
            float y = ((AppMain.NNS_MOTION_KEY_Class9)~arrayPointer1).Bhandle.Out.y;
            float num5 = (float)((AppMain.NNS_MOTION_KEY_Class9)~arrayPointer2).Value;
            float num6 = ((AppMain.NNS_MOTION_KEY_Class9)~arrayPointer2).Bhandle.In.y - y;
            float num7 = num5 - num4 + num6;
            float num8 = -2f * num7 - num6;
            float num9 = (float)(3.0 * ((double)num7 - (double)y));
            float num10 = 3f * y;
            float num11 = num4;
            val = (int)((((double)num8 * (double)num3 + (double)num9) * (double)num3 + (double)num10) * (double)num3 + (double)num11);
        }
    }

    private static void nnRotXYZtoQuat(
      ref AppMain.NNS_QUATERNION dst,
      int rx,
      int ry,
      int rz,
      uint rtype)
    {
        switch (rtype)
        {
            case 256:
                AppMain.nnMakeRotateXZYQuaternion(out dst, rx, ry, rz);
                break;
            case 1024:
                AppMain.nnMakeRotateZXYQuaternion(out dst, rx, ry, rz);
                break;
            default:
                AppMain.nnMakeRotateXYZQuaternion(out dst, rx, ry, rz);
                break;
        }
    }

    private static void nnInterpolateLerpA16_3(
      AppMain.NNS_MOTION_KEY_Class16[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_QUATERNION val,
      uint rtype)
    {
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if ((int)num1 >= (int)vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16>(vk, (int)num2);
        if ((int)num2 >= nKey - 1)
        {
            AppMain.nnRotXYZtoQuat(ref val, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.z, rtype);
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16>)(arrayPointer1 + 1);
            float t = (frame - (float)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Frame) / (float)((int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Frame - (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Frame);
            AppMain.nnRotXYZtoQuat(ref nnsQuaternion1, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.z, rtype);
            AppMain.nnRotXYZtoQuat(ref nnsQuaternion2, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.z, rtype);
            AppMain.nnLerpQuaternion(ref val, ref nnsQuaternion1, ref nnsQuaternion2, t);
        }
    }

    private static void nnInterpolateLerpA32_3(
      AppMain.NNS_MOTION_KEY_Class13[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_QUATERNION val,
      uint rtype)
    {
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            AppMain.nnRotXYZtoQuat(ref val, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.z, rtype);
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13>)(arrayPointer1 + 1);
            float t = (float)(((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Frame) / ((double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Frame - (double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Frame));
            AppMain.nnRotXYZtoQuat(ref nnsQuaternion1, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.z, rtype);
            AppMain.nnRotXYZtoQuat(ref nnsQuaternion2, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.z, rtype);
            AppMain.nnLerpQuaternion(ref val, ref nnsQuaternion1, ref nnsQuaternion2, t);
        }
    }

    private static void nnInterpolateLerpQuat_4(
      AppMain.NNS_MOTION_KEY_Class7[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_QUATERNION val)
    {
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Value;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7>)(arrayPointer1 + 1);
            float t = (float)(((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Frame) / ((double)((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer2).Frame - (double)((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Frame));
            AppMain.NNS_QUATERNION quat1 = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Value;
            AppMain.NNS_QUATERNION quat2 = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer2).Value;
            AppMain.nnLerpQuaternion(ref val, ref quat1, ref quat2, t);
        }
    }

    private static void nnInterpolateSlerpA16_3(
      AppMain.NNS_MOTION_KEY_Class16[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_QUATERNION val,
      uint rtype)
    {
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if ((int)num1 >= (int)vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16>(vk, (int)num2);
        if ((int)num2 >= nKey - 1)
        {
            AppMain.nnRotXYZtoQuat(ref val, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.z, rtype);
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16>)(arrayPointer1 + 1);
            float t = (frame - (float)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Frame) / (float)((int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Frame - (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Frame);
            AppMain.nnRotXYZtoQuat(ref nnsQuaternion1, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.z, rtype);
            AppMain.nnRotXYZtoQuat(ref nnsQuaternion2, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.z, rtype);
            AppMain.nnSlerpQuaternion(out val, ref nnsQuaternion1, ref nnsQuaternion2, t);
        }
    }

    private static void nnInterpolateSlerpA32_3(
      AppMain.NNS_MOTION_KEY_Class13[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_QUATERNION val,
      uint rtype)
    {
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            AppMain.nnRotXYZtoQuat(ref val, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.z, rtype);
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13>)(arrayPointer1 + 1);
            float t = (float)(((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Frame) / ((double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Frame - (double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Frame));
            AppMain.nnRotXYZtoQuat(ref nnsQuaternion1, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.z, rtype);
            AppMain.nnRotXYZtoQuat(ref nnsQuaternion2, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.z, rtype);
            AppMain.nnSlerpQuaternion(out val, ref nnsQuaternion1, ref nnsQuaternion2, t);
        }
    }

    private static void nnInterpolateSlerpQuat_4(
      AppMain.NNS_MOTION_KEY_Class7[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_QUATERNION val)
    {
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Value;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7>)(arrayPointer1 + 1);
            float t = (float)(((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Frame) / ((double)((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer2).Frame - (double)((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Frame));
            AppMain.NNS_QUATERNION quat1 = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Value;
            AppMain.NNS_QUATERNION quat2 = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer2).Value;
            AppMain.nnSlerpQuaternion(out val, ref quat1, ref quat2, t);
        }
    }

    private static void nnInterpolateSquadA16_3(
      AppMain.NNS_MOTION_KEY_Class16[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_QUATERNION val,
      uint rtype)
    {
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion3 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion4 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion5 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion6 = new AppMain.NNS_QUATERNION();
        short num1 = (short)frame;
        uint num2 = 0;
        uint num3 = (uint)nKey;
        while (num3 - num2 > 1U)
        {
            uint num4 = num2 + num3 >> 1;
            if ((int)num1 >= (int)vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16>(vk, (int)num2);
        if ((int)num2 >= nKey - 1)
        {
            AppMain.nnRotXYZtoQuat(ref val, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.z, rtype);
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16>)(arrayPointer1 + 1);
            if (num2 > 0U && (int)num2 < nKey - 2)
            {
                AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16> arrayPointer3 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16>)(arrayPointer1 - 1);
                AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16> arrayPointer4 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class16>)(arrayPointer1 + 2);
                float t = (frame - (float)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Frame) / (float)((int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Frame - (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Frame);
                AppMain.nnRotXYZtoQuat(ref nnsQuaternion1, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer3).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer3).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer3).Value.z, rtype);
                AppMain.nnRotXYZtoQuat(ref nnsQuaternion2, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer1).Value.z, rtype);
                AppMain.nnRotXYZtoQuat(ref nnsQuaternion3, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer2).Value.z, rtype);
                AppMain.nnRotXYZtoQuat(ref nnsQuaternion4, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer4).Value.x, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer4).Value.y, (int)((AppMain.NNS_MOTION_KEY_Class16)~arrayPointer4).Value.z, rtype);
                AppMain.nnSplineQuaternion(ref nnsQuaternion5, ref nnsQuaternion1, ref nnsQuaternion2, ref nnsQuaternion3);
                AppMain.nnSplineQuaternion(ref nnsQuaternion6, ref nnsQuaternion2, ref nnsQuaternion3, ref nnsQuaternion4);
                AppMain.nnSquadQuaternion(ref val, ref nnsQuaternion2, ref nnsQuaternion5, ref nnsQuaternion6, ref nnsQuaternion3, t);
            }
            else
                AppMain.nnInterpolateSlerpA16_3(vk, nKey, frame, ref val, rtype);
        }
    }

    private static void nnInterpolateSquadA32_3(
      AppMain.NNS_MOTION_KEY_Class13[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_QUATERNION val,
      uint rtype)
    {
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion3 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion4 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion5 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion6 = new AppMain.NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            AppMain.nnRotXYZtoQuat(ref val, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.z, rtype);
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13>)(arrayPointer1 + 1);
            if (num1 > 0U && (int)num1 < nKey - 2)
            {
                AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13> arrayPointer3 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13>)(arrayPointer1 - 1);
                AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13> arrayPointer4 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class13>)(arrayPointer1 + 2);
                float t = (float)(((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Frame) / ((double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Frame - (double)((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Frame));
                AppMain.nnRotXYZtoQuat(ref nnsQuaternion1, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer3).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer3).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer3).Value.z, rtype);
                AppMain.nnRotXYZtoQuat(ref nnsQuaternion2, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer1).Value.z, rtype);
                AppMain.nnRotXYZtoQuat(ref nnsQuaternion3, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer2).Value.z, rtype);
                AppMain.nnRotXYZtoQuat(ref nnsQuaternion4, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer4).Value.x, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer4).Value.y, ((AppMain.NNS_MOTION_KEY_Class13)~arrayPointer4).Value.z, rtype);
                AppMain.nnSplineQuaternion(ref nnsQuaternion5, ref nnsQuaternion1, ref nnsQuaternion2, ref nnsQuaternion3);
                AppMain.nnSplineQuaternion(ref nnsQuaternion6, ref nnsQuaternion2, ref nnsQuaternion3, ref nnsQuaternion4);
                AppMain.nnSquadQuaternion(ref val, ref nnsQuaternion2, ref nnsQuaternion5, ref nnsQuaternion6, ref nnsQuaternion3, t);
            }
            else
                AppMain.nnInterpolateSlerpA32_3(vk, nKey, frame, ref val, rtype);
        }
    }

    private static void nnInterpolateSquadQuat_4(
      AppMain.NNS_MOTION_KEY_Class7[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_QUATERNION val)
    {
        AppMain.NNS_QUATERNION nnsQuaternion1 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion2 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion3 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion4 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion5 = new AppMain.NNS_QUATERNION();
        AppMain.NNS_QUATERNION nnsQuaternion6 = new AppMain.NNS_QUATERNION();
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Value;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7>)(arrayPointer1 + 1);
            if (num1 > 0U && (int)num1 < nKey - 2)
            {
                AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7> arrayPointer3 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7>)(arrayPointer1 - 1);
                AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7> arrayPointer4 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class7>)(arrayPointer1 + 2);
                float t = (float)(((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Frame) / ((double)((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer2).Frame - (double)((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Frame));
                AppMain.NNS_QUATERNION quatprev = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer3).Value;
                AppMain.NNS_QUATERNION nnsQuaternion7 = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer1).Value;
                AppMain.NNS_QUATERNION nnsQuaternion8 = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer2).Value;
                AppMain.NNS_QUATERNION quatnext = ((AppMain.NNS_MOTION_KEY_Class7)~arrayPointer4).Value;
                AppMain.nnSplineQuaternion(ref nnsQuaternion5, ref quatprev, ref nnsQuaternion7, ref nnsQuaternion8);
                AppMain.nnSplineQuaternion(ref nnsQuaternion6, ref nnsQuaternion7, ref nnsQuaternion8, ref quatnext);
                AppMain.nnSquadQuaternion(ref val, ref nnsQuaternion7, ref nnsQuaternion5, ref nnsQuaternion6, ref nnsQuaternion8, t);
            }
            else
                AppMain.nnInterpolateSlerpQuat_4(vk, nKey, frame, ref val);
        }
    }

    private static void nnInterpolateConstantQuat_4(
      AppMain.NNS_MOTION_KEY_Class7[] vk,
      int nKey,
      float frame,
      ref AppMain.NNS_QUATERNION val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class7 nnsMotionKeyClass7 = vk[(int)num1];
        val.x = nnsMotionKeyClass7.Value.x;
        val.y = nnsMotionKeyClass7.Value.y;
        val.z = nnsMotionKeyClass7.Value.z;
        val.w = nnsMotionKeyClass7.Value.w;
    }

    private static void nnInterpolateSISplineF1(
      AppMain.NNS_MOTION_KEY_Class3[] vk,
      int nKey,
      float frame,
      out float val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class3> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class3>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = ((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer1).Value;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class3> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class3>)(arrayPointer1 + 1);
            float num3 = ((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer2).Frame - ((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer1).Frame;
            float num4 = ((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer2).Value - ((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer1).Value;
            float num5 = (frame - ((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer1).Frame) / num3;
            float num6 = (float)(-2.0 * (double)num4 + (double)num3 * ((double)((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer1).Shandle.Out + (double)((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer2).Shandle.In));
            float num7 = (float)(3.0 * (double)num4 - (double)num3 * (2.0 * (double)((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer1).Shandle.Out + (double)((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer2).Shandle.In));
            float num8 = num3 * ((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer1).Shandle.Out;
            val = ((AppMain.NNS_MOTION_KEY_Class3)~arrayPointer1).Value + num5 * (num5 * (num6 * num5 + num7) + num8);
        }
    }

    private static void nnInterpolateSISplineA32_1(
      AppMain.NNS_MOTION_KEY_Class10[] vk,
      int nKey,
      float frame,
      out int val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class10> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class10>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = ((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer1).Value;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class10> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class10>)(arrayPointer1 + 1);
            float num3 = ((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer2).Frame - ((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer1).Frame;
            int num4 = ((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer2).Value - ((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer1).Value;
            float num5 = (frame - ((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer1).Frame) / num3;
            float num6 = (float)(-2 * num4) + num3 * (((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer1).Shandle.Out + ((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer2).Shandle.In);
            float num7 = (float)(3 * num4) - num3 * (2f * ((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer1).Shandle.Out + ((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer2).Shandle.In);
            float num8 = num3 * ((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer1).Shandle.Out;
            val = (int)((double)((AppMain.NNS_MOTION_KEY_Class10)~arrayPointer1).Value + (double)num5 * ((double)num5 * ((double)num6 * (double)num5 + (double)num7) + (double)num8));
        }
    }

    private static void nnInterpolateSISplineA16_1(
      AppMain.NNS_MOTION_KEY_Class15[] vk,
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
            if ((int)num1 >= (int)vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class15> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class15>(vk, (int)num2);
        if ((int)num2 >= nKey - 1)
        {
            val = ((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer1).Value;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class15> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class15>)(arrayPointer1 + 1);
            float num4 = (float)((int)((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer2).Frame - (int)((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer1).Frame);
            short num5 = (short)((int)((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer2).Value - (int)((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer1).Value);
            float num6 = (frame - (float)((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer1).Frame) / num4;
            float num7 = (float)(-2 * (int)num5) + num4 * (((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer1).Shandle.Out + ((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer2).Shandle.In);
            float num8 = (float)(3 * (int)num5) - num4 * (2f * ((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer1).Shandle.Out + ((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer2).Shandle.In);
            float num9 = num4 * ((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer1).Shandle.Out;
            val = (short)((double)((AppMain.NNS_MOTION_KEY_Class15)~arrayPointer1).Value + (double)num6 * ((double)num6 * ((double)num7 * (double)num6 + (double)num8) + (double)num9));
        }
    }

    private static void nnInterpolateConstantU1(
      AppMain.NNS_MOTION_KEY_Class12[] vk,
      int nKey,
      float frame,
      out uint val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class12 motionKeyClass12 = vk[(int)num1];
        val = motionKeyClass12.Value;
    }

    private static void nnInterpolateLinearU1(
      AppMain.NNS_MOTION_KEY_Class12[] vk,
      int nKey,
      float frame,
      out uint val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class12> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class12>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val = ((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer1).Value;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class12> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class12>)(arrayPointer1 + 1);
            float num3 = (float)(((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer2).Frame) / ((double)((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer1).Frame - (double)((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer2).Frame));
            val = (uint)((double)((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer1).Value * (double)num3 + (double)((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer2).Value * (1.0 - (double)num3));
        }
    }

    private static int nnInterpolateTriggerU1(
      AppMain.NNS_MOTION_KEY_Class12[] vk,
      int nKey,
      float frame,
      out uint val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class12 motionKeyClass12 = vk[(int)num1];
        if ((double)frame > (double)motionKeyClass12.Frame + (double)AppMain.nngNodeUserMotionTriggerTime)
        {
            val = 0U;
            return 0;
        }
        val = motionKeyClass12.Value;
        return 1;
    }

    private static void nnSearchTriggerU1(
      AppMain.NNS_MOTION_KEY_Class12[] vk,
      int nKey,
      float frame,
      float interval,
      AppMain.NNS_NODEUSRMOT_CALLBACK_FUNC func,
      AppMain.NNS_NODEUSRMOT_CALLBACK_VAL val)
    {
        if (nKey == 0)
            return;
        int _offset1;
        int _offset2;
        if ((double)vk[0].Frame < (double)frame && (double)frame < (double)vk[nKey - 1].Frame)
        {
            uint num1 = 0;
            uint num2 = (uint)nKey;
            while (num2 - num1 > 1U)
            {
                uint num3 = num1 + num2 >> 1;
                if ((double)frame > (double)vk[(int)num3].Frame)
                    num1 = num3;
                else
                    num2 = num3;
            }
            if ((double)vk[(int)num1].Frame == (double)frame)
                _offset2 = _offset1 = (int)num1;
            else if ((double)vk[(int)num2].Frame == (double)frame)
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
            if ((double)frame < (double)vk[0].Frame)
                _offset1 = 0;
            if ((double)vk[nKey - 1].Frame < (double)frame)
                _offset2 = nKey - 1;
            if ((double)vk[0].Frame == (double)frame)
                _offset2 = _offset1 = 0;
            if ((double)vk[nKey - 1].Frame == (double)frame)
                _offset2 = _offset1 = nKey - 1;
        }
        if ((double)interval > 0.0 && _offset2 != -1)
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class12> arrayPointer = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class12>(vk, _offset2);
            while ((double)((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer).Frame + (double)interval > (double)frame && _offset2 >= 0)
            {
                val.IValue = ((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer).Value;
                val.Frame = ((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer).Frame;
                func(val);
                --_offset2;
                --arrayPointer;
            }
        }
        else
        {
            if ((double)interval >= 0.0 || _offset1 == -1)
                return;
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class12> arrayPointer = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class12>(vk, _offset1);
            while ((double)((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer).Frame + (double)interval < (double)frame && _offset1 < nKey)
            {
                val.IValue = ((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer).Value;
                val.Frame = ((AppMain.NNS_MOTION_KEY_Class12)~arrayPointer).Frame;
                func(val);
                ++_offset1;
                ++arrayPointer;
            }
        }
    }

    private static void nnInterpolateConstantS32_1(
      AppMain.NNS_MOTION_KEY_Class11[] vk,
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
            if ((double)num1 >= (double)vk[(int)num4].Frame)
                num2 = num4;
            else
                num3 = num4;
        }
        AppMain.NNS_MOTION_KEY_Class11 motionKeyClass11 = vk[(int)num2];
        val = motionKeyClass11.Value;
    }

    private static void nnInterpolateConstantF2(
      AppMain.NNS_MOTION_KEY_Class4[] vk,
      int nKey,
      float frame,
      out AppMain.NNS_TEXCOORD val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.NNS_MOTION_KEY_Class4 nnsMotionKeyClass4 = vk[(int)num1];
        val.u = nnsMotionKeyClass4.Value.u;
        val.v = nnsMotionKeyClass4.Value.v;
    }

    private static void nnInterpolateLinearF2(
      AppMain.NNS_MOTION_KEY_Class4[] vk,
      int nKey,
      float frame,
      out AppMain.NNS_TEXCOORD val)
    {
        uint num1 = 0;
        uint num2 = (uint)nKey;
        while (num2 - num1 > 1U)
        {
            uint num3 = num1 + num2 >> 1;
            if ((double)frame >= (double)vk[(int)num3].Frame)
                num1 = num3;
            else
                num2 = num3;
        }
        AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class4> arrayPointer1 = new AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class4>(vk, (int)num1);
        if ((int)num1 >= nKey - 1)
        {
            val.u = ((AppMain.NNS_MOTION_KEY_Class4)~arrayPointer1).Value.u;
            val.v = ((AppMain.NNS_MOTION_KEY_Class4)~arrayPointer1).Value.v;
        }
        else
        {
            AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class4> arrayPointer2 = (AppMain.ArrayPointer<AppMain.NNS_MOTION_KEY_Class4>)(arrayPointer1 + 1);
            float num3 = (float)(((double)frame - (double)((AppMain.NNS_MOTION_KEY_Class4)~arrayPointer2).Frame) / ((double)((AppMain.NNS_MOTION_KEY_Class4)~arrayPointer1).Frame - (double)((AppMain.NNS_MOTION_KEY_Class4)~arrayPointer2).Frame));
            float num4 = 1f - num3;
            val.u = (float)((double)((AppMain.NNS_MOTION_KEY_Class4)~arrayPointer1).Value.u * (double)num3 + (double)((AppMain.NNS_MOTION_KEY_Class4)~arrayPointer2).Value.u * (double)num4);
            val.v = (float)((double)((AppMain.NNS_MOTION_KEY_Class4)~arrayPointer1).Value.v * (double)num3 + (double)((AppMain.NNS_MOTION_KEY_Class4)~arrayPointer2).Value.v * (double)num4);
        }
    }


    private static void nnCalc1BoneSIIK(
      AppMain.NNS_MATRIX jnt1mtx,
      AppMain.NNS_MATRIX jnt1motmtx,
      AppMain.NNS_MATRIX effmtx,
      float lbone1)
    {
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.mppAssertNotImpl();
    }

    private static void nnCalc2BoneSIIK(
      AppMain.NNS_MATRIX jnt1mtx,
      AppMain.NNS_MATRIX jnt1motmtx,
      AppMain.NNS_MATRIX jnt2mtx,
      AppMain.NNS_MATRIX jnt2motmtx,
      AppMain.NNS_MATRIX effmtx,
      float lbone1,
      float lbone2,
      int zpref)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnAdjustMatrixXaxis(AppMain.NNS_MATRIX mtx, AppMain.NNS_VECTORFAST pos)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcCosineTheorem(out float sin, out float cos, float a, float b, float c)
    {
        AppMain.mppAssertNotImpl();
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
        AppMain.mppAssertNotImpl();
        sin0 = cos0 = sin1 = cos1 = 0.0f;
    }

    private void nnCalcNodeStatusListInitialPose(
      uint[] nodestatlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnmSetUpVector(AppMain.NNS_VECTOR vec, float x, float y, float z)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnmAddScaleVector(AppMain.NNS_VECTOR dst, AppMain.NNS_VECTOR add, float scl)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnmBlendVector(
      AppMain.NNS_VECTOR dst,
      AppMain.NNS_VECTOR vec1,
      float blend1,
      AppMain.NNS_VECTOR vec2,
      float blend2)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnmTransformVector(
      AppMain.NNS_VECTOR dst,
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR src)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnmTransformNormalVector(
      AppMain.NNS_VECTOR dst,
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_VECTOR src)
    {
        AppMain.mppAssertNotImpl();
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
      AppMain.NNS_MATRIX[] posmtx,
      AppMain.NNS_MATRIX[] nrmmtx)
    {
        AppMain.mppAssertNotImpl();
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
      AppMain.NNS_MATRIX[] posmtx,
      AppMain.NNS_MATRIX[] nrmmtx)
    {
        AppMain.mppAssertNotImpl();
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
      AppMain.NNS_MATRIX[] posmtx,
      AppMain.NNS_MATRIX[] nrmmtx)
    {
        AppMain.mppAssertNotImpl();
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
      AppMain.NNS_MATRIX[] posmtx,
      AppMain.NNS_MATRIX[] nrmmtx)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcPliableObject(
      AppMain.NNS_PLIABLEOBJ pobj,
      AppMain.NNS_MATRIX mtxpal,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnCalcPliableObjectNodeStatusList(
      AppMain.NNS_PLIABLEOBJ pobj,
      AppMain.NNS_MATRIX mtxpal,
      uint nodestatlist,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnTransformUpVectorCameraLocal(
      AppMain.NNS_VECTOR vec,
      AppMain.NNS_CAMERA_TARGET_UPVECTOR cam,
      float x,
      float y,
      float z)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnPitchUpVectorCamera(AppMain.NNS_CAMERA_TARGET_UPVECTOR cam, int pitch)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnRollUpVectorCamera(AppMain.NNS_CAMERA_TARGET_UPVECTOR cam, int roll)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnYawUpVectorCamera(AppMain.NNS_CAMERA_TARGET_UPVECTOR cam, int yaw)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnApproachTargetUpVectorCamera(AppMain.NNS_CAMERA_TARGET_UPVECTOR cam, float d)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnApproachTargetUpVectorCameraLevel(AppMain.NNS_CAMERA_TARGET_UPVECTOR cam, float d)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnMoveTargetUpVectorCamera(AppMain.NNS_CAMERA_TARGET_UPVECTOR cam, float d)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnRotateUpVectorCameraAroundTargetH(AppMain.NNS_CAMERA_TARGET_UPVECTOR cam, int ang)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnRotateUpVectorCameraAroundTargetV(AppMain.NNS_CAMERA_TARGET_UPVECTOR cam, int ang)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnRotateUpVectorCameraLevelAroundTarget(
      AppMain.NNS_CAMERA_TARGET_UPVECTOR cam,
      int ang)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnMoveUpVectorCameraLocal(
      AppMain.NNS_CAMERA_TARGET_UPVECTOR cam,
      float x,
      float y,
      float z)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnConvertCameraPointerUpVectorCamera(
      AppMain.NNS_CAMERA_TARGET_UPVECTOR cam,
      AppMain.NNS_CAMERAPTR camptr)
    {
        AppMain.mppAssertNotImpl();
    }


    private void nnDrawMultiObject(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX[] mtxpalptrlist,
      uint[] nodestatlistptrlist,
      uint subobjtype,
      uint flag,
      int num)
    {
        AppMain.mppAssertNotImpl();
    }

    private uint nnCalcObjectSize(AppMain.NNS_OBJECT obj)
    {
        return 0;
    }

    private static uint nnCopyMaterialList(
      AppMain.NNS_MATERIALPTR[] dstmatptr,
      AppMain.NNS_MATERIALPTR[] srcmatptr,
      int nMaterial,
      uint flag)
    {
        for (int index1 = 0; index1 < nMaterial; ++index1)
        {
            if (((int)srcmatptr[index1].fType & 2) != 0)
            {
                AppMain.NNS_MATERIAL_STDSHADER_DESC materialStdshaderDesc = (AppMain.NNS_MATERIAL_STDSHADER_DESC)null;
                AppMain.NNS_MATERIAL_STDSHADER_DESC pMaterial = (AppMain.NNS_MATERIAL_STDSHADER_DESC)srcmatptr[index1].pMaterial;
                if (((int)srcmatptr[index1].fType & 4) != 0)
                {
                    if (dstmatptr != null)
                    {
                        dstmatptr[index1].fType = 6U;
                        dstmatptr[index1].pMaterial = (object)(AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)(materialStdshaderDesc = (AppMain.NNS_MATERIAL_STDSHADER_DESC)new AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE());
                        ((AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)materialStdshaderDesc).Assign((AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)pMaterial);
                    }
                }
                else if (dstmatptr != null)
                {
                    dstmatptr[index1].fType = 2U;
                    dstmatptr[index1].pMaterial = (object)(materialStdshaderDesc = new AppMain.NNS_MATERIAL_STDSHADER_DESC());
                    materialStdshaderDesc.Assign(pMaterial);
                }
                AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor1 = pMaterial.pColor;
                int num1 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor2 = ((AppMain.NNS_MATERIAL_STDSHADER_DESC)srcmatptr[index2].pMaterial).pColor;
                    if (pColor1 == pColor2)
                    {
                        if (dstmatptr != null)
                            materialStdshaderDesc.pColor = ((AppMain.NNS_MATERIAL_STDSHADER_DESC)dstmatptr[index2].pMaterial).pColor;
                        num1 = 0;
                        break;
                    }
                }
                if (num1 != 0 && dstmatptr != null)
                {
                    materialStdshaderDesc.pColor = new AppMain.NNS_MATERIAL_STDSHADER_COLOR();
                    materialStdshaderDesc.pColor.Assign(pColor1);
                }
                AppMain.NNS_MATERIAL_LOGIC pLogic1 = pMaterial.pLogic;
                int num2 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    AppMain.NNS_MATERIAL_LOGIC pLogic2 = ((AppMain.NNS_MATERIAL_STDSHADER_DESC)srcmatptr[index2].pMaterial).pLogic;
                    if (pLogic1 == pLogic2)
                    {
                        if (dstmatptr != null)
                            materialStdshaderDesc.pLogic = ((AppMain.NNS_MATERIAL_STDSHADER_DESC)dstmatptr[index2].pMaterial).pLogic;
                        num2 = 0;
                        break;
                    }
                }
                if (num2 != 0 && dstmatptr != null)
                {
                    materialStdshaderDesc.pLogic = new AppMain.NNS_MATERIAL_LOGIC();
                    materialStdshaderDesc.pLogic.Assign(pLogic1);
                }
                if (pMaterial.nTex > 0)
                {
                    if (dstmatptr != null)
                    {
                        materialStdshaderDesc.pTexDesc = new AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC[pMaterial.nTex];
                        int index2 = 0;
                        for (; index1 < pMaterial.nTex; ++index1)
                            materialStdshaderDesc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                    }
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                    {
                        AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC stdshaderTexmapDesc1 = pMaterial.pTexDesc[index2];
                        AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC stdshaderTexmapDesc2 = (AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC)null;
                        if (dstmatptr != null)
                            stdshaderTexmapDesc2 = materialStdshaderDesc.pTexDesc[index2];
                        if (stdshaderTexmapDesc1.pBorderColor.HasValue && dstmatptr != null)
                        {
                            stdshaderTexmapDesc2.pBorderColor = new AppMain.NNS_RGBA?(new AppMain.NNS_RGBA());
                            stdshaderTexmapDesc2.pBorderColor = stdshaderTexmapDesc1.pBorderColor;
                        }
                        if (stdshaderTexmapDesc1.pFilterMode != null && dstmatptr != null)
                        {
                            stdshaderTexmapDesc2.pFilterMode = new AppMain.NNS_TEXTURE_FILTERMODE();
                            stdshaderTexmapDesc2.pFilterMode.Assign(stdshaderTexmapDesc1.pFilterMode);
                        }
                        if (stdshaderTexmapDesc1.pLODParam != null && dstmatptr != null)
                        {
                            stdshaderTexmapDesc2.pLODParam = new AppMain.NNS_TEXTURE_LOD_PARAM();
                            stdshaderTexmapDesc2.pLODParam.Assign(stdshaderTexmapDesc1.pLODParam);
                        }
                    }
                }
            }
            else if (((int)srcmatptr[index1].fType & 8) != 0)
            {
                AppMain.NNS_MATERIAL_GLES11_DESC materialGleS11Desc = (AppMain.NNS_MATERIAL_GLES11_DESC)null;
                AppMain.NNS_MATERIAL_GLES11_DESC pMaterial = (AppMain.NNS_MATERIAL_GLES11_DESC)srcmatptr[index1].pMaterial;
                if (dstmatptr != null)
                {
                    dstmatptr[index1].fType = 8U;
                    dstmatptr[index1].pMaterial = (object)(materialGleS11Desc = new AppMain.NNS_MATERIAL_GLES11_DESC());
                    materialGleS11Desc.Assign(pMaterial);
                }
                AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor1 = pMaterial.pColor;
                int num1 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor2 = ((AppMain.NNS_MATERIAL_GLES11_DESC)srcmatptr[index2].pMaterial).pColor;
                    if (pColor1 == pColor2)
                    {
                        if (dstmatptr != null)
                            materialGleS11Desc.pColor = ((AppMain.NNS_MATERIAL_GLES11_DESC)dstmatptr[index2].pMaterial).pColor;
                        num1 = 0;
                        break;
                    }
                }
                if (num1 != 0 && dstmatptr != null)
                {
                    materialGleS11Desc.pColor = new AppMain.NNS_MATERIAL_STDSHADER_COLOR();
                    materialGleS11Desc.pColor.Assign(pColor1);
                }
                AppMain.NNS_MATERIAL_GLES11_LOGIC pLogic1 = pMaterial.pLogic;
                int num2 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    AppMain.NNS_MATERIAL_GLES11_LOGIC pLogic2 = ((AppMain.NNS_MATERIAL_GLES11_DESC)srcmatptr[index2].pMaterial).pLogic;
                    if (pLogic1 == pLogic2)
                    {
                        if (dstmatptr != null)
                            materialGleS11Desc.pLogic = ((AppMain.NNS_MATERIAL_GLES11_DESC)dstmatptr[index2].pMaterial).pLogic;
                        num2 = 0;
                        break;
                    }
                }
                if (num2 != 0 && dstmatptr != null)
                {
                    materialGleS11Desc.pLogic = new AppMain.NNS_MATERIAL_GLES11_LOGIC();
                    materialGleS11Desc.pLogic.Assign(pLogic1);
                }
                if (pMaterial.nTex > 0)
                {
                    if (dstmatptr != null)
                    {
                        materialGleS11Desc.pTexDesc = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[pMaterial.nTex];
                        for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                            materialGleS11Desc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC(ref pMaterial.pTexDesc[index2]);
                    }
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                    {
                        AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC gleS11TexmapDesc1 = pMaterial.pTexDesc[index2];
                        AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC gleS11TexmapDesc2;
                        if (dstmatptr != null)
                            gleS11TexmapDesc2 = materialGleS11Desc.pTexDesc[index2];
                        if (gleS11TexmapDesc1.pCombine != null && dstmatptr != null)
                        {
                            gleS11TexmapDesc2.pCombine = new AppMain.NNS_TEXTURE_GLES11_COMBINE();
                            gleS11TexmapDesc2.pCombine.Assign(gleS11TexmapDesc1.pCombine);
                        }
                        if (gleS11TexmapDesc1.pFilterMode != null && dstmatptr != null)
                        {
                            gleS11TexmapDesc2.pFilterMode = new AppMain.NNS_TEXTURE_FILTERMODE();
                            gleS11TexmapDesc2.pFilterMode.Assign(gleS11TexmapDesc1.pFilterMode);
                        }
                    }
                }
            }
            else if (((int)srcmatptr[index1].fType & 1) != 0)
            {
                AppMain.NNS_MATERIAL_DESC nnsMaterialDesc = (AppMain.NNS_MATERIAL_DESC)null;
                AppMain.NNS_MATERIAL_DESC pMaterial1 = (AppMain.NNS_MATERIAL_DESC)srcmatptr[index1].pMaterial;
                if (dstmatptr != null)
                {
                    dstmatptr[index1].fType = 1U;
                    dstmatptr[index1].pMaterial = (object)(nnsMaterialDesc = new AppMain.NNS_MATERIAL_DESC());
                    nnsMaterialDesc.Assign(pMaterial1);
                }
                AppMain.NNS_MATERIAL_COLOR pColor1 = pMaterial1.pColor;
                int num1 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    AppMain.NNS_MATERIAL_DESC pMaterial2 = (AppMain.NNS_MATERIAL_DESC)srcmatptr[index2].pMaterial;
                    AppMain.NNS_MATERIAL_COLOR pColor2 = pMaterial2.pColor;
                    AppMain.NNS_MATERIAL_COLOR pBackColor = pMaterial2.pBackColor;
                    if (pColor1 == pColor2)
                    {
                        if (dstmatptr != null)
                            nnsMaterialDesc.pColor = ((AppMain.NNS_MATERIAL_DESC)dstmatptr[index2].pMaterial).pColor;
                        num1 = 0;
                        break;
                    }
                    if (pColor1 == pBackColor)
                    {
                        if (dstmatptr != null)
                            nnsMaterialDesc.pColor = ((AppMain.NNS_MATERIAL_DESC)dstmatptr[index2].pMaterial).pBackColor;
                        num1 = 0;
                        break;
                    }
                }
                if (num1 != 0 && dstmatptr != null)
                {
                    nnsMaterialDesc.pColor = new AppMain.NNS_MATERIAL_COLOR();
                    nnsMaterialDesc.pColor.Assign(pColor1);
                }
                AppMain.NNS_MATERIAL_COLOR pBackColor1 = pMaterial1.pBackColor;
                if (pBackColor1 != null)
                {
                    int num2 = 1;
                    for (int index2 = 0; index2 < index1; ++index2)
                    {
                        AppMain.NNS_MATERIAL_DESC pMaterial2 = (AppMain.NNS_MATERIAL_DESC)srcmatptr[index2].pMaterial;
                        AppMain.NNS_MATERIAL_COLOR pColor2 = pMaterial2.pColor;
                        AppMain.NNS_MATERIAL_COLOR pBackColor2 = pMaterial2.pBackColor;
                        if (pBackColor1 == pColor2)
                        {
                            if (dstmatptr != null)
                                nnsMaterialDesc.pBackColor = ((AppMain.NNS_MATERIAL_DESC)dstmatptr[index2].pMaterial).pColor;
                            num2 = 0;
                            break;
                        }
                        if (pBackColor1 == pBackColor2)
                        {
                            if (dstmatptr != null)
                                nnsMaterialDesc.pBackColor = ((AppMain.NNS_MATERIAL_DESC)dstmatptr[index2].pMaterial).pBackColor;
                            num2 = 0;
                            break;
                        }
                    }
                    if (num2 != 0 && dstmatptr != null)
                    {
                        nnsMaterialDesc.pBackColor = new AppMain.NNS_MATERIAL_COLOR();
                        nnsMaterialDesc.pBackColor.Assign(pBackColor1);
                    }
                }
                AppMain.NNS_MATERIAL_LOGIC pLogic1 = pMaterial1.pLogic;
                int num3 = 1;
                for (int index2 = 0; index2 < index1; ++index2)
                {
                    AppMain.NNS_MATERIAL_LOGIC pLogic2 = ((AppMain.NNS_MATERIAL_DESC)srcmatptr[index2].pMaterial).pLogic;
                    if (pLogic1 == pLogic2)
                    {
                        if (dstmatptr != null)
                            nnsMaterialDesc.pLogic = ((AppMain.NNS_MATERIAL_DESC)dstmatptr[index2].pMaterial).pLogic;
                        num3 = 0;
                        break;
                    }
                }
                if (num3 != 0 && dstmatptr != null)
                {
                    nnsMaterialDesc.pLogic = new AppMain.NNS_MATERIAL_LOGIC();
                    nnsMaterialDesc.pLogic.Assign(pLogic1);
                }
                if (pMaterial1.nTex > 0)
                {
                    if (dstmatptr != null)
                    {
                        nnsMaterialDesc.pTexDesc = new AppMain.NNS_MATERIAL_TEXMAP_DESC[pMaterial1.nTex];
                        int index2 = 0;
                        for (; index1 < pMaterial1.nTex; ++index1)
                            nnsMaterialDesc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_TEXMAP_DESC(pMaterial1.pTexDesc[index2]);
                    }
                    for (int index2 = 0; index2 < pMaterial1.nTex; ++index2)
                    {
                        AppMain.NNS_MATERIAL_TEXMAP_DESC materialTexmapDesc1 = pMaterial1.pTexDesc[index2];
                        AppMain.NNS_MATERIAL_TEXMAP_DESC materialTexmapDesc2 = (AppMain.NNS_MATERIAL_TEXMAP_DESC)null;
                        if (dstmatptr != null)
                            materialTexmapDesc2 = nnsMaterialDesc.pTexDesc[index2];
                        if (materialTexmapDesc1.pCombine != null && dstmatptr != null)
                        {
                            materialTexmapDesc2.pCombine = new AppMain.NNS_TEXTURE_COMBINE();
                            materialTexmapDesc2.pCombine.Assign(materialTexmapDesc1.pCombine);
                        }
                        if (materialTexmapDesc1.pBorderColor.HasValue && dstmatptr != null)
                        {
                            materialTexmapDesc2.pBorderColor = new AppMain.NNS_RGBA?(new AppMain.NNS_RGBA());
                            materialTexmapDesc2.pBorderColor = materialTexmapDesc1.pBorderColor;
                        }
                        if (materialTexmapDesc1.pFilterMode != null && dstmatptr != null)
                        {
                            materialTexmapDesc2.pFilterMode = new AppMain.NNS_TEXTURE_FILTERMODE();
                            materialTexmapDesc2.pFilterMode.Assign(materialTexmapDesc1.pFilterMode);
                        }
                        if (materialTexmapDesc1.pLODParam != null && dstmatptr != null)
                        {
                            materialTexmapDesc2.pLODParam = new AppMain.NNS_TEXTURE_LOD_PARAM();
                            materialTexmapDesc2.pLODParam.Assign(materialTexmapDesc1.pLODParam);
                        }
                    }
                }
            }
        }
        return 0;
    }

    private static uint nnCopyVertexList(
      AppMain.NNS_VTXLISTPTR[] dstvlist,
      AppMain.NNS_VTXLISTPTR[] srcvlist,
      int nVtxList,
      uint flag)
    {
        for (int index1 = 0; index1 < nVtxList; ++index1)
        {
            if (dstvlist != null)
                dstvlist[index1].fType = srcvlist[index1].fType;
            if (((int)srcvlist[index1].fType & 1) != 0)
            {
                AppMain.NNS_VTXLIST_GL_DESC pVtxList = (AppMain.NNS_VTXLIST_GL_DESC)srcvlist[index1].pVtxList;
                AppMain.NNS_VTXLIST_GL_DESC nnsVtxlistGlDesc = (AppMain.NNS_VTXLIST_GL_DESC)null;
                if (dstvlist != null)
                {
                    dstvlist[index1].pVtxList = (object)(nnsVtxlistGlDesc = new AppMain.NNS_VTXLIST_GL_DESC());
                    nnsVtxlistGlDesc.Assign(pVtxList);
                }
                if (dstvlist != null)
                {
                    nnsVtxlistGlDesc.pArray = new AppMain.NNS_VTXARRAY_GL[pVtxList.nArray];
                    for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
                        nnsVtxlistGlDesc.pArray[index2] = new AppMain.NNS_VTXARRAY_GL(pVtxList.pArray[index1]);
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
                        Array.Copy((Array)pVtxList.pVertexBuffer.Data, (Array)data, pVtxList.VertexBufferSize);
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
                    Array.Copy((Array)pVtxList.pVertexBuffer.Data, (Array)data, pVtxList.VertexBufferSize);
                    nnsVtxlistGlDesc.pVertexBuffer = ByteBuffer.Wrap(data);
                    for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
                        nnsVtxlistGlDesc.pArray[index2].Pointer = nnsVtxlistGlDesc.pVertexBuffer + pVtxList.pArray[index2].Pointer.Offset;
                }
                if (dstvlist != null && pVtxList.nMatrix != 0)
                {
                    nnsVtxlistGlDesc.pMatrixIndices = new ushort[pVtxList.nMatrix];
                    Array.Copy((Array)pVtxList.pMatrixIndices, (Array)nnsVtxlistGlDesc.pMatrixIndices, pVtxList.nMatrix);
                }
            }
            else if (((int)srcvlist[index1].fType & 16711680) != 0)
            {
                AppMain.NNS_VTXLIST_COMMON_DESC pVtxList = (AppMain.NNS_VTXLIST_COMMON_DESC)srcvlist[index1].pVtxList;
                AppMain.NNS_VTXLIST_COMMON_DESC vtxlistCommonDesc = (AppMain.NNS_VTXLIST_COMMON_DESC)null;
                if (dstvlist != null)
                {
                    dstvlist[index1].pVtxList = (object)(vtxlistCommonDesc = new AppMain.NNS_VTXLIST_COMMON_DESC());
                    vtxlistCommonDesc.Assign(pVtxList);
                }
                for (int index2 = 0; index2 < 4; ++index2)
                {
                    AppMain.NNS_VTXLIST_COMMON_ARRAY vtxlistCommonArray = (AppMain.NNS_VTXLIST_COMMON_ARRAY)null;
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
                                AppMain.NNS_VTXLIST_COMMON_ARRAY list0 = vtxlistCommonDesc.List0;
                                break;
                            case 1:
                                AppMain.NNS_VTXLIST_COMMON_ARRAY list1 = vtxlistCommonDesc.List1;
                                break;
                            case 2:
                                AppMain.NNS_VTXLIST_COMMON_ARRAY list2 = vtxlistCommonDesc.List2;
                                break;
                            case 3:
                                AppMain.NNS_VTXLIST_COMMON_ARRAY list3 = vtxlistCommonDesc.List3;
                                break;
                        }
                        AppMain.mppAssertNotImpl();
                    }
                }
            }
            else
                AppMain.NNM_ASSERT(0, "Unknown vertex foramt.\n");
        }
        return 0;
    }

    private static uint nnCopyPrimitiveList(
      AppMain.NNS_PRIMLISTPTR[] dstplist,
      AppMain.NNS_PRIMLISTPTR[] srcplist,
      int nPrimList,
      uint flag)
    {
        for (int index1 = 0; index1 < nPrimList; ++index1)
        {
            if (dstplist != null)
                dstplist[index1].fType = srcplist[index1].fType;
            if (((int)srcplist[index1].fType & 1) != 0)
            {
                AppMain.NNS_PRIMLIST_GL_DESC pPrimList = (AppMain.NNS_PRIMLIST_GL_DESC)srcplist[index1].pPrimList;
                AppMain.NNS_PRIMLIST_GL_DESC nnsPrimlistGlDesc = (AppMain.NNS_PRIMLIST_GL_DESC)null;
                if (dstplist != null)
                {
                    dstplist[index1].pPrimList = (object)(nnsPrimlistGlDesc = new AppMain.NNS_PRIMLIST_GL_DESC());
                    nnsPrimlistGlDesc.Assign(pPrimList);
                }
                if (dstplist != null)
                {
                    nnsPrimlistGlDesc.pCounts = new int[pPrimList.nPrim];
                    Array.Copy((Array)pPrimList.pCounts, (Array)nnsPrimlistGlDesc.pCounts, pPrimList.nPrim);
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
                    Array.Copy((Array)pPrimList.pIndexBuffer.Data, (Array)data, pPrimList.IndexBufferSize);
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
                        AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_STRIP pPrimList1 = (AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_STRIP)srcplist[index1].pPrimList;
                        AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_STRIP commonTriangleStrip = (AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_STRIP)null;
                        if (dstplist != null)
                        {
                            dstplist[index1].pPrimList = (object)(commonTriangleStrip = new AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_STRIP());
                            commonTriangleStrip.Assign(pPrimList1);
                        }
                        int nStrip = pPrimList1.nStrip;
                        if (dstplist != null)
                        {
                            commonTriangleStrip.pLengthList = new ushort[pPrimList1.nStrip];
                            Array.Copy((Array)pPrimList1.pLengthList, (Array)commonTriangleStrip.pLengthList, pPrimList1.nStrip);
                        }
                        int num = 0;
                        for (int index2 = 0; index2 < nStrip; ++index2)
                            num += (int)pPrimList1.pLengthList[index2];
                        if (dstplist != null)
                        {
                            commonTriangleStrip.pStripList = new ushort[pPrimList1.nIndexSetSize * num];
                            Array.Copy((Array)pPrimList1.pStripList, (Array)commonTriangleStrip.pStripList, pPrimList1.nIndexSetSize * num);
                            continue;
                        }
                        continue;
                    case 262144:
                        AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_LIST pPrimList2 = (AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_LIST)srcplist[index1].pPrimList;
                        AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_LIST commonTriangleList = (AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_LIST)null;
                        if (dstplist != null)
                        {
                            dstplist[index1].pPrimList = (object)(commonTriangleList = new AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_LIST());
                            commonTriangleList.Assign(pPrimList2);
                        }
                        if (dstplist != null)
                        {
                            commonTriangleList.pTriangleList = new ushort[pPrimList2.nIndexSetSize * pPrimList2.nTriangle];
                            Array.Copy((Array)pPrimList2.pTriangleList, (Array)commonTriangleList.pTriangleList, pPrimList2.nIndexSetSize * pPrimList2.nTriangle);
                            continue;
                        }
                        continue;
                    case 524288:
                        AppMain.NNS_PRIMLIST_COMMON_QUAD_LIST pPrimList3 = (AppMain.NNS_PRIMLIST_COMMON_QUAD_LIST)srcplist[index1].pPrimList;
                        AppMain.NNS_PRIMLIST_COMMON_QUAD_LIST primlistCommonQuadList = (AppMain.NNS_PRIMLIST_COMMON_QUAD_LIST)null;
                        if (dstplist != null)
                        {
                            dstplist[index1].pPrimList = (object)(primlistCommonQuadList = new AppMain.NNS_PRIMLIST_COMMON_QUAD_LIST());
                            primlistCommonQuadList.Assign(pPrimList3);
                        }
                        if (dstplist != null)
                        {
                            primlistCommonQuadList.pQuadList = new ushort[pPrimList3.nIndexSetSize * pPrimList3.nQuad];
                            Array.Copy((Array)pPrimList3.pQuadList, (Array)primlistCommonQuadList.pQuadList, pPrimList3.nIndexSetSize * pPrimList3.nQuad);
                            continue;
                        }
                        continue;
                    case 1048576:
                        AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST pPrimList4 = (AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST)srcplist[index1].pPrimList;
                        AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST triangleQuadList = (AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST)null;
                        if (dstplist != null)
                        {
                            dstplist[index1].pPrimList = (object)(triangleQuadList = new AppMain.NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST());
                            triangleQuadList.Assign(pPrimList4);
                        }
                        if (dstplist != null)
                        {
                            triangleQuadList.pTriangleList = new ushort[pPrimList4.nIndexSetSize * pPrimList4.nTriangle];
                            Array.Copy((Array)pPrimList4.pTriangleList, (Array)triangleQuadList.pTriangleList, pPrimList4.nIndexSetSize * pPrimList4.nTriangle);
                        }
                        if (dstplist != null)
                        {
                            triangleQuadList.pQuadList = new ushort[pPrimList4.nIndexSetSize * pPrimList4.nQuad];
                            Array.Copy((Array)pPrimList4.pQuadList, (Array)triangleQuadList.pQuadList, pPrimList4.nIndexSetSize * pPrimList4.nQuad);
                            continue;
                        }
                        continue;
                    default:
                        continue;
                }
            }
            else
                AppMain.NNM_ASSERT(0, "Unknown primitive foramt.\n");
        }
        return 0;
    }

    private static uint nnCopySubobjList(
      AppMain.NNS_SUBOBJ[] pSubobjListDst,
      AppMain.NNS_SUBOBJ[] pSubobjListSrc,
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
            AppMain.NNS_SUBOBJ nnsSubobj1 = pSubobjListSrc[index1];
            AppMain.NNS_SUBOBJ nnsSubobj2 = (AppMain.NNS_SUBOBJ)null;
            if (pSubobjListDst != null)
            {
                nnsSubobj2 = pSubobjListDst[index1];
                nnsSubobj2.pMeshsetList = new AppMain.NNS_MESHSET[nnsSubobj1.nMeshset];
                for (int index2 = 0; index2 < nnsSubobj1.nMeshset; ++index2)
                    nnsSubobj2.pMeshsetList[index2] = new AppMain.NNS_MESHSET(nnsSubobj1.pMeshsetList[index2]);
            }
            if (pSubobjListDst != null && nnsSubobj1.nTex != 0)
            {
                nnsSubobj2.pTexNumList = new int[nnsSubobj1.nTex];
                Array.Copy((Array)nnsSubobj1.pTexNumList, (Array)nnsSubobj2.pTexNumList, nnsSubobj1.nTex);
            }
        }
        return 0;
    }

    private static uint nnCopyObject(AppMain.NNS_OBJECT dstobj, AppMain.NNS_OBJECT srcobj, uint flag)
    {
        if (dstobj != null)
        {
            dstobj.Assign(srcobj);
            dstobj.fType |= 128U;
        }
        if (dstobj != null)
        {
            dstobj.pMatPtrList = AppMain.New<AppMain.NNS_MATERIALPTR>(srcobj.pMatPtrList.Length);
            int num = (int)AppMain.nnCopyMaterialList(dstobj.pMatPtrList, srcobj.pMatPtrList, srcobj.nMaterial, flag);
        }
        else
        {
            int num1 = (int)AppMain.nnCopyMaterialList((AppMain.NNS_MATERIALPTR[])null, srcobj.pMatPtrList, srcobj.nMaterial, flag);
        }
        if (dstobj != null)
        {
            dstobj.pVtxListPtrList = AppMain.New<AppMain.NNS_VTXLISTPTR>(srcobj.pVtxListPtrList.Length);
            int num2 = (int)AppMain.nnCopyVertexList(dstobj.pVtxListPtrList, srcobj.pVtxListPtrList, srcobj.nVtxList, flag);
        }
        else
        {
            int num3 = (int)AppMain.nnCopyVertexList((AppMain.NNS_VTXLISTPTR[])null, srcobj.pVtxListPtrList, srcobj.nVtxList, flag);
        }
        if (dstobj != null)
        {
            dstobj.pPrimListPtrList = AppMain.New<AppMain.NNS_PRIMLISTPTR>(srcobj.pPrimListPtrList.Length);
            int num2 = (int)AppMain.nnCopyPrimitiveList(dstobj.pPrimListPtrList, srcobj.pPrimListPtrList, srcobj.nPrimList, flag);
        }
        else
        {
            int num4 = (int)AppMain.nnCopyPrimitiveList((AppMain.NNS_PRIMLISTPTR[])null, srcobj.pPrimListPtrList, srcobj.nPrimList, flag);
        }
        if (dstobj != null)
        {
            dstobj.pNodeList = AppMain.New<AppMain.NNS_NODE>(srcobj.nNode);
            for (int index = 0; index < srcobj.nNode; ++index)
                dstobj.pNodeList[index].Assign(srcobj.pNodeList[index]);
        }
        if (dstobj != null)
        {
            dstobj.pSubobjList = AppMain.New<AppMain.NNS_SUBOBJ>(srcobj.pSubobjList.Length);
            int num2 = (int)AppMain.nnCopySubobjList(dstobj.pSubobjList, srcobj.pSubobjList, srcobj.nSubobj, flag);
        }
        else
        {
            int num5 = (int)AppMain.nnCopySubobjList((AppMain.NNS_SUBOBJ[])null, srcobj.pSubobjList, srcobj.nSubobj, flag);
        }
        return 0;
    }

    private uint nnCalcMorphObjectBufferSize(
      ref AppMain.NNS_OBJECT obj,
      ref AppMain.NNS_MORPHTARGETLIST mtgt,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private uint nnInitMorphObject(
      ref AppMain.NNS_OBJECT mobj,
      ref AppMain.NNS_OBJECT obj,
      ref AppMain.NNS_MORPHTARGETLIST mtgt,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private void nnDrawMorphObject(
      ref AppMain.NNS_OBJECT mobj,
      ref AppMain.NNS_MATRIX mtxpal,
      ref uint nodestatlist,
      uint subobjtype,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnMakeUnitMatrix(ref AppMain.SNNS_MATRIX dst)
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

    private static void nnMakeUnitMatrix(AppMain.NNS_MATRIX dst)
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
      out AppMain.SNNS_MATRIX dst,
      ref AppMain.NNS_QUATERNION quat)
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
        dst.M01 = (float)(2.0 * ((double)num5 - (double)num10));
        dst.M02 = (float)(2.0 * ((double)num7 + (double)num9));
        dst.M10 = (float)(2.0 * ((double)num5 + (double)num10));
        dst.M11 = num11 + num12;
        dst.M12 = (float)(2.0 * ((double)num6 - (double)num8));
        dst.M20 = (float)(2.0 * ((double)num7 - (double)num9));
        dst.M21 = (float)(2.0 * ((double)num6 + (double)num8));
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
      AppMain.NNS_MATRIX dst,
      ref AppMain.NNS_QUATERNION quat)
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
        dst.M01 = (float)(2.0 * ((double)num5 - (double)num10));
        dst.M02 = (float)(2.0 * ((double)num7 + (double)num9));
        dst.M10 = (float)(2.0 * ((double)num5 + (double)num10));
        dst.M11 = num11 + num12;
        dst.M12 = (float)(2.0 * ((double)num6 - (double)num8));
        dst.M20 = (float)(2.0 * ((double)num7 - (double)num9));
        dst.M21 = (float)(2.0 * ((double)num6 + (double)num8));
        dst.M22 = num11 - num12;
        dst.M03 = 0.0f;
        dst.M13 = 0.0f;
        dst.M23 = 0.0f;
        dst.M30 = 0.0f;
        dst.M31 = 0.0f;
        dst.M32 = 0.0f;
        dst.M33 = 1f;
    }

    public static void nnMakeRotateXMatrix(AppMain.NNS_MATRIX dst, int ax)
    {
        float s;
        float c;
        AppMain.nnSinCos(ax, out s, out c);
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

    public static void nnMakeRotateXMatrix(out AppMain.SNNS_MATRIX dst, int ax)
    {
        float s;
        float c;
        AppMain.nnSinCos(ax, out s, out c);
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

    public static void nnMakeRotateYMatrix(AppMain.NNS_MATRIX dst, int ay)
    {
        float s;
        float c;
        AppMain.nnSinCos(ay, out s, out c);
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

    public static void nnMakeRotateZMatrix(out AppMain.SNNS_MATRIX dst, int az)
    {
        float s;
        float c;
        AppMain.nnSinCos(az, out s, out c);
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

    public static void nnMakeRotateZMatrix(AppMain.NNS_MATRIX dst, int az)
    {
        float s;
        float c;
        AppMain.nnSinCos(az, out s, out c);
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

    public static void nnMakeRotateXYZMatrix(AppMain.NNS_MATRIX dst, int ax, int ay, int az)
    {
        AppMain.nnMakeRotateZMatrix(dst, az);
        AppMain.nnRotateYMatrix(dst, dst, ay);
        AppMain.nnRotateXMatrix(dst, dst, ax);
    }

    public static void nnMakeRotateXZYMatrix(AppMain.NNS_MATRIX dst, int ax, int ay, int az)
    {
        AppMain.nnMakeRotateYMatrix(dst, ay);
        AppMain.nnRotateZMatrix(dst, dst, az);
        AppMain.nnRotateXMatrix(dst, dst, ax);
    }

    public static void nnMakeRotateZXYMatrix(AppMain.NNS_MATRIX dst, int ax, int ay, int az)
    {
        AppMain.nnMakeRotateYMatrix(dst, ay);
        AppMain.nnRotateXMatrix(dst, dst, ax);
        AppMain.nnRotateZMatrix(dst, dst, az);
    }

    public static void nnMakeScaleMatrix(out AppMain.SNNS_MATRIX dst, float x, float y, float z)
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

    public static void nnMakeScaleMatrix(AppMain.NNS_MATRIX dst, float x, float y, float z)
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
      out AppMain.SNNS_MATRIX dst,
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

    private static void nnMakeTranslateMatrix(AppMain.NNS_MATRIX dst, float x, float y, float z)
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
      AppMain.NNS_MATRIX mtx,
      int fovy,
      float aspect,
      float znear,
      float zfar)
    {
        double num = 1.0 / (double)AppMain.nnTan(fovy >> 1);
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
        mtx.M22 = (float)(-(double)znear / ((double)zfar - (double)znear));
        mtx.M23 = (float)(-((double)zfar * (double)znear) / ((double)zfar - (double)znear));
        mtx.M30 = 0.0f;
        mtx.M31 = 0.0f;
        mtx.M32 = -1f;
        mtx.M33 = 0.0f;
    }

    public static void nnMakePerspectiveOffCenterMatrix(
      AppMain.NNS_MATRIX mtx,
      float left,
      float right,
      float bottom,
      float top,
      float znear,
      float zfar)
    {
        mtx.M00 = (float)(2.0 * (double)znear / ((double)right - (double)left));
        mtx.M01 = 0.0f;
        mtx.M02 = (float)(((double)right + (double)left) / ((double)right - (double)left));
        mtx.M03 = 0.0f;
        mtx.M10 = 0.0f;
        mtx.M11 = (float)(2.0 * (double)znear / ((double)top - (double)bottom));
        mtx.M12 = (float)(((double)top + (double)bottom) / ((double)top - (double)bottom));
        mtx.M13 = 0.0f;
        mtx.M20 = 0.0f;
        mtx.M21 = 0.0f;
        mtx.M22 = (float)(-(double)znear / ((double)zfar - (double)znear));
        mtx.M23 = (float)(-((double)zfar * (double)znear) / ((double)zfar - (double)znear));
        mtx.M30 = 0.0f;
        mtx.M31 = 0.0f;
        mtx.M32 = -1f;
        mtx.M33 = 0.0f;
    }

    public static void nnMakeOrthoMatrix(
      AppMain.NNS_MATRIX mtx,
      float left,
      float right,
      float bottom,
      float top,
      float znear,
      float zfar)
    {
        mtx.M00 = (float)(2.0 / ((double)right - (double)left));
        mtx.M01 = 0.0f;
        mtx.M02 = 0.0f;
        mtx.M03 = (float)(-((double)right + (double)left) / ((double)right - (double)left));
        mtx.M10 = 0.0f;
        mtx.M11 = (float)(2.0 / ((double)top - (double)bottom));
        mtx.M12 = 0.0f;
        mtx.M13 = (float)(-((double)top + (double)bottom) / ((double)top - (double)bottom));
        mtx.M20 = 0.0f;
        mtx.M21 = 0.0f;
        mtx.M22 = (float)(-1.0 / ((double)zfar - (double)znear));
        mtx.M23 = (float)(-(double)zfar / ((double)zfar - (double)znear));
        mtx.M30 = 0.0f;
        mtx.M31 = 0.0f;
        mtx.M32 = 0.0f;
        mtx.M33 = 1f;
    }
    private static void nnCalcMatrixPaletteMotion(
     AppMain.NNS_MATRIX[] mtxpal,
     uint[] nodestatlist,
     AppMain.NNS_OBJECT obj,
     AppMain.NNS_MOTION mot,
     float frame,
     AppMain.NNS_MATRIX basemtx,
     AppMain.NNS_MATRIXSTACK mstk,
     uint flag)
    {
        if (((int)mot.fType & 1) == 0)
            return;
        AppMain.nncalcmatrixpalettemotion.nnsSubMotIdx = 0;
        float dstframe;
        int num = AppMain.nnCalcMotionFrame(out dstframe, mot.fType, mot.StartFrame, mot.EndFrame, frame);
        AppMain.nncalcmatrixpalettemotion.nnsMtxPal = mtxpal;
        AppMain.nncalcmatrixpalettemotion.nnsNodeStatList = nodestatlist;
        AppMain.nncalcmatrixpalettemotion.nnsNSFlag = flag;
        AppMain.nncalcmatrixpalettemotion.nnsObj = obj;
        AppMain.nncalcmatrixpalettemotion.nnsNodeList = obj.pNodeList;
        AppMain.nncalcmatrixpalettemotion.nnsMstk = mstk;
        AppMain.nncalcmatrixpalettemotion.nnsMot = mot;
        AppMain.nncalcmatrixpalettemotion.nnsFrame = dstframe;
        if (num != 0)
        {
            AppMain.nncalcmatrixpalettemotion.nnsBaseMtx = basemtx == null ? AppMain.nngUnitMatrix : basemtx;
            AppMain.nnSetCurrentMatrix(mstk, AppMain.nncalcmatrixpalettemotion.nnsBaseMtx);
            AppMain.nnCalcMatrixPaletteMotionNode(0);
        }
        else
            AppMain.nnCalcMatrixPalette(mtxpal, nodestatlist, obj, basemtx, mstk, AppMain.nncalcmatrixpalettemotion.nnsNSFlag);
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
                if ((double)startframe <= (double)frame && (double)frame < (double)endframe)
                {
                    dstframe = frame;
                    return 1;
                }
                dstframe = frame;
                return 0;
            case 131072:
                dstframe = (double)startframe <= (double)frame ? ((double)endframe > (double)frame ? frame : endframe) : startframe;
                return 1;
            case 262144:
                if ((double)startframe <= (double)frame && (double)frame < (double)endframe)
                {
                    dstframe = frame;
                }
                else
                {
                    float num1 = frame - startframe;
                    float num2 = endframe - startframe;
                    float num3 = num1 / num2;
                    int num4 = (int)num3;
                    if ((double)num3 < 0.0)
                        --num4;
                    float num5 = (float)num4;
                    dstframe = frame - num2 * num5;
                }
                return 1;
            case 524288:
                float num6 = frame - startframe;
                float num7 = endframe - startframe;
                float num8 = num6 / num7;
                int num9 = (int)num8;
                if ((double)num8 < 0.0)
                    --num9;
                float num10 = (float)num9;
                dstframe = (num9 & 1) == 0 ? frame - num7 * num10 : (float)((double)startframe - (double)num6 + (double)num7 * ((double)num10 + 1.0));
                return 1;
            default:
                dstframe = frame;
                return 0;
        }
    }

    private static void nnCalcMatrixPaletteMotionNode(int nodeIdx)
    {
        int? pHideFlag = new int?(0);
        AppMain.NNS_NODE nnsNode;
        do
        {
            nnsNode = AppMain.nncalcmatrixpalettemotion.nnsNodeList[nodeIdx];
            if (((int)nnsNode.fType & 134217728) != 0)
            {
                if (((int)nnsNode.fType & 100663296) != 0)
                {
                    AppMain.nnPushMatrix(AppMain.nncalcmatrixpalettemotion.nnsMstk, (AppMain.NNS_MATRIX)null);
                    if (((int)nnsNode.fType & 33554432) != 0)
                        AppMain.nnCalcMatrixPaletteMotionNode1BoneXSIIK(nodeIdx);
                    else if (((int)nnsNode.fType & 67108864) != 0)
                        AppMain.nnCalcMatrixPaletteMotionNode2BoneXSIIK(nodeIdx);
                    AppMain.nnPopMatrix(AppMain.nncalcmatrixpalettemotion.nnsMstk);
                    nodeIdx = (int)nnsNode.iSibling;
                    goto label_25;
                }
            }
            else
            {
                if (((int)nnsNode.fType & 16384) != 0)
                {
                    AppMain.nnPushMatrix(AppMain.nncalcmatrixpalettemotion.nnsMstk, (AppMain.NNS_MATRIX)null);
                    AppMain.nnCalcMatrixPaletteMotionNode1BoneSIIK(nodeIdx);
                    AppMain.nnPopMatrix(AppMain.nncalcmatrixpalettemotion.nnsMstk);
                    break;
                }
                if (((int)nnsNode.fType & 32768) != 0)
                {
                    AppMain.nnPushMatrix(AppMain.nncalcmatrixpalettemotion.nnsMstk, (AppMain.NNS_MATRIX)null);
                    AppMain.nnCalcMatrixPaletteMotionNode2BoneSIIK(nodeIdx);
                    AppMain.nnPopMatrix(AppMain.nncalcmatrixpalettemotion.nnsMstk);
                    break;
                }
            }
            AppMain.nnPushMatrix(AppMain.nncalcmatrixpalettemotion.nnsMstk, (AppMain.NNS_MATRIX)null);
            AppMain.NNS_MATRIX currentMatrix = AppMain.nnGetCurrentMatrix(AppMain.nncalcmatrixpalettemotion.nnsMstk);
            AppMain.nncalcmatrixpalettemotion.nnsSubMotIdx = AppMain.nnCalcNodeMotionCore(currentMatrix, ref pHideFlag, AppMain.nncalcmatrixpalettemotion.nnsBaseMtx, nnsNode, nodeIdx, AppMain.nncalcmatrixpalettemotion.nnsObj, AppMain.nncalcmatrixpalettemotion.nnsMot, AppMain.nncalcmatrixpalettemotion.nnsSubMotIdx, AppMain.nncalcmatrixpalettemotion.nnsFrame);
            if (nnsNode.iMatrix != (short)-1)
            {
                if (((int)nnsNode.fType & 8) != 0)
                    AppMain.nnCopyMatrix(AppMain.nncalcmatrixpalettemotion.nnsMtxPal[(int)nnsNode.iMatrix], currentMatrix);
                else
                    AppMain.nnMultiplyMatrix(AppMain.nncalcmatrixpalettemotion.nnsMtxPal[(int)nnsNode.iMatrix], currentMatrix, nnsNode.InvInitMtx);
            }
            if (AppMain.nncalcmatrixpalettemotion.nnsNodeStatList != null)
            {
                if (nodeIdx == 0 && AppMain.nncalcmatrixpalettemotion.nnsNSFlag != 0U)
                    AppMain.nncalcmatrixpalettemotion.nnsRootScale = AppMain.nnEstimateMatrixScaling(currentMatrix);
                int? nullable = pHideFlag;
                if ((nullable.GetValueOrDefault() != 0 ? 1 : (!nullable.HasValue ? 1 : 0)) != 0)
                    AppMain.nncalcmatrixpalettemotion.nnsNodeStatList[nodeIdx] |= 1U;
                AppMain.nnCalcClipSetNodeStatus(AppMain.nncalcmatrixpalettemotion.nnsNodeStatList, AppMain.nncalcmatrixpalettemotion.nnsNodeList, nodeIdx, currentMatrix, AppMain.nncalcmatrixpalettemotion.nnsRootScale, AppMain.nncalcmatrixpalettemotion.nnsNSFlag);
            }
            if (nnsNode.iChild != (short)-1)
                AppMain.nnCalcMatrixPaletteMotionNode((int)nnsNode.iChild);
            AppMain.nnPopMatrix(AppMain.nncalcmatrixpalettemotion.nnsMstk);
            nodeIdx = (int)nnsNode.iSibling;
        label_25:;
        }
        while (nnsNode.iSibling != (short)-1);
    }

    private static int nnCalcNodeMotionCore(
      AppMain.NNS_MATRIX pNodeMtx,
      ref int? pHideFlag,
      AppMain.NNS_MATRIX pBaseMtx,
      AppMain.NNS_NODE pNode,
      int NodeIdx,
      AppMain.NNS_OBJECT pObj,
      AppMain.NNS_MOTION pMot,
      int SubMotIdx,
      float frame)
    {
        AppMain.NNS_VECTOR tv = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_ROTATE_A32 rv = new AppMain.NNS_ROTATE_A32();
        AppMain.NNS_QUATERNION quat = new AppMain.NNS_QUATERNION();
        AppMain.NNS_VECTOR sv = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
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
            AppMain.NNS_SUBMOTION submot = pMot.pSubmotion[SubMotIdx1];
            if (NodeIdx < submot.Id)
            {
                SubMotIdx = SubMotIdx1;
                break;
            }
            if (NodeIdx == submot.Id && submot.fType != 0U && ((double)submot.StartFrame <= (double)frame && (double)frame <= (double)submot.EndFrame))
            {
                uint fType2 = submot.fIPType;
                if (((int)pMot.fType & 131072) != 0 && (double)frame == (double)submot.EndFrame)
                    fType2 = 131072U;
                float dstframe;
                if (AppMain.nnCalcMotionFrame(out dstframe, fType2, submot.StartKeyFrame, submot.EndKeyFrame, frame) != 0)
                {
                    if (((int)submot.fType & 30720) != 0)
                        num2 |= AppMain.nnCalcMotionRotate(submot, dstframe, ref rv, quat, rtype);
                    else if (((int)submot.fType & 1792) != 0)
                        num1 |= AppMain.nnCalcMotionTranslate(submot, dstframe, tv);
                    else if (((int)submot.fType & 229376) != 0)
                        num3 |= AppMain.nnCalcMotionScale(submot, dstframe, sv);
                    else if (((int)submot.fType & 786432) != 0)
                        AppMain.nnCallbackMotionUserData(pObj, pMot, SubMotIdx1, NodeIdx, dstframe, frame);
                    else if (((int)submot.fType & 1048576) != 0 && pHideFlag.HasValue)
                        pHideFlag = new int?(AppMain.nnCalcMotionNodeHide(submot, dstframe));
                }
            }
        }
        if (num1 == 1)
        {
            if (((int)pNode.fType & 8192) != 0)
            {
                AppMain.NNS_VECTORFAST dst = new AppMain.NNS_VECTORFAST();
                AppMain.nnmSetUpVectorFast(out dst, tv.x, tv.y, tv.z);
                AppMain.nnTransformVectorFast(out dst, pBaseMtx, dst);
                AppMain.nnCopyVectorFastMatrixTranslation(pNodeMtx, ref dst);
            }
            else
                AppMain.nnTranslateMatrixFast(pNodeMtx, tv.x, tv.y, tv.z);
        }
        if (((int)pNode.fType & 1839104) != 0)
        {
            if (((int)pNode.fType & 4096) != 0)
                AppMain.nnCopyMatrix33(pNodeMtx, pBaseMtx);
            if (((int)pNode.fType & 262144) != 0)
                AppMain.nnNormalizeColumn0(pNodeMtx);
            if (((int)pNode.fType & 524288) != 0)
                AppMain.nnNormalizeColumn1(pNodeMtx);
            if (((int)pNode.fType & 1048576) != 0)
                AppMain.nnNormalizeColumn2(pNodeMtx);
        }
        if (num2 == 1)
        {
            switch (rtype)
            {
                case 0:
                    AppMain.nnRotateXYZMatrix(pNodeMtx, pNodeMtx, rv.x, rv.y, rv.z);
                    break;
                case 256:
                    AppMain.nnRotateXZYMatrixFast(pNodeMtx, rv.x, rv.y, rv.z);
                    break;
                case 1024:
                    AppMain.nnRotateZXYMatrixFast(pNodeMtx, rv.x, rv.y, rv.z);
                    break;
                default:
                    AppMain.nnRotateXYZMatrix(pNodeMtx, pNodeMtx, rv.x, rv.y, rv.z);
                    break;
            }
        }
        else if ((num2 & 2) != 0)
            AppMain.nnQuaternionMatrix(pNodeMtx, pNodeMtx, ref quat);
        if (num3 == 1)
            AppMain.nnScaleMatrixFast(pNodeMtx, sv.x, sv.y, sv.z);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(tv);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(sv);
        return SubMotIdx;
    }

    private static int nnCalcMotionRotate(
      AppMain.NNS_SUBMOTION submot,
      float frame,
      ref AppMain.NNS_ROTATE_A32 rv,
      AppMain.NNS_QUATERNION rq,
      uint rtype)
    {
        int num1 = 0;
        AppMain.NNS_ROTATE_A16 val = new AppMain.NNS_ROTATE_A16();
        int[] arv = AppMain._nnCalcMotionRotate.arv;
        arv[0] = rv.x;
        arv[1] = rv.y;
        arv[2] = rv.z;
        short[] arsv = AppMain._nnCalcMotionRotate.arsv;
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
                if (num3 == 16U || pKeyList is AppMain.NNS_MOTION_KEY_Class16[])
                {
                    switch (submot.fIPType & 3703U)
                    {
                        case 2:
                            AppMain.nnInterpolateLinearA16_3((AppMain.NNS_MOTION_KEY_Class16[])pKeyList, nKeyFrame, frame, ref val);
                            rv.x = (int)val.x;
                            rv.y = (int)val.y;
                            rv.z = (int)val.z;
                            num1 = 1;
                            break;
                        case 4:
                            AppMain.nnInterpolateConstantA16_3((AppMain.NNS_MOTION_KEY_Class16[])pKeyList, nKeyFrame, frame, ref val);
                            rv.x = (int)val.x;
                            rv.y = (int)val.y;
                            rv.z = (int)val.z;
                            num1 = 1;
                            break;
                        case 512:
                            AppMain.nnInterpolateLerpA16_3((AppMain.NNS_MOTION_KEY_Class16[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                        case 1024:
                            AppMain.nnInterpolateSlerpA16_3((AppMain.NNS_MOTION_KEY_Class16[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                        case 2048:
                            AppMain.nnInterpolateSquadA16_3((AppMain.NNS_MOTION_KEY_Class16[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                    }
                }
                else
                {
                    switch (submot.fIPType & 3703U)
                    {
                        case 2:
                            AppMain.nnInterpolateLinearA32_3((AppMain.NNS_MOTION_KEY_Class13[])pKeyList, nKeyFrame, frame, ref rv);
                            num1 = 1;
                            break;
                        case 4:
                            AppMain.nnInterpolateConstantA32_3((AppMain.NNS_MOTION_KEY_Class13[])pKeyList, nKeyFrame, frame, ref rv);
                            num1 = 1;
                            break;
                        case 512:
                            AppMain.nnInterpolateLerpA32_3((AppMain.NNS_MOTION_KEY_Class13[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                        case 1024:
                            AppMain.nnInterpolateSlerpA32_3((AppMain.NNS_MOTION_KEY_Class13[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                        case 2048:
                            AppMain.nnInterpolateSquadA32_3((AppMain.NNS_MOTION_KEY_Class13[])pKeyList, nKeyFrame, frame, ref rq, rtype);
                            num1 = 2;
                            break;
                    }
                }
                break;
            case 16384:
                switch (submot.fIPType & 3703U)
                {
                    case 4:
                        AppMain.nnInterpolateConstantQuat_4((AppMain.NNS_MOTION_KEY_Class7[])pKeyList, nKeyFrame, frame, ref rq);
                        num1 = 2;
                        break;
                    case 512:
                        AppMain.nnInterpolateLerpQuat_4((AppMain.NNS_MOTION_KEY_Class7[])pKeyList, nKeyFrame, frame, ref rq);
                        num1 = 2;
                        break;
                    case 1024:
                        AppMain.nnInterpolateSlerpQuat_4((AppMain.NNS_MOTION_KEY_Class7[])pKeyList, nKeyFrame, frame, ref rq);
                        num1 = 2;
                        break;
                    case 2048:
                        AppMain.nnInterpolateSquadQuat_4((AppMain.NNS_MOTION_KEY_Class7[])pKeyList, nKeyFrame, frame, ref rq);
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
                                AppMain.nnInterpolateLinearA16_1((AppMain.NNS_MOTION_KEY_Class14[])pKeyList, nKeyFrame, frame, out arsv[index]);
                                arv[index] = (int)arsv[index];
                                break;
                            case 4:
                                AppMain.nnInterpolateConstantA16_1((AppMain.NNS_MOTION_KEY_Class14[])pKeyList, nKeyFrame, frame, out arsv[index]);
                                arv[index] = (int)arsv[index];
                                break;
                            case 32:
                                AppMain.nnInterpolateSISplineA16_1((AppMain.NNS_MOTION_KEY_Class15[])pKeyList, nKeyFrame, frame, out arsv[index]);
                                arv[index] = (int)arsv[index];
                                break;
                        }
                    }
                    else
                    {
                        switch (submot.fIPType & 3703U)
                        {
                            case 2:
                                AppMain.nnInterpolateLinearA32_1((AppMain.NNS_MOTION_KEY_Class8[])pKeyList, nKeyFrame, frame, out arv[index]);
                                num1 = 1;
                                break;
                            case 4:
                                AppMain.nnInterpolateConstantA32_1((AppMain.NNS_MOTION_KEY_Class8[])pKeyList, nKeyFrame, frame, out arv[index]);
                                num1 = 1;
                                break;
                            case 16:
                                AppMain.nnInterpolateBezierA32_1((AppMain.NNS_MOTION_KEY_Class9[])pKeyList, nKeyFrame, frame, out arv[index]);
                                num1 = 1;
                                break;
                            case 32:
                                AppMain.nnInterpolateSISplineA32_1((AppMain.NNS_MOTION_KEY_Class10[])pKeyList, nKeyFrame, frame, out arv[index]);
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

    private static void nnRotateXYZMatrixFast(AppMain.NNS_MATRIX mtx, int ax, int ay, int az)
    {
        AppMain.nnmRotateMatrixFast(mtx, az, 0, 1);
        AppMain.nnmRotateMatrixFast(mtx, ay, 2, 0);
        AppMain.nnmRotateMatrixFast(mtx, ax, 1, 2);
    }

    private static void nnRotateXZYMatrixFast(AppMain.NNS_MATRIX mtx, int ax, int ay, int az)
    {
        AppMain.nnmRotateMatrixFast(mtx, ay, 2, 0);
        AppMain.nnmRotateMatrixFast(mtx, az, 0, 1);
        AppMain.nnmRotateMatrixFast(mtx, ax, 1, 2);
    }

    private static void nnRotateZXYMatrixFast(AppMain.NNS_MATRIX mtx, int ax, int ay, int az)
    {
        AppMain.nnmRotateMatrixFast(mtx, ay, 2, 0);
        AppMain.nnmRotateMatrixFast(mtx, ax, 1, 2);
        AppMain.nnmRotateMatrixFast(mtx, az, 0, 1);
    }

    private static int nnCalcMotionTranslate(
      AppMain.NNS_SUBMOTION submot,
      float frame,
      AppMain.NNS_VECTOR tv)
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
                    AppMain.nnInterpolateLinearF3((AppMain.NNS_MOTION_KEY_Class5[])pKeyList, nKeyFrame, frame, tv);
                    num1 = 1;
                    break;
                case 4:
                    AppMain.nnInterpolateConstantF3((AppMain.NNS_MOTION_KEY_Class5[])pKeyList, nKeyFrame, frame, tv);
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
                        AppMain.nnInterpolateLinearF1((AppMain.NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 4:
                        AppMain.nnInterpolateConstantF1((AppMain.NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 16:
                        AppMain.nnInterpolateBezierF1((AppMain.NNS_MOTION_KEY_Class2[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 32:
                        AppMain.nnInterpolateSISplineF1((AppMain.NNS_MOTION_KEY_Class3[])pKeyList, nKeyFrame, frame, out val);
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

    private static void nnTranslateMatrixFast(AppMain.NNS_MATRIX mtx, float x, float y, float z)
    {
        mtx.M03 = (float)((double)mtx.M00 * (double)x + (double)mtx.M01 * (double)y + (double)mtx.M02 * (double)z) + mtx.M03;
        mtx.M13 = (float)((double)mtx.M10 * (double)x + (double)mtx.M11 * (double)y + (double)mtx.M12 * (double)z) + mtx.M13;
        mtx.M23 = (float)((double)mtx.M20 * (double)x + (double)mtx.M21 * (double)y + (double)mtx.M22 * (double)z) + mtx.M23;
    }

    private static int nnCalcMotionScale(
      AppMain.NNS_SUBMOTION submot,
      float frame,
      AppMain.NNS_VECTOR sv)
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
                    AppMain.nnInterpolateLinearF3((AppMain.NNS_MOTION_KEY_Class5[])pKeyList, nKeyFrame, frame, sv);
                    num1 = 1;
                    break;
                case 4:
                    AppMain.nnInterpolateConstantF3((AppMain.NNS_MOTION_KEY_Class5[])pKeyList, nKeyFrame, frame, sv);
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
                        AppMain.nnInterpolateLinearF1((AppMain.NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 4:
                        AppMain.nnInterpolateConstantF1((AppMain.NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 16:
                        AppMain.nnInterpolateBezierF1((AppMain.NNS_MOTION_KEY_Class2[])pKeyList, nKeyFrame, frame, out val);
                        num1 = 1;
                        break;
                    case 32:
                        AppMain.nnInterpolateSISplineF1((AppMain.NNS_MOTION_KEY_Class3[])pKeyList, nKeyFrame, frame, out val);
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

    private static void nnScaleMatrixFast(AppMain.NNS_MATRIX mtx, float x, float y, float z)
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
        AppMain.mppAssertNotImpl();
    }

    private static void nnCalcMatrixPaletteMotionNode1BoneSIIK(int jnt1nodeIdx)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnCalcMatrixPaletteMotionNode2BoneXSIIK(int rootidx)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnCalcMatrixPaletteMotionNode1BoneXSIIK(int rootidx)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnCallbackMotionUserData(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mot,
      int SubMotIdx,
      int NodeIdx,
      float nframe,
      float origframe)
    {
        AppMain.NNS_NODEUSRMOT_CALLBACK_VAL val = new AppMain.NNS_NODEUSRMOT_CALLBACK_VAL();
        val.iNode = NodeIdx;
        val.Frame = origframe;
        val.pMotion = mot;
        val.iSubmot = SubMotIdx;
        AppMain.NNS_SUBMOTION submot = mot.pSubmotion[SubMotIdx];
        val.fSubmotType = submot.fType;
        val.fSubmotIPType = submot.fIPType;
        val.pObject = obj;
        AppMain.nnCalcMotionUserData(val, submot, nframe);
    }

    private static void nnCalcMotionUserData(
      AppMain.NNS_NODEUSRMOT_CALLBACK_VAL val,
      AppMain.NNS_SUBMOTION submot,
      float frame)
    {
        if (AppMain.nngNodeUserMotionCallbackFunc == null)
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
                    AppMain.nnInterpolateLinearU1((AppMain.NNS_MOTION_KEY_Class12[])pKeyList, nKeyFrame, frame, out val1);
                    break;
                case 4:
                    AppMain.nnInterpolateConstantU1((AppMain.NNS_MOTION_KEY_Class12[])pKeyList, nKeyFrame, frame, out val1);
                    break;
                case 64:
                    AppMain.nnSearchTriggerU1((AppMain.NNS_MOTION_KEY_Class12[])pKeyList, nKeyFrame, frame, AppMain.nngNodeUserMotionTriggerTime, AppMain.nngNodeUserMotionCallbackFunc, val);
                    return;
            }
            val.IValue = val1;
        }
        else if (((int)num & 524288) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    AppMain.nnInterpolateLinearF1((AppMain.NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val2);
                    break;
                case 4:
                    AppMain.nnInterpolateConstantF1((AppMain.NNS_MOTION_KEY_Class1[])pKeyList, nKeyFrame, frame, out val2);
                    break;
                case 16:
                    AppMain.nnInterpolateBezierF1((AppMain.NNS_MOTION_KEY_Class2[])pKeyList, nKeyFrame, frame, out val2);
                    break;
                case 32:
                    AppMain.nnInterpolateSISplineF1((AppMain.NNS_MOTION_KEY_Class3[])pKeyList, nKeyFrame, frame, out val2);
                    break;
            }
            val.FValue = val2;
        }
        AppMain.nngNodeUserMotionCallbackFunc(val);
    }

    private static void nnNormalizeColumn0(AppMain.NNS_MATRIX mtx)
    {
        float m00 = mtx.M00;
        float num1 = m00 * m00;
        float m10 = mtx.M10;
        float num2 = num1 + m10 * m10;
        float m20 = mtx.M20;
        float num3 = AppMain.nnInvertSqrt(num2 + m20 * m20);
        mtx.M00 *= num3;
        mtx.M10 *= num3;
        mtx.M20 *= num3;
    }

    private static void nnNormalizeColumn1(AppMain.NNS_MATRIX mtx)
    {
        float m01 = mtx.M01;
        float num1 = m01 * m01;
        float m11 = mtx.M11;
        float num2 = num1 + m11 * m11;
        float m21 = mtx.M21;
        float num3 = AppMain.nnInvertSqrt(num2 + m21 * m21);
        mtx.M01 *= num3;
        mtx.M11 *= num3;
        mtx.M21 *= num3;
    }

    private static void nnNormalizeColumn2(AppMain.NNS_MATRIX mtx)
    {
        float m02 = mtx.M02;
        float num1 = m02 * m02;
        float m12 = mtx.M12;
        float num2 = num1 + m12 * m12;
        float m22 = mtx.M22;
        float num3 = AppMain.nnInvertSqrt(num2 + m22 * m22);
        mtx.M02 *= num3;
        mtx.M12 *= num3;
        mtx.M22 *= num3;
    }

    private static void nnNormalizeColumn3(AppMain.NNS_MATRIX mtx)
    {
        float m03 = mtx.M03;
        float num1 = m03 * m03;
        float m13 = mtx.M13;
        float num2 = num1 + m13 * m13;
        float m23 = mtx.M23;
        float num3 = AppMain.nnInvertSqrt(num2 + m23 * m23);
        mtx.M03 *= num3;
        mtx.M13 *= num3;
        mtx.M23 *= num3;
    }

    private static void nnPutFixedMaterialGL()
    {
        OpenGL.glDisable(2884U);
        OpenGL.glLightModelf(2898U, 0.0f);
        OpenGL.glDisable(2896U);
        AppMain.nnPutFogSwitchGL(false);
        OpenGL.glDepthMask((byte)1);
        OpenGL.glColorMask((byte)1, (byte)1, (byte)1, (byte)1);
        OpenGL.glDisable(2903U);
        OpenGL.glEnable(3042U);
        OpenGL.glBlendFunc(770U, 771U);
        OpenGL.glBlendEquation(32774U);
        OpenGL.glDisable(3058U);
        OpenGL.glDisable(3008U);
        OpenGL.glEnable(2929U);
        OpenGL.glDepthFunc(515U);
        OpenGL.glMaterialfv(1032U, 4609U, (OpenGL.glArray4f)AppMain.nngColorWhite);
    }

    private static void nnPutDisableTexturesGL()
    {
        OpenGL.glActiveTexture(33984U);
        OpenGL.glDisable(3553U);
        OpenGL.glActiveTexture(33985U);
        OpenGL.glDisable(3553U);
        OpenGL.glClientActiveTexture(33984U);
        OpenGL.glDisableClientState(32888U);
        OpenGL.glClientActiveTexture(33985U);
        OpenGL.glDisableClientState(32888U);
    }

    private static void nnSetDivColor(float r, float g, float b, float a)
    {
        OpenGL.glColor4f(r, g, b, a);
    }

    private static void nnSetDivColorRandom(int i)
    {
        Random random = new Random(i * 15485863);
        OpenGL.glColor3f((float)random.Next() / (float)short.MaxValue, (float)random.Next() / (float)short.MaxValue, (float)random.Next() / (float)short.MaxValue);
    }

    private static void nnSetDivColorRandomA(int nSeed, uint[] seeds)
    {
        int num1;
        uint num2 = (uint)(num1 = 0);
        uint num3 = (uint)num1;
        uint num4 = (uint)num1;
        int num5 = 0;
        for (int index = 0; index < nSeed; ++index)
        {
            int Seed = num5 ^ (int)seeds[index] * 15485863;
            Random random = new Random(Seed);
            num5 = Seed ^ random.Next() ^ random.Next() << 10 ^ random.Next() << 20;
            num4 ^= (uint)random.Next();
            num3 ^= (uint)random.Next();
            num2 ^= (uint)random.Next();
        }
        OpenGL.glColor3f((float)num4 / (float)short.MaxValue, (float)num3 / (float)short.MaxValue, (float)num2 / (float)short.MaxValue);
    }

    private static void nnPutColorStrip(int iStrip, int iMeshset, int iSubobj)
    {
        AppMain.nnSetDivColorRandom(iStrip * 10007 + iMeshset * 7 + iSubobj);
    }

    private static void nnPutColorMeshset(int iMeshset, int iSubobj)
    {
        AppMain.nnSetDivColorRandom(iMeshset * 7 + iSubobj);
    }

    private static void nnPutColorMaterial(int iMaterial)
    {
        AppMain.nnSetDivColorRandom(iMaterial);
    }

    private static void nnPutColorNWeight(AppMain.NNS_VTXLISTPTR vlistptr)
    {
        float[][] numArray = new float[5][]
        {
      new float[3]{ 0.0f, 0.0f, 1f },
      new float[3]{ 0.0f, 1f, 0.0f },
      new float[3]{ 1f, 1f, 0.0f },
      new float[3]{ 1f, 0.0f, 1f },
      new float[3]{ 1f, 0.0f, 0.0f }
        };
        if (((int)vlistptr.fType & 1) != 0)
        {
            AppMain.NNS_VTXLIST_GL_DESC pVtxList = (AppMain.NNS_VTXLIST_GL_DESC)vlistptr.pVtxList;
            int index1 = 0;
            for (int index2 = 0; index2 < pVtxList.nArray; ++index2)
            {
                AppMain.NNS_VTXARRAY_GL p = pVtxList.pArray[index2];
                if (p.Type == 4U)
                    index1 = p.Size;
            }
            OpenGL.glColor3fv(numArray[index1]);
        }
        else
        {
            if (((int)vlistptr.fType & 16711680) == 0)
                return;
            switch (((AppMain.NNS_VTXLIST_COMMON_DESC)vlistptr.pVtxList).List0.fType & 15872U)
            {
                case 0:
                    OpenGL.glColor3fv(numArray[0]);
                    break;
                case 512:
                    OpenGL.glColor3fv(numArray[1]);
                    break;
                case 1024:
                    OpenGL.glColor3fv(numArray[2]);
                    break;
                case 2048:
                    OpenGL.glColor3fv(numArray[3]);
                    break;
                case 4096:
                    OpenGL.glColor3fv(numArray[4]);
                    break;
            }
        }
    }

    private static void nnPutColorNTexture(int nTexture)
    {
        float[][] numArray = new float[8][]
        {
      new float[3],
      new float[3]{ 0.0f, 0.0f, 1f },
      new float[3]{ 0.0f, 1f, 1f },
      new float[3]{ 0.0f, 1f, 0.0f },
      new float[3]{ 1f, 1f, 0.0f },
      new float[3]{ 1f, 0.0f, 0.0f },
      new float[3]{ 1f, 0.0f, 1f },
      new float[3]{ 1f, 1f, 1f }
        };
        if (nTexture >= numArray.Length)
            nTexture = numArray.Length - 1;
        OpenGL.glColor3fv(numArray[nTexture]);
    }
    private void nnCalcNodeMatrixNode(AppMain.NNS_MATRIX mtx, AppMain.NNS_OBJECT obj, int nodeidx)
    {
        AppMain.NNS_NODE pNode = obj.pNodeList[nodeidx];
        if (pNode.iParent != (short)-1)
            this.nnCalcNodeMatrixNode(mtx, obj, (int)pNode.iParent);
        if (((int)pNode.fType & 1) == 0)
        {
            if (((int)pNode.fType & 8192) != 0)
            {
                AppMain.nnTranslateMatrixFast(mtx, pNode.Translation.x, pNode.Translation.y, pNode.Translation.z);
            }
            else
            {
                AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
                AppMain.nnTransformVector(nnsVector, AppMain.nncalcnodematrix.nnsBaseMtx, pNode.Translation);
                AppMain.nnCopyVectorMatrixTranslation(mtx, nnsVector);
            }
        }
        if (((int)pNode.fType & 4096) != 0)
            AppMain.nnCopyMatrix33(mtx, AppMain.nncalcnodematrix.nnsBaseMtx);
        else if (((int)pNode.fType & 1835008) != 0)
        {
            if (((int)pNode.fType & 262144) != 0)
                AppMain.nnNormalizeColumn0(mtx);
            if (((int)pNode.fType & 524288) != 0)
                AppMain.nnNormalizeColumn1(mtx);
            if (((int)pNode.fType & 1048576) != 0)
                AppMain.nnNormalizeColumn2(mtx);
        }
        if (((int)pNode.fType & 2) == 0)
        {
            switch (pNode.fType & 3840U)
            {
                case 0:
                    AppMain.nnRotateXYZMatrixFast(mtx, pNode.Rotation.x, pNode.Rotation.y, pNode.Rotation.z);
                    break;
                case 256:
                    AppMain.nnRotateXZYMatrixFast(mtx, pNode.Rotation.x, pNode.Rotation.y, pNode.Rotation.z);
                    break;
                case 1024:
                    AppMain.nnRotateZXYMatrixFast(mtx, pNode.Rotation.x, pNode.Rotation.y, pNode.Rotation.z);
                    break;
                default:
                    AppMain.nnRotateXYZMatrixFast(mtx, pNode.Rotation.x, pNode.Rotation.y, pNode.Rotation.z);
                    break;
            }
        }
        if (((int)pNode.fType & 4) != 0)
            return;
        AppMain.nnScaleMatrixFast(mtx, pNode.Scaling.x, pNode.Scaling.y, pNode.Scaling.z);
    }

    private void nnCalcNodeMatrix(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_OBJECT obj,
      int nodeidx,
      AppMain.NNS_MATRIX basemtx)
    {
        AppMain.nncalcnodematrix.nnsBaseMtx = basemtx == null ? AppMain.nngUnitMatrix : basemtx;
        AppMain.nnCopyMatrix(mtx, AppMain.nncalcnodematrix.nnsBaseMtx);
        this.nnCalcNodeMatrixNode(mtx, obj, nodeidx);
    }

    private void nnCalcNodeMatrixMotionNode(AppMain.NNS_MATRIX mtx, int nodeidx)
    {
        AppMain.NNS_MATRIX src = (AppMain.NNS_MATRIX)null;
        int num = 0;
        AppMain.NNS_NODE nnsNode = AppMain.nncalcnodematrix.nnsNodeList[nodeidx];
        if (((int)nnsNode.fType & 122880) == 0)
        {
            if (nnsNode.iParent != (short)-1)
                this.nnCalcNodeMatrixMotionNode(mtx, (int)nnsNode.iParent);
            int? pHideFlag = new int?();
            AppMain.nncalcnodematrix.nnsSubMotIdx = AppMain.nnCalcNodeMotionCore(mtx, ref pHideFlag, AppMain.nncalcnodematrix.nnsBaseMtx, nnsNode, nodeidx, AppMain.nncalcnodematrix.nnsObj, AppMain.nncalcnodematrix.nnsMot, AppMain.nncalcnodematrix.nnsSubMotIdx, AppMain.nncalcnodematrix.nnsFrame);
        }
        else
        {
            AppMain.NNS_NODE pNode1 = (AppMain.NNS_NODE)null;
            AppMain.NNS_NODE pNode2 = (AppMain.NNS_NODE)null;
            AppMain.NNS_NODE pNode3 = (AppMain.NNS_NODE)null;
            AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_MATRIX jnt2mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_MATRIX nnsMatrix3 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_MATRIX nnsMatrix4 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_VECTORFAST dst = new AppMain.NNS_VECTORFAST();
            int nodeidx1 = 0;
            int NodeIdx1 = 0;
            int NodeIdx2 = 0;
            int NodeIdx3 = 0;
            if (((int)nnsNode.fType & 8192) != 0)
            {
                src = nnsMatrix2;
                NodeIdx3 = nodeidx;
                pNode3 = nnsNode;
                if (((int)AppMain.nncalcnodematrix.nnsNodeList[(int)nnsNode.iParent].fType & 16384) != 0)
                {
                    num = 1;
                    NodeIdx1 = (int)pNode3.iParent;
                    pNode1 = AppMain.nncalcnodematrix.nnsNodeList[NodeIdx1];
                    nodeidx1 = (int)pNode1.iParent;
                }
                else if (((int)AppMain.nncalcnodematrix.nnsNodeList[(int)nnsNode.iParent].fType & 65536) != 0)
                {
                    num = 2;
                    NodeIdx2 = (int)pNode3.iParent;
                    pNode2 = AppMain.nncalcnodematrix.nnsNodeList[NodeIdx2];
                    NodeIdx1 = (int)pNode2.iParent;
                    pNode1 = AppMain.nncalcnodematrix.nnsNodeList[NodeIdx1];
                    nodeidx1 = (int)pNode1.iParent;
                }
            }
            else if (((int)nnsNode.fType & 16384) != 0)
            {
                src = nnsMatrix1;
                num = 1;
                NodeIdx1 = nodeidx;
                pNode1 = nnsNode;
                nodeidx1 = (int)pNode1.iParent;
                NodeIdx3 = (int)pNode1.iChild;
                pNode3 = AppMain.nncalcnodematrix.nnsNodeList[NodeIdx3];
            }
            else if (((int)nnsNode.fType & 32768) != 0)
            {
                src = nnsMatrix1;
                num = 2;
                NodeIdx1 = nodeidx;
                pNode1 = nnsNode;
                nodeidx1 = (int)pNode1.iParent;
                NodeIdx2 = (int)pNode1.iChild;
                pNode2 = AppMain.nncalcnodematrix.nnsNodeList[NodeIdx2];
                NodeIdx3 = (int)pNode2.iChild;
                pNode3 = AppMain.nncalcnodematrix.nnsNodeList[NodeIdx3];
            }
            else if (((int)nnsNode.fType & 65536) != 0)
            {
                src = jnt2mtx;
                num = 2;
                NodeIdx2 = nodeidx;
                pNode2 = nnsNode;
                NodeIdx3 = (int)pNode2.iChild;
                pNode3 = AppMain.nncalcnodematrix.nnsNodeList[NodeIdx3];
                NodeIdx1 = (int)pNode2.iParent;
                pNode1 = AppMain.nncalcnodematrix.nnsNodeList[NodeIdx1];
                nodeidx1 = (int)pNode1.iParent;
            }
            AppMain.nnCopyMatrix(nnsMatrix1, AppMain.nncalcnodematrix.nnsBaseMtx);
            this.nnCalcNodeMatrixMotionNode(nnsMatrix1, nodeidx1);
            AppMain.nnMakeUnitMatrix(nnsMatrix3);
            int? pHideFlag = new int?();
            AppMain.nncalcnodematrix.nnsSubMotIdx = AppMain.nnCalcNodeMotionCore(nnsMatrix3, ref pHideFlag, nnsMatrix3, pNode1, NodeIdx1, AppMain.nncalcnodematrix.nnsObj, AppMain.nncalcnodematrix.nnsMot, AppMain.nncalcnodematrix.nnsSubMotIdx, AppMain.nncalcnodematrix.nnsFrame);
            float siikBoneLength1 = pNode1.SIIKBoneLength;
            switch (num)
            {
                case 1:
                    AppMain.nnMakeUnitMatrix(nnsMatrix2);
                    pHideFlag = new int?();
                    AppMain.nncalcnodematrix.nnsSubMotIdx = AppMain.nnCalcNodeMotionCore(nnsMatrix2, ref pHideFlag, nnsMatrix2, pNode3, NodeIdx3, AppMain.nncalcnodematrix.nnsObj, AppMain.nncalcnodematrix.nnsMot, AppMain.nncalcnodematrix.nnsSubMotIdx, AppMain.nncalcnodematrix.nnsFrame);
                    AppMain.nnCopyMatrixTranslationVectorFast(out dst, nnsMatrix2);
                    AppMain.nnTransformVectorFast(out dst, AppMain.nncalcnodematrix.nnsBaseMtx, dst);
                    AppMain.nnCopyVectorFastMatrixTranslation(nnsMatrix2, ref dst);
                    AppMain.nnCalc1BoneSIIK(nnsMatrix1, nnsMatrix3, nnsMatrix2, siikBoneLength1);
                    if (((int)pNode3.fType & 4096) != 0)
                    {
                        AppMain.nnCopyMatrix33(nnsMatrix2, AppMain.nncalcnodematrix.nnsBaseMtx);
                        break;
                    }
                    break;
                case 2:
                    AppMain.nnMakeUnitMatrix(nnsMatrix4);
                    pHideFlag = new int?();
                    AppMain.nncalcnodematrix.nnsSubMotIdx = AppMain.nnCalcNodeMotionCore(nnsMatrix4, ref pHideFlag, nnsMatrix4, pNode2, NodeIdx2, AppMain.nncalcnodematrix.nnsObj, AppMain.nncalcnodematrix.nnsMot, AppMain.nncalcnodematrix.nnsSubMotIdx, AppMain.nncalcnodematrix.nnsFrame);
                    AppMain.nnMakeUnitMatrix(nnsMatrix2);
                    AppMain.nncalcnodematrix.nnsSubMotIdx = AppMain.nnCalcNodeMotionCore(nnsMatrix2, ref pHideFlag, nnsMatrix2, pNode3, NodeIdx3, AppMain.nncalcnodematrix.nnsObj, AppMain.nncalcnodematrix.nnsMot, AppMain.nncalcnodematrix.nnsSubMotIdx, AppMain.nncalcnodematrix.nnsFrame);
                    AppMain.nnCopyMatrixTranslationVectorFast(out dst, nnsMatrix2);
                    AppMain.nnTransformVectorFast(out dst, AppMain.nncalcnodematrix.nnsBaseMtx, dst);
                    AppMain.nnCopyVectorFastMatrixTranslation(nnsMatrix2, ref dst);
                    int zpref = ((int)pNode2.fType & 131072) == 0 ? 0 : 1;
                    float siikBoneLength2 = pNode2.SIIKBoneLength;
                    AppMain.nnCalc2BoneSIIK(nnsMatrix1, nnsMatrix3, jnt2mtx, nnsMatrix4, nnsMatrix2, siikBoneLength1, siikBoneLength2, zpref);
                    if (((int)pNode3.fType & 4096) != 0)
                    {
                        AppMain.nnCopyMatrix33(nnsMatrix2, AppMain.nncalcnodematrix.nnsBaseMtx);
                        break;
                    }
                    break;
            }
            AppMain.nnCopyMatrix(mtx, src);
        }
    }

    private void nnCalcNodeMatrixMotion(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_OBJECT obj,
      int nodeidx,
      AppMain.NNS_MOTION mot,
      float frame,
      AppMain.NNS_MATRIX basemtx)
    {
        if (((int)mot.fType & 1) == 0)
            return;
        float dstframe;
        if (AppMain.nnCalcMotionFrame(out dstframe, mot.fType, mot.StartFrame, mot.EndFrame, frame) != 0)
        {
            AppMain.nncalcnodematrix.nnsSubMotIdx = 0;
            if (basemtx != null)
            {
                AppMain.nnCopyMatrix(mtx, basemtx);
                AppMain.nncalcnodematrix.nnsBaseMtx = basemtx;
            }
            else
            {
                AppMain.nnCopyMatrix(mtx, AppMain.nngUnitMatrix);
                AppMain.nncalcnodematrix.nnsBaseMtx = AppMain.nngUnitMatrix;
            }
            AppMain.nncalcnodematrix.nnsFrame = dstframe;
            AppMain.nncalcnodematrix.nnsObj = obj;
            AppMain.nncalcnodematrix.nnsMot = mot;
            AppMain.nncalcnodematrix.nnsNodeList = obj.pNodeList;
            this.nnCalcNodeMatrixMotionNode(mtx, nodeidx);
        }
        else
            this.nnCalcNodeMatrix(mtx, obj, nodeidx, basemtx);
    }

    private static void nnCalcNodeMatrixTRSListNode(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_OBJECT obj,
      int nodeidx,
      AppMain.ArrayPointer<AppMain.NNS_TRS> trslist)
    {
        AppMain.NNS_NODE nnsNode1 = (AppMain.NNS_NODE)null;
        AppMain.NNS_NODE nnsNode2 = (AppMain.NNS_NODE)null;
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.nnCalcNode_mtx_pool.Alloc();
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.nnCalcNode_mtx_pool.Alloc();
        AppMain.NNS_MATRIX nnsMatrix3 = AppMain.nnCalcNode_mtx_pool.Alloc();
        int nodeidx1 = 0;
        int index1 = 0;
        int index2 = 0;
        int num = 0;
        AppMain.NNS_NODE pNode = obj.pNodeList[nodeidx];
        AppMain.NNS_TRS nnsTrs1 = trslist[nodeidx];
        AppMain.NNS_MATRIX src;
        int index3;
        AppMain.NNS_NODE nnsNode3;
        if (((int)pNode.fType & 8192) != 0)
        {
            src = nnsMatrix3;
            index3 = nodeidx;
            nnsNode3 = pNode;
            if (((int)obj.pNodeList[(int)pNode.iParent].fType & 16384) != 0)
            {
                num = 1;
                index1 = (int)nnsNode3.iParent;
                nnsNode1 = obj.pNodeList[index1];
                nodeidx1 = (int)nnsNode1.iParent;
            }
            else if (((int)obj.pNodeList[(int)pNode.iParent].fType & 65536) != 0)
            {
                num = 2;
                index2 = (int)nnsNode3.iParent;
                nnsNode2 = obj.pNodeList[index2];
                index1 = (int)nnsNode2.iParent;
                nnsNode1 = obj.pNodeList[index1];
                nodeidx1 = (int)nnsNode1.iParent;
            }
        }
        else if (((int)pNode.fType & 16384) != 0)
        {
            src = nnsMatrix1;
            num = 1;
            index1 = nodeidx;
            nnsNode1 = pNode;
            nodeidx1 = (int)nnsNode1.iParent;
            index3 = (int)nnsNode1.iChild;
            nnsNode3 = obj.pNodeList[index3];
        }
        else if (((int)pNode.fType & 32768) != 0)
        {
            src = nnsMatrix1;
            num = 2;
            index1 = nodeidx;
            nnsNode1 = pNode;
            nodeidx1 = (int)nnsNode1.iParent;
            index2 = (int)nnsNode1.iChild;
            nnsNode2 = obj.pNodeList[index2];
            index3 = (int)nnsNode2.iChild;
            nnsNode3 = obj.pNodeList[index3];
        }
        else if (((int)pNode.fType & 65536) != 0)
        {
            src = nnsMatrix2;
            num = 2;
            index2 = nodeidx;
            nnsNode2 = pNode;
            index3 = (int)nnsNode2.iChild;
            nnsNode3 = obj.pNodeList[index3];
            index1 = (int)nnsNode2.iParent;
            nnsNode1 = obj.pNodeList[index1];
            nodeidx1 = (int)nnsNode1.iParent;
        }
        else
        {
            AppMain.nnCalcNode_mtx_pool.Release(nnsMatrix1);
            AppMain.nnCalcNode_mtx_pool.Release(nnsMatrix2);
            AppMain.nnCalcNode_mtx_pool.Release(nnsMatrix3);
            if (pNode.iParent != (short)-1)
                AppMain.nnCalcNodeMatrixTRSListNode(mtx, obj, (int)pNode.iParent, trslist);
            else
                AppMain.nnCopyMatrix(mtx, AppMain.nncalcnodematrix.nnsBaseMtx);
            AppMain.nnTranslateMatrix(mtx, mtx, nnsTrs1.Translation.x, nnsTrs1.Translation.y, nnsTrs1.Translation.z);
            if (((int)pNode.fType & 4096) != 0)
                AppMain.nnCopyMatrix33(mtx, AppMain.nncalcnodematrix.nnsBaseMtx);
            else if (((int)pNode.fType & 1835008) != 0)
            {
                if (((int)pNode.fType & 262144) != 0)
                    AppMain.nnNormalizeColumn0(mtx);
                if (((int)pNode.fType & 524288) != 0)
                    AppMain.nnNormalizeColumn1(mtx);
                if (((int)pNode.fType & 1048576) != 0)
                    AppMain.nnNormalizeColumn2(mtx);
            }
            AppMain.nnQuaternionMatrix(mtx, mtx, ref nnsTrs1.Rotation);
            AppMain.nnScaleMatrix(mtx, mtx, nnsTrs1.Scaling.x, nnsTrs1.Scaling.y, nnsTrs1.Scaling.z);
            return;
        }
        AppMain.NNS_MATRIX nnsMatrix4 = AppMain.nnCalcNode_mtx_pool.Alloc();
        AppMain.NNS_MATRIX nnsMatrix5 = AppMain.nnCalcNode_mtx_pool.Alloc();
        AppMain.NNS_VECTORFAST dst = new AppMain.NNS_VECTORFAST();
        AppMain.NNS_TRS nnsTrs2 = (AppMain.NNS_TRS)(AppMain.ArrayPointer<AppMain.NNS_TRS>)(trslist + index1);
        AppMain.NNS_TRS nnsTrs3 = (AppMain.NNS_TRS)(AppMain.ArrayPointer<AppMain.NNS_TRS>)(trslist + index2);
        AppMain.NNS_TRS nnsTrs4 = (AppMain.NNS_TRS)(AppMain.ArrayPointer<AppMain.NNS_TRS>)(trslist + index3);
        AppMain.nnCalcNodeMatrixTRSListNode(nnsMatrix1, obj, nodeidx1, trslist);
        AppMain.nnMakeUnitMatrix(nnsMatrix4);
        AppMain.nnTranslateMatrix(nnsMatrix4, nnsMatrix4, nnsTrs2.Translation.x, nnsTrs2.Translation.y, nnsTrs2.Translation.z);
        AppMain.nnQuaternionMatrix(nnsMatrix4, nnsMatrix4, ref nnsTrs2.Rotation);
        AppMain.nnScaleMatrix(nnsMatrix4, nnsMatrix4, nnsTrs2.Scaling.x, nnsTrs2.Scaling.y, nnsTrs2.Scaling.z);
        float siikBoneLength1 = nnsNode1.SIIKBoneLength;
        switch (num)
        {
            case 1:
                AppMain.nnMakeUnitMatrix(nnsMatrix3);
                AppMain.nnTranslateMatrix(nnsMatrix3, nnsMatrix3, nnsTrs4.Translation.x, nnsTrs4.Translation.y, nnsTrs4.Translation.z);
                AppMain.nnQuaternionMatrix(nnsMatrix3, nnsMatrix3, ref nnsTrs4.Rotation);
                AppMain.nnScaleMatrix(nnsMatrix3, nnsMatrix3, nnsTrs4.Scaling.x, nnsTrs4.Scaling.y, nnsTrs4.Scaling.z);
                AppMain.nnCopyMatrixTranslationVectorFast(out dst, nnsMatrix3);
                AppMain.nnTransformVectorFast(out dst, AppMain.nncalcnodematrix.nnsBaseMtx, dst);
                AppMain.nnCopyVectorFastMatrixTranslation(nnsMatrix3, ref dst);
                AppMain.nnCalc1BoneSIIK(nnsMatrix1, nnsMatrix4, nnsMatrix3, siikBoneLength1);
                if (((int)nnsNode3.fType & 4096) != 0)
                {
                    AppMain.nnCopyMatrix33(nnsMatrix3, AppMain.nncalcnodematrix.nnsBaseMtx);
                    break;
                }
                break;
            case 2:
                AppMain.nnMakeUnitMatrix(nnsMatrix5);
                AppMain.nnTranslateMatrix(nnsMatrix2, nnsMatrix2, nnsTrs3.Translation.x, nnsTrs3.Translation.y, nnsTrs3.Translation.z);
                AppMain.nnQuaternionMatrix(nnsMatrix2, nnsMatrix2, ref nnsTrs3.Rotation);
                AppMain.nnScaleMatrix(nnsMatrix2, nnsMatrix2, nnsTrs3.Scaling.x, nnsTrs3.Scaling.y, nnsTrs3.Scaling.z);
                AppMain.nnMakeUnitMatrix(nnsMatrix3);
                AppMain.nnTranslateMatrix(nnsMatrix3, nnsMatrix3, nnsTrs4.Translation.x, nnsTrs4.Translation.y, nnsTrs4.Translation.z);
                AppMain.nnQuaternionMatrix(nnsMatrix3, nnsMatrix3, ref nnsTrs4.Rotation);
                AppMain.nnScaleMatrix(nnsMatrix3, nnsMatrix3, nnsTrs4.Scaling.x, nnsTrs4.Scaling.y, nnsTrs4.Scaling.z);
                AppMain.nnCopyMatrixTranslationVectorFast(out dst, nnsMatrix3);
                AppMain.nnTransformVectorFast(out dst, AppMain.nncalcnodematrix.nnsBaseMtx, dst);
                AppMain.nnCopyVectorFastMatrixTranslation(nnsMatrix3, ref dst);
                int zpref = ((int)nnsNode2.fType & 131072) != 0 ? 0 : 1;
                float siikBoneLength2 = nnsNode2.SIIKBoneLength;
                AppMain.nnCalc2BoneSIIK(nnsMatrix1, nnsMatrix4, nnsMatrix2, nnsMatrix5, nnsMatrix3, siikBoneLength1, siikBoneLength2, zpref);
                if (((int)nnsNode3.fType & 4096) != 0)
                {
                    AppMain.nnCopyMatrix33(nnsMatrix3, AppMain.nncalcnodematrix.nnsBaseMtx);
                    break;
                }
                break;
        }
        AppMain.nnCopyMatrix(mtx, src);
        AppMain.nnCalcNode_mtx_pool.Release(nnsMatrix4);
        AppMain.nnCalcNode_mtx_pool.Release(nnsMatrix5);
        AppMain.nnCalcNode_mtx_pool.Release(nnsMatrix1);
        AppMain.nnCalcNode_mtx_pool.Release(nnsMatrix2);
        AppMain.nnCalcNode_mtx_pool.Release(nnsMatrix3);
    }

    private static void nnCalcNodeMatrixTRSList(
      AppMain.NNS_MATRIX mtx,
      AppMain.NNS_OBJECT obj,
      int nodeidx,
      AppMain.ArrayPointer<AppMain.NNS_TRS> trslist,
      AppMain.NNS_MATRIX basemtx)
    {
        AppMain.nncalcnodematrix.nnsBaseMtx = basemtx == null ? AppMain.nngUnitMatrix : basemtx;
        AppMain.nnCalcNodeMatrixTRSListNode(mtx, obj, nodeidx, trslist);
    }

    public static void nnCopyMatrix33(AppMain.NNS_MATRIX dst, AppMain.NNS_MATRIX src)
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

    public static void nnCopyMatrix33(ref AppMain.SNNS_MATRIX dst, ref AppMain.SNNS_MATRIX src)
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
        AppMain.NNS_NODE nnsNode;
        do
        {
            nnsNode = AppMain.nncalcmatrixpalette.nnsNodeList[nodeIdx];
            AppMain.nnPushMatrix(AppMain.nncalcmatrixpalette.nnsMstk, (AppMain.NNS_MATRIX)null);
            AppMain.NNS_MATRIX currentMatrix = AppMain.nnGetCurrentMatrix(AppMain.nncalcmatrixpalette.nnsMstk);
            if (((int)nnsNode.fType & 1) == 0)
            {
                if (((int)nnsNode.fType & 8192) == 0)
                {
                    AppMain.nnTranslateMatrixFast(currentMatrix, nnsNode.Translation.x, nnsNode.Translation.y, nnsNode.Translation.z);
                }
                else
                {
                    AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
                    AppMain.nnTransformVector(nnsVector, AppMain.nncalcmatrixpalette.nnsBaseMtx, nnsNode.Translation);
                    AppMain.nnCopyVectorMatrixTranslation(currentMatrix, nnsVector);
                    AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
                }
            }
            if (((int)nnsNode.fType & 4096) != 0)
                AppMain.nnCopyMatrix33(currentMatrix, AppMain.nncalcmatrixpalette.nnsBaseMtx);
            else if (((int)nnsNode.fType & 1835008) != 0)
            {
                if (((int)nnsNode.fType & 262144) != 0)
                    AppMain.nnNormalizeColumn0(currentMatrix);
                if (((int)nnsNode.fType & 524288) != 0)
                    AppMain.nnNormalizeColumn1(currentMatrix);
                if (((int)nnsNode.fType & 1048576) != 0)
                    AppMain.nnNormalizeColumn2(currentMatrix);
            }
            if (((int)nnsNode.fType & 2) == 0)
            {
                switch (nnsNode.fType & 3840U)
                {
                    case 0:
                        AppMain.nnRotateXYZMatrixFast(currentMatrix, nnsNode.Rotation.x, nnsNode.Rotation.y, nnsNode.Rotation.z);
                        break;
                    case 256:
                        AppMain.nnRotateXZYMatrixFast(currentMatrix, nnsNode.Rotation.x, nnsNode.Rotation.y, nnsNode.Rotation.z);
                        break;
                    case 1024:
                        AppMain.nnRotateZXYMatrixFast(currentMatrix, nnsNode.Rotation.x, nnsNode.Rotation.y, nnsNode.Rotation.z);
                        break;
                    default:
                        AppMain.nnRotateXYZMatrixFast(currentMatrix, nnsNode.Rotation.x, nnsNode.Rotation.y, nnsNode.Rotation.z);
                        break;
                }
            }
            if (((int)nnsNode.fType & 4) == 0)
                AppMain.nnScaleMatrixFast(currentMatrix, nnsNode.Scaling.x, nnsNode.Scaling.y, nnsNode.Scaling.z);
            if (nnsNode.iMatrix != (short)-1)
            {
                if (((int)nnsNode.fType & 8) != 0)
                    AppMain.nnCopyMatrix(AppMain.nncalcmatrixpalette.nnsMtxPal[(int)nnsNode.iMatrix], currentMatrix);
                else
                    AppMain.nnMultiplyMatrix(AppMain.nncalcmatrixpalette.nnsMtxPal[(int)nnsNode.iMatrix], currentMatrix, nnsNode.InvInitMtx);
            }
            if (AppMain.nncalcmatrixpalette.nnsNodeStatList != null)
            {
                if (nodeIdx == 0 && AppMain.nncalcmatrixpalette.nnsNSFlag != 0U)
                    AppMain.nncalcmatrixpalette.nnsRootScale = AppMain.nnEstimateMatrixScaling(currentMatrix);
                AppMain.nnCalcClipSetNodeStatus(AppMain.nncalcmatrixpalette.nnsNodeStatList, AppMain.nncalcmatrixpalette.nnsNodeList, nodeIdx, currentMatrix, AppMain.nncalcmatrixpalette.nnsRootScale, AppMain.nncalcmatrixpalette.nnsNSFlag);
            }
            if (nnsNode.iChild != (short)-1)
                AppMain.nnCalcMatrixPaletteNode((int)nnsNode.iChild);
            AppMain.nnPopMatrix(AppMain.nncalcmatrixpalette.nnsMstk);
            nodeIdx = (int)nnsNode.iSibling;
        }
        while (nnsNode.iSibling != (short)-1);
    }

    private static void nnCalcMatrixPalette(
      AppMain.NNS_MATRIX[] mtxpal,
      uint[] nodestatlist,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MATRIX basemtx,
      AppMain.NNS_MATRIXSTACK mstk,
      uint flag)
    {
        AppMain.nncalcmatrixpalette.nnsBaseMtx = basemtx == null ? AppMain.nngUnitMatrix : basemtx;
        AppMain.nnSetCurrentMatrix(mstk, AppMain.nncalcmatrixpalette.nnsBaseMtx);
        AppMain.nncalcmatrixpalette.nnsMtxPal = mtxpal;
        AppMain.nncalcmatrixpalette.nnsNodeStatList = nodestatlist;
        AppMain.nncalcmatrixpalette.nnsNSFlag = flag;
        AppMain.nncalcmatrixpalette.nnsNodeList = obj.pNodeList;
        AppMain.nncalcmatrixpalette.nnsMstk = mstk;
        AppMain.nnCalcMatrixPaletteNode(0);
    }

    private static void nnPutCommonVertex(
      AppMain.NNS_VTXLIST_COMMON_DESC vdesc,
      int nIndexSetSize,
      ushort indices,
      AppMain.NNS_MATRIX mtxpal,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnPutNormalVector(AppMain.NNS_VECTOR pos, AppMain.NNS_VECTOR nrm)
    {
        AppMain.mppAssertNotImpl();
    }

    private static void nnPutCommonVertexNormal(
      AppMain.NNS_VTXLIST_COMMON_DESC vdesc,
      int nIndexSetSize,
      ushort indices,
      AppMain.NNS_MATRIX mtxpal)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawObjectCommonVertex(
      AppMain.NNS_VTXLISTPTR vlistptr,
      AppMain.NNS_PRIMLISTPTR plistptr,
      AppMain.NNS_MATRIX mtxpal,
      uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private void nnDrawObjectCommonVertexNormal(
      AppMain.NNS_VTXLISTPTR vlistptr,
      AppMain.NNS_PRIMLISTPTR plistptr,
      AppMain.NNS_MATRIX mtxpal)
    {
        AppMain.mppAssertNotImpl();
    }

    public static uint NNM_GL_TEXTURE(int _slot)
    {
        return (uint)(33984 + _slot);
    }

    public static uint NNM_GL_LIGHT(int _idx)
    {
        return (uint)(16384 + _idx);
    }

    private int nnCheckCollisionSS(ref AppMain.NNS_SPHERE sphere1, ref AppMain.NNS_SPHERE sphere2)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private int nnCheckCollisionCC(ref AppMain.NNS_CAPSULE capsule1, ref AppMain.NNS_CAPSULE capsule2)
    {
        AppMain.mppAssertNotImpl();
        return 1;
    }

    private int nnCheckCollisionBS(ref AppMain.NNS_BOX box, ref AppMain.NNS_SPHERE sphere)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private int nnCheckCollisionBB(ref AppMain.NNS_BOX box1, ref AppMain.NNS_BOX box2)
    {
        AppMain.mppAssertNotImpl();
        return 1;
    }

    public static void nnInitMaterialMotionObject(
      AppMain.NNS_OBJECT mmobj,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mmot)
    {
        int nMaterial = obj.nMaterial;
        mmobj.Assign(obj);
        mmobj.pMatPtrList = new AppMain.NNS_MATERIALPTR[nMaterial];
        for (int index = 0; index < nMaterial; ++index)
            mmobj.pMatPtrList[index] = new AppMain.NNS_MATERIALPTR(obj.pMatPtrList[index]);
        if (mmot != null)
        {
            if (((int)mmot.fType & 31) != 16)
                return;
            AppMain.ArrayPointer<AppMain.NNS_SUBMOTION> pSubmotion = (AppMain.ArrayPointer<AppMain.NNS_SUBMOTION>)mmot.pSubmotion;
            int nSubmotion = mmot.nSubmotion;
            for (int index1 = 0; index1 < nMaterial; ++index1)
            {
                bool flag1 = false;
                bool flag2 = false;
                bool flag3 = false;
                bool[] objectBTexOffsetMot = AppMain.nnInitMaterialMotionObject_bTexOffsetMot;
                Array.Clear((Array)objectBTexOffsetMot, 0, 8);
                for (; nSubmotion > 0 && (int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id0 < index1; --nSubmotion)
                    ++pSubmotion;
                for (; nSubmotion > 0 && (int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id0 == index1; --nSubmotion)
                {
                    switch (((AppMain.NNS_SUBMOTION)~pSubmotion).fType & 4294967040U)
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
                            objectBTexOffsetMot[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1] = true;
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
                        AppMain.NNS_MATERIAL_STDSHADER_DESC pMaterial = (AppMain.NNS_MATERIAL_STDSHADER_DESC)obj.pMatPtrList[index1].pMaterial;
                        AppMain.NNS_MATERIAL_STDSHADER_DESC materialStdshaderDesc;
                        mmobj.pMatPtrList[index1].pMaterial = ((int)fType & 4) == 0 ? (object)(materialStdshaderDesc = new AppMain.NNS_MATERIAL_STDSHADER_DESC(pMaterial)) : (object)(AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)(materialStdshaderDesc = (AppMain.NNS_MATERIAL_STDSHADER_DESC)new AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE((AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)pMaterial));
                        if (flag2)
                            materialStdshaderDesc.pColor = new AppMain.NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            materialStdshaderDesc.pTexDesc = new AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                materialStdshaderDesc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                                if (objectBTexOffsetMot[index2])
                                    materialStdshaderDesc.pTexDesc[index2].fType &= 3221225471U;
                            }
                        }
                    }
                    else if (((int)fType & 8) != 0)
                    {
                        AppMain.NNS_MATERIAL_GLES11_DESC pMaterial = (AppMain.NNS_MATERIAL_GLES11_DESC)obj.pMatPtrList[index1].pMaterial;
                        AppMain.NNS_MATERIAL_GLES11_DESC materialGleS11Desc;
                        mmobj.pMatPtrList[index1].pMaterial = (object)(materialGleS11Desc = new AppMain.NNS_MATERIAL_GLES11_DESC(pMaterial));
                        if (flag2)
                            materialGleS11Desc.pColor = new AppMain.NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            materialGleS11Desc.pTexDesc = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                materialGleS11Desc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC(ref pMaterial.pTexDesc[index2]);
                                if (objectBTexOffsetMot[index2])
                                    materialGleS11Desc.pTexDesc[index2].fType &= 3221225471U;
                            }
                        }
                    }
                    else if (((int)fType & 1) != 0)
                    {
                        AppMain.NNS_MATERIAL_DESC pMaterial = (AppMain.NNS_MATERIAL_DESC)obj.pMatPtrList[index1].pMaterial;
                        AppMain.NNS_MATERIAL_DESC nnsMaterialDesc;
                        mmobj.pMatPtrList[index1].pMaterial = (object)(nnsMaterialDesc = new AppMain.NNS_MATERIAL_DESC(pMaterial));
                        if (flag2)
                            nnsMaterialDesc.pColor = new AppMain.NNS_MATERIAL_COLOR(pMaterial.pColor);
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            nnsMaterialDesc.pTexDesc = new AppMain.NNS_MATERIAL_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                nnsMaterialDesc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
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
                    AppMain.NNS_MATERIAL_STDSHADER_DESC pMaterial = (AppMain.NNS_MATERIAL_STDSHADER_DESC)obj.pMatPtrList[index1].pMaterial;
                    AppMain.NNS_MATERIAL_STDSHADER_DESC materialStdshaderDesc;
                    mmobj.pMatPtrList[index1].pMaterial = ((int)fType & 4) == 0 ? (object)(materialStdshaderDesc = new AppMain.NNS_MATERIAL_STDSHADER_DESC(pMaterial)) : (object)(AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)(materialStdshaderDesc = (AppMain.NNS_MATERIAL_STDSHADER_DESC)new AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE((AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)pMaterial));
                    materialStdshaderDesc.pColor = new AppMain.NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                    materialStdshaderDesc.pTexDesc = new AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        materialStdshaderDesc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                }
                else if (((int)fType & 8) != 0)
                {
                    AppMain.NNS_MATERIAL_GLES11_DESC pMaterial = (AppMain.NNS_MATERIAL_GLES11_DESC)obj.pMatPtrList[index1].pMaterial;
                    AppMain.NNS_MATERIAL_GLES11_DESC materialGleS11Desc;
                    mmobj.pMatPtrList[index1].pMaterial = (object)(materialGleS11Desc = new AppMain.NNS_MATERIAL_GLES11_DESC(pMaterial));
                    materialGleS11Desc.pColor = new AppMain.NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                    materialGleS11Desc.pTexDesc = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        materialGleS11Desc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC(ref pMaterial.pTexDesc[index2]);
                }
                else if (((int)fType & 1) != 0)
                {
                    AppMain.NNS_MATERIAL_DESC pMaterial = (AppMain.NNS_MATERIAL_DESC)obj.pMatPtrList[index1].pMaterial;
                    AppMain.NNS_MATERIAL_DESC nnsMaterialDesc;
                    mmobj.pMatPtrList[index1].pMaterial = (object)(nnsMaterialDesc = new AppMain.NNS_MATERIAL_DESC(pMaterial));
                    nnsMaterialDesc.pColor = new AppMain.NNS_MATERIAL_COLOR(pMaterial.pColor);
                    nnsMaterialDesc.pTexDesc = new AppMain.NNS_MATERIAL_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        nnsMaterialDesc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                }
            }
        }
    }

    public static void nnInitMaterialMotionObject_fast(
      AppMain.NNS_OBJECT mmobj,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mmot)
    {
        int nMaterial = obj.nMaterial;
        mmobj.Assign(obj);
        mmobj.pMatPtrList = AppMain.amDrawAlloc_NNS_MATERIALPTR(nMaterial);
        for (int index = 0; index < nMaterial; ++index)
        {
            mmobj.pMatPtrList[index] = AppMain.amDrawAlloc_NNS_MATERIALPTR();
            mmobj.pMatPtrList[index].Assign(obj.pMatPtrList[index]);
        }
        if (mmot != null)
        {
            if (((int)mmot.fType & 31) != 16)
                return;
            AppMain.ArrayPointer<AppMain.NNS_SUBMOTION> pSubmotion = (AppMain.ArrayPointer<AppMain.NNS_SUBMOTION>)mmot.pSubmotion;
            int nSubmotion = mmot.nSubmotion;
            for (int index1 = 0; index1 < nMaterial; ++index1)
            {
                bool flag1 = false;
                bool flag2 = false;
                bool flag3 = false;
                bool[] objectBTexOffsetMot = AppMain.nnInitMaterialMotionObject_bTexOffsetMot;
                Array.Clear((Array)objectBTexOffsetMot, 0, 8);
                for (; nSubmotion > 0 && (int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id0 < index1; --nSubmotion)
                    ++pSubmotion;
                for (; nSubmotion > 0 && (int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id0 == index1; --nSubmotion)
                {
                    switch (((AppMain.NNS_SUBMOTION)~pSubmotion).fType & 4294967040U)
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
                            objectBTexOffsetMot[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1] = true;
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
                        AppMain.NNS_MATERIAL_STDSHADER_DESC pMaterial = (AppMain.NNS_MATERIAL_STDSHADER_DESC)obj.pMatPtrList[index1].pMaterial;
                        AppMain.NNS_MATERIAL_STDSHADER_DESC materialStdshaderDesc;
                        mmobj.pMatPtrList[index1].pMaterial = ((int)fType & 4) == 0 ? (object)(materialStdshaderDesc = new AppMain.NNS_MATERIAL_STDSHADER_DESC(pMaterial)) : (object)(AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)(materialStdshaderDesc = (AppMain.NNS_MATERIAL_STDSHADER_DESC)new AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE((AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)pMaterial));
                        if (flag2)
                            materialStdshaderDesc.pColor = new AppMain.NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            materialStdshaderDesc.pTexDesc = new AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                materialStdshaderDesc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                                if (objectBTexOffsetMot[index2])
                                    materialStdshaderDesc.pTexDesc[index2].fType &= 3221225471U;
                            }
                        }
                    }
                    else if (((int)fType & 8) != 0)
                    {
                        AppMain.NNS_MATERIAL_GLES11_DESC pMaterial = (AppMain.NNS_MATERIAL_GLES11_DESC)obj.pMatPtrList[index1].pMaterial;
                        AppMain.NNS_MATERIAL_GLES11_DESC materialGleS11Desc;
                        mmobj.pMatPtrList[index1].pMaterial = (object)(materialGleS11Desc = AppMain.amDrawAlloc_NNS_MATERIAL_GLES11_DESC());
                        materialGleS11Desc.Assign(pMaterial);
                        if (flag2)
                        {
                            materialGleS11Desc.pColor = AppMain.amDrawAlloc_NNS_MATERIAL_STDSHADER_COLOR();
                            materialGleS11Desc.pColor.Assign(pMaterial.pColor);
                        }
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            materialGleS11Desc.pTexDesc = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                materialGleS11Desc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC(ref pMaterial.pTexDesc[index2]);
                                if (objectBTexOffsetMot[index2])
                                    materialGleS11Desc.pTexDesc[index2].fType &= 3221225471U;
                            }
                        }
                    }
                    else if (((int)fType & 1) != 0)
                    {
                        AppMain.NNS_MATERIAL_DESC pMaterial = (AppMain.NNS_MATERIAL_DESC)obj.pMatPtrList[index1].pMaterial;
                        AppMain.NNS_MATERIAL_DESC nnsMaterialDesc;
                        mmobj.pMatPtrList[index1].pMaterial = (object)(nnsMaterialDesc = new AppMain.NNS_MATERIAL_DESC(pMaterial));
                        if (flag2)
                            nnsMaterialDesc.pColor = new AppMain.NNS_MATERIAL_COLOR(pMaterial.pColor);
                        if (flag3)
                        {
                            int nTex = pMaterial.nTex;
                            nnsMaterialDesc.pTexDesc = new AppMain.NNS_MATERIAL_TEXMAP_DESC[nTex];
                            for (int index2 = 0; index2 < nTex; ++index2)
                            {
                                nnsMaterialDesc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
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
                    AppMain.NNS_MATERIAL_STDSHADER_DESC pMaterial = (AppMain.NNS_MATERIAL_STDSHADER_DESC)obj.pMatPtrList[index1].pMaterial;
                    AppMain.NNS_MATERIAL_STDSHADER_DESC materialStdshaderDesc;
                    mmobj.pMatPtrList[index1].pMaterial = ((int)fType & 4) == 0 ? (object)(materialStdshaderDesc = new AppMain.NNS_MATERIAL_STDSHADER_DESC(pMaterial)) : (object)(AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)(materialStdshaderDesc = (AppMain.NNS_MATERIAL_STDSHADER_DESC)new AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE((AppMain.NNS_MATERIAL_STDSHADER_DESC_USER_PROFILE)pMaterial));
                    materialStdshaderDesc.pColor = new AppMain.NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                    materialStdshaderDesc.pTexDesc = new AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        materialStdshaderDesc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                }
                else if (((int)fType & 8) != 0)
                {
                    AppMain.NNS_MATERIAL_GLES11_DESC pMaterial = (AppMain.NNS_MATERIAL_GLES11_DESC)obj.pMatPtrList[index1].pMaterial;
                    AppMain.NNS_MATERIAL_GLES11_DESC materialGleS11Desc;
                    mmobj.pMatPtrList[index1].pMaterial = (object)(materialGleS11Desc = new AppMain.NNS_MATERIAL_GLES11_DESC(pMaterial));
                    materialGleS11Desc.pColor = new AppMain.NNS_MATERIAL_STDSHADER_COLOR(pMaterial.pColor);
                    materialGleS11Desc.pTexDesc = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        materialGleS11Desc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC(ref pMaterial.pTexDesc[index2]);
                }
                else if (((int)fType & 1) != 0)
                {
                    AppMain.NNS_MATERIAL_DESC pMaterial = (AppMain.NNS_MATERIAL_DESC)obj.pMatPtrList[index1].pMaterial;
                    AppMain.NNS_MATERIAL_DESC nnsMaterialDesc;
                    mmobj.pMatPtrList[index1].pMaterial = (object)(nnsMaterialDesc = new AppMain.NNS_MATERIAL_DESC(pMaterial));
                    nnsMaterialDesc.pColor = new AppMain.NNS_MATERIAL_COLOR(pMaterial.pColor);
                    nnsMaterialDesc.pTexDesc = new AppMain.NNS_MATERIAL_TEXMAP_DESC[pMaterial.nTex];
                    for (int index2 = 0; index2 < pMaterial.nTex; ++index2)
                        nnsMaterialDesc.pTexDesc[index2] = new AppMain.NNS_MATERIAL_TEXMAP_DESC(pMaterial.pTexDesc[index2]);
                }
            }
        }
    }

    public static int nnInterpolateFloat(ref float val, AppMain.NNS_SUBMOTION submot, float frame)
    {
        if (AppMain.nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    AppMain.nnInterpolateLinearF1((AppMain.NNS_MOTION_KEY_Class1[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 4:
                    AppMain.nnInterpolateConstantF1((AppMain.NNS_MOTION_KEY_Class1[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 16:
                    AppMain.nnInterpolateBezierF1((AppMain.NNS_MOTION_KEY_Class2[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 32:
                    AppMain.nnInterpolateSISplineF1((AppMain.NNS_MOTION_KEY_Class3[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
            }
        }
        return 0;
    }

    public static int nnInterpolateFloat2(
      AppMain.NNS_TEXCOORD val,
      AppMain.NNS_SUBMOTION submot,
      float frame)
    {
        if (AppMain.nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    AppMain.nnInterpolateLinearF2((AppMain.NNS_MOTION_KEY_Class4[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 4:
                    AppMain.nnInterpolateConstantF2((AppMain.NNS_MOTION_KEY_Class4[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
            }
        }
        return 0;
    }

    public static int nnInterpolateFloat3(
      AppMain.NNS_VECTOR val,
      AppMain.NNS_SUBMOTION submot,
      float frame)
    {
        if (AppMain.nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    AppMain.nnInterpolateLinearF3((AppMain.NNS_MOTION_KEY_Class5[])submot.pKeyList, submot.nKeyFrame, frame, val);
                    return 1;
                case 4:
                    AppMain.nnInterpolateConstantF3((AppMain.NNS_MOTION_KEY_Class5[])submot.pKeyList, submot.nKeyFrame, frame, val);
                    return 1;
            }
        }
        return 0;
    }

    public static int nnInterpolateFloat3(
      ref AppMain.SNNS_VECTOR val,
      AppMain.NNS_SUBMOTION submot,
      float frame)
    {
        if (AppMain.nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    AppMain.nnInterpolateLinearF3((AppMain.NNS_MOTION_KEY_Class5[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 4:
                    AppMain.nnInterpolateConstantF3((AppMain.NNS_MOTION_KEY_Class5[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
            }
        }
        return 0;
    }

    public static int nnInterpolateFloat3(
      ref AppMain.NNS_RGBA _val,
      AppMain.NNS_SUBMOTION submot,
      float frame)
    {
        AppMain.SNNS_VECTOR val;
        val.x = _val.r;
        val.y = _val.g;
        val.z = _val.b;
        int num = AppMain.nnInterpolateFloat3(ref val, submot, frame);
        _val.r = val.x;
        _val.g = val.y;
        _val.b = val.z;
        return num;
    }

    public static int nnInterpolateUint32(ref uint val, AppMain.NNS_SUBMOTION submot, float frame)
    {
        if (AppMain.nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) != 0)
        {
            switch (submot.fIPType & 3703U)
            {
                case 2:
                    AppMain.nnInterpolateLinearU1((AppMain.NNS_MOTION_KEY_Class12[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 4:
                    AppMain.nnInterpolateConstantU1((AppMain.NNS_MOTION_KEY_Class12[])submot.pKeyList, submot.nKeyFrame, frame, out val);
                    return 1;
                case 64:
                    return AppMain.nnInterpolateTriggerU1((AppMain.NNS_MOTION_KEY_Class12[])submot.pKeyList, submot.nKeyFrame, frame, out val);
            }
        }
        return 0;
    }

    public static int nnInterpolateSint32(ref int val, AppMain.NNS_SUBMOTION submot, float frame)
    {
        if (AppMain.nnCalcMotionFrame(out frame, submot.fIPType, submot.StartFrame, submot.EndFrame, frame) == 0 || (submot.fIPType & 3703U) != 4U)
            return 0;
        AppMain.nnInterpolateConstantS32_1((AppMain.NNS_MOTION_KEY_Class11[])submot.pKeyList, submot.nKeyFrame, frame, out val);
        return 1;
    }

    public static void nnCalcMaterialMotion(
      AppMain.NNS_OBJECT mmobj,
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mmot,
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
                AppMain.NNS_MATERIAL_STDSHADER_DESC pMaterial1 = (AppMain.NNS_MATERIAL_STDSHADER_DESC)obj.pMatPtrList[index1].pMaterial;
                AppMain.NNS_MATERIAL_STDSHADER_DESC pMaterial2 = (AppMain.NNS_MATERIAL_STDSHADER_DESC)mmobj.pMatPtrList[index1].pMaterial;
                if (pMaterial1 != pMaterial2)
                {
                    AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor1 = pMaterial1.pColor;
                    AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor2 = pMaterial2.pColor;
                    AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC[] pTexDesc1 = pMaterial1.pTexDesc;
                    AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC[] pTexDesc2 = pMaterial2.pTexDesc;
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
                AppMain.NNS_MATERIAL_GLES11_DESC pMaterial1 = (AppMain.NNS_MATERIAL_GLES11_DESC)obj.pMatPtrList[index1].pMaterial;
                AppMain.NNS_MATERIAL_GLES11_DESC pMaterial2 = (AppMain.NNS_MATERIAL_GLES11_DESC)mmobj.pMatPtrList[index1].pMaterial;
                if (pMaterial1 != pMaterial2)
                {
                    AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor1 = pMaterial1.pColor;
                    AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor2 = pMaterial2.pColor;
                    AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[] pTexDesc1 = pMaterial1.pTexDesc;
                    AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[] pTexDesc2 = pMaterial2.pTexDesc;
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
                AppMain.NNS_MATERIAL_DESC pMaterial1 = (AppMain.NNS_MATERIAL_DESC)obj.pMatPtrList[index1].pMaterial;
                AppMain.NNS_MATERIAL_DESC pMaterial2 = (AppMain.NNS_MATERIAL_DESC)mmobj.pMatPtrList[index1].pMaterial;
                if (pMaterial1 != pMaterial2)
                {
                    AppMain.NNS_MATERIAL_COLOR pColor1 = pMaterial1.pColor;
                    AppMain.NNS_MATERIAL_COLOR pColor2 = pMaterial2.pColor;
                    AppMain.NNS_MATERIAL_TEXMAP_DESC[] pTexDesc1 = pMaterial1.pTexDesc;
                    AppMain.NNS_MATERIAL_TEXMAP_DESC[] pTexDesc2 = pMaterial2.pTexDesc;
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
        if (AppMain.nnCalcMotionFrame(out frame, mmot.fType, mmot.StartFrame, mmot.EndFrame, frame) == 0)
            return;
        AppMain.ArrayPointer<AppMain.NNS_SUBMOTION> pSubmotion = (AppMain.ArrayPointer<AppMain.NNS_SUBMOTION>)mmot.pSubmotion;
        int nSubmotion = mmot.nSubmotion;
        for (int index = 0; index < nMaterial; ++index)
        {
            uint fType = obj.pMatPtrList[index].fType;
            if (((int)fType & 2) != 0)
            {
                AppMain.NNS_MATERIAL_STDSHADER_DESC pMaterial = (AppMain.NNS_MATERIAL_STDSHADER_DESC)mmobj.pMatPtrList[index].pMaterial;
                AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor = pMaterial.pColor;
                AppMain.NNS_MATERIAL_STDSHADER_TEXMAP_DESC[] pTexDesc = pMaterial.pTexDesc;
                for (; nSubmotion > 0 && (int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id0 < index; --nSubmotion)
                    ++pSubmotion;
                for (; nSubmotion > 0 && (int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id0 == index; --nSubmotion)
                {
                    switch (((AppMain.NNS_SUBMOTION)~pSubmotion).fType & 4294967040U)
                    {
                        case 512:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.r, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 1024:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.g, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 2048:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.b, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 3584:
                            AppMain.nnInterpolateFloat3(ref pColor.Diffuse, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 4096:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.a, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 8192:
                            AppMain.nnInterpolateFloat(ref pColor.Specular.r, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 16384:
                            AppMain.nnInterpolateFloat(ref pColor.Specular.g, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 32768:
                            AppMain.nnInterpolateFloat(ref pColor.Specular.b, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 57344:
                            AppMain.nnInterpolateFloat3(ref pColor.Specular, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 65536:
                            AppMain.nnInterpolateFloat(ref pColor.SpecularIntensity, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 131072:
                            float val = 0.0f;
                            if (AppMain.nnInterpolateFloat(ref val, (AppMain.NNS_SUBMOTION)pSubmotion, frame) != 0)
                            {
                                pColor.Shininess = AppMain.nnPow(2.0, 10.0 * (double)val + 2.0);
                                break;
                            }
                            break;
                        case 262144:
                            AppMain.nnInterpolateFloat(ref pColor.Ambient.r, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 524288:
                            AppMain.nnInterpolateFloat(ref pColor.Ambient.g, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 1048576:
                            AppMain.nnInterpolateFloat(ref pColor.Ambient.b, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 1835008:
                            AppMain.nnInterpolateFloat3(ref pColor.Ambient, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 2097152:
                            AppMain.nnInterpolateSint32(ref pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].iTexIdx, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 4194304:
                            AppMain.nnInterpolateFloat(ref pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].Blend, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 8388608:
                            AppMain.nnInterpolateFloat(ref pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].Offset.u, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 16777216:
                            AppMain.nnInterpolateFloat(ref pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].Offset.v, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 25165824:
                            AppMain.nnInterpolateFloat2(pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].Offset, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 33554432:
                            AppMain.nnInterpolateUint32(ref pMaterial.User, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                    }
                    ++pSubmotion;
                }
            }
            else if (((int)fType & 8) != 0)
            {
                AppMain.NNS_MATERIAL_GLES11_DESC pMaterial = (AppMain.NNS_MATERIAL_GLES11_DESC)mmobj.pMatPtrList[index].pMaterial;
                AppMain.NNS_MATERIAL_STDSHADER_COLOR pColor = pMaterial.pColor;
                AppMain.NNS_MATERIAL_GLES11_TEXMAP_DESC[] pTexDesc = pMaterial.pTexDesc;
                for (; nSubmotion > 0 && (int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id0 < index; --nSubmotion)
                    ++pSubmotion;
                for (; nSubmotion > 0 && (int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id0 == index; --nSubmotion)
                {
                    switch (((AppMain.NNS_SUBMOTION)~pSubmotion).fType & 4294967040U)
                    {
                        case 512:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.r, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 1024:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.g, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 2048:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.b, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 3584:
                            AppMain.nnInterpolateFloat3(ref pColor.Diffuse, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 4096:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.a, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 8192:
                            AppMain.nnInterpolateFloat(ref pColor.Specular.r, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 16384:
                            AppMain.nnInterpolateFloat(ref pColor.Specular.g, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 32768:
                            AppMain.nnInterpolateFloat(ref pColor.Specular.b, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 57344:
                            AppMain.nnInterpolateFloat3(ref pColor.Specular, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 65536:
                            AppMain.nnInterpolateFloat(ref pColor.SpecularIntensity, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 131072:
                            float val = 0.0f;
                            if (AppMain.nnInterpolateFloat(ref val, (AppMain.NNS_SUBMOTION)pSubmotion, frame) != 0)
                            {
                                pColor.Shininess = AppMain.nnPow(2.0, 10.0 * (double)val + 2.0);
                                break;
                            }
                            break;
                        case 262144:
                            AppMain.nnInterpolateFloat(ref pColor.Ambient.r, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 524288:
                            AppMain.nnInterpolateFloat(ref pColor.Ambient.g, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 1048576:
                            AppMain.nnInterpolateFloat(ref pColor.Ambient.b, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 1835008:
                            AppMain.nnInterpolateFloat3(ref pColor.Ambient, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 2097152:
                            AppMain.nnInterpolateSint32(ref pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].iTexIdx, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 8388608:
                            AppMain.nnInterpolateFloat(ref pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].Offset.u, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 16777216:
                            AppMain.nnInterpolateFloat(ref pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].Offset.v, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 25165824:
                            AppMain.nnInterpolateFloat2(pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].Offset, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 33554432:
                            AppMain.nnInterpolateUint32(ref pMaterial.User, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                    }
                    ++pSubmotion;
                }
            }
            else if (((int)fType & 1) != 0)
            {
                AppMain.NNS_MATERIAL_DESC pMaterial = (AppMain.NNS_MATERIAL_DESC)mmobj.pMatPtrList[index].pMaterial;
                AppMain.NNS_MATERIAL_COLOR pColor = pMaterial.pColor;
                AppMain.NNS_MATERIAL_TEXMAP_DESC[] pTexDesc = pMaterial.pTexDesc;
                for (; nSubmotion > 0 && (int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id0 < index; --nSubmotion)
                    ++pSubmotion;
                for (; nSubmotion > 0 && (int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id0 == index; --nSubmotion)
                {
                    switch (((AppMain.NNS_SUBMOTION)~pSubmotion).fType & 4294967040U)
                    {
                        case 512:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.r, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 1024:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.g, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 2048:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.b, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 3584:
                            AppMain.nnInterpolateFloat3(ref pColor.Diffuse, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 4096:
                            AppMain.nnInterpolateFloat(ref pColor.Diffuse.a, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 8192:
                            AppMain.nnInterpolateFloat(ref pColor.Specular.r, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 16384:
                            AppMain.nnInterpolateFloat(ref pColor.Specular.g, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 32768:
                            AppMain.nnInterpolateFloat(ref pColor.Specular.b, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 57344:
                            AppMain.nnInterpolateFloat3(ref pColor.Specular, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 65536:
                            float val1 = 0.0f;
                            if (AppMain.nnInterpolateFloat(ref val1, (AppMain.NNS_SUBMOTION)pSubmotion, frame) != 0)
                            {
                                pColor.Specular.r *= val1;
                                pColor.Specular.g *= val1;
                                pColor.Specular.b *= val1;
                                break;
                            }
                            break;
                        case 131072:
                            float val2 = 0.0f;
                            if (AppMain.nnInterpolateFloat(ref val2, (AppMain.NNS_SUBMOTION)pSubmotion, frame) != 0)
                            {
                                pColor.Shininess = AppMain.nnPow(2.0, 10.0 * (double)val2 + 2.0);
                                break;
                            }
                            break;
                        case 262144:
                            AppMain.nnInterpolateFloat(ref pColor.Ambient.r, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 524288:
                            AppMain.nnInterpolateFloat(ref pColor.Ambient.g, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 1048576:
                            AppMain.nnInterpolateFloat(ref pColor.Ambient.b, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 1835008:
                            AppMain.nnInterpolateFloat3(ref pColor.Ambient, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 2097152:
                            AppMain.nnInterpolateSint32(ref pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].iTexIdx, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 4194304:
                            float val3 = 0.0f;
                            if (AppMain.nnInterpolateFloat(ref val3, (AppMain.NNS_SUBMOTION)pSubmotion, frame) != 0)
                            {
                                pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].EnvColor.r = val3;
                                pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].EnvColor.g = val3;
                                pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].EnvColor.b = val3;
                                break;
                            }
                            break;
                        case 8388608:
                            AppMain.nnInterpolateFloat(ref pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].Offset.u, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 16777216:
                            AppMain.nnInterpolateFloat(ref pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].Offset.v, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 25165824:
                            AppMain.nnInterpolateFloat2(pTexDesc[(int)((AppMain.NNS_SUBMOTION)~pSubmotion).Id1].Offset, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                        case 33554432:
                            AppMain.nnInterpolateUint32(ref pMaterial.User, (AppMain.NNS_SUBMOTION)pSubmotion, frame);
                            break;
                    }
                    ++pSubmotion;
                }
            }
        }
    }

    public static void nnDrawMaterialMotionObject(
      AppMain.NNS_OBJECT mmobj,
      AppMain.NNS_MATRIX[] mtxpal,
      uint[] nodestatlist,
      uint subobjtype,
      uint flag)
    {
        AppMain.nnDrawObject(mmobj, mtxpal, nodestatlist, subobjtype, flag, 0U);
    }

    private static uint nnCalcClipBoxNode(AppMain.NNS_NODE node, AppMain.NNS_MATRIX mtx)
    {
        float boundingBoxX = node.BoundingBoxX;
        float boundingBoxY = node.BoundingBoxY;
        float boundingBoxZ = node.BoundingBoxZ;
        return AppMain.nnCalcClipBox(node.Center, boundingBoxX, boundingBoxY, boundingBoxZ, mtx);
    }

    private static uint nnCalcClipBox(
      AppMain.NNS_VECTOR center,
      float sx,
      float sy,
      float sz,
      AppMain.NNS_MATRIX mtx)
    {
        uint num1 = 0;
        float nClip = AppMain.nngClip3d.n_clip;
        float fClip = AppMain.nngClip3d.f_clip;
        AppMain.NNS_VECTORFAST dst1;
        AppMain.nnmSetUpVectorFast(out dst1, center.x, center.y, center.z);
        AppMain.NNS_VECTORFAST dst2;
        AppMain.nnTransformVectorFast(out dst2, mtx, dst1);
        float num2 = sx * mtx.M20;
        float num3 = sy * mtx.M21;
        float num4 = sz * mtx.M22;
        float num5 = AppMain.nnAbs((double)num2) + AppMain.nnAbs((double)num3) + AppMain.nnAbs((double)num4);
        if ((double)dst2.z > -(double)nClip + (double)num5 || (double)dst2.z < -(double)fClip - (double)num5)
            return 16;
        if ((double)dst2.z > -(double)nClip - (double)num5)
            num1 |= 260U;
        if ((double)dst2.z < -(double)fClip + (double)num5)
            num1 |= 520U;
        float num6 = sx * mtx.M00;
        float num7 = sy * mtx.M01;
        float num8 = sz * mtx.M02;
        if (AppMain.nngProjectionType != 1)
        {
            float num9 = AppMain.nnAbs((double)num6 * (double)AppMain.nngClipPlane.Right.nx + (double)num2 * (double)AppMain.nngClipPlane.Right.nz) + AppMain.nnAbs((double)num7 * (double)AppMain.nngClipPlane.Right.nx + (double)num3 * (double)AppMain.nngClipPlane.Right.nz) + AppMain.nnAbs((double)num8 * (double)AppMain.nngClipPlane.Right.nx + (double)num4 * (double)AppMain.nngClipPlane.Right.nz);
            float num10 = (float)((double)dst2.x * (double)AppMain.nngClipPlane.Right.nx + (double)dst2.z * (double)AppMain.nngClipPlane.Right.nz);
            if ((double)num10 > (double)num9)
                return 16;
            if ((double)num10 > -(double)num9)
                num1 |= 4096U;
            float num11 = AppMain.nnAbs((double)num6 * (double)AppMain.nngClipPlane.Left.nx + (double)num2 * (double)AppMain.nngClipPlane.Left.nz) + AppMain.nnAbs((double)num7 * (double)AppMain.nngClipPlane.Left.nx + (double)num3 * (double)AppMain.nngClipPlane.Left.nz) + AppMain.nnAbs((double)num8 * (double)AppMain.nngClipPlane.Left.nx + (double)num4 * (double)AppMain.nngClipPlane.Left.nz);
            float num12 = (float)((double)dst2.x * (double)AppMain.nngClipPlane.Left.nx + (double)dst2.z * (double)AppMain.nngClipPlane.Left.nz);
            if ((double)num12 > (double)num11)
                return 16;
            if ((double)num12 > -(double)num11)
                num1 |= 8192U;
            float num13 = sx * mtx.M10;
            float num14 = sy * mtx.M11;
            float num15 = sz * mtx.M12;
            float num16 = AppMain.nnAbs((double)num13 * (double)AppMain.nngClipPlane.Top.ny + (double)num2 * (double)AppMain.nngClipPlane.Top.nz) + AppMain.nnAbs((double)num14 * (double)AppMain.nngClipPlane.Top.ny + (double)num3 * (double)AppMain.nngClipPlane.Top.nz) + AppMain.nnAbs((double)num15 * (double)AppMain.nngClipPlane.Top.ny + (double)num4 * (double)AppMain.nngClipPlane.Top.nz);
            float num17 = (float)((double)dst2.y * (double)AppMain.nngClipPlane.Top.ny + (double)dst2.z * (double)AppMain.nngClipPlane.Top.nz);
            if ((double)num17 > (double)num16)
                return 16;
            if ((double)num17 > -(double)num16)
                num1 |= 16384U;
            float num18 = AppMain.nnAbs((double)num13 * (double)AppMain.nngClipPlane.Bottom.ny + (double)num2 * (double)AppMain.nngClipPlane.Bottom.nz) + AppMain.nnAbs((double)num14 * (double)AppMain.nngClipPlane.Bottom.ny + (double)num3 * (double)AppMain.nngClipPlane.Bottom.nz) + AppMain.nnAbs((double)num15 * (double)AppMain.nngClipPlane.Bottom.ny + (double)num4 * (double)AppMain.nngClipPlane.Bottom.nz);
            float num19 = (float)((double)dst2.y * (double)AppMain.nngClipPlane.Bottom.ny + (double)dst2.z * (double)AppMain.nngClipPlane.Bottom.nz);
            if ((double)num19 > (double)num18)
                return 16;
            if ((double)num19 > -(double)num18)
                num1 |= 32768U;
            if (num1 == 0U)
                return 2;
        }
        else
        {
            float num9 = AppMain.nnAbs((double)num6) + AppMain.nnAbs((double)num7) + AppMain.nnAbs((double)num8);
            float num10 = dst2.x - AppMain.nngClipPlane.Right.mul - AppMain.nngClipPlane.Right.ofs;
            if ((double)num10 > (double)num9)
                return 16;
            if ((double)num10 > -(double)num9)
                num1 |= 4096U;
            float num11 = dst2.x - AppMain.nngClipPlane.Left.mul - AppMain.nngClipPlane.Left.ofs;
            if ((double)num11 < -(double)num9)
                return 16;
            if ((double)num11 < (double)num9)
                num1 |= 8192U;
            float num12 = AppMain.nnAbs((double)(sx * mtx.M10)) + AppMain.nnAbs((double)(sy * mtx.M11)) + AppMain.nnAbs((double)(sz * mtx.M12));
            float num13 = dst2.y - AppMain.nngClipPlane.Top.mul - AppMain.nngClipPlane.Top.ofs;
            if ((double)num13 > (double)num12)
                return 16;
            if ((double)num13 > -(double)num12)
                num1 |= 16384U;
            float num14 = dst2.y - AppMain.nngClipPlane.Bottom.mul - AppMain.nngClipPlane.Bottom.ofs;
            if ((double)num14 < -(double)num12)
                return 16;
            if ((double)num14 < (double)num12)
                num1 |= 32768U;
            if (num1 == 0U)
                return 2;
        }
        return num1 & 62U;
    }

    private static uint nnCalcClipCore(
      AppMain.NNS_VECTOR center,
      float radius,
      AppMain.NNS_MATRIX mtx)
    {
        uint num1 = 0;
        float nClip = AppMain.nngClip3d.n_clip;
        float fClip = AppMain.nngClip3d.f_clip;
        AppMain.NNS_VECTORFAST dst1;
        AppMain.nnmSetUpVectorFast(out dst1, center.x, center.y, center.z);
        AppMain.NNS_VECTORFAST dst2;
        AppMain.nnTransformVectorFast(out dst2, mtx, dst1);
        if ((double)dst2.z > -(double)nClip + (double)radius || (double)dst2.z < -(double)fClip - (double)radius)
            return 16;
        if ((double)dst2.z > -(double)nClip - (double)radius)
            num1 |= 260U;
        if ((double)dst2.z < -(double)fClip + (double)radius)
            num1 |= 520U;
        if (AppMain.nngProjectionType != 1)
        {
            float num2 = (float)((double)dst2.x * (double)AppMain.nngClipPlane.Right.nx + (double)dst2.z * (double)AppMain.nngClipPlane.Right.nz);
            if ((double)num2 > (double)radius)
                return 16;
            if ((double)num2 > -(double)radius)
                num1 |= 4096U;
            float num3 = (float)((double)dst2.x * (double)AppMain.nngClipPlane.Left.nx + (double)dst2.z * (double)AppMain.nngClipPlane.Left.nz);
            if ((double)num3 > (double)radius)
                return 16;
            if ((double)num3 > -(double)radius)
                num1 |= 8192U;
            float num4 = (float)((double)dst2.y * (double)AppMain.nngClipPlane.Top.ny + (double)dst2.z * (double)AppMain.nngClipPlane.Top.nz);
            if ((double)num4 > (double)radius)
                return 16;
            if ((double)num4 > -(double)radius)
                num1 |= 16384U;
            float num5 = (float)((double)dst2.y * (double)AppMain.nngClipPlane.Bottom.ny + (double)dst2.z * (double)AppMain.nngClipPlane.Bottom.nz);
            if ((double)num5 > (double)radius)
                return 16;
            if ((double)num5 > -(double)radius)
                num1 |= 32768U;
            if (num1 == 0U)
                return 2;
        }
        else
        {
            float num2 = dst2.y - AppMain.nngClipPlane.Top.mul - AppMain.nngClipPlane.Top.ofs;
            if ((double)num2 > (double)radius)
                return 16;
            if ((double)num2 > -(double)radius)
                num1 |= 16384U;
            float num3 = dst2.y - AppMain.nngClipPlane.Bottom.mul - AppMain.nngClipPlane.Bottom.ofs;
            if ((double)num3 < -(double)radius)
                return 16;
            if ((double)num3 < (double)radius)
                num1 |= 32768U;
            float num4 = dst2.x - AppMain.nngClipPlane.Right.mul - AppMain.nngClipPlane.Right.ofs;
            if ((double)num4 > (double)radius)
                return 16;
            if ((double)num4 > -(double)radius)
                num1 |= 4096U;
            float num5 = dst2.x - AppMain.nngClipPlane.Left.mul - AppMain.nngClipPlane.Left.ofs;
            if ((double)num5 < -(double)radius)
                return 16;
            if ((double)num5 < (double)radius)
                num1 |= 8192U;
            if (num1 == 0U)
                return 2;
        }
        return num1 & 62U;
    }

    private static uint nnCalcClip(AppMain.NNS_VECTOR center, float radius, AppMain.NNS_MATRIX mtx)
    {
        if ((double)radius == 0.0)
            return 0;
        radius *= AppMain.nnEstimateMatrixScaling(mtx);
        return AppMain.nnCalcClipCore(center, radius, mtx);
    }

    private static uint nnCalcClipUniformScale(
      AppMain.NNS_VECTOR center,
      float radius,
      AppMain.NNS_MATRIX mtx,
      float factor)
    {
        if ((double)radius == 0.0)
            return 0;
        radius *= factor;
        return AppMain.nnCalcClipCore(center, radius, mtx);
    }

    private static void nnCalcClipSetNodeStatus(
      uint[] pNodeStatList,
      AppMain.NNS_NODE[] pNodeList,
      int nodeIdx,
      AppMain.NNS_MATRIX pNodeMtx,
      float rootscale,
      uint flag)
    {
        AppMain.nnclip.nnsNodeStatList = pNodeStatList;
        AppMain.nnclip.nnsNodeList = pNodeList;
        AppMain.NNS_NODE pNode = pNodeList[nodeIdx];
        if (((int)pNodeStatList[nodeIdx] & 1025) != 0)
            return;
        if (((int)pNode.fType & 16) != 0)
            pNodeStatList[nodeIdx] |= 1U;
        if (((int)pNode.fType & 32) != 0)
        {
            pNodeStatList[nodeIdx] |= 1U;
            if (pNode.iChild == (short)-1)
                return;
            AppMain.nnSetUpNodeStatusListFlag((int)pNode.iChild, 1U);
        }
        else
        {
            if (flag == 0U || pNode.iMatrix == (short)-1 && ((int)flag & 8) == 0)
                return;
            if (((int)pNode.fType & 2097152) != 0 && ((int)pNode.fType & 4194304) == 0 && ((int)flag & 32) == 0)
                pNodeStatList[nodeIdx] |= AppMain.nnCalcClipBoxNode(pNode, pNodeMtx);
            else if (((int)flag & 16) != 0)
                pNodeStatList[nodeIdx] |= AppMain.nnCalcClip(pNode.Center, pNode.Radius, pNodeMtx);
            else
                pNodeStatList[nodeIdx] |= AppMain.nnCalcClipUniformScale(pNode.Center, pNode.Radius, pNodeMtx, rootscale);
            if (((int)pNodeStatList[nodeIdx] & 16) == 0)
                return;
            if (((int)flag & 2) != 0)
                pNodeStatList[nodeIdx] |= 1024U;
            else
                pNodeStatList[nodeIdx] |= 1U;
            if (((int)flag & 8) == 0 || pNode.iChild == (short)-1)
                return;
            AppMain.nnSetUpNodeStatusListFlag((int)pNode.iChild, pNodeStatList[nodeIdx]);
        }
    }

    private static void nnSetUpNodeStatusListFlag(int nodeidx, uint flag)
    {
        AppMain.mppAssertNotImpl();
    }

    private uint nnCheckObjectClip(AppMain.NNS_OBJECT obj, AppMain.NNS_MATRIX basemtx)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private uint nnCheckObjectClipMotionCore(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mot,
      float frame,
      AppMain.NNS_MATRIX basemtx)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

    private uint nnCheckObjectClipMotion(
      AppMain.NNS_OBJECT obj,
      AppMain.NNS_MOTION mot,
      float frame,
      AppMain.NNS_MATRIX basemtx)
    {
        AppMain.mppAssertNotImpl();
        return uint.MaxValue;
    }

    private static float nnEstimateMatrixScaling(AppMain.NNS_MATRIX mtx)
    {
        AppMain.NNS_VECTORFAST dst1;
        AppMain.nnSetUpVectorFast(out dst1, mtx.M00, mtx.M10, mtx.M20);
        AppMain.NNS_VECTORFAST dst2;
        AppMain.nnSetUpVectorFast(out dst2, mtx.M01, mtx.M11, mtx.M21);
        AppMain.NNS_VECTORFAST dst3;
        AppMain.nnSetUpVectorFast(out dst3, mtx.M02, mtx.M12, mtx.M22);
        return AppMain.nnSqrt(AppMain.NNM_MAX(AppMain.NNM_MAX(AppMain.nnLengthSqVectorFast(ref dst1), AppMain.nnLengthSqVectorFast(ref dst2)), AppMain.nnLengthSqVectorFast(ref dst3)) + 2f * AppMain.NNM_MAX(AppMain.NNM_MAX(AppMain.nnAbs((double)AppMain.nnDotProductVectorFast(ref dst1, ref dst2)), AppMain.nnAbs((double)AppMain.nnDotProductVectorFast(ref dst1, ref dst3))), AppMain.nnAbs((double)AppMain.nnDotProductVectorFast(ref dst2, ref dst3))));
    }

    public static uint NND_DRAWOBJ_SHADER_USER_PROFILE(byte _n)
    {
        return (uint)(((int)_n & (int)byte.MaxValue) << 2);
    }

    private static void nnSetMaterialCallback(AppMain.NNS_MATERIALCALLBACK_FUNC func)
    {
        AppMain.nngMaterialCallbackFunc = func;
    }

    private static AppMain.NNS_MATERIALCALLBACK_FUNC nnGetMaterialCallback()
    {
        return AppMain.nngMaterialCallbackFunc;
    }

    private static int nnPutMaterial(AppMain.NNS_DRAWCALLBACK_VAL val)
    {
        return AppMain.nngMaterialCallbackFunc != null ? AppMain.nngMaterialCallbackFunc(val) : AppMain.nnPutMaterialCore(val);
    }

}