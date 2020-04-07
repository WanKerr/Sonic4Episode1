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
    public static void GmEneMoguBuild()
    {
        AppMain.gm_ene_mogu_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(674)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(675)), 0U);
    }

    public static void GmEneMoguFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(674));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_mogu_obj_3d_list, amsAmbHeader.file_num);
    }

    public static AppMain.OBS_OBJECT_WORK GmEneMoguInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_MOGU_WORK()), "ENE_MOGU");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_MOGU_WORK gmsEneMoguWork = (AppMain.GMS_ENE_MOGU_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_mogu_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(676), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-11, (short)-24, (short)11, (short)0);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        AppMain.ObjRectWorkSet(pRec2, (short)-19, (short)-32, (short)19, (short)0);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-19, (short)-32, (short)19, (short)0);
        pRec3.flag &= 4294967291U;
        AppMain.ObjObjectFieldRectSet(work, (short)-4, (short)-44, (short)4, (short)-38);
        work.move_flag |= 128U;
        if (((int)eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        gmsEneMoguWork.spd_dec = 102;
        gmsEneMoguWork.spd_dec_dist = 20480;
        gmsEneMoguWork.flag = 0U;
        AppMain.gmEneMoguWaitInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static int gmEneMoguCheckWater(AppMain.GMS_ENE_MOGU_WORK mogu_work, short ofst)
    {
        AppMain.OBS_OBJECT_WORK parent_obj = (AppMain.OBS_OBJECT_WORK)mogu_work;
        if (!AppMain.GmMainIsWaterLevel())
            return 0;
        if ((parent_obj.pos.y >> 12) - (int)ofst >= (int)AppMain.g_gm_main_system.water_level)
        {
            if (((int)mogu_work.flag & 1) == 0 && ((int)mogu_work.flag & 2) != 0)
            {
                AppMain.GmEfctCmnEsCreate(parent_obj, 76);
                AppMain.GmSoundPlaySE("Spray");
            }
            mogu_work.flag |= 1U;
            return 1;
        }
        if (((int)mogu_work.flag & 1) == 0)
            return 0;
        if (((int)mogu_work.flag & 2) != 0)
        {
            AppMain.GmEfctCmnEsCreate(parent_obj, 76);
            AppMain.GmSoundPlaySE("Spray");
        }
        mogu_work.flag &= 4294967294U;
        return 1;
    }

    public static void gmEneMoguWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet(obj_work, 4);
        obj_work.disp_flag |= 4U;
        obj_work.pos.z = -655360;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMoguWaitMain);
    }

    public static void gmEneMoguWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_MOGU_WORK mogu_work = (AppMain.GMS_ENE_MOGU_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        int num1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int num2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        if (AppMain.FX_Mul(num1, num1) + AppMain.FX_Mul(num2, num2) > AppMain.FX_F32_TO_FX32(25600f))
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMoguJumpInit);
        AppMain.GMS_EFFECT_3DES_WORK efct_work;
        if (AppMain.gmEneMoguCheckWater(mogu_work, (short)48) == 0)
        {
            efct_work = AppMain.GmEfctEneEsCreate(obj_work, 7);
            if (AppMain.g_gs_main_sys_info.stage_id == (ushort)9)
                efct_work.efct_com.obj_work.pos.z = 393216;
        }
        else
            efct_work = AppMain.GmEfctEneEsCreate(obj_work, 8);
        AppMain.GmComEfctSetDispOffsetF(efct_work, 0.0f, -30f, 0.0f);
    }

    public static void gmEneMoguJumpInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_MOGU_WORK mogu_work = (AppMain.GMS_ENE_MOGU_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet(obj_work, 4);
        obj_work.disp_flag |= 4U;
        obj_work.spd.y = AppMain.FX_F32_TO_FX32(-6f);
        obj_work.spd_fall = AppMain.FX_F32_TO_FX32(0.16f);
        obj_work.pos.y -= AppMain.FX_F32_TO_FX32(4f);
        obj_work.move_flag |= 128U;
        obj_work.move_flag &= 4294967294U;
        obj_work.move_flag &= 4294967291U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMoguJumpMain);
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
        AppMain.gmEneMoguCheckWater(mogu_work, (short)0);
        mogu_work.jumpdown = 0;
    }

    public static void gmEneMoguJumpMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_MOGU_WORK mogu_work = (AppMain.GMS_ENE_MOGU_WORK)obj_work;
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
        if (obj_work.spd.y > 0)
        {
            if (mogu_work.jumpdown == 0)
            {
                mogu_work.jumpdown = 1;
                AppMain.ObjDrawObjectActionSet(obj_work, 5);
                obj_work.disp_flag |= 4U;
                obj_work.move_flag &= 4294967039U;
                AppMain.ObjObjectFieldRectSet(obj_work, (short)-4, (short)-8, (short)4, (short)-2);
                obj_work.pos.z = 0;
                mogu_work.flag |= 2U;
            }
            if (((int)obj_work.move_flag & 4) != 0)
            {
                if (((int)obj_work.disp_flag & 1) != 0)
                    obj_work.disp_flag &= 4294967294U;
                else
                    obj_work.disp_flag |= 1U;
            }
        }
        if (((int)obj_work.move_flag & 1) != 0)
        {
            AppMain.GmEneComActionSetDependHFlip(obj_work, 0, 1);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMoguJumpEnd);
        }
        AppMain.gmEneMoguCheckWater(mogu_work, (short)0);
    }

    public static void gmEneMoguJumpEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmEneMoguCheckWater((AppMain.GMS_ENE_MOGU_WORK)obj_work, (short)0);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMoguWalkInit);
    }

    public static void gmEneMoguWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 6, 7);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMoguWalkMain);
        obj_work.move_flag &= 4294967291U;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -2048;
        else
            obj_work.spd.x = 2048;
    }

    public static void gmEneMoguWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        AppMain.GMS_ENE_MOGU_WORK mogu_work = (AppMain.GMS_ENE_MOGU_WORK)obj_work;
        AppMain.gmEneMoguCheckWater(mogu_work, (short)0);
        int num1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int num2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        int num3 = AppMain.FX_Mul(num1, num1) + AppMain.FX_Mul(num2, num2);
        if (((int)((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 2) != 0)
        {
            num3 = AppMain.FX_F32_TO_FX32(6400f) + 1;
            mogu_work.wait_time = 216000;
        }
        if (num3 > AppMain.FX_F32_TO_FX32(6400f))
        {
            if (mogu_work.wait_time > 0)
            {
                --mogu_work.wait_time;
                if (((int)obj_work.move_flag & 4) == 0)
                    return;
                AppMain.gmEneMoguJumpInit(obj_work);
            }
            else
            {
                if (AppMain.AkMathRandFx() > AppMain.FX_F32_TO_FX32(0.5f))
                    AppMain.gmEneMoguFlipInit(obj_work);
                mogu_work.wait_time = 216000;
            }
        }
        else
        {
            mogu_work.wait_time = 0;
            if (((int)obj_work.disp_flag & 1) != 0)
            {
                if (AppMain.GmEneComTargetIsLeft(obj_work, gmsPlayerWork.obj_work) == 0)
                    AppMain.gmEneMoguFlipInit(obj_work);
            }
            else if (AppMain.GmEneComTargetIsLeft(obj_work, gmsPlayerWork.obj_work) != 0)
                AppMain.gmEneMoguFlipInit(obj_work);
            if (((int)obj_work.move_flag & 4) == 0)
                return;
            AppMain.gmEneMoguJumpInit(obj_work);
        }
    }

    public static void gmEneMoguFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        AppMain.gmEneMoguFlipInit(obj_work);
    }

    public static void gmEneMoguFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSet3DNNBlendDependHFlip(obj_work, 2, 3);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMoguFlipMain);
    }

    public static void gmEneMoguFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmEneMoguSetWalkSpeed((AppMain.GMS_ENE_MOGU_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        AppMain.gmEneMoguWalkInit(obj_work);
    }

    public static int gmEneMoguSetWalkSpeed(AppMain.GMS_ENE_MOGU_WORK mogu_work)
    {
        int num = 0;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)mogu_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 3 && (double)obsObjectWork.obj_3d.frame[0] >= 20.0)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, mogu_work.spd_dec, 2048);
            else if (obsObjectWork.pos.x <= (int)obsObjectWork.user_work + mogu_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, mogu_work.spd_dec);
                num = 1;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > (int)obsObjectWork.user_work)
                {
                    obsObjectWork.spd.x = (int)obsObjectWork.user_work - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -mogu_work.spd_dec)
                        obsObjectWork.spd.x = -mogu_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -2048)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -mogu_work.spd_dec, 2048);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 2 && (double)obsObjectWork.obj_3d.frame[0] >= 20.0)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -mogu_work.spd_dec, 2048);
        else if (obsObjectWork.pos.x >= (int)obsObjectWork.user_flag - mogu_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, mogu_work.spd_dec);
            num = 1;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < (int)obsObjectWork.user_flag)
            {
                obsObjectWork.spd.x = (int)obsObjectWork.user_flag - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > mogu_work.spd_dec)
                    obsObjectWork.spd.x = mogu_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < 2048)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, mogu_work.spd_dec, 2048);
        return num;
    }

}