public partial class AppMain
{
    private static int _amInitSurface(object p)
    {
        AMS_AME_CREATE_PARAM amsAmeCreateParam = (AMS_AME_CREATE_PARAM)p;
        AMS_AME_NODE_SURFACE node = (AMS_AME_NODE_SURFACE)amsAmeCreateParam.node;
        AMS_AME_RUNTIME_WORK_SURFACE work = (AMS_AME_RUNTIME_WORK_SURFACE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        amVectorAdd(work.position, node.translate);
        amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        work.rotate = node.rotate;
        float sizeRate = amsAmeCreateParam.ecb.size_rate;
        work.width = node.width * sizeRate;
        work.height = node.height * sizeRate;
        work.offset = node.offset * sizeRate;
        work.offset_chaos = node.offset_chaos * sizeRate;
        return 0;
    }

    private static int _amUpdateSurface(object r)
    {
        AMS_AME_RUNTIME amsAmeRuntime1 = (AMS_AME_RUNTIME)r;
        AMS_AME_NODE_SURFACE node = (AMS_AME_NODE_SURFACE)amsAmeRuntime1.node;
        AMS_AME_RUNTIME_WORK_SURFACE work = (AMS_AME_RUNTIME_WORK_SURFACE)amsAmeRuntime1.work;
        work.time += _am_unit_frame;
        if (work.time <= 0.0)
            return 0;
        if (node.life != -1.0 && work.time >= (double)node.life)
            return 1;
        NNS_VECTOR4D amEffectVel = _amEffect_vel;
        amVectorScale(amEffectVel, work.velocity, _am_unit_time);
        amVectorAdd(work.position, amEffectVel);
        float sizeRate = amsAmeRuntime1.ecb.size_rate;
        if (node.width_variation != 0.0 || node.height_variation != 0.0)
        {
            work.width += node.width_variation * _am_unit_time;
            work.height += node.height_variation * _am_unit_time;
        }
        else
        {
            work.width = node.width * sizeRate;
            work.height = node.height * sizeRate;
        }
        work.offset = node.offset * sizeRate;
        work.offset_chaos = node.offset_chaos * sizeRate;
        NNS_MATRIX amEffectMtx = _amEffect_mtx;
        nnMakeUnitMatrix(amEffectMtx);
        amMatrixPush(amEffectMtx);
        NNS_QUATERNION rotate = work.rotate;
        amQuatToMatrix(null, ref rotate, null);
        work.rotate = rotate;
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
                    amVectorSet(amEffectDirection, 0.0f, 1f, 0.0f);
                    float num1 = (float)(nnRandom() / 2.0 + nnRandom() * 0.5);
                    float x = (float)(work.width * (double)num1 - work.width * 0.5);
                    float num2 = (float)(nnRandom() / 2.0 + nnRandom() * 0.5);
                    float z = (float)(work.height * (double)num2 - work.height * 0.5);
                    if (((int)node.flag & 1) != 0)
                    {
                        float num3 = nnRandom();
                        bool flag = false;
                        if (nnRandom() > 0.5)
                            flag = true;
                        if (num3 > 0.5)
                        {
                            float num4 = work.width * 0.5f;
                            x = num4 <= 0.0 || !flag ? num4 : -num4;
                        }
                        else
                        {
                            float num4 = work.height * 0.5f;
                            z = num4 <= 0.0 || !flag ? num4 : -num4;
                        }
                    }
                    amVectorSet(amEffectPosition, x, 0.0f, z);
                    amMatrixCalcPoint(amEffectPosition, amEffectPosition);
                    amMatrixCalcPoint(amEffectDirection, amEffectDirection);
                    amVectorScale(amEffectVelocity, amEffectDirection, work.offset + work.offset_chaos * nnRandom());
                    amVectorAdd(amEffectPosition, amEffectVelocity);
                    amVectorScale(amEffectVelocity, amEffectDirection, node.speed + node.speed_chaos * nnRandom());
                    effectCreateParam.ecb = amsAmeRuntime1.ecb;
                    effectCreateParam.runtime = amsAmeRuntime2;
                    effectCreateParam.node = amsAmeRuntime2.node;
                    effectCreateParam.parent_position = work.position;
                    effectCreateParam.parent_velocity = work.velocity;
                    effectCreateParam.position = amEffectPosition;
                    effectCreateParam.velocity = amEffectVelocity;
                    switch (AMD_AME_NODE_TYPE(amsAmeRuntime2.node) & 65280)
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

    private static int _amDrawSurface(object r)
    {
        mppAssertNotImpl();
        return 0;
    }
}