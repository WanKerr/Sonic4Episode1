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
    public static void VEC3_COPY(AppMain.NNS_QUATERNION d_vec, AppMain.NNS_VECTOR s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.z = 0.0f;
    }

    public static void VEC3_COPY(AppMain.NNS_VECTOR d_vec, AppMain.NNS_VECTOR s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
    }

    public static void VEC4_COPY(AppMain.NNS_VECTOR d_vec, AppMain.NNS_VECTOR s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
    }

    public static void VEC4_COPY(ref AppMain.SNNS_VECTOR4D d_vec, ref AppMain.SNNS_VECTOR4D s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = s_vec.w;
    }

    public static void VEC4_COPY(ref AppMain.SNNS_VECTOR4D d_vec, AppMain.NNS_VECTOR4D s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = s_vec.w;
    }

    public static void VEC4_COPY(AppMain.NNS_VECTOR4D d_vec, ref AppMain.NNS_QUATERNION s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = s_vec.w;
    }

    public static void VEC4_COPY(ref AppMain.SNNS_VECTOR4D d_vec, ref AppMain.NNS_QUATERNION s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = s_vec.w;
    }

    public static void VEC4_COPY(ref AppMain.NNS_QUATERNION d_vec, AppMain.NNS_VECTOR s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = 0.0f;
    }

    public static void VEC4_COPY(ref AppMain.NNS_QUATERNION d_vec, ref AppMain.SNNS_VECTOR4D s_vec)
    {
        d_vec.x = s_vec.x;
        d_vec.y = s_vec.y;
        d_vec.z = s_vec.z;
        d_vec.w = s_vec.w;
    }

    public static void VEC4_NEG(AppMain.NNS_VECTOR d_vec, AppMain.NNS_VECTOR s_vec)
    {
        d_vec.x = -s_vec.x;
        d_vec.y = -s_vec.y;
        d_vec.z = -s_vec.z;
    }

    public void amVectorInit(AppMain.NNS_VECTOR pVec)
    {
        pVec.x = 0.0f;
        pVec.y = 0.0f;
        pVec.z = 0.0f;
    }

    public static void amVectorInit(ref AppMain.SNNS_VECTOR4D pVec)
    {
        pVec.x = 0.0f;
        pVec.y = 0.0f;
        pVec.z = 0.0f;
        pVec.w = 1f;
    }

    public static void amVectorInit(AppMain.NNS_VECTOR4D pVec)
    {
        pVec.x = 0.0f;
        pVec.y = 0.0f;
        pVec.z = 0.0f;
        pVec.w = 1f;
    }

    public static void amVectorOne(ref AppMain.SNNS_VECTOR4D pVec)
    {
        pVec.x = 1f;
        pVec.y = 1f;
        pVec.z = 1f;
        pVec.w = 1f;
    }

    public static void amVectorOne(AppMain.NNS_VECTOR4D pVec)
    {
        pVec.x = 1f;
        pVec.y = 1f;
        pVec.z = 1f;
        pVec.w = 1f;
    }

    public static void amVectorSet(AppMain.NNS_PRIM3D_PC pDst, float x, float y, float z)
    {
        pDst.Pos.x = x;
        pDst.Pos.y = y;
        pDst.Pos.z = z;
    }

    public static void amVectorSet(ref AppMain.SNNS_VECTOR pDst, float x, float y, float z)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
    }

    public static void amVectorSet(AppMain.NNS_VECTOR pDst, float x, float y, float z)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
    }

    public static void amVectorSet(AppMain.NNS_VECTOR2D pDst, float x, float y)
    {
        pDst.x = x;
        pDst.y = y;
    }

    public static void amVectorSet(out AppMain.SNNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
        pDst.w = 1f;
    }

    public static void amVectorSet(ref AppMain.NNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
        pDst.w = 1f;
    }

    public static void amVectorSet(AppMain.NNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
        pDst.w = 1f;
    }

    public static void amVectorSet(AppMain.NNS_VECTOR4D pDst, float x, float y, float z, float w)
    {
        pDst.x = x;
        pDst.y = y;
        pDst.z = z;
        pDst.w = w;
    }

    public static void amVectorCopy(ref AppMain.SNNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x = pSrc.x;
        pDst.y = pSrc.y;
        pDst.z = pSrc.z;
        pDst.w = pSrc.w;
    }

    public static void amVectorCopy(ref AppMain.SNNS_VECTOR4D pDst, ref AppMain.SNNS_VECTOR4D pSrc)
    {
        pDst.x = pSrc.x;
        pDst.y = pSrc.y;
        pDst.z = pSrc.z;
        pDst.w = pSrc.w;
    }

    public static void amVectorCopy(AppMain.NNS_VECTOR4D pDst, ref AppMain.SNNS_VECTOR4D pSrc)
    {
        pDst.x = pSrc.x;
        pDst.y = pSrc.y;
        pDst.z = pSrc.z;
        pDst.w = pSrc.w;
    }

    public static void amVectorCopy(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x = pSrc.x;
        pDst.y = pSrc.y;
        pDst.z = pSrc.z;
        pDst.w = pSrc.w;
    }

    public static void amVectorAdd(
      ref AppMain.SNNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      out AppMain.SNNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      ref AppMain.SNNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      ref AppMain.SNNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      AppMain.NNS_VECTOR pDst,
      AppMain.NNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      AppMain.NNS_VECTOR pDst,
      ref AppMain.SNNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      ref AppMain.SNNS_VECTOR pDst,
      ref AppMain.SNNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      AppMain.NNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      AppMain.NNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorAdd(
      ref AppMain.SNNS_VECTOR pDst,
      AppMain.NNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      ref AppMain.SNNS_VECTOR pDst,
      AppMain.NNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      ref AppMain.SNNS_VECTOR pDst,
      ref AppMain.SNNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      ref AppMain.SNNS_VECTOR pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      AppMain.NNS_VECTOR pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR pV2)
    {
        pDst.x = pV1.x + pV2.x;
        pDst.y = pV1.y + pV2.y;
        pDst.z = pV1.z + pV2.z;
    }

    public static void amVectorAdd(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pSrc,
      float x,
      float y,
      float z)
    {
        pDst.x = pSrc.x + x;
        pDst.y = pSrc.y + y;
        pDst.z = pSrc.z + z;
        pDst.w = pSrc.w;
    }

    public static void amVectorAdd(ref AppMain.SNNS_VECTOR4D pDst, ref AppMain.SNNS_VECTOR4D pSrc)
    {
        pDst.x += pSrc.x;
        pDst.y += pSrc.y;
        pDst.z += pSrc.z;
    }

    public static void amVectorAdd(ref AppMain.SNNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x += pSrc.x;
        pDst.y += pSrc.y;
        pDst.z += pSrc.z;
    }

    public static void amVectorAdd(AppMain.NNS_VECTOR4D pDst, ref AppMain.SNNS_VECTOR4D pSrc)
    {
        pDst.x += pSrc.x;
        pDst.y += pSrc.y;
        pDst.z += pSrc.z;
    }

    public static void amVectorAdd(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x += pSrc.x;
        pDst.y += pSrc.y;
        pDst.z += pSrc.z;
    }

    public static void amVectorAdd(AppMain.NNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x += x;
        pDst.y += y;
        pDst.z += z;
    }

    public static void amVectorSub(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x - pV2.x;
        pDst.y = pV1.y - pV2.y;
        pDst.z = pV1.z - pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorSub(
      AppMain.NNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x - pV2.x;
        pDst.y = pV1.y - pV2.y;
        pDst.z = pV1.z - pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorSub(
      AppMain.NNS_VECTOR4D pDst,
      ref AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x - pV2.x;
        pDst.y = pV1.y - pV2.y;
        pDst.z = pV1.z - pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorSub(
      ref AppMain.SNNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x - pV2.x;
        pDst.y = pV1.y - pV2.y;
        pDst.z = pV1.z - pV2.z;
        pDst.w = pV1.w;
    }

    public static void amVectorSub(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pSrc,
      float x,
      float y,
      float z)
    {
        pDst.x = pSrc.x - x;
        pDst.y = pSrc.y - y;
        pDst.z = pSrc.z - z;
        pDst.w = pSrc.w;
    }

    public static void amVectorSub(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x -= pSrc.x;
        pDst.y -= pSrc.y;
        pDst.z -= pSrc.z;
    }

    public static void amVectorSub(AppMain.NNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x -= x;
        pDst.y -= y;
        pDst.z -= z;
    }

    public static void amVectorGetInner(
      AppMain.NNS_VECTOR pDst,
      AppMain.NNS_VECTOR pV1,
      AppMain.NNS_VECTOR pV2,
      float per)
    {
        float num = 1f - per;
        pDst.x = (float)((double)pV1.x * (double)num + (double)pV2.x * (double)per);
        pDst.y = (float)((double)pV1.y * (double)num + (double)pV2.y * (double)per);
        pDst.z = (float)((double)pV1.z * (double)num + (double)pV2.z * (double)per);
    }

    public static void amVectorGetInner(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2,
      float per)
    {
        float num = 1f - per;
        pDst.x = (float)((double)pV1.x * (double)num + (double)pV2.x * (double)per);
        pDst.y = (float)((double)pV1.y * (double)num + (double)pV2.y * (double)per);
        pDst.z = (float)((double)pV1.z * (double)num + (double)pV2.z * (double)per);
        pDst.w = pV1.w;
    }

    public static void amVectorGetAverage(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2,
      float p1,
      float p2)
    {
        pDst.x = (float)((double)pV1.x * (double)p1 + (double)pV2.x * (double)p2);
        pDst.y = (float)((double)pV1.y * (double)p1 + (double)pV2.y * (double)p2);
        pDst.z = (float)((double)pV1.z * (double)p1 + (double)pV2.z * (double)p2);
        pDst.w = pV1.w;
    }

    public static void amVectorGetAverage(
      ref AppMain.SNNS_VECTOR pDst,
      ref AppMain.SNNS_VECTOR pV1,
      ref AppMain.SNNS_VECTOR pV2,
      float p1,
      float p2)
    {
        pDst.x = (float)((double)pV1.x * (double)p1 + (double)pV2.x * (double)p2);
        pDst.y = (float)((double)pV1.y * (double)p1 + (double)pV2.y * (double)p2);
        pDst.z = (float)((double)pV1.z * (double)p1 + (double)pV2.z * (double)p2);
    }

    public static void amVectorGetAverage(
      ref AppMain.SNNS_VECTOR pDst,
      ref AppMain.SNNS_VECTOR pV1,
      AppMain.NNS_VECTOR pV2,
      float p1,
      float p2)
    {
        pDst.x = (float)((double)pV1.x * (double)p1 + (double)pV2.x * (double)p2);
        pDst.y = (float)((double)pV1.y * (double)p1 + (double)pV2.y * (double)p2);
        pDst.z = (float)((double)pV1.z * (double)p1 + (double)pV2.z * (double)p2);
    }

    public static void amVectorGetAverage(
      ref AppMain.SNNS_VECTOR pDst,
      AppMain.NNS_VECTOR pV1,
      AppMain.NNS_VECTOR pV2,
      float p1,
      float p2)
    {
        pDst.x = (float)((double)pV1.x * (double)p1 + (double)pV2.x * (double)p2);
        pDst.y = (float)((double)pV1.y * (double)p1 + (double)pV2.y * (double)p2);
        pDst.z = (float)((double)pV1.z * (double)p1 + (double)pV2.z * (double)p2);
    }

    public static void amVectorGetAverage(
      AppMain.NNS_VECTOR pDst,
      AppMain.NNS_VECTOR pV1,
      AppMain.NNS_VECTOR pV2,
      float p1,
      float p2)
    {
        pDst.x = (float)((double)pV1.x * (double)p1 + (double)pV2.x * (double)p2);
        pDst.y = (float)((double)pV1.y * (double)p1 + (double)pV2.y * (double)p2);
        pDst.z = (float)((double)pV1.z * (double)p1 + (double)pV2.z * (double)p2);
    }

    public static float amVectorGetLength(AppMain.NNS_VECTOR4D pV1, AppMain.NNS_VECTOR4D pV2)
    {
        return AppMain.amSqrt(AppMain.amPow2(pV1.x - pV2.x) + AppMain.amPow2(pV1.y - pV2.y) + AppMain.amPow2(pV1.z - pV2.z));
    }

    public static float amVectorGetLength2(AppMain.NNS_VECTOR4D pV1, AppMain.NNS_VECTOR4D pV2)
    {
        return AppMain.amPow2(pV1.x - pV2.x) + AppMain.amPow2(pV1.y - pV2.y) + AppMain.amPow2(pV1.z - pV2.z);
    }

    public static float amVectorScalor(AppMain.NNS_VECTOR4D pVec)
    {
        return AppMain.amSqrt(AppMain.amPow2(pVec.x) + AppMain.amPow2(pVec.y) + AppMain.amPow2(pVec.z));
    }

    public static float amVectorScalor(ref AppMain.SNNS_VECTOR4D pVec)
    {
        return AppMain.amSqrt(AppMain.amPow2(pVec.x) + AppMain.amPow2(pVec.y) + AppMain.amPow2(pVec.z));
    }

    public static float amVectorScalor2(AppMain.NNS_VECTOR4D pVec)
    {
        return AppMain.amPow2(pVec.x) + AppMain.amPow2(pVec.y) + AppMain.amPow2(pVec.z);
    }

    public static float amVectorScaleUnit(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pSrc,
      float len)
    {
        float fs = AppMain.amSqrt(AppMain.amPow2(pSrc.x) + AppMain.amPow2(pSrc.y) + AppMain.amPow2(pSrc.z));
        AppMain.amVectorCopy(pDst, pSrc);
        if (!AppMain.amIsZerof(fs))
        {
            len /= fs;
            pDst.x *= len;
            pDst.y *= len;
            pDst.z *= len;
        }
        return fs;
    }

    public static float amVectorScaleUnit(
      ref AppMain.SNNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pSrc,
      float len)
    {
        float fs = AppMain.amSqrt(AppMain.amPow2(pSrc.x) + AppMain.amPow2(pSrc.y) + AppMain.amPow2(pSrc.z));
        AppMain.amVectorCopy(ref pDst, ref pSrc);
        if (!AppMain.amIsZerof(fs))
        {
            len /= fs;
            pDst.x *= len;
            pDst.y *= len;
            pDst.z *= len;
        }
        return fs;
    }

    public static float amVectorScaleUnit(AppMain.NNS_VECTOR4D pDst, float len)
    {
        float fs = AppMain.amSqrt(AppMain.amPow2(pDst.x) + AppMain.amPow2(pDst.y) + AppMain.amPow2(pDst.z));
        if (!AppMain.amIsZerof(fs))
        {
            len /= fs;
            pDst.x *= len;
            pDst.y *= len;
            pDst.z *= len;
        }
        return fs;
    }

    public static void amVectorScale(
      ref AppMain.SNNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pSrc,
      float sc)
    {
        pDst.x = pSrc.x * sc;
        pDst.y = pSrc.y * sc;
        pDst.z = pSrc.z * sc;
        pDst.w = pSrc.w;
    }

    public static void amVectorScale(
      ref AppMain.SNNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pSrc,
      float sc)
    {
        pDst.x = pSrc.x * sc;
        pDst.y = pSrc.y * sc;
        pDst.z = pSrc.z * sc;
        pDst.w = pSrc.w;
    }

    public static void amVectorScale(
      AppMain.NNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pSrc,
      float sc)
    {
        pDst.x = pSrc.x * sc;
        pDst.y = pSrc.y * sc;
        pDst.z = pSrc.z * sc;
        pDst.w = pSrc.w;
    }

    public static void amVectorScale(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc, float sc)
    {
        pDst.x = pSrc.x * sc;
        pDst.y = pSrc.y * sc;
        pDst.z = pSrc.z * sc;
        pDst.w = pSrc.w;
    }

    public void amVectorScale(AppMain.NNS_VECTOR4D pDst, float sc)
    {
        pDst.x *= sc;
        pDst.y *= sc;
        pDst.z *= sc;
    }

    public static float amVectorUnit(AppMain.NNS_VECTOR pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        float fs = AppMain.amSqrt(AppMain.amPow2(pSrc.x) + AppMain.amPow2(pSrc.y) + AppMain.amPow2(pSrc.z));
        AppMain.nnCopyVector(pDst, (AppMain.NNS_VECTOR)pSrc);
        if (!AppMain.amIsZerof(fs))
        {
            float num = 1f / fs;
            pDst.x *= num;
            pDst.y *= num;
            pDst.z *= num;
        }
        return fs;
    }

    public static float amVectorUnit(AppMain.NNS_VECTOR pDst, ref AppMain.SNNS_VECTOR4D pSrc)
    {
        float fs = AppMain.amSqrt(AppMain.amPow2(pSrc.x) + AppMain.amPow2(pSrc.y) + AppMain.amPow2(pSrc.z));
        AppMain.nnCopyVector(pDst, ref pSrc);
        if (!AppMain.amIsZerof(fs))
        {
            float num = 1f / fs;
            pDst.x *= num;
            pDst.y *= num;
            pDst.z *= num;
        }
        return fs;
    }

    public static float amVectorUnit(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.w = pSrc.w;
        return AppMain.amVectorUnit((AppMain.NNS_VECTOR)pDst, pSrc);
    }

    public static float amVectorUnit(AppMain.NNS_VECTOR4D pDst)
    {
        float fs = AppMain.amSqrt(AppMain.amPow2(pDst.x) + AppMain.amPow2(pDst.y) + AppMain.amPow2(pDst.z));
        if (!AppMain.amIsZerof(fs))
        {
            float num = 1f / fs;
            pDst.x *= num;
            pDst.y *= num;
            pDst.z *= num;
        }
        return fs;
    }

    public static float amVectorUnit(ref AppMain.NNS_VECTOR4D pDst)
    {
        float fs = AppMain.amSqrt(AppMain.amPow2(pDst.x) + AppMain.amPow2(pDst.y) + AppMain.amPow2(pDst.z));
        if (!AppMain.amIsZerof(fs))
        {
            float num = 1f / fs;
            pDst.x *= num;
            pDst.y *= num;
            pDst.z *= num;
        }
        return fs;
    }

    public static float amVectorUnit(ref AppMain.SNNS_VECTOR4D pDst)
    {
        float fs = AppMain.amSqrt(AppMain.amPow2(pDst.x) + AppMain.amPow2(pDst.y) + AppMain.amPow2(pDst.z));
        if (!AppMain.amIsZerof(fs))
        {
            float num = 1f / fs;
            pDst.x *= num;
            pDst.y *= num;
            pDst.z *= num;
        }
        return fs;
    }

    public void amVectorInvert(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x = -pSrc.x;
        pDst.y = -pSrc.y;
        pDst.z = -pSrc.z;
        pDst.w = pSrc.w;
    }

    public void amVectorInvert(AppMain.NNS_VECTOR pDst, AppMain.NNS_VECTOR pSrc)
    {
        pDst.x = -pSrc.x;
        pDst.y = -pSrc.y;
        pDst.z = -pSrc.z;
    }

    public void amVectorInvert(AppMain.NNS_VECTOR4D pVec)
    {
        pVec.x = -pVec.x;
        pVec.y = -pVec.y;
        pVec.z = -pVec.z;
    }

    public void amVectorInvert(AppMain.NNS_VECTOR pVec)
    {
        pVec.x = -pVec.x;
        pVec.y = -pVec.y;
        pVec.z = -pVec.z;
    }

    public float amVectorInnerProduct(AppMain.NNS_VECTOR4D pV1, AppMain.NNS_VECTOR4D pV2)
    {
        return (float)((double)pV1.x * (double)pV2.x + (double)pV1.y * (double)pV2.y + (double)pV1.z * (double)pV2.z);
    }

    public static void amVectorOuterProduct(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2)
    {
        AppMain.amVectorSet(pDst, (float)((double)pV1.y * (double)pV2.z - (double)pV1.z * (double)pV2.y), (float)((double)pV1.z * (double)pV2.x - (double)pV1.x * (double)pV2.z), (float)((double)pV1.x * (double)pV2.y - (double)pV1.y * (double)pV2.x));
    }

    public static void amVectorOuterProduct(
      AppMain.NNS_VECTOR4D pDst,
      ref AppMain.SNNS_VECTOR4D pV1,
      ref AppMain.SNNS_VECTOR4D pV2)
    {
        AppMain.amVectorSet(pDst, (float)((double)pV1.y * (double)pV2.z - (double)pV1.z * (double)pV2.y), (float)((double)pV1.z * (double)pV2.x - (double)pV1.x * (double)pV2.z), (float)((double)pV1.x * (double)pV2.y - (double)pV1.y * (double)pV2.x));
    }

    public void amVectorMul(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2)
    {
        pDst.x = pV1.x * pV2.x;
        pDst.y = pV1.y * pV2.y;
        pDst.z = pV1.z * pV2.z;
        pDst.w = pV1.w;
    }

    public void amVectorMul(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x *= pSrc.x;
        pDst.y *= pSrc.y;
        pDst.z *= pSrc.z;
    }

    public void amVectorMul(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pSrc,
      float x,
      float y,
      float z)
    {
        pDst.x = pSrc.x * x;
        pDst.y = pSrc.y * y;
        pDst.z = pSrc.z * z;
        pDst.w = pSrc.w;
    }

    public void amVectorMul(AppMain.NNS_VECTOR4D pDst, float x, float y, float z)
    {
        pDst.x *= x;
        pDst.y *= y;
        pDst.z *= z;
    }

    public void amVectorMax(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2)
    {
        pDst.x = AppMain.amMax<float>(pV1.x, pV2.x);
        pDst.y = AppMain.amMax<float>(pV1.y, pV2.y);
        pDst.z = AppMain.amMax<float>(pV1.z, pV2.z);
        pDst.w = pV1.w;
    }

    public void amVectorMax(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc, float val)
    {
        pDst.x = AppMain.amMax<float>(pSrc.x, val);
        pDst.y = AppMain.amMax<float>(pSrc.y, val);
        pDst.z = AppMain.amMax<float>(pSrc.z, val);
        pDst.w = pSrc.w;
    }

    public void amVectorMax(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x = AppMain.amMax<float>(pDst.x, pSrc.x);
        pDst.y = AppMain.amMax<float>(pDst.y, pSrc.y);
        pDst.z = AppMain.amMax<float>(pDst.z, pSrc.z);
    }

    public void amVectorMax(AppMain.NNS_VECTOR4D pDst, float val)
    {
        pDst.x = AppMain.amMax<float>(pDst.x, val);
        pDst.y = AppMain.amMax<float>(pDst.y, val);
        pDst.z = AppMain.amMax<float>(pDst.z, val);
    }

    public void amVectorMin(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pV1,
      AppMain.NNS_VECTOR4D pV2)
    {
        pDst.x = AppMain.amMin<float>(pV1.x, pV2.x);
        pDst.y = AppMain.amMin<float>(pV1.y, pV2.y);
        pDst.z = AppMain.amMin<float>(pV1.z, pV2.z);
        pDst.w = pV1.w;
    }

    public void amVectorMin(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc, float val)
    {
        pDst.x = AppMain.amMin<float>(pSrc.x, val);
        pDst.y = AppMain.amMin<float>(pSrc.y, val);
        pDst.z = AppMain.amMin<float>(pSrc.z, val);
        pDst.w = pSrc.w;
    }

    public void amVectorMin(AppMain.NNS_VECTOR4D pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x = AppMain.amMin<float>(pDst.x, pSrc.x);
        pDst.y = AppMain.amMin<float>(pDst.y, pSrc.y);
        pDst.z = AppMain.amMin<float>(pDst.z, pSrc.z);
    }

    public void amVectorMin(AppMain.NNS_VECTOR4D pDst, float val)
    {
        pDst.x = AppMain.amMin<float>(pDst.x, val);
        pDst.y = AppMain.amMin<float>(pDst.y, val);
        pDst.z = AppMain.amMin<float>(pDst.z, val);
    }

    public void amVectorClamp(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pSrc,
      AppMain.NNS_VECTOR4D pMin,
      AppMain.NNS_VECTOR4D pMax)
    {
        pDst.x = AppMain.amClamp(pSrc.x, pMin.x, pMax.x);
        pDst.y = AppMain.amClamp(pSrc.y, pMin.y, pMax.y);
        pDst.z = AppMain.amClamp(pSrc.z, pMin.z, pMax.z);
        pDst.w = pSrc.w;
    }

    public void amVectorClamp(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pSrc,
      float min,
      float max)
    {
        pDst.x = AppMain.amClamp(pSrc.x, min, max);
        pDst.y = AppMain.amClamp(pSrc.y, min, max);
        pDst.z = AppMain.amClamp(pSrc.z, min, max);
        pDst.w = pSrc.w;
    }

    public void amVectorClamp(
      AppMain.NNS_VECTOR4D pDst,
      AppMain.NNS_VECTOR4D pMin,
      AppMain.NNS_VECTOR4D pMax)
    {
        pDst.x = AppMain.amClamp(pDst.x, pMin.x, pMax.x);
        pDst.y = AppMain.amClamp(pDst.y, pMin.y, pMax.y);
        pDst.z = AppMain.amClamp(pDst.z, pMin.z, pMax.z);
    }

    public void amVectorClamp(AppMain.NNS_VECTOR4D pDst, float min, float max)
    {
        pDst.x = AppMain.amClamp(pDst.x, min, max);
        pDst.y = AppMain.amClamp(pDst.y, min, max);
        pDst.z = AppMain.amClamp(pDst.z, min, max);
    }

    public void amVectorCeil(AppMain.AMS_VECTOR4I pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x = (int)Math.Ceiling((double)pSrc.x);
        pDst.y = (int)Math.Ceiling((double)pSrc.y);
        pDst.z = (int)Math.Ceiling((double)pSrc.z);
        pDst.w = (int)Math.Ceiling((double)pSrc.w);
    }

    public void amVectorTrunc(AppMain.AMS_VECTOR4I pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x = (double)pSrc.x >= 0.0 ? (int)Math.Floor((double)pSrc.x) : (int)-Math.Floor(-(double)pSrc.x);
        pDst.y = (double)pSrc.y >= 0.0 ? (int)Math.Floor((double)pSrc.y) : (int)-Math.Floor(-(double)pSrc.y);
        pDst.z = (double)pSrc.z >= 0.0 ? (int)Math.Floor((double)pSrc.z) : (int)-Math.Floor(-(double)pSrc.z);
        pDst.w = (double)pSrc.w >= 0.0 ? (int)Math.Floor((double)pSrc.w) : (int)-Math.Floor(-(double)pSrc.w);
    }

    public void amVectorFloor(AppMain.AMS_VECTOR4I pDst, AppMain.NNS_VECTOR4D pSrc)
    {
        pDst.x = (int)Math.Floor((double)pSrc.x);
        pDst.y = (int)Math.Floor((double)pSrc.y);
        pDst.z = (int)Math.Floor((double)pSrc.z);
        pDst.w = (int)Math.Floor((double)pSrc.w);
    }

    public void amVectorIntToFloat(AppMain.NNS_VECTOR4D pDst, AppMain.AMS_VECTOR4I pSrc)
    {
        pDst.x = (float)pSrc.x;
        pDst.y = (float)pSrc.y;
        pDst.z = (float)pSrc.z;
        pDst.w = (float)pSrc.w;
    }

    public static void amVectorRandom(ref AppMain.SNNS_VECTOR4D pDst)
    {
        AppMain.amVectorSet(out pDst, AppMain.nnRandom() - 0.5f, AppMain.nnRandom() - 0.5f, AppMain.nnRandom() - 0.5f);
        double num = (double)AppMain.amVectorUnit(ref pDst);
    }

    public static void amVectorRandom(AppMain.NNS_VECTOR4D pDst)
    {
        AppMain.amVectorSet(pDst, AppMain.nnRandom() - 0.5f, AppMain.nnRandom() - 0.5f, AppMain.nnRandom() - 0.5f);
        double num = (double)AppMain.amVectorUnit(pDst);
    }

    public uint amVectorCmp(AppMain.NNS_VECTOR4D pV1, AppMain.NNS_VECTOR4D pV2)
    {
        return (double)pV1.x == (double)pV2.x && (double)pV1.y == (double)pV2.y && ((double)pV1.z == (double)pV2.z && (double)pV1.w == (double)pV2.w) ? 1U : 0U;
    }

}