using System;
using Microsoft.Xna.Framework;

public partial class AppMain
{
    public static int AMM_BLEND(int equ, int a, int b)
    {
        return equ << 8 | b << 4 | a;
    }

    public static int AMD_AME_NODE_TYPE(AMS_AME_NODE node)
    {
        return node.type;
    }

    public static int AMD_AME_SUPER_CLASS_ID(AMS_AME_NODE node)
    {
        return (ushort)node.type & 65280;
    }

    public static int AMD_AME_CLASS_ID(AMS_AME_NODE node)
    {
        return (ushort)node.type & byte.MaxValue;
    }

    public static bool AMD_AME_IS_EMITTER(AMS_AME_NODE node)
    {
        return AMD_AME_SUPER_CLASS_ID(node) == 256;
    }

    public static bool AMD_AME_IS_PARTICLE(AMS_AME_NODE node)
    {
        return AMD_AME_SUPER_CLASS_ID(node) == 512;
    }

    public static bool AMD_AME_IS_FIELD(AMS_AME_NODE node)
    {
        return AMD_AME_SUPER_CLASS_ID(node) == 768;
    }

    public static int amEffectIsDelete(AMS_AME_ECB ecb)
    {
        return ecb.entry_num >= 0 ? 0 : 1;
    }

    private AMS_AME_ECB amEffectCreate(AMS_AME_NODE node)
    {
        return this.amEffectCreate(node, 0, 0);
    }

    private AMS_AME_ECB amEffectCreate(AMS_AME_HEADER header)
    {
        return _amEffectCreate(header, 0, 0);
    }

    private void amEffectSetTransparency(AMS_AME_ECB ecb, float t)
    {
        ecb.transparency = (int)(t * 256.0);
    }

    public static void amEffectSetSizeRate(AMS_AME_ECB ecb, float t)
    {
        ecb.size_rate = t;
    }

    public static void amEffectLerpColor(
      out AMS_RGBA8888 pCO,
      ref AMS_RGBA8888 pC1,
      ref AMS_RGBA8888 pC2,
      float rate)
    {
        int num = (int)(rate * 256.0);
        pCO = new AMS_RGBA8888();
        pCO.r = (byte)((pC1.r << 8) + (pC2.r - pC1.r) * num >> 8);
        pCO.g = (byte)((pC1.g << 8) + (pC2.g - pC1.g) * num >> 8);
        pCO.b = (byte)((pC1.b << 8) + (pC2.b - pC1.b) * num >> 8);
        pCO.a = (byte)((pC1.a << 8) + (pC2.a - pC1.a) * num >> 8);
    }

    public static void amEffectDisconnectLink(AMS_AME_LIST list)
    {
        list.prev.next = list.next;
        list.next.prev = list.prev;
    }

    private void amEffectRandomConeVector(ref Vector4 pOut, float s)
    {
        mppAssertNotImpl();
    }

    public static void amEffectRandomConeVectorDeg(NNS_VECTOR4D pOut, float s)
    {
        float pCs = nnCos(NNM_DEGtoA32(s));
        float y = nnRandom() * (1f - pCs) + pCs;
        float num1 = (float)Math.Sqrt(1.0 - y * (double)y);
        amSinCos(nnRandom() * 6.283185f, out s, out pCs);
        amVectorSet(pOut, num1 * pCs, y, num1 * s);
        double num2 = amVectorUnit(pOut);
    }

    private void _amConnectLinkToHead(AMS_AME_LIST head, AMS_AME_LIST list)
    {
        list.next = head.next;
        list.prev = head.next.prev;
        head.next.prev = list;
        head.next = list;
    }

    public static void _amConnectLinkToTail(AMS_AME_LIST tail, AMS_AME_LIST list)
    {
        list.prev = tail.prev;
        list.next = tail.prev.next;
        tail.prev.next = list;
        tail.prev = list;
    }

