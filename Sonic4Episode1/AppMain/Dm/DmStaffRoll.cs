public partial class AppMain
{
    private static readonly int[] dm_stfrl_lng_amb_id_tbl;
    private static readonly int[] dm_stfrl_cmn_msg_lng_amb_id_tbl;
    private static readonly sbyte[] dm_stfrl_list_font_id_tbl;
    private static readonly sbyte[] dm_stfrl_font_width_length_tbl;
    private static readonly float[] dm_stfrl_list_id_font_size_tbl;
    private static readonly uint[] dm_stfrl_list_id_font_height_tbl;
    private static readonly uint[][] dm_stfrl_list_id_font_color_tbl;
    private static readonly uint[] dm_stfrl_list_logo_width_tbl;
    private static readonly float[][] dm_stfrl_win_act_pos_tbl;
    private static readonly uint[] g_dm_act_id_tbl_staff;
    private static readonly DMS_STFRL_MGR dm_stfrl_mgr;
    private static DMS_STFRL_MGR dm_stfrl_mgr_p;
    private static readonly DMS_STFRL_FS_DATA_MGR dm_stfrl_fs_data_mgr;
    private static DMS_STFRL_FS_DATA_MGR dm_stfrl_fs_data_mgr_p;
    private static readonly DMS_STFRL_DATA_MGR dm_stfrl_data_mgr;
    private static DMS_STFRL_DATA_MGR dm_stfrl_data_mgr_p;
    private static AMS_AMB_HEADER dm_stfrl_font_amb;
    private static readonly AOS_TEXTURE dm_stfrl_font_tex;
    private static AMS_AMB_HEADER dm_stfrl_scr_amb;
    private static readonly AOS_TEXTURE dm_stfrl_scr_tex;
    private static A2S_AMA_HEADER dm_stfrl_end_cmn_ama;
    private static AMS_AMB_HEADER dm_stfrl_end_cmn_amb;
    private static readonly AOS_TEXTURE dm_stfrl_end_tex;
    private static A2S_AMA_HEADER dm_stfrl_end_lng_ama;
    private static AMS_AMB_HEADER dm_stfrl_end_lng_amb;
    private static readonly AOS_TEXTURE dm_stfrl_end_jp_tex;
    private static readonly A2S_AMA_HEADER[] dm_stfrl_cmn_ama;
    private static readonly AMS_AMB_HEADER[] dm_stfrl_cmn_amb;
    private static readonly AOS_TEXTURE[] dm_stfrl_cmn_tex;
    private static bool dm_stfrl_is_full_staffroll;
    private static bool dm_stfrl_is_pause_maingame;
    private static readonly NNS_PRIM3D_PCT_ARRAY dmStaffRollStageScrDraw_DrawArray;

    private static void DmStaffRollBuildForGame()
    {
        dm_stfrl_fs_data_mgr.Clear();
        dm_stfrl_fs_data_mgr_p = dm_stfrl_fs_data_mgr;
        int language = GsEnvGetLanguage();
        dm_stfrl_fs_data_mgr_p.arc_list_font_amb_fs = GmGameDatGetGimmickData(958);
        dm_stfrl_fs_data_mgr_p.arc_scr_amb_fs = GmGameDatGetGimmickData(960);
        dm_stfrl_fs_data_mgr_p.arc_end_amb_fs = GmGameDatGetGimmickData(972);
        dm_stfrl_fs_data_mgr_p.arc_end_jp_amb_fs = GmGameDatGetGimmickData(dm_stfrl_lng_amb_id_tbl[language]);
        dm_stfrl_fs_data_mgr_p.arc_cmn_amb_fs[0] = GmGameDatGetGimmickData(959);
        dm_stfrl_fs_data_mgr_p.arc_cmn_amb_fs[1] = GmGameDatGetGimmickData(dm_stfrl_cmn_msg_lng_amb_id_tbl[language]);
    }

    private static void DmStaffRollBuild(DMS_STFRL_DATA_MGR data_mgr)
    {
        int curEvtId = SyGetEvtInfo().cur_evt_id;
        dm_stfrl_data_mgr.Clear();
        dm_stfrl_data_mgr_p = dm_stfrl_data_mgr;
        dm_stfrl_data_mgr_p = data_mgr;
        dm_stfrl_font_amb = data_mgr.arc_font_amb;
        AoTexBuild(dm_stfrl_font_tex, dm_stfrl_font_amb);
        AoTexLoad(dm_stfrl_font_tex);
        dm_stfrl_scr_amb = data_mgr.arc_scr_amb;
        AoTexBuild(dm_stfrl_scr_tex, dm_stfrl_scr_amb);
        AoTexLoad(dm_stfrl_scr_tex);
        dm_stfrl_end_cmn_ama = readAMAFile(amBindGet(data_mgr.arc_end_amb, 0));
        string sPath;
        dm_stfrl_end_cmn_amb = readAMBFile(amBindGet(data_mgr.arc_end_amb, 1, out sPath));
        dm_stfrl_end_cmn_amb.dir = sPath;
        dm_stfrl_end_lng_ama = readAMAFile(amBindGet(data_mgr.arc_end_jp_amb, 0));
        dm_stfrl_end_lng_amb = readAMBFile(amBindGet(data_mgr.arc_end_jp_amb, 1, out sPath));
        dm_stfrl_end_lng_amb.dir = sPath;
        AoTexBuild(dm_stfrl_end_tex, dm_stfrl_end_cmn_amb);
        AoTexLoad(dm_stfrl_end_tex);
        AoTexBuild(dm_stfrl_end_jp_tex, dm_stfrl_end_lng_amb);
        AoTexLoad(dm_stfrl_end_jp_tex);
        for (int index = 0; index < 2; ++index)
        {
            dm_stfrl_cmn_ama[index] = readAMAFile(amBindGet(data_mgr.arc_cmn_amb[index], 0));
            dm_stfrl_cmn_amb[index] = readAMBFile(amBindGet(data_mgr.arc_cmn_amb[index], 1, out sPath));
            dm_stfrl_cmn_amb[index].dir = sPath;
            AoTexBuild(dm_stfrl_cmn_tex[index], dm_stfrl_cmn_amb[index]);
            AoTexLoad(dm_stfrl_cmn_tex[index]);
        }
    }

    private static bool DmStaffRollBuildCheck()
    {
        return dmStaffRollIsTexLoad() != 0;
    }

    private static void DmStaffRollFlush()
    {
        AoTexRelease(dm_stfrl_font_tex);
        if (!dm_stfrl_is_full_staffroll)
            return;
        AoTexRelease(dm_stfrl_scr_tex);
        AoTexRelease(dm_stfrl_end_tex);
        AoTexRelease(dm_stfrl_end_jp_tex);
        for (int index = 0; index < 2; ++index)
            AoTexRelease(dm_stfrl_cmn_tex[index]);
    }

    private static bool DmStaffRollFlushCheck()
    {
        return dmStaffRollIsTexRelease() != 0;
    }

    private static void DmStaffRollStart(object arg)
    {
        UNREFERENCED_PARAMETER(arg);
        dm_stfrl_is_full_staffroll = SyGetEvtInfo().old_evt_id == 9;
        if (dm_stfrl_mgr_p == null)
            dm_stfrl_mgr_p = dm_stfrl_mgr;
        switch (SyGetEvtInfo().cur_evt_id)
        {
            case 6:
            case 11:
                dm_stfrl_is_pause_maingame = true;
                break;
            default:
                dm_stfrl_is_pause_maingame = false;
                break;
        }
        dmStaffRollInit();
    }

    private static bool DmStaffRollIsExit()
    {
        return dm_stfrl_mgr_p == null || dm_stfrl_mgr_p.tcb == null;
    }

    private static void DmStaffRollExit()
    {
        if (dm_stfrl_mgr_p.tcb == null)
            return;
        mtTaskClearTcb(dm_stfrl_mgr_p.tcb);
        dm_stfrl_mgr_p.tcb = null;
    }

    private static void dmStaffRollInit()
    {
        dm_stfrl_mgr_p.tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(dmStaffRollProcMain), new GSF_TASK_PROCEDURE(dmStaffRollDest), 0U, (ushort)short.MaxValue, 12288U, 10, () => new DMS_STFRL_MAIN_WORK(), "STAFFROLL_MAIN");
        DMS_STFRL_MAIN_WORK work = (DMS_STFRL_MAIN_WORK)dm_stfrl_mgr_p.tcb.work;
        AoActSysSetDrawStateEnable(true);
        AoActSysSetDrawState(AoActSysGetDrawState());
        dmStaffRollSetInitData(work);
        work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcInit);
    }

    private static void dmStaffRollSetInitData(DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.disp_mode = 0U;
        main_work.disp_frm_time = 1f;
        main_work.question_act_alpha.r = byte.MaxValue;
        main_work.question_act_alpha.g = byte.MaxValue;
        main_work.question_act_alpha.b = byte.MaxValue;
        main_work.question_act_alpha.a = 0;
        if (((int)g_gs_main_sys_info.game_flag & 32) != 0)
            main_work.is_eme_comp = true;
        else
            main_work.is_eme_comp = false;
    }

    private static void dmStaffRollProcMain(MTS_TASK_TCB tcb)
    {
        DMS_STFRL_MAIN_WORK work = (DMS_STFRL_MAIN_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
        {
            DmStaffRollExit();
            if (!dm_stfrl_is_full_staffroll)
                return;
            dmStaffRollSetNextEvt(work);
        }
        else
        {
            if (((int)work.flag & int.MinValue) != 0 && !AoAccountIsCurrentEnable())
            {
                work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcFadeOut);
                work.flag &= int.MaxValue;
                work.flag |= 1073741824U;
                if (dm_stfrl_is_pause_maingame)
                    IzFadeInitEasyColor(0, (ushort)short.MaxValue, 61439, 18U, 1U, 1U, 80f, true);
                else
                    IzFadeInitEasy(1U, 1U, 64f);
                if (work.bgm_scb != null)
                    GsSoundStopBgm(work.bgm_scb, 79);
                work.flag &= 4294967291U;
                work.flag &= 4294967293U;
                work.proc_input = null;
                work.win_timer = 0.0f;
                work.win_mode = 0U;
            }
            if (work.proc_update != null)
                work.proc_update(work);
            if (work.proc_draw == null)
                return;
            work.proc_draw(work);
        }
    }

    private static void dmStaffRollDest(MTS_TASK_TCB tcb)
    {
        if (!dm_stfrl_is_full_staffroll)
            return;
        ObjDrawESEffectSystemExit();
        ObjCameraExit();
        GmMainExitForStaffroll();
    }

    private static void dmStaffRollSetNextEvt(DMS_STFRL_MAIN_WORK main_work)
    {
        if (dm_stfrl_is_full_staffroll)
        {
            short evt_case = 0;
            if (((int)main_work.flag & 1073741824) != 0)
                evt_case = 1;
            SyDecideEvtCase(evt_case);
        }
        else
        {
            short req_id = SyGetEvtInfo().old_evt_id;
            if (((int)main_work.flag & 1073741824) != 0)
                req_id = 1;
            SyDecideEvt(req_id);
        }
    }

    private static void dmStaffRollProcInit(DMS_STFRL_MAIN_WORK main_work)
    {
        if (dm_stfrl_is_full_staffroll)
        {
            main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcLoadData);
            DmStaffRollBuildForGame();
            main_work.arc_list_font_amb = readAMBFile(dm_stfrl_fs_data_mgr_p.arc_list_font_amb_fs);
            main_work.arc_scr_amb_fs = readAMBFile(dm_stfrl_fs_data_mgr_p.arc_scr_amb_fs);
            main_work.arc_end_amb_fs = readAMBFile(dm_stfrl_fs_data_mgr_p.arc_end_amb_fs);
            main_work.arc_end_jp_amb_fs = readAMBFile(dm_stfrl_fs_data_mgr_p.arc_end_jp_amb_fs);
            main_work.arc_cmn_amb_fs[0] = readAMBFile(dm_stfrl_fs_data_mgr_p.arc_cmn_amb_fs[0]);
            main_work.arc_cmn_amb_fs[1] = readAMBFile(dm_stfrl_fs_data_mgr_p.arc_cmn_amb_fs[1]);
            main_work.staff_list_fs = amFsRead("DEMO/STFRL/STAFF_LIST.YSD");
        }
        else
        {
            main_work.arc_list_font_amb_fs = amFsReadBackground("DEMO/STFRL/D_STFRL_FONT.AMB");
            main_work.staff_list_fs = amFsRead("DEMO/STFRL/STAFF_LIST.YSD");
            main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcLoadData);
        }
        GsMainSysSetSleepFlag(false);
    }

    private static void dmStaffRollProcLoadData(DMS_STFRL_MAIN_WORK main_work)
    {
        if (dmStaffRollIsDataLoad(main_work) == 0)
            return;
        if (dm_stfrl_is_full_staffroll)
        {
            dmStaffRollDataClearRequestFull(main_work);
            DmStaffRollBuild(main_work.arc_data);
        }
        else
        {
            dmStaffRollDataClearRequestEasy(main_work);
            dmStaffRollDataBuildEasy(main_work);
        }
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcDataBuild);
    }

    private static void dmStaffRollProcDataBuild(DMS_STFRL_MAIN_WORK main_work)
    {
        if (!DmStaffRollBuildCheck())
            return;
        if (dm_stfrl_is_full_staffroll)
        {
            dmStaffRollSetObjSystemData(main_work);
            main_work.ring_work[0] = DmStfrlMdlCtrlSetRingObj(0, 0U);
            main_work.ring_work[1] = DmStfrlMdlCtrlSetRingObj(20, 3U);
            main_work.ring_work[2] = DmStfrlMdlCtrlSetRingObj(40, 6U);
            main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcCreateAct);
        }
        else
        {
            main_work.flag |= 2147483648U;
            main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcFadeIn);
            main_work.proc_draw = new DMS_STFRL_MAIN_WORK._proc_draw_(dmStaffRollProcActDraw);
            IzFadeInitEasy(0U, 0U, 64f);
        }
        if (!AoYsdFileIsYsdFile(dm_stfrl_data_mgr_p.stf_list_ysd))
            MTM_ASSERT(0);
        main_work.disp_list_page_num = AoYsdFileGetPageNum(dm_stfrl_data_mgr_p.stf_list_ysd);
        main_work.bgm_scb = GsSoundAssignScb(0);
        main_work.bgm_scb.flag |= 2147483648U;
        main_work.se_handle = GsSoundAllocSeHandle();
        if (GsSoundIsRunning())
            return;
        GsSoundBegin(4096, 1U, 3);
        main_work.flag |= 2048U;
    }

    private static void dmStaffRollProcCreateAct(DMS_STFRL_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 11U; ++index)
        {
            A2S_AMA_HEADER ama;
            AOS_TEXTURE tex;
            if (index >= 8U)
            {
                ama = dm_stfrl_end_lng_ama;
                tex = dm_stfrl_end_jp_tex;
            }
            else
            {
                ama = dm_stfrl_end_cmn_ama;
                tex = dm_stfrl_end_tex;
            }
            AoActSetTexture(AoTexGetTexList(tex));
            main_work.act[(int)index] = AoActCreate(ama, g_dm_act_id_tbl_staff[(int)index]);
        }
        main_work.flag |= 2147483648U;
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcFadeIn);
        main_work.proc_draw = new DMS_STFRL_MAIN_WORK._proc_draw_(dmStaffRollProcActDraw);
        IzFadeInitEasy(0U, 0U, 64f);
    }

    private static void dmStaffRollProcFadeIn(DMS_STFRL_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcNowStaffRoll);
        dmStaffRollSetFadePageInfoEfctData(main_work);
        main_work.timer = 32f;
        if (dm_stfrl_is_full_staffroll)
            GsSoundPlayBgm(main_work.bgm_scb, "snd_sng_ending", 0);
        else
            GsSoundPlayBgm(main_work.bgm_scb, "snd_sng_z1a1", 0);
        main_work.proc_input = new DMS_STFRL_MAIN_WORK._proc_input_(dmStaffRollInputProcStaffRollMain);
    }

    private static void dmStaffRollProcDispWaitStaffList(DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = !dm_stfrl_is_full_staffroll ? new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcFadeOut) : new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcModeFadeOut);
            if (dm_stfrl_is_pause_maingame)
            {
                IzFadeInitEasyColor(0, (ushort)short.MaxValue, 61439, 18U, 0U, 1U, 80f, true);
                GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            else
            {
                IzFadeInitEasy(0U, 1U, 80f);
                GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            main_work.flag &= 4294967291U;
        }
        else
        {
            if (main_work.timer <= 0.0)
            {
                main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcNowStaffRoll);
                main_work.timer = 0.0f;
            }
            main_work.timer -= main_work.disp_frm_time;
        }
    }

    private static void dmStaffRollProcNowStaffRoll(DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = !dm_stfrl_is_full_staffroll ? new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcFadeOut) : new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcModeFadeOut);
            if (dm_stfrl_is_pause_maingame)
            {
                IzFadeInitEasyColor(0, (ushort)short.MaxValue, 61439, 18U, 0U, 1U, 80f, true);
                GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            else
            {
                IzFadeInitEasy(0U, 1U, 80f);
                GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            main_work.flag &= 4294967291U;
        }
        else if (main_work.fade_timer <= 0.0)
        {
            main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcSetChangeData);
            main_work.fade_timer = 0.0f;
        }
        else
        {
            dmStaffRollSetEfctChngAlphaListData(main_work);
            main_work.fade_timer -= main_work.disp_frm_time;
        }
    }

    private static void dmStaffRollProcSetChangeData(DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = !dm_stfrl_is_full_staffroll ? new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcFadeOut) : new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcModeFadeOut);
            if (dm_stfrl_is_pause_maingame)
            {
                IzFadeInitEasyColor(0, (ushort)short.MaxValue, 61439, 18U, 0U, 1U, 80f, true);
                GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            else
            {
                IzFadeInitEasy(0U, 1U, 80f);
                GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            main_work.flag &= 4294967291U;
        }
        else
        {
            ++main_work.cur_disp_list_page;
            if (main_work.cur_disp_list_page > main_work.disp_list_page_num - 1U)
            {
                main_work.cur_disp_list_page = main_work.disp_list_page_num - 1U;
                main_work.proc_update = !dm_stfrl_is_full_staffroll ? new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcFadeOut) : new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcModeFadeOut);
                if (dm_stfrl_is_pause_maingame)
                {
                    IzFadeInitEasyColor(0, (ushort)short.MaxValue, 61439, 18U, 0U, 1U, 80f, true);
                    GsSoundStopBgm(main_work.bgm_scb, 80);
                }
                else
                {
                    IzFadeInitEasy(0U, 1U, 80f);
                    GsSoundStopBgm(main_work.bgm_scb, 80);
                }
            }
            else
            {
                dmStaffRollSetFadePageInfoEfctData(main_work);
                main_work.timer = 32f;
                main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcDispWaitStaffList);
                UNREFERENCED_PARAMETER(main_work);
            }
        }
    }

    private static void dmStaffRollProcModeFadeOut(DMS_STFRL_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcModeFadeIn);
        main_work.disp_mode = 1U;
        if (dm_stfrl_is_pause_maingame)
            IzFadeInitEasyColor(0, (ushort)short.MaxValue, 61439, 18U, 0U, 0U, 80f, true);
        else
            IzFadeInitEasy(0U, 0U, 80f);
        dmStaffRollSetupEndModel(main_work);
    }

    private static void dmStaffRollProcModeFadeIn(DMS_STFRL_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcEndModeIdle);
        main_work.proc_input = new DMS_STFRL_MAIN_WORK._proc_input_(dmStaffRollInputProcWin);
        main_work.timer = 32f;
        main_work.flag |= 128U;
    }

    private static void dmStaffRollProcEndModeIdle(DMS_STFRL_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 256) != 0 && main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (((int)main_work.flag & 4) != 0)
        {
            if (dm_stfrl_is_full_staffroll)
            {
                main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcWinModeFadeOut);
                main_work.proc_input = null;
            }
            else
            {
                main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcFadeOut);
                main_work.proc_input = null;
            }
            main_work.flag &= 4294967291U;
            if (dm_stfrl_is_pause_maingame)
                IzFadeInitEasyColor(0, (ushort)short.MaxValue, 61439, 18U, 0U, 1U, 80f, true);
            else
                IzFadeInitEasy(0U, 1U, 80f);
        }
        else
        {
            if (main_work.is_eme_comp)
            {
                if (((int)main_work.body_work.flag & 8) != 0)
                {
                    main_work.question_act_alpha.a += 8;
                    if (main_work.question_act_alpha.a > 247)
                    {
                        main_work.question_act_alpha.a = byte.MaxValue;
                        main_work.body_work.flag &= 4294967287U;
                    }
                }
                if (((int)main_work.body_work.flag & 16) != 0)
                    ++main_work.end_act_frm;
                if (main_work.end_act_frm > 240)
                    main_work.flag |= 4096U;
            }
            else
                main_work.end_act_frm = 0;
            if (main_work.end_act_frm == 140)
                GsSoundPlaySe("Metal_Sonic", main_work.se_handle);
            if (main_work.sonic_work != null && main_work.body_work != null && (main_work.sonic_work.flag & 1) != 0)
            {
                main_work.body_work.flag |= 2U;
                ushort num = 1;
                main_work.sonic_work.flag = (ushort)(main_work.sonic_work.flag & ~num);
            }
            if (((int)main_work.flag & 128) == 0)
                return;
            main_work.timer -= main_work.disp_frm_time;
            if (main_work.timer > 0.0)
                return;
            main_work.flag &= 4294967167U;
            main_work.flag |= 256U;
            main_work.timer = 0.0f;
        }
    }

    private static void dmStaffRollProcWinModeFadeOut(DMS_STFRL_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        dmStaffRollNodispEndModel(main_work);
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcWinModeFadeIn);
        main_work.disp_mode = 2U;
        IzFadeInitEasy(0U, 0U, 80f);
        GsSoundStopBgm(main_work.bgm_scb, 0);
    }

    private static void dmStaffRollProcWinModeFadeIn(DMS_STFRL_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcWindowNodispIdle);
        if (((int)g_gs_main_sys_info.game_flag & 32) != 0)
            return;
        main_work.announce_flag |= 1U;
    }

    private static void dmStaffRollProcWindowNodispIdle(DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.announce_flag != 0U)
        {
            main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcWindowOpenEfct);
            main_work.proc_input = null;
            main_work.win_timer = 0.0f;
            for (int index = 0; index < 1; ++index)
            {
                if (((int)main_work.announce_flag & 1 << index) != 0)
                {
                    main_work.win_mode = (uint)index;
                    break;
                }
            }
            main_work.flag |= 1024U;
            GsSoundPlaySe("Window", main_work.se_handle);
        }
        else
            main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcTrophyCheck);
    }

    private static void dmStaffRollProcWindowOpenEfct(DMS_STFRL_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 512) != 0)
        {
            main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcWindowAnnounceIdle);
            main_work.proc_input = new DMS_STFRL_MAIN_WORK._proc_input_(dmStaffRollInputProcWin);
            main_work.flag |= 64U;
            main_work.flag &= 4294966783U;
        }
        else
            dmStaffRollSetWinOpenEfct(main_work);
    }

    private static void dmStaffRollProcWindowAnnounceIdle(DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (main_work.win_mode != 0U || ((int)main_work.flag & 4) == 0)
            return;
        main_work.proc_input = null;
        main_work.win_timer = 8f;
        main_work.flag &= 4294967231U;
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcWindowCloseEfct);
        GsSoundPlaySe("Ok", main_work.se_handle);
        main_work.flag &= 4294967291U;
        main_work.flag &= 4294967293U;
    }

    private static void dmStaffRollProcWindowCloseEfct(DMS_STFRL_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 512) != 0)
        {
            main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcWindowNodispIdle);
            main_work.announce_flag &= (uint)~(1 << (int)main_work.win_mode);
            main_work.flag &= 4294966271U;
            main_work.flag &= 4294966783U;
        }
        dmStaffRollSetWinCloseEfct(main_work);
    }

    private static void dmStaffRollProcTrophyCheck(DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcFadeOut);
        if (dm_stfrl_is_full_staffroll)
            HgTrophyTryAcquisition(3);
        IzFadeInitEasy(0U, 1U, 80f);
    }

    private static void dmStaffRollProcFadeOut(DMS_STFRL_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        if (((int)main_work.flag & 2048) != 0)
        {
            GsSoundHalt();
            GsSoundEnd();
        }
        if (main_work.bgm_scb != null)
        {
            GsSoundStopBgm(main_work.bgm_scb, 0);
            GsSoundResignScb(main_work.bgm_scb);
            main_work.bgm_scb = null;
        }
        if (main_work.se_handle != null)
        {
            GsSoundFreeSeHandle(main_work.se_handle);
            main_work.se_handle = null;
        }
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcStopDraw);
        main_work.proc_draw = null;
    }

    private static void dmStaffRollProcStopDraw(DMS_STFRL_MAIN_WORK main_work)
    {
        for (int index = 0; index < 11; ++index)
        {
            if (main_work.act[index] != null)
            {
                AoActDelete(main_work.act[index]);
                main_work.act[index] = null;
            }
        }
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcDataRelease);
    }

    private static void dmStaffRollProcDataRelease(DMS_STFRL_MAIN_WORK main_work)
    {
        DmStaffRollFlush();
        if (dm_stfrl_is_full_staffroll)
        {
            ObjObjectClearAllObject();
            ObjPreExit();
        }
        main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcFinish);
    }

    private static void dmStaffRollProcFinish(DMS_STFRL_MAIN_WORK main_work)
    {
        if (DmStaffRollFlushCheck())
        {
            for (int index = 0; index < 11; ++index)
            {
                if (main_work.act[index] != null)
                {
                    AoActDelete(main_work.act[index]);
                    main_work.act[index] = null;
                }
            }
            if (dm_stfrl_is_full_staffroll)
            {
                if (dm_stfrl_data_mgr_p.arc_font_amb != null)
                    dm_stfrl_data_mgr_p.arc_font_amb = null;
                if (dm_stfrl_data_mgr_p.arc_scr_amb != null)
                    dm_stfrl_data_mgr_p.arc_scr_amb = null;
                if (dm_stfrl_data_mgr_p.arc_end_amb != null)
                    dm_stfrl_data_mgr_p.arc_end_amb = null;
                if (dm_stfrl_data_mgr_p.arc_end_jp_amb != null)
                    dm_stfrl_data_mgr_p.arc_end_jp_amb = null;
                for (int index = 0; index < 2; ++index)
                {
                    if (dm_stfrl_data_mgr_p.arc_cmn_amb[index] != null)
                        dm_stfrl_data_mgr_p.arc_cmn_amb[index] = null;
                }
            }
            else if (dm_stfrl_font_amb != null)
                dm_stfrl_font_amb = null;
            if (dm_stfrl_data_mgr_p.stf_list_ysd != null)
                dm_stfrl_data_mgr_p.stf_list_ysd = null;
            if (main_work.page_line_type != null)
                main_work.page_line_type = null;
            if (dm_stfrl_is_full_staffroll)
            {
                main_work.proc_update = new DMS_STFRL_MAIN_WORK._proc_update_(dmStaffRollProcSaveEndCheck);
                DmSaveMenuStart(true, false);
            }
            else
            {
                main_work.flag |= 1U;
                main_work.proc_update = null;
            }
        }
        GsMainSysSetSleepFlag(true);
    }

    private static void dmStaffRollProcSaveEndCheck(DMS_STFRL_MAIN_WORK main_work)
    {
        if (!DmSaveIsExit())
            return;
        main_work.flag |= 1U;
        main_work.proc_update = null;
        if (((int)main_work.flag & 2048) != 0)
            GsSoundReset();
        GsFontRelease();
    }

    private static void dmStaffRollInputProcStaffRollMain(DMS_STFRL_MAIN_WORK main_work)
    {
        if (!amTpIsTouchPush(0) && !isBackKeyPressed())
            return;
        main_work.flag |= 4U;
        setBackKeyRequest(false);
    }

    private static void dmStaffRollInputProcWin(DMS_STFRL_MAIN_WORK main_work)
    {
        if (!amTpIsTouchPush(0) && !isBackKeyPressed())
            return;
        main_work.flag |= 4U;
        setBackKeyRequest(false);
    }

    private static void dmStaffRollProcActDraw(DMS_STFRL_MAIN_WORK main_work)
    {
        dmStaffRollBGDraw(main_work);
        if (main_work.disp_mode == 0U)
        {
            dmStaffRollStaffListDraw(main_work);
            if (dm_stfrl_is_full_staffroll)
                dmStaffRollStageScrDraw(main_work);
        }
        else if (main_work.disp_mode == 1U && dm_stfrl_is_full_staffroll)
            dmStaffRollEndActDraw(main_work);
        if (((int)main_work.flag & 1024) == 0 || !dm_stfrl_is_full_staffroll)
            return;
        dmStaffRollWinActDraw(main_work);
    }

    private static void dmStaffRollBGDraw(DMS_STFRL_MAIN_WORK main_work)
    {
        UNREFERENCED_PARAMETER(main_work);
        AMS_PARAM_DRAW_PRIMITIVE setParam = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.mtx = null;
        setParam.vtxPC3D = amDrawAlloc_NNS_PRIM3D_PC(4);
        NNS_PRIM3D_PC[] vtxPc3D = setParam.vtxPC3D;
        vtxPc3D[0].Pos.x = vtxPc3D[1].Pos.x = -160f;
        vtxPc3D[2].Pos.x = vtxPc3D[3].Pos.x = vtxPc3D[0].Pos.x + 1280f;
        vtxPc3D[0].Pos.y = vtxPc3D[2].Pos.y = 0.0f;
        vtxPc3D[1].Pos.y = vtxPc3D[3].Pos.y = 720f;
        vtxPc3D[0].Pos.z = vtxPc3D[1].Pos.z = vtxPc3D[2].Pos.z = vtxPc3D[3].Pos.z = -2f;
        vtxPc3D[0].Col = vtxPc3D[1].Col = vtxPc3D[2].Col = vtxPc3D[3].Col = AMD_RGBA8888(0, 0, 0, byte.MaxValue);
        setParam.format3D = 2;
        setParam.type = 1;
        setParam.count = 4;
        setParam.ablend = 1;
        amDrawGetPrimBlendParam(0, setParam);
        setParam.aTest = 0;
        setParam.zMask = 1;
        setParam.zTest = 0;
        AoActDrawCorWide(vtxPc3D, 0, 4U, AOE_ACT_CORW.AOD_ACT_CORW_NONE);
        amDrawPrimitive3D(40U, setParam);
        amDrawMakeTask(new TaskProc(dmStaffRollTaskBgDraw), 2048, 0U);
        GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    private static void dmStaffRollTaskBgDraw(AMS_TCB tcb)
    {
        UNREFERENCED_PARAMETER(tcb);
        AoActDrawPre();
        amDrawExecCommand(40U);
        amDrawEndScene();
    }

    private static void dmStaffRollStageScrDraw(DMS_STFRL_MAIN_WORK main_work)
    {
        AMS_PARAM_DRAW_PRIMITIVE setParam = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.mtx = null;
        setParam.vtxPCT3D = dmStaffRollStageScrDraw_DrawArray;
        NNS_PRIM3D_PCT[] buffer = setParam.vtxPCT3D.buffer;
        int offset = setParam.vtxPCT3D.offset;
        buffer[offset].Pos.x = buffer[offset + 1].Pos.x = 160f;
        buffer[offset + 2].Pos.x = buffer[offset + 3].Pos.x = 672f;
        buffer[offset].Pos.y = buffer[offset + 2].Pos.y = 216f;
        buffer[offset + 1].Pos.y = buffer[offset + 3].Pos.y = 504f;
        buffer[offset].Pos.z = buffer[offset + 1].Pos.z = buffer[offset + 2].Pos.z = buffer[offset + 3].Pos.z = -2f;
        buffer[offset].Col = buffer[offset + 1].Col = buffer[2].Col = buffer[3].Col = AMD_RGBA8888(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)main_work.cur_page_scr_alpha_data);
        buffer[offset].Tex.u = buffer[offset + 1].Tex.u = 0.0f;
        buffer[offset + 2].Tex.u = buffer[offset + 3].Tex.u = 1f;
        buffer[offset].Tex.v = buffer[offset + 2].Tex.v = 0.0f;
        buffer[offset + 1].Tex.v = buffer[offset + 3].Tex.v = 9f / 16f;
        setParam.format3D = 4;
        setParam.type = 1;
        setParam.count = 4;
        setParam.texlist = AoTexGetTexList(dm_stfrl_scr_tex);
        setParam.texId = (int)main_work.cur_disp_image;
        setParam.ablend = 1;
        setParam.zOffset = -1f;
        setParam.aTest = 0;
        setParam.zMask = 1;
        setParam.zTest = 0;
        AoActDrawCorWide(dmStaffRollStageScrDraw_DrawArray, 0, 4U, AOE_ACT_CORW.AOD_ACT_CORW_LEFT);
        amDrawPrimitive3D(30U, setParam);
        amDrawMakeTask(new TaskProc(dmStaffRollStageScrTaskDraw), 2304, null);
        GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    private static void dmStaffRollStageScrTaskDraw(AMS_TCB tcb)
    {
        UNREFERENCED_PARAMETER(tcb);
        AoActDrawPre();
        amDrawExecCommand(30U);
        amDrawEndScene();
    }

    private static void dmStaffRollStaffListDraw(DMS_STFRL_MAIN_WORK main_work)
    {
        uint num1 = 0;
        uint lineNum = AoYsdFileGetLineNum(dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page);
        main_work.page_line_type = new uint[(int)lineNum];
        for (uint line_no = 0; line_no < lineNum; ++line_no)
        {
            main_work.page_line_type[(int)line_no] = AoYsdFileGetLineId(dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page, line_no);
            num1 += dm_stfrl_list_id_font_height_tbl[(int)main_work.page_line_type[(int)line_no]];
        }
        uint num2 = (uint)(360.0 - num1 / 2U);
        for (uint cur_line = 0; cur_line < lineNum; ++cur_line)
        {
            dmStaffRollStaffListOneLineDraw(main_work, (uint)(num2 * 0.899999976158142 * 0.899999976158142 + 5.0 + 32.0), cur_line);
            num2 += dm_stfrl_list_id_font_height_tbl[(int)main_work.page_line_type[(int)cur_line]];
        }
        main_work.page_line_type = null;
        amDrawMakeTask(new TaskProc(dmStaffRollTaskDraw), 2560, 0U);
    }

    public static void dmStaffRollStaffListOneLineDraw(
      DMS_STFRL_MAIN_WORK main_work,
      uint disp_pos_y,
      uint cur_line)
    {
        int index1 = 0;
        AMS_PARAM_DRAW_PRIMITIVE setParam = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        uint num1 = 0;
        uint[] numArray = new uint[3];
        float num2 = !dm_stfrl_is_full_staffroll || (int)main_work.cur_disp_list_page == (int)main_work.disp_list_page_num - 1 ? 0.0f : ((1 & (int)AoYsdFileGetPageOption(dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page)) == 0 ? 112f : 0.0f);
        uint lineId = AoYsdFileGetLineId(dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page, cur_line);
        if (lineId >= 4U && lineId <= 6U)
        {
            uint num3 = dm_stfrl_list_logo_width_tbl[(int)lineId];
            uint num4 = (uint)(480.0 - num3 / 2U + num2);
            for (int index2 = 0; index2 < 3; ++index2)
                numArray[index2] = byte.MaxValue;
            int num5;
            switch (lineId)
            {
                case 5:
                    num5 = 2;
                    break;
                case 6:
                    num5 = GsEnvGetRegion() != GSE_REGION.GSD_REGION_JP ? 4 : 3;
                    break;
                default:
                    num5 = (int)lineId - 4 + 1;
                    break;
            }
            setParam.mtx = null;
            setParam.vtxPCT3D = amDrawAlloc_NNS_PRIM3D_PCT(4);
            NNS_PRIM3D_PCT[] buffer = setParam.vtxPCT3D.buffer;
            int offset = setParam.vtxPCT3D.offset;
            buffer[offset].Pos.x = buffer[offset + 1].Pos.x = num4;
            buffer[offset + 2].Pos.x = buffer[offset + 3].Pos.x = num4 + num3;
            buffer[offset].Pos.y = buffer[offset + 2].Pos.y = disp_pos_y;
            buffer[offset + 1].Pos.y = buffer[offset + 3].Pos.y = disp_pos_y + dm_stfrl_list_id_font_height_tbl[(int)lineId];
            buffer[offset].Pos.z = buffer[offset + 1].Pos.z = buffer[offset + 2].Pos.z = buffer[offset + 3].Pos.z = -2f;
            buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 3].Col = AMD_RGBA8888((byte)numArray[0], (byte)numArray[1], (byte)numArray[2], (byte)main_work.cur_page_list_alpha_data);
            buffer[offset].Tex.u = buffer[offset + 1].Tex.u = 0.0f;
            buffer[offset + 2].Tex.u = buffer[offset + 3].Tex.u = 1f;
            buffer[offset].Tex.v = buffer[offset + 2].Tex.v = 0.0f;
            buffer[offset + 1].Tex.v = buffer[offset + 3].Tex.v = dm_stfrl_list_id_font_height_tbl[(int)lineId] / 128f;
            setParam.format3D = 4;
            setParam.type = 1;
            setParam.count = 4;
            setParam.texlist = AoTexGetTexList(dm_stfrl_font_tex);
            setParam.texId = num5;
            setParam.ablend = 1;
            setParam.aTest = 0;
            setParam.zMask = 1;
            setParam.zTest = 0;
            AoActDrawCorWide(setParam.vtxPCT3D, 0, 4U, AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
            amDrawPrimitive3D(20U, setParam);
            GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
        }
        else
        {
            int num3 = 0;
            float num4 = 32f * dm_stfrl_list_id_font_size_tbl[(int)lineId];
            for (int index2 = 0; index2 < 3; ++index2)
                numArray[index2] = dm_stfrl_list_id_font_color_tbl[(int)lineId][index2];
            string lineString = AoYsdFileGetLineString(dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page, cur_line);
            int index3 = 0;
            for (int length = lineString.Length; index3 < length; ++index3)
            {
                uint num5 = lineString[index3];
                float num6 = dm_stfrl_font_width_length_tbl[(int)num5] * dm_stfrl_list_id_font_size_tbl[(int)lineId];
                num1 += (uint)num6;
                ++num3;
            }
            uint num7 = (uint)(480.0 - num1 / 2U + num2);
            NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = amDrawAlloc_NNS_PRIM3D_PCT(6 * num3);
            for (int length = lineString.Length; index1 < length; ++index1)
            {
                setParam.Clear();
                byte num5 = (byte)lineString[index1];
                byte num6 = (byte)dm_stfrl_list_font_id_tbl[num5];
                byte num8 = num6 <= 0 ? (byte)0 : (byte)(num6 % 16U);
                byte num9 = num6 <= 0 ? (byte)0 : (byte)(num6 / 16U);
                setParam.mtx = null;
                NNS_PRIM3D_PCT_ARRAY array = amDrawAlloc_NNS_PRIM3D_PCT(4);
                setParam.vtxPCT3D = array;
                NNS_PRIM3D_PCT[] buffer = array.buffer;
                int offset = array.offset;
                buffer[offset].Pos.x = buffer[offset + 1].Pos.x = num7;
                buffer[offset + 2].Pos.x = buffer[offset + 3].Pos.x = num7 + num4;
                buffer[offset].Pos.y = buffer[offset + 2].Pos.y = disp_pos_y;
                buffer[offset + 1].Pos.y = buffer[offset + 3].Pos.y = disp_pos_y + num4;
                buffer[offset].Pos.z = buffer[offset + 1].Pos.z = buffer[offset + 2].Pos.z = buffer[offset + 3].Pos.z = -2f;
                float num10 = dm_stfrl_font_width_length_tbl[num5] * dm_stfrl_list_id_font_size_tbl[(int)lineId];
                num7 = (uint)(num7 + (double)num10);
                buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 3].Col = AMD_RGBA8888(numArray[0], numArray[1], numArray[2], main_work.cur_page_list_alpha_data);
                buffer[offset].Tex.u = buffer[offset + 1].Tex.u = num8 * (1f / 16f);
                buffer[offset + 2].Tex.u = buffer[offset + 3].Tex.u = buffer[offset].Tex.u + 1f / 16f;
                buffer[offset].Tex.v = buffer[offset + 2].Tex.v = num9 * 0.125f;
                buffer[offset + 1].Tex.v = buffer[offset + 3].Tex.v = buffer[offset].Tex.v + 0.125f;
                setParam.format3D = 4;
                setParam.type = 1;
                setParam.count = 4;
                setParam.texlist = AoTexGetTexList(dm_stfrl_font_tex);
                setParam.texId = 0;
                setParam.ablend = 1;
                setParam.aTest = 0;
                setParam.zMask = 1;
                setParam.zTest = 0;
                AoActDrawCorWide(array, 0, 4U, AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
                int index2 = nnsPriM3DPctArray.offset + index1 * 6;
                nnsPriM3DPctArray.buffer[index2] = buffer[offset];
                nnsPriM3DPctArray.buffer[index2 + 1] = buffer[offset];
                nnsPriM3DPctArray.buffer[index2 + 2] = buffer[offset + 1];
                nnsPriM3DPctArray.buffer[index2 + 3] = buffer[offset + 2];
                nnsPriM3DPctArray.buffer[index2 + 4] = buffer[offset + 3];
                nnsPriM3DPctArray.buffer[index2 + 5] = buffer[offset + 3];
                if (index1 + 1 >= lineString.Length)
                {
                    setParam.vtxPCT3D = nnsPriM3DPctArray;
                    setParam.count = 6 * num3;
                    amDrawPrimitive3D(20U, setParam);
                }
            }
            GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
        }
    }

    private static void dmStaffRollTaskDraw(AMS_TCB tcb)
    {
        UNREFERENCED_PARAMETER(tcb);
        AoActDrawPre();
        amDrawExecCommand(20U);
        amDrawEndScene();
    }

    private static void dmStaffRollEndActDraw(DMS_STFRL_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(2304U);
        AoActSysSetDrawStateEnable(true);
        AoActSysSetDrawState(30U);
        if (((int)main_work.flag & 4096) != 0)
            ++main_work.continue_act_frm;
        if (main_work.is_eme_comp)
        {
            AoActSetTexture(AoTexGetTexList(dm_stfrl_end_tex));
            if (main_work.end_act_frm > 0)
            {
                for (int index = 0; index <= 5; ++index)
                    AoActSortRegAction(main_work.act[index]);
            }
            for (int index = 6; index <= 7; ++index)
                AoActSortRegAction(main_work.act[index]);
            AoActSetTexture(AoTexGetTexList(dm_stfrl_end_jp_tex));
            AoActSortRegAction(main_work.act[9]);
            AoActSetFrame(main_work.act[9], main_work.end_act_frm);
        }
        else
            AoActSortRegAction(main_work.act[8]);
        for (int index = 0; index <= 7; ++index)
            AoActSetFrame(main_work.act[index], main_work.end_act_frm);
        for (int index = 0; index < 11; ++index)
        {
            if (index <= 7)
                AoActSetTexture(AoTexGetTexList(dm_stfrl_end_tex));
            else
                AoActSetTexture(AoTexGetTexList(dm_stfrl_end_jp_tex));
            AoActUpdate(main_work.act[index], 0.0f);
        }
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
        amDrawMakeTask(new TaskProc(dmStaffRollEndActTaskDraw), 2304, 0U);
    }

    private static void dmStaffRollEndActTaskDraw(AMS_TCB tcb)
    {
        UNREFERENCED_PARAMETER(tcb);
        AoActDrawPre();
        amDrawExecCommand(30U);
        amDrawEndScene();
    }

    private static void dmStaffRollWinActDraw(DMS_STFRL_MAIN_WORK main_work)
    {
        float[] numArray = new float[2];
        AoActSysSetDrawTaskPrio(3072U);
        AoActSysSetDrawStateEnable(true);
        AoActSysSetDrawState(50U);
        numArray[0] = 641.25f;
        numArray[1] = 303.75f;
        AoWinSysDrawState(0, AoTexGetTexList(dm_stfrl_cmn_tex[0]), 1U, 480f, 356f, numArray[0] * main_work.win_size_rate[0], (float)(numArray[1] * (double)main_work.win_size_rate[1] * 0.899999976158142), 50U);
        if (((int)main_work.flag & 64) != 0)
        {
            AoActSetTexture(AoTexGetTexList(dm_stfrl_cmn_tex[0]));
            AoActSetTexture(AoTexGetTexList(dm_stfrl_cmn_tex[1]));
            AoActSetTexture(AoTexGetTexList(dm_stfrl_end_jp_tex));
            AoActSortRegAction(main_work.act[10]);
        }
        AoActAcmPush();
        AoActAcmInit();
        AoActAcmApplyScale(27f / 16f, 27f / 16f);
        AoActAcmApplyTrans(dm_stfrl_win_act_pos_tbl[3][0], dm_stfrl_win_act_pos_tbl[3][1], 0.0f);
        AoActSetTexture(AoTexGetTexList(dm_stfrl_end_jp_tex));
        AoActUpdate(main_work.act[10], 0.0f);
        AoActAcmPop();
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
        amDrawMakeTask(new TaskProc(dmStaffRollWinActTaskDraw), 3072, 0U);
    }

    private static void dmStaffRollWinActTaskDraw(AMS_TCB tcb)
    {
        UNREFERENCED_PARAMETER(tcb);
        AoActDrawPre();
        amDrawExecCommand(50U);
        amDrawEndScene();
    }

    private static int dmStaffRollIsDataLoad(DMS_STFRL_MAIN_WORK main_work)
    {
        if (dm_stfrl_is_full_staffroll)
        {
            if (main_work.staff_list_fs is AMS_FS && !amFsIsComplete((AMS_FS)main_work.staff_list_fs))
                return 0;
        }
        else if (!amFsIsComplete(main_work.arc_list_font_amb_fs))
            return 0;
        return 1;
    }

    private static void dmStaffRollDataClearRequestFull(DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.arc_data.arc_font_amb = main_work.arc_list_font_amb;
        main_work.arc_data.arc_scr_amb = readAMBFile(main_work.arc_scr_amb_fs);
        main_work.arc_data.arc_end_amb = readAMBFile(main_work.arc_end_amb_fs);
        main_work.arc_data.arc_end_jp_amb = readAMBFile(main_work.arc_end_jp_amb_fs);
        for (uint index = 0; index < 2U; ++index)
            main_work.arc_data.arc_cmn_amb[(int)index] = readAMBFile(main_work.arc_cmn_amb_fs[(int)index]);
        main_work.arc_data.stf_list_ysd = new YSDS_HEADER((byte[])main_work.staff_list_fs);
        main_work.staff_list_fs = null;
    }

    private static void dmStaffRollDataClearRequestEasy(DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.arc_data.arc_font_amb = readAMBFile(main_work.arc_list_font_amb_fs);
        main_work.arc_list_font_amb_fs = null;
        main_work.arc_list_font_amb_fs = null;
        main_work.arc_data.stf_list_ysd = new YSDS_HEADER((byte[])main_work.staff_list_fs);
        main_work.staff_list_fs = null;
        main_work.staff_list_fs = null;
    }

    private static void dmStaffRollDataBuildEasy(DMS_STFRL_MAIN_WORK main_work)
    {
        dm_stfrl_font_amb = main_work.arc_data.arc_font_amb;
        AoTexBuild(dm_stfrl_font_tex, dm_stfrl_font_amb);
        AoTexLoad(dm_stfrl_font_tex);
        dm_stfrl_data_mgr_p = main_work.arc_data;
    }

    private static int dmStaffRollIsTexLoad()
    {
        if (dm_stfrl_is_full_staffroll)
        {
            if (!AoTexIsLoaded(dm_stfrl_font_tex) || !AoTexIsLoaded(dm_stfrl_scr_tex) || (!AoTexIsLoaded(dm_stfrl_end_tex) || !AoTexIsLoaded(dm_stfrl_end_jp_tex)))
                return 0;
            for (int index = 0; index < 2; ++index)
            {
                if (!AoTexIsLoaded(dm_stfrl_cmn_tex[index]))
                    return 0;
            }
        }
        else if (!AoTexIsLoaded(dm_stfrl_font_tex))
            return 0;
        return 1;
    }

    private static int dmStaffRollIsTexRelease()
    {
        if (dm_stfrl_is_full_staffroll)
        {
            if (!AoTexIsReleased(dm_stfrl_font_tex) || !AoTexIsReleased(dm_stfrl_scr_tex) || (!AoTexIsReleased(dm_stfrl_end_tex) || !AoTexIsReleased(dm_stfrl_end_jp_tex)))
                return 0;
            for (int index = 0; index < 2; ++index)
            {
                if (!AoTexIsReleased(dm_stfrl_cmn_tex[index]))
                    return 0;
            }
        }
        else if (!AoTexIsReleased(dm_stfrl_font_tex))
            return 0;
        return 1;
    }

    private static void dmStaffRollSetFadePageInfoEfctData(DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.disp_page_time = AoYsdFileGetPageTime(dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page);
        main_work.disp_page_time = (uint)(main_work.disp_page_time - 32.0 - 2.0);
        main_work.fade_timer = main_work.disp_page_time;
        if (AoYsdFileIsPageShowImage(dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page))
        {
            main_work.flag |= 16U;
            main_work.cur_disp_image = AoYsdFileGetPageShowImageNo(dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page);
            for (int index = 0; index < 3; ++index)
            {
                if (main_work.ring_work[index] != null)
                    main_work.ring_work[index].flag |= 1U;
            }
        }
        if (!AoYsdFileIsPageHideImage(dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page))
            return;
        main_work.flag |= 32U;
    }

    private static void dmStaffRollSetEfctChngAlphaListData(DMS_STFRL_MAIN_WORK main_work)
    {
        int num1 = (int)main_work.cur_page_list_alpha_data;
        int num2 = (int)main_work.cur_page_scr_alpha_data;
        if (main_work.fade_timer >= (double)(main_work.disp_page_time - 32f))
        {
            num1 += 8;
            if (((int)main_work.flag & 16) != 0)
            {
                num2 += 8;
                if (num2 >= byte.MaxValue)
                {
                    num2 = byte.MaxValue;
                    main_work.flag &= 4294967279U;
                }
            }
        }
        else if (main_work.fade_timer <= 32.0)
        {
            num1 -= 8;
            if (((int)main_work.flag & 32) != 0)
            {
                num2 -= 8;
                if (num2 <= 0)
                {
                    num2 = 0;
                    main_work.flag &= 4294967263U;
                }
            }
        }
        if (num1 >= byte.MaxValue)
            num1 = byte.MaxValue;
        if (num1 <= 0)
            num1 = 0;
        main_work.cur_page_list_alpha_data = (uint)num1;
        main_work.cur_page_scr_alpha_data = (uint)num2;
    }

    private static void dmStaffRollSetWinOpenEfct(DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.win_timer > 8.0)
        {
            main_work.flag |= 512U;
            main_work.win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int)index] = 1f;
        }
        else
            ++main_work.win_timer;
        for (uint index = 0; index < 2U; ++index)
        {
            main_work.win_size_rate[(int)index] = 0.0 == main_work.win_timer ? 1f : main_work.win_timer / 8f;
            if (main_work.win_size_rate[(int)index] > 1.0)
                main_work.win_size_rate[(int)index] = 1f;
        }
    }

    private static void dmStaffRollSetWinCloseEfct(DMS_STFRL_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
            main_work.win_size_rate[(int)index] = 0.0 == main_work.win_timer ? 0.0f : main_work.win_timer / 8f;
        if (main_work.win_timer < 0.0)
        {
            main_work.flag |= 512U;
            main_work.win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int)index] = 0.0f;
        }
        else
            --main_work.win_timer;
    }

    private static void dmStaffRollSetObjSystemData(DMS_STFRL_MAIN_WORK main_work)
    {
        ObjDrawESEffectSystemInit(0, 20480U, 5U);
        ObjDrawSetNNCommandStateTbl(0U, 1U, false);
        ObjDrawSetNNCommandStateTbl(1U, 2U, false);
        ObjDrawSetNNCommandStateTbl(2U, 3U, true);
        ObjDrawSetNNCommandStateTbl(3U, 5U, true);
        ObjDrawSetNNCommandStateTbl(4U, 11U, true);
        ObjDrawSetNNCommandStateTbl(5U, 12U, true);
        ObjDrawSetNNCommandStateTbl(6U, 9U, true);
        ObjDrawSetNNCommandStateTbl(7U, 4U, true);
        ObjDrawSetNNCommandStateTbl(8U, 8U, true);
        ObjDrawSetNNCommandStateTbl(9U, 7U, true);
        ObjDrawSetNNCommandStateTbl(10U, 10U, true);
        ObjDrawSetNNCommandStateTbl(11U, 6U, true);
        ObjDrawSetNNCommandStateTbl(12U, 0U, true);
        if (((int)g_gm_main_system.game_flag & 512) != 0)
            g_gm_main_system.game_time = 0U;
        g_gm_main_system.game_flag &= 0b11111001100101111101110000000001U;
        g_obj.flag = 0b10000000000000001101000U;
        g_obj.ppPre = null;
        g_obj.ppPost = null;
        g_obj.ppCollision = null;
        g_obj.ppObjPre = new OBJECT_WORK_Delegate(GmObjObjPreFunc);
        g_obj.ppObjPost = new OBJECT_WORK_Delegate(GmObjObjPostFunc);
        g_obj.ppRegRecAuto = null;
        g_obj.draw_scale.x = g_obj.draw_scale.y = g_obj.draw_scale.z = 13107;
        g_obj.inv_draw_scale.x = g_obj.inv_draw_scale.y = g_obj.inv_draw_scale.z = FX_Div(4096, g_obj.draw_scale.x);
        g_obj.depth = 128;
        dmStaffRollInitLight();
        dmStaffRollCameraInit();
        GmEfctCmnBuildDataInit();
    }

    private static void dmStaffRollSetupEndModel(DMS_STFRL_MAIN_WORK main_work)
    {
        for (int index = 0; index < 3; ++index)
        {
            if (main_work.ring_work[index] != null)
            {
                OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)main_work.ring_work[index];
                obsObjectWork.ppOut = null;
                obsObjectWork.flag |= 8U;
            }
        }
        dmStaffRollSetBossObj(main_work);
        if (!main_work.is_eme_comp)
            return;
        main_work.sonic_work = DmStfrlMdlCtrlSetSonicObj();
    }

    private static void dmStaffRollNodispEndModel(DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.body_work != null)
        {
            OBS_OBJECT_WORK bodyWork = (OBS_OBJECT_WORK)main_work.body_work;
            bodyWork.ppOut = null;
            bodyWork.flag |= 8U;
        }
        if (main_work.egg_work != null)
        {
            OBS_OBJECT_WORK eggWork = (OBS_OBJECT_WORK)main_work.egg_work;
            eggWork.ppOut = null;
            eggWork.flag |= 8U;
        }
        for (int index = 0; index < 3; ++index)
        {
            if (main_work.ring_work[index] != null)
            {
                OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)main_work.ring_work[index];
                obsObjectWork.ppOut = null;
                obsObjectWork.flag |= 8U;
            }
        }
        if (main_work.sonic_work == null)
            return;
        OBS_OBJECT_WORK sonicWork = (OBS_OBJECT_WORK)main_work.sonic_work;
        sonicWork.ppOut = null;
        sonicWork.flag |= 8U;
    }

    private static void dmStaffRollSetBossObj(DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.body_work = DmStfrlMdlCtrlSetBodyObj();
        main_work.egg_work = DmStfrlMdlCtrlSetEggObj((OBS_OBJECT_WORK)main_work.body_work);
        if (!main_work.is_eme_comp)
            return;
        main_work.body_work.flag |= 1U;
    }

    private static void dmStaffRollInitLight()
    {
        NNS_RGBA col = new NNS_RGBA(1f, 1f, 1f, 1f);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        g_obj.ambient_color.r = 0.8f;
        g_obj.ambient_color.g = 0.8f;
        g_obj.ambient_color.b = 0.8f;
        nnsVector.x = -1f;
        nnsVector.y = -1f;
        nnsVector.z = -1f;
        nnNormalizeVector(nnsVector, nnsVector);
        ObjDrawSetParallelLight(NNE_LIGHT_0, ref col, 1f, nnsVector);
        g_gm_main_system.def_light_vec.Assign(nnsVector);
        g_gm_main_system.def_light_col = col;
        ObjDrawSetParallelLight(NNE_LIGHT_6, ref col, 1f, nnsVector);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    private static void dmStaffRollCameraInit()
    {
        ObjCameraInit(0, new NNS_VECTOR(0.0f, 0.0f, 50f), 4, 0, 8192);
        ObjCamera3dInit(0);
        g_obj.glb_camera_id = 0;
        g_obj.glb_camera_type = 1;
        GmCameraDelayReset();
        GmCameraAllowReset();
        ObjCameraSetUserFunc(0, new OBJF_CAMERA_USER_FUNC(dmStaffRollCameraFunc));
        OBS_CAMERA obsCamera = ObjCameraGet(0);
        obsCamera.scale = GMD_CAMERA_SCALE;
        obsCamera.ofst.z = 1000f;
    }

    private static void dmStaffRollCameraFunc(OBS_CAMERA obj_camera)
    {
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.x = FXM_FX32_TO_FLOAT(0);
        nnsVector.y = FXM_FX32_TO_FLOAT(0);
        nnsVector.z = FXM_FX32_TO_FLOAT(409600);
        obj_camera.work.x = nnsVector.x;
        obj_camera.work.y = nnsVector.y;
        obj_camera.work.z = nnsVector.z;
        obj_camera.prev_pos.x = obj_camera.pos.x;
        obj_camera.prev_pos.y = obj_camera.pos.y;
        obj_camera.prev_pos.z = obj_camera.pos.z;
        obj_camera.pos.x = 0.0f;
        obj_camera.pos.y = 0.0f;
        obj_camera.pos.z = 50f;
        obj_camera.disp_pos.x = obj_camera.pos.x;
        obj_camera.disp_pos.y = obj_camera.pos.y;
        obj_camera.disp_pos.z = obj_camera.pos.z;
        obj_camera.target_pos.Assign(obj_camera.disp_pos);
        obj_camera.target_pos.z -= 50f;
        ObjObjectCameraSet(FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - OBD_LCD_Y / 2), FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - OBD_LCD_Y / 2));
        GmCameraSetClipCamera(obj_camera);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    public void DmMovieInit(object arg)
    {
        SyDecideEvtCase(0);
        SyChangeNextEvt();
    }


    private static void DmStfrlMdlCtrlSonicBuild()
    {
        dm_stfrl_sonic_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(g_gm_player_data_work[0][0].pData), readAMBFile(g_gm_player_data_work[0][1].pData), 0U);
    }

    private static void DmStfrlMdlCtrlSonicFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(g_gm_player_data_work[0][0].pData);
        GmGameDBuildRegFlushModel(dm_stfrl_sonic_obj_3d_list, amsAmbHeader.file_num);
        dm_stfrl_sonic_obj_3d_list = null;
    }

    private static DMS_STFRL_SONIC_WORK DmStfrlMdlCtrlSetSonicObj()
    {
        OBS_OBJECT_WORK obj_work = OBM_OBJECT_TASK_DETAIL_INIT(24576, 0, 0, 0, () => new DMS_STFRL_SONIC_WORK(), "STAFFROLL_SONIC");
        DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (DMS_STFRL_SONIC_WORK)obj_work;
        ObjObjectCopyAction3dNNModel(obj_work, dm_stfrl_sonic_obj_3d_list[0], obj_work.obj_3d);
        obj_work.obj_3d.blend_spd = 1f / 16f;
        ObjDrawObjectSetToon(obj_work);
        OBS_ACTION3D_NN_WORK obj3d = dmsStfrlSonicWork.obj_work.obj_3d;
        ObjObjectAction3dNNMotionLoad(obj_work, 0, true, g_gm_player_data_work[0][4], null, 0, null, 136, 16);
        obj_work.flag |= 16U;
        obj_work.disp_flag |= 4194309U;
        obj_work.disp_flag &= 4294967263U;
        obj_work.disp_flag |= 150995456U;
        obj_work.obj_3d.drawflag |= 8388608U;
        obj_work.pos.x = 0;
        obj_work.pos.y = 98304;
        obj_work.pos.z = -12288;
        obj_work.dir.y = (ushort)AKM_DEGtoA16(90);
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        dmsStfrlSonicWork.alpha = 1f;
        ObjDrawObjectActionSet(obj_work, 21);
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlSonicDrawFunc);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlSonicProcWaitSetup);
        return dmsStfrlSonicWork;
    }

    private static void DmStfrlMdlCtrlBoss1Build()
    {
        dm_stfrl_boss1_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(ObjDataLoadAmbIndex(null, 0, g_gm_gamedat_enemy_arc)), readAMBFile(ObjDataLoadAmbIndex(null, 1, g_gm_gamedat_enemy_arc)), 0U);
        ObjDataLoadAmbIndex(ObjDataGet(728), 2, g_gm_gamedat_enemy_arc);
        ObjDataLoadAmbIndex(ObjDataGet(729), 3, g_gm_gamedat_enemy_arc);
    }

    private static void DmStfrlMdlCtrlBoss1Flush()
    {
        ObjDataRelease(ObjDataGet(729));
        ObjDataRelease(ObjDataGet(728));
        AMS_AMB_HEADER amsAmbHeader = (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, g_gm_gamedat_enemy_arc);
        GmGameDBuildRegFlushModel(dm_stfrl_boss1_obj_3d_list, amsAmbHeader.file_num);
        dm_stfrl_boss1_obj_3d_list = null;
    }

    private static DMS_STFRL_BOSS_BODY_WORK DmStfrlMdlCtrlSetBodyObj()
    {
        OBS_OBJECT_WORK obj_work = OBM_OBJECT_TASK_DETAIL_INIT(24576, 0, 0, 0, () => new DMS_STFRL_BOSS_BODY_WORK(), "BOSS1_BODY");
        DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (DMS_STFRL_BOSS_BODY_WORK)obj_work;
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(dmStfrlMdlCtrlBoss1BodyExit));
        obj_work.flag |= 16U;
        obj_work.disp_flag |= 4194309U;
        obj_work.disp_flag &= 4294967263U;
        ObjObjectCopyAction3dNNModel(obj_work, dm_stfrl_boss1_obj_3d_list[0], obj_work.obj_3d);
        ObjObjectAction3dNNMotionLoad(obj_work, 0, true, ObjDataGet(728), null, 0, null);
        ObjDrawObjectSetToon(obj_work);
        obj_work.obj_3d.blend_spd = 0.125f;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(ObjDrawActionSummary);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlBodyProcWaitSetup);
        return stfrlBossBodyWork;
    }

    private static DMS_STFRL_BOSS_EGG_WORK DmStfrlMdlCtrlSetEggObj(
      OBS_OBJECT_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = OBM_OBJECT_TASK_DETAIL_INIT(24576, 0, 0, 0, () => new DMS_STFRL_BOSS_EGG_WORK(), "BOSS1_EGG");
        DMS_STFRL_BOSS_EGG_WORK stfrlBossEggWork = (DMS_STFRL_BOSS_EGG_WORK)obj_work;
        obj_work.parent_obj = body_work;
        obj_work.move_flag |= 256U;
        ObjObjectCopyAction3dNNModel(obj_work, dm_stfrl_boss1_obj_3d_list[1], obj_work.obj_3d);
        ObjObjectAction3dNNMotionLoad(obj_work, 0, true, ObjDataGet(729), null, 0, null);
        ObjDrawObjectSetToon(obj_work);
        obj_work.disp_flag |= 134217728U;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(ObjDrawActionSummary);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlEggProcWaitSetup);
        obj_work.flag |= 16U;
        obj_work.disp_flag |= 4U;
        obj_work.disp_flag |= 4194304U;
        return stfrlBossEggWork;
    }

    private static void DmStfrlMdlCtrlRingBuild()
    {
        dm_stfrl_ring_obj_3d = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(956), GmGameDatGetGimmickData(957), 0U);
    }

    private static void DmStfrlMdlCtrlRingFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(956);
        GmGameDBuildRegFlushModel(dm_stfrl_ring_obj_3d, gimmickData.file_num);
        dm_stfrl_ring_obj_3d = null;
    }

    private static DMS_STFRL_RING_WORK DmStfrlMdlCtrlSetRingObj(
      int delay_time,
      uint type)
    {
        OBS_OBJECT_WORK obj_work = OBM_OBJECT_TASK_DETAIL_INIT(24576, 0, 0, 0, () => new DMS_STFRL_RING_WORK(), "RING_OBJ");
        DMS_STFRL_RING_WORK dmsStfrlRingWork = (DMS_STFRL_RING_WORK)obj_work;
        obj_work.flag |= 16U;
        obj_work.disp_flag |= 4194309U;
        obj_work.disp_flag &= 4294967263U;
        obj_work.disp_flag |= 134217728U;
        ObjObjectCopyAction3dNNModel(obj_work, dm_stfrl_ring_obj_3d[0], obj_work.obj_3d);
        ObjObjectAction3dNNMaterialMotionLoad(obj_work, 0, null, null, 0, readAMBFile(ObjDataGet(4).pData));
        obj_work.disp_flag |= 4194309U;
        obj_work.disp_flag &= 4294967263U;
        obj_work.disp_flag |= 150995456U;
        obj_work.obj_3d.drawflag |= 8388608U;
        obj_work.obj_3d.draw_state.alpha.alpha = 0.0f;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlRingDrawFunc);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlRingProcStartWait);
        dmsStfrlRingWork.disp_ring_pos_no = (int)type;
        dmsStfrlRingWork.start_pos.x = dm_stfrl_ring_disp_pos_tbl[dmsStfrlRingWork.disp_ring_pos_no][0];
        dmsStfrlRingWork.start_pos.y = dm_stfrl_ring_disp_pos_tbl[dmsStfrlRingWork.disp_ring_pos_no][1];
        dmsStfrlRingWork.start_pos.z = -12288;
        dmsStfrlRingWork.efct_start_time = delay_time;
        dmsStfrlRingWork.disp_efct_pos_no = (int)type;
        return dmsStfrlRingWork;
    }

    private static void dmStfrlMdlCtrlSonicProcWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (DMS_STFRL_SONIC_WORK)obj_work;
        ++dmsStfrlSonicWork.timer;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (dmsStfrlSonicWork.timer > 300)
        {
            ObjDrawObjectActionSet3DNNBlend(obj_work, 42);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlSonicProcWaitChngDash2);
            dmsStfrlSonicWork.timer = 0;
        }
        else
            ObjDrawObjectActionSet3DNNBlend(obj_work, 21);
    }

    private static void dmStfrlMdlCtrlSonicProcWaitChngDash2(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (DMS_STFRL_SONIC_WORK)obj_work;
        ++dmsStfrlSonicWork.timer;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (dmsStfrlSonicWork.timer > 30)
        {
            obj_work.obj_3d.blend_spd = 0.125f;
            ObjDrawObjectActionSet3DNNBlend(obj_work, 9);
            GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(obj_work, 53);
            GmComEfctSetDispOffsetF(efct_work, -1.5f, 0.0f, 9f);
            efct_work.obj_3des.ecb.drawObjState = 0U;
            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlSonicProcWaitMtnEnd);
            dmsStfrlSonicWork.timer = 0;
        }
        else
            ObjDrawObjectActionSet3DNNBlend(obj_work, 42);
    }

    private static void dmStfrlMdlCtrlSonicProcWaitMtnEnd(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (DMS_STFRL_SONIC_WORK)obj_work;
        ++dmsStfrlSonicWork.timer;
        if (obj_work.spd_m <= 25292)
            obj_work.spd_m += 512;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        ObjDrawObjectActionSet3DNNBlend(obj_work, 9);
        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        if (dmsStfrlSonicWork.timer <= 60)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlSonicProcWaitFadeEnd);
        ObjDrawObjectActionSet3DNNBlend(obj_work, 9);
        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        dmsStfrlSonicWork.timer = 120;
    }

    private static void dmStfrlMdlCtrlSonicProcWaitFadeEnd(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (DMS_STFRL_SONIC_WORK)obj_work;
        --dmsStfrlSonicWork.timer;
        obj_work.pos.x += 73728;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (dmsStfrlSonicWork.timer <= 0)
        {
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlSonicProcIdle);
            dmsStfrlSonicWork.timer = 0;
            dmsStfrlSonicWork.flag |= 1;
            obj_work.disp_flag |= 32U;
        }
        else
        {
            ObjDrawObjectActionSet3DNNBlend(obj_work, 9);
            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        }
    }

    private static void dmStfrlMdlCtrlSonicProcIdle(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void dmStfrlMdlCtrlSonicDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (DMS_STFRL_SONIC_WORK)obj_work;
        obj_work.obj_3d.draw_state.alpha.alpha = dmsStfrlSonicWork.alpha;
        ObjDrawActionSummary(obj_work);
    }

    private static void dmStfrlMdlCtrlBoss1BodyExit(MTS_TASK_TCB tcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (DMS_STFRL_BOSS_BODY_WORK)tcbWork;
        GmBsCmnClearBossMotionCBSystem(tcbWork);
        GmBsCmnDeleteSNMWork(stfrlBossBodyWork.snm_work);
        ObjObjectExit(tcb);
    }

    private static void dmStfrlMdlCtrlBodyProcWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (DMS_STFRL_BOSS_BODY_WORK)obj_work;
        GmBsCmnInitBossMotionCBSystem(obj_work, stfrlBossBodyWork.bmcb_mgr);
        GmBsCmnCreateSNMWork(stfrlBossBodyWork.snm_work, obj_work.obj_3d._object, 1);
        GmBsCmnAppendBossMotionCallback(stfrlBossBodyWork.bmcb_mgr, stfrlBossBodyWork.snm_work.bmcb_link);
        stfrlBossBodyWork.egg_snm_reg_id = GmBsCmnRegisterSNMNode(stfrlBossBodyWork.snm_work, 11);
        if (((int)stfrlBossBodyWork.flag & 1) != 0)
        {
            stfrlBossBodyWork.timer = 0;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlBodyProcBodyCompInitStart);
        }
        else
        {
            stfrlBossBodyWork.timer = 120;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlBodyProcBodyMain);
        }
    }

    private static void dmStfrlMdlCtrlBodyProcBodyMain(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (DMS_STFRL_BOSS_BODY_WORK)obj_work;
        obj_work.pos.x = 0;
        obj_work.pos.y = -65536;
        obj_work.pos.z = -81920;
        obj_work.dir.y = (ushort)AKM_DEGtoA16(300);
        if (stfrlBossBodyWork.timer != 0)
            --stfrlBossBodyWork.timer;
        else
            stfrlBossBodyWork.flag |= 2097152U;
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompInitStart(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (DMS_STFRL_BOSS_BODY_WORK)obj_work;
        obj_work.pos.x = 0;
        obj_work.pos.y = -737280;
        obj_work.pos.z = -81920;
        obj_work.dir.y = (ushort)AKM_DEGtoA16(300);
        if (((int)stfrlBossBodyWork.flag & 2) == 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlBodyProcBodyCompStartWait);
        stfrlBossBodyWork.flag &= 4294967293U;
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompStartWait(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (DMS_STFRL_BOSS_BODY_WORK)obj_work;
        ++stfrlBossBodyWork.timer;
        if (stfrlBossBodyWork.timer <= 60)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlBodyProcBodyCompMoveDown);
        stfrlBossBodyWork.timer = 0;
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompMoveDown(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (DMS_STFRL_BOSS_BODY_WORK)obj_work;
        obj_work.pos.y += 4096;
        if (obj_work.pos.y < -163840)
            return;
        obj_work.pos.y = -163840;
        stfrlBossBodyWork.flag |= 2097152U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlBodyProcBodyCompLaughWait);
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompLaughWait(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (DMS_STFRL_BOSS_BODY_WORK)obj_work;
        ++stfrlBossBodyWork.timer;
        if (stfrlBossBodyWork.timer < 180)
            return;
        stfrlBossBodyWork.timer = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlBodyProcBodyCompMoveUpWait);
        stfrlBossBodyWork.flag &= 4294967291U;
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompMoveUpWait(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (DMS_STFRL_BOSS_BODY_WORK)obj_work;
        obj_work.pos.y += -2457;
        if (stfrlBossBodyWork.timer > 100)
            stfrlBossBodyWork.flag |= 16U;
        else
            ++stfrlBossBodyWork.timer;
        if (obj_work.pos.y > -737280)
            return;
        obj_work.pos.y = -737280;
        stfrlBossBodyWork.flag |= 8U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlBodyProcBodyCompEndWaitIdle);
        stfrlBossBodyWork.timer = 0;
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompEndWaitIdle(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void dmStfrlMdlCtrlEggProcWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d.blend_spd = 0.025f;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlEggProcMain);
    }

    private static void dmStfrlMdlCtrlEggProcMain(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_BOSS_BODY_WORK parentObj = (DMS_STFRL_BOSS_BODY_WORK)obj_work.parent_obj;
        DMS_STFRL_BOSS_EGG_WORK stfrlBossEggWork = (DMS_STFRL_BOSS_EGG_WORK)obj_work;
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.egg_snm_reg_id, 1);
        if (((int)parentObj.flag & 2097152) == 0 || ((int)stfrlBossEggWork.flag & 1) != 0)
            return;
        ObjDrawObjectActionSet3DNNBlend(obj_work, 3);
        obj_work.obj_3d.frame[0] = 0.0f;
        obj_work.obj_3d.blend_spd = 0.025f;
        stfrlBossEggWork.flag |= 1U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlEggProcMainIdle);
    }

    private static void dmStfrlMdlCtrlEggProcMainIdle(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_BOSS_BODY_WORK parentObj = (DMS_STFRL_BOSS_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.egg_snm_reg_id, 1);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        ObjDrawObjectActionSet3DNNBlend(obj_work, 3);
        obj_work.obj_3d.frame[0] = 0.0f;
        obj_work.obj_3d.blend_spd = 0.125f;
    }

    private static void dmStfrlMdlCtrlRingProcStartWait(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_RING_WORK dmsStfrlRingWork = (DMS_STFRL_RING_WORK)obj_work;
        if (((int)dmsStfrlRingWork.flag & 1) == 0)
            return;
        ++dmsStfrlRingWork.timer;
        if (dmsStfrlRingWork.timer < dmsStfrlRingWork.efct_start_time)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlRingProcInitSetup);
        dmStfrlMdlCtrlCreateRingEfct(dmsStfrlRingWork.start_pos.x + dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no][0], dmsStfrlRingWork.start_pos.y + dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no][1]);
        dmStfrlMdlCtrlCreateRingEfct(dmsStfrlRingWork.start_pos.x + dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no + 1][0], dmsStfrlRingWork.start_pos.y + dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no + 1][1]);
        dmStfrlMdlCtrlCreateRingEfct(dmsStfrlRingWork.start_pos.x + dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no + 2][0], dmsStfrlRingWork.start_pos.y + dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no + 2][1]);
        dmsStfrlRingWork.timer = 0;
        dmsStfrlRingWork.flag &= 4294967294U;
    }

    private static void dmStfrlMdlCtrlRingProcInitSetup(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_RING_WORK dmsStfrlRingWork = (DMS_STFRL_RING_WORK)obj_work;
        obj_work.obj_3d.draw_state.alpha.alpha = 0.0f;
        ushort num1 = 10922;
        ushort num2 = 0;
        for (int index = 0; index < 6; ++index)
        {
            dmsStfrlRingWork.pos[index].x = dmsStfrlRingWork.start_pos.x;
            dmsStfrlRingWork.pos[index].y = dmsStfrlRingWork.start_pos.y;
            dmsStfrlRingWork.pos[index].z = -3;
            dmsStfrlRingWork.spd_x[index] = mtMathSin((ushort)(num2 + (uint)index * num1));
            dmsStfrlRingWork.spd_y[index] = mtMathCos((ushort)(num2 + (uint)index * num1));
            dmsStfrlRingWork.spd_y[index] += 512;
        }
        dmsStfrlRingWork.alpha_spd = 0.1f;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlRingProcDispIdle);
    }

    private static void dmStfrlMdlCtrlRingProcDispIdle(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_RING_WORK dmsStfrlRingWork = (DMS_STFRL_RING_WORK)obj_work;
        for (int index = 0; index < 6; ++index)
            dmsStfrlRingWork.spd_y[index] += 64;
        ++dmsStfrlRingWork.timer;
        if (dmsStfrlRingWork.timer >= 10)
        {
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlRingProcNoDispIdle);
            dmsStfrlRingWork.timer = 60;
            dmsStfrlRingWork.alpha_spd = 0.01666667f;
        }
        if (dmsStfrlRingWork.alpha >= 1.0)
            dmsStfrlRingWork.alpha = 1f;
        else
            dmsStfrlRingWork.alpha = dmsStfrlRingWork.alpha_spd * dmsStfrlRingWork.timer;
    }

    private static void dmStfrlMdlCtrlRingProcNoDispIdle(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_RING_WORK dmsStfrlRingWork = (DMS_STFRL_RING_WORK)obj_work;
        for (int index = 0; index < 6; ++index)
            dmsStfrlRingWork.spd_y[index] += 64;
        --dmsStfrlRingWork.timer;
        if (dmsStfrlRingWork.timer <= 0)
        {
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(dmStfrlMdlCtrlRingProcStartWait);
            dmsStfrlRingWork.timer = 0;
            ++dmsStfrlRingWork.disp_ring_pos_no;
            if (dmsStfrlRingWork.disp_ring_pos_no > 12)
                dmsStfrlRingWork.disp_ring_pos_no = 0;
            ++dmsStfrlRingWork.disp_efct_pos_no;
            if (dmsStfrlRingWork.disp_efct_pos_no > 12)
                dmsStfrlRingWork.disp_efct_pos_no = 0;
            dmsStfrlRingWork.start_pos.x = dm_stfrl_ring_disp_pos_tbl[dmsStfrlRingWork.disp_ring_pos_no][0];
            dmsStfrlRingWork.start_pos.y = dm_stfrl_ring_disp_pos_tbl[dmsStfrlRingWork.disp_ring_pos_no][1];
        }
        if (dmsStfrlRingWork.alpha <= 0.0)
            dmsStfrlRingWork.alpha = 0.0f;
        else
            dmsStfrlRingWork.alpha = dmsStfrlRingWork.alpha_spd * dmsStfrlRingWork.timer;
    }

    private static void dmStfrlMdlCtrlRingDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        DMS_STFRL_RING_WORK dmsStfrlRingWork = (DMS_STFRL_RING_WORK)obj_work;
        ref VecU16 local = ref obj_work.dir;
        local.y = local.y;
        obj_work.obj_3d.draw_state.alpha.alpha = dmsStfrlRingWork.alpha;
        for (int index = 0; index < 6; ++index)
        {
            dmsStfrlRingWork.pos[index].x += dmsStfrlRingWork.spd_x[index];
            dmsStfrlRingWork.pos[index].y += dmsStfrlRingWork.spd_y[index];
            obj_work.pos.x = dmsStfrlRingWork.pos[index].x;
            obj_work.pos.y = dmsStfrlRingWork.pos[index].y;
            obj_work.pos.z = dmsStfrlRingWork.pos[index].z;
            if (index == 0)
                obj_work.disp_flag &= 4294967279U;
            else
                obj_work.disp_flag |= 16U;
            ObjDrawActionSummary(obj_work);
        }
    }

    private static void dmStfrlMdlCtrlCreateRingEfct(int pos_x, int pos_y)
    {
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(null, 50);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = pos_x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = pos_y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = -3;
    }


}