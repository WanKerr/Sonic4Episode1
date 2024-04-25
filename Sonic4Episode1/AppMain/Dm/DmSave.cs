using gs.backup;

public partial class AppMain
{
    public static void DmSaveStart(uint disp_flag, bool is_first_save, bool is_task_draw)
    {
        switch (SyGetEvtInfo().cur_evt_id)
        {
            case 6:
                break;
            case 9:
                break;
            case 11:
                break;
            default:
                if (((int) disp_flag & 4) != 0 && GsTrialIsTrial())
                    break;
                dm_save_mgr.Clear();
                dm_save_mgr_p = dm_save_mgr;
                dm_save_msg_flag = disp_flag;
                dm_save_first_save = is_first_save;
                dm_save_is_task_draw = is_task_draw;
                dm_save_is_snd_build = false;
                dmSaveInit();
                break;
        }
    }

    private static void DmSaveAttenMsgStart()
    {
        int curEvtId = SyGetEvtInfo().cur_evt_id;
        dm_save_mgr.Clear();
        dm_save_mgr_p = dm_save_mgr;
        dm_save_msg_flag = 2U;
        dm_save_first_save = false;
        dm_save_is_task_draw = false;
        dm_save_is_snd_build = true;
        dmSaveInit();
    }

    private static void DmSaveMenuStart(bool is_task_draw, bool is_snd_build)
    {
        switch (SyGetEvtInfo().cur_evt_id)
        {
            case 6:
                break;
            case 9:
                break;
            case 11:
                break;
            default:
                if (!AoAccountIsCurrentEnable())
                    break;
                dmSaveSetSysDataForBackup();
                if (GsTrialIsTrial() || dmSaveIsSaveNecessary())
                    break;
                dm_save_mgr.Clear();
                dm_save_mgr_p = dm_save_mgr;
                dm_save_msg_flag = 4U;
                dm_save_first_save = false;
                dm_save_is_task_draw = is_task_draw;
                dm_save_is_snd_build = is_snd_build;
                dmSaveInit();
                break;
        }
    }

    public static bool DmSaveIsExit()
    {
        return dm_save_mgr_p == null || dm_save_mgr_p.tcb == null;
    }

    private static bool DmSaveIsDraw()
    {
        return dm_save_draw_reserve;
    }

    private static void dmSaveInit()
    {
        dm_save_mgr_p.tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(dmSaveProcMain), new GSF_TASK_PROCEDURE(dmSaveDest), 0U, (ushort) short.MaxValue, 8192U, 0, () => new DMS_SAVE_MAIN_WORK(), "SAVE_TASK");
        DMS_SAVE_MAIN_WORK work = (DMS_SAVE_MAIN_WORK) dm_save_mgr_p.tcb.work;
        dm_save_disp_flag = 0U;
        dm_save_is_draw_state = 0U;
        dm_save_win_mode = 0;
        dm_save_draw_reserve = false;
        for (int index = 0; index < 2; ++index)
        {
            dm_save_win_size_rate[index] = 0.0f;
            dm_save_cmn_tex[index] = null;
        }

