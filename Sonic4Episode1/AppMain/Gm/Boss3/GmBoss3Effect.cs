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

    private static void gmBoss3EffDamageInit(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 0U)).pos.z += 131072;
    }

    private static void gmBoss3EffBombsInit(
      AppMain.GMS_BOSS3_EFF_BOMB_WORK bomb_work,
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

    private static void gmBoss3EffBombsUpdate(AppMain.GMS_BOSS3_EFF_BOMB_WORK bomb_work)
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

    private static void gmBoss3EffAfterburnerRequestCreate(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        body_work.flag |= 33554432U;
    }

    private static void gmBoss3EffAfterburnerRequestDelete(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        body_work.flag &= 4294967293U;
        body_work.flag &= 4261412863U;
    }

    private static void gmBoss3EffAfterburnerInit(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 2) != 0)
            return;
        body_work.flag &= 4261412863U;
        body_work.flag |= 2U;
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 4U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -30f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3EffAfterburnerMainFunc);
    }

    private static void gmBoss3EffAfterburnerMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS3_BODY_WORK parentObj = (AppMain.GMS_BOSS3_BODY_WORK)obj_work.parent_obj;
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj.flag & 2) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss3EffAfterburnerSmokeInit(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 5U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3EffAfterburnerSmokeMainFunc);
    }

    private static void gmBoss3EffAfterburnerSmokeMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS3_BODY_WORK parentObj = (AppMain.GMS_BOSS3_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss3EffBodySmokeInit(AppMain.GMS_BOSS3_BODY_WORK body_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctBossCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 3U);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 0.0f, -32f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3EffBodySmokeMainFunc);
    }

    private static void gmBoss3EffBodySmokeMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS3_BODY_WORK parentObj = (AppMain.GMS_BOSS3_BODY_WORK)obj_work.parent_obj;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
    }

    private static void gmBoss3EffSweatInit(AppMain.GMS_BOSS3_EGG_WORK egg_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)egg_work), 93);
        AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 32f, 0.0f);
        AppMain.GMM_BS_OBJ((object)efct_3des).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3EffSweatMainFunc);
        egg_work.flag |= 2U;
    }

    private static void gmBoss3EffSweatMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)((AppMain.GMS_BOSS3_EGG_WORK)obj_work.parent_obj).flag & 2) == 0)
            AppMain.ObjDrawKillAction3DES(obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.flag |= 4U;
    }

}