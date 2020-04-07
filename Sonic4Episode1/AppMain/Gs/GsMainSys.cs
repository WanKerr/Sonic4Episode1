using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static void GsMainSysSystemInitEvent(object obj)
    {
        AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gsMainSysSystemInitMain), (AppMain.GSF_TASK_PROCEDURE)null, 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "GS_SYS_INIT");
    }

    private static void gsMainSysSystemInitMain(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GsMainSysCheckLoadShaderFinished())
            return;
        AppMain.GsInitOtherStart();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gsMainSysSystemInitMain2));
    }

    private static void gsMainSysSystemInitMain2(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GsInitOtherIsInitialized())
            return;
        AppMain.mtTaskClearTcb(tcb);
        if (!AppMain.GsRebootIsTitleReboot())
            AppMain.SyDecideEvtCase((short)0);
        else
            AppMain.SyDecideEvtCase((short)2);
        AppMain.SyChangeNextEvt();
    }

    private static void GsMainSysSetSleepFlag(bool flag)
    {
        AppMain._am_sample_is_sleep = flag;
    }

    private static bool GsMainSysIsSuspendedSystem()
    {
        return AppMain._am_sample_is_suspended;
    }

    private static void GsMainSysSetSuspendedFlag(bool flag)
    {
        if (flag)
            AppMain.amFlagOn(ref AppMain.g_gs_main_sys_info.sys_flag, 1U);
        else
            AppMain.amFlagOff(ref AppMain.g_gs_main_sys_info.sys_flag, 1U);
    }

    private static bool GsMainSysGetSuspendedFlag()
    {
        bool flag = false;
        if (((int)AppMain.g_gs_main_sys_info.sys_flag & 1) != 0)
            flag = true;
        return flag;
    }

    private static void GsMainSysSetAccelFlag(bool flag)
    {
        AppMain._am_sample_is_accel = flag;
    }

    public static void GsMainSysInfoInit(AppMain.GSS_MAIN_SYS_INFO gs_main)
    {
        gs_main.Clear();
        gs_main.sys_disp_width = 480f;
        gs_main.sys_disp_height = 320f;
        gs_main.level = 1;
        gs_main.Save.Init();
    }

    private static bool GsMainSysIsStageClear(int stage_id)
    {
        if (stage_id >= 21)
        {
            uint num = (uint)(stage_id - 21);
            SSpecial instance = SSpecial.CreateInstance();
            if (instance[(int)num].GetHighScore() != 0 && instance[(int)num].IsGetEmerald() || instance[(int)num].GetFastTime() != 0)
                return true;
        }
        else
        {
            SStage instance = SStage.CreateInstance();
            uint num = stage_id < 16 ? (uint)stage_id : 16U;
            if (instance[(int)num].GetHighScore(false) != 0 || instance[(int)num].GetHighScore(true) != 0 || (instance[(int)num].GetFastTime(false) != 0 || instance[(int)num].GetFastTime(true) != 0))
                return true;
        }
        return false;
    }

    private static bool GsMainSysIsStageSonicClear(int stage_id)
    {
        if (stage_id >= 21)
        {
            uint num = (uint)(stage_id - 21);
            SSpecial instance = SSpecial.CreateInstance();
            if (instance[(int)num].GetHighScore() != 0 && instance[(int)num].IsGetEmerald() || instance[(int)num].GetFastTime() != 0)
                return true;
        }
        else
        {
            SStage instance = SStage.CreateInstance();
            uint num = stage_id < 16 ? (uint)stage_id : 16U;
            if (instance[(int)num].GetHighScore(false) != 0 || instance[(int)num].GetHighScore(true) != 0 || (instance[(int)num].GetFastTime(false) != 0 || instance[(int)num].GetFastTime(true) != 0))
                return true;
        }
        return false;
    }

    private static bool GsMainSysIsStageSuperSonicClear(int stage_id)
    {
        uint num1 = 0;
        if ((uint)stage_id > 20U)
            num1 = 0U;
        SStage instance = SStage.CreateInstance();
        uint num2 = stage_id < 16 ? (uint)stage_id : 16U;
        return instance[(int)num2].GetHighScore(true) != 0 || instance[(int)num2].GetFastTime(true) != 0;
    }

    private static bool GsMainSysIsStageGoalAsSuperSonic(int stage_id)
    {
        uint num = 0;
        if ((uint)stage_id > 20U)
            num = 0U;
        return SStage.CreateInstance()[stage_id < 16 ? stage_id : 16].IsUseSuperSonicOnce();
    }

    private static bool GsMainSysIsStageScoreUploadOnce(int stage_id)
    {
        if (stage_id >= 21)
        {
            if (SSpecial.CreateInstance()[stage_id - 21].IsScoreUploadedOnce())
                return true;
        }
        else if (SStage.CreateInstance()[stage_id < 16 ? stage_id : 16].IsScoreUploadedOnce())
            return true;
        return false;
    }

    private static bool GsMainSysIsStageTimeUploadOnce(int stage_id)
    {
        if (stage_id >= 21)
        {
            if (SSpecial.CreateInstance()[stage_id - 21].IsTimeUploadedOnce())
                return true;
        }
        else if (SStage.CreateInstance()[stage_id < 16 ? stage_id : 16].IsTimeUploadedOnce())
            return true;
        return false;
    }

    private static bool GsMainSysIsSpecialStageClearedAct(int stage_id)
    {
        SSpecial instance = SSpecial.CreateInstance();
        for (int index = 0; index < 7; ++index)
        {
            int num = (int)AppMain.gs_main_eme_get_act_no_tbl[(int)instance[index].GetEmeraldStage()];
            if (stage_id == num)
                return true;
        }
        return false;
    }

    private static void GsMainSysLoadShader(string file_path)
    {
    }

    private static bool GsMainSysCheckLoadShaderFinished()
    {
        return true;
    }


}