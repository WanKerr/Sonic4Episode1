using er;
using gs.backup;

public partial class AppMain
{
    private static void DmOptionStart(object arg)
    {
        dm_opt_mgr.Clear();
        dm_opt_mgr_p = dm_opt_mgr;
        dm_opt_win_tex.Clear();
        switch (SyGetEvtInfo().cur_evt_id)
        {
            case 6:
            case 11:
                dm_opt_is_pause_maingame = true;
                mtTaskStartPause(2);
                break;
            default:
                dm_opt_is_pause_maingame = false;
                break;
        }
        dm_xbox_show_progress = 0;
        dmOptInit();
    }

    private static bool DmOptionIsExit()
    {
        return dm_opt_mgr_p == null || dm_opt_mgr_p.tcb == null;
    }

    private static void dmOptInit()
    {
        dm_opt_mgr_p.tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(dmOptProcMain), new GSF_TASK_PROCEDURE(dmOptDest), 0U, (ushort)short.MaxValue, 8192U, 10, () => new DMS_OPT_MAIN_WORK(), "OPT_MAIN");
        DMS_OPT_MAIN_WORK work = (DMS_OPT_MAIN_WORK)dm_opt_mgr_p.tcb.work;
        if (dm_opt_is_pause_maingame)
        {
            work.draw_state = AoActSysGetDrawStateEnable() ? 1U : 0U;
            if (work.draw_state > 0U)
                dm_opt_draw_state = AoActSysGetDrawState();
        }
        else
        {
            work.draw_state = 1U;
            dm_opt_draw_state = 0U;
        }
        AoActSysSetDrawStateEnable(work.draw_state > 0U);
        if (work.draw_state > 0U)
            dm_opt_draw_state = AoActSysGetDrawState();
        dmOptSetInitDispOptionData(work);
        work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptLoadFontData);
    }

    private static void dmOptSetSaveOptionData(DMS_OPT_MAIN_WORK main_work)
    {
        SOption instance = SOption.CreateInstance();
        int volumeBgm = (int)instance.GetVolumeBgm();
        int volumeSe = (int)instance.GetVolumeSe();
        main_work.volume_data[0] = volumeBgm == 0 ? 0 : volumeBgm / 10;
        main_work.volume_data[1] = volumeSe == 0 ? 0 : volumeSe / 10;
        instance.SetVolumeBgm((uint)(main_work.volume_data[0] * 10));
        instance.SetVolumeSe((uint)(main_work.volume_data[1] * 10));
    }

    private static void dmOptSetInitDispOptionData(DMS_OPT_MAIN_WORK main_work)
    {
        main_work.is_jp_region = GeEnvGetDecideKey() == GSE_DECIDE_KEY.GSD_DECIDE_KEY_O;
        main_work.vol_icon_col.r = byte.MaxValue;
        main_work.vol_icon_col.g = byte.MaxValue;
        main_work.vol_icon_col.b = 0;
        main_work.vol_icon_col.a = byte.MaxValue;
        main_work.win_col.r = 0;
        main_work.win_col.g = 0;
        main_work.win_col.b = 0;
        main_work.win_col.a = byte.MaxValue;
        main_work.win_size_rate[0] = 0.0f;
        main_work.win_size_rate[1] = 0.0f;
        main_work.ctrl_tab_pos_x[0] = dm_opt_ctrl_nrml_disp_pos_tbl[main_work.ctrl_mode][0];
        main_work.ctrl_tab_pos_y[0] = dm_opt_ctrl_nrml_disp_pos_tbl[main_work.ctrl_mode][1];
        main_work.ctrl_tab_pos_x[1] = dm_opt_ctrl_clsc_disp_pos_tbl[main_work.ctrl_mode][0];
        main_work.ctrl_tab_pos_y[1] = dm_opt_ctrl_clsc_disp_pos_tbl[main_work.ctrl_mode][1];
        main_work.decide_menu_col.r = byte.MaxValue;
        main_work.decide_menu_col.g = byte.MaxValue;
        main_work.decide_menu_col.b = byte.MaxValue;
        main_work.decide_menu_col.a = 0;
        main_work.prev_nrml_disp_type = 2;
        main_work.top_crsr_pos_y = 250f;
        main_work.frm_update_time = 1f;
        main_work.obi_tex_pos[0] = 0.0f;
        main_work.obi_tex_pos[1] = 1120f;
    }

    private static void dmOptProcMain(MTS_TASK_TCB tcb)
    {
        DMS_OPT_MAIN_WORK work = (DMS_OPT_MAIN_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
        {
            mtTaskClearTcb(tcb);
            dm_opt_mgr_p = null;
            if (dm_opt_is_pause_maingame)
                mtTaskEndPause();
            dmOptSetNextEvt(work);
        }
        if ((work.flag & 2147483648U) > 0U && !AoAccountIsCurrentEnable())
        {
            work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcFadeOut);
            work.flag &= int.MaxValue;
            work.next_evt = 1;
            if (dm_opt_is_pause_maingame)
                IzFadeInitEasyColor(0, (ushort)short.MaxValue, IZD_FADE_DT_PRIO_DEF, 18U, 1U, 1U, 32f, true);
            else
                IzFadeInitEasy(1U, 1U, 32f);
            DmSndBgmPlayerExit();
            work.flag |= 1048576U;
            work.flag &= 4294967291U;
            work.flag &= 4294967293U;
            work.proc_input = null;
            work.win_timer = 0.0f;
        }
        if (work.proc_update != null)
            work.proc_update(work);
        if (work.proc_draw == null)
            return;
        work.proc_draw(work);
    }

    private static void dmOptDest(MTS_TASK_TCB tcb)
    {
    }

    private static void dmOptSetNextEvt(DMS_OPT_MAIN_WORK main_work)
    {
        short req_id;
        if (((int)main_work.flag & 524288) != 0)
        {
            req_id = 10;
            dm_opt_prev_evt = SyGetEvtInfo().old_evt_id;
            main_work.flag &= 4294443007U;
        }
        else
        {
            req_id = SyGetEvtInfo().old_evt_id;
            if (req_id == 10)
                req_id = dm_opt_prev_evt;
        }
        if (req_id == 3)
            req_id = 4;
        if (main_work.next_evt == 1)
            req_id = 3;
        if (dm_opt_is_pause_maingame)
            return;
        SyDecideEvt(req_id);
        SyChangeNextEvt();
    }

    private static void dmOptLoadFontData(DMS_OPT_MAIN_WORK main_work)
    {
        GsFontBuild();
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptIsLoadFontData);
    }

    private static void dmOptIsLoadFontData(DMS_OPT_MAIN_WORK main_work)
    {
        if (!GsFontIsBuilded())
            return;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptLoadRequest);
    }

    private static void dmOptLoadRequest(DMS_OPT_MAIN_WORK main_work)
    {
        main_work.arc_amb_fs[0] = amFsReadBackground("DEMO/OPTION/D_OPTION.AMB");
        main_work.arc_amb_fs[1] = amFsReadBackground(dm_opt_main_lng_amb_name_tbl[GsEnvGetLanguage()]);
        for (int index = 0; index < 4; ++index)
            main_work.arc_cmn_amb_fs[index] = amFsReadBackground(dm_opt_menu_cmn_amb_name_tbl[index]);
        main_work.arc_cmn_amb_fs[4] = amFsReadBackground(dm_opt_menu_cmn_lng_amb_name_tbl[GsEnvGetLanguage()]);
        main_work.manual_arc_amb_fs[0] = amFsReadBackground("DEMO/MANUAL/D_MANUAL.AMB");
        main_work.manual_arc_amb_fs[1] = amFsReadBackground(dm_opt_manual_file_lng_amb_name_tbl[GsEnvGetLanguage()]);
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcLoadWait);
    }

    private static void dmOptProcLoadWait(DMS_OPT_MAIN_WORK main_work)
    {
        if (dmOptIsDataLoad(main_work) <= 0)
            return;
        for (int index = 0; index < 2; ++index)
        {
            main_work.arc_amb[index] = readAMBFile(main_work.arc_amb_fs[index]);
            main_work.ama[index] = readAMAFile(amBindGet(main_work.arc_amb[index], 0));
            string sPath;
            main_work.amb[index] = readAMBFile(amBindGet(main_work.arc_amb[index], 1, out sPath));
            main_work.amb[index].dir = sPath;
            amFsClearRequest(main_work.arc_amb_fs[index]);
            main_work.arc_amb_fs[index] = null;
            AoTexBuild(main_work.tex[index], main_work.amb[index]);
            AoTexLoad(main_work.tex[index]);
        }
        for (int index = 0; index < 5; ++index)
        {
            main_work.arc_cmn_amb[index] = readAMBFile(main_work.arc_cmn_amb_fs[index]);
            main_work.cmn_ama[index] = readAMAFile(amBindGet(main_work.arc_cmn_amb[index], 0));
            string sPath;
            main_work.cmn_amb[index] = readAMBFile(amBindGet(main_work.arc_cmn_amb[index], 1, out sPath));
            main_work.cmn_amb[index].dir = sPath;
            amFsClearRequest(main_work.arc_cmn_amb_fs[index]);
            main_work.arc_cmn_amb_fs[index] = null;
            AoTexBuild(main_work.cmn_tex[index], main_work.cmn_amb[index]);
            AoTexLoad(main_work.cmn_tex[index]);
        }
        if (!dm_opt_is_pause_maingame)
        {
            GsFontBuild();
            DmSndBgmPlayerInit();
        }
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcLoadWait2);
    }

    private static void dmOptProcLoadWait2(DMS_OPT_MAIN_WORK main_work)
    {
        if (dmOptIsTexLoad(main_work) != 1)
            return;
        for (int index = 0; index < 2; ++index)
        {
            main_work.manual_arc_amb[index] = readAMBFile(main_work.manual_arc_amb_fs[index]);
            amFsClearRequest(main_work.manual_arc_amb_fs[index]);
            main_work.manual_arc_amb_fs[index] = null;
        }
        DmManualBuild(main_work.manual_arc_amb);
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcTexBuildWait);
    }

    private static void dmOptProcTexBuildWait(DMS_OPT_MAIN_WORK main_work)
    {
        if (dmOptIsTexLoad2(main_work) != 1)
            return;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcCheckLoadingEnd);
        if (!dm_opt_is_pause_maingame)
            return;
        main_work.bgm_scb = GsSoundAssignScb(0);
        main_work.bgm_scb.flag |= 2147483648U;
        main_work.se_handle = GsSoundAllocSeHandle();
    }

    private static void dmOptProcCheckLoadingEnd(DMS_OPT_MAIN_WORK main_work)
    {
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcCreateAct);
        if (dm_opt_is_pause_maingame)
            IzFadeInitEasyColor(0, (ushort)short.MaxValue, IZD_FADE_DT_PRIO_DEF, 18U, 0U, 0U, 32f, true);
        else
            IzFadeInitEasy(0U, 0U, 32f);
    }

    private static void dmOptProcCreateAct(DMS_OPT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 102U; ++index)
        {
            A2S_AMA_HEADER ama;
            AOS_TEXTURE tex;
            if (index >= 101U)
            {
                ama = main_work.cmn_ama[4];
                tex = main_work.cmn_tex[4];
            }
            else if (index >= 100U)
            {
                ama = main_work.cmn_ama[1];
                tex = main_work.cmn_tex[1];
            }
            else if (index >= 97U)
            {
                ama = main_work.cmn_ama[0];
                tex = main_work.cmn_tex[0];
            }
            else if (1U <= index && index <= 2U)
            {
                ama = main_work.cmn_ama[3];
                tex = main_work.cmn_tex[3];
            }
            else if (index >= 69U)
            {
                ama = main_work.ama[1];
                tex = main_work.tex[1];
            }
            else
            {
                ama = main_work.ama[0];
                tex = main_work.tex[0];
            }
            AoActSetTexture(AoTexGetTexList(tex));
            main_work.act[(int)index] = AoActCreate(ama, g_dm_act_id_tbl_opt[(int)index]);
        }
        for (int index = 0; index < main_work.trg_slct.Length; ++index)
            main_work.trg_slct[index].Create(main_work.act[3]);
        int index1 = 0;
        for (int length = main_work.trg_bgm_btn.Length; index1 < length; ++index1)
        {
            int[] numArray = new int[2] { 11, 10 };
            main_work.trg_bgm_btn[index1].Create(main_work.act[numArray[index1]]);
            main_work.trg_se_btn[index1].Create(main_work.act[numArray[index1]]);
        }
        CTrgRect trgBgmSlider = main_work.trg_bgm_slider;
        trgBgmSlider.Create(42, 70, 426, 124);
        trgBgmSlider.SetMoveThreshold(30);
        CTrgRect trgSeSlider = main_work.trg_se_slider;
        trgSeSlider.Create(42, 136, 436, 180);
        trgSeSlider.SetMoveThreshold(30);
        main_work.trg_return.Create(main_work.act[2]);
        int index2 = 0;
        for (int length = main_work.trg_ctrl_btn.Length; index2 < length; ++index2)
        {
            int[] numArray = new int[2] { 23, 26 };
            main_work.trg_ctrl_btn[index2].Create(main_work.act[numArray[index2]]);
        }
        int index3 = 0;
        for (int length = main_work.ctrl_win_trg_btn.Length; index3 < length; ++index3)
        {
            int[] numArray = new int[2] { 37, 40 };
            main_work.ctrl_win_trg_btn[index3].Create(main_work.act[numArray[index3]]);
        }
        A2S_AMA_HEADER a2SAmaHeader = main_work.ama[0];
        AOS_TEXTURE aosTexture = main_work.tex[0];
        main_work.obi_pos_y = 192f;
        main_work.proc_draw = new DMS_OPT_MAIN_WORK._proc_draw_(dmOptProcActDraw);
        main_work.proc_menu_draw = new DMS_OPT_MAIN_WORK._proc_menu_draw_(dmOptTopMenuDraw);
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcFadeIn);
        main_work.flag |= 2147483648U;
        if (!dm_opt_is_pause_maingame)
            DmSndBgmPlayerPlayBgm(0);
        else
            GsSoundPlayBgm(main_work.bgm_scb, "snd_sng_menu", 32);
    }

    private static void dmOptProcFadeIn(DMS_OPT_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        main_work.proc_input = new DMS_OPT_MAIN_WORK._proc_input_(dmOptInputProcTopMenu);
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcTopMenuIdle);
    }

    private static void dmOptProcTopMenuIdle(DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        CTrgAoAction trgReturn = main_work.trg_return;
        float frame = main_work.act[1].frame;
        if (trgReturn.GetState(0U)[10] && trgReturn.GetState(0U)[1])
            frame = 2f;
        else if (trgReturn.GetState(0U)[0])
            frame = 1f;
        else if (2.0 > main_work.act[1].frame)
            frame = 0.0f;
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index = 1; index <= 2; ++index)
        {
            AoActSetFrame(main_work.act[index], frame);
            AoActUpdate(main_work.act[index], 0.0f);
        }
        if (((int)main_work.flag & 2) != 0)
        {
            main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcFadeOut);
            if (dm_opt_is_pause_maingame)
                IzFadeInitEasyColor(0, (ushort)short.MaxValue, 61439, 18U, 0U, 1U, 32f, true);
            else
                IzFadeInitEasy(0U, 1U, 32f);
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
            if (!dm_opt_is_pause_maingame)
            {
                DmSoundPlaySE("Cancel");
            }
            else
            {
                GsSoundPlaySe("Cancel", main_work.se_handle);
                GsSoundStopBgm(main_work.bgm_scb, 32);
            }
        }
        else if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcTopMenuDecideEfct);
            dmOptSetTopMenuDecideEfctData(main_work);
            if (!dm_opt_is_pause_maingame)
                DmSoundPlaySE("Ok");
            else
                GsSoundPlaySe("Ok", main_work.se_handle);
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }
        else
        {
            if (((int)main_work.flag & 64) != 0)
            {
                main_work.cur_slct_top = dmOptGetRevisedTopMenuNo(main_work.cur_slct_top, -1);
                if (!dm_opt_is_pause_maingame)
                    DmSoundPlaySE("Cursol");
                else
                    GsSoundPlaySe("Cursol", main_work.se_handle);
                main_work.flag |= 262144U;
                dmOptSetChngFocusCrsrData(main_work);
                main_work.flag &= 4294967231U;
                main_work.flag &= 4294967167U;
            }
            if (((int)main_work.flag & 128) != 0)
            {
                main_work.cur_slct_top = dmOptGetRevisedTopMenuNo(main_work.cur_slct_top, 1);
                if (!dm_opt_is_pause_maingame)
                    DmSoundPlaySE("Cursol");
                else
                    GsSoundPlaySe("Cursol", main_work.se_handle);
                main_work.flag |= 262144U;
                dmOptSetChngFocusCrsrData(main_work);
                main_work.flag &= 4294967231U;
                main_work.flag &= 4294967167U;
            }
            if (((int)main_work.flag & 262144) == 0)
                return;
            dmOptSetCtrlFocusChangeEfct(main_work);
            if (!dmOptIsCtrlFocusChangeEfctEnd(main_work))
                return;
            main_work.flag &= 4294705151U;
        }
    }

    private static void dmOptProcTopMenuDecideEfct(DMS_OPT_MAIN_WORK main_work)
    {
        if (dmOptIsTopMenuTabDecideEfctEnd(main_work))
        {
            dmOptSetNextProcFunc(main_work);
            main_work.timer = 0.0f;
        }
        else
        {
            if ((main_work.flag & 262144U) > 0U)
            {
                dmOptSetCtrlFocusChangeEfct(main_work);
                if (dmOptIsCtrlFocusChangeEfctEnd(main_work))
                    main_work.flag &= 4294705151U;
            }
            ++main_work.timer;
        }
    }

    private static void dmOptProcManualStartFadeOut(DMS_OPT_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcManualIdle);
        main_work.flag &= int.MaxValue;
        DmManualStart();
        main_work.proc_menu_draw = null;
    }

    private static void dmOptProcManualIdle(DMS_OPT_MAIN_WORK main_work)
    {
        if (!DmManualIsExit())
            return;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcManualEndFadeIn);
        main_work.proc_menu_draw = new DMS_OPT_MAIN_WORK._proc_menu_draw_(dmOptTopMenuDraw);
        main_work.flag |= 2147483648U;
        if (dm_opt_is_pause_maingame)
            IzFadeInitEasyColor(0, (ushort)short.MaxValue, 61439, 18U, 0U, 0U, 32f, true);
        else
            IzFadeInitEasy(0U, 0U, 32f);
    }

    private static void dmOptProcManualEndFadeIn(DMS_OPT_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcTopMenuIdle);
        main_work.proc_input = new DMS_OPT_MAIN_WORK._proc_input_(dmOptInputProcTopMenu);
    }

    private static void dmOptProcSetMenuInEfct(DMS_OPT_MAIN_WORK main_work)
    {
        if ((main_work.flag & 2048U) > 0U)
        {
            main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcSetMenuIdle);
            main_work.proc_input = new DMS_OPT_MAIN_WORK._proc_input_(dmOptInputProcSettingMenu);
            main_work.disp_flag |= 1U;
            main_work.flag &= 4294965247U;
        }
        else
            dmOptSetWinOpenEfct(main_work);
    }

    private static void dmOptProcSetMenuIdle(DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        CTrgAoAction trgReturn = main_work.trg_return;
        float frame = main_work.act[1].frame;
        if (trgReturn.GetState(0U)[10] && trgReturn.GetState(0U)[1])
            frame = 2f;
        else if (trgReturn.GetState(0U)[0])
            frame = 1f;
        else if (2.0 > main_work.act[1].frame)
            frame = 0.0f;
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index = 1; index <= 2; ++index)
        {
            AoActSetFrame(main_work.act[index], frame);
            AoActUpdate(main_work.act[index], 0.0f);
        }
        if ((main_work.flag & 2U) > 0U && (16777216U & main_work.flag) > 0U)
        {
            main_work.flag &= 4278190077U;
            if (!dm_opt_is_pause_maingame)
                DmSoundPlaySE("Cancel");
            else
                GsSoundPlaySe("Cancel", main_work.se_handle);
        }
        else if (((int)main_work.flag & 2) != 0)
        {
            SOption instance = SOption.CreateInstance();
            instance.SetVolumeBgm((uint)(main_work.volume_data[0] * 10));
            instance.SetVolumeSe((uint)(main_work.volume_data[1] * 10));
            GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
            switch (instance.GetControl())
            {
                case SOption.EControl.Type.VirtualPadDown:
                    mainSysInfo.game_flag &= 4294966783U;
                    mainSysInfo.game_flag |= 1U;
                    break;
                case SOption.EControl.Type.VirtualPadUp:
                    mainSysInfo.game_flag |= 512U;
                    mainSysInfo.game_flag |= 1U;
                    break;
                default:
                    mainSysInfo.game_flag &= 4294966783U;
                    mainSysInfo.game_flag &= 4294967294U;
                    break;
            }
            main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcSetMenuOutEfct);
            main_work.win_timer = 8f;
            main_work.disp_flag &= 4294967294U;
            if (!dm_opt_is_pause_maingame)
                DmSoundPlaySE("Cancel");
            else
                GsSoundPlaySe("Cancel", main_work.se_handle);
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }
        else if (((int)main_work.flag & 4) != 0)
        {
            if (main_work.cur_slct_set == 2)
                dmOptSetDefaultDataSetMenu(main_work);
            if (!dm_opt_is_pause_maingame)
                DmSoundPlaySE("Ok");
            else
                GsSoundPlaySe("Ok", main_work.se_handle);
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }
        else if ((main_work.flag & 64U) > 0U)
        {
            main_work.cur_slct_set = dmOptGetRevisedSettingMenuNo(main_work.cur_slct_set, -1);
            if (!dm_opt_is_pause_maingame)
                DmSoundPlaySE("Cursol");
            else
                GsSoundPlaySe("Cursol", main_work.se_handle);
            main_work.flag &= 4294967231U;
            main_work.flag &= 4294967167U;
        }
        else if ((main_work.flag & 128U) > 0U)
        {
            main_work.cur_slct_set = dmOptGetRevisedSettingMenuNo(main_work.cur_slct_set, 1);
            if (!dm_opt_is_pause_maingame)
                DmSoundPlaySE("Cursol");
            else
                GsSoundPlaySe("Cursol", main_work.se_handle);
            main_work.flag &= 4294967231U;
            main_work.flag &= 4294967167U;
        }
        else
        {
            dmOptSetVolPushEfct(main_work);
            if ((main_work.flag & 1024U) > 0U)
                dmOptSetDfltPushEfct(main_work);
            if ((16777216U & main_work.flag) > 0U)
            {
                if (main_work.ctrl_win_window_prgrs >= 1.0)
                    return;
                main_work.ctrl_win_window_prgrs += 0.125f;
                if (1.0 >= main_work.ctrl_win_window_prgrs)
                    return;
                main_work.ctrl_win_window_prgrs = 1f;
            }
            else
            {
                if (0.0 >= main_work.ctrl_win_window_prgrs)
                    return;
                main_work.ctrl_win_window_prgrs -= 0.125f;
                if (main_work.ctrl_win_window_prgrs >= 0.0)
                    return;
                main_work.ctrl_win_window_prgrs = 0.0f;
            }
        }
    }

    private static void dmOptProcSetMenuOutEfct(DMS_OPT_MAIN_WORK main_work)
    {
        if ((main_work.flag & 2048U) > 0U)
        {
            main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcTopMenuIdle);
            main_work.proc_input = new DMS_OPT_MAIN_WORK._proc_input_(dmOptInputProcTopMenu);
            main_work.proc_menu_draw = new DMS_OPT_MAIN_WORK._proc_menu_draw_(dmOptTopMenuDraw);
            main_work.state = 0;
            main_work.flag &= 4294965247U;
        }
        else
            dmOptSetWinCloseEfct(main_work);
    }

    private static void dmOptProcCtrlMenuIdle(DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        CTrgAoAction trgReturn = main_work.trg_return;
        float frame = main_work.act[1].frame;
        if (trgReturn.GetState(0U)[10] && trgReturn.GetState(0U)[1])
            frame = 2f;
        else if (trgReturn.GetState(0U)[0])
            frame = 1f;
        else if (2.0 > main_work.act[1].frame)
            frame = 0.0f;
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index = 1; index <= 2; ++index)
        {
            AoActSetFrame(main_work.act[index], frame);
            AoActUpdate(main_work.act[index], 0.0f);
        }
        if ((main_work.flag & 2U) <= 0U)
            return;
        main_work.proc_input = new DMS_OPT_MAIN_WORK._proc_input_(dmOptInputProcTopMenu);
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcTopMenuIdle);
        main_work.proc_menu_draw = new DMS_OPT_MAIN_WORK._proc_menu_draw_(dmOptTopMenuDraw);
        main_work.state = 0;
        main_work.top_crsr_pos_y = (float)(250.0 + main_work.cur_slct_top * 220.0);
        if (!dm_opt_is_pause_maingame)
            DmSoundPlaySE("Cancel");
        else
            GsSoundPlaySe("Cancel", main_work.se_handle);
        main_work.flag &= 4294967291U;
        main_work.flag &= 4294967293U;
    }

    private static void dmOptProcStfrlStartFadeOut(DMS_OPT_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcStfrlIdle);
        main_work.proc_draw = null;
        main_work.flag &= int.MaxValue;
        DmStaffRollStart(null);
    }

    private static void dmOptProcStfrlIdle(DMS_OPT_MAIN_WORK main_work)
    {
        if (!DmStaffRollIsExit())
            return;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcStfrlEndFadeIn);
        main_work.proc_draw = new DMS_OPT_MAIN_WORK._proc_draw_(dmOptProcActDraw);
        AoActSysSetDrawStateEnable(true);
        AoActSysSetDrawState(dm_opt_draw_state);
        main_work.flag |= 2147483648U;
        if (dm_opt_is_pause_maingame)
        {
            GsSoundPlayBgm(main_work.bgm_scb, "snd_sng_menu", 32);
            IzFadeInitEasyColor(0, (ushort)short.MaxValue, 61439, 18U, 0U, 0U, 32f, true);
        }
        else
        {
            DmSndBgmPlayerPlayBgm(0);
            IzFadeInitEasy(0U, 0U, 32f);
        }
    }

    private static void dmOptProcStfrlEndFadeIn(DMS_OPT_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        main_work.flag |= 2147483648U;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcTopMenuIdle);
        main_work.proc_input = new DMS_OPT_MAIN_WORK._proc_input_(dmOptInputProcTopMenu);
    }

    private static void dmOptProcFadeOut(DMS_OPT_MAIN_WORK main_work)
    {
        if (dm_opt_show_xboxlive && dm_xbox_show_progress > 0)
            dm_xbox_show_progress -= 20;
        if (!IzFadeIsEnd())
            return;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcStopDraw);
        main_work.proc_draw = null;
        main_work.timer = 0.0f;
        if (!dm_opt_is_pause_maingame)
            return;
        GsSoundStopBgm(main_work.bgm_scb, 0);
        GsSoundResignScb(main_work.bgm_scb);
        main_work.bgm_scb = null;
        GsSoundFreeSeHandle(main_work.se_handle);
        main_work.se_handle = null;
    }

    private static void dmOptProcStopDraw(DMS_OPT_MAIN_WORK main_work)
    {
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcDataRelease);
    }

    private static void dmOptProcDataRelease(DMS_OPT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
            AoTexRelease(main_work.tex[index]);
        for (int index = 0; index < 5; ++index)
            AoTexRelease(main_work.cmn_tex[index]);
        DmManualFlush();
        int num = dm_opt_is_pause_maingame ? 1 : 0;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcFinish);
    }

    private static void dmOptProcFinish(DMS_OPT_MAIN_WORK main_work)
    {
        if (dmOptIsTexRelease(main_work) != 1)
            return;
        if (dm_opt_show_xboxlive)
        {
            LiveFeature.endInterrupt();
            dm_opt_show_xboxlive = false;
        }
        for (int index = 0; index < main_work.trg_slct.Length; ++index)
            main_work.trg_slct[index].Release();
        for (int index = 0; index < main_work.trg_bgm_btn.Length; ++index)
            main_work.trg_bgm_btn[index].Release();
        for (int index = 0; index < main_work.trg_se_btn.Length; ++index)
            main_work.trg_se_btn[index].Release();
        main_work.trg_bgm_slider.Release();
        main_work.trg_se_slider.Release();
        main_work.trg_return.Release();
        for (int index = 0; index < main_work.trg_ctrl_btn.Length; ++index)
            main_work.trg_ctrl_btn[index].Release();
        for (int index = 0; index < main_work.ctrl_win_trg_btn.Length; ++index)
            main_work.ctrl_win_trg_btn[index].Release();
        for (int index = 0; index < 102; ++index)
        {
            if (main_work.act[index] != null)
            {
                AoActDelete(main_work.act[index]);
                main_work.act[index] = null;
            }
        }
        for (int index = 0; index < 2; ++index)
        {
            if (main_work.arc_amb[index] != null)
                main_work.arc_amb[index] = null;
        }
        for (int index = 0; index < 5; ++index)
        {
            if (main_work.arc_cmn_amb[index] != null)
                main_work.arc_cmn_amb[index] = null;
        }
        for (int index = 0; index < 2; ++index)
        {
            if (main_work.manual_arc_amb[index] != null)
                main_work.manual_arc_amb[index] = null;
        }
        DmSaveMenuStart(true, false);
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcFinishWaitSave);
    }

    private static void dmOptProcFinishWaitSave(DMS_OPT_MAIN_WORK main_work)
    {
        if (!DmSaveIsExit())
            return;
        main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcWaitFinished);
    }

    private static void dmOptProcWaitFinished(DMS_OPT_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 1048576) != 0)
        {
            if (!DmSndBgmPlayerIsTaskExit())
                return;
            main_work.flag |= 1U;
            main_work.proc_update = null;
            main_work.flag &= 4293918719U;
        }
        else
        {
            main_work.flag |= 1U;
            main_work.proc_update = null;
        }
    }

    private static void dmOptInputProcTopMenu(DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.trg_return.GetState(0U)[10] && main_work.trg_return.GetState(0U)[1] || isBackKeyPressed())
        {
            setBackKeyRequest(false);
            main_work.flag |= 2U;
        }
        else
        {
            int index = 0;
            for (int length = main_work.trg_slct.Length; index < length; ++index)
            {
                CTrgAoAction ctrgAoAction = main_work.trg_slct[index];
                if (ctrgAoAction.GetState(0U)[10] && ctrgAoAction.GetState(0U)[1])
                {
                    main_work.cur_slct_top = index;
                    main_work.flag |= 4U;
                    break;
                }
            }
        }
    }

    private static void dmOptInputProcSettingMenu(DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.trg_return.GetState(0U)[10] && main_work.trg_return.GetState(0U)[1] || isBackKeyPressed())
        {
            setBackKeyRequest(false);
            main_work.flag |= 2U;
        }
        else
        {
            if ((16777216 & (int)main_work.flag) == 0)
            {
                if (main_work.trg_ctrl_btn[0].GetState(0U)[2])
                {
                    SOption.CreateInstance().SetControl(SOption.EControl.Type.Tilt);
                    if (!dm_opt_is_pause_maingame)
                        DmSoundPlaySE("Cursol");
                    else
                        GsSoundPlaySe("Cursol", main_work.se_handle);
                }
                CTrgAoAction ctrgAoAction = main_work.trg_ctrl_btn[1];
                if (ctrgAoAction.GetState(0U)[2])
                {
                    SOption instance = SOption.CreateInstance();
                    if (instance.GetControl() == SOption.EControl.Type.Tilt)
                        instance.SetControl(SOption.EControl.Type.VirtualPadDown);
                    if (!dm_opt_is_pause_maingame)
                        DmSoundPlaySE("Cursol");
                    else
                        GsSoundPlaySe("Cursol", main_work.se_handle);
                }
                else if (ctrgAoAction.GetState(0U)[10] && ctrgAoAction.GetState(0U)[1])
                {
                    main_work.flag |= 16777216U;
                    if (!dm_opt_is_pause_maingame)
                    {
                        DmSoundPlaySE("Window");
                        return;
                    }
                    GsSoundPlaySe("Window", main_work.se_handle);
                    return;
                }
            }
            if (0.0 < main_work.ctrl_win_window_prgrs)
            {
                if (1.0 != main_work.ctrl_win_window_prgrs)
                    return;
                SOption instance = SOption.CreateInstance();
                bool flag = true;
                if (main_work.ctrl_win_trg_btn[0].GetState(0U)[2])
                    instance.SetControl(SOption.EControl.Type.VirtualPadDown);
                else if (main_work.ctrl_win_trg_btn[1].GetState(0U)[2])
                    instance.SetControl(SOption.EControl.Type.VirtualPadUp);
                else
                    flag = false;
                if (!flag)
                    return;
                if (!dm_opt_is_pause_maingame)
                    DmSoundPlaySE("Cursol");
                else
                    GsSoundPlaySe("Cursol", main_work.se_handle);
            }
            else
            {
                bool flag = false;
                bool[] flagArray = new bool[main_work.trg_bgm_btn.Length];
                int index1 = 0;
                for (int length = main_work.trg_bgm_btn.Length; index1 < length; ++index1)
                {
                    for (int index2 = 0; index2 < 2; ++index2)
                    {
                        CTrgAoAction[] ctrgAoActionArray = index2 == 0 ? main_work.trg_bgm_btn : main_work.trg_se_btn;
                        if (ctrgAoActionArray[index1].GetState(0U)[14])
                        {
                            if (ctrgAoActionArray[index1].GetState(0U)[7])
                            {
                                main_work.cur_slct_set = index2 == 0 ? 0 : 1;
                                flagArray[index1] = true;
                                break;
                            }
                        }
                        else if (ctrgAoActionArray[index1].GetState(0U)[13])
                            ctrgAoActionArray[index1].DelLock();
                    }
                    for (int index2 = 0; index2 < 2; ++index2)
                    {
                        CTrgRect ctrgRect = index2 == 0 ? main_work.trg_bgm_slider : main_work.trg_se_slider;
                        CTrgState state = ctrgRect.GetState(0U);
                        if (state[9])
                        {
                            IntPair lastMove = state.GetLastMove();
                            int moveThreshold = state.GetMoveThreshold();
                            if (lastMove.first < -moveThreshold || moveThreshold < lastMove.first)
                            {
                                main_work.cur_slct_set = index2 == 0 ? 0 : 1;
                                flagArray[lastMove.first < 0 ? 0 : 1] = true;
                                break;
                            }
                            if (lastMove.second < -moveThreshold || moveThreshold < lastMove.second)
                                ctrgRect.DelLock();
                        }
                    }
                }
                if (flag)
                {
                    if (main_work.cur_slct_set == 2)
                    {
                        main_work.flag |= 1024U;
                        main_work.efct_timer = 0.0f;
                    }
                    main_work.flag |= 4U;
                }
                else if (flagArray[0])
                {
                    switch (main_work.cur_slct_set)
                    {
                        case 0:
                            main_work.volume_data[0] = dmOptGetRevisedVolume(main_work.volume_data[0], -1);
                            main_work.push_efct_timer[1] = 12f;
                            DmSoundSetVolumeBGM(main_work.volume_data[0]);
                            break;
                        case 1:
                            main_work.volume_data[1] = dmOptGetRevisedVolume(main_work.volume_data[1], -1);
                            main_work.push_efct_timer[3] = 12f;
                            DmSoundSetVolumeSE(main_work.volume_data[1]);
                            if (!dm_opt_is_pause_maingame)
                            {
                                DmSoundPlaySE("Cursol");
                                break;
                            }
                            GsSoundPlaySe("Cursol", main_work.se_handle);
                            break;
                    }
                }
                else
                {
                    if (!flagArray[1])
                        return;
                    switch (main_work.cur_slct_set)
                    {
                        case 0:
                            main_work.volume_data[0] = dmOptGetRevisedVolume(main_work.volume_data[0], 1);
                            main_work.push_efct_timer[0] = 12f;
                            DmSoundSetVolumeBGM(main_work.volume_data[0]);
                            break;
                        case 1:
                            main_work.volume_data[1] = dmOptGetRevisedVolume(main_work.volume_data[1], 1);
                            main_work.push_efct_timer[2] = 12f;
                            DmSoundSetVolumeSE(main_work.volume_data[1]);
                            if (!dm_opt_is_pause_maingame)
                            {
                                DmSoundPlaySE("Cursol");
                                break;
                            }
                            GsSoundPlaySe("Cursol", main_work.se_handle);
                            break;
                    }
                }
            }
        }
    }

    private static void dmOptInputProcControlMenu(DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.trg_return.GetState(0U)[10] && main_work.trg_return.GetState(0U)[1] || isBackKeyPressed())
        {
            setBackKeyRequest(false);
            main_work.flag |= 2U;
        }
        else
        {
            if ((AoPad.AoPadStand() & ControllerConsts.CONFIRM) <= 0)
                return;
            main_work.flag |= 4U;
        }
    }

    private static void dmOptProcActDraw(DMS_OPT_MAIN_WORK main_work)
    {
        dmOptSetObiEfctPos(main_work);
        dmOptCommonDraw(main_work);
        dmOptCommonFixDraw(main_work);
        if (main_work.proc_menu_draw != null)
        {
            if (!dm_opt_show_xboxlive)
                main_work.proc_menu_draw(main_work);
            else if (dm_xbox_show_progress < 100 && main_work.proc_update != new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcFadeOut))
                dm_xbox_show_progress += 5;
        }
        if (dm_opt_is_pause_maingame)
        {
            if (main_work.draw_state == 0U)
                return;
            amDrawMakeTask(new TaskProc(dmOptTaskDraw), 32768, 0U);
        }
        else
            amDrawMakeTask(new TaskProc(dmOptTaskDraw), 32768, 0U);
    }

    private static void dmOptTaskDraw(AMS_TCB tcb)
    {
        AoActDrawPre();
        amDrawExecCommand(dm_opt_draw_state);
        amDrawEndScene();
    }

    private static void dmOptCommonDraw(DMS_OPT_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(4096U);
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[0]));
        AoActSortRegAction(main_work.act[97]);
        if (!dm_opt_show_xboxlive || LiveFeature.interruptMainLoop == 1)
            AoActSortRegAction(main_work.act[99]);
        AoActSortRegAction(main_work.act[98]);
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[0]));
        AoActUpdate(main_work.act[97], 1f);
        if (!dm_opt_show_xboxlive || LiveFeature.interruptMainLoop == 1)
            AoActUpdate(main_work.act[99], 0.0f);
        AoActUpdate(main_work.act[98], 0.0f);
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private static void dmOptCommonFixDraw(DMS_OPT_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(12288U);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        if (!dm_opt_show_xboxlive || LiveFeature.interruptMainLoop == 1)
        {
            for (int index = 0; index <= 0; ++index)
                AoActSortRegAction(main_work.act[index]);
        }
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        if (!dm_opt_show_xboxlive)
        {
            for (int index = 69; index <= 69; ++index)
                AoActSortRegAction(main_work.act[index]);
        }
        for (int index = 1; index <= 2; ++index)
            AoActSortRegAction(main_work.act[index]);
        AoActSortRegAction(main_work.act[101]);
        AoActSetFrame(main_work.act[69], main_work.state);
        if (main_work.is_jp_region)
            AoActSetFrame(main_work.act[100], 0.0f);
        else
            AoActSetFrame(main_work.act[100], 1f);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        for (int index = 0; index <= 0; ++index)
            AoActUpdate(main_work.act[index], 0.0f);
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        AoActUpdate(main_work.act[69], 0.0f);
        main_work.act[69].sprite.center_y += 10f;
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[4]));
        AoActUpdate(main_work.act[101], 0.0f);
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index = 1; index <= 2; ++index)
        {
            float frame = 2.0 <= main_work.act[index].frame ? 1f : 0.0f;
            AoActUpdate(main_work.act[index], frame);
        }
        main_work.trg_return.Update();
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private static void dmOptTopMenuDraw(DMS_OPT_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(8192U);
        for (int index = 0; index < 4; ++index)
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            AoActSortRegAction(main_work.act[3]);
            AoActSortRegAction(main_work.act[4]);
            AoActSortRegAction(main_work.act[5]);
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            AoActSortRegAction(main_work.act[dm_opt_top_menu_tex_tbl[index]]);
            if ((16U & main_work.flag) > 0U)
            {
                if (index == main_work.cur_slct_top)
                {
                    float frame = 2f + main_work.timer;
                    AoActSetFrame(main_work.act[3], frame);
                    AoActSetFrame(main_work.act[4], frame);
                    AoActSetFrame(main_work.act[5], frame);
                    AoActSetFrame(main_work.act[dm_opt_top_menu_tex_tbl[index]], 1f);
                }
                else
                {
                    AoActSetFrame(main_work.act[3], 0.0f);
                    AoActSetFrame(main_work.act[4], 0.0f);
                    AoActSetFrame(main_work.act[5], 0.0f);
                    AoActSetFrame(main_work.act[dm_opt_top_menu_tex_tbl[index]], 0.0f);
                }
            }
            else if (IzFadeIsExe() && !IzFadeIsEnd())
            {
                AoActSetFrame(main_work.act[3], 0.0f);
                AoActSetFrame(main_work.act[4], 0.0f);
                AoActSetFrame(main_work.act[5], 0.0f);
                AoActSetFrame(main_work.act[dm_opt_top_menu_tex_tbl[index]], 0.0f);
            }
            else if (main_work.trg_slct[index].GetState(0U)[0])
            {
                AoActSetFrame(main_work.act[3], 1f);
                AoActSetFrame(main_work.act[4], 1f);
                AoActSetFrame(main_work.act[5], 1f);
                AoActSetFrame(main_work.act[dm_opt_top_menu_tex_tbl[index]], 1f);
            }
            else
            {
                AoActSetFrame(main_work.act[3], 0.0f);
                AoActSetFrame(main_work.act[4], 0.0f);
                AoActSetFrame(main_work.act[5], 0.0f);
                AoActSetFrame(main_work.act[dm_opt_top_menu_tex_tbl[index]], 0.0f);
            }
            AoActAcmPush();
            AoActAcmInit();
            AoActAcmApplyTrans((float)(240.0 + index % 2 * 480.0), (float)(250.0 + index / 2 * 220.0), 0.0f);
            if (main_work.cur_slct_top == index && (main_work.flag & 16U) > 0U)
                AoActAcmApplyFade(main_work.decide_menu_col);
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            AoActUpdate(main_work.act[3], 0.0f);
            AoActUpdate(main_work.act[4], 0.0f);
            AoActUpdate(main_work.act[5], 0.0f);
            main_work.trg_slct[index].Update();
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            AoActUpdate(main_work.act[dm_opt_top_menu_tex_tbl[index]], 0.0f);
            main_work.act[dm_opt_top_menu_tex_tbl[index]].sprite.center_y += 7f;
            AoActAcmPop();
            AoActSortExecute();
            AoActSortDraw();
            AoActSortUnregAll();
        }
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private static void dmOptSettingMenuDraw(DMS_OPT_MAIN_WORK main_work)
    {
        AOS_ACT_COL aosActCol = new AOS_ACT_COL();
        float num = 0.0f;
        AoActSysSetDrawTaskPrio(8192U);
        AoWinSysDrawState(0, AoTexGetTexList(main_work.tex[0]), 3U, 480f, 356f, 840f * main_work.win_size_rate[0], (float)(400.0 * main_work.win_size_rate[1] * 0.899999976158142), dm_opt_draw_state);
        if (((int)main_work.disp_flag & 1) == 0)
            return;
        for (int index1 = 0; index1 < 2; ++index1)
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            if (index1 < 2)
            {
                for (uint index2 = 10; index2 <= 21U; ++index2)
                    AoActSortRegAction(main_work.act[(int)index2]);
                if (main_work.volume_data[index1] == 10)
                    AoActSortRegAction(main_work.act[6]);
                if (main_work.volume_data[index1] > 0)
                    AoActSortRegAction(main_work.act[7]);
                AoActSortRegAction(main_work.act[8]);
                AoActSortRegAction(main_work.act[9]);
            }
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            AoActSortRegAction(main_work.act[dm_opt_set_menu_tex_tbl[index1]]);
            if (index1 > 1)
            {
                AoActSortRegAction(main_work.act[78]);
                AoActSortRegAction(main_work.act[79]);
            }
            if (main_work.cur_slct_set == index1)
            {
                AoActSetFrame(main_work.act[dm_opt_set_menu_tex_tbl[index1]], 0.0f);
                aosActCol.r = aosActCol.g = aosActCol.b = aosActCol.a = byte.MaxValue;
            }
            else
            {
                aosActCol.r = aosActCol.g = aosActCol.b = byte.MaxValue;
                aosActCol.a = 60;
            }
            AoActSetFrame(main_work.act[6], 1f);
            AoActSetFrame(main_work.act[7], main_work.volume_data[index1] % 10);
            AoActSetFrame(main_work.act[8], 0.0f);
            if (index1 <= 1)
            {
                for (int index2 = 0; index2 < 10; ++index2)
                {
                    if (index2 < main_work.volume_data[index1])
                        AoActSetFrame(main_work.act[12 + index2], 0.0f);
                    else
                        AoActSetFrame(main_work.act[12 + index2], 1f);
                }
            }
            if (main_work.set_vbrt == 0)
            {
                AoActSetFrame(main_work.act[78], 0.0f);
                AoActSetFrame(main_work.act[79], 1f);
            }
            else
            {
                AoActSetFrame(main_work.act[78], 1f);
                AoActSetFrame(main_work.act[79], 0.0f);
            }
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            for (int index2 = 6; index2 <= 21; ++index2)
            {
                AoActAcmPush();
                AoActAcmInit();
                AoActAcmApplyTrans(480f + num, dm_opt_set_tab_pos_y_tbl[index1], 0.0f);
                if (main_work.push_efct_timer[2 * index1] > 0.0 && index2 == 10)
                    AoActAcmApplyColor(main_work.vol_icon_col);
                if (main_work.push_efct_timer[1 + 2 * index1] > 0.0 && index2 == 11)
                    AoActAcmApplyColor(main_work.vol_icon_col);
                AoActUpdate(main_work.act[index2], 0.0f);
                int index3;
                switch (index2)
                {
                    case 10:
                        index3 = 1;
                        break;
                    case 11:
                        index3 = 0;
                        break;
                    default:
                        index3 = -1;
                        break;
                }
                if (0 <= index3)
                {
                    CTrgAoAction[] ctrgAoActionArray;
                    switch (index1)
                    {
                        case 0:
                            ctrgAoActionArray = main_work.trg_bgm_btn;
                            break;
                        case 1:
                            ctrgAoActionArray = main_work.trg_se_btn;
                            break;
                        default:
                            ctrgAoActionArray = null;
                            break;
                    }
                    ctrgAoActionArray?[index3].Update();
                }
                AoActAcmPop();
            }
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            for (uint index2 = 74; index2 <= 79U; ++index2)
            {
                AoActAcmPush();
                AoActAcmInit();
                AoActAcmApplyTrans(480f, dm_opt_set_tab_pos_y_tbl[index1], 0.0f);
                AoActUpdate(main_work.act[(int)index2], 0.0f);
                AoActAcmPop();
            }
            AoActSortExecute();
            AoActSortDraw();
            AoActSortUnregAll();
        }
        main_work.trg_bgm_slider.Update();
        main_work.trg_se_slider.Update();
        float frame1;
        float frame2;
        if (SOption.CreateInstance().GetControl() == SOption.EControl.Type.Tilt)
        {
            frame1 = 1f;
            frame2 = 0.0f;
        }
        else
        {
            frame1 = 0.0f;
            frame2 = 1f;
        }
        for (int index = 22; index < 25; ++index)
            AoActSetFrame(main_work.act[index], frame1);
        for (int index = 80; index < 81; ++index)
            AoActSetFrame(main_work.act[index], frame1);
        for (int index = 25; index < 28; ++index)
            AoActSetFrame(main_work.act[index], frame2);
        for (int index = 81; index < 82; ++index)
            AoActSetFrame(main_work.act[index], frame2);
        AoActAcmPush();
        AoActAcmInit();
        AoActAcmApplyTrans(480f + num, dm_opt_set_tab_pos_y_tbl[2], 0.0f);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        for (int index = 22; index < 28; ++index)
            AoActUpdate(main_work.act[index], 0.0f);
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        AoActUpdate(main_work.act[76], 0.0f);
        for (int index = 80; index < 82; ++index)
            AoActUpdate(main_work.act[index], 0.0f);
        for (int index = 0; index < main_work.trg_ctrl_btn.Length; ++index)
            main_work.trg_ctrl_btn[index].Update();
        AoActAcmPop();
        AoActSortRegAction(main_work.act[76]);
        for (int index = 22; index < 28; ++index)
            AoActSortRegAction(main_work.act[index]);
        for (int index = 80; index < 82; ++index)
            AoActSortRegAction(main_work.act[index]);
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
        if (0.0 >= main_work.ctrl_win_window_prgrs)
            return;
        AoWinSysDrawState(0, AoTexGetTexList(main_work.cmn_tex[3]), 0U, 480f, 356f, 1280f * main_work.ctrl_win_window_prgrs, (float)(720.0 * main_work.ctrl_win_window_prgrs * 0.899999976158142), dm_opt_draw_state);
        if (1.0 == main_work.ctrl_win_window_prgrs)
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            for (int index = 28; index < 42; ++index)
                AoActUpdate(main_work.act[index]);
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            for (int index = 82; index < 88; ++index)
                AoActUpdate(main_work.act[index]);
            for (int index = 0; index < main_work.ctrl_win_trg_btn.Length; ++index)
                main_work.ctrl_win_trg_btn[index].Update();
            SOption.EControl.Type control = SOption.CreateInstance().GetControl();
            if (SOption.EControl.Type.VirtualPadUp == control)
            {
                AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
                for (int index = 36; index < 39; ++index)
                {
                    AOS_ACTION act = main_work.act[index];
                    AoActSetFrame(act, 1f);
                    AoActUpdate(act, 0.0f);
                }
                for (int index = 39; index < 42; ++index)
                {
                    AOS_ACTION act = main_work.act[index];
                    AoActSetFrame(act, 0.0f);
                    AoActUpdate(act, 0.0f);
                }
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSetFrame(main_work.act[84], 1f);
                AoActSetFrame(main_work.act[85], 0.0f);
                for (int index = 84; index < 86; ++index)
                    AoActUpdate(main_work.act[index], 0.0f);
            }
            else
            {
                AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
                for (int index = 36; index < 39; ++index)
                {
                    AOS_ACTION act = main_work.act[index];
                    AoActSetFrame(act, 0.0f);
                    AoActUpdate(act, 0.0f);
                }
                for (int index = 39; index < 42; ++index)
                {
                    AOS_ACTION act = main_work.act[index];
                    AoActSetFrame(act, 1f);
                    AoActUpdate(act, 0.0f);
                }
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSetFrame(main_work.act[84], 0.0f);
                AoActSetFrame(main_work.act[85], 1f);
                for (int index = 84; index < 86; ++index)
                    AoActUpdate(main_work.act[index], 0.0f);
            }
            for (int index = 28; index < 29; ++index)
            {
                AOS_ACTION aosAction = main_work.act[index];
                AoActSortRegAction(main_work.act[index]);
            }
            for (int index = 36; index < 42; ++index)
            {
                AOS_ACTION aosAction = main_work.act[index];
                AoActSortRegAction(main_work.act[index]);
            }
            for (int index = 82; index < 86; ++index)
            {
                AOS_ACTION aosAction = main_work.act[index];
                AoActSortRegAction(main_work.act[index]);
            }
            if (SOption.EControl.Type.VirtualPadUp == control)
            {
                AoActSortRegAction(main_work.act[31]);
                for (int index = 32; index < 36; ++index)
                {
                    AOS_ACTION aosAction = main_work.act[index];
                    AoActSortRegAction(main_work.act[index]);
                }
                AoActSortRegAction(main_work.act[87]);
            }
            else
            {
                for (int index = 29; index < 31; ++index)
                {
                    AOS_ACTION aosAction = main_work.act[index];
                    AoActSortRegAction(main_work.act[index]);
                }
                AoActSortRegAction(main_work.act[86]);
            }
        }
        AoActSortRegAction(main_work.act[101]);
        for (int index = 1; index < 3; ++index)
            AoActSortRegAction(main_work.act[index]);
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private static void dmOptControlMenuDraw(DMS_OPT_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(8192U);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        for (int index = 42; index <= 68; ++index)
        {
            if (index >= 52 && index <= 56)
            {
                if ((g_gs_main_sys_info.game_flag & 32U) > 0U && main_work.act[index] != null)
                    AoActSortRegAction(main_work.act[index]);
            }
            else if (index != 51 && main_work.act[index] != null)
                AoActSortRegAction(main_work.act[index]);
        }
        if (SOption.CreateInstance().GetControl() != SOption.EControl.Type.Tilt)
        {
            if (main_work.act[50] != null)
                AoActSetFrame(main_work.act[50], 0.0f);
            if (main_work.act[55] != null)
                AoActSetFrame(main_work.act[55], 0.0f);
        }
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        for (int index = 88; index <= 96; ++index)
        {
            if (index == 92)
            {
                if ((g_gs_main_sys_info.game_flag & 32U) > 0U && main_work.act[index] != null)
                    AoActSortRegAction(main_work.act[index]);
            }
            else if (main_work.act[index] != null)
                AoActSortRegAction(main_work.act[index]);
        }
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        for (int index = 42; index <= 68; ++index)
        {
            if (main_work.act[index] != null)
                AoActUpdate(main_work.act[index]);
        }
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        for (int index = 88; index <= 96; ++index)
        {
            if (main_work.act[index] != null)
                AoActUpdate(main_work.act[index]);
        }
        AoActSortExecuteFix();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private static void dmOptSetObiEfctPos(DMS_OPT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
        {
            if (main_work.obi_tex_pos[(int)index] < -1120.0)
                main_work.obi_tex_pos[(int)index] = 1120f;
            main_work.obi_tex_pos[(int)index] += -3f;
        }
    }

    private static void dmOptSetNextProcFunc(DMS_OPT_MAIN_WORK main_work)
    {
        switch (main_work.cur_slct_top)
        {
            case 0:
                main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcManualStartFadeOut);
                main_work.proc_input = null;
                main_work.state = 0;
                if (dm_opt_is_pause_maingame)
                {
                    IzFadeInitEasyColor(0, (ushort)short.MaxValue, IZD_FADE_DT_PRIO_DEF, 18U, 0U, 1U, 32f, true);
                    break;
                }
                IzFadeInitEasy(0U, 1U, 32f);
                break;
            case 1:
                main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcCtrlMenuIdle);
                main_work.proc_input = new DMS_OPT_MAIN_WORK._proc_input_(dmOptInputProcControlMenu);
                main_work.proc_menu_draw = new DMS_OPT_MAIN_WORK._proc_menu_draw_(dmOptControlMenuDraw);
                dmOptControlResetAct(main_work);
                main_work.state = 1;
                break;
            case 2:
                main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcSetMenuInEfct);
                main_work.proc_input = null;
                main_work.proc_menu_draw = new DMS_OPT_MAIN_WORK._proc_menu_draw_(dmOptSettingMenuDraw);
                main_work.state = 2;
                main_work.cur_slct_set = 0;
                if (!dm_opt_is_pause_maingame)
                    DmSoundPlaySE("Window");
                else
                    GsSoundPlaySe("Window", main_work.se_handle, 0);
                dmOptSetSaveOptionData(main_work);
                break;
            case 3:
                main_work.proc_update = new DMS_OPT_MAIN_WORK._proc_update_(dmOptProcStfrlStartFadeOut);
                main_work.proc_input = null;
                main_work.state = 0;
                if (dm_opt_is_pause_maingame)
                    IzFadeInitEasyColor(0, (ushort)short.MaxValue, IZD_FADE_DT_PRIO_DEF, 18U, 0U, 1U, 32f, true);
                else
                    IzFadeInitEasy(0U, 1U, 32f);
                if (!dm_opt_is_pause_maingame)
                {
                    DmSndBgmPlayerBgmStop();
                    break;
                }
                GsSoundStopBgm(main_work.bgm_scb, 32);
                break;
        }
    }

    private static void dmOptSetDefaultDataSetMenu(DMS_OPT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
            main_work.volume_data[(int)index] = 10;
        DmSoundSetVolumeBGM(main_work.volume_data[0]);
        DmSoundSetVolumeSE(main_work.volume_data[1]);
        main_work.set_vbrt = 0;
    }

    private static void dmOptSetTopMenuDecideEfctData(DMS_OPT_MAIN_WORK main_work)
    {
        main_work.flag |= 16U;
    }

    private static void dmOptSetTopMenuTabDecideEfct(DMS_OPT_MAIN_WORK main_work)
    {
        float num1 = main_work.decide_menu_col.a;
        if (main_work.timer <= 8.0)
        {
            float num2 = 31.875f;
            num1 += num2;
            if (num1 >= (double)byte.MaxValue)
                num1 = byte.MaxValue;
        }
        else if (main_work.timer <= 16.0)
        {
            float num2 = 31.875f;
            num1 -= num2;
            if (num1 < 0.0)
                num1 = 0.0f;
        }
        main_work.decide_menu_col.a = (byte)num1;
    }

    private static bool dmOptIsTopMenuTabDecideEfctEnd(DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.timer <= 28.0)
            return false;
        main_work.flag &= 4294967279U;
        main_work.decide_menu_col.a = 0;
        main_work.timer = 0.0f;
        return true;
    }

    private static void dmOptSetCtrlFocusChangeEfct(DMS_OPT_MAIN_WORK main_work)
    {
        float num = (main_work.dst_crsr_pos_y - main_work.src_crsr_pos_y) / 8f;
        main_work.top_crsr_pos_y += num;
    }

    private static bool dmOptIsCtrlFocusChangeEfctEnd(DMS_OPT_MAIN_WORK main_work)
    {
        float num = main_work.dst_crsr_pos_y - main_work.src_crsr_pos_y;
        if (main_work.top_crsr_pos_y >= (double)main_work.dst_crsr_pos_y && num >= 0.0)
        {
            main_work.top_crsr_pos_y = main_work.dst_crsr_pos_y;
            return true;
        }
        if (main_work.top_crsr_pos_y > (double)main_work.dst_crsr_pos_y || num > 0.0)
            return false;
        main_work.top_crsr_pos_y = main_work.dst_crsr_pos_y;
        return true;
    }

    private static void dmOptSetVolPushEfct(DMS_OPT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 4; ++index)
        {
            if (main_work.push_efct_timer[index] > 0.0)
                --main_work.push_efct_timer[index];
            else
                main_work.push_efct_timer[index] = 0.0f;
        }
    }

    private static void dmOptSetDfltPushEfct(DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.efct_timer > 10.0)
        {
            main_work.flag &= 4294966271U;
            main_work.efct_timer = 0.0f;
        }
        ++main_work.efct_timer;
    }

    private static void dmOptSetWinOpenEfct(DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.win_timer > 8.0)
        {
            main_work.flag |= 2048U;
            main_work.win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int)index] = 1f;
        }
        else
            ++main_work.win_timer;
        for (uint index = 0; index < 2U; ++index)
        {
            main_work.win_size_rate[(int)index] = main_work.win_timer == 0.0 ? 1f : main_work.win_timer / 8f;
            if (main_work.win_size_rate[(int)index] > 1.0)
                main_work.win_size_rate[(int)index] = 1f;
        }
    }

    private static void dmOptSetWinCloseEfct(DMS_OPT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
        {
            main_work.win_size_rate[(int)index] = main_work.win_timer <= 0.0 ? 0.0f : main_work.win_timer / 8f;
            if (main_work.win_size_rate[(int)index] < 0.0)
                main_work.win_size_rate[(int)index] = 0.0f;
        }
        if (main_work.win_timer < 0.0)
        {
            main_work.flag |= 2048U;
            main_work.win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int)index] = 0.0f;
        }
        else
            --main_work.win_timer;
    }

    private static void dmOptSetChngFocusCrsrData(DMS_OPT_MAIN_WORK main_work)
    {
        main_work.src_crsr_pos_y = main_work.top_crsr_pos_y;
        main_work.dst_crsr_pos_y = (float)(250.0 + main_work.cur_slct_top * 220.0);
        main_work.flag |= 262144U;
    }

    private static int dmOptIsDataLoad(DMS_OPT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!amFsIsComplete(main_work.arc_amb_fs[index]))
                return 0;
        }
        for (int index = 0; index < 5; ++index)
        {
            if (!amFsIsComplete(main_work.arc_cmn_amb_fs[index]))
                return 0;
        }
        for (int index = 0; index < 2; ++index)
        {
            if (!amFsIsComplete(main_work.manual_arc_amb_fs[index]))
                return 0;
        }
        return 1;
    }

    private static int dmOptIsTexLoad(DMS_OPT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AoTexIsLoaded(main_work.tex[index]))
                return 0;
        }
        for (int index = 0; index < 5; ++index)
        {
            if (!AoTexIsLoaded(main_work.cmn_tex[index]))
                return 0;
        }
        return !GsFontIsBuilded() || !dm_opt_is_pause_maingame && !DmSndBgmPlayerIsSndSysBuild() ? 0 : 1;
    }

    private static int dmOptIsTexLoad2(DMS_OPT_MAIN_WORK main_work)
    {
        return !DmManualBuildCheck() ? 0 : 1;
    }

    private static int dmOptIsTexRelease(DMS_OPT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AoTexIsReleased(main_work.tex[index]))
                return 0;
        }
        for (int index = 0; index < 5; ++index)
        {
            if (!AoTexIsReleased(main_work.cmn_tex[index]))
                return 0;
        }
        return !DmManualFlushCheck() ? 0 : 1;
    }

    private static int dmOptGetRevisedTopMenuNo(int idx, int diff)
    {
        int num = idx + diff;
        if (num < 0)
            num = 3;
        if (num >= 4)
            num = 0;
        return num;
    }

    private static int dmOptGetRevisedSettingMenuNo(int idx, int diff)
    {
        int num = idx + diff;
        if (num < 0)
            num = 2;
        if (num >= 3)
            num = 0;
        return num;
    }

    private static int dmOptGetRevisedVolume(int idx, int diff)
    {
        int num = idx + diff;
        if (num < 0)
            num = 0;
        if (num > 10)
            num = 10;
        return num;
    }

    private static void dmOptControlResetAct(DMS_OPT_MAIN_WORK main_work)
    {
        SResetLocalTable[][] sresetLocalTableArray = new SResetLocalTable[3][]
        {
      new SResetLocalTable[23]
      {
        new SResetLocalTable(51, 32, 0),
        new SResetLocalTable(52, 33, 0),
        new SResetLocalTable(53, 34, 0),
        new SResetLocalTable(54, 35, 0),
        new SResetLocalTable(55, 45, 0),
        new SResetLocalTable(56, 44, 0),
        new SResetLocalTable(57, 36, 0),
        new SResetLocalTable(58, 37, 0),
        new SResetLocalTable(59, 38, 0),
        new SResetLocalTable(60, 41, 0),
        new SResetLocalTable(61, 42, 0),
        new SResetLocalTable(62, 43, 0),
        new SResetLocalTable(63, 39, 0),
        new SResetLocalTable(64, 40, 0),
        new SResetLocalTable(65, -1, -1),
        new SResetLocalTable(66, 46, 0),
        new SResetLocalTable(88, 11, 1),
        new SResetLocalTable(89, 12, 1),
        new SResetLocalTable(92, 15, 1),
        new SResetLocalTable(93, 19, 1),
        new SResetLocalTable(94, 18, 1),
        new SResetLocalTable(95, 16, 1),
        new SResetLocalTable(96, 17, 1)
      },
      new SResetLocalTable[23]
      {
        new SResetLocalTable(51, 56, 0),
        new SResetLocalTable(52, 57, 0),
        new SResetLocalTable(53, 58, 0),
        new SResetLocalTable(54, 59, 0),
        new SResetLocalTable(55, 69, 0),
        new SResetLocalTable(56, -1, -1),
        new SResetLocalTable(57, 60, 0),
        new SResetLocalTable(58, 61, 0),
        new SResetLocalTable(59, 62, 0),
        new SResetLocalTable(60, 66, 0),
        new SResetLocalTable(61, 67, 0),
        new SResetLocalTable(62, 68, 0),
        new SResetLocalTable(63, 63, 0),
        new SResetLocalTable(64, 64, 0),
        new SResetLocalTable(65, 65, 0),
        new SResetLocalTable(66, -1, -1),
        new SResetLocalTable(88, 32, 1),
        new SResetLocalTable(89, 33, 1),
        new SResetLocalTable(92, 28, 1),
        new SResetLocalTable(93, -1, -1),
        new SResetLocalTable(94, 29, 1),
        new SResetLocalTable(95, 31, 1),
        new SResetLocalTable(96, 30, 1)
      },
      new SResetLocalTable[23]
      {
        new SResetLocalTable(51, 56, 0),
        new SResetLocalTable(52, 57, 0),
        new SResetLocalTable(53, 58, 0),
        new SResetLocalTable(54, 59, 0),
        new SResetLocalTable(55, 69, 0),
        new SResetLocalTable(56, -1, -1),
        new SResetLocalTable(57, 60, 0),
        new SResetLocalTable(58, 61, 0),
        new SResetLocalTable(59, 62, 0),
        new SResetLocalTable(60, 66, 0),
        new SResetLocalTable(61, 67, 0),
        new SResetLocalTable(62, 68, 0),
        new SResetLocalTable(63, 63, 0),
        new SResetLocalTable(64, 64, 0),
        new SResetLocalTable(65, 65, 0),
        new SResetLocalTable(66, -1, -1),
        new SResetLocalTable(88, 32, 1),
        new SResetLocalTable(89, 33, 1),
        new SResetLocalTable(92, 28, 1),
        new SResetLocalTable(93, -1, -1),
        new SResetLocalTable(94, 29, 1),
        new SResetLocalTable(95, 31, 1),
        new SResetLocalTable(96, 30, 1)
      }
        };
        for (int index = 0; index < sresetLocalTableArray[0].Length; ++index)
        {
            if (main_work.act[sresetLocalTableArray[0][index].act_idx] != null)
            {
                AoActDelete(main_work.act[sresetLocalTableArray[0][index].act_idx]);
                main_work.act[sresetLocalTableArray[0][index].act_idx] = null;
            }
        }
        SOption.EControl.Type control = SOption.CreateInstance().GetControl();
        for (int index = 0; index < sresetLocalTableArray[(int)control].Length; ++index)
        {
            if (0 <= sresetLocalTableArray[(int)control][index].act_id)
                main_work.act[sresetLocalTableArray[(int)control][index].act_idx] = AoActCreate(main_work.ama[sresetLocalTableArray[(int)control][index].ama_idx], (uint)sresetLocalTableArray[(int)control][index].act_id);
        }
        if (control != SOption.EControl.Type.Tilt)
            return;
        for (int index = 42; index != 69; ++index)
        {
            if (main_work.act[index] != null)
                AoActSetFrame(main_work.act[index], 0.0f);
        }
        for (int index = 88; index != 97; ++index)
        {
            if (main_work.act[index] != null)
                AoActSetFrame(main_work.act[index], 0.0f);
        }
    }
}