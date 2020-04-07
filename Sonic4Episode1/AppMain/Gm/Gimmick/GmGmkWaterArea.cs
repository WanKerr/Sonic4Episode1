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
    private static AppMain.OBS_OBJECT_WORK GmGmkWaterAreaInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        ushort water_level = (ushort)((uint)eve_rec.left * 100U + (uint)eve_rec.top);
        ushort time = 0;
        ushort flag = eve_rec.flag;
        for (ushort index = 0; (ushort)10 > index; ++index)
        {
            if (((int)flag & 1) != 0)
                time += (ushort)((uint)index + 1U);
            flag >>= 1;
        }
        if (AppMain.gmGmkWaterAreaGetType(eve_rec) == 0U)
        {
            if (AppMain.gmGmkWaterAreaCheckRestart(pos_x, pos_y))
                AppMain.GmWaterSurfaceRequestChangeWaterLevel(water_level, (ushort)((uint)time * 60U), false);
            eve_rec.pos_x = byte.MaxValue;
            return (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.GMS_ENEMY_3D_WORK gimmick_work = AppMain.gmGmkWaterAreaLoadObj(eve_rec, pos_x, pos_y, type);
        AppMain.gmGmkWaterAreaInit(gimmick_work, water_level, time);
        return (AppMain.OBS_OBJECT_WORK)gimmick_work;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkWaterAreaLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_WATER_AREA");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static void gmGmkWaterAreaInit(
      AppMain.GMS_ENEMY_3D_WORK gimmick_work,
      ushort water_level,
      ushort time)
    {
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)gimmick_work;
        AppMain.GMS_EVE_RECORD_EVENT eveRec = gimmick_work.ene_com.eve_rec;
        uint type = AppMain.gmGmkWaterAreaGetType(eveRec);
        byte width = eveRec.width;
        byte height = eveRec.height;
        AppMain.gmGmkWaterAreaSetRect(gimmick_work, width, height, type);
        gimmick_work.ene_com.target_obj = AppMain.g_gm_main_system.ply_work[0].obj_work;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 32U;
        AppMain.gmGmkWaterAreaUserWorkSetLevel(obj_work, water_level);
        AppMain.gmGmkWaterAreaUserWorkSetTime(obj_work, time);
    }

    private static bool gmGmkWaterAreaCheckRestart(int pos_x, int pos_y)
    {
        int num = AppMain.MTM_MATH_ABS(AppMain.g_gm_main_system.resume_pos_x - pos_x);
        return AppMain.MTM_MATH_ABS(AppMain.g_gm_main_system.resume_pos_y - pos_y) <= 524288 && num <= 524288;
    }

    private static uint gmGmkWaterAreaGetType(AppMain.GMS_EVE_RECORD_EVENT eve_rec)
    {
        uint num = uint.MaxValue;
        switch (eve_rec.id)
        {
            case 102:
                num = 1U;
                break;
            case 103:
                num = 2U;
                break;
            case 104:
                num = 3U;
                break;
            case 105:
                num = 4U;
                break;
            case 106:
                num = 0U;
                break;
        }
        return num;
    }

    private static void gmGmkWaterAreaRequestChangeWatarLevel(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GmWaterSurfaceRequestChangeWaterLevel(AppMain.gmGmkWaterAreaUserWorkGetLevel(obj_work), (ushort)((uint)AppMain.gmGmkWaterAreaUserWorkGetTime(obj_work) * 60U), false);
    }

    private static void gmGmkWaterAreaSetRect(
      AppMain.GMS_ENEMY_3D_WORK gimmick_work,
      byte width,
      byte height,
      uint type)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        if (width < (byte)34)
            width = (byte)34;
        if (height < (byte)34)
            height = (byte)34;
        AppMain.ObjRectWorkZSet(pRec, (short)((int)-width / 2), (short)((int)-height / 2), (short)-500, (short)((int)width / 2), (short)((int)height / 2), (short)500);
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectDefSet(pRec, (ushort)0, (short)0);
        switch (type)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                pRec.flag |= 1024U;
                pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkWaterAreaDefFuncDelay);
                break;
        }
    }

    private static bool gmGmkWaterAreaCheckDir(
      AppMain.OBS_OBJECT_WORK gimmick_obj_work,
      AppMain.OBS_OBJECT_WORK player_obj_work,
      uint type)
    {
        bool flag = false;
        switch (type)
        {
            case 0:
                flag = true;
                break;
            case 1:
                if (player_obj_work.pos.x < gimmick_obj_work.pos.x)
                {
                    flag = true;
                    break;
                }
                break;
            case 2:
                if (gimmick_obj_work.pos.x < player_obj_work.pos.x)
                {
                    flag = true;
                    break;
                }
                break;
            case 3:
                if (player_obj_work.pos.y < gimmick_obj_work.pos.y)
                {
                    flag = true;
                    break;
                }
                break;
            case 4:
                if (gimmick_obj_work.pos.y < player_obj_work.pos.y)
                {
                    flag = true;
                    break;
                }
                break;
        }
        return flag;
    }

    private static void gmGmkWaterAreaDefFuncDelay(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = own_rect.parent_obj;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)parentObj1;
        AppMain.OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (!AppMain.gmGmkWaterAreaModeCheckWait(parentObj1))
            return;
        uint type = AppMain.gmGmkWaterAreaGetType(gmsEnemy3DWork.ene_com.eve_rec);
        if (!AppMain.gmGmkWaterAreaCheckDir(parentObj1, parentObj2, type))
            return;
        AppMain.gmGmkWaterAreaModeChangeLady(parentObj1);
    }

    private static bool gmGmkWaterAreaModeCheckWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return obj_work.ppFunc == null;
    }

    private static void gmGmkWaterAreaModeChangeWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag &= 4294967279U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmGmkWaterAreaModeChangeLady(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag &= 4294967279U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkWaterAreaMainLady);
    }

    private static void gmGmkWaterAreaModeChangeActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkWaterAreaRequestChangeWatarLevel(obj_work);
        obj_work.flag |= 16U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkWaterAreaMainActive);
        AppMain.gmGmkWaterAreaUserTimerSetCounter(obj_work, 0);
        AppMain.gmGmkWaterAreaRequestChangeWatarLevel(obj_work);
        AppMain.GmWaterSurfaceSetFlagDraw(true);
    }

    private static void gmGmkWaterAreaMainLady(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (((int)gmsEnemy3DWork.ene_com.rect_work[2].flag & 131072) != 0)
            return;
        uint type = AppMain.gmGmkWaterAreaGetType(gmsEnemy3DWork.ene_com.eve_rec);
        AppMain.OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        if (AppMain.gmGmkWaterAreaCheckDir(obj_work, targetObj, type))
            AppMain.gmGmkWaterAreaModeChangeWait(obj_work);
        else
            AppMain.gmGmkWaterAreaModeChangeActive(obj_work);
    }

    private static void gmGmkWaterAreaMainActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int time = (int)AppMain.gmGmkWaterAreaUserWorkGetTime(obj_work);
        int counter = AppMain.gmGmkWaterAreaUserTimerGetCounter(obj_work);
        AppMain.gmGmkWaterAreaUserTimerAddCounter(obj_work, 1);
        if (counter < time * 60)
            return;
        AppMain.gmGmkWaterAreaModeChangeWait(obj_work);
    }

    private static void gmGmkWaterAreaUserWorkSetLevel(AppMain.OBS_OBJECT_WORK obj_work, ushort level)
    {
        obj_work.user_work |= (uint)level << 16;
    }

    private static ushort gmGmkWaterAreaUserWorkGetLevel(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return (ushort)(obj_work.user_work >> 16);
    }

    private static void gmGmkWaterAreaUserWorkSetTime(AppMain.OBS_OBJECT_WORK obj_work, ushort time)
    {
        obj_work.user_work &= 4294901760U;
        obj_work.user_work |= (uint)time;
    }

    private static ushort gmGmkWaterAreaUserWorkGetTime(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return (ushort)obj_work.user_work;
    }

    private static void gmGmkWaterAreaUserTimerSetCounter(AppMain.OBS_OBJECT_WORK obj_work, int time)
    {
        obj_work.user_timer = time;
    }

    private static void gmGmkWaterAreaUserTimerAddCounter(AppMain.OBS_OBJECT_WORK obj_work, int time)
    {
        obj_work.user_timer += time;
    }

    private static int gmGmkWaterAreaUserTimerGetCounter(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_timer;
    }

}