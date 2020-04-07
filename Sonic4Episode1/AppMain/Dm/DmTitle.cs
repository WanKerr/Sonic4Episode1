using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using er;
using er.web;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private void DmTitleStart(object arg)
    {
        AppMain.dm_title_is_title_start = true;
        int oldEvtId = (int)AppMain.SyGetEvtInfo().old_evt_id;
        if (AppMain.dm_title_is_title_start)
            SSave.CreateInstance().Init();
        this.dmTitleInit();
    }

    private void DmMainMenuStart(object arg)
    {
        AppMain.dm_title_is_title_start = false;
        this.dmTitleInit();
    }

    private void dmTitleInit()
    {
        GC.Collect();
        AppMain.AoActSysSetDrawStateEnable(true);
        AppMain.AoActSysSetDrawState(10U);
        AppMain.DMS_TITLE_MAIN_WORK work = (AppMain.DMS_TITLE_MAIN_WORK)AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(this.dmTitleProcMain), new AppMain.GSF_TASK_PROCEDURE(this.dmTitleDest), 0U, (ushort)0, 8192U, 10, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_TITLE_MAIN_WORK()), "TITLE_MAIN").work;
        this.dmTitleSetInitDispData(work);
        if (AppMain.dm_title_is_title_start)
            AppMain.AoAccountClearCurrentId();
        work.is_jp_region = AppMain.GeEnvGetDecideKey() == AppMain.GSE_DECIDE_KEY.GSD_DECIDE_KEY_O;
        work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleLoadFontData);
    }

    private void dmTitleSetInitDispData(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        switch (AppMain.SyGetEvtInfo().old_evt_id)
        {
            case 5:
                main_work.cur_slct_menu = 1;
                break;
            case 6:
                main_work.cur_slct_menu = 0;
                break;
            case 8:
                main_work.cur_slct_menu = !AppMain.GsMainSysIsStageClear(0) || AppMain.GsTrialIsTrial() ? 1 : 2;
                break;
            case 10:
                main_work.cur_slct_menu = 1;
                break;
            default:
                main_work.cur_slct_menu = 0;
                break;
        }
        main_work.disp_flag |= 2U;
        if (AppMain.GsTrialIsTrial())
            main_work.cur_crsr_pos_y = AppMain.dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
        else
            main_work.cur_crsr_pos_y = (float)main_work.cur_slct_menu * AppMain.dm_title_crsr_pos_y_tbl[1] + AppMain.dm_title_crsr_pos_y_tbl[0];
    }

    private void dmTitleProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_TITLE_MAIN_WORK work = (AppMain.DMS_TITLE_MAIN_WORK)tcb.work;
        work.flag_prev = work.flag;
        if (((int)work.flag & 1) != 0)
        {
            AppMain.mtTaskClearTcb(tcb);
            this.dmTitleSetNextEvent(work);
        }
        if (((int)work.flag & int.MinValue) != 0 && AppMain.event_after_buy)
        {
            work.is_init_play = true;
            work.is_no_save_data = true;
            AppMain.event_after_buy = false;
            work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFadeOut);
            work.flag &= (uint)int.MaxValue;
            work.flag |= 1073741824U;
            AppMain.IzFadeInitEasy(1U, 1U, 32f);
            AppMain.DmSndBgmPlayerExit();
            work.flag |= 524288U;
            work.flag &= 4294967291U;
            work.flag &= 4294967293U;
            work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
            work.proc_win_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
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

    private void dmTitleDest(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private void dmTitleSetNextEvent(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        short evt_case1 = 0;
        if (((int)main_work.flag & 1073741824) != 0)
        {
            short evt_case2 = 5;
            main_work.flag &= 3221225471U;
            AppMain.SyDecideEvtCase(evt_case2);
            AppMain.SyChangeNextEvt();
        }
        else if (((int)main_work.flag & 1048576) != 0)
        {
            short evt_case2 = 4;
            main_work.flag &= 4293918719U;
            AppMain.SyDecideEvtCase(evt_case2);
            AppMain.SyChangeNextEvt();
        }
        else
        {
            switch (main_work.cur_slct_menu)
            {
                case 0:
                    evt_case1 = (short)0;
                    this.dmTitleSetFirstPlayData(main_work);
                    AppMain.nextDemoLevel = 0;
                    AppMain.g_gs_main_sys_info.stage_id = (ushort)0;
                    AppMain.g_gs_main_sys_info.char_id[0] = 0;
                    AppMain.g_gs_main_sys_info.game_mode = 0;
                    if (AppMain.GsTrialIsTrial())
                        AppMain.g_gs_main_sys_info.rest_player_num = 3U;
                    AppMain.g_gs_main_sys_info.game_flag &= 4294967167U;
                    AppMain.GmMainGSInit();
                    break;
                case 1:
                    if (!AppMain.GsMainSysIsStageClear(0) || main_work.is_init_play)
                    {
                        evt_case1 = (short)0;
                        AppMain.g_gs_main_sys_info.stage_id = (ushort)0;
                        AppMain.g_gs_main_sys_info.char_id[0] = 0;
                        AppMain.g_gs_main_sys_info.game_mode = 0;
                        AppMain.g_gs_main_sys_info.game_flag &= 4294967167U;
                        AppMain.GmMainGSInit();
                        break;
                    }
                    evt_case1 = (short)1;
                    break;
                case 2:
                    evt_case1 = (short)2;
                    AppMain.dm_opt_show_xboxlive = false;
                    break;
                case 3:
                    evt_case1 = (short)2;
                    LiveFeature.getInstance().ShowAchievements();
                    AppMain.dm_opt_show_xboxlive = true;
                    break;
                case 4:
                    evt_case1 = (short)2;
                    LiveFeature.getInstance().ShowLeaderboards();
                    AppMain.dm_opt_show_xboxlive = true;
                    break;
            }
            AppMain.SyDecideEvtCase(evt_case1);
            AppMain.SyChangeNextEvt();
        }
    }

    private void dmTitleLoadFontData(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        AppMain.GsFontBuild();
        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleIsLoadFontData);
    }

    private void dmTitleIsLoadFontData(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!AppMain.GsFontIsBuilded())
            return;
        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleLoadRequest);
    }

    private void dmTitleLoadRequest(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        main_work.arc_amb_fs[0] = AppMain.amFsReadBackground("DEMO/TITLE/D_TITLE.AMB");
        main_work.arc_amb_fs[1] = AppMain.amFsReadBackground(AppMain.dm_title_file_lng_amb_name_tbl[AppMain.GsEnvGetLanguage()]);
        for (int index = 0; index < 3; ++index)
            main_work.arc_cmn_amb_fs[index] = AppMain.amFsReadBackground(AppMain.dm_title_menu_cmn_amb_name_tbl[index]);
        main_work.arc_cmn_amb_fs[3] = AppMain.amFsReadBackground(AppMain.dm_title_menu_cmn_lng_amb_name_tbl[AppMain.GsEnvGetLanguage()]);
        this.DmTitleOpLoad();
        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcLoadWait);
    }

    private void dmTitleProcLoadWait(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!this.dmTitleIsDataLoad(main_work))
            return;
        for (int index1 = 0; index1 < 2; ++index1)
        {
            main_work.arc_amb[index1] = AppMain.readAMBFile(main_work.arc_amb_fs[index1]);
            main_work.ama[index1] = AppMain.readAMAFile(AppMain.amBindGet(main_work.arc_amb[index1], 0));
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
                AppMain.A2S_AMA_ACT[] a2SAmaActArray = new AppMain.A2S_AMA_ACT[(int)main_work.ama[index1].act_num];
                Array.Copy((Array)main_work.ama[index1].act_tbl, (Array)a2SAmaActArray, (int)main_work.ama[index1].act_num - 2);
                uint num = main_work.ama[index1].act_num - 2U;
                a2SAmaActArray[(int)num] = new AppMain.A2S_AMA_ACT();
                a2SAmaActArray[(int)num].Assign(a2SAmaActArray[0]);
                a2SAmaActArray[(int)(num + 1U)] = new AppMain.A2S_AMA_ACT();
                a2SAmaActArray[(int)(num + 1U)].Assign(a2SAmaActArray[1]);
                a2SAmaActArray[(int)num].anm.anm_tbl[0].tex_id = 9;
                a2SAmaActArray[(int)(num + 1U)].anm.anm_tbl[0].tex_id = 10;
                main_work.ama[index1].act_tbl = a2SAmaActArray;
            }
            string sPath;
            main_work.amb[index1] = AppMain.readAMBFile(AppMain.amBindGet(main_work.arc_amb[index1], 1, out sPath));
            main_work.amb[index1].dir = sPath;
            AppMain.amFsClearRequest(main_work.arc_amb_fs[index1]);
            main_work.arc_amb_fs[index1] = (AppMain.AMS_FS)null;
            AppMain.AoTexBuild(main_work.tex[index1], main_work.amb[index1]);
            if (index1 == 1)
            {
                main_work.tex[index1].txb.texfilelist.nTex += 2;
                AppMain.NNS_TEXFILE[] nnsTexfileArray = AppMain.New<AppMain.NNS_TEXFILE>(main_work.tex[index1].txb.texfilelist.nTex);
                for (int index2 = 0; index2 < main_work.tex[index1].txb.texfilelist.nTex; ++index2)
                {
                    int index3 = index2 < main_work.tex[index1].txb.texfilelist.nTex - 2 ? index2 : main_work.tex[index1].txb.texfilelist.nTex - 2 - 1;
                    nnsTexfileArray[index2].Bank = main_work.tex[index1].txb.texfilelist.pTexFileList[index3].Bank;
                    nnsTexfileArray[index2].fType = main_work.tex[index1].txb.texfilelist.pTexFileList[index3].fType;
                    nnsTexfileArray[index2].Filename = main_work.tex[index1].txb.texfilelist.pTexFileList[index3].Filename;
                    nnsTexfileArray[index2].MinFilter = main_work.tex[index1].txb.texfilelist.pTexFileList[index3].MinFilter;
                    nnsTexfileArray[index2].MagFilter = main_work.tex[index1].txb.texfilelist.pTexFileList[index3].MagFilter;
                    nnsTexfileArray[index2].GlobalIndex = main_work.tex[index1].txb.texfilelist.pTexFileList[index3].GlobalIndex;
                }
                switch (AppMain.GsEnvGetLanguage())
                {
                    case 0:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_JP.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_JP.PNG";
                        break;
                    case 1:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_US.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_US.PNG";
                        break;
                    case 2:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_FR.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_FR.PNG";
                        break;
                    case 3:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_IT.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_IT.PNG";
                        break;
                    case 4:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_GE.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_GE.PNG";
                        break;
                    case 5:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_SP.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_SP.PNG";
                        break;
                    case 6:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_FI.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_FI.PNG";
                        break;
                    case 7:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_PT.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_PT.PNG";
                        break;
                    case 8:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_RU.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_RU.PNG";
                        break;
                    case 9:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_CN.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_CN.PNG";
                        break;
                    case 10:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_HK.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_HK.PNG";
                        break;
                    default:
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 2].Filename = "D_TITLE_TEX_TROPHY_US.PNG";
                        nnsTexfileArray[main_work.tex[index1].txb.texfilelist.nTex - 1].Filename = "D_TITLE_TEX_LB_US.PNG";
                        break;
                }
                main_work.tex[index1].txb.texfilelist.pTexFileList = nnsTexfileArray;
            }
            AppMain.AoTexLoad(main_work.tex[index1]);
        }
        for (int index = 0; index < 4; ++index)
        {
            main_work.arc_cmn_amb[index] = AppMain.readAMBFile(main_work.arc_cmn_amb_fs[index]);
            main_work.cmn_ama[index] = AppMain.readAMAFile(AppMain.amBindGet(main_work.arc_cmn_amb[index], 0));
            string sPath;
            main_work.cmn_amb[index] = AppMain.readAMBFile(AppMain.amBindGet(main_work.arc_cmn_amb[index], 1, out sPath));
            main_work.cmn_amb[index].dir = sPath;
            AppMain.amFsClearRequest(main_work.arc_cmn_amb_fs[index]);
            main_work.arc_cmn_amb_fs[index] = (AppMain.AMS_FS)null;
            AppMain.AoTexBuild(main_work.cmn_tex[index], main_work.cmn_amb[index]);
            AppMain.AoTexLoad(main_work.cmn_tex[index]);
        }
        this.DmTitleOpBuild();
        AppMain.DmSndBgmPlayerInit();
        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcTexBuildWait);
    }

    private void dmTitleProcTexBuildWait(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!this.dmTitleIsTexLoad(main_work) || !AppMain.DmSndBgmPlayerIsSndSysBuild())
            return;
        for (int index = 0; index < 41; ++index)
        {
            int num1 = 38;
            int num2 = 32;
            AppMain.A2S_AMA_HEADER ama;
            AppMain.AOS_TEXTURE tex;
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
                        ama.act_tbl[(int)AppMain.dm_title_g_dm_act_id_tbl[index]].mtn.trs_tbl[0].trs_y = 90f;
                        break;
                    case 21:
                        ama.act_tbl[(int)AppMain.dm_title_g_dm_act_id_tbl[index]].mtn.trs_tbl[0].trs_y = 200f;
                        break;
                    case 22:
                        ama.act_tbl[(int)AppMain.dm_title_g_dm_act_id_tbl[index]].mtn.trs_tbl[0].trs_y = 310f;
                        break;
                    case 24:
                        ama.act_tbl[(int)AppMain.dm_title_g_dm_act_id_tbl[index]].mtn.trs_tbl[0].trs_y = 420f;
                        break;
                    case 25:
                        ama.act_tbl[(int)AppMain.dm_title_g_dm_act_id_tbl[index]].mtn.trs_tbl[0].trs_y = 530f;
                        break;
                }
            }
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(tex));
            main_work.act[index] = AppMain.AoActCreate(ama, AppMain.dm_title_g_dm_act_id_tbl[index]);
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
        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcCheckLoadingEnd);
        this.DmTitleOpInit();
        AppMain.AoActSysSetDrawState(10U);
        AppMain.AoActSysSetDrawStateEnable(true);
    }

    private void dmTitleProcCheckLoadingEnd(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (AppMain.dm_title_is_title_start)
        {
            main_work.disp_change_time = 40;
            main_work.flag |= 8U;
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFadeIn);
            main_work.proc_draw = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleDrawSetProcDispData);
            main_work.flag |= 512U;
            this.DmTitleOpDispRightEnable(true);
            for (int index = 0; index < 2; ++index)
                main_work.mmenu_win_size_rate[index] = 0.0f;
            AppMain.IzFadeInitEasy(1U, 0U, 32f);
            XBOXLive.allowShowUpdate = true;
        }
        else
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFadeIn);
            this.dmTitleSetMenuInfo(main_work);
            this.DmTitleOpSetRetOptionState();
            main_work.proc_draw = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleDrawSetProcDispData);
            main_work.flag |= 1024U;
            main_work.flag |= 2097152U;
            this.DmTitleOpDispRightEnable(false);
            for (int index = 0; index < 2; ++index)
                main_work.mmenu_win_size_rate[index] = 1f;
        }
    }

    private void dmTitleProcFadeIn(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        if (AppMain.dm_title_is_title_start)
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWaitInput);
            main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcTitle);
            AppMain.DmSndBgmPlayerPlayBgm(1);
        }
        else
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuIdle);
            main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcMainMenu);
            main_work.flag |= 2147483648U;
            AppMain.DmSndBgmPlayerPlayBgm(0);
        }
        main_work.proc_win_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWindowNodispIdle);
    }

    private void dmTitleProcWaitInput(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        ++main_work.timer;
        if ((double)main_work.timer > 3600.0)
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFadeOut);
            main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
            main_work.timer = 0.0f;
            main_work.flag |= 1048576U;
            AppMain.IzFadeInitEasy(0U, 1U, 32f);
            AppMain.DmSndBgmPlayerExit();
            main_work.flag |= 524288U;
        }
        else
        {
            if (main_work.proc_input != null)
                main_work.proc_input(main_work);
            if (((int)main_work.flag & 32) == 0)
                return;
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcDecideEfct);
            main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
            main_work.timer = 0.0f;
            main_work.flag &= 4294967263U;
            AppMain.DmSoundPlaySE("Ok");
            AppMain.DmSndBgmPlayerBgmStop();
        }
    }

    private void dmTitleProcDecideEfct(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if ((double)main_work.timer >= 60.0 && AppMain.AoAccountSetCurrentIdIsFinished())
        {
            if (AppMain.AoAccountGetCurrentId() >= 0)
            {
                main_work.flag &= 4294966783U;
                main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcCheckTrialIdle);
                AppMain.GsRebootSetTitle();
            }
            else
            {
                main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWaitInput);
                main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcTitle);
                AppMain.DmSndBgmPlayerPlayBgm(1);
            }
            main_work.timer = 0.0f;
        }
        else
            ++main_work.timer;
    }

    private void dmTitleProcCheckTrialIdle(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!AppMain.GsTrialCheckIsFinished())
            return;
        if (AppMain.GsTrialIsTrial())
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuOpenWin);
            main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
            main_work.flag |= 2147483648U;
            main_work.flag |= 4096U;
            main_work.flag |= 2097152U;
            main_work.is_init_play = false;
            main_work.slct_menu_num = 3;
            main_work.cur_slct_menu = 0;
            main_work.cur_crsr_pos_y = AppMain.dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
            AppMain.DmSndBgmPlayerPlayBgm(0);
        }
        else
        {
            AppMain.DmSaveStart(1U, false, false);
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFileSlctWaitDataLoad);
            AppMain.DmSndBgmPlayerPlayBgm(0);
        }
    }

    private void dmTitleProcFileSlctWaitDataLoad(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!AppMain.DmSaveIsExit())
            return;
        if (AppMain.DmCmnBackupIsLoadSuccessed())
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuOpenWin);
            main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
            main_work.flag |= 2147483648U;
            main_work.flag |= 4096U;
            main_work.flag |= 2097152U;
            if (AppMain.GsMainSysIsStageClear(0))
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
            main_work.cur_crsr_pos_y = !AppMain.GsTrialIsTrial() ? (float)main_work.cur_slct_menu * AppMain.dm_title_crsr_pos_y_tbl[1] + AppMain.dm_title_crsr_pos_y_tbl[0] : AppMain.dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
            this.dmTitleSetLoadSysData(main_work);
        }
        else
        {
            int error = XmlStorage.GetLastError();
            main_work.is_init_play = true;
            main_work.slct_menu_num = 3;
            if (error == 1)
            {
                main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFileSlctSaveStartWait);
            }
            else
            {
                main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuOpenWin);
                main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
                main_work.flag |= 2147483648U;
                main_work.flag |= 4096U;
                main_work.flag |= 2097152U;
                this.dmTitleSetInitSaveData(main_work);
                this.dmTitleSetInitSysData(main_work);
            }
        }
    }

    private void dmTitleProcFileSlctSaveStartWait(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!AppMain.DmSaveIsExit())
            return;
        AppMain.DmSaveStart(4U, true, false);
        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFileSlctWaitDataSave);
    }

    private void dmTitleProcFileSlctWaitDataSave(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!AppMain.DmSaveIsExit())
            return;
        if (!AppMain.DmCmnBackupIsSaveSuccessed())
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWaitInput);
            main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcTitle);
            main_work.flag |= 512U;
            main_work.flag &= 4294966271U;
            main_work.flag &= (uint)int.MaxValue;
            main_work.timer = 0.0f;
            AppMain.DmSndBgmPlayerPlayBgm(1);
        }
        else
        {
            SSave.CreateInstance().Init();
            main_work.is_init_play = true;
            this.dmTitleSetInitSysData(main_work);
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuOpenWin);
            main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
            main_work.flag |= 2147483648U;
            main_work.flag |= 2097152U;
        }
    }

    private void dmTitleProcMainMenuOpenWin(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 4194304) != 0)
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuIdle);
            main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcMainMenu);
            main_work.mmenu_win_timer = 0.0f;
            main_work.flag |= 1024U;
            main_work.flag &= 4294966783U;
            main_work.flag &= 4290772991U;
            this.DmTitleOpDispRightEnable(false);
            main_work.cur_slct_menu = AppMain.GsTrialIsTrial() || main_work.is_init_play ? 0 : 1;
            main_work.cur_crsr_pos_y = !AppMain.GsTrialIsTrial() ? (float)main_work.cur_slct_menu * AppMain.dm_title_crsr_pos_y_tbl[1] + AppMain.dm_title_crsr_pos_y_tbl[0] : AppMain.dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
            SaveState.showResumeWarning();
        }
        else
            this.dmTitleSetMMenuWinOpenEfct(main_work);
    }

    private void dmTitleProcMainMenuCloseWin(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 4194304) != 0)
        {
            AppMain.AoAccountClearCurrentId();
            SSave.CreateInstance().Init();
            main_work.flag &= (uint)int.MaxValue;
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWaitInput);
            main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcTitle);
            main_work.flag |= 512U;
            main_work.flag &= 4294966271U;
            main_work.flag &= 4292870143U;
            this.DmTitleOpDispRightEnable(true);
            AppMain.DmSndBgmPlayerPlayBgm(1);
            main_work.mmenu_win_timer = 0.0f;
            main_work.flag &= 4292870143U;
            main_work.flag &= 4290772991U;
        }
        else
            this.dmTitleSetMMenuWinCloseEfct(main_work);
    }

    private void dmTitleProcMainMenuIdle(AppMain.DMS_TITLE_MAIN_WORK main_work)
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
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuCloseWin);
            main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
            main_work.mmenu_win_timer = 8f;
            main_work.flag &= 4294966271U;
            main_work.flag &= 4294967293U;
            main_work.flag &= 4294967291U;
            AppMain.DmSoundPlaySE("Cancel");
        }
        else if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuDecideEfct);
            main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
            main_work.timer = 0.0f;
            main_work.flag &= 4294967293U;
            main_work.flag &= 4294967291U;
            AppMain.DmSoundPlaySE("Ok");
        }
        else if (((int)main_work.flag & 2048) != 0)
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuCompBuyIdle);
            main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
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
                AppMain.DmSoundPlaySE("Cursol");
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

    private void dmTitleProcMainMenuCompBuyFadeOut(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (AppMain.IzFadeIsEnd())
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuCompBuyIdle);
            Upsell.launchUpsellScreen(main_work.buy_scr_work);
            main_work.flag &= (uint)int.MaxValue;
            main_work.disp_flag &= 4294967293U;
        }
        if (((int)main_work.flag & 256) == 0)
            return;
        this.dmTitleSetCtrlFocusChangeEfct(main_work);
        if (!this.dmTitleIsCtrlFocusChangeEfctEnd(main_work))
            return;
        main_work.flag &= 4294967039U;
    }

    private void dmTitleProcMainMenuCompBuyIdle(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (Upsell.showUpsell)
            return;
        if (AppMain.DmBuyScreenGetResult(main_work.buy_scr_work) == 0)
        {
            if (!AppMain.GsTrialIsTrial())
            {
                main_work.is_init_play = true;
                main_work.is_no_save_data = true;
                AppMain.event_after_buy = true;
            }
            if (((int)main_work.flag & 262144) != 0)
                main_work.flag &= 4294705151U;
            else
                AppMain.IzFadeInitEasy(1U, 0U, 32f);
            AppMain.DmSndBgmPlayerPlayBgm(0);
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuCompBuyFadeIn);
            main_work.disp_flag |= 2U;
        }
        else
        {
            if (((int)main_work.flag & 262144) != 0)
                main_work.flag &= 4294705151U;
            else
                AppMain.IzFadeInitEasy(1U, 0U, 32f);
            AppMain.DmSndBgmPlayerPlayBgm(0);
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuCompBuyFadeIn);
            main_work.disp_flag |= 2U;
        }
    }

    public static void dmTitleProcMainMenuComUpsellScreenFinished()
    {
        AppMain.IzFadeInitEasy(1U, 0U, 32f);
        AppMain.DmSndBgmPlayerPlayBgm(0);
    }

    private void dmTitleProcMainMenuCompBuyFadeIn(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        if (AppMain.DmBuyScreenGetResult(main_work.buy_scr_work) == 0)
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuIdle);
            main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcMainMenu);
            main_work.flag |= 2147483648U;
            if (!AppMain.GsTrialIsTrial())
            {
                main_work.is_init_play = true;
                main_work.is_no_save_data = true;
                AppMain.event_after_buy = true;
            }
        }
        else
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuIdle);
            main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcMainMenu);
            main_work.flag |= 2147483648U;
        }
        for (int index = 0; index < 5; ++index)
            main_work.decide_menu_frm[index] = 0.0f;
    }

    private void dmTitleProcMainMenuTrophyIdle(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
    }

    private void dmTitleProcMainMenuDecideEfct(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 256) != 0)
        {
            this.dmTitleSetCtrlFocusChangeEfct(main_work);
            if (this.dmTitleIsCtrlFocusChangeEfctEnd(main_work))
                main_work.flag &= 4294967039U;
        }
        int curSlctMenu = main_work.cur_slct_menu;
        if ((double)main_work.timer > 15.0)
        {
            if (curSlctMenu == 4 && (AppMain.GsTrialIsTrial() || XBOXLive.signinStatus == XBOXLive.SigninStatus.UpdateNeeded || XBOXLive.signinStatus == XBOXLive.SigninStatus.Local))
            {
                for (int index = 0; index < 5; ++index)
                    main_work.decide_menu_frm[index] = 0.0f;
                main_work.flag &= 4294967291U;
                main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuIdle);
                main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcMainMenu);
                if (XBOXLive.signinStatus == XBOXLive.SigninStatus.UpdateNeeded)
                {
                    XBOXLive.displayTitleUpdateMessage = true;
                }
                else
                {
                    List<string> stringList = new List<string>();
                    stringList.Add(Sonic4ep1.Strings.ID_OK);
                    string text = !AppMain.GsTrialIsTrial() ? (XBOXLive.signinStatus != XBOXLive.SigninStatus.Local ? Sonic4ep1.Strings.ID_LB_UPDATE : Sonic4ep1.Strings.ID_LB_OFFLINE) : Sonic4ep1.Strings.ID_LB_BUY;
                    //AppMain.g_ao_sys_global.is_show_ui = true;
                    //Guide.BeginShowMessageBox(" ", text, (IEnumerable<string>)stringList, 0, MessageBoxIcon.Alert, new AsyncCallback(AppMain.TitleMBResult), (object)null);
                }
            }
            else
            {
                if (curSlctMenu == 0 && SaveState.shouldResume())
                {
                    main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFadeOut);
                }
                else
                {
                    if (curSlctMenu == 0 && !main_work.is_init_play && !AppMain.GsTrialIsTrial())
                    {
                        if (this.dmTitleIsSaveRunning())
                        {
                            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuDelSaveWin);
                            main_work.announce_flag |= 1U;
                            main_work.flag &= 4292869119U;
                            return;
                        }
                        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFadeOut);
                        return;
                    }
                    if (AppMain.GsTrialIsTrial() && curSlctMenu == 1)
                    {
                        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuCompBuyFadeOut);
                        main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
                        main_work.timer = 0.0f;
                        AppMain.IzFadeInitEasy(0U, 1U, 32f);
                        AppMain.DmSndBgmPlayerBgmStop();
                        return;
                    }
                    main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFadeOut);
                }
                main_work.timer = 0.0f;
                for (int index = 0; index < 5; ++index)
                    main_work.decide_menu_frm[index] = 0.0f;
                AppMain.IzFadeInitEasy(0U, 1U, 32f);
                if (curSlctMenu != 0)
                    return;
                AppMain.DmSndBgmPlayerExit();
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
        AppMain.g_ao_sys_global.is_show_ui = false;
    }

    private void dmTitleProcMainMenuDelSaveWin(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 8192) != 0)
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuIdle);
            main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcMainMenu);
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
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcSaveInitData);
            AppMain.DmSaveMenuStart(false, false);
            main_work.flag &= 4294950911U;
        }
        if (((int)main_work.flag & 256) == 0)
            return;
        this.dmTitleSetCtrlFocusChangeEfct(main_work);
        if (!this.dmTitleIsCtrlFocusChangeEfctEnd(main_work))
            return;
        main_work.flag &= 4294967039U;
    }

    private void dmTitleProcSaveInitData(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!AppMain.DmSaveIsExit())
            return;
        if (!AppMain.GsTrialIsTrial() && !AppMain.DmCmnBackupIsSaveSuccessed())
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWaitInput);
            main_work.proc_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcTitle);
            main_work.flag |= 512U;
            main_work.flag &= 4294966271U;
            main_work.flag &= (uint)int.MaxValue;
            AppMain.DmSndBgmPlayerPlayBgm(1);
        }
        else
        {
            main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFadeOut);
            main_work.timer = 0.0f;
            AppMain.IzFadeInitEasy(0U, 1U, 32f);
            AppMain.DmSndBgmPlayerExit();
            main_work.flag |= 524288U;
        }
    }

    private void dmTitleProcFadeOut(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        this.DmTitleOpExit();
        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcDataRelease);
        main_work.proc_draw = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
    }

    private void dmTitleProcWindowNodispIdle(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (main_work.proc_win_input != null)
            main_work.proc_win_input(main_work);
        if (main_work.announce_flag == 0U)
            return;
        main_work.proc_win_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWindowOpenEfct);
        main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
        main_work.proc_win_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
        main_work.win_timer = 0.0f;
        for (int index = 0; index < 5; ++index)
        {
            if (((long)main_work.announce_flag & (long)(1 << index)) != 0L)
            {
                main_work.win_mode = index;
                break;
            }
        }
        main_work.win_cur_slct = 1;
        AppMain.DmSoundPlaySE("Window");
        main_work.flag |= 131072U;
    }

    private void dmTitleProcWindowOpenEfct(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 65536) != 0)
        {
            main_work.proc_win_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWindowAnnounceIdle);
            main_work.proc_win_input = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleInputProcWindow);
            main_work.disp_flag |= 1U;
            main_work.flag &= 4294901759U;
        }
        else
            this.dmTitleSetWinOpenEfct(main_work);
    }

    private void dmTitleProcWindowAnnounceIdle(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (main_work.proc_win_input != null)
            main_work.proc_win_input(main_work);
        if (main_work.win_mode == 0)
        {
            if (((int)main_work.flag & 4) != 0 && main_work.win_cur_slct == 0)
            {
                main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
                main_work.proc_win_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
                main_work.win_timer = 8f;
                main_work.disp_flag &= 4294967294U;
                main_work.proc_win_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWindowCloseEfct);
                AppMain.DmSoundPlaySE("Ok");
                main_work.flag &= 4294967291U;
                main_work.flag &= 4294967293U;
            }
            else
            {
                if (((int)main_work.flag & 2) == 0 && (main_work.win_cur_slct != 1 || ((int)main_work.flag & 4) == 0))
                    return;
                main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
                main_work.proc_win_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
                main_work.win_timer = 8f;
                main_work.disp_flag &= 4294967294U;
                main_work.proc_win_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWindowCloseEfct);
                AppMain.DmSoundPlaySE("Cancel");
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
                main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
                main_work.proc_win_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
                main_work.win_timer = 8f;
                main_work.disp_flag &= 4294967294U;
                main_work.proc_win_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWindowCloseEfct);
                AppMain.DmSoundPlaySE("Ok");
                main_work.flag &= 4294967291U;
                main_work.flag &= 4294967293U;
            }
            else
            {
                if (((int)main_work.flag & 2) == 0 && (main_work.win_cur_slct != 1 || ((int)main_work.flag & 4) == 0))
                    return;
                main_work.proc_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
                main_work.proc_win_input = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
                main_work.win_timer = 8f;
                main_work.disp_flag &= 4294967294U;
                main_work.proc_win_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWindowCloseEfct);
                AppMain.DmSoundPlaySE("Cancel");
                main_work.flag |= 2U;
                main_work.flag &= 4294967291U;
            }
        }
    }

    private void dmTitleProcWindowCloseEfct(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 65536) != 0)
        {
            main_work.proc_win_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWindowNodispIdle);
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

    private void dmTitleProcDataRelease(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!this.DmTitleOpExitEndCheck())
            return;
        for (int index = 0; index < 2; ++index)
            AppMain.AoTexRelease(main_work.tex[index]);
        for (int index = 0; index < 4; ++index)
            AppMain.AoTexRelease(main_work.cmn_tex[index]);
        this.DmTitleOpFlush();
        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcFinish);
    }

    private void dmTitleProcFinish(AppMain.DMS_TITLE_MAIN_WORK main_work)
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
                AppMain.AoActDelete(main_work.act[index]);
                main_work.act[index] = (AppMain.AOS_ACTION)null;
            }
        }
        for (int index = 0; index < 2; ++index)
        {
            if (main_work.arc_amb[index] != null)
                main_work.arc_amb[index] = (AppMain.AMS_AMB_HEADER)null;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (main_work.arc_cmn_amb[index] != null)
                main_work.arc_cmn_amb[index] = (AppMain.AMS_AMB_HEADER)null;
        }
        this.DmTitleOpRelease();
        main_work.proc_update = new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcWaitFinished);
    }

    private void dmTitleProcWaitFinished(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (!this.DmTitleOpReleaseCheck())
            return;
        if (((int)main_work.flag & 524288) != 0)
        {
            if (!AppMain.DmSndBgmPlayerIsTaskExit())
                return;
            main_work.flag &= 4294443007U;
        }
        main_work.flag |= 1U;
        main_work.proc_update = (AppMain.DMS_TITLE_MAIN_WORK._proc_)null;
    }

    private void dmTitleInputProcTitle(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (AppMain.amTpIsTouchPush(0))
        {
            AppMain.AoAccountSetCurrentIdStart(0U);
            main_work.flag |= 32U;
        }
        if (!AppMain.isBackKeyPressed())
            return;
        AppMain.finish();
        AppMain.setBackKeyRequest(false);
    }

    private void dmTitleInputProcMainMenu(AppMain.DMS_TITLE_MAIN_WORK main_work)
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
        }
        if ((4 & (int)main_work.flag) != 0)
            return;
        CTrgAoAction trgReturn = main_work.trg_return;
        if (trgReturn.GetState(0U)[10] && trgReturn.GetState(0U)[1] || AppMain.isBackKeyPressed())
        {
            AppMain.setBackKeyRequest(false);
            if (LiveFeature.isInterrupted())
                LiveFeature.endInterrupt();
            else
                main_work.flag |= 2U;
        }
        CTrgAoAction trgGame = main_work.trg_game;
        if (!trgGame.GetState(0U)[10] || !trgGame.GetState(0U)[1])
            return;
        string url = AppMain.GsEnvGetLanguage() != 0 ? "http://sega.com/apps" : "http://sega.jp/kt/microsoft/smart/";
        AppMain.DmSoundPlaySE("Ok");
        erWeb.StartWeb(url);
    }

    private void dmTitleInputProcWindow(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        CTrgAoAction[] trgAnswer = main_work.trg_answer;
        if (trgAnswer[0].GetState(0U)[10] && trgAnswer[0].GetState(0U)[1] || AppMain.isBackKeyPressed())
        {
            AppMain.setBackKeyRequest(false);
            main_work.win_cur_slct = 1;
            main_work.flag |= 4U;
        }
        if (!trgAnswer[1].GetState(0U)[10] || !trgAnswer[1].GetState(0U)[1])
            return;
        main_work.win_cur_slct = 0;
        main_work.flag |= 4U;
    }

    private void dmTitleDrawSetProcDispData(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        float num = 0.0f;
        if (((int)main_work.disp_flag & 2) != 0)
        {
            AppMain.AoActSysSetDrawTaskPrio(32768U);
            this.DmTitleOpDraw2D();
            AppMain.AoActSortExecute();
            AppMain.AoActSortDraw();
            AppMain.AoActSortUnregAll();
            if (((int)main_work.flag & 512) != 0 && this.DmTitleOpIsLogoActFinish())
                this.dmTitleDrawProcTitle(main_work);
            if (((int)main_work.flag & 2097152) != 0)
            {
                AppMain.AoActSysSetDrawTaskPrio(35840U);
                if ((double)main_work.mmenu_win_size_rate[0] >= 0.9 || AppMain.AoAccountIsCurrentEnable())
                {
                    int[] numArray = new int[2] { 800, 540 };
                    AppMain.GsTrialIsTrial();
                    bool flag = !main_work.is_init_play;
                    AppMain.AoWinSysDrawState(0, AppMain.AoTexGetTexList(main_work.cmn_tex[2]), 0U, 480f, 311.9f, (float)numArray[0] * main_work.mmenu_win_size_rate[0], (float)((double)numArray[1] * (double)main_work.mmenu_win_size_rate[1] * 0.899999976158142) + num, 10U);
                }
            }
            if (((int)main_work.flag & 1024) != 0)
            {
                if (((int)main_work.flag_prev & 1024) == 0)
                {
                    main_work.trg_return.ResetState();
                    int index1 = 15;
                    for (int index2 = 17; index1 < index2; ++index1)
                        AppMain.AoActSetFrame(main_work.act[index1], 0.0f);
                    main_work.trg_game.ResetState();
                    int index3 = 17;
                    for (int index2 = 19; index3 < index2; ++index3)
                        AppMain.AoActSetFrame(main_work.act[index3], 0.0f);
                }
                this.dmTitleDrawProcMainMenu(main_work);
            }
            if (((int)main_work.flag & 131072) != 0)
                this.dmTitleWinSelectDraw(main_work);
        }
        AppMain.amDrawMakeTask(new AppMain.TaskProc(this.dmTitleTaskDraw), (ushort)32768, 0U);
    }

    private void dmTitleDrawProcTitle(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(33792U);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
        if (((int)main_work.flag & 8) != 0)
            AppMain.AoActSortRegAction(main_work.act[19]);
        if (((int)main_work.flag & 16) != 0)
        {
            if ((double)main_work.disp_timer >= 4.0)
            {
                main_work.disp_timer = 0.0f;
                main_work.flag ^= 8U;
            }
            else
                ++main_work.disp_timer;
        }
        else if ((double)main_work.disp_timer >= (double)main_work.disp_change_time)
        {
            main_work.flag ^= 8U;
            main_work.disp_change_time = ((int)main_work.flag & 8) == 0 ? 30 : 40;
            main_work.disp_timer = 0.0f;
        }
        else
            ++main_work.disp_timer;
        if (AppMain.GeEnvGetDecideKey() == AppMain.GSE_DECIDE_KEY.GSD_DECIDE_KEY_O)
            AppMain.AoActSetFrame(main_work.act[19], 0.0f);
        else
            AppMain.AoActSetFrame(main_work.act[19], 1f);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
        AppMain.AoActUpdate(main_work.act[19], 0.0f);
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private void dmTitleDrawProcMainMenu(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        bool is_trial = AppMain.GsTrialIsTrial();
        bool has_save_data = !main_work.is_init_play;
        this.dmTitleDrawProcMainMenuIphone(main_work, has_save_data, is_trial);
    }

    private void CLocalBtnBase_Update(
      AppMain.AOS_ACTION src,
      AppMain.ArrayPointer<AppMain.AOS_ACTION> btn,
      CTrgAoAction trg)
    {
        this.CLocalBtnBase_Update(src, btn, trg, 0.0f);
    }

    private void CLocalBtnBase_Update(
      AppMain.AOS_ACTION src,
      AppMain.ArrayPointer<AppMain.AOS_ACTION> btn,
      CTrgAoAction trg,
      float frame)
    {
        AppMain.AoActAcmPush();
        AppMain.AoActAcmInit();
        int[] numArray = new int[2]
        {
      (int) src.sprite.center_x,
      (int) src.sprite.center_y
        };
        AppMain.AoActAcmApplyTrans((float)numArray[0], (float)numArray[1], 1f);
        AppMain.AoActUpdate(btn[0], frame);
        AppMain.AoActUpdate(btn[1], frame);
        AppMain.AoActUpdate(btn[2], frame);
        trg.Update();
        AppMain.AoActAcmPop();
    }

    private void CLocalBtnBase_Draw(AppMain.ArrayPointer<AppMain.AOS_ACTION> btn)
    {
        AppMain.AoActSortRegAction(btn[0]);
        AppMain.AoActSortRegAction(btn[1]);
        AppMain.AoActSortRegAction(btn[2]);
    }

    private void CLocalBtnBase_SetFrame(AppMain.ArrayPointer<AppMain.AOS_ACTION> btn, float frame)
    {
        AppMain.AoActSetFrame(btn[0], frame);
        AppMain.AoActSetFrame(btn[1], frame);
        AppMain.AoActSetFrame(btn[2], frame);
    }

    private void dmTitleDrawProcMainMenuIphone(
      AppMain.DMS_TITLE_MAIN_WORK main_work,
      bool has_save_data,
      bool is_trial)
    {
        AppMain.AoActSysSetDrawTaskPrio(33792U);
        int[] numArray1 = new int[5] { 20, 21, 22, 24, 25 };
        bool flag = has_save_data;
        int[] numArray2 = new int[5] { 0, 3, 6, 9, 12 };
        float[] numArray3 = new float[5];
        if (is_trial)
        {
            numArray1[1] = 23;
            flag = true;
        }
        if (new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcMainMenuDecideEfct) == main_work.proc_update)
            numArray3[main_work.cur_slct_menu] = 1f;
        else if ((!AppMain.IzFadeIsExe() || AppMain.IzFadeIsEnd()) && (!(new AppMain.DMS_TITLE_MAIN_WORK._proc_(this.dmTitleProcSaveInitData) == main_work.proc_update) && (131072 & (int)main_work.flag) == 0))
        {
            for (int index = 0; index < 5; ++index)
            {
                float frame = !main_work.trg_slct[index].GetState(0U)[10] || !main_work.trg_slct[index].GetState(0U)[1] ? (!main_work.trg_slct[index].GetState(0U)[0] ? 0.0f : 1f) : 2f;
                AppMain.AoActSetFrame(main_work.act[numArray1[index]], frame);
                this.CLocalBtnBase_SetFrame(new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[index]), frame);
            }
            float frame1 = !main_work.trg_return.GetState(0U)[10] || !main_work.trg_return.GetState(0U)[1] ? (!main_work.trg_return.GetState(0U)[0] ? (2.0 > (double)main_work.act[15].frame ? 0.0f : main_work.act[15].frame) : 1f) : 2f;
            AppMain.AoActSetFrame(main_work.act[26], frame1);
            int index1 = 15;
            for (int index2 = 17; index1 < index2; ++index1)
                AppMain.AoActSetFrame(main_work.act[index1], frame1);
            float frame2 = !main_work.trg_game.GetState(0U)[10] || !main_work.trg_game.GetState(0U)[1] ? (!main_work.trg_game.GetState(0U)[0] ? (2.0 > (double)main_work.act[17].frame ? 0.0f : main_work.act[17].frame) : 1f) : 2f;
            AppMain.AoActSetFrame(main_work.act[27], frame2);
            int index3 = 17;
            for (int index2 = 19; index3 < index2; ++index3)
                AppMain.AoActSetFrame(main_work.act[index3], frame2);
        }
        if (!LiveFeature.isInterrupted())
        {
            if (!flag)
            {
                main_work.ama[1].act_tbl[(int)AppMain.dm_title_g_dm_act_id_tbl[20]].mtn.trs_tbl[0].trs_y = 140f;
                main_work.ama[1].act_tbl[(int)AppMain.dm_title_g_dm_act_id_tbl[22]].mtn.trs_tbl[0].trs_y = 250f;
                main_work.ama[1].act_tbl[(int)AppMain.dm_title_g_dm_act_id_tbl[24]].mtn.trs_tbl[0].trs_y = 360f;
                main_work.ama[1].act_tbl[(int)AppMain.dm_title_g_dm_act_id_tbl[25]].mtn.trs_tbl[0].trs_y = 470f;
            }
            else
                main_work.ama[1].act_tbl[(int)AppMain.dm_title_g_dm_act_id_tbl[21]].mtn.trs_tbl[0].trs_y = 200f;
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
            AppMain.AoActUpdate(main_work.act[numArray1[0]], numArray3[0]);
            if (flag)
            {
                AppMain.AoActUpdate(main_work.act[numArray1[1]], numArray3[1]);
                main_work.act[numArray1[1]].sprite.center_y = 200f;
            }
            AppMain.AoActUpdate(main_work.act[numArray1[2]], numArray3[2]);
            AppMain.AoActUpdate(main_work.act[numArray1[3]], numArray3[3]);
            AppMain.AoActUpdate(main_work.act[numArray1[4]], numArray3[4]);
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
            this.CLocalBtnBase_Update(main_work.act[numArray1[0]], new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[0]), main_work.trg_slct[0], numArray3[0]);
            if (flag)
                this.CLocalBtnBase_Update(main_work.act[numArray1[1]], new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[1]), main_work.trg_slct[1], numArray3[1]);
            this.CLocalBtnBase_Update(main_work.act[numArray1[2]], new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[2]), main_work.trg_slct[2], numArray3[2]);
            this.CLocalBtnBase_Update(main_work.act[numArray1[3]], new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[3]), main_work.trg_slct[3], numArray3[3]);
            this.CLocalBtnBase_Update(main_work.act[numArray1[4]], new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[4]), main_work.trg_slct[4], numArray3[4]);
        }
        float frame3 = 2.0 <= (double)main_work.act[15].frame ? 1f : 0.0f;
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
        AppMain.AoActUpdate(main_work.act[26], frame3);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
        int index4 = 15;
        for (int index1 = 17; index4 < index1; ++index4)
            AppMain.AoActUpdate(main_work.act[index4], frame3);
        main_work.trg_return.Update();
        float frame4 = 2.0 <= (double)main_work.act[17].frame ? 1f : 0.0f;
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
        AppMain.AoActUpdate(main_work.act[27], frame4);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
        int index5 = 17;
        for (int index1 = 19; index5 < index1; ++index5)
            AppMain.AoActUpdate(main_work.act[index5], frame4);
        main_work.trg_game.Update();
        if (!LiveFeature.isInterrupted())
        {
            this.CLocalBtnBase_Draw(new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[0]));
            AppMain.AoActSortRegAction(main_work.act[numArray1[0]]);
            main_work.act[numArray1[0]].sprite.center_y -= 3f;
            if (flag)
            {
                this.CLocalBtnBase_Draw(new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[1]));
                AppMain.AoActSortRegAction(main_work.act[numArray1[1]]);
                main_work.act[numArray1[1]].sprite.center_y -= 3f;
            }
            this.CLocalBtnBase_Draw(new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[2]));
            AppMain.AoActSortRegAction(main_work.act[numArray1[2]]);
            main_work.act[numArray1[2]].sprite.center_y -= 3f;
            this.CLocalBtnBase_Draw(new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[3]));
            AppMain.AoActSortRegAction(main_work.act[numArray1[3]]);
            main_work.act[numArray1[3]].sprite.center_y -= 3f;
            this.CLocalBtnBase_Draw(new AppMain.ArrayPointer<AppMain.AOS_ACTION>(main_work.act, numArray2[4]));
            AppMain.AoActSortRegAction(main_work.act[numArray1[4]]);
            main_work.act[numArray1[4]].sprite.center_y -= 3f;
        }
        if ((2097152 & (int)main_work.flag) != 0 && (131072 & (int)main_work.flag) == 0)
        {
            int index1 = 15;
            for (int index2 = 17; index1 < index2; ++index1)
                AppMain.AoActSortRegAction(main_work.act[index1]);
            AppMain.AoActSortRegAction(main_work.act[26]);
            int index3 = 17;
            for (int index2 = 19; index3 < index2; ++index3)
                AppMain.AoActSortRegAction(main_work.act[index3]);
            AppMain.AoActSortRegAction(main_work.act[27]);
        }
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private void dmTitleWinSelectDraw(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(35840U);
        AppMain.AoWinSysDrawState(0, AppMain.AoTexGetTexList(main_work.cmn_tex[2]), 0U, 480f, 356f, 708.75f * main_work.win_size_rate[0], (float)(303.75 * (double)main_work.win_size_rate[1] * 0.899999976158142), 10U);
        if (((int)main_work.disp_flag & 1) == 0)
            return;
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[2]));
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[3]));
        switch (main_work.win_mode)
        {
            case 0:
                int index1 = 32;
                for (int index2 = 38; index1 < index2; ++index1)
                    AppMain.AoActSortRegAction(main_work.act[index1]);
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
                AppMain.AoActSortRegAction(main_work.act[28]);
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[3]));
                AppMain.AoActSortRegAction(main_work.act[38]);
                AppMain.AoActSortRegAction(main_work.act[39]);
                break;
            case 1:
                int index3 = 32;
                for (int index2 = 38; index3 < index2; ++index3)
                    AppMain.AoActSortRegAction(main_work.act[index3]);
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
                AppMain.AoActSortRegAction(main_work.act[29]);
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[3]));
                AppMain.AoActSortRegAction(main_work.act[38]);
                AppMain.AoActSortRegAction(main_work.act[39]);
                break;
        }
        AppMain.AoActAcmPush();
        for (int index2 = 0; index2 < 3; ++index2)
        {
            AppMain.AoActAcmInit();
            AppMain.AoActAcmApplyTrans(AppMain.dm_title_win_act_pos_tbl[index2 + 1, 0], AppMain.dm_title_win_act_pos_tbl[index2 + 1, 1], 0.0f);
            if (index2 <= 1)
                AppMain.AoActAcmApplyScale(27f / 16f, 27f / 16f);
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
            AppMain.AoActUpdate(main_work.act[28 + index2], 0.0f);
        }
        for (int index2 = 1; index2 < 3; ++index2)
        {
            int num = 37;
            AppMain.AoActAcmInit();
            AppMain.AoActAcmApplyTrans(AppMain.dm_title_win_act_pos_tbl[index2 + 4, 0], AppMain.dm_title_win_act_pos_tbl[index2 + 4, 1], 0.0f);
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[3]));
            AppMain.AoActUpdate(main_work.act[num + index2], 0.0f);
            main_work.act[num + index2].sprite.center_y += 5f;
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[2]));
            int[,] numArray1 = new int[3, 2]
            {
        {
          0,
          -1
        },
        {
          35,
          37
        },
        {
          32,
          34
        }
            };
            AppMain.AoActUpdate(main_work.act[numArray1[index2, 0] + 1], 0.0f);
            int[] numArray2 = new int[3] { -1, 1, 0 };
            CTrgAoAction ctrgAoAction = main_work.trg_answer[numArray2[index2]];
            if (0 <= numArray2[index2])
                ctrgAoAction.Update();
            float frame = ctrgAoAction.GetState(0U)[0] ? 1f : 0.0f;
            int index4 = numArray1[index2, 0];
            for (int index5 = numArray1[index2, 1] + 1; index4 < index5; ++index4)
            {
                AppMain.AoActSetFrame(main_work.act[index4], frame);
                AppMain.AoActUpdate(main_work.act[index4], 0.0f);
            }
        }
        AppMain.AoActAcmPop();
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private void dmTitleTaskDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(10U);
        AppMain.amDrawEndScene();
    }

    private bool dmTitleIsDataLoad(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.amFsIsComplete(main_work.arc_amb_fs[index]))
                return false;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (!AppMain.amFsIsComplete(main_work.arc_cmn_amb_fs[index]))
                return false;
        }
        return this.DmTitleOpLoadCheck();
    }

    private bool dmTitleIsTexLoad(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.AoTexIsLoaded(main_work.tex[index]))
                return false;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (!AppMain.AoTexIsLoaded(main_work.cmn_tex[index]))
                return false;
        }
        return AppMain.GsFontIsBuilded() && this.DmTitleOpBuildCheck();
    }

    private bool dmTitleIsTexRelease(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.AoTexIsReleased(main_work.tex[index]))
                return false;
        }
        for (int index = 0; index < 4; ++index)
        {
            if (!AppMain.AoTexIsReleased(main_work.cmn_tex[index]))
                return false;
        }
        return this.DmTitleOpFlushCheck();
    }

    private void dmTitleSetChngFocusCrsrData(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 64) != 0)
        {
            main_work.prev_slct_menu = main_work.cur_slct_menu;
            main_work.cur_slct_menu = this.dmTitleGetRevisedMenuFocus(main_work.cur_slct_menu, -1, main_work.slct_menu_num);
            main_work.src_crsr_pos_y = main_work.cur_crsr_pos_y;
            main_work.dst_crsr_pos_y = !AppMain.GsTrialIsTrial() ? AppMain.dm_title_crsr_pos_y_tbl[0] + (float)main_work.cur_slct_menu * AppMain.dm_title_crsr_pos_y_tbl[1] : AppMain.dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
            main_work.flag |= 256U;
        }
        if (((int)main_work.flag & 128) == 0)
            return;
        main_work.prev_slct_menu = main_work.cur_slct_menu;
        main_work.cur_slct_menu = this.dmTitleGetRevisedMenuFocus(main_work.cur_slct_menu, 1, main_work.slct_menu_num);
        main_work.src_crsr_pos_y = main_work.cur_crsr_pos_y;
        main_work.dst_crsr_pos_y = !AppMain.GsTrialIsTrial() ? AppMain.dm_title_crsr_pos_y_tbl[0] + (float)main_work.cur_slct_menu * AppMain.dm_title_crsr_pos_y_tbl[1] : AppMain.dm_title_crsr_trial_pos_y_tbl[main_work.cur_slct_menu];
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

    private void dmTitleSetCtrlFocusChangeEfct(AppMain.DMS_TITLE_MAIN_WORK main_work)
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

    private void dmTitleSetWinOpenEfct(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if ((double)main_work.win_timer > 8.0)
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
            main_work.win_size_rate[(int)index] = (double)main_work.win_timer == 0.0 ? 1f : main_work.win_timer / 8f;
            if ((double)main_work.win_size_rate[(int)index] > 1.0)
                main_work.win_size_rate[(int)index] = 1f;
        }
    }

    private void dmTitleSetWinCloseEfct(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
            main_work.win_size_rate[(int)index] = (double)main_work.win_timer == 0.0 ? 0.0f : main_work.win_timer / 8f;
        if ((double)main_work.win_timer < 0.0)
        {
            main_work.flag |= 65536U;
            main_work.win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int)index] = 0.0f;
        }
        else
            --main_work.win_timer;
    }

    private void dmTitleSetMMenuWinOpenEfct(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (0.0 == (double)main_work.mmenu_win_timer)
            AppMain.DmSoundPlaySE("Window");
        if ((double)main_work.mmenu_win_timer > 8.0)
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
            main_work.mmenu_win_size_rate[(int)index] = (double)main_work.mmenu_win_timer == 0.0 ? 1f : main_work.mmenu_win_timer / 8f;
            if ((double)main_work.mmenu_win_size_rate[(int)index] > 1.0)
                main_work.mmenu_win_size_rate[(int)index] = 1f;
        }
    }

    private void dmTitleSetMMenuWinCloseEfct(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
            main_work.mmenu_win_size_rate[(int)index] = (double)main_work.mmenu_win_timer == 0.0 ? 0.0f : main_work.mmenu_win_timer / 8f;
        if ((double)main_work.mmenu_win_timer < 0.0)
        {
            main_work.flag |= 4194304U;
            main_work.mmenu_win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.mmenu_win_size_rate[(int)index] = 0.0f;
        }
        else
            --main_work.mmenu_win_timer;
    }

    private bool dmTitleIsCtrlFocusChangeEfctEnd(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        float num = main_work.dst_crsr_pos_y - main_work.src_crsr_pos_y;
        if ((double)main_work.cur_crsr_pos_y >= (double)main_work.dst_crsr_pos_y && (double)num >= 0.0)
        {
            main_work.cur_crsr_pos_y = main_work.dst_crsr_pos_y;
            main_work.timer = 0.0f;
            return true;
        }
        if ((double)main_work.cur_crsr_pos_y <= (double)main_work.dst_crsr_pos_y && (double)num <= 0.0)
        {
            main_work.cur_crsr_pos_y = main_work.dst_crsr_pos_y;
            main_work.timer = 0.0f;
            return true;
        }
        ++main_work.timer;
        return false;
    }

    private void dmTitleSetLoadSysData(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        uint num = 0;
        SOption instance1 = SOption.CreateInstance();
        int volumeBgm = (int)instance1.GetVolumeBgm();
        int volumeSe = (int)instance1.GetVolumeSe();
        mainSysInfo.bgm_volume = (float)(volumeBgm / 10);
        mainSysInfo.se_volume = (float)(volumeSe / 10);
        AppMain.DmSoundSetVolumeBGM(mainSysInfo.bgm_volume);
        AppMain.DmSoundSetVolumeSE(mainSysInfo.se_volume);
        mainSysInfo.game_flag |= 64U;
        SSpecial instance2 = SSpecial.CreateInstance();
        for (int index = 0; index < 7; ++index)
        {
            if (instance2[(int)(ushort)index].IsGetEmerald())
                num |= (uint)(1 << index);
        }
        if (num == (uint)sbyte.MaxValue)
            mainSysInfo.game_flag |= 32U;
        else
            mainSysInfo.game_flag &= 4294967263U;
        SSystem instance3 = SSystem.CreateInstance();
        mainSysInfo.rest_player_num = instance3.GetPlayerStock();
        mainSysInfo.ene_kill_count = instance3.GetKilled();
        mainSysInfo.final_clear_count = instance3.GetClearCount();
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

    private void dmTitleSetInitSysData(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        SOption instance = SOption.CreateInstance();
        int volumeBgm = (int)instance.GetVolumeBgm();
        int volumeSe = (int)instance.GetVolumeSe();
        mainSysInfo.bgm_volume = (float)volumeBgm / 10f;
        mainSysInfo.se_volume = (float)volumeSe / 10f;
        AppMain.DmSoundSetVolumeBGM(mainSysInfo.bgm_volume);
        AppMain.DmSoundSetVolumeSE(mainSysInfo.se_volume);
        instance.IsVibration();
        mainSysInfo.game_flag |= 64U;
        mainSysInfo.game_flag &= 4294967263U;
        mainSysInfo.rest_player_num = 3U;
        mainSysInfo.is_spe_clear = false;
        mainSysInfo.is_first_play = false;
    }

    private void dmTitleSetMenuInfo(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        if (AppMain.GsTrialIsTrial())
        {
            main_work.slct_menu_num = 3;
            main_work.is_init_play = true;
        }
        else if (!AppMain.GsMainSysIsStageClear(0))
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

    private void dmTitleSetFirstPlayData(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        SSystem instance = SSystem.CreateInstance();
        mainSysInfo.rest_player_num = 3U;
        instance.SetPlayerStock(mainSysInfo.rest_player_num);
        this.dmTitleSetLoadSysData(main_work);
    }

    private void dmTitleSetInitSaveData(AppMain.DMS_TITLE_MAIN_WORK main_work)
    {
        AppMain.GsGetMainSysInfo().Save.Init();
    }

    private bool dmTitleIsSaveRunning()
    {
        return AppMain.GsGetMainSysInfo().is_save_run != 0U;
    }

    private bool dmTitleIsChangeOptVol()
    {
        SOption instance = SOption.CreateInstance();
        return instance.GetVolumeBgm() != 100U || instance.GetVolumeSe() != 100U;
    }

}