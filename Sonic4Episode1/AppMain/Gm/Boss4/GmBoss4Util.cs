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
    public static float GMM_BOSS4_PAL_SPEED(float spd)
    {
        return !AppMain.GmBoss4UtilCheckPAL() ? spd : spd * 1.2f;
    }

    public static int GMM_BOSS4_PAL_TIME(float time)
    {
        return !AppMain.GmBoss4UtilCheckPAL() ? (int)time : (int)((double)time * 0.833333313465118);
    }

    public static float GMM_BOSS4_PAL_ZOOM(float zoom)
    {
        if (!AppMain.GmBoss4UtilCheckPAL())
            return zoom;
        return (double)zoom < 1.0 ? zoom * 0.8333333f : zoom * 1.2f;
    }

    private static void GmBoss4UtilInit1ShotTimer(
      AppMain.GMS_BOSS4_1SHOT_TIMER one_shot_timer,
      uint frame)
    {
        AppMain.MTM_ASSERT((object)one_shot_timer);
        one_shot_timer.timer = frame;
        one_shot_timer.is_active = true;
    }

    private static bool GmBoss4UtilUpdate1ShotTimer(AppMain.GMS_BOSS4_1SHOT_TIMER one_shot_timer)
    {
        AppMain.MTM_ASSERT((object)one_shot_timer);
        if (!one_shot_timer.is_active)
            return false;
        if (one_shot_timer.timer != 0U)
        {
            --one_shot_timer.timer;
            return false;
        }
        one_shot_timer.is_active = false;
        return true;
    }

    private static void GmBoss4UtilInitNodeMatrix(
      AppMain.GMS_BOSS4_NODE_MATRIX node_work,
      AppMain.OBS_OBJECT_WORK obj_work,
      int max_node)
    {
        node_work.initCount = max_node;
        node_work.useCount = 0;
        AppMain.GmBsCmnInitBossMotionCBSystem(obj_work, node_work.mtn_mgr);
        AppMain.GmBsCmnCreateSNMWork(node_work.snm_work, obj_work.obj_3d._object, (ushort)max_node);
        AppMain.GmBsCmnAppendBossMotionCallback(node_work.mtn_mgr, node_work.snm_work.bmcb_link);
        node_work.obj_work = obj_work;
        for (int index = 0; index < 32; ++index)
            node_work.work[index] = -1;
        node_work._id = "SNM SYS";
    }

    private static void GmBoss4UtilExitNodeMatrix(AppMain.GMS_BOSS4_NODE_MATRIX node_work)
    {
        if (node_work._id != "SNM SYS")
            return;
        AppMain.GmBsCmnClearBossMotionCBSystem(node_work.obj_work);
        AppMain.GmBsCmnDeleteSNMWork(node_work.snm_work);
        node_work._id = "";
    }

    private static AppMain.NNS_MATRIX GmBoss4UtilGetNodeMatrix(
      AppMain.GMS_BOSS4_NODE_MATRIX node_work,
      int node_id)
    {
        if (node_work.work[node_id] < 0)
            node_work.work[node_id] = AppMain.GmBsCmnRegisterSNMNode(node_work.snm_work, node_id);
        return AppMain.GmBsCmnGetSNMMtx(node_work.snm_work, node_work.work[node_id]);
    }

    private static void GmBoss4UtilSetNodeMatrixNN(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BOSS4_NODE_MATRIX node_work,
      int node_id)
    {
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, node_work.snm_work, node_work.work[node_id], 1);
    }

    private static void GmBoss4UtilSetMatrixNN(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.NNS_MATRIX w_mtx)
    {
        AppMain.MTM_ASSERT((object)obj_work);
        AppMain.MTM_ASSERT((object)obj_work.obj_3d);
        AppMain.NNS_MATRIX userObjMtxR = obj_work.obj_3d.user_obj_mtx_r;
        obj_work.pos.x = AppMain.FX_F32_TO_FX32(w_mtx.M03);
        obj_work.pos.y = -AppMain.FX_F32_TO_FX32(w_mtx.M13);
        obj_work.pos.z = AppMain.FX_F32_TO_FX32(w_mtx.M23);
        obj_work.disp_flag |= 16777216U;
        AppMain.AkMathNormalizeMtx(userObjMtxR, w_mtx);
    }

    private static void GmBoss4UtilSetNodeMatrixES(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BOSS4_NODE_MATRIX node_work,
      int node_id)
    {
        AppMain.GmBsCmnUpdateObject3DESStuckWithNode(obj_work, node_work.snm_work, node_work.work[node_id], 1);
    }

    private static void GmBoss4UtilSetMatrixES(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.NNS_MATRIX w_mtx)
    {
        AppMain.NNS_MATRIX dst_mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.MTM_ASSERT((object)obj_work);
        AppMain.MTM_ASSERT((object)obj_work.obj_3des);
        obj_work.pos.x = AppMain.FX_F32_TO_FX32(w_mtx.M03);
        obj_work.pos.y = -AppMain.FX_F32_TO_FX32(w_mtx.M13);
        obj_work.pos.z = AppMain.FX_F32_TO_FX32(w_mtx.M23);
        obj_work.obj_3des.flag |= 32U;
        AppMain.AkMathNormalizeMtx(dst_mtx, w_mtx);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(dst_mtx);
    }

    private static void GmBoss4UtilPlayerStop(bool b)
    {
        if (b)
            ((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj()).no_key_timer = 737280;
        else
            ((AppMain.GMS_PLAYER_WORK)AppMain.GmBsCmnGetPlayerObj()).no_key_timer = 0;
    }

    private static void GmBoss4UtilTimerStop(bool b)
    {
        if (b)
            AppMain.g_gm_main_system.game_flag &= 4294966271U;
        else
            AppMain.g_gm_main_system.game_flag |= 1024U;
    }

    private static void GmBoss4UtilInitMove(
      AppMain.GMS_BOSS4_MOVE _work,
      AppMain.VecFx32 start,
      AppMain.VecFx32 end,
      int count,
      int type)
    {
        _work.start.x = start.x;
        _work.start.y = start.y;
        _work.start.z = start.z;
        _work.end.x = end.x;
        _work.end.y = end.y;
        _work.end.z = end.z;
        _work.max_count = count;
        _work.type = type;
        _work.now_count = 0;
    }

    private static bool GmBoss4UtilUpdateMove(AppMain.GMS_BOSS4_MOVE _work)
    {
        return AppMain.GmBoss4UtilUpdateMove(_work, out AppMain.VecFx32 _);
    }

    private static bool GmBoss4UtilUpdateMove(AppMain.GMS_BOSS4_MOVE _work, out AppMain.VecFx32 pos)
    {
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        vecFx32.x = _work.end.x - _work.start.x;
        vecFx32.y = _work.end.y - _work.start.y;
        vecFx32.z = _work.end.z - _work.start.z;
        if (_work.now_count < _work.max_count)
            ++_work.now_count;
        if (_work.now_count >= _work.max_count)
        {
            _work.now_count = _work.max_count;
            _work.pos.x = _work.end.x;
            _work.pos.y = _work.end.y;
            _work.pos.z = _work.end.z;
            pos.x = _work.end.x;
            pos.y = _work.end.y;
            pos.z = _work.end.z;
            return true;
        }
        if (_work.type == 0)
        {
            _work.pos.x = (int)((double)_work.start.x + (double)vecFx32.x * ((double)_work.now_count / (double)_work.max_count));
            _work.pos.y = (int)((double)_work.start.y + (double)vecFx32.y * ((double)_work.now_count / (double)_work.max_count));
            _work.pos.z = (int)((double)_work.start.z + (double)vecFx32.z * ((double)_work.now_count / (double)_work.max_count));
        }
        else if ((double)_work.now_count / (double)_work.max_count <= 0.5)
        {
            float num = (float)(0.5 - (double)AppMain.FX_Cos(AppMain.AKM_DEGtoA32((float)(180.0 * ((double)_work.now_count / (double)_work.max_count)))) * 0.000244140625 * 0.5);
            _work.pos.x = _work.start.x + (int)((double)vecFx32.x * (double)num);
            _work.pos.y = _work.start.y + (int)((double)vecFx32.y * (double)num);
            _work.pos.z = _work.start.z + (int)((double)vecFx32.z * (double)num);
        }
        else
        {
            float num = (float)((double)AppMain.FX_Cos(AppMain.AKM_DEGtoA32((float)(180.0 * ((double)_work.now_count / (double)_work.max_count)))) * 0.000244140625 * 0.5);
            _work.pos.x = _work.start.x + (int)((double)vecFx32.x * (0.5 - (double)num));
            _work.pos.y = _work.start.y + (int)((double)vecFx32.y * (0.5 - (double)num));
            _work.pos.z = _work.start.z + (int)((double)vecFx32.z * (0.5 - (double)num));
        }
        pos.x = _work.pos.x;
        pos.y = _work.pos.y;
        pos.z = _work.pos.z;
        return false;
    }

    private static void GmBoss4UtilUpdateMovePosition(
      AppMain.GMS_BOSS4_MOVE _work,
      AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x = _work.pos.x;
        obj_work.pos.y = _work.pos.y;
        obj_work.pos.z = _work.pos.z;
    }

    private static bool GmBoss4UtilIsDirectionPositiveFromCurrent(
      AppMain.GMS_BOSS4_DIRECTION _work,
      short target_angle)
    {
        return (int)((long)ushort.MaxValue & (long)((int)_work.cur_angle - (int)target_angle)) >= AppMain.AKM_DEGtoA32(180);
    }

    private static void GmBoss4UtilUpdateDirection(
      AppMain.GMS_BOSS4_DIRECTION _work,
      AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GmBoss4UtilUpdateDirection(_work, obj_work, false);
    }

    private static void GmBoss4UtilUpdateDirection(
      AppMain.GMS_BOSS4_DIRECTION _work,
      AppMain.OBS_OBJECT_WORK obj_work,
      bool flag)
    {
        if (_work.direction == 1)
            obj_work.disp_flag |= 1U;
        else
            obj_work.disp_flag &= 4294967294U;
        if (flag)
            return;
        obj_work.dir.y = (ushort)_work.cur_angle;
    }

    private static void GmBoss4UtilSetDirectionNormal(AppMain.GMS_BOSS4_DIRECTION _work)
    {
        if (_work.direction == 1)
            AppMain.GmBoss4UtilSetDirection(_work, AppMain.GMD_BOSS4_LEFTWARD_ANGLE);
        else
            AppMain.GmBoss4UtilSetDirection(_work, AppMain.GMD_BOSS4_RIGHTWARD_ANGLE);
        _work.orig_angle = (short)0;
        _work.turn_angle = 0;
    }

    private static void GmBoss4UtilSetDirection(AppMain.GMS_BOSS4_DIRECTION _work, short deg)
    {
        _work.cur_angle = deg;
    }

    private static void GmBoss4UtilInitTurn(
      AppMain.GMS_BOSS4_DIRECTION _work,
      int turn_amount,
      int turn_spd)
    {
        AppMain.MTM_ASSERT(0 == (int.MinValue & (turn_amount ^ turn_spd)));
        _work.orig_angle = _work.cur_angle;
        _work.turn_angle = 0;
        _work.turn_amount = turn_amount;
        _work.turn_spd = turn_spd;
        AppMain.GmBoss4UtilSetDirection(_work, (short)((int)_work.orig_angle + _work.turn_angle));
    }

    private static void GmBoss4UtilInitTurn(
      AppMain.GMS_BOSS4_DIRECTION _work,
      short dest_angle,
      int frame,
      bool is_positive)
    {
        AppMain.MTM_ASSERT(frame > 0);
        int turn_amount = !is_positive ? (int)(ushort)((int)dest_angle - AppMain.AKM_DEGtoA32(360) - ((int)_work.cur_angle - AppMain.AKM_DEGtoA32(360))) - AppMain.AKM_DEGtoA32(360) : (int)(ushort)((uint)dest_angle - (uint)_work.cur_angle);
        int turn_spd = turn_amount / frame;
        AppMain.GmBoss4UtilInitTurn(_work, turn_amount, turn_spd);
    }

    private static bool GmBoss4UtilUpdateTurn(AppMain.GMS_BOSS4_DIRECTION _work, float spd_rate)
    {
        bool flag = false;
        AppMain.MTM_ASSERT((double)spd_rate >= 0.0);
        float a = spd_rate * (float)_work.turn_spd;
        AppMain.MTM_ASSERT((double)AppMain.MTM_MATH_ABS(a) <= 2147483648.0);
        _work.turn_angle += (int)a;
        if (_work.turn_spd > 0)
        {
            if (_work.turn_angle >= _work.turn_amount)
                flag = true;
        }
        else if (_work.turn_spd < 0 && _work.turn_angle <= _work.turn_amount)
            flag = true;
        if (flag)
            _work.turn_angle = _work.turn_amount;
        AppMain.GmBoss4UtilSetDirection(_work, (short)((int)_work.orig_angle + _work.turn_angle));
        return flag;
    }

    private static void GmBoss4UtilInitTurnGently(
      AppMain.GMS_BOSS4_DIRECTION _work,
      short dest_angle,
      int frame,
      bool is_positive)
    {
        AppMain.MTM_ASSERT(frame > 0);
        _work.orig_angle = _work.cur_angle;
        _work.turn_angle = 0;
        _work.turn_spd = 0;
        if (is_positive)
        {
            ushort num = (ushort)((uint)dest_angle - (uint)_work.cur_angle);
            _work.turn_amount = (int)num;
        }
        else
        {
            ushort num = (ushort)((int)dest_angle - AppMain.AKM_DEGtoA32(360) - ((int)_work.cur_angle - AppMain.AKM_DEGtoA32(360)));
            _work.turn_amount = (int)num - AppMain.AKM_DEGtoA32(360);
        }
        _work.turn_gen_var = 0;
        float num1 = 180f / (float)frame;
        AppMain.MTM_ASSERT((double)AppMain.MTM_MATH_ABS(num1) <= 2147483648.0);
        _work.turn_gen_factor = AppMain.AKM_DEGtoA32(num1);
        AppMain.GmBoss4UtilSetDirection(_work, (short)((int)_work.orig_angle + _work.turn_angle));
    }

    private static bool GmBoss4UtilUpdateTurnGently(AppMain.GMS_BOSS4_DIRECTION _work)
    {
        bool flag = false;
        AppMain.MTM_ASSERT(_work.turn_gen_factor > 0);
        _work.turn_gen_var += _work.turn_gen_factor;
        if (_work.turn_gen_var >= AppMain.AKM_DEGtoA32(180))
        {
            _work.turn_gen_var = AppMain.AKM_DEGtoA32(180);
            flag = true;
        }
        float a = (float)((double)_work.turn_amount * 0.5 * (1.0 - (double)AppMain.nnCos(_work.turn_gen_var)));
        AppMain.MTM_ASSERT((double)AppMain.MTM_MATH_ABS(a) <= 2147483648.0);
        _work.turn_angle = (int)a;
        if (flag)
            _work.turn_angle = _work.turn_amount;
        AppMain.GmBoss4UtilSetDirection(_work, (short)((int)_work.orig_angle + _work.turn_angle));
        return flag;
    }

    private static bool GmBoss4UtilLookAtPlayer(
      AppMain.GMS_BOSS4_DIRECTION _work,
      AppMain.OBS_OBJECT_WORK obj_work,
      int time)
    {
        if (AppMain.GmBsCmnGetPlayerObj().pos.x < obj_work.pos.x)
        {
            _work.direction = 1;
            AppMain.GmBoss4UtilInitTurnGently(_work, AppMain.GMD_BOSS4_LEFTWARD_ANGLE, time, false);
        }
        else
        {
            _work.direction = 0;
            AppMain.GmBoss4UtilInitTurnGently(_work, AppMain.GMD_BOSS4_RIGHTWARD_ANGLE, time, true);
        }
        return AppMain.GmBoss4UtilUpdateTurnGently(_work);
    }

    private static bool GmBoss4UtilLookAtPlayerCheckDirection(
      AppMain.GMS_BOSS4_DIRECTION _work,
      AppMain.OBS_OBJECT_WORK obj_work,
      int time)
    {
        if (AppMain.GmBsCmnGetPlayerObj().pos.x < obj_work.pos.x)
        {
            if (_work.direction != 1)
            {
                _work.direction = 1;
                AppMain.GmBoss4UtilInitTurnGently(_work, AppMain.GMD_BOSS4_LEFTWARD_ANGLE, time, false);
            }
        }
        else if (_work.direction != 0)
        {
            _work.direction = 0;
            AppMain.GmBoss4UtilInitTurnGently(_work, AppMain.GMD_BOSS4_RIGHTWARD_ANGLE, time, true);
        }
        return AppMain.GmBoss4UtilUpdateTurnGently(_work);
    }

    private static bool GmBoss4UtilLookAtCenter(
      AppMain.GMS_BOSS4_DIRECTION _work,
      AppMain.OBS_OBJECT_WORK obj_work,
      int time)
    {
        if (AppMain.GMM_BOSS4_AREA_CENTER_X() < obj_work.pos.x)
        {
            _work.direction = 1;
            AppMain.GmBoss4UtilInitTurnGently(_work, AppMain.GMD_BOSS4_LEFTWARD_ANGLE, time, false);
        }
        else
        {
            _work.direction = 0;
            AppMain.GmBoss4UtilInitTurnGently(_work, AppMain.GMD_BOSS4_RIGHTWARD_ANGLE, time, true);
        }
        return AppMain.GmBoss4UtilUpdateTurnGently(_work);
    }

    private static bool GmBoss4UtilLookAt(AppMain.GMS_BOSS4_DIRECTION _work)
    {
        return AppMain.GmBoss4UtilUpdateTurnGently(_work);
    }

    private static void GmBoss4UtilInitFlicker(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BOSS4_FLICKER_WORK flk_work)
    {
        AppMain.GmBoss4UtilInitFlicker(obj_work, flk_work, 3, 0, 4, 0, AppMain.gm_boss4_color_white);
    }

    private static void GmBoss4UtilInitFlicker(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BOSS4_FLICKER_WORK flk_work,
      int times,
      int start,
      int spd,
      int interval,
      AppMain.NNS_RGB rgb)
    {
        int num = AppMain.AKM_DEGtoA32(360f / (float)(spd + 1));
        AppMain.MTM_ASSERT((object)obj_work);
        AppMain.MTM_ASSERT((object)obj_work.obj_3d);
        AppMain.MTM_ASSERT((object)flk_work);
        flk_work.is_active = true;
        flk_work.cycles = (uint)times;
        flk_work.interval_timer = (uint)start;
        flk_work.cur_angle = 0;
        flk_work.add_timer = num;
        flk_work.interval_flk = (uint)interval;
        flk_work.color.r = rgb.r;
        flk_work.color.g = rgb.g;
        flk_work.color.b = rgb.b;
        AppMain.GmBsCmnClearObject3DNNFadedColor(obj_work);
    }

    private static bool GmBoss4UtilUpdateFlicker(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BOSS4_FLICKER_WORK flk_work)
    {
        AppMain.MTM_ASSERT((object)obj_work);
        AppMain.MTM_ASSERT((object)obj_work.obj_3d);
        AppMain.MTM_ASSERT((object)flk_work);
        if (!flk_work.is_active)
            return true;
        if (flk_work.cycles != 0U)
        {
            if (flk_work.interval_timer != 0U)
            {
                --flk_work.interval_timer;
            }
            else
            {
                flk_work.cur_angle += flk_work.add_timer;
                if (flk_work.cur_angle >= AppMain.AKM_DEGtoA32(360f))
                {
                    flk_work.cur_angle = 0;
                    --flk_work.cycles;
                    flk_work.interval_timer = flk_work.interval_flk;
                }
            }
            AppMain.GmBsCmnSetObject3DNNFadedColor(obj_work, flk_work.color, (float)((1.0 - (double)AppMain.nnCos(flk_work.cur_angle)) / 2.0));
            return false;
        }
        if (flk_work.is_active)
            AppMain.GmBoss4UtilEndFlicker(obj_work, flk_work);
        return true;
    }

    private static void GmBoss4UtilEndFlicker(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_BOSS4_FLICKER_WORK flk_work)
    {
        AppMain.MTM_ASSERT((object)obj_work);
        AppMain.MTM_ASSERT((object)obj_work.obj_3d);
        AppMain.MTM_ASSERT((object)flk_work);
        flk_work.Clear();
        AppMain.GmBsCmnClearObject3DNNFadedColor(obj_work);
    }

    private static void GmBoss4UtilRotateVecFx32(ref AppMain.VecFx32 f, int angle)
    {
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        nnsMatrix.M00 = 1f;
        nnsMatrix.M10 = 0.0f;
        nnsMatrix.M20 = 0.0f;
        nnsMatrix.M30 = 0.0f;
        nnsMatrix.M01 = 0.0f;
        nnsMatrix.M11 = 1f;
        nnsMatrix.M21 = 0.0f;
        nnsMatrix.M31 = 0.0f;
        nnsMatrix.M02 = 0.0f;
        nnsMatrix.M12 = 0.0f;
        nnsMatrix.M22 = 1f;
        nnsMatrix.M32 = 0.0f;
        nnsMatrix.M03 = 0.0f;
        nnsMatrix.M13 = 0.0f;
        nnsMatrix.M23 = 0.0f;
        nnsMatrix.M33 = 1f;
        AppMain.nnMakeRotateZMatrix(nnsMatrix, angle);
        AppMain.nnTranslateMatrix(nnsMatrix, nnsMatrix, AppMain.FX_FX32_TO_F32(f.x), AppMain.FX_FX32_TO_F32(f.y), AppMain.FX_FX32_TO_F32(f.z));
        f.x = AppMain.FX_F32_TO_FX32(nnsMatrix.M03);
        f.y = AppMain.FX_F32_TO_FX32(nnsMatrix.M13);
        f.z = AppMain.FX_F32_TO_FX32(nnsMatrix.M23);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void GmBoss4UtilIterateDamageRingInit()
    {
        AppMain.gm_boss4_util_ring = AppMain.GmRingGetWork().damage_ring_list_start;
    }

    private static AppMain.GMS_RING_WORK GmBoss4UtilIterateDamageRingGet()
    {
        AppMain.GMS_RING_WORK gmBoss4UtilRing = AppMain.gm_boss4_util_ring;
        if (gmBoss4UtilRing == null)
            return (AppMain.GMS_RING_WORK)null;
        AppMain.gm_boss4_util_ring = gmBoss4UtilRing.post_ring;
        return gmBoss4UtilRing;
    }

    private static void GmBoss4UtilSetPlayerAttackReaction(
      AppMain.OBS_OBJECT_WORK player,
      AppMain.OBS_OBJECT_WORK enemy)
    {
        AppMain.UNREFERENCED_PARAMETER((object)enemy);
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)player;
        AppMain.OBS_OBJECT_WORK bodyWork = AppMain.GmBoss4GetBodyWork();
        if (((int)ply_work.obj_work.move_flag & 16) != 0)
        {
            AppMain.GmPlySeqAtkReactionInit(ply_work);
            AppMain.GmPlySeqSetJumpState(ply_work, 0, 5U);
            ply_work.obj_work.spd_m = 0;
            if (bodyWork != null)
            {
                if (ply_work.obj_work.pos.x < bodyWork.pos.x)
                {
                    ply_work.obj_work.spd.x = -AppMain.FX_F32_TO_FX32(5f);
                    if (AppMain.GmBoss4GetScrollOffset() != 0)
                        ply_work.obj_work.spd.x = AppMain.FX_F32_TO_FX32(3f);
                }
                else
                    ply_work.obj_work.spd.x = AppMain.FX_F32_TO_FX32(5f);
            }
            else
            {
                if (ply_work.obj_work.move.x >= 0)
                {
                    ply_work.obj_work.spd.x = -AppMain.FX_F32_TO_FX32(5f);
                    if (AppMain.GmBoss4GetScrollOffset() != 0)
                        ply_work.obj_work.spd.x = AppMain.FX_F32_TO_FX32(3f);
                }
                else
                    ply_work.obj_work.spd.x = AppMain.FX_F32_TO_FX32(5f);
                int seqState = ply_work.seq_state;
            }
            ply_work.obj_work.spd.y = -AppMain.FX_F32_TO_FX32(4f);
            AppMain.GmPlySeqSetNoJumpMoveTime(ply_work, 102400);
        }
        else
        {
            ply_work.obj_work.disp_flag ^= 1U;
            AppMain.GmPlySeqChangeSequence(ply_work, 10);
            if (ply_work.obj_work.spd_m != 0)
            {
                ply_work.obj_work.spd_m = -ply_work.obj_work.spd_m;
            }
            else
            {
                int num = 0;
                if (bodyWork != null)
                    num = bodyWork.pos.x;
                if (num > ply_work.obj_work.pos.x)
                {
                    ply_work.obj_work.spd_m = -49152;
                    ply_work.obj_work.disp_flag |= 1U;
                }
                else
                {
                    ply_work.obj_work.spd_m = 49152;
                    ply_work.obj_work.disp_flag &= 4294967294U;
                }
            }
        }
    }

    private static bool GmBoss4UtilCheckPAL()
    {
        return false;
    }

    private static bool GmBoss4UtilIsScrollLocking()
    {
        return AppMain.g_gm_main_system.map_fcol.left != 0 || AppMain.g_gm_main_system.map_fcol.right != (int)AppMain.g_gm_main_system.map_fcol.map_block_num_x * 64;
    }

    private static void GmBoss4UtilInitNoHitTimer(
      AppMain.GMS_BOSS4_NOHIT_TIMER work,
      AppMain.GMS_ENEMY_COM_WORK ene_com,
      int time)
    {
        work.ene_com = ene_com;
        work.timer = (uint)(time + 1);
        AppMain.GmBoss4UtilUpdateNoHitTimer(work);
    }

    private static bool GmBoss4UtilUpdateNoHitTimer(AppMain.GMS_BOSS4_NOHIT_TIMER work)
    {
        AppMain.GMS_ENEMY_COM_WORK eneCom = work.ene_com;
        if (work.timer > 0U)
        {
            --work.timer;
            eneCom.rect_work[1].flag |= 2048U;
            return false;
        }
        eneCom.rect_work[1].flag &= 4294965247U;
        return true;
    }
}