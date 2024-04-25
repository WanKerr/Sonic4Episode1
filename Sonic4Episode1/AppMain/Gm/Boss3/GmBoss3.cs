public partial class AppMain
{
    private static void GmBoss3Build()
    {
        AMS_AMB_HEADER gameDatEnemyArc = GmBoss3GetGameDatEnemyArc();
        gm_boss3_obj_3d_list = GmGameDBuildRegBuildModel((AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, gameDatEnemyArc), (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 1, gameDatEnemyArc), 0U);
        ObjDataLoadAmbIndex(ObjDataGet(728), 2, gameDatEnemyArc);
        ObjDataLoadAmbIndex(ObjDataGet(729), 3, gameDatEnemyArc);
    }

    private static void GmBoss3Flush()
    {
        GmEfctBossFlushSingleDataInit();
        ObjDataRelease(ObjDataGet(729));
        ObjDataRelease(ObjDataGet(728));
        AMS_AMB_HEADER amsAmbHeader = (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, GmBoss3GetGameDatEnemyArc());
        GmGameDBuildRegFlushModel(gm_boss3_obj_3d_list, amsAmbHeader.file_num);
        gm_boss3_obj_3d_list = null;
    }

    private static AMS_AMB_HEADER GmBoss3GetGameDatEnemyArc()
    {
        return g_gm_gamedat_enemy_arc;
    }

    private static OBS_OBJECT_WORK GmBoss3Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_BOSS3_MGR_WORK work = (GMS_BOSS3_MGR_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS3_MGR_WORK(), "BOSS3_MGR");
        OBS_OBJECT_WORK objWork = work.ene_3d.ene_com.obj_work;
        objWork.flag |= 16U;
        objWork.disp_flag |= 32U;
        objWork.move_flag |= 8448U;
        work.ene_3d.ene_com.enemy_flag |= 32768U;
        objWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3MgrMainFuncWaitLoad);
        work.life = GmBsCmnIsFinalZoneType(objWork) == 0 ? 8 : 4;
        return objWork;
    }

    private static OBS_OBJECT_WORK GmBoss3BodyInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_BOSS3_BODY_WORK work = (GMS_BOSS3_BODY_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS3_BODY_WORK(), "BOSS3_BODY");
        GMS_ENEMY_3D_WORK ene3d = work.ene_3d;
        OBS_OBJECT_WORK objWork = ene3d.ene_com.obj_work;
        ObjObjectCopyAction3dNNModel(objWork, gm_boss3_obj_3d_list[0], ene3d.obj_3d);
        ObjObjectAction3dNNMotionLoad(objWork, 0, true, ObjDataGet(728), null, 0, null);
        ene3d.ene_com.vit = 1;
        ObjRectWorkSet(ene3d.ene_com.rect_work[2], -24, -24, 24, 24);
        ObjRectGroupSet(ene3d.ene_com.rect_work[2], 1, 3);
        ene3d.ene_com.rect_work[2].flag &= 4294967291U;
        ene3d.ene_com.rect_work[2].flag |= 1024U;
        work.ene_3d.ene_com.rect_work[1].flag |= 1024U;
        ObjRectWorkSet(ene3d.ene_com.rect_work[0], -28, -28, 28, 24);
        ene3d.ene_com.rect_work[0].ppDef = new OBS_RECT_WORK_Delegate1(gmBoss3BodyDefFunc);
        ene3d.ene_com.rect_work[0].flag |= 1024U;
        gmBoss3BodySetRectNormal(work);
        objWork.pos.z = 655360;
        objWork.flag |= 16U;
        objWork.disp_flag |= 4194309U;
        objWork.move_flag &= 4294967167U;
        objWork.move_flag |= 53776U;
        work.is_move = 0;
        objWork.obj_3d.blend_spd = 0.125f;
        ObjDrawObjectSetToon(objWork);
        objWork.disp_flag |= 134217728U;
        objWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3BodyMainFuncWaitSetup);
        objWork.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3BodyOutFunc);
        objWork.ppMove = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3BodyChaseMoveFunc);
        gmBoss3BodyChangeState(work, 0);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return objWork;
    }

    private static OBS_OBJECT_WORK GmBoss3EggInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_ENEMY_3D_WORK ene3d = ((GMS_BOSS3_EGG_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS3_EGG_WORK(), "BOSS3_EGG")).ene_3d;
        OBS_OBJECT_WORK objWork = ene3d.ene_com.obj_work;
        ObjObjectCopyAction3dNNModel(objWork, gm_boss3_obj_3d_list[1], ene3d.obj_3d);
        ObjObjectAction3dNNMotionLoad(objWork, 0, true, ObjDataGet(729), null, 0, null);
        ene3d.ene_com.rect_work[1].flag |= 3072U;
        ene3d.ene_com.rect_work[0].flag |= 3072U;
        ene3d.ene_com.rect_work[2].flag |= 3072U;
        objWork.flag |= 16U;
        objWork.disp_flag |= 4194309U;
        objWork.move_flag |= 4352U;
        objWork.move_flag &= 4294967167U;
        objWork.obj_3d.blend_spd = 0.125f;
        ObjDrawObjectSetToon(objWork);
        objWork.disp_flag |= 134217728U;
        ene3d.ene_com.enemy_flag |= 32768U;
        objWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3EggmanMainFuncWaitSetup);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return objWork;
    }

    private static void gmBoss3ChangeTextureBurnt(OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d.drawflag |= 268435456U;
        obj_work.obj_3d.draw_state.texoffset[0].mode = 2;
        obj_work.obj_3d.draw_state.texoffset[0].u = 0.5f;
    }

    private static void gmBoss3ExitFunc(MTS_TASK_TCB tcb)
    {
        gmBoss3MgrDeleteObject(mtTaskGetTcbWork(tcb));
        GmEnemyDefaultExit(tcb);
    }

}