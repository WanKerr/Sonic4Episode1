public partial class AppMain
{
    private void AoWinSysDraw(
         int type,
         NNS_TEXLIST texlist,
         uint tex_id,
         float x,
         float y,
         float w,
         float h)
    {
        AoActDrawPre();
        aoWinSysDrawPrimitveA(texlist, tex_id, x, y, w, h);
    }

    private static void AoWinSysDrawState(
      int type,
      NNS_TEXLIST texlist,
      uint tex_id,
      float x,
      float y,
      float w,
      float h,
      uint state)
    {
        AoWinSysDrawState(type, texlist, tex_id, x, y, w, h, state, 0.0f);
    }

    private static void AoWinSysDrawState(
      int type,
      NNS_TEXLIST texlist,
      uint tex_id,
      float x,
      float y,
      float w,
      float h,
      uint state,
      float z)
    {
        aoWinSysMakeCommandA(state, texlist, tex_id, x, y, w, h, z);
    }

    private static void AoWinSysDrawTask(
      int type,
      NNS_TEXLIST texlist,
      uint tex_id,
      float x,
      float y,
      float w,
      float h,
      ushort prio)
    {
        amDrawMakeTask(new TaskProc(aoWinSysTaskDraw), prio, new AOS_WIN_DRAW_WORK()
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

    private static void aoWinSysTaskDraw(AMS_TCB tcb)
    {
        AOS_WIN_DRAW_WORK work = (AOS_WIN_DRAW_WORK)amTaskGetWork(tcb);
        AoActDrawPre();
        aoWinSysDrawPrimitveA(work.texlist, work.tex_id, work.x, work.y, work.w, work.h);
    }

    private static void aoWinSysDrawPrimitveA(
      NNS_TEXLIST texlist,
      uint tex_id,
      float x,
      float y,
      float w,
      float h)
    {
        NNS_PRIM3D_PCT_ARRAY array = amDrawAlloc_NNS_PRIM3D_PCT(8);
        NNS_PRIM3D_PCT[] buffer = array.buffer;
        int offset = array.offset;
        amDrawPushState();
        amDrawInitState();
        nnSetPrimitive3DAlphaFuncGL(519U, 0.5f);
        nnSetPrimitive3DDepthMaskGL(false);
        nnSetPrimitive3DDepthFuncGL(519U);
        nnSetPrimitiveBlend(1);
        amDrawSetFog(0);
        nnSetPrimitiveTexNum(texlist, (int)tex_id);
        nnSetPrimitiveTexState(0, 0, 1, 1);
        nnBeginDrawPrimitive3D(4, 1, 0, 0);
        aoWinSysMakeVertex00A(array, x, y, w, h);
        nnDrawPrimitive3D(1, array, 8);
        aoWinSysMakeVertex01A(array, x, y, w, h);
        nnDrawPrimitive3D(1, array, 8);
        aoWinSysMakeVertex02A(array, x, y, w, h);
        nnDrawPrimitive3D(1, array, 8);
        nnEndDrawPrimitive3D();
        amDrawPopState();
    }

    private static void aoWinSysMakeCommandA(
      uint state,
      NNS_TEXLIST texlist,
      uint tex_id,
      float x,
      float y,
      float w,
      float h,
      float z)
    {
        AMS_PARAM_DRAW_PRIMITIVE setParam = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.mtx = null;
        setParam.vtxPCT3D = null;
        setParam.format3D = 4;
        setParam.type = 1;
        setParam.count = 8;
        setParam.texlist = texlist;
        setParam.texId = (int)tex_id;
        setParam.ablend = 1;
        setParam.sortZ = z;
        setParam.aTest = 0;
        setParam.zMask = 1;
        setParam.zTest = 0;
        setParam.noSort = 1;
        setParam.uwrap = 1;
        setParam.vwrap = 1;
        NNS_PRIM3D_PCT_ARRAY array1 = amDrawAlloc_NNS_PRIM3D_PCT(8);
        aoWinSysMakeVertex00A(array1, x, y, w, h);
        setParam.vtxPCT3D = array1;
        amDrawPrimitive3D(state, setParam);
        NNS_PRIM3D_PCT_ARRAY array2 = amDrawAlloc_NNS_PRIM3D_PCT(8);
        aoWinSysMakeVertex01A(array2, x, y, w, h);
        setParam.vtxPCT3D = array2;
        amDrawPrimitive3D(state, setParam);
        NNS_PRIM3D_PCT_ARRAY array3 = amDrawAlloc_NNS_PRIM3D_PCT(8);
        aoWinSysMakeVertex02A(array3, x, y, w, h);
        setParam.vtxPCT3D = array3;
        amDrawPrimitive3D(state, setParam);
        GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    private static void aoWinSysMakeVertex00A(
      NNS_PRIM3D_PCT_ARRAY array,
      float x,
      float y,
      float w,
      float h)
    {
        int offset = array.offset;
        NNS_PRIM3D_PCT[] buffer = array.buffer;
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
        AoActDrawCorWide(array, 0, 8U, AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
    }

    private static void aoWinSysMakeVertex01A(
      NNS_PRIM3D_PCT_ARRAY array,
      float x,
      float y,
      float w,
      float h)
    {
        NNS_PRIM3D_PCT[] buffer = array.buffer;
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
        AoActDrawCorWide(array, 0, 8U, AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
    }

    private static void aoWinSysMakeVertex02A(
      NNS_PRIM3D_PCT_ARRAY array,
      float x,
      float y,
      float w,
      float h)
    {
        NNS_PRIM3D_PCT[] buffer = array.buffer;
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
        AoActDrawCorWide(array, 0, 8U, AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
    }


}