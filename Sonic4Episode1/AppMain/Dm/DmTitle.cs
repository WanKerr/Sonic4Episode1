using System;
using System.Collections.Generic;
using er;
using er.web;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

public partial class AppMain
{
    private void DmTitleStart(object arg)
    {
        dm_title_is_title_start = true;
        int oldEvtId = SyGetEvtInfo().old_evt_id;
        if (dm_title_is_title_start)
            SSave.CreateInstance().Init();
        this.dmTitleInit();
    }

    private void DmMainMenuStart(object arg)
    {
        dm_title_is_title_start = false;
        this.dmTitleInit();
    }

    private void dmTitleInit()
    {
        // GC.Collect();
        AoActSysSetDrawStateEnable(true);
        AoActSysSetDrawState(10U);
        var work = (DMS_TITLE_MAIN_WORK)MTM_TASK_MAKE_TCB(this.dmTitleProcMain, this.dmTitleDest, 0U, 0, 8192U, 10, () => new DMS_TITLE_MAIN_WORK(), "TITLE_MAIN").work;
        this.dmTitleSetInitDispData(work);
        if (dm_title_is_title_start)
            AoAccountClearCurrentId();
        work.is_jp_region = GeEnvGetDecideKey() == GSE_DECIDE_KEY.GSD_DECIDE_KEY_O;
        work.proc_update = this.dmTitleLoadFontData;
    }

    private void dmTitleSetInitDispData(DMS_TITLE_MAIN_WORK main_work)
    {
        switch (SyGetEvtInfo().old_evt_id)
        {
            case 5:
                main_work.cur_slct_menu = 1;
                break;
            case 6:
                main_work.cur_slct_menu = 0;
                break;
            case 8:
                main_work.cur_slct_menu = !GsMainSysIsStageClear(0) || GsTrialIsTrial() ? 1 : 2;
                break;
            case 10:
                main_work.cur_slct_menu = 1;
                break;
            default:
                main_work.cur_slct_menu = 0;
                break;
        }

        main_work.disp_flag |= 2U;
        if (GsTrialIsTrial())
            main_work.cur_crsr_pos_y = dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
        else
            main_work.cur_crsr_pos_y =
                main_work.cur_slct_menu * dm_title_crsr_pos_y_tbl[1] + dm_title_crsr_pos_y_tbl[0];
    }

    private void dmTitleProcMain(MTS_TASK_TCB tcb)
    {
        DMS_TITLE_MAIN_WORK work = (DMS_TITLE_MAIN_WORK)tcb.work;
        work.flag_prev = work.flag;
        if (((int)work.flag & 1) != 0)
        {
            mtTaskClearTcb(tcb);
            this.dmTitleSetNextEvent(work);
        }

        if (((int)work.flag & int.MinValue) != 0 && event_after_buy)
        {
            work.is_init_play = true;
            work.is_no_save_data = true;
            event_after_buy = false;
            work.proc_update = (this.dmTitleProcFadeOut);
            work.flag &= int.MaxValue;
            work.flag |= 1073741824U;
            IzFadeInitEasy(1U, 1U, 32f);
            DmSndBgmPlayerExit();
            work.flag |= 524288U;
            work.flag &= 4294967291U;
            work.flag &= 4294967293U;
            work.proc_input = null;
            work.proc_win_input = null;
            work.win_timer = 0.0f;
            work.win_cur_slct = 0;
            work.win_mode = 0;
        }

        if (work.proc_win_update != null)
            work.proc_win_update(work);
        if (work.proc_update != null)
            work.proc_update(work);
        if (work.proc_draw == null)
            return;
        work.proc_draw(work);
    }

    private void dmTitleDest(MTS_TASK_TCB tcb)
    {
    }

    private void dmTitleSetNextEvent(DMS_TITLE_MAIN_WORK main_work)
    {
        short evt_case1 = 0;
        if (((int)main_work.flag & 1073741824) != 0)
        {
            short evt_case2 = 5;
            main_work.flag &= 3221225471U;
            SyDecideEvtCase(evt_case2);
            SyChangeNextEvt();
        }
        else if (((int)main_work.flag & 1048576) != 0)
        {
            short evt_case2 = 4;
            main_work.flag &= 4293918719U;
            SyDecideEvtCase(evt_case2);
            SyChangeNextEvt();
        }
        else
        {
            switch (main_work.cur_slct_menu)
            {
                case 0:
                    evt_case1 = 0;
                    this.dmTitleSetFirstPlayData(main_work);
                    nextDemoLevel = 0;
                    g_gs_main_sys_info.stage_id = 0;
                    g_gs_main_sys_info.char_id[0] = 0;
                    g_gs_main_sys_info.game_mode = 0;
                    if (GsTrialIsTrial())
                        g_gs_main_sys_info.rest_player_num = 3U;
                    g_gs_main_sys_info.game_flag &= 4294967167U;
                    GmMainGSInit();
                    break;
                case 1:
                    if (!GsMainSysIsStageClear(0) || main_work.is_init_play)
                    {
                        evt_case1 = 0;
                        g_gs_main_sys_info.stage_id = 0;
                        g_gs_main_sys_info.char_id[0] = 0;
                        g_gs_main_sys_info.game_mode = 0;
                        g_gs_main_sys_info.game_flag &= 4294967167U;
                        GmMainGSInit();
                        break;
                    }

                    evt_case1 = 1;
                    break;
                case 2:
                    evt_case1 = 2;
                    dm_opt_show_xboxlive = false;
                    break;
                case 3:
                    evt_case1 = 2;
                    LiveFeature.getInstance().ShowAchievements();
                    dm_opt_show_xboxlive = true;
                    break;
                case 4:
                    evt_case1 = 2;
                    LiveFeature.getInstance().ShowLeaderboards();
                    dm_opt_show_xboxlive = true;
                    break;
            }

            SyDecideEvtCase(evt_case1);
            SyChangeNextEvt();
        }
    }

    private void dmTitleLoadFontData(DMS_TITLE_MAIN_WORK main_work)
    {
        GsFontBuild();
        main_work.proc_update = (this.dmTitleIsLoadFontData);
    }

