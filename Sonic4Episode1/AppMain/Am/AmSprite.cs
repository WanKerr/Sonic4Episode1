
public partial class AppMain
{
    private static int _amInitCircle(object p)
    {
        AMS_AME_CREATE_PARAM amsAmeCreateParam = (AMS_AME_CREATE_PARAM)p;
        AMS_AME_NODE_CIRCLE node = (AMS_AME_NODE_CIRCLE)amsAmeCreateParam.node;
        AMS_AME_RUNTIME_WORK_CIRCLE work = (AMS_AME_RUNTIME_WORK_CIRCLE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        amVectorAdd(work.position, node.translate);
        amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
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
        AMS_AME_RUNTIME amsAmeRuntime1 = (AMS_AME_RUNTIME)rr;
        AMS_AME_NODE_CIRCLE node = (AMS_AME_NODE_CIRCLE)amsAmeRuntime1.node;
        AMS_AME_RUNTIME_WORK_CIRCLE work = (AMS_AME_RUNTIME_WORK_CIRCLE)amsAmeRuntime1.work;
        work.time += _am_unit_frame;
        if (work.time <= 0.0)
            return 0;
        if (node.life != -1.0 && work.time >= (double)node.life)
            return 1;
        NNS_VECTOR4D nnsVectoR4D1 = GlobalPool<NNS_VECTOR4D>.Alloc();
        amVectorScale(nnsVectoR4D1, work.velocity, _am_unit_time);
        amVectorAdd(work.position, nnsVectoR4D1);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D1);
        float sizeRate = amsAmeRuntime1.ecb.size_rate;
        work.spread += node.spread_variation * _am_unit_time;
        if (node.radius_variation != 0.0)
            work.radius += node.radius_variation * _am_unit_time;
        else
            work.radius = node.radius * sizeRate;
        work.offset = node.offset * sizeRate;
        work.offset_chaos = node.offset_chaos * sizeRate;
        NNS_MATRIX amUpdateCircleMtx = _amUpdateCircle_mtx;
        nnMakeUnitMatrix(amUpdateCircleMtx);
        amMatrixPush(amUpdateCircleMtx);
        NNS_QUATERNION rotate = work.rotate;
        amQuatToMatrix(null, ref rotate, null);
        work.rotate = rotate;
        NNS_VECTOR4D nnsVectoR4D2 = GlobalPool<NNS_VECTOR4D>.Alloc();
        NNS_VECTOR4D nnsVectoR4D3 = GlobalPool<NNS_VECTOR4D>.Alloc();
        NNS_VECTOR4D nnsVectoR4D4 = GlobalPool<NNS_VECTOR4D>.Alloc();
        AMS_AME_LIST next = amsAmeRuntime1.child_head.next;
        for (AMS_AME_LIST childTail = amsAmeRuntime1.child_tail; next != childTail; next = next.next)
        {
            AMS_AME_RUNTIME amsAmeRuntime2 = (AMS_AME_RUNTIME)next;
            amsAmeRuntime2.amount += node.frequency * _am_unit_frame;
            while (amsAmeRuntime2.amount >= 1.0)
            {
                --amsAmeRuntime2.amount;
                ++amsAmeRuntime2.count;
                if (node.max_count != -1.0 && amsAmeRuntime2.work_num + amsAmeRuntime2.active_num < (double)node.max_count)
                {
                    AMS_AME_CREATE_PARAM amsAmeCreateParam = new AMS_AME_CREATE_PARAM();
                    amVectorSet(nnsVectoR4D4, 0.0f, 1f, 0.0f);
                    float radius = work.radius;
                    int angle = (int)(nnRandom() * 10000000.0);
                    if (((int)node.flag & 1) == 0)
                        radius *= nnRandom();
                    else if (((int)node.flag & 2) != 0)
                    {
                        int maxCount = (int)node.max_count;
                        angle = (int)(ushort.MaxValue / maxCount * (amsAmeRuntime2.count % maxCount));
                    }
                    float pSn;
                    float pCs;
                    amSinCos(angle, out pSn, out pCs);
                    amVectorSet(nnsVectoR4D2, pSn * radius, 0.0f, pCs * radius);
                    NNS_VECTOR4D nnsVectoR4D5 = GlobalPool<NNS_VECTOR4D>.Alloc();
                    NNS_QUATERNION pQuat = new NNS_QUATERNION();
                    amVectorOuterProduct(nnsVectoR4D5, nnsVectoR4D4, nnsVectoR4D2);
                    double num = amVectorUnit(nnsVectoR4D5);
                    amQuatRotAxisToQuat(ref pQuat, nnsVectoR4D5, NNM_DEGtoRAD((int)work.spread));
                    amQuatMultiVector(nnsVectoR4D4, nnsVectoR4D4, ref pQuat, null);
                    GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D5);
                    amMatrixCalcVector(nnsVectoR4D2, nnsVectoR4D2);
                    amMatrixCalcVector(nnsVectoR4D4, nnsVectoR4D4);
                    amVectorScale(nnsVectoR4D3, nnsVectoR4D4, work.offset + work.offset_chaos * nnRandom());
                    amVectorAdd(nnsVectoR4D2, nnsVectoR4D3);
                    amVectorScale(nnsVectoR4D3, nnsVectoR4D4, node.speed + node.speed_chaos * nnRandom());
                    amsAmeCreateParam.ecb = amsAmeRuntime1.ecb;
                    amsAmeCreateParam.runtime = amsAmeRuntime2;
                    amsAmeCreateParam.node = amsAmeRuntime2.node;
                    amsAmeCreateParam.parent_position = work.position;
                    amsAmeCreateParam.parent_velocity = work.velocity;
                    amsAmeCreateParam.position = nnsVectoR4D2;
                    amsAmeCreateParam.velocity = nnsVectoR4D3;
                    switch (AMD_AME_NODE_TYPE(amsAmeRuntime2.node) & 65280)
                    {
                        case 256:
                            _amCreateEmitter(amsAmeCreateParam);
                            continue;
                        case 512:
                            _amCreateParticle(amsAmeCreateParam);
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D2);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D3);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D4);
        amMatrixPop();
        return 0;
    }

