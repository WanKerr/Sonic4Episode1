using gs.backup;

public partial class AppMain
{
    private static void GmStartDemoBuild()
    {
        gmStartDemoDataInit();
        GMS_START_DEMO_DATA info = gmStartDemoDataGetInfo();
        for (int index = 0; 2 > index; ++index)
        {
            int language = GsEnvGetLanguage();
            info.demo_amb[index] = ObjDataLoadAmbIndex(null, g_gm_start_demo_data_amb_id[language][index], GmGameDatGetCockpitData());
        }
        info.flag_regist = false;
    }

    private static bool GmStartDemoBuildCheck()
    {
        GMS_START_DEMO_DATA info = gmStartDemoDataGetInfo();
        if (!info.flag_regist)
        {
            if (GsMainSysGetDisplayListRegistNum() >= 192)
                return false;
            for (int index = 0; 2 > index; ++index)
            {
                AoTexBuild(info.aos_texture[index], (AMS_AMB_HEADER)info.demo_amb[index]);
                AoTexLoad(info.aos_texture[index]);
            }
            info.flag_regist = true;
        }
        return gmStartDemoDataCheckLoading();
    }

    private static void GmStartDemoFlush()
    {
        GMS_START_DEMO_DATA info = gmStartDemoDataGetInfo();
        for (int index = 0; 2 > index; ++index)
            AoTexRelease(info.aos_texture[index]);
    }

    private static bool GmStartDemoFlushCheck()
    {
        if (!gmStartDemoDataCheckRelease())
            return false;
        gmStartDemoDataFlush();
        return true;
    }

    private static void GmStartDemoStart()
    {
        gmStartDemoInit();
        gmStartDemoSetGameFlag(4096U);
    }

    private static void GmStartDemoExit()
    {
        gmStartDemoExit();
    }

    private static void gmStartDemoDataInit()
    {
        g_start_demo_data_real.Clear();
        g_start_demo_data = g_start_demo_data_real;
    }

    private static void gmStartDemoDataFlush()
    {
        if (g_start_demo_data == null)
            return;
        g_start_demo_data = null;
    }

    private static GMS_START_DEMO_DATA gmStartDemoDataGetInfo()
    {
        return g_start_demo_data;
    }

    private static bool gmStartDemoDataCheckLoading()
    {
        int num = 0;
        GMS_START_DEMO_DATA info = gmStartDemoDataGetInfo();
        for (int index = 0; 2 > index; ++index)
        {
            if (AoTexIsLoaded(info.aos_texture[index]))
                num |= 1 << index;
        }
        return 3 == num;
    }

    private static bool gmStartDemoDataCheckRelease()
    {
        int num = 0;
        GMS_START_DEMO_DATA info = gmStartDemoDataGetInfo();
        if (info == null)
            return true;
        for (int index = 0; 2 > index; ++index)
        {
            if (AoTexIsReleased(info.aos_texture[index]))
                num |= 1 << index;
        }
        return 3 == num;
    }

    private static GMS_START_DEMO_MGR gmStartDemoMgrGetInfo()
    {
        return g_start_demo_mgr;
    }

