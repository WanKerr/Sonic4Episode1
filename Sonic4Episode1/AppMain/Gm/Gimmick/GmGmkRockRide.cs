public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkRockRideInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK objWork = gmGmkRockRideLoadObj(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        gmGmkRockRideWaitInit(objWork);
        return objWork;
    }

    public static void GmGmkRockRideBuild()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(817));
        TXB_HEADER txb = readTXBfile(amBindGet(amsAmbHeader, 0));
        g_gm_gmk_rock_ride_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetGimmickData(816)), amsAmbHeader, 0U, txb);
    }

    public static void GmGmkRockRideFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(816));
        GmGameDBuildRegFlushModel(g_gm_gmk_rock_ride_obj_3d_list, amsAmbHeader.file_num);
        g_gm_gmk_rock_ride_obj_3d_list = null;
    }

    private static GMS_ENEMY_3D_WORK gmGmkRockRideLoadObj(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_GMK_ROCK_WORK work = (GMS_GMK_ROCK_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_ROCK_WORK(), "GMK_ROCK_RIDE");
        GMS_ENEMY_3D_WORK enemyWork = work.enemy_work;
        OBS_OBJECT_WORK objWork = work.enemy_work.ene_com.obj_work;
        enemyWork.ene_com.rect_work[0].flag &= 4294967291U;
        enemyWork.ene_com.rect_work[1].flag &= 4294967291U;
        ObjObjectCopyAction3dNNModel(objWork, g_gm_gmk_rock_ride_obj_3d_list[0], enemyWork.obj_3d);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return enemyWork;
    }

    private static void gmGmkRockRideMoveFunc(OBS_OBJECT_WORK obj_work)
    {
        ObjObjectMove(obj_work);
    }

    private static void gmGmkRockRideDrawFunc(OBS_OBJECT_WORK obj_work)
    {
        VecU16 vecU16 = new VecU16(obj_work.dir);
        ushort userTimerAngleZ = gmGmkRockRideGetUserTimerAngleZ(obj_work);
        obj_work.dir.z = obj_work.spd_m >= 0 ? userTimerAngleZ : userTimerAngleZ;
        ushort userWork = (ushort)obj_work.user_work;
        obj_work.dir.x = userWork;
        ObjDrawActionSummary(obj_work);
        obj_work.dir.Assign(vecU16);
    }

    private static void gmGmkRockRideTcbDest(MTS_TASK_TCB tcb)
    {
        GMS_GMK_ROCK_WORK tcbWork = (GMS_GMK_ROCK_WORK)mtTaskGetTcbWork(tcb);
        if (tcbWork.se_handle != null)
        {
            GmSoundStopSE(tcbWork.se_handle);
            GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = null;
        }
        GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkRockRideSetUserTimerAngleZ(
      OBS_OBJECT_WORK obj_work,
      ushort angle_z)
    {
        obj_work.user_timer = angle_z;
    }

    private static void gmGmkRockRideAddUserTimerAngleZ(
      OBS_OBJECT_WORK obj_work,
      short angle_z)
    {
        obj_work.user_timer += angle_z;
    }

    private static ushort gmGmkRockRideGetUserTimerAngleZ(OBS_OBJECT_WORK obj_work)
    {
        return (ushort)obj_work.user_timer;
    }

    private static void gmGmkRockRideWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gimmick_work = (GMS_ENEMY_3D_WORK)obj_work;
        gmGmkRockRideWaitSetRect(gimmick_work);
        obj_work.flag |= 1U;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        obj_work.spd_m = 0;
        gimmick_work.ene_com.target_obj = g_gm_main_system.ply_work[0].obj_work;
        ushort angle_z = mtMathRand();
        gmGmkRockRideSetUserTimerAngleZ(obj_work, angle_z);
        obj_work.user_work = mtMathRand();
        ((GMS_GMK_ROCK_WORK)obj_work).se_handle = GsSoundAllocSeHandle();
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockRideWaitMain);
        obj_work.ppMove = null;
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockRideDrawFunc);
        mtTaskChangeTcbDestructor(obj_work.tcb, new GSF_TASK_PROCEDURE(gmGmkRockRideTcbDest));
    }

    private static void gmGmkRockRideWaitMain(OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkRockRideWaitDefFunc(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj1 = own_rect.parent_obj;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)parentObj1;
        GMS_ENEMY_COM_WORK eneCom = gmsEnemy3DWork.ene_com;
        OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != 1)
            return;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)parentObj2;
        if (((int)parentObj2.move_flag & 1) != 0)
        {
            if (gmsEnemy3DWork.ene_com.eve_rec.flag != 0)
            {
                if (parentObj1.pos.x >= parentObj2.pos.x)
                    return;
            }
            else if (parentObj1.pos.x <= parentObj2.pos.x)
                return;
            GmPlySeqInitRockRideStart(ply_work, eneCom);
            gmGmkRockRideStartInit(parentObj1);
        }
        else
        {
            int num1 = parentObj1.pos.x - parentObj2.pos.x;
            int num2 = parentObj1.pos.y - parentObj2.pos.y;
            if (FX_Mul(num1, num1) + FX_Mul(num2, num2) > 12845056 || ply_work.seq_state != GME_PLY_SEQ_STATE_JUMP && ply_work.seq_state != GME_PLY_SEQ_STATE_JUMPDASH && (ply_work.seq_state != GME_PLY_SEQ_STATE_FALL && ply_work.seq_state != GME_PLY_SEQ_STATE_GMK_SPRINGJUMP))
                return;
            int spd_x = -(229376 - MTM_MATH_ABS(num1)) / 30;
            int spd_y = 0;
            if (parentObj1.pos.x < parentObj2.pos.x)
                spd_x = -spd_x;
            GmPlySeqInitPinballAir(ply_work, spd_x, spd_y, 60, 1, 0);
        }
    }

    private static void gmGmkRockRideWaitSetRect(GMS_ENEMY_3D_WORK gimmick_work)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        ObjRectWorkZSet(pRec, -48, -56, -500, 48, 56, 500);
        pRec.flag &= 4294966271U;
        ObjRectDefSet(pRec, 65534, 0);
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkRockRideWaitDefFunc);
    }

    private static void gmGmkRockRideStartInit(OBS_OBJECT_WORK obj_work)
    {
        gmGmkRockRideStartSetRect((GMS_ENEMY_3D_WORK)obj_work);
        ObjObjectFieldRectSet(obj_work, -16, -16, 16, 16);
        obj_work.flag &= 4294967294U;
        obj_work.move_flag &= 4294958847U;
        obj_work.move_flag |= 192U;
        obj_work.move_flag &= 4294836223U;
        obj_work.spd_m = 0;
        GmSoundPlaySE("BigRock3", ((GMS_GMK_ROCK_WORK)obj_work).se_handle);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockRideStartMain);
        obj_work.ppMove = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockRideMoveFunc);
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockRideDrawFunc);
    }

    private static void gmGmkRockRideStartMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK targetObj = ((GMS_ENEMY_3D_WORK)obj_work).ene_com.target_obj;
        gmGmkRockRideAddUserTimerAngleZ(obj_work, (short)(obj_work.spd_m >> 4));
        int num = 224;
        if (obj_work.pos.x < targetObj.pos.x)
            num = -num;
        obj_work.spd_m += num;
        if (MTM_MATH_ABS(obj_work.spd_m) > 12288)
            gmGmkRockRideRollInit(obj_work);
        float val = FX_FX32_TO_F32(FX_Div(MTM_MATH_ABS(obj_work.spd_m), 6));
        if (val > 1.0)
            val = 1f;
        GMS_GMK_ROCK_WORK gmsGmkRockWork = (GMS_GMK_ROCK_WORK)obj_work;
        if (gmsGmkRockWork.se_handle == null)
            return;
        gmsGmkRockWork.se_handle.au_player.SetAisac("Speed", val);
    }

    private static void gmGmkRockRideStartSetRect(GMS_ENEMY_3D_WORK gimmick_work)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        ObjRectDefSet(pRec, ushort.MaxValue, 3);
        pRec.ppDef = null;
    }

    private static void gmGmkRockRideRollInit(OBS_OBJECT_WORK obj_work)
    {
        gmGmkRockRideRollSetRect((GMS_ENEMY_3D_WORK)obj_work);
        ObjObjectFieldRectSet(obj_work, -16, -16, 16, 16);
        obj_work.flag &= 4294967294U;
        obj_work.move_flag &= 4294958847U;
        obj_work.move_flag |= 131264U;
        obj_work.spd_slope = 192;
        obj_work.spd_slope_max = 61440;
        obj_work.pos.z = 131072;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockRideRollMainNoPlayer);
        obj_work.ppMove = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockRideMoveFunc);
        obj_work.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockRideDrawFunc);
        GMS_GMK_ROCK_WORK gmsGmkRockWork = (GMS_GMK_ROCK_WORK)obj_work;
        if (gmsGmkRockWork.effect_work != null)
            return;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctZoneEsCreate(obj_work, 2, 18);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = null;
        gmsEffect3DesWork.efct_com.obj_work.parent_ofst.z = 98304;
        gmsEffect3DesWork.efct_com.obj_work.parent_ofst.y = 131072;
        gmsGmkRockWork.effect_work = gmsEffect3DesWork;
    }

    private static void gmGmkRockRideRollMain(OBS_OBJECT_WORK obj_work)
    {
        gmGmkRockRideAddUserTimerAngleZ(obj_work, (short)(obj_work.spd_m >> 4));
        float val = FX_FX32_TO_F32(FX_Div(MTM_MATH_ABS(obj_work.spd_m), 6));
        if (val > 1.0)
            val = 1f;
        GMS_GMK_ROCK_WORK gmsGmkRockWork = (GMS_GMK_ROCK_WORK)obj_work;
        if (gmsGmkRockWork.se_handle != null)
            gmsGmkRockWork.se_handle.au_player.SetAisac("Speed", val);
        if (((int)obj_work.move_flag & 4) != 0 || ((int)obj_work.move_flag & 8) != 0)
            gmGmkRockRideStopInit(obj_work);
        if (gmsGmkRockWork.vib_timer % 30 == 0)
            GMM_PAD_VIB_SMALL_TIME(10f);
        ++gmsGmkRockWork.vib_timer;
        if (g_gm_main_system.ply_work[0].seq_state == 31)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockRideRollMainNoPlayer);
        GMM_PAD_VIB_STOP();
        gmsGmkRockWork.vib_timer = 0;
        obj_work.pos.z = -262144;
    }

    private static void gmGmkRockRideRollMainNoPlayer(OBS_OBJECT_WORK obj_work)
    {
        gmGmkRockRideAddUserTimerAngleZ(obj_work, (short)(obj_work.spd_m >> 4));
        float val = FX_FX32_TO_F32(FX_Div(MTM_MATH_ABS(obj_work.spd_m), 6));
        if (val > 1.0)
            val = 1f;
        GMS_GMK_ROCK_WORK gmsGmkRockWork = (GMS_GMK_ROCK_WORK)obj_work;
        if (gmsGmkRockWork.se_handle != null)
            gmsGmkRockWork.se_handle.au_player.SetAisac("Speed", val);
        if (((int)obj_work.move_flag & 4) == 0 && ((int)obj_work.move_flag & 8) == 0)
            return;
        gmGmkRockRideStopInit(obj_work);
    }

    private static void gmGmkRockRideRollDefFunc(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj1 = own_rect.parent_obj;
        GMS_ENEMY_COM_WORK eneCom = ((GMS_ENEMY_3D_WORK)parentObj1).ene_com;
        OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != 1)
            return;
        GmPlySeqInitRockRide((GMS_PLAYER_WORK)parentObj2, eneCom);
        own_rect.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkRockRideRollDefFunc);
        parentObj1.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkRockRideRollMain);
        parentObj1.pos.z = 131072;
    }

    private static void gmGmkRockRideRollSetRect(GMS_ENEMY_3D_WORK gimmick_work)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        ObjRectWorkZSet(pRec, -48, -48, -500, 48, 48, 500);
        pRec.flag |= 1024U;
        ObjRectDefSet(pRec, 65534, 0);
        pRec.ppDef = new OBS_RECT_WORK_Delegate1(gmGmkRockRideRollDefFunc);
    }

    private static void gmGmkRockRideStopInit(OBS_OBJECT_WORK obj_work)
    {
        gmGmkRockRideStopSetRect((GMS_ENEMY_3D_WORK)obj_work);
        ObjObjectFieldRectSet(obj_work, -16, -16, 16, 16);
        obj_work.flag &= 4294967294U;
        obj_work.move_flag |= 256U;
        obj_work.move_flag &= 4294967294U;
        obj_work.spd_slope = 0;
        obj_work.spd_slope_max = 0;
        obj_work.spd_m = 0;
        GMS_GMK_ROCK_WORK gmsGmkRockWork = (GMS_GMK_ROCK_WORK)obj_work;
        if (gmsGmkRockWork.se_handle != null)
            GmSoundStopSE(gmsGmkRockWork.se_handle);
        obj_work.ppFunc = null;
        if (gmsGmkRockWork.effect_work != null)
            ObjDrawKillAction3DES((OBS_OBJECT_WORK)gmsGmkRockWork.effect_work);
        GMM_PAD_VIB_STOP();
    }

    private static void gmGmkRockRideStopSetRect(GMS_ENEMY_3D_WORK gimmick_work)
    {
        OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        pRec.flag |= 1024U;
        ObjRectDefSet(pRec, ushort.MaxValue, 3);
        pRec.ppDef = null;
    }


}