        for (int index = 0; index < 6; ++index)
            dm_save_act[index] = null;
        work.announce_flag = dm_save_msg_flag;
        work.draw_state = AoActSysGetDrawStateEnable() ? 1U : 0U;
        dm_save_draw_state = work.draw_state == 0U ? 0U : AoActSysGetDrawState();
        for (int index = 0; index < 2; ++index)
            dm_save_cmn_tex[index] = null;
        dm_save_is_draw_state = work.draw_state;
        work.proc_menu_update = new _saveproc_input_update(dmSaveLoadFontData);
    }

    private static void dmSaveSetSysDataForBackup()
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        SSystem instance = SSystem.CreateInstance();
        instance.Killed = mainSysInfo.ene_kill_count;
        instance.PlayerStock = mainSysInfo.rest_player_num;
    }

    private static bool dmSaveIsSaveNecessary()
    {
        return false;
    }

    private static void dmSaveProcMain(MTS_TASK_TCB tcb)
    {
        DMS_SAVE_MAIN_WORK work = (DMS_SAVE_MAIN_WORK) tcb.work;
        if (((int) work.flag & 1) != 0)
        {
            mtTaskClearTcb(tcb);
            dm_save_disp_flag = 0U;
            dm_save_is_draw_state = 0U;
            dm_save_win_mode = 0;
            dm_save_is_task_draw = false;
            for (int index = 0; index < 2; ++index)
                dm_save_win_size_rate[index] = 0.0f;
            dm_save_mgr_p = null;
        }

        if (((int) work.flag & int.MinValue) != 0 && !AoAccountIsCurrentEnable())
        {
            work.proc_menu_update = new _saveproc_input_update(dmSaveProcStopDraw);
            work.proc_input = null;
            work.proc_draw = null;
            dm_save_draw_reserve = false;
            work.flag &= int.MaxValue;
        }
        else
        {
            if (work.proc_menu_update != null)
                work.proc_menu_update(work);
            if (work.proc_draw == null || AoSysMsgIsShow())
                return;
            work.proc_draw();
        }
    }

    private static void dmSaveDest(MTS_TASK_TCB tcb)
    {
    }

    private static void dmSaveLoadFontData(DMS_SAVE_MAIN_WORK main_work)
    {
        if (SyGetEvtInfo().cur_evt_id == 10)
            GsFontBuild(false);
        else
            GsFontBuild();
        main_work.proc_menu_update = new _saveproc_input_update(dmSaveIsLoadFontData);
    }

    private static void dmSaveIsLoadFontData(DMS_SAVE_MAIN_WORK main_work)
    {
        if (!GsFontIsBuilded())
            return;
        main_work.proc_menu_update = new _saveproc_input_update(dmSaveLoadRequest);
    }

    private static void dmSaveLoadRequest(DMS_SAVE_MAIN_WORK main_work)
    {
        main_work.arc_cmn_amb_fs[0] = amFsReadBackground(DMD_MANUAL_CMN_DATA_FILENAME);
        main_work.arc_cmn_amb_fs[1] = amFsReadBackground(dm_save_menu_cmn_lng_amb_name_tbl[GsEnvGetLanguage()]);
        main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcLoadWait);
    }

    private static void dmSaveProcLoadWait(DMS_SAVE_MAIN_WORK main_work)
    {
        if (dmSaveIsDataLoad(main_work) == 0)
            return;
        for (int index = 0; index < 2; ++index)
        {
            main_work.arc_cmn_amb[index] = readAMBFile(main_work.arc_cmn_amb_fs[index]);
            main_work.arc_cmn_amb_fs[index] = null;
            main_work.cmn_ama[index] = readAMAFile(amBindGet(main_work.arc_cmn_amb[index], 0));
            string sPath;
            main_work.cmn_amb[index] = readAMBFile(amBindGet(main_work.arc_cmn_amb[index], 1, out sPath));
            main_work.cmn_amb[index].dir = sPath;
            amFsClearRequest(main_work.arc_cmn_amb_fs[index]);
            main_work.arc_cmn_amb_fs[index] = null;
            AoTexBuild(main_work.cmn_tex[index], main_work.cmn_amb[index]);
            AoTexLoad(main_work.cmn_tex[index]);
        }

        if (dm_save_is_snd_build)
            DmSoundBuild();
        main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcTexBuildWait);
    }

    private static void dmSaveProcTexBuildWait(DMS_SAVE_MAIN_WORK main_work)
    {
        if (dmSaveIsTexLoad(main_work) != 1)
            return;
        for (int index = 0; index < 2; ++index)
            dm_save_cmn_tex[index] = main_work.cmn_tex[index];
        main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcCreateAct);
    }

    private static void dmSaveProcCreateAct(DMS_SAVE_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 6U; ++index)
        {
            A2S_AMA_HEADER ama;
            AOS_TEXTURE tex;
            if (index >= 1U)
            {
                ama = main_work.cmn_ama[1];
                tex = main_work.cmn_tex[1];
            }
            else
            {
                ama = main_work.cmn_ama[0];
                tex = main_work.cmn_tex[0];
            }

            AoActSetTexture(AoTexGetTexList(tex));
            main_work.act[(int) index] = AoActCreate(ama, g_dm_act_id_tbl[(int) index]);
            dm_save_act[(int) index] = main_work.act[(int) index];
        }

        if (((int) dm_save_msg_flag & 4) != 0)
            main_work.flag |= 2147483648U;
        main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowNodispIdle);
    }

    private static void dmSaveProcWindowNodispIdle(DMS_SAVE_MAIN_WORK main_work)
    {
        if (((int) main_work.flag & 8) != 0 || main_work.announce_flag != 0U)
        {
            main_work.win_timer = 0;
            for (uint index = 0; index < 4U; ++index)
            {
                if (((int) main_work.announce_flag & 1 << (int) index) != 0)
                {
                    dm_save_win_mode = main_work.win_mode = (int) index;
                    break;
                }
            }

            if (main_work.win_mode == 0)
            {
                DmCmnBackupLoad();
                main_work.proc_input = null;
                main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowOpenWaitLoadIdle);
            }
            else if (main_work.win_mode == 1)
            {
                main_work.proc_input = new _saveproc_input_update(dmSaveInputProcWindow);
                main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowOpenEfct);
                main_work.proc_draw = new _saveproc_draw(DmSaveWinSelectDraw);
                dm_save_draw_reserve = true;
                DmSoundPlaySE("Window");
            }
            else if (main_work.win_mode == 2)
            {
                DmCmnBackupSave(dm_save_first_save, false, false);
                main_work.proc_input = null;
                main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowOpenWaitIdle);
                main_work.proc_draw = null;
                dm_save_draw_reserve = false;
            }
            else if (main_work.win_mode == 3)
            {
                DmCmnBackupSave(dm_save_first_save, true, false);
                main_work.proc_input = null;
                main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowOpenWaitIdle);
                main_work.proc_draw = null;
                dm_save_draw_reserve = false;
            }

            main_work.flag &= 4294967287U;
        }
        else
        {
            main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcStopDraw);
            dm_save_draw_reserve = false;
        }
    }

    private static void dmSaveProcWindowOpenWaitLoadIdle(DMS_SAVE_MAIN_WORK main_work)
    {
        main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowOpenEfct);
        main_work.proc_draw = new _saveproc_draw(DmSaveWinSelectDraw);
        DmSoundPlaySE("Window");
        dm_save_draw_reserve = true;
    }

    private static void dmSaveProcWindowOpenWaitIdle(DMS_SAVE_MAIN_WORK main_work)
    {
        main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowOpenEfct);
        main_work.proc_draw = new _saveproc_draw(DmSaveWinSelectDraw);
        DmSoundPlaySE("Window");
        dm_save_draw_reserve = true;
    }

    private static void dmSaveProcWindowOpenEfct(DMS_SAVE_MAIN_WORK main_work)
    {
        if (((int) main_work.flag & 16) != 0)
        {
            main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowAnnounceIdle);
            main_work.disp_flag |= 1U;
            dm_save_disp_flag = main_work.disp_flag;
            main_work.flag &= 4294967279U;
        }
        else
            dmSaveSetWinOpenEfct(main_work);
    }

    private static void dmSaveProcWindowAnnounceIdle(DMS_SAVE_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (main_work.win_mode == 0)
        {
            if (DmCmnBackupIsLoadFinished())
            {
                main_work.win_timer = 8;
                if (main_work.timer >= 60)
                {
                    main_work.disp_flag &= 4294967294U;
                    dm_save_disp_flag = main_work.disp_flag;
                    main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowCloseEfct);
                    main_work.timer = 0;
                }
            }
            else if (AoSysMsgIsShow())
            {
                main_work.win_timer = 8;
                main_work.disp_flag &= 4294967294U;
                dm_save_disp_flag = main_work.disp_flag;
                main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowCloseEfct);
                main_work.timer = 0;
            }
            else
                main_work.timer = 0;
        }
        else if (main_work.win_mode == 1)
        {
            if (((int) main_work.flag & 4) != 0)
            {
                main_work.proc_input = null;
                main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowCloseEfct);
                main_work.win_timer = 8;
                main_work.disp_flag &= 4294967294U;
                DmSoundPlaySE("Ok");
                main_work.flag &= 4294967291U;
                main_work.timer = 0;
            }
        }
        else if (main_work.win_mode == 2)
        {
            if (DmCmnBackupIsSaveFinished())
            {
                main_work.win_timer = 8;
                if (main_work.timer >= 60)
                {
                    main_work.proc_input = null;
                    main_work.disp_flag &= 4294967294U;
                    main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowCloseEfct);
                    main_work.timer = 0;
                }
            }
            else
                main_work.timer = 0;
        }
        else if (main_work.win_mode == 3)
        {
            if (DmCmnBackupIsSaveFinished())
            {
                main_work.win_timer = 8;
                if (main_work.timer >= 60)
                {
                    main_work.proc_input = null;
                    main_work.disp_flag &= 4294967294U;
                    main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowCloseEfct);
                    main_work.timer = 0;
                }
            }
            else
                main_work.timer = 0;
        }
        else
        {
            main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowCloseEfct);
            main_work.disp_flag &= 4294967294U;
        }

        dm_save_disp_flag = main_work.disp_flag;
        ++main_work.timer;
    }

    private static void dmSaveProcWindowCloseEfct(DMS_SAVE_MAIN_WORK main_work)
    {
        if (((int) main_work.flag & 16) != 0)
        {
            main_work.proc_menu_update = main_work.win_mode != 1 ? (main_work.win_mode != 0 ? new _saveproc_input_update(dmSaveProcWindowNodispIdle) : new _saveproc_input_update(dmSaveProcWaitLoadEnd)) : new _saveproc_input_update(dmSaveProcWaitSeStop);
            main_work.announce_flag &= (uint) ~(1 << main_work.win_mode);
            main_work.flag &= 4294967279U;
            main_work.proc_draw = null;
            dm_save_draw_reserve = false;
        }

        dmSaveSetWinCloseEfct(main_work);
    }

    private static void dmSaveProcWaitSeStop(DMS_SAVE_MAIN_WORK main_work)
    {
        if (main_work.timer > 60)
        {
            main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowNodispIdle);
            main_work.timer = 0;
        }
        else
            ++main_work.timer;
    }

    private static void dmSaveProcWaitLoadEnd(DMS_SAVE_MAIN_WORK main_work)
    {
        if (!DmCmnBackupIsLoadFinished())
            return;
        main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcWindowNodispIdle);
        main_work.timer = 0;
    }

    private static void dmSaveProcStopDraw(DMS_SAVE_MAIN_WORK main_work)
    {
        main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcDataRelease);
    }

    private static void dmSaveProcDataRelease(DMS_SAVE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
            AoTexRelease(main_work.cmn_tex[index]);
        if (dm_save_is_snd_build)
        {
            DmSoundExit();
            DmSoundFlush();
        }

        main_work.proc_menu_update = new _saveproc_input_update(dmSaveProcFinish);
    }

    private static void dmSaveProcFinish(DMS_SAVE_MAIN_WORK main_work)
    {
        if (dmSaveIsTexRelease(main_work) != 1)
            return;
        for (int index = 0; index < 2; ++index)
            dm_save_cmn_tex[index] = null;
        for (int index = 0; index < 6; ++index)
        {
            if (main_work.act[index] != null)
            {
                AoActDelete(main_work.act[index]);
                main_work.act[index] = null;
            }

            dm_save_act[index] = null;
        }

        for (int index = 0; index < 2; ++index)
        {
            if (main_work.arc_cmn_amb[index] != null)
                main_work.arc_cmn_amb[index] = null;
        }

        main_work.flag |= 1U;
        main_work.proc_menu_update = null;
    }

    private static void dmSaveInputProcWindow(DMS_SAVE_MAIN_WORK main_work)
    {
        if (AoAccountGetCurrentId() < 0)
        {
            if (AoPad.AoPadSomeoneStand(ControllerConsts.CONFIRM) < 0)
                return;
            main_work.flag |= 4U;
        }
        else
        {
            if ((AoPad.AoPadStand() & ControllerConsts.CONFIRM) == 0)
                return;
            main_work.flag |= 4U;
        }
    }

    private static void DmSaveWinSelectDraw()
    {
        AoActSysSetDrawTaskPrio(61439U);
        int num1;
        int num2;
        if (((int) dm_save_msg_flag & 2) != 0)
        {
            num1 = 749;
            num2 = (int) ((180.0 + dm_save_win_size_y_tbl[GsEnvGetLanguage()]) * (27.0 / 16.0));
        }
        else
        {
            if (GsEnvGetLanguage() == 4)
            {
                num1 = 749;
                num2 = 303;
            }
            else
            {
                num1 = 641;
                num2 = 303;
            }
        }

        uint tex_id = !dm_save_is_task_draw ? (((int) dm_save_msg_flag & 2) == 0 ? 0U : 1U) : 1U;
        if (dm_save_is_draw_state != 0U)
            AoWinSysDrawState(0, AoTexGetTexList(dm_save_cmn_tex[0]), tex_id, 480f, 356f, num1 * dm_save_win_size_rate[0], num2 * dm_save_win_size_rate[1], dm_save_draw_state);
        else
            AoWinSysDrawTask(0, AoTexGetTexList(dm_save_cmn_tex[0]), tex_id, 480f, 356f, num1 * dm_save_win_size_rate[0], (float) (num2 * (double) dm_save_win_size_rate[1] * 0.899999976158142), 61439);
        if (((int) dm_save_disp_flag & 1) != 0)
        {
            switch (dm_save_win_mode)
            {
                case 0:
                    AoActSetTexture(AoTexGetTexList(dm_save_cmn_tex[1]));
                    AoActSortRegAction(dm_save_act[2]);
                    break;
                case 1:
                    AoActSetTexture(AoTexGetTexList(dm_save_cmn_tex[1]));
                    AoActSortRegAction(dm_save_act[3]);
                    AoActSortRegAction(dm_save_act[5]);
                    break;
                case 2:
                    AoActSetTexture(AoTexGetTexList(dm_save_cmn_tex[1]));
                    AoActSortRegAction(dm_save_act[4]);
                    break;
                case 3:
                    AoActSetTexture(AoTexGetTexList(dm_save_cmn_tex[1]));
                    AoActSortRegAction(dm_save_act[4]);
                    break;
            }

            AoActAcmPush();
            int num3 = GsEnvGetLanguage() == 0 ? 0 : dm_save_win_size_y_tbl[GsEnvGetLanguage()] / 2;
            for (int index = 0; index < 6; ++index)
            {
                AOS_TEXTURE tex = index < 1 ? dm_save_cmn_tex[0] : dm_save_cmn_tex[1];
                AoActAcmInit();
                AoActAcmApplyTrans(dm_save_win_act_pos_tbl[index][0], dm_save_win_act_pos_tbl[index][1], 0.0f);
                if (((int) dm_save_msg_flag & 2) != 0)
                {
                    switch (index)
                    {
                        case 0:
                        case 1:
                            AoActAcmApplyTrans(-32f, num3 * -1, 0.0f);
                            break;
                        case 5:
                            AoActAcmApplyTrans(0.0f, 16f + num3, 0.0f);
                            break;
                    }
                }

                AoActAcmApplyScale(27f / 16f, 27f / 16f);
                AoActSetTexture(AoTexGetTexList(tex));
                AoActUpdate(dm_save_act[index], 0.0f);
            }

            AoActAcmPop();
            AoActSortExecute();
            AoActSortDraw();
            AoActSortUnregAll();
        }

        if (dm_save_is_draw_state == 0U || !dm_save_is_task_draw)
            return;
        amDrawMakeTask(new TaskProc(dmSaveTaskDraw), 61439, 0U);
    }

    private static void dmSaveTaskDraw(AMS_TCB tcb)
    {
        AoActDrawPre();
        amDrawExecCommand(dm_save_draw_state);
        amDrawEndScene();
    }

    private static int dmSaveIsDataLoad(DMS_SAVE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!amFsIsComplete(main_work.arc_cmn_amb_fs[index]))
                return 0;
        }

        return 1;
    }

    private static int dmSaveIsTexLoad(DMS_SAVE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AoTexIsLoaded(main_work.cmn_tex[index]))
                return 0;
        }

        return dm_save_is_snd_build && !DmSoundBuildCheck() ? 0 : 1;
    }

    private static int dmSaveIsTexRelease(DMS_SAVE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AoTexIsReleased(main_work.cmn_tex[index]))
                return 0;
        }

        return 1;
    }

    private static void dmSaveSetWinOpenEfct(DMS_SAVE_MAIN_WORK main_work)
    {
        if (main_work.win_timer > 8)
        {
            main_work.flag |= 16U;
            main_work.win_timer = 0;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int) index] = 1f;
        }
        else
            ++main_work.win_timer;

        for (uint index = 0; index < 2U; ++index)
        {
            main_work.win_size_rate[(int) index] = main_work.win_timer == 0 ? 1f : main_work.win_timer / 8f;
            if (main_work.win_size_rate[(int) index] > 1.0)
                main_work.win_size_rate[(int) index] = 1f;
            dm_save_win_size_rate[(int) index] = main_work.win_size_rate[(int) index];
        }
    }

    private static void dmSaveSetWinCloseEfct(DMS_SAVE_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
        {
            main_work.win_size_rate[(int) index] = main_work.win_timer == 0 ? 0.0f : main_work.win_timer / 8f;
            dm_save_win_size_rate[(int) index] = main_work.win_size_rate[(int) index];
        }

        if (main_work.win_timer < 0)
        {
            main_work.flag |= 16U;
            main_work.win_timer = 0;
            for (uint index = 0; index < 2U; ++index)
            {
                main_work.win_size_rate[(int) index] = 0.0f;
                dm_save_win_size_rate[(int) index] = main_work.win_size_rate[(int) index];
            }
        }
        else
            --main_work.win_timer;
    }
}