using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using mpp;

public partial class AppMain
{
    private static AppMain.GMS_EFFECT_3DES_WORK gmBoss1EffShockwaveInit(
      AppMain.GMS_BOSS1_CHAIN_WORK chain_work)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK()), AppMain.GMM_BS_OBJ((object)chain_work), (ushort)0, "B01_ShockWave");
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = (AppMain.GMS_EFFECT_3DES_WORK)work;
        AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK sw_work = (AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK)efct_3des;
        sw_work.mgr_work = chain_work.mgr_work;
        AppMain.gmBoss1MgrIncObjCreateCount(sw_work.mgr_work);
        int index = AppMain.GmBsCmnIsFinalZoneType(AppMain.GMM_BS_OBJ((object)chain_work.mgr_work)) == 0 ? 706 : 708;
        AppMain.ObjObjectAction3dESEffectLoad(AppMain.GMM_BS_OBJ((object)efct_3des), efct_3des.obj_3des, AppMain.ObjDataGet(index), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjObjectAction3dESTextureLoad(AppMain.GMM_BS_OBJ((object)efct_3des), efct_3des.obj_3des, AppMain.ObjDataGet(709), (string)null, 0, (AppMain.AMS_AMB_HEADER)null, false);
        AppMain.ObjObjectAction3dESTextureSetByDwork(work, AppMain.ObjDataGet(710));
        AppMain.GmEffect3DESSetupBase(efct_3des, 1U, 1U);
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(chain_work.snm_work, chain_work.ball_snm_reg_id);
        AppMain.VEC_Set(ref work.pos, AppMain.FX_F32_TO_FX32(snmMtx.M03), AppMain.GMD_BOSS1_GROUND_POS_Y, 0);
        work.flag &= 4294967293U;
        AppMain.GmEffectRectInit(efct_3des.efct_com, AppMain.gm_boss1_eff_sw_atk_flag_tbl, AppMain.gm_boss1_eff_sw_def_flag_tbl, (byte)1, (byte)1);
        AppMain.ObjRectWorkSet(efct_3des.efct_com.rect_work[1], (short)-64, (short)-32, (short)64, (short)32);
        sw_work.atk_rect_timer = 10U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1EffShockwaveProcMain);
        AppMain.gmBoss1EffShockwaveSubpartInit(sw_work, 163840, true);
        AppMain.gmBoss1EffShockwaveSubpartInit(sw_work, 163840, false);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss1EffShockwaveExit));
        return efct_3des;
    }

    private static void gmBoss1EffShockwaveExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmBoss1MgrDecObjCreateCount(((AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK)AppMain.mtTaskGetTcbWork(tcb)).mgr_work);
        AppMain.GmEffectDefaultExit(tcb);
    }

    private static void gmBoss1EffShockwaveProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK effShockwaveWork = (AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK)obj_work;
        if (effShockwaveWork.atk_rect_timer != 0U)
            --effShockwaveWork.atk_rect_timer;
        else
            obj_work.flag |= 2U;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static AppMain.GMS_EFFECT_3DES_WORK gmBoss1EffShockwaveSubpartInit(
      AppMain.GMS_BOSS1_EFF_SHOCKWAVE_WORK sw_work,
      int ofst_h,
      bool is_left)
    {
        AppMain.MTM_ASSERT(ofst_h >= 0);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK()), AppMain.GMM_BS_OBJ((object)sw_work).parent_obj, (ushort)0, "B01_SW_Subpart");
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = (AppMain.GMS_EFFECT_3DES_WORK)work;
        AppMain.GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK shockwaveSubWork = (AppMain.GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK)efct_3des;
        shockwaveSubWork.mgr_work = sw_work.mgr_work;
        AppMain.gmBoss1MgrIncObjCreateCount(shockwaveSubWork.mgr_work);
        AppMain.ObjObjectAction3dESEffectLoad(AppMain.GMM_BS_OBJ((object)efct_3des), efct_3des.obj_3des, AppMain.ObjDataGet(707), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjObjectAction3dESTextureLoad(AppMain.GMM_BS_OBJ((object)efct_3des), efct_3des.obj_3des, AppMain.ObjDataGet(709), (string)null, 0, (AppMain.AMS_AMB_HEADER)null, false);
        AppMain.ObjObjectAction3dESTextureSetByDwork(work, AppMain.ObjDataGet(710));
        AppMain.GmEffect3DESSetupBase(efct_3des, 1U, 0U);
        AppMain.GmEffect3DESSetDispRotation(efct_3des, (short)AppMain.GMD_BOSS1_EFF_SHOCKWAVE_SUB_ROT_X, (short)0, (short)0);
        AppMain.GmEffect3DESSetDispOffset(efct_3des, 0.0f, -AppMain.FX_FX32_TO_F32(-16384), AppMain.FX_FX32_TO_F32(-ofst_h));
        work.pos.x = AppMain.GMM_BS_OBJ((object)sw_work).pos.x;
        work.pos.y = AppMain.GMM_BS_OBJ((object)sw_work).pos.y;
        work.pos.z = AppMain.GMM_BS_OBJ((object)sw_work).pos.z;
        if (is_left)
            work.disp_flag &= 4294967294U;
        else
            work.disp_flag |= 1U;
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss1EffShockwaveSubExit));
        return efct_3des;
    }

    private static void gmBoss1EffShockwaveSubExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmBoss1MgrDecObjCreateCount(((AppMain.GMS_BOSS1_EFF_SHOCKWAVE_SUB_WORK)AppMain.mtTaskGetTcbWork(tcb)).mgr_work);
        AppMain.GmEffectDefaultExit(tcb);
    }

    private static void gmBoss1EffScatterInit(AppMain.GMS_BOSS1_CHAIN_WORK chain_work)
    {
        for (int index = 0; index < 9; ++index)
        {
            AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT controlObjectBySize = AppMain.GmBsCmnCreateNodeControlObjectBySize(AppMain.GMM_BS_OBJ((object)chain_work), chain_work.cnm_mgr_work, chain_work.sct_cnm_reg_ids[index], chain_work.snm_work, chain_work.sct_snm_reg_ids[index], (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK()));
            AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK sct_part_ndc = (AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK)controlObjectBySize;
            controlObjectBySize.user_timer = (uint)AppMain.mtMathRand() % 20U;
            controlObjectBySize.is_enable = 0;
            AppMain.gmBoss1EffScatterSetPartParam(sct_part_ndc, index == 8);
            AppMain.GMM_BS_OBJ((object)controlObjectBySize).move_flag |= 4608U;
            AppMain.nnMakeUnitQuaternion(ref controlObjectBySize.user_quat);
            controlObjectBySize.proc_update = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1EffScatterProcWait);
        }
    }

    private static void gmBoss1EffScatterSetPartParam(
      AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK sct_part_ndc,
      bool is_ironball)
    {
        AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT cmnNodeCtrlObject = (AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)sct_part_ndc;
        int ang;
        if (is_ironball)
        {
            sct_part_ndc.is_ironball = true;
            cmnNodeCtrlObject.user_ofst.y = 32f;
            ang = AppMain.GMD_BOSS1_EFF_SCT_PART_IBALL_SPIN_SPD_DEG;
        }
        else
        {
            sct_part_ndc.is_ironball = false;
            cmnNodeCtrlObject.user_ofst.y = 8f;
            ang = AppMain.GMD_BOSS1_EFF_SCT_PART_RING_SPIN_SPD_DEG;
        }
        AppMain.nnMakeUnitQuaternion(ref sct_part_ndc.spin_quat);
        for (int index = 0; index < 2; ++index)
        {
            AppMain.NNS_VECTOR dst_vec = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            AppMain.NNS_QUATERNION dst = new AppMain.NNS_QUATERNION();
            float rand_z = AppMain.MTM_MATH_CLIP((float)((double)AppMain.FX_FX32_TO_F32(AppMain.AkMathRandFx()) * 2.0 - 1.0), -1f, 1f);
            short rand_angle = AppMain.AKM_DEGtoA16(360f * AppMain.FX_FX32_TO_F32(AppMain.AkMathRandFx()));
            AppMain.AkMathGetRandomUnitVector(dst_vec, rand_z, rand_angle);
            AppMain.nnMakeRotateAxisQuaternion(out dst, dst_vec.x, dst_vec.y, dst_vec.z, ang);
            AppMain.nnMultiplyQuaternion(ref sct_part_ndc.spin_quat, ref dst, ref sct_part_ndc.spin_quat);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(dst_vec);
        }
        if (is_ironball)
            AppMain.ObjObjectFieldRectSet(AppMain.GMM_BS_OBJ((object)cmnNodeCtrlObject), (short)-24, (short)-24, (short)24, (short)24);
        else
            AppMain.ObjObjectFieldRectSet(AppMain.GMM_BS_OBJ((object)cmnNodeCtrlObject), (short)-8, (short)-8, (short)8, (short)8);
    }

    private static void gmBoss1EffScatterSetFlyParam(
      AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK sct_part_ndc)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)sct_part_ndc);
        int num1 = (int)AppMain.mtMathRand() % 180;
        int ang = AppMain.AKM_DEGtoA32(num1 + (num1 > 90 ? 45 : -45));
        float num2 = !sct_part_ndc.is_ironball ? 5f : 3f;
        obsObjectWork.spd.y = (int)(4096.0 * (double)num2 * (double)AppMain.nnSin(ang));
        obsObjectWork.spd.x = (int)(4096.0 * (double)num2 * (double)AppMain.nnCos(ang));
        obsObjectWork.move_flag |= 128U;
    }

    private static void gmBoss1EffScatterProcWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj = (AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work;
        AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK sct_part_ndc = (AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK)ndc_obj;
        if (ndc_obj.user_timer != 0U)
        {
            --ndc_obj.user_timer;
        }
        else
        {
            AppMain.GmBsCmnAttachNCObjectToSNMNode(ndc_obj);
            AppMain.gmBoss1EffScatterSetFlyParam(sct_part_ndc);
            ndc_obj.is_enable = 1;
            ndc_obj.user_timer = 180U;
            ndc_obj.proc_update = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1EffScatterProcFly);
        }
    }

    private static void gmBoss1EffScatterProcFly(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj = (AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work;
        AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK effSctPartNdcWork = (AppMain.GMS_BOSS1_EFF_SCT_PART_NDC_WORK)ndc_obj;
        AppMain.nnMultiplyQuaternion(ref ndc_obj.user_quat, ref effSctPartNdcWork.spin_quat, ref ndc_obj.user_quat);
        AppMain.GmBsCmnSetWorldMtxFromNCObjectPosture(ndc_obj);
        if (ndc_obj.user_timer != 0U)
            --ndc_obj.user_timer;
        else
            obj_work.flag |= 4U;
    }

    private static void gmBoss1EffBombInitCreate(
      AppMain.GMS_BOSS1_EFF_BOMB_WORK bomb_work,
      int bomb_type,
      AppMain.OBS_OBJECT_WORK parent_obj,
      int pos_x,
      int pos_y,
      int width,
      int height,
      uint interval_min,
      uint interval_max)
    {
        AppMain.MTM_ASSERT((object)bomb_work);
        AppMain.MTM_ASSERT((object)parent_obj);
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

    private static void gmBoss1EffBombUpdateCreate(AppMain.GMS_BOSS1_EFF_BOMB_WORK bomb_work)
    {
        AppMain.MTM_ASSERT((object)bomb_work.parent_obj);
        if (bomb_work.interval_timer != 0U)
        {
            --bomb_work.interval_timer;
        }
        else
        {
            int v2_1 = bomb_work.area[0];
            int v2_2 = bomb_work.area[1];
            int num1 = AppMain.FX_Mul(AppMain.AkMathRandFx(), v2_1);
            int num2 = AppMain.FX_Mul(AppMain.AkMathRandFx(), v2_2);
            if (bomb_work.bomb_type == 0)
            {
                AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 7);
                if (--bomb_work.interval_timer_sound <= 0)
                {
                    bomb_work.interval_timer_sound = 3;
                    AppMain.GmSoundPlaySE("Boss0_02");
                }
                AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)gmsEffect3DesWork);
                AppMain.MTM_ASSERT((object)obsObjectWork);
                obsObjectWork.pos.x = bomb_work.pos[0] - (v2_1 >> 1) + num1;
                obsObjectWork.pos.y = bomb_work.pos[1] - (v2_2 >> 1) + num2;
                obsObjectWork.pos.z = AppMain.GMM_BS_OBJ((object)bomb_work.parent_obj).pos.z + 131072;
                uint num3 = (uint)((long)AppMain.AkMathRandFx() * (long)(bomb_work.interval_max - bomb_work.interval_min) >> 12);
                bomb_work.interval_timer = bomb_work.interval_min + num3;
            }
            else
                AppMain.MTM_ASSERT(false);
        }
    }

    private static void gmBoss1EffDamageInit(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 0U)).pos.z += 131072;
    }

    private static void gmBoss1EffAfterburnerSetEnable(
      AppMain.GMS_BOSS1_BODY_WORK body_work,
      bool is_enable)
    {
        if (is_enable)
        {
            AppMain.MTM_ASSERT(0 == ((int)body_work.flag & 16));
            AppMain.MTM_ASSERT(0 == ((int)body_work.flag & 33554432));
            body_work.flag |= 33554432U;
        }
        else
        {
            body_work.flag &= 4294967279U;
            body_work.flag &= 4261412863U;
        }
    }

    private static void gmBoss1EffAfterburnerUpdateCreate(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 33554432) == 0)
            return;
        body_work.flag &= 4261412863U;
        body_work.flag |= 16U;
        AppMain.gmBoss1EffAfterburnerInit(body_work);
    }

    private static void gmBoss1EffAfterburnerInit(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 4U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -30f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1EffAfterburnerProcMain);
    }

    private static void gmBoss1EffAfterburnerProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_BODY_WORK parentObj = (AppMain.GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((int)parentObj.snm_work.reg_node_max);
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 16) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1);
    }

    private static void gmBoss1EffABSmokeInit(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 5U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1EffABSmokeProcMain);
    }

    private static void gmBoss1EffABSmokeProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_BODY_WORK parentObj = (AppMain.GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((int)parentObj.snm_work.reg_node_max);
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1);
    }

    private static void gmBoss1EffBodySmokeInit(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 3U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1EffBodySmokeProcMain);
    }

    private static void gmBoss1EffBodySmokeProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_BODY_WORK parentObj = (AppMain.GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((int)parentObj.snm_work.reg_node_max);
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1);
    }

    private static void gmBoss1EffBodySmallSmokeInit(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK parent_obj = AppMain.GMM_BS_OBJ((object)body_work);
        for (int index = 0; index < 3; ++index)
        {
            AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(parent_obj, 2U);
            AppMain.GmEffect3DESSetDispOffset(efct_3des, AppMain.gm_boss1_eff_small_smoke_disp_ofst_tbl[index][0], AppMain.gm_boss1_eff_small_smoke_disp_ofst_tbl[index][1], AppMain.gm_boss1_eff_small_smoke_disp_ofst_tbl[index][2]);
            AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1EffBodySmallSmokeProcMain);
        }
    }

    private static void gmBoss1EffBodySmallSmokeProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_BODY_WORK parentObj = (AppMain.GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((int)parentObj.snm_work.reg_node_max);
        obj_work.flag &= 4294966271U;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1);
    }

    private static void gmBoss1EffBodyDebrisInit(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        ((AppMain.OBS_OBJECT_WORK)AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 1U)).parent_ofst.x = -65536;
    }

    private static void gmBoss1EffSweatInit(AppMain.GMS_BOSS1_EGG_WORK egg_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)egg_work), 93);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 32f, 0.0f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1EffSweatProcMain);
        egg_work.flag |= 2U;
    }

    private static void gmBoss1EffSweatProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_EGG_WORK parentObj = (AppMain.GMS_BOSS1_EGG_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((object)parentObj);
        if (((int)parentObj.flag & 2) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

}