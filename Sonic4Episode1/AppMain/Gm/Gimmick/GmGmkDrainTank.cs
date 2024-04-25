public partial class AppMain
{
    public static int GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH => OBD_LCD_X + 256;

    public static int GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT => OBD_LCD_Y + 256;

    private static OBS_OBJECT_WORK GmGmkDrainTankInitIn(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkDrainTankLoadObj(eve_rec, pos_x, pos_y, 0, 0U).ene_com.obj_work;
        gmGmkDrainTankInInit(objWork);
        return objWork;
    }

    private static OBS_OBJECT_WORK GmGmkDrainTankInitOut(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)g_gm_main_system.ply_work[0];
        if (pos_x - 524288 < obsObjectWork.pos.x)
            return null;
        OBS_OBJECT_WORK objWork = gmGmkDrainTankLoadObj(eve_rec, pos_x, pos_y, 1, 1U).ene_com.obj_work;
        gmGmkDrainTankOutInit(objWork);
        return objWork;
    }

    private static OBS_OBJECT_WORK GmGmkDrainTankSplashInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkDrainTankLoadObj(eve_rec, pos_x, pos_y, 2, 2U).ene_com.obj_work;
        gmGmkDrainTankSplashInit(objWork);
        return objWork;
    }

    public static void GmGmkDrainTankBuild()
    {
        g_gm_gmk_drain_tank_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(923), GmGameDatGetGimmickData(924), 0U);
    }

    public static void GmGmkDrainTankFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(923);
        GmGameDBuildRegFlushModel(g_gm_gmk_drain_tank_obj_3d_list, gimmickData.file_num);
        g_gm_gmk_drain_tank_obj_3d_list = null;
    }

    private static GMS_ENEMY_3D_WORK gmGmkDrainTankLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork;
        if (type == 1)
        {
            GMS_GMK_DRAIN_TANK_OUT_WORK work = (GMS_GMK_DRAIN_TANK_OUT_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_DRAIN_TANK_OUT_WORK(), "GMK_DRAIN_TANK");
            work.base_pos_x = pos_x + 262144;
            work.base_pos_y = pos_y;
            gmsEnemy3DWork = work.enemy_work;
        }
        else
            gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_DRAIN_TANK");
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        return gmsEnemy3DWork;
    }

    private static GMS_ENEMY_3D_WORK gmGmkDrainTankLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type,
      uint model_id)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkDrainTankLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        ObjObjectCopyAction3dNNModel(gmsEnemy3DWork.ene_com.obj_work, g_gm_gmk_drain_tank_obj_3d_list[(int)model_id], gmsEnemy3DWork.obj_3d);
        if (type == 2)
        {
            int index = 0;
            object pData = ObjDataGet(925).pData;
            ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, null, null, index, (AMS_AMB_HEADER)pData);
        }
        return gmsEnemy3DWork;
    }

    private static ushort gmGmkDrainTankGameSystemGetWaterLevel()
    {
        return g_gm_main_system.water_level;
    }

    private static void gmGmkDrainTankInInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjObjectFieldRectSet(obj_work, -16, 0, 0, 14);
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = 16;
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = 32;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = -16;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = -32;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankDrawFuncIn);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankInMainReady);
        obj_work.disp_flag |= 4194304U;
        gmsEnemy3DWork.obj_3d.command_state = 8U;
        obj_work.pos.z = 1441792;
    }

    private static void gmGmkDrainTankOutInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjObjectFieldRectSet(obj_work, 0, 0, 32, 16);
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankDrawFuncOut);
        gmGmkDrainTankOutChangeModeReady(obj_work);
        obj_work.disp_flag |= 4194304U;
        obj_work.move_flag |= 128U;
        obj_work.flag |= 16U;
        gmsEnemy3DWork.obj_3d.command_state = 8U;
        obj_work.pos.z = 1441792;
        GmWaterSurfaceRequestChangeWaterLevel((ushort)FX_FX32_TO_F32(obj_work.pos.y - 196608), 0, false);
        GmWaterSurfaceSetFlagEnableRef(false);
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkDrainTankTcbDestOut));
    }

    private static void gmGmkDrainTankSplashInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.disp_flag |= 4194304U;
        obj_work.move_flag = 8448U;
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.pos.z = 262144;
        obj_work.disp_flag |= 134217728U;
        obj_work.obj_3d.drawflag |= 8388608U;
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        obj_work.user_timer = 60;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankSplashMainFunc);
        obj_work.ppMove = null;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork1 = GmEfctZoneEsCreate(obj_work, 2, 34);
        gmsEffect3DesWork1.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankSplashEffectMain);
        gmsEffect3DesWork1.efct_com.obj_work.move_flag = 384U;
        gmsEffect3DesWork1.efct_com.obj_work.pos.y += -131072;
        gmsEffect3DesWork1.efct_com.obj_work.pos.z = obj_work.pos.z + 131072;
        gmsEffect3DesWork1.efct_com.obj_work.spd.x = 49152;
        gmsEffect3DesWork1.efct_com.obj_work.spd.y = 12288;
        gmsEffect3DesWork1.efct_com.obj_work.spd_add.x = -1376;
        gmsEffect3DesWork1.efct_com.obj_work.spd_add.y = 0;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork2 = GmEfctZoneEsCreate(obj_work, 2, 35);
        gmsEffect3DesWork2.efct_com.obj_work.pos.y += -65536;
        gmsEffect3DesWork2.efct_com.obj_work.pos.z = obj_work.pos.z + 131072;
    }

    private static void gmGmkDrainTankDrawFuncIn(OBS_OBJECT_WORK obj_work)
    {
        ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, 1, 8U);
        ObjDrawActionSummary(obj_work);
    }

    private static void gmGmkDrainTankDrawFuncOut(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 32) == 0)
        {
            GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
            int num1 = ((-(int)FX_FX32_TO_F32(drainTankOutWork.base_pos_y) - 64) / 64 - 1) * 64;
            int num2 = ((int)FX_FX32_TO_F32(drainTankOutWork.base_pos_x) - 96) / 64 * 64;
            int num3 = num2 - 152;
            int num4 = num1 + 96;
            int num5 = num2 + 64;
            ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, 1, 9U);
            GmWaterSurfaceDrawNoWaterField(FX_FX32_TO_F32(obj_work.pos.x), -FX_FX32_TO_F32(obj_work.pos.y) + GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT, FX_FX32_TO_F32(obj_work.pos.x) + GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH, -FX_FX32_TO_F32(obj_work.pos.y) - GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT);
            GmWaterSurfaceDrawNoWaterField(num2, -FX_FX32_TO_F32(drainTankOutWork.base_pos_y) + GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT, FX_FX32_TO_F32(drainTankOutWork.base_pos_x) + GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH, num4);
            GmWaterSurfaceDrawNoWaterField(FX_FX32_TO_F32(drainTankOutWork.base_pos_x) - GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH, num1, FX_FX32_TO_F32(drainTankOutWork.base_pos_x) + GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH, num1 - GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT);
            GmWaterSurfaceDrawNoWaterField(num3 - GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH, -FX_FX32_TO_F32(drainTankOutWork.base_pos_y) + GMD_GMK_DRAIN_TANK_DRAW_WATER_HEIGHT, num3, num1);
            GmWaterSurfaceDrawNoWaterField(num5, num4, num5 + GMD_GMK_DRAIN_TANK_DRAW_WATER_WIDTH, num1);
        }
        ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, 1, 8U);
        ObjDrawActionSummary(obj_work);
    }

    private static void gmGmkDrainTankTcbDestOut(MTS_TASK_TCB tcb)
    {
        GmWaterSurfaceSetFlagEnableRef(true);
        g_gm_main_system.game_flag &= 4294959103U;
        GmEnemyDefaultExit(tcb);
    }

    private static bool gmGmkDrainTankInCheckDeleteTask(
      OBS_OBJECT_WORK obj_work,
      int cmp_x,
      int cmp_y)
    {
        OBS_OBJECT_WORK objWork = g_gm_main_system.ply_work[0].obj_work;
        int num1 = MTM_MATH_ABS(obj_work.pos.x - objWork.pos.x);
        int num2 = MTM_MATH_ABS(obj_work.pos.y - objWork.pos.y);
        return num1 > cmp_x || num2 > cmp_y;
    }

    private static void gmGmkDrainTankInRequestDeleteTask(OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag |= 4U;
    }

    private static void gmGmkDrainTankInMainReady(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
            return;
        if (gmGmkDrainTankInCheckDeleteTask(obj_work, 1843200, 1228800))
        {
            gmGmkDrainTankInRequestDeleteTask(obj_work);
        }
        else
        {
            OBS_OBJECT_WORK objWork = gmsPlayerWork.obj_work;
            if (obj_work.pos.x + 65536 >= objWork.pos.x)
                return;
            obj_work.pos.y += 32768;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankInMainWait);
            obj_work.move_flag |= 128U;
        }
    }

    private static void gmGmkDrainTankInMainWait(OBS_OBJECT_WORK obj_work)
    {
        if (((int)g_gm_main_system.ply_work[0].player_flag & 1024) != 0 || !gmGmkDrainTankInCheckDeleteTask(obj_work, 1843200, 1228800))
            return;
        gmGmkDrainTankInRequestDeleteTask(obj_work);
    }

    private static bool gmGmkDrainTankOutCheckDeleteTask(
      OBS_OBJECT_WORK obj_work,
      int cmp_x,
      int cmp_y)
    {
        OBS_OBJECT_WORK objWork = g_gm_main_system.ply_work[0].obj_work;
        GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
        int num1 = MTM_MATH_ABS(drainTankOutWork.base_pos_x - objWork.pos.x);
        int num2 = MTM_MATH_ABS(drainTankOutWork.base_pos_y - objWork.pos.y);
        return num1 > cmp_x || num2 > cmp_y;
    }

    private static void gmGmkDrainTankOutRequestDeleteTask(OBS_OBJECT_WORK obj_work)
    {
        GmPlayerCameraOffsetSet(g_gm_main_system.ply_work[0], 0, 0);
        GmCameraAllowReset();
        GmWaterSurfaceRequestChangeWaterLevel(ushort.MaxValue, 0, false);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankOutMainWaitDelete);
    }

    private static void gmGmkDrainTankOutChangeModeReady(OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.x = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankOutMainReady);
    }

    private static void gmGmkDrainTankOutChangeModeWait(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK objWork = g_gm_main_system.ply_work[0].obj_work;
        GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
        drainTankOutWork.player_offset_x = objWork.pos.x - drainTankOutWork.base_pos_x;
        drainTankOutWork.player_offset_y = objWork.pos.y - drainTankOutWork.base_pos_y;
        obj_work.spd.x = 0;
        drainTankOutWork.counter_roll_key = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankOutMainWait);
    }

    private static void gmGmkDrainTankOutChangeModeDamage(OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.x = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankOutMainDamage);
    }

    private static void gmGmkDrainTankOutChangeModeSplash(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (GMS_GMK_DRAIN_TANK_OUT_WORK)gmsEnemy3DWork;
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        OBS_OBJECT_WORK objWork = ply_work.obj_work;
        objWork.spd.x = !drainTankOutWork.flag_dir_left ? 65536 : -65536;
        objWork.pos.y = obj_work.pos.y;
        objWork.move_flag |= 256U;
        GmPlayerBreathingSet(ply_work);
        obj_work.move_flag |= 256U;
        g_gm_main_system.game_flag |= 8192U;
        GmEventMgrLocalEventBirth(305, obj_work.pos.x, obj_work.pos.y + 65536, gmsEnemy3DWork.ene_com.eve_rec.flag, gmsEnemy3DWork.ene_com.eve_rec.left, gmsEnemy3DWork.ene_com.eve_rec.top, gmsEnemy3DWork.ene_com.eve_rec.width, gmsEnemy3DWork.ene_com.eve_rec.height, 0);
        GmSoundPlaySE("Fluid2");
        GMM_PAD_VIB_SMALL();
        GmPlayerCameraOffsetSet(ply_work, 0, 0);
        GmCameraAllowReset();
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankOutMainSplash);
    }

    private static void gmGmkDrainTankOutChangeModeEnd(OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkDrainTankOutMainEnd);
    }

    private static void gmGmkDrainTankOutMainReady(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        OBS_OBJECT_WORK objWork = gmsPlayerWork.obj_work;
        GMS_GMK_DRAIN_TANK_OUT_WORK out_work = (GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
        {
            gmGmkDrainTankOutUpdateDie(obj_work);
            int num = ((int)FX_FX32_TO_F32(out_work.base_pos_x) / 64 - 3) * 262144;
            if (objWork.pos.x < num)
                g_gm_main_system.game_flag |= 8192U;
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        else if (gmGmkDrainTankOutCheckDeleteTask(obj_work, 1843200, 1228800))
        {
            gmGmkDrainTankOutRequestDeleteTask(obj_work);
        }
        else
        {
            if (gmGmkDrainTankGameSystemGetWaterLevel() * 4096 + 196608 >= objWork.pos.y)
                return;
            gmGmkDrainTankOutChangeModeWait(obj_work);
            out_work.flag_dir_left = gmGmkDrainTankOutCheckDirLeft(obj_work, objWork);
            int x = objWork.spd.x;
            GmPlySeqInitDrainTank(gmsPlayerWork);
            out_work.player_offset_x += x * 5;
            gmGmkDrainTankOutUpdateCameraOffset(gmsPlayerWork, out_work);
            GmCameraAllowSet(10f, 10f, 10f);
        }
    }

    private static void gmGmkDrainTankOutMainWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
        {
            gmGmkDrainTankOutUpdateDie(obj_work);
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        else if (gmGmkDrainTankOutCheckDeleteTask(obj_work, 1843200, 1228800))
        {
            gmGmkDrainTankOutRequestDeleteTask(obj_work);
        }
        else
        {
            OBS_OBJECT_WORK objWork = gmsPlayerWork.obj_work;
            if (gmsPlayerWork.seq_state == GME_PLY_SEQ_STATE_DAMAGE)
                gmGmkDrainTankOutChangeModeDamage(obj_work);
            else if (((int)obj_work.move_flag & 1) == 0)
            {
                gmGmkDrainTankOutChangeModeSplash(obj_work);
            }
            else
            {
                GMS_GMK_DRAIN_TANK_OUT_WORK out_work = (GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
                int gimmickRotZ = GmPlayerKeyGetGimmickRotZ(gmsPlayerWork);
                if (MTM_MATH_ABS(gimmickRotZ) > 8192)
                    ++out_work.counter_roll_key;
                else
                    out_work.counter_roll_key = 0;
                if (out_work.counter_roll_key >= 0)
                {
                    gmGmkDrainTankOutUpdateCameraRoll(out_work, gimmickRotZ);
                    if (19 == out_work.counter_roll_key % 20 && MTM_MATH_ABS(out_work.camera_roll) < GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX)
                        GmSoundPlaySE("Fluid1");
                }
                if (gmGmkDrainTankOutCheckKeyDir(obj_work, out_work.camera_roll))
                {
                    int num = out_work.camera_roll >> 3;
                    obj_work.spd.x = num;
                    GmWaterSurfaceRequestAddWatarLevel(MTM_MATH_ABS(MTM_MATH_ABS(FX_FX32_TO_F32(out_work.camera_roll >> 4))), 0, true);
                }
                gmGmkDrainTankOutAdjustPlayerOffsetBuoyancy(out_work);
                gmGmkDrainTankOutAdjustPlayerOffsetWave(out_work, objWork);
                gmGmkDrainTankOutApplyPlayerOffset(objWork, out_work);
                ObjCameraGet(0).roll = out_work.camera_roll;
                gmGmkDrainTankOutUpdateCameraOffset(gmsPlayerWork, out_work);
            }
        }
    }

    private static void gmGmkDrainTankOutMainDamage(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
        {
            gmGmkDrainTankOutUpdateDie(obj_work);
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        else if (gmGmkDrainTankOutCheckDeleteTask(obj_work, 1843200, 1228800))
        {
            gmGmkDrainTankOutRequestDeleteTask(obj_work);
        }
        else
        {
            GMS_GMK_DRAIN_TANK_OUT_WORK out_work = (GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
            OBS_OBJECT_WORK objWork = gmsPlayerWork.obj_work;
            gmGmkDrainTankOutUpdateCameraRollDamage(out_work);
            int num1 = (GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX - MTM_MATH_ABS(out_work.camera_roll)) * 4;
            int num2 = num1 >> 3;
            obj_work.spd.x = !out_work.flag_dir_left ? -num2 : num2;
            GmWaterSurfaceRequestAddWatarLevel(-FX_FX32_TO_F32(num1 >> 4), 0, true);
            ++obj_work.user_timer;
            if (((int)objWork.move_flag & 1) != 0)
            {
                obj_work.user_timer = 0;
                gmGmkDrainTankOutChangeModeWait(obj_work);
                GmPlySeqInitDrainTank(gmsPlayerWork);
            }
            else
            {
                ObjCameraGet(0).roll = out_work.camera_roll;
                gmGmkDrainTankOutUpdateCameraOffset(gmsPlayerWork, out_work);
                gmGmkDrainTankOutSinkRing();
            }
        }
    }

    private static void gmGmkDrainTankOutMainSplash(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        if ((ply_work.player_flag & 1024) != 0)
            gmGmkDrainTankOutUpdateDie(obj_work);
        else if (gmGmkDrainTankOutCheckDeleteTask(obj_work, 1638400, 1228800))
        {
            gmGmkDrainTankOutRequestDeleteTask(obj_work);
        }
        else
        {
            GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
            OBS_OBJECT_WORK objWork = ply_work.obj_work;
            GmWaterSurfaceRequestAddWatarLevel(1f / 1000f, 0, true);
            ObjCameraGet(0).roll = drainTankOutWork.camera_roll;
            if (drainTankOutWork.base_pos_x >= objWork.pos.x)
                return;
            objWork.move_flag |= 128U;
            objWork.move_flag &= 4294967039U;
            objWork.spd.x = 65536;
            objWork.spd_add.x = -896;
            GmPlayerCameraOffsetSet(ply_work, 0, 0);
            GmCameraAllowReset();
            gmGmkDrainTankOutChangeModeEnd(obj_work);
        }
    }

    private static void gmGmkDrainTankOutMainEnd(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        if ((ply_work.player_flag & 1024) != 0)
        {
            gmGmkDrainTankOutUpdateDie(obj_work);
        }
        else
        {
            if (!gmGmkDrainTankOutCheckDeleteTask(obj_work, 1638400, 1228800))
                return;
            gmGmkDrainTankOutRequestDeleteTask(obj_work);
            GmPlySeqInitDrainTankFall(ply_work);
        }
    }

    private static void gmGmkDrainTankOutMainWaitDelete(OBS_OBJECT_WORK obj_work)
    {
        if (gmGmkDrainTankGameSystemGetWaterLevel() != ushort.MaxValue)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmGmkDrainTankOutUpdateDie(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_DRAIN_TANK_OUT_WORK out_work = (GMS_GMK_DRAIN_TANK_OUT_WORK)obj_work;
        gmGmkDrainTankOutUpdateCameraRollDie(out_work);
        ObjCameraGet(0).roll = out_work.camera_roll;
    }

    private static bool gmGmkDrainTankOutCheckDirLeft(
      OBS_OBJECT_WORK gimmick_obj_work,
      OBS_OBJECT_WORK player_obj_work)
    {
        return player_obj_work.pos.x >= gimmick_obj_work.pos.x;
    }

    private static void gmGmkDrainTankOutAdjustPlayerOffsetBuoyancy(
      GMS_GMK_DRAIN_TANK_OUT_WORK out_work)
    {
        int num = gmGmkDrainTankGameSystemGetWaterLevel() * 4096 + 131072;
        if (num > out_work.base_pos_y)
            num = out_work.base_pos_y;
        int a = num - (out_work.base_pos_y + out_work.player_offset_y);
        if (MTM_MATH_ABS(a) > 2048)
            a = a >= 0 ? 2048 : -2048;
        out_work.player_offset_y += a;
    }

    private static void gmGmkDrainTankOutAdjustPlayerOffsetWave(
      GMS_GMK_DRAIN_TANK_OUT_WORK out_work,
      OBS_OBJECT_WORK player_obj_work)
    {
        int num = out_work.camera_roll >> 2;
        if (((int)player_obj_work.move_flag & 4) != 0)
        {
            if (((int)player_obj_work.disp_flag & 1) != 0)
                out_work.player_offset_x += FX_F32_TO_FX32(0.4f);
            else
                out_work.player_offset_x -= FX_F32_TO_FX32(0.4f);
        }
        else
        {
            if (out_work.camera_roll < 0)
                player_obj_work.disp_flag |= 1U;
            else if (out_work.camera_roll > 0)
                player_obj_work.disp_flag &= 4294967294U;
            out_work.player_offset_x += num;
        }
    }

    private static void gmGmkDrainTankOutApplyPlayerOffset(
      OBS_OBJECT_WORK player_obj_work,
      GMS_GMK_DRAIN_TANK_OUT_WORK out_work)
    {
        int num1 = out_work.base_pos_x + out_work.player_offset_x;
        int num2 = out_work.base_pos_y + out_work.player_offset_y;
        player_obj_work.spd.x = num1 - player_obj_work.pos.x;
        player_obj_work.spd.y = num2 - player_obj_work.pos.y;
    }

    private static void gmGmkDrainTankOutUpdateCameraRoll(
      GMS_GMK_DRAIN_TANK_OUT_WORK out_work,
      int rot_z)
    {
        if (rot_z < -8192)
        {
            if (out_work.camera_roll <= -GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX)
                out_work.camera_roll = -GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX;
            else
                out_work.camera_roll -= GMD_GMK_DRAIN_TANK_ROLL_ANGLE_SPEED;
        }
        else
        {
            if (rot_z <= 8192)
                return;
            if (out_work.camera_roll >= GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX)
                out_work.camera_roll = GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX;
            else
                out_work.camera_roll += GMD_GMK_DRAIN_TANK_ROLL_ANGLE_SPEED;
        }
    }

    private static void gmGmkDrainTankOutUpdateCameraRollDamage(
      GMS_GMK_DRAIN_TANK_OUT_WORK out_work)
    {
        int num = GMD_GMK_DRAIN_TANK_ROLL_ANGLE_SPEED * 4;
        if (out_work.flag_dir_left)
        {
            out_work.camera_roll += num;
            if (out_work.camera_roll >= -GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX)
                return;
            out_work.camera_roll = -GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX;
        }
        else
        {
            out_work.camera_roll -= num;
            if (out_work.camera_roll <= GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX)
                return;
            out_work.camera_roll = GMD_GMK_DRAIN_TANK_ROLL_ANGLE_MAX;
        }
    }

    private static void gmGmkDrainTankOutUpdateCameraRollDie(
      GMS_GMK_DRAIN_TANK_OUT_WORK out_work)
    {
        out_work.camera_roll -= out_work.camera_roll / 5;
    }

    private static void gmGmkDrainTankOutUpdateCameraOffset(
      GMS_PLAYER_WORK player_work,
      GMS_GMK_DRAIN_TANK_OUT_WORK out_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)player_work;
        float f32 = FX_FX32_TO_F32(out_work.base_pos_x - 737280 - obsObjectWork.pos.x);
        GmPlayerCameraOffsetSet(player_work, (short)f32, 0);
    }

    private static bool gmGmkDrainTankOutCheckKeyDir(
      OBS_OBJECT_WORK gimmick_obj_work,
      int camera_roll)
    {
        GMS_GMK_DRAIN_TANK_OUT_WORK drainTankOutWork = (GMS_GMK_DRAIN_TANK_OUT_WORK)gimmick_obj_work;
        return camera_roll < 0 && drainTankOutWork.flag_dir_left || camera_roll > 0 && !drainTankOutWork.flag_dir_left;
    }

    private static void gmGmkDrainTankOutSinkRing()
    {
        for (GMS_RING_WORK gmsRingWork = GmRingGetWork().damage_ring_list_start; gmsRingWork != null; gmsRingWork = gmsRingWork.post_ring)
        {
            gmsRingWork.spd_y = 4096;
            gmsRingWork.spd_x /= 2;
        }
    }

    private static void gmGmkDrainTankSplashMainFunc(OBS_OBJECT_WORK obj_work)
    {
        --obj_work.user_timer;
        if (obj_work.user_timer > 0)
        {
            float num = obj_work.user_timer / 60f;
            obj_work.obj_3d.draw_state.alpha.alpha = num;
        }
        else
        {
            obj_work.disp_flag |= 16U;
            obj_work.ppFunc = null;
        }
    }

    private static void gmGmkDrainTankSplashEffectMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.dir.z += 384;
    }

}