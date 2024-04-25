public partial class AppMain
{
    public static void GmStartMsgExit()
    {
        if (gm_start_msg_tcb == null)
            return;
        mtTaskClearTcb(gm_start_msg_tcb);
        gm_start_msg_tcb = null;
        ObjDrawSetNNCommandStateTbl(16U, uint.MaxValue, false);
        ObjDrawSetNNCommandStateTbl(17U, uint.MaxValue, false);
        g_obj.ppPost = null;
        g_gm_main_system.game_flag &= 4278190079U;
    }

    public static void GmStartMsgFlush()
    {
        int num = 2;
        if (g_gs_main_sys_info.stage_id != 5)
            num = 3;
        AOS_TEXTURE[] gmStartMsgAosTex = gm_start_msg_aos_tex;
        for (int index = 0; index < num; ++index)
            AoTexRelease(gmStartMsgAosTex[index]);
    }

    public static bool GmStartMsgFlushCheck()
    {
        bool flag = true;
        int num = 2;
        if (g_gs_main_sys_info.stage_id != 5)
            num = 3;
        if (gm_start_msg_aos_tex != null)
        {
            AOS_TEXTURE[] gmStartMsgAosTex = gm_start_msg_aos_tex;
            for (int index = 0; index < num; ++index)
            {
                if (!AoTexIsReleased(gmStartMsgAosTex[index]))
                    flag = false;
            }
            if (flag)
                gm_start_msg_aos_tex = null;
        }
        return flag;
    }

    public static bool GmStartMsgBuildCheck()
    {
        bool flag = true;
        int num = 2;
        if (g_gs_main_sys_info.stage_id != 5)
            num = 3;
        if (gm_start_msg_aos_tex != null)
        {
            for (int index = 0; index < num; ++index)
            {
                if (!AoTexIsLoaded(gm_start_msg_aos_tex[index]))
                    flag = false;
            }
        }
        return flag;
    }

    public static void GmStartMsgBuild()
    {
        int language = GsEnvGetLanguage();
        object[] objArray = new object[3];
        gm_start_msg_aos_tex = New<AOS_TEXTURE>(3);
        AMS_AMB_HEADER gimmickData1 = GmGameDatGetGimmickData(991);
        AMS_AMB_HEADER gimmickData2 = GmGameDatGetGimmickData(992);
        objArray[0] = amBindGet(gimmickData1, language * 2 + 1);
        objArray[1] = amBindGet(gimmickData2, 1);
        int num = 2;
        if (g_gs_main_sys_info.stage_id != 5)
        {
            objArray[2] = amBindGet(gimmickData1, gimmickData1.file_num - 1);
            num = 3;
        }
        for (int index = 0; index < num; ++index)
        {
            AOS_TEXTURE tex = gm_start_msg_aos_tex[index];
            AoTexBuild(tex, (AMS_AMB_HEADER)objArray[index]);
            AoTexLoad(tex);
        }
    }

    public static void gmStartMsgObjPost()
    {
        if (gm_start_msg_tcb == null)
            return;
        GMS_SMSG_MGR_WORK work = (GMS_SMSG_MGR_WORK)gm_start_msg_tcb.work;
        ObjDraw3DNNUserFunc(new OBF_DRAW_USER_DT_FUNC(gmStartMsgDrawWindowPre_DT), null, 0, 14U);
        AoActSysSetDrawState(14U);
        for (int index = 0; index < gm_start_msg_ama_act_num_tbl[work.msg_type]; ++index)
        {
            if (work.ama_2d_work[index] != null)
                ObjDrawActionSummary(work.ama_2d_work[index].obj_work);
        }
        for (int index = 0; index < GMD_SMSG_AMA_ACT_ACTION_MAX; ++index)
        {
            if (work.ama_2d_work_act[index] != null)
                ObjDrawActionSummary(work.ama_2d_work_act[index].obj_work);
        }
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
        AoActSysSetDrawState(6U);
    }

    public static void gmStartMsgDrawWindowPre_DT(object param)
    {
        AoActDrawPre();
    }

    public static void gmStartMsgObjMain(OBS_OBJECT_WORK obj_work)
    {
        if (!gm_start_msg_end_state)
            return;
        obj_work.flag |= 4U;
        obj_work.disp_flag |= 32U;
    }

    public static void gmStartMsgMain(MTS_TASK_TCB tcb)
    {
        int language = GsEnvGetLanguage();
        GMS_SMSG_MGR_WORK work = (GMS_SMSG_MGR_WORK)tcb.work;
        if (work.func != null)
            work.func(work);
        if (((int)work.flag & (int)GMD_SMSG_FLAG_END) != 0)
        {
            GmStartMsgExit();
            GmPlySeqChangeFw(g_gm_main_system.ply_work[0]);
            g_gm_main_system.ply_work[0].no_key_timer = 32768;
            ObjObjectPauseOut();
            g_gm_main_system.game_flag |= 3072U;
        }
        else
        {
            if (((int)work.flag & (int)GMD_SMSG_FLAG_WIN_DISP) == 0)
                return;
            ObjDraw3DNNUserFunc(new OBF_DRAW_USER_DT_FUNC(gmStartMsgDrawWindowPre_DT), null, 0, 13U);
            AoWinSysDrawState(0, AoTexGetTexList(gm_start_msg_aos_tex[1]), 0U, gm_start_msg_win_size_tbl[work.msg_type][language][0], gm_start_msg_win_size_tbl[work.msg_type][language][1], (gm_start_msg_win_size_tbl[work.msg_type][language][2] - 32f) * work.win_per, (float)((gm_start_msg_win_size_tbl[work.msg_type][language][3] - 32.0) * work.win_per * 0.899999976158142 - 16.0), 13U);
        }
    }

    public static void GmStartMsgInit()
    {
        int language = GsEnvGetLanguage();
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        g_gm_main_system.game_flag |= 16777216U;
        gm_start_msg_end_state = false;
        gm_start_msg_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmStartMsgMain), new GSF_TASK_PROCEDURE(gmStartMsgDest), 0U, 3, 18502U, 5, () => new GMS_SMSG_MGR_WORK(), "GM_S_MSG_MGR");
        GMS_SMSG_MGR_WORK work = (GMS_SMSG_MGR_WORK)gm_start_msg_tcb.work;
        work.Clear();
        ObjDrawSetNNCommandStateTbl(16U, 13U, true);
        ObjDrawSetNNCommandStateTbl(17U, 14U, true);
        g_obj.ppPost = new OBJECT_Delegate(gmStartMsgObjPost);
        switch (g_gs_main_sys_info.stage_id)
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
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(991);
        NNS_TEXLIST texList1 = AoTexGetTexList(gm_start_msg_aos_tex[0]);
        for (int index = 0; index < gm_start_msg_ama_act_num_tbl[work.msg_type]; ++index)
        {
            work.ama_2d_work[index] = (GMS_SMSG_2D_OBJ_WORK)OBM_OBJECT_TASK_DETAIL_INIT(18512, 5, 0, 3, () => new GMS_SMSG_2D_OBJ_WORK(), "GM_SMSG");
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
            ObjObjectAction2dAMALoadSetTexlist(work.ama_2d_work[index].obj_work, work.ama_2d_work[index].obj_2d, null, null, language * 2, gimmickData, texList1, id, 0);
            work.ama_2d_work[index].obj_work.ppOut = null;
            work.ama_2d_work[index].obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmStartMsgObjMain);
            work.ama_2d_work[index].obj_work.disp_flag |= 32U;
            work.ama_2d_work[index].obj_work.flag |= 18U;
            work.ama_2d_work[index].obj_work.move_flag |= 8448U;
            work.ama_2d_work[index].obj_work.disp_flag |= 1048960U;
            work.ama_2d_work[index].obj_work.pos.x = gm_start_msg_ama_act_pos_tbl[work.msg_type][language][index][0];
            work.ama_2d_work[index].obj_work.pos.y = gm_start_msg_ama_act_pos_tbl[work.msg_type][language][index][1];
            if (index == 0)
            {
                work.ama_2d_work[index].obj_work.scale.x = GMD_SMSG_ACT_SCALE;
                work.ama_2d_work[index].obj_work.scale.y = GMD_SMSG_ACT_SCALE;
            }
        }
        for (int index = 0; index < GMD_SMSG_AMA_ACT_ACTION_MAX; ++index)
        {
            int num = gm_start_msg_body_act_id_table[work.msg_type][index];
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
                work.ama_2d_work_act[index] = null;
            }
            else
            {
                work.ama_2d_work_act[index] = (GMS_SMSG_2D_OBJ_WORK)OBM_OBJECT_TASK_DETAIL_INIT(18512, 5, 0, 3, () => new GMS_SMSG_2D_OBJ_WORK(), "GM_SMSG");
                NNS_TEXLIST texList2 = AoTexGetTexList(gm_start_msg_aos_tex[2]);
                ObjObjectAction2dAMALoadSetTexlist(work.ama_2d_work_act[index].obj_work, work.ama_2d_work_act[index].obj_2d, null, null, gimmickData.file_num - 2, gimmickData, texList2, (uint)num, 0);
                work.ama_2d_work_act[index].obj_work.ppOut = null;
                work.ama_2d_work_act[index].obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmStartMsgObjMain);
                work.ama_2d_work_act[index].obj_work.disp_flag |= 32U;
                work.ama_2d_work_act[index].obj_work.flag |= 18U;
                work.ama_2d_work_act[index].obj_work.move_flag |= 8448U;
                work.ama_2d_work_act[index].obj_work.disp_flag |= 1048960U;
            }
        }
        work.func = new pfnGMS_SMSG_MGR_WORK(gmStartMsgMain_StartWait);
        gmStartMsgMain_StartWait(work);
    }

    public static void gmStartMsgDest(MTS_TASK_TCB tcb)
    {
        UNREFERENCED_PARAMETER(tcb);
        gm_start_msg_end_state = true;
    }

    public static void gmStartMsgMain_StartWait(GMS_SMSG_MGR_WORK mgr_work)
    {
        if (((int)g_gm_main_system.game_flag & 4096) != 0)
            return;
        mgr_work.flag |= GMD_SMSG_FLAG_WIN_DISP;
        ObjObjectPause(2);
        g_gm_main_system.game_flag &= 4294964223U;
        mgr_work.win_per = 0.0f;
        GmSoundPlaySE("Window");
        mgr_work.timer = 0;
        mgr_work.func = new pfnGMS_SMSG_MGR_WORK(gmStartMsgMain_WindowOpen);
    }

    public static void gmStartMsgMain_WindowOpen(GMS_SMSG_MGR_WORK mgr_work)
    {
        ++mgr_work.timer;
        if (mgr_work.timer >= 8)
        {
            mgr_work.win_per = 1f;
            for (int index = 0; index < gm_start_msg_ama_act_num_tbl[mgr_work.msg_type]; ++index)
                mgr_work.ama_2d_work[index].obj_work.disp_flag &= 4294967263U;
            for (int index = 0; index < GMD_SMSG_AMA_ACT_ACTION_MAX; ++index)
            {
                if (mgr_work.ama_2d_work_act[index] != null)
                    mgr_work.ama_2d_work_act[index].obj_work.disp_flag &= 4294967263U;
            }
            mgr_work.timer = GMD_SMSG_KEY_WAIT;
            mgr_work.func = new pfnGMS_SMSG_MGR_WORK(gmStartMsgMain_KeyWait);
        }
        else
            mgr_work.win_per = mgr_work.timer / 8f;
    }

    public static void gmStartMsgMain_KeyWait(GMS_SMSG_MGR_WORK mgr_work)
    {
        if (mgr_work.timer != 0)
        {
            --mgr_work.timer;
        }
        else
        {
            if (!amTpIsTouchOn(0) && AoPad.AoPadDirect() == (ControllerConsts)0)
                return;
            for (int index = 0; index < gm_start_msg_ama_act_num_tbl[mgr_work.msg_type]; ++index)
                mgr_work.ama_2d_work[index].obj_work.disp_flag |= 32U;
            for (int index = 0; index < GMD_SMSG_AMA_ACT_ACTION_MAX; ++index)
            {
                if (mgr_work.ama_2d_work_act[index] != null)
                    mgr_work.ama_2d_work_act[index].obj_work.disp_flag |= 32U;
            }
            mgr_work.timer = 8;
            mgr_work.func = new pfnGMS_SMSG_MGR_WORK(gmStartMsgMain_WindowClose);
        }
    }

    public static void gmStartMsgMain_WindowClose(GMS_SMSG_MGR_WORK mgr_work)
    {
        --mgr_work.timer;
        if (mgr_work.timer <= 0)
        {
            mgr_work.win_per = 0.0f;
            mgr_work.func = null;
            mgr_work.flag |= GMD_SMSG_FLAG_END;
        }
        else
            mgr_work.win_per = mgr_work.timer / 8f;
    }

}