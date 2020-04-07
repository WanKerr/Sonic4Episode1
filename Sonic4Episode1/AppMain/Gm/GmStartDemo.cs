using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static void GmStartDemoBuild()
    {
        AppMain.gmStartDemoDataInit();
        AppMain.GMS_START_DEMO_DATA info = AppMain.gmStartDemoDataGetInfo();
        for (int index = 0; 2 > index; ++index)
        {
            int language = AppMain.GsEnvGetLanguage();
            info.demo_amb[index] = AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, AppMain.g_gm_start_demo_data_amb_id[language][index], AppMain.GmGameDatGetCockpitData());
        }
        info.flag_regist = false;
    }

    private static bool GmStartDemoBuildCheck()
    {
        AppMain.GMS_START_DEMO_DATA info = AppMain.gmStartDemoDataGetInfo();
        if (!info.flag_regist)
        {
            if (AppMain.GsMainSysGetDisplayListRegistNum() >= 192)
                return false;
            for (int index = 0; 2 > index; ++index)
            {
                AppMain.AoTexBuild(info.aos_texture[index], (AppMain.AMS_AMB_HEADER)info.demo_amb[index]);
                AppMain.AoTexLoad(info.aos_texture[index]);
            }
            info.flag_regist = true;
        }
        return AppMain.gmStartDemoDataCheckLoading();
    }

    private static void GmStartDemoFlush()
    {
        AppMain.GMS_START_DEMO_DATA info = AppMain.gmStartDemoDataGetInfo();
        for (int index = 0; 2 > index; ++index)
            AppMain.AoTexRelease(info.aos_texture[index]);
    }

    private static bool GmStartDemoFlushCheck()
    {
        if (!AppMain.gmStartDemoDataCheckRelease())
            return false;
        AppMain.gmStartDemoDataFlush();
        return true;
    }

    private static void GmStartDemoStart()
    {
        AppMain.gmStartDemoInit();
        AppMain.gmStartDemoSetGameFlag(4096U);
    }

    private static void GmStartDemoExit()
    {
        AppMain.gmStartDemoExit();
    }

    private static void gmStartDemoDataInit()
    {
        AppMain.g_start_demo_data_real.Clear();
        AppMain.g_start_demo_data = AppMain.g_start_demo_data_real;
    }

    private static void gmStartDemoDataFlush()
    {
        if (AppMain.g_start_demo_data == null)
            return;
        AppMain.g_start_demo_data = (AppMain.GMS_START_DEMO_DATA)null;
    }

    private static AppMain.GMS_START_DEMO_DATA gmStartDemoDataGetInfo()
    {
        return AppMain.g_start_demo_data;
    }

    private static bool gmStartDemoDataCheckLoading()
    {
        int num = 0;
        AppMain.GMS_START_DEMO_DATA info = AppMain.gmStartDemoDataGetInfo();
        for (int index = 0; 2 > index; ++index)
        {
            if (AppMain.AoTexIsLoaded(info.aos_texture[index]))
                num |= 1 << index;
        }
        return 3 == num;
    }

    private static bool gmStartDemoDataCheckRelease()
    {
        int num = 0;
        AppMain.GMS_START_DEMO_DATA info = AppMain.gmStartDemoDataGetInfo();
        if (info == null)
            return true;
        for (int index = 0; 2 > index; ++index)
        {
            if (AppMain.AoTexIsReleased(info.aos_texture[index]))
                num |= 1 << index;
        }
        return 3 == num;
    }

    private static AppMain.GMS_START_DEMO_MGR gmStartDemoMgrGetInfo()
    {
        return AppMain.g_start_demo_mgr;
    }

    private static void gmStartDemoInit()
    {
        AppMain.g_start_demo_mgr_real.Clear();
        AppMain.g_start_demo_mgr = AppMain.g_start_demo_mgr_real;
        AppMain.g_start_demo_mgr.main_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmStartDemoProcMain), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 18448U, 5, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_START_DEMO_WORK()), "START_DEMO_MAIN");
        AppMain.GMS_START_DEMO_WORK work = (AppMain.GMS_START_DEMO_WORK)AppMain.g_start_demo_mgr.main_tcb.work;
        work.counter = 0U;
        work.update = new AppMain.GMS_START_DEMO_WORK._update_(AppMain.gmStartDemoProcFade);
        AppMain.gmStartDemo2DActionCreate(work);
        AppMain.GmPlySeqInitDemoFw(AppMain.g_gm_main_system.ply_work[0]);
        AppMain.IzFadeInitEasy(1U, 2U, 30f);
    }

    private static void gmStartDemoRequestExit()
    {
        AppMain.GMS_START_DEMO_MGR info = AppMain.gmStartDemoMgrGetInfo();
        if (info == null)
            return;
        if (info.main_tcb != null)
        {
            AppMain.mtTaskClearTcb(info.main_tcb);
            info.main_tcb = (AppMain.MTS_TASK_TCB)null;
        }
        AppMain.gmStartDemoClearGameFlag(4096U);
    }

    private static void gmStartDemoExit()
    {
        if (AppMain.g_start_demo_mgr == null)
            return;
        AppMain.gmStartDemoRequestExit();
        AppMain.g_start_demo_mgr = (AppMain.GMS_START_DEMO_MGR)null;
    }

    private static void gmStartDemoProcMain(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_START_DEMO_WORK work = (AppMain.GMS_START_DEMO_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
        {
            AppMain.gmStartDemoRequestExit();
        }
        else
        {
            if (work.update != null)
                work.update(work);
            ++work.counter;
        }
    }

    private static void gmStartDemoSetGameFlag(uint flag)
    {
        AppMain.g_gm_main_system.game_flag |= flag;
    }

    private static void gmStartDemoClearGameFlag(uint flag)
    {
        AppMain.g_gm_main_system.game_flag &= ~flag;
    }

    private static void gmStartDemo2DActionCreate(AppMain.GMS_START_DEMO_WORK work)
    {
        AppMain.GMS_START_DEMO_DATA info = AppMain.gmStartDemoDataGetInfo();
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        int language = AppMain.GsEnvGetLanguage();
        for (int index1 = 0; 4 > index1; ++index1)
        {
            int index2 = AppMain.g_gm_start_demo_data_type_cmn[index1];
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = AppMain.gmStartDemo2DActionCreate(AppMain.g_gm_start_demo_action_name_cmn[index1], info.aos_texture[index2], AppMain.g_gm_start_demo_data_ama_id[language][index2], AppMain.g_gm_start_demo_action_id_cmn[index1], AppMain.g_gm_start_demo_action_node_flag_cmn[index1]);
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.obj_2d.speed = 0.0f;
            work.action_obj_work_cmn[index1] = gmsCockpit2DWork;
        }
        int index3 = AppMain.g_gm_gamedat_zone_type_tbl[(int)mainSysInfo.stage_id];
        for (int index1 = 0; 1 > index1; ++index1)
        {
            int index2 = AppMain.g_gm_start_demo_data_type_zone[index1];
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = AppMain.gmStartDemo2DActionCreate(AppMain.g_gm_start_demo_action_name_zone[index1], info.aos_texture[index2], AppMain.g_gm_start_demo_data_ama_id[language][index2], AppMain.g_gm_start_demo_action_id_zone[index3][index1].Value, AppMain.g_gm_start_demo_action_node_flag_zone[index1]);
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.obj_2d.speed = 0.0f;
            work.action_obj_work_zone[index1] = gmsCockpit2DWork;
        }
        int index4 = AppMain.g_gm_start_demo_act_no[(int)mainSysInfo.stage_id];
        if (index3 != 4)
        {
            for (int index1 = 0; 2 > index1; ++index1)
            {
                int index2 = AppMain.g_gm_start_demo_data_type_act[index1];
                AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = AppMain.gmStartDemo2DActionCreate(AppMain.g_gm_start_demo_action_name_act[index1], info.aos_texture[index2], AppMain.g_gm_start_demo_data_ama_id[language][index2], AppMain.g_gm_start_demo_action_id_act[index4][index1], AppMain.g_gm_start_demo_action_node_flag_act[index1]);
                if (gmsCockpit2DWork != null)
                    gmsCockpit2DWork.obj_2d.speed = 0.0f;
                work.action_obj_work_act[index1] = gmsCockpit2DWork;
            }
        }
        int index5 = 1;
        AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork1 = AppMain.gmStartDemo2DActionCreate(AppMain.g_gm_start_demo_action_name_message, info.aos_texture[index5], AppMain.g_gm_start_demo_data_ama_id[language][index5], AppMain.g_gm_start_demo_action_id_message[index3][index4], 1);
        if (gmsCockpit2DWork1 != null)
            gmsCockpit2DWork1.obj_2d.speed = 0.0f;
        work.action_obj_work_message = gmsCockpit2DWork1;
    }

    private static AppMain.GMS_COCKPIT_2D_WORK gmStartDemo2DActionCreate(
      string tcb_name,
      AppMain.AOS_TEXTURE aos_texture,
      int ama_id,
      int action_id,
      int node_flag)
    {
        if (action_id == -1)
            return (AppMain.GMS_COCKPIT_2D_WORK)null;
        AppMain.GMS_COCKPIT_2D_WORK work = (AppMain.GMS_COCKPIT_2D_WORK)AppMain.GMM_COCKPIT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_COCKPIT_2D_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, tcb_name);
        work.cpit_com.obj_work.disp_flag |= (uint)node_flag;
        AppMain.ObjObjectAction2dAMALoadSetTexlist(work.cpit_com.obj_work, work.obj_2d, (AppMain.OBS_DATA_WORK)null, (string)null, ama_id, AppMain.GmGameDatGetCockpitData(), AppMain.AoTexGetTexList(aos_texture), (uint)action_id, node_flag);
        ((AppMain.OBS_OBJECT_WORK)work).pos.z -= AppMain.FX_F32_TO_FX32(10f);
        return work;
    }

    private static void gmStartDemoProcFade(AppMain.GMS_START_DEMO_WORK work)
    {
        if (!AppMain.IzFadeIsEnd())
            return;
        AppMain.IzFadeExit();
        work.update = new AppMain.GMS_START_DEMO_WORK._update_(AppMain.gmStartDemoProcIn);
        work.counter = 0U;
        for (int index = 0; 4 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_cmn[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 1f;
                gmsCockpit2DWork.obj_2d.frame = 0.0f;
            }
        }
        for (int index = 0; 1 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_zone[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 1f;
                gmsCockpit2DWork.obj_2d.frame = 0.0f;
            }
        }
        for (int index = 0; 2 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_act[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 1f;
                gmsCockpit2DWork.obj_2d.frame = 0.0f;
            }
        }
        AppMain.GMS_COCKPIT_2D_WORK actionObjWorkMessage = work.action_obj_work_message;
        if (actionObjWorkMessage == null)
            return;
        actionObjWorkMessage.obj_2d.speed = 1f;
        actionObjWorkMessage.obj_2d.frame = 0.0f;
    }

    private static void gmStartDemoProcIn(AppMain.GMS_START_DEMO_WORK work)
    {
        if (work.counter < 39U)
            return;
        work.update = new AppMain.GMS_START_DEMO_WORK._update_(AppMain.gmStartDemoProcWait);
        for (int index = 0; 4 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_cmn[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 0.0f;
                gmsCockpit2DWork.obj_2d.frame = 40f;
            }
        }
        for (int index = 0; 1 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_zone[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 0.0f;
                gmsCockpit2DWork.obj_2d.frame = 40f;
            }
        }
        for (int index = 0; 2 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_act[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 0.0f;
                gmsCockpit2DWork.obj_2d.frame = 40f;
            }
        }
        AppMain.GMS_COCKPIT_2D_WORK actionObjWorkMessage = work.action_obj_work_message;
        if (actionObjWorkMessage == null)
            return;
        actionObjWorkMessage.obj_2d.speed = 0.0f;
        actionObjWorkMessage.obj_2d.frame = 40f;
    }

    public static bool GmStartMsgIsExe()
    {
        bool flag = false;
        AppMain.GSS_MAIN_SYS_INFO mainSysInfo = AppMain.GsGetMainSysInfo();
        if (((int)AppMain.g_gs_main_sys_info.game_flag & 4) == 0)
        {
            SSystem instance = SSystem.CreateInstance();
            switch (mainSysInfo.stage_id)
            {
                case 5:
                    flag = !AppMain.GsMainSysIsStageClear(5);
                    break;
                case 9:
                    if (!AppMain.GsMainSysIsStageClear(9))
                        flag = true;
                    else if ((512 & (int)mainSysInfo.game_flag) != 0)
                    {
                        if (!instance.IsAnnounce(SSystem.EAnnounce.TruckFlick))
                            flag = true;
                    }
                    else if (!instance.IsAnnounce(SSystem.EAnnounce.TruckTilt))
                        flag = true;
                    if (flag)
                    {
                        if ((512 & (int)mainSysInfo.game_flag) != 0)
                        {
                            instance.SetAnnounce(SSystem.EAnnounce.TruckFlick, true);
                            break;
                        }
                        instance.SetAnnounce(SSystem.EAnnounce.TruckTilt, true);
                        break;
                    }
                    break;
                case 21:
                    if (!AppMain.GsMainSysIsStageClear(21))
                    {
                        flag = true;
                        goto case 22;
                    }
                    else
                        goto case 22;
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                    if (!flag)
                    {
                        if ((512 & (int)mainSysInfo.game_flag) != 0)
                        {
                            if (!instance.IsAnnounce(SSystem.EAnnounce.SpecialStageFlick))
                                flag = true;
                        }
                        else if (!instance.IsAnnounce(SSystem.EAnnounce.SpecialStageTilt))
                            flag = true;
                    }
                    if (flag)
                    {
                        if ((512 & (int)mainSysInfo.game_flag) != 0)
                        {
                            instance.SetAnnounce(SSystem.EAnnounce.SpecialStageFlick, true);
                            break;
                        }
                        instance.SetAnnounce(SSystem.EAnnounce.SpecialStageTilt, true);
                        break;
                    }
                    break;
            }
        }
        if (flag)
            AppMain.GmMainClearSuspendedPause();
        return flag;
    }

    private static void gmStartDemoProcWait(AppMain.GMS_START_DEMO_WORK work)
    {
        if (work.counter < 160U)
            return;
        work.update = new AppMain.GMS_START_DEMO_WORK._update_(AppMain.gmStartDemoProcOut);
        if (AppMain.GmStartMsgIsExe())
        {
            AppMain.GmStartMsgInit();
        }
        else
        {
            AppMain.GmPlySeqChangeSequence(AppMain.g_gm_main_system.ply_work[0], 0);
            AppMain.gmStartDemoSetGameFlag(1024U);
        }
        for (int index = 0; 4 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_cmn[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.obj_2d.speed = 1f;
        }
        for (int index = 0; 1 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_zone[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.obj_2d.speed = 1f;
        }
        for (int index = 0; 2 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_act[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.obj_2d.speed = 1f;
        }
        AppMain.GMS_COCKPIT_2D_WORK actionObjWorkMessage = work.action_obj_work_message;
        if (actionObjWorkMessage == null)
            return;
        actionObjWorkMessage.obj_2d.speed = 1f;
    }

    private static void gmStartDemoProcOut(AppMain.GMS_START_DEMO_WORK work)
    {
        if (work.counter <= 230U)
            return;
        work.update = new AppMain.GMS_START_DEMO_WORK._update_(AppMain.gmStartDemoProcEnd);
    }

    private static void gmStartDemoProcEnd(AppMain.GMS_START_DEMO_WORK work)
    {
        work.update = (AppMain.GMS_START_DEMO_WORK._update_)null;
        AppMain.gmStartDemoRequestExit();
        for (int index = 0; 4 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_cmn[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.cpit_com.obj_work.flag |= 8U;
        }
        for (int index = 0; 1 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_zone[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.cpit_com.obj_work.flag |= 8U;
        }
        for (int index = 0; 2 > index; ++index)
        {
            AppMain.GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_act[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.cpit_com.obj_work.flag |= 8U;
        }
        AppMain.GMS_COCKPIT_2D_WORK actionObjWorkMessage = work.action_obj_work_message;
        if (actionObjWorkMessage == null)
            return;
        actionObjWorkMessage.cpit_com.obj_work.flag |= 8U;
    }





}