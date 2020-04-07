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
    private static void GmEneGardonBuild()
    {
        AppMain.gm_ene_gardon_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(677)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(678)), 0U);
    }

    private static void GmEneGardonFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(677));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_gardon_obj_3d_list, amsAmbHeader.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmEneGardonInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_GARDON_WORK()), "ENE_GARDON");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_GARDON_WORK gmsEneGardonWork = (AppMain.GMS_ENE_GARDON_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_gardon_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(679), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-11, (short)-24, (short)11, (short)0);
        pRec1.flag |= 1024U;
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        pRec2.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmEneGardonDefFunc);
        AppMain.ObjRectWorkSet(pRec2, (short)-24, (short)-32, (short)24, (short)0);
        pRec2.flag |= 1024U;
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-19, (short)-32, (short)19, (short)0);
        pRec3.flag &= 4294967291U;
        AppMain.ObjObjectFieldRectSet(work, (short)-4, (short)-8, (short)4, (short)-2);
        work.move_flag |= 128U;
        if (((int)eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        gmsEneGardonWork.spd_dec = 51;
        gmsEneGardonWork.spd_dec_dist = 10240;
        AppMain.gmEneGardonWalkInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static int gmEneGardonGetLength2N(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
            return int.MaxValue;
        int x1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int x2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        float f32_1 = AppMain.FX_FX32_TO_F32(x1);
        float f32_2 = AppMain.FX_FX32_TO_F32(x2);
        return (int)((double)f32_1 * (double)f32_1 + (double)f32_2 * (double)f32_2);
    }

    private static int gmEneGardonIsPlayerAttack()
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        return gmsPlayerWork.seq_state == 19 || gmsPlayerWork.seq_state == 17 || gmsPlayerWork.seq_state == 10 ? 1 : 0;
    }

    private static int gmEneGardonIsPlayerFront(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (((int)obj_work.disp_flag & 1) != 0)
        {
            if (obj_work.pos.x > gmsPlayerWork.obj_work.pos.x)
                return 1;
        }
        else if (obj_work.pos.x < gmsPlayerWork.obj_work.pos.x)
            return 1;
        return 0;
    }

    private static void gmEneGardonAtkRectOff(AppMain.OBS_OBJECT_WORK obj_work)
    {
        ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1].flag &= 4294967291U;
    }

    private static void gmEneGardonAtkRectOn(AppMain.OBS_OBJECT_WORK obj_work)
    {
        ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1].flag |= 4U;
    }

    private static void gmEneGardonDefFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        AppMain.GMS_ENE_GARDON_WORK gmsEneGardonWork = (AppMain.GMS_ENE_GARDON_WORK)parentObj1;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)parentObj2;
        if (parentObj2 == null || (ushort)1 != parentObj2.obj_type)
            return;
        if (ply_work.seq_state == 19 || ply_work.seq_state == 20)
        {
            if (AppMain.gmEneGardonIsPlayerFront(parentObj1) != 0)
            {
                AppMain.GmEneComActionSetDependHFlip(parentObj1, 8, 9);
                parentObj1.disp_flag &= 4294967291U;
                gmsEneGardonWork.shield = 1;
                AppMain.GmPlySeqAtkReactionInit(ply_work);
                ply_work.obj_work.spd.y = (int)((double)ply_work.obj_work.spd.y * 1.5);
                AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctEneEsCreate(parentObj1, 5);
                efct_3des.efct_com.obj_work.pos.x = parentObj1.pos.x;
                efct_3des.efct_com.obj_work.pos.y = parentObj1.pos.y;
                AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 30f, 0.0f);
                AppMain.GmSoundPlaySE("Casino1");
                AppMain.gmEneGardonAtkRectOff(parentObj1);
            }
            else
                AppMain.GmEnemyDefaultDefFunc(my_rect, your_rect);
        }
        else if (parentObj1.pos.y - AppMain.FX_F32_TO_FX32(20f) > parentObj2.pos.y)
        {
            if (AppMain.gmEneGardonIsPlayerFront(parentObj1) != 0 || ((int)parentObj1.disp_flag & 1) != ((int)parentObj2.disp_flag & 1))
            {
                AppMain.GmEneComActionSetDependHFlip(parentObj1, 8, 9);
                parentObj1.disp_flag &= 4294967291U;
                gmsEneGardonWork.shield = 1;
                AppMain.GmPlySeqAtkReactionInit(ply_work);
                ply_work.obj_work.spd.y = (int)((double)ply_work.obj_work.spd.y * 1.5);
                AppMain.GMS_EFFECT_3DES_WORK efct_3des = AppMain.GmEfctEneEsCreate(parentObj1, 5);
                efct_3des.efct_com.obj_work.pos.x = parentObj1.pos.x;
                efct_3des.efct_com.obj_work.pos.y = parentObj1.pos.y;
                AppMain.GmEffect3DESAddDispOffset(efct_3des, 0.0f, 30f, 0.0f);
                AppMain.GmSoundPlaySE("Casino1");
            }
            else
                AppMain.GmEnemyDefaultDefFunc(my_rect, your_rect);
        }
        else if (AppMain.gmEneGardonIsPlayerFront(parentObj1) != 0)
        {
            AppMain.GmEneComActionSetDependHFlip(parentObj1, 4, 5);
            parentObj1.disp_flag &= 4294967291U;
            gmsEneGardonWork.shield = 2;
            ply_work.obj_work.disp_flag ^= 1U;
            AppMain.GmPlySeqChangeSequence(ply_work, 10);
            if (ply_work.obj_work.spd_m != 0)
            {
                ply_work.obj_work.spd_m = -ply_work.obj_work.spd_m;
                if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 32768)
                    ply_work.obj_work.spd_m = ((int)ply_work.obj_work.disp_flag & 1) == 0 ? 32768 : (int)short.MinValue;
            }
            else if (parentObj1.pos.x > ply_work.obj_work.pos.x)
            {
                ply_work.obj_work.spd_m = -49152;
                ply_work.obj_work.disp_flag |= 1U;
            }
            else
            {
                ply_work.obj_work.spd_m = 49152;
                ply_work.obj_work.disp_flag &= 4294967294U;
            }
            AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctEneEsCreate(parentObj1, 5);
            gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj1.pos.x;
            gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj1.pos.y;
            AppMain.GmSoundPlaySE("Casino1");
        }
        else
            AppMain.GmEnemyDefaultDefFunc(my_rect, your_rect);
    }

    private static void gmEneGardonWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 0, 1);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGardonWalkMain);
        obj_work.move_flag &= 4294967291U;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -1024;
        else
            obj_work.spd.x = 1024;
    }

    private static void gmEneGardonWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_GARDON_WORK gmsEneGardonWork = (AppMain.GMS_ENE_GARDON_WORK)obj_work;
        if (gmsEneGardonWork.shield != 0)
        {
            obj_work.spd.x = 0;
            if (((int)obj_work.disp_flag & 8) == 0)
                return;
            if (gmsEneGardonWork.shield == 1)
                AppMain.GmEneComActionSetDependHFlip(obj_work, 10, 11);
            else
                AppMain.GmEneComActionSetDependHFlip(obj_work, 6, 7);
            AppMain.gmEneGardonAtkRectOn(obj_work);
            gmsEneGardonWork.shield = 0;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGardonWalkWait);
        }
        else
        {
            obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 1024 : -1024;
            if ((double)obj_work.obj_3d.frame[0] >= 40.0 && (double)obj_work.obj_3d.frame[0] <= 60.0)
                obj_work.spd.x = 0;
            if ((double)obj_work.obj_3d.frame[0] >= 100.0 && (double)obj_work.obj_3d.frame[0] <= 120.0)
                obj_work.spd.x = 0;
            if (AppMain.gmEneGardonGetLength2N(obj_work) <= 4)
            {
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGardonWaitToWalkInit);
            }
            else
            {
                if (((int)obj_work.move_flag & 4) == 0 && AppMain.GmEneComCheckMoveLimit(obj_work, (int)obj_work.user_work, (int)obj_work.user_flag) != 0)
                    return;
                AppMain.gmEneGardonWaitToFlipInit(obj_work);
            }
        }
    }

    private static void gmEneGardonWalkWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGardonWaitToWalkInit);
    }

    private static void gmEneGardonWaitToFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGardonWaitToFlipMain);
        obj_work.spd.x = 0;
        ((AppMain.GMS_ENE_GARDON_WORK)obj_work).timer = 1;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 0, 1);
    }

    private static void gmEneGardonWaitToFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_GARDON_WORK gmsEneGardonWork = (AppMain.GMS_ENE_GARDON_WORK)obj_work;
        if (gmsEneGardonWork.timer > 0)
            --gmsEneGardonWork.timer;
        else
            AppMain.gmEneGardonFlipInit(obj_work);
    }

    private static void gmEneGardonWaitToWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGardonWaitToWalkMain);
        obj_work.spd.x = 0;
        AppMain.GMS_ENE_GARDON_WORK gmsEneGardonWork = (AppMain.GMS_ENE_GARDON_WORK)obj_work;
        gmsEneGardonWork.timer = 60;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 0, 1);
        obj_work.disp_flag |= 4U;
        AppMain.gmEneGardonAtkRectOn(obj_work);
        gmsEneGardonWork.shield = 0;
    }

    private static void gmEneGardonWaitToWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_GARDON_WORK gmsEneGardonWork = (AppMain.GMS_ENE_GARDON_WORK)obj_work;
        if (gmsEneGardonWork.shield != 0)
        {
            obj_work.spd.x = 0;
            if (((int)obj_work.disp_flag & 8) == 0)
                return;
            if (gmsEneGardonWork.shield == 1)
                AppMain.GmEneComActionSetDependHFlip(obj_work, 10, 11);
            else
                AppMain.GmEneComActionSetDependHFlip(obj_work, 6, 7);
            AppMain.gmEneGardonAtkRectOn(obj_work);
            gmsEneGardonWork.shield = 0;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGardonWalkWait);
        }
        else
        {
            if (AppMain.gmEneGardonGetLength2N(obj_work) <= 4)
                return;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGardonWalkInit);
        }
    }

    private static void gmEneGardonFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 2, 3);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGardonFlipMain);
    }

    private static void gmEneGardonFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_ENE_GARDON_WORK)obj_work).shield != 0)
        {
            obj_work.disp_flag ^= 1U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGardonWalkMain);
        }
        else
        {
            AppMain.gmEneGardonSetWalkSpeed((AppMain.GMS_ENE_GARDON_WORK)obj_work);
            if (((int)obj_work.disp_flag & 8) == 0)
                return;
            obj_work.disp_flag ^= 1U;
            AppMain.gmEneGardonWalkInit(obj_work);
        }
    }

    private static int gmEneGardonSetWalkSpeed(AppMain.GMS_ENE_GARDON_WORK gardon_work)
    {
        int num = 0;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)gardon_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 2 && (double)obsObjectWork.obj_3d.frame[0] >= 20.0)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, gardon_work.spd_dec, 1024);
            else if (obsObjectWork.pos.x <= (int)obsObjectWork.user_work + gardon_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, gardon_work.spd_dec);
                num = 1;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > (int)obsObjectWork.user_work)
                {
                    obsObjectWork.spd.x = (int)obsObjectWork.user_work - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -gardon_work.spd_dec)
                        obsObjectWork.spd.x = -gardon_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -1024)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -gardon_work.spd_dec, 1024);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 2 && (double)obsObjectWork.obj_3d.frame[0] >= 20.0)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -gardon_work.spd_dec, 1024);
        else if (obsObjectWork.pos.x >= (int)obsObjectWork.user_flag - gardon_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, gardon_work.spd_dec);
            num = 1;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < (int)obsObjectWork.user_flag)
            {
                obsObjectWork.spd.x = (int)obsObjectWork.user_flag - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > gardon_work.spd_dec)
                    obsObjectWork.spd.x = gardon_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < 1024)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, gardon_work.spd_dec, 1024);
        return num;
    }


}