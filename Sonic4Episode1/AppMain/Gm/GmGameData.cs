using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

public partial class AppMain
{

    private static void GmGameDatLoadBossBattleExit()
    {
        if (AppMain.gm_main_load_bossbattle_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_main_load_bossbattle_tcb);
        AppMain.gm_main_load_bossbattle_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void GmGameDatReleaseBossBattleStart(int boss_type)
    {
        AppMain.GmGameDatFlushBossBattleInit();
        AppMain.GmGameDatFlushBossBattle(boss_type);
        AppMain.gm_main_release_bossbattle_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmMainDataReleaseBoosBattleMgr_FlushWait), (AppMain.GSF_TASK_PROCEDURE)null, 0U, ushort.MaxValue, 2048U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_MAIN_LOAD_BB_MGR_WORK()), "GM_RELEASEBBM");
        AppMain.GMS_MAIN_LOAD_BB_MGR_WORK work = (AppMain.GMS_MAIN_LOAD_BB_MGR_WORK)AppMain.gm_main_release_bossbattle_tcb.work;
        work.boss_type = boss_type;
        work.b_end = false;
        AppMain.g_gm_main_system.game_flag |= 4194304U;
    }

    private static bool GmMainDatReleaseBossBattleReleaseCheck()
    {
        return AppMain.g_gm_main_system.boss_load_no == -1 && ((int)AppMain.g_gm_main_system.game_flag & 2097152) == 0;
    }

    private static bool GmMainDatReleaseBossBattleReleaseNowCheck()
    {
        return ((int)AppMain.g_gm_main_system.game_flag & 4194304) != 0;
    }

    private static void GmGameDatReleaseBossBattleExit()
    {
        if (AppMain.gm_main_release_bossbattle_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_main_release_bossbattle_tcb);
        AppMain.gm_main_release_bossbattle_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    public static AppMain.AMS_AMB_HEADER GmGameDatGetCockpitData()
    {
        return AppMain.g_gm_gamedat_cockpit_main_arc;
    }

    public static void GmGameDatLoadInit(int proc_type, ushort stage_id, short[] char_id_list)
    {
        AppMain.MTS_TASK_TCB mtsTaskTcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmDataLoadMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmDataLoadDest), 0U, ushort.MaxValue, 2048U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GAMEDAT_LOAD_WORK()), "GM_LOAD");
        AppMain.gm_gamedat_load_tcb = mtsTaskTcb;
        AppMain.GMS_GAMEDAT_LOAD_WORK work = (AppMain.GMS_GAMEDAT_LOAD_WORK)mtsTaskTcb.work;
        AppMain.gm_gamedat_load_work = work;
        work.Clear();
        AppMain.gm_gamedat_load_work.stage_id = stage_id;
        for (int index = 0; index < 1; ++index)
            work.char_id[index] = (ushort)char_id_list[index];
        work.proc_type = proc_type;
        AppMain.ArrayPointer<AppMain.GMS_GAMEDAT_LOAD_CONTEXT> arrayPointer = new AppMain.ArrayPointer<AppMain.GMS_GAMEDAT_LOAD_CONTEXT>(work.context);
        AppMain.GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo1 = AppMain.gm_gamedat_tbl_common_info_tbl[0];
        int index1 = 0;
        while (index1 < gmsGamedatLoadInfo1.num)
        {
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).load_data = gmsGamedatLoadInfo1.data_tbl[index1];
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).data_no = (ushort)index1;
            int num = (int)AppMain.gmGameDatLoad((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)arrayPointer);
            ++index1;
            ++arrayPointer;
            ++work.context_num;
        }
        for (int index2 = 0; index2 < 1; ++index2)
        {
            if (work.char_id[index2] != (ushort)short.MaxValue)
            {
                AppMain.GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo2 = AppMain.gm_gamedat_tbl_player_info_tbl[(int)work.char_id[index2]];
                int index3 = 0;
                while (index3 < gmsGamedatLoadInfo2.num)
                {
                    ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).load_data = gmsGamedatLoadInfo2.data_tbl[index3];
                    ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).char_id = work.char_id[index2];
                    ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).ply_no = (ushort)index2;
                    ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).data_no = (ushort)index3;
                    int num = (int)AppMain.gmGameDatLoad((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)arrayPointer);
                    ++index3;
                    ++arrayPointer;
                    ++work.context_num;
                }
            }
        }
        AppMain.GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo3 = AppMain.gm_gamedat_tbl_map_info_tbl[(int)stage_id];
        int index4 = 0;
        while (index4 < gmsGamedatLoadInfo3.num)
        {
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).load_data = gmsGamedatLoadInfo3.data_tbl[index4];
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).stage_id = stage_id;
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).data_no = (ushort)index4;
            int num = (int)AppMain.gmGameDatLoad((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)arrayPointer);
            ++index4;
            ++arrayPointer;
            ++work.context_num;
        }
        AppMain.GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo4 = AppMain.gm_gamedat_tbl_effect_info_tbl[(int)stage_id];
        int index5 = 0;
        while (index5 < gmsGamedatLoadInfo4.num)
        {
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).load_data = gmsGamedatLoadInfo4.data_tbl[index5];
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).stage_id = stage_id;
            int num = (int)AppMain.gmGameDatLoad((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)arrayPointer);
            ++index5;
            ++arrayPointer;
            ++work.context_num;
        }
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)16)
        {
            AppMain.GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo2 = AppMain.gm_gamedat_tbl_enemy_info_tbl[(int)stage_id];
            int index2 = 0;
            while (index2 < gmsGamedatLoadInfo2.num)
            {
                ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).load_data = gmsGamedatLoadInfo2.data_tbl[index2];
                ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).stage_id = stage_id;
                int num = (int)AppMain.gmGameDatLoad((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)arrayPointer);
                ++index2;
                ++arrayPointer;
                ++work.context_num;
            }
        }
        AppMain.GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo5 = AppMain.gm_gamedat_tbl_gimmick_common_info_tbl[0];
        int index6 = 0;
        while (index6 < gmsGamedatLoadInfo5.num)
        {
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).load_data = gmsGamedatLoadInfo5.data_tbl[index6];
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).stage_id = stage_id;
            int num = (int)AppMain.gmGameDatLoad((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)arrayPointer);
            ++index6;
            ++arrayPointer;
            ++work.context_num;
        }
        AppMain.GMS_GAMEDAT_LOAD_INFO gmsGamedatLoadInfo6 = AppMain.gm_gamedat_tbl_gimmick_info_tbl[(int)stage_id];
        int index7 = 0;
        while (index7 < gmsGamedatLoadInfo6.num)
        {
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).load_data = gmsGamedatLoadInfo6.data_tbl[index7];
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer).stage_id = stage_id;
            int num = (int)AppMain.gmGameDatLoad((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)arrayPointer);
            ++index7;
            ++arrayPointer;
            ++work.context_num;
        }
    }

    public static void GmGameDatLoadPost()
    {
        AppMain.GMS_GAMEDAT_LOAD_WORK gmGamedatLoadWork = AppMain.gm_gamedat_load_work;
        if (gmGamedatLoadWork == null)
            return;
        gmGamedatLoadWork.proc_type = 0;
    }

    public static AppMain.GME_GAMEDAT_LOAD_PROGRESS GmGameDatLoadCheck()
    {
        if (AppMain.gm_gamedat_load_work == null)
            return AppMain.GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_NOLOAD;
        if (AppMain.gm_gamedat_load_work.post_finish)
            return AppMain.GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_COMPLETE;
        return AppMain.gm_gamedat_load_work.load_finish ? AppMain.GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_LOADFINISH : AppMain.GME_GAMEDAT_LOAD_PROGRESS.GMD_GAMEDAT_LOAD_PROGRESS_LOADING;
    }

    public static void GmGameDatLoadExit()
    {
        if (AppMain.gm_gamedat_load_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_gamedat_load_tcb);
    }

    public static void GmGameDatRelease()
    {
        AppMain.GmGameDatReleaseStandard();
        AppMain.GmGameDatReleaseArea();
    }

    public static void GmGameDatReleaseStandard()
    {
        AppMain.GmPlayerRelease();
        if (AppMain.g_gm_gamedat_cockpit_main_arc != null)
            AppMain.g_gm_gamedat_cockpit_main_arc = (AppMain.AMS_AMB_HEADER)null;
        for (int index = 0; index < 3; ++index)
        {
            if (AppMain.g_gm_gamedat_ring[index] != null)
                AppMain.g_gm_gamedat_ring[index] = (object)null;
        }
    }

    public static void GmGameDatReleaseArea()
    {
        AppMain.GmMapRelease();
        AppMain.GmMapFarRelease();
        AppMain.GmDecoRelease();
        AppMain.GmWaterSurfaceRelease();
        for (int index = 0; index < 11; ++index)
        {
            if (AppMain.g_gm_gamedat_effect[index] != null)
                AppMain.g_gm_gamedat_effect[index] = (object)null;
        }
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)16)
        {
            for (int index = 0; index < 44; ++index)
            {
                if (AppMain.g_gm_gamedat_enemy[index] != null)
                    AppMain.g_gm_gamedat_enemy[index] = (object)null;
            }
        }
        if (AppMain.g_gm_gamedat_enemy_arc != null)
            AppMain.g_gm_gamedat_enemy_arc = (AppMain.AMS_AMB_HEADER)null;
        for (int index = 0; index < 204; ++index)
        {
            if (AppMain.g_gm_gamedat_gimmick[index] != null)
                AppMain.g_gm_gamedat_gimmick[index] = (AppMain.AMS_AMB_HEADER)null;
        }
    }

    public static bool GmGameDatReleaseCheck()
    {
        return true;
    }

    public static object GmGameDatGetEnemyData(int data_no)
    {
        return AppMain.g_gm_gamedat_enemy[data_no - 658];
    }

    public static AppMain.AMS_AMB_HEADER GmGameDatGetGimmickData(int data_no)
    {
        return AppMain.g_gm_gamedat_gimmick[data_no - 789];
    }

    private static void GmGameDatLoadBoosBattleInit(int boss_type)
    {
        ushort num1 = (ushort)AppMain.g_gm_gamedat_bossbattle_stage_id_tbl[boss_type];
        AppMain.MTS_TASK_TCB mtsTaskTcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmDataLoadMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmDataLoadDest), 0U, ushort.MaxValue, 2048U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GAMEDAT_LOAD_WORK()), "GM_LOAD_BB");
        AppMain.gm_gamedat_load_tcb = mtsTaskTcb;
        AppMain.GMS_GAMEDAT_LOAD_WORK work = (AppMain.GMS_GAMEDAT_LOAD_WORK)mtsTaskTcb.work;
        AppMain.gm_gamedat_load_work = work;
        work.Clear();
        AppMain.gm_gamedat_load_work.stage_id = num1;
        work.proc_type = 0;
        AppMain.ArrayPointer<AppMain.GMS_GAMEDAT_LOAD_CONTEXT> arrayPointer1 = new AppMain.ArrayPointer<AppMain.GMS_GAMEDAT_LOAD_CONTEXT>(work.context);
        AppMain.ArrayPointer<AppMain.GMS_GAMEDAT_LOAD_INFO> arrayPointer2 = new AppMain.ArrayPointer<AppMain.GMS_GAMEDAT_LOAD_INFO>(AppMain.gm_gamedat_tbl_enemy_final_info_tbl, (int)num1);
        int num2 = 0;
        AppMain.ArrayPointer<AppMain.GMS_GAMEDAT_LOAD_DATA> dataTbl = (AppMain.ArrayPointer<AppMain.GMS_GAMEDAT_LOAD_DATA>)((AppMain.GMS_GAMEDAT_LOAD_INFO)~arrayPointer2).data_tbl;
        while (num2 < ((AppMain.GMS_GAMEDAT_LOAD_INFO)~arrayPointer2).num)
        {
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer1).load_data = (AppMain.GMS_GAMEDAT_LOAD_DATA)dataTbl;
            ((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)~arrayPointer1).stage_id = num1;
            int num3 = (int)AppMain.gmGameDatLoad((AppMain.GMS_GAMEDAT_LOAD_CONTEXT)arrayPointer1);
            ++num2;
            ++arrayPointer1;
            ++dataTbl;
            ++work.context_num;
        }
    }

    public static void GmGameDatBoosBattleRelease(int boss_type)
    {
        int num = AppMain.g_gm_gamedat_bossbattle_stage_id_tbl[boss_type];
        for (int index = 0; index < 44; ++index)
        {
            if (AppMain.g_gm_gamedat_enemy[index] != null)
                AppMain.g_gm_gamedat_enemy[index] = (object)null;
        }
        if (AppMain.g_gm_gamedat_enemy_arc == null)
            return;
        AppMain.g_gm_gamedat_enemy_arc = (AppMain.AMS_AMB_HEADER)null;
    }

    public static void gmDataLoadMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GAMEDAT_LOAD_WORK work = (AppMain.GMS_GAMEDAT_LOAD_WORK)tcb.work;
        AppMain.GME_GAMEDAT_LOAD_STATE gamedatLoadState = work.proc_type != 1 ? AppMain.GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_COMPLETE : AppMain.GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_LOADFINISH;
        for (int index = 0; index < work.context_num; ++index)
        {
            if (AppMain.gmGameDatLoad(work.context[index]) < gamedatLoadState)
                return;
        }
        work.load_finish = true;
        if (work.proc_type == 1)
        {
            AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmDataLoadMainPostWait));
        }
        else
        {
            work.post_finish = true;
            AppMain.mtTaskChangeTcbProcedure(tcb, (AppMain.GSF_TASK_PROCEDURE)null);
        }
    }

    public static void gmDataLoadMainPostWait(AppMain.MTS_TASK_TCB tcb)
    {
        if (((AppMain.GMS_GAMEDAT_LOAD_WORK)tcb.work).proc_type != 0)
            return;
        AppMain.mtTaskChangeTcbProcedure(tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmDataLoadMain));
        AppMain.gmDataLoadMain(tcb);
    }

    public static void gmDataLoadDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gm_gamedat_load_tcb = (AppMain.MTS_TASK_TCB)null;
        AppMain.gm_gamedat_load_work = (AppMain.GMS_GAMEDAT_LOAD_WORK)null;
    }

    public static object gmGameDatLoadAllocHead(string path)
    {
        return (object)null;
    }

    public static object gmGameDatLoadAllocHeadSub(string path)
    {
        return (object)null;
    }

    public static AppMain.GME_GAMEDAT_LOAD_STATE gmGameDatLoad(
      AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        return AppMain.gmGameDatLoadFileReq(context);
    }

    public static AppMain.GME_GAMEDAT_LOAD_STATE gmGameDatLoadFileReq(
      AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        if (context.state == AppMain.GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_COMPLETE || context.state == AppMain.GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_ERROR)
            return context.state;
        AppMain.GMS_GAMEDAT_LOAD_DATA loadData = context.load_data;
        if (context.fs_req == null)
        {
            context.file_path = loadData.path;
            if (loadData.proc_pre != null)
                loadData.proc_pre(context);
            int stageId = (int)context.stage_id;
            context.fs_req = 7 == stageId && "G_ZONE2/BOSS/BOSS02.AMB" == context.file_path || 11 == stageId && "G_ZONE3/BOSS/BOSS03.AMB" == context.file_path || (15 == stageId && "G_ZONEF/BOSS/BOSS04.AMB" == context.file_path || 16 == stageId && "G_ZONEF/BOSS/BOSS05.AMB" == context.file_path) ? AppMain.amFsReadBackground(context.file_path, 8192) : AppMain.amFsReadBackground(context.file_path, 65536);
            context.state = AppMain.GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_LOADING;
        }
        else if (AppMain.amFsIsComplete(context.fs_req))
        {
            context.state = AppMain.GME_GAMEDAT_LOAD_STATE.GMD_GAMEDAT_LOAD_STATE_COMPLETE;
            if (AppMain.gm_gamedat_load_work.proc_type != 1 && context.fs_req != null)
            {
                if (loadData.proc_post != null)
                    loadData.proc_post(context);
                AppMain.amFsClearRequest(context.fs_req);
                context.fs_req = (AppMain.AMS_FS)null;
            }
        }
        return context.state;
    }

    public static void gmGameDatLoadProcPostRing(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.g_gm_gamedat_ring[context.load_data.user_data - 2] = (object)context.fs_req;
        context.fs_req = (AppMain.AMS_FS)null;
    }

    public static void gmGameDatLoadProcPostPlayer(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.ObjDataSet(AppMain.g_gm_player_data_work[(int)context.ply_no][(int)context.data_no], (object)context.fs_req);
        context.fs_req = (AppMain.AMS_FS)null;
    }

    public static void gmGameDatLoadProcPostCockpit(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.g_gm_gamedat_cockpit_main_arc = AppMain.readAMBFile(context.fs_req);
        context.fs_req = (AppMain.AMS_FS)null;
    }

    public static void gmGameDatLoadProcPostMap(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.g_gm_gamedat_map[(int)context.data_no] = (object)AppMain.readAMBFile(context.fs_req);
        switch (context.data_no)
        {
            case 0:
                AppMain.AMS_AMB_HEADER header1 = AppMain.readAMBFile(context.fs_req);
                for (int index = 0; index < 9 && index < header1.file_num; ++index)
                {
                    switch (index)
                    {
                        case 0:
                        case 1:
                        case 4:
                        case 5:
                            AppMain.g_gm_gamedat_map_set[index] = (object)AppMain.readMPFile((AppMain.AmbChunk)AppMain.amBindGet(header1, index));
                            break;
                        case 2:
                        case 3:
                            AppMain.g_gm_gamedat_map_set[index] = (object)AppMain.readMDFile((AppMain.AmbChunk)AppMain.amBindGet(header1, index));
                            break;
                        default:
                            AppMain.g_gm_gamedat_map_set[index] = AppMain.amBindGet(header1, index);
                            break;
                    }
                }
                for (int index = 0; index < 10; index += 2)
                {
                    if (header1.file_num >= 11 + index)
                    {
                        AppMain.g_gm_gamedat_map_set_add[index] = (object)AppMain.readMPFile((AppMain.AmbChunk)AppMain.amBindGet(header1, 9 + index));
                        AppMain.g_gm_gamedat_map_set_add[index + 1] = (object)AppMain.readMDFile((AppMain.AmbChunk)AppMain.amBindGet(header1, 9 + index + 1));
                    }
                    else
                    {
                        AppMain.g_gm_gamedat_map_set_add[index] = (object)null;
                        AppMain.g_gm_gamedat_map_set_add[index + 1] = (object)null;
                    }
                }
                goto case 2;
            case 1:
                AppMain.AMS_AMB_HEADER gGmGamedat = (AppMain.AMS_AMB_HEADER)AppMain.g_gm_gamedat_map[(int)context.data_no];
                AppMain.TVX_FILE[] tvxFileArray = new AppMain.TVX_FILE[gGmGamedat.file_num];
                for (int index = 0; index < gGmGamedat.file_num; ++index)
                    tvxFileArray[index] = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(gGmGamedat, index));
                if (AppMain.g_gs_main_sys_info.stage_id >= (ushort)0 && AppMain.g_gs_main_sys_info.stage_id <= (ushort)3)
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
                            if ((double)tvxFileArray[index1].vertexes[0][index2].x == (double)val1_1)
                                tvxFileArray[index1].vertexes[0][index2].x -= 0.5f;
                            else if ((double)tvxFileArray[index1].vertexes[0][index2].x == (double)val1_2)
                                tvxFileArray[index1].vertexes[0][index2].x += 0.5f;
                            if ((double)tvxFileArray[index1].vertexes[0][index2].y == (double)val1_3)
                                tvxFileArray[index1].vertexes[0][index2].y -= 0.5f;
                            else if ((double)tvxFileArray[index1].vertexes[0][index2].y == (double)val1_4)
                                tvxFileArray[index1].vertexes[0][index2].y += 0.5f;
                        }
                    }
                    if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2)
                    {
                        tvxFileArray[103].vertexes[0][1].x = 63.5f;
                        tvxFileArray[103].vertexes[0][3].x = 63.5f;
                    }
                }
                if (AppMain.g_gs_main_sys_info.stage_id >= (ushort)4 && AppMain.g_gs_main_sys_info.stage_id <= (ushort)7)
                {
                    tvxFileArray[201].vertexes[1][1].x = 64f;
                    tvxFileArray[201].vertexes[1][3].x = 64f;
                }
                if (AppMain.g_gs_main_sys_info.stage_id >= (ushort)8 && AppMain.g_gs_main_sys_info.stage_id <= (ushort)11)
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
                AppMain.g_gm_gamedat_map[(int)context.data_no] = (object)tvxFileArray;
                goto case 2;
            case 2:
                context.fs_req = (AppMain.AMS_FS)null;
                break;
            case 3:
                AppMain.AMS_AMB_HEADER header2 = AppMain.readAMBFile(context.fs_req);
                for (int index = 0; index < 3 && index < header2.file_num; ++index)
                    AppMain.g_gm_gamedat_map_attr_set[index] = AppMain.amBindGet(header2, index);
                goto case 2;
        }
    }

    public static void gmGameDatLoadProcPostMapFar(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.GmMapFarInitData(AppMain.readAMBFile(context.fs_req));
        context.fs_req = (AppMain.AMS_FS)null;
    }

    public static void gmGameDatLoadProcPostEffect(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.g_gm_gamedat_effect[context.load_data.user_data - 5] = (object)AppMain.readAMBFile(context.fs_req);
        context.fs_req = (AppMain.AMS_FS)null;
    }

    public static void gmGameDatLoadProcPostEnemy(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.g_gm_gamedat_enemy[context.load_data.user_data - 658] = (object)context.fs_req;
        context.fs_req = (AppMain.AMS_FS)null;
    }

    public static void gmGameDatLoadProcPostBoss(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.g_gm_gamedat_enemy_arc = AppMain.readAMBFile(context.fs_req);
        context.fs_req = (AppMain.AMS_FS)null;
    }

    public static void gmGameDatLoadProcPostGimmick(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.g_gm_gamedat_gimmick[context.load_data.user_data - 789] = AppMain.readAMBFile(context.fs_req);
        context.fs_req = (AppMain.AMS_FS)null;
    }

    public static void gmGameDatLoadProcPostDeco(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.GmDecoInitData(AppMain.readAMBFile(context.fs_req));
        context.fs_req = (AppMain.AMS_FS)null;
    }

    public static void gmGameDatLoadProcPostWaterSurface(AppMain.GMS_GAMEDAT_LOAD_CONTEXT context)
    {
        AppMain.GmWaterSurfaceInitData(AppMain.readAMBFile(context.fs_req));
        context.fs_req = (AppMain.AMS_FS)null;
    }

    private static void GmGameDatBuildInit()
    {
        AppMain.ObjLoadSetInitDrawFlag(true);
        AppMain.GmGameDBuildModelBuildInit();
        for (int index = 0; index < 44; ++index)
        {
            if (AppMain.g_gm_gamedat_enemy[index] != null)
            {
                AppMain.OBS_DATA_WORK pWork = AppMain.ObjDataGet(658 + index);
                AppMain.ObjDataSet(pWork, AppMain.g_gm_gamedat_enemy[index]);
                pWork.num |= (ushort)32768;
            }
        }
        for (int index = 0; index < 204; ++index)
        {
            if (AppMain.g_gm_gamedat_gimmick[index] != null)
            {
                AppMain.OBS_DATA_WORK pWork = AppMain.ObjDataGet(789 + index);
                AppMain.ObjDataSet(pWork, (object)AppMain.g_gm_gamedat_gimmick[index]);
                pWork.num |= (ushort)32768;
            }
        }
        for (int index = 0; index < 3; ++index)
        {
            if (AppMain.g_gm_gamedat_ring[index] != null)
            {
                AppMain.OBS_DATA_WORK pWork = AppMain.ObjDataGet(2 + index);
                AppMain.ObjDataSet(pWork, AppMain.g_gm_gamedat_ring[index]);
                pWork.num |= (ushort)32768;
            }
        }
        for (int index = 0; index < 11; ++index)
        {
            if (AppMain.g_gm_gamedat_effect[index] != null)
            {
                AppMain.OBS_DATA_WORK pWork = AppMain.ObjDataGet(5 + index);
                AppMain.ObjDataSet(pWork, AppMain.g_gm_gamedat_effect[index]);
                pWork.num |= (ushort)32768;
            }
        }
    }

    private static void GmGameDatBuildStandard()
    {
        AppMain.GmPlayerBuild();
        AppMain.GmSoundBuild();
        AppMain.GmFixBuildDataInit();
        AppMain.GmClearDemoBuild();
        AppMain.GmStartDemoBuild();
        AppMain.GmOverBuildDataInit();
        AppMain.GmPauseMenuBuildStart();
        AppMain.GmEfctCmnBuildDataInit();
        AppMain.GmRingBuild();
    }

    private static void GmGameDatBuildArea()
    {
        AppMain.GmTvxBuild();
        AppMain.GmMapBuildDataInit();
        AppMain.GmMapBuildColData();
        AppMain.GmMapFarBuildData();
        AppMain.GmDecoBuildData();
        AppMain.GmWaterSurfaceBuildData();
        AppMain.GmEventDataBuild();
        AppMain.GmGmkSpringBuild();
        AppMain.GmGmkDashPanelBuild();
        AppMain.GmGmkGoalPanelBuild();
        AppMain.GmGmkItemBuild();
        AppMain.GmGmkNeedleBuild();
        AppMain.GmGmkPointMarkerBuild();
        AppMain.GmGmkAnimalBuild();
        AppMain.GmGmkSplRingBuild();
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)11 || !AppMain.GMM_MAIN_STAGE_IS_BOSS() && !AppMain.GMM_MAIN_STAGE_IS_SS() && !AppMain.GMM_MAIN_STAGE_IS_ENDING())
            AppMain.GmEneHariSenboBuild();
        AppMain.GmEfctBossBuildSingleDataInit();
        if (AppMain.gm_gamedat_build_area_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == null)
            return;
        AppMain.gm_gamedat_build_area_tbl[(int)AppMain.g_gs_main_sys_info.stage_id]();
    }

    private static bool GmGameDatBuildStandardCheck()
    {
        return AppMain.GmPlayerBuildCheck() && AppMain.GmSoundBuildCheck() && (AppMain.GmRingBuildCheck() != 0 && AppMain.GmFixBuildDataLoop()) && (AppMain.GmClearDemoBuildCheck() && AppMain.GmStartDemoBuildCheck() && (AppMain.GmOverBuildDataLoop() && AppMain.GmPauseMenuBuildIsFinished())) && AppMain.GmEfctCmnBuildDataLoop();
    }

    private static bool GmGameDatBuildAreaCheck()
    {
        bool flag = true;
        if (!AppMain.GmMapBuildDataLoop())
            flag = false;
        if (!AppMain.GmMapFarCheckLoading())
            flag = false;
        if (!AppMain.GmDecoCheckLoading())
            flag = false;
        if (!AppMain.GmWaterSurfaceCheckLoading())
            flag = false;
        if (!AppMain.GmGameDBuildCheckBuildModel())
            flag = false;
        if (!AppMain.GmEfctZoneBuildDataLoop())
            flag = false;
        if (!AppMain.GmEfctEneBuildDataLoop())
            flag = false;
        if (!AppMain.GmEfctBossCmnBuildDataLoop())
            flag = false;
        if (!AppMain.GmEfctBossBuildSingleDataLoop())
            flag = false;
        if (!AppMain.GmStartMsgBuildCheck())
            flag = false;
        return flag;
    }

    private static void GmGameDatFlushInit()
    {
        AppMain.ObjLoadSetInitDrawFlag(false);
        AppMain.GmGameDBuildModelFlushInit();
    }

    private static void GmGameDatFlushStandard()
    {
        AppMain.GmRingFlush();
        AppMain.GmEfctCmnFlushDataInit();
        AppMain.GmPauseMenuFlushStart();
        AppMain.GmOverFlushDataInit();
        AppMain.GmStartDemoFlush();
        AppMain.GmClearDemoFlush();
        AppMain.GmFixFlushDataInit();
        AppMain.GmSoundFlush();
        AppMain.GmPlayerFlush();
    }

    private static void GmGameDatFlushArea()
    {
        if (AppMain.gm_gamedat_flush_area_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] != null)
            AppMain.gm_gamedat_flush_area_tbl[(int)AppMain.g_gs_main_sys_info.stage_id]();
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)11 || !AppMain.GMM_MAIN_STAGE_IS_BOSS() && !AppMain.GMM_MAIN_STAGE_IS_SS() && !AppMain.GMM_MAIN_STAGE_IS_ENDING())
            AppMain.GmEneHariSenboFlush();
        AppMain.GmGmkSplRingFlush();
        AppMain.GmGmkAnimalFlush();
        AppMain.GmGmkPointMarkerFlush();
        AppMain.GmGmkNeedleFlush();
        AppMain.GmGmkItemFlush();
        AppMain.GmGmkGoalPanelFlush();
        AppMain.GmGmkDashPanelFlush();
        AppMain.GmGmkSpringFlush();
        AppMain.GmEventDataFlush();
        AppMain.GmWaterSurfaceFlushData();
        AppMain.GmDecoFlushData();
        AppMain.GmMapFarFlushData();
        AppMain.GmMapFlushColData();
        AppMain.GmMapFlushData();
        AppMain.GmTvxFlush();
    }

    private static bool GmGameDatFlushStandardCheck()
    {
        return AppMain.GmRingFlushCheck() != 0 && AppMain.GmEfctCmnFlushDataLoop() && (AppMain.GmFixFlushDataLoop() && AppMain.GmStartDemoFlushCheck()) && (AppMain.GmClearDemoFlushCheck() && AppMain.GmOverFlushDataLoop() && (AppMain.GmPauseMenuFlushIsFinished() && AppMain.GmGameDBuildCheckFlushModel())) && (AppMain.GmWaterSurfaceCheckFlush() && AppMain.GmPlayerFlushCheck());
    }

    private static bool GmGameDatFlushAreaCheck()
    {
        bool flag = true;
        if (!AppMain.GmStartMsgFlushCheck())
            flag = false;
        if (!AppMain.GmEfctZoneFlushDataLoop())
            flag = false;
        if (!AppMain.GmEfctEneFlushDataLoop())
            flag = false;
        if (!AppMain.GmEfctBossCmnFlushDataLoop())
            flag = false;
        if (!AppMain.GmEfctBossFlushSingleDataLoop())
            flag = false;
        if (!AppMain.GmMapFlushDataLoop())
            flag = false;
        if (!AppMain.GmDecoCheckFlushing())
            flag = false;
        return flag;
    }

    private static void GmGameDatBuildBossBattleInit()
    {
        AppMain.GmGameDBuildModelBuildInit();
        for (int index = 0; index < 44; ++index)
        {
            if (AppMain.g_gm_gamedat_enemy[index] != null)
            {
                AppMain.OBS_DATA_WORK pWork = AppMain.ObjDataGet(658 + index);
                AppMain.ObjDataSet(pWork, AppMain.g_gm_gamedat_enemy[index]);
                pWork.num |= (ushort)32768;
            }
        }
    }

    private static void GmGameDatBuildBossBattle(int boss_type)
    {
        int num = AppMain.g_gm_gamedat_bossbattle_stage_id_tbl[boss_type];
        AppMain.GmEfctBossBuildSingleDataInit();
        if (AppMain.gm_gamedat_build_boss_buttle_tbl[boss_type] == null)
            return;
        AppMain.gm_gamedat_build_boss_buttle_tbl[boss_type]();
    }

    private static bool GmGameDatBuildBossBattleCheck()
    {
        bool flag = true;
        if (!AppMain.GmGameDBuildCheckBuildModel())
            flag = false;
        if (!AppMain.GmEfctZoneBuildDataLoop())
            flag = false;
        if (!AppMain.GmEfctEneBuildDataLoop())
            flag = false;
        if (!AppMain.GmEfctBossCmnBuildDataLoop())
            flag = false;
        if (!AppMain.GmEfctBossBuildSingleDataLoop())
            flag = false;
        return flag;
    }

    private static void GmGameDatFlushBossBattleInit()
    {
        AppMain.GmGameDBuildModelFlushInit();
    }

    private static void GmGameDatFlushBossBattle(int boss_type)
    {
        int num = AppMain.g_gm_gamedat_bossbattle_stage_id_tbl[boss_type];
        if (AppMain.gm_gamedat_flush_boss_buttle_tbl[boss_type] == null)
            return;
        AppMain.gm_gamedat_flush_boss_buttle_tbl[boss_type]();
    }

    private static bool GmGameDatFlushBossBattleCheck()
    {
        bool flag = true;
        if (!AppMain.GmGameDBuildCheckFlushModel())
            flag = false;
        if (!AppMain.GmEfctZoneFlushDataLoop())
            flag = false;
        if (!AppMain.GmEfctBossFlushSingleDataLoop())
            flag = false;
        return flag;
    }

    private static void GmGameDatFlashRestart()
    {
        AppMain.GmEventDataFlush();
    }

    private static void GmGameDatReBuildRestart()
    {
        AppMain.GmEventDataBuild();
        AppMain.GmMapBuildColData();
        AppMain.GmGmkSwitchReBuild();
        AppMain.GmGmkPressPillarClear();
    }

    private static bool GmGameDatReBuildRestartCheck()
    {
        return true;
    }

    private static void gmGameDatBuildStage11()
    {
        AppMain.GmEneMotoraBuild();
        AppMain.GmEneGabuBuild();
        AppMain.GmEneStingBuild();
        AppMain.GmEneMereonBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkBridgeBuild();
        AppMain.GmGmkBreakLandBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkBreakObjBuild();
        AppMain.GmEfctEneBuildDataInit(0);
        AppMain.GmEfctZoneBuildDataInit(0);
    }

    private static void gmGameDatBuildStage12()
    {
        AppMain.GmEneMotoraBuild();
        AppMain.GmEneGabuBuild();
        AppMain.GmEneStingBuild();
        AppMain.GmEneMereonBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkTarzanRopeBuild();
        AppMain.GmGmkBridgeBuild();
        AppMain.GmGmkBreakLandBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkBreakObjBuild();
        AppMain.GmEfctEneBuildDataInit(0);
        AppMain.GmEfctZoneBuildDataInit(0);
    }

    private static void gmGameDatBuildStage13()
    {
        AppMain.GmEneMotoraBuild();
        AppMain.GmEneGabuBuild();
        AppMain.GmEneStingBuild();
        AppMain.GmEneMereonBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkPulleyBuild();
        AppMain.GmGmkBridgeBuild();
        AppMain.GmGmkBreakLandBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkBreakObjBuild();
        AppMain.GmEfctEneBuildDataInit(0);
        AppMain.GmEfctZoneBuildDataInit(0);
    }

    private static void gmGameDatBuildStage1Boss()
    {
        AppMain.GmBoss1Build();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkCapsuleBuild();
        AppMain.GmEfctZoneBuildDataInit(0);
        AppMain.GmEfctBossCmnBuildDataInit();
    }

    private static void gmGameDatBuildStage21()
    {
        AppMain.GmEneGardonBuild();
        AppMain.GmEneHaroBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkBumperBuild();
        AppMain.GmGmkEnBmprBuild();
        AppMain.GmGmkBobbinBuild();
        AppMain.GmGmkFlipperBuild();
        AppMain.GmGmkStopperBuild();
        AppMain.GmGmkSlotBuild();
        AppMain.GmGmkSpCtpltBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkBreakObjBuild();
        AppMain.GmEfctEneBuildDataInit(1);
        AppMain.GmEfctZoneBuildDataInit(1);
    }

    private static void gmGameDatBuildStage22()
    {
        AppMain.GmGmkBumperBuild();
        AppMain.GmGmkEnBmprBuild();
        AppMain.GmGmkBobbinBuild();
        AppMain.GmGmkFlipperBuild();
        AppMain.GmGmkStopperBuild();
        AppMain.GmGmkSlotBuild();
        AppMain.GmGmkSpCtpltBuild();
        AppMain.GmGmkSsArrowBuild();
        AppMain.GmEfctEneBuildDataInit(1);
        AppMain.GmEfctZoneBuildDataInit(1);
        AppMain.GmStartMsgBuild();
    }

    private static void gmGameDatBuildStage23()
    {
        AppMain.GmEneGardonBuild();
        AppMain.GmEneHaroBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkBumperBuild();
        AppMain.GmGmkEnBmprBuild();
        AppMain.GmGmkBobbinBuild();
        AppMain.GmGmkFlipperBuild();
        AppMain.GmGmkCannonBuild();
        AppMain.GmGmkSpCtpltBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkBreakObjBuild();
        AppMain.GmGmkSsArrowBuild();
        AppMain.GmEfctEneBuildDataInit(1);
        AppMain.GmEfctZoneBuildDataInit(1);
    }

    private static void gmGameDatBuildStage2Boss()
    {
        AppMain.GmBoss2Build();
        AppMain.GmGmkCapsuleBuild();
        AppMain.GmGmkBumperBuild();
        AppMain.GmGmkEnBmprBuild();
        AppMain.GmGmkFlipperBuild();
        AppMain.GmGmkShutterBuild();
        AppMain.GmGmkNeedleNeonBuild();
        AppMain.GmEfctBossCmnBuildDataInit();
        AppMain.GmEfctZoneBuildDataInit(1);
    }

    private static void gmGameDatBuildStage31()
    {
        AppMain.GmEneMoguBuild();
        AppMain.GmEneUnidesBuild();
        AppMain.GmEneUniuniBuild();
        AppMain.GmEneBukuBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkRockBuild();
        AppMain.GmGmkSpearBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkBreakObjBuild();
        AppMain.GmGmkRockRideBuild();
        AppMain.GmGmkSwitchBuildTypeZone3();
        AppMain.GmGmkSwWallBuild();
        AppMain.GmEfctEneBuildDataInit(2);
        AppMain.GmEfctZoneBuildDataInit(2);
    }

    private static void gmGameDatBuildStage32()
    {
        AppMain.GmEneMoguBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkSpearBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkBreakObjBuild();
        AppMain.GmGmkTruckBuild();
        AppMain.GmGmkSwitchBuildTypeZone3();
        AppMain.GmGmkSwWallBuild();
        AppMain.GmGmkDSignBuild();
        AppMain.GmEfctEneBuildDataInit(2);
        AppMain.GmEfctZoneBuildDataInit(2);
        AppMain.GmStartMsgBuild();
    }

    private static void gmGameDatBuildStage33()
    {
        AppMain.GmEneMoguBuild();
        AppMain.GmEneUnidesBuild();
        AppMain.GmEneUniuniBuild();
        AppMain.GmEneBukuBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkWaterSliderBuild();
        AppMain.GmGmkSpearBuild();
        AppMain.GmGmkPressWallBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkBreakObjBuild();
        AppMain.GmGmkDrainTankBuild();
        AppMain.GmGmkSwitchBuildTypeZone3();
        AppMain.GmGmkSwWallBuild();
        AppMain.GmEfctEneBuildDataInit(2);
        AppMain.GmEfctZoneBuildDataInit(2);
    }

    private static void gmGameDatBuildStage3Boss()
    {
        AppMain.GmBoss3Build();
        AppMain.GmEneMoguBuild();
        AppMain.GmEneUnidesBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkCapsuleBuild();
        AppMain.GmGmkSpearBuild();
        AppMain.GmGmkBoss3PillarBuild();
        AppMain.GmEfctEneBuildDataInit(2);
        AppMain.GmEfctZoneBuildDataInit(2);
        AppMain.GmEfctBossCmnBuildDataInit();
    }

    private static void gmGameDatBuildStage41()
    {
        AppMain.GmEneTStarBuild();
        AppMain.GmEneKaniBuild();
        AppMain.GmEneKamaBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkPistonBuild();
        AppMain.GmGmkBeltConveyorBuild();
        AppMain.GmGmkUpBumperBuild();
        AppMain.GmGmkSteamPipeBuild();
        AppMain.GmGmkPopSteamBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkSwitchBuildTypeZone4();
        AppMain.GmGmkSwWallBuild();
        AppMain.GmEfctEneBuildDataInit(3);
        AppMain.GmEfctZoneBuildDataInit(3);
    }

    private static void gmGameDatBuildStage42()
    {
        AppMain.GmEneTStarBuild();
        AppMain.GmEneKaniBuild();
        AppMain.GmEneKamaBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkPistonBuild();
        AppMain.GmGmkBeltConveyorBuild();
        AppMain.GmGmkUpBumperBuild();
        AppMain.GmGmkSteamPipeBuild();
        AppMain.GmGmkPopSteamBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkGearBuild();
        AppMain.GmGmkSwitchBuildTypeZone4();
        AppMain.GmGmkSwWallBuild();
        AppMain.GmEfctEneBuildDataInit(3);
        AppMain.GmEfctZoneBuildDataInit(3);
    }

    private static void gmGameDatBuildStage43()
    {
        AppMain.GmEneTStarBuild();
        AppMain.GmEneKaniBuild();
        AppMain.GmEneKamaBuild();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkPistonBuild();
        AppMain.GmGmkBeltConveyorBuild();
        AppMain.GmGmkUpBumperBuild();
        AppMain.GmGmkSeesawBuild();
        AppMain.GmGmkSteamPipeBuild();
        AppMain.GmGmkPopSteamBuild();
        AppMain.GmGmkPressWallBuild();
        AppMain.GmGmkPressPillarBuild();
        AppMain.GmGmkBreakWallBuild();
        AppMain.GmGmkGearBuild();
        AppMain.GmGmkSwitchBuildTypeZone4();
        AppMain.GmGmkSwWallBuild();
        AppMain.GmEfctEneBuildDataInit(3);
        AppMain.GmEfctZoneBuildDataInit(3);
    }

    private static void gmGameDatBuildStage4Boss()
    {
        AppMain.GmBoss4Build();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkCapsuleBuild();
        AppMain.GmEfctBossCmnBuildDataInit();
    }

    private static void gmGameDatBuildStageFinalBoss01()
    {
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkSteamPipeBuild();
        AppMain.GmGmkBumperBuild();
        AppMain.GmGmkFlipperBuild();
        AppMain.GmGmkShutterBuild();
        AppMain.GmGmkBoss3PillarBuild();
        AppMain.GmGmkNeedleNeonBuild();
        AppMain.GmEfctBossCmnBuildDataInit();
        AppMain.GmEfctZoneBuildDataInit(4);
    }

    private static void gmGameDatBuildStageFinalBoss02()
    {
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkSteamPipeBuild();
        AppMain.GmGmkBumperBuild();
        AppMain.GmGmkFlipperBuild();
        AppMain.GmGmkShutterBuild();
        AppMain.GmGmkBoss3PillarBuild();
        AppMain.GmGmkNeedleNeonBuild();
        AppMain.GmEfctBossCmnBuildDataInit();
        AppMain.GmEfctZoneBuildDataInit(4);
    }

    private static void gmGameDatBuildStageFinalBoss03()
    {
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkSteamPipeBuild();
        AppMain.GmGmkBumperBuild();
        AppMain.GmGmkFlipperBuild();
        AppMain.GmGmkShutterBuild();
        AppMain.GmGmkBoss3PillarBuild();
        AppMain.GmGmkNeedleNeonBuild();
        AppMain.GmEfctBossCmnBuildDataInit();
        AppMain.GmEfctZoneBuildDataInit(4);
    }

    private static void gmGameDatBuildStageFinalBoss04()
    {
        AppMain.GmGmkLandBuild();
        AppMain.GmEfctZoneBuildDataInit(4);
    }

    private static void gmGameDatBuildStageFinalBoss05()
    {
        AppMain.GmBoss5Build();
        AppMain.GmGmkLandBuild();
        AppMain.GmGmkSteamPipeBuild();
        AppMain.GmGmkBumperBuild();
        AppMain.GmGmkFlipperBuild();
        AppMain.GmGmkShutterBuild();
        AppMain.GmGmkBoss3PillarBuild();
        AppMain.GmGmkNeedleNeonBuild();
        AppMain.GmEfctBossCmnBuildDataInit();
        AppMain.GmEfctZoneBuildDataInit(4);
    }

    private static void gmGameDatBuildSS01()
    {
        AppMain.GmGmkSsSquareBuild();
        AppMain.GmGmkSsCircleBuild();
        AppMain.GmGmkSsEnduranceBuild();
        AppMain.GmGmkSsGoalBuild();
        AppMain.GmGmkSsEmeraldBuild();
        AppMain.GmGmkSsTimeBuild();
        AppMain.GmGmkSsRingGateBuild();
        AppMain.GmGmkSsArrowBuild();
        AppMain.GmGmkSsOblongBuild();
        AppMain.GmGmkBobbinBuild();
        AppMain.GmEfctZoneBuildDataInit(5);
        AppMain.GmStartMsgBuild();
    }

    private static void gmGameDatBuildEnding()
    {
        AppMain.GmGmkLandBuild();
        AppMain.GmEfctZoneBuildDataInit(0);
        AppMain.GmEndingBuild();
        AppMain.DmStfrlMdlCtrlRingBuild();
        AppMain.DmStfrlMdlCtrlBoss1Build();
        AppMain.DmStfrlMdlCtrlSonicBuild();
    }

    private static void gmGameDatFlushStage11()
    {
        AppMain.GmEneMotoraFlush();
        AppMain.GmEneGabuFlush();
        AppMain.GmEneStingFlush();
        AppMain.GmEneMereonFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkBridgeFlush();
        AppMain.GmGmkBreakObjFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmGmkBreakLandFlush();
        AppMain.GmEfctEneFlushDataInit(0);
        AppMain.GmEfctZoneFlushDataInit(0);
    }

    private static void gmGameDatFlushStage12()
    {
        AppMain.GmEneMotoraFlush();
        AppMain.GmEneGabuFlush();
        AppMain.GmEneStingFlush();
        AppMain.GmEneMereonFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkTarzanRopeFlush();
        AppMain.GmGmkBridgeFlush();
        AppMain.GmGmkBreakObjFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmGmkBreakLandFlush();
        AppMain.GmEfctEneFlushDataInit(0);
        AppMain.GmEfctZoneFlushDataInit(0);
    }

    private static void gmGameDatFlushStage13()
    {
        AppMain.GmEneMotoraFlush();
        AppMain.GmEneGabuFlush();
        AppMain.GmEneStingFlush();
        AppMain.GmEneMereonFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkPulleyFlush();
        AppMain.GmGmkBridgeFlush();
        AppMain.GmGmkBreakObjFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmGmkBreakLandFlush();
        AppMain.GmEfctEneFlushDataInit(0);
        AppMain.GmEfctZoneFlushDataInit(0);
    }

    private static void gmGameDatFlushStage1Boss()
    {
        AppMain.GmBoss1Flush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkCapsuleFlush();
        AppMain.GmEfctZoneFlushDataInit(0);
        AppMain.GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStage21()
    {
        AppMain.GmEneGardonFlush();
        AppMain.GmEneHaroFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkBumperFlush();
        AppMain.GmGmkEnBmprFlush();
        AppMain.GmGmkBobbinFlush();
        AppMain.GmGmkFlipperFlush();
        AppMain.GmGmkStopperFlush();
        AppMain.GmGmkSlotFlush();
        AppMain.GmGmkSpCtpltFlush();
        AppMain.GmGmkBreakObjFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmEfctEneFlushDataInit(1);
        AppMain.GmEfctZoneFlushDataInit(1);
    }

    private static void gmGameDatFlushStage22()
    {
        AppMain.GmGmkBumperFlush();
        AppMain.GmGmkEnBmprFlush();
        AppMain.GmGmkBobbinFlush();
        AppMain.GmGmkFlipperFlush();
        AppMain.GmGmkStopperFlush();
        AppMain.GmGmkSlotFlush();
        AppMain.GmGmkSpCtpltFlush();
        AppMain.GmGmkSsArrowFlush();
        AppMain.GmEfctEneFlushDataInit(1);
        AppMain.GmEfctZoneFlushDataInit(1);
        AppMain.GmStartMsgFlush();
    }

    private static void gmGameDatFlushStage23()
    {
        AppMain.GmEneGardonFlush();
        AppMain.GmEneHaroFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkBumperFlush();
        AppMain.GmGmkEnBmprFlush();
        AppMain.GmGmkBobbinFlush();
        AppMain.GmGmkFlipperFlush();
        AppMain.GmGmkCannonFlush();
        AppMain.GmGmkSpCtpltFlush();
        AppMain.GmGmkBreakObjFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmGmkSsArrowFlush();
        AppMain.GmEfctEneFlushDataInit(1);
        AppMain.GmEfctZoneFlushDataInit(1);
    }

    private static void gmGameDatFlushStage2Boss()
    {
        AppMain.GmBoss2Flush();
        AppMain.GmGmkCapsuleFlush();
        AppMain.GmGmkBumperFlush();
        AppMain.GmGmkEnBmprFlush();
        AppMain.GmGmkFlipperFlush();
        AppMain.GmGmkShutterFlush();
        AppMain.GmGmkNeedleNeonFlush();
        AppMain.GmEfctZoneFlushDataInit(1);
        AppMain.GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStage31()
    {
        AppMain.GmEneMoguFlush();
        AppMain.GmEneUnidesFlush();
        AppMain.GmEneUniuniFlush();
        AppMain.GmEneBukuFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkRockFlush();
        AppMain.GmGmkSpearFlush();
        AppMain.GmGmkBreakObjFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmGmkRockRideFlush();
        AppMain.GmGmkSwitchFlush();
        AppMain.GmGmkSwWallFlush();
        AppMain.GmEfctEneFlushDataInit(2);
        AppMain.GmEfctZoneFlushDataInit(2);
    }

    private static void gmGameDatFlushStage32()
    {
        AppMain.GmEneMoguFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkSpearFlush();
        AppMain.GmGmkBreakObjFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmGmkTruckFlush();
        AppMain.GmGmkSwitchFlush();
        AppMain.GmGmkSwWallFlush();
        AppMain.GmGmkDSignFlush();
        AppMain.GmEfctEneFlushDataInit(2);
        AppMain.GmEfctZoneFlushDataInit(2);
        AppMain.GmStartMsgFlush();
    }

    private static void gmGameDatFlushStage33()
    {
        AppMain.GmEneMoguFlush();
        AppMain.GmEneUnidesFlush();
        AppMain.GmEneUniuniFlush();
        AppMain.GmEneBukuFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkWaterSliderFlush();
        AppMain.GmGmkSpearFlush();
        AppMain.GmGmkPressWallFlush();
        AppMain.GmGmkBreakObjFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmGmkDrainTankFlush();
        AppMain.GmGmkSwitchFlush();
        AppMain.GmGmkSwWallFlush();
        AppMain.GmEfctEneFlushDataInit(2);
        AppMain.GmEfctZoneFlushDataInit(2);
    }

    private static void gmGameDatFlushStage3Boss()
    {
        AppMain.GmBoss3Flush();
        AppMain.GmEneMoguFlush();
        AppMain.GmEneUnidesFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkCapsuleFlush();
        AppMain.GmGmkSpearFlush();
        AppMain.GmGmkBoss3PillarFlush();
        AppMain.GmEfctEneFlushDataInit(2);
        AppMain.GmEfctZoneFlushDataInit(2);
        AppMain.GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStage41()
    {
        AppMain.GmEneTStarFlush();
        AppMain.GmEneKaniFlush();
        AppMain.GmEneKamaFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkPistonFlush();
        AppMain.GmGmkBeltConveyorFlush();
        AppMain.GmGmkUpBumperFlush();
        AppMain.GmGmkSteamPipeFlush();
        AppMain.GmGmkPopSteamFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmGmkSwitchFlush();
        AppMain.GmGmkSwWallFlush();
        AppMain.GmEfctEneFlushDataInit(3);
        AppMain.GmEfctZoneFlushDataInit(3);
    }

    private static void gmGameDatFlushStage42()
    {
        AppMain.GmEneTStarFlush();
        AppMain.GmEneKaniFlush();
        AppMain.GmEneKamaFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkPistonFlush();
        AppMain.GmGmkBeltConveyorFlush();
        AppMain.GmGmkUpBumperFlush();
        AppMain.GmGmkSteamPipeFlush();
        AppMain.GmGmkPopSteamFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmGmkGearFlush();
        AppMain.GmGmkSwitchFlush();
        AppMain.GmGmkSwWallFlush();
        AppMain.GmEfctEneFlushDataInit(3);
        AppMain.GmEfctZoneFlushDataInit(3);
    }

    private static void gmGameDatFlushStage43()
    {
        AppMain.GmEneTStarFlush();
        AppMain.GmEneKaniFlush();
        AppMain.GmEneKamaFlush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkPistonFlush();
        AppMain.GmGmkBeltConveyorFlush();
        AppMain.GmGmkUpBumperFlush();
        AppMain.GmGmkSeesawFlush();
        AppMain.GmGmkSteamPipeFlush();
        AppMain.GmGmkPopSteamFlush();
        AppMain.GmGmkPressWallFlush();
        AppMain.GmGmkPressPillarFlush();
        AppMain.GmGmkBreakWallFlush();
        AppMain.GmGmkGearFlush();
        AppMain.GmGmkSwitchFlush();
        AppMain.GmGmkSwWallFlush();
        AppMain.GmEfctEneFlushDataInit(3);
        AppMain.GmEfctZoneFlushDataInit(3);
    }

    private static void gmGameDatFlushStage4Boss()
    {
        AppMain.GmBoss4Flush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkCapsuleFlush();
        AppMain.GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStageFinalBoss01()
    {
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkSteamPipeFlush();
        AppMain.GmGmkBumperFlush();
        AppMain.GmGmkFlipperFlush();
        AppMain.GmGmkShutterFlush();
        AppMain.GmGmkBoss3PillarFlush();
        AppMain.GmGmkNeedleNeonFlush();
        AppMain.GmEfctZoneFlushDataInit(4);
        AppMain.GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStageFinalBoss02()
    {
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkSteamPipeFlush();
        AppMain.GmGmkBumperFlush();
        AppMain.GmGmkFlipperFlush();
        AppMain.GmGmkShutterFlush();
        AppMain.GmGmkBoss3PillarFlush();
        AppMain.GmGmkNeedleNeonFlush();
        AppMain.GmEfctZoneFlushDataInit(4);
        AppMain.GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStageFinalBoss03()
    {
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkSteamPipeFlush();
        AppMain.GmGmkBumperFlush();
        AppMain.GmGmkFlipperFlush();
        AppMain.GmGmkShutterFlush();
        AppMain.GmGmkBoss3PillarFlush();
        AppMain.GmGmkNeedleNeonFlush();
        AppMain.GmEfctZoneFlushDataInit(4);
        AppMain.GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushStageFinalBoss04()
    {
        AppMain.GmGmkLandFlush();
        AppMain.GmEfctZoneFlushDataInit(4);
    }

    private static void gmGameDatFlushStageFinalBoss05()
    {
        AppMain.GmBoss5Flush();
        AppMain.GmGmkLandFlush();
        AppMain.GmGmkSteamPipeFlush();
        AppMain.GmGmkBumperFlush();
        AppMain.GmGmkFlipperFlush();
        AppMain.GmGmkShutterFlush();
        AppMain.GmGmkBoss3PillarFlush();
        AppMain.GmGmkNeedleNeonFlush();
        AppMain.GmEfctZoneFlushDataInit(4);
        AppMain.GmEfctBossCmnFlushDataInit();
    }

    private static void gmGameDatFlushSS01()
    {
        AppMain.GmGmkSsSquareFlush();
        AppMain.GmGmkSsCircleFlush();
        AppMain.GmGmkSsEnduranceFlush();
        AppMain.GmGmkSsGoalFlush();
        AppMain.GmGmkSsEmeraldFlush();
        AppMain.GmGmkSsTimeFlush();
        AppMain.GmGmkSsRingGateFlush();
        AppMain.GmGmkSsArrowFlush();
        AppMain.GmGmkSsOblongFlush();
        AppMain.GmGmkBobbinFlush();
        AppMain.GmEfctZoneFlushDataInit(5);
        AppMain.GmStartMsgFlush();
    }

    private static void gmGameDatFlushEnding()
    {
        AppMain.GmGmkLandFlush();
        AppMain.GmEfctZoneFlushDataInit(0);
        AppMain.GmEndingFlush();
        AppMain.DmStfrlMdlCtrlSonicFlush();
        AppMain.DmStfrlMdlCtrlRingFlush();
        AppMain.DmStfrlMdlCtrlBoss1Flush();
    }

    private static void gmGameDatBuildStageF_BB1()
    {
        AppMain.GmBoss1Build();
    }

    private static void gmGameDatBuildStageF_BB2()
    {
        AppMain.GmBoss2Build();
    }

    private static void gmGameDatBuildStageF_BB3()
    {
        AppMain.GmBoss3Build();
    }

    private static void gmGameDatBuildStageF_BB4()
    {
        AppMain.GmBoss4Build();
    }

    private static void gmGameDatBuildStageF_BBF()
    {
        AppMain.GmBoss5Build();
    }

    private static void gmGameDatFlushStageF_BB1()
    {
        AppMain.GmBoss1Flush();
    }

    private static void gmGameDatFlushStageF_BB2()
    {
        AppMain.GmBoss2Flush();
    }

    private static void gmGameDatFlushStageF_BB3()
    {
        AppMain.GmBoss3Flush();
    }

    private static void gmGameDatFlushStageF_BB4()
    {
        AppMain.GmBoss4Flush();
    }

    private static void gmGameDatFlushStageF_BBF()
    {
        AppMain.GmBoss5Flush();
    }

}