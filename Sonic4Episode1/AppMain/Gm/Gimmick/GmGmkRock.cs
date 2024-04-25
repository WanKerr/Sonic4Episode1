public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkRockChaseManagerInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK obsObjectWork = GmGmkRockHookInit(eve_rec, pos_x, pos_y, type);
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obsObjectWork;
        ((GMS_GMK_ROCK_CHASE_WORK)GmEventMgrLocalEventBirth(307, pos_x, pos_y, eve_rec.flag, eve_rec.left, eve_rec.top, eve_rec.width, eve_rec.height, 0)).hook_work = gmsEnemy3DWork;
        return obsObjectWork;
    }

    private static OBS_OBJECT_WORK GmGmkRockFallManagerInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_ROCK_FALL_MGR_WORK mgr_work = (GMS_GMK_ROCK_FALL_MGR_WORK)gmGmkRockLoadObjNoModel(eve_rec, pos_x, pos_y, type, () => new GMS_GMK_ROCK_FALL_MGR_WORK());
        OBS_OBJECT_WORK objWork = mgr_work.enemy_work.ene_com.obj_work;
        gmGmkRockManagerInit(objWork);
        gmGmkRockFallMgrSetInterval(mgr_work, eve_rec.left * 60);
        gmGmkRockFallMgrSetUserTimer(objWork, eve_rec.left * 60);
        OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(306, objWork.pos.x, objWork.pos.y - eve_rec.top * 2 * 4096 + 262144, eve_rec.flag, eve_rec.left, eve_rec.top, eve_rec.width, eve_rec.height, 0);
        obsObjectWork.flag |= 16U;
        obsObjectWork.parent_obj = objWork;
        mgr_work.hook_work = (GMS_ENEMY_3D_WORK)obsObjectWork;
        return objWork;
    }

    private static OBS_OBJECT_WORK GmGmkRockFallInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_ROCK_FALL_WORK gmsGmkRockFallWork = (GMS_GMK_ROCK_FALL_WORK)gmGmkRockLoadObj(eve_rec, pos_x, pos_y, type, () => new GMS_GMK_ROCK_FALL_WORK());
        OBS_OBJECT_WORK objWork = gmsGmkRockFallWork.enemy_work.ene_com.obj_work;
        gmGmkRockFallInit(objWork);
        gmsGmkRockFallWork.wait_time = type == 0 ? 0 : 30;
        return objWork;
    }

    private static OBS_OBJECT_WORK GmGmkRockHookInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkRockLoadObjHook(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        gmGmkRockHookInit(objWork);
        return objWork;
    }

    private static OBS_OBJECT_WORK GmGmkRockChaseInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_ROCK_CHASE_WORK rock_work = (GMS_GMK_ROCK_CHASE_WORK)gmGmkRockLoadObj(eve_rec, pos_x, pos_y, type, () => new GMS_GMK_ROCK_CHASE_WORK());
        OBS_OBJECT_WORK objWork = rock_work.enemy_work.ene_com.obj_work;
        gmGmkRockChaseInit(objWork);
        gmGmkRockChaseSetLength(rock_work, eve_rec.left * 2 * 4096);
        gmGmkRockChaseSetSpeed(rock_work, eve_rec.top * 2 * 4096);
        return objWork;
    }

    public static void GmGmkRockBuild()
    {
        g_gm_gmk_rock_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(816)), readAMBFile(GmGameDatGetGimmickData(817)), 0U);
    }

    public static void GmGmkRockFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(816));
        GmGameDBuildRegFlushModel(g_gm_gmk_rock_obj_3d_list, amsAmbHeader.file_num);
    }

    private static GMS_ENEMY_3D_WORK gmGmkRockLoadObjNoModel(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      TaskWorkFactoryDelegate work_size)
    {
        GMS_ENEMY_3D_WORK work = (GMS_ENEMY_3D_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, work_size, "GMK_ROCK");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static GMS_ENEMY_3D_WORK gmGmkRockLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      TaskWorkFactoryDelegate work_size)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkRockLoadObjNoModel(eve_rec, pos_x, pos_y, type, work_size);
        OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        ObjObjectCopyAction3dNNModel(objWork, g_gm_gmk_rock_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return gmsEnemy3DWork;
    }

    private static GMS_ENEMY_3D_WORK gmGmkRockLoadObjHook(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int pos_y1 = pos_y >> 17 << 17;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = gmGmkRockLoadObjNoModel(eve_rec, pos_x, pos_y1, type, () => new GMS_ENEMY_3D_WORK());
        OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        ObjObjectCopyAction3dNNModel(objWork, g_gm_gmk_rock_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(objWork, 0, false, ObjDataGet(818), null, 0, null);
        return gmsEnemy3DWork;
    }

    private static void gmGmkRockMoveFunc(OBS_OBJECT_WORK obj_work)
    {
        ObjObjectMove(obj_work);
    }

    private static void gmGmkRockFallDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        VecU16 vecU16 = new VecU16(obj_work.dir);
        ushort roll = ((GMS_GMK_ROCK_FALL_WORK)obj_work).roll;
        obj_work.dir.y = roll;
        ushort userWork = (ushort)obj_work.user_work;
        obj_work.dir.z = userWork;
        obj_work.dir.z += roll;
        ObjDrawActionSummary(obj_work);
        ObjDrawActionSummary(obj_work);
        obj_work.dir.Assign(vecU16);
    }

    private static void gmGmkRockChaseDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_ROCK_CHASE_WORK rock_work = (GMS_GMK_ROCK_CHASE_WORK)obj_work;
        VecU16 vecU16 = new VecU16(obj_work.dir);
        ushort angleZ = gmGmkRockChaseGetAngleZ(rock_work);
        obj_work.dir.z = obj_work.spd_m >= 0 ? angleZ : angleZ;
        ushort userWork = (ushort)obj_work.user_work;
        obj_work.dir.x = userWork;
        obj_work.pos.y += rock_work.current_bound;
        ObjDrawActionSummary(obj_work);
        ObjDrawActionSummary(obj_work);
        obj_work.dir.Assign(vecU16);
        obj_work.pos.y -= rock_work.current_bound;
    }

    private static void gmGmkRockChaseTcbDest(MTS_TASK_TCB tcb)
    {
        GMS_GMK_ROCK_CHASE_WORK tcbWork = (GMS_GMK_ROCK_CHASE_WORK)mtTaskGetTcbWork(tcb);
        if (tcbWork.se_handle != null)
        {
            GmSoundStopSE(tcbWork.se_handle);
            GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = null;
        }
        GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkRockWaitDefFunc(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj = own_rect.parent_obj;
        GMS_GMK_ROCK_CHASE_WORK gmkRockChaseWork = (GMS_GMK_ROCK_CHASE_WORK)parentObj;
        gmGmkRockHookkChangeModeActive((OBS_OBJECT_WORK)gmkRockChaseWork.hook_work);
        gmkRockChaseWork.hook_work = null;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctZoneEsCreate(parentObj, 2, 32);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = null;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
        gmsEffect3DesWork.efct_com.obj_work.parent_ofst.y = 204800;
        gmGmkRockChaseChangeModeFall(parentObj);
    }

    private static void gmGmkRockSetRectActive(GMS_ENEMY_3D_WORK gimmick_work)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        ObjRectWorkZSet(pRec, -40, -40, -500, 40, 40, 500);
        pRec.flag |= 1024U;
        ObjRectAtkSet(pRec, 2, 1);
        ObjRectDefSet(pRec, 0, 0);
        pRec.ppDef = null;
    }

    private static void gmGmkRockSetRectWait(GMS_ENEMY_3D_WORK gimmick_work)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        ObjRectWorkZSet(pRec, -40, -40, -500, 40, 500, 500);
        pRec.flag |= 1024U;
        ObjRectAtkSet(pRec, 0, 0);
        ObjRectDefSet(pRec, 65534, 0);
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkRockWaitDefFunc);
    }

    private static void gmGmkRockChaseInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gimmick_work = (GMS_ENEMY_3D_WORK)obj_work;
        gmGmkRockSetRectWait(gimmick_work);
        ObjObjectFieldRectSet(obj_work, -28, -28, 28, 42);
        obj_work.disp_flag |= 4194304U;
        gimmick_work.ene_com.target_obj = g_gm_main_system.ply_work[0].obj_work;
        obj_work.pos.z = -131072;
        GMS_GMK_ROCK_CHASE_WORK rock_work = (GMS_GMK_ROCK_CHASE_WORK)obj_work;
        ushort angle_z = mtMathRand();
        gmGmkRockChaseSetAngleZ(rock_work, angle_z);
        obj_work.user_work = mtMathRand();
        rock_work.se_handle = GsSoundAllocSeHandle();
        obj_work.ppFunc = null;
        obj_work.ppMove = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockMoveFunc);
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockChaseDrawFunc);
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkRockChaseTcbDest));
    }

    private static void gmGmkRockChaseChangeModeFall(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_ROCK_CHASE_WORK gmkRockChaseWork = (GMS_GMK_ROCK_CHASE_WORK)obj_work;
        gmGmkRockSetRectActive((GMS_ENEMY_3D_WORK)obj_work);
        obj_work.spd_m = 0;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        obj_work.dir.z = 0;
        obj_work.flag |= 16U;
        obj_work.move_flag |= 192U;
        obj_work.move_flag &= 4294443007U;
        if (gmkRockChaseWork.se_handle != null)
            GmSoundStopSE(gmkRockChaseWork.se_handle);
        if (gmkRockChaseWork.flag_vib)
        {
            GMM_PAD_VIB_STOP();
            gmkRockChaseWork.flag_vib = false;
        }
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockChaseMainFall);
    }

    private static void gmGmkRockChaseChangeModeChase(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_ROCK_CHASE_WORK rock_work = (GMS_GMK_ROCK_CHASE_WORK)obj_work;
        obj_work.spd_m = 0;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        gmGmkRockChaseSetDirType(rock_work, 0U);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockChaseMainChase);
        if (rock_work.effect_work == null)
        {
            GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctZoneEsCreate(obj_work, 2, 24);
            gmsEffect3DesWork.efct_com.obj_work.ppFunc = null;
            gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
            gmsEffect3DesWork.efct_com.obj_work.parent_ofst.y = 204800;
            rock_work.effect_work = gmsEffect3DesWork;
        }
        GmSoundPlaySE("BigRock2", rock_work.se_handle);
    }

    private static void gmGmkRockChaseChangeModeEnd(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_ROCK_CHASE_WORK gmkRockChaseWork = (GMS_GMK_ROCK_CHASE_WORK)obj_work;
        if (gmkRockChaseWork.flag_vib)
        {
            GMM_PAD_VIB_STOP();
            gmkRockChaseWork.flag_vib = false;
        }
        obj_work.flag &= 4294967279U;
        obj_work.move_flag |= 256U;
        obj_work.ppFunc = null;
    }

    private static void gmGmkRockChaseMainFall(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.move_flag & 1) != 0)
        {
            GmSoundPlaySE("BigRock1");
            GmCameraVibrationSet(0, 12288, 0);
            gmGmkRockChaseChangeModeChase(obj_work);
        }
        else
        {
            if (g_gm_main_system.ply_work[0].obj_work.pos.y >= obj_work.pos.y - 2097152)
                return;
            gmGmkRockChaseChangeModeEnd(obj_work);
        }
    }

    private static void gmGmkRockChaseMainChase(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_ROCK_CHASE_WORK rock_work = (GMS_GMK_ROCK_CHASE_WORK)obj_work;
        OBS_OBJECT_WORK targetObj = ((GMS_ENEMY_3D_WORK)obj_work).ene_com.target_obj;
        int a = targetObj.pos.x - obj_work.pos.x;
        uint type = gmGmkRockChaseGetDirType(rock_work);
        if (type == 0U)
        {
            type = obj_work.dir.z > NNM_DEGtoA16(180f) ? 2U : 1U;
            gmGmkRockChaseSetDirType(rock_work, type);
        }
        int num1;
        int num2;
        if (type == 1U && a < 0 || type == 2U && a >= 0)
        {
            num1 = 768;
            num2 = 32768;
            if (rock_work.flag_vib)
            {
                GMM_PAD_VIB_STOP();
                rock_work.flag_vib = false;
            }
        }
        else
        {
            int num3 = MTM_MATH_ABS(targetObj.spd_m);
            if (num3 < gmGmkRockChaseGetSpeed(rock_work))
            {
                num1 = 512;
                num2 = 65536;
            }
            else
            {
                int num4 = MTM_MATH_ABS(a);
                int length = gmGmkRockChaseGetLength(rock_work);
                int num5 = FX_Mul(length, 8192);
                if (num5 < 1228800)
                    num5 = 1228800;
                if (num4 > num5)
                {
                    num1 = 3840;
                    num2 = (int)(num3 + 32768L);
                    if (rock_work.flag_vib)
                    {
                        GMM_PAD_VIB_STOP();
                        rock_work.flag_vib = false;
                    }
                }
                else if (num4 > length)
                {
                    num1 = 768;
                    num2 = (int)(num3 + 10240L);
                    if (!rock_work.flag_vib)
                    {
                        GMM_PAD_VIB_MID_NOEND();
                        rock_work.flag_vib = true;
                    }
                }
                else
                {
                    num1 = -768;
                    num2 = (int)(num3 + -6144L);
                    if (!rock_work.flag_vib)
                    {
                        GMM_PAD_VIB_MID_NOEND();
                        rock_work.flag_vib = true;
                    }
                }
            }
        }
        if (type == 1U)
        {
            obj_work.spd_m += num1;
            gmGmkRockChaseAddAngleZ(rock_work, 1000);
            if (obj_work.spd_m > num2)
                obj_work.spd_m = num2;
        }
        else
        {
            int num3 = -num1;
            int num4 = -num2;
            obj_work.spd_m += num3;
            gmGmkRockChaseAddAngleZ(rock_work, -1000);
            if (obj_work.spd_m < num4)
                obj_work.spd_m = num4;
        }
        if (((int)obj_work.move_flag & 1) == 0)
            gmGmkRockChaseChangeModeFall(obj_work);
        else if (rock_work.current_bound >= 0)
        {
            rock_work.current_bound = 0;
            if (mtMathRand() % 10 != 0)
                return;
            int num3 = 32 + mtMathRand() % 16;
            rock_work.target_bound = -num3 * 4096;
            rock_work.current_bound -= 8192;
            if (rock_work.se_handle == null)
                return;
            GmSoundStopSE(rock_work.se_handle);
        }
        else if (rock_work.target_bound > rock_work.current_bound)
        {
            rock_work.target_bound = 0;
            rock_work.current_bound += 8192;
            if (rock_work.current_bound < 0)
                return;
            GmSoundPlaySE("BigRock1");
            GmSoundPlaySE("BigRock2", rock_work.se_handle);
            GmCameraVibrationSet(0, 12288, 0);
        }
        else
            rock_work.current_bound -= 8192;
    }

    private static void gmGmkRockManagerInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 8448U;
        gmGmkRockFallMgrSetUserTimer(obj_work, 0);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockManagerMain);
    }

    private static void gmGmkRockManagerMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_ROCK_FALL_MGR_WORK mgr_work = (GMS_GMK_ROCK_FALL_MGR_WORK)obj_work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        int interval = gmGmkRockFallMgrGetInterval(mgr_work);
        if (gmGmkRockFallMgrGetUserTimer(obj_work) >= interval)
        {
            gmGmkRockFallMgrSetUserTimer(obj_work, 0);
            byte type = 0;
            if (interval >= 120)
                type = 1;
            OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(300, obj_work.pos.x, obj_work.pos.y - gmsEnemy3DWork.ene_com.eve_rec.top * 2 * 4096, gmsEnemy3DWork.ene_com.eve_rec.flag, gmsEnemy3DWork.ene_com.eve_rec.left, gmsEnemy3DWork.ene_com.eve_rec.top, gmsEnemy3DWork.ene_com.eve_rec.width, gmsEnemy3DWork.ene_com.eve_rec.height, type);
            obsObjectWork.spd_fall = 336;
            obsObjectWork.spd_fall_max = 32768;
            ((GMS_GMK_ROCK_FALL_WORK)obsObjectWork).hook_work = mgr_work.hook_work;
        }
        gmGmkRockFallMgrAddUserTimer(obj_work, 1);
    }

    private static void gmGmkRockFallInit(OBS_OBJECT_WORK obj_work)
    {
        gmGmkRockSetRectActive((GMS_ENEMY_3D_WORK)obj_work);
        obj_work.move_flag |= 384U;
        obj_work.disp_flag |= 4194304U;
        obj_work.pos.z = -131072;
        obj_work.user_work = mtMathRand();
        GMS_GMK_ROCK_FALL_WORK gmsGmkRockFallWork = (GMS_GMK_ROCK_FALL_WORK)obj_work;
        gmsGmkRockFallWork.roll = mtMathRand();
        gmsGmkRockFallWork.roll_d = 128;
        if (gmsGmkRockFallWork.roll % 2 != 0)
            gmsGmkRockFallWork.roll_d = (ushort)-gmsGmkRockFallWork.roll_d;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockFallMainStart);
        obj_work.ppMove = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockMoveFunc);
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockFallDrawFunc);
    }

    private static void gmGmkRockFallMainStart(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK hookWork = (OBS_OBJECT_WORK)((GMS_GMK_ROCK_FALL_WORK)obj_work).hook_work;
        if (hookWork.pos.y + 98304 > obj_work.pos.y)
            return;
        GmEfctZoneEsCreate(obj_work, 2, 17).efct_com.obj_work.pos.z = 131072;
        GmSoundPlaySE("BigRock4");
        obj_work.pos.y = hookWork.pos.y + 98304;
        GmCameraVibrationSet(0, 4096, 0);
        obj_work.move_flag &= 4294967167U;
        obj_work.spd.y = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockFallMainWait);
    }

    private static void gmGmkRockFallMainWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_ROCK_FALL_WORK gmsGmkRockFallWork = (GMS_GMK_ROCK_FALL_WORK)obj_work;
        OBS_OBJECT_WORK hookWork = (OBS_OBJECT_WORK)gmsGmkRockFallWork.hook_work;
        ++obj_work.user_timer;
        if (obj_work.user_timer < gmsGmkRockFallWork.wait_time)
            return;
        obj_work.user_timer = 0;
        obj_work.move_flag |= 128U;
        gmGmkRockHookkChangeModeActive(hookWork);
        gmsGmkRockFallWork.hook_work = null;
        GmSoundPlaySE("BigRock5");
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockFallMainFallWaitEffect);
    }

    private static void gmGmkRockFallMainFallWaitEffect(OBS_OBJECT_WORK obj_work)
    {
        GMS_GMK_ROCK_FALL_WORK gmsGmkRockFallWork = (GMS_GMK_ROCK_FALL_WORK)obj_work;
        gmsGmkRockFallWork.roll += gmsGmkRockFallWork.roll_d;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 30)
            return;
        obj_work.user_timer = 0;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctZoneEsCreate(obj_work, 2, 32);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = null;
        gmsEffect3DesWork.efct_com.obj_work.pos.y -= 262144;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
        gmsEffect3DesWork.efct_com.obj_work.parent_ofst.y = 204800;
        gmsGmkRockFallWork.effect_work = gmsEffect3DesWork;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockFallMainFall);
    }

    private static void gmGmkRockFallMainFall(OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 128U;
        GMS_GMK_ROCK_FALL_WORK gmsGmkRockFallWork = (GMS_GMK_ROCK_FALL_WORK)obj_work;
        gmsGmkRockFallWork.roll += gmsGmkRockFallWork.roll_d;
    }

    private static void gmGmkRockHookInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        obj_work.pos.z = 0;
        gmGmkRockHookChangeModeWait(obj_work);
    }

    private static void gmGmkRockHookChangeModeWait(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawObjectActionSet3DNN(obj_work, 0, 0);
        obj_work.ppFunc = null;
    }

    private static void gmGmkRockHookkChangeModeActive(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawObjectActionSet3DNN(obj_work, 1, 0);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockHookMainActive);
    }

    private static void gmGmkRockHookMainActive(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        gmGmkRockHookChangeModeWait(obj_work);
    }

    private static void gmGmkRockChaseSetLength(GMS_GMK_ROCK_CHASE_WORK rock_work, int length)
    {
        rock_work.length = length;
    }

    private static int gmGmkRockChaseGetLength(GMS_GMK_ROCK_CHASE_WORK rock_work)
    {
        return rock_work.length;
    }

    private static void gmGmkRockChaseSetSpeed(GMS_GMK_ROCK_CHASE_WORK rock_work, int speed)
    {
        rock_work.speed = speed;
    }

    private static int gmGmkRockChaseGetSpeed(GMS_GMK_ROCK_CHASE_WORK rock_work)
    {
        return rock_work.speed;
    }

    private static void gmGmkRockChaseSetAngleZ(
      GMS_GMK_ROCK_CHASE_WORK rock_work,
      ushort angle_z)
    {
        rock_work.angle_z = angle_z;
    }

    private static void gmGmkRockChaseAddAngleZ(
      GMS_GMK_ROCK_CHASE_WORK rock_work,
      short angle_z)
    {
        rock_work.angle_z += (ushort)angle_z;
    }

    private static ushort gmGmkRockChaseGetAngleZ(GMS_GMK_ROCK_CHASE_WORK rock_work)
    {
        return rock_work.angle_z;
    }

    private static void gmGmkRockChaseSetDirType(GMS_GMK_ROCK_CHASE_WORK rock_work, uint type)
    {
        rock_work.dir_type = type;
    }

    private static uint gmGmkRockChaseGetDirType(GMS_GMK_ROCK_CHASE_WORK rock_work)
    {
        return rock_work.dir_type;
    }

    private static void gmGmkRockFallMgrSetInterval(
      GMS_GMK_ROCK_FALL_MGR_WORK mgr_work,
      int interval)
    {
        mgr_work.interval = interval;
    }

    private static int gmGmkRockFallMgrGetInterval(GMS_GMK_ROCK_FALL_MGR_WORK mgr_work)
    {
        return mgr_work.interval;
    }

    private static void gmGmkRockFallMgrSetUserTimer(OBS_OBJECT_WORK obj_work, int count)
    {
        obj_work.user_timer = count;
    }

    private static void gmGmkRockFallMgrAddUserTimer(OBS_OBJECT_WORK obj_work, int count)
    {
        obj_work.user_timer += count;
    }

    private static int gmGmkRockFallMgrGetUserTimer(OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_timer;
    }

}