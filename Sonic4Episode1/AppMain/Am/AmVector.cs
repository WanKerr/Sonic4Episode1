using System;

public partial class AppMain
{
    public static void VEC3_COPY(NNS_QUATERNION d_vec, NNS_VECTOR s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.z = 0.0f;
    }

    public static void VEC3_COPY(NNS_VECTOR d_vec, NNS_VECTOR s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
    }

    public static void VEC4_COPY(NNS_VECTOR d_vec, NNS_VECTOR s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
    }

    public static void VEC4_COPY(ref SNNS_VECTOR4D d_vec, ref SNNS_VECTOR4D s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = s_vec.w;
    }

    public static void VEC4_COPY(ref SNNS_VECTOR4D d_vec, NNS_VECTOR4D s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = s_vec.w;
    }

    public static void VEC4_COPY(NNS_VECTOR4D d_vec, ref NNS_QUATERNION s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = s_vec.w;
    }

    public static void VEC4_COPY(ref SNNS_VECTOR4D d_vec, ref NNS_QUATERNION s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = s_vec.w;
    }

    public static void VEC4_COPY(ref NNS_QUATERNION d_vec, NNS_VECTOR s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = 0.0f;
    }

    public static void VEC4_COPY(ref NNS_QUATERNION d_vec, ref SNNS_VECTOR4D s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = s_vec.w;
    }

    public static void VEC4_NEG(NNS_VECTOR d_vec, NNS_VECTOR s_vec)
    {
        d_vec.x = -s_vec.x;
        d_vec.y = -s_vec.y;
        d_vec.z = -s_vec.z;
    }

    public void amVectorInit(NNS_VECTOR pVec)
    {
        pVec.x = 0.0f;
        pVec.y = 0.0f;
        pVec.z = 0.0f;
    }

    public static void amVectorInit(ref SNNS_VECTOR4D pVec)
    {
        pVec.x = 0.0f;
        pVec.y = 0.0f;
        pVec.z = 0.0f;
        pVec.w = 1f;
    }

    public static void amVectorInit(NNS_VECTOR4D pVec)
    {
        pVec.x = 0.0f;
        pVec.y = 0.0f;
        pVec.z = 0.0f;
        pVec.w = 1f;
    }

    public static void amVectorOne(ref SNNS_VECTOR4D pVec)
    {
        pVec.x = 1f;
        pVec.y = 1f;
        pVec.z = 1f;
        pVec.w = 1f;
    }

    public static void amVectorOne(NNS_VECTOR4D pVec)
    {
        pVec.x = 1f;
        pVec.y = 1f;
        pVec.z = 1f;
        pVec.w = 1f;
    }

    public static void amVectorSet(NNS_PRIM3D_PC pDst, float x, float y, float z)
    {
        pDst.Pos.x = x;
        pDst.Pos.y = y;
        pDst.Pos.z = z;
    }

