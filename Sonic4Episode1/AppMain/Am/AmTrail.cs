using mpp;

public partial class AppMain
{
    private void amTrailEFInitialize()
    {
        _amTrailEF_head.Clear();
        _amTrailEF_tail.Clear();
        _amTrailEF_head.pNext = _amTrailEF_tail;
        _amTrailEF_head.pPrev = null;
        _amTrailEF_tail.pNext = null;
        _amTrailEF_tail.pPrev = _amTrailEF_head;
        _amTrailEF_alloc = 0;
        _amTrailEF_free = 0;
        for (int index = 0; index < 8; ++index)
        {
            _amTrailEF_buf[index].Clear();
            _amTrailEF_ref[index] = _amTrailEF_buf[index];
        }
    }

    private static void amTrailEFUpdate(ushort handleId)
    {
        for (AMS_TRAIL_EFFECT pNext = _amTrailEF_head.pNext; pNext != _amTrailEF_tail; pNext = pNext.pNext)
        {
            if (pNext.Procedure != null && (pNext.handleId & handleId) != 0 && pNext.Procedure != -1)
            {
                int num = ((AMTREffectProc)pNext.Procedure)(pNext);
                if ((pNext.Work.state & 32768) != 0)
                {
                    _amTrailEFDelete(pNext);
                }
                else
                {
                    pNext.fFrame += amEffectGetUnitFrame();
                    if (pNext.fEndFrame > 0.0 && pNext.fFrame > (double)pNext.fEndFrame)
                        _amTrailEFDelete(pNext);
                }
            }
        }
        for (AMS_TRAIL_EFFECT pNext = _amTrailEF_head.pNext; pNext != _amTrailEF_tail; pNext = pNext.pNext)
        {
            if (pNext.Procedure == -1)
                _amTrailEFDeleteEffectReal(pNext);
        }
    }

    private static void amTrailEFDraw(ushort handleId, NNS_TEXLIST texlist, uint state)
    {
        for (AMS_TRAIL_EFFECT pNext = _amTrailEF_head.pNext; pNext != _amTrailEF_tail; pNext = pNext.pNext)
        {
            if (pNext.Procedure != null && (pNext.handleId & handleId) != 0 && pNext.Procedure != -1)
            {
                pNext.drawState = state;
                pNext.Work.texlist = texlist;
                _amTrailDrawNormal(pNext);
            }
        }
    }

    private static void amTrailEFDeleteGroup(ushort handleId)
    {
        for (AMS_TRAIL_EFFECT pNext = _amTrailEF_head.pNext; pNext != _amTrailEF_tail; pNext = pNext.pNext)
        {
            if ((pNext.handleId & handleId) != 0)
                _amTrailEFDelete(pNext);
        }
    }

    private static void amTrailEFOffsetPos(ushort handleId, NNS_VECTOR offset)
    {
        for (AMS_TRAIL_EFFECT pNext = _amTrailEF_head.pNext; pNext != _amTrailEF_tail; pNext = pNext.pNext)
        {
            if ((pNext.handleId & handleId) != 0)
                _amTrailAddPosition(pNext, offset);
        }
    }

    private static void amTrailMakeEffect(AMS_TRAIL_PARAM param, ushort handleId, short flag)
    {
        ++pTr.trailNum;
        AMS_TRAIL_EFFECT pEffect = _amTrailEFMake(handleId);
        if (pEffect == null)
            return;
        pEffect.Procedure = new AMTREffectProc(_amTrailUpdateNormal);
        pEffect.Destractor = new AMTREffectProc(_amTrailFinalizeNormal);
        pEffect.fEndFrame = -1f;
        pEffect.flag = flag;
        AMS_TRAIL_PARAM amsTrailParam = pEffect.Work.Assign(param);
        amsTrailParam.time = amsTrailParam.life * amEffectGetUnitFrame();
        amsTrailParam.trailId = pTr.trailId;
        if (pTr.trailEffect[amsTrailParam.trailId] != null)
            _amTrailEFDelete(pTr.trailEffect[amsTrailParam.trailId]);
        pTr.trailEffect[amsTrailParam.trailId] = pEffect;
        _amTrailInitNormal(pEffect);
        ++pTr.trailId;
        if (pTr.trailId < 8)
            return;
        pTr.trailId = 0;
    }

