using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

public partial class AppMain
{
    public static void GmBoss5EfctBuild()
    {
        for (int index = 0; index < 27; ++index)
        {
            AppMain.GMS_BOSS5_EFCT_DATA_INFO bosS5EfctDataInfo = AppMain.gm_boss5_efct_data_info_tbl[index];
            AppMain.OBS_DATA_WORK model_dwork = (AppMain.OBS_DATA_WORK)null;
            AppMain.OBS_DATA_WORK object_dwork = (AppMain.OBS_DATA_WORK)null;
            if (bosS5EfctDataInfo.use_model != 0)
            {
                model_dwork = AppMain.ObjDataGet(bosS5EfctDataInfo.model_dwork_no);
                object_dwork = AppMain.ObjDataGet(bosS5EfctDataInfo.object_dwork_no);
            }
            AppMain.GmEfctBossBuildSingleDataReg(bosS5EfctDataInfo.tex_amb_arc_idx, AppMain.ObjDataGet(bosS5EfctDataInfo.tex_amb_dwork_no), AppMain.ObjDataGet(bosS5EfctDataInfo.tex_list_dwork_no), bosS5EfctDataInfo.model_arc_idx, model_dwork, object_dwork, AppMain.g_gm_gamedat_enemy_arc);
        }
    }

    public static void GmBoss5EfctFlush()
    {
        AppMain.GmEfctBossFlushSingleDataInit();
    }

