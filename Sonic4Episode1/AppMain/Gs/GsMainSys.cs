using gs.backup;

public partial class AppMain
{
    private static void GsMainSysSystemInitEvent(object obj)
    {
        MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gsMainSysSystemInitMain), null, 0U, ushort.MaxValue, 4096U, 0, null, "GS_SYS_INIT");
    }

    private static void gsMainSysSystemInitMain(MTS_TASK_TCB tcb)
    {
        if (!GsMainSysCheckLoadShaderFinished())
            return;
        GsInitOtherStart();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gsMainSysSystemInitMain2));
    }

    private static void gsMainSysSystemInitMain2(MTS_TASK_TCB tcb)
    {
        if (!GsInitOtherIsInitialized())
            return;
        mtTaskClearTcb(tcb);
        if (!GsRebootIsTitleReboot())
            SyDecideEvtCase(0);
        else
            SyDecideEvtCase(2);
        SyChangeNextEvt();
    }

    private static void GsMainSysSetSleepFlag(bool flag)
    {
        _am_sample_is_sleep = flag;
    }

    private static bool GsMainSysIsSuspendedSystem()
    {
        return _am_sample_is_suspended;
    }

    private static void GsMainSysSetSuspendedFlag(bool flag)
    {
        if (flag)
            amFlagOn(ref g_gs_main_sys_info.sys_flag, 1U);
        else
            amFlagOff(ref g_gs_main_sys_info.sys_flag, 1U);
    }

    private static bool GsMainSysGetSuspendedFlag()
    {
        return ((int)g_gs_main_sys_info.sys_flag & 1) != 0;
    }

    private static void GsMainSysSetAccelFlag(bool flag)
    {
        _am_sample_is_accel = flag;
    }

    public static void GsMainSysInfoInit(GSS_MAIN_SYS_INFO gs_main)
    {
        gs_main.Clear();
        gs_main.sys_disp_width = _am_draw_video.disp_width;
        gs_main.sys_disp_height = _am_draw_video.disp_height;
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
            int num = (int)gs_main_eme_get_act_no_tbl[(int)instance[index].GetEmeraldStage()];
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