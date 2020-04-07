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
    public static uint IZM_FADE_COL_PAC(uint r, uint g, uint b, uint a)
    {
        return (uint)(((int)r & (int)byte.MaxValue) << 24 | ((int)g & (int)byte.MaxValue) << 16 | ((int)b & (int)byte.MaxValue) << 8 | (int)a & (int)byte.MaxValue);
    }

    private static void IzFadeInitEasy(uint fade_set_type, uint fade_type, float time)
    {
        AppMain.IzFadeInitEasy(fade_set_type, fade_type, time, true);
    }

    private static void IzFadeInitEasy(
      uint fade_set_type,
      uint fade_type,
      float time,
      bool draw_start)
    {
        int index1 = (int)fade_type >> 1;
        int index2 = (int)fade_type & 1;
        AppMain.IzFadeInit(0, (ushort)0, (ushort)61439, 18U, fade_set_type, AppMain.iz_fade_color[index1][0], AppMain.iz_fade_color[index1][1], AppMain.iz_fade_color[index1][2], AppMain.iz_fade_alpha[index2], AppMain.iz_fade_color[index1][0], AppMain.iz_fade_color[index1][1], AppMain.iz_fade_color[index1][2], AppMain.iz_fade_alpha[index2 ^ 1], time, draw_start);
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
        AppMain.IzFadeInitEasyTask(fade_set_type, start_col_r, start_col_g, start_col_b, start_col_a, end_col_r, end_col_g, end_col_b, end_col_a, time, true);
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
        AppMain.IzFadeInit(0, (ushort)0, (ushort)61439, 18U, fade_set_type, start_col_r, start_col_g, start_col_b, start_col_a, end_col_r, end_col_g, end_col_b, end_col_a, time, draw_start);
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
        AppMain.IzFadeInit(group, pause_level, dt_prio, draw_state, fade_set_type, AppMain.iz_fade_color[index1][0], AppMain.iz_fade_color[index1][1], AppMain.iz_fade_color[index1][2], AppMain.iz_fade_alpha[index2], AppMain.iz_fade_color[index1][0], AppMain.iz_fade_color[index1][1], AppMain.iz_fade_color[index1][2], AppMain.iz_fade_alpha[index2 ^ 1], time, draw_start);
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
        AppMain.IZS_FADE_WORK work;
        if (AppMain.iz_fade_tcb == null)
        {
            AppMain.iz_fade_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.izFadeMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.izFadeDest), 2U, pause_level, 4096U, group, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.IZS_FADE_WORK()), "IZ_FADE_SYS");
            work = (AppMain.IZS_FADE_WORK)AppMain.iz_fade_tcb.work;
        }
        else
        {
            work = (AppMain.IZS_FADE_WORK)AppMain.iz_fade_tcb.work;
            conti_state = true;
        }
        AppMain.IzFadeSetWork(work, dt_prio, draw_state, fade_set_type, start_col_r, start_col_g, start_col_b, start_col_a, end_col_r, end_col_g, end_col_b, end_col_a, time, draw_start, conti_state);
    }

    private static void IzFadeExit()
    {
        if (AppMain.iz_fade_tcb == null)
            return;
        AppMain.IZS_FADE_WORK work = (AppMain.IZS_FADE_WORK)AppMain.iz_fade_tcb.work;
        AppMain.mtTaskChangeTcbProcedure(AppMain.iz_fade_tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.izFadeEndWaitMain));
        work.count = 0.0f;
        AppMain.iz_fade_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static bool IzFadeIsExe()
    {
        return AppMain.iz_fade_tcb != null;
    }

    private static bool IzFadeIsEnd()
    {
        if (AppMain.iz_fade_tcb == null)
            return true;
        AppMain.IZS_FADE_WORK work = (AppMain.IZS_FADE_WORK)AppMain.iz_fade_tcb.work;
        return (double)work.count >= (double)work.time;
    }

    private static void IzFadeRestoreDrawSetting()
    {
        AppMain.nnSetPrimitive2DAlphaFuncGL(519U, 16f);
        AppMain.nnSetPrimitive2DDepthMaskGL(true);
        AppMain.nnSetPrimitive3DDepthFuncGL(515U);
        AppMain.nnSetPrimitiveBlend(0);
    }

    private static void IzFadeSetStopUpdate1Frame(AppMain.IZS_FADE_WORK fade_work)
    {
        if (fade_work == null && AppMain.iz_fade_tcb != null)
            fade_work = (AppMain.IZS_FADE_WORK)AppMain.iz_fade_tcb.work;
        if (fade_work == null)
            return;
        fade_work.flag |= 2U;
    }

    private static void IzFadeSetWork(
      AppMain.IZS_FADE_WORK fade_work,
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
        AppMain.NNS_RGBA nnsRgba = new AppMain.NNS_RGBA();
        ushort num = 1;
        if (!conti_state)
        {
            fade_work.Clear();
            nnsRgba.r = (float)start_col_r;
            nnsRgba.g = (float)start_col_g;
            nnsRgba.b = (float)start_col_b;
            nnsRgba.a = (float)start_col_a;
        }
        else if (fade_set_type == 1U)
        {
            nnsRgba = fade_work.now_col;
            num = fade_work.vtx_no;
        }
        else
        {
            nnsRgba.r = (float)start_col_r;
            nnsRgba.g = (float)start_col_g;
            nnsRgba.b = (float)start_col_b;
            nnsRgba.a = (float)start_col_a;
        }
        fade_work.count = 0.0f;
        fade_work.start_col = nnsRgba;
        fade_work.end_col.r = (float)end_col_r;
        fade_work.end_col.g = (float)end_col_g;
        fade_work.end_col.b = (float)end_col_b;
        fade_work.end_col.a = (float)end_col_a;
        fade_work.now_col = fade_work.start_col;
        fade_work.time = time;
        fade_work.speed = 1f;
        fade_work.dt_prio = dt_prio;
        fade_work.draw_state = draw_state;
        fade_work.vtx_no = num;
        AppMain.nnMakeUnitMatrix(fade_work.mtx);
        fade_work.flag &= 4294967294U;
        if (draw_start)
            fade_work.flag |= 1U;
        AppMain.AMS_PARAM_DRAW_PRIMITIVE primParam = fade_work.prim_param;
        primParam.vtxPC2D = fade_work.vtx[(int)fade_work.vtx_no];
        primParam.mtx = fade_work.mtx;
        primParam.format2D = 1;
        primParam.type = 1;
        primParam.count = 4;
        primParam.texlist = (AppMain.NNS_TEXLIST)null;
        primParam.texId = -1;
        primParam.ablend = 1;
        primParam.zOffset = -1f;
        AppMain.amDrawGetPrimBlendParam(0, primParam);
        primParam.aTest = (short)0;
        primParam.zMask = (short)1;
        primParam.zTest = (short)0;
        for (int index = 0; index < 2; ++index)
        {
            fade_work.vtx[index][0].Pos.x = 0.0f;
            fade_work.vtx[index][0].Pos.y = 0.0f;
            fade_work.vtx[index][1].Pos.x = 0.0f;
            fade_work.vtx[index][1].Pos.y = AppMain.AMD_SCREEN_HEIGHT;
            fade_work.vtx[index][2].Pos.x = AppMain.AMD_SCREEN_WIDTH;
            fade_work.vtx[index][2].Pos.y = 0.0f;
            fade_work.vtx[index][3].Pos.x = AppMain.AMD_SCREEN_WIDTH;
            fade_work.vtx[index][3].Pos.y = AppMain.AMD_SCREEN_HEIGHT;
        }
    }

    private static void IzFadeUpdate(AppMain.IZS_FADE_WORK fade_work)
    {
        if (((int)fade_work.flag & 2) != 0)
        {
            fade_work.flag &= 4294967293U;
        }
        else
        {
            fade_work.count += fade_work.speed;
            if ((double)fade_work.count > (double)fade_work.time)
                fade_work.count = fade_work.time;
        }
        float num1 = fade_work.count / fade_work.time;
        fade_work.now_col.a = (float)((double)fade_work.start_col.a * (1.0 - (double)num1) + (double)fade_work.end_col.a * (double)num1);
        fade_work.now_col.r = (float)((double)fade_work.start_col.r * (1.0 - (double)num1) + (double)fade_work.end_col.r * (double)num1);
        fade_work.now_col.g = (float)((double)fade_work.start_col.g * (1.0 - (double)num1) + (double)fade_work.end_col.g * (double)num1);
        fade_work.now_col.b = (float)((double)fade_work.start_col.b * (1.0 - (double)num1) + (double)fade_work.end_col.b * (double)num1);
        byte num2 = (byte)AppMain.nnRoundOff(fade_work.now_col.r + 0.5f);
        byte num3 = (byte)AppMain.nnRoundOff(fade_work.now_col.g + 0.5f);
        byte num4 = (byte)AppMain.nnRoundOff(fade_work.now_col.b + 0.5f);
        byte num5 = (byte)AppMain.nnRoundOff(fade_work.now_col.a + 0.5f);
        ++fade_work.vtx_no;
        if (fade_work.vtx_no >= (ushort)2)
            fade_work.vtx_no = (ushort)0;
        AppMain.ArrayPointer<AppMain.NNS_PRIM2D_PC> arrayPointer = new AppMain.ArrayPointer<AppMain.NNS_PRIM2D_PC>(fade_work.vtx[(int)fade_work.vtx_no], 0);
        int num6 = 0;
        while (num6 < 4)
        {
            ((AppMain.NNS_PRIM2D_PC)~arrayPointer).Col = AppMain.IZM_FADE_COL_PAC((uint)num2, (uint)num3, (uint)num4, (uint)num5);
            ++num6;
            ++arrayPointer;
        }
    }

    private static void IzFadeDraw(AppMain.IZS_FADE_WORK fade_work)
    {
        fade_work.prim_param.vtxPC2D = fade_work.vtx[(int)fade_work.vtx_no];
        AppMain.amDrawPrim2D(fade_work.draw_state, fade_work.prim_param);
        if (((int)fade_work.flag & 1) == 0)
            return;
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.izFadeDrawStart_DT), fade_work.dt_prio, (object)new AppMain.IZS_FADE_DT_WORK()
        {
            draw_state = fade_work.draw_state,
            drawflag = 0U
        });
    }

    private static void izFadeDest(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static void izFadeMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.IZS_FADE_WORK work = (AppMain.IZS_FADE_WORK)tcb.work;
        AppMain.IzFadeUpdate(work);
        AppMain.IzFadeDraw(work);
    }

    private static void izFadeEndWaitMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.IZS_FADE_WORK work = (AppMain.IZS_FADE_WORK)tcb.work;
        ++work.count;
        if ((double)work.count <= 1.0)
            return;
        AppMain.mtTaskClearTcb(tcb);
    }

    private static void izFadeDrawStart_DT(AppMain.AMS_TCB am_tcb)
    {
        AppMain.IZS_FADE_DT_WORK work = (AppMain.IZS_FADE_DT_WORK)AppMain.amTaskGetWork(am_tcb);
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(work.draw_state, work.drawflag);
        AppMain.amDrawEndScene();
        AppMain.IzFadeRestoreDrawSetting();
    }

}