    private static void gmStartDemoInit()
    {
        g_start_demo_mgr_real.Clear();
        g_start_demo_mgr = g_start_demo_mgr_real;
        g_start_demo_mgr.main_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmStartDemoProcMain), null, 0U, 0, 18448U, 5, () => new GMS_START_DEMO_WORK(), "START_DEMO_MAIN");
        GMS_START_DEMO_WORK work = (GMS_START_DEMO_WORK)g_start_demo_mgr.main_tcb.work;
        work.counter = 0U;
        work.update = new GMS_START_DEMO_WORK._update_(gmStartDemoProcFade);
        gmStartDemo2DActionCreate(work);
        GmPlySeqInitDemoFw(g_gm_main_system.ply_work[0]);
        IzFadeInitEasy(1U, 2U, 30f);
    }

    private static void gmStartDemoRequestExit()
    {
        GMS_START_DEMO_MGR info = gmStartDemoMgrGetInfo();
        if (info == null)
            return;
        if (info.main_tcb != null)
        {
            mtTaskClearTcb(info.main_tcb);
            info.main_tcb = null;
        }
        gmStartDemoClearGameFlag(4096U);
    }

    private static void gmStartDemoExit()
    {
        if (g_start_demo_mgr == null)
            return;
        gmStartDemoRequestExit();
        g_start_demo_mgr = null;
    }

    private static void gmStartDemoProcMain(MTS_TASK_TCB tcb)
    {
        GMS_START_DEMO_WORK work = (GMS_START_DEMO_WORK)tcb.work;
        if (((int)work.flag & 1) != 0)
        {
            gmStartDemoRequestExit();
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
        g_gm_main_system.game_flag |= flag;
    }

    private static void gmStartDemoClearGameFlag(uint flag)
    {
        g_gm_main_system.game_flag &= ~flag;
    }

    private static void gmStartDemo2DActionCreate(GMS_START_DEMO_WORK work)
    {
        GMS_START_DEMO_DATA info = gmStartDemoDataGetInfo();
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        int language = GsEnvGetLanguage();
        for (int index1 = 0; 4 > index1; ++index1)
        {
            int index2 = g_gm_start_demo_data_type_cmn[index1];
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = gmStartDemo2DActionCreate(g_gm_start_demo_action_name_cmn[index1], info.aos_texture[index2], g_gm_start_demo_data_ama_id[language][index2], g_gm_start_demo_action_id_cmn[index1], g_gm_start_demo_action_node_flag_cmn[index1]);
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.obj_2d.speed = 0.0f;
            work.action_obj_work_cmn[index1] = gmsCockpit2DWork;
        }
        int index3 = g_gm_gamedat_zone_type_tbl[mainSysInfo.stage_id];
        for (int index1 = 0; 1 > index1; ++index1)
        {
            int index2 = g_gm_start_demo_data_type_zone[index1];
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = gmStartDemo2DActionCreate(g_gm_start_demo_action_name_zone[index1], info.aos_texture[index2], g_gm_start_demo_data_ama_id[language][index2], g_gm_start_demo_action_id_zone[index3][index1].Value, g_gm_start_demo_action_node_flag_zone[index1]);
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.obj_2d.speed = 0.0f;
            work.action_obj_work_zone[index1] = gmsCockpit2DWork;
        }
        int index4 = g_gm_start_demo_act_no[mainSysInfo.stage_id];
        if (index3 != 4)
        {
            for (int index1 = 0; 2 > index1; ++index1)
            {
                int index2 = g_gm_start_demo_data_type_act[index1];
                GMS_COCKPIT_2D_WORK gmsCockpit2DWork = gmStartDemo2DActionCreate(g_gm_start_demo_action_name_act[index1], info.aos_texture[index2], g_gm_start_demo_data_ama_id[language][index2], g_gm_start_demo_action_id_act[index4][index1], g_gm_start_demo_action_node_flag_act[index1]);
                if (gmsCockpit2DWork != null)
                    gmsCockpit2DWork.obj_2d.speed = 0.0f;
                work.action_obj_work_act[index1] = gmsCockpit2DWork;
            }
        }
        int index5 = 1;
        GMS_COCKPIT_2D_WORK gmsCockpit2DWork1 = gmStartDemo2DActionCreate(g_gm_start_demo_action_name_message, info.aos_texture[index5], g_gm_start_demo_data_ama_id[language][index5], g_gm_start_demo_action_id_message[index3][index4], 1);
        if (gmsCockpit2DWork1 != null)
            gmsCockpit2DWork1.obj_2d.speed = 0.0f;
        work.action_obj_work_message = gmsCockpit2DWork1;
    }

    private static GMS_COCKPIT_2D_WORK gmStartDemo2DActionCreate(
      string tcb_name,
      AOS_TEXTURE aos_texture,
      int ama_id,
      int action_id,
      int node_flag)
    {
        if (action_id == -1)
            return null;
        GMS_COCKPIT_2D_WORK work = (GMS_COCKPIT_2D_WORK)GMM_COCKPIT_CREATE_WORK(() => new GMS_COCKPIT_2D_WORK(), null, 0, tcb_name);
        work.cpit_com.obj_work.disp_flag |= (uint)node_flag;
        ObjObjectAction2dAMALoadSetTexlist(work.cpit_com.obj_work, work.obj_2d, null, null, ama_id, GmGameDatGetCockpitData(), AoTexGetTexList(aos_texture), (uint)action_id, node_flag);
        ((OBS_OBJECT_WORK)work).pos.z -= FX_F32_TO_FX32(10f);
        return work;
    }

    private static void gmStartDemoProcFade(GMS_START_DEMO_WORK work)
    {
        if (!IzFadeIsEnd())
            return;
        IzFadeExit();
        work.update = new GMS_START_DEMO_WORK._update_(gmStartDemoProcIn);
        work.counter = 0U;
        for (int index = 0; 4 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_cmn[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 1f;
                gmsCockpit2DWork.obj_2d.frame = 0.0f;
            }
        }
        for (int index = 0; 1 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_zone[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 1f;
                gmsCockpit2DWork.obj_2d.frame = 0.0f;
            }
        }
        for (int index = 0; 2 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_act[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 1f;
                gmsCockpit2DWork.obj_2d.frame = 0.0f;
            }
        }
        GMS_COCKPIT_2D_WORK actionObjWorkMessage = work.action_obj_work_message;
        if (actionObjWorkMessage == null)
            return;
        actionObjWorkMessage.obj_2d.speed = 1f;
        actionObjWorkMessage.obj_2d.frame = 0.0f;
    }

    private static void gmStartDemoProcIn(GMS_START_DEMO_WORK work)
    {
        if (work.counter < 39U)
            return;
        work.update = new GMS_START_DEMO_WORK._update_(gmStartDemoProcWait);
        for (int index = 0; 4 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_cmn[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 0.0f;
                gmsCockpit2DWork.obj_2d.frame = 40f;
            }
        }
        for (int index = 0; 1 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_zone[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 0.0f;
                gmsCockpit2DWork.obj_2d.frame = 40f;
            }
        }
        for (int index = 0; 2 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_act[index];
            if (gmsCockpit2DWork != null)
            {
                gmsCockpit2DWork.obj_2d.speed = 0.0f;
                gmsCockpit2DWork.obj_2d.frame = 40f;
            }
        }
        GMS_COCKPIT_2D_WORK actionObjWorkMessage = work.action_obj_work_message;
        if (actionObjWorkMessage == null)
            return;
        actionObjWorkMessage.obj_2d.speed = 0.0f;
        actionObjWorkMessage.obj_2d.frame = 40f;
    }

    public static bool GmStartMsgIsExe()
    {
        bool flag = false;
        GSS_MAIN_SYS_INFO mainSysInfo = GsGetMainSysInfo();
        if (((int)g_gs_main_sys_info.game_flag & 4) == 0)
        {
            SSystem instance = SSystem.CreateInstance();
            switch (mainSysInfo.stage_id)
            {
                case 5:
                    flag = !GsMainSysIsStageClear(5);
                    break;
                case 9:
                    if (!GsMainSysIsStageClear(9))
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
                    if (!GsMainSysIsStageClear(21))
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
            GmMainClearSuspendedPause();
        return flag;
    }

    private static void gmStartDemoProcWait(GMS_START_DEMO_WORK work)
    {
        if (work.counter < 160U)
            return;
        work.update = new GMS_START_DEMO_WORK._update_(gmStartDemoProcOut);
        if (GmStartMsgIsExe())
        {
            GmStartMsgInit();
        }
        else
        {
            GmPlySeqChangeSequence(g_gm_main_system.ply_work[0], 0);
            gmStartDemoSetGameFlag(1024U);
        }
        for (int index = 0; 4 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_cmn[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.obj_2d.speed = 1f;
        }
        for (int index = 0; 1 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_zone[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.obj_2d.speed = 1f;
        }
        for (int index = 0; 2 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_act[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.obj_2d.speed = 1f;
        }
        GMS_COCKPIT_2D_WORK actionObjWorkMessage = work.action_obj_work_message;
        if (actionObjWorkMessage == null)
            return;
        actionObjWorkMessage.obj_2d.speed = 1f;
    }

    private static void gmStartDemoProcOut(GMS_START_DEMO_WORK work)
    {
        if (work.counter <= 230U)
            return;
        work.update = new GMS_START_DEMO_WORK._update_(gmStartDemoProcEnd);
    }

    private static void gmStartDemoProcEnd(GMS_START_DEMO_WORK work)
    {
        work.update = null;
        gmStartDemoRequestExit();
        for (int index = 0; 4 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_cmn[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.cpit_com.obj_work.flag |= 8U;
        }
        for (int index = 0; 1 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_zone[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.cpit_com.obj_work.flag |= 8U;
        }
        for (int index = 0; 2 > index; ++index)
        {
            GMS_COCKPIT_2D_WORK gmsCockpit2DWork = work.action_obj_work_act[index];
            if (gmsCockpit2DWork != null)
                gmsCockpit2DWork.cpit_com.obj_work.flag |= 8U;
        }
        GMS_COCKPIT_2D_WORK actionObjWorkMessage = work.action_obj_work_message;
        if (actionObjWorkMessage == null)
            return;
        actionObjWorkMessage.cpit_com.obj_work.flag |= 8U;
    }





}