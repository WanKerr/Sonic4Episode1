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
    private static int _amInitSurface(object p)
    {
        AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = (AppMain.AMS_AME_CREATE_PARAM)p;
        AppMain.AMS_AME_NODE_SURFACE node = (AppMain.AMS_AME_NODE_SURFACE)amsAmeCreateParam.node;
        AppMain.AMS_AME_RUNTIME_WORK_SURFACE work = (AppMain.AMS_AME_RUNTIME_WORK_SURFACE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        AppMain.amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        AppMain.amVectorAdd(work.position, node.translate);
        AppMain.amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        AppMain.amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
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
        AppMain.AMS_AME_RUNTIME amsAmeRuntime1 = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_SURFACE node = (AppMain.AMS_AME_NODE_SURFACE)amsAmeRuntime1.node;
        AppMain.AMS_AME_RUNTIME_WORK_SURFACE work = (AppMain.AMS_AME_RUNTIME_WORK_SURFACE)amsAmeRuntime1.work;
        work.time += AppMain._am_unit_frame;
        if ((double)work.time <= 0.0)
            return 0;
        if ((double)node.life != -1.0 && (double)work.time >= (double)node.life)
            return 1;
        AppMain.NNS_VECTOR4D amEffectVel = AppMain._amEffect_vel;
        AppMain.amVectorScale(amEffectVel, work.velocity, AppMain._am_unit_time);
        AppMain.amVectorAdd(work.position, amEffectVel);
        float sizeRate = amsAmeRuntime1.ecb.size_rate;
        if ((double)node.width_variation != 0.0 || (double)node.height_variation != 0.0)
        {
            work.width += node.width_variation * AppMain._am_unit_time;
            work.height += node.height_variation * AppMain._am_unit_time;
        }
        else
        {
            work.width = node.width * sizeRate;
            work.height = node.height * sizeRate;
        }
        work.offset = node.offset * sizeRate;
        work.offset_chaos = node.offset_chaos * sizeRate;
        AppMain.NNS_MATRIX amEffectMtx = AppMain._amEffect_mtx;
        AppMain.nnMakeUnitMatrix(amEffectMtx);
        AppMain.amMatrixPush(amEffectMtx);
        AppMain.NNS_QUATERNION rotate = work.rotate;
        AppMain.amQuatToMatrix((AppMain.NNS_MATRIX)null, ref rotate, (AppMain.NNS_VECTOR4D)null);
        work.rotate = rotate;
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
                    AppMain.amVectorSet(amEffectDirection, 0.0f, 1f, 0.0f);
                    float num1 = (float)((double)AppMain.nnRandom() / 2.0 + (double)AppMain.nnRandom() * 0.5);
                    float x = (float)((double)work.width * (double)num1 - (double)work.width * 0.5);
                    float num2 = (float)((double)AppMain.nnRandom() / 2.0 + (double)AppMain.nnRandom() * 0.5);
                    float z = (float)((double)work.height * (double)num2 - (double)work.height * 0.5);
                    if (((int)node.flag & 1) != 0)
                    {
                        float num3 = AppMain.nnRandom();
                        bool flag = false;
                        if ((double)AppMain.nnRandom() > 0.5)
                            flag = true;
                        if ((double)num3 > 0.5)
                        {
                            float num4 = work.width * 0.5f;
                            x = (double)num4 <= 0.0 || !flag ? num4 : -num4;
                        }
                        else
                        {
                            float num4 = work.height * 0.5f;
                            z = (double)num4 <= 0.0 || !flag ? num4 : -num4;
                        }
                    }
                    AppMain.amVectorSet(amEffectPosition, x, 0.0f, z);
                    AppMain.amMatrixCalcPoint(amEffectPosition, amEffectPosition);
                    AppMain.amMatrixCalcPoint(amEffectDirection, amEffectDirection);
                    AppMain.amVectorScale(amEffectVelocity, amEffectDirection, work.offset + work.offset_chaos * AppMain.nnRandom());
                    AppMain.amVectorAdd(amEffectPosition, amEffectVelocity);
                    AppMain.amVectorScale(amEffectVelocity, amEffectDirection, node.speed + node.speed_chaos * AppMain.nnRandom());
                    effectCreateParam.ecb = amsAmeRuntime1.ecb;
                    effectCreateParam.runtime = amsAmeRuntime2;
                    effectCreateParam.node = amsAmeRuntime2.node;
                    effectCreateParam.parent_position = work.position;
                    effectCreateParam.parent_velocity = work.velocity;
                    effectCreateParam.position = amEffectPosition;
                    effectCreateParam.velocity = amEffectVelocity;
                    switch (AppMain.AMD_AME_NODE_TYPE(amsAmeRuntime2.node) & 65280)
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

    private static int _amDrawSurface(object r)
    {
        AppMain.mppAssertNotImpl();
        return 0;
    }
}