    private static int _amDrawCircle(object r)
    {
        return 0;
    }

    public static int _amInitSimpleSprite(object p)
    {
        AMS_AME_CREATE_PARAM amsAmeCreateParam = (AMS_AME_CREATE_PARAM)p;
        AMS_AME_NODE_SPRITE node = (AMS_AME_NODE_SPRITE)amsAmeCreateParam.node;
        AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE work = (AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        work.set_color(node.color_start.r, node.color_start.g, node.color_start.b, (byte)(node.color_start.a * amsAmeCreateParam.ecb.transparency >> 8));
        amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        amVectorAdd(work.position, node.translate);
        amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        float z = node.size + node.size_chaos * nnRandom();
        work.set_size(z * node.scale_x_start, z * node.scale_y_start, z, 0.0f);
        if (((int)node.flag & 32768) != 0)
        {
            work.tex_time = 0.0f;
            work.tex_no = 0;
            if (((int)node.flag & 524288) != 0)
                work.tex_no = (int)(100.0 * nnRandom()) % node.tex_anim.key_num;
            AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = node.tex_anim.key_buf[work.tex_no];
            //Vector4 vector4 = new Vector4();
            work.set_st(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
        }
        else if (((int)node.flag & 8192) != 0)
            work.set_st(node.cropping_l, node.cropping_t, node.cropping_r, node.cropping_b);
        else
            work.set_st(0.0f, 0.0f, 1f, 1f);
        if (((int)node.flag & 1048576) != 0 || ((int)node.flag & 131072) != 0 && nnRandom() > 0.5)
        {
            work.flag |= 8U;
            work.set_st(work.st.z, work.st.y, work.st.x, work.st.w);
        }
        if (((int)node.flag & 2097152) != 0 || ((int)node.flag & 262144) != 0 && nnRandom() > 0.5)
        {
            work.flag |= 16U;
            work.set_st(work.st.x, work.st.w, work.st.z, work.st.y);
        }
        return 0;
    }

    public static int _amUpdateSimpleSprite(object r)
    {
        AMS_AME_RUNTIME runtime = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_SPRITE node = (AMS_AME_NODE_SPRITE)runtime.node;
        AMS_AME_LIST next = runtime.active_head.next;
        AMS_AME_LIST activeTail = runtime.active_tail;
        int transparency = runtime.ecb.transparency;
        float num1;
        float num2;
        if (node.life >= 0.0)
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
            AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE workSimpleSprite = (AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)(AMS_AME_RUNTIME_WORK)next;
            workSimpleSprite.time += _am_unit_frame;
            float rate = workSimpleSprite.time * num2;
            NNS_VECTOR4D amEffectVel = _amEffect_vel;
            amVectorScale(amEffectVel, workSimpleSprite.velocity, _am_unit_time);
            amVectorAdd(workSimpleSprite.position, amEffectVel);
            if (workSimpleSprite.time >= (double)num1)
            {
                if (runtime.spawn_runtime != null)
                    _amCreateSpawnParticle(runtime, workSimpleSprite);
                amEffectDisconnectLink((AMS_AME_LIST)workSimpleSprite);
                --runtime.active_num;
                amEffectFreeRuntimeWork(workSimpleSprite);
            }
            else
            {
                float num7 = 1f - rate;
                float num8 = (float)(num3 * (double)num7 + num5 * (double)rate);
                float num9 = (float)(num4 * (double)num7 + num6 * (double)rate);
                Vector4D_Buf size = workSimpleSprite.size;
                workSimpleSprite.set_size(size.z * num8, size.z * num9, size.z, size.w);
                AMS_RGBA8888 pCO;
                amEffectLerpColor(out pCO, ref node.color_start, ref node.color_end, rate);
                pCO.a = (byte)(pCO.a * transparency >> 8);
                workSimpleSprite.set_color(pCO.color);
                if (((int)node.flag & 32768) != 0)
                {
                    AMS_AME_TEX_ANIM texAnim = node.tex_anim;
                    if (((int)workSimpleSprite.flag & 2) == 0)
                    {
                        workSimpleSprite.tex_time += _am_unit_frame;
                        if (workSimpleSprite.tex_time >= (double)texAnim.key_buf[workSimpleSprite.tex_no].time)
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
                    AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = texAnim.key_buf[workSimpleSprite.tex_no];
                    SNNS_VECTOR4D vector4 = new SNNS_VECTOR4D(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
                    if (((int)workSimpleSprite.flag & 8) != 0)
                    {
                        float x = vector4.x;
                        vector4.x = vector4.z;
                        vector4.z = x;
                    }
                    if (((int)workSimpleSprite.flag & 16) != 0)
                    {
                        float y = vector4.y;
                        vector4.y = vector4.w;
                        vector4.w = y;
                    }
                    workSimpleSprite.set_st(vector4.x, vector4.y, vector4.z, vector4.w);
                }
                else if (((int)node.flag & 16384) != 0)
                {
                    float num10 = node.scroll_u * _am_unit_time;
                    float num11 = node.scroll_v * _am_unit_time;
                    if (((int)workSimpleSprite.flag & 8) != 0)
                        num10 = -num10;
                    if (((int)workSimpleSprite.flag & 16) != 0)
                        num11 = -num11;
                    Vector4D_Quat st = workSimpleSprite.st;
                    workSimpleSprite.set_st(st.x + num10, st.y + num11, st.z + num10, st.w + num11);
                }
            }
        }
        return 0;
    }

    public static int _amDrawSimpleSprite(object r)
    {
        AMS_AME_RUNTIME runtime = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_SPRITE node = (AMS_AME_NODE_SPRITE)runtime.node;
        AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE next1 = (AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)(AMS_AME_RUNTIME_WORK)runtime.active_head.next;
        AMS_AME_LIST next2 = runtime.active_head.next;
        AMS_AME_LIST activeTail = runtime.active_tail;
        AMS_PARAM_DRAW_PRIMITIVE setParam = amDrawAlloc_AMS_PARAM_DRAW_PRIMITIVE();
        int num1 = _amEffectSetDrawMode(runtime, setParam, node.blend);
        SNNS_VECTOR snnsVector1 = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector2 = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector3 = new SNNS_VECTOR();
        float zBias = node.z_bias;
        amVectorSet(ref snnsVector1, zBias * _am_ef_worldViewMtx.M20, zBias * _am_ef_worldViewMtx.M21, zBias * _am_ef_worldViewMtx.M22);
        amVectorSet(ref snnsVector2, _am_ef_worldViewMtx.M10, _am_ef_worldViewMtx.M11, _am_ef_worldViewMtx.M12);
        amVectorSet(ref snnsVector3, _am_ef_worldViewMtx.M00, _am_ef_worldViewMtx.M01, _am_ef_worldViewMtx.M02);
        SNNS_VECTOR snnsVector4 = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector5 = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector6 = new SNNS_VECTOR();
        if (((int)node.flag & 4096) != 0)
        {
            NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = amDrawAlloc_NNS_PRIM3D_PCT(6 * runtime.active_num);
            NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            float num2 = 0.0f;
            for (; next2 != activeTail; next2 = next2.next)
            {
                AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE workSimpleSprite = (AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)(AMS_AME_RUNTIME_WORK)next2;
                nnScaleVector(ref snnsVector5, ref snnsVector3, workSimpleSprite.size.y);
                nnScaleVector(ref snnsVector6, ref snnsVector2, workSimpleSprite.size.x);
                amVectorAdd(ref snnsVector4, workSimpleSprite.position, ref snnsVector1);
                num2 = nnDistanceVector(ref snnsVector4, _am_ef_camPos);
                nnSubtractVector(ref buffer[offset + 2].Pos, ref snnsVector4, ref snnsVector5);
                nnAddVector(ref buffer[offset + 2].Pos, ref buffer[offset + 2].Pos, ref snnsVector6);
                nnAddVector(ref buffer[offset].Pos, ref snnsVector4, ref snnsVector5);
                nnAddVector(ref buffer[offset].Pos, ref buffer[offset].Pos, ref snnsVector6);
                nnSubtractVector(ref buffer[offset + 5].Pos, ref snnsVector4, ref snnsVector5);
                nnSubtractVector(ref buffer[offset + 5].Pos, ref buffer[offset + 5].Pos, ref snnsVector6);
                nnAddVector(ref buffer[offset + 1].Pos, ref snnsVector4, ref snnsVector5);
                nnSubtractVector(ref buffer[offset + 1].Pos, ref buffer[offset + 1].Pos, ref snnsVector6);
                buffer[offset + 5].Col = AMD_RGBA8888(workSimpleSprite.color.r, workSimpleSprite.color.g, workSimpleSprite.color.b, workSimpleSprite.color.a);
                buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 5].Col;
                Vector4D_Quat st = workSimpleSprite.st;
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
            setParam.texId = node.texture_id;
            setParam.count = 6 * runtime.active_num;
            setParam.ablend = num1;
            setParam.sortZ = num2;
            amDrawPrimitive3D(runtime.ecb.drawState, setParam);
        }
        else
        {
            NNS_PRIM3D_PC[] nnsPriM3DPcArray1 = amDrawAlloc_NNS_PRIM3D_PC(6 * runtime.active_num);
            int index = 0;
            NNS_PRIM3D_PC[] nnsPriM3DPcArray2 = nnsPriM3DPcArray1;
            float num2 = 0.0f;
            for (; next2 != activeTail; next2 = next2.next)
            {
                AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE workSimpleSprite = (AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)(AMS_AME_RUNTIME_WORK)next2;
                nnScaleVector(ref snnsVector5, ref snnsVector3, workSimpleSprite.size.x);
                nnScaleVector(ref snnsVector6, ref snnsVector2, workSimpleSprite.size.y);
                amVectorAdd(ref snnsVector4, workSimpleSprite.position, ref snnsVector1);
                num2 = nnDistanceVector(ref snnsVector4, _am_ef_camPos);
                nnSubtractVector(ref nnsPriM3DPcArray1[index + 2].Pos, ref snnsVector4, ref snnsVector5);
                nnAddVector(ref nnsPriM3DPcArray1[index + 2].Pos, ref nnsPriM3DPcArray1[index + 2].Pos, ref snnsVector6);
                nnAddVector(ref nnsPriM3DPcArray1[index].Pos, ref snnsVector4, ref snnsVector5);
                nnAddVector(ref nnsPriM3DPcArray1[index].Pos, ref nnsPriM3DPcArray1[index].Pos, ref snnsVector6);
                nnSubtractVector(ref nnsPriM3DPcArray1[index + 5].Pos, ref snnsVector4, ref snnsVector5);
                nnSubtractVector(ref nnsPriM3DPcArray1[index + 5].Pos, ref nnsPriM3DPcArray1[index + 5].Pos, ref snnsVector6);
                nnAddVector(ref nnsPriM3DPcArray1[index + 1].Pos, ref snnsVector4, ref snnsVector5);
                nnSubtractVector(ref nnsPriM3DPcArray1[index + 1].Pos, ref nnsPriM3DPcArray1[index + 1].Pos, ref snnsVector6);
                nnsPriM3DPcArray1[index + 5].Col = AMD_RGBA8888(workSimpleSprite.color.r, workSimpleSprite.color.g, workSimpleSprite.color.b, workSimpleSprite.color.a);
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
            setParam.count = 6 * runtime.active_num;
            setParam.ablend = num1;
            setParam.sortZ = num2;
            amDrawPrimitive3D(runtime.ecb.drawState, setParam);
        }
        return 0;
    }

    public static int _amInitSprite(object p)
    {
        AMS_AME_CREATE_PARAM amsAmeCreateParam = (AMS_AME_CREATE_PARAM)p;
        AMS_AME_NODE_SPRITE node = (AMS_AME_NODE_SPRITE)amsAmeCreateParam.node;
        AMS_AME_RUNTIME_WORK_SPRITE work = (AMS_AME_RUNTIME_WORK_SPRITE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        AMS_RGBA8888 colorStart = node.color_start;
        colorStart.color = node.color_start.color;
        colorStart.a = (byte)(colorStart.a * amsAmeCreateParam.ecb.transparency >> 8);
        work.set_color(colorStart.color);
        amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        amVectorAdd(work.position, node.translate);
        amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        float z = node.size + node.size_chaos * nnRandom();
        work.set_size(z * node.scale_x_start, z * node.scale_x_start, z, 0.0f);
        work.twist = node.twist_angle + node.twist_angle_chaos * nnRandom();
        if (((int)node.flag & 4) != 0 && nnRandom() > 0.5)
            work.flag |= 4U;
        work.twist_speed = ((int)work.flag & 4) == 0 ? node.twist_angle_speed : -node.twist_angle_speed;
        if (((int)node.flag & 32768) != 0)
        {
            work.tex_time = 0.0f;
            work.tex_no = 0;
            if (((int)node.flag & 524288) != 0)
                work.tex_no = (int)(100.0 * nnRandom()) % node.tex_anim.key_num;
            AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = node.tex_anim.key_buf[work.tex_no];
            work.set_st(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
        }
        else if (((int)node.flag & 8192) != 0)
            work.set_st(node.cropping_l, node.cropping_t, node.cropping_r, node.cropping_b);
        else
            work.set_st(0.0f, 0.0f, 1f, 1f);
        if (((int)node.flag & 1048576) != 0 || ((int)node.flag & 131072) != 0 && nnRandom() > 0.5)
        {
            work.flag |= 8U;
            work.set_st(work.st.z, work.st.y, work.st.x, work.st.w);
        }
        if (((int)node.flag & 2097152) != 0 || ((int)node.flag & 262144) != 0 && nnRandom() > 0.5)
        {
            work.flag |= 16U;
            work.set_st(work.st.x, work.st.w, work.st.z, work.st.y);
        }
        return 0;
    }

    public static int _amUpdateSprite(object r)
    {
        AMS_AME_RUNTIME runtime = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_SPRITE node = (AMS_AME_NODE_SPRITE)runtime.node;
        AMS_AME_LIST activeTail = runtime.active_tail;
        AMS_AME_LIST next = runtime.active_head.next;
        int transparency = runtime.ecb.transparency;
        float num1;
        float num2;
        if (node.life >= 0.0)
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
        NNS_VECTOR4D nnsVectoR4D = GlobalPool<NNS_VECTOR4D>.Alloc();
        for (; next != activeTail; next = next.next)
        {
            AMS_AME_RUNTIME_WORK_SPRITE runtimeWorkSprite = (AMS_AME_RUNTIME_WORK_SPRITE)(AMS_AME_RUNTIME_WORK)next;
            runtimeWorkSprite.time += _am_unit_frame;
            float rate = runtimeWorkSprite.time * num2;
            amVectorScale(nnsVectoR4D, runtimeWorkSprite.velocity, _am_unit_time);
            amVectorAdd(runtimeWorkSprite.position, nnsVectoR4D);
            if (runtimeWorkSprite.time >= (double)num1)
            {
                if (runtime.spawn_runtime != null)
                    _amCreateSpawnParticle(runtime, runtimeWorkSprite);
                amEffectDisconnectLink((AMS_AME_LIST)runtimeWorkSprite);
                --runtime.active_num;
                amEffectFreeRuntimeWork(runtimeWorkSprite);
            }
            else
            {
                float num7 = 1f - rate;
                float num8 = (float)(num3 * (double)num7 + num5 * (double)rate);
                float num9 = (float)(num4 * (double)num7 + num6 * (double)rate);
                runtimeWorkSprite.set_size(runtimeWorkSprite.size.z * num8, runtimeWorkSprite.size.z * num9, runtimeWorkSprite.size.z, runtimeWorkSprite.size.w);
                runtimeWorkSprite.twist += runtimeWorkSprite.twist_speed * _am_unit_time;
                AMS_RGBA8888 pCO;
                amEffectLerpColor(out pCO, ref node.color_start, ref node.color_end, rate);
                pCO.a = (byte)(pCO.a * transparency >> 8);
                runtimeWorkSprite.set_color(pCO.color);
                if (((int)node.flag & 32768) != 0)
                {
                    AMS_AME_TEX_ANIM texAnim = node.tex_anim;
                    if (((int)runtimeWorkSprite.flag & 2) == 0)
                    {
                        runtimeWorkSprite.tex_time += _am_unit_frame;
                        if (runtimeWorkSprite.tex_time >= (double)texAnim.key_buf[runtimeWorkSprite.tex_no].time)
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
                    AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = texAnim.key_buf[runtimeWorkSprite.tex_no];
                    runtimeWorkSprite.set_st(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
                    if (((int)runtimeWorkSprite.flag & 8) != 0)
                        runtimeWorkSprite.set_st(runtimeWorkSprite.st.z, runtimeWorkSprite.st.y, runtimeWorkSprite.st.x, runtimeWorkSprite.st.w);
                    if (((int)runtimeWorkSprite.flag & 16) != 0)
                        runtimeWorkSprite.set_st(runtimeWorkSprite.st.x, runtimeWorkSprite.st.w, runtimeWorkSprite.st.z, runtimeWorkSprite.st.y);
                }
                else if (((int)node.flag & 16384) != 0)
                {
                    float num10 = node.scroll_u * _am_unit_time;
                    float num11 = node.scroll_v * _am_unit_time;
                    if (((int)runtimeWorkSprite.flag & 8) != 0)
                        num10 = -num10;
                    if (((int)runtimeWorkSprite.flag & 16) != 0)
                        num11 = -num11;
                    runtimeWorkSprite.set_st(runtimeWorkSprite.st.x + num10, runtimeWorkSprite.st.y + num11, runtimeWorkSprite.st.z + num10, runtimeWorkSprite.st.w + num11);
                }
            }
        }
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D);
        return 0;
    }

    public static int _amDrawSprite(object r)
    {
        AMS_AME_RUNTIME runtime = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_SPRITE node = (AMS_AME_NODE_SPRITE)runtime.node;
        AMS_AME_LIST next = runtime.active_head.next;
        AMS_AME_LIST activeTail = runtime.active_tail;
        AMS_PARAM_DRAW_PRIMITIVE setParam = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        int num1 = _amEffectSetDrawMode(runtime, setParam, node.blend);
        SNNS_VECTOR snnsVector1 = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector2 = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector3 = new SNNS_VECTOR();
        float zBias = node.z_bias;
        amVectorSet(ref snnsVector1, zBias * _am_ef_worldViewMtx.M20, zBias * _am_ef_worldViewMtx.M21, zBias * _am_ef_worldViewMtx.M22);
        amVectorSet(ref snnsVector2, _am_ef_worldViewMtx.M10, _am_ef_worldViewMtx.M11, _am_ef_worldViewMtx.M12);
        amVectorSet(ref snnsVector3, _am_ef_worldViewMtx.M00, _am_ef_worldViewMtx.M01, _am_ef_worldViewMtx.M02);
        SNNS_VECTOR snnsVector4 = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector5 = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector6 = new SNNS_VECTOR();
        if (((int)node.flag & 4096) != 0)
        {
            NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = amDrawAlloc_NNS_PRIM3D_PCT(6 * runtime.active_num);
            NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            float num2 = 0.0f;
            for (; next != activeTail; next = next.next)
            {
                AMS_AME_RUNTIME_WORK_SPRITE runtimeWorkSprite = (AMS_AME_RUNTIME_WORK_SPRITE)(AMS_AME_RUNTIME_WORK)next;
                float pSn;
                float pCs;
                amSinCos(runtimeWorkSprite.twist, out pSn, out pCs);
                amVectorGetAverage(ref snnsVector5, ref snnsVector3, ref snnsVector2, pCs, -pSn);
                amVectorGetAverage(ref snnsVector6, ref snnsVector3, ref snnsVector2, pSn, pCs);
                nnScaleVector(ref snnsVector5, ref snnsVector5, runtimeWorkSprite.size.x);
                nnScaleVector(ref snnsVector6, ref snnsVector6, runtimeWorkSprite.size.y);
                amVectorAdd(ref snnsVector4, runtimeWorkSprite.position, ref snnsVector1);
                num2 = nnDistanceVector(ref snnsVector4, _am_ef_camPos);
                nnSubtractVector(ref buffer[offset].Pos, ref snnsVector4, ref snnsVector5);
                nnAddVector(ref buffer[offset].Pos, ref buffer[offset].Pos, ref snnsVector6);
                nnAddVector(ref buffer[offset + 1].Pos, ref snnsVector4, ref snnsVector5);
                nnAddVector(ref buffer[offset + 1].Pos, ref buffer[offset + 1].Pos, ref snnsVector6);
                nnSubtractVector(ref buffer[offset + 2].Pos, ref snnsVector4, ref snnsVector5);
                nnSubtractVector(ref buffer[offset + 2].Pos, ref buffer[offset + 2].Pos, ref snnsVector6);
                nnAddVector(ref buffer[offset + 5].Pos, ref snnsVector4, ref snnsVector5);
                nnSubtractVector(ref buffer[offset + 5].Pos, ref buffer[offset + 5].Pos, ref snnsVector6);
                buffer[offset + 5].Col = AMD_RGBA8888(runtimeWorkSprite.color.r, runtimeWorkSprite.color.g, runtimeWorkSprite.color.b, runtimeWorkSprite.color.a);
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
            setParam.texId = node.texture_id;
            setParam.count = 6 * runtime.active_num;
            setParam.ablend = num1;
            setParam.sortZ = num2;
            amDrawPrimitive3D(runtime.ecb.drawState, setParam);
        }
        else
        {
            NNS_PRIM3D_PC[] nnsPriM3DPcArray1 = amDrawAlloc_NNS_PRIM3D_PC(6 * runtime.active_num);
            int index = 0;
            NNS_PRIM3D_PC[] nnsPriM3DPcArray2 = nnsPriM3DPcArray1;
            float num2 = 0.0f;
            for (; next != activeTail; next = next.next)
            {
                AMS_AME_RUNTIME_WORK_SPRITE runtimeWorkSprite = (AMS_AME_RUNTIME_WORK_SPRITE)(AMS_AME_RUNTIME_WORK)next;
                float pSn;
                float pCs;
                amSinCos(runtimeWorkSprite.twist, out pSn, out pCs);
                amVectorGetAverage(ref snnsVector5, ref snnsVector3, ref snnsVector2, pCs, -pSn);
                amVectorGetAverage(ref snnsVector6, ref snnsVector3, ref snnsVector2, pSn, pCs);
                nnScaleVector(ref snnsVector5, ref snnsVector5, runtimeWorkSprite.size.x);
                nnScaleVector(ref snnsVector6, ref snnsVector6, runtimeWorkSprite.size.y);
                amVectorAdd(ref snnsVector4, runtimeWorkSprite.position, ref snnsVector1);
                num2 = nnDistanceVector(ref snnsVector4, _am_ef_camPos);
                nnSubtractVector(ref nnsPriM3DPcArray1[index].Pos, ref snnsVector4, ref snnsVector5);
                nnAddVector(ref nnsPriM3DPcArray1[index].Pos, ref nnsPriM3DPcArray1[index].Pos, ref snnsVector6);
                nnAddVector(ref nnsPriM3DPcArray1[index + 1].Pos, ref snnsVector4, ref snnsVector5);
                nnAddVector(ref nnsPriM3DPcArray1[index + 1].Pos, ref nnsPriM3DPcArray1[index + 1].Pos, ref snnsVector6);
                nnSubtractVector(ref nnsPriM3DPcArray1[index + 2].Pos, ref snnsVector4, ref snnsVector5);
                nnSubtractVector(ref nnsPriM3DPcArray1[index + 2].Pos, ref nnsPriM3DPcArray1[index + 2].Pos, ref snnsVector6);
                nnAddVector(ref nnsPriM3DPcArray1[index + 5].Pos, ref snnsVector4, ref snnsVector5);
                nnSubtractVector(ref nnsPriM3DPcArray1[index + 5].Pos, ref nnsPriM3DPcArray1[index + 5].Pos, ref snnsVector6);
                nnsPriM3DPcArray1[index + 5].Col = AMD_RGBA8888(runtimeWorkSprite.color.r, runtimeWorkSprite.color.g, runtimeWorkSprite.color.b, runtimeWorkSprite.color.a);
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
            setParam.count = 6 * runtime.active_num;
            setParam.ablend = num1;
            setParam.sortZ = num2;
            amDrawPrimitive3D(runtime.ecb.drawState, setParam);
        }
        GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
        return 0;
    }
}