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
    public static int GME_BOSS4_LIFE_H
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<int>(3, 4);
        }
    }

    public static int GME_BOSS4_LIFE_L
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<int>(1, 2);
        }
    }

    public static uint GMD_BOSS4_CHIBI_LIFE_TIME
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(300f);
        }
    }

    public static float GMD_BOSS4_CHIBI_HIT_CHIBI_ADDSPD
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(1f);
        }
    }

    public static float GMD_BOSS4_CHIBI_SPD_LIMIT
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(2f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_Y_NORMAL
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-4.5f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_Y_BOUND
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-4.5f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_Y_SPEED
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-4.5f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_Y_BIG
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-3.5f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_Y_IRON
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-2f);
        }
    }

    public static float GMD_BOSS4_CHIBI_FALL_SPD_NORMAL
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(0.1f);
        }
    }

    public static float GMD_BOSS4_CHIBI_FALL_SPD_BOUND
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(0.1f);
        }
    }

    public static float GMD_BOSS4_CHIBI_FALL_SPD_SPEED
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(0.2f);
        }
    }

    public static float GMD_BOSS4_CHIBI_FALL_SPD_BIG
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(0.1f);
        }
    }

    public static float GMD_BOSS4_CHIBI_FALL_SPD_IRON
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(0.18f);
        }
    }

    public static int GMD_BOSS4_CHIBI_BOUND_FRAME
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_TIME(10f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_MULTI_SPD_X
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(0.8f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_ADD_SPD_X
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(1.5f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_X_BOUND
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-1.2f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_X_SPEED
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-2.5f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_X_BIG
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-2f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_X_IRON
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-2f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_OUT_Y
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_ZOOM(1.04f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_OUT_X
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_ZOOM(0.95f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_IN_Y
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_ZOOM(0.9f);
        }
    }

    public static float GMD_BOSS4_CHIBI_BOUND_IN_X
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_ZOOM(1.15f);
        }
    }

    public static float GMD_BOSS4_CHIBI_EXPLOSION_BASE_HIDE_TIME
    {
        get
        {
            return (float)AppMain.GMM_BOSS4_PAL_TIME(2f);
        }
    }

    public static float GMD_BOSS4_CHIBI_EXPLOSION_TASK_KILL_TIME
    {
        get
        {
            return (float)AppMain.GMM_BOSS4_PAL_TIME(60f);
        }
    }

    public static void SET_FLAG(uint f, AppMain.GMS_BOSS4_CHIBI_WORK w)
    {
        w.flag |= f;
    }

    public static void RESET_FLAG(uint f, AppMain.GMS_BOSS4_CHIBI_WORK w)
    {
        w.flag &= ~f;
    }

    public static bool IS_FLAG(uint f, AppMain.GMS_BOSS4_CHIBI_WORK w)
    {
        return 0 != ((int)w.flag & (int)f);
    }

    private static int gmBoss4ChibiGetAttackType(int life)
    {
        AppMain.UNREFERENCED_PARAMETER((object)life);
        AppMain.gmBoss4ChibiGetAttackTypeStatics._index %= 20;
        int num;
        if (AppMain.GmBoss4GetLife() >= AppMain.GME_BOSS4_LIFE_H)
        {
            if (!AppMain.GmBoss4CheckBossRush())
                return 1;
            num = AppMain.gmBoss4ChibiGetAttackTypeStatics.life3_tbl_f[AppMain.gmBoss4ChibiGetAttackTypeStatics._index];
        }
        else
            num = !AppMain.GmBoss4CheckBossRush() ? (AppMain.GmBoss4GetLife() >= AppMain.GME_BOSS4_LIFE_H || AppMain.GmBoss4GetLife() <= AppMain.GME_BOSS4_LIFE_L ? AppMain.gmBoss4ChibiGetAttackTypeStatics.life1_tbl[AppMain.gmBoss4ChibiGetAttackTypeStatics._index] : AppMain.gmBoss4ChibiGetAttackTypeStatics.life2_tbl[AppMain.gmBoss4ChibiGetAttackTypeStatics._index]) : (AppMain.GmBoss4GetLife() >= AppMain.GME_BOSS4_LIFE_H || AppMain.GmBoss4GetLife() <= AppMain.GME_BOSS4_LIFE_L ? AppMain.gmBoss4ChibiGetAttackTypeStatics.life1_tbl_f[AppMain.gmBoss4ChibiGetAttackTypeStatics._index] : AppMain.gmBoss4ChibiGetAttackTypeStatics.life2_tbl_f[AppMain.gmBoss4ChibiGetAttackTypeStatics._index]);
        ++AppMain.gmBoss4ChibiGetAttackTypeStatics._index;
        switch (num)
        {
            case 1:
                return 2;
            case 2:
                return 3;
            case 3:
                return 4;
            default:
                return 1;
        }
    }

    private static int gmBoss4ChibiGetThrowType()
    {
        return AppMain.gmBoss4ChibiGetThrowType(1);
    }

    private static int gmBoss4ChibiGetThrowType(int t)
    {
        AppMain.gmBoss4ChibiGetThrowTypeStatics._index %= 20;
        int num = AppMain.gmBoss4ChibiGetThrowTypeStatics._tbl[AppMain.gmBoss4ChibiGetThrowTypeStatics._index];
        ++AppMain.gmBoss4ChibiGetThrowTypeStatics._index;
        return num;
    }

    private static void GmBoss4ChibiBuild()
    {
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(732), 4, AppMain.GMD_BOSS4_ARC);
    }

    private static void GmBoss4ChibiFlush()
    {
        AppMain.ObjDataRelease(AppMain.ObjDataGet(732));
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4ChibiInit1st(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        return AppMain.GmBoss4ChibiInit(eve_rec, pos_x, pos_y + 65536, 0);
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4ChibiInit2nd(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        return AppMain.GmBoss4ChibiInit(eve_rec, pos_x, pos_y + 65536, 1);
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4ChibiInit2ndSpeed(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        return AppMain.GmBoss4ChibiInit(eve_rec, pos_x, pos_y + 65536, 2);
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4ChibiInit2ndBig(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        return AppMain.GmBoss4ChibiInit(eve_rec, pos_x, pos_y + 65536, 3);
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4ChibiInit2ndIron(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        return AppMain.GmBoss4ChibiInit(eve_rec, pos_x, pos_y + 65536, 4);
    }

    private static void GmBoss4ChibiSetInvincible(bool flg)
    {
        AppMain.gm_chibi_inv_flag = flg;
    }

    private static void GmBoss4ChibiExplosion()
    {
        AppMain.gm_chibi_exp_flag = true;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4ChibiInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS4_CHIBI_WORK()), "BOSS4_C.E");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS4_CHIBI_WORK w = (AppMain.GMS_BOSS4_CHIBI_WORK)work;
        w.type = type;
        switch (w.type)
        {
            case 2:
                AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss4GetObj3D(4), gmsEnemy3DWork.obj_3d);
                break;
            case 3:
                AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss4GetObj3D(5), gmsEnemy3DWork.obj_3d);
                break;
            case 4:
                AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss4GetObj3D(6), gmsEnemy3DWork.obj_3d);
                break;
            default:
                AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss4GetObj3D(3), gmsEnemy3DWork.obj_3d);
                break;
        }
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(732), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.disp_flag |= 134217728U;
        work.flag |= 16U;
        work.disp_flag |= 4194304U;
        work.move_flag &= 4294963199U;
        work.move_flag |= 128U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        AppMain.ObjObjectFieldRectSet(work, (short)-20, (short)-44, (short)20, (short)-4);
        AppMain.T_FUNC(new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4ChibiWaitLoad), work);
        AppMain.gm_chibi_exp_flag = false;
        AppMain.RESET_FLAG(536870912U, w);
        if (w.type != 0)
            AppMain.SET_FLAG(536870912U, w);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss4ChibiExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss4ChibiAtkHitFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        AppMain.GMS_BOSS4_BODY_WORK parentObj1 = (AppMain.GMS_BOSS4_BODY_WORK)my_rect.parent_obj.parent_obj;
        AppMain.GMS_BOSS4_CHIBI_WORK parentObj2 = (AppMain.GMS_BOSS4_CHIBI_WORK)my_rect.parent_obj;
        parentObj1.flag[0] |= 268435456U;
        AppMain.GmEnemyDefaultAtkFunc(my_rect, your_rect);
        if (parentObj2.type != 0)
        {
            AppMain.GMS_PLAYER_WORK playerObj = (AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj();
            if (AppMain.GmPlayerKeyCheckWalkLeft(playerObj))
                AppMain.GmPlayerSetReverse(playerObj);
        }
        AppMain.SET_FLAG(1073741824U, parentObj2);
    }

    private static void gmBoss4ChibiDefHitFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        AppMain.GMS_BOSS4_CHIBI_WORK parentObj1 = (AppMain.GMS_BOSS4_CHIBI_WORK)my_rect.parent_obj;
        AppMain.GMS_BOSS4_CHIBI_WORK parentObj2 = (AppMain.GMS_BOSS4_CHIBI_WORK)your_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK obsObjectWork1 = (AppMain.OBS_OBJECT_WORK)parentObj1;
        AppMain.OBS_OBJECT_WORK obsObjectWork2 = (AppMain.OBS_OBJECT_WORK)parentObj2;
        if (!AppMain.IS_FLAG(536870912U, parentObj1) || !AppMain.IS_FLAG(536870912U, parentObj2))
            return;
        if (obsObjectWork1.pos.x > obsObjectWork2.pos.x)
        {
            obsObjectWork1.pos.x += AppMain.FX_F32_TO_FX32(4f);
            obsObjectWork1.spd.x += AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_HIT_CHIBI_ADDSPD);
        }
        if (obsObjectWork1.pos.x < obsObjectWork2.pos.x)
        {
            obsObjectWork1.pos.x -= AppMain.FX_F32_TO_FX32(4f);
            obsObjectWork1.spd.x -= AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_HIT_CHIBI_ADDSPD);
        }
        if (obsObjectWork1.pos.x == obsObjectWork2.pos.x)
        {
            // BUGBUG
            if (obsObjectWork1.pos.y < obsObjectWork2.pos.y)
            {
                obsObjectWork1.pos.x += AppMain.FX_F32_TO_FX32(4f);
                obsObjectWork1.spd.x += AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_HIT_CHIBI_ADDSPD);
            }
            else
            {
                obsObjectWork1.pos.x -= AppMain.FX_F32_TO_FX32(4f);
                obsObjectWork1.spd.x -= AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_HIT_CHIBI_ADDSPD);
            }
        }
        if (obsObjectWork1.spd.x > AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_SPD_LIMIT))
            obsObjectWork1.spd.x = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_SPD_LIMIT);
        if (obsObjectWork1.spd.x >= -AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_SPD_LIMIT))
            return;
        obsObjectWork1.spd.x = -AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_SPD_LIMIT);
    }

    private static void gmBoss4ChibiWaitLoad(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_CHIBI_WORK chibi = (AppMain.GMS_BOSS4_CHIBI_WORK)obj_work;
        if (!AppMain.GmBoss4IsBuilded())
            return;
        AppMain.T_FUNC(new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4ChibiMain), obj_work);
        AppMain.GmBoss4UtilInit1ShotTimer(chibi.timer, AppMain.GMD_BOSS4_CHIBI_LIFE_TIME);
        AppMain.GmBoss4UtilInitFlicker(obj_work, chibi.flk_work, 1, 180, 4, (int)(chibi.timer.timer / 20U) * 3, AppMain.gm_boss4_color_red);
        chibi.count = -1;
        switch (chibi.type)
        {
            case 1:
                AppMain.GmBsCmnSetAction(obj_work, 0, 0);
                obj_work.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_Y_BOUND);
                break;
            case 2:
                AppMain.GmBsCmnSetAction(obj_work, 0, 0);
                obj_work.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_Y_SPEED);
                break;
            case 3:
                AppMain.GmBsCmnSetAction(obj_work, 0, 0);
                obj_work.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_Y_BIG);
                break;
            case 4:
                AppMain.GmBsCmnSetAction(obj_work, 0, 0);
                obj_work.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_Y_IRON);
                break;
            default:
                AppMain.GmBsCmnSetAction(obj_work, 0, 0);
                obj_work.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_Y_NORMAL);
                break;
        }
        obj_work.spd_fall = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_FALL_SPD_NORMAL);
        obj_work.pos.z = 131072;
        chibi.bound = 0;
        AppMain.GmBoss4UtilInitNodeMatrix(chibi.node_work, obj_work, 1);
        AppMain.GmBoss4UtilGetNodeMatrix(chibi.node_work, 0);
        AppMain.gmBoss4ChibiBoosterCreate(chibi);
        AppMain.GmSoundPlaySE("Boss4_01");
    }

    private static void gmBoss4ChibiExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_BOSS4_CHIBI_WORK tcbWork = (AppMain.GMS_BOSS4_CHIBI_WORK)AppMain.mtTaskGetTcbWork(tcb);
        AppMain.GmBoss4DecObjCreateCount();
        AppMain.GmBoss4UtilExitNodeMatrix(tcbWork.node_work);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss4ChibiMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_CHIBI_WORK w = (AppMain.GMS_BOSS4_CHIBI_WORK)obj_work;
        if (AppMain.GmBsCmnIsActionEnd(obj_work) != 0)
        {
            AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
            AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[1], (short)-10, (short)-22, (short)10, (short)-2);
            gmsEnemy3DWork.ene_com.rect_work[1].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss4ChibiAtkHitFunc);
            gmsEnemy3DWork.ene_com.rect_work[1].flag |= 33554432U;
            AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[2], (short)-14, (short)-26, (short)18, (short)2);
            gmsEnemy3DWork.ene_com.rect_work[2].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss4ChibiDefHitFunc);
            AppMain.ObjRectGroupSet(gmsEnemy3DWork.ene_com.rect_work[2], (byte)2, (byte)4);
            gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294965247U;
            gmsEnemy3DWork.ene_com.rect_work[2].flag |= 4U;
            switch (w.type)
            {
                case 3:
                    AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[1], (short)-30, (short)-60, (short)30, (short)0);
                    AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[2], (short)-44, (short)-64, (short)44, (short)4);
                    break;
            }
        }
        switch (w.type)
        {
            case 1:
                obj_work.move_flag &= 4294443007U;
                obj_work.spd_fall = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_FALL_SPD_BOUND);
                obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
                break;
            case 2:
                obj_work.move_flag &= 4294443007U;
                obj_work.spd_fall = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_FALL_SPD_SPEED);
                obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
                if (w.bound == 0)
                {
                    obj_work.pos.y += obj_work.spd.y;
                    break;
                }
                break;
            case 3:
                obj_work.move_flag &= 4294443007U;
                obj_work.spd_fall = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_FALL_SPD_BIG);
                obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
                break;
            case 4:
                obj_work.move_flag &= 4294443007U;
                obj_work.spd_fall = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_FALL_SPD_IRON);
                obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
                obj_work.pos.x -= AppMain.FX_F32_TO_FX32(4f);
                break;
            default:
                obj_work.spd_fall = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_FALL_SPD_NORMAL);
                break;
        }
        if (obj_work.scale.y < AppMain.FX_F32_TO_FX32(1f))
        {
            obj_work.scale.y = (int)((double)obj_work.scale.y * (double)AppMain.GMD_BOSS4_CHIBI_BOUND_OUT_Y);
            if (obj_work.scale.y > AppMain.FX_F32_TO_FX32(1f))
                obj_work.scale.y = AppMain.FX_F32_TO_FX32(1f);
        }
        if (obj_work.scale.x > AppMain.FX_F32_TO_FX32(1f))
        {
            obj_work.scale.x = (int)((double)obj_work.scale.x * (double)AppMain.GMD_BOSS4_CHIBI_BOUND_OUT_X);
            if (obj_work.scale.x < AppMain.FX_F32_TO_FX32(1f))
                obj_work.scale.x = AppMain.FX_F32_TO_FX32(1f);
        }
        if (AppMain.gm_chibi_exp_flag)
            AppMain.SET_FLAG(1073741824U, w);
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork1 = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (AppMain.gm_chibi_inv_flag)
            gmsEnemy3DWork1.ene_com.rect_work[1].flag |= 2048U;
        else
            gmsEnemy3DWork1.ene_com.rect_work[1].flag &= 4294965247U;
        if (((int)obj_work.move_flag & 1) != 0)
        {
            AppMain.SET_FLAG(536870912U, w);
            obj_work.move_flag &= 4294967294U;
            obj_work.move_flag &= 4294967167U;
            obj_work.move_flag |= 256U;
            w.bnd_xspd = obj_work.spd.x;
            obj_work.spd.y = 0;
            obj_work.spd.x = 0;
            obj_work.spd_fall = 0;
            w.bound = w.type != 4 ? AppMain.GMD_BOSS4_CHIBI_BOUND_FRAME : 1000;
            if (w.type == 3)
                AppMain.GmSoundPlaySE("Boss4_03");
            else if (w.type != 4)
                AppMain.GmSoundPlaySE("Boss4_02");
        }
        if (w.bound > 0)
        {
            if (--w.bound == 0)
            {
                obj_work.pos.y += AppMain.FX_F32_TO_FX32(-4f);
                obj_work.move_flag |= 128U;
                obj_work.move_flag &= 4294967039U;
                obj_work.move_flag &= 4294967294U;
                obj_work.spd.x = w.bnd_xspd;
                switch (w.type)
                {
                    case 1:
                        obj_work.spd.x = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_X_BOUND);
                        obj_work.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_Y_BOUND);
                        break;
                    case 2:
                        obj_work.spd.x = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_X_SPEED);
                        obj_work.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_Y_SPEED);
                        break;
                    case 3:
                        obj_work.spd.x = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_X_BIG);
                        obj_work.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_Y_BIG);
                        break;
                    case 4:
                        obj_work.spd.x = AppMain.FX_F32_TO_FX32(0.0);
                        obj_work.spd.y = AppMain.FX_F32_TO_FX32(0.0);
                        break;
                    default:
                        obj_work.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_SPD_Y_NORMAL);
                        obj_work.spd.x = AppMain.GmBsCmnGetPlayerObj().pos.x >= obj_work.pos.x ? (int)((double)obj_work.spd.x * (double)AppMain.GMD_BOSS4_CHIBI_BOUND_MULTI_SPD_X) + AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_ADD_SPD_X) : (int)((double)obj_work.spd.x * (double)AppMain.GMD_BOSS4_CHIBI_BOUND_MULTI_SPD_X) - AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_BOUND_ADD_SPD_X);
                        if (obj_work.spd.x > AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_SPD_LIMIT))
                            obj_work.spd.x = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_SPD_LIMIT);
                        if (obj_work.spd.x < -AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_SPD_LIMIT))
                        {
                            obj_work.spd.x = -AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_CHIBI_SPD_LIMIT);
                            break;
                        }
                        break;
                }
            }
            else if (w.type != 4)
            {
                obj_work.scale.y = (int)((double)obj_work.scale.y * (double)AppMain.GMD_BOSS4_CHIBI_BOUND_IN_Y);
                obj_work.scale.x = (int)((double)obj_work.scale.x * (double)AppMain.GMD_BOSS4_CHIBI_BOUND_IN_X);
                obj_work.spd_fall = 0;
                if (obj_work.scale.y < AppMain.FX_F32_TO_FX32(0.6f))
                    obj_work.scale.y = AppMain.FX_F32_TO_FX32(0.6f);
                if (obj_work.scale.x > AppMain.FX_F32_TO_FX32(1.5f))
                    obj_work.scale.x = AppMain.FX_F32_TO_FX32(1.5f);
            }
        }
        if (w.type != 4)
            AppMain.GmBoss4UtilLookAtPlayer(w.dir, obj_work, 5);
        if (AppMain.GmBoss4UtilUpdate1ShotTimer(w.timer))
            AppMain.SET_FLAG(1073741824U, w);
        if (w.type == 0 && AppMain.GmBoss4UtilUpdateFlicker(obj_work, w.flk_work))
        {
            int start = (int)(w.timer.timer / 20U) * 3;
            AppMain.GmBoss4UtilInitFlicker(obj_work, w.flk_work, 1, start, 2, 0, AppMain.gm_boss4_color_red);
        }
        if (AppMain.IS_FLAG(1073741824U, w))
        {
            AppMain.RESET_FLAG(1073741824U, w);
            int id;
            switch (w.type)
            {
                case 2:
                    id = 737;
                    break;
                case 3:
                    id = 738;
                    break;
                default:
                    id = 736;
                    break;
            }
            AppMain.VecFx32 pos = obj_work.pos;
            pos.y += AppMain.FX_F32_TO_FX32(-16f);
            pos.z = 135168;
            AppMain.GmBoss4EffCommonInit(id, new AppMain.VecFx32?(pos));
            AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork2 = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
            gmsEnemy3DWork2.ene_com.rect_work[1].flag &= 4294967291U;
            gmsEnemy3DWork2.ene_com.rect_work[2].flag &= 4294967291U;
            obj_work.spd_fall = 0;
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
            obj_work.move_flag &= 4294967294U;
            obj_work.move_flag |= 256U;
            obj_work.move_flag |= 256U;
            w.wait = 0;
            AppMain.T_FUNC(new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4ChibiBomb), obj_work);
            AppMain.GmSoundPlaySE("Boss4_04");
        }
        AppMain.GmBoss4UtilUpdateDirection(w.dir, obj_work, true);
    }

    private static void gmBoss4ChibiBomb(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_CHIBI_WORK gmsBosS4ChibiWork = (AppMain.GMS_BOSS4_CHIBI_WORK)obj_work;
        obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
        ++gmsBosS4ChibiWork.wait;
        if (gmsBosS4ChibiWork.wait >= 2)
            obj_work.disp_flag |= 32U;
        if (gmsBosS4ChibiWork.wait < 60)
            return;
        AppMain.GMM_BS_OBJ((object)gmsBosS4ChibiWork).flag |= 8U;
    }

    private static void gmBoss4ChibiFuncBoost(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_CHIBI_WORK parentObj1 = (AppMain.GMS_BOSS4_CHIBI_WORK)obj_work.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = obj_work.parent_obj;
        AppMain.MTM_ASSERT((int)parentObj1.node_work.snm_work.reg_node_max);
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj2.disp_flag & 32) != 0)
            AppMain.gmBoss4ChibiBoosterDelete(parentObj1);
        obj_work.disp_flag &= 4294967263U;
        if ((int)parentObj1.dir.cur_angle < (int)AppMain.AKM_DEGtoA16(50) && (int)parentObj1.dir.cur_angle > (int)AppMain.AKM_DEGtoA16(-50))
            obj_work.disp_flag |= 32U;
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj1.node_work.snm_work, parentObj1.node_work.work[0], 1);
        obj_work.disp_flag &= 4294963199U;
        if (((int)AppMain.g_obj.flag & 1) != 0)
        {
            obj_work.disp_flag |= 4096U;
        }
        else
        {
            obj_work.pos.x += parentObj2.spd.x;
            obj_work.pos.y += parentObj2.spd.y;
            obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
        }
    }

    private static void gmBoss4ChibiBoosterCreate(AppMain.GMS_BOSS4_CHIBI_WORK chibi)
    {
    }

    private static void gmBoss4ChibiBoosterDelete(AppMain.GMS_BOSS4_CHIBI_WORK chibi)
    {
        if (chibi.boost == null)
            return;
        AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)chibi.boost);
        chibi.boost = (AppMain.GMS_EFFECT_3DES_WORK)null;
    }

}