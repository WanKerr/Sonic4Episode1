using System;

public partial class AppMain
{
    public const int AMD_TRAIL_SAMPLING_NUM = 4;
    public const int AMD_FX32_ONE = 4096;
    public const float AMD_MATH_E = 2.718282f;
    public const float AMD_MATH_LOG2E = 1.442695f;
    public const float AMD_MATH_LOG10E = 0.4342945f;
    public const float AMD_MATH_LN2 = 0.6931472f;
    public const float AMD_MATH_LN10 = 2.302585f;
    public const float AMD_MATH_PI = 3.141593f;
    public const float AMD_MATH_2PI = 6.283185f;
    public const float AMD_MATH_PI_2 = 1.570796f;
    public const float AMD_MATH_PI_4 = 0.7853982f;
    public const float AMD_MATH_3PI_4 = 2.356194f;
    public const float AMD_MATH_SQRTPI = 1.772454f;
    public const float AMD_MATH_1_PI = 0.3183099f;
    public const float AMD_MATH_2_PI = 0.6366197f;
    public const float AMD_MATH_2_SQRTPI = 1.128379f;
    public const float AMD_MATH_SQRT2 = 1.414214f;
    public const float AMD_MATH_1_SQRT2 = 0.7071068f;
    public const float AMD_MATH_SQRT3 = 1.732051f;
    public const float AMD_MATH_1_LN2 = 1.442695f;
    public const float AMD_MATH_1_LN10 = 0.4342945f;
    public const float AMD_MATH_LOG10TWO = 0.30103f;
    public const float AMD_MATH_LOG2TEN = 3.321928f;
    public const float AMD_MATH_SQRT3_2 = 0.8660254f;
    public const float AMD_MATH_S8TOFLOAT = 0.003921569f;

    private static float AMD_FX32_TO_FLOAT(int a)
    {
        return a / 4096f;
    }

    private static float AMD_FX32_TO_FLOAT(float a)
    {
        return a / 4096f;
    }

    public static uint AMD_RGBA8888(uint r, uint g, uint b, uint a)
    {
        return (uint)(((int)r & byte.MaxValue) << 24 | ((int)g & byte.MaxValue) << 16 | ((int)b & byte.MaxValue) << 8 | (int)a & byte.MaxValue);
    }

    public static uint AMD_RGBA8888(byte r, byte g, byte b, byte a)
    {
        return (uint)((r & byte.MaxValue) << 24 | (g & byte.MaxValue) << 16 | (b & byte.MaxValue) << 8 | a & byte.MaxValue);
    }

    public static void amFlagOn(ref uint dst_, uint on_)
    {
        dst_ |= on_;
    }

    public static void amFlagOff(ref uint dst_, uint off_)
    {
        dst_ &= ~off_;
    }

    public static void amFlagFlip(ref uint dst_, uint flip_)
    {
        dst_ ^= flip_;
    }

    public static void amFlagOnOff(ref uint dst_, uint on_, uint off_)
    {
        dst_ = dst_ & ~off_ | on_;
    }

    public static float amSqrt(float fs_)
    {
        return nnSqrt(fs_);
    }

    public static float amPow2(float n_)
    {
        return n_ * n_;
    }

    public static float amPow3(float n_)
    {
        return n_ * n_ * n_;
    }

    public static bool amIsZerof(float fs)
    {
        return 0.0 >= fs && fs >= 0.0;
    }

    public static float amClamp(float n_, float min_, float max_)
    {
        if (n_ < (double)min_)
            return min_;
        return n_ <= (double)max_ ? n_ : max_;
    }

    public static void amSinCos(float radian, out float pSn, out float pCs)
    {
        nnSinCos(NNM_RADtoA32(radian), out pSn, out pCs);
    }

    public static void amSinCos(int angle, out float pSn, out float pCs)
    {
        nnSinCos(angle, out pSn, out pCs);
    }

