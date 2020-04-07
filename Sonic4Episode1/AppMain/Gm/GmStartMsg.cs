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
    public static void GmStartMsgExit()
    {
        if (AppMain.gm_start_msg_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_start_msg_tcb);
        AppMain.gm_start_msg_tcb = (AppMain.MTS_TASK_TCB)null;
        AppMain.ObjDrawSetNNCommandStateTbl(16U, uint.MaxValue, false);
        AppMain.ObjDrawSetNNCommandStateTbl(17U, uint.MaxValue, false);
        AppMain.g_obj.ppPost = (AppMain.OBJECT_Delegate)null;
        AppMain.g_gm_main_system.game_flag &= 4278190079U;
    }

    public static void GmStartMsgFlush()
    {
        int num = 2;
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)5)
            num = 3;
        AppMain.AOS_TEXTURE[] gmStartMsgAosTex = AppMain.gm_start_msg_aos_tex;
        for (int index = 0; index < num; ++index)
            AppMain.AoTexRelease(gmStartMsgAosTex[index]);
    }

    public static bool GmStartMsgFlushCheck()
    {
        bool flag = true;
        int num = 2;
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)5)
            num = 3;
        if (AppMain.gm_start_msg_aos_tex != null)
        {
            AppMain.AOS_TEXTURE[] gmStartMsgAosTex = AppMain.gm_start_msg_aos_tex;
            for (int index = 0; index < num; ++index)
            {
                if (!AppMain.AoTexIsReleased(gmStartMsgAosTex[index]))
                    flag = false;
            }
            if (flag)
                AppMain.gm_start_msg_aos_tex = (AppMain.AOS_TEXTURE[])null;
        }
        return flag;
    }

    public static bool GmStartMsgBuildCheck()
    {
        bool flag = true;
        int num = 2;
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)5)
            num = 3;
        if (AppMain.gm_start_msg_aos_tex != null)
        {
            for (int index = 0; index < num; ++index)
            {
                if (!AppMain.AoTexIsLoaded(AppMain.gm_start_msg_aos_tex[index]))
                    flag = false;
            }
        }
        return flag;
    }

    public static void GmStartMsgBuild()
    {
        int language = AppMain.GsEnvGetLanguage();
        object[] objArray = new object[3];
        AppMain.gm_start_msg_aos_tex = AppMain.New<AppMain.AOS_TEXTURE>(3);
        AppMain.AMS_AMB_HEADER gimmickData1 = AppMain.GmGameDatGetGimmickData(991);
        AppMain.AMS_AMB_HEADER gimmickData2 = AppMain.GmGameDatGetGimmickData(992);
        objArray[0] = AppMain.amBindGet(gimmickData1, language * 2 + 1);
        objArray[1] = AppMain.amBindGet(gimmickData2, 1);
        int num = 2;
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)5)
        {
            objArray[2] = AppMain.amBindGet(gimmickData1, gimmickData1.file_num - 1);
            num = 3;
        }
        for (int index = 0; index < num; ++index)
        {
            AppMain.AOS_TEXTURE tex = AppMain.gm_start_msg_aos_tex[index];
            AppMain.AoTexBuild(tex, (AppMain.AMS_AMB_HEADER)objArray[index]);
            AppMain.AoTexLoad(tex);
        }
    }

    public static void gmStartMsgObjPost()
    {
        if (AppMain.gm_start_msg_tcb == null)
            return;
        AppMain.GMS_SMSG_MGR_WORK work = (AppMain.GMS_SMSG_MGR_WORK)AppMain.gm_start_msg_tcb.work;
        AppMain.ObjDraw3DNNUserFunc(new AppMain.OBF_DRAW_USER_DT_FUNC(AppMain.gmStartMsgDrawWindowPre_DT), (object)null, 0, 14U);
        AppMain.AoActSysSetDrawState(14U);
        for (int index = 0; index < (int)AppMain.gm_start_msg_ama_act_num_tbl[work.msg_type]; ++index)
        {
            if (work.ama_2d_work[index] != null)
                AppMain.ObjDrawActionSummary(work.ama_2d_work[index].obj_work);
        }
        for (int index = 0; index < AppMain.GMD_SMSG_AMA_ACT_ACTION_MAX; ++index)
        {
            if (work.ama_2d_work_act[index] != null)
                AppMain.ObjDrawActionSummary(work.ama_2d_work_act[index].obj_work);
        }
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
        AppMain.AoActSysSetDrawState(6U);
    }

    public static void gmStartMsgDrawWindowPre_DT(object param)
    {
        AppMain.AoActDrawPre();
    }

    public static void gmStartMsgObjMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.gm_start_msg_end_state)
            return;
        obj_work.flag |= 4U;
        obj_work.disp_flag |= 32U;
    }

    public static void gmStartMsgMain(AppMain.MTS_TASK_TCB tcb)
    {
        int language = AppMain.GsEnvGetLanguage();
        AppMain.GMS_SMSG_MGR_WORK work = (AppMain.GMS_SMSG_MGR_WORK)tcb.work;
        if (work.func != null)
            work.func(work);
        if (((int)work.flag & (int)AppMain.GMD_SMSG_FLAG_END) != 0)
        {
            AppMain.GmStartMsgExit();
            AppMain.GmPlySeqChangeFw(AppMain.g_gm_main_system.ply_work[0]);
            AppMain.g_gm_main_system.ply_work[0].no_key_timer = 32768;
            AppMain.ObjObjectPauseOut();
            AppMain.g_gm_main_system.game_flag |= 3072U;
        }
        else
        {
            if (((int)work.flag & (int)AppMain.GMD_SMSG_FLAG_WIN_DISP) == 0)
                return;
            AppMain.ObjDraw3DNNUserFunc(new AppMain.OBF_DRAW_USER_DT_FUNC(AppMain.gmStartMsgDrawWindowPre_DT), (object)null, 0, 13U);
            AppMain.AoWinSysDrawState(0, AppMain.AoTexGetTexList(AppMain.gm_start_msg_aos_tex[1]), 0U, AppMain.gm_start_msg_win_size_tbl[work.msg_type][language][0], AppMain.gm_start_msg_win_size_tbl[work.msg_type][language][1], (AppMain.gm_start_msg_win_size_tbl[work.msg_type][language][2] - 32f) * work.win_per, (float)(((double)AppMain.gm_start_msg_win_size_tbl[work.msg_type][language][3] - 32.0) * (double)work.win_per * 0.899999976158142 - 16.0), 13U);
        }
    }

    public static void GmStartMsgInit()
    {
        int language = AppMain.GsEnvGetLanguage();
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        AppMain.g_gm_main_system.game_flag |= 16777216U;
        AppMain.gm_start_msg_end_state = false;
        AppMain.gm_start_msg_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmStartMsgMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.gmStartMsgDest), 0U, (ushort)3, 18502U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_SMSG_MGR_WORK()), "GM_S_MSG_MGR");
        AppMain.GMS_SMSG_MGR_WORK work = (AppMain.GMS_SMSG_MGR_WORK)AppMain.gm_start_msg_tcb.work;
        work.Clear();
        AppMain.ObjDrawSetNNCommandStateTbl(16U, 13U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(17U, 14U, true);
        AppMain.g_obj.ppPost = new AppMain.OBJECT_Delegate(AppMain.gmStartMsgObjPost);
        switch (AppMain.g_gs_main_sys_info.stage_id)
        {
            case 9:
                work.msg_type = 1;
                break;
            case 21:
            case 22:
            case 23:
            case 24:
            case 25:
            case 26:
            case 27:
                work.msg_type = 2;
                break;
            default:
                work.msg_type = 0;
                break;
        }
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(991);
        AppMain.NNS_TEXLIST texList1 = AppMain.AoTexGetTexList(AppMain.gm_start_msg_aos_tex[0]);
        for (int index = 0; index < (int)AppMain.gm_start_msg_ama_act_num_tbl[work.msg_type]; ++index)
        {
            work.ama_2d_work[index] = (AppMain.GMS_SMSG_2D_OBJ_WORK)AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)18512, (byte)5, (byte)0, (byte)3, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_SMSG_2D_OBJ_WORK()), "GM_SMSG");
            uint id = (uint)index;
            if ((512 & (int)mainSysInfo.game_flag) != 0)
            {
                switch (work.msg_type)
                {
                    case 1:
                        if (id == 0U)
                        {
                            id = 2U;
                            break;
                        }
                        break;
                    case 2:
                        if (id == 0U)
                        {
                            id = 2U;
                            break;
                        }
                        break;
                }
            }
            AppMain.ObjObjectAction2dAMALoadSetTexlist(work.ama_2d_work[index].obj_work, work.ama_2d_work[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, language * 2, gimmickData, texList1, id, 0);
            work.ama_2d_work[index].obj_work.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
            work.ama_2d_work[index].obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmStartMsgObjMain);
            work.ama_2d_work[index].obj_work.disp_flag |= 32U;
            work.ama_2d_work[index].obj_work.flag |= 18U;
            work.ama_2d_work[index].obj_work.move_flag |= 8448U;
            work.ama_2d_work[index].obj_work.disp_flag |= 1048960U;
            work.ama_2d_work[index].obj_work.pos.x = AppMain.gm_start_msg_ama_act_pos_tbl[work.msg_type][language][index][0];
            work.ama_2d_work[index].obj_work.pos.y = AppMain.gm_start_msg_ama_act_pos_tbl[work.msg_type][language][index][1];
            if (index == 0)
            {
                work.ama_2d_work[index].obj_work.scale.x = AppMain.GMD_SMSG_ACT_SCALE;
                work.ama_2d_work[index].obj_work.scale.y = AppMain.GMD_SMSG_ACT_SCALE;
            }
        }
        for (int index = 0; index < AppMain.GMD_SMSG_AMA_ACT_ACTION_MAX; ++index)
        {
            int num = AppMain.gm_start_msg_body_act_id_table[work.msg_type][index];
            if ((512 & (int)mainSysInfo.game_flag) == 0)
            {
                switch (num)
                {
                    case -1:
                    case 4:
                    case 5:
                        break;
                    case 7:
                        num = 0;
                        break;
                    default:
                        num = -1;
                        break;
                }
            }
            if (num < 0)
            {
                work.ama_2d_work_act[index] = (AppMain.GMS_SMSG_2D_OBJ_WORK)null;
            }
            else
            {
                work.ama_2d_work_act[index] = (AppMain.GMS_SMSG_2D_OBJ_WORK)AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)18512, (byte)5, (byte)0, (byte)3, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_SMSG_2D_OBJ_WORK()), "GM_SMSG");
                AppMain.NNS_TEXLIST texList2 = AppMain.AoTexGetTexList(AppMain.gm_start_msg_aos_tex[2]);
                AppMain.ObjObjectAction2dAMALoadSetTexlist(work.ama_2d_work_act[index].obj_work, work.ama_2d_work_act[index].obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, gimmickData.file_num - 2, gimmickData, texList2, (uint)num, 0);
                work.ama_2d_work_act[index].obj_work.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
                work.ama_2d_work_act[index].obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmStartMsgObjMain);
                work.ama_2d_work_act[index].obj_work.disp_flag |= 32U;
                work.ama_2d_work_act[index].obj_work.flag |= 18U;
                work.ama_2d_work_act[index].obj_work.move_flag |= 8448U;
                work.ama_2d_work_act[index].obj_work.disp_flag |= 1048960U;
            }
        }
        work.func = new AppMain.pfnGMS_SMSG_MGR_WORK(AppMain.gmStartMsgMain_StartWait);
        AppMain.gmStartMsgMain_StartWait(work);
    }

    public static void gmStartMsgDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.UNREFERENCED_PARAMETER((object)tcb);
        AppMain.gm_start_msg_end_state = true;
    }

    public static void gmStartMsgMain_StartWait(AppMain.GMS_SMSG_MGR_WORK mgr_work)
    {
        if (((int)AppMain.g_gm_main_system.game_flag & 4096) != 0)
            return;
        mgr_work.flag |= AppMain.GMD_SMSG_FLAG_WIN_DISP;
        AppMain.ObjObjectPause((ushort)2);
        AppMain.g_gm_main_system.game_flag &= 4294964223U;
        mgr_work.win_per = 0.0f;
        AppMain.GmSoundPlaySE("Window");
        mgr_work.timer = 0;
        mgr_work.func = new AppMain.pfnGMS_SMSG_MGR_WORK(AppMain.gmStartMsgMain_WindowOpen);
    }

    public static void gmStartMsgMain_WindowOpen(AppMain.GMS_SMSG_MGR_WORK mgr_work)
    {
        ++mgr_work.timer;
        if (mgr_work.timer >= 8)
        {
            mgr_work.win_per = 1f;
            for (int index = 0; index < (int)AppMain.gm_start_msg_ama_act_num_tbl[mgr_work.msg_type]; ++index)
                mgr_work.ama_2d_work[index].obj_work.disp_flag &= 4294967263U;
            for (int index = 0; index < AppMain.GMD_SMSG_AMA_ACT_ACTION_MAX; ++index)
            {
                if (mgr_work.ama_2d_work_act[index] != null)
                    mgr_work.ama_2d_work_act[index].obj_work.disp_flag &= 4294967263U;
            }
            mgr_work.timer = AppMain.GMD_SMSG_KEY_WAIT;
            mgr_work.func = new AppMain.pfnGMS_SMSG_MGR_WORK(AppMain.gmStartMsgMain_KeyWait);
        }
        else
            mgr_work.win_per = (float)mgr_work.timer / 8f;
    }

    public static void gmStartMsgMain_KeyWait(AppMain.GMS_SMSG_MGR_WORK mgr_work)
    {
        if (mgr_work.timer != 0)
        {
            --mgr_work.timer;
        }
        else
        {
            if (!AppMain.amTpIsTouchOn(0))
                return;
            for (int index = 0; index < (int)AppMain.gm_start_msg_ama_act_num_tbl[mgr_work.msg_type]; ++index)
                mgr_work.ama_2d_work[index].obj_work.disp_flag |= 32U;
            for (int index = 0; index < AppMain.GMD_SMSG_AMA_ACT_ACTION_MAX; ++index)
            {
                if (mgr_work.ama_2d_work_act[index] != null)
                    mgr_work.ama_2d_work_act[index].obj_work.disp_flag |= 32U;
            }
            mgr_work.timer = 8;
            mgr_work.func = new AppMain.pfnGMS_SMSG_MGR_WORK(AppMain.gmStartMsgMain_WindowClose);
        }
    }

    public static void gmStartMsgMain_WindowClose(AppMain.GMS_SMSG_MGR_WORK mgr_work)
    {
        --mgr_work.timer;
        if (mgr_work.timer <= 0)
        {
            mgr_work.win_per = 0.0f;
            mgr_work.func = (AppMain.pfnGMS_SMSG_MGR_WORK)null;
            mgr_work.flag |= AppMain.GMD_SMSG_FLAG_END;
        }
        else
            mgr_work.win_per = (float)mgr_work.timer / 8f;
    }

}