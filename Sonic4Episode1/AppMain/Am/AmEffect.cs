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





    public static int AMM_BLEND(int equ, int a, int b)
    {
        return equ << 8 | b << 4 | a;
    }

    public static int AMD_AME_NODE_TYPE(AppMain.AMS_AME_NODE node)
    {
        return (int)node.type;
    }

    public static int AMD_AME_SUPER_CLASS_ID(AppMain.AMS_AME_NODE node)
    {
        return (int)(ushort)node.type & 65280;
    }

    public static int AMD_AME_CLASS_ID(AppMain.AMS_AME_NODE node)
    {
        return (int)(ushort)node.type & (int)byte.MaxValue;
    }

    public static bool AMD_AME_IS_EMITTER(AppMain.AMS_AME_NODE node)
    {
        return AppMain.AMD_AME_SUPER_CLASS_ID(node) == 256;
    }

    public static bool AMD_AME_IS_PARTICLE(AppMain.AMS_AME_NODE node)
    {
        return AppMain.AMD_AME_SUPER_CLASS_ID(node) == 512;
    }

    public static bool AMD_AME_IS_FIELD(AppMain.AMS_AME_NODE node)
    {
        return AppMain.AMD_AME_SUPER_CLASS_ID(node) == 768;
    }

    public static int amEffectIsDelete(AppMain.AMS_AME_ECB ecb)
    {
        return ecb.entry_num >= 0 ? 0 : 1;
    }

    private AppMain.AMS_AME_ECB amEffectCreate(AppMain.AMS_AME_NODE node)
    {
        return this.amEffectCreate(node, 0, 0);
    }

    private AppMain.AMS_AME_ECB amEffectCreate(AppMain.AMS_AME_HEADER header)
    {
        return AppMain._amEffectCreate(header, 0, 0);
    }

    private void amEffectSetTransparency(AppMain.AMS_AME_ECB ecb, float t)
    {
        ecb.transparency = (int)((double)t * 256.0);
    }

    public static void amEffectSetSizeRate(AppMain.AMS_AME_ECB ecb, float t)
    {
        ecb.size_rate = t;
    }

    public static void amEffectLerpColor(
      out AppMain.AMS_RGBA8888 pCO,
      ref AppMain.AMS_RGBA8888 pC1,
      ref AppMain.AMS_RGBA8888 pC2,
      float rate)
    {
        int num = (int)((double)rate * 256.0);
        pCO = new AppMain.AMS_RGBA8888();
        pCO.r = (byte)(((int)pC1.r << 8) + ((int)pC2.r - (int)pC1.r) * num >> 8);
        pCO.g = (byte)(((int)pC1.g << 8) + ((int)pC2.g - (int)pC1.g) * num >> 8);
        pCO.b = (byte)(((int)pC1.b << 8) + ((int)pC2.b - (int)pC1.b) * num >> 8);
        pCO.a = (byte)(((int)pC1.a << 8) + ((int)pC2.a - (int)pC1.a) * num >> 8);
    }

    public static void amEffectDisconnectLink(AppMain.AMS_AME_LIST list)
    {
        list.prev.next = list.next;
        list.next.prev = list.prev;
    }

    private void amEffectRandomConeVector(ref Vector4 pOut, float s)
    {
        AppMain.mppAssertNotImpl();
    }

    public static void amEffectRandomConeVectorDeg(AppMain.NNS_VECTOR4D pOut, float s)
    {
        float pCs = AppMain.nnCos(AppMain.NNM_DEGtoA32(s));
        float y = AppMain.nnRandom() * (1f - pCs) + pCs;
        float num1 = (float)Math.Sqrt(1.0 - (double)y * (double)y);
        AppMain.amSinCos(AppMain.nnRandom() * 6.283185f, out s, out pCs);
        AppMain.amVectorSet(pOut, num1 * pCs, y, num1 * s);
        double num2 = (double)AppMain.amVectorUnit(pOut);
    }

    private void _amConnectLinkToHead(AppMain.AMS_AME_LIST head, AppMain.AMS_AME_LIST list)
    {
        list.next = head.next;
        list.prev = head.next.prev;
        head.next.prev = list;
        head.next = list;
    }

    public static void _amConnectLinkToTail(AppMain.AMS_AME_LIST tail, AppMain.AMS_AME_LIST list)
    {
        list.prev = tail.prev;
        list.next = tail.prev.next;
        tail.prev.next = list;
        tail.prev = list;
    }

    public static void amEffectSystemInit()
    {
        AppMain.NNS_RGBA diffuse = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_RGB ambient = new AppMain.NNS_RGB(1f, 1f, 1f);
        AppMain.nnSetPrimitive3DMaterial(ref diffuse, ref ambient, 1f);
        AppMain._am_enable_draw = 1;
        AppMain._am_unit_frame = 1f;
        AppMain._am_unit_time = 0.01666667f;
        AppMain._am_ecb_alloc = 0;
        AppMain._am_ecb_free = 0;
        AppMain._am_ecb_head.Clear();
        AppMain._am_ecb_tail.Clear();
        AppMain._am_ecb_head.next = (AppMain.AMS_AME_LIST)AppMain._am_ecb_tail;
        AppMain._am_ecb_tail.prev = (AppMain.AMS_AME_LIST)AppMain._am_ecb_head;
        AppMain._am_ecb_buf = AppMain.New<AppMain.AMS_AME_ECB>(128);
        AppMain._am_ecb_ref = new AppMain.AMS_AME_ECB[128];
        for (int index = 0; index < 128; ++index)
            AppMain._am_ecb_ref[index] = AppMain._am_ecb_buf[index];
        AppMain._am_entry_alloc = 0;
        AppMain._am_entry_free = 0;
        AppMain._am_entry_buf = new AppMain.AMS_AME_ENTRY[512];
        AppMain._am_entry_ref = new AppMain.AMS_AME_ENTRY[512];
        for (int index = 0; index < 512; ++index)
        {
            AppMain._am_entry_buf[index] = new AppMain.AMS_AME_ENTRY();
            AppMain._am_entry_ref[index] = AppMain._am_entry_buf[index];
        }
        AppMain._am_runtime_alloc = 0;
        AppMain._am_runtime_free = 0;
        AppMain._am_runtime_buf = new AppMain.AMS_AME_RUNTIME[512];
        AppMain._am_runtime_ref = new AppMain.AMS_AME_RUNTIME[512];
        for (int index = 0; index < 512; ++index)
        {
            AppMain._am_runtime_buf[index] = new AppMain.AMS_AME_RUNTIME();
            AppMain._am_runtime_ref[index] = AppMain._am_runtime_buf[index];
        }
        AppMain._am_work_alloc = 0;
        AppMain._am_work_free = 0;
        AppMain._am_work_buf = new AppMain.AMS_AME_RUNTIME_WORK[1024];
        AppMain._am_work_ref = new AppMain.AMS_AME_RUNTIME_WORK[1024];
        for (int index = 0; index < 1024; ++index)
        {
            AppMain._am_work_buf[index] = new AppMain.AMS_AME_RUNTIME_WORK();
            AppMain._am_work_ref[index] = AppMain._am_work_buf[index];
        }
    }

    private void amEffectSystemReset()
    {
        AppMain.mppAssertNotImpl();
        AppMain.amEffectSystemInit();
    }

    public static void amEffectExecute()
    {
        for (AppMain.AMS_AME_ECB next = (AppMain.AMS_AME_ECB)AppMain._am_ecb_head.next; next != AppMain._am_ecb_tail; next = (AppMain.AMS_AME_ECB)next.next)
        {
            if (next.entry_num < 0)
                AppMain._amEffectFinalize(next);
        }
    }

    private void amEffectRegistCustomFunc(int classId, AppMain.AMS_AME_CUSTOM_PARAM pParam)
    {
        AppMain.mppAssertNotImpl();
        switch (classId & 65280)
        {
            case 256:
                int index1 = (classId & (int)byte.MaxValue) << 2;
                AppMain._am_emitter_func[index1] = pParam.pInitFunc;
                AppMain._am_emitter_func[index1 + 1] = pParam.pUpdateFunc;
                AppMain._am_emitter_func[index1 + 2] = pParam.pDrawFunc;
                break;
            case 512:
                int index2 = (classId & (int)byte.MaxValue) << 2;
                AppMain._am_particle_func[index2] = pParam.pInitFunc;
                AppMain._am_particle_func[index2 + 1] = pParam.pUpdateFunc;
                AppMain._am_particle_func[index2 + 2] = pParam.pDrawFunc;
                break;
            case 768:
                int index3 = classId & (int)byte.MaxValue;
                AppMain._am_field_func[index3] = pParam.pFieldFunc;
                break;
        }
    }

    private void amEffectUnregistCustomFunc(int classId)
    {
        AppMain.mppAssertNotImpl();
        switch (classId & 65280)
        {
            case 256:
                int index1 = (classId & (int)byte.MaxValue) << 2;
                AppMain._am_emitter_func[index1] = (AppMain.AmeDelegateFunc)null;
                AppMain._am_emitter_func[index1 + 1] = (AppMain.AmeDelegateFunc)null;
                AppMain._am_emitter_func[index1 + 2] = (AppMain.AmeDelegateFunc)null;
                break;
            case 512:
                int index2 = (classId & (int)byte.MaxValue) << 2;
                AppMain._am_particle_func[index2] = (AppMain.AmeDelegateFunc)null;
                AppMain._am_particle_func[index2 + 1] = (AppMain.AmeDelegateFunc)null;
                AppMain._am_particle_func[index2 + 2] = (AppMain.AmeDelegateFunc)null;
                break;
            case 768:
                int index3 = classId & (int)byte.MaxValue;
                AppMain._am_field_func[index3] = (AppMain.AmeFieldFunc)null;
                break;
        }
    }

    public static void amEffectSetObject(
      AppMain.AMS_AME_ECB ecb,
      AppMain.NNS_OBJECT object_,
      int state)
    {
        ecb.pObj = object_;
        ecb.drawObjState = (uint)state;
    }

    private static void amEffectSetWorldViewMatrix(AppMain.NNS_MATRIX mtx)
    {
        AppMain.nnCopyMatrix(AppMain._am_ef_worldViewMtx, mtx);
    }

    public static void amEffectSetCameraPos(ref AppMain.SNNS_VECTOR pos)
    {
        AppMain.nnCopyVector(AppMain._am_ef_camPos, ref pos);
    }

    public static void amEffectSetCameraPos(AppMain.NNS_VECTOR pos)
    {
        AppMain.nnCopyVector(AppMain._am_ef_camPos, pos);
    }

    private void amEffectEnableDraw(int flag)
    {
        AppMain.mppAssertNotImpl();
        AppMain._am_enable_draw = flag;
    }

    public static void amEffectSetUnitTime(float speed, int frame_rate)
    {
        AppMain._am_unit_frame = speed;
        AppMain._am_unit_time = speed / (float)frame_rate;
    }

    public static float amEffectGetUnitFrame()
    {
        return AppMain._am_unit_frame;
    }

    private AppMain.AMS_AME_NODE amEffectSearchNode(AppMain.AMS_AME_NODE node, int id)
    {
        AppMain.mppAssertNotImpl();
        AppMain.AMS_AME_NODE amsAmeNode = (AppMain.AMS_AME_NODE)null;
        if ((int)node.id == id)
            return node;
        if (node.child != null)
        {
            amsAmeNode = this.amEffectSearchNode(node.child, id);
            if (amsAmeNode != null)
                return amsAmeNode;
        }
        if (node.sibling != null)
            amsAmeNode = this.amEffectSearchNode(node.sibling, id);
        return amsAmeNode;
    }

    private AppMain.AMS_AME_NODE amEffectSearchNode(AppMain.AME_HEADER header, int id)
    {
        return this.amEffectSearchNode(header.node[0], id);
    }

    private AppMain.AMS_AME_ECB amEffectCreate(
      AppMain.AMS_AME_NODE node,
      int attribute,
      int priority)
    {
        AppMain.mppAssertNotImpl();
        return (AppMain.AMS_AME_ECB)null;
    }

    private static AppMain.AMS_AME_ECB _amEffectCreate(
      AppMain.AMS_AME_HEADER header,
      int attribute,
      int priority)
    {
        AppMain.AMS_AME_ECB amsAmeEcb = AppMain._am_ecb_ref[AppMain._am_ecb_alloc];
        ++AppMain._am_ecb_alloc;
        if (AppMain._am_ecb_alloc >= 128)
            AppMain._am_ecb_alloc = 0;
        amsAmeEcb.Clear();
        amsAmeEcb.attribute = attribute;
        amsAmeEcb.priority = priority;
        amsAmeEcb.transparency = 256;
        amsAmeEcb.size_rate = 1f;
        AppMain.amVectorInit(amsAmeEcb.translate);
        AppMain.amQuatInit(ref amsAmeEcb.rotate);
        amsAmeEcb.bounding.Assign(header.bounding);
        AppMain.AMS_AME_ECB next = (AppMain.AMS_AME_ECB)AppMain._am_ecb_head.next;
        while (next != AppMain._am_ecb_tail && next.priority <= priority)
            next = (AppMain.AMS_AME_ECB)next.next;
        next.prev.next = (AppMain.AMS_AME_LIST)amsAmeEcb;
        amsAmeEcb.prev = next.prev;
        next.prev = (AppMain.AMS_AME_LIST)amsAmeEcb;
        amsAmeEcb.next = (AppMain.AMS_AME_LIST)next;
        AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = new AppMain.AMS_AME_CREATE_PARAM();
        AppMain.NNS_VECTOR4D amEffectCreateVec = AppMain._amEffectCreate_vec;
        AppMain.AMS_AME_NODE sibling = header.node[0];
        AppMain.amVectorInit(amEffectCreateVec);
        for (; sibling != null; sibling = sibling.sibling)
        {
            amsAmeCreateParam.ecb = amsAmeEcb;
            amsAmeCreateParam.runtime = (AppMain.AMS_AME_RUNTIME)null;
            amsAmeCreateParam.node = sibling;
            amsAmeCreateParam.position = amEffectCreateVec;
            amsAmeCreateParam.velocity = amEffectCreateVec;
            amsAmeCreateParam.parent_position = amEffectCreateVec;
            amsAmeCreateParam.parent_velocity = amEffectCreateVec;
            switch (AppMain.AMD_AME_SUPER_CLASS_ID(sibling))
            {
                case 256:
                    AppMain._amCreateRuntimeEmitter(amsAmeCreateParam).state |= 8192;
                    break;
                case 512:
                    AppMain._amCreateRuntimeParticle(amsAmeCreateParam).state |= 8192;
                    break;
            }
        }
        amsAmeEcb.skip_update = 1;
        return amsAmeEcb;
    }

    private void amEffectDeleteGroup(int attr, int flag)
    {
        AppMain.mppAssertNotImpl();
        uint num1 = (uint)(attr & -65536);
        uint num2 = (uint)(attr & (int)ushort.MaxValue);
        if (num1 == 0U)
            num1 = 4294901760U;
        switch (flag)
        {
            case 0:
                for (AppMain.AMS_AME_ECB next = (AppMain.AMS_AME_ECB)AppMain._am_ecb_head.next; next != AppMain._am_ecb_tail; next = (AppMain.AMS_AME_ECB)next.next)
                {
                    if (((long)next.attribute & (long)num1) != 0L && ((long)next.attribute & (long)num2) != 0L)
                        AppMain.amEffectDelete(next);
                }
                break;
            case 1:
                for (AppMain.AMS_AME_ECB next = (AppMain.AMS_AME_ECB)AppMain._am_ecb_head.next; next != AppMain._am_ecb_tail; next = (AppMain.AMS_AME_ECB)next.next)
                {
                    if (((long)next.attribute & (long)num1 | (long)next.attribute & (long)num2) == (long)attr)
                        AppMain.amEffectDelete(next);
                }
                break;
        }
    }

    public static void amEffectKill(AppMain.AMS_AME_ECB ecb)
    {
        for (AppMain.AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AppMain.AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AppMain.AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((AppMain.AMD_AME_NODE_TYPE(runtime.node) & 65280) == 256 && (runtime.state & 32768) == 0)
            {
                if (runtime.spawn_runtime != null)
                    runtime.spawn_runtime.state |= 16384;
                runtime.state |= 32768;
                AppMain.AMS_AME_LIST next = runtime.child_head.next;
                for (AppMain.AMS_AME_LIST childTail = runtime.child_tail; next != childTail; next = next.next)
                    ((AppMain.AMS_AME_RUNTIME)next).state |= 16384;
                if (runtime.parent_runtime != null)
                {
                    AppMain.amEffectDisconnectLink((AppMain.AMS_AME_LIST)runtime);
                    --runtime.parent_runtime.work_num;
                }
            }
        }
    }

    private void amEffectKillGroup(int attr, int flag)
    {
        uint num1 = (uint)(attr & -65536);
        uint num2 = (uint)(attr & (int)ushort.MaxValue);
        if (num1 == 0U)
            num1 = 4294901760U;
        switch (flag)
        {
            case 0:
                for (AppMain.AMS_AME_ECB next = (AppMain.AMS_AME_ECB)AppMain._am_ecb_head.next; next != AppMain._am_ecb_tail; next = (AppMain.AMS_AME_ECB)next.next)
                {
                    if (((long)next.attribute & (long)num1) != 0L && ((long)next.attribute & (long)num2) != 0L)
                        AppMain.amEffectKill(next);
                }
                break;
            case 1:
                for (AppMain.AMS_AME_ECB next = (AppMain.AMS_AME_ECB)AppMain._am_ecb_head.next; next != AppMain._am_ecb_tail; next = (AppMain.AMS_AME_ECB)next.next)
                {
                    if (((long)next.attribute & (long)num1) != 0L && ((long)next.attribute & (long)num2) == (long)num2)
                        AppMain.amEffectKill(next);
                }
                break;
        }
    }

    public static void amEffectUpdate(AppMain.AMS_AME_ECB ecb)
    {
        if (ecb.entry_num <= 0)
            return;
        if (ecb.skip_update != 0)
            ecb.skip_update = 0;
        for (AppMain.AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AppMain.AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AppMain.AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            AppMain.AMS_AME_NODE node1 = runtime.node;
            int num1 = AppMain.AMD_AME_NODE_TYPE(node1);
            if ((runtime.state & 16384) != 0 && (int)runtime.work_num + (int)runtime.active_num == 0)
            {
                if (runtime.spawn_runtime != null)
                    runtime.spawn_runtime.state |= 16384;
                runtime.state |= 32768;
            }
            else
            {
                switch (num1 & 65280)
                {
                    case 256:
                        if (runtime.work != null)
                        {
                            AppMain.AmeDelegateFunc ameDelegateFunc = AppMain._am_emitter_func[((num1 & (int)byte.MaxValue) << 2) + 1];
                            if (runtime.work != null)
                            {
                                if (ameDelegateFunc((object)runtime) != 0)
                                {
                                    runtime.state |= 32768;
                                    AppMain.AMS_AME_LIST next = runtime.child_head.next;
                                    for (AppMain.AMS_AME_LIST childTail = runtime.child_tail; next != childTail; next = next.next)
                                        ((AppMain.AMS_AME_RUNTIME)next).state |= 16384;
                                    if (runtime.parent_runtime != null)
                                    {
                                        AppMain.amEffectDisconnectLink((AppMain.AMS_AME_LIST)runtime);
                                        --runtime.parent_runtime.work_num;
                                        continue;
                                    }
                                    continue;
                                }
                                for (AppMain.AMS_AME_NODE node2 = node1.child; node2 != null; node2 = node2.sibling)
                                {
                                    if (AppMain.AMD_AME_IS_FIELD(node2))
                                        AppMain._am_field_func[AppMain.AMD_AME_NODE_TYPE(node2) & (int)byte.MaxValue](runtime.ecb, node2, runtime.work);
                                }
                                continue;
                            }
                            continue;
                        }
                        continue;
                    case 512:
                        if (runtime.work_num != (short)0)
                        {
                            AppMain.AMS_AME_RUNTIME_WORK next = (AppMain.AMS_AME_RUNTIME_WORK)runtime.work_head.next;
                            AppMain.AMS_AME_RUNTIME_WORK workTail = (AppMain.AMS_AME_RUNTIME_WORK)runtime.work_tail;
                            while (next != workTail)
                            {
                                AppMain.AMS_AME_RUNTIME_WORK amsAmeRuntimeWork = next;
                                next = (AppMain.AMS_AME_RUNTIME_WORK)next.next;
                                amsAmeRuntimeWork.time += AppMain._am_unit_frame;
                                if ((double)amsAmeRuntimeWork.time > 0.0)
                                {
                                    amsAmeRuntimeWork.time -= AppMain._am_unit_frame;
                                    AppMain.amEffectDisconnectLink((AppMain.AMS_AME_LIST)amsAmeRuntimeWork);
                                    AppMain._amConnectLinkToTail(runtime.active_tail, (AppMain.AMS_AME_LIST)amsAmeRuntimeWork);
                                    --runtime.work_num;
                                    ++runtime.active_num;
                                }
                            }
                        }
                        int num2 = AppMain._am_particle_func[((num1 & (int)byte.MaxValue) << 2) + 1]((object)runtime);
                        AppMain.AMS_AME_RUNTIME_WORK next1 = (AppMain.AMS_AME_RUNTIME_WORK)runtime.active_head.next;
                        AppMain.AMS_AME_LIST activeTail = runtime.active_tail;
                        AppMain.AMS_AME_RUNTIME_WORK work;
                        for (AppMain.AMS_AME_LIST next2 = runtime.active_head.next; next2 != activeTail; next2 = work.next)
                        {
                            work = (AppMain.AMS_AME_RUNTIME_WORK)next2;
                            for (AppMain.AMS_AME_NODE node2 = node1.child; node2 != null; node2 = node2.sibling)
                            {
                                if (AppMain.AMD_AME_IS_FIELD(node2))
                                    AppMain._am_field_func[AppMain.AMD_AME_NODE_TYPE(node2) & (int)byte.MaxValue](runtime.ecb, node2, work);
                            }
                        }
                        continue;
                    default:
                        continue;
                }
            }
        }
        for (AppMain.AMS_AME_ENTRY entry = ecb.entry_head; entry != null; entry = (AppMain.AMS_AME_ENTRY)entry.next)
        {
            if ((entry.runtime.state & 32768) != 0)
            {
                AppMain._amFreeRuntime(entry.runtime);
                AppMain._amDelEntry(ecb, entry);
            }
        }
        if (ecb.entry_num != 0)
            return;
        AppMain.amEffectDelete(ecb);
    }

    private void amEffectUpdateGroup(int attr, int flag)
    {
        uint num1 = (uint)(attr & -65536);
        uint num2 = (uint)(attr & (int)ushort.MaxValue);
        if (num1 == 0U)
            num1 = 4294901760U;
        switch (flag)
        {
            case 0:
                for (AppMain.AMS_AME_ECB next = (AppMain.AMS_AME_ECB)AppMain._am_ecb_head.next; next != AppMain._am_ecb_tail; next = (AppMain.AMS_AME_ECB)next.next)
                {
                    if (((long)next.attribute & (long)num1) != 0L && ((long)next.attribute & (long)num2) != 0L)
                        AppMain.amEffectUpdate(next);
                }
                break;
            case 1:
                for (AppMain.AMS_AME_ECB next = (AppMain.AMS_AME_ECB)AppMain._am_ecb_head.next; next != AppMain._am_ecb_tail; next = (AppMain.AMS_AME_ECB)next.next)
                {
                    if (((long)next.attribute & (long)num1 | (long)next.attribute & (long)num2) == (long)attr)
                        AppMain.amEffectUpdate(next);
                }
                break;
        }
    }

    public static void amEffectDraw(AppMain.AMS_AME_ECB ecb, AppMain.NNS_TEXLIST texlist, uint state)
    {
        ecb.drawState = state;
        if (AppMain._am_enable_draw == 0 || ecb.entry_num <= 0)
            return;
        if ((double)ecb.bounding.radius > 0.0)
        {
            AppMain.NNS_VECTOR4D pPos = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
            if (AppMain._amEffectFrustumCulling(pPos, AppMain._am_view_frustum, ecb.bounding) == 0)
                return;
            AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(pPos);
        }
        for (AppMain.AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AppMain.AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AppMain.AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            runtime.texlist = texlist;
            if (AppMain.AMD_AME_SUPER_CLASS_ID(runtime.node) == 512 && runtime.active_num != (short)0)
            {
                int num = AppMain._am_particle_func[(AppMain.AMD_AME_CLASS_ID(runtime.node) << 2) + 2]((object)runtime);
            }
        }
    }

    private void amEffectDrawGroup(AppMain.NNS_TEXLIST texlist, int attr, uint state, int flag)
    {
        if (AppMain._am_enable_draw == 0)
            return;
        uint num1 = (uint)(attr & -65536);
        uint num2 = (uint)(attr & (int)ushort.MaxValue);
        if (num1 == 0U)
            num1 = 4294901760U;
        switch (flag)
        {
            case 0:
                for (AppMain.AMS_AME_ECB next = (AppMain.AMS_AME_ECB)AppMain._am_ecb_head.next; next != AppMain._am_ecb_tail; next = (AppMain.AMS_AME_ECB)next.next)
                {
                    if (0L != ((long)next.attribute & (long)num1) && 0L != ((long)next.attribute & (long)num2))
                        AppMain.amEffectDraw(next, texlist, state);
                }
                break;
            case 1:
                for (AppMain.AMS_AME_ECB next = (AppMain.AMS_AME_ECB)AppMain._am_ecb_head.next; next != AppMain._am_ecb_tail; next = (AppMain.AMS_AME_ECB)next.next)
                {
                    if (((long)next.attribute & (long)num1 | (long)next.attribute & (long)num2) == (long)attr)
                        AppMain.amEffectDraw(next, texlist, state);
                }
                break;
        }
    }

    public static void amEffectSetTranslate(
      AppMain.AMS_AME_ECB ecb,
      ref AppMain.SNNS_VECTOR4D translate)
    {
        AppMain.amVectorCopy(ecb.translate, ref translate);
        for (AppMain.AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AppMain.AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AppMain.AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((runtime.state & 8192) != 0 && ((int)runtime.node.flag & 67108864) == 0)
            {
                if (runtime.work != null)
                    AppMain.amVectorAdd((AppMain.NNS_VECTOR)runtime.work.position, ((AppMain.AMS_AME_NODE_TR_ROT)runtime.node).translate, ref translate);
                if ((int)runtime.work_num + (int)runtime.active_num != 0)
                {
                    AppMain.AMS_AME_RUNTIME_WORK next1 = (AppMain.AMS_AME_RUNTIME_WORK)runtime.work_head.next;
                    for (AppMain.AMS_AME_RUNTIME_WORK workTail = (AppMain.AMS_AME_RUNTIME_WORK)runtime.work_tail; next1 != workTail; next1 = (AppMain.AMS_AME_RUNTIME_WORK)next1.next)
                        AppMain.amVectorAdd((AppMain.NNS_VECTOR)next1.position, ((AppMain.AMS_AME_NODE_TR_ROT)runtime.node).translate, ref translate);
                    AppMain.AMS_AME_RUNTIME_WORK next2 = (AppMain.AMS_AME_RUNTIME_WORK)runtime.active_head.next;
                    for (AppMain.AMS_AME_RUNTIME_WORK activeTail = (AppMain.AMS_AME_RUNTIME_WORK)runtime.active_tail; next2 != activeTail; next2 = (AppMain.AMS_AME_RUNTIME_WORK)next2.next)
                        AppMain.amVectorAdd((AppMain.NNS_VECTOR)next2.position, ((AppMain.AMS_AME_NODE_TR_ROT)runtime.node).translate, ref translate);
                }
            }
        }
    }

    public static void amEffectSetTranslate(AppMain.AMS_AME_ECB ecb, AppMain.NNS_VECTOR4D translate)
    {
        AppMain.amVectorCopy(ecb.translate, translate);
        for (AppMain.AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AppMain.AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AppMain.AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((runtime.state & 8192) != 0 && ((int)runtime.node.flag & 67108864) == 0)
            {
                if (runtime.work != null)
                    AppMain.amVectorAdd(runtime.work.position, ((AppMain.AMS_AME_NODE_TR_ROT)runtime.node).translate, translate);
                if ((int)runtime.work_num + (int)runtime.active_num != 0)
                {
                    AppMain.AMS_AME_RUNTIME_WORK next1 = (AppMain.AMS_AME_RUNTIME_WORK)runtime.work_head.next;
                    for (AppMain.AMS_AME_RUNTIME_WORK workTail = (AppMain.AMS_AME_RUNTIME_WORK)runtime.work_tail; next1 != workTail; next1 = (AppMain.AMS_AME_RUNTIME_WORK)next1.next)
                        AppMain.amVectorAdd(next1.position, ((AppMain.AMS_AME_NODE_TR_ROT)runtime.node).translate, translate);
                    AppMain.AMS_AME_RUNTIME_WORK next2 = (AppMain.AMS_AME_RUNTIME_WORK)runtime.active_head.next;
                    for (AppMain.AMS_AME_RUNTIME_WORK activeTail = (AppMain.AMS_AME_RUNTIME_WORK)runtime.active_tail; next2 != activeTail; next2 = (AppMain.AMS_AME_RUNTIME_WORK)next2.next)
                        AppMain.amVectorAdd(next2.position, ((AppMain.AMS_AME_NODE_TR_ROT)runtime.node).translate, translate);
                }
            }
        }
    }

    private void amEffectTranslate(AppMain.AMS_AME_ECB ecb, AppMain.NNS_VECTOR4D translate)
    {
        AppMain.amVectorAdd(ecb.translate, translate);
        for (AppMain.AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AppMain.AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AppMain.AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((runtime.state & 8192) != 0 && ((int)runtime.node.flag & 67108864) == 0)
            {
                if (runtime.work != null)
                    AppMain.amVectorAdd(runtime.work.position, translate);
                if ((int)runtime.work_num + (int)runtime.active_num != 0)
                {
                    AppMain.AMS_AME_LIST next1 = runtime.work_head.next;
                    for (AppMain.AMS_AME_LIST workTail = runtime.work_tail; next1 != workTail; next1 = next1.next)
                        AppMain.amVectorAdd(((AppMain.AMS_AME_RUNTIME_WORK)next1).position, translate);
                    AppMain.AMS_AME_LIST next2 = runtime.active_head.next;
                    for (AppMain.AMS_AME_LIST activeTail = runtime.active_tail; next2 != activeTail; next2 = next2.next)
                        AppMain.amVectorAdd(((AppMain.AMS_AME_RUNTIME_WORK)next2).position, translate);
                }
            }
        }
    }

    private void amEffectSetRotate(AppMain.AMS_AME_ECB ecb, int x, int y, int z)
    {
        AppMain.NNS_QUATERNION nnsQuaternion = new AppMain.NNS_QUATERNION();
        AppMain.amQuatEulerToQuatXYZ(ref nnsQuaternion, x, y, z);
        AppMain.amEffectSetRotate(ecb, ref nnsQuaternion);
    }

    public static void amEffectSetRotate(AppMain.AMS_AME_ECB ecb, ref AppMain.NNS_QUATERNION q)
    {
        AppMain.amEffectSetRotate(ecb, ref q, 0);
    }

    public static void amEffectSetRotate(
      AppMain.AMS_AME_ECB ecb,
      ref AppMain.NNS_QUATERNION q,
      int offset)
    {
        ecb.rotate = q;
        for (AppMain.AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AppMain.AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AppMain.AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((runtime.state & 8192) != 0 && ((int)runtime.node.flag & 134217728) == 0)
            {
                if (runtime.work != null)
                {
                    if (offset != 0)
                        AppMain.amQuatMulti(ref runtime.work.rotate[0], ref ((AppMain.AMS_AME_NODE_TR_ROT)runtime.node).rotate, ref q);
                    else
                        runtime.work.rotate[0] = q;
                }
                if ((int)runtime.work_num + (int)runtime.active_num != 0)
                {
                    AppMain.AMS_AME_RUNTIME_WORK next1 = (AppMain.AMS_AME_RUNTIME_WORK)runtime.work_head.next;
                    for (AppMain.AMS_AME_RUNTIME_WORK workTail = (AppMain.AMS_AME_RUNTIME_WORK)runtime.work_tail; next1 != workTail; next1 = (AppMain.AMS_AME_RUNTIME_WORK)next1.next)
                    {
                        if (offset != 0)
                            AppMain.amQuatMulti(ref next1.rotate[0], ref ((AppMain.AMS_AME_NODE_TR_ROT)runtime.node).rotate, ref q);
                        else
                            next1.rotate[0] = q;
                    }
                    AppMain.AMS_AME_RUNTIME_WORK next2 = (AppMain.AMS_AME_RUNTIME_WORK)runtime.active_head.next;
                    for (AppMain.AMS_AME_RUNTIME_WORK activeTail = (AppMain.AMS_AME_RUNTIME_WORK)runtime.active_tail; next2 != activeTail; next2 = (AppMain.AMS_AME_RUNTIME_WORK)next2.next)
                    {
                        if (offset != 0)
                            AppMain.amQuatMulti(ref next2.rotate[0], ref ((AppMain.AMS_AME_NODE_TR_ROT)runtime.node).rotate, ref q);
                        else
                            next2.rotate[0] = q;
                    }
                }
            }
        }
    }

    private void amEffectRotate(AppMain.AMS_AME_ECB ecb, int x, int y, int z)
    {
        AppMain.NNS_QUATERNION nnsQuaternion = new AppMain.NNS_QUATERNION();
        AppMain.amQuatEulerToQuatXYZ(ref nnsQuaternion, x, y, z);
        this.amEffectRotate(ecb, ref nnsQuaternion);
    }

    private void amEffectRotate(AppMain.AMS_AME_ECB ecb, ref AppMain.NNS_QUATERNION q)
    {
        AppMain.amQuatMulti(ref ecb.rotate, ref ecb.rotate, ref q);
        for (AppMain.AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AppMain.AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AppMain.AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((runtime.state & 8192) != 0 && ((int)runtime.node.flag & 134217728) == 0)
            {
                if (runtime.work != null)
                    AppMain.amQuatMulti(ref runtime.work.rotate[0], ref runtime.work.rotate[0], ref q);
                if ((int)runtime.work_num + (int)runtime.active_num != 0)
                {
                    AppMain.AMS_AME_LIST next1 = runtime.work_head.next;
                    for (AppMain.AMS_AME_LIST workTail = runtime.work_tail; next1 != workTail; next1 = next1.next)
                    {
                        AppMain.AMS_AME_RUNTIME_WORK amsAmeRuntimeWork = (AppMain.AMS_AME_RUNTIME_WORK)next1;
                        if (runtime.work != null)
                            AppMain.amQuatMulti(ref amsAmeRuntimeWork.rotate[0], ref runtime.work.rotate[0], ref q);
                    }
                    AppMain.AMS_AME_LIST next2 = runtime.active_head.next;
                    for (AppMain.AMS_AME_LIST activeTail = runtime.active_tail; next2 != activeTail; next2 = next2.next)
                    {
                        AppMain.AMS_AME_RUNTIME_WORK amsAmeRuntimeWork = (AppMain.AMS_AME_RUNTIME_WORK)next2;
                        if (runtime.work != null)
                            AppMain.amQuatMulti(ref amsAmeRuntimeWork.rotate[0], ref runtime.work.rotate[0], ref q);
                    }
                }
            }
        }
    }

    public static void amEffectFreeRuntimeWork(AppMain.AMS_AME_RUNTIME_WORK work)
    {
        AppMain._am_work_ref[AppMain._am_work_free] = work;
        ++AppMain._am_work_free;
        if (AppMain._am_work_free < 1024)
            return;
        AppMain._am_work_free = 0;
    }

    private static int _amEffectSetDrawMode(
      AppMain.AMS_AME_RUNTIME runtime,
      AppMain.AMS_PARAM_DRAW_PRIMITIVE param,
      int NodeBlend)
    {
        AppMain.AMS_AME_NODE node = runtime.node;
        int num = 0;
        param.ablend = num;
        param.aTest = ((int)node.flag & 2) == 0 ? (short)0 : (short)1;
        param.zMask = ((int)node.flag & 33554432) == 0 ? (short)0 : (short)1;
        param.zTest = ((int)node.flag & 16777216) != 0 ? (short)1 : (short)0;
        if (((int)node.flag & 1) != 0)
        {
            num = 1;
            param.ablend = num;
            switch (NodeBlend)
            {
                case 50:
                    param.bldSrc = 770;
                    param.bldDst = 771;
                    param.bldMode = 32774;
                    break;
                case 162:
                    param.bldSrc = 770;
                    param.bldDst = 1;
                    param.bldMode = 32774;
                    break;
                case 674:
                    param.bldSrc = 770;
                    param.bldDst = 1;
                    param.bldMode = 32779;
                    break;
            }
        }
        return num;
    }

    private static uint _amEffectSetMaterial(
      AppMain.AMS_AME_RUNTIME runtime,
      ref int blend,
      int NodeBlend)
    {
        AppMain.AMS_AME_NODE node = runtime.node;
        uint num = 0;
        if (((int)node.flag & 1) != 0)
        {
            num |= 8389632U;
            switch (NodeBlend)
            {
                case 50:
                    blend = 0;
                    break;
                case 162:
                    blend = 1;
                    break;
                case 674:
                    blend = 2;
                    break;
            }
        }
        return num;
    }

    public static void amEffectDelete(AppMain.AMS_AME_ECB ecb)
    {
        ecb.entry_num = -1;
    }
}