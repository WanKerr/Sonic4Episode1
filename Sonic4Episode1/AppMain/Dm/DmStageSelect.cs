using System;
using accel;
using er;
using gs.backup;

public partial class AppMain
{
    private void DmStgSlctStart(object arg)
    {
        this.dmStgSlctInit();
    }

    private void dmStgSlctInit()
    {
        AoActSysSetDrawStateEnable(false);
        DMS_STGSLCT_MAIN_WORK work = (DMS_STGSLCT_MAIN_WORK) MTM_TASK_MAKE_TCB(
            new GSF_TASK_PROCEDURE(this.dmStgSlctProcMain), new GSF_TASK_PROCEDURE(this.dmStgSlctDest), 0U, 0, 8192U, 0,
            () => new DMS_STGSLCT_MAIN_WORK(), "STGSLCT_MAIN").work;
        work.is_jp_region = GeEnvGetDecideKey() == GSE_DECIDE_KEY.GSD_DECIDE_KEY_O;
        this.dmStgSlctSetInitData(work);
        this.dmStgSlctSetHiScore(work);
        this.dmStgSlctSetClearInfo(work);
        this.dmStgSlctSetAnnounceMsg(work);
        work.tex_u[0] = work.tex_u[1] = work.tex_v[0] = work.tex_v[1] = 0.1f;
        work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctLoadFontData);
    }

    private void dmStgSlctSetInitData(DMS_STGSLCT_MAIN_WORK main_work)
    {
        UNREFERENCED_PARAMETER(main_work);
        switch (SyGetEvtInfo().old_evt_id)
        {
            case 3:
            case 4:
                dm_stgslct_is_stage_start = false;
                main_work.cur_game_mode = 0U;
                break;
            case 6:
            case 11:
                dm_stgslct_is_stage_start = true;
                main_work.cur_game_mode = (uint) g_gs_main_sys_info.game_mode;
                break;
            case 7:
                dm_stgslct_is_stage_start = true;
                main_work.cur_game_mode = (uint) g_gs_main_sys_info.game_mode;
                main_work.flag |= 16777216U;
                break;
            default:
                dm_stgslct_is_stage_start = false;
                main_work.cur_game_mode = 0U;
                break;
        }

        main_work.bg_fade.r = byte.MaxValue;
        main_work.bg_fade.g = byte.MaxValue;
        main_work.bg_fade.b = byte.MaxValue;
        main_work.bg_fade.a = 0;
        if (dm_stgslct_is_stage_start)
        {
            if (g_gs_main_sys_info.stage_id < 16)
            {
                main_work.cur_stage = this.dmStgSlctSetNextFocusAct(main_work, g_gs_main_sys_info.stage_id);
                main_work.cur_zone = dm_stgslct_act_zone_no_tbl[main_work.cur_stage];
                main_work.chng_zone = 7U;
                main_work.focus_disp_no = 0;
                main_work.crsr_idx = main_work.cur_stage % 4;
            }
            else if (g_gs_main_sys_info.stage_id >= 28)
            {
                main_work.cur_stage = 0;
                main_work.cur_zone = 0U;
                main_work.chng_zone = 7U;
                main_work.focus_disp_no = 0;
                main_work.crsr_idx = 0;
            }
            else if (g_gs_main_sys_info.stage_id >= 21)
            {
                if (g_gs_main_sys_info.prev_stage_id != ushort.MaxValue)
                {
                    main_work.cur_stage = this.dmStgSlctSetNextFocusAct(main_work, g_gs_main_sys_info.prev_stage_id);
                    main_work.cur_zone = dm_stgslct_act_zone_no_tbl[main_work.cur_stage];
                    main_work.chng_zone = 7U;
                    main_work.focus_disp_no = 0;
                    main_work.crsr_idx = main_work.cur_stage % 4;
                }
                else
                {
                    main_work.cur_stage =
                        (int) (dm_stgslct_zone_act_num_tbl[5][0] + (g_gs_main_sys_info.stage_id - 21));
                    main_work.cur_zone = 5U;
                    main_work.chng_zone = 7U;
                    if (main_work.cur_stage <= 20)
                    {
                        main_work.focus_disp_no = main_work.cur_stage - 17;
                        main_work.crsr_idx = 0;
                    }
                    else
                    {
                        main_work.focus_disp_no = 3;
                        main_work.crsr_idx = main_work.cur_stage - 20;
                    }
                }
            }
            else
            {
                main_work.cur_stage = 16;
                main_work.cur_zone = 4U;
                main_work.chng_zone = 7U;
                main_work.focus_disp_no = 0;
                main_work.crsr_idx = 0;
            }

            main_work.crsr_pos_y = main_work.cur_zone == 4U
                ? 320f
                : this.dm_stgslct_act_crsr_disp_y_pos_tbl[main_work.crsr_idx];
        }

        main_work.btn_l_disp_frm = 12U;
        main_work.btn_r_disp_frm = 12U;
        main_work.cur_bg_id = main_work.cur_zone;
        main_work.obi_pos[0] = 0.0f;
        main_work.obi_pos[1] = 1120f;
    }

    private void dmStgSlctSetHiScore(DMS_STGSLCT_MAIN_WORK main_work)
    {
    }

    private void dmStgSlctSetClearInfo(DMS_STGSLCT_MAIN_WORK main_work)
    {
        uint num = 0;
        SStage instance1 = SStage.CreateInstance();
        for (uint index = 0; index <= 16U; ++index)
        {
            main_work.n_sonic_hi_score[(int) index] = (int) instance1[(int) index].GetHighScore(false);
            main_work.s_sonic_hi_score[(int) index] = (int) instance1[(int) index].GetHighScore(true);
            main_work.hi_score[(int) index] =
                main_work.n_sonic_hi_score[(int) index] == 0 || main_work.s_sonic_hi_score[(int) index] == 0
                    ? (main_work.n_sonic_hi_score[(int) index] != 0 || main_work.s_sonic_hi_score[(int) index] != 0
                        ? (main_work.n_sonic_hi_score[(int) index] != 0
                            ? (main_work.s_sonic_hi_score[(int) index] != 0
                                ? main_work.n_sonic_hi_score[(int) index]
                                : main_work.n_sonic_hi_score[(int) index])
                            : main_work.s_sonic_hi_score[(int) index])
                        : main_work.n_sonic_hi_score[(int) index])
                    : (main_work.n_sonic_hi_score[(int) index] < main_work.s_sonic_hi_score[(int) index]
                        ? main_work.s_sonic_hi_score[(int) index]
                        : main_work.n_sonic_hi_score[(int) index]);
            if (main_work.hi_score[(int) index] > 1000000000)
                main_work.hi_score[(int) index] = 999999999;
            main_work.n_sonic_record_time[(int) index] = (int) instance1[(int) index].GetFastTime(false);
            main_work.s_sonic_record_time[(int) index] = (int) instance1[(int) index].GetFastTime(true);
            main_work.record_time[(int) index] =
                main_work.n_sonic_record_time[(int) index] == 0 || main_work.s_sonic_record_time[(int) index] == 0
                    ? (main_work.n_sonic_record_time[(int) index] != 0 ||
                       main_work.s_sonic_record_time[(int) index] != 0
                        ? (main_work.n_sonic_record_time[(int) index] != 0
                            ? (main_work.s_sonic_record_time[(int) index] != 0
                                ? main_work.n_sonic_record_time[(int) index]
                                : main_work.n_sonic_record_time[(int) index])
                            : main_work.s_sonic_record_time[(int) index])
                        : main_work.n_sonic_record_time[(int) index])
                    : (main_work.n_sonic_record_time[(int) index] > main_work.s_sonic_record_time[(int) index]
                        ? main_work.s_sonic_record_time[(int) index]
                        : main_work.n_sonic_record_time[(int) index]);
            if (main_work.record_time[(int) index] > 36000)
                main_work.record_time[(int) index] = 35999;
        }

        SSpecial instance2 = SSpecial.CreateInstance();
        for (uint index = 0; index < 7U; ++index)
        {
            main_work.hi_score[(int) (index + 17U)] = (int) instance2[(int) index].GetHighScore();
            if (main_work.hi_score[(int) (index + 17U)] > 1000000000)
                main_work.hi_score[(int) (index + 17U)] = 999999999;
            main_work.record_time[(int) (index + 17U)] = (int) instance2[(int) index].GetFastTime();
            if (main_work.record_time[(int) index] > 36000)
                main_work.record_time[(int) index] = 35999;
        }

        for (uint index = 0; index < 24U; ++index)
            main_work.is_clear_stage[(int) index] =
                main_work.hi_score[(int) index] != 0 || main_work.record_time[(int) index] != 0 ? 1 : 0;
        for (uint index1 = 0; index1 < 24U; ++index1)
        {
            if (index1 > 16U && main_work.is_clear_stage[(int) index1] == 0)
                main_work.is_clear_stage[(int) index1] = -1;
            switch (index1)
            {
                case 16:
                    for (uint index2 = 3; index2 < 16U; index2 += 4U)
                    {
                        if (main_work.is_clear_stage[(int) index2] != 1)
                            num = 1U;
                    }

                    if (num != 0U)
                    {
                        main_work.is_clear_stage[(int) index1] = -1;
                        break;
                    }

                    break;
                case 17:
                    main_work.is_clear_stage[(int) index1] = 1;
                    break;
                default:
                    if (index1 < 16U && (index1 + 1U) % 4U == 0U)
                    {
                        for (uint index2 = 0; index2 < 3U; ++index2)
                        {
                            if (main_work.is_clear_stage[(int) (index1 - 3U + index2)] == 0)
                                num = 1U;
                        }

                        if (num != 0U)
                        {
                            main_work.is_clear_stage[(int) index1] = -1;
                            break;
                        }

                        break;
                    }

                    break;
            }

            num = 0U;
        }

        if (main_work.is_clear_stage[3] == 1 && main_work.is_clear_stage[7] == 1 &&
            (main_work.is_clear_stage[11] == 1 && main_work.is_clear_stage[15] == 1))
            main_work.is_final_open |= 1;
        main_work.player_stock = g_gs_main_sys_info.rest_player_num;
        for (uint index = 0; index < 7U; ++index)
        {
            if (instance2[(int) index].IsGetEmerald())
            {
                main_work.is_final_open |= 2;
                main_work.get_emerald |= 1U << (int) index;
                main_work.eme_stage_no[(int) index] =
                    dm_stgslct_eme_get_act_no_tbl[(int) instance2[(int) index].GetEmeraldStage()];
            }
            else
                main_work.is_clear_stage[(int) (17U + index)] = -1;
        }
    }

    private void dmStgSlctSetAnnounceMsg(DMS_STGSLCT_MAIN_WORK main_work)
    {
        SSystem instance = SSystem.CreateInstance();
        if (!instance.IsAnnounce(SSystem.EAnnounce.OpenZoneSelect) && main_work.is_clear_stage[0] == 1)
        {
            main_work.announce_flag |= 4U;
            instance.SetAnnounce(SSystem.EAnnounce.OpenZoneSelect, true);
        }

        if (!instance.IsAnnounce(SSystem.EAnnounce.OpenZone1Boss) && main_work.is_clear_stage[0] == 1 &&
            (main_work.is_clear_stage[1] == 1 && main_work.is_clear_stage[2] == 1))
        {
            main_work.announce_flag |= 8U;
            instance.SetAnnounce(SSystem.EAnnounce.OpenZone1Boss, true);
        }

        if (!instance.IsAnnounce(SSystem.EAnnounce.OpenZone2Boss) && main_work.is_clear_stage[4] == 1 &&
            (main_work.is_clear_stage[5] == 1 && main_work.is_clear_stage[6] == 1))
        {
            main_work.announce_flag |= 16U;
            instance.SetAnnounce(SSystem.EAnnounce.OpenZone2Boss, true);
        }

        if (!instance.IsAnnounce(SSystem.EAnnounce.OpenZone3Boss) && main_work.is_clear_stage[8] == 1 &&
            (main_work.is_clear_stage[9] == 1 && main_work.is_clear_stage[10] == 1))
        {
            main_work.announce_flag |= 32U;
            instance.SetAnnounce(SSystem.EAnnounce.OpenZone3Boss, true);
        }

        if (!instance.IsAnnounce(SSystem.EAnnounce.OpenZone4Boss) && main_work.is_clear_stage[12] == 1 &&
            (main_work.is_clear_stage[13] == 1 && main_work.is_clear_stage[14] == 1))
        {
            main_work.announce_flag |= 64U;
            instance.SetAnnounce(SSystem.EAnnounce.OpenZone4Boss, true);
        }

        if (!instance.IsAnnounce(SSystem.EAnnounce.OpenFinalZone) && main_work.is_clear_stage[3] == 1 &&
            (main_work.is_clear_stage[7] == 1 && main_work.is_clear_stage[11] == 1) &&
            main_work.is_clear_stage[15] == 1)
        {
            main_work.announce_flag |= 128U;
            instance.SetAnnounce(SSystem.EAnnounce.OpenFinalZone, true);
        }

        if (!instance.IsAnnounce(SSystem.EAnnounce.OpenSuperSonic) && main_work.is_clear_stage[23] == 1)
        {
            main_work.announce_flag |= 256U;
            instance.SetAnnounce(SSystem.EAnnounce.OpenSuperSonic, true);
        }

        if (instance.IsAnnounce(SSystem.EAnnounce.OpenSpecialStage) || (main_work.is_final_open & 2) == 0)
            return;
        main_work.announce_flag |= 512U;
        instance.SetAnnounce(SSystem.EAnnounce.OpenSpecialStage, true);
    }

    private void dmStgSlctProcMain(MTS_TASK_TCB tcb)
    {
        DMS_STGSLCT_MAIN_WORK work = (DMS_STGSLCT_MAIN_WORK) tcb.work;
        if (((int) work.flag & 1) != 0)
        {
            mtTaskClearTcb(tcb);
            this.dmStgSlctSetNextEvt(work);
        }

        if (((int) work.flag & int.MinValue) != 0 && !AoAccountIsCurrentEnable())
        {
            work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcFadeOut);
            work.flag &= int.MaxValue;
            work.next_evt = 4;
            IzFadeInitEasy(1U, 1U, 32f);
            DmSndBgmPlayerExit();
            work.flag |= 33554432U;
            work.flag &= 4294967291U;
            work.flag &= 4294967293U;
            work.proc_input = null;
            work.proc_win_input = null;
            work.win_timer = 0.0f;
            work.win_cur_slct = 0;
            work.win_mode = 1;
            work.win_is_disp_cover = false;
        }

        this.dmStgSlctSetBgFadeEfct(work);
        if (work.proc_win_update != null)
            work.proc_win_update(work);
        if (work.proc_menu_update != null)
            work.proc_menu_update(work);
        if (work.proc_draw == null)
            return;
        work.proc_draw(work);
    }

    private void dmStgSlctDest(MTS_TASK_TCB tcb)
    {
    }

    private void dmStgSlctSetNextEvt(DMS_STGSLCT_MAIN_WORK main_work)
    {
        ushort curStage = (ushort) main_work.cur_stage;
        switch (main_work.next_evt)
        {
            case 0:
                g_gs_main_sys_info.stage_id = curStage;
                g_gs_main_sys_info.prev_stage_id = ushort.MaxValue;
                g_gs_main_sys_info.char_id[0] = 0;
                g_gs_main_sys_info.game_mode = (int) main_work.cur_game_mode;
                if (main_work.is_clear_stage[23] == 1)
                    g_gs_main_sys_info.game_flag |= 32U;
                else
                    g_gs_main_sys_info.game_flag &= 4294967263U;
                g_gs_main_sys_info.game_flag &= 4294967167U;
                GmMainGSInit();
                break;
            case 1:
                g_gs_main_sys_info.stage_id = (ushort) (curStage + 4U);
                g_gs_main_sys_info.prev_stage_id = ushort.MaxValue;
                if (main_work.is_clear_stage[23] == 1)
                    g_gs_main_sys_info.game_flag |= 32U;
                else
                    g_gs_main_sys_info.game_flag &= 4294967263U;
                g_gs_main_sys_info.game_mode = (int) main_work.cur_game_mode;
                g_gs_main_sys_info.game_flag &= 4294967167U;
                break;
            case 2:
                dm_stgslct_is_stage_start = true;
                if (main_work.cur_zone == 5U)
                {
                    g_gs_main_sys_info.stage_id = (ushort) (curStage + 4U);
                    g_gs_main_sys_info.prev_stage_id = ushort.MaxValue;
                }
                else
                {
                    g_gs_main_sys_info.stage_id = curStage;
                    g_gs_main_sys_info.prev_stage_id = ushort.MaxValue;
                }

                g_gs_main_sys_info.game_mode = main_work.cur_game_mode != 0U ? 1 : 0;
                break;
        }

        SyDecideEvtCase((short) main_work.next_evt);
        SyChangeNextEvt();
    }

    private void dmStgSlctLoadFontData(DMS_STGSLCT_MAIN_WORK main_work)
    {
        GsFontBuild();
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctIsLoadFontData);
    }

    private void dmStgSlctIsLoadFontData(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (!GsFontIsBuilded())
            return;
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctLoadRequest);
    }

    private void dmStgSlctLoadRequest(DMS_STGSLCT_MAIN_WORK main_work)
    {
        main_work.arc_amb_fs[0] = amFsReadBackground(DMD_STGSLCT_DATA_FILENAME);
        main_work.arc_amb_fs[1] = amFsReadBackground(dm_stgslct_main_lng_amb_name_tbl[GsEnvGetLanguage()]);
        for (int index = 0; index < 4; ++index)
            main_work.arc_cmn_amb_fs[index] = amFsReadBackground(dm_stgslct_menu_cmn_amb_name_tbl[index]);
        main_work.arc_cmn_amb_fs[4] = amFsReadBackground(dm_stgslct_menu_cmn_lng_amb_name_tbl[GsEnvGetLanguage()]);
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcLoadWait);
    }

    private void dmStgSlctProcLoadWait(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (this.dmStgSlctIsDataLoad(main_work) == 0)
            return;
        for (int index = 0; index < 2; ++index)
        {
            main_work.arc_amb[index] = readAMBFile(main_work.arc_amb_fs[index]);
            main_work.arc_amb_fs[index] = null;
            main_work.ama[index] = readAMAFile(amBindGet(main_work.arc_amb[index], 0));
            string sPath;
            main_work.amb[index] = readAMBFile(amBindGet(main_work.arc_amb[index], 1, out sPath));
            main_work.amb[index].dir = sPath;
            amFsClearRequest(main_work.arc_amb_fs[index]);
            main_work.arc_amb_fs[index] = null;
            AoTexBuild(main_work.tex[index], main_work.amb[index]);
            AoTexLoad(main_work.tex[index]);
        }

        GsFontBuild();
        DmSndBgmPlayerInit();
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcLoadWait2);
    }

    private void dmStgSlctProcLoadWait2(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (this.dmStgSlctIsTexLoad(main_work) != 1 || !DmSndBgmPlayerIsSndSysBuild())
            return;
        for (int index = 0; index < 5; ++index)
        {
            main_work.arc_cmn_amb[index] = readAMBFile(main_work.arc_cmn_amb_fs[index]);
            main_work.arc_cmn_amb_fs[index] = null;
            main_work.cmn_ama[index] = readAMAFile(amBindGet(main_work.arc_cmn_amb[index], 0));
            string sPath;
            main_work.cmn_amb[index] = readAMBFile(amBindGet(main_work.arc_cmn_amb[index], 1, out sPath));
            main_work.cmn_amb[index].dir = sPath;
            main_work.arc_cmn_amb_fs[index] = null;
            AoTexBuild(main_work.cmn_tex[index], main_work.cmn_amb[index]);
            AoTexLoad(main_work.cmn_tex[index]);
        }

        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcTexBuildWait);
    }

    private void dmStgSlctProcTexBuildWait(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (this.dmStgSlctIsTexLoad2(main_work) != 1 || !DmSndBgmPlayerIsSndSysBuild())
            return;
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcCheckLoadingEnd);
        DmSaveMenuStart(true, false);
    }

    private void dmStgSlctProcCheckLoadingEnd(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (!DmSaveIsExit())
            return;
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcCreateAct);
        DmSndBgmPlayerPlayBgm(2);
        main_work.flag |= 2147483648U;
    }

    private void dmStgSlctProcCreateAct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 116U; ++index)
        {
            A2S_AMA_HEADER ama;
            AOS_TEXTURE tex;
            if (index >= 112U)
            {
                ama = main_work.cmn_ama[4];
                tex = main_work.cmn_tex[4];
            }
            else if (104U <= index && index <= 105U)
            {
                ama = main_work.cmn_ama[3];
                tex = main_work.cmn_tex[3];
            }
            else if (106U <= index && index <= 111U)
            {
                ama = main_work.cmn_ama[3];
                tex = main_work.cmn_tex[3];
            }
            else if (index >= 96U)
            {
                ama = main_work.cmn_ama[1];
                tex = main_work.cmn_tex[1];
            }
            else if (index >= 93U)
            {
                ama = main_work.cmn_ama[0];
                tex = main_work.cmn_tex[0];
            }
            else if (index >= 88U)
            {
                ama = main_work.ama[1];
                tex = main_work.tex[1];
            }
            else if (index == 18U || index >= 72U && index <= 75U || (index == 39U || index == 63U || index == 65U))
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
            main_work.act[(int) index] = AoActCreate(ama, g_dm_act_id_tbl_stg_slct[(int) index]);
        }

        AoActUpdate(main_work.act[57], 0.0f);
        main_work.act_tab_state_move_base_pos[0] = main_work.act[57].sprite.center_x;
        main_work.act_tab_state_move_base_pos[1] = main_work.act[57].sprite.center_y;
        int index1 = 0;
        for (int index2 = arrayof(main_work.trg_zone); index1 < index2; ++index1)
            main_work.trg_zone[index1].Create(main_work.act[37]);
        int index3 = 0;
        for (int index2 = arrayof(main_work.trg_act); index3 < index2; ++index3)
            main_work.trg_act[index3].Create(main_work.act[58]);
        int index4 = 0;
        for (int index2 = arrayof(main_work.trg_act_tab); index4 < index2; ++index4)
            main_work.trg_act_tab[index4].Create(main_work.act[43 + index4]);
        int index5 = 0;
        for (int index2 = arrayof(main_work.trg_act_lr); index5 < index2; ++index5)
        {
            CTrgAoAction ctrgAoAction = main_work.trg_act_lr[index5];
            int index6 = index5 == 0 ? 49 : 50;
            ctrgAoAction.Create(main_work.act[index6]);
        }

        CTrgFlick trgActMove = main_work.trg_act_move;
        trgActMove.Create(50, 90, (int) AMD_SCREEN_2D_WIDTH - 100, 258);
        trgActMove.SetMoveThreshold(4);
        int index7 = 0;
        for (int index2 = arrayof(main_work.trg_mode); index7 < index2; ++index7)
        {
            CTrgAoAction ctrgAoAction = main_work.trg_mode[index7];
            int index6 = index7 == 0 ? 52 : 55;
            ctrgAoAction.Create(main_work.act[index6]);
        }

        main_work.trg_cancel.Create(main_work.act[105]);
        int index8 = 0;
        for (int index2 = arrayof(main_work.trg_answer); index8 < index2; ++index8)
        {
            CTrgAoAction ctrgAoAction = main_work.trg_answer[index8];
            int index6 = index8 == 0 ? 110 : 107;
            ctrgAoAction.Create(main_work.act[index6]);
        }

        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcSetDispEfctData);
    }

    private void dmStgSlctProcSetDispEfctData(DMS_STGSLCT_MAIN_WORK main_work)
    {
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num1;
        if (((int) main_work.announce_flag & 4) != 0 || ((int) main_work.announce_flag & 512) != 0 ||
            ((int) main_work.announce_flag & 128) != 0)
            main_work.flag |= 131072U;
        if (dm_stgslct_is_stage_start && ((int) main_work.flag & 131072) == 0)
        {
            main_work.proc_draw = new DMS_STGSLCT_MAIN_WORK._proc_draw_(this.dmStgSlctStageSelectDraw);
            main_work.state = 1;
        }
        else
        {
            main_work.proc_draw = new DMS_STGSLCT_MAIN_WORK._proc_draw_(this.dmStgSlctZoneSelectDraw);
            main_work.state = 0;
            main_work.cur_zone = main_work.cur_zone = uint.MaxValue;
        }

        if (main_work.state == 0)
        {
            if (main_work.is_final_open == 3)
            {
                for (uint index = 0; index < 6U; ++index)
                {
                    main_work.zone_pos[(int) index][0] = dm_stgslct_a_zone_disp_pos_tbl[(int) index][0];
                    main_work.zone_pos[(int) index][1] = dm_stgslct_a_zone_disp_pos_tbl[(int) index][1];
                }
            }
            else if ((main_work.is_final_open & 2) != 0)
            {
                for (uint index = 0; index < 6U; ++index)
                {
                    main_work.zone_pos[(int) index][0] = dm_stgslct_s_zone_disp_pos_tbl[(int) index][0];
                    main_work.zone_pos[(int) index][1] = dm_stgslct_s_zone_disp_pos_tbl[(int) index][1];
                }
            }
            else if ((main_work.is_final_open & 1) != 0)
            {
                for (uint index = 0; index < 6U; ++index)
                {
                    main_work.zone_pos[(int) index][0] = dm_stgslct_f_zone_disp_pos_tbl[(int) index][0];
                    main_work.zone_pos[(int) index][1] = dm_stgslct_f_zone_disp_pos_tbl[(int) index][1];
                }
            }
            else
            {
                for (uint index = 0; index < 6U; ++index)
                {
                    main_work.zone_pos[(int) index][0] = dm_stgslct_n_zone_disp_pos_tbl[(int) index][0];
                    main_work.zone_pos[(int) index][1] = dm_stgslct_n_zone_disp_pos_tbl[(int) index][1];
                }
            }

            for (uint index = 0; index < 24U; ++index)
            {
                main_work.act_top_pos_x[(int) index] = 1120f;
                main_work.act_top_pos_y[(int) index] = dm_stgslct_act_disp_y_pos_tbl[(int) index];
            }

            main_work.mode_tex_pos_y = 180f;
            main_work.chaos_eme_pos_y = 0.0f;
            main_work.mode_tex_move_frm = 0U;
        }
        else
        {
            if (main_work.is_final_open == 3)
            {
                for (uint index = 0; index < 6U; ++index)
                {
                    main_work.zone_pos[(int) index][0] = dm_stgslct_a_zone_nodisp_pos_tbl[(int) index][0];
                    main_work.zone_pos[(int) index][1] = dm_stgslct_a_zone_nodisp_pos_tbl[(int) index][1];
                }
            }
            else if ((main_work.is_final_open & 2) != 0)
            {
                for (uint index = 0; index < 6U; ++index)
                {
                    main_work.zone_pos[(int) index][0] = dm_stgslct_s_zone_nodisp_pos_tbl[(int) index][0];
                    main_work.zone_pos[(int) index][1] = dm_stgslct_s_zone_nodisp_pos_tbl[(int) index][1];
                }
            }
            else if ((main_work.is_final_open & 1) != 0)
            {
                for (uint index = 0; index < 6U; ++index)
                {
                    main_work.zone_pos[(int) index][0] = dm_stgslct_f_zone_nodisp_pos_tbl[(int) index][0];
                    main_work.zone_pos[(int) index][1] = dm_stgslct_f_zone_nodisp_pos_tbl[(int) index][1];
                }
            }
            else
            {
                for (uint index = 0; index < 6U; ++index)
                {
                    main_work.zone_pos[(int) index][0] = dm_stgslct_n_zone_nodisp_pos_tbl[(int) index][0];
                    main_work.zone_pos[(int) index][1] = dm_stgslct_n_zone_nodisp_pos_tbl[(int) index][1];
                }
            }

            for (uint index = num1; index < num2; ++index)
            {
                main_work.act_top_pos_x[(int) index] = 100f;
                main_work.act_top_pos_y[(int) index] = main_work.cur_zone == 5U
                    ? this.dm_stgslct_act_tab_disp_y_pos_tbl[main_work.focus_disp_no]
                    : dm_stgslct_act_disp_y_pos_tbl[(int) index];
            }

            this.dmStgSlctStageSelectChngZoneSetInZoneScroll(main_work, main_work.cur_stage);
            main_work.mode_tex_pos_y = 0.0f;
            main_work.chaos_eme_pos_y = 180f;
            main_work.mode_tex_move_frm = 5U;
        }

        main_work.crsr_pos_y = dm_stgslct_act_disp_y_pos_tbl[0];
        main_work.proc_draw = new DMS_STGSLCT_MAIN_WORK._proc_draw_(this.dmStgSlctProcActDraw);
        IzFadeInitEasy(0U, 0U, 32f);
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcFadeIn);
    }

    private void dmStgSlctProcFadeIn(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        AoPresenceSet(AO_PRESENCE_STAGE_ID_STAGE_SELECT, false);
        if (((int) main_work.flag & 131072) != 0)
        {
            IzFadeInitEasy(0U, 3U, 32f);
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcSetWhiteFlashEfct);
        }
        else
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcSetSlctStartData);
    }

    private void dmStgSlctProcSetWhiteFlashEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeInitEasy(0U, 2U, 32f);
        main_work.flag &= 4294836223U;
        main_work.proc_menu_update =
            new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcIsWhiteFlashEfctEnd);
    }

    private void dmStgSlctProcIsWhiteFlashEfctEnd(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcSetSlctStartData);
    }

    private void dmStgSlctProcSetSlctStartData(DMS_STGSLCT_MAIN_WORK main_work)
    {
        main_work.proc_win_update = new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowNodispIdle);
        if (main_work.state == 0)
        {
            main_work.proc_input = new DMS_STGSLCT_MAIN_WORK._proc_input_(this.dmStgSlctInputProcZoneSelect);
            main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcZoneSelectIdle);
        }
        else
        {
            main_work.proc_input = new DMS_STGSLCT_MAIN_WORK._proc_input_(this.dmStgSlctInputProcStageSelect);
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStageSelectIdle);
            main_work.disp_flag |= 2U;
        }

        main_work.zone_scr_id = 0U;
    }

    private void dmStgSlctProcZoneSelectIdle(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        CTrgAoAction trgCancel = main_work.trg_cancel;
        float frame = main_work.act[104].frame;
        if (trgCancel.GetState(0U)[10] && trgCancel.GetState(0U)[1])
            frame = 2f;
        else if (trgCancel.GetState(0U)[0])
            frame = 1f;
        else if (2.0 > frame)
            frame = 0.0f;
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index = 104; index <= 105; ++index)
        {
            AoActSetFrame(main_work.act[index], frame);
            AoActUpdate(main_work.act[index], 0.0f);
        }

        if (((int) main_work.flag & 2) != 0)
        {
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
            main_work.proc_win_input = null;
            main_work.proc_input = null;
            main_work.proc_win_update = null;
            main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcFadeOut);
            main_work.next_evt = 3;
            IzFadeInitEasy(0U, 1U, 32f);
            DmSoundPlaySE("Cancel");
        }
        else if (((int) main_work.flag & 4) != 0)
        {
            DmSoundPlaySE("Ok");
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcZoneSelectDecideEfct);
            main_work.flag |= 262144U;
            main_work.timer = 0;
        }
        else
            this.dmStgSlctSetZoneScrChangeEfct(main_work);
    }

    private void dmStgSlctProcZoneSelectDecideEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        ++main_work.timer;
        this.dmStgSlctSetDecideZoneEfctPos(main_work);
        if (this.dmStgSlctIsDecideZoneEfctPos(main_work))
        {
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcZoneSelectOutEfct);
            main_work.flag &= 4294967291U;
            for (uint index = 0; index < 6U; ++index)
            {
                if ((int) main_work.cur_zone != (int) index)
                    main_work.efct_out_flag |= 1U << (int) index;
            }

            main_work.decide_zone_efct_dist_x = 0;
            main_work.decide_zone_efct_dist_y = 0;
            main_work.cur_stage = (int) dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
            main_work.prev_stage = main_work.cur_stage;
            main_work.proc_input = null;
            main_work.timer = 0;
            main_work.trg_act_move.ResetState();
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }
        else
            this.dmStgSlctSetZoneScrChangeEfct(main_work);
    }

    private void dmStgSlctProcZoneSelectOutEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num1;
        if (this.dmStgSlctIsZonePosOutEfct(main_work))
        {
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStageSelectInEfct);
            main_work.efct_time = 0U;
            for (uint index = 0; index < 6U; ++index)
            {
                if ((int) main_work.cur_zone != (int) index)
                {
                    main_work.zone_pos[(int) index][0] = dm_stgslct_a_zone_nodisp_pos_tbl[(int) index][0];
                    main_work.zone_pos[(int) index][1] = dm_stgslct_a_zone_nodisp_pos_tbl[(int) index][1];
                }
            }

            for (uint index = num1; index < num2; ++index)
            {
                main_work.act_top_pos_x[(int) index] = 1120f;
                main_work.act_top_pos_y[(int) index] = main_work.cur_zone == 5U
                    ? this.dm_stgslct_act_tab_disp_y_pos_tbl[0]
                    : dm_stgslct_act_disp_y_pos_tbl[(int) index];
            }

            main_work.state = 1;
            main_work.mode_tex_move_frm = 0U;
            main_work.is_disp_cover = false;
            this.dmStgSlctStageSelectChngZoneSetInZoneScroll(main_work);
        }
        else
        {
            this.dmStgSlctSetZonePosOutEfct(main_work);
            if (main_work.efct_time == 20U)
                main_work.efct_out_flag |= 1U << (int) main_work.cur_zone;
            ++main_work.efct_time;
            this.dmStgSlctSetZoneScrChangeEfct(main_work);
        }
    }

    private void dmStgSlctProcStageSelectInEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        this.dmStgSlctSetStagePosInEfct(main_work);
        if (this.dmStgSlctIsStagePosInEfct(main_work))
        {
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStageSelectIdle);
            if (main_work.proc_win_update ==
                new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowNodispIdle))
                main_work.proc_input = new DMS_STGSLCT_MAIN_WORK._proc_input_(this.dmStgSlctInputProcStageSelect);
            main_work.disp_flag |= 2U;
            main_work.crsr_idx = 0;
            if (main_work.cur_zone != 4U)
                main_work.crsr_pos_y = this.dm_stgslct_act_crsr_disp_y_pos_tbl[main_work.crsr_idx];
            else
                main_work.crsr_pos_y = 320f;
        }
        else
        {
            ++main_work.mode_tex_move_frm;
            if (main_work.mode_tex_move_frm <= 5U)
                return;
            main_work.mode_tex_move_frm = 5U;
        }
    }

    private void dmStgSlctProcStageSelectIdle(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        CTrgAoAction trgCancel = main_work.trg_cancel;
        float frame = main_work.act[104].frame;
        if (!main_work.trg_act_move.GetState(0U)[15] && trgCancel.GetState(0U)[14])
            frame = !trgCancel.GetState(0U)[10] || !trgCancel.GetState(0U)[1]
                ? (!trgCancel.GetState(0U)[0] ? 0.0f : 1f)
                : 2f;
        else if (2.0 > frame)
            frame = 0.0f;
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index = 104; index <= 105; ++index)
        {
            AoActSetFrame(main_work.act[index], frame);
            AoActUpdate(main_work.act[index], 0.0f);
        }

        if (((int) main_work.flag & 2) != 0)
        {
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStageSelectOutEfct);
            DmSoundPlaySE("Cancel");
            main_work.flag &= 4294967293U;
            main_work.proc_input = null;
            main_work.disp_flag &= 4294967293U;
        }
        else if (((int) main_work.flag & 4) != 0)
        {
            if (!this.dmStgSlctIsCanSelectAct(main_work))
            {
                main_work.flag &= 4294967291U;
                main_work.is_disp_cover = false;
            }
            else
            {
                DmSoundPlaySE("Ok");
                main_work.flag &= 4294967291U;
                main_work.proc_win_update =
                    new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowOpenEfct);
                main_work.proc_input = null;
                main_work.proc_win_input = null;
                main_work.win_timer = 0.0f;
                main_work.win_cur_slct = 0;
                main_work.win_mode = 1;
                main_work.win_is_disp_cover = false;
                main_work.win_timer = -10.5f;
            }
        }
        else if (((int) main_work.flag & 524288) != 0)
        {
            main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcFadeOut);
            IzFadeInitEasy(0U, 1U, 32f);
            main_work.next_evt = 2;
            DmSoundPlaySE("Ok");
            main_work.proc_input = null;
            main_work.proc_win_input = null;
            main_work.win_timer = 0.0f;
            main_work.win_cur_slct = 0;
            main_work.flag &= 4294443007U;
        }
        else if (((int) main_work.flag & 32) != 0)
        {
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStageSelectChngZone);
            main_work.proc_input = new DMS_STGSLCT_MAIN_WORK._proc_input_(this.dmStgSlctInputProcStageSelectChngZone);
            DmSoundPlaySE("Cursol");
            main_work.timer = 0;
            this.dmStgSlctStageSelectChngZoneSetInZoneScroll(main_work);
        }
        else
        {
            if (((int) main_work.flag & 64) == 0)
                return;
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStageSelectChngVrtclAct);
            main_work.proc_input = new DMS_STGSLCT_MAIN_WORK._proc_input_(this.dmStgSlctInputProcStageSelectMove);
            this.dmStgSlctSetFocusChangeEfctData(main_work);
            main_work.timer = 0;
        }
    }

    private void dmStgSlctProcStageSelectChngZone(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (this.dmStgSlctIsStageZoneChangeEfct(main_work))
        {
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStageSelectIdle);
            if (main_work.proc_win_update ==
                new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowNodispIdle))
                main_work.proc_input = new DMS_STGSLCT_MAIN_WORK._proc_input_(this.dmStgSlctInputProcStageSelect);
            main_work.flag &= 4294967263U;
        }
        else
            this.dmStgSlctSetStageZoneChangeEfct(main_work);
    }

    private void dmStgSlctProcStageSelectChngVrtclAct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (((int) main_work.flag & 2048) != 0 || ((int) main_work.flag & 4096) != 0)
        {
            this.dmStgSlctSetFocusChangeEfctData(main_work);
            DmSoundPlaySE("Cursol");
            main_work.timer = 0;
            main_work.flag &= 4294965247U;
            main_work.flag &= 4294963199U;
        }
        else if (((int) main_work.flag & 64) == 0 && ((int) main_work.flag & 128) == 0)
        {
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStageSelectIdle);
            if (main_work.proc_win_update ==
                new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowNodispIdle))
                main_work.proc_input = new DMS_STGSLCT_MAIN_WORK._proc_input_(this.dmStgSlctInputProcStageSelect);
            main_work.timer = 0;
        }
        else
        {
            if (((int) main_work.flag & 64) != 0)
            {
                this.dmStgSlctSetStageVrtclChangeEfct(main_work);
                if (this.dmStgSlctIsStageVrtclChangeEfct(main_work))
                    main_work.flag &= 4294967231U;
            }

            if (((int) main_work.flag & 128) != 0)
            {
                this.dmStgSlctSetStageCrsrChangeEfct(main_work);
                if (this.dmStgSlctIsStageCrsrChangeEfct(main_work))
                    main_work.flag &= 4294967167U;
            }

            ++main_work.timer;
        }
    }

    private void dmStgSlctProcStageSelectOutEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (this.dmStgSlctIsStagePosOutEfct(main_work))
        {
            main_work.proc_menu_update =
                new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcZoneSelectInEfct);
            main_work.state = 0;
            for (int index = 0; index < 6; ++index)
            {
                if (main_work.is_final_open == 3)
                {
                    main_work.zone_pos[index][0] = dm_stgslct_a_zone_nodisp_pos_tbl[index][0];
                    main_work.zone_pos[index][1] = dm_stgslct_a_zone_nodisp_pos_tbl[index][1];
                }
                else if ((main_work.is_final_open & 2) != 0)
                {
                    main_work.zone_pos[index][0] = dm_stgslct_s_zone_nodisp_pos_tbl[index][0];
                    main_work.zone_pos[index][1] = dm_stgslct_s_zone_nodisp_pos_tbl[index][1];
                }
                else if ((main_work.is_final_open & 1) != 0)
                {
                    main_work.zone_pos[index][0] = dm_stgslct_f_zone_nodisp_pos_tbl[index][0];
                    main_work.zone_pos[index][1] = dm_stgslct_f_zone_nodisp_pos_tbl[index][1];
                }
                else
                {
                    main_work.zone_pos[index][0] = dm_stgslct_n_zone_nodisp_pos_tbl[index][0];
                    main_work.zone_pos[index][1] = dm_stgslct_n_zone_nodisp_pos_tbl[index][1];
                }
            }

            main_work.mode_tex_move_frm = 0U;
            main_work.zone_scr_id = 0U;
            main_work.is_disp_cover = false;
        }
        else
        {
            this.dmStgSlctSetStagePosOutEfct(main_work);
            ++main_work.mode_tex_move_frm;
            if (main_work.mode_tex_move_frm <= 10U)
                return;
            main_work.mode_tex_move_frm = 10U;
        }
    }

    private void dmStgSlctProcZoneSelectInEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (this.dmStgSlctIsZonePosInEfct(main_work))
        {
            main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcZoneSelectIdle);
            for (int index1 = 0; index1 < 6; ++index1)
            {
                if (main_work.is_final_open == 3)
                {
                    for (uint index2 = 0; index2 < 6U; ++index2)
                    {
                        main_work.zone_pos[(int) index2][0] = dm_stgslct_a_zone_disp_pos_tbl[(int) index2][0];
                        main_work.zone_pos[(int) index2][1] = dm_stgslct_a_zone_disp_pos_tbl[(int) index2][1];
                    }
                }
                else if ((main_work.is_final_open & 2) != 0)
                {
                    for (uint index2 = 0; index2 < 6U; ++index2)
                    {
                        main_work.zone_pos[(int) index2][0] = dm_stgslct_s_zone_disp_pos_tbl[(int) index2][0];
                        main_work.zone_pos[(int) index2][1] = dm_stgslct_s_zone_disp_pos_tbl[(int) index2][1];
                    }
                }
                else if ((main_work.is_final_open & 1) != 0)
                {
                    for (uint index2 = 0; index2 < 6U; ++index2)
                    {
                        main_work.zone_pos[(int) index2][0] = dm_stgslct_f_zone_disp_pos_tbl[(int) index2][0];
                        main_work.zone_pos[(int) index2][1] = dm_stgslct_f_zone_disp_pos_tbl[(int) index2][1];
                    }
                }
                else
                {
                    for (uint index2 = 0; index2 < 6U; ++index2)
                    {
                        main_work.zone_pos[(int) index2][0] = dm_stgslct_n_zone_disp_pos_tbl[(int) index2][0];
                        main_work.zone_pos[(int) index2][1] = dm_stgslct_n_zone_disp_pos_tbl[(int) index2][1];
                    }
                }
            }

            if (!(main_work.proc_win_update ==
                  new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowNodispIdle)))
                return;
            main_work.proc_input = new DMS_STGSLCT_MAIN_WORK._proc_input_(this.dmStgSlctInputProcZoneSelect);
        }
        else
        {
            this.dmStgSlctSetZonePosInEfct(main_work);
            main_work.cur_zone = uint.MaxValue;
        }
    }

    private void dmStgSlctProcWindowNodispIdle(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.proc_win_input != null)
            main_work.proc_win_input(main_work);
        if (((int) main_work.flag & 8) == 0 && main_work.announce_flag == 0U)
            return;
        main_work.proc_win_update = new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowOpenEfct);
        main_work.proc_input = null;
        main_work.proc_win_input = null;
        main_work.win_timer = 0.0f;
        for (uint index = 2; index < 10U; ++index)
        {
            if ((main_work.announce_flag & (uint)(1 << (int)index)) != 0)
            {
                main_work.win_mode = (int) index;
                break;
            }
        }

        main_work.flag &= 4294967287U;
        DmSoundPlaySE("Window");
    }

    private void dmStgSlctProcWindowOpenEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (((int) main_work.flag & 16) != 0)
        {
            main_work.proc_win_update =
                new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowAnnounceIdle);
            main_work.proc_win_input = new DMS_STGSLCT_MAIN_WORK._proc_win_input_(this.dmStgSlctInputProcWinDispIdle);
            ArrayPointer<AOS_ACTION> arrayPointer1 = new ArrayPointer<AOS_ACTION>(main_work.act, 106);
            ArrayPointer<AOS_ACTION> arrayPointer2 = new ArrayPointer<AOS_ACTION>(main_work.act, 112);
            while (arrayPointer1 != arrayPointer2)
            {
                AoActSetFrame(~arrayPointer1, 0.0f);
                ++arrayPointer1;
            }

            int index1 = 0;
            for (int index2 = arrayof(main_work.trg_answer); index1 < index2; ++index1)
                main_work.trg_answer[index1].ResetState();
            main_work.disp_flag |= 1U;
            main_work.flag &= 4294967279U;
        }
        else
            this.dmStgSlctSetWinOpenEfct(main_work);

        this.dmStgSlctWinSelectDraw(main_work);
    }

    private void dmStgSlctProcWindowAnnounceIdle(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.proc_win_input != null)
            main_work.proc_win_input(main_work);
        if (main_work.win_mode == 0)
        {
            if (((int) main_work.flag & 4) != 0)
            {
                main_work.proc_input = null;
                main_work.proc_win_input = null;
                main_work.win_timer = 8f;
                main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcFadeOut);
                IzFadeInitEasy(0U, 1U, 32f);
                if (main_work.win_cur_slct == 0)
                {
                    main_work.next_evt = 2;
                    DmSoundPlaySE("Ok");
                }
                else
                {
                    main_work.next_evt = 3;
                    DmSoundPlaySE("Ok");
                }

                main_work.flag &= 4294967291U;
                main_work.flag &= 4294967293U;
            }
            else if (((int) main_work.flag & 2) != 0)
            {
                main_work.proc_input = null;
                main_work.proc_win_input = null;
                main_work.win_timer = 8f;
                main_work.disp_flag &= 4294967294U;
                main_work.proc_win_update =
                    new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowCloseEfct);
                DmSoundPlaySE("Cancel");
                main_work.flag &= 4294967291U;
                main_work.flag &= 4294967293U;
            }
        }
        else if (main_work.win_mode != 1)
        {
            if (((int) main_work.flag & 4) != 0 || ((int) main_work.flag & 2) != 0)
            {
                main_work.proc_input = null;
                main_work.proc_win_input = null;
                main_work.win_timer = 8f;
                main_work.disp_flag &= 4294967294U;
                main_work.proc_win_update =
                    new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowCloseEfct);
                DmSoundPlaySE("Ok");
                main_work.flag &= 4294967291U;
                main_work.flag &= 4294967293U;
            }
        }
        else if (((int) main_work.flag & 4) != 0 && main_work.win_cur_slct == 0)
        {
            main_work.proc_input = null;
            main_work.proc_win_input = null;
            main_work.win_timer = 8f;
            main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcFadeOut);
            IzFadeInitEasy(0U, 1U, 32f);
            main_work.next_evt = main_work.cur_zone != 5U ? 0 : 1;
            DmSndBgmPlayerExit();
            main_work.flag |= 33554432U;
            DmSoundPlaySE("Ok");
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }
        else if (((int) main_work.flag & 4) != 0 || ((int) main_work.flag & 2) != 0)
        {
            main_work.proc_input = null;
            main_work.proc_win_input = null;
            main_work.win_timer = 8f;
            main_work.disp_flag &= 4294967294U;
            main_work.proc_win_update = new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowCloseEfct);
            if (((int) main_work.flag & 2) != 0)
                DmSoundPlaySE("Cancel");
            else
                DmSoundPlaySE("Ok");
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }

        this.dmStgSlctWinSelectDraw(main_work);
    }

    private void dmStgSlctProcWindowCloseEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (((int) main_work.flag & 16) != 0)
        {
            main_work.proc_win_update = new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowNodispIdle);
            main_work.announce_flag &= (uint) ~(1 << main_work.win_mode);
            if (main_work.announce_flag == 0U)
            {
                if (main_work.state == 0)
                    main_work.proc_input = new DMS_STGSLCT_MAIN_WORK._proc_input_(this.dmStgSlctInputProcZoneSelect);
                else if (main_work.state == 1)
                {
                    main_work.proc_input = new DMS_STGSLCT_MAIN_WORK._proc_input_(this.dmStgSlctInputProcStageSelect);
                    main_work.is_disp_cover = false;
                }
            }

            main_work.flag &= 4294967279U;
        }

        this.dmStgSlctSetWinCloseEfct(main_work);
        this.dmStgSlctWinSelectDraw(main_work);
    }

    private void dmStgSlctProcFadeOut(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        main_work.proc_win_update = null;
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStopDraw);
        main_work.proc_draw = null;
        main_work.timer = 0;
    }

    private void dmStgSlctProcStopDraw(DMS_STGSLCT_MAIN_WORK main_work)
    {
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcDataRelease);
    }

    private void dmStgSlctProcDataRelease(DMS_STGSLCT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
            AoTexRelease(main_work.tex[index]);
        for (int index = 0; index < 5; ++index)
            AoTexRelease(main_work.cmn_tex[index]);
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcFinish);
    }

    private void dmStgSlctProcFinish(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (this.dmStgSlctIsTexRelease(main_work) != 1)
            return;
        for (int index = 0; index < main_work.trg_zone.Length; ++index)
            main_work.trg_zone[index].Release();
        for (int index = 0; index < main_work.trg_act.Length; ++index)
            main_work.trg_act[index].Release();
        for (int index = 0; index < main_work.trg_act_tab.Length; ++index)
            main_work.trg_act_tab[index].Release();
        for (int index = 0; index < main_work.trg_act_lr.Length; ++index)
            main_work.trg_act_lr[index].Release();
        main_work.trg_act_move.Release();
        for (int index = 0; index < main_work.trg_mode.Length; ++index)
            main_work.trg_mode[index].Release();
        main_work.trg_cancel.Release();
        for (int index = 0; index < main_work.trg_answer.Length; ++index)
            main_work.trg_answer[index].Release();
        for (int index = 0; index < 116; ++index)
        {
            if (main_work.act[index] != null)
            {
                AoActDelete(main_work.act[index]);
                main_work.act[index] = null;
            }
        }

        for (int index = 0; index < 2; ++index)
            main_work.arc_amb[index] = null;
        for (int index = 0; index < 5; ++index)
            main_work.arc_cmn_amb[index] = null;
        main_work.proc_win_update = null;
        main_work.proc_menu_update = new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcWaitFinished);
    }

    private void dmStgSlctProcWaitFinished(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (((int) main_work.flag & 33554432) != 0)
        {
            if (!DmSndBgmPlayerIsTaskExit())
                return;
            main_work.flag |= 1U;
            main_work.proc_win_update = null;
            main_work.proc_menu_update = null;
            main_work.flag &= 4261412863U;
        }
        else
        {
            main_work.flag |= 1U;
            main_work.proc_win_update = null;
            main_work.proc_menu_update = null;
        }
    }

    private void dmStgSlctInputProcZoneSelect(DMS_STGSLCT_MAIN_WORK main_work)
    {
        uint curZone = main_work.cur_zone;
        var pad = AoPad.AoPadMStand();

        if (main_work.trg_cancel.GetState(0U)[10] && main_work.trg_cancel.GetState(0U)[1] ||
            pad.HasFlag(ControllerConsts.B) || isBackKeyPressed())
        {
            main_work.flag |= 2U;
            setBackKeyRequest(false);
        }
        else
        {
            for (int index = 0; index < main_work.trg_zone.Length; ++index)
            {
                int[] numArray = new int[6] {0, 1, 2, 3, 4, 5};
                CTrgAoAction ctrgAoAction = main_work.trg_zone[index];
                //ctrgAoAction.IsSelected = index == main_work.cur_zone;
                if (ctrgAoAction.GetState(0U)[10] && ctrgAoAction.GetState(0U)[1])
                {
                    main_work.cur_zone = (uint) numArray[index];
                    main_work.flag |= 4U;
                    break;
                }

                if (ctrgAoAction.GetState(0U)[2])
                {
                    main_work.cur_zone = (uint) numArray[index];
                    main_work.is_disp_cover = true;
                    break;
                }

                if (ctrgAoAction.GetState(0U)[13])
                {
                    main_work.cur_zone = uint.MaxValue;
                    main_work.is_disp_cover = false;
                }
            }

            if (main_work.cur_zone != 7U)
                return;
            main_work.cur_zone = curZone;
        }
    }

    private void dmStgSlctInputProcStageSelect(DMS_STGSLCT_MAIN_WORK main_work)
    {
        uint num1 = 0;
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num3 = num2 + dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
        int num4 = (int) dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
        if (main_work.is_clear_stage[3] == 1 && main_work.is_clear_stage[7] == 1 && main_work.is_clear_stage[11] == 1)
        {
            int num5 = main_work.is_clear_stage[15];
        }

        uint num6 = 17;
        while (num6 < 24U && main_work.is_clear_stage[(int) num6] == -1)
            ++num6;
        if (isBackKeyPressed() || !main_work.trg_act_move.GetState(0U)[15] && main_work.trg_cancel.GetState(0U)[14] &&
            (main_work.trg_cancel.GetState(0U)[10] && main_work.trg_cancel.GetState(0U)[1]))
        {
            setBackKeyRequest(false);
            main_work.flag |= 2U;
        }
        else
        {
            float frame =
                main_work.trg_act_move.GetState(0U)[15] || !main_work.trg_mode[0].GetState(0U)[14] &&
                !main_work.trg_mode[1].GetState(0U)[14]
                    ? (2.0 >= main_work.act[51].frame ? 0.0f : -1f)
                    : (!main_work.trg_mode[0].GetState(0U)[10] || !main_work.trg_mode[0].GetState(0U)[1]
                        ? (!main_work.trg_mode[1].GetState(0U)[10] || !main_work.trg_mode[1].GetState(0U)[1]
                            ? (main_work.trg_mode[0].GetState(0U)[0] || main_work.trg_mode[1].GetState(0U)[0]
                                ? 1f
                                : 0.0f)
                            : 2f)
                        : 2f);
            if (0.0 <= frame)
            {
                if (2.0 == frame)
                {
                    main_work.cur_game_mode ^= 1U;
                    DmSoundPlaySE("Cursol");
                }

                ArrayPointer<AOS_ACTION> arrayPointer1 = new ArrayPointer<AOS_ACTION>(main_work.act, 51);
                ArrayPointer<AOS_ACTION> arrayPointer2 = new ArrayPointer<AOS_ACTION>(main_work.act, 57);
                while (arrayPointer1 != arrayPointer2)
                {
                    AoActSetFrame(~arrayPointer1, frame);
                    ++arrayPointer1;
                }

                ArrayPointer<AOS_ACTION> arrayPointer3 = new ArrayPointer<AOS_ACTION>(main_work.act, 88);
                ArrayPointer<AOS_ACTION> arrayPointer4 = new ArrayPointer<AOS_ACTION>(main_work.act, 90);
                while (arrayPointer3 != arrayPointer4)
                {
                    AoActSetFrame(~arrayPointer3, frame);
                    ++arrayPointer3;
                }
            }

            uint num7 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
            if (3U < num7)
                num7 = 3U;
            uint focusDispNo = (uint) main_work.focus_disp_no;
            for (uint index = focusDispNo + num7; focusDispNo < index; ++focusDispNo)
            {
                CTrgAoAction ctrgAoAction = main_work.trg_act[(int) focusDispNo];
                if (ctrgAoAction.GetState(0U)[8])
                {
                    main_work.cur_stage = (int) num2 + (int) focusDispNo;
                    main_work.is_disp_cover = true;
                }

                if (ctrgAoAction.GetState(0U)[13])
                    main_work.is_disp_cover = false;
                if (ctrgAoAction.GetState(0U)[4])
                {
                    main_work.cur_stage = (int) num2 + (int) focusDispNo;
                    main_work.is_disp_cover = true;
                    main_work.flag |= 4U;
                    return;
                }
            }

            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            int isFinalOpen = main_work.is_final_open;
            int index1 = 0;
            for (int index2 = arrayof(main_work.trg_act_tab); index1 < index2; ++index1)
            {
                int num8 = index1;
                if ((2 & isFinalOpen) != 0 && (1 & isFinalOpen) == 0)
                {
                    switch (index1)
                    {
                        case 4:
                            num8 = 5;
                            break;
                        case 5:
                            continue;
                    }
                }

                if (main_work.trg_act_tab[index1].GetState(0U)[8] && main_work.cur_zone != num8)
                {
                    main_work.chng_zone = main_work.cur_zone;
                    main_work.cur_zone = (uint) num8;
                    if (main_work.chng_zone < main_work.cur_zone)
                        flag2 = true;
                    else
                        flag1 = true;
                }
            }

            int index3 = 0;
            for (int index2 = arrayof(main_work.trg_act_lr); index3 < index2; ++index3)
            {
                CTrgAoAction ctrgAoAction = main_work.trg_act_lr[index3];
                if (!main_work.trg_act_move.GetState(0U)[15] && ctrgAoAction.GetState(0U)[7] &&
                    ctrgAoAction.GetState(0U)[14])
                {
                    main_work.chng_zone = main_work.cur_zone;
                    main_work.cur_zone = (uint) this.dmStgSlctGetRevisedZoneNo((int) main_work.cur_zone,
                        index3 == 0 ? -1 : 1, 1 & isFinalOpen, 2 & isFinalOpen);
                    if (index3 == 0)
                        flag1 = true;
                    else
                        flag2 = true;
                }
            }

            CTrgStateEx state = main_work.trg_act_move.GetState(0U);
            CArray2<float> dragSpeed = state.GetDragSpeed();
            if (3.0 <= Math.Abs(dragSpeed.x) && Math.Abs(dragSpeed.y) < (double) Math.Abs(dragSpeed.x))
            {
                if (0.0 <= dragSpeed.x)
                    flag1 = true;
                else
                    flag2 = true;
                main_work.chng_zone = main_work.cur_zone;
                main_work.cur_zone = (uint) this.dmStgSlctGetRevisedZoneNo((int) main_work.cur_zone, flag1 ? 1 : -1,
                    1 & isFinalOpen, 2 & isFinalOpen);
            }
            else if (state[10])
            {
                if (state[15])
                {
                    int num8 = 0;
                    int num9;
                    switch (main_work.cur_zone)
                    {
                        case 4:
                            num9 = 0;
                            break;
                        case 5:
                            num9 = 4;
                            break;
                        default:
                            num9 = 1;
                            break;
                    }

                    int num10 = (int) amClamp(
                        ((int) ((int) dm_stgslct_act_disp_y_pos_tbl[0] - (double) main_work.act_top_pos_y[(int) num2] +
                                60.0) - (int) (dragSpeed.y * 120.0 * 0.25)) / 120, num8, num9);
                    main_work.prev_disp_no = main_work.focus_disp_no;
                    main_work.focus_disp_no = num10;
                    main_work.flag |= 64U;
                }
            }
            else if (state[9])
            {
                IntPair lastMove = state.GetLastMove();
                if (lastMove.second != 0)
                {
                    for (int index2 = (int) num2; index2 < num3; ++index2)
                        main_work.act_top_pos_y[index2] += lastMove.second * 2;
                    flag3 = true;
                }
            }

            if (flag1 || flag2 || (flag3 || (64 & (int) main_work.flag) != 0))
            {
                int index2 = 0;
                for (int index4 = arrayof(main_work.trg_act); index2 < index4; ++index2)
                    main_work.trg_act[index2].DelLock();
                main_work.is_disp_cover = false;
            }

            if (flag1)
            {
                uint num8 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
                num1 = num8 + dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
                main_work.flag |= 32U;
                main_work.act_move_dest[0] = 1120f;
                main_work.act_move_dest[1] = 100f;
                if (main_work.cur_zone == 4U)
                {
                    main_work.crsr_pos_y = 320f;
                    main_work.crsr_idx = 0;
                }

                if (main_work.chng_zone == 4U)
                {
                    main_work.crsr_pos_y = 160f;
                    main_work.crsr_idx = 0;
                }

                main_work.prev_disp_no = main_work.focus_disp_no;
                main_work.focus_disp_no = 0;
                main_work.cur_stage = (int) (main_work.crsr_idx + main_work.focus_disp_no + num8);
                this.dmStgSlctSetActChngZonePosInit(main_work, -1);
                DmSoundPlaySE("Cursol");
                if ((AoPad.AoPadMRepeat() & ControllerConsts.A) != 0 || (AoPad.AoPadMStand() & ControllerConsts.A) != 0)
                {
                    main_work.flag |= 4194304U;
                    main_work.btn_l_disp_frm = 0U;
                }

                main_work.timer = 0;
            }
            else
            {
                if (!flag2)
                    return;
                uint num8 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
                num1 = num8 + dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
                main_work.flag |= 32U;
                main_work.act_move_dest[0] = -1120f;
                main_work.act_move_dest[1] = 100f;
                if (main_work.cur_zone == 4U)
                {
                    main_work.crsr_pos_y = 320f;
                    main_work.crsr_idx = 0;
                }

                if (main_work.chng_zone == 4U)
                {
                    main_work.crsr_pos_y = 160f;
                    main_work.crsr_idx = 0;
                }

                main_work.prev_disp_no = main_work.focus_disp_no;
                main_work.focus_disp_no = 0;
                main_work.cur_stage = (int) (main_work.crsr_idx + main_work.focus_disp_no + num8);
                this.dmStgSlctSetActChngZonePosInit(main_work, 1);
                DmSoundPlaySE("Cursol");
                if ((AoPad.AoPadMRepeat() & ControllerConsts.R) != 0 || (AoPad.AoPadMStand() & ControllerConsts.R) != 0)
                {
                    main_work.flag |= 8388608U;
                    main_work.btn_r_disp_frm = 0U;
                }

                main_work.timer = 0;
            }
        }
    }

    private void dmStgSlctInputProcStageSelectChngZone(DMS_STGSLCT_MAIN_WORK main_work)
    {
        uint num1 = 0;
        int is_final_open = 0;
        int is_spe_open = 0;
        num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        int num2 = (int) dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
        int num3 = (int) dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
        int num4 = 3;
        if (main_work.is_clear_stage[3] == 1 && main_work.is_clear_stage[7] == 1 &&
            (main_work.is_clear_stage[11] == 1 && main_work.is_clear_stage[15] == 1))
        {
            is_final_open = 1;
            num4 = 4;
        }

        for (uint index = 17; index < 24U; ++index)
        {
            if (main_work.is_clear_stage[(int) index] != -1)
            {
                is_spe_open = 1;
                num4 = 5;
                break;
            }

            is_spe_open = 0;
        }

        if ((AoPad.AoPadStand() & ControllerConsts.B) != 0)
            main_work.flag |= 2U;
        else if ((AoPad.AoPadStand() & ControllerConsts.A) != 0)
            main_work.flag |= 4U;
        else if ((AoPad.AoPadStand() & ControllerConsts.X) != 0)
        {
            main_work.cur_game_mode ^= 1U;
            main_work.mode_tex_frm = 1f;
            DmSoundPlaySE("Cursol");
        }
        else
        {
            this.dmStgSlctInputChangeEvtRanking(main_work);
            if ((AoPad.AoPadMRepeat() & ControllerConsts.LEFT) != 0 || (AoPad.AoPadMRepeat() & ControllerConsts.L) != 0)
            {
                if ((AoPad.AoPadMStand() & ControllerConsts.LEFT) == 0 &&
                    (AoPad.AoPadMStand() & ControllerConsts.L) == 0 && main_work.cur_zone == 0U)
                    return;
                main_work.chng_zone = main_work.cur_zone;
                main_work.cur_zone =
                    (uint) this.dmStgSlctGetRevisedZoneNo((int) main_work.cur_zone, -1, is_final_open, is_spe_open);
                uint num5 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
                int num6 = (int) dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
                main_work.flag |= 32U;
                main_work.act_move_dest[0] = 1120f;
                main_work.act_move_dest[1] = 100f;
                if (main_work.cur_zone == 4U)
                {
                    main_work.crsr_pos_y = 320f;
                    main_work.crsr_idx = 0;
                }

                if (main_work.chng_zone == 4U)
                {
                    main_work.crsr_pos_y = 160f;
                    main_work.crsr_idx = 0;
                }

                main_work.prev_disp_no = main_work.focus_disp_no;
                main_work.focus_disp_no = 0;
                main_work.cur_stage = (int) (main_work.crsr_idx + main_work.focus_disp_no + num5);
                this.dmStgSlctSetActChngZonePosInit(main_work, -1);
                DmSoundPlaySE("Cursol");
                if ((AoPad.AoPadMRepeat() & ControllerConsts.L) != 0 || (AoPad.AoPadMStand() & ControllerConsts.L) != 0)
                {
                    main_work.flag |= 4194304U;
                    main_work.btn_l_disp_frm = 0U;
                }

                main_work.timer = 0;
            }
            else
            {
                if ((AoPad.AoPadMRepeat() & ControllerConsts.RIGHT) == 0 &&
                    (AoPad.AoPadMRepeat() & ControllerConsts.R) == 0 ||
                    (AoPad.AoPadMStand() & ControllerConsts.RIGHT) == 0 &&
                    (AoPad.AoPadMStand() & ControllerConsts.R) == 0 && (int) main_work.cur_zone == num4)
                    return;
                main_work.chng_zone = main_work.cur_zone;
                main_work.cur_zone =
                    (uint) this.dmStgSlctGetRevisedZoneNo((int) main_work.cur_zone, 1, is_final_open, is_spe_open);
                uint num5 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
                int num6 = (int) dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
                main_work.flag |= 32U;
                main_work.act_move_dest[0] = -1120f;
                main_work.act_move_dest[1] = 100f;
                if (main_work.cur_zone == 4U)
                {
                    main_work.crsr_pos_y = 320f;
                    main_work.crsr_idx = 0;
                }

                if (main_work.chng_zone == 4U)
                {
                    main_work.crsr_pos_y = 160f;
                    main_work.crsr_idx = 0;
                }

                main_work.prev_disp_no = main_work.focus_disp_no;
                main_work.focus_disp_no = 0;
                main_work.cur_stage = (int) (main_work.crsr_idx + main_work.focus_disp_no + num5);
                this.dmStgSlctSetActChngZonePosInit(main_work, 1);
                DmSoundPlaySE("Cursol");
                if ((AoPad.AoPadMRepeat() & ControllerConsts.R) != 0 || (AoPad.AoPadMStand() & ControllerConsts.R) != 0)
                {
                    main_work.flag |= 8388608U;
                    main_work.btn_r_disp_frm = 0U;
                }

                main_work.timer = 0;
            }
        }
    }

    private void dmStgSlctInputProcStageSelectMove(DMS_STGSLCT_MAIN_WORK main_work)
    {
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = num1 + dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
        uint num3 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] - 1U;
        if ((AoPad.AoPadStand() & ControllerConsts.B) != 0)
            main_work.flag |= 2U;
        else if ((AoPad.AoPadStand() & ControllerConsts.A) != 0)
            main_work.flag |= 4U;
        else if (((int) AoPad.AoPadStand() & 64) != 0)
        {
            main_work.cur_game_mode ^= 1U;
            main_work.mode_tex_frm = 1f;
            DmSoundPlaySE("Cursol");
        }
        else
        {
            this.dmStgSlctInputChangeEvtRanking(main_work);
            if ((AoPad.AoPadMRepeat() & ControllerConsts.UP) != 0)
            {
                if ((AoPad.AoPadMStand() & ControllerConsts.UP) == 0 && main_work.cur_stage == (int) num1 ||
                    main_work.cur_zone == 4U)
                    return;
                if (main_work.crsr_idx == 0)
                {
                    if (num3 > 3U)
                        main_work.flag |= 64U;
                    else
                        main_work.flag |= 128U;
                }
                else
                    main_work.flag |= 128U;

                main_work.flag |= 2048U;
            }
            else
            {
                if ((AoPad.AoPadMRepeat() & ControllerConsts.DOWN) == 0 ||
                    (AoPad.AoPadMStand() & ControllerConsts.DOWN) == 0 && main_work.cur_stage == (int) num2 - 1 ||
                    main_work.cur_zone == 4U)
                    return;
                if (main_work.crsr_idx == 3)
                {
                    if (num3 > 3U)
                        main_work.flag |= 64U;
                    else
                        main_work.flag |= 128U;
                }
                else
                    main_work.flag |= 128U;

                main_work.flag |= 4096U;
            }
        }
    }

    private void dmStgSlctInputChangeEvtRanking(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if ((AoPad.AoPadStand() & ControllerConsts.Y) == 0)
            return;
        main_work.flag |= 524288U;
    }

    private void dmStgSlctInputProcWinDispIdle(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.win_mode == 1)
        {
            int num1 = 0;
            for (int index = 2; num1 < index; ++num1)
            {
                ArrayPointer<AOS_ACTION> arrayPointer1 = null;
                ArrayPointer<AOS_ACTION> arrayPointer2 = null;
                int num2 = 0;
                uint num3 = 0;
                CTrgAoAction ctrgAoAction;
                switch (114 + num1)
                {
                    case 114:
                        ctrgAoAction = main_work.trg_answer[1];
                        arrayPointer1 = new ArrayPointer<AOS_ACTION>(main_work.act, 106);
                        arrayPointer2 = new ArrayPointer<AOS_ACTION>(main_work.act, 109);
                        num2 = 0;
                        num3 = 4U;
                        break;
                    case 115:
                        ctrgAoAction = main_work.trg_answer[0];
                        arrayPointer1 = new ArrayPointer<AOS_ACTION>(main_work.act, 109);
                        arrayPointer2 = new ArrayPointer<AOS_ACTION>(main_work.act, 112);
                        num2 = 1;
                        num3 = 2U;
                        break;
                    default:
                        ctrgAoAction = null;
                        break;
                }

                float frame;
                if (isBackKeyPressed())
                {
                    setBackKeyRequest(false);
                    int num4 = 1;
                    uint num5 = 2;
                    main_work.flag |= num5;
                    main_work.win_cur_slct = num4;
                    frame = 2f;
                }
                else if (ctrgAoAction.GetState(0U)[10] && ctrgAoAction.GetState(0U)[1])
                {
                    frame = 2f;
                    main_work.win_cur_slct = num2;
                    main_work.flag |= num3;
                }
                else
                    frame = !ctrgAoAction.GetState(0U)[0]
                        ? (2.0 > (~arrayPointer1).frame ? 0.0f : (~arrayPointer1).frame)
                        : 1f;

                while (arrayPointer1 != arrayPointer2)
                {
                    AoActSetFrame(arrayPointer1, frame);
                    AoActUpdate(arrayPointer1, 0.0f);
                    ++arrayPointer1;
                }
            }
        }
        else
        {
            if (!amTpIsTouchPush(0) && !isBackKeyPressed())
                return;
            setBackKeyRequest(false);
            main_work.flag |= 2U;
        }
    }

    private void dmStgSlctProcActDraw(DMS_STGSLCT_MAIN_WORK main_work)
    {
        this.dmStgSlctSetObiEfctPos(main_work);
        this.dmStgSlctCommonDraw(main_work);
        this.dmStgSlctZoneSelectDraw(main_work);
        if (main_work.state == 1)
            this.dmStgSlctStageSelectDraw(main_work);
        this.dmStgSlctCommonFixDraw(main_work);
        this.dmStgSlctMakeVertexAct(main_work, main_work.up_bg_vrtx);
    }

    private void dmStgSlctCommonDraw(DMS_STGSLCT_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(4096U);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        if (uint.MaxValue == main_work.cur_bg_id)
        {
            AoActSortRegAction(main_work.act[93]);
            AoActAcmPush();
            AoActAcmInit();
            AoActAcmApplyFade(main_work.bg_fade);
            AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[0]));
            AoActUpdate(main_work.act[93], 1f);
            AoActAcmPop();
        }
        else
        {
            for (int index = 0; index < 4; ++index)
            {
                AoActSortRegAction(main_work.act[index]);
                AoActSetFrame(main_work.act[index], main_work.cur_bg_id);
                AoActAcmPush();
                AoActAcmInit();
                AoActAcmApplyFade(main_work.bg_fade);
                AoActUpdate(main_work.act[index], 0.0f);
                AoActAcmPop();
            }
        }

        if (_am_sample_draw_enable)
        {
            AoActSortExecute();
            AoActSortDraw();
        }

        AoActSortUnregAll();
        AoActSysSetDrawTaskPrio(11264U);
        for (int index = 94; index <= 95; ++index)
            AoActSortRegAction(main_work.act[index]);
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[0]));
        for (int index = 94; index <= 95; ++index)
            AoActUpdate(main_work.act[index], 0.0f);
        if (_am_sample_draw_enable)
        {
            AoActSortExecute();
            AoActSortDraw();
        }

        AoActSortUnregAll();
    }

    private void dmStgSlctCommonFixDraw(DMS_STGSLCT_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(12288U);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        for (int index = 4; index <= 9; ++index)
            AoActSortRegAction(main_work.act[index]);
        for (int index = 10; index <= 16; ++index)
        {
            if ((main_work.get_emerald & (uint)(1 << index - 10)) != 0)
                AoActSortRegAction(main_work.act[index]);
        }

        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[1]));
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        if (main_work.state == 1)
        {
            if (main_work.cur_game_mode != 1U)
            {
                AoActSortRegAction(main_work.act[51]);
                AoActSortRegAction(main_work.act[52]);
                AoActSortRegAction(main_work.act[53]);
            }
            else
            {
                AoActSortRegAction(main_work.act[54]);
                AoActSortRegAction(main_work.act[55]);
                AoActSortRegAction(main_work.act[56]);
            }
        }

        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        if (main_work.cur_game_mode == 1U)
            AoActSortRegAction(main_work.act[88]);
        else
            AoActSortRegAction(main_work.act[89]);
        main_work.mode_tex_frm = 0.0f;
        if (uint.MaxValue == main_work.cur_bg_id || main_work.state == 0)
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            AoActSortRegAction(main_work.act[18]);
        }
        else if (main_work.cur_zone != 5U)
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            AoActSortRegAction(main_work.act[17]);
        }
        else
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            AoActSortRegAction(main_work.act[18]);
        }

        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[4]));
        if (0.0 == main_work.win_size_rate[0] && 0.0 == main_work.win_size_rate[1])
        {
            AoActSortRegAction(main_work.act[112]);
            AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
            for (int index = 104; index <= 105; ++index)
                AoActSortRegAction(main_work.act[index]);
        }

        this.dmStgSlctSetSonicStockDispFrame(main_work);
        if (main_work.is_jp_region)
            AoActSetFrame(main_work.act[96], 0.0f);
        else
            AoActSetFrame(main_work.act[96], 1f);
        AoActSetFrame(main_work.act[17], main_work.cur_zone);
        float frame1 = 0.0f;
        if (uint.MaxValue == main_work.cur_bg_id || main_work.state == 0)
            frame1 = 1f;
        AoActSetFrame(main_work.act[18], frame1);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        for (int index = 4; index <= 8; ++index)
            AoActUpdate(main_work.act[index], 0.0f);
        AoActAcmPush();
        for (int index = 9; index <= 16; ++index)
        {
            AoActAcmInit();
            AoActAcmApplyTrans(0.0f, main_work.chaos_eme_pos_y, 0.0f);
            AoActUpdate(main_work.act[index], 0.0f);
        }

        AoActAcmPop();
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        AoActAcmPush();
        bool flag = false;
        if (main_work.proc_win_update != null &&
            new DMS_STGSLCT_MAIN_WORK._proc_win_update_(this.dmStgSlctProcWindowNodispIdle) !=
            main_work.proc_win_update)
            flag = true;
        if (main_work.trg_act_move.GetState(0U)[15] || flag ||
            !main_work.trg_mode[0].GetState(0U)[14] && !main_work.trg_mode[1].GetState(0U)[14])
            AoActUpdate(main_work.act[57]);
        float[] numArray = new float[2]
        {
            main_work.act[57].sprite.center_x - main_work.act_tab_state_move_base_pos[0],
            main_work.act[57].sprite.center_y - main_work.act_tab_state_move_base_pos[1]
        };
        for (int index = 51; index <= 56; ++index)
        {
            AoActAcmInit();
            AoActAcmApplyTrans(0.0f, main_work.mode_tex_pos_y, 0.0f);
            AoActAcmApplyTrans(numArray[0], numArray[1], 0.0f);
            float frame2 = 2.0 <= main_work.act[index].frame ? 1f : 0.0f;
            AoActUpdate(main_work.act[index], frame2);
        }

        AoActAcmPop();
        int index1 = 0;
        for (int index2 = arrayof(main_work.trg_mode); index1 < index2; ++index1)
            main_work.trg_mode[index1].Update();
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        AoActUpdate(main_work.act[17], 0.0f);
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        AoActUpdate(main_work.act[18], 0.0f);
        AoActAcmPush();
        for (int index2 = 88; index2 <= 89; ++index2)
        {
            AoActAcmInit();
            AoActAcmApplyTrans(0.0f, main_work.mode_tex_pos_y, 0.0f);
            AoActAcmApplyTrans(numArray[0], numArray[1], 0.0f);
            float frame2 = 2.0 <= main_work.act[index2].frame ? 1f : 0.0f;
            AoActUpdate(main_work.act[index2], frame2);
        }

        AoActAcmPop();
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[4]));
        AoActUpdate(main_work.act[112], 0.0f);
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index2 = 104; index2 <= 105; ++index2)
        {
            float frame2 = 2.0 <= main_work.act[index2].frame ? 1f : 0.0f;
            AoActUpdate(main_work.act[index2], frame2);
        }

        main_work.trg_cancel.Update();
        uint num;
        float x;
        if (main_work.is_final_open == 3)
        {
            num = 6U;
            x = 0.0f;
        }
        else if ((main_work.is_final_open & 2) != 0 || (main_work.is_final_open & 1) != 0)
        {
            num = 5U;
            x = 24f;
        }
        else if ((main_work.is_final_open & 2) != 0 || (main_work.is_final_open & 1) != 0)
        {
            num = 5U;
            x = 24f;
        }
        else
        {
            num = 4U;
            x = 64f;
        }

        if (main_work.state == 1)
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            for (uint index2 = 0; index2 < num; ++index2)
            {
                AoActSortRegAction(main_work.act[(int) (43U + index2)]);
                if ((int) main_work.cur_zone == (int) index2)
                    AoActSetFrame(main_work.act[(int) (43U + index2)], 0.0f);
                else
                    AoActSetFrame(main_work.act[(int) (43U + index2)], 1f);
                if ((main_work.is_final_open & 2) != 0 && main_work.cur_zone == 5U &&
                    (index2 == 4U && (main_work.is_final_open & 1) == 0))
                    AoActSetFrame(main_work.act[(int) (43U + index2)], 0.0f);
                AoActAcmPush();
                AoActAcmInit();
                AoActAcmApplyTrans(x, 0.0f, 0.0f);
                AoActUpdate(main_work.act[(int) (43U + index2)], 0.0f);
                main_work.act[(int) (43U + index2)].sprite.hit.rect.bottom += 25f;
                main_work.act[(int) (43U + index2)].sprite.hit.rect.top -= 30f;
                main_work.trg_act_tab[(int) index2].Update();
                AoActAcmPop();
            }
        }

        if (_am_sample_draw_enable)
        {
            AoActSortExecuteFix();
            AoActSortDraw();
        }

        AoActSortUnregAll();
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        if (_am_sample_draw_enable)
        {
            AoActSortExecute();
            AoActSortDraw();
        }

        AoActSortUnregAll();
    }

    private void dmStgSlctSetSonicStockDispFrame(DMS_STGSLCT_MAIN_WORK main_work)
    {
        int[] numArray = new int[3];
        int num1 = 1;
        int num2 = (int) main_work.player_stock - 1;
        if (num2 < 0)
            num2 = 0;
        if (num2 > 999)
            num2 = 999;
        for (uint index1 = 0; index1 < 3U; ++index1)
        {
            for (uint index2 = 0; index2 < (uint) (3 - (int) index1 - 1); ++index2)
                num1 *= 10;
            if (num2 >= num1)
            {
                numArray[(int) index1] = num2 / num1;
                num2 -= numArray[(int) index1] * num1;
            }
            else
                numArray[(int) index1] = 0;

            num1 = 1;
        }

        for (uint index = 0; index < 3U; ++index)
            AoActSetFrame(main_work.act[(int) (6U + index)], numArray[(int) index]);
    }

    private void dmStgSlctZoneSelectDraw(DMS_STGSLCT_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(8192U);
        for (uint i = 0; i < 6U; ++i)
            this.dmStgSlctOneZoneTableDraw(main_work, i);
        if (((int) main_work.flag & 262144) == 0)
            return;
        int decideZoneEfctDistX = main_work.decide_zone_efct_dist_x;
        int decideZoneEfctDistY = main_work.decide_zone_efct_dist_y;
    }

    private void dmStgSlctOneZoneTableDraw(DMS_STGSLCT_MAIN_WORK main_work, uint i)
    {
        if (((int) main_work.announce_flag & 4) != 0)
        {
            if (((int) main_work.flag & 131072) != 0 && i != 0U)
                return;
        }
        else if (((int) main_work.announce_flag & 128) != 0)
        {
            if (((int) main_work.flag & 131072) != 0 && i == 4U)
                return;
        }
        else if (((int) main_work.announce_flag & 512) != 0 && ((int) main_work.flag & 131072) != 0 && i == 5U)
            return;

        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        AoActSortRegAction(main_work.act[37]);
        AoActSortRegAction(main_work.act[(int) (19U + i)]);
        if (i != 5U)
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            AoActSortRegAction(main_work.act[38]);
        }
        else
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            AoActSortRegAction(main_work.act[39]);
        }

        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        if ((int) main_work.cur_zone != (int) i && main_work.is_disp_cover)
        {
            AoActSortRegAction(main_work.act[41]);
            AoActSortRegAction(main_work.act[40]);
            AoActSortRegAction(main_work.act[42]);
        }

        AoActSetFrame(main_work.act[(int) (19U + i)], main_work.zone_scr_id);
        if (i < 4U)
        {
            AoActSortRegAction(main_work.act[(int) (uint) (25 + (int) i * 3)]);
            AoActSortRegAction(main_work.act[(int) (uint) (26 + (int) i * 3)]);
            AoActSortRegAction(main_work.act[(int) (uint) (27 + (int) i * 3)]);
            AoActSetFrame(main_work.act[(int) (uint) (25 + (int) i * 3)], main_work.zone_scr_id);
            AoActSetFrame(main_work.act[(int) (uint) (26 + (int) i * 3)], main_work.zone_scr_id);
            AoActSetFrame(main_work.act[(int) (uint) (27 + (int) i * 3)], main_work.zone_scr_id);
        }

        AoActSetFrame(main_work.act[38], i);
        int num1;
        int num2;
        if (((int) main_work.flag & 262144) != 0 && (int) main_work.cur_zone == (int) i)
        {
            num1 = main_work.decide_zone_efct_dist_x;
            num2 = main_work.decide_zone_efct_dist_y;
        }
        else
        {
            num1 = 0;
            num2 = 0;
        }

        AoActAcmPush();
        AoActAcmInit();
        AoActAcmApplyTrans(main_work.zone_pos[(int) i][0] + num1, main_work.zone_pos[(int) i][1] + num2, 0.0f);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        AoActUpdate(main_work.act[37], 0.0f);
        AoActUpdate(main_work.act[(int) (19U + i)], 0.0f);
        main_work.trg_zone[(int) i].Update();
        for (i = 25U; i <= 36U; ++i)
            AoActUpdate(main_work.act[(int) i], 0.0f);
        AoActUpdate(main_work.act[41], 0.0f);
        AoActUpdate(main_work.act[40], 0.0f);
        AoActUpdate(main_work.act[42], 0.0f);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        AoActUpdate(main_work.act[38], 0.0f);
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        AoActUpdate(main_work.act[39], 0.0f);
        AoActAcmPop();
        if (_am_sample_draw_enable)
        {
            AoActSortExecute();
            AoActSortDraw();
        }

        AoActSortUnregAll();
    }

    private void dmStgSlctStageSelectDraw(DMS_STGSLCT_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(8192U);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        this.dmStgSlctSetDrawStageSelectTable(main_work, main_work.cur_zone, true);
        if (((int) main_work.flag & 32) != 0 && main_work.chng_zone != 7U)
            this.dmStgSlctSetDrawStageSelectTable(main_work, main_work.chng_zone);
        main_work.trg_act_move.Update();
        if (main_work.state == 1 && main_work.cur_zone == 5U)
            main_work.disp_flag |= 4U;
        else
            main_work.disp_flag &= 4294967291U;
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        if (((int) main_work.disp_flag & 2) != 0 && 4U != main_work.cur_zone)
        {
            if (0 < main_work.focus_disp_no || (64 & (int) main_work.flag) != 0)
                AoActSortRegAction(main_work.act[70]);
            int num = 5U == main_work.cur_zone ? 4 : 1;
            if (main_work.focus_disp_no < num || (64 & (int) main_work.flag) != 0)
                AoActSortRegAction(main_work.act[71]);
        }

        AoActAcmPush();
        for (uint index = 70; index <= 71U; ++index)
        {
            AoActAcmInit();
            AoActUpdate(main_work.act[(int) index], 1f);
        }

        AoActAcmPop();
        if (_am_sample_draw_enable)
        {
            AoActSortExecute();
            AoActSortDraw();
        }

        AoActSortUnregAll();
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        if (((int) main_work.disp_flag & 2) == 0)
            return;
        for (uint index = 0; index < 2U; ++index)
        {
            AoActSortRegAction(main_work.act[(int) (49U + index)]);
            AoActUpdate(main_work.act[(int) (49U + index)], 1f);
        }

        uint num1 = 0;
        for (int index = arrayof(main_work.trg_act_lr); num1 < index; ++num1)
            main_work.trg_act_lr[(int) num1].Update();
        if (_am_sample_draw_enable)
        {
            AoActSortExecute();
            AoActSortDraw();
        }

        AoActSortUnregAll();
    }

    private void dmStgSlctSetDrawStageSelectTable(DMS_STGSLCT_MAIN_WORK main_work, uint zone)
    {
        this.dmStgSlctSetDrawStageSelectTable(main_work, zone, false);
    }

    private void dmStgSlctSetDrawStageSelectTable(
        DMS_STGSLCT_MAIN_WORK main_work,
        uint zone,
        bool is_trg_update)
    {
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) zone][1] + num1;
        CActionDraw selectActDrawSys = stgSelect_act_draw_sys;
        selectActDrawSys.Clear();
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        for (uint cnt = num1; cnt < num2; ++cnt)
        {
            for (uint index = 58; index <= 60U; ++index)
            {
                this.dmStgSlctSetTableActiveInfo(main_work, cnt);
                AoActAcmPush();
                AoActAcmInit();
                AoActAcmApplyTrans(main_work.act_top_pos_x[(int) cnt],
                    main_work.act_top_pos_y[(int) cnt] + (cnt - num1) * 120f, 0.0f);
                AoActUpdate(main_work.act[(int) index], 0.0f);
                AoActAcmPop();
                selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[(int) index],
                    main_work.act[(int) index].frame, main_work.act_top_pos_x[(int) cnt],
                    main_work.act_top_pos_y[(int) cnt] + (cnt - num1) * 120f);
            }

            if (is_trg_update && num1 <= cnt && cnt < num1 + arrayof(main_work.trg_act))
            {
                uint num3 = cnt - num1;
                main_work.trg_act[(int) num3].Update();
            }
        }

        selectActDrawSys.Draw();
        selectActDrawSys.Clear();
        if (zone != 5U)
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            for (uint index = num1; index < num2; ++index)
            {
                AoActUpdate(main_work.act[62], 0.0f);
                selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[62], main_work.act[62].frame,
                    main_work.act_top_pos_x[(int) index], main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
            }

            for (uint cnt = num1; cnt < num2; ++cnt)
            {
                this.dmStgSlctSetTableActiveInfo(main_work, cnt);
                AoActUpdate(main_work.act[61], 0.0f);
                selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[61], main_work.act[61].frame,
                    main_work.act_top_pos_x[(int) cnt], main_work.act_top_pos_y[(int) cnt] + (cnt - num1) * 120f);
            }

            selectActDrawSys.Draw();
            selectActDrawSys.Clear();
            for (uint index = num1; index < num2; ++index)
            {
                if (index == 16U)
                {
                    AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                    AoActUpdate(main_work.act[74], 0.0f);
                    selectActDrawSys.Entry(main_work.ama[1], g_dm_act_id_tbl_stg_slct[74], main_work.act[74].frame,
                        main_work.act_top_pos_x[(int) index],
                        main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
                }
                else if ((index + 1U) % 4U != 0U || index == 0U)
                {
                    AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                    AoActUpdate(main_work.act[63], 0.0f);
                    selectActDrawSys.Entry(main_work.ama[1], g_dm_act_id_tbl_stg_slct[63], main_work.act[63].frame,
                        main_work.act_top_pos_x[(int) index],
                        (float) (main_work.act_top_pos_y[(int) index] + (index - num1) * 120.0 - 4.0));
                }
                else
                {
                    AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                    AoActUpdate(main_work.act[74], 0.0f);
                    selectActDrawSys.Entry(main_work.ama[1], g_dm_act_id_tbl_stg_slct[74], main_work.act[74].frame,
                        main_work.act_top_pos_x[(int) index],
                        main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
                }
            }

            for (uint index = num1; index < num2; ++index)
            {
                if (index != 16U && ((index + 1U) % 4U != 0U || index == 0U))
                {
                    AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
                    AoActSetFrame(main_work.act[64], this.dm_stgslct_act_num_disp_id_tbl[(int) index]);
                    AoActUpdate(main_work.act[64], 0.0f);
                    selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[64], main_work.act[64].frame,
                        main_work.act_top_pos_x[(int) index],
                        main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
                }
            }

            selectActDrawSys.Draw();
            selectActDrawSys.Clear();
            for (uint cnt = num1; cnt < num2; ++cnt)
            {
                this.dmStgSlctSetTableActiveInfo(main_work, cnt);
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActUpdate(main_work.act[65], 0.0f);
                selectActDrawSys.Entry(main_work.ama[1], g_dm_act_id_tbl_stg_slct[65], main_work.act[65].frame,
                    main_work.act_top_pos_x[(int) cnt], main_work.act_top_pos_y[(int) cnt] + (cnt - num1) * 120f);
            }

            for (uint index = num1; index < num2; ++index)
            {
                AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
                AoActUpdate(main_work.act[66], 0.0f);
                AoActSortRegAction(main_work.act[66]);
                selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[66], main_work.act[66].frame,
                    main_work.act_top_pos_x[(int) index], main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
            }

            selectActDrawSys.Draw();
            selectActDrawSys.Clear();
            for (uint index1 = num1; index1 < num2; ++index1)
            {
                for (uint index2 = 0; index2 < 7U; ++index2)
                {
                    if ((int) main_work.eme_stage_no[(int) index2] == (int) index1 &&
                        (main_work.get_emerald & (uint)(1 << (int)index2)) != 0)
                    {
                        AoActUpdate(main_work.act[67], 0.0f);
                        AoActSetFrame(main_work.act[67], index2);
                        selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[67], main_work.act[67].frame,
                            main_work.act_top_pos_x[(int) index1],
                            main_work.act_top_pos_y[(int) index1] + (index1 - num1) * 120f);
                        break;
                    }
                }
            }

            selectActDrawSys.Draw();
            selectActDrawSys.Clear();
        }
        else
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            for (uint index = num1; index < num2; ++index)
            {
                AoActUpdate(main_work.act[75], 0.0f);
                selectActDrawSys.Entry(main_work.ama[1], g_dm_act_id_tbl_stg_slct[75], main_work.act[75].frame,
                    main_work.act_top_pos_x[(int) index], main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
            }

            selectActDrawSys.Draw();
            selectActDrawSys.Clear();
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            for (uint index = num1; index < num2; ++index)
            {
                if (GsMainSysIsStageClear(21 + ((int) index - (int) num1)))
                {
                    AoActSetFrame(main_work.act[69], index - num1);
                    AoActUpdate(main_work.act[69], 0.0f);
                    selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[69], main_work.act[69].frame,
                        main_work.act_top_pos_x[(int) index],
                        main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
                }
            }

            for (uint index = num1; index < num2; ++index)
            {
                AoActSetFrame(main_work.act[68], this.dm_stgslct_act_num_disp_id_tbl[(int) index]);
                AoActUpdate(main_work.act[68], 0.0f);
                selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[68], main_work.act[68].frame,
                    main_work.act_top_pos_x[(int) index], main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
            }

            selectActDrawSys.Draw();
            selectActDrawSys.Clear();
        }

        if (main_work.cur_game_mode == 0U)
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            for (uint index = num1; index < num2; ++index)
            {
                AoActUpdate(main_work.act[72], 0.0f);
                selectActDrawSys.Entry(main_work.ama[1], g_dm_act_id_tbl_stg_slct[72], main_work.act[72].frame,
                    main_work.act_top_pos_x[(int) index], main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
            }

            for (uint act_no = num1; act_no < num2; ++act_no)
            {
                this.dmStgSlctSetScoreDispFrame(main_work, zone, act_no);
                uint num3 = 0;
                while (num3 < 9U && 0.0 == main_work.act[(int) (76U + num3)].frame)
                    ++num3;
                for (; num3 < 9U; ++num3)
                {
                    AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
                    AoActUpdate(main_work.act[(int) (76U + num3)], 0.0f);
                    selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[(int) (76U + num3)],
                        main_work.act[(int) (76U + num3)].frame, main_work.act_top_pos_x[(int) act_no],
                        main_work.act_top_pos_y[(int) act_no] + (act_no - num1) * 120f);
                }
            }
        }
        else
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            for (uint index = num1; index < num2; ++index)
            {
                AoActUpdate(main_work.act[73], 0.0f);
                selectActDrawSys.Entry(main_work.ama[1], g_dm_act_id_tbl_stg_slct[73], main_work.act[73].frame,
                    main_work.act_top_pos_x[(int) index], main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
            }

            for (uint act_no = num1; act_no < num2; ++act_no)
            {
                this.dmStgSlctSetScoreDispFrame(main_work, zone, act_no);
                for (uint index = 0; index < 7U; ++index)
                {
                    AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
                    AoActUpdate(main_work.act[(int) (76U + index)], 0.0f);
                    selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[(int) (76U + index)],
                        main_work.act[(int) (76U + index)].frame, main_work.act_top_pos_x[(int) act_no],
                        main_work.act_top_pos_y[(int) act_no] + (act_no - num1) * 120f);
                }
            }
        }

        selectActDrawSys.Draw();
        selectActDrawSys.Clear();
        for (uint act_no = num1; act_no < num2; ++act_no)
        {
            this.dmStgSlctSetScoreDispFrame(main_work, zone, act_no);
            if (_am_sample_draw_enable)
            {
                AoActSortExecute();
                AoActSortDraw();
            }

            AoActSortUnregAll();
        }

        for (uint index = num1; index < num2; ++index)
        {
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            if (main_work.cur_stage != (int) index && main_work.is_disp_cover)
            {
                AoActUpdate(main_work.act[85], 0.0f);
                AoActUpdate(main_work.act[86], 0.0f);
                AoActUpdate(main_work.act[87], 0.0f);
                selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[87], main_work.act[87].frame,
                    main_work.act_top_pos_x[(int) index], main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
                selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[86], main_work.act[86].frame,
                    main_work.act_top_pos_x[(int) index], main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
                selectActDrawSys.Entry(main_work.ama[0], g_dm_act_id_tbl_stg_slct[85], main_work.act[85].frame,
                    main_work.act_top_pos_x[(int) index], main_work.act_top_pos_y[(int) index] + (index - num1) * 120f);
            }
        }

        selectActDrawSys.Draw();
        selectActDrawSys.Clear();
    }

    private void dmStgSlctSetScoreDispFrame(
        DMS_STGSLCT_MAIN_WORK main_work,
        uint zone,
        uint act_no)
    {
        int[] dispFrameTmpDigit = dmStgSlctSetScoreDispFrame_tmp_digit;
        Array.Clear(dispFrameTmpDigit, 0, dispFrameTmpDigit.Length);
        int num1 = 1;
        ushort min = 0;
        ushort sec = 0;
        ushort msec = 0;
        int[] dispFrameTimeDigit = dmStgSlctSetScoreDispFrame_time_digit;
        Array.Clear(dispFrameTimeDigit, 0, dispFrameTimeDigit.Length);
        bool flag = false;
        if (main_work.cur_game_mode == 0U)
        {
            if (main_work.hi_score[(int) act_no] != 1000000000)
            {
                int num2 = main_work.hi_score[(int) act_no];
                for (uint index1 = 0; index1 < 9U; ++index1)
                {
                    if ((uint) (9 - (int) index1 - 1) <= 0U)
                    {
                        num2 = 1;
                    }
                    else
                    {
                        for (uint index2 = 0; index2 < (uint) (9 - (int) index1 - 1); ++index2)
                            num1 *= 10;
                    }

                    if (num2 >= num1)
                    {
                        AoActSortRegAction(main_work.act[(int) (76U + index1)]);
                        if (index1 >= 8U)
                        {
                            dispFrameTmpDigit[(int) index1] = 0;
                        }
                        else
                        {
                            dispFrameTmpDigit[(int) index1] = num2 / num1;
                            num2 -= dispFrameTmpDigit[(int) index1] * num1 - 1;
                        }

                        flag = true;
                    }
                    else
                    {
                        if (flag)
                            AoActSortRegAction(main_work.act[(int) (76U + index1)]);
                        dispFrameTmpDigit[(int) index1] = 0;
                    }

                    num1 = 1;
                }

                for (uint index = 0; index < 9U; ++index)
                    AoActSetFrame(main_work.act[(int) (76U + index)], dispFrameTmpDigit[(int) index]);
            }
            else
            {
                for (uint index = 0; index < 9U; ++index)
                {
                    AoActSortRegAction(main_work.act[(int) (76U + index)]);
                    AoActSetFrame(main_work.act[(int) (76U + index)], 10f);
                }
            }
        }
        else if (main_work.record_time[(int) act_no] != 36000)
        {
            AkUtilFrame60ToTime((uint) main_work.record_time[(int) act_no], ref min, ref sec, ref msec);
            float num2 = sec;
            if (num2 >= 10.0)
            {
                dispFrameTimeDigit[0] = (int) (num2 / 10.0);
                num2 -= dispFrameTimeDigit[0] * 10f;
            }
            else
                dispFrameTimeDigit[0] = 0;

            dispFrameTimeDigit[1] = (int) num2;
            AoActSetFrame(main_work.act[76], min);
            AoActSetFrame(main_work.act[77], 11f);
            AoActSetFrame(main_work.act[78], dispFrameTimeDigit[0]);
            AoActSetFrame(main_work.act[79], dispFrameTimeDigit[1]);
            AoActSetFrame(main_work.act[80], 11f);
            float num3 = msec;
            if (num3 >= 10.0)
            {
                dispFrameTimeDigit[0] = (int) (num3 / 10.0);
                num3 -= dispFrameTimeDigit[0] * 10f;
            }
            else
                dispFrameTimeDigit[0] = 0;

            dispFrameTimeDigit[1] = (int) num3;
            AoActSetFrame(main_work.act[81], dispFrameTimeDigit[0]);
            AoActSetFrame(main_work.act[82], dispFrameTimeDigit[1]);
        }
        else
        {
            for (uint index = 0; index < 7U; ++index)
            {
                if (index == 1U || index == 4U)
                    AoActSetFrame(main_work.act[(int) (76U + index)], 11f);
                else
                    AoActSetFrame(main_work.act[(int) (76U + index)], 10f);
            }
        }
    }

    private void dmStgSlctWinSelectDraw(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float[] numArray1 = new float[2];
        AoActSysSetDrawTaskPrio(16384U);
        numArray1[0] = 705.6f;
        numArray1[1] = 302.4f;
        if (main_work.win_mode == 1)
        {
            float[] numArray2 = new float[2] {1.01f, 1.27f};
            for (int index = 0; index < 2; ++index)
                numArray1[index] *= numArray2[index];
        }

        if (0.0 < main_work.win_size_rate[0] && 0.0 < main_work.win_size_rate[1])
            AoWinSysDrawTask(0, AoTexGetTexList(main_work.cmn_tex[3]), 0U, 480f, 360f,
                numArray1[0] * main_work.win_size_rate[0], numArray1[1] * main_work.win_size_rate[1], 13568);
        if (((int) main_work.disp_flag & 1) == 0)
            return;
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[4]));
        switch (main_work.win_mode)
        {
            case 1:
                AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
                int index1 = 106;
                for (int index2 = 111; index1 <= index2; ++index1)
                    AoActSortRegAction(main_work.act[index1]);
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[91]);
                AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[4]));
                AoActSortRegAction(main_work.act[114]);
                AoActSortRegAction(main_work.act[115]);
                if (main_work.cur_zone != 5U)
                {
                    AoActSetFrame(main_work.act[91], 0.0f);
                    break;
                }

                AoActSetFrame(main_work.act[91], 1f);
                break;
            case 2:
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[90]);
                AoActSetFrame(main_work.act[90], this.dm_stgslct_win_act_frm_tbl[2][1]);
                break;
            case 3:
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[90]);
                AoActSetFrame(main_work.act[90], this.dm_stgslct_win_act_frm_tbl[3][1]);
                break;
            case 4:
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[90]);
                AoActSetFrame(main_work.act[90], this.dm_stgslct_win_act_frm_tbl[4][1]);
                break;
            case 5:
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[90]);
                AoActSetFrame(main_work.act[90], this.dm_stgslct_win_act_frm_tbl[5][1]);
                break;
            case 6:
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[90]);
                AoActSetFrame(main_work.act[90], this.dm_stgslct_win_act_frm_tbl[6][1]);
                break;
            case 7:
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[90]);
                AoActSetFrame(main_work.act[90], this.dm_stgslct_win_act_frm_tbl[7][1]);
                break;
            case 8:
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[92]);
                break;
            case 9:
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[90]);
                AoActSetFrame(main_work.act[90], this.dm_stgslct_win_act_frm_tbl[9][1]);
                break;
        }

        if (main_work.is_jp_region)
            AoActSetFrame(main_work.act[101], 0.0f);
        else
            AoActSetFrame(main_work.act[101], 1f);
        AoActAcmPush();
        for (int index2 = 0; index2 < 3; ++index2)
        {
            AoActAcmInit();
            AoActAcmApplyTrans(this.dm_stgslct_win_act_pos_tbl[index2 + 1][0],
                this.dm_stgslct_win_act_pos_tbl[index2 + 1][1], 0.0f);
            if (index2 == 0)
                AoActAcmApplyTrans(this.dm_stgslct_back_text_length_tbl[GsEnvGetLanguage()], 0.0f, 0.0f);
            AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[1]));
            AoActUpdate(main_work.act[101 + index2], 0.0f);
        }

        AoActAcmPop();
        AoActAcmPush();
        for (int index2 = 0; index2 < 5; ++index2)
        {
            AoActAcmInit();
            AoActAcmApplyTrans(this.dm_stgslct_win_act_pos_tbl[4 + index2][0],
                this.dm_stgslct_win_act_pos_tbl[4 + index2][1], 0.0f);
            AoActAcmApplyScale(1.68f, 1.68f);
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            AoActUpdate(main_work.act[90 + index2], 0.0f);
        }

        AoActAcmPop();
        AoActAcmPush();
        int num = 113;
        for (int index2 = 0; index2 < 3; ++index2)
        {
            AoActAcmInit();
            AoActAcmApplyTrans(this.dm_stgslct_win_act_pos_tbl[11 + index2][0],
                this.dm_stgslct_win_act_pos_tbl[11 + index2][1], 0.0f);
            if (main_work.win_mode == 8 && index2 == 2)
                AoActAcmApplyTrans(0.0f, 16f, 0.0f);
            AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[4]));
            AoActUpdate(main_work.act[num + index2], 0.0f);
            main_work.act[num + index2].sprite.center_y += 5f;
            CTrgAoAction ctrgAoAction;
            switch (num + index2)
            {
                case 114:
                    ctrgAoAction = main_work.trg_answer[1];
                    AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
                    ArrayPointer<AOS_ACTION> arrayPointer1 = new ArrayPointer<AOS_ACTION>(main_work.act, 106);
                    ArrayPointer<AOS_ACTION> arrayPointer2 = new ArrayPointer<AOS_ACTION>(main_work.act, 109);
                    while (arrayPointer1 != arrayPointer2)
                    {
                        float frame = 2.0 <= (~arrayPointer1).frame ? 1f : 0.0f;
                        AoActUpdate(arrayPointer1, frame);
                        ++arrayPointer1;
                    }

                    break;
                case 115:
                    ctrgAoAction = main_work.trg_answer[0];
                    AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
                    ArrayPointer<AOS_ACTION> arrayPointer3 = new ArrayPointer<AOS_ACTION>(main_work.act, 109);
                    ArrayPointer<AOS_ACTION> arrayPointer4 = new ArrayPointer<AOS_ACTION>(main_work.act, 112);
                    while (arrayPointer3 != arrayPointer4)
                    {
                        float frame = 2.0 <= (~arrayPointer3).frame ? 1f : 0.0f;
                        AoActUpdate(arrayPointer3, frame);
                        ++arrayPointer3;
                    }

                    break;
                default:
                    ctrgAoAction = null;
                    break;
            }

            ctrgAoAction?.Update();
        }

        AoActAcmPop();
        if (_am_sample_draw_enable)
        {
            AoActSortExecute();
            AoActSortDraw();
        }

        AoActSortUnregAll();
    }

    private int dmStgSlctIsDataLoad(DMS_STGSLCT_MAIN_WORK main_work)
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

        return 1;
    }

    private int dmStgSlctIsTexLoad(DMS_STGSLCT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AoTexIsLoaded(main_work.tex[index]))
                return 0;
        }

        return !GsFontIsBuilded() ? 0 : 1;
    }

    private int dmStgSlctIsTexLoad2(DMS_STGSLCT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 5; ++index)
        {
            if (!AoTexIsLoaded(main_work.cmn_tex[index]))
                return 0;
        }

        return 1;
    }

    private int dmStgSlctIsTexRelease(DMS_STGSLCT_MAIN_WORK main_work)
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

        return 1;
    }

    private void dmStgSlctSetDecideZoneEfctPos(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.timer < 6)
        {
            main_work.decide_zone_efct_dist_x = 0;
            main_work.decide_zone_efct_dist_y = 8;
        }
        else
        {
            main_work.decide_zone_efct_dist_x = 0;
            main_work.decide_zone_efct_dist_y = 0;
        }
    }

    private bool dmStgSlctIsDecideZoneEfctPos(DMS_STGSLCT_MAIN_WORK main_work)
    {
        return main_work.timer > 10;
    }

    private void dmStgSlctSetZonePosOutEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float[] numArray1 = new float[2];
        float[] numArray2 = new float[2];
        for (uint index = 0; index < 6U; ++index)
        {
            if ((main_work.efct_out_flag & (uint)(1 << (int)index)) != 0 && (int)index != (int)main_work.cur_zone)
            {
                if (main_work.is_final_open == 3)
                {
                    numArray2[0] = dm_stgslct_a_zone_nodisp_pos_tbl[(int) index][0] -
                                   dm_stgslct_a_zone_disp_pos_tbl[(int) index][0];
                    numArray2[1] = dm_stgslct_a_zone_nodisp_pos_tbl[(int) index][1] -
                                   dm_stgslct_a_zone_disp_pos_tbl[(int) index][1];
                }
                else if ((main_work.is_final_open & 2) != 0)
                {
                    numArray2[0] = dm_stgslct_s_zone_nodisp_pos_tbl[(int) index][0] -
                                   dm_stgslct_s_zone_disp_pos_tbl[(int) index][0];
                    numArray2[1] = dm_stgslct_s_zone_nodisp_pos_tbl[(int) index][1] -
                                   dm_stgslct_s_zone_disp_pos_tbl[(int) index][1];
                }
                else if ((main_work.is_final_open & 1) != 0)
                {
                    numArray2[0] = dm_stgslct_f_zone_nodisp_pos_tbl[(int) index][0] -
                                   dm_stgslct_f_zone_disp_pos_tbl[(int) index][0];
                    numArray2[1] = dm_stgslct_f_zone_nodisp_pos_tbl[(int) index][1] -
                                   dm_stgslct_f_zone_disp_pos_tbl[(int) index][1];
                }
                else
                {
                    numArray2[0] = dm_stgslct_n_zone_nodisp_pos_tbl[(int) index][0] -
                                   dm_stgslct_n_zone_disp_pos_tbl[(int) index][0];
                    numArray2[1] = dm_stgslct_n_zone_nodisp_pos_tbl[(int) index][1] -
                                   dm_stgslct_n_zone_disp_pos_tbl[(int) index][1];
                }

                numArray1[0] = numArray2[0] / 16f;
                numArray1[1] = numArray2[1] / 16f;
                main_work.zone_pos[(int) index][0] += numArray1[0];
                main_work.zone_pos[(int) index][1] += numArray1[1];
            }
        }

        this.dmStgSlctSetEmeTableOutEfct(main_work);
    }

    private bool dmStgSlctIsZonePosOutEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.is_final_open == 3)
        {
            if ((main_work.zone_pos[0][0] > (double) dm_stgslct_a_zone_nodisp_pos_tbl[0][0] ||
                 main_work.zone_pos[0][1] > (double) dm_stgslct_a_zone_nodisp_pos_tbl[0][1]) &&
                main_work.cur_zone != 0U ||
                (main_work.zone_pos[1][0] > (double) dm_stgslct_a_zone_nodisp_pos_tbl[1][0] ||
                 main_work.zone_pos[1][1] > (double) dm_stgslct_a_zone_nodisp_pos_tbl[1][1]) &&
                main_work.cur_zone != 1U ||
                ((main_work.zone_pos[2][0] < (double) dm_stgslct_a_zone_nodisp_pos_tbl[2][0] ||
                  main_work.zone_pos[2][1] > (double) dm_stgslct_a_zone_nodisp_pos_tbl[2][1]) &&
                 main_work.cur_zone != 2U ||
                 (main_work.zone_pos[3][0] < (double) dm_stgslct_a_zone_nodisp_pos_tbl[3][0] ||
                  main_work.zone_pos[3][1] < (double) dm_stgslct_a_zone_nodisp_pos_tbl[3][1]) &&
                 main_work.cur_zone != 3U) ||
                ((main_work.zone_pos[4][0] > (double) dm_stgslct_a_zone_nodisp_pos_tbl[4][0] ||
                  main_work.zone_pos[4][1] < (double) dm_stgslct_a_zone_nodisp_pos_tbl[4][1]) &&
                 main_work.cur_zone != 4U ||
                 (main_work.zone_pos[5][0] > (double) dm_stgslct_a_zone_nodisp_pos_tbl[5][0] ||
                  main_work.zone_pos[5][1] > (double) dm_stgslct_a_zone_nodisp_pos_tbl[5][1]) &&
                 main_work.cur_zone != 5U))
                return false;
        }
        else if ((main_work.is_final_open & 2) != 0)
        {
            if ((main_work.zone_pos[0][0] > (double) dm_stgslct_s_zone_nodisp_pos_tbl[0][0] ||
                 main_work.zone_pos[0][1] > (double) dm_stgslct_s_zone_nodisp_pos_tbl[0][1]) &&
                main_work.cur_zone != 0U ||
                (main_work.zone_pos[1][0] > (double) dm_stgslct_s_zone_nodisp_pos_tbl[1][0] ||
                 main_work.zone_pos[1][1] > (double) dm_stgslct_s_zone_nodisp_pos_tbl[1][1]) &&
                main_work.cur_zone != 1U ||
                ((main_work.zone_pos[2][0] < (double) dm_stgslct_s_zone_nodisp_pos_tbl[2][0] ||
                  main_work.zone_pos[2][1] > (double) dm_stgslct_s_zone_nodisp_pos_tbl[2][1]) &&
                 main_work.cur_zone != 2U ||
                 (main_work.zone_pos[3][0] < (double) dm_stgslct_s_zone_nodisp_pos_tbl[3][0] ||
                  main_work.zone_pos[3][1] < (double) dm_stgslct_s_zone_nodisp_pos_tbl[3][1]) &&
                 main_work.cur_zone != 3U) ||
                (main_work.zone_pos[5][0] > (double) dm_stgslct_s_zone_nodisp_pos_tbl[5][0] ||
                 main_work.zone_pos[5][1] < (double) dm_stgslct_s_zone_nodisp_pos_tbl[5][1]) &&
                main_work.cur_zone != 5U)
                return false;
        }
        else if ((main_work.is_final_open & 1) != 0)
        {
            if ((main_work.zone_pos[0][0] > (double) dm_stgslct_f_zone_nodisp_pos_tbl[0][0] ||
                 main_work.zone_pos[0][1] > (double) dm_stgslct_f_zone_nodisp_pos_tbl[0][1]) &&
                main_work.cur_zone != 0U ||
                (main_work.zone_pos[1][0] > (double) dm_stgslct_f_zone_nodisp_pos_tbl[1][0] ||
                 main_work.zone_pos[1][1] > (double) dm_stgslct_f_zone_nodisp_pos_tbl[1][1]) &&
                main_work.cur_zone != 1U ||
                ((main_work.zone_pos[2][0] < (double) dm_stgslct_f_zone_nodisp_pos_tbl[2][0] ||
                  main_work.zone_pos[2][1] > (double) dm_stgslct_f_zone_nodisp_pos_tbl[2][1]) &&
                 main_work.cur_zone != 2U ||
                 (main_work.zone_pos[3][0] < (double) dm_stgslct_f_zone_nodisp_pos_tbl[3][0] ||
                  main_work.zone_pos[3][1] < (double) dm_stgslct_f_zone_nodisp_pos_tbl[3][1]) &&
                 main_work.cur_zone != 3U) ||
                (main_work.zone_pos[4][0] > (double) dm_stgslct_f_zone_nodisp_pos_tbl[4][0] ||
                 main_work.zone_pos[4][1] < (double) dm_stgslct_f_zone_nodisp_pos_tbl[4][1]) &&
                main_work.cur_zone != 4U)
                return false;
        }
        else if
            ((main_work.zone_pos[0][0] > (double) dm_stgslct_n_zone_nodisp_pos_tbl[0][0] ||
              main_work.zone_pos[0][1] > (double) dm_stgslct_n_zone_nodisp_pos_tbl[0][1]) && main_work.cur_zone != 0U ||
             (main_work.zone_pos[1][0] > (double) dm_stgslct_n_zone_nodisp_pos_tbl[1][0] ||
              main_work.zone_pos[1][1] < (double) dm_stgslct_n_zone_nodisp_pos_tbl[1][1]) && main_work.cur_zone != 1U ||
             ((main_work.zone_pos[2][0] < (double) dm_stgslct_n_zone_nodisp_pos_tbl[2][0] ||
               main_work.zone_pos[2][1] > (double) dm_stgslct_n_zone_nodisp_pos_tbl[2][1]) &&
              main_work.cur_zone != 2U ||
              (main_work.zone_pos[3][0] < (double) dm_stgslct_n_zone_nodisp_pos_tbl[3][0] ||
               main_work.zone_pos[3][1] < (double) dm_stgslct_n_zone_nodisp_pos_tbl[3][1]) && main_work.cur_zone != 3U))
            return false;

        main_work.efct_out_flag = 0U;
        return this.dmStgSlctIsEmeTableOutEfctEnd(main_work);
    }

    private void dmStgSlctSetStagePosInEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float[] numArray1 = new float[2];
        float[] numArray2 = new float[2];
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num1;
        if (main_work.act_top_pos_x[(int) num1] > 100.0)
        {
            for (uint index = num1; index < num2; ++index)
            {
                numArray2[0] = -1020f;
                numArray1[0] = numArray2[0] / 16f;
                main_work.act_top_pos_x[(int) index] += numArray1[0];
            }
        }

        this.dmStgSlctSetDecideZonePosOutEfct(main_work);
        this.dmStgSlctSetModeTexInEfct(main_work);
    }

    private bool dmStgSlctIsStagePosInEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num1;
        if (!this.dmStgSlctIsModeTexInEfctEnd(main_work) || main_work.act_top_pos_x[(int) num1] > 100.0 ||
            main_work.zone_pos[(int) main_work.cur_zone][0] >= (double) dm_stgslct_a_zone_nodisp_pos_tbl[0][0])
            return false;
        for (uint index = num1; index < num2; ++index)
            main_work.act_top_pos_x[(int) index] = 100f;
        main_work.zone_pos[(int) main_work.cur_zone][0] = dm_stgslct_a_zone_nodisp_pos_tbl[0][0];
        return true;
    }

    private void dmStgSlctSetStagePosOutEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float[] numArray1 = new float[2];
        float[] numArray2 = new float[2];
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num1;
        for (uint index = num1; index < num2; ++index)
        {
            numArray2[0] = 1020f;
            numArray1[0] = numArray2[0] / 16f;
            main_work.act_top_pos_x[(int) index] += numArray1[0];
        }

        this.dmStgSlctSetModeTexOutEfct(main_work);
    }

    private bool dmStgSlctIsStagePosOutEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num1;
        if (!this.dmStgSlctIsModeTexOutEfctEnd(main_work) || main_work.act_top_pos_x[(int) num1] < 1120.0)
            return false;
        for (uint index = num1; index < num2; ++index)
            main_work.act_top_pos_x[(int) index] = 1120f;
        return true;
    }

    private void dmStgSlctSetZonePosInEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float[] numArray1 = new float[2];
        float[] numArray2 = new float[2];
        for (int index = 0; index < 6; ++index)
        {
            if (main_work.is_final_open == 3)
            {
                numArray2[0] = dm_stgslct_a_zone_disp_pos_tbl[index][0] - dm_stgslct_a_zone_nodisp_pos_tbl[index][0];
                numArray2[1] = dm_stgslct_a_zone_disp_pos_tbl[index][1] - dm_stgslct_a_zone_nodisp_pos_tbl[index][1];
            }
            else if ((main_work.is_final_open & 2) != 0)
            {
                numArray2[0] = dm_stgslct_s_zone_disp_pos_tbl[index][0] - dm_stgslct_s_zone_nodisp_pos_tbl[index][0];
                numArray2[1] = dm_stgslct_s_zone_disp_pos_tbl[index][1] - dm_stgslct_s_zone_nodisp_pos_tbl[index][1];
            }
            else if ((main_work.is_final_open & 1) != 0)
            {
                numArray2[0] = dm_stgslct_f_zone_disp_pos_tbl[index][0] - dm_stgslct_f_zone_nodisp_pos_tbl[index][0];
                numArray2[1] = dm_stgslct_f_zone_disp_pos_tbl[index][1] - dm_stgslct_f_zone_nodisp_pos_tbl[index][1];
            }
            else
            {
                numArray2[0] = dm_stgslct_n_zone_disp_pos_tbl[index][0] - dm_stgslct_n_zone_nodisp_pos_tbl[index][0];
                numArray2[1] = dm_stgslct_n_zone_disp_pos_tbl[index][1] - dm_stgslct_n_zone_nodisp_pos_tbl[index][1];
            }

            numArray1[0] = numArray2[0] / 16f;
            numArray1[1] = numArray2[1] / 16f;
            main_work.zone_pos[index][0] += numArray1[0];
            main_work.zone_pos[index][1] += numArray1[1];
        }

        this.dmStgSlctSetEmeTableInEfct(main_work);
        ++main_work.timer;
    }

    private bool dmStgSlctIsZonePosInEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.is_final_open == 3)
        {
            if (main_work.zone_pos[0][0] < (double) dm_stgslct_a_zone_disp_pos_tbl[0][0] ||
                main_work.zone_pos[0][1] < (double) dm_stgslct_a_zone_disp_pos_tbl[0][1] ||
                (main_work.zone_pos[1][0] < (double) dm_stgslct_a_zone_disp_pos_tbl[1][0] ||
                 main_work.zone_pos[1][1] < (double) dm_stgslct_a_zone_disp_pos_tbl[1][1]) ||
                (main_work.zone_pos[2][0] > (double) dm_stgslct_a_zone_disp_pos_tbl[2][0] ||
                 main_work.zone_pos[2][1] < (double) dm_stgslct_a_zone_disp_pos_tbl[2][1] ||
                 (main_work.zone_pos[3][0] > (double) dm_stgslct_a_zone_disp_pos_tbl[3][0] ||
                  main_work.zone_pos[3][1] > (double) dm_stgslct_a_zone_disp_pos_tbl[3][1])) ||
                (main_work.zone_pos[4][0] < (double) dm_stgslct_a_zone_disp_pos_tbl[4][0] ||
                 main_work.zone_pos[4][1] < (double) dm_stgslct_a_zone_disp_pos_tbl[4][1] ||
                 (main_work.zone_pos[5][0] < (double) dm_stgslct_a_zone_disp_pos_tbl[5][0] ||
                  main_work.zone_pos[5][1] > (double) dm_stgslct_a_zone_disp_pos_tbl[5][1])))
                return false;
        }
        else if ((main_work.is_final_open & 2) != 0)
        {
            if (main_work.zone_pos[0][0] < (double) dm_stgslct_s_zone_disp_pos_tbl[0][0] ||
                main_work.zone_pos[0][1] < (double) dm_stgslct_s_zone_disp_pos_tbl[0][1] ||
                (main_work.zone_pos[1][0] < (double) dm_stgslct_s_zone_disp_pos_tbl[1][0] ||
                 main_work.zone_pos[1][1] < (double) dm_stgslct_s_zone_disp_pos_tbl[1][1]) ||
                (main_work.zone_pos[2][0] > (double) dm_stgslct_s_zone_disp_pos_tbl[2][0] ||
                 main_work.zone_pos[2][1] < (double) dm_stgslct_s_zone_disp_pos_tbl[2][1] ||
                 (main_work.zone_pos[3][0] > (double) dm_stgslct_s_zone_disp_pos_tbl[3][0] ||
                  main_work.zone_pos[3][1] > (double) dm_stgslct_s_zone_disp_pos_tbl[3][1])) ||
                (main_work.zone_pos[5][0] < (double) dm_stgslct_s_zone_disp_pos_tbl[5][0] ||
                 main_work.zone_pos[5][1] > (double) dm_stgslct_s_zone_disp_pos_tbl[5][1]))
                return false;
        }
        else if ((main_work.is_final_open & 1) != 0)
        {
            if (main_work.zone_pos[0][0] < (double) dm_stgslct_f_zone_disp_pos_tbl[0][0] ||
                main_work.zone_pos[0][1] < (double) dm_stgslct_f_zone_disp_pos_tbl[0][1] ||
                (main_work.zone_pos[1][0] < (double) dm_stgslct_f_zone_disp_pos_tbl[1][0] ||
                 main_work.zone_pos[1][1] < (double) dm_stgslct_f_zone_disp_pos_tbl[1][1]) ||
                (main_work.zone_pos[2][0] > (double) dm_stgslct_f_zone_disp_pos_tbl[2][0] ||
                 main_work.zone_pos[2][1] < (double) dm_stgslct_f_zone_disp_pos_tbl[2][1] ||
                 (main_work.zone_pos[3][0] > (double) dm_stgslct_f_zone_disp_pos_tbl[3][0] ||
                  main_work.zone_pos[3][1] > (double) dm_stgslct_f_zone_disp_pos_tbl[3][1])) ||
                (main_work.zone_pos[4][0] < (double) dm_stgslct_f_zone_disp_pos_tbl[4][0] ||
                 main_work.zone_pos[4][1] > (double) dm_stgslct_f_zone_disp_pos_tbl[4][1]))
                return false;
        }
        else if (main_work.zone_pos[0][0] < (double) dm_stgslct_n_zone_disp_pos_tbl[0][0] ||
                 main_work.zone_pos[0][1] < (double) dm_stgslct_n_zone_disp_pos_tbl[0][1] ||
                 (main_work.zone_pos[1][0] < (double) dm_stgslct_n_zone_disp_pos_tbl[1][0] ||
                  main_work.zone_pos[1][1] > (double) dm_stgslct_n_zone_disp_pos_tbl[1][1]) ||
                 (main_work.zone_pos[2][0] > (double) dm_stgslct_n_zone_disp_pos_tbl[2][0] ||
                  main_work.zone_pos[2][1] < (double) dm_stgslct_n_zone_disp_pos_tbl[2][1] ||
                  (main_work.zone_pos[3][0] > (double) dm_stgslct_n_zone_disp_pos_tbl[3][0] ||
                   main_work.zone_pos[3][1] > (double) dm_stgslct_n_zone_disp_pos_tbl[3][1])))
            return false;

        return this.dmStgSlctIsEmeTableInEfctEnd(main_work);
    }

    private void dmStgSlctSetDecideZonePosOutEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float[] numArray1 = new float[2];
        float[] numArray2 = new float[2]
        {
            main_work.is_final_open != 3
                ? ((main_work.is_final_open & 2) == 0
                    ? ((main_work.is_final_open & 1) == 0
                        ? dm_stgslct_n_zone_nodisp_pos_tbl[0][0] -
                          dm_stgslct_n_zone_disp_pos_tbl[(int) main_work.cur_zone][0]
                        : dm_stgslct_f_zone_nodisp_pos_tbl[0][0] -
                          dm_stgslct_f_zone_disp_pos_tbl[(int) main_work.cur_zone][0])
                    : dm_stgslct_s_zone_nodisp_pos_tbl[0][0] -
                      dm_stgslct_s_zone_disp_pos_tbl[(int) main_work.cur_zone][0])
                : dm_stgslct_a_zone_nodisp_pos_tbl[0][0] - dm_stgslct_a_zone_disp_pos_tbl[(int) main_work.cur_zone][0],
            0.0f
        };
        numArray1[0] = numArray2[0] / 16f;
        main_work.zone_pos[(int) main_work.cur_zone][0] += numArray1[0];
    }

    private void dmStgSlctSetStageZoneChangeEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float[] numArray1 = new float[2];
        float[] numArray2 = new float[2];
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.chng_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.chng_zone][1] + num1;
        for (uint index = num1; index < num2; ++index)
        {
            numArray2[0] = main_work.act_move_dest[0] - main_work.act_move_src[0];
            numArray1[0] = numArray2[0] / 16f;
            main_work.act_top_pos_x[(int) index] += numArray1[0];
        }

        uint num3 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num4 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num3;
        for (uint index = num3; index < num4; ++index)
        {
            numArray2[1] = main_work.act_move_dest[1] - main_work.act_move_src[1];
            numArray1[1] = numArray2[1] / 16f;
            main_work.act_top_pos_x[(int) index] += numArray1[1];
        }
    }

    private bool dmStgSlctIsStageZoneChangeEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        byte num1 = 0;
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.chng_zone][0];
        uint num3 = dm_stgslct_zone_act_num_tbl[(int) main_work.chng_zone][1] + num2;
        if (main_work.act_move_dest[0] > 0.0)
        {
            if (main_work.act_top_pos_x[(int) num2] >= (double) main_work.act_move_dest[0])
            {
                for (uint index = num2; index < num3; ++index)
                    main_work.act_top_pos_x[(int) index] = main_work.act_move_dest[0];
                num1 |= 1;
            }

            uint num4 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
            uint num5 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num4;
            if (main_work.act_top_pos_x[(int) num4] >= (double) main_work.act_move_dest[1])
            {
                for (uint index = num4; index < num5; ++index)
                    main_work.act_top_pos_x[(int) index] = main_work.act_move_dest[1];
                num1 |= 2;
            }
        }
        else if (main_work.act_move_dest[0] < 0.0)
        {
            if (main_work.act_top_pos_x[(int) num2] <= (double) main_work.act_move_dest[0])
            {
                for (uint index = num2; index < num3; ++index)
                    main_work.act_top_pos_x[(int) index] = main_work.act_move_dest[0];
                num1 |= 1;
            }

            uint num4 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
            uint num5 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num4;
            if (main_work.act_top_pos_x[(int) num4] <= (double) main_work.act_move_dest[1])
            {
                for (uint index = num4; index < num5; ++index)
                    main_work.act_top_pos_x[(int) index] = main_work.act_move_dest[1];
                num1 |= 2;
            }
        }

        return num1 == 3;
    }

    private void dmStgSlctSetStageVrtclChangeEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float[] numArray1 = new float[2];
        float[] numArray2 = new float[2];
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num1;
        for (uint index = num1; index < num2; ++index)
        {
            numArray2[1] = main_work.act_move_pos_dst[(int) index] - main_work.act_move_pos_src[(int) index];
            numArray1[1] = numArray2[1] / 12f;
            main_work.act_top_pos_y[(int) index] += numArray1[1];
        }
    }

    private bool dmStgSlctIsStageVrtclChangeEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num1;
        if (main_work.timer < 12.0)
            return false;
        for (uint index = num1; index < num2; ++index)
            main_work.act_top_pos_y[(int) index] = main_work.act_move_pos_dst[(int) index];
        return true;
    }

    private void dmStgSlctSetStageCrsrChangeEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float num = (main_work.crsr_move_dst - main_work.crsr_move_src) / 8f;
        main_work.crsr_pos_y += num;
    }

    private bool dmStgSlctIsStageCrsrChangeEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.timer < 8.0)
            return false;
        main_work.crsr_pos_y = main_work.crsr_move_dst;
        return true;
    }

    private void dmStgSlctSetWinOpenEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (0.0 <= main_work.win_timer && main_work.win_timer < 1.0)
            DmSoundPlaySE("Window");
        if (main_work.win_timer > 8.0)
        {
            main_work.flag |= 16U;
            main_work.win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int) index] = 1f;
        }
        else
            ++main_work.win_timer;

        for (uint index = 0; index < 2U; ++index)
        {
            main_work.win_size_rate[(int) index] = main_work.win_timer == 0.0 ? 1f : main_work.win_timer / 8f;
            if (main_work.win_size_rate[(int) index] > 1.0)
                main_work.win_size_rate[(int) index] = 1f;
        }
    }

    private void dmStgSlctSetWinCloseEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
            main_work.win_size_rate[(int) index] = main_work.win_timer == 0.0 ? 0.0f : main_work.win_timer / 8f;
        if (main_work.win_timer < 0.0)
        {
            main_work.flag |= 16U;
            main_work.win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int) index] = 0.0f;
        }
        else
            --main_work.win_timer;
    }

    private void dmStgSlctSetActChngZonePosInit(DMS_STGSLCT_MAIN_WORK main_work, int diff)
    {
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.chng_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.chng_zone][1] + num1;
        for (uint index = num1; index < num2; ++index)
            main_work.act_top_pos_x[(int) index] = 100f;
        main_work.act_move_src[0] = main_work.act_top_pos_x[(int) num1];
        uint num3 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num4 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num3;
        for (uint index = num3; index < num4; ++index)
        {
            if (diff > 0)
            {
                main_work.act_top_pos_x[(int) index] = 1120f;
                main_work.act_top_pos_y[(int) index] = dm_stgslct_act_disp_y_pos_tbl[(int) index];
            }
            else
            {
                main_work.act_top_pos_x[(int) index] = -1120f;
                main_work.act_top_pos_y[(int) index] = dm_stgslct_act_disp_y_pos_tbl[(int) index];
            }
        }

        main_work.act_move_src[1] = main_work.act_top_pos_x[(int) num3];
    }

    private void dmStgSlctSetFocusChangeEfctData(DMS_STGSLCT_MAIN_WORK main_work)
    {
        int diff = 0;
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = num1 + dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
        int num3 = (int) dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1];
        if (((int) main_work.flag & 2048) != 0)
            diff = -1;
        else if (((int) main_work.flag & 4096) != 0)
            diff = 1;
        main_work.prev_stage = main_work.cur_stage;
        main_work.prev_disp_no = main_work.focus_disp_no;
        if (((int) main_work.flag & 64) != 0)
        {
            if (main_work.cur_zone == 4U)
            {
                for (uint index = num1; index < num2; ++index)
                {
                    main_work.act_move_pos_src[(int) index] = main_work.act_top_pos_y[(int) index];
                    main_work.act_move_pos_dst[(int) index] = dm_stgslct_act_disp_y_pos_tbl[(int) index];
                }
            }
            else
            {
                for (uint index = num1; index < num2; ++index)
                {
                    main_work.act_move_pos_src[(int) index] = main_work.act_top_pos_y[(int) index];
                    main_work.act_move_pos_dst[(int) index] =
                        this.dm_stgslct_act_tab_disp_y_pos_tbl[main_work.focus_disp_no];
                }
            }

            if (((int) main_work.flag & 2048) != 0 && main_work.prev_disp_no == 0 && main_work.focus_disp_no == 3)
                main_work.flag |= 128U;
            if (((int) main_work.flag & 4096) != 0 && main_work.prev_disp_no == 3 && main_work.focus_disp_no == 0)
                main_work.flag |= 128U;
            if (main_work.focus_disp_no == 3 && main_work.prev_disp_no == 0 ||
                main_work.focus_disp_no == 0 && main_work.prev_disp_no == 3)
            {
                main_work.crsr_prev_idx = main_work.crsr_idx;
                int prevDispNo = main_work.prev_disp_no;
                main_work.crsr_idx = this.dmStgSlctGetRevisedStageCrsrNo(main_work.crsr_idx, diff,
                    (int) main_work.cur_zone, main_work.prev_disp_no);
                main_work.crsr_move_src = this.dm_stgslct_act_crsr_disp_y_pos_tbl[main_work.crsr_prev_idx];
                main_work.crsr_move_dst = this.dm_stgslct_act_crsr_disp_y_pos_tbl[main_work.crsr_idx];
                main_work.flag &= 4294967167U;
            }
            else if ((((int) main_work.flag & 2048) == 0 || main_work.focus_disp_no != 0 ||
                      main_work.prev_disp_no != 0) && (((int) main_work.flag & 4096) == 0 ||
                                                       main_work.focus_disp_no != 3 || main_work.prev_disp_no != 3))
                main_work.flag &= 4294967167U;
        }

        if (((int) main_work.flag & 128) != 0)
        {
            main_work.crsr_prev_idx = main_work.crsr_idx;
            int prevDispNo = main_work.prev_disp_no;
            main_work.crsr_idx = this.dmStgSlctGetRevisedStageCrsrNo(main_work.crsr_idx, diff, (int) main_work.cur_zone,
                main_work.prev_disp_no);
            main_work.crsr_move_src = this.dm_stgslct_act_crsr_disp_y_pos_tbl[main_work.crsr_prev_idx];
            main_work.crsr_move_dst = this.dm_stgslct_act_crsr_disp_y_pos_tbl[main_work.crsr_idx];
        }

        main_work.cur_stage = (int) (main_work.crsr_idx + main_work.focus_disp_no + num1);
    }

    private int dmStgSlctGetRevisedStageVrtclNo(int idx, int diff, int zone_no, int crsr_idx)
    {
        UNREFERENCED_PARAMETER(zone_no);
        int num = idx + diff;
        if (num < 0)
            num = crsr_idx != 0 ? 0 : 3;
        if (num >= 4)
            num = crsr_idx != 3 ? 3 : 0;
        return num;
    }

    private int dmStgSlctGetRevisedStageCrsrNo(int idx, int diff, int zone_no, int disp_act)
    {
        uint num1 = 0;
        num1 = dm_stgslct_zone_act_num_tbl[zone_no][0];
        int num2 = (int) dm_stgslct_zone_act_num_tbl[zone_no][1];
        int num3 = idx + diff;
        int num4 = (int) dm_stgslct_zone_act_num_tbl[zone_no][1] - 4;
        if (num4 < 0)
            num4 = 0;
        if (num3 < 0)
            num3 = disp_act != 0 ? 0 : 3;
        if (num3 > 3)
            num3 = disp_act != num4 ? 3 : 0;
        return num3;
    }

    private int dmStgSlctGetRevisedZoneNo(int idx, int diff, int is_final_open, int is_spe_open)
    {
        int num;
        if (is_final_open != 0)
        {
            num = idx + diff;
            if (is_spe_open != 0)
            {
                if (num < 0)
                    num = 5;
                if (num >= 6)
                    num = 0;
            }
            else
            {
                if (num < 0)
                    num = 4;
                if (num > 4)
                    num = 0;
            }
        }
        else if (is_spe_open != 0)
        {
            num = idx + diff;
            if (num == 4)
                num = diff <= 0 ? 3 : 5;
            if (num < 0)
                num = 5;
            if (num >= 6)
                num = 0;
        }
        else
        {
            num = idx + diff;
            if (num < 0)
                num = 3;
            if (num > 3)
                num = 0;
        }

        return num;
    }

    private void dmStgSlctSetTableActiveInfo(DMS_STGSLCT_MAIN_WORK main_work, uint cnt)
    {
        if (main_work.is_clear_stage[(int) cnt] < 0)
        {
            for (uint index = 58; index <= 60U; ++index)
                AoActSetFrame(main_work.act[(int) index], 1f);
            AoActSetFrame(main_work.act[61], 17f);
            AoActSetFrame(main_work.act[65], 17f);
        }
        else
        {
            for (uint index = 58; index <= 60U; ++index)
                AoActSetFrame(main_work.act[(int) index], 0.0f);
            AoActSetFrame(main_work.act[61], this.dm_stgslct_disp_msg_id_table_tbl[(int) cnt]);
            AoActSetFrame(main_work.act[65], this.dm_stgslct_disp_msg_id_table_tbl[(int) cnt]);
            if (main_work.hi_score[(int) cnt] == 1000000000 && main_work.cur_game_mode == 1U)
            {
                for (uint index = 58; index <= 60U; ++index)
                    AoActSetFrame(main_work.act[(int) index], 1f);
            }
            else
            {
                for (uint index = 58; index <= 60U; ++index)
                    AoActSetFrame(main_work.act[(int) index], 0.0f);
            }
        }
    }

    private bool dmStgSlctIsCanSelectAct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.is_clear_stage[main_work.cur_stage] == -1)
        {
            main_work.flag &= 4294967291U;
            return false;
        }

        return main_work.hi_score[main_work.cur_stage] != 1000000000 || main_work.cur_game_mode != 1U;
    }

    private void dmStgSlctSetObiEfctPos(DMS_STGSLCT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
        {
            if (main_work.obi_pos[(int) index] < -1120.0)
                main_work.obi_pos[(int) index] = 1120f;
            main_work.obi_pos[(int) index] += -3f;
        }
    }

    private void dmStgSlctMakeVertexAct(
        DMS_STGSLCT_MAIN_WORK main_work,
        AMS_PARAM_DRAW_PRIMITIVE param)
    {
        if (main_work.proc_menu_update == null || main_work.proc_menu_update ==
            new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStageSelectIdle))
            return;
        int num = main_work.proc_menu_update ==
                  new DMS_STGSLCT_MAIN_WORK._proc_menu_update_(this.dmStgSlctProcStageSelectChngVrtclAct)
            ? 1
            : 0;
    }

    private void dmStgSlctDrawVertexAct(AMS_TCB tcb_p)
    {
        AoActDrawPre();
        amDrawExecCommand(120U);
        amDrawEndScene();
    }

    private void dmStgSlctSetEmeTableInEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float num = -180f / 16f;
        main_work.chaos_eme_pos_y += num;
    }

    private bool dmStgSlctIsEmeTableInEfctEnd(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.chaos_eme_pos_y > 0.0)
            return false;
        main_work.chaos_eme_pos_y = 0.0f;
        return true;
    }

    private void dmStgSlctSetEmeTableOutEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float num = 180f / 16f;
        main_work.chaos_eme_pos_y += num;
    }

    private bool dmStgSlctIsEmeTableOutEfctEnd(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.chaos_eme_pos_y < 180.0)
            return false;
        main_work.chaos_eme_pos_y = 180f;
        return true;
    }

    private void dmStgSlctSetModeTexInEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float num = -180f / 16f;
        main_work.mode_tex_pos_y += num;
    }

    private bool dmStgSlctIsModeTexInEfctEnd(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.mode_tex_pos_y > 0.0)
            return false;
        main_work.mode_tex_pos_y = 0.0f;
        return true;
    }

    private void dmStgSlctSetModeTexOutEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        float num = 180f / 16f;
        main_work.mode_tex_pos_y += num;
    }

    private bool dmStgSlctIsModeTexOutEfctEnd(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.mode_tex_pos_y < 180.0)
            return false;
        main_work.mode_tex_pos_y = 180f;
        return true;
    }

    private void dmStgSlctSetBgFadeEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        int a1 = main_work.bg_fade.a;
        if ((int) main_work.cur_zone != (int) main_work.cur_bg_id)
        {
            main_work.flag |= 2097152U;
            main_work.next_bg_id = main_work.cur_zone;
        }

        int a2;
        if (((int) main_work.flag & 2097152) != 0)
        {
            if (a1 < byte.MaxValue)
            {
                a2 = a1 + 20;
            }
            else
            {
                a2 = byte.MaxValue;
                main_work.cur_bg_id = main_work.next_bg_id;
                main_work.flag &= 4292870143U;
            }
        }
        else
            a2 = a1 <= 0 ? 0 : a1 - 20;

        int num = MTM_MATH_CLIP(a2, 0, byte.MaxValue);
        main_work.bg_fade.a = (byte) num;
    }

    private void dmStgSlctSetZoneScrChangeEfct(DMS_STGSLCT_MAIN_WORK main_work)
    {
        if (main_work.zone_scr_id > 360U)
            main_work.zone_scr_id = 0U;
        else
            ++main_work.zone_scr_id;
    }

    private int dmStgSlctSetNextFocusAct(DMS_STGSLCT_MAIN_WORK main_work, int set_stage_id)
    {
        if (set_stage_id >= 21)
            return (int) (uint) (dm_stgslct_zone_act_num_tbl[5][0] + (ulong) (set_stage_id - 21));
        if (set_stage_id >= 16)
            return 16;
        uint num1 = (uint) set_stage_id;
        if (!g_gs_main_sys_info.is_first_play || g_gs_main_sys_info.game_mode == 1 ||
            ((int) main_work.flag & 16777216) != 0)
            return (int) num1;
        int num2 = (int) dm_stgslct_act_zone_no_tbl[(int) num1];
        uint num3 = dm_stgslct_zone_array_act_tbl[(int) num1];
        for (uint index = num1 - num3; index < (uint) ((int) num1 - (int) num3 + 4); ++index)
        {
            if (!GsMainSysIsStageClear((int) index))
                return (int) index;
        }

        for (uint index1 = 0; index1 < 4U; ++index1)
        {
            uint num4 = index1;
            uint num5 = dm_stgslct_zone_act_num_tbl[(int) num4][0];
            for (int index2 = 0; index2 < 4; ++index2)
            {
                if (!GsMainSysIsStageClear((int) (num5 + index2)))
                    return (int) (num5 + index2);
            }
        }

        return 16;
    }

    private bool dmStgSlctIsBossFocus(DMS_STGSLCT_MAIN_WORK main_work)
    {
        bool flag = false;
        switch (main_work.cur_zone)
        {
            case 4:
            case 5:
                return flag;
            default:
                if (main_work.cur_game_mode == 0U)
                {
                    uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
                    uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num1;
                    uint num3 = 0;
                    for (uint index = num1; index < num2; ++index)
                    {
                        if (1 == main_work.is_clear_stage[(int) index])
                            ++num3;
                    }

                    if (3U == num3)
                    {
                        flag = true;
                        goto case 4;
                    }
                    else
                        goto case 4;
                }
                else
                    goto case 4;
        }
    }

    private void dmStgSlctStageSelectChngZoneSetInZoneScroll(DMS_STGSLCT_MAIN_WORK main_work)
    {
        this.dmStgSlctStageSelectChngZoneSetInZoneScroll(main_work, 0);
    }

    private void dmStgSlctStageSelectChngZoneSetInZoneScroll(
        DMS_STGSLCT_MAIN_WORK main_work,
        int stage)
    {
        uint num1 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][0];
        uint num2 = dm_stgslct_zone_act_num_tbl[(int) main_work.cur_zone][1] + num1;
        switch (main_work.cur_zone)
        {
            case 4:
                main_work.focus_disp_no = 0;
                for (uint index = num1; index < num2; ++index)
                    main_work.act_top_pos_y[(int) index] = dm_stgslct_act_disp_y_pos_tbl[(int) index];
                break;
            case 5:
                int a = stage - 18;
                main_work.focus_disp_no = MTM_MATH_CLIP(a, 0, 4);
                for (uint index = num1; index < num2; ++index)
                    main_work.act_top_pos_y[(int) index] =
                        this.dm_stgslct_act_tab_disp_y_pos_tbl[main_work.focus_disp_no];
                break;
            default:
                if (this.dmStgSlctIsBossFocus(main_work))
                {
                    main_work.focus_disp_no = 1;
                }
                else
                {
                    switch (stage)
                    {
                        case 3:
                        case 7:
                        case 11:
                        case 15:
                            main_work.focus_disp_no = 1;
                            break;
                        default:
                            main_work.focus_disp_no = 0;
                            break;
                    }
                }

                for (uint index = num1; index < num2; ++index)
                    main_work.act_top_pos_y[(int) index] =
                        this.dm_stgslct_act_tab_disp_y_pos_tbl[main_work.focus_disp_no];
                break;
        }
    }
}