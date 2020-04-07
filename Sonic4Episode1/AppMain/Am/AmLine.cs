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
    public static int _amInitLine(object p)
    {
        AppMain.AMS_AME_CREATE_PARAM amsAmeCreateParam = (AppMain.AMS_AME_CREATE_PARAM)p;
        AppMain.AMS_AME_NODE_LINE node = (AppMain.AMS_AME_NODE_LINE)amsAmeCreateParam.node;
        AppMain.AMS_AME_RUNTIME_WORK_LINE work = (AppMain.AMS_AME_RUNTIME_WORK_LINE)amsAmeCreateParam.work;
        work.time = -node.start_time;
        work.length = node.length_start;
        work.inside_width = node.inside_width_start;
        work.outside_width = node.outside_width_start;
        AppMain.AMS_RGBA8888 insideColorStart = node.inside_color_start;
        insideColorStart.a = (byte)((int)insideColorStart.a * amsAmeCreateParam.ecb.transparency >> 8);
        work.set_inside_color(insideColorStart.color);
        AppMain.AMS_RGBA8888 outsideColorStart = node.outside_color_start;
        outsideColorStart.a = (byte)((int)outsideColorStart.a * amsAmeCreateParam.ecb.transparency >> 8);
        work.set_outside_color(outsideColorStart.color);
        AppMain.amVectorAdd(work.position, amsAmeCreateParam.parent_position, amsAmeCreateParam.position);
        AppMain.amVectorAdd(work.position, node.translate);
        AppMain.amVectorScale(work.velocity, amsAmeCreateParam.parent_velocity, node.inheritance_rate);
        AppMain.amVectorAdd(work.velocity, amsAmeCreateParam.velocity);
        if (((int)node.flag & 32768) != 0)
        {
            work.tex_time = 0.0f;
            work.tex_no = 0;
            if (((int)node.flag & 524288) != 0)
                work.tex_no = (int)(100.0 * (double)AppMain.nnRandom()) % node.tex_anim.key_num;
            AppMain.AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = node.tex_anim.key_buf[work.tex_no];
            work.set_st(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
        }
        else if (((int)node.flag & 8192) != 0)
            work.set_st(node.cropping_l, node.cropping_t, node.cropping_r, node.cropping_b);
        else
            work.set_st(0.0f, 0.0f, 1f, 1f);
        if (((int)node.flag & 1048576) != 0 || ((int)node.flag & 131072) != 0 && (double)AppMain.nnRandom() > 0.5)
        {
            work.flag |= 8U;
            work.set_st(work.st.z, work.st.y, work.st.x, work.st.w);
        }
        if (((int)node.flag & 2097152) != 0 || ((int)node.flag & 262144) != 0 && (double)AppMain.nnRandom() > 0.5)
        {
            work.flag |= 16U;
            work.set_st(work.st.x, work.st.w, work.st.z, work.st.y);
        }
        return 0;
    }

    public static int _amUpdateLine(object r)
    {
        AppMain.AMS_AME_RUNTIME runtime = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_LINE node = (AppMain.AMS_AME_NODE_LINE)runtime.node;
        AppMain.AMS_AME_LIST next = runtime.active_head.next;
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
            num1 = 1E+38f;
            num2 = 0.0f;
        }
        float sizeRate = runtime.ecb.size_rate;
        float num3 = node.length_start * sizeRate;
        float num4 = node.length_end * sizeRate;
        float num5 = node.inside_width_start * sizeRate;
        float num6 = node.outside_width_start * sizeRate;
        float num7 = node.inside_width_end * sizeRate;
        float num8 = node.outside_width_end * sizeRate;
        AppMain.NNS_VECTOR4D nnsVectoR4D = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        for (; next != activeTail; next = next.next)
        {
            AppMain.AMS_AME_RUNTIME_WORK_LINE ameRuntimeWorkLine = (AppMain.AMS_AME_RUNTIME_WORK_LINE)(AppMain.AMS_AME_RUNTIME_WORK)next;
            ameRuntimeWorkLine.time += AppMain._am_unit_frame;
            float rate = ameRuntimeWorkLine.time * num2;
            AppMain.amVectorScale(nnsVectoR4D, ameRuntimeWorkLine.velocity, AppMain._am_unit_time);
            AppMain.amVectorAdd(ameRuntimeWorkLine.position, nnsVectoR4D);
            if ((double)ameRuntimeWorkLine.time >= (double)num1)
            {
                if (runtime.spawn_runtime != null)
                    AppMain._amCreateSpawnParticle(runtime, (AppMain.AMS_AME_RUNTIME_WORK)ameRuntimeWorkLine);
                AppMain.amEffectDisconnectLink((AppMain.AMS_AME_LIST)ameRuntimeWorkLine);
                --runtime.active_num;
                AppMain.amEffectFreeRuntimeWork((AppMain.AMS_AME_RUNTIME_WORK)ameRuntimeWorkLine);
            }
            else
            {
                float num9 = 1f - rate;
                ameRuntimeWorkLine.length = (float)((double)num3 * (double)num9 + (double)num4 * (double)rate);
                ameRuntimeWorkLine.inside_width = (float)((double)num5 * (double)num9 + (double)num7 * (double)rate);
                ameRuntimeWorkLine.outside_width = (float)((double)num6 * (double)num9 + (double)num8 * (double)rate);
                AppMain.AMS_RGBA8888 pCO;
                AppMain.amEffectLerpColor(out pCO, ref node.inside_color_start, ref node.inside_color_end, rate);
                pCO.a = (byte)((int)pCO.a * transparency >> 8);
                ameRuntimeWorkLine.set_inside_color(pCO.color);
                AppMain.amEffectLerpColor(out pCO, ref node.outside_color_start, ref node.outside_color_end, rate);
                pCO.a = (byte)((int)pCO.a * transparency >> 8);
                ameRuntimeWorkLine.set_outside_color(pCO.color);
                if (((int)node.flag & 32768) != 0)
                {
                    AppMain.AMS_AME_TEX_ANIM texAnim = node.tex_anim;
                    if (((int)ameRuntimeWorkLine.flag & 2) == 0)
                    {
                        ameRuntimeWorkLine.tex_time += AppMain._am_unit_frame;
                        if ((double)ameRuntimeWorkLine.tex_time >= (double)texAnim.key_buf[ameRuntimeWorkLine.tex_no].time)
                        {
                            ameRuntimeWorkLine.tex_time = 0.0f;
                            ++ameRuntimeWorkLine.tex_no;
                            if (ameRuntimeWorkLine.tex_no == texAnim.key_num)
                            {
                                if (((int)node.flag & 65536) != 0)
                                {
                                    ameRuntimeWorkLine.tex_no = 0;
                                }
                                else
                                {
                                    ameRuntimeWorkLine.tex_no = texAnim.key_num - 1;
                                    ameRuntimeWorkLine.flag |= 2U;
                                }
                            }
                        }
                    }
                    AppMain.AMS_AME_TEX_ANIM_KEY amsAmeTexAnimKey = texAnim.key_buf[ameRuntimeWorkLine.tex_no];
                    ameRuntimeWorkLine.set_st(amsAmeTexAnimKey.l, amsAmeTexAnimKey.t, amsAmeTexAnimKey.r, amsAmeTexAnimKey.b);
                    if (((int)ameRuntimeWorkLine.flag & 8) != 0)
                        ameRuntimeWorkLine.set_st(ameRuntimeWorkLine.st.z, ameRuntimeWorkLine.st.y, ameRuntimeWorkLine.st.x, ameRuntimeWorkLine.st.w);
                    if (((int)ameRuntimeWorkLine.flag & 16) != 0)
                        ameRuntimeWorkLine.set_st(ameRuntimeWorkLine.st.x, ameRuntimeWorkLine.st.w, ameRuntimeWorkLine.st.z, ameRuntimeWorkLine.st.y);
                }
                else if (((int)node.flag & 16384) != 0)
                {
                    float num10 = node.scroll_u * AppMain._am_unit_time;
                    float num11 = node.scroll_v * AppMain._am_unit_time;
                    if (((int)ameRuntimeWorkLine.flag & 8) != 0)
                        num10 = -num10;
                    if (((int)ameRuntimeWorkLine.flag & 16) != 0)
                        num11 = -num11;
                    ameRuntimeWorkLine.set_st(ameRuntimeWorkLine.st.x + num10, ameRuntimeWorkLine.st.w + num10, ameRuntimeWorkLine.st.z + num11, ameRuntimeWorkLine.st.y + num11);
                }
            }
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Release(nnsVectoR4D);
        return 0;
    }

    public static int _amDrawLine(object r)
    {
        AppMain.AMS_AME_RUNTIME runtime = (AppMain.AMS_AME_RUNTIME)r;
        AppMain.AMS_AME_NODE_LINE node = (AppMain.AMS_AME_NODE_LINE)runtime.node;
        AppMain.AMS_AME_LIST next = runtime.active_head.next;
        AppMain.AMS_AME_LIST activeTail = runtime.active_tail;
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.amDrawAlloc_AMS_PARAM_DRAW_PRIMITIVE();
        int num1 = AppMain._amEffectSetDrawMode(runtime, setParam, node.blend);
        AppMain.NNS_VECTOR4D amDrawLineOffset = AppMain._amDrawLine_offset;
        AppMain.NNS_VECTOR4D amDrawLineEye = AppMain._amDrawLine_eye;
        float zBias = node.z_bias;
        AppMain.amVectorSet(amDrawLineOffset, zBias * AppMain._am_ef_worldViewMtx.M20, zBias * AppMain._am_ef_worldViewMtx.M21, zBias * AppMain._am_ef_worldViewMtx.M22);
        AppMain.amVectorSet(amDrawLineEye, AppMain._am_ef_worldViewMtx.M20, AppMain._am_ef_worldViewMtx.M21, AppMain._am_ef_worldViewMtx.M22);
        AppMain.NNS_VECTOR4D amDrawLinePos0 = AppMain._amDrawLine_pos0;
        AppMain.NNS_VECTOR4D amDrawLinePos1 = AppMain._amDrawLine_pos1;
        AppMain.NNS_VECTOR amDrawLineVel = (AppMain.NNS_VECTOR)AppMain._amDrawLine_vel;
        AppMain.NNS_VECTOR amDrawLineCross = (AppMain.NNS_VECTOR)AppMain._amDrawLine_cross;
        if (((int)node.flag & 4096) != 0)
        {
            AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(6 * (int)runtime.active_num);
            AppMain.NNS_PRIM3D_PCT[] buffer = nnsPriM3DPctArray.buffer;
            int offset = nnsPriM3DPctArray.offset;
            float num2 = 0.0f;
            for (; next != activeTail; next = next.next)
            {
                AppMain.AMS_AME_RUNTIME_WORK_LINE ameRuntimeWorkLine = (AppMain.AMS_AME_RUNTIME_WORK_LINE)(AppMain.AMS_AME_RUNTIME_WORK)next;
                double num3 = (double)AppMain.amVectorUnit(amDrawLineVel, ameRuntimeWorkLine.velocity);
                AppMain.nnScaleVector((AppMain.NNS_VECTOR)amDrawLinePos0, amDrawLineVel, ameRuntimeWorkLine.length);
                AppMain.amVectorAdd(amDrawLinePos1, ameRuntimeWorkLine.position, amDrawLineOffset);
                AppMain.nnAddVector((AppMain.NNS_VECTOR)amDrawLinePos0, (AppMain.NNS_VECTOR)amDrawLinePos0, (AppMain.NNS_VECTOR)amDrawLinePos1);
                num2 = AppMain.nnDistanceVector((AppMain.NNS_VECTOR)amDrawLinePos0, AppMain._am_ef_camPos);
                AppMain.nnCrossProductVector(amDrawLineCross, amDrawLineVel, (AppMain.NNS_VECTOR)amDrawLineEye);
                AppMain.nnNormalizeVector(amDrawLineCross, amDrawLineCross);
                AppMain.nnScaleVector(amDrawLineVel, amDrawLineCross, ameRuntimeWorkLine.outside_width);
                AppMain.nnSubtractVector(ref buffer[offset].Pos, (AppMain.NNS_VECTOR)amDrawLinePos0, amDrawLineVel);
                AppMain.nnAddVector(ref buffer[offset + 1].Pos, (AppMain.NNS_VECTOR)amDrawLinePos0, amDrawLineVel);
                AppMain.nnScaleVector(amDrawLineVel, amDrawLineCross, ameRuntimeWorkLine.inside_width);
                AppMain.nnSubtractVector(ref buffer[offset + 2].Pos, (AppMain.NNS_VECTOR)amDrawLinePos1, amDrawLineVel);
                AppMain.nnAddVector(ref buffer[offset + 5].Pos, (AppMain.NNS_VECTOR)amDrawLinePos1, amDrawLineVel);
                buffer[offset + 1].Col = AppMain.AMD_RGBA8888(ameRuntimeWorkLine.outside_color.r, ameRuntimeWorkLine.outside_color.g, ameRuntimeWorkLine.outside_color.b, ameRuntimeWorkLine.outside_color.a);
                buffer[offset].Col = buffer[offset + 1].Col;
                buffer[offset + 5].Col = AppMain.AMD_RGBA8888(ameRuntimeWorkLine.inside_color.r, ameRuntimeWorkLine.inside_color.g, ameRuntimeWorkLine.inside_color.b, ameRuntimeWorkLine.inside_color.a);
                buffer[offset + 2].Col = buffer[offset + 5].Col;
                AppMain.Vector4D_Quat st = ameRuntimeWorkLine.st;
                buffer[offset].Tex.u = st.x;
                buffer[offset].Tex.v = st.y;
                buffer[offset + 1].Tex.u = st.z;
                buffer[offset + 1].Tex.v = st.y;
                buffer[offset + 2].Tex.u = st.x;
                buffer[offset + 2].Tex.v = st.w;
                buffer[offset + 5].Tex.u = st.z;
                buffer[offset + 5].Tex.v = st.w;
                buffer[offset + 3] = buffer[offset + 1];
                buffer[offset + 4] = buffer[offset + 2];
                offset += 6;
            }
            setParam.format3D = 4;
            setParam.type = 0;
            setParam.vtxPCT3D = nnsPriM3DPctArray;
            setParam.texlist = runtime.texlist;
            setParam.texId = (int)node.texture_id;
            setParam.count = 6 * (int)runtime.active_num;
            setParam.ablend = num1;
            setParam.sortZ = num2;
            AppMain.amDrawPrimitive3D(runtime.ecb.drawState, setParam);
        }
        else
        {
            AppMain.NNS_PRIM3D_PC[] nnsPriM3DPcArray1 = new AppMain.NNS_PRIM3D_PC[6 * (int)runtime.active_num];
            int index = 0;
            AppMain.NNS_PRIM3D_PC[] nnsPriM3DPcArray2 = nnsPriM3DPcArray1;
            float num2 = 0.0f;
            for (; next != activeTail; next = next.next)
            {
                AppMain.AMS_AME_RUNTIME_WORK_LINE ameRuntimeWorkLine = (AppMain.AMS_AME_RUNTIME_WORK_LINE)(AppMain.AMS_AME_RUNTIME_WORK)next;
                double num3 = (double)AppMain.amVectorUnit(amDrawLineVel, ameRuntimeWorkLine.velocity);
                AppMain.nnScaleVector((AppMain.NNS_VECTOR)amDrawLinePos0, amDrawLineVel, ameRuntimeWorkLine.length);
                AppMain.amVectorAdd(amDrawLinePos1, ameRuntimeWorkLine.position, amDrawLineOffset);
                AppMain.nnAddVector((AppMain.NNS_VECTOR)amDrawLinePos0, (AppMain.NNS_VECTOR)amDrawLinePos0, (AppMain.NNS_VECTOR)amDrawLinePos1);
                num2 = AppMain.nnDistanceVector((AppMain.NNS_VECTOR)amDrawLinePos0, AppMain._am_ef_camPos);
                AppMain.nnCrossProductVector(amDrawLineCross, amDrawLineVel, (AppMain.NNS_VECTOR)amDrawLineEye);
                AppMain.nnNormalizeVector(amDrawLineCross, amDrawLineCross);
                AppMain.nnScaleVector(amDrawLineVel, amDrawLineCross, ameRuntimeWorkLine.outside_width);
                AppMain.nnSubtractVector(ref nnsPriM3DPcArray1[index].Pos, (AppMain.NNS_VECTOR)amDrawLinePos0, amDrawLineVel);
                AppMain.nnAddVector(ref nnsPriM3DPcArray1[index + 1].Pos, (AppMain.NNS_VECTOR)amDrawLinePos0, amDrawLineVel);
                AppMain.nnScaleVector(amDrawLineVel, amDrawLineCross, ameRuntimeWorkLine.inside_width);
                AppMain.nnSubtractVector(ref nnsPriM3DPcArray1[index + 2].Pos, (AppMain.NNS_VECTOR)amDrawLinePos1, amDrawLineVel);
                AppMain.nnAddVector(ref nnsPriM3DPcArray1[index + 5].Pos, (AppMain.NNS_VECTOR)amDrawLinePos1, amDrawLineVel);
                nnsPriM3DPcArray1[index + 1].Col = AppMain.AMD_RGBA8888(ameRuntimeWorkLine.outside_color.r, ameRuntimeWorkLine.outside_color.g, ameRuntimeWorkLine.outside_color.b, ameRuntimeWorkLine.outside_color.a);
                nnsPriM3DPcArray1[index].Col = nnsPriM3DPcArray1[index + 1].Col;
                nnsPriM3DPcArray1[index + 5].Col = AppMain.AMD_RGBA8888(ameRuntimeWorkLine.inside_color.r, ameRuntimeWorkLine.inside_color.g, ameRuntimeWorkLine.inside_color.b, ameRuntimeWorkLine.inside_color.a);
                nnsPriM3DPcArray1[index + 2].Col = nnsPriM3DPcArray1[index + 5].Col;
                nnsPriM3DPcArray1[index + 3] = nnsPriM3DPcArray1[index + 1];
                nnsPriM3DPcArray1[index + 4] = nnsPriM3DPcArray1[index + 2];
                index += 6;
            }
            setParam.format3D = 2;
            setParam.type = 0;
            setParam.vtxPC3D = nnsPriM3DPcArray2;
            setParam.texlist = runtime.texlist;
            setParam.texId = -1;
            setParam.count = 6 * (int)runtime.active_num;
            setParam.ablend = num1;
            setParam.sortZ = num2;
            AppMain.amDrawPrimitive3D(runtime.ecb.drawState, setParam);
        }
        return 0;
    }

}