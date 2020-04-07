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
    private static void GmEneMereonBuild()
    {
        AppMain.gm_ene_mereon_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(670)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(672)), 0U);
        AppMain.gm_ene_mereon_r_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(671)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(672)), 0U);
    }

    private static void GmEneMereonFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader1 = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(670));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_mereon_obj_3d_list, amsAmbHeader1.file_num);
        AppMain.AMS_AMB_HEADER amsAmbHeader2 = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(671));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_mereon_r_obj_3d_list, amsAmbHeader2.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmEneMereonInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "ENE_MEREON");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_mereon_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(673), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
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
        AppMain.ObjObjectFieldRectSet(work, (short)-4, (short)-8, (short)4, (short)0);
        work.obj_3d.drawflag |= 8388608U;
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        work.flag |= 2U;
        AppMain.GmEneComActionSetDependHFlip(work, 0, 1);
        if (eve_rec.id == (ushort)4)
            gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        AppMain.gmEneMereonHideSearchInit(work);
        return work;
    }

    private static void gmEneMereonHideSearchInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        obj_work.disp_flag &= 4294967294U;
        if (gmsPlayerWork.obj_work.pos.x < obj_work.pos.x)
            obj_work.disp_flag |= 1U;
        AppMain.gmEneMereonCheckFwFlip(obj_work);
        obj_work.disp_flag |= 32U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMereonHideSearchMain);
    }

    private static void gmEneMereonHideSearchMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        float[] numArray1 = new float[2];
        float[] numArray2 = new float[2];
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        AppMain.OBS_RECT_WORK obsRectWork1 = gmsPlayerWork.rect_work[2];
        numArray1[0] = AppMain.FXM_FX32_TO_FLOAT(gmsPlayerWork.obj_work.pos.x + ((int)obsRectWork1.rect.top + (int)obsRectWork1.rect.bottom >> 1));
        numArray1[1] = AppMain.FXM_FX32_TO_FLOAT(gmsPlayerWork.obj_work.pos.y + ((int)obsRectWork1.rect.left + (int)obsRectWork1.rect.right >> 1));
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        AppMain.OBS_RECT_WORK obsRectWork2 = gmsEnemyComWork.rect_work[2];
        numArray2[0] = AppMain.FXM_FX32_TO_FLOAT(obj_work.pos.x + ((int)obsRectWork2.rect.top + (int)obsRectWork2.rect.bottom >> 1));
        numArray2[1] = AppMain.FXM_FX32_TO_FLOAT(obj_work.pos.y + ((int)obsRectWork2.rect.left + (int)obsRectWork2.rect.right >> 1));
        float num1 = numArray2[0] - numArray1[0];
        float num2 = numArray2[1] - numArray1[1];
        float num3 = gmsEnemyComWork.eve_rec.id != (ushort)4 ? 192f : 96f;
        if ((double)num1 * (double)num1 + (double)num2 * (double)num2 > (double)num3 * (double)num3)
            return;
        obj_work.disp_flag &= 4294967263U;
        AppMain.gmEneMereonAppearInit(obj_work);
    }

    private static void gmEneMereonAppearInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        obj_work.disp_flag &= 4294967294U;
        if (gmsEnemyComWork.eve_rec.id == (ushort)4 && gmsPlayerWork.obj_work.pos.x > obj_work.pos.x)
            obj_work.disp_flag |= 1U;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 0, 1);
        obj_work.disp_flag |= 4U;
        obj_work.disp_flag |= 134217728U;
        obj_work.obj_3d.draw_state.alpha.alpha = 0.0f;
        obj_work.user_timer = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMereonAppearMain);
    }

    private static void gmEneMereonAppearMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        obj_work.user_timer = AppMain.ObjTimeCountUp(obj_work.user_timer);
        int num = gmsEnemyComWork.eve_rec.id != (ushort)4 ? 15 : 30;
        obj_work.obj_3d.draw_state.alpha.alpha = (float)obj_work.user_timer / (float)(num * 4096);
        if (obj_work.user_timer < num * 4096)
            return;
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        obj_work.disp_flag &= 4160749567U;
        if (gmsEnemyComWork.eve_rec.id == (ushort)4)
            AppMain.gmEneMereonAtkInit(obj_work);
        else
            AppMain.gmEneMereonAtkRocketInit(obj_work);
    }

    private static void gmEneMereonAtkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GmEneComActionSet3DNNBlendDependHFlip(obj_work, 3, 4);
        obj_work.flag &= 4294967293U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMereonAtkMain);
        obj_work.user_timer = 0;
    }

    private static void gmEneMereonAtkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) != 0)
        {
            int spd_x = -8192;
            short dir = -16384;
            if (((int)obj_work.disp_flag & 1) != 0)
            {
                spd_x = -spd_x;
                dir += short.MinValue;
            }
            AppMain.GmEneStingCreateBullet(obj_work, -81920, -49152, 32768, -98304, -49152, 0, spd_x, 0, dir);
            AppMain.GmSoundPlaySE("Sting");
            AppMain.GmEneComActionSet3DNNBlendDependHFlip(obj_work, 0, 1);
            obj_work.user_timer = 122880;
        }
        if (obj_work.user_timer == 0)
            return;
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer != 0)
            return;
        AppMain.gmEneMereonHideInit(obj_work);
    }

    private static void gmEneMereonHideInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMereonHideMain);
        obj_work.disp_flag |= 134217728U;
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        obj_work.user_timer = 122880;
    }

    private static void gmEneMereonHideMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        obj_work.obj_3d.draw_state.alpha.alpha = (float)obj_work.user_timer / 122880f;
        if (obj_work.user_timer > 0)
            return;
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        obj_work.disp_flag &= 4160749567U;
        obj_work.disp_flag |= 32U;
        obj_work.flag |= 4U;
    }

    private static void gmEneMereonAtkRocketInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 2);
        obj_work.move_flag &= 4294967039U;
        obj_work.move_flag |= 128U;
        obj_work.flag &= 4294967293U;
        obj_work.disp_flag &= 1U;
        obj_work.user_timer = 0;
        if (AppMain.GmEneComTargetIsLeft(obj_work, AppMain.g_gm_main_system.ply_work[0].obj_work) == 0)
            obj_work.user_timer = 4096;
        obj_work.user_work = 0U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMereonRocketFallMain);
    }

    private static void gmEneMereonRocketFallMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer != 0)
        {
            obj_work.dir.y += (ushort)obj_work.user_timer;
            if (obj_work.dir.y >= (ushort)32768)
            {
                obj_work.dir.y = (ushort)32768;
                obj_work.user_timer = 0;
            }
        }
        if (obj_work.user_work != 0U)
        {
            obj_work.user_work = (uint)AppMain.ObjTimeCountDown((int)obj_work.user_work);
            if (obj_work.user_work != 0U)
                return;
            AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
            AppMain.ObjAction3dNNMotionRelease(obj_work.obj_3d);
            AppMain.ObjObjectAction3dNNModelReleaseCopy(obj_work);
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_ene_mereon_r_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            AppMain.ObjObjectAction3dNNMotionLoad(obj_work, 0, false, AppMain.ObjDataGet(673), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
            AppMain.ObjDrawObjectSetToon(obj_work);
            AppMain.ObjObjectFieldRectSet(obj_work, (short)-4, (short)-4, (short)4, (short)4);
            AppMain.ObjDrawObjectActionSet(obj_work, 5);
            obj_work.disp_flag |= 4U;
            if (obj_work.dir.y != (ushort)0 || obj_work.user_timer != 0)
            {
                obj_work.dir.y = (ushort)0;
                obj_work.disp_flag &= 4294967294U;
            }
            else
                obj_work.disp_flag |= 1U;
            obj_work.move_flag |= 64U;
            obj_work.spd_m = ((int)obj_work.disp_flag & 1) == 0 ? 8192 : -8192;
            obj_work.move_flag |= 1024U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMereonRocketMain);
            obj_work.user_flag = 0U;
            obj_work.user_timer = 0;
            AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctEneEsCreate(obj_work, 1);
            AppMain.GmComEfctSetDispOffsetF(efct_work, -38f, -11f, 0.0f);
            efct_work.efct_com.obj_work.flag |= 524288U;
        }
        else
        {
            if (((int)obj_work.disp_flag & 8) == 0 || ((int)obj_work.move_flag & 1) == 0)
                return;
            obj_work.user_work = 4096U;
            AppMain.GmComEfctAddDispOffsetF(AppMain.GmEfctEneEsCreate(obj_work, 4), 0.0f, -16f, 16f);
        }
    }

    private static void gmEneMereonRocketMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_flag == 0U && (ushort)((uint)obj_work.dir.z + 8192U) > (ushort)16384)
        {
            obj_work.user_flag = 1U;
            obj_work.move_flag &= 4294967231U;
            obj_work.move_flag |= 257U;
            obj_work.spd.x = obj_work.spd_m;
        }
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
            if (obj_work.user_timer != 0)
                return;
            obj_work.move_flag |= 64U;
            obj_work.user_flag = 0U;
            obj_work.spd.x = 0;
        }
        else
        {
            if (obj_work.user_flag == 0U)
                return;
            obj_work.dir.z = AppMain.ObjRoopMove16(obj_work.dir.z, (ushort)0, (short)1024);
            if (obj_work.dir.z != (ushort)0)
                return;
            obj_work.user_timer = 4;
        }
    }

    private static void gmEneMereonCheckFwFlip(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (gmsPlayerWork.obj_work.pos.x < obj_work.pos.x)
        {
            if (obj_work.obj_3d.act_id[0] != 0)
            {
                AppMain.ObjDrawObjectActionSet(obj_work, 0);
                obj_work.disp_flag &= 4294967294U;
            }
        }
        else if (gmsPlayerWork.obj_work.pos.x > obj_work.pos.x && obj_work.obj_3d.act_id[0] != 1)
        {
            AppMain.ObjDrawObjectActionSet(obj_work, 1);
            obj_work.disp_flag |= 1U;
        }
        obj_work.disp_flag |= 4U;
    }


}