    private void dmTitleIsLoadFontData(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!GsFontIsBuilded())
            return;
        main_work.proc_update = (this.dmTitleLoadRequest);
    }

    private void dmTitleLoadRequest(DMS_TITLE_MAIN_WORK main_work)
    {
        main_work.arc_amb_fs[0] = amFsReadBackground("DEMO/TITLE/D_TITLE.AMB");
        main_work.arc_amb_fs[1] = amFsReadBackground(dm_title_file_lng_amb_name_tbl[GsEnvGetLanguage()]);
        for (int index = 0; index < 3; ++index)
            main_work.arc_cmn_amb_fs[index] = amFsReadBackground(dm_title_menu_cmn_amb_name_tbl[index]);
        main_work.arc_cmn_amb_fs[3] = amFsReadBackground(dm_title_menu_cmn_lng_amb_name_tbl[GsEnvGetLanguage()]);
        this.DmTitleOpLoad();
        main_work.proc_update = (this.dmTitleProcLoadWait);
    }

    private void dmTitleProcLoadWait(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!this.dmTitleIsDataLoad(main_work))
            return;
        for (int index1 = 0; index1 < 2; ++index1)
        {
            main_work.arc_amb[index1] = readAMBFile(main_work.arc_amb_fs[index1]);
            main_work.ama[index1] = readAMAFile(amBindGet(main_work.arc_amb[index1], 0));
            if (index1 == 0)
            {
                main_work.ama[index1].act_tbl[0].ofst.top = 50f;
                main_work.ama[index1].act_tbl[0].ofst.bottom = -50f;
                main_work.ama[index1].act_tbl[1].ofst.top = 50f;
                main_work.ama[index1].act_tbl[1].ofst.bottom = -50f;
                main_work.ama[index1].act_tbl[1].hit.hit_tbl[0].rect.top = -50f;
                main_work.ama[index1].act_tbl[1].hit.hit_tbl[0].rect.bottom = 50f;
                main_work.ama[index1].act_tbl[2].ofst.top = 50f;
                main_work.ama[index1].act_tbl[2].ofst.bottom = -50f;
            }

            if (index1 == 1)
            {
                main_work.ama[index1].act_num += 2U;
                A2S_AMA_ACT[] a2SAmaActArray = new A2S_AMA_ACT[(int)main_work.ama[index1].act_num];
                Array.Copy(main_work.ama[index1].act_tbl, a2SAmaActArray, (int)main_work.ama[index1].act_num - 2);
                uint num = main_work.ama[index1].act_num - 2U;
                a2SAmaActArray[(int)num] = new A2S_AMA_ACT();
                a2SAmaActArray[(int)num].Assign(a2SAmaActArray[0]);
                a2SAmaActArray[(int)(num + 1U)] = new A2S_AMA_ACT();
                a2SAmaActArray[(int)(num + 1U)].Assign(a2SAmaActArray[1]);
                a2SAmaActArray[(int)num].anm.anm_tbl[0].tex_id = 9;
                a2SAmaActArray[(int)(num + 1U)].anm.anm_tbl[0].tex_id = 10;
                main_work.ama[index1].act_tbl = a2SAmaActArray;
            }

            string sPath;
            main_work.amb[index1] = readAMBFile(amBindGet(main_work.arc_amb[index1], 1, out sPath));
            main_work.amb[index1].dir = sPath;
            amFsClearRequest(main_work.arc_amb_fs[index1]);
            main_work.arc_amb_fs[index1] = null;
            AoTexBuild(main_work.tex[index1], main_work.amb[index1]);
            if (index1 == 1)
            {
                main_work.tex[index1].txb.texfilelist.nTex += 2;
                NNS_TEXFILE[] nnsTexfileArray = New<NNS_TEXFILE>(main_work.tex[index1].txb.texfilelist.nTex);
                for (int index2 = 0; index2 < main_work.tex[index1].txb.texfilelist.nTex; ++index2)
                {
                    int index3 = index2 < main_work.tex[index1].txb.texfilelist.nTex - 2
                        ? index2
                        : main_work.tex[index1].txb.texfilelist.nTex - 2 - 1;
                    nnsTexfileArray[index2].Bank = main_work.tex[index1].txb.texfilelist.pTexFileList[index3].Bank;
                    nnsTexfileArray[index2].fType = main_work.tex[index1].txb.texfilelist.pTexFileList[index3].fType;
                    nnsTexfileArray[index2].Filename =
                        main_work.tex[index1].txb.texfilelist.pTexFileList[index3].Filename;
                    nnsTexfileArray[index2].MinFilter =
                        main_work.tex[index1].txb.texfilelist.pTexFileList[index3].MinFilter;
                    nnsTexfileArray[index2].MagFilter =
                        main_work.tex[index1].txb.texfilelist.pTexFileList[index3].MagFilter;
                    nnsTexfileArray[index2].GlobalIndex =
                        main_work.tex[index1].txb.texfilelist.pTexFileList[index3].GlobalIndex;
                }

                switch (GsEnvGetLanguage())
                {
                    case 0:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_JP.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_JP.PNG";
                        break;
                    case 1:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_US.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_US.PNG";
                        break;
                    case 2:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_FR.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_FR.PNG";
                        break;
                    case 3:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_IT.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_IT.PNG";
                        break;
                    case 4:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_GE.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_GE.PNG";
                        break;
                    case 5:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_SP.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_SP.PNG";
                        break;
                    case 6:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_FI.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_FI.PNG";
                        break;
                    case 7:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_PT.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_PT.PNG";
                        break;
                    case 8:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_RU.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_RU.PNG";
                        break;
                    case 9:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_CN.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_CN.PNG";
                        break;
                    case 10:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_HK.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_HK.PNG";
                        break;
                    default:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename =
                            "D_TITLE_TEX_TROPHY_US.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename =
                            "D_TITLE_TEX_LB_US.PNG";
                        break;
                }

                main_work.tex[index1].txb.texfilelist.pTexFileList = nnsTexfileArray;
            }

            AoTexLoad(main_work.tex[index1]);
        }

        for (int index = 0; index < 4; ++index)
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

        this.DmTitleOpBuild();
        DmSndBgmPlayerInit();
        main_work.proc_update = (this.dmTitleProcTexBuildWait);
    }

    private void dmTitleProcTexBuildWait(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!this.dmTitleIsTexLoad(main_work) || !DmSndBgmPlayerIsSndSysBuild())
            return;
        for (int index = 0; index < 41; ++index)
        {
            int num1 = 38;
            int num2 = 32;
            A2S_AMA_HEADER ama;
            AOS_TEXTURE tex;
            if (index >= num1)
            {
                ama = main_work.cmn_ama[3];
                tex = main_work.cmn_tex[3];
            }
            else if (index >= num2)
            {
                ama = main_work.cmn_ama[2];
                tex = main_work.cmn_tex[2];
            }
            else if (index >= 30)
            {
                ama = main_work.cmn_ama[0];
                tex = main_work.cmn_tex[0];
            }
            else if (index < 19)
            {
                ama = main_work.ama[0];
                tex = main_work.tex[0];
            }
            else
            {
                ama = main_work.ama[1];
                tex = main_work.tex[1];
                switch (index)
                {
                    case 20:
                        ama.act_tbl[(int)dm_title_g_dm_act_id_tbl[index]].mtn.trs_tbl[0].trs_y = 195f;
                        break;
                    case 21:
                        ama.act_tbl[(int)dm_title_g_dm_act_id_tbl[index]].mtn.trs_tbl[0].trs_y = 305f;
                        break;
                    case 22:
                        ama.act_tbl[(int)dm_title_g_dm_act_id_tbl[index]].mtn.trs_tbl[0].trs_y = 415f;
                        break;
                    case 24:
                        ama.act_tbl[(int)dm_title_g_dm_act_id_tbl[index]].mtn.trs_tbl[0].trs_y = 525f;
                        break;
                    case 25:
                        ama.act_tbl[(int)dm_title_g_dm_act_id_tbl[index]].mtn.trs_tbl[0].trs_y = 635f;
                        break;
                }
            }

            AoActSetTexture(AoTexGetTexList(tex));
            main_work.act[index] = AoActCreate(ama, dm_title_g_dm_act_id_tbl[index]);
        }

        int[] numArray1 = new int[5] { 1, 4, 7, 10, 13 };
        int index1 = 0;
        for (int length = numArray1.Length; index1 < length; ++index1)
            main_work.trg_slct[index1].Create(main_work.act[numArray1[index1]]);
        int[] numArray2 = new int[2] { 33, 36 };
        int index2 = 0;
        for (int length = numArray2.Length; index2 < length; ++index2)
            main_work.trg_answer[index2].Create(main_work.act[numArray2[index2]]);
        int index3 = 16;
        main_work.trg_return.Create(main_work.act[index3]);
        int index4 = 18;
        main_work.trg_game.Create(main_work.act[index4]);
        main_work.proc_update = (this.dmTitleProcCheckLoadingEnd);
        this.DmTitleOpInit();
        AoActSysSetDrawState(10U);
        AoActSysSetDrawStateEnable(true);
    }

    private void dmTitleProcCheckLoadingEnd(DMS_TITLE_MAIN_WORK main_work)
    {
        if (dm_title_is_title_start)
        {
            main_work.disp_change_time = 40;
            main_work.flag |= 8U;
            main_work.proc_update = (this.dmTitleProcFadeIn);
            main_work.proc_draw = (this.dmTitleDrawSetProcDispData);
            main_work.flag |= 512U;
            this.DmTitleOpDispRightEnable(true);
            for (int index = 0; index < 2; ++index)
                main_work.mmenu_win_size_rate[index] = 0.0f;
            IzFadeInitEasy(1U, 0U, 32f);
            XBOXLive.allowShowUpdate = true;
        }
        else
        {
            main_work.proc_update = (this.dmTitleProcFadeIn);
            this.dmTitleSetMenuInfo(main_work);
            this.DmTitleOpSetRetOptionState();
            main_work.proc_draw = (this.dmTitleDrawSetProcDispData);
            main_work.flag |= 1024U;
            main_work.flag |= 2097152U;
            this.DmTitleOpDispRightEnable(false);
            for (int index = 0; index < 2; ++index)
                main_work.mmenu_win_size_rate[index] = 1f;
        }
    }

    private void dmTitleProcFadeIn(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        AoPresenceSet(AO_PRESENCE_STAGE_ID_TITLE, false);
        if (dm_title_is_title_start)
        {
            main_work.proc_update = (this.dmTitleProcWaitInput);
            main_work.proc_input = (this.dmTitleInputProcTitle);
            DmSndBgmPlayerPlayBgm(1);
        }
        else
        {
            main_work.proc_update = (this.dmTitleProcMainMenuIdle);
            main_work.proc_input = (this.dmTitleInputProcMainMenu);
            main_work.flag |= 2147483648U;
            DmSndBgmPlayerPlayBgm(0);
        }

        main_work.proc_win_update = (this.dmTitleProcWindowNodispIdle);
    }

    private void dmTitleProcWaitInput(DMS_TITLE_MAIN_WORK main_work)
    {
        ++main_work.timer;
        if (main_work.timer > 3600.0)
        {
            main_work.proc_update = (this.dmTitleProcFadeOut);
            main_work.proc_input = null;
            main_work.timer = 0.0f;
            main_work.flag |= 1048576U;
            IzFadeInitEasy(0U, 1U, 32f);
            DmSndBgmPlayerExit();
            main_work.flag |= 524288U;
        }
        else
        {
            if (main_work.proc_input != null)
                main_work.proc_input(main_work);
            if (((int)main_work.flag & 32) == 0)
                return;
            main_work.proc_update = (this.dmTitleProcDecideEfct);
            main_work.proc_input = null;
            main_work.timer = 0.0f;
            main_work.flag &= 4294967263U;
            DmSoundPlaySE("Ok");
            DmSndBgmPlayerBgmStop();
        }
    }

    private void dmTitleProcDecideEfct(DMS_TITLE_MAIN_WORK main_work)
    {
        if (main_work.timer >= 60.0 && AoAccountSetCurrentIdIsFinished())
        {
            if (AoAccountGetCurrentId() >= 0)
            {
                main_work.flag &= 4294966783U;
                main_work.proc_update = (this.dmTitleProcCheckTrialIdle);
                GsRebootSetTitle();
            }
            else
            {
                main_work.proc_update = (this.dmTitleProcWaitInput);
                main_work.proc_input = (this.dmTitleInputProcTitle);
                DmSndBgmPlayerPlayBgm(1);
            }

            main_work.timer = 0.0f;
        }
        else
            ++main_work.timer;
    }

    private void dmTitleProcCheckTrialIdle(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!GsTrialCheckIsFinished())
            return;
        if (GsTrialIsTrial())
        {
            main_work.proc_update = (this.dmTitleProcMainMenuOpenWin);
            main_work.proc_input = null;
            main_work.flag |= 2147483648U;
            main_work.flag |= 4096U;
            main_work.flag |= 2097152U;
            main_work.is_init_play = false;
            main_work.slct_menu_num = 3;
            main_work.cur_slct_menu = 0;
            main_work.cur_crsr_pos_y = dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
            DmSndBgmPlayerPlayBgm(0);
        }
        else
        {
            DmSaveStart(1U, false, false);
            main_work.proc_update = (this.dmTitleProcFileSlctWaitDataLoad);
            DmSndBgmPlayerPlayBgm(0);
        }
    }

    private void dmTitleProcFileSlctWaitDataLoad(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!DmSaveIsExit())
            return;
        if (DmCmnBackupIsLoadSuccessed())
        {
            main_work.proc_update = (this.dmTitleProcMainMenuOpenWin);
            main_work.proc_input = null;
            main_work.flag |= 2147483648U;
            main_work.flag |= 4096U;
            main_work.flag |= 2097152U;
            if (GsMainSysIsStageClear(0))
            {
                main_work.is_init_play = false;
                main_work.slct_menu_num = 3;
                main_work.cur_slct_menu = 1;
            }
            else
            {
                main_work.is_init_play = true;
                main_work.slct_menu_num = 3;
                main_work.cur_slct_menu = 0;
            }

            main_work.flag |= 2097152U;
            main_work.cur_crsr_pos_y = !GsTrialIsTrial()
                ? main_work.cur_slct_menu * dm_title_crsr_pos_y_tbl[1] + dm_title_crsr_pos_y_tbl[0]
                : dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
            this.dmTitleSetLoadSysData(main_work);
        }
        else
        {
            int error = XmlStorage.GetLastError();
            main_work.is_init_play = true;
            main_work.slct_menu_num = 3;
            if (error == 1)
            {
                main_work.proc_update = (this.dmTitleProcFileSlctSaveStartWait);
            }
            else
            {
                main_work.proc_update = (this.dmTitleProcMainMenuOpenWin);
                main_work.proc_input = null;
                main_work.flag |= 2147483648U;
                main_work.flag |= 4096U;
                main_work.flag |= 2097152U;
                this.dmTitleSetInitSaveData(main_work);
                this.dmTitleSetInitSysData(main_work);
            }
        }
    }

    private void dmTitleProcFileSlctSaveStartWait(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!DmSaveIsExit())
            return;
        DmSaveStart(4U, true, false);
        main_work.proc_update = (this.dmTitleProcFileSlctWaitDataSave);
    }

    private void dmTitleProcFileSlctWaitDataSave(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!DmSaveIsExit())
            return;
        if (!DmCmnBackupIsSaveSuccessed())
        {
            main_work.proc_update = (this.dmTitleProcWaitInput);
            main_work.proc_input = (this.dmTitleInputProcTitle);
            main_work.flag |= 512U;
            main_work.flag &= 4294966271U;
            main_work.flag &= int.MaxValue;
            main_work.timer = 0.0f;
            DmSndBgmPlayerPlayBgm(1);
        }
        else
        {
            SSave.CreateInstance().Init();
            main_work.is_init_play = true;
            this.dmTitleSetInitSysData(main_work);
            main_work.proc_update = (this.dmTitleProcMainMenuOpenWin);
            main_work.proc_input = null;
            main_work.flag |= 2147483648U;
            main_work.flag |= 2097152U;
        }
    }

    private void dmTitleProcMainMenuOpenWin(DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 4194304) != 0)
        {
            main_work.proc_update = (this.dmTitleProcMainMenuIdle);
            main_work.proc_input = (this.dmTitleInputProcMainMenu);
            main_work.mmenu_win_timer = 0.0f;
            main_work.flag |= 1024U;
            main_work.flag &= 4294966783U;
            main_work.flag &= 4290772991U;
            this.DmTitleOpDispRightEnable(false);
            main_work.cur_slct_menu = GsTrialIsTrial() || main_work.is_init_play ? 0 : 1;
            main_work.cur_crsr_pos_y = !GsTrialIsTrial()
                ? main_work.cur_slct_menu * dm_title_crsr_pos_y_tbl[1] + dm_title_crsr_pos_y_tbl[0]
                : dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
            SaveState.showResumeWarning();
        }
        else
            this.dmTitleSetMMenuWinOpenEfct(main_work);
    }

    private void dmTitleProcMainMenuCloseWin(DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 4194304) != 0)
        {
            AoAccountClearCurrentId();
            SSave.CreateInstance().Init();
            main_work.flag &= int.MaxValue;
            main_work.proc_update = (this.dmTitleProcWaitInput);
            main_work.proc_input = (this.dmTitleInputProcTitle);
            main_work.flag |= 512U;
            main_work.flag &= 4294966271U;
            main_work.flag &= 4292870143U;
            this.DmTitleOpDispRightEnable(true);
            DmSndBgmPlayerPlayBgm(1);
            main_work.mmenu_win_timer = 0.0f;
            main_work.flag &= 4292870143U;
            main_work.flag &= 4290772991U;
        }
        else
            this.dmTitleSetMMenuWinCloseEfct(main_work);
    }

    private void dmTitleProcMainMenuIdle(DMS_TITLE_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (SaveState.shouldResume())
        {
            main_work.cur_slct_menu = 0;
            main_work.flag |= 4U;
        }

        if (((int)main_work.flag & 2) != 0)
        {
            main_work.proc_update = (this.dmTitleProcMainMenuCloseWin);
            main_work.proc_input = null;
            main_work.mmenu_win_timer = 8f;
            main_work.flag &= 4294966271U;
            main_work.flag &= 4294967293U;
            main_work.flag &= 4294967291U;
            DmSoundPlaySE("Cancel");
        }
        else if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = (this.dmTitleProcMainMenuDecideEfct);
            main_work.proc_input = null;
            main_work.timer = 0.0f;
            main_work.flag &= 4294967293U;
            main_work.flag &= 4294967291U;
            DmSoundPlaySE("Ok");
        }
        else if (((int)main_work.flag & 2048) != 0)
        {
            main_work.proc_update = (this.dmTitleProcMainMenuCompBuyIdle);
            main_work.proc_input = null;
            main_work.timer = 0.0f;
            main_work.flag |= 262144U;
            Upsell.launchUpsellScreen(main_work.buy_scr_work);
            main_work.flag &= 4294965247U;
        }
        else
        {
            if (((int)main_work.flag & 64) != 0 || ((int)main_work.flag & 128) != 0)
            {
                this.dmTitleSetChngFocusCrsrData(main_work);
                DmSoundPlaySE("Cursol");
                main_work.flag &= 4294967231U;
                main_work.flag &= 4294967167U;
            }

            if (((int)main_work.flag & 256) == 0)
                return;
            this.dmTitleSetCtrlFocusChangeEfct(main_work);
            if (!this.dmTitleIsCtrlFocusChangeEfctEnd(main_work))
                return;
            main_work.flag &= 4294967039U;
        }
    }

    private void dmTitleProcMainMenuCompBuyFadeOut(DMS_TITLE_MAIN_WORK main_work)
    {
        if (IzFadeIsEnd())
        {
            main_work.proc_update = (this.dmTitleProcMainMenuCompBuyIdle);
            Upsell.launchUpsellScreen(main_work.buy_scr_work);
            main_work.flag &= int.MaxValue;
            main_work.disp_flag &= 4294967293U;
        }

        if (((int)main_work.flag & 256) == 0)
            return;
        this.dmTitleSetCtrlFocusChangeEfct(main_work);
        if (!this.dmTitleIsCtrlFocusChangeEfctEnd(main_work))
            return;
        main_work.flag &= 4294967039U;
    }

    private void dmTitleProcMainMenuCompBuyIdle(DMS_TITLE_MAIN_WORK main_work)
    {
        if (Upsell.showUpsell)
            return;
        if (DmBuyScreenGetResult(main_work.buy_scr_work) == 0)
        {
            if (!GsTrialIsTrial())
            {
                main_work.is_init_play = true;
                main_work.is_no_save_data = true;
                event_after_buy = true;
            }

            if (((int)main_work.flag & 262144) != 0)
                main_work.flag &= 4294705151U;
            else
                IzFadeInitEasy(1U, 0U, 32f);
            DmSndBgmPlayerPlayBgm(0);
            main_work.proc_update = (this.dmTitleProcMainMenuCompBuyFadeIn);
            main_work.disp_flag |= 2U;
        }
        else
        {
            if (((int)main_work.flag & 262144) != 0)
                main_work.flag &= 4294705151U;
            else
                IzFadeInitEasy(1U, 0U, 32f);
            DmSndBgmPlayerPlayBgm(0);
            main_work.proc_update = (this.dmTitleProcMainMenuCompBuyFadeIn);
            main_work.disp_flag |= 2U;
        }
    }

    public static void dmTitleProcMainMenuComUpsellScreenFinished()
    {
        IzFadeInitEasy(1U, 0U, 32f);
        DmSndBgmPlayerPlayBgm(0);
    }

    private void dmTitleProcMainMenuCompBuyFadeIn(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        if (DmBuyScreenGetResult(main_work.buy_scr_work) == 0)
        {
            main_work.proc_update = (this.dmTitleProcMainMenuIdle);
            main_work.proc_input = (this.dmTitleInputProcMainMenu);
            main_work.flag |= 2147483648U;
            if (!GsTrialIsTrial())
            {
                main_work.is_init_play = true;
                main_work.is_no_save_data = true;
                event_after_buy = true;
            }
        }
        else
        {
            main_work.proc_update = (this.dmTitleProcMainMenuIdle);
            main_work.proc_input = (this.dmTitleInputProcMainMenu);
            main_work.flag |= 2147483648U;
        }

        for (int index = 0; index < 5; ++index)
            main_work.decide_menu_frm[index] = 0.0f;
    }

    private void dmTitleProcMainMenuTrophyIdle(DMS_TITLE_MAIN_WORK main_work)
    {
    }

    private void dmTitleProcMainMenuDecideEfct(DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 256) != 0)
        {
            this.dmTitleSetCtrlFocusChangeEfct(main_work);
            if (this.dmTitleIsCtrlFocusChangeEfctEnd(main_work))
                main_work.flag &= 4294967039U;
        }

        int curSlctMenu = main_work.cur_slct_menu;
        if (main_work.timer > 15.0)
        {
            if (curSlctMenu == 4 && (GsTrialIsTrial() || XBOXLive.signinStatus == XBOXLive.SigninStatus.UpdateNeeded ||
                                     XBOXLive.signinStatus == XBOXLive.SigninStatus.Local))
            {
                for (int index = 0; index < 5; ++index)
                    main_work.decide_menu_frm[index] = 0.0f;
                main_work.flag &= 4294967291U;
                main_work.proc_update = (this.dmTitleProcMainMenuIdle);
                main_work.proc_input = (this.dmTitleInputProcMainMenu);
                if (XBOXLive.signinStatus == XBOXLive.SigninStatus.UpdateNeeded)
                {
                    XBOXLive.displayTitleUpdateMessage = true;
                }
                else
                {
                    List<string> stringList = new List<string>();
                    stringList.Add(Sonic4ep1.Strings.ID_OK);
                    string text;

                    if (!GsTrialIsTrial())
                    {
                        text = (XBOXLive.signinStatus != XBOXLive.SigninStatus.Local
                            ? Sonic4ep1.Strings.ID_LB_UPDATE
                            : Sonic4ep1.Strings.ID_LB_OFFLINE);
                    }
                    else
                    {
                        text = Sonic4ep1.Strings.ID_LB_BUY;
                    }

                    AppMain.g_ao_sys_global.is_show_ui = true;
                    Guide.BeginShowMessageBox(" ", text, (IEnumerable<string>)stringList, 0, MessageBoxIcon.Alert, new AsyncCallback(AppMain.TitleMBResult), (object)null);
                }
            }
            else
            {
                if (curSlctMenu == 0 && SaveState.shouldResume())
                {
                    main_work.proc_update = (this.dmTitleProcFadeOut);
                }
                else
                {
                    if (curSlctMenu == 0 && !main_work.is_init_play && !GsTrialIsTrial())
                    {
                        if (this.dmTitleIsSaveRunning())
                        {
                            main_work.proc_update = (this.dmTitleProcMainMenuDelSaveWin);
                            main_work.announce_flag |= 1U;
                            main_work.flag &= 4292869119U;
                            return;
                        }

                        main_work.proc_update = (this.dmTitleProcFadeOut);
                        return;
                    }

                    if (GsTrialIsTrial() && curSlctMenu == 1)
                    {
                        main_work.proc_update = (this.dmTitleProcMainMenuCompBuyFadeOut);
                        main_work.proc_input = null;
                        main_work.timer = 0.0f;
                        IzFadeInitEasy(0U, 1U, 32f);
                        DmSndBgmPlayerBgmStop();
                        return;
                    }

                    main_work.proc_update = (this.dmTitleProcFadeOut);
                }

                main_work.timer = 0.0f;
                for (int index = 0; index < 5; ++index)
                    main_work.decide_menu_frm[index] = 0.0f;
                IzFadeInitEasy(0U, 1U, 32f);
                if (curSlctMenu != 0)
                    return;
                DmSndBgmPlayerExit();
                main_work.flag |= 524288U;
            }
        }
        else
        {
            ++main_work.decide_menu_frm[curSlctMenu];
            ++main_work.timer;
        }
    }

    protected static void TitleMBResult(IAsyncResult userResult)
    {
        g_ao_sys_global.is_show_ui = false;
    }

    private void dmTitleProcMainMenuDelSaveWin(DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 8192) != 0)
        {
            main_work.proc_update = (this.dmTitleProcMainMenuIdle);
            main_work.proc_input = (this.dmTitleInputProcMainMenu);
            main_work.timer = 0.0f;
            main_work.flag &= 4294959103U;
            for (int index = 0; index < 5; ++index)
                main_work.decide_menu_frm[index] = 0.0f;
            main_work.flag |= 2098176U;
        }
        else if (((int)main_work.flag & 16384) != 0)
        {
            SOption instance = SOption.CreateInstance();
            SOption.EControl.Type control = instance.GetControl();
            this.dmTitleSetInitSaveData(main_work);
            instance.SetControl(control);
            main_work.proc_update = (this.dmTitleProcSaveInitData);
            DmSaveMenuStart(false, false);
            main_work.flag &= 4294950911U;
        }

        if (((int)main_work.flag & 256) == 0)
            return;
        this.dmTitleSetCtrlFocusChangeEfct(main_work);
        if (!this.dmTitleIsCtrlFocusChangeEfctEnd(main_work))
            return;
        main_work.flag &= 4294967039U;
    }

    private void dmTitleProcSaveInitData(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!DmSaveIsExit())
            return;
        if (!GsTrialIsTrial() && !DmCmnBackupIsSaveSuccessed())
        {
            main_work.proc_update = (this.dmTitleProcWaitInput);
            main_work.proc_input = (this.dmTitleInputProcTitle);
            main_work.flag |= 512U;
            main_work.flag &= 4294966271U;
            main_work.flag &= int.MaxValue;
            DmSndBgmPlayerPlayBgm(1);
        }
        else
        {
            main_work.proc_update = (this.dmTitleProcFadeOut);
            main_work.timer = 0.0f;
            IzFadeInitEasy(0U, 1U, 32f);
            DmSndBgmPlayerExit();
            main_work.flag |= 524288U;
        }
    }

    private void dmTitleProcFadeOut(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!IzFadeIsEnd())
            return;
        this.DmTitleOpExit();
        main_work.proc_update = (this.dmTitleProcDataRelease);
        main_work.proc_draw = null;
    }

    private void dmTitleProcWindowNodispIdle(DMS_TITLE_MAIN_WORK main_work)
    {
        if (main_work.proc_win_input != null)
            main_work.proc_win_input(main_work);
        if (main_work.announce_flag == 0U)
            return;
        main_work.proc_win_update = (this.dmTitleProcWindowOpenEfct);
        main_work.proc_input = null;
        main_work.proc_win_input = null;
        main_work.win_timer = 0.0f;
        for (int index = 0; index < 5; ++index)
        {
            if ((main_work.announce_flag & (uint)(1 << index)) != 0)
            {
                main_work.win_mode = index;
                break;
            }
        }

        main_work.win_cur_slct = 0;
        DmSoundPlaySE("Window");
        main_work.flag |= 131072U;
    }

    private void dmTitleProcWindowOpenEfct(DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 65536) != 0)
        {
            main_work.proc_win_update = (this.dmTitleProcWindowAnnounceIdle);
            main_work.proc_win_input = (this.dmTitleInputProcWindow);
            main_work.disp_flag |= 1U;
            main_work.flag &= 4294901759U;
        }
        else
            this.dmTitleSetWinOpenEfct(main_work);
    }

    private void dmTitleProcWindowAnnounceIdle(DMS_TITLE_MAIN_WORK main_work)
    {
        if (main_work.proc_win_input != null)
            main_work.proc_win_input(main_work);
        if (main_work.win_mode == 0)
        {
            if (((int)main_work.flag & 4) != 0 && main_work.win_cur_slct == 0)
            {
                main_work.proc_input = null;
                main_work.proc_win_input = null;
                main_work.win_timer = 8f;
                main_work.disp_flag &= 4294967294U;
                main_work.proc_win_update = (this.dmTitleProcWindowCloseEfct);
                DmSoundPlaySE("Ok");
                main_work.flag &= 4294967291U;
                main_work.flag &= 4294967293U;
            }
            else
            {
                if (((int)main_work.flag & 2) == 0 && (main_work.win_cur_slct != 1 || ((int)main_work.flag & 4) == 0))
                    return;
                main_work.proc_input = null;
                main_work.proc_win_input = null;
                main_work.win_timer = 8f;
                main_work.disp_flag &= 4294967294U;
                main_work.proc_win_update = (this.dmTitleProcWindowCloseEfct);
                DmSoundPlaySE("Cancel");
                main_work.flag |= 2U;
                main_work.flag &= 4294967291U;
            }
        }
        else
        {
            if (main_work.win_mode != 1)
                return;
            if (((int)main_work.flag & 4) != 0 && main_work.win_cur_slct == 0)
            {
                main_work.proc_input = null;
                main_work.proc_win_input = null;
                main_work.win_timer = 8f;
                main_work.disp_flag &= 4294967294U;
                main_work.proc_win_update = (this.dmTitleProcWindowCloseEfct);
                DmSoundPlaySE("Ok");
                main_work.flag &= 4294967291U;
                main_work.flag &= 4294967293U;
            }
            else
            {
                if (((int)main_work.flag & 2) == 0 && (main_work.win_cur_slct != 1 || ((int)main_work.flag & 4) == 0))
                    return;
                main_work.proc_input = null;
                main_work.proc_win_input = null;
                main_work.win_timer = 8f;
                main_work.disp_flag &= 4294967294U;
                main_work.proc_win_update = (this.dmTitleProcWindowCloseEfct);
                DmSoundPlaySE("Cancel");
                main_work.flag |= 2U;
                main_work.flag &= 4294967291U;
            }
        }
    }

    private void dmTitleProcWindowCloseEfct(DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 65536) != 0)
        {
            main_work.proc_win_update = (this.dmTitleProcWindowNodispIdle);
            main_work.announce_flag &= (uint)~(1 << main_work.win_mode);
            if (main_work.win_mode == 0 || main_work.win_mode == 1)
            {
                if (((int)main_work.flag & 2) != 0)
                {
                    main_work.flag |= 8192U;
                    main_work.flag &= 4294967293U;
                }
                else if (main_work.win_mode == 0)
                    main_work.announce_flag |= 2U;
                else if (main_work.win_mode == 1)
                    main_work.flag |= 16384U;
            }
            else if (((int)main_work.flag & 2) != 0)
            {
                main_work.flag |= 16777216U;
                main_work.flag &= 4294967293U;
            }
            else if (main_work.win_mode == 2)
                main_work.flag |= 8388608U;
            else if (main_work.win_mode == 3)
                main_work.flag |= 33554432U;
            else
                main_work.flag |= 67108864U;

            main_work.flag &= 4294836223U;
            main_work.flag &= 4294901759U;
        }

        this.dmTitleSetWinCloseEfct(main_work);
    }

    private void dmTitleProcDataRelease(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!this.DmTitleOpExitEndCheck())
            return;
        for (int index = 0; index < 2; ++index)
            AoTexRelease(main_work.tex[index]);
        for (int index = 0; index < 4; ++index)
            AoTexRelease(main_work.cmn_tex[index]);
        this.DmTitleOpFlush();
        main_work.proc_update = (this.dmTitleProcFinish);
    }

    private void dmTitleProcFinish(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!this.dmTitleIsTexRelease(main_work))
            return;
        int length1 = main_work.trg_slct.Length;
        for (int index = 0; index < length1; ++index)
            main_work.trg_slct[index].Release();
        int length2 = main_work.trg_answer.Length;
        for (int index = 0; index < length2; ++index)
            main_work.trg_answer[index].Release();
        main_work.trg_return.Release();
        main_work.trg_game.Release();
        for (int index = 0; index < 41; ++index)
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

        for (int index = 0; index < 4; ++index)
        {
            if (main_work.arc_cmn_amb[index] != null)
                main_work.arc_cmn_amb[index] = null;
        }

        this.DmTitleOpRelease();
        main_work.proc_update = (this.dmTitleProcWaitFinished);
    }

    private void dmTitleProcWaitFinished(DMS_TITLE_MAIN_WORK main_work)
    {
        if (!this.DmTitleOpReleaseCheck())
            return;
        if (((int)main_work.flag & 524288) != 0)
        {
            if (!DmSndBgmPlayerIsTaskExit())
                return;
            main_work.flag &= 4294443007U;
        }

        main_work.flag |= 1U;
        main_work.proc_update = null;
    }

    private void dmTitleInputProcTitle(DMS_TITLE_MAIN_WORK main_work)
    {
        var stand = AoPad.AoPadSomeoneMStand(ControllerConsts.START | ControllerConsts.CONFIRM);
        if (amTpIsTouchPush(0) || stand != -1)
        {
            AoAccountSetCurrentIdStart(stand != -1 ? (uint)stand : 0);
            main_work.flag |= 32U;
        }

        if (!isBackKeyPressed())
            return;
        finish();
        setBackKeyRequest(false);
    }

    private void dmTitleInputProcMainMenu(DMS_TITLE_MAIN_WORK main_work)
    {
        int length = main_work.trg_slct.Length;
        if (!LiveFeature.isInterrupted())
        {
            for (int index = 0; index < length; ++index)
            {
                CTrgAoAction ctrgAoAction = main_work.trg_slct[index];
                if (ctrgAoAction.GetState(0U)[10] && ctrgAoAction.GetState(0U)[1])
                {
                    main_work.cur_slct_menu = index;
                    main_work.flag |= 4U;
                    break;
                }
            }

            var pad = AoPad.AoPadMStand();
            if (pad.HasFlag(ControllerConsts.UP))
            {
                main_work.flag |= 64U;
            }

            if (pad.HasFlag(ControllerConsts.DOWN))
            {
                main_work.flag |= 128U;
            }

            if (pad.HasFlag(ControllerConsts.CONFIRM))
            {
                main_work.flag |= 4U;
            }

            CTrgAoAction trgReturn = main_work.trg_return;
            if (trgReturn.GetState(0U)[10] && trgReturn.GetState(0U)[1] || isBackKeyPressed() ||
                pad.HasFlag(ControllerConsts.B))
            {
                setBackKeyRequest(false);
                if (LiveFeature.isInterrupted())
                    LiveFeature.endInterrupt();
                else
                    main_work.flag |= 2U;
            }


            CTrgAoAction trgGame = main_work.trg_game;
            if (trgGame.GetState(0U)[10] && trgGame.GetState(0U)[1] || pad.HasFlag(ControllerConsts.Y))
            {
                string url = GsEnvGetLanguage() != 0 ? c_url : "http://sega.jp/kt/microsoft/smart/";
                DmSoundPlaySE("Ok");
                erWeb.StartWeb(url);
            }
        }
    }

    private void dmTitleInputProcWindow(DMS_TITLE_MAIN_WORK main_work)
    {
        CTrgAoAction[] trgAnswer = main_work.trg_answer;

        var pad = AoPad.AoPadMStand();
        if (pad.HasFlag(ControllerConsts.LEFT))
        {
            main_work.win_cur_slct = main_work.win_cur_slct == 0 ? 1 : 0;
            DmSoundPlaySE("Cursol");
        }

        if (pad.HasFlag(ControllerConsts.RIGHT))
        {
            main_work.win_cur_slct = main_work.win_cur_slct == 1 ? 0 : 1;
            DmSoundPlaySE("Cursol");
        }

        if (pad.HasFlag(ControllerConsts.CANCEL))
        {
            main_work.win_cur_slct = 1;
            main_work.flag |= 4U;
        }

        if (pad.HasFlag(ControllerConsts.CONFIRM))
        {
            main_work.win_cur_slct = main_work.win_cur_slct == 0 ? 1 : 0;
            main_work.flag |= 4U;
        }

        for (int i = 0; i < 2; i++)
        {
            var trig = trgAnswer[i];
            trig.IsSelected = i == main_work.win_cur_slct;
            if (trgAnswer[0].GetState(0U)[10] && trgAnswer[0].GetState(0U)[1])
            {
                main_work.win_cur_slct = i;
                main_work.flag |= 4U;
            }
        }
    }

    private void dmTitleDrawSetProcDispData(DMS_TITLE_MAIN_WORK main_work)
    {
        float num = 0.0f;
        if (((int)main_work.disp_flag & 2) != 0)
        {
            AoActSysSetDrawTaskPrio(32768U);
            this.DmTitleOpDraw2D();
            AoActSortExecute();
            AoActSortDraw();
            AoActSortUnregAll();
            if (((int)main_work.flag & 512) != 0 && this.DmTitleOpIsLogoActFinish())
                this.dmTitleDrawProcTitle(main_work);
            if (((int)main_work.flag & 2097152) != 0)
            {
                AoActSysSetDrawTaskPrio(35840U);
                if (main_work.mmenu_win_size_rate[0] >= 0.9 || AoAccountIsCurrentEnable())
                {
                    int[] numArray = new int[2] { 800, 340 };
                    GsTrialIsTrial();
                    bool flag = !main_work.is_init_play;
                    AoWinSysDrawState(0, AoTexGetTexList(main_work.cmn_tex[2]), 0U, 480f, 307f,
                        numArray[0] * main_work.mmenu_win_size_rate[0],
                        (float)(numArray[1] * (double)main_work.mmenu_win_size_rate[1] * 0.899999976158142) + num,
                        10U);
                }
            }

            if (((int)main_work.flag & 1024) != 0)
            {
                if (((int)main_work.flag_prev & 1024) == 0)
                {
                    main_work.trg_return.ResetState();
                    int index1 = 15;
                    for (int index2 = 17; index1 < index2; ++index1)
                        AoActSetFrame(main_work.act[index1], 0.0f);
                    main_work.trg_game.ResetState();
                    int index3 = 17;
                    for (int index2 = 19; index3 < index2; ++index3)
                        AoActSetFrame(main_work.act[index3], 0.0f);
                }

                this.dmTitleDrawProcMainMenu(main_work);
            }

            if (((int)main_work.flag & 131072) != 0)
                this.dmTitleWinSelectDraw(main_work);
        }

        amDrawMakeTask(new TaskProc(this.dmTitleTaskDraw), 32768, 0U);
    }

    private void dmTitleDrawProcTitle(DMS_TITLE_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(33792U);
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        if (((int)main_work.flag & 8) != 0)
            AoActSortRegAction(main_work.act[19]);
        if (((int)main_work.flag & 16) != 0)
        {
            if (main_work.disp_timer >= 4.0)
            {
                main_work.disp_timer = 0.0f;
                main_work.flag ^= 8U;
            }
            else
                ++main_work.disp_timer;
        }
        else if (main_work.disp_timer >= (double)main_work.disp_change_time)
        {
            main_work.flag ^= 8U;
            main_work.disp_change_time = ((int)main_work.flag & 8) == 0 ? 30 : 40;
            main_work.disp_timer = 0.0f;
        }
        else
            ++main_work.disp_timer;

        if (GeEnvGetDecideKey() == GSE_DECIDE_KEY.GSD_DECIDE_KEY_O)
            AoActSetFrame(main_work.act[19], 0.0f);
        else
            AoActSetFrame(main_work.act[19], 1f);
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        AoActUpdate(main_work.act[19], 0.0f);
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private void dmTitleDrawProcMainMenu(DMS_TITLE_MAIN_WORK main_work)
    {
        bool is_trial = GsTrialIsTrial();
        bool has_save_data = !main_work.is_init_play;
        this.dmTitleDrawProcMainMenuIphone(main_work, has_save_data, is_trial);
    }

    private void CLocalBtnBase_Update(
        AOS_ACTION src,
        ArrayPointer<AOS_ACTION> btn,
        CTrgAoAction trg)
    {
        this.CLocalBtnBase_Update(src, btn, trg, 0.0f);
    }

    private void CLocalBtnBase_Update(
        AOS_ACTION src,
        ArrayPointer<AOS_ACTION> btn,
        CTrgAoAction trg,
        float frame)
    {
        AoActAcmPush();
        AoActAcmInit();
        int[] numArray = new int[2]
        {
            (int) src.sprite.center_x,
            (int) src.sprite.center_y
        };
        AoActAcmApplyTrans(numArray[0], numArray[1], 1f);
        AoActUpdate(btn[0], frame);
        AoActUpdate(btn[1], frame);
        AoActUpdate(btn[2], frame);
        trg.Update();
        AoActAcmPop();
    }

    private void CLocalBtnBase_Draw(ArrayPointer<AOS_ACTION> btn)
    {
        AoActSortRegAction(btn[0]);
        AoActSortRegAction(btn[1]);
        AoActSortRegAction(btn[2]);
    }

    private void CLocalBtnBase_SetFrame(ArrayPointer<AOS_ACTION> btn, float frame)
    {
        AoActSetFrame(btn[0], frame);
        AoActSetFrame(btn[1], frame);
        AoActSetFrame(btn[2], frame);
    }

    private void dmTitleDrawProcMainMenuIphone(
        DMS_TITLE_MAIN_WORK main_work,
        bool has_save_data,
        bool is_trial)
    {
        AoActSysSetDrawTaskPrio(33792U);
        int[] numArray1 = new int[3] { 20, 21, 22 };
        bool flag = has_save_data;
        int[] numArray2 = new int[3] { 0, 3, 6 };
        float[] numArray3 = new float[3];
        if (is_trial)
        {
            numArray1[1] = 23;
            flag = true;
        }

        if ((this.dmTitleProcMainMenuDecideEfct) == main_work.proc_update)
            numArray3[main_work.cur_slct_menu] = 1f;
        else if ((!IzFadeIsExe() || IzFadeIsEnd()) &&
                 (!((this.dmTitleProcSaveInitData) == main_work.proc_update) &&
                  (131072 & (int)main_work.flag) == 0))
        {
            for (int index = 0; index < 3; ++index)
            {
                float frame;
                if (!main_work.trg_slct[index].GetState(0U)[10] || !main_work.trg_slct[index].GetState(0U)[1])
                    frame = !main_work.trg_slct[index].GetState(0U)[0] ? 0.0f : 1f;
                else
                    frame = 2f;
                AoActSetFrame(main_work.act[numArray1[index]], frame);
                this.CLocalBtnBase_SetFrame(new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[index]), frame);
            }

            float frame1;
            if (!main_work.trg_return.GetState(0U)[10] || !main_work.trg_return.GetState(0U)[1])
                if (!main_work.trg_return.GetState(0U)[0])
                    frame1 = 2.0 > main_work.act[15].frame ? 0.0f : main_work.act[15].frame;
                else
                    frame1 = 1f;
            else
                frame1 = 2f;

            AoActSetFrame(main_work.act[26], frame1);
            int index1 = 15;
            for (int index2 = 17; index1 < index2; ++index1)
                AoActSetFrame(main_work.act[index1], frame1);
            float frame2;

            if (!main_work.trg_game.GetState(0U)[10] || !main_work.trg_game.GetState(0U)[1])
                if (!main_work.trg_game.GetState(0U)[0])
                    frame2 = 2.0 > main_work.act[17].frame ? 0.0f : main_work.act[17].frame;
                else
                    frame2 = 1f;
            else
                frame2 = 2f;

            AoActSetFrame(main_work.act[27], frame2);
            int index3 = 17;
            for (int index2 = 19; index3 < index2; ++index3)
                AoActSetFrame(main_work.act[index3], frame2);
        }

        if (!LiveFeature.isInterrupted())
        {
            if (!flag)
            {
                main_work.ama[1].act_tbl[(int)dm_title_g_dm_act_id_tbl[20]].mtn.trs_tbl[0].trs_y = 250f;
                main_work.ama[1].act_tbl[(int)dm_title_g_dm_act_id_tbl[22]].mtn.trs_tbl[0].trs_y = 360f;
                main_work.ama[1].act_tbl[(int)dm_title_g_dm_act_id_tbl[24]].mtn.trs_tbl[0].trs_y = 470f;
                main_work.ama[1].act_tbl[(int)dm_title_g_dm_act_id_tbl[25]].mtn.trs_tbl[0].trs_y = 580f;
            }
            else
                main_work.ama[1].act_tbl[(int)dm_title_g_dm_act_id_tbl[21]].mtn.trs_tbl[0].trs_y = 305f;

            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            AoActUpdate(main_work.act[numArray1[0]], numArray3[0]);
            if (flag)
            {
                AoActUpdate(main_work.act[numArray1[1]], numArray3[1]);
                main_work.act[numArray1[1]].sprite.center_y = 305f;
            }

            AoActUpdate(main_work.act[numArray1[2]], numArray3[2]);
            //AoActUpdate(main_work.act[numArray1[3]], numArray3[3]);
            //AoActUpdate(main_work.act[numArray1[4]], numArray3[4]);
            AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
            this.CLocalBtnBase_Update(main_work.act[numArray1[0]],
                new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[0]), main_work.trg_slct[0], numArray3[0]);
            if (flag)
                this.CLocalBtnBase_Update(main_work.act[numArray1[1]],
                    new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[1]), main_work.trg_slct[1], numArray3[1]);
            this.CLocalBtnBase_Update(main_work.act[numArray1[2]],
                new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[2]), main_work.trg_slct[2], numArray3[2]);
            // this.CLocalBtnBase_Update(main_work.act[numArray1[3]], new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[3]), main_work.trg_slct[3], numArray3[3]);
            // this.CLocalBtnBase_Update(main_work.act[numArray1[4]], new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[4]), main_work.trg_slct[4], numArray3[4]);
        }

        float frame3 = 2.0 <= main_work.act[15].frame ? 1f : 0.0f;
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        AoActUpdate(main_work.act[26], frame3);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        int index4 = 15;
        for (int index1 = 17; index4 < index1; ++index4)
            AoActUpdate(main_work.act[index4], frame3);
        main_work.trg_return.Update();
        float frame4 = 2.0 <= main_work.act[17].frame ? 1f : 0.0f;
        AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
        AoActUpdate(main_work.act[27], frame4);
        AoActSetTexture(AoTexGetTexList(main_work.tex[0]));
        int index5 = 17;
        for (int index1 = 19; index5 < index1; ++index5)
            AoActUpdate(main_work.act[index5], frame4);
        main_work.trg_game.Update();
        if (!LiveFeature.isInterrupted())
        {
            this.CLocalBtnBase_Draw(new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[0]));
            AoActSortRegAction(main_work.act[numArray1[0]]);
            main_work.act[numArray1[0]].sprite.center_y -= 3f;
            if (flag)
            {
                this.CLocalBtnBase_Draw(new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[1]));
                AoActSortRegAction(main_work.act[numArray1[1]]);
                main_work.act[numArray1[1]].sprite.center_y -= 3f;
            }

            this.CLocalBtnBase_Draw(new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[2]));
            AoActSortRegAction(main_work.act[numArray1[2]]);
            main_work.act[numArray1[2]].sprite.center_y -= 3f;
            // this.CLocalBtnBase_Draw(new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[3]));
            // AoActSortRegAction(main_work.act[numArray1[3]]);
            // main_work.act[numArray1[3]].sprite.center_y -= 3f;
            // this.CLocalBtnBase_Draw(new ArrayPointer<AOS_ACTION>(main_work.act, numArray2[4]));
            // AoActSortRegAction(main_work.act[numArray1[4]]);
            // main_work.act[numArray1[4]].sprite.center_y -= 3f;
        }

        if ((2097152 & (int)main_work.flag) != 0 && (131072 & (int)main_work.flag) == 0)
        {
            int index1 = 15;
            for (int index2 = 17; index1 < index2; ++index1)
                AoActSortRegAction(main_work.act[index1]);
            AoActSortRegAction(main_work.act[26]);
            int index3 = 17;
            for (int index2 = 19; index3 < index2; ++index3)
                AoActSortRegAction(main_work.act[index3]);
            AoActSortRegAction(main_work.act[27]);
        }

        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private void dmTitleWinSelectDraw(DMS_TITLE_MAIN_WORK main_work)
    {
        AoActSysSetDrawTaskPrio(35840U);
        AoWinSysDrawState(0, AoTexGetTexList(main_work.cmn_tex[2]), 0U, 480f, 356f,
            708.75f * main_work.win_size_rate[0], (float)(303.75 * main_work.win_size_rate[1] * 0.899999976158142),
            10U);
        if (((int)main_work.disp_flag & 1) == 0)
            return;
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[2]));
        AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
        switch (main_work.win_mode)
        {
            case 0:
                int index1 = 32;
                for (int index2 = 38; index1 < index2; ++index1)
                    AoActSortRegAction(main_work.act[index1]);
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[28]);
                AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
                AoActSortRegAction(main_work.act[38]);
                AoActSortRegAction(main_work.act[39]);
                break;
            case 1:
                int index3 = 32;
                for (int index2 = 38; index3 < index2; ++index3)
                    AoActSortRegAction(main_work.act[index3]);
                AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
                AoActSortRegAction(main_work.act[29]);
                AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
                AoActSortRegAction(main_work.act[38]);
                AoActSortRegAction(main_work.act[39]);
                break;
        }

        AoActAcmPush();
        for (int index2 = 0; index2 < 3; ++index2)
        {
            AoActAcmInit();
            AoActAcmApplyTrans(dm_title_win_act_pos_tbl[index2 + 1, 0], dm_title_win_act_pos_tbl[index2 + 1, 1], 0.0f);
            if (index2 <= 1)
                AoActAcmApplyScale(27f / 16f, 27f / 16f);
            AoActSetTexture(AoTexGetTexList(main_work.tex[1]));
            AoActUpdate(main_work.act[28 + index2], 0.0f);
        }

        for (int index2 = 1; index2 < 3; ++index2)
        {
            int num = 37;
            AoActAcmInit();
            AoActAcmApplyTrans(dm_title_win_act_pos_tbl[index2 + 4, 0], dm_title_win_act_pos_tbl[index2 + 4, 1], 0.0f);
            AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[3]));
            AoActUpdate(main_work.act[num + index2], 0.0f);
            main_work.act[num + index2].sprite.center_y += 5f;
            AoActSetTexture(AoTexGetTexList(main_work.cmn_tex[2]));
            int[,] numArray1 = new int[3, 2]
            {
                {0, -1}, {35, 37}, {32, 34}
            };
            AoActUpdate(main_work.act[numArray1[index2, 0] + 1], 0.0f);
            int[] numArray2 = new int[3] { -1, 1, 0 };
            CTrgAoAction ctrgAoAction = main_work.trg_answer[numArray2[index2]];
            if (0 <= numArray2[index2])
                ctrgAoAction.Update();
            float frame = ctrgAoAction.GetState(0U)[0] ? 1f : 0.0f;
            int index4 = numArray1[index2, 0];
            for (int index5 = numArray1[index2, 1] + 1; index4 < index5; ++index4)
            {
                AoActSetFrame(main_work.act[index4], frame);
                AoActUpdate(main_work.act[index4], 0.0f);
            }
        }

        AoActAcmPop();
        AoActSortExecute();
        AoActSortDraw();
        AoActSortUnregAll();
    }

    private void dmTitleTaskDraw(AMS_TCB tcb)
    {
        AoActDrawPre();
        amDrawExecCommand(10U);
        amDrawEndScene();
    }

    private bool dmTitleIsDataLoad(DMS_TITLE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!amFsIsComplete(main_work.arc_amb_fs[index]))
                return false;
        }

        for (int index = 0; index < 4; ++index)
        {
            if (!amFsIsComplete(main_work.arc_cmn_amb_fs[index]))
                return false;
        }

        return this.DmTitleOpLoadCheck();
    }

    private bool dmTitleIsTexLoad(DMS_TITLE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AoTexIsLoaded(main_work.tex[index]))
                return false;
        }

        for (int index = 0; index < 4; ++index)
        {
            if (!AoTexIsLoaded(main_work.cmn_tex[index]))
                return false;
        }

        return GsFontIsBuilded() && this.DmTitleOpBuildCheck();
    }

    private bool dmTitleIsTexRelease(DMS_TITLE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AoTexIsReleased(main_work.tex[index]))
                return false;
        }

        for (int index = 0; index < 4; ++index)
        {
            if (!AoTexIsReleased(main_work.cmn_tex[index]))
                return false;
        }

        return this.DmTitleOpFlushCheck();
    }

    private void dmTitleSetChngFocusCrsrData(DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 64) != 0)
        {
            main_work.prev_slct_menu = main_work.cur_slct_menu;
            main_work.cur_slct_menu =
                this.dmTitleGetRevisedMenuFocus(main_work.cur_slct_menu, -1, main_work.slct_menu_num);
            main_work.src_crsr_pos_y = main_work.cur_crsr_pos_y;
            main_work.dst_crsr_pos_y = !GsTrialIsTrial()
                ? dm_title_crsr_pos_y_tbl[0] + main_work.cur_slct_menu * dm_title_crsr_pos_y_tbl[1]
                : dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
            main_work.flag |= 256U;
        }

        if (((int)main_work.flag & 128) == 0)
            return;
        main_work.prev_slct_menu = main_work.cur_slct_menu;
        main_work.cur_slct_menu = this.dmTitleGetRevisedMenuFocus(main_work.cur_slct_menu, 1, main_work.slct_menu_num);
        main_work.src_crsr_pos_y = main_work.cur_crsr_pos_y;
        main_work.dst_crsr_pos_y = !GsTrialIsTrial()
            ? dm_title_crsr_pos_y_tbl[0] + main_work.cur_slct_menu * dm_title_crsr_pos_y_tbl[1]
            : dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
        main_work.flag |= 256U;
    }

    private int dmTitleGetRevisedMenuFocus(int idx, int diff, int menu_num)
    {
        int num = idx + diff;
        if (num < 0)
            num = menu_num - 1;
        if (num >= menu_num)
            num = 0;
        return num;
    }

    private void dmTitleSetCtrlFocusChangeEfct(DMS_TITLE_MAIN_WORK main_work)
    {
        float[] numArray1 = new float[2];
        float[] numArray2 = new float[2]
        {
            0.0f,
            main_work.dst_crsr_pos_y - main_work.src_crsr_pos_y
        };
        numArray1[1] = numArray2[1] / 8f;
        main_work.cur_crsr_pos_y += numArray1[1];
    }

    private void dmTitleSetWinOpenEfct(DMS_TITLE_MAIN_WORK main_work)
    {
        if (main_work.win_timer > 8.0)
        {
            main_work.flag |= 65536U;
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

    private void dmTitleSetWinCloseEfct(DMS_TITLE_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
            main_work.win_size_rate[(int)index] = main_work.win_timer == 0.0 ? 0.0f : main_work.win_timer / 8f;
        if (main_work.win_timer < 0.0)
        {
            main_work.flag |= 65536U;
            main_work.win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int)index] = 0.0f;
        }
        else
            --main_work.win_timer;
    }

    private void dmTitleSetMMenuWinOpenEfct(DMS_TITLE_MAIN_WORK main_work)
    {
        if (0.0 == main_work.mmenu_win_timer)
            DmSoundPlaySE("Window");
        if (main_work.mmenu_win_timer > 8.0)
        {
            main_work.flag |= 4194304U;
            main_work.mmenu_win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.mmenu_win_size_rate[(int)index] = 1f;
        }
        else
            ++main_work.mmenu_win_timer;

        for (uint index = 0; index < 2U; ++index)
        {
            main_work.mmenu_win_size_rate[(int)index] =
                main_work.mmenu_win_timer == 0.0 ? 1f : main_work.mmenu_win_timer / 8f;
            if (main_work.mmenu_win_size_rate[(int)index] > 1.0)
                main_work.mmenu_win_size_rate[(int)index] = 1f;
        }
    }

    private void dmTitleSetMMenuWinCloseEfct(DMS_TITLE_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
            main_work.mmenu_win_size_rate[(int)index] =
                main_work.mmenu_win_timer == 0.0 ? 0.0f : main_work.mmenu_win_timer / 8f;
        if (main_work.mmenu_win_timer < 0.0)
        {
            main_work.flag |= 4194304U;
            main_work.mmenu_win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.mmenu_win_size_rate[(int)index] = 0.0f;
        }
        else
            --main_work.mmenu_win_timer;
    }

    private bool dmTitleIsCtrlFocusChangeEfctEnd(DMS_TITLE_MAIN_WORK main_work)
    {
        float num = main_work.dst_crsr_pos_y - main_work.src_crsr_pos_y;
        if (main_work.cur_crsr_pos_y >= (double)main_work.dst_crsr_pos_y && num >= 0.0)
        {
            main_work.cur_crsr_pos_y = main_work.dst_crsr_pos_y;
            main_work.timer = 0.0f;
            return true;
        }

        if (main_work.cur_crsr_pos_y <= (double)main_work.dst_crsr_pos_y && num <= 0.0)
        {
            main_work.cur_crsr_pos_y = main_work.dst_crsr_pos_y;
            main_work.timer = 0.0f;
            return true;
        }

        ++main_work.timer;
        return false;
    }

    private void dmTitleSetLoadSysData(DMS_TITLE_MAIN_WORK main_work)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        uint num = 0;
        SOption instance1 = SOption.CreateInstance();
        int volumeBgm = (int)instance1.GetVolumeBgm();
        int volumeSe = (int)instance1.GetVolumeSe();
        mainSysInfo.bgm_volume = volumeBgm / 10;
        mainSysInfo.se_volume = volumeSe / 10;
        DmSoundSetVolumeBGM(mainSysInfo.bgm_volume);
        DmSoundSetVolumeSE(mainSysInfo.se_volume);
        mainSysInfo.game_flag |= 64U;
        SSpecial instance2 = SSpecial.CreateInstance();
        for (int index = 0; index < 7; ++index)
        {
            if (instance2[(ushort)index].IsGetEmerald())
                num |= (uint)(1 << index);
        }

        if (num == (uint)sbyte.MaxValue)
            mainSysInfo.game_flag |= 32U;
        else
            mainSysInfo.game_flag &= 4294967263U;
        SSystem instance3 = SSystem.CreateInstance();
        mainSysInfo.rest_player_num = instance3.PlayerStock;
        mainSysInfo.ene_kill_count = instance3.Killed;
        mainSysInfo.final_clear_count = instance3.ClearCount;
        switch (SOption.CreateInstance().GetControl())
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

        mainSysInfo.is_spe_clear = false;
        mainSysInfo.is_first_play = false;
    }

    private void dmTitleSetInitSysData(DMS_TITLE_MAIN_WORK main_work)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        SOption instance = SOption.CreateInstance();
        int volumeBgm = (int)instance.GetVolumeBgm();
        int volumeSe = (int)instance.GetVolumeSe();
        mainSysInfo.bgm_volume = volumeBgm / 10f;
        mainSysInfo.se_volume = volumeSe / 10f;
        DmSoundSetVolumeBGM(mainSysInfo.bgm_volume);
        DmSoundSetVolumeSE(mainSysInfo.se_volume);
        instance.IsVibration();
        mainSysInfo.game_flag |= 64U;
        mainSysInfo.game_flag &= 4294967263U;
        mainSysInfo.rest_player_num = 3U;
        mainSysInfo.is_spe_clear = false;
        mainSysInfo.is_first_play = false;
    }

    private void dmTitleSetMenuInfo(DMS_TITLE_MAIN_WORK main_work)
    {
        if (GsTrialIsTrial())
        {
            main_work.slct_menu_num = 3;
            main_work.is_init_play = true;
        }
        else if (!GsMainSysIsStageClear(0))
        {
            main_work.slct_menu_num = 3;
            main_work.is_init_play = true;
        }
        else
        {
            main_work.slct_menu_num = 3;
            main_work.is_init_play = false;
        }
    }

    private void dmTitleSetFirstPlayData(DMS_TITLE_MAIN_WORK main_work)
    {
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        SSystem instance = SSystem.CreateInstance();
        mainSysInfo.rest_player_num = 3U;
        instance.PlayerStock = mainSysInfo.rest_player_num;
        this.dmTitleSetLoadSysData(main_work);
    }

    private void dmTitleSetInitSaveData(DMS_TITLE_MAIN_WORK main_work)
    {
        GsGetMainSysInfo().Save.Init();
    }

    private bool dmTitleIsSaveRunning()
    {
        return GsGetMainSysInfo().is_save_run != 0U;
    }

    private bool dmTitleIsChangeOptVol()
    {
        SOption instance = SOption.CreateInstance();
        return instance.GetVolumeBgm() != 100U || instance.GetVolumeSe() != 100U;
    }
}