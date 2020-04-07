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
    public static double floorf(double f)
    {
        return Math.Floor(f);
    }

    public static float floorf(float f)
    {
        return (float)Math.Floor((double)f);
    }

    public static float nnRandom()
    {
        return (float)AppMain.random.Next(0, 32768) * 3.051758E-05f;
    }

    public static void nnRandomSeed(int n)
    {
        AppMain.random = new Random(n);
    }

    public static float nnAbs(double n)
    {
        return (float)Math.Abs(n);
    }

    public static int nnArcCos(double n)
    {
        return AppMain.NNM_RADtoA32((float)Math.Acos(n));
    }

    public static int nnArcSin(double n)
    {
        return AppMain.NNM_RADtoA32((float)Math.Asin(n));
    }

    public static int nnArcTan(double n)
    {
        return AppMain.NNM_RADtoA32((float)Math.Atan(n));
    }

    public static int nnArcTan2(double y, double x)
    {
        return AppMain.NNM_RADtoA32((float)Math.Atan2(y, x));
    }

    public static float nnExp(double x)
    {
        return (float)Math.Exp(x);
    }

    public static float nnFloor(double n)
    {
        return (float)Math.Floor(n);
    }

    public static float nnFraction(double n)
    {
        AppMain.mppAssertNotImpl();
        return n > 0.0 ? (float)(n - Math.Floor(n)) : (float)(n - Math.Ceiling(n));
    }

    public static float nnHypot(double x, double y)
    {
        return (float)Math.Sqrt(x * x + y * y);
    }

    public static float nnInvertSqrt(float n)
    {
        return (float)(1.0 / Math.Sqrt((double)n));
    }

    public static float nnLog(double n)
    {
        return (float)Math.Log(n);
    }

    public static float nnLog10(double n)
    {
        return (float)Math.Log10(n);
    }

    public static float nnPow(double n1, double n2)
    {
        return (float)Math.Pow(n1, n2);
    }

    public static float nnSqrt(float n)
    {
        return (float)Math.Sqrt((double)n);
    }

    public static float nnTan(int ang)
    {
        return (float)Math.Tan((double)AppMain.NNM_A32toRAD(ang));
    }

    private static float nnRoundOff(float n)
    {
        return (double)n >= 0.0 ? AppMain.nnFloor((double)AppMain.nnAbs((double)n)) : -AppMain.nnFloor((double)AppMain.nnAbs((double)n));
    }

    private static float amSystemGetFrameRateMain()
    {
        return AppMain._am_framerate_main;
    }

    public static short AKM_DEGtoA16(float n)
    {
        return (short)((long)ushort.MaxValue & (long)(int)((double)n * 182.04443359375));
    }

    public static int AKM_DEGtoA32(float n)
    {
        return AppMain.NNM_DEGtoA32(n);
    }

    public static short AKM_DEGtoA16(int n)
    {
        return (short)((long)ushort.MaxValue & (long)(int)((double)n * 182.04443359375));
    }

    public static int AKM_DEGtoA32(int n)
    {
        return AppMain.NNM_DEGtoA32(n);
    }

    public static int AkMathRandFx()
    {
        return (int)AppMain.mtMathRand() >> 4;
    }

    public static void AkMathGetRandomUnitVector(
      AppMain.NNS_VECTOR dst_vec,
      float rand_z,
      short rand_angle)
    {
        dst_vec.x = AppMain.nnSqrt((float)(1.0 - (double)rand_z * (double)rand_z)) * AppMain.nnCos((int)rand_angle);
        dst_vec.y = AppMain.nnSqrt((float)(1.0 - (double)rand_z * (double)rand_z)) * AppMain.nnSin((int)rand_angle);
        dst_vec.z = rand_z;
    }

    public static void AkMathNormalizeMtx(AppMain.NNS_MATRIX dst_mtx, AppMain.NNS_MATRIX src_mtx)
    {
        AppMain.NNS_VECTOR nnsVector1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector3 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.amVectorSet(nnsVector1, src_mtx.M00, src_mtx.M01, src_mtx.M02);
        AppMain.amVectorSet(nnsVector2, src_mtx.M10, src_mtx.M11, src_mtx.M12);
        AppMain.amVectorSet(nnsVector3, src_mtx.M20, src_mtx.M21, src_mtx.M22);
        AppMain.nnMakeUnitMatrix(dst_mtx);
        float num1 = 1f / AppMain.nnLengthVector(nnsVector1);
        dst_mtx.M00 = nnsVector1.x * num1;
        dst_mtx.M01 = nnsVector1.y * num1;
        dst_mtx.M02 = nnsVector1.z * num1;
        float num2 = 1f / AppMain.nnLengthVector(nnsVector2);
        dst_mtx.M10 = nnsVector2.x * num2;
        dst_mtx.M11 = nnsVector2.y * num2;
        dst_mtx.M12 = nnsVector2.z * num2;
        float num3 = 1f / AppMain.nnLengthVector(nnsVector3);
        dst_mtx.M20 = nnsVector3.x * num3;
        dst_mtx.M21 = nnsVector3.y * num3;
        dst_mtx.M22 = nnsVector3.z * num3;
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector2);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector3);
    }

    private static void AkMathExtractScaleMtx(AppMain.NNS_MATRIX dst_mtx, AppMain.NNS_MATRIX src_mtx)
    {
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.amAssert(true);
        AppMain.amVectorSet(nnsVector, src_mtx.M(0, 0), src_mtx.M(0, 1), src_mtx.M(0, 2));
        float x = AppMain.nnLengthVector(nnsVector);
        AppMain.amVectorSet(nnsVector, src_mtx.M(1, 0), src_mtx.M(1, 1), src_mtx.M(1, 2));
        float y = AppMain.nnLengthVector(nnsVector);
        AppMain.amVectorSet(nnsVector, src_mtx.M(2, 0), src_mtx.M(2, 1), src_mtx.M(2, 2));
        float z = AppMain.nnLengthVector(nnsVector);
        AppMain.nnMakeScaleMatrix(dst_mtx, x, y, z);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    public static void AkMathInvertYZQuaternion(
      out AppMain.NNS_QUATERNION dst_quat,
      ref AppMain.NNS_QUATERNION src_quat)
    {
        dst_quat = src_quat;
        dst_quat.y = -dst_quat.y;
        dst_quat.z = -dst_quat.z;
    }

    public static void AkMathInvertXZQuaternion(
      out AppMain.NNS_QUATERNION dst_quat,
      ref AppMain.NNS_QUATERNION src_quat)
    {
        dst_quat = src_quat;
        dst_quat.x = -dst_quat.x;
        dst_quat.z = -dst_quat.z;
    }

    public static void AkMathInvertXYQuaternion(
      out AppMain.NNS_QUATERNION dst_quat,
      ref AppMain.NNS_QUATERNION src_quat)
    {
        dst_quat = src_quat;
        dst_quat.x = -dst_quat.x;
        dst_quat.y = -dst_quat.y;
    }

    private static byte AkMathCountBitPopulation(uint bits)
    {
        bits -= bits >> 1 & 1431655765U;
        bits = (uint)(((int)bits & 858993459) + ((int)(bits >> 2) & 858993459));
        bits = (uint)((int)bits + (int)(bits >> 4) & 252645135);
        bits += bits >> 8;
        bits += bits >> 16;
        return (byte)(bits & 63U);
    }
}