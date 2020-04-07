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
    private static readonly AppMain.DMS_STFRL_MGR dm_stfrl_mgr;
    private static AppMain.DMS_STFRL_MGR dm_stfrl_mgr_p;
    private static readonly AppMain.DMS_STFRL_FS_DATA_MGR dm_stfrl_fs_data_mgr;
    private static AppMain.DMS_STFRL_FS_DATA_MGR dm_stfrl_fs_data_mgr_p;
    private static readonly AppMain.DMS_STFRL_DATA_MGR dm_stfrl_data_mgr;
    private static AppMain.DMS_STFRL_DATA_MGR dm_stfrl_data_mgr_p;
    private static AppMain.AMS_AMB_HEADER dm_stfrl_font_amb;
    private static readonly AppMain.AOS_TEXTURE dm_stfrl_font_tex;
    private static AppMain.AMS_AMB_HEADER dm_stfrl_scr_amb;
    private static readonly AppMain.AOS_TEXTURE dm_stfrl_scr_tex;
    private static AppMain.A2S_AMA_HEADER dm_stfrl_end_cmn_ama;
    private static AppMain.AMS_AMB_HEADER dm_stfrl_end_cmn_amb;
    private static readonly AppMain.AOS_TEXTURE dm_stfrl_end_tex;
    private static AppMain.A2S_AMA_HEADER dm_stfrl_end_lng_ama;
    private static AppMain.AMS_AMB_HEADER dm_stfrl_end_lng_amb;
    private static readonly AppMain.AOS_TEXTURE dm_stfrl_end_jp_tex;
    private static readonly AppMain.A2S_AMA_HEADER[] dm_stfrl_cmn_ama;
    private static readonly AppMain.AMS_AMB_HEADER[] dm_stfrl_cmn_amb;
    private static readonly AppMain.AOS_TEXTURE[] dm_stfrl_cmn_tex;
    private static bool dm_stfrl_is_full_staffroll;
    private static bool dm_stfrl_is_pause_maingame;
    private static readonly AppMain.NNS_PRIM3D_PCT_ARRAY dmStaffRollStageScrDraw_DrawArray;

    private static void DmStaffRollBuildForGame()
    {
        AppMain.dm_stfrl_fs_data_mgr.Clear();
        AppMain.dm_stfrl_fs_data_mgr_p = AppMain.dm_stfrl_fs_data_mgr;
        int language = AppMain.GsEnvGetLanguage();
        AppMain.dm_stfrl_fs_data_mgr_p.arc_list_font_amb_fs = AppMain.GmGameDatGetGimmickData(958);
        AppMain.dm_stfrl_fs_data_mgr_p.arc_scr_amb_fs = AppMain.GmGameDatGetGimmickData(960);
        AppMain.dm_stfrl_fs_data_mgr_p.arc_end_amb_fs = AppMain.GmGameDatGetGimmickData(972);
        AppMain.dm_stfrl_fs_data_mgr_p.arc_end_jp_amb_fs = AppMain.GmGameDatGetGimmickData(AppMain.dm_stfrl_lng_amb_id_tbl[language]);
        AppMain.dm_stfrl_fs_data_mgr_p.arc_cmn_amb_fs[0] = AppMain.GmGameDatGetGimmickData(959);
        AppMain.dm_stfrl_fs_data_mgr_p.arc_cmn_amb_fs[1] = AppMain.GmGameDatGetGimmickData(AppMain.dm_stfrl_cmn_msg_lng_amb_id_tbl[language]);
    }

    private static void DmStaffRollBuild(AppMain.DMS_STFRL_DATA_MGR data_mgr)
    {
        int curEvtId = (int)AppMain.SyGetEvtInfo().cur_evt_id;
        AppMain.dm_stfrl_data_mgr.Clear();
        AppMain.dm_stfrl_data_mgr_p = AppMain.dm_stfrl_data_mgr;
        AppMain.dm_stfrl_data_mgr_p = data_mgr;
        AppMain.dm_stfrl_font_amb = data_mgr.arc_font_amb;
        AppMain.AoTexBuild(AppMain.dm_stfrl_font_tex, AppMain.dm_stfrl_font_amb);
        AppMain.AoTexLoad(AppMain.dm_stfrl_font_tex);
        AppMain.dm_stfrl_scr_amb = data_mgr.arc_scr_amb;
        AppMain.AoTexBuild(AppMain.dm_stfrl_scr_tex, AppMain.dm_stfrl_scr_amb);
        AppMain.AoTexLoad(AppMain.dm_stfrl_scr_tex);
        AppMain.dm_stfrl_end_cmn_ama = AppMain.readAMAFile(AppMain.amBindGet(data_mgr.arc_end_amb, 0));
        string sPath;
        AppMain.dm_stfrl_end_cmn_amb = AppMain.readAMBFile(AppMain.amBindGet(data_mgr.arc_end_amb, 1, out sPath));
        AppMain.dm_stfrl_end_cmn_amb.dir = sPath;
        AppMain.dm_stfrl_end_lng_ama = AppMain.readAMAFile(AppMain.amBindGet(data_mgr.arc_end_jp_amb, 0));
        AppMain.dm_stfrl_end_lng_amb = AppMain.readAMBFile(AppMain.amBindGet(data_mgr.arc_end_jp_amb, 1, out sPath));
        AppMain.dm_stfrl_end_lng_amb.dir = sPath;
        AppMain.AoTexBuild(AppMain.dm_stfrl_end_tex, AppMain.dm_stfrl_end_cmn_amb);
        AppMain.AoTexLoad(AppMain.dm_stfrl_end_tex);
        AppMain.AoTexBuild(AppMain.dm_stfrl_end_jp_tex, AppMain.dm_stfrl_end_lng_amb);
        AppMain.AoTexLoad(AppMain.dm_stfrl_end_jp_tex);
        for (int index = 0; index < 2; ++index)
        {
            AppMain.dm_stfrl_cmn_ama[index] = AppMain.readAMAFile(AppMain.amBindGet(data_mgr.arc_cmn_amb[index], 0));
            AppMain.dm_stfrl_cmn_amb[index] = AppMain.readAMBFile(AppMain.amBindGet(data_mgr.arc_cmn_amb[index], 1, out sPath));
            AppMain.dm_stfrl_cmn_amb[index].dir = sPath;
            AppMain.AoTexBuild(AppMain.dm_stfrl_cmn_tex[index], AppMain.dm_stfrl_cmn_amb[index]);
            AppMain.AoTexLoad(AppMain.dm_stfrl_cmn_tex[index]);
        }
    }

    private static bool DmStaffRollBuildCheck()
    {
        return AppMain.dmStaffRollIsTexLoad() != 0;
    }

    private static void DmStaffRollFlush()
    {
        AppMain.AoTexRelease(AppMain.dm_stfrl_font_tex);
        if (!AppMain.dm_stfrl_is_full_staffroll)
            return;
        AppMain.AoTexRelease(AppMain.dm_stfrl_scr_tex);
        AppMain.AoTexRelease(AppMain.dm_stfrl_end_tex);
        AppMain.AoTexRelease(AppMain.dm_stfrl_end_jp_tex);
        for (int index = 0; index < 2; ++index)
            AppMain.AoTexRelease(AppMain.dm_stfrl_cmn_tex[index]);
    }

    private static bool DmStaffRollFlushCheck()
    {
        return AppMain.dmStaffRollIsTexRelease() != 0;
    }

    private static void DmStaffRollStart(object arg)
    {
        AppMain.UNREFERENCED_PARAMETER(arg);
        AppMain.dm_stfrl_is_full_staffroll = AppMain.SyGetEvtInfo().old_evt_id == (short)9;
        if (AppMain.dm_stfrl_mgr_p == null)
            AppMain.dm_stfrl_mgr_p = AppMain.dm_stfrl_mgr;
        switch (AppMain.SyGetEvtInfo().cur_evt_id)
        {
            case 6:
            case 11:
                AppMain.dm_stfrl_is_pause_maingame = true;
                break;
            default:
                AppMain.dm_stfrl_is_pause_maingame = false;
                break;
        }
        AppMain.dmStaffRollInit();
    }

    private static bool DmStaffRollIsExit()
    {
        return AppMain.dm_stfrl_mgr_p == null || AppMain.dm_stfrl_mgr_p.tcb == null;
    }

    private static void DmStaffRollExit()
    {
        if (AppMain.dm_stfrl_mgr_p.tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.dm_stfrl_mgr_p.tcb);
        AppMain.dm_stfrl_mgr_p.tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void dmStaffRollInit()
    {
        AppMain.dm_stfrl_mgr_p.tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.dmStaffRollProcMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.dmStaffRollDest), 0U, (ushort)short.MaxValue, 12288U, 10, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_STFRL_MAIN_WORK()), "STAFFROLL_MAIN");
        AppMain.DMS_STFRL_MAIN_WORK work = (AppMain.DMS_STFRL_MAIN_WORK)AppMain.dm_stfrl_mgr_p.tcb.work;
        AppMain.AoActSysSetDrawStateEnable(true);
        AppMain.AoActSysSetDrawState(AppMain.AoActSysGetDrawState());
        AppMain.dmStaffRollSetInitData(work);
        work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcInit);
    }

    private static void dmStaffRollSetInitData(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.disp_mode = 0U;
        main_work.disp_frm_time = 1f;
        main_work.question_act_alpha.r = byte.MaxValue;
        main_work.question_act_alpha.g = byte.MaxValue;
        main_work.question_act_alpha.b = byte.MaxValue;
        main_work.question_act_alpha.a = (byte)0;
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 32) != 0)
            main_work.is_eme_comp = true;
        else
            main_work.is_eme_comp = false;
    }

    private static void dmStaffRollProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_STFRL_MAIN_WORK work = (AppMain.DMS_STFRL_MAIN_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
        {
            AppMain.DmStaffRollExit();
            if (!AppMain.dm_stfrl_is_full_staffroll)
                return;
            AppMain.dmStaffRollSetNextEvt(work);
        }
        else
        {
            if (((int)work.flag & int.MinValue) != 0 && !AppMain.AoAccountIsCurrentEnable())
            {
                work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcFadeOut);
                work.flag &= (uint)int.MaxValue;
                work.flag |= 1073741824U;
                if (AppMain.dm_stfrl_is_pause_maingame)
                    AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 1U, 1U, 80f, true);
                else
                    AppMain.IzFadeInitEasy(1U, 1U, 64f);
                if (work.bgm_scb != null)
                    AppMain.GsSoundStopBgm(work.bgm_scb, 79);
                work.flag &= 4294967291U;
                work.flag &= 4294967293U;
                work.proc_input = (AppMain.DMS_STFRL_MAIN_WORK._proc_input_)null;
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

    private static void dmStaffRollDest(AppMain.MTS_TASK_TCB tcb)
    {
        if (!AppMain.dm_stfrl_is_full_staffroll)
            return;
        AppMain.ObjDrawESEffectSystemExit();
        AppMain.ObjCameraExit();
        AppMain.GmMainExitForStaffroll();
    }

    private static void dmStaffRollSetNextEvt(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (AppMain.dm_stfrl_is_full_staffroll)
        {
            short evt_case = 0;
            if (((int)main_work.flag & 1073741824) != 0)
                evt_case = (short)1;
            AppMain.SyDecideEvtCase(evt_case);
        }
        else
        {
            short req_id = AppMain.SyGetEvtInfo().old_evt_id;
            if (((int)main_work.flag & 1073741824) != 0)
                req_id = (short)1;
            AppMain.SyDecideEvt(req_id);
        }
    }

    private static void dmStaffRollProcInit(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (AppMain.dm_stfrl_is_full_staffroll)
        {
            main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcLoadData);
            AppMain.DmStaffRollBuildForGame();
            main_work.arc_list_font_amb = AppMain.readAMBFile((object)AppMain.dm_stfrl_fs_data_mgr_p.arc_list_font_amb_fs);
            main_work.arc_scr_amb_fs = AppMain.readAMBFile((object)AppMain.dm_stfrl_fs_data_mgr_p.arc_scr_amb_fs);
            main_work.arc_end_amb_fs = AppMain.readAMBFile((object)AppMain.dm_stfrl_fs_data_mgr_p.arc_end_amb_fs);
            main_work.arc_end_jp_amb_fs = AppMain.readAMBFile((object)AppMain.dm_stfrl_fs_data_mgr_p.arc_end_jp_amb_fs);
            main_work.arc_cmn_amb_fs[0] = AppMain.readAMBFile((object)AppMain.dm_stfrl_fs_data_mgr_p.arc_cmn_amb_fs[0]);
            main_work.arc_cmn_amb_fs[1] = AppMain.readAMBFile((object)AppMain.dm_stfrl_fs_data_mgr_p.arc_cmn_amb_fs[1]);
            main_work.staff_list_fs = (object)AppMain.amFsRead("DEMO/STFRL/STAFF_LIST.YSD");
        }
        else
        {
            main_work.arc_list_font_amb_fs = AppMain.amFsReadBackground("DEMO/STFRL/D_STFRL_FONT.AMB");
            main_work.staff_list_fs = (object)AppMain.amFsRead("DEMO/STFRL/STAFF_LIST.YSD");
            main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcLoadData);
        }
        AppMain.GsMainSysSetSleepFlag(false);
    }

    private static void dmStaffRollProcLoadData(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (AppMain.dmStaffRollIsDataLoad(main_work) == 0)
            return;
        if (AppMain.dm_stfrl_is_full_staffroll)
        {
            AppMain.dmStaffRollDataClearRequestFull(main_work);
            AppMain.DmStaffRollBuild(main_work.arc_data);
        }
        else
        {
            AppMain.dmStaffRollDataClearRequestEasy(main_work);
            AppMain.dmStaffRollDataBuildEasy(main_work);
        }
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcDataBuild);
    }

    private static void dmStaffRollProcDataBuild(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (!AppMain.DmStaffRollBuildCheck())
            return;
        if (AppMain.dm_stfrl_is_full_staffroll)
        {
            AppMain.dmStaffRollSetObjSystemData(main_work);
            main_work.ring_work[0] = AppMain.DmStfrlMdlCtrlSetRingObj(0, 0U);
            main_work.ring_work[1] = AppMain.DmStfrlMdlCtrlSetRingObj(20, 3U);
            main_work.ring_work[2] = AppMain.DmStfrlMdlCtrlSetRingObj(40, 6U);
            main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcCreateAct);
        }
        else
        {
            main_work.flag |= 2147483648U;
            main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcFadeIn);
            main_work.proc_draw = new AppMain.DMS_STFRL_MAIN_WORK._proc_draw_(AppMain.dmStaffRollProcActDraw);
            AppMain.IzFadeInitEasy(0U, 0U, 64f);
        }
        if (!AppMain.AoYsdFileIsYsdFile((object)AppMain.dm_stfrl_data_mgr_p.stf_list_ysd))
            AppMain.MTM_ASSERT(0);
        main_work.disp_list_page_num = AppMain.AoYsdFileGetPageNum(AppMain.dm_stfrl_data_mgr_p.stf_list_ysd);
        main_work.bgm_scb = AppMain.GsSoundAssignScb(0);
        main_work.bgm_scb.flag |= 2147483648U;
        main_work.se_handle = AppMain.GsSoundAllocSeHandle();
        if (AppMain.GsSoundIsRunning())
            return;
        AppMain.GsSoundBegin((ushort)4096, 1U, 3);
        main_work.flag |= 2048U;
    }

    private static void dmStaffRollProcCreateAct(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 11U; ++index)
        {
            AppMain.A2S_AMA_HEADER ama;
            AppMain.AOS_TEXTURE tex;
            if (index >= 8U)
            {
                ama = AppMain.dm_stfrl_end_lng_ama;
                tex = AppMain.dm_stfrl_end_jp_tex;
            }
            else
            {
                ama = AppMain.dm_stfrl_end_cmn_ama;
                tex = AppMain.dm_stfrl_end_tex;
            }
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(tex));
            main_work.act[(int)index] = AppMain.AoActCreate(ama, AppMain.g_dm_act_id_tbl_staff[(int)index]);
        }
        main_work.flag |= 2147483648U;
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcFadeIn);
        main_work.proc_draw = new AppMain.DMS_STFRL_MAIN_WORK._proc_draw_(AppMain.dmStaffRollProcActDraw);
        AppMain.IzFadeInitEasy(0U, 0U, 64f);
    }

    private static void dmStaffRollProcFadeIn(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcNowStaffRoll);
        AppMain.dmStaffRollSetFadePageInfoEfctData(main_work);
        main_work.timer = 32f;
        if (AppMain.dm_stfrl_is_full_staffroll)
            AppMain.GsSoundPlayBgm(main_work.bgm_scb, "snd_sng_ending", 0);
        else
            AppMain.GsSoundPlayBgm(main_work.bgm_scb, "snd_sng_z1a1", 0);
        main_work.proc_input = new AppMain.DMS_STFRL_MAIN_WORK._proc_input_(AppMain.dmStaffRollInputProcStaffRollMain);
    }

    private static void dmStaffRollProcDispWaitStaffList(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = !AppMain.dm_stfrl_is_full_staffroll ? new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcFadeOut) : new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcModeFadeOut);
            if (AppMain.dm_stfrl_is_pause_maingame)
            {
                AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 1U, 80f, true);
                AppMain.GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            else
            {
                AppMain.IzFadeInitEasy(0U, 1U, 80f);
                AppMain.GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            main_work.flag &= 4294967291U;
        }
        else
        {
            if ((double)main_work.timer <= 0.0)
            {
                main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcNowStaffRoll);
                main_work.timer = 0.0f;
            }
            main_work.timer -= main_work.disp_frm_time;
        }
    }

    private static void dmStaffRollProcNowStaffRoll(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = !AppMain.dm_stfrl_is_full_staffroll ? new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcFadeOut) : new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcModeFadeOut);
            if (AppMain.dm_stfrl_is_pause_maingame)
            {
                AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 1U, 80f, true);
                AppMain.GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            else
            {
                AppMain.IzFadeInitEasy(0U, 1U, 80f);
                AppMain.GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            main_work.flag &= 4294967291U;
        }
        else if ((double)main_work.fade_timer <= 0.0)
        {
            main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcSetChangeData);
            main_work.fade_timer = 0.0f;
        }
        else
        {
            AppMain.dmStaffRollSetEfctChngAlphaListData(main_work);
            main_work.fade_timer -= main_work.disp_frm_time;
        }
    }

    private static void dmStaffRollProcSetChangeData(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (((int)main_work.flag & 4) != 0)
        {
            main_work.proc_update = !AppMain.dm_stfrl_is_full_staffroll ? new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcFadeOut) : new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcModeFadeOut);
            if (AppMain.dm_stfrl_is_pause_maingame)
            {
                AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 1U, 80f, true);
                AppMain.GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            else
            {
                AppMain.IzFadeInitEasy(0U, 1U, 80f);
                AppMain.GsSoundStopBgm(main_work.bgm_scb, 80);
            }
            main_work.flag &= 4294967291U;
        }
        else
        {
            ++main_work.cur_disp_list_page;
            if (main_work.cur_disp_list_page > main_work.disp_list_page_num - 1U)
            {
                main_work.cur_disp_list_page = main_work.disp_list_page_num - 1U;
                main_work.proc_update = !AppMain.dm_stfrl_is_full_staffroll ? new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcFadeOut) : new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcModeFadeOut);
                if (AppMain.dm_stfrl_is_pause_maingame)
                {
                    AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 1U, 80f, true);
                    AppMain.GsSoundStopBgm(main_work.bgm_scb, 80);
                }
                else
                {
                    AppMain.IzFadeInitEasy(0U, 1U, 80f);
                    AppMain.GsSoundStopBgm(main_work.bgm_scb, 80);
                }
            }
            else
            {
                AppMain.dmStaffRollSetFadePageInfoEfctData(main_work);
                main_work.timer = 32f;
                main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcDispWaitStaffList);
                AppMain.UNREFERENCED_PARAMETER((object)main_work);
            }
        }
    }

    private static void dmStaffRollProcModeFadeOut(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcModeFadeIn);
        main_work.disp_mode = 1U;
        if (AppMain.dm_stfrl_is_pause_maingame)
            AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 0U, 80f, true);
        else
            AppMain.IzFadeInitEasy(0U, 0U, 80f);
        AppMain.dmStaffRollSetupEndModel(main_work);
    }

    private static void dmStaffRollProcModeFadeIn(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcEndModeIdle);
        main_work.proc_input = new AppMain.DMS_STFRL_MAIN_WORK._proc_input_(AppMain.dmStaffRollInputProcWin);
        main_work.timer = 32f;
        main_work.flag |= 128U;
    }

    private static void dmStaffRollProcEndModeIdle(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 256) != 0 && main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (((int)main_work.flag & 4) != 0)
        {
            if (AppMain.dm_stfrl_is_full_staffroll)
            {
                main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcWinModeFadeOut);
                main_work.proc_input = (AppMain.DMS_STFRL_MAIN_WORK._proc_input_)null;
            }
            else
            {
                main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcFadeOut);
                main_work.proc_input = (AppMain.DMS_STFRL_MAIN_WORK._proc_input_)null;
            }
            main_work.flag &= 4294967291U;
            if (AppMain.dm_stfrl_is_pause_maingame)
                AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 1U, 80f, true);
            else
                AppMain.IzFadeInitEasy(0U, 1U, 80f);
        }
        else
        {
            if (main_work.is_eme_comp)
            {
                if (((int)main_work.body_work.flag & 8) != 0)
                {
                    main_work.question_act_alpha.a += (byte)8;
                    if (main_work.question_act_alpha.a > (byte)247)
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
                AppMain.GsSoundPlaySe("Metal_Sonic", main_work.se_handle);
            if (main_work.sonic_work != null && main_work.body_work != null && ((int)main_work.sonic_work.flag & 1) != 0)
            {
                main_work.body_work.flag |= 2U;
                ushort num = 1;
                main_work.sonic_work.flag = (ushort)(main_work.sonic_work.flag & ~num);
            }
            if (((int)main_work.flag & 128) == 0)
                return;
            main_work.timer -= main_work.disp_frm_time;
            if ((double)main_work.timer > 0.0)
                return;
            main_work.flag &= 4294967167U;
            main_work.flag |= 256U;
            main_work.timer = 0.0f;
        }
    }

    private static void dmStaffRollProcWinModeFadeOut(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.dmStaffRollNodispEndModel(main_work);
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcWinModeFadeIn);
        main_work.disp_mode = 2U;
        AppMain.IzFadeInitEasy(0U, 0U, 80f);
        AppMain.GsSoundStopBgm(main_work.bgm_scb, 0);
    }

    private static void dmStaffRollProcWinModeFadeIn(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcWindowNodispIdle);
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 32) != 0)
            return;
        main_work.announce_flag |= 1U;
    }

    private static void dmStaffRollProcWindowNodispIdle(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.announce_flag != 0U)
        {
            main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcWindowOpenEfct);
            main_work.proc_input = (AppMain.DMS_STFRL_MAIN_WORK._proc_input_)null;
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
            AppMain.GsSoundPlaySe("Window", main_work.se_handle);
        }
        else
            main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcTrophyCheck);
    }

    private static void dmStaffRollProcWindowOpenEfct(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 512) != 0)
        {
            main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcWindowAnnounceIdle);
            main_work.proc_input = new AppMain.DMS_STFRL_MAIN_WORK._proc_input_(AppMain.dmStaffRollInputProcWin);
            main_work.flag |= 64U;
            main_work.flag &= 4294966783U;
        }
        else
            AppMain.dmStaffRollSetWinOpenEfct(main_work);
    }

    private static void dmStaffRollProcWindowAnnounceIdle(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        if (main_work.win_mode != 0U || ((int)main_work.flag & 4) == 0)
            return;
        main_work.proc_input = (AppMain.DMS_STFRL_MAIN_WORK._proc_input_)null;
        main_work.win_timer = 8f;
        main_work.flag &= 4294967231U;
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcWindowCloseEfct);
        AppMain.GsSoundPlaySe("Ok", main_work.se_handle);
        main_work.flag &= 4294967291U;
        main_work.flag &= 4294967293U;
    }

    private static void dmStaffRollProcWindowCloseEfct(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (((int)main_work.flag & 512) != 0)
        {
            main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcWindowNodispIdle);
            main_work.announce_flag &= (uint)~(1 << (int)main_work.win_mode);
            main_work.flag &= 4294966271U;
            main_work.flag &= 4294966783U;
        }
        AppMain.dmStaffRollSetWinCloseEfct(main_work);
    }

    private static void dmStaffRollProcTrophyCheck(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcFadeOut);
        if (AppMain.dm_stfrl_is_full_staffroll)
            AppMain.HgTrophyTryAcquisition(3);
        AppMain.IzFadeInitEasy(0U, 1U, 80f);
    }

    private static void dmStaffRollProcFadeOut(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        if (((int)main_work.flag & 2048) != 0)
        {
            AppMain.GsSoundHalt();
            AppMain.GsSoundEnd();
        }
        if (main_work.bgm_scb != null)
        {
            AppMain.GsSoundStopBgm(main_work.bgm_scb, 0);
            AppMain.GsSoundResignScb(main_work.bgm_scb);
            main_work.bgm_scb = (AppMain.GSS_SND_SCB)null;
        }
        if (main_work.se_handle != null)
        {
            AppMain.GsSoundFreeSeHandle(main_work.se_handle);
            main_work.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcStopDraw);
        main_work.proc_draw = (AppMain.DMS_STFRL_MAIN_WORK._proc_draw_)null;
    }

    private static void dmStaffRollProcStopDraw(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        for (int index = 0; index < 11; ++index)
        {
            if (main_work.act[index] != null)
            {
                AppMain.AoActDelete(main_work.act[index]);
                main_work.act[index] = (AppMain.AOS_ACTION)null;
            }
        }
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcDataRelease);
    }

    private static void dmStaffRollProcDataRelease(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        AppMain.DmStaffRollFlush();
        if (AppMain.dm_stfrl_is_full_staffroll)
        {
            AppMain.ObjObjectClearAllObject();
            AppMain.ObjPreExit();
        }
        main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcFinish);
    }

    private static void dmStaffRollProcFinish(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (AppMain.DmStaffRollFlushCheck())
        {
            for (int index = 0; index < 11; ++index)
            {
                if (main_work.act[index] != null)
                {
                    AppMain.AoActDelete(main_work.act[index]);
                    main_work.act[index] = (AppMain.AOS_ACTION)null;
                }
            }
            if (AppMain.dm_stfrl_is_full_staffroll)
            {
                if (AppMain.dm_stfrl_data_mgr_p.arc_font_amb != null)
                    AppMain.dm_stfrl_data_mgr_p.arc_font_amb = (AppMain.AMS_AMB_HEADER)null;
                if (AppMain.dm_stfrl_data_mgr_p.arc_scr_amb != null)
                    AppMain.dm_stfrl_data_mgr_p.arc_scr_amb = (AppMain.AMS_AMB_HEADER)null;
                if (AppMain.dm_stfrl_data_mgr_p.arc_end_amb != null)
                    AppMain.dm_stfrl_data_mgr_p.arc_end_amb = (AppMain.AMS_AMB_HEADER)null;
                if (AppMain.dm_stfrl_data_mgr_p.arc_end_jp_amb != null)
                    AppMain.dm_stfrl_data_mgr_p.arc_end_jp_amb = (AppMain.AMS_AMB_HEADER)null;
                for (int index = 0; index < 2; ++index)
                {
                    if (AppMain.dm_stfrl_data_mgr_p.arc_cmn_amb[index] != null)
                        AppMain.dm_stfrl_data_mgr_p.arc_cmn_amb[index] = (AppMain.AMS_AMB_HEADER)null;
                }
            }
            else if (AppMain.dm_stfrl_font_amb != null)
                AppMain.dm_stfrl_font_amb = (AppMain.AMS_AMB_HEADER)null;
            if (AppMain.dm_stfrl_data_mgr_p.stf_list_ysd != null)
                AppMain.dm_stfrl_data_mgr_p.stf_list_ysd = (AppMain.YSDS_HEADER)null;
            if (main_work.page_line_type != null)
                main_work.page_line_type = (uint[])null;
            if (AppMain.dm_stfrl_is_full_staffroll)
            {
                main_work.proc_update = new AppMain.DMS_STFRL_MAIN_WORK._proc_update_(AppMain.dmStaffRollProcSaveEndCheck);
                AppMain.DmSaveMenuStart(true, false);
            }
            else
            {
                main_work.flag |= 1U;
                main_work.proc_update = (AppMain.DMS_STFRL_MAIN_WORK._proc_update_)null;
            }
        }
        AppMain.GsMainSysSetSleepFlag(true);
    }

    private static void dmStaffRollProcSaveEndCheck(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (!AppMain.DmSaveIsExit())
            return;
        main_work.flag |= 1U;
        main_work.proc_update = (AppMain.DMS_STFRL_MAIN_WORK._proc_update_)null;
        if (((int)main_work.flag & 2048) != 0)
            AppMain.GsSoundReset();
        AppMain.GsFontRelease();
    }

    private static void dmStaffRollInputProcStaffRollMain(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (!AppMain.amTpIsTouchPush(0) && !AppMain.isBackKeyPressed())
            return;
        main_work.flag |= 4U;
        AppMain.setBackKeyRequest(false);
    }

    private static void dmStaffRollInputProcWin(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (!AppMain.amTpIsTouchPush(0) && !AppMain.isBackKeyPressed())
            return;
        main_work.flag |= 4U;
        AppMain.setBackKeyRequest(false);
    }

    private static void dmStaffRollProcActDraw(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        AppMain.dmStaffRollBGDraw(main_work);
        if (main_work.disp_mode == 0U)
        {
            AppMain.dmStaffRollStaffListDraw(main_work);
            if (AppMain.dm_stfrl_is_full_staffroll)
                AppMain.dmStaffRollStageScrDraw(main_work);
        }
        else if (main_work.disp_mode == 1U && AppMain.dm_stfrl_is_full_staffroll)
            AppMain.dmStaffRollEndActDraw(main_work);
        if (((int)main_work.flag & 1024) == 0 || !AppMain.dm_stfrl_is_full_staffroll)
            return;
        AppMain.dmStaffRollWinActDraw(main_work);
    }

    private static void dmStaffRollBGDraw(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)main_work);
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.mtx = (AppMain.NNS_MATRIX)null;
        setParam.vtxPC3D = AppMain.amDrawAlloc_NNS_PRIM3D_PC(4);
        AppMain.NNS_PRIM3D_PC[] vtxPc3D = setParam.vtxPC3D;
        vtxPc3D[0].Pos.x = vtxPc3D[1].Pos.x = -160f;
        vtxPc3D[2].Pos.x = vtxPc3D[3].Pos.x = vtxPc3D[0].Pos.x + 1280f;
        vtxPc3D[0].Pos.y = vtxPc3D[2].Pos.y = 0.0f;
        vtxPc3D[1].Pos.y = vtxPc3D[3].Pos.y = 720f;
        vtxPc3D[0].Pos.z = vtxPc3D[1].Pos.z = vtxPc3D[2].Pos.z = vtxPc3D[3].Pos.z = -2f;
        vtxPc3D[0].Col = vtxPc3D[1].Col = vtxPc3D[2].Col = vtxPc3D[3].Col = AppMain.AMD_RGBA8888((byte)0, (byte)0, (byte)0, byte.MaxValue);
        setParam.format3D = 2;
        setParam.type = 1;
        setParam.count = 4;
        setParam.ablend = 1;
        AppMain.amDrawGetPrimBlendParam(0, setParam);
        setParam.aTest = (short)0;
        setParam.zMask = (short)1;
        setParam.zTest = (short)0;
        AppMain.AoActDrawCorWide(vtxPc3D, 0, 4U, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_NONE);
        AppMain.amDrawPrimitive3D(40U, setParam);
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.dmStaffRollTaskBgDraw), (ushort)2048, 0U);
        AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    private static void dmStaffRollTaskBgDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.UNREFERENCED_PARAMETER((object)tcb);
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(40U);
        AppMain.amDrawEndScene();
    }

    private static void dmStaffRollStageScrDraw(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.mtx = (AppMain.NNS_MATRIX)null;
        setParam.vtxPCT3D = AppMain.dmStaffRollStageScrDraw_DrawArray;
        AppMain.NNS_PRIM3D_PCT[] buffer = setParam.vtxPCT3D.buffer;
        int offset = setParam.vtxPCT3D.offset;
        buffer[offset].Pos.x = buffer[offset + 1].Pos.x = 160f;
        buffer[offset + 2].Pos.x = buffer[offset + 3].Pos.x = 672f;
        buffer[offset].Pos.y = buffer[offset + 2].Pos.y = 216f;
        buffer[offset + 1].Pos.y = buffer[offset + 3].Pos.y = 504f;
        buffer[offset].Pos.z = buffer[offset + 1].Pos.z = buffer[offset + 2].Pos.z = buffer[offset + 3].Pos.z = -2f;
        buffer[offset].Col = buffer[offset + 1].Col = buffer[2].Col = buffer[3].Col = AppMain.AMD_RGBA8888(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)main_work.cur_page_scr_alpha_data);
        buffer[offset].Tex.u = buffer[offset + 1].Tex.u = 0.0f;
        buffer[offset + 2].Tex.u = buffer[offset + 3].Tex.u = 1f;
        buffer[offset].Tex.v = buffer[offset + 2].Tex.v = 0.0f;
        buffer[offset + 1].Tex.v = buffer[offset + 3].Tex.v = 9f / 16f;
        setParam.format3D = 4;
        setParam.type = 1;
        setParam.count = 4;
        setParam.texlist = AppMain.AoTexGetTexList(AppMain.dm_stfrl_scr_tex);
        setParam.texId = (int)main_work.cur_disp_image;
        setParam.ablend = 1;
        setParam.zOffset = -1f;
        setParam.aTest = (short)0;
        setParam.zMask = (short)1;
        setParam.zTest = (short)0;
        AppMain.AoActDrawCorWide(AppMain.dmStaffRollStageScrDraw_DrawArray, 0, 4U, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_LEFT);
        AppMain.amDrawPrimitive3D(30U, setParam);
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.dmStaffRollStageScrTaskDraw), (ushort)2304, (object)null);
        AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    private static void dmStaffRollStageScrTaskDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.UNREFERENCED_PARAMETER((object)tcb);
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(30U);
        AppMain.amDrawEndScene();
    }

    private static void dmStaffRollStaffListDraw(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        uint num1 = 0;
        uint lineNum = AppMain.AoYsdFileGetLineNum(AppMain.dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page);
        main_work.page_line_type = new uint[(int)lineNum];
        for (uint line_no = 0; line_no < lineNum; ++line_no)
        {
            main_work.page_line_type[(int)line_no] = AppMain.AoYsdFileGetLineId(AppMain.dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page, line_no);
            num1 += AppMain.dm_stfrl_list_id_font_height_tbl[(int)main_work.page_line_type[(int)line_no]];
        }
        uint num2 = (uint)(360.0 - (double)(num1 / 2U));
        for (uint cur_line = 0; cur_line < lineNum; ++cur_line)
        {
            AppMain.dmStaffRollStaffListOneLineDraw(main_work, (uint)((double)num2 * 0.899999976158142 * 0.899999976158142 + 5.0 + 32.0), cur_line);
            num2 += AppMain.dm_stfrl_list_id_font_height_tbl[(int)main_work.page_line_type[(int)cur_line]];
        }
        main_work.page_line_type = (uint[])null;
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.dmStaffRollTaskDraw), (ushort)2560, 0U);
    }

    public static void dmStaffRollStaffListOneLineDraw(
      AppMain.DMS_STFRL_MAIN_WORK main_work,
      uint disp_pos_y,
      uint cur_line)
    {
        int index1 = 0;
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        uint num1 = 0;
        uint[] numArray = new uint[3];
        float num2 = !AppMain.dm_stfrl_is_full_staffroll || (int)main_work.cur_disp_list_page == (int)main_work.disp_list_page_num - 1 ? 0.0f : ((1 & (int)AppMain.AoYsdFileGetPageOption(AppMain.dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page)) == 0 ? 112f : 0.0f);
        uint lineId = AppMain.AoYsdFileGetLineId(AppMain.dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page, cur_line);
        if (lineId >= 4U && lineId <= 6U)
        {
            uint num3 = AppMain.dm_stfrl_list_logo_width_tbl[(int)lineId];
            uint num4 = (uint)(480.0 - (double)(num3 / 2U) + (double)num2);
            for (int index2 = 0; index2 < 3; ++index2)
                numArray[index2] = (uint)byte.MaxValue;
            int num5;
            switch (lineId)
            {
                case 5:
                    num5 = 2;
                    break;
                case 6:
                    num5 = AppMain.GsEnvGetRegion() != AppMain.GSE_REGION.GSD_REGION_JP ? 4 : 3;
                    break;
                default:
                    num5 = (int)lineId - 4 + 1;
                    break;
            }
            setParam.mtx = (AppMain.NNS_MATRIX)null;
            setParam.vtxPCT3D = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(4);
            AppMain.NNS_PRIM3D_PCT[] buffer = setParam.vtxPCT3D.buffer;
            int offset = setParam.vtxPCT3D.offset;
            buffer[offset].Pos.x = buffer[offset + 1].Pos.x = (float)num4;
            buffer[offset + 2].Pos.x = buffer[offset + 3].Pos.x = (float)(num4 + num3);
            buffer[offset].Pos.y = buffer[offset + 2].Pos.y = (float)disp_pos_y;
            buffer[offset + 1].Pos.y = buffer[offset + 3].Pos.y = (float)(disp_pos_y + AppMain.dm_stfrl_list_id_font_height_tbl[(int)lineId]);
            buffer[offset].Pos.z = buffer[offset + 1].Pos.z = buffer[offset + 2].Pos.z = buffer[offset + 3].Pos.z = -2f;
            buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 3].Col = AppMain.AMD_RGBA8888((byte)numArray[0], (byte)numArray[1], (byte)numArray[2], (byte)main_work.cur_page_list_alpha_data);
            buffer[offset].Tex.u = buffer[offset + 1].Tex.u = 0.0f;
            buffer[offset + 2].Tex.u = buffer[offset + 3].Tex.u = 1f;
            buffer[offset].Tex.v = buffer[offset + 2].Tex.v = 0.0f;
            buffer[offset + 1].Tex.v = buffer[offset + 3].Tex.v = (float)AppMain.dm_stfrl_list_id_font_height_tbl[(int)lineId] / 128f;
            setParam.format3D = 4;
            setParam.type = 1;
            setParam.count = 4;
            setParam.texlist = AppMain.AoTexGetTexList(AppMain.dm_stfrl_font_tex);
            setParam.texId = num5;
            setParam.ablend = 1;
            setParam.aTest = (short)0;
            setParam.zMask = (short)1;
            setParam.zTest = (short)0;
            AppMain.AoActDrawCorWide(setParam.vtxPCT3D, 0, 4U, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
            AppMain.amDrawPrimitive3D(20U, setParam);
            AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
        }
        else
        {
            int num3 = 0;
            float num4 = 32f * AppMain.dm_stfrl_list_id_font_size_tbl[(int)lineId];
            for (int index2 = 0; index2 < 3; ++index2)
                numArray[index2] = AppMain.dm_stfrl_list_id_font_color_tbl[(int)lineId][index2];
            string lineString = AppMain.AoYsdFileGetLineString(AppMain.dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page, cur_line);
            int index3 = 0;
            for (int length = lineString.Length; index3 < length; ++index3)
            {
                uint num5 = (uint)lineString[index3];
                float num6 = (float)AppMain.dm_stfrl_font_width_length_tbl[(int)num5] * AppMain.dm_stfrl_list_id_font_size_tbl[(int)lineId];
                num1 += (uint)num6;
                ++num3;
            }
            uint num7 = (uint)(480.0 - (double)(num1 / 2U) + (double)num2);
            AppMain.NNS_PRIM3D_PCT_ARRAY nnsPriM3DPctArray = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(6 * num3);
            for (int length = lineString.Length; index1 < length; ++index1)
            {
                setParam.Clear();
                byte num5 = (byte)lineString[index1];
                byte num6 = (byte)AppMain.dm_stfrl_list_font_id_tbl[(int)num5];
                byte num8 = num6 <= (byte)0 ? (byte)0 : (byte)((uint)num6 % 16U);
                byte num9 = num6 <= (byte)0 ? (byte)0 : (byte)((uint)num6 / 16U);
                setParam.mtx = (AppMain.NNS_MATRIX)null;
                AppMain.NNS_PRIM3D_PCT_ARRAY array = AppMain.amDrawAlloc_NNS_PRIM3D_PCT(4);
                setParam.vtxPCT3D = array;
                AppMain.NNS_PRIM3D_PCT[] buffer = array.buffer;
                int offset = array.offset;
                buffer[offset].Pos.x = buffer[offset + 1].Pos.x = (float)num7;
                buffer[offset + 2].Pos.x = buffer[offset + 3].Pos.x = (float)num7 + num4;
                buffer[offset].Pos.y = buffer[offset + 2].Pos.y = (float)disp_pos_y;
                buffer[offset + 1].Pos.y = buffer[offset + 3].Pos.y = (float)disp_pos_y + num4;
                buffer[offset].Pos.z = buffer[offset + 1].Pos.z = buffer[offset + 2].Pos.z = buffer[offset + 3].Pos.z = -2f;
                float num10 = (float)AppMain.dm_stfrl_font_width_length_tbl[(int)num5] * AppMain.dm_stfrl_list_id_font_size_tbl[(int)lineId];
                num7 = (uint)((double)num7 + (double)num10);
                buffer[offset].Col = buffer[offset + 1].Col = buffer[offset + 2].Col = buffer[offset + 3].Col = AppMain.AMD_RGBA8888(numArray[0], numArray[1], numArray[2], main_work.cur_page_list_alpha_data);
                buffer[offset].Tex.u = buffer[offset + 1].Tex.u = (float)num8 * (1f / 16f);
                buffer[offset + 2].Tex.u = buffer[offset + 3].Tex.u = buffer[offset].Tex.u + 1f / 16f;
                buffer[offset].Tex.v = buffer[offset + 2].Tex.v = (float)num9 * 0.125f;
                buffer[offset + 1].Tex.v = buffer[offset + 3].Tex.v = buffer[offset].Tex.v + 0.125f;
                setParam.format3D = 4;
                setParam.type = 1;
                setParam.count = 4;
                setParam.texlist = AppMain.AoTexGetTexList(AppMain.dm_stfrl_font_tex);
                setParam.texId = 0;
                setParam.ablend = 1;
                setParam.aTest = (short)0;
                setParam.zMask = (short)1;
                setParam.zTest = (short)0;
                AppMain.AoActDrawCorWide(array, 0, 4U, AppMain.AOE_ACT_CORW.AOD_ACT_CORW_CENTER);
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
                    AppMain.amDrawPrimitive3D(20U, setParam);
                }
            }
            AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
        }
    }

    private static void dmStaffRollTaskDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.UNREFERENCED_PARAMETER((object)tcb);
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(20U);
        AppMain.amDrawEndScene();
    }

    private static void dmStaffRollEndActDraw(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(2304U);
        AppMain.AoActSysSetDrawStateEnable(true);
        AppMain.AoActSysSetDrawState(30U);
        if (((int)main_work.flag & 4096) != 0)
            ++main_work.continue_act_frm;
        if (main_work.is_eme_comp)
        {
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_stfrl_end_tex));
            if (main_work.end_act_frm > 0)
            {
                for (int index = 0; index <= 5; ++index)
                    AppMain.AoActSortRegAction(main_work.act[index]);
            }
            for (int index = 6; index <= 7; ++index)
                AppMain.AoActSortRegAction(main_work.act[index]);
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_stfrl_end_jp_tex));
            AppMain.AoActSortRegAction(main_work.act[9]);
            AppMain.AoActSetFrame(main_work.act[9], (float)main_work.end_act_frm);
        }
        else
            AppMain.AoActSortRegAction(main_work.act[8]);
        for (int index = 0; index <= 7; ++index)
            AppMain.AoActSetFrame(main_work.act[index], (float)main_work.end_act_frm);
        for (int index = 0; index < 11; ++index)
        {
            if (index <= 7)
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_stfrl_end_tex));
            else
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_stfrl_end_jp_tex));
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
        }
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.dmStaffRollEndActTaskDraw), (ushort)2304, 0U);
    }

    private static void dmStaffRollEndActTaskDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.UNREFERENCED_PARAMETER((object)tcb);
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(30U);
        AppMain.amDrawEndScene();
    }

    private static void dmStaffRollWinActDraw(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        float[] numArray = new float[2];
        AppMain.AoActSysSetDrawTaskPrio(3072U);
        AppMain.AoActSysSetDrawStateEnable(true);
        AppMain.AoActSysSetDrawState(50U);
        numArray[0] = 641.25f;
        numArray[1] = 303.75f;
        AppMain.AoWinSysDrawState(0, AppMain.AoTexGetTexList(AppMain.dm_stfrl_cmn_tex[0]), 1U, 480f, 356f, numArray[0] * main_work.win_size_rate[0], (float)((double)numArray[1] * (double)main_work.win_size_rate[1] * 0.899999976158142), 50U);
        if (((int)main_work.flag & 64) != 0)
        {
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_stfrl_cmn_tex[0]));
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_stfrl_cmn_tex[1]));
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_stfrl_end_jp_tex));
            AppMain.AoActSortRegAction(main_work.act[10]);
        }
        AppMain.AoActAcmPush();
        AppMain.AoActAcmInit();
        AppMain.AoActAcmApplyScale(27f / 16f, 27f / 16f);
        AppMain.AoActAcmApplyTrans(AppMain.dm_stfrl_win_act_pos_tbl[3][0], AppMain.dm_stfrl_win_act_pos_tbl[3][1], 0.0f);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_stfrl_end_jp_tex));
        AppMain.AoActUpdate(main_work.act[10], 0.0f);
        AppMain.AoActAcmPop();
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.dmStaffRollWinActTaskDraw), (ushort)3072, 0U);
    }

    private static void dmStaffRollWinActTaskDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.UNREFERENCED_PARAMETER((object)tcb);
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(50U);
        AppMain.amDrawEndScene();
    }

    private static int dmStaffRollIsDataLoad(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (AppMain.dm_stfrl_is_full_staffroll)
        {
            if (main_work.staff_list_fs is AppMain.AMS_FS && !AppMain.amFsIsComplete((AppMain.AMS_FS)main_work.staff_list_fs))
                return 0;
        }
        else if (!AppMain.amFsIsComplete(main_work.arc_list_font_amb_fs))
            return 0;
        return 1;
    }

    private static void dmStaffRollDataClearRequestFull(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.arc_data.arc_font_amb = main_work.arc_list_font_amb;
        main_work.arc_data.arc_scr_amb = AppMain.readAMBFile((object)main_work.arc_scr_amb_fs);
        main_work.arc_data.arc_end_amb = AppMain.readAMBFile((object)main_work.arc_end_amb_fs);
        main_work.arc_data.arc_end_jp_amb = AppMain.readAMBFile((object)main_work.arc_end_jp_amb_fs);
        for (uint index = 0; index < 2U; ++index)
            main_work.arc_data.arc_cmn_amb[(int)index] = AppMain.readAMBFile((object)main_work.arc_cmn_amb_fs[(int)index]);
        main_work.arc_data.stf_list_ysd = new AppMain.YSDS_HEADER((byte[])main_work.staff_list_fs);
        main_work.staff_list_fs = (object)null;
    }

    private static void dmStaffRollDataClearRequestEasy(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.arc_data.arc_font_amb = AppMain.readAMBFile(main_work.arc_list_font_amb_fs);
        main_work.arc_list_font_amb_fs = (AppMain.AMS_FS)null;
        main_work.arc_list_font_amb_fs = (AppMain.AMS_FS)null;
        main_work.arc_data.stf_list_ysd = new AppMain.YSDS_HEADER((byte[])main_work.staff_list_fs);
        main_work.staff_list_fs = (object)null;
        main_work.staff_list_fs = (object)null;
    }

    private static void dmStaffRollDataBuildEasy(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        AppMain.dm_stfrl_font_amb = main_work.arc_data.arc_font_amb;
        AppMain.AoTexBuild(AppMain.dm_stfrl_font_tex, AppMain.dm_stfrl_font_amb);
        AppMain.AoTexLoad(AppMain.dm_stfrl_font_tex);
        AppMain.dm_stfrl_data_mgr_p = main_work.arc_data;
    }

    private static int dmStaffRollIsTexLoad()
    {
        if (AppMain.dm_stfrl_is_full_staffroll)
        {
            if (!AppMain.AoTexIsLoaded(AppMain.dm_stfrl_font_tex) || !AppMain.AoTexIsLoaded(AppMain.dm_stfrl_scr_tex) || (!AppMain.AoTexIsLoaded(AppMain.dm_stfrl_end_tex) || !AppMain.AoTexIsLoaded(AppMain.dm_stfrl_end_jp_tex)))
                return 0;
            for (int index = 0; index < 2; ++index)
            {
                if (!AppMain.AoTexIsLoaded(AppMain.dm_stfrl_cmn_tex[index]))
                    return 0;
            }
        }
        else if (!AppMain.AoTexIsLoaded(AppMain.dm_stfrl_font_tex))
            return 0;
        return 1;
    }

    private static int dmStaffRollIsTexRelease()
    {
        if (AppMain.dm_stfrl_is_full_staffroll)
        {
            if (!AppMain.AoTexIsReleased(AppMain.dm_stfrl_font_tex) || !AppMain.AoTexIsReleased(AppMain.dm_stfrl_scr_tex) || (!AppMain.AoTexIsReleased(AppMain.dm_stfrl_end_tex) || !AppMain.AoTexIsReleased(AppMain.dm_stfrl_end_jp_tex)))
                return 0;
            for (int index = 0; index < 2; ++index)
            {
                if (!AppMain.AoTexIsReleased(AppMain.dm_stfrl_cmn_tex[index]))
                    return 0;
            }
        }
        else if (!AppMain.AoTexIsReleased(AppMain.dm_stfrl_font_tex))
            return 0;
        return 1;
    }

    private static void dmStaffRollSetFadePageInfoEfctData(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.disp_page_time = AppMain.AoYsdFileGetPageTime(AppMain.dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page);
        main_work.disp_page_time = (uint)((double)main_work.disp_page_time - 32.0 - 2.0);
        main_work.fade_timer = (float)main_work.disp_page_time;
        if (AppMain.AoYsdFileIsPageShowImage(AppMain.dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page))
        {
            main_work.flag |= 16U;
            main_work.cur_disp_image = AppMain.AoYsdFileGetPageShowImageNo(AppMain.dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page);
            for (int index = 0; index < 3; ++index)
            {
                if (main_work.ring_work[index] != null)
                    main_work.ring_work[index].flag |= 1U;
            }
        }
        if (!AppMain.AoYsdFileIsPageHideImage(AppMain.dm_stfrl_data_mgr_p.stf_list_ysd, main_work.cur_disp_list_page))
            return;
        main_work.flag |= 32U;
    }

    private static void dmStaffRollSetEfctChngAlphaListData(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        int num1 = (int)main_work.cur_page_list_alpha_data;
        int num2 = (int)main_work.cur_page_scr_alpha_data;
        if ((double)main_work.fade_timer >= (double)((float)main_work.disp_page_time - 32f))
        {
            num1 += 8;
            if (((int)main_work.flag & 16) != 0)
            {
                num2 += 8;
                if (num2 >= (int)byte.MaxValue)
                {
                    num2 = (int)byte.MaxValue;
                    main_work.flag &= 4294967279U;
                }
            }
        }
        else if ((double)main_work.fade_timer <= 32.0)
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
        if (num1 >= (int)byte.MaxValue)
            num1 = (int)byte.MaxValue;
        if (num1 <= 0)
            num1 = 0;
        main_work.cur_page_list_alpha_data = (uint)num1;
        main_work.cur_page_scr_alpha_data = (uint)num2;
    }

    private static void dmStaffRollSetWinOpenEfct(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if ((double)main_work.win_timer > 8.0)
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
            main_work.win_size_rate[(int)index] = 0.0 == (double)main_work.win_timer ? 1f : main_work.win_timer / 8f;
            if ((double)main_work.win_size_rate[(int)index] > 1.0)
                main_work.win_size_rate[(int)index] = 1f;
        }
    }

    private static void dmStaffRollSetWinCloseEfct(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        for (uint index = 0; index < 2U; ++index)
            main_work.win_size_rate[(int)index] = 0.0 == (double)main_work.win_timer ? 0.0f : main_work.win_timer / 8f;
        if ((double)main_work.win_timer < 0.0)
        {
            main_work.flag |= 512U;
            main_work.win_timer = 0.0f;
            for (uint index = 0; index < 2U; ++index)
                main_work.win_size_rate[(int)index] = 0.0f;
        }
        else
            --main_work.win_timer;
    }

    private static void dmStaffRollSetObjSystemData(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        AppMain.ObjDrawESEffectSystemInit((ushort)0, 20480U, 5U);
        AppMain.ObjDrawSetNNCommandStateTbl(0U, 1U, false);
        AppMain.ObjDrawSetNNCommandStateTbl(1U, 2U, false);
        AppMain.ObjDrawSetNNCommandStateTbl(2U, 3U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(3U, 5U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(4U, 11U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(5U, 12U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(6U, 9U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(7U, 4U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(8U, 8U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(9U, 7U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(10U, 10U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(11U, 6U, true);
        AppMain.ObjDrawSetNNCommandStateTbl(12U, 0U, true);
        if (((int)AppMain.g_gm_main_system.game_flag & 512) != 0)
            AppMain.g_gm_main_system.game_time = 0U;
        AppMain.g_gm_main_system.game_flag &= 4187479041U;
        AppMain.g_obj.flag = 4194408U;
        AppMain.g_obj.ppPre = (AppMain.OBJECT_Delegate)null;
        AppMain.g_obj.ppPost = (AppMain.OBJECT_Delegate)null;
        AppMain.g_obj.ppCollision = (AppMain.OBJECT_WORK_Delegate)null;
        AppMain.g_obj.ppObjPre = new AppMain.OBJECT_WORK_Delegate(AppMain.GmObjObjPreFunc);
        AppMain.g_obj.ppObjPost = new AppMain.OBJECT_WORK_Delegate(AppMain.GmObjObjPostFunc);
        AppMain.g_obj.ppRegRecAuto = (AppMain.OBJECT_WORK_Delegate)null;
        AppMain.g_obj.draw_scale.x = AppMain.g_obj.draw_scale.y = AppMain.g_obj.draw_scale.z = 13107;
        AppMain.g_obj.inv_draw_scale.x = AppMain.g_obj.inv_draw_scale.y = AppMain.g_obj.inv_draw_scale.z = AppMain.FX_Div(4096, AppMain.g_obj.draw_scale.x);
        AppMain.g_obj.depth = 128;
        AppMain.dmStaffRollInitLight();
        AppMain.dmStaffRollCameraInit();
        AppMain.GmEfctCmnBuildDataInit();
    }

    private static void dmStaffRollSetupEndModel(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        for (int index = 0; index < 3; ++index)
        {
            if (main_work.ring_work[index] != null)
            {
                AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)main_work.ring_work[index];
                obsObjectWork.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
                obsObjectWork.flag |= 8U;
            }
        }
        AppMain.dmStaffRollSetBossObj(main_work);
        if (!main_work.is_eme_comp)
            return;
        main_work.sonic_work = AppMain.DmStfrlMdlCtrlSetSonicObj();
    }

    private static void dmStaffRollNodispEndModel(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        if (main_work.body_work != null)
        {
            AppMain.OBS_OBJECT_WORK bodyWork = (AppMain.OBS_OBJECT_WORK)main_work.body_work;
            bodyWork.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
            bodyWork.flag |= 8U;
        }
        if (main_work.egg_work != null)
        {
            AppMain.OBS_OBJECT_WORK eggWork = (AppMain.OBS_OBJECT_WORK)main_work.egg_work;
            eggWork.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
            eggWork.flag |= 8U;
        }
        for (int index = 0; index < 3; ++index)
        {
            if (main_work.ring_work[index] != null)
            {
                AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)main_work.ring_work[index];
                obsObjectWork.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
                obsObjectWork.flag |= 8U;
            }
        }
        if (main_work.sonic_work == null)
            return;
        AppMain.OBS_OBJECT_WORK sonicWork = (AppMain.OBS_OBJECT_WORK)main_work.sonic_work;
        sonicWork.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        sonicWork.flag |= 8U;
    }

    private static void dmStaffRollSetBossObj(AppMain.DMS_STFRL_MAIN_WORK main_work)
    {
        main_work.body_work = AppMain.DmStfrlMdlCtrlSetBodyObj();
        main_work.egg_work = AppMain.DmStfrlMdlCtrlSetEggObj((AppMain.OBS_OBJECT_WORK)main_work.body_work);
        if (!main_work.is_eme_comp)
            return;
        main_work.body_work.flag |= 1U;
    }

    private static void dmStaffRollInitLight()
    {
        AppMain.NNS_RGBA col = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.g_obj.ambient_color.r = 0.8f;
        AppMain.g_obj.ambient_color.g = 0.8f;
        AppMain.g_obj.ambient_color.b = 0.8f;
        nnsVector.x = -1f;
        nnsVector.y = -1f;
        nnsVector.z = -1f;
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_0, ref col, 1f, nnsVector);
        AppMain.g_gm_main_system.def_light_vec.Assign(nnsVector);
        AppMain.g_gm_main_system.def_light_col = col;
        AppMain.ObjDrawSetParallelLight(AppMain.NNE_LIGHT_6, ref col, 1f, nnsVector);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    private static void dmStaffRollCameraInit()
    {
        AppMain.ObjCameraInit(0, new AppMain.NNS_VECTOR(0.0f, 0.0f, 50f), 4, (ushort)0, 8192);
        AppMain.ObjCamera3dInit(0);
        AppMain.g_obj.glb_camera_id = 0;
        AppMain.g_obj.glb_camera_type = 1;
        AppMain.GmCameraDelayReset();
        AppMain.GmCameraAllowReset();
        AppMain.ObjCameraSetUserFunc(0, new AppMain.OBJF_CAMERA_USER_FUNC(AppMain.dmStaffRollCameraFunc));
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(0);
        obsCamera.scale = GMD_CAMERA_SCALE;
        obsCamera.ofst.z = 1000f;
    }

    private static void dmStaffRollCameraFunc(AppMain.OBS_CAMERA obj_camera)
    {
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        nnsVector.x = AppMain.FXM_FX32_TO_FLOAT(0);
        nnsVector.y = AppMain.FXM_FX32_TO_FLOAT(0);
        nnsVector.z = AppMain.FXM_FX32_TO_FLOAT(409600);
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
        AppMain.ObjObjectCameraSet(AppMain.FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - (float)((int)AppMain.OBD_LCD_X / 2)), AppMain.FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - (float)((int)AppMain.OBD_LCD_Y / 2)), AppMain.FXM_FLOAT_TO_FX32(obj_camera.disp_pos.x - (float)((int)AppMain.OBD_LCD_X / 2)), AppMain.FXM_FLOAT_TO_FX32(-obj_camera.disp_pos.y - (float)((int)AppMain.OBD_LCD_Y / 2)));
        AppMain.GmCameraSetClipCamera(obj_camera);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    public void DmMovieInit(object arg)
    {
        AppMain.SyDecideEvtCase((short)0);
        AppMain.SyChangeNextEvt();
    }


    private static void DmStfrlMdlCtrlSonicBuild()
    {
        AppMain.dm_stfrl_sonic_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.g_gm_player_data_work[0][0].pData), AppMain.readAMBFile(AppMain.g_gm_player_data_work[0][1].pData), 0U);
    }

    private static void DmStfrlMdlCtrlSonicFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.g_gm_player_data_work[0][0].pData);
        AppMain.GmGameDBuildRegFlushModel(AppMain.dm_stfrl_sonic_obj_3d_list, amsAmbHeader.file_num);
        AppMain.dm_stfrl_sonic_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.DMS_STFRL_SONIC_WORK DmStfrlMdlCtrlSetSonicObj()
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)24576, (byte)0, (byte)0, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_STFRL_SONIC_WORK()), "STAFFROLL_SONIC");
        AppMain.DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (AppMain.DMS_STFRL_SONIC_WORK)obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.dm_stfrl_sonic_obj_3d_list[0], obj_work.obj_3d);
        obj_work.obj_3d.blend_spd = 1f / 16f;
        AppMain.ObjDrawObjectSetToon(obj_work);
        AppMain.OBS_ACTION3D_NN_WORK obj3d = dmsStfrlSonicWork.obj_work.obj_3d;
        AppMain.ObjObjectAction3dNNMotionLoad(obj_work, 0, true, AppMain.g_gm_player_data_work[0][4], (string)null, 0, (AppMain.AMS_AMB_HEADER)null, 136, 16);
        obj_work.flag |= 16U;
        obj_work.disp_flag |= 4194309U;
        obj_work.disp_flag &= 4294967263U;
        obj_work.disp_flag |= 150995456U;
        obj_work.obj_3d.drawflag |= 8388608U;
        obj_work.pos.x = 0;
        obj_work.pos.y = 98304;
        obj_work.pos.z = -12288;
        obj_work.dir.y = (ushort)AppMain.AKM_DEGtoA16(90);
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        dmsStfrlSonicWork.alpha = 1f;
        AppMain.ObjDrawObjectActionSet(obj_work, 21);
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlSonicDrawFunc);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlSonicProcWaitSetup);
        return dmsStfrlSonicWork;
    }

    private static void DmStfrlMdlCtrlBoss1Build()
    {
        AppMain.dm_stfrl_boss1_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, AppMain.g_gm_gamedat_enemy_arc)), AppMain.readAMBFile(AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 1, AppMain.g_gm_gamedat_enemy_arc)), 0U);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(728), 2, AppMain.g_gm_gamedat_enemy_arc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(729), 3, AppMain.g_gm_gamedat_enemy_arc);
    }

    private static void DmStfrlMdlCtrlBoss1Flush()
    {
        AppMain.ObjDataRelease(AppMain.ObjDataGet(729));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(728));
        AppMain.AMS_AMB_HEADER amsAmbHeader = (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, AppMain.g_gm_gamedat_enemy_arc);
        AppMain.GmGameDBuildRegFlushModel(AppMain.dm_stfrl_boss1_obj_3d_list, amsAmbHeader.file_num);
        AppMain.dm_stfrl_boss1_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.DMS_STFRL_BOSS_BODY_WORK DmStfrlMdlCtrlSetBodyObj()
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)24576, (byte)0, (byte)0, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_STFRL_BOSS_BODY_WORK()), "BOSS1_BODY");
        AppMain.DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (AppMain.DMS_STFRL_BOSS_BODY_WORK)obj_work;
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.dmStfrlMdlCtrlBoss1BodyExit));
        obj_work.flag |= 16U;
        obj_work.disp_flag |= 4194309U;
        obj_work.disp_flag &= 4294967263U;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.dm_stfrl_boss1_obj_3d_list[0], obj_work.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(obj_work, 0, true, AppMain.ObjDataGet(728), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(obj_work);
        obj_work.obj_3d.blend_spd = 0.125f;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.ObjDrawActionSummary);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlBodyProcWaitSetup);
        return stfrlBossBodyWork;
    }

    private static AppMain.DMS_STFRL_BOSS_EGG_WORK DmStfrlMdlCtrlSetEggObj(
      AppMain.OBS_OBJECT_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)24576, (byte)0, (byte)0, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_STFRL_BOSS_EGG_WORK()), "BOSS1_EGG");
        AppMain.DMS_STFRL_BOSS_EGG_WORK stfrlBossEggWork = (AppMain.DMS_STFRL_BOSS_EGG_WORK)obj_work;
        obj_work.parent_obj = body_work;
        obj_work.move_flag |= 256U;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.dm_stfrl_boss1_obj_3d_list[1], obj_work.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(obj_work, 0, true, AppMain.ObjDataGet(729), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(obj_work);
        obj_work.disp_flag |= 134217728U;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.ObjDrawActionSummary);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlEggProcWaitSetup);
        obj_work.flag |= 16U;
        obj_work.disp_flag |= 4U;
        obj_work.disp_flag |= 4194304U;
        return stfrlBossEggWork;
    }

    private static void DmStfrlMdlCtrlRingBuild()
    {
        AppMain.dm_stfrl_ring_obj_3d = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(956), AppMain.GmGameDatGetGimmickData(957), 0U);
    }

    private static void DmStfrlMdlCtrlRingFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(956);
        AppMain.GmGameDBuildRegFlushModel(AppMain.dm_stfrl_ring_obj_3d, gimmickData.file_num);
        AppMain.dm_stfrl_ring_obj_3d = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.DMS_STFRL_RING_WORK DmStfrlMdlCtrlSetRingObj(
      int delay_time,
      uint type)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.OBM_OBJECT_TASK_DETAIL_INIT((ushort)24576, (byte)0, (byte)0, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_STFRL_RING_WORK()), "RING_OBJ");
        AppMain.DMS_STFRL_RING_WORK dmsStfrlRingWork = (AppMain.DMS_STFRL_RING_WORK)obj_work;
        obj_work.flag |= 16U;
        obj_work.disp_flag |= 4194309U;
        obj_work.disp_flag &= 4294967263U;
        obj_work.disp_flag |= 134217728U;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.dm_stfrl_ring_obj_3d[0], obj_work.obj_3d);
        AppMain.ObjObjectAction3dNNMaterialMotionLoad(obj_work, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, (object)AppMain.readAMBFile(AppMain.ObjDataGet(4).pData));
        obj_work.disp_flag |= 4194309U;
        obj_work.disp_flag &= 4294967263U;
        obj_work.disp_flag |= 150995456U;
        obj_work.obj_3d.drawflag |= 8388608U;
        obj_work.obj_3d.draw_state.alpha.alpha = 0.0f;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlRingDrawFunc);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlRingProcStartWait);
        dmsStfrlRingWork.disp_ring_pos_no = (int)type;
        dmsStfrlRingWork.start_pos.x = AppMain.dm_stfrl_ring_disp_pos_tbl[dmsStfrlRingWork.disp_ring_pos_no][0];
        dmsStfrlRingWork.start_pos.y = AppMain.dm_stfrl_ring_disp_pos_tbl[dmsStfrlRingWork.disp_ring_pos_no][1];
        dmsStfrlRingWork.start_pos.z = -12288;
        dmsStfrlRingWork.efct_start_time = delay_time;
        dmsStfrlRingWork.disp_efct_pos_no = (int)type;
        return dmsStfrlRingWork;
    }

    private static void dmStfrlMdlCtrlSonicProcWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (AppMain.DMS_STFRL_SONIC_WORK)obj_work;
        ++dmsStfrlSonicWork.timer;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (dmsStfrlSonicWork.timer > (short)300)
        {
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 42);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlSonicProcWaitChngDash2);
            dmsStfrlSonicWork.timer = (short)0;
        }
        else
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 21);
    }

    private static void dmStfrlMdlCtrlSonicProcWaitChngDash2(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (AppMain.DMS_STFRL_SONIC_WORK)obj_work;
        ++dmsStfrlSonicWork.timer;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (dmsStfrlSonicWork.timer > (short)30)
        {
            obj_work.obj_3d.blend_spd = 0.125f;
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 9);
            AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(obj_work, 53);
            AppMain.GmComEfctSetDispOffsetF(efct_work, -1.5f, 0.0f, 9f);
            efct_work.obj_3des.ecb.drawObjState = 0U;
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlSonicProcWaitMtnEnd);
            dmsStfrlSonicWork.timer = (short)0;
        }
        else
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 42);
    }

    private static void dmStfrlMdlCtrlSonicProcWaitMtnEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (AppMain.DMS_STFRL_SONIC_WORK)obj_work;
        ++dmsStfrlSonicWork.timer;
        if (obj_work.spd_m <= 25292)
            obj_work.spd_m += 512;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 9);
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        if (dmsStfrlSonicWork.timer <= (short)60)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlSonicProcWaitFadeEnd);
        AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 9);
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        dmsStfrlSonicWork.timer = (short)120;
    }

    private static void dmStfrlMdlCtrlSonicProcWaitFadeEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (AppMain.DMS_STFRL_SONIC_WORK)obj_work;
        --dmsStfrlSonicWork.timer;
        obj_work.pos.x += 73728;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (dmsStfrlSonicWork.timer <= (short)0)
        {
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlSonicProcIdle);
            dmsStfrlSonicWork.timer = (short)0;
            dmsStfrlSonicWork.flag |= (ushort)1;
            obj_work.disp_flag |= 32U;
        }
        else
        {
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 9);
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        }
    }

    private static void dmStfrlMdlCtrlSonicProcIdle(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void dmStfrlMdlCtrlSonicDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_SONIC_WORK dmsStfrlSonicWork = (AppMain.DMS_STFRL_SONIC_WORK)obj_work;
        obj_work.obj_3d.draw_state.alpha.alpha = dmsStfrlSonicWork.alpha;
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void dmStfrlMdlCtrlBoss1BodyExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        AppMain.DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (AppMain.DMS_STFRL_BOSS_BODY_WORK)tcbWork;
        AppMain.GmBsCmnClearBossMotionCBSystem(tcbWork);
        AppMain.GmBsCmnDeleteSNMWork(stfrlBossBodyWork.snm_work);
        AppMain.ObjObjectExit(tcb);
    }

    private static void dmStfrlMdlCtrlBodyProcWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (AppMain.DMS_STFRL_BOSS_BODY_WORK)obj_work;
        AppMain.GmBsCmnInitBossMotionCBSystem(obj_work, stfrlBossBodyWork.bmcb_mgr);
        AppMain.GmBsCmnCreateSNMWork(stfrlBossBodyWork.snm_work, obj_work.obj_3d._object, (ushort)1);
        AppMain.GmBsCmnAppendBossMotionCallback(stfrlBossBodyWork.bmcb_mgr, stfrlBossBodyWork.snm_work.bmcb_link);
        stfrlBossBodyWork.egg_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(stfrlBossBodyWork.snm_work, 11);
        if (((int)stfrlBossBodyWork.flag & 1) != 0)
        {
            stfrlBossBodyWork.timer = 0;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlBodyProcBodyCompInitStart);
        }
        else
        {
            stfrlBossBodyWork.timer = 120;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlBodyProcBodyMain);
        }
    }

    private static void dmStfrlMdlCtrlBodyProcBodyMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (AppMain.DMS_STFRL_BOSS_BODY_WORK)obj_work;
        obj_work.pos.x = 0;
        obj_work.pos.y = -65536;
        obj_work.pos.z = -81920;
        obj_work.dir.y = (ushort)AppMain.AKM_DEGtoA16(300);
        if (stfrlBossBodyWork.timer != 0)
            --stfrlBossBodyWork.timer;
        else
            stfrlBossBodyWork.flag |= 2097152U;
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompInitStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (AppMain.DMS_STFRL_BOSS_BODY_WORK)obj_work;
        obj_work.pos.x = 0;
        obj_work.pos.y = -737280;
        obj_work.pos.z = -81920;
        obj_work.dir.y = (ushort)AppMain.AKM_DEGtoA16(300);
        if (((int)stfrlBossBodyWork.flag & 2) == 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlBodyProcBodyCompStartWait);
        stfrlBossBodyWork.flag &= 4294967293U;
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompStartWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (AppMain.DMS_STFRL_BOSS_BODY_WORK)obj_work;
        ++stfrlBossBodyWork.timer;
        if (stfrlBossBodyWork.timer <= 60)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlBodyProcBodyCompMoveDown);
        stfrlBossBodyWork.timer = 0;
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompMoveDown(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (AppMain.DMS_STFRL_BOSS_BODY_WORK)obj_work;
        obj_work.pos.y += 4096;
        if (obj_work.pos.y < -163840)
            return;
        obj_work.pos.y = -163840;
        stfrlBossBodyWork.flag |= 2097152U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlBodyProcBodyCompLaughWait);
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompLaughWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (AppMain.DMS_STFRL_BOSS_BODY_WORK)obj_work;
        ++stfrlBossBodyWork.timer;
        if (stfrlBossBodyWork.timer < 180)
            return;
        stfrlBossBodyWork.timer = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlBodyProcBodyCompMoveUpWait);
        stfrlBossBodyWork.flag &= 4294967291U;
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompMoveUpWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_BOSS_BODY_WORK stfrlBossBodyWork = (AppMain.DMS_STFRL_BOSS_BODY_WORK)obj_work;
        obj_work.pos.y += -2457;
        if (stfrlBossBodyWork.timer > 100)
            stfrlBossBodyWork.flag |= 16U;
        else
            ++stfrlBossBodyWork.timer;
        if (obj_work.pos.y > -737280)
            return;
        obj_work.pos.y = -737280;
        stfrlBossBodyWork.flag |= 8U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlBodyProcBodyCompEndWaitIdle);
        stfrlBossBodyWork.timer = 0;
    }

    private static void dmStfrlMdlCtrlBodyProcBodyCompEndWaitIdle(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void dmStfrlMdlCtrlEggProcWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d.blend_spd = 0.025f;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlEggProcMain);
    }

    private static void dmStfrlMdlCtrlEggProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_BOSS_BODY_WORK parentObj = (AppMain.DMS_STFRL_BOSS_BODY_WORK)obj_work.parent_obj;
        AppMain.DMS_STFRL_BOSS_EGG_WORK stfrlBossEggWork = (AppMain.DMS_STFRL_BOSS_EGG_WORK)obj_work;
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.egg_snm_reg_id, 1);
        if (((int)parentObj.flag & 2097152) == 0 || ((int)stfrlBossEggWork.flag & 1) != 0)
            return;
        AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 3);
        obj_work.obj_3d.frame[0] = 0.0f;
        obj_work.obj_3d.blend_spd = 0.025f;
        stfrlBossEggWork.flag |= 1U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlEggProcMainIdle);
    }

    private static void dmStfrlMdlCtrlEggProcMainIdle(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_BOSS_BODY_WORK parentObj = (AppMain.DMS_STFRL_BOSS_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.egg_snm_reg_id, 1);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 3);
        obj_work.obj_3d.frame[0] = 0.0f;
        obj_work.obj_3d.blend_spd = 0.125f;
    }

    private static void dmStfrlMdlCtrlRingProcStartWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_RING_WORK dmsStfrlRingWork = (AppMain.DMS_STFRL_RING_WORK)obj_work;
        if (((int)dmsStfrlRingWork.flag & 1) == 0)
            return;
        ++dmsStfrlRingWork.timer;
        if (dmsStfrlRingWork.timer < dmsStfrlRingWork.efct_start_time)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlRingProcInitSetup);
        AppMain.dmStfrlMdlCtrlCreateRingEfct(dmsStfrlRingWork.start_pos.x + AppMain.dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no][0], dmsStfrlRingWork.start_pos.y + AppMain.dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no][1]);
        AppMain.dmStfrlMdlCtrlCreateRingEfct(dmsStfrlRingWork.start_pos.x + AppMain.dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no + 1][0], dmsStfrlRingWork.start_pos.y + AppMain.dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no + 1][1]);
        AppMain.dmStfrlMdlCtrlCreateRingEfct(dmsStfrlRingWork.start_pos.x + AppMain.dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no + 2][0], dmsStfrlRingWork.start_pos.y + AppMain.dm_stfrl_ring_efct_disp_offset_tbl[dmsStfrlRingWork.disp_efct_pos_no + 2][1]);
        dmsStfrlRingWork.timer = 0;
        dmsStfrlRingWork.flag &= 4294967294U;
    }

    private static void dmStfrlMdlCtrlRingProcInitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_RING_WORK dmsStfrlRingWork = (AppMain.DMS_STFRL_RING_WORK)obj_work;
        obj_work.obj_3d.draw_state.alpha.alpha = 0.0f;
        ushort num1 = 10922;
        ushort num2 = 0;
        for (int index = 0; index < 6; ++index)
        {
            dmsStfrlRingWork.pos[index].x = dmsStfrlRingWork.start_pos.x;
            dmsStfrlRingWork.pos[index].y = dmsStfrlRingWork.start_pos.y;
            dmsStfrlRingWork.pos[index].z = -3;
            dmsStfrlRingWork.spd_x[index] = AppMain.mtMathSin((int)(ushort)((uint)num2 + (uint)index * (uint)num1));
            dmsStfrlRingWork.spd_y[index] = AppMain.mtMathCos((int)(ushort)((uint)num2 + (uint)index * (uint)num1));
            dmsStfrlRingWork.spd_y[index] += 512;
        }
        dmsStfrlRingWork.alpha_spd = 0.1f;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlRingProcDispIdle);
    }

    private static void dmStfrlMdlCtrlRingProcDispIdle(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_RING_WORK dmsStfrlRingWork = (AppMain.DMS_STFRL_RING_WORK)obj_work;
        for (int index = 0; index < 6; ++index)
            dmsStfrlRingWork.spd_y[index] += 64;
        ++dmsStfrlRingWork.timer;
        if (dmsStfrlRingWork.timer >= 10)
        {
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlRingProcNoDispIdle);
            dmsStfrlRingWork.timer = 60;
            dmsStfrlRingWork.alpha_spd = 0.01666667f;
        }
        if ((double)dmsStfrlRingWork.alpha >= 1.0)
            dmsStfrlRingWork.alpha = 1f;
        else
            dmsStfrlRingWork.alpha = dmsStfrlRingWork.alpha_spd * (float)dmsStfrlRingWork.timer;
    }

    private static void dmStfrlMdlCtrlRingProcNoDispIdle(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_RING_WORK dmsStfrlRingWork = (AppMain.DMS_STFRL_RING_WORK)obj_work;
        for (int index = 0; index < 6; ++index)
            dmsStfrlRingWork.spd_y[index] += 64;
        --dmsStfrlRingWork.timer;
        if (dmsStfrlRingWork.timer <= 0)
        {
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.dmStfrlMdlCtrlRingProcStartWait);
            dmsStfrlRingWork.timer = 0;
            ++dmsStfrlRingWork.disp_ring_pos_no;
            if (dmsStfrlRingWork.disp_ring_pos_no > 12)
                dmsStfrlRingWork.disp_ring_pos_no = 0;
            ++dmsStfrlRingWork.disp_efct_pos_no;
            if (dmsStfrlRingWork.disp_efct_pos_no > 12)
                dmsStfrlRingWork.disp_efct_pos_no = 0;
            dmsStfrlRingWork.start_pos.x = AppMain.dm_stfrl_ring_disp_pos_tbl[dmsStfrlRingWork.disp_ring_pos_no][0];
            dmsStfrlRingWork.start_pos.y = AppMain.dm_stfrl_ring_disp_pos_tbl[dmsStfrlRingWork.disp_ring_pos_no][1];
        }
        if ((double)dmsStfrlRingWork.alpha <= 0.0)
            dmsStfrlRingWork.alpha = 0.0f;
        else
            dmsStfrlRingWork.alpha = dmsStfrlRingWork.alpha_spd * (float)dmsStfrlRingWork.timer;
    }

    private static void dmStfrlMdlCtrlRingDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.DMS_STFRL_RING_WORK dmsStfrlRingWork = (AppMain.DMS_STFRL_RING_WORK)obj_work;
        ref AppMain.VecU16 local = ref obj_work.dir;
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
            AppMain.ObjDrawActionSummary(obj_work);
        }
    }

    private static void dmStfrlMdlCtrlCreateRingEfct(int pos_x, int pos_y)
    {
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 50);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = pos_x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = pos_y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = -3;
    }


}