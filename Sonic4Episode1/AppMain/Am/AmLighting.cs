public partial class AppMain
{
    private static int _amInitOmni(object p)
    {
        AMS_AME_CREATE_PARAM amsAmeCreateParam = (AMS_AME_CREATE_PARAM)p;
        AMS_AME_NODE_OMNI node = (AMS_AME_NODE_OMNI)amsAmeCreateParam.node;
        AMS_AME_RUNTIME_WORK_OMNI work = (AMS_AME_RUNTIME_WORK_OMNI)amsAmeCreateParam.work;
        work.time = -node.start_time;
        amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        amVectorAdd(work.position, node.translate);
        amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        work.rotate = node.rotate;
        float sizeRate = amsAmeCreateParam.ecb.size_rate;
        work.offset = node.offset * sizeRate;
        work.offset_chaos = node.offset_chaos * sizeRate;
        return 0;
    }

    private static int _amUpdateOmni(object r)
    {
        AMS_AME_RUNTIME amsAmeRuntime1 = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_OMNI node = (AMS_AME_NODE_OMNI)amsAmeRuntime1.node;
        AMS_AME_RUNTIME_WORK_OMNI work = (AMS_AME_RUNTIME_WORK_OMNI)amsAmeRuntime1.work;
        work.time += _am_unit_frame;
        if (work.time <= 0.0)
            return 0;
        if (node.life != -1.0 && work.time >= (double)node.life)
            return 1;
        NNS_VECTOR4D amEffectVel = _amEffect_vel;
        amVectorScale(amEffectVel, work.velocity, _am_unit_time);
        amVectorAdd(work.position, amEffectVel);
        float sizeRate = amsAmeRuntime1.ecb.size_rate;
        work.offset = node.offset * sizeRate;
        work.offset_chaos = node.offset_chaos * sizeRate;
        NNS_VECTOR4D amEffectPosition = _amEffect_position;
        NNS_VECTOR4D amEffectVelocity = _amEffect_velocity;
        NNS_VECTOR4D amEffectDirection = _amEffect_direction;
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
                    AMS_AME_CREATE_PARAM effectCreateParam = _amEffect_create_param;
                    effectCreateParam.Clear();
                    amVectorRandom(amEffectDirection);
                    amVectorScale(amEffectVelocity, amEffectDirection, work.offset + work.offset_chaos * nnRandom());
                    amEffectPosition.Assign(amEffectVelocity);
                    amVectorScale(amEffectVelocity, amEffectDirection, node.speed + node.speed_chaos * nnRandom());
                    effectCreateParam.ecb = amsAmeRuntime1.ecb;
                    effectCreateParam.runtime = amsAmeRuntime2;
                    effectCreateParam.node = amsAmeRuntime2.node;
                    effectCreateParam.parent_position = work.position;
                    effectCreateParam.parent_velocity = work.velocity;
                    effectCreateParam.position = amEffectPosition;
                    effectCreateParam.velocity = amEffectVelocity;
                    switch (AMD_AME_SUPER_CLASS_ID(amsAmeRuntime2.node))
                    {
                        case 256:
                            _amCreateEmitter(effectCreateParam);
                            continue;
                        case 512:
                            _amCreateParticle(effectCreateParam);
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }
        return 0;
    }

    private static int _amDrawOmni(object r)
    {
        return 0;
    }

    private static int _amInitDirectional(object p)
    {
        AMS_AME_CREATE_PARAM amsAmeCreateParam = (AMS_AME_CREATE_PARAM)p;
        AMS_AME_NODE_DIRECTIONAL node = (AMS_AME_NODE_DIRECTIONAL)amsAmeCreateParam.node;
        AMS_AME_RUNTIME_WORK_DIRECTIONAL work = (AMS_AME_RUNTIME_WORK_DIRECTIONAL)amsAmeCreateParam.work;
        work.time = -node.start_time;
        amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        amVectorAdd(work.position, node.translate);
        amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        work.rotate = node.rotate;
        work.spread = node.spread;
        return 0;
    }

    private static int _amUpdateDirectional(object r)
    {
        AMS_AME_RUNTIME amsAmeRuntime1 = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_DIRECTIONAL node = (AMS_AME_NODE_DIRECTIONAL)amsAmeRuntime1.node;
        AMS_AME_RUNTIME_WORK_DIRECTIONAL runtimeWorkDirectional = new AMS_AME_RUNTIME_WORK_DIRECTIONAL(amsAmeRuntime1.work);
        runtimeWorkDirectional.time += _am_unit_frame;
        if (runtimeWorkDirectional.time <= 0.0)
            return 0;
        if (node.life != -1.0 && runtimeWorkDirectional.time >= (double)node.life)
            return 1;
        NNS_VECTOR4D amEffectVel = _amEffect_vel;
        amVectorScale(amEffectVel, runtimeWorkDirectional.velocity, _am_unit_time);
        amVectorAdd(runtimeWorkDirectional.position, amEffectVel);
        runtimeWorkDirectional.spread += node.spread_variation * _am_unit_time;
        NNS_MATRIX amEffectMtx = _amEffect_mtx;
        nnMakeUnitMatrix(amEffectMtx);
        amMatrixPush(amEffectMtx);
        NNS_QUATERNION rotate = runtimeWorkDirectional.rotate;
        amQuatToMatrix(null, ref rotate, null);
        runtimeWorkDirectional.rotate = rotate;
        AMS_AME_LIST next = amsAmeRuntime1.child_head.next;
        AMS_AME_LIST childTail = amsAmeRuntime1.child_tail;
        NNS_VECTOR4D amEffectPosition = _amEffect_position;
        NNS_VECTOR4D amEffectVelocity = _amEffect_velocity;
        NNS_VECTOR4D amEffectDirection = _amEffect_direction;
        for (; next != childTail; next = next.next)
        {
            AMS_AME_RUNTIME amsAmeRuntime2 = (AMS_AME_RUNTIME)next;
            amsAmeRuntime2.amount += node.frequency * _am_unit_frame;
            while (amsAmeRuntime2.amount >= 1.0)
            {
                --amsAmeRuntime2.amount;
                ++amsAmeRuntime2.count;
                if (node.max_count != -1.0 && amsAmeRuntime2.work_num + amsAmeRuntime2.active_num < (double)node.max_count)
                {
                    AMS_AME_CREATE_PARAM effectCreateParam = _amEffect_create_param;
                    effectCreateParam.Clear();
                    amEffectRandomConeVectorDeg(amEffectDirection, runtimeWorkDirectional.spread);
                    amMatrixCalcPoint(amEffectDirection, amEffectDirection);
                    amVectorScale(amEffectVelocity, amEffectDirection, node.offset + node.offset_chaos * nnRandom());
                    amEffectPosition.Assign(amEffectVelocity);
                    amVectorScale(amEffectVelocity, amEffectDirection, node.speed + node.speed_chaos * nnRandom());
                    effectCreateParam.ecb = amsAmeRuntime1.ecb;
                    effectCreateParam.runtime = amsAmeRuntime2;
                    effectCreateParam.node = amsAmeRuntime2.node;
                    effectCreateParam.parent_position = runtimeWorkDirectional.position;
                    effectCreateParam.parent_velocity = runtimeWorkDirectional.velocity;
                    effectCreateParam.position = amEffectPosition;
                    effectCreateParam.velocity = amEffectVelocity;
                    switch (AMD_AME_SUPER_CLASS_ID(amsAmeRuntime2.node))
                    {
                        case 256:
                            _amCreateEmitter(effectCreateParam);
                            continue;
                        case 512:
                            _amCreateParticle(effectCreateParam);
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }
        amMatrixPop();
        return 0;
    }

    private static int _amDrawDirectional(object r)
    {
        mppAssertNotImpl();
        return 0;
    }

}