using System;
using System.Collections.Generic;
using System.Text;
using mpp;

public partial class AppMain
{
    private void amTrailEFInitialize()
    {
        AppMain._amTrailEF_head.Clear();
        AppMain._amTrailEF_tail.Clear();
        AppMain._amTrailEF_head.pNext = AppMain._amTrailEF_tail;
        AppMain._amTrailEF_head.pPrev = (AppMain.AMS_TRAIL_EFFECT)null;
        AppMain._amTrailEF_tail.pNext = (AppMain.AMS_TRAIL_EFFECT)null;
        AppMain._amTrailEF_tail.pPrev = AppMain._amTrailEF_head;
        AppMain._amTrailEF_alloc = 0;
        AppMain._amTrailEF_free = 0;
        for (int index = 0; index < 8; ++index)
        {
            AppMain._amTrailEF_buf[index].Clear();
            AppMain._amTrailEF_ref[index] = AppMain._amTrailEF_buf[index];
        }
    }

    private static void amTrailEFUpdate(ushort handleId)
    {
        for (AppMain.AMS_TRAIL_EFFECT pNext = AppMain._amTrailEF_head.pNext; pNext != AppMain._amTrailEF_tail; pNext = pNext.pNext)
        {
            if (pNext.Procedure != (DoubleType<AppMain.AMTREffectProc, int>)(AppMain.AMTREffectProc)null && ((int)pNext.handleId & (int)handleId) != 0 && pNext.Procedure != (DoubleType<AppMain.AMTREffectProc, int>)(-1))
            {
                int num = ((AppMain.AMTREffectProc)pNext.Procedure)(pNext);
                if (((int)pNext.Work.state & 32768) != 0)
                {
                    AppMain._amTrailEFDelete(pNext);
                }
                else
                {
                    pNext.fFrame += AppMain.amEffectGetUnitFrame();
                    if ((double)pNext.fEndFrame > 0.0 && (double)pNext.fFrame > (double)pNext.fEndFrame)
                        AppMain._amTrailEFDelete(pNext);
                }
            }
        }
        for (AppMain.AMS_TRAIL_EFFECT pNext = AppMain._amTrailEF_head.pNext; pNext != AppMain._amTrailEF_tail; pNext = pNext.pNext)
        {
            if (pNext.Procedure == (DoubleType<AppMain.AMTREffectProc, int>)(-1))
                AppMain._amTrailEFDeleteEffectReal(pNext);
        }
    }

    private static void amTrailEFDraw(ushort handleId, AppMain.NNS_TEXLIST texlist, uint state)
    {
        for (AppMain.AMS_TRAIL_EFFECT pNext = AppMain._amTrailEF_head.pNext; pNext != AppMain._amTrailEF_tail; pNext = pNext.pNext)
        {
            if (pNext.Procedure != (DoubleType<AppMain.AMTREffectProc, int>)(AppMain.AMTREffectProc)null && ((int)pNext.handleId & (int)handleId) != 0 && pNext.Procedure != (DoubleType<AppMain.AMTREffectProc, int>)(-1))
            {
                pNext.drawState = state;
                pNext.Work.texlist = texlist;
                AppMain._amTrailDrawNormal(pNext);
            }
        }
    }

    private static void amTrailEFDeleteGroup(ushort handleId)
    {
        for (AppMain.AMS_TRAIL_EFFECT pNext = AppMain._amTrailEF_head.pNext; pNext != AppMain._amTrailEF_tail; pNext = pNext.pNext)
        {
            if (((int)pNext.handleId & (int)handleId) != 0)
                AppMain._amTrailEFDelete(pNext);
        }
    }

    private static void amTrailEFOffsetPos(ushort handleId, AppMain.NNS_VECTOR offset)
    {
        for (AppMain.AMS_TRAIL_EFFECT pNext = AppMain._amTrailEF_head.pNext; pNext != AppMain._amTrailEF_tail; pNext = pNext.pNext)
        {
            if (((int)pNext.handleId & (int)handleId) != 0)
                AppMain._amTrailAddPosition(pNext, offset);
        }
    }

