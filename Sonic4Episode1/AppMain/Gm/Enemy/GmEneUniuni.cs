public partial class AppMain
{
    private static void GmEneUniuniBuild()
    {
        gm_ene_uniuni_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(693)), readAMBFile(GmGameDatGetEnemyData(694)), 0U);
    }

    private static void GmEneUniuniFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(693));
        GmGameDBuildRegFlushModel(gm_ene_uniuni_obj_3d_list, amsAmbHeader.file_num);
    }

    private static OBS_OBJECT_WORK GmEneUniuniInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_UNIUNI_WORK(), "ENE_UNIUNI");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_UNIUNI_WORK gmsEneUniuniWork = (GMS_ENE_UNIUNI_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_uniuni_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(695), null, 0, null);
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
        gmsEneUniuniWork.spd_dec = 76;
        gmsEneUniuniWork.spd_dec_dist = 15360;
        gmsEneUniuniWork.len = 17.5f;
        gmsEneUniuniWork.len_target = 35.5f;
        gmsEneUniuniWork.len_spd = 1f;
        gmsEneUniuniWork.rot_x = AKM_DEGtoA32(90f);
        gmsEneUniuniWork.rot_y = AKM_DEGtoA32(0.0f);
        gmsEneUniuniWork.rot_z = AKM_DEGtoA32(0.0f);
        gmsEneUniuniWork.num = 0;
        gmEneUniuniWalkInit(work);
        for (int index = 0; index < 4; ++index)
        {
            OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(310, pos_x, pos_y, 0, 0, 0, 0, 0, 0);
            obsObjectWork.parent_obj = work;
            ((GMS_ENE_UNIUNI_WORK)obsObjectWork).num = index;
            ++gmsEneUniuniWork.num;
        }
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static OBS_OBJECT_WORK GmEneUniuniNeedleInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_UNIUNI_WORK(), "ENE_UNIUNI");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_UNIUNI_WORK gmsEneUniuniWork = (GMS_ENE_UNIUNI_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_uniuni_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(695), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 655360;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -4, -4, 4, 4);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec2, -19, 0, 19, 32);
        pRec2.flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        work.move_flag &= 4294967167U;
        work.move_flag |= 256U;
        work.disp_flag |= 4194304U;
        gmEneUniuniNeedleWaitInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static int gmEneUniuniGetLength2N(OBS_OBJECT_WORK obj_work)
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

    private static void gmEneUniuniWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_UNIUNI_WORK gmsEneUniuniWork = (GMS_ENE_UNIUNI_WORK)obj_work;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUniuniWalkMain);
        obj_work.move_flag &= 4294967291U;
        gmsEneUniuniWork.timer = 1;
    }

    private static void gmEneUniuniWalkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_UNIUNI_WORK gmsEneUniuniWork = (GMS_ENE_UNIUNI_WORK)obj_work;
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 1536 : -1536;
        if (gmsEneUniuniWork.len_target == 17.5)
        {
            if (((int)obj_work.disp_flag & 1) != 0)
                gmsEneUniuniWork.rot_y += AKM_DEGtoA32(1);
            else
                gmsEneUniuniWork.rot_y += AKM_DEGtoA32(-1);
        }
        else
        {
            if (((int)obj_work.disp_flag & 1) != 0)
                gmsEneUniuniWork.rot_y += AKM_DEGtoA32(0.5f);
            else
                gmsEneUniuniWork.rot_y += AKM_DEGtoA32(-0.5f);
            obj_work.spd.x = 0;
        }
        if (gmsEneUniuniWork.len_target > (double)gmsEneUniuniWork.len)
        {
            gmsEneUniuniWork.len += gmsEneUniuniWork.len_spd;
            if (gmsEneUniuniWork.len_target <= (double)gmsEneUniuniWork.len)
                gmsEneUniuniWork.len = gmsEneUniuniWork.len_target;
            gmsEneUniuniWork.len_spd += 0.03f;
        }
        if (gmsEneUniuniWork.len_target < (double)gmsEneUniuniWork.len)
        {
            gmsEneUniuniWork.len -= gmsEneUniuniWork.len_spd;
            if (gmsEneUniuniWork.len_target >= (double)gmsEneUniuniWork.len)
                gmsEneUniuniWork.len = gmsEneUniuniWork.len_target;
            gmsEneUniuniWork.len_spd -= 0.05f;
            if (gmsEneUniuniWork.len_spd < 0.100000001490116)
                gmsEneUniuniWork.len_spd = 0.1f;
        }
        if (gmsEneUniuniWork.timer > 0)
            --gmsEneUniuniWork.timer;
        else if (gmsEneUniuniWork.len_target == 17.5)
        {
            gmsEneUniuniWork.timer = 120;
            gmsEneUniuniWork.len_spd = 0.0f;
            gmsEneUniuniWork.len_target = 35.5f;
        }
        else
        {
            if (gmsEneUniuniWork.len_target != 35.5)
                return;
            gmsEneUniuniWork.timer = 120;
            gmsEneUniuniWork.len_spd = 1f;
            gmsEneUniuniWork.len_target = 17.5f;
        }
    }

    private static void gmEneUniuniFwMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        gmEneUniuniFlipInit(obj_work);
    }

    private static void gmEneUniuniFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUniuniFlipMain);
    }

    private static void gmEneUniuniFlipMain(OBS_OBJECT_WORK obj_work)
    {
        gmEneUniuniSetWalkSpeed((GMS_ENE_UNIUNI_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        gmEneUniuniWalkInit(obj_work);
    }

    private static int gmEneUniuniSetWalkSpeed(GMS_ENE_UNIUNI_WORK uniuni_work)
    {
        return 0;
    }

    private static void gmEneUniuniNeedleWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUniuniNeedleWaitMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
    }

    private static void gmEneUniuniNeedleWaitMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_UNIUNI_WORK gmsEneUniuniWork = (GMS_ENE_UNIUNI_WORK)obj_work;
        GMS_ENE_UNIUNI_WORK parentObj = (GMS_ENE_UNIUNI_WORK)obj_work.parent_obj;
        int rotY = parentObj.rot_y;
        int rotX = parentObj.rot_x;
        int rotZ = parentObj.rot_z;
        float len = parentObj.len;
        int ay = (rotY + AKM_DEGtoA32(360) / 4 * gmsEneUniuniWork.num) % AKM_DEGtoA32(360);
        SNNS_MATRIX dst1;
        nnMakeRotateXMatrix(out dst1, rotX);
        nnRotateZMatrix(ref dst1, ref dst1, rotZ);
        nnRotateYMatrix(ref dst1, ref dst1, ay);
        SNNS_MATRIX dst2;
        nnMakeTranslateMatrix(out dst2, len, 0.0f, 0.0f);
        SNNS_MATRIX dst3;
        nnMultiplyMatrix(out dst3, ref dst1, ref dst2);
        SNNS_VECTOR dst4;
        nnCopyMatrixTranslationVector(out dst4, ref dst3);
        obj_work.pos.x = FX_F32_TO_FX32(dst4.x) + parentObj.ene_3d_work.ene_com.obj_work.pos.x;
        obj_work.pos.y = FX_F32_TO_FX32(dst4.y) + parentObj.ene_3d_work.ene_com.obj_work.pos.y;
        obj_work.pos.z = 655360;
        if (parentObj.attack == 0 || dst4.y < len * 0.98)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUniuniNeedleAttackInit);
    }

    private static void gmEneUniuniNeedleAttackInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_UNIUNI_WORK parentObj = (GMS_ENE_UNIUNI_WORK)obj_work.parent_obj;
        --parentObj.num;
        obj_work.spd.x = ((int)parentObj.ene_3d_work.ene_com.obj_work.disp_flag & 1) == 0 ? FX_F32_TO_FX32(1f) : -FX_F32_TO_FX32(1f);
        obj_work.parent_obj = null;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneUniuniNeedleAttackMain);
    }

    private static void gmEneUniuniNeedleAttackMain(OBS_OBJECT_WORK obj_work)
    {
        UNREFERENCED_PARAMETER(obj_work);
    }

}