    private static void _amTrailEFDelete(AMS_TRAIL_EFFECT pEffect)
    {
        pEffect.Procedure = -1;
        if (!(pEffect.Destractor != null) || !(pEffect.Destractor != -1))
            return;
        int num = ((AMTREffectProc)pEffect.Destractor)(pEffect);
    }

    private static AMS_TRAIL_EFFECT _amTrailEFMake(ushort handleId)
    {
        AMS_TRAIL_EFFECT amsTrailEffect = _amTrailEFAlloc();
        if (amsTrailEffect == null)
            return null;
        _amTrailEF_tail.pPrev.pNext = amsTrailEffect;
        amsTrailEffect.pPrev = _amTrailEF_tail.pPrev;
        _amTrailEF_tail.pPrev = amsTrailEffect;
        amsTrailEffect.pNext = _amTrailEF_tail;
        amsTrailEffect.handleId = handleId;
        return amsTrailEffect;
    }

    private static AMS_TRAIL_EFFECT _amTrailEFAlloc()
    {
        AMS_TRAIL_EFFECT amsTrailEffect = _amTrailEF_ref[_amTrailEF_alloc];
        ++_amTrailEF_alloc;
        if (_amTrailEF_alloc >= 8)
            _amTrailEF_alloc = 0;
        amsTrailEffect.Clear();
        return amsTrailEffect;
    }

    private static void _amTrailEFFree(AMS_TRAIL_EFFECT pEffect)
    {
        _amTrailEF_ref[_amTrailEF_free] = pEffect;
        ++_amTrailEF_free;
        if (_amTrailEF_free < 8)
            return;
        _amTrailEF_free = 0;
    }

    private static void _amTrailEFDeleteEffectReal(AMS_TRAIL_EFFECT pEffect)
    {
        pEffect.pPrev.pNext = pEffect.pNext;
        pEffect.pNext.pPrev = pEffect.pPrev;
        _amTrailEFFree(pEffect);
    }

    private static void _amTrailInitNormal(AMS_TRAIL_EFFECT pEffect)
    {
        AMS_TRAIL_PARAM work = pEffect.Work;
        AMS_TRAIL_PARTSDATA amsTrailPartsdata = pTr.trailData[work.trailId];
        AMS_TRAIL_PARTS part = amsTrailPartsdata.parts[0];
        AMS_TRAIL_PARTS trailTail = amsTrailPartsdata.trailTail;
        AMS_TRAIL_PARTS trailHead = amsTrailPartsdata.trailHead;
        amsTrailPartsdata.Clear();
        part.pNext = trailTail;
        trailTail.pPrev = part;
        part.pPrev = trailHead;
        trailHead.pNext = part;
        if ((pEffect.flag & 1) != 0)
        {
            part.pos.x = AMD_FX32_TO_FLOAT(work.trail_pos.x);
            part.pos.y = -AMD_FX32_TO_FLOAT(work.trail_pos.y);
            part.pos.z = AMD_FX32_TO_FLOAT(work.zBias);
        }
        else
        {
            part.pos.x = MppBitConverter.Int32ToSingle(work.trail_pos.x);
            part.pos.y = MppBitConverter.Int32ToSingle(work.trail_pos.y);
            part.pos.z = MppBitConverter.Int32ToSingle(work.trail_pos.z);
        }
        part.time = work.life;
        part.partsId = 0;
        work.trailPartsId = 1;
        ++work.trailPartsNum;
    }

