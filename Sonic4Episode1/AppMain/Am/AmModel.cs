public partial class AppMain
{
    public static int _amInitModel(object p)
    {
        AMS_AME_CREATE_PARAM amsAmeCreateParam = (AMS_AME_CREATE_PARAM)p;
        AMS_AME_NODE_MODEL node = (AMS_AME_NODE_MODEL)amsAmeCreateParam.node;
        AMS_AME_RUNTIME_WORK_MODEL work = (AMS_AME_RUNTIME_WORK_MODEL)amsAmeCreateParam.work;
        work.time = -node.start_time;
        work.scale.Assign(node.scale_start);
        work.set_color(node.color_start.color);
        amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        amVectorAdd(work.position, node.translate);
        amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        if (((int)node.flag & 4) != 0)
        {
            NNS_VECTOR4D nnsVectoR4D = GlobalPool<NNS_VECTOR4D>.Alloc();
            amVectorRandom(nnsVectoR4D);
            NNS_QUATERNION rotate = work.rotate;
            amQuatRotAxisToQuat(ref rotate, nnsVectoR4D, (float)(nnRandom() * 2.0 * 3.14159274101257));
            work.rotate = rotate;
        }
        else
            work.rotate = node.rotate;
        if (((int)node.flag & 8) != 0)
        {
            NNS_VECTOR4D pDst = GlobalPool<NNS_VECTOR4D>.Alloc();
            amVectorRandom(pDst);
            work.set_rotate_axis(pDst.x, pDst.y, pDst.z, node.rotate_axis.w);
        }
        else
            work.rotate_axis.Assign(node.rotate_axis);
        return 0;
    }

    public static int _amUpdateModel(object r)
    {
        AMS_AME_RUNTIME runtime = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_MODEL node = (AMS_AME_NODE_MODEL)runtime.node;
        AMS_AME_RUNTIME_WORK_MODEL next1 = (AMS_AME_RUNTIME_WORK_MODEL)(AMS_AME_RUNTIME_WORK)runtime.active_head.next;
        AMS_AME_LIST next2 = runtime.active_head.next;
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
        NNS_VECTOR4D nnsVectoR4D1 = GlobalPool<NNS_VECTOR4D>.Alloc();
        NNS_VECTOR4D nnsVectoR4D2 = GlobalPool<NNS_VECTOR4D>.Alloc();
        amVectorScale(nnsVectoR4D1, node.scale_start, runtime.ecb.size_rate);
        amVectorScale(nnsVectoR4D2, node.scale_end, runtime.ecb.size_rate);
        AMS_AME_RUNTIME_WORK_MODEL runtimeWorkModel;
        for (; next2 != activeTail; next2 = runtimeWorkModel.next)
        {
            runtimeWorkModel = (AMS_AME_RUNTIME_WORK_MODEL)(AMS_AME_RUNTIME_WORK)next2;
            runtimeWorkModel.time += _am_unit_frame;
            float num3 = runtimeWorkModel.time * num2;
            NNS_VECTOR4D nnsVectoR4D3 = GlobalPool<NNS_VECTOR4D>.Alloc();
            amVectorScale(nnsVectoR4D3, runtimeWorkModel.velocity, _am_unit_time);
            amVectorAdd(runtimeWorkModel.position, nnsVectoR4D3);
            GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D3);
            if (runtimeWorkModel.time >= (double)num1)
            {
                if (runtime.spawn_runtime != null)
                    _amCreateSpawnParticle(runtime, runtimeWorkModel);
                amEffectDisconnectLink((AMS_AME_LIST)runtimeWorkModel);
                --runtime.active_num;
                amEffectFreeRuntimeWork(runtimeWorkModel);
            }
            else
            {
                NNS_QUATERNION nnsQuaternion = new NNS_QUATERNION();
                NNS_VECTOR4D nnsVectoR4D4 = GlobalPool<NNS_VECTOR4D>.Alloc();
                Vector4D_Buf rotateAxis = runtimeWorkModel.rotate_axis;
                nnsVectoR4D4.x = rotateAxis.x;
                nnsVectoR4D4.y = rotateAxis.y;
                nnsVectoR4D4.z = rotateAxis.z;
                nnsVectoR4D4.w = rotateAxis.w;
                amQuatRotAxisToQuat(ref nnsQuaternion, nnsVectoR4D4, nnsVectoR4D4.w * _am_unit_time);
                NNS_QUATERNION rotate = runtimeWorkModel.rotate;
                amQuatMulti(ref rotate, ref rotate, ref nnsQuaternion);
                runtimeWorkModel.rotate = rotate;
                amVectorGetInner(nnsVectoR4D4, nnsVectoR4D1, nnsVectoR4D2, num3);
                runtimeWorkModel.set_scale(nnsVectoR4D4);
                AMS_RGBA8888 pCO;
                amEffectLerpColor(out pCO, ref node.color_start, ref node.color_end, num3);
                pCO.a = (byte)(pCO.a * transparency >> 8);
                runtimeWorkModel.set_color(pCO.color);
                runtimeWorkModel.scroll_u += node.scroll_u * _am_unit_time;
                runtimeWorkModel.scroll_v += node.scroll_v * _am_unit_time;
                GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D4);
            }
        }
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D1);
        GlobalPool<NNS_VECTOR4D>.Release(nnsVectoR4D2);
        return 0;
    }

    public static int _amDrawModel(object r)
    {
        AMS_AME_RUNTIME runtime = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_MODEL node = (AMS_AME_NODE_MODEL)runtime.node;
        AMS_AME_ECB ecb = runtime.ecb;
        NNS_RGBA color = new NNS_RGBA();
        int blend = -1;
        if (runtime.ecb.pObj == null)
            return 0;
        uint drawflag = _amEffectSetMaterial(runtime, ref blend, node.blend);
        float zBias = node.z_bias;
        SNNS_VECTOR4D pDst1;
        amVectorSet(out pDst1, zBias * _am_ef_worldViewMtx.M20, zBias * _am_ef_worldViewMtx.M21, zBias * _am_ef_worldViewMtx.M22);
        AMS_AME_LIST activeTail = runtime.active_tail;
        AMS_AME_LIST next = runtime.active_head.next;
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        AMS_AME_RUNTIME_WORK_MODEL runtimeWorkModel;
        for (; next != activeTail; next = runtimeWorkModel.next)
        {
            runtimeWorkModel = (AMS_AME_RUNTIME_WORK_MODEL)(AMS_AME_RUNTIME_WORK)next;
            amMatrixPush();
            SNNS_VECTOR4D pDst2;
            amVectorAdd(out pDst2, runtimeWorkModel.position, ref pDst1);
            NNS_QUATERNION rotate = runtimeWorkModel.rotate;
            amQuatMultiMatrix(ref rotate, ref pDst2);
            runtimeWorkModel.rotate = rotate;
            float scrollU = runtimeWorkModel.scroll_u;
            float scrollV = runtimeWorkModel.scroll_v;
            uint num = drawflag | 268435456U;
            if (scrollU >= 1.0)
            {
                scrollU -= (int)scrollU;
            }
            else
            {
                while (scrollU < 0.0)
                    ++scrollU;
            }
            if (scrollV >= 1.0)
            {
                scrollV -= (int)scrollV;
            }
            else
            {
                while (scrollV < 0.0)
                    ++scrollV;
            }
            drawflag = num | 3145728U;
            color.a = runtimeWorkModel.color.a * 0.003921569f;
            color.r = runtimeWorkModel.color.r * 0.003921569f;
            color.g = runtimeWorkModel.color.g * 0.003921569f;
            color.b = runtimeWorkModel.color.b * 0.003921569f;
            NNS_MATRIX current = amMatrixGetCurrent();
            nnsMatrix.Assign(current);
            nnScaleMatrix(nnsMatrix, nnsMatrix, runtimeWorkModel.scale.x, runtimeWorkModel.scale.y, runtimeWorkModel.scale.z);
            nnCopyMatrix(current, nnsMatrix);
            nnsVector.x = runtimeWorkModel.scale.x;
            nnsVector.y = runtimeWorkModel.scale.y;
            nnsVector.z = runtimeWorkModel.scale.z;
            amDrawObjectSetMaterial(runtime.ecb.drawObjState, ecb.pObj, runtime.texlist, nnsVector, ref color, scrollU, scrollV, blend, drawflag);
            runtimeWorkModel.set_scale(nnsVector);
            amMatrixPop();
        }
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
        return 0;
    }
}