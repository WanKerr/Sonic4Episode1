public partial class AppMain
{
    private static void gmBoss2EffDamageInit(GMS_BOSS2_BODY_WORK body_work)
    {
        GMM_BS_OBJ(GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 0U)).pos.z += 131072;
    }

    private static void gmBoss2EffBombsInit(
      GMS_BOSS2_EFF_BOMB_WORK bomb_work,
      OBS_OBJECT_WORK parent_obj,
      int pos_x,
      int pos_y,
      int width,
      int height,
      uint interval_min,
      uint interval_max)
    {
        bomb_work.parent_obj = parent_obj;
        bomb_work.interval_timer = 0U;
        bomb_work.interval_min = interval_min;
        bomb_work.interval_max = interval_max;
        bomb_work.pos[0] = pos_x;
        bomb_work.pos[1] = pos_y;
        bomb_work.area[0] = width;
        bomb_work.area[1] = height;
    }

    private static void gmBoss2EffBombsUpdate(GMS_BOSS2_EFF_BOMB_WORK bomb_work)
    {
        if (bomb_work.interval_timer > 0U)
        {
            --bomb_work.interval_timer;
        }
        else
        {
            GmSoundPlaySE("Boss0_02");
            OBS_OBJECT_WORK obsObjectWork1 = GMM_BS_OBJ(GmEfctCmnEsCreate(null, 7));
            OBS_OBJECT_WORK obsObjectWork2 = GMM_BS_OBJ(bomb_work.parent_obj);
            int v2_1 = bomb_work.area[0];
            int v2_2 = bomb_work.area[1];
            int num1 = FX_Mul(AkMathRandFx(), v2_1);
            int num2 = FX_Mul(AkMathRandFx(), v2_2);
            obsObjectWork1.pos.x = bomb_work.pos[0] - (v2_1 >> 1) + num1;
            obsObjectWork1.pos.y = bomb_work.pos[1] - (v2_2 >> 1) + num2;
            obsObjectWork1.pos.z = obsObjectWork2.pos.z + 131072;
            uint num3 = (uint)(AkMathRandFx() * ((int)bomb_work.interval_max - (int)bomb_work.interval_min) >> 12);
            bomb_work.interval_timer = bomb_work.interval_min + num3;
        }
    }

    private static void gmBoss2EffAfterburnerRequestCreate(GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.flag |= 33554432U;
    }

    private static void gmBoss2EffAfterburnerRequestDelete(GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.flag &= 4294967293U;
        body_work.flag &= 4261412863U;
    }

    private static void gmBoss2EffAfterburnerInit(GMS_BOSS2_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 2) != 0)
            return;
        body_work.flag &= 4261412863U;
        body_work.flag |= 2U;
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 4U);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -30f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffAfterburnerMainFunc);
    }

    private static void gmBoss2EffAfterburnerMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 2) == 0)
            ObjDrawKillAction3DES(obj_work);
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss2EffAfterburnerSmokeInit(GMS_BOSS2_BODY_WORK body_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 5U);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffAfterburnerSmokeMainFunc);
    }

    private static void gmBoss2EffAfterburnerSmokeMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss2EffBodySmokeInit(GMS_BOSS2_BODY_WORK body_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 3U);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffBodySmokeMainFunc);
    }

    private static void gmBoss2EffBodySmokeMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss2EffSweatInit(GMS_BOSS2_EGG_WORK egg_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctCmnEsCreate(GMM_BS_OBJ(egg_work), 93);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 32f, 0.0f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffSweatMainFunc);
        egg_work.flag |= 2U;
    }

    private static void gmBoss2EffSweatMainFunc(OBS_OBJECT_WORK obj_work)
    {
        if (((int)((GMS_BOSS2_EGG_WORK)obj_work.parent_obj).flag & 2) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static OBS_OBJECT_WORK gmBoss2EffInit(
      OBS_DATA_WORK data_work,
      int effect_type,
      OBS_OBJECT_WORK parent_obj_work,
      short rot_x,
      short rot_y,
      short rot_z,
      float offset_x,
      float offset_y,
      float offset_z,
      int flag_flip,
      int flag_data_rotate)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_3DES_WORK(), null, 0, "B02_effect");
        GMS_EFFECT_3DES_WORK efct_3des = (GMS_EFFECT_3DES_WORK)work;
        ObjObjectAction3dESEffectLoad(work, efct_3des.obj_3des, data_work, null, 0, null);
        ObjObjectAction3dESTextureLoad(work, efct_3des.obj_3des, ObjDataGet(722), null, 0, null, false);
        ObjObjectAction3dESTextureSetByDwork(work, ObjDataGet(723));
        uint init_flag = 0;
        if (parent_obj_work != null)
        {
            work.parent_obj = parent_obj_work;
            work.pos.x = parent_obj_work.pos.x;
            work.pos.y = parent_obj_work.pos.y;
            work.pos.z = parent_obj_work.pos.z;
            init_flag |= 18U;
        }
        GmEffect3DESSetDispRotation(efct_3des, rot_x, rot_y, rot_z);
        GmEffect3DESAddDispOffset(efct_3des, offset_x, offset_y, offset_z);
        if (flag_flip == 0)
            init_flag |= 1U;
        if (flag_data_rotate != 0)
            init_flag |= 64U;
        GmEffect3DESSetupBase(efct_3des, (uint)effect_type, init_flag);
        return work;
    }

    private static void gmBoss2EffBallBombInit(
      VecFx32 create_pos,
      OBS_OBJECT_WORK body_obj_work)
    {
        OBS_OBJECT_WORK obj_work_parts = gmBoss2EffInit(ObjDataGet(717), 1, null, 0, 0, 0, 0.0f, 0.0f, 0.0f, 0, 0);
        obj_work_parts.pos.x = create_pos.x;
        obj_work_parts.pos.y = create_pos.y;
        obj_work_parts.pos.z = create_pos.z + 131072;
        gmBoss2MgrAddObject(gmBoss2MgrGetMgrWork(body_obj_work), obj_work_parts);
        mtTaskChangeTcbDestructor(obj_work_parts.tcb, new GSF_TASK_PROCEDURE(gmBoss2EffectExitFunc));
    }

    private static OBS_OBJECT_WORK gmBoss2EffBallBombPartInit(
      VecFx32 create_pos,
      OBS_OBJECT_WORK body_obj_work,
      int spd_x)
    {
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (GMS_EFFECT_3DES_WORK)gmBoss2EffInit(ObjDataGet(718), 2, null, 0, 0, 0, 0.0f, 0.0f, 0.0f, 0, 0);
        OBS_OBJECT_WORK objWork = gmsEffect3DesWork.efct_com.obj_work;
        OBS_RECT_WORK[] rectWork = gmsEffect3DesWork.efct_com.rect_work;
        GmBsCmnSetEfctAtkVsPly(gmsEffect3DesWork.efct_com, 64);
        ObjRectWorkSet(rectWork[1], -8, -8, 8, 8);
        rectWork[1].ppHit = new OBS_RECT_WORK_Delegate1(gmBoss2BallHitFunc);
        rectWork[1].flag |= 1028U;
        rectWork[0].flag |= 3072U;
        objWork.pos.x = create_pos.x;
        objWork.pos.y = create_pos.y;
        objWork.pos.z = create_pos.z;
        objWork.spd.x = spd_x;
        objWork.spd.y = -16384;
        objWork.move_flag |= 32912U;
        objWork.flag |= 16U;
        objWork.parent_obj = body_obj_work;
        objWork.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffBallBombPartMainFunc);
        gmBoss2MgrAddObject(gmBoss2MgrGetMgrWork(body_obj_work), objWork);
        mtTaskChangeTcbDestructor(objWork.tcb, new GSF_TASK_PROCEDURE(gmBoss2EffectExitFunc));
        return objWork;
    }

    private static void gmBoss2EffBallBombPartMainFunc(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (ObjViewOutCheck(obj_work.pos.x, obj_work.pos.y, 64, 0, 0, 0, 0) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmBoss2EffBlitzInit(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK obj_work_parts1 = gmBoss2EffInit(ObjDataGet(714), 2, obsObjectWork, 0, 0, 0, 24f, 0.0f, 0.0f, 0, 0);
        obj_work_parts1.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffBlitzMainFuncBlitzCoreL);
        OBS_OBJECT_WORK obj_work_parts2 = gmBoss2EffInit(ObjDataGet(714), 2, obsObjectWork, 0, 0, 0, -24f, 0.0f, 0.0f, 0, 0);
        obj_work_parts2.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffBlitzMainFuncBlitzCoreR);
        OBS_OBJECT_WORK obj_work_parts3 = gmBoss2EffInit(ObjDataGet(715), 2, obsObjectWork, 0, 0, GMD_BOSS2_EFFECT_BLITZ_LINE_DISP_ROT_Z, 0.0f, -30f, 0.0f, 0, 0);
        obj_work_parts3.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffBlitzMainFuncBlitzLineCreate);
        body_work.flag |= 4U;
        GMS_BOSS2_MGR_WORK mgrWork = gmBoss2MgrGetMgrWork(obsObjectWork);
        gmBoss2MgrAddObject(mgrWork, obj_work_parts1);
        gmBoss2MgrAddObject(mgrWork, obj_work_parts2);
        gmBoss2MgrAddObject(mgrWork, obj_work_parts3);
        mtTaskChangeTcbDestructor(obj_work_parts1.tcb, new GSF_TASK_PROCEDURE(gmBoss2EffectExitFunc));
        mtTaskChangeTcbDestructor(obj_work_parts2.tcb, new GSF_TASK_PROCEDURE(gmBoss2EffectExitFunc));
        mtTaskChangeTcbDestructor(obj_work_parts3.tcb, new GSF_TASK_PROCEDURE(gmBoss2EffectExitFunc));
    }

    private static void gmBoss2EffBlitzMainFuncBlitzLineCreate(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 4) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[2], 1);
        ++obj_work.user_timer;
        if (obj_work.user_timer < 64.0)
            return;
        obj_work.user_timer = 0;
        GmEffect3DESAddDispOffset((GMS_EFFECT_3DES_WORK)obj_work, 0.0f, -4f, 0.0f);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffBlitzMainFuncBlitzLineNormal);
    }

    private static void gmBoss2EffBlitzMainFuncBlitzLineNormal(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 4) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[2], 1);
    }

    private static void gmBoss2EffBlitzMainFuncBlitzCoreL(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 4) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[7], 1);
    }

    private static void gmBoss2EffBlitzMainFuncBlitzCoreR(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)parentObj.flag & 4) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[10], 1);
    }

    private static void gmBoss2EffBlitzMainFuncBlitzL(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)parentObj.flag & 4) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[6], 1);
    }

    private static void gmBoss2EffBlitzMainFuncBlitzR(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 4) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[9], 1);
    }

    private static void gmBoss2EffScatterInit(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK parent_obj = GMM_BS_OBJ(body_work);
        GMS_BOSS2_EFFECT_SCATTER_WORK effectScatterWork = null;
        for (int index = 3; 13 > index; ++index)
        {
            GMS_BOSS2_EFFECT_SCATTER_WORK controlObjectBySize = (GMS_BOSS2_EFFECT_SCATTER_WORK)GmBsCmnCreateNodeControlObjectBySize(parent_obj, body_work.cnm_mgr_work, body_work.cnm_reg_id[index], body_work.snm_work, body_work.snm_reg_id[2 + index], () => new GMS_BOSS2_EFFECT_SCATTER_WORK());
            GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj = (GMS_BS_CMN_NODE_CTRL_OBJECT)controlObjectBySize;
            GmBsCmnChangeCNMModeNode(body_work.cnm_mgr_work, body_work.cnm_reg_id[index], 0U);
            GmBsCmnEnableCNMLocalCoordinate(body_work.cnm_mgr_work, body_work.cnm_reg_id[index], 0);
            GmBsCmnAttachNCObjectToSNMNode(ndc_obj);
            ndc_obj.is_enable = 1;
            ndc_obj.proc_update = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffScatterMainFunc);
            OBS_OBJECT_WORK objWork1 = ndc_obj.efct_com.obj_work;
            objWork1.move_flag |= 128U;
            if (index == 4 || index == 5 || (index == 7 || index == 8))
            {
                OBS_OBJECT_WORK objWork2 = effectScatterWork.control_node_work.efct_com.obj_work;
                objWork1.spd.x = objWork2.spd.x;
                objWork1.spd.y = objWork2.spd.y;
            }
            else
            {
                int right_flag = 0;
                if (index % 2 != 0)
                    right_flag = 1;
                gmBoss2EffScatterSetParamMove(objWork1, right_flag);
            }
            effectScatterWork = controlObjectBySize;
        }
    }

    private static void gmBoss2EffScatterMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GmBsCmnSetWorldMtxFromNCObjectPosture((GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work);
        ++obj_work.user_timer;
        if (obj_work.user_timer < 100)
            return;
        obj_work.user_timer = 0;
        obj_work.flag |= 4U;
    }

    private static void gmBoss2EffScatterSetParamMove(
      OBS_OBJECT_WORK obj_work,
      int right_flag)
    {
        int num = mtMathRand() % 30 + 45;
        if (right_flag != 0)
            num = -num;
        int ang = AKM_DEGtoA32(num + 90);
        obj_work.spd.y = -(int)(4096.0 * nnSin(ang));
        obj_work.spd.x = (int)(4096.0 * nnCos(ang));
    }

    private static void gmBoss2EffCreateRollModel(GMS_BOSS2_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(body_work);
        if (((int)body_work.flag & 32) != 0)
            return;
        body_work.flag |= 32U;
        OBS_OBJECT_WORK byParam = (OBS_OBJECT_WORK)GmEffect3dESCreateByParam(new GMS_EFFECT_CREATE_PARAM(11, 0U, 19U, new NNS_VECTOR(0.0f, 0.0f, 64f), new NNS_ROTATE_A16(0, ((int)obsObjectWork.disp_flag & 1) == 0 ? (short)AKM_DEGtoA32(90f) : (short)AKM_DEGtoA32(-90f), 0), 3.2f, new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEnd), 5), obsObjectWork, GmBoss2GetGameDatEnemyArc(), ObjDataGet(719), ObjDataGet(726), ObjDataGet(727), ObjDataGet(725), ObjDataGet(724), () => new GMS_EFFECT_3DES_WORK());
        byParam.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss2EffRollModelMainFunc);
        byParam.obj_3des.command_state = 16U;
        gmBoss2MgrAddObject(gmBoss2MgrGetMgrWork(obsObjectWork), byParam);
        mtTaskChangeTcbDestructor(byParam.tcb, new GSF_TASK_PROCEDURE(gmBoss2EffectExitFunc));
    }

    private static void gmBoss2EffCreateRollModelLost(GMS_BOSS2_BODY_WORK body_work)
    {
        if (((int)GMM_BS_OBJ(body_work).disp_flag & 1) != 0)
            AKM_DEGtoA32(-90f);
        else
            AKM_DEGtoA32(90f);
    }

    public static void gmBoss2EffRollModelMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        int num = (int)obj_work.disp_flag & 8;
        if (((int)parentObj.flag & 32) != 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmBoss2EffRollMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS2_BODY_WORK parentObj = (GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        int num = (int)obj_work.disp_flag & 8;
        if (((int)parentObj.flag & 32) != 0)
            return;
        obj_work.flag |= 4U;
    }

}