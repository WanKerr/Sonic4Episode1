public partial class AppMain
{
    public static void GmEneTStarBuild()
    {
        gm_ene_t_star_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(680)), readAMBFile(GmGameDatGetEnemyData(681)), 0U);
    }

    public static void GmEneTStarFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(680));
        GmGameDBuildRegFlushModel(gm_ene_t_star_obj_3d_list, amsAmbHeader.file_num);
    }

    public static OBS_OBJECT_WORK GmEneTStarInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_T_STAR_WORK(), "ENE_T_STAR");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_T_STAR_WORK gmsEneTStarWork = (GMS_ENE_T_STAR_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_t_star_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(682), null, 0, null);
        OBS_DATA_WORK data_work = ObjDataGet(683);
        ObjObjectAction3dNNMaterialMotionLoad(work, 0, data_work, null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -16, -16, 16, 16);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        ObjRectWorkSet(pRec2, -10, -10, 10, 10);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -20, -20, 20, 20);
        pRec3.flag &= 4294967291U;
        work.disp_flag |= 4194304U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        if ((eve_rec.flag & 7) == 0)
        {
            gmsEneTStarWork.fSpd = 1f;
        }
        else
        {
            gmsEneTStarWork.fSpd = 0.0f;
            if ((eve_rec.flag & 1) != 0)
                gmsEneTStarWork.fSpd += 0.5f;
            if ((eve_rec.flag & 2) != 0)
                gmsEneTStarWork.fSpd += 0.25f;
            if ((eve_rec.flag & 4) != 0)
                gmsEneTStarWork.fSpd += 0.125f;
        }
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        gmEneTStarWaitInit(work);
        GmEneUtilInitNodeMatrix(gmsEneTStarWork.node_work, work, 10);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmEneTStarExit));
        GmEneUtilGetNodeMatrix(gmsEneTStarWork.node_work, 4);
        GmEneUtilGetNodeMatrix(gmsEneTStarWork.node_work, 5);
        GmEneUtilGetNodeMatrix(gmsEneTStarWork.node_work, 6);
        GmEneUtilGetNodeMatrix(gmsEneTStarWork.node_work, 7);
        GmEneUtilGetNodeMatrix(gmsEneTStarWork.node_work, 8);
        ((GMS_ENEMY_3D_WORK)work).ene_com.enemy_flag |= 32768U;
        work.scale.x = FX_F32_TO_FX32(1.25f);
        work.scale.y = FX_F32_TO_FX32(1.25f);
        work.scale.z = FX_F32_TO_FX32(1.25f);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static OBS_OBJECT_WORK GmEneTStarNeedleInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_T_STAR_WORK(), "ENE_T_STAR");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_T_STAR_WORK gmsEneTStarWork = (GMS_ENE_T_STAR_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_t_star_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(682), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -11, -12, 11, 12);
        pRec1.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec2, -19, -16, 19, 16);
        pRec2.flag &= 4294967291U;
        work.disp_flag |= 4194304U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneTStarNeedleMain);
        work.scale.x = FX_F32_TO_FX32(1.25f);
        work.scale.y = FX_F32_TO_FX32(1.25f);
        work.scale.z = FX_F32_TO_FX32(1.25f);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static void gmEneTStarExit(MTS_TASK_TCB tcb)
    {
        GmEneUtilExitNodeMatrix(((GMS_ENE_T_STAR_WORK)mtTaskGetTcbWork(tcb)).node_work);
        GmEnemyDefaultExit(tcb);
    }

    public static int gmEneTStarGetLength2N(OBS_OBJECT_WORK obj_work)
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

    public static VecFx32 gmEneTStarGetPlayerVectorFx(OBS_OBJECT_WORK obj_work)
    {
        VecFx32 vecFx32 = new VecFx32();
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        int num1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int num2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
        {
            num1 = 2965504;
            num2 = 2965504;
        }
        int denom = FX_Sqrt(FX_Mul(num1, num1) + FX_Mul(num2, num2));
        if (denom == 0)
        {
            vecFx32.x = 0;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = FX_Div(4096, denom);
            vecFx32.x = FX_Mul(num1, v2);
            vecFx32.y = FX_Mul(num2, v2);
        }
        vecFx32.z = 0;
        return vecFx32;
    }

    public static void gmEneTStarWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneTStarWaitMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
    }

    public static void gmEneTStarWaitMain(OBS_OBJECT_WORK obj_work)
    {
        if (gmEneTStarGetLength2N(obj_work) >= 16384)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneTStarWalkInit);
    }

    public static void gmEneTStarWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_ENE_T_STAR_WORK gmsEneTStarWork = (GMS_ENE_T_STAR_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneTStarWalkMain);
        obj_work.move_flag &= 4294967291U;
        VecFx32 playerVectorFx = gmEneTStarGetPlayerVectorFx(obj_work);
        obj_work.spd.x = (int)(playerVectorFx.x * 0.5 * gmsEneTStarWork.fSpd);
        obj_work.spd.y = (int)(playerVectorFx.y * 0.5 * gmsEneTStarWork.fSpd);
        gmsEneTStarWork.timer = 120;
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        gmsEneTStarWork.rotate = 0;
    }

    public static void gmEneTStarWalkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_T_STAR_WORK gmsEneTStarWork = (GMS_ENE_T_STAR_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        if (gmsEneTStarWork.rotate > 0)
        {
            obj_work.dir.z += (ushort)AKM_DEGtoA16(10);
            --gmsEneTStarWork.rotate;
            if (gmsEneTStarWork.rotate == 0)
                obj_work.dir.z = 0;
        }
        if (gmsEneTStarWork.timer > 0)
        {
            --gmsEneTStarWork.timer;
            if (gmsEneTStarWork.timer != 60)
                return;
            gmsEneTStarWork.rotate = 36;
        }
        else
        {
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
            gmsEneTStarWork.timer = 15;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneTStarStopMain);
        }
    }

    public static void gmEneTStarStopMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_T_STAR_WORK gmsEneTStarWork = (GMS_ENE_T_STAR_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        if (gmsEneTStarWork.timer > 0)
        {
            --gmsEneTStarWork.timer;
        }
        else
        {
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneTStarAttackInit);
            GmEfctEneEsCreate(obj_work, 11);
        }
    }

    public static void gmEneTStarAttackInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_T_STAR_WORK gmsEneTStarWork = (GMS_ENE_T_STAR_WORK)obj_work;
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
        nnMakeUnitMatrix(nnsMatrix1);
        nnMakeUnitMatrix(nnsMatrix2);
        nnMakeRotateZMatrix(nnsMatrix2, AKM_DEGtoA32(72));
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        for (int index = 0; index < 5; ++index)
        {
            OBS_OBJECT_WORK parent_obj = GmEventMgrLocalEventBirth(308, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
            parent_obj.parent_obj = obj_work;
            parent_obj.dir.y = 49152;
            parent_obj.dir.z = (ushort)AKM_DEGtoA16(-72 * index);
            nnsVector.x = nnsMatrix1.M01;
            nnsVector.y = nnsMatrix1.M11;
            nnsVector.z = 0.0f;
            parent_obj.spd.x = FX_F32_TO_FX32(nnsVector.x * 4f);
            parent_obj.spd.y = -FX_F32_TO_FX32(nnsVector.y * 4f);
            parent_obj.pos.x += FX_F32_TO_FX32(nnsVector.x * 10f);
            parent_obj.pos.y += -FX_F32_TO_FX32(nnsVector.y * 10f);
            nnMultiplyMatrix(nnsMatrix1, nnsMatrix1, nnsMatrix2);
            ((GMS_ENEMY_3D_WORK)parent_obj).ene_com.enemy_flag |= 32768U;
            GmEfctEneEsCreate(parent_obj, 10).efct_com.obj_work.dir.z = (ushort)AKM_DEGtoA16(-72 * index);
        }
        GlobalPool<NNS_VECTOR>.Release(nnsVector);
        obj_work.disp_flag |= 32U;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneTStarAttackMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        gmsEneTStarWork.timer = 300;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        GmSoundPlaySE(GMD_ENE_KAMA_SE_BOMB);
        gmsEnemy3DWork.ene_com.enemy_flag |= 65536U;
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix1);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix2);
    }

    public static void gmEneTStarAttackMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_T_STAR_WORK gmsEneTStarWork = (GMS_ENE_T_STAR_WORK)obj_work;
        if (gmsEneTStarWork.timer > 0)
            --gmsEneTStarWork.timer;
        else
            obj_work.flag |= 8U;
    }

    public static void gmEneTStarNeedleMain(OBS_OBJECT_WORK obj_work)
    {
        UNREFERENCED_PARAMETER(obj_work);
    }

    public static void gmEneTStarFwMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        gmEneTStarFlipInit(obj_work);
    }

    public static void gmEneTStarFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneTStarFlipMain);
    }

    public static void gmEneTStarFlipMain(OBS_OBJECT_WORK obj_work)
    {
        gmEneTStarSetWalkSpeed((GMS_ENE_T_STAR_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        gmEneTStarWalkInit(obj_work);
    }

    public static int gmEneTStarSetWalkSpeed(GMS_ENE_T_STAR_WORK t_star_work)
    {
        return 0;
    }


}