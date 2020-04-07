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
    private void AoWinSysDraw(
         int type,
         AppMain.NNS_TEXLIST texlist,
         uint tex_id,
         float x,
         float y,
         float w,
         float h)
    {
        AppMain.AoActDrawPre();
        AppMain.aoWinSysDrawPrimitveA(texlist, tex_id, x, y, w, h);
    }

    private static void AoWinSysDrawState(
      int type,
      AppMain.NNS_TEXLIST texlist,
      uint tex_id,
      float x,
      float y,
      float w,
      float h,
      uint state)
    {
        AppMain.AoWinSysDrawState(type, texlist, tex_id, x, y, w, h, state, 0.0f);
    }

    private static void AoWinSysDrawState(
      int type,
      AppMain.NNS_TEXLIST texlist,
      uint tex_id,
      float x,
      float y,
      float w,
      float h,
      uint state,
      float z)
    {
        AppMain.aoWinSysMakeCommandA(state, texlist, tex_id, x, y, w, h, z);
    }

    private static void AoWinSysDrawTask(
      int type,
      AppMain.NNS_TEXLIST texlist,
      uint tex_id,
      float x,
      float y,
      float w,
      float h,
      ushort prio)
    {
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.aoWinSysTaskDraw), prio, (object)new AppMain.AOS_WIN_DRAW_WORK()
        {
            type = type,
            texlist = texlist,
            tex_id = tex_id,
            x = x,
            y = y,
            w = w,
            h = h
        });
    }

    private static void aoWinSysTaskDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.AOS_WIN_DRAW_WORK work = (AppMain.AOS_WIN_DRAW_WORK)AppMain.amTaskGetWork(tcb);
        AppMain.AoActDrawPre();
        AppMain.aoWinSysDrawPrimitveA(work.texlist, work.tex_id, work.x, work.y, work.w, work.h);
    }

    private static void aoWinSysDrawPrimitveA(
      AppMain.NNS_TEXLIST texlist,
      uint tex_id,
      float x,
      float y,
      float w,
      float h)
    {
        AppMain.NNS_PRIM3D_PCT_ARRAY array = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(8);
        AppMain.NNS_PRIM3D_PCT[] buffer = array.buffer;
        int offset = array.offset;
        AppMain.amDrawPushState();
        AppMain.amDrawInitState();
        AppMain.nnSetPrimitive3DAlphaFuncGL(519U, 0.5f);
        AppMain.nnSetPrimitive3DDepthMaskGL(false);
        AppMain.nnSetPrimitive3DDepthFuncGL(519U);
        AppMain.nnSetPrimitiveBlend(1);
        AppMain.amDrawSetFog(0);
        AppMain.nnSetPrimitiveTexNum(texlist, (int)tex_id);
        AppMain.nnSetPrimitiveTexState(0, 0, 1, 1);
        AppMain.nnBeginDrawPrimitive3D(4, 1, 0, 0);
        AppMain.aoWinSysMakeVertex00A(array, x, y, w, h);
        AppMain.nnDrawPrimitive3D(1, (object)array, 8);
        AppMain.aoWinSysMakeVertex01A(array, x, y, w, h);
        AppMain.nnDrawPrimitive3D(1, (object)array, 8);
        AppMain.aoWinSysMakeVertex02A(array, x, y, w, h);
        AppMain.nnDrawPrimitive3D(1, (object)array, 8);
        AppMain.nnEndDrawPrimitive3D();
        AppMain.amDrawPopState();
    }

    private static void aoWinSysMakeCommandA(
      uint state,
      AppMain.NNS_TEXLIST texlist,
      uint tex_id,
      float x,
      float y,
      float w,
      float h,
      float z)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.mtx = (AppMain.NNS_MATRIX)null;
        setParam.vtxPCT3D = (AppMain.NNS_PRIM3D_PCT_ARRAY)null;
        setParam.format3D = 4;
        setParam.type = 1;
        setParam.count = 8;
        setParam.texlist = texlist;
        setParam.texId = (int)tex_id;
        setParam.ablend = 1;
        setParam.sortZ = z;
        setParam.aTest = (short)0;
        setParam.zMask = (short)1;
        setParam.zTest = (short)0;
        setParam.noSort = (short)1;
        setParam.uwrap = 1;
        setParam.vwrap = 1;
        AppMain.NNS_PRIM3D_PCT_ARRAY array1 = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(8);
        AppMain.aoWinSysMakeVertex00A(array1, x, y, w, h);
        setParam.vtxPCT3D = array1;
        AppMain.amDrawPrimitive3D(state, setParam);
        AppMain.NNS_PRIM3D_PCT_ARRAY array2 = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(8);
        AppMain.aoWinSysMakeVertex01A(array2, x, y, w, h);
        setParam.vtxPCT3D = array2;
        AppMain.amDrawPrimitive3D(state, setParam);
        AppMain.NNS_PRIM3D_PCT_ARRAY array3 = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(8);
        AppMain.aoWinSysMakeVertex02A(array3, x, y, w, h);
        setParam.vtxPCT3D = array3;
        AppMain.amDrawPrimitive3D(state, setParam);
        AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    private static void aoWinSysMakeVertex00A(
      AppMain.NNS_PRIM3D_PCT_ARRAY array,
      float x,
      float y,
      float w,
      float h)
    {
        int offset = array.offset;
        AppMain.NNS_PRIM3D_PCT[] buffer = array.buffer;
        buffer[offset].Tex.u = buffer[offset + 1].Tex.u = buffer[offset + 6].Tex.u = buffer[offset + 7].Tex.u = 0.0f;
        buffer[offset + 2].Tex.u = buffer[offset + 3].Tex.u = buffer[offset + 4].Tex.u = buffer[offset + 5].Tex.u = 1f;
        buffer[offset].Tex.v = buffer[offset + 2].Tex.v = buffer[offset + 4].Tex.v = buffer[offset + 6].Tex.v = 0.0f;
        buffer[offset + 1].Tex.v = buffer[offset + 3].Tex.v = buffer[offset + 5].Tex.v = buffer[offset + 7].Tex.v = 1f;
        buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 3].Col = buffer[offset + 4].Col = buffer[offset + 5].Col = buffer[offset + 6].Col = buffer[offset + 7].Col = uint.MaxValue;
        float num1 = x - w * 0.5f;
        float num2 = y - h * 0.5f;
        float num3 = num1 + w;
        buffer[offset].Pos.x = buffer[offset + 1].Pos.x = num1 - 32f;
        buffer[offset + 2].Pos.x = buffer[offset + 3].Pos.x = num1;
        buffer[offset + 4].Pos.x = buffer[offset + 5].Pos.x = num3;
        buffer[offset + 6].Pos.x = buffer[offset + 7].Pos.x = num3 + 32f;
        buffer[offset].Pos.y = buffer[offset + 2].Pos.y = buffer[offset + 4].Pos.y = buffer[offset + 6].Pos.y = num2 - 32f;
        buffer[offset + 1].Pos.y = buffer[offset + 3].Pos.y = buffer[offset + 5].Pos.y = buffer[offset + 7].Pos.y = num2;
        buffer[offset].Pos.z = buffer[offset + 1].Pos.z = buffer[offset + 2].Pos.z = buffer[offset + 3].Pos.z = buffer[offset + 4].Pos.z = buffer[offset + 5].Pos.z = buffer[offset + 6].Pos.z = buffer[offset + 7].Pos.z = -2f;
        AppMain.AoActDrawCorWide(array, 0, 8U, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
    }

    private static void aoWinSysMakeVertex01A(
      AppMain.NNS_PRIM3D_PCT_ARRAY array,
      float x,
      float y,
      float w,
      float h)
    {
        AppMain.NNS_PRIM3D_PCT[] buffer = array.buffer;
        int offset = array.offset;
        buffer[offset].Tex.u = buffer[offset + 1].Tex.u = buffer[offset + 6].Tex.u = buffer[offset + 7].Tex.u = 0.0f;
        buffer[offset + 2].Tex.u = buffer[offset + 3].Tex.u = buffer[offset + 4].Tex.u = buffer[offset + 5].Tex.u = 1f;
        buffer[offset].Tex.v = buffer[offset + 2].Tex.v = buffer[offset + 4].Tex.v = buffer[offset + 6].Tex.v = 1f;
        buffer[offset + 1].Tex.v = buffer[offset + 3].Tex.v = buffer[offset + 5].Tex.v = buffer[offset + 7].Tex.v = 1f;
        buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 3].Col = buffer[offset + 4].Col = buffer[offset + 5].Col = buffer[offset + 6].Col = buffer[offset + 7].Col = uint.MaxValue;
        float num1 = x - w * 0.5f;
        float num2 = y - h * 0.5f;
        float num3 = num1 + w;
        float num4 = num2 + h;
        buffer[offset].Pos.x = buffer[offset + 1].Pos.x = num1 - 32f;
        buffer[offset + 2].Pos.x = buffer[offset + 3].Pos.x = num1;
        buffer[offset + 4].Pos.x = buffer[offset + 5].Pos.x = num3;
        buffer[offset + 6].Pos.x = buffer[offset + 7].Pos.x = num3 + 32f;
        buffer[offset].Pos.y = buffer[offset + 2].Pos.y = buffer[offset + 4].Pos.y = buffer[offset + 6].Pos.y = num2;
        buffer[offset + 1].Pos.y = buffer[offset + 3].Pos.y = buffer[offset + 5].Pos.y = buffer[offset + 7].Pos.y = num4;
        buffer[offset].Pos.z = buffer[offset + 1].Pos.z = buffer[offset + 2].Pos.z = buffer[offset + 3].Pos.z = buffer[offset + 4].Pos.z = buffer[offset + 5].Pos.z = buffer[offset + 6].Pos.z = buffer[offset + 7].Pos.z = -2f;
        AppMain.AoActDrawCorWide(array, 0, 8U, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
    }

    private static void aoWinSysMakeVertex02A(
      AppMain.NNS_PRIM3D_PCT_ARRAY array,
      float x,
      float y,
      float w,
      float h)
    {
        AppMain.NNS_PRIM3D_PCT[] buffer = array.buffer;
        int offset = array.offset;
        buffer[offset].Tex.u = buffer[offset + 1].Tex.u = buffer[offset + 6].Tex.u = buffer[offset + 7].Tex.u = 0.0f;
        buffer[offset + 2].Tex.u = buffer[offset + 3].Tex.u = buffer[offset + 4].Tex.u = buffer[offset + 5].Tex.u = 1f;
        buffer[offset].Tex.v = buffer[offset + 2].Tex.v = buffer[offset + 4].Tex.v = buffer[offset + 6].Tex.v = 1f;
        buffer[offset + 1].Tex.v = buffer[offset + 3].Tex.v = buffer[offset + 5].Tex.v = buffer[offset + 7].Tex.v = 0.0f;
        buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 3].Col = buffer[offset + 4].Col = buffer[offset + 5].Col = buffer[offset + 6].Col = buffer[offset + 7].Col = uint.MaxValue;
        float num1 = x - w * 0.5f;
        float num2 = y - h * 0.5f;
        float num3 = num1 + w;
        float num4 = num2 + h;
        buffer[offset].Pos.x = buffer[offset + 1].Pos.x = num1 - 32f;
        buffer[offset + 2].Pos.x = buffer[offset + 3].Pos.x = num1;
        buffer[offset + 4].Pos.x = buffer[offset + 5].Pos.x = num3;
        buffer[offset + 6].Pos.x = buffer[offset + 7].Pos.x = num3 + 32f;
        buffer[offset].Pos.y = buffer[offset + 2].Pos.y = buffer[offset + 4].Pos.y = buffer[offset + 6].Pos.y = num4;
        buffer[offset + 1].Pos.y = buffer[offset + 3].Pos.y = buffer[offset + 5].Pos.y = buffer[offset + 7].Pos.y = num4 + 32f;
        buffer[offset].Pos.z = buffer[offset + 1].Pos.z = buffer[offset + 2].Pos.z = buffer[offset + 3].Pos.z = buffer[offset + 4].Pos.z = buffer[offset + 5].Pos.z = buffer[offset + 6].Pos.z = buffer[offset + 7].Pos.z = -2f;
        AppMain.AoActDrawCorWide(array, 0, 8U, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
    }


}