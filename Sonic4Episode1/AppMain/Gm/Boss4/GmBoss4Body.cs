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
    public static float GMD_BOSS4_BODY_START_POS_Y
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<float>(-120f, 1480f);
        }
    }

    public static float GMD_BOSS4_BODY_END_POS_Y
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<float>(280f, 1880f);
        }
    }

    public static int GMD_BOSS4_SPEED_TIMES_IN_DAMAGE
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_TIME(5f);
        }
    }

    public static float GMD_BOSS4_BODY_PRE_ATKNML_SPD_ADD
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(0.02f);
        }
    }

    public static int GMD_BOSS4_BODY_PRE_ATKNML_SPD_MAX_ABS
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_TIME(1f);
        }
    }

    public static int GMD_BOSS4_BODY_ATKNML_MOVE_FRAME
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_TIME(720f);
        }
    }

    public static int GMD_BOSS4_BODY_ATKNML_DRIFT_FRAME
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_TIME(30f);
        }
    }

    public static int GMD_BOSS4_BODY_ATKNML_TURN_FRAME
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_TIME(28f);
        }
    }

    public static int GMD_BOSS4_BODY_2ND_POS_X
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<int>(3500, 12100);
        }
    }

    public static int GMD_BOSS4_BODY_2ND_POS_Y
    {
        get
        {
            return AppMain.GMM_BOSS4_STAGE<int>(250, 1850);
        }
    }

    public static uint GMD_BOSS4_BODY_SONIC_CTRL_TIME
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(240f);
        }
    }

    public static uint GMD_BOSS4_BODY_CREATE_CAP_FIRST_TIME
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(120f);
        }
    }

    public static float GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_1
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-1f);
        }
    }

    public static float GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_1
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-2f);
        }
    }

    public static float GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_2
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-2f);
        }
    }

    public static float GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_2
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(-2f);
        }
    }

    public static uint GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_3
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(300f);
        }
    }

    public static uint GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_2
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(180f);
        }
    }

    public static uint GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_1
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(120f);
        }
    }

    public static uint GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_2_2
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(90f);
        }
    }

    public static uint GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_TIME
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(120f);
        }
    }

    public static uint GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_INTERVAL_MIN_TIME
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(10f);
        }
    }

    public static uint GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_INTERVAL_MAX_TIME
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(30f);
        }
    }

    public static uint GMD_BOSS4_BODY_DEFEAT_BOMB_PARTS_INTERVAL_MIN_TIME
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(10f);
        }
    }

    public static uint GMD_BOSS4_BODY_DEFEAT_BOMB_PARTS_INTERVAL_MAX_TIME
    {
        get
        {
            return (uint)AppMain.GMM_BOSS4_PAL_TIME(30f);
        }
    }

    public static AppMain.GMS_BOSS4_MGR_WORK GMM_BOSS4_MGR(AppMain.GMS_BOSS4_BODY_WORK work)
    {
        return work.mgr_work;
    }

    private static void GmBoss4BodyBuild()
    {
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(730), 2, AppMain.GMD_BOSS4_ARC);
    }

    private static void GmBoss4BodyFlush()
    {
        AppMain.ObjDataRelease(AppMain.ObjDataGet(730));
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4BodyInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS4_BODY_WORK()), "BOSS4_BODY");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS4_BODY_WORK body_work = (AppMain.GMS_BOSS4_BODY_WORK)work;
        work.pos.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_START_POS_Y);
        work.pos.z = -131072;
        body_work.atk_nml_alt = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_END_POS_Y);
        work.flag |= 16U;
        work.disp_flag |= 4194309U;
        work.move_flag |= 4096U;
        work.move_flag &= 4294967167U;
        gmsEnemy3DWork.ene_com.vit = (byte)1;
        AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[0], (short)-40, (short)-16, (short)40, (short)2);
        gmsEnemy3DWork.ene_com.rect_work[0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss4BodyDamageDefFunc);
        AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[1], (short)-32, (short)-8, (short)32, (short)40);
        gmsEnemy3DWork.ene_com.rect_work[1].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss4BodyAtkHitFunc);
        AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[2], (short)-30, (short)-18, (short)30, (short)40);
        gmsEnemy3DWork.ene_com.rect_work[2].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss4BodyDefHitFunc);
        AppMain.ObjRectGroupSet(gmsEnemy3DWork.ene_com.rect_work[2], (byte)1, (byte)1);
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294965247U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag |= 4U;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss4GetObj3D(0), gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(730), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.disp_flag |= 134217728U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4BodyWaitLoad);
        AppMain.gmBoss4BodyChangeState(body_work, 0);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4BodyOutFunc);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss4BodyExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss4BodyExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        AppMain.GMS_BOSS4_BODY_WORK gmsBosS4BodyWork = (AppMain.GMS_BOSS4_BODY_WORK)tcbWork;
        AppMain.GmBoss4DecObjCreateCount();
        AppMain.GmBoss4UtilExitNodeMatrix(gmsBosS4BodyWork.node_work);
        AppMain.GmBsCmnClearCNMCb(tcbWork);
        AppMain.GmBsCmnDeleteCNMMgrWork(gmsBosS4BodyWork.cnm_mgr_work);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss4BodySetActionWhole(AppMain.GMS_BOSS4_BODY_WORK body_work, int act_id)
    {
        AppMain.gmBoss4BodySetActionWhole(body_work, act_id, false);
    }

    private static void gmBoss4BodySetActionWhole(
      AppMain.GMS_BOSS4_BODY_WORK body_work,
      int act_id,
      bool force_change)
    {
        AppMain.GMS_BOSS4_PART_ACT_INFO[] bosS4PartActInfoArray = AppMain.gm_boss4_act_id_tbl[act_id];
        if (!force_change && body_work.whole_act_id == act_id)
            return;
        body_work.whole_act_id = act_id;
        for (int index = 0; index < 2; ++index)
        {
            if (body_work.parts_objs[index] != null)
            {
                if (index == 1)
                {
                    AppMain.GMS_BOSS4_EGG_WORK partsObj = (AppMain.GMS_BOSS4_EGG_WORK)body_work.parts_objs[index];
                    body_work.egg_revert_mtn_id = act_id;
                    if (AppMain.GmBoss4GetActInfo(body_work.egg_revert_mtn_id, 1).act_id == (ushort)4)
                        body_work.egg_revert_mtn_id = !AppMain.GmBoss4Is2ndStage() ? 3 : 5;
                    if (((int)partsObj.flag & 1) != 0)
                        continue;
                }
                if (bosS4PartActInfoArray[index].is_maintain == (byte)0)
                    AppMain.GmBsCmnSetAction(body_work.parts_objs[index], (int)bosS4PartActInfoArray[index].act_id, (int)bosS4PartActInfoArray[index].is_repeat, bosS4PartActInfoArray[index].is_blend);
                else if (bosS4PartActInfoArray[index].is_repeat != (byte)0)
                    AppMain.GMM_BS_OBJ((object)body_work).disp_flag |= 4U;
                body_work.parts_objs[index].obj_3d.speed[0] = bosS4PartActInfoArray[index].mtn_spd;
                body_work.parts_objs[index].obj_3d.blend_spd = bosS4PartActInfoArray[index].blend_spd;
            }
        }
    }

    private static void gmBoss4BodyUpdateSuspendAction(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
    }

    private static void gmBoss4BodyExecDamageRoutine(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GMS_BOSS4_MGR_WORK mgrWork = body_work.mgr_work;
        AppMain.MTM_ASSERT((object)mgrWork);
        if (body_work.damage_timer > 0)
            return;
        if (mgrWork.life != 0)
            --mgrWork.life;
        if (0 < mgrWork.life)
        {
            body_work.flag[0] |= 1073741824U;
            if (1 >= mgrWork.life)
                AppMain.GmDecoStartLoop();
        }
        else
            body_work.flag[0] |= 2147483648U;
        body_work.damage_timer = 60;
        AppMain.GmBoss4CapsuleSetInvincible(30);
        AppMain.gmBoss4BodySetActionWhole(body_work, 6, true);
    }

    private static bool gmBoss4BodyIsExtraAttack(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        return AppMain.GMM_BOSS4_MGR(body_work).life <= AppMain.GMD_BOSS4_EXTRA_ATK_THRESHOLD_LIFE;
    }

    private static void gmBoss4BodyInitPreANChainMotion(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss4BodyInitPreANMove(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.spd.x = 0;
        obsObjectWork.spd_add.x = -AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_PRE_ATKNML_SPD_ADD);
    }

    private static bool gmBoss4BodyUpdatePreANMoveLeft(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        bool flag = false;
        if (AppMain.MTM_MATH_ABS(obj_work.spd.x) >= AppMain.FX_F32_TO_FX32((float)AppMain.GMD_BOSS4_BODY_PRE_ATKNML_SPD_MAX_ABS))
        {
            obj_work.spd.x = -AppMain.FX_F32_TO_FX32((float)AppMain.GMD_BOSS4_BODY_PRE_ATKNML_SPD_MAX_ABS);
            obj_work.spd_add.x = 0;
        }
        if (obj_work.pos.x <= AppMain.GMM_BOSS4_AREA_LEFT() + AppMain.FX_F32_TO_FX32(74f))
        {
            obj_work.pos.x = AppMain.GMM_BOSS4_AREA_LEFT() + AppMain.FX_F32_TO_FX32(74f);
            flag = true;
        }
        if (flag)
            AppMain.GmBsCmnSetObjSpdZero(obj_work);
        return flag;
    }

    private static bool gmBoss4BodyUpdatePreANMoveRight(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        bool flag = false;
        if (AppMain.MTM_MATH_ABS(obj_work.spd.x) >= AppMain.FX_F32_TO_FX32((float)AppMain.GMD_BOSS4_BODY_PRE_ATKNML_SPD_MAX_ABS))
        {
            obj_work.spd.x = AppMain.FX_F32_TO_FX32((float)AppMain.GMD_BOSS4_BODY_PRE_ATKNML_SPD_MAX_ABS);
            obj_work.spd_add.x = 0;
        }
        if (obj_work.pos.x >= AppMain.GMM_BOSS4_AREA_LEFT() + AppMain.FX_F32_TO_FX32(310f))
        {
            obj_work.pos.x = AppMain.GMM_BOSS4_AREA_LEFT() + AppMain.FX_F32_TO_FX32(310f);
            flag = true;
        }
        if (flag)
            AppMain.GmBsCmnSetObjSpdZero(obj_work);
        return flag;
    }

    private static void gmBoss4BodySetANChainInitialBlendSpd(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
    }

    private static void gmBoss4BodyInitAtkNmlMove(AppMain.GMS_BOSS4_BODY_WORK body_work, int frame)
    {
        body_work.move_time = frame;
        body_work.move_cnt = 0;
        AppMain.gmBoss4BodyUpdateAtkNmlMove(body_work);
    }

    private static bool gmBoss4BodyUpdateAtkNmlMove(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num1 = AppMain.GMM_BOSS4_AREA_LEFT() + AppMain.FX_F32_TO_FX32(74f);
        int num2 = AppMain.GMM_BOSS4_AREA_LEFT() + AppMain.FX_F32_TO_FX32(310f);
        if (body_work.dir.direction == 1)
        {
            int num3 = num1;
            num1 = num2;
            num2 = num3;
        }
        bool flag;
        if (body_work.move_cnt < body_work.move_time)
        {
            int moveCurveAngleWidth = AppMain.GMD_BOSS4_BODY_ATKNML_MOVE_CURVE_ANGLE_WIDTH;
            int moveCurveStartAngle = AppMain.GMD_BOSS4_BODY_ATKNML_MOVE_CURVE_START_ANGLE;
            ++body_work.move_cnt;
            int num3 = (int)((double)moveCurveAngleWidth / (double)body_work.move_time);
            float num4 = AppMain.nnCos(moveCurveStartAngle) - AppMain.nnCos(moveCurveStartAngle + moveCurveAngleWidth);
            float num5 = (AppMain.nnCos(moveCurveStartAngle) - AppMain.nnCos(moveCurveStartAngle + num3 * body_work.move_cnt)) / num4;
            obsObjectWork.pos.x = num1 + (int)((double)(num2 - num1) * (double)num5);
            flag = false;
        }
        else
        {
            obsObjectWork.pos.x = num2;
            flag = true;
        }
        return flag;
    }

    private static void gmBoss4BodySetFlipForAtkNmlMove(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (AppMain.GMM_BS_OBJ((object)body_work).pos.x < AppMain.GMM_BOSS4_AREA_CENTER_X())
            body_work.dir.direction = 0;
        else
            body_work.dir.direction = 1;
    }

    private static void gmBoss4BodyInitAtkNmlFlipAndTurn(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        int bodyAtknmlTurnFrame = AppMain.GMD_BOSS4_BODY_ATKNML_TURN_FRAME;
        AppMain.MTM_ASSERT(bodyAtknmlTurnFrame < AppMain.GMD_BOSS4_BODY_ATKNML_DRIFT_FRAME);
        AppMain.gmBoss4BodySetFlipForAtkNmlMove(body_work);
        if (body_work.dir.direction == 1)
            AppMain.GmBoss4UtilInitTurnGently(body_work.dir, AppMain.GMD_BOSS4_LEFTWARD_ANGLE, bodyAtknmlTurnFrame, true);
        else
            AppMain.GmBoss4UtilInitTurnGently(body_work.dir, AppMain.GMD_BOSS4_RIGHTWARD_ANGLE, bodyAtknmlTurnFrame, false);
    }

    private static bool gmBoss4BodyUpdateAtkNmlFlipAndTurn(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        return AppMain.GmBoss4UtilUpdateTurnGently(body_work.dir);
    }

    private static void gmBoss4BodyInitAtkNmlDrift(AppMain.GMS_BOSS4_BODY_WORK body_work, int frame)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.MTM_ASSERT(frame > 0);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        body_work.drift_angle = 0;
        body_work.drift_ang_spd = (int)AppMain.nnRoundOff((float)((double)AppMain.AKM_DEGtoA32(180f) / (double)frame + 0.5));
        body_work.drift_timer = frame;
        body_work.drift_pivot_x = body_work.dir.direction != 1 ? AppMain.GMM_BOSS4_AREA_LEFT() + AppMain.FX_F32_TO_FX32(74f) : AppMain.GMM_BOSS4_AREA_LEFT() + AppMain.FX_F32_TO_FX32(310f);
        AppMain.gmBoss4BodyUpdateAtkNmlDrift(body_work);
    }

    private static bool gmBoss4BodyUpdateAtkNmlDrift(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num;
        bool flag;
        if (body_work.drift_timer != 0)
        {
            --body_work.drift_timer;
            body_work.drift_angle = (int)((long)ushort.MaxValue & (long)(body_work.drift_angle + body_work.drift_ang_spd));
            num = AppMain.FX_Mul(AppMain.FX_Sin(body_work.drift_angle), AppMain.FX_F32_TO_FX32(16f));
            flag = false;
        }
        else
        {
            num = 0;
            flag = true;
        }
        if (body_work.dir.direction == 0)
            num = -num;
        obsObjectWork.pos.x = body_work.drift_pivot_x + num;
        return flag;
    }

    private static void gmBoss4BodyInitEscapeMove(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.spd.x = 0;
        obsObjectWork.spd_add.x = body_work.dir.direction != 1 ? 409 : -409;
        obsObjectWork.spd_add.y = 204;
    }

    private static bool gmBoss4BodyUpdateEscapeMove(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        bool flag = false;
        if (AppMain.MTM_MATH_ABS(obj_work.spd.x) >= 11264)
        {
            obj_work.spd.x = 11264;
            obj_work.spd.y = -1536;
            obj_work.spd_add.x = 0;
            obj_work.spd_add.y = 0;
        }
        float right = (float)AppMain.g_gm_main_system.map_fcol.right;
        if (obj_work.pos.x > AppMain.FX_F32_TO_FX32(right + 100f))
            flag = true;
        if (flag)
            AppMain.GmBsCmnSetObjSpdZero(obj_work);
        return flag;
    }

    private static void gmBoss4BodyInitDefeatState(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        bool flag = false;
        if (((int)body_work.flag[0] & 1) != 0)
            flag = true;
        AppMain.gmBoss4BodyChangeState(body_work, 7, true);
        if (flag)
            body_work.flag[0] |= 1U;
        else
            body_work.flag[0] &= 4294967294U;
        AppMain.GmSoundChangeWinBossBGM();
    }

    private static void gmBoss4BodyUpdateChainTopDirection(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
    }

    private static void gmBoss4BodyAtkHitFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        ((AppMain.GMS_BOSS4_BODY_WORK)my_rect.parent_obj).flag[0] |= 268435456U;
        AppMain.GmEnemyDefaultAtkFunc(my_rect, your_rect);
    }

    private static void gmBoss4BodyDamageDefFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        AppMain.GMS_BOSS4_BODY_WORK body_work = (AppMain.GMS_BOSS4_BODY_WORK)parentObj1;
        if (parentObj2 == null || (ushort)1 != parentObj2.obj_type)
            return;
        AppMain.GmBoss4UtilSetPlayerAttackReaction(parentObj2, parentObj1);
        if (body_work.nohit_work.timer == 0U)
        {
            AppMain.GmSoundPlaySE("Boss0_01");
            AppMain.gmBoss4EffDamageInit((object)body_work);
            AppMain.gmBoss4BodyExecDamageRoutine(body_work);
            if (AppMain.GmBoss4Is2ndStage())
            {
                AppMain.GmBoss4ChibiExplosion();
                body_work.wait_timer = 60U;
                body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate2nd);
            }
            AppMain.GMM_PAD_VIB_SMALL_TIME(30f);
        }
        AppMain.GmBoss4UtilInitNoHitTimer(body_work.nohit_work, (AppMain.GMS_ENEMY_COM_WORK)parentObj1, 10);
    }

    private static void gmBoss4BodyDefHitFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        parentObj2.pos.x -= parentObj2.move.x;
        if (parentObj1.pos.x > parentObj2.pos.x)
        {
            parentObj2.pos.x -= AppMain.FX_F32_TO_FX32(2f);
            parentObj2.spd.x = -AppMain.MTM_MATH_ABS(parentObj2.spd.x);
            parentObj2.spd_m = -AppMain.MTM_MATH_ABS(parentObj2.spd_m);
        }
        if (parentObj1.pos.x >= parentObj2.pos.x)
            return;
        parentObj2.pos.x += AppMain.FX_F32_TO_FX32(2f);
        parentObj2.spd.x = AppMain.MTM_MATH_ABS(parentObj2.spd.x);
        parentObj2.spd_m = AppMain.MTM_MATH_ABS(parentObj2.spd_m);
    }

    private static void gmBoss4BodyOutFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK gmsBosS4BodyWork = (AppMain.GMS_BOSS4_BODY_WORK)obj_work;
        AppMain.GmBsCmnUpdateCNMParam(obj_work, gmsBosS4BodyWork.cnm_mgr_work);
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void gmBoss4BodyChangeState(AppMain.GMS_BOSS4_BODY_WORK body_work, int state)
    {
        AppMain.gmBoss4BodyChangeState(body_work, state, false);
    }

    private static void gmBoss4BodyChangeState(
      AppMain.GMS_BOSS4_BODY_WORK body_work,
      int state,
      bool is_wrapped)
    {
        AppMain.GMF_BOSS4_BODY_STATE_LEAVE_FUNC bodyStateLeaveFunc = AppMain.gm_boss4_body_state_leave_func_tbl[body_work.state];
        if (bodyStateLeaveFunc != null)
            bodyStateLeaveFunc(body_work);
        body_work.prev_state = body_work.state;
        body_work.state = state;
        AppMain.GMS_BOSS4_BODY_STATE_ENTER_INFO bodyStateEnterInfo = AppMain.gm_boss4_body_state_enter_info_tbl[body_work.state];
        if (bodyStateEnterInfo.enter_func == null)
            return;
        bodyStateEnterInfo.enter_func(body_work);
    }

    private static void gmBoss4BodyWaitLoad(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK body_work = (AppMain.GMS_BOSS4_BODY_WORK)obj_work;
        if (!AppMain.GmBoss4IsBuilded())
            return;
        AppMain.GmBoss4UtilInitNodeMatrix(body_work.node_work, obj_work, 6);
        AppMain.GmBoss4UtilGetNodeMatrix(body_work.node_work, 2);
        AppMain.GmBoss4UtilGetNodeMatrix(body_work.node_work, 2);
        AppMain.GmBoss4UtilGetNodeMatrix(body_work.node_work, 9);
        AppMain.GmBoss4UtilGetNodeMatrix(body_work.node_work, 10);
        AppMain.GmBoss4UtilGetNodeMatrix(body_work.node_work, 5);
        AppMain.GmBoss4UtilGetNodeMatrix(body_work.node_work, 8);
        AppMain.GmBsCmnCreateCNMMgrWork(body_work.cnm_mgr_work, obj_work.obj_3d._object, (ushort)1);
        AppMain.GmBsCmnInitCNMCb(obj_work, body_work.cnm_mgr_work);
        body_work.chaintop_cnm_reg_id = AppMain.GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, 0);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4BodyMain);
        body_work.damage_timer = 0;
        AppMain.GmBoss4UtilInitNoHitTimer(body_work.nohit_work, (AppMain.GMS_ENEMY_COM_WORK)body_work, 0);
        if (AppMain.GmBoss4CheckBossRush())
            AppMain.gmBoss4BodyChangeState(body_work, 5);
        else
            AppMain.gmBoss4BodyChangeState(body_work, 1);
    }

    private static void gmBoss4BodyMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK gmsBosS4BodyWork = (AppMain.GMS_BOSS4_BODY_WORK)obj_work;
        AppMain.GmBoss4UtilUpdateNoHitTimer(gmsBosS4BodyWork.nohit_work);
        if (gmsBosS4BodyWork.proc_update != null)
            gmsBosS4BodyWork.proc_update(gmsBosS4BodyWork);
        AppMain.gmBoss4BodyUpdateSuspendAction(gmsBosS4BodyWork);
        AppMain.gmBoss4EffAfterburnerUpdateCreate(gmsBosS4BodyWork);
        if (((int)gmsBosS4BodyWork.flag[0] & int.MinValue) != 0)
        {
            gmsBosS4BodyWork.flag[0] &= 1073741823U;
            AppMain.gmBoss4BodyInitDefeatState(gmsBosS4BodyWork);
        }
        else
        {
            if (gmsBosS4BodyWork.damage_timer > 0)
                --gmsBosS4BodyWork.damage_timer;
            if (((int)gmsBosS4BodyWork.flag[0] & 1073741824) != 0)
            {
                gmsBosS4BodyWork.flag[0] &= 3221225471U;
                gmsBosS4BodyWork.flag[0] |= 536870912U;
                AppMain.GmBsCmnInitObject3DNNDamageFlicker(obj_work, gmsBosS4BodyWork.flk_work, 32f);
            }
            AppMain.GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, gmsBosS4BodyWork.flk_work);
            AppMain.GmBoss4UtilUpdateDirection(gmsBosS4BodyWork.dir, obj_work);
            AppMain.gmBoss4BodyUpdateChainTopDirection(gmsBosS4BodyWork);
            AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
            AppMain.GMS_PLAYER_WORK playerObj = (AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj();
            if (playerObj.seq_state == 17 || playerObj.seq_state == 19)
                gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
            else
                gmsEnemy3DWork.ene_com.rect_work[2].flag |= 4U;
        }
    }

    private static void gmBoss4BodyStateEnterStart(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        obj_work.flag |= 2U;
        body_work.flag[0] |= 64U;
        AppMain.gmBoss4BodySetActionWhole(body_work, 0, true);
        body_work.flag[0] |= 32U;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        AppMain.GmBoss4UtilSetDirectionNormal(body_work.dir);
        body_work.wait_timer = 120U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateStartWithWait);
        AppMain.gmBoss4EffBossLightSetEnable(body_work, true);
    }

    private static void gmBoss4BodyStateLeaveStart(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
        body_work.flag[0] &= 4294967231U;
        obsObjectWork.flag &= 4294967293U;
        body_work.flag[0] &= 4294967263U;
    }

    private static void gmBoss4BodyStateUpdateStartWithWait(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (!AppMain.gmBoss4IsScrollLockBusy())
            return;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateStartWithWaitEnd);
    }

    private static void gmBoss4BodyStateUpdateStartWithWaitEnd(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.gmBoss4IsScrollLockBusy())
            return;
        AppMain.GmBsCmnSetObjSpd(obj_work, 0, 4096, 0);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateStartWithFall);
        AppMain.GmBoss4CapsuleSetInvincible(600, false);
        AppMain.GmBoss4ChibiSetInvincible(false);
        body_work.wait_timer2 = 90U;
        body_work.flag[0] |= 268435456U;
        body_work.wait_timer = 120U;
        AppMain.GmBoss4UtilLookAtPlayer(body_work.dir, obj_work, 1);
        AppMain.GmBoss4UtilLookAt(body_work.dir);
        AppMain.GmMapSetMapDrawSize(6);
    }

    private static void gmBoss4BodyStateUpdateStartWithFall(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.enemy_flag |= 32768U;
        if (body_work.wait_timer2 > 0U)
        {
            --body_work.wait_timer2;
            AppMain.GmBoss4UtilLookAtPlayer(body_work.dir, obj_work, 1);
        }
        if (--body_work.wait_timer <= 0U && obj_work.pos.y <= AppMain.FX_F32_TO_FX32(235f))
        {
            body_work.flag[0] |= 268435456U;
            body_work.wait_timer = 120U;
        }
        if (obj_work.pos.y < body_work.atk_nml_alt)
            return;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.pos.y = body_work.atk_nml_alt;
        AppMain.GmBsCmnSetObjSpd(obj_work, 0, 0, 0);
        body_work.wait_timer = 30U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateStartWithFallWait);
        AppMain.GmBoss4UtilLookAtPlayer(body_work.dir, obj_work, 28);
    }

    private static void gmBoss4BodyStateUpdateStartWithFallWait(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBoss4UtilLookAt(body_work.dir);
        if (--body_work.wait_timer != 0U)
            return;
        AppMain.GmBoss4CapsuleSetInvincible(0);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.pos.y = body_work.atk_nml_alt;
        AppMain.GmBsCmnSetObjSpd(obj_work, -4096, 0, 0);
        AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 1);
        AppMain.gmBoss4BodyChangeState(body_work, 2);
        ((AppMain.GMS_ENEMY_3D_WORK)(AppMain.GMS_BOSS4_EGG_WORK)body_work.parts_objs[1]).ene_com.enemy_flag &= 4294934527U;
    }

    private static void gmBoss4BodyStateEnterPreAtkNml(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.flag &= 4294967293U;
        AppMain.gmBoss4BodySetActionWhole(body_work, 2);
        AppMain.gmBoss4BodyInitPreANChainMotion(body_work);
        AppMain.gmBoss4BodyInitPreANMove(body_work);
        AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 1);
        if (body_work.dir.direction == 0)
        {
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdatePreAtkNmlWithMoveRight);
            obsObjectWork.spd_add.x = -obsObjectWork.spd_add.x;
        }
        else
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdatePreAtkNmlWithMoveLeft);
    }

    private static void gmBoss4BodyStateLeavePreAtkNml(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
    }

    private static void gmBoss4BodyStateUpdatePreAtkNmlWithMoveLeft(
      AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.VecFx32 vecFx32_1 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(-30f), AppMain.FX_F32_TO_FX32(0.0f));
        AppMain.VecFx32 vecFx32_2 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(180f), AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f));
        if (((int)body_work.flag[0] & 2048) != 0)
        {
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
            if (((int)body_work.flag[0] & 1024) == 0)
                AppMain.GmBoss4EffCommonInit(742, new AppMain.VecFx32?(vecFx32_1), (AppMain.OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 2, new AppMain.VecFx32?(vecFx32_2), body_work.flag, 1024U);
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
            obsObjectWork.spd.x = 0;
            obsObjectWork.spd_add.x = 0;
            if (body_work.ene_3d.ene_com.obj_work.pos.y > AppMain.FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd += AppMain.FX_F32_TO_FX32(0.03f);
            }
            else
            {
                body_work.avoid_yspd -= AppMain.FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < AppMain.FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = AppMain.FX_F32_TO_FX32(1f);
            }
            body_work.ene_3d.ene_com.obj_work.pos.y -= body_work.avoid_yspd;
            if (body_work.ene_3d.ene_com.obj_work.pos.y < AppMain.FX_F32_TO_FX32(190f))
            {
                body_work.ene_3d.ene_com.obj_work.pos.y = AppMain.FX_F32_TO_FX32(190f);
                --body_work.avoid_timer;
            }
            if (body_work.avoid_timer < 0)
            {
                body_work.flag[0] &= 4294965247U;
                body_work.avoid_yspd = 0;
            }
            AppMain.ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], (short)-8, (short)20, (short)8, (short)40);
        }
        else if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
        {
            body_work.flag[0] |= 4096U;
            body_work.ene_3d.ene_com.rect_work[1].flag &= 4294967291U;
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
            body_work.flag[0] &= 4294966271U;
            if (body_work.ene_3d.ene_com.obj_work.pos.y > AppMain.FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd -= AppMain.FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < AppMain.FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = AppMain.FX_F32_TO_FX32(1f);
            }
            else
                body_work.avoid_yspd += AppMain.FX_F32_TO_FX32(0.03f);
            body_work.ene_3d.ene_com.obj_work.pos.y += body_work.avoid_yspd;
            if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
                return;
            AppMain.gmBoss4BodyInitPreANMove(body_work);
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 1);
            body_work.flag[0] &= 4294966271U;
            AppMain.ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], (short)-32, (short)-8, (short)32, (short)40);
            body_work.ene_3d.ene_com.rect_work[1].flag |= 4U;
        }
        else
        {
            body_work.ene_3d.ene_com.obj_work.pos.y = body_work.atk_nml_alt;
            body_work.flag[0] &= 4294963199U;
            AppMain.GmBoss4UtilSetDirectionNormal(body_work.dir);
            if (AppMain.gmBoss4BodyUpdatePreANMoveLeft(body_work))
            {
                AppMain.gmBoss4BodyChangeState(body_work, 3);
                AppMain.gmBoss4BodySetANChainInitialBlendSpd(body_work);
            }
            if (body_work.damage_timer == 0)
                return;
            for (int index = 1; index < AppMain.GMD_BOSS4_SPEED_TIMES_IN_DAMAGE; ++index)
            {
                if (AppMain.gmBoss4BodyUpdatePreANMoveLeft(body_work))
                {
                    AppMain.gmBoss4BodyChangeState(body_work, 3);
                    AppMain.gmBoss4BodySetANChainInitialBlendSpd(body_work);
                    break;
                }
            }
        }
    }

    private static void gmBoss4BodyStateUpdatePreAtkNmlWithMoveRight(
      AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.VecFx32 vecFx32_1 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(-30f), AppMain.FX_F32_TO_FX32(0.0f));
        AppMain.VecFx32 vecFx32_2 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(180f), AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f));
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (((int)body_work.flag[0] & 2048) != 0)
        {
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
            if (((int)body_work.flag[0] & 1024) == 0)
                AppMain.GmBoss4EffCommonInit(742, new AppMain.VecFx32?(vecFx32_1), (AppMain.OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 2, new AppMain.VecFx32?(vecFx32_2), body_work.flag, 1024U);
            obsObjectWork.spd.x = 0;
            obsObjectWork.spd_add.x = 0;
            if (body_work.ene_3d.ene_com.obj_work.pos.y > AppMain.FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd += AppMain.FX_F32_TO_FX32(0.03f);
            }
            else
            {
                body_work.avoid_yspd -= AppMain.FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < AppMain.FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = AppMain.FX_F32_TO_FX32(1f);
            }
            body_work.ene_3d.ene_com.obj_work.pos.y -= body_work.avoid_yspd;
            if (body_work.ene_3d.ene_com.obj_work.pos.y < AppMain.FX_F32_TO_FX32(190f))
            {
                body_work.ene_3d.ene_com.obj_work.pos.y = AppMain.FX_F32_TO_FX32(190f);
                --body_work.avoid_timer;
            }
            if (body_work.avoid_timer < 0)
            {
                body_work.flag[0] &= 4294965247U;
                body_work.avoid_yspd = 0;
            }
            AppMain.ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], (short)-8, (short)20, (short)8, (short)40);
        }
        else if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
        {
            body_work.ene_3d.ene_com.rect_work[1].flag &= 4294967291U;
            body_work.flag[0] |= 4096U;
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
            body_work.flag[0] &= 4294966271U;
            if (body_work.ene_3d.ene_com.obj_work.pos.y > AppMain.FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd -= AppMain.FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < AppMain.FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = AppMain.FX_F32_TO_FX32(1f);
            }
            else
                body_work.avoid_yspd += AppMain.FX_F32_TO_FX32(0.03f);
            body_work.ene_3d.ene_com.obj_work.pos.y += body_work.avoid_yspd;
            if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
                return;
            AppMain.gmBoss4BodyInitPreANMove(body_work);
            obsObjectWork.spd_add.x = -obsObjectWork.spd_add.x;
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 1);
            body_work.flag[0] &= 4294966271U;
            AppMain.ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], (short)-32, (short)-8, (short)32, (short)40);
            body_work.ene_3d.ene_com.rect_work[1].flag |= 4U;
        }
        else
        {
            body_work.ene_3d.ene_com.obj_work.pos.y = body_work.atk_nml_alt;
            body_work.flag[0] &= 4294963199U;
            AppMain.GmBoss4UtilSetDirectionNormal(body_work.dir);
            if (AppMain.gmBoss4BodyUpdatePreANMoveRight(body_work))
            {
                AppMain.gmBoss4BodyChangeState(body_work, 3);
                AppMain.gmBoss4BodySetANChainInitialBlendSpd(body_work);
            }
            if (body_work.damage_timer == 0)
                return;
            for (int index = 1; index < AppMain.GMD_BOSS4_SPEED_TIMES_IN_DAMAGE; ++index)
            {
                if (AppMain.gmBoss4BodyUpdatePreANMoveRight(body_work))
                {
                    AppMain.gmBoss4BodyChangeState(body_work, 3);
                    AppMain.gmBoss4BodySetANChainInitialBlendSpd(body_work);
                    break;
                }
            }
        }
    }

    private static void gmBoss4BodyStateEnterAtkNml(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        bool force_change = false;
        obsObjectWork.flag &= 4294967293U;
        if (body_work.dir.direction == 1)
            force_change = true;
        AppMain.gmBoss4BodySetActionWhole(body_work, 3, force_change);
        AppMain.gmBoss4BodyInitAtkNmlFlipAndTurn(body_work);
        AppMain.gmBoss4BodyInitAtkNmlDrift(body_work, AppMain.GMD_BOSS4_BODY_ATKNML_DRIFT_FRAME);
        AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateAtkNmlWithTurn);
    }

    private static void gmBoss4BodyStateLeaveAtkNml(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
    }

    private static void gmBoss4BodyStateUpdateAtkNmlWithTurn(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        bool flag = AppMain.gmBoss4BodyUpdateAtkNmlDrift(body_work);
        if (!AppMain.gmBoss4BodyUpdateAtkNmlFlipAndTurn(body_work) || !flag)
            return;
        AppMain.gmBoss4BodySetFlipForAtkNmlMove(body_work);
        AppMain.gmBoss4BodyInitAtkNmlMove(body_work, AppMain.GMD_BOSS4_BODY_ATKNML_MOVE_FRAME);
        AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 1);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateAtkNmlWithMove);
    }

    private static void gmBoss4BodyStateUpdateAtkNmlWithMove(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.VecFx32 vecFx32_1 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(-30f), AppMain.FX_F32_TO_FX32(0.0f));
        AppMain.VecFx32 vecFx32_2 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(180f), AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f));
        if (((int)body_work.flag[0] & 2048) != 0)
        {
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
            if (((int)body_work.flag[0] & 1024) == 0)
                AppMain.GmBoss4EffCommonInit(742, new AppMain.VecFx32?(vecFx32_1), (AppMain.OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 2, new AppMain.VecFx32?(vecFx32_2), body_work.flag, 1024U);
            if (body_work.ene_3d.ene_com.obj_work.pos.y > AppMain.FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd += AppMain.FX_F32_TO_FX32(0.03f);
            }
            else
            {
                body_work.avoid_yspd -= AppMain.FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < AppMain.FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = AppMain.FX_F32_TO_FX32(1f);
            }
            body_work.ene_3d.ene_com.obj_work.pos.y -= body_work.avoid_yspd;
            if (body_work.ene_3d.ene_com.obj_work.pos.y < AppMain.FX_F32_TO_FX32(190f))
            {
                body_work.ene_3d.ene_com.obj_work.pos.y = AppMain.FX_F32_TO_FX32(190f);
                --body_work.avoid_timer;
            }
            if (body_work.avoid_timer < 0)
            {
                body_work.flag[0] &= 4294965247U;
                body_work.avoid_yspd = 0;
            }
            AppMain.ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], (short)-8, (short)20, (short)8, (short)40);
        }
        else if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
        {
            body_work.ene_3d.ene_com.rect_work[1].flag &= 4294967291U;
            body_work.flag[0] |= 4096U;
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
            body_work.flag[0] &= 4294966271U;
            if (body_work.ene_3d.ene_com.obj_work.pos.y > AppMain.FX_F32_TO_FX32(240f))
            {
                body_work.avoid_yspd -= AppMain.FX_F32_TO_FX32(0.05f);
                if (body_work.avoid_yspd < AppMain.FX_F32_TO_FX32(1f))
                    body_work.avoid_yspd = AppMain.FX_F32_TO_FX32(1f);
            }
            else
                body_work.avoid_yspd += AppMain.FX_F32_TO_FX32(0.03f);
            body_work.ene_3d.ene_com.obj_work.pos.y += body_work.avoid_yspd;
            if (body_work.atk_nml_alt > body_work.ene_3d.ene_com.obj_work.pos.y)
                return;
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
            AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 1);
            body_work.flag[0] &= 4294966271U;
            AppMain.ObjRectWorkSet(body_work.ene_3d.ene_com.rect_work[1], (short)-32, (short)-8, (short)32, (short)40);
            body_work.ene_3d.ene_com.rect_work[1].flag |= 4U;
        }
        else
        {
            body_work.ene_3d.ene_com.obj_work.pos.y = body_work.atk_nml_alt;
            body_work.flag[0] &= 4294963199U;
            AppMain.GmBoss4UtilSetDirectionNormal(body_work.dir);
            if (AppMain.gmBoss4BodyIsExtraAttack(body_work))
                AppMain.gmBoss4BodyChangeState(body_work, 4);
            else if (AppMain.gmBoss4BodyUpdateAtkNmlMove(body_work))
            {
                AppMain.gmBoss4BodyChangeState(body_work, 3);
            }
            else
            {
                if (body_work.damage_timer == 0)
                    return;
                for (int index = 1; index < AppMain.GMD_BOSS4_SPEED_TIMES_IN_DAMAGE; ++index)
                {
                    if (AppMain.gmBoss4BodyUpdateAtkNmlMove(body_work))
                    {
                        AppMain.gmBoss4BodyChangeState(body_work, 3);
                        break;
                    }
                }
            }
        }
    }

    private static void gmBoss4BodyStateEnter1stEnd(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GmBoss4UtilInitNoHitTimer(body_work.nohit_work, (AppMain.GMS_ENEMY_COM_WORK)body_work, 1200);
        ((AppMain.GMS_ENEMY_3D_WORK)body_work).ene_com.rect_work[0].flag |= 2048U;
        AppMain.GmBoss4CapsuleSetInvincible(600, false);
        AppMain.GmBoss4ChibiSetInvincible(true);
        AppMain.GmBoss4ChibiExplosion();
        AppMain.GmBoss4UtilPlayerStop(true);
        AppMain.GmBoss4UtilTimerStop(true);
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)body_work;
        AppMain.VecFx32 end = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(AppMain.ObjCameraGet(0).target_pos.x), AppMain.FX_F32_TO_FX32(220f), 0);
        AppMain.GmBoss4UtilInitMove(body_work.move_work, obsObjectWork.pos, end, 180, 1);
        bool is_positive = AppMain.GmBoss4UtilIsDirectionPositiveFromCurrent(body_work.dir, AppMain.GMD_BOSS4_LEFTWARD_ANGLE);
        body_work.dir.direction = 1;
        AppMain.GmBoss4UtilInitTurnGently(body_work.dir, AppMain.GMD_BOSS4_LEFTWARD_ANGLE, 40, is_positive);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate1stEnd);
    }

    private static void gmBoss4BodyStateUpdate1stEnd(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GmBoss4UtilPlayerStop(true);
        AppMain.GmBoss4ChibiExplosion();
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)body_work;
        AppMain.GmBoss4UtilUpdateTurnGently(body_work.dir);
        if (body_work.move_work.now_count == 30)
        {
            AppMain.VecFx32 vecFx32_1 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(-30f), AppMain.FX_F32_TO_FX32(0.0f));
            AppMain.VecFx32 vecFx32_2 = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(180f), AppMain.FX_F32_TO_FX32(0.0f), AppMain.FX_F32_TO_FX32(0.0f));
            AppMain.GmBoss4EffCommonInit(742, new AppMain.VecFx32?(vecFx32_1), (AppMain.OBS_OBJECT_WORK)body_work, 2U, 2U, body_work.node_work, 2, new AppMain.VecFx32?(vecFx32_2), body_work.flag, 1024U);
        }
        if (AppMain.GmBoss4UtilUpdateMove(body_work.move_work))
            body_work.proc_update = AppMain.GmBoss4CapsuleGetCount() <= 0 ? new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateInit1stEndAngry) : new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateInit1stEndExplosion);
        AppMain.GmBoss4UtilUpdateMovePosition(body_work.move_work, obj_work);
    }

    private static void gmBoss4BodyStateInit1stEndExplosion(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)body_work;
        AppMain.GmBoss4UtilPlayerStop(true);
        AppMain.GmBoss4CapsuleExplosion();
        body_work.wait_timer = 180U;
        body_work.flag[0] |= 16777216U;
        AppMain.VecFx32 end = new AppMain.VecFx32(obsObjectWork.pos.x, obsObjectWork.pos.y + AppMain.FX_F32_TO_FX32(20f), 0);
        AppMain.GmBoss4UtilInitMove(body_work.move_work, obsObjectWork.pos, end, 60, 1);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate1stEndExplosion);
    }

    private static void gmBoss4BodyStateUpdate1stEndExplosion(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GmBoss4UtilPlayerStop(true);
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)body_work;
        AppMain.GmBoss4UtilUpdateMove(body_work.move_work);
        AppMain.GmBoss4UtilUpdateMovePosition(body_work.move_work, obj_work);
        if (body_work.wait_timer > 0U)
            --body_work.wait_timer;
        else
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateInit1stEndAngry);
    }

    private static void gmBoss4BodyStateInit1stEndAngry(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GmBoss4UtilPlayerStop(true);
        AppMain.gmBoss4BodySetActionWhole(body_work, 8);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate1stEndAngry);
    }

    private static void gmBoss4BodyStateUpdate1stEndAngry(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GmBoss4UtilPlayerStop(true);
        if (AppMain.GmBsCmnIsActionEnd((AppMain.OBS_OBJECT_WORK)body_work) == 0)
            return;
        AppMain.gmBoss4BodySetActionWhole(body_work, 9);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate1stEndAngryL2);
    }

    private static void gmBoss4BodyStateUpdate1stEndAngryL2(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GmBoss4UtilPlayerStop(true);
        if (AppMain.GmBsCmnIsActionEnd((AppMain.OBS_OBJECT_WORK)body_work) == 0)
            return;
        AppMain.gmBoss4BodySetActionWhole(body_work, 10);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateInit1stEndEscape);
    }

    private static void gmBoss4BodyStateInit1stEndEscape(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GmBoss4UtilPlayerStop(true);
        AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 2);
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)body_work;
        AppMain.VecFx32 end = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(AppMain.ObjCameraGet(0).target_pos.x + 200f) + (AppMain.GMM_BOSS4_AREA_RIGHT() - AppMain.GMM_BOSS4_AREA_LEFT()) / 2, obsObjectWork.pos.y, 0);
        AppMain.GmBoss4UtilInitMove(body_work.move_work, obsObjectWork.pos, end, 150, 1);
        AppMain.GmMapSetMapDrawSize(1);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate1stEndEscape);
    }

    private static void gmBoss4BodyStateUpdate1stEndEscape(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GmBoss4UtilPlayerStop(true);
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)body_work;
        if (AppMain.GmBoss4UtilUpdateMove(body_work.move_work))
            AppMain.gmBoss4BodyChangeState(body_work, 5);
        else
            AppMain.GmBoss4UtilUpdateMovePosition(body_work.move_work, obj_work);
    }

    private static void gmBoss4BodyStateEnter2nd(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)body_work;
        AppMain.GmBoss4UtilInitNoHitTimer(body_work.nohit_work, (AppMain.GMS_ENEMY_COM_WORK)body_work, 0);
        AppMain.GmBoss4CapsuleSetInvincible(0);
        AppMain.GmBoss4ChibiSetInvincible(false);
        AppMain.GmBoss4UtilPlayerStop(false);
        AppMain.GmBoss4UtilTimerStop(false);
        if (!AppMain.GmBoss4CheckBossRush())
            AppMain.GmGmkCamScrLimitRelease((byte)4).user_work = 16U;
        obsObjectWork.pos.x = AppMain.FX_F32_TO_FX32((float)AppMain.GMD_BOSS4_BODY_2ND_POS_X);
        obsObjectWork.pos.y = AppMain.FX_F32_TO_FX32((float)AppMain.GMD_BOSS4_BODY_2ND_POS_Y);
        obsObjectWork.pos.z = -131072;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)body_work;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag |= 2048U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag |= 2048U;
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294965247U;
        AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[0], (short)-38, (short)-24, (short)38, (short)32);
        AppMain.gmBoss4SetPartTextureBurnt((AppMain.OBS_OBJECT_WORK)(AppMain.GMS_BOSS4_EGG_WORK)body_work.parts_objs[1], false);
        AppMain.GmBoss4UtilInitTurnGently(body_work.dir, AppMain.GMD_BOSS4_LEFTWARD_ANGLE, 1, false);
        AppMain.GmBoss4UtilUpdateTurnGently(body_work.dir);
        AppMain.gmBoss4BodySetActionWhole(body_work, 5);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateInit2nd);
        AppMain.GmBoss4CapsuleExplosion();
        bool is_positive = AppMain.GmBoss4UtilIsDirectionPositiveFromCurrent(body_work.dir, AppMain.GMD_BOSS4_LEFTWARD_ANGLE);
        body_work.dir.direction = 1;
        AppMain.GmBoss4UtilInitTurnGently(body_work.dir, AppMain.GMD_BOSS4_LEFTWARD_ANGLE, 1, is_positive);
        AppMain.GmBoss4UtilUpdateTurnGently(body_work.dir);
        AppMain.gm_boss4_locking = 0;
    }

    private static void gmBoss4BodyStateInit2nd(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)body_work;
        if (AppMain.GmBsCmnGetPlayerObj().pos.x >= AppMain.FX_F32_TO_FX32((float)AppMain.GMD_BOSS4_SCROLL_INIT_X))
        {
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate2ndWaitBoss);
            AppMain.GmBoss4ScrollInit((AppMain.GMS_EVE_RECORD_EVENT)null, 0, 0, (byte)0);
            body_work.wait_timer = AppMain.GMD_BOSS4_BODY_SONIC_CTRL_TIME;
            if (AppMain.GmBoss4CheckBossRush())
            {
                AppMain.gmBoss4BodySetActionWhole(body_work, 5);
                AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
                AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 2);
            }
            else
                AppMain.GmSoundChangeAngryBossBGM();
            AppMain.GmMapSetMapDrawSize(7);
        }
        obsObjectWork.pos.y = AppMain.FX_F32_TO_FX32((float)AppMain.GMD_BOSS4_BODY_2ND_POS_Y);
    }

    private static void gmBoss4BodyStateUpdate2ndWaitBoss(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (((int)((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj()).obj_work.move_flag & 1) != 0)
            AppMain.GmBoss4UtilPlayerStop(true);
        float right = (float)AppMain.g_gm_main_system.map_fcol.right;
        if (((AppMain.OBS_OBJECT_WORK)body_work).pos.x < AppMain.FX_F32_TO_FX32(right - 10f))
            return;
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate2nd);
            AppMain.GmBoss4ScrollInit((AppMain.GMS_EVE_RECORD_EVENT)null, 0, 0, (byte)0);
            body_work.wait_timer = AppMain.GMD_BOSS4_BODY_CREATE_CAP_FIRST_TIME;
            AppMain.GmBoss4UtilPlayerStop(false);
        }
    }

    private static void gmBoss4BodyStateUpdate2nd(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateInit2ndAttack);
            body_work.wait_timer = 34U;
            body_work.flag[0] |= 2097152U;
            AppMain.GMS_BOSS4_EGG_WORK partsObj = (AppMain.GMS_BOSS4_EGG_WORK)body_work.parts_objs[1];
            AppMain.GmBoss4UtilGetNodeMatrix(partsObj.node_work, 9);
            AppMain.GmBoss4UtilGetNodeMatrix(partsObj.node_work, 6);
        }
    }

    private static void gmBoss4BodyStateInit2ndAttack(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork1 = (AppMain.OBS_OBJECT_WORK)body_work;
            AppMain.NNS_MATRIX nodeMatrix1 = AppMain.GmBoss4UtilGetNodeMatrix(((AppMain.GMS_BOSS4_EGG_WORK)body_work.parts_objs[1]).node_work, 9);
            AppMain.NNS_MATRIX nodeMatrix2 = AppMain.GmBoss4UtilGetNodeMatrix(body_work.node_work, 2);
            AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            nnsVector.x = (float)((double)nodeMatrix1.M03 - (double)nodeMatrix2.M03 + (double)obsObjectWork1.pos.x / 4096.0);
            nnsVector.y = nodeMatrix1.M13;
            nnsVector.z = nodeMatrix1.M23;
            AppMain.OBS_OBJECT_WORK obsObjectWork2 = AppMain.GmEventMgrLocalEventBirth((ushort)324, AppMain.FX_F32_TO_FX32(nnsVector.x + 0.0f), -AppMain.FX_F32_TO_FX32(nnsVector.y - 22f), (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
            obsObjectWork2.parent_obj = obsObjectWork1;
            AppMain.GmBoss4IncObjCreateCount();
            if (AppMain.gmBoss4ChibiGetThrowType() != 0)
            {
                obsObjectWork2.spd.x = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_1);
                obsObjectWork2.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_1);
            }
            else
            {
                obsObjectWork2.spd.x = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_2);
                obsObjectWork2.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_2);
            }
            obsObjectWork2.pos.z = 0;
            if (AppMain.GmBoss4GetLife() < AppMain.GME_BOSS4_LIFE_H && AppMain.GmBoss4GetLife() > AppMain.GME_BOSS4_LIFE_L)
            {
                body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate2ndAttackWait);
                body_work.wait_timer = AppMain.GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_2_2;
            }
            else
                body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate2ndAttack);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        }
    }

    private static void gmBoss4BodyStateUpdate2ndAttackWait(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateInit2ndAttack2);
            body_work.wait_timer = 34U;
            body_work.flag[0] |= 4194304U;
        }
    }

    private static void gmBoss4BodyStateInit2ndAttack2(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork1 = (AppMain.OBS_OBJECT_WORK)body_work;
            AppMain.NNS_MATRIX nodeMatrix1 = AppMain.GmBoss4UtilGetNodeMatrix(((AppMain.GMS_BOSS4_EGG_WORK)body_work.parts_objs[1]).node_work, 6);
            AppMain.NNS_MATRIX nodeMatrix2 = AppMain.GmBoss4UtilGetNodeMatrix(body_work.node_work, 2);
            AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            nnsVector.x = (float)((double)nodeMatrix1.M03 - (double)nodeMatrix2.M03 + (double)obsObjectWork1.pos.x / 4096.0);
            nnsVector.y = nodeMatrix1.M13;
            nnsVector.z = nodeMatrix1.M23;
            AppMain.OBS_OBJECT_WORK obsObjectWork2 = AppMain.GmEventMgrLocalEventBirth((ushort)324, AppMain.FX_F32_TO_FX32(nnsVector.x + 0.0f), -AppMain.FX_F32_TO_FX32(nnsVector.y - 22f), (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
            obsObjectWork2.parent_obj = obsObjectWork1;
            AppMain.GmBoss4IncObjCreateCount();
            if (AppMain.gmBoss4ChibiGetThrowType() != 0)
            {
                obsObjectWork2.spd.x = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_1);
                obsObjectWork2.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_1);
            }
            else
            {
                obsObjectWork2.spd.x = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_X_2);
                obsObjectWork2.spd.y = AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_BODY_CREATE_CAP_THROW_SPD_Y_2);
            }
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate2ndAttack);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        }
    }

    private static void gmBoss4BodyStateUpdate2ndAttack(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        body_work.wait_timer = AppMain.GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_3;
        if (AppMain.GmBoss4GetLife() == 1)
            body_work.wait_timer = AppMain.GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_1;
        if (AppMain.GmBoss4GetLife() == 2)
            body_work.wait_timer = AppMain.GMD_BOSS4_BODY_CREATE_CAP_TIMING_LIFE_2;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdate2nd);
    }

    private static void gmBoss4BodyStateLeave1stEnd(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss4BodyStateLeave2nd(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss4BodyStateEnterDmgNml(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss4BodyStateLeaveDmgNml(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss4BodyStateEnterDefeat(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        obj_work.flag |= 2U;
        body_work.flag[0] |= 8U;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        body_work.wait_timer = 40U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateDefeatWithWaitStart);
        AppMain.gmBoss4EffAfterburnerSetEnable(body_work, 0);
        AppMain.GmBoss4ScrollNext();
        AppMain.GmBoss4UtilInitNoHitTimer(body_work.nohit_work, (AppMain.GMS_ENEMY_COM_WORK)body_work, 1200);
        AppMain.GmBoss4CapsuleSetInvincible(600, false);
        AppMain.GmBoss4ChibiSetInvincible(true);
    }

    private static void gmBoss4BodyStateLeaveDefeat(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss4BodyStateUpdateDefeatWithWaitStart(
      AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GmBoss4ChibiExplosion();
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.GmBoss4EffBombInitCreate(body_work.bomb_work, 0, AppMain.GMM_BS_OBJ((object)body_work), AppMain.GMM_BS_OBJ((object)body_work).pos.x, AppMain.GMM_BS_OBJ((object)body_work).pos.y, AppMain.FX_F32_TO_FX32(80f), AppMain.FX_F32_TO_FX32(80f), AppMain.GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_INTERVAL_MIN_TIME, AppMain.GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_INTERVAL_MAX_TIME);
            AppMain.GmBoss4EffBombInitCreate(body_work.bomb_work2, 5, AppMain.GMM_BS_OBJ((object)body_work), AppMain.GMM_BS_OBJ((object)body_work).pos.x, AppMain.GMM_BS_OBJ((object)body_work).pos.y, AppMain.FX_F32_TO_FX32(80f), AppMain.FX_F32_TO_FX32(80f), AppMain.GMD_BOSS4_BODY_DEFEAT_BOMB_PARTS_INTERVAL_MIN_TIME, AppMain.GMD_BOSS4_BODY_DEFEAT_BOMB_PARTS_INTERVAL_MAX_TIME);
            body_work.wait_timer = AppMain.GMD_BOSS4_BODY_DEFEAT_BOMB_SMALL_TIME;
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateDefeatWithExplode);
        }
    }

    private static void gmBoss4BodyStateUpdateDefeatWithExplode(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
            AppMain.GmBoss4EffBombUpdateCreate(body_work.bomb_work);
            AppMain.GmBoss4EffBombUpdateCreate(body_work.bomb_work2);
            body_work.bomb_work.pos[0] -= AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_SCROLL_SPD_MAX - AppMain.GMD_BOSS4_SCROLL_SPD_BOSS_BROKEN);
            body_work.bomb_work2.pos[0] -= AppMain.FX_F32_TO_FX32(AppMain.GMD_BOSS4_SCROLL_SPD_MAX - AppMain.GMD_BOSS4_SCROLL_SPD_BOSS_BROKEN);
        }
        else
        {
            AppMain.GmSoundPlaySE("Boss0_03");
            AppMain.GMM_PAD_VIB_MID_TIME(120f);
            AppMain.GmBsCmnInitFlashScreen(body_work.flash_work, 4f, 5f, 30f);
            AppMain.GmPlayerAddScoreNoDisp((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), 1000);
            AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 8);
            obsObjectWork.pos.z = obsObjectWork.parent_obj.pos.z + 131072;
            AppMain.GmBoss4EffChangeType((AppMain.GMS_EFFECT_3DES_WORK)obsObjectWork, 2U, 1U);
            obsObjectWork.spd.x -= AppMain.FX_F32_TO_FX32(1f);
            AppMain.gmBoss4BodySetActionWhole(body_work, 7);
            AppMain.GmBoss4ScrollOut();
            body_work.wait_timer = 40U;
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateDefeatWithScatter);
        }
    }

    private static void gmBoss4BodyStateUpdateDefeatWithScatter(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.GmBsCmnUpdateFlashScreen(body_work.flash_work);
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss4SetPartTextureBurnt(AppMain.GMM_BS_OBJ((object)body_work));
            body_work.flag[0] |= 16777216U;
            AppMain.gmBoss4EffABSmokeInit(body_work);
            AppMain.gmBoss4EffBodySmokeInit(body_work);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateDefeatWithWaitEnd);
        }
    }

    private static void gmBoss4BodyStateUpdateDefeatWithWaitEnd(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
            --body_work.wait_timer;
        else
            AppMain.gmBoss4BodyChangeState(body_work, 8);
    }

    private static void gmBoss4BodyStateEnterEscape(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.flag |= 2U;
        obsObjectWork.disp_flag &= 4294967279U;
        AppMain.gmBoss4BodySetActionWhole(body_work, 7);
        body_work.flag[0] |= 8388608U;
        bool is_positive = AppMain.GmBoss4UtilIsDirectionPositiveFromCurrent(body_work.dir, AppMain.GMD_BOSS4_RIGHTWARD_ANGLE);
        AppMain.GmBoss4UtilInitTurnGently(body_work.dir, AppMain.GMD_BOSS4_RIGHTWARD_ANGLE, 90, is_positive);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateEscapeWithTurn);
    }

    private static void gmBoss4BodyStateLeaveEscape(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss4BodyStateUpdateEscapeWithTurn(AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (!AppMain.GmBoss4UtilUpdateTurnGently(body_work.dir))
            return;
        AppMain.gmBoss4BodyInitEscapeMove(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateEscapeWithMoveLocked);
    }

    private static void gmBoss4BodyStateUpdateEscapeWithMoveLocked(
      AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.gmBoss4BodyUpdateEscapeMove(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateEscapeWithMoveUnlocked);
    }

    private static void gmBoss4BodyStateUpdateEscapeWithMoveUnlocked(
      AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK parent_obj = (AppMain.OBS_OBJECT_WORK)body_work;
        float right = (float)AppMain.g_gm_main_system.map_fcol.right;
        AppMain.gmBoss4BodyUpdateEscapeMove(body_work);
        if (parent_obj.pos.x <= AppMain.FX_F32_TO_FX32(right - 100f))
            return;
        AppMain.GmBoss4EffChangeType(AppMain.GmEfctBossCmnEsCreate(parent_obj, 4U), 2U, 3U);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK(AppMain.gmBoss4BodyStateUpdateEscapeWithMoveFinish);
        AppMain.GmMapSetMapDrawSize(1);
    }

    private static void gmBoss4BodyStateUpdateEscapeWithMoveFinish(
      AppMain.GMS_BOSS4_BODY_WORK body_work)
    {
        if (!AppMain.gmBoss4BodyUpdateEscapeMove(body_work))
            return;
        AppMain.GMM_BOSS4_MGR(body_work).flag |= 2U;
        body_work.proc_update = (AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK)null;
        AppMain.GmBoss4ScrollNext();
    }

}