using System;

public partial class AppMain
{
    public static double floorf(double f)
    {
        return Math.Floor(f);
    }

    public static float floorf(float f)
    {
        return (float)Math.Floor(f);
    }

    public static float nnRandom()
    {
        return (float)random.NextDouble();
    }

    public static void nnRandomSeed(int n)
    {
        random = new Random(n);
    }

    public static float nnAbs(double n)
    {
        return (float)Math.Abs(n);
    }

    public static int nnArcCos(double n)
    {
        return NNM_RADtoA32((float)Math.Acos(n));
    }

    public static int nnArcSin(double n)
    {
        return NNM_RADtoA32((float)Math.Asin(n));
    }

    public static int nnArcTan(double n)
    {
        return NNM_RADtoA32((float)Math.Atan(n));
    }

    public static int nnArcTan2(double y, double x)
    {
        return NNM_RADtoA32((float)Math.Atan2(y, x));
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
        mppAssertNotImpl();
        return n > 0.0 ? (float)(n - Math.Floor(n)) : (float)(n - Math.Ceiling(n));
    }

    public static float nnHypot(double x, double y)
    {
        return (float)Math.Sqrt(x * x + y * y);
    }

    public static float nnInvertSqrt(float n)
    {
        return (float)(1.0 / Math.Sqrt(n));
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
        return (float)Math.Sqrt(n);
    }

    public static float nnTan(int ang)
    {
        return (float)Math.Tan(NNM_A32toRAD(ang));
    }

    private static float nnRoundOff(float n)
    {
        return n >= 0.0 ? nnFloor(nnAbs(n)) : -nnFloor(nnAbs(n));
    }

    private static float amSystemGetFrameRateMain()
    {
        return _am_framerate_main;
    }

    public static short AKM_DEGtoA16(float n)
    {
        return (short)(ushort.MaxValue & (long)(int)(n * 182.04443359375));
    }

    public static int AKM_DEGtoA32(float n)
    {
        return NNM_DEGtoA32(n);
    }

    public static short AKM_DEGtoA16(int n)
    {
        return (short)(ushort.MaxValue & (long)(int)(n * 182.04443359375));
    }

    public static int AKM_DEGtoA32(int n)
    {
        return NNM_DEGtoA32(n);
    }

    public static int AkMathRandFx()
    {
        return mtMathRand() >> 4;
    }

    public static void AkMathGetRandomUnitVector(
      NNS_VECTOR dst_vec,
      float rand_z,
      short rand_angle)
    {
        dst_vec.x = nnSqrt((float)(1.0 - rand_z * (double)rand_z)) * nnCos(rand_angle);
        dst_vec.y = nnSqrt((float)(1.0 - rand_z * (double)rand_z)) * nnSin(rand_angle);
        dst_vec.z = rand_z;
    }

    public static void AkMathNormalizeMtx(NNS_MATRIX dst_mtx, NNS_MATRIX src_mtx)
    {
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector3 = GlobalPool<NNS_VECTOR>.Alloc();
        amVectorSet(nnsVector1, src_mtx.M00, src_mtx.M01, src_mtx.M02);
        amVectorSet(nnsVector2, src_mtx.M10, src_mtx.M11, src_mtx.M12);
        amVectorSet(nnsVector3, src_mtx.M20, src_mtx.M21, src_mtx.M22);
        nnMakeUnitMatrix(dst_mtx);
        float num1 = 1f / nnLengthVector(nnsVector1);
        dst_mtx.M00 = nnsVector1.x * num1;
        dst_mtx.M01 = nnsVector1.y * num1;
        dst_mtx.M02 = nnsVector1.z * num1;
        float num2 = 1f / nnLengthVector(nnsVector2);
        dst_mtx.M10 = nnsVector2.x * num2;
        dst_mtx.M11 = nnsVector2.y * num2;
        dst_mtx.M12 = nnsVector2.z * num2;
        float num3 = 1f / nnLengthVector(nnsVector3);
        dst_mtx.M20 = nnsVector3.x * num3;
        dst_mtx.M21 = nnsVector3.y * num3;
        dst_mtx.M22 = nnsVector3.z * num3;
        GlobalPool<NNS_VECTOR>.Release(nnsVector1);
        GlobalPool<NNS_VECTOR>.Release(nnsVector2);
        GlobalPool<NNS_VECTOR>.Release(nnsVector3);
    }

    private static void AkMathExtractScaleMtx(NNS_MATRIX dst_mtx, NNS_MATRIX src_mtx)
    {
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        amAssert(true);
        amVectorSet(nnsVector, src_mtx.M(0, 0), src_mtx.M(0, 1), src_mtx.M(0, 2));
        float x = nnLengthVector(nnsVector);
        amVectorSet(nnsVector, src_mtx.M(1, 0), src_mtx.M(1, 1), src_mtx.M(1, 2));
        float y = nnLengthVector(nnsVector);
        amVectorSet(nnsVector, src_mtx.M(2, 0), src_mtx.M(2, 1), src_mtx.M(2, 2));
        float z = nnLengthVector(nnsVector);
        nnMakeScaleMatrix(dst_mtx, x, y, z);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    public static void AkMathInvertYZQuaternion(
      out NNS_QUATERNION dst_quat,
      ref NNS_QUATERNION src_quat)
    {
        dst_quat = src_quat;
        dst_quat.y = -dst_quat.y;
        dst_quat.z = -dst_quat.z;
    }

    public static void AkMathInvertXZQuaternion(
      out NNS_QUATERNION dst_quat,
      ref NNS_QUATERNION src_quat)
    {
        dst_quat = src_quat;
        dst_quat.x = -dst_quat.x;
        dst_quat.z = -dst_quat.z;
    }

    public static void AkMathInvertXYQuaternion(
      out NNS_QUATERNION dst_quat,
      ref NNS_QUATERNION src_quat)
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