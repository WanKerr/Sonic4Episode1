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
    public static void _amApplyGravity(
      AppMain.AMS_AME_ECB ecb,
      AppMain.AMS_AME_NODE node,
      AppMain.AMS_AME_RUNTIME_WORK work)
    {
        AppMain.AMS_AME_NODE_GRAVITY amsAmeNodeGravity = (AppMain.AMS_AME_NODE_GRAVITY)node;
        float amUnitTime = AppMain._am_unit_time;
        AppMain.NNS_VECTOR4D nnsVectoR4D1 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.NNS_VECTOR4D nnsVectoR4D2 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.amVectorCopy(nnsVectoR4D2, amsAmeNodeGravity.direction);
        if (((int)amsAmeNodeGravity.flag & 2) != 0)
            AppMain.amQuatMultiVector(nnsVectoR4D2, nnsVectoR4D2, ref ecb.rotate, (AppMain.NNS_VECTOR4D)null);
        AppMain.amVectorScale(nnsVectoR4D1, nnsVectoR4D2, (float)((double)amsAmeNodeGravity.magnitude * (double)amUnitTime * (double)amUnitTime * 0.5));
        AppMain.amVectorAdd(work.position, nnsVectoR4D1);
        AppMain.amVectorScale(nnsVectoR4D1, nnsVectoR4D2, amsAmeNodeGravity.magnitude * amUnitTime);
        AppMain.amVectorAdd(work.velocity, nnsVectoR4D1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D2);
    }

    public static void _amApplyUniform(
      AppMain.AMS_AME_ECB ecb,
      AppMain.AMS_AME_NODE node,
      AppMain.AMS_AME_RUNTIME_WORK work)
    {
        AppMain.AMS_AME_NODE_UNIFORM amsAmeNodeUniform = (AppMain.AMS_AME_NODE_UNIFORM)node;
        float amUnitTime = AppMain._am_unit_time;
        AppMain.NNS_VECTOR4D nnsVectoR4D = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.amVectorScale(nnsVectoR4D, amsAmeNodeUniform.direction, amsAmeNodeUniform.magnitude * amUnitTime);
        if (((int)amsAmeNodeUniform.flag & 2) != 0)
            AppMain.amQuatMultiVector(nnsVectoR4D, nnsVectoR4D, ref ecb.rotate, (AppMain.NNS_VECTOR4D)null);
        AppMain.amVectorAdd(work.position, nnsVectoR4D);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D);
    }

    public static void _amApplyRadial(
      AppMain.AMS_AME_ECB ecb,
      AppMain.AMS_AME_NODE node,
      AppMain.AMS_AME_RUNTIME_WORK work)
    {
        AppMain.AMS_AME_NODE_RADIAL amsAmeNodeRadial = (AppMain.AMS_AME_NODE_RADIAL)node;
        float amUnitTime = AppMain._am_unit_time;
        AppMain.NNS_VECTOR4D nnsVectoR4D1 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.NNS_VECTOR4D nnsVectoR4D2 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.amVectorCopy(nnsVectoR4D2, amsAmeNodeRadial.position);
        if (((int)amsAmeNodeRadial.flag & 1) != 0)
            AppMain.amVectorAdd(nnsVectoR4D2, ecb.translate);
        AppMain.amVectorSub(nnsVectoR4D1, work.position, nnsVectoR4D2);
        float num1 = 1f / (float)Math.Pow((double)AppMain.amVectorScalor(nnsVectoR4D1), (double)amsAmeNodeRadial.attenuation);
        double num2 = (double)AppMain.amVectorScaleUnit(nnsVectoR4D1, nnsVectoR4D1, amsAmeNodeRadial.magnitude * num1 * amUnitTime);
        AppMain.amVectorAdd(work.position, nnsVectoR4D1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D2);
    }

    public static void _amApplyVortex(
      AppMain.AMS_AME_ECB ecb,
      AppMain.AMS_AME_NODE node,
      AppMain.AMS_AME_RUNTIME_WORK work)
    {
        AppMain.AMS_AME_NODE_VORTEX amsAmeNodeVortex = (AppMain.AMS_AME_NODE_VORTEX)node;
        AppMain.NNS_VECTOR4D nnsVectoR4D1 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.NNS_VECTOR4D nnsVectoR4D2 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.NNS_VECTOR4D nnsVectoR4D3 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.NNS_VECTOR4D nnsVectoR4D4 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.amVectorCopy(nnsVectoR4D1, amsAmeNodeVortex.position);
        AppMain.amVectorCopy(nnsVectoR4D3, amsAmeNodeVortex.axis);
        if (((int)amsAmeNodeVortex.flag & 1) != 0)
            AppMain.amVectorAdd(nnsVectoR4D1, ecb.translate);
        if (((int)amsAmeNodeVortex.flag & 2) != 0)
            AppMain.amQuatMultiVector(nnsVectoR4D3, nnsVectoR4D3, ref ecb.rotate, (AppMain.NNS_VECTOR4D)null);
        AppMain.amVectorSub(nnsVectoR4D2, work.position, nnsVectoR4D1);
        AppMain.amVectorOuterProduct(nnsVectoR4D4, nnsVectoR4D3, nnsVectoR4D2);
        AppMain.amVectorScale(nnsVectoR4D2, nnsVectoR4D4, AppMain._am_unit_time);
        AppMain.amVectorAdd(work.velocity, nnsVectoR4D2);
        AppMain.amVectorOuterProduct(nnsVectoR4D4, nnsVectoR4D3, nnsVectoR4D4);
        AppMain.amVectorScale(nnsVectoR4D2, nnsVectoR4D4, AppMain._am_unit_time);
        AppMain.amVectorAdd(work.velocity, nnsVectoR4D2);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D2);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D3);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D4);
    }

    public static void _amApplyDrag(
      AppMain.AMS_AME_ECB ecb,
      AppMain.AMS_AME_NODE node,
      AppMain.AMS_AME_RUNTIME_WORK work)
    {
        AppMain.AMS_AME_NODE_DRAG amsAmeNodeDrag = (AppMain.AMS_AME_NODE_DRAG)node;
        float amUnitTime = AppMain._am_unit_time;
        AppMain.NNS_VECTOR4D nnsVectoR4D1 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.NNS_VECTOR4D nnsVectoR4D2 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.NNS_VECTOR4D nnsVectoR4D3 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.amVectorCopy(nnsVectoR4D3, amsAmeNodeDrag.position);
        if (((int)amsAmeNodeDrag.flag & 1) != 0)
            AppMain.amVectorAdd(nnsVectoR4D3, ecb.translate);
        AppMain.amVectorSub(nnsVectoR4D2, work.position, nnsVectoR4D3);
        double num = (double)AppMain.amVectorUnit(nnsVectoR4D2);
        AppMain.amVectorScale(nnsVectoR4D1, nnsVectoR4D2, (float)((double)amsAmeNodeDrag.magnitude * (double)amUnitTime * (double)amUnitTime * 0.5));
        AppMain.amVectorAdd(work.position, nnsVectoR4D1);
        AppMain.amVectorScale(nnsVectoR4D1, nnsVectoR4D2, amsAmeNodeDrag.magnitude * amUnitTime);
        AppMain.amVectorAdd(work.velocity, nnsVectoR4D1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D2);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D3);
    }

    public static void _amApplyNoise(
      AppMain.AMS_AME_ECB ecb,
      AppMain.AMS_AME_NODE node,
      AppMain.AMS_AME_RUNTIME_WORK work)
    {
        AppMain.AMS_AME_NODE_NOISE amsAmeNodeNoise = (AppMain.AMS_AME_NODE_NOISE)node;
        float num = amsAmeNodeNoise.magnitude * AppMain._am_unit_time;
        AppMain.SNNS_VECTOR4D pSrc;
        pSrc.x = (AppMain.nnRandom() - 0.5f) * num * amsAmeNodeNoise.axis.x;
        pSrc.y = (AppMain.nnRandom() - 0.5f) * num * amsAmeNodeNoise.axis.y;
        pSrc.z = (AppMain.nnRandom() - 0.5f) * num * amsAmeNodeNoise.axis.z;
        pSrc.w = 1f;
        AppMain.amVectorAdd(work.position, ref pSrc);
    }
}