public partial class AppMain
{
    public static float GMM_BOSS4_PAL_SPEED(float spd)
    {
        return !GmBoss4UtilCheckPAL() ? spd : spd * 1.2f;
    }

    public static int GMM_BOSS4_PAL_TIME(float time)
    {
        return !GmBoss4UtilCheckPAL() ? (int)time : (int)(time * 0.833333313465118);
    }

    public static float GMM_BOSS4_PAL_ZOOM(float zoom)
    {
        if (!GmBoss4UtilCheckPAL())
            return zoom;
        return zoom < 1.0 ? zoom * 0.8333333f : zoom * 1.2f;
    }

    private static void GmBoss4UtilInit1ShotTimer(
      GMS_BOSS4_1SHOT_TIMER one_shot_timer,
      uint frame)
    {
        MTM_ASSERT(one_shot_timer);
        one_shot_timer.timer = frame;
        one_shot_timer.is_active = true;
    }

    private static bool GmBoss4UtilUpdate1ShotTimer(GMS_BOSS4_1SHOT_TIMER one_shot_timer)
    {
        MTM_ASSERT(one_shot_timer);
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
      GMS_BOSS4_NODE_MATRIX node_work,
      OBS_OBJECT_WORK obj_work,
      int max_node)
    {
        node_work.initCount = max_node;
        node_work.useCount = 0;
        GmBsCmnInitBossMotionCBSystem(obj_work, node_work.mtn_mgr);
        GmBsCmnCreateSNMWork(node_work.snm_work, obj_work.obj_3d._object, (ushort)max_node);
        GmBsCmnAppendBossMotionCallback(node_work.mtn_mgr, node_work.snm_work.bmcb_link);
        node_work.obj_work = obj_work;
        for (int index = 0; index < 32; ++index)
            node_work.work[index] = -1;
        node_work._id = "SNM SYS";
    }

    private static void GmBoss4UtilExitNodeMatrix(GMS_BOSS4_NODE_MATRIX node_work)
    {
        if (node_work._id != "SNM SYS")
            return;
        GmBsCmnClearBossMotionCBSystem(node_work.obj_work);
        GmBsCmnDeleteSNMWork(node_work.snm_work);
        node_work._id = "";
    }

    private static NNS_MATRIX GmBoss4UtilGetNodeMatrix(
      GMS_BOSS4_NODE_MATRIX node_work,
      int node_id)
    {
        if (node_work.work[node_id] < 0)
            node_work.work[node_id] = GmBsCmnRegisterSNMNode(node_work.snm_work, node_id);
        return GmBsCmnGetSNMMtx(node_work.snm_work, node_work.work[node_id]);
    }

