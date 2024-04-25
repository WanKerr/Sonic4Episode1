public partial class AppMain
{
    private static void GmEneUnidesBuild()
    {
        gm_ene_unides_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(690)), readAMBFile(GmGameDatGetEnemyData(691)), 0U);
    }

    private static void GmEneUnidesFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(690));
        GmGameDBuildRegFlushModel(gm_ene_unides_obj_3d_list, amsAmbHeader.file_num);
    }

    private static OBS_OBJECT_WORK GmEneUnidesInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_UNIDES_WORK(), "ENE_UNIDES");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (GMS_ENE_UNIDES_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_unides_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(692), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -8, 0, 8, 16);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        ObjRectWorkSet(pRec2, -16, -8, 16, 16);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -19, -16, 19, 16);
        pRec3.flag &= 4294967291U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        if ((eve_rec.flag & 1) == 0)
        {
            work.disp_flag |= 1U;
            work.dir.y = (ushort)AKM_DEGtoA16(45);
        }
        else
            work.dir.y = (ushort)AKM_DEGtoA16(-45);
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        gmsEneUnidesWork.spd_dec = 76;
        gmsEneUnidesWork.spd_dec_dist = 15360;
        gmsEneUnidesWork.len = 17.5f;
        gmsEneUnidesWork.rot_x = AKM_DEGtoA32(90f);
        gmsEneUnidesWork.rot_y = AKM_DEGtoA32(0.0f);
        gmsEneUnidesWork.rot_z = AKM_DEGtoA32(0.0f);
        gmsEneUnidesWork.num = 0;
        gmEneUnidesWaitInit(work);
        for (int index = 0; index < 4; ++index)
        {
            OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(309, pos_x, pos_y, 0, 0, 0, 0, 0, 0);
            obsObjectWork.parent_obj = work;
            ((GMS_ENE_UNIDES_WORK)obsObjectWork).num = index;
            ++gmsEneUnidesWork.num;
        }
        gmsEneUnidesWork.attack = 0;
        gmsEneUnidesWork.attack_first = 0;
        gmsEneUnidesWork.zoom_now = 0;
        gmsEneUnidesWork.zoom = 1f;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static OBS_OBJECT_WORK GmEneUnidesNeedleInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_UNIDES_WORK(), "ENE_UNIDES");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (GMS_ENE_UNIDES_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_unides_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(692), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -6, -4, 6, 8);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec2, -19, 0, 19, 32);
        pRec2.flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        work.disp_flag |= 4194304U;
        gmEneUnidesNeedleWaitInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static int gmEneUnidesGetLength2N(OBS_OBJECT_WORK obj_work)
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

    private static void gmEneUnidesWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUnidesWaitMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
    }

    private static void gmEneUnidesWaitMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (GMS_ENE_UNIDES_WORK)obj_work;
        if (gmEneUnidesGetLength2N(obj_work) < 9216)
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUnidesAttackInit);
        if (((int)obj_work.disp_flag & 1) != 0)
            gmsEneUnidesWork.rot_y += AKM_DEGtoA32(1);
        else
            gmsEneUnidesWork.rot_y += AKM_DEGtoA32(-1);
    }

    private static void gmEneUnidesWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (GMS_ENE_UNIDES_WORK)obj_work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUnidesWalkMain);
        obj_work.move_flag &= 4294967291U;
        gmsEneUnidesWork.timer = 60;
    }

    private static void gmEneUnidesWalkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (GMS_ENE_UNIDES_WORK)obj_work;
        if (gmsEneUnidesWork.timer > 0)
            --gmsEneUnidesWork.timer;
        else if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -1536;
        else
            obj_work.spd.x = 1536;
    }

    private static void gmEneUnidesFwMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        gmEneUnidesFlipInit(obj_work);
    }

    private static void gmEneUnidesFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUnidesFlipMain);
    }

    private static void gmEneUnidesFlipMain(OBS_OBJECT_WORK obj_work)
    {
        gmEneUnidesSetWalkSpeed((GMS_ENE_UNIDES_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        gmEneUnidesWalkInit(obj_work);
    }

    private static void gmEneUnidesAttackInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUnidesAttackMain);
        obj_work.move_flag &= 4294967291U;
    }

    private static void gmEneUnidesAttackMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (GMS_ENE_UNIDES_WORK)obj_work;
        if (gmsEneUnidesWork.stop == 0)
        {
            if (((int)obj_work.disp_flag & 1) != 0)
                gmsEneUnidesWork.rot_y += AKM_DEGtoA32(1);
            else
                gmsEneUnidesWork.rot_y += AKM_DEGtoA32(-1);
        }
        gmsEneUnidesWork.attack = 1;
        if (gmsEneUnidesWork.zoom_now == 1)
        {
            gmsEneUnidesWork.zoom += 0.07f;
            if (gmsEneUnidesWork.zoom > 1.39999997615814)
                gmsEneUnidesWork.zoom_now = 2;
        }
        else if (gmsEneUnidesWork.zoom_now >= 2 && gmsEneUnidesWork.zoom_now <= 12)
        {
            ++gmsEneUnidesWork.zoom_now;
            gmsEneUnidesWork.zoom -= 0.07f;
        }
        else if (gmsEneUnidesWork.zoom_now >= 13 && gmsEneUnidesWork.zoom_now <= 23)
        {
            ++gmsEneUnidesWork.zoom_now;
            gmsEneUnidesWork.zoom += 0.07f;
        }
        else if (gmsEneUnidesWork.zoom > 1.0)
        {
            gmsEneUnidesWork.zoom -= 0.07f;
            if (gmsEneUnidesWork.zoom <= 1.0)
            {
                gmsEneUnidesWork.zoom = 1f;
                gmsEneUnidesWork.stop = 0;
                gmsEneUnidesWork.zoom_now = 0;
            }
        }
        obj_work.scale.x = FX_F32_TO_FX32(gmsEneUnidesWork.zoom);
        obj_work.scale.y = FX_F32_TO_FX32(gmsEneUnidesWork.zoom);
        obj_work.scale.z = FX_F32_TO_FX32(gmsEneUnidesWork.zoom);
        if (gmsEneUnidesWork.num != 0 || gmsEneUnidesWork.zoom != 1.0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUnidesWalkInit);
    }

    private static int gmEneUnidesSetWalkSpeed(GMS_ENE_UNIDES_WORK unides_work)
    {
        return 0;
    }

    private static void gmEneUnidesNeedleWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUnidesNeedleWaitMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
    }

    private static void gmEneUnidesNeedleWaitMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_UNIDES_WORK gmsEneUnidesWork = (GMS_ENE_UNIDES_WORK)obj_work;
        GMS_ENE_UNIDES_WORK parentObj = (GMS_ENE_UNIDES_WORK)obj_work.parent_obj;
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX nnsMatrix3 = GlobalPool<NNS_MATRIX>.Alloc();
        int rotY = parentObj.rot_y;
        int rotX = parentObj.rot_x;
        int rotZ = parentObj.rot_z;
        float len = parentObj.len;
        int ay = (rotY + AKM_DEGtoA32(360) / 4 * gmsEneUnidesWork.num) % AKM_DEGtoA32(360);
        nnMakeRotateXMatrix(nnsMatrix1, rotX);
        nnRotateZMatrix(nnsMatrix1, nnsMatrix1, rotZ);
        nnRotateYMatrix(nnsMatrix1, nnsMatrix1, ay);
        nnMakeTranslateMatrix(nnsMatrix2, len, 0.0f, 0.0f);
        nnMultiplyMatrix(nnsMatrix3, nnsMatrix1, nnsMatrix2);
        SNNS_VECTOR dst;
        nnCopyMatrixTranslationVector(out dst, nnsMatrix3);
        obj_work.pos.x = FX_F32_TO_FX32(dst.x) + parentObj.ene_3d_work.ene_com.obj_work.pos.x;
        obj_work.pos.y = FX_F32_TO_FX32(dst.y) + parentObj.ene_3d_work.ene_com.obj_work.pos.y;
        obj_work.pos.z = 655360;
        if (parentObj.attack != 0 && dst.y >= len * 0.98 && parentObj.stop == 0)
        {
            if (parentObj.attack_first != 0)
            {
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUnidesNeedleAttackInit);
            }
            else
            {
                parentObj.zoom_now = 1;
                parentObj.attack_first = 1;
                parentObj.stop = 1;
            }
        }
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix1);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix2);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix3);
    }

    private static void gmEneUnidesNeedleAttackInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_UNIDES_WORK parentObj = (GMS_ENE_UNIDES_WORK)obj_work.parent_obj;
        --parentObj.num;
        obj_work.spd.x = ((int)parentObj.ene_3d_work.ene_com.obj_work.disp_flag & 1) == 0 ? FX_F32_TO_FX32(1f) : -FX_F32_TO_FX32(1f);
        obj_work.parent_obj = null;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUnidesNeedleAttackMain);
    }

    private static void gmEneUnidesNeedleAttackMain(OBS_OBJECT_WORK obj_work)
    {
        UNREFERENCED_PARAMETER(obj_work);
    }


}