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
    public static AppMain.AMS_AMB_HEADER GMD_BOSS1_ARC
    {
        get
        {
            return AppMain.g_gm_gamedat_enemy_arc;
        }
    }

    public static int GMM_BOSS1_STAGE_MAP_POS_OFST_Y()
    {
        return AppMain.GMM_MAIN_GET_ZONE_TYPE() != 4 ? 0 : 14155776;
    }

    public static int GMD_BOSS1_GROUND_POS_Y
    {
        get
        {
            return 1286144 + AppMain.GMM_BOSS1_STAGE_MAP_POS_OFST_Y();
        }
    }

    public static int GMD_BOSS1_BODY_DEFAULT_ALTITUDE
    {
        get
        {
            return 712704 + AppMain.GMM_BOSS1_STAGE_MAP_POS_OFST_Y();
        }
    }

    public static int GMD_BOSS1_BODY_START_POS_Y
    {
        get
        {
            return AppMain.GMM_BOSS1_STAGE_MAP_POS_OFST_Y() - 245760;
        }
    }

    public static int GMD_BOSS1_BODY_ATKBASH_TARG_Y
    {
        get
        {
            return AppMain.GMD_BOSS1_GROUND_POS_Y - 360448;
        }
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss1BodyInit(
     AppMain.GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS1_BODY_WORK()), "BOSS1_BODY");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS1_BODY_WORK body_work = (AppMain.GMS_BOSS1_BODY_WORK)work;
        work.pos.y = AppMain.GMD_BOSS1_BODY_START_POS_Y;
        work.pos.z = 0;
        body_work.atk_nml_alt = AppMain.GMD_BOSS1_BODY_DEFAULT_ALTITUDE;
        work.flag |= 16U;
        work.disp_flag |= 4194309U;
        work.move_flag |= 4096U;
        work.move_flag &= 4294967167U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        gmsEnemy3DWork.ene_com.vit = (byte)1;
        AppMain.gmBoss1BodySetDmgRectSizeToDefault(body_work);
        gmsEnemy3DWork.ene_com.rect_work[0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss1BodyDamageDefFunc);
        gmsEnemy3DWork.ene_com.rect_work[1].flag |= 2048U;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_boss1_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(703), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = 0.125f;
        work.disp_flag |= 134217728U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1BodyWaitSetup);
        AppMain.gmBoss1BodyChangeState(body_work, 0);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1BodyOutFunc);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss1BodyExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss1BodyExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        AppMain.GMS_BOSS1_BODY_WORK gmsBosS1BodyWork = (AppMain.GMS_BOSS1_BODY_WORK)tcbWork;
        AppMain.gmBoss1MgrDecObjCreateCount(gmsBosS1BodyWork.mgr_work);
        AppMain.GmBsCmnClearBossMotionCBSystem(tcbWork);
        AppMain.GmBsCmnDeleteSNMWork(gmsBosS1BodyWork.snm_work);
        AppMain.GmBsCmnClearCNMCb(tcbWork);
        AppMain.GmBsCmnDeleteCNMMgrWork(gmsBosS1BodyWork.cnm_mgr_work);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss1BodySetActionWhole(AppMain.GMS_BOSS1_BODY_WORK body_work, int act_id)
    {
        AppMain.gmBoss1BodySetActionWhole(body_work, act_id, false);
    }

    private static void gmBoss1BodySetActionWhole(
      AppMain.GMS_BOSS1_BODY_WORK body_work,
      int act_id,
      bool force_change)
    {
        AppMain.GMS_BOSS1_PART_ACT_INFO[] bosS1PartActInfoArray = AppMain.gm_boss1_act_id_tbl[act_id];
        if (!force_change && body_work.whole_act_id == act_id)
            return;
        body_work.whole_act_id = act_id;
        for (int index = 0; index < 3; ++index)
        {
            if (body_work.parts_objs[index] != null)
            {
                if (index == 2)
                {
                    AppMain.GMS_BOSS1_EGG_WORK partsObj = (AppMain.GMS_BOSS1_EGG_WORK)body_work.parts_objs[index];
                    body_work.egg_revert_mtn_id = bosS1PartActInfoArray[index].act_id;
                    if (((int)partsObj.flag & 1) != 0)
                        continue;
                }
                if (bosS1PartActInfoArray[index].is_maintain == (byte)0)
                    AppMain.GmBsCmnSetAction(body_work.parts_objs[index], (int)bosS1PartActInfoArray[index].act_id, (int)bosS1PartActInfoArray[index].is_repeat, bosS1PartActInfoArray[index].is_blend ? 1 : 0);
                else if (bosS1PartActInfoArray[index].is_repeat != (byte)0)
                    AppMain.GMM_BS_OBJ((object)body_work).disp_flag |= 4U;
                if (bosS1PartActInfoArray[index].is_blend && bosS1PartActInfoArray[index].is_merge_manual)
                {
                    if (index == 1)
                    {
                        AppMain.GMS_BOSS1_CHAIN_WORK partsObj = (AppMain.GMS_BOSS1_CHAIN_WORK)body_work.parts_objs[1];
                        partsObj.flag |= 1U;
                        AppMain.GMM_BS_OBJ((object)partsObj).disp_flag |= 4U;
                    }
                    else
                        AppMain.MTM_ASSERT(false);
                }
                body_work.parts_objs[index].obj_3d.speed[0] = bosS1PartActInfoArray[index].mtn_spd;
                body_work.parts_objs[index].obj_3d.blend_spd = bosS1PartActInfoArray[index].blend_spd;
            }
        }
    }

    private static void gmBoss1BodySetSuspendAction(
      AppMain.GMS_BOSS1_BODY_WORK body_work,
      int part_idx,
      uint suspend_time)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = body_work.parts_objs[part_idx].obj_3d;
        AppMain.MTM_ASSERT(part_idx == 1);
        obj3d.speed[0] = 0.0f;
        body_work.mtn_suspend[part_idx].is_suspended = true;
        body_work.mtn_suspend[part_idx].suspend_timer = suspend_time;
    }

    private static void gmBoss1BodyUpdateSuspendAction(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        for (int index = 0; index < 3; ++index)
        {
            AppMain.GMS_BOSS1_MTN_SUSPEND_WORK s1MtnSuspendWork = body_work.mtn_suspend[index];
            if (s1MtnSuspendWork.is_suspended)
            {
                AppMain.MTM_ASSERT(index == 1);
                if (s1MtnSuspendWork.suspend_timer != 0U)
                {
                    --s1MtnSuspendWork.suspend_timer;
                }
                else
                {
                    body_work.parts_objs[index].obj_3d.speed[0] = AppMain.gm_boss1_act_id_tbl[body_work.whole_act_id][index].mtn_spd;
                    s1MtnSuspendWork.is_suspended = false;
                }
            }
        }
    }

    private static bool gmBoss1BodyCheckChainMotionMergeEnd(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        return ((int)((AppMain.GMS_BOSS1_CHAIN_WORK)body_work.parts_objs[1]).flag & 1) == 0;
    }

    private static void gmBoss1BodySetNoHitTime(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)body_work;
        body_work.no_hit_timer = 10U;
        gmsEnemyComWork.rect_work[0].flag |= 2048U;
    }

    private static void gmBoss1BodyUpdateNoHitTime(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.no_hit_timer != 0U)
            --body_work.no_hit_timer;
        else
            ((AppMain.GMS_ENEMY_COM_WORK)body_work).rect_work[0].flag &= 4294965247U;
    }

    private static void gmBoss1BodyExecDamageRoutine(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.GMS_BOSS1_MGR_WORK mgrWork = body_work.mgr_work;
        AppMain.MTM_ASSERT((object)mgrWork);
        if (mgrWork.life != 0)
            --mgrWork.life;
        if (0 < mgrWork.life)
        {
            body_work.flag |= 1073741824U;
        }
        else
        {
            body_work.flag |= 2147483648U;
            AppMain.GMM_BS_OBJ((object)body_work).flag |= 2U;
        }
    }

    private static void gmBoss1BodySetDmgRectSizeForAtkNml(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.ObjRectWorkSet(((AppMain.GMS_ENEMY_COM_WORK)body_work).rect_work[0], (short)-24, (short)-24, (short)24, (short)16);
    }

    private static void gmBoss1BodySetDmgRectSizeToDefault(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.ObjRectWorkSet(((AppMain.GMS_ENEMY_COM_WORK)body_work).rect_work[0], (short)-24, (short)-24, (short)24, (short)24);
    }

    private static void gmBoss1BodySetAtkRectToWeakAttacker(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)body_work;
        AppMain.ObjRectWorkSet(gmsEnemyComWork.rect_work[1], (short)-16, (short)-16, (short)16, (short)16);
        gmsEnemyComWork.rect_work[1].flag &= 4294965247U;
    }

    private static void gmBoss1BodySetAtkRectToNormal(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)body_work;
        AppMain.ObjRectWorkSet(gmsEnemyComWork.rect_work[1], (short)0, (short)0, (short)0, (short)0);
        gmsEnemyComWork.rect_work[1].flag |= 2048U;
    }

    private static bool gmBoss1BodyIsExtraAttack(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        return AppMain.GMM_BOSS1_MGR(body_work).life <= 3;
    }

    private static bool gmBoss1BodyIsEscapeScrUnlock(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        return AppMain.GMM_BS_OBJ((object)body_work).pos.x >= AppMain.GMM_BOSS1_AREA_RIGHT() - 131072;
    }

    private static bool gmBoss1BodyIsEscapeOutFinalZone(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        return AppMain.GMM_BS_OBJ((object)body_work).pos.x >= AppMain.GMM_BOSS1_AREA_RIGHT() + 393216;
    }

    private static bool gmBoss1BodyIsDirectionPositiveFromCurrent(
      AppMain.GMS_BOSS1_BODY_WORK body_work,
      short target_angle)
    {
        return (int)((long)ushort.MaxValue & (long)((int)body_work.cur_angle - (int)target_angle)) >= AppMain.AKM_DEGtoA32(180);
    }

    private static void gmBoss1BodyUpdateDirection(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.GMM_BS_OBJ((object)body_work).dir.y = (ushort)body_work.cur_angle;
    }

    private static void gmBoss1BodySetDirectionNormal(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)body_work).disp_flag & 1) != 0)
            AppMain.gmBoss1BodySetDirection(body_work, AppMain.GMD_BOSS1_LEFTWARD_ANGLE);
        else
            AppMain.gmBoss1BodySetDirection(body_work, AppMain.GMD_BOSS1_RIGHTWARD_ANGLE);
        body_work.orig_angle = (short)0;
        body_work.turn_angle = 0;
    }

    private static void gmBoss1BodySetDirection(AppMain.GMS_BOSS1_BODY_WORK body_work, short deg)
    {
        body_work.cur_angle = deg;
    }

    private static void gmBoss1BodyInitTurnGently(
      AppMain.GMS_BOSS1_BODY_WORK body_work,
      short dest_angle,
      int frame,
      bool is_positive)
    {
        AppMain.MTM_ASSERT(frame > 0);
        body_work.orig_angle = body_work.cur_angle;
        body_work.turn_angle = 0;
        body_work.turn_spd = 0;
        if (is_positive)
        {
            ushort num = (ushort)((uint)dest_angle - (uint)body_work.cur_angle);
            body_work.turn_amount = (int)num;
        }
        else
        {
            ushort num = (ushort)((int)dest_angle - AppMain.AKM_DEGtoA32(360) - ((int)body_work.cur_angle - AppMain.AKM_DEGtoA32(360)));
            body_work.turn_amount = (int)num - AppMain.AKM_DEGtoA32(360);
        }
        body_work.turn_gen_var = 0;
        float num1 = 180f / (float)frame;
        AppMain.MTM_ASSERT((double)AppMain.MTM_MATH_ABS(num1) <= 2147483648.0);
        body_work.turn_gen_factor = AppMain.AKM_DEGtoA32(num1);
        AppMain.gmBoss1BodySetDirection(body_work, (short)((int)body_work.orig_angle + body_work.turn_angle));
    }

    private static bool gmBoss1BodyUpdateTurnGently(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        bool flag = false;
        AppMain.MTM_ASSERT(body_work.turn_gen_factor > 0);
        body_work.turn_gen_var += body_work.turn_gen_factor;
        if (body_work.turn_gen_var >= AppMain.AKM_DEGtoA32(180))
        {
            body_work.turn_gen_var = AppMain.AKM_DEGtoA32(180);
            flag = true;
        }
        float a = (float)((double)body_work.turn_amount * 0.5 * (1.0 - (double)AppMain.nnCos(body_work.turn_gen_var)));
        AppMain.MTM_ASSERT((double)AppMain.MTM_MATH_ABS(a) <= 2147483648.0);
        body_work.turn_angle = (int)a;
        if (flag)
            body_work.turn_angle = body_work.turn_amount;
        AppMain.gmBoss1BodySetDirection(body_work, (short)((int)body_work.orig_angle + body_work.turn_angle));
        return flag;
    }

    private static void gmBoss1BodyInitPreANChainMotion(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.MTM_ASSERT((object)body_work.parts_objs[1]);
        body_work.parts_objs[1].obj_3d.frame[0] = 160f;
    }

    private static void gmBoss1BodyInitPreANMove(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.spd.x = 0;
        obsObjectWork.spd_add.x = -81;
    }

    private static bool gmBoss1BodyUpdatePreANMove(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        bool flag = false;
        if (AppMain.MTM_MATH_ABS(obj_work.spd.x) >= 7372)
        {
            obj_work.spd.x = -7372;
            obj_work.spd_add.x = 0;
        }
        if (obj_work.pos.x <= AppMain.GMM_BOSS1_AREA_LEFT() + 589824)
        {
            obj_work.pos.x = AppMain.GMM_BOSS1_AREA_LEFT() + 589824;
            flag = true;
        }
        if (flag)
            AppMain.GmBsCmnSetObjSpdZero(obj_work);
        return flag;
    }

    private static void gmBoss1BodySetANChainInitialBlendSpd(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.MTM_ASSERT((object)body_work.parts_objs[1]);
        body_work.parts_objs[1].obj_3d.blend_spd = 0.01f;
    }

    private static void gmBoss1BodyInitAtkNmlMove(AppMain.GMS_BOSS1_BODY_WORK body_work, int frame)
    {
        body_work.move_time = frame;
        body_work.move_cnt = 0;
        AppMain.gmBoss1BodyUpdateAtkNmlMove(body_work);
    }

    private static bool gmBoss1BodyUpdateAtkNmlMove(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num1 = AppMain.GMM_BOSS1_AREA_LEFT() + 589824;
        int num2 = AppMain.GMM_BOSS1_AREA_LEFT() + 983040;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            int num3 = num1;
            num1 = num2;
            num2 = num3;
        }
        bool flag;
        if (body_work.move_cnt < body_work.move_time)
        {
            int moveCurveAngleWidth = AppMain.GMD_BOSS1_BODY_ATKNML_MOVE_CURVE_ANGLE_WIDTH;
            int moveCurveStartAngle = AppMain.GMD_BOSS1_BODY_ATKNML_MOVE_CURVE_START_ANGLE;
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

    private static void gmBoss1BodySetFlipForAtkNmlMove(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num = AppMain.GMM_BOSS1_AREA_CENTER_X();
        if (obsObjectWork.pos.x < num)
            obsObjectWork.disp_flag &= 4294967294U;
        else
            obsObjectWork.disp_flag |= 1U;
    }

    private static void gmBoss1BodyInitAtkNmlFlipAndTurn(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int frame = 40;
        AppMain.MTM_ASSERT(frame < 72);
        AppMain.gmBoss1BodySetFlipForAtkNmlMove(body_work);
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            AppMain.gmBoss1BodyInitTurnGently(body_work, AppMain.GMD_BOSS1_LEFTWARD_ANGLE, frame, true);
        else
            AppMain.gmBoss1BodyInitTurnGently(body_work, AppMain.GMD_BOSS1_RIGHTWARD_ANGLE, frame, false);
    }

    private static bool gmBoss1BodyUpdateAtkNmlFlipAndTurn(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        return AppMain.gmBoss1BodyUpdateTurnGently(body_work);
    }

    private static void gmBoss1BodyInitAtkNmlDrift(AppMain.GMS_BOSS1_BODY_WORK body_work, int frame)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.MTM_ASSERT(frame > 0);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        body_work.drift_angle = 0;
        body_work.drift_ang_spd = (int)AppMain.nnRoundOff((float)((double)AppMain.AKM_DEGtoA32(180f) / (double)frame + 0.5));
        body_work.drift_timer = frame;
        body_work.drift_pivot_x = ((int)obj_work.disp_flag & 1) == 0 ? AppMain.GMM_BOSS1_AREA_LEFT() + 589824 : AppMain.GMM_BOSS1_AREA_LEFT() + 983040;
        AppMain.gmBoss1BodyUpdateAtkNmlDrift(body_work);
    }

    private static bool gmBoss1BodyUpdateAtkNmlDrift(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        int num;
        bool flag;
        if (body_work.drift_timer != 0)
        {
            --body_work.drift_timer;
            body_work.drift_angle = (int)((long)ushort.MaxValue & (long)(body_work.drift_angle + body_work.drift_ang_spd));
            num = AppMain.FX_Mul(AppMain.FX_Sin(body_work.drift_angle), 131072);
            flag = false;
        }
        else
        {
            num = 0;
            flag = true;
        }
        if (((int)obsObjectWork.disp_flag & 1) == 0)
            num = -num;
        obsObjectWork.pos.x = body_work.drift_pivot_x + num;
        return flag;
    }

    private static void gmBoss1BodyInitRush(AppMain.GMS_BOSS1_BODY_WORK body_work, bool is_left)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (is_left)
        {
            body_work.bash_targ_pos.x = AppMain.GMM_BOSS1_AREA_LEFT() + 720896;
            body_work.bash_targ_pos.y = AppMain.GMD_BOSS1_BODY_ATKBASH_TARG_Y;
            body_work.bash_targ_pos.z = 0;
        }
        else
        {
            body_work.bash_targ_pos.x = AppMain.GMM_BOSS1_AREA_LEFT() + 851968;
            body_work.bash_targ_pos.y = AppMain.GMD_BOSS1_BODY_ATKBASH_TARG_Y;
            body_work.bash_targ_pos.z = 0;
        }
        int num1 = (body_work.bash_targ_pos.x - obsObjectWork.pos.x) / 39;
        int num2 = (body_work.bash_targ_pos.y - obsObjectWork.pos.y) / 39;
        obsObjectWork.spd_add.x = (int)((double)num1 * (1.0 / 32.0));
        obsObjectWork.spd_add.y = (int)((double)num2 * (1.0 / 32.0));
        obsObjectWork.spd.x = 0;
        obsObjectWork.spd.y = 0;
    }

    private static bool gmBoss1BodyUpdateRush(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.NNS_VECTOR nnsVector1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.amVectorSet(nnsVector1, AppMain.FX_FX32_TO_F32(body_work.bash_targ_pos.x) - AppMain.FX_FX32_TO_F32(obj_work.pos.x), AppMain.FX_FX32_TO_F32(body_work.bash_targ_pos.y) - AppMain.FX_FX32_TO_F32(obj_work.pos.y), 0.0f);
        AppMain.amVectorSet(nnsVector2, AppMain.FX_FX32_TO_F32(obj_work.spd.x), AppMain.FX_FX32_TO_F32(obj_work.spd.y), 0.0f);
        if (0.0 >= (double)AppMain.nnDotProductVector(nnsVector2, nnsVector1))
        {
            AppMain.GmBsCmnSetObjSpdZero(obj_work);
            AppMain.VEC_Set(ref obj_work.pos, body_work.bash_targ_pos.x, body_work.bash_targ_pos.y, body_work.bash_targ_pos.z);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector1);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector2);
            return true;
        }
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector2);
        return false;
    }

    private static void gmBoss1BodyInitBashReturn(AppMain.GMS_BOSS1_BODY_WORK body_work, bool is_left)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        if (is_left)
        {
            body_work.bash_ret_pos.x = AppMain.GMM_BOSS1_AREA_LEFT() + 524288;
            body_work.bash_ret_pos.y = body_work.atk_nml_alt;
            body_work.bash_ret_pos.z = 0;
        }
        else
        {
            body_work.bash_ret_pos.x = AppMain.GMM_BOSS1_AREA_LEFT() + 1048576;
            body_work.bash_ret_pos.y = body_work.atk_nml_alt;
            body_work.bash_ret_pos.z = 0;
        }
        AppMain.VEC_Set(ref body_work.bash_orig_pos, obsObjectWork.pos.x, obsObjectWork.pos.y, obsObjectWork.pos.z);
        body_work.bash_homing_deg = 0;
    }

    private static bool gmBoss1BodyUpdateBashReturn(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        body_work.bash_homing_deg += AppMain.AKM_DEGtoA32(3);
        if (body_work.bash_homing_deg >= AppMain.AKM_DEGtoA32(180))
        {
            body_work.bash_homing_deg = AppMain.AKM_DEGtoA32(180);
            obsObjectWork.pos.x = body_work.bash_ret_pos.x;
            obsObjectWork.pos.y = body_work.bash_ret_pos.y;
            return true;
        }
        obsObjectWork.pos.x = body_work.bash_orig_pos.x + AppMain.FX_Mul(body_work.bash_ret_pos.x - body_work.bash_orig_pos.x, 4096 - AppMain.mtMathCos(body_work.bash_homing_deg) >> 1);
        obsObjectWork.pos.y = body_work.bash_orig_pos.y + AppMain.FX_Mul(body_work.bash_ret_pos.y - body_work.bash_orig_pos.y, 4096 - AppMain.mtMathCos(body_work.bash_homing_deg) >> 1);
        return false;
    }

    private static void gmBoss1BodyInitEscapeMove(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.spd_add.x = ((int)obj_work.disp_flag & 1) == 0 ? 409 : -409;
        obj_work.spd_add.y = -256;
    }

    private static bool gmBoss1BodyUpdateEscapeMove(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        bool flag = false;
        if (AppMain.MTM_MATH_ABS(obj_work.spd.x) >= 4915)
        {
            obj_work.spd.x = 4915;
            obj_work.spd.y = -3072;
            obj_work.spd_add.x = 0;
            obj_work.spd_add.y = 0;
        }
        if (obj_work.pos.y < 0)
            flag = true;
        else if (obj_work.pos.x >= (AppMain.g_gm_main_system.map_size[0] << 12) + 262144)
            flag = true;
        if (flag)
            AppMain.GmBsCmnSetObjSpdZero(obj_work);
        return flag;
    }

    private static void gmBoss1BodyInitDefeatState(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        bool flag = false;
        if (((int)body_work.flag & 1) != 0)
            flag = true;
        AppMain.gmBoss1BodyChangeState(body_work, 7, true);
        if (flag)
            body_work.flag |= 1U;
        else
            body_work.flag &= 4294967294U;
    }

    private static void gmBoss1BodyUpdateChainTopDirection(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        if (((int)body_work.flag & 1) == 0)
        {
            AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(body_work.snm_work, body_work.chaintop_snm_reg_id);
            AppMain.nnRotateYMatrix(nnsMatrix, snmMtx, (int)-AppMain.GMM_BS_OBJ((object)body_work).dir.y + (int)AppMain.AKM_DEGtoA16(90));
            AppMain.GmBsCmnSetCNMMtx(body_work.cnm_mgr_work, nnsMatrix, body_work.chaintop_cnm_reg_id);
            AppMain.GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, body_work.chaintop_cnm_reg_id, 1);
        }
        else
            AppMain.GmBsCmnEnableCNMMtxNode(body_work.cnm_mgr_work, body_work.chaintop_cnm_reg_id, 0);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss1BodyDamageDefFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        AppMain.GMS_BOSS1_BODY_WORK body_work = (AppMain.GMS_BOSS1_BODY_WORK)parentObj1;
        if (parentObj2 == null || (ushort)1 != parentObj2.obj_type)
            return;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)parentObj2;
        AppMain.GmPlySeqAtkReactionInit(ply_work);
        AppMain.GmPlySeqSetJumpState(ply_work, 0, 5U);
        if (ply_work.seq_state == 20)
        {
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ply_work.obj_work.move.x < 0 ? 20480 : -20480;
            ply_work.obj_work.spd.y = parentObj2.pos.y > parentObj1.pos.y ? 16384 : -16384;
            AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, 102400);
        }
        else
        {
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ply_work.obj_work.move.x < 0 ? 16384 : -16384;
            ply_work.obj_work.spd.y = parentObj2.pos.y > parentObj1.pos.y ? 12288 : -12288;
            AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, 102400);
        }
        AppMain.gmBoss1BodySetNoHitTime(body_work);
        AppMain.GmSoundPlaySE("Boss0_01");
        AppMain.GMM_PAD_VIB_SMALL_TIME(30f);
        AppMain.gmBoss1EffDamageInit(body_work);
        if (((int)body_work.flag & 4) != 0)
            return;
        AppMain.gmBoss1BodyExecDamageRoutine(body_work);
    }

    private static void gmBoss1BodyOutFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_BODY_WORK gmsBosS1BodyWork = (AppMain.GMS_BOSS1_BODY_WORK)obj_work;
        AppMain.GmBsCmnUpdateCNMParam(obj_work, gmsBosS1BodyWork.cnm_mgr_work);
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static void gmBoss1BodyChangeState(AppMain.GMS_BOSS1_BODY_WORK body_work, int state)
    {
        AppMain.gmBoss1BodyChangeState(body_work, state, false);
    }

    private static void gmBoss1BodyChangeState(
      AppMain.GMS_BOSS1_BODY_WORK body_work,
      int state,
      bool is_wrapped)
    {
        AppMain.UNREFERENCED_PARAMETER((object)is_wrapped);
        AppMain.GMF_BOSS1_BODY_STATE_LEAVE_FUNC bodyStateLeaveFunc = AppMain.gm_boss1_body_state_leave_func_tbl[body_work.state];
        if (bodyStateLeaveFunc != null)
            bodyStateLeaveFunc(body_work);
        body_work.prev_state = body_work.state;
        body_work.state = state;
        AppMain.GMS_BOSS1_BODY_STATE_ENTER_INFO bodyStateEnterInfo = AppMain.gm_boss1_body_state_enter_info_tbl[body_work.state];
        if (bodyStateEnterInfo.enter_func == null)
            return;
        bodyStateEnterInfo.enter_func(body_work);
    }

    private static void gmBoss1BodyWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_BODY_WORK body_work = (AppMain.GMS_BOSS1_BODY_WORK)obj_work;
        if (((int)body_work.mgr_work.flag & 1) == 0)
            return;
        AppMain.GmBsCmnInitBossMotionCBSystem(obj_work, body_work.bmcb_mgr);
        AppMain.GmBsCmnCreateSNMWork(body_work.snm_work, obj_work.obj_3d._object, (ushort)4);
        AppMain.GmBsCmnAppendBossMotionCallback(body_work.bmcb_mgr, body_work.snm_work.bmcb_link);
        body_work.chain_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, 13);
        body_work.egg_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, 11);
        body_work.body_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, 2);
        body_work.chaintop_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(body_work.snm_work, 9);
        AppMain.GmBsCmnCreateCNMMgrWork(body_work.cnm_mgr_work, obj_work.obj_3d._object, (ushort)1);
        AppMain.GmBsCmnInitCNMCb(obj_work, body_work.cnm_mgr_work);
        body_work.chaintop_cnm_reg_id = AppMain.GmBsCmnRegisterCNMNode(body_work.cnm_mgr_work, 9);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1BodyMain);
        AppMain.gmBoss1BodyChangeState(body_work, 1);
    }

    private static void gmBoss1BodyMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_BODY_WORK gmsBosS1BodyWork = (AppMain.GMS_BOSS1_BODY_WORK)obj_work;
        AppMain.gmBoss1BodyUpdateNoHitTime(gmsBosS1BodyWork);
        if (((int)gmsBosS1BodyWork.flag & int.MinValue) != 0)
        {
            gmsBosS1BodyWork.flag &= 1073741823U;
            AppMain.gmBoss1BodyInitDefeatState(gmsBosS1BodyWork);
        }
        else if (gmsBosS1BodyWork.proc_update != null)
            gmsBosS1BodyWork.proc_update(gmsBosS1BodyWork);
        AppMain.gmBoss1BodyUpdateSuspendAction(gmsBosS1BodyWork);
        AppMain.gmBoss1EffAfterburnerUpdateCreate(gmsBosS1BodyWork);
        if (((int)gmsBosS1BodyWork.flag & 1073741824) != 0)
        {
            gmsBosS1BodyWork.flag &= 3221225471U;
            gmsBosS1BodyWork.flag |= 536870912U;
            AppMain.GmBsCmnInitObject3DNNDamageFlicker(obj_work, gmsBosS1BodyWork.flk_work, 32f);
        }
        AppMain.GmBsCmnUpdateObject3DNNDamageFlicker(obj_work, gmsBosS1BodyWork.flk_work);
        AppMain.gmBoss1BodyUpdateDirection(gmsBosS1BodyWork);
        AppMain.gmBoss1BodyUpdateChainTopDirection(gmsBosS1BodyWork);
    }

    private static void gmBoss1BodyStateEnterStart(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.flag |= 2U;
        body_work.flag |= 64U;
        AppMain.gmBoss1BodySetActionWhole(body_work, 0, true);
        body_work.flag |= 32U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        AppMain.gmBoss1BodySetDirectionNormal(body_work);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateStartWithWaitLockBegin);
    }

    private static void gmBoss1BodyStateLeaveStart(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obsObjectWork;
        AppMain.gmBoss1EffAfterburnerSetEnable(body_work, false);
        gmsEnemy3DWork.ene_com.enemy_flag &= 4294934527U;
        body_work.flag &= 4294967231U;
        obsObjectWork.flag &= 4294967293U;
        body_work.flag &= 4294967263U;
    }

    private static void gmBoss1BodyStateUpdateStartWithWaitLockBegin(
      AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (!AppMain.gmBoss1IsScrollLockBusy())
            return;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateStartWithWaitLockComplete);
    }

    private static void gmBoss1BodyStateUpdateStartWithWaitLockComplete(
      AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.gmBoss1IsScrollLockBusy())
            return;
        AppMain.GmMapSetMapDrawSize(3);
        AppMain.GmBsCmnSetObjSpd(obj_work, 0, 4096, 0);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateStartWithFall);
    }

    private static void gmBoss1BodyStateUpdateStartWithFall(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (obj_work.pos.y < body_work.atk_nml_alt)
            return;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.pos.y = body_work.atk_nml_alt;
        AppMain.GmBsCmnSetObjSpd(obj_work, -4096, 0, 0);
        AppMain.gmBoss1EffAfterburnerSetEnable(body_work, true);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateStartWithMove);
    }

    private static void gmBoss1BodyStateUpdateStartWithMove(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        int num = AppMain.GMM_BOSS1_AREA_LEFT() + 786432;
        if (obj_work.pos.x > num)
            return;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.pos.x = num;
        body_work.wait_timer = 10U;
        AppMain.gmBoss1EffAfterburnerSetEnable(body_work, false);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateStartWithWaitEnd);
    }

    private static void gmBoss1BodyStateUpdateStartWithWaitEnd(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else
            AppMain.gmBoss1BodyChangeState(body_work, 2);
    }

    private static void gmBoss1BodyStateEnterPrep(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.flag |= 2U;
        body_work.flag |= 64U;
        AppMain.gmBoss1BodySetActionWhole(body_work, 2);
        body_work.flag &= 4294967263U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        body_work.wait_timer = 95U;
        AppMain.GmSoundPlaySE("Boss1_01");
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdatePrepWithWait);
    }

    private static void gmBoss1BodyStateLeavePrep(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        ((AppMain.GMS_ENEMY_3D_WORK)obsObjectWork).ene_com.enemy_flag &= 4294934527U;
        body_work.flag &= 4294967231U;
        obsObjectWork.flag &= 4294967293U;
    }

    private static void gmBoss1BodyStateUpdatePrepWithWait(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (body_work.wait_timer != 0U)
            --body_work.wait_timer;
        else
            body_work.flag &= 4294967231U;
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        AppMain.gmBoss1BodyChangeState(body_work, 3);
    }

    private static void gmBoss1BodyStateEnterPreAtkNml(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss1BodySetDmgRectSizeForAtkNml(body_work);
        obsObjectWork.flag &= 4294967293U;
        AppMain.gmBoss1BodySetActionWhole(body_work, 3);
        AppMain.gmBoss1BodyInitPreANChainMotion(body_work);
        AppMain.gmBoss1BodySetFlipForAtkNmlMove(body_work);
        AppMain.gmBoss1BodyInitPreANMove(body_work);
        AppMain.gmBoss1EffAfterburnerSetEnable(body_work, true);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdatePreAtkNmlWithMove);
    }

    private static void gmBoss1BodyStateLeavePreAtkNml(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.gmBoss1EffAfterburnerSetEnable(body_work, false);
        AppMain.gmBoss1BodySetDmgRectSizeToDefault(body_work);
    }

    private static void gmBoss1BodyStateUpdatePreAtkNmlWithMove(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.gmBoss1BodySetDirectionNormal(body_work);
        if (!AppMain.gmBoss1BodyUpdatePreANMove(body_work))
            return;
        AppMain.gmBoss1BodyChangeState(body_work, 4);
        AppMain.gmBoss1BodySetANChainInitialBlendSpd(body_work);
    }

    private static void gmBoss1BodyStateEnterAtkNml(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        bool force_change = false;
        AppMain.gmBoss1BodySetDmgRectSizeForAtkNml(body_work);
        obsObjectWork.flag &= 4294967293U;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            force_change = true;
        AppMain.gmBoss1BodySetActionWhole(body_work, 4, force_change);
        AppMain.gmBoss1BodyInitAtkNmlFlipAndTurn(body_work);
        AppMain.gmBoss1BodyInitAtkNmlDrift(body_work, 72);
        AppMain.gmBoss1EffAfterburnerSetEnable(body_work, false);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateAtkNmlWithTurn);
    }

    private static void gmBoss1BodyStateLeaveAtkNml(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.gmBoss1EffAfterburnerSetEnable(body_work, false);
        AppMain.gmBoss1BodySetDmgRectSizeToDefault(body_work);
    }

    private static void gmBoss1BodyStateUpdateAtkNmlWithTurn(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        bool flag = AppMain.gmBoss1BodyUpdateAtkNmlDrift(body_work);
        if (!AppMain.gmBoss1BodyUpdateAtkNmlFlipAndTurn(body_work) || !flag)
            return;
        AppMain.gmBoss1BodySetFlipForAtkNmlMove(body_work);
        AppMain.gmBoss1BodyInitAtkNmlMove(body_work, 56);
        AppMain.gmBoss1EffAfterburnerSetEnable(body_work, true);
        AppMain.GmSoundPlaySE("Boss1_02");
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateAtkNmlWithMove);
    }

    private static void gmBoss1BodyStateUpdateAtkNmlWithMove(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.gmBoss1BodySetDirectionNormal(body_work);
        if (AppMain.gmBoss1BodyIsExtraAttack(body_work))
        {
            if (AppMain.GmBsCmnIsFinalZoneType(AppMain.GMM_BS_OBJ((object)body_work)) == 0)
                AppMain.GmSoundChangeAngryBossBGM();
            AppMain.gmBoss1BodyChangeState(body_work, 5);
        }
        else
        {
            if (!AppMain.gmBoss1BodyUpdateAtkNmlMove(body_work))
                return;
            AppMain.gmBoss1BodyChangeState(body_work, 4);
        }
    }

    private static void gmBoss1BodyStateEnterAtkBash(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.gmBoss1BodySetActionWhole(body_work, 5);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        AppMain.gmBoss1BodySetDirectionNormal(body_work);
        AppMain.gmBoss1BodySetDmgRectSizeForAtkNml(body_work);
        if (AppMain.GmBsCmnGetPlayerObj().pos.x < obj_work.pos.x)
        {
            obj_work.disp_flag |= 1U;
            AppMain.gmBoss1BodyInitTurnGently(body_work, AppMain.AKM_DEGtoA16(270f), 30, false);
        }
        else
        {
            obj_work.disp_flag &= 4294967294U;
            AppMain.gmBoss1BodyInitTurnGently(body_work, AppMain.AKM_DEGtoA16(90f), 30, true);
        }
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateAtkBashWithLock);
    }

    private static void gmBoss1BodyStateLeaveAtkBash(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        body_work.flag &= 4294967294U;
        body_work.flag &= 4294967291U;
        AppMain.gmBoss1BodySetAtkRectToNormal(body_work);
        AppMain.gmBoss1BodySetDmgRectSizeToDefault(body_work);
    }

    private static void gmBoss1BodyStateUpdateAtkBashWithLock(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (!AppMain.gmBoss1BodyUpdateTurnGently(body_work))
            return;
        AppMain.gmBoss1BodySetActionWhole(body_work, 6);
        AppMain.gmBoss1Init1ShotTimer(body_work.se_timer, 110U);
        body_work.se_cnt = 3U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateAtkBashWithPrep);
    }

    private static void gmBoss1BodyStateUpdateAtkBashWithPrep(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.gmBoss1Update1ShotTimer(body_work.se_timer) && body_work.se_cnt != 0U)
        {
            --body_work.se_cnt;
            AppMain.GmSoundPlaySE("Boss1_03");
            AppMain.gmBoss1Init1ShotTimer(body_work.se_timer, 15U);
        }
        if (AppMain.gmBoss1BodyCheckChainMotionMergeEnd(body_work))
            body_work.flag |= 1U;
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        AppMain.gmBoss1BodySetDmgRectSizeToDefault(body_work);
        AppMain.gmBoss1BodySetAtkRectToWeakAttacker(body_work);
        body_work.flag |= 4U;
        body_work.flag |= 1U;
        AppMain.gmBoss1BodySetActionWhole(body_work, 7);
        if (((int)obj_work.disp_flag & 1) != 0)
            AppMain.gmBoss1BodyInitRush(body_work, true);
        else
            AppMain.gmBoss1BodyInitRush(body_work, false);
        AppMain.GmSoundPlaySE("Boss1_04");
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateAtkBashWithSwing);
    }

    private static void gmBoss1BodyStateUpdateAtkBashWithSwing(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (!AppMain.gmBoss1BodyUpdateRush(body_work) || AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        AppMain.gmBoss1BodySetDmgRectSizeToDefault(body_work);
        AppMain.gmBoss1BodySetAtkRectToWeakAttacker(body_work);
        body_work.flag &= 4294967291U;
        body_work.flag |= 134217728U;
        AppMain.GmCameraVibrationSet(0, 65536, 0);
        AppMain.GmSoundPlaySE("Boss1_05");
        AppMain.GMM_PAD_VIB_MID_TIME(30f);
        AppMain.gmBoss1BodySetActionWhole(body_work, 8);
        AppMain.gmBoss1BodySetSuspendAction(body_work, 1, 1U);
        AppMain.gmBoss1Init1ShotTimer(body_work.se_timer, 70U);
        body_work.se_cnt = 2U;
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateAtkBashWithFinish);
    }

    private static void gmBoss1BodyStateUpdateAtkBashWithFinish(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        if (AppMain.gmBoss1Update1ShotTimer(body_work.se_timer) && body_work.se_cnt != 0U)
        {
            --body_work.se_cnt;
            AppMain.GmSoundPlaySE("Boss1_03");
            AppMain.gmBoss1Init1ShotTimer(body_work.se_timer, 15U);
        }
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        AppMain.gmBoss1BodySetAtkRectToNormal(body_work);
        AppMain.gmBoss1BodySetActionWhole(body_work, 9);
        if (((int)obj_work.disp_flag & 1) != 0)
        {
            AppMain.gmBoss1BodyInitBashReturn(body_work, true);
            obj_work.disp_flag &= 4294967294U;
            AppMain.gmBoss1BodyInitTurnGently(body_work, AppMain.AKM_DEGtoA16(90), 90, false);
        }
        else
        {
            AppMain.gmBoss1BodyInitBashReturn(body_work, false);
            obj_work.disp_flag |= 1U;
            AppMain.gmBoss1BodyInitTurnGently(body_work, AppMain.AKM_DEGtoA16(270), 90, true);
        }
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateAtkBashWithHoming);
    }

    private static void gmBoss1BodyStateUpdateAtkBashWithHoming(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        bool flag = true;
        if (!AppMain.gmBoss1BodyUpdateTurnGently(body_work))
            flag = false;
        if (!AppMain.gmBoss1BodyUpdateBashReturn(body_work))
            flag = false;
        if (!flag)
            return;
        AppMain.gmBoss1BodySetDmgRectSizeToDefault(body_work);
        AppMain.gmBoss1BodySetAtkRectToWeakAttacker(body_work);
        body_work.flag |= 4U;
        body_work.flag |= 1U;
        AppMain.gmBoss1BodySetActionWhole(body_work, 7);
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            AppMain.gmBoss1BodyInitRush(body_work, true);
        else
            AppMain.gmBoss1BodyInitRush(body_work, false);
        AppMain.GmSoundPlaySE("Boss1_04");
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateAtkBashWithSwing);
    }

    private static void gmBoss1BodyStateEnterDmgNml(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss1BodyStateLeaveDmgNml(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss1BodyStateEnterDefeat(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)body_work);
        obj_work.flag |= 2U;
        obj_work.disp_flag |= 16U;
        body_work.flag |= 8U;
        body_work.ene_3d.ene_com.enemy_flag |= 32768U;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        body_work.wait_timer = 40U;
        AppMain.GmSoundChangeWinBossBGM();
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateDefeatWithWaitStart);
    }

    private static void gmBoss1BodyStateLeaveDefeat(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss1BodyStateUpdateDefeatWithWaitStart(
      AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss1EffBombInitCreate(body_work.bomb_work, 0, AppMain.GMM_BS_OBJ((object)body_work), AppMain.GMM_BS_OBJ((object)body_work).pos.x, AppMain.GMM_BS_OBJ((object)body_work).pos.y, 327680, 327680, 10U, 10U);
            body_work.wait_timer = 120U;
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateDefeatWithExplode);
        }
    }

    private static void gmBoss1BodyStateUpdateDefeatWithExplode(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
        {
            --body_work.wait_timer;
            AppMain.gmBoss1EffBombUpdateCreate(body_work.bomb_work);
        }
        else
        {
            body_work.flag |= 67108864U;
            AppMain.GmSoundPlaySE("Boss0_03");
            AppMain.gmBoss1InitFlashScreen();
            AppMain.GMM_PAD_VIB_MID_TIME(120f);
            AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate(AppMain.GMM_BS_OBJ((object)body_work), 8);
            obsObjectWork.pos.z = obsObjectWork.parent_obj.pos.z + 131072;
            body_work.wait_timer = 40U;
            AppMain.GmPlayerAddScoreNoDisp((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), 1000);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateDefeatWithScatter);
        }
    }

    private static void gmBoss1BodyStateUpdateDefeatWithScatter(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.wait_timer != 0U)
        {
            --body_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss1SetPartTextureBurnt(AppMain.GMM_BS_OBJ((object)body_work));
            body_work.flag |= 16777216U;
            AppMain.gmBoss1EffABSmokeInit(body_work);
            AppMain.gmBoss1EffBodySmokeInit(body_work);
            AppMain.gmBoss1EffBodySmallSmokeInit(body_work);
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateDefeatWithWaitEnd);
        }
    }

    private static void gmBoss1BodyStateUpdateDefeatWithWaitEnd(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (body_work.wait_timer > 0U)
            --body_work.wait_timer;
        else
            AppMain.gmBoss1BodyChangeState(body_work, 8);
    }

    private static void gmBoss1BodyStateEnterEscape(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)body_work);
        obsObjectWork.flag |= 2U;
        obsObjectWork.disp_flag &= 4294967279U;
        AppMain.gmBoss1BodySetActionWhole(body_work, 12);
        body_work.flag |= 8388608U;
        bool is_positive = AppMain.gmBoss1BodyIsDirectionPositiveFromCurrent(body_work, AppMain.GMD_BOSS1_RIGHTWARD_ANGLE);
        AppMain.gmBoss1BodyInitTurnGently(body_work, AppMain.GMD_BOSS1_RIGHTWARD_ANGLE, 90, is_positive);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateEscapeWithTurn);
    }

    private static void gmBoss1BodyStateLeaveEscape(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)body_work);
    }

    private static void gmBoss1BodyStateUpdateEscapeWithTurn(AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (!AppMain.gmBoss1BodyUpdateTurnGently(body_work))
            return;
        AppMain.gmBoss1BodyInitEscapeMove(body_work);
        if (AppMain.GmBsCmnIsFinalZoneType(AppMain.GMM_BS_OBJ((object)AppMain.GMM_BOSS1_MGR(body_work))) != 0)
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateEscapeWithMoveMoveFinalZone);
        else
            body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateEscapeWithMoveLocked);
    }

    private static void gmBoss1BodyStateUpdateEscapeWithMoveLocked(
      AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        AppMain.gmBoss1BodyUpdateEscapeMove(body_work);
        if (!AppMain.gmBoss1BodyIsEscapeScrUnlock(body_work))
            return;
        AppMain.GmMapSetMapDrawSize(1);
        AppMain.gmBoss1EffBodyDebrisInit(body_work);
        body_work.flag |= 256U;
        AppMain.GmGmkCamScrLimitRelease((byte)4);
        body_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK(AppMain.gmBoss1BodyStateUpdateEscapeWithMoveUnlocked);
    }

    private static void gmBoss1BodyStateUpdateEscapeWithMoveUnlocked(
      AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (!AppMain.gmBoss1BodyUpdateEscapeMove(body_work))
            return;
        AppMain.GMM_BOSS1_MGR(body_work).flag |= 2U;
        body_work.proc_update = (AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK)null;
    }

    private static void gmBoss1BodyStateUpdateEscapeWithMoveMoveFinalZone(
      AppMain.GMS_BOSS1_BODY_WORK body_work)
    {
        if (((int)body_work.flag & 256) == 0 && AppMain.gmBoss1BodyIsEscapeScrUnlock(body_work))
        {
            AppMain.gmBoss1EffBodyDebrisInit(body_work);
            body_work.flag |= 256U;
        }
        if (!AppMain.gmBoss1BodyUpdateEscapeMove(body_work) && !AppMain.gmBoss1BodyIsEscapeOutFinalZone(body_work))
            return;
        AppMain.GMM_BOSS1_MGR(body_work).flag |= 2U;
        body_work.proc_update = (AppMain.MPP_VOID_GMS_BOSS1_BODY_WORK)null;
    }

}