    private static void amTrailMakeEffect(AppMain.AMS_TRAIL_PARAM param, ushort handleId, short flag)
    {
        ++AppMain.pTr.trailNum;
        AppMain.AMS_TRAIL_EFFECT pEffect = AppMain._amTrailEFMake(handleId);
        if (pEffect == null)
            return;
        pEffect.Procedure = (DoubleType<AppMain.AMTREffectProc, int>)new AppMain.AMTREffectProc(AppMain._amTrailUpdateNormal);
        pEffect.Destractor = (DoubleType<AppMain.AMTREffectProc, int>)new AppMain.AMTREffectProc(AppMain._amTrailFinalizeNormal);
        pEffect.fEndFrame = -1f;
        pEffect.flag = flag;
        AppMain.AMS_TRAIL_PARAM amsTrailParam = pEffect.Work.Assign(param);
        amsTrailParam.time = amsTrailParam.life * AppMain.amEffectGetUnitFrame();
        amsTrailParam.trailId = AppMain.pTr.trailId;
        if (AppMain.pTr.trailEffect[(int)amsTrailParam.trailId] != null)
            AppMain._amTrailEFDelete(AppMain.pTr.trailEffect[(int)amsTrailParam.trailId]);
        AppMain.pTr.trailEffect[(int)amsTrailParam.trailId] = pEffect;
        AppMain._amTrailInitNormal(pEffect);
        ++AppMain.pTr.trailId;
        if (AppMain.pTr.trailId < (short)8)
            return;
        AppMain.pTr.trailId = (short)0;
    }

    private static void _amTrailEFDelete(AppMain.AMS_TRAIL_EFFECT pEffect)
    {
        pEffect.Procedure = ((DoubleType<AppMain.AMTREffectProc, int>)(-1));
        if (!(pEffect.Destractor != (DoubleType<AppMain.AMTREffectProc, int>)(AppMain.AMTREffectProc)null) || !(pEffect.Destractor != ((DoubleType<AppMain.AMTREffectProc, int>)(-1))))
            return;
        int num = ((AppMain.AMTREffectProc)pEffect.Destractor)(pEffect);
    }

    private static AppMain.AMS_TRAIL_EFFECT _amTrailEFMake(ushort handleId)
    {
        AppMain.AMS_TRAIL_EFFECT amsTrailEffect = AppMain._amTrailEFAlloc();
        if (amsTrailEffect == null)
            return (AppMain.AMS_TRAIL_EFFECT)null;
        AppMain._amTrailEF_tail.pPrev.pNext = amsTrailEffect;
        amsTrailEffect.pPrev = AppMain._amTrailEF_tail.pPrev;
        AppMain._amTrailEF_tail.pPrev = amsTrailEffect;
        amsTrailEffect.pNext = AppMain._amTrailEF_tail;
        amsTrailEffect.handleId = handleId;
        return amsTrailEffect;
    }

    private static AppMain.AMS_TRAIL_EFFECT _amTrailEFAlloc()
    {
        AppMain.AMS_TRAIL_EFFECT amsTrailEffect = AppMain._amTrailEF_ref[AppMain._amTrailEF_alloc];
        ++AppMain._amTrailEF_alloc;
        if (AppMain._amTrailEF_alloc >= 8)
            AppMain._amTrailEF_alloc = 0;
        amsTrailEffect.Clear();
        return amsTrailEffect;
    }

    private static void _amTrailEFFree(AppMain.AMS_TRAIL_EFFECT pEffect)
    {
        AppMain._amTrailEF_ref[AppMain._amTrailEF_free] = pEffect;
        ++AppMain._amTrailEF_free;
        if (AppMain._amTrailEF_free < 8)
            return;
        AppMain._amTrailEF_free = 0;
    }

    private static void _amTrailEFDeleteEffectReal(AppMain.AMS_TRAIL_EFFECT pEffect)
    {
        pEffect.pPrev.pNext = pEffect.pNext;
        pEffect.pNext.pPrev = pEffect.pPrev;
        AppMain._amTrailEFFree(pEffect);
    }