    public static void amEffectSystemInit()
    {
        NNS_RGBA diffuse = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_RGB ambient = new NNS_RGB(1f, 1f, 1f);
        nnSetPrimitive3DMaterial(ref diffuse, ref ambient, 1f);
        _am_enable_draw = 1;
        _am_unit_frame = 1f;
        _am_unit_time = 0.01666667f;
        _am_ecb_alloc = 0;
        _am_ecb_free = 0;
        _am_ecb_head.Clear();
        _am_ecb_tail.Clear();
        _am_ecb_head.next = _am_ecb_tail;
        _am_ecb_tail.prev = _am_ecb_head;
        _am_ecb_buf = New<AMS_AME_ECB>(128);
        _am_ecb_ref = new AMS_AME_ECB[128];
        for (int index = 0; index < 128; ++index)
            _am_ecb_ref[index] = _am_ecb_buf[index];
        _am_entry_alloc = 0;
        _am_entry_free = 0;
        _am_entry_buf = new AMS_AME_ENTRY[512];
        _am_entry_ref = new AMS_AME_ENTRY[512];
        for (int index = 0; index < 512; ++index)
        {
            _am_entry_buf[index] = new AMS_AME_ENTRY();
            _am_entry_ref[index] = _am_entry_buf[index];
        }
        _am_runtime_alloc = 0;
        _am_runtime_free = 0;
        _am_runtime_buf = new AMS_AME_RUNTIME[512];
        _am_runtime_ref = new AMS_AME_RUNTIME[512];
        for (int index = 0; index < 512; ++index)
        {
            _am_runtime_buf[index] = new AMS_AME_RUNTIME();
            _am_runtime_ref[index] = _am_runtime_buf[index];
        }
        _am_work_alloc = 0;
        _am_work_free = 0;
        _am_work_buf = new AMS_AME_RUNTIME_WORK[1024];
        _am_work_ref = new AMS_AME_RUNTIME_WORK[1024];
        for (int index = 0; index < 1024; ++index)
        {
            _am_work_buf[index] = new AMS_AME_RUNTIME_WORK();
            _am_work_ref[index] = _am_work_buf[index];
        }
    }

    private void amEffectSystemReset()
    {
        mppAssertNotImpl();
        amEffectSystemInit();
    }

    public static void amEffectExecute()
    {
        for (AMS_AME_ECB next = (AMS_AME_ECB)_am_ecb_head.next; next != _am_ecb_tail; next = (AMS_AME_ECB)next.next)
        {
            if (next.entry_num < 0)
                _amEffectFinalize(next);
        }
    }

    private void amEffectRegistCustomFunc(int classId, AMS_AME_CUSTOM_PARAM pParam)
    {
        mppAssertNotImpl();
        switch (classId & 65280)
        {
            case 256:
                int index1 = (classId & byte.MaxValue) << 2;
                _am_emitter_func[index1] = pParam.pInitFunc;
                _am_emitter_func[index1 + 1] = pParam.pUpdateFunc;
                _am_emitter_func[index1 + 2] = pParam.pDrawFunc;
                break;
            case 512:
                int index2 = (classId & byte.MaxValue) << 2;
                _am_particle_func[index2] = pParam.pInitFunc;
                _am_particle_func[index2 + 1] = pParam.pUpdateFunc;
                _am_particle_func[index2 + 2] = pParam.pDrawFunc;
                break;
            case 768:
                int index3 = classId & byte.MaxValue;
                _am_field_func[index3] = pParam.pFieldFunc;
                break;
        }
    }

    private void amEffectUnregistCustomFunc(int classId)
    {
        mppAssertNotImpl();
        switch (classId & 65280)
        {
            case 256:
                int index1 = (classId & byte.MaxValue) << 2;
                _am_emitter_func[index1] = null;
                _am_emitter_func[index1 + 1] = null;
                _am_emitter_func[index1 + 2] = null;
                break;
            case 512:
                int index2 = (classId & byte.MaxValue) << 2;
                _am_particle_func[index2] = null;
                _am_particle_func[index2 + 1] = null;
                _am_particle_func[index2 + 2] = null;
                break;
            case 768:
                int index3 = classId & byte.MaxValue;
                _am_field_func[index3] = null;
                break;
        }
    }

