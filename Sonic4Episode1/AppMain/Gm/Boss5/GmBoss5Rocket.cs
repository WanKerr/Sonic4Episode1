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
    public static AppMain.OBS_OBJECT_WORK GmBoss5RocketInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_ROCKET_WORK()), "BOSS5_RKT");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS5_ROCKET_WORK rkt_work = (AppMain.GMS_BOSS5_ROCKET_WORK)work;
        AppMain.ObjObjectFieldRectSet(work, (short)-16, (short)-16, (short)16, (short)16);
        work.move_flag |= 256U;
        work.move_flag &= 4294443007U;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss5GetObject3dList()[3], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(747), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = AppMain.GMD_BOSS5_DEFAULT_BLEND_SPD;
        work.flag |= 16U;
        work.disp_flag |= 4194304U;
        work.move_flag |= 16U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        AppMain.gmBoss5RocketSetRectSize(rkt_work, 1);
        gmsEnemy3DWork.ene_com.rect_work[1].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss5RocketAtkPlyHitFunc);
        gmsEnemy3DWork.ene_com.rect_work[0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss5RocketDamageDefFunc);
        gmsEnemy3DWork.ene_com.rect_work[0].flag |= 2048U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag |= 2048U;
        AppMain.GmBsCmnSetAction(work, 0, 1, 0);
        AppMain.gmBoss5RocketInitCallbacks(rkt_work);
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5RocketMain);
        work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5RocketOutFunc);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss5RocketExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static AppMain.GMS_BOSS5_ROCKET_WORK GmBoss5RocketLaunchNormal(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int rkt_type)
    {
        AppMain.GMS_BOSS5_ROCKET_WORK rkt_work = AppMain.gmBoss5RocketCreate(body_work, rkt_type);
        AppMain.gmBoss5RocketNmlProcInit(rkt_work);
        return rkt_work;
    }

    public static AppMain.GMS_BOSS5_ROCKET_WORK GmBoss5RocketLaunchStrong(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int rkt_type)
    {
        AppMain.GMS_BOSS5_ROCKET_WORK rkt_work = AppMain.gmBoss5RocketCreate(body_work, rkt_type);
        AppMain.gmBoss5RocketStrProcInit(rkt_work);
        return rkt_work;
    }

    public static AppMain.GMS_BOSS5_ROCKET_WORK GmBoss5RocketSpawnConnected(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int rkt_type)
    {
        AppMain.GMS_BOSS5_ROCKET_WORK rkt_work = AppMain.gmBoss5RocketCreate(body_work, rkt_type);
        AppMain.gmBoss5RocketCnctProcInit(rkt_work);
        return rkt_work;
    }

    public static void gmBoss5RocketExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmBoss5RocketReleaseCallbacks((AppMain.GMS_BOSS5_ROCKET_WORK)AppMain.mtTaskGetTcbWork(tcb));
        AppMain.GmEnemyDefaultExit(tcb);
    }

    public static AppMain.GMS_BOSS5_ROCKET_WORK gmBoss5RocketCreate(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int rkt_type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork1 = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK obsObjectWork2 = AppMain.GmEventMgrLocalEventBirth((ushort)332, obsObjectWork1.pos.x, obsObjectWork1.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        AppMain.GMS_BOSS5_ROCKET_WORK gmsBosS5RocketWork = (AppMain.GMS_BOSS5_ROCKET_WORK)obsObjectWork2;
        obsObjectWork2.parent_obj = obsObjectWork1;
        gmsBosS5RocketWork.rkt_type = rkt_type;
        gmsBosS5RocketWork.arm_snm_id = 1 != rkt_type ? body_work.armpt_snm_reg_ids[0][2] : body_work.armpt_snm_reg_ids[1][2];
        obsObjectWork2.disp_flag |= 16777216U;
        if (((int)obsObjectWork1.disp_flag & 1) != 0)
        {
            AppMain.nnMakeRotateXMatrix(obsObjectWork2.obj_3d.user_obj_mtx_r, AppMain.AKM_DEGtoA32(180));
            obsObjectWork2.disp_flag |= 1U;
        }
        else
        {
            AppMain.nnMakeRotateXMatrix(obsObjectWork2.obj_3d.user_obj_mtx_r, AppMain.AKM_DEGtoA32(0));
            obsObjectWork2.disp_flag &= 4294967294U;
        }
        return (AppMain.GMS_BOSS5_ROCKET_WORK)obsObjectWork2;
    }

    public static void gmBoss5RocketSetAtkBodyRect(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_RECT_WORK pRec = ((AppMain.GMS_ENEMY_COM_WORK)rkt_work).rect_work[1];
        AppMain.ObjRectGroupSet(pRec, (byte)0, (byte)2);
        AppMain.ObjRectAtkSet(pRec, (ushort)2, (short)1);
        AppMain.ObjRectDefSet(pRec, ushort.MaxValue, (short)0);
        pRec.flag |= 4U;
        pRec.ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss5RocketAtkBossHitFunc);
        pRec.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
    }

    public static void gmBoss5RocketSetNoHitTime(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)rkt_work;
        rkt_work.no_hit_timer = 10U;
        gmsEnemyComWork.rect_work[0].flag |= 2048U;
        gmsEnemyComWork.rect_work[1].flag |= 2048U;
    }

    public static void gmBoss5RocketUpdateNoHitTime(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (rkt_work.no_hit_timer != 0U)
        {
            --rkt_work.no_hit_timer;
        }
        else
        {
            AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)rkt_work;
            gmsEnemyComWork.rect_work[0].flag &= 4294965247U;
            gmsEnemyComWork.rect_work[1].flag &= 4294965247U;
        }
    }

    public static void gmBoss5RocketUpdateMainRectPosition(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(rkt_work.snm_work, rkt_work.drill_snm_reg_id);
        int x = AppMain.FX_F32_TO_FX32(snmMtx.M03) - rkt_work.pivot_prev_pos.x;
        int y = -AppMain.FX_F32_TO_FX32(snmMtx.M13) - rkt_work.pivot_prev_pos.y;
        for (int index = 0; index < 3; ++index)
            AppMain.VEC_Set(ref rkt_work.ene_3d.ene_com.rect_work[index].rect.pos, x, y, 0);
    }

    public static void gmBoss5RocketSetAtkEnable(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work, int enable)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)rkt_work;
        if (enable != 0)
            gmsEnemyComWork.rect_work[1].flag &= 4294965247U;
        else
            gmsEnemyComWork.rect_work[1].flag |= 2048U;
    }

    public static void gmBoss5RocketSetDmgEnable(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work, int enable)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)rkt_work;
        if (enable != 0)
            gmsEnemyComWork.rect_work[0].flag &= 4294965247U;
        else
            gmsEnemyComWork.rect_work[0].flag |= 2048U;
    }

    public static void gmBoss5RocketSetRectSize(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      int sides_type)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)rkt_work;
        switch (sides_type)
        {
            case 1:
                AppMain.ObjRectWorkSet(gmsEnemyComWork.rect_work[1], (short)-20, (short)-20, (short)20, (short)20);
                AppMain.ObjRectWorkSet(gmsEnemyComWork.rect_work[0], (short)-10, (short)-10, (short)10, (short)10);
                break;
            default:
                AppMain.ObjRectWorkSet(gmsEnemyComWork.rect_work[0], (short)-20, (short)-20, (short)20, (short)20);
                AppMain.ObjRectWorkSet(gmsEnemyComWork.rect_work[1], (short)-10, (short)-10, (short)10, (short)10);
                break;
        }
    }

    public static void gmBoss5RocketInitPlySearch(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work, int delay)
    {
        rkt_work.ply_search_delay = delay;
        AppMain.GmBsCmnInitDelaySearch(rkt_work.dsearch_work, AppMain.GmBsCmnGetPlayerObj(), rkt_work.search_hist_buf, 21);
    }

    public static void gmBoss5RocketUpdatePlySearch(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.GmBsCmnUpdateDelaySearch(rkt_work.dsearch_work);
    }

    public static void gmBoss5RocketGetPlySearchPos(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      out AppMain.VecFx32 pos)
    {
        AppMain.GmBsCmnGetDelaySearchPos(rkt_work.dsearch_work, rkt_work.ply_search_delay, out pos);
    }

    public static int gmBoss5RocketSetPlyRebound(
      AppMain.OBS_RECT_WORK rkt_rect,
      AppMain.OBS_RECT_WORK ply_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = ply_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = rkt_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)parentObj1;
        AppMain.OBS_OBJECT_WORK parentObj3 = parentObj2.parent_obj;
        AppMain.GmPlySeqAtkReactionInit(ply_work);
        if (ply_work.seq_state == 20)
        {
            AppMain.GmPlySeqSetJumpState(ply_work, 0, 5U);
            uint num = AppMain.GmBsCmnCheckRectHitSideVFirst(rkt_rect, ply_rect);
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ((int)num & (int)AppMain.GMD_BS_CMN_RECT_HIT_SIDE_H_MASK) == 0 ? (ply_work.obj_work.move.x <= 0 ? (ply_work.obj_work.move.x >= 0 ? (ply_work.obj_work.pos.x >= parentObj3.pos.x ? (parentObj3.pos.x >= ply_work.obj_work.pos.x ? (((int)ply_work.obj_work.disp_flag & 1) == 0 ? -16384 : 16384) : -16384) : 16384) : 16384) : -16384) : (((int)num & (int)AppMain.GMD_BS_CMN_RECT_HIT_SIDE_LEFT) == 0 ? 16384 : -16384);
            ply_work.obj_work.spd.y = -16384;
            AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, 49152);
        }
        else
        {
            uint num = AppMain.GmBsCmnCheckRectHitSideVFirst(rkt_rect, ply_rect);
            ply_work.obj_work.spd_m = 0;
            ply_work.obj_work.spd.x = ((int)num & (int)AppMain.GMD_BS_CMN_RECT_HIT_SIDE_H_MASK) == 0 ? (ply_work.obj_work.move.x <= 0 ? (ply_work.obj_work.move.x >= 0 ? (ply_work.obj_work.pos.x >= parentObj3.pos.x ? (parentObj3.pos.x >= ply_work.obj_work.pos.x ? (((int)ply_work.obj_work.disp_flag & 1) == 0 ? -12288 : 12288) : -12288) : 12288) : 12288) : -12288) : (((int)num & (int)AppMain.GMD_BS_CMN_RECT_HIT_SIDE_LEFT) == 0 ? 12288 : -12288);
            ply_work.obj_work.spd.y = -16384;
            AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, 32768);
            ply_work.homing_timer = 40960;
        }
        return 1;
    }

    public static void gmBoss5RocketUpdateRocketStuckWithArm(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      int b_rotation)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        AppMain.NNS_MATRIX ofst_mtx;
        if (((int)parentObj.flag & 16) != 0)
        {
            int index = rkt_work.rkt_type != 1 ? 0 : 1;
            ofst_mtx = parentObj.rkt_ofst_mtx[index];
        }
        else
            ofst_mtx = (AppMain.NNS_MATRIX)null;
        if (parentObj.adj_hgap_is_active != 0)
            AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, rkt_work.arm_snm_id, b_rotation, ofst_mtx);
        else
            AppMain.GmBsCmnUpdateObject3DNNStuckWithNodeRelative(obj_work, parentObj.snm_work, rkt_work.arm_snm_id, b_rotation, obj_work.parent_obj.pos, parentObj.pivot_prev_pos, ofst_mtx);
        if (b_rotation == 0 || rkt_work.rkt_type != 1)
            return;
        obj_work.disp_flag |= 16777216U;
        AppMain.nnRotateYMatrix(obj_work.obj_3d.user_obj_mtx_r, obj_work.obj_3d.user_obj_mtx_r, AppMain.AKM_DEGtoA32(180));
    }

    public static void gmBoss5RocketInitRocketStuckWithArmLerpRot(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      float ratio_spd)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(rkt_work.snm_work, rkt_work.drill_snm_reg_id);
        AppMain.AkMathNormalizeMtx(nnsMatrix, snmMtx);
        AppMain.nnMakeRotateMatrixQuaternion(out rkt_work.stuck_lerp_src_quat, nnsMatrix);
        AppMain.gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 0);
        obsObjectWork.dir.x = obsObjectWork.dir.y = obsObjectWork.dir.z = (ushort)0;
        rkt_work.stuck_lerp_ratio = 0.0f;
        rkt_work.stuck_lerp_ratio_spd = ratio_spd;
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    public static int gmBoss5RocketUpdateRocketStuckWithArmLerpRot(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj;
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        int num = 0;
        rkt_work.stuck_lerp_ratio += rkt_work.stuck_lerp_ratio_spd;
        if ((double)rkt_work.stuck_lerp_ratio >= 1.0)
        {
            rkt_work.stuck_lerp_ratio = 1f;
            num = 1;
        }
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(parentObj.snm_work, rkt_work.arm_snm_id);
        AppMain.AkMathNormalizeMtx(nnsMatrix, snmMtx);
        AppMain.NNS_QUATERNION dst1;
        AppMain.nnMakeRotateMatrixQuaternion(out dst1, nnsMatrix);
        if (rkt_work.rkt_type == 1)
        {
            AppMain.NNS_QUATERNION dst2;
            AppMain.nnMakeRotateXYZQuaternion(out dst2, 0, AppMain.AKM_DEGtoA32(180), 0);
            AppMain.nnMultiplyQuaternion(ref dst1, ref dst1, ref dst2);
        }
        AppMain.NNS_QUATERNION dst3;
        AppMain.nnSlerpQuaternion(out dst3, ref rkt_work.stuck_lerp_src_quat, ref dst1, rkt_work.stuck_lerp_ratio);
        AppMain.gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 0);
        obsObjectWork.disp_flag |= 16777216U;
        AppMain.nnQuaternionMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, obsObjectWork.obj_3d.user_obj_mtx_r, ref dst3);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
        return num != 0 ? 1 : 0;
    }

    public static void gmBoss5RocketGetArmNodePosFx(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      out AppMain.VecFx32 pos_out)
    {
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(((AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)rkt_work).parent_obj).snm_work, rkt_work.arm_snm_id);
        pos_out.x = AppMain.FX_F32_TO_FX32(snmMtx.M03);
        pos_out.y = -AppMain.FX_F32_TO_FX32(snmMtx.M13);
        pos_out.z = AppMain.FX_F32_TO_FX32(snmMtx.M23);
    }

    public static void gmBoss5RocketSetDispOfst(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      float disp_ofst,
      int b_pos_slide)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        obsObjectWork.disp_flag |= 16777216U;
        AppMain.nnTranslateMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, obsObjectWork.obj_3d.user_obj_mtx_r, disp_ofst, 0.0f, 0.0f);
        if (b_pos_slide == 0)
            return;
        obsObjectWork.pos.x -= AppMain.FX_Mul(AppMain.FX_F32_TO_FX32(AppMain.nnCos((int)obsObjectWork.dir.z) * disp_ofst), AppMain.g_obj.draw_scale.x);
        obsObjectWork.pos.y -= AppMain.FX_Mul(AppMain.FX_F32_TO_FX32(AppMain.nnSin((int)obsObjectWork.dir.z) * disp_ofst), AppMain.g_obj.draw_scale.y);
    }

    public static void gmBoss5RocketGetDispOfst(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      ref AppMain.VecFx32 ofst_pos_out)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.amVectorSet(nnsVector, 0.0f, 0.0f, 0.0f);
        AppMain.nnMakeRotateXYZMatrix(nnsMatrix, (int)((long)ushort.MaxValue & (long)-obsObjectWork.dir.x), (int)ushort.MaxValue & (int)obsObjectWork.dir.y, (int)((long)ushort.MaxValue & (long)-obsObjectWork.dir.z));
        AppMain.nnScaleMatrix(nnsMatrix, nnsMatrix, AppMain.FX_FX32_TO_F32(AppMain.g_obj.draw_scale.x), AppMain.FX_FX32_TO_F32(AppMain.g_obj.draw_scale.y), AppMain.FX_FX32_TO_F32(AppMain.g_obj.draw_scale.z));
        AppMain.nnMultiplyMatrix(nnsMatrix, nnsMatrix, obsObjectWork.obj_3d.user_obj_mtx_r);
        AppMain.nnTransformVector(nnsVector, nnsMatrix, nnsVector);
        AppMain.VEC_Set(ref ofst_pos_out, AppMain.FX_F32_TO_FX32(nnsVector.x), AppMain.FX_F32_TO_FX32(-nnsVector.y), AppMain.FX_F32_TO_FX32(nnsVector.z));
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    public static int gmBoss5RocketInitFlyDestPos(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      int init_acc,
      int init_spd,
      int max_spd,
      AppMain.VecFx32 launch_pos,
      ref AppMain.VecFx32 dest_pos)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        rkt_work.acc = init_acc;
        rkt_work.max_spd = max_spd;
        int num = AppMain.nnArcTan2((double)AppMain.FX_FX32_TO_F32(dest_pos.y - launch_pos.y), (double)AppMain.FX_FX32_TO_F32(dest_pos.x - launch_pos.x));
        rkt_work.move_dir = num;
        int fx32_1 = AppMain.FX_F32_TO_FX32(AppMain.nnCos(rkt_work.move_dir));
        int fx32_2 = AppMain.FX_F32_TO_FX32(AppMain.nnSin(rkt_work.move_dir));
        obsObjectWork.spd_add.x = AppMain.FX_Mul(rkt_work.acc, fx32_1);
        obsObjectWork.spd_add.y = AppMain.FX_Mul(rkt_work.acc, fx32_2);
        obsObjectWork.spd_add.z = 0;
        obsObjectWork.spd.x = AppMain.FX_Mul(init_spd, fx32_1);
        obsObjectWork.spd.y = AppMain.FX_Mul(init_spd, fx32_2);
        rkt_work.launch_pos = launch_pos;
        rkt_work.dest_pos = dest_pos;
        return num;
    }

    public static void gmBoss5RocketInitFlyDestDistance(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      int init_acc,
      int init_spd,
      int max_spd,
      ref AppMain.VecFx32 launch_pos,
      int angle,
      int distance)
    {
        AppMain.VecFx32 dest_pos;
        dest_pos.x = launch_pos.x + AppMain.FX_Mul(distance, AppMain.FX_F32_TO_FX32(AppMain.nnCos(angle)));
        dest_pos.y = launch_pos.y + AppMain.FX_Mul(distance, AppMain.FX_F32_TO_FX32(AppMain.nnSin(angle)));
        dest_pos.z = 0;
        AppMain.gmBoss5RocketInitFlyDestPos(rkt_work, init_acc, init_spd, max_spd, launch_pos, ref dest_pos);
    }

    public static void gmBoss5RocketRedirectFlyDestPos(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      int init_acc,
      ref AppMain.VecFx32 dest_pos)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.VecFx32 spd = obsObjectWork.spd;
        AppMain.gmBoss5RocketInitFlyDestPos(rkt_work, init_acc, 4096, rkt_work.max_spd, obsObjectWork.pos, ref dest_pos);
        obsObjectWork.spd.Assign(spd);
    }

    public static int gmBoss5RocketUpdateFlyDest(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        return AppMain.gmBoss5RocketUpdateFlyDest(rkt_work, 0);
    }

    public static int gmBoss5RocketUpdateFlyDest(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      int b_mdl_center)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.NNS_VECTOR nnsVector1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.VecFx32 ofst_pos_out = new AppMain.VecFx32();
        if (AppMain.FX_Sqrt(AppMain.FX_Mul(obsObjectWork.spd.x, obsObjectWork.spd.x) + AppMain.FX_Mul(obsObjectWork.spd.y, obsObjectWork.spd.y)) >= rkt_work.max_spd)
            obsObjectWork.spd_add.x = obsObjectWork.spd_add.y = obsObjectWork.spd_add.z = 0;
        AppMain.amVectorSet(nnsVector1, AppMain.FX_FX32_TO_F32(obsObjectWork.spd.x), AppMain.FX_FX32_TO_F32(obsObjectWork.spd.y), 0.0f);
        if (b_mdl_center != 0)
            AppMain.gmBoss5RocketGetDispOfst(rkt_work, ref ofst_pos_out);
        AppMain.amVectorSet(nnsVector2, AppMain.FX_FX32_TO_F32(rkt_work.dest_pos.x - (obsObjectWork.pos.x + ofst_pos_out.x)), AppMain.FX_FX32_TO_F32(rkt_work.dest_pos.y - (obsObjectWork.pos.y + ofst_pos_out.y)), 0.0f);
        int num = (double)AppMain.nnDotProductVector(nnsVector1, nnsVector2) <= 0.0 ? 1 : 0;
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector2);
        return num;
    }

    public static void gmBoss5RocketInitFlyReverse(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      int acc_scalar)
    {
        AppMain.gmBoss5RocketInitFlyReverse(rkt_work, acc_scalar, 0);
    }

    public static void gmBoss5RocketInitFlyReverse(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      int acc_scalar,
      int is_add)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.amVectorSet(nnsVector, -AppMain.FX_FX32_TO_F32(obsObjectWork.spd.x), -AppMain.FX_FX32_TO_F32(obsObjectWork.spd.y), 0.0f);
        AppMain.nnNormalizeVector(nnsVector, nnsVector);
        AppMain.VecFx32 acc_vec = new AppMain.VecFx32(AppMain.FX_Mul(AppMain.FX_F32_TO_FX32(nnsVector.x), acc_scalar), AppMain.FX_Mul(AppMain.FX_F32_TO_FX32(nnsVector.y), acc_scalar), 0);
        AppMain.gmBoss5RocketInitFlyReverseVec(rkt_work, ref acc_vec, is_add);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector);
    }

    public static void gmBoss5RocketInitFlyReverseVec(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      ref AppMain.VecFx32 acc_vec)
    {
        AppMain.gmBoss5RocketInitFlyReverseVec(rkt_work, ref acc_vec, 0);
    }

    public static void gmBoss5RocketInitFlyReverseVec(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      ref AppMain.VecFx32 acc_vec,
      int is_add)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        rkt_work.rvs_acc.x = acc_vec.x;
        rkt_work.rvs_acc.y = acc_vec.y;
        rkt_work.rvs_acc.z = 0;
        if (is_add != 0)
        {
            obsObjectWork.spd_add.x += rkt_work.rvs_acc.x;
            obsObjectWork.spd_add.y += rkt_work.rvs_acc.y;
            obsObjectWork.spd_add.z = 0;
        }
        else
            obsObjectWork.spd_add.Assign(rkt_work.rvs_acc);
    }

    public static int gmBoss5RocketUpdateFlyReverse(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.NNS_VECTOR nnsVector1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.amVectorSet(nnsVector1, AppMain.FX_FX32_TO_F32(rkt_work.rvs_acc.x), AppMain.FX_FX32_TO_F32(rkt_work.rvs_acc.y), 0.0f);
        AppMain.amVectorSet(nnsVector2, AppMain.FX_FX32_TO_F32(obsObjectWork.spd.x), AppMain.FX_FX32_TO_F32(obsObjectWork.spd.y), 0.0f);
        int num = (double)AppMain.nnDotProductVector(nnsVector1, nnsVector2) >= 0.0 ? 1 : 0;
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector2);
        return num;
    }

    public static void gmBoss5RocketSetInitialDir(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        obsObjectWork.dir.z = ((int)obsObjectWork.parent_obj.disp_flag & 1) == 0 ? (ushort)AppMain.GMD_BOSS5_RKT_SEARCH_INITIAL_DIR_Z_R : (ushort)AppMain.GMD_BOSS5_RKT_SEARCH_INITIAL_DIR_Z_L;
        if (rkt_work.rkt_type == 1)
            obsObjectWork.dir.x = (ushort)((ulong)ushort.MaxValue & (ulong)((int)obsObjectWork.dir.x + AppMain.GMD_BOSS5_RKT_SEARCH_INITIAL_ADJ_DIR_X_RA));
        else
            obsObjectWork.dir.x = (ushort)((ulong)ushort.MaxValue & (ulong)((int)obsObjectWork.dir.x + AppMain.GMD_BOSS5_RKT_SEARCH_INITIAL_ADJ_DIR_X_LA));
    }

    public static void gmBoss5RocketUpdateDirFollowingPos(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      ref AppMain.VecFx32 targ_pos,
      float deg,
      int is_reverse)
    {
        AppMain.gmBoss5RocketUpdateDirFollowingPos(rkt_work, targ_pos, deg, is_reverse, 0);
    }

    public static void gmBoss5RocketUpdateDirFollowingPos(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      AppMain.VecFx32 targ_pos,
      float deg,
      int is_reverse,
      int force_rot_spd)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        if (is_reverse != 0)
            obsObjectWork.dir.z -= (ushort)AppMain.AKM_DEGtoA32(180);
        int a1 = (int)((long)ushort.MaxValue & (long)((int)((long)ushort.MaxValue & (long)AppMain.nnArcTan2((double)AppMain.FX_FX32_TO_F32(targ_pos.y - obsObjectWork.pos.y), (double)AppMain.FX_FX32_TO_F32(targ_pos.x - obsObjectWork.pos.x))) - (int)obsObjectWork.dir.z));
        int a2;
        if (a1 >= AppMain.AKM_DEGtoA32(180))
        {
            a1 = -(AppMain.AKM_DEGtoA32(360) - a1);
            a2 = AppMain.AKM_DEGtoA32(-deg);
        }
        else
            a2 = AppMain.AKM_DEGtoA32(deg);
        if (AppMain.MTM_MATH_ABS(a1) <= AppMain.MTM_MATH_ABS(a2))
            a2 = a1;
        if (is_reverse != 0)
            a2 += AppMain.AKM_DEGtoA32(180);
        if (force_rot_spd != 0)
            a2 = force_rot_spd;
        obsObjectWork.dir.z = (ushort)(short)((long)ushort.MaxValue & (long)((int)obsObjectWork.dir.z + a2));
    }

    public static void gmBoss5RocketUpdateDirPlyLockOn(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      ref AppMain.VecFx32 lock_pos)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.OBS_OBJECT_WORK parentObj = obsObjectWork.parent_obj;
        AppMain.OBS_OBJECT_WORK playerObj = AppMain.GmBsCmnGetPlayerObj();
        if (((int)parentObj.disp_flag & 1) != 0)
        {
            if (playerObj.pos.x >= obsObjectWork.pos.x)
                AppMain.gmBoss5RocketUpdateDirFollowingPos(rkt_work, lock_pos, 1f, 0, AppMain.AKM_DEGtoA32(1f));
            else
                AppMain.gmBoss5RocketUpdateDirFollowingPos(rkt_work, ref lock_pos, 1f, 0);
            AppMain.gmBoss5RocketLimitDir(rkt_work, AppMain.GMD_BOSS5_RKT_LOCKON_DIR_LIMIT_L_START, AppMain.GMD_BOSS5_RKT_LOCKON_DIR_LIMIT_L_END);
        }
        else
        {
            if (playerObj.pos.x <= obsObjectWork.pos.x)
                AppMain.gmBoss5RocketUpdateDirFollowingPos(rkt_work, lock_pos, 1f, 0, AppMain.AKM_DEGtoA32(-1f));
            else
                AppMain.gmBoss5RocketUpdateDirFollowingPos(rkt_work, ref lock_pos, 1f, 0);
            AppMain.gmBoss5RocketLimitDir(rkt_work, AppMain.GMD_BOSS5_RKT_LOCKON_DIR_LIMIT_R_START, AppMain.GMD_BOSS5_RKT_LOCKON_DIR_LIMIT_R_END);
        }
    }

    public static void gmBoss5RocketUpdateDirFollowingAccSpd(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      float deg,
      int is_reverse)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        if (is_reverse != 0)
            obsObjectWork.dir.z -= (ushort)AppMain.AKM_DEGtoA32(180);
        int num1 = (int)((long)ushort.MaxValue & (long)(AppMain.nnArcTan2((double)AppMain.FX_FX32_TO_F32(obsObjectWork.spd.y), (double)AppMain.FX_FX32_TO_F32(obsObjectWork.spd.x)) - (int)obsObjectWork.dir.z));
        if (num1 >= AppMain.AKM_DEGtoA32(180))
            num1 = -(AppMain.AKM_DEGtoA32(360) - num1);
        int num2;
        if (obsObjectWork.spd_add.x != 0 || obsObjectWork.spd_add.y != 0)
        {
            num2 = (int)((long)ushort.MaxValue & (long)(AppMain.nnArcTan2((double)AppMain.FX_FX32_TO_F32(obsObjectWork.spd_add.y), (double)AppMain.FX_FX32_TO_F32(obsObjectWork.spd_add.x)) - (int)obsObjectWork.dir.z));
            if (num2 >= AppMain.AKM_DEGtoA32(180))
                num2 = -(AppMain.AKM_DEGtoA32(360) - num2);
        }
        else
            num2 = num1;
        int a1 = (int)((long)ushort.MaxValue & (long)((num2 + num1) / 2));
        int a2;
        if (a1 >= AppMain.AKM_DEGtoA32(180))
        {
            a1 = -(AppMain.AKM_DEGtoA32(360) - a1);
            a2 = AppMain.AKM_DEGtoA32(-deg);
        }
        else
            a2 = AppMain.AKM_DEGtoA32(deg);
        if (AppMain.MTM_MATH_ABS(a1) < AppMain.MTM_MATH_ABS(a2))
            a2 = a1;
        if (is_reverse != 0)
            a2 += AppMain.AKM_DEGtoA32(180);
        obsObjectWork.dir.z = (ushort)(short)((long)ushort.MaxValue & (long)((int)obsObjectWork.dir.z + a2));
    }

    public static void gmBoss5RocketLimitDir(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work,
      int start_angle,
      int end_angle)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        int num1 = (int)((long)ushort.MaxValue & (long)((int)((long)ushort.MaxValue & (long)obsObjectWork.dir.z) - start_angle));
        int num2 = (int)((long)ushort.MaxValue & (long)(end_angle - start_angle));
        if (num1 <= num2)
            return;
        int num3 = num2 / 2 + AppMain.AKM_DEGtoA32(180);
        if (num1 >= num3)
            obsObjectWork.dir.z = (ushort)(short)((long)ushort.MaxValue & (long)start_angle);
        else
            obsObjectWork.dir.z = (ushort)(short)((long)ushort.MaxValue & (long)end_angle);
    }

    public static void gmBoss5RocketInitDirFalling(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        rkt_work.pivot_fall_angle = AppMain.AKM_DEGtoA32(90);
        rkt_work.wobble_sin_param_angle = AppMain.AKM_DEGtoA32(0);
        obsObjectWork.dir.z = (ushort)((ulong)ushort.MaxValue & (ulong)rkt_work.pivot_fall_angle);
    }

    public static void gmBoss5RocketUpdateDirFalling(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        obsObjectWork.dir.x = (ushort)((ulong)ushort.MaxValue & (ulong)((int)obsObjectWork.dir.x + AppMain.GMD_BOSS5_RKT_FALL_SPIN_ANGLE_SPD));
        rkt_work.wobble_sin_param_angle += AppMain.GMD_BOSS5_RKT_FALL_WOBBLE_SIN_PARAM_DEG_SPD;
        rkt_work.wobble_sin_param_angle = (int)((long)ushort.MaxValue & (long)rkt_work.wobble_sin_param_angle);
        obsObjectWork.dir.z = (ushort)((ulong)ushort.MaxValue & (ulong)(rkt_work.pivot_fall_angle + AppMain.AKM_DEGtoA32(15f * AppMain.nnSin(rkt_work.wobble_sin_param_angle))));
    }

    public static void gmBoss5RocketEndDirFalling(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        rkt_work.pivot_fall_angle = 0;
        rkt_work.wobble_sin_param_angle = 0;
        obsObjectWork.dir.z = (ushort)AppMain.AKM_DEGtoA32(90);
    }

    public static void gmBoss5RocketInitLeakageFlicker(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.wfall_atk_toggle_timer = 0U;
        rkt_work.flag &= 4294967287U;
    }

    public static int gmBoss5RocketUpdateLeakageFlicker(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (rkt_work.wfall_atk_toggle_timer != 0U)
            --rkt_work.wfall_atk_toggle_timer;
        else
            rkt_work.wfall_atk_toggle_timer = 39U;
        if (rkt_work.wfall_atk_toggle_timer < 20U)
        {
            rkt_work.flag &= 4294967287U;
            return 0;
        }
        rkt_work.flag |= 8U;
        return 1;
    }

    public static void gmBoss5RocketClearLeakageFlicker(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.wfall_atk_toggle_timer = 0U;
        rkt_work.flag &= 4294967287U;
    }

    public static void gmBoss5RocketInitFlyBlow(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj;
        obsObjectWork.move_flag |= 16U;
        obsObjectWork.move_flag &= 4294967166U;
        rkt_work.launch_pos.Assign(obsObjectWork.pos);
        rkt_work.dest_pos.Assign(parentObj.part_obj_core.pos);
        int ang = AppMain.nnArcTan2((double)AppMain.FX_FX32_TO_F32(rkt_work.dest_pos.y - obsObjectWork.pos.y), (double)AppMain.FX_FX32_TO_F32(rkt_work.dest_pos.x - obsObjectWork.pos.x));
        int fx32_1 = AppMain.FX_F32_TO_FX32(AppMain.nnCos(ang));
        int fx32_2 = AppMain.FX_F32_TO_FX32(AppMain.nnSin(ang));
        obsObjectWork.spd.x = AppMain.FX_Mul(49152, fx32_1);
        obsObjectWork.spd.y = AppMain.FX_Mul(49152, fx32_2);
        obsObjectWork.spd.z = 0;
        if (obsObjectWork.spd.x < 0)
            rkt_work.rot_spd = -AppMain.GMD_BOSS5_RKT_BLOW_FLY_ROT_SPD;
        else
            rkt_work.rot_spd = AppMain.GMD_BOSS5_RKT_BLOW_FLY_ROT_SPD;
    }

    public static int gmBoss5RocketUpdateFlyBlow(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.NNS_VECTOR nnsVector1 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        AppMain.NNS_VECTOR nnsVector2 = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        obsObjectWork.dir.z = (ushort)(short)((long)ushort.MaxValue & (long)((int)obsObjectWork.dir.z + rkt_work.rot_spd));
        AppMain.amVectorSet(nnsVector1, AppMain.FX_FX32_TO_F32(rkt_work.dest_pos.x - rkt_work.launch_pos.x), AppMain.FX_FX32_TO_F32(rkt_work.dest_pos.y - rkt_work.launch_pos.y), 0.0f);
        AppMain.amVectorSet(nnsVector2, AppMain.FX_FX32_TO_F32(rkt_work.dest_pos.x - obsObjectWork.pos.x), AppMain.FX_FX32_TO_F32(rkt_work.dest_pos.y - obsObjectWork.pos.y), 0.0f);
        int num = (double)AppMain.nnDotProductVector(nnsVector1, nnsVector2) <= 0.0 ? 1 : 0;
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector1);
        AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(nnsVector2);
        return num;
    }

    public static void gmBoss5RocketInitFlyBounce(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        obsObjectWork.move_flag |= 16U;
        obsObjectWork.move_flag &= 4294967166U;
        int fx32_1 = AppMain.FX_F32_TO_FX32(AppMain.nnCos(AppMain.GMD_BOSS5_RKT_BOUNCE_DIR_ANGLE));
        int fx32_2 = AppMain.FX_F32_TO_FX32(AppMain.nnSin(AppMain.GMD_BOSS5_RKT_BOUNCE_DIR_ANGLE));
        int num = 0;
        if (obsObjectWork.spd.x < 0)
            num = 1;
        obsObjectWork.spd.x = AppMain.FX_Mul(16384, fx32_1);
        obsObjectWork.spd.y = AppMain.FX_Mul(16384, fx32_2);
        obsObjectWork.spd.z = 0;
        if (num != 0)
            obsObjectWork.spd.x = -obsObjectWork.spd.x;
        if (obsObjectWork.spd.x < 0)
            rkt_work.rot_spd = -AppMain.GMD_BOSS5_RKT_BOUNCE_FLY_ROT_SPD;
        else
            rkt_work.rot_spd = AppMain.GMD_BOSS5_RKT_BOUNCE_FLY_ROT_SPD;
    }

    public static int gmBoss5RocketUpdateFlyBounce(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        obsObjectWork.dir.z = (ushort)(short)((long)ushort.MaxValue & (long)((int)obsObjectWork.dir.z + rkt_work.rot_spd));
        return obsObjectWork.pos.y <= AppMain.GMM_BOSS5_AREA_TOP() - 196608 ? 1 : 0;
    }

    public static void gmBoss5RocketInitStuckLean(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        rkt_work.stuck_dir = (int)obsObjectWork.dir.z;
        rkt_work.stuck_lean_ratio = 0.0f;
    }

    public static int gmBoss5RocketUpdateStuckLean(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK pObj = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.OBS_OBJECT_WORK parentObj = pObj.parent_obj;
        int num1 = 1;
        int num2 = 0;
        rkt_work.hit_vib_amp_deg -= 0.5f;
        if ((double)rkt_work.hit_vib_amp_deg <= 0.0)
            rkt_work.hit_vib_amp_deg = 0.0f;
        rkt_work.hit_vib_sin_angle = (int)((long)ushort.MaxValue & (long)(rkt_work.hit_vib_sin_angle + AppMain.GMD_BOSS5_RKT_GRD_STUCK_LEAN_HIT_VIB_SIN_ANGLE_ADD));
        float num3 = AppMain.nnSin(rkt_work.hit_vib_sin_angle) * rkt_work.hit_vib_amp_deg;
        float n;
        if (((int)rkt_work.flag & 128) != 0)
        {
            float stuckLeanRatio = rkt_work.stuck_lean_ratio;
            if ((double)rkt_work.stuck_lean_ratio < 1.0)
            {
                rkt_work.stuck_lean_ratio += 0.075f;
                num1 = 0;
            }
            rkt_work.stuck_lean_ratio = AppMain.MTM_MATH_CLIP(rkt_work.stuck_lean_ratio, 0.0f, 1f);
            n = 180f * rkt_work.stuck_lean_ratio - num3;
            num2 = (int)(131072.0 * ((double)rkt_work.stuck_lean_ratio - (double)stuckLeanRatio));
            AppMain.ObjObjectFieldRectSet(pObj, (short)-16, (short)-16, (short)16, (short)(16 + (int)(short)((double)rkt_work.stuck_lean_ratio * 8.0)));
        }
        else
            n = (float)(int)rkt_work.hit_count * -10f + num3;
        if (pObj.pos.x < parentObj.pos.x)
        {
            n = -n;
            num2 = -num2;
        }
        pObj.dir.z = (ushort)((ulong)ushort.MaxValue & (ulong)(rkt_work.stuck_dir + AppMain.AKM_DEGtoA32(n)));
        pObj.pos.x += num2;
        return num1;
    }

    public static void gmBoss5RocketSetStuckLeanHitVib(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        rkt_work.hit_vib_amp_deg = 10f;
        rkt_work.hit_vib_sin_angle = AppMain.AKM_DEGtoA32(0);
    }

    public static void gmBoss5RocketInitScatter(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.GmBoss5ScatterSetFlyParam(obj_work);
        AppMain.AkMathNormalizeMtx(nnsMatrix, obj_work.obj_3d.user_obj_mtx_r);
        AppMain.nnMakeRotateMatrixQuaternion(out rkt_work.sct_cur_quat, nnsMatrix);
        AppMain.nnMakeUnitQuaternion(ref rkt_work.sct_spin_quat);
        for (int index = 0; index < 2; ++index)
        {
            AppMain.NNS_VECTOR dst_vec = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            float rand_z = AppMain.MTM_MATH_CLIP((float)((double)AppMain.FX_FX32_TO_F32(AppMain.AkMathRandFx()) * 2.0 - 1.0), -1f, 1f);
            short rand_angle = AppMain.AKM_DEGtoA16(360f * AppMain.FX_FX32_TO_F32(AppMain.AkMathRandFx()));
            AppMain.AkMathGetRandomUnitVector(dst_vec, rand_z, rand_angle);
            AppMain.NNS_QUATERNION dst;
            AppMain.nnMakeRotateAxisQuaternion(out dst, dst_vec.x, dst_vec.y, dst_vec.z, AppMain.GMD_BOSS5_SCT_SPIN_SPD_ANGLE);
            AppMain.nnMultiplyQuaternion(ref rkt_work.sct_spin_quat, ref dst, ref rkt_work.sct_spin_quat);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(dst_vec);
        }
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    public static void gmBoss5RocketUpdateScatter(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.nnMultiplyQuaternion(ref rkt_work.sct_cur_quat, ref rkt_work.sct_spin_quat, ref rkt_work.sct_cur_quat);
        AppMain.nnMakeQuaternionMatrix(obsObjectWork.obj_3d.user_obj_mtx_r, ref rkt_work.sct_cur_quat);
        obsObjectWork.disp_flag |= 16777216U;
    }

    public static int gmBoss5RocketReceiveSignalLaunch(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)rkt_work).parent_obj;
        if (rkt_work.rkt_type == 0 && ((int)parentObj.flag & 268435456) != 0)
        {
            parentObj.flag &= 4026531839U;
            return 1;
        }
        if (rkt_work.rkt_type != 1 || ((int)parentObj.flag & 134217728) == 0)
            return 0;
        parentObj.flag &= 4160749567U;
        return 1;
    }

    public static int gmBoss5RocketReceiveSignalReturn(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)rkt_work).parent_obj;
        if (rkt_work.rkt_type == 0 && ((int)parentObj.flag & 1073741824) != 0)
        {
            parentObj.flag &= 3221225471U;
            return 1;
        }
        if (rkt_work.rkt_type != 1 || ((int)parentObj.flag & 536870912) == 0)
            return 0;
        parentObj.flag &= 3758096383U;
        return 1;
    }

    public static void gmBoss5RocketDispatchSignalReturned(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)rkt_work).parent_obj;
        if (rkt_work.rkt_type == 0)
        {
            parentObj.flag |= 67108864U;
        }
        else
        {
            if (rkt_work.rkt_type != 1)
                return;
            parentObj.flag |= 33554432U;
        }
    }

    public static void gmBoss5RocketInitCallbacks(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GmBsCmnInitBossMotionCBSystem(obj_work, rkt_work.bmcb_mgr);
        AppMain.GmBsCmnCreateSNMWork(rkt_work.snm_work, obj_work.obj_3d._object, (ushort)1);
        AppMain.GmBsCmnAppendBossMotionCallback(rkt_work.bmcb_mgr, rkt_work.snm_work.bmcb_link);
        rkt_work.drill_snm_reg_id = AppMain.GmBsCmnRegisterSNMNode(rkt_work.snm_work, 3);
    }

    public static void gmBoss5RocketReleaseCallbacks(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.GmBsCmnClearBossMotionCBSystem(AppMain.GMM_BS_OBJ((object)rkt_work));
        AppMain.GmBsCmnDeleteSNMWork(rkt_work.snm_work);
    }

    public static void gmBoss5RocketAtkPlyHitFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        ((AppMain.GMS_BOSS5_ROCKET_WORK)my_rect.parent_obj).flag |= 1U;
        AppMain.GmEnemyDefaultAtkFunc(my_rect, your_rect);
    }

    public static void gmBoss5RocketAtkBossHitFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        AppMain.GMS_BOSS5_ROCKET_WORK rkt_work = (AppMain.GMS_BOSS5_ROCKET_WORK)parentObj1;
        if (((AppMain.GMS_ENEMY_COM_WORK)parentObj2).eve_rec.id == (ushort)330)
        {
            parentObj1.flag |= 2U;
            AppMain.gmBoss5RocketBounceProcInit(rkt_work);
        }
        else
            AppMain.ObjRectFuncNoHit(my_rect, your_rect);
    }

    public static void gmBoss5RocketDamageDefFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        AppMain.GMS_BOSS5_ROCKET_WORK parentObj1 = (AppMain.GMS_BOSS5_ROCKET_WORK)my_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        if (parentObj2 == null || (ushort)1 != parentObj2.obj_type || ((int)((AppMain.GMS_PLAYER_WORK)parentObj2).obj_work.move_flag & 1) != 0)
            return;
        int num = AppMain.gmBoss5RocketSetPlyRebound(my_rect, your_rect);
        if (((int)parentObj1.flag & 2) != 0)
        {
            if (num != 0)
            {
                ++parentObj1.hit_count;
                if (parentObj1.hit_count >= 3U)
                {
                    AppMain.gmBoss5RocketClearLeakageFlicker(parentObj1);
                    AppMain.gmBoss5RocketBlowProcInit(parentObj1);
                }
                else
                {
                    AppMain.gmBoss5RocketSetNoHitTime(parentObj1);
                    AppMain.gmBoss5RocketSetStuckLeanHitVib(parentObj1);
                }
            }
            else
            {
                AppMain.gmBoss5RocketSetDmgEnable(parentObj1, 0);
                AppMain.GmBoss5EfctCreateRocketRollSpark(parentObj1);
                parentObj1.flag |= 128U;
            }
        }
        else
        {
            AppMain.gmBoss5RocketClearLeakageFlicker(parentObj1);
            AppMain.gmBoss5RocketBlowProcInit(parentObj1);
        }
        AppMain.GmSoundPlaySE("Boss0_01");
    }

    public static void gmBoss5RocketOutFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_ROCKET_WORK gmsBosS5RocketWork = (AppMain.GMS_BOSS5_ROCKET_WORK)obj_work;
        AppMain.ObjDrawActionSummary(obj_work);
        gmsBosS5RocketWork.pivot_prev_pos.Assign(obj_work.pos);
    }

    public static void gmBoss5RocketMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_ROCKET_WORK gmsBosS5RocketWork = (AppMain.GMS_BOSS5_ROCKET_WORK)obj_work;
        if (((int)gmsBosS5RocketWork.flag & 2) != 0 && ((int)gmsBosS5RocketWork.flag & 128) == 0)
            AppMain.gmBoss5RocketUpdateNoHitTime(gmsBosS5RocketWork);
        if (gmsBosS5RocketWork.proc_update != null)
            gmsBosS5RocketWork.proc_update(gmsBosS5RocketWork);
        if (((int)gmsBosS5RocketWork.flag & 8) != 0)
            AppMain.GmBoss5EfctTryStartRocketLeakage(gmsBosS5RocketWork);
        else
            AppMain.GmBoss5EfctEndRocketLeakage(gmsBosS5RocketWork);
        AppMain.gmBoss5RocketUpdateMainRectPosition(gmsBosS5RocketWork);
    }

    public static void gmBoss5RocketNmlProcInit(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.gmBoss5RocketSetInitialDir(rkt_work);
        AppMain.gmBoss5RocketSetRectSize(rkt_work, 1);
        AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 0);
        AppMain.gmBoss5RocketSetDmgEnable(rkt_work, 0);
        AppMain.gmBoss5RocketInitPlySearch(rkt_work, 20);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketNmlProcUpdateFace);
    }

    public static void gmBoss5RocketNmlProcUpdateFace(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 0);
        AppMain.gmBoss5RocketUpdatePlySearch(rkt_work);
        AppMain.VecFx32 pos;
        AppMain.gmBoss5RocketGetPlySearchPos(rkt_work, out pos);
        AppMain.gmBoss5RocketUpdateDirPlyLockOn(rkt_work, ref pos);
        if (AppMain.gmBoss5RocketReceiveSignalLaunch(rkt_work) == 0)
            return;
        AppMain.gmBoss5RocketInitFlyDestDistance(rkt_work, 4096, 40960, 49152, ref obsObjectWork.pos, (int)obsObjectWork.dir.z, 786432);
        AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 1);
        AppMain.GmBoss5EfctCreateRocketLaunch(rkt_work);
        AppMain.GmSoundPlaySE("FinalBoss07");
        AppMain.GmBoss5EfctStartRocketJet(rkt_work);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketNmlProcUpdateFly);
    }

    public static void gmBoss5RocketNmlProcUpdateFly(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (AppMain.gmBoss5RocketUpdateFlyDest(rkt_work) == 0)
            return;
        AppMain.gmBoss5RocketInitFlyReverse(rkt_work, 4096, 0);
        AppMain.GmBoss5EfctEndRocketJet(rkt_work);
        AppMain.GmBoss5EfctStartRocketJetReverse(rkt_work);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketNmlProcUpdateWaitDecel);
    }

    public static void gmBoss5RocketNmlProcUpdateWaitDecel(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        if (AppMain.gmBoss5RocketUpdateFlyReverse(rkt_work) == 0)
            return;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        AppMain.VecFx32 pos_out;
        AppMain.gmBoss5RocketGetArmNodePosFx(rkt_work, out pos_out);
        int num = AppMain.gmBoss5RocketInitFlyDestPos(rkt_work, 2048, 0, 40960, obj_work.pos, ref pos_out);
        obj_work.dir.z = (ushort)(short)((long)ushort.MaxValue & (long)(num + AppMain.AKM_DEGtoA32(180)));
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketNmlProcUpdateWaitReturn);
    }

    public static void gmBoss5RocketNmlProcUpdateWaitReturn(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        if (AppMain.gmBoss5RocketUpdateFlyDest(rkt_work, 1) == 0)
            return;
        AppMain.gmBoss5RocketDispatchSignalReturned(rkt_work);
        AppMain.GmBoss5EfctEndRocketJet(rkt_work);
        AppMain.GmBoss5EfctCreateRocketDock((AppMain.GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj, rkt_work.rkt_type);
        AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 0);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketNmlProcUpdateFinalize);
    }

    public static void gmBoss5RocketNmlProcUpdateFinalize(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5RocketStrProcInit(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.gmBoss5RocketSetInitialDir(rkt_work);
        AppMain.gmBoss5RocketSetRectSize(rkt_work, 1);
        AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 0);
        AppMain.gmBoss5RocketSetDmgEnable(rkt_work, 0);
        AppMain.gmBoss5RocketInitPlySearch(rkt_work, 20);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketStrProcUpdateFace);
    }

    public static void gmBoss5RocketStrProcUpdateFace(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 0);
        AppMain.gmBoss5RocketUpdatePlySearch(rkt_work);
        AppMain.VecFx32 pos;
        AppMain.gmBoss5RocketGetPlySearchPos(rkt_work, out pos);
        AppMain.gmBoss5RocketUpdateDirPlyLockOn(rkt_work, ref pos);
        if (AppMain.gmBoss5RocketReceiveSignalReturn(rkt_work) != 0)
        {
            AppMain.gmBoss5RocketInitRocketStuckWithArmLerpRot(rkt_work, 0.1f);
            AppMain.gmBoss5RocketUpdateRocketStuckWithArmLerpRot(rkt_work);
            rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketStrProcUpdateRecover);
        }
        else
        {
            if (AppMain.gmBoss5RocketReceiveSignalLaunch(rkt_work) == 0)
                return;
            AppMain.gmBoss5RocketSetDispOfst(rkt_work, -10f, 1);
            AppMain.gmBoss5RocketInitFlyDestDistance(rkt_work, 6144, 40960, 61440, ref obsObjectWork.pos, (int)obsObjectWork.dir.z, 786432);
            AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 1);
            AppMain.GmBoss5EfctCreateRocketLaunch(rkt_work);
            AppMain.GmSoundPlaySE("FinalBoss07");
            AppMain.GmBoss5EfctStartRocketJet(rkt_work);
            rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketStrProcUpdateFlyTarget);
        }
    }

    public static void gmBoss5RocketStrProcUpdateFlyTarget(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (AppMain.gmBoss5RocketUpdateFlyDest(rkt_work) == 0)
            return;
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.VecFx32 dest_pos = new AppMain.VecFx32(obsObjectWork.pos.x, AppMain.GMM_BOSS5_AREA_TOP() - 196608, 0);
        AppMain.VecFx32 acc_vec = new AppMain.VecFx32(-2048, 0, 0);
        AppMain.gmBoss5RocketRedirectFlyDestPos(rkt_work, 2048, ref dest_pos);
        if (((int)obsObjectWork.disp_flag & 1) != 0)
            acc_vec.x = -acc_vec.x;
        AppMain.gmBoss5RocketInitFlyReverseVec(rkt_work, ref acc_vec, 1);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketStrProcUpdateFlyDecel);
    }

    public static void gmBoss5RocketStrProcUpdateFlyDecel(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.gmBoss5RocketUpdateDirFollowingAccSpd(rkt_work, 5f, 0);
        if (AppMain.gmBoss5RocketUpdateFlyReverse(rkt_work) == 0)
            return;
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.VecFx32 dest_pos = new AppMain.VecFx32(obsObjectWork.pos.x, AppMain.GMM_BOSS5_AREA_TOP() - 196608, 0);
        AppMain.gmBoss5RocketInitFlyDestPos(rkt_work, 2048, AppMain.MTM_MATH_ABS(obsObjectWork.spd.y), 61440, obsObjectWork.pos, ref dest_pos);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketStrProcUpdateFlyAbove);
    }

    public static void gmBoss5RocketStrProcUpdateFlyAbove(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.gmBoss5RocketUpdateDirFollowingAccSpd(rkt_work, 1f, 0);
        if (AppMain.gmBoss5RocketUpdateFlyDest(rkt_work) == 0)
            return;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 0);
        rkt_work.wait_timer = AppMain.gmBoss5RocketSeqGetWaitFallTime(rkt_work);
        AppMain.GmBoss5EfctEndRocketJet(rkt_work);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketStrProcUpdateWaitFall);
    }

    public static void gmBoss5RocketStrProcUpdateWaitFall(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        if (rkt_work.wait_timer >= 10U)
            obsObjectWork.pos.x = AppMain.GmBsCmnGetPlayerObj().pos.x;
        if (rkt_work.wait_timer != 0U)
        {
            --rkt_work.wait_timer;
        }
        else
        {
            AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obsObjectWork;
            obsObjectWork.spd.y = 8192;
            AppMain.gmBoss5RocketSetRectSize(rkt_work, 1);
            AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 1);
            AppMain.gmBoss5RocketSetDmgEnable(rkt_work, 1);
            AppMain.gmBoss5RocketInitLeakageFlicker(rkt_work);
            AppMain.GmBoss5EfctStartRocketSmoke(rkt_work);
            AppMain.gmBoss5RocketInitDirFalling(rkt_work);
            gmsEnemyComWork.enemy_flag &= 4294934527U;
            rkt_work.flag &= 4294967294U;
            rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketStrProcUpdateFall);
        }
    }

    public static void gmBoss5RocketStrProcUpdateFall(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.gmBoss5RocketUpdateDirFalling(rkt_work);
        if (AppMain.gmBoss5RocketUpdateLeakageFlicker(rkt_work) != 0)
            AppMain.gmBoss5RocketSetRectSize(rkt_work, 1);
        else
            AppMain.gmBoss5RocketSetRectSize(rkt_work, 0);
        if (obj_work.pos.y >= AppMain.GMM_BOSS5_AREA_TOP())
            obj_work.move_flag &= 4294967039U;
        if (((int)obj_work.move_flag & 1) == 0)
            return;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        rkt_work.flag |= 2U;
        AppMain.gmBoss5RocketSetRectSize(rkt_work, 0);
        AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 1);
        AppMain.gmBoss5RocketSetDmgEnable(rkt_work, 1);
        rkt_work.wait_timer = ((int)rkt_work.flag & 1) == 0 ? 300U : 30U;
        AppMain.gmBoss5RocketClearLeakageFlicker(rkt_work);
        AppMain.GmBoss5EfctCreateRocketLandingShockwave(rkt_work);
        AppMain.GmSoundPlaySE("FinalBoss13");
        AppMain.gmBoss5RocketEndDirFalling(rkt_work);
        AppMain.gmBoss5RocketInitStuckLean(rkt_work);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketStrProcUpdateStuck);
    }

    public static void gmBoss5RocketStrProcUpdateStuck(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        int num = AppMain.gmBoss5RocketUpdateStuckLean(rkt_work);
        if (num != 0 && ((int)rkt_work.flag & 128) != 0 && ((int)rkt_work.flag & 256) == 0)
        {
            AppMain.gmBoss5RocketSetStuckLeanHitVib(rkt_work);
            rkt_work.flag |= 256U;
        }
        if (((int)rkt_work.flag & 128) != 0)
            rkt_work.flag |= 8U;
        else
            rkt_work.flag &= 4294967287U;
        if (rkt_work.wait_timer != 0U)
        {
            --rkt_work.wait_timer;
        }
        else
        {
            if (num == 0)
                return;
            AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obsObjectWork;
            rkt_work.flag &= 4294967287U;
            AppMain.GmBoss5EfctEndRocketSmoke(rkt_work);
            AppMain.VecFx32 pos_out;
            AppMain.gmBoss5RocketGetArmNodePosFx(rkt_work, out pos_out);
            AppMain.gmBoss5RocketInitFlyDestPos(rkt_work, 1228, 0, 61440, obsObjectWork.pos, ref pos_out);
            rkt_work.flag &= 4294967293U;
            AppMain.gmBoss5RocketSetRectSize(rkt_work, 1);
            AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 1);
            AppMain.gmBoss5RocketSetDmgEnable(rkt_work, 0);
            gmsEnemyComWork.enemy_flag |= 32768U;
            rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketStrProcUpdateReturn);
        }
    }

    public static void gmBoss5RocketStrProcUpdateReturn(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        if (((int)rkt_work.flag & 128) != 0)
            AppMain.gmBoss5RocketUpdateDirFollowingAccSpd(rkt_work, 5f, 1);
        else
            AppMain.gmBoss5RocketUpdateDirFollowingAccSpd(rkt_work, 2f, 1);
        if (AppMain.gmBoss5RocketUpdateFlyDest(rkt_work, 1) == 0)
            return;
        AppMain.GmBoss5EfctCreateRocketDock((AppMain.GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj, rkt_work.rkt_type);
        AppMain.gmBoss5RocketDispatchSignalReturned(rkt_work);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketStrProcUpdateFinalize);
    }

    public static void gmBoss5RocketStrProcUpdateFinalize(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5RocketStrProcUpdateRecover(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj;
        AppMain.gmBoss5RocketUpdateRocketStuckWithArmLerpRot(rkt_work);
        if ((rkt_work.rkt_type != 0 || ((int)parentObj.flag & 4) != 0) && (rkt_work.rkt_type != 1 || ((int)parentObj.flag & 4) != 0))
            return;
        obsObjectWork.flag |= 4U;
    }

    public static void gmBoss5RocketBlowProcInit(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obsObjectWork;
        AppMain.GmPlayerAddScore((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj(), 100, obsObjectWork.pos.x, obsObjectWork.pos.y);
        AppMain.GmBoss5EfctEndRocketSmoke(rkt_work);
        rkt_work.flag &= 4294967293U;
        obsObjectWork.move_flag |= 16U;
        obsObjectWork.move_flag &= 4294967294U;
        gmsEnemyComWork.enemy_flag |= 32768U;
        AppMain.gmBoss5RocketSetAtkBodyRect(rkt_work);
        AppMain.gmBoss5RocketSetRectSize(rkt_work, 1);
        AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 1);
        AppMain.gmBoss5RocketSetDmgEnable(rkt_work, 0);
        AppMain.gmBoss5RocketInitFlyBlow(rkt_work);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketBlowProcUpdateFly);
    }

    public static void gmBoss5RocketBlowProcUpdateFly(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        if (AppMain.gmBoss5RocketUpdateFlyBlow(rkt_work) == 0)
            return;
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketBlowProcUpdateWaitHit);
    }

    public static void gmBoss5RocketBlowProcUpdateWaitHit(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.gmBoss5RocketUpdateFlyBlow(rkt_work);
        obj_work.pos.x = rkt_work.dest_pos.x;
        obj_work.pos.y = rkt_work.dest_pos.y;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
    }

    public static void gmBoss5RocketBounceProcInit(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obsObjectWork;
        obsObjectWork.move_flag |= 16U;
        obsObjectWork.move_flag &= 4294967294U;
        gmsEnemyComWork.enemy_flag |= 32768U;
        AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 0);
        AppMain.gmBoss5RocketSetDmgEnable(rkt_work, 0);
        AppMain.gmBoss5RocketInitFlyBounce(rkt_work);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketBounceProcUpdateFlyUp);
    }

    public static void gmBoss5RocketBounceProcUpdateFlyUp(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        if (AppMain.gmBoss5RocketUpdateFlyBounce(rkt_work) == 0)
            return;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketBounceProcUpdateWait);
    }

    public static void gmBoss5RocketBounceProcUpdateWait(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        if (AppMain.gmBoss5RocketReceiveSignalReturn(rkt_work) == 0)
            return;
        AppMain.VecFx32 pos_out;
        AppMain.gmBoss5RocketGetArmNodePosFx(rkt_work, out pos_out);
        AppMain.gmBoss5RocketInitFlyDestPos(rkt_work, 1228, 0, 61440, obsObjectWork.pos, ref pos_out);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketBounceProcUpdateReturn);
    }

    public static void gmBoss5RocketBounceProcUpdateReturn(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.gmBoss5RocketUpdateDirFollowingAccSpd(rkt_work, 5f, 1);
        if (AppMain.gmBoss5RocketUpdateFlyDest(rkt_work, 1) == 0)
            return;
        AppMain.GmBoss5EfctCreateRocketDock((AppMain.GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj, rkt_work.rkt_type);
        AppMain.gmBoss5RocketDispatchSignalReturned(rkt_work);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketBounceProcUpdateFinalize);
    }

    public static void gmBoss5RocketBounceProcUpdateFinalize(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.flag |= 4U;
    }

    public static void gmBoss5RocketCnctProcInit(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 1);
        AppMain.gmBoss5RocketSetRectSize(rkt_work, 1);
        AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 0);
        AppMain.gmBoss5RocketSetDmgEnable(rkt_work, 0);
        rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketCnctProcUpdateIdle);
    }

    public static void gmBoss5RocketCnctProcUpdateIdle(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj;
        AppMain.gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 1);
        if (rkt_work.rkt_type == 0 && ((int)parentObj.flag & 4) != 0 || rkt_work.rkt_type == 1 && ((int)parentObj.flag & 8) != 0)
        {
            obsObjectWork.disp_flag |= 32U;
            AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 0);
        }
        else
        {
            obsObjectWork.disp_flag &= 4294967263U;
            if (((int)parentObj.flag & 256) != 0)
                AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 1);
            else
                AppMain.gmBoss5RocketSetAtkEnable(rkt_work, 0);
            if (((int)parentObj.flag & 262144) == 0)
                return;
            rkt_work.wait_timer = rkt_work.rkt_type != 0 ? 30U : 10U;
            rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketCnctProcUpdateWaitScatterStart);
        }
    }

    public static void gmBoss5RocketCnctProcUpdateWaitScatterStart(
      AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.gmBoss5RocketUpdateRocketStuckWithArm(rkt_work, 1);
        if (rkt_work.wait_timer != 0U)
        {
            --rkt_work.wait_timer;
        }
        else
        {
            AppMain.gmBoss5RocketInitScatter(rkt_work);
            rkt_work.wait_timer = 180U;
            rkt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_ROCKET_WORK(AppMain.gmBoss5RocketCnctProcUpdateScatter);
        }
    }

    public static void gmBoss5RocketCnctProcUpdateScatter(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)rkt_work);
        AppMain.gmBoss5RocketUpdateScatter(rkt_work);
        if (rkt_work.wait_timer != 0U)
            --rkt_work.wait_timer;
        else
            obsObjectWork.flag |= 4U;
    }

    public static uint gmBoss5RocketSeqGetWaitFallTime(AppMain.GMS_BOSS5_ROCKET_WORK rkt_work)
    {
        int life = ((AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)rkt_work).parent_obj).mgr_work.life;
        AppMain.GMS_BOSS5_RKT_SEQ_WAITFALL_INFO rktSeqWaitfallInfo = (AppMain.GMS_BOSS5_RKT_SEQ_WAITFALL_INFO)null;
        int index1 = 0;
        int num1 = 0;
        for (int index2 = 0; index2 < 3; ++index2)
        {
            if (life <= AppMain.gm_boss5_rkt_seq_wait_fall_time_tbl[index2].life_threshold)
            {
                rktSeqWaitfallInfo = AppMain.gm_boss5_rkt_seq_wait_fall_time_tbl[index2];
                break;
            }
        }
        if (rktSeqWaitfallInfo == null)
            return 0;
        int num2 = AppMain.AkMathRandFx();
        for (int index2 = 0; index2 < 3; ++index2)
        {
            int num3 = rktSeqWaitfallInfo.probability[index2];
            if (num3 > 0)
            {
                index1 = index2;
                num1 += num3;
                if (num2 <= num1)
                    return rktSeqWaitfallInfo.frame[index2];
            }
        }
        return rktSeqWaitfallInfo.frame[index1];
    }

}