    private static int _amTrailFinalizeNormal(AMS_TRAIL_EFFECT pEffect)
    {
        AMS_TRAIL_PARAM work = pEffect.Work;
        if (pTr.trailNum > 0)
        {
            --pTr.trailNum;
            if (pTr.trailNum == 0)
                pTr.trailState &= short.MaxValue;
        }
        pTr.trailEffect[work.trailId] = null;
        return 0;
    }

    private static int _amTrailUpdateNormal(AMS_TRAIL_EFFECT pEffect)
    {
        AMS_TRAIL_PARAM work = pEffect.Work;
        AMS_TRAIL_PARTSDATA amsTrailPartsdata = pTr.trailData[work.trailId];
        AMS_TRAIL_PARTS part = amsTrailPartsdata.parts[work.trailPartsId];
        AMS_TRAIL_PARTS trailTail = amsTrailPartsdata.trailTail;
        AMS_TRAIL_PARTS trailHead = amsTrailPartsdata.trailHead;
        if ((work.state & short.MinValue) != 0)
            return 1;
        AMS_TRAIL_PARTS pPrev = trailTail.pPrev;
        if (part.pNext != null && part == trailHead.pNext)
        {
            trailHead.pNext.pNext.pPrev = trailHead;
            trailHead.pNext = trailHead.pNext.pNext;
        }
        part.Clear();
        part.pNext = trailTail;
        part.pPrev = trailTail.pPrev;
        trailTail.pPrev = part;
        part.pPrev.pNext = part;
        if ((pEffect.flag & 1) != 0)
        {
            part.pos.x = AMD_FX32_TO_FLOAT(work.trail_pos.x);
            part.pos.y = -AMD_FX32_TO_FLOAT(work.trail_pos.y);
            part.pos.z = AMD_FX32_TO_FLOAT(work.zBias);
        }
        else
        {
            part.pos.x = MppBitConverter.Int32ToSingle(work.trail_pos.x);
            part.pos.y = MppBitConverter.Int32ToSingle(work.trail_pos.y);
            part.pos.z = MppBitConverter.Int32ToSingle(work.trail_pos.z);
        }
        nnSubtractVector(part.dir, part.pos, part.pPrev.pos);
        if (amIsZerof(part.dir.x) && amIsZerof(part.dir.y) && amIsZerof(part.dir.z))
            part.dir.x = 1f;
        _amTrailAddParts(part, work);
        part.m_Flag |= 1U;
        work.time -= amEffectGetUnitFrame();
        if (work.time < 0.0)
        {
            work.time = 0.0f;
            work.state |= short.MinValue;
        }
        return 0;
    }

