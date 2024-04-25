public partial class AppMain
{
    private static void GmBoss2Build()
    {
        AMS_AMB_HEADER gameDatEnemyArc = GmBoss2GetGameDatEnemyArc();
        gm_boss2_obj_3d_list = GmGameDBuildRegBuildModel((AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, gameDatEnemyArc), (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 1, gameDatEnemyArc), 0U);
        ObjDataLoadAmbIndex(ObjDataGet(711), 2, gameDatEnemyArc);
        ObjDataLoadAmbIndex(ObjDataGet(713), 4, gameDatEnemyArc);
        ObjDataLoadAmbIndex(ObjDataGet(712), 3, gameDatEnemyArc);
        ObjDataLoadAmbIndex(ObjDataGet(714), 6, gameDatEnemyArc);
        ObjDataLoadAmbIndex(ObjDataGet(715), 7, gameDatEnemyArc);
        ObjDataLoadAmbIndex(ObjDataGet(716), 8, gameDatEnemyArc);
        ObjDataLoadAmbIndex(ObjDataGet(717), 9, gameDatEnemyArc);
        ObjDataLoadAmbIndex(ObjDataGet(718), 10, gameDatEnemyArc);
        GmEfctBossBuildSingleDataReg(14, ObjDataGet(722), ObjDataGet(723), 0, null, null, gameDatEnemyArc);
        GmEfctBossBuildSingleDataReg(14, ObjDataGet(722), ObjDataGet(723), 0, null, null, gameDatEnemyArc);
        GmEfctBossBuildSingleDataReg(14, ObjDataGet(722), ObjDataGet(723), 0, null, null, gameDatEnemyArc);
        GmEfctBossBuildSingleDataReg(14, ObjDataGet(722), ObjDataGet(723), 0, null, null, gameDatEnemyArc);
        GmEfctBossBuildSingleDataReg(14, ObjDataGet(722), ObjDataGet(723), 0, null, null, gameDatEnemyArc);
        GmEfctBossBuildSingleDataReg(15, ObjDataGet(726), ObjDataGet(727), 5, ObjDataGet(725), ObjDataGet(724), gameDatEnemyArc);
        GmEfctBossBuildSingleDataReg(14, ObjDataGet(722), ObjDataGet(723), 0, null, null, gameDatEnemyArc);
        GmEfctBossBuildSingleDataReg(15, ObjDataGet(726), ObjDataGet(727), 5, ObjDataGet(725), ObjDataGet(724), gameDatEnemyArc);
    }

    private static void GmBoss2Flush()
    {
        GmEfctBossFlushSingleDataInit();
        ObjDataRelease(ObjDataGet(718));
        ObjDataRelease(ObjDataGet(717));
        ObjDataRelease(ObjDataGet(716));
        ObjDataRelease(ObjDataGet(715));
        ObjDataRelease(ObjDataGet(714));
        ObjDataRelease(ObjDataGet(713));
        ObjDataRelease(ObjDataGet(711));
        ObjDataRelease(ObjDataGet(712));
        AMS_AMB_HEADER amsAmbHeader = (AMS_AMB_HEADER)ObjDataLoadAmbIndex(null, 0, GmBoss2GetGameDatEnemyArc());
        GmGameDBuildRegFlushModel(gm_boss2_obj_3d_list, amsAmbHeader.file_num);
        gm_boss2_obj_3d_list = null;
    }

    private static AMS_AMB_HEADER GmBoss2GetGameDatEnemyArc()
    {
        return g_gm_gamedat_enemy_arc;
    }

    private static OBS_OBJECT_WORK GmBoss2Init(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_BOSS2_MGR_WORK work = (GMS_BOSS2_MGR_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS2_MGR_WORK(), "BOSS2_MGR");
        OBS_OBJECT_WORK objWork = work.ene_3d.ene_com.obj_work;
        objWork.flag |= 16U;
        objWork.disp_flag |= 32U;
        objWork.move_flag |= 8448U;
        work.ene_3d.ene_com.enemy_flag |= 32768U;
        objWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2MgrMainFuncWaitLoad);
        work.life = GmBsCmnIsFinalZoneType(objWork) == 0 ? 8 : 4;
        return objWork;
    }

    private static OBS_OBJECT_WORK GmBoss2BodyInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_BOSS2_BODY_WORK work = (GMS_BOSS2_BODY_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS2_BODY_WORK(), "BOSS2_BODY");
        GMS_ENEMY_3D_WORK ene3d = work.ene_3d;
        OBS_OBJECT_WORK objWork = ene3d.ene_com.obj_work;
        ObjObjectCopyAction3dNNModel(objWork, gm_boss2_obj_3d_list[1], ene3d.obj_3d);
        ObjObjectAction3dNNMotionLoad(objWork, 0, true, ObjDataGet(711), null, 0, null);
        ene3d.ene_com.vit = 1;
        ObjRectWorkSet(ene3d.ene_com.rect_work[2], -16, -16, 16, 16);
        ObjRectGroupSet(ene3d.ene_com.rect_work[2], 1, 3);
        ene3d.ene_com.rect_work[2].flag &= 4294967291U;
        ene3d.ene_com.rect_work[2].flag |= 1024U;
        work.ene_3d.ene_com.rect_work[1].flag |= 1024U;
        ObjRectWorkSet(ene3d.ene_com.rect_work[0], -26, -26, 26, 26);
        ene3d.ene_com.rect_work[0].ppDef = new OBS_RECT_WORK_Delegate1(gmBoss2BodyDefFunc);
        ene3d.ene_com.rect_work[0].flag |= 1024U;
        gmBoss2BodySetRectArm(work);
        gmBoss2BodySetRectNormal(work);
        ObjObjectFieldRectSet(objWork, -24, -24, 24, 24);
        objWork.pos.z = 131072;
        objWork.flag |= 16U;
        objWork.disp_flag |= 4194309U;
        objWork.move_flag &= 4294967167U;
        objWork.move_flag |= 49168U;
        objWork.obj_3d.blend_spd = 0.125f;
        ObjDrawObjectSetToon(objWork);
        objWork.disp_flag |= 134217728U;
        work.se_handle = GsSoundAllocSeHandle();
        objWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2BodyMainFuncWaitSetup);
        objWork.ppOut = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2BodyOutFunc);
        gmBoss2BodyChangeState(work, 0);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return objWork;
    }

    private static OBS_OBJECT_WORK GmBoss2EggInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_ENEMY_3D_WORK ene3d = ((GMS_BOSS2_EGG_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS2_EGG_WORK(), "BOSS2_EGG")).ene_3d;
        OBS_OBJECT_WORK objWork = ene3d.ene_com.obj_work;
        ObjObjectCopyAction3dNNModel(objWork, gm_boss2_obj_3d_list[2], ene3d.obj_3d);
        ObjObjectAction3dNNMotionLoad(objWork, 0, true, ObjDataGet(713), null, 0, null);
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
        objWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EggmanMainFuncWaitSetup);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return objWork;
    }

    private static OBS_OBJECT_WORK GmBoss2BallInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        GMS_ENEMY_3D_WORK ene3d = ((GMS_BOSS2_BALL_WORK)GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS2_BALL_WORK(), "BOSS2_BALL")).ene_3d;
        OBS_OBJECT_WORK objWork = ene3d.ene_com.obj_work;
        ObjObjectCopyAction3dNNModel(objWork, gm_boss2_obj_3d_list[0], ene3d.obj_3d);
        ObjRectWorkSet(ene3d.ene_com.rect_work[1], -8, -8, 8, 8);
        ene3d.ene_com.rect_work[1].ppHit = new OBS_RECT_WORK_Delegate1(gmBoss2BallHitFunc);
        ene3d.ene_com.rect_work[1].flag |= 1024U;
        ene3d.ene_com.rect_work[0].flag |= 2048U;
        ene3d.ene_com.rect_work[2].flag |= 2048U;
        ene3d.ene_com.enemy_flag |= 32768U;
        objWork.disp_flag |= 4194304U;
        objWork.obj_3d.blend_spd = 0.125f;
        ObjDrawObjectSetToon(objWork);
        objWork.disp_flag |= 134217728U;
        ObjObjectFieldRectSet(objWork, -4, -8, 4, 6);
        objWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2BallMainFuncWaitSetup);
        objWork.obj_3d.use_light_flag &= 4294967294U;
        objWork.obj_3d.use_light_flag |= 64U;
        return objWork;
    }

    private static void gmBoss2ChangeTextureBurnt(OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d.drawflag |= 268435456U;
        obj_work.obj_3d.draw_state.texoffset[0].mode = 2;
        obj_work.obj_3d.draw_state.texoffset[0].u = 0.5f;
    }

    private static void gmBoss2ExitFunc(MTS_TASK_TCB tcb)
    {
        gmBoss2MgrDeleteObject(mtTaskGetTcbWork(tcb));
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss2EffectExitFunc(MTS_TASK_TCB tcb)
    {
        gmBoss2MgrDeleteObject(mtTaskGetTcbWork(tcb));
        GmEffectDefaultExit(tcb);
    }

    private static int gmBoss2CheckScrollLocked()
    {
        return ((int)g_gm_main_system.game_flag & 32768) != 0 ? 1 : 0;
    }

    private static int gmBoss2MgrCheckSetupComplete(GMS_BOSS2_MGR_WORK mgr_work)
    {
        return ((int)mgr_work.flag & 1) != 0 ? 1 : 0;
    }

    private static GMS_BOSS2_MGR_WORK gmBoss2MgrGetMgrWork(
      OBS_OBJECT_WORK obj_work_parts)
    {
        return (GMS_BOSS2_MGR_WORK)obj_work_parts.user_work_OBJECT;
    }

    private static void gmBoss2MgrAddObject(
      GMS_BOSS2_MGR_WORK mgr_work,
      OBS_OBJECT_WORK obj_work_parts)
    {
        ++mgr_work.obj_create_count;
        obj_work_parts.user_work_OBJECT = mgr_work;
    }

    private static void gmBoss2MgrDeleteObject(OBS_OBJECT_WORK obj_work_parts)
    {
        --gmBoss2MgrGetMgrWork(obj_work_parts).obj_create_count;
        obj_work_parts.user_work = 0U;
    }

    private static void gmBoss2MgrMainFuncWaitLoad(OBS_OBJECT_WORK obj_work)
    {
        if (GmBsCmnIsFinalZoneType(obj_work) != 0 && !GmMainDatLoadBossBattleLoadCheck(1))
            return;
        GMS_BOSS2_MGR_WORK mgr_work = (GMS_BOSS2_MGR_WORK)obj_work;
        GMS_BOSS2_BODY_WORK gmsBosS2BodyWork = (GMS_BOSS2_BODY_WORK)GmEventMgrLocalEventBirth(316, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
        OBS_OBJECT_WORK objWork1 = gmsBosS2BodyWork.ene_3d.ene_com.obj_work;
        objWork1.parent_obj = obj_work;
        gmsBosS2BodyWork.parts_objs[0] = objWork1;
        mgr_work.body_work = gmsBosS2BodyWork;
        gmBoss2MgrAddObject(mgr_work, objWork1);
        mtTaskChangeTcbDestructor(objWork1.tcb, new GSF_TASK_PROCEDURE(gmBoss2BodyExit));
        OBS_OBJECT_WORK objWork2 = ((GMS_BOSS2_EGG_WORK)GmEventMgrLocalEventBirth(317, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0)).ene_3d.ene_com.obj_work;
        objWork2.parent_obj = objWork1;
        gmBoss2MgrAddObject(mgr_work, objWork2);
        mtTaskChangeTcbDestructor(objWork2.tcb, new GSF_TASK_PROCEDURE(gmBoss2ExitFunc));
        gmsBosS2BodyWork.parts_objs[1] = objWork2;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2MgrMainFuncWaitSetup);
    }

    private static void gmBoss2MgrMainFuncWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_MGR_WORK gmsBosS2MgrWork = (GMS_BOSS2_MGR_WORK)obj_work;
        GMS_BOSS2_BODY_WORK bodyWork = gmsBosS2MgrWork.body_work;
        for (int index = 0; 2 > index; ++index)
        {
            if (bodyWork.parts_objs[index] == null)
                return;
        }
        gmsBosS2MgrWork.flag |= 1U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2MgrMainFunc);
    }

    private static void gmBoss2MgrMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_MGR_WORK gmsBosS2MgrWork = (GMS_BOSS2_MGR_WORK)obj_work;
        if (((int)gmsBosS2MgrWork.flag & 2) == 0)
            return;
        GMM_BS_OBJ(gmsBosS2MgrWork.body_work).flag |= 8U;
        gmsBosS2MgrWork.body_work = null;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2MgrMainFuncWaitRelease);
    }

    private static void gmBoss2MgrMainFuncWaitRelease(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_MGR_WORK gmsBosS2MgrWork = (GMS_BOSS2_MGR_WORK)obj_work;
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
        {
            if (gmsBosS2MgrWork.obj_create_count > 0)
                return;
            ((GMS_ENEMY_COM_WORK)obj_work).enemy_flag |= 65536U;
            obj_work.flag |= 4U;
            GmGameDatReleaseBossBattleStart(1);
            GmGmkCamScrLimitRelease(14);
            OBS_OBJECT_WORK obj_work1 = gmBoss2BodySearchShutterOut();
            if (obj_work1 != null)
                GmGmkShutterOutChangeModeOpen(obj_work1);
        }
        obj_work.ppFunc = null;
    }

    private static void gmBoss2BodyExit(MTS_TASK_TCB tcb)
    {
        GMS_BOSS2_BODY_WORK tcbWork = (GMS_BOSS2_BODY_WORK)mtTaskGetTcbWork(tcb);
        OBS_OBJECT_WORK objWork = tcbWork.ene_3d.ene_com.obj_work;
        GmBsCmnClearBossMotionCBSystem(objWork);
        GmBsCmnDeleteSNMWork(tcbWork.snm_work);
        GmBsCmnClearCNMCb(objWork);
        GmBsCmnDeleteCNMMgrWork(tcbWork.cnm_mgr_work);
        if (tcbWork.se_handle != null)
        {
            GmSoundStopSE(tcbWork.se_handle);
            GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = null;
        }
        gmBoss2ExitFunc(tcb);
    }

    private static void gmBoss2BodyReactionPlayer(
      OBS_OBJECT_WORK obj_work_player,
      OBS_OBJECT_WORK obj_work_body)
    {
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)obj_work_player;
        GmPlySeqAtkReactionInit(ply_work);
        GmPlySeqSetJumpState(ply_work, 0, 5U);
        obj_work_player.spd_m = 0;
        obj_work_player.spd.x = obj_work_player.move.x < 0 ? 20480 : -20480;
        obj_work_player.spd.y = obj_work_player.pos.y > obj_work_body.pos.y ? 16384 : -16384;
        GmPlySeqSetNoJumpMoveTime(ply_work, 102400);
    }

    private static void gmBoss2BodyRecFuncRegistArmRect(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK gmsBosS2BodyWork = (GMS_BOSS2_BODY_WORK)obj_work;
        ObjObjectRectRegist(obj_work, gmsBosS2BodyWork.rect_work_arm);
    }

    private static void gmBoss2BodyCatchChangeArmRectNormal(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_RECT_WORK rectWorkArm = body_work.rect_work_arm;
        rectWorkArm.ppHit = null;
        rectWorkArm.ppDef = null;
        ObjRectAtkSet(rectWorkArm, 0, 0);
        ObjRectDefSet(rectWorkArm, 0, 0);
        rectWorkArm.flag &= 4294967291U;
    }

    private static void gmBoss2BodyCatchChangeArmRectActive(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_RECT_WORK rectWorkArm = body_work.rect_work_arm;
        rectWorkArm.ppHit = new OBS_RECT_WORK_Delegate1(gmBoss2BodyHitFunc);
        rectWorkArm.ppDef = null;
        ObjRectAtkSet(rectWorkArm, 2, 1);
        ObjRectDefSet(rectWorkArm, ushort.MaxValue, 0);
        rectWorkArm.flag |= 4U;
    }

    private static void gmBoss2BodyCatchChangeArmRectCatch(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_RECT_WORK rectWorkArm = body_work.rect_work_arm;
        rectWorkArm.ppHit = null;
        rectWorkArm.ppDef = new OBS_RECT_WORK_Delegate1(gmBoss2BodyCatchHitFuncArmCatch);
        ObjRectAtkSet(rectWorkArm, 0, 0);
        ObjRectDefSet(rectWorkArm, 65534, 0);
        rectWorkArm.flag |= 4U;
    }

    private static void gmBoss2BodySetRectNormal(GMS_BOSS2_BODY_WORK body_work)
    {
        ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], 0, 0, 0, 0);
        body_work.ene_3d.ene_com.rect_work[0].flag |= 4U;
        gmBoss2BodyCatchChangeArmRectNormal(body_work);
    }

    private static void gmBoss2BodySetRectActive(GMS_BOSS2_BODY_WORK body_work)
    {
        ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], -8, -8, 8, 8);
        body_work.ene_3d.ene_com.rect_work[0].flag |= 4U;
        gmBoss2BodyCatchChangeArmRectActive(body_work);
    }

    private static void gmBoss2BodySetRectRoll(GMS_BOSS2_BODY_WORK body_work)
    {
        ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], -36, -36, 36, 36);
        body_work.ene_3d.ene_com.rect_work[0].flag &= 4294967291U;
        gmBoss2BodyCatchChangeArmRectNormal(body_work);
    }

    private static void gmBoss2BodySetRectArm(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)body_work;
        OBS_RECT_WORK rectWorkArm = body_work.rect_work_arm;
        ObjRectGroupSet(rectWorkArm, 1, 1);
        gmBoss2BodyRectApplyOffsetArm(body_work);
        rectWorkArm.ppHit = new OBS_RECT_WORK_Delegate1(gmBoss2BodyHitFunc);
        rectWorkArm.parent_obj = obsObjectWork;
        rectWorkArm.flag |= 1028U;
        obsObjectWork.ppRec = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2BodyRecFuncRegistArmRect);
        gmBoss2BodyCatchChangeArmRectNormal(body_work);
    }

    private static void gmBoss2BodyRectApplyOffsetArm(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_RECT_WORK rectWorkArm = body_work.rect_work_arm;
        uint flag = rectWorkArm.flag;
        ObjRectWorkSet(rectWorkArm, -40, (short)(24.0 - body_work.offset_arm), 40, (short)(40.0 - body_work.offset_arm));
        rectWorkArm.flag = flag;
    }

    private static void gmBoss2BodySetActionAllParts(
      GMS_BOSS2_BODY_WORK body_work,
      int action_id)
    {
        gmBoss2BodySetActionAllParts(body_work, action_id, 0);
    }

    private static void gmBoss2BodySetActionAllParts(
      GMS_BOSS2_BODY_WORK body_work,
      int action_id,
      int force_change)
    {
        if (force_change == 0 && body_work.action_id == action_id)
            return;
        body_work.action_id = action_id;
        for (int index = 0; 2 > index; ++index)
        {
            OBS_OBJECT_WORK partsObj = body_work.parts_objs[index];
            if (partsObj != null)
            {
                GMS_BOSS2_PART_ACT_INFO bosS2PartActInfo = gm_boss2_act_info_tbl[action_id][index];
                if (index != 1 || ((int)((GMS_BOSS2_EGG_WORK)partsObj).flag & 1) == 0)
                {
                    if (bosS2PartActInfo.is_maintain != 0)
                    {
                        if (bosS2PartActInfo.is_repeat != 0)
                            partsObj.disp_flag |= 4U;
                    }
                    else
                        GmBsCmnSetAction(partsObj, bosS2PartActInfo.mtn_id, bosS2PartActInfo.is_repeat, bosS2PartActInfo.is_blend);
                    partsObj.obj_3d.speed[0] = bosS2PartActInfo.mtn_spd;
                    partsObj.obj_3d.blend_spd = bosS2PartActInfo.blend_spd;
                }
            }
        }
    }

    private static void gmBoss2BodyOutFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK gmsBosS2BodyWork = (GMS_BOSS2_BODY_WORK)obj_work;
        GmBsCmnUpdateCNMParam(obj_work, gmsBosS2BodyWork.cnm_mgr_work);
        ObjDrawActionSummary(obj_work);
    }

    private static void gmBoss2BodyDefFunc(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        OBS_OBJECT_WORK parentObj1 = target_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = own_rect.parent_obj;
        GMS_BOSS2_BODY_WORK body_work = (GMS_BOSS2_BODY_WORK)parentObj2;
        if (parentObj1 == null || 1 != parentObj1.obj_type)
            return;
        gmBoss2BodyReactionPlayer(parentObj1, parentObj2);
        gmBoss2BodySetNoHitTime(body_work, 10U);
        if (((int)body_work.rect_work_arm.flag & 4) != 0 && parentObj2.pos.y < parentObj1.pos.y)
            return;
        gmBoss2BodyDamage(body_work);
    }

    private static void gmBoss2BodyHitFunc(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        UNREFERENCED_PARAMETER(target_rect);
        ((GMS_BOSS2_BODY_WORK)own_rect.parent_obj).flag |= 268435456U;
    }

    private static OBS_OBJECT_WORK gmBoss2BodySearchShutterIn()
    {
        OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, 3);
        while (obj_work != null && ((GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id != 261)
            obj_work = ObjObjectSearchRegistObject(obj_work, 3);
        return obj_work;
    }

    private static OBS_OBJECT_WORK gmBoss2BodySearchShutterOut()
    {
        OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, 3);
        while (obj_work != null && ((GMS_ENEMY_COM_WORK)obj_work).eve_rec.id != 262)
            obj_work = ObjObjectSearchRegistObject(obj_work, 3);
        return obj_work;
    }

    private static void gmBoss2BodySetNoHitTime(GMS_BOSS2_BODY_WORK body_work, uint time)
    {
        GMS_ENEMY_COM_WORK eneCom = body_work.ene_3d.ene_com;
        body_work.counter_no_hit = time;
        eneCom.rect_work[0].flag |= 2048U;
    }

    private static void gmBoss2BodyUpdateNoHitTime(GMS_BOSS2_BODY_WORK body_work)
    {
        if (body_work.counter_no_hit > 0U)
            --body_work.counter_no_hit;
        else
            body_work.ene_3d.ene_com.rect_work[0].flag &= 4294965247U;
    }

    private static void gmBoss2BodySetInvincibleTime(GMS_BOSS2_BODY_WORK body_work, uint time)
    {
        body_work.counter_invincible = time;
        body_work.flag |= 1U;
    }

    private static void gmBoss2BodyUpdateInvincibleTime(GMS_BOSS2_BODY_WORK body_work)
    {
        if (body_work.counter_invincible > 0U)
            --body_work.counter_invincible;
        else
            body_work.flag &= 4294967294U;
    }

    private static void gmBoss2BodySetDirection(GMS_BOSS2_BODY_WORK body_work, short deg)
    {
        body_work.angle_current = deg;
    }

    private static void gmBoss2BodySetDirectionNormal(GMS_BOSS2_BODY_WORK body_work)
    {
        if (((int)GMM_BS_OBJ(body_work).disp_flag & 1) != 0)
            gmBoss2BodySetDirection(body_work, GMD_BOSS2_ANGLE_LEFT);
        else
            gmBoss2BodySetDirection(body_work, GMD_BOSS2_ANGLE_RIGHT);
    }

    private static void gmBoss2BodySetDirectionRoll(GMS_BOSS2_BODY_WORK body_work)
    {
        if (((int)GMM_BS_OBJ(body_work).disp_flag & 1) != 0)
            gmBoss2BodySetDirection(body_work, GMD_BOSS2_BODY_PINBALL_ANGLE_LEFT_ROLL);
        else
            gmBoss2BodySetDirection(body_work, GMD_BOSS2_BODY_PINBALL_ANGLE_RIGHT_ROLL);
    }

    private static void gmBoss2BodyUpdateDirection(GMS_BOSS2_BODY_WORK body_work)
    {
        GMM_BS_OBJ(body_work).dir.y = (ushort)body_work.angle_current;
    }

    private static float gmBoss2BodyCalcMoveXNormalFrame(
      GMS_BOSS2_BODY_WORK body_work,
      int x,
      int speed)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        return MTM_MATH_ABS(x - obsObjectWork.pos.x) / (float)speed;
    }

    private static void gmBoss2BodyInitMoveNormal(
      GMS_BOSS2_BODY_WORK body_work,
      VecFx32 dest_pos,
      float frame)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        body_work.start_pos.x = obsObjectWork.pos.x;
        body_work.start_pos.y = obsObjectWork.pos.y;
        body_work.start_pos.z = obsObjectWork.pos.z;
        body_work.end_pos.x = dest_pos.x;
        body_work.end_pos.y = dest_pos.y;
        body_work.end_pos.z = body_work.start_pos.z;
        body_work.move_counter = 0.0f;
        if (frame > 0.0)
            body_work.move_frame = frame;
        else
            body_work.move_frame = 1f;
    }

    private static float gmBoss2BodyUpdateMoveNormal(GMS_BOSS2_BODY_WORK body_work)
    {
        VecFx32 vecFx32_1 = new VecFx32();
        ++body_work.move_counter;
        if (body_work.move_counter >= (double)body_work.move_frame)
        {
            vecFx32_1.x = body_work.end_pos.x;
            vecFx32_1.y = body_work.end_pos.y;
            vecFx32_1.z = body_work.end_pos.z;
        }
        else
        {
            float num = (float)(0.5 * (1.0 - nnCos(AKM_DEGtoA32(180f * (body_work.move_counter / body_work.move_frame)))));
            VecFx32 vecFx32_2 = new VecFx32();
            vecFx32_2.x = (int)((body_work.end_pos.x - body_work.start_pos.x) * (double)num);
            vecFx32_2.y = (int)((body_work.end_pos.y - body_work.start_pos.y) * (double)num);
            vecFx32_2.z = (int)((body_work.end_pos.z - body_work.start_pos.z) * (double)num);
            vecFx32_1.x = body_work.start_pos.x + vecFx32_2.x;
            vecFx32_1.y = body_work.start_pos.y + vecFx32_2.y;
            vecFx32_1.z = body_work.start_pos.z + vecFx32_2.z;
        }
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.pos.x = vecFx32_1.x;
        obsObjectWork.pos.y = vecFx32_1.y;
        obsObjectWork.pos.z = vecFx32_1.z;
        return body_work.move_frame - body_work.move_counter;
    }

    private static void gmBoss2BodyInitMovePinBall(
      GMS_BOSS2_BODY_WORK body_work,
      VecFx32 dir_pos,
      int speed)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        VecFx32 vecFx32 = new VecFx32();
        vecFx32.x = dir_pos.x - obsObjectWork.pos.x;
        vecFx32.y = dir_pos.y - obsObjectWork.pos.y;
        vecFx32.z = 0;
        if (vecFx32.x > 2048000)
            vecFx32.x = 2048000;
        else if (vecFx32.x < -2048000)
            vecFx32.x = -2048000;
        if (vecFx32.y > 2048000)
            vecFx32.y = 2048000;
        else if (vecFx32.y < -2048000)
            vecFx32.y = -2048000;
        int denom = FX_Sqrt(FX_Mul(vecFx32.x, vecFx32.x) + FX_Mul(vecFx32.y, vecFx32.y));
        if (denom == 0)
            return;
        obsObjectWork.spd.x = FX_Mul(FX_Div(vecFx32.x, denom), speed);
        obsObjectWork.spd.y = FX_Mul(FX_Div(vecFx32.y, denom), speed);
    }

    private static int gmBoss2BodyPinBallCheckFieldUnder(OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.move_flag & 1) != 0 ? 1 : 0;
    }

    private static int gmBoss2BodyPinBallCheckFieldOver(OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.move_flag & 2) != 0 ? 1 : 0;
    }

    private static int gmBoss2BodyPinBallCheckFieldFront(OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.move_flag & 4) != 0 ? 1 : 0;
    }

    private static int gmBoss2BodyPinBallCheckFieldBack(OBS_OBJECT_WORK obj_work)
    {
        return ((int)obj_work.move_flag & 8) != 0 ? 1 : 0;
    }

    private static void gmBoss2BodyUpdateMovePinBall(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        int num1 = 0;
        int num2 = MTM_MATH_ABS(obj_work.spd.x);
        int num3 = MTM_MATH_ABS(obj_work.spd.y);
        if (gmBoss2BodyPinBallCheckFieldUnder(obj_work) != 0)
        {
            obj_work.spd.y = -num3;
            if (MTM_MATH_ABS(obj_work.spd.x) < 256)
                obj_work.spd.x = 256;
            obj_work.move_flag |= 32784U;
            num1 = 1;
        }
        else if (gmBoss2BodyPinBallCheckFieldOver(obj_work) != 0)
        {
            obj_work.spd.y = num3;
            if (MTM_MATH_ABS(obj_work.spd.x) < 256)
                obj_work.spd.x = 256;
            obj_work.move_flag |= 32784U;
            num1 = 1;
        }
        if (gmBoss2BodyPinBallCheckFieldFront(obj_work) != 0 || gmBoss2BodyPinBallCheckFieldBack(obj_work) != 0)
        {
            obj_work.spd.x = obj_work.spd.x >= 0 ? -num2 : num2;
            if (MTM_MATH_ABS(obj_work.spd.y) < 256)
                obj_work.spd.y = 256;
            obj_work.move_flag |= 32784U;
            num1 = 1;
        }
        if (num1 != 0)
        {
            if (((int)body_work.flag & 128) != 0)
                return;
            GmSoundPlaySE("Boss2_05");
            body_work.flag |= 128U;
        }
        else
            body_work.flag &= 4294967167U;
    }

    private static void gmBoss2BodyInitTurn(
      GMS_BOSS2_BODY_WORK body_work,
      short dest_angle,
      float frame,
      int flag_positive)
    {
        body_work.turn_counter = 0.0f;
        body_work.turn_frame = frame;
        body_work.turn_start = body_work.angle_current;
        ushort num1 = (ushort)((uint)dest_angle - (uint)body_work.angle_current);
        body_work.turn_amount = num1;
        if (flag_positive != 0)
            return;
        ushort num2 = (ushort)(body_work.turn_amount - AKM_DEGtoA32(360));
        body_work.turn_amount = num2 - AKM_DEGtoA32(360);
    }

    private static float gmBoss2BodyUpdateTurn(GMS_BOSS2_BODY_WORK body_work)
    {
        if (body_work.turn_frame < 1.0)
            return 0.0f;
        ++body_work.turn_counter;
        short deg;
        if (body_work.turn_counter >= (double)body_work.turn_frame)
        {
            deg = (short)(body_work.turn_start + (short)body_work.turn_amount);
        }
        else
        {
            int ang = AKM_DEGtoA32(180f * (body_work.turn_counter / body_work.turn_frame));
            float num = (float)(body_work.turn_amount * 0.5 * (1.0 - nnCos(ang)));
            deg = (short)(body_work.turn_start + (short)num);
        }
        gmBoss2BodySetDirection(body_work, deg);
        return body_work.turn_frame - body_work.turn_counter;
    }

    private static int gmBoss2BodyCatchArmCheckTarget(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        return ((int)body_work.flag & 16) != 0 || playerObj.pos.y < obsObjectWork.pos.y || MTM_MATH_ABS(playerObj.pos.x - obsObjectWork.pos.x) > 32768 ? 0 : 1;
    }

    private static int gmBoss2BodyCatchArmCountReleaseKey(GMS_BOSS2_BODY_WORK body_work)
    {
        GMS_PLAYER_WORK playerObj = (GMS_PLAYER_WORK)GmBsCmnGetPlayerObj();
        int num = 0;
        for (int index = 0; 16 > index; ++index)
        {
            if ((byte.MaxValue & playerObj.key_push & 1 << index) != 0)
            {
                ++body_work.count_release_key;
                num = 1;
            }
        }
        return num != 0 ? 1 : 0;
    }

    private static int gmBoss2BodyCatchArmCheckRelease(GMS_BOSS2_BODY_WORK body_work)
    {
        return body_work.count_release_key < 6 ? 0 : 1;
    }

    private static void gmBoss2BodyCatchArmUpdateShakePlayer(
      GMS_BOSS2_BODY_WORK body_work,
      uint shake_count)
    {
        body_work.shake_pos += body_work.shake_speed;
        int num1 = MTM_MATH_ABS(body_work.shake_pos);
        int num2 = MTM_MATH_ABS(body_work.shake_speed);
        if (num1 > 5)
            body_work.shake_speed = -body_work.shake_speed;
        else if (num1 < num2)
            ++body_work.shake_count;
        if (body_work.shake_count < shake_count * 2U)
            return;
        body_work.shake_speed = 0;
        body_work.shake_pos = 0;
        body_work.shake_count = 0U;
    }

    private static void gmBoss2BodyCatchSetArmLength(
      GMS_BOSS2_BODY_WORK body_work,
      float length)
    {
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(body_work.snm_work, body_work.snm_reg_id[2]);
        AkMathExtractScaleMtx(new NNS_MATRIX(), snmMtx);
        NNS_MATRIX nnsMatrix = new NNS_MATRIX();
        nnMakeTranslateMatrix(nnsMatrix, 0.0f, length / snmMtx.M11, 0.0f);
        for (int index = 0; 13 > index; ++index)
        {
            int cnm_reg_id = body_work.cnm_reg_id[index];
            GmBsCmnChangeCNMModeNode(body_work.cnm_mgr_work, cnm_reg_id, 1U);
            GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix, cnm_reg_id);
            GmBsCmnEnableCNMLocalCoordinate(body_work.cnm_mgr_work, cnm_reg_id, 1);
            GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, cnm_reg_id, 1);
            nnMakeUnitMatrix(nnsMatrix);
        }
    }

    private static void gmBoss2BodyCatchHitFuncArmCatch(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        GMS_BOSS2_BODY_WORK parentObj1 = (GMS_BOSS2_BODY_WORK)own_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != 1)
            return;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)parentObj2;
        own_rect.flag &= 4294967291U;
        GmPlySeqGmkInitBoss2Catch(ply_work);
        parentObj1.flag |= 8U;
        parentObj1.flag |= 268435456U;
    }

    private static void gmBoss2BodyCatchChangeNeedleModeActive()
    {
        for (OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, 3); obj_work != null; obj_work = ObjObjectSearchRegistObject(obj_work, 3))
        {
            GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
            if (gmsEnemy3DWork == null)
            {
                if (((GMS_ENEMY_COM_WORK)obj_work).eve_rec.id == 337)
                    GmGmkNeedleNeonChangeModeActive(obj_work);
            }
            else if (gmsEnemy3DWork.ene_com.eve_rec.id == 337)
                GmGmkNeedleNeonChangeModeActive(obj_work);
        }
    }

    private static void gmBoss2BodyCatchChangeNeedleModeWait()
    {
        for (OBS_OBJECT_WORK obj_work = ObjObjectSearchRegistObject(null, 3); obj_work != null; obj_work = ObjObjectSearchRegistObject(obj_work, 3))
        {
            GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
            if (gmsEnemy3DWork == null)
            {
                if (((GMS_ENEMY_COM_WORK)obj_work).eve_rec.id == 337)
                    GmGmkNeedleNeonChangeModeWait(obj_work);
            }
            else if (gmsEnemy3DWork.ene_com.eve_rec.id == 337)
                GmGmkNeedleNeonChangeModeWait(obj_work);
        }
    }

    private static int gmBoss2BodyBallShootCheckTarget(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        return ((int)body_work.flag & 16) != 0 || playerObj.pos.y < obsObjectWork.pos.y || MTM_MATH_ABS(playerObj.pos.x - obsObjectWork.pos.x) > 32768 ? 0 : 1;
    }

    private static int gmBoss2BodyPinBallCheckTurn(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (playerObj.pos.x < obsObjectWork.pos.x)
                return 0;
        }
        else if (obsObjectWork.pos.x < playerObj.pos.x)
            return 0;
        return 1;
    }

    private static void gmBoss2BodyPinBallAdjustMoveSpeed(
      GMS_BOSS2_BODY_WORK body_work,
      int speed)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        VecFx32 vecFx32 = new VecFx32();
        int denom = FX_Sqrt(FX_Mul(obsObjectWork.spd.x, obsObjectWork.spd.x) + FX_Mul(obsObjectWork.spd.y, obsObjectWork.spd.y));
        if (denom == 0)
        {
            vecFx32.x = 4096;
            vecFx32.y = 0;
        }
        else
        {
            vecFx32.x = FX_Div(obsObjectWork.spd.x, denom);
            vecFx32.y = FX_Div(obsObjectWork.spd.y, denom);
        }
        vecFx32.z = 0;
        int num = (speed - denom) / 10;
        obsObjectWork.spd.x = FX_Mul(vecFx32.x, denom + num);
        obsObjectWork.spd.y = FX_Mul(vecFx32.y, denom + num);
        obsObjectWork.spd.z = 0;
    }

    private static int gmBoss2BodyPinBallCheckAreaStop(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        int num1 = g_gm_main_system.map_fcol.left + (g_gm_main_system.map_fcol.right - g_gm_main_system.map_fcol.left) / 2;
        int num2 = g_gm_main_system.map_fcol.top + (g_gm_main_system.map_fcol.bottom - g_gm_main_system.map_fcol.top) / 2;
        return (num1 - 70) * 4096 < obsObjectWork.pos.x && obsObjectWork.pos.x < (num1 + 70) * 4096 && ((num2 - 110) * 4096 < obsObjectWork.pos.y && obsObjectWork.pos.y < (num2 + 110) * 4096) ? 0 : 1;
    }

    private static void gmBoss2BodyDamage(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work_parts = GMM_BS_OBJ(body_work);
        if (((int)body_work.flag & 1) != 0)
            return;
        GMS_BOSS2_MGR_WORK mgrWork = gmBoss2MgrGetMgrWork(obj_work_parts);
        --mgrWork.life;
        if (mgrWork.life > 0)
            body_work.flag |= 1073741824U;
        else
            body_work.flag |= 2147483648U;
        GmSoundPlaySE("Boss0_01");
        gmBoss2EffDamageInit(body_work);
        GMM_PAD_VIB_SMALL();
        gmBoss2BodySetInvincibleTime(body_work, 90U);
    }

    private static int gmBoss2BodyEscapeCheckScrollUnlock(GMS_BOSS2_BODY_WORK body_work)
    {
        return GMM_BS_OBJ(body_work).pos.y <= g_gm_main_system.map_fcol.top * 4096 + 131072 ? 1 : 0;
    }

    private static int gmBoss2BodyEscapeCheckScreenOut(GMS_BOSS2_BODY_WORK body_work)
    {
        return GMM_BS_OBJ(body_work).pos.y <= (g_gm_main_system.map_fcol.top - 64) * 4096 ? 1 : 0;
    }

    private static void gmBoss2BodyEscapeAddjustSpeed(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (MTM_MATH_ABS(obsObjectWork.spd.x) <= -3276)
            return;
        obsObjectWork.spd.x = 0;
        obsObjectWork.spd.y = -3276;
        obsObjectWork.spd_add.x = 0;
        obsObjectWork.spd_add.y = 0;
    }

    private static void gmBoss2BodyChangeState(GMS_BOSS2_BODY_WORK body_work, int state)
    {
        MPP_VOID_GMS_BOSS2_BODY_WORK gmsBosS2BodyWork1 = gm_boss2_body_state_func_tbl_leave[body_work.state];
        if (gmsBosS2BodyWork1 != null)
            gmsBosS2BodyWork1(body_work);
        body_work.prev_state = body_work.state;
        body_work.state = state;
        MPP_VOID_GMS_BOSS2_BODY_WORK gmsBosS2BodyWork2 = gm_boss2_body_state_func_tbl_enter[body_work.state];
        if (gmsBosS2BodyWork2 == null)
            return;
        gmsBosS2BodyWork2(body_work);
    }

    private static void gmBoss2BodyStateStartEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK objWork = body_work.ene_3d.ene_com.obj_work;
        objWork.flag |= 2U;
        gmBoss2BodySetActionAllParts(body_work, 0, 1);
        GmBsCmnSetObjSpdZero(objWork);
        gmBoss2BodySetDirectionNormal(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateStartUpdateWait);
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
    }

    private static void gmBoss2BodyStateStartLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        GMM_BS_OBJ(body_work).flag &= 4294967293U;
        body_work.flag &= 4294967279U;
        body_work.ene_3d.ene_com.enemy_flag &= 4294934527U;
    }

    private static void gmBoss2BodyStateStartUpdateWait(GMS_BOSS2_BODY_WORK body_work)
    {
        if (gmBoss2CheckScrollLocked() == 0)
            return;
        GmMapSetMapDrawSize(4);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateStartUpdateEnd);
    }

    private static void gmBoss2BodyStateStartUpdateEnd(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (ObjViewOutCheck(obsObjectWork.pos.x, obsObjectWork.pos.y, 0, 0, 0, 0, 0) != 0)
            return;
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 180)
            return;
        obsObjectWork.user_timer = 0;
        OBS_OBJECT_WORK obj_work = gmBoss2BodySearchShutterIn();
        if (obj_work != null)
            GmGmkShutterInChangeModeClose(obj_work);
        gmBoss2BodyChangeState(body_work, 2);
    }

    private static void gmBoss2BodyStateCatchMoveEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss2BodySetActionAllParts(body_work, 1);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchMoveUpdateMove);
        gmBoss2EffAfterburnerRequestCreate(body_work);
        VecFx32 dest_pos = new VecFx32(((int)obsObjectWork.disp_flag & 1) == 0 ? g_gm_main_system.map_fcol.right * 4096 - 348160 : g_gm_main_system.map_fcol.left * 4096 + 348160, obsObjectWork.pos.y, obsObjectWork.pos.z);
        int speed = 4915;
        float frame = gmBoss2BodyCalcMoveXNormalFrame(body_work, dest_pos.x, speed);
        gmBoss2BodyInitMoveNormal(body_work, dest_pos, frame);
    }

    private static void gmBoss2BodyStateCatchMoveLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        gmBoss2EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss2BodyStateCatchMoveUpdateMove(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss2BodySetDirectionNormal(body_work);
        GMS_BOSS2_MGR_WORK mgrWork = gmBoss2MgrGetMgrWork(obsObjectWork);
        if (GmBsCmnIsFinalZoneType(obsObjectWork) != 0)
        {
            if (mgrWork.life <= 3)
            {
                gmBoss2BodyChangeState(body_work, 5);
                return;
            }
        }
        else if (8 - mgrWork.life >= 3)
        {
            gmBoss2BodyChangeState(body_work, 5);
            return;
        }
        if (gmBoss2BodyCatchArmCheckTarget(body_work) != 0)
        {
            gmBoss2BodyChangeState(body_work, 3);
        }
        else
        {
            if (gmBoss2BodyUpdateMoveNormal(body_work) > 60.0)
                return;
            short dest_angle;
            int flag_positive;
            if (((int)obsObjectWork.disp_flag & 1) != 0)
            {
                obsObjectWork.disp_flag &= 4294967294U;
                dest_angle = GMD_BOSS2_ANGLE_RIGHT;
                flag_positive = 1;
            }
            else
            {
                obsObjectWork.disp_flag |= 1U;
                dest_angle = GMD_BOSS2_ANGLE_LEFT;
                flag_positive = 0;
            }
            gmBoss2BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
            body_work.flag &= 4294967279U;
            body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchMoveUpdateTurn);
            gmBoss2EffAfterburnerRequestDelete(body_work);
        }
    }

    private static void gmBoss2BodyStateCatchMoveUpdateTurn(GMS_BOSS2_BODY_WORK body_work)
    {
        double num = gmBoss2BodyUpdateMoveNormal(body_work);
        if (0.0 < gmBoss2BodyUpdateTurn(body_work))
            return;
        gmBoss2BodyChangeState(body_work, 2);
    }

    private static void gmBoss2BodyStateCatchArmEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        gmBoss2BodySetActionAllParts(body_work, 2);
        GmBsCmnSetObjSpdZero(obj_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchArmUpdateOpen);
        gmBoss2EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss2BodyStateCatchArmLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.offset_arm = 0.0f;
        gmBoss2BodyCatchSetArmLength(body_work, body_work.offset_arm);
        body_work.count_release_key = 0;
        body_work.flag |= 16U;
    }

    private static void gmBoss2BodyStateCatchArmUpdateOpen(GMS_BOSS2_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchArmUpdateReady);
        gmBoss2BodySetActionAllParts(body_work, 3);
    }

    private static void gmBoss2BodyStateCatchArmUpdateReady(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 5)
            return;
        obsObjectWork.user_timer = 0;
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchArmUpdateDown);
        gmBoss2BodySetActionAllParts(body_work, 5);
        gmBoss2BodyCatchChangeArmRectCatch(body_work);
        GmSoundPlaySE("Boss2_01", body_work.se_handle);
    }

    private static void gmBoss2BodyStateCatchArmUpdateDown(GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.offset_arm -= 6f;
        gmBoss2BodyCatchSetArmLength(body_work, body_work.offset_arm);
        gmBoss2BodyRectApplyOffsetArm(body_work);
        if (((int)body_work.flag & 8) == 0 && body_work.offset_arm > -60.0)
            return;
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchArmUpdateClose);
        GmSoundStopSE(body_work.se_handle);
        GmSoundPlaySE("Boss2_02");
    }

    private static void gmBoss2BodyStateCatchArmUpdateClose(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        if ((((GMS_PLAYER_WORK)playerObj).player_flag & 1024) != 0)
            body_work.flag &= 4294967287U;
        if (((int)body_work.flag & 8) != 0)
        {
            GmBsCmnUpdateObject3DNNStuckWithNode(playerObj, body_work.snm_work, body_work.snm_reg_id[2], 1);
            playerObj.pos.y += -FX_F32_TO_FX32(body_work.offset_arm) + 163840;
        }
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchArmUpdateUp);
        gmBoss2BodySetActionAllParts(body_work, 6);
        if (((int)body_work.flag & 8) != 0)
            return;
        gmBoss2BodyCatchChangeArmRectNormal(body_work);
    }

    private static void gmBoss2BodyStateCatchArmUpdateUp(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)playerObj;
        ++body_work.offset_arm;
        gmBoss2BodyCatchSetArmLength(body_work, body_work.offset_arm);
        gmBoss2BodyRectApplyOffsetArm(body_work);
        gmBoss2BodyCatchArmUpdateShakePlayer(body_work, 1U);
        obsObjectWork.dir.x = (ushort)AKM_DEGtoA16(body_work.shake_pos);
        if ((ply_work.player_flag & 1024) != 0)
            body_work.flag &= 4294967287U;
        if (((int)body_work.flag & 8) != 0)
        {
            if (gmBoss2BodyCatchArmCountReleaseKey(body_work) != 0 && body_work.shake_speed == 0)
                body_work.shake_speed = 2;
            GmBsCmnUpdateObject3DNNStuckWithNode(playerObj, body_work.snm_work, body_work.snm_reg_id[2], 1);
            playerObj.pos.y += -FX_F32_TO_FX32(body_work.offset_arm) + 163840;
            if (gmBoss2BodyCatchArmCheckRelease(body_work) != 0)
            {
                body_work.flag &= 4294967287U;
                GmPlySeqChangeSequence(ply_work, 16);
                ply_work.player_flag |= 160U;
                playerObj.move_flag &= 4294958847U;
                playerObj.spd.x = 0;
                playerObj.spd.y = 0;
                playerObj.spd_add.x = 0;
                playerObj.spd_add.y = 0;
            }
        }
        if (body_work.offset_arm < 0.0 || obsObjectWork.dir.x != 0)
            return;
        body_work.offset_arm = 0.0f;
        gmBoss2BodyCatchSetArmLength(body_work, body_work.offset_arm);
        gmBoss2BodyRectApplyOffsetArm(body_work);
        if (((int)body_work.flag & 8) != 0)
            gmBoss2BodyChangeState(body_work, 4);
        else
            body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchArmUpdateEnd);
    }

    private static void gmBoss2BodyStateCatchArmUpdateEnd(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 10.0)
            return;
        obsObjectWork.user_timer = 0;
        gmBoss2BodyChangeState(body_work, 2);
        gmBoss2BodyCatchChangeArmRectNormal(body_work);
    }

    private static void gmBoss2BodyStateCatchCarryEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss2BodySetActionAllParts(body_work, 7);
        int num1 = g_gm_main_system.map_fcol.right - g_gm_main_system.map_fcol.left;
        int _x = (g_gm_main_system.map_fcol.left + num1 / 2) * 4096;
        VecFx32 dest_pos = new VecFx32(_x, obsObjectWork.pos.y, obsObjectWork.pos.z);
        int speed = 4915;
        float frame = gmBoss2BodyCalcMoveXNormalFrame(body_work, dest_pos.x, speed);
        gmBoss2BodyInitMoveNormal(body_work, dest_pos, frame);
        gmBoss2BodySetDirectionNormal(body_work);
        short dest_angle = 0;
        int flag_positive = 0;
        int num2 = 0;
        if (((int)obsObjectWork.disp_flag & 1) != 0 && _x - obsObjectWork.pos.x >= 0)
        {
            obsObjectWork.disp_flag &= 4294967294U;
            dest_angle = GMD_BOSS2_ANGLE_RIGHT;
            flag_positive = 1;
            num2 = 1;
        }
        else if (((int)obsObjectWork.disp_flag & 1) == 0 && _x - obsObjectWork.pos.x < 0)
        {
            obsObjectWork.disp_flag |= 1U;
            dest_angle = GMD_BOSS2_ANGLE_LEFT;
            flag_positive = 0;
            num2 = 1;
        }
        if (num2 != 0)
            gmBoss2BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchCarryUpdateMove);
        gmBoss2EffAfterburnerRequestCreate(body_work);
        gmBoss2BodyCatchChangeNeedleModeActive();
    }

    private static void gmBoss2BodyStateCatchCarryLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        gmBoss2EffAfterburnerRequestDelete(body_work);
        body_work.flag &= 4294967287U;
        gmBoss2BodyCatchChangeArmRectNormal(body_work);
        gmBoss2BodyCatchChangeNeedleModeWait();
    }

    private static void gmBoss2BodyStateCatchCarryUpdateMove(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        if (((int)((GMS_PLAYER_WORK)playerObj).player_flag & 1024) != 0)
            body_work.flag &= 4294967287U;
        if (((int)body_work.flag & 8) != 0)
        {
            GmBsCmnUpdateObject3DNNStuckWithNode(playerObj, body_work.snm_work, body_work.snm_reg_id[2], 1);
            playerObj.pos.y += -FX_F32_TO_FX32(body_work.offset_arm) + 163840;
        }
        float num = gmBoss2BodyUpdateTurn(body_work);
        if (0.0 < gmBoss2BodyUpdateMoveNormal(body_work) || 0.0 < num)
            return;
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchCarryUpdateOpen);
        gmBoss2EffAfterburnerRequestDelete(body_work);
        gmBoss2BodySetActionAllParts(body_work, 8);
    }

    private static void gmBoss2BodyStateCatchCarryUpdateOpen(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)playerObj;
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 40.0)
            return;
        obsObjectWork.user_timer = 0;
        if ((ply_work.player_flag & 1024) != 0)
            body_work.flag &= 4294967287U;
        if (((int)body_work.flag & 8) != 0)
        {
            body_work.flag &= 4294967287U;
            GmPlySeqChangeSequence(ply_work, 16);
            ply_work.player_flag |= 160U;
            playerObj.move_flag &= 4294958847U;
            playerObj.spd.x = 0;
            playerObj.spd.y = 0;
            playerObj.spd_add.x = 0;
            playerObj.spd_add.y = 0;
        }
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateCatchCarryUpdateEnd);
    }

    private static void gmBoss2BodyStateCatchCarryUpdateEnd(GMS_BOSS2_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        gmBoss2BodyChangeState(body_work, 2);
    }

    private static void gmBoss2BodyStatePreBallEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        gmBoss2BodySetActionAllParts(body_work, 17, 1);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePreBallUpdateAngry);
        gmBoss2EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss2BodyStatePreBallLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        gmBoss2EffAfterburnerRequestDelete(body_work);
        body_work.flag &= 4294967279U;
    }

    private static void gmBoss2BodyStatePreBallUpdateAngry(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePreBallUpdateRise);
        VecFx32 dest_pos = new VecFx32(obj_work.pos.x, obj_work.pos.y - 409600, obj_work.pos.z);
        gmBoss2BodyInitMoveNormal(body_work, dest_pos, 60f);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePreBallUpdateRise);
        GmCameraScaleSet(0.82f, 3f / 1000f);
    }

    private static void gmBoss2BodyStatePreBallUpdateRise(GMS_BOSS2_BODY_WORK body_work)
    {
        if (0.0 < gmBoss2BodyUpdateMoveNormal(body_work))
            return;
        gmBoss2BodyChangeState(body_work, 6);
    }

    private static void gmBoss2BodyStateBallMoveEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss2BodySetActionAllParts(body_work, 9);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateBallMoveUpdateMove);
        gmBoss2EffAfterburnerRequestCreate(body_work);
        VecFx32 dest_pos = new VecFx32(((int)obsObjectWork.disp_flag & 1) == 0 ? g_gm_main_system.map_fcol.right * 4096 - 348160 : g_gm_main_system.map_fcol.left * 4096 + 348160, obsObjectWork.pos.y, obsObjectWork.pos.z);
        int speed = 4915;
        float frame = gmBoss2BodyCalcMoveXNormalFrame(body_work, dest_pos.x, speed);
        gmBoss2BodyInitMoveNormal(body_work, dest_pos, frame);
    }

    private static void gmBoss2BodyStateBallMoveLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        gmBoss2EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss2BodyStateBallMoveUpdateMove(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        gmBoss2BodySetDirectionNormal(body_work);
        GMS_BOSS2_MGR_WORK mgrWork = gmBoss2MgrGetMgrWork(obsObjectWork);
        if (GmBsCmnIsFinalZoneType(obsObjectWork) != 0)
            gmBoss2BodyChangeState(body_work, 8);
        else if (8 - mgrWork.life >= 5)
        {
            if (GmBsCmnIsActionEnd(obsObjectWork) == 0)
                return;
            gmBoss2BodyChangeState(body_work, 8);
        }
        else if (gmBoss2BodyBallShootCheckTarget(body_work) != 0)
        {
            gmBoss2BodyChangeState(body_work, 7);
        }
        else
        {
            if (gmBoss2BodyUpdateMoveNormal(body_work) > 60.0)
                return;
            short dest_angle;
            int flag_positive;
            if (((int)obsObjectWork.disp_flag & 1) != 0)
            {
                obsObjectWork.disp_flag &= 4294967294U;
                dest_angle = GMD_BOSS2_ANGLE_RIGHT;
                flag_positive = 1;
            }
            else
            {
                obsObjectWork.disp_flag |= 1U;
                dest_angle = GMD_BOSS2_ANGLE_LEFT;
                flag_positive = 0;
            }
            gmBoss2BodyInitTurn(body_work, dest_angle, 60f, flag_positive);
            body_work.flag &= 4294967279U;
            body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateBallMoveUpdateTurn);
            gmBoss2EffAfterburnerRequestDelete(body_work);
        }
    }

    private static void gmBoss2BodyStateBallMoveUpdateTurn(GMS_BOSS2_BODY_WORK body_work)
    {
        double num = gmBoss2BodyUpdateMoveNormal(body_work);
        if (0.0 < gmBoss2BodyUpdateTurn(body_work))
            return;
        gmBoss2BodyChangeState(body_work, 6);
    }

    private static void gmBoss2BodyStateBallShootEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        gmBoss2BodySetActionAllParts(body_work, 10);
        GmBsCmnSetObjSpdZero(obj_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateBallShootUpdateWaitCreate);
        gmBoss2EffAfterburnerRequestDelete(body_work);
    }

    private static void gmBoss2BodyStateBallShootLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.flag |= 16U;
    }

    private static void gmBoss2BodyStateBallShootUpdateWaitCreate(
      GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work_parts1 = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK obj_work_parts2 = GmEventMgrLocalEventBirth(318, obj_work_parts1.pos.x, obj_work_parts1.pos.y, 0, 0, 0, 0, 0, 0);
        obj_work_parts2.parent_obj = obj_work_parts1;
        gmBoss2MgrAddObject(gmBoss2MgrGetMgrWork(obj_work_parts1), obj_work_parts2);
        mtTaskChangeTcbDestructor(obj_work_parts2.tcb, new GSF_TASK_PROCEDURE(gmBoss2ExitFunc));
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateBallShootUpdateCatch);
    }

    private static void gmBoss2BodyStateBallShootUpdateCatch(GMS_BOSS2_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        body_work.flag |= 4194304U;
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateBallShootUpdateShoot);
    }

    private static void gmBoss2BodyStateBallShootUpdateShoot(GMS_BOSS2_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        gmBoss2BodyChangeState(body_work, 6);
    }

    private static void gmBoss2BodyStatePrePinBallEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        gmBoss2BodySetActionAllParts(body_work, 12, 1);
        body_work.ene_3d.ene_com.rect_work[2].flag |= 4U;
        GmBsCmnSetObjSpdZero(obj_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePrePinBallUpdateWaitEffect);
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
            return;
        GmSoundChangeAngryBossBGM();
    }

    private static void gmBoss2BodyStatePrePinBallLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.flag &= 4294967279U;
    }

    private static void gmBoss2BodyStatePrePinBallUpdateWaitEffect(
      GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 119.0)
            return;
        obsObjectWork.user_timer = 0;
        gmBoss2EffBlitzInit(body_work);
        GmSoundPlaySE("FinalBoss11", body_work.se_handle);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePrePinBallUpdateWaitMotion);
    }

    private static void gmBoss2BodyStatePrePinBallUpdateWaitMotion(
      GMS_BOSS2_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        gmBoss2BodyChangeState(body_work, 9);
    }

    private static void gmBoss2BodyStatePinBallMoveEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        gmBoss2BodySetActionAllParts(body_work, 15);
        obj_work.move_flag &= 4294963199U;
        gmBoss2BodySetRectActive(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePinBallMoveUpdateMove);
        if (GmBsCmnIsFinalZoneType(obj_work) == 0)
            return;
        gmBoss2BodyCatchChangeNeedleModeActive();
    }

    private static void gmBoss2BodyStatePinBallMoveLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        GMM_BS_OBJ(body_work).move_flag |= 4096U;
    }

    private static void gmBoss2BodyStatePinBallMoveUpdateMove(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        int num = 0;
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
            num = FX_Mul(num, 6144);
        gmBoss2BodyPinBallAdjustMoveSpeed(body_work, num);
        gmBoss2BodyUpdateMovePinBall(body_work);
        gmBoss2BodySetDirectionNormal(body_work);
        ++body_work.counter_pinball;
        if (body_work.counter_pinball == 90U && GmBsCmnIsFinalZoneType(obj_work) != 0)
            gmBoss2BodyCatchChangeNeedleModeWait();
        if (body_work.counter_pinball < 360U)
            return;
        body_work.counter_pinball = 0U;
        gmBoss2BodyChangeState(body_work, 10);
    }

    private static void gmBoss2BodyStatePinBallRollEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        obj_work.move_flag &= 4294963199U;
        GmBsCmnSetObjSpdZero(obj_work);
        gmBoss2BodySetActionAllParts(body_work, 15);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePinBallRollUpdateSearch);
    }

    private static void gmBoss2BodyStatePinBallRollLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        GMM_BS_OBJ(body_work).move_flag |= 4096U;
        body_work.flag &= 4294967231U;
    }

    private static void gmBoss2BodyStatePinBallRollUpdateSearch(GMS_BOSS2_BODY_WORK body_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(body_work)) == 0)
            return;
        gmBoss2BodySetActionAllParts(body_work, 16);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePinBallRollUpdateFind);
    }

    private static void gmBoss2BodyStatePinBallRollUpdateFind(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        if (gmBoss2BodyPinBallCheckTurn(body_work) != 0)
        {
            short dest_angle;
            int flag_positive;
            if (((int)obj_work.disp_flag & 1) != 0)
            {
                obj_work.disp_flag &= 4294967294U;
                dest_angle = GMD_BOSS2_ANGLE_RIGHT;
                flag_positive = 1;
            }
            else
            {
                obj_work.disp_flag |= 1U;
                dest_angle = GMD_BOSS2_ANGLE_LEFT;
                flag_positive = 0;
            }
            gmBoss2BodyInitTurn(body_work, dest_angle, 20f, flag_positive);
        }
        gmBoss2BodySetActionAllParts(body_work, 14);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePinBallRollReady);
    }

    private static void gmBoss2BodyStatePinBallRollReady(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        float num1 = gmBoss2BodyUpdateTurn(body_work);
        ++obj_work.user_timer;
        if (obj_work.user_timer < 10 || 0.0 < num1)
            return;
        obj_work.user_timer = 0;
        int num2 = 20480;
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
            num2 = FX_Mul(num2, 6144);
        OBS_OBJECT_WORK playerObj = GmBsCmnGetPlayerObj();
        gmBoss2BodyInitMovePinBall(body_work, playerObj.pos, num2);
        gmBoss2BodySetRectRoll(body_work);
        gmBoss2BodySetActionAllParts(body_work, 14);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePinBallRollUpdateMove);
        gmBoss2EffCreateRollModel(body_work);
    }

    private static void gmBoss2BodyStatePinBallRollUpdateMove(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        int num = 20480;
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
            num = FX_Mul(num, 6144);
        gmBoss2BodyPinBallAdjustMoveSpeed(body_work, num);
        gmBoss2BodyUpdateMovePinBall(body_work);
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.dir.z -= (ushort)GMD_BOSS2_BODY_PINBALL_ROLL_ROT_Z;
        else
            obj_work.dir.z += (ushort)GMD_BOSS2_BODY_PINBALL_ROLL_ROT_Z;
        gmBoss2BodySetDirectionRoll(body_work);
        body_work.flag |= 64U;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 180 || gmBoss2BodyPinBallCheckAreaStop(body_work) == 0)
            return;
        obj_work.user_timer = 0;
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStatePinBallRollUpdateStop);
        body_work.flag &= 4294967263U;
        gmBoss2EffCreateRollModelLost(body_work);
    }

    private static void gmBoss2BodyStatePinBallRollUpdateStop(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        int num = 20480;
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
            num = FX_Mul(num, 6144);
        gmBoss2BodyPinBallAdjustMoveSpeed(body_work, num);
        gmBoss2BodyUpdateMovePinBall(body_work);
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.dir.z -= (ushort)GMD_BOSS2_BODY_PINBALL_ROLL_ROT_Z;
        else
            obj_work.dir.z += (ushort)GMD_BOSS2_BODY_PINBALL_ROLL_ROT_Z;
        gmBoss2BodySetDirectionRoll(body_work);
        ++obj_work.user_timer;
        if (obj_work.user_timer < 10 || gmBoss2BodyPinBallCheckAreaStop(body_work) == 0)
            return;
        obj_work.dir.z = 0;
        obj_work.user_timer = 0;
        obj_work.dir.z = 0;
        gmBoss2BodyChangeState(body_work, 9);
    }

    private static void gmBoss2BodyStateDefeatEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        obj_work.flag |= 2U;
        obj_work.disp_flag |= 16U;
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
        GmBsCmnSetObjSpdZero(obj_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateDefeatUpdateStart);
        body_work.flag &= 4294967291U;
        GmSoundStopSE(body_work.se_handle);
        body_work.flag &= 4294967263U;
        gmBoss2BodyCatchChangeNeedleModeWait();
        GmSoundChangeWinBossBGM();
    }

    private static void gmBoss2BodyStateDefeatLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        obsObjectWork.disp_flag &= 4294967279U;
        obsObjectWork.flag &= 4294967293U;
    }

    private static void gmBoss2BodyStateDefeatUpdateStart(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 40)
            return;
        obsObjectWork.user_timer = 0;
        OBS_OBJECT_WORK parent_obj = GMM_BS_OBJ(body_work);
        gmBoss2EffBombsInit(body_work.bomb_work, parent_obj, parent_obj.pos.x, parent_obj.pos.y, 327680, 327680, 10U, 30U);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateDefeatUpdateFall);
    }

    private static void gmBoss2BodyStateDefeatUpdateFall(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 120)
        {
            gmBoss2EffBombsUpdate(body_work.bomb_work);
        }
        else
        {
            obsObjectWork.user_timer = 0;
            obsObjectWork.move_flag |= 128U;
            body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateDefeatUpdateExplode);
        }
    }

    private static void gmBoss2BodyStateDefeatUpdateExplode(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        int num = g_gm_main_system.map_fcol.bottom * 4096 - 614400;
        if (obj_work.pos.y < num)
            return;
        obj_work.move_flag &= 4294967167U;
        GmBsCmnSetObjSpdZero(obj_work);
        body_work.flag |= 134217728U;
        GmSoundPlaySE("Boss0_03");
        GMM_PAD_VIB_MID_TIME(120f);
        GmBsCmnInitFlashScreen(body_work.flash_work, 4f, 5f, 30f);
        OBS_OBJECT_WORK parent_obj = GMM_BS_OBJ(body_work);
        ((OBS_OBJECT_WORK)GmEfctCmnEsCreate(parent_obj, 8)).pos.z = parent_obj.pos.z + 131072;
        GmPlayerAddScoreNoDisp((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj(), 1000);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateDefeatUpdateScatter);
    }

    private static void gmBoss2BodyStateDefeatUpdateScatter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        GmBsCmnUpdateFlashScreen(body_work.flash_work);
        ++obj_work.user_timer;
        if (obj_work.user_timer < 40)
            return;
        obj_work.user_timer = 0;
        gmBoss2ChangeTextureBurnt(obj_work);
        body_work.flag |= 16777216U;
        gmBoss2EffAfterburnerSmokeInit(body_work);
        gmBoss2EffBodySmokeInit(body_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateDefeatUpdateEnd);
        gmBoss2EffScatterInit(body_work);
        GmCameraScaleSet(1f, 0.0015f);
        GmMapSetDrawMarginNormal();
    }

    private static void gmBoss2BodyStateDefeatUpdateEnd(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 120)
            return;
        obsObjectWork.user_timer = 0;
        gmBoss2BodyChangeState(body_work, 12);
    }

    private static void gmBoss2BodyStateEscapeEnter(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(body_work);
        obj_work.spd.x = 0;
        obj_work.spd_add.x = ((int)obj_work.disp_flag & 1) == 0 ? 0 : 0;
        obj_work.spd_add.y = -655;
        obj_work.flag |= 2U;
        obj_work.move_flag |= 4352U;
        gmBoss2BodySetDirectionNormal(body_work);
        gmBoss2BodySetActionAllParts(body_work, 18, 1);
        body_work.flag |= 8388608U;
        body_work.proc_update = GmBsCmnIsFinalZoneType(obj_work) == 0 ? new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateEscapeUpdateScrollLock) : new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateEscapeUpdateFinalZone);
        GmMapSetMapDrawSize(1);
    }

    private static void gmBoss2BodyStateEscapeLeave(GMS_BOSS2_BODY_WORK body_work)
    {
        GMM_BS_OBJ(body_work).flag &= 4294967293U;
    }

    private static void gmBoss2BodyStateEscapeUpdateScrollLock(GMS_BOSS2_BODY_WORK body_work)
    {
        gmBoss2BodyEscapeAddjustSpeed(body_work);
        if (gmBoss2BodyEscapeCheckScrollUnlock(body_work) == 0)
            return;
        GmGmkCamScrLimitRelease(4);
        OBS_OBJECT_WORK obj_work = gmBoss2BodySearchShutterOut();
        if (obj_work != null)
            GmGmkShutterOutChangeModeOpen(obj_work);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateEscapeUpdateWaitScreenOut);
        GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 1U);
    }

    private static void gmBoss2BodyStateEscapeUpdateWaitScreenOut(
      GMS_BOSS2_BODY_WORK body_work)
    {
        gmBoss2BodyEscapeAddjustSpeed(body_work);
        if (gmBoss2BodyEscapeCheckScreenOut(body_work) == 0)
            return;
        gmBoss2MgrGetMgrWork(GMM_BS_OBJ(body_work)).flag |= 2U;
        body_work.proc_update = null;
    }

    private static void gmBoss2BodyStateEscapeUpdateFinalZone(GMS_BOSS2_BODY_WORK body_work)
    {
        gmBoss2BodyEscapeAddjustSpeed(body_work);
        if (gmBoss2BodyEscapeCheckScrollUnlock(body_work) == 0)
            return;
        GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 1U);
        body_work.proc_update = new MPP_VOID_GMS_BOSS2_BODY_WORK(gmBoss2BodyStateEscapeUpdateWaitScreenOut);
    }

    private static void gmBoss2BodyMainFuncWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK body_work = (GMS_BOSS2_BODY_WORK)obj_work;
        if (gmBoss2MgrCheckSetupComplete(gmBoss2MgrGetMgrWork(obj_work)) == 0)
            return;
        GmBsCmnInitBossMotionCBSystem(obj_work, body_work.bmcb_mgr);
        GmBsCmnCreateSNMWork(body_work.snm_work, obj_work.obj_3d._object, 15);
        GmBsCmnAppendBossMotionCallback(body_work.bmcb_mgr, body_work.snm_work.bmcb_link);
        for (int index = 0; 15 > index; ++index)
            body_work.snm_reg_id[index] = GmBsCmnRegisterSNMNode(body_work.snm_work, g_boss2_node_index_list[index]);
        GmBsCmnCreateCNMMgrWork(body_work.cnm_mgr_work, obj_work.obj_3d._object, 13);
        GmBsCmnInitCNMCb(obj_work, body_work.cnm_mgr_work);
        for (int index = 0; 13 > index; ++index)
            body_work.cnm_reg_id[index] = GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, g_boss2_node_index_list[2 + index]);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2BodyMainFunc);
        gmBoss2BodyChangeState(body_work, 1);
    }

    private static void gmBoss2BodyMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK gmsBosS2BodyWork = (GMS_BOSS2_BODY_WORK)obj_work;
        gmBoss2BodyUpdateNoHitTime(gmsBosS2BodyWork);
        gmBoss2BodyUpdateInvincibleTime(gmsBosS2BodyWork);
        if (gmsBosS2BodyWork.proc_update != null)
            gmsBosS2BodyWork.proc_update(gmsBosS2BodyWork);
        if (((int)gmsBosS2BodyWork.flag & 33554432) != 0)
            gmBoss2EffAfterburnerInit(gmsBosS2BodyWork);
        if (((int)gmsBosS2BodyWork.flag & int.MinValue) != 0)
        {
            gmsBosS2BodyWork.flag &= 1073741823U;
            gmBoss2BodyChangeState(gmsBosS2BodyWork, 11);
        }
        else
        {
            if (((int)gmsBosS2BodyWork.flag & 1073741824) != 0)
            {
                gmsBosS2BodyWork.flag &= 3221225471U;
                gmsBosS2BodyWork.flag |= 536870912U;
                GmBsCmnInitObject3DNNDamageFlicker(obj_work, gmsBosS2BodyWork.flk_work, 32f);
            }
            GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, gmsBosS2BodyWork.flk_work);
            gmBoss2BodyUpdateDirection(gmsBosS2BodyWork);
        }
    }

    private static void gmBoss2EggChangeAction(GMS_BOSS2_EGG_WORK egg_work, int action_id)
    {
        gmBoss2EggChangeAction(egg_work, action_id, 0);
    }

    private static void gmBoss2EggChangeAction(
      GMS_BOSS2_EGG_WORK egg_work,
      int action_id,
      int force_change)
    {
        GMS_BOSS2_PART_ACT_INFO bosS2PartActInfo = gm_boss2_egg_act_info_tbl[action_id];
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        if (force_change == 0 && egg_work.egg_action_id == action_id && ((int)egg_work.flag & 1) != 0)
            return;
        egg_work.egg_action_id = action_id;
        egg_work.flag |= 1U;
        if (bosS2PartActInfo.is_maintain != 0)
        {
            if (bosS2PartActInfo.is_repeat != 0)
                obj_work.disp_flag |= 4U;
        }
        else
            GmBsCmnSetAction(obj_work, bosS2PartActInfo.mtn_id, bosS2PartActInfo.is_repeat, bosS2PartActInfo.is_blend);
        obj_work.obj_3d.speed[0] = bosS2PartActInfo.mtn_spd;
        obj_work.obj_3d.blend_spd = bosS2PartActInfo.blend_spd;
    }

    private static void gmBoss2EggRevertAction(GMS_BOSS2_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(parentObj);
        egg_work.flag &= 4294967294U;
        GMS_BOSS2_PART_ACT_INFO bosS2PartActInfo = gm_boss2_act_info_tbl[parentObj.action_id][1];
        GmBsCmnSetAction(obj_work, bosS2PartActInfo.mtn_id, bosS2PartActInfo.is_repeat, 1);
        obj_work.obj_3d.frame[0] = obsObjectWork.obj_3d.frame[0];
    }

    private static void gmBoss2EggStateIdleInit(GMS_BOSS2_EGG_WORK egg_work)
    {
        egg_work.proc_update = new MPP_VOID_GMS_BOSS2_EGG_WORK(gmBoss2EggStateIdleUpdate);
    }

    private static void gmBoss2EggStateIdleUpdate(GMS_BOSS2_EGG_WORK egg_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)GMM_BS_OBJ(egg_work).parent_obj;
        if (((int)parentObj.flag & 268435456) == 0)
            return;
        parentObj.flag &= 4026531839U;
        gmBoss2EggStateLaughInit(egg_work);
    }

    private static void gmBoss2EggStateLaughInit(GMS_BOSS2_EGG_WORK egg_work)
    {
        if (((int)GMM_BS_OBJ(egg_work).parent_obj.disp_flag & 1) != 0)
            gmBoss2EggChangeAction(egg_work, 0);
        else
            gmBoss2EggChangeAction(egg_work, 1);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS2_EGG_WORK(gmBoss2EggStateLaughUpdate);
    }

    private static void gmBoss2EggStateLaughUpdate(GMS_BOSS2_EGG_WORK egg_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(egg_work)) == 0)
            return;
        gmBoss2EggRevertAction(egg_work);
        gmBoss2EggStateIdleInit(egg_work);
    }

    private static void gmBoss2EggStateDamageInit(GMS_BOSS2_EGG_WORK egg_work)
    {
        gmBoss2EggChangeAction(egg_work, 2);
        gmBoss2EffSweatInit(egg_work);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS2_EGG_WORK(gmBoss2EggStateDamageUpdate);
    }

    private static void gmBoss2EggStateDamageUpdate(GMS_BOSS2_EGG_WORK egg_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(egg_work)) == 0)
            return;
        egg_work.flag &= 4294967293U;
        gmBoss2EggRevertAction(egg_work);
        gmBoss2EggStateIdleInit(egg_work);
    }

    private static void gmBoss2EggStateEscapeInit(GMS_BOSS2_EGG_WORK egg_work)
    {
        if (((int)egg_work.flag & 2) == 0)
            gmBoss2EffSweatInit(egg_work);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS2_EGG_WORK(gmBoss2EggStateEscapeUpdate);
    }

    private static void gmBoss2EggStateEscapeUpdate(GMS_BOSS2_EGG_WORK egg_work)
    {
        UNREFERENCED_PARAMETER(egg_work);
    }

    private static void gmBoss2EggmanMainFuncWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        if (gmBoss2MgrCheckSetupComplete(gmBoss2MgrGetMgrWork(((GMS_BOSS2_BODY_WORK)obj_work.parent_obj).ene_3d.ene_com.obj_work)) == 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EggmanMainFunc);
        gmBoss2EggStateIdleInit((GMS_BOSS2_EGG_WORK)obj_work);
    }

    private static void gmBoss2EggmanMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS2_EGG_WORK gmsBosS2EggWork = (GMS_BOSS2_EGG_WORK)obj_work;
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
        if (gmsBosS2EggWork.proc_update != null)
            gmsBosS2EggWork.proc_update(gmsBosS2EggWork);
        if (((int)parentObj.flag & 8388608) != 0)
        {
            parentObj.flag &= 4286578687U;
            gmBoss2EggStateEscapeInit(gmsBosS2EggWork);
        }
        if (((int)parentObj.flag & 536870912) != 0)
        {
            parentObj.flag &= 3758096383U;
            gmBoss2EggStateDamageInit(gmsBosS2EggWork);
        }
        if (((int)parentObj.flag & 16777216) != 0)
        {
            parentObj.flag &= 4278190079U;
            gmBoss2ChangeTextureBurnt(obj_work);
        }
        if (((int)GMM_BS_OBJ(parentObj).disp_flag & 16) != 0)
            obj_work.disp_flag |= 16U;
        else
            obj_work.disp_flag &= 4294967279U;
        if (((int)parentObj.flag & 64) != 0)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
    }

    private static void gmBoss2BallHitFunc(
      OBS_RECT_WORK own_rect,
      OBS_RECT_WORK target_rect)
    {
        ((GMS_BOSS2_BODY_WORK)own_rect.parent_obj.parent_obj).flag |= 268435456U;
    }

    private static void gmBoss2BallMainFuncWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        if (gmBoss2MgrCheckSetupComplete(gmBoss2MgrGetMgrWork(GMM_BS_OBJ((GMS_BOSS2_BODY_WORK)obj_work.parent_obj))) == 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2BallMainFunc);
        gmBoss2BallInit((GMS_BOSS2_BALL_WORK)obj_work);
    }

    private static void gmBoss2BallMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BALL_WORK wrk = (GMS_BOSS2_BALL_WORK)obj_work;
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    private static void gmBoss2BallInit(GMS_BOSS2_BALL_WORK ball_work)
    {
        ball_work.proc_update = new MPP_VOID_GMS_BOSS2_BALL_WORK(gmBoss2BallUpdateCatch);
    }

    private static void gmBoss2BallUpdateCatch(GMS_BOSS2_BALL_WORK ball_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(ball_work);
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[1], 1);
        ball_work.proc_update = new MPP_VOID_GMS_BOSS2_BALL_WORK(gmBoss2BallUpdateWaitShoot);
    }

    private static void gmBoss2BallUpdateWaitShoot(GMS_BOSS2_BALL_WORK ball_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(ball_work);
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[1], 1);
        if (((int)parentObj.flag & 4194304) == 0)
            return;
        parentObj.flag &= 4290772991U;
        ball_work.proc_update = new MPP_VOID_GMS_BOSS2_BALL_WORK(gmBoss2BallUpdateShoot);
        obj_work.move_flag |= 128U;
    }

    private static void gmBoss2BallUpdateShoot(GMS_BOSS2_BALL_WORK ball_work)
    {
        if (((int)GMM_BS_OBJ(ball_work).move_flag & 1) == 0)
            return;
        ball_work.proc_update = new MPP_VOID_GMS_BOSS2_BALL_WORK(gmBoss2BallUpdateWaitBomb);
    }

    private static void gmBoss2BallUpdateWaitBomb(GMS_BOSS2_BALL_WORK ball_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(ball_work);
        ++obsObjectWork.user_timer;
        if (obsObjectWork.user_timer < 120)
            return;
        obsObjectWork.user_timer = 0;
        ball_work.proc_update = new MPP_VOID_GMS_BOSS2_BALL_WORK(gmBoss2BallUpdateFlicker);
        GmBsCmnInitObject3DNNDamageFlicker(obsObjectWork, ball_work.flk_work, 16f);
        gmBoss2EffBallBombInit(obsObjectWork.pos, obsObjectWork);
    }

    private static void gmBoss2BallUpdateFlicker(GMS_BOSS2_BALL_WORK ball_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(ball_work);
        OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        if (GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, ball_work.flk_work) == 0)
            return;
        gmBoss2EffBallBombPartInit(obj_work.pos, parentObj, 4096);
        gmBoss2EffBallBombPartInit(obj_work.pos, parentObj, -4096);
        ((OBS_OBJECT_WORK)GmEfctCmnEsCreate(null, 10)).pos.Assign(obj_work.pos);
        obj_work.flag |= 4U;
        GmSoundPlaySE(GMD_ENE_KAMA_SE_BOMB);
    }


}