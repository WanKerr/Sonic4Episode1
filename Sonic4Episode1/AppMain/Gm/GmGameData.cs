using System;

public partial class AppMain
{
    private static void GmGameDatLoadBossBattleExit()
    {
        if (gm_main_load_bossbattle_tcb == null)
            return;
        mtTaskClearTcb(gm_main_load_bossbattle_tcb);
        gm_main_load_bossbattle_tcb = null;
    }

    private static void GmGameDatReleaseBossBattleStart(int boss_type)
    {
        GmGameDatFlushBossBattleInit();
        GmGameDatFlushBossBattle(boss_type);
        gm_main_release_bossbattle_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmMainDataReleaseBoosBattleMgr_FlushWait), null, 0U, ushort.MaxValue, 2048U, 5, () => new GMS_MAIN_LOAD_BB_MGR_WORK(), "GM_RELEASEBBM");
        GMS_MAIN_LOAD_BB_MGR_WORK work = (GMS_MAIN_LOAD_BB_MGR_WORK)gm_main_release_bossbattle_tcb.work;
        work.boss_type = boss_type;
        work.b_end = false;
        g_gm_main_system.game_flag |= 4194304U;
    }

    private static bool GmMainDatReleaseBossBattleReleaseCheck()
    {
        return g_gm_main_system.boss_load_no == -1 && ((int)g_gm_main_system.game_flag & 2097152) == 0;
    }

    private static bool GmMainDatReleaseBossBattleReleaseNowCheck()
    {
        return ((int)g_gm_main_system.game_flag & 4194304) != 0;
    }

    private static void GmGameDatReleaseBossBattleExit()
    {
        if (gm_main_release_bossbattle_tcb == null)
            return;
        mtTaskClearTcb(gm_main_release_bossbattle_tcb);
        gm_main_release_bossbattle_tcb = null;
    }

    public static AMS_AMB_HEADER GmGameDatGetCockpitData()
    {
        return g_gm_gamedat_cockpit_main_arc;
    }

    public static void GmGameDatLoadInit(int proc_type, ushort stage_id, short[] char_id_list)
    {
        MTS_TASK_TCB mtsTaskTcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmDataLoadMain), new GSF_TASK_PROCEDURE(gmDataLoadDest), 0U, ushort.MaxValue, 2048U, 5, () => new GMS_GAMEDAT_LOAD_WORK(), "GM_LOAD");
        gm_gamedat_load_tcb = mtsTaskTcb;
        GMS_GAMEDAT_LOAD_WORK work = (GMS_GAMEDAT_LOAD_WORK)mtsTaskTcb.work;
        gm_gamedat_load_work = work;
        work.Clear();
        gm_gamedat_load_work.stage_id = stage_id;
        for (int index = 0; index < 1; ++index)
            work.char_id[index] = (ushort)char_id_list[index];
        work.proc_type = proc_type;
        ArrayPointer<GMS_GAMEDAT_LOAD_CONTEXT> arrayPointer = new ArrayPointer<GMS_GAMEDAT_LOAD_CONTEXT>(work.context);
        GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo1 = gm_gamedat_tbl_common_info_tbl[0];
        int index1 = 0;
        while (index1 < gmsGamedatLoadInfo1.num)
        {
            (~arrayPointer).load_data = gmsGamedatLoadInfo1.data_tbl[index1];
            (~arrayPointer).data_no = (ushort)index1;
            int num = (int)gmGameDatLoad(arrayPointer);
            ++index1;
            ++arrayPointer;
            ++work.context_num;
        }
        for (int index2 = 0; index2 < 1; ++index2)
        {
            if (work.char_id[index2] != (ushort)short.MaxValue)
            {
                GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo2 = gm_gamedat_tbl_player_info_tbl[work.char_id[index2]];
                int index3 = 0;
                while (index3 < gmsGamedatLoadInfo2.num)
                {
                    (~arrayPointer).load_data = gmsGamedatLoadInfo2.data_tbl[index3];
                    (~arrayPointer).char_id = work.char_id[index2];
                    (~arrayPointer).ply_no = (ushort)index2;
                    (~arrayPointer).data_no = (ushort)index3;
                    int num = (int)gmGameDatLoad(arrayPointer);
                    ++index3;
                    ++arrayPointer;
                    ++work.context_num;
                }
            }
        }
        GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo3 = gm_gamedat_tbl_map_info_tbl[stage_id];
        int index4 = 0;
        while (index4 < gmsGamedatLoadInfo3.num)
        {
            (~arrayPointer).load_data = gmsGamedatLoadInfo3.data_tbl[index4];
            (~arrayPointer).stage_id = stage_id;
            (~arrayPointer).data_no = (ushort)index4;
            int num = (int)gmGameDatLoad(arrayPointer);
            ++index4;
            ++arrayPointer;
            ++work.context_num;
        }
        GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo4 = gm_gamedat_tbl_effect_info_tbl[stage_id];
        int index5 = 0;
        while (index5 < gmsGamedatLoadInfo4.num)
        {
            (~arrayPointer).load_data = gmsGamedatLoadInfo4.data_tbl[index5];
            (~arrayPointer).stage_id = stage_id;
            int num = (int)gmGameDatLoad(arrayPointer);
            ++index5;
            ++arrayPointer;
            ++work.context_num;
        }
        if (g_gs_main_sys_info.stage_id != 16)
        {
            GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo2 = gm_gamedat_tbl_enemy_info_tbl[stage_id];
            int index2 = 0;
            while (index2 < gmsGamedatLoadInfo2.num)
            {
                (~arrayPointer).load_data = gmsGamedatLoadInfo2.data_tbl[index2];
                (~arrayPointer).stage_id = stage_id;
                int num = (int)gmGameDatLoad(arrayPointer);
                ++index2;
                ++arrayPointer;
                ++work.context_num;
            }
        }
        GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo5 = gm_gamedat_tbl_gimmick_common_info_tbl[0];
        int index6 = 0;
        while (index6 < gmsGamedatLoadInfo5.num)
        {
            (~arrayPointer).load_data = gmsGamedatLoadInfo5.data_tbl[index6];
            (~arrayPointer).stage_id = stage_id;
            int num = (int)gmGameDatLoad(arrayPointer);
            ++index6;
            ++arrayPointer;
            ++work.context_num;
        }
        GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo6 = gm_gamedat_tbl_gimmick_info_tbl[stage_id];
        int index7 = 0;
        while (index7 < gmsGamedatLoadInfo6.num)
        {
            (~arrayPointer).load_data = gmsGamedatLoadInfo6.data_tbl[index7];
            (~arrayPointer).stage_id = stage_id;
            int num = (int)gmGameDatLoad(arrayPointer);
            ++index7;
            ++arrayPointer;
            ++work.context_num;
        }
    }

    public static void GmGameDatLoadPost()
    {
        GMS_GAMEDAT_LOAD_WORK gmGamedatLoadWork = gm_gamedat_load_work;
        if (gmGamedatLoadWork == null)
            return;
        gmGamedatLoadWork.proc_type = 0;
    }

    public static GME_GAMEDAT_LOAD_PROGRESS GmGameDatLoadCheck()
    {
        if (gm_gamedat_load_work == null)
            return GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_NOLOAD;
        if (gm_gamedat_load_work.post_finish)
            return GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_COMPLETE;
        if (gm_gamedat_load_work.load_finish)
            return GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_LOADFINISH;
        else
            return GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_LOADING;
    }

    public static void GmGameDatLoadExit()
    {
        if (gm_gamedat_load_tcb == null)
            return;
        mtTaskClearTcb(gm_gamedat_load_tcb);
    }

    public static void GmGameDatRelease()
    {
        GmGameDatReleaseStandard();
        GmGameDatReleaseArea();
    }

    public static void GmGameDatReleaseStandard()
    {
        GmPlayerRelease();
        if (g_gm_gamedat_cockpit_main_arc != null)
            g_gm_gamedat_cockpit_main_arc = null;
        for (int index = 0; index < 3; ++index)
        {
            if (g_gm_gamedat_ring[index] != null)
                g_gm_gamedat_ring[index] = null;
        }
    }

    public static void GmGameDatReleaseArea()
    {
        GmMapRelease();
        GmMapFarRelease();
        GmDecoRelease();
        GmWaterSurfaceRelease();
        for (int index = 0; index < 11; ++index)
        {
            if (g_gm_gamedat_effect[index] != null)
                g_gm_gamedat_effect[index] = null;
        }
        if (g_gs_main_sys_info.stage_id != 16)
        {
            for (int index = 0; index < 44; ++index)
            {
                if (g_gm_gamedat_enemy[index] != null)
                    g_gm_gamedat_enemy[index] = null;
            }
        }
        if (g_gm_gamedat_enemy_arc != null)
            g_gm_gamedat_enemy_arc = null;
        for (int index = 0; index < 204; ++index)
        {
            if (g_gm_gamedat_gimmick[index] != null)
                g_gm_gamedat_gimmick[index] = null;
        }
    }

    public static bool GmGameDatReleaseCheck()
    {
        return true;
    }

    public static object GmGameDatGetEnemyData(int data_no)
    {
        return g_gm_gamedat_enemy[data_no - 658];
    }

    public static AMS_AMB_HEADER GmGameDatGetGimmickData(int data_no)
    {
        return g_gm_gamedat_gimmick[data_no - 789];
    }

    private static void GmGameDatLoadBoosBattleInit(int boss_type)
    {
        ushort num1 = (ushort)g_gm_gamedat_bossbattle_stage_id_tbl[boss_type];
        MTS_TASK_TCB mtsTaskTcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmDataLoadMain), new GSF_TASK_PROCEDURE(gmDataLoadDest), 0U, ushort.MaxValue, 2048U, 5, () => new GMS_GAMEDAT_LOAD_WORK(), "GM_LOAD_BB");
        gm_gamedat_load_tcb = mtsTaskTcb;
        GMS_GAMEDAT_LOAD_WORK work = (GMS_GAMEDAT_LOAD_WORK)mtsTaskTcb.work;
        gm_gamedat_load_work = work;
        work.Clear();
        gm_gamedat_load_work.stage_id = num1;
        work.proc_type = 0;
        ArrayPointer<GMS_GAMEDAT_LOAD_CONTEXT> arrayPointer1 = new ArrayPointer<GMS_GAMEDAT_LOAD_CONTEXT>(work.context);
        ArrayPointer<GMS_GAMEDAT_LOAD_INFO> arrayPointer2 = new ArrayPointer<GMS_GAMEDAT_LOAD_INFO>(gm_gamedat_tbl_enemy_final_info_tbl, num1);
        int num2 = 0;
        ArrayPointer<GMS_GAMEDAT_LOAD_DATA> dataTbl = (~arrayPointer2).data_tbl;
        while (num2 < (~arrayPointer2).num)
        {
            (~arrayPointer1).load_data = dataTbl;
            (~arrayPointer1).stage_id = num1;
            int num3 = (int)gmGameDatLoad(arrayPointer1);
            ++num2;
            ++arrayPointer1;
            ++dataTbl;
            ++work.context_num;
        }
    }

    public static void GmGameDatBoosBattleRelease(int boss_type)
    {
        int num = g_gm_gamedat_bossbattle_stage_id_tbl[boss_type];
        for (int index = 0; index < 44; ++index)
        {
            if (g_gm_gamedat_enemy[index] != null)
                g_gm_gamedat_enemy[index] = null;
        }
        if (g_gm_gamedat_enemy_arc == null)
            return;
        g_gm_gamedat_enemy_arc = null;
    }

    public static void gmDataLoadMain(MTS_TASK_TCB tcb)
    {
        var work = (GMS_GAMEDAT_LOAD_WORK)tcb.work;
        var gamedatLoadState = work.proc_type != 1 ? GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_COMPLETE : GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_LOADFINISH;
        for (int index = 0; index < work.context_num; ++index)
        {
            if (gmGameDatLoad(work.context[index]) < gamedatLoadState)
                return;
        }
        work.load_finish = true;
        if (work.proc_type == 1)
        {
            mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmDataLoadMainPostWait));
        }
        else
        {
            work.post_finish = true;
            mtTaskChangeTcbProcedure(tcb, null);
        }
    }

    public static void gmDataLoadMainPostWait(MTS_TASK_TCB tcb)
    {
        if (((GMS_GAMEDAT_LOAD_WORK)tcb.work).proc_type != 0)
            return;
        mtTaskChangeTcbProcedure(tcb, new GSF_TASK_PROCEDURE(gmDataLoadMain));
        gmDataLoadMain(tcb);
    }

    public static void gmDataLoadDest(MTS_TASK_TCB tcb)
    {
        gm_gamedat_load_tcb = null;
        gm_gamedat_load_work = null;
    }

    public static object gmGameDatLoadAllocHead(string path)
    {
        return null;
    }

    public static object gmGameDatLoadAllocHeadSub(string path)
    {
        return null;
    }

    public static GME_GAMEDAT_LOAD_STATE gmGameDatLoad(
      GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        return gmGameDatLoadFileReq(context);
    }

    public static GME_GAMEDAT_LOAD_STATE gmGameDatLoadFileReq(
      GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        if (context.state == GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_COMPLETE || context.state == GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_ERROR)
            return context.state;
        GMS_GAMEDAT_LOAD_DATA loadData = context.load_data;
        if (context.fs_req == null)
        {
            context.file_path = loadData.path;
            if (loadData.proc_pre != null)
                loadData.proc_pre(context);
            int stageId = context.stage_id;
            context.fs_req = 7 == stageId && "G_ZONE2/BOSS/BOSS02.AMB" == context.file_path || 11 == stageId && "G_ZONE3/BOSS/BOSS03.AMB" == context.file_path || (15 == stageId && "G_ZONEF/BOSS/BOSS04.AMB" == context.file_path || 16 == stageId && "G_ZONEF/BOSS/BOSS05.AMB" == context.file_path) ? amFsReadBackground(context.file_path, 8192) : amFsReadBackground(context.file_path, 65536);
            context.state = GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_LOADING;
        }
        else if (amFsIsComplete(context.fs_req))
        {
            context.state = GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_COMPLETE;
            if (gm_gamedat_load_work.proc_type != 1 && context.fs_req != null)
            {
                if (loadData.proc_post != null)
                    loadData.proc_post(context);
                amFsClearRequest(context.fs_req);
                context.fs_req = null;
            }
        }
        return context.state;
    }

    public static void gmGameDatLoadProcPostRing(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        g_gm_gamedat_ring[context.load_data.user_data - 2] = context.fs_req;
        context.fs_req = null;
    }

    public static void gmGameDatLoadProcPostPlayer(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        ObjDataSet(g_gm_player_data_work[context.ply_no][context.data_no], context.fs_req);
        context.fs_req = null;
    }

    public static void gmGameDatLoadProcPostCockpit(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        g_gm_gamedat_cockpit_main_arc = readAMBFile(context.fs_req);
        context.fs_req = null;
    }

    public static void gmGameDatLoadProcPostMap(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        g_gm_gamedat_map[context.data_no] = readAMBFile(context.fs_req);
        switch (context.data_no)
        {
            case 0:
                AMS_AMB_HEADER header1 = readAMBFile(context.fs_req);
                for (int index = 0; index < 9 && index < header1.file_num; ++index)
                {
                    switch (index)
                    {
                        case 0:
                        case 1:
                        case 4:
                        case 5:
                            g_gm_gamedat_map_set[index] = readMPFile((AmbChunk)amBindGet(header1, index));
                            break;
                        case 2:
                        case 3:
                            g_gm_gamedat_map_set[index] = readMDFile((AmbChunk)amBindGet(header1, index));
                            break;
                        default:
                            g_gm_gamedat_map_set[index] = amBindGet(header1, index);
                            break;
                    }
                }
                for (int index = 0; index < 10; index += 2)
                {
                    if (header1.file_num >= 11 + index)
                    {
                        g_gm_gamedat_map_set_add[index] = readMPFile((AmbChunk)amBindGet(header1, 9 + index));
                        g_gm_gamedat_map_set_add[index + 1] = readMDFile((AmbChunk)amBindGet(header1, 9 + index + 1));
                    }
                    else
                    {
                        g_gm_gamedat_map_set_add[index] = null;
                        g_gm_gamedat_map_set_add[index + 1] = null;
                    }
                }
                goto case 2;
            case 1:
                AMS_AMB_HEADER gGmGamedat = (AMS_AMB_HEADER)g_gm_gamedat_map[context.data_no];
                TVX_FILE[] tvxFileArray = new TVX_FILE[gGmGamedat.file_num];
                for (int index = 0; index < gGmGamedat.file_num; ++index)
                    tvxFileArray[index] = new TVX_FILE((AmbChunk)amBindGet(gGmGamedat, index));
                if (g_gs_main_sys_info.stage_id >= 0 && g_gs_main_sys_info.stage_id <= 3)
                {
                    for (int index1 = 0; index1 < gGmGamedat.file_num; ++index1)
                    {
                        float val1_1 = tvxFileArray[index1].vertexes[0][0].x;
                        float val1_2 = tvxFileArray[index1].vertexes[0][0].x;
                        float val1_3 = tvxFileArray[index1].vertexes[0][0].y;
                        float val1_4 = tvxFileArray[index1].vertexes[0][0].y;
                        for (int index2 = 1; index2 < tvxFileArray[index1].vertexes[0].Length; ++index2)
                        {
                            val1_1 = Math.Min(val1_1, tvxFileArray[index1].vertexes[0][index2].x);
                            val1_2 = Math.Max(val1_2, tvxFileArray[index1].vertexes[0][index2].x);
                            val1_3 = Math.Min(val1_3, tvxFileArray[index1].vertexes[0][index2].y);
                            val1_4 = Math.Max(val1_4, tvxFileArray[index1].vertexes[0][index2].y);
                        }
                        for (int index2 = 0; index2 < tvxFileArray[index1].vertexes[0].Length; ++index2)
                        {
                            if (tvxFileArray[index1].vertexes[0][index2].x == (double)val1_1)
                                tvxFileArray[index1].vertexes[0][index2].x -= 0.5f;
                            else if (tvxFileArray[index1].vertexes[0][index2].x == (double)val1_2)
                                tvxFileArray[index1].vertexes[0][index2].x += 0.5f;
                            if (tvxFileArray[index1].vertexes[0][index2].y == (double)val1_3)
                                tvxFileArray[index1].vertexes[0][index2].y -= 0.5f;
                            else if (tvxFileArray[index1].vertexes[0][index2].y == (double)val1_4)
                                tvxFileArray[index1].vertexes[0][index2].y += 0.5f;
                        }
                    }
                    if (g_gs_main_sys_info.stage_id == 2)
                    {
                        tvxFileArray[103].vertexes[0][1].x = 63.5f;
                        tvxFileArray[103].vertexes[0][3].x = 63.5f;
                    }
                }
                if (g_gs_main_sys_info.stage_id >= 4 && g_gs_main_sys_info.stage_id <= 7)
                {
                    tvxFileArray[201].vertexes[1][1].x = 64f;
                    tvxFileArray[201].vertexes[1][3].x = 64f;
                }
                if (g_gs_main_sys_info.stage_id >= 8 && g_gs_main_sys_info.stage_id <= 11)
                {
                    for (int index = 59; index <= 62; ++index)
                    {
                        tvxFileArray[index].vertexes[0][0].x = 64.5f;
                        tvxFileArray[index].vertexes[0][2].x = 64.5f;
                        tvxFileArray[index].vertexes[0][0].y = 64.5f;
                        tvxFileArray[index].vertexes[0][1].y = 64.5f;
                        tvxFileArray[index].vertexes[0][1].x = -0.5f;
                        tvxFileArray[index].vertexes[0][3].x = -0.5f;
                        tvxFileArray[index].vertexes[0][2].y = -0.5f;
                        tvxFileArray[index].vertexes[0][3].y = -0.5f;
                    }
                }
                g_gm_gamedat_map[context.data_no] = tvxFileArray;
                goto case 2;
            case 2:
                context.fs_req = null;
                break;
            case 3:
                AMS_AMB_HEADER header2 = readAMBFile(context.fs_req);
                for (int index = 0; index < 3 && index < header2.file_num; ++index)
                    g_gm_gamedat_map_attr_set[index] = amBindGet(header2, index);
                goto case 2;
        }
    }

    public static void gmGameDatLoadProcPostMapFar(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        GmMapFarInitData(readAMBFile(context.fs_req));
        context.fs_req = null;
    }

    public static void gmGameDatLoadProcPostEffect(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        g_gm_gamedat_effect[context.load_data.user_data - 5] = readAMBFile(context.fs_req);
        context.fs_req = null;
    }

    public static void gmGameDatLoadProcPostEnemy(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        g_gm_gamedat_enemy[context.load_data.user_data - 658] = context.fs_req;
        context.fs_req = null;
    }

    public static void gmGameDatLoadProcPostBoss(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        g_gm_gamedat_enemy_arc = readAMBFile(context.fs_req);
        context.fs_req = null;
    }

    public static void gmGameDatLoadProcPostGimmick(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        g_gm_gamedat_gimmick[context.load_data.user_data - 789] = readAMBFile(context.fs_req);
        context.fs_req = null;
    }

    public static void gmGameDatLoadProcPostDeco(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        GmDecoInitData(readAMBFile(context.fs_req));
        context.fs_req = null;
    }

    public static void gmGameDatLoadProcPostWaterSurface(GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        GmWaterSurfaceInitData(readAMBFile(context.fs_req));
        context.fs_req = null;
    }

    private static void GmGameDatBuildInit()
    {
        ObjLoadSetInitDrawFlag(true);
        GmGameDBuildModelBuildInit();
        for (int index = 0; index < 44; ++index)
        {
            if (g_gm_gamedat_enemy[index] != null)
            {
                OBS_DATA_WORK pWork = ObjDataGet(658 + index);
                ObjDataSet(pWork, g_gm_gamedat_enemy[index]);
                pWork.num |= 32768;
            }
        }
        for (int index = 0; index < 204; ++index)
        {
            if (g_gm_gamedat_gimmick[index] != null)
            {
                OBS_DATA_WORK pWork = ObjDataGet(789 + index);
                ObjDataSet(pWork, g_gm_gamedat_gimmick[index]);
                pWork.num |= 32768;
            }
        }
        for (int index = 0; index < 3; ++index)
        {
            if (g_gm_gamedat_ring[index] != null)
            {
                OBS_DATA_WORK pWork = ObjDataGet(2 + index);
                ObjDataSet(pWork, g_gm_gamedat_ring[index]);
                pWork.num |= 32768;
            }
        }
        for (int index = 0; index < 11; ++index)
        {
            if (g_gm_gamedat_effect[index] != null)
            {
                OBS_DATA_WORK pWork = ObjDataGet(5 + index);
                ObjDataSet(pWork, g_gm_gamedat_effect[index]);
                pWork.num |= 32768;
            }
        }
    }

    private static void GmGameDatBuildStandard()
    {
        GmPlayerBuild();
        GmSoundBuild();
        GmFixBuildDataInit();
        GmClearDemoBuild();
        GmStartDemoBuild();
        GmOverBuildDataInit();
        GmPauseMenuBuildStart();
        GmEfctCmnBuildDataInit();
        GmRingBuild();
    }

    private static void GmGameDatBuildArea()
    {
        GmTvxBuild();
        GmMapBuildDataInit();
        GmMapBuildColData();
        GmMapFarBuildData();
        GmDecoBuildData();
        GmWaterSurfaceBuildData();
        GmEventDataBuild();
        GmGmkSpringBuild();
        GmGmkDashPanelBuild();
        GmGmkGoalPanelBuild();
        GmGmkItemBuild();
        GmGmkNeedleBuild();
        GmGmkPointMarkerBuild();
        GmGmkAnimalBuild();
        GmGmkSplRingBuild();
        if (g_gs_main_sys_info.stage_id == 11 || !GMM_MAIN_STAGE_IS_BOSS() && !GMM_MAIN_STAGE_IS_SS() && !GMM_MAIN_STAGE_IS_ENDING())
            GmEneHariSenboBuild();
        GmEfctBossBuildSingleDataInit();
        if (gm_gamedat_build_area_tbl[g_gs_main_sys_info.stage_id] == null)
            return;
        gm_gamedat_build_area_tbl[g_gs_main_sys_info.stage_id]();
    }

    private static bool GmGameDatBuildStandardCheck()
    {
        return GmPlayerBuildCheck() && GmSoundBuildCheck() && (GmRingBuildCheck() != 0 && GmFixBuildDataLoop()) && (GmClearDemoBuildCheck() && GmStartDemoBuildCheck() && (GmOverBuildDataLoop() && GmPauseMenuBuildIsFinished())) && GmEfctCmnBuildDataLoop();
    }

    private static bool GmGameDatBuildAreaCheck()
    {
        bool flag = true;
        if (!GmMapBuildDataLoop())
            flag = false;
        if (!GmMapFarCheckLoading())
            flag = false;
        if (!GmDecoCheckLoading())
            flag = false;
        if (!GmWaterSurfaceCheckLoading())
            flag = false;
        if (!GmGameDBuildCheckBuildModel())
            flag = false;
        if (!GmEfctZoneBuildDataLoop())
            flag = false;
        if (!GmEfctEneBuildDataLoop())
            flag = false;
        if (!GmEfctBossCmnBuildDataLoop())
            flag = false;
        if (!GmEfctBossBuildSingleDataLoop())
            flag = false;
        if (!GmStartMsgBuildCheck())
            flag = false;
        return flag;
    }

    private static void GmGameDatFlushInit()
    {
        ObjLoadSetInitDrawFlag(false);
        GmGameDBuildModelFlushInit();
    }

    private static void GmGameDatFlushStandard()
    {
        GmRingFlush();
        GmEfctCmnFlushDataInit();
        GmPauseMenuFlushStart();
        GmOverFlushDataInit();
        GmStartDemoFlush();
        GmClearDemoFlush();
        GmFixFlushDataInit();
        GmSoundFlush();
        GmPlayerFlush();
    }

    private static void GmGameDatFlushArea()
    {
        if (gm_gamedat_flush_area_tbl[g_gs_main_sys_info.stage_id] != null)
            gm_gamedat_flush_area_tbl[g_gs_main_sys_info.stage_id]();
        if (g_gs_main_sys_info.stage_id == 11 || !GMM_MAIN_STAGE_IS_BOSS() && !GMM_MAIN_STAGE_IS_SS() && !GMM_MAIN_STAGE_IS_ENDING())
            GmEneHariSenboFlush();
        GmGmkSplRingFlush();
        GmGmkAnimalFlush();
        GmGmkPointMarkerFlush();
        GmGmkNeedleFlush();
        GmGmkItemFlush();
        GmGmkGoalPanelFlush();
        GmGmkDashPanelFlush();
        GmGmkSpringFlush();
        GmEventDataFlush();
        GmWaterSurfaceFlushData();
        GmDecoFlushData();
        GmMapFarFlushData();
        GmMapFlushColData();
        GmMapFlushData();
        GmTvxFlush();
    }

    private static bool GmGameDatFlushStandardCheck()
    {
        return GmRingFlushCheck() != 0 && GmEfctCmnFlushDataLoop() && (GmFixFlushDataLoop() && GmStartDemoFlushCheck()) && (GmClearDemoFlushCheck() && GmOverFlushDataLoop() && (GmPauseMenuFlushIsFinished() && GmGameDBuildCheckFlushModel())) && (GmWaterSurfaceCheckFlush() && GmPlayerFlushCheck());
    }

    private static bool GmGameDatFlushAreaCheck()
    {
        bool flag = true;
        if (!GmStartMsgFlushCheck())
            flag = false;
        if (!GmEfctZoneFlushDataLoop())
            flag = false;
        if (!GmEfctEneFlushDataLoop())
            flag = false;
        if (!GmEfctBossCmnFlushDataLoop())
            flag = false;
        if (!GmEfctBossFlushSingleDataLoop())
            flag = false;
        if (!GmMapFlushDataLoop())
            flag = false;
        if (!GmDecoCheckFlushing())
            flag = false;
        return flag;
    }

    private static void GmGameDatBuildBossBattleInit()
    {
        GmGameDBuildModelBuildInit();
        for (int index = 0; index < 44; ++index)
        {
            if (g_gm_gamedat_enemy[index] != null)
            {
                OBS_DATA_WORK pWork = ObjDataGet(658 + index);
                ObjDataSet(pWork, g_gm_gamedat_enemy[index]);
                pWork.num |= 32768;
            }
        }
    }

    private static void GmGameDatBuildBossBattle(int boss_type)
    {
        int num = g_gm_gamedat_bossbattle_stage_id_tbl[boss_type];
        GmEfctBossBuildSingleDataInit();
        if (gm_gamedat_build_boss_buttle_tbl[boss_type] == null)
            return;
        gm_gamedat_build_boss_buttle_tbl[boss_type]();
    }

    private static bool GmGameDatBuildBossBattleCheck()
    {
        bool flag = true;
        if (!GmGameDBuildCheckBuildModel())
            flag = false;
        if (!GmEfctZoneBuildDataLoop())
            flag = false;
        if (!GmEfctEneBuildDataLoop())
            flag = false;
        if (!GmEfctBossCmnBuildDataLoop())
            flag = false;
        if (!GmEfctBossBuildSingleDataLoop())
            flag = false;
        return flag;
    }

    private static void GmGameDatFlushBossBattleInit()
    {
        GmGameDBuildModelFlushInit();
    }

    private static void GmGameDatFlushBossBattle(int boss_type)
    {
        int num = g_gm_gamedat_bossbattle_stage_id_tbl[boss_type];
        if (gm_gamedat_flush_boss_buttle_tbl[boss_type] == null)
            return;
        gm_gamedat_flush_boss_buttle_tbl[boss_type]();
    }

    private static bool GmGameDatFlushBossBattleCheck()
    {
        bool flag = true;
        if (!GmGameDBuildCheckFlushModel())
            flag = false;
        if (!GmEfctZoneFlushDataLoop())
            flag = false;
        if (!GmEfctBossFlushSingleDataLoop())
            flag = false;
        return flag;
    }

    private static void GmGameDatFlashRestart()
    {
        GmEventDataFlush();
    }

    private static void GmGameDatReBuildRestart()
    {
        GmEventDataBuild();
        GmMapBuildColData();
        GmGmkSwitchReBuild();
        GmGmkPressPillarClear();
    }

    private static bool GmGameDatReBuildRestartCheck()
    {
        return true;
    }

    private static void gmGameDatBuildStage11()
    {
        GmEneMotoraBuild();
        GmEneGabuBuild();
        GmEneStingBuild();
        GmEneMereonBuild();
        GmGmkLandBuild();
        GmGmkBridgeBuild();
        GmGmkBreakLandBuild();
        GmGmkBreakWallBuild();
        GmGmkBreakObjBuild();
        GmEfctEneBuildDataInit(0);
        GmEfctZoneBuildDataInit(0);
    }

    private static void gmGameDatBuildStage12()
    {
        GmEneMotoraBuild();
        GmEneGabuBuild();
        GmEneStingBuild();
        GmEneMereonBuild();
        GmGmkLandBuild();
        GmGmkTarzanRopeBuild();
        GmGmkBridgeBuild();
        GmGmkBreakLandBuild();
        GmGmkBreakWallBuild();
        GmGmkBreakObjBuild();
        GmEfctEneBuildDataInit(0);
        GmEfctZoneBuildDataInit(0);
    }

    private static void gmGameDatBuildStage13()
    {
        GmEneMotoraBuild();
        GmEneGabuBuild();
        GmEneStingBuild();
        GmEneMereonBuild();
        GmGmkLandBuild();
        GmGmkPulleyBuild();
        GmGmkBridgeBuild();
        GmGmkBreakLandBuild();
        GmGmkBreakWallBuild();
        GmGmkBreakObjBuild();
        GmEfctEneBuildDataInit(0);
        GmEfctZoneBuildDataInit(0);
    }

    private static void gmGameDatBuildStage1Boss()
    {
        GmBoss1Build();
        GmGmkLandBuild();
        GmGmkCapsuleBuild();
        GmEfctZoneBuildDataInit(0);
        GmEfctBossCmnBuildDataInit();
    }

    private static void gmGameDatBuildStage21()
    {
        GmEneGardonBuild();
        GmEneHaroBuild();
        GmGmkLandBuild();
        GmGmkBumperBuild();
        GmGmkEnBmprBuild();
        GmGmkBobbinBuild();
        GmGmkFlipperBuild();
        GmGmkStopperBuild();
        GmGmkSlotBuild();
        GmGmkSpCtpltBuild();
        GmGmkBreakWallBuild();
        GmGmkBreakObjBuild();
        GmEfctEneBuildDataInit(1);
        GmEfctZoneBuildDataInit(1);
    }

    private static void gmGameDatBuildStage22()
    {
        GmGmkBumperBuild();
        GmGmkEnBmprBuild();
        GmGmkBobbinBuild();
        GmGmkFlipperBuild();
        GmGmkStopperBuild();
        GmGmkSlotBuild();
        GmGmkSpCtpltBuild();
        GmGmkSsArrowBuild();
        GmEfctEneBuildDataInit(1);
        GmEfctZoneBuildDataInit(1);
        GmStartMsgBuild();
    }

    private static void gmGameDatBuildStage23()
    {
        GmEneGardonBuild();
        GmEneHaroBuild();
        GmGmkLandBuild();
        GmGmkBumperBuild();
        GmGmkEnBmprBuild();
        GmGmkBobbinBuild();
        GmGmkFlipperBuild();
        GmGmkCannonBuild();
        GmGmkSpCtpltBuild();
        GmGmkBreakWallBuild();
        GmGmkBreakObjBuild();
        GmGmkSsArrowBuild();
        GmEfctEneBuildDataInit(1);
        GmEfctZoneBuildDataInit(1);
    }

    private static void gmGameDatBuildStage2Boss()
    {
        GmBoss2Build();
        GmGmkCapsuleBuild();
        GmGmkBumperBuild();
        GmGmkEnBmprBuild();
        GmGmkFlipperBuild();
        GmGmkShutterBuild();
        GmGmkNeedleNeonBuild();
        GmEfctBossCmnBuildDataInit();
        GmEfctZoneBuildDataInit(1);
    }

    private static void gmGameDatBuildStage31()
    {
        GmEneMoguBuild();
        GmEneUnidesBuild();
        GmEneUniuniBuild();
        GmEneBukuBuild();
        GmGmkLandBuild();
        GmGmkRockBuild();
        GmGmkSpearBuild();
        GmGmkBreakWallBuild();
        GmGmkBreakObjBuild();
        GmGmkRockRideBuild();
        GmGmkSwitchBuildTypeZone3();
        GmGmkSwWallBuild();
        GmEfctEneBuildDataInit(2);
        GmEfctZoneBuildDataInit(2);
    }

    private static void gmGameDatBuildStage32()
    {
        GmEneMoguBuild();
        GmGmkLandBuild();
        GmGmkSpearBuild();
        GmGmkBreakWallBuild();
        GmGmkBreakObjBuild();
        GmGmkTruckBuild();
        GmGmkSwitchBuildTypeZone3();
        GmGmkSwWallBuild();
        GmGmkDSignBuild();
        GmEfctEneBuildDataInit(2);
        GmEfctZoneBuildDataInit(2);
        GmStartMsgBuild();
    }

    private static void gmGameDatBuildStage33()
    {
        GmEneMoguBuild();
        GmEneUnidesBuild();
        GmEneUniuniBuild();
        GmEneBukuBuild();
        GmGmkLandBuild();
        GmGmkWaterSliderBuild();
        GmGmkSpearBuild();
        GmGmkPressWallBuild();
        GmGmkBreakWallBuild();
        GmGmkBreakObjBuild();
        GmGmkDrainTankBuild();
        GmGmkSwitchBuildTypeZone3();
        GmGmkSwWallBuild();
        GmEfctEneBuildDataInit(2);
        GmEfctZoneBuildDataInit(2);
    }

    private static void gmGameDatBuildStage3Boss()
    {
        GmBoss3Build();
        GmEneMoguBuild();
        GmEneUnidesBuild();
        GmGmkLandBuild();
        GmGmkCapsuleBuild();
        GmGmkSpearBuild();
        GmGmkBoss3PillarBuild();
        GmEfctEneBuildDataInit(2);
        GmEfctZoneBuildDataInit(2);
        GmEfctBossCmnBuildDataInit();
    }

    private static void gmGameDatBuildStage41()
    {
        GmEneTStarBuild();
        GmEneKaniBuild();
        GmEneKamaBuild();
        GmGmkLandBuild();
        GmGmkPistonBuild();
        GmGmkBeltConveyorBuild();
        GmGmkUpBumperBuild();
        GmGmkSteamPipeBuild();
        GmGmkPopSteamBuild();
        GmGmkBreakWallBuild();
        GmGmkSwitchBuildTypeZone4();
        GmGmkSwWallBuild();
        GmEfctEneBuildDataInit(3);
        GmEfctZoneBuildDataInit(3);
    }

    private static void gmGameDatBuildStage42()
    {
        GmEneTStarBuild();
        GmEneKaniBuild();
        GmEneKamaBuild();
        GmGmkLandBuild();
        GmGmkPistonBuild();
        GmGmkBeltConveyorBuild();
        GmGmkUpBumperBuild();
        GmGmkSteamPipeBuild();
        GmGmkPopSteamBuild();
        GmGmkBreakWallBuild();
        GmGmkGearBuild();
        GmGmkSwitchBuildTypeZone4();
        GmGmkSwWallBuild();
        GmEfctEneBuildDataInit(3);
        GmEfctZoneBuildDataInit(3);
    }

    private static void gmGameDatBuildStage43()
    {
        GmEneTStarBuild();
        GmEneKaniBuild();
        GmEneKamaBuild();
        GmGmkLandBuild();
        GmGmkPistonBuild();
        GmGmkBeltConveyorBuild();
        GmGmkUpBumperBuild();
        GmGmkSeesawBuild();
        GmGmkSteamPipeBuild();
        GmGmkPopSteamBuild();
        GmGmkPressWallBuild();
        GmGmkPressPillarBuild();
        GmGmkBreakWallBuild();
        GmGmkGearBuild();
        GmGmkSwitchBuildTypeZone4();
        GmGmkSwWallBuild();
        GmEfctEneBuildDataInit(3);
        GmEfctZoneBuildDataInit(3);
    }

    private static void gmGameDatBuildStage4Boss()
    {
        GmBoss4Build();
        GmGmkLandBuild();
        GmGmkCapsuleBuild();
        GmEfctBossCmnBuildDataInit();
    }

    private static void gmGameDatBuildStageFinalBoss01()
    {
        GmGmkLandBuild();
        GmGmkSteamPipeBuild();
        GmGmkBumperBuild();
        GmGmkFlipperBuild();
        GmGmkShutterBuild();
        GmGmkBoss3PillarBuild();
        GmGmkNeedleNeonBuild();
        GmEfctBossCmnBuildDataInit();
        GmEfctZoneBuildDataInit(4);
    }

    private static void gmGameDatBuildStageFinalBoss02()
    {
        GmGmkLandBuild();
        GmGmkSteamPipeBuild();
        GmGmkBumperBuild();
        GmGmkFlipperBuild();
        GmGmkShutterBuild();
        GmGmkBoss3PillarBuild();
        GmGmkNeedleNeonBuild();
        GmEfctBossCmnBuildDataInit();
        GmEfctZoneBuildDataInit(4);
    }

    private static void gmGameDatBuildStageFinalBoss03()
    {
        GmGmkLandBuild();
        GmGmkSteamPipeBuild();
        GmGmkBumperBuild();
        GmGmkFlipperBuild();
        GmGmkShutterBuild();
        GmGmkBoss3PillarBuild();
        GmGmkNeedleNeonBuild();
        GmEfctBossCmnBuildDataInit();
        GmEfctZoneBuildDataInit(4);
    }

    private static void gmGameDatBuildStageFinalBoss04()
    {
        GmGmkLandBuild();
        GmEfctZoneBuildDataInit(4);
    }

    private static void gmGameDatBuildStageFinalBoss05()
    {
        GmBoss5Build();
        GmGmkLandBuild();
        GmGmkSteamPipeBuild();
        GmGmkBumperBuild();
        GmGmkFlipperBuild();
        GmGmkShutterBuild();
        GmGmkBoss3PillarBuild();
        GmGmkNeedleNeonBuild();
        GmEfctBossCmnBuildDataInit();
        GmEfctZoneBuildDataInit(4);
    }

    private static void gmGameDatBuildSS01()
    {
        GmGmkSsSquareBuild();
        GmGmkSsCircleBuild();
        GmGmkSsEnduranceBuild();
        GmGmkSsGoalBuild();
        GmGmkSsEmeraldBuild();
        GmGmkSsTimeBuild();
        GmGmkSsRingGateBuild();
        GmGmkSsArrowBuild();
        GmGmkSsOblongBuild();
        GmGmkBobbinBuild();
        GmEfctZoneBuildDataInit(5);
        GmStartMsgBuild();
    }

    private static void gmGameDatBuildEnding()
    {
        GmGmkLandBuild();
        GmEfctZoneBuildDataInit(0);
        GmEndingBuild();
        DmStfrlMdlCtrlRingBuild();
        DmStfrlMdlCtrlBoss1Build();
        DmStfrlMdlCtrlSonicBuild();
    }

    private static void gmGameDatFlushStage11()
    {
        GmEneMotoraFlush();
        GmEneGabuFlush();
        GmEneStingFlush();
        GmEneMereonFlush();
        GmGmkLandFlush();
        GmGmkBridgeFlush();
        GmGmkBreakObjFlush();
        GmGmkBreakWallFlush();
        GmGmkBreakLandFlush();
        GmEfctEneFlushDataInit(0);
        GmEfctZoneFlushDataInit(0);
    }

    private static void gmGameDatFlushStage12()
    {
        GmEneMotoraFlush();
        GmEneGabuFlush();
        GmEneStingFlush();
        GmEneMereonFlush();
        GmGmkLandFlush();
        GmGmkTarzanRopeFlush();
        GmGmkBridgeFlush();
        GmGmkBreakObjFlush();
        GmGmkBreakWallFlush();
        GmGmkBreakLandFlush();
        GmEfctEneFlushDataInit(0);
        GmEfctZoneFlushDataInit(0);
    }

    private static void gmGameDatFlushStage13()
    {
        GmEneMotoraFlush();
        GmEneGabuFlush();
        GmEneStingFlush();
        GmEneMereonFlush();
        GmGmkLandFlush();
        GmGmkPulleyFlush();
        GmGmkBridgeFlush();
        GmGmkBreakObjFlush();
        GmGmkBreakWallFlush();
        GmGmkBreakLandFlush();
        GmEfctEneFlushDataInit(0);
        GmEfctZoneFlushDataInit(0);
    }

    private static void gmGameDatFlushStage1Boss()
    {
        GmBoss1Flush();
        GmGmkLandFlush();
        GmGmkCapsuleFlush();
        GmEfctZoneFlushDataInit(0);
        GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStage21()
    {
        GmEneGardonFlush();
        GmEneHaroFlush();
        GmGmkLandFlush();
        GmGmkBumperFlush();
        GmGmkEnBmprFlush();
        GmGmkBobbinFlush();
        GmGmkFlipperFlush();
        GmGmkStopperFlush();
        GmGmkSlotFlush();
        GmGmkSpCtpltFlush();
        GmGmkBreakObjFlush();
        GmGmkBreakWallFlush();
        GmEfctEneFlushDataInit(1);
        GmEfctZoneFlushDataInit(1);
    }

    private static void gmGameDatFlushStage22()
    {
        GmGmkBumperFlush();
        GmGmkEnBmprFlush();
        GmGmkBobbinFlush();
        GmGmkFlipperFlush();
        GmGmkStopperFlush();
        GmGmkSlotFlush();
        GmGmkSpCtpltFlush();
        GmGmkSsArrowFlush();
        GmEfctEneFlushDataInit(1);
        GmEfctZoneFlushDataInit(1);
        GmStartMsgFlush();
    }

    private static void gmGameDatFlushStage23()
    {
        GmEneGardonFlush();
        GmEneHaroFlush();
        GmGmkLandFlush();
        GmGmkBumperFlush();
        GmGmkEnBmprFlush();
        GmGmkBobbinFlush();
        GmGmkFlipperFlush();
        GmGmkCannonFlush();
        GmGmkSpCtpltFlush();
        GmGmkBreakObjFlush();
        GmGmkBreakWallFlush();
        GmGmkSsArrowFlush();
        GmEfctEneFlushDataInit(1);
        GmEfctZoneFlushDataInit(1);
    }

    private static void gmGameDatFlushStage2Boss()
    {
        GmBoss2Flush();
        GmGmkCapsuleFlush();
        GmGmkBumperFlush();
        GmGmkEnBmprFlush();
        GmGmkFlipperFlush();
        GmGmkShutterFlush();
        GmGmkNeedleNeonFlush();
        GmEfctZoneFlushDataInit(1);
        GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStage31()
    {
        GmEneMoguFlush();
        GmEneUnidesFlush();
        GmEneUniuniFlush();
        GmEneBukuFlush();
        GmGmkLandFlush();
        GmGmkRockFlush();
        GmGmkSpearFlush();
        GmGmkBreakObjFlush();
        GmGmkBreakWallFlush();
        GmGmkRockRideFlush();
        GmGmkSwitchFlush();
        GmGmkSwWallFlush();
        GmEfctEneFlushDataInit(2);
        GmEfctZoneFlushDataInit(2);
    }

    private static void gmGameDatFlushStage32()
    {
        GmEneMoguFlush();
        GmGmkLandFlush();
        GmGmkSpearFlush();
        GmGmkBreakObjFlush();
        GmGmkBreakWallFlush();
        GmGmkTruckFlush();
        GmGmkSwitchFlush();
        GmGmkSwWallFlush();
        GmGmkDSignFlush();
        GmEfctEneFlushDataInit(2);
        GmEfctZoneFlushDataInit(2);
        GmStartMsgFlush();
    }

    private static void gmGameDatFlushStage33()
    {
        GmEneMoguFlush();
        GmEneUnidesFlush();
        GmEneUniuniFlush();
        GmEneBukuFlush();
        GmGmkLandFlush();
        GmGmkWaterSliderFlush();
        GmGmkSpearFlush();
        GmGmkPressWallFlush();
        GmGmkBreakObjFlush();
        GmGmkBreakWallFlush();
        GmGmkDrainTankFlush();
        GmGmkSwitchFlush();
        GmGmkSwWallFlush();
        GmEfctEneFlushDataInit(2);
        GmEfctZoneFlushDataInit(2);
    }

    private static void gmGameDatFlushStage3Boss()
    {
        GmBoss3Flush();
        GmEneMoguFlush();
        GmEneUnidesFlush();
        GmGmkLandFlush();
        GmGmkCapsuleFlush();
        GmGmkSpearFlush();
        GmGmkBoss3PillarFlush();
        GmEfctEneFlushDataInit(2);
        GmEfctZoneFlushDataInit(2);
        GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStage41()
    {
        GmEneTStarFlush();
        GmEneKaniFlush();
        GmEneKamaFlush();
        GmGmkLandFlush();
        GmGmkPistonFlush();
        GmGmkBeltConveyorFlush();
        GmGmkUpBumperFlush();
        GmGmkSteamPipeFlush();
        GmGmkPopSteamFlush();
        GmGmkBreakWallFlush();
        GmGmkSwitchFlush();
        GmGmkSwWallFlush();
        GmEfctEneFlushDataInit(3);
        GmEfctZoneFlushDataInit(3);
    }

    private static void gmGameDatFlushStage42()
    {
        GmEneTStarFlush();
        GmEneKaniFlush();
        GmEneKamaFlush();
        GmGmkLandFlush();
        GmGmkPistonFlush();
        GmGmkBeltConveyorFlush();
        GmGmkUpBumperFlush();
        GmGmkSteamPipeFlush();
        GmGmkPopSteamFlush();
        GmGmkBreakWallFlush();
        GmGmkGearFlush();
        GmGmkSwitchFlush();
        GmGmkSwWallFlush();
        GmEfctEneFlushDataInit(3);
        GmEfctZoneFlushDataInit(3);
    }

    private static void gmGameDatFlushStage43()
    {
        GmEneTStarFlush();
        GmEneKaniFlush();
        GmEneKamaFlush();
        GmGmkLandFlush();
        GmGmkPistonFlush();
        GmGmkBeltConveyorFlush();
        GmGmkUpBumperFlush();
        GmGmkSeesawFlush();
        GmGmkSteamPipeFlush();
        GmGmkPopSteamFlush();
        GmGmkPressWallFlush();
        GmGmkPressPillarFlush();
        GmGmkBreakWallFlush();
        GmGmkGearFlush();
        GmGmkSwitchFlush();
        GmGmkSwWallFlush();
        GmEfctEneFlushDataInit(3);
        GmEfctZoneFlushDataInit(3);
    }

    private static void gmGameDatFlushStage4Boss()
    {
        GmBoss4Flush();
        GmGmkLandFlush();
        GmGmkCapsuleFlush();
        GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStageFinalBoss01()
    {
        GmGmkLandFlush();
        GmGmkSteamPipeFlush();
        GmGmkBumperFlush();
        GmGmkFlipperFlush();
        GmGmkShutterFlush();
        GmGmkBoss3PillarFlush();
        GmGmkNeedleNeonFlush();
        GmEfctZoneFlushDataInit(4);
        GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStageFinalBoss02()
    {
        GmGmkLandFlush();
        GmGmkSteamPipeFlush();
        GmGmkBumperFlush();
        GmGmkFlipperFlush();
        GmGmkShutterFlush();
        GmGmkBoss3PillarFlush();
        GmGmkNeedleNeonFlush();
        GmEfctZoneFlushDataInit(4);
        GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStageFinalBoss03()
    {
        GmGmkLandFlush();
        GmGmkSteamPipeFlush();
        GmGmkBumperFlush();
        GmGmkFlipperFlush();
        GmGmkShutterFlush();
        GmGmkBoss3PillarFlush();
        GmGmkNeedleNeonFlush();
        GmEfctZoneFlushDataInit(4);
        GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStageFinalBoss04()
    {
        GmGmkLandFlush();
        GmEfctZoneFlushDataInit(4);
    }

    private static void gmGameDatFlushStageFinalBoss05()
    {
        GmBoss5Flush();
        GmGmkLandFlush();
        GmGmkSteamPipeFlush();
        GmGmkBumperFlush();
        GmGmkFlipperFlush();
        GmGmkShutterFlush();
        GmGmkBoss3PillarFlush();
        GmGmkNeedleNeonFlush();
        GmEfctZoneFlushDataInit(4);
        GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushSS01()
    {
        GmGmkSsSquareFlush();
        GmGmkSsCircleFlush();
        GmGmkSsEnduranceFlush();
        GmGmkSsGoalFlush();
        GmGmkSsEmeraldFlush();
        GmGmkSsTimeFlush();
        GmGmkSsRingGateFlush();
        GmGmkSsArrowFlush();
        GmGmkSsOblongFlush();
        GmGmkBobbinFlush();
        GmEfctZoneFlushDataInit(5);
        GmStartMsgFlush();
    }

    private static void gmGameDatFlushEnding()
    {
        GmGmkLandFlush();
        GmEfctZoneFlushDataInit(0);
        GmEndingFlush();
        DmStfrlMdlCtrlSonicFlush();
        DmStfrlMdlCtrlRingFlush();
        DmStfrlMdlCtrlBoss1Flush();
    }

    private static void gmGameDatBuildStageF_BB1()
    {
        GmBoss1Build();
    }

    private static void gmGameDatBuildStageF_BB2()
    {
        GmBoss2Build();
    }

    private static void gmGameDatBuildStageF_BB3()
    {
        GmBoss3Build();
    }

    private static void gmGameDatBuildStageF_BB4()
    {
        GmBoss4Build();
    }

    private static void gmGameDatBuildStageF_BBF()
    {
        GmBoss5Build();
    }

    private static void gmGameDatFlushStageF_BB1()
    {
        GmBoss1Flush();
    }

    private static void gmGameDatFlushStageF_BB2()
    {
        GmBoss2Flush();
    }

    private static void gmGameDatFlushStageF_BB3()
    {
        GmBoss3Flush();
    }

    private static void gmGameDatFlushStageF_BB4()
    {
        GmBoss4Flush();
    }

    private static void gmGameDatFlushStageF_BBF()
    {
        GmBoss5Flush();
    }

}