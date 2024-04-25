using System;

public partial class AppMain
{
    private static int GMM_BOSS5_AREA_LEFT()
    {
        return g_gm_main_system.map_fcol.left << 12;
    }

    private static int GMM_BOSS5_AREA_TOP()
    {
        return g_gm_main_system.map_fcol.top << 12;
    }

    private static int GMM_BOSS5_AREA_RIGHT()
    {
        return g_gm_main_system.map_fcol.right << 12;
    }

    private static int GMM_BOSS5_AREA_BOTTOM()
    {
        return g_gm_main_system.map_fcol.bottom << 12;
    }

    private static int GMM_BOSS5_AREA_CENTER_X()
    {
        return GMM_BOSS5_AREA_LEFT() + (GMM_BOSS5_AREA_RIGHT() - GMM_BOSS5_AREA_LEFT()) / 2;
    }

    private static int GMM_BOSS5_AREA_CENTER_Y()
    {
        return GMM_BOSS5_AREA_TOP() + (GMM_BOSS5_AREA_BOTTOM() - GMM_BOSS5_AREA_TOP()) / 2;
    }

    private static void GmBoss5Build()
    {
        gm_boss5_obj_3d_list = GmGameDBuildRegBuildModel((AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, g_gm_gamedat_enemy_arc), (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 1, g_gm_gamedat_enemy_arc), 0U);
        ObjDataLoadAmbIndex(ObjDataGet(745), 2, g_gm_gamedat_enemy_arc);
        ObjDataLoadAmbIndex(ObjDataGet(746), 4, g_gm_gamedat_enemy_arc);
        ObjDataLoadAmbIndex(ObjDataGet(747), 8, g_gm_gamedat_enemy_arc);
        ObjDataLoadAmbIndex(ObjDataGet(748), 3, g_gm_gamedat_enemy_arc);
        ObjDataLoadAmbIndex(ObjDataGet(749), 5, g_gm_gamedat_enemy_arc);
        ObjDataLoadAmbIndex(ObjDataGet(750), 6, g_gm_gamedat_enemy_arc);
        ObjDataLoadAmbIndex(ObjDataGet(751), 7, g_gm_gamedat_enemy_arc);
        GmBoss5EfctBuild();
    }

    private static void GmBoss5Flush()
    {
        GmBoss5EfctFlush();
        ObjDataRelease(ObjDataGet(751));
        ObjDataRelease(ObjDataGet(750));
        ObjDataRelease(ObjDataGet(749));
        ObjDataRelease(ObjDataGet(748));
        ObjDataRelease(ObjDataGet(747));
        ObjDataRelease(ObjDataGet(746));
        ObjDataRelease(ObjDataGet(745));
        AMS_AMB_HEADER amsAmbHeader = (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, g_gm_gamedat_enemy_arc);
        GmGameDBuildRegFlushModel(gm_boss5_obj_3d_list, amsAmbHeader.file_num);
        gm_boss5_obj_3d_list = null;
    }

