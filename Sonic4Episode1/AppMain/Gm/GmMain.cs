using System;
using System.Threading;
using System.Threading.Tasks;
using mpp;

public partial class AppMain
{
    public static short GMD_OBJ_LCD_X => (short)(0.674383342266083 * GSD_DISP_WIDTH);

    public static short GMD_OBJ_LCD_Y => (short)(0.674383342266083 * GSD_DISP_HEIGHT);

    public static bool GMM_MAIN_STAGE_IS_BOSS()
    {
        return g_gs_main_sys_info.stage_id < g_gm_gamedat_stage_type_tbl.Length && g_gm_gamedat_stage_type_tbl[g_gs_main_sys_info.stage_id] == GSE_MAIN_STAGE_TYPE.GSD_MAIN_STAGE_TYPE_BOSS;
    }

    public static bool GMM_MAIN_STAGE_IS_SS()
    {
        return g_gs_main_sys_info.stage_id < g_gm_gamedat_stage_type_tbl.Length && g_gm_gamedat_stage_type_tbl[g_gs_main_sys_info.stage_id] == GSE_MAIN_STAGE_TYPE.GSD_MAIN_STAGE_TYPE_SS;
    }

    public static bool GMM_MAIN_STAGE_IS_ENDING()
    {
        return g_gs_main_sys_info.game_mode == 2;
    }

    private static int GMM_MAIN_GET_ZONE_TYPE()
    {
        return g_gm_gamedat_zone_type_tbl[g_gs_main_sys_info.stage_id];
    }

    public static bool GMM_MAIN_USE_SUPER_SONIC()
    {
        return ((int)g_gm_main_system.game_flag & 524288) != 0;
    }

    public static bool GMM_MAIN_GOAL_AS_SUPER_SONIC()
    {
        return ((int)g_gm_main_system.game_flag & 33554432) != 0;
    }

    private static bool GmMainIsWaterLevel()
    {
        return g_gm_main_system.water_level != ushort.MaxValue;
    }

    public static void GmMainClearSuspendedPause()
    {
        g_gs_main_sys_info.game_flag &= 3758096383U;
    }

    private static void GmMainGSInit()
    {
        g_gs_main_sys_info.game_flag &= 4294967009U;
        g_gs_main_sys_info.clear_ring = 0U;
        g_gs_main_sys_info.clear_score = 0U;
        g_gs_main_sys_info.clear_time = 0;
        
        AoPresenceSet();
    }

    private static void GmMainGSRetryInit()
    {
        _bossFinishThread = true;
        g_gs_main_sys_info.rest_player_num = g_gm_main_system.player_rest_num[0];
        g_gs_main_sys_info.game_flag &= 4294967025U;
        g_gs_main_sys_info.clear_ring = 0U;
        g_gs_main_sys_info.clear_score = 0U;
        g_gs_main_sys_info.clear_time = 0;
        
        AoPresenceSet();
    }

    private void GmMainInit(object arg)
    {
        GsFontRelease();
        //AppMain.CPadEmu.CreateInstance().Create(AppMain.CPadEmu.EMode.Game);
        if (GsTrialIsTrial())
            g_gs_main_sys_info.stage_id = (ushort)nextDemoLevel;
        if (SaveState.shouldResume())
            SaveState.resumeStageState();
        if (((int)g_gs_main_sys_info.game_flag & 4) == 0)
            gmMainSysInit();
        if (g_gs_main_sys_info.stage_id == 9)
            ObjInit(4, GMD_TASK_PRIO_OBJSYS, 0, (short)(GMD_OBJ_LCD_X * 1.42), (short)(GMD_OBJ_LCD_X * 1.42), GSD_DISP_WIDTH, GSD_DISP_HEIGHT);
        else
            ObjInit(4, GMD_TASK_PRIO_OBJSYS, 0, GMD_OBJ_LCD_X, GMD_OBJ_LCD_Y, GSD_DISP_WIDTH, GSD_DISP_HEIGHT);
        ObjDataAlloc(994);
        ObjDrawESEffectSystemInit(0, 20480U, 5U);
        amTrailEFInitialize();
        ObjDrawSetNNCommandStateTbl(0U, 1U, true);
        ObjDrawSetNNCommandStateTbl(1U, 2U, false);
        ObjDrawSetNNCommandStateTbl(2U, 3U, true);
        ObjDrawSetNNCommandStateTbl(3U, 5U, true);
        ObjDrawSetNNCommandStateTbl(4U, 11U, true);
        ObjDrawSetNNCommandStateTbl(5U, 12U, true);
        ObjDrawSetNNCommandStateTbl(6U, 15U, false);
        ObjDrawSetNNCommandStateTbl(7U, 0U, false);
        ObjDrawSetNNCommandStateTbl(8U, 16U, true);
        if (g_gs_main_sys_info.stage_id == 10 || g_gs_main_sys_info.stage_id == 14)
            ObjDrawSetNNCommandStateTbl(9U, 17U, true);
        else
            ObjDrawSetNNCommandStateTbl(9U, uint.MaxValue, false);
        ObjDrawSetNNCommandStateTbl(10U, 9U, true);
        ObjDrawSetNNCommandStateTbl(11U, 4U, true);
        ObjDrawSetNNCommandStateTbl(12U, 8U, true);
        ObjDrawSetNNCommandStateTbl(13U, 7U, true);
        ObjDrawSetNNCommandStateTbl(14U, 10U, true);
        ObjDrawSetNNCommandStateTbl(15U, 6U, true);
        AoActSysClearPeak();
        if (((int)g_gs_main_sys_info.game_flag & 16) == 0)
            gmMainLoad(0);
        else
            gmMainRebuild();
        
        //AoPresenceSet((AOE_PRESENCE)GsGetMainSysInfo().stage_id, false);
    }

