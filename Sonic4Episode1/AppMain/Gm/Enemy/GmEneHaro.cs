public partial class AppMain
{
    public static void GmEneHaroBuild()
    {
        gm_ene_haro_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(687)), readAMBFile(GmGameDatGetEnemyData(688)), 0U);
    }

    public static void GmEneHaroFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(687));
        GmGameDBuildRegFlushModel(gm_ene_haro_obj_3d_list, amsAmbHeader.file_num);
    }

    public static OBS_OBJECT_WORK GmEneHaroInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_HARO_WORK(), "ENE_HARO");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_HARO_WORK gmsEneHaroWork = (GMS_ENE_HARO_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_haro_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(689), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        work.disp_flag |= 4194304U;
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
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        if ((eve_rec.flag & GMD_ENE_HARO_EVE_FLAG_RIGHT) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        gmsEneHaroWork.spd_dec = GMD_ENE_HARO_MOVE_SPD_X / (GMD_ENE_HARO_TURN_FRAME / 2);
        gmsEneHaroWork.spd_dec_dist = GMD_ENE_HARO_MOVE_SPD_X * (GMD_ENE_HARO_TURN_FRAME / 2) / 2;
        gmsEneHaroWork.vec.x = 0;
        gmsEneHaroWork.vec.y = FX_F32_TO_FX32(1.0);
        gmsEneHaroWork.angle = 0;
        gmsEneHaroWork.angle_add = 0;
        gmsEneHaroWork.lighton = 0;
        GmEneUtilInitNodeMatrix(gmsEneHaroWork.node_work, work, 16);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmEneHaroExit));
        GmEneUtilGetNodeMatrix(gmsEneHaroWork.node_work, 2);
        gmEneHaroWaitInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static void gmEneHaroExit(MTS_TASK_TCB tcb)
    {
        GmEneUtilExitNodeMatrix(((GMS_ENE_HARO_WORK)mtTaskGetTcbWork(tcb)).node_work);
        GmEnemyDefaultExit(tcb);
    }

    public static int gmEneHaroGetLength2N(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & GMD_PLF_DIE) != 0)
            return int.MaxValue;
        int x1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int x2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        float f32_1 = FX_FX32_TO_F32(x1);
        float f32_2 = FX_FX32_TO_F32(x2);
        return (int)(f32_1 * (double)f32_1 + f32_2 * (double)f32_2);
    }

    public static int gmEneHaroIsPlayerLeft(GMS_ENE_HARO_WORK obj_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)g_gm_main_system.ply_work[0];
        if (((int)g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
            return 1;
        VecFx32 vecFx32 = new VecFx32();
        vecFx32.x = obsObjectWork.pos.x - obj_work.ene_3d_work.ene_com.obj_work.pos.x;
        vecFx32.y = obsObjectWork.pos.y - obj_work.ene_3d_work.ene_com.obj_work.pos.y;
        return FX_Mul(vecFx32.x, obj_work.vec.y) - FX_Mul(vecFx32.y, obj_work.vec.x) > 0 ? 0 : 1;
    }

    public static int gmEneHaroIsPlayerCenter(GMS_ENE_HARO_WORK obj_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)g_gm_main_system.ply_work[0];
        if (((int)g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
            return 1;
        VecFx32 vecFx32 = new VecFx32();
        vecFx32.x = obsObjectWork.pos.x - obj_work.ene_3d_work.ene_com.obj_work.pos.x;
        vecFx32.y = obsObjectWork.pos.y - obj_work.ene_3d_work.ene_com.obj_work.pos.y;
        int num = FX_Mul(vecFx32.x, obj_work.vec.y) - FX_Mul(vecFx32.y, obj_work.vec.x);
        return num < FX_F32_TO_FX32(0.2f) && num > -FX_F32_TO_FX32(0.2f) ? 1 : 0;
    }

    public static int gmEneHaroIsPlayerFront(GMS_ENE_HARO_WORK obj_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)g_gm_main_system.ply_work[0];
        if (((int)g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
            return 1;
        VecFx32 vecFx32 = new VecFx32();
        vecFx32.x = obsObjectWork.pos.x - obj_work.ene_3d_work.ene_com.obj_work.pos.x;
        vecFx32.y = obsObjectWork.pos.y - obj_work.ene_3d_work.ene_com.obj_work.pos.y;
        return FX_Mul(vecFx32.x, obj_work.vec.x) + FX_Mul(vecFx32.y, obj_work.vec.y) > 0 ? 1 : 0;
    }

    public static void gmEneHaroWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_HARO_WORK gmsEneHaroWork = (GMS_ENE_HARO_WORK)obj_work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjDrawObjectActionSet(obj_work, 0);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneHaroWaitMain);
        obj_work.move_flag &= 4294967291U;
        int denom = FX_Sqrt(FX_Mul(gmsEneHaroWork.vec.x, gmsEneHaroWork.vec.x) + FX_Mul(gmsEneHaroWork.vec.y, gmsEneHaroWork.vec.y));
        if (denom == 0)
        {
            gmsEneHaroWork.vec.x = 0;
            gmsEneHaroWork.vec.y = FX_F32_TO_FX32(1f);
        }
        else
        {
            gmsEneHaroWork.vec.x = FX_Div(gmsEneHaroWork.vec.x, denom);
            gmsEneHaroWork.vec.y = FX_Div(gmsEneHaroWork.vec.y, denom);
        }
    }

    public static void gmEneHaroWaitMain(OBS_OBJECT_WORK obj_work)
    {
        if (gmEneHaroGetLength2N(obj_work) > 10000)
            return;
        GmSoundPlaySE("Halogen");
        obj_work.obj_3d.blend_spd = 0.05f;
        ObjDrawObjectActionSet3DNNBlend(obj_work, 1);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneHaroWalkInit);
    }

    public static void gmEneHaroWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_HARO_WORK gmsEneHaroWork = (GMS_ENE_HARO_WORK)obj_work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneHaroWalkMain);
        obj_work.move_flag &= 4294967291U;
        int denom = FX_Sqrt(FX_Mul(gmsEneHaroWork.vec.x, gmsEneHaroWork.vec.x) + FX_Mul(gmsEneHaroWork.vec.y, gmsEneHaroWork.vec.y));
        if (denom == 0)
        {
            gmsEneHaroWork.vec.x = 0;
            gmsEneHaroWork.vec.y = FX_F32_TO_FX32(1f);
        }
        else
        {
            gmsEneHaroWork.vec.x = FX_Div(gmsEneHaroWork.vec.x, denom);
            gmsEneHaroWork.vec.y = FX_Div(gmsEneHaroWork.vec.y, denom);
        }
        gmsEneHaroWork.timer = 120;
        if (gmsEneHaroWork.lighton == 0)
        {
            GmEfctEneEsCreate(obj_work, 6).efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneEffectMainFuncHarogen);
            gmsEneHaroWork.lighton = 1;
        }
        if (gmEneHaroGetLength2N(obj_work) <= 10000)
        {
            GmSoundPlaySE("Halogen");
            ObjDrawObjectActionSet(obj_work, 1);
            obj_work.disp_flag |= 4U;
        }
        else
        {
            ObjDrawObjectActionSet(obj_work, 1);
            obj_work.disp_flag |= 4U;
        }
    }

    public static void gmEneHaroWalkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_HARO_WORK obj_work1 = (GMS_ENE_HARO_WORK)obj_work;
        if (gmEneHaroIsPlayerCenter(obj_work1) == 0)
        {
            if (gmEneHaroIsPlayerLeft(obj_work1) != 0)
            {
                obj_work1.angle_add -= AKM_DEGtoA32(0.03f);
                if (obj_work1.angle_add < -AKM_DEGtoA32(0.35f))
                    obj_work1.angle_add = -AKM_DEGtoA32(0.35f);
                obj_work1.angle += obj_work1.angle_add;
            }
            else
            {
                obj_work1.angle_add += AKM_DEGtoA32(0.03f);
                if (obj_work1.angle_add > AKM_DEGtoA32(0.35f))
                    obj_work1.angle_add = AKM_DEGtoA32(0.35f);
                obj_work1.angle += obj_work1.angle_add;
            }
            if (obj_work1.angle < -AKM_DEGtoA32(1.3f))
                obj_work1.angle = -AKM_DEGtoA32(1.3f);
            if (obj_work1.angle > AKM_DEGtoA32(1.3f))
                obj_work1.angle = AKM_DEGtoA32(1.3f);
        }
        int v2_1 = FX_Cos(obj_work1.angle);
        int v2_2 = FX_Sin(obj_work1.angle);
        obj_work1.vec.x = FX_Mul(obj_work1.vec.x, v2_1) + FX_Mul(obj_work1.vec.y, v2_2);
        obj_work1.vec.y = FX_Mul(obj_work1.vec.x, -v2_2) + FX_Mul(obj_work1.vec.y, v2_1);
        obj_work1.vvv.x = (int)(obj_work1.vvv.x * 0.959999978542328);
        obj_work1.vvv.y = (int)(obj_work1.vvv.x * 0.959999978542328);
        obj_work1.vvv.x += obj_work1.vec.x;
        obj_work1.vvv.y += obj_work1.vec.y;
        obj_work1.spd = FX_F32_TO_FX32(1.5f);
        obj_work.spd.x = FX_Mul(obj_work1.vec.x, obj_work1.spd);
        obj_work.spd.y = FX_Mul(obj_work1.vec.y, obj_work1.spd);
        obj_work.spd.x += FX_Mul(obj_work1.vvv.x, FX_F32_TO_FX32(0.025));
        obj_work.spd.y += FX_Mul(obj_work1.vvv.y, FX_F32_TO_FX32(0.025));
        if (obj_work1.timer > 0)
            --obj_work1.timer;
        else
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneHaroWalkInit);
        if (obj_work1.vec.x < 0)
        {
            obj_work.disp_flag &= 4294967294U;
            obj_work1.targetAngle = AKM_DEGtoA32(250);
        }
        else
        {
            obj_work.disp_flag &= 4294967294U;
            obj_work1.targetAngle = AKM_DEGtoA32(330);
        }
        if (obj_work.dir.y > obj_work1.targetAngle)
            obj_work.dir.y -= (ushort)AKM_DEGtoA32(5);
        if (obj_work.dir.y >= obj_work1.targetAngle)
            return;
        obj_work.dir.y += (ushort)AKM_DEGtoA32(5);
    }

    public static void gmEneHaroFwMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        gmEneHaroFlipInit(obj_work);
    }

    public static void gmEneHaroFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneHaroFlipMain);
    }

    public static void gmEneHaroFlipMain(OBS_OBJECT_WORK obj_work)
    {
        gmEneHaroSetWalkSpeed((GMS_ENE_HARO_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        gmEneHaroWalkInit(obj_work);
    }

    public static int gmEneHaroSetWalkSpeed(GMS_ENE_HARO_WORK haro_work)
    {
        return 0;
    }

    public static void gmEneEffectMainFuncHarogen(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.parent_obj != null)
        {
            GMS_ENE_HARO_WORK parentObj = (GMS_ENE_HARO_WORK)obj_work.parent_obj;
            NNS_MATRIX nodeMatrix = GmEneUtilGetNodeMatrix(parentObj.node_work, 2);
            if (nodeMatrix != null)
            {
                float num1 = nodeMatrix.M03 - FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.pos.x);
                float num2 = nodeMatrix.M13 + FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.pos.y);
                float num3 = nodeMatrix.M23 - FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.pos.z);
                float x1 = num1 + FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.spd.x);
                float num4 = num2 - FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.spd.y);
                float x2 = num3 + FX_FX32_TO_F32(parentObj.ene_3d_work.ene_com.obj_work.spd.z);
                obj_work.parent_ofst.x = FX_F32_TO_FX32(x1);
                obj_work.parent_ofst.y = -FX_F32_TO_FX32(num4 - 10f);
                obj_work.parent_ofst.z = FX_F32_TO_FX32(x2);
            }
        }
        else
            obj_work.flag |= 4U;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }
}