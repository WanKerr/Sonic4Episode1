public partial class AppMain
{
    private static GMS_EFFECT_3DES_WORK gmBoss1EffShockwaveInit(
      GMS_BOSS1_CHAIN_WORK chain_work)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_BOSS1_EFF_SHOCKWAVE_WORK(), GMM_BS_OBJ(chain_work), 0, "B01_ShockWave");
        GMS_EFFECT_3DES_WORK efct_3des = (GMS_EFFECT_3DES_WORK)work;
        GMS_BOSS1_EFF_SHOCKWAVE_WORK sw_work = (GMS_BOSS1_EFF_SHOCKWAVE_WORK)efct_3des;
        sw_work.mgr_work = chain_work.mgr_work;
        gmBoss1MgrIncObjCreateCount(sw_work.mgr_work);
        int index = GmBsCmnIsFinalZoneType(GMM_BS_OBJ(chain_work.mgr_work)) == 0 ? 706 : 708;
        ObjObjectAction3dESEffectLoad(GMM_BS_OBJ(efct_3des), efct_3des.obj_3des, ObjDataGet(index), null, 0, null);
        ObjObjectAction3dESTextureLoad(GMM_BS_OBJ(efct_3des), efct_3des.obj_3des, ObjDataGet(709), null, 0, null, false);
        ObjObjectAction3dESTextureSetByDwork(work, ObjDataGet(710));
        GmEffect3DESSetupBase(efct_3des, 1U, 1U);
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(chain_work.snm_work, chain_work.ball_snm_reg_id);
        VEC_Set(ref work.pos, FX_F32_TO_FX32(snmMtx.M03), GMD_BOSS1_GROUND_POS_Y, 0);
        work.flag &= 4294967293U;
        GmEffectRectInit(efct_3des.efct_com, gm_boss1_eff_sw_atk_flag_tbl, gm_boss1_eff_sw_def_flag_tbl, 1, 1);
        ObjRectWorkSet(efct_3des.efct_com.rect_work[1], -64, -32, 64, 32);
        sw_work.atk_rect_timer = 10U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1EffShockwaveProcMain);
        gmBoss1EffShockwaveSubpartInit(sw_work, 163840, true);
        gmBoss1EffShockwaveSubpartInit(sw_work, 163840, false);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss1EffShockwaveExit));
        return efct_3des;
    }

    private static void gmBoss1EffShockwaveExit(MTS_TASK_TCB tcb)
    {
        gmBoss1MgrDecObjCreateCount(((GMS_BOSS1_EFF_SHOCKWAVE_WORK)mtTaskGetTcbWork(tcb)).mgr_work);
        GmEffectDefaultExit(tcb);
    }

    private static void gmBoss1EffShockwaveProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_EFF_SHOCKWAVE_WORK effShockwaveWork = (GMS_BOSS1_EFF_SHOCKWAVE_WORK)obj_work;
        if (effShockwaveWork.atk_rect_timer != 0U)
            --effShockwaveWork.atk_rect_timer;
        else
            obj_work.flag |= 2U;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static GMS_EFFECT_3DES_WORK gmBoss1EffShockwaveSubpartInit(
      GMS_BOSS1_EFF_SHOCKWAVE_WORK sw_work,
      int ofst_h,
      bool is_left)
    {
        MTM_ASSERT(ofst_h >= 0);
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK(), GMM_BS_OBJ(sw_work).parent_obj, 0, "B01_SW_Subpart");
        GMS_EFFECT_3DES_WORK efct_3des = (GMS_EFFECT_3DES_WORK)work;
        GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK shockwaveSubWork = (GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK)efct_3des;
        shockwaveSubWork.mgr_work = sw_work.mgr_work;
        gmBoss1MgrIncObjCreateCount(shockwaveSubWork.mgr_work);
        ObjObjectAction3dESEffectLoad(GMM_BS_OBJ(efct_3des), efct_3des.obj_3des, ObjDataGet(707), null, 0, null);
        ObjObjectAction3dESTextureLoad(GMM_BS_OBJ(efct_3des), efct_3des.obj_3des, ObjDataGet(709), null, 0, null, false);
        ObjObjectAction3dESTextureSetByDwork(work, ObjDataGet(710));
        GmEffect3DESSetupBase(efct_3des, 1U, 0U);
        GmEffect3DESSetDispRotation(efct_3des, (short)GMD_BOSS1_EFF_SHOCKWAVE_SUB_ROT_X, 0, 0);
        GmEffect3DESSetDispOffset(efct_3des, 0.0f, -FX_FX32_TO_F32(-16384), FX_FX32_TO_F32(-ofst_h));
        work.pos.x = GMM_BS_OBJ(sw_work).pos.x;
        work.pos.y = GMM_BS_OBJ(sw_work).pos.y;
        work.pos.z = GMM_BS_OBJ(sw_work).pos.z;
        if (is_left)
            work.disp_flag &= 4294967294U;
        else
            work.disp_flag |= 1U;
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss1EffShockwaveSubExit));
        return efct_3des;
    }

    private static void gmBoss1EffShockwaveSubExit(MTS_TASK_TCB tcb)
    {
        gmBoss1MgrDecObjCreateCount(((GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK)mtTaskGetTcbWork(tcb)).mgr_work);
        GmEffectDefaultExit(tcb);
    }

    private static void gmBoss1EffScatterInit(GMS_BOSS1_CHAIN_WORK chain_work)
    {
        for (int index = 0; index < 9; ++index)
        {
            GMS_BS_CMN_NODE_CTRL_OBJECT controlObjectBySize = GmBsCmnCreateNodeControlObjectBySize(GMM_BS_OBJ(chain_work), chain_work.cnm_mgr_work, chain_work.sct_cnm_reg_ids[index], chain_work.snm_work, chain_work.sct_snm_reg_ids[index], () => new GMS_BOSS1_EFF_SCT_PART_NDC_WORK());
            GMS_BOSS1_EFF_SCT_PART_NDC_WORK sct_part_ndc = (GMS_BOSS1_EFF_SCT_PART_NDC_WORK)controlObjectBySize;
            controlObjectBySize.user_timer = mtMathRand() % 20U;
            controlObjectBySize.is_enable = 0;
            gmBoss1EffScatterSetPartParam(sct_part_ndc, index == 8);
            GMM_BS_OBJ(controlObjectBySize).move_flag |= 4608U;
            nnMakeUnitQuaternion(ref controlObjectBySize.user_quat);
            controlObjectBySize.proc_update = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1EffScatterProcWait);
        }
    }

    private static void gmBoss1EffScatterSetPartParam(
      GMS_BOSS1_EFF_SCT_PART_NDC_WORK sct_part_ndc,
      bool is_ironball)
    {
        GMS_BS_CMN_NODE_CTRL_OBJECT cmnNodeCtrlObject = (GMS_BS_CMN_NODE_CTRL_OBJECT)sct_part_ndc;
        int ang;
        if (is_ironball)
        {
            sct_part_ndc.is_ironball = true;
            cmnNodeCtrlObject.user_ofst.y = 32f;
            ang = GMD_BOSS1_EFF_SCT_PART_IBALL_SPIN_SPD_DEG;
        }
        else
        {
            sct_part_ndc.is_ironball = false;
            cmnNodeCtrlObject.user_ofst.y = 8f;
            ang = GMD_BOSS1_EFF_SCT_PART_RING_SPIN_SPD_DEG;
        }
        nnMakeUnitQuaternion(ref sct_part_ndc.spin_quat);
        for (int index = 0; index < 2; ++index)
        {
            NNS_VECTOR dst_vec = GlobalPool<NNS_VECTOR>.Alloc();
            NNS_QUATERNION dst = new NNS_QUATERNION();
            float rand_z = MTM_MATH_CLIP((float)(FX_FX32_TO_F32(AkMathRandFx()) * 2.0 - 1.0), -1f, 1f);
            short rand_angle = AKM_DEGtoA16(360f * FX_FX32_TO_F32(AkMathRandFx()));
            AkMathGetRandomUnitVector(dst_vec, rand_z, rand_angle);
            nnMakeRotateAxisQuaternion(out dst, dst_vec.x, dst_vec.y, dst_vec.z, ang);
            nnMultiplyQuaternion(ref sct_part_ndc.spin_quat, ref dst, ref sct_part_ndc.spin_quat);
            GlobalPool<NNS_VECTOR>.Release(dst_vec);
        }
        if (is_ironball)
            ObjObjectFieldRectSet(GMM_BS_OBJ(cmnNodeCtrlObject), -24, -24, 24, 24);
        else
            ObjObjectFieldRectSet(GMM_BS_OBJ(cmnNodeCtrlObject), -8, -8, 8, 8);
    }

    private static void gmBoss1EffScatterSetFlyParam(
      GMS_BOSS1_EFF_SCT_PART_NDC_WORK sct_part_ndc)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(sct_part_ndc);
        int num1 = mtMathRand() % 180;
        int ang = AKM_DEGtoA32(num1 + (num1 > 90 ? 45 : -45));
        float num2 = !sct_part_ndc.is_ironball ? 5f : 3f;
        obsObjectWork.spd.y = (int)(4096.0 * num2 * nnSin(ang));
        obsObjectWork.spd.x = (int)(4096.0 * num2 * nnCos(ang));
        obsObjectWork.move_flag |= 128U;
    }

    private static void gmBoss1EffScatterProcWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj = (GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work;
        GMS_BOSS1_EFF_SCT_PART_NDC_WORK sct_part_ndc = (GMS_BOSS1_EFF_SCT_PART_NDC_WORK)ndc_obj;
        if (ndc_obj.user_timer != 0U)
        {
            --ndc_obj.user_timer;
        }
        else
        {
            GmBsCmnAttachNCObjectToSNMNode(ndc_obj);
            gmBoss1EffScatterSetFlyParam(sct_part_ndc);
            ndc_obj.is_enable = 1;
            ndc_obj.user_timer = 180U;
            ndc_obj.proc_update = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1EffScatterProcFly);
        }
    }

    private static void gmBoss1EffScatterProcFly(OBS_OBJECT_WORK obj_work)
    {
        GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj = (GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work;
        GMS_BOSS1_EFF_SCT_PART_NDC_WORK effSctPartNdcWork = (GMS_BOSS1_EFF_SCT_PART_NDC_WORK)ndc_obj;
        nnMultiplyQuaternion(ref ndc_obj.user_quat, ref effSctPartNdcWork.spin_quat, ref ndc_obj.user_quat);
        GmBsCmnSetWorldMtxFromNCObjectPosture(ndc_obj);
        if (ndc_obj.user_timer != 0U)
            --ndc_obj.user_timer;
        else
            obj_work.flag |= 4U;
    }

    private static void gmBoss1EffBombInitCreate(
      GMS_BOSS1_EFF_BOMB_WORK bomb_work,
      int bomb_type,
      OBS_OBJECT_WORK parent_obj,
      int pos_x,
      int pos_y,
      int width,
      int height,
      uint interval_min,
      uint interval_max)
    {
        MTM_ASSERT(bomb_work);
        MTM_ASSERT(parent_obj);
        bomb_work.parent_obj = parent_obj;
        bomb_work.bomb_type = bomb_type;
        bomb_work.interval_timer = 0U;
        bomb_work.interval_min = interval_min;
        bomb_work.interval_max = interval_max;
        bomb_work.pos[0] = pos_x;
        bomb_work.pos[1] = pos_y;
        bomb_work.area[0] = width;
        bomb_work.area[1] = height;
        bomb_work.interval_timer_sound = 0;
    }

    private static void gmBoss1EffBombUpdateCreate(GMS_BOSS1_EFF_BOMB_WORK bomb_work)
    {
        MTM_ASSERT(bomb_work.parent_obj);
        if (bomb_work.interval_timer != 0U)
        {
            --bomb_work.interval_timer;
        }
        else
        {
            int v2_1 = bomb_work.area[0];
            int v2_2 = bomb_work.area[1];
            int num1 = FX_Mul(AkMathRandFx(), v2_1);
            int num2 = FX_Mul(AkMathRandFx(), v2_2);
            if (bomb_work.bomb_type == 0)
            {
                GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(null, 7);
                if (--bomb_work.interval_timer_sound <= 0)
                {
                    bomb_work.interval_timer_sound = 3;
                    GmSoundPlaySE("Boss0_02");
                }
                OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(gmsEffect3DesWork);
                MTM_ASSERT(obsObjectWork);
                obsObjectWork.pos.x = bomb_work.pos[0] - (v2_1 >> 1) + num1;
                obsObjectWork.pos.y = bomb_work.pos[1] - (v2_2 >> 1) + num2;
                obsObjectWork.pos.z = GMM_BS_OBJ(bomb_work.parent_obj).pos.z + 131072;
                uint num3 = (uint)(AkMathRandFx() * (bomb_work.interval_max - bomb_work.interval_min) >> 12);
                bomb_work.interval_timer = bomb_work.interval_min + num3;
            }
            else
                MTM_ASSERT(false);
        }
    }

    private static void gmBoss1EffDamageInit(GMS_BOSS1_BODY_WORK body_work)
    {
        GMM_BS_OBJ(GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 0U)).pos.z += 131072;
    }

    private static void gmBoss1EffAfterburnerSetEnable(
      GMS_BOSS1_BODY_WORK body_work,
      bool is_enable)
    {
        if (is_enable)
        {
            MTM_ASSERT(0 == ((int)body_work.flag & 16));
            MTM_ASSERT(0 == ((int)body_work.flag & 33554432));
            body_work.flag |= 33554432U;
        }
        else
        {
            body_work.flag &= 4294967279U;
            body_work.flag &= 4261412863U;
        }
    }

    private static void gmBoss1EffAfterburnerUpdateCreate(GMS_BOSS1_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 33554432) == 0)
            return;
        body_work.flag &= 4261412863U;
        body_work.flag |= 16U;
        gmBoss1EffAfterburnerInit(body_work);
    }

    private static void gmBoss1EffAfterburnerInit(GMS_BOSS1_BODY_WORK body_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 4U);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -30f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1EffAfterburnerProcMain);
    }

    private static void gmBoss1EffAfterburnerProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_BODY_WORK parentObj = (GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj.snm_work.reg_node_max);
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 16) == 0)
            ObjDrawKillAction3DES(obj_work);
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1);
    }

    private static void gmBoss1EffABSmokeInit(GMS_BOSS1_BODY_WORK body_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 5U);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1EffABSmokeProcMain);
    }

    private static void gmBoss1EffABSmokeProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_BODY_WORK parentObj = (GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj.snm_work.reg_node_max);
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1);
    }

    private static void gmBoss1EffBodySmokeInit(GMS_BOSS1_BODY_WORK body_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 3U);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1EffBodySmokeProcMain);
    }

    private static void gmBoss1EffBodySmokeProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_BODY_WORK parentObj = (GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj.snm_work.reg_node_max);
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1);
    }

    private static void gmBoss1EffBodySmallSmokeInit(GMS_BOSS1_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK parent_obj = GMM_BS_OBJ(body_work);
        for (int index = 0; index < 3; ++index)
        {
            GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(parent_obj, 2U);
            GmEffect3DESSetDispOffset(efct_3des, gm_boss1_eff_small_smoke_disp_ofst_tbl[index][0], gm_boss1_eff_small_smoke_disp_ofst_tbl[index][1], gm_boss1_eff_small_smoke_disp_ofst_tbl[index][2]);
            GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1EffBodySmallSmokeProcMain);
        }
    }

    private static void gmBoss1EffBodySmallSmokeProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_BODY_WORK parentObj = (GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj.snm_work.reg_node_max);
        obj_work.flag &= 4294966271U;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1);
    }

    private static void gmBoss1EffBodyDebrisInit(GMS_BOSS1_BODY_WORK body_work)
    {
        ((OBS_OBJECT_WORK)GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 1U)).parent_ofst.x = -65536;
    }

    private static void gmBoss1EffSweatInit(GMS_BOSS1_EGG_WORK egg_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctCmnEsCreate(GMM_BS_OBJ(egg_work), 93);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 32f, 0.0f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1EffSweatProcMain);
        egg_work.flag |= 2U;
    }

    private static void gmBoss1EffSweatProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_EGG_WORK parentObj = (GMS_BOSS1_EGG_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj);
        if (((int)parentObj.flag & 2) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

}