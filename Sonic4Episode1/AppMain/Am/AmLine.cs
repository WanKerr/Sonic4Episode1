public partial class AppMain
{
    public static int _amInitLine(object p)
    {
        AMS_AME_CREATE_PARAM amsAmeCreateParam = (AMS_AME_CREATE_PARAM)p;
        AMS_AME_NODE_LINE node = (AMS_AME_NODE_LINE)amsAmeCreateParam.node;
        AMS_AME_RUNTIME_WORK_LINE work = (AMS_AME_RUNTIME_WORK_LINE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        work.length = node.length_start;
        work.inside_width = node.inside_width_start;
        work.outside_width = node.outside_width_start;
        AMS_RGBA8888 insideColorStart = node.inside_color_start;
        insideColorStart.a = (byte)(insideColorStart.a * amsAmeCreateParam.ecb.transparency >> 8);
        work.set_inside_color(insideColorStart.color);
        AMS_RGBA8888 outsideColorStart = node.outside_color_start;
        outsideColorStart.a = (byte)(outsideColorStart.a * amsAmeCreateParam.ecb.transparency >> 8);
        work.set_outside_color(outsideColorStart.color);
        amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        amVectorAdd(work.position, node.translate);
        amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
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

    public static int _amUpdateLine(object r)
    {
        AMS_AME_RUNTIME runtime = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_LINE node = (AMS_AME_NODE_LINE)runtime.node;
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
            num1 = 1E+38f;
            num2 = 0.0f;
        }
        float sizeRate = runtime.ecb.size_rate;
        float num3 = node.length_start * sizeRate;
        float num4 = node.length_end * sizeRate;
        float num5 = node.inside_width_start * sizeRate;
        float num6 = node.outside_width_start * sizeRate;
        float num7 = node.inside_width_end * sizeRate;
        float num8 = node.outside_width_end * sizeRate;
        NNS_VECTOR4D nnsVectoR4D = GlobalPool<NNS_VECTOR4D>.Alloc();
        for (; next != activeTail; next = next.next)
        {
            AMS_AME_RUNTIME_WORK_LINE ameRuntimeWorkLine = (AMS_AME_RUNTIME_WORK_LINE)(AMS_AME_RUNTIME_WORK)next;
            ameRuntimeWorkLine.time += _am_unit_frame;
            float rate = ameRuntimeWorkLine.time * num2;
            amVectorScale(nnsVectoR4D, ameRuntimeWorkLine.velocity, _am_unit_time);
            amVectorAdd(ameRuntimeWorkLine.position, nnsVectoR4D);
            if (ameRuntimeWorkLine.time >= (double)num1)
            {
                if (runtime.spawn_runtime != null)
                    _amCreateSpawnParticle(runtime, ameRuntimeWorkLine);
                amEffectDisconnectLink((AMS_AME_LIST)ameRuntimeWorkLine);
                --runtime.active_num;
                amEffectFreeRuntimeWork(ameRuntimeWorkLine);
            }
            else
            {
                float num9 = 1f - rate;
                ameRuntimeWorkLine.length = (float)(num3 * (double)num9 + num4 * (double)rate);
                ameRuntimeWorkLine.inside_width = (float)(num5 * (double)num9 + num7 * (double)rate);
                ameRuntimeWorkLine.outside_width = (float)(num6 * (double)num9 + num8 * (double)rate);
                AMS_RGBA8888 pCO;
                amEffectLerpColor(out pCO, ref node.inside_color_start, ref node.inside_color_end, rate);
                pCO.a = (byte)(pCO.a * transparency >> 8);
                ameRuntimeWorkLine.set_inside_color(pCO.color);
                amEffectLerpColor(out pCO, ref node.outside_color_start, ref node.outside_color_end, rate);
                pCO.a = (byte)(pCO.a * transparency >> 8);
                ameRuntimeWorkLine.set_outside_color(pCO.color);
                if (((int)node.flag & 32768) != 0)
                {
                    AMS_AME_TEX_ANIM texAnim = node.tex_anim;
                    if (((int)ameRuntimeWorkLine.flag & 2) == 0)
                    {
                        ameRuntimeWorkLine.tex_time += _am_unit_frame;
                        if (ameRuntimeWorkLine.tex_time >= (double)texAnim.key_buf[ameRuntimeWorkLine.tex_no].time)
                        {
                            ameRuntimeWorkLine.tex_time = 0.0f;
                            ++ameRuntimeWorkLine.tex_no;
                            if (ameRuntimeWorkLine.tex_no == texAnim.key_num)
                            {
                                if (((int)node.flag & 65536) != 0)
                                {
                                    ameRuntimeWorkLine.tex_no = 0;
                                }
                                else
                                {
                                    ameRuntimeWorkLine.tex_no = texAnim.key_num - 1;
                                    ameRuntimeWorkLine.flag |= 2U;
                                }
                            }
                        }
                    }
                    AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = texAnim.key_buf[ameRuntimeWorkLine.tex_no];
                    ameRuntimeWorkLine.set_st(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
                    if (((int)ameRuntimeWorkLine.flag & 8) != 0)
                        ameRuntimeWorkLine.set_st(ameRuntimeWorkLine.st.z, ameRuntimeWorkLine.st.y, ameRuntimeWorkLine.st.x, ameRuntimeWorkLine.st.w);
                    if (((int)ameRuntimeWorkLine.flag & 16) != 0)
                        ameRuntimeWorkLine.set_st(ameRuntimeWorkLine.st.x, ameRuntimeWorkLine.st.w, ameRuntimeWorkLine.st.z, ameRuntimeWorkLine.st.y);
                }
                else if (((int)node.flag & 16384) != 0)
                {
                    float num10 = node.scroll_u * _am_unit_time;
                    float num11 = node.scroll_v * _am_unit_time;
                    if (((int)ameRuntimeWorkLine.flag & 8) != 0)
                        num10 = -num10;
                    if (((int)ameRuntimeWorkLine.flag & 16) != 0)
                        num11 = -num11;
                    ameRuntimeWorkLine.set_st(ameRuntimeWorkLine.st.x + num10, ameRuntimeWorkLine.st.w + num10, ameRuntimeWorkLine.st.z + num11, ameRuntimeWorkLine.st.y + num11);
                }
            }
        }
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D);
        return 0;
    }

    public static int _amDrawLine(object r)
    {
        AMS_AME_RUNTIME runtime = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_LINE node = (AMS_AME_NODE_LINE)runtime.node;
        AMS_AME_LIST next = runtime.active_head.next;
        AMS_AME_LIST activeTail = runtime.active_tail;
        AMS_PARAM_DRAW_PRIMITIVE setParam = amDrawAlloc_AMS_PARAM_DRAW_PRIMITIVE();
        int num1 = _amEffectSetDrawMode(runtime, setParam, node.blend);
        NNS_VECTOR4D amDrawLineOffset = _amDrawLine_offset;
        NNS_VECTOR4D amDrawLineEye = _amDrawLine_eye;
        float zBias = node.z_bias;
        amVectorSet(amDrawLineOffset, zBias * _am_ef_worldViewMtx.M20, zBias * _am_ef_worldViewMtx.M21, zBias * _am_ef_worldViewMtx.M22);
        amVectorSet(amDrawLineEye, _am_ef_worldViewMtx.M20, _am_ef_worldViewMtx.M21, _am_ef_worldViewMtx.M22);
        NNS_VECTOR4D amDrawLinePos0 = _amDrawLine_pos0;
        NNS_VECTOR4D amDrawLinePos1 = _amDrawLine_pos1;
        NNS_VECTOR amDrawLineVel = _amDrawLine_vel;
        NNS_VECTOR amDrawLineCross = _amDrawLine_cross;
        if (((int)node.flag & 4096) != 0)
        {
            NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = amDrawAlloc_NNS_PRIM3D_PCT(6 * runtime.active_num);
            NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            float num2 = 0.0f;
            for (; next != activeTail; next = next.next)
            {
                AMS_AME_RUNTIME_WORK_LINE ameRuntimeWorkLine = (AMS_AME_RUNTIME_WORK_LINE)(AMS_AME_RUNTIME_WORK)next;
                double num3 = amVectorUnit(amDrawLineVel, ameRuntimeWorkLine.velocity);
                nnScaleVector(amDrawLinePos0, amDrawLineVel, ameRuntimeWorkLine.length);
                amVectorAdd(amDrawLinePos1, ameRuntimeWorkLine.position, amDrawLineOffset);
                nnAddVector(amDrawLinePos0, amDrawLinePos0, amDrawLinePos1);
                num2 = nnDistanceVector(amDrawLinePos0, _am_ef_camPos);
                nnCrossProductVector(amDrawLineCross, amDrawLineVel, amDrawLineEye);
                nnNormalizeVector(amDrawLineCross, amDrawLineCross);
                nnScaleVector(amDrawLineVel, amDrawLineCross, ameRuntimeWorkLine.outside_width);
                nnSubtractVector(ref buffer[offset].Pos, amDrawLinePos0, amDrawLineVel);
                nnAddVector(ref buffer[offset + 1].Pos, amDrawLinePos0, amDrawLineVel);
                nnScaleVector(amDrawLineVel, amDrawLineCross, ameRuntimeWorkLine.inside_width);
                nnSubtractVector(ref buffer[offset + 2].Pos, amDrawLinePos1, amDrawLineVel);
                nnAddVector(ref buffer[offset + 5].Pos, amDrawLinePos1, amDrawLineVel);
                buffer[offset + 1].Col = AMD_RGBA8888(ameRuntimeWorkLine.outside_color.r, ameRuntimeWorkLine.outside_color.g, ameRuntimeWorkLine.outside_color.b, ameRuntimeWorkLine.outside_color.a);
                buffer[offset].Col = buffer[offset + 1].Col;
                buffer[offset + 5].Col = AMD_RGBA8888(ameRuntimeWorkLine.inside_color.r, ameRuntimeWorkLine.inside_color.g, ameRuntimeWorkLine.inside_color.b, ameRuntimeWorkLine.inside_color.a);
                buffer[offset + 2].Col = buffer[offset + 5].Col;
                Vector4D_Quat st = ameRuntimeWorkLine.st;
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
            NNS_PRIM3D_PC[] nnsPriM3DPcArray1 = new NNS_PRIM3D_PC[6 * runtime.active_num];
            int index = 0;
            NNS_PRIM3D_PC[] nnsPriM3DPcArray2 = nnsPriM3DPcArray1;
            float num2 = 0.0f;
            for (; next != activeTail; next = next.next)
            {
                AMS_AME_RUNTIME_WORK_LINE ameRuntimeWorkLine = (AMS_AME_RUNTIME_WORK_LINE)(AMS_AME_RUNTIME_WORK)next;
                double num3 = amVectorUnit(amDrawLineVel, ameRuntimeWorkLine.velocity);
                nnScaleVector(amDrawLinePos0, amDrawLineVel, ameRuntimeWorkLine.length);
                amVectorAdd(amDrawLinePos1, ameRuntimeWorkLine.position, amDrawLineOffset);
                nnAddVector(amDrawLinePos0, amDrawLinePos0, amDrawLinePos1);
                num2 = nnDistanceVector(amDrawLinePos0, _am_ef_camPos);
                nnCrossProductVector(amDrawLineCross, amDrawLineVel, amDrawLineEye);
                nnNormalizeVector(amDrawLineCross, amDrawLineCross);
                nnScaleVector(amDrawLineVel, amDrawLineCross, ameRuntimeWorkLine.outside_width);
                nnSubtractVector(ref nnsPriM3DPcArray1[index].Pos, amDrawLinePos0, amDrawLineVel);
                nnAddVector(ref nnsPriM3DPcArray1[index + 1].Pos, amDrawLinePos0, amDrawLineVel);
                nnScaleVector(amDrawLineVel, amDrawLineCross, ameRuntimeWorkLine.inside_width);
                nnSubtractVector(ref nnsPriM3DPcArray1[index + 2].Pos, amDrawLinePos1, amDrawLineVel);
                nnAddVector(ref nnsPriM3DPcArray1[index + 5].Pos, amDrawLinePos1, amDrawLineVel);
                nnsPriM3DPcArray1[index + 1].Col = AMD_RGBA8888(ameRuntimeWorkLine.outside_color.r, ameRuntimeWorkLine.outside_color.g, ameRuntimeWorkLine.outside_color.b, ameRuntimeWorkLine.outside_color.a);
                nnsPriM3DPcArray1[index].Col = nnsPriM3DPcArray1[index + 1].Col;
                nnsPriM3DPcArray1[index + 5].Col = AMD_RGBA8888(ameRuntimeWorkLine.inside_color.r, ameRuntimeWorkLine.inside_color.g, ameRuntimeWorkLine.inside_color.b, ameRuntimeWorkLine.inside_color.a);
                nnsPriM3DPcArray1[index + 2].Col = nnsPriM3DPcArray1[index + 5].Col;
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

}