    private static void _amTrailInitNormal(AppMain.AMS_TRAIL_EFFECT pEffect)
    {
        AppMain.AMS_TRAIL_PARAM work = pEffect.Work;
        AppMain.AMS_TRAIL_PARTSDATA amsTrailPartsdata = AppMain.pTr.trailData[(int)work.trailId];
        AppMain.AMS_TRAIL_PARTS part = amsTrailPartsdata.parts[0];
        AppMain.AMS_TRAIL_PARTS trailTail = amsTrailPartsdata.trailTail;
        AppMain.AMS_TRAIL_PARTS trailHead = amsTrailPartsdata.trailHead;
        amsTrailPartsdata.Clear();
        part.pNext = trailTail;
        trailTail.pPrev = part;
        part.pPrev = trailHead;
        trailHead.pNext = part;
        if (((int)pEffect.flag & 1) != 0)
        {
            part.pos.x = AppMain.AMD_FX32_TO_FLOAT(work.trail_pos.x);
            part.pos.y = -AppMain.AMD_FX32_TO_FLOAT(work.trail_pos.y);
            part.pos.z = AppMain.AMD_FX32_TO_FLOAT(work.zBias);
        }
        else
        {
            part.pos.x = MppBitConverter.Int32ToSingle(work.trail_pos.x);
            part.pos.y = MppBitConverter.Int32ToSingle(work.trail_pos.y);
            part.pos.z = MppBitConverter.Int32ToSingle(work.trail_pos.z);
        }
        part.time = work.life;
        part.partsId = (short)0;
        work.trailPartsId = (short)1;
        ++work.trailPartsNum;
    }

    private static int _amTrailFinalizeNormal(AppMain.AMS_TRAIL_EFFECT pEffect)
    {
        AppMain.AMS_TRAIL_PARAM work = pEffect.Work;
        if (AppMain.pTr.trailNum > (short)0)
        {
            --AppMain.pTr.trailNum;
            if (AppMain.pTr.trailNum == (short)0)
                AppMain.pTr.trailState &= short.MaxValue;
        }
        AppMain.pTr.trailEffect[(int)work.trailId] = (AppMain.AMS_TRAIL_EFFECT)null;
        return 0;
    }

    private static int _amTrailUpdateNormal(AppMain.AMS_TRAIL_EFFECT pEffect)
    {
        AppMain.AMS_TRAIL_PARAM work = pEffect.Work;
        AppMain.AMS_TRAIL_PARTSDATA amsTrailPartsdata = AppMain.pTr.trailData[(int)work.trailId];
        AppMain.AMS_TRAIL_PARTS part = amsTrailPartsdata.parts[(int)work.trailPartsId];
        AppMain.AMS_TRAIL_PARTS trailTail = amsTrailPartsdata.trailTail;
        AppMain.AMS_TRAIL_PARTS trailHead = amsTrailPartsdata.trailHead;
        if (((int)work.state & (int)short.MinValue) != 0)
            return 1;
        AppMain.AMS_TRAIL_PARTS pPrev = trailTail.pPrev;
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
        if (((int)pEffect.flag & 1) != 0)
        {
            part.pos.x = AppMain.AMD_FX32_TO_FLOAT(work.trail_pos.x);
            part.pos.y = -AppMain.AMD_FX32_TO_FLOAT(work.trail_pos.y);
            part.pos.z = AppMain.AMD_FX32_TO_FLOAT(work.zBias);
        }
        else
        {
            part.pos.x = MppBitConverter.Int32ToSingle(work.trail_pos.x);
            part.pos.y = MppBitConverter.Int32ToSingle(work.trail_pos.y);
            part.pos.z = MppBitConverter.Int32ToSingle(work.trail_pos.z);
        }
        AppMain.nnSubtractVector(part.dir, part.pos, part.pPrev.pos);
        if (AppMain.amIsZerof(part.dir.x) && AppMain.amIsZerof(part.dir.y) && AppMain.amIsZerof(part.dir.z))
            part.dir.x = 1f;
        AppMain._amTrailAddParts(part, work);
        part.m_Flag |= 1U;
        work.time -= AppMain.amEffectGetUnitFrame();
        if ((double)work.time < 0.0)
        {
            work.time = 0.0f;
            work.state |= short.MinValue;
        }
        return 0;
    }