    public static void GmBoss5EfctTryStartLeakage(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 2048) != 0)
            return;
        AppMain.GmSoundPlaySE("FinalBoss11", body_work.se_hnd_leakage);
        AppMain.gmBoss5EfctCreateLeakage(body_work);
        body_work.flag |= 2048U;
    }

    public static void GmBoss5EfctEndLeakage(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.GmBoss5EfctEndLeakage(body_work, 0);
    }

    public static void GmBoss5EfctEndLeakage(AppMain.GMS_BOSS5_BODY_WORK body_work, int no_vanish)
    {
        if (((int)body_work.flag & 2048) != 0)
            AppMain.GsSoundStopSeHandle(body_work.se_hnd_leakage, 30);
        body_work.flag &= 4294965247U;
    }

    public static void GmBoss5EfctStartPrelimLeakage(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag |= 1048576U;
        AppMain.gmBoss5EfctCreatePrelimLeakage(body_work);
    }

    public static void GmBoss5EfctEndPrelimLeakage(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4293918719U;
    }

    public static void GmBoss5EfctCreateWalkStepSmoke(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int leg_type)
    {
    }

    public static void GmBoss5EfctCreateRunStepSmoke(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int leg_type)
    {
    }

    public static void GmBoss5EfctCreateBerserkStampSmoke(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int leg_type,
      uint spawn_delay)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)(AppMain.GMS_EFFECT_COM_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 9, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        s5EfctGeneralWork.ref_node_snm_id = body_work.leg_snm_reg_ids[leg_type];
        obsObjectWork.disp_flag |= 4128U;
        s5EfctGeneralWork.timer = spawn_delay;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctBerserkStampSmokeProcWaitStart);
    }

    public static void GmBoss5EfctCreateCrashLandingSmoke(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 10, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_EFFECT_COM_WORK gmsEffectComWork = (AppMain.GMS_EFFECT_COM_WORK)s5EfctGeneralWork;
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)s5EfctGeneralWork);
        obsObjectWork.pos.x = AppMain.GMM_BS_OBJ((object)body_work).pos.x;
        obsObjectWork.pos.y = body_work.ground_v_pos;
        obsObjectWork.pos.z = AppMain.GMM_BS_OBJ((object)body_work).pos.z + 262144;
    }

    public static void GmBoss5EfctCreateBreakingGlass(AppMain.OBS_OBJECT_WORK parent_obj)
    {
        if (AppMain.ObjViewOutCheck(parent_obj.pos.x, parent_obj.pos.y, (short)0, (short)0, (short)-((int)AppMain.OBD_OBJ_CLIP_LCD_Y / 2), (short)0, (short)((int)AppMain.OBD_OBJ_CLIP_LCD_Y / 2)) != 0)
            return;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)AppMain.gmBoss5EfctEsCreate(parent_obj, 11, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_EFFECT_COM_WORK gmsEffectComWork = (AppMain.GMS_EFFECT_COM_WORK)s5EfctGeneralWork;
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)s5EfctGeneralWork);
        obsObjectWork.pos.y += -131072;
        obsObjectWork.pos.z += 262144;
    }

    public static void GmBoss5EfctStartJet(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag |= 16384U;
        AppMain.gmBoss5EfctCreateJet(body_work);
    }

    public static void GmBoss5EfctEndJet(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294950911U;
    }

    public static void GmBoss5EfctStartJetSmoke(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag |= 32768U;
        AppMain.gmBoss5EfctCreateJetSmoke(body_work);
    }

    public static void GmBoss5EfctEndJetSmoke(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        body_work.flag &= 4294934527U;
    }

    public static void GmBoss5EfctTryStartRocketLeakage(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (((int)rkt_work.flag & 4) != 0)
            return;
        AppMain.gmBoss5EfctCreateRocketLeakage(rkt_work);
        rkt_work.flag |= 4U;
    }

    public static void GmBoss5EfctEndRocketLeakage(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.flag &= 4294967291U;
    }

    public static void GmBoss5EfctCreateRocketLaunch(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
    }

    public static void GmBoss5EfctCreateRocketDock(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int rkt_type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)(AppMain.GMS_EFFECT_COM_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 15, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
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
        AppMain.nnMakeTranslateMatrix(s5EfctGeneralWork.ofst_mtx, x, 0.0f, 0.0f);
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctRocketDockProcMain);
    }

    public static void GmBoss5EfctStartRocketJet(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (((int)rkt_work.flag & 16) != 0)
            return;
        AppMain.gmBoss5EfctCreateRocketJet(rkt_work, 0);
        rkt_work.flag |= 16U;
    }

    public static void GmBoss5EfctEndRocketJet(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.flag &= 4294967279U;
    }

    public static void GmBoss5EfctStartRocketJetReverse(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (((int)rkt_work.flag & 32) != 0)
            return;
        AppMain.gmBoss5EfctCreateRocketJet(rkt_work, 1);
        rkt_work.flag |= 32U;
    }

    public static void GmBoss5EfctEndRocketJetReverse(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.flag &= 4294967263U;
    }

    public static void GmBoss5EfctCreateRocketLandingShockwave(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
    }

    public static void GmBoss5EfctCreateLandingShockwave(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.GMS_EFFECT_COM_WORK gmsEffectComWork = (AppMain.GMS_EFFECT_COM_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 19, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)gmsEffectComWork;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        obsObjectWork.pos.z += 262144;
        AppMain.GmBsCmnSetEfctAtkVsPly((AppMain.GMS_EFFECT_COM_WORK)obsObjectWork, (short)128);
        obsObjectWork.flag |= 16U;
        AppMain.ObjRectWorkSet(gmsEffectComWork.rect_work[1], (short)-88, (short)-32, (short)88, (short)32);
        gmsEffectComWork.rect_work[1].flag &= 4294965247U;
        s5EfctGeneralWork.timer = 8U;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctLandingShockwaveProcMain);
    }

    public static void GmBoss5EfctCreateStrikeShockwave(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      uint spawn_delay)
    {
        AppMain.GMS_EFFECT_COM_WORK gmsEffectComWork = (AppMain.GMS_EFFECT_COM_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 20, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)gmsEffectComWork;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        AppMain.GmBsCmnSetEfctAtkVsPly((AppMain.GMS_EFFECT_COM_WORK)obsObjectWork, (short)64);
        obsObjectWork.flag |= 16U;
        AppMain.ObjRectWorkSet(gmsEffectComWork.rect_work[1], (short)-64, (short)-64, (short)64, (short)32);
        gmsEffectComWork.rect_work[1].flag &= 4294965247U;
        obsObjectWork.flag |= 2U;
        obsObjectWork.disp_flag |= 4128U;
        s5EfctGeneralWork.timer = spawn_delay;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctStrikeShockwaveProcWaitStart);
    }

    public static void GmBoss5EfctTargetCursorInit(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.gmBoss5EfctCreateTargetCursorStart(body_work);
    }

    public static void GmBoss5EfctCrashCursorInit(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int pos_x,
      uint duration_time)
    {
        AppMain.gmBoss5EfctCreateCrashCursorStart(body_work, pos_x, duration_time);
    }

    public static void GmBoss5EfctCreateVulcanFire(
      AppMain.GMS_BOSS5_TURRET_WORK trt_work,
      AppMain.VecFx32 pos,
      int angle)
    {
        AppMain.UNREFERENCED_PARAMETER((object)trt_work);
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)trt_work), 14);
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)efct_3des;
        obsObjectWork.dir.z = (ushort)(short)((long)ushort.MaxValue & (long)angle);
        AppMain.GmEffect3DESAddDispRotation(efct_3des, (short)AppMain.AKM_DEGtoA32(90), (short)0, (short)0);
        obsObjectWork.pos.Assign(pos);
        AppMain.GmSoundPlaySE("FinalBoss14");
    }

    public static void GmBoss5EfctCreateVulcanBullet(
      AppMain.GMS_BOSS5_TURRET_WORK trt_work,
      AppMain.VecFx32 pos,
      int angle,
      int spd)
    {
        AppMain.UNREFERENCED_PARAMETER((object)trt_work);
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)trt_work), 15);
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)efct_3des;
        AppMain.GmBsCmnSetEfctAtkVsPly(efct_3des.efct_com, (short)16);
        obsObjectWork.flag |= 16U;
        AppMain.OBS_RECT_WORK pRec = efct_3des.efct_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec, (short)-8, (short)-8, (short)8, (short)8);
        pRec.flag |= 4U;
        obsObjectWork.dir.z = (ushort)(short)((long)ushort.MaxValue & (long)angle);
        AppMain.GmEffect3DESAddDispRotation(efct_3des, (short)AppMain.AKM_DEGtoA32(-90), (short)0, (short)0);
        obsObjectWork.pos.Assign(pos);
        obsObjectWork.pos.z += 262144;
        obsObjectWork.spd.x = AppMain.FX_Mul(spd, AppMain.FX_F32_TO_FX32(AppMain.nnCos(angle)));
        obsObjectWork.spd.y = AppMain.FX_Mul(spd, AppMain.FX_F32_TO_FX32(AppMain.nnSin(angle)));
        obsObjectWork.spd.z = 0;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctVulcanBulletProcMain);
    }

    public static void GmBoss5EfctCreateSmallExplosion(int pos_x, int pos_y, int pos_z)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 7);
        obsObjectWork.pos.x = pos_x;
        obsObjectWork.pos.y = pos_y;
        obsObjectWork.pos.z = pos_z;
    }

    public static void GmBoss5EfctCreateBigExplosion(int pos_x, int pos_y, int pos_z)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 8);
        obsObjectWork.pos.x = pos_x;
        obsObjectWork.pos.y = pos_y;
        obsObjectWork.pos.z = pos_z;
    }

    public static void GmBoss5EfctCreateFragments(int pos_x, int pos_y, int pos_z)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctBossCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 0U);
        obsObjectWork.pos.x = pos_x;
        obsObjectWork.pos.y = pos_y;
        obsObjectWork.pos.z = pos_z;
    }

    public static void GmBoss5EfctCreateDamage(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctBossCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 0U);
        obsObjectWork.pos.x = body_work.part_obj_core.pos.x;
        obsObjectWork.pos.y = body_work.part_obj_core.pos.y;
        obsObjectWork.pos.z = body_work.part_obj_core.pos.z + 262144;
    }

    public static void GmBoss5EfctStartRocketSmoke(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.flag |= 64U;
        AppMain.gmBoss5EfctCreateRocketSmoke(rkt_work);
    }

    public static void GmBoss5EfctEndRocketSmoke(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.flag &= 4294967231U;
    }

    public static void GmBoss5EfctBreakdownSmokesInit(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      uint duration_time)
    {
        for (uint index = 0; index < 2U; ++index)
        {
            AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 3U);
            AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)efct_3des;
            AppMain.GmEffect3DESChangeBase(efct_3des, 1U, (uint)((ulong)efct_3des.saved_init_flag & 18446744073709551613UL));
            AppMain.GmEffect3DESSetDispOffset(efct_3des, AppMain.gm_boss5_efct_breakdown_smoke_disp_ofst_tbl[(int)index][0], AppMain.gm_boss5_efct_breakdown_smoke_disp_ofst_tbl[(int)index][1], AppMain.gm_boss5_efct_breakdown_smoke_disp_ofst_tbl[(int)index][2]);
            AppMain.GmEffect3DESSetDispRotation(efct_3des, AppMain.gm_boss5_efct_breakdown_smoke_disp_rot_tbl[(int)index][0], AppMain.gm_boss5_efct_breakdown_smoke_disp_rot_tbl[(int)index][1], AppMain.gm_boss5_efct_breakdown_smoke_disp_rot_tbl[(int)index][2]);
            obsObjectWork.user_timer = (int)duration_time;
            obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctBreakdownSmokeProcLoop);
        }
    }

    public static void GmBoss5EfctBodySmallSmokesInit(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        for (uint part_idx = 0; part_idx < 3U; ++part_idx)
            AppMain.gmBoss5EfctCreateBodySmallSmoke(body_work, part_idx);
    }

    public static void GmBoss5EfctBerserkSteamInit(AppMain.GMS_BOSS5_BODY_WORK body_work, uint count)
    {
        for (uint part_idx = 0; part_idx < 2U; ++part_idx)
            AppMain.gmBoss5EfctCreateBerserkSteam(body_work, count, part_idx);
    }

    public static void GmBoss5EfctStartEggSweat(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        egg_work.flag |= AppMain.GMD_BOSS5_EGG_FLAG_SWEAT_ACTIVE;
        AppMain.gmBoss5EfctCreateEggSweat(egg_work);
    }

    public static void GmBoss5EfctEndEggSweat(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        egg_work.flag &= ~AppMain.GMD_BOSS5_EGG_FLAG_SWEAT_ACTIVE;
    }

    public static void GmBoss5EfctCreateRocketRollSpark(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)rkt_work), 48);
        obsObjectWork.pos.y = ((AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)rkt_work).parent_obj).ground_v_pos;
        obsObjectWork.dir.z = AppMain.GMD_BOSS5_EFCT_ROCKET_ROLLING_SPARK_DIR_Z;
        obsObjectWork.pos.z += 131072;
    }

    public static AppMain.GMS_EFFECT_3DES_WORK gmBoss5EfctEsCreate(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int efct_idx)
    {
        return AppMain.gmBoss5EfctEsCreate(parent_obj, efct_idx, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DES_WORK()));
    }

    public static AppMain.GMS_EFFECT_3DES_WORK gmBoss5EfctEsCreate(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int efct_idx,
      AppMain.TaskWorkFactoryDelegate work_size)
    {
        AppMain.GMS_EFFECT_CREATE_PARAM create_param = AppMain.gm_boss5_efct_create_param_tbl[efct_idx];
        AppMain.GMS_BOSS5_EFCT_DATA_INFO bosS5EfctDataInfo = AppMain.gm_boss5_efct_data_info_tbl[efct_idx];
        AppMain.OBS_DATA_WORK model_dwork;
        AppMain.OBS_DATA_WORK object_dwork;
        if (create_param.model_idx != -1)
        {
            model_dwork = AppMain.ObjDataGet(bosS5EfctDataInfo.model_dwork_no);
            object_dwork = AppMain.ObjDataGet(bosS5EfctDataInfo.object_dwork_no);
        }
        else
        {
            model_dwork = (AppMain.OBS_DATA_WORK)null;
            object_dwork = (AppMain.OBS_DATA_WORK)null;
        }
        return AppMain.GmEffect3dESCreateByParam(create_param, parent_obj, (object)AppMain.g_gm_gamedat_enemy_arc, AppMain.ObjDataGet(bosS5EfctDataInfo.ame_dwork_no), AppMain.ObjDataGet(bosS5EfctDataInfo.tex_amb_dwork_no), AppMain.ObjDataGet(bosS5EfctDataInfo.tex_list_dwork_no), model_dwork, object_dwork, work_size);
    }

    public static void gmBoss5EfctCreateLeakage(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)(AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 7, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()))).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctLeakagePartProcMain);
    }

    public static void gmBoss5EfctLeakagePartProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        obj_work.pos.z += 327680;
        if (((int)parentObj.flag & 2048) != 0)
            return;
        AppMain.ObjDrawKillAction3DES(obj_work);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctLeakageProcFade);
    }

    public static void gmBoss5EfctLeakageProcFade(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreatePrelimLeakage(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)(AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 7, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()))).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctPrelimLeakageProcLoop);
    }

    public static void gmBoss5EfctPrelimLeakageProcLoop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        obj_work.pos.z += 327680;
        if (((int)parentObj.flag & 1048576) != 0)
            return;
        AppMain.ObjDrawKillAction3DES(obj_work);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctPrelimLeakageProcFade);
    }

    public static void gmBoss5EfctPrelimLeakageProcFade(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctBerserkStampSmokeProcWaitStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (s5EfctGeneralWork.timer != 0U)
        {
            --s5EfctGeneralWork.timer;
        }
        else
        {
            AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
            AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(parentObj.snm_work, s5EfctGeneralWork.ref_node_snm_id);
            obj_work.pos.x = AppMain.FX_F32_TO_FX32(snmMtx.M03);
            obj_work.pos.y = parentObj.ground_v_pos;
            obj_work.pos.z = AppMain.FX_F32_TO_FX32(snmMtx.M23);
            obj_work.pos.z = 65536;
            obj_work.disp_flag &= 4294963167U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd);
        }
    }

    public static void gmBoss5EfctCreateJet(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        int num = ((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) == 0 ? body_work.nozzle_snm_reg_ids[1] : body_work.nozzle_snm_reg_ids[0];
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 12, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_EFFECT_COM_WORK gmsEffectComWork = (AppMain.GMS_EFFECT_COM_WORK)s5EfctGeneralWork;
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)s5EfctGeneralWork);
        obsObjectWork.obj_3des.ecb.drawObjState = 0U;
        s5EfctGeneralWork.ref_node_snm_id = num;
        AppMain.GmBsCmnSetEfctAtkVsPly((AppMain.GMS_EFFECT_COM_WORK)obsObjectWork, (short)16);
        obsObjectWork.flag |= 16U;
        AppMain.ObjRectWorkSet(gmsEffectComWork.rect_work[1], (short)-8, (short)0, (short)8, (short)88);
        gmsEffectComWork.rect_work[1].flag |= 2048U;
        s5EfctGeneralWork.timer = 65U;
        AppMain.GmBoss5Init1ShotTimer(s5EfctGeneralWork.se_timer, 50U);
        AppMain.GMM_BS_OBJ((object)s5EfctGeneralWork).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctJetProcMain);
    }

    public static void gmBoss5EfctJetProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (AppMain.GmBoss5Update1ShotTimer(s5EfctGeneralWork.se_timer) != 0)
            AppMain.GmSoundPlaySE("FinalBoss04");
        if (s5EfctGeneralWork.timer != 0U)
            --s5EfctGeneralWork.timer;
        else
            ((AppMain.GMS_EFFECT_COM_WORK)obj_work).rect_work[1].flag &= 4294965247U;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, s5EfctGeneralWork.ref_node_snm_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        if (((int)parentObj.flag & 16384) == 0 && ((int)s5EfctGeneralWork.flag & 1) == 0)
        {
            s5EfctGeneralWork.flag |= 1U;
            AppMain.ObjDrawKillAction3DES(obj_work);
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateJetSmoke(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 13, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_EFFECT_COM_WORK gmsEffectComWork = (AppMain.GMS_EFFECT_COM_WORK)s5EfctGeneralWork;
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)s5EfctGeneralWork);
        int snm_reg_id = ((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) == 0 ? body_work.nozzle_snm_reg_ids[1] : body_work.nozzle_snm_reg_ids[0];
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(body_work.snm_work, snm_reg_id);
        obsObjectWork.pos.x = AppMain.FX_F32_TO_FX32(snmMtx.M03);
        obsObjectWork.pos.y = body_work.ground_v_pos;
        obsObjectWork.pos.z = AppMain.GMM_BS_OBJ((object)body_work).pos.z;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctJetSmokeProcMain);
    }

    public static void gmBoss5EfctJetSmokeProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (((int)parentObj.flag & 32768) == 0 && ((int)s5EfctGeneralWork.flag & 1) == 0)
        {
            s5EfctGeneralWork.flag |= 1U;
            AppMain.ObjDrawKillAction3DES(obj_work);
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateRocketLeakage(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)rkt_work), 14, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.nnMakeTranslateMatrix(s5EfctGeneralWork.ofst_mtx, -7f, 0.0f, 0.0f);
        s5EfctGeneralWork.se_handle = AppMain.GsSoundAllocSeHandle();
        AppMain.mtTaskChangeTcbDestructor(((AppMain.OBS_OBJECT_WORK)s5EfctGeneralWork).tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss5EfctRocketLeakageExit));
        AppMain.GmSoundPlaySE("FinalBoss11", s5EfctGeneralWork.se_handle);
        AppMain.GMM_BS_OBJ((object)s5EfctGeneralWork).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctRocketLeakageProcMain);
    }

    public static void gmBoss5EfctRocketLeakageExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GsSoundFreeSeHandle(((AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)AppMain.mtTaskGetTcbWork(tcb)).se_handle);
        AppMain.GmEffectDefaultExit(tcb);
    }

    public static void gmBoss5EfctRocketLeakageProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_ROCKET_WORK parentObj = (AppMain.GMS_BOSS5_ROCKET_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.drill_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos, s5EfctGeneralWork.ofst_mtx);
        obj_work.pos.z += 262144;
        if (((int)parentObj.flag & 4) == 0 && ((int)s5EfctGeneralWork.flag & 1) == 0)
        {
            s5EfctGeneralWork.flag |= 1U;
            AppMain.ObjDrawKillAction3DES(obj_work);
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        AppMain.GsSoundStopSeHandle(s5EfctGeneralWork.se_handle);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctRocketDockProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, s5EfctGeneralWork.ref_node_snm_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos, s5EfctGeneralWork.ofst_mtx);
        obj_work.pos.z += 65536;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateRocketJet(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      int is_rev_jet)
    {
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)rkt_work), 16, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        if (is_rev_jet != 0)
        {
            AppMain.nnMakeTranslateMatrix(s5EfctGeneralWork.ofst_mtx, 4f, 0.0f, 0.0f);
            AppMain.nnRotateYMatrix(s5EfctGeneralWork.ofst_mtx, s5EfctGeneralWork.ofst_mtx, AppMain.AKM_DEGtoA32(180));
            s5EfctGeneralWork.user_flag |= 1U;
        }
        else
            AppMain.nnMakeTranslateMatrix(s5EfctGeneralWork.ofst_mtx, -6f, 0.0f, 0.0f);
        AppMain.GMM_BS_OBJ((object)s5EfctGeneralWork).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctRocketJetProcMain);
    }

    public static void gmBoss5EfctRocketJetProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_ROCKET_WORK parentObj = (AppMain.GMS_BOSS5_ROCKET_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        int num = 0;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.drill_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos, s5EfctGeneralWork.ofst_mtx);
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
            AppMain.ObjDrawKillAction3DES(obj_work);
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctLandingShockwaveProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (s5EfctGeneralWork.timer != 0U)
        {
            --s5EfctGeneralWork.timer;
        }
        else
        {
            AppMain.GMS_EFFECT_COM_WORK gmsEffectComWork = (AppMain.GMS_EFFECT_COM_WORK)obj_work;
            gmsEffectComWork.rect_work[1].flag |= 2048U;
            gmsEffectComWork.rect_work[1].flag &= 4294967291U;
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctStrikeShockwaveProcWaitStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (s5EfctGeneralWork.timer != 0U)
        {
            --s5EfctGeneralWork.timer;
        }
        else
        {
            AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
            AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(parentObj.snm_work, parentObj.armpt_snm_reg_ids[1][2]);
            obj_work.pos.x = AppMain.FX_F32_TO_FX32(snmMtx.M03);
            obj_work.pos.y = parentObj.ground_v_pos;
            obj_work.pos.z = AppMain.FX_F32_TO_FX32(snmMtx.M23);
            obj_work.pos.z += 262144;
            obj_work.flag &= 4294967293U;
            obj_work.disp_flag &= 4294963167U;
            s5EfctGeneralWork.timer = 15U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctStrikeShockwaveProcLoop);
        }
    }

    public static void gmBoss5EfctStrikeShockwaveProcLoop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        if (s5EfctGeneralWork.timer != 0U)
        {
            --s5EfctGeneralWork.timer;
        }
        else
        {
            AppMain.GMS_EFFECT_COM_WORK gmsEffectComWork = (AppMain.GMS_EFFECT_COM_WORK)obj_work;
            gmsEffectComWork.rect_work[1].flag |= 2048U;
            gmsEffectComWork.rect_work[1].flag &= 4294967291U;
        }
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateTargetCursorStart(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 23, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_EFFECT_COM_WORK parent_efct = (AppMain.GMS_EFFECT_COM_WORK)efct_3des;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)parent_efct;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.pos.Assign(AppMain.GmBsCmnGetPlayerObj().pos);
        targ_cursor.timer = 120U;
        obsObjectWork.obj_3des.speed = 0.5f;
        AppMain.gmBoss5EfctTargetCursorInitFlickerNoDisp(targ_cursor);
        AppMain.gmBoss5EfctCreateTargetCursorFlash(parent_efct, 26);
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctTargetCursorStartProcMain);
    }

    public static void gmBoss5EfctTargetCursorStartProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        AppMain.GmBoss5BodyGetPlySearchPos(parentObj, out obj_work.pos);
        float num = AppMain.MTM_MATH_CLIP((float)(1.0 - (double)targ_cursor.timer / 120.0), 0.0f, 1f);
        obj_work.obj_3des.speed = (float)(0.5 + (double)num * 0.5);
        float nodisp_time = 10f / obj_work.obj_3des.speed;
        float cycle_time = 20f / obj_work.obj_3des.speed;
        AppMain.gmBoss5EfctTargetCursorUpdateFlickerNoDisp(targ_cursor, nodisp_time, cycle_time);
        if (targ_cursor.timer != 0U)
        {
            --targ_cursor.timer;
        }
        else
        {
            AppMain.gmBoss5EfctCreateTargetCursorLoop(parentObj, targ_cursor.efct_3des.efct_com);
            obj_work.flag |= 4U;
        }
    }

    public static void gmBoss5EfctCreateTargetCursorLoop(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      AppMain.GMS_EFFECT_COM_WORK former_efct)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 21, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_EFFECT_COM_WORK parent_efct = (AppMain.GMS_EFFECT_COM_WORK)efct_3des;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)parent_efct;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.pos.Assign(((AppMain.OBS_OBJECT_WORK)former_efct).pos);
        obsObjectWork.obj_3des.speed = 0.8f;
        AppMain.gmBoss5EfctCreateTargetCursorFlash(parent_efct, 24);
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctTargetCursorLoopProcMain);
    }

    public static void gmBoss5EfctTargetCursorLoopProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        AppMain.GmBoss5BodyGetPlySearchPos(parentObj, out obj_work.pos);
        obj_work.obj_3des.speed += 0.005f;
        obj_work.obj_3des.speed = AppMain.MTM_MATH_CLIP(obj_work.obj_3des.speed, 0.0f, 1.5f);
        float nodisp_time = 10f / obj_work.obj_3des.speed;
        float cycle_time = 20f / obj_work.obj_3des.speed;
        AppMain.gmBoss5EfctTargetCursorUpdateFlickerNoDisp(targ_cursor, nodisp_time, cycle_time);
        if (((int)parentObj.flag & 2) != 0)
            return;
        AppMain.gmBoss5EfctCreateTargetCursorEnd(parentObj, (AppMain.GMS_EFFECT_COM_WORK)obj_work);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateTargetCursorEnd(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      AppMain.GMS_EFFECT_COM_WORK former_efct)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 22, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_EFFECT_COM_WORK parent_efct = (AppMain.GMS_EFFECT_COM_WORK)efct_3des;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)parent_efct;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.pos.Assign(((AppMain.OBS_OBJECT_WORK)former_efct).pos);
        AppMain.gmBoss5EfctCreateTargetCursorFlash(parent_efct, 25);
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctTargetCursorEndProcMain);
    }

    public static void gmBoss5EfctTargetCursorEndProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateTargetCursorFlash(
      AppMain.GMS_EFFECT_COM_WORK parent_efct,
      int efct_idx)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.gmBoss5EfctEsCreate((AppMain.OBS_OBJECT_WORK)parent_efct, efct_idx);
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)efct_3des;
        obsObjectWork.disp_flag |= 32U;
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.obj_3des.speed = ((AppMain.OBS_OBJECT_WORK)parent_efct).obj_3des.speed;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctTargetCursorFlashProcMain);
    }

    public static void gmBoss5EfctTargetCursorFlashProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3des.speed = obj_work.parent_obj.obj_3des.speed;
        if (((int)obj_work.parent_obj.disp_flag & 32) != 0)
            obj_work.disp_flag &= 4294967263U;
        else
            obj_work.disp_flag |= 32U;
    }

    public static void gmBoss5EfctTargetCursorInitFlickerNoDisp(
      AppMain.GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor)
    {
        targ_cursor.ratio_timer = 0.0f;
    }

    public static void gmBoss5EfctTargetCursorUpdateFlickerNoDisp(
      AppMain.GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor,
      float nodisp_time,
      float cycle_time)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)targ_cursor;
        ++targ_cursor.ratio_timer;
        if ((double)targ_cursor.ratio_timer >= (double)cycle_time)
            targ_cursor.ratio_timer = 0.0f;
        if ((double)targ_cursor.ratio_timer >= (double)nodisp_time)
            obsObjectWork.disp_flag &= 4294967263U;
        else
            obsObjectWork.disp_flag |= 32U;
    }

    public static void gmBoss5EfctCreateCrashCursorStart(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int pos_x,
      uint duration_time)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 23, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_EFFECT_COM_WORK parent_efct = (AppMain.GMS_EFFECT_COM_WORK)efct_3des;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)parent_efct;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        s5EfctGeneralWork.user_work = (uint)pos_x;
        s5EfctGeneralWork.timer = duration_time;
        obsObjectWork.user_work = duration_time;
        s5EfctGeneralWork.user_flag = ((int)AppMain.mtMathRand() & 1) == 0 ? 0U : 1U;
        AppMain.gmBoss5EfctCrashCursorStartSetCurPos(s5EfctGeneralWork, 0.0f, (int)s5EfctGeneralWork.user_flag);
        obsObjectWork.pos.y = body_work.ground_v_pos - 131072;
        obsObjectWork.pos.z = AppMain.GmBsCmnGetPlayerObj().pos.z;
        AppMain.gmBoss5EfctTargetCursorInitFlickerNoDisp(s5EfctGeneralWork);
        AppMain.gmBoss5EfctCreateTargetCursorFlash(parent_efct, 26);
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctCrashCursorStartProcMain);
    }

    public static void gmBoss5EfctCrashCursorStartProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work;
        AppMain.gmBoss5EfctTargetCursorUpdateFlickerNoDisp(s5EfctGeneralWork, 10f, 20f);
        if (s5EfctGeneralWork.timer != 0U)
        {
            --s5EfctGeneralWork.timer;
            float ratio = AppMain.MTM_MATH_CLIP((float)((1.0 - (double)AppMain.nnSin(AppMain.nnArcCos((1.0 - (double)s5EfctGeneralWork.timer / (double)obj_work.user_work) * 0.899999976158142))) / (1.0 - (double)AppMain.nnSin(AppMain.nnArcCos(0.899999976158142)))), 0.0f, 1f);
            AppMain.gmBoss5EfctCrashCursorStartSetCurPos(s5EfctGeneralWork, ratio, (int)s5EfctGeneralWork.user_flag);
        }
        else
        {
            AppMain.gmBoss5EfctCrashCursorStartSetCurPos(s5EfctGeneralWork, 1f, (int)s5EfctGeneralWork.user_flag);
            AppMain.gmBoss5EfctCreateCrashCursorLoop(parentObj, obj_work.pos.x);
            obj_work.flag |= 4U;
        }
    }

    public static void gmBoss5EfctCrashCursorStartSetCurPos(
      AppMain.GMS_BOSS5_EFCT_GENERAL_WORK ctarg_start,
      float ratio,
      int app_dir_left)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)ctarg_start;
        int low = AppMain.GMM_BOSS5_AREA_CENTER_X() - 1048576;
        int high = AppMain.GMM_BOSS5_AREA_CENTER_X() + 1048576;
        int num1 = 2097152 - (int)(4096.0 * (double)AppMain.fmod(AppMain.FX_FX32_TO_F32(13631488), AppMain.FX_FX32_TO_F32(2097152)));
        int num2 = AppMain.MTM_MATH_CLIP((int)ctarg_start.user_work, low, high);
        int num3 = app_dir_left == 0 ? num1 + 2097152 + (num2 - low) : num1 + (high - num2);
        int num4 = (int)(13631488.0 * (double)ratio) + num3;
        int num5 = (int)(4096.0 * (double)AppMain.fmod(AppMain.FX_FX32_TO_F32(num4), AppMain.FX_FX32_TO_F32(2097152)));
        if ((AppMain.FX_Div(num4, 2097152) >> 12 & 1) != 0)
            obsObjectWork.pos.x = high - num5;
        else
            obsObjectWork.pos.x = low + num5;
    }

    public static void gmBoss5EfctCreateCrashCursorLoop(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int pos_x)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 21, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.GMS_EFFECT_COM_WORK parent_efct = (AppMain.GMS_EFFECT_COM_WORK)efct_3des;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)parent_efct;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK targ_cursor = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.pos.x = pos_x;
        obsObjectWork.pos.y = body_work.ground_v_pos - 131072;
        obsObjectWork.pos.z = AppMain.GmBsCmnGetPlayerObj().pos.z;
        AppMain.gmBoss5EfctTargetCursorInitFlickerNoDisp(targ_cursor);
        AppMain.gmBoss5EfctCreateTargetCursorFlash(parent_efct, 24);
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctCrashCursorLoopProcMain);
    }

    public static void gmBoss5EfctCrashCursorLoopProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.gmBoss5EfctTargetCursorUpdateFlickerNoDisp((AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obj_work, 10f, 20f);
        if (((int)parentObj.flag & 2) != 0)
            return;
        AppMain.gmBoss5EfctCreateCrashCursorEnd(parentObj, (AppMain.GMS_EFFECT_COM_WORK)obj_work);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateCrashCursorEnd(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      AppMain.GMS_EFFECT_COM_WORK former_efct)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.gmBoss5EfctEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 22, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EFCT_GENERAL_WORK()));
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)(AppMain.GMS_EFFECT_COM_WORK)efct_3des;
        AppMain.GMS_BOSS5_EFCT_GENERAL_WORK s5EfctGeneralWork = (AppMain.GMS_BOSS5_EFCT_GENERAL_WORK)obsObjectWork;
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, 32f);
        obsObjectWork.pos.Assign(((AppMain.OBS_OBJECT_WORK)former_efct).pos);
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctCrashCursorEndProcMain);
    }

    public static void gmBoss5EfctCrashCursorEndProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctVulcanBulletProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.pos.x <= AppMain.GMM_BOSS5_AREA_LEFT() || obj_work.pos.y <= AppMain.GMM_BOSS5_AREA_TOP() || (obj_work.pos.x >= AppMain.GMM_BOSS5_AREA_RIGHT() || obj_work.pos.y >= AppMain.GMM_BOSS5_AREA_BOTTOM()))
            obj_work.flag &= 4294967279U;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateRocketSmoke(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)rkt_work), 3U);
        AppMain.GmEffect3DESSetDispOffset(efct_3des, 0.0f, 0.0f, 0.0f);
        AppMain.GmEffect3DESSetDispRotation(efct_3des, (short)AppMain.AKM_DEGtoA32(0), (short)0, (short)0);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctRocketSmokeProcLoop);
    }

    public static void gmBoss5EfctRocketSmokeProcLoop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)((AppMain.GMS_BOSS5_ROCKET_WORK)obj_work.parent_obj).flag & 64) != 0)
            return;
        AppMain.ObjDrawKillAction3DES(obj_work);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctRocketSmokeProcFade);
    }

    public static void gmBoss5EfctRocketSmokeProcFade(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctBreakdownSmokeProcLoop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctBreakdownSmokeProcFade);
        }
    }

    public static void gmBoss5EfctBreakdownSmokeProcFade(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    public static void gmBoss5EfctCreateBodySmallSmoke(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      uint part_idx)
    {
    }

    public static void gmBoss5EfctBodySmallSmokeProcLoop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.user_timer = AppMain.AkMathRandFx() * 60 + 60 >> 12;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctBodySmallSmokeProcFade);
        }
    }

    public static void gmBoss5EfctBodySmallSmokeProcFade(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.gmBoss5EfctCreateBodySmallSmoke(parentObj, obj_work.user_work);
            obj_work.flag |= 4U;
        }
    }

    public static void gmBoss5EfctCreateBerserkSteam(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      uint count,
      uint part_idx)
    {
        if (count == 0U)
            return;
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 89);
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)efct_3des;
        obsObjectWork.user_flag = part_idx;
        obsObjectWork.user_work = count - 1U;
        AppMain.GmEffect3DESSetDispOffset(efct_3des, AppMain.gm_boss5_efct_berserk_steam_disp_ofst_tbl[(int)part_idx][0], AppMain.gm_boss5_efct_berserk_steam_disp_ofst_tbl[(int)part_idx][1], AppMain.gm_boss5_efct_berserk_steam_disp_ofst_tbl[(int)part_idx][2]);
        AppMain.GmEffect3DESSetDispRotation(efct_3des, AppMain.gm_boss5_efct_berserk_steam_disp_rot_tbl[(int)part_idx][0], AppMain.gm_boss5_efct_berserk_steam_disp_rot_tbl[(int)part_idx][1], AppMain.gm_boss5_efct_berserk_steam_disp_rot_tbl[(int)part_idx][2]);
        obsObjectWork.user_timer = 6;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctBerserkSteamProcLoop);
    }

    public static void gmBoss5EfctBerserkSteamProcLoop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.user_timer = 0;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctBerserkSteamProcFade);
        }
    }

    public static void gmBoss5EfctBerserkSteamProcFade(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNodeRelative(obj_work, parentObj.snm_work, parentObj.body_snm_reg_id, 1, obj_work.parent_obj.pos, parentObj.pivot_prev_pos);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            if (obj_work.user_work != 0U)
                AppMain.gmBoss5EfctCreateBerserkSteam(parentObj, obj_work.user_work, obj_work.user_flag);
            obj_work.flag |= 4U;
        }
    }

    public static void gmBoss5EfctCreateEggSweat(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)egg_work), 93);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 56f, -8f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EfctEggSweatProcLoop);
    }

    public static void gmBoss5EfctEggSweatProcLoop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)((AppMain.GMS_BOSS5_EGG_WORK)obj_work.parent_obj).flag & (int)AppMain.GMD_BOSS5_EGG_FLAG_SWEAT_ACTIVE) != 0)
            return;
        AppMain.ObjDrawKillAction3DES(obj_work);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd);
    }

}