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
    private static int _amInitCircle(object p)
    {
        AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = (AppMain.AMS_AME_CREATE_PARAM)p;
        AppMain.AMS_AME_NODE_CIRCLE node = (AppMain.AMS_AME_NODE_CIRCLE)amsAmeCreateParam.node;
        AppMain.AMS_AME_RUNTIME_WORK_CIRCLE work = (AppMain.AMS_AME_RUNTIME_WORK_CIRCLE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        AppMain.amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        AppMain.amVectorAdd(work.position, node.translate);
        AppMain.amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        AppMain.amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        work.rotate = node.rotate;
        float sizeRate = amsAmeCreateParam.ecb.size_rate;
        work.spread = node.spread;
        work.radius = node.radius * sizeRate;
        work.offset = node.offset * sizeRate;
        work.offset_chaos = node.offset_chaos * sizeRate;
        return 0;
    }

    private static int _amUpdateCircle(object rr)
    {
        AppMain.AMS_AME_RUNTIME amsAmeRuntime1 = (AppMain.AMS_AME_RUNTIME)rr;
        AppMain.AMS_AME_NODE_CIRCLE node = (AppMain.AMS_AME_NODE_CIRCLE)amsAmeRuntime1.node;
        AppMain.AMS_AME_RUNTIME_WORK_CIRCLE work = (AppMain.AMS_AME_RUNTIME_WORK_CIRCLE)amsAmeRuntime1.work;
        work.time += AppMain._am_unit_frame;
        if ((double)work.time <= 0.0)
            return 0;
        if ((double)node.life != -1.0 && (double)work.time >= (double)node.life)
            return 1;
        AppMain.NNS_VECTOR4D nnsVectoR4D1 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.amVectorScale(nnsVectoR4D1, work.velocity, AppMain._am_unit_time);
        AppMain.amVectorAdd(work.position, nnsVectoR4D1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D1);
        float sizeRate = amsAmeRuntime1.ecb.size_rate;
        work.spread += node.spread_variation * AppMain._am_unit_time;
        if ((double)node.radius_variation != 0.0)
            work.radius += node.radius_variation * AppMain._am_unit_time;
        else
            work.radius = node.radius * sizeRate;
        work.offset = node.offset * sizeRate;
        work.offset_chaos = node.offset_chaos * sizeRate;
        AppMain.NNS_MATRIX amUpdateCircleMtx = AppMain._amUpdateCircle_mtx;
        AppMain.nnMakeUnitMatrix(amUpdateCircleMtx);
        AppMain.amMatrixPush(amUpdateCircleMtx);
        AppMain.NNS_QUATERNION rotate = work.rotate;
        AppMain.amQuatToMatrix((AppMain.NNS_MATRIX)null, ref rotate, (AppMain.NNS_VECTOR4D)null);
        work.rotate = rotate;
        AppMain.NNS_VECTOR4D nnsVectoR4D2 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.NNS_VECTOR4D nnsVectoR4D3 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.NNS_VECTOR4D nnsVectoR4D4 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.AMS_AME_LIST next = amsAmeRuntime1.child_head.next;
        for (AppMain.AMS_AME_LIST childTail = amsAmeRuntime1.child_tail; next != childTail; next = next.next)
        {
            AppMain.AMS_AME_RUNTIME amsAmeRuntime2 = (AppMain.AMS_AME_RUNTIME)next;
            amsAmeRuntime2.amount += node.frequency * AppMain._am_unit_frame;
            while ((double)amsAmeRuntime2.amount >= 1.0)
            {
                --amsAmeRuntime2.amount;
                ++amsAmeRuntime2.count;
                if ((double)node.max_count != -1.0 && (double)((int)amsAmeRuntime2.work_num + (int)amsAmeRuntime2.active_num) < (double)node.max_count)
                {
                    AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = new AppMain.AMS_AME_CREATE_PARAM();
                    AppMain.amVectorSet(nnsVectoR4D4, 0.0f, 1f, 0.0f);
                    float radius = work.radius;
                    int angle = (int)((double)AppMain.nnRandom() * 10000000.0);
                    if (((int)node.flag & 1) == 0)
                        radius *= AppMain.nnRandom();
                    else if (((int)node.flag & 2) != 0)
                    {
                        int maxCount = (int)node.max_count;
                        angle = (int)((long)((int)ushort.MaxValue / maxCount) * ((long)amsAmeRuntime2.count % (long)maxCount));
                    }
                    float pSn;
                    float pCs;
                    AppMain.amSinCos(angle, out pSn, out pCs);
                    AppMain.amVectorSet(nnsVectoR4D2, pSn * radius, 0.0f, pCs * radius);
                    AppMain.NNS_VECTOR4D nnsVectoR4D5 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
                    AppMain.NNS_QUATERNION pQuat = new AppMain.NNS_QUATERNION();
                    AppMain.amVectorOuterProduct(nnsVectoR4D5, nnsVectoR4D4, nnsVectoR4D2);
                    double num = (double)AppMain.amVectorUnit(nnsVectoR4D5);
                    AppMain.amQuatRotAxisToQuat(ref pQuat, nnsVectoR4D5, AppMain.NNM_DEGtoRAD((int)work.spread));
                    AppMain.amQuatMultiVector(nnsVectoR4D4, nnsVectoR4D4, ref pQuat, (AppMain.NNS_VECTOR4D)null);
                    AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D5);
                    AppMain.amMatrixCalcVector(nnsVectoR4D2, nnsVectoR4D2);
                    AppMain.amMatrixCalcVector(nnsVectoR4D4, nnsVectoR4D4);
                    AppMain.amVectorScale(nnsVectoR4D3, nnsVectoR4D4, work.offset + work.offset_chaos * AppMain.nnRandom());
                    AppMain.amVectorAdd(nnsVectoR4D2, nnsVectoR4D3);
                    AppMain.amVectorScale(nnsVectoR4D3, nnsVectoR4D4, node.speed + node.speed_chaos * AppMain.nnRandom());
                    amsAmeCreateParam.ecb = amsAmeRuntime1.ecb;
                    amsAmeCreateParam.runtime = amsAmeRuntime2;
                    amsAmeCreateParam.node = amsAmeRuntime2.node;
                    amsAmeCreateParam.parent_position = work.position;
                    amsAmeCreateParam.parent_velocity = work.velocity;
                    amsAmeCreateParam.position = nnsVectoR4D2;
                    amsAmeCreateParam.velocity = nnsVectoR4D3;
                    switch (AppMain.AMD_AME_NODE_TYPE(amsAmeRuntime2.node) & 65280)
                    {
                        case 256:
                            AppMain._amCreateEmitter(amsAmeCreateParam);
                            continue;
                        case 512:
                            AppMain._amCreateParticle(amsAmeCreateParam);
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D2);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D3);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D4);
        AppMain.amMatrixPop();
        return 0;
    }

    private static int _amDrawCircle(object r)
    {
        return 0;
    }

    public static int _amInitSimpleSprite(object p)
    {
        AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = (AppMain.AMS_AME_CREATE_PARAM)p;
        AppMain.AMS_AME_NODE_SPRITE node = (AppMain.AMS_AME_NODE_SPRITE)amsAmeCreateParam.node;
        AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE work = (AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        work.set_color(node.color_start.r, node.color_start.g, node.color_start.b, (byte)((int)node.color_start.a * amsAmeCreateParam.ecb.transparency >> 8));
        AppMain.amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        AppMain.amVectorAdd(work.position, node.translate);
        AppMain.amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        AppMain.amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        float z = node.size + node.size_chaos * AppMain.nnRandom();
        work.set_size(z * node.scale_x_start, z * node.scale_y_start, z, 0.0f);
        if (((int)node.flag & 32768) != 0)
        {
            work.tex_time = 0.0f;
            work.tex_no = 0;
            if (((int)node.flag & 524288) != 0)
                work.tex_no = (int)(100.0 * (double)AppMain.nnRandom()) % node.tex_anim.key_num;
            AppMain.AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = node.tex_anim.key_buf[work.tex_no];
            Vector4 vector4 = new Vector4(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
            work.set_st(vector4.X, vector4.Y, vector4.Z, vector4.W);
        }
        else if (((int)node.flag & 8192) != 0)
            work.set_st(node.cropping_l, node.cropping_t, node.cropping_r, node.cropping_b);
        else
            work.set_st(0.0f, 0.0f, 1f, 1f);
        if (((int)node.flag & 1048576) != 0 || ((int)node.flag & 131072) != 0 && (double)AppMain.nnRandom() > 0.5)
        {
            work.flag |= 8U;
            work.set_st(work.st.z, work.st.y, work.st.x, work.st.w);
        }
        if (((int)node.flag & 2097152) != 0 || ((int)node.flag & 262144) != 0 && (double)AppMain.nnRandom() > 0.5)
        {
            work.flag |= 16U;
            work.set_st(work.st.x, work.st.w, work.st.z, work.st.y);
        }
        return 0;
    }

    public static int _amUpdateSimpleSprite(object r)
    {
        AppMain.AMS_AME_RUNTIME runtime = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_SPRITE node = (AppMain.AMS_AME_NODE_SPRITE)runtime.node;
        AppMain.AMS_AME_LIST next = runtime.active_head.next;
        AppMain.AMS_AME_LIST activeTail = runtime.active_tail;
        int transparency = runtime.ecb.transparency;
        float num1;
        float num2;
        if ((double)node.life >= 0.0)
        {
            num1 = node.life;
            num2 = 1f / num1;
        }
        else
        {
            num1 = float.MaxValue;
            num2 = 0.0f;
        }
        float sizeRate = runtime.ecb.size_rate;
        float num3 = node.scale_x_start * sizeRate;
        float num4 = node.scale_y_start * sizeRate;
        float num5 = node.scale_x_end * sizeRate;
        float num6 = node.scale_y_end * sizeRate;
        for (; next != activeTail; next = next.next)
        {
            AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE workSimpleSprite = (AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)(AppMain.AMS_AME_RUNTIME_WORK)next;
            workSimpleSprite.time += AppMain._am_unit_frame;
            float rate = workSimpleSprite.time * num2;
            AppMain.NNS_VECTOR4D amEffectVel = AppMain._amEffect_vel;
            AppMain.amVectorScale(amEffectVel, workSimpleSprite.velocity, AppMain._am_unit_time);
            AppMain.amVectorAdd(workSimpleSprite.position, amEffectVel);
            if ((double)workSimpleSprite.time >= (double)num1)
            {
                if (runtime.spawn_runtime != null)
                    AppMain._amCreateSpawnParticle(runtime, (AppMain.AMS_AME_RUNTIME_WORK)workSimpleSprite);
                AppMain.amEffectDisconnectLink((AppMain.AMS_AME_LIST)workSimpleSprite);
                --runtime.active_num;
                AppMain.amEffectFreeRuntimeWork((AppMain.AMS_AME_RUNTIME_WORK)workSimpleSprite);
            }
            else
            {
                float num7 = 1f - rate;
                float num8 = (float)((double)num3 * (double)num7 + (double)num5 * (double)rate);
                float num9 = (float)((double)num4 * (double)num7 + (double)num6 * (double)rate);
                AppMain.Vector4D_Buf size = workSimpleSprite.size;
                workSimpleSprite.set_size(size.z * num8, size.z * num9, size.z, size.w);
                AppMain.AMS_RGBA8888 pCO;
                AppMain.amEffectLerpColor(out pCO, ref node.color_start, ref node.color_end, rate);
                pCO.a = (byte)((int)pCO.a * transparency >> 8);
                workSimpleSprite.set_color(pCO.color);
                if (((int)node.flag & 32768) != 0)
                {
                    AppMain.AMS_AME_TEX_ANIM texAnim = node.tex_anim;
                    if (((int)workSimpleSprite.flag & 2) == 0)
                    {
                        workSimpleSprite.tex_time += AppMain._am_unit_frame;
                        if ((double)workSimpleSprite.tex_time >= (double)texAnim.key_buf[workSimpleSprite.tex_no].time)
                        {
                            workSimpleSprite.tex_time = 0.0f;
                            ++workSimpleSprite.tex_no;
                            if (workSimpleSprite.tex_no == texAnim.key_num)
                            {
                                if (((int)node.flag & 65536) != 0)
                                {
                                    workSimpleSprite.tex_no = 0;
                                }
                                else
                                {
                                    workSimpleSprite.tex_no = texAnim.key_num - 1;
                                    workSimpleSprite.flag |= 2U;
                                }
                            }
                        }
                    }
                    AppMain.AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = texAnim.key_buf[workSimpleSprite.tex_no];
                    Vector4 vector4 = new Vector4(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
                    if (((int)workSimpleSprite.flag & 8) != 0)
                    {
                        float x = vector4.X;
                        vector4.X = vector4.Z;
                        vector4.Z = x;
                    }
                    if (((int)workSimpleSprite.flag & 16) != 0)
                    {
                        float y = vector4.Y;
                        vector4.Y = vector4.W;
                        vector4.W = y;
                    }
                    workSimpleSprite.set_st(vector4.X, vector4.Y, vector4.Z, vector4.W);
                }
                else if (((int)node.flag & 16384) != 0)
                {
                    float num10 = node.scroll_u * AppMain._am_unit_time;
                    float num11 = node.scroll_v * AppMain._am_unit_time;
                    if (((int)workSimpleSprite.flag & 8) != 0)
                        num10 = -num10;
                    if (((int)workSimpleSprite.flag & 16) != 0)
                        num11 = -num11;
                    AppMain.Vector4D_Quat st = workSimpleSprite.st;
                    Vector4 vector4 = new Vector4(st.x + num10, st.y + num11, st.z + num10, st.w + num11);
                    workSimpleSprite.set_st(vector4.X, vector4.Y, vector4.Z, vector4.W);
                }
            }
        }
        return 0;
    }

    public static int _amDrawSimpleSprite(object r)
    {
        AppMain.AMS_AME_RUNTIME runtime = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_SPRITE node = (AppMain.AMS_AME_NODE_SPRITE)runtime.node;
        AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE next1 = (AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)(AppMain.AMS_AME_RUNTIME_WORK)runtime.active_head.next;
        AppMain.AMS_AME_LIST next2 = runtime.active_head.next;
        AppMain.AMS_AME_LIST activeTail = runtime.active_tail;
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.amDrawAlloc_AMS_PARAM_DRAW_PRIMITIVE();
        int num1 = AppMain._amEffectSetDrawMode(runtime, setParam, node.blend);
        AppMain.SNNS_VECTOR snnsVector1 = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector2 = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector3 = new AppMain.SNNS_VECTOR();
        float zBias = node.z_bias;
        AppMain.amVectorSet(ref snnsVector1, zBias * AppMain._am_ef_worldViewMtx.M20, zBias * AppMain._am_ef_worldViewMtx.M21, zBias * AppMain._am_ef_worldViewMtx.M22);
        AppMain.amVectorSet(ref snnsVector2, AppMain._am_ef_worldViewMtx.M10, AppMain._am_ef_worldViewMtx.M11, AppMain._am_ef_worldViewMtx.M12);
        AppMain.amVectorSet(ref snnsVector3, AppMain._am_ef_worldViewMtx.M00, AppMain._am_ef_worldViewMtx.M01, AppMain._am_ef_worldViewMtx.M02);
        AppMain.SNNS_VECTOR snnsVector4 = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector5 = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector6 = new AppMain.SNNS_VECTOR();
        if (((int)node.flag & 4096) != 0)
        {
            AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(6 * (int)runtime.active_num);
            AppMain.NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            float num2 = 0.0f;
            for (; next2 != activeTail; next2 = next2.next)
            {
                AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE workSimpleSprite = (AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)(AppMain.AMS_AME_RUNTIME_WORK)next2;
                AppMain.nnScaleVector(ref snnsVector5, ref snnsVector3, workSimpleSprite.size.y);
                AppMain.nnScaleVector(ref snnsVector6, ref snnsVector2, workSimpleSprite.size.x);
                AppMain.amVectorAdd(ref snnsVector4, workSimpleSprite.position, ref snnsVector1);
                num2 = AppMain.nnDistanceVector(ref snnsVector4, AppMain._am_ef_camPos);
                AppMain.nnSubtractVector(ref buffer[offset + 2].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnAddVector(ref buffer[offset + 2].Pos, ref buffer[offset + 2].Pos, ref snnsVector6);
                AppMain.nnAddVector(ref buffer[offset].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnAddVector(ref buffer[offset].Pos, ref buffer[offset].Pos, ref snnsVector6);
                AppMain.nnSubtractVector(ref buffer[offset + 5].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnSubtractVector(ref buffer[offset + 5].Pos, ref buffer[offset + 5].Pos, ref snnsVector6);
                AppMain.nnAddVector(ref buffer[offset + 1].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnSubtractVector(ref buffer[offset + 1].Pos, ref buffer[offset + 1].Pos, ref snnsVector6);
                buffer[offset + 5].Col = AppMain.AMD_RGBA8888(workSimpleSprite.color.r, workSimpleSprite.color.g, workSimpleSprite.color.b, workSimpleSprite.color.a);
                buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 5].Col;
                AppMain.Vector4D_Quat st = workSimpleSprite.st;
                buffer[offset].Tex.u = st.x;
                buffer[offset].Tex.v = st.y;
                buffer[offset + 1].Tex.u = st.z;
                buffer[offset + 1].Tex.v = st.y;
                buffer[offset + 2].Tex.u = st.x;
                buffer[offset + 2].Tex.v = st.w;
                buffer[offset + 5].Tex.u = st.z;
                buffer[offset + 5].Tex.v = st.w;
                buffer[offset + 3] = buffer[offset + 1];
                buffer[offset + 4] = buffer[offset + 2];
                offset += 6;
            }
            setParam.format3D = 4;
            setParam.type = 0;
            setParam.vtxPCT3D = nnsPriM3DPctArray;
            setParam.texlist = runtime.texlist;
            setParam.texId = (int)node.texture_id;
            setParam.count = 6 * (int)runtime.active_num;
            setParam.ablend = num1;
            setParam.sortZ = num2;
            AppMain.amDrawPrimitive3D(runtime.ecb.drawState, setParam);
        }
        else
        {
            AppMain.NNS_PRIM3D_PC[] nnsPriM3DPcArray1 = AppMain.amDrawAlloc_NNS_PRIM3D_PC(6 * (int)runtime.active_num);
            int index = 0;
            AppMain.NNS_PRIM3D_PC[] nnsPriM3DPcArray2 = nnsPriM3DPcArray1;
            float num2 = 0.0f;
            for (; next2 != activeTail; next2 = next2.next)
            {
                AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE workSimpleSprite = (AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)(AppMain.AMS_AME_RUNTIME_WORK)next2;
                AppMain.nnScaleVector(ref snnsVector5, ref snnsVector3, workSimpleSprite.size.x);
                AppMain.nnScaleVector(ref snnsVector6, ref snnsVector2, workSimpleSprite.size.y);
                AppMain.amVectorAdd(ref snnsVector4, workSimpleSprite.position, ref snnsVector1);
                num2 = AppMain.nnDistanceVector(ref snnsVector4, AppMain._am_ef_camPos);
                AppMain.nnSubtractVector(ref nnsPriM3DPcArray1[index + 2].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnAddVector(ref nnsPriM3DPcArray1[index + 2].Pos, ref nnsPriM3DPcArray1[index + 2].Pos, ref snnsVector6);
                AppMain.nnAddVector(ref nnsPriM3DPcArray1[index].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnAddVector(ref nnsPriM3DPcArray1[index].Pos, ref nnsPriM3DPcArray1[index].Pos, ref snnsVector6);
                AppMain.nnSubtractVector(ref nnsPriM3DPcArray1[index + 5].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnSubtractVector(ref nnsPriM3DPcArray1[index + 5].Pos, ref nnsPriM3DPcArray1[index + 5].Pos, ref snnsVector6);
                AppMain.nnAddVector(ref nnsPriM3DPcArray1[index + 1].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnSubtractVector(ref nnsPriM3DPcArray1[index + 1].Pos, ref nnsPriM3DPcArray1[index + 1].Pos, ref snnsVector6);
                nnsPriM3DPcArray1[index + 5].Col = AppMain.AMD_RGBA8888(workSimpleSprite.color.r, workSimpleSprite.color.g, workSimpleSprite.color.b, workSimpleSprite.color.a);
                nnsPriM3DPcArray1[index].Col = nnsPriM3DPcArray1[index + 1].Col = nnsPriM3DPcArray1[index + 2].Col = nnsPriM3DPcArray1[index + 5].Col;
                nnsPriM3DPcArray1[index + 3] = nnsPriM3DPcArray1[index + 1];
                nnsPriM3DPcArray1[index + 4] = nnsPriM3DPcArray1[index + 2];
                index += 6;
            }
            setParam.format3D = 2;
            setParam.type = 0;
            setParam.vtxPC3D = nnsPriM3DPcArray2;
            setParam.texlist = runtime.texlist;
            setParam.texId = -1;
            setParam.count = 6 * (int)runtime.active_num;
            setParam.ablend = num1;
            setParam.sortZ = num2;
            AppMain.amDrawPrimitive3D(runtime.ecb.drawState, setParam);
        }
        return 0;
    }

    public static int _amInitSprite(object p)
    {
        AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = (AppMain.AMS_AME_CREATE_PARAM)p;
        AppMain.AMS_AME_NODE_SPRITE node = (AppMain.AMS_AME_NODE_SPRITE)amsAmeCreateParam.node;
        AppMain.AMS_AME_RUNTIME_WORK_SPRITE work = (AppMain.AMS_AME_RUNTIME_WORK_SPRITE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        AppMain.AMS_RGBA8888 colorStart = node.color_start;
        colorStart.color = node.color_start.color;
        colorStart.a = (byte)((int)colorStart.a * amsAmeCreateParam.ecb.transparency >> 8);
        work.set_color(colorStart.color);
        AppMain.amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        AppMain.amVectorAdd(work.position, node.translate);
        AppMain.amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        AppMain.amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        float z = node.size + node.size_chaos * AppMain.nnRandom();
        work.set_size(z * node.scale_x_start, z * node.scale_x_start, z, 0.0f);
        work.twist = node.twist_angle + node.twist_angle_chaos * AppMain.nnRandom();
        if (((int)node.flag & 4) != 0 && (double)AppMain.nnRandom() > 0.5)
            work.flag |= 4U;
        work.twist_speed = ((int)work.flag & 4) == 0 ? node.twist_angle_speed : -node.twist_angle_speed;
        if (((int)node.flag & 32768) != 0)
        {
            work.tex_time = 0.0f;
            work.tex_no = 0;
            if (((int)node.flag & 524288) != 0)
                work.tex_no = (int)(100.0 * (double)AppMain.nnRandom()) % node.tex_anim.key_num;
            AppMain.AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = node.tex_anim.key_buf[work.tex_no];
            work.set_st(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
        }
        else if (((int)node.flag & 8192) != 0)
            work.set_st(node.cropping_l, node.cropping_t, node.cropping_r, node.cropping_b);
        else
            work.set_st(0.0f, 0.0f, 1f, 1f);
        if (((int)node.flag & 1048576) != 0 || ((int)node.flag & 131072) != 0 && (double)AppMain.nnRandom() > 0.5)
        {
            work.flag |= 8U;
            work.set_st(work.st.z, work.st.y, work.st.x, work.st.w);
        }
        if (((int)node.flag & 2097152) != 0 || ((int)node.flag & 262144) != 0 && (double)AppMain.nnRandom() > 0.5)
        {
            work.flag |= 16U;
            work.set_st(work.st.x, work.st.w, work.st.z, work.st.y);
        }
        return 0;
    }

    public static int _amUpdateSprite(object r)
    {
        AppMain.AMS_AME_RUNTIME runtime = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_SPRITE node = (AppMain.AMS_AME_NODE_SPRITE)runtime.node;
        AppMain.AMS_AME_LIST activeTail = runtime.active_tail;
        AppMain.AMS_AME_LIST next = runtime.active_head.next;
        int transparency = runtime.ecb.transparency;
        float num1;
        float num2;
        if ((double)node.life >= 0.0)
        {
            num1 = node.life;
            num2 = 1f / num1;
        }
        else
        {
            num1 = float.MaxValue;
            num2 = 0.0f;
        }
        float sizeRate = runtime.ecb.size_rate;
        float num3 = node.scale_x_start * sizeRate;
        float num4 = node.scale_y_start * sizeRate;
        float num5 = node.scale_x_end * sizeRate;
        float num6 = node.scale_y_end * sizeRate;
        AppMain.NNS_VECTOR4D nnsVectoR4D = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        for (; next != activeTail; next = next.next)
        {
            AppMain.AMS_AME_RUNTIME_WORK_SPRITE runtimeWorkSprite = (AppMain.AMS_AME_RUNTIME_WORK_SPRITE)(AppMain.AMS_AME_RUNTIME_WORK)next;
            runtimeWorkSprite.time += AppMain._am_unit_frame;
            float rate = runtimeWorkSprite.time * num2;
            AppMain.amVectorScale(nnsVectoR4D, runtimeWorkSprite.velocity, AppMain._am_unit_time);
            AppMain.amVectorAdd(runtimeWorkSprite.position, nnsVectoR4D);
            if ((double)runtimeWorkSprite.time >= (double)num1)
            {
                if (runtime.spawn_runtime != null)
                    AppMain._amCreateSpawnParticle(runtime, (AppMain.AMS_AME_RUNTIME_WORK)runtimeWorkSprite);
                AppMain.amEffectDisconnectLink((AppMain.AMS_AME_LIST)runtimeWorkSprite);
                --runtime.active_num;
                AppMain.amEffectFreeRuntimeWork((AppMain.AMS_AME_RUNTIME_WORK)runtimeWorkSprite);
            }
            else
            {
                float num7 = 1f - rate;
                float num8 = (float)((double)num3 * (double)num7 + (double)num5 * (double)rate);
                float num9 = (float)((double)num4 * (double)num7 + (double)num6 * (double)rate);
                runtimeWorkSprite.set_size(runtimeWorkSprite.size.z * num8, runtimeWorkSprite.size.z * num9, runtimeWorkSprite.size.z, runtimeWorkSprite.size.w);
                runtimeWorkSprite.twist += runtimeWorkSprite.twist_speed * AppMain._am_unit_time;
                AppMain.AMS_RGBA8888 pCO;
                AppMain.amEffectLerpColor(out pCO, ref node.color_start, ref node.color_end, rate);
                pCO.a = (byte)((int)pCO.a * transparency >> 8);
                runtimeWorkSprite.set_color(pCO.color);
                if (((int)node.flag & 32768) != 0)
                {
                    AppMain.AMS_AME_TEX_ANIM texAnim = node.tex_anim;
                    if (((int)runtimeWorkSprite.flag & 2) == 0)
                    {
                        runtimeWorkSprite.tex_time += AppMain._am_unit_frame;
                        if ((double)runtimeWorkSprite.tex_time >= (double)texAnim.key_buf[runtimeWorkSprite.tex_no].time)
                        {
                            runtimeWorkSprite.tex_time = 0.0f;
                            ++runtimeWorkSprite.tex_no;
                            if (runtimeWorkSprite.tex_no == texAnim.key_num)
                            {
                                if (((int)node.flag & 65536) != 0)
                                {
                                    runtimeWorkSprite.tex_no = 0;
                                }
                                else
                                {
                                    runtimeWorkSprite.tex_no = texAnim.key_num - 1;
                                    runtimeWorkSprite.flag |= 2U;
                                }
                            }
                        }
                    }
                    AppMain.AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = texAnim.key_buf[runtimeWorkSprite.tex_no];
                    runtimeWorkSprite.set_st(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
                    if (((int)runtimeWorkSprite.flag & 8) != 0)
                        runtimeWorkSprite.set_st(runtimeWorkSprite.st.z, runtimeWorkSprite.st.y, runtimeWorkSprite.st.x, runtimeWorkSprite.st.w);
                    if (((int)runtimeWorkSprite.flag & 16) != 0)
                        runtimeWorkSprite.set_st(runtimeWorkSprite.st.x, runtimeWorkSprite.st.w, runtimeWorkSprite.st.z, runtimeWorkSprite.st.y);
                }
                else if (((int)node.flag & 16384) != 0)
                {
                    float num10 = node.scroll_u * AppMain._am_unit_time;
                    float num11 = node.scroll_v * AppMain._am_unit_time;
                    if (((int)runtimeWorkSprite.flag & 8) != 0)
                        num10 = -num10;
                    if (((int)runtimeWorkSprite.flag & 16) != 0)
                        num11 = -num11;
                    runtimeWorkSprite.set_st(runtimeWorkSprite.st.x + num10, runtimeWorkSprite.st.y + num11, runtimeWorkSprite.st.z + num10, runtimeWorkSprite.st.w + num11);
                }
            }
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D);
        return 0;
    }

    public static int _amDrawSprite(object r)
    {
        AppMain.AMS_AME_RUNTIME runtime = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_SPRITE node = (AppMain.AMS_AME_NODE_SPRITE)runtime.node;
        AppMain.AMS_AME_LIST next = runtime.active_head.next;
        AppMain.AMS_AME_LIST activeTail = runtime.active_tail;
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        int num1 = AppMain._amEffectSetDrawMode(runtime, setParam, node.blend);
        AppMain.SNNS_VECTOR snnsVector1 = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector2 = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector3 = new AppMain.SNNS_VECTOR();
        float zBias = node.z_bias;
        AppMain.amVectorSet(ref snnsVector1, zBias * AppMain._am_ef_worldViewMtx.M20, zBias * AppMain._am_ef_worldViewMtx.M21, zBias * AppMain._am_ef_worldViewMtx.M22);
        AppMain.amVectorSet(ref snnsVector2, AppMain._am_ef_worldViewMtx.M10, AppMain._am_ef_worldViewMtx.M11, AppMain._am_ef_worldViewMtx.M12);
        AppMain.amVectorSet(ref snnsVector3, AppMain._am_ef_worldViewMtx.M00, AppMain._am_ef_worldViewMtx.M01, AppMain._am_ef_worldViewMtx.M02);
        AppMain.SNNS_VECTOR snnsVector4 = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector5 = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector6 = new AppMain.SNNS_VECTOR();
        if (((int)node.flag & 4096) != 0)
        {
            AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(6 * (int)runtime.active_num);
            AppMain.NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            float num2 = 0.0f;
            for (; next != activeTail; next = next.next)
            {
                AppMain.AMS_AME_RUNTIME_WORK_SPRITE runtimeWorkSprite = (AppMain.AMS_AME_RUNTIME_WORK_SPRITE)(AppMain.AMS_AME_RUNTIME_WORK)next;
                float pSn;
                float pCs;
                AppMain.amSinCos(runtimeWorkSprite.twist, out pSn, out pCs);
                AppMain.amVectorGetAverage(ref snnsVector5, ref snnsVector3, ref snnsVector2, pCs, -pSn);
                AppMain.amVectorGetAverage(ref snnsVector6, ref snnsVector3, ref snnsVector2, pSn, pCs);
                AppMain.nnScaleVector(ref snnsVector5, ref snnsVector5, runtimeWorkSprite.size.x);
                AppMain.nnScaleVector(ref snnsVector6, ref snnsVector6, runtimeWorkSprite.size.y);
                AppMain.amVectorAdd(ref snnsVector4, runtimeWorkSprite.position, ref snnsVector1);
                num2 = AppMain.nnDistanceVector(ref snnsVector4, AppMain._am_ef_camPos);
                AppMain.nnSubtractVector(ref buffer[offset].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnAddVector(ref buffer[offset].Pos, ref buffer[offset].Pos, ref snnsVector6);
                AppMain.nnAddVector(ref buffer[offset + 1].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnAddVector(ref buffer[offset + 1].Pos, ref buffer[offset + 1].Pos, ref snnsVector6);
                AppMain.nnSubtractVector(ref buffer[offset + 2].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnSubtractVector(ref buffer[offset + 2].Pos, ref buffer[offset + 2].Pos, ref snnsVector6);
                AppMain.nnAddVector(ref buffer[offset + 5].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnSubtractVector(ref buffer[offset + 5].Pos, ref buffer[offset + 5].Pos, ref snnsVector6);
                buffer[offset + 5].Col = AppMain.AMD_RGBA8888(runtimeWorkSprite.color.r, runtimeWorkSprite.color.g, runtimeWorkSprite.color.b, runtimeWorkSprite.color.a);
                buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 5].Col;
                buffer[offset].Tex.u = runtimeWorkSprite.st.x;
                buffer[offset].Tex.v = runtimeWorkSprite.st.y;
                buffer[offset + 1].Tex.u = runtimeWorkSprite.st.z;
                buffer[offset + 1].Tex.v = runtimeWorkSprite.st.y;
                buffer[offset + 2].Tex.u = runtimeWorkSprite.st.x;
                buffer[offset + 2].Tex.v = runtimeWorkSprite.st.w;
                buffer[offset + 5].Tex.u = runtimeWorkSprite.st.z;
                buffer[offset + 5].Tex.v = runtimeWorkSprite.st.w;
                buffer[offset + 3] = buffer[offset + 1];
                buffer[offset + 4] = buffer[offset + 2];
                offset += 6;
            }
            setParam.format3D = 4;
            setParam.type = 0;
            setParam.vtxPCT3D = nnsPriM3DPctArray;
            setParam.texlist = runtime.texlist;
            setParam.texId = (int)node.texture_id;
            setParam.count = 6 * (int)runtime.active_num;
            setParam.ablend = num1;
            setParam.sortZ = num2;
            AppMain.amDrawPrimitive3D(runtime.ecb.drawState, setParam);
        }
        else
        {
            AppMain.NNS_PRIM3D_PC[] nnsPriM3DPcArray1 = AppMain.amDrawAlloc_NNS_PRIM3D_PC(6 * (int)runtime.active_num);
            int index = 0;
            AppMain.NNS_PRIM3D_PC[] nnsPriM3DPcArray2 = nnsPriM3DPcArray1;
            float num2 = 0.0f;
            for (; next != activeTail; next = next.next)
            {
                AppMain.AMS_AME_RUNTIME_WORK_SPRITE runtimeWorkSprite = (AppMain.AMS_AME_RUNTIME_WORK_SPRITE)(AppMain.AMS_AME_RUNTIME_WORK)next;
                float pSn;
                float pCs;
                AppMain.amSinCos(runtimeWorkSprite.twist, out pSn, out pCs);
                AppMain.amVectorGetAverage(ref snnsVector5, ref snnsVector3, ref snnsVector2, pCs, -pSn);
                AppMain.amVectorGetAverage(ref snnsVector6, ref snnsVector3, ref snnsVector2, pSn, pCs);
                AppMain.nnScaleVector(ref snnsVector5, ref snnsVector5, runtimeWorkSprite.size.x);
                AppMain.nnScaleVector(ref snnsVector6, ref snnsVector6, runtimeWorkSprite.size.y);
                AppMain.amVectorAdd(ref snnsVector4, runtimeWorkSprite.position, ref snnsVector1);
                num2 = AppMain.nnDistanceVector(ref snnsVector4, AppMain._am_ef_camPos);
                AppMain.nnSubtractVector(ref nnsPriM3DPcArray1[index].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnAddVector(ref nnsPriM3DPcArray1[index].Pos, ref nnsPriM3DPcArray1[index].Pos, ref snnsVector6);
                AppMain.nnAddVector(ref nnsPriM3DPcArray1[index + 1].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnAddVector(ref nnsPriM3DPcArray1[index + 1].Pos, ref nnsPriM3DPcArray1[index + 1].Pos, ref snnsVector6);
                AppMain.nnSubtractVector(ref nnsPriM3DPcArray1[index + 2].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnSubtractVector(ref nnsPriM3DPcArray1[index + 2].Pos, ref nnsPriM3DPcArray1[index + 2].Pos, ref snnsVector6);
                AppMain.nnAddVector(ref nnsPriM3DPcArray1[index + 5].Pos, ref snnsVector4, ref snnsVector5);
                AppMain.nnSubtractVector(ref nnsPriM3DPcArray1[index + 5].Pos, ref nnsPriM3DPcArray1[index + 5].Pos, ref snnsVector6);
                nnsPriM3DPcArray1[index + 5].Col = AppMain.AMD_RGBA8888(runtimeWorkSprite.color.r, runtimeWorkSprite.color.g, runtimeWorkSprite.color.b, runtimeWorkSprite.color.a);
                nnsPriM3DPcArray1[index].Col = nnsPriM3DPcArray1[index + 1].Col = nnsPriM3DPcArray1[index + 2].Col = nnsPriM3DPcArray1[index + 5].Col;
                nnsPriM3DPcArray1[index + 3] = nnsPriM3DPcArray1[index + 1];
                nnsPriM3DPcArray1[index + 4] = nnsPriM3DPcArray1[index + 2];
                index += 6;
            }
            setParam.format3D = 2;
            setParam.type = 0;
            setParam.vtxPC3D = nnsPriM3DPcArray2;
            setParam.texlist = runtime.texlist;
            setParam.texId = -1;
            setParam.count = 6 * (int)runtime.active_num;
            setParam.ablend = num1;
            setParam.sortZ = num2;
            AppMain.amDrawPrimitive3D(runtime.ecb.drawState, setParam);
        }
        AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
        return 0;
    }
}