using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    public static short GMD_OBJ_LCD_X
    {
        get
        {
            return (short)(0.674383342266083 * (double)AppMain.GSD_DISP_WIDTH);
        }
    }

    public static short GMD_OBJ_LCD_Y
    {
        get
        {
            return (short)(0.674383342266083 * (double)AppMain.GSD_DISP_HEIGHT);
        }
    }

    public static bool GMM_MAIN_STAGE_IS_BOSS()
    {
        return (int)AppMain.g_gs_main_sys_info.stage_id < AppMain.g_gm_gamedat_stage_type_tbl.Length && AppMain.g_gm_gamedat_stage_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == AppMain.GSE_MAIN_STAGE_TYPE.GSD_MAIN_STAGE_TYPE_BOSS;
    }

    public static bool GMM_MAIN_STAGE_IS_SS()
    {
        return (int)AppMain.g_gs_main_sys_info.stage_id < AppMain.g_gm_gamedat_stage_type_tbl.Length && AppMain.g_gm_gamedat_stage_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == AppMain.GSE_MAIN_STAGE_TYPE.GSD_MAIN_STAGE_TYPE_SS;
    }

    public static bool GMM_MAIN_STAGE_IS_ENDING()
    {
        return AppMain.g_gs_main_sys_info.game_mode == 2;
    }

    private static int GMM_MAIN_GET_ZONE_TYPE()
    {
        return AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
    }

    public static bool GMM_MAIN_USE_SUPER_SONIC()
    {
        return ((int)AppMain.g_gm_main_system.game_flag & 524288) != 0;
    }

    public static bool GMM_MAIN_GOAL_AS_SUPER_SONIC()
    {
        return ((int)AppMain.g_gm_main_system.game_flag & 33554432) != 0;
    }

    private static bool GmMainIsWaterLevel()
    {
        return AppMain.g_gm_main_system.water_level != ushort.MaxValue;
    }

    public static void GmMainClearSuspendedPause()
    {
        AppMain.g_gs_main_sys_info.game_flag &= 3758096383U;
    }

    private static void GmMainGSInit()
    {
        AppMain.g_gs_main_sys_info.game_flag &= 4294967009U;
        AppMain.g_gs_main_sys_info.clear_ring = 0U;
        AppMain.g_gs_main_sys_info.clear_score = 0U;
        AppMain.g_gs_main_sys_info.clear_time = 0;
    }

    private static void GmMainGSRetryInit()
    {
        AppMain._bossFinishThread = true;
        AppMain.g_gs_main_sys_info.rest_player_num = AppMain.g_gm_main_system.player_rest_num[0];
        AppMain.g_gs_main_sys_info.game_flag &= 4294967025U;
        AppMain.g_gs_main_sys_info.clear_ring = 0U;
        AppMain.g_gs_main_sys_info.clear_score = 0U;
        AppMain.g_gs_main_sys_info.clear_time = 0;
    }

    private void GmMainInit(object arg)
    {
        AppMain.GsFontRelease();
        //AppMain.CPadEmu.CreateInstance().Create(AppMain.CPadEmu.EMode.Game);
        if (AppMain.GsTrialIsTrial())
            AppMain.g_gs_main_sys_info.stage_id = (ushort)AppMain.nextDemoLevel;
        if (SaveState.shouldResume())
            SaveState.resumeStageState();
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 4) == 0)
            AppMain.gmMainSysInit();
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)9)
            AppMain.ObjInit((byte)4, (ushort)61435, (byte)0, (short)((double)AppMain.GMD_OBJ_LCD_X * 1.42), (short)((double)AppMain.GMD_OBJ_LCD_X * 1.42), (float)AppMain.GSD_DISP_WIDTH, (float)AppMain.GSD_DISP_HEIGHT);
        else
            AppMain.ObjInit((byte)4, (ushort)61435, (byte)0, AppMain.GMD_OBJ_LCD_X, AppMain.GMD_OBJ_LCD_Y, (float)AppMain.GSD_DISP_WIDTH, (float)AppMain.GSD_DISP_HEIGHT);
        AppMain.ObjDataAlloc(994);
        AppMain.ObjDrawESEffectSystemInit((ushort)0, 20480U, 5U);
        this.amTrailEFInitialize();
        AppMain.ObjDrawSetNNCommandStateTbl(0U, 1U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(1U, 2U, false);
        AppMain.ObjDrawSetNNCommandStateTbl(2U, 3U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(3U, 5U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(4U, 11U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(5U, 12U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(6U, 15U, false);
        AppMain.ObjDrawSetNNCommandStateTbl(7U, 0U, false);
        AppMain.ObjDrawSetNNCommandStateTbl(8U, 16U, true);
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)10 || AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
            AppMain.ObjDrawSetNNCommandStateTbl(9U, 17U, true);
        else
            AppMain.ObjDrawSetNNCommandStateTbl(9U, uint.MaxValue, false);
        AppMain.ObjDrawSetNNCommandStateTbl(10U, 9U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(11U, 4U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(12U, 8U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(13U, 7U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(14U, 10U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(15U, 6U, true);
        AppMain.AoActSysClearPeak();
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 16) == 0)
            AppMain.gmMainLoad(0);
        else
            AppMain.gmMainRebuild();
    }

    private static void GmMainEnd()
    {
        AppMain.GmPadVibExit();
        if (AppMain.g_gm_main_system.pre_tcb != null)
        {
            AppMain.mtTaskClearTcb(AppMain.g_gm_main_system.pre_tcb);
            AppMain.g_gm_main_system.pre_tcb = (AppMain.MTS_TASK_TCB)null;
        }
        if (AppMain.g_gm_main_system.post_tcb != null)
        {
            AppMain.mtTaskClearTcb(AppMain.g_gm_main_system.post_tcb);
            AppMain.g_gm_main_system.post_tcb = (AppMain.MTS_TASK_TCB)null;
        }
        AppMain.amTrailEFDeleteGroup((ushort)1);
        AppMain.GmPlyEfctTrailSysExit();
        AppMain.g_obj.ppPre = (AppMain.OBJECT_Delegate)null;
        AppMain.ObjObjectClearAllObject();
        AppMain.ObjPreExit();
        AppMain.GmMapExit();
        AppMain.GmFixExit();
        AppMain.GmPauseExit();
        AppMain.GmRingExit();
        AppMain.GmCameraExit();
        AppMain.GmSoundExit();
        AppMain.GmMapFarExit();
        AppMain.GmDecoExit();
        AppMain.GmWaterSurfaceExit();
        AppMain.GmEventMgrExit();
        AppMain.ObjDrawESEffectSystemExit();
        AppMain.GmClearDemoExit();
        AppMain.GmOverExit();
        AppMain.GmSplStageExit();
        AppMain.GmEndingExit();
        AppMain.GmStartDemoExit();
        AppMain.GmStartMsgExit();
        CPadVirtualPad.CreateInstance().Release();
        AppMain.CPadPolarHandle.CreateInstance().Release();
        AppMain.GsMainSysSetSleepFlag(true);
        AppMain.GsMainSysSetAccelFlag(false);
    }

    private static void GmMainExit()
    {
        AppMain.GmMainEnd();
        AppMain.gmMainDataRelease();
    }

    private static void GmMainRestartExit()
    {
        AppMain.GmMainEnd();
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)16)
        {
            AppMain.gmMainObjectRelease();
        }
        else
        {
            AppMain.g_obj.flag |= 1073741824U;
            AppMain.ObjExit();
            AppMain.gmMainObjectRelease();
        }
    }

    private static void GmMainExitForStaffroll()
    {
        AppMain.gmMainDataRelease();
    }

    private static bool GmMainCheckExeTimerCount()
    {
        return ((int)AppMain.g_gm_main_system.game_flag & 1024) != 0;
    }

    public static bool GmMainIsDrawEnable()
    {
        return AppMain._am_sample_draw_enable;
    }

    public static float GmMainGetDrawMotionSpeed()
    {
        return (float)AppMain._am_sample_count;
    }

    private static ushort GmMainGetObjectRotation()
    {
        ushort num = 0;
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        if (obsCamera != null)
            num = (ushort)-obsCamera.roll;
        return num;
    }

    private static uint GmMainGetLightColor()
    {
        uint num = 3772834047;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
            num = 3767175935U;
        else if (AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
            num = 2593823487U;
        return num;
    }

    private static uint GmMainGetLightColorABGR()
    {
        uint num = 4292927712;
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
            num = 4287269600U;
        else if (AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
            num = 4288322202U;
        return num;
    }

    private static OpenGL.glArray4f GmMainGetLightColorArray4f()
    {
        switch (AppMain.g_gs_main_sys_info.stage_id)
        {
            case 2:
            case 3:
                return AppMain.LightColor[1];
            case 14:
                return AppMain.LightColor[2];
            default:
                return AppMain.LightColor[0];
        }
    }

    public static void GmMainDatLoadBossBattleStart(int boss_type)
    {
        if (AppMain.g_gm_main_system.boss_load_no == boss_type)
            return;
        if (AppMain.GmMainDatReleaseBossBattleReleaseCheck())
            AppMain.GmGameDatReleaseBossBattleExit();
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)16 && boss_type > 0 && boss_type < 4)
        {
            if (AppMain._bossThread != null && AppMain._bossThread.IsAlive)
                AppMain._bossThread.Join();
            AppMain._bossThread = (Thread)null;
            AppMain._bossThread = new Thread(new ParameterizedThreadStart(AppMain._GmMainDatLoadBossBattleStart));
            AppMain._bossFinishThread = false;
            AppMain._bossThread.Start((object)boss_type);
        }
        else
        {
            AppMain._bossFinishThread = true;
            AppMain.GmGameDatLoadBoosBattleInit(boss_type);
            AppMain.gm_main_load_bossbattle_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataLoadBoosBattleMgr_LoadWait), (AppMain.GSF_TASK_PROCEDURE)null, 0U, ushort.MaxValue, 2048U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_MAIN_LOAD_BB_MGR_WORK()), "GM_LOAD_BBM");
            AppMain.GMS_MAIN_LOAD_BB_MGR_WORK work = (AppMain.GMS_MAIN_LOAD_BB_MGR_WORK)AppMain.gm_main_load_bossbattle_tcb.work;
            work.boss_type = boss_type;
            work.b_end = false;
            AppMain.g_gm_main_system.game_flag |= 2097152U;
        }
    }

    public static void _GmMainDatLoadBossBattleStart(object o)
    {
        int boss_type = (int)o;
        bool flag = false;
        do
        {
            int x = AppMain.g_gm_main_system.ply_work[0].obj_work.pos.x;
            switch (boss_type)
            {
                case 0:
                    flag = true;
                    break;
                case 1:
                    if (x >= 10424705)
                    {
                        flag = true;
                        break;
                    }
                    break;
                case 2:
                    if (x >= 23266972)
                    {
                        flag = true;
                        break;
                    }
                    break;
                case 3:
                    if (x >= 37948249)
                    {
                        flag = true;
                        break;
                    }
                    break;
                case 4:
                    if (x >= 52666673)
                    {
                        flag = true;
                        break;
                    }
                    break;
            }
            if (AppMain._bossFinishThread)
                return;
            if (!flag)
                Thread.Sleep(30);
        }
        while (!flag);
        AppMain.GmGameDatLoadBoosBattleInit(boss_type);
        AppMain.gm_main_load_bossbattle_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataLoadBoosBattleMgr_LoadWait), (AppMain.GSF_TASK_PROCEDURE)null, 0U, ushort.MaxValue, 2048U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_MAIN_LOAD_BB_MGR_WORK()), "GM_LOAD_BBM");
        AppMain.GMS_MAIN_LOAD_BB_MGR_WORK work = (AppMain.GMS_MAIN_LOAD_BB_MGR_WORK)AppMain.gm_main_load_bossbattle_tcb.work;
        work.boss_type = boss_type;
        work.b_end = false;
        AppMain.g_gm_main_system.game_flag |= 2097152U;
        AppMain._bossFinishThread = true;
    }

    private static bool GmMainDatLoadBossBattleLoadCheck()
    {
        return AppMain.GmMainDatLoadBossBattleLoadCheck(0);
    }

    private static bool GmMainDatLoadBossBattleLoadCheck(int boss_type)
    {
        return AppMain.g_gm_main_system.boss_load_no != -1 && ((int)AppMain.g_gm_main_system.game_flag & 4194304) == 0 && (boss_type == -1 || boss_type == AppMain.g_gm_main_system.boss_load_no);
    }

    private static bool GmMainDatLoadBossBattleLoadNowCheck()
    {
        return ((int)AppMain.g_gm_main_system.game_flag & 2097152) != 0;
    }


    private static int GmMainKeyCheckPauseKeyOn()
    {
        int num1 = -1;
        uint num2 = 215;
        uint num3 = 0;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            num2 = 390U;
            num3 = 0U;
        }
        else if (AppMain.GsGetMainSysInfo().game_mode == 1)
        {
            num2 = 275U;
            num3 = 0U;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (AppMain.amTpIsTouchOn(index))
            {
                ushort num4 = AppMain._am_tp_touch[index].on[0];
                ushort num5 = AppMain._am_tp_touch[index].on[1];
                if ((uint)num4 >= num2 && (uint)num4 <= num2 + 115U && ((uint)num5 >= num3 && (uint)num5 <= num3 + 60U))
                {
                    ushort num6 = AppMain._am_tp_touch[index].push[0];
                    ushort num7 = AppMain._am_tp_touch[index].push[1];
                    if ((uint)num6 >= num2 && (uint)num6 <= num2 + 115U && ((uint)num7 >= num3 && (uint)num7 <= num3 + 60U))
                    {
                        num1 = index;
                        break;
                    }
                }
            }
        }
        return num1;
    }

    private static int GmMainKeyCheckPauseKeyPush()
    {
        int num1 = -1;
        uint num2 = 215;
        uint num3 = 0;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            num2 = 390U;
            num3 = 0U;
        }
        else if (AppMain.GsGetMainSysInfo().game_mode == 1)
        {
            num2 = 275U;
            num3 = 0U;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (AppMain.amTpIsTouchPush(index))
            {
                ushort num4 = AppMain._am_tp_touch[index].push[0];
                ushort num5 = AppMain._am_tp_touch[index].push[1];
                if ((uint)num4 >= num2 && (uint)num4 <= num2 + 115U && ((uint)num5 >= num3 && (uint)num5 <= num3 + 60U))
                {
                    num1 = index;
                    break;
                }
            }
        }
        if (!AppMain.g_pause_flag)
            return num1;
        AppMain.g_pause_flag = false;
        return 1;
    }

    private static void gmMainSysInit()
    {
        AppMain.g_gm_main_system.Clear();
        AppMain.g_gm_main_system.player_rest_num[0] = AppMain.g_gs_main_sys_info.rest_player_num;
        if (AppMain.g_gm_main_system.player_rest_num[0] <= 0U)
            AppMain.g_gm_main_system.player_rest_num[0] = 1U;
        AppMain.mtMathSRand((uint)((double)AppMain.nnRandom() * (double)short.MaxValue));
    }

    private static void gmMainLoad(int load_proc)
    {
        short[] char_id_list = new short[1];
        AppMain.GmMainClearSuspendedPause();
        AppMain.DmLoadingStart();
        AppMain.GmPauseMenuLoadStart();
        for (int index = 0; index < 1; ++index)
            char_id_list[index] = (short)AppMain.g_gs_main_sys_info.char_id[index];
        AppMain.GmGameDatLoadInit(load_proc, AppMain.g_gs_main_sys_info.stage_id, char_id_list);
        AppMain.gm_main_load_wait_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataLoadWait), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataLoadDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "GM_LOAD_WAIT");
    }

    private static void gmMainDataLoadWait(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmMainUpdateSuspendedPause();
        if (!AppMain.GmPauseMenuLoadIsFinished() || AppMain.GmGameDatLoadCheck() != AppMain.GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_COMPLETE)
            return;
        if (!SaveState.shouldResume())
            SaveState.deleteSave();
        AppMain.GmGameDatBuildInit();
        AppMain.GmGameDatBuildStandard();
        AppMain.GmGameDatBuildArea();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataBuildWait));
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    private static void gmMainDataBuildWait(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmMainUpdateSuspendedPause();
        if (!AppMain.GmGameDatBuildStandardCheck() || !AppMain.GmGameDatBuildAreaCheck() || (!AppMain.SoundPartialCache(5) || !AppMain.GsSoundPrepareBGMForLevel((int)AppMain.g_gs_main_sys_info.stage_id)))
            return;
        AppMain.GmGameDatLoadExit();
        AppMain.g_gs_main_sys_info.game_flag |= 16U;
        AppMain.DmLoadingSetLoadComplete();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataLoadingEndWait));
    }

    private static void gmMainDataLoadingEndWait(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmMainUpdateSuspendedPause();
        if (!AppMain.DmLoadingIsExit())
            return;
        AppMain.gmMainGameStart();
        AppMain.mtTaskClearTcb(tcb);
    }

    private static void gmMainDataLoadDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gm_main_load_wait_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmMainRebuild()
    {
        AppMain.gm_main_load_wait_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainRebuildWait), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainRebuildDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "GM_REBUILD_WAIT");
        AppMain.GmGameDatReBuildRestart();
    }

    private static void gmMainRebuildWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmGameDatReBuildRestartCheck())
            return;
        AppMain.gmMainGameStart();
        AppMain.mtTaskClearTcb(tcb);
    }

    private static void gmMainRebuildDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gm_main_load_wait_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmMainDataRelease()
    {
        AppMain.gm_main_release_wait_tcb = AppMain.MTM_TASK_MAKE_TCB(AppMain.g_gs_main_sys_info.stage_id == (ushort)16 ? new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataFlushExitFinalClearObjWait) : new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataFlushExitWait), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataReleaseDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "GM_UNLOAD_WAIT");
    }

    private static void gmMainDataFlushExitFinalClearObjWait(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain._bossFinishThread = true;
        if (!AppMain.ObjObjectCheckClearAllObject())
            return;
        AppMain.GsSoundReset();
        AppMain.GSF_TASK_PROCEDURE proc;
        if (AppMain.GmMainDatLoadBossBattleLoadCheck())
        {
            proc = new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataFlushExitFinalWait);
            AppMain.GmGameDatLoadBossBattleExit();
            AppMain.GmGameDatReleaseBossBattleStart(AppMain.g_gm_main_system.boss_load_no);
        }
        else if (AppMain.GmMainDatLoadBossBattleLoadNowCheck())
            proc = new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataFlushExitFinalLoadWait);
        else if (AppMain.GmMainDatReleaseBossBattleReleaseNowCheck())
        {
            proc = new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataFlushExitFinalWait);
            AppMain.GmGameDatLoadBossBattleExit();
        }
        else
        {
            proc = new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataFlushExitWait);
            AppMain.GmGameDatReleaseBossBattleExit();
        }
        AppMain.mtTaskChangeTcbProcedure(tcb, proc);
    }

    private static void gmMainDataFlushExitFinalLoadWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmMainDatLoadBossBattleLoadCheck())
            return;
        AppMain.GmGameDatLoadBossBattleExit();
        AppMain.GmGameDatReleaseBossBattleStart(AppMain.g_gm_main_system.boss_load_no);
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataFlushExitFinalWait));
    }

    private static void gmMainDataFlushExitFinalWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmMainDatReleaseBossBattleReleaseCheck())
            return;
        AppMain.GmGameDatReleaseBossBattleExit();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataFlushExitWait));
    }

    private static void gmMainDataFlushExitWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.ObjObjectCheckClearAllObject())
            return;
        AppMain.GsSoundReset();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataFlushWait));
        AppMain.GmGameDatFlushInit();
        AppMain.GmGameDatFlushArea();
        AppMain.GmGameDatFlushStandard();
    }

    private static void gmMainDataFlushWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmGameDatFlushStandardCheck() || !AppMain.GmGameDatFlushAreaCheck())
            return;
        AppMain.OBS_DATA_WORK[] pData = AppMain.g_obj.pData;
        for (int index = 0; index < AppMain.g_obj.data_max; ++index)
        {
            if (pData[index].pData != null && ((int)pData[index].num & 32768) == 0)
                pData[index].pData = (object)null;
        }
        AppMain.ObjExit();
        AppMain.GmGameDatReleaseStandard();
        AppMain.GmGameDatReleaseArea();
        AppMain.GmPauseMenuRelease();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataReleaseWait));
    }

    private static void gmMainDataReleaseWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmGameDatReleaseCheck() || AppMain.ObjIsExitWait())
            return;
        AppMain.g_gs_main_sys_info.game_flag &= 4294967279U;
        AppMain.mtTaskClearTcb(tcb);
        AppMain.g_gs_main_sys_info.rest_player_num = AppMain.g_gm_main_system.player_rest_num[0];
        if (AppMain.g_gs_main_sys_info.rest_player_num <= 0U)
            AppMain.g_gs_main_sys_info.rest_player_num = 3U;
        if (AppMain.g_gs_main_sys_info.game_mode == 0 && ((int)AppMain.g_gs_main_sys_info.game_flag & 2) != 0 && AppMain.g_gs_main_sys_info.stage_id == (ushort)16)
        {
            AppMain.g_gs_main_sys_info.stage_id = (ushort)28;
            AppMain.g_gs_main_sys_info.char_id[0] = 0;
            AppMain.g_gs_main_sys_info.game_mode = 2;
            AppMain.GmMainGSInit();
        }
        AppMain.SyChangeNextEvt();
    }

    private static void gmMainDataReleaseDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gm_main_release_wait_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmMainObjectRelease()
    {
        AppMain.gm_main_release_wait_tcb = AppMain.MTM_TASK_MAKE_TCB(AppMain.g_gs_main_sys_info.stage_id == (ushort)16 ? new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainObjectReleaseFinalClearObjWait) : new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainObjectReleaseWait), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainObjectReleaseDest), 0U, ushort.MaxValue, 4096U, 0, (AppMain.TaskWorkFactoryDelegate)null, "GM_UNLOAD_OBJ_WAIT");
    }

    private static void gmMainObjectReleaseFinalClearObjWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.ObjObjectCheckClearAllObject())
            return;
        AppMain.GSF_TASK_PROCEDURE proc;
        if (AppMain.GmMainDatLoadBossBattleLoadCheck())
        {
            proc = new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainObjectReleaseFinalWait);
            AppMain.GmGameDatLoadBossBattleExit();
            AppMain.GmGameDatReleaseBossBattleStart(AppMain.g_gm_main_system.boss_load_no);
        }
        else if (AppMain.GmMainDatLoadBossBattleLoadNowCheck())
            proc = new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainObjectReleaseFinalLoadWait);
        else if (AppMain.GmMainDatReleaseBossBattleReleaseNowCheck())
        {
            proc = new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainObjectReleaseFinalWait);
            AppMain.GmGameDatLoadBossBattleExit();
        }
        else
        {
            proc = new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainObjectReleaseWait);
            AppMain.GmGameDatReleaseBossBattleExit();
            AppMain.g_obj.flag |= 1073741824U;
            AppMain.ObjExit();
        }
        AppMain.mtTaskChangeTcbProcedure(tcb, proc);
    }

    private static void gmMainObjectReleaseFinalLoadWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmMainDatLoadBossBattleLoadCheck())
            return;
        AppMain.GmGameDatLoadBossBattleExit();
        AppMain.GmGameDatReleaseBossBattleStart(AppMain.g_gm_main_system.boss_load_no);
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainObjectReleaseFinalWait));
    }

    private static void gmMainObjectReleaseFinalWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmMainDatReleaseBossBattleReleaseCheck())
            return;
        AppMain.GmGameDatReleaseBossBattleExit();
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainObjectReleaseWait));
        AppMain.g_obj.flag |= 1073741824U;
        AppMain.ObjExit();
    }

    private static void gmMainObjectReleaseWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.ObjObjectCheckClearAllObject() || AppMain.ObjIsExitWait())
            return;
        AppMain.GmGameDatFlashRestart();
        AppMain.mtTaskClearTcb(tcb);
        AppMain.SyChangeNextEvt();
    }

    private static void gmMainObjectReleaseDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gm_main_release_wait_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmMainGameStart()
    {
        bool flag = false;
        if (SaveState.shouldResume())
        {
            SaveState.resumePlayerState();
            SaveState.resumeMapData();
        }
        AppMain.amIPhoneTouchCanceled();
        CPadVirtualPad.CreateInstance().Create(new float[4]
        {
      -120f,
      166f,
      232f,
      318f
        });
        AppMain.CPadPolarHandle instance = AppMain.CPadPolarHandle.CreateInstance();
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)9)
            instance.Create(0.0f, 0.0f, (float)((double)AppMain.AMD_SCREEN_2D_WIDTH * 4.0 / 5.0), AppMain.AMD_SCREEN_2D_HEIGHT);
        else
            instance.Create();
        instance.SetValue(0.0f);
        AppMain.g_gm_main_system.polar_now = 0;
        AppMain.g_gm_main_system.polar_diff = 0;
        if (((int)AppMain.GsGetMainSysInfo().game_flag & 1) == 0)
        {
            AppMain.GsMainSysSetSleepFlag(false);
            AppMain.GsMainSysSetAccelFlag(true);
        }
        else if (((int)AppMain.GsGetMainSysInfo().game_flag & 512) == 0 && (AppMain.GsGetMainSysInfo().stage_id == (ushort)9 || AppMain.GSM_MAIN_STAGE_IS_SPSTAGE()))
        {
            AppMain.GsMainSysSetSleepFlag(false);
            AppMain.GsMainSysSetAccelFlag(true);
        }
        else
        {
            AppMain.GsMainSysSetSleepFlag(true);
            AppMain.GsMainSysSetAccelFlag(false);
        }
        AppMain.GmPadVibInit();
        if (((int)AppMain.g_gm_main_system.game_flag & 512) != 0)
        {
            AppMain.g_gm_main_system.game_time = 0U;
            flag = true;
        }
        AppMain.g_gm_main_system.game_flag &= 4187479041U;
        AppMain.g_gm_main_system.die_event_wait_time = 0;
        AppMain.g_gm_main_system.pseudofall_dir = (ushort)0;
        AppMain.g_gm_main_system.boss_load_no = -1;
        AppMain.g_gm_main_system.pre_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainPre), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 4096U, 5, (AppMain.TaskWorkFactoryDelegate)null, "GM_MAIN_PRE");
        AppMain.g_gm_main_system.post_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainPost), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 32768U, 5, (AppMain.TaskWorkFactoryDelegate)null, "GM_MAIN_POST");
        AppMain.g_obj.flag = 4194408U;
        AppMain.g_obj.ppPre = new AppMain.OBJECT_Delegate(AppMain.GmObjPreFunc);
        AppMain.g_obj.ppPost = (AppMain.OBJECT_Delegate)null;
        AppMain.g_obj.ppCollision = new AppMain.OBJECT_WORK_Delegate(AppMain.GmObjCollision);
        AppMain.g_obj.ppObjPre = new AppMain.OBJECT_WORK_Delegate(AppMain.GmObjObjPreFunc);
        AppMain.g_obj.ppObjPost = new AppMain.OBJECT_WORK_Delegate(AppMain.GmObjObjPostFunc);
        AppMain.g_obj.ppRegRecAuto = new AppMain.OBJECT_WORK_Delegate(AppMain.GmObjRegistRectAuto);
        AppMain.g_obj.draw_scale.x = AppMain.g_obj.draw_scale.y = AppMain.g_obj.draw_scale.z = 13107;
        AppMain.g_obj.inv_draw_scale.x = AppMain.g_obj.inv_draw_scale.y = AppMain.g_obj.inv_draw_scale.z = AppMain.FX_Div(4096, AppMain.g_obj.draw_scale.x);
        AppMain.g_obj.depth = 128;
        AppMain.ObjDebugRectActionInit();
        AppMain.gmMainInitLight();
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 4) != 0)
        {
            AppMain.g_gm_main_system.game_time = AppMain.g_gm_main_system.time_save;
            if (AppMain.g_gm_main_system.marker_pri == 0U)
            {
                AppMain.g_gm_main_system.ply_dmg_count = 0U;
                AppMain.g_gm_main_system.game_flag &= 4227858431U;
            }
            else
            {
                AppMain.g_gm_main_system.game_flag |= 67108864U;
                if (flag)
                    AppMain.g_gs_main_sys_info.game_flag |= 256U;
            }
        }
        AppMain.GmMapInit();
        AppMain.GmTvxInit();
        AppMain.GmMapFarInit();
        AppMain.GmDecoInit();
        AppMain.GmWaterSurfaceInit();
        AppMain.GmPlyEfctTrailSysInit();
        AppMain.GmFixInit();
        AppMain.GmCameraInit();
        AppMain.GmSoundInit();
        AppMain.GmRingInit();
        AppMain.GmEventMgrInit();
        AppMain.GmEventMgrStart();
        for (int index = 0; index < 1; ++index)
        {
            if (AppMain.g_gs_main_sys_info.char_id[index] != (int)short.MaxValue)
                AppMain.g_gm_main_system.ply_work[index] = AppMain.GmPlayerInit(AppMain.g_gs_main_sys_info.char_id[index], (ushort)0, (ushort)index, (ushort)0);
        }
        AppMain.GmEveMgrCreateStateEvent();
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 4) != 0 && AppMain.g_gm_main_system.marker_pri > 0U)
            SaveState.saveCurrentState(0);
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)28)
        {
            AppMain.g_gm_main_system.game_flag |= 268435456U;
            AppMain.g_gm_main_system.game_flag &= 4160749567U;
        }
        AppMain.g_gm_main_system.game_flag &= 4294964223U;
        AppMain.g_gm_main_system.game_flag |= 2048U;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
            AppMain.GmSplStageStart();
        else if (AppMain.g_gs_main_sys_info.game_mode == 2)
            AppMain.GmEndingStart();
        else
            AppMain.GmStartDemoStart();
    }

    private static void gmMainInitLight()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
        {
            AppMain.g_obj.ambient_color.r = 1f;
            AppMain.g_obj.ambient_color.g = 0.0f;
            AppMain.g_obj.ambient_color.b = 0.0f;
        }
        else if (AppMain.g_gs_main_sys_info.stage_id == (ushort)14)
        {
            AppMain.g_obj.ambient_color.r = 0.1f;
            AppMain.g_obj.ambient_color.g = 0.1f;
            AppMain.g_obj.ambient_color.b = 0.1f;
        }
        else
        {
            AppMain.g_obj.ambient_color.r = 0.8f;
            AppMain.g_obj.ambient_color.g = 0.8f;
            AppMain.g_obj.ambient_color.b = 0.8f;
        }
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 3)
        {
            nnsVector.x = -0.95f;
            nnsVector.y = 0.25f;
            nnsVector.z = -1f;
        }
        else if (AppMain.g_gs_main_sys_info.stage_id == (ushort)16)
        {
            nnsVector.x = -1f;
            nnsVector.y = -1f;
            nnsVector.z = -1f;
        }
        else
        {
            nnsVector.x = 0.0f;
            nnsVector.y = 0.0f;
            nnsVector.z = -1f;
        }
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        float intensity1 = AppMain.g_gs_main_sys_info.stage_id != (ushort)14 ? 1f : 0.8f;
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_0, ref col, intensity1, nnsVector);
        AppMain.g_gm_main_system.def_light_vec.Assign(nnsVector);
        AppMain.g_gm_main_system.def_light_col = col;
        float intensity2 = AppMain.g_gs_main_sys_info.stage_id != (ushort)14 ? 1.5f : 1f;
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 3)
        {
            nnsVector.x = 0.05f;
            nnsVector.y = 0.15f;
            nnsVector.z = -0.05f;
        }
        else
        {
            nnsVector.x = -0.5f;
            nnsVector.y = -0.4f;
            nnsVector.z = -0.25f;
        }
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_6, ref col, intensity2, nnsVector);
        AppMain.g_gm_main_system.ply_light_vec.Assign(nnsVector);
        AppMain.g_gm_main_system.ply_light_col = col;
        AppMain.GmMapSetLight();
        if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 0)
            AppMain.GmGmkBreakLandSetLight();
        else if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 3)
        {
            AppMain.GmGmkGearSetLight();
            AppMain.GmGmkNeedleSetLight();
        }
        else if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
            AppMain.GmSplStageSetLight();
        else if (AppMain.GMM_MAIN_GET_ZONE_TYPE() == 4)
        {
            AppMain.GmBoss5LandSetLight();
            AppMain.GmDecoSetLightFinalZone();
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    private static void gmMainPre(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmMainUpdateSuspendedPause();
        AppMain.GMS_MAIN_SYSTEM gGmMainSystem = AppMain.g_gm_main_system;
        if (((int)gGmMainSystem.game_flag & 134217728) != 0)
        {
            AppMain.g_gm_main_system.game_flag &= 4160749567U;
            if (AppMain.g_gs_main_sys_info.stage_id != (ushort)28 && ((int)AppMain.g_gm_main_system.game_flag & 524288) == 0)
                AppMain.GmSoundPlayStageBGM(0);
        }
        CPadVirtualPad.CreateInstance().Update();
        AppMain.CPadPolarHandle instance = AppMain.CPadPolarHandle.CreateInstance();
        instance.Update();
        int polarNow = gGmMainSystem.polar_now;
        gGmMainSystem.polar_now = instance.GetAngle32Value();
        gGmMainSystem.polar_diff = gGmMainSystem.polar_now - polarNow;
        if (!AppMain.gmMainIsUseWaitUpCamera())
            return;
        if (AppMain.GmPlayerIsStateWait(gGmMainSystem.ply_work[0]))
        {
            if (gGmMainSystem.camscale_state == AppMain.GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_NON)
                gGmMainSystem.camscale_state = AppMain.GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_ZOOM;
        }
        else
        {
            gGmMainSystem.camscale_state = AppMain.GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_NON;
            gGmMainSystem.camera_scale = 0.6743833f;
        }
        if (gGmMainSystem.camscale_state == AppMain.GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_ZOOM)
        {
            gGmMainSystem.camera_scale -= 0.01f;
            if ((double)gGmMainSystem.camera_scale < 0.337191671133041)
            {
                gGmMainSystem.camera_scale = 0.3371917f;
                gGmMainSystem.camscale_state = AppMain.GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_UP;
            }
        }
        for (int cam_id = 0; cam_id < 7; ++cam_id)
        {
            AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(cam_id);
            if (obsCamera != null)
                obsCamera.scale = gGmMainSystem.camera_scale;
        }
    }

    private static void gmMainPost(AppMain.MTS_TASK_TCB tcb)
    {
        //AppMain.CPadEmu.CreateInstance();
        if (((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 1024) != 0 && AppMain.g_gm_main_system.die_event_wait_time < 491520 && ((int)AppMain.g_gm_main_system.game_flag & 64) == 0)
        {
            AppMain.g_gm_main_system.die_event_wait_time = AppMain.ObjTimeCountUp(AppMain.g_gm_main_system.die_event_wait_time);
            if (AppMain.g_gm_main_system.die_event_wait_time >= 491520)
            {
                if (AppMain.g_gs_main_sys_info.game_mode == 1)
                {
                    AppMain.GmClearDemoRetryStart();
                }
                else
                {
                    --AppMain.g_gm_main_system.player_rest_num[0];
                    if ((int)AppMain.g_gm_main_system.player_rest_num[0] < 0)
                        AppMain.g_gm_main_system.player_rest_num[0] = 0U;
                    if (AppMain.g_gm_main_system.player_rest_num[0] == 0U)
                    {
                        AppMain.g_gm_main_system.game_flag |= 32U;
                        AppMain.GmOverStart(0);
                        AppMain.GmSoundPlayGameOver();
                    }
                    else
                    {
                        AppMain.g_gm_main_system.game_flag |= 2U;
                        AppMain.g_gs_main_sys_info.game_flag |= 4U;
                        if (((int)AppMain.g_gm_main_system.game_flag & 512) != 0)
                        {
                            AppMain.g_gm_main_system.game_flag |= 32U;
                            AppMain.GmOverStart(1);
                        }
                        else
                            AppMain.IzFadeInitEasy(0U, 1U, 15f);
                    }
                }
            }
        }
        if (AppMain.g_gs_main_sys_info.game_mode == 1 && ((int)AppMain.g_gm_main_system.game_flag & 16) != 0 || AppMain.g_gs_main_sys_info.game_mode != 1 && (((int)AppMain.g_gm_main_system.game_flag & 256) != 0 || ((int)AppMain.g_gm_main_system.game_flag & 768) == 0 && ((int)AppMain.g_gm_main_system.game_flag & 2) != 0 && AppMain.IzFadeIsEnd() || ((int)AppMain.g_gm_main_system.game_flag & 20) == 20))
        {
            AppMain.gmMainDecideNextEvt();
            if (((int)AppMain.g_gm_main_system.game_flag & 2) != 0)
            {
                AppMain.GmMainRestartExit();
                if (AppMain.g_gs_main_sys_info.game_mode != 1)
                    return;
                AppMain.GmMainGSRetryInit();
            }
            else
                AppMain.GmMainExit();
        }
        else if (!AppMain.AoAccountIsCurrentEnable())
        {
            AppMain.SyDecideEvtCase((short)5);
            AppMain.IzFadeInitEasy(0U, 3U, 1f);
            AppMain.GmMainExit();
        }
        else
        {
            if (((int)AppMain.g_gm_main_system.game_flag & 4) != 0 && ((int)AppMain.g_gm_main_system.game_flag & 8) == 0)
            {
                AppMain.g_gs_main_sys_info.game_flag |= 2U;
                AppMain.GmClearDemoStart();
                AppMain.g_gm_main_system.game_flag |= 8U;
            }
            if (AppMain.GmMainCheckExeTimerCount())
            {
                if (!AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
                    ++AppMain.g_gm_main_system.game_time;
                else if (AppMain.g_gm_main_system.game_time != 0U)
                    --AppMain.g_gm_main_system.game_time;
            }
            if (AppMain.gmMainCheckExeSyncTimerCount())
                ++AppMain.g_gm_main_system.sync_time;
            if ((AppMain.GmMainKeyCheckPauseKeyPush() != -1 || AppMain.gmMainIsSuspendedPause() || (AppMain.isBackKeyPressed() || !AppMain.isForeground) || AppMain.g_ao_sys_global.is_show_ui) && AppMain.GmPauseCheckExecutable())
            {
                AppMain.setBackKeyRequest(false);
                AppMain.GmMainClearSuspendedPause();
                AppMain.GmPauseInit();
            }
            else
            {
                if (((int)AppMain.g_gm_main_system.game_flag & 128) == 0)
                    return;
                AppMain.gmMainDecideNextEvt();
                AppMain.g_gm_main_system.game_flag &= 4294967167U;
                if (AppMain.GmPauseMenuGetResult() == 0)
                {
                    AppMain.GmMainRestartExit();
                    AppMain.GmMainGSRetryInit();
                }
                else
                    AppMain.GmMainExit();
            }
        }
    }

    private static void gmMainDecideNextEvt()
    {
        short evt_case;
        if (((int)AppMain.g_gm_main_system.game_flag & 128) != 0)
        {
            switch (AppMain.GmPauseMenuGetResult())
            {
                case 0:
                    evt_case = (short)1;
                    break;
                case 2:
                    evt_case = (short)0;
                    break;
                case 3:
                    evt_case = (short)4;
                    break;
                default:
                    evt_case = (short)1;
                    break;
            }
        }
        else if (((int)AppMain.g_gm_main_system.game_flag & 2) == 0 && AppMain.GsTrialIsTrial())
        {
            AppMain.g_gs_main_sys_info.game_flag &= 4294967009U;
            AppMain.nextDemoLevel = 0;
            evt_case = (short)6;
        }
        else if (AppMain.g_gs_main_sys_info.game_mode == 1)
        {
            AppMain.GmMainClearSuspendedPause();
            evt_case = ((int)AppMain.g_gm_main_system.game_flag & 2) == 0 ? (!AppMain.GsMainSysIsStageClear(0) ? (short)4 : (short)0) : (short)1;
        }
        else
            evt_case = ((int)AppMain.g_gm_main_system.game_flag & 4) == 0 ? (((int)AppMain.g_gm_main_system.game_flag & 2) == 0 ? (!AppMain.GsMainSysIsStageClear(0) ? (short)4 : (short)0) : (short)1) : (((int)AppMain.g_gm_main_system.game_flag & 16384) == 0 ? (AppMain.g_gs_main_sys_info.stage_id != (ushort)16 ? (short)0 : (short)2) : (short)3);
        AppMain.SyDecideEvtCase(evt_case);
    }

    private static bool gmMainCheckExeSyncTimerCount()
    {
        return ((int)AppMain.g_gm_main_system.game_flag & 2048) != 0;
    }

    private static void gmMainDataLoadBoosBattleMgr_LoadWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (AppMain.GmGameDatLoadCheck() != AppMain.GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_COMPLETE)
            return;
        AppMain.GMS_MAIN_LOAD_BB_MGR_WORK work = (AppMain.GMS_MAIN_LOAD_BB_MGR_WORK)tcb.work;
        AppMain.GmGameDatLoadExit();
        AppMain.GmGameDatBuildBossBattleInit();
        AppMain.GmGameDatBuildBossBattle(work.boss_type);
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataLoadBoosBattleMgr_BuildWait));
    }

    private static void gmMainDataLoadBoosBattleMgr_BuildWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmGameDatBuildBossBattleCheck())
            return;
        AppMain.GMS_MAIN_LOAD_BB_MGR_WORK work = (AppMain.GMS_MAIN_LOAD_BB_MGR_WORK)tcb.work;
        work.b_end = true;
        AppMain.g_gm_main_system.boss_load_no = work.boss_type;
        AppMain.g_gm_main_system.game_flag &= 4292870143U;
        AppMain.mtTaskClearTcb(tcb);
        AppMain.gm_main_load_bossbattle_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void gmMainDataReleaseBoosBattleMgr_FlushWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmGameDatFlushBossBattleCheck())
            return;
        AppMain.GmGameDatBoosBattleRelease(((AppMain.GMS_MAIN_LOAD_BB_MGR_WORK)tcb.work).boss_type);
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataReleaseBoosBattleMgr_ReleaseWait));
    }

    private static void gmMainDataReleaseBoosBattleMgr_ReleaseWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.GmGameDatReleaseCheck())
            return;
        ((AppMain.GMS_MAIN_LOAD_BB_MGR_WORK)tcb.work).b_end = true;
        AppMain.g_gm_main_system.boss_load_no = -1;
        AppMain.g_gm_main_system.game_flag &= 4290772991U;
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataReleaseBoosBattleMgr_EndWait));
    }

    private static void gmMainDataReleaseBoosBattleMgr_EndWait(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static bool gmMainIsUseWaitUpCamera()
    {
        bool flag = true;
        switch (AppMain.g_gs_main_sys_info.stage_id)
        {
            case 3:
            case 7:
            case 11:
            case 15:
            case 16:
            case 17:
            case 18:
            case 19:
            case 20:
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
                flag = false;
                break;
            default:
                if (((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 65536) != 0)
                {
                    flag = false;
                    break;
                }
                break;
        }
        return flag;
    }

    public static bool gmMainIsSuspendedPause()
    {
        return ((int)AppMain.g_gs_main_sys_info.game_flag & 536870912) != 0;
    }

    private static void gmMainUpdateSuspendedPause()
    {
        uint num = 4104;
        if (!AppMain.GsMainSysGetSuspendedFlag() || ((int)AppMain.g_gm_main_system.game_flag & 32968936 & ~(int)num) != 0)
            return;
        AppMain.g_gs_main_sys_info.game_flag |= 536870912U;
    }

}