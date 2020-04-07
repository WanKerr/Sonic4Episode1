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

    private static void GmEneBukuBuild()
    {
        AppMain.gm_ene_buku_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(696)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(697)), 0U);
    }

    private static void GmEneBukuFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(696));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_buku_obj_3d_list, amsAmbHeader.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmEneBukuInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_BUKU_WORK()), "ENE_BUKU");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_BUKU_WORK gmsEneBukuWork = (AppMain.GMS_ENE_BUKU_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_buku_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(698), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-8, (short)-8, (short)8, (short)8);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        AppMain.ObjRectWorkSet(pRec2, (short)-16, (short)-16, (short)16, (short)16);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-19, (short)-16, (short)19, (short)16);
        pRec3.flag &= 4294967291U;
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        if (((int)eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        gmsEneBukuWork.spd_dec = 102;
        gmsEneBukuWork.spd_dec_dist = 20480;
        AppMain.gmEneBukuWalkInit(work);
        AppMain.GmComEfctSetDispOffsetF(AppMain.GmEfctEneEsCreate(work, 9), -24f, -5f, 0.0f);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmEneBukuWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 0, 1);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneBukuWalkMain);
        obj_work.move_flag &= 4294967291U;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -2048;
        else
            obj_work.spd.x = 2048;
    }

    private static void gmEneBukuWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_BUKU_WORK buku_work = (AppMain.GMS_ENE_BUKU_WORK)obj_work;
        if (AppMain.gmEneBukuSetWalkSpeed(buku_work) != 0)
            AppMain.gmEneBukuFlipInit(obj_work);
        if (buku_work.timer > 0)
            --buku_work.timer;
        else
            buku_work.timer = 216000 + (int)AppMain.mtMathRand() % 30;
    }

    private static void gmEneBukuFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        AppMain.gmEneBukuFlipInit(obj_work);
    }

    private static void gmEneBukuFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSet3DNNBlendDependHFlip(obj_work, 2, 3);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneBukuFlipMain);
    }

    private static void gmEneBukuFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmEneBukuSetWalkSpeed((AppMain.GMS_ENE_BUKU_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        AppMain.gmEneBukuWalkInit(obj_work);
    }

    private static int gmEneBukuSetWalkSpeed(AppMain.GMS_ENE_BUKU_WORK buku_work)
    {
        int num = 0;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)buku_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 3 && (double)obsObjectWork.obj_3d.frame[0] >= 20.0)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, buku_work.spd_dec, 2048);
            else if (obsObjectWork.pos.x <= (int)obsObjectWork.user_work + buku_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, buku_work.spd_dec);
                num = 1;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > (int)obsObjectWork.user_work)
                {
                    obsObjectWork.spd.x = (int)obsObjectWork.user_work - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -buku_work.spd_dec)
                        obsObjectWork.spd.x = -buku_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -2048)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -buku_work.spd_dec, 2048);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 2 && (double)obsObjectWork.obj_3d.frame[0] >= 20.0)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -buku_work.spd_dec, 2048);
        else if (obsObjectWork.pos.x >= (int)obsObjectWork.user_flag - buku_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, buku_work.spd_dec);
            num = 1;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < (int)obsObjectWork.user_flag)
            {
                obsObjectWork.spd.x = (int)obsObjectWork.user_flag - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > buku_work.spd_dec)
                    obsObjectWork.spd.x = buku_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < 2048)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, buku_work.spd_dec, 2048);
        return num;
    }
}