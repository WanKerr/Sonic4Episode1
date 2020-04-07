using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using er;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static void DmOptionStart(object arg)
    {
        AppMain.dm_opt_mgr.Clear();
        AppMain.dm_opt_mgr_p = AppMain.dm_opt_mgr;
        AppMain.dm_opt_win_tex.Clear();
        switch (AppMain.SyGetEvtInfo().cur_evt_id)
        {
            case 6:
            case 11:
                AppMain.dm_opt_is_pause_maingame = true;
                AppMain.mtTaskStartPause((ushort)2);
                break;
            default:
                AppMain.dm_opt_is_pause_maingame = false;
                break;
        }
        AppMain.dm_xbox_show_progress = 0;
        AppMain.dmOptInit();
    }

    private static bool DmOptionIsExit()
    {
        return AppMain.dm_opt_mgr_p == null || AppMain.dm_opt_mgr_p.tcb == null;
    }

    private static void dmOptInit()
    {
        AppMain.dm_opt_mgr_p.tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.dmOptProcMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.dmOptDest), 0U, (ushort)short.MaxValue, 8192U, 10, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_OPT_MAIN_WORK()), "OPT_MAIN");
        AppMain.DMS_OPT_MAIN_WORK work = (AppMain.DMS_OPT_MAIN_WORK)AppMain.dm_opt_mgr_p.tcb.work;
        if (AppMain.dm_opt_is_pause_maingame)
        {
            work.draw_state = AppMain.AoActSysGetDrawStateEnable() ? 1U : 0U;
            if (work.draw_state > 0U)
                AppMain.dm_opt_draw_state = AppMain.AoActSysGetDrawState();
        }
        else
        {
            work.draw_state = 1U;
            AppMain.dm_opt_draw_state = 0U;
        }
        AppMain.AoActSysSetDrawStateEnable(work.draw_state > 0U);
        if (work.draw_state > 0U)
            AppMain.dm_opt_draw_state = AppMain.AoActSysGetDrawState();
        AppMain.dmOptSetInitDispOptionData(work);
        work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptLoadFontData);
    }

    private static void dmOptSetSaveOptionData(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        SOption instance = SOption.CreateInstance();
        int volumeBgm = (int)instance.GetVolumeBgm();
        int volumeSe = (int)instance.GetVolumeSe();
        main_work.volume_data[0] = volumeBgm == 0 ? 0 : volumeBgm / 10;
        main_work.volume_data[1] = volumeSe == 0 ? 0 : volumeSe / 10;
        instance.SetVolumeBgm((uint)(main_work.volume_data[0] * 10));
        instance.SetVolumeSe((uint)(main_work.volume_data[1] * 10));
    }

    private static void dmOptSetInitDispOptionData(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        main_work.is_jp_region = AppMain.GeEnvGetDecideKey() == AppMain.GSE_DECIDE_KEY.GSD_DECIDE_KEY_O;
        main_work.vol_icon_col.r = byte.MaxValue;
        main_work.vol_icon_col.g = byte.MaxValue;
        main_work.vol_icon_col.b = (byte)0;
        main_work.vol_icon_col.a = byte.MaxValue;
        main_work.win_col.r = (byte)0;
        main_work.win_col.g = (byte)0;
        main_work.win_col.b = (byte)0;
        main_work.win_col.a = byte.MaxValue;
        main_work.win_size_rate[0] = 0.0f;
        main_work.win_size_rate[1] = 0.0f;
        main_work.ctrl_tab_pos_x[0] = AppMain.dm_opt_ctrl_nrml_disp_pos_tbl[main_work.ctrl_mode][0];
        main_work.ctrl_tab_pos_y[0] = AppMain.dm_opt_ctrl_nrml_disp_pos_tbl[main_work.ctrl_mode][1];
        main_work.ctrl_tab_pos_x[1] = AppMain.dm_opt_ctrl_clsc_disp_pos_tbl[main_work.ctrl_mode][0];
        main_work.ctrl_tab_pos_y[1] = AppMain.dm_opt_ctrl_clsc_disp_pos_tbl[main_work.ctrl_mode][1];
        main_work.decide_menu_col.r = byte.MaxValue;
        main_work.decide_menu_col.g = byte.MaxValue;
        main_work.decide_menu_col.b = byte.MaxValue;
        main_work.decide_menu_col.a = (byte)0;
        main_work.prev_nrml_disp_type = 2;
        main_work.top_crsr_pos_y = 250f;
        main_work.frm_update_time = 1f;
        main_work.obi_tex_pos[0] = 0.0f;
        main_work.obi_tex_pos[1] = 1120f;
    }

    private static void dmOptProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_OPT_MAIN_WORK work = (AppMain.DMS_OPT_MAIN_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
        {
            AppMain.mtTaskClearTcb(tcb);
            AppMain.dm_opt_mgr_p = (AppMain.DMS_OPT_MGR)null;
            if (AppMain.dm_opt_is_pause_maingame)
                AppMain.mtTaskEndPause();
            AppMain.dmOptSetNextEvt(work);
        }
        if ((work.flag & 2147483648U) > 0U && !AppMain.AoAccountIsCurrentEnable())
        {
            work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcFadeOut);
            work.flag &= (uint)int.MaxValue;
            work.next_evt = 1;
            if (AppMain.dm_opt_is_pause_maingame)
                AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 1U, 1U, 32f, true);
            else
                AppMain.IzFadeInitEasy(1U, 1U, 32f);
            AppMain.DmSndBgmPlayerExit();
            work.flag |= 1048576U;
            work.flag &= 4294967291U;
            work.flag &= 4294967293U;
            work.proc_input = (AppMain.DMS_OPT_MAIN_WORK._proc_input_)null;
            work.win_timer = 0.0f;
        }
        if (work.proc_update != null)
            work.proc_update(work);
        if (work.proc_draw == null)
            return;
        work.proc_draw(work);
    }

    private static void dmOptDest(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static void dmOptSetNextEvt(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        short req_id;
        if (((int)main_work.flag & 524288) != 0)
        {
            req_id = (short)10;
            AppMain.dm_opt_prev_evt = AppMain.SyGetEvtInfo().old_evt_id;
            main_work.flag &= 4294443007U;
        }
        else
        {
            req_id = AppMain.SyGetEvtInfo().old_evt_id;
            if (req_id == (short)10)
                req_id = AppMain.dm_opt_prev_evt;
        }
        if (req_id == (short)3)
            req_id = (short)4;
        if (main_work.next_evt == 1)
            req_id = (short)3;
        if (AppMain.dm_opt_is_pause_maingame)
            return;
        AppMain.SyDecideEvt(req_id);
        AppMain.SyChangeNextEvt();
    }

    private static void dmOptLoadFontData(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        AppMain.GsFontBuild();
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptIsLoadFontData);
    }

    private static void dmOptIsLoadFontData(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (!AppMain.GsFontIsBuilded())
            return;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptLoadRequest);
    }

    private static void dmOptLoadRequest(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        main_work.arc_amb_fs[0] = AppMain.amFsReadBackground("DEMO/OPTION/D_OPTION.AMB");
        main_work.arc_amb_fs[1] = AppMain.amFsReadBackground(AppMain.dm_opt_main_lng_amb_name_tbl[AppMain.GsEnvGetLanguage()]);
        for (int index = 0; index < 4; ++index)
            main_work.arc_cmn_amb_fs[index] = AppMain.amFsReadBackground(AppMain.dm_opt_menu_cmn_amb_name_tbl[index]);
        main_work.arc_cmn_amb_fs[4] = AppMain.amFsReadBackground(AppMain.dm_opt_menu_cmn_lng_amb_name_tbl[AppMain.GsEnvGetLanguage()]);
        main_work.manual_arc_amb_fs[0] = AppMain.amFsReadBackground("DEMO/MANUAL/D_MANUAL.AMB");
        main_work.manual_arc_amb_fs[1] = AppMain.amFsReadBackground(AppMain.dm_opt_manual_file_lng_amb_name_tbl[AppMain.GsEnvGetLanguage()]);
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcLoadWait);
    }

    private static void dmOptProcLoadWait(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (AppMain.dmOptIsDataLoad(main_work) <= 0)
            return;
        for (int index = 0; index < 2; ++index)
        {
            main_work.arc_amb[index] = AppMain.readAMBFile(main_work.arc_amb_fs[index]);
            main_work.ama[index] = AppMain.readAMAFile(AppMain.amBindGet(main_work.arc_amb[index], 0));
            string sPath;
            main_work.amb[index] = AppMain.readAMBFile(AppMain.amBindGet(main_work.arc_amb[index], 1, out sPath));
            main_work.amb[index].dir = sPath;
            AppMain.amFsClearRequest(main_work.arc_amb_fs[index]);
            main_work.arc_amb_fs[index] = (AppMain.AMS_FS)null;
            AppMain.AoTexBuild(main_work.tex[index], main_work.amb[index]);
            AppMain.AoTexLoad(main_work.tex[index]);
        }
        for (int index = 0; index < 5; ++index)
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
        if (!AppMain.dm_opt_is_pause_maingame)
        {
            AppMain.GsFontBuild();
            AppMain.DmSndBgmPlayerInit();
        }
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcLoadWait2);
    }

    private static void dmOptProcLoadWait2(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (AppMain.dmOptIsTexLoad(main_work) != 1)
            return;
        for (int index = 0; index < 2; ++index)
        {
            main_work.manual_arc_amb[index] = AppMain.readAMBFile(main_work.manual_arc_amb_fs[index]);
            AppMain.amFsClearRequest(main_work.manual_arc_amb_fs[index]);
            main_work.manual_arc_amb_fs[index] = (AppMain.AMS_FS)null;
        }
        AppMain.DmManualBuild(main_work.manual_arc_amb);
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcTexBuildWait);
    }

    private static void dmOptProcTexBuildWait(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (AppMain.dmOptIsTexLoad2(main_work) != 1)
            return;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcCheckLoadingEnd);
        if (!AppMain.dm_opt_is_pause_maingame)
            return;
        main_work.bgm_scb = AppMain.GsSoundAssignScb(0);
        main_work.bgm_scb.flag |= 2147483648U;
        main_work.se_handle = AppMain.GsSoundAllocSeHandle();
    }

    private static void dmOptProcCheckLoadingEnd(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcCreateAct);
        if (AppMain.dm_opt_is_pause_maingame)
            AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 0U, 32f, true);
        else
            AppMain.IzFadeInitEasy(0U, 0U, 32f);
    }

    private static void dmOptProcCreateAct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 102U; ++index)
        {
            AppMain.A2S_AMA_HEADER ama;
            AppMain.AOS_TEXTURE tex;
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
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(tex));
            main_work.act[(int)index] = AppMain.AoActCreate(ama, AppMain.g_dm_act_id_tbl_opt[(int)index]);
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
        AppMain.A2S_AMA_HEADER a2SAmaHeader = main_work.ama[0];
        AppMain.AOS_TEXTURE aosTexture = main_work.tex[0];
        main_work.obi_pos_y = 192f;
        main_work.proc_draw = new AppMain.DMS_OPT_MAIN_WORK._proc_draw_(AppMain.dmOptProcActDraw);
        main_work.proc_menu_draw = new AppMain.DMS_OPT_MAIN_WORK._proc_menu_draw_(AppMain.dmOptTopMenuDraw);
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcFadeIn);
        main_work.flag |= 2147483648U;
        if (!AppMain.dm_opt_is_pause_maingame)
            AppMain.DmSndBgmPlayerPlayBgm(0);
        else
            AppMain.GsSoundPlayBgm(main_work.bgm_scb, "snd_sng_menu", 32);
    }

    private static void dmOptProcFadeIn(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        main_work.proc_input = new AppMain.DMS_OPT_MAIN_WORK._proc_input_(AppMain.dmOptInputProcTopMenu);
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcTopMenuIdle);
    }

    private static void dmOptProcTopMenuIdle(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        CTrgAoAction trgReturn = main_work.trg_return;
        float frame = main_work.act[1].frame;
        if (trgReturn.GetState(0U)[10] && trgReturn.GetState(0U)[1])
            frame = 2f;
        else if (trgReturn.GetState(0U)[0])
            frame = 1f;
        else if (2.0 > (double)main_work.act[1].frame)
            frame = 0.0f;
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index = 1; index <= 2; ++index)
        {
            AppMain.AoActSetFrame(main_work.act[index], frame);
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
        }
        if (((int)main_work.flag & 2) != 0)
        {
            main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcFadeOut);
            if (AppMain.dm_opt_is_pause_maingame)
                AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 1U, 32f, true);
            else
                AppMain.IzFadeInitEasy(0U, 1U, 32f);
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
            if (!AppMain.dm_opt_is_pause_maingame)
            {
                AppMain.DmSoundPlaySE("Cancel");
            }
            else
            {
                AppMain.GsSoundPlaySe("Cancel", main_work.se_handle);
                AppMain.GsSoundStopBgm(main_work.bgm_scb, 32);
            }
        }
        else if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcTopMenuDecideEfct);
            AppMain.dmOptSetTopMenuDecideEfctData(main_work);
            if (!AppMain.dm_opt_is_pause_maingame)
                AppMain.DmSoundPlaySE("Ok");
            else
                AppMain.GsSoundPlaySe("Ok", main_work.se_handle);
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }
        else
        {
            if (((int)main_work.flag & 64) != 0)
            {
                main_work.cur_slct_top = AppMain.dmOptGetRevisedTopMenuNo(main_work.cur_slct_top, -1);
                if (!AppMain.dm_opt_is_pause_maingame)
                    AppMain.DmSoundPlaySE("Cursol");
                else
                    AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
                main_work.flag |= 262144U;
                AppMain.dmOptSetChngFocusCrsrData(main_work);
                main_work.flag &= 4294967231U;
                main_work.flag &= 4294967167U;
            }
            if (((int)main_work.flag & 128) != 0)
            {
                main_work.cur_slct_top = AppMain.dmOptGetRevisedTopMenuNo(main_work.cur_slct_top, 1);
                if (!AppMain.dm_opt_is_pause_maingame)
                    AppMain.DmSoundPlaySE("Cursol");
                else
                    AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
                main_work.flag |= 262144U;
                AppMain.dmOptSetChngFocusCrsrData(main_work);
                main_work.flag &= 4294967231U;
                main_work.flag &= 4294967167U;
            }
            if (((int)main_work.flag & 262144) == 0)
                return;
            AppMain.dmOptSetCtrlFocusChangeEfct(main_work);
            if (!AppMain.dmOptIsCtrlFocusChangeEfctEnd(main_work))
                return;
            main_work.flag &= 4294705151U;
        }
    }

    private static void dmOptProcTopMenuDecideEfct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (AppMain.dmOptIsTopMenuTabDecideEfctEnd(main_work))
        {
            AppMain.dmOptSetNextProcFunc(main_work);
            main_work.timer = 0.0f;
        }
        else
        {
            if ((main_work.flag & 262144U) > 0U)
            {
                AppMain.dmOptSetCtrlFocusChangeEfct(main_work);
                if (AppMain.dmOptIsCtrlFocusChangeEfctEnd(main_work))
                    main_work.flag &= 4294705151U;
            }
            ++main_work.timer;
        }
    }

    private static void dmOptProcManualStartFadeOut(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcManualIdle);
        main_work.flag &= (uint)int.MaxValue;
        AppMain.DmManualStart();
        main_work.proc_menu_draw = (AppMain.DMS_OPT_MAIN_WORK._proc_menu_draw_)null;
    }

    private static void dmOptProcManualIdle(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (!AppMain.DmManualIsExit())
            return;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcManualEndFadeIn);
        main_work.proc_menu_draw = new AppMain.DMS_OPT_MAIN_WORK._proc_menu_draw_(AppMain.dmOptTopMenuDraw);
        main_work.flag |= 2147483648U;
        if (AppMain.dm_opt_is_pause_maingame)
            AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 0U, 32f, true);
        else
            AppMain.IzFadeInitEasy(0U, 0U, 32f);
    }

    private static void dmOptProcManualEndFadeIn(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcTopMenuIdle);
        main_work.proc_input = new AppMain.DMS_OPT_MAIN_WORK._proc_input_(AppMain.dmOptInputProcTopMenu);
    }

    private static void dmOptProcSetMenuInEfct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if ((main_work.flag & 2048U) > 0U)
        {
            main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcSetMenuIdle);
            main_work.proc_input = new AppMain.DMS_OPT_MAIN_WORK._proc_input_(AppMain.dmOptInputProcSettingMenu);
            main_work.disp_flag |= 1U;
            main_work.flag &= 4294965247U;
        }
        else
            AppMain.dmOptSetWinOpenEfct(main_work);
    }

    private static void dmOptProcSetMenuIdle(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        CTrgAoAction trgReturn = main_work.trg_return;
        float frame = main_work.act[1].frame;
        if (trgReturn.GetState(0U)[10] && trgReturn.GetState(0U)[1])
            frame = 2f;
        else if (trgReturn.GetState(0U)[0])
            frame = 1f;
        else if (2.0 > (double)main_work.act[1].frame)
            frame = 0.0f;
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index = 1; index <= 2; ++index)
        {
            AppMain.AoActSetFrame(main_work.act[index], frame);
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
        }
        if ((main_work.flag & 2U) > 0U && (16777216U & main_work.flag) > 0U)
        {
            main_work.flag &= 4278190077U;
            if (!AppMain.dm_opt_is_pause_maingame)
                AppMain.DmSoundPlaySE("Cancel");
            else
                AppMain.GsSoundPlaySe("Cancel", main_work.se_handle);
        }
        else if (((int)main_work.flag & 2) != 0)
        {
            SOption instance = SOption.CreateInstance();
            instance.SetVolumeBgm((uint)(main_work.volume_data[0] * 10));
            instance.SetVolumeSe((uint)(main_work.volume_data[1] * 10));
            AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
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
            main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcSetMenuOutEfct);
            main_work.win_timer = 8f;
            main_work.disp_flag &= 4294967294U;
            if (!AppMain.dm_opt_is_pause_maingame)
                AppMain.DmSoundPlaySE("Cancel");
            else
                AppMain.GsSoundPlaySe("Cancel", main_work.se_handle);
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }
        else if (((int)main_work.flag & 4) != 0)
        {
            if (main_work.cur_slct_set == 2)
                AppMain.dmOptSetDefaultDataSetMenu(main_work);
            if (!AppMain.dm_opt_is_pause_maingame)
                AppMain.DmSoundPlaySE("Ok");
            else
                AppMain.GsSoundPlaySe("Ok", main_work.se_handle);
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }
        else if ((main_work.flag & 64U) > 0U)
        {
            main_work.cur_slct_set = AppMain.dmOptGetRevisedSettingMenuNo(main_work.cur_slct_set, -1);
            if (!AppMain.dm_opt_is_pause_maingame)
                AppMain.DmSoundPlaySE("Cursol");
            else
                AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
            main_work.flag &= 4294967231U;
            main_work.flag &= 4294967167U;
        }
        else if ((main_work.flag & 128U) > 0U)
        {
            main_work.cur_slct_set = AppMain.dmOptGetRevisedSettingMenuNo(main_work.cur_slct_set, 1);
            if (!AppMain.dm_opt_is_pause_maingame)
                AppMain.DmSoundPlaySE("Cursol");
            else
                AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
            main_work.flag &= 4294967231U;
            main_work.flag &= 4294967167U;
        }
        else
        {
            AppMain.dmOptSetVolPushEfct(main_work);
            if ((main_work.flag & 1024U) > 0U)
                AppMain.dmOptSetDfltPushEfct(main_work);
            if ((16777216U & main_work.flag) > 0U)
            {
                if ((double)main_work.ctrl_win_window_prgrs >= 1.0)
                    return;
                main_work.ctrl_win_window_prgrs += 0.125f;
                if (1.0 >= (double)main_work.ctrl_win_window_prgrs)
                    return;
                main_work.ctrl_win_window_prgrs = 1f;
            }
            else
            {
                if (0.0 >= (double)main_work.ctrl_win_window_prgrs)
                    return;
                main_work.ctrl_win_window_prgrs -= 0.125f;
                if ((double)main_work.ctrl_win_window_prgrs >= 0.0)
                    return;
                main_work.ctrl_win_window_prgrs = 0.0f;
            }
        }
    }

    private static void dmOptProcSetMenuOutEfct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if ((main_work.flag & 2048U) > 0U)
        {
            main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcTopMenuIdle);
            main_work.proc_input = new AppMain.DMS_OPT_MAIN_WORK._proc_input_(AppMain.dmOptInputProcTopMenu);
            main_work.proc_menu_draw = new AppMain.DMS_OPT_MAIN_WORK._proc_menu_draw_(AppMain.dmOptTopMenuDraw);
            main_work.state = 0;
            main_work.flag &= 4294965247U;
        }
        else
            AppMain.dmOptSetWinCloseEfct(main_work);
    }

    private static void dmOptProcCtrlMenuIdle(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        CTrgAoAction trgReturn = main_work.trg_return;
        float frame = main_work.act[1].frame;
        if (trgReturn.GetState(0U)[10] && trgReturn.GetState(0U)[1])
            frame = 2f;
        else if (trgReturn.GetState(0U)[0])
            frame = 1f;
        else if (2.0 > (double)main_work.act[1].frame)
            frame = 0.0f;
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index = 1; index <= 2; ++index)
        {
            AppMain.AoActSetFrame(main_work.act[index], frame);
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
        }
        if ((main_work.flag & 2U) <= 0U)
            return;
        main_work.proc_input = new AppMain.DMS_OPT_MAIN_WORK._proc_input_(AppMain.dmOptInputProcTopMenu);
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcTopMenuIdle);
        main_work.proc_menu_draw = new AppMain.DMS_OPT_MAIN_WORK._proc_menu_draw_(AppMain.dmOptTopMenuDraw);
        main_work.state = 0;
        main_work.top_crsr_pos_y = (float)(250.0 + (double)main_work.cur_slct_top * 220.0);
        if (!AppMain.dm_opt_is_pause_maingame)
            AppMain.DmSoundPlaySE("Cancel");
        else
            AppMain.GsSoundPlaySe("Cancel", main_work.se_handle);
        main_work.flag &= 4294967291U;
        main_work.flag &= 4294967293U;
    }

    private static void dmOptProcStfrlStartFadeOut(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcStfrlIdle);
        main_work.proc_draw = (AppMain.DMS_OPT_MAIN_WORK._proc_draw_)null;
        main_work.flag &= (uint)int.MaxValue;
        AppMain.DmStaffRollStart((object)null);
    }

    private static void dmOptProcStfrlIdle(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (!AppMain.DmStaffRollIsExit())
            return;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcStfrlEndFadeIn);
        main_work.proc_draw = new AppMain.DMS_OPT_MAIN_WORK._proc_draw_(AppMain.dmOptProcActDraw);
        AppMain.AoActSysSetDrawStateEnable(true);
        AppMain.AoActSysSetDrawState(AppMain.dm_opt_draw_state);
        main_work.flag |= 2147483648U;
        if (AppMain.dm_opt_is_pause_maingame)
        {
            AppMain.GsSoundPlayBgm(main_work.bgm_scb, "snd_sng_menu", 32);
            AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 0U, 32f, true);
        }
        else
        {
            AppMain.DmSndBgmPlayerPlayBgm(0);
            AppMain.IzFadeInitEasy(0U, 0U, 32f);
        }
    }

    private static void dmOptProcStfrlEndFadeIn(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        main_work.flag |= 2147483648U;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcTopMenuIdle);
        main_work.proc_input = new AppMain.DMS_OPT_MAIN_WORK._proc_input_(AppMain.dmOptInputProcTopMenu);
    }

    private static void dmOptProcFadeOut(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (AppMain.dm_opt_show_xboxlive && AppMain.dm_xbox_show_progress > 0)
            AppMain.dm_xbox_show_progress -= 20;
        if (!AppMain.IzFadeIsEnd())
            return;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcStopDraw);
        main_work.proc_draw = (AppMain.DMS_OPT_MAIN_WORK._proc_draw_)null;
        main_work.timer = 0.0f;
        if (!AppMain.dm_opt_is_pause_maingame)
            return;
        AppMain.GsSoundStopBgm(main_work.bgm_scb, 0);
        AppMain.GsSoundResignScb(main_work.bgm_scb);
        main_work.bgm_scb = (AppMain.GSS_SND_SCB)null;
        AppMain.GsSoundFreeSeHandle(main_work.se_handle);
        main_work.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
    }

    private static void dmOptProcStopDraw(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcDataRelease);
    }

    private static void dmOptProcDataRelease(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
            AppMain.AoTexRelease(main_work.tex[index]);
        for (int index = 0; index < 5; ++index)
            AppMain.AoTexRelease(main_work.cmn_tex[index]);
        AppMain.DmManualFlush();
        int num = AppMain.dm_opt_is_pause_maingame ? 1 : 0;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcFinish);
    }

    private static void dmOptProcFinish(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (AppMain.dmOptIsTexRelease(main_work) != 1)
            return;
        if (AppMain.dm_opt_show_xboxlive)
        {
            LiveFeature.endInterrupt();
            AppMain.dm_opt_show_xboxlive = false;
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
                AppMain.AoActDelete(main_work.act[index]);
                main_work.act[index] = (AppMain.AOS_ACTION)null;
            }
        }
        for (int index = 0; index < 2; ++index)
        {
            if (main_work.arc_amb[index] != null)
                main_work.arc_amb[index] = (AppMain.AMS_AMB_HEADER)null;
        }
        for (int index = 0; index < 5; ++index)
        {
            if (main_work.arc_cmn_amb[index] != null)
                main_work.arc_cmn_amb[index] = (AppMain.AMS_AMB_HEADER)null;
        }
        for (int index = 0; index < 2; ++index)
        {
            if (main_work.manual_arc_amb[index] != null)
                main_work.manual_arc_amb[index] = (AppMain.AMS_AMB_HEADER)null;
        }
        AppMain.DmSaveMenuStart(true, false);
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcFinishWaitSave);
    }

    private static void dmOptProcFinishWaitSave(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (!AppMain.DmSaveIsExit())
            return;
        main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcWaitFinished);
    }

    private static void dmOptProcWaitFinished(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 1048576) != 0)
        {
            if (!AppMain.DmSndBgmPlayerIsTaskExit())
                return;
            main_work.flag |= 1U;
            main_work.proc_update = (AppMain.DMS_OPT_MAIN_WORK._proc_update_)null;
            main_work.flag &= 4293918719U;
        }
        else
        {
            main_work.flag |= 1U;
            main_work.proc_update = (AppMain.DMS_OPT_MAIN_WORK._proc_update_)null;
        }
    }

    private static void dmOptInputProcTopMenu(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.trg_return.GetState(0U)[10] && main_work.trg_return.GetState(0U)[1] || AppMain.isBackKeyPressed())
        {
            AppMain.setBackKeyRequest(false);
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

    private static void dmOptInputProcSettingMenu(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.trg_return.GetState(0U)[10] && main_work.trg_return.GetState(0U)[1] || AppMain.isBackKeyPressed())
        {
            AppMain.setBackKeyRequest(false);
            main_work.flag |= 2U;
        }
        else
        {
            if ((16777216 & (int)main_work.flag) == 0)
            {
                if (main_work.trg_ctrl_btn[0].GetState(0U)[2])
                {
                    SOption.CreateInstance().SetControl(SOption.EControl.Type.Tilt);
                    if (!AppMain.dm_opt_is_pause_maingame)
                        AppMain.DmSoundPlaySE("Cursol");
                    else
                        AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
                }
                CTrgAoAction ctrgAoAction = main_work.trg_ctrl_btn[1];
                if (ctrgAoAction.GetState(0U)[2])
                {
                    SOption instance = SOption.CreateInstance();
                    if (instance.GetControl() == SOption.EControl.Type.Tilt)
                        instance.SetControl(SOption.EControl.Type.VirtualPadDown);
                    if (!AppMain.dm_opt_is_pause_maingame)
                        AppMain.DmSoundPlaySE("Cursol");
                    else
                        AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
                }
                else if (ctrgAoAction.GetState(0U)[10] && ctrgAoAction.GetState(0U)[1])
                {
                    main_work.flag |= 16777216U;
                    if (!AppMain.dm_opt_is_pause_maingame)
                    {
                        AppMain.DmSoundPlaySE("Window");
                        return;
                    }
                    AppMain.GsSoundPlaySe("Window", main_work.se_handle);
                    return;
                }
            }
            if (0.0 < (double)main_work.ctrl_win_window_prgrs)
            {
                if (1.0 != (double)main_work.ctrl_win_window_prgrs)
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
                if (!AppMain.dm_opt_is_pause_maingame)
                    AppMain.DmSoundPlaySE("Cursol");
                else
                    AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
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
                            main_work.volume_data[0] = AppMain.dmOptGetRevisedVolume(main_work.volume_data[0], -1);
                            main_work.push_efct_timer[1] = 12f;
                            AppMain.DmSoundSetVolumeBGM((float)main_work.volume_data[0]);
                            break;
                        case 1:
                            main_work.volume_data[1] = AppMain.dmOptGetRevisedVolume(main_work.volume_data[1], -1);
                            main_work.push_efct_timer[3] = 12f;
                            AppMain.DmSoundSetVolumeSE((float)main_work.volume_data[1]);
                            if (!AppMain.dm_opt_is_pause_maingame)
                            {
                                AppMain.DmSoundPlaySE("Cursol");
                                break;
                            }
                            AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
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
                            main_work.volume_data[0] = AppMain.dmOptGetRevisedVolume(main_work.volume_data[0], 1);
                            main_work.push_efct_timer[0] = 12f;
                            AppMain.DmSoundSetVolumeBGM((float)main_work.volume_data[0]);
                            break;
                        case 1:
                            main_work.volume_data[1] = AppMain.dmOptGetRevisedVolume(main_work.volume_data[1], 1);
                            main_work.push_efct_timer[2] = 12f;
                            AppMain.DmSoundSetVolumeSE((float)main_work.volume_data[1]);
                            if (!AppMain.dm_opt_is_pause_maingame)
                            {
                                AppMain.DmSoundPlaySE("Cursol");
                                break;
                            }
                            AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
                            break;
                    }
                }
            }
        }
    }

    private static void dmOptInputProcControlMenu(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if (main_work.trg_return.GetState(0U)[10] && main_work.trg_return.GetState(0U)[1] || AppMain.isBackKeyPressed())
        {
            AppMain.setBackKeyRequest(false);
            main_work.flag |= 2U;
        }
        else
        {
            if ((AoPad.AoPadStand() & ControllerConsts.CONFIRM) <= 0)
                return;
            main_work.flag |= 4U;
        }
    }

    private static void dmOptProcActDraw(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        AppMain.dmOptSetObiEfctPos(main_work);
        AppMain.dmOptCommonDraw(main_work);
        AppMain.dmOptCommonFixDraw(main_work);
        if (main_work.proc_menu_draw != null)
        {
            if (!AppMain.dm_opt_show_xboxlive)
                main_work.proc_menu_draw(main_work);
            else if (AppMain.dm_xbox_show_progress < 100 && main_work.proc_update != new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcFadeOut))
                AppMain.dm_xbox_show_progress += 5;
        }
        if (AppMain.dm_opt_is_pause_maingame)
        {
            if (main_work.draw_state == 0U)
                return;
            AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.dmOptTaskDraw), (ushort)32768, 0U);
        }
        else
            AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.dmOptTaskDraw), (ushort)32768, 0U);
    }

    private static void dmOptTaskDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(AppMain.dm_opt_draw_state);
        AppMain.amDrawEndScene();
    }

    private static void dmOptCommonDraw(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(4096U);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[0]));
        AppMain.AoActSortRegAction(main_work.act[97]);
        if (!AppMain.dm_opt_show_xboxlive || LiveFeature.interruptMainLoop == 1)
            AppMain.AoActSortRegAction(main_work.act[99]);
        AppMain.AoActSortRegAction(main_work.act[98]);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[0]));
        AppMain.AoActUpdate(main_work.act[97], 1f);
        if (!AppMain.dm_opt_show_xboxlive || LiveFeature.interruptMainLoop == 1)
            AppMain.AoActUpdate(main_work.act[99], 0.0f);
        AppMain.AoActUpdate(main_work.act[98], 0.0f);
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private static void dmOptCommonFixDraw(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(12288U);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
        if (!AppMain.dm_opt_show_xboxlive || LiveFeature.interruptMainLoop == 1)
        {
            for (int index = 0; index <= 0; ++index)
                AppMain.AoActSortRegAction(main_work.act[index]);
        }
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
        if (!AppMain.dm_opt_show_xboxlive)
        {
            for (int index = 69; index <= 69; ++index)
                AppMain.AoActSortRegAction(main_work.act[index]);
        }
        for (int index = 1; index <= 2; ++index)
            AppMain.AoActSortRegAction(main_work.act[index]);
        AppMain.AoActSortRegAction(main_work.act[101]);
        AppMain.AoActSetFrame(main_work.act[69], (float)main_work.state);
        if (main_work.is_jp_region)
            AppMain.AoActSetFrame(main_work.act[100], 0.0f);
        else
            AppMain.AoActSetFrame(main_work.act[100], 1f);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
        for (int index = 0; index <= 0; ++index)
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
        AppMain.AoActUpdate(main_work.act[69], 0.0f);
        main_work.act[69].sprite.center_y += 10f;
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[4]));
        AppMain.AoActUpdate(main_work.act[101], 0.0f);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.cmn_tex[3]));
        for (int index = 1; index <= 2; ++index)
        {
            float frame = 2.0 <= (double)main_work.act[index].frame ? 1f : 0.0f;
            AppMain.AoActUpdate(main_work.act[index], frame);
        }
        main_work.trg_return.Update();
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private static void dmOptTopMenuDraw(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(8192U);
        for (int index = 0; index < 4; ++index)
        {
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
            AppMain.AoActSortRegAction(main_work.act[3]);
            AppMain.AoActSortRegAction(main_work.act[4]);
            AppMain.AoActSortRegAction(main_work.act[5]);
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
            AppMain.AoActSortRegAction(main_work.act[AppMain.dm_opt_top_menu_tex_tbl[index]]);
            if ((16U & main_work.flag) > 0U)
            {
                if (index == main_work.cur_slct_top)
                {
                    float frame = 2f + main_work.timer;
                    AppMain.AoActSetFrame(main_work.act[3], frame);
                    AppMain.AoActSetFrame(main_work.act[4], frame);
                    AppMain.AoActSetFrame(main_work.act[5], frame);
                    AppMain.AoActSetFrame(main_work.act[AppMain.dm_opt_top_menu_tex_tbl[index]], 1f);
                }
                else
                {
                    AppMain.AoActSetFrame(main_work.act[3], 0.0f);
                    AppMain.AoActSetFrame(main_work.act[4], 0.0f);
                    AppMain.AoActSetFrame(main_work.act[5], 0.0f);
                    AppMain.AoActSetFrame(main_work.act[AppMain.dm_opt_top_menu_tex_tbl[index]], 0.0f);
                }
            }
            else if (AppMain.IzFadeIsExe() && !AppMain.IzFadeIsEnd())
            {
                AppMain.AoActSetFrame(main_work.act[3], 0.0f);
                AppMain.AoActSetFrame(main_work.act[4], 0.0f);
                AppMain.AoActSetFrame(main_work.act[5], 0.0f);
                AppMain.AoActSetFrame(main_work.act[AppMain.dm_opt_top_menu_tex_tbl[index]], 0.0f);
            }
            else if (main_work.trg_slct[index].GetState(0U)[0])
            {
                AppMain.AoActSetFrame(main_work.act[3], 1f);
                AppMain.AoActSetFrame(main_work.act[4], 1f);
                AppMain.AoActSetFrame(main_work.act[5], 1f);
                AppMain.AoActSetFrame(main_work.act[AppMain.dm_opt_top_menu_tex_tbl[index]], 1f);
            }
            else
            {
                AppMain.AoActSetFrame(main_work.act[3], 0.0f);
                AppMain.AoActSetFrame(main_work.act[4], 0.0f);
                AppMain.AoActSetFrame(main_work.act[5], 0.0f);
                AppMain.AoActSetFrame(main_work.act[AppMain.dm_opt_top_menu_tex_tbl[index]], 0.0f);
            }
            AppMain.AoActAcmPush();
            AppMain.AoActAcmInit();
            AppMain.AoActAcmApplyTrans((float)(240.0 + (double)(index % 2) * 480.0), (float)(250.0 + (double)(index / 2) * 220.0), 0.0f);
            if (main_work.cur_slct_top == index && (main_work.flag & 16U) > 0U)
                AppMain.AoActAcmApplyFade(main_work.decide_menu_col);
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
            AppMain.AoActUpdate(main_work.act[3], 0.0f);
            AppMain.AoActUpdate(main_work.act[4], 0.0f);
            AppMain.AoActUpdate(main_work.act[5], 0.0f);
            main_work.trg_slct[index].Update();
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
            AppMain.AoActUpdate(main_work.act[AppMain.dm_opt_top_menu_tex_tbl[index]], 0.0f);
            main_work.act[AppMain.dm_opt_top_menu_tex_tbl[index]].sprite.center_y += 7f;
            AppMain.AoActAcmPop();
            AppMain.AoActSortExecute();
            AppMain.AoActSortDraw();
            AppMain.AoActSortUnregAll();
        }
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private static void dmOptSettingMenuDraw(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        AppMain.AOS_ACT_COL aosActCol = new AppMain.AOS_ACT_COL();
        float num = 0.0f;
        AppMain.AoActSysSetDrawTaskPrio(8192U);
        AppMain.AoWinSysDrawState(0, AppMain.AoTexGetTexList(main_work.tex[0]), 3U, 480f, 356f, 840f * main_work.win_size_rate[0], (float)(400.0 * (double)main_work.win_size_rate[1] * 0.899999976158142), AppMain.dm_opt_draw_state);
        if (((int)main_work.disp_flag & 1) == 0)
            return;
        for (int index1 = 0; index1 < 2; ++index1)
        {
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
            if (index1 < 2)
            {
                for (uint index2 = 10; index2 <= 21U; ++index2)
                    AppMain.AoActSortRegAction(main_work.act[(int)index2]);
                if (main_work.volume_data[index1] == 10)
                    AppMain.AoActSortRegAction(main_work.act[6]);
                if (main_work.volume_data[index1] > 0)
                    AppMain.AoActSortRegAction(main_work.act[7]);
                AppMain.AoActSortRegAction(main_work.act[8]);
                AppMain.AoActSortRegAction(main_work.act[9]);
            }
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
            AppMain.AoActSortRegAction(main_work.act[AppMain.dm_opt_set_menu_tex_tbl[index1]]);
            if (index1 > 1)
            {
                AppMain.AoActSortRegAction(main_work.act[78]);
                AppMain.AoActSortRegAction(main_work.act[79]);
            }
            if (main_work.cur_slct_set == index1)
            {
                AppMain.AoActSetFrame(main_work.act[AppMain.dm_opt_set_menu_tex_tbl[index1]], 0.0f);
                aosActCol.r = aosActCol.g = aosActCol.b = aosActCol.a = byte.MaxValue;
            }
            else
            {
                aosActCol.r = aosActCol.g = aosActCol.b = byte.MaxValue;
                aosActCol.a = (byte)60;
            }
            AppMain.AoActSetFrame(main_work.act[6], 1f);
            AppMain.AoActSetFrame(main_work.act[7], (float)(main_work.volume_data[index1] % 10));
            AppMain.AoActSetFrame(main_work.act[8], 0.0f);
            if (index1 <= 1)
            {
                for (int index2 = 0; index2 < 10; ++index2)
                {
                    if (index2 < main_work.volume_data[index1])
                        AppMain.AoActSetFrame(main_work.act[12 + index2], 0.0f);
                    else
                        AppMain.AoActSetFrame(main_work.act[12 + index2], 1f);
                }
            }
            if (main_work.set_vbrt == 0)
            {
                AppMain.AoActSetFrame(main_work.act[78], 0.0f);
                AppMain.AoActSetFrame(main_work.act[79], 1f);
            }
            else
            {
                AppMain.AoActSetFrame(main_work.act[78], 1f);
                AppMain.AoActSetFrame(main_work.act[79], 0.0f);
            }
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
            for (int index2 = 6; index2 <= 21; ++index2)
            {
                AppMain.AoActAcmPush();
                AppMain.AoActAcmInit();
                AppMain.AoActAcmApplyTrans(480f + num, AppMain.dm_opt_set_tab_pos_y_tbl[index1], 0.0f);
                if ((double)main_work.push_efct_timer[2 * index1] > 0.0 && index2 == 10)
                    AppMain.AoActAcmApplyColor(main_work.vol_icon_col);
                if ((double)main_work.push_efct_timer[1 + 2 * index1] > 0.0 && index2 == 11)
                    AppMain.AoActAcmApplyColor(main_work.vol_icon_col);
                AppMain.AoActUpdate(main_work.act[index2], 0.0f);
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
                            ctrgAoActionArray = (CTrgAoAction[])null;
                            break;
                    }
                    ctrgAoActionArray?[index3].Update();
                }
                AppMain.AoActAcmPop();
            }
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
            for (uint index2 = 74; index2 <= 79U; ++index2)
            {
                AppMain.AoActAcmPush();
                AppMain.AoActAcmInit();
                AppMain.AoActAcmApplyTrans(480f, AppMain.dm_opt_set_tab_pos_y_tbl[index1], 0.0f);
                AppMain.AoActUpdate(main_work.act[(int)index2], 0.0f);
                AppMain.AoActAcmPop();
            }
            AppMain.AoActSortExecute();
            AppMain.AoActSortDraw();
            AppMain.AoActSortUnregAll();
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
            AppMain.AoActSetFrame(main_work.act[index], frame1);
        for (int index = 80; index < 81; ++index)
            AppMain.AoActSetFrame(main_work.act[index], frame1);
        for (int index = 25; index < 28; ++index)
            AppMain.AoActSetFrame(main_work.act[index], frame2);
        for (int index = 81; index < 82; ++index)
            AppMain.AoActSetFrame(main_work.act[index], frame2);
        AppMain.AoActAcmPush();
        AppMain.AoActAcmInit();
        AppMain.AoActAcmApplyTrans(480f + num, AppMain.dm_opt_set_tab_pos_y_tbl[2], 0.0f);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
        for (int index = 22; index < 28; ++index)
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
        AppMain.AoActUpdate(main_work.act[76], 0.0f);
        for (int index = 80; index < 82; ++index)
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
        for (int index = 0; index < main_work.trg_ctrl_btn.Length; ++index)
            main_work.trg_ctrl_btn[index].Update();
        AppMain.AoActAcmPop();
        AppMain.AoActSortRegAction(main_work.act[76]);
        for (int index = 22; index < 28; ++index)
            AppMain.AoActSortRegAction(main_work.act[index]);
        for (int index = 80; index < 82; ++index)
            AppMain.AoActSortRegAction(main_work.act[index]);
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
        if (0.0 >= (double)main_work.ctrl_win_window_prgrs)
            return;
        AppMain.AoWinSysDrawState(0, AppMain.AoTexGetTexList(main_work.cmn_tex[3]), 0U, 480f, 356f, 1280f * main_work.ctrl_win_window_prgrs, (float)(720.0 * (double)main_work.ctrl_win_window_prgrs * 0.899999976158142), AppMain.dm_opt_draw_state);
        if (1.0 == (double)main_work.ctrl_win_window_prgrs)
        {
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
            for (int index = 28; index < 42; ++index)
                AppMain.AoActUpdate(main_work.act[index]);
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
            for (int index = 82; index < 88; ++index)
                AppMain.AoActUpdate(main_work.act[index]);
            for (int index = 0; index < main_work.ctrl_win_trg_btn.Length; ++index)
                main_work.ctrl_win_trg_btn[index].Update();
            SOption.EControl.Type control = SOption.CreateInstance().GetControl();
            if (SOption.EControl.Type.VirtualPadUp == control)
            {
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
                for (int index = 36; index < 39; ++index)
                {
                    AppMain.AOS_ACTION act = main_work.act[index];
                    AppMain.AoActSetFrame(act, 1f);
                    AppMain.AoActUpdate(act, 0.0f);
                }
                for (int index = 39; index < 42; ++index)
                {
                    AppMain.AOS_ACTION act = main_work.act[index];
                    AppMain.AoActSetFrame(act, 0.0f);
                    AppMain.AoActUpdate(act, 0.0f);
                }
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
                AppMain.AoActSetFrame(main_work.act[84], 1f);
                AppMain.AoActSetFrame(main_work.act[85], 0.0f);
                for (int index = 84; index < 86; ++index)
                    AppMain.AoActUpdate(main_work.act[index], 0.0f);
            }
            else
            {
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
                for (int index = 36; index < 39; ++index)
                {
                    AppMain.AOS_ACTION act = main_work.act[index];
                    AppMain.AoActSetFrame(act, 0.0f);
                    AppMain.AoActUpdate(act, 0.0f);
                }
                for (int index = 39; index < 42; ++index)
                {
                    AppMain.AOS_ACTION act = main_work.act[index];
                    AppMain.AoActSetFrame(act, 1f);
                    AppMain.AoActUpdate(act, 0.0f);
                }
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
                AppMain.AoActSetFrame(main_work.act[84], 0.0f);
                AppMain.AoActSetFrame(main_work.act[85], 1f);
                for (int index = 84; index < 86; ++index)
                    AppMain.AoActUpdate(main_work.act[index], 0.0f);
            }
            for (int index = 28; index < 29; ++index)
            {
                AppMain.AOS_ACTION aosAction = main_work.act[index];
                AppMain.AoActSortRegAction(main_work.act[index]);
            }
            for (int index = 36; index < 42; ++index)
            {
                AppMain.AOS_ACTION aosAction = main_work.act[index];
                AppMain.AoActSortRegAction(main_work.act[index]);
            }
            for (int index = 82; index < 86; ++index)
            {
                AppMain.AOS_ACTION aosAction = main_work.act[index];
                AppMain.AoActSortRegAction(main_work.act[index]);
            }
            if (SOption.EControl.Type.VirtualPadUp == control)
            {
                AppMain.AoActSortRegAction(main_work.act[31]);
                for (int index = 32; index < 36; ++index)
                {
                    AppMain.AOS_ACTION aosAction = main_work.act[index];
                    AppMain.AoActSortRegAction(main_work.act[index]);
                }
                AppMain.AoActSortRegAction(main_work.act[87]);
            }
            else
            {
                for (int index = 29; index < 31; ++index)
                {
                    AppMain.AOS_ACTION aosAction = main_work.act[index];
                    AppMain.AoActSortRegAction(main_work.act[index]);
                }
                AppMain.AoActSortRegAction(main_work.act[86]);
            }
        }
        AppMain.AoActSortRegAction(main_work.act[101]);
        for (int index = 1; index < 3; ++index)
            AppMain.AoActSortRegAction(main_work.act[index]);
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private static void dmOptControlMenuDraw(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(8192U);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
        for (int index = 42; index <= 68; ++index)
        {
            if (index >= 52 && index <= 56)
            {
                if ((AppMain.g_gs_main_sys_info.game_flag & 32U) > 0U && main_work.act[index] != null)
                    AppMain.AoActSortRegAction(main_work.act[index]);
            }
            else if (index != 51 && main_work.act[index] != null)
                AppMain.AoActSortRegAction(main_work.act[index]);
        }
        if (SOption.CreateInstance().GetControl() != SOption.EControl.Type.Tilt)
        {
            if (main_work.act[50] != null)
                AppMain.AoActSetFrame(main_work.act[50], 0.0f);
            if (main_work.act[55] != null)
                AppMain.AoActSetFrame(main_work.act[55], 0.0f);
        }
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
        for (int index = 88; index <= 96; ++index)
        {
            if (index == 92)
            {
                if ((AppMain.g_gs_main_sys_info.game_flag & 32U) > 0U && main_work.act[index] != null)
                    AppMain.AoActSortRegAction(main_work.act[index]);
            }
            else if (main_work.act[index] != null)
                AppMain.AoActSortRegAction(main_work.act[index]);
        }
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[0]));
        for (int index = 42; index <= 68; ++index)
        {
            if (main_work.act[index] != null)
                AppMain.AoActUpdate(main_work.act[index]);
        }
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(main_work.tex[1]));
        for (int index = 88; index <= 96; ++index)
        {
            if (main_work.act[index] != null)
                AppMain.AoActUpdate(main_work.act[index]);
        }
        AppMain.AoActSortExecuteFix();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private static void dmOptSetObiEfctPos(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
        {
            if ((double)main_work.obi_tex_pos[(int)index] < -1120.0)
                main_work.obi_tex_pos[(int)index] = 1120f;
            main_work.obi_tex_pos[(int)index] += -3f;
        }
    }

    private static void dmOptSetNextProcFunc(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        switch (main_work.cur_slct_top)
        {
            case 0:
                main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcManualStartFadeOut);
                main_work.proc_input = (AppMain.DMS_OPT_MAIN_WORK._proc_input_)null;
                main_work.state = 0;
                if (AppMain.dm_opt_is_pause_maingame)
                {
                    AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 1U, 32f, true);
                    break;
                }
                AppMain.IzFadeInitEasy(0U, 1U, 32f);
                break;
            case 1:
                main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcCtrlMenuIdle);
                main_work.proc_input = new AppMain.DMS_OPT_MAIN_WORK._proc_input_(AppMain.dmOptInputProcControlMenu);
                main_work.proc_menu_draw = new AppMain.DMS_OPT_MAIN_WORK._proc_menu_draw_(AppMain.dmOptControlMenuDraw);
                AppMain.dmOptControlResetAct(main_work);
                main_work.state = 1;
                break;
            case 2:
                main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcSetMenuInEfct);
                main_work.proc_input = (AppMain.DMS_OPT_MAIN_WORK._proc_input_)null;
                main_work.proc_menu_draw = new AppMain.DMS_OPT_MAIN_WORK._proc_menu_draw_(AppMain.dmOptSettingMenuDraw);
                main_work.state = 2;
                main_work.cur_slct_set = 0;
                if (!AppMain.dm_opt_is_pause_maingame)
                    AppMain.DmSoundPlaySE("Window");
                else
                    AppMain.GsSoundPlaySe("Window", main_work.se_handle, 0);
                AppMain.dmOptSetSaveOptionData(main_work);
                break;
            case 3:
                main_work.proc_update = new AppMain.DMS_OPT_MAIN_WORK._proc_update_(AppMain.dmOptProcStfrlStartFadeOut);
                main_work.proc_input = (AppMain.DMS_OPT_MAIN_WORK._proc_input_)null;
                main_work.state = 0;
                if (AppMain.dm_opt_is_pause_maingame)
                    AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 1U, 32f, true);
                else
                    AppMain.IzFadeInitEasy(0U, 1U, 32f);
                if (!AppMain.dm_opt_is_pause_maingame)
                {
                    AppMain.DmSndBgmPlayerBgmStop();
                    break;
                }
                AppMain.GsSoundStopBgm(main_work.bgm_scb, 32);
                break;
        }
    }

    private static void dmOptSetDefaultDataSetMenu(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
            main_work.volume_data[(int)index] = 10;
        AppMain.DmSoundSetVolumeBGM((float)main_work.volume_data[0]);
        AppMain.DmSoundSetVolumeSE((float)main_work.volume_data[1]);
        main_work.set_vbrt = 0;
    }

    private static void dmOptSetTopMenuDecideEfctData(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        main_work.flag |= 16U;
    }

    private static void dmOptSetTopMenuTabDecideEfct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        float num1 = (float)main_work.decide_menu_col.a;
        if ((double)main_work.timer <= 8.0)
        {
            float num2 = 31.875f;
            num1 += num2;
            if ((double)num1 >= (double)byte.MaxValue)
                num1 = (float)byte.MaxValue;
        }
        else if ((double)main_work.timer <= 16.0)
        {
            float num2 = 31.875f;
            num1 -= num2;
            if ((double)num1 < 0.0)
                num1 = 0.0f;
        }
        main_work.decide_menu_col.a = (byte)num1;
    }

    private static bool dmOptIsTopMenuTabDecideEfctEnd(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if ((double)main_work.timer <= 28.0)
            return false;
        main_work.flag &= 4294967279U;
        main_work.decide_menu_col.a = (byte)0;
        main_work.timer = 0.0f;
        return true;
    }

    private static void dmOptSetCtrlFocusChangeEfct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        float num = (main_work.dst_crsr_pos_y - main_work.src_crsr_pos_y) / 8f;
        main_work.top_crsr_pos_y += num;
    }

    private static bool dmOptIsCtrlFocusChangeEfctEnd(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        float num = main_work.dst_crsr_pos_y - main_work.src_crsr_pos_y;
        if ((double)main_work.top_crsr_pos_y >= (double)main_work.dst_crsr_pos_y && (double)num >= 0.0)
        {
            main_work.top_crsr_pos_y = main_work.dst_crsr_pos_y;
            return true;
        }
        if ((double)main_work.top_crsr_pos_y > (double)main_work.dst_crsr_pos_y || (double)num > 0.0)
            return false;
        main_work.top_crsr_pos_y = main_work.dst_crsr_pos_y;
        return true;
    }

    private static void dmOptSetVolPushEfct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 4; ++index)
        {
            if ((double)main_work.push_efct_timer[index] > 0.0)
                --main_work.push_efct_timer[index];
            else
                main_work.push_efct_timer[index] = 0.0f;
        }
    }

    private static void dmOptSetDfltPushEfct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if ((double)main_work.efct_timer > 10.0)
        {
            main_work.flag &= 4294966271U;
            main_work.efct_timer = 0.0f;
        }
        ++main_work.efct_timer;
    }

    private static void dmOptSetWinOpenEfct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        if ((double)main_work.win_timer > 8.0)
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
            main_work.win_size_rate[(int)index] = (double)main_work.win_timer == 0.0 ? 1f : main_work.win_timer / 8f;
            if ((double)main_work.win_size_rate[(int)index] > 1.0)
                main_work.win_size_rate[(int)index] = 1f;
        }
    }

    private static void dmOptSetWinCloseEfct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
        {
            main_work.win_size_rate[(int)index] = (double)main_work.win_timer <= 0.0 ? 0.0f : main_work.win_timer / 8f;
            if ((double)main_work.win_size_rate[(int)index] < 0.0)
                main_work.win_size_rate[(int)index] = 0.0f;
        }
        if ((double)main_work.win_timer < 0.0)
        {
            main_work.flag |= 2048U;
            main_work.win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int)index] = 0.0f;
        }
        else
            --main_work.win_timer;
    }

    private static void dmOptSetChngFocusCrsrData(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        main_work.src_crsr_pos_y = main_work.top_crsr_pos_y;
        main_work.dst_crsr_pos_y = (float)(250.0 + (double)main_work.cur_slct_top * 220.0);
        main_work.flag |= 262144U;
    }

    private static int dmOptIsDataLoad(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.amFsIsComplete(main_work.arc_amb_fs[index]))
                return 0;
        }
        for (int index = 0; index < 5; ++index)
        {
            if (!AppMain.amFsIsComplete(main_work.arc_cmn_amb_fs[index]))
                return 0;
        }
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.amFsIsComplete(main_work.manual_arc_amb_fs[index]))
                return 0;
        }
        return 1;
    }

    private static int dmOptIsTexLoad(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.AoTexIsLoaded(main_work.tex[index]))
                return 0;
        }
        for (int index = 0; index < 5; ++index)
        {
            if (!AppMain.AoTexIsLoaded(main_work.cmn_tex[index]))
                return 0;
        }
        return !AppMain.GsFontIsBuilded() || !AppMain.dm_opt_is_pause_maingame && !AppMain.DmSndBgmPlayerIsSndSysBuild() ? 0 : 1;
    }

    private static int dmOptIsTexLoad2(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        return !AppMain.DmManualBuildCheck() ? 0 : 1;
    }

    private static int dmOptIsTexRelease(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.AoTexIsReleased(main_work.tex[index]))
                return 0;
        }
        for (int index = 0; index < 5; ++index)
        {
            if (!AppMain.AoTexIsReleased(main_work.cmn_tex[index]))
                return 0;
        }
        return !AppMain.DmManualFlushCheck() ? 0 : 1;
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

    private static void dmOptControlResetAct(AppMain.DMS_OPT_MAIN_WORK main_work)
    {
        AppMain.SResetLocalTable[][] sresetLocalTableArray = new AppMain.SResetLocalTable[3][]
        {
      new AppMain.SResetLocalTable[23]
      {
        new AppMain.SResetLocalTable(51, 32, 0),
        new AppMain.SResetLocalTable(52, 33, 0),
        new AppMain.SResetLocalTable(53, 34, 0),
        new AppMain.SResetLocalTable(54, 35, 0),
        new AppMain.SResetLocalTable(55, 45, 0),
        new AppMain.SResetLocalTable(56, 44, 0),
        new AppMain.SResetLocalTable(57, 36, 0),
        new AppMain.SResetLocalTable(58, 37, 0),
        new AppMain.SResetLocalTable(59, 38, 0),
        new AppMain.SResetLocalTable(60, 41, 0),
        new AppMain.SResetLocalTable(61, 42, 0),
        new AppMain.SResetLocalTable(62, 43, 0),
        new AppMain.SResetLocalTable(63, 39, 0),
        new AppMain.SResetLocalTable(64, 40, 0),
        new AppMain.SResetLocalTable(65, -1, -1),
        new AppMain.SResetLocalTable(66, 46, 0),
        new AppMain.SResetLocalTable(88, 11, 1),
        new AppMain.SResetLocalTable(89, 12, 1),
        new AppMain.SResetLocalTable(92, 15, 1),
        new AppMain.SResetLocalTable(93, 19, 1),
        new AppMain.SResetLocalTable(94, 18, 1),
        new AppMain.SResetLocalTable(95, 16, 1),
        new AppMain.SResetLocalTable(96, 17, 1)
      },
      new AppMain.SResetLocalTable[23]
      {
        new AppMain.SResetLocalTable(51, 56, 0),
        new AppMain.SResetLocalTable(52, 57, 0),
        new AppMain.SResetLocalTable(53, 58, 0),
        new AppMain.SResetLocalTable(54, 59, 0),
        new AppMain.SResetLocalTable(55, 69, 0),
        new AppMain.SResetLocalTable(56, -1, -1),
        new AppMain.SResetLocalTable(57, 60, 0),
        new AppMain.SResetLocalTable(58, 61, 0),
        new AppMain.SResetLocalTable(59, 62, 0),
        new AppMain.SResetLocalTable(60, 66, 0),
        new AppMain.SResetLocalTable(61, 67, 0),
        new AppMain.SResetLocalTable(62, 68, 0),
        new AppMain.SResetLocalTable(63, 63, 0),
        new AppMain.SResetLocalTable(64, 64, 0),
        new AppMain.SResetLocalTable(65, 65, 0),
        new AppMain.SResetLocalTable(66, -1, -1),
        new AppMain.SResetLocalTable(88, 32, 1),
        new AppMain.SResetLocalTable(89, 33, 1),
        new AppMain.SResetLocalTable(92, 28, 1),
        new AppMain.SResetLocalTable(93, -1, -1),
        new AppMain.SResetLocalTable(94, 29, 1),
        new AppMain.SResetLocalTable(95, 31, 1),
        new AppMain.SResetLocalTable(96, 30, 1)
      },
      new AppMain.SResetLocalTable[23]
      {
        new AppMain.SResetLocalTable(51, 56, 0),
        new AppMain.SResetLocalTable(52, 57, 0),
        new AppMain.SResetLocalTable(53, 58, 0),
        new AppMain.SResetLocalTable(54, 59, 0),
        new AppMain.SResetLocalTable(55, 69, 0),
        new AppMain.SResetLocalTable(56, -1, -1),
        new AppMain.SResetLocalTable(57, 60, 0),
        new AppMain.SResetLocalTable(58, 61, 0),
        new AppMain.SResetLocalTable(59, 62, 0),
        new AppMain.SResetLocalTable(60, 66, 0),
        new AppMain.SResetLocalTable(61, 67, 0),
        new AppMain.SResetLocalTable(62, 68, 0),
        new AppMain.SResetLocalTable(63, 63, 0),
        new AppMain.SResetLocalTable(64, 64, 0),
        new AppMain.SResetLocalTable(65, 65, 0),
        new AppMain.SResetLocalTable(66, -1, -1),
        new AppMain.SResetLocalTable(88, 32, 1),
        new AppMain.SResetLocalTable(89, 33, 1),
        new AppMain.SResetLocalTable(92, 28, 1),
        new AppMain.SResetLocalTable(93, -1, -1),
        new AppMain.SResetLocalTable(94, 29, 1),
        new AppMain.SResetLocalTable(95, 31, 1),
        new AppMain.SResetLocalTable(96, 30, 1)
      }
        };
        for (int index = 0; index < sresetLocalTableArray[0].Length; ++index)
        {
            if (main_work.act[sresetLocalTableArray[0][index].act_idx] != null)
            {
                AppMain.AoActDelete(main_work.act[sresetLocalTableArray[0][index].act_idx]);
                main_work.act[sresetLocalTableArray[0][index].act_idx] = (AppMain.AOS_ACTION)null;
            }
        }
        SOption.EControl.Type control = SOption.CreateInstance().GetControl();
        for (int index = 0; index < sresetLocalTableArray[(int)control].Length; ++index)
        {
            if (0 <= sresetLocalTableArray[(int)control][index].act_id)
                main_work.act[sresetLocalTableArray[(int)control][index].act_idx] = AppMain.AoActCreate(main_work.ama[sresetLocalTableArray[(int)control][index].ama_idx], (uint)sresetLocalTableArray[(int)control][index].act_id);
        }
        if (control != SOption.EControl.Type.Tilt)
            return;
        for (int index = 42; index != 69; ++index)
        {
            if (main_work.act[index] != null)
                AppMain.AoActSetFrame(main_work.act[index], 0.0f);
        }
        for (int index = 88; index != 97; ++index)
        {
            if (main_work.act[index] != null)
                AppMain.AoActSetFrame(main_work.act[index], 0.0f);
        }
    }
}