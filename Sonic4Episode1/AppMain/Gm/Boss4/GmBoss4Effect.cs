using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    private static void GmBoss4EffectBuild()
    {
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(735), 5, AppMain.GMD_BOSS4_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(736), 6, AppMain.GMD_BOSS4_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(737), 7, AppMain.GMD_BOSS4_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(738), 8, AppMain.GMD_BOSS4_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(739), 9, AppMain.GMD_BOSS4_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(740), 10, AppMain.GMD_BOSS4_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(741), 11, AppMain.GMD_BOSS4_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(742), 12, AppMain.GMD_BOSS4_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(743), 13, AppMain.GMD_BOSS4_ARC);
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(744), 14, AppMain.GMD_BOSS4_ARC);
        for (int index = 0; index < 9; ++index)
            AppMain.GmEfctBossBuildSingleDataReg(15, AppMain.ObjDataGet(733), AppMain.ObjDataGet(734), 0, (AppMain.OBS_DATA_WORK)null, (AppMain.OBS_DATA_WORK)null, AppMain.GMD_BOSS4_ARC);
    }

    private static void GmBoss4EffectFlush()
    {
        AppMain.GmEfctBossFlushSingleDataInit();
        AppMain.ObjDataRelease(AppMain.ObjDataGet(735));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(736));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(737));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(738));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(739));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(740));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(741));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(742));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(743));
        AppMain.ObjDataRelease(AppMain.ObjDataGet(744));
    }

    private static void GmBoss4EffChangeType(
      AppMain.GMS_EFFECT_3DES_WORK efct_work,
      uint type,
      uint init_flag)
    {
        efct_work.efct_com.obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.GmEffect3DESSetupBase(efct_work, type, init_flag);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffMainFuncDeleteAtEnd);
    }

    private static AppMain.GMS_EFFECT_3DES_WORK GmBoss4EffCommonInit(
      int id,
      AppMain.VecFx32? pos)
    {
        return AppMain.GmBoss4EffCommonInit(id, pos, (AppMain.OBS_OBJECT_WORK)null, 2U, 1U, (AppMain.GMS_BOSS4_NODE_MATRIX)null, -1, new AppMain.VecFx32?(), (uint[])null, 0U);
    }

    private static AppMain.GMS_EFFECT_3DES_WORK GmBoss4EffCommonInit(
      int id,
      AppMain.VecFx32? pos,
      AppMain.OBS_OBJECT_WORK parent_obj)
    {
        return AppMain.GmBoss4EffCommonInit(id, pos, parent_obj, 2U, 1U, (AppMain.GMS_BOSS4_NODE_MATRIX)null, -1, new AppMain.VecFx32?(), (uint[])null, 0U);
    }

    private static AppMain.GMS_EFFECT_3DES_WORK GmBoss4EffCommonInit(
      int id,
      AppMain.VecFx32? pos,
      AppMain.OBS_OBJECT_WORK parent_obj,
      uint type,
      uint flag,
      AppMain.GMS_BOSS4_NODE_MATRIX mtx,
      int link,
      AppMain.VecFx32? rot,
      uint[] ctrl_flag,
      uint mask)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS4_EFF_COMMON_WORK()), parent_obj, (ushort)0, "B04_CapOver");
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = (AppMain.GMS_EFFECT_3DES_WORK)work;
        AppMain.GMS_BOSS4_EFF_COMMON_WORK bosS4EffCommonWork = (AppMain.GMS_BOSS4_EFF_COMMON_WORK)work;
        AppMain.ObjObjectAction3dESEffectLoad(AppMain.GMM_BS_OBJ((object)efct_3des), efct_3des.obj_3des, AppMain.ObjDataGet(id), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjObjectAction3dESTextureLoad(AppMain.GMM_BS_OBJ((object)efct_3des), efct_3des.obj_3des, AppMain.ObjDataGet(733), (string)null, 0, (AppMain.AMS_AMB_HEADER)null, false);
        AppMain.ObjObjectAction3dESTextureSetByDwork(work, AppMain.ObjDataGet(734));
        AppMain.GmEffect3DESSetupBase(efct_3des, type, flag);
        if (pos.HasValue)
            AppMain.VEC_Set(ref work.pos, pos.Value.x, pos.Value.y, pos.Value.z);
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffMainFuncFlagLink);
        work.flag |= 32U;
        bosS4EffCommonWork.lookflag = ctrl_flag;
        bosS4EffCommonWork.lookmask = mask;
        if (bosS4EffCommonWork.lookflag != null)
            bosS4EffCommonWork.lookflag[0] |= bosS4EffCommonWork.lookmask;
        bosS4EffCommonWork.link = -1;
        if (link >= 0)
        {
            bosS4EffCommonWork.link = link;
            bosS4EffCommonWork.node_work = mtx;
            if (rot.HasValue)
                AppMain.GmEffect3DESSetDispRotation(efct_3des, AppMain.AKM_DEGtoA16(AppMain.FX_FX32_TO_F32(rot.Value.x)), AppMain.AKM_DEGtoA16(AppMain.FX_FX32_TO_F32(rot.Value.y)), AppMain.AKM_DEGtoA16(AppMain.FX_FX32_TO_F32(rot.Value.z)));
            if (pos.HasValue)
                AppMain.GmEffect3DESAddDispOffset(efct_3des, AppMain.FX_FX32_TO_F32(pos.Value.x), AppMain.FX_FX32_TO_F32(pos.Value.y), AppMain.FX_FX32_TO_F32(pos.Value.z));
        }
        return efct_3des;
    }

    private static void GmBoss4EffBombInitCreate(
      AppMain.GMS_BOSS4_EFF_BOMB_WORK bomb_work,
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

    private static void GmBoss4EffBombUpdateCreate(AppMain.GMS_BOSS4_EFF_BOMB_WORK bomb_work)
    {
        AppMain.MTM_ASSERT((object)bomb_work.parent_obj);
        bomb_work.pos[0] += AppMain.GmBoss4GetScrollOffset();
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
            AppMain.GMS_EFFECT_3DES_WORK efct_3des;
            switch (bomb_work.bomb_type)
            {
                case 0:
                    efct_3des = AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 7);
                    efct_3des.efct_com.obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
                    AppMain.GmEffect3DESSetupBase(efct_3des, 2U, 1U);
                    efct_3des.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffMainFuncDeleteAtEnd);
                    if (--bomb_work.interval_timer_sound <= 0)
                    {
                        bomb_work.interval_timer_sound = 3;
                        AppMain.GmSoundPlaySE("Boss0_02");
                        break;
                    }
                    break;
                case 5:
                    efct_3des = AppMain.GmBoss4EffCommonInit(743, new AppMain.VecFx32?());
                    efct_3des.efct_com.obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
                    AppMain.GmEffect3DESSetupBase(efct_3des, 2U, 64U);
                    efct_3des.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffMainFuncDeleteAtEnd);
                    break;
                default:
                    AppMain.MTM_ASSERT(false);
                    return;
            }
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)efct_3des);
            AppMain.MTM_ASSERT((object)obsObjectWork);
            obsObjectWork.pos.x = bomb_work.pos[0] - (v2_1 >> 1) + num1;
            obsObjectWork.pos.y = bomb_work.pos[1] - (v2_2 >> 1) + num2;
            obsObjectWork.pos.z = AppMain.GMM_BS_OBJ((object)bomb_work.parent_obj).pos.z + 131072;
            uint num3 = (uint)((long)AppMain.AkMathRandFx() * (long)(bomb_work.interval_max - bomb_work.interval_max) >> 12);
            bomb_work.interval_timer = bomb_work.interval_min + num3;
        }
    }

    private static void gmBoss4EffMainFuncFlagLink(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_EFF_COMMON_WORK bosS4EffCommonWork = (AppMain.GMS_BOSS4_EFF_COMMON_WORK)obj_work;
        obj_work.disp_flag &= 4294963199U;
        if (((int)AppMain.g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        else
            obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
        if (((int)obj_work.disp_flag & 8) != 0)
        {
            obj_work.flag |= 4U;
            if (bosS4EffCommonWork.lookflag != null)
                bosS4EffCommonWork.lookflag[0] &= ~bosS4EffCommonWork.lookmask;
        }
        if (bosS4EffCommonWork.lookflag != null && ((int)bosS4EffCommonWork.lookflag[0] & (int)bosS4EffCommonWork.lookmask) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (bosS4EffCommonWork.link < 0)
            return;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, bosS4EffCommonWork.node_work.snm_work, bosS4EffCommonWork.node_work.work[bosS4EffCommonWork.link], 1);
    }

    private static void gmBoss4EffMainFuncDeleteAtEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmBoss4EffDamageInit(object body_work)
    {
        AppMain.GMM_BS_OBJ((object)AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ(body_work), 0U)).pos.z += 131072;
    }


    private static void gmBoss4EffSweatInit(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)egg_work), 93);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 32f, 0.0f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffSweatProcMain);
        egg_work.flag |= 2U;
    }

    private static void gmBoss4EffSweatProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_EGG_WORK parentObj = (AppMain.GMS_BOSS4_EGG_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((object)parentObj);
        if (((int)parentObj.flag & 2) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmBoss4EffAfterburnerSetEnable(
     AppMain.GMS_BOSS4_BODY_WORK body_work,
     int type)
    {
        switch (type)
        {
            case 1:
                AppMain.MTM_ASSERT(0 == ((int)body_work.flag[0] & 16));
                AppMain.MTM_ASSERT(0 == ((int)body_work.flag[0] & 33554432));
                body_work.flag[0] &= 4294967039U;
                body_work.flag[0] &= 4293918719U;
                body_work.flag[0] |= 33554432U;
                break;
            case 2:
                AppMain.MTM_ASSERT(0 == ((int)body_work.flag[0] & 256));
                AppMain.MTM_ASSERT(0 == ((int)body_work.flag[0] & 1048576));
                body_work.flag[0] &= 4294967279U;
                body_work.flag[0] &= 4261412863U;
                body_work.flag[0] |= 1048576U;
                body_work.flag[0] &= 4294966271U;
                break;
            default:
                body_work.flag[0] &= 4294967279U;
                body_work.flag[0] &= 4261412863U;
                body_work.flag[0] &= 4294967039U;
                body_work.flag[0] &= 4293918719U;
                break;
        }
    }

    private static void gmBoss4EffAfterburnerUpdateCreate(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (((int)body_work.flag[0] & 1048576) != 0)
        {
            body_work.flag[0] &= 4261412863U;
            body_work.flag[0] &= 4293918719U;
            body_work.flag[0] |= 256U;
            AppMain.gmBoss4EffAfterburnerExInit(body_work);
        }
        if (((int)body_work.flag[0] & 33554432) == 0)
            return;
        body_work.flag[0] &= 4261412863U;
        body_work.flag[0] &= 4293918719U;
        body_work.flag[0] |= 16U;
        AppMain.gmBoss4EffAfterburnerInit(body_work);
    }

    private static void gmBoss4EffAfterburnerInit(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK parent_obj = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GMS_EFFECT_3DES_WORK efct_3des1 = AppMain.GmEfctBossCmnEsCreate(parent_obj, 4U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des1, 0.0f, -15f, -45f);
        AppMain.GMM_BS_OBJ((object)efct_3des1).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffAfterburnerProcMain);
        AppMain.GMS_EFFECT_3DES_WORK efct_3des2 = AppMain.GmEfctBossCmnEsCreate(parent_obj, 4U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des2, 0.0f, 5f, -45f);
        AppMain.GMM_BS_OBJ((object)efct_3des2).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffAfterburnerProcMain);
    }

    private static void gmBoss4EffAfterburnerProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK parentObj = (AppMain.GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((int)parentObj.node_work.snm_work.reg_node_max);
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag[0] & 16) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.node_work.snm_work, parentObj.node_work.work[2], 1);
    }

    private static void gmBoss4EffAfterburnerExInit(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK parent_obj = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GMS_EFFECT_3DES_WORK efct_3des1 = AppMain.GmBoss4EffCommonInit(741, new AppMain.VecFx32?(), parent_obj);
        AppMain.GMM_BS_OBJ((object)efct_3des1).ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.GmEffect3DESSetupBase(efct_3des1, 2U, 2U);
        AppMain.GmEffect3DESSetDispRotation(efct_3des1, (short)AppMain.GMD_BOSS4_EFF_ABURNER3_DISP_ROT_X, (short)0, (short)0);
        AppMain.GmEffect3DESAddDispOffset(efct_3des1, -0.0f, -0.0f, 0.0f);
        AppMain.GMM_BS_OBJ((object)efct_3des1).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffAfterburnerExProcMainL);
        AppMain.GMS_EFFECT_3DES_WORK efct_3des2 = AppMain.GmBoss4EffCommonInit(741, new AppMain.VecFx32?(), parent_obj);
        AppMain.GMM_BS_OBJ((object)efct_3des2).ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.GmEffect3DESSetupBase(efct_3des2, 2U, 2U);
        AppMain.GmEffect3DESSetDispRotation(efct_3des2, (short)AppMain.GMD_BOSS4_EFF_ABURNER3_DISP_ROT_X, (short)0, (short)0);
        AppMain.GmEffect3DESAddDispOffset(efct_3des2, 0.0f, -0.0f, 0.0f);
        AppMain.GMM_BS_OBJ((object)efct_3des2).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffAfterburnerExProcMainR);
    }

    private static void gmBoss4EffAfterburnerExProcMainL(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK parentObj = (AppMain.GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((int)parentObj.node_work.snm_work.reg_node_max);
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag[0] & 256) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        AppMain.NNS_MATRIX nodeMatrix1 = AppMain.GmBoss4UtilGetNodeMatrix(parentObj.node_work, 5);
        AppMain.NNS_MATRIX nodeMatrix2 = AppMain.GmBoss4UtilGetNodeMatrix(parentObj.node_work, 2);
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnCopyMatrix(nnsMatrix, nodeMatrix1);
        nnsMatrix.M03 = (float)((double)nodeMatrix1.M03 - (double)nodeMatrix2.M03 + (double)parentObj.ene_3d.ene_com.obj_work.pos.x / 4096.0);
        AppMain.GmBoss4UtilSetMatrixES(obj_work, nnsMatrix);
        obj_work.disp_flag &= 4294963199U;
        if (((int)AppMain.g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss4EffAfterburnerExProcMainR(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK parentObj = (AppMain.GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((int)parentObj.node_work.snm_work.reg_node_max);
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag[0] & 256) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        AppMain.NNS_MATRIX nodeMatrix1 = AppMain.GmBoss4UtilGetNodeMatrix(parentObj.node_work, 8);
        AppMain.NNS_MATRIX nodeMatrix2 = AppMain.GmBoss4UtilGetNodeMatrix(parentObj.node_work, 2);
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnCopyMatrix(nnsMatrix, nodeMatrix1);
        nnsMatrix.M03 = (float)((double)nodeMatrix1.M03 - (double)nodeMatrix2.M03 + (double)parentObj.ene_3d.ene_com.obj_work.pos.x / 4096.0);
        AppMain.GmBoss4UtilSetMatrixES(obj_work, nnsMatrix);
        obj_work.disp_flag &= 4294963199U;
        if (((int)AppMain.g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss4EffABSmokeInit(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 5U);
        AppMain.GmBoss4EffChangeType(gmsEffect3DesWork, 2U, 19U);
        AppMain.GmEffect3DESAddDispOffset(gmsEffect3DesWork, 0.0f, 0.0f, -32f);
        AppMain.GMM_BS_OBJ((object)gmsEffect3DesWork).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffABSmokeProcMain);
    }

    private static void gmBoss4EffABSmokeProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK parentObj = (AppMain.GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((int)parentObj.node_work.snm_work.reg_node_max);
        obj_work.disp_flag &= 4294963199U;
        if (((int)AppMain.g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        else
            obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.node_work.snm_work, parentObj.node_work.work[2], 1);
    }

    private static void gmBoss4EffBodySmokeInit(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK parent_obj = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork1 = AppMain.GmEfctBossCmnEsCreate(parent_obj, 3U);
        AppMain.GmBoss4EffChangeType(gmsEffect3DesWork1, 2U, 19U);
        AppMain.GmEffect3DESAddDispOffset(gmsEffect3DesWork1, 0.0f, 0.0f, -32f);
        AppMain.GMM_BS_OBJ((object)gmsEffect3DesWork1).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffBodySmokeProcMain);
        float[][] numArray = new float[4][]
        {
      new float[3]{ -36f, 0.0f, -6f },
      new float[3]{ -20f, 6f, 16f },
      new float[3]{ 0.0f, 8f, -24f },
      new float[3]{ 36f, 0.0f, 0.0f }
        };
        for (int index = 0; index < 4; ++index)
        {
            AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork2 = AppMain.GmEfctBossCmnEsCreate(parent_obj, 2U);
            AppMain.GmBoss4EffChangeType(gmsEffect3DesWork2, 2U, 19U);
            AppMain.GmEffect3DESAddDispOffset(gmsEffect3DesWork2, numArray[index][0], numArray[index][1], numArray[index][2]);
            AppMain.GMM_BS_OBJ((object)gmsEffect3DesWork2).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EffBodySmokeProcMain);
        }
    }

    private static void gmBoss4EffBodySmokeProcMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK parentObj = (AppMain.GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT((int)parentObj.node_work.snm_work.reg_node_max);
        obj_work.disp_flag &= 4294963199U;
        if (((int)AppMain.g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        else
            obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.node_work.snm_work, parentObj.node_work.work[2], 1);
    }

    private static void gmBoss4EffBossLightUpdateCreate(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (((int)body_work.flag[0] & 524288) == 0)
            return;
        body_work.flag[0] &= 4294443007U;
        AppMain.VecFx32 vecFx32_1 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f));
        AppMain.VecFx32 vecFx32_2 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f));
        AppMain.GmBoss4EffCommonInit(744, new AppMain.VecFx32?(vecFx32_1), (AppMain.OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 9, new AppMain.VecFx32?(vecFx32_2), body_work.flag, 512U);
        AppMain.VecFx32 vecFx32_3 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f));
        AppMain.VecFx32 vecFx32_4 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f));
        AppMain.GmBoss4EffCommonInit(744, new AppMain.VecFx32?(vecFx32_3), (AppMain.OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 10, new AppMain.VecFx32?(vecFx32_4), body_work.flag, 512U);
    }

    private static void gmBoss4EffBossLightSetEnable(AppMain.GMS_BOSS4_BODY_WORK body_work, bool on)
    {
        if (on)
        {
            if (((int)body_work.flag[0] & 512) != 0)
                return;
            body_work.flag[0] |= 524288U;
        }
        else
        {
            body_work.flag[0] &= 4294966783U;
            body_work.flag[0] &= 4294443007U;
        }
    }

}