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
    public static AppMain.OBS_OBJECT_WORK GmGmkBubbleManagerInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkBubbleLoadObjNoModel(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        AppMain.gmGmkBubbleManagerInit(objWork);
        AppMain.gmGmkBubbleSetUserWorkIntervalNormal(objWork, (ushort)((uint)eve_rec.left * 60U));
        return objWork;
    }

    public static AppMain.GMS_ENEMY_3D_WORK gmGmkBubbleLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_BUBBLE");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    public static ushort gmGmkBubbleGameSystemGetWaterLevel()
    {
        return AppMain.g_gm_main_system.water_level;
    }

    public static void gmGmkBubbleManagerInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 8448U;
        AppMain.gmGmkBubbleSetUserTimerCounter(obj_work, 0U);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBubbleManagerMainWait);
        AppMain.GmEfctZoneEsCreate(obj_work, 2, 4).efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBubbleManagerEffectMain);
    }

    public static void gmGmkBubbleManagerMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if ((int)AppMain.gmGmkBubbleGameSystemGetWaterLevel() * 4096 > obj_work.pos.y)
            return;
        if ((uint)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.top * 60U < AppMain.gmGmkBubbleGetUserTimerCounter(obj_work))
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBubbleManagerMain);
        AppMain.gmGmkBubbleAddUserTimerCounter(obj_work, 1);
    }

    public static void gmGmkBubbleManagerMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if ((int)AppMain.gmGmkBubbleGameSystemGetWaterLevel() * 4096 > obj_work.pos.y)
            return;
        uint num = (uint)AppMain.gmGmkBubbleGetUserWorkIntervalNormal(obj_work);
        if (num == 0U)
            num = 60U;
        if (AppMain.gmGmkBubbleGetUserTimerCounter(obj_work) % num == 0U)
            AppMain.gmGmkBubbleInit(AppMain.GmEfctZoneEsCreate(obj_work, 2, 1));
        AppMain.gmGmkBubbleAddUserTimerCounter(obj_work, 1);
    }

    public static void gmGmkBubbleManagerEffectMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if ((int)AppMain.gmGmkBubbleGameSystemGetWaterLevel() * 4096 > obj_work.pos.y)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
    }

    public static void gmGmkBubbleInit(AppMain.GMS_EFFECT_3DES_WORK effect_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)effect_work;
        obsObjectWork.flag &= 4294967293U;
        obsObjectWork.move_flag &= 4294958847U;
        obsObjectWork.move_flag |= 4608U;
        obsObjectWork.spd.y = (int)-AppMain.GMD_GMK_BUBBLE_SPEED_Y;
        obsObjectWork.pos.z = 1048576;
        AppMain.OBS_RECT_WORK[] rectWork = effect_work.efct_com.rect_work;
        AppMain.GmEffectRectInit(effect_work.efct_com, AppMain.gm_gmk_bubble_table_atk, AppMain.gm_gmk_bubble_table_def, (byte)1, (byte)1);
        AppMain.ObjRectWorkSet(rectWork[0], (short)-8, (short)7, (short)8, (short)8);
        rectWork[0].flag |= 1028U;
        rectWork[0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkBubbleDefFunc);
        rectWork[1].flag |= 3072U;
        obsObjectWork.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBubbleMainMoveLeft);
    }

    public static void gmGmkBubbleDefFunc(
      AppMain.OBS_RECT_WORK own_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = own_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != (ushort)1)
            return;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)parentObj2;
        AppMain.GmPlySeqInitBreathing(ply_work);
        AppMain.GmPlayerBreathingSet(ply_work);
        parentObj1.flag |= 4U;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, 2, 3);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj1.pos.x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj1.pos.y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = parentObj1.pos.z;
        AppMain.GMM_PAD_VIB_SMALL();
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBubbleMainHit);
    }

    public static void gmGmkBubbleMainMoveLeft(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.x = AppMain.gmGmkBubbleAddUserWorkSpeedX(obj_work, (int)-AppMain.GMD_GMK_BUBBLE_SPEED_X_ADD);
        if ((long)obj_work.spd.x < -AppMain.GMD_GMK_BUBBLE_SPEED_X_MAX)
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBubbleMainMoveRight);
        if ((int)AppMain.gmGmkBubbleGameSystemGetWaterLevel() * 4096 <= obj_work.pos.y)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBubbleMainEnd);
    }

    public static void gmGmkBubbleMainMoveRight(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.spd.x = AppMain.gmGmkBubbleAddUserWorkSpeedX(obj_work, (int)AppMain.GMD_GMK_BUBBLE_SPEED_X_ADD);
        if ((long)obj_work.spd.x > AppMain.GMD_GMK_BUBBLE_SPEED_X_MAX)
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBubbleMainMoveLeft);
        if ((int)AppMain.gmGmkBubbleGameSystemGetWaterLevel() * 4096 <= obj_work.pos.y)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBubbleMainEnd);
    }

    public static void gmGmkBubbleMainHit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        ++obj_work.user_timer;
        int num1 = AppMain.GMD_GMK_BUBBLE_FRAME_HIT_DELETE - obj_work.user_timer;
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        if (num1 < AppMain.GMD_GMK_BUBBLE_HIT_EFFECT_NUM)
            AppMain.GmPlyEfctCreateBubble(ply_work);
        if (num1 > 0)
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)ply_work;
            int num2 = obsObjectWork.pos.x - obj_work.pos.x;
            int num3 = obsObjectWork.pos.y - AppMain.GMD_GMK_BUBBLE_OFFSET_Y * 4096 - obj_work.pos.y;
            int num4 = ((int)obsObjectWork.disp_flag & 1) == 0 ? num2 + AppMain.GMD_GMK_BUBBLE_OFFSET_X * 4096 : num2 - AppMain.GMD_GMK_BUBBLE_OFFSET_X * 4096;
            obj_work.spd.x = num4 / num1;
            obj_work.spd.y = num3 / num1;
        }
        else
        {
            obj_work.user_timer = 0;
            obj_work.flag |= 4U;
        }
    }

    public static void gmGmkBubbleMainEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag |= 4U;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, 2, 2);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = obj_work.pos.x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = obj_work.pos.y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = obj_work.pos.z;
    }

    public static void gmGmkBubbleSetUserWorkIntervalNormal(
      AppMain.OBS_OBJECT_WORK obj_work,
      ushort interval)
    {
        obj_work.user_work |= (uint)interval << 16;
    }

    public static ushort gmGmkBubbleGetUserWorkIntervalNormal(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return (ushort)(obj_work.user_work >> 16);
    }

    public static void gmGmkBubbleSetUserTimerCounter(AppMain.OBS_OBJECT_WORK obj_work, uint count)
    {
        obj_work.user_timer = (int)count;
    }

    public static void gmGmkBubbleAddUserTimerCounter(AppMain.OBS_OBJECT_WORK obj_work, int count)
    {
        obj_work.user_timer += count;
    }

    public static uint gmGmkBubbleGetUserTimerCounter(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return (uint)obj_work.user_timer;
    }

    public static int gmGmkBubbleAddUserWorkSpeedX(AppMain.OBS_OBJECT_WORK obj_work, int speed)
    {
        obj_work.user_work += (uint)speed;
        return (int)obj_work.user_work;
    }

}