    private static void GmBoss4UtilSetNodeMatrixNN(
      OBS_OBJECT_WORK obj_work,
      GMS_BOSS4_NODE_MATRIX node_work,
      int node_id)
    {
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, node_work.snm_work, node_work.work[node_id], 1);
    }

    private static void GmBoss4UtilSetMatrixNN(
      OBS_OBJECT_WORK obj_work,
      NNS_MATRIX w_mtx)
    {
        MTM_ASSERT(obj_work);
        MTM_ASSERT(obj_work.obj_3d);
        NNS_MATRIX userObjMtxR = obj_work.obj_3d.user_obj_mtx_r;
        obj_work.pos.x = FX_F32_TO_FX32(w_mtx.M03);
        obj_work.pos.y = -FX_F32_TO_FX32(w_mtx.M13);
        obj_work.pos.z = FX_F32_TO_FX32(w_mtx.M23);
        obj_work.disp_flag |= 16777216U;
        AkMathNormalizeMtx(userObjMtxR, w_mtx);
    }

    private static void GmBoss4UtilSetNodeMatrixES(
      OBS_OBJECT_WORK obj_work,
      GMS_BOSS4_NODE_MATRIX node_work,
      int node_id)
    {
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, node_work.snm_work, node_work.work[node_id], 1);
    }

    private static void GmBoss4UtilSetMatrixES(
      OBS_OBJECT_WORK obj_work,
      NNS_MATRIX w_mtx)
    {
        NNS_MATRIX dst_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        MTM_ASSERT(obj_work);
        MTM_ASSERT(obj_work.obj_3des);
        obj_work.pos.x = FX_F32_TO_FX32(w_mtx.M03);
        obj_work.pos.y = -FX_F32_TO_FX32(w_mtx.M13);
        obj_work.pos.z = FX_F32_TO_FX32(w_mtx.M23);
        obj_work.obj_3des.flag |= 32U;
        AkMathNormalizeMtx(dst_mtx, w_mtx);
        GlobalPool<NNS_MATRIX>.Release(dst_mtx);
    }

    private static void GmBoss4UtilPlayerStop(bool b)
    {
        if (b)
            ((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj()).no_key_timer = 737280;
        else
            ((GMS_PLAYER_WORK)GmBsCmnGetPlayerObj()).no_key_timer = 0;
    }

    private static void GmBoss4UtilTimerStop(bool b)
    {
        if (b)
            g_gm_main_system.game_flag &= 4294966271U;
        else
            g_gm_main_system.game_flag |= 1024U;
    }

    private static void GmBoss4UtilInitMove(
      GMS_BOSS4_MOVE _work,
      VecFx32 start,
      VecFx32 end,
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

    private static bool GmBoss4UtilUpdateMove(GMS_BOSS4_MOVE _work)
    {
        return GmBoss4UtilUpdateMove(_work, out VecFx32 _);
    }

    private static bool GmBoss4UtilUpdateMove(GMS_BOSS4_MOVE _work, out VecFx32 pos)
    {
        VecFx32 vecFx32 = new VecFx32();
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
            _work.pos.x = (int)(_work.start.x + vecFx32.x * (_work.now_count / (double)_work.max_count));
            _work.pos.y = (int)(_work.start.y + vecFx32.y * (_work.now_count / (double)_work.max_count));
            _work.pos.z = (int)(_work.start.z + vecFx32.z * (_work.now_count / (double)_work.max_count));
        }
        else if (_work.now_count / (double)_work.max_count <= 0.5)
        {
            float num = (float)(0.5 - FX_Cos(AKM_DEGtoA32((float)(180.0 * (_work.now_count / (double)_work.max_count)))) * 0.000244140625 * 0.5);
            _work.pos.x = _work.start.x + (int)(vecFx32.x * (double)num);
            _work.pos.y = _work.start.y + (int)(vecFx32.y * (double)num);
            _work.pos.z = _work.start.z + (int)(vecFx32.z * (double)num);
        }
        else
        {
            float num = (float)(FX_Cos(AKM_DEGtoA32((float)(180.0 * (_work.now_count / (double)_work.max_count)))) * 0.000244140625 * 0.5);
            _work.pos.x = _work.start.x + (int)(vecFx32.x * (0.5 - num));
            _work.pos.y = _work.start.y + (int)(vecFx32.y * (0.5 - num));
            _work.pos.z = _work.start.z + (int)(vecFx32.z * (0.5 - num));
        }
        pos.x = _work.pos.x;
        pos.y = _work.pos.y;
        pos.z = _work.pos.z;
        return false;
    }

    private static void GmBoss4UtilUpdateMovePosition(
      GMS_BOSS4_MOVE _work,
      OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x = _work.pos.x;
        obj_work.pos.y = _work.pos.y;
        obj_work.pos.z = _work.pos.z;
    }

    private static bool GmBoss4UtilIsDirectionPositiveFromCurrent(
      GMS_BOSS4_DIRECTION _work,
      short target_angle)
    {
        return (int)(ushort.MaxValue & (long)(_work.cur_angle - target_angle)) >= AKM_DEGtoA32(180);
    }

    private static void GmBoss4UtilUpdateDirection(
      GMS_BOSS4_DIRECTION _work,
      OBS_OBJECT_WORK obj_work)
    {
        GmBoss4UtilUpdateDirection(_work, obj_work, false);
    }

    private static void GmBoss4UtilUpdateDirection(
      GMS_BOSS4_DIRECTION _work,
      OBS_OBJECT_WORK obj_work,
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

    private static void GmBoss4UtilSetDirectionNormal(GMS_BOSS4_DIRECTION _work)
    {
        if (_work.direction == 1)
            GmBoss4UtilSetDirection(_work, GMD_BOSS4_LEFTWARD_ANGLE);
        else
            GmBoss4UtilSetDirection(_work, GMD_BOSS4_RIGHTWARD_ANGLE);
        _work.orig_angle = 0;
        _work.turn_angle = 0;
    }

    private static void GmBoss4UtilSetDirection(GMS_BOSS4_DIRECTION _work, short deg)
    {
        _work.cur_angle = deg;
    }

    private static void GmBoss4UtilInitTurn(
      GMS_BOSS4_DIRECTION _work,
      int turn_amount,
      int turn_spd)
    {
        MTM_ASSERT(0 == (int.MinValue & (turn_amount ^ turn_spd)));
        _work.orig_angle = _work.cur_angle;
        _work.turn_angle = 0;
        _work.turn_amount = turn_amount;
        _work.turn_spd = turn_spd;
        GmBoss4UtilSetDirection(_work, (short)(_work.orig_angle + _work.turn_angle));
    }

    private static void GmBoss4UtilInitTurn(
      GMS_BOSS4_DIRECTION _work,
      short dest_angle,
      int frame,
      bool is_positive)
    {
        MTM_ASSERT(frame > 0);
        int turn_amount = !is_positive ? (ushort)(dest_angle - AKM_DEGtoA32(360) - (_work.cur_angle - AKM_DEGtoA32(360))) - AKM_DEGtoA32(360) : (ushort)((uint)dest_angle - (uint)_work.cur_angle);
        int turn_spd = turn_amount / frame;
        GmBoss4UtilInitTurn(_work, turn_amount, turn_spd);
    }

    private static bool GmBoss4UtilUpdateTurn(GMS_BOSS4_DIRECTION _work, float spd_rate)
    {
        bool flag = false;
        MTM_ASSERT(spd_rate >= 0.0);
        float a = spd_rate * _work.turn_spd;
        MTM_ASSERT(MTM_MATH_ABS(a) <= 2147483648.0);
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
        GmBoss4UtilSetDirection(_work, (short)(_work.orig_angle + _work.turn_angle));
        return flag;
    }

    private static void GmBoss4UtilInitTurnGently(
      GMS_BOSS4_DIRECTION _work,
      short dest_angle,
      int frame,
      bool is_positive)
    {
        MTM_ASSERT(frame > 0);
        _work.orig_angle = _work.cur_angle;
        _work.turn_angle = 0;
        _work.turn_spd = 0;
        if (is_positive)
        {
            ushort num = (ushort)((uint)dest_angle - (uint)_work.cur_angle);
            _work.turn_amount = num;
        }
        else
        {
            ushort num = (ushort)(dest_angle - AKM_DEGtoA32(360) - (_work.cur_angle - AKM_DEGtoA32(360)));
            _work.turn_amount = num - AKM_DEGtoA32(360);
        }
        _work.turn_gen_var = 0;
        float num1 = 180f / frame;
        MTM_ASSERT(MTM_MATH_ABS(num1) <= 2147483648.0);
        _work.turn_gen_factor = AKM_DEGtoA32(num1);
        GmBoss4UtilSetDirection(_work, (short)(_work.orig_angle + _work.turn_angle));
    }

    private static bool GmBoss4UtilUpdateTurnGently(GMS_BOSS4_DIRECTION _work)
    {
        bool flag = false;
        MTM_ASSERT(_work.turn_gen_factor > 0);
        _work.turn_gen_var += _work.turn_gen_factor;
        if (_work.turn_gen_var >= AKM_DEGtoA32(180))
        {
            _work.turn_gen_var = AKM_DEGtoA32(180);
            flag = true;
        }
        float a = (float)(_work.turn_amount * 0.5 * (1.0 - nnCos(_work.turn_gen_var)));
        MTM_ASSERT(MTM_MATH_ABS(a) <= 2147483648.0);
        _work.turn_angle = (int)a;
        if (flag)
            _work.turn_angle = _work.turn_amount;
        GmBoss4UtilSetDirection(_work, (short)(_work.orig_angle + _work.turn_angle));
        return flag;
    }

    private static bool GmBoss4UtilLookAtPlayer(
      GMS_BOSS4_DIRECTION _work,
      OBS_OBJECT_WORK obj_work,
      int time)
    {
        if (GmBsCmnGetPlayerObj().pos.x < obj_work.pos.x)
        {
            _work.direction = 1;
            GmBoss4UtilInitTurnGently(_work, GMD_BOSS4_LEFTWARD_ANGLE, time, false);
        }
        else
        {
            _work.direction = 0;
            GmBoss4UtilInitTurnGently(_work, GMD_BOSS4_RIGHTWARD_ANGLE, time, true);
        }
        return GmBoss4UtilUpdateTurnGently(_work);
    }

    private static bool GmBoss4UtilLookAtPlayerCheckDirection(
      GMS_BOSS4_DIRECTION _work,
      OBS_OBJECT_WORK obj_work,
      int time)
    {
        if (GmBsCmnGetPlayerObj().pos.x < obj_work.pos.x)
        {
            if (_work.direction != 1)
            {
                _work.direction = 1;
                GmBoss4UtilInitTurnGently(_work, GMD_BOSS4_LEFTWARD_ANGLE, time, false);
            }
        }
        else if (_work.direction != 0)
        {
            _work.direction = 0;
            GmBoss4UtilInitTurnGently(_work, GMD_BOSS4_RIGHTWARD_ANGLE, time, true);
        }
        return GmBoss4UtilUpdateTurnGently(_work);
    }

    private static bool GmBoss4UtilLookAtCenter(
      GMS_BOSS4_DIRECTION _work,
      OBS_OBJECT_WORK obj_work,
      int time)
    {
        if (GMM_BOSS4_AREA_CENTER_X() < obj_work.pos.x)
        {
            _work.direction = 1;
            GmBoss4UtilInitTurnGently(_work, GMD_BOSS4_LEFTWARD_ANGLE, time, false);
        }
        else
        {
            _work.direction = 0;
            GmBoss4UtilInitTurnGently(_work, GMD_BOSS4_RIGHTWARD_ANGLE, time, true);
        }
        return GmBoss4UtilUpdateTurnGently(_work);
    }

    private static bool GmBoss4UtilLookAt(GMS_BOSS4_DIRECTION _work)
    {
        return GmBoss4UtilUpdateTurnGently(_work);
    }

    private static void GmBoss4UtilInitFlicker(
      OBS_OBJECT_WORK obj_work,
      GMS_BOSS4_FLICKER_WORK flk_work)
    {
        GmBoss4UtilInitFlicker(obj_work, flk_work, 3, 0, 4, 0, gm_boss4_color_white);
    }

    private static void GmBoss4UtilInitFlicker(
      OBS_OBJECT_WORK obj_work,
      GMS_BOSS4_FLICKER_WORK flk_work,
      int times,
      int start,
      int spd,
      int interval,
      NNS_RGB rgb)
    {
        int num = AKM_DEGtoA32(360f / (spd + 1));
        MTM_ASSERT(obj_work);
        MTM_ASSERT(obj_work.obj_3d);
        MTM_ASSERT(flk_work);
        flk_work.is_active = true;
        flk_work.cycles = (uint)times;
        flk_work.interval_timer = (uint)start;
        flk_work.cur_angle = 0;
        flk_work.add_timer = num;
        flk_work.interval_flk = (uint)interval;
        flk_work.color.r = rgb.r;
        flk_work.color.g = rgb.g;
        flk_work.color.b = rgb.b;
        GmBsCmnClearObject3DNNFadedColor(obj_work);
    }

    private static bool GmBoss4UtilUpdateFlicker(
      OBS_OBJECT_WORK obj_work,
      GMS_BOSS4_FLICKER_WORK flk_work)
    {
        MTM_ASSERT(obj_work);
        MTM_ASSERT(obj_work.obj_3d);
        MTM_ASSERT(flk_work);
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
                if (flk_work.cur_angle >= AKM_DEGtoA32(360f))
                {
                    flk_work.cur_angle = 0;
                    --flk_work.cycles;
                    flk_work.interval_timer = flk_work.interval_flk;
                }
            }
            GmBsCmnSetObject3DNNFadedColor(obj_work, flk_work.color, (float)((1.0 - nnCos(flk_work.cur_angle)) / 2.0));
            return false;
        }
        if (flk_work.is_active)
            GmBoss4UtilEndFlicker(obj_work, flk_work);
        return true;
    }

    private static void GmBoss4UtilEndFlicker(
      OBS_OBJECT_WORK obj_work,
      GMS_BOSS4_FLICKER_WORK flk_work)
    {
        MTM_ASSERT(obj_work);
        MTM_ASSERT(obj_work.obj_3d);
        MTM_ASSERT(flk_work);
        flk_work.Clear();
        GmBsCmnClearObject3DNNFadedColor(obj_work);
    }

    private static void GmBoss4UtilRotateVecFx32(ref VecFx32 f, int angle)
    {
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
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
        nnMakeRotateZMatrix(nnsMatrix, angle);
        nnTranslateMatrix(nnsMatrix, nnsMatrix, FX_FX32_TO_F32(f.x), FX_FX32_TO_F32(f.y), FX_FX32_TO_F32(f.z));
        f.x = FX_F32_TO_FX32(nnsMatrix.M03);
        f.y = FX_F32_TO_FX32(nnsMatrix.M13);
        f.z = FX_F32_TO_FX32(nnsMatrix.M23);
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void GmBoss4UtilIterateDamageRingInit()
    {
        gm_boss4_util_ring = GmRingGetWork().damage_ring_list_start;
    }

    private static GMS_RING_WORK GmBoss4UtilIterateDamageRingGet()
    {
        GMS_RING_WORK gmBoss4UtilRing = gm_boss4_util_ring;
        if (gmBoss4UtilRing == null)
            return null;
        gm_boss4_util_ring = gmBoss4UtilRing.post_ring;
        return gmBoss4UtilRing;
    }

    private static void GmBoss4UtilSetPlayerAttackReaction(
      OBS_OBJECT_WORK player,
      OBS_OBJECT_WORK enemy)
    {
        UNREFERENCED_PARAMETER(enemy);
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)player;
        OBS_OBJECT_WORK bodyWork = GmBoss4GetBodyWork();
        if (((int)ply_work.obj_work.move_flag & 16) != 0)
        {
            GmPlySeqAtkReactionInit(ply_work);
            GmPlySeqSetJumpState(ply_work, 0, 5U);
            ply_work.obj_work.spd_m = 0;
            if (bodyWork != null)
            {
                if (ply_work.obj_work.pos.x < bodyWork.pos.x)
                {
                    ply_work.obj_work.spd.x = -FX_F32_TO_FX32(5f);
                    if (GmBoss4GetScrollOffset() != 0)
                        ply_work.obj_work.spd.x = FX_F32_TO_FX32(3f);
                }
                else
                    ply_work.obj_work.spd.x = FX_F32_TO_FX32(5f);
            }
            else
            {
                if (ply_work.obj_work.move.x >= 0)
                {
                    ply_work.obj_work.spd.x = -FX_F32_TO_FX32(5f);
                    if (GmBoss4GetScrollOffset() != 0)
                        ply_work.obj_work.spd.x = FX_F32_TO_FX32(3f);
                }
                else
                    ply_work.obj_work.spd.x = FX_F32_TO_FX32(5f);
                int seqState = ply_work.seq_state;
            }
            ply_work.obj_work.spd.y = -FX_F32_TO_FX32(4f);
            GmPlySeqSetNoJumpMoveTime(ply_work, 102400);
        }
        else
        {
            ply_work.obj_work.disp_flag ^= 1U;
            GmPlySeqChangeSequence(ply_work, 10);
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
        return g_gm_main_system.map_fcol.left != 0 || g_gm_main_system.map_fcol.right != g_gm_main_system.map_fcol.map_block_num_x * 64;
    }

    private static void GmBoss4UtilInitNoHitTimer(
      GMS_BOSS4_NOHIT_TIMER work,
      GMS_ENEMY_COM_WORK ene_com,
      int time)
    {
        work.ene_com = ene_com;
        work.timer = (uint)(time + 1);
        GmBoss4UtilUpdateNoHitTimer(work);
    }

    private static bool GmBoss4UtilUpdateNoHitTimer(GMS_BOSS4_NOHIT_TIMER work)
    {
        GMS_ENEMY_COM_WORK eneCom = work.ene_com;
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