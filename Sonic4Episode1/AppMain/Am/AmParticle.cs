public partial class AppMain
{
    private void amEffectCreateParticle(AMS_AME_CREATE_PARAM param)
    {
        _amCreateParticle(param);
    }

    public static int _amEffectFrustumCulling(
      NNS_VECTOR4D pPos,
      AMS_FRUSTUM pFrustum,
      AMS_AME_BOUNDING pBounding)
    {
        return 1;
    }

    public static void _amEffectFinalize(AMS_AME_ECB ecb)
    {
        for (AMS_AME_ENTRY entry = ecb.entry_head; entry != null; entry = (AMS_AME_ENTRY)entry.next)
        {
            AMS_AME_RUNTIME runtime = entry.runtime;
            if (AMD_AME_SUPER_CLASS_ID(runtime.node) == 256 && runtime.parent_runtime != null)
            {
                amEffectDisconnectLink(runtime);
                --runtime.parent_runtime.work_num;
            }
            _amFreeRuntime(entry.runtime);
            _amDelEntry(ecb, entry);
        }
        ecb.prev.next = ecb.next;
        ecb.next.prev = ecb.prev;
        _am_ecb_ref[_am_ecb_free] = ecb;
        ++_am_ecb_free;
        if (_am_ecb_free < 128)
            return;
        _am_ecb_free = 0;
    }

    public static AMS_AME_RUNTIME _amCreateRuntimeEmitter(
      AMS_AME_CREATE_PARAM param)
    {
        AMS_AME_RUNTIME runtime = _amAllocRuntime();
        runtime.ecb = param.ecb;
        runtime.node = param.node;
        runtime.child_head.next = runtime.child_tail;
        runtime.child_tail.prev = runtime.child_head;
        runtime.work_head.next = runtime.work_tail;
        runtime.work_tail.prev = runtime.work_head;
        runtime.active_head.next = runtime.active_tail;
        runtime.active_tail.prev = runtime.active_head;
        _amAddEntry(param.ecb, runtime);
        runtime.work = _amAllocRuntimeWork();
        param.work = runtime.work;
        int num = _am_emitter_func[(AMD_AME_NODE_TYPE(param.node) & byte.MaxValue) << 2](param);
        for (AMS_AME_NODE node = param.node.child; node != null; node = node.sibling)
        {
            if (!AMD_AME_IS_FIELD(node))
            {
                AMS_AME_RUNTIME runtimeGroup = _amCreateRuntimeGroup(param.ecb, node);
                _amConnectLinkToTail(runtime.child_tail, runtimeGroup);
                ++runtime.child_num;
            }
        }
        return runtime;
    }

    private static AMS_AME_RUNTIME _amCreateRuntimeParticle(
      AMS_AME_CREATE_PARAM param)
    {
        AMS_AME_RUNTIME runtime = _amAllocRuntime();
        runtime.ecb = param.ecb;
        runtime.node = param.node;
        runtime.state = 16384;
        runtime.child_head.next = runtime.child_tail;
        runtime.child_tail.prev = runtime.child_head;
        runtime.work_head.next = runtime.work_tail;
        runtime.work_tail.prev = runtime.work_head;
        runtime.active_head.next = runtime.active_tail;
        runtime.active_tail.prev = runtime.active_head;
        for (AMS_AME_NODE node = param.node.child; node != null; node = node.sibling)
        {
            if (AMD_AME_IS_PARTICLE(node))
            {
                runtime.spawn_runtime = _amCreateRuntimeGroup(param.ecb, node);
                break;
            }
        }
        _amAddEntry(param.ecb, runtime);
        param.runtime = runtime;
        _amCreateParticle(param);
        return runtime;
    }

    public static AMS_AME_RUNTIME _amCreateRuntimeGroup(
      AMS_AME_ECB ecb,
      AMS_AME_NODE node)
    {
        AMS_AME_RUNTIME runtime = _amAllocRuntime();
        runtime.ecb = ecb;
        runtime.node = node;
        runtime.child_head.next = runtime.child_tail;
        runtime.child_tail.prev = runtime.child_head;
        runtime.work_head.next = runtime.work_tail;
        runtime.work_tail.prev = runtime.work_head;
        runtime.active_head.next = runtime.active_tail;
        runtime.active_tail.prev = runtime.active_head;
        for (AMS_AME_NODE node1 = node.child; node1 != null; node1 = node1.sibling)
        {
            if (AMD_AME_IS_PARTICLE(node1))
            {
                runtime.spawn_runtime = _amCreateRuntimeGroup(ecb, node1);
                break;
            }
        }
        _amAddEntry(ecb, runtime);
        return runtime;
    }

    public static void _amCreateEmitter(AMS_AME_CREATE_PARAM param)
    {
        AMS_AME_RUNTIME runtimeEmitter = _amCreateRuntimeEmitter(param);
        runtimeEmitter.parent_runtime = param.runtime;
        _amConnectLinkToTail(runtimeEmitter.work_tail, runtimeEmitter);
        ++runtimeEmitter.work_num;
        param.runtime = runtimeEmitter;
        param.node = runtimeEmitter.node;
        param.work = runtimeEmitter.work;
    }

    public static void _amCreateParticle(AMS_AME_CREATE_PARAM param)
    {
        AMS_AME_RUNTIME_WORK_MODEL runtimeWorkModel = (AMS_AME_RUNTIME_WORK_MODEL)_amAllocRuntimeWork();
        param.work = runtimeWorkModel;
        int num = _am_particle_func[(AMD_AME_NODE_TYPE(param.node) & byte.MaxValue) << 2](param);
        if (runtimeWorkModel.time < 0.0)
        {
            _amConnectLinkToTail(param.runtime.work_tail, (AMS_AME_LIST)runtimeWorkModel);
            ++param.runtime.work_num;
        }
        else
        {
            _amConnectLinkToTail(param.runtime.active_tail, (AMS_AME_LIST)runtimeWorkModel);
            ++param.runtime.active_num;
        }
    }

    public static void _amCreateSpawnParticle(
      AMS_AME_RUNTIME runtime,
      AMS_AME_RUNTIME_WORK work)
    {
        NNS_VECTOR4D pVec = GlobalPool<NNS_VECTOR4D>.Alloc();
        amVectorInit(pVec);
        AMS_AME_CREATE_PARAM amsAmeCreateParam = GlobalPool<AMS_AME_CREATE_PARAM>.Alloc();
        amsAmeCreateParam.ecb = runtime.ecb;
        amsAmeCreateParam.runtime = runtime.spawn_runtime;
        amsAmeCreateParam.node = runtime.spawn_runtime.node;
        amsAmeCreateParam.position = pVec;
        amsAmeCreateParam.velocity = pVec;
        amsAmeCreateParam.parent_position = work.position;
        amsAmeCreateParam.parent_velocity = work.velocity;
        if ((runtime.state & 8192) != 0)
            runtime.spawn_runtime.state |= 8192;
        _amCreateParticle(amsAmeCreateParam);
        if (AMD_AME_NODE_TYPE(runtime.node) == AMD_AME_NODE_TYPE(amsAmeCreateParam.node))
        {
            AMS_AME_NODE node1 = runtime.node;
            AMS_AME_NODE node2 = amsAmeCreateParam.node;
            switch (AMD_AME_NODE_TYPE(amsAmeCreateParam.node))
            {
                case 512:
                    AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE workSimpleSprite = (AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)work;
                    AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE work1 = (AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)amsAmeCreateParam.work;
                    if (((int)node1.flag & (int)node2.flag & 131072) != 0 && (((int)workSimpleSprite.flag ^ (int)work1.flag) & 8) != 0)
                    {
                        work1.flag ^= 8U;
                        work1.set_st(work1.st.z, work1.st.y, work1.st.x, work1.st.w);
                    }
                    if (((int)node1.flag & (int)node2.flag & 262144) != 0 && (((int)workSimpleSprite.flag ^ (int)work1.flag) & 16) != 0)
                    {
                        work1.flag ^= 16U;
                        work1.set_st(work1.st.x, work1.st.w, work1.st.z, work1.st.y);
                        break;
                    }
                    break;
                case 513:
                    AMS_AME_RUNTIME_WORK_SPRITE runtimeWorkSprite = (AMS_AME_RUNTIME_WORK_SPRITE)work;
                    AMS_AME_RUNTIME_WORK_SPRITE work2 = (AMS_AME_RUNTIME_WORK_SPRITE)amsAmeCreateParam.work;
                    if (((int)node1.flag & (int)node2.flag & 4) != 0)
                    {
                        if (((int)runtimeWorkSprite.flag & 4) != 0)
                            work2.flag |= 4U;
                        else
                            work2.flag &= 4294967291U;
                    }
                    if (((int)node1.flag & (int)node2.flag & 131072) != 0 && (((int)runtimeWorkSprite.flag ^ (int)work2.flag) & 8) != 0)
                    {
                        work2.flag ^= 8U;
                        work2.set_st(work2.st.z, work2.st.y, work2.st.x, work2.st.w);
                    }
                    if (((int)node1.flag & (int)node2.flag & 262144) != 0 && (((int)runtimeWorkSprite.flag ^ (int)work2.flag) & 16) != 0)
                    {
                        work2.flag ^= 16U;
                        work2.set_st(work2.st.x, work2.st.w, work2.st.z, work2.st.y);
                        break;
                    }
                    break;
                case 514:
                    AMS_AME_RUNTIME_WORK_LINE ameRuntimeWorkLine = (AMS_AME_RUNTIME_WORK_LINE)work;
                    AMS_AME_RUNTIME_WORK_LINE work3 = (AMS_AME_RUNTIME_WORK_LINE)amsAmeCreateParam.work;
                    if (((int)node1.flag & (int)node2.flag & 131072) != 0 && (((int)ameRuntimeWorkLine.flag ^ (int)work3.flag) & 8) != 0)
                    {
                        work3.flag ^= 8U;
                        work3.set_st(work3.st.z, work3.st.y, work3.st.x, work3.st.w);
                    }
                    if (((int)node1.flag & (int)node2.flag & 262144) != 0 && (((int)ameRuntimeWorkLine.flag ^ (int)work3.flag) & 16) != 0)
                    {
                        work3.flag ^= 16U;
                        work3.set_st(work3.st.x, work3.st.w, work3.st.z, work3.st.y);
                        break;
                    }
                    break;
                case 515:
                    AMS_AME_RUNTIME_WORK_PLANE runtimeWorkPlane = (AMS_AME_RUNTIME_WORK_PLANE)work;
                    AMS_AME_RUNTIME_WORK_PLANE work4 = (AMS_AME_RUNTIME_WORK_PLANE)amsAmeCreateParam.work;
                    if (((int)node1.flag & (int)node2.flag & 131072) != 0 && (((int)runtimeWorkPlane.flag ^ (int)work4.flag) & 8) != 0)
                    {
                        work4.flag ^= 8U;
                        work4.set_st(work4.st.z, work4.st.y, work4.st.x, work4.st.w);
                    }
                    if (((int)node1.flag & (int)node2.flag & 262144) != 0 && (((int)runtimeWorkPlane.flag ^ (int)work4.flag) & 16) != 0)
                    {
                        work4.flag ^= 16U;
                        work4.set_st(work4.st.x, work4.st.w, work4.st.z, work4.st.y);
                        break;
                    }
                    break;
            }
        }
        GlobalPool<AMS_AME_CREATE_PARAM>.Release(amsAmeCreateParam);
        GlobalPool<NNS_VECTOR4D>.Release(pVec);
    }

}