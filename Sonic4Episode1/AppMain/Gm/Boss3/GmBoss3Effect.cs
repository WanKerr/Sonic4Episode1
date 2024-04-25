public partial class AppMain
{

    private static void gmBoss3EffDamageInit(GMS_BOSS3_BODY_WORK body_work)
    {
        GMM_BS_OBJ(GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 0U)).pos.z += 131072;
    }

    private static void gmBoss3EffBombsInit(
      GMS_BOSS3_EFF_BOMB_WORK bomb_work,
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

    private static void gmBoss3EffBombsUpdate(GMS_BOSS3_EFF_BOMB_WORK bomb_work)
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

    private static void gmBoss3EffAfterburnerRequestCreate(GMS_BOSS3_BODY_WORK body_work)
    {
        body_work.flag |= 33554432U;
    }

    private static void gmBoss3EffAfterburnerRequestDelete(GMS_BOSS3_BODY_WORK body_work)
    {
        body_work.flag &= 4294967293U;
        body_work.flag &= 4261412863U;
    }

    private static void gmBoss3EffAfterburnerInit(GMS_BOSS3_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 2) != 0)
            return;
        body_work.flag &= 4261412863U;
        body_work.flag |= 2U;
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 4U);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -30f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3EffAfterburnerMainFunc);
    }

    private static void gmBoss3EffAfterburnerMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS3_BODY_WORK parentObj = (GMS_BOSS3_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 2) == 0)
            ObjDrawKillAction3DES(obj_work);
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss3EffAfterburnerSmokeInit(GMS_BOSS3_BODY_WORK body_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 5U);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3EffAfterburnerSmokeMainFunc);
    }

    private static void gmBoss3EffAfterburnerSmokeMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS3_BODY_WORK parentObj = (GMS_BOSS3_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss3EffBodySmokeInit(GMS_BOSS3_BODY_WORK body_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctBossCmnEsCreate(GMM_BS_OBJ(body_work), 3U);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3EffBodySmokeMainFunc);
    }

    private static void gmBoss3EffBodySmokeMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS3_BODY_WORK parentObj = (GMS_BOSS3_BODY_WORK)obj_work.parent_obj;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss3EffSweatInit(GMS_BOSS3_EGG_WORK egg_work)
    {
        GMS_EFFECT_3DES_WORK efct_3des = GmEfctCmnEsCreate(GMM_BS_OBJ(egg_work), 93);
        GmEffect3DESAddDispOffset(efct_3des, 0.0f, 32f, 0.0f);
        GMM_BS_OBJ(efct_3des).ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3EffSweatMainFunc);
        egg_work.flag |= 2U;
    }

    private static void gmBoss3EffSweatMainFunc(OBS_OBJECT_WORK obj_work)
    {
        if (((int)((GMS_BOSS3_EGG_WORK)obj_work.parent_obj).flag & 2) == 0)
            ObjDrawKillAction3DES(obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

}