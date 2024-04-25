public partial class AppMain
{
    public static void GmEneKaniBuild()
    {
        gm_ene_kani_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(684)), readAMBFile(GmGameDatGetEnemyData(685)), 0U);
    }

    public static void GmEneKaniFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(684));
        GmGameDBuildRegFlushModel(gm_ene_kani_obj_3d_list, amsAmbHeader.file_num);
    }

    public static OBS_OBJECT_WORK GmEneKaniInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_KANI_WORK(), "ENE_KANI");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_KANI_WORK gmsEneKaniWork = (GMS_ENE_KANI_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_kani_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(686), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -11, -24, 11, 0);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        ObjRectWorkSet(pRec2, -19, -32, 19, 0);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -19, -32, 19, 0);
        pRec3.flag &= 4294967291U;
        ObjObjectFieldRectSet(work, -4, -8, 4, -2);
        work.disp_flag |= 4194304U;
        work.move_flag |= 128U;
        gmsEneKaniWork.walk_s = 0;
        if ((eve_rec.flag & 1) != 0)
            gmsEneKaniWork.walk_s = 1;
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        gmsEneKaniWork.spd_dec = 102;
        gmsEneKaniWork.spd_dec_dist = 20480;
        gmEneKaniWalkInit(work);
        GmEneUtilInitNodeMatrix(gmsEneKaniWork.node_work, work, 3);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmEneExit));
        GmEneUtilGetNodeMatrix(gmsEneKaniWork.node_work, 16);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        work.flag |= 1073741824U;
        gmsEneKaniWork.ata_futa = 0;
        return work;
    }

    public static int gmEneKaniGetLength2N(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
            return int.MaxValue;
        int x1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int x2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        float f32_1 = FX_FX32_TO_F32(x1);
        float f32_2 = FX_FX32_TO_F32(x2);
        return (int)(f32_1 * (double)f32_1 + f32_2 * (double)f32_2);
    }

    public static int gmEneKaniIsPlayerLeft(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        return GmEneComTargetIsLeft(obj_work, gmsPlayerWork.obj_work);
    }

    public static void gmEneKaniWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 2, 2);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKaniWalkMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
        OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec, -11, -24, 11, 0);
        pRec.flag |= 4U;
        GMS_ENE_KANI_WORK gmsEneKaniWork = (GMS_ENE_KANI_WORK)obj_work;
        if (gmsEneKaniWork.walk_s != 0)
            gmsEneKaniWork.timer = 15;
        else
            gmsEneKaniWork.timer = 10 + mtMathRand() % 20;
    }

    public static void gmEneKaniWalkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KANI_WORK gmsEneKaniWork = (GMS_ENE_KANI_WORK)obj_work;
        if (gmsEneKaniWork.ata_futa != 0)
        {
            if (gmsEneKaniWork.timer > 0)
            {
                --gmsEneKaniWork.timer;
                return;
            }
            obj_work.obj_3d.speed[0] = 2f;
            obj_work.disp_flag ^= 1U;
            obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKaniWalkMain);
            gmsEneKaniWork.timer = gmsEneKaniWork.walk_s == 0 ? 10 + mtMathRand() % 20 : 15;
        }
        else
        {
            obj_work.obj_3d.speed[0] = 1f;
            if (((int)obj_work.move_flag & 4) != 0 || GmEneComCheckMoveLimit(obj_work, (int)obj_work.user_work, (int)obj_work.user_flag) == 0)
            {
                gmEneKaniFlipInit(obj_work);
                gmsEneKaniWork.timer = 0;
            }
        }
        if (gmEneKaniIsPlayerLeft(obj_work) != 0)
        {
            gmsEneKaniWork.ata_futa = 0;
            if (gmEneKaniGetLength2N(obj_work) >= 8464)
                return;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKaniAttackInit);
        }
        else
            gmsEneKaniWork.ata_futa = 1;
    }

    public static void gmEneKaniAttackInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjDrawObjectActionSet(obj_work, 0);
        obj_work.disp_flag &= 4294967291U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKaniAttackMain);
        obj_work.spd.x = 0;
        GmSoundPlaySE(GMD_ENE_KANI_SE_PUNCH);
    }

    public static void gmEneKaniAttackMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_RECT_WORK pRec = ((GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1];
        NNS_MATRIX nodeMatrix = GmEneUtilGetNodeMatrix(((GMS_ENE_KANI_WORK)obj_work).node_work, 16);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.x = nodeMatrix.M03 - FX_FX32_TO_F32(obj_work.pos.x);
        nnsVector.y = nodeMatrix.M13 - FX_FX32_TO_F32(-obj_work.pos.y);
        nnsVector.z = nodeMatrix.M23 - FX_FX32_TO_F32(obj_work.pos.z);
        if (((int)obj_work.disp_flag & 1) != 0)
            nnsVector.x = -nnsVector.x;
        ObjRectWorkSet(pRec, (short)((short)nnsVector.x - 11), (short)(-24 - (short)nnsVector.y), (short)(11 + (short)nnsVector.x), (short)-nnsVector.y);
        pRec.flag |= 4U;
        if (GmBsCmnIsActionEnd(obj_work) != 0)
        {
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKaniAttackEnd);
            ObjDrawObjectActionSet(obj_work, 1);
        }
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    public static void gmEneKaniAttackEnd(OBS_OBJECT_WORK obj_work)
    {
        OBS_RECT_WORK pRec = ((GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1];
        NNS_MATRIX nodeMatrix = GmEneUtilGetNodeMatrix(((GMS_ENE_KANI_WORK)obj_work).node_work, 16);
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.x = nodeMatrix.M03 - FX_FX32_TO_F32(obj_work.pos.x);
        nnsVector.y = nodeMatrix.M13 - FX_FX32_TO_F32(-obj_work.pos.y);
        nnsVector.z = nodeMatrix.M23 - FX_FX32_TO_F32(obj_work.pos.z);
        if (((int)obj_work.disp_flag & 1) != 0)
            nnsVector.x = -nnsVector.x;
        ObjRectWorkSet(pRec, (short)((short)nnsVector.x - 11), (short)(-24 - (short)nnsVector.y), (short)(11 + (short)nnsVector.x), (short)-nnsVector.y);
        pRec.flag |= 4U;
        if (GmBsCmnIsActionEnd(obj_work) != 0)
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKaniWalkInit);
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
    }

    public static void gmEneKaniFwMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        gmEneKaniFlipInit(obj_work);
    }

    public static void gmEneKaniFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.spd.x = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKaniFlipMain);
    }

    public static void gmEneKaniFlipMain(OBS_OBJECT_WORK obj_work)
    {
        gmEneKaniSetWalkSpeed((GMS_ENE_KANI_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        gmEneKaniWalkInit(obj_work);
    }

    public static int gmEneKaniSetWalkSpeed(GMS_ENE_KANI_WORK kani_work)
    {
        return 0;
    }

}