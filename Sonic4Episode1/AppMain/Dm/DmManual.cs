public partial class AppMain
{
    private static void DmManualBuild(AMS_AMB_HEADER[] arc_amb)
    {
        dm_manual_mgr.Clear();
        dm_manual_mgr_p = dm_manual_mgr;
        for (int index = 0; index < 2; ++index)
            dm_manual_tex[index].Clear();
        for (int index = 0; index < 2; ++index)
        {
            dm_manual_ama[index] = readAMAFile(amBindGet(arc_amb[index], 0));
            string sPath;
            dm_manual_amb[index] = readAMBFile(amBindGet(arc_amb[index], 1, out sPath));
            dm_manual_amb[index].dir = sPath;
        }
        for (int index = 0; index < 2; ++index)
        {
            AoTexBuild(dm_manual_tex[index], dm_manual_amb[index]);
            AoTexLoad(dm_manual_tex[index]);
        }
    }

    private static bool DmManualBuildCheck()
    {
        return dmManualIsTexLoad() != 0;
    }

    private static void DmManualFlush()
    {
        for (int index = 0; index < 2; ++index)
            AoTexRelease(dm_manual_tex[index]);
    }

    private static bool DmManualFlushCheck()
    {
        return dmManualIsTexRelease() != 0;
    }

    private static void DmManualStart()
    {
        dmManualInit();
    }

    private static bool DmManualIsExit()
    {
        return dm_manual_mgr_p.tcb == null;
    }

    private static void DmManualExit()
    {
        if (dm_manual_mgr_p.tcb == null)
            return;
        mtTaskClearTcb(dm_manual_mgr_p.tcb);
        dm_manual_mgr_p.tcb = null;
    }

    private static void dmManualInit()
    {
        dm_manual_mgr_p.tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(dmManualProcMain), new GSF_TASK_PROCEDURE(dmManualDest), 0U, (ushort)short.MaxValue, 12288U, 10, () => new DMS_MANUAL_MAIN_WORK(), "MANUAL_MAIN");
        DMS_MANUAL_MAIN_WORK work = (DMS_MANUAL_MAIN_WORK)dm_manual_mgr_p.tcb.work;
        work.draw_state = AoActSysGetDrawStateEnable() ? 1U : 0U;
        AoActSysSetDrawStateEnable(work.draw_state != 0U);
        if (work.draw_state != 0U)
        {
            dm_manual_draw_state = AoActSysGetDrawState();
            AoActSysSetDrawState(dm_manual_draw_state);
        }
        work.is_jp_region = GeEnvGetDecideKey() == GSE_DECIDE_KEY.GSD_DECIDE_KEY_O;
        dmManualSetInitData(work);
        switch (SyGetEvtInfo().cur_evt_id)
        {
            case 6:
            case 11:
                dm_manual_is_pause_maingame = true;
                work.se_handle = GsSoundAllocSeHandle();
                break;
        }
        work.proc_update = new DMS_MANUAL_MAIN_WORK._proc_update_(dmManualProcInit);
    }

    private static void dmManualSetInitData(DMS_MANUAL_MAIN_WORK main_work)
    {
        main_work.cur_disp_page = 0;
        main_work.cur_disp_page_prev = -1;
    }

    private static void dmManualProcMain(MTS_TASK_TCB tcb)
    {
        DMS_MANUAL_MAIN_WORK work = (DMS_MANUAL_MAIN_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
            DmManualExit();
        if (((int)work.flag & int.MinValue) != 0 && !AoAccountIsCurrentEnable())
        {
            work.proc_update = new DMS_MANUAL_MAIN_WORK._proc_update_(dmManualProcFadeOut);
            work.flag &= int.MaxValue;
            if (dm_manual_is_pause_maingame)
                IzFadeInitEasyColor(0, (ushort)short.MaxValue, IZD_FADE_DT_PRIO_DEF, 18U, 1U, 1U, 32f, true);
            else
                IzFadeInitEasy(1U, 1U, 32f);
            work.flag &= 4294967291U;
            work.flag &= 4294967293U;
            work.proc_input = null;
        }
        if (work.proc_update != null)
            work.proc_update(work);
        if (work.proc_draw == null)
            return;
        work.proc_draw(work);
    }

    private static void dmManualDest(MTS_TASK_TCB tcb)
    {
    }

    private static void dmManualProcInit(DMS_MANUAL_MAIN_WORK main_work)
    {
        main_work.proc_update = new DMS_MANUAL_MAIN_WORK._proc_update_(dmManualProcCreateAct);
        main_work.flag |= 2147483648U;
    }

    private static void dmManualProcCreateAct(DMS_MANUAL_MAIN_WORK main_work)
    {
        for (uint index = 0; index <= 13U; ++index)
        {
            A2S_AMA_HEADER ama = dm_manual_ama[0];
            AoActSetTexture(AoTexGetTexList(dm_manual_tex[0]));
            main_work.act[(int)index] = AoActCreate(ama, g_dm_act_id_tbl_m[(int)index]);
        }
        for (uint index = 10; index <= 11U; ++index)
            main_work.trg_btn[(int)(index - 10U)].Create(main_work.act[(int)index]);
        main_work.trg_return.Create(main_work.act[13]);
        for (uint index = 119; index <= 120U; ++index)
        {
            A2S_AMA_HEADER ama = dm_manual_ama[1];
            AoActSetTexture(AoTexGetTexList(dm_manual_tex[1]));
            main_work.act[(int)index] = AoActCreate(ama, g_dm_act_id_tbl_m[(int)index]);
        }
        main_work.proc_update = new DMS_MANUAL_MAIN_WORK._proc_update_(dmManualProcFadeIn);
        main_work.proc_draw = new DMS_MANUAL_MAIN_WORK._proc_draw_(dmManualProcActDraw);
        if (dm_manual_is_pause_maingame)
            IzFadeInitEasyColor(0, (ushort)short.MaxValue, IZD_FADE_DT_PRIO_DEF, 18U, 0U, 0U, 32f, true);
        else
            IzFadeInitEasy(0U, 0U, 32f);
    }

    private static void dmManualProcFadeIn(DMS_MANUAL_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        main_work.proc_update = new DMS_MANUAL_MAIN_WORK._proc_update_(dmManualProcWaitInput);
        main_work.proc_input = new DMS_MANUAL_MAIN_WORK._proc_input_(dmManualInputProcMain);
    }

    private static void dmManualProcWaitInput(DMS_MANUAL_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        int[] numArray = new int[3] { 12, 13, 119 };
        int index = 0;
        for (int length = numArray.Length; index < length; ++index)
        {
            AOS_ACTION act = main_work.act[numArray[index]];
            float frame = !main_work.trg_return.GetState(0U)[10] || !main_work.trg_return.GetState(0U)[1] ? (!main_work.trg_return.GetState(0U)[0] ? (2.0 > act.frame ? 0.0f : act.frame) : 1f) : 2f;
            AoActSetFrame(act, frame);
        }
        if (((int)main_work.flag & 2) != 0)
        {
            main_work.proc_update = new DMS_MANUAL_MAIN_WORK._proc_update_(dmManualProcFadeOut);
            if (dm_manual_is_pause_maingame)
                IzFadeInitEasyColor(0, (ushort)short.MaxValue, IZD_FADE_DT_PRIO_DEF, 18U, 0U, 1U, 32f, true);
            else
                IzFadeInitEasy(0U, 1U, 32f);
            if (!dm_manual_is_pause_maingame)
                DmSoundPlaySE("Cancel");
            else
                GsSoundPlaySe("Cancel", main_work.se_handle);
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }
        else if (((int)main_work.flag & 8) != 0)
        {
            ++main_work.cur_disp_page;
            if (main_work.cur_disp_page > 14)
                main_work.cur_disp_page = 14;
            else if (!dm_manual_is_pause_maingame)
                DmSoundPlaySE("Cursol");
            else
                GsSoundPlaySe("Cursol", main_work.se_handle);
            main_work.flag &= 4294967287U;
        }
        else
        {
            if (((int)main_work.flag & 16) == 0)
                return;
            --main_work.cur_disp_page;
            if (main_work.cur_disp_page < 0)
                main_work.cur_disp_page = 0;
            else if (!dm_manual_is_pause_maingame)
                DmSoundPlaySE("Cursol");
            else
                GsSoundPlaySe("Cursol", main_work.se_handle);
            main_work.flag &= 4294967279U;
        }
    }

    private static void dmManualProcFadeOut(DMS_MANUAL_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        main_work.proc_update = new DMS_MANUAL_MAIN_WORK._proc_update_(dmManualProcStopDraw);
        main_work.proc_draw = null;
        if (main_work.se_handle == null)
            return;
        GsSoundFreeSeHandle(main_work.se_handle);
        main_work.se_handle = null;
    }

    private static void dmManualInputProcMain(DMS_MANUAL_MAIN_WORK main_work)
    {
        if (main_work.trg_return.GetState(0U)[10] && main_work.trg_return.GetState(0U)[1] || isBackKeyPressed())
        {
            setBackKeyRequest(false);
            main_work.flag |= 2U;
        }
        else if (main_work.trg_btn[1].GetState(0U)[7])
        {
            if (!main_work.trg_btn[1].GetState(0U)[8] && main_work.cur_disp_page == 14)
                return;
            main_work.flag |= 8U;
        }
        else
        {
            if (!main_work.trg_btn[0].GetState(0U)[7] || !main_work.trg_btn[0].GetState(0U)[8] && main_work.cur_disp_page == 0)
                return;
            main_work.flag |= 16U;
        }
    }

    private static void dmManualProcStopDraw(DMS_MANUAL_MAIN_WORK main_work)
    {
        for (int index = 0; index < 179; ++index)
        {
            if (main_work.act[index] != null)
            {
                AoActDelete(main_work.act[index]);
                main_work.act[index] = null;
            }
        }
        for (int index = 0; index < main_work.trg_btn.Length; ++index)
            main_work.trg_btn[index].Release();
        main_work.trg_return.Release();
        main_work.proc_update = null;
        main_work.flag |= 1U;
    }

    private static void dmManualProcActDraw(DMS_MANUAL_MAIN_WORK main_work)
    {
        dmManualCommonBgDraw(main_work);
        dmManualPageDraw(main_work);
        dmManualCommonDraw(main_work);
        if (main_work.draw_state <= 0U)
            return;
        amDrawMakeTask(new TaskProc(dmManualTaskDraw), 32768, 0U);
    }

    private static void dmManualTaskDraw(AMS_TCB tcb)
    {
        AoActDrawPre();
        amDrawExecCommand(dm_manual_draw_state);
        amDrawEndScene();
    }

    private static void dmManualCommonBgDraw(DMS_MANUAL_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(6144U);
        AoActSetTexture(AoTexGetTexList(dm_manual_tex[0]));
        for (int index = 0; index <= 4; ++index)
            AoActSortRegAction(main_work.act[index]);
        for (int index = 0; index <= 4; ++index)
        {
            AoActUpdate(main_work.act[index], 0.0f);
            if (index == 3)
            {
                --main_work.act[index].sprite.offset.top;
                main_work.act[index].sprite.offset.left -= 2f;
            }
        }
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private static void dmManualCommonDraw(DMS_MANUAL_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(12288U);
        AoActSetTexture(AoTexGetTexList(dm_manual_tex[0]));
        for (int index = 5; index <= 11; ++index)
        {
            switch (index)
            {
                case 5:
                    if (main_work.cur_disp_page >= 9)
                    {
                        AoActSortRegAction(main_work.act[index]);
                        break;
                    }
                    break;
                case 10:
                    if (main_work.cur_disp_page != 0)
                    {
                        AoActSortRegAction(main_work.act[index]);
                        break;
                    }
                    break;
                case 11:
                    if (main_work.cur_disp_page != 14)
                    {
                        AoActSortRegAction(main_work.act[index]);
                        break;
                    }
                    break;
                default:
                    AoActSortRegAction(main_work.act[index]);
                    break;
            }
        }
        AoActSetTexture(AoTexGetTexList(dm_manual_tex[1]));
        for (int index = 119; index < 120; ++index)
            AoActSortRegAction(main_work.act[index]);
        if (main_work.cur_disp_page >= 1)
            AoActSortRegAction(main_work.act[120]);
        for (int index = 12; index <= 13; ++index)
            AoActSortRegAction(main_work.act[index]);
        float frame1 = main_work.cur_disp_page == 0 ? 0.0f : main_work.cur_disp_page / 10;
        float num = main_work.cur_disp_page % 10;
        if (main_work.cur_disp_page == 9)
        {
            AoActSetFrame(main_work.act[5], 0.0f);
            AoActSetFrame(main_work.act[6], 0.0f);
        }
        else if (main_work.cur_disp_page < 10)
        {
            AoActSetFrame(main_work.act[5], 0.0f);
            AoActSetFrame(main_work.act[6], main_work.cur_disp_page + 1f);
        }
        else
        {
            AoActSetFrame(main_work.act[5], frame1);
            AoActSetFrame(main_work.act[6], num + 1f);
        }
        AoActSetFrame(main_work.act[8], 0.0f);
        AoActSetFrame(main_work.act[9], 0.0f);
        AoActSetTexture(AoTexGetTexList(dm_manual_tex[0]));
        for (int index = 5; index < 10; ++index)
            AoActUpdate(main_work.act[index], 0.0f);
        for (int index = 10; index <= 11; ++index)
            AoActUpdate(main_work.act[index], 1f);
        for (int index = 12; index <= 13; ++index)
        {
            float frame2 = 2.0 <= main_work.act[index].frame ? 1f : 0.0f;
            AoActUpdate(main_work.act[index], frame2);
        }
        for (int index = 0; index < main_work.trg_btn.Length; ++index)
            main_work.trg_btn[index].Update();
        main_work.trg_return.Update();
        AoActSetTexture(AoTexGetTexList(dm_manual_tex[1]));
        for (int index = 119; index < 120; ++index)
        {
            float frame2 = 2.0 <= main_work.act[index].frame ? 1f : 0.0f;
            AoActUpdate(main_work.act[index], frame2);
        }
        for (int index = 120; index <= 120; ++index)
            AoActUpdate(main_work.act[index], 0.0f);
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private static void dmManualPageDraw(DMS_MANUAL_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(8192U);
        int curDispPage = main_work.cur_disp_page;
        int num1 = dm_manual_disp_act_cmn_tbl[curDispPage][0];
        int num2 = dm_manual_disp_act_cmn_tbl[curDispPage][1];
        int num3 = dm_manual_disp_act_lang_tbl[curDispPage][0];
        int num4 = dm_manual_disp_act_lang_tbl[curDispPage][1];
        bool flag = main_work.cur_disp_page_prev != main_work.cur_disp_page;
        if (flag && 0 <= main_work.cur_disp_page_prev)
        {
            for (int index = dm_manual_disp_act_cmn_tbl[main_work.cur_disp_page_prev][0]; index <= dm_manual_disp_act_cmn_tbl[main_work.cur_disp_page_prev][1]; ++index)
            {
                AoActDelete(main_work.act[index]);
                main_work.act[index] = null;
            }
            for (int index = dm_manual_disp_act_lang_tbl[main_work.cur_disp_page_prev][0]; index <= dm_manual_disp_act_lang_tbl[main_work.cur_disp_page_prev][1]; ++index)
            {
                AoActDelete(main_work.act[index]);
                main_work.act[index] = null;
            }
        }
        main_work.cur_disp_page_prev = main_work.cur_disp_page;
        for (int index = num1; index <= num2; ++index)
        {
            if (flag)
            {
                A2S_AMA_HEADER ama = dm_manual_ama[0];
                AoActSetTexture(AoTexGetTexList(dm_manual_tex[0]));
                main_work.act[index] = AoActCreate(ama, g_dm_act_id_tbl_m[index]);
            }
            if (index != 118)
                AoActSortRegAction(main_work.act[index]);
        }
        AoActSetTexture(AoTexGetTexList(dm_manual_tex[1]));
        for (int index = num3; index <= num4; ++index)
        {
            if (flag)
            {
                A2S_AMA_HEADER ama = dm_manual_ama[1];
                main_work.act[index] = AoActCreate(ama, g_dm_act_id_tbl_m[index]);
                if (GsEnvGetLanguage() == 6)
                {
                    if (g_dm_act_id_tbl_m[index] == 9U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 120f;
                    if (g_dm_act_id_tbl_m[index] == 10U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 125f;
                    if (g_dm_act_id_tbl_m[index] == 48U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 120f;
                    if (g_dm_act_id_tbl_m[index] == 49U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 125f;
                    if (g_dm_act_id_tbl_m[index] == 18U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 90f;
                    if (g_dm_act_id_tbl_m[index] == 19U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 200f;
                    if (g_dm_act_id_tbl_m[index] == 52U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 90f;
                    if (g_dm_act_id_tbl_m[index] == 53U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 200f;
                }
                if (GsEnvGetLanguage() == 7)
                {
                    if (g_dm_act_id_tbl_m[index] == 9U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 180f;
                    if (g_dm_act_id_tbl_m[index] == 10U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 185f;
                    if (g_dm_act_id_tbl_m[index] == 48U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 180f;
                    if (g_dm_act_id_tbl_m[index] == 49U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 185f;
                    if (g_dm_act_id_tbl_m[index] == 18U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 280f;
                    if (g_dm_act_id_tbl_m[index] == 19U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 235f;
                    if (g_dm_act_id_tbl_m[index] == 52U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 280f;
                    if (g_dm_act_id_tbl_m[index] == 53U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 235f;
                }
                if (GsEnvGetLanguage() == 8)
                {
                    if (g_dm_act_id_tbl_m[index] == 9U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 160f;
                    if (g_dm_act_id_tbl_m[index] == 10U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 245f;
                    if (g_dm_act_id_tbl_m[index] == 48U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 160f;
                    if (g_dm_act_id_tbl_m[index] == 49U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 245f;
                    if (g_dm_act_id_tbl_m[index] == 18U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 19U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 220f;
                    if (g_dm_act_id_tbl_m[index] == 52U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 53U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 220f;
                }
                if (GsEnvGetLanguage() == 9)
                {
                    if (g_dm_act_id_tbl_m[index] == 9U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 10U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 225f;
                    if (g_dm_act_id_tbl_m[index] == 48U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 49U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 225f;
                    if (g_dm_act_id_tbl_m[index] == 18U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 19U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 52U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 53U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                }
                if (GsEnvGetLanguage() == 10)
                {
                    if (g_dm_act_id_tbl_m[index] == 9U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 10U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 225f;
                    if (g_dm_act_id_tbl_m[index] == 48U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 49U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 225f;
                    if (g_dm_act_id_tbl_m[index] == 18U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 19U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 52U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (g_dm_act_id_tbl_m[index] == 53U)
                        ((A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                }
            }
            AoActSortRegAction(main_work.act[index]);
        }
        AoActSetTexture(AoTexGetTexList(dm_manual_tex[0]));
        for (int index = num1; index <= num2; ++index)
        {
            if (index != 118)
            {
                AoActUpdate(main_work.act[index], 1f);
                if (main_work.cur_disp_page_prev != 8)
                {
                    main_work.act[index].sprite.center_y -= 16f;
                    if (main_work.cur_disp_page_prev == 6 || main_work.cur_disp_page_prev == 7 || (main_work.cur_disp_page_prev == 10 || main_work.cur_disp_page_prev == 12))
                        main_work.act[index].sprite.center_y -= 16f;
                    if (index == 14)
                        ++main_work.act[index].sprite.offset.right;
                }
                if (index >= 54 && index <= 55 || index >= 59 && index <= 61 || (index == 65 || index == 80 || (index == 81 || index == 25)) || (index == 24 || index >= 91 && index <= 93 || index >= 103 && index <= 105))
                    main_work.act[index].sprite.center_y += 16f;
                if (index >= 82 && index <= 90)
                {
                    main_work.act[index].sprite.offset.top += 40f;
                    main_work.act[index].sprite.offset.bottom -= 40f;
                }
                else if (index >= 114 && index <= 116)
                {
                    main_work.act[index].sprite.offset.top += 80f;
                    main_work.act[index].sprite.offset.bottom -= 80f;
                }
                else if (index >= 67 && index <= 72 || index >= 94 && index <= 102 || (index == 80 || index == 81))
                {
                    main_work.act[index].sprite.offset.top += 20f;
                    main_work.act[index].sprite.offset.bottom -= 20f;
                    if (index == 80 || index == 81)
                    {
                        main_work.act[index].sprite.offset.left += 25f;
                        main_work.act[index].sprite.offset.right -= 25f;
                    }
                }
                else if (index == 117)
                    main_work.act[index].sprite.center_y += 70f;
            }
        }
        AoActSetTexture(AoTexGetTexList(dm_manual_tex[1]));
        for (int index = num3; index <= num4; ++index)
        {
            AoActUpdate(main_work.act[index], 1f);
            if (main_work.cur_disp_page_prev < 3 || main_work.cur_disp_page_prev > 9 || main_work.cur_disp_page_prev == 7)
                main_work.act[index].sprite.center_y -= 16f;
            if (index == num3 && (main_work.cur_disp_page_prev < 3 || main_work.cur_disp_page_prev > 12))
                main_work.act[index].sprite.center_y += 16f;
            if (index == 143 || index == 144)
                main_work.act[index].sprite.center_y -= 16f;
            else if (index >= 147 && index <= 148 || index >= 151 && index <= 152 || (index == 157 || index == 174 || (index == 170 || index == 161)) || (index == 162 || main_work.cur_disp_page_prev == 11))
                main_work.act[index].sprite.center_y += 16f;
        }
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private static int dmManualIsTexLoad()
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AoTexIsLoaded(dm_manual_tex[index]))
                return 0;
        }
        return 1;
    }

    private static int dmManualIsTexRelease()
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AoTexIsReleased(dm_manual_tex[index]))
                return 0;
        }
        return 1;
    }

}