    private static OBS_OBJECT_WORK GmBoss5Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS5_MGR_WORK(), "BOSS5_MGR");
        GMS_BOSS5_MGR_WORK gmsBosS5MgrWork = (GMS_BOSS5_MGR_WORK)work;
        work.flag |= 16U;
        work.disp_flag |= 32U;
        work.move_flag |= 8448U;
        gmsBosS5MgrWork.ene_3d.ene_com.enemy_flag |= 32768U;
        gmsBosS5MgrWork.life = GMD_BOSS5_LIFE;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5MgrWaitLoad);
        return work;
    }

    private static OBS_OBJECT_WORK GmBoss5BodyInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS5_BODY_WORK(), "BOSS5_BODY");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS5_BODY_WORK body_work = (GMS_BOSS5_BODY_WORK)work;
        work.pos.z = GMD_BOSS5_DEFAULT_POS_Z;
        body_work.ground_v_pos = pos_y;
        gmsEnemy3DWork.ene_com.vit = 1;
        ObjObjectFieldRectSet(work, GMD_BOSS5_BODY_DEFAULT_FIELD_RECT_SIZE_LEFT, GMD_BOSS5_BODY_DEFAULT_FIELD_RECT_SIZE_TOP, GMD_BOSS5_BODY_DEFAULT_FIELD_RECT_SIZE_RIGHT, GMD_BOSS5_BODY_DEFAULT_FIELD_RECT_SIZE_BOTTOM);
        gmBoss5BodySetupRect(body_work);
        ObjObjectCopyAction3dNNModel(work, gm_boss5_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(745), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = GMD_BOSS5_DEFAULT_BLEND_SPD;
        work.disp_flag |= 134217728U;
        work.flag |= 16U;
        work.disp_flag |= 4194308U;
        work.move_flag &= 4294967167U;
        work.move_flag &= 4294443007U;
        work.move_flag |= 256U;
        work.move_flag |= 1024U;
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5BodyWaitSetup);
        gmBoss5BodyChangeState(body_work, 0, 0);
        work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5BodyOutFunc);
        work.ppRec = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5BodyRecFunc);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss5BodyExit));
        gmBoss5BodyAllocSeHandles(body_work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static OBS_OBJECT_WORK GmBoss5CoreInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS5_CORE_WORK(), "BOSS5_CORE");
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)work;
        GMS_BOSS5_CORE_WORK gmsBosS5CoreWork = (GMS_BOSS5_CORE_WORK)work;
        work.move_flag |= 256U;
        work.flag |= 18U;
        work.disp_flag &= 4294967263U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5CoreWaitSetup);
        return work;
    }

    private static OBS_ACTION3D_NN_WORK[] GmBoss5GetObject3dList()
    {
        return gm_boss5_obj_3d_list;
    }

    private static void GmBoss5BodyGetPlySearchPos(
      GMS_BOSS5_BODY_WORK body_work,
      out VecFx32 pos)
    {
        GmBsCmnGetDelaySearchPos(body_work.dsearch_work, body_work.ply_search_delay, out pos);
    }

    private static void GmBoss5ScatterSetFlyParam(OBS_OBJECT_WORK obj_work)
    {
        int num = mtMathRand() % 90;
        int ang = obj_work.pos.x > obj_work.parent_obj.pos.x ? AKM_DEGtoA32(num - 45) : AKM_DEGtoA32(num + 90 + 45);
        float sctNdcFlySpdFloat = GMD_BOSS5_SCT_NDC_FLY_SPD_FLOAT;
        obj_work.spd.y = (int)(4096.0 * sctNdcFlySpdFloat * nnSin(ang));
        obj_work.spd.x = (int)(4096.0 * sctNdcFlySpdFloat * nnCos(ang));
        obj_work.move_flag |= 128U;
    }

    private static void GmBoss5Init1ShotTimer(
      GMS_BOSS5_1SHOT_TIMER one_shot_timer,
      uint frame)
    {
        one_shot_timer.timer = frame;
        one_shot_timer.is_active = 1;
    }

    private static int GmBoss5Update1ShotTimer(GMS_BOSS5_1SHOT_TIMER one_shot_timer)
    {
        if (one_shot_timer.is_active == 0)
            return 0;
        if (one_shot_timer.timer != 0U)
        {
            --one_shot_timer.timer;
            return 0;
        }
        one_shot_timer.is_active = 0;
        return 1;
    }

    private static int GmBoss5UpdateVib(int phase_cnt, int scale, ref int pos_x, ref int pos_y)
    {
        int num1 = phase_cnt;
        if (num1 >= 40)
            num1 %= 40;
        pos_x = FX_Mul(scale, gm_boss5_vib_tbl[phase_cnt][0]);
        pos_y = FX_Mul(scale, gm_boss5_vib_tbl[phase_cnt][1]);
        int num2 = num1 + 1;
        if (num2 >= 40)
            num2 = 0;
        return num2;
    }

    private static void gmBoss5InitExplCreate(
      GMS_BOSS5_EXPL_WORK expl_work,
      int expl_type,
      OBS_OBJECT_WORK parent_obj,
      int ofst_pos_x,
      int ofst_pos_y,
      int width,
      int height,
      uint interval_min,
      uint interval_max,
      float se_freq)
    {
        expl_work.parent_obj = parent_obj;
        expl_work.expl_type = expl_type;
        expl_work.interval_timer = 0U;
        expl_work.interval_min = interval_min;
        expl_work.interval_max = interval_max;
        expl_work.se_frequency = se_freq;
        expl_work.se_freq_cnt = 0.0f;
        expl_work.ofst_pos[0] = ofst_pos_x;
        expl_work.ofst_pos[1] = ofst_pos_y;
        expl_work.area[0] = width;
        expl_work.area[1] = height;
    }

    public static void gmBoss5UpdateExplCreate(GMS_BOSS5_EXPL_WORK expl_work)
    {
        if (expl_work.interval_timer != 0U)
        {
            --expl_work.interval_timer;
        }
        else
        {
            int v2_1 = expl_work.area[0];
            int v2_2 = expl_work.area[1];
            VecFx32 vecFx32 = new VecFx32(expl_work.parent_obj.pos);
            int num1 = 0;
            int num2 = FX_Mul(AkMathRandFx(), v2_1);
            int num3 = FX_Mul(AkMathRandFx(), v2_2);
            if (expl_work.se_freq_cnt < 1.0)
                expl_work.se_freq_cnt += expl_work.se_frequency;
            if (expl_work.se_freq_cnt >= 1.0)
            {
                --expl_work.se_freq_cnt;
                num1 = 1;
            }
            switch (expl_work.expl_type)
            {
                case 0:
                    GmBoss5EfctCreateSmallExplosion(vecFx32.x + expl_work.ofst_pos[0] - (v2_1 >> 1) + num2, vecFx32.y + expl_work.ofst_pos[1] - (v2_2 >> 1) + num3, vecFx32.z + GMD_BOSS5_EXPL_OFST_Z);
                    if (num1 != 0)
                    {
                        GmSoundPlaySE("Boss0_02");
                        break;
                    }
                    break;
                case 1:
                    GmBoss5EfctCreateSmallExplosion(vecFx32.x + expl_work.ofst_pos[0] - (v2_1 >> 1) + num2, vecFx32.y + expl_work.ofst_pos[1] - (v2_2 >> 1) + num3, vecFx32.z + GMD_BOSS5_EXPL_OFST_Z);
                    GmBoss5EfctCreateFragments(vecFx32.x + expl_work.ofst_pos[0] - (v2_1 >> 1) + num2, vecFx32.y + expl_work.ofst_pos[1] - (v2_2 >> 1) + num3, vecFx32.z + GMD_BOSS5_EXPL_OFST_Z);
                    if (num1 != 0)
                    {
                        GmSoundPlaySE("Boss0_02");
                        break;
                    }
                    break;
                case 2:
                    GmBoss5EfctCreateBigExplosion(vecFx32.x + expl_work.ofst_pos[0] - (v2_1 >> 1) + num2, vecFx32.y + expl_work.ofst_pos[1] - (v2_2 >> 1) + num3, vecFx32.z + GMD_BOSS5_EXPL_OFST_Z);
                    if (num1 != 0)
                    {
                        GmSoundPlaySE("Boss0_03");
                        break;
                    }
                    break;
                default:
                    mppAssertNotImpl();
                    return;
            }
            uint num4 = (uint)(AkMathRandFx() * (expl_work.interval_max - expl_work.interval_min) >> 12);
            expl_work.interval_timer = expl_work.interval_min + num4;
        }
    }

    private static void gmBoss5SetCameraLift(GMS_BOSS5_MGR_WORK mgr_work)
    {
        GMS_PLAYER_WORK playerObj = (GMS_PLAYER_WORK)GmBsCmnGetPlayerObj();
        if (((int)mgr_work.flag & 16) == 0)
        {
            mgr_work.flag |= 16U;
            mgr_work.save_camera_offset[0] = playerObj.gmk_camera_center_ofst_x;
            mgr_work.save_camera_offset[1] = playerObj.gmk_camera_center_ofst_y;
        }
        GmCameraScaleSet(0.85f, 0.0025f);
        GmPlayerCameraOffsetSet((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), 0, GMD_BOSS5_CAMERA_LIFT_OFFSET_POS_Y);
    }

    private static void gmBoss5RestoreCameraLift(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 16) == 0)
            return;
        GmPlayerCameraOffsetSet((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), mgr_work.save_camera_offset[0], mgr_work.save_camera_offset[1]);
        GmCameraScaleSet(1f, 0.0025f);
        mgr_work.save_camera_offset[0] = mgr_work.save_camera_offset[1] = 0;
        mgr_work.flag &= 4294967279U;
    }

    private static void gmBoss5SetCameraSlideForNarrowScreen(GMS_BOSS5_MGR_WORK mgr_work)
    {
        GMS_PLAYER_WORK playerObj = (GMS_PLAYER_WORK)GmBsCmnGetPlayerObj();
        if (((int)mgr_work.flag & 16) == 0)
        {
            mgr_work.flag |= 16U;
            mgr_work.save_camera_offset[0] = playerObj.gmk_camera_center_ofst_x;
            mgr_work.save_camera_offset[1] = playerObj.gmk_camera_center_ofst_y;
        }
        GmPlayerCameraOffsetSet((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), GMD_BOSS5_CAMERA_SLIDE_FOR_NARROW_OFFSET_POS_X, 0);
    }

    private static void gmBoss5RestoreCameraSlideForNarrowScreen(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 16) == 0)
            return;
        GmPlayerCameraOffsetSet((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), mgr_work.save_camera_offset[0], mgr_work.save_camera_offset[1]);
        mgr_work.save_camera_offset[0] = mgr_work.save_camera_offset[1] = 0;
        mgr_work.flag &= 4294967279U;
    }

    private static void gmBoss5CamScrLimitReleaseGently()
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_COM_WORK(), null, 0, "scr_lim_rel_gently");
        work.disp_flag |= 4128U;
        work.flag |= 16U;
        work.user_timer = GMD_BOSS5_CAM_SCR_LIMIT_RELEASE_GNTL_SPD_X_INIT;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5CamScrLimitReleaseGentlyProcMain);
    }

    private static void gmBoss5CamScrLimitReleaseGentlyProcMain(OBS_OBJECT_WORK obj_work)
    {
        int num1 = g_gm_main_system.map_fcol.map_block_num_x * 64 << 12;
        int num2 = g_gm_main_system.map_fcol.right << 12;
        int num3 = 0;
        obj_work.user_timer += GMD_BOSS5_CAM_SCR_LIMIT_RELEASE_GNTL_ACC_X;
        obj_work.user_timer = MTM_MATH_CLIP(obj_work.user_timer, 0, int.MaxValue);
        int pos_x = num2 + obj_work.user_timer;
        if (pos_x >= num1)
        {
            pos_x = num1;
            num3 = 1;
        }
        GmCamScrLimitSetDirect(new GMS_EVE_RECORD_EVENT()
        {
            flag = 4,
            left = 0,
            top = 0,
            width = 0,
            height = 0
        }, pos_x, 0);
        if (num3 == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmBoss5HideMapBSide()
    {
        GmMapSetDispB(false);
    }

    private static void gmBoss5TransferPlayerToASide()
    {
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        GMS_PLAYER_WORK gmsPlayerWork = (GMS_PLAYER_WORK)playerObj;
        playerObj.flag &= 4294967294U;
        gmsPlayerWork.graind_prev_ride = 0;
    }

    private static void gmBoss5Vibration(int vib_idx)
    {
        GmCameraVibrationSet(gm_boss5_vib_param_tbl[vib_idx][0], gm_boss5_vib_param_tbl[vib_idx][1], gm_boss5_vib_param_tbl[vib_idx][2]);
    }

    private static void gmBoss5DelayedVibration(int vib_idx, uint delay)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_COM_WORK(), null, 0, "boss5_delay_vib");
        work.disp_flag |= 4128U;
        work.flag |= 16U;
        work.user_work = (uint)vib_idx;
        work.user_timer = (int)delay;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5DelayedVibrationProcMain);
    }

    private static void gmBoss5DelayedVibrationProcMain(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            gmBoss5Vibration((int)obj_work.user_work);
            obj_work.flag |= 4U;
        }
    }

    private static void gmBoss5DelayedSePlayback(string cue_name, uint delay)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_COM_WORK(), null, 0, "boss5_delay_se");
        work.disp_flag |= 4128U;
        work.flag |= 16U;
        amCriAudioGetGlobal();
        work.user_work = CriAuPlayer.GetCueId(cue_name);
        if (work.user_work == uint.MaxValue)
            return;
        work.user_timer = (int)delay;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5DelayedSePlaybackProcMain);
    }

    private static void gmBoss5DelayedSePlaybackProcMain(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            GsSoundPlaySeById(obj_work.user_work);
            obj_work.flag |= 4U;
        }
    }

    private static void gmBoss5MgrSetAlarmLevel(GMS_BOSS5_MGR_WORK mgr_work, int alarm_level)
    {
        mgr_work.alarm_level = alarm_level;
    }

    private static void gmBoss5MgrSetDemoRunDestPos(
      GMS_BOSS5_MGR_WORK mgr_work,
      int dest_pos_x)
    {
        mgr_work.ply_demo_run_dest_x = dest_pos_x;
    }

    private static void gmBoss5InitChasingExpl(GMS_BOSS5_MGR_WORK mgr_work)
    {
        gmBoss5InitExplCreate(mgr_work.small_expl_work, 0, GmBsCmnGetPlayerObj(), GMD_BOSS5_EXPL_CHASE_SMALL_OFST_X, GMD_BOSS5_EXPL_CHASE_SMALL_OFST_Y, GMD_BOSS5_EXPL_CHASE_SMALL_WIDTH, GMD_BOSS5_EXPL_CHASE_SMALL_HEIGHT, GMD_BOSS5_EXPL_CHASE_SMALL_INTERVAL_MIN, GMD_BOSS5_EXPL_CHASE_SMALL_INTERVAL_MAX, GMD_BOSS5_EXPL_CHASE_SMALL_SE_FREQUENCY);
        gmBoss5InitExplCreate(mgr_work.big_expl_work, 2, GmBsCmnGetPlayerObj(), GMD_BOSS5_EXPL_CHASE_BIG_OFST_X + GMD_BOSS5_EXPL_CHASE_BIG_DELAY_OFST_X, GMD_BOSS5_EXPL_CHASE_BIG_OFST_Y, GMD_BOSS5_EXPL_CHASE_BIG_WIDTH, GMD_BOSS5_EXPL_CHASE_BIG_HEIGHT, GMD_BOSS5_EXPL_CHASE_BIG_INTERVAL_MIN, GMD_BOSS5_EXPL_CHASE_BIG_INTERVAL_MAX, GMD_BOSS5_EXPL_CHASE_BIG_SE_FREQUENCY);
    }

    private static void gmBoss5UpdateChasingExpl(GMS_BOSS5_MGR_WORK mgr_work)
    {
        GMS_BOSS5_EXPL_WORK smallExplWork = mgr_work.small_expl_work;
        GMS_BOSS5_EXPL_WORK bigExplWork = mgr_work.big_expl_work;
        smallExplWork.ofst_pos[0] += GMD_BOSS5_EXPL_CHASE_SMALL_CHASE_SPD_X;
        if (smallExplWork.ofst_pos[0] >= GMD_BOSS5_EXPL_CHASE_SMALL_CHASE_OFST_X_MAX)
            smallExplWork.ofst_pos[0] = GMD_BOSS5_EXPL_CHASE_SMALL_CHASE_OFST_X_MAX;
        bigExplWork.ofst_pos[0] += GMD_BOSS5_EXPL_CHASE_BIG_CHASE_SPD_X;
        if (bigExplWork.ofst_pos[0] >= GMD_BOSS5_EXPL_CHASE_BIG_CHASE_OFST_X_MAX + GMD_BOSS5_EXPL_CHASE_BIG_DELAY_OFST_X)
            bigExplWork.ofst_pos[0] = GMD_BOSS5_EXPL_CHASE_BIG_CHASE_OFST_X_MAX + GMD_BOSS5_EXPL_CHASE_BIG_DELAY_OFST_X;
        gmBoss5UpdateExplCreate(mgr_work.small_expl_work);
        gmBoss5UpdateExplCreate(mgr_work.big_expl_work);
    }

    private static void gmBoss5MgrWaitLoad(OBS_OBJECT_WORK obj_work)
    {
        int num = 0;
        if (g_gs_main_sys_info.stage_id != 16 || GmMainDatLoadBossBattleLoadCheck(4))
            num = 1;
        if (num == 0)
            return;
        GMS_BOSS5_MGR_WORK gmsBosS5MgrWork = (GMS_BOSS5_MGR_WORK)obj_work;
        OBS_OBJECT_WORK obsObjectWork1 = GmEventMgrLocalEventBirth(330, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
        OBS_OBJECT_WORK obsObjectWork2 = GmEventMgrLocalEventBirth(331, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
        GMS_BOSS5_BODY_WORK gmsBosS5BodyWork = (GMS_BOSS5_BODY_WORK)obsObjectWork1;
        gmsBosS5MgrWork.body_work = gmsBosS5BodyWork;
        gmsBosS5BodyWork.mgr_work = gmsBosS5MgrWork;
        obsObjectWork1.parent_obj = obj_work;
        obsObjectWork2.parent_obj = obsObjectWork1;
        gmsBosS5BodyWork.parts_objs[0] = obsObjectWork1;
        gmsBosS5BodyWork.part_obj_core = obsObjectWork2;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5MgrWaitSetup);
    }

    private static void gmBoss5MgrWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_MGR_WORK mgr_work = (GMS_BOSS5_MGR_WORK)obj_work;
        GMS_BOSS5_BODY_WORK bodyWork = mgr_work.body_work;
        int num = 1;
        for (int index = 0; index < 1; ++index)
        {
            if (bodyWork.parts_objs[index] == null)
                num = 0;
        }
        if (bodyWork.part_obj_core == null)
            num = 0;
        if (num == 0)
            return;
        mgr_work.flag |= 1U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5MgrMain);
        gmBoss5MgrProcInit(mgr_work);
    }

    private static void gmBoss5MgrMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_MGR_WORK wrk = (GMS_BOSS5_MGR_WORK)obj_work;
        if (((int)wrk.flag & 2) != 0 && wrk.body_work != null)
        {
            GMM_BS_OBJ(wrk.body_work).flag |= 8U;
            wrk.body_work = null;
        }
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    private static void gmBoss5MgrProcInit(GMS_BOSS5_MGR_WORK mgr_work)
    {
        mgr_work.proc_update = new MPP_VOID_GMS_BOSS5_MGR_WORK(gmBoss5MgrProcUpdateWaitOpeningDemoBegin);
    }

    private static void gmBoss5MgrProcUpdateWaitOpeningDemoBegin(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 2097152) == 0 || ((int)mgr_work.flag & 4194304) == 0)
            return;
        GMS_PLAYER_WORK playerObj = (GMS_PLAYER_WORK)GmBsCmnGetPlayerObj();
        g_gm_main_system.game_flag &= 4294966271U;
        GmSoundChangeFinalBossBGM();
        GmPlySeqChangeBoss5Demo(playerObj, mgr_work.ply_demo_run_dest_x, false);
        mgr_work.proc_update = new MPP_VOID_GMS_BOSS5_MGR_WORK(gmBoss5MgrProcUpdateOpeningDemo);
        GmMapSetMapDrawSize(8);
    }

    private static void gmBoss5MgrProcUpdateOpeningDemo(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 33554432) == 0)
            return;
        GMS_PLAYER_WORK playerObj = (GMS_PLAYER_WORK)GmBsCmnGetPlayerObj();
        g_gm_main_system.game_flag |= 1024U;
        GmPlySeqChangeBoss5DemoEnd(playerObj);
        mgr_work.proc_update = new MPP_VOID_GMS_BOSS5_MGR_WORK(gmBoss5MgrProcUpdateIdle);
    }

    private static void gmBoss5MgrProcUpdateIdle(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 67108864) == 0)
            return;
        GmBoss5LandCreate(mgr_work);
        gmBoss5TransferPlayerToASide();
        gmBoss5HideMapBSide();
        mgr_work.proc_update = new MPP_VOID_GMS_BOSS5_MGR_WORK(gmBoss5MgrProcUpdateWaitDefeat);
    }

    private static void gmBoss5MgrProcUpdateWaitDefeat(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 134217728) == 0)
            return;
        g_gm_main_system.game_flag &= 4294966271U;
        g_gm_main_system.game_flag |= 1048576U;
        mgr_work.wait_timer = GMD_BOSS5_MGR_WAIT_EXPLODE_TIME;
        GmPadVibSet(1, -1f, 16384, 16384, -1f, 0.0f, 0.0f, 32768U);
        mgr_work.proc_update = new MPP_VOID_GMS_BOSS5_MGR_WORK(gmBoss5MgrProcUpdateWaitExplode);
    }

    private static void gmBoss5MgrProcUpdateWaitExplode(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            mgr_work.wait_timer = GMD_BOSS5_MGR_CLOSING_DEMO_WAIT_BEGIN_TIME_MAX;
            gmBoss5CamScrLimitReleaseGently();
            mgr_work.proc_update = new MPP_VOID_GMS_BOSS5_MGR_WORK(gmBoss5MgrProcUpdateWaitClosingDemoBegin);
        }
    }

    private static void gmBoss5MgrProcUpdateWaitClosingDemoBegin(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (mgr_work.wait_timer != 0U)
            --mgr_work.wait_timer;
        if (((int)GmBsCmnGetPlayerObj().move_flag & 1) == 0 && mgr_work.wait_timer != 0U)
            return;
        GmPlySeqChangeBoss5Demo((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), g_gm_main_system.map_size[0] << 12, true);
        mgr_work.proc_update = new MPP_VOID_GMS_BOSS5_MGR_WORK(gmBoss5MgrProcUpdateClosingDemoLeaveBody);
    }

    private static void gmBoss5MgrProcUpdateClosingDemoLeaveBody(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 268435456) == 0)
            return;
        gmBoss5InitChasingExpl(mgr_work);
        mgr_work.wait_timer = GMD_BOSS5_MGR_CLOSING_DEMO_DURATION_TIME;
        mgr_work.proc_update = new MPP_VOID_GMS_BOSS5_MGR_WORK(gmBoss5MgrProcUpdateClosingDemoEscape);
    }

    private static void gmBoss5MgrProcUpdateClosingDemoEscape(GMS_BOSS5_MGR_WORK mgr_work)
    {
        gmBoss5UpdateChasingExpl(mgr_work);
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            gmBoss5InitLastFadeOut(mgr_work);
            mgr_work.proc_update = new MPP_VOID_GMS_BOSS5_MGR_WORK(gmBoss5MgrProcUpdateClosingDemoWaitFadeEnd);
        }
    }

    private static void gmBoss5MgrProcUpdateClosingDemoWaitFadeEnd(GMS_BOSS5_MGR_WORK mgr_work)
    {
        gmBoss5UpdateChasingExpl(mgr_work);
        if (((int)mgr_work.flag & int.MinValue) == 0)
            return;
        GMM_PAD_VIB_STOP();
        mgr_work.wait_timer = GMD_BOSS5_MGR_CLOSING_DEMO_WHITEOUT_TIME;
        mgr_work.proc_update = new MPP_VOID_GMS_BOSS5_MGR_WORK(gmBoss5MgrProcUpdateClosingDemoWaitFinish);
    }

    private static void gmBoss5MgrProcUpdateClosingDemoWaitFinish(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            g_gm_main_system.game_flag |= 4U;
            mgr_work.proc_update = null;
        }
    }

    private static void gmBoss5BodyExit(MTS_TASK_TCB tcb)
    {
        GMS_BOSS5_BODY_WORK tcbWork = (GMS_BOSS5_BODY_WORK)mtTaskGetTcbWork(tcb);
        gmBoss5BodyReleaseCallbacks(tcbWork);
        gmBoss5BodyFreeSeHandles(tcbWork);
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss5BodySetActionWhole(GMS_BOSS5_BODY_WORK body_work, int act_id)
    {
        gmBoss5BodySetActionWhole(body_work, act_id, 0);
    }

    private static void gmBoss5BodySetActionWhole(
      GMS_BOSS5_BODY_WORK body_work,
      int act_id,
      int force_change)
    {
        GMS_BOSS5_PART_ACT_INFO[] bosS5PartActInfoArray = gm_boss5_act_id_tbl[act_id];
        if (force_change == 0 && body_work.whole_act_id == act_id)
            return;
        body_work.whole_act_id = act_id;
        for (int index = 0; index < 1; ++index)
        {
            if (body_work.parts_objs[index] != null)
            {
                if (bosS5PartActInfoArray[index].is_maintain == 0)
                    GmBsCmnSetAction(body_work.parts_objs[index], bosS5PartActInfoArray[index].act_id, bosS5PartActInfoArray[index].is_repeat, bosS5PartActInfoArray[index].is_blend);
                else if (bosS5PartActInfoArray[index].is_repeat != 0)
                    GMM_BS_OBJ(body_work).disp_flag |= 4U;
                body_work.parts_objs[index].obj_3d.speed[0] = bosS5PartActInfoArray[index].mtn_spd;
                body_work.parts_objs[index].obj_3d.blend_spd = bosS5PartActInfoArray[index].blend_spd;
            }
        }
    }

    private static void gmBoss5BodySetDirection(GMS_BOSS5_BODY_WORK body_work, int dir_type)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        switch (dir_type)
        {
            case 0:
                obsObjectWork.dir.y = (ushort)AKM_DEGtoA16(-90);
                obsObjectWork.disp_flag |= 1U;
                break;
            case 1:
                obsObjectWork.dir.y = (ushort)AKM_DEGtoA16(90);
                obsObjectWork.disp_flag &= 4294967294U;
                break;
            case 2:
                obsObjectWork.dir.y = 0;
                obsObjectWork.disp_flag &= 4294967294U;
                break;
            default:
                obsObjectWork.dir.y = (ushort)AKM_DEGtoA16(-90);
                obsObjectWork.disp_flag |= 1U;
                break;
        }
    }

    private static int gmBoss5BodyIsPlayerBehind(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK partObjCore = body_work.part_obj_core;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (partObjCore.pos.x < playerObj.pos.x)
                return 1;
        }
        else if (playerObj.pos.x < partObjCore.pos.x)
            return 1;
        return 0;
    }

    private static void gmBoss5BodySetNoHitTime(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_RECT_WORK[][] obsRectWorkArray = new OBS_RECT_WORK[3][]
        {
      body_work.sub_rect_work[0],
      body_work.sub_rect_work[1],
      ((GMS_ENEMY_COM_WORK) body_work).rect_work
        };
        body_work.no_hit_timer = (uint)GMD_BOSS5_BODY_DMG_NO_HIT_TIME;
        body_work.flag |= 32U;
        for (int index = 0; index < 3; ++index)
        {
            obsRectWorkArray[index][0].flag |= 2048U;
            obsRectWorkArray[index][0].flag &= 4294967291U;
        }
    }

    private static void gmBoss5BodyUpdateNoHitTime(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.no_hit_timer != 0U)
        {
            --body_work.no_hit_timer;
        }
        else
        {
            OBS_RECT_WORK[][] obsRectWorkArray = new OBS_RECT_WORK[3][]
            {
        body_work.sub_rect_work[0],
        body_work.sub_rect_work[1],
        ((GMS_ENEMY_COM_WORK) body_work).rect_work
            };
            body_work.flag &= 4294967263U;
            for (int index = 0; index < 3; ++index)
            {
                if ((body_work.def_rect_req_flag & 1 << index) != 0L)
                {
                    obsRectWorkArray[index][0].flag &= 4294965247U;
                    obsRectWorkArray[index][0].flag |= 4U;
                }
                else
                {
                    obsRectWorkArray[index][0].flag |= 2048U;
                    obsRectWorkArray[index][0].flag &= 4294967291U;
                }
            }
        }
    }

    private static void gmBoss5BodySetPokeTriggerLimitTime(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.poke_trg_limit_timer = 120U;
        body_work.flag |= 128U;
    }

    private static void gmBoss5BodyUpdatePokeTriggerLimitTime(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.poke_trg_limit_timer != 0U)
            --body_work.poke_trg_limit_timer;
        else
            body_work.flag &= 4294967167U;
    }

    private static int gmBoss5BodyIsWithinPokeTriggerLimitTime(GMS_BOSS5_BODY_WORK body_work)
    {
        return ((int)body_work.flag & 128) != 0 ? 1 : 0;
    }

    private static void gmBoss5BodyClearPokeTriggerLimitTime(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.poke_trg_limit_timer = 0U;
        body_work.flag &= 4294967167U;
    }

    private static void gmBoss5BodyExecDamageRoutine(GMS_BOSS5_BODY_WORK body_work)
    {
        GMS_BOSS5_MGR_WORK mgrWork = body_work.mgr_work;
        if (mgrWork.life != 0)
            --mgrWork.life;
        if (0 < mgrWork.life)
        {
            body_work.flag |= 2147483648U;
            if (body_work.state == 2)
            {
                if (gmBoss5BodyIsWithinPokeTriggerLimitTime(body_work) != 0)
                {
                    if (gmBoss5BodyIsPoking(body_work) == 0)
                        body_work.flag |= 8388608U;
                }
                else
                    gmBoss5BodySetPokeTriggerLimitTime(body_work);
            }
            gmBoss5BodyTryStartTurret(body_work);
        }
        else if (((int)body_work.flag & 65536) == 0)
        {
            mgrWork.life = 1;
        }
        else
        {
            body_work.flag |= 4194304U;
            GMM_BS_OBJ(body_work).flag |= 2U;
            mgrWork.life = 0;
        }

        mgrWork.life = Math.Max(mgrWork.life, 0);

        //Console.WriteLine(mgrWork.life);
        gmBoss5BodySeqTryRequestEnableStr(body_work);
    }

    private static void gmBoss5BodyUpdateMainRectPosition(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK partObjCore = body_work.part_obj_core;
        int x = partObjCore.pos.x - obsObjectWork.pos.x;
        int y = partObjCore.pos.y - obsObjectWork.pos.y;
        for (int index = 0; index < 3; ++index)
            VEC_Set(ref body_work.ene_3d.ene_com.rect_work[index].rect.pos, x, y, 0);
    }

    private static void gmBoss5BodyUpdateSubRectPosition(GMS_BOSS5_BODY_WORK body_work)
    {
        snm_reg_id_tbl[0] = body_work.leg_snm_reg_ids[0];
        snm_reg_id_tbl[1] = body_work.leg_snm_reg_ids[1];
        for (int index1 = 0; index1 < 2; ++index1)
        {
            NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(body_work.snm_work, snm_reg_id_tbl[index1]);
            int x = FX_F32_TO_FX32(snmMtx.M03) - body_work.pivot_prev_pos.x;
            int y = -FX_F32_TO_FX32(snmMtx.M13) - body_work.pivot_prev_pos.y;
            for (int index2 = 0; index2 < 3; ++index2)
                VEC_Set(ref body_work.sub_rect_work[index1][index2].rect.pos, x, y, 0);
        }
    }

    private static void gmBoss5BodySetupRect(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        OBS_RECT_WORK[][] obsRectWorkArray = new OBS_RECT_WORK[3][]
        {
            body_work.sub_rect_work[0],
            body_work.sub_rect_work[1],
            ((GMS_ENEMY_COM_WORK) body_work).rect_work
        };
        ushort[] numArray1 = new ushort[3] { 0, 2, 1 };
        ushort[] numArray2 = new ushort[3] { 65533, ushort.MaxValue, 65534 };
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
            {
                ObjRectGroupSet(body_work.sub_rect_work[index1][index2], 1, 1);
                ObjRectAtkSet(body_work.sub_rect_work[index1][index2], numArray1[index2], 1);
                ObjRectDefSet(body_work.sub_rect_work[index1][index2], numArray2[index2], 0);
                body_work.sub_rect_work[index1][index2].parent_obj = obsObjectWork;
                body_work.sub_rect_work[index1][index2].flag &= 4294967291U;
            }
            body_work.sub_rect_work[index1][0].ppDef = new OBS_RECT_WORK_Delegate1(GmEnemyDefaultDefFunc);
            body_work.sub_rect_work[index1][1].ppHit = new OBS_RECT_WORK_Delegate1(GmEnemyDefaultAtkFunc);
            body_work.sub_rect_work[index1][2].flag |= 1048800U;
        }
        for (int index = 0; index < 3; ++index)
            obsRectWorkArray[index][0].ppDef = new OBS_RECT_WORK_Delegate1(gmBoss5BodyDamageDefFunc);
        gmBoss5BodyChangeRectSetting(body_work, 0);
    }

    private static void gmBoss5BodyChangeRectSetting(
      GMS_BOSS5_BODY_WORK body_work,
      int rect_setting)
    {
        OBS_RECT_WORK[][] obsRectWorkArray = new OBS_RECT_WORK[3][]
        {
          body_work.sub_rect_work[0],
          body_work.sub_rect_work[1],
          ((GMS_ENEMY_COM_WORK) body_work).rect_work
        };
        GMS_BOSS5_BODY_RECT_SETTING_INFO bodyRectSettingInfo = gm_boss5_body_rect_setting_info_tbl[rect_setting];
        if (bodyRectSettingInfo.is_invincible != 0)
            body_work.flag |= 1U;
        else
            body_work.flag &= 4294967294U;
        if (bodyRectSettingInfo.is_leakage != 0)
            body_work.flag |= 4096U;
        else
            body_work.flag &= 4294963199U;
        for (int index1 = 0; index1 < 3; ++index1)
        {
            GMS_BOSS5_RECTPOINT_SETTING_INFO rectpointSettingInfo = bodyRectSettingInfo.point_setting_info[index1];
            for (int index2 = 0; index2 < 3; ++index2)
            {
                ObjRectWorkSet(obsRectWorkArray[index1][index2], rectpointSettingInfo.rect_size[index2][0], rectpointSettingInfo.rect_size[index2][1], rectpointSettingInfo.rect_size[index2][2], rectpointSettingInfo.rect_size[index2][3]);
                if ((1 << index2 & rectpointSettingInfo.enable_bit_flag) != 0L)
                {
                    if (index2 == 0)
                    {
                        body_work.def_rect_req_flag |= (uint)(1 << index1);
                        if (((int)body_work.flag & 32) == 0)
                        {
                            obsRectWorkArray[index1][index2].flag &= 4294965247U;
                            obsRectWorkArray[index1][index2].flag |= 4U;
                        }
                    }
                    else
                    {
                        obsRectWorkArray[index1][index2].flag &= 4294965247U;
                        obsRectWorkArray[index1][index2].flag |= 4U;
                    }
                }
                else
                {
                    obsRectWorkArray[index1][index2].flag |= 2048U;
                    obsRectWorkArray[index1][index2].flag &= 4294967291U;
                    if (index2 == 0)
                        body_work.def_rect_req_flag &= (uint)~(1 << index1);
                }
            }
        }
    }

    private static void gmBoss5BodyChangeRectSettingDefault(GMS_BOSS5_BODY_WORK body_work)
    {
        if (gmBoss5BodySeqIsStr(body_work) != 0)
            gmBoss5BodyChangeRectSetting(body_work, 1);
        else
            gmBoss5BodyChangeRectSetting(body_work, 0);
    }

    private static void gmBoss5BodySwitchEnableLegRectOneSide(
      GMS_BOSS5_BODY_WORK body_work,
      int leg_type)
    {
        int index1;
        int index2;
        if (leg_type == 0)
        {
            index1 = 0;
            index2 = 1;
        }
        else
        {
            index1 = 1;
            index2 = 0;
        }
        body_work.sub_rect_work[index1][1].flag &= 4294965247U;
        body_work.sub_rect_work[index1][1].flag |= 4U;
        body_work.sub_rect_work[index2][1].flag |= 2048U;
        body_work.sub_rect_work[index2][1].flag &= 4294967291U;
    }

    private static void gmBoss5BodyTryImmobilizePlayer(GMS_BOSS5_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
        if (((int)GmBsCmnGetPlayerObj().move_flag & 1) == 0)
            return;
        GmPlySeqGmkInitBoss5Quake((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), GMD_BOSS5_BODY_CRASH_PLY_IMMOBILE_TIME);
    }

    private static void gmBoss5BodyAllocSeHandles(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.se_hnd_leakage = GsSoundAllocSeHandle();
    }

    private static void gmBoss5BodyFreeSeHandles(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.se_hnd_leakage == null)
            return;
        GsSoundFreeSeHandle(body_work.se_hnd_leakage);
        body_work.se_hnd_leakage = null;
    }

    private static void gmBoss5BodyInitPlayTargetSe(
      GMS_BOSS5_BODY_WORK body_work,
      float init_interval)
    {
        body_work.targ_se_cur_interval = init_interval;
        GmBoss5Init1ShotTimer(body_work.targ_se_timer, (uint)body_work.targ_se_cur_interval);
        GmSoundPlaySE("FinalBoss05");
    }

    private static void gmBoss5BodyUpdatePlayTargetSe(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.targ_se_cur_interval >= 0.0)
            body_work.targ_se_cur_interval -= GMD_BOSS5_BODY_SE_TARGET_INTERVAL_DEC_SPD;
        else
            body_work.targ_se_cur_interval = 0.0f;
        if (GmBoss5Update1ShotTimer(body_work.targ_se_timer) == 0)
            return;
        uint frame = (uint)body_work.targ_se_cur_interval;
        if (frame <= GMD_BOSS5_BODY_SE_TARGET_INTERVAL_MIN)
            frame = (uint)GMD_BOSS5_BODY_SE_TARGET_INTERVAL_MIN;
        GmSoundPlaySE("FinalBoss05");
        GmBoss5Init1ShotTimer(body_work.targ_se_timer, frame);
    }

    private static void gmBoss5BodyForceEndLeakage(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294963199U;
        GmBoss5EfctEndLeakage(body_work, 1);
    }

    private static void gmBoss5BodyInitPlySearch(GMS_BOSS5_BODY_WORK body_work, int delay)
    {
        body_work.ply_search_delay = delay;
        GmBsCmnInitDelaySearch(body_work.dsearch_work, GmBsCmnGetPlayerObj(), body_work.search_hist_buf, 11);
    }

    private static void gmBoss5BodyUpdatePlySearch(GMS_BOSS5_BODY_WORK body_work)
    {
        GmBsCmnUpdateDelaySearch(body_work.dsearch_work);
    }

    private static void gmBoss5BodySetPlyRebound(
      GMS_PLAYER_WORK ply_work,
      GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)ply_work;
        GmPlySeqAtkReactionInit(ply_work);
        if (ply_work.seq_state == GME_PLY_SEQ_STATE_HOMING_REF)
        {
            GmPlySeqSetJumpState(ply_work, 0, 5U);
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ply_work.obj_work.move.x < 0 ? GMD_BOSS5_BODY_PLY_HOMING_REBOUND_X : -GMD_BOSS5_BODY_PLY_HOMING_REBOUND_X;
            ply_work.obj_work.spd.y = obsObjectWork.pos.y > body_work.part_obj_core.pos.y ? -GMD_BOSS5_BODY_PLY_HOMING_REBOUND_Y : GMD_BOSS5_BODY_PLY_HOMING_REBOUND_Y;
            GmPlySeqSetNoJumpMoveTime(ply_work, GMD_BOSS5_BODY_PLY_HOMING_REBOUND_NOJUMPMOVE_TIME);
        }
        else
        {
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ply_work.obj_work.move.x < 0 ? GMD_BOSS5_BODY_PLY_NML_REBOUND_X : -GMD_BOSS5_BODY_PLY_NML_REBOUND_X;
            ply_work.obj_work.spd.y = obsObjectWork.pos.y > body_work.part_obj_core.pos.y ? -GMD_BOSS5_BODY_PLY_NML_REBOUND_Y : GMD_BOSS5_BODY_PLY_NML_REBOUND_Y;
            GmPlySeqSetNoJumpMoveTime(ply_work, GMD_BOSS5_BODY_PLY_NML_REBOUND_NOJUMPMOVE_TIME);
            ply_work.homing_timer = GMD_BOSS5_BODY_DMG_NO_HIT_TIME * 4096;
        }
    }

    private static void gmBoss5BodySetMoveFastTime(
      GMS_BOSS5_BODY_WORK body_work,
      uint fast_move_time)
    {
        body_work.fast_move_timer = fast_move_time;
    }

    private static void gmBoss5BodyUpdateMoveFastTime(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.fast_move_timer == 0U)
            return;
        --body_work.fast_move_timer;
    }

    private static int gmBoss5BodyIsMoveFastEnd(GMS_BOSS5_BODY_WORK body_work)
    {
        return body_work.fast_move_timer != 0U ? 0 : 1;
    }

    private static int gmBoss5BodyGetStompFallPosX(
      GMS_BOSS5_BODY_WORK body_work,
      int search_pos_x)
    {
        UNREFERENCED_PARAMETER(body_work);
        int num1 = 0;
        int num2 = search_pos_x - GMD_BOSS5_BODY_STOMP_FALL_POS_MARGIN;
        int num3 = search_pos_x + GMD_BOSS5_BODY_STOMP_FALL_POS_MARGIN;
        if (num2 <= GMM_BOSS5_AREA_LEFT())
            num1 = GMM_BOSS5_AREA_LEFT() - num2;
        else if (num3 >= GMM_BOSS5_AREA_RIGHT())
            num1 = GMM_BOSS5_AREA_RIGHT() - num3;
        return search_pos_x + num1;
    }

    private static void gmBoss5BodyDecideCrashFallPosX(GMS_BOSS5_BODY_WORK body_work)
    {
        ushort sec = 0;
        uint frame = g_gm_main_system.game_time < 35999U ? g_gm_main_system.game_time : 35999U;
        AkUtilFrame60ToTime(frame, new ushort?(), ref sec, new ushort?());
        switch ((ushort)(frame % 10U))
        {
            case 3:
            case 5:
            case 7:
            case 9:
                body_work.crash_pos_ofst_x = 1048576;
                break;
            case 4:
            case 6:
            case 8:
                body_work.crash_pos_ofst_x = -1048576;
                break;
            default:
                body_work.crash_pos_ofst_x = 0;
                break;
        }
    }

    private static int gmBoss5BodyGetCrashFallPosX(GMS_BOSS5_BODY_WORK body_work)
    {
        return GMM_BOSS5_AREA_CENTER_X() + body_work.crash_pos_ofst_x;
    }

    private static int gmBoss5BodyCheckJetSmokeClearTiming(GMS_BOSS5_BODY_WORK body_work)
    {
        return GMM_BS_OBJ(body_work).pos.y <= body_work.ground_v_pos - GMD_BOSS5_BODY_JETSMOKE_CLEAR_HEIGHT ? 1 : 0;
    }

    private static int gmBoss5BodyIsMoveFastDirFwd(GMS_BOSS5_BODY_WORK body_work)
    {
        return body_work.state == 3 || body_work.state != 4 ? 1 : 0;
    }

    private static int gmBoss5BodyIsBodyExplosionStopAllowed(GMS_BOSS5_BODY_WORK body_work)
    {
        int num = ObjViewOutCheck(body_work.part_obj_core.pos.x, body_work.ground_v_pos, 0, (short)-(GMD_BOSS5_EXPL_BODY_OFST_X + GMD_BOSS5_EXPL_BODY_WIDTH / 2 >> 12), (short)-(GMD_BOSS5_EXPL_BODY_OFST_Y + GMD_BOSS5_EXPL_BODY_HEIGHT / 2 >> 12), (short)-(GMD_BOSS5_EXPL_BODY_OFST_X - GMD_BOSS5_EXPL_BODY_WIDTH / 2 >> 12), (short)-(GMD_BOSS5_EXPL_BODY_OFST_Y - GMD_BOSS5_EXPL_BODY_HEIGHT / 2 >> 12));
        if (GmBsCmnGetPlayerObj().pos.x < body_work.part_obj_core.pos.x)
            num = 0;
        return num != 0 ? 1 : 0;
    }

    private static int gmBoss5BodyGetStompFallDirectionType(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        return obsObjectWork.pos.x <= GMM_BOSS5_AREA_LEFT() + GMD_BOSS5_BODY_STOMP_WALL_BEHIND_WALL_DISTANCE || obsObjectWork.pos.x < GMM_BOSS5_AREA_RIGHT() - GMD_BOSS5_BODY_STOMP_WALL_BEHIND_WALL_DISTANCE && (obsObjectWork.pos.x < GmBsCmnGetPlayerObj().pos.x || obsObjectWork.pos.x <= GmBsCmnGetPlayerObj().pos.x && ((int)GmBsCmnGetPlayerObj().disp_flag & 1) == 0) ? 1 : 0;
    }

    private static void gmBoss5BodyTryStartTurret(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.mgr_work.life > GMD_BOSS5_TURRET_START_LIFE_THRESHOLD || ((int)body_work.flag & 1024) != 0)
            return;
        body_work.flag |= 1024U;
        GmBoss5TurretStartUp(body_work);
    }

    private static void gmBoss5BodyRecordGapAdjustmentDest(GMS_BOSS5_BODY_WORK body_work)
    {
        int[] numArray = new int[2]
        {
      body_work.lfoot_snm_reg_id,
      body_work.rfoot_snm_reg_id
        };
        for (int index = 0; index < 2; ++index)
        {
            NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(body_work.snm_work, numArray[index]);
            int x = body_work.grdmv_pivot_pos.x;
            int fx32 = FX_F32_TO_FX32(snmMtx.M03);
            body_work.foot_ofst_record_dest[index] = fx32 - x;
            if (((int)GMM_BS_OBJ(body_work).disp_flag & 1) != 0)
                body_work.foot_ofst_record_dest[index] = -body_work.foot_ofst_record_dest[index];
        }
    }

    private static void gmBoss5BodyRecordGapAdjustmentSrc(GMS_BOSS5_BODY_WORK body_work)
    {
        int[] numArray = new int[2]
        {
      body_work.lfoot_snm_reg_id,
      body_work.rfoot_snm_reg_id
        };
        for (int index = 0; index < 2; ++index)
        {
            NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(body_work.snm_work, numArray[index]);
            int x = body_work.grdmv_pivot_pos.x;
            int fx32 = FX_F32_TO_FX32(snmMtx.M03);
            body_work.foot_ofst_record_src[index] = fx32 - x;
            if (((int)GMM_BS_OBJ(body_work).disp_flag & 1) != 0)
                body_work.foot_ofst_record_src[index] = -body_work.foot_ofst_record_src[index];
        }
    }

    private static void gmBoss5BodyInitAdjustMtnBlendHGap(
      GMS_BOSS5_BODY_WORK body_work,
      int dest_act_id,
      int leg_type)
    {
        body_work.adj_hgap_is_active = 1;
        body_work.adj_hgap_act_id = dest_act_id;
        body_work.adj_hgap_leg_type = leg_type;
    }

    private static void gmBoss5BodyUpdateAdjustMtnBlendHGap(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (body_work.adj_hgap_is_active == 0)
            return;
        int num = (int)((body_work.foot_ofst_record_dest[body_work.adj_hgap_leg_type] - body_work.foot_ofst_record_src[body_work.adj_hgap_leg_type]) * (double)gm_boss5_act_id_tbl[body_work.adj_hgap_act_id][0].blend_spd);
        if (((int)GMM_BS_OBJ(body_work).disp_flag & 1) != 0)
            obsObjectWork.pos.x += num;
        else
            obsObjectWork.pos.x -= num;
    }

    private static void gmBoss5BodyClearAdjustMtnBlendHGap(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.adj_hgap_is_active = 0;
    }

    private static void gmBoss5BodyInitGroundingMove(
      GMS_BOSS5_BODY_WORK body_work,
      int ref_snm_reg_id)
    {
        GMS_BOSS5_GRD_MOVE_WORK grdmvWork = body_work.grdmv_work;
        grdmvWork.cur_diff_x = 0;
        grdmvWork.prev_diff_x = 0;
        grdmvWork.ref_snm_reg_id = ref_snm_reg_id;
        grdmvWork.is_first_updated = 0;
    }

    private static void gmBoss5BodyUpdateGroundingMove(GMS_BOSS5_BODY_WORK body_work)
    {
        GMS_BOSS5_GRD_MOVE_WORK grdmvWork = body_work.grdmv_work;
        if (grdmvWork.ref_snm_reg_id == -1)
            return;
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(body_work.snm_work, grdmvWork.ref_snm_reg_id);
        int x = body_work.grdmv_pivot_pos.x;
        int fx32 = FX_F32_TO_FX32(snmMtx.M03);
        grdmvWork.prev_diff_x = grdmvWork.cur_diff_x;
        grdmvWork.cur_diff_x = fx32 - x;
        if (grdmvWork.is_first_updated == 0)
            grdmvWork.is_first_updated = 1;
        else
            GMM_BS_OBJ(body_work).pos.x -= grdmvWork.cur_diff_x - grdmvWork.prev_diff_x;
    }

    private static void gmBoss5BodyChangeMovePhase(
      GMS_BOSS5_BODY_WORK body_work,
      int move_phase_type)
    {
        body_work.cur_move_phase_type = move_phase_type;
        int ref_snm_reg_id = body_work.cur_move_phase_type != 1 ? (body_work.cur_move_phase_type != 2 ? -1 : body_work.rfoot_snm_reg_id) : body_work.lfoot_snm_reg_id;
        gmBoss5BodyInitGroundingMove(body_work, ref_snm_reg_id);
    }

    private static void gmBoss5BodyInitWalk(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodyChangeMovePhase(body_work, gm_boss5_body_walk_move_info_tbl[0].move_phase_type);
    }

    private static int gmBoss5BodyUpdateWalk(GMS_BOSS5_BODY_WORK body_work)
    {
        float num = GMM_BS_OBJ(body_work).obj_3d.frame[0];
        gmBoss5BodyUpdateGroundingMove(body_work);
        int index;
        for (index = 3; index >= 0; --index)
        {
            if (gm_boss5_body_walk_move_info_tbl[index].switching_frame <= (double)num)
            {
                if (gm_boss5_body_walk_move_info_tbl[index].move_phase_type != body_work.cur_move_phase_type)
                {
                    gmBoss5BodyChangeMovePhase(body_work, gm_boss5_body_walk_move_info_tbl[index].move_phase_type);
                    break;
                }
                break;
            }
        }
        return index >= 3 ? 1 : 0;
    }

    private static void gmBoss5BodyInitWalkAbortRecovery(
      GMS_BOSS5_BODY_WORK body_work,
      int cur_move_phase_type)
    {
        int leg_type = cur_move_phase_type != 1 ? 1 : 0;
        gmBoss5BodyInitWalkAbortRecoveryByLegType(body_work, leg_type);
    }

    private static void gmBoss5BodyInitWalkAbortRecoveryByLegType(
      GMS_BOSS5_BODY_WORK body_work,
      int leg_type)
    {
        gmBoss5BodyRecordGapAdjustmentSrc(body_work);
        gmBoss5BodyInitAdjustMtnBlendHGap(body_work, body_work.whole_act_id, leg_type);
        gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
    }

    private static int gmBoss5BodyUpdateWalkAbortRecovery(GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)GMM_BS_OBJ(body_work).obj_3d.flag & 1) == 0)
            return 1;
        gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
        return 0;
    }

    private static void gmBoss5BodyInitMonitoringWalkEnd(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.walk_end_monitor_phase_cnt = 0;
        body_work.is_player_behind = 0;
    }

    private static int gmBoss5BodyUpdateMonitoringWalkEnd(
      GMS_BOSS5_BODY_WORK body_work,
      int? leg_type)
    {
        int leg_type1 = 0;
        return _gmBoss5BodyUpdateMonitoringWalkEnd(body_work, ref leg_type1, false);
    }

    private static int gmBoss5BodyUpdateMonitoringWalkEnd(
      GMS_BOSS5_BODY_WORK body_work,
      ref int leg_type)
    {
        return _gmBoss5BodyUpdateMonitoringWalkEnd(body_work, ref leg_type, true);
    }

    private static int _gmBoss5BodyUpdateMonitoringWalkEnd(
      GMS_BOSS5_BODY_WORK body_work,
      ref int leg_type,
      bool ltAvailable)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        float num1 = obsObjectWork.obj_3d.frame[0];
        int num2 = 0;
        if (gmBoss5BodyIsPlayerBehind(body_work) != 0)
            body_work.is_player_behind = 1;
        int index;
        for (index = 4; index >= 0; --index)
        {
            if (gm_boss5_body_walk_ground_timing_info_tbl[index].grounding_frame <= (double)num1)
            {
                if (index != body_work.walk_end_monitor_phase_cnt)
                {
                    num2 = 1;
                    body_work.walk_end_monitor_phase_cnt = index;
                    break;
                }
                break;
            }
        }
        if (ltAvailable)
            leg_type = index < 0 ? 0 : gm_boss5_body_walk_ground_timing_info_tbl[index].leg_type;
        if (index >= 4 || num2 == 0)
            return 0;
        if (body_work.is_player_behind != 0)
            return 1;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.pos.x <= GMM_BOSS5_AREA_LEFT() + GMD_BOSS5_BODY_WALK_WALK_END_WALL_DISTANCE)
                return 1;
        }
        else if (obsObjectWork.pos.x >= GMM_BOSS5_AREA_RIGHT() - GMD_BOSS5_BODY_WALK_WALK_END_WALL_DISTANCE)
            return 1;
        return 0;
    }

    private static void gmBoss5BodyInitWalkGroundingEffects(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.cur_walk_grnd_phase_cnt = 0;
    }

    private static int gmBoss5BodyUpdateWalkGroundingEffects(GMS_BOSS5_BODY_WORK body_work)
    {
        float num = GMM_BS_OBJ(body_work).obj_3d.frame[0];
        int index;
        for (index = 4; index >= 0; --index)
        {
            if (gm_boss5_body_walk_ground_timing_info_tbl[index].grounding_frame <= (double)num)
            {
                if (index != body_work.cur_walk_grnd_phase_cnt)
                {
                    GmBoss5EfctCreateWalkStepSmoke(body_work, gm_boss5_body_walk_ground_timing_info_tbl[index].leg_type);
                    gmBoss5Vibration(0);
                    GmSoundPlaySE("FinalBoss03");
                    body_work.cur_walk_grnd_phase_cnt = index;
                    break;
                }
                break;
            }
        }
        return index >= 4 ? 1 : 0;
    }

    private static void gmBoss5BodyInitRunGroundingEffects(
      GMS_BOSS5_BODY_WORK body_work,
      int run_type,
      uint delay)
    {
        body_work.run_grnd_runtype = run_type;
        body_work.run_grnd_delay_timer = delay;
        body_work.run_grnd_spawn_remain = 1U;
    }

    private static int gmBoss5BodyUpdateRunGroundingEffects(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.run_grnd_delay_timer != 0U)
        {
            --body_work.run_grnd_delay_timer;
            return 0;
        }
        if (body_work.run_grnd_spawn_remain == 0U)
            return 1;
        --body_work.run_grnd_spawn_remain;
        switch (body_work.run_grnd_runtype)
        {
            case 0:
                GmBoss5EfctCreateRunStepSmoke(body_work, 0);
                break;
            case 1:
                GmBoss5EfctCreateRunStepSmoke(body_work, 1);
                break;
        }
        gmBoss5Vibration(1);
        GmSoundPlaySE("FinalBoss03");
        return 1;
    }

    private static void gmBoss5BodyInitStompFlyUp(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag &= 4294967166U;
        obj_work.move_flag |= 272U;
        obj_work.spd.y = GMD_BOSS5_BODY_SFLYUP_INIT_SPD;
        obj_work.spd_add.y = GMD_BOSS5_BODY_SFLYUP_ACC;
    }

    private static int gmBoss5BodyUpdateStompFlyUp(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (obj_work.pos.y > GMM_BOSS5_AREA_TOP() - GMD_BOSS5_BODY_HIDE_RADIUS)
            return 0;
        GmBsCmnSetObjSpdZero(obj_work);
        return 1;
    }

    private static void gmBoss5BodyInitStompFall(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.spd.y = GMD_BOSS5_BODY_STOMP_FALL_INIT_SPD;
        obj_work.spd_add.y = GMD_BOSS5_BODY_STOMP_FALL_ACC;
    }

    private static int gmBoss5BodyUpdateStompFall(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (obj_work.pos.y >= GMM_BOSS5_AREA_TOP())
            obj_work.move_flag &= 4294967039U;
        if (((int)obj_work.move_flag & 1) == 0)
            return 0;
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag |= 128U;
        return 1;
    }

    private static void gmBoss5BodyInitCrashFlyUp(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag &= 4294967166U;
        obj_work.move_flag |= 272U;
        obj_work.spd.y = GMD_BOSS5_BODY_CFLYUP_INIT_SPD;
        obj_work.spd_add.y = GMD_BOSS5_BODY_CFLYUP_ACC;
    }

    private static int gmBoss5BodyUpdateCrashFlyUp(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (obj_work.pos.y > GMM_BOSS5_AREA_TOP() - GMD_BOSS5_BODY_HIDE_RADIUS)
            return 0;
        GmBsCmnSetObjSpdZero(obj_work);
        return 1;
    }

    private static void gmBoss5BodyInitCrashFall(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.spd.y = GMD_BOSS5_BODY_CRASH_FALL_INIT_SPD;
        obj_work.spd_add.y = GMD_BOSS5_BODY_CRASH_FALL_ACC;
    }

    private static int gmBoss5BodyUpdateCrashFall(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (obj_work.pos.y >= GMM_BOSS5_AREA_TOP())
            obj_work.move_flag &= 4294967039U;
        if (((int)obj_work.move_flag & 1) == 0)
            return 0;
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag |= 128U;
        return 1;
    }

    private static void gmBoss5BodyInitCrashSink(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag |= 384U;
        obj_work.move_flag &= 4294967294U;
    }

    private static int gmBoss5BodyUpdateCrashSink(GMS_BOSS5_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
        return 0;
    }

    private static void gmBoss5BodyInitBerserkTurn(
      GMS_BOSS5_BODY_WORK body_work,
      int turn_type)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        float n;
        switch (turn_type)
        {
            case 0:
                n = GMD_BOSS5_BODY_BERSERK_TURN_FRONT_DEG_F;
                break;
            case 1:
                n = GMD_BOSS5_BODY_BERSERK_TURN_RETURN_DEG_F;
                break;
            default:
                n = 0.0f;
                break;
        }
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            n = -n;
        body_work.turn_src_dir = (int)(ushort.MaxValue & (long)obsObjectWork.dir.y);
        int num = (int)(ushort.MaxValue & (long)AKM_DEGtoA32(n));
        body_work.turn_tgt_ofst_dir = (int)(ushort.MaxValue & (long)(num - body_work.turn_src_dir));
        if (body_work.turn_tgt_ofst_dir > AKM_DEGtoA32(180))
            body_work.turn_tgt_ofst_dir = (int)(body_work.turn_tgt_ofst_dir - 65536L);
        body_work.turn_ratio = 0.0f;
    }

    private static int gmBoss5BodyUpdateBerserkTurn(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num1 = 0;
        body_work.turn_ratio += GMD_BOSS5_BODY_BERSERK_TURN_RATIO_SPD;
        if (body_work.turn_ratio >= 1.0)
        {
            body_work.turn_ratio = 1f;
            num1 = 1;
        }
        int num2 = (int)(body_work.turn_tgt_ofst_dir * (double)body_work.turn_ratio);
        obsObjectWork.dir.y = (ushort)(ushort.MaxValue & (ulong)(body_work.turn_src_dir + num2));
        return num1;
    }

    private static void gmBoss5BodyClearBerserkTurn(GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)GMM_BS_OBJ(body_work).disp_flag & 1) != 0)
            gmBoss5BodySetDirection(body_work, 0);
        else
            gmBoss5BodySetDirection(body_work, 1);
        body_work.turn_src_dir = 0;
        body_work.turn_tgt_ofst_dir = 0;
        body_work.turn_ratio = 0.0f;
    }

    private static void gmBoss5BodyInitPoke(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodyInitArmPose(body_work);
        body_work.arm_poke_anim_phase = 0;
        gmBoss5BodyInitArmAnim(body_work, gm_boss5_arm_anim_info_tbl[body_work.arm_poke_anim_phase]);
        body_work.flag |= 64U;
        body_work.flag |= 256U;
    }

    private static int gmBoss5BodyUpdatePoke(GMS_BOSS5_BODY_WORK body_work)
    {
        int num = 0;
        if (gmBoss5BodyUpdateArmAnim(body_work) != 0)
        {
            ++body_work.arm_poke_anim_phase;
            if (body_work.arm_poke_anim_phase >= 9)
            {
                body_work.arm_poke_anim_phase = 8;
                num = 1;
            }
            else
                gmBoss5BodyInitArmAnim(body_work, gm_boss5_arm_anim_info_tbl[body_work.arm_poke_anim_phase]);
        }
        return num;
    }

    private static void gmBoss5BodyEndPoke(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294967039U;
        body_work.flag &= 4294967231U;
        gmBoss5BodyEndArmPose(body_work);
        body_work.arm_poke_anim_phase = 0;
    }

    private static int gmBoss5BodyIsPoking(GMS_BOSS5_BODY_WORK body_work)
    {
        return ((int)body_work.flag & 64) != 0 ? 1 : 0;
    }

    private static void gmBoss5BodyInitArmAnim(
      GMS_BOSS5_BODY_WORK body_work,
      GMS_BOSS5_ARM_ANIM_INFO anim_info)
    {
        if (anim_info.wait_time > 0U)
        {
            body_work.arm_anim_work.is_anim = 0;
            body_work.arm_anim_work.anim_wait_timer = anim_info.wait_time;
            body_work.arm_anim_work.cur_rate = 0.0f;
            body_work.arm_anim_work.rate_add = 0.0f;
            for (int index = 0; index < 3; ++index)
            {
                nnMakeRotateXYZQuaternion(out body_work.arm_anim_work.start_quat[index], anim_info.part_anim_info[index].start_rot.x, anim_info.part_anim_info[index].start_rot.y, anim_info.part_anim_info[index].start_rot.z);
                nnMakeUnitQuaternion(ref body_work.arm_anim_work.end_quat[index]);
            }
        }
        else
        {
            body_work.arm_anim_work.is_anim = 1;
            body_work.arm_anim_work.anim_wait_timer = 0U;
            body_work.arm_anim_work.cur_rate = 0.0f;
            body_work.arm_anim_work.rate_add = anim_info.slerp_inc_rate;
            for (int index = 0; index < 3; ++index)
            {
                nnMakeRotateXYZQuaternion(out body_work.arm_anim_work.start_quat[index], anim_info.part_anim_info[index].start_rot.x, anim_info.part_anim_info[index].start_rot.y, anim_info.part_anim_info[index].start_rot.z);
                nnMakeRotateXYZQuaternion(out body_work.arm_anim_work.end_quat[index], anim_info.part_anim_info[index].end_rot.x, anim_info.part_anim_info[index].end_rot.y, anim_info.part_anim_info[index].end_rot.z);
            }
        }
        for (int arm_part_idx = 0; arm_part_idx < 3; ++arm_part_idx)
        {
            gmBoss5BodySetArmPoseParam(body_work, 0, arm_part_idx, ref body_work.arm_anim_work.start_quat[arm_part_idx]);
            NNS_QUATERNION dst_quat;
            AkMathInvertYZQuaternion(out dst_quat, ref body_work.arm_anim_work.start_quat[arm_part_idx]);
            gmBoss5BodySetArmPoseParam(body_work, 1, arm_part_idx, ref dst_quat);
        }
        gmBoss5BodyApplyArmPose(body_work);
    }

    private static int gmBoss5BodyUpdateArmAnim(GMS_BOSS5_BODY_WORK body_work)
    {
        int num = 0;
        if (body_work.arm_anim_work.is_anim != 0)
        {
            body_work.arm_anim_work.cur_rate += body_work.arm_anim_work.rate_add;
            if (body_work.arm_anim_work.cur_rate >= 1.0)
            {
                body_work.arm_anim_work.cur_rate = 1f;
                num = 1;
            }
            for (int arm_part_idx = 0; arm_part_idx < 3; ++arm_part_idx)
            {
                NNS_QUATERNION dst;
                nnSlerpQuaternion(out dst, ref body_work.arm_anim_work.start_quat[arm_part_idx], ref body_work.arm_anim_work.end_quat[arm_part_idx], body_work.arm_anim_work.cur_rate);
                gmBoss5BodySetArmPoseParam(body_work, 0, arm_part_idx, ref dst);
                NNS_QUATERNION dst_quat;
                AkMathInvertYZQuaternion(out dst_quat, ref dst);
                gmBoss5BodySetArmPoseParam(body_work, 1, arm_part_idx, ref dst_quat);
            }
        }
        else if (body_work.arm_anim_work.anim_wait_timer != 0U)
            --body_work.arm_anim_work.anim_wait_timer;
        else
            num = 1;
        gmBoss5BodyApplyArmPose(body_work);
        return num;
    }

    private static void gmBoss5BodyInitCloseCanopy(GMS_BOSS5_BODY_WORK body_work)
    {
        nnMakeRotateXYZQuaternion(out body_work.cnpy_close_init_quat, GMD_BOSS5_BODY_CANOPY_CLOSE_START_ANGLE_X, 0, 0);
        nnMakeRotateXYZQuaternion(out body_work.cnpy_close_dest_quat, 0, 0, 0);
        body_work.cnpy_close_ratio = 0.0f;
        body_work.cnpy_close_ratio_spd = 0.0f;
    }

    private static int gmBoss5BodyUpdateCloseCanopy(
      GMS_BOSS5_BODY_WORK body_work,
      int is_update)
    {
        NNS_QUATERNION dst = new NNS_QUATERNION();
        int num = 0;
        if (is_update != 0)
            body_work.cnpy_close_ratio_spd += GMD_BOSS5_BODY_CANOPY_CLOSE_RATIO_SPD_ACC;
        if (body_work.cnpy_close_ratio_spd >= (double)GMD_BOSS5_BODY_CANOPY_CLOSE_RATIO_SPD_MAX)
            body_work.cnpy_close_ratio_spd = GMD_BOSS5_BODY_CANOPY_CLOSE_RATIO_SPD_MAX;
        if (is_update != 0)
            body_work.cnpy_close_ratio += body_work.cnpy_close_ratio_spd;
        if (body_work.cnpy_close_ratio >= 1.0)
        {
            body_work.cnpy_close_ratio = 1f;
            num = 1;
        }
        nnSlerpQuaternion(out dst, ref body_work.cnpy_close_init_quat, ref body_work.cnpy_close_dest_quat, body_work.cnpy_close_ratio);
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        nnMakeQuaternionMatrix(nnsMatrix1, ref dst);
        GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix1, body_work.head_cnm_reg_id);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix1);
        NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
        nnMakeScaleMatrix(nnsMatrix2, 0.0f, 0.0f, 0.0f);
        GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix2, body_work.pole_cnm_reg_id);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix2);
        return num;
    }

    private static void gmBoss5BodyInitScatterFall(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK pObj = GMM_BS_OBJ(body_work);
        pObj.move_flag |= 144U;
        pObj.move_flag &= 4294967294U;
        ObjObjectFieldRectSet(pObj, GMD_BOSS5_BODY_SCT_FIEELD_RECT_SIZE_LEFT, GMD_BOSS5_BODY_SCT_FIEELD_RECT_SIZE_TOP, GMD_BOSS5_BODY_SCT_FIEELD_RECT_SIZE_RIGHT, GMD_BOSS5_BODY_SCT_FIEELD_RECT_SIZE_BOTTOM);
        body_work.sct_land_vib_timer = GMD_BOSS5_BODY_DEFEAT_SCT_FALL_LAND_VIB_TIME;
    }

    private static int gmBoss5BodyUpdateScatterFall(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (body_work.sct_land_vib_timer == 0U)
            return 1;
        if (((int)obsObjectWork.move_flag & 1) == 0)
        {
            int num = (int)(GMD_BOSS5_BODY_DEFEAT_SCT_FALL_LAND_VIB_AMP * (body_work.sct_land_vib_timer / (double)GMD_BOSS5_BODY_DEFEAT_SCT_FALL_LAND_VIB_TIME));
            obsObjectWork.ofst.y = (int)(num * (double)nnSin((int)((GMD_BOSS5_BODY_DEFEAT_SCT_FALL_LAND_VIB_TIME - body_work.sct_land_vib_timer) * GMD_BOSS5_BODY_DEFEAT_SCT_FALL_LAND_VIB_DEG_SPD)));
            --body_work.sct_land_vib_timer;
        }
        return 0;
    }

    private static void gmBoss5BodyInitShakeAccelerate(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        body_work.bsk_shake_acc_ratio = 0.0f;
        body_work.bsk_shake_acc_ratio_spd = 0.0f;
        body_work.bsk_shake_init_spd = obsObjectWork.obj_3d.speed[0];
    }

    private static int gmBoss5BodyUpdateShakeAccelerate(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num = 0;
        body_work.bsk_shake_acc_ratio_spd += GMD_BOSS5_BODY_BERSERK_SHAKE_MOTION_SPD_RATIO_ACC;
        body_work.bsk_shake_acc_ratio += body_work.bsk_shake_acc_ratio_spd;
        if (body_work.bsk_shake_acc_ratio >= 1.0)
        {
            body_work.bsk_shake_acc_ratio = 1f;
            body_work.bsk_shake_acc_ratio_spd = 0.0f;
            num = 1;
        }
        obsObjectWork.obj_3d.speed[0] = (float)(body_work.bsk_shake_acc_ratio * (double)GMD_BOSS5_BODY_BERSERK_SHAKE_MOTION_SPD_DEST + (1.0 - body_work.bsk_shake_acc_ratio) * body_work.bsk_shake_init_spd);
        return num;
    }

    private static void gmBoss5BodyInitCrashStrikeVib(
      GMS_BOSS5_BODY_WORK body_work,
      uint delay)
    {
        body_work.crash_strike_vib_delay_timer = delay;
        body_work.crash_strike_vib_phase = 0;
        body_work.crash_strike_vib_ratio = 0.0f;
    }

    private static int gmBoss5BodyUpdateCrashStrikeVib(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num = 0;
        if (body_work.crash_strike_vib_delay_timer != 0U)
        {
            --body_work.crash_strike_vib_delay_timer;
            return 0;
        }
        body_work.crash_strike_vib_ratio += GMD_BOSS5_BODY_CRASH_STRIKE_BODY_VIB_RATIO_ADD;
        if (body_work.crash_strike_vib_ratio >= 1.0)
        {
            body_work.crash_strike_vib_ratio = 1f;
            num = 1;
        }
        int scale = (int)(GMD_BOSS5_BODY_CRASH_STRIKE_BODY_VIB_SCALE * (1.0 - body_work.crash_strike_vib_ratio));
        body_work.crash_strike_vib_phase = GmBoss5UpdateVib(body_work.crash_strike_vib_phase, scale, ref obsObjectWork.ofst.x, ref obsObjectWork.ofst.y);
        return num;
    }

    private static void gmBoss5BodyInitStartRiseVib(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.start_rise_vib_int_timer = 0U;
    }

    private static void gmBoss5BodyUpdateStartRiseVib(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.start_rise_vib_int_timer != 0U)
        {
            --body_work.start_rise_vib_int_timer;
        }
        else
        {
            float f32 = FX_FX32_TO_F32(FX_Div(GMM_BS_OBJ(body_work).pos.y - body_work.ground_v_pos, GMD_BOSS5_BODY_START_BURY_HEIGHT));
            GmCameraVibrationSet((int)(GMD_BOSS5_BODY_START_RISE_VIB_AMP_MAX * (double)nnSin(AKM_DEGtoA32(180f * f32))), 0, 0);
            body_work.start_rise_vib_int_timer = GMD_BOSS5_BODY_START_RISE_VIB_INTERVAL;
        }
    }

    private static void gmBoss5BodyInitArmPose(GMS_BOSS5_BODY_WORK body_work)
    {
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        nnMakeUnitMatrix(nnsMatrix);
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
            {
                GmBsCmnChangeCNMModeNode(body_work.cnm_mgr_work, body_work.arm_cnm_reg_id[index1][index2], 1U);
                GmBsCmnEnableCNMLocalCoordinate(body_work.cnm_mgr_work, body_work.arm_cnm_reg_id[index1][index2], 1);
                GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, body_work.arm_cnm_reg_id[index1][index2], 1);
                GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix, body_work.arm_cnm_reg_id[index1][index2]);
                nnMakeUnitQuaternion(ref body_work.arm_part_rot_quat[index1][index2]);
            }
            nnMakeUnitMatrix(body_work.rkt_ofst_mtx[index1]);
        }
        body_work.flag |= 16U;
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss5BodyEndArmPose(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294967279U;
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
                GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, body_work.arm_cnm_reg_id[index1][index2], 0);
        }
    }

    private static void gmBoss5BodySetArmPoseParam(
      GMS_BOSS5_BODY_WORK body_work,
      int arm_type,
      int arm_part_idx,
      ref NNS_QUATERNION quat)
    {
        body_work.arm_part_rot_quat[arm_type][arm_part_idx] = quat;
    }

    private static void gmBoss5BodyApplyArmPose(GMS_BOSS5_BODY_WORK body_work)
    {
        NNS_MATRIX[][] nnsMatrixArray = New<NNS_MATRIX>(2, 3);
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
            {
                nnMakeQuaternionMatrix(nnsMatrixArray[index1][index2], ref body_work.arm_part_rot_quat[index1][index2]);
                GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrixArray[index1][index2], body_work.arm_cnm_reg_id[index1][index2]);
            }
        }
        for (int index = 0; index < 2; ++index)
        {
            NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_MATRIX nnsMatrix3 = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_MATRIX nnsMatrix4 = GlobalPool<NNS_MATRIX>.Alloc();
            NNS_MATRIX snmMtx1 = GmBsCmnGetSNMMtx(body_work.snm_work, body_work.armpt_snm_reg_ids[index][0]);
            NNS_MATRIX snmMtx2 = GmBsCmnGetSNMMtx(body_work.snm_work, body_work.armpt_snm_reg_ids[index][1]);
            NNS_MATRIX snmMtx3 = GmBsCmnGetSNMMtx(body_work.snm_work, body_work.armpt_snm_reg_ids[index][2]);
            nnInvertMatrix(nnsMatrix1, snmMtx1);
            nnInvertMatrix(nnsMatrix2, snmMtx2);
            nnInvertMatrix(nnsMatrix3, snmMtx3);
            nnMultiplyMatrix(nnsMatrix4, nnsMatrix3, snmMtx1);
            nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, nnsMatrixArray[index][0]);
            nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, nnsMatrix1);
            nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, snmMtx2);
            nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, nnsMatrixArray[index][1]);
            nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, nnsMatrix2);
            nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, snmMtx3);
            nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, nnsMatrixArray[index][2]);
            nnCopyMatrix(body_work.rkt_ofst_mtx[index], nnsMatrix4);
            GlobalPool<NNS_MATRIX>.Release(nnsMatrix1);
            GlobalPool<NNS_MATRIX>.Release(nnsMatrix2);
            GlobalPool<NNS_MATRIX>.Release(nnsMatrix3);
            GlobalPool<NNS_MATRIX>.Release(nnsMatrix4);
        }
    }

    private static void gmBoss5BodyInitCanopyPartsPose(GMS_BOSS5_BODY_WORK body_work)
    {
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        int[] numArray = new int[4]
        {
      body_work.head_cnm_reg_id,
      body_work.neck_cnm_reg_id,
      body_work.cover_cnm_reg_id,
      body_work.pole_cnm_reg_id
        };
        int length = numArray.Length;
        nnMakeUnitMatrix(nnsMatrix);
        for (int index = 0; index < length; ++index)
        {
            GmBsCmnChangeCNMModeNode(body_work.cnm_mgr_work, numArray[index], 1U);
            GmBsCmnEnableCNMLocalCoordinate(body_work.cnm_mgr_work, numArray[index], 1);
            GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, numArray[index], 1);
            GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix, numArray[index]);
        }
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss5BodyEndCanopyPartsPose(GMS_BOSS5_BODY_WORK body_work)
    {
        int[] numArray = new int[4]
        {
      body_work.head_cnm_reg_id,
      body_work.neck_cnm_reg_id,
      body_work.cover_cnm_reg_id,
      body_work.pole_cnm_reg_id
        };
        int length = numArray.Length;
        for (int index = 0; index < length; ++index)
            GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, numArray[index], 0);
    }

    private static int gmBoss5BodyTryTransitCrashWall(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (body_work.sub_seq == -1)
        {
            if (body_work.state == 3)
            {
                if (((int)obsObjectWork.move_flag & 4) != 0)
                {
                    gmBoss5BodyStartSubsequence(body_work, 0);
                    return 1;
                }
            }
            else if (body_work.state == 4 && ((int)obsObjectWork.move_flag & 8) != 0)
            {
                gmBoss5BodyStartSubsequence(body_work, 1);
                return 1;
            }
        }
        return 0;
    }

    private static void gmBoss5BodyResumeMoveFast(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.sub_seq == 0)
            gmBoss5BodyChangeState(body_work, 4, body_work.strat_state, 1);
        else
            gmBoss5BodyChangeState(body_work, 3, body_work.strat_state, 1);
    }

    private static int gmBoss5BodyReceiveSignalRocketReturned(GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 67108864) != 0)
        {
            body_work.flag &= 4227858431U;
            return 1;
        }
        if (((int)body_work.flag & 33554432) == 0)
            return 0;
        body_work.flag &= 4261412863U;
        return 1;
    }

    private static void gmBoss5BodyInitCallbacks(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBsCmnInitBossMotionCBSystem(obj_work, body_work.bmcb_mgr);
        GmBsCmnCreateSNMWork(body_work.snm_work, obj_work.obj_3d._object, (ushort)GMD_BOSS5_BODY_NODE_SNM_NUM);
        GmBsCmnAppendBossMotionCallback(body_work.bmcb_mgr, body_work.snm_work.bmcb_link);
        body_work.body_snm_reg_id = GmBsCmnRegisterSNMNode(body_work.snm_work, GMD_BOSS5_BODY_NODE_IDX_BODY);
        body_work.lfoot_snm_reg_id = GmBsCmnRegisterSNMNode(body_work.snm_work, GMD_BOSS5_BODY_NODE_IDX_FOOT_L);
        body_work.rfoot_snm_reg_id = GmBsCmnRegisterSNMNode(body_work.snm_work, GMD_BOSS5_BODY_NODE_IDX_FOOT_R);
        body_work.leg_snm_reg_ids[0] = GmBsCmnRegisterSNMNode(body_work.snm_work, GMD_BOSS5_BODY_NODE_IDX_LEG_L);
        body_work.leg_snm_reg_ids[1] = GmBsCmnRegisterSNMNode(body_work.snm_work, GMD_BOSS5_BODY_NODE_IDX_LEG_R);
        body_work.pole_snm_reg_id = GmBsCmnRegisterSNMNode(body_work.snm_work, GMD_BOSS5_BODY_NODE_IDX_POLE);
        body_work.groin_snm_reg_ids[0] = GmBsCmnRegisterSNMNode(body_work.snm_work, GMD_BOSS5_BODY_NODE_IDX_GROIN_L);
        body_work.groin_snm_reg_ids[1] = GmBsCmnRegisterSNMNode(body_work.snm_work, GMD_BOSS5_BODY_NODE_IDX_GROIN_R);
        body_work.nozzle_snm_reg_ids[0] = GmBsCmnRegisterSNMNode(body_work.snm_work, GMD_BOSS5_BODY_NODE_IDX_NOZZLE_L);
        body_work.nozzle_snm_reg_ids[1] = GmBsCmnRegisterSNMNode(body_work.snm_work, GMD_BOSS5_BODY_NODE_IDX_NOZZLE_R);
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
                body_work.armpt_snm_reg_ids[index1][index2] = GmBsCmnRegisterSNMNode(body_work.snm_work, arm_part_node_ids[index1][index2]);
        }
        GmBsCmnCreateCNMMgrWork(body_work.cnm_mgr_work, obj_work.obj_3d._object, (ushort)GMD_BOSS5_BODY_NODE_CNM_NUM);
        GmBsCmnInitCNMCb(obj_work, body_work.cnm_mgr_work);
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
                body_work.arm_cnm_reg_id[index1][index2] = GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, arm_part_node_ids[index1][index2]);
        }
        body_work.head_cnm_reg_id = GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, GMD_BOSS5_BODY_NODE_IDX_HEAD);
        body_work.neck_cnm_reg_id = GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, GMD_BOSS5_BODY_NODE_IDX_NECK);
        body_work.cover_cnm_reg_id = GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, GMD_BOSS5_BODY_NODE_IDX_COVER);
        body_work.pole_cnm_reg_id = GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, GMD_BOSS5_BODY_NODE_IDX_POLE);
    }

    private static void gmBoss5BodyReleaseCallbacks(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBsCmnClearBossMotionCBSystem(obj_work);
        GmBsCmnDeleteSNMWork(body_work.snm_work);
        GmBsCmnClearCNMCb(obj_work);
        GmBsCmnDeleteCNMMgrWork(body_work.cnm_mgr_work);
    }

    private static void gmBoss5BodyRegisterScatterPartsCNM(GMS_BOSS5_BODY_WORK body_work)
    {
        for (int index = 0; index < 22; ++index)
            body_work.scatter_cnm_reg_ids[index] = GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, gm_boss5_body_scatter_parts_cnm_node_id_tbl[index]);
    }

    private static void gmBoss5BodyDamageDefFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        GMS_BOSS5_BODY_WORK body_work = (GMS_BOSS5_BODY_WORK)parentObj1;
        int num1 = 0;
        int num2 = 0;
        if (parentObj2 != null)
        {
            if (1 == parentObj2.obj_type)
                num1 = 1;
            else if (2 == parentObj2.obj_type && ((GMS_ENEMY_COM_WORK)parentObj2).eve_rec.id == 332)
                num2 = 1;
        }
        if (num1 == 0 && num2 == 0)
            return;
        if (num1 != 0)
        {
            gmBoss5BodySetPlyRebound((GMS_PLAYER_WORK)parentObj2, body_work);
            GMM_PAD_VIB_SMALL_TIME(30f);
        }
        else
        {
            gmBoss5BodyStartSubsequence(body_work, 2);
            GMM_PAD_VIB_SMALL();
            GmSoundPlaySE("FinalBoss15");
        }
        gmBoss5BodySetNoHitTime(body_work);
        GmSoundPlaySE("Boss0_01");
        GmBoss5EfctCreateDamage(body_work);
        if (((int)body_work.flag & 1) != 0 && num2 == 0)
            return;
        gmBoss5BodyExecDamageRoutine(body_work);
    }

    private static void gmBoss5BodyOutFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK gmsBosS5BodyWork = (GMS_BOSS5_BODY_WORK)obj_work;
        GmBsCmnUpdateCNMParam(obj_work, gmsBosS5BodyWork.cnm_mgr_work);
        ObjDrawActionSummary(obj_work);
        gmsBosS5BodyWork.grdmv_pivot_pos.Assign(obj_work.pos);
        gmsBosS5BodyWork.pivot_prev_pos.Assign(obj_work.pos);
    }

    private static void gmBoss5BodyRecFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK gmsBosS5BodyWork = (GMS_BOSS5_BODY_WORK)obj_work;
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
                ObjObjectRectRegist(obj_work, gmsBosS5BodyWork.sub_rect_work[index1][index2]);
        }
    }

    private static void gmBoss5BodyChangeState(
      GMS_BOSS5_BODY_WORK body_work,
      int state,
      int strat_state)
    {
        gmBoss5BodyChangeState(body_work, state, strat_state, 0);
    }

    private static void gmBoss5BodyChangeState(
      GMS_BOSS5_BODY_WORK body_work,
      int state,
      int strat_state,
      int is_wrapped)
    {
        UNREFERENCED_PARAMETER(is_wrapped);
        MPP_VOID_GMS_BOSS5_BODY_WORK gmsBosS5BodyWork = gm_boss5_body_state_leave_func_tbl[body_work.state];
        if (gmsBosS5BodyWork != null)
            gmsBosS5BodyWork(body_work);
        body_work.prev_state = body_work.state;
        body_work.state = state;
        body_work.sub_seq = -1;
        body_work.strat_state = strat_state;
        GMS_BOSS5_BODY_STATE_ENTER_INFO bodyStateEnterInfo = gm_boss5_body_state_enter_info_tbl[body_work.state];
        if (bodyStateEnterInfo.enter_func == null)
            return;
        bodyStateEnterInfo.enter_func(body_work);
    }

    private static void gmBoss5BodyStartSubsequence(
      GMS_BOSS5_BODY_WORK body_work,
      int sub_seq)
    {
        GMS_BOSS5_BODY_SUBSEQ_ENTER_INFO bodySubseqEnterInfo = gm_boss5_body_sub_seq_enter_func_tbl[sub_seq];
        body_work.sub_seq = sub_seq;
        if (bodySubseqEnterInfo.enter_func == null)
            return;
        bodySubseqEnterInfo.enter_func(body_work);
    }

    private static void gmBoss5BodyWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK body_work = (GMS_BOSS5_BODY_WORK)obj_work;
        if (((int)body_work.mgr_work.flag & 1) == 0)
            return;
        gmBoss5BodyInitCallbacks(body_work);
        GmBoss5RocketSpawnConnected(body_work, 0);
        GmBoss5RocketSpawnConnected(body_work, 1);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5BodyMain);
        gmBoss5BodyChangeState(body_work, 1, 1);
    }

    private static void gmBoss5BodyMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK gmsBosS5BodyWork = (GMS_BOSS5_BODY_WORK)obj_work;
        int num = 0;
        gmBoss5BodyUpdatePokeTriggerLimitTime(gmsBosS5BodyWork);
        gmBoss5BodyUpdateNoHitTime(gmsBosS5BodyWork);
        if (gmsBosS5BodyWork.sub_seq != 0 && gmsBosS5BodyWork.sub_seq != 1)
            gmBoss5BodyUpdateMoveFastTime(gmsBosS5BodyWork);
        if (((int)gmsBosS5BodyWork.flag & 131072) == 0)
        {
            if (((int)gmsBosS5BodyWork.flag & 4194304) != 0)
            {
                gmsBosS5BodyWork.flag &= 2143289343U;
                GmBsCmnInitObject3DNNDamageFlicker(obj_work, gmsBosS5BodyWork.flk_work, GMD_BOSS5_BODY_DMG_FLICKER_RADIUS);
                gmBoss5BodyProceedToDefeatState(gmsBosS5BodyWork);
                num = 1;
            }
            if (num == 0 && gmBoss5BodyTryTransitCrashWall(gmsBosS5BodyWork) != 0)
            {
                num = 1;
                GmSoundPlaySE("FinalBoss12");
            }
        }
        if (num == 0 && gmsBosS5BodyWork.proc_update != null)
            gmsBosS5BodyWork.proc_update(gmsBosS5BodyWork);
        if (((int)gmsBosS5BodyWork.flag & int.MinValue) != 0)
        {
            gmsBosS5BodyWork.flag &= int.MaxValue;
            GmBsCmnInitObject3DNNDamageFlicker(obj_work, gmsBosS5BodyWork.flk_work, GMD_BOSS5_BODY_DMG_FLICKER_RADIUS);
        }
        GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, gmsBosS5BodyWork.flk_work);
        if (((int)gmsBosS5BodyWork.flag & 4096) != 0)
            GmBoss5EfctTryStartLeakage(gmsBosS5BodyWork);
        else
            GmBoss5EfctEndLeakage(gmsBosS5BodyWork);
    }

    private static void gmBoss5BodyStateEnterStart(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss5BodySetActionWhole(body_work, 0, 1);
        gmBoss5BodySetDirection(body_work, 0);
        obsObjectWork.move_flag &= 4294967039U;
        obsObjectWork.move_flag |= 128U;
        body_work.flag |= 8192U;
        obsObjectWork.spd.y = obsObjectWork.spd_fall_max;
        obsObjectWork.pos.z = GMD_BOSS5_BG_FARSIDE_POS_Z;
        gmBoss5BodyChangeRectSetting(body_work, 2);
        GmBoss5EggCreate(body_work, obsObjectWork.pos.x, obsObjectWork.pos.y);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStartWithPlacement);
    }

    private static void gmBoss5BodyStateLeaveStart(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss5RestoreCameraSlideForNarrowScreen(body_work.mgr_work);
        gmBoss5BodyEndCanopyPartsPose(body_work);
        gmBoss5BodyChangeRectSettingDefault(body_work);
        body_work.flag &= 4294959103U;
        obsObjectWork.move_flag &= 4294967039U;
        obsObjectWork.move_flag |= 128U;
        obsObjectWork.pos.z = GMD_BOSS5_DEFAULT_POS_Z;
    }

    private static void gmBoss5BodyStateUpdateStartWithPlacement(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (((int)obsObjectWork.move_flag & 1) == 0)
            return;
        body_work.ground_v_pos = obsObjectWork.pos.y;
        obsObjectWork.move_flag |= 256U;
        obsObjectWork.move_flag &= 4294967167U;
        obsObjectWork.pos.y += GMD_BOSS5_BODY_START_BURY_HEIGHT;
        GmBoss5CtpltCreate(body_work);
        body_work.mgr_work.flag |= 2097152U;
        gmBoss5MgrSetDemoRunDestPos(body_work.mgr_work, obsObjectWork.pos.x + GMD_BOSS5_PLY_OP_DEMO_RUN_DEST_X_OFST_FROM_BODY);
        gmBoss5BodyInitCanopyPartsPose(body_work);
        gmBoss5BodyInitCloseCanopy(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStartWithWaitEggRide);
    }

    private static void gmBoss5BodyStateUpdateStartWithWaitEggRide(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.mgr_work.flag & 524288) != 0 && ((int)body_work.mgr_work.flag & 1048576) == 0)
        {
            body_work.mgr_work.flag |= 1048576U;
            gmBoss5SetCameraSlideForNarrowScreen(body_work.mgr_work);
        }
        gmBoss5BodyUpdateCloseCanopy(body_work, 0);
        if (((int)body_work.flag & 16777216) == 0)
            return;
        body_work.flag &= 4278190079U;
        gmBoss5BodyRecordGapAdjustmentDest(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStartWithCockpitClose);
    }

    private static void gmBoss5BodyStateUpdateStartWithCockpitClose(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (gmBoss5BodyUpdateCloseCanopy(body_work, 1) == 0)
            return;
        GmSoundPlaySE("FinalBoss01");
        body_work.mgr_work.flag |= 33554432U;
        body_work.wait_timer = GMD_BOSS5_BODY_START_WAIT_RISE_TIME;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStartWithWaitRise);
    }

    private static void gmBoss5BodyStateUpdateStartWithWaitRise(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            obsObjectWork.spd.y = GMD_BOSS5_BODY_START_RISE_SPD_Y;
            gmBoss5BodyInitStartRiseVib(body_work);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStartWithRise);
        }
    }

    private static void gmBoss5BodyStateUpdateStartWithRise(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GMS_BOSS5_MGR_WORK mgrWork = body_work.mgr_work;
        gmBoss5BodyUpdateStartRiseVib(body_work);
        if (obj_work.pos.y > body_work.ground_v_pos)
            return;
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag &= 4294967039U;
        obj_work.move_flag |= 128U;
        mgrWork.flag |= 8388608U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStartWithWaitCtplt);
    }

    private static void gmBoss5BodyStateUpdateStartWithWaitCtplt(GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.mgr_work.flag & 16777216) == 0)
            return;
        gmBoss5RestoreCameraSlideForNarrowScreen(body_work.mgr_work);
        body_work.wait_timer = GMD_BOSS5_BODY_START_WAIT_END_TIME;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStartWithWaitEnd);
    }

    private static void gmBoss5BodyStateUpdateStartWithWaitEnd(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else
            gmBoss5BodyProceedToNextSeqNml(body_work);
    }

    private static void gmBoss5BodyStateEnterMoveNml(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss5BodySetActionWhole(body_work, 2, 1);
        gmBoss5BodyInitWalk(body_work);
        gmBoss5BodyInitMonitoringWalkEnd(body_work);
        gmBoss5BodyInitWalkGroundingEffects(body_work);
        gmBoss5BodyChangeRectSettingDefault(body_work);
        obsObjectWork.move_flag &= 4294966271U;
        obsObjectWork.move_flag |= 524288U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveNmlWithLoop);
        if (gmBoss5BodyIsPlayerBehind(body_work) == 0)
            return;
        gmBoss5BodySetActionWhole(body_work, 3);
        gmBoss5BodyInitWalkAbortRecovery(body_work, body_work.cur_move_phase_type);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveNmlWithAbort);
    }

    private static void gmBoss5BodyStateLeaveMoveNml(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss5BodyClearAdjustMtnBlendHGap(body_work);
        body_work.flag &= 4286578687U;
        gmBoss5BodyClearPokeTriggerLimitTime(body_work);
        gmBoss5BodyEndPoke(body_work);
        obsObjectWork.move_flag &= 4294443007U;
        obsObjectWork.move_flag |= 1024U;
        gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateMoveNmlWithLoop(GMS_BOSS5_BODY_WORK body_work)
    {
        if (gmBoss5BodyIsPoking(body_work) != 0 && gmBoss5BodyUpdatePoke(body_work) != 0)
            gmBoss5BodyEndPoke(body_work);
        gmBoss5BodyUpdateWalkGroundingEffects(body_work);
        if (gmBoss5BodyUpdateWalk(body_work) != 0 && GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) != 0 && gmBoss5BodyIsPoking(body_work) == 0)
        {
            gmBoss5BodyProceedToNextSeqNml(body_work);
        }
        else
        {
            int leg_type = 0;
            if (gmBoss5BodyUpdateMonitoringWalkEnd(body_work, ref leg_type) != 0)
            {
                gmBoss5BodySetActionWhole(body_work, 3);
                gmBoss5BodyInitWalkAbortRecoveryByLegType(body_work, leg_type);
                body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveNmlWithAbort);
            }
            else
            {
                if (((int)body_work.flag & 8388608) == 0)
                    return;
                body_work.flag &= 4286578687U;
                gmBoss5BodyInitPoke(body_work);
            }
        }
    }

    private static void gmBoss5BodyStateUpdateMoveNmlWithAbort(GMS_BOSS5_BODY_WORK body_work)
    {
        if (gmBoss5BodyIsPoking(body_work) != 0 && gmBoss5BodyUpdatePoke(body_work) != 0)
            gmBoss5BodyEndPoke(body_work);
        if (gmBoss5BodyUpdateWalkAbortRecovery(body_work) == 0 || gmBoss5BodyIsPoking(body_work) != 0)
            return;
        gmBoss5BodyProceedToNextSeqNml(body_work);
    }

    private static void gmBoss5BodyStateEnterMoveFast(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss5BodyChangeRectSettingDefault(body_work);
        if (gmBoss5BodyIsMoveFastEnd(body_work) != 0)
        {
            gmBoss5BodySetActionWhole(body_work, 20);
            gmBoss5BodyRecordGapAdjustmentSrc(body_work);
            gmBoss5BodyInitAdjustMtnBlendHGap(body_work, body_work.whole_act_id, 0);
            gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
            body_work.wait_timer = GMD_BOSS5_BODY_RUN_RECOVER_TIMEOUT_FRAME;
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveFastWithRecover);
        }
        else
        {
            if (body_work.prev_state == 10)
            {
                if (body_work.whole_act_id != 45)
                    gmBoss5BodySetActionWhole(body_work, 45);
                gmBoss5BodyClearAdjustMtnBlendHGap(body_work);
            }
            else
            {
                if (gmBoss5BodyIsMoveFastDirFwd(body_work) != 0)
                    gmBoss5BodySetActionWhole(body_work, 4);
                else
                    gmBoss5BodySetActionWhole(body_work, 11);
                gmBoss5BodyRecordGapAdjustmentSrc(body_work);
                gmBoss5BodyInitAdjustMtnBlendHGap(body_work, body_work.whole_act_id, 0);
                gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
            }
            obsObjectWork.move_flag &= 4294966271U;
            obsObjectWork.move_flag |= 524288U;
            body_work.cur_run_type = 1;
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveFastWithPrep);
        }
    }

    private static void gmBoss5BodyStateLeaveMoveFast(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.move_flag &= 4294967279U;
        obsObjectWork.move_flag &= 4294443007U;
        obsObjectWork.move_flag |= 1024U;
        gmBoss5BodyClearAdjustMtnBlendHGap(body_work);
        gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithPrep(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (((int)obj_work.obj_3d.flag & 1) != 0)
            gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
        else
            gmBoss5BodyClearAdjustMtnBlendHGap(body_work);
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        gmBoss5BodyClearAdjustMtnBlendHGap(body_work);
        obj_work.spd.x = GMD_BOSS5_BODY_RUN_FWD_JUMP_INIT_SPD_X;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -obj_work.spd.x;
        if (gmBoss5BodyIsMoveFastDirFwd(body_work) != 0)
        {
            gmBoss5BodySetActionWhole(body_work, 5);
        }
        else
        {
            obj_work.spd.x = FX_Mul(-obj_work.spd.x, GMD_BOSS5_BODY_RUN_BWD_SPD_X_FACTOR);
            gmBoss5BodySetActionWhole(body_work, 12);
        }
        gmBoss5BodySwitchEnableLegRectOneSide(body_work, 1);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveFastWithJump);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithJump(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (GmBsCmnIsActionEndFlexibly(obj_work, GMD_BOSS5_BODY_RUN_ACT_FRAME_OVERRUN_ALLOW_RATIO) == 0)
            return;
        int num = gmBoss5BodyIsMoveFastDirFwd(body_work);
        if (body_work.cur_run_type == 1)
        {
            if (num != 0)
                gmBoss5BodySetActionWhole(body_work, 6);
            else
                gmBoss5BodySetActionWhole(body_work, 13);
        }
        else if (num != 0)
            gmBoss5BodySetActionWhole(body_work, 9);
        else
            gmBoss5BodySetActionWhole(body_work, 16);
        obj_work.spd.x = GMD_BOSS5_BODY_RUN_FWD_FLY_INIT_SPD_X;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -obj_work.spd.x;
        obj_work.spd.y = GMD_BOSS5_BODY_RUN_FWD_FLY_INIT_SPD_Y;
        if (num == 0)
            obj_work.spd.x = FX_Mul(-obj_work.spd.x, GMD_BOSS5_BODY_RUN_BWD_SPD_X_FACTOR);
        obj_work.move_flag |= 144U;
        obj_work.move_flag &= 4294967294U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveFastWithAir);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithAir(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (((int)obj_work.move_flag & 1) == 0)
            return;
        int num = gmBoss5BodyIsMoveFastDirFwd(body_work);
        GmBsCmnSetObjSpdZero(obj_work);
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        gmBoss5BodyInitRunGroundingEffects(body_work, body_work.cur_run_type, GMD_BOSS5_BODY_RUN_GROUNDING_EFCT_DELAY);
        if (body_work.cur_run_type == 1)
        {
            if (num != 0)
                gmBoss5BodySetActionWhole(body_work, 7);
            else
                gmBoss5BodySetActionWhole(body_work, 14);
        }
        else if (num != 0)
            gmBoss5BodySetActionWhole(body_work, 10);
        else
            gmBoss5BodySetActionWhole(body_work, 17);
        if (gmBoss5BodyIsMoveFastEnd(body_work) != 0)
        {
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveFastWithPreRecover);
            body_work.wait_timer = GMD_BOSS5_BODY_RUN_PRE_RECOVER_TIMEOUT_FRAME;
        }
        else
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveFastWithLand);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithLand(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        gmBoss5BodyUpdateRunGroundingEffects(body_work);
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        int num = gmBoss5BodyIsMoveFastDirFwd(body_work);
        if (body_work.cur_run_type == 1)
        {
            body_work.cur_run_type = 0;
            if (num != 0)
                gmBoss5BodySetActionWhole(body_work, 8);
            else
                gmBoss5BodySetActionWhole(body_work, 15);
            gmBoss5BodySwitchEnableLegRectOneSide(body_work, 0);
        }
        else
        {
            body_work.cur_run_type = 1;
            if (num != 0)
                gmBoss5BodySetActionWhole(body_work, 5);
            else
                gmBoss5BodySetActionWhole(body_work, 12);
            gmBoss5BodySwitchEnableLegRectOneSide(body_work, 1);
        }
        obj_work.spd.x = GMD_BOSS5_BODY_RUN_FWD_JUMP_INIT_SPD_X;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -obj_work.spd.x;
        if (num == 0)
            obj_work.spd.x = FX_Mul(-obj_work.spd.x, GMD_BOSS5_BODY_RUN_BWD_SPD_X_FACTOR);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveFastWithJump);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithPreRecover(
      GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num = 0;
        gmBoss5BodyUpdateRunGroundingEffects(body_work);
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else
            num = 1;
        if (((int)obsObjectWork.obj_3d.flag & 1) != 0 && num == 0)
            return;
        gmBoss5BodySetActionWhole(body_work, 20);
        gmBoss5BodyRecordGapAdjustmentSrc(body_work);
        if (body_work.cur_run_type == 1)
            gmBoss5BodyInitAdjustMtnBlendHGap(body_work, body_work.whole_act_id, 1);
        else
            gmBoss5BodyInitAdjustMtnBlendHGap(body_work, body_work.whole_act_id, 0);
        gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
        body_work.wait_timer = GMD_BOSS5_BODY_RUN_RECOVER_TIMEOUT_FRAME;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateMoveFastWithRecover);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithRecover(
      GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num = 0;
        gmBoss5BodyUpdateRunGroundingEffects(body_work);
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else
            num = 1;
        if (((int)obsObjectWork.obj_3d.flag & 1) == 0 || num != 0)
            gmBoss5BodyProceedToNextSeqStr(body_work);
        else
            gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
    }

    private static void gmBoss5BodyStateEnterStomp(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodySetActionWhole(body_work, 21);
        body_work.wait_timer = GMD_BOSS5_BODY_STOMP_IGNITE_TIME;
        gmBoss5BodyChangeRectSettingDefault(body_work);
        GmBoss5EfctStartJet(body_work);
        GmBoss5EfctStartJetSmoke(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStompWithPrep);
    }

    private static void gmBoss5BodyStateLeaveStomp(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.move_flag &= 4294967023U;
        obsObjectWork.move_flag |= 128U;
        GmBoss5EfctEndJetSmoke(body_work);
        GmBoss5EfctEndJet(body_work);
        gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateStompWithPrep(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            gmBoss5BodySetActionWhole(body_work, 22);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStompWithHover);
        }
    }

    private static void gmBoss5BodyStateUpdateStompWithHover(GMS_BOSS5_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        gmBoss5BodySetActionWhole(body_work, 23);
        gmBoss5BodyInitStompFlyUp(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStompWithFlyUp);
    }

    private static void gmBoss5BodyStateUpdateStompWithFlyUp(GMS_BOSS5_BODY_WORK body_work)
    {
        if (gmBoss5BodyCheckJetSmokeClearTiming(body_work) != 0)
            GmBoss5EfctEndJetSmoke(body_work);
        if (gmBoss5BodyUpdateStompFlyUp(body_work) == 0)
            return;
        GmBoss5EfctEndJetSmoke(body_work);
        GmBoss5EfctEndJet(body_work);
        GmBoss5EfctTargetCursorInit(body_work);
        body_work.flag |= 2U;
        gmBoss5BodyInitPlySearch(body_work, (int)GMD_BOSS5_BODY_STOMP_SEARCH_DELAY);
        body_work.wait_timer = GMD_BOSS5_BODY_STOMP_WAIT_TIME;
        gmBoss5BodyInitPlayTargetSe(body_work, GMD_BOSS5_BODY_SE_TARGET_INIT_INTERVAL);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStompWithWait);
    }

    private static void gmBoss5BodyStateUpdateStompWithWait(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer >= GMD_BOSS5_BODY_STOMP_NO_SEARCH_TIME)
            gmBoss5BodyUpdatePlySearch(body_work);
        gmBoss5BodyUpdatePlayTargetSe(body_work);
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
            VecFx32 pos = new VecFx32();
            body_work.flag &= 4294967293U;
            GmBoss5BodyGetPlySearchPos(body_work, out pos);
            obsObjectWork.pos.x = gmBoss5BodyGetStompFallPosX(body_work, pos.x);
            gmBoss5BodySetActionWhole(body_work, 24);
            gmBoss5BodySetDirection(body_work, gmBoss5BodyGetStompFallDirectionType(body_work));
            gmBoss5BodyInitStompFall(body_work);
            if (gmBoss5BodySeqIsStr(body_work) != 0)
                gmBoss5BodyChangeRectSetting(body_work, 4);
            else
                gmBoss5BodyChangeRectSetting(body_work, 3);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStompWithFall);
        }
    }

    private static void gmBoss5BodyStateUpdateStompWithFall(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (gmBoss5BodyUpdateStompFall(body_work) == 0)
            return;
        GmSoundPlaySE("FinalBoss06");
        GMM_PAD_VIB_MID_TIME(30f);
        obsObjectWork.move_flag &= 4294967279U;
        gmBoss5BodySetActionWhole(body_work, 25);
        gmBoss5BodyChangeRectSettingDefault(body_work);
        GmBoss5EfctCreateLandingShockwave(body_work);
        gmBoss5Vibration(3);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateStompWithLand);
    }

    private static void gmBoss5BodyStateUpdateStompWithLand(GMS_BOSS5_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        if (body_work.state == 6)
            gmBoss5BodyProceedToNextSeqStr(body_work);
        else
            gmBoss5BodyProceedToNextSeqNml(body_work);
    }

    private static void gmBoss5BodyStateEnterCrash(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5MgrSetAlarmLevel(body_work.mgr_work, 1);
        body_work.flag |= 65536U;
        body_work.flag |= 512U;
        gmBoss5BodySetActionWhole(body_work, 26);
        gmBoss5BodyChangeRectSettingDefault(body_work);
        GmBoss5EfctStartJet(body_work);
        GmBoss5EfctStartJetSmoke(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateCrashWithPrep);
    }

    private static void gmBoss5BodyStateLeaveCrash(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        body_work.flag &= 4294959103U;
        GMM_PAD_VIB_STOP();
        obsObjectWork.move_flag &= 4294967023U;
        obsObjectWork.move_flag |= 128U;
        body_work.flag &= 4294966783U;
        GmBoss5EfctEndJetSmoke(body_work);
        gmBoss5RestoreCameraLift(body_work.mgr_work);
        GmBoss5EfctEndJet(body_work);
        gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateCrashWithPrep(GMS_BOSS5_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        gmBoss5BodyDecideCrashFallPosX(body_work);
        gmBoss5BodySetActionWhole(body_work, 27);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateCrashWithStartFly);
    }

    private static void gmBoss5BodyStateUpdateCrashWithStartFly(GMS_BOSS5_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        gmBoss5BodySetActionWhole(body_work, 28);
        gmBoss5BodyInitCrashFlyUp(body_work);
        gmBoss5SetCameraLift(body_work.mgr_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateCrashWithFlyUp);
    }

    private static void gmBoss5BodyStateUpdateCrashWithFlyUp(GMS_BOSS5_BODY_WORK body_work)
    {
        if (gmBoss5BodyCheckJetSmokeClearTiming(body_work) != 0)
            GmBoss5EfctEndJetSmoke(body_work);
        if (gmBoss5BodyUpdateCrashFlyUp(body_work) == 0)
            return;
        body_work.mgr_work.flag |= 67108864U;
        GmBoss5EfctEndJetSmoke(body_work);
        GmBoss5EfctEndJet(body_work);
        body_work.wait_timer = GMD_BOSS5_BODY_CRASH_WAIT_TIME;
        GmBoss5EfctCrashCursorInit(body_work, gmBoss5BodyGetCrashFallPosX(body_work), GMD_BOSS5_BODY_CRASH_WAIT_TIME - GMD_BOSS5_BODY_CRASH_CURSOR_SPAWN_TIME_TRHESHOLD);
        body_work.flag |= 2U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateCrashWithWait);
    }

    private static void gmBoss5BodyStateUpdateCrashWithWait(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            GMM_BS_OBJ(body_work).pos.x = gmBoss5BodyGetCrashFallPosX(body_work);
            gmBoss5BodySetDirection(body_work, 0);
            gmBoss5BodySetActionWhole(body_work, 29);
            gmBoss5BodyInitCrashFall(body_work);
            gmBoss5BodyChangeRectSetting(body_work, 5);
            GmSoundPlaySE("FinalBoss17");
            gmBoss5RestoreCameraLift(body_work.mgr_work);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateCrashWithFall);
        }
    }

    private static void gmBoss5BodyStateUpdateCrashWithFall(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (gmBoss5BodyUpdateCrashFall(body_work) == 0)
            return;
        body_work.flag &= 4294967293U;
        obsObjectWork.move_flag &= 4294967279U;
        gmBoss5BodySetActionWhole(body_work, 30);
        GmBoss5EfctCreateStrikeShockwave(body_work, GMD_BOSS5_BODY_CRASH_STRIKE_SW_CREATE_DELAY);
        gmBoss5DelayedVibration(5, GMD_BOSS5_BODY_CRASH_STRIKE_VIB_START_DELAY);
        gmBoss5BodyInitCrashStrikeVib(body_work, GMD_BOSS5_BODY_CRASH_STRIKE_BODY_VIB_START_DELAY);
        gmBoss5BodyChangeRectSetting(body_work, 6);
        gmBoss5BodyForceEndLeakage(body_work);
        GmBoss5EfctCreateLandingShockwave(body_work);
        GmBoss5EfctCreateCrashLandingSmoke(body_work);
        gmBoss5BodyTryImmobilizePlayer(body_work);
        gmBoss5Vibration(4);
        GmPadVibSet(1, -1f, 32768, 32768, -1f, 0.0f, 0.0f, 32768U);
        gmBoss5DelayedSePlayback("FinalBoss18", GMD_BOSS5_BODY_CRASH_STRIKE_SE_START_DELAY);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateCrashWithLand);
    }

    private static void gmBoss5BodyStateUpdateCrashWithLand(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodyUpdateCrashStrikeVib(body_work);
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        body_work.mgr_work.flag |= 536870912U;
        body_work.wait_timer = GMD_BOSS5_BODY_CRASH_LANDED_IDLE_TIME;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateCrashWithIdle);
    }

    private static void gmBoss5BodyStateUpdateCrashWithIdle(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss5BodyUpdateCrashStrikeVib(body_work);
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            GMM_PAD_VIB_STOP();
            gmBoss5BodySetActionWhole(body_work, 31);
            gmBoss5BodyInitCrashSink(body_work);
            gmBoss5BodyChangeRectSetting(body_work, 7);
            obsObjectWork.flag |= 2U;
            body_work.flag |= 8192U;
            body_work.flag |= 131072U;
            body_work.mgr_work.flag |= 1073741824U;
            GmGmkCamScrLimitRelease(8);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateCrashWithSink);
        }
    }

    private static void gmBoss5BodyStateUpdateCrashWithSink(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodyUpdateCrashSink(body_work);
    }

    private static void gmBoss5BodyStateEnterRpc(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodySetActionWhole(body_work, 32);
        gmBoss5BodyChangeRectSettingDefault(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateRpcWithPrep);
    }

    private static void gmBoss5BodyStateLeaveRpc(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 2281701375U;
        body_work.flag &= 4294967283U;
        gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateRpcWithPrep(GMS_BOSS5_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        body_work.flag |= 4U;
        if (body_work.state == 9)
            GmBoss5RocketLaunchStrong(body_work, 0);
        else
            GmBoss5RocketLaunchNormal(body_work, 0);
        body_work.wait_timer = body_work.state != 9 ? gmBoss5BodySeqGetRpcNmlSearchTime(body_work) : gmBoss5BodySeqGetRpcStrSearchTime(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateRpcWithSearch);
    }

    private static void gmBoss5BodyStateUpdateRpcWithSearch(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            gmBoss5BodySetActionWhole(body_work, 33);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateRpcWithLaunchFirst);
        }
    }

    private static void gmBoss5BodyStateUpdateRpcWithLaunchFirst(GMS_BOSS5_BODY_WORK body_work)
    {
        if (GMM_BS_OBJ(body_work).obj_3d.frame[0] < (double)GMD_BOSS5_BODY_RPUNCH_LAUNCH_TIMING_DELAY)
            return;
        body_work.flag |= 268435456U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateRpcWithWaitReturnFirst);
    }

    private static void gmBoss5BodyStateUpdateRpcWithWaitReturnFirst(
      GMS_BOSS5_BODY_WORK body_work)
    {
        int num = 0;
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) != 0)
            num = 1;
        if (((int)body_work.flag & 67108864) != 0)
        {
            body_work.flag &= 4227858431U;
            num = 1;
            body_work.flag &= 4294967291U;
            gmBoss5BodySetActionWhole(body_work, 34);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateRpcWithLaunchSecond);
        }
        if (num == 0 || ((int)body_work.flag & 8) != 0)
            return;
        body_work.flag |= 8U;
        if (body_work.state == 9)
            GmBoss5RocketLaunchStrong(body_work, 1);
        else
            GmBoss5RocketLaunchNormal(body_work, 1);
    }

    private static void gmBoss5BodyStateUpdateRpcWithLaunchSecond(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (GMM_BS_OBJ(body_work).obj_3d.frame[0] < (double)GMD_BOSS5_BODY_RPUNCH_LAUNCH_TIMING_DELAY)
            return;
        body_work.flag |= 134217728U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateRpcWithWaitReturnSecond);
    }

    private static void gmBoss5BodyStateUpdateRpcWithWaitReturnSecond(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 33554432) == 0)
            return;
        body_work.flag &= 4261412863U;
        body_work.flag &= 4294967287U;
        gmBoss5BodySetActionWhole(body_work, 35);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateRpcWithRecover);
    }

    private static void gmBoss5BodyStateUpdateRpcWithRecover(GMS_BOSS5_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        if (body_work.state == 9)
            gmBoss5BodyProceedToNextSeqStr(body_work);
        else
            gmBoss5BodyProceedToNextSeqNml(body_work);
    }

    private static void gmBoss5BodyStateEnterBerserk(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodySetActionWhole(body_work, 39);
        gmBoss5BodyChangeRectSetting(body_work, 9);
        GmBoss5EfctBreakdownSmokesInit(body_work, GMD_BOSS5_BODY_BERSERK_BREAKDOWN_TIME);
        GmBoss5EfctBodySmallSmokesInit(body_work);
        body_work.wait_timer = GMD_BOSS5_BODY_BERSERK_BREAKDOWN_TIME;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateBerserkWithBreakdown);
    }

    private static void gmBoss5BodyStateLeaveBerserk(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodyClearBerserkTurn(body_work);
        gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateBerserkWithBreakdown(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            gmBoss5BodySetActionWhole(body_work, 40);
            gmBoss5BodyInitShakeAccelerate(body_work);
            GmBoss5EfctStartPrelimLeakage(body_work);
            body_work.wait_timer = GMD_BOSS5_BODY_BERSERK_SHAKE_STAY_TIME;
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateBerserkWithShake);
        }
    }

    private static void gmBoss5BodyStateUpdateBerserkWithShake(GMS_BOSS5_BODY_WORK body_work)
    {
        if (gmBoss5BodyUpdateShakeAccelerate(body_work) == 0)
            return;
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            gmBoss5InitAlarmFade(body_work.mgr_work);
            gmBoss5MgrSetAlarmLevel(body_work.mgr_work, 0);
            GmDecoStartEffectFinalBossLight();
            gmBoss5BodySetActionWhole(body_work, 41);
            GmSoundPlaySE("FinalBoss08");
            gmBoss5BodyInitBerserkTurn(body_work, 0);
            GmBoss5EfctEndPrelimLeakage(body_work);
            gmBoss5BodyChangeRectSettingDefault(body_work);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateBerserkWithTurnFront);
        }
    }

    private static void gmBoss5BodyStateUpdateBerserkWithTurnFront(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (gmBoss5BodyUpdateBerserkTurn(body_work) == 0)
            return;
        GmBoss5EfctBerserkSteamInit(body_work, 1U);
        body_work.wait_timer = GMD_BOSS5_BODY_BERSERK_ROAR_PREP_TIME;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateBerserkWithRoarPrep);
    }

    private static void gmBoss5BodyStateUpdateBerserkWithRoarPrep(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            gmBoss5BodySetActionWhole(body_work, 42);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateBerserkWithRoarStart);
        }
    }

    private static void gmBoss5BodyStateUpdateBerserkWithRoarStart(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        GmBoss5EfctBerserkSteamInit(body_work, 1U);
        gmBoss5BodySetActionWhole(body_work, 43);
        body_work.wait_timer = GMD_BOSS5_BODY_BERSERK_ROAR_LOOP_TIME;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateBerserkWithRoarLoop);
    }

    private static void gmBoss5BodyStateUpdateBerserkWithRoarLoop(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            gmBoss5BodySetActionWhole(body_work, 44);
            GmBoss5EfctCreateBerserkStampSmoke(body_work, 0, GMD_BOSS5_BODY_BERSERK_STAMP_SMOKE_CREATE_DELAY);
            gmBoss5DelayedVibration(2, GMD_BOSS5_BODY_BERSERK_STAMP_VIB_START_DELAY);
            gmBoss5DelayedSePlayback("FinalBoss03", GMD_BOSS5_BODY_BERSERK_STAMP_SE_START_DELAY);
            gmBoss5BodyInitBerserkTurn(body_work, 1);
            gmBoss5BodyInitGroundingMove(body_work, body_work.rfoot_snm_reg_id);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateBerserkWithTurnSide);
        }
    }

    private static void gmBoss5BodyStateUpdateBerserkWithTurnSide(
      GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodyUpdateGroundingMove(body_work);
        if (gmBoss5BodyUpdateBerserkTurn(body_work) == 0)
            return;
        gmBoss5BodyClearBerserkTurn(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateBerserkWithStamp);
    }

    private static void gmBoss5BodyStateUpdateBerserkWithStamp(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        gmBoss5BodyUpdateGroundingMove(body_work);
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        gmBoss5BodySetActionWhole(body_work, 45);
        gmBoss5BodyInitGroundingMove(body_work, body_work.lfoot_snm_reg_id);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateBerserkWithKickUp);
    }

    private static void gmBoss5BodyStateUpdateBerserkWithKickUp(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        gmBoss5BodyUpdateGroundingMove(body_work);
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        gmBoss5BodyProceedToNextSeqStr(body_work);
    }

    private static void gmBoss5BodyStateEnterDefeat(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.mgr_work.flag |= 134217728U;
        GmPlayerAddScoreNoDisp((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), 1000);
        body_work.flag |= 512U;
        body_work.flag |= 8192U;
        gmBoss5BodyChangeRectSetting(body_work, 10);
        gmBoss5InitExplCreate(body_work.expl_work, 1, body_work.part_obj_core, GMD_BOSS5_EXPL_BODY_OFST_X, GMD_BOSS5_EXPL_BODY_OFST_Y, GMD_BOSS5_EXPL_BODY_WIDTH, GMD_BOSS5_EXPL_BODY_HEIGHT, GMD_BOSS5_EXPL_BODY_INTERVAL_MIN, GMD_BOSS5_EXPL_BODY_INTERVAL_MAX, GMD_BOSS5_EXPL_BODY_SE_FREQUENCY);
        body_work.wait_timer = GMD_BOSS5_BODY_DEFEAT_WAIT_START_TIME;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateDefeatWithWaitStart);
    }

    private static void gmBoss5BodyStateLeaveDefeat(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294959103U;
        body_work.flag &= 4294966783U;
    }

    private static void gmBoss5BodyStateUpdateDefeatWithWaitStart(
      GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5UpdateExplCreate(body_work.expl_work);
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            gmBoss5BodyRegisterScatterPartsCNM(body_work);
            gmBoss5InitScatter(body_work);
            body_work.wait_timer = MTM_MATH_MAX(80U, 90U);
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodyStateUpdateDefeatWithExplode);
        }
    }

    private static void gmBoss5BodyStateUpdateDefeatWithExplode(GMS_BOSS5_BODY_WORK body_work)
    {
        int num = 0;
        gmBoss5UpdateExplCreate(body_work.expl_work);
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else if (((int)body_work.flag & 524288) == 0)
        {
            gmBoss5BodyInitScatterFall(body_work);
            body_work.flag |= 524288U;
        }
        else
        {
            OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
            if (gmBoss5BodyUpdateScatterFall(body_work) != 0)
                num = 1;
            if (((int)obsObjectWork.move_flag & 1) != 0 && ((int)body_work.flag & 2097152) == 0)
            {
                GmBoss5EfctCreateBigExplosion(body_work.part_obj_core.pos.x, body_work.part_obj_core.pos.y, body_work.part_obj_core.pos.z + GMD_BOSS5_EXPL_OFST_Z);
                body_work.flag |= 2097152U;
                GmSoundPlaySE("Boss0_03");
                gmBoss5InitFlashScreen();
                GMM_PAD_VIB_MID_TIME(120f);
            }
        }
        if (gmBoss5BodyIsBodyExplosionStopAllowed(body_work) == 0 || num == 0)
            return;
        body_work.proc_update = null;
        body_work.mgr_work.flag |= 268435456U;
    }

    private static void gmBoss5BodySubSeqEnterMoveFastCrash(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.sub_seq == 0)
            gmBoss5BodySetActionWhole(body_work, 18);
        else
            gmBoss5BodySetActionWhole(body_work, 19);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodySubSeqUpdateMoveFastCrashWithStagger);
    }

    private static void gmBoss5BodySubSeqUpdateMoveFastCrashWithStagger(
      GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (((int)obj_work.move_flag & 1) == 0 || GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        gmBoss5BodyResumeMoveFast(body_work);
    }

    private static void gmBoss5BodySubSeqEnterRpcStrDmg(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodySetActionWhole(body_work, 36);
        gmBoss5BodyChangeRectSetting(body_work, 8);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodySubSeqUpdateRpcStrDmgWithBend);
    }

    private static void gmBoss5BodySubSeqUpdateRpcStrDmgWithBend(GMS_BOSS5_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        gmBoss5BodySetActionWhole(body_work, 37);
        body_work.wait_timer = 240U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodySubSeqUpdateRpcStrDmgWithSwing);
    }

    private static void gmBoss5BodySubSeqUpdateRpcStrDmgWithSwing(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            body_work.flag |= 1610612736U;
            body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodySubSeqUpdateRpcStrDmgWithWaitReturn);
        }
    }

    private static void gmBoss5BodySubSeqUpdateRpcStrDmgWithWaitReturn(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (gmBoss5BodyReceiveSignalRocketReturned(body_work) == 0)
            return;
        body_work.flag &= 4294967283U;
        gmBoss5BodySetActionWhole(body_work, 38);
        body_work.proc_update = new MPP_VOID_GMS_BOSS5_BODY_WORK(gmBoss5BodySubSeqUpdateRpcStrDmgWithRecover);
    }

    private static void gmBoss5BodySubSeqUpdateRpcStrDmgWithRecover(
      GMS_BOSS5_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        gmBoss5BodyChangeRectSettingDefault(body_work);
        gmBoss5BodyProceedToNextSeqStr(body_work);
    }

    private static void gmBoss5CoreWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS5_CORE_WORK core_work = (GMS_BOSS5_CORE_WORK)obj_work;
        if (((int)parentObj.mgr_work.flag & 1) == 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5CoreMain);
        gmBoss5CoreProcInit(core_work);
    }

    private static void gmBoss5CoreMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_CORE_WORK wrk = (GMS_BOSS5_CORE_WORK)obj_work;
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    private static void gmBoss5CoreProcInit(GMS_BOSS5_CORE_WORK core_work)
    {
        core_work.proc_update = new MPP_VOID_GMS_BOSS5_CORE_WORK(gmBoss5CoreProcUpdateLoop);
    }

    private static void gmBoss5CoreProcUpdateLoop(GMS_BOSS5_CORE_WORK core_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(core_work);
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObjectGeneralStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        gmBoss5BodyUpdateMainRectPosition(parentObj);
        gmBoss5BodyUpdateSubRectPosition(parentObj);
        if (((int)parentObj.flag & 8192) != 0)
            ((GMS_ENEMY_COM_WORK)obj_work).enemy_flag |= 32768U;
        else
            ((GMS_ENEMY_COM_WORK)obj_work).enemy_flag &= 4294934527U;
    }

    private static void gmBoss5InitAlarmFade(GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 32) != 0)
            return;
        mgr_work.flag |= 32U;
        GMS_BOSS5_ALARM_FADE_WORK fadeObj = (GMS_BOSS5_ALARM_FADE_WORK)GmFadeCreateFadeObj(GMD_TASK_PRIO_EFFECT, 3, 0, () => new GMS_BOSS5_ALARM_FADE_WORK(), IZD_FADE_DT_PRIO_DEF, 0U);
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)fadeObj;
        obsObjectWork.parent_obj = GMM_BS_OBJ(mgr_work);
        fadeObj.mgr_work = mgr_work;
        GmFadeSetFade(fadeObj.fade_obj, 0U, 0, 0, 0, 0, 0, 0, 0, 0, 1f, 0, 0);
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5AlarmFadeMain);
        gmBoss5AlarmFadeProcInit(fadeObj);
    }

    private static void gmBoss5RequestClearAlarmFade(GMS_BOSS5_MGR_WORK mgr_work)
    {
        mgr_work.flag &= 4294967263U;
    }

    private static void gmBoss5AlarmFadeInitFade(
      GMS_BOSS5_ALARM_FADE_WORK alarm_fade,
      int alarm_level)
    {
        alarm_fade.cur_phase = 0;
        alarm_fade.cur_level = alarm_level;
        alarm_fade.wait_timer = 0U;
        gmBoss5AlarmFadeUpdateFade(alarm_fade);
    }

    private static int gmBoss5AlarmFadeUpdateFade(GMS_BOSS5_ALARM_FADE_WORK alarm_fade)
    {
        if (alarm_fade.wait_timer != 0U)
        {
            --alarm_fade.wait_timer;
        }
        else
        {
            if (GmFadeIsEnd(alarm_fade.fade_obj) == 0)
                return 0;
            GMS_BOSS5_ALARM_FADE_INFO bosS5AlarmFadeInfo = gm_boss5_alarm_fade_info[alarm_fade.cur_level];
            switch (alarm_fade.cur_phase)
            {
                case 0:
                    GmFadeSetFade(alarm_fade.fade_obj, 0U, byte.MaxValue, 0, 0, 0, GMD_BOSS5_ALARM_FADE_DEST_RED, GMD_BOSS5_ALARM_FADE_DEST_GREEN, GMD_BOSS5_ALARM_FADE_DEST_BLUE, GMD_BOSS5_ALARM_FADE_DEST_ALPHA, bosS5AlarmFadeInfo.fo_frame, 0, 0);
                    alarm_fade.cur_phase = 1;
                    break;
                case 1:
                    alarm_fade.wait_timer = bosS5AlarmFadeInfo.on_frame;
                    alarm_fade.cur_phase = 2;
                    break;
                case 2:
                    GmFadeSetFade(alarm_fade.fade_obj, 0U, GMD_BOSS5_ALARM_FADE_DEST_RED, GMD_BOSS5_ALARM_FADE_DEST_GREEN, GMD_BOSS5_ALARM_FADE_DEST_BLUE, GMD_BOSS5_ALARM_FADE_DEST_ALPHA, byte.MaxValue, 0, 0, 0, bosS5AlarmFadeInfo.fi_frame, 0, 1);
                    alarm_fade.cur_phase = 3;
                    break;
                case 3:
                    alarm_fade.wait_timer = bosS5AlarmFadeInfo.off_frame;
                    alarm_fade.cur_phase = 4;
                    break;
                case 4:
                    return 1;
                default:
                    alarm_fade.cur_phase = 0;
                    break;
            }
        }
        return 0;
    }

    private static void gmBoss5AlarmFadeInitAlertSe(GMS_BOSS5_ALARM_FADE_WORK alarm_fade)
    {
        GMS_BOSS5_MGR_WORK mgrWork = alarm_fade.mgr_work;
        alarm_fade.alert_se_ref_level = mgrWork.alarm_level;
        GmBoss5Init1ShotTimer(alarm_fade.alert_se_timer, gm_boss5_alarm_se_interval_time_tbl[alarm_fade.alert_se_ref_level]);
        GmSoundPlaySE("FinalBoss10");
    }

    private static void gmBoss5AlarmFadeUpdateAlertSe(GMS_BOSS5_ALARM_FADE_WORK alarm_fade)
    {
        GMS_BOSS5_MGR_WORK mgrWork = alarm_fade.mgr_work;
        if (GmBoss5Update1ShotTimer(alarm_fade.alert_se_timer) == 0 && alarm_fade.alert_se_ref_level == mgrWork.alarm_level)
            return;
        GmSoundPlaySE("FinalBoss10");
        alarm_fade.alert_se_ref_level = mgrWork.alarm_level;
        GmBoss5Init1ShotTimer(alarm_fade.alert_se_timer, gm_boss5_alarm_se_interval_time_tbl[alarm_fade.alert_se_ref_level]);
    }

    private static void gmBoss5AlarmFadeMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_ALARM_FADE_WORK wrk = (GMS_BOSS5_ALARM_FADE_WORK)obj_work;
        if (((int)wrk.mgr_work.flag & 32) == 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            if (wrk.proc_update == null)
                return;
            wrk.proc_update(wrk);
        }
    }

    private static void gmBoss5AlarmFadeProcInit(GMS_BOSS5_ALARM_FADE_WORK alarm_fade)
    {
        GMS_BOSS5_MGR_WORK mgrWork = alarm_fade.mgr_work;
        gmBoss5AlarmFadeInitFade(alarm_fade, mgrWork.alarm_level);
        gmBoss5AlarmFadeInitAlertSe(alarm_fade);
        alarm_fade.proc_update = new MPP_VOID_GMS_BOSS5_ALARM_FADE_WORK(gmBoss5AlarmFadeProcUpdateLoop);
    }

    private static void gmBoss5AlarmFadeProcUpdateLoop(GMS_BOSS5_ALARM_FADE_WORK alarm_fade)
    {
        GMS_BOSS5_MGR_WORK mgrWork = alarm_fade.mgr_work;
        gmBoss5AlarmFadeUpdateAlertSe(alarm_fade);
        if (gmBoss5AlarmFadeUpdateFade(alarm_fade) == 0)
            return;
        gmBoss5AlarmFadeInitFade(alarm_fade, mgrWork.alarm_level);
    }

    private static void gmBoss5InitFlashScreen()
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_BOSS5_FLASH_SCREEN_WORK(), null, 0, "boss5_flash_scr");
        GMS_BOSS5_FLASH_SCREEN_WORK s5FlashScreenWork = (GMS_BOSS5_FLASH_SCREEN_WORK)work;
        work.disp_flag |= 4128U;
        work.flag |= 16U;
        GmBsCmnInitFlashScreen(s5FlashScreenWork.flash_work, GMD_BOSS5_FLASH_SCREEN_FADEOUT_TIME, 5f, 30f);
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5FlashScreenMain);
    }

    private static void gmBoss5FlashScreenMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_FLASH_SCREEN_WORK s5FlashScreenWork = (GMS_BOSS5_FLASH_SCREEN_WORK)obj_work;
        if (GmBsCmnUpdateFlashScreen(s5FlashScreenWork.flash_work) == 0)
            return;
        GmBsCmnClearFlashScreen(s5FlashScreenWork.flash_work);
        obj_work.flag |= 4U;
    }

    private static void gmBoss5InitLastFadeOut(GMS_BOSS5_MGR_WORK mgr_work)
    {
        GMS_FADE_OBJ_WORK fadeObj = GmFadeCreateFadeObj(GMD_TASK_PRIO_EFFECT, 3, 0, () => new GMS_FADE_OBJ_WORK(), IZD_FADE_DT_PRIO_DEF, 6U);
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)fadeObj;
        obsObjectWork.parent_obj = GMM_BS_OBJ(mgr_work);
        GmFadeSetFade(fadeObj, 0U, 0, 0, 0, 0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 300f, 0, 0);
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5LastFadeOutMain);
    }

    private static void gmBoss5LastFadeOutMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_FADE_OBJ_WORK fade_obj = (GMS_FADE_OBJ_WORK)obj_work;
        GMS_BOSS5_MGR_WORK parentObj = (GMS_BOSS5_MGR_WORK)obj_work.parent_obj;
        if (GmFadeIsEnd(fade_obj) == 0)
            return;
        gmBoss5RequestClearAlarmFade(parentObj);
        GmFixSetDisp(false);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5LastFadeOutEnd);
    }

    private static void gmBoss5LastFadeOutEnd(OBS_OBJECT_WORK obj_work)
    {
        GMS_FADE_OBJ_WORK gmsFadeObjWork = (GMS_FADE_OBJ_WORK)obj_work;
        ((GMS_BOSS5_MGR_WORK)obj_work.parent_obj).flag |= 2147483648U;
        gmsFadeObjWork.fade_work.draw_state = 0U;
        obj_work.ppFunc = null;
    }

    private static void gmBoss5InitScatter(GMS_BOSS5_BODY_WORK body_work)
    {
        int[] numArray = new int[6]
        {
      body_work.armpt_snm_reg_ids[0][2],
      body_work.armpt_snm_reg_ids[1][2],
      body_work.groin_snm_reg_ids[0],
      body_work.groin_snm_reg_ids[1],
      body_work.leg_snm_reg_ids[0],
      body_work.leg_snm_reg_ids[1]
        };
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        nnMakeUnitMatrix(nnsMatrix);
        for (int index = 0; index < 22; ++index)
        {
            GMS_BOSS5_SCT_PART_INFO bosS5SctPartInfo = gm_boss5_scatter_parts_info_tbl[index];
            GmBsCmnChangeCNMModeNode(body_work.cnm_mgr_work, body_work.scatter_cnm_reg_ids[index], (uint)bosS5SctPartInfo.cnm_mode);
            GmBsCmnEnableCNMLocalCoordinate(body_work.cnm_mgr_work, body_work.scatter_cnm_reg_ids[index], bosS5SctPartInfo.is_local_coord);
            GmBsCmnEnableCNMInheritNodeScale(body_work.cnm_mgr_work, body_work.scatter_cnm_reg_ids[index], bosS5SctPartInfo.is_inherit_scale);
            GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix, body_work.scatter_cnm_reg_ids[index], 1);
        }
        for (int index = 0; index < 6; ++index)
        {
            GMS_BOSS5_SCT_NDC_INFO gmsBosS5SctNdcInfo = gm_boss5_scatter_ndc_info_tbl[index];
            int partIdx = gmsBosS5SctNdcInfo.part_idx;
            GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, body_work.scatter_cnm_reg_ids[partIdx], 0);
            GMS_BS_CMN_NODE_CTRL_OBJECT controlObjectBySize = GmBsCmnCreateNodeControlObjectBySize(GMM_BS_OBJ(body_work), body_work.cnm_mgr_work, body_work.scatter_cnm_reg_ids[partIdx], body_work.snm_work, numArray[index], () => new GMS_BOSS5_SCT_PART_NDC_WORK());
            GMS_BOSS5_SCT_PART_NDC_WORK sct_part_ndc = (GMS_BOSS5_SCT_PART_NDC_WORK)controlObjectBySize;
            controlObjectBySize.user_timer = gmsBosS5SctNdcInfo.delay_time;
            controlObjectBySize.is_enable = 0;
            gmBoss5ScatterSetPartParam(sct_part_ndc);
            GMM_BS_OBJ(controlObjectBySize).move_flag |= 4608U;
            nnMakeUnitQuaternion(ref controlObjectBySize.user_quat);
            controlObjectBySize.proc_update = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5ScatterProcWait);
        }
        body_work.flag |= 262144U;
    }

    private static void gmBoss5ScatterSetPartParam(GMS_BOSS5_SCT_PART_NDC_WORK sct_part_ndc)
    {
        nnMakeUnitQuaternion(ref sct_part_ndc.spin_quat);
        for (int index = 0; index < 2; ++index)
        {
            NNS_VECTOR dst_vec = GlobalPool<NNS_VECTOR>.Alloc();
            float rand_z = MTM_MATH_CLIP((float)(FX_FX32_TO_F32(AkMathRandFx()) * 2.0 - 1.0), -1f, 1f);
            short rand_angle = AKM_DEGtoA16(360f * FX_FX32_TO_F32(AkMathRandFx()));
            AkMathGetRandomUnitVector(dst_vec, rand_z, rand_angle);
            NNS_QUATERNION dst;
            nnMakeRotateAxisQuaternion(out dst, dst_vec.x, dst_vec.y, dst_vec.z, GMD_BOSS5_SCT_SPIN_SPD_ANGLE);
            nnMultiplyQuaternion(ref sct_part_ndc.spin_quat, ref dst, ref sct_part_ndc.spin_quat);
            GlobalPool<NNS_VECTOR>.Release(dst_vec);
        }
    }

    private static void gmBoss5ScatterProcWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj = (GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work;
        if (ndc_obj.user_timer != 0U)
        {
            --ndc_obj.user_timer;
        }
        else
        {
            GmBsCmnAttachNCObjectToSNMNode(ndc_obj);
            GmBoss5ScatterSetFlyParam(obj_work);
            ndc_obj.is_enable = 1;
            ndc_obj.user_timer = 180U;
            GmBoss5EfctCreateSmallExplosion(obj_work.pos.x, obj_work.pos.y, obj_work.pos.z + GMD_BOSS5_EXPL_OFST_Z);
            ndc_obj.proc_update = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5ScatterProcFly);
        }
    }

    private static void gmBoss5ScatterProcFly(OBS_OBJECT_WORK obj_work)
    {
        GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj = (GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work;
        GMS_BOSS5_SCT_PART_NDC_WORK s5SctPartNdcWork = (GMS_BOSS5_SCT_PART_NDC_WORK)ndc_obj;
        nnMultiplyQuaternion(ref ndc_obj.user_quat, ref s5SctPartNdcWork.spin_quat, ref ndc_obj.user_quat);
        GmBsCmnSetWorldMtxFromNCObjectPosture(ndc_obj);
        if (ndc_obj.user_timer != 0U)
            --ndc_obj.user_timer;
        else
            obj_work.flag |= 4U;
    }

    private static void gmBoss5BodySeqTryRequestEnableStr(GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.mgr_work.life > GMD_BOSS5_STRONG_MODE_THRESHOLD_LIFE)
            return;
        body_work.mgr_work.flag |= 4U;
    }

    private static void gmBoss5BodySeqTryEnableStr(GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.mgr_work.flag & 4) == 0)
            return;
        body_work.mgr_work.flag |= 8U;
    }

    private static int gmBoss5BodySeqIsStr(GMS_BOSS5_BODY_WORK body_work)
    {
        return ((int)body_work.mgr_work.flag & 8) != 0 ? 1 : 0;
    }

    private static int gmBoss5BodySeqIsNearDeath(GMS_BOSS5_BODY_WORK body_work)
    {
        return body_work.mgr_work.life <= 1 ? 1 : 0;
    }

    private static int gmBoss5BodySeqLotStrat(int strat_branch)
    {
        return gmBoss5BodySeqLotStrat(strat_branch, 0);
    }

    private static int gmBoss5BodySeqLotStrat(int strat_branch, int b_no_rkt)
    {
        GMS_BOSS5_STRAT_PROB_INFO[] bosS5StratProbInfoArray = gm_boss5_body_seq_strat_branch_prob_tbl[strat_branch];
        int index1 = 0;
        int num1 = 0;
        int v1 = AkMathRandFx();
        if (b_no_rkt != 0)
        {
            int num2 = 4096;
            for (int index2 = 0; index2 < 3; ++index2)
            {
                if (bosS5StratProbInfoArray[index2].is_rkt != 0)
                    num2 = MTM_MATH_CLIP(num2 - bosS5StratProbInfoArray[index2].probability, 0, 4096);
            }
            v1 = MTM_MATH_CLIP(FX_Mul(v1, num2), 0, num2);
        }
        for (int index2 = 0; index2 < 3; ++index2)
        {
            int probability = bosS5StratProbInfoArray[index2].probability;
            if ((b_no_rkt == 0 || bosS5StratProbInfoArray[index2].is_rkt == 0) && probability > 0)
            {
                index1 = index2;
                num1 += probability;
                if (v1 <= num1)
                    return bosS5StratProbInfoArray[index2].strat_state;
            }
        }
        return bosS5StratProbInfoArray[index1].strat_state;
    }

    private static void gmBoss5BodyProceedToNextSeqNml(GMS_BOSS5_BODY_WORK body_work)
    {
        int state1 = body_work.state;
        int strat_state = 0;
        gmBoss5BodySeqTryEnableStr(body_work);
        if (gmBoss5BodySeqIsStr(body_work) != 0)
        {
            strat_state = 8;
        }
        else
        {
            int b_no_rkt = 0;
            if (gmBoss5BodyIsPlayerBehind(body_work) != 0)
                b_no_rkt = 1;
            switch (body_work.strat_state)
            {
                case 1:
                    strat_state = 2;
                    break;
                case 2:
                case 3:
                case 4:
                    strat_state = 5;
                    break;
                case 5:
                    strat_state = gmBoss5BodySeqLotStrat(0, b_no_rkt);
                    break;
                case 6:
                    strat_state = 5;
                    break;
                case 7:
                    strat_state = gmBoss5BodySeqLotStrat(1, b_no_rkt);
                    break;
            }
        }
        int state2 = gm_boss5_body_state_strat_tbl[strat_state];
        gmBoss5BodyChangeState(body_work, state2, strat_state, 1);
    }

    private static void gmBoss5BodyProceedToNextSeqStr(GMS_BOSS5_BODY_WORK body_work)
    {
        int state1 = body_work.state;
        int strat_state = 0;
        if (gmBoss5BodySeqIsNearDeath(body_work) != 0)
        {
            strat_state = 16;
        }
        else
        {
            int b_no_rkt = 0;
            if (gmBoss5BodyIsPlayerBehind(body_work) != 0)
                b_no_rkt = 1;
            switch (body_work.strat_state)
            {
                case 8:
                    strat_state = 9;
                    break;
                case 9:
                    strat_state = 12;
                    break;
                case 10:
                case 13:
                    strat_state = b_no_rkt == 0 ? 15 : 12;
                    break;
                case 11:
                    strat_state = 12;
                    break;
                case 12:
                    strat_state = gmBoss5BodySeqLotStrat(2, b_no_rkt);
                    break;
                case 14:
                    strat_state = gmBoss5BodySeqLotStrat(3, b_no_rkt);
                    break;
                case 15:
                    strat_state = 12;
                    break;
            }
        }
        int state2 = gm_boss5_body_state_strat_tbl[strat_state];
        if (state2 == 3)
            gmBoss5BodySetMoveFastTime(body_work, 360U);
        else
            gmBoss5BodySetMoveFastTime(body_work, 0U);
        gmBoss5BodyChangeState(body_work, state2, strat_state, 1);
    }

    private static void gmBoss5BodyProceedToDefeatState(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5BodyChangeState(body_work, 11, 0, 1);
    }

    private static uint gmBoss5BodySeqGetRpcNmlSearchTime(GMS_BOSS5_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
        return AkMathRandFx() < GMD_BOSS5_BODY_SEQ_RPUNCH_NML_FASTSHOT_PROB ? GMD_BOSS5_BODY_SEQ_RPUNCH_NML_SEARCH_TIME_SHORT : GMD_BOSS5_BODY_SEQ_RPUNCH_NML_SEARCH_TIME_NORMAL;
    }

    private static uint gmBoss5BodySeqGetRpcStrSearchTime(GMS_BOSS5_BODY_WORK body_work)
    {
        UNREFERENCED_PARAMETER(body_work);
        return AkMathRandFx() < GMD_BOSS5_BODY_SEQ_RPUNCH_STR_FASTSHOT_PROB ? GMD_BOSS5_BODY_SEQ_RPUNCH_STR_SEARCH_TIME_SHORT : GMD_BOSS5_BODY_SEQ_RPUNCH_STR_SEARCH_TIME_NORMAL;
    }

}