    private static void _amTrailDrawNormal(AppMain.AMS_TRAIL_EFFECT pEffect)
    {
        AppMain.AMS_TRAIL_PARAM work = pEffect.Work;
        AppMain.AMS_TRAIL_PARTSDATA amsTrailPartsdata = AppMain.pTr.trailData[(int)work.trailId];
        AppMain.AMS_TRAIL_PARTS trailTail = amsTrailPartsdata.trailTail;
        AppMain.AMS_TRAIL_PARTS trailHead = amsTrailPartsdata.trailHead;
        AppMain.AMS_TRAIL_PARTS pPrev1 = trailTail.pPrev;
        if (trailTail.pPrev.pPrev == trailHead || (double)work.time <= 0.0)
            return;
        AppMain.NNS_RGBA startColor = work.startColor;
        AppMain.NNS_RGBA ptclColor = work.ptclColor;
        AppMain.NNS_VECTOR nnsVector1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector3 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        float num1 = 1f;
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        int num2 = 1;
        AppMain.amDrawGetPrimBlendParam((int)work.blendType, setParam);
        if (work.zTest != (short)0)
            setParam.zTest = (short)1;
        if (work.zMask != (short)0)
            setParam.zMask = (short)1;
        AppMain.amVectorSet(nnsVector3, 0.0f, 0.0f, 1f);
        if ((double)work.time < (double)work.vanish_time)
            num1 = work.time / work.vanish_time;
        work.vanish_rate = num1;
        startColor.a = work.startColor.a * num1;
        ptclColor.a = work.ptclColor.a * num1;
        float startSize = work.startSize;
        float ptclSize = work.ptclSize;
        if (work.ptclFlag != (short)0 && work.ptclTexId != (short)-1)
        {
            AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(6);
            AppMain.NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            float num3 = AppMain.nnDistanceVector(pPrev1.pos, AppMain._am_ef_camPos);
            AppMain.mppAssertNotImpl();
            buffer[offset].Col = AppMain.AMD_FCOLTORGBA8888(ptclColor.r, ptclColor.g, ptclColor.b, ptclColor.a);
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
            setParam.texId = (int)work.ptclTexId;
            setParam.count = 6;
            setParam.ablend = num2;
            setParam.sortZ = num3;
            AppMain.amDrawPrimitive3D(pEffect.drawState, setParam);
        }
        if (work.trailPartsNum < (short)3)
            return;
        if (work.texlist == null || work.texId == -1)
        {
            AppMain.NNS_PRIM3D_PC[] _pv = AppMain.amDrawAlloc_NNS_PRIM3D_PC(6 * ((int)work.trailPartsNum - 1));
            AppMain.NNS_PRIM3D_PC[] nnsPriM3DPcArray = _pv;
            int num3 = 0;
            float num4 = AppMain.nnDistanceVector(pPrev1.pos, AppMain._am_ef_camPos);
            AppMain.nnCrossProductVector(nnsVector1, nnsVector3, pPrev1.dir);
            AppMain.nnNormalizeVector(nnsVector1, nnsVector1);
            AppMain.nnScaleVector(nnsVector2, nnsVector1, startSize);
            AppMain.nnAddVector(ref _pv[0].Pos, pPrev1.pos, nnsVector2);
            AppMain.nnAddVector(ref _pv[1].Pos, pPrev1.pPrev.pos, nnsVector2);
            AppMain.nnSubtractVector(ref _pv[2].Pos, pPrev1.pos, nnsVector2);
            AppMain.nnSubtractVector(ref _pv[5].Pos, pPrev1.pPrev.pos, nnsVector2);
            _pv[5].Col = AppMain.AMD_FCOLTORGBA8888(startColor.r, startColor.g, startColor.b, startColor.a);
            _pv[0].Col = _pv[1].Col = _pv[2].Col = _pv[5].Col;
            _pv[3] = _pv[1];
            _pv[4] = _pv[2];
            int pv = num3 + 6;
            AppMain.AMS_TRAIL_PARTS pPrev2 = pPrev1.pPrev;
            work.list_no = (short)1;
            while (pPrev2 != trailHead.pNext)
            {
                AppMain.mppAssertNotImpl();
                pPrev2.m_Flag &= 4294967293U;
                ++work.list_no;
                AppMain._amTrailDrawPartsNormal(pPrev2, work, _pv, pv);
                pPrev2 = pPrev2.pPrev;
                pv += 6;
            }
            setParam.format3D = 2;
            setParam.type = 0;
            setParam.vtxPC3D = nnsPriM3DPcArray;
            setParam.texlist = work.texlist;
            setParam.texId = work.texId;
            setParam.count = 6 * ((int)work.trailPartsNum - 1);
            setParam.ablend = num2;
            setParam.sortZ = num4;
            AppMain.amDrawPrimitive3D(pEffect.drawState, setParam);
        }
        else
        {
            AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(6 * ((int)work.trailPartsNum - 1));
            AppMain.NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            int num3 = offset;
            float num4 = (float)((int)work.trailPartsNum - 1) / (float)work.trailPartsNum * work.vanish_rate;
            float num5 = AppMain.nnDistanceVector(pPrev1.pos, AppMain._am_ef_camPos);
            AppMain.nnCrossProductVector(nnsVector1, nnsVector3, pPrev1.dir);
            AppMain.nnNormalizeVector(nnsVector1, nnsVector1);
            AppMain.nnScaleVector(nnsVector2, nnsVector1, startSize);
            AppMain.nnAddVector(ref buffer[offset].Pos, pPrev1.pos, nnsVector2);
            AppMain.nnAddVector(ref buffer[offset + 1].Pos, pPrev1.pPrev.pos, nnsVector2);
            AppMain.nnSubtractVector(ref buffer[offset + 2].Pos, pPrev1.pos, nnsVector2);
            AppMain.nnSubtractVector(ref buffer[offset + 5].Pos, pPrev1.pPrev.pos, nnsVector2);
            buffer[offset + 5].Col = AppMain.AMD_FCOLTORGBA8888(startColor.r, startColor.g, startColor.b, startColor.a);
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
            AppMain.AMS_TRAIL_PARTS pPrev2 = pPrev1.pPrev;
            work.list_no = (short)1;
            while (pPrev2 != trailHead.pNext)
            {
                pPrev2.m_Flag &= 4294967293U;
                ++work.list_no;
                AppMain._amTrailDrawPartsNormalTex(pPrev2, work, buffer, pv);
                pPrev2 = pPrev2.pPrev;
                pv += 6;
            }
            setParam.format3D = 4;
            setParam.type = 0;
            setParam.vtxPCT3D = nnsPriM3DPctArray;
            setParam.texlist = work.texlist;
            setParam.texId = work.texId;
            setParam.count = 6 * ((int)work.trailPartsNum - 1);
            setParam.ablend = num2;
            setParam.sortZ = num5;
            AppMain.amDrawPrimitive3D(pEffect.drawState, setParam);
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector2);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector3);
    }

    private static void _amTrailDrawPartsNormal(
      AppMain.AMS_TRAIL_PARTS pNow,
      AppMain.AMS_TRAIL_PARAM work,
      AppMain.NNS_PRIM3D_PC[] _pv,
      int pv)
    {
        int num1 = pv - 6;
        float startSize = work.startSize;
        float num2 = (float)((int)work.trailPartsNum - (int)work.list_no) / (float)work.trailPartsNum;
        float scale = (float)((double)work.startSize * (double)num2 + (double)work.endSize * (1.0 - (double)num2));
        float num3 = num2 * work.vanish_rate;
        AppMain.NNS_RGBA nnsRgba;
        nnsRgba.r = (float)((double)work.startColor.r * (double)num3 + (double)work.endColor.r * (1.0 - (double)num3));
        nnsRgba.g = (float)((double)work.startColor.g * (double)num3 + (double)work.endColor.g * (1.0 - (double)num3));
        nnsRgba.b = (float)((double)work.startColor.b * (double)num3 + (double)work.endColor.b * (1.0 - (double)num3));
        nnsRgba.a = (float)((double)work.startColor.a * (double)num3 + (double)work.endColor.a * (1.0 - (double)num3));
        AppMain.NNS_VECTOR nnsVector1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector3 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        double num4 = (double)AppMain.nnDistanceVector(pNow.pos, AppMain._am_ef_camPos);
        AppMain.amVectorSet(nnsVector3, 0.0f, 0.0f, 1f);
        AppMain.nnCrossProductVector(nnsVector1, nnsVector3, pNow.dir);
        AppMain.nnNormalizeVector(nnsVector1, nnsVector1);
        AppMain.nnScaleVector(nnsVector2, nnsVector1, scale);
        AppMain.nnAddVector(ref _pv[pv + 1].Pos, pNow.pPrev.pos, nnsVector2);
        AppMain.nnSubtractVector(ref _pv[pv + 5].Pos, pNow.pPrev.pos, nnsVector2);
        _pv[pv] = _pv[num1 + 1];
        _pv[pv + 2] = _pv[num1 + 5];
        _pv[pv + 4] = _pv[num1 + 2];
        _pv[pv + 5].Col = AppMain.AMD_FCOLTORGBA8888(nnsRgba.r, nnsRgba.g, nnsRgba.b, nnsRgba.a);
        _pv[pv + 1].Col = _pv[pv + 5].Col;
        _pv[pv + 3] = _pv[pv + 1];
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector2);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector3);
    }

    private static void _amTrailDrawPartsNormalTex(
      AppMain.AMS_TRAIL_PARTS pNow,
      AppMain.AMS_TRAIL_PARAM work,
      AppMain.NNS_PRIM3D_PCT[] _pv,
      int pv)
    {
        int num1 = pv - 6;
        float startSize = work.startSize;
        float num2 = (float)((int)work.trailPartsNum - (int)work.list_no) / (float)work.trailPartsNum * work.vanish_rate;
        float scale = (float)((double)work.startSize * (double)num2 + (double)work.endSize * (1.0 - (double)num2));
        AppMain.NNS_RGBA nnsRgba;
        nnsRgba.r = (float)((double)work.startColor.r * (double)num2 + (double)work.endColor.r * (1.0 - (double)num2));
        nnsRgba.g = (float)((double)work.startColor.g * (double)num2 + (double)work.endColor.g * (1.0 - (double)num2));
        nnsRgba.b = (float)((double)work.startColor.b * (double)num2 + (double)work.endColor.b * (1.0 - (double)num2));
        nnsRgba.a = (float)((double)work.startColor.a * (double)num2 + (double)work.endColor.a * (1.0 - (double)num2));
        AppMain.NNS_VECTOR nnsVector1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector3 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        double num3 = (double)AppMain.nnDistanceVector(pNow.pos, AppMain._am_ef_camPos);
        AppMain.amVectorSet(nnsVector3, 0.0f, 0.0f, 1f);
        AppMain.nnCrossProductVector(nnsVector1, nnsVector3, pNow.dir);
        AppMain.nnNormalizeVector(nnsVector1, nnsVector1);
        AppMain.nnScaleVector(nnsVector2, nnsVector1, scale);
        AppMain.nnAddVector(ref _pv[pv + 1].Pos, pNow.pPrev.pos, nnsVector2);
        AppMain.nnSubtractVector(ref _pv[pv + 5].Pos, pNow.pPrev.pos, nnsVector2);
        _pv[pv] = _pv[num1 + 1];
        _pv[pv + 2] = _pv[num1 + 5];
        _pv[pv + 4] = _pv[pv + 2];
        _pv[pv + 5].Col = AppMain.AMD_FCOLTORGBA8888(nnsRgba.r, nnsRgba.g, nnsRgba.b, nnsRgba.a);
        _pv[pv + 1].Col = _pv[pv + 5].Col;
        _pv[pv + 1].Tex.u = num2;
        _pv[pv + 1].Tex.v = 0.0f;
        _pv[pv + 5].Tex.u = num2;
        _pv[pv + 5].Tex.v = 1f;
        _pv[pv + 3] = _pv[pv + 1];
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector2);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector3);
    }

    private static void _amTrailAddParts(AppMain.AMS_TRAIL_PARTS pNew, AppMain.AMS_TRAIL_PARAM work)
    {
        AppMain.AMS_TRAIL_PARTS trailHead = AppMain.pTr.trailData[(int)work.trailId].trailHead;
        pNew.time = work.life;
        pNew.partsId = work.trailPartsId;
        ++work.trailPartsId;
        ++work.trailPartsNum;
        if ((int)work.trailPartsNum >= (int)work.partsNum)
        {
            work.trailPartsNum = work.partsNum;
            work.trailPartsId = trailHead.pNext.partsId;
        }
        if (work.trailPartsNum < (short)64)
            return;
        work.trailPartsNum = (short)64;
        work.trailPartsId = trailHead.pNext.partsId;
    }

    private static void _amTrailAddPosition(
      AppMain.AMS_TRAIL_EFFECT pEffect,
      AppMain.NNS_VECTOR offset)
    {
        AppMain.AMS_TRAIL_PARAM work = pEffect.Work;
        AppMain.AMS_TRAIL_PARTSDATA amsTrailPartsdata = AppMain.pTr.trailData[(int)work.trailId];
        AppMain.AMS_TRAIL_PARTS trailTail = amsTrailPartsdata.trailTail;
        AppMain.AMS_TRAIL_PARTS trailHead = amsTrailPartsdata.trailHead;
        AppMain.AMS_TRAIL_PARTS pPrev = trailTail.pPrev;
        if (trailTail.pPrev.pPrev == trailHead || (double)work.time <= 0.0)
            return;
        for (; pPrev != trailHead; pPrev = pPrev.pPrev)
            AppMain.nnAddVector(pPrev.pos, pPrev.pos, offset);
    }

    private static int _amTrailCalcSplinePos(
      AppMain.NNS_VECTOR[] Pos,
      AppMain.NNS_VECTOR[] Dir,
      AppMain.AMS_TRAIL_PARTS pNPP,
      AppMain.AMS_TRAIL_PARTS pNP,
      AppMain.AMS_TRAIL_PARTS pNow,
      AppMain.AMS_TRAIL_PARTS pNext,
      float len,
      int MaxComp)
    {
        AppMain.AMTRS_FC_PARAM FcWk = new AppMain.AMTRS_FC_PARAM();
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
        return AppMain._amTrailCalcSplinePos(Pos, Dir, FcWk, len, MaxComp);
    }

    private static int _amTrailCalcSplinePos(
      AppMain.NNS_VECTOR[] pos,
      AppMain.NNS_VECTOR[] dir,
      AppMain.AMTRS_FC_PARAM FcWk,
      float len,
      int MaxComp)
    {
        int num = (int)AppMain.amClamp((float)(int)len, 0.0f, (float)MaxComp);
        AppMain._amTrailCalcSpline(FcWk, FcWk.m_x);
        for (int index = 0; index < num; ++index)
        {
            float t = (float)(index + 1) / (float)(num + 1);
            pos[index].x = AppMain._amTrailGetValue(FcWk, t);
        }
        AppMain._amTrailCalcSpline(FcWk, FcWk.m_y);
        for (int index = 0; index < num; ++index)
        {
            float t = (float)(index + 1) / (float)(num + 1);
            pos[index].y = AppMain._amTrailGetValue(FcWk, t);
        }
        AppMain._amTrailCalcSpline(FcWk, FcWk.m_z);
        for (int index = 0; index < num; ++index)
        {
            float t = (float)(index + 1) / (float)(num + 1);
            pos[index].z = AppMain._amTrailGetValue(FcWk, t);
        }
        AppMain._amTrailCalcSpline(FcWk, FcWk.m_Dx);
        for (int index = 0; index < num; ++index)
        {
            float t = (float)(index + 1) / (float)(num + 1);
            dir[index].x = AppMain._amTrailGetValue(FcWk, t);
        }
        AppMain._amTrailCalcSpline(FcWk, FcWk.m_Dy);
        for (int index = 0; index < num; ++index)
        {
            float t = (float)(index + 1) / (float)(num + 1);
            dir[index].y = AppMain._amTrailGetValue(FcWk, t);
        }
        AppMain._amTrailCalcSpline(FcWk, FcWk.m_Dz);
        for (int index = 0; index < num; ++index)
        {
            float t = (float)(index + 1) / (float)(num + 1);
            dir[index].z = AppMain._amTrailGetValue(FcWk, t);
        }
        return num;
    }

    private static void _amTrailCalcSpline(AppMain.AMTRS_FC_PARAM param, float[] P)
    {
        float num1;
        float num2;
        switch (param.m_flag & 3U)
        {
            case 1:
                num1 = (float)(((double)P[2] - (double)P[1]) / 4.0);
                num2 = (float)(((double)P[3] - (double)P[1]) / 1.0);
                break;
            case 2:
                num1 = (float)(((double)P[2] - (double)P[0]) / 1.0);
                num2 = (float)(((double)P[2] - (double)P[1]) / 4.0);
                break;
            default:
                num1 = (float)(((double)P[2] - (double)P[0]) / 2.0);
                num2 = (float)(((double)P[3] - (double)P[1]) / 2.0);
                break;
        }
        param.m_CalcParam.x = (float)(2.0 * (double)P[1] - 2.0 * (double)P[2]) + num1 + num2;
        param.m_CalcParam.y = (float)(-3.0 * (double)P[1] + 3.0 * (double)P[2] - 2.0 * (double)num1) - num2;
        param.m_CalcParam.z = num1;
        param.m_CalcParam.w = P[1];
        param.m_flag |= 256U;
    }

    private static float _amTrailGetValue(AppMain.AMTRS_FC_PARAM param, float t)
    {
        float num = 0.0f;
        if (((int)param.m_flag & 256) == 0)
            return num;
        AppMain.NNS_VECTOR4D calcParam = param.m_CalcParam;
        return (float)((double)t * (double)t * (double)t * (double)calcParam.x + (double)t * (double)t * (double)calcParam.y + (double)t * (double)calcParam.z);
    }
}