public partial class AppMain
{
    public static void GmBoss5EfctBuild()
    {
        for (int index = 0; index < 27; ++index)
        {
            GMS_BOSS5_EFCT_DATA_INFO bosS5EfctDataInfo = gm_boss5_efct_data_info_tbl[index];
            OBS_DATA_WORK model_dwork = null;
            OBS_DATA_WORK object_dwork = null;
            if (bosS5EfctDataInfo.use_model != 0)
            {
                model_dwork = ObjDataGet(bosS5EfctDataInfo.model_dwork_no);
                object_dwork = ObjDataGet(bosS5EfctDataInfo.object_dwork_no);
            }
            GmEfctBossBuildSingleDataReg(bosS5EfctDataInfo.tex_amb_arc_idx, ObjDataGet(bosS5EfctDataInfo.tex_amb_dwork_no), ObjDataGet(bosS5EfctDataInfo.tex_list_dwork_no), bosS5EfctDataInfo.model_arc_idx, model_dwork, object_dwork, g_gm_gamedat_enemy_arc);
        }
    }

    public static void GmBoss5EfctFlush()
    {
        GmEfctBossFlushSingleDataInit();
    }

    public static void GmBoss5EfctTryStartLeakage(GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 2048) != 0)
            return;
        GmSoundPlaySE("FinalBoss11", body_work.se_hnd_leakage);
        gmBoss5EfctCreateLeakage(body_work);
        body_work.flag |= 2048U;
    }

    public static void GmBoss5EfctEndLeakage(GMS_BOSS5_BODY_WORK body_work)
    {
        GmBoss5EfctEndLeakage(body_work, 0);
    }

    public static void GmBoss5EfctEndLeakage(GMS_BOSS5_BODY_WORK body_work, int no_vanish)
    {
        if (((int)body_work.flag & 2048) != 0)
            GsSoundStopSeHandle(body_work.se_hnd_leakage, 30);
        body_work.flag &= 4294965247U;
    }

    public static void GmBoss5EfctStartPrelimLeakage(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag |= 1048576U;
        gmBoss5EfctCreatePrelimLeakage(body_work);
    }

    public static void GmBoss5EfctEndPrelimLeakage(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4293918719U;
    }

    public static void GmBoss5EfctCreateWalkStepSmoke(
      GMS_BOSS5_BODY_WORK body_work,
      int leg_type)
    {
    }

    public static void GmBoss5EfctCreateRunStepSmoke(
      GMS_BOSS5_BODY_WORK body_work,
      int leg_type)
    {
    }

    public static void GmBoss5EfctCreateBerserkStampSmoke(
      GMS_BOSS5_BODY_WORK body_work,
      int leg_type,
      uint spawn_delay)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)(GMS_EFFECT_COM_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 9, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        s5EfctGeneralWork.ref_node_snm_id = body_work.leg_snm_reg_ids[leg_type];
        obsObjectWork.disp_flag |= 4128U;
        s5EfctGeneralWork.timer = spawn_delay;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctBerserkStampSmokeProcWaitStart);
    }

    public static void GmBoss5EfctCreateCrashLandingSmoke(GMS_BOSS5_BODY_WORK body_work)
    {
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 10, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_EFFECT_COM_WORK gmsEffectComWork = (GMS_EFFECT_COM_WORK)s5EfctGeneralWork;
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(s5EfctGeneralWork);
        obsObjectWork.pos.x = GMM_BS_OBJ(body_work).pos.x;
        obsObjectWork.pos.y = body_work.ground_v_pos;
        obsObjectWork.pos.z = GMM_BS_OBJ(body_work).pos.z + 262144;
    }

    public static void GmBoss5EfctCreateBreakingGlass(OBS_OBJECT_WORK parent_obj)
    {
        if (ObjViewOutCheck(parent_obj.pos.x, parent_obj.pos.y, 0, 0, (short)-(OBD_OBJ_CLIP_LCD_Y / 2), 0, (short)(OBD_OBJ_CLIP_LCD_Y / 2)) != 0)
            return;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)gmBoss5EfctEsCreate(parent_obj, 11, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_EFFECT_COM_WORK gmsEffectComWork = (GMS_EFFECT_COM_WORK)s5EfctGeneralWork;
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(s5EfctGeneralWork);
        obsObjectWork.pos.y += -131072;
        obsObjectWork.pos.z += 262144;
    }

    public static void GmBoss5EfctStartJet(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag |= 16384U;
        gmBoss5EfctCreateJet(body_work);
    }

    public static void GmBoss5EfctEndJet(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294950911U;
    }

    public static void GmBoss5EfctStartJetSmoke(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag |= 32768U;
        gmBoss5EfctCreateJetSmoke(body_work);
    }

    public static void GmBoss5EfctEndJetSmoke(GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294934527U;
    }

    public static void GmBoss5EfctTryStartRocketLeakage(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (((int)rkt_work.flag & 4) != 0)
            return;
        gmBoss5EfctCreateRocketLeakage(rkt_work);
        rkt_work.flag |= 4U;
    }

    public static void GmBoss5EfctEndRocketLeakage(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.flag &= 4294967291U;
    }

    public static void GmBoss5EfctCreateRocketLaunch(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
    }

    public static void GmBoss5EfctCreateRocketDock(
      GMS_BOSS5_BODY_WORK body_work,
      int rkt_type)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)(GMS_EFFECT_COM_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 15, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        float x;
        if (rkt_type == 0)
        {
            s5EfctGeneralWork.ref_node_snm_id = body_work.armpt_snm_reg_ids[0][2];
            x = 2f;
        }
        else
        {
            s5EfctGeneralWork.ref_node_snm_id = body_work.armpt_snm_reg_ids[1][2];
            x = -2f;
        }
        nnMakeTranslateMatrix(s5EfctGeneralWork.ofst_mtx, x, 0.0f, 0.0f);
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctRocketDockProcMain);
    }

    public static void GmBoss5EfctStartRocketJet(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (((int)rkt_work.flag & 16) != 0)
            return;
        gmBoss5EfctCreateRocketJet(rkt_work, 0);
        rkt_work.flag |= 16U;
    }

    public static void GmBoss5EfctEndRocketJet(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.flag &= 4294967279U;
    }

    public static void GmBoss5EfctStartRocketJetReverse(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (((int)rkt_work.flag & 32) != 0)
            return;
        gmBoss5EfctCreateRocketJet(rkt_work, 1);
        rkt_work.flag |= 32U;
    }

    public static void GmBoss5EfctEndRocketJetReverse(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.flag &= 4294967263U;
    }

    public static void GmBoss5EfctCreateRocketLandingShockwave(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
    }

    public static void GmBoss5EfctCreateLandingShockwave(GMS_BOSS5_BODY_WORK body_work)
    {
        GMS_EFFECT_COM_WORK gmsEffectComWork = (GMS_EFFECT_COM_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 19, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)gmsEffectComWork;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        obsObjectWork.pos.z += 262144;
        GmBsCmnSetEfctAtkVsPly((GMS_EFFECT_COM_WORK)obsObjectWork, 128);
        obsObjectWork.flag |= 16U;
        ObjRectWorkSet(gmsEffectComWork.rect_work[1], -88, -32, 88, 32);
        gmsEffectComWork.rect_work[1].flag &= 4294965247U;
        s5EfctGeneralWork.timer = 8U;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctLandingShockwaveProcMain);
    }

    public static void GmBoss5EfctCreateStrikeShockwave(
      GMS_BOSS5_BODY_WORK body_work,
      uint spawn_delay)
    {
        GMS_EFFECT_COM_WORK gmsEffectComWork = (GMS_EFFECT_COM_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 20, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)gmsEffectComWork;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        GmBsCmnSetEfctAtkVsPly((GMS_EFFECT_COM_WORK)obsObjectWork, 64);
        obsObjectWork.flag |= 16U;
        ObjRectWorkSet(gmsEffectComWork.rect_work[1], -64, -64, 64, 32);
        gmsEffectComWork.rect_work[1].flag &= 4294965247U;
        obsObjectWork.flag |= 2U;
        obsObjectWork.disp_flag |= 4128U;
        s5EfctGeneralWork.timer = spawn_delay;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctStrikeShockwaveProcWaitStart);
    }

    public static void GmBoss5EfctTargetCursorInit(GMS_BOSS5_BODY_WORK body_work)
    {
        gmBoss5EfctCreateTargetCursorStart(body_work);
    }

    public static void GmBoss5EfctCrashCursorInit(
      GMS_BOSS5_BODY_WORK body_work,
      int pos_x,
      uint duration_time)
    {
        gmBoss5EfctCreateCrashCursorStart(body_work, pos_x, duration_time);
    }

    public static void GmBoss5EfctCreateVulcanFire(
      GMS_BOSS5_TURRET_WORK trt_work,
      VecFx32 pos,
      int angle)
    {
        UNREFERENCED_PARAMETER(trt_work);
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctCmnEsCreate(GMM_BS_OBJ(trt_work), 14);
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)efct_3des;
        obsObjectWork.dir.z = (ushort)(short)(ushort.MaxValue & (long)angle);
        GmEffect3DESAddDispRotation(efct_3des, (short)AKM_DEGtoA32(90), 0, 0);
        obsObjectWork.pos.Assign(pos);
        GmSoundPlaySE("FinalBoss14");
    }

    public static void GmBoss5EfctCreateVulcanBullet(
      GMS_BOSS5_TURRET_WORK trt_work,
      VecFx32 pos,
      int angle,
      int spd)
    {
        UNREFERENCED_PARAMETER(trt_work);
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctCmnEsCreate(GMM_BS_OBJ(trt_work), 15);
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)efct_3des;
        GmBsCmnSetEfctAtkVsPly(efct_3des.efct_com, 16);
        obsObjectWork.flag |= 16U;
        OBS_RECT_WORK pRec = efct_3des.efct_com.rect_work[1];
        ObjRectWorkSet(pRec, -8, -8, 8, 8);
        pRec.flag |= 4U;
        obsObjectWork.dir.z = (ushort)(short)(ushort.MaxValue & (long)angle);
        GmEffect3DESAddDispRotation(efct_3des, (short)AKM_DEGtoA32(-90), 0, 0);
        obsObjectWork.pos.Assign(pos);
        obsObjectWork.pos.z += 262144;
        obsObjectWork.spd.x = FX_Mul(spd, FX_F32_TO_FX32(nnCos(angle)));
        obsObjectWork.spd.y = FX_Mul(spd, FX_F32_TO_FX32(nnSin(angle)));
        obsObjectWork.spd.z = 0;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctVulcanBulletProcMain);
    }

    public static void GmBoss5EfctCreateSmallExplosion(int pos_x, int pos_y, int pos_z)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(null, 7);
        obsObjectWork.pos.x = pos_x;
        obsObjectWork.pos.y = pos_y;
        obsObjectWork.pos.z = pos_z;
    }

    public static void GmBoss5EfctCreateBigExplosion(int pos_x, int pos_y, int pos_z)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(null, 8);
        obsObjectWork.pos.x = pos_x;
        obsObjectWork.pos.y = pos_y;
        obsObjectWork.pos.z = pos_z;
    }

    public static void GmBoss5EfctCreateFragments(int pos_x, int pos_y, int pos_z)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctBossCmnEsCreate(null, 0U);
        obsObjectWork.pos.x = pos_x;
        obsObjectWork.pos.y = pos_y;
        obsObjectWork.pos.z = pos_z;
    }

    public static void GmBoss5EfctCreateDamage(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctBossCmnEsCreate(null, 0U);
        obsObjectWork.pos.x = body_work.part_obj_core.pos.x;
        obsObjectWork.pos.y = body_work.part_obj_core.pos.y;
        obsObjectWork.pos.z = body_work.part_obj_core.pos.z + 262144;
    }

    public static void GmBoss5EfctStartRocketSmoke(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.flag |= 64U;
        gmBoss5EfctCreateRocketSmoke(rkt_work);
    }

    public static void GmBoss5EfctEndRocketSmoke(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.flag &= 4294967231U;
    }

    public static void GmBoss5EfctBreakdownSmokesInit(
      GMS_BOSS5_BODY_WORK body_work,
      uint duration_time)
    {
        for (uint index = 0; index < 2U; ++index)
        {
            GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 3U);
            OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)efct_3des;
            GmEffect3DESChangeBase(efct_3des, 1U, (uint)(efct_3des.saved_init_flag & 18446744073709551613UL));
            GmEffect3DESSetDispOffset(efct_3des, gm_boss5_efct_breakdown_smoke_disp_ofst_tbl[(int)index][0], gm_boss5_efct_breakdown_smoke_disp_ofst_tbl[(int)index][1], gm_boss5_efct_breakdown_smoke_disp_ofst_tbl[(int)index][2]);
            GmEffect3DESSetDispRotation(efct_3des, gm_boss5_efct_breakdown_smoke_disp_rot_tbl[(int)index][0], gm_boss5_efct_breakdown_smoke_disp_rot_tbl[(int)index][1], gm_boss5_efct_breakdown_smoke_disp_rot_tbl[(int)index][2]);
            obsObjectWork.user_timer = (int)duration_time;
            obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctBreakdownSmokeProcLoop);
        }
    }

    public static void GmBoss5EfctBodySmallSmokesInit(GMS_BOSS5_BODY_WORK body_work)
    {
        for (uint part_idx = 0; part_idx < 3U; ++part_idx)
            gmBoss5EfctCreateBodySmallSmoke(body_work, part_idx);
    }

    public static void GmBoss5EfctBerserkSteamInit(GMS_BOSS5_BODY_WORK body_work, uint count)
    {
        for (uint part_idx = 0; part_idx < 2U; ++part_idx)
            gmBoss5EfctCreateBerserkSteam(body_work, count, part_idx);
    }

    public static void GmBoss5EfctStartEggSweat(GMS_BOSS5_EGG_WORK egg_work)
    {
        egg_work.flag |= GMD_BOSS5_EGG_FLAG_SWEAT_ACTIVE;
        gmBoss5EfctCreateEggSweat(egg_work);
    }

    public static void GmBoss5EfctEndEggSweat(GMS_BOSS5_EGG_WORK egg_work)
    {
        egg_work.flag &= ~GMD_BOSS5_EGG_FLAG_SWEAT_ACTIVE;
    }

    public static void GmBoss5EfctCreateRocketRollSpark(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)GmEfctCmnEsCreate(GMM_BS_OBJ(rkt_work), 48);
        obsObjectWork.pos.y = ((GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(rkt_work).parent_obj).ground_v_pos;
        obsObjectWork.dir.z = GMD_BOSS5_EFCT_ROCKET_ROLLING_SPARK_DIR_Z;
        obsObjectWork.pos.z += 131072;
    }

    public static GMS_EFFECT_3DES_WORK gmBoss5EfctEsCreate(
      OBS_OBJECT_WORK parent_obj,
      int efct_idx)
    {
        return gmBoss5EfctEsCreate(parent_obj, efct_idx, () => new GMS_EFFECT_3DES_WORK());
    }

    public static GMS_EFFECT_3DES_WORK gmBoss5EfctEsCreate(
      OBS_OBJECT_WORK parent_obj,
      int efct_idx,
      TaskWorkFactoryDelegate work_size)
    {
        GMS_EFFECT_CREATE_PARAM create_param = gm_boss5_efct_create_param_tbl[efct_idx];
        GMS_BOSS5_EFCT_DATA_INFO bosS5EfctDataInfo = gm_boss5_efct_data_info_tbl[efct_idx];
        OBS_DATA_WORK model_dwork;
        OBS_DATA_WORK object_dwork;
        if (create_param.model_idx != -1)
        {
            model_dwork = ObjDataGet(bosS5EfctDataInfo.model_dwork_no);
            object_dwork = ObjDataGet(bosS5EfctDataInfo.object_dwork_no);
        }
        else
        {
            model_dwork = null;
            object_dwork = null;
        }
        return GmEffect3dESCreateByParam(create_param, parent_obj, g_gm_gamedat_enemy_arc, ObjDataGet(bosS5EfctDataInfo.ame_dwork_no), ObjDataGet(bosS5EfctDataInfo.tex_amb_dwork_no), ObjDataGet(bosS5EfctDataInfo.tex_list_dwork_no), model_dwork, object_dwork, work_size);
    }

    public static void gmBoss5EfctCreateLeakage(GMS_BOSS5_BODY_WORK body_work)
    {
        GMM_BS_OBJ((GMS_BOSS5_EFCT_GENERAL_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 7, () => new GMS_BOSS5_EFCT_GENERAL_WORK())).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctLeakagePartProcMain);
    }

    public static void gmBoss5EfctLeakagePartProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        obj_work.pos.z += 327680;
        if (((int)parentObj.flag & 2048) != 0)
            return;
        ObjDrawKillAction3DES(obj_work);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctLeakageProcFade);
    }

    public static void gmBoss5EfctLeakageProcFade(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreatePrelimLeakage(GMS_BOSS5_BODY_WORK body_work)
    {
        GMM_BS_OBJ((GMS_BOSS5_EFCT_GENERAL_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 7, () => new GMS_BOSS5_EFCT_GENERAL_WORK())).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctPrelimLeakageProcLoop);
    }

    public static void gmBoss5EfctPrelimLeakageProcLoop(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        obj_work.pos.z += 327680;
        if (((int)parentObj.flag & 1048576) != 0)
            return;
        ObjDrawKillAction3DES(obj_work);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctPrelimLeakageProcFade);
    }

    public static void gmBoss5EfctPrelimLeakageProcFade(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctBerserkStampSmokeProcWaitStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (s5EfctGeneralWork.timer != 0U)
        {
            --s5EfctGeneralWork.timer;
        }
        else
        {
            GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
            NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(parentObj.snm_work, s5EfctGeneralWork.ref_node_snm_id);
            obj_work.pos.x = FX_F32_TO_FX32(snmMtx.M03);
            obj_work.pos.y = parentObj.ground_v_pos;
            obj_work.pos.z = FX_F32_TO_FX32(snmMtx.M23);
            obj_work.pos.z = 65536;
            obj_work.disp_flag &= 4294963167U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEnd);
        }
    }

    public static void gmBoss5EfctCreateJet(GMS_BOSS5_BODY_WORK body_work)
    {
        int num = ((int)GMM_BS_OBJ(body_work).disp_flag & 1) == 0 ? body_work.nozzle_snm_reg_ids[1] : body_work.nozzle_snm_reg_ids[0];
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 12, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_EFFECT_COM_WORK gmsEffectComWork = (GMS_EFFECT_COM_WORK)s5EfctGeneralWork;
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(s5EfctGeneralWork);
        obsObjectWork.obj_3des.ecb.drawObjState = 0U;
        s5EfctGeneralWork.ref_node_snm_id = num;
        GmBsCmnSetEfctAtkVsPly((GMS_EFFECT_COM_WORK)obsObjectWork, 16);
        obsObjectWork.flag |= 16U;
        ObjRectWorkSet(gmsEffectComWork.rect_work[1], -8, 0, 8, 88);
        gmsEffectComWork.rect_work[1].flag |= 2048U;
        s5EfctGeneralWork.timer = 65U;
        GmBoss5Init1ShotTimer(s5EfctGeneralWork.se_timer, 50U);
        GMM_BS_OBJ(s5EfctGeneralWork).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctJetProcMain);
    }

    public static void gmBoss5EfctJetProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (GmBoss5Update1ShotTimer(s5EfctGeneralWork.se_timer) != 0)
            GmSoundPlaySE("FinalBoss04");
        if (s5EfctGeneralWork.timer != 0U)
            --s5EfctGeneralWork.timer;
        else
            ((GMS_EFFECT_COM_WORK)obj_work).rect_work[1].flag &= 4294965247U;
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, s5EfctGeneralWork.ref_node_snm_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        if (((int)parentObj.flag & 16384) == 0 && ((int)s5EfctGeneralWork.flag & 1) == 0)
        {
            s5EfctGeneralWork.flag |= 1U;
            ObjDrawKillAction3DES(obj_work);
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateJetSmoke(GMS_BOSS5_BODY_WORK body_work)
    {
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 13, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_EFFECT_COM_WORK gmsEffectComWork = (GMS_EFFECT_COM_WORK)s5EfctGeneralWork;
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(s5EfctGeneralWork);
        int snm_reg_id = ((int)GMM_BS_OBJ(body_work).disp_flag & 1) == 0 ? body_work.nozzle_snm_reg_ids[1] : body_work.nozzle_snm_reg_ids[0];
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(body_work.snm_work, snm_reg_id);
        obsObjectWork.pos.x = FX_F32_TO_FX32(snmMtx.M03);
        obsObjectWork.pos.y = body_work.ground_v_pos;
        obsObjectWork.pos.z = GMM_BS_OBJ(body_work).pos.z;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctJetSmokeProcMain);
    }

    public static void gmBoss5EfctJetSmokeProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (((int)parentObj.flag & 32768) == 0 && ((int)s5EfctGeneralWork.flag & 1) == 0)
        {
            s5EfctGeneralWork.flag |= 1U;
            ObjDrawKillAction3DES(obj_work);
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateRocketLeakage(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(rkt_work), 14, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        nnMakeTranslateMatrix(s5EfctGeneralWork.ofst_mtx, -7f, 0.0f, 0.0f);
        s5EfctGeneralWork.se_handle = GsSoundAllocSeHandle();
        mtTaskChangeTcbDestructor(((OBS_OBJECT_WORK)s5EfctGeneralWork).tcb, new GSF_TASK_PROCEDURE(gmBoss5EfctRocketLeakageExit));
        GmSoundPlaySE("FinalBoss11", s5EfctGeneralWork.se_handle);
        GMM_BS_OBJ(s5EfctGeneralWork).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctRocketLeakageProcMain);
    }

    public static void gmBoss5EfctRocketLeakageExit(MTS_TASK_TCB tcb)
    {
        GsSoundFreeSeHandle(((GMS_BOSS5_EFCT_GENERAL_WORK)mtTaskGetTcbWork(tcb)).se_handle);
        GmEffectDefaultExit(tcb);
    }

    public static void gmBoss5EfctRocketLeakageProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_ROCKET_WORK parentObj = (GMS_BOSS5_ROCKET_WORK)obj_work.parent_obj;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.drill_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos, s5EfctGeneralWork.ofst_mtx);
        obj_work.pos.z += 262144;
        if (((int)parentObj.flag & 4) == 0 && ((int)s5EfctGeneralWork.flag & 1) == 0)
        {
            s5EfctGeneralWork.flag |= 1U;
            ObjDrawKillAction3DES(obj_work);
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        GsSoundStopSeHandle(s5EfctGeneralWork.se_handle);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctRocketDockProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, s5EfctGeneralWork.ref_node_snm_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos, s5EfctGeneralWork.ofst_mtx);
        obj_work.pos.z += 65536;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateRocketJet(
      GMS_BOSS5_ROCKET_WORK rkt_work,
      int is_rev_jet)
    {
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)gmBoss5EfctEsCreate(GMM_BS_OBJ(rkt_work), 16, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        if (is_rev_jet != 0)
        {
            nnMakeTranslateMatrix(s5EfctGeneralWork.ofst_mtx, 4f, 0.0f, 0.0f);
            nnRotateYMatrix(s5EfctGeneralWork.ofst_mtx, s5EfctGeneralWork.ofst_mtx, AKM_DEGtoA32(180));
            s5EfctGeneralWork.user_flag |= 1U;
        }
        else
            nnMakeTranslateMatrix(s5EfctGeneralWork.ofst_mtx, -6f, 0.0f, 0.0f);
        GMM_BS_OBJ(s5EfctGeneralWork).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctRocketJetProcMain);
    }

    public static void gmBoss5EfctRocketJetProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_ROCKET_WORK parentObj = (GMS_BOSS5_ROCKET_WORK)obj_work.parent_obj;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        int num = 0;
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.drill_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos, s5EfctGeneralWork.ofst_mtx);
        obj_work.pos.z += 32768;
        if (((int)s5EfctGeneralWork.user_flag & 1) != 0)
        {
            if (((int)parentObj.flag & 32) == 0)
                num = 1;
        }
        else if (((int)parentObj.flag & 16) == 0)
            num = 1;
        if (num != 0 && ((int)s5EfctGeneralWork.flag & 1) == 0)
        {
            s5EfctGeneralWork.flag |= 1U;
            ObjDrawKillAction3DES(obj_work);
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctLandingShockwaveProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (s5EfctGeneralWork.timer != 0U)
        {
            --s5EfctGeneralWork.timer;
        }
        else
        {
            GMS_EFFECT_COM_WORK gmsEffectComWork = (GMS_EFFECT_COM_WORK)obj_work;
            gmsEffectComWork.rect_work[1].flag |= 2048U;
            gmsEffectComWork.rect_work[1].flag &= 4294967291U;
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctStrikeShockwaveProcWaitStart(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (s5EfctGeneralWork.timer != 0U)
        {
            --s5EfctGeneralWork.timer;
        }
        else
        {
            GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
            NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(parentObj.snm_work, parentObj.armpt_snm_reg_ids[1][2]);
            obj_work.pos.x = FX_F32_TO_FX32(snmMtx.M03);
            obj_work.pos.y = parentObj.ground_v_pos;
            obj_work.pos.z = FX_F32_TO_FX32(snmMtx.M23);
            obj_work.pos.z += 262144;
            obj_work.flag &= 4294967293U;
            obj_work.disp_flag &= 4294963167U;
            s5EfctGeneralWork.timer = 15U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctStrikeShockwaveProcLoop);
        }
    }

    public static void gmBoss5EfctStrikeShockwaveProcLoop(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (s5EfctGeneralWork.timer != 0U)
        {
            --s5EfctGeneralWork.timer;
        }
        else
        {
            GMS_EFFECT_COM_WORK gmsEffectComWork = (GMS_EFFECT_COM_WORK)obj_work;
            gmsEffectComWork.rect_work[1].flag |= 2048U;
            gmsEffectComWork.rect_work[1].flag &= 4294967291U;
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateTargetCursorStart(GMS_BOSS5_BODY_WORK body_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 23, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_EFFECT_COM_WORK parent_efct = (GMS_EFFECT_COM_WORK)efct_3des;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)parent_efct;
        GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor = (GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.pos.Assign(GmBsCmnGetPlayerObj().pos);
        targ_cursor.timer = 120U;
        obsObjectWork.obj_3des.speed = 0.5f;
        gmBoss5EfctTargetCursorInitFlickerNoDisp(targ_cursor);
        gmBoss5EfctCreateTargetCursorFlash(parent_efct, 26);
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctTargetCursorStartProcMain);
    }

    public static void gmBoss5EfctTargetCursorStartProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        GmBoss5BodyGetPlySearchPos(parentObj, out obj_work.pos);
        float num = MTM_MATH_CLIP((float)(1.0 - targ_cursor.timer / 120.0), 0.0f, 1f);
        obj_work.obj_3des.speed = (float)(0.5 + num * 0.5);
        float nodisp_time = 10f / obj_work.obj_3des.speed;
        float cycle_time = 20f / obj_work.obj_3des.speed;
        gmBoss5EfctTargetCursorUpdateFlickerNoDisp(targ_cursor, nodisp_time, cycle_time);
        if (targ_cursor.timer != 0U)
        {
            --targ_cursor.timer;
        }
        else
        {
            gmBoss5EfctCreateTargetCursorLoop(parentObj, targ_cursor.efct_3des.efct_com);
            obj_work.flag |= 4U;
        }
    }

    public static void gmBoss5EfctCreateTargetCursorLoop(
      GMS_BOSS5_BODY_WORK body_work,
      GMS_EFFECT_COM_WORK former_efct)
    {
        GMS_EFFECT_3DES_WORK efct_3des = gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 21, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_EFFECT_COM_WORK parent_efct = (GMS_EFFECT_COM_WORK)efct_3des;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)parent_efct;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.pos.Assign(((OBS_OBJECT_WORK)former_efct).pos);
        obsObjectWork.obj_3des.speed = 0.8f;
        gmBoss5EfctCreateTargetCursorFlash(parent_efct, 24);
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctTargetCursorLoopProcMain);
    }

    public static void gmBoss5EfctTargetCursorLoopProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        GmBoss5BodyGetPlySearchPos(parentObj, out obj_work.pos);
        obj_work.obj_3des.speed += 0.005f;
        obj_work.obj_3des.speed = MTM_MATH_CLIP(obj_work.obj_3des.speed, 0.0f, 1.5f);
        float nodisp_time = 10f / obj_work.obj_3des.speed;
        float cycle_time = 20f / obj_work.obj_3des.speed;
        gmBoss5EfctTargetCursorUpdateFlickerNoDisp(targ_cursor, nodisp_time, cycle_time);
        if (((int)parentObj.flag & 2) != 0)
            return;
        gmBoss5EfctCreateTargetCursorEnd(parentObj, (GMS_EFFECT_COM_WORK)obj_work);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateTargetCursorEnd(
      GMS_BOSS5_BODY_WORK body_work,
      GMS_EFFECT_COM_WORK former_efct)
    {
        GMS_EFFECT_3DES_WORK efct_3des = gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 22, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_EFFECT_COM_WORK parent_efct = (GMS_EFFECT_COM_WORK)efct_3des;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)parent_efct;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.pos.Assign(((OBS_OBJECT_WORK)former_efct).pos);
        gmBoss5EfctCreateTargetCursorFlash(parent_efct, 25);
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctTargetCursorEndProcMain);
    }

    public static void gmBoss5EfctTargetCursorEndProcMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateTargetCursorFlash(
      GMS_EFFECT_COM_WORK parent_efct,
      int efct_idx)
    {
        GMS_EFFECT_3DES_WORK efct_3des = gmBoss5EfctEsCreate((OBS_OBJECT_WORK)parent_efct, efct_idx);
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)efct_3des;
        obsObjectWork.disp_flag |= 32U;
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.obj_3des.speed = ((OBS_OBJECT_WORK)parent_efct).obj_3des.speed;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctTargetCursorFlashProcMain);
    }

    public static void gmBoss5EfctTargetCursorFlashProcMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3des.speed = obj_work.parent_obj.obj_3des.speed;
        if (((int)obj_work.parent_obj.disp_flag & 32) != 0)
            obj_work.disp_flag &= 4294967263U;
        else
            obj_work.disp_flag |= 32U;
    }

    public static void gmBoss5EfctTargetCursorInitFlickerNoDisp(
      GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor)
    {
        targ_cursor.ratio_timer = 0.0f;
    }

    public static void gmBoss5EfctTargetCursorUpdateFlickerNoDisp(
      GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor,
      float nodisp_time,
      float cycle_time)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)targ_cursor;
        ++targ_cursor.ratio_timer;
        if (targ_cursor.ratio_timer >= (double)cycle_time)
            targ_cursor.ratio_timer = 0.0f;
        if (targ_cursor.ratio_timer >= (double)nodisp_time)
            obsObjectWork.disp_flag &= 4294967263U;
        else
            obsObjectWork.disp_flag |= 32U;
    }

    public static void gmBoss5EfctCreateCrashCursorStart(
      GMS_BOSS5_BODY_WORK body_work,
      int pos_x,
      uint duration_time)
    {
        GMS_EFFECT_3DES_WORK efct_3des = gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 23, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_EFFECT_COM_WORK parent_efct = (GMS_EFFECT_COM_WORK)efct_3des;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)parent_efct;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        s5EfctGeneralWork.user_work = (uint)pos_x;
        s5EfctGeneralWork.timer = duration_time;
        obsObjectWork.user_work = duration_time;
        s5EfctGeneralWork.user_flag = (mtMathRand() & 1) == 0 ? 0U : 1U;
        gmBoss5EfctCrashCursorStartSetCurPos(s5EfctGeneralWork, 0.0f, (int)s5EfctGeneralWork.user_flag);
        obsObjectWork.pos.y = body_work.ground_v_pos - 131072;
        obsObjectWork.pos.z = GmBsCmnGetPlayerObj().pos.z;
        gmBoss5EfctTargetCursorInitFlickerNoDisp(s5EfctGeneralWork);
        gmBoss5EfctCreateTargetCursorFlash(parent_efct, 26);
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctCrashCursorStartProcMain);
    }

    public static void gmBoss5EfctCrashCursorStartProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        gmBoss5EfctTargetCursorUpdateFlickerNoDisp(s5EfctGeneralWork, 10f, 20f);
        if (s5EfctGeneralWork.timer != 0U)
        {
            --s5EfctGeneralWork.timer;
            float ratio = MTM_MATH_CLIP((float)((1.0 - nnSin(nnArcCos((1.0 - s5EfctGeneralWork.timer / (double)obj_work.user_work) * 0.899999976158142))) / (1.0 - nnSin(nnArcCos(0.899999976158142)))), 0.0f, 1f);
            gmBoss5EfctCrashCursorStartSetCurPos(s5EfctGeneralWork, ratio, (int)s5EfctGeneralWork.user_flag);
        }
        else
        {
            gmBoss5EfctCrashCursorStartSetCurPos(s5EfctGeneralWork, 1f, (int)s5EfctGeneralWork.user_flag);
            gmBoss5EfctCreateCrashCursorLoop(parentObj, obj_work.pos.x);
            obj_work.flag |= 4U;
        }
    }

    public static void gmBoss5EfctCrashCursorStartSetCurPos(
      GMS_BOSS5_EFCT_GENERAL_WORK ctarg_start,
      float ratio,
      int app_dir_left)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)ctarg_start;
        int low = GMM_BOSS5_AREA_CENTER_X() - 1048576;
        int high = GMM_BOSS5_AREA_CENTER_X() + 1048576;
        int num1 = 2097152 - (int)(4096.0 * fmod(FX_FX32_TO_F32(13631488), FX_FX32_TO_F32(2097152)));
        int num2 = MTM_MATH_CLIP((int)ctarg_start.user_work, low, high);
        int num3 = app_dir_left == 0 ? num1 + 2097152 + (num2 - low) : num1 + (high - num2);
        int num4 = (int)(13631488.0 * ratio) + num3;
        int num5 = (int)(4096.0 * fmod(FX_FX32_TO_F32(num4), FX_FX32_TO_F32(2097152)));
        if ((FX_Div(num4, 2097152) >> 12 & 1) != 0)
            obsObjectWork.pos.x = high - num5;
        else
            obsObjectWork.pos.x = low + num5;
    }

    public static void gmBoss5EfctCreateCrashCursorLoop(
      GMS_BOSS5_BODY_WORK body_work,
      int pos_x)
    {
        GMS_EFFECT_3DES_WORK efct_3des = gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 21, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        GMS_EFFECT_COM_WORK parent_efct = (GMS_EFFECT_COM_WORK)efct_3des;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)parent_efct;
        GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor = (GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.pos.x = pos_x;
        obsObjectWork.pos.y = body_work.ground_v_pos - 131072;
        obsObjectWork.pos.z = GmBsCmnGetPlayerObj().pos.z;
        gmBoss5EfctTargetCursorInitFlickerNoDisp(targ_cursor);
        gmBoss5EfctCreateTargetCursorFlash(parent_efct, 24);
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctCrashCursorLoopProcMain);
    }

    public static void gmBoss5EfctCrashCursorLoopProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        gmBoss5EfctTargetCursorUpdateFlickerNoDisp((GMS_BOSS5_EFCT_GENERAL_WORK)obj_work, 10f, 20f);
        if (((int)parentObj.flag & 2) != 0)
            return;
        gmBoss5EfctCreateCrashCursorEnd(parentObj, (GMS_EFFECT_COM_WORK)obj_work);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateCrashCursorEnd(
      GMS_BOSS5_BODY_WORK body_work,
      GMS_EFFECT_COM_WORK former_efct)
    {
        GMS_EFFECT_3DES_WORK efct_3des = gmBoss5EfctEsCreate(GMM_BS_OBJ(body_work), 22, () => new GMS_BOSS5_EFCT_GENERAL_WORK());
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)(GMS_EFFECT_COM_WORK)efct_3des;
        GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.pos.Assign(((OBS_OBJECT_WORK)former_efct).pos);
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctCrashCursorEndProcMain);
    }

    public static void gmBoss5EfctCrashCursorEndProcMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctVulcanBulletProcMain(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.pos.x <= GMM_BOSS5_AREA_LEFT() || obj_work.pos.y <= GMM_BOSS5_AREA_TOP() || (obj_work.pos.x >= GMM_BOSS5_AREA_RIGHT() || obj_work.pos.y >= GMM_BOSS5_AREA_BOTTOM()))
            obj_work.flag &= 4294967279U;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateRocketSmoke(GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(rkt_work), 3U);
        GmEffect3DESSetDispOffset(efct_3des, 0.0f, 0.0f, 0.0f);
        GmEffect3DESSetDispRotation(efct_3des, (short)AKM_DEGtoA32(0), 0, 0);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctRocketSmokeProcLoop);
    }

    public static void gmBoss5EfctRocketSmokeProcLoop(OBS_OBJECT_WORK obj_work)
    {
        if (((int)((GMS_BOSS5_ROCKET_WORK)obj_work.parent_obj).flag & 64) != 0)
            return;
        ObjDrawKillAction3DES(obj_work);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctRocketSmokeProcFade);
    }

    public static void gmBoss5EfctRocketSmokeProcFade(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctBreakdownSmokeProcLoop(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctBreakdownSmokeProcFade);
        }
    }

    public static void gmBoss5EfctBreakdownSmokeProcFade(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateBodySmallSmoke(
      GMS_BOSS5_BODY_WORK body_work,
      uint part_idx)
    {
    }

    public static void gmBoss5EfctBodySmallSmokeProcLoop(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.user_timer = AkMathRandFx() * 60 + 60 >> 12;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctBodySmallSmokeProcFade);
        }
    }

    public static void gmBoss5EfctBodySmallSmokeProcFade(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            gmBoss5EfctCreateBodySmallSmoke(parentObj, obj_work.user_work);
            obj_work.flag |= 4U;
        }
    }

    public static void gmBoss5EfctCreateBerserkSteam(
      GMS_BOSS5_BODY_WORK body_work,
      uint count,
      uint part_idx)
    {
        if (count == 0U)
            return;
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctCmnEsCreate(GMM_BS_OBJ(body_work), 89);
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)efct_3des;
        obsObjectWork.user_flag = part_idx;
        obsObjectWork.user_work = count - 1U;
        GmEffect3DESSetDispOffset(efct_3des, gm_boss5_efct_berserk_steam_disp_ofst_tbl[(int)part_idx][0], gm_boss5_efct_berserk_steam_disp_ofst_tbl[(int)part_idx][1], gm_boss5_efct_berserk_steam_disp_ofst_tbl[(int)part_idx][2]);
        GmEffect3DESSetDispRotation(efct_3des, gm_boss5_efct_berserk_steam_disp_rot_tbl[(int)part_idx][0], gm_boss5_efct_berserk_steam_disp_rot_tbl[(int)part_idx][1], gm_boss5_efct_berserk_steam_disp_rot_tbl[(int)part_idx][2]);
        obsObjectWork.user_timer = 6;
        obsObjectWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctBerserkSteamProcLoop);
    }

    public static void gmBoss5EfctBerserkSteamProcLoop(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.user_timer = 0;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctBerserkSteamProcFade);
        }
    }

    public static void gmBoss5EfctBerserkSteamProcFade(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            if (obj_work.user_work != 0U)
                gmBoss5EfctCreateBerserkSteam(parentObj, obj_work.user_work, obj_work.user_flag);
            obj_work.flag |= 4U;
        }
    }

    public static void gmBoss5EfctCreateEggSweat(GMS_BOSS5_EGG_WORK egg_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctCmnEsCreate(GMM_BS_OBJ(egg_work), 93);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 56f, -8f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EfctEggSweatProcLoop);
    }

    public static void gmBoss5EfctEggSweatProcLoop(OBS_OBJECT_WORK obj_work)
    {
        if (((int)((GMS_BOSS5_EGG_WORK)obj_work.parent_obj).flag & (int)GMD_BOSS5_EGG_FLAG_SWEAT_ACTIVE) != 0)
            return;
        ObjDrawKillAction3DES(obj_work);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEnd);
    }

}