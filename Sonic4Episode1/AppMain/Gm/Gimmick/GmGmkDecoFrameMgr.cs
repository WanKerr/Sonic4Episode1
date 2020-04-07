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
    private static AppMain.OBS_OBJECT_WORK GmGmkDecoFrameMgrInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        if (eve_rec.byte_param[1] != (byte)0)
        {
            eve_rec.pos_x = byte.MaxValue;
            return (AppMain.OBS_OBJECT_WORK)null;
        }
        if (((int)eve_rec.flag & 1) != 0)
        {
            int index = 0;
            if (eve_rec.id == (ushort)293)
                index = 1;
            AppMain.GmDecoSetFrameMotion(0, index);
            eve_rec.pos_x = byte.MaxValue;
            eve_rec.byte_param[1] = (byte)1;
            return (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)AppMain.gmGmkDecoFrameMgrLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        AppMain.gmGmkDecoFrameMgrInit(obj_work);
        return obj_work;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkDecoFrameMgrLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_DECO_FRAME_MGR");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static void gmGmkDecoFrameMgrInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.disp_flag |= 32U;
        obj_work.move_flag |= 8448U;
        obj_work.flag |= 16U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkDecoFrameMgrMainFunc);
        obj_work.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.user_timer = (int)gmsEnemy3DWork.ene_com.eve_rec.byte_param[1] * 2;
    }

    private static void gmGmkDecoFrameMgrMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer >= 510)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            ++obj_work.user_timer;
            int index = 0;
            AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
            if (gmsEnemy3DWork.ene_com.eve_rec.id == (ushort)293)
                index = 1;
            AppMain.GmDecoSetFrameMotion(obj_work.user_timer, index);
            gmsEnemy3DWork.ene_com.eve_rec.byte_param[1] = (byte)(obj_work.user_timer / 2);
        }
    }

}