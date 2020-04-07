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
    private static int _amInitOmni(object p)
    {
        AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = (AppMain.AMS_AME_CREATE_PARAM)p;
        AppMain.AMS_AME_NODE_OMNI node = (AppMain.AMS_AME_NODE_OMNI)amsAmeCreateParam.node;
        AppMain.AMS_AME_RUNTIME_WORK_OMNI work = (AppMain.AMS_AME_RUNTIME_WORK_OMNI)amsAmeCreateParam.work;
        work.time = -node.start_time;
        AppMain.amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        AppMain.amVectorAdd(work.position, node.translate);
        AppMain.amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        AppMain.amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        work.rotate = node.rotate;
        float sizeRate = amsAmeCreateParam.ecb.size_rate;
        work.offset = node.offset * sizeRate;
        work.offset_chaos = node.offset_chaos * sizeRate;
        return 0;
    }

    private static int _amUpdateOmni(object r)
    {
        AppMain.AMS_AME_RUNTIME amsAmeRuntime1 = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_OMNI node = (AppMain.AMS_AME_NODE_OMNI)amsAmeRuntime1.node;
        AppMain.AMS_AME_RUNTIME_WORK_OMNI work = (AppMain.AMS_AME_RUNTIME_WORK_OMNI)amsAmeRuntime1.work;
        work.time += AppMain._am_unit_frame;
        if ((double)work.time <= 0.0)
            return 0;
        if ((double)node.life != -1.0 && (double)work.time >= (double)node.life)
            return 1;
        AppMain.NNS_VECTOR4D amEffectVel = AppMain._amEffect_vel;
        AppMain.amVectorScale(amEffectVel, work.velocity, AppMain._am_unit_time);
        AppMain.amVectorAdd(work.position, amEffectVel);
        float sizeRate = amsAmeRuntime1.ecb.size_rate;
        work.offset = node.offset * sizeRate;
        work.offset_chaos = node.offset_chaos * sizeRate;
        AppMain.NNS_VECTOR4D amEffectPosition = AppMain._amEffect_position;
        AppMain.NNS_VECTOR4D amEffectVelocity = AppMain._amEffect_velocity;
        AppMain.NNS_VECTOR4D amEffectDirection = AppMain._amEffect_direction;
        AppMain.AMS_AME_LIST next = amsAmeRuntime1.child_head.next;
        for (AppMain.AMS_AME_LIST childTail = amsAmeRuntime1.child_tail; next != childTail; next = next.next)
        {
            AppMain.AMS_AME_RUNTIME amsAmeRuntime2 = (AppMain.AMS_AME_RUNTIME)next;
            amsAmeRuntime2.amount += node.frequency * AppMain._am_unit_frame;
            while ((double)amsAmeRuntime2.amount >= 1.0)
            {
                --amsAmeRuntime2.amount;
                ++amsAmeRuntime2.count;
                if ((double)node.max_count != -1.0 && (double)((int)amsAmeRuntime2.work_num + (int)amsAmeRuntime2.active_num) < (double)node.max_count)
                {
                    AppMain.AMS_AME_CREATE_PARAM effectCreateParam = AppMain._amEffect_create_param;
                    effectCreateParam.Clear();
                    AppMain.amVectorRandom(amEffectDirection);
                    AppMain.amVectorScale(amEffectVelocity, amEffectDirection, work.offset + work.offset_chaos * AppMain.nnRandom());
                    amEffectPosition.Assign(amEffectVelocity);
                    AppMain.amVectorScale(amEffectVelocity, amEffectDirection, node.speed + node.speed_chaos * AppMain.nnRandom());
                    effectCreateParam.ecb = amsAmeRuntime1.ecb;
                    effectCreateParam.runtime = amsAmeRuntime2;
                    effectCreateParam.node = amsAmeRuntime2.node;
                    effectCreateParam.parent_position = work.position;
                    effectCreateParam.parent_velocity = work.velocity;
                    effectCreateParam.position = amEffectPosition;
                    effectCreateParam.velocity = amEffectVelocity;
                    switch (AppMain.AMD_AME_SUPER_CLASS_ID(amsAmeRuntime2.node))
                    {
                        case 256:
                            AppMain._amCreateEmitter(effectCreateParam);
                            continue;
                        case 512:
                            AppMain._amCreateParticle(effectCreateParam);
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
        AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = (AppMain.AMS_AME_CREATE_PARAM)p;
        AppMain.AMS_AME_NODE_DIRECTIONAL node = (AppMain.AMS_AME_NODE_DIRECTIONAL)amsAmeCreateParam.node;
        AppMain.AMS_AME_RUNTIME_WORK_DIRECTIONAL work = (AppMain.AMS_AME_RUNTIME_WORK_DIRECTIONAL)amsAmeCreateParam.work;
        work.time = -node.start_time;
        AppMain.amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        AppMain.amVectorAdd(work.position, node.translate);
        AppMain.amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        AppMain.amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        work.rotate = node.rotate;
        work.spread = node.spread;
        return 0;
    }

    private static int _amUpdateDirectional(object r)
    {
        AppMain.AMS_AME_RUNTIME amsAmeRuntime1 = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_DIRECTIONAL node = (AppMain.AMS_AME_NODE_DIRECTIONAL)amsAmeRuntime1.node;
        AppMain.AMS_AME_RUNTIME_WORK_DIRECTIONAL runtimeWorkDirectional = new AppMain.AMS_AME_RUNTIME_WORK_DIRECTIONAL(amsAmeRuntime1.work);
        runtimeWorkDirectional.time += AppMain._am_unit_frame;
        if ((double)runtimeWorkDirectional.time <= 0.0)
            return 0;
        if ((double)node.life != -1.0 && (double)runtimeWorkDirectional.time >= (double)node.life)
            return 1;
        AppMain.NNS_VECTOR4D amEffectVel = AppMain._amEffect_vel;
        AppMain.amVectorScale(amEffectVel, runtimeWorkDirectional.velocity, AppMain._am_unit_time);
        AppMain.amVectorAdd(runtimeWorkDirectional.position, amEffectVel);
        runtimeWorkDirectional.spread += node.spread_variation * AppMain._am_unit_time;
        AppMain.NNS_MATRIX amEffectMtx = AppMain._amEffect_mtx;
        AppMain.nnMakeUnitMatrix(amEffectMtx);
        AppMain.amMatrixPush(amEffectMtx);
        AppMain.NNS_QUATERNION rotate = runtimeWorkDirectional.rotate;
        AppMain.amQuatToMatrix((AppMain.NNS_MATRIX)null, ref rotate, (AppMain.NNS_VECTOR4D)null);
        runtimeWorkDirectional.rotate = rotate;
        AppMain.AMS_AME_LIST next = amsAmeRuntime1.child_head.next;
        AppMain.AMS_AME_LIST childTail = amsAmeRuntime1.child_tail;
        AppMain.NNS_VECTOR4D amEffectPosition = AppMain._amEffect_position;
        AppMain.NNS_VECTOR4D amEffectVelocity = AppMain._amEffect_velocity;
        AppMain.NNS_VECTOR4D amEffectDirection = AppMain._amEffect_direction;
        for (; next != childTail; next = next.next)
        {
            AppMain.AMS_AME_RUNTIME amsAmeRuntime2 = (AppMain.AMS_AME_RUNTIME)next;
            amsAmeRuntime2.amount += node.frequency * AppMain._am_unit_frame;
            while ((double)amsAmeRuntime2.amount >= 1.0)
            {
                --amsAmeRuntime2.amount;
                ++amsAmeRuntime2.count;
                if ((double)node.max_count != -1.0 && (double)((int)amsAmeRuntime2.work_num + (int)amsAmeRuntime2.active_num) < (double)node.max_count)
                {
                    AppMain.AMS_AME_CREATE_PARAM effectCreateParam = AppMain._amEffect_create_param;
                    effectCreateParam.Clear();
                    AppMain.amEffectRandomConeVectorDeg(amEffectDirection, runtimeWorkDirectional.spread);
                    AppMain.amMatrixCalcPoint(amEffectDirection, amEffectDirection);
                    AppMain.amVectorScale(amEffectVelocity, amEffectDirection, node.offset + node.offset_chaos * AppMain.nnRandom());
                    amEffectPosition.Assign(amEffectVelocity);
                    AppMain.amVectorScale(amEffectVelocity, amEffectDirection, node.speed + node.speed_chaos * AppMain.nnRandom());
                    effectCreateParam.ecb = amsAmeRuntime1.ecb;
                    effectCreateParam.runtime = amsAmeRuntime2;
                    effectCreateParam.node = amsAmeRuntime2.node;
                    effectCreateParam.parent_position = runtimeWorkDirectional.position;
                    effectCreateParam.parent_velocity = runtimeWorkDirectional.velocity;
                    effectCreateParam.position = amEffectPosition;
                    effectCreateParam.velocity = amEffectVelocity;
                    switch (AppMain.AMD_AME_SUPER_CLASS_ID(amsAmeRuntime2.node))
                    {
                        case 256:
                            AppMain._amCreateEmitter(effectCreateParam);
                            continue;
                        case 512:
                            AppMain._amCreateParticle(effectCreateParam);
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }
        AppMain.amMatrixPop();
        return 0;
    }

    private static int _amDrawDirectional(object r)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }

}