    private static void GmMainEnd()
    {
        GmPadVibExit();
        if (g_gm_main_system.pre_tcb != null)
        {
            mtTaskClearTcb(g_gm_main_system.pre_tcb);
            g_gm_main_system.pre_tcb = null;
        }
        if (g_gm_main_system.post_tcb != null)
        {
            mtTaskClearTcb(g_gm_main_system.post_tcb);
            g_gm_main_system.post_tcb = null;
        }
        amTrailEFDeleteGroup(1);
        GmPlyEfctTrailSysExit();
        g_obj.ppPre = null;
        ObjObjectClearAllObject();
        ObjPreExit();
        GmMapExit();
        GmFixExit();
        GmPauseExit();
        GmRingExit();
        GmCameraExit();
        GmSoundExit();
        GmMapFarExit();
        GmDecoExit();
        GmWaterSurfaceExit(); 
        GmEventMgrExit();
        ObjDrawESEffectSystemExit();
        GmClearDemoExit();
        GmOverExit();
        GmSplStageExit();
        GmEndingExit();
        GmStartDemoExit();
        GmStartMsgExit();
        CPadVirtualPad.CreateInstance().Release();
        CPadPolarHandle.CreateInstance().Release();
        GsMainSysSetSleepFlag(true);
        GsMainSysSetAccelFlag(false);
    }

    private static void GmMainExit()
    {
        GmMainEnd();
        gmMainDataRelease();
    }

    private static void GmMainRestartExit()
    {
        GmMainEnd();
        if (g_gs_main_sys_info.stage_id == 16)
        {
            gmMainObjectRelease();
        }
        else
        {
            g_obj.flag |= 1073741824U;
            ObjExit();
            gmMainObjectRelease();
        }
    }

    private static void GmMainExitForStaffroll()
    {
        gmMainDataRelease();
    }

    private static bool GmMainCheckExeTimerCount()
    {
        return ((int)g_gm_main_system.game_flag & 1024) != 0;
    }

    public static bool GmMainIsDrawEnable()
    {
        return _am_sample_draw_enable;
    }

    public static float GmMainGetDrawMotionSpeed()
    {
        return _am_sample_count;
    }

    private static ushort GmMainGetObjectRotation()
    {
        ushort num = 0;
        OBS_CAMERA obsCamera = ObjCameraGet(g_obj.glb_camera_id);
        if (obsCamera != null)
            num = (ushort)-obsCamera.roll;
        return num;
    }

    private static uint GmMainGetLightColor()
    {
        uint num = 3772834047;
        if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
            num = 3767175935U;
        else if (g_gs_main_sys_info.stage_id == 14)
            num = 2593823487U;
        return num;
    }

    private static uint GmMainGetLightColorABGR()
    {
        uint num = 4292927712;
        if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
            num = 4287269600U;
        else if (g_gs_main_sys_info.stage_id == 14)
            num = 4288322202U;
        return num;
    }

    private static OpenGL.glArray4f GmMainGetLightColorArray4f()
    {
        switch (g_gs_main_sys_info.stage_id)
        {
            case 2:
            case 3:
                return LightColor[1];
            case 14:
                return LightColor[2];
            default:
                return LightColor[0];
        }
    }

#if USE_THREADS
    private static Thread _bossThread;
#else
    private static Task _bossTask;
    private static CancellationTokenSource _bossCancellation;
#endif
    private static bool _bossFinishThread;

    public static void GmMainDatLoadBossBattleStart(int boss_type)
    {
        if (g_gm_main_system.boss_load_no == boss_type)
            return;
        if (GmMainDatReleaseBossBattleReleaseCheck())
            GmGameDatReleaseBossBattleExit();
        if (g_gs_main_sys_info.stage_id == 16 && boss_type > 0 && boss_type < 4)
        {
#if USE_THREADS
            if (_bossThread != null && _bossThread.ThreadState == ThreadState.Running)
                _bossThread.Abort();

            _bossFinishThread = false;

            _bossThread = new Thread(() => _GmMainDatLoadBossBattleStart(boss_type));
            _bossThread.Start();
#else
            if (_bossTask != null && !_bossCancellation.IsCancellationRequested)
                _bossCancellation.Cancel();

            _bossTask = null;
            _bossFinishThread = false;

            _bossCancellation = new CancellationTokenSource();
#if ASYNC_TARGETING_PACK
            _bossTask = TaskEx.Run(async () => await _GmMainDatLoadBossBattleStart(boss_type));
#else
            _bossTask = Task.Run(async () => await _GmMainDatLoadBossBattleStart(boss_type));
#endif
#endif
        }
        else
        {
            _bossFinishThread = true;
            GmGameDatLoadBoosBattleInit(boss_type);
            gm_main_load_bossbattle_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmMainDataLoadBoosBattleMgr_LoadWait), null, 0U, ushort.MaxValue, 2048U, 5, () => new GMS_MAIN_LOAD_BB_MGR_WORK(), "GM_LOAD_BBM");
            GMS_MAIN_LOAD_BB_MGR_WORK work = (GMS_MAIN_LOAD_BB_MGR_WORK)gm_main_load_bossbattle_tcb.work;
            work.boss_type = boss_type;
            work.b_end = false;
            g_gm_main_system.game_flag |= 2097152U;
        }
    }

