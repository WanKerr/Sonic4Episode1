public partial class AppMain
{
    public static int _amInitPlane(object p)
    {
        AMS_AME_CREATE_PARAM amsAmeCreateParam = (AMS_AME_CREATE_PARAM)p;
        AMS_AME_NODE_PLANE node = (AMS_AME_NODE_PLANE)amsAmeCreateParam.node;
        AMS_AME_RUNTIME_WORK_PLANE work = (AMS_AME_RUNTIME_WORK_PLANE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        AMS_RGBA8888 colorStart = node.color_start;
        work.set_color(colorStart.r, colorStart.g, colorStart.b, (byte)(colorStart.a * amsAmeCreateParam.ecb.transparency >> 8));
        amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        amVectorAdd(work.position, node.translate);
        amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        if (((int)node.flag & 4) != 0)
        {
            NNS_VECTOR4D nnsVectoR4D = GlobalPool<NNS_VECTOR4D>.Alloc();
            float radian = (float)(nnRandom() * 2.0 * 3.14159274101257);
            amVectorRandom(nnsVectoR4D);
            NNS_QUATERNION rotate = work.rotate;
            amQuatRotAxisToQuat(ref rotate, nnsVectoR4D, radian);
            work.rotate = rotate;
            GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D);
        }
        else
            work.rotate = node.rotate;
        if (((int)node.flag & 8) != 0)
        {
            NNS_VECTOR4D pDst = GlobalPool<NNS_VECTOR4D>.Alloc();
            amVectorRandom(pDst);
            work.set_rotate_axis(pDst.x, pDst.y, pDst.z, node.rotate_axis.w);
            GlobalPool<NNS_VECTOR4D>.Release(pDst);
        }
        else
            work.rotate_axis.Assign(node.rotate_axis);
        float z = node.size + node.size_chaos * nnRandom();
        work.set_size(z * node.scale_x_start, z * node.scale_y_start, z, 0.0f);
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

    public static int _amUpdatePlane(object r)
    {
        AMS_AME_RUNTIME runtime = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_PLANE node = (AMS_AME_NODE_PLANE)runtime.node;
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
        NNS_VECTOR4D amEffectVel = _amEffect_vel;
        NNS_QUATERNION nnsQuaternion = new NNS_QUATERNION();
        NNS_VECTOR4D pVec = GlobalPool<NNS_VECTOR4D>.Alloc();
        for (; next != activeTail; next = next.next)
        {
            AMS_AME_RUNTIME_WORK_PLANE runtimeWorkPlane = (AMS_AME_RUNTIME_WORK_PLANE)(AMS_AME_RUNTIME_WORK)next;
            runtimeWorkPlane.time += _am_unit_frame;
            float rate = runtimeWorkPlane.time * num2;
            amVectorScale(amEffectVel, runtimeWorkPlane.velocity, _am_unit_time);
            amVectorAdd(runtimeWorkPlane.position, amEffectVel);
            if (runtimeWorkPlane.time >= (double)num1)
            {
                if (runtime.spawn_runtime != null)
                    _amCreateSpawnParticle(runtime, (AMS_AME_RUNTIME_WORK)(AMS_AME_LIST)runtimeWorkPlane);
                amEffectDisconnectLink((AMS_AME_LIST)runtimeWorkPlane);
                --runtime.active_num;
                amEffectFreeRuntimeWork((AMS_AME_RUNTIME_WORK)(AMS_AME_LIST)runtimeWorkPlane);
            }
            else
            {
                pVec.x = runtimeWorkPlane.rotate_axis.x;
                pVec.y = runtimeWorkPlane.rotate_axis.y;
                pVec.z = runtimeWorkPlane.rotate_axis.z;
                pVec.w = runtimeWorkPlane.rotate_axis.w;
                amQuatRotAxisToQuat(ref nnsQuaternion, pVec, pVec.w * _am_unit_time);
                NNS_QUATERNION rotate = runtimeWorkPlane.rotate;
                amQuatMulti(ref rotate, ref nnsQuaternion, ref rotate);
                runtimeWorkPlane.rotate = rotate;
                float num7 = 1f - rate;
                float num8 = (float)(num3 * (double)num7 + num5 * (double)rate);
                float num9 = (float)(num4 * (double)num7 + num6 * (double)rate);
                runtimeWorkPlane.set_size(runtimeWorkPlane.size.z * num8, runtimeWorkPlane.size.z * num9, runtimeWorkPlane.size.z, runtimeWorkPlane.size.w);
                AMS_RGBA8888 pCO;
                amEffectLerpColor(out pCO, ref node.color_start, ref node.color_end, rate);
                pCO.a = (byte)(pCO.a * transparency >> 8);
                runtimeWorkPlane.set_color(pCO.color);
                if (((int)node.flag & 32768) != 0)
                {
                    AMS_AME_TEX_ANIM texAnim = node.tex_anim;
                    if (((int)runtimeWorkPlane.flag & 2) == 0)
                    {
                        runtimeWorkPlane.tex_time += _am_unit_frame;
                        if (runtimeWorkPlane.tex_time >= (double)texAnim.key_buf[runtimeWorkPlane.tex_no].time)
                        {
                            runtimeWorkPlane.tex_time = 0.0f;
                            ++runtimeWorkPlane.tex_no;
                            if (runtimeWorkPlane.tex_no == texAnim.key_num)
                            {
                                if (((int)node.flag & 65536) != 0)
                                {
                                    runtimeWorkPlane.tex_no = 0;
                                }
                                else
                                {
                                    runtimeWorkPlane.tex_no = texAnim.key_num - 1;
                                    runtimeWorkPlane.flag |= 2U;
                                }
                            }
                        }
                    }
                    AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = texAnim.key_buf[runtimeWorkPlane.tex_no];
                    runtimeWorkPlane.set_st(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
                    if (((int)runtimeWorkPlane.flag & 8) != 0)
                        runtimeWorkPlane.set_st(runtimeWorkPlane.st.z, runtimeWorkPlane.st.y, runtimeWorkPlane.st.x, runtimeWorkPlane.st.w);
                    if (((int)runtimeWorkPlane.flag & 16) != 0)
                        runtimeWorkPlane.set_st(runtimeWorkPlane.st.x, runtimeWorkPlane.st.w, runtimeWorkPlane.st.z, runtimeWorkPlane.st.y);
                }
                else if (((int)node.flag & 16384) != 0)
                {
                    float num10 = node.scroll_u * _am_unit_time;
                    float num11 = node.scroll_v * _am_unit_time;
                    if (((int)runtimeWorkPlane.flag & 8) != 0)
                        num10 = -num10;
                    if (((int)runtimeWorkPlane.flag & 16) != 0)
                        num11 = -num11;
                    runtimeWorkPlane.set_st(runtimeWorkPlane.st.x + num10, runtimeWorkPlane.st.y + num11, runtimeWorkPlane.st.z + num10, runtimeWorkPlane.st.w + num11);
                }
            }
        }
        GlobalPool<NNS_VECTOR4D>.Release(pVec);
        return 0;
    }

    public static int _amDrawPlane(object r)
    {
        AMS_AME_RUNTIME runtime = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_PLANE node = (AMS_AME_NODE_PLANE)runtime.node;
        AMS_AME_LIST next = runtime.active_head.next;
        AMS_AME_LIST activeTail = runtime.active_tail;
        AMS_PARAM_DRAW_PRIMITIVE setParam = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        int num1 = _amEffectSetDrawMode(runtime, setParam, node.blend);
        SNNS_VECTOR4D pDst = new SNNS_VECTOR4D();
        float zBias = node.z_bias;
        amVectorSet(out pDst, zBias * _am_ef_worldViewMtx.M20, zBias * _am_ef_worldViewMtx.M21, zBias * _am_ef_worldViewMtx.M22);
        SNNS_VECTOR snnsVector = new SNNS_VECTOR();
        if (((int)node.flag & 4096) != 0)
        {
            NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = amDrawAlloc_NNS_PRIM3D_PCT(6 * runtime.active_num);
            NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            float num2 = 0.0f;
            for (; next != activeTail; next = next.next)
            {
                AMS_AME_RUNTIME_WORK_PLANE runtimeWorkPlane = (AMS_AME_RUNTIME_WORK_PLANE)(AMS_AME_RUNTIME_WORK)next;
                amMatrixPush();
                NNS_MATRIX current = amMatrixGetCurrent();
                float x = runtimeWorkPlane.size.x;
                float y = runtimeWorkPlane.size.y;
                amVectorAdd(ref snnsVector, runtimeWorkPlane.position, ref pDst);
                NNS_QUATERNION rotate = runtimeWorkPlane.rotate;
                amQuatMultiMatrix(ref rotate, ref snnsVector);
                runtimeWorkPlane.rotate = rotate;
                num2 = nnDistanceVector(ref snnsVector, _am_ef_camPos);
                buffer[offset].Pos.Assign(-x, y, 0.0f);
                buffer[offset + 1].Pos.Assign(x, y, 0.0f);
                buffer[offset + 2].Pos.Assign(-x, -y, 0.0f);
                buffer[offset + 5].Pos.Assign(x, -y, 0.0f);
                nnTransformVector(ref buffer[offset].Pos, current, ref buffer[offset].Pos);
                nnTransformVector(ref buffer[offset + 1].Pos, current, ref buffer[offset + 1].Pos);
                nnTransformVector(ref buffer[offset + 2].Pos, current, ref buffer[offset + 2].Pos);
                nnTransformVector(ref buffer[offset + 5].Pos, current, ref buffer[offset + 5].Pos);
                buffer[offset + 5].Col = AMD_RGBA8888(runtimeWorkPlane.color.r, runtimeWorkPlane.color.g, runtimeWorkPlane.color.b, runtimeWorkPlane.color.a);
                buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 5].Col;
                buffer[offset].Tex.u = runtimeWorkPlane.st.x;
                buffer[offset].Tex.v = runtimeWorkPlane.st.y;
                buffer[offset + 1].Tex.u = runtimeWorkPlane.st.z;
                buffer[offset + 1].Tex.v = runtimeWorkPlane.st.y;
                buffer[offset + 2].Tex.u = runtimeWorkPlane.st.x;
                buffer[offset + 2].Tex.v = runtimeWorkPlane.st.w;
                buffer[offset + 5].Tex.u = runtimeWorkPlane.st.z;
                buffer[offset + 5].Tex.v = runtimeWorkPlane.st.w;
                buffer[offset + 3] = buffer[offset + 1];
                buffer[offset + 4] = buffer[offset + 2];
                offset += 6;
                amMatrixPop();
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
                AMS_AME_RUNTIME_WORK_PLANE runtimeWorkPlane = (AMS_AME_RUNTIME_WORK_PLANE)(AMS_AME_RUNTIME_WORK)next;
                amMatrixPush();
                NNS_MATRIX current = amMatrixGetCurrent();
                float x = runtimeWorkPlane.size.x;
                float y = runtimeWorkPlane.size.y;
                amVectorAdd(ref snnsVector, runtimeWorkPlane.position, ref pDst);
                NNS_QUATERNION rotate = runtimeWorkPlane.rotate;
                amQuatMultiMatrix(ref rotate, ref snnsVector);
                runtimeWorkPlane.rotate = rotate;
                num2 = nnDistanceVector(ref snnsVector, _am_ef_camPos);
                nnsPriM3DPcArray1[index].Pos.Assign(-x, y, 0.0f);
                nnsPriM3DPcArray1[index + 1].Pos.Assign(x, y, 0.0f);
                nnsPriM3DPcArray1[index + 2].Pos.Assign(-x, -y, 0.0f);
                nnsPriM3DPcArray1[index + 5].Pos.Assign(x, -y, 0.0f);
                nnTransformVector(ref nnsPriM3DPcArray1[index].Pos, current, ref nnsPriM3DPcArray1[index].Pos);
                nnTransformVector(ref nnsPriM3DPcArray1[index + 1].Pos, current, ref nnsPriM3DPcArray1[index + 1].Pos);
                nnTransformVector(ref nnsPriM3DPcArray1[index + 2].Pos, current, ref nnsPriM3DPcArray1[index + 2].Pos);
                nnTransformVector(ref nnsPriM3DPcArray1[index + 5].Pos, current, ref nnsPriM3DPcArray1[index + 5].Pos);
                nnsPriM3DPcArray1[index + 5].Col = AMD_RGBA8888(runtimeWorkPlane.color.r, runtimeWorkPlane.color.g, runtimeWorkPlane.color.b, runtimeWorkPlane.color.a);
                nnsPriM3DPcArray1[index].Col = nnsPriM3DPcArray1[index + 1].Col = nnsPriM3DPcArray1[index + 2].Col = nnsPriM3DPcArray1[index + 5].Col;
                nnsPriM3DPcArray1[index + 3] = nnsPriM3DPcArray1[index + 1];
                nnsPriM3DPcArray1[index + 4] = nnsPriM3DPcArray1[index + 2];
                index += 6;
                amMatrixPop();
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