    public static void amEffectSetObject(
      AMS_AME_ECB ecb,
      NNS_OBJECT object_,
      int state)
    {
        ecb.pObj = object_;
        ecb.drawObjState = (uint)state;
    }

    private static void amEffectSetWorldViewMatrix(NNS_MATRIX mtx)
    {
        nnCopyMatrix(_am_ef_worldViewMtx, mtx);
    }

    public static void amEffectSetCameraPos(ref SNNS_VECTOR pos)
    {
        nnCopyVector(_am_ef_camPos, ref pos);
    }

    public static void amEffectSetCameraPos(NNS_VECTOR pos)
    {
        nnCopyVector(_am_ef_camPos, pos);
    }

    private void amEffectEnableDraw(int flag)
    {
        mppAssertNotImpl();
        _am_enable_draw = flag;
    }

    public static void amEffectSetUnitTime(float speed, int frame_rate)
    {
        _am_unit_frame = speed;
        _am_unit_time = speed / frame_rate;
    }

    public static float amEffectGetUnitFrame()
    {
        return _am_unit_frame;
    }

    private AMS_AME_NODE amEffectSearchNode(AMS_AME_NODE node, int id)
    {
        mppAssertNotImpl();
        AMS_AME_NODE amsAmeNode = null;
        if (node.id == id)
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

    private AMS_AME_NODE amEffectSearchNode(AME_HEADER header, int id)
    {
        return this.amEffectSearchNode(header.node[0], id);
    }

    private AMS_AME_ECB amEffectCreate(
      AMS_AME_NODE node,
      int attribute,
      int priority)
    {
        mppAssertNotImpl();
        return null;
    }

    private static AMS_AME_ECB _amEffectCreate(
      AMS_AME_HEADER header,
      int attribute,
      int priority)
    {
        AMS_AME_ECB amsAmeEcb = _am_ecb_ref[_am_ecb_alloc];
        ++_am_ecb_alloc;
        if (_am_ecb_alloc >= 128)
            _am_ecb_alloc = 0;
        amsAmeEcb.Clear();
        amsAmeEcb.attribute = attribute;
        amsAmeEcb.priority = priority;
        amsAmeEcb.transparency = 256;
        amsAmeEcb.size_rate = 1f;
        amVectorInit(amsAmeEcb.translate);
        amQuatInit(ref amsAmeEcb.rotate);
        amsAmeEcb.bounding.Assign(header.bounding);
        AMS_AME_ECB next = (AMS_AME_ECB)_am_ecb_head.next;
        while (next != _am_ecb_tail && next.priority <= priority)
            next = (AMS_AME_ECB)next.next;
        next.prev.next = amsAmeEcb;
        amsAmeEcb.prev = next.prev;
        next.prev = amsAmeEcb;
        amsAmeEcb.next = next;
        AMS_AME_CREATE_PARAM amsAmeCreateParam = new AMS_AME_CREATE_PARAM();
        NNS_VECTOR4D amEffectCreateVec = _amEffectCreate_vec;
        AMS_AME_NODE sibling = header.node[0];
        amVectorInit(amEffectCreateVec);
        for (; sibling != null; sibling = sibling.sibling)
        {
            amsAmeCreateParam.ecb = amsAmeEcb;
            amsAmeCreateParam.runtime = null;
            amsAmeCreateParam.node = sibling;
            amsAmeCreateParam.position = amEffectCreateVec;
            amsAmeCreateParam.velocity = amEffectCreateVec;
            amsAmeCreateParam.parent_position = amEffectCreateVec;
            amsAmeCreateParam.parent_velocity = amEffectCreateVec;
            switch (AMD_AME_SUPER_CLASS_ID(sibling))
            {
                case 256:
                    _amCreateRuntimeEmitter(amsAmeCreateParam).state |= 8192;
                    break;
                case 512:
                    _amCreateRuntimeParticle(amsAmeCreateParam).state |= 8192;
                    break;
            }
        }
        amsAmeEcb.skip_update = 1;
        return amsAmeEcb;
    }

    private void amEffectDeleteGroup(int attr, int flag)
    {
        mppAssertNotImpl();
        uint num1 = (uint)(attr & -65536);
        uint num2 = (uint)(attr & ushort.MaxValue);
        if (num1 == 0U)
            num1 = 4294901760U;
        switch (flag)
        {
            case 0:
                for (AMS_AME_ECB next = (AMS_AME_ECB)_am_ecb_head.next; next != _am_ecb_tail; next = (AMS_AME_ECB)next.next)
                {
                    if ((next.attribute & num1) != 0L && (next.attribute & num2) != 0L)
                        amEffectDelete(next);
                }
                break;
            case 1:
                for (AMS_AME_ECB next = (AMS_AME_ECB)_am_ecb_head.next; next != _am_ecb_tail; next = (AMS_AME_ECB)next.next)
                {
                    if ((next.attribute & num1 | next.attribute & num2) == attr)
                        amEffectDelete(next);
                }
                break;
        }
    }

    public static void amEffectKill(AMS_AME_ECB ecb)
    {
        for (AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((AMD_AME_NODE_TYPE(runtime.node) & 65280) == 256 && (runtime.state & 32768) == 0)
            {
                if (runtime.spawn_runtime != null)
                    runtime.spawn_runtime.state |= 16384;
                runtime.state |= 32768;
                AMS_AME_LIST next = runtime.child_head.next;
                for (AMS_AME_LIST childTail = runtime.child_tail; next != childTail; next = next.next)
                    ((AMS_AME_RUNTIME)next).state |= 16384;
                if (runtime.parent_runtime != null)
                {
                    amEffectDisconnectLink(runtime);
                    --runtime.parent_runtime.work_num;
                }
            }
        }
    }

    private void amEffectKillGroup(int attr, int flag)
    {
        uint num1 = (uint)(attr & -65536);
        uint num2 = (uint)(attr & ushort.MaxValue);
        if (num1 == 0U)
            num1 = 4294901760U;
        switch (flag)
        {
            case 0:
                for (AMS_AME_ECB next = (AMS_AME_ECB)_am_ecb_head.next; next != _am_ecb_tail; next = (AMS_AME_ECB)next.next)
                {
                    if ((next.attribute & num1) != 0L && (next.attribute & num2) != 0L)
                        amEffectKill(next);
                }
                break;
            case 1:
                for (AMS_AME_ECB next = (AMS_AME_ECB)_am_ecb_head.next; next != _am_ecb_tail; next = (AMS_AME_ECB)next.next)
                {
                    if ((next.attribute & num1) != 0L && (next.attribute & num2) == num2)
                        amEffectKill(next);
                }
                break;
        }
    }

    public static void amEffectUpdate(AMS_AME_ECB ecb)
    {
        if (ecb.entry_num <= 0)
            return;
        if (ecb.skip_update != 0)
            ecb.skip_update = 0;
        for (AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            AMS_AME_NODE node1 = runtime.node;
            int num1 = AMD_AME_NODE_TYPE(node1);
            if ((runtime.state & 16384) != 0 && runtime.work_num + runtime.active_num == 0)
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
                            AmeDelegateFunc ameDelegateFunc = _am_emitter_func[((num1 & byte.MaxValue) << 2) + 1];
                            if (runtime.work != null)
                            {
                                if (ameDelegateFunc(runtime) != 0)
                                {
                                    runtime.state |= 32768;
                                    AMS_AME_LIST next = runtime.child_head.next;
                                    for (AMS_AME_LIST childTail = runtime.child_tail; next != childTail; next = next.next)
                                        ((AMS_AME_RUNTIME)next).state |= 16384;
                                    if (runtime.parent_runtime != null)
                                    {
                                        amEffectDisconnectLink(runtime);
                                        --runtime.parent_runtime.work_num;
                                        continue;
                                    }
                                    continue;
                                }
                                for (AMS_AME_NODE node2 = node1.child; node2 != null; node2 = node2.sibling)
                                {
                                    if (AMD_AME_IS_FIELD(node2))
                                        _am_field_func[AMD_AME_NODE_TYPE(node2) & byte.MaxValue](runtime.ecb, node2, runtime.work);
                                }
                                continue;
                            }
                            continue;
                        }
                        continue;
                    case 512:
                        if (runtime.work_num != 0)
                        {
                            AMS_AME_RUNTIME_WORK next = (AMS_AME_RUNTIME_WORK)runtime.work_head.next;
                            AMS_AME_RUNTIME_WORK workTail = (AMS_AME_RUNTIME_WORK)runtime.work_tail;
                            while (next != workTail)
                            {
                                AMS_AME_RUNTIME_WORK amsAmeRuntimeWork = next;
                                next = (AMS_AME_RUNTIME_WORK)next.next;
                                amsAmeRuntimeWork.time += _am_unit_frame;
                                if (amsAmeRuntimeWork.time > 0.0)
                                {
                                    amsAmeRuntimeWork.time -= _am_unit_frame;
                                    amEffectDisconnectLink(amsAmeRuntimeWork);
                                    _amConnectLinkToTail(runtime.active_tail, amsAmeRuntimeWork);
                                    --runtime.work_num;
                                    ++runtime.active_num;
                                }
                            }
                        }
                        int num2 = _am_particle_func[((num1 & byte.MaxValue) << 2) + 1](runtime);
                        AMS_AME_RUNTIME_WORK next1 = (AMS_AME_RUNTIME_WORK)runtime.active_head.next;
                        AMS_AME_LIST activeTail = runtime.active_tail;
                        AMS_AME_RUNTIME_WORK work;
                        for (AMS_AME_LIST next2 = runtime.active_head.next; next2 != activeTail; next2 = work.next)
                        {
                            work = (AMS_AME_RUNTIME_WORK)next2;
                            for (AMS_AME_NODE node2 = node1.child; node2 != null; node2 = node2.sibling)
                            {
                                if (AMD_AME_IS_FIELD(node2))
                                    _am_field_func[AMD_AME_NODE_TYPE(node2) & byte.MaxValue](runtime.ecb, node2, work);
                            }
                        }
                        continue;
                    default:
                        continue;
                }
            }
        }
        for (AMS_AME_ENTRY entry = ecb.entry_head; entry != null; entry = (AMS_AME_ENTRY)entry.next)
        {
            if ((entry.runtime.state & 32768) != 0)
            {
                _amFreeRuntime(entry.runtime);
                _amDelEntry(ecb, entry);
            }
        }
        if (ecb.entry_num != 0)
            return;
        amEffectDelete(ecb);
    }

    private void amEffectUpdateGroup(int attr, int flag)
    {
        uint num1 = (uint)(attr & -65536);
        uint num2 = (uint)(attr & ushort.MaxValue);
        if (num1 == 0U)
            num1 = 4294901760U;
        switch (flag)
        {
            case 0:
                for (AMS_AME_ECB next = (AMS_AME_ECB)_am_ecb_head.next; next != _am_ecb_tail; next = (AMS_AME_ECB)next.next)
                {
                    if ((next.attribute & num1) != 0L && (next.attribute & num2) != 0L)
                        amEffectUpdate(next);
                }
                break;
            case 1:
                for (AMS_AME_ECB next = (AMS_AME_ECB)_am_ecb_head.next; next != _am_ecb_tail; next = (AMS_AME_ECB)next.next)
                {
                    if ((next.attribute & num1 | next.attribute & num2) == attr)
                        amEffectUpdate(next);
                }
                break;
        }
    }

    public static void amEffectDraw(AMS_AME_ECB ecb, NNS_TEXLIST texlist, uint state)
    {
        ecb.drawState = state;
        if (_am_enable_draw == 0 || ecb.entry_num <= 0)
            return;
        if (ecb.bounding.radius > 0.0)
        {
            NNS_VECTOR4D pPos = GlobalPool<NNS_VECTOR4D>.Alloc();
            if (_amEffectFrustumCulling(pPos, _am_view_frustum, ecb.bounding) == 0)
                return;
            GlobalPool<NNS_VECTOR4D>.Release(pPos);
        }
        for (AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            runtime.texlist = texlist;
            if (AMD_AME_SUPER_CLASS_ID(runtime.node) == 512 && runtime.active_num != 0)
            {
                int num = _am_particle_func[(AMD_AME_CLASS_ID(runtime.node) << 2) + 2](runtime);
            }
        }
    }

    private void amEffectDrawGroup(NNS_TEXLIST texlist, int attr, uint state, int flag)
    {
        if (_am_enable_draw == 0)
            return;
        uint num1 = (uint)(attr & -65536);
        uint num2 = (uint)(attr & ushort.MaxValue);
        if (num1 == 0U)
            num1 = 4294901760U;
        switch (flag)
        {
            case 0:
                for (AMS_AME_ECB next = (AMS_AME_ECB)_am_ecb_head.next; next != _am_ecb_tail; next = (AMS_AME_ECB)next.next)
                {
                    if (0L != (next.attribute & num1) && 0L != (next.attribute & num2))
                        amEffectDraw(next, texlist, state);
                }
                break;
            case 1:
                for (AMS_AME_ECB next = (AMS_AME_ECB)_am_ecb_head.next; next != _am_ecb_tail; next = (AMS_AME_ECB)next.next)
                {
                    if ((next.attribute & num1 | next.attribute & num2) == attr)
                        amEffectDraw(next, texlist, state);
                }
                break;
        }
    }

    public static void amEffectSetTranslate(
      AMS_AME_ECB ecb,
      ref SNNS_VECTOR4D translate)
    {
        amVectorCopy(ecb.translate, ref translate);
        for (AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((runtime.state & 8192) != 0 && ((int)runtime.node.flag & 67108864) == 0)
            {
                if (runtime.work != null)
                    amVectorAdd(runtime.work.position, ((AMS_AME_NODE_TR_ROT)runtime.node).translate, ref translate);
                if (runtime.work_num + runtime.active_num != 0)
                {
                    AMS_AME_RUNTIME_WORK next1 = (AMS_AME_RUNTIME_WORK)runtime.work_head.next;
                    for (AMS_AME_RUNTIME_WORK workTail = (AMS_AME_RUNTIME_WORK)runtime.work_tail; next1 != workTail; next1 = (AMS_AME_RUNTIME_WORK)next1.next)
                        amVectorAdd(next1.position, ((AMS_AME_NODE_TR_ROT)runtime.node).translate, ref translate);
                    AMS_AME_RUNTIME_WORK next2 = (AMS_AME_RUNTIME_WORK)runtime.active_head.next;
                    for (AMS_AME_RUNTIME_WORK activeTail = (AMS_AME_RUNTIME_WORK)runtime.active_tail; next2 != activeTail; next2 = (AMS_AME_RUNTIME_WORK)next2.next)
                        amVectorAdd(next2.position, ((AMS_AME_NODE_TR_ROT)runtime.node).translate, ref translate);
                }
            }
        }
    }

    public static void amEffectSetTranslate(AMS_AME_ECB ecb, NNS_VECTOR4D translate)
    {
        amVectorCopy(ecb.translate, translate);
        for (AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((runtime.state & 8192) != 0 && ((int)runtime.node.flag & 67108864) == 0)
            {
                if (runtime.work != null)
                    amVectorAdd(runtime.work.position, ((AMS_AME_NODE_TR_ROT)runtime.node).translate, translate);
                if (runtime.work_num + runtime.active_num != 0)
                {
                    AMS_AME_RUNTIME_WORK next1 = (AMS_AME_RUNTIME_WORK)runtime.work_head.next;
                    for (AMS_AME_RUNTIME_WORK workTail = (AMS_AME_RUNTIME_WORK)runtime.work_tail; next1 != workTail; next1 = (AMS_AME_RUNTIME_WORK)next1.next)
                        amVectorAdd(next1.position, ((AMS_AME_NODE_TR_ROT)runtime.node).translate, translate);
                    AMS_AME_RUNTIME_WORK next2 = (AMS_AME_RUNTIME_WORK)runtime.active_head.next;
                    for (AMS_AME_RUNTIME_WORK activeTail = (AMS_AME_RUNTIME_WORK)runtime.active_tail; next2 != activeTail; next2 = (AMS_AME_RUNTIME_WORK)next2.next)
                        amVectorAdd(next2.position, ((AMS_AME_NODE_TR_ROT)runtime.node).translate, translate);
                }
            }
        }
    }

    private void amEffectTranslate(AMS_AME_ECB ecb, NNS_VECTOR4D translate)
    {
        amVectorAdd(ecb.translate, translate);
        for (AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((runtime.state & 8192) != 0 && ((int)runtime.node.flag & 67108864) == 0)
            {
                if (runtime.work != null)
                    amVectorAdd(runtime.work.position, translate);
                if (runtime.work_num + runtime.active_num != 0)
                {
                    AMS_AME_LIST next1 = runtime.work_head.next;
                    for (AMS_AME_LIST workTail = runtime.work_tail; next1 != workTail; next1 = next1.next)
                        amVectorAdd(((AMS_AME_RUNTIME_WORK)next1).position, translate);
                    AMS_AME_LIST next2 = runtime.active_head.next;
                    for (AMS_AME_LIST activeTail = runtime.active_tail; next2 != activeTail; next2 = next2.next)
                        amVectorAdd(((AMS_AME_RUNTIME_WORK)next2).position, translate);
                }
            }
        }
    }

    private void amEffectSetRotate(AMS_AME_ECB ecb, int x, int y, int z)
    {
        NNS_QUATERNION nnsQuaternion = new NNS_QUATERNION();
        amQuatEulerToQuatXYZ(ref nnsQuaternion, x, y, z);
        amEffectSetRotate(ecb, ref nnsQuaternion);
    }

    public static void amEffectSetRotate(AMS_AME_ECB ecb, ref NNS_QUATERNION q)
    {
        amEffectSetRotate(ecb, ref q, 0);
    }

    public static void amEffectSetRotate(
      AMS_AME_ECB ecb,
      ref NNS_QUATERNION q,
      int offset)
    {
        ecb.rotate = q;
        for (AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((runtime.state & 8192) != 0 && ((int)runtime.node.flag & 134217728) == 0)
            {
                if (runtime.work != null)
                {
                    if (offset != 0)
                        amQuatMulti(ref runtime.work.rotate[0], ref ((AMS_AME_NODE_TR_ROT)runtime.node).rotate, ref q);
                    else
                        runtime.work.rotate[0] = q;
                }
                if (runtime.work_num + runtime.active_num != 0)
                {
                    AMS_AME_RUNTIME_WORK next1 = (AMS_AME_RUNTIME_WORK)runtime.work_head.next;
                    for (AMS_AME_RUNTIME_WORK workTail = (AMS_AME_RUNTIME_WORK)runtime.work_tail; next1 != workTail; next1 = (AMS_AME_RUNTIME_WORK)next1.next)
                    {
                        if (offset != 0)
                            amQuatMulti(ref next1.rotate[0], ref ((AMS_AME_NODE_TR_ROT)runtime.node).rotate, ref q);
                        else
                            next1.rotate[0] = q;
                    }
                    AMS_AME_RUNTIME_WORK next2 = (AMS_AME_RUNTIME_WORK)runtime.active_head.next;
                    for (AMS_AME_RUNTIME_WORK activeTail = (AMS_AME_RUNTIME_WORK)runtime.active_tail; next2 != activeTail; next2 = (AMS_AME_RUNTIME_WORK)next2.next)
                    {
                        if (offset != 0)
                            amQuatMulti(ref next2.rotate[0], ref ((AMS_AME_NODE_TR_ROT)runtime.node).rotate, ref q);
                        else
                            next2.rotate[0] = q;
                    }
                }
            }
        }
    }

    private void amEffectRotate(AMS_AME_ECB ecb, int x, int y, int z)
    {
        NNS_QUATERNION nnsQuaternion = new NNS_QUATERNION();
        amQuatEulerToQuatXYZ(ref nnsQuaternion, x, y, z);
        this.amEffectRotate(ecb, ref nnsQuaternion);
    }

    private void amEffectRotate(AMS_AME_ECB ecb, ref NNS_QUATERNION q)
    {
        amQuatMulti(ref ecb.rotate, ref ecb.rotate, ref q);
        for (AMS_AME_ENTRY amsAmeEntry = ecb.entry_head; amsAmeEntry != null; amsAmeEntry = (AMS_AME_ENTRY)amsAmeEntry.next)
        {
            AMS_AME_RUNTIME runtime = amsAmeEntry.runtime;
            if ((runtime.state & 8192) != 0 && ((int)runtime.node.flag & 134217728) == 0)
            {
                if (runtime.work != null)
                    amQuatMulti(ref runtime.work.rotate[0], ref runtime.work.rotate[0], ref q);
                if (runtime.work_num + runtime.active_num != 0)
                {
                    AMS_AME_LIST next1 = runtime.work_head.next;
                    for (AMS_AME_LIST workTail = runtime.work_tail; next1 != workTail; next1 = next1.next)
                    {
                        AMS_AME_RUNTIME_WORK amsAmeRuntimeWork = (AMS_AME_RUNTIME_WORK)next1;
                        if (runtime.work != null)
                            amQuatMulti(ref amsAmeRuntimeWork.rotate[0], ref runtime.work.rotate[0], ref q);
                    }
                    AMS_AME_LIST next2 = runtime.active_head.next;
                    for (AMS_AME_LIST activeTail = runtime.active_tail; next2 != activeTail; next2 = next2.next)
                    {
                        AMS_AME_RUNTIME_WORK amsAmeRuntimeWork = (AMS_AME_RUNTIME_WORK)next2;
                        if (runtime.work != null)
                            amQuatMulti(ref amsAmeRuntimeWork.rotate[0], ref runtime.work.rotate[0], ref q);
                    }
                }
            }
        }
    }

    public static void amEffectFreeRuntimeWork(AMS_AME_RUNTIME_WORK work)
    {
        _am_work_ref[_am_work_free] = work;
        ++_am_work_free;
        if (_am_work_free < 1024)
            return;
        _am_work_free = 0;
    }

    private static int _amEffectSetDrawMode(
      AMS_AME_RUNTIME runtime,
      AMS_PARAM_DRAW_PRIMITIVE param,
      int NodeBlend)
    {
        AMS_AME_NODE node = runtime.node;
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
      AMS_AME_RUNTIME runtime,
      ref int blend,
      int NodeBlend)
    {
        AMS_AME_NODE node = runtime.node;
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

    public static void amEffectDelete(AMS_AME_ECB ecb)
    {
        ecb.entry_num = -1;
    }
}