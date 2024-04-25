public partial class AppMain
{
    public static float GMD_BOSS4_CAP_ROTATE_SPD => GMM_BOSS4_PAL_SPEED(6f);

    public static int GMD_BOSS4_CAP_ZOOM_TIME => GMM_BOSS4_PAL_TIME(270f);

    public static int GMD_BOSS4_CAP_ZOOM_TIME_RAND => GMM_BOSS4_PAL_TIME(60f);

    public static void T_FUNC(MPP_VOID_OBS_OBJECT_WORK func, OBS_OBJECT_WORK w)
    {
        w.ppFunc = func;
    }

    public static void SET_FLAG(uint f, GMS_BOSS4_CAP_WORK w)
    {
        w.flag |= f;
    }

    public static void RESET_FLAG(uint f, GMS_BOSS4_CAP_WORK w)
    {
        w.flag &= ~f;
    }

    public static bool IS_FLAG(uint f, GMS_BOSS4_CAP_WORK w)
    {
        return 0 != ((int)w.flag & (int)f);
    }

    private static void GmBoss4CapsuleBuild()
    {
        _cap_no = 0;
        _cap_count = 0;
        _cap_inv_flag = 0;
        _cap_inv_hit = true;
        _cap_kill_flag = 0;
    }

    private static void GmBoss4CapsuleFlush()
    {
    }

    private static OBS_OBJECT_WORK GmBoss4CapsuleInit1st(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        return GmBoss4CapsuleInit(eve_rec, pos_x, pos_y, 0);
    }

    private static OBS_OBJECT_WORK GmBoss4CapsuleInit2nd(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        return GmBoss4CapsuleInit(eve_rec, pos_x, pos_y, 1);
    }

    private static void GmBoss4CapsuleSetInvincible(int inv)
    {
        GmBoss4CapsuleSetInvincible(inv, true);
    }

    private static void GmBoss4CapsuleSetInvincible(int count, bool hit)
    {
        _cap_inv_flag = count;
        _cap_inv_hit = hit;
    }

    private static int GmBoss4CapsuleGetCount()
    {
        return _cap_count;
    }

    private static void GmBoss4CapsuleClear()
    {
        _cap_count = 0;
    }

    private static void GmBoss4CapsuleExplosion()
    {
        _cap_kill_flag = 1;
    }

    private static OBS_OBJECT_WORK GmBoss4CapsuleInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        _cap_kill_flag = 0;
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS4_CAP_WORK(), "BOSS4_CAP");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS4_CAP_WORK gmsBosS4CapWork = (GMS_BOSS4_CAP_WORK)work;
        gmsBosS4CapWork.cap_no = _cap_no++ % 6;
        gmsBosS4CapWork.type = type;
        work.move_flag |= 256U;
        ObjObjectCopyAction3dNNModel(work, GmBoss4GetObj3D(2), gmsEnemy3DWork.obj_3d);
        ObjDrawObjectSetToon(work);
        work.disp_flag |= 134217728U;
        work.flag |= 16U;
        work.disp_flag |= 4194304U;
        ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[0], -14, -30, 14, -2);
        gmsEnemy3DWork.ene_com.rect_work[0].ppDef = new OBS_RECT_WORK_Delegate1(gmBoss4CapsuleDamageDefFunc);
        ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[1], -1, -9, 1, -7);
        gmsEnemy3DWork.ene_com.rect_work[1].ppHit = new OBS_RECT_WORK_Delegate1(gmBoss4CapsuleAtkHitFunc);
        gmsEnemy3DWork.ene_com.rect_work[1].flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        T_FUNC(new MPP_VOID_OBS_OBJECT_WORK(gmBoss4CapsuleWaitLoad), work);
        if (gmsBosS4CapWork.chibi_type == 4)
            work.disp_flag |= 32U;
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss4CapsuleExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void GmBoss4CapsuleUpdateRol(float spd)
    {
        _cap_rot_y += AKM_DEGtoA32(spd);
        _cap_rot_y %= AKM_DEGtoA32(360);
        if (_cap_rot_z_flag != 0)
        {
            _cap_rot_z += AKM_DEGtoA32(1f);
            if (_cap_rot_z >= AKM_DEGtoA32(45f))
                _cap_rot_z_flag = 0;
        }
        else
        {
            _cap_rot_z -= AKM_DEGtoA32(1f);
            if (_cap_rot_z <= AKM_DEGtoA32(-45f))
                _cap_rot_z_flag = 1;
        }
        if (_cap_rot_x_flag != 0)
        {
            _cap_rot_x += AKM_DEGtoA32(0.5f);
            if (_cap_rot_x >= AKM_DEGtoA32(60f))
                _cap_rot_x_flag = 0;
        }
        else
        {
            _cap_rot_x -= AKM_DEGtoA32(0.5f);
            if (_cap_rot_x <= AKM_DEGtoA32(-60f))
                _cap_rot_x_flag = 1;
        }
        if (_cap_len_time > 0.0)
            --_cap_len_time;
        else if (0.0 != _cap_len_flag)
        {
            _cap_len += 2f;
            if (_cap_len >= 100.0)
                _cap_len_flag = 0.0f;
        }
        else
        {
            _cap_len -= 2f;
            if (_cap_len <= 65.0)
            {
                _cap_len_flag = 1f;
                _cap_len_time = GMD_BOSS4_CAP_ZOOM_TIME + GMD_BOSS4_CAP_ZOOM_TIME_RAND * (random.Next() % 256 / 256f);
            }
        }
        if (_cap_inv_flag > 900)
            _cap_inv_flag = 0;
        if (_cap_inv_flag > 0)
            --_cap_inv_flag;
        else
            _cap_inv_flag = 0;
    }

    private static void gmBoss4CapsuleWaitLoad(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_CAP_WORK gmsBosS4CapWork = (GMS_BOSS4_CAP_WORK)obj_work;
        if (!GmBoss4IsBuilded())
            return;
        if (gmsBosS4CapWork.type == 0)
        {
            T_FUNC(new MPP_VOID_OBS_OBJECT_WORK(gmBoss4CapsuleMain), obj_work);
        }
        else
        {
            obj_work.move_flag &= 4294963199U;
            obj_work.move_flag |= 128U;
            ObjObjectFieldRectSet(obj_work, -20, -40, 20, 0);
            obj_work.dir.y = 0;
            gmsBosS4CapWork.chibi_type = gmBoss4ChibiGetAttackType(GmBoss4GetLife());
            T_FUNC(new MPP_VOID_OBS_OBJECT_WORK(gmBoss4CapsuleMain2nd), obj_work);
            if (gmsBosS4CapWork.chibi_type == 4)
            {
                OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(329, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
                obsObjectWork.spd.x = FX_F32_TO_FX32(2f);
                obsObjectWork.spd.y = FX_F32_TO_FX32(-3f);
                GmBoss4IncObjCreateCount();
                obsObjectWork.parent_obj = obj_work.parent_obj;
                GMM_BS_OBJ(gmsBosS4CapWork).flag |= 8U;
            }
        }
        ++_cap_count;
        gmsBosS4CapWork.wait = 0;
    }

    private static void gmBoss4CapsuleExit(MTS_TASK_TCB tcb)
    {
        GmBoss4DecObjCreateCount();
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss4CapsuleMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK parentObj = (GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS4_CAP_WORK w = (GMS_BOSS4_CAP_WORK)obj_work;
        NNS_MATRIX nnsMatrix1 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX nnsMatrix2 = GlobalPool<NNS_MATRIX>.Alloc();
        NNS_MATRIX nnsMatrix3 = GlobalPool<NNS_MATRIX>.Alloc();
        if (w.wait > 0)
        {
            obj_work.pos.z = 131072;
            GmBoss4UtilUpdateFlicker(obj_work, w.flk_work);
            if (GmBoss4UtilUpdate1ShotTimer(w.timer))
            {
                VecFx32 pos = obj_work.pos;
                pos.z = 135168;
                GmBoss4EffCommonInit(735, new VecFx32?(pos));
                T_FUNC(new MPP_VOID_OBS_OBJECT_WORK(gmBoss4CapsuleBomb), obj_work);
            }
        }
        else
        {
            GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.node_work.snm_work, parentObj.node_work.work[2], 0);
            obj_work.pos.y += FX_F32_TO_FX32(20f);
            int ay = (_cap_rot_y + AKM_DEGtoA32(360) / 6 * w.cap_no) % AKM_DEGtoA32(360);
            nnMakeRotateXMatrix(nnsMatrix1, _cap_rot_x);
            nnRotateZMatrix(nnsMatrix1, nnsMatrix1, _cap_rot_z);
            nnRotateYMatrix(nnsMatrix1, nnsMatrix1, ay);
            nnMakeTranslateMatrix(nnsMatrix2, _cap_len, 0.0f, 0.0f);
            nnMultiplyMatrix(nnsMatrix3, nnsMatrix1, nnsMatrix2);
            NNS_VECTOR dst = GlobalPool<NNS_VECTOR>.Alloc();
            nnCopyMatrixTranslationVector(dst, nnsMatrix3);
            obj_work.pos.x += FX_F32_TO_FX32(dst.x);
            obj_work.pos.y += FX_F32_TO_FX32(dst.y);
            obj_work.pos.z += FX_F32_TO_FX32(dst.z);
            GlobalPool<NNS_VECTOR>.Release(dst);
        }
        if (_cap_kill_flag != 0)
        {
            VecFx32 pos = obj_work.pos;
            pos.z = 135168;
            GmBoss4EffCommonInit(735, new VecFx32?(pos));
            w.wait = 30;
            T_FUNC(new MPP_VOID_OBS_OBJECT_WORK(gmBoss4CapsuleBomb), obj_work);
            GlobalPool<NNS_MATRIX>.Release(nnsMatrix3);
            GlobalPool<NNS_MATRIX>.Release(nnsMatrix2);
            GlobalPool<NNS_MATRIX>.Release(nnsMatrix1);
        }
        else
        {
            GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
            if (_cap_inv_flag != 0)
            {
                if (!_cap_inv_hit)
                {
                    gmsEnemy3DWork.ene_com.rect_work[0].flag |= 2048U;
                    gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
                }
                gmsEnemy3DWork.ene_com.rect_work[1].flag |= 2048U;
            }
            else
            {
                gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294965247U;
                gmsEnemy3DWork.ene_com.rect_work[0].flag |= 4U;
                gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294965247U;
            }
            if (IS_FLAG(1073741824U, w))
            {
                --_cap_count;
                gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
                gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
                gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
                RESET_FLAG(1073741824U, w);
                GmBoss4UtilInitFlicker(obj_work, w.flk_work);
                GmBoss4UtilInit1ShotTimer(w.timer, 20U);
                w.wait = 60;
            }
            GlobalPool<NNS_MATRIX>.Release(nnsMatrix3);
            GlobalPool<NNS_MATRIX>.Release(nnsMatrix2);
            GlobalPool<NNS_MATRIX>.Release(nnsMatrix1);
        }
    }

    private static void gmBoss4CapsuleMain2nd(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_CAP_WORK gmsBosS4CapWork = (GMS_BOSS4_CAP_WORK)obj_work;
        obj_work.move_flag &= 4294443007U;
        obj_work.spd_fall = FX_F32_TO_FX32(0.2f);
        obj_work.move_flag |= 128U;
        obj_work.pos.x += GmBoss4GetScrollOffset();
        if (((int)obj_work.move_flag & 1) == 0)
            return;
        VecFx32 pos = obj_work.pos;
        pos.z = 135168;
        GmBoss4EffCommonInit(735, new VecFx32?(pos));
        gmsBosS4CapWork.wait = 30;
        obj_work.spd.x = FX_F32_TO_FX32(0.0f);
        obj_work.spd.y = FX_F32_TO_FX32(-1f);
        obj_work.move_flag &= 4294967294U;
        obj_work.move_flag |= 256U;
        ushort id;
        switch (gmsBosS4CapWork.chibi_type)
        {
            case 2:
                id = 327;
                break;
            case 3:
                id = 328;
                break;
            case 4:
                id = 329;
                break;
            default:
                id = 326;
                break;
        }
        OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(id, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
        obsObjectWork.parent_obj = obj_work.parent_obj;
        GmBoss4IncObjCreateCount();
        obsObjectWork.spd.y = FX_F32_TO_FX32(-4f);
        obsObjectWork.spd.x = FX_F32_TO_FX32(-1f);
        T_FUNC(new MPP_VOID_OBS_OBJECT_WORK(gmBoss4CapsuleBomb2nd), obj_work);
    }

    private static void gmBoss4CapsuleAtkHitFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        ((GMS_BOSS4_BODY_WORK)my_rect.parent_obj.parent_obj).flag[0] |= 268435456U;
        GmEnemyDefaultAtkFunc(my_rect, your_rect);
    }

    private static void gmBoss4CapsuleDamageDefFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        GMS_BOSS4_CAP_WORK w = (GMS_BOSS4_CAP_WORK)parentObj1;
        GMS_BOSS4_BODY_WORK parentObj3 = (GMS_BOSS4_BODY_WORK)parentObj1.parent_obj;
        if (parentObj2 == null || 1 != parentObj2.obj_type || _cap_inv_flag > 0)
            return;
        GmBoss4UtilSetPlayerAttackReaction(parentObj2, parentObj1);
        GmSoundPlaySE("Enemy");
        GmBoss4CapsuleSetInvincible(30);
        GmBoss4UtilInitNoHitTimer(parentObj3.nohit_work, (GMS_ENEMY_COM_WORK)parentObj3, 25);
        if (IS_FLAG(4U, w))
            return;
        SET_FLAG(1073741824U, w);
        if (((int)parentObj3.flag[0] & 4096) != 0)
            return;
        parentObj3.flag[0] |= 2048U;
        parentObj3.avoid_timer = 90;
    }

    private static void gmBoss4CapsuleBomb(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_CAP_WORK gmsBosS4CapWork = (GMS_BOSS4_CAP_WORK)obj_work;
        obj_work.disp_flag |= 32U;
        if (gmsBosS4CapWork.wait > 0)
        {
            --gmsBosS4CapWork.wait;
            if (gmsBosS4CapWork.wait != 30 || _cap_kill_flag != 0)
                return;
            OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(325, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
            GmBoss4IncObjCreateCount();
            obsObjectWork.parent_obj = obj_work.parent_obj;
        }
        else
            GMM_BS_OBJ(gmsBosS4CapWork).flag |= 8U;
    }

    private static void gmBoss4CapsuleBomb2nd(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_CAP_WORK gmsBosS4CapWork = (GMS_BOSS4_CAP_WORK)obj_work;
        obj_work.disp_flag &= 4294963199U;
        if (((int)g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        else
            obj_work.pos.x += GmBoss4GetScrollOffset();
        if (gmsBosS4CapWork.wait > 0)
        {
            --gmsBosS4CapWork.wait;
            if (gmsBosS4CapWork.wait >= 36)
                return;
            obj_work.disp_flag |= 32U;
        }
        else
            GMM_BS_OBJ(gmsBosS4CapWork).flag |= 8U;
    }


}