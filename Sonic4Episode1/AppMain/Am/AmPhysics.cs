using System;

public partial class AppMain
{
    public static void _amApplyGravity(
      AMS_AME_ECB ecb,
      AMS_AME_NODE node,
      AMS_AME_RUNTIME_WORK work)
    {
        AMS_AME_NODE_GRAVITY amsAmeNodeGravity = (AMS_AME_NODE_GRAVITY)node;
        float amUnitTime = _am_unit_time;
        NNS_VECTOR4D nnsVectoR4D1 = GlobalPool<NNS_VECTOR4D>.Alloc();
        NNS_VECTOR4D nnsVectoR4D2 = GlobalPool<NNS_VECTOR4D>.Alloc();
        amVectorCopy(nnsVectoR4D2, amsAmeNodeGravity.direction);
        if (((int)amsAmeNodeGravity.flag & 2) != 0)
            amQuatMultiVector(nnsVectoR4D2, nnsVectoR4D2, ref ecb.rotate, null);
        amVectorScale(nnsVectoR4D1, nnsVectoR4D2, (float)(amsAmeNodeGravity.magnitude * (double)amUnitTime * amUnitTime * 0.5));
        amVectorAdd(work.position, nnsVectoR4D1);
        amVectorScale(nnsVectoR4D1, nnsVectoR4D2, amsAmeNodeGravity.magnitude * amUnitTime);
        amVectorAdd(work.velocity, nnsVectoR4D1);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D1);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D2);
    }

    public static void _amApplyUniform(
      AMS_AME_ECB ecb,
      AMS_AME_NODE node,
      AMS_AME_RUNTIME_WORK work)
    {
        AMS_AME_NODE_UNIFORM amsAmeNodeUniform = (AMS_AME_NODE_UNIFORM)node;
        float amUnitTime = _am_unit_time;
        NNS_VECTOR4D nnsVectoR4D = GlobalPool<NNS_VECTOR4D>.Alloc();
        amVectorScale(nnsVectoR4D, amsAmeNodeUniform.direction, amsAmeNodeUniform.magnitude * amUnitTime);
        if (((int)amsAmeNodeUniform.flag & 2) != 0)
            amQuatMultiVector(nnsVectoR4D, nnsVectoR4D, ref ecb.rotate, null);
        amVectorAdd(work.position, nnsVectoR4D);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D);
    }

    public static void _amApplyRadial(
      AMS_AME_ECB ecb,
      AMS_AME_NODE node,
      AMS_AME_RUNTIME_WORK work)
    {
        AMS_AME_NODE_RADIAL amsAmeNodeRadial = (AMS_AME_NODE_RADIAL)node;
        float amUnitTime = _am_unit_time;
        NNS_VECTOR4D nnsVectoR4D1 = GlobalPool<NNS_VECTOR4D>.Alloc();
        NNS_VECTOR4D nnsVectoR4D2 = GlobalPool<NNS_VECTOR4D>.Alloc();
        amVectorCopy(nnsVectoR4D2, amsAmeNodeRadial.position);
        if (((int)amsAmeNodeRadial.flag & 1) != 0)
            amVectorAdd(nnsVectoR4D2, ecb.translate);
        amVectorSub(nnsVectoR4D1, work.position, nnsVectoR4D2);
        float num1 = 1f / (float)Math.Pow(amVectorScalor(nnsVectoR4D1), amsAmeNodeRadial.attenuation);
        double num2 = amVectorScaleUnit(nnsVectoR4D1, nnsVectoR4D1, amsAmeNodeRadial.magnitude * num1 * amUnitTime);
        amVectorAdd(work.position, nnsVectoR4D1);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D1);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D2);
    }

    public static void _amApplyVortex(
      AMS_AME_ECB ecb,
      AMS_AME_NODE node,
      AMS_AME_RUNTIME_WORK work)
    {
        AMS_AME_NODE_VORTEX amsAmeNodeVortex = (AMS_AME_NODE_VORTEX)node;
        NNS_VECTOR4D nnsVectoR4D1 = GlobalPool<NNS_VECTOR4D>.Alloc();
        NNS_VECTOR4D nnsVectoR4D2 = GlobalPool<NNS_VECTOR4D>.Alloc();
        NNS_VECTOR4D nnsVectoR4D3 = GlobalPool<NNS_VECTOR4D>.Alloc();
        NNS_VECTOR4D nnsVectoR4D4 = GlobalPool<NNS_VECTOR4D>.Alloc();
        amVectorCopy(nnsVectoR4D1, amsAmeNodeVortex.position);
        amVectorCopy(nnsVectoR4D3, amsAmeNodeVortex.axis);
        if (((int)amsAmeNodeVortex.flag & 1) != 0)
            amVectorAdd(nnsVectoR4D1, ecb.translate);
        if (((int)amsAmeNodeVortex.flag & 2) != 0)
            amQuatMultiVector(nnsVectoR4D3, nnsVectoR4D3, ref ecb.rotate, null);
        amVectorSub(nnsVectoR4D2, work.position, nnsVectoR4D1);
        amVectorOuterProduct(nnsVectoR4D4, nnsVectoR4D3, nnsVectoR4D2);
        amVectorScale(nnsVectoR4D2, nnsVectoR4D4, _am_unit_time);
        amVectorAdd(work.velocity, nnsVectoR4D2);
        amVectorOuterProduct(nnsVectoR4D4, nnsVectoR4D3, nnsVectoR4D4);
        amVectorScale(nnsVectoR4D2, nnsVectoR4D4, _am_unit_time);
        amVectorAdd(work.velocity, nnsVectoR4D2);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D1);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D2);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D3);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D4);
    }

    public static void _amApplyDrag(
      AMS_AME_ECB ecb,
      AMS_AME_NODE node,
      AMS_AME_RUNTIME_WORK work)
    {
        AMS_AME_NODE_DRAG amsAmeNodeDrag = (AMS_AME_NODE_DRAG)node;
        float amUnitTime = _am_unit_time;
        NNS_VECTOR4D nnsVectoR4D1 = GlobalPool<NNS_VECTOR4D>.Alloc();
        NNS_VECTOR4D nnsVectoR4D2 = GlobalPool<NNS_VECTOR4D>.Alloc();
        NNS_VECTOR4D nnsVectoR4D3 = GlobalPool<NNS_VECTOR4D>.Alloc();
        amVectorCopy(nnsVectoR4D3, amsAmeNodeDrag.position);
        if (((int)amsAmeNodeDrag.flag & 1) != 0)
            amVectorAdd(nnsVectoR4D3, ecb.translate);
        amVectorSub(nnsVectoR4D2, work.position, nnsVectoR4D3);
        double num = amVectorUnit(nnsVectoR4D2);
        amVectorScale(nnsVectoR4D1, nnsVectoR4D2, (float)(amsAmeNodeDrag.magnitude * (double)amUnitTime * amUnitTime * 0.5));
        amVectorAdd(work.position, nnsVectoR4D1);
        amVectorScale(nnsVectoR4D1, nnsVectoR4D2, amsAmeNodeDrag.magnitude * amUnitTime);
        amVectorAdd(work.velocity, nnsVectoR4D1);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D1);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D2);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D3);
    }

    public static void _amApplyNoise(
      AMS_AME_ECB ecb,
      AMS_AME_NODE node,
      AMS_AME_RUNTIME_WORK work)
    {
        AMS_AME_NODE_NOISE amsAmeNodeNoise = (AMS_AME_NODE_NOISE)node;
        float num = amsAmeNodeNoise.magnitude * _am_unit_time;
        SNNS_VECTOR4D pSrc;
        pSrc.x = (nnRandom() - 0.5f) * num * amsAmeNodeNoise.axis.x;
        pSrc.y = (nnRandom() - 0.5f) * num * amsAmeNodeNoise.axis.y;
        pSrc.z = (nnRandom() - 0.5f) * num * amsAmeNodeNoise.axis.z;
        pSrc.w = 1f;
        amVectorAdd(work.position, ref pSrc);
    }
}