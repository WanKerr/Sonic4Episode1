public partial class AppMain
{
    private static void GmBoss4EffectBuild()
    {
        ObjDataLoadAmbIndex(ObjDataGet(735), 5, GMD_BOSS4_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(736), 6, GMD_BOSS4_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(737), 7, GMD_BOSS4_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(738), 8, GMD_BOSS4_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(739), 9, GMD_BOSS4_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(740), 10, GMD_BOSS4_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(741), 11, GMD_BOSS4_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(742), 12, GMD_BOSS4_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(743), 13, GMD_BOSS4_ARC);
        ObjDataLoadAmbIndex(ObjDataGet(744), 14, GMD_BOSS4_ARC);
        for (int index = 0; index < 9; ++index)
            GmEfctBossBuildSingleDataReg(15, ObjDataGet(733), ObjDataGet(734), 0, null, null, GMD_BOSS4_ARC);
    }

    private static void GmBoss4EffectFlush()
    {
        GmEfctBossFlushSingleDataInit();
        ObjDataRelease(ObjDataGet(735));
        ObjDataRelease(ObjDataGet(736));
        ObjDataRelease(ObjDataGet(737));
        ObjDataRelease(ObjDataGet(738));
        ObjDataRelease(ObjDataGet(739));
        ObjDataRelease(ObjDataGet(740));
        ObjDataRelease(ObjDataGet(741));
        ObjDataRelease(ObjDataGet(742));
        ObjDataRelease(ObjDataGet(743));
        ObjDataRelease(ObjDataGet(744));
    }

    private static void GmBoss4EffChangeType(
      GMS_EFFECT_3DES_WORK efct_work,
      uint type,
      uint init_flag)
    {
        efct_work.efct_com.obj_work.ppFunc = null;
        GmEffect3DESSetupBase(efct_work, type, init_flag);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffMainFuncDeleteAtEnd);
    }

    private static GMS_EFFECT_3DES_WORK GmBoss4EffCommonInit(
      int id,
      VecFx32? pos)
    {
        return GmBoss4EffCommonInit(id, pos, null, 2U, 1U, null, -1, new VecFx32?(), null, 0U);
    }

    private static GMS_EFFECT_3DES_WORK GmBoss4EffCommonInit(
      int id,
      VecFx32? pos,
      OBS_OBJECT_WORK parent_obj)
    {
        return GmBoss4EffCommonInit(id, pos, parent_obj, 2U, 1U, null, -1, new VecFx32?(), null, 0U);
    }

    private static GMS_EFFECT_3DES_WORK GmBoss4EffCommonInit(
      int id,
      VecFx32? pos,
      OBS_OBJECT_WORK parent_obj,
      uint type,
      uint flag,
      GMS_BOSS4_NODE_MATRIX mtx,
      int link,
      VecFx32? rot,
      uint[] ctrl_flag,
      uint mask)
    {
        OBS_OBJECT_WORK work = GMM_EFFECT_CREATE_WORK(() => new GMS_BOSS4_EFF_COMMON_WORK(), parent_obj, 0, "B04_CapOver");
        GMS_EFFECT_3DES_WORK efct_3des = (GMS_EFFECT_3DES_WORK)work;
        GMS_BOSS4_EFF_COMMON_WORK bosS4EffCommonWork = (GMS_BOSS4_EFF_COMMON_WORK)work;
        ObjObjectAction3dESEffectLoad(GMM_BS_OBJ(efct_3des), efct_3des.obj_3des, ObjDataGet(id), null, 0, null);
        ObjObjectAction3dESTextureLoad(GMM_BS_OBJ(efct_3des), efct_3des.obj_3des, ObjDataGet(733), null, 0, null, false);
        ObjObjectAction3dESTextureSetByDwork(work, ObjDataGet(734));
        GmEffect3DESSetupBase(efct_3des, type, flag);
        if (pos.HasValue)
            VEC_Set(ref work.pos, pos.Value.x, pos.Value.y, pos.Value.z);
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffMainFuncFlagLink);
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
                GmEffect3DESSetDispRotation(efct_3des, AKM_DEGtoA16(FX_FX32_TO_F32(rot.Value.x)), AKM_DEGtoA16(FX_FX32_TO_F32(rot.Value.y)), AKM_DEGtoA16(FX_FX32_TO_F32(rot.Value.z)));
            if (pos.HasValue)
                GmEffect3DESAddDispOffset(efct_3des, FX_FX32_TO_F32(pos.Value.x), FX_FX32_TO_F32(pos.Value.y), FX_FX32_TO_F32(pos.Value.z));
        }
        return efct_3des;
    }

    private static void GmBoss4EffBombInitCreate(
      GMS_BOSS4_EFF_BOMB_WORK bomb_work,
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

    private static void GmBoss4EffBombUpdateCreate(GMS_BOSS4_EFF_BOMB_WORK bomb_work)
    {
        MTM_ASSERT(bomb_work.parent_obj);
        bomb_work.pos[0] += GmBoss4GetScrollOffset();
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
            GMS_EFFECT_3DES_WORK efct_3des;
            switch (bomb_work.bomb_type)
            {
                case 0:
                    efct_3des = GmEfctCmnEsCreate(null, 7);
                    efct_3des.efct_com.obj_work.ppFunc = null;
                    GmEffect3DESSetupBase(efct_3des, 2U, 1U);
                    efct_3des.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffMainFuncDeleteAtEnd);
                    if (--bomb_work.interval_timer_sound <= 0)
                    {
                        bomb_work.interval_timer_sound = 3;
                        GmSoundPlaySE("Boss0_02");
                        break;
                    }
                    break;
                case 5:
                    efct_3des = GmBoss4EffCommonInit(743, new VecFx32?());
                    efct_3des.efct_com.obj_work.ppFunc = null;
                    GmEffect3DESSetupBase(efct_3des, 2U, 64U);
                    efct_3des.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffMainFuncDeleteAtEnd);
                    break;
                default:
                    MTM_ASSERT(false);
                    return;
            }
            OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(efct_3des);
            MTM_ASSERT(obsObjectWork);
            obsObjectWork.pos.x = bomb_work.pos[0] - (v2_1 >> 1) + num1;
            obsObjectWork.pos.y = bomb_work.pos[1] - (v2_2 >> 1) + num2;
            obsObjectWork.pos.z = GMM_BS_OBJ(bomb_work.parent_obj).pos.z + 131072;
            uint num3 = (uint)(AkMathRandFx() * (bomb_work.interval_max - bomb_work.interval_max) >> 12);
            bomb_work.interval_timer = bomb_work.interval_min + num3;
        }
    }

    private static void gmBoss4EffMainFuncFlagLink(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_EFF_COMMON_WORK bosS4EffCommonWork = (GMS_BOSS4_EFF_COMMON_WORK)obj_work;
        obj_work.disp_flag &= 4294963199U;
        if (((int)g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        else
            obj_work.pos.x += GmBoss4GetScrollOffset();
        if (((int)obj_work.disp_flag & 8) != 0)
        {
            obj_work.flag |= 4U;
            if (bosS4EffCommonWork.lookflag != null)
                bosS4EffCommonWork.lookflag[0] &= ~bosS4EffCommonWork.lookmask;
        }
        if (bosS4EffCommonWork.lookflag != null && ((int)bosS4EffCommonWork.lookflag[0] & (int)bosS4EffCommonWork.lookmask) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (bosS4EffCommonWork.link < 0)
            return;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, bosS4EffCommonWork.node_work.snm_work, bosS4EffCommonWork.node_work.work[bosS4EffCommonWork.link], 1);
    }

    private static void gmBoss4EffMainFuncDeleteAtEnd(OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x += GmBoss4GetScrollOffset();
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmBoss4EffDamageInit(object body_work)
    {
        GMM_BS_OBJ(GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 0U)).pos.z += 131072;
    }


    private static void gmBoss4EffSweatInit(GMS_BOSS4_EGG_WORK egg_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctCmnEsCreate(GMM_BS_OBJ(egg_work), 93);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 32f, 0.0f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffSweatProcMain);
        egg_work.flag |= 2U;
    }

    private static void gmBoss4EffSweatProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_EGG_WORK parentObj = (GMS_BOSS4_EGG_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj);
        if (((int)parentObj.flag & 2) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

    private static void gmBoss4EffAfterburnerSetEnable(
     GMS_BOSS4_BODY_WORK body_work,
     int type)
    {
        switch (type)
        {
            case 1:
                MTM_ASSERT(0 == ((int)body_work.flag[0] & 16));
                MTM_ASSERT(0 == ((int)body_work.flag[0] & 33554432));
                body_work.flag[0] &= 4294967039U;
                body_work.flag[0] &= 4293918719U;
                body_work.flag[0] |= 33554432U;
                break;
            case 2:
                MTM_ASSERT(0 == ((int)body_work.flag[0] & 256));
                MTM_ASSERT(0 == ((int)body_work.flag[0] & 1048576));
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

    private static void gmBoss4EffAfterburnerUpdateCreate(GMS_BOSS4_BODY_WORK body_work)
    {
        if (((int)body_work.flag[0] & 1048576) != 0)
        {
            body_work.flag[0] &= 4261412863U;
            body_work.flag[0] &= 4293918719U;
            body_work.flag[0] |= 256U;
            gmBoss4EffAfterburnerExInit(body_work);
        }
        if (((int)body_work.flag[0] & 33554432) == 0)
            return;
        body_work.flag[0] &= 4261412863U;
        body_work.flag[0] &= 4293918719U;
        body_work.flag[0] |= 16U;
        gmBoss4EffAfterburnerInit(body_work);
    }

    private static void gmBoss4EffAfterburnerInit(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK parent_obj = GMM_BS_OBJ(body_work);
        GMS_EFFECT_3DES_WORK efct_3des1 = GmEfctBossCmnEsCreate(parent_obj, 4U);
        GmEffect3DESAddDispOffset(efct_3des1, 0.0f, -15f, -45f);
        GMM_BS_OBJ(efct_3des1).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffAfterburnerProcMain);
        GMS_EFFECT_3DES_WORK efct_3des2 = GmEfctBossCmnEsCreate(parent_obj, 4U);
        GmEffect3DESAddDispOffset(efct_3des2, 0.0f, 5f, -45f);
        GMM_BS_OBJ(efct_3des2).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffAfterburnerProcMain);
    }

    private static void gmBoss4EffAfterburnerProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK parentObj = (GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj.node_work.snm_work.reg_node_max);
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag[0] & 16) == 0)
            ObjDrawKillAction3DES(obj_work);
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.node_work.snm_work, parentObj.node_work.work[2], 1);
    }

    private static void gmBoss4EffAfterburnerExInit(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK parent_obj = GMM_BS_OBJ(body_work);
        GMS_EFFECT_3DES_WORK efct_3des1 = GmBoss4EffCommonInit(741, new VecFx32?(), parent_obj);
        GMM_BS_OBJ(efct_3des1).ppFunc = null;
        GmEffect3DESSetupBase(efct_3des1, 2U, 2U);
        GmEffect3DESSetDispRotation(efct_3des1, (short)GMD_BOSS4_EFF_ABURNER3_DISP_ROT_X, 0, 0);
        GmEffect3DESAddDispOffset(efct_3des1, -0.0f, -0.0f, 0.0f);
        GMM_BS_OBJ(efct_3des1).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffAfterburnerExProcMainL);
        GMS_EFFECT_3DES_WORK efct_3des2 = GmBoss4EffCommonInit(741, new VecFx32?(), parent_obj);
        GMM_BS_OBJ(efct_3des2).ppFunc = null;
        GmEffect3DESSetupBase(efct_3des2, 2U, 2U);
        GmEffect3DESSetDispRotation(efct_3des2, (short)GMD_BOSS4_EFF_ABURNER3_DISP_ROT_X, 0, 0);
        GmEffect3DESAddDispOffset(efct_3des2, 0.0f, -0.0f, 0.0f);
        GMM_BS_OBJ(efct_3des2).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffAfterburnerExProcMainR);
    }

    private static void gmBoss4EffAfterburnerExProcMainL(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK parentObj = (GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj.node_work.snm_work.reg_node_max);
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag[0] & 256) == 0)
            ObjDrawKillAction3DES(obj_work);
        NNS_MATRIX nodeMatrix1 = GmBoss4UtilGetNodeMatrix(parentObj.node_work, 5);
        NNS_MATRIX nodeMatrix2 = GmBoss4UtilGetNodeMatrix(parentObj.node_work, 2);
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        nnCopyMatrix(nnsMatrix, nodeMatrix1);
        nnsMatrix.M03 = (float)(nodeMatrix1.M03 - (double)nodeMatrix2.M03 + parentObj.ene_3d.ene_com.obj_work.pos.x / 4096.0);
        GmBoss4UtilSetMatrixES(obj_work, nnsMatrix);
        obj_work.disp_flag &= 4294963199U;
        if (((int)g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss4EffAfterburnerExProcMainR(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK parentObj = (GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj.node_work.snm_work.reg_node_max);
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag[0] & 256) == 0)
            ObjDrawKillAction3DES(obj_work);
        NNS_MATRIX nodeMatrix1 = GmBoss4UtilGetNodeMatrix(parentObj.node_work, 8);
        NNS_MATRIX nodeMatrix2 = GmBoss4UtilGetNodeMatrix(parentObj.node_work, 2);
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        nnCopyMatrix(nnsMatrix, nodeMatrix1);
        nnsMatrix.M03 = (float)(nodeMatrix1.M03 - (double)nodeMatrix2.M03 + parentObj.ene_3d.ene_com.obj_work.pos.x / 4096.0);
        GmBoss4UtilSetMatrixES(obj_work, nnsMatrix);
        obj_work.disp_flag &= 4294963199U;
        if (((int)g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss4EffABSmokeInit(GMS_BOSS4_BODY_WORK body_work)
    {
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 5U);
        GmBoss4EffChangeType(gmsEffect3DesWork, 2U, 19U);
        GmEffect3DESAddDispOffset(gmsEffect3DesWork, 0.0f, 0.0f, -32f);
        GMM_BS_OBJ(gmsEffect3DesWork).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffABSmokeProcMain);
    }

    private static void gmBoss4EffABSmokeProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK parentObj = (GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj.node_work.snm_work.reg_node_max);
        obj_work.disp_flag &= 4294963199U;
        if (((int)g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        else
            obj_work.pos.x += GmBoss4GetScrollOffset();
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.node_work.snm_work, parentObj.node_work.work[2], 1);
    }

    private static void gmBoss4EffBodySmokeInit(GMS_BOSS4_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK parent_obj = GMM_BS_OBJ(body_work);
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork1 = GmEfctBossCmnEsCreate(parent_obj, 3U);
        GmBoss4EffChangeType(gmsEffect3DesWork1, 2U, 19U);
        GmEffect3DESAddDispOffset(gmsEffect3DesWork1, 0.0f, 0.0f, -32f);
        GMM_BS_OBJ(gmsEffect3DesWork1).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffBodySmokeProcMain);
        float[][] numArray = new float[4][]
        {
      new float[3]{ -36f, 0.0f, -6f },
      new float[3]{ -20f, 6f, 16f },
      new float[3]{ 0.0f, 8f, -24f },
      new float[3]{ 36f, 0.0f, 0.0f }
        };
        for (int index = 0; index < 4; ++index)
        {
            GMS_EFFECT_3DES_WORK gmsEffect3DesWork2 = GmEfctBossCmnEsCreate(parent_obj, 2U);
            GmBoss4EffChangeType(gmsEffect3DesWork2, 2U, 19U);
            GmEffect3DESAddDispOffset(gmsEffect3DesWork2, numArray[index][0], numArray[index][1], numArray[index][2]);
            GMM_BS_OBJ(gmsEffect3DesWork2).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EffBodySmokeProcMain);
        }
    }

    private static void gmBoss4EffBodySmokeProcMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK parentObj = (GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(parentObj.node_work.snm_work.reg_node_max);
        obj_work.disp_flag &= 4294963199U;
        if (((int)g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        else
            obj_work.pos.x += GmBoss4GetScrollOffset();
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.node_work.snm_work, parentObj.node_work.work[2], 1);
    }

    private static void gmBoss4EffBossLightUpdateCreate(GMS_BOSS4_BODY_WORK body_work)
    {
        if (((int)body_work.flag[0] & 524288) == 0)
            return;
        body_work.flag[0] &= 4294443007U;
        VecFx32 vecFx32_1 = new VecFx32(FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f));
        VecFx32 vecFx32_2 = new VecFx32(FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f));
        GmBoss4EffCommonInit(744, new VecFx32?(vecFx32_1), (OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 9, new VecFx32?(vecFx32_2), body_work.flag, 512U);
        VecFx32 vecFx32_3 = new VecFx32(FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f));
        VecFx32 vecFx32_4 = new VecFx32(FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f), FX_F32_TO_FX32(0.0f));
        GmBoss4EffCommonInit(744, new VecFx32?(vecFx32_3), (OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 10, new VecFx32?(vecFx32_4), body_work.flag, 512U);
    }

    private static void gmBoss4EffBossLightSetEnable(GMS_BOSS4_BODY_WORK body_work, bool on)
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