    public static T amMax<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) <= 0 ? b : a;
    }

    public static T amMin<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) >= 0 ? b : a;
    }

    public static int FX_MUL(int v1, int v2)
    {
        return FX32_CAST(v1 * (long)v2 + 2048L >> 12);
    }

    public static int FX_MUL32x64C(int v1, long v2)
    {
        return FX32_CAST(v2 * v1 + 2147483648L >> 32);
    }

    public static float FX_FX32_TO_F32(int x)
    {
        return x / 4096f;
    }

    public static int FX_F32_TO_FX32(double x)
    {
        return x > 0.0 ? (int)(x * 4096.0 + 0.5) : (int)(x * 4096.0 - 0.5);
    }

    public static int FX_F32_TO_FX32(float x)
    {
        return x > 0.0 ? (int)(x * 4096.0 + 0.5) : (int)(x * 4096.0 - 0.5);
    }

    public static int FX32_CONST(float x)
    {
        return FX_F32_TO_FX32(x);
    }

    public static float FX_FX64_TO_F32(long x)
    {
        return x / 4096f;
    }

    public static long FX_F32_TO_FX64(float x)
    {
        return x > 0.0 ? (long)(x * 4096.0 + 0.5) : (long)(x * 4096.0 - 0.5);
    }

    public static long FX64_CONST(float x)
    {
        return FX_F32_TO_FX64(x);
    }

    public static float FX_FX64C_TO_F32(long x)
    {
        return x / (float)uint.MaxValue;
    }

    public static long FX_F32_TO_FX64C(float x)
    {
        return x > 0.0 ? (long)(x * 4294967296.0 + 0.5) : (long)(x * 4294967296.0 - 0.5);
    }

    public static long FX64C_CONST(float x)
    {
        return FX_F32_TO_FX64C(x);
    }

    public static float FX_FX16_TO_F32(short x)
    {
        return x / 4096f;
    }

    public static short FX_F32_TO_FX16(float x)
    {
        return x <= 0.0 ? (short)(x * 4096.0 - 0.5) : (short)(x * 4096.0 + 0.5);
    }

    public static short FX16_CONST(float x)
    {
        return FX_F32_TO_FX16(x);
    }

    public static float FXM_FX32_TO_FLOAT(int a)
    {
        return a / 4096f;
    }

    public static int FXM_FLOAT_TO_FX32(float a)
    {
        return (int)(a * 4096.0);
    }

    public static void VEC_Set(ref VecFx32 a, int x, int y, int z)
    {
        a.x = x;
        a.y = y;
        a.z = z;
    }

    public static int FX32_CAST(long res)
    {
        return (int)res;
    }

    public static int FX_Mul(int v1, int v2)
    {
        return FX32_CAST(v1 * (long)v2 + 2048L >> 12);
    }

    public static int FX_Mul32x64c(int v32, long v64c)
    {
        return FX32_CAST(v64c * v32 + 2147483648L >> 32);
    }

    public static int FX_Div(int numer, int denom)
    {
        return (int)(((long)numer << 32) / denom + 524288L >> 20);
    }

    public static int FX_DivS32(int numer, int denom)
    {
        return numer / denom;
    }

    public static long FX_DivFx64c(int numer, int denom)
    {
        return (int)(((long)numer << 32) / denom);
    }

    public static int FX_Mod(int numer, int denom)
    {
        return (int)(((long)numer << 32) % denom + 524288L) >> 20;
    }

    public static int FX_ModS32(int numer, int denom)
    {
        return numer % denom;
    }

    public static int FX_Inv(int denom)
    {
        return FX_Div(4096, denom);
    }

    public static long FX_InvFx64c(int denom)
    {
        return FX_DivFx64c(4096, denom);
    }

    public static int FX_Sqrt(int x)
    {
        return FXM_FLOAT_TO_FX32(nnSqrt(FXM_FX32_TO_FLOAT(x)));
    }

    public static int FX_Sin(int angle)
    {
        return FXM_FLOAT_TO_FX32(nnSin(angle));
    }

    public static int FX_Cos(int angle)
    {
        return FXM_FLOAT_TO_FX32(nnCos(angle));
    }


    public void amQuatToEulerXYZ(
      out float rx,
      out float ry,
      out float rz,
      ref NNS_QUATERNION pQuat)
    {
        rx = ry = rz = 0.0f;
        NNS_QUATERNION pDst = new NNS_QUATERNION();
        amQuatUnit(ref pDst, ref pQuat);
        float num1 = pDst.x * pDst.x;
        float num2 = pDst.x * pDst.y;
        float num3 = pDst.x * pDst.z;
        float num4 = pDst.x * pDst.w;
        float num5 = pDst.y * pDst.y;
        float num6 = pDst.y * pDst.z;
        float num7 = pDst.y * pDst.w;
        float num8 = pDst.z * pDst.z;
        float num9 = pDst.z * pDst.w;
        float n_1 = (float)(1.0 - 2.0 * (num5 + (double)num8));
        float n_2 = (float)(2.0 * (num2 + (double)num9));
        float n_3 = (float)(2.0 * (num3 - (double)num7));
        float num10 = 1f - amPow2(n_3);
        ry = (float)Math.Atan2(-n_3, num10 > 0.0 ? Math.Sqrt(num10) : 0.0);
        float fs = (float)Math.Sqrt(amPow2(n_1) + (double)amPow2(n_2));
        float num11;
        float num12;
        if (amIsZerof(fs))
        {
            num11 = 0.0f;
            num12 = 1f;
        }
        else
        {
            num11 = n_2 / fs;
            num12 = n_1 / fs;
        }
        float num13 = (float)(2.0 * (num2 - (double)num9));
        float num14 = (float)(2.0 * (num3 + (double)num7));
        float num15 = (float)(1.0 - 2.0 * (num1 + (double)num8));
        float num16 = (float)(2.0 * (num6 - (double)num4));
        float num17 = (float)(num14 * (double)num11 - num16 * (double)num12);
        float num18 = (float)(num15 * (double)num12 - num13 * (double)num11);
        rx = (float)Math.Atan2(num17, num18);
        rz = (float)Math.Atan2(num11, num12);
    }

    public void amQuatToEulerXYZ(
      out int ax,
      out int ay,
      out int az,
      ref NNS_QUATERNION pQuat)
    {
        float rx;
        float ry;
        float rz;
        this.amQuatToEulerXYZ(out rx, out ry, out rz, ref pQuat);
        ax = NNM_RADtoA32(rx);
        ay = NNM_RADtoA32(ry);
        az = NNM_RADtoA32(rz);
    }

    public void amQuatVectorToQuat(
      ref NNS_QUATERNION pQuat,
      NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector3 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector4 = GlobalPool<NNS_VECTOR>.Alloc();
        VEC3_COPY(nnsVector1, pV1);
        VEC3_COPY(nnsVector2, pV2);
        nnAddVector(nnsVector4, nnsVector1, nnsVector2);
        float scale = (float)(1.0 / Math.Sqrt(nnDotProductVector(nnsVector4, nnsVector4)));
        nnScaleVector(nnsVector2, nnsVector4, scale);
        nnCrossProductVector(nnsVector3, nnsVector1, nnsVector2);
        VEC3_COPY(pQuat, nnsVector3);
        pQuat.w = nnDotProductVector(nnsVector1, nnsVector2);
        GlobalPool<NNS_VECTOR>.Release(nnsVector1);
        GlobalPool<NNS_VECTOR>.Release(nnsVector2);
        GlobalPool<NNS_VECTOR>.Release(nnsVector3);
        GlobalPool<NNS_VECTOR>.Release(nnsVector4);
    }

    public static void amQuatRotAxisToQuat(
      ref NNS_QUATERNION pQuat,
      NNS_VECTOR4D pVec,
      float radian)
    {
        radian *= 0.5f;
        float pSn;
        float pCs;
        amSinCos(radian, out pSn, out pCs);
        pQuat.x = pVec.x * pSn;
        pQuat.y = pVec.y * pSn;
        pQuat.z = pVec.z * pSn;
        pQuat.w = pCs;
    }

    public static void amQuatToMatrix(
      NNS_MATRIX pMtx,
      ref NNS_QUATERNION pQuat,
      NNS_VECTOR4D pVec)
    {
        if (pMtx == null)
        {
            NNS_MATRIX current = amMatrixGetCurrent();
            nnMakeQuaternionMatrix(out tempSNNS_MATRIX0, ref pQuat);
            if (pVec != null)
                nnCopyVectorMatrixTranslation(ref tempSNNS_MATRIX0, (NNS_VECTOR)pVec);
            nnCopyMatrix(current, ref tempSNNS_MATRIX0);
        }
        else
        {
            nnMakeQuaternionMatrix(pMtx, ref pQuat);
            if (pVec == null)
                return;
            nnCopyVectorMatrixTranslation(ref tempSNNS_MATRIX0, (NNS_VECTOR)pVec);
        }
    }

    public static void amQuatMultiMatrix(ref NNS_QUATERNION pQuat, NNS_VECTOR4D pVec)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnMakeQuaternionMatrix(out tempSNNS_MATRIX0, ref pQuat);
        if (pVec != null)
            nnCopyVectorMatrixTranslation(ref tempSNNS_MATRIX0, pVec);
        nnMultiplyMatrix(current, current, ref tempSNNS_MATRIX0);
    }

    public static void amQuatMultiMatrix(
      ref NNS_QUATERNION pQuat,
      ref SNNS_VECTOR4D pVec)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnMakeQuaternionMatrix(out tempSNNS_MATRIX0, ref pQuat);
        nnCopyVectorMatrixTranslation(ref tempSNNS_MATRIX0, ref pVec);
        nnMultiplyMatrix(current, current, ref tempSNNS_MATRIX0);
    }

    public static void amQuatMultiMatrix(
      ref NNS_QUATERNION pQuat,
      ref SNNS_VECTOR pVec)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnMakeQuaternionMatrix(out tempSNNS_MATRIX0, ref pQuat);
        nnCopyVectorMatrixTranslation(ref tempSNNS_MATRIX0, ref pVec);
        nnMultiplyMatrix(current, current, ref tempSNNS_MATRIX0);
    }

    public static void amQuatMultiMatrix(ref NNS_QUATERNION pQuat, NNS_VECTOR pVec)
    {
        NNS_MATRIX current = amMatrixGetCurrent();
        nnMakeQuaternionMatrix(out tempSNNS_MATRIX0, ref pQuat);
        if (pVec != null)
            nnCopyVectorMatrixTranslation(ref tempSNNS_MATRIX0, pVec);
        nnMultiplyMatrix(current, current, ref tempSNNS_MATRIX0);
    }

    public static void amQuatMultiVector(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pSrc,
      ref NNS_QUATERNION pQuat,
      NNS_VECTOR4D pVec)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION quat2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion3 = new NNS_QUATERNION();
        VEC4_COPY(ref nnsQuaternion3, pSrc);
        quat2.x = -pQuat.x;
        quat2.y = -pQuat.y;
        quat2.z = -pQuat.z;
        quat2.w = pQuat.w;
        nnMultiplyQuaternion(ref nnsQuaternion2, ref pQuat, ref nnsQuaternion3);
        nnMultiplyQuaternion(ref nnsQuaternion1, ref nnsQuaternion2, ref quat2);
        if (pVec == null)
        {
            VEC4_COPY(pDst, ref nnsQuaternion1);
        }
        else
        {
            NNS_VECTOR4D nnsVectoR4D = GlobalPool<NNS_VECTOR4D>.Alloc();
            NNS_VECTOR4D pV2 = GlobalPool<NNS_VECTOR4D>.Alloc();
            VEC4_COPY(nnsVectoR4D, ref nnsQuaternion1);
            VEC4_COPY(pV2, pVec);
            amVectorAdd(pDst, nnsVectoR4D, pV2);
            GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D);
            GlobalPool<NNS_VECTOR4D>.Release(pV2);
        }
    }

    public static void amQuatMultiVector(
      ref SNNS_VECTOR4D pDst,
      NNS_VECTOR4D pSrc,
      ref NNS_QUATERNION pQuat,
      NNS_VECTOR4D pVec)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION quat2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion3 = new NNS_QUATERNION();
        VEC4_COPY(ref nnsQuaternion3, pSrc);
        quat2.x = -pQuat.x;
        quat2.y = -pQuat.y;
        quat2.z = -pQuat.z;
        quat2.w = pQuat.w;
        nnMultiplyQuaternion(ref nnsQuaternion2, ref pQuat, ref nnsQuaternion3);
        nnMultiplyQuaternion(ref nnsQuaternion1, ref nnsQuaternion2, ref quat2);
        if (pVec == null)
        {
            VEC4_COPY(ref pDst, ref nnsQuaternion1);
        }
        else
        {
            SNNS_VECTOR4D snnsVectoR4D1 = new SNNS_VECTOR4D();
            SNNS_VECTOR4D snnsVectoR4D2 = new SNNS_VECTOR4D();
            VEC4_COPY(ref snnsVectoR4D1, ref nnsQuaternion1);
            VEC4_COPY(ref snnsVectoR4D2, pVec);
            amVectorAdd(ref pDst, ref snnsVectoR4D1, ref snnsVectoR4D2);
        }
    }

    public static void amQuatMultiVector(
      ref SNNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pSrc,
      ref NNS_QUATERNION pQuat,
      NNS_VECTOR4D pVec)
    {
        NNS_QUATERNION nnsQuaternion1 = new NNS_QUATERNION();
        NNS_QUATERNION quat2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion2 = new NNS_QUATERNION();
        NNS_QUATERNION nnsQuaternion3 = new NNS_QUATERNION();
        VEC4_COPY(ref nnsQuaternion3, ref pSrc);
        quat2.x = -pQuat.x;
        quat2.y = -pQuat.y;
        quat2.z = -pQuat.z;
        quat2.w = pQuat.w;
        nnMultiplyQuaternion(ref nnsQuaternion2, ref pQuat, ref nnsQuaternion3);
        nnMultiplyQuaternion(ref nnsQuaternion1, ref nnsQuaternion2, ref quat2);
        if (pVec == null)
        {
            VEC4_COPY(ref pDst, ref nnsQuaternion1);
        }
        else
        {
            SNNS_VECTOR4D snnsVectoR4D1 = new SNNS_VECTOR4D();
            SNNS_VECTOR4D snnsVectoR4D2 = new SNNS_VECTOR4D();
            VEC4_COPY(ref snnsVectoR4D1, ref nnsQuaternion1);
            VEC4_COPY(ref snnsVectoR4D2, pVec);
            amVectorAdd(ref pDst, ref snnsVectoR4D1, ref snnsVectoR4D2);
        }
    }

    public static void amQuatRotAxisToQuat(
      ref NNS_QUATERNION pQuat,
      NNS_VECTOR4D pVec,
      int angle)
    {
        amQuatRotAxisToQuat(ref pQuat, pVec, NNM_A32toRAD(angle));
    }

    public static void amQuatSet(
      ref NNS_QUATERNION pDst,
      float x,
      float y,
      float z,
      float w)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
        pDst.w = w;
    }

    public static void amQuatSquad(
      ref NNS_QUATERNION pDst,
      ref NNS_QUATERNION pQ1,
      ref NNS_QUATERNION pQ2,
      ref NNS_QUATERNION pQ3,
      ref NNS_QUATERNION pQ4,
      float t)
    {
        nnSquadQuaternion(ref pDst, ref pQ1, ref pQ2, ref pQ3, ref pQ4, t);
    }

    public static void amQuatSlerp(
      ref NNS_QUATERNION pDst,
      ref NNS_QUATERNION pQ1,
      ref NNS_QUATERNION pQ2,
      float per)
    {
        nnSlerpQuaternion(out pDst, ref pQ1, ref pQ2, per);
    }

    public static void amQuatUnitLerp(
      ref NNS_QUATERNION pDst,
      ref NNS_QUATERNION pQ1,
      ref NNS_QUATERNION pQ2,
      float per)
    {
        NNS_QUATERNION nnsQuaternion = new NNS_QUATERNION();
        amQuatLerp(ref nnsQuaternion, ref pQ1, ref pQ2, per);
        nnNormalizeQuaternion(ref pDst, ref nnsQuaternion);
    }

    public static void amQuatLerp(
      ref NNS_QUATERNION pDst,
      ref NNS_QUATERNION pQ1,
      ref NNS_QUATERNION pQ2,
      float per)
    {
        nnLerpQuaternion(ref pDst, ref pQ1, ref pQ2, per);
    }

    public static void amQuatMatrixToQuat(ref NNS_QUATERNION pQuat, NNS_MATRIX pMtx)
    {
        nnMakeRotateMatrixQuaternion(out pQuat, pMtx);
    }

    public static void amQuatEulerToQuatXYZ(
      ref NNS_QUATERNION pQuat,
      NNS_VECTOR4D pRot)
    {
        nnMakeRotateXYZQuaternion(out pQuat, NNM_RADtoA32(pRot.x), NNM_RADtoA32(pRot.y), NNM_RADtoA32(pRot.z));
    }

    public static void amQuatEulerToQuatXYZ(
      ref NNS_QUATERNION pQuat,
      float rx,
      float ry,
      float rz)
    {
        nnMakeRotateXYZQuaternion(out pQuat, NNM_RADtoA32(rx), NNM_RADtoA32(ry), NNM_RADtoA32(rz));
    }

    public static void amQuatEulerToQuatXYZ(
      ref NNS_QUATERNION pQuat,
      int ax,
      int ay,
      int az)
    {
        nnMakeRotateXYZQuaternion(out pQuat, ax, ay, az);
    }

    public static void amQuatMakeRotateAxis(
      ref NNS_QUATERNION pDst,
      NNS_VECTOR pV,
      int ang)
    {
        nnMakeRotateAxisQuaternion(out pDst, pV.x, pV.y, pV.z, ang);
    }

    public static void amQuatMulti(
      ref NNS_QUATERNION pDst,
      ref NNS_QUATERNION pQ1,
      ref NNS_QUATERNION pQ2)
    {
        nnMultiplyQuaternion(ref pDst, ref pQ1, ref pQ2);
    }

    public static void amQuatUnit(ref NNS_QUATERNION pDst, ref NNS_QUATERNION pSrc)
    {
        nnNormalizeQuaternion(ref pDst, ref pSrc);
    }

    public static void amQuatInverse(ref NNS_QUATERNION pDst, ref NNS_QUATERNION pSrc)
    {
        nnInvertQuaternion(ref pDst, ref pSrc);
    }

    public static void amQuatCopy(ref NNS_QUATERNION pDst, ref NNS_QUATERNION pSrc)
    {
        nnCopyQuaternion(ref pDst, ref pSrc);
    }

    public static void amQuatInit(ref NNS_QUATERNION pQuat)
    {
        nnMakeUnitQuaternion(ref pQuat);
    }

}