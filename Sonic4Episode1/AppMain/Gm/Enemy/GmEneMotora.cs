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
    private static AppMain.OBS_OBJECT_WORK GmEneMotoraInit(
         AppMain.GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_MOTORA_WORK()), "ENE_MOTORA");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_MOTORA_WORK gmsEneMotoraWork = (AppMain.GMS_ENE_MOTORA_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_motora_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(663), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
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
        AppMain.ObjObjectFieldRectSet(work, (short)-4, (short)-8, (short)4, (short)-2);
        work.move_flag |= 128U;
        if (((int)eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        gmsEneMotoraWork.spd_dec = 102;
        gmsEneMotoraWork.spd_dec_dist = 20480;
        AppMain.gmEneMotoraWalkInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static void GmEneMotoraBuild()
    {
        AppMain.gm_ene_motora_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(661)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(662)), 0U);
    }

    public static void GmEneMotoraFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(661));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_motora_obj_3d_list, amsAmbHeader.file_num);
    }

    public static void gmEneMotoraWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 1, 2);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMotoraWalkMain);
        obj_work.move_flag &= 4294967291U;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -2048;
        else
            obj_work.spd.x = 2048;
    }

    private static void gmEneMotoraWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.gmEneMotoraSetWalkSpeed((AppMain.GMS_ENE_MOTORA_WORK)obj_work))
            return;
        AppMain.gmEneMotoraFlipInit(obj_work);
    }

    private static bool gmEneMotoraSetWalkSpeed(AppMain.GMS_ENE_MOTORA_WORK motora_work)
    {
        bool flag = false;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)motora_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 4 && (double)obsObjectWork.obj_3d.frame[0] >= 20.0)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, motora_work.spd_dec, 2048);
            else if (obsObjectWork.pos.x <= Convert.ToInt32(obsObjectWork.user_work) + motora_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, motora_work.spd_dec);
                flag = true;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > Convert.ToInt32(obsObjectWork.user_work))
                {
                    obsObjectWork.spd.x = Convert.ToInt32(obsObjectWork.user_work) - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -motora_work.spd_dec)
                        obsObjectWork.spd.x = -motora_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -2048)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -motora_work.spd_dec, 2048);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 3 && (double)obsObjectWork.obj_3d.frame[0] >= 20.0)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -motora_work.spd_dec, 2048);
        else if (obsObjectWork.pos.x >= Convert.ToInt32(obsObjectWork.user_flag) - motora_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, motora_work.spd_dec);
            flag = true;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < Convert.ToInt32(obsObjectWork.user_flag))
            {
                obsObjectWork.spd.x = Convert.ToInt32(obsObjectWork.user_flag) - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > motora_work.spd_dec)
                    obsObjectWork.spd.x = motora_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < 2048)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, motora_work.spd_dec, 2048);
        return flag;
    }

    private static void gmEneMotoraFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSet3DNNBlendDependHFlip(obj_work, 3, 4);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneMotoraFlipMain);
    }

    private static void gmEneMotoraFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmEneMotoraSetWalkSpeed((AppMain.GMS_ENE_MOTORA_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        AppMain.gmEneMotoraWalkInit(obj_work);
    }
}