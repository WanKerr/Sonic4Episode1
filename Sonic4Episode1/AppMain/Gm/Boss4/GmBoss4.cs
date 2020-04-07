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
    public static AppMain.AMS_AMB_HEADER GMD_BOSS4_ARC
    {
        get
        {
            return AppMain.g_gm_gamedat_enemy_arc;
        }
    }

    public static T GMM_BOSS4_STAGE<T>(T s4, T sf)
    {
        return !AppMain.GmBoss4CheckBossRush() ? s4 : sf;
    }

    public static int GMD_BOSS4_SCROLL_INIT_X
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<int>(2500, 11000);
        }
    }

    public static int GMD_BOSS4_SCROLL_START_X
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<int>(2500, 11000);
        }
    }

    public static int GMD_BOSS4_SCROLL_END_X
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<int>(3972, 12536);
        }
    }

    public static int GMD_BOSS4_SCROLL_OUT_X
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<int>(2500, 11000);
        }
    }

    public static int GMD_BOSS4_LIFE
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<int>(8, 4);
        }
    }

    public static int GMD_BOSS4_EXTRA_ATK_THRESHOLD_LIFE
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<int>(3, 4);
        }
    }

    public static int GMM_BOSS4_AREA_LEFT()
    {
        return AppMain.g_gm_main_system.map_fcol.left << 12;
    }

    public static int GMM_BOSS4_AREA_TOP()
    {
        return AppMain.g_gm_main_system.map_fcol.top << 12;
    }

    public static int GMM_BOSS4_AREA_RIGHT()
    {
        return AppMain.g_gm_main_system.map_fcol.right << 12;
    }

    public static int GMM_BOSS4_AREA_BOTTOM()
    {
        return AppMain.g_gm_main_system.map_fcol.bottom << 12;
    }

    public static int GMM_BOSS4_AREA_CENTER_X()
    {
        return AppMain.GMM_BOSS4_AREA_LEFT() + (AppMain.GMM_BOSS4_AREA_RIGHT() - AppMain.GMM_BOSS4_AREA_LEFT()) / 2;
    }

    public static int GMM_BOSS4_AREA_CENTER_Y()
    {
        return AppMain.GMM_BOSS4_AREA_TOP() + (AppMain.GMM_BOSS4_AREA_BOTTOM() - AppMain.GMM_BOSS4_AREA_TOP()) / 2;
    }

    private static AppMain.OBS_OBJECT_WORK GMM_BOSS4_CREATE_WORK(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      AppMain.TaskWorkFactoryDelegate work_size,
      string name)
    {
        return AppMain.GmEnemyCreateWork(eve_rec, pos_x, pos_y, work_size, (ushort)4342, name);
    }

    private static void GmBoss4SetLife(int life)
    {
        AppMain.MTM_ASSERT((object)AppMain.gm_boss4_mgr_work);
        AppMain.gm_boss4_mgr_work.life = life;
    }

    private static int GmBoss4GetLife()
    {
        AppMain.MTM_ASSERT((object)AppMain.gm_boss4_mgr_work);
        return AppMain.gm_boss4_mgr_work == null ? 0 : AppMain.gm_boss4_mgr_work.life;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4GetBodyWork()
    {
        AppMain.MTM_ASSERT((object)AppMain.gm_boss4_mgr_work);
        return AppMain.gm_boss4_mgr_work == null ? (AppMain.OBS_OBJECT_WORK)null : (AppMain.OBS_OBJECT_WORK)AppMain.gm_boss4_mgr_work.body_work;
    }

    private static AppMain.OBS_ACTION3D_NN_WORK GmBoss4GetObj3D(int n)
    {
        return AppMain.gm_boss4_obj_3d_list[n];
    }

    private static AppMain.GMS_BOSS4_PART_ACT_INFO GmBoss4GetActInfo(int act, int parts)
    {
        return AppMain.gm_boss4_act_id_tbl[act][parts];
    }

    private static void GmBoss4Build()
    {
        AppMain.gm_boss4_obj_3d_list = AppMain.GmGameDBuildRegBuildModel((AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, AppMain.GMD_BOSS4_ARC), (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 1, AppMain.GMD_BOSS4_ARC), 0U);
        AppMain.GmBoss4BodyBuild();
        AppMain.GmBoss4EggmanBuild();
        AppMain.GmBoss4CapsuleBuild();
        AppMain.GmBoss4ChibiBuild();
        AppMain.GmBoss4EffectBuild();
    }

    private static void GmBoss4Flush()
    {
        AppMain.GmBoss4EffectFlush();
        AppMain.GmBoss4ChibiFlush();
        AppMain.GmBoss4CapsuleFlush();
        AppMain.GmBoss4EggmanFlush();
        AppMain.GmBoss4BodyFlush();
        AppMain.AMS_AMB_HEADER amsAmbHeader = (AppMain.AMS_AMB_HEADER)AppMain.ObjDataLoadAmbIndex((AppMain.OBS_DATA_WORK)null, 0, AppMain.GMD_BOSS4_ARC);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_boss4_obj_3d_list, amsAmbHeader.file_num);
        AppMain.gm_boss4_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        AppMain.gm_boss4_mgr_work = (AppMain.GMS_BOSS4_MGR_WORK)null;
    }

    private static bool GmBoss4IsBuilded()
    {
        AppMain.MTM_ASSERT((object)AppMain.gm_boss4_mgr_work);
        return 0 != ((int)AppMain.gm_boss4_mgr_work.flag & 1);
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4Init(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_BOSS4_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS4_MGR_WORK()), "Boss4_MGR");
        AppMain.GMS_BOSS4_MGR_WORK gmsBosS4MgrWork = (AppMain.GMS_BOSS4_MGR_WORK)work;
        AppMain.gm_boss4_mgr_work = gmsBosS4MgrWork;
        work.flag |= 16U;
        work.disp_flag |= 32U;
        work.move_flag |= 8448U;
        gmsBosS4MgrWork.life = AppMain.GMD_BOSS4_LIFE;
        work.pos.x = pos_x;
        work.pos.y = pos_y;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4MgrWaitLoad);
        AppMain.GmBoss4ScrollOff();
        AppMain.gm_boss4_is_2nd = false;
        if (AppMain.g_gs_main_sys_info.stage_id != (ushort)16)
        {
            AppMain.GmMapSetAddMapScrlScaleMagX(2, 1);
            AppMain.GmMapSetAddMapScrlScaleMagX(3, 1);
            AppMain.GmMapSetAddMapScrlScaleMagX(4, 1);
            AppMain.GmMapSetAddMapXLoop();
            AppMain.GmMapEnableAddMapUserScrlX();
        }
        AppMain.gm_boss4_n_scroll_start = AppMain.GMD_BOSS4_SCROLL_START_X;
        AppMain.gm_boss4_n_scroll_end = AppMain.GMD_BOSS4_SCROLL_END_X;
        return work;
    }

    private static int GmBoss4GetScrollOffset()
    {
        return AppMain.FX_F32_TO_FX32((float)AppMain.gm_boss4_n_offset_x);
    }

    private static void gmBoss4SetPartTextureBurnt(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmBoss4SetPartTextureBurnt(obj_work, true);
    }

    private static void gmBoss4SetPartTextureBurnt(AppMain.OBS_OBJECT_WORK obj_work, bool burn)
    {
        AppMain.MTM_ASSERT((object)obj_work);
        AppMain.MTM_ASSERT((object)obj_work.obj_3d);
        AppMain.MTM_ASSERT(obj_work.disp_flag & 134217728U);
        obj_work.obj_3d.drawflag |= 268435456U;
        obj_work.obj_3d.draw_state.texoffset[0].mode = 2;
        if (burn)
            obj_work.obj_3d.draw_state.texoffset[0].u = 0.5f;
        else
            obj_work.obj_3d.draw_state.texoffset[0].u = 0.0f;
    }

    private static bool gmBoss4IsScrollLockBusy()
    {
        return ((int)AppMain.g_gm_main_system.game_flag & 32768) != 0;
    }

  

    private static void gmCameraForceScrollFunc(AppMain.OBS_CAMERA obj_cam)
    {
        AppMain.GMS_PLAYER_WORK playerObj1 = (AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj();
        if (((int)AppMain.g_gm_main_system.game_flag & 64) != 0)
            return;
        if ((double)AppMain.gm_boss4_f_scroll_spd <= -100.0 && AppMain.gm_boss4_n_scroll_pt_x == 0)
        {
            AppMain.gm_boss4_f_scroll_spd = (double)obj_cam.pos.x == (double)obj_cam.prev_pos.x ? AppMain.FXM_FX32_TO_FLOAT(playerObj1.obj_work.move.x) : obj_cam.pos.x - obj_cam.prev_pos.x;
            AppMain.gm_boss4_n_scroll_pt_x = (int)((double)obj_cam.pos.x + (double)AppMain.gm_boss4_f_scroll_spd);
        }
        if (AppMain.gm_boss4_n_scroll != 0)
            AppMain.gm_boss4_f_scroll_spd += AppMain.GMD_BOSS4_SCROLL_SPD_ADD;
        else
            AppMain.gm_boss4_f_scroll_spd -= AppMain.GMD_BOSS4_SCROLL_SPD_SUB;
        if ((double)AppMain.gm_boss4_f_scroll_spd > (double)AppMain.gm_boss4_f_scroll_spd_max)
            AppMain.gm_boss4_f_scroll_spd = AppMain.gm_boss4_f_scroll_spd_max;
        if ((double)AppMain.gm_boss4_f_scroll_spd < 0.0)
            AppMain.gm_boss4_f_scroll_spd = 0.0f;
        if (AppMain.gm_boss4_n_scroll == 0 && (double)AppMain.gm_boss4_f_scroll_spd == 0.0)
        {
            AppMain.g_gm_main_system.map_fcol.left = 0;
            AppMain.g_gm_main_system.map_fcol.right = (int)AppMain.g_gm_main_system.map_fcol.map_block_num_x * 64;
            AppMain.ObjCameraSetUserFunc(0, new AppMain.OBJF_CAMERA_USER_FUNC(AppMain.GmCameraFunc));
            AppMain.GmGmkCamScrLimitRelease((byte)14);
            AppMain.GmBoss4UtilPlayerStop(false);
        }
        else
        {
            AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            nnsVector.x = AppMain.FXM_FX32_TO_FLOAT(playerObj1.obj_work.pos.x);
            nnsVector.y = AppMain.FXM_FX32_TO_FLOAT(-playerObj1.obj_work.pos.y + 24576);
            nnsVector.z = AppMain.FXM_FX32_TO_FLOAT(playerObj1.obj_work.pos.z);
            obj_cam.work.x = nnsVector.x;
            obj_cam.work.y = nnsVector.y;
            obj_cam.work.z = nnsVector.z;
            if (AppMain.gm_boss4_n_scroll == 0)
            {
                AppMain.GmBoss4UtilPlayerStop(false);
                int f32 = (int)AppMain.FX_FX32_TO_F32(playerObj1.obj_work.pos.x + playerObj1.obj_work.spd.x);
                AppMain.gm_boss4_n_scroll_pt_x = (double)f32 >= (double)AppMain.gm_boss4_n_scroll_pt_x - (double)obj_cam.allow.x ? ((double)f32 <= (double)AppMain.gm_boss4_n_scroll_pt_x + (double)obj_cam.allow.x ? AppMain.gm_boss4_n_scroll_pt_x : f32 - (int)obj_cam.allow.x) : f32 + (int)obj_cam.allow.x;
            }
            bool flag = false;
            if ((double)obj_cam.pos.x > (double)AppMain.gm_boss4_n_scroll_pt_x)
                flag = true;
            obj_cam.prev_pos.x = obj_cam.pos.x;
            obj_cam.pos.x = (float)AppMain.gm_boss4_n_scroll_pt_x;
            obj_cam.disp_pos.x = (float)AppMain.gm_boss4_n_scroll_pt_x;
            obj_cam.target_pos.x = (float)AppMain.gm_boss4_n_scroll_pt_x + AppMain.gmCameraForceScrollFuncStatics.ofst;
            AppMain.ObjObjectCameraSet(AppMain.FXM_FLOAT_TO_FX32(obj_cam.disp_pos.x - (float)((int)AppMain.OBD_LCD_X / 2)), AppMain.FXM_FLOAT_TO_FX32(-obj_cam.disp_pos.y - (float)((int)AppMain.OBD_LCD_Y / 2)), AppMain.FXM_FLOAT_TO_FX32(obj_cam.disp_pos.x - (float)((int)AppMain.OBD_LCD_X / 2)), AppMain.FXM_FLOAT_TO_FX32(-obj_cam.disp_pos.y - (float)((int)AppMain.OBD_LCD_Y / 2)));
            AppMain.GmCameraSetClipCamera(obj_cam);
            if (flag && AppMain.g_gs_main_sys_info.stage_id >= (ushort)16)
            {
                if (AppMain.gm_boss4_n_scroll == 1 || AppMain.gm_boss4_n_scroll == 2)
                    AppMain.GmDecoSetLoopState();
                AppMain.GmEveMgrCreateEventLcd(0U);
                AppMain.GmDecoClearLoopState();
            }
            AppMain.GmBoss4UtilIterateDamageRingInit();
            if (AppMain.gmCameraForceScrollFuncStatics._damage_cnt == 0)
            {
                if (AppMain.GmBoss4UtilIterateDamageRingGet() != null)
                {
                    ++AppMain.gmCameraForceScrollFuncStatics._damage_cnt;
                    AppMain.GmBoss4UtilIterateDamageRingInit();
                    AppMain.GMS_RING_WORK gmsRingWork;
                    while ((gmsRingWork = AppMain.GmBoss4UtilIterateDamageRingGet()) != null)
                    {
                        gmsRingWork.pos.x += AppMain.GmBoss4GetScrollOffset();
                        gmsRingWork.spd_x += AppMain.FX_F32_TO_FX32(1f);
                    }
                }
            }
            else if (AppMain.GmBoss4UtilIterateDamageRingGet() == null)
                AppMain.gmCameraForceScrollFuncStatics._damage_cnt = 0;
            AppMain.GmBoss4UtilIterateDamageRingInit();
            AppMain.GMS_RING_WORK gmsRingWork1;
            while ((gmsRingWork1 = AppMain.GmBoss4UtilIterateDamageRingGet()) != null)
            {
                gmsRingWork1.pos.x += AppMain.GmBoss4GetScrollOffset();
                gmsRingWork1.spd_x += AppMain.FX_F32_TO_FX32(-0.1f);
            }
            AppMain.OBS_OBJECT_WORK playerObj2 = AppMain.GmBsCmnGetPlayerObj();
            AppMain.gmCameraForceScrollFuncStatics.ofst = (float)(((double)AppMain.AMD_SCREEN_2D_WIDTH / (double)AppMain.GSD_DISP_WIDTH - 1.0) * ((double)(playerObj2.pos.x / 4096) - (double)obj_cam.pos.x) / 30.0);
            bool enable = false;
            if (AppMain.gm_boss4_n_scroll == 1 || AppMain.gm_boss4_n_scroll == 2)
                enable = true;
            else
                AppMain.GmDecoEndLoop();
            float gmBoss4FScrollSpd = AppMain.gm_boss4_f_scroll_spd;
            if ((double)gmBoss4FScrollSpd < -100.0)
                gmBoss4FScrollSpd += (float)(AppMain.GMD_BOSS4_SCROLL_END_X - AppMain.GMD_BOSS4_SCROLL_START_X);
            if (AppMain.gm_boss4_n_scroll == 2)
            {
                if (playerObj1.seq_state == 1)
                    AppMain.GmBoss4UtilPlayerStop(true);
                if (playerObj2.pos.x > AppMain.FX_F32_TO_FX32(obj_cam.pos.x))
                    AppMain.GmPlayerSetAutoRun((AppMain.GMS_PLAYER_WORK)playerObj2, (int)(((double)gmBoss4FScrollSpd + 0.600000023841858) * 4096.0), enable);
                else
                    AppMain.GmPlayerSetAutoRun((AppMain.GMS_PLAYER_WORK)playerObj2, (int)(((double)gmBoss4FScrollSpd + 1.39999997615814) * 4096.0), enable);
            }
            else
                AppMain.GmPlayerSetAutoRun((AppMain.GMS_PLAYER_WORK)playerObj2, (int)(((double)gmBoss4FScrollSpd - 0.5) * 4096.0), enable);
            AppMain.gm_boss4_n_scroll_pt_x += (int)AppMain.gm_boss4_f_scroll_spd;
            AppMain.gm_boss4_n_offset_x = (int)AppMain.gm_boss4_f_scroll_spd;
            if (AppMain.gm_boss4_b_warpout)
            {
                AppMain.gm_boss4_b_warpout = false;
                int num = AppMain.GMD_BOSS4_SCROLL_OUT_X - AppMain.gm_boss4_n_scroll_pt_x;
                AppMain.gm_boss4_n_scroll_pt_x = AppMain.GMD_BOSS4_SCROLL_OUT_X;
                AppMain.gm_boss4_n_offset_x = num;
            }
            if (AppMain.gm_boss4_n_scroll_pt_x > AppMain.gm_boss4_n_scroll_end)
            {
                AppMain.gm_boss4_n_offset_x = -(AppMain.gm_boss4_n_scroll_end - AppMain.gm_boss4_n_scroll_start) + (int)AppMain.gm_boss4_f_scroll_spd;
                AppMain.gm_boss4_n_scroll_pt_x -= AppMain.gm_boss4_n_scroll_end - AppMain.gm_boss4_n_scroll_start;
            }
            AppMain.g_gm_main_system.map_fcol.left = AppMain.gm_boss4_n_scroll_pt_x - (int)((double)(AppMain.AMD_SCREEN_2D_WIDTH / 2f) * 0.674383342266083) - 48;
            AppMain.g_gm_main_system.map_fcol.right = AppMain.gm_boss4_n_scroll_pt_x + (int)((double)(AppMain.AMD_SCREEN_2D_WIDTH / 2f) * 0.674383342266083);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        }
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4ScrollInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.UNREFERENCED_PARAMETER((object)pos_x);
        AppMain.UNREFERENCED_PARAMETER((object)pos_y);
        AppMain.UNREFERENCED_PARAMETER((object)eve_rec);
        AppMain.gm_boss4_n_scroll = 1;
        AppMain.gm_boss4_f_scroll_spd = AppMain.GMD_BOSS4_SCROLL_SPD_START;
        AppMain.gm_boss4_f_scroll_spd = -100f;
        AppMain.gm_boss4_n_scroll_pt_x = 0;
        AppMain.ObjCameraSetUserFunc(0, new AppMain.OBJF_CAMERA_USER_FUNC(AppMain.gmCameraForceScrollFunc));
        AppMain.gm_boss4_is_2nd = true;
        return (AppMain.OBS_OBJECT_WORK)null;
    }

    private static void GmBoss4ScrollNext()
    {
        switch (AppMain.gm_boss4_n_scroll)
        {
            case 1:
                AppMain.gm_boss4_n_scroll = 2;
                break;
            case 2:
                AppMain.gm_boss4_n_scroll = 0;
                break;
        }
        AppMain.gm_boss4_n_offset_x = 0;
    }

    private static void GmBoss4ScrollOff()
    {
        AppMain.gm_boss4_n_scroll = 0;
        AppMain.gm_boss4_n_offset_x = 0;
        AppMain.gm_boss4_f_scroll_spd = 0.0f;
        AppMain.gm_boss4_b_warpout = false;
    }

    private static void GmBoss4ScrollOut()
    {
        AppMain.gm_boss4_b_warpout = true;
    }

    private static bool GmBoss4Is2ndStage()
    {
        return AppMain.gm_boss4_is_2nd;
    }

    private static bool GmBoss4CheckBossRush()
    {
        return AppMain.gm_boss4_mgr_work != null && 0 != AppMain.GmBsCmnIsFinalZoneType(AppMain.gm_boss4_mgr_work.Cast());
    }

    private static void GmBoss4IncObjCreateCount()
    {
        AppMain.MTM_ASSERT(AppMain.gm_boss4_mgr_work != null);
        AppMain.MTM_ASSERT(AppMain.gm_boss4_mgr_work.obj_create_cnt >= 0);
        ++AppMain.gm_boss4_mgr_work.obj_create_cnt;
    }

    private static void GmBoss4DecObjCreateCount()
    {
        AppMain.MTM_ASSERT(AppMain.gm_boss4_mgr_work != null);
        AppMain.MTM_ASSERT(AppMain.gm_boss4_mgr_work.obj_create_cnt > 0);
        --AppMain.gm_boss4_mgr_work.obj_create_cnt;
    }

    private static bool GmBoss4IsAllCreatedObjDeleted()
    {
        AppMain.MTM_ASSERT(AppMain.gm_boss4_mgr_work != null);
        AppMain.MTM_ASSERT(AppMain.gm_boss4_mgr_work.obj_create_cnt >= 0);
        return AppMain.gm_boss4_mgr_work.obj_create_cnt <= 0;
    }

}
