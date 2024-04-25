public partial class AppMain
{
    private static void GmGmkCapsuleBuild()
    {
        gm_gmk_capsule_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(860), GmGameDatGetGimmickData(861), 0U);
    }

    private static void GmGmkCapsuleFlush()
    {
        AMS_AMB_HEADER gimmickData = GmGameDatGetGimmickData(860);
        GmGameDBuildRegFlushModel(gm_gmk_capsule_obj_3d_list, gimmickData.file_num);
    }

    private static OBS_OBJECT_WORK GmGmkCapsuleInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work1 = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_CAPSULE");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork1 = (GMS_ENEMY_3D_WORK)work1;
        ObjObjectCopyAction3dNNModel(work1, gm_gmk_capsule_obj_3d_list[2], gmsEnemy3DWork1.obj_3d);
        work1.pos.z = -393216;
        work1.move_flag |= 8448U;
        work1.disp_flag |= 4194304U;
        OBS_COLLISION_WORK colWork = gmsEnemy3DWork1.ene_com.col_work;
        colWork.obj_col.obj = work1;
        colWork.obj_col.width = 32;
        colWork.obj_col.height = 40;
        colWork.obj_col.ofst_x = (short)(-colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = -76;
        gmsEnemy3DWork1.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork1.ene_com.rect_work[1].flag &= 4294967291U;
        OBS_RECT_WORK pRec = gmsEnemy3DWork1.ene_com.rect_work[2];
        pRec.ppHit = null;
        pRec.ppDef = null;
        ObjRectAtkSet(pRec, 0, 0);
        ObjRectDefSet(pRec, 65534, 0);
        ObjRectWorkSet(pRec, -4, -80, 4, -72);
        work1.user_flag = (uint)(work1.user_flag & 18446744073709551614UL);
        work1.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCapsuleSwitchMain);
        OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(301, pos_x, pos_y, gmsEnemy3DWork1.ene_com.eve_rec.flag, gmsEnemy3DWork1.ene_com.eve_rec.left, gmsEnemy3DWork1.ene_com.eve_rec.top, gmsEnemy3DWork1.ene_com.eve_rec.width, gmsEnemy3DWork1.ene_com.eve_rec.height, 0);
        obsObjectWork.parent_obj = work1;
        obsObjectWork.view_out_ofst = work1.view_out_ofst;
        GMS_ENEMY_3D_WORK gmsEnemy3DWork2 = (GMS_ENEMY_3D_WORK)obsObjectWork;
        OBS_OBJECT_WORK work2 = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DNN_WORK(), work1, 0, "GMK_CAPSULE_BODY");
        GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (GMS_EFFECT_3DNN_WORK)work2;
        ObjObjectCopyAction3dNNModel(work2, gm_gmk_capsule_obj_3d_list[1], gmsEffect3DnnWork.obj_3d);
        work2.pos.z = -131072;
        work2.move_flag |= 8448U;
        work2.disp_flag |= 4194304U;
        work2.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCapsuleKeyMain);
        return work1;
    }

    private static OBS_OBJECT_WORK GmGmkCapsuleBodyInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_CAPSULE_BODY");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_capsule_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, false, ObjDataGet(862), null, 0, null);
        ObjDrawObjectActionSet(work, 0);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work;
        colWork.obj_col.width = 64;
        colWork.obj_col.height = 60;
        colWork.obj_col.ofst_x = (short)(-colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-colWork.obj_col.height;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkCapsuleBodyMain);
        return work;
    }

    private static void gmGmkCapsuleSwitchMain(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.col_work.obj_col.rider_obj != null)
        {
            obj_work.ofst.y = 24576;
            if (((int)obj_work.user_flag & 1) == 0)
            {
                g_gm_main_system.game_flag &= 4294966271U;
                g_gm_main_system.game_flag |= 1048576U;
                GmEffect3DESSetDispOffset(GmEfctCmnEsCreate(obj_work, 23), 0.0f, 24f, 40f);
                gmGmkCapsuleAnimalMake(obj_work);
                obj_work.user_timer = 1;
                GmGmkCamScrLimitSet(new GMS_EVE_RECORD_EVENT()
                {
                    flag = 7,
                    left = -96,
                    top = -104,
                    width = 192,
                    height = 112
                }, obj_work.pos.x, obj_work.pos.y);
                GMM_PAD_VIB_SMALL();
                GmSoundPlaySE("Capsule");
                GmPlySeqChangeBossGoal(g_gm_main_system.ply_work[0], obj_work.pos.x, obj_work.pos.y);
            }
            obj_work.user_flag |= 1U;
        }
        else
            obj_work.ofst.y = 0;
        if (obj_work.user_timer == 0)
            return;
        ++obj_work.user_timer;
        if (obj_work.user_timer != 420)
            return;
        g_gm_main_system.game_flag |= 4U;
    }

    private static void gmGmkCapsuleBodyMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.parent_obj.user_flag & 1) == 0)
            return;
        ObjDrawObjectActionSet3DNN(obj_work, 1, 0);
        obj_work.ppFunc = null;
    }

    private static void gmGmkCapsuleKeyMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.parent_obj.user_flag & 1) == 0)
            return;
        obj_work.spd.x = 24576;
        obj_work.spd.y = -16384;
        obj_work.move_flag &= 4294959103U;
        obj_work.move_flag |= 128U;
        obj_work.ppFunc = null;
        GmEffect3DESSetDispOffset(GmEfctCmnEsCreate(obj_work, 21), 0.0f, 46f, 40f);
        GmEffect3DESSetDispOffset(GmEfctCmnEsCreate(obj_work, 22), 0.0f, 46f, 40f);
    }

    private static void gmGmkCapsuleAnimalMake(OBS_OBJECT_WORK obj_work)
    {
        for (ushort index = 0; index < 20; ++index)
            GmGmkAnimalInit(obj_work, g_gm_gmk_capsule_animal_set[index].ofs_x << 12, g_gm_gmk_capsule_animal_set[index].ofs_y << 12, g_gm_gmk_capsule_animal_set[index].ofs_z << 12, g_gm_gmk_capsule_animal_set[index].type, g_gm_gmk_capsule_animal_set[index].vec, g_gm_gmk_capsule_animal_set[index].time);
    }

}