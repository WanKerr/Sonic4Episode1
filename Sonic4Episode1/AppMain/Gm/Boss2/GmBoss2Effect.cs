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
    private static void gmBoss2EffDamageInit(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 0U)).pos.z += 131072;
    }

    private static void gmBoss2EffBombsInit(
      AppMain.GMS_BOSS2_EFF_BOMB_WORK bomb_work,
      AppMain.OBS_OBJECT_WORK parent_obj,
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

    private static void gmBoss2EffBombsUpdate(AppMain.GMS_BOSS2_EFF_BOMB_WORK bomb_work)
    {
        if (bomb_work.interval_timer > 0U)
        {
            --bomb_work.interval_timer;
        }
        else
        {
            AppMain.GmSoundPlaySE("Boss0_02");
            AppMain.OBS_OBJECT_WORK obsObjectWork1 = AppMain.GMM_BS_OBJ((object)AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 7));
            AppMain.OBS_OBJECT_WORK obsObjectWork2 = AppMain.GMM_BS_OBJ((object)bomb_work.parent_obj);
            int v2_1 = bomb_work.area[0];
            int v2_2 = bomb_work.area[1];
            int num1 = AppMain.FX_Mul(AppMain.AkMathRandFx(), v2_1);
            int num2 = AppMain.FX_Mul(AppMain.AkMathRandFx(), v2_2);
            obsObjectWork1.pos.x = bomb_work.pos[0] - (v2_1 >> 1) + num1;
            obsObjectWork1.pos.y = bomb_work.pos[1] - (v2_2 >> 1) + num2;
            obsObjectWork1.pos.z = obsObjectWork2.pos.z + 131072;
            uint num3 = (uint)(AppMain.AkMathRandFx() * ((int)bomb_work.interval_max - (int)bomb_work.interval_min) >> 12);
            bomb_work.interval_timer = bomb_work.interval_min + num3;
        }
    }

    private static void gmBoss2EffAfterburnerRequestCreate(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.flag |= 33554432U;
    }

    private static void gmBoss2EffAfterburnerRequestDelete(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        body_work.flag &= 4294967293U;
        body_work.flag &= 4261412863U;
    }

    private static void gmBoss2EffAfterburnerInit(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 2) != 0)
            return;
        body_work.flag &= 4261412863U;
        body_work.flag |= 2U;
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 4U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -30f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffAfterburnerMainFunc);
    }

    private static void gmBoss2EffAfterburnerMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 2) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss2EffAfterburnerSmokeInit(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 5U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffAfterburnerSmokeMainFunc);
    }

    private static void gmBoss2EffAfterburnerSmokeMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss2EffBodySmokeInit(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 3U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffBodySmokeMainFunc);
    }

    private static void gmBoss2EffBodySmokeMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss2EffSweatInit(AppMain.GMS_BOSS2_EGG_WORK egg_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)egg_work), 93);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 32f, 0.0f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffSweatMainFunc);
        egg_work.flag |= 2U;
    }

    private static void gmBoss2EffSweatMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)((AppMain.GMS_BOSS2_EGG_WORK)obj_work.parent_obj).flag & 2) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static AppMain.OBS_OBJECT_WORK gmBoss2EffInit(
      AppMain.OBS_DATA_WORK data_work,
      int effect_type,
      AppMain.OBS_OBJECT_WORK parent_obj_work,
      short rot_x,
      short rot_y,
      short rot_z,
      float offset_x,
      float offset_y,
      float offset_z,
      int flag_flip,
      int flag_data_rotate)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DES_WORK()), (AppMain.OBS_OBJECT_WORK)null, (ushort)0, "B02_effect");
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = (AppMain.GMS_EFFECT_3DES_WORK)work;
        AppMain.ObjObjectAction3dESEffectLoad(work, efct_3des.obj_3des, data_work, (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjObjectAction3dESTextureLoad(work, efct_3des.obj_3des, AppMain.ObjDataGet(722), (string)null, 0, (AppMain.AMS_AMB_HEADER)null, false);
        AppMain.ObjObjectAction3dESTextureSetByDwork(work, AppMain.ObjDataGet(723));
        uint init_flag = 0;
        if (parent_obj_work != null)
        {
            work.parent_obj = parent_obj_work;
            work.pos.x = parent_obj_work.pos.x;
            work.pos.y = parent_obj_work.pos.y;
            work.pos.z = parent_obj_work.pos.z;
            init_flag |= 18U;
        }
        AppMain.GmEffect3DESSetDispRotation(efct_3des, rot_x, rot_y, rot_z);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, offset_x, offset_y, offset_z);
        if (flag_flip == 0)
            init_flag |= 1U;
        if (flag_data_rotate != 0)
            init_flag |= 64U;
        AppMain.GmEffect3DESSetupBase(efct_3des, (uint)effect_type, init_flag);
        return work;
    }

    private static void gmBoss2EffBallBombInit(
      AppMain.VecFx32 create_pos,
      AppMain.OBS_OBJECT_WORK body_obj_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work_parts = AppMain.gmBoss2EffInit(AppMain.ObjDataGet(717), 1, (AppMain.OBS_OBJECT_WORK)null, (short)0, (short)0, (short)0, 0.0f, 0.0f, 0.0f, 0, 0);
        obj_work_parts.pos.x = create_pos.x;
        obj_work_parts.pos.y = create_pos.y;
        obj_work_parts.pos.z = create_pos.z + 131072;
        AppMain.gmBoss2MgrAddObject(AppMain.gmBoss2MgrGetMgrWork(body_obj_work), obj_work_parts);
        AppMain.mtTaskChangeTcbDestructor(obj_work_parts.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss2EffectExitFunc));
    }

    private static AppMain.OBS_OBJECT_WORK gmBoss2EffBallBombPartInit(
      AppMain.VecFx32 create_pos,
      AppMain.OBS_OBJECT_WORK body_obj_work,
      int spd_x)
    {
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (AppMain.GMS_EFFECT_3DES_WORK)AppMain.gmBoss2EffInit(AppMain.ObjDataGet(718), 2, (AppMain.OBS_OBJECT_WORK)null, (short)0, (short)0, (short)0, 0.0f, 0.0f, 0.0f, 0, 0);
        AppMain.OBS_OBJECT_WORK objWork = gmsEffect3DesWork.efct_com.obj_work;
        AppMain.OBS_RECT_WORK[] rectWork = gmsEffect3DesWork.efct_com.rect_work;
        AppMain.GmBsCmnSetEfctAtkVsPly(gmsEffect3DesWork.efct_com, (short)64);
        AppMain.ObjRectWorkSet(rectWork[1], (short)-8, (short)-8, (short)8, (short)8);
        rectWork[1].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss2BallHitFunc);
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
        objWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffBallBombPartMainFunc);
        AppMain.gmBoss2MgrAddObject(AppMain.gmBoss2MgrGetMgrWork(body_obj_work), objWork);
        AppMain.mtTaskChangeTcbDestructor(objWork.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss2EffectExitFunc));
        return objWork;
    }

    private static void gmBoss2EffBallBombPartMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (AppMain.ObjViewOutCheck(obj_work.pos.x, obj_work.pos.y, (short)64, (short)0, (short)0, (short)0, (short)0) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmBoss2EffBlitzInit(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK obj_work_parts1 = AppMain.gmBoss2EffInit(AppMain.ObjDataGet(714), 2, obsObjectWork, (short)0, (short)0, (short)0, 24f, 0.0f, 0.0f, 0, 0);
        obj_work_parts1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffBlitzMainFuncBlitzCoreL);
        AppMain.OBS_OBJECT_WORK obj_work_parts2 = AppMain.gmBoss2EffInit(AppMain.ObjDataGet(714), 2, obsObjectWork, (short)0, (short)0, (short)0, -24f, 0.0f, 0.0f, 0, 0);
        obj_work_parts2.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffBlitzMainFuncBlitzCoreR);
        AppMain.OBS_OBJECT_WORK obj_work_parts3 = AppMain.gmBoss2EffInit(AppMain.ObjDataGet(715), 2, obsObjectWork, (short)0, (short)0, AppMain.GMD_BOSS2_EFFECT_BLITZ_LINE_DISP_ROT_Z, 0.0f, -30f, 0.0f, 0, 0);
        obj_work_parts3.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffBlitzMainFuncBlitzLineCreate);
        body_work.flag |= 4U;
        AppMain.GMS_BOSS2_MGR_WORK mgrWork = AppMain.gmBoss2MgrGetMgrWork(obsObjectWork);
        AppMain.gmBoss2MgrAddObject(mgrWork, obj_work_parts1);
        AppMain.gmBoss2MgrAddObject(mgrWork, obj_work_parts2);
        AppMain.gmBoss2MgrAddObject(mgrWork, obj_work_parts3);
        AppMain.mtTaskChangeTcbDestructor(obj_work_parts1.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss2EffectExitFunc));
        AppMain.mtTaskChangeTcbDestructor(obj_work_parts2.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss2EffectExitFunc));
        AppMain.mtTaskChangeTcbDestructor(obj_work_parts3.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss2EffectExitFunc));
    }

    private static void gmBoss2EffBlitzMainFuncBlitzLineCreate(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 4) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[2], 1);
        ++obj_work.user_timer;
        if ((double)obj_work.user_timer < 64.0)
            return;
        obj_work.user_timer = 0;
        AppMain.GmEffect3DESAddDispOffset((AppMain.GMS_EFFECT_3DES_WORK)obj_work, 0.0f, -4f, 0.0f);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffBlitzMainFuncBlitzLineNormal);
    }

    private static void gmBoss2EffBlitzMainFuncBlitzLineNormal(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 4) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[2], 1);
    }

    private static void gmBoss2EffBlitzMainFuncBlitzCoreL(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 4) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[7], 1);
    }

    private static void gmBoss2EffBlitzMainFuncBlitzCoreR(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)parentObj.flag & 4) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[10], 1);
    }

    private static void gmBoss2EffBlitzMainFuncBlitzL(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)parentObj.flag & 4) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[6], 1);
    }

    private static void gmBoss2EffBlitzMainFuncBlitzR(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 4) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (obj_work.parent_obj != null)
            obj_work.dir.z = obj_work.parent_obj.dir.z;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[9], 1);
    }

    private static void gmBoss2EffScatterInit(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK parent_obj = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GMS_BOSS2_EFFECT_SCATTER_WORK effectScatterWork = (AppMain.GMS_BOSS2_EFFECT_SCATTER_WORK)null;
        for (int index = 3; 13 > index; ++index)
        {
            AppMain.GMS_BOSS2_EFFECT_SCATTER_WORK controlObjectBySize = (AppMain.GMS_BOSS2_EFFECT_SCATTER_WORK)AppMain.GmBsCmnCreateNodeControlObjectBySize(parent_obj, body_work.cnm_mgr_work, body_work.cnm_reg_id[index], body_work.snm_work, body_work.snm_reg_id[2 + index], (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS2_EFFECT_SCATTER_WORK()));
            AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj = (AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)controlObjectBySize;
            AppMain.GmBsCmnChangeCNMModeNode(body_work.cnm_mgr_work, body_work.cnm_reg_id[index], 0U);
            AppMain.GmBsCmnEnableCNMLocalCoordinate(body_work.cnm_mgr_work, body_work.cnm_reg_id[index], 0);
            AppMain.GmBsCmnAttachNCObjectToSNMNode(ndc_obj);
            ndc_obj.is_enable = 1;
            ndc_obj.proc_update = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffScatterMainFunc);
            AppMain.OBS_OBJECT_WORK objWork1 = ndc_obj.efct_com.obj_work;
            objWork1.move_flag |= 128U;
            if (index == 4 || index == 5 || (index == 7 || index == 8))
            {
                AppMain.OBS_OBJECT_WORK objWork2 = effectScatterWork.control_node_work.efct_com.obj_work;
                objWork1.spd.x = objWork2.spd.x;
                objWork1.spd.y = objWork2.spd.y;
            }
            else
            {
                int right_flag = 0;
                if (index % 2 != 0)
                    right_flag = 1;
                AppMain.gmBoss2EffScatterSetParamMove(objWork1, right_flag);
            }
            effectScatterWork = controlObjectBySize;
        }
    }

    private static void gmBoss2EffScatterMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GmBsCmnSetWorldMtxFromNCObjectPosture((AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)obj_work);
        ++obj_work.user_timer;
        if (obj_work.user_timer < 100)
            return;
        obj_work.user_timer = 0;
        obj_work.flag |= 4U;
    }

    private static void gmBoss2EffScatterSetParamMove(
      AppMain.OBS_OBJECT_WORK obj_work,
      int right_flag)
    {
        int num = (int)AppMain.mtMathRand() % 30 + 45;
        if (right_flag != 0)
            num = -num;
        int ang = AppMain.AKM_DEGtoA32(num + 90);
        obj_work.spd.y = -(int)(4096.0 * (double)AppMain.nnSin(ang));
        obj_work.spd.x = (int)(4096.0 * (double)AppMain.nnCos(ang));
    }

    private static void gmBoss2EffCreateRollModel(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (((int)body_work.flag & 32) != 0)
            return;
        body_work.flag |= 32U;
        AppMain.OBS_OBJECT_WORK byParam = (AppMain.OBS_OBJECT_WORK)AppMain.GmEffect3dESCreateByParam(new AppMain.GMS_EFFECT_CREATE_PARAM(11, 0U, 19U, new AppMain.NNS_VECTOR(0.0f, 0.0f, 64f), new AppMain.NNS_ROTATE_A16((short)0, ((int)obsObjectWork.disp_flag & 1) == 0 ? (short)AppMain.AKM_DEGtoA32(90f) : (short)AppMain.AKM_DEGtoA32(-90f), (short)0), 3.2f, new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd), 5), obsObjectWork, (object)AppMain.GmBoss2GetGameDatEnemyArc(), AppMain.ObjDataGet(719), AppMain.ObjDataGet(726), AppMain.ObjDataGet(727), AppMain.ObjDataGet(725), AppMain.ObjDataGet(724), (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DES_WORK()));
        byParam.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss2EffRollModelMainFunc);
        byParam.obj_3des.command_state = 16U;
        AppMain.gmBoss2MgrAddObject(AppMain.gmBoss2MgrGetMgrWork(obsObjectWork), byParam);
        AppMain.mtTaskChangeTcbDestructor(byParam.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss2EffectExitFunc));
    }

    private static void gmBoss2EffCreateRollModelLost(AppMain.GMS_BOSS2_BODY_WORK body_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) != 0)
            AppMain.AKM_DEGtoA32(-90f);
        else
            AppMain.AKM_DEGtoA32(90f);
    }

    public static void gmBoss2EffRollModelMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        int num = (int)obj_work.disp_flag & 8;
        if (((int)parentObj.flag & 32) != 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmBoss2EffRollMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS2_BODY_WORK parentObj = (AppMain.GMS_BOSS2_BODY_WORK)obj_work.parent_obj;
        int num = (int)obj_work.disp_flag & 8;
        if (((int)parentObj.flag & 32) != 0)
            return;
        obj_work.flag |= 4U;
    }

}