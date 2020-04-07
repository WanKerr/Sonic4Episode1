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
    private static void DmManualBuild(AppMain.AMS_AMB_HEADER[] arc_amb)
    {
        AppMain.dm_manual_mgr.Clear();
        AppMain.dm_manual_mgr_p = AppMain.dm_manual_mgr;
        for (int index = 0; index < 2; ++index)
            AppMain.dm_manual_tex[index].Clear();
        for (int index = 0; index < 2; ++index)
        {
            AppMain.dm_manual_ama[index] = AppMain.readAMAFile(AppMain.amBindGet(arc_amb[index], 0));
            string sPath;
            AppMain.dm_manual_amb[index] = AppMain.readAMBFile(AppMain.amBindGet(arc_amb[index], 1, out sPath));
            AppMain.dm_manual_amb[index].dir = sPath;
        }
        for (int index = 0; index < 2; ++index)
        {
            AppMain.AoTexBuild(AppMain.dm_manual_tex[index], AppMain.dm_manual_amb[index]);
            AppMain.AoTexLoad(AppMain.dm_manual_tex[index]);
        }
    }

    private static bool DmManualBuildCheck()
    {
        return AppMain.dmManualIsTexLoad() != 0;
    }

    private static void DmManualFlush()
    {
        for (int index = 0; index < 2; ++index)
            AppMain.AoTexRelease(AppMain.dm_manual_tex[index]);
    }

    private static bool DmManualFlushCheck()
    {
        return AppMain.dmManualIsTexRelease() != 0;
    }

    private static void DmManualStart()
    {
        AppMain.dmManualInit();
    }

    private static bool DmManualIsExit()
    {
        return AppMain.dm_manual_mgr_p.tcb == null;
    }

    private static void DmManualExit()
    {
        if (AppMain.dm_manual_mgr_p.tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.dm_manual_mgr_p.tcb);
        AppMain.dm_manual_mgr_p.tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void dmManualInit()
    {
        AppMain.dm_manual_mgr_p.tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.dmManualProcMain), new AppMain.GSF_TASK_PROCEDURE(AppMain.dmManualDest), 0U, (ushort)short.MaxValue, 12288U, 10, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.DMS_MANUAL_MAIN_WORK()), "MANUAL_MAIN");
        AppMain.DMS_MANUAL_MAIN_WORK work = (AppMain.DMS_MANUAL_MAIN_WORK)AppMain.dm_manual_mgr_p.tcb.work;
        work.draw_state = AppMain.AoActSysGetDrawStateEnable() ? 1U : 0U;
        AppMain.AoActSysSetDrawStateEnable(work.draw_state != 0U);
        if (work.draw_state != 0U)
        {
            AppMain.dm_manual_draw_state = AppMain.AoActSysGetDrawState();
            AppMain.AoActSysSetDrawState(AppMain.dm_manual_draw_state);
        }
        work.is_jp_region = AppMain.GeEnvGetDecideKey() == AppMain.GSE_DECIDE_KEY.GSD_DECIDE_KEY_O;
        AppMain.dmManualSetInitData(work);
        switch (AppMain.SyGetEvtInfo().cur_evt_id)
        {
            case 6:
            case 11:
                AppMain.dm_manual_is_pause_maingame = true;
                work.se_handle = AppMain.GsSoundAllocSeHandle();
                break;
        }
        work.proc_update = new AppMain.DMS_MANUAL_MAIN_WORK._proc_update_(AppMain.dmManualProcInit);
    }

    private static void dmManualSetInitData(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        main_work.cur_disp_page = 0;
        main_work.cur_disp_page_prev = -1;
    }

    private static void dmManualProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.DMS_MANUAL_MAIN_WORK work = (AppMain.DMS_MANUAL_MAIN_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
            AppMain.DmManualExit();
        if (((int)work.flag & int.MinValue) != 0 && !AppMain.AoAccountIsCurrentEnable())
        {
            work.proc_update = new AppMain.DMS_MANUAL_MAIN_WORK._proc_update_(AppMain.dmManualProcFadeOut);
            work.flag &= (uint)int.MaxValue;
            if (AppMain.dm_manual_is_pause_maingame)
                AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 1U, 1U, 32f, true);
            else
                AppMain.IzFadeInitEasy(1U, 1U, 32f);
            work.flag &= 4294967291U;
            work.flag &= 4294967293U;
            work.proc_input = (AppMain.DMS_MANUAL_MAIN_WORK._proc_input_)null;
        }
        if (work.proc_update != null)
            work.proc_update(work);
        if (work.proc_draw == null)
            return;
        work.proc_draw(work);
    }

    private static void dmManualDest(AppMain.MTS_TASK_TCB tcb)
    {
    }

    private static void dmManualProcInit(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        main_work.proc_update = new AppMain.DMS_MANUAL_MAIN_WORK._proc_update_(AppMain.dmManualProcCreateAct);
        main_work.flag |= 2147483648U;
    }

    private static void dmManualProcCreateAct(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        for (uint index = 0; index <= 13U; ++index)
        {
            AppMain.A2S_AMA_HEADER ama = AppMain.dm_manual_ama[0];
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[0]));
            main_work.act[(int)index] = AppMain.AoActCreate(ama, AppMain.g_dm_act_id_tbl_m[(int)index]);
        }
        for (uint index = 10; index <= 11U; ++index)
            main_work.trg_btn[(int)(index - 10U)].Create(main_work.act[(int)index]);
        main_work.trg_return.Create(main_work.act[13]);
        for (uint index = 119; index <= 120U; ++index)
        {
            AppMain.A2S_AMA_HEADER ama = AppMain.dm_manual_ama[1];
            AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[1]));
            main_work.act[(int)index] = AppMain.AoActCreate(ama, AppMain.g_dm_act_id_tbl_m[(int)index]);
        }
        main_work.proc_update = new AppMain.DMS_MANUAL_MAIN_WORK._proc_update_(AppMain.dmManualProcFadeIn);
        main_work.proc_draw = new AppMain.DMS_MANUAL_MAIN_WORK._proc_draw_(AppMain.dmManualProcActDraw);
        if (AppMain.dm_manual_is_pause_maingame)
            AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 0U, 32f, true);
        else
            AppMain.IzFadeInitEasy(0U, 0U, 32f);
    }

    private static void dmManualProcFadeIn(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        main_work.proc_update = new AppMain.DMS_MANUAL_MAIN_WORK._proc_update_(AppMain.dmManualProcWaitInput);
        main_work.proc_input = new AppMain.DMS_MANUAL_MAIN_WORK._proc_input_(AppMain.dmManualInputProcMain);
    }

    private static void dmManualProcWaitInput(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        if (main_work.proc_input != null)
            main_work.proc_input(main_work);
        int[] numArray = new int[3] { 12, 13, 119 };
        int index = 0;
        for (int length = numArray.Length; index < length; ++index)
        {
            AppMain.AOS_ACTION act = main_work.act[numArray[index]];
            float frame = !main_work.trg_return.GetState(0U)[10] || !main_work.trg_return.GetState(0U)[1] ? (!main_work.trg_return.GetState(0U)[0] ? (2.0 > (double)act.frame ? 0.0f : act.frame) : 1f) : 2f;
            AppMain.AoActSetFrame(act, frame);
        }
        if (((int)main_work.flag & 2) != 0)
        {
            main_work.proc_update = new AppMain.DMS_MANUAL_MAIN_WORK._proc_update_(AppMain.dmManualProcFadeOut);
            if (AppMain.dm_manual_is_pause_maingame)
                AppMain.IzFadeInitEasyColor(0, (ushort)short.MaxValue, (ushort)61439, 18U, 0U, 1U, 32f, true);
            else
                AppMain.IzFadeInitEasy(0U, 1U, 32f);
            if (!AppMain.dm_manual_is_pause_maingame)
                AppMain.DmSoundPlaySE("Cancel");
            else
                AppMain.GsSoundPlaySe("Cancel", main_work.se_handle);
            main_work.flag &= 4294967291U;
            main_work.flag &= 4294967293U;
        }
        else if (((int)main_work.flag & 8) != 0)
        {
            ++main_work.cur_disp_page;
            if (main_work.cur_disp_page > 14)
                main_work.cur_disp_page = 14;
            else if (!AppMain.dm_manual_is_pause_maingame)
                AppMain.DmSoundPlaySE("Cursol");
            else
                AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
            main_work.flag &= 4294967287U;
        }
        else
        {
            if (((int)main_work.flag & 16) == 0)
                return;
            --main_work.cur_disp_page;
            if (main_work.cur_disp_page < 0)
                main_work.cur_disp_page = 0;
            else if (!AppMain.dm_manual_is_pause_maingame)
                AppMain.DmSoundPlaySE("Cursol");
            else
                AppMain.GsSoundPlaySe("Cursol", main_work.se_handle);
            main_work.flag &= 4294967279U;
        }
    }

    private static void dmManualProcFadeOut(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        main_work.proc_update = new AppMain.DMS_MANUAL_MAIN_WORK._proc_update_(AppMain.dmManualProcStopDraw);
        main_work.proc_draw = (AppMain.DMS_MANUAL_MAIN_WORK._proc_draw_)null;
        if (main_work.se_handle == null)
            return;
        AppMain.GsSoundFreeSeHandle(main_work.se_handle);
        main_work.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
    }

    private static void dmManualInputProcMain(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        if (main_work.trg_return.GetState(0U)[10] && main_work.trg_return.GetState(0U)[1] || AppMain.isBackKeyPressed())
        {
            AppMain.setBackKeyRequest(false);
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

    private static void dmManualProcStopDraw(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        for (int index = 0; index < 179; ++index)
        {
            if (main_work.act[index] != null)
            {
                AppMain.AoActDelete(main_work.act[index]);
                main_work.act[index] = (AppMain.AOS_ACTION)null;
            }
        }
        for (int index = 0; index < main_work.trg_btn.Length; ++index)
            main_work.trg_btn[index].Release();
        main_work.trg_return.Release();
        main_work.proc_update = (AppMain.DMS_MANUAL_MAIN_WORK._proc_update_)null;
        main_work.flag |= 1U;
    }

    private static void dmManualProcActDraw(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        AppMain.dmManualCommonBgDraw(main_work);
        AppMain.dmManualPageDraw(main_work);
        AppMain.dmManualCommonDraw(main_work);
        if (main_work.draw_state <= 0U)
            return;
        AppMain.amDrawMakeTask(new AppMain.TaskProc(AppMain.dmManualTaskDraw), (ushort)32768, 0U);
    }

    private static void dmManualTaskDraw(AppMain.AMS_TCB tcb)
    {
        AppMain.AoActDrawPre();
        AppMain.amDrawExecCommand(AppMain.dm_manual_draw_state);
        AppMain.amDrawEndScene();
    }

    private static void dmManualCommonBgDraw(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(6144U);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[0]));
        for (int index = 0; index <= 4; ++index)
            AppMain.AoActSortRegAction(main_work.act[index]);
        for (int index = 0; index <= 4; ++index)
        {
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
            if (index == 3)
            {
                --main_work.act[index].sprite.offset.top;
                main_work.act[index].sprite.offset.left -= 2f;
            }
        }
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private static void dmManualCommonDraw(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(12288U);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[0]));
        for (int index = 5; index <= 11; ++index)
        {
            switch (index)
            {
                case 5:
                    if (main_work.cur_disp_page >= 9)
                    {
                        AppMain.AoActSortRegAction(main_work.act[index]);
                        break;
                    }
                    break;
                case 10:
                    if (main_work.cur_disp_page != 0)
                    {
                        AppMain.AoActSortRegAction(main_work.act[index]);
                        break;
                    }
                    break;
                case 11:
                    if (main_work.cur_disp_page != 14)
                    {
                        AppMain.AoActSortRegAction(main_work.act[index]);
                        break;
                    }
                    break;
                default:
                    AppMain.AoActSortRegAction(main_work.act[index]);
                    break;
            }
        }
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[1]));
        for (int index = 119; index < 120; ++index)
            AppMain.AoActSortRegAction(main_work.act[index]);
        if (main_work.cur_disp_page >= 1)
            AppMain.AoActSortRegAction(main_work.act[120]);
        for (int index = 12; index <= 13; ++index)
            AppMain.AoActSortRegAction(main_work.act[index]);
        float frame1 = main_work.cur_disp_page == 0 ? 0.0f : (float)(main_work.cur_disp_page / 10);
        float num = (float)(main_work.cur_disp_page % 10);
        if (main_work.cur_disp_page == 9)
        {
            AppMain.AoActSetFrame(main_work.act[5], 0.0f);
            AppMain.AoActSetFrame(main_work.act[6], 0.0f);
        }
        else if (main_work.cur_disp_page < 10)
        {
            AppMain.AoActSetFrame(main_work.act[5], 0.0f);
            AppMain.AoActSetFrame(main_work.act[6], (float)main_work.cur_disp_page + 1f);
        }
        else
        {
            AppMain.AoActSetFrame(main_work.act[5], frame1);
            AppMain.AoActSetFrame(main_work.act[6], num + 1f);
        }
        AppMain.AoActSetFrame(main_work.act[8], 0.0f);
        AppMain.AoActSetFrame(main_work.act[9], 0.0f);
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[0]));
        for (int index = 5; index < 10; ++index)
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
        for (int index = 10; index <= 11; ++index)
            AppMain.AoActUpdate(main_work.act[index], 1f);
        for (int index = 12; index <= 13; ++index)
        {
            float frame2 = 2.0 <= (double)main_work.act[index].frame ? 1f : 0.0f;
            AppMain.AoActUpdate(main_work.act[index], frame2);
        }
        for (int index = 0; index < main_work.trg_btn.Length; ++index)
            main_work.trg_btn[index].Update();
        main_work.trg_return.Update();
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[1]));
        for (int index = 119; index < 120; ++index)
        {
            float frame2 = 2.0 <= (double)main_work.act[index].frame ? 1f : 0.0f;
            AppMain.AoActUpdate(main_work.act[index], frame2);
        }
        for (int index = 120; index <= 120; ++index)
            AppMain.AoActUpdate(main_work.act[index], 0.0f);
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private static void dmManualPageDraw(AppMain.DMS_MANUAL_MAIN_WORK main_work)
    {
        AppMain.AoActSysSetDrawTaskPrio(8192U);
        int curDispPage = main_work.cur_disp_page;
        int num1 = AppMain.dm_manual_disp_act_cmn_tbl[curDispPage][0];
        int num2 = AppMain.dm_manual_disp_act_cmn_tbl[curDispPage][1];
        int num3 = AppMain.dm_manual_disp_act_lang_tbl[curDispPage][0];
        int num4 = AppMain.dm_manual_disp_act_lang_tbl[curDispPage][1];
        bool flag = main_work.cur_disp_page_prev != main_work.cur_disp_page;
        if (flag && 0 <= main_work.cur_disp_page_prev)
        {
            for (int index = AppMain.dm_manual_disp_act_cmn_tbl[main_work.cur_disp_page_prev][0]; index <= AppMain.dm_manual_disp_act_cmn_tbl[main_work.cur_disp_page_prev][1]; ++index)
            {
                AppMain.AoActDelete(main_work.act[index]);
                main_work.act[index] = (AppMain.AOS_ACTION)null;
            }
            for (int index = AppMain.dm_manual_disp_act_lang_tbl[main_work.cur_disp_page_prev][0]; index <= AppMain.dm_manual_disp_act_lang_tbl[main_work.cur_disp_page_prev][1]; ++index)
            {
                AppMain.AoActDelete(main_work.act[index]);
                main_work.act[index] = (AppMain.AOS_ACTION)null;
            }
        }
        main_work.cur_disp_page_prev = main_work.cur_disp_page;
        for (int index = num1; index <= num2; ++index)
        {
            if (flag)
            {
                AppMain.A2S_AMA_HEADER ama = AppMain.dm_manual_ama[0];
                AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[0]));
                main_work.act[index] = AppMain.AoActCreate(ama, AppMain.g_dm_act_id_tbl_m[index]);
            }
            if (index != 118)
                AppMain.AoActSortRegAction(main_work.act[index]);
        }
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[1]));
        for (int index = num3; index <= num4; ++index)
        {
            if (flag)
            {
                AppMain.A2S_AMA_HEADER ama = AppMain.dm_manual_ama[1];
                main_work.act[index] = AppMain.AoActCreate(ama, AppMain.g_dm_act_id_tbl_m[index]);
                if (AppMain.GsEnvGetLanguage() == 6)
                {
                    if (AppMain.g_dm_act_id_tbl_m[index] == 9U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 120f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 10U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 125f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 48U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 120f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 49U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 125f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 18U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 90f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 19U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 200f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 52U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 90f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 53U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 200f;
                }
                if (AppMain.GsEnvGetLanguage() == 7)
                {
                    if (AppMain.g_dm_act_id_tbl_m[index] == 9U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 180f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 10U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 185f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 48U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 180f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 49U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 185f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 18U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 280f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 19U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 235f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 52U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 280f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 53U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 235f;
                }
                if (AppMain.GsEnvGetLanguage() == 8)
                {
                    if (AppMain.g_dm_act_id_tbl_m[index] == 9U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 160f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 10U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 245f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 48U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 160f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 49U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 245f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 18U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 19U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 220f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 52U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 53U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 220f;
                }
                if (AppMain.GsEnvGetLanguage() == 9)
                {
                    if (AppMain.g_dm_act_id_tbl_m[index] == 9U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 10U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 225f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 48U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 49U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 225f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 18U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 19U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 52U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 53U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                }
                if (AppMain.GsEnvGetLanguage() == 10)
                {
                    if (AppMain.g_dm_act_id_tbl_m[index] == 9U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 10U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 225f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 48U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 49U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 225f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 18U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 19U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 52U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                    if (AppMain.g_dm_act_id_tbl_m[index] == 53U)
                        ((AppMain.A2S_AMA_ACT)main_work.act[index].data).mtn.trs_tbl[0].trs_x = 110f;
                }
            }
            AppMain.AoActSortRegAction(main_work.act[index]);
        }
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[0]));
        for (int index = num1; index <= num2; ++index)
        {
            if (index != 118)
            {
                AppMain.AoActUpdate(main_work.act[index], 1f);
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
        AppMain.AoActSetTexture(AppMain.AoTexGetTexList(AppMain.dm_manual_tex[1]));
        for (int index = num3; index <= num4; ++index)
        {
            AppMain.AoActUpdate(main_work.act[index], 1f);
            if (main_work.cur_disp_page_prev < 3 || main_work.cur_disp_page_prev > 9 || main_work.cur_disp_page_prev == 7)
                main_work.act[index].sprite.center_y -= 16f;
            if (index == num3 && (main_work.cur_disp_page_prev < 3 || main_work.cur_disp_page_prev > 12))
                main_work.act[index].sprite.center_y += 16f;
            if (index == 143 || index == 144)
                main_work.act[index].sprite.center_y -= 16f;
            else if (index >= 147 && index <= 148 || index >= 151 && index <= 152 || (index == 157 || index == 174 || (index == 170 || index == 161)) || (index == 162 || main_work.cur_disp_page_prev == 11))
                main_work.act[index].sprite.center_y += 16f;
        }
        AppMain.AoActSortExecute();
        AppMain.AoActSortDraw();
        AppMain.AoActSortUnregAll();
    }

    private static int dmManualIsTexLoad()
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.AoTexIsLoaded(AppMain.dm_manual_tex[index]))
                return 0;
        }
        return 1;
    }

    private static int dmManualIsTexRelease()
    {
        for (int index = 0; index < 2; ++index)
        {
            if (!AppMain.AoTexIsReleased(AppMain.dm_manual_tex[index]))
                return 0;
        }
        return 1;
    }

}