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
    private void amEffectCreateParticle(AppMain.AMS_AME_CREATE_PARAM param)
    {
        AppMain._amCreateParticle(param);
    }

    public static int _amEffectFrustumCulling(
      AppMain.NNS_VECTOR4D pPos,
      AppMain.AMS_FRUSTUM pFrustum,
      AppMain.AMS_AME_BOUNDING pBounding)
    {
        return 1;
    }

    public static void _amEffectFinalize(AppMain.AMS_AME_ECB ecb)
    {
        for (AppMain.AMS_AME_ENTRY entry = ecb.entry_head; entry != null; entry = (AppMain.AMS_AME_ENTRY)entry.next)
        {
            AppMain.AMS_AME_RUNTIME runtime = entry.runtime;
            if (AppMain.AMD_AME_SUPER_CLASS_ID(runtime.node) == 256 && runtime.parent_runtime != null)
            {
                AppMain.amEffectDisconnectLink((AppMain.AMS_AME_LIST)runtime);
                --runtime.parent_runtime.work_num;
            }
            AppMain._amFreeRuntime(entry.runtime);
            AppMain._amDelEntry(ecb, entry);
        }
        ecb.prev.next = ecb.next;
        ecb.next.prev = ecb.prev;
        AppMain._am_ecb_ref[AppMain._am_ecb_free] = ecb;
        ++AppMain._am_ecb_free;
        if (AppMain._am_ecb_free < 128)
            return;
        AppMain._am_ecb_free = 0;
    }

    public static AppMain.AMS_AME_RUNTIME _amCreateRuntimeEmitter(
      AppMain.AMS_AME_CREATE_PARAM param)
    {
        AppMain.AMS_AME_RUNTIME runtime = AppMain._amAllocRuntime();
        runtime.ecb = param.ecb;
        runtime.node = param.node;
        runtime.child_head.next = runtime.child_tail;
        runtime.child_tail.prev = runtime.child_head;
        runtime.work_head.next = runtime.work_tail;
        runtime.work_tail.prev = runtime.work_head;
        runtime.active_head.next = runtime.active_tail;
        runtime.active_tail.prev = runtime.active_head;
        AppMain._amAddEntry(param.ecb, runtime);
        runtime.work = AppMain._amAllocRuntimeWork();
        param.work = runtime.work;
        int num = AppMain._am_emitter_func[(AppMain.AMD_AME_NODE_TYPE(param.node) & (int)byte.MaxValue) << 2]((object)param);
        for (AppMain.AMS_AME_NODE node = param.node.child; node != null; node = node.sibling)
        {
            if (!AppMain.AMD_AME_IS_FIELD(node))
            {
                AppMain.AMS_AME_RUNTIME runtimeGroup = AppMain._amCreateRuntimeGroup(param.ecb, node);
                AppMain._amConnectLinkToTail(runtime.child_tail, (AppMain.AMS_AME_LIST)runtimeGroup);
                ++runtime.child_num;
            }
        }
        return runtime;
    }

    private static AppMain.AMS_AME_RUNTIME _amCreateRuntimeParticle(
      AppMain.AMS_AME_CREATE_PARAM param)
    {
        AppMain.AMS_AME_RUNTIME runtime = AppMain._amAllocRuntime();
        runtime.ecb = param.ecb;
        runtime.node = param.node;
        runtime.state = 16384;
        runtime.child_head.next = runtime.child_tail;
        runtime.child_tail.prev = runtime.child_head;
        runtime.work_head.next = runtime.work_tail;
        runtime.work_tail.prev = runtime.work_head;
        runtime.active_head.next = runtime.active_tail;
        runtime.active_tail.prev = runtime.active_head;
        for (AppMain.AMS_AME_NODE node = param.node.child; node != null; node = node.sibling)
        {
            if (AppMain.AMD_AME_IS_PARTICLE(node))
            {
                runtime.spawn_runtime = AppMain._amCreateRuntimeGroup(param.ecb, node);
                break;
            }
        }
        AppMain._amAddEntry(param.ecb, runtime);
        param.runtime = runtime;
        AppMain._amCreateParticle(param);
        return runtime;
    }

    public static AppMain.AMS_AME_RUNTIME _amCreateRuntimeGroup(
      AppMain.AMS_AME_ECB ecb,
      AppMain.AMS_AME_NODE node)
    {
        AppMain.AMS_AME_RUNTIME runtime = AppMain._amAllocRuntime();
        runtime.ecb = ecb;
        runtime.node = node;
        runtime.child_head.next = runtime.child_tail;
        runtime.child_tail.prev = runtime.child_head;
        runtime.work_head.next = runtime.work_tail;
        runtime.work_tail.prev = runtime.work_head;
        runtime.active_head.next = runtime.active_tail;
        runtime.active_tail.prev = runtime.active_head;
        for (AppMain.AMS_AME_NODE node1 = node.child; node1 != null; node1 = node1.sibling)
        {
            if (AppMain.AMD_AME_IS_PARTICLE(node1))
            {
                runtime.spawn_runtime = AppMain._amCreateRuntimeGroup(ecb, node1);
                break;
            }
        }
        AppMain._amAddEntry(ecb, runtime);
        return runtime;
    }

    public static void _amCreateEmitter(AppMain.AMS_AME_CREATE_PARAM param)
    {
        AppMain.AMS_AME_RUNTIME runtimeEmitter = AppMain._amCreateRuntimeEmitter(param);
        runtimeEmitter.parent_runtime = param.runtime;
        AppMain._amConnectLinkToTail(runtimeEmitter.work_tail, (AppMain.AMS_AME_LIST)runtimeEmitter);
        ++runtimeEmitter.work_num;
        param.runtime = runtimeEmitter;
        param.node = runtimeEmitter.node;
        param.work = runtimeEmitter.work;
    }

    public static void _amCreateParticle(AppMain.AMS_AME_CREATE_PARAM param)
    {
        AppMain.AMS_AME_RUNTIME_WORK_MODEL runtimeWorkModel = (AppMain.AMS_AME_RUNTIME_WORK_MODEL)AppMain._amAllocRuntimeWork();
        param.work = (AppMain.AMS_AME_RUNTIME_WORK)runtimeWorkModel;
        int num = AppMain._am_particle_func[(AppMain.AMD_AME_NODE_TYPE(param.node) & (int)byte.MaxValue) << 2]((object)param);
        if ((double)runtimeWorkModel.time < 0.0)
        {
            AppMain._amConnectLinkToTail(param.runtime.work_tail, (AppMain.AMS_AME_LIST)runtimeWorkModel);
            ++param.runtime.work_num;
        }
        else
        {
            AppMain._amConnectLinkToTail(param.runtime.active_tail, (AppMain.AMS_AME_LIST)runtimeWorkModel);
            ++param.runtime.active_num;
        }
    }

    public static void _amCreateSpawnParticle(
      AppMain.AMS_AME_RUNTIME runtime,
      AppMain.AMS_AME_RUNTIME_WORK work)
    {
        AppMain.NNS_VECTOR4D pVec = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.amVectorInit(pVec);
        AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = AppMain.GlobalPool<AppMain.AMS_AME_CREATE_PARAM>.Alloc();
        amsAmeCreateParam.ecb = runtime.ecb;
        amsAmeCreateParam.runtime = runtime.spawn_runtime;
        amsAmeCreateParam.node = runtime.spawn_runtime.node;
        amsAmeCreateParam.position = pVec;
        amsAmeCreateParam.velocity = pVec;
        amsAmeCreateParam.parent_position = work.position;
        amsAmeCreateParam.parent_velocity = work.velocity;
        if ((runtime.state & 8192) != 0)
            runtime.spawn_runtime.state |= 8192;
        AppMain._amCreateParticle(amsAmeCreateParam);
        if (AppMain.AMD_AME_NODE_TYPE(runtime.node) == AppMain.AMD_AME_NODE_TYPE(amsAmeCreateParam.node))
        {
            AppMain.AMS_AME_NODE node1 = runtime.node;
            AppMain.AMS_AME_NODE node2 = amsAmeCreateParam.node;
            switch (AppMain.AMD_AME_NODE_TYPE(amsAmeCreateParam.node))
            {
                case 512:
                    AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE workSimpleSprite = (AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)work;
                    AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE work1 = (AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE)amsAmeCreateParam.work;
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
                    AppMain.AMS_AME_RUNTIME_WORK_SPRITE runtimeWorkSprite = (AppMain.AMS_AME_RUNTIME_WORK_SPRITE)work;
                    AppMain.AMS_AME_RUNTIME_WORK_SPRITE work2 = (AppMain.AMS_AME_RUNTIME_WORK_SPRITE)amsAmeCreateParam.work;
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
                    AppMain.AMS_AME_RUNTIME_WORK_LINE ameRuntimeWorkLine = (AppMain.AMS_AME_RUNTIME_WORK_LINE)work;
                    AppMain.AMS_AME_RUNTIME_WORK_LINE work3 = (AppMain.AMS_AME_RUNTIME_WORK_LINE)amsAmeCreateParam.work;
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
                    AppMain.AMS_AME_RUNTIME_WORK_PLANE runtimeWorkPlane = (AppMain.AMS_AME_RUNTIME_WORK_PLANE)work;
                    AppMain.AMS_AME_RUNTIME_WORK_PLANE work4 = (AppMain.AMS_AME_RUNTIME_WORK_PLANE)amsAmeCreateParam.work;
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
        AppMain.GlobalPool<AppMain.AMS_AME_CREATE_PARAM>.Release(amsAmeCreateParam);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(pVec);
    }

}