    private static void _amTrailDrawNormal(AMS_TRAIL_EFFECT pEffect)
    {
        AMS_TRAIL_PARAM work = pEffect.Work;
        AMS_TRAIL_PARTSDATA amsTrailPartsdata = pTr.trailData[work.trailId];
        AMS_TRAIL_PARTS trailTail = amsTrailPartsdata.trailTail;
        AMS_TRAIL_PARTS trailHead = amsTrailPartsdata.trailHead;
        AMS_TRAIL_PARTS pPrev1 = trailTail.pPrev;
        if (trailTail.pPrev.pPrev == trailHead || work.time <= 0.0)
            return;
        NNS_RGBA startColor = work.startColor;
        NNS_RGBA ptclColor = work.ptclColor;
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector3 = GlobalPool<NNS_VECTOR>.Alloc();
        float num1 = 1f;
        AMS_PARAM_DRAW_PRIMITIVE setParam = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        int num2 = 1;
        amDrawGetPrimBlendParam(work.blendType, setParam);
        if (work.zTest != 0)
            setParam.zTest = 1;
        if (work.zMask != 0)
            setParam.zMask = 1;
        amVectorSet(nnsVector3, 0.0f, 0.0f, 1f);
        if (work.time < (double)work.vanish_time)
            num1 = work.time / work.vanish_time;
        work.vanish_rate = num1;
        startColor.a = work.startColor.a * num1;
        ptclColor.a = work.ptclColor.a * num1;
        float startSize = work.startSize;
        float ptclSize = work.ptclSize;
        if (work.ptclFlag != 0 && work.ptclTexId != -1)
        {
            NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = amDrawAlloc_NNS_PRIM3D_PCT(6);
            NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            float num3 = nnDistanceVector(pPrev1.pos, _am_ef_camPos);
            mppAssertNotImpl();
            buffer[offset].Col = AMD_FCOLTORGBA8888(ptclColor.r, ptclColor.g, ptclColor.b, ptclColor.a);
            buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 5].Col = buffer[offset].Col;
            buffer[offset].Tex.u = 0.0f;
            buffer[offset].Tex.v = 0.0f;
            buffer[offset + 1].Tex.u = 1f;
            buffer[offset + 1].Tex.v = 0.0f;
            buffer[offset + 2].Tex.u = 0.0f;
            buffer[offset + 2].Tex.v = 1f;
            buffer[offset + 5].Tex.u = 1f;
            buffer[offset + 5].Tex.v = 1f;
            buffer[offset + 3] = buffer[offset + 1];
            buffer[offset + 4] = buffer[offset + 2];
            setParam.format3D = 4;
            setParam.type = 0;
            setParam.vtxPCT3D = nnsPriM3DPctArray;
            setParam.texlist = work.texlist;
            setParam.texId = work.ptclTexId;
            setParam.count = 6;
            setParam.ablend = num2;
            setParam.sortZ = num3;
            amDrawPrimitive3D(pEffect.drawState, setParam);
        }
        if (work.trailPartsNum < 3)
            return;
        if (work.texlist == null || work.texId == -1)
        {
            NNS_PRIM3D_PC[] _pv = amDrawAlloc_NNS_PRIM3D_PC(6 * (work.trailPartsNum - 1));
            NNS_PRIM3D_PC[] nnsPriM3DPcArray = _pv;
            int num3 = 0;
            float num4 = nnDistanceVector(pPrev1.pos, _am_ef_camPos);
            nnCrossProductVector(nnsVector1, nnsVector3, pPrev1.dir);
            nnNormalizeVector(nnsVector1, nnsVector1);
            nnScaleVector(nnsVector2, nnsVector1, startSize);
            nnAddVector(ref _pv[0].Pos, pPrev1.pos, nnsVector2);
            nnAddVector(ref _pv[1].Pos, pPrev1.pPrev.pos, nnsVector2);
            nnSubtractVector(ref _pv[2].Pos, pPrev1.pos, nnsVector2);
            nnSubtractVector(ref _pv[5].Pos, pPrev1.pPrev.pos, nnsVector2);
            _pv[5].Col = AMD_FCOLTORGBA8888(startColor.r, startColor.g, startColor.b, startColor.a);
            _pv[0].Col = _pv[1].Col = _pv[2].Col = _pv[5].Col;
            _pv[3] = _pv[1];
            _pv[4] = _pv[2];
            int pv = num3 + 6;
            AMS_TRAIL_PARTS pPrev2 = pPrev1.pPrev;
            work.list_no = 1;
            while (pPrev2 != trailHead.pNext)
            {
                mppAssertNotImpl();
                pPrev2.m_Flag &= 4294967293U;
                ++work.list_no;
                _amTrailDrawPartsNormal(pPrev2, work, _pv, pv);
                pPrev2 = pPrev2.pPrev;
                pv += 6;
            }
            setParam.format3D = 2;
            setParam.type = 0;
            setParam.vtxPC3D = nnsPriM3DPcArray;
            setParam.texlist = work.texlist;
            setParam.texId = work.texId;
            setParam.count = 6 * (work.trailPartsNum - 1);
            setParam.ablend = num2;
            setParam.sortZ = num4;
            amDrawPrimitive3D(pEffect.drawState, setParam);
        }
        else
        {
            NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = amDrawAlloc_NNS_PRIM3D_PCT(6 * (work.trailPartsNum - 1));
            NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            int num3 = offset;
            float num4 = (work.trailPartsNum - 1) / (float)work.trailPartsNum * work.vanish_rate;
            float num5 = nnDistanceVector(pPrev1.pos, _am_ef_camPos);
            nnCrossProductVector(nnsVector1, nnsVector3, pPrev1.dir);
            nnNormalizeVector(nnsVector1, nnsVector1);
            nnScaleVector(nnsVector2, nnsVector1, startSize);
            nnAddVector(ref buffer[offset].Pos, pPrev1.pos, nnsVector2);
            nnAddVector(ref buffer[offset + 1].Pos, pPrev1.pPrev.pos, nnsVector2);
            nnSubtractVector(ref buffer[offset + 2].Pos, pPrev1.pos, nnsVector2);
            nnSubtractVector(ref buffer[offset + 5].Pos, pPrev1.pPrev.pos, nnsVector2);
            buffer[offset + 5].Col = AMD_FCOLTORGBA8888(startColor.r, startColor.g, startColor.b, startColor.a);
            buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 5].Col;
            buffer[offset].Tex.u = 1f;
            buffer[offset].Tex.v = 0.0f;
            buffer[offset + 1].Tex.u = num4;
            buffer[offset + 1].Tex.v = 0.0f;
            buffer[offset + 2].Tex.u = 1f;
            buffer[offset + 2].Tex.v = 1f;
            buffer[offset + 5].Tex.u = num4;
            buffer[offset + 5].Tex.v = 1f;
            buffer[offset + 3] = buffer[offset + 1];
            buffer[offset + 4] = buffer[offset + 2];
            int pv = num3 + 6;
            AMS_TRAIL_PARTS pPrev2 = pPrev1.pPrev;
            work.list_no = 1;
            while (pPrev2 != trailHead.pNext)
            {
                pPrev2.m_Flag &= 4294967293U;
                ++work.list_no;
                _amTrailDrawPartsNormalTex(pPrev2, work, buffer, pv);
                pPrev2 = pPrev2.pPrev;
                pv += 6;
            }
            setParam.format3D = 4;
            setParam.type = 0;
            setParam.vtxPCT3D = nnsPriM3DPctArray;
            setParam.texlist = work.texlist;
            setParam.texId = work.texId;
            setParam.count = 6 * (work.trailPartsNum - 1);
            setParam.ablend = num2;
            setParam.sortZ = num5;
            amDrawPrimitive3D(pEffect.drawState, setParam);
        }
        GlobalPool<NNS_VECTOR>.Release(nnsVector1);
        GlobalPool<NNS_VECTOR>.Release(nnsVector2);
        GlobalPool<NNS_VECTOR>.Release(nnsVector3);
    }

    private static void _amTrailDrawPartsNormal(
      AMS_TRAIL_PARTS pNow,
      AMS_TRAIL_PARAM work,
      NNS_PRIM3D_PC[] _pv,
      int pv)
    {
        int num1 = pv - 6;
        float startSize = work.startSize;
        float num2 = (work.trailPartsNum - work.list_no) / (float)work.trailPartsNum;
        float scale = (float)(work.startSize * (double)num2 + work.endSize * (1.0 - num2));
        float num3 = num2 * work.vanish_rate;
        NNS_RGBA nnsRgba;
        nnsRgba.r = (float)(work.startColor.r * (double)num3 + work.endColor.r * (1.0 - num3));
        nnsRgba.g = (float)(work.startColor.g * (double)num3 + work.endColor.g * (1.0 - num3));
        nnsRgba.b = (float)(work.startColor.b * (double)num3 + work.endColor.b * (1.0 - num3));
        nnsRgba.a = (float)(work.startColor.a * (double)num3 + work.endColor.a * (1.0 - num3));
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector3 = GlobalPool<NNS_VECTOR>.Alloc();
        double num4 = nnDistanceVector(pNow.pos, _am_ef_camPos);
        amVectorSet(nnsVector3, 0.0f, 0.0f, 1f);
        nnCrossProductVector(nnsVector1, nnsVector3, pNow.dir);
        nnNormalizeVector(nnsVector1, nnsVector1);
        nnScaleVector(nnsVector2, nnsVector1, scale);
        nnAddVector(ref _pv[pv + 1].Pos, pNow.pPrev.pos, nnsVector2);
        nnSubtractVector(ref _pv[pv + 5].Pos, pNow.pPrev.pos, nnsVector2);
        _pv[pv] = _pv[num1 + 1];
        _pv[pv + 2] = _pv[num1 + 5];
        _pv[pv + 4] = _pv[num1 + 2];
        _pv[pv + 5].Col = AMD_FCOLTORGBA8888(nnsRgba.r, nnsRgba.g, nnsRgba.b, nnsRgba.a);
        _pv[pv + 1].Col = _pv[pv + 5].Col;
        _pv[pv + 3] = _pv[pv + 1];
        GlobalPool<NNS_VECTOR>.Release(nnsVector1);
        GlobalPool<NNS_VECTOR>.Release(nnsVector2);
        GlobalPool<NNS_VECTOR>.Release(nnsVector3);
    }

    private static void _amTrailDrawPartsNormalTex(
      AMS_TRAIL_PARTS pNow,
      AMS_TRAIL_PARAM work,
      NNS_PRIM3D_PCT[] _pv,
      int pv)
    {
        int num1 = pv - 6;
        float startSize = work.startSize;
        float num2 = (work.trailPartsNum - work.list_no) / (float)work.trailPartsNum * work.vanish_rate;
        float scale = (float)(work.startSize * (double)num2 + work.endSize * (1.0 - num2));
        NNS_RGBA nnsRgba;
        nnsRgba.r = (float)(work.startColor.r * (double)num2 + work.endColor.r * (1.0 - num2));
        nnsRgba.g = (float)(work.startColor.g * (double)num2 + work.endColor.g * (1.0 - num2));
        nnsRgba.b = (float)(work.startColor.b * (double)num2 + work.endColor.b * (1.0 - num2));
        nnsRgba.a = (float)(work.startColor.a * (double)num2 + work.endColor.a * (1.0 - num2));
        NNS_VECTOR nnsVector1 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector2 = GlobalPool<NNS_VECTOR>.Alloc();
        NNS_VECTOR nnsVector3 = GlobalPool<NNS_VECTOR>.Alloc();
        double num3 = nnDistanceVector(pNow.pos, _am_ef_camPos);
        amVectorSet(nnsVector3, 0.0f, 0.0f, 1f);
        nnCrossProductVector(nnsVector1, nnsVector3, pNow.dir);
        nnNormalizeVector(nnsVector1, nnsVector1);
        nnScaleVector(nnsVector2, nnsVector1, scale);
        nnAddVector(ref _pv[pv + 1].Pos, pNow.pPrev.pos, nnsVector2);
        nnSubtractVector(ref _pv[pv + 5].Pos, pNow.pPrev.pos, nnsVector2);
        _pv[pv] = _pv[num1 + 1];
        _pv[pv + 2] = _pv[num1 + 5];
        _pv[pv + 4] = _pv[pv + 2];
        _pv[pv + 5].Col = AMD_FCOLTORGBA8888(nnsRgba.r, nnsRgba.g, nnsRgba.b, nnsRgba.a);
        _pv[pv + 1].Col = _pv[pv + 5].Col;
        _pv[pv + 1].Tex.u = num2;
        _pv[pv + 1].Tex.v = 0.0f;
        _pv[pv + 5].Tex.u = num2;
        _pv[pv + 5].Tex.v = 1f;
        _pv[pv + 3] = _pv[pv + 1];
        GlobalPool<NNS_VECTOR>.Release(nnsVector1);
        GlobalPool<NNS_VECTOR>.Release(nnsVector2);
        GlobalPool<NNS_VECTOR>.Release(nnsVector3);
    }

    private static void _amTrailAddParts(AMS_TRAIL_PARTS pNew, AMS_TRAIL_PARAM work)
    {
        AMS_TRAIL_PARTS trailHead = pTr.trailData[work.trailId].trailHead;
        pNew.time = work.life;
        pNew.partsId = work.trailPartsId;
        ++work.trailPartsId;
        ++work.trailPartsNum;
        if (work.trailPartsNum >= work.partsNum)
        {
            work.trailPartsNum = work.partsNum;
            work.trailPartsId = trailHead.pNext.partsId;
        }
        if (work.trailPartsNum < 64)
            return;
        work.trailPartsNum = 64;
        work.trailPartsId = trailHead.pNext.partsId;
    }

    private static void _amTrailAddPosition(
      AMS_TRAIL_EFFECT pEffect,
      NNS_VECTOR offset)
    {
        AMS_TRAIL_PARAM work = pEffect.Work;
        AMS_TRAIL_PARTSDATA amsTrailPartsdata = pTr.trailData[work.trailId];
        AMS_TRAIL_PARTS trailTail = amsTrailPartsdata.trailTail;
        AMS_TRAIL_PARTS trailHead = amsTrailPartsdata.trailHead;
        AMS_TRAIL_PARTS pPrev = trailTail.pPrev;
        if (trailTail.pPrev.pPrev == trailHead || work.time <= 0.0)
            return;
        for (; pPrev != trailHead; pPrev = pPrev.pPrev)
            nnAddVector(pPrev.pos, pPrev.pos, offset);
    }

    private static int _amTrailCalcSplinePos(
      NNS_VECTOR[] Pos,
      NNS_VECTOR[] Dir,
      AMS_TRAIL_PARTS pNPP,
      AMS_TRAIL_PARTS pNP,
      AMS_TRAIL_PARTS pNow,
      AMS_TRAIL_PARTS pNext,
      float len,
      int MaxComp)
    {
        AMTRS_FC_PARAM FcWk = new AMTRS_FC_PARAM();
        FcWk.m_flag = 0U;
        if (pNPP != null)
        {
            FcWk.m_x[0] = pNPP.pos.x;
            FcWk.m_y[0] = pNPP.pos.y;
            FcWk.m_z[0] = pNPP.pos.z;
        }
        else
            FcWk.m_flag |= 1U;
        FcWk.m_x[1] = pNP.pos.x;
        FcWk.m_y[1] = pNP.pos.y;
        FcWk.m_z[1] = pNP.pos.z;
        FcWk.m_x[2] = pNow.pos.x;
        FcWk.m_y[2] = pNow.pos.y;
        FcWk.m_z[2] = pNow.pos.z;
        if (pNext != null)
        {
            FcWk.m_x[3] = pNext.pos.x;
            FcWk.m_y[3] = pNext.pos.y;
            FcWk.m_z[3] = pNext.pos.z;
        }
        else
            FcWk.m_flag |= 2U;
        FcWk.m_flag |= 512U;
        if (pNPP != null)
        {
            FcWk.m_Dx[0] = pNPP.dir.x;
            FcWk.m_Dy[0] = pNPP.dir.y;
            FcWk.m_Dz[0] = pNPP.dir.z;
        }
        else
            FcWk.m_flag |= 1U;
        FcWk.m_Dx[1] = pNP.dir.x;
        FcWk.m_Dy[1] = pNP.dir.y;
        FcWk.m_Dz[1] = pNP.dir.z;
        FcWk.m_Dx[2] = pNow.dir.x;
        FcWk.m_Dy[2] = pNow.dir.y;
        FcWk.m_Dz[2] = pNow.dir.z;
        if (pNext != null)
        {
            FcWk.m_Dx[3] = pNext.dir.x;
            FcWk.m_Dy[3] = pNext.dir.y;
            FcWk.m_Dz[3] = pNext.dir.z;
        }
        else
            FcWk.m_flag |= 2U;
        FcWk.m_flag |= 512U;
        return _amTrailCalcSplinePos(Pos, Dir, FcWk, len, MaxComp);
    }

    private static int _amTrailCalcSplinePos(
      NNS_VECTOR[] pos,
      NNS_VECTOR[] dir,
      AMTRS_FC_PARAM FcWk,
      float len,
      int MaxComp)
    {
        int num = (int)amClamp((int)len, 0.0f, MaxComp);
        _amTrailCalcSpline(FcWk, FcWk.m_x);
        for (int index = 0; index < num; ++index)
        {
            float t = (index + 1) / (float)(num + 1);
            pos[index].x = _amTrailGetValue(FcWk, t);
        }
        _amTrailCalcSpline(FcWk, FcWk.m_y);
        for (int index = 0; index < num; ++index)
        {
            float t = (index + 1) / (float)(num + 1);
            pos[index].y = _amTrailGetValue(FcWk, t);
        }
        _amTrailCalcSpline(FcWk, FcWk.m_z);
        for (int index = 0; index < num; ++index)
        {
            float t = (index + 1) / (float)(num + 1);
            pos[index].z = _amTrailGetValue(FcWk, t);
        }
        _amTrailCalcSpline(FcWk, FcWk.m_Dx);
        for (int index = 0; index < num; ++index)
        {
            float t = (index + 1) / (float)(num + 1);
            dir[index].x = _amTrailGetValue(FcWk, t);
        }
        _amTrailCalcSpline(FcWk, FcWk.m_Dy);
        for (int index = 0; index < num; ++index)
        {
            float t = (index + 1) / (float)(num + 1);
            dir[index].y = _amTrailGetValue(FcWk, t);
        }
        _amTrailCalcSpline(FcWk, FcWk.m_Dz);
        for (int index = 0; index < num; ++index)
        {
            float t = (index + 1) / (float)(num + 1);
            dir[index].z = _amTrailGetValue(FcWk, t);
        }
        return num;
    }

    private static void _amTrailCalcSpline(AMTRS_FC_PARAM param, float[] P)
    {
        float num1;
        float num2;
        switch (param.m_flag & 3U)
        {
            case 1:
                num1 = (float)((P[2] - (double)P[1]) / 4.0);
                num2 = (float)((P[3] - (double)P[1]) / 1.0);
                break;
            case 2:
                num1 = (float)((P[2] - (double)P[0]) / 1.0);
                num2 = (float)((P[2] - (double)P[1]) / 4.0);
                break;
            default:
                num1 = (float)((P[2] - (double)P[0]) / 2.0);
                num2 = (float)((P[3] - (double)P[1]) / 2.0);
                break;
        }
        param.m_CalcParam.x = (float)(2.0 * P[1] - 2.0 * P[2]) + num1 + num2;
        param.m_CalcParam.y = (float)(-3.0 * P[1] + 3.0 * P[2] - 2.0 * num1) - num2;
        param.m_CalcParam.z = num1;
        param.m_CalcParam.w = P[1];
        param.m_flag |= 256U;
    }

    private static float _amTrailGetValue(AMTRS_FC_PARAM param, float t)
    {
        float num = 0.0f;
        if (((int)param.m_flag & 256) == 0)
            return num;
        NNS_VECTOR4D calcParam = param.m_CalcParam;
        return (float)(t * (double)t * t * calcParam.x + t * (double)t * calcParam.y + t * (double)calcParam.z);
    }
}