#if USE_THREADS
    public static void _GmMainDatLoadBossBattleStart(object o)
#else
    public static async Task _GmMainDatLoadBossBattleStart(object o)
#endif
    {
        int boss_type = (int)o;
        bool flag = false;
        do
        {
            int x = g_gm_main_system.ply_work[0].obj_work.pos.x;
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

#if USE_THREADS
            if (!flag)
                Thread.Sleep(30);

#else
            if (_bossFinishThread)
                return;

#if ASYNC_TARGETING_PACK
            if (!flag)
                await TaskEx.Delay(30);
#else
            if (!flag)
                await Task.Delay(30);
#endif
            if (_bossCancellation.Token.IsCancellationRequested) return;
#endif
        }
        while (!flag);
        GmGameDatLoadBoosBattleInit(boss_type);
        gm_main_load_bossbattle_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmMainDataLoadBoosBattleMgr_LoadWait), null, 0U, ushort.MaxValue, 2048U, 5, () => new GMS_MAIN_LOAD_BB_MGR_WORK(), "GM_LOAD_BBM");
        GMS_MAIN_LOAD_BB_MGR_WORK work = (GMS_MAIN_LOAD_BB_MGR_WORK)gm_main_load_bossbattle_tcb.work;
        work.boss_type = boss_type;
        work.b_end = false;
        g_gm_main_system.game_flag |= 2097152U;
        _bossFinishThread = true;
    }

    private static bool GmMainDatLoadBossBattleLoadCheck()
    {
        return GmMainDatLoadBossBattleLoadCheck(0);
    }

    private static bool GmMainDatLoadBossBattleLoadCheck(int boss_type)
    {
        return g_gm_main_system.boss_load_no != -1 && ((int)g_gm_main_system.game_flag & 4194304) == 0 && (boss_type == -1 || boss_type == g_gm_main_system.boss_load_no);
    }

    private static bool GmMainDatLoadBossBattleLoadNowCheck()
    {
        return ((int)g_gm_main_system.game_flag & 2097152) != 0;
    }


    private static int GmMainKeyCheckPauseKeyOn()
    {
        int num1 = -1;
        uint num2 = 215;
        uint num3 = 0;
        if (GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            num2 = 390U;
            num3 = 0U;
        }
        else if (GsGetMainSysInfo().game_mode == 1)
        {
            num2 = 275U;
            num3 = 0U;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (amTpIsTouchOn(index))
            {
                ushort num4 = _am_tp_touch[index].on[0];
                ushort num5 = _am_tp_touch[index].on[1];
                if (num4 >= num2 && num4 <= num2 + 115U && (num5 >= num3 && num5 <= num3 + 60U))
                {
                    ushort num6 = _am_tp_touch[index].push[0];
                    ushort num7 = _am_tp_touch[index].push[1];
                    if (num6 >= num2 && num6 <= num2 + 115U && (num7 >= num3 && num7 <= num3 + 60U))
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
        if (GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            num2 = 390U;
            num3 = 0U;
        }
        else if (GsGetMainSysInfo().game_mode == 1)
        {
            num2 = 275U;
            num3 = 0U;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (amTpIsTouchPush(index))
            {
                ushort num4 = _am_tp_touch[index].push[0];
                ushort num5 = _am_tp_touch[index].push[1];
                if (num4 >= num2 && num4 <= num2 + 115U && (num5 >= num3 && num5 <= num3 + 60U))
                {
                    num1 = index;
                    break;
                }
            }
        }
        if (!g_pause_flag)
            return num1;
        g_pause_flag = false;
        return 1;
    }

    private static void gmMainSysInit()
    {
        g_gm_main_system.Clear();
        g_gm_main_system.player_rest_num[0] = g_gs_main_sys_info.rest_player_num;
        if (g_gm_main_system.player_rest_num[0] <= 0U)
            g_gm_main_system.player_rest_num[0] = 1U;
        mtMathSRand((uint)(nnRandom() * (double)short.MaxValue));
    }

    private static void gmMainLoad(int load_proc)
    {
        short[] char_id_list = new short[1];
        GmMainClearSuspendedPause();
        DmLoadingStart();
        GmPauseMenuLoadStart();
        for (int index = 0; index < 1; ++index)
            char_id_list[index] = (short)g_gs_main_sys_info.char_id[index];
        GmGameDatLoadInit(load_proc, g_gs_main_sys_info.stage_id, char_id_list);
        gm_main_load_wait_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmMainDataLoadWait), new GSF_TASK_PROCEDURE(gmMainDataLoadDest), 0U, ushort.MaxValue, 4096U, 0, null, "GM_LOAD_WAIT");
    }

    private static void gmMainDataLoadWait(MTS_TASK_TCB tcb)
    {
        gmMainUpdateSuspendedPause();
        if (!GmPauseMenuLoadIsFinished() || GmGameDatLoadCheck() != GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_COMPLETE)
            return;
        if (!SaveState.shouldResume())
            SaveState.deleteSave();
        GmGameDatBuildInit();
        GmGameDatBuildStandard();
        GmGameDatBuildArea();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainDataBuildWait));
        // GC.Collect();
        // GC.WaitForPendingFinalizers();
        // GC.Collect();
    }

    private static void gmMainDataBuildWait(MTS_TASK_TCB tcb)
    {
        gmMainUpdateSuspendedPause();
        if (!GmGameDatBuildStandardCheck() || !GmGameDatBuildAreaCheck() || (!SoundPartialCache(5) || !GsSoundPrepareBGMForLevel(g_gs_main_sys_info.stage_id)))
            return;
        GmGameDatLoadExit();
        g_gs_main_sys_info.game_flag |= 16U;
        DmLoadingSetLoadComplete();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainDataLoadingEndWait));
    }

    private static void gmMainDataLoadingEndWait(MTS_TASK_TCB tcb)
    {
        gmMainUpdateSuspendedPause();
        if (!DmLoadingIsExit())
            return;
        gmMainGameStart();
        mtTaskClearTcb(tcb);
    }

    private static void gmMainDataLoadDest(MTS_TASK_TCB tcb)
    {
        gm_main_load_wait_tcb = null;
    }

    private static void gmMainRebuild()
    {
        gm_main_load_wait_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmMainRebuildWait), new GSF_TASK_PROCEDURE(gmMainRebuildDest), 0U, ushort.MaxValue, 4096U, 0, null, "GM_REBUILD_WAIT");
        GmGameDatReBuildRestart();
    }

    private static void gmMainRebuildWait(MTS_TASK_TCB tcb)
    {
        if (!GmGameDatReBuildRestartCheck())
            return;
        gmMainGameStart();
        mtTaskClearTcb(tcb);
    }

    private static void gmMainRebuildDest(MTS_TASK_TCB tcb)
    {
        gm_main_load_wait_tcb = null;
    }

    private static void gmMainDataRelease()
    {
        gm_main_release_wait_tcb = MTM_TASK_MAKE_TCB(g_gs_main_sys_info.stage_id == 16 ? new GSF_TASK_PROCEDURE(gmMainDataFlushExitFinalClearObjWait) : new GSF_TASK_PROCEDURE(gmMainDataFlushExitWait), new GSF_TASK_PROCEDURE(gmMainDataReleaseDest), 0U, ushort.MaxValue, 4096U, 0, null, "GM_UNLOAD_WAIT");
    }

    private static void gmMainDataFlushExitFinalClearObjWait(MTS_TASK_TCB tcb)
    {
        _bossFinishThread = true;
        if (!ObjObjectCheckClearAllObject())
            return;
        GsSoundReset();
        GSF_TASK_PROCEDURE proc;
        if (GmMainDatLoadBossBattleLoadCheck())
        {
            proc = new GSF_TASK_PROCEDURE(gmMainDataFlushExitFinalWait);
            GmGameDatLoadBossBattleExit();
            GmGameDatReleaseBossBattleStart(g_gm_main_system.boss_load_no);
        }
        else if (GmMainDatLoadBossBattleLoadNowCheck())
            proc = new GSF_TASK_PROCEDURE(gmMainDataFlushExitFinalLoadWait);
        else if (GmMainDatReleaseBossBattleReleaseNowCheck())
        {
            proc = new GSF_TASK_PROCEDURE(gmMainDataFlushExitFinalWait);
            GmGameDatLoadBossBattleExit();
        }
        else
        {
            proc = new GSF_TASK_PROCEDURE(gmMainDataFlushExitWait);
            GmGameDatReleaseBossBattleExit();
        }
        mtTaskChangeTcbProcedure(tcb, proc);
    }

    private static void gmMainDataFlushExitFinalLoadWait(MTS_TASK_TCB tcb)
    {
        if (!GmMainDatLoadBossBattleLoadCheck())
            return;
        GmGameDatLoadBossBattleExit();
        GmGameDatReleaseBossBattleStart(g_gm_main_system.boss_load_no);
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainDataFlushExitFinalWait));
    }

    private static void gmMainDataFlushExitFinalWait(MTS_TASK_TCB tcb)
    {
        if (!GmMainDatReleaseBossBattleReleaseCheck())
            return;
        GmGameDatReleaseBossBattleExit();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainDataFlushExitWait));
    }

    private static void gmMainDataFlushExitWait(MTS_TASK_TCB tcb)
    {
        if (!ObjObjectCheckClearAllObject())
            return;
        GsSoundReset();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainDataFlushWait));
        GmGameDatFlushInit();
        GmGameDatFlushArea();
        GmGameDatFlushStandard();
    }

    private static void gmMainDataFlushWait(MTS_TASK_TCB tcb)
    {
        if (!GmGameDatFlushStandardCheck() || !GmGameDatFlushAreaCheck())
            return;
        OBS_DATA_WORK[] pData = g_obj.pData;
        for (int index = 0; index < g_obj.data_max; ++index)
        {
            if (pData[index].pData != null && (pData[index].num & 32768) == 0)
                pData[index].pData = null;
        }
        ObjExit();
        GmGameDatReleaseStandard();
        GmGameDatReleaseArea();
        GmPauseMenuRelease();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainDataReleaseWait));
    }

    private static void gmMainDataReleaseWait(MTS_TASK_TCB tcb)
    {
        if (!GmGameDatReleaseCheck() || ObjIsExitWait())
            return;
        g_gs_main_sys_info.game_flag &= 4294967279U;
        mtTaskClearTcb(tcb);
        g_gs_main_sys_info.rest_player_num = g_gm_main_system.player_rest_num[0];
        if (g_gs_main_sys_info.rest_player_num <= 0U)
            g_gs_main_sys_info.rest_player_num = 3U;
        if (g_gs_main_sys_info.game_mode == 0 && ((int)g_gs_main_sys_info.game_flag & 2) != 0 && g_gs_main_sys_info.stage_id == 16)
        {
            g_gs_main_sys_info.stage_id = 28;
            g_gs_main_sys_info.char_id[0] = 0;
            g_gs_main_sys_info.game_mode = 2;
            GmMainGSInit();
        }
        SyChangeNextEvt();
    }

    private static void gmMainDataReleaseDest(MTS_TASK_TCB tcb)
    {
        gm_main_release_wait_tcb = null;
    }

    private static void gmMainObjectRelease()
    {
        gm_main_release_wait_tcb = MTM_TASK_MAKE_TCB(g_gs_main_sys_info.stage_id == 16 ? new GSF_TASK_PROCEDURE(gmMainObjectReleaseFinalClearObjWait) : new GSF_TASK_PROCEDURE(gmMainObjectReleaseWait), new GSF_TASK_PROCEDURE(gmMainObjectReleaseDest), 0U, ushort.MaxValue, 4096U, 0, null, "GM_UNLOAD_OBJ_WAIT");
    }

    private static void gmMainObjectReleaseFinalClearObjWait(MTS_TASK_TCB tcb)
    {
        if (!ObjObjectCheckClearAllObject())
            return;
        GSF_TASK_PROCEDURE proc;
        if (GmMainDatLoadBossBattleLoadCheck())
        {
            proc = new GSF_TASK_PROCEDURE(gmMainObjectReleaseFinalWait);
            GmGameDatLoadBossBattleExit();
            GmGameDatReleaseBossBattleStart(g_gm_main_system.boss_load_no);
        }
        else if (GmMainDatLoadBossBattleLoadNowCheck())
            proc = new GSF_TASK_PROCEDURE(gmMainObjectReleaseFinalLoadWait);
        else if (GmMainDatReleaseBossBattleReleaseNowCheck())
        {
            proc = new GSF_TASK_PROCEDURE(gmMainObjectReleaseFinalWait);
            GmGameDatLoadBossBattleExit();
        }
        else
        {
            proc = new GSF_TASK_PROCEDURE(gmMainObjectReleaseWait);
            GmGameDatReleaseBossBattleExit();
            g_obj.flag |= 1073741824U;
            ObjExit();
        }
        mtTaskChangeTcbProcedure(tcb, proc);
    }

    private static void gmMainObjectReleaseFinalLoadWait(MTS_TASK_TCB tcb)
    {
        if (!GmMainDatLoadBossBattleLoadCheck())
            return;
        GmGameDatLoadBossBattleExit();
        GmGameDatReleaseBossBattleStart(g_gm_main_system.boss_load_no);
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainObjectReleaseFinalWait));
    }

    private static void gmMainObjectReleaseFinalWait(MTS_TASK_TCB tcb)
    {
        if (!GmMainDatReleaseBossBattleReleaseCheck())
            return;
        GmGameDatReleaseBossBattleExit();
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainObjectReleaseWait));
        g_obj.flag |= 1073741824U;
        ObjExit();
    }

    private static void gmMainObjectReleaseWait(MTS_TASK_TCB tcb)
    {
        if (!ObjObjectCheckClearAllObject() || ObjIsExitWait())
            return;
        GmGameDatFlashRestart();
        mtTaskClearTcb(tcb);
        SyChangeNextEvt();
    }

    private static void gmMainObjectReleaseDest(MTS_TASK_TCB tcb)
    {
        gm_main_release_wait_tcb = null;
    }

    private static void gmMainGameStart()
    {
        bool flag = false;
        if (SaveState.shouldResume())
        {
            SaveState.resumePlayerState();
            SaveState.resumeMapData();
        }
        amIPhoneTouchCanceled();
        CPadVirtualPad.CreateInstance().Create(new float[4] { -120f, 166f, 232f, 318f });
        CPadPolarHandle instance = CPadPolarHandle.CreateInstance();
        if (g_gs_main_sys_info.stage_id == 9)
            instance.Create(0.0f, 0.0f, (float)(AMD_SCREEN_2D_WIDTH * 4.0 / 5.0), AMD_SCREEN_2D_HEIGHT);
        else
            instance.Create();
        instance.SetValue(0.0f);
        g_gm_main_system.polar_now = 0;
        g_gm_main_system.polar_diff = 0;
        if (((int)GsGetMainSysInfo().game_flag & 1) == 0)
        {
            GsMainSysSetSleepFlag(false);
            GsMainSysSetAccelFlag(true);
        }
        else if (((int)GsGetMainSysInfo().game_flag & 512) == 0 && (GsGetMainSysInfo().stage_id == 9 || GSM_MAIN_STAGE_IS_SPSTAGE()))
        {
            GsMainSysSetSleepFlag(false);
            GsMainSysSetAccelFlag(true);
        }
        else
        {
            GsMainSysSetSleepFlag(true);
            GsMainSysSetAccelFlag(false);
        }
        GmPadVibInit();
        if (((int)g_gm_main_system.game_flag & 512) != 0)
        {
            g_gm_main_system.game_time = 0U;
            flag = true;
        }
        g_gm_main_system.game_flag &= 4187479041U;
        g_gm_main_system.die_event_wait_time = 0;
        g_gm_main_system.pseudofall_dir = 0;
        g_gm_main_system.boss_load_no = -1;
        g_gm_main_system.pre_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmMainPre), null, 0U, 0, 4096U, 5, null, "GM_MAIN_PRE");
        g_gm_main_system.post_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmMainPost), null, 0U, 0, 32768U, 5, null, "GM_MAIN_POST");
        g_obj.flag = 4194408U;
        g_obj.ppPre = new OBJECT_Delegate(GmObjPreFunc);
        g_obj.ppPost = null;
        g_obj.ppCollision = new OBJECT_WORK_Delegate(GmObjCollision);
        g_obj.ppObjPre = new OBJECT_WORK_Delegate(GmObjObjPreFunc);
        g_obj.ppObjPost = new OBJECT_WORK_Delegate(GmObjObjPostFunc);
        g_obj.ppRegRecAuto = new OBJECT_WORK_Delegate(GmObjRegistRectAuto);
        g_obj.draw_scale.x = g_obj.draw_scale.y = g_obj.draw_scale.z = 13107;
        g_obj.inv_draw_scale.x = g_obj.inv_draw_scale.y = g_obj.inv_draw_scale.z = FX_Div(4096, g_obj.draw_scale.x);
        g_obj.depth = 128;
        ObjDebugRectActionInit();
        gmMainInitLight();
        if (((int)g_gs_main_sys_info.game_flag & 4) != 0)
        {
            g_gm_main_system.game_time = g_gm_main_system.time_save;
            if (g_gm_main_system.marker_pri == 0U)
            {
                g_gm_main_system.ply_dmg_count = 0U;
                g_gm_main_system.game_flag &= 4227858431U;
            }
            else
            {
                g_gm_main_system.game_flag |= 67108864U;
                if (flag)
                    g_gs_main_sys_info.game_flag |= 256U;
            }
        }
        GmMapInit();
        GmTvxInit();
        GmMapFarInit();
        GmDecoInit();
        GmWaterSurfaceInit();
        GmPlyEfctTrailSysInit();
        GmFixInit();
        GmCameraInit();
        GmSoundInit();
        GmRingInit();
        GmEventMgrInit();
        GmEventMgrStart();
        for (int index = 0; index < 1; ++index)
        {
            if (g_gs_main_sys_info.char_id[index] != short.MaxValue)
                g_gm_main_system.ply_work[index] = GmPlayerInit(g_gs_main_sys_info.char_id[index], 0, (ushort)index, 0);
        }
        GmEveMgrCreateStateEvent();
        if (((int)g_gs_main_sys_info.game_flag & 4) != 0 && g_gm_main_system.marker_pri > 0U)
            SaveState.saveCurrentState(0);
        if (g_gs_main_sys_info.stage_id != 28)
        {
            g_gm_main_system.game_flag |= 268435456U;
            g_gm_main_system.game_flag &= 4160749567U;
        }
        g_gm_main_system.game_flag &= 4294964223U;
        g_gm_main_system.game_flag |= 2048U;
        if (GSM_MAIN_STAGE_IS_SPSTAGE())
            GmSplStageStart();
        else if (g_gs_main_sys_info.game_mode == 2)
            GmEndingStart();
        else
            GmStartDemoStart();
    }

    private static void gmMainInitLight()
    {
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        if (g_gs_main_sys_info.stage_id == 2 || g_gs_main_sys_info.stage_id == 3)
        {
            g_obj.ambient_color.r = 1f;
            g_obj.ambient_color.g = 0.0f;
            g_obj.ambient_color.b = 0.0f;
        }
        else if (g_gs_main_sys_info.stage_id == 14)
        {
            g_obj.ambient_color.r = 0.1f;
            g_obj.ambient_color.g = 0.1f;
            g_obj.ambient_color.b = 0.1f;
        }
        else
        {
            g_obj.ambient_color.r = 0.8f;
            g_obj.ambient_color.g = 0.8f;
            g_obj.ambient_color.b = 0.8f;
        }
        if (GMM_MAIN_GET_ZONE_TYPE() == 3)
        {
            nnsVector.x = -0.95f;
            nnsVector.y = 0.25f;
            nnsVector.z = -1f;
        }
        else if (g_gs_main_sys_info.stage_id == 16)
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
        nnNormalizeVector(nnsVector, nnsVector);
        float intensity1 = g_gs_main_sys_info.stage_id != 14 ? 1f : 0.8f;
        ObjDrawSetParallelLight(NNE_LIGHT_0, ref col, intensity1, nnsVector);
        g_gm_main_system.def_light_vec.Assign(nnsVector);
        g_gm_main_system.def_light_col = col;
        float intensity2 = g_gs_main_sys_info.stage_id != 14 ? 1.5f : 1f;
        if (GMM_MAIN_GET_ZONE_TYPE() == 3)
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
        nnNormalizeVector(nnsVector, nnsVector);
        ObjDrawSetParallelLight(NNE_LIGHT_6, ref col, intensity2, nnsVector);
        g_gm_main_system.ply_light_vec.Assign(nnsVector);
        g_gm_main_system.ply_light_col = col;
        GmMapSetLight();
        if (GMM_MAIN_GET_ZONE_TYPE() == 0)
            GmGmkBreakLandSetLight();
        else if (GMM_MAIN_GET_ZONE_TYPE() == 3)
        {
            GmGmkGearSetLight();
            GmGmkNeedleSetLight();
        }
        else if (GSM_MAIN_STAGE_IS_SPSTAGE())
            GmSplStageSetLight();
        else if (GMM_MAIN_GET_ZONE_TYPE() == 4)
        {
            GmBoss5LandSetLight();
            GmDecoSetLightFinalZone();
        }
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    private static void gmMainPre(MTS_TASK_TCB tcb)
    {
        gmMainUpdateSuspendedPause();
        GMS_MAIN_SYSTEM gGmMainSystem = g_gm_main_system;
        if (((int)gGmMainSystem.game_flag & 134217728) != 0)
        {
            g_gm_main_system.game_flag &= 4160749567U;
            if (g_gs_main_sys_info.stage_id != 28 && ((int)g_gm_main_system.game_flag & 524288) == 0)
                GmSoundPlayStageBGM(0);
        }
        CPadVirtualPad.CreateInstance().Update();
        CPadPolarHandle instance = CPadPolarHandle.CreateInstance();
        instance.Update();
        int polarNow = gGmMainSystem.polar_now;
        gGmMainSystem.polar_now = instance.GetAngle32Value();
        gGmMainSystem.polar_diff = gGmMainSystem.polar_now - polarNow;
        if (!gmMainIsUseWaitUpCamera())
            return;
        if (GmPlayerIsStateWait(gGmMainSystem.ply_work[0]))
        {
            if (gGmMainSystem.camscale_state == GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_NON)
                gGmMainSystem.camscale_state = GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_ZOOM;
        }
        else
        {
            gGmMainSystem.camscale_state = GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_NON;
            gGmMainSystem.camera_scale = 0.6743833f;
        }
        if (gGmMainSystem.camscale_state == GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_ZOOM)
        {
            gGmMainSystem.camera_scale -= 0.01f;
            if (gGmMainSystem.camera_scale < 0.337191671133041)
            {
                gGmMainSystem.camera_scale = 0.3371917f;
                gGmMainSystem.camscale_state = GME_MAIN_CAMSCALE_STATE.GMD_MAIN_CAMSCALE_STATE_UP;
            }
        }
        for (int cam_id = 0; cam_id < 7; ++cam_id)
        {
            OBS_CAMERA obsCamera = ObjCameraGet(cam_id);
            if (obsCamera != null)
                obsCamera.scale = gGmMainSystem.camera_scale;
        }
    }

    private static void gmMainPost(MTS_TASK_TCB tcb)
    {
        //AppMain.CPadEmu.CreateInstance();
        if ((g_gm_main_system.ply_work[0].player_flag & GMD_PLF_DIE) != 0 && g_gm_main_system.die_event_wait_time < 491520 && ((int)g_gm_main_system.game_flag & 64) == 0)
        {
            g_gm_main_system.die_event_wait_time = ObjTimeCountUp(g_gm_main_system.die_event_wait_time);
            if (g_gm_main_system.die_event_wait_time >= 491520)
            {
                if (g_gs_main_sys_info.game_mode == 1)
                {
                    GmClearDemoRetryStart();
                }
                else
                {
                    --g_gm_main_system.player_rest_num[0];
                    if ((int)g_gm_main_system.player_rest_num[0] < 0)
                        g_gm_main_system.player_rest_num[0] = 0U;
                    if (g_gm_main_system.player_rest_num[0] == 0U)
                    {
                        g_gm_main_system.game_flag |= 32U;
                        GmOverStart(0);
                        GmSoundPlayGameOver();
                    }
                    else
                    {
                        g_gm_main_system.game_flag |= 2U;
                        g_gs_main_sys_info.game_flag |= 4U;
                        if (((int)g_gm_main_system.game_flag & 512) != 0)
                        {
                            g_gm_main_system.game_flag |= 32U;
                            GmOverStart(1);
                        }
                        else
                            IzFadeInitEasy(0U, 1U, 15f);
                    }
                }
            }
        }
        if (g_gs_main_sys_info.game_mode == 1 && ((int)g_gm_main_system.game_flag & 16) != 0 || g_gs_main_sys_info.game_mode != 1 && (((int)g_gm_main_system.game_flag & 256) != 0 || ((int)g_gm_main_system.game_flag & 768) == 0 && ((int)g_gm_main_system.game_flag & 2) != 0 && IzFadeIsEnd() || ((int)g_gm_main_system.game_flag & 20) == 20))
        {
            gmMainDecideNextEvt();
            if (((int)g_gm_main_system.game_flag & 2) != 0)
            {
                GmMainRestartExit();
                if (g_gs_main_sys_info.game_mode != 1)
                    return;
                GmMainGSRetryInit();
            }
            else
                GmMainExit();
        }
        else if (!AoAccountIsCurrentEnable())
        {
            SyDecideEvtCase(5);
            IzFadeInitEasy(0U, 3U, 1f);
            GmMainExit();
        }
        else
        {
            if (((int)g_gm_main_system.game_flag & 4) != 0 && ((int)g_gm_main_system.game_flag & 8) == 0)
            {
                g_gs_main_sys_info.game_flag |= 2U;
                GmClearDemoStart();
                g_gm_main_system.game_flag |= 8U;
            }
            if (GmMainCheckExeTimerCount())
            {
                if (!GSM_MAIN_STAGE_IS_SPSTAGE())
                    ++g_gm_main_system.game_time;
                else if (g_gm_main_system.game_time != 0U)
                    --g_gm_main_system.game_time;
            }
            if (gmMainCheckExeSyncTimerCount())
                ++g_gm_main_system.sync_time;
            if ((GmMainKeyCheckPauseKeyPush() != -1 || gmMainIsSuspendedPause() || (isBackKeyPressed() || !isForeground) || g_ao_sys_global.is_show_ui) && GmPauseCheckExecutable())
            {
                setBackKeyRequest(false);
                GmMainClearSuspendedPause();
                GmPauseInit();
            }
            else
            {
                if (((int)g_gm_main_system.game_flag & 128) == 0)
                    return;
                gmMainDecideNextEvt();
                g_gm_main_system.game_flag &= 4294967167U;
                if (GmPauseMenuGetResult() == 0)
                {
                    GmMainRestartExit();
                    GmMainGSRetryInit();
                }
                else
                    GmMainExit();
            }
        }
    }

    private static void gmMainDecideNextEvt()
    {
        short evt_case;
        if (((int)g_gm_main_system.game_flag & 128) != 0)
        {
            switch (GmPauseMenuGetResult())
            {
                case 0:
                    evt_case = 1;
                    break;
                case 2:
                    evt_case = 0;
                    break;
                case 3:
                    evt_case = 4;
                    break;
                default:
                    evt_case = 1;
                    break;
            }
        }
        else if (((int)g_gm_main_system.game_flag & 2) == 0 && GsTrialIsTrial())
        {
            g_gs_main_sys_info.game_flag &= 4294967009U;
            nextDemoLevel = 0;
            evt_case = 6;
        }
        else if (g_gs_main_sys_info.game_mode == 1)
        {
            GmMainClearSuspendedPause();
            evt_case = ((int)g_gm_main_system.game_flag & 2) == 0 ? (!GsMainSysIsStageClear(0) ? (short)4 : (short)0) : (short)1;
        }
        else
            evt_case = ((int)g_gm_main_system.game_flag & 4) == 0 ? (((int)g_gm_main_system.game_flag & 2) == 0 ? (!GsMainSysIsStageClear(0) ? (short)4 : (short)0) : (short)1) : (((int)g_gm_main_system.game_flag & 16384) == 0 ? (g_gs_main_sys_info.stage_id != 16 ? (short)0 : (short)2) : (short)3);
        SyDecideEvtCase(evt_case);
    }

    private static bool gmMainCheckExeSyncTimerCount()
    {
        return ((int)g_gm_main_system.game_flag & 2048) != 0;
    }

    private static void gmMainDataLoadBoosBattleMgr_LoadWait(MTS_TASK_TCB tcb)
    {
        if (GmGameDatLoadCheck() != GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_COMPLETE)
            return;
        GMS_MAIN_LOAD_BB_MGR_WORK work = (GMS_MAIN_LOAD_BB_MGR_WORK)tcb.work;
        GmGameDatLoadExit();
        GmGameDatBuildBossBattleInit();
        GmGameDatBuildBossBattle(work.boss_type);
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainDataLoadBoosBattleMgr_BuildWait));
    }

    private static void gmMainDataLoadBoosBattleMgr_BuildWait(MTS_TASK_TCB tcb)
    {
        if (!GmGameDatBuildBossBattleCheck())
            return;
        GMS_MAIN_LOAD_BB_MGR_WORK work = (GMS_MAIN_LOAD_BB_MGR_WORK)tcb.work;
        work.b_end = true;
        g_gm_main_system.boss_load_no = work.boss_type;
        g_gm_main_system.game_flag &= 4292870143U;
        mtTaskClearTcb(tcb);
        gm_main_load_bossbattle_tcb = null;
    }

    private static void gmMainDataReleaseBoosBattleMgr_FlushWait(MTS_TASK_TCB tcb)
    {
        if (!GmGameDatFlushBossBattleCheck())
            return;
        GmGameDatBoosBattleRelease(((GMS_MAIN_LOAD_BB_MGR_WORK)tcb.work).boss_type);
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainDataReleaseBoosBattleMgr_ReleaseWait));
    }

    private static void gmMainDataReleaseBoosBattleMgr_ReleaseWait(MTS_TASK_TCB tcb)
    {
        if (!GmGameDatReleaseCheck())
            return;
        ((GMS_MAIN_LOAD_BB_MGR_WORK)tcb.work).b_end = true;
        g_gm_main_system.boss_load_no = -1;
        g_gm_main_system.game_flag &= 4290772991U;
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmMainDataReleaseBoosBattleMgr_EndWait));
    }

    private static void gmMainDataReleaseBoosBattleMgr_EndWait(MTS_TASK_TCB tcb)
    {
    }

    private static bool gmMainIsUseWaitUpCamera()
    {
        bool flag = true;
        switch (g_gs_main_sys_info.stage_id)
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
                if ((g_gm_main_system.ply_work[0].player_flag & GMD_PLF_TATK_RETRY) != 0)
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
        return ((int)g_gs_main_sys_info.game_flag & 536870912) != 0;
    }

    private static void gmMainUpdateSuspendedPause()
    {
        uint num = 4104;
        if (!GsMainSysGetSuspendedFlag() || ((int)g_gm_main_system.game_flag & 32968936 & ~(int)num) != 0)
            return;
        g_gs_main_sys_info.game_flag |= 536870912U;
    }

}