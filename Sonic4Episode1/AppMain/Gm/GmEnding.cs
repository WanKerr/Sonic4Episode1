public partial class AppMain
{
    private static void GmEndingTrophySet()
    {
        GMS_ENDING_WORK work = gmEndingGetWork();
        if (g_gm_main_system.ply_work[0] != null)
            GsGetMainSysInfo().clear_ring = work.get_ring;
        else
            GsGetMainSysInfo().clear_ring = 0U;
    }

    public static void GmEndingExit()
    {
        if (gm_ending_tcb == null)
            return;
        mtTaskClearTcb(gm_ending_tcb);
        g_gm_main_system.game_flag &= 4286578687U;
        gm_ending_tcb = null;
    }

    public static void GmEndingStart()
    {
        g_gm_main_system.game_flag |= 8388608U;
        GmFixSetDispEx(false, false, false, true, false);
        gm_ending_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmEndingCtrl), null, 0U, 0, 18448U, 5, () => new GMS_ENDING_WORK(), "ENDING_CTRL");
        GMS_ENDING_WORK work = (GMS_ENDING_WORK)gm_ending_tcb.work;
        work.step = 0;
        work.flag = 1U;
        work.timer = 16U;
        GmCameraAllowSet(0.0f, 50f, 0.0f);
        g_gm_main_system.map_fcol.bottom -= 32;
    }

    public static void GmEndingBuild()
    {
        gm_ending_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(949), GmGameDatGetGimmickData(950), 0U);
    }

    public static void GmEndingFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(949);
        GmGameDBuildRegFlushModel(gm_ending_obj_3d_list, gimmickData.file_num);
    }

    public static GMS_ENDING_WORK gmEndingGetWork()
    {
        return (GMS_ENDING_WORK)gm_ending_tcb.work;
    }

    public static bool GmEndingAnimalForwardChk()
    {
        GMS_ENDING_WORK work = gmEndingGetWork();
        return work.type != 0 && work.step >= 7;
    }

    public static void GmEndingPlyKeyCustom(GMS_PLAYER_WORK ply_work)
    {
        GMS_ENDING_WORK work = gmEndingGetWork();
        if (((int)work.flag & 1) != 0)
        {
            ply_work.key_on = 0;
            ply_work.key_push = 0;
            ply_work.key_release = 0;
        }
        else
        {
            ply_work.key_on &= 160;
            ply_work.key_push &= 160;
            ply_work.key_release &= 160;
        }
        ply_work.key_rot_z = ply_work.key_walk_rot_z = 0;
        if (((int)work.flag & 2) != 0)
        {
            ply_work.key_on |= 4;
            ply_work.key_rot_z = ply_work.key_walk_rot_z = -32767;
        }
        else
        {
            if (((int)work.flag & 4) == 0)
                return;
            ply_work.key_on |= 8;
            ply_work.key_rot_z = ply_work.key_walk_rot_z = short.MaxValue;
        }
    }

    private static void GmEndingPlyNopSet()
    {
        GMS_ENDING_WORK work = gmEndingGetWork();
        if (work.step != 1)
            return;
        work.step = 2;
        g_gm_main_system.ply_work[0].obj_work.spd_m = -36864;
    }

    private static void GmEndingPlyBrakeSet()
    {
        GMS_ENDING_WORK work = gmEndingGetWork();
        if (work.step != 2)
            return;
        work.step = 3;
        g_gm_main_system.ply_work[0].obj_work.pos.x = 2080768;
        g_gm_main_system.ply_work[0].obj_work.spd_m = -36864;
    }

    private static void gmEndingLastPic(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        obj_work.disp_flag |= 32U;
        if (gmsPlayerWork.act_state != 80 && gmsPlayerWork.act_state != 82 && gmsPlayerWork.act_state != 84)
            return;
        obj_work.disp_flag &= 4294967263U;
        gmsPlayerWork.obj_work.disp_flag |= 32U;
    }

    private static void gmEndingLastPicInit()
    {
        GMS_ENDING_WORK work1 = gmEndingGetWork();
        OBS_OBJECT_WORK work2 = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DNN_WORK(), g_gm_main_system.ply_work[0].obj_work, 0, "END_PIC");
        GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (GMS_EFFECT_3DNN_WORK)work2;
        work2.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEndingLastPic);
        ObjObjectCopyAction3dNNModel(work2, gm_ending_obj_3d_list[work1.type], gmsEffect3DnnWork.obj_3d);
        work2.move_flag |= 16128U;
        work2.disp_flag |= 4194336U;
        work2.flag |= 1026U;
        work2.scale.x = work2.scale.y = work2.scale.z = 5120;
        work2.parent_ofst.y = gm_ending_obj_offset[work1.type];
    }

    private static void gmEndingCtrl(MTS_TASK_TCB tcb)
    {
        GMS_ENDING_WORK work = (GMS_ENDING_WORK)tcb.work;
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        switch (work.step)
        {
            case 0:
                if (work.timer != 0U)
                {
                    --work.timer;
                    break;
                }
                g_gm_main_system.game_flag |= 1024U;
                IzFadeInitEasy(1U, 2U, 60f);
                work.step = 1;
                work.flag = 2U;
                GmSoundPlayStageBGM(0);
                AoPresenceSet();
                break;
            case 2:
                work.flag |= 1U;
                ply_work.spd_jump_add = ply_work.spd_add;
                break;
            case 3:
                ply_work.obj_work.disp_flag |= 1U;
                if (((int)ply_work.obj_work.move_flag & 1) != 0 && ply_work.obj_work.spd_m > -4096)
                {
                    if (amTpIsTouchOn(0))
                    {
                        ply_work.ring_num = (short)GmEventMgrGetRingNum();
                        if (amTpIsTouchOn(1))
                            g_gs_main_sys_info.game_flag |= 32U;
                        else
                            g_gs_main_sys_info.game_flag &= 4294967263U;
                    }
                    work.get_ring = (uint)ply_work.ring_num;
                    if (work.get_ring < GmEventMgrGetRingNum())
                    {
                        work.step = 4;
                        work.type = 0;
                    }
                    else if (((int)g_gs_main_sys_info.game_flag & 32) != 0)
                    {
                        work.step = 5;
                        work.type = 2;
                    }
                    else
                    {
                        work.step = 4;
                        work.type = 1;
                    }
                    work.flag &= 4294967291U;
                    gmEndingLastPicInit();
                    break;
                }
                work.flag &= 4294967293U;
                work.flag |= 4U;
                break;
            case 4:
                if (g_gm_main_system.game_time <= 720U || ply_work.seq_state == 62)
                    break;
                GmPlySeqGmkInitEndingDemo1(ply_work);
                work.step = 6;
                break;
            case 5:
                if (g_gm_main_system.game_time <= 720U || ply_work.seq_state == 24)
                    break;
                GmPlySeqChangeTransformSuper(ply_work);
                work.step = 6;
                break;
            case 6:
                if (g_gm_main_system.game_time <= 900U || ply_work.seq_state == 63)
                    break;
                bool type2 = false;
                if (work.type == 1)
                    type2 = true;
                GmPlySeqGmkInitEndingDemo2(ply_work, type2);
                work.step = 7;
                break;
            case 7:
                if (g_gm_main_system.game_time <= 1140U)
                    break;
                IzFadeInitEasy(0U, 1U, 32f);
                work.step = 8;
                break;
            case 8:
                if (!IzFadeIsEnd())
                    break;
                GmMainEnd();
                SyDecideEvtCase(0);
                SyChangeNextEvt();
                break;
        }
    }

}