    public static void amVectorSet(ref SNNS_VECTOR pDst, float x, float y, float z)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
    }

    public static void amVectorSet(NNS_VECTOR pDst, float x, float y, float z)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
    }

    public static void amVectorSet(NNS_VECTOR2D pDst, float x, float y)
    {
        pDst.x = x;
        pDst.y = y;
    }

    public static void amVectorSet(out SNNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
        pDst.w = 1f;
    }

    public static void amVectorSet(ref NNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
        pDst.w = 1f;
    }

    public static void amVectorSet(NNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
        pDst.w = 1f;
    }

    public static void amVectorSet(NNS_VECTOR4D pDst, float x, float y, float z, float w)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
        pDst.w = w;
    }

    public static void amVectorCopy(ref SNNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x = pSrc.x;
        pDst.y = pSrc.y;
        pDst.z = pSrc.z;
        pDst.w = pSrc.w;
    }

    public static void amVectorCopy(ref SNNS_VECTOR4D pDst, ref SNNS_VECTOR4D pSrc)
    {
        pDst.x = pSrc.x;
        pDst.y = pSrc.y;
        pDst.z = pSrc.z;
        pDst.w = pSrc.w;
    }

    public static void amVectorCopy(NNS_VECTOR4D pDst, ref SNNS_VECTOR4D pSrc)
    {
        pDst.x = pSrc.x;
        pDst.y = pSrc.y;
        pDst.z = pSrc.z;
        pDst.w = pSrc.w;
    }

    public static void amVectorCopy(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x = pSrc.x;
        pDst.y = pSrc.y;
        pDst.z = pSrc.z;
        pDst.w = pSrc.w;
    }

    public static void amVectorAdd(
      ref SNNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pV1,
      ref SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      out SNNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      ref SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      ref SNNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      ref SNNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pV1,
      ref SNNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      NNS_VECTOR pDst,
      NNS_VECTOR4D pV1,
      ref SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      NNS_VECTOR pDst,
      ref SNNS_VECTOR4D pV1,
      ref SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      ref SNNS_VECTOR pDst,
      ref SNNS_VECTOR4D pV1,
      ref SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      NNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pV1,
      ref SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      NNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      ref SNNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      ref SNNS_VECTOR pDst,
      NNS_VECTOR4D pV1,
      ref SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      ref SNNS_VECTOR pDst,
      NNS_VECTOR4D pV1,
      ref SNNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      ref SNNS_VECTOR pDst,
      ref SNNS_VECTOR4D pV1,
      ref SNNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      ref SNNS_VECTOR pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      NNS_VECTOR pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pSrc,
      float x,
      float y,
      float z)
    {
        pDst.x = pSrc.x + x;
        pDst.y = pSrc.y + y;
        pDst.z = pSrc.z + z;
        pDst.w = pSrc.w;
    }

    public static void amVectorAdd(ref SNNS_VECTOR4D pDst, ref SNNS_VECTOR4D pSrc)
    {
        pDst.x += pSrc.x;
        pDst.y += pSrc.y;
        pDst.z += pSrc.z;
    }

    public static void amVectorAdd(ref SNNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x += pSrc.x;
        pDst.y += pSrc.y;
        pDst.z += pSrc.z;
    }

    public static void amVectorAdd(NNS_VECTOR4D pDst, ref SNNS_VECTOR4D pSrc)
    {
        pDst.x += pSrc.x;
        pDst.y += pSrc.y;
        pDst.z += pSrc.z;
    }

    public static void amVectorAdd(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x += pSrc.x;
        pDst.y += pSrc.y;
        pDst.z += pSrc.z;
    }

    public static void amVectorAdd(NNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x += x;
        pDst.y += y;
        pDst.z += z;
    }

    public static void amVectorSub(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x - pV2.x;
        pDst.y = pV1.y - pV2.y;
        pDst.z = pV1.z - pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorSub(
      NNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x - pV2.x;
        pDst.y = pV1.y - pV2.y;
        pDst.z = pV1.z - pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorSub(
      NNS_VECTOR4D pDst,
      ref NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x - pV2.x;
        pDst.y = pV1.y - pV2.y;
        pDst.z = pV1.z - pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorSub(
      ref SNNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pV1,
      ref SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x - pV2.x;
        pDst.y = pV1.y - pV2.y;
        pDst.z = pV1.z - pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorSub(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pSrc,
      float x,
      float y,
      float z)
    {
        pDst.x = pSrc.x - x;
        pDst.y = pSrc.y - y;
        pDst.z = pSrc.z - z;
        pDst.w = pSrc.w;
    }

    public static void amVectorSub(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x -= pSrc.x;
        pDst.y -= pSrc.y;
        pDst.z -= pSrc.z;
    }

    public static void amVectorSub(NNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x -= x;
        pDst.y -= y;
        pDst.z -= z;
    }

    public static void amVectorGetInner(
      NNS_VECTOR pDst,
      NNS_VECTOR pV1,
      NNS_VECTOR pV2,
      float per)
    {
        float num = 1f - per;
        pDst.x = (float)(pV1.x * (double)num + pV2.x * (double)per);
        pDst.y = (float)(pV1.y * (double)num + pV2.y * (double)per);
        pDst.z = (float)(pV1.z * (double)num + pV2.z * (double)per);
    }

    public static void amVectorGetInner(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2,
      float per)
    {
        float num = 1f - per;
        pDst.x = (float)(pV1.x * (double)num + pV2.x * (double)per);
        pDst.y = (float)(pV1.y * (double)num + pV2.y * (double)per);
        pDst.z = (float)(pV1.z * (double)num + pV2.z * (double)per);
        pDst.w = pV1.w;
    }

    public static void amVectorGetAverage(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2,
      float p1,
      float p2)
    {
        pDst.x = (float)(pV1.x * (double)p1 + pV2.x * (double)p2);
        pDst.y = (float)(pV1.y * (double)p1 + pV2.y * (double)p2);
        pDst.z = (float)(pV1.z * (double)p1 + pV2.z * (double)p2);
        pDst.w = pV1.w;
    }

    public static void amVectorGetAverage(
      ref SNNS_VECTOR pDst,
      ref SNNS_VECTOR pV1,
      ref SNNS_VECTOR pV2,
      float p1,
      float p2)
    {
        pDst.x = (float)(pV1.x * (double)p1 + pV2.x * (double)p2);
        pDst.y = (float)(pV1.y * (double)p1 + pV2.y * (double)p2);
        pDst.z = (float)(pV1.z * (double)p1 + pV2.z * (double)p2);
    }

    public static void amVectorGetAverage(
      ref SNNS_VECTOR pDst,
      ref SNNS_VECTOR pV1,
      NNS_VECTOR pV2,
      float p1,
      float p2)
    {
        pDst.x = (float)(pV1.x * (double)p1 + pV2.x * (double)p2);
        pDst.y = (float)(pV1.y * (double)p1 + pV2.y * (double)p2);
        pDst.z = (float)(pV1.z * (double)p1 + pV2.z * (double)p2);
    }

    public static void amVectorGetAverage(
      ref SNNS_VECTOR pDst,
      NNS_VECTOR pV1,
      NNS_VECTOR pV2,
      float p1,
      float p2)
    {
        pDst.x = (float)(pV1.x * (double)p1 + pV2.x * (double)p2);
        pDst.y = (float)(pV1.y * (double)p1 + pV2.y * (double)p2);
        pDst.z = (float)(pV1.z * (double)p1 + pV2.z * (double)p2);
    }

    public static void amVectorGetAverage(
      NNS_VECTOR pDst,
      NNS_VECTOR pV1,
      NNS_VECTOR pV2,
      float p1,
      float p2)
    {
        pDst.x = (float)(pV1.x * (double)p1 + pV2.x * (double)p2);
        pDst.y = (float)(pV1.y * (double)p1 + pV2.y * (double)p2);
        pDst.z = (float)(pV1.z * (double)p1 + pV2.z * (double)p2);
    }

    public static float amVectorGetLength(NNS_VECTOR4D pV1, NNS_VECTOR4D pV2)
    {
        return amSqrt(amPow2(pV1.x - pV2.x) + amPow2(pV1.y - pV2.y) + amPow2(pV1.z - pV2.z));
    }

    public static float amVectorGetLength2(NNS_VECTOR4D pV1, NNS_VECTOR4D pV2)
    {
        return amPow2(pV1.x - pV2.x) + amPow2(pV1.y - pV2.y) + amPow2(pV1.z - pV2.z);
    }

    public static float amVectorScalor(NNS_VECTOR4D pVec)
    {
        return amSqrt(amPow2(pVec.x) + amPow2(pVec.y) + amPow2(pVec.z));
    }

    public static float amVectorScalor(ref SNNS_VECTOR4D pVec)
    {
        return amSqrt(amPow2(pVec.x) + amPow2(pVec.y) + amPow2(pVec.z));
    }

    public static float amVectorScalor2(NNS_VECTOR4D pVec)
    {
        return amPow2(pVec.x) + amPow2(pVec.y) + amPow2(pVec.z);
    }

    public static float amVectorScaleUnit(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pSrc,
      float len)
    {
        float fs = amSqrt(amPow2(pSrc.x) + amPow2(pSrc.y) + amPow2(pSrc.z));
        amVectorCopy(pDst, pSrc);
        if (!amIsZerof(fs))
        {
            len /= fs;
            pDst.x *= len;
            pDst.y *= len;
            pDst.z *= len;
        }
        return fs;
    }

    public static float amVectorScaleUnit(
      ref SNNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pSrc,
      float len)
    {
        float fs = amSqrt(amPow2(pSrc.x) + amPow2(pSrc.y) + amPow2(pSrc.z));
        amVectorCopy(ref pDst, ref pSrc);
        if (!amIsZerof(fs))
        {
            len /= fs;
            pDst.x *= len;
            pDst.y *= len;
            pDst.z *= len;
        }
        return fs;
    }

    public static float amVectorScaleUnit(NNS_VECTOR4D pDst, float len)
    {
        float fs = amSqrt(amPow2(pDst.x) + amPow2(pDst.y) + amPow2(pDst.z));
        if (!amIsZerof(fs))
        {
            len /= fs;
            pDst.x *= len;
            pDst.y *= len;
            pDst.z *= len;
        }
        return fs;
    }

    public static void amVectorScale(
      ref SNNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pSrc,
      float sc)
    {
        pDst.x = pSrc.x * sc;
        pDst.y = pSrc.y * sc;
        pDst.z = pSrc.z * sc;
        pDst.w = pSrc.w;
    }

    public static void amVectorScale(
      ref SNNS_VECTOR4D pDst,
      NNS_VECTOR4D pSrc,
      float sc)
    {
        pDst.x = pSrc.x * sc;
        pDst.y = pSrc.y * sc;
        pDst.z = pSrc.z * sc;
        pDst.w = pSrc.w;
    }

    public static void amVectorScale(
      NNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pSrc,
      float sc)
    {
        pDst.x = pSrc.x * sc;
        pDst.y = pSrc.y * sc;
        pDst.z = pSrc.z * sc;
        pDst.w = pSrc.w;
    }

    public static void amVectorScale(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc, float sc)
    {
        pDst.x = pSrc.x * sc;
        pDst.y = pSrc.y * sc;
        pDst.z = pSrc.z * sc;
        pDst.w = pSrc.w;
    }

    public void amVectorScale(NNS_VECTOR4D pDst, float sc)
    {
        pDst.x *= sc;
        pDst.y *= sc;
        pDst.z *= sc;
    }

    public static float amVectorUnit(NNS_VECTOR pDst, NNS_VECTOR4D pSrc)
    {
        float fs = amSqrt(amPow2(pSrc.x) + amPow2(pSrc.y) + amPow2(pSrc.z));
        nnCopyVector(pDst, pSrc);
        if (!amIsZerof(fs))
        {
            float num = 1f / fs;
            pDst.x *= num;
            pDst.y *= num;
            pDst.z *= num;
        }
        return fs;
    }

    public static float amVectorUnit(NNS_VECTOR pDst, ref SNNS_VECTOR4D pSrc)
    {
        float fs = amSqrt(amPow2(pSrc.x) + amPow2(pSrc.y) + amPow2(pSrc.z));
        nnCopyVector(pDst, ref pSrc);
        if (!amIsZerof(fs))
        {
            float num = 1f / fs;
            pDst.x *= num;
            pDst.y *= num;
            pDst.z *= num;
        }
        return fs;
    }

    public static float amVectorUnit(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        pDst.w = pSrc.w;
        return amVectorUnit((NNS_VECTOR)pDst, pSrc);
    }

    public static float amVectorUnit(NNS_VECTOR4D pDst)
    {
        float fs = amSqrt(amPow2(pDst.x) + amPow2(pDst.y) + amPow2(pDst.z));
        if (!amIsZerof(fs))
        {
            float num = 1f / fs;
            pDst.x *= num;
            pDst.y *= num;
            pDst.z *= num;
        }
        return fs;
    }

    public static float amVectorUnit(ref NNS_VECTOR4D pDst)
    {
        float fs = amSqrt(amPow2(pDst.x) + amPow2(pDst.y) + amPow2(pDst.z));
        if (!amIsZerof(fs))
        {
            float num = 1f / fs;
            pDst.x *= num;
            pDst.y *= num;
            pDst.z *= num;
        }
        return fs;
    }

    public static float amVectorUnit(ref SNNS_VECTOR4D pDst)
    {
        float fs = amSqrt(amPow2(pDst.x) + amPow2(pDst.y) + amPow2(pDst.z));
        if (!amIsZerof(fs))
        {
            float num = 1f / fs;
            pDst.x *= num;
            pDst.y *= num;
            pDst.z *= num;
        }
        return fs;
    }

    public void amVectorInvert(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x = -pSrc.x;
        pDst.y = -pSrc.y;
        pDst.z = -pSrc.z;
        pDst.w = pSrc.w;
    }

    public void amVectorInvert(NNS_VECTOR pDst, NNS_VECTOR pSrc)
    {
        pDst.x = -pSrc.x;
        pDst.y = -pSrc.y;
        pDst.z = -pSrc.z;
    }

    public void amVectorInvert(NNS_VECTOR4D pVec)
    {
        pVec.x = -pVec.x;
        pVec.y = -pVec.y;
        pVec.z = -pVec.z;
    }

    public void amVectorInvert(NNS_VECTOR pVec)
    {
        pVec.x = -pVec.x;
        pVec.y = -pVec.y;
        pVec.z = -pVec.z;
    }

    public float amVectorInnerProduct(NNS_VECTOR4D pV1, NNS_VECTOR4D pV2)
    {
        return (float)(pV1.x * (double)pV2.x + pV1.y * (double)pV2.y + pV1.z * (double)pV2.z);
    }

    public static void amVectorOuterProduct(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        amVectorSet(pDst, (float)(pV1.y * (double)pV2.z - pV1.z * (double)pV2.y), (float)(pV1.z * (double)pV2.x - pV1.x * (double)pV2.z), (float)(pV1.x * (double)pV2.y - pV1.y * (double)pV2.x));
    }

    public static void amVectorOuterProduct(
      NNS_VECTOR4D pDst,
      ref SNNS_VECTOR4D pV1,
      ref SNNS_VECTOR4D pV2)
    {
        amVectorSet(pDst, (float)(pV1.y * (double)pV2.z - pV1.z * (double)pV2.y), (float)(pV1.z * (double)pV2.x - pV1.x * (double)pV2.z), (float)(pV1.x * (double)pV2.y - pV1.y * (double)pV2.x));
    }

    public void amVectorMul(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x * pV2.x;
        pDst.y = pV1.y * pV2.y;
        pDst.z = pV1.z * pV2.z;
        pDst.w = pV1.w;
    }

    public void amVectorMul(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x *= pSrc.x;
        pDst.y *= pSrc.y;
        pDst.z *= pSrc.z;
    }

    public void amVectorMul(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pSrc,
      float x,
      float y,
      float z)
    {
        pDst.x = pSrc.x * x;
        pDst.y = pSrc.y * y;
        pDst.z = pSrc.z * z;
        pDst.w = pSrc.w;
    }

    public void amVectorMul(NNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x *= x;
        pDst.y *= y;
        pDst.z *= z;
    }

    public void amVectorMax(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        pDst.x = amMax(pV1.x, pV2.x);
        pDst.y = amMax(pV1.y, pV2.y);
        pDst.z = amMax(pV1.z, pV2.z);
        pDst.w = pV1.w;
    }

    public void amVectorMax(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc, float val)
    {
        pDst.x = amMax(pSrc.x, val);
        pDst.y = amMax(pSrc.y, val);
        pDst.z = amMax(pSrc.z, val);
        pDst.w = pSrc.w;
    }

    public void amVectorMax(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x = amMax(pDst.x, pSrc.x);
        pDst.y = amMax(pDst.y, pSrc.y);
        pDst.z = amMax(pDst.z, pSrc.z);
    }

    public void amVectorMax(NNS_VECTOR4D pDst, float val)
    {
        pDst.x = amMax(pDst.x, val);
        pDst.y = amMax(pDst.y, val);
        pDst.z = amMax(pDst.z, val);
    }

    public void amVectorMin(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pV1,
      NNS_VECTOR4D pV2)
    {
        pDst.x = amMin(pV1.x, pV2.x);
        pDst.y = amMin(pV1.y, pV2.y);
        pDst.z = amMin(pV1.z, pV2.z);
        pDst.w = pV1.w;
    }

    public void amVectorMin(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc, float val)
    {
        pDst.x = amMin(pSrc.x, val);
        pDst.y = amMin(pSrc.y, val);
        pDst.z = amMin(pSrc.z, val);
        pDst.w = pSrc.w;
    }

    public void amVectorMin(NNS_VECTOR4D pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x = amMin(pDst.x, pSrc.x);
        pDst.y = amMin(pDst.y, pSrc.y);
        pDst.z = amMin(pDst.z, pSrc.z);
    }

    public void amVectorMin(NNS_VECTOR4D pDst, float val)
    {
        pDst.x = amMin(pDst.x, val);
        pDst.y = amMin(pDst.y, val);
        pDst.z = amMin(pDst.z, val);
    }

    public void amVectorClamp(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pSrc,
      NNS_VECTOR4D pMin,
      NNS_VECTOR4D pMax)
    {
        pDst.x = amClamp(pSrc.x, pMin.x, pMax.x);
        pDst.y = amClamp(pSrc.y, pMin.y, pMax.y);
        pDst.z = amClamp(pSrc.z, pMin.z, pMax.z);
        pDst.w = pSrc.w;
    }

    public void amVectorClamp(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pSrc,
      float min,
      float max)
    {
        pDst.x = amClamp(pSrc.x, min, max);
        pDst.y = amClamp(pSrc.y, min, max);
        pDst.z = amClamp(pSrc.z, min, max);
        pDst.w = pSrc.w;
    }

    public void amVectorClamp(
      NNS_VECTOR4D pDst,
      NNS_VECTOR4D pMin,
      NNS_VECTOR4D pMax)
    {
        pDst.x = amClamp(pDst.x, pMin.x, pMax.x);
        pDst.y = amClamp(pDst.y, pMin.y, pMax.y);
        pDst.z = amClamp(pDst.z, pMin.z, pMax.z);
    }

    public void amVectorClamp(NNS_VECTOR4D pDst, float min, float max)
    {
        pDst.x = amClamp(pDst.x, min, max);
        pDst.y = amClamp(pDst.y, min, max);
        pDst.z = amClamp(pDst.z, min, max);
    }

    public void amVectorCeil(AMS_VECTOR4I pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x = (int)Math.Ceiling(pSrc.x);
        pDst.y = (int)Math.Ceiling(pSrc.y);
        pDst.z = (int)Math.Ceiling(pSrc.z);
        pDst.w = (int)Math.Ceiling(pSrc.w);
    }

    public void amVectorTrunc(AMS_VECTOR4I pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x = pSrc.x >= 0.0 ? (int)Math.Floor(pSrc.x) : (int)-Math.Floor(-pSrc.x);
        pDst.y = pSrc.y >= 0.0 ? (int)Math.Floor(pSrc.y) : (int)-Math.Floor(-pSrc.y);
        pDst.z = pSrc.z >= 0.0 ? (int)Math.Floor(pSrc.z) : (int)-Math.Floor(-pSrc.z);
        pDst.w = pSrc.w >= 0.0 ? (int)Math.Floor(pSrc.w) : (int)-Math.Floor(-pSrc.w);
    }

    public void amVectorFloor(AMS_VECTOR4I pDst, NNS_VECTOR4D pSrc)
    {
        pDst.x = (int)Math.Floor(pSrc.x);
        pDst.y = (int)Math.Floor(pSrc.y);
        pDst.z = (int)Math.Floor(pSrc.z);
        pDst.w = (int)Math.Floor(pSrc.w);
    }

    public void amVectorIntToFloat(NNS_VECTOR4D pDst, AMS_VECTOR4I pSrc)
    {
        pDst.x = pSrc.x;
        pDst.y = pSrc.y;
        pDst.z = pSrc.z;
        pDst.w = pSrc.w;
    }

    public static void amVectorRandom(ref SNNS_VECTOR4D pDst)
    {
        amVectorSet(out pDst, nnRandom() - 0.5f, nnRandom() - 0.5f, nnRandom() - 0.5f);
        double num = amVectorUnit(ref pDst);
    }

    public static void amVectorRandom(NNS_VECTOR4D pDst)
    {
        amVectorSet(pDst, nnRandom() - 0.5f, nnRandom() - 0.5f, nnRandom() - 0.5f);
        double num = amVectorUnit(pDst);
    }

    public uint amVectorCmp(NNS_VECTOR4D pV1, NNS_VECTOR4D pV2)
    {
        return pV1.x == (double)pV2.x && pV1.y == (double)pV2.y && (pV1.z == (double)pV2.z && pV1.w == (double)pV2.w) ? 1U : 0U;
    }

}