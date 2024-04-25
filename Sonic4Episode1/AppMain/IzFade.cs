public partial class AppMain
{
    public static uint IZM_FADE_COL_PAC(uint r, uint g, uint b, uint a)
    {
        return (uint)(((int)r & byte.MaxValue) << 24 | ((int)g & byte.MaxValue) << 16 | ((int)b & byte.MaxValue) << 8 | (int)a & byte.MaxValue);
    }

    private static void IzFadeInitEasy(uint fade_set_type, uint fade_type, float time)
    {
        IzFadeInitEasy(fade_set_type, fade_type, time, true);
    }

    private static void IzFadeInitEasy(
      uint fade_set_type,
      uint fade_type,
      float time,
      bool draw_start)
    {
        int index1 = (int)fade_type >> 1;
        int index2 = (int)fade_type & 1;
        IzFadeInit(0, 0, IZD_FADE_DT_PRIO_DEF, 18U, fade_set_type, iz_fade_color[index1][0], iz_fade_color[index1][1], iz_fade_color[index1][2], iz_fade_alpha[index2], iz_fade_color[index1][0], iz_fade_color[index1][1], iz_fade_color[index1][2], iz_fade_alpha[index2 ^ 1], time, draw_start);
    }

    private static void IzFadeInitEasyTask(
      uint fade_set_type,
      byte start_col_r,
      byte start_col_g,
      byte start_col_b,
      byte start_col_a,
      byte end_col_r,
      byte end_col_g,
      byte end_col_b,
      byte end_col_a,
      float time)
    {
        IzFadeInitEasyTask(fade_set_type, start_col_r, start_col_g, start_col_b, start_col_a, end_col_r, end_col_g, end_col_b, end_col_a, time, true);
    }

    private static void IzFadeInitEasyTask(
      uint fade_set_type,
      byte start_col_r,
      byte start_col_g,
      byte start_col_b,
      byte start_col_a,
      byte end_col_r,
      byte end_col_g,
      byte end_col_b,
      byte end_col_a,
      float time,
      bool draw_start)
    {
        IzFadeInit(0, 0, IZD_FADE_DT_PRIO_DEF, 18U, fade_set_type, start_col_r, start_col_g, start_col_b, start_col_a, end_col_r, end_col_g, end_col_b, end_col_a, time, draw_start);
    }

    private static void IzFadeInitEasyColor(
      int group,
      ushort pause_level,
      ushort dt_prio,
      uint draw_state,
      uint fade_set_type,
      uint fade_type,
      float time,
      bool draw_start)
    {
        int index1 = (int)fade_type >> 1;
        int index2 = (int)fade_type & 1;
        IzFadeInit(group, pause_level, dt_prio, draw_state, fade_set_type, iz_fade_color[index1][0], iz_fade_color[index1][1], iz_fade_color[index1][2], iz_fade_alpha[index2], iz_fade_color[index1][0], iz_fade_color[index1][1], iz_fade_color[index1][2], iz_fade_alpha[index2 ^ 1], time, draw_start);
    }

    private static void IzFadeInit(
      int group,
      ushort pause_level,
      ushort dt_prio,
      uint draw_state,
      uint fade_set_type,
      byte start_col_r,
      byte start_col_g,
      byte start_col_b,
      byte start_col_a,
      byte end_col_r,
      byte end_col_g,
      byte end_col_b,
      byte end_col_a,
      float time,
      bool draw_start)
    {
        bool conti_state = false;
        IZS_FADE_WORK work;
        if (iz_fade_tcb == null)
        {
            iz_fade_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(izFadeMain), new GSF_TASK_PROCEDURE(izFadeDest), 2U, pause_level, 4096U, group, () => new IZS_FADE_WORK(), "IZ_FADE_SYS");
            work = (IZS_FADE_WORK)iz_fade_tcb.work;
        }
        else
        {
            work = (IZS_FADE_WORK)iz_fade_tcb.work;
            conti_state = true;
        }
        IzFadeSetWork(work, dt_prio, draw_state, fade_set_type, start_col_r, start_col_g, start_col_b, start_col_a, end_col_r, end_col_g, end_col_b, end_col_a, time, draw_start, conti_state);
    }

    private static void IzFadeExit()
    {
        if (iz_fade_tcb == null)
            return;
        IZS_FADE_WORK work = (IZS_FADE_WORK)iz_fade_tcb.work;
        mtTaskChangeTcbProcedure(iz_fade_tcb, new GSF_TASK_PROCEDURE(izFadeEndWaitMain));
        work.count = 0.0f;
        iz_fade_tcb = null;
    }

    private static bool IzFadeIsExe()
    {
        return iz_fade_tcb != null;
    }

    private static bool IzFadeIsEnd()
    {
        if (iz_fade_tcb == null)
            return true;
        IZS_FADE_WORK work = (IZS_FADE_WORK)iz_fade_tcb.work;
        return work.count >= (double)work.time;
    }

    private static void IzFadeRestoreDrawSetting()
    {
        nnSetPrimitive2DAlphaFuncGL(519U, 16f);
        nnSetPrimitive2DDepthMaskGL(true);
        nnSetPrimitive3DDepthFuncGL(515U);
        nnSetPrimitiveBlend(0);
    }

    private static void IzFadeSetStopUpdate1Frame(IZS_FADE_WORK fade_work)
    {
        if (fade_work == null && iz_fade_tcb != null)
            fade_work = (IZS_FADE_WORK)iz_fade_tcb.work;
        if (fade_work == null)
            return;
        fade_work.flag |= 2U;
    }

    private static void IzFadeSetWork(
      IZS_FADE_WORK fade_work,
      ushort dt_prio,
      uint draw_state,
      uint fade_set_type,
      byte start_col_r,
      byte start_col_g,
      byte start_col_b,
      byte start_col_a,
      byte end_col_r,
      byte end_col_g,
      byte end_col_b,
      byte end_col_a,
      float time,
      bool draw_start,
      bool conti_state)
    {
        NNS_RGBA nnsRgba = new NNS_RGBA();
        ushort num = 1;
        if (!conti_state)
        {
            fade_work.Clear();
            nnsRgba.r = start_col_r;
            nnsRgba.g = start_col_g;
            nnsRgba.b = start_col_b;
            nnsRgba.a = start_col_a;
        }
        else if (fade_set_type == 1U)
        {
            nnsRgba = fade_work.now_col;
            num = fade_work.vtx_no;
        }
        else
        {
            nnsRgba.r = start_col_r;
            nnsRgba.g = start_col_g;
            nnsRgba.b = start_col_b;
            nnsRgba.a = start_col_a;
        }
        fade_work.count = 0.0f;
        fade_work.start_col = nnsRgba;
        fade_work.end_col.r = end_col_r;
        fade_work.end_col.g = end_col_g;
        fade_work.end_col.b = end_col_b;
        fade_work.end_col.a = end_col_a;
        fade_work.now_col = fade_work.start_col;
        fade_work.time = time;
        fade_work.speed = 1f;
        fade_work.dt_prio = dt_prio;
        fade_work.draw_state = draw_state;
        fade_work.vtx_no = num;
        nnMakeUnitMatrix(fade_work.mtx);
        fade_work.flag &= 4294967294U;
        if (draw_start)
            fade_work.flag |= 1U;
        AMS_PARAM_DRAW_PRIMITIVE primParam = fade_work.prim_param;
        primParam.vtxPC2D = fade_work.vtx[fade_work.vtx_no];
        primParam.mtx = fade_work.mtx;
        primParam.format2D = 1;
        primParam.type = 1;
        primParam.count = 4;
        primParam.texlist = null;
        primParam.texId = -1;
        primParam.ablend = 1;
        primParam.zOffset = -1f;
        amDrawGetPrimBlendParam(0, primParam);
        primParam.aTest = 0;
        primParam.zMask = 1;
        primParam.zTest = 0;
        for (int index = 0; index < 2; ++index)
        {
            fade_work.vtx[index][0].Pos.x = 0.0f;
            fade_work.vtx[index][0].Pos.y = 0.0f;
            fade_work.vtx[index][1].Pos.x = 0.0f;
            fade_work.vtx[index][1].Pos.y = AMD_SCREEN_HEIGHT;
            fade_work.vtx[index][2].Pos.x = AMD_SCREEN_WIDTH;
            fade_work.vtx[index][2].Pos.y = 0.0f;
            fade_work.vtx[index][3].Pos.x = AMD_SCREEN_WIDTH;
            fade_work.vtx[index][3].Pos.y = AMD_SCREEN_HEIGHT;
        }
    }

    private static void IzFadeUpdate(IZS_FADE_WORK fade_work)
    {
        if (((int)fade_work.flag & 2) != 0)
        {
            fade_work.flag &= 4294967293U;
        }
        else
        {
            fade_work.count += fade_work.speed;
            if (fade_work.count > (double)fade_work.time)
                fade_work.count = fade_work.time;
        }
        float num1 = fade_work.count / fade_work.time;
        fade_work.now_col.a = (float)(fade_work.start_col.a * (1.0 - num1) + fade_work.end_col.a * (double)num1);
        fade_work.now_col.r = (float)(fade_work.start_col.r * (1.0 - num1) + fade_work.end_col.r * (double)num1);
        fade_work.now_col.g = (float)(fade_work.start_col.g * (1.0 - num1) + fade_work.end_col.g * (double)num1);
        fade_work.now_col.b = (float)(fade_work.start_col.b * (1.0 - num1) + fade_work.end_col.b * (double)num1);
        byte num2 = (byte)nnRoundOff(fade_work.now_col.r + 0.5f);
        byte num3 = (byte)nnRoundOff(fade_work.now_col.g + 0.5f);
        byte num4 = (byte)nnRoundOff(fade_work.now_col.b + 0.5f);
        byte num5 = (byte)nnRoundOff(fade_work.now_col.a + 0.5f);
        ++fade_work.vtx_no;
        if (fade_work.vtx_no >= 2)
            fade_work.vtx_no = 0;
        ArrayPointer<NNS_PRIM2D_PC> arrayPointer = new ArrayPointer<NNS_PRIM2D_PC>(fade_work.vtx[fade_work.vtx_no], 0);
        int num6 = 0;
        while (num6 < 4)
        {
            (~arrayPointer).Col = IZM_FADE_COL_PAC(num2, num3, num4, num5);
            ++num6;
            ++arrayPointer;
        }
    }

    private static void IzFadeDraw(IZS_FADE_WORK fade_work)
    {
        fade_work.prim_param.vtxPC2D = fade_work.vtx[fade_work.vtx_no];
        amDrawPrim2D(fade_work.draw_state, fade_work.prim_param);
        if (((int)fade_work.flag & 1) == 0)
            return;
        amDrawMakeTask(new TaskProc(izFadeDrawStart_DT), fade_work.dt_prio, new IZS_FADE_DT_WORK()
        {
            draw_state = fade_work.draw_state,
            drawflag = 0U
        });
    }

    private static void izFadeDest(MTS_TASK_TCB tcb)
    {
    }

    private static void izFadeMain(MTS_TASK_TCB tcb)
    {
        IZS_FADE_WORK work = (IZS_FADE_WORK)tcb.work;
        IzFadeUpdate(work);
        IzFadeDraw(work);
    }

    private static void izFadeEndWaitMain(MTS_TASK_TCB tcb)
    {
        IZS_FADE_WORK work = (IZS_FADE_WORK)tcb.work;
        ++work.count;
        if (work.count <= 1.0)
            return;
        mtTaskClearTcb(tcb);
    }

    private static void izFadeDrawStart_DT(AMS_TCB am_tcb)
    {
        IZS_FADE_DT_WORK work = (IZS_FADE_DT_WORK)amTaskGetWork(am_tcb);
        AoActDrawPre();
        amDrawExecCommand(work.draw_state, work.drawflag);
        amDrawEndScene();
        IzFadeRestoreDrawSetting();
    }

}