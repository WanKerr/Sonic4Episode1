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
    private static int GMM_BOSS5_AREA_LEFT()
    {
        return AppMain.g_gm_main_system.map_fcol.left << 12;
    }

    private static int GMM_BOSS5_AREA_TOP()
    {
        return AppMain.g_gm_main_system.map_fcol.top << 12;
    }

    private static int GMM_BOSS5_AREA_RIGHT()
    {
        return AppMain.g_gm_main_system.map_fcol.right << 12;
    }

    private static int GMM_BOSS5_AREA_BOTTOM()
    {
        return AppMain.g_gm_main_system.map_fcol.bottom << 12;
    }

    private static int GMM_BOSS5_AREA_CENTER_X()
    {
        return AppMain.GMM_BOSS5_AREA_LEFT() + (AppMain.GMM_BOSS5_AREA_RIGHT() - AppMain.GMM_BOSS5_AREA_LEFT()) / 2;
    }

    private static int GMM_BOSS5_AREA_CENTER_Y()
    {
        return AppMain.GMM_BOSS5_AREA_TOP() + (AppMain.GMM_BOSS5_AREA_BOTTOM() - AppMain.GMM_BOSS5_AREA_TOP()) / 2;
    }

    private static void GmBoss5Build()
    {
        AppMain.gm_boss5_obj_3d_list = AppMain.GmGameDBuildRegBuildModel((AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, AppMain.g_gm_gamedat_enemy_arc), (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 1, AppMain.g_gm_gamedat_enemy_arc), 0U);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(745), 2, AppMain.g_gm_gamedat_enemy_arc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(746), 4, AppMain.g_gm_gamedat_enemy_arc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(747), 8, AppMain.g_gm_gamedat_enemy_arc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(748), 3, AppMain.g_gm_gamedat_enemy_arc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(749), 5, AppMain.g_gm_gamedat_enemy_arc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(750), 6, AppMain.g_gm_gamedat_enemy_arc);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(751), 7, AppMain.g_gm_gamedat_enemy_arc);
        AppMain.GmBoss5EfctBuild();
    }

    private static void GmBoss5Flush()
    {
        AppMain.GmBoss5EfctFlush();
        AppMain.ObjDataRelease(AppMain.ObjDataGet(751));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(750));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(749));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(748));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(747));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(746));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(745));
        AppMain.AMS_AMB_HEADER amsAmbHeader = (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, AppMain.g_gm_gamedat_enemy_arc);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_boss5_obj_3d_list, amsAmbHeader.file_num);
        AppMain.gm_boss5_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss5Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_MGR_WORK()), "BOSS5_MGR");
        AppMain.GMS_BOSS5_MGR_WORK gmsBosS5MgrWork = (AppMain.GMS_BOSS5_MGR_WORK)work;
        work.flag |= 16U;
        work.disp_flag |= 32U;
        work.move_flag |= 8448U;
        gmsBosS5MgrWork.ene_3d.ene_com.enemy_flag |= 32768U;
        gmsBosS5MgrWork.life = AppMain.GMD_BOSS5_LIFE;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5MgrWaitLoad);
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss5BodyInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_BODY_WORK()), "BOSS5_BODY");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS5_BODY_WORK body_work = (AppMain.GMS_BOSS5_BODY_WORK)work;
        work.pos.z = AppMain.GMD_BOSS5_DEFAULT_POS_Z;
        body_work.ground_v_pos = pos_y;
        gmsEnemy3DWork.ene_com.vit = (byte)1;
        AppMain.ObjObjectFieldRectSet(work, AppMain.GMD_BOSS5_BODY_DEFAULT_FIELD_RECT_SIZE_LEFT, AppMain.GMD_BOSS5_BODY_DEFAULT_FIELD_RECT_SIZE_TOP, AppMain.GMD_BOSS5_BODY_DEFAULT_FIELD_RECT_SIZE_RIGHT, AppMain.GMD_BOSS5_BODY_DEFAULT_FIELD_RECT_SIZE_BOTTOM);
        AppMain.gmBoss5BodySetupRect(body_work);
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_boss5_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(745), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = AppMain.GMD_BOSS5_DEFAULT_BLEND_SPD;
        work.disp_flag |= 134217728U;
        work.flag |= 16U;
        work.disp_flag |= 4194308U;
        work.move_flag &= 4294967167U;
        work.move_flag &= 4294443007U;
        work.move_flag |= 256U;
        work.move_flag |= 1024U;
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5BodyWaitSetup);
        AppMain.gmBoss5BodyChangeState(body_work, 0, 0);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5BodyOutFunc);
        work.ppRec = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5BodyRecFunc);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss5BodyExit));
        AppMain.gmBoss5BodyAllocSeHandles(body_work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss5CoreInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_CORE_WORK()), "BOSS5_CORE");
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)work;
        AppMain.GMS_BOSS5_CORE_WORK gmsBosS5CoreWork = (AppMain.GMS_BOSS5_CORE_WORK)work;
        work.move_flag |= 256U;
        work.flag |= 18U;
        work.disp_flag &= 4294967263U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5CoreWaitSetup);
        return work;
    }

    private static AppMain.OBS_ACTION3D_NN_WORK[] GmBoss5GetObject3dList()
    {
        return AppMain.gm_boss5_obj_3d_list;
    }

    private static void GmBoss5BodyGetPlySearchPos(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      out AppMain.VecFx32 pos)
    {
        AppMain.GmBsCmnGetDelaySearchPos(body_work.dsearch_work, body_work.ply_search_delay, out pos);
    }

    private static void GmBoss5ScatterSetFlyParam(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int num = (int)AppMain.mtMathRand() % 90;
        int ang = obj_work.pos.x > obj_work.parent_obj.pos.x ? AppMain.AKM_DEGtoA32(num - 45) : AppMain.AKM_DEGtoA32(num + 90 + 45);
        float sctNdcFlySpdFloat = AppMain.GMD_BOSS5_SCT_NDC_FLY_SPD_FLOAT;
        obj_work.spd.y = (int)(4096.0 * (double)sctNdcFlySpdFloat * (double)AppMain.nnSin(ang));
        obj_work.spd.x = (int)(4096.0 * (double)sctNdcFlySpdFloat * (double)AppMain.nnCos(ang));
        obj_work.move_flag |= 128U;
    }

    private static void GmBoss5Init1ShotTimer(
      AppMain.GMS_BOSS5_1SHOT_TIMER one_shot_timer,
      uint frame)
    {
        one_shot_timer.timer = frame;
        one_shot_timer.is_active = 1;
    }

    private static int GmBoss5Update1ShotTimer(AppMain.GMS_BOSS5_1SHOT_TIMER one_shot_timer)
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
        pos_x = AppMain.FX_Mul(scale, AppMain.gm_boss5_vib_tbl[phase_cnt][0]);
        pos_y = AppMain.FX_Mul(scale, AppMain.gm_boss5_vib_tbl[phase_cnt][1]);
        int num2 = num1 + 1;
        if (num2 >= 40)
            num2 = 0;
        return num2;
    }

    private static void gmBoss5InitExplCreate(
      AppMain.GMS_BOSS5_EXPL_WORK expl_work,
      int expl_type,
      AppMain.OBS_OBJECT_WORK parent_obj,
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

    public static void gmBoss5UpdateExplCreate(AppMain.GMS_BOSS5_EXPL_WORK expl_work)
    {
        if (expl_work.interval_timer != 0U)
        {
            --expl_work.interval_timer;
        }
        else
        {
            int v2_1 = expl_work.area[0];
            int v2_2 = expl_work.area[1];
            AppMain.VecFx32 vecFx32 = new AppMain.VecFx32(expl_work.parent_obj.pos);
            int num1 = 0;
            int num2 = AppMain.FX_Mul(AppMain.AkMathRandFx(), v2_1);
            int num3 = AppMain.FX_Mul(AppMain.AkMathRandFx(), v2_2);
            if ((double)expl_work.se_freq_cnt < 1.0)
                expl_work.se_freq_cnt += expl_work.se_frequency;
            if ((double)expl_work.se_freq_cnt >= 1.0)
            {
                --expl_work.se_freq_cnt;
                num1 = 1;
            }
            switch (expl_work.expl_type)
            {
                case 0:
                    AppMain.GmBoss5EfctCreateSmallExplosion(vecFx32.x + expl_work.ofst_pos[0] - (v2_1 >> 1) + num2, vecFx32.y + expl_work.ofst_pos[1] - (v2_2 >> 1) + num3, vecFx32.z + AppMain.GMD_BOSS5_EXPL_OFST_Z);
                    if (num1 != 0)
                    {
                        AppMain.GmSoundPlaySE("Boss0_02");
                        break;
                    }
                    break;
                case 1:
                    AppMain.GmBoss5EfctCreateSmallExplosion(vecFx32.x + expl_work.ofst_pos[0] - (v2_1 >> 1) + num2, vecFx32.y + expl_work.ofst_pos[1] - (v2_2 >> 1) + num3, vecFx32.z + AppMain.GMD_BOSS5_EXPL_OFST_Z);
                    AppMain.GmBoss5EfctCreateFragments(vecFx32.x + expl_work.ofst_pos[0] - (v2_1 >> 1) + num2, vecFx32.y + expl_work.ofst_pos[1] - (v2_2 >> 1) + num3, vecFx32.z + AppMain.GMD_BOSS5_EXPL_OFST_Z);
                    if (num1 != 0)
                    {
                        AppMain.GmSoundPlaySE("Boss0_02");
                        break;
                    }
                    break;
                case 2:
                    AppMain.GmBoss5EfctCreateBigExplosion(vecFx32.x + expl_work.ofst_pos[0] - (v2_1 >> 1) + num2, vecFx32.y + expl_work.ofst_pos[1] - (v2_2 >> 1) + num3, vecFx32.z + AppMain.GMD_BOSS5_EXPL_OFST_Z);
                    if (num1 != 0)
                    {
                        AppMain.GmSoundPlaySE("Boss0_03");
                        break;
                    }
                    break;
                default:
                    AppMain.mppAssertNotImpl();
                    return;
            }
            uint num4 = (uint)((long)AppMain.AkMathRandFx() * (long)(expl_work.interval_max - expl_work.interval_min) >> 12);
            expl_work.interval_timer = expl_work.interval_min + num4;
        }
    }

    private static void gmBoss5SetCameraLift(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        AppMain.GMS_PLAYER_WORK playerObj = (AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj();
        if (((int)mgr_work.flag & 16) == 0)
        {
            mgr_work.flag |= 16U;
            mgr_work.save_camera_offset[0] = playerObj.gmk_camera_center_ofst_x;
            mgr_work.save_camera_offset[1] = playerObj.gmk_camera_center_ofst_y;
        }
        AppMain.GmCameraScaleSet(0.85f, 0.0025f);
        AppMain.GmPlayerCameraOffsetSet((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), (short)0, AppMain.GMD_BOSS5_CAMERA_LIFT_OFFSET_POS_Y);
    }

    private static void gmBoss5RestoreCameraLift(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 16) == 0)
            return;
        AppMain.GmPlayerCameraOffsetSet((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), mgr_work.save_camera_offset[0], mgr_work.save_camera_offset[1]);
        AppMain.GmCameraScaleSet(1f, 0.0025f);
        mgr_work.save_camera_offset[0] = mgr_work.save_camera_offset[1] = (short)0;
        mgr_work.flag &= 4294967279U;
    }

    private static void gmBoss5SetCameraSlideForNarrowScreen(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        AppMain.GMS_PLAYER_WORK playerObj = (AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj();
        if (((int)mgr_work.flag & 16) == 0)
        {
            mgr_work.flag |= 16U;
            mgr_work.save_camera_offset[0] = playerObj.gmk_camera_center_ofst_x;
            mgr_work.save_camera_offset[1] = playerObj.gmk_camera_center_ofst_y;
        }
        AppMain.GmPlayerCameraOffsetSet((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), AppMain.GMD_BOSS5_CAMERA_SLIDE_FOR_NARROW_OFFSET_POS_X, (short)0);
    }

    private static void gmBoss5RestoreCameraSlideForNarrowScreen(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 16) == 0)
            return;
        AppMain.GmPlayerCameraOffsetSet((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), mgr_work.save_camera_offset[0], mgr_work.save_camera_offset[1]);
        mgr_work.save_camera_offset[0] = mgr_work.save_camera_offset[1] = (short)0;
        mgr_work.flag &= 4294967279U;
    }

    private static void gmBoss5CamScrLimitReleaseGently()
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_COM_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "scr_lim_rel_gently");
        work.disp_flag |= 4128U;
        work.flag |= 16U;
        work.user_timer = AppMain.GMD_BOSS5_CAM_SCR_LIMIT_RELEASE_GNTL_SPD_X_INIT;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5CamScrLimitReleaseGentlyProcMain);
    }

    private static void gmBoss5CamScrLimitReleaseGentlyProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int num1 = (int)AppMain.g_gm_main_system.map_fcol.map_block_num_x * 64 << 12;
        int num2 = AppMain.g_gm_main_system.map_fcol.right << 12;
        int num3 = 0;
        obj_work.user_timer += AppMain.GMD_BOSS5_CAM_SCR_LIMIT_RELEASE_GNTL_ACC_X;
        obj_work.user_timer = AppMain.MTM_MATH_CLIP(obj_work.user_timer, 0, int.MaxValue);
        int pos_x = num2 + obj_work.user_timer;
        if (pos_x >= num1)
        {
            pos_x = num1;
            num3 = 1;
        }
        AppMain.GmCamScrLimitSetDirect(new AppMain.GMS_EVE_RECORD_EVENT()
        {
            flag = (ushort)4,
            left = (sbyte)0,
            top = (sbyte)0,
            width = (byte)0,
            height = (byte)0
        }, pos_x, 0);
        if (num3 == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmBoss5HideMapBSide()
    {
        AppMain.GmMapSetDispB(false);
    }

    private static void gmBoss5TransferPlayerToASide()
    {
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = (AppMain.GMS_PLAYER_WORK)playerObj;
        playerObj.flag &= 4294967294U;
        gmsPlayerWork.graind_prev_ride = (byte)0;
    }

    private static void gmBoss5Vibration(int vib_idx)
    {
        AppMain.GmCameraVibrationSet(AppMain.gm_boss5_vib_param_tbl[vib_idx][0], AppMain.gm_boss5_vib_param_tbl[vib_idx][1], AppMain.gm_boss5_vib_param_tbl[vib_idx][2]);
    }

    private static void gmBoss5DelayedVibration(int vib_idx, uint delay)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_COM_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "boss5_delay_vib");
        work.disp_flag |= 4128U;
        work.flag |= 16U;
        work.user_work = (uint)vib_idx;
        work.user_timer = (int)delay;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5DelayedVibrationProcMain);
    }

    private static void gmBoss5DelayedVibrationProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.gmBoss5Vibration((int)obj_work.user_work);
            obj_work.flag |= 4U;
        }
    }

    private static void gmBoss5DelayedSePlayback(string cue_name, uint delay)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_COM_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "boss5_delay_se");
        work.disp_flag |= 4128U;
        work.flag |= 16U;
        AppMain.amCriAudioGetGlobal();
        work.user_work = AppMain.CriAuPlayer.GetCueId(cue_name);
        if (work.user_work == uint.MaxValue)
            return;
        work.user_timer = (int)delay;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5DelayedSePlaybackProcMain);
    }

    private static void gmBoss5DelayedSePlaybackProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.GsSoundPlaySeById(obj_work.user_work);
            obj_work.flag |= 4U;
        }
    }

    private static void gmBoss5MgrSetAlarmLevel(AppMain.GMS_BOSS5_MGR_WORK mgr_work, int alarm_level)
    {
        mgr_work.alarm_level = alarm_level;
    }

    private static void gmBoss5MgrSetDemoRunDestPos(
      AppMain.GMS_BOSS5_MGR_WORK mgr_work,
      int dest_pos_x)
    {
        mgr_work.ply_demo_run_dest_x = dest_pos_x;
    }

    private static void gmBoss5InitChasingExpl(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        AppMain.gmBoss5InitExplCreate(mgr_work.small_expl_work, 0, AppMain.GmBsCmnGetPlayerObj(), AppMain.GMD_BOSS5_EXPL_CHASE_SMALL_OFST_X, AppMain.GMD_BOSS5_EXPL_CHASE_SMALL_OFST_Y, AppMain.GMD_BOSS5_EXPL_CHASE_SMALL_WIDTH, AppMain.GMD_BOSS5_EXPL_CHASE_SMALL_HEIGHT, AppMain.GMD_BOSS5_EXPL_CHASE_SMALL_INTERVAL_MIN, AppMain.GMD_BOSS5_EXPL_CHASE_SMALL_INTERVAL_MAX, AppMain.GMD_BOSS5_EXPL_CHASE_SMALL_SE_FREQUENCY);
        AppMain.gmBoss5InitExplCreate(mgr_work.big_expl_work, 2, AppMain.GmBsCmnGetPlayerObj(), AppMain.GMD_BOSS5_EXPL_CHASE_BIG_OFST_X + AppMain.GMD_BOSS5_EXPL_CHASE_BIG_DELAY_OFST_X, AppMain.GMD_BOSS5_EXPL_CHASE_BIG_OFST_Y, AppMain.GMD_BOSS5_EXPL_CHASE_BIG_WIDTH, AppMain.GMD_BOSS5_EXPL_CHASE_BIG_HEIGHT, AppMain.GMD_BOSS5_EXPL_CHASE_BIG_INTERVAL_MIN, AppMain.GMD_BOSS5_EXPL_CHASE_BIG_INTERVAL_MAX, AppMain.GMD_BOSS5_EXPL_CHASE_BIG_SE_FREQUENCY);
    }

    private static void gmBoss5UpdateChasingExpl(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        AppMain.GMS_BOSS5_EXPL_WORK smallExplWork = mgr_work.small_expl_work;
        AppMain.GMS_BOSS5_EXPL_WORK bigExplWork = mgr_work.big_expl_work;
        smallExplWork.ofst_pos[0] += AppMain.GMD_BOSS5_EXPL_CHASE_SMALL_CHASE_SPD_X;
        if (smallExplWork.ofst_pos[0] >= AppMain.GMD_BOSS5_EXPL_CHASE_SMALL_CHASE_OFST_X_MAX)
            smallExplWork.ofst_pos[0] = AppMain.GMD_BOSS5_EXPL_CHASE_SMALL_CHASE_OFST_X_MAX;
        bigExplWork.ofst_pos[0] += AppMain.GMD_BOSS5_EXPL_CHASE_BIG_CHASE_SPD_X;
        if (bigExplWork.ofst_pos[0] >= AppMain.GMD_BOSS5_EXPL_CHASE_BIG_CHASE_OFST_X_MAX + AppMain.GMD_BOSS5_EXPL_CHASE_BIG_DELAY_OFST_X)
            bigExplWork.ofst_pos[0] = AppMain.GMD_BOSS5_EXPL_CHASE_BIG_CHASE_OFST_X_MAX + AppMain.GMD_BOSS5_EXPL_CHASE_BIG_DELAY_OFST_X;
        AppMain.gmBoss5UpdateExplCreate(mgr_work.small_expl_work);
        AppMain.gmBoss5UpdateExplCreate(mgr_work.big_expl_work);
    }

    private static void gmBoss5MgrWaitLoad(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int num = 0;
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)16 || AppMain.GmMainDatLoadBossBattleLoadCheck(4))
            num = 1;
        if (num == 0)
            return;
        AppMain.GMS_BOSS5_MGR_WORK gmsBosS5MgrWork = (AppMain.GMS_BOSS5_MGR_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK obsObjectWork1 = AppMain.GmEventMgrLocalEventBirth((ushort)330, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        AppMain.OBS_OBJECT_WORK obsObjectWork2 = AppMain.GmEventMgrLocalEventBirth((ushort)331, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        AppMain.GMS_BOSS5_BODY_WORK gmsBosS5BodyWork = (AppMain.GMS_BOSS5_BODY_WORK)obsObjectWork1;
        gmsBosS5MgrWork.body_work = gmsBosS5BodyWork;
        gmsBosS5BodyWork.mgr_work = gmsBosS5MgrWork;
        obsObjectWork1.parent_obj = obj_work;
        obsObjectWork2.parent_obj = obsObjectWork1;
        gmsBosS5BodyWork.parts_objs[0] = obsObjectWork1;
        gmsBosS5BodyWork.part_obj_core = obsObjectWork2;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5MgrWaitSetup);
    }

    private static void gmBoss5MgrWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_MGR_WORK mgr_work = (AppMain.GMS_BOSS5_MGR_WORK)obj_work;
        AppMain.GMS_BOSS5_BODY_WORK bodyWork = mgr_work.body_work;
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
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5MgrMain);
        AppMain.gmBoss5MgrProcInit(mgr_work);
    }

    private static void gmBoss5MgrMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_MGR_WORK wrk = (AppMain.GMS_BOSS5_MGR_WORK)obj_work;
        if (((int)wrk.flag & 2) != 0 && wrk.body_work != null)
        {
            AppMain.GMM_BS_OBJ((object)wrk.body_work).flag |= 8U;
            wrk.body_work = (AppMain.GMS_BOSS5_BODY_WORK)null;
        }
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    private static void gmBoss5MgrProcInit(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        mgr_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.gmBoss5MgrProcUpdateWaitOpeningDemoBegin);
    }

    private static void gmBoss5MgrProcUpdateWaitOpeningDemoBegin(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 2097152) == 0 || ((int)mgr_work.flag & 4194304) == 0)
            return;
        AppMain.GMS_PLAYER_WORK playerObj = (AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj();
        AppMain.g_gm_main_system.game_flag &= 4294966271U;
        AppMain.GmSoundChangeFinalBossBGM();
        AppMain.GmPlySeqChangeBoss5Demo(playerObj, mgr_work.ply_demo_run_dest_x, false);
        mgr_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.gmBoss5MgrProcUpdateOpeningDemo);
        AppMain.GmMapSetMapDrawSize(8);
    }

    private static void gmBoss5MgrProcUpdateOpeningDemo(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 33554432) == 0)
            return;
        AppMain.GMS_PLAYER_WORK playerObj = (AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj();
        AppMain.g_gm_main_system.game_flag |= 1024U;
        AppMain.GmPlySeqChangeBoss5DemoEnd(playerObj);
        mgr_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.gmBoss5MgrProcUpdateIdle);
    }

    private static void gmBoss5MgrProcUpdateIdle(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 67108864) == 0)
            return;
        AppMain.GmBoss5LandCreate(mgr_work);
        AppMain.gmBoss5TransferPlayerToASide();
        AppMain.gmBoss5HideMapBSide();
        mgr_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.gmBoss5MgrProcUpdateWaitDefeat);
    }

    private static void gmBoss5MgrProcUpdateWaitDefeat(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 134217728) == 0)
            return;
        AppMain.g_gm_main_system.game_flag &= 4294966271U;
        AppMain.g_gm_main_system.game_flag |= 1048576U;
        mgr_work.wait_timer = AppMain.GMD_BOSS5_MGR_WAIT_EXPLODE_TIME;
        AppMain.GmPadVibSet(1, -1f, (ushort)16384, (ushort)16384, -1f, 0.0f, 0.0f, 32768U);
        mgr_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.gmBoss5MgrProcUpdateWaitExplode);
    }

    private static void gmBoss5MgrProcUpdateWaitExplode(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            mgr_work.wait_timer = AppMain.GMD_BOSS5_MGR_CLOSING_DEMO_WAIT_BEGIN_TIME_MAX;
            AppMain.gmBoss5CamScrLimitReleaseGently();
            mgr_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.gmBoss5MgrProcUpdateWaitClosingDemoBegin);
        }
    }

    private static void gmBoss5MgrProcUpdateWaitClosingDemoBegin(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (mgr_work.wait_timer != 0U)
            --mgr_work.wait_timer;
        if (((int)AppMain.GmBsCmnGetPlayerObj().move_flag & 1) == 0 && mgr_work.wait_timer != 0U)
            return;
        AppMain.GmPlySeqChangeBoss5Demo((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), AppMain.g_gm_main_system.map_size[0] << 12, true);
        mgr_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.gmBoss5MgrProcUpdateClosingDemoLeaveBody);
    }

    private static void gmBoss5MgrProcUpdateClosingDemoLeaveBody(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 268435456) == 0)
            return;
        AppMain.gmBoss5InitChasingExpl(mgr_work);
        mgr_work.wait_timer = AppMain.GMD_BOSS5_MGR_CLOSING_DEMO_DURATION_TIME;
        mgr_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.gmBoss5MgrProcUpdateClosingDemoEscape);
    }

    private static void gmBoss5MgrProcUpdateClosingDemoEscape(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        AppMain.gmBoss5UpdateChasingExpl(mgr_work);
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5InitLastFadeOut(mgr_work);
            mgr_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.gmBoss5MgrProcUpdateClosingDemoWaitFadeEnd);
        }
    }

    private static void gmBoss5MgrProcUpdateClosingDemoWaitFadeEnd(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        AppMain.gmBoss5UpdateChasingExpl(mgr_work);
        if (((int)mgr_work.flag & int.MinValue) == 0)
            return;
        AppMain.GMM_PAD_VIB_STOP();
        mgr_work.wait_timer = AppMain.GMD_BOSS5_MGR_CLOSING_DEMO_WHITEOUT_TIME;
        mgr_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK(AppMain.gmBoss5MgrProcUpdateClosingDemoWaitFinish);
    }

    private static void gmBoss5MgrProcUpdateClosingDemoWaitFinish(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (mgr_work.wait_timer != 0U)
        {
            --mgr_work.wait_timer;
        }
        else
        {
            AppMain.g_gm_main_system.game_flag |= 4U;
            mgr_work.proc_update = (AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK)null;
        }
    }

    private static void gmBoss5BodyExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_BOSS5_BODY_WORK tcbWork = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.mtTaskGetTcbWork(tcb);
        AppMain.gmBoss5BodyReleaseCallbacks(tcbWork);
        AppMain.gmBoss5BodyFreeSeHandles(tcbWork);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss5BodySetActionWhole(AppMain.GMS_BOSS5_BODY_WORK body_work, int act_id)
    {
        AppMain.gmBoss5BodySetActionWhole(body_work, act_id, 0);
    }

    private static void gmBoss5BodySetActionWhole(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int act_id,
      int force_change)
    {
        AppMain.GMS_BOSS5_PART_ACT_INFO[] bosS5PartActInfoArray = AppMain.gm_boss5_act_id_tbl[act_id];
        if (force_change == 0 && body_work.whole_act_id == act_id)
            return;
        body_work.whole_act_id = act_id;
        for (int index = 0; index < 1; ++index)
        {
            if (body_work.parts_objs[index] != null)
            {
                if (bosS5PartActInfoArray[index].is_maintain == (byte)0)
                    AppMain.GmBsCmnSetAction(body_work.parts_objs[index], (int)bosS5PartActInfoArray[index].act_id, (int)bosS5PartActInfoArray[index].is_repeat, bosS5PartActInfoArray[index].is_blend);
                else if (bosS5PartActInfoArray[index].is_repeat != (byte)0)
                    AppMain.GMM_BS_OBJ((object)body_work).disp_flag |= 4U;
                body_work.parts_objs[index].obj_3d.speed[0] = bosS5PartActInfoArray[index].mtn_spd;
                body_work.parts_objs[index].obj_3d.blend_spd = bosS5PartActInfoArray[index].blend_spd;
            }
        }
    }

    private static void gmBoss5BodySetDirection(AppMain.GMS_BOSS5_BODY_WORK body_work, int dir_type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        switch (dir_type)
        {
            case 0:
                obsObjectWork.dir.y = (ushort)AppMain.AKM_DEGtoA16(-90);
                obsObjectWork.disp_flag |= 1U;
                break;
            case 1:
                obsObjectWork.dir.y = (ushort)AppMain.AKM_DEGtoA16(90);
                obsObjectWork.disp_flag &= 4294967294U;
                break;
            case 2:
                obsObjectWork.dir.y = (ushort)0;
                obsObjectWork.disp_flag &= 4294967294U;
                break;
            default:
                obsObjectWork.dir.y = (ushort)AppMain.AKM_DEGtoA16(-90);
                obsObjectWork.disp_flag |= 1U;
                break;
        }
    }

    private static int gmBoss5BodyIsPlayerBehind(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK partObjCore = body_work.part_obj_core;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (partObjCore.pos.x < playerObj.pos.x)
                return 1;
        }
        else if (playerObj.pos.x < partObjCore.pos.x)
            return 1;
        return 0;
    }

    private static void gmBoss5BodySetNoHitTime(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_RECT_WORK[][] obsRectWorkArray = new AppMain.OBS_RECT_WORK[3][]
        {
      body_work.sub_rect_work[0],
      body_work.sub_rect_work[1],
      ((AppMain.GMS_ENEMY_COM_WORK) body_work).rect_work
        };
        body_work.no_hit_timer = (uint)AppMain.GMD_BOSS5_BODY_DMG_NO_HIT_TIME;
        body_work.flag |= 32U;
        for (int index = 0; index < 3; ++index)
        {
            obsRectWorkArray[index][0].flag |= 2048U;
            obsRectWorkArray[index][0].flag &= 4294967291U;
        }
    }

    private static void gmBoss5BodyUpdateNoHitTime(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.no_hit_timer != 0U)
        {
            --body_work.no_hit_timer;
        }
        else
        {
            AppMain.OBS_RECT_WORK[][] obsRectWorkArray = new AppMain.OBS_RECT_WORK[3][]
            {
        body_work.sub_rect_work[0],
        body_work.sub_rect_work[1],
        ((AppMain.GMS_ENEMY_COM_WORK) body_work).rect_work
            };
            body_work.flag &= 4294967263U;
            for (int index = 0; index < 3; ++index)
            {
                if (((long)body_work.def_rect_req_flag & (long)(1 << index)) != 0L)
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

    private static void gmBoss5BodySetPokeTriggerLimitTime(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.poke_trg_limit_timer = 120U;
        body_work.flag |= 128U;
    }

    private static void gmBoss5BodyUpdatePokeTriggerLimitTime(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.poke_trg_limit_timer != 0U)
            --body_work.poke_trg_limit_timer;
        else
            body_work.flag &= 4294967167U;
    }

    private static int gmBoss5BodyIsWithinPokeTriggerLimitTime(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        return ((int)body_work.flag & 128) != 0 ? 1 : 0;
    }

    private static void gmBoss5BodyClearPokeTriggerLimitTime(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.poke_trg_limit_timer = 0U;
        body_work.flag &= 4294967167U;
    }

    private static void gmBoss5BodyExecDamageRoutine(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.GMS_BOSS5_MGR_WORK mgrWork = body_work.mgr_work;
        if (mgrWork.life != 0)
            --mgrWork.life;
        if (0 < mgrWork.life)
        {
            body_work.flag |= 2147483648U;
            if (body_work.state == 2)
            {
                if (AppMain.gmBoss5BodyIsWithinPokeTriggerLimitTime(body_work) != 0)
                {
                    if (AppMain.gmBoss5BodyIsPoking(body_work) == 0)
                        body_work.flag |= 8388608U;
                }
                else
                    AppMain.gmBoss5BodySetPokeTriggerLimitTime(body_work);
            }
            AppMain.gmBoss5BodyTryStartTurret(body_work);
        }
        else if (((int)body_work.flag & 65536) == 0)
        {
            mgrWork.life = 1;
        }
        else
        {
            body_work.flag |= 4194304U;
            AppMain.GMM_BS_OBJ((object)body_work).flag |= 2U;
            mgrWork.life = 0;
        }

        mgrWork.life = Math.Max(mgrWork.life, 0);

        Console.WriteLine(mgrWork.life);
        AppMain.gmBoss5BodySeqTryRequestEnableStr(body_work);
    }

    private static void gmBoss5BodyUpdateMainRectPosition(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK partObjCore = body_work.part_obj_core;
        int x = partObjCore.pos.x - obsObjectWork.pos.x;
        int y = partObjCore.pos.y - obsObjectWork.pos.y;
        for (int index = 0; index < 3; ++index)
            AppMain.VEC_Set(ref body_work.ene_3d.ene_com.rect_work[index].rect.pos, x, y, 0);
    }

    private static void gmBoss5BodyUpdateSubRectPosition(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.snm_reg_id_tbl[0] = body_work.leg_snm_reg_ids[0];
        AppMain.snm_reg_id_tbl[1] = body_work.leg_snm_reg_ids[1];
        for (int index1 = 0; index1 < 2; ++index1)
        {
            AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(body_work.snm_work, AppMain.snm_reg_id_tbl[index1]);
            int x = AppMain.FX_F32_TO_FX32(snmMtx.M03) - body_work.pivot_prev_pos.x;
            int y = -AppMain.FX_F32_TO_FX32(snmMtx.M13) - body_work.pivot_prev_pos.y;
            for (int index2 = 0; index2 < 3; ++index2)
                AppMain.VEC_Set(ref body_work.sub_rect_work[index1][index2].rect.pos, x, y, 0);
        }
    }

    private static void gmBoss5BodySetupRect(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_RECT_WORK[][] obsRectWorkArray = new AppMain.OBS_RECT_WORK[3][]
        {
      body_work.sub_rect_work[0],
      body_work.sub_rect_work[1],
      ((AppMain.GMS_ENEMY_COM_WORK) body_work).rect_work
        };
        ushort[] numArray1 = new ushort[3]
        {
      (ushort) 0,
      (ushort) 2,
      (ushort) 1
        };
        ushort[] numArray2 = new ushort[3]
        {
      (ushort) 65533,
      ushort.MaxValue,
      (ushort) 65534
        };
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
            {
                AppMain.ObjRectGroupSet(body_work.sub_rect_work[index1][index2], (byte)1, (byte)1);
                AppMain.ObjRectAtkSet(body_work.sub_rect_work[index1][index2], numArray1[index2], (short)1);
                AppMain.ObjRectDefSet(body_work.sub_rect_work[index1][index2], numArray2[index2], (short)0);
                body_work.sub_rect_work[index1][index2].parent_obj = obsObjectWork;
                body_work.sub_rect_work[index1][index2].flag &= 4294967291U;
            }
            body_work.sub_rect_work[index1][0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.GmEnemyDefaultDefFunc);
            body_work.sub_rect_work[index1][1].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.GmEnemyDefaultAtkFunc);
            body_work.sub_rect_work[index1][2].flag |= 1048800U;
        }
        for (int index = 0; index < 3; ++index)
            obsRectWorkArray[index][0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss5BodyDamageDefFunc);
        AppMain.gmBoss5BodyChangeRectSetting(body_work, 0);
    }

    private static void gmBoss5BodyChangeRectSetting(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int rect_setting)
    {
        AppMain.OBS_RECT_WORK[][] obsRectWorkArray = new AppMain.OBS_RECT_WORK[3][]
        {
      body_work.sub_rect_work[0],
      body_work.sub_rect_work[1],
      ((AppMain.GMS_ENEMY_COM_WORK) body_work).rect_work
        };
        AppMain.GMS_BOSS5_BODY_RECT_SETTING_INFO bodyRectSettingInfo = AppMain.gm_boss5_body_rect_setting_info_tbl[rect_setting];
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
            AppMain.GMS_BOSS5_RECTPOINT_SETTING_INFO rectpointSettingInfo = bodyRectSettingInfo.point_setting_info[index1];
            for (int index2 = 0; index2 < 3; ++index2)
            {
                AppMain.ObjRectWorkSet(obsRectWorkArray[index1][index2], rectpointSettingInfo.rect_size[index2][0], rectpointSettingInfo.rect_size[index2][1], rectpointSettingInfo.rect_size[index2][2], rectpointSettingInfo.rect_size[index2][3]);
                if (((long)(1 << index2) & (long)rectpointSettingInfo.enable_bit_flag) != 0L)
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

    private static void gmBoss5BodyChangeRectSettingDefault(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.gmBoss5BodySeqIsStr(body_work) != 0)
            AppMain.gmBoss5BodyChangeRectSetting(body_work, 1);
        else
            AppMain.gmBoss5BodyChangeRectSetting(body_work, 0);
    }

    private static void gmBoss5BodySwitchEnableLegRectOneSide(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
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

    private static void gmBoss5BodyTryImmobilizePlayer(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
        if (((int)AppMain.GmBsCmnGetPlayerObj().move_flag & 1) == 0)
            return;
        AppMain.GmPlySeqGmkInitBoss5Quake((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), AppMain.GMD_BOSS5_BODY_CRASH_PLY_IMMOBILE_TIME);
    }

    private static void gmBoss5BodyAllocSeHandles(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.se_hnd_leakage = AppMain.GsSoundAllocSeHandle();
    }

    private static void gmBoss5BodyFreeSeHandles(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.se_hnd_leakage == null)
            return;
        AppMain.GsSoundFreeSeHandle(body_work.se_hnd_leakage);
        body_work.se_hnd_leakage = (AppMain.GSS_SND_SE_HANDLE)null;
    }

    private static void gmBoss5BodyInitPlayTargetSe(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      float init_interval)
    {
        body_work.targ_se_cur_interval = init_interval;
        AppMain.GmBoss5Init1ShotTimer(body_work.targ_se_timer, (uint)body_work.targ_se_cur_interval);
        AppMain.GmSoundPlaySE("FinalBoss05");
    }

    private static void gmBoss5BodyUpdatePlayTargetSe(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if ((double)body_work.targ_se_cur_interval >= 0.0)
            body_work.targ_se_cur_interval -= AppMain.GMD_BOSS5_BODY_SE_TARGET_INTERVAL_DEC_SPD;
        else
            body_work.targ_se_cur_interval = 0.0f;
        if (AppMain.GmBoss5Update1ShotTimer(body_work.targ_se_timer) == 0)
            return;
        uint frame = (uint)body_work.targ_se_cur_interval;
        if ((long)frame <= (long)AppMain.GMD_BOSS5_BODY_SE_TARGET_INTERVAL_MIN)
            frame = (uint)AppMain.GMD_BOSS5_BODY_SE_TARGET_INTERVAL_MIN;
        AppMain.GmSoundPlaySE("FinalBoss05");
        AppMain.GmBoss5Init1ShotTimer(body_work.targ_se_timer, frame);
    }

    private static void gmBoss5BodyForceEndLeakage(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294963199U;
        AppMain.GmBoss5EfctEndLeakage(body_work, 1);
    }

    private static void gmBoss5BodyInitPlySearch(AppMain.GMS_BOSS5_BODY_WORK body_work, int delay)
    {
        body_work.ply_search_delay = delay;
        AppMain.GmBsCmnInitDelaySearch(body_work.dsearch_work, AppMain.GmBsCmnGetPlayerObj(), body_work.search_hist_buf, 11);
    }

    private static void gmBoss5BodyUpdatePlySearch(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.GmBsCmnUpdateDelaySearch(body_work.dsearch_work);
    }

    private static void gmBoss5BodySetPlyRebound(
      AppMain.GMS_PLAYER_WORK ply_work,
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)ply_work;
        AppMain.GmPlySeqAtkReactionInit(ply_work);
        if (ply_work.seq_state == 20)
        {
            AppMain.GmPlySeqSetJumpState(ply_work, 0, 5U);
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ply_work.obj_work.move.x < 0 ? AppMain.GMD_BOSS5_BODY_PLY_HOMING_REBOUND_X : -AppMain.GMD_BOSS5_BODY_PLY_HOMING_REBOUND_X;
            ply_work.obj_work.spd.y = obsObjectWork.pos.y > body_work.part_obj_core.pos.y ? -AppMain.GMD_BOSS5_BODY_PLY_HOMING_REBOUND_Y : AppMain.GMD_BOSS5_BODY_PLY_HOMING_REBOUND_Y;
            AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, AppMain.GMD_BOSS5_BODY_PLY_HOMING_REBOUND_NOJUMPMOVE_TIME);
        }
        else
        {
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ply_work.obj_work.move.x < 0 ? AppMain.GMD_BOSS5_BODY_PLY_NML_REBOUND_X : -AppMain.GMD_BOSS5_BODY_PLY_NML_REBOUND_X;
            ply_work.obj_work.spd.y = obsObjectWork.pos.y > body_work.part_obj_core.pos.y ? -AppMain.GMD_BOSS5_BODY_PLY_NML_REBOUND_Y : AppMain.GMD_BOSS5_BODY_PLY_NML_REBOUND_Y;
            AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, AppMain.GMD_BOSS5_BODY_PLY_NML_REBOUND_NOJUMPMOVE_TIME);
            ply_work.homing_timer = AppMain.GMD_BOSS5_BODY_DMG_NO_HIT_TIME * 4096;
        }
    }

    private static void gmBoss5BodySetMoveFastTime(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      uint fast_move_time)
    {
        body_work.fast_move_timer = fast_move_time;
    }

    private static void gmBoss5BodyUpdateMoveFastTime(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.fast_move_timer == 0U)
            return;
        --body_work.fast_move_timer;
    }

    private static int gmBoss5BodyIsMoveFastEnd(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        return body_work.fast_move_timer != 0U ? 0 : 1;
    }

    private static int gmBoss5BodyGetStompFallPosX(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int search_pos_x)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
        int num1 = 0;
        int num2 = search_pos_x - AppMain.GMD_BOSS5_BODY_STOMP_FALL_POS_MARGIN;
        int num3 = search_pos_x + AppMain.GMD_BOSS5_BODY_STOMP_FALL_POS_MARGIN;
        if (num2 <= AppMain.GMM_BOSS5_AREA_LEFT())
            num1 = AppMain.GMM_BOSS5_AREA_LEFT() - num2;
        else if (num3 >= AppMain.GMM_BOSS5_AREA_RIGHT())
            num1 = AppMain.GMM_BOSS5_AREA_RIGHT() - num3;
        return search_pos_x + num1;
    }

    private static void gmBoss5BodyDecideCrashFallPosX(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        ushort sec = 0;
        uint frame = AppMain.g_gm_main_system.game_time < 35999U ? AppMain.g_gm_main_system.game_time : 35999U;
        AppMain.AkUtilFrame60ToTime(frame, new ushort?(), ref sec, new ushort?());
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

    private static int gmBoss5BodyGetCrashFallPosX(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        return AppMain.GMM_BOSS5_AREA_CENTER_X() + body_work.crash_pos_ofst_x;
    }

    private static int gmBoss5BodyCheckJetSmokeClearTiming(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        return AppMain.GMM_BS_OBJ((object)body_work).pos.y <= body_work.ground_v_pos - AppMain.GMD_BOSS5_BODY_JETSMOKE_CLEAR_HEIGHT ? 1 : 0;
    }

    private static int gmBoss5BodyIsMoveFastDirFwd(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        return body_work.state == 3 || body_work.state != 4 ? 1 : 0;
    }

    private static int gmBoss5BodyIsBodyExplosionStopAllowed(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        int num = AppMain.ObjViewOutCheck(body_work.part_obj_core.pos.x, body_work.ground_v_pos, (short)0, (short)-(AppMain.GMD_BOSS5_EXPL_BODY_OFST_X + AppMain.GMD_BOSS5_EXPL_BODY_WIDTH / 2 >> 12), (short)-(AppMain.GMD_BOSS5_EXPL_BODY_OFST_Y + AppMain.GMD_BOSS5_EXPL_BODY_HEIGHT / 2 >> 12), (short)-(AppMain.GMD_BOSS5_EXPL_BODY_OFST_X - AppMain.GMD_BOSS5_EXPL_BODY_WIDTH / 2 >> 12), (short)-(AppMain.GMD_BOSS5_EXPL_BODY_OFST_Y - AppMain.GMD_BOSS5_EXPL_BODY_HEIGHT / 2 >> 12));
        if (AppMain.GmBsCmnGetPlayerObj().pos.x < body_work.part_obj_core.pos.x)
            num = 0;
        return num != 0 ? 1 : 0;
    }

    private static int gmBoss5BodyGetStompFallDirectionType(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        return obsObjectWork.pos.x <= AppMain.GMM_BOSS5_AREA_LEFT() + AppMain.GMD_BOSS5_BODY_STOMP_WALL_BEHIND_WALL_DISTANCE || obsObjectWork.pos.x < AppMain.GMM_BOSS5_AREA_RIGHT() - AppMain.GMD_BOSS5_BODY_STOMP_WALL_BEHIND_WALL_DISTANCE && (obsObjectWork.pos.x < AppMain.GmBsCmnGetPlayerObj().pos.x || obsObjectWork.pos.x <= AppMain.GmBsCmnGetPlayerObj().pos.x && ((int)AppMain.GmBsCmnGetPlayerObj().disp_flag & 1) == 0) ? 1 : 0;
    }

    private static void gmBoss5BodyTryStartTurret(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.mgr_work.life > AppMain.GMD_BOSS5_TURRET_START_LIFE_THRESHOLD || ((int)body_work.flag & 1024) != 0)
            return;
        body_work.flag |= 1024U;
        AppMain.GmBoss5TurretStartUp(body_work);
    }

    private static void gmBoss5BodyRecordGapAdjustmentDest(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        int[] numArray = new int[2]
        {
      body_work.lfoot_snm_reg_id,
      body_work.rfoot_snm_reg_id
        };
        for (int index = 0; index < 2; ++index)
        {
            AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(body_work.snm_work, numArray[index]);
            int x = body_work.grdmv_pivot_pos.x;
            int fx32 = AppMain.FX_F32_TO_FX32(snmMtx.M03);
            body_work.foot_ofst_record_dest[index] = fx32 - x;
            if (((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) != 0)
                body_work.foot_ofst_record_dest[index] = -body_work.foot_ofst_record_dest[index];
        }
    }

    private static void gmBoss5BodyRecordGapAdjustmentSrc(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        int[] numArray = new int[2]
        {
      body_work.lfoot_snm_reg_id,
      body_work.rfoot_snm_reg_id
        };
        for (int index = 0; index < 2; ++index)
        {
            AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(body_work.snm_work, numArray[index]);
            int x = body_work.grdmv_pivot_pos.x;
            int fx32 = AppMain.FX_F32_TO_FX32(snmMtx.M03);
            body_work.foot_ofst_record_src[index] = fx32 - x;
            if (((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) != 0)
                body_work.foot_ofst_record_src[index] = -body_work.foot_ofst_record_src[index];
        }
    }

    private static void gmBoss5BodyInitAdjustMtnBlendHGap(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int dest_act_id,
      int leg_type)
    {
        body_work.adj_hgap_is_active = 1;
        body_work.adj_hgap_act_id = dest_act_id;
        body_work.adj_hgap_leg_type = leg_type;
    }

    private static void gmBoss5BodyUpdateAdjustMtnBlendHGap(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (body_work.adj_hgap_is_active == 0)
            return;
        int num = (int)((double)(body_work.foot_ofst_record_dest[body_work.adj_hgap_leg_type] - body_work.foot_ofst_record_src[body_work.adj_hgap_leg_type]) * (double)AppMain.gm_boss5_act_id_tbl[body_work.adj_hgap_act_id][0].blend_spd);
        if (((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) != 0)
            obsObjectWork.pos.x += num;
        else
            obsObjectWork.pos.x -= num;
    }

    private static void gmBoss5BodyClearAdjustMtnBlendHGap(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.adj_hgap_is_active = 0;
    }

    private static void gmBoss5BodyInitGroundingMove(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int ref_snm_reg_id)
    {
        AppMain.GMS_BOSS5_GRD_MOVE_WORK grdmvWork = body_work.grdmv_work;
        grdmvWork.cur_diff_x = 0;
        grdmvWork.prev_diff_x = 0;
        grdmvWork.ref_snm_reg_id = ref_snm_reg_id;
        grdmvWork.is_first_updated = 0;
    }

    private static void gmBoss5BodyUpdateGroundingMove(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.GMS_BOSS5_GRD_MOVE_WORK grdmvWork = body_work.grdmv_work;
        if (grdmvWork.ref_snm_reg_id == -1)
            return;
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(body_work.snm_work, grdmvWork.ref_snm_reg_id);
        int x = body_work.grdmv_pivot_pos.x;
        int fx32 = AppMain.FX_F32_TO_FX32(snmMtx.M03);
        grdmvWork.prev_diff_x = grdmvWork.cur_diff_x;
        grdmvWork.cur_diff_x = fx32 - x;
        if (grdmvWork.is_first_updated == 0)
            grdmvWork.is_first_updated = 1;
        else
            AppMain.GMM_BS_OBJ((object)body_work).pos.x -= grdmvWork.cur_diff_x - grdmvWork.prev_diff_x;
    }

    private static void gmBoss5BodyChangeMovePhase(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int move_phase_type)
    {
        body_work.cur_move_phase_type = move_phase_type;
        int ref_snm_reg_id = body_work.cur_move_phase_type != 1 ? (body_work.cur_move_phase_type != 2 ? -1 : body_work.rfoot_snm_reg_id) : body_work.lfoot_snm_reg_id;
        AppMain.gmBoss5BodyInitGroundingMove(body_work, ref_snm_reg_id);
    }

    private static void gmBoss5BodyInitWalk(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodyChangeMovePhase(body_work, AppMain.gm_boss5_body_walk_move_info_tbl[0].move_phase_type);
    }

    private static int gmBoss5BodyUpdateWalk(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        float num = AppMain.GMM_BS_OBJ((object)body_work).obj_3d.frame[0];
        AppMain.gmBoss5BodyUpdateGroundingMove(body_work);
        int index;
        for (index = 3; index >= 0; --index)
        {
            if ((double)AppMain.gm_boss5_body_walk_move_info_tbl[index].switching_frame <= (double)num)
            {
                if (AppMain.gm_boss5_body_walk_move_info_tbl[index].move_phase_type != body_work.cur_move_phase_type)
                {
                    AppMain.gmBoss5BodyChangeMovePhase(body_work, AppMain.gm_boss5_body_walk_move_info_tbl[index].move_phase_type);
                    break;
                }
                break;
            }
        }
        return index >= 3 ? 1 : 0;
    }

    private static void gmBoss5BodyInitWalkAbortRecovery(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int cur_move_phase_type)
    {
        int leg_type = cur_move_phase_type != 1 ? 1 : 0;
        AppMain.gmBoss5BodyInitWalkAbortRecoveryByLegType(body_work, leg_type);
    }

    private static void gmBoss5BodyInitWalkAbortRecoveryByLegType(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int leg_type)
    {
        AppMain.gmBoss5BodyRecordGapAdjustmentSrc(body_work);
        AppMain.gmBoss5BodyInitAdjustMtnBlendHGap(body_work, body_work.whole_act_id, leg_type);
        AppMain.gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
    }

    private static int gmBoss5BodyUpdateWalkAbortRecovery(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)body_work).obj_3d.flag & 1) == 0)
            return 1;
        AppMain.gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
        return 0;
    }

    private static void gmBoss5BodyInitMonitoringWalkEnd(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.walk_end_monitor_phase_cnt = 0;
        body_work.is_player_behind = 0;
    }

    private static int gmBoss5BodyUpdateMonitoringWalkEnd(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int? leg_type)
    {
        int leg_type1 = 0;
        return AppMain._gmBoss5BodyUpdateMonitoringWalkEnd(body_work, ref leg_type1, false);
    }

    private static int gmBoss5BodyUpdateMonitoringWalkEnd(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      ref int leg_type)
    {
        return AppMain._gmBoss5BodyUpdateMonitoringWalkEnd(body_work, ref leg_type, true);
    }

    private static int _gmBoss5BodyUpdateMonitoringWalkEnd(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      ref int leg_type,
      bool ltAvailable)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        float num1 = obsObjectWork.obj_3d.frame[0];
        int num2 = 0;
        if (AppMain.gmBoss5BodyIsPlayerBehind(body_work) != 0)
            body_work.is_player_behind = 1;
        int index;
        for (index = 4; index >= 0; --index)
        {
            if ((double)AppMain.gm_boss5_body_walk_ground_timing_info_tbl[index].grounding_frame <= (double)num1)
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
            leg_type = index < 0 ? 0 : AppMain.gm_boss5_body_walk_ground_timing_info_tbl[index].leg_type;
        if (index >= 4 || num2 == 0)
            return 0;
        if (body_work.is_player_behind != 0)
            return 1;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.pos.x <= AppMain.GMM_BOSS5_AREA_LEFT() + AppMain.GMD_BOSS5_BODY_WALK_WALK_END_WALL_DISTANCE)
                return 1;
        }
        else if (obsObjectWork.pos.x >= AppMain.GMM_BOSS5_AREA_RIGHT() - AppMain.GMD_BOSS5_BODY_WALK_WALK_END_WALL_DISTANCE)
            return 1;
        return 0;
    }

    private static void gmBoss5BodyInitWalkGroundingEffects(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.cur_walk_grnd_phase_cnt = 0;
    }

    private static int gmBoss5BodyUpdateWalkGroundingEffects(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        float num = AppMain.GMM_BS_OBJ((object)body_work).obj_3d.frame[0];
        int index;
        for (index = 4; index >= 0; --index)
        {
            if ((double)AppMain.gm_boss5_body_walk_ground_timing_info_tbl[index].grounding_frame <= (double)num)
            {
                if (index != body_work.cur_walk_grnd_phase_cnt)
                {
                    AppMain.GmBoss5EfctCreateWalkStepSmoke(body_work, AppMain.gm_boss5_body_walk_ground_timing_info_tbl[index].leg_type);
                    AppMain.gmBoss5Vibration(0);
                    AppMain.GmSoundPlaySE("FinalBoss03");
                    body_work.cur_walk_grnd_phase_cnt = index;
                    break;
                }
                break;
            }
        }
        return index >= 4 ? 1 : 0;
    }

    private static void gmBoss5BodyInitRunGroundingEffects(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int run_type,
      uint delay)
    {
        body_work.run_grnd_runtype = run_type;
        body_work.run_grnd_delay_timer = delay;
        body_work.run_grnd_spawn_remain = 1U;
    }

    private static int gmBoss5BodyUpdateRunGroundingEffects(AppMain.GMS_BOSS5_BODY_WORK body_work)
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
                AppMain.GmBoss5EfctCreateRunStepSmoke(body_work, 0);
                break;
            case 1:
                AppMain.GmBoss5EfctCreateRunStepSmoke(body_work, 1);
                break;
        }
        AppMain.gmBoss5Vibration(1);
        AppMain.GmSoundPlaySE("FinalBoss03");
        return 1;
    }

    private static void gmBoss5BodyInitStompFlyUp(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag &= 4294967166U;
        obj_work.move_flag |= 272U;
        obj_work.spd.y = AppMain.GMD_BOSS5_BODY_SFLYUP_INIT_SPD;
        obj_work.spd_add.y = AppMain.GMD_BOSS5_BODY_SFLYUP_ACC;
    }

    private static int gmBoss5BodyUpdateStompFlyUp(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (obj_work.pos.y > AppMain.GMM_BOSS5_AREA_TOP() - AppMain.GMD_BOSS5_BODY_HIDE_RADIUS)
            return 0;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        return 1;
    }

    private static void gmBoss5BodyInitStompFall(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.spd.y = AppMain.GMD_BOSS5_BODY_STOMP_FALL_INIT_SPD;
        obj_work.spd_add.y = AppMain.GMD_BOSS5_BODY_STOMP_FALL_ACC;
    }

    private static int gmBoss5BodyUpdateStompFall(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (obj_work.pos.y >= AppMain.GMM_BOSS5_AREA_TOP())
            obj_work.move_flag &= 4294967039U;
        if (((int)obj_work.move_flag & 1) == 0)
            return 0;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag |= 128U;
        return 1;
    }

    private static void gmBoss5BodyInitCrashFlyUp(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag &= 4294967166U;
        obj_work.move_flag |= 272U;
        obj_work.spd.y = AppMain.GMD_BOSS5_BODY_CFLYUP_INIT_SPD;
        obj_work.spd_add.y = AppMain.GMD_BOSS5_BODY_CFLYUP_ACC;
    }

    private static int gmBoss5BodyUpdateCrashFlyUp(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (obj_work.pos.y > AppMain.GMM_BOSS5_AREA_TOP() - AppMain.GMD_BOSS5_BODY_HIDE_RADIUS)
            return 0;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        return 1;
    }

    private static void gmBoss5BodyInitCrashFall(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.spd.y = AppMain.GMD_BOSS5_BODY_CRASH_FALL_INIT_SPD;
        obj_work.spd_add.y = AppMain.GMD_BOSS5_BODY_CRASH_FALL_ACC;
    }

    private static int gmBoss5BodyUpdateCrashFall(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (obj_work.pos.y >= AppMain.GMM_BOSS5_AREA_TOP())
            obj_work.move_flag &= 4294967039U;
        if (((int)obj_work.move_flag & 1) == 0)
            return 0;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag |= 128U;
        return 1;
    }

    private static void gmBoss5BodyInitCrashSink(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag |= 384U;
        obj_work.move_flag &= 4294967294U;
    }

    private static int gmBoss5BodyUpdateCrashSink(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
        return 0;
    }

    private static void gmBoss5BodyInitBerserkTurn(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int turn_type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        float n;
        switch (turn_type)
        {
            case 0:
                n = AppMain.GMD_BOSS5_BODY_BERSERK_TURN_FRONT_DEG_F;
                break;
            case 1:
                n = AppMain.GMD_BOSS5_BODY_BERSERK_TURN_RETURN_DEG_F;
                break;
            default:
                n = 0.0f;
                break;
        }
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            n = -n;
        body_work.turn_src_dir = (int)((long)ushort.MaxValue & (long)obsObjectWork.dir.y);
        int num = (int)((long)ushort.MaxValue & (long)AppMain.AKM_DEGtoA32(n));
        body_work.turn_tgt_ofst_dir = (int)((long)ushort.MaxValue & (long)(num - body_work.turn_src_dir));
        if (body_work.turn_tgt_ofst_dir > AppMain.AKM_DEGtoA32(180))
            body_work.turn_tgt_ofst_dir = (int)((long)body_work.turn_tgt_ofst_dir - 65536L);
        body_work.turn_ratio = 0.0f;
    }

    private static int gmBoss5BodyUpdateBerserkTurn(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num1 = 0;
        body_work.turn_ratio += AppMain.GMD_BOSS5_BODY_BERSERK_TURN_RATIO_SPD;
        if ((double)body_work.turn_ratio >= 1.0)
        {
            body_work.turn_ratio = 1f;
            num1 = 1;
        }
        int num2 = (int)((double)body_work.turn_tgt_ofst_dir * (double)body_work.turn_ratio);
        obsObjectWork.dir.y = (ushort)((ulong)ushort.MaxValue & (ulong)(body_work.turn_src_dir + num2));
        return num1;
    }

    private static void gmBoss5BodyClearBerserkTurn(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) != 0)
            AppMain.gmBoss5BodySetDirection(body_work, 0);
        else
            AppMain.gmBoss5BodySetDirection(body_work, 1);
        body_work.turn_src_dir = 0;
        body_work.turn_tgt_ofst_dir = 0;
        body_work.turn_ratio = 0.0f;
    }

    private static void gmBoss5BodyInitPoke(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodyInitArmPose(body_work);
        body_work.arm_poke_anim_phase = 0;
        AppMain.gmBoss5BodyInitArmAnim(body_work, AppMain.gm_boss5_arm_anim_info_tbl[body_work.arm_poke_anim_phase]);
        body_work.flag |= 64U;
        body_work.flag |= 256U;
    }

    private static int gmBoss5BodyUpdatePoke(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        int num = 0;
        if (AppMain.gmBoss5BodyUpdateArmAnim(body_work) != 0)
        {
            ++body_work.arm_poke_anim_phase;
            if (body_work.arm_poke_anim_phase >= 9)
            {
                body_work.arm_poke_anim_phase = 8;
                num = 1;
            }
            else
                AppMain.gmBoss5BodyInitArmAnim(body_work, AppMain.gm_boss5_arm_anim_info_tbl[body_work.arm_poke_anim_phase]);
        }
        return num;
    }

    private static void gmBoss5BodyEndPoke(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294967039U;
        body_work.flag &= 4294967231U;
        AppMain.gmBoss5BodyEndArmPose(body_work);
        body_work.arm_poke_anim_phase = 0;
    }

    private static int gmBoss5BodyIsPoking(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        return ((int)body_work.flag & 64) != 0 ? 1 : 0;
    }

    private static void gmBoss5BodyInitArmAnim(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      AppMain.GMS_BOSS5_ARM_ANIM_INFO anim_info)
    {
        if (anim_info.wait_time > 0U)
        {
            body_work.arm_anim_work.is_anim = 0;
            body_work.arm_anim_work.anim_wait_timer = anim_info.wait_time;
            body_work.arm_anim_work.cur_rate = 0.0f;
            body_work.arm_anim_work.rate_add = 0.0f;
            for (int index = 0; index < 3; ++index)
            {
                AppMain.nnMakeRotateXYZQuaternion(out body_work.arm_anim_work.start_quat[index], anim_info.part_anim_info[index].start_rot.x, anim_info.part_anim_info[index].start_rot.y, anim_info.part_anim_info[index].start_rot.z);
                AppMain.nnMakeUnitQuaternion(ref body_work.arm_anim_work.end_quat[index]);
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
                AppMain.nnMakeRotateXYZQuaternion(out body_work.arm_anim_work.start_quat[index], anim_info.part_anim_info[index].start_rot.x, anim_info.part_anim_info[index].start_rot.y, anim_info.part_anim_info[index].start_rot.z);
                AppMain.nnMakeRotateXYZQuaternion(out body_work.arm_anim_work.end_quat[index], anim_info.part_anim_info[index].end_rot.x, anim_info.part_anim_info[index].end_rot.y, anim_info.part_anim_info[index].end_rot.z);
            }
        }
        for (int arm_part_idx = 0; arm_part_idx < 3; ++arm_part_idx)
        {
            AppMain.gmBoss5BodySetArmPoseParam(body_work, 0, arm_part_idx, ref body_work.arm_anim_work.start_quat[arm_part_idx]);
            AppMain.NNS_QUATERNION dst_quat;
            AppMain.AkMathInvertYZQuaternion(out dst_quat, ref body_work.arm_anim_work.start_quat[arm_part_idx]);
            AppMain.gmBoss5BodySetArmPoseParam(body_work, 1, arm_part_idx, ref dst_quat);
        }
        AppMain.gmBoss5BodyApplyArmPose(body_work);
    }

    private static int gmBoss5BodyUpdateArmAnim(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        int num = 0;
        if (body_work.arm_anim_work.is_anim != 0)
        {
            body_work.arm_anim_work.cur_rate += body_work.arm_anim_work.rate_add;
            if ((double)body_work.arm_anim_work.cur_rate >= 1.0)
            {
                body_work.arm_anim_work.cur_rate = 1f;
                num = 1;
            }
            for (int arm_part_idx = 0; arm_part_idx < 3; ++arm_part_idx)
            {
                AppMain.NNS_QUATERNION dst;
                AppMain.nnSlerpQuaternion(out dst, ref body_work.arm_anim_work.start_quat[arm_part_idx], ref body_work.arm_anim_work.end_quat[arm_part_idx], body_work.arm_anim_work.cur_rate);
                AppMain.gmBoss5BodySetArmPoseParam(body_work, 0, arm_part_idx, ref dst);
                AppMain.NNS_QUATERNION dst_quat;
                AppMain.AkMathInvertYZQuaternion(out dst_quat, ref dst);
                AppMain.gmBoss5BodySetArmPoseParam(body_work, 1, arm_part_idx, ref dst_quat);
            }
        }
        else if (body_work.arm_anim_work.anim_wait_timer != 0U)
            --body_work.arm_anim_work.anim_wait_timer;
        else
            num = 1;
        AppMain.gmBoss5BodyApplyArmPose(body_work);
        return num;
    }

    private static void gmBoss5BodyInitCloseCanopy(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.nnMakeRotateXYZQuaternion(out body_work.cnpy_close_init_quat, AppMain.GMD_BOSS5_BODY_CANOPY_CLOSE_START_ANGLE_X, 0, 0);
        AppMain.nnMakeRotateXYZQuaternion(out body_work.cnpy_close_dest_quat, 0, 0, 0);
        body_work.cnpy_close_ratio = 0.0f;
        body_work.cnpy_close_ratio_spd = 0.0f;
    }

    private static int gmBoss5BodyUpdateCloseCanopy(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int is_update)
    {
        AppMain.NNS_QUATERNION dst = new AppMain.NNS_QUATERNION();
        int num = 0;
        if (is_update != 0)
            body_work.cnpy_close_ratio_spd += AppMain.GMD_BOSS5_BODY_CANOPY_CLOSE_RATIO_SPD_ACC;
        if ((double)body_work.cnpy_close_ratio_spd >= (double)AppMain.GMD_BOSS5_BODY_CANOPY_CLOSE_RATIO_SPD_MAX)
            body_work.cnpy_close_ratio_spd = AppMain.GMD_BOSS5_BODY_CANOPY_CLOSE_RATIO_SPD_MAX;
        if (is_update != 0)
            body_work.cnpy_close_ratio += body_work.cnpy_close_ratio_spd;
        if ((double)body_work.cnpy_close_ratio >= 1.0)
        {
            body_work.cnpy_close_ratio = 1f;
            num = 1;
        }
        AppMain.nnSlerpQuaternion(out dst, ref body_work.cnpy_close_init_quat, ref body_work.cnpy_close_dest_quat, body_work.cnpy_close_ratio);
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMakeQuaternionMatrix(nnsMatrix1, ref dst);
        AppMain.GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix1, body_work.head_cnm_reg_id);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix1);
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMakeScaleMatrix(nnsMatrix2, 0.0f, 0.0f, 0.0f);
        AppMain.GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix2, body_work.pole_cnm_reg_id);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix2);
        return num;
    }

    private static void gmBoss5BodyInitScatterFall(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK pObj = AppMain.GMM_BS_OBJ((object)body_work);
        pObj.move_flag |= 144U;
        pObj.move_flag &= 4294967294U;
        AppMain.ObjObjectFieldRectSet(pObj, AppMain.GMD_BOSS5_BODY_SCT_FIEELD_RECT_SIZE_LEFT, AppMain.GMD_BOSS5_BODY_SCT_FIEELD_RECT_SIZE_TOP, AppMain.GMD_BOSS5_BODY_SCT_FIEELD_RECT_SIZE_RIGHT, AppMain.GMD_BOSS5_BODY_SCT_FIEELD_RECT_SIZE_BOTTOM);
        body_work.sct_land_vib_timer = AppMain.GMD_BOSS5_BODY_DEFEAT_SCT_FALL_LAND_VIB_TIME;
    }

    private static int gmBoss5BodyUpdateScatterFall(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (body_work.sct_land_vib_timer == 0U)
            return 1;
        if (((int)obsObjectWork.move_flag & 1) == 0)
        {
            int num = (int)((double)AppMain.GMD_BOSS5_BODY_DEFEAT_SCT_FALL_LAND_VIB_AMP * ((double)body_work.sct_land_vib_timer / (double)AppMain.GMD_BOSS5_BODY_DEFEAT_SCT_FALL_LAND_VIB_TIME));
            obsObjectWork.ofst.y = (int)((double)num * (double)AppMain.nnSin((int)((long)(AppMain.GMD_BOSS5_BODY_DEFEAT_SCT_FALL_LAND_VIB_TIME - body_work.sct_land_vib_timer) * (long)AppMain.GMD_BOSS5_BODY_DEFEAT_SCT_FALL_LAND_VIB_DEG_SPD)));
            --body_work.sct_land_vib_timer;
        }
        return 0;
    }

    private static void gmBoss5BodyInitShakeAccelerate(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        body_work.bsk_shake_acc_ratio = 0.0f;
        body_work.bsk_shake_acc_ratio_spd = 0.0f;
        body_work.bsk_shake_init_spd = obsObjectWork.obj_3d.speed[0];
    }

    private static int gmBoss5BodyUpdateShakeAccelerate(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num = 0;
        body_work.bsk_shake_acc_ratio_spd += AppMain.GMD_BOSS5_BODY_BERSERK_SHAKE_MOTION_SPD_RATIO_ACC;
        body_work.bsk_shake_acc_ratio += body_work.bsk_shake_acc_ratio_spd;
        if ((double)body_work.bsk_shake_acc_ratio >= 1.0)
        {
            body_work.bsk_shake_acc_ratio = 1f;
            body_work.bsk_shake_acc_ratio_spd = 0.0f;
            num = 1;
        }
        obsObjectWork.obj_3d.speed[0] = (float)((double)body_work.bsk_shake_acc_ratio * (double)AppMain.GMD_BOSS5_BODY_BERSERK_SHAKE_MOTION_SPD_DEST + (1.0 - (double)body_work.bsk_shake_acc_ratio) * (double)body_work.bsk_shake_init_spd);
        return num;
    }

    private static void gmBoss5BodyInitCrashStrikeVib(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      uint delay)
    {
        body_work.crash_strike_vib_delay_timer = delay;
        body_work.crash_strike_vib_phase = 0;
        body_work.crash_strike_vib_ratio = 0.0f;
    }

    private static int gmBoss5BodyUpdateCrashStrikeVib(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num = 0;
        if (body_work.crash_strike_vib_delay_timer != 0U)
        {
            --body_work.crash_strike_vib_delay_timer;
            return 0;
        }
        body_work.crash_strike_vib_ratio += AppMain.GMD_BOSS5_BODY_CRASH_STRIKE_BODY_VIB_RATIO_ADD;
        if ((double)body_work.crash_strike_vib_ratio >= 1.0)
        {
            body_work.crash_strike_vib_ratio = 1f;
            num = 1;
        }
        int scale = (int)((double)AppMain.GMD_BOSS5_BODY_CRASH_STRIKE_BODY_VIB_SCALE * (1.0 - (double)body_work.crash_strike_vib_ratio));
        body_work.crash_strike_vib_phase = AppMain.GmBoss5UpdateVib(body_work.crash_strike_vib_phase, scale, ref obsObjectWork.ofst.x, ref obsObjectWork.ofst.y);
        return num;
    }

    private static void gmBoss5BodyInitStartRiseVib(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.start_rise_vib_int_timer = 0U;
    }

    private static void gmBoss5BodyUpdateStartRiseVib(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.start_rise_vib_int_timer != 0U)
        {
            --body_work.start_rise_vib_int_timer;
        }
        else
        {
            float f32 = AppMain.FX_FX32_TO_F32(AppMain.FX_Div(AppMain.GMM_BS_OBJ((object)body_work).pos.y - body_work.ground_v_pos, AppMain.GMD_BOSS5_BODY_START_BURY_HEIGHT));
            AppMain.GmCameraVibrationSet((int)((double)AppMain.GMD_BOSS5_BODY_START_RISE_VIB_AMP_MAX * (double)AppMain.nnSin(AppMain.AKM_DEGtoA32(180f * f32))), 0, 0);
            body_work.start_rise_vib_int_timer = AppMain.GMD_BOSS5_BODY_START_RISE_VIB_INTERVAL;
        }
    }

    private static void gmBoss5BodyInitArmPose(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMakeUnitMatrix(nnsMatrix);
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
            {
                AppMain.GmBsCmnChangeCNMModeNode(body_work.cnm_mgr_work, body_work.arm_cnm_reg_id[index1][index2], 1U);
                AppMain.GmBsCmnEnableCNMLocalCoordinate(body_work.cnm_mgr_work, body_work.arm_cnm_reg_id[index1][index2], 1);
                AppMain.GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, body_work.arm_cnm_reg_id[index1][index2], 1);
                AppMain.GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix, body_work.arm_cnm_reg_id[index1][index2]);
                AppMain.nnMakeUnitQuaternion(ref body_work.arm_part_rot_quat[index1][index2]);
            }
            AppMain.nnMakeUnitMatrix(body_work.rkt_ofst_mtx[index1]);
        }
        body_work.flag |= 16U;
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss5BodyEndArmPose(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294967279U;
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
                AppMain.GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, body_work.arm_cnm_reg_id[index1][index2], 0);
        }
    }

    private static void gmBoss5BodySetArmPoseParam(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int arm_type,
      int arm_part_idx,
      ref AppMain.NNS_QUATERNION quat)
    {
        body_work.arm_part_rot_quat[arm_type][arm_part_idx] = quat;
    }

    private static void gmBoss5BodyApplyArmPose(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.NNS_MATRIX[][] nnsMatrixArray = AppMain.New<AppMain.NNS_MATRIX>(2, 3);
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
            {
                AppMain.nnMakeQuaternionMatrix(nnsMatrixArray[index1][index2], ref body_work.arm_part_rot_quat[index1][index2]);
                AppMain.GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrixArray[index1][index2], body_work.arm_cnm_reg_id[index1][index2]);
            }
        }
        for (int index = 0; index < 2; ++index)
        {
            AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_MATRIX nnsMatrix3 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_MATRIX nnsMatrix4 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
            AppMain.NNS_MATRIX snmMtx1 = AppMain.GmBsCmnGetSNMMtx(body_work.snm_work, body_work.armpt_snm_reg_ids[index][0]);
            AppMain.NNS_MATRIX snmMtx2 = AppMain.GmBsCmnGetSNMMtx(body_work.snm_work, body_work.armpt_snm_reg_ids[index][1]);
            AppMain.NNS_MATRIX snmMtx3 = AppMain.GmBsCmnGetSNMMtx(body_work.snm_work, body_work.armpt_snm_reg_ids[index][2]);
            AppMain.nnInvertMatrix(nnsMatrix1, snmMtx1);
            AppMain.nnInvertMatrix(nnsMatrix2, snmMtx2);
            AppMain.nnInvertMatrix(nnsMatrix3, snmMtx3);
            AppMain.nnMultiplyMatrix(nnsMatrix4, nnsMatrix3, snmMtx1);
            AppMain.nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, nnsMatrixArray[index][0]);
            AppMain.nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, nnsMatrix1);
            AppMain.nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, snmMtx2);
            AppMain.nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, nnsMatrixArray[index][1]);
            AppMain.nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, nnsMatrix2);
            AppMain.nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, snmMtx3);
            AppMain.nnMultiplyMatrix(nnsMatrix4, nnsMatrix4, nnsMatrixArray[index][2]);
            AppMain.nnCopyMatrix(body_work.rkt_ofst_mtx[index], nnsMatrix4);
            AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix1);
            AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix2);
            AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix3);
            AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix4);
        }
    }

    private static void gmBoss5BodyInitCanopyPartsPose(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        int[] numArray = new int[4]
        {
      body_work.head_cnm_reg_id,
      body_work.neck_cnm_reg_id,
      body_work.cover_cnm_reg_id,
      body_work.pole_cnm_reg_id
        };
        int length = numArray.Length;
        AppMain.nnMakeUnitMatrix(nnsMatrix);
        for (int index = 0; index < length; ++index)
        {
            AppMain.GmBsCmnChangeCNMModeNode(body_work.cnm_mgr_work, numArray[index], 1U);
            AppMain.GmBsCmnEnableCNMLocalCoordinate(body_work.cnm_mgr_work, numArray[index], 1);
            AppMain.GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, numArray[index], 1);
            AppMain.GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix, numArray[index]);
        }
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss5BodyEndCanopyPartsPose(AppMain.GMS_BOSS5_BODY_WORK body_work)
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
            AppMain.GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, numArray[index], 0);
    }

    private static int gmBoss5BodyTryTransitCrashWall(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (body_work.sub_seq == -1)
        {
            if (body_work.state == 3)
            {
                if (((int)obsObjectWork.move_flag & 4) != 0)
                {
                    AppMain.gmBoss5BodyStartSubsequence(body_work, 0);
                    return 1;
                }
            }
            else if (body_work.state == 4 && ((int)obsObjectWork.move_flag & 8) != 0)
            {
                AppMain.gmBoss5BodyStartSubsequence(body_work, 1);
                return 1;
            }
        }
        return 0;
    }

    private static void gmBoss5BodyResumeMoveFast(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.sub_seq == 0)
            AppMain.gmBoss5BodyChangeState(body_work, 4, body_work.strat_state, 1);
        else
            AppMain.gmBoss5BodyChangeState(body_work, 3, body_work.strat_state, 1);
    }

    private static int gmBoss5BodyReceiveSignalRocketReturned(AppMain.GMS_BOSS5_BODY_WORK body_work)
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

    private static void gmBoss5BodyInitCallbacks(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBsCmnInitBossMotionCBSystem(obj_work, body_work.bmcb_mgr);
        AppMain.GmBsCmnCreateSNMWork(body_work.snm_work, obj_work.obj_3d._object, (ushort)AppMain.GMD_BOSS5_BODY_NODE_SNM_NUM);
        AppMain.GmBsCmnAppendBossMotionCallback(body_work.bmcb_mgr, body_work.snm_work.bmcb_link);
        body_work.body_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_BODY);
        body_work.lfoot_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_FOOT_L);
        body_work.rfoot_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_FOOT_R);
        body_work.leg_snm_reg_ids[0] = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_LEG_L);
        body_work.leg_snm_reg_ids[1] = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_LEG_R);
        body_work.pole_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_POLE);
        body_work.groin_snm_reg_ids[0] = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_GROIN_L);
        body_work.groin_snm_reg_ids[1] = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_GROIN_R);
        body_work.nozzle_snm_reg_ids[0] = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_NOZZLE_L);
        body_work.nozzle_snm_reg_ids[1] = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_NOZZLE_R);
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
                body_work.armpt_snm_reg_ids[index1][index2] = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, AppMain.arm_part_node_ids[index1][index2]);
        }
        AppMain.GmBsCmnCreateCNMMgrWork(body_work.cnm_mgr_work, obj_work.obj_3d._object, (ushort)AppMain.GMD_BOSS5_BODY_NODE_CNM_NUM);
        AppMain.GmBsCmnInitCNMCb(obj_work, body_work.cnm_mgr_work);
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
                body_work.arm_cnm_reg_id[index1][index2] = AppMain.GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, AppMain.arm_part_node_ids[index1][index2]);
        }
        body_work.head_cnm_reg_id = AppMain.GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_HEAD);
        body_work.neck_cnm_reg_id = AppMain.GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_NECK);
        body_work.cover_cnm_reg_id = AppMain.GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_COVER);
        body_work.pole_cnm_reg_id = AppMain.GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, AppMain.GMD_BOSS5_BODY_NODE_IDX_POLE);
    }

    private static void gmBoss5BodyReleaseCallbacks(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBsCmnClearBossMotionCBSystem(obj_work);
        AppMain.GmBsCmnDeleteSNMWork(body_work.snm_work);
        AppMain.GmBsCmnClearCNMCb(obj_work);
        AppMain.GmBsCmnDeleteCNMMgrWork(body_work.cnm_mgr_work);
    }

    private static void gmBoss5BodyRegisterScatterPartsCNM(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        for (int index = 0; index < 22; ++index)
            body_work.scatter_cnm_reg_ids[index] = AppMain.GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, AppMain.gm_boss5_body_scatter_parts_cnm_node_id_tbl[index]);
    }

    private static void gmBoss5BodyDamageDefFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        AppMain.GMS_BOSS5_BODY_WORK body_work = (AppMain.GMS_BOSS5_BODY_WORK)parentObj1;
        int num1 = 0;
        int num2 = 0;
        if (parentObj2 != null)
        {
            if ((ushort)1 == parentObj2.obj_type)
                num1 = 1;
            else if ((ushort)2 == parentObj2.obj_type && ((AppMain.GMS_ENEMY_COM_WORK)parentObj2).eve_rec.id == (ushort)332)
                num2 = 1;
        }
        if (num1 == 0 && num2 == 0)
            return;
        if (num1 != 0)
        {
            AppMain.gmBoss5BodySetPlyRebound((AppMain.GMS_PLAYER_WORK)parentObj2, body_work);
            AppMain.GMM_PAD_VIB_SMALL_TIME(30f);
        }
        else
        {
            AppMain.gmBoss5BodyStartSubsequence(body_work, 2);
            AppMain.GMM_PAD_VIB_SMALL();
            AppMain.GmSoundPlaySE("FinalBoss15");
        }
        AppMain.gmBoss5BodySetNoHitTime(body_work);
        AppMain.GmSoundPlaySE("Boss0_01");
        AppMain.GmBoss5EfctCreateDamage(body_work);
        if (((int)body_work.flag & 1) != 0 && num2 == 0)
            return;
        AppMain.gmBoss5BodyExecDamageRoutine(body_work);
    }

    private static void gmBoss5BodyOutFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK gmsBosS5BodyWork = (AppMain.GMS_BOSS5_BODY_WORK)obj_work;
        AppMain.GmBsCmnUpdateCNMParam(obj_work, gmsBosS5BodyWork.cnm_mgr_work);
        AppMain.ObjDrawActionSummary(obj_work);
        gmsBosS5BodyWork.grdmv_pivot_pos.Assign(obj_work.pos);
        gmsBosS5BodyWork.pivot_prev_pos.Assign(obj_work.pos);
    }

    private static void gmBoss5BodyRecFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK gmsBosS5BodyWork = (AppMain.GMS_BOSS5_BODY_WORK)obj_work;
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 3; ++index2)
                AppMain.ObjObjectRectRegist(obj_work, gmsBosS5BodyWork.sub_rect_work[index1][index2]);
        }
    }

    private static void gmBoss5BodyChangeState(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int state,
      int strat_state)
    {
        AppMain.gmBoss5BodyChangeState(body_work, state, strat_state, 0);
    }

    private static void gmBoss5BodyChangeState(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int state,
      int strat_state,
      int is_wrapped)
    {
        AppMain.UNREFERENCED_PARAMETER((object)is_wrapped);
        AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK gmsBosS5BodyWork = AppMain.gm_boss5_body_state_leave_func_tbl[body_work.state];
        if (gmsBosS5BodyWork != null)
            gmsBosS5BodyWork(body_work);
        body_work.prev_state = body_work.state;
        body_work.state = state;
        body_work.sub_seq = -1;
        body_work.strat_state = strat_state;
        AppMain.GMS_BOSS5_BODY_STATE_ENTER_INFO bodyStateEnterInfo = AppMain.gm_boss5_body_state_enter_info_tbl[body_work.state];
        if (bodyStateEnterInfo.enter_func == null)
            return;
        bodyStateEnterInfo.enter_func(body_work);
    }

    private static void gmBoss5BodyStartSubsequence(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int sub_seq)
    {
        AppMain.GMS_BOSS5_BODY_SUBSEQ_ENTER_INFO bodySubseqEnterInfo = AppMain.gm_boss5_body_sub_seq_enter_func_tbl[sub_seq];
        body_work.sub_seq = sub_seq;
        if (bodySubseqEnterInfo.enter_func == null)
            return;
        bodySubseqEnterInfo.enter_func(body_work);
    }

    private static void gmBoss5BodyWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK body_work = (AppMain.GMS_BOSS5_BODY_WORK)obj_work;
        if (((int)body_work.mgr_work.flag & 1) == 0)
            return;
        AppMain.gmBoss5BodyInitCallbacks(body_work);
        AppMain.GmBoss5RocketSpawnConnected(body_work, 0);
        AppMain.GmBoss5RocketSpawnConnected(body_work, 1);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5BodyMain);
        AppMain.gmBoss5BodyChangeState(body_work, 1, 1);
    }

    private static void gmBoss5BodyMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK gmsBosS5BodyWork = (AppMain.GMS_BOSS5_BODY_WORK)obj_work;
        int num = 0;
        AppMain.gmBoss5BodyUpdatePokeTriggerLimitTime(gmsBosS5BodyWork);
        AppMain.gmBoss5BodyUpdateNoHitTime(gmsBosS5BodyWork);
        if (gmsBosS5BodyWork.sub_seq != 0 && gmsBosS5BodyWork.sub_seq != 1)
            AppMain.gmBoss5BodyUpdateMoveFastTime(gmsBosS5BodyWork);
        if (((int)gmsBosS5BodyWork.flag & 131072) == 0)
        {
            if (((int)gmsBosS5BodyWork.flag & 4194304) != 0)
            {
                gmsBosS5BodyWork.flag &= 2143289343U;
                AppMain.GmBsCmnInitObject3DNNDamageFlicker(obj_work, gmsBosS5BodyWork.flk_work, AppMain.GMD_BOSS5_BODY_DMG_FLICKER_RADIUS);
                AppMain.gmBoss5BodyProceedToDefeatState(gmsBosS5BodyWork);
                num = 1;
            }
            if (num == 0 && AppMain.gmBoss5BodyTryTransitCrashWall(gmsBosS5BodyWork) != 0)
            {
                num = 1;
                AppMain.GmSoundPlaySE("FinalBoss12");
            }
        }
        if (num == 0 && gmsBosS5BodyWork.proc_update != null)
            gmsBosS5BodyWork.proc_update(gmsBosS5BodyWork);
        if (((int)gmsBosS5BodyWork.flag & int.MinValue) != 0)
        {
            gmsBosS5BodyWork.flag &= (uint)int.MaxValue;
            AppMain.GmBsCmnInitObject3DNNDamageFlicker(obj_work, gmsBosS5BodyWork.flk_work, AppMain.GMD_BOSS5_BODY_DMG_FLICKER_RADIUS);
        }
        AppMain.GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, gmsBosS5BodyWork.flk_work);
        if (((int)gmsBosS5BodyWork.flag & 4096) != 0)
            AppMain.GmBoss5EfctTryStartLeakage(gmsBosS5BodyWork);
        else
            AppMain.GmBoss5EfctEndLeakage(gmsBosS5BodyWork);
    }

    private static void gmBoss5BodyStateEnterStart(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss5BodySetActionWhole(body_work, 0, 1);
        AppMain.gmBoss5BodySetDirection(body_work, 0);
        obsObjectWork.move_flag &= 4294967039U;
        obsObjectWork.move_flag |= 128U;
        body_work.flag |= 8192U;
        obsObjectWork.spd.y = obsObjectWork.spd_fall_max;
        obsObjectWork.pos.z = AppMain.GMD_BOSS5_BG_FARSIDE_POS_Z;
        AppMain.gmBoss5BodyChangeRectSetting(body_work, 2);
        AppMain.GmBoss5EggCreate(body_work, obsObjectWork.pos.x, obsObjectWork.pos.y);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStartWithPlacement);
    }

    private static void gmBoss5BodyStateLeaveStart(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss5RestoreCameraSlideForNarrowScreen(body_work.mgr_work);
        AppMain.gmBoss5BodyEndCanopyPartsPose(body_work);
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
        body_work.flag &= 4294959103U;
        obsObjectWork.move_flag &= 4294967039U;
        obsObjectWork.move_flag |= 128U;
        obsObjectWork.pos.z = AppMain.GMD_BOSS5_DEFAULT_POS_Z;
    }

    private static void gmBoss5BodyStateUpdateStartWithPlacement(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (((int)obsObjectWork.move_flag & 1) == 0)
            return;
        body_work.ground_v_pos = obsObjectWork.pos.y;
        obsObjectWork.move_flag |= 256U;
        obsObjectWork.move_flag &= 4294967167U;
        obsObjectWork.pos.y += AppMain.GMD_BOSS5_BODY_START_BURY_HEIGHT;
        AppMain.GmBoss5CtpltCreate(body_work);
        body_work.mgr_work.flag |= 2097152U;
        AppMain.gmBoss5MgrSetDemoRunDestPos(body_work.mgr_work, obsObjectWork.pos.x + AppMain.GMD_BOSS5_PLY_OP_DEMO_RUN_DEST_X_OFST_FROM_BODY);
        AppMain.gmBoss5BodyInitCanopyPartsPose(body_work);
        AppMain.gmBoss5BodyInitCloseCanopy(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStartWithWaitEggRide);
    }

    private static void gmBoss5BodyStateUpdateStartWithWaitEggRide(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.mgr_work.flag & 524288) != 0 && ((int)body_work.mgr_work.flag & 1048576) == 0)
        {
            body_work.mgr_work.flag |= 1048576U;
            AppMain.gmBoss5SetCameraSlideForNarrowScreen(body_work.mgr_work);
        }
        AppMain.gmBoss5BodyUpdateCloseCanopy(body_work, 0);
        if (((int)body_work.flag & 16777216) == 0)
            return;
        body_work.flag &= 4278190079U;
        AppMain.gmBoss5BodyRecordGapAdjustmentDest(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStartWithCockpitClose);
    }

    private static void gmBoss5BodyStateUpdateStartWithCockpitClose(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.gmBoss5BodyUpdateCloseCanopy(body_work, 1) == 0)
            return;
        AppMain.GmSoundPlaySE("FinalBoss01");
        body_work.mgr_work.flag |= 33554432U;
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_START_WAIT_RISE_TIME;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStartWithWaitRise);
    }

    private static void gmBoss5BodyStateUpdateStartWithWaitRise(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            obsObjectWork.spd.y = AppMain.GMD_BOSS5_BODY_START_RISE_SPD_Y;
            AppMain.gmBoss5BodyInitStartRiseVib(body_work);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStartWithRise);
        }
    }

    private static void gmBoss5BodyStateUpdateStartWithRise(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GMS_BOSS5_MGR_WORK mgrWork = body_work.mgr_work;
        AppMain.gmBoss5BodyUpdateStartRiseVib(body_work);
        if (obj_work.pos.y > body_work.ground_v_pos)
            return;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.move_flag &= 4294967039U;
        obj_work.move_flag |= 128U;
        mgrWork.flag |= 8388608U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStartWithWaitCtplt);
    }

    private static void gmBoss5BodyStateUpdateStartWithWaitCtplt(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.mgr_work.flag & 16777216) == 0)
            return;
        AppMain.gmBoss5RestoreCameraSlideForNarrowScreen(body_work.mgr_work);
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_START_WAIT_END_TIME;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStartWithWaitEnd);
    }

    private static void gmBoss5BodyStateUpdateStartWithWaitEnd(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else
            AppMain.gmBoss5BodyProceedToNextSeqNml(body_work);
    }

    private static void gmBoss5BodyStateEnterMoveNml(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss5BodySetActionWhole(body_work, 2, 1);
        AppMain.gmBoss5BodyInitWalk(body_work);
        AppMain.gmBoss5BodyInitMonitoringWalkEnd(body_work);
        AppMain.gmBoss5BodyInitWalkGroundingEffects(body_work);
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
        obsObjectWork.move_flag &= 4294966271U;
        obsObjectWork.move_flag |= 524288U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveNmlWithLoop);
        if (AppMain.gmBoss5BodyIsPlayerBehind(body_work) == 0)
            return;
        AppMain.gmBoss5BodySetActionWhole(body_work, 3);
        AppMain.gmBoss5BodyInitWalkAbortRecovery(body_work, body_work.cur_move_phase_type);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveNmlWithAbort);
    }

    private static void gmBoss5BodyStateLeaveMoveNml(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss5BodyClearAdjustMtnBlendHGap(body_work);
        body_work.flag &= 4286578687U;
        AppMain.gmBoss5BodyClearPokeTriggerLimitTime(body_work);
        AppMain.gmBoss5BodyEndPoke(body_work);
        obsObjectWork.move_flag &= 4294443007U;
        obsObjectWork.move_flag |= 1024U;
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateMoveNmlWithLoop(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.gmBoss5BodyIsPoking(body_work) != 0 && AppMain.gmBoss5BodyUpdatePoke(body_work) != 0)
            AppMain.gmBoss5BodyEndPoke(body_work);
        AppMain.gmBoss5BodyUpdateWalkGroundingEffects(body_work);
        if (AppMain.gmBoss5BodyUpdateWalk(body_work) != 0 && AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) != 0 && AppMain.gmBoss5BodyIsPoking(body_work) == 0)
        {
            AppMain.gmBoss5BodyProceedToNextSeqNml(body_work);
        }
        else
        {
            int leg_type = 0;
            if (AppMain.gmBoss5BodyUpdateMonitoringWalkEnd(body_work, ref leg_type) != 0)
            {
                AppMain.gmBoss5BodySetActionWhole(body_work, 3);
                AppMain.gmBoss5BodyInitWalkAbortRecoveryByLegType(body_work, leg_type);
                body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveNmlWithAbort);
            }
            else
            {
                if (((int)body_work.flag & 8388608) == 0)
                    return;
                body_work.flag &= 4286578687U;
                AppMain.gmBoss5BodyInitPoke(body_work);
            }
        }
    }

    private static void gmBoss5BodyStateUpdateMoveNmlWithAbort(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.gmBoss5BodyIsPoking(body_work) != 0 && AppMain.gmBoss5BodyUpdatePoke(body_work) != 0)
            AppMain.gmBoss5BodyEndPoke(body_work);
        if (AppMain.gmBoss5BodyUpdateWalkAbortRecovery(body_work) == 0 || AppMain.gmBoss5BodyIsPoking(body_work) != 0)
            return;
        AppMain.gmBoss5BodyProceedToNextSeqNml(body_work);
    }

    private static void gmBoss5BodyStateEnterMoveFast(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
        if (AppMain.gmBoss5BodyIsMoveFastEnd(body_work) != 0)
        {
            AppMain.gmBoss5BodySetActionWhole(body_work, 20);
            AppMain.gmBoss5BodyRecordGapAdjustmentSrc(body_work);
            AppMain.gmBoss5BodyInitAdjustMtnBlendHGap(body_work, body_work.whole_act_id, 0);
            AppMain.gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
            body_work.wait_timer = AppMain.GMD_BOSS5_BODY_RUN_RECOVER_TIMEOUT_FRAME;
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveFastWithRecover);
        }
        else
        {
            if (body_work.prev_state == 10)
            {
                if (body_work.whole_act_id != 45)
                    AppMain.gmBoss5BodySetActionWhole(body_work, 45);
                AppMain.gmBoss5BodyClearAdjustMtnBlendHGap(body_work);
            }
            else
            {
                if (AppMain.gmBoss5BodyIsMoveFastDirFwd(body_work) != 0)
                    AppMain.gmBoss5BodySetActionWhole(body_work, 4);
                else
                    AppMain.gmBoss5BodySetActionWhole(body_work, 11);
                AppMain.gmBoss5BodyRecordGapAdjustmentSrc(body_work);
                AppMain.gmBoss5BodyInitAdjustMtnBlendHGap(body_work, body_work.whole_act_id, 0);
                AppMain.gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
            }
            obsObjectWork.move_flag &= 4294966271U;
            obsObjectWork.move_flag |= 524288U;
            body_work.cur_run_type = 1;
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveFastWithPrep);
        }
    }

    private static void gmBoss5BodyStateLeaveMoveFast(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.move_flag &= 4294967279U;
        obsObjectWork.move_flag &= 4294443007U;
        obsObjectWork.move_flag |= 1024U;
        AppMain.gmBoss5BodyClearAdjustMtnBlendHGap(body_work);
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithPrep(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (((int)obj_work.obj_3d.flag & 1) != 0)
            AppMain.gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
        else
            AppMain.gmBoss5BodyClearAdjustMtnBlendHGap(body_work);
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        AppMain.gmBoss5BodyClearAdjustMtnBlendHGap(body_work);
        obj_work.spd.x = AppMain.GMD_BOSS5_BODY_RUN_FWD_JUMP_INIT_SPD_X;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -obj_work.spd.x;
        if (AppMain.gmBoss5BodyIsMoveFastDirFwd(body_work) != 0)
        {
            AppMain.gmBoss5BodySetActionWhole(body_work, 5);
        }
        else
        {
            obj_work.spd.x = AppMain.FX_Mul(-obj_work.spd.x, AppMain.GMD_BOSS5_BODY_RUN_BWD_SPD_X_FACTOR);
            AppMain.gmBoss5BodySetActionWhole(body_work, 12);
        }
        AppMain.gmBoss5BodySwitchEnableLegRectOneSide(body_work, 1);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveFastWithJump);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithJump(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.GmBsCmnIsActionEndFlexibly(obj_work, AppMain.GMD_BOSS5_BODY_RUN_ACT_FRAME_OVERRUN_ALLOW_RATIO) == 0)
            return;
        int num = AppMain.gmBoss5BodyIsMoveFastDirFwd(body_work);
        if (body_work.cur_run_type == 1)
        {
            if (num != 0)
                AppMain.gmBoss5BodySetActionWhole(body_work, 6);
            else
                AppMain.gmBoss5BodySetActionWhole(body_work, 13);
        }
        else if (num != 0)
            AppMain.gmBoss5BodySetActionWhole(body_work, 9);
        else
            AppMain.gmBoss5BodySetActionWhole(body_work, 16);
        obj_work.spd.x = AppMain.GMD_BOSS5_BODY_RUN_FWD_FLY_INIT_SPD_X;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -obj_work.spd.x;
        obj_work.spd.y = AppMain.GMD_BOSS5_BODY_RUN_FWD_FLY_INIT_SPD_Y;
        if (num == 0)
            obj_work.spd.x = AppMain.FX_Mul(-obj_work.spd.x, AppMain.GMD_BOSS5_BODY_RUN_BWD_SPD_X_FACTOR);
        obj_work.move_flag |= 144U;
        obj_work.move_flag &= 4294967294U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveFastWithAir);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithAir(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (((int)obj_work.move_flag & 1) == 0)
            return;
        int num = AppMain.gmBoss5BodyIsMoveFastDirFwd(body_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        AppMain.gmBoss5BodyInitRunGroundingEffects(body_work, body_work.cur_run_type, AppMain.GMD_BOSS5_BODY_RUN_GROUNDING_EFCT_DELAY);
        if (body_work.cur_run_type == 1)
        {
            if (num != 0)
                AppMain.gmBoss5BodySetActionWhole(body_work, 7);
            else
                AppMain.gmBoss5BodySetActionWhole(body_work, 14);
        }
        else if (num != 0)
            AppMain.gmBoss5BodySetActionWhole(body_work, 10);
        else
            AppMain.gmBoss5BodySetActionWhole(body_work, 17);
        if (AppMain.gmBoss5BodyIsMoveFastEnd(body_work) != 0)
        {
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveFastWithPreRecover);
            body_work.wait_timer = AppMain.GMD_BOSS5_BODY_RUN_PRE_RECOVER_TIMEOUT_FRAME;
        }
        else
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveFastWithLand);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithLand(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss5BodyUpdateRunGroundingEffects(body_work);
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        int num = AppMain.gmBoss5BodyIsMoveFastDirFwd(body_work);
        if (body_work.cur_run_type == 1)
        {
            body_work.cur_run_type = 0;
            if (num != 0)
                AppMain.gmBoss5BodySetActionWhole(body_work, 8);
            else
                AppMain.gmBoss5BodySetActionWhole(body_work, 15);
            AppMain.gmBoss5BodySwitchEnableLegRectOneSide(body_work, 0);
        }
        else
        {
            body_work.cur_run_type = 1;
            if (num != 0)
                AppMain.gmBoss5BodySetActionWhole(body_work, 5);
            else
                AppMain.gmBoss5BodySetActionWhole(body_work, 12);
            AppMain.gmBoss5BodySwitchEnableLegRectOneSide(body_work, 1);
        }
        obj_work.spd.x = AppMain.GMD_BOSS5_BODY_RUN_FWD_JUMP_INIT_SPD_X;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -obj_work.spd.x;
        if (num == 0)
            obj_work.spd.x = AppMain.FX_Mul(-obj_work.spd.x, AppMain.GMD_BOSS5_BODY_RUN_BWD_SPD_X_FACTOR);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveFastWithJump);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithPreRecover(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num = 0;
        AppMain.gmBoss5BodyUpdateRunGroundingEffects(body_work);
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else
            num = 1;
        if (((int)obsObjectWork.obj_3d.flag & 1) != 0 && num == 0)
            return;
        AppMain.gmBoss5BodySetActionWhole(body_work, 20);
        AppMain.gmBoss5BodyRecordGapAdjustmentSrc(body_work);
        if (body_work.cur_run_type == 1)
            AppMain.gmBoss5BodyInitAdjustMtnBlendHGap(body_work, body_work.whole_act_id, 1);
        else
            AppMain.gmBoss5BodyInitAdjustMtnBlendHGap(body_work, body_work.whole_act_id, 0);
        AppMain.gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_RUN_RECOVER_TIMEOUT_FRAME;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateMoveFastWithRecover);
    }

    private static void gmBoss5BodyStateUpdateMoveFastWithRecover(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num = 0;
        AppMain.gmBoss5BodyUpdateRunGroundingEffects(body_work);
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else
            num = 1;
        if (((int)obsObjectWork.obj_3d.flag & 1) == 0 || num != 0)
            AppMain.gmBoss5BodyProceedToNextSeqStr(body_work);
        else
            AppMain.gmBoss5BodyUpdateAdjustMtnBlendHGap(body_work);
    }

    private static void gmBoss5BodyStateEnterStomp(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodySetActionWhole(body_work, 21);
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_STOMP_IGNITE_TIME;
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
        AppMain.GmBoss5EfctStartJet(body_work);
        AppMain.GmBoss5EfctStartJetSmoke(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStompWithPrep);
    }

    private static void gmBoss5BodyStateLeaveStomp(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.move_flag &= 4294967023U;
        obsObjectWork.move_flag |= 128U;
        AppMain.GmBoss5EfctEndJetSmoke(body_work);
        AppMain.GmBoss5EfctEndJet(body_work);
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateStompWithPrep(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5BodySetActionWhole(body_work, 22);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStompWithHover);
        }
    }

    private static void gmBoss5BodyStateUpdateStompWithHover(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.gmBoss5BodySetActionWhole(body_work, 23);
        AppMain.gmBoss5BodyInitStompFlyUp(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStompWithFlyUp);
    }

    private static void gmBoss5BodyStateUpdateStompWithFlyUp(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.gmBoss5BodyCheckJetSmokeClearTiming(body_work) != 0)
            AppMain.GmBoss5EfctEndJetSmoke(body_work);
        if (AppMain.gmBoss5BodyUpdateStompFlyUp(body_work) == 0)
            return;
        AppMain.GmBoss5EfctEndJetSmoke(body_work);
        AppMain.GmBoss5EfctEndJet(body_work);
        AppMain.GmBoss5EfctTargetCursorInit(body_work);
        body_work.flag |= 2U;
        AppMain.gmBoss5BodyInitPlySearch(body_work, (int)AppMain.GMD_BOSS5_BODY_STOMP_SEARCH_DELAY);
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_STOMP_WAIT_TIME;
        AppMain.gmBoss5BodyInitPlayTargetSe(body_work, (float)AppMain.GMD_BOSS5_BODY_SE_TARGET_INIT_INTERVAL);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStompWithWait);
    }

    private static void gmBoss5BodyStateUpdateStompWithWait(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer >= AppMain.GMD_BOSS5_BODY_STOMP_NO_SEARCH_TIME)
            AppMain.gmBoss5BodyUpdatePlySearch(body_work);
        AppMain.gmBoss5BodyUpdatePlayTargetSe(body_work);
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
            AppMain.VecFx32 pos = new AppMain.VecFx32();
            body_work.flag &= 4294967293U;
            AppMain.GmBoss5BodyGetPlySearchPos(body_work, out pos);
            obsObjectWork.pos.x = AppMain.gmBoss5BodyGetStompFallPosX(body_work, pos.x);
            AppMain.gmBoss5BodySetActionWhole(body_work, 24);
            AppMain.gmBoss5BodySetDirection(body_work, AppMain.gmBoss5BodyGetStompFallDirectionType(body_work));
            AppMain.gmBoss5BodyInitStompFall(body_work);
            if (AppMain.gmBoss5BodySeqIsStr(body_work) != 0)
                AppMain.gmBoss5BodyChangeRectSetting(body_work, 4);
            else
                AppMain.gmBoss5BodyChangeRectSetting(body_work, 3);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStompWithFall);
        }
    }

    private static void gmBoss5BodyStateUpdateStompWithFall(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.gmBoss5BodyUpdateStompFall(body_work) == 0)
            return;
        AppMain.GmSoundPlaySE("FinalBoss06");
        AppMain.GMM_PAD_VIB_MID_TIME(30f);
        obsObjectWork.move_flag &= 4294967279U;
        AppMain.gmBoss5BodySetActionWhole(body_work, 25);
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
        AppMain.GmBoss5EfctCreateLandingShockwave(body_work);
        AppMain.gmBoss5Vibration(3);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateStompWithLand);
    }

    private static void gmBoss5BodyStateUpdateStompWithLand(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        if (body_work.state == 6)
            AppMain.gmBoss5BodyProceedToNextSeqStr(body_work);
        else
            AppMain.gmBoss5BodyProceedToNextSeqNml(body_work);
    }

    private static void gmBoss5BodyStateEnterCrash(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5MgrSetAlarmLevel(body_work.mgr_work, 1);
        body_work.flag |= 65536U;
        body_work.flag |= 512U;
        AppMain.gmBoss5BodySetActionWhole(body_work, 26);
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
        AppMain.GmBoss5EfctStartJet(body_work);
        AppMain.GmBoss5EfctStartJetSmoke(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateCrashWithPrep);
    }

    private static void gmBoss5BodyStateLeaveCrash(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        body_work.flag &= 4294959103U;
        AppMain.GMM_PAD_VIB_STOP();
        obsObjectWork.move_flag &= 4294967023U;
        obsObjectWork.move_flag |= 128U;
        body_work.flag &= 4294966783U;
        AppMain.GmBoss5EfctEndJetSmoke(body_work);
        AppMain.gmBoss5RestoreCameraLift(body_work.mgr_work);
        AppMain.GmBoss5EfctEndJet(body_work);
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateCrashWithPrep(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.gmBoss5BodyDecideCrashFallPosX(body_work);
        AppMain.gmBoss5BodySetActionWhole(body_work, 27);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateCrashWithStartFly);
    }

    private static void gmBoss5BodyStateUpdateCrashWithStartFly(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.gmBoss5BodySetActionWhole(body_work, 28);
        AppMain.gmBoss5BodyInitCrashFlyUp(body_work);
        AppMain.gmBoss5SetCameraLift(body_work.mgr_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateCrashWithFlyUp);
    }

    private static void gmBoss5BodyStateUpdateCrashWithFlyUp(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.gmBoss5BodyCheckJetSmokeClearTiming(body_work) != 0)
            AppMain.GmBoss5EfctEndJetSmoke(body_work);
        if (AppMain.gmBoss5BodyUpdateCrashFlyUp(body_work) == 0)
            return;
        body_work.mgr_work.flag |= 67108864U;
        AppMain.GmBoss5EfctEndJetSmoke(body_work);
        AppMain.GmBoss5EfctEndJet(body_work);
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_CRASH_WAIT_TIME;
        AppMain.GmBoss5EfctCrashCursorInit(body_work, AppMain.gmBoss5BodyGetCrashFallPosX(body_work), AppMain.GMD_BOSS5_BODY_CRASH_WAIT_TIME - AppMain.GMD_BOSS5_BODY_CRASH_CURSOR_SPAWN_TIME_TRHESHOLD);
        body_work.flag |= 2U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateCrashWithWait);
    }

    private static void gmBoss5BodyStateUpdateCrashWithWait(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.GMM_BS_OBJ((object)body_work).pos.x = AppMain.gmBoss5BodyGetCrashFallPosX(body_work);
            AppMain.gmBoss5BodySetDirection(body_work, 0);
            AppMain.gmBoss5BodySetActionWhole(body_work, 29);
            AppMain.gmBoss5BodyInitCrashFall(body_work);
            AppMain.gmBoss5BodyChangeRectSetting(body_work, 5);
            AppMain.GmSoundPlaySE("FinalBoss17");
            AppMain.gmBoss5RestoreCameraLift(body_work.mgr_work);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateCrashWithFall);
        }
    }

    private static void gmBoss5BodyStateUpdateCrashWithFall(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.gmBoss5BodyUpdateCrashFall(body_work) == 0)
            return;
        body_work.flag &= 4294967293U;
        obsObjectWork.move_flag &= 4294967279U;
        AppMain.gmBoss5BodySetActionWhole(body_work, 30);
        AppMain.GmBoss5EfctCreateStrikeShockwave(body_work, AppMain.GMD_BOSS5_BODY_CRASH_STRIKE_SW_CREATE_DELAY);
        AppMain.gmBoss5DelayedVibration(5, AppMain.GMD_BOSS5_BODY_CRASH_STRIKE_VIB_START_DELAY);
        AppMain.gmBoss5BodyInitCrashStrikeVib(body_work, AppMain.GMD_BOSS5_BODY_CRASH_STRIKE_BODY_VIB_START_DELAY);
        AppMain.gmBoss5BodyChangeRectSetting(body_work, 6);
        AppMain.gmBoss5BodyForceEndLeakage(body_work);
        AppMain.GmBoss5EfctCreateLandingShockwave(body_work);
        AppMain.GmBoss5EfctCreateCrashLandingSmoke(body_work);
        AppMain.gmBoss5BodyTryImmobilizePlayer(body_work);
        AppMain.gmBoss5Vibration(4);
        AppMain.GmPadVibSet(1, -1f, (ushort)32768, (ushort)32768, -1f, 0.0f, 0.0f, 32768U);
        AppMain.gmBoss5DelayedSePlayback("FinalBoss18", AppMain.GMD_BOSS5_BODY_CRASH_STRIKE_SE_START_DELAY);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateCrashWithLand);
    }

    private static void gmBoss5BodyStateUpdateCrashWithLand(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodyUpdateCrashStrikeVib(body_work);
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        body_work.mgr_work.flag |= 536870912U;
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_CRASH_LANDED_IDLE_TIME;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateCrashWithIdle);
    }

    private static void gmBoss5BodyStateUpdateCrashWithIdle(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss5BodyUpdateCrashStrikeVib(body_work);
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.GMM_PAD_VIB_STOP();
            AppMain.gmBoss5BodySetActionWhole(body_work, 31);
            AppMain.gmBoss5BodyInitCrashSink(body_work);
            AppMain.gmBoss5BodyChangeRectSetting(body_work, 7);
            obsObjectWork.flag |= 2U;
            body_work.flag |= 8192U;
            body_work.flag |= 131072U;
            body_work.mgr_work.flag |= 1073741824U;
            AppMain.GmGmkCamScrLimitRelease((byte)8);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateCrashWithSink);
        }
    }

    private static void gmBoss5BodyStateUpdateCrashWithSink(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodyUpdateCrashSink(body_work);
    }

    private static void gmBoss5BodyStateEnterRpc(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodySetActionWhole(body_work, 32);
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateRpcWithPrep);
    }

    private static void gmBoss5BodyStateLeaveRpc(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 2281701375U;
        body_work.flag &= 4294967283U;
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateRpcWithPrep(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        body_work.flag |= 4U;
        if (body_work.state == 9)
            AppMain.GmBoss5RocketLaunchStrong(body_work, 0);
        else
            AppMain.GmBoss5RocketLaunchNormal(body_work, 0);
        body_work.wait_timer = body_work.state != 9 ? AppMain.gmBoss5BodySeqGetRpcNmlSearchTime(body_work) : AppMain.gmBoss5BodySeqGetRpcStrSearchTime(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateRpcWithSearch);
    }

    private static void gmBoss5BodyStateUpdateRpcWithSearch(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5BodySetActionWhole(body_work, 33);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateRpcWithLaunchFirst);
        }
    }

    private static void gmBoss5BodyStateUpdateRpcWithLaunchFirst(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if ((double)AppMain.GMM_BS_OBJ((object)body_work).obj_3d.frame[0] < (double)AppMain.GMD_BOSS5_BODY_RPUNCH_LAUNCH_TIMING_DELAY)
            return;
        body_work.flag |= 268435456U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateRpcWithWaitReturnFirst);
    }

    private static void gmBoss5BodyStateUpdateRpcWithWaitReturnFirst(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        int num = 0;
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) != 0)
            num = 1;
        if (((int)body_work.flag & 67108864) != 0)
        {
            body_work.flag &= 4227858431U;
            num = 1;
            body_work.flag &= 4294967291U;
            AppMain.gmBoss5BodySetActionWhole(body_work, 34);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateRpcWithLaunchSecond);
        }
        if (num == 0 || ((int)body_work.flag & 8) != 0)
            return;
        body_work.flag |= 8U;
        if (body_work.state == 9)
            AppMain.GmBoss5RocketLaunchStrong(body_work, 1);
        else
            AppMain.GmBoss5RocketLaunchNormal(body_work, 1);
    }

    private static void gmBoss5BodyStateUpdateRpcWithLaunchSecond(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if ((double)AppMain.GMM_BS_OBJ((object)body_work).obj_3d.frame[0] < (double)AppMain.GMD_BOSS5_BODY_RPUNCH_LAUNCH_TIMING_DELAY)
            return;
        body_work.flag |= 134217728U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateRpcWithWaitReturnSecond);
    }

    private static void gmBoss5BodyStateUpdateRpcWithWaitReturnSecond(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 33554432) == 0)
            return;
        body_work.flag &= 4261412863U;
        body_work.flag &= 4294967287U;
        AppMain.gmBoss5BodySetActionWhole(body_work, 35);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateRpcWithRecover);
    }

    private static void gmBoss5BodyStateUpdateRpcWithRecover(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        if (body_work.state == 9)
            AppMain.gmBoss5BodyProceedToNextSeqStr(body_work);
        else
            AppMain.gmBoss5BodyProceedToNextSeqNml(body_work);
    }

    private static void gmBoss5BodyStateEnterBerserk(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodySetActionWhole(body_work, 39);
        AppMain.gmBoss5BodyChangeRectSetting(body_work, 9);
        AppMain.GmBoss5EfctBreakdownSmokesInit(body_work, AppMain.GMD_BOSS5_BODY_BERSERK_BREAKDOWN_TIME);
        AppMain.GmBoss5EfctBodySmallSmokesInit(body_work);
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_BERSERK_BREAKDOWN_TIME;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateBerserkWithBreakdown);
    }

    private static void gmBoss5BodyStateLeaveBerserk(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodyClearBerserkTurn(body_work);
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
    }

    private static void gmBoss5BodyStateUpdateBerserkWithBreakdown(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5BodySetActionWhole(body_work, 40);
            AppMain.gmBoss5BodyInitShakeAccelerate(body_work);
            AppMain.GmBoss5EfctStartPrelimLeakage(body_work);
            body_work.wait_timer = AppMain.GMD_BOSS5_BODY_BERSERK_SHAKE_STAY_TIME;
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateBerserkWithShake);
        }
    }

    private static void gmBoss5BodyStateUpdateBerserkWithShake(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.gmBoss5BodyUpdateShakeAccelerate(body_work) == 0)
            return;
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5InitAlarmFade(body_work.mgr_work);
            AppMain.gmBoss5MgrSetAlarmLevel(body_work.mgr_work, 0);
            AppMain.GmDecoStartEffectFinalBossLight();
            AppMain.gmBoss5BodySetActionWhole(body_work, 41);
            AppMain.GmSoundPlaySE("FinalBoss08");
            AppMain.gmBoss5BodyInitBerserkTurn(body_work, 0);
            AppMain.GmBoss5EfctEndPrelimLeakage(body_work);
            AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateBerserkWithTurnFront);
        }
    }

    private static void gmBoss5BodyStateUpdateBerserkWithTurnFront(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.gmBoss5BodyUpdateBerserkTurn(body_work) == 0)
            return;
        AppMain.GmBoss5EfctBerserkSteamInit(body_work, 1U);
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_BERSERK_ROAR_PREP_TIME;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateBerserkWithRoarPrep);
    }

    private static void gmBoss5BodyStateUpdateBerserkWithRoarPrep(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5BodySetActionWhole(body_work, 42);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateBerserkWithRoarStart);
        }
    }

    private static void gmBoss5BodyStateUpdateBerserkWithRoarStart(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.GmBoss5EfctBerserkSteamInit(body_work, 1U);
        AppMain.gmBoss5BodySetActionWhole(body_work, 43);
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_BERSERK_ROAR_LOOP_TIME;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateBerserkWithRoarLoop);
    }

    private static void gmBoss5BodyStateUpdateBerserkWithRoarLoop(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5BodySetActionWhole(body_work, 44);
            AppMain.GmBoss5EfctCreateBerserkStampSmoke(body_work, 0, AppMain.GMD_BOSS5_BODY_BERSERK_STAMP_SMOKE_CREATE_DELAY);
            AppMain.gmBoss5DelayedVibration(2, AppMain.GMD_BOSS5_BODY_BERSERK_STAMP_VIB_START_DELAY);
            AppMain.gmBoss5DelayedSePlayback("FinalBoss03", AppMain.GMD_BOSS5_BODY_BERSERK_STAMP_SE_START_DELAY);
            AppMain.gmBoss5BodyInitBerserkTurn(body_work, 1);
            AppMain.gmBoss5BodyInitGroundingMove(body_work, body_work.rfoot_snm_reg_id);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateBerserkWithTurnSide);
        }
    }

    private static void gmBoss5BodyStateUpdateBerserkWithTurnSide(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodyUpdateGroundingMove(body_work);
        if (AppMain.gmBoss5BodyUpdateBerserkTurn(body_work) == 0)
            return;
        AppMain.gmBoss5BodyClearBerserkTurn(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateBerserkWithStamp);
    }

    private static void gmBoss5BodyStateUpdateBerserkWithStamp(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss5BodyUpdateGroundingMove(body_work);
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        AppMain.gmBoss5BodySetActionWhole(body_work, 45);
        AppMain.gmBoss5BodyInitGroundingMove(body_work, body_work.lfoot_snm_reg_id);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateBerserkWithKickUp);
    }

    private static void gmBoss5BodyStateUpdateBerserkWithKickUp(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss5BodyUpdateGroundingMove(body_work);
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        AppMain.gmBoss5BodyProceedToNextSeqStr(body_work);
    }

    private static void gmBoss5BodyStateEnterDefeat(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.mgr_work.flag |= 134217728U;
        AppMain.GmPlayerAddScoreNoDisp((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), 1000);
        body_work.flag |= 512U;
        body_work.flag |= 8192U;
        AppMain.gmBoss5BodyChangeRectSetting(body_work, 10);
        AppMain.gmBoss5InitExplCreate(body_work.expl_work, 1, body_work.part_obj_core, AppMain.GMD_BOSS5_EXPL_BODY_OFST_X, AppMain.GMD_BOSS5_EXPL_BODY_OFST_Y, AppMain.GMD_BOSS5_EXPL_BODY_WIDTH, AppMain.GMD_BOSS5_EXPL_BODY_HEIGHT, AppMain.GMD_BOSS5_EXPL_BODY_INTERVAL_MIN, AppMain.GMD_BOSS5_EXPL_BODY_INTERVAL_MAX, AppMain.GMD_BOSS5_EXPL_BODY_SE_FREQUENCY);
        body_work.wait_timer = AppMain.GMD_BOSS5_BODY_DEFEAT_WAIT_START_TIME;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateDefeatWithWaitStart);
    }

    private static void gmBoss5BodyStateLeaveDefeat(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294959103U;
        body_work.flag &= 4294966783U;
    }

    private static void gmBoss5BodyStateUpdateDefeatWithWaitStart(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5UpdateExplCreate(body_work.expl_work);
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5BodyRegisterScatterPartsCNM(body_work);
            AppMain.gmBoss5InitScatter(body_work);
            body_work.wait_timer = AppMain.MTM_MATH_MAX(80U, 90U);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodyStateUpdateDefeatWithExplode);
        }
    }

    private static void gmBoss5BodyStateUpdateDefeatWithExplode(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        int num = 0;
        AppMain.gmBoss5UpdateExplCreate(body_work.expl_work);
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else if (((int)body_work.flag & 524288) == 0)
        {
            AppMain.gmBoss5BodyInitScatterFall(body_work);
            body_work.flag |= 524288U;
        }
        else
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
            if (AppMain.gmBoss5BodyUpdateScatterFall(body_work) != 0)
                num = 1;
            if (((int)obsObjectWork.move_flag & 1) != 0 && ((int)body_work.flag & 2097152) == 0)
            {
                AppMain.GmBoss5EfctCreateBigExplosion(body_work.part_obj_core.pos.x, body_work.part_obj_core.pos.y, body_work.part_obj_core.pos.z + AppMain.GMD_BOSS5_EXPL_OFST_Z);
                body_work.flag |= 2097152U;
                AppMain.GmSoundPlaySE("Boss0_03");
                AppMain.gmBoss5InitFlashScreen();
                AppMain.GMM_PAD_VIB_MID_TIME(120f);
            }
        }
        if (AppMain.gmBoss5BodyIsBodyExplosionStopAllowed(body_work) == 0 || num == 0)
            return;
        body_work.proc_update = (AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK)null;
        body_work.mgr_work.flag |= 268435456U;
    }

    private static void gmBoss5BodySubSeqEnterMoveFastCrash(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.sub_seq == 0)
            AppMain.gmBoss5BodySetActionWhole(body_work, 18);
        else
            AppMain.gmBoss5BodySetActionWhole(body_work, 19);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodySubSeqUpdateMoveFastCrashWithStagger);
    }

    private static void gmBoss5BodySubSeqUpdateMoveFastCrashWithStagger(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (((int)obj_work.move_flag & 1) == 0 || AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        AppMain.gmBoss5BodyResumeMoveFast(body_work);
    }

    private static void gmBoss5BodySubSeqEnterRpcStrDmg(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodySetActionWhole(body_work, 36);
        AppMain.gmBoss5BodyChangeRectSetting(body_work, 8);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodySubSeqUpdateRpcStrDmgWithBend);
    }

    private static void gmBoss5BodySubSeqUpdateRpcStrDmgWithBend(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.gmBoss5BodySetActionWhole(body_work, 37);
        body_work.wait_timer = 240U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodySubSeqUpdateRpcStrDmgWithSwing);
    }

    private static void gmBoss5BodySubSeqUpdateRpcStrDmgWithSwing(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            body_work.flag |= 1610612736U;
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodySubSeqUpdateRpcStrDmgWithWaitReturn);
        }
    }

    private static void gmBoss5BodySubSeqUpdateRpcStrDmgWithWaitReturn(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.gmBoss5BodyReceiveSignalRocketReturned(body_work) == 0)
            return;
        body_work.flag &= 4294967283U;
        AppMain.gmBoss5BodySetActionWhole(body_work, 38);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_BODY_WORK(AppMain.gmBoss5BodySubSeqUpdateRpcStrDmgWithRecover);
    }

    private static void gmBoss5BodySubSeqUpdateRpcStrDmgWithRecover(
      AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
            return;
        AppMain.gmBoss5BodyChangeRectSettingDefault(body_work);
        AppMain.gmBoss5BodyProceedToNextSeqStr(body_work);
    }

    private static void gmBoss5CoreWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS5_CORE_WORK core_work = (AppMain.GMS_BOSS5_CORE_WORK)obj_work;
        if (((int)parentObj.mgr_work.flag & 1) == 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5CoreMain);
        AppMain.gmBoss5CoreProcInit(core_work);
    }

    private static void gmBoss5CoreMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_CORE_WORK wrk = (AppMain.GMS_BOSS5_CORE_WORK)obj_work;
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    private static void gmBoss5CoreProcInit(AppMain.GMS_BOSS5_CORE_WORK core_work)
    {
        core_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_CORE_WORK(AppMain.gmBoss5CoreProcUpdateLoop);
    }

    private static void gmBoss5CoreProcUpdateLoop(AppMain.GMS_BOSS5_CORE_WORK core_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)core_work);
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObjectGeneralStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        AppMain.gmBoss5BodyUpdateMainRectPosition(parentObj);
        AppMain.gmBoss5BodyUpdateSubRectPosition(parentObj);
        if (((int)parentObj.flag & 8192) != 0)
            ((AppMain.GMS_ENEMY_COM_WORK)obj_work).enemy_flag |= 32768U;
        else
            ((AppMain.GMS_ENEMY_COM_WORK)obj_work).enemy_flag &= 4294934527U;
    }

    private static void gmBoss5InitAlarmFade(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        if (((int)mgr_work.flag & 32) != 0)
            return;
        mgr_work.flag |= 32U;
        AppMain.GMS_BOSS5_ALARM_FADE_WORK fadeObj = (AppMain.GMS_BOSS5_ALARM_FADE_WORK)AppMain.GmFadeCreateFadeObj((ushort)6656, (byte)3, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_ALARM_FADE_WORK()), (ushort)61439, 0U);
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)fadeObj;
        obsObjectWork.parent_obj = AppMain.GMM_BS_OBJ((object)mgr_work);
        fadeObj.mgr_work = mgr_work;
        AppMain.GmFadeSetFade(fadeObj.fade_obj, 0U, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, 1f, 0, 0);
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5AlarmFadeMain);
        AppMain.gmBoss5AlarmFadeProcInit(fadeObj);
    }

    private static void gmBoss5RequestClearAlarmFade(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        mgr_work.flag &= 4294967263U;
    }

    private static void gmBoss5AlarmFadeInitFade(
      AppMain.GMS_BOSS5_ALARM_FADE_WORK alarm_fade,
      int alarm_level)
    {
        alarm_fade.cur_phase = 0;
        alarm_fade.cur_level = alarm_level;
        alarm_fade.wait_timer = 0U;
        AppMain.gmBoss5AlarmFadeUpdateFade(alarm_fade);
    }

    private static int gmBoss5AlarmFadeUpdateFade(AppMain.GMS_BOSS5_ALARM_FADE_WORK alarm_fade)
    {
        if (alarm_fade.wait_timer != 0U)
        {
            --alarm_fade.wait_timer;
        }
        else
        {
            if (AppMain.GmFadeIsEnd(alarm_fade.fade_obj) == 0)
                return 0;
            AppMain.GMS_BOSS5_ALARM_FADE_INFO bosS5AlarmFadeInfo = AppMain.gm_boss5_alarm_fade_info[alarm_fade.cur_level];
            switch (alarm_fade.cur_phase)
            {
                case 0:
                    AppMain.GmFadeSetFade(alarm_fade.fade_obj, 0U, byte.MaxValue, (byte)0, (byte)0, (byte)0, AppMain.GMD_BOSS5_ALARM_FADE_DEST_RED, AppMain.GMD_BOSS5_ALARM_FADE_DEST_GREEN, AppMain.GMD_BOSS5_ALARM_FADE_DEST_BLUE, AppMain.GMD_BOSS5_ALARM_FADE_DEST_ALPHA, (float)bosS5AlarmFadeInfo.fo_frame, 0, 0);
                    alarm_fade.cur_phase = 1;
                    break;
                case 1:
                    alarm_fade.wait_timer = bosS5AlarmFadeInfo.on_frame;
                    alarm_fade.cur_phase = 2;
                    break;
                case 2:
                    AppMain.GmFadeSetFade(alarm_fade.fade_obj, 0U, AppMain.GMD_BOSS5_ALARM_FADE_DEST_RED, AppMain.GMD_BOSS5_ALARM_FADE_DEST_GREEN, AppMain.GMD_BOSS5_ALARM_FADE_DEST_BLUE, AppMain.GMD_BOSS5_ALARM_FADE_DEST_ALPHA, byte.MaxValue, (byte)0, (byte)0, (byte)0, (float)bosS5AlarmFadeInfo.fi_frame, 0, 1);
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

    private static void gmBoss5AlarmFadeInitAlertSe(AppMain.GMS_BOSS5_ALARM_FADE_WORK alarm_fade)
    {
        AppMain.GMS_BOSS5_MGR_WORK mgrWork = alarm_fade.mgr_work;
        alarm_fade.alert_se_ref_level = mgrWork.alarm_level;
        AppMain.GmBoss5Init1ShotTimer(alarm_fade.alert_se_timer, AppMain.gm_boss5_alarm_se_interval_time_tbl[alarm_fade.alert_se_ref_level]);
        AppMain.GmSoundPlaySE("FinalBoss10");
    }

    private static void gmBoss5AlarmFadeUpdateAlertSe(AppMain.GMS_BOSS5_ALARM_FADE_WORK alarm_fade)
    {
        AppMain.GMS_BOSS5_MGR_WORK mgrWork = alarm_fade.mgr_work;
        if (AppMain.GmBoss5Update1ShotTimer(alarm_fade.alert_se_timer) == 0 && alarm_fade.alert_se_ref_level == mgrWork.alarm_level)
            return;
        AppMain.GmSoundPlaySE("FinalBoss10");
        alarm_fade.alert_se_ref_level = mgrWork.alarm_level;
        AppMain.GmBoss5Init1ShotTimer(alarm_fade.alert_se_timer, AppMain.gm_boss5_alarm_se_interval_time_tbl[alarm_fade.alert_se_ref_level]);
    }

    private static void gmBoss5AlarmFadeMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_ALARM_FADE_WORK wrk = (AppMain.GMS_BOSS5_ALARM_FADE_WORK)obj_work;
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

    private static void gmBoss5AlarmFadeProcInit(AppMain.GMS_BOSS5_ALARM_FADE_WORK alarm_fade)
    {
        AppMain.GMS_BOSS5_MGR_WORK mgrWork = alarm_fade.mgr_work;
        AppMain.gmBoss5AlarmFadeInitFade(alarm_fade, mgrWork.alarm_level);
        AppMain.gmBoss5AlarmFadeInitAlertSe(alarm_fade);
        alarm_fade.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ALARM_FADE_WORK(AppMain.gmBoss5AlarmFadeProcUpdateLoop);
    }

    private static void gmBoss5AlarmFadeProcUpdateLoop(AppMain.GMS_BOSS5_ALARM_FADE_WORK alarm_fade)
    {
        AppMain.GMS_BOSS5_MGR_WORK mgrWork = alarm_fade.mgr_work;
        AppMain.gmBoss5AlarmFadeUpdateAlertSe(alarm_fade);
        if (AppMain.gmBoss5AlarmFadeUpdateFade(alarm_fade) == 0)
            return;
        AppMain.gmBoss5AlarmFadeInitFade(alarm_fade, mgrWork.alarm_level);
    }

    private static void gmBoss5InitFlashScreen()
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_FLASH_SCREEN_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "boss5_flash_scr");
        AppMain.GMS_BOSS5_FLASH_SCREEN_WORK s5FlashScreenWork = (AppMain.GMS_BOSS5_FLASH_SCREEN_WORK)work;
        work.disp_flag |= 4128U;
        work.flag |= 16U;
        AppMain.GmBsCmnInitFlashScreen(s5FlashScreenWork.flash_work, (float)AppMain.GMD_BOSS5_FLASH_SCREEN_FADEOUT_TIME, 5f, 30f);
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5FlashScreenMain);
    }

    private static void gmBoss5FlashScreenMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_FLASH_SCREEN_WORK s5FlashScreenWork = (AppMain.GMS_BOSS5_FLASH_SCREEN_WORK)obj_work;
        if (AppMain.GmBsCmnUpdateFlashScreen(s5FlashScreenWork.flash_work) == 0)
            return;
        AppMain.GmBsCmnClearFlashScreen(s5FlashScreenWork.flash_work);
        obj_work.flag |= 4U;
    }

    private static void gmBoss5InitLastFadeOut(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        AppMain.GMS_FADE_OBJ_WORK fadeObj = AppMain.GmFadeCreateFadeObj((ushort)6656, (byte)3, (byte)0, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_FADE_OBJ_WORK()), (ushort)61439, 6U);
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)fadeObj;
        obsObjectWork.parent_obj = AppMain.GMM_BS_OBJ((object)mgr_work);
        AppMain.GmFadeSetFade(fadeObj, 0U, (byte)0, (byte)0, (byte)0, (byte)0, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, 300f, 0, 0);
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5LastFadeOutMain);
    }

    private static void gmBoss5LastFadeOutMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_FADE_OBJ_WORK fade_obj = (AppMain.GMS_FADE_OBJ_WORK)obj_work;
        AppMain.GMS_BOSS5_MGR_WORK parentObj = (AppMain.GMS_BOSS5_MGR_WORK)obj_work.parent_obj;
        if (AppMain.GmFadeIsEnd(fade_obj) == 0)
            return;
        AppMain.gmBoss5RequestClearAlarmFade(parentObj);
        AppMain.GmFixSetDisp(false);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5LastFadeOutEnd);
    }

    private static void gmBoss5LastFadeOutEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_FADE_OBJ_WORK gmsFadeObjWork = (AppMain.GMS_FADE_OBJ_WORK)obj_work;
        ((AppMain.GMS_BOSS5_MGR_WORK)obj_work.parent_obj).flag |= 2147483648U;
        gmsFadeObjWork.fade_work.draw_state = 0U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmBoss5InitScatter(AppMain.GMS_BOSS5_BODY_WORK body_work)
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
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMakeUnitMatrix(nnsMatrix);
        for (int index = 0; index < 22; ++index)
        {
            AppMain.GMS_BOSS5_SCT_PART_INFO bosS5SctPartInfo = AppMain.gm_boss5_scatter_parts_info_tbl[index];
            AppMain.GmBsCmnChangeCNMModeNode(body_work.cnm_mgr_work, body_work.scatter_cnm_reg_ids[index], (uint)bosS5SctPartInfo.cnm_mode);
            AppMain.GmBsCmnEnableCNMLocalCoordinate(body_work.cnm_mgr_work, body_work.scatter_cnm_reg_ids[index], bosS5SctPartInfo.is_local_coord);
            AppMain.GmBsCmnEnableCNMInheritNodeScale(body_work.cnm_mgr_work, body_work.scatter_cnm_reg_ids[index], bosS5SctPartInfo.is_inherit_scale);
            AppMain.GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix, body_work.scatter_cnm_reg_ids[index], 1);
        }
        for (int index = 0; index < 6; ++index)
        {
            AppMain.GMS_BOSS5_SCT_NDC_INFO gmsBosS5SctNdcInfo = AppMain.gm_boss5_scatter_ndc_info_tbl[index];
            int partIdx = gmsBosS5SctNdcInfo.part_idx;
            AppMain.GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, body_work.scatter_cnm_reg_ids[partIdx], 0);
            AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT controlObjectBySize = AppMain.GmBsCmnCreateNodeControlObjectBySize(AppMain.GMM_BS_OBJ((object)body_work), body_work.cnm_mgr_work, body_work.scatter_cnm_reg_ids[partIdx], body_work.snm_work, numArray[index], (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_SCT_PART_NDC_WORK()));
            AppMain.GMS_BOSS5_SCT_PART_NDC_WORK sct_part_ndc = (AppMain.GMS_BOSS5_SCT_PART_NDC_WORK)controlObjectBySize;
            controlObjectBySize.user_timer = gmsBosS5SctNdcInfo.delay_time;
            controlObjectBySize.is_enable = 0;
            AppMain.gmBoss5ScatterSetPartParam(sct_part_ndc);
            AppMain.GMM_BS_OBJ((object)controlObjectBySize).move_flag |= 4608U;
            AppMain.nnMakeUnitQuaternion(ref controlObjectBySize.user_quat);
            controlObjectBySize.proc_update = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5ScatterProcWait);
        }
        body_work.flag |= 262144U;
    }

    private static void gmBoss5ScatterSetPartParam(AppMain.GMS_BOSS5_SCT_PART_NDC_WORK sct_part_ndc)
    {
        AppMain.nnMakeUnitQuaternion(ref sct_part_ndc.spin_quat);
        for (int index = 0; index < 2; ++index)
        {
            AppMain.NNS_VECTOR dst_vec = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            float rand_z = AppMain.MTM_MATH_CLIP((float)((double)AppMain.FX_FX32_TO_F32(AppMain.AkMathRandFx()) * 2.0 - 1.0), -1f, 1f);
            short rand_angle = AppMain.AKM_DEGtoA16(360f * AppMain.FX_FX32_TO_F32(AppMain.AkMathRandFx()));
            AppMain.AkMathGetRandomUnitVector(dst_vec, rand_z, rand_angle);
            AppMain.NNS_QUATERNION dst;
            AppMain.nnMakeRotateAxisQuaternion(out dst, dst_vec.x, dst_vec.y, dst_vec.z, AppMain.GMD_BOSS5_SCT_SPIN_SPD_ANGLE);
            AppMain.nnMultiplyQuaternion(ref sct_part_ndc.spin_quat, ref dst, ref sct_part_ndc.spin_quat);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(dst_vec);
        }
    }

    private static void gmBoss5ScatterProcWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj = (AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work;
        if (ndc_obj.user_timer != 0U)
        {
            --ndc_obj.user_timer;
        }
        else
        {
            AppMain.GmBsCmnAttachNCObjectToSNMNode(ndc_obj);
            AppMain.GmBoss5ScatterSetFlyParam(obj_work);
            ndc_obj.is_enable = 1;
            ndc_obj.user_timer = 180U;
            AppMain.GmBoss5EfctCreateSmallExplosion(obj_work.pos.x, obj_work.pos.y, obj_work.pos.z + AppMain.GMD_BOSS5_EXPL_OFST_Z);
            ndc_obj.proc_update = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5ScatterProcFly);
        }
    }

    private static void gmBoss5ScatterProcFly(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj = (AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work;
        AppMain.GMS_BOSS5_SCT_PART_NDC_WORK s5SctPartNdcWork = (AppMain.GMS_BOSS5_SCT_PART_NDC_WORK)ndc_obj;
        AppMain.nnMultiplyQuaternion(ref ndc_obj.user_quat, ref s5SctPartNdcWork.spin_quat, ref ndc_obj.user_quat);
        AppMain.GmBsCmnSetWorldMtxFromNCObjectPosture(ndc_obj);
        if (ndc_obj.user_timer != 0U)
            --ndc_obj.user_timer;
        else
            obj_work.flag |= 4U;
    }

    private static void gmBoss5BodySeqTryRequestEnableStr(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (body_work.mgr_work.life > AppMain.GMD_BOSS5_STRONG_MODE_THRESHOLD_LIFE)
            return;
        body_work.mgr_work.flag |= 4U;
    }

    private static void gmBoss5BodySeqTryEnableStr(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.mgr_work.flag & 4) == 0)
            return;
        body_work.mgr_work.flag |= 8U;
    }

    private static int gmBoss5BodySeqIsStr(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        return ((int)body_work.mgr_work.flag & 8) != 0 ? 1 : 0;
    }

    private static int gmBoss5BodySeqIsNearDeath(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        return body_work.mgr_work.life <= 1 ? 1 : 0;
    }

    private static int gmBoss5BodySeqLotStrat(int strat_branch)
    {
        return AppMain.gmBoss5BodySeqLotStrat(strat_branch, 0);
    }

    private static int gmBoss5BodySeqLotStrat(int strat_branch, int b_no_rkt)
    {
        AppMain.GMS_BOSS5_STRAT_PROB_INFO[] bosS5StratProbInfoArray = AppMain.gm_boss5_body_seq_strat_branch_prob_tbl[strat_branch];
        int index1 = 0;
        int num1 = 0;
        int v1 = AppMain.AkMathRandFx();
        if (b_no_rkt != 0)
        {
            int num2 = 4096;
            for (int index2 = 0; index2 < 3; ++index2)
            {
                if (bosS5StratProbInfoArray[index2].is_rkt != 0)
                    num2 = AppMain.MTM_MATH_CLIP(num2 - bosS5StratProbInfoArray[index2].probability, 0, 4096);
            }
            v1 = AppMain.MTM_MATH_CLIP(AppMain.FX_Mul(v1, num2), 0, num2);
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

    private static void gmBoss5BodyProceedToNextSeqNml(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        int state1 = body_work.state;
        int strat_state = 0;
        AppMain.gmBoss5BodySeqTryEnableStr(body_work);
        if (AppMain.gmBoss5BodySeqIsStr(body_work) != 0)
        {
            strat_state = 8;
        }
        else
        {
            int b_no_rkt = 0;
            if (AppMain.gmBoss5BodyIsPlayerBehind(body_work) != 0)
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
                    strat_state = AppMain.gmBoss5BodySeqLotStrat(0, b_no_rkt);
                    break;
                case 6:
                    strat_state = 5;
                    break;
                case 7:
                    strat_state = AppMain.gmBoss5BodySeqLotStrat(1, b_no_rkt);
                    break;
            }
        }
        int state2 = AppMain.gm_boss5_body_state_strat_tbl[strat_state];
        AppMain.gmBoss5BodyChangeState(body_work, state2, strat_state, 1);
    }

    private static void gmBoss5BodyProceedToNextSeqStr(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        int state1 = body_work.state;
        int strat_state = 0;
        if (AppMain.gmBoss5BodySeqIsNearDeath(body_work) != 0)
        {
            strat_state = 16;
        }
        else
        {
            int b_no_rkt = 0;
            if (AppMain.gmBoss5BodyIsPlayerBehind(body_work) != 0)
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
                    strat_state = AppMain.gmBoss5BodySeqLotStrat(2, b_no_rkt);
                    break;
                case 14:
                    strat_state = AppMain.gmBoss5BodySeqLotStrat(3, b_no_rkt);
                    break;
                case 15:
                    strat_state = 12;
                    break;
            }
        }
        int state2 = AppMain.gm_boss5_body_state_strat_tbl[strat_state];
        if (state2 == 3)
            AppMain.gmBoss5BodySetMoveFastTime(body_work, 360U);
        else
            AppMain.gmBoss5BodySetMoveFastTime(body_work, 0U);
        AppMain.gmBoss5BodyChangeState(body_work, state2, strat_state, 1);
    }

    private static void gmBoss5BodyProceedToDefeatState(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5BodyChangeState(body_work, 11, 0, 1);
    }

    private static uint gmBoss5BodySeqGetRpcNmlSearchTime(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
        return AppMain.AkMathRandFx() < AppMain.GMD_BOSS5_BODY_SEQ_RPUNCH_NML_FASTSHOT_PROB ? AppMain.GMD_BOSS5_BODY_SEQ_RPUNCH_NML_SEARCH_TIME_SHORT : AppMain.GMD_BOSS5_BODY_SEQ_RPUNCH_NML_SEARCH_TIME_NORMAL;
    }

    private static uint gmBoss5BodySeqGetRpcStrSearchTime(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
        return (long)AppMain.AkMathRandFx() < (long)AppMain.GMD_BOSS5_BODY_SEQ_RPUNCH_STR_FASTSHOT_PROB ? AppMain.GMD_BOSS5_BODY_SEQ_RPUNCH_STR_SEARCH_TIME_SHORT : AppMain.GMD_BOSS5_BODY_SEQ_RPUNCH_STR_SEARCH_TIME_NORMAL;
    }

}