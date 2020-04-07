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
    public static int _amInitModel(object p)
    {
        AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = (AppMain.AMS_AME_CREATE_PARAM)p;
        AppMain.AMS_AME_NODE_MODEL node = (AppMain.AMS_AME_NODE_MODEL)amsAmeCreateParam.node;
        AppMain.AMS_AME_RUNTIME_WORK_MODEL work = (AppMain.AMS_AME_RUNTIME_WORK_MODEL)amsAmeCreateParam.work;
        work.time = -node.start_time;
        work.scale.Assign(node.scale_start);
        work.set_color(node.color_start.color);
        AppMain.amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        AppMain.amVectorAdd(work.position, node.translate);
        AppMain.amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        AppMain.amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        if (((int)node.flag & 4) != 0)
        {
            AppMain.NNS_VECTOR4D nnsVectoR4D = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
            AppMain.amVectorRandom(nnsVectoR4D);
            AppMain.NNS_QUATERNION rotate = work.rotate;
            AppMain.amQuatRotAxisToQuat(ref rotate, nnsVectoR4D, (float)((double)AppMain.nnRandom() * 2.0 * 3.14159274101257));
            work.rotate = rotate;
        }
        else
            work.rotate = node.rotate;
        if (((int)node.flag & 8) != 0)
        {
            AppMain.NNS_VECTOR4D pDst = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
            AppMain.amVectorRandom(pDst);
            work.set_rotate_axis(pDst.x, pDst.y, pDst.z, node.rotate_axis.w);
        }
        else
            work.rotate_axis.Assign(node.rotate_axis);
        return 0;
    }

    public static int _amUpdateModel(object r)
    {
        AppMain.AMS_AME_RUNTIME runtime = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_MODEL node = (AppMain.AMS_AME_NODE_MODEL)runtime.node;
        AppMain.AMS_AME_RUNTIME_WORK_MODEL next1 = (AppMain.AMS_AME_RUNTIME_WORK_MODEL)(AppMain.AMS_AME_RUNTIME_WORK)runtime.active_head.next;
        AppMain.AMS_AME_LIST next2 = runtime.active_head.next;
        AppMain.AMS_AME_LIST activeTail = runtime.active_tail;
        int transparency = runtime.ecb.transparency;
        float num1;
        float num2;
        if ((double)node.life >= 0.0)
        {
            num1 = node.life;
            num2 = 1f / num1;
        }
        else
        {
            num1 = float.MaxValue;
            num2 = 0.0f;
        }
        AppMain.NNS_VECTOR4D nnsVectoR4D1 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.NNS_VECTOR4D nnsVectoR4D2 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        AppMain.amVectorScale(nnsVectoR4D1, node.scale_start, runtime.ecb.size_rate);
        AppMain.amVectorScale(nnsVectoR4D2, node.scale_end, runtime.ecb.size_rate);
        AppMain.AMS_AME_RUNTIME_WORK_MODEL runtimeWorkModel;
        for (; next2 != activeTail; next2 = runtimeWorkModel.next)
        {
            runtimeWorkModel = (AppMain.AMS_AME_RUNTIME_WORK_MODEL)(AppMain.AMS_AME_RUNTIME_WORK)next2;
            runtimeWorkModel.time += AppMain._am_unit_frame;
            float num3 = runtimeWorkModel.time * num2;
            AppMain.NNS_VECTOR4D nnsVectoR4D3 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
            AppMain.amVectorScale(nnsVectoR4D3, runtimeWorkModel.velocity, AppMain._am_unit_time);
            AppMain.amVectorAdd(runtimeWorkModel.position, nnsVectoR4D3);
            AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D3);
            if ((double)runtimeWorkModel.time >= (double)num1)
            {
                if (runtime.spawn_runtime != null)
                    AppMain._amCreateSpawnParticle(runtime, (AppMain.AMS_AME_RUNTIME_WORK)runtimeWorkModel);
                AppMain.amEffectDisconnectLink((AppMain.AMS_AME_LIST)runtimeWorkModel);
                --runtime.active_num;
                AppMain.amEffectFreeRuntimeWork((AppMain.AMS_AME_RUNTIME_WORK)runtimeWorkModel);
            }
            else
            {
                AppMain.NNS_QUATERNION nnsQuaternion = new AppMain.NNS_QUATERNION();
                AppMain.NNS_VECTOR4D nnsVectoR4D4 = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
                AppMain.Vector4D_Buf rotateAxis = runtimeWorkModel.rotate_axis;
                nnsVectoR4D4.x = rotateAxis.x;
                nnsVectoR4D4.y = rotateAxis.y;
                nnsVectoR4D4.z = rotateAxis.z;
                nnsVectoR4D4.w = rotateAxis.w;
                AppMain.amQuatRotAxisToQuat(ref nnsQuaternion, nnsVectoR4D4, nnsVectoR4D4.w * AppMain._am_unit_time);
                AppMain.NNS_QUATERNION rotate = runtimeWorkModel.rotate;
                AppMain.amQuatMulti(ref rotate, ref rotate, ref nnsQuaternion);
                runtimeWorkModel.rotate = rotate;
                AppMain.amVectorGetInner(nnsVectoR4D4, nnsVectoR4D1, nnsVectoR4D2, num3);
                runtimeWorkModel.set_scale(nnsVectoR4D4);
                AppMain.AMS_RGBA8888 pCO;
                AppMain.amEffectLerpColor(out pCO, ref node.color_start, ref node.color_end, num3);
                pCO.a = (byte)((int)pCO.a * transparency >> 8);
                runtimeWorkModel.set_color(pCO.color);
                runtimeWorkModel.scroll_u += node.scroll_u * AppMain._am_unit_time;
                runtimeWorkModel.scroll_v += node.scroll_v * AppMain._am_unit_time;
                AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D4);
            }
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D2);
        return 0;
    }

    public static int _amDrawModel(object r)
    {
        AppMain.AMS_AME_RUNTIME runtime = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_MODEL node = (AppMain.AMS_AME_NODE_MODEL)runtime.node;
        AppMain.AMS_AME_ECB ecb = runtime.ecb;
        AppMain.NNS_RGBA color = new AppMain.NNS_RGBA();
        int blend = -1;
        if (runtime.ecb.pObj == null)
            return 0;
        uint drawflag = AppMain._amEffectSetMaterial(runtime, ref blend, node.blend);
        float zBias = node.z_bias;
        AppMain.SNNS_VECTOR4D pDst1;
        AppMain.amVectorSet(out pDst1, zBias * AppMain._am_ef_worldViewMtx.M20, zBias * AppMain._am_ef_worldViewMtx.M21, zBias * AppMain._am_ef_worldViewMtx.M22);
        AppMain.AMS_AME_LIST activeTail = runtime.active_tail;
        AppMain.AMS_AME_LIST next = runtime.active_head.next;
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.AMS_AME_RUNTIME_WORK_MODEL runtimeWorkModel;
        for (; next != activeTail; next = runtimeWorkModel.next)
        {
            runtimeWorkModel = (AppMain.AMS_AME_RUNTIME_WORK_MODEL)(AppMain.AMS_AME_RUNTIME_WORK)next;
            AppMain.amMatrixPush();
            AppMain.SNNS_VECTOR4D pDst2;
            AppMain.amVectorAdd(out pDst2, runtimeWorkModel.position, ref pDst1);
            AppMain.NNS_QUATERNION rotate = runtimeWorkModel.rotate;
            AppMain.amQuatMultiMatrix(ref rotate, ref pDst2);
            runtimeWorkModel.rotate = rotate;
            float scrollU = runtimeWorkModel.scroll_u;
            float scrollV = runtimeWorkModel.scroll_v;
            uint num = drawflag | 268435456U;
            if ((double)scrollU >= 1.0)
            {
                scrollU -= (float)(int)scrollU;
            }
            else
            {
                while ((double)scrollU < 0.0)
                    ++scrollU;
            }
            if ((double)scrollV >= 1.0)
            {
                scrollV -= (float)(int)scrollV;
            }
            else
            {
                while ((double)scrollV < 0.0)
                    ++scrollV;
            }
            drawflag = num | 3145728U;
            color.a = (float)runtimeWorkModel.color.a * 0.003921569f;
            color.r = (float)runtimeWorkModel.color.r * 0.003921569f;
            color.g = (float)runtimeWorkModel.color.g * 0.003921569f;
            color.b = (float)runtimeWorkModel.color.b * 0.003921569f;
            AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
            nnsMatrix.Assign(current);
            AppMain.nnScaleMatrix(nnsMatrix, nnsMatrix, runtimeWorkModel.scale.x, runtimeWorkModel.scale.y, runtimeWorkModel.scale.z);
            AppMain.nnCopyMatrix(current, nnsMatrix);
            nnsVector.x = runtimeWorkModel.scale.x;
            nnsVector.y = runtimeWorkModel.scale.y;
            nnsVector.z = runtimeWorkModel.scale.z;
            AppMain.amDrawObjectSetMaterial(runtime.ecb.drawObjState, ecb.pObj, runtime.texlist, nnsVector, ref color, scrollU, scrollV, blend, drawflag);
            runtimeWorkModel.set_scale(nnsVector);
            AppMain.amMatrixPop();
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
        return 0;
    }
}