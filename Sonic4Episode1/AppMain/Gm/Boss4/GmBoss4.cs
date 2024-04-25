public partial class AppMain
{
    public static AMS_AMB_HEADER GMD_BOSS4_ARC => g_gm_gamedat_enemy_arc;

    public static T GMM_BOSS4_STAGE<T>(T s4, T sf)
    {
        return !GmBoss4CheckBossRush() ? s4 : sf;
    }

    public static int GMD_BOSS4_SCROLL_INIT_X => GMM_BOSS4_STAGE(2500, 11000);

    public static int GMD_BOSS4_SCROLL_START_X => GMM_BOSS4_STAGE(2500, 11000);

    public static int GMD_BOSS4_SCROLL_END_X => GMM_BOSS4_STAGE(3972, 12536);

    public static int GMD_BOSS4_SCROLL_OUT_X => GMM_BOSS4_STAGE(2500, 11000);

    public static int GMD_BOSS4_LIFE => GMM_BOSS4_STAGE(8, 4);

    public static int GMD_BOSS4_EXTRA_ATK_THRESHOLD_LIFE => GMM_BOSS4_STAGE(3, 4);

    public static int GMM_BOSS4_AREA_LEFT()
    {
        return g_gm_main_system.map_fcol.left << 12;
    }

    public static int GMM_BOSS4_AREA_TOP()
    {
        return g_gm_main_system.map_fcol.top << 12;
    }

    public static int GMM_BOSS4_AREA_RIGHT()
    {
        return g_gm_main_system.map_fcol.right << 12;
    }

    public static int GMM_BOSS4_AREA_BOTTOM()
    {
        return g_gm_main_system.map_fcol.bottom << 12;
    }

    public static int GMM_BOSS4_AREA_CENTER_X()
    {
        return GMM_BOSS4_AREA_LEFT() + (GMM_BOSS4_AREA_RIGHT() - GMM_BOSS4_AREA_LEFT()) / 2;
    }

    public static int GMM_BOSS4_AREA_CENTER_Y()
    {
        return GMM_BOSS4_AREA_TOP() + (GMM_BOSS4_AREA_BOTTOM() - GMM_BOSS4_AREA_TOP()) / 2;
    }

    private static OBS_OBJECT_WORK GMM_BOSS4_CREATE_WORK(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      TaskWorkFactoryDelegate work_size,
      string name)
    {
        return GmEnemyCreateWork(eve_rec, pos_x, pos_y, work_size, 4342, name);
    }

    private static void GmBoss4SetLife(int life)
    {
        MTM_ASSERT(gm_boss4_mgr_work);
        gm_boss4_mgr_work.life = life;
    }

    private static int GmBoss4GetLife()
    {
        MTM_ASSERT(gm_boss4_mgr_work);
        return gm_boss4_mgr_work == null ? 0 : gm_boss4_mgr_work.life;
    }

    private static OBS_OBJECT_WORK GmBoss4GetBodyWork()
    {
        MTM_ASSERT(gm_boss4_mgr_work);
        return gm_boss4_mgr_work == null ? null : (OBS_OBJECT_WORK)gm_boss4_mgr_work.body_work;
    }

    private static OBS_ACTION3D_NN_WORK GmBoss4GetObj3D(int n)
    {
        return gm_boss4_obj_3d_list[n];
    }

    private static GMS_BOSS4_PART_ACT_INFO GmBoss4GetActInfo(int act, int parts)
    {
        return gm_boss4_act_id_tbl[act][parts];
    }

    private static void GmBoss4Build()
    {
        gm_boss4_obj_3d_list = GmGameDBuildRegBuildModel((AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, GMD_BOSS4_ARC), (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 1, GMD_BOSS4_ARC), 0U);
        GmBoss4BodyBuild();
        GmBoss4EggmanBuild();
        GmBoss4CapsuleBuild();
        GmBoss4ChibiBuild();
        GmBoss4EffectBuild();
    }

    private static void GmBoss4Flush()
    {
        GmBoss4EffectFlush();
        GmBoss4ChibiFlush();
        GmBoss4CapsuleFlush();
        GmBoss4EggmanFlush();
        GmBoss4BodyFlush();
        AMS_AMB_HEADER amsAmbHeader = (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, GMD_BOSS4_ARC);
        GmGameDBuildRegFlushModel(gm_boss4_obj_3d_list, amsAmbHeader.file_num);
        gm_boss4_obj_3d_list = null;
        gm_boss4_mgr_work = null;
    }

    private static bool GmBoss4IsBuilded()
    {
        MTM_ASSERT(gm_boss4_mgr_work);
        return 0 != ((int)gm_boss4_mgr_work.flag & 1);
    }

    private static OBS_OBJECT_WORK GmBoss4Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_BOSS4_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS4_MGR_WORK(), "Boss4_MGR");
        GMS_BOSS4_MGR_WORK gmsBosS4MgrWork = (GMS_BOSS4_MGR_WORK)work;
        gm_boss4_mgr_work = gmsBosS4MgrWork;
        work.flag |= 16U;
        work.disp_flag |= 32U;
        work.move_flag |= 8448U;
        gmsBosS4MgrWork.life = GMD_BOSS4_LIFE;
        work.pos.x = pos_x;
        work.pos.y = pos_y;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4MgrWaitLoad);
        GmBoss4ScrollOff();
        gm_boss4_is_2nd = false;
        if (g_gs_main_sys_info.stage_id != 16)
        {
            GmMapSetAddMapScrlScaleMagX(2, 1);
            GmMapSetAddMapScrlScaleMagX(3, 1);
            GmMapSetAddMapScrlScaleMagX(4, 1);
            GmMapSetAddMapXLoop();
            GmMapEnableAddMapUserScrlX();
        }
        gm_boss4_n_scroll_start = GMD_BOSS4_SCROLL_START_X;
        gm_boss4_n_scroll_end = GMD_BOSS4_SCROLL_END_X;
        return work;
    }

    private static int GmBoss4GetScrollOffset()
    {
        return FX_F32_TO_FX32(gm_boss4_n_offset_x);
    }

    private static void gmBoss4SetPartTextureBurnt(OBS_OBJECT_WORK obj_work)
    {
        gmBoss4SetPartTextureBurnt(obj_work, true);
    }

    private static void gmBoss4SetPartTextureBurnt(OBS_OBJECT_WORK obj_work, bool burn)
    {
        MTM_ASSERT(obj_work);
        MTM_ASSERT(obj_work.obj_3d);
        MTM_ASSERT(obj_work.disp_flag & 134217728U);
        obj_work.obj_3d.drawflag |= 268435456U;
        obj_work.obj_3d.draw_state.texoffset[0].mode = 2;
        if (burn)
            obj_work.obj_3d.draw_state.texoffset[0].u = 0.5f;
        else
            obj_work.obj_3d.draw_state.texoffset[0].u = 0.0f;
    }

    private static bool gmBoss4IsScrollLockBusy()
    {
        return ((int)g_gm_main_system.game_flag & 32768) != 0;
    }



    private static void gmCameraForceScrollFunc(OBS_CAMERA obj_cam)
    {
        GMS_PLAYER_WORK playerObj1 = (GMS_PLAYER_WORK)GmBsCmnGetPlayerObj();
        if (((int)g_gm_main_system.game_flag & 64) != 0)
            return;
        if (gm_boss4_f_scroll_spd <= -100.0 && gm_boss4_n_scroll_pt_x == 0)
        {
            gm_boss4_f_scroll_spd = obj_cam.pos.x == (double)obj_cam.prev_pos.x ? FXM_FX32_TO_FLOAT(playerObj1.obj_work.move.x) : obj_cam.pos.x - obj_cam.prev_pos.x;
            gm_boss4_n_scroll_pt_x = (int)(obj_cam.pos.x + (double)gm_boss4_f_scroll_spd);
        }
        if (gm_boss4_n_scroll != 0)
            gm_boss4_f_scroll_spd += GMD_BOSS4_SCROLL_SPD_ADD;
        else
            gm_boss4_f_scroll_spd -= GMD_BOSS4_SCROLL_SPD_SUB;
        if (gm_boss4_f_scroll_spd > (double)gm_boss4_f_scroll_spd_max)
            gm_boss4_f_scroll_spd = gm_boss4_f_scroll_spd_max;
        if (gm_boss4_f_scroll_spd < 0.0)
            gm_boss4_f_scroll_spd = 0.0f;
        if (gm_boss4_n_scroll == 0 && gm_boss4_f_scroll_spd == 0.0)
        {
            g_gm_main_system.map_fcol.left = 0;
            g_gm_main_system.map_fcol.right = g_gm_main_system.map_fcol.map_block_num_x * 64;
            ObjCameraSetUserFunc(0, new OBJF_CAMERA_USER_FUNC(GmCameraFunc));
            GmGmkCamScrLimitRelease(14);
            GmBoss4UtilPlayerStop(false);
        }
        else
        {
            NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
            nnsVector.x = FXM_FX32_TO_FLOAT(playerObj1.obj_work.pos.x);
            nnsVector.y = FXM_FX32_TO_FLOAT(-playerObj1.obj_work.pos.y + 24576);
            nnsVector.z = FXM_FX32_TO_FLOAT(playerObj1.obj_work.pos.z);
            obj_cam.work.x = nnsVector.x;
            obj_cam.work.y = nnsVector.y;
            obj_cam.work.z = nnsVector.z;
            if (gm_boss4_n_scroll == 0)
            {
                GmBoss4UtilPlayerStop(false);
                int f32 = (int)FX_FX32_TO_F32(playerObj1.obj_work.pos.x + playerObj1.obj_work.spd.x);
                gm_boss4_n_scroll_pt_x = f32 >= gm_boss4_n_scroll_pt_x - (double)obj_cam.allow.x ? (f32 <= gm_boss4_n_scroll_pt_x + (double)obj_cam.allow.x ? gm_boss4_n_scroll_pt_x : f32 - (int)obj_cam.allow.x) : f32 + (int)obj_cam.allow.x;
            }
            bool flag = false;
            if (obj_cam.pos.x > (double)gm_boss4_n_scroll_pt_x)
                flag = true;
            obj_cam.prev_pos.x = obj_cam.pos.x;
            obj_cam.pos.x = gm_boss4_n_scroll_pt_x;
            obj_cam.disp_pos.x = gm_boss4_n_scroll_pt_x;
            obj_cam.target_pos.x = gm_boss4_n_scroll_pt_x + gmCameraForceScrollFuncStatics.ofst;
            ObjObjectCameraSet(FXM_FLOAT_TO_FX32(obj_cam.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_cam.disp_pos.y - OBD_LCD_Y / 2), FXM_FLOAT_TO_FX32(obj_cam.disp_pos.x - OBD_LCD_X / 2), FXM_FLOAT_TO_FX32(-obj_cam.disp_pos.y - OBD_LCD_Y / 2));
            GmCameraSetClipCamera(obj_cam);
            if (flag && g_gs_main_sys_info.stage_id >= 16)
            {
                if (gm_boss4_n_scroll == 1 || gm_boss4_n_scroll == 2)
                    GmDecoSetLoopState();
                GmEveMgrCreateEventLcd(0U);
                GmDecoClearLoopState();
            }
            GmBoss4UtilIterateDamageRingInit();
            if (gmCameraForceScrollFuncStatics._damage_cnt == 0)
            {
                if (GmBoss4UtilIterateDamageRingGet() != null)
                {
                    ++gmCameraForceScrollFuncStatics._damage_cnt;
                    GmBoss4UtilIterateDamageRingInit();
                    GMS_RING_WORK gmsRingWork;
                    while ((gmsRingWork = GmBoss4UtilIterateDamageRingGet()) != null)
                    {
                        gmsRingWork.pos.x += GmBoss4GetScrollOffset();
                        gmsRingWork.spd_x += FX_F32_TO_FX32(1f);
                    }
                }
            }
            else if (GmBoss4UtilIterateDamageRingGet() == null)
                gmCameraForceScrollFuncStatics._damage_cnt = 0;
            GmBoss4UtilIterateDamageRingInit();
            GMS_RING_WORK gmsRingWork1;
            while ((gmsRingWork1 = GmBoss4UtilIterateDamageRingGet()) != null)
            {
                gmsRingWork1.pos.x += GmBoss4GetScrollOffset();
                gmsRingWork1.spd_x += FX_F32_TO_FX32(-0.1f);
            }
            OBS_OBJECT_WORK playerObj2 = GmBsCmnGetPlayerObj();
            gmCameraForceScrollFuncStatics.ofst = (float)((AMD_SCREEN_2D_WIDTH / (double)GSD_DISP_WIDTH - 1.0) * (playerObj2.pos.x / 4096 - (double)obj_cam.pos.x) / 30.0);
            bool enable = false;
            if (gm_boss4_n_scroll == 1 || gm_boss4_n_scroll == 2)
                enable = true;
            else
                GmDecoEndLoop();
            float gmBoss4FScrollSpd = gm_boss4_f_scroll_spd;
            if (gmBoss4FScrollSpd < -100.0)
                gmBoss4FScrollSpd += GMD_BOSS4_SCROLL_END_X - GMD_BOSS4_SCROLL_START_X;
            if (gm_boss4_n_scroll == 2)
            {
                if (playerObj1.seq_state == GME_PLY_SEQ_STATE_WALK)
                    GmBoss4UtilPlayerStop(true);
                if (playerObj2.pos.x > FX_F32_TO_FX32(obj_cam.pos.x))
                    GmPlayerSetAutoRun((GMS_PLAYER_WORK)playerObj2, (int)((gmBoss4FScrollSpd + 0.600000023841858) * 4096.0), enable);
                else
                    GmPlayerSetAutoRun((GMS_PLAYER_WORK)playerObj2, (int)((gmBoss4FScrollSpd + 1.39999997615814) * 4096.0), enable);
            }
            else
                GmPlayerSetAutoRun((GMS_PLAYER_WORK)playerObj2, (int)((gmBoss4FScrollSpd - 0.5) * 4096.0), enable);
            gm_boss4_n_scroll_pt_x += (int)gm_boss4_f_scroll_spd;
            gm_boss4_n_offset_x = (int)gm_boss4_f_scroll_spd;
            if (gm_boss4_b_warpout)
            {
                gm_boss4_b_warpout = false;
                int num = GMD_BOSS4_SCROLL_OUT_X - gm_boss4_n_scroll_pt_x;
                gm_boss4_n_scroll_pt_x = GMD_BOSS4_SCROLL_OUT_X;
                gm_boss4_n_offset_x = num;
            }
            if (gm_boss4_n_scroll_pt_x > gm_boss4_n_scroll_end)
            {
                gm_boss4_n_offset_x = -(gm_boss4_n_scroll_end - gm_boss4_n_scroll_start) + (int)gm_boss4_f_scroll_spd;
                gm_boss4_n_scroll_pt_x -= gm_boss4_n_scroll_end - gm_boss4_n_scroll_start;
            }
            g_gm_main_system.map_fcol.left = gm_boss4_n_scroll_pt_x - (int)(AMD_SCREEN_2D_WIDTH / 2f * 0.674383342266083) - 48;
            g_gm_main_system.map_fcol.right = gm_boss4_n_scroll_pt_x + (int)(AMD_SCREEN_2D_WIDTH / 2f * 0.674383342266083);
            GlobalPool<NNS_VECTOR>.Release(nnsVector);
        }
    }

    private static OBS_OBJECT_WORK GmBoss4ScrollInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        UNREFERENCED_PARAMETER(pos_x);
        UNREFERENCED_PARAMETER(pos_y);
        UNREFERENCED_PARAMETER(eve_rec);
        gm_boss4_n_scroll = 1;
        gm_boss4_f_scroll_spd = GMD_BOSS4_SCROLL_SPD_START;
        gm_boss4_f_scroll_spd = -100f;
        gm_boss4_n_scroll_pt_x = 0;
        ObjCameraSetUserFunc(0, new OBJF_CAMERA_USER_FUNC(gmCameraForceScrollFunc));
        gm_boss4_is_2nd = true;
        return null;
    }

    private static void GmBoss4ScrollNext()
    {
        switch (gm_boss4_n_scroll)
        {
            case 1:
                gm_boss4_n_scroll = 2;
                break;
            case 2:
                gm_boss4_n_scroll = 0;
                break;
        }
        gm_boss4_n_offset_x = 0;
    }

    private static void GmBoss4ScrollOff()
    {
        gm_boss4_n_scroll = 0;
        gm_boss4_n_offset_x = 0;
        gm_boss4_f_scroll_spd = 0.0f;
        gm_boss4_b_warpout = false;
    }

    private static void GmBoss4ScrollOut()
    {
        gm_boss4_b_warpout = true;
    }

    private static bool GmBoss4Is2ndStage()
    {
        return gm_boss4_is_2nd;
    }

    private static bool GmBoss4CheckBossRush()
    {
        return gm_boss4_mgr_work != null && 0 != GmBsCmnIsFinalZoneType(gm_boss4_mgr_work.Cast());
    }

    private static void GmBoss4IncObjCreateCount()
    {
        MTM_ASSERT(gm_boss4_mgr_work != null);
        MTM_ASSERT(gm_boss4_mgr_work.obj_create_cnt >= 0);
        ++gm_boss4_mgr_work.obj_create_cnt;
    }

    private static void GmBoss4DecObjCreateCount()
    {
        MTM_ASSERT(gm_boss4_mgr_work != null);
        MTM_ASSERT(gm_boss4_mgr_work.obj_create_cnt > 0);
        --gm_boss4_mgr_work.obj_create_cnt;
    }

    private static bool GmBoss4IsAllCreatedObjDeleted()
    {
        MTM_ASSERT(gm_boss4_mgr_work != null);
        MTM_ASSERT(gm_boss4_mgr_work.obj_create_cnt >= 0);
        return gm_boss4_mgr_work.obj_create_cnt <= 0;
    }

}
