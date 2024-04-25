public partial class AppMain
{
    public static int GME_BOSS4_LIFE_H => GMM_BOSS4_STAGE(3, 4);

    public static int GME_BOSS4_LIFE_L => GMM_BOSS4_STAGE(1, 2);

    public static uint GMD_BOSS4_CHIBI_LIFE_TIME => (uint)GMM_BOSS4_PAL_TIME(300f);

    public static float GMD_BOSS4_CHIBI_HIT_CHIBI_ADDSPD => GMM_BOSS4_PAL_SPEED(1f);

    public static float GMD_BOSS4_CHIBI_SPD_LIMIT => GMM_BOSS4_PAL_SPEED(2f);

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_Y_NORMAL => GMM_BOSS4_PAL_SPEED(-4.5f);

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_Y_BOUND => GMM_BOSS4_PAL_SPEED(-4.5f);

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_Y_SPEED => GMM_BOSS4_PAL_SPEED(-4.5f);

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_Y_BIG => GMM_BOSS4_PAL_SPEED(-3.5f);

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_Y_IRON => GMM_BOSS4_PAL_SPEED(-2f);

    public static float GMD_BOSS4_CHIBI_FALL_SPD_NORMAL => GMM_BOSS4_PAL_SPEED(0.1f);

    public static float GMD_BOSS4_CHIBI_FALL_SPD_BOUND => GMM_BOSS4_PAL_SPEED(0.1f);

    public static float GMD_BOSS4_CHIBI_FALL_SPD_SPEED => GMM_BOSS4_PAL_SPEED(0.2f);

    public static float GMD_BOSS4_CHIBI_FALL_SPD_BIG => GMM_BOSS4_PAL_SPEED(0.1f);

    public static float GMD_BOSS4_CHIBI_FALL_SPD_IRON => GMM_BOSS4_PAL_SPEED(0.18f);

    public static int GMD_BOSS4_CHIBI_BOUND_FRAME => GMM_BOSS4_PAL_TIME(10f);

    public static float GMD_BOSS4_CHIBI_BOUND_MULTI_SPD_X => GMM_BOSS4_PAL_SPEED(0.8f);

    public static float GMD_BOSS4_CHIBI_BOUND_ADD_SPD_X => GMM_BOSS4_PAL_SPEED(1.5f);

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_X_BOUND => GMM_BOSS4_PAL_SPEED(-1.2f);

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_X_SPEED => GMM_BOSS4_PAL_SPEED(-2.5f);

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_X_BIG => GMM_BOSS4_PAL_SPEED(-2f);

    public static float GMD_BOSS4_CHIBI_BOUND_SPD_X_IRON => GMM_BOSS4_PAL_SPEED(-2f);

    public static float GMD_BOSS4_CHIBI_BOUND_OUT_Y => GMM_BOSS4_PAL_ZOOM(1.04f);

    public static float GMD_BOSS4_CHIBI_BOUND_OUT_X => GMM_BOSS4_PAL_ZOOM(0.95f);

    public static float GMD_BOSS4_CHIBI_BOUND_IN_Y => GMM_BOSS4_PAL_ZOOM(0.9f);

    public static float GMD_BOSS4_CHIBI_BOUND_IN_X => GMM_BOSS4_PAL_ZOOM(1.15f);

    public static float GMD_BOSS4_CHIBI_EXPLOSION_BASE_HIDE_TIME => GMM_BOSS4_PAL_TIME(2f);

    public static float GMD_BOSS4_CHIBI_EXPLOSION_TASK_KILL_TIME => GMM_BOSS4_PAL_TIME(60f);

    public static void SET_FLAG(uint f, GMS_BOSS4_CHIBI_WORK w)
    {
        w.flag |= f;
    }

    public static void RESET_FLAG(uint f, GMS_BOSS4_CHIBI_WORK w)
    {
        w.flag &= ~f;
    }

    public static bool IS_FLAG(uint f, GMS_BOSS4_CHIBI_WORK w)
    {
        return 0 != ((int)w.flag & (int)f);
    }

    private static int gmBoss4ChibiGetAttackType(int life)
    {
        UNREFERENCED_PARAMETER(life);
        gmBoss4ChibiGetAttackTypeStatics._index %= 20;
        int num;
        if (GmBoss4GetLife() >= GME_BOSS4_LIFE_H)
        {
            if (!GmBoss4CheckBossRush())
                return 1;
            num = gmBoss4ChibiGetAttackTypeStatics.life3_tbl_f[gmBoss4ChibiGetAttackTypeStatics._index];
        }
        else
            num = !GmBoss4CheckBossRush() ? (GmBoss4GetLife() >= GME_BOSS4_LIFE_H || GmBoss4GetLife() <= GME_BOSS4_LIFE_L ? gmBoss4ChibiGetAttackTypeStatics.life1_tbl[gmBoss4ChibiGetAttackTypeStatics._index] : gmBoss4ChibiGetAttackTypeStatics.life2_tbl[gmBoss4ChibiGetAttackTypeStatics._index]) : (GmBoss4GetLife() >= GME_BOSS4_LIFE_H || GmBoss4GetLife() <= GME_BOSS4_LIFE_L ? gmBoss4ChibiGetAttackTypeStatics.life1_tbl_f[gmBoss4ChibiGetAttackTypeStatics._index] : gmBoss4ChibiGetAttackTypeStatics.life2_tbl_f[gmBoss4ChibiGetAttackTypeStatics._index]);
        ++gmBoss4ChibiGetAttackTypeStatics._index;
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
        return gmBoss4ChibiGetThrowType(1);
    }

    private static int gmBoss4ChibiGetThrowType(int t)
    {
        gmBoss4ChibiGetThrowTypeStatics._index %= 20;
        int num = gmBoss4ChibiGetThrowTypeStatics._tbl[gmBoss4ChibiGetThrowTypeStatics._index];
        ++gmBoss4ChibiGetThrowTypeStatics._index;
        return num;
    }

    private static void GmBoss4ChibiBuild()
    {
        ObjDataLoadAmbIndex(ObjDataGet(732), 4, GMD_BOSS4_ARC);
    }

    private static void GmBoss4ChibiFlush()
    {
        ObjDataRelease(ObjDataGet(732));
    }

    private static OBS_OBJECT_WORK GmBoss4ChibiInit1st(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        return GmBoss4ChibiInit(eve_rec, pos_x, pos_y + 65536, 0);
    }

    private static OBS_OBJECT_WORK GmBoss4ChibiInit2nd(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        return GmBoss4ChibiInit(eve_rec, pos_x, pos_y + 65536, 1);
    }

    private static OBS_OBJECT_WORK GmBoss4ChibiInit2ndSpeed(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        return GmBoss4ChibiInit(eve_rec, pos_x, pos_y + 65536, 2);
    }

    private static OBS_OBJECT_WORK GmBoss4ChibiInit2ndBig(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        return GmBoss4ChibiInit(eve_rec, pos_x, pos_y + 65536, 3);
    }

    private static OBS_OBJECT_WORK GmBoss4ChibiInit2ndIron(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        UNREFERENCED_PARAMETER(type);
        return GmBoss4ChibiInit(eve_rec, pos_x, pos_y + 65536, 4);
    }

    private static void GmBoss4ChibiSetInvincible(bool flg)
    {
        gm_chibi_inv_flag = flg;
    }

    private static void GmBoss4ChibiExplosion()
    {
        gm_chibi_exp_flag = true;
    }

    private static OBS_OBJECT_WORK GmBoss4ChibiInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS4_CHIBI_WORK(), "BOSS4_C.E");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS4_CHIBI_WORK w = (GMS_BOSS4_CHIBI_WORK)work;
        w.type = type;
        switch (w.type)
        {
            case 2:
                ObjObjectCopyAction3dNNModel(work, GmBoss4GetObj3D(4), gmsEnemy3DWork.obj_3d);
                break;
            case 3:
                ObjObjectCopyAction3dNNModel(work, GmBoss4GetObj3D(5), gmsEnemy3DWork.obj_3d);
                break;
            case 4:
                ObjObjectCopyAction3dNNModel(work, GmBoss4GetObj3D(6), gmsEnemy3DWork.obj_3d);
                break;
            default:
                ObjObjectCopyAction3dNNModel(work, GmBoss4GetObj3D(3), gmsEnemy3DWork.obj_3d);
                break;
        }
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(732), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.disp_flag |= 134217728U;
        work.flag |= 16U;
        work.disp_flag |= 4194304U;
        work.move_flag &= 4294963199U;
        work.move_flag |= 128U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        ObjObjectFieldRectSet(work, -20, -44, 20, -4);
        T_FUNC(new MPP_VOID_OBS_OBJECT_WORK(gmBoss4ChibiWaitLoad), work);
        gm_chibi_exp_flag = false;
        RESET_FLAG(536870912U, w);
        if (w.type != 0)
            SET_FLAG(536870912U, w);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss4ChibiExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss4ChibiAtkHitFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        GMS_BOSS4_BODY_WORK parentObj1 = (GMS_BOSS4_BODY_WORK)my_rect.parent_obj.parent_obj;
        GMS_BOSS4_CHIBI_WORK parentObj2 = (GMS_BOSS4_CHIBI_WORK)my_rect.parent_obj;
        parentObj1.flag[0] |= 268435456U;
        GmEnemyDefaultAtkFunc(my_rect, your_rect);
        if (parentObj2.type != 0)
        {
            GMS_PLAYER_WORK playerObj = (GMS_PLAYER_WORK)GmBsCmnGetPlayerObj();
            if (GmPlayerKeyCheckWalkLeft(playerObj))
                GmPlayerSetReverse(playerObj);
        }
        SET_FLAG(1073741824U, parentObj2);
    }

    private static void gmBoss4ChibiDefHitFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        GMS_BOSS4_CHIBI_WORK parentObj1 = (GMS_BOSS4_CHIBI_WORK)my_rect.parent_obj;
        GMS_BOSS4_CHIBI_WORK parentObj2 = (GMS_BOSS4_CHIBI_WORK)your_rect.parent_obj;
        OBS_OBJECT_WORK obsObjectWork1 = (OBS_OBJECT_WORK)parentObj1;
        OBS_OBJECT_WORK obsObjectWork2 = (OBS_OBJECT_WORK)parentObj2;
        if (!IS_FLAG(536870912U, parentObj1) || !IS_FLAG(536870912U, parentObj2))
            return;
        if (obsObjectWork1.pos.x > obsObjectWork2.pos.x)
        {
            obsObjectWork1.pos.x += FX_F32_TO_FX32(4f);
            obsObjectWork1.spd.x += FX_F32_TO_FX32(GMD_BOSS4_CHIBI_HIT_CHIBI_ADDSPD);
        }
        if (obsObjectWork1.pos.x < obsObjectWork2.pos.x)
        {
            obsObjectWork1.pos.x -= FX_F32_TO_FX32(4f);
            obsObjectWork1.spd.x -= FX_F32_TO_FX32(GMD_BOSS4_CHIBI_HIT_CHIBI_ADDSPD);
        }
        if (obsObjectWork1.pos.x == obsObjectWork2.pos.x)
        {
            // BUGBUG
            if (obsObjectWork1.pos.y < obsObjectWork2.pos.y)
            {
                obsObjectWork1.pos.x += FX_F32_TO_FX32(4f);
                obsObjectWork1.spd.x += FX_F32_TO_FX32(GMD_BOSS4_CHIBI_HIT_CHIBI_ADDSPD);
            }
            else
            {
                obsObjectWork1.pos.x -= FX_F32_TO_FX32(4f);
                obsObjectWork1.spd.x -= FX_F32_TO_FX32(GMD_BOSS4_CHIBI_HIT_CHIBI_ADDSPD);
            }
        }
        if (obsObjectWork1.spd.x > FX_F32_TO_FX32(GMD_BOSS4_CHIBI_SPD_LIMIT))
            obsObjectWork1.spd.x = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_SPD_LIMIT);
        if (obsObjectWork1.spd.x >= -FX_F32_TO_FX32(GMD_BOSS4_CHIBI_SPD_LIMIT))
            return;
        obsObjectWork1.spd.x = -FX_F32_TO_FX32(GMD_BOSS4_CHIBI_SPD_LIMIT);
    }

    private static void gmBoss4ChibiWaitLoad(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_CHIBI_WORK chibi = (GMS_BOSS4_CHIBI_WORK)obj_work;
        if (!GmBoss4IsBuilded())
            return;
        T_FUNC(new MPP_VOID_OBS_OBJECT_WORK(gmBoss4ChibiMain), obj_work);
        GmBoss4UtilInit1ShotTimer(chibi.timer, GMD_BOSS4_CHIBI_LIFE_TIME);
        GmBoss4UtilInitFlicker(obj_work, chibi.flk_work, 1, 180, 4, (int)(chibi.timer.timer / 20U) * 3, gm_boss4_color_red);
        chibi.count = -1;
        switch (chibi.type)
        {
            case 1:
                GmBsCmnSetAction(obj_work, 0, 0);
                obj_work.spd.y = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_Y_BOUND);
                break;
            case 2:
                GmBsCmnSetAction(obj_work, 0, 0);
                obj_work.spd.y = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_Y_SPEED);
                break;
            case 3:
                GmBsCmnSetAction(obj_work, 0, 0);
                obj_work.spd.y = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_Y_BIG);
                break;
            case 4:
                GmBsCmnSetAction(obj_work, 0, 0);
                obj_work.spd.y = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_Y_IRON);
                break;
            default:
                GmBsCmnSetAction(obj_work, 0, 0);
                obj_work.spd.y = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_Y_NORMAL);
                break;
        }
        obj_work.spd_fall = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_FALL_SPD_NORMAL);
        obj_work.pos.z = 131072;
        chibi.bound = 0;
        GmBoss4UtilInitNodeMatrix(chibi.node_work, obj_work, 1);
        GmBoss4UtilGetNodeMatrix(chibi.node_work, 0);
        gmBoss4ChibiBoosterCreate(chibi);
        GmSoundPlaySE("Boss4_01");
    }

    private static void gmBoss4ChibiExit(MTS_TASK_TCB tcb)
    {
        GMS_BOSS4_CHIBI_WORK tcbWork = (GMS_BOSS4_CHIBI_WORK)mtTaskGetTcbWork(tcb);
        GmBoss4DecObjCreateCount();
        GmBoss4UtilExitNodeMatrix(tcbWork.node_work);
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss4ChibiMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_CHIBI_WORK w = (GMS_BOSS4_CHIBI_WORK)obj_work;
        if (GmBsCmnIsActionEnd(obj_work) != 0)
        {
            GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
            ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[1], -10, -22, 10, -2);
            gmsEnemy3DWork.ene_com.rect_work[1].ppHit = new OBS_RECT_WORK_Delegate1(gmBoss4ChibiAtkHitFunc);
            gmsEnemy3DWork.ene_com.rect_work[1].flag |= 33554432U;
            ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[2], -14, -26, 18, 2);
            gmsEnemy3DWork.ene_com.rect_work[2].ppHit = new OBS_RECT_WORK_Delegate1(gmBoss4ChibiDefHitFunc);
            ObjRectGroupSet(gmsEnemy3DWork.ene_com.rect_work[2], 2, 4);
            gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294965247U;
            gmsEnemy3DWork.ene_com.rect_work[2].flag |= 4U;
            switch (w.type)
            {
                case 3:
                    ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[1], -30, -60, 30, 0);
                    ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[2], -44, -64, 44, 4);
                    break;
            }
        }
        switch (w.type)
        {
            case 1:
                obj_work.move_flag &= 4294443007U;
                obj_work.spd_fall = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_FALL_SPD_BOUND);
                obj_work.pos.x += GmBoss4GetScrollOffset();
                break;
            case 2:
                obj_work.move_flag &= 4294443007U;
                obj_work.spd_fall = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_FALL_SPD_SPEED);
                obj_work.pos.x += GmBoss4GetScrollOffset();
                if (w.bound == 0)
                {
                    obj_work.pos.y += obj_work.spd.y;
                    break;
                }
                break;
            case 3:
                obj_work.move_flag &= 4294443007U;
                obj_work.spd_fall = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_FALL_SPD_BIG);
                obj_work.pos.x += GmBoss4GetScrollOffset();
                break;
            case 4:
                obj_work.move_flag &= 4294443007U;
                obj_work.spd_fall = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_FALL_SPD_IRON);
                obj_work.pos.x += GmBoss4GetScrollOffset();
                obj_work.pos.x -= FX_F32_TO_FX32(4f);
                break;
            default:
                obj_work.spd_fall = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_FALL_SPD_NORMAL);
                break;
        }
        if (obj_work.scale.y < FX_F32_TO_FX32(1f))
        {
            obj_work.scale.y = (int)(obj_work.scale.y * (double)GMD_BOSS4_CHIBI_BOUND_OUT_Y);
            if (obj_work.scale.y > FX_F32_TO_FX32(1f))
                obj_work.scale.y = FX_F32_TO_FX32(1f);
        }
        if (obj_work.scale.x > FX_F32_TO_FX32(1f))
        {
            obj_work.scale.x = (int)(obj_work.scale.x * (double)GMD_BOSS4_CHIBI_BOUND_OUT_X);
            if (obj_work.scale.x < FX_F32_TO_FX32(1f))
                obj_work.scale.x = FX_F32_TO_FX32(1f);
        }
        if (gm_chibi_exp_flag)
            SET_FLAG(1073741824U, w);
        GMS_ENEMY_3D_WORK gmsEnemy3DWork1 = (GMS_ENEMY_3D_WORK)obj_work;
        if (gm_chibi_inv_flag)
            gmsEnemy3DWork1.ene_com.rect_work[1].flag |= 2048U;
        else
            gmsEnemy3DWork1.ene_com.rect_work[1].flag &= 4294965247U;
        if (((int)obj_work.move_flag & 1) != 0)
        {
            SET_FLAG(536870912U, w);
            obj_work.move_flag &= 4294967294U;
            obj_work.move_flag &= 4294967167U;
            obj_work.move_flag |= 256U;
            w.bnd_xspd = obj_work.spd.x;
            obj_work.spd.y = 0;
            obj_work.spd.x = 0;
            obj_work.spd_fall = 0;
            w.bound = w.type != 4 ? GMD_BOSS4_CHIBI_BOUND_FRAME : 1000;
            if (w.type == 3)
                GmSoundPlaySE("Boss4_03");
            else if (w.type != 4)
                GmSoundPlaySE("Boss4_02");
        }
        if (w.bound > 0)
        {
            if (--w.bound == 0)
            {
                obj_work.pos.y += FX_F32_TO_FX32(-4f);
                obj_work.move_flag |= 128U;
                obj_work.move_flag &= 4294967039U;
                obj_work.move_flag &= 4294967294U;
                obj_work.spd.x = w.bnd_xspd;
                switch (w.type)
                {
                    case 1:
                        obj_work.spd.x = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_X_BOUND);
                        obj_work.spd.y = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_Y_BOUND);
                        break;
                    case 2:
                        obj_work.spd.x = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_X_SPEED);
                        obj_work.spd.y = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_Y_SPEED);
                        break;
                    case 3:
                        obj_work.spd.x = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_X_BIG);
                        obj_work.spd.y = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_Y_BIG);
                        break;
                    case 4:
                        obj_work.spd.x = FX_F32_TO_FX32(0.0);
                        obj_work.spd.y = FX_F32_TO_FX32(0.0);
                        break;
                    default:
                        obj_work.spd.y = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_SPD_Y_NORMAL);
                        obj_work.spd.x = GmBsCmnGetPlayerObj().pos.x >= obj_work.pos.x ? (int)(obj_work.spd.x * (double)GMD_BOSS4_CHIBI_BOUND_MULTI_SPD_X) + FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_ADD_SPD_X) : (int)(obj_work.spd.x * (double)GMD_BOSS4_CHIBI_BOUND_MULTI_SPD_X) - FX_F32_TO_FX32(GMD_BOSS4_CHIBI_BOUND_ADD_SPD_X);
                        if (obj_work.spd.x > FX_F32_TO_FX32(GMD_BOSS4_CHIBI_SPD_LIMIT))
                            obj_work.spd.x = FX_F32_TO_FX32(GMD_BOSS4_CHIBI_SPD_LIMIT);
                        if (obj_work.spd.x < -FX_F32_TO_FX32(GMD_BOSS4_CHIBI_SPD_LIMIT))
                        {
                            obj_work.spd.x = -FX_F32_TO_FX32(GMD_BOSS4_CHIBI_SPD_LIMIT);
                            break;
                        }
                        break;
                }
            }
            else if (w.type != 4)
            {
                obj_work.scale.y = (int)(obj_work.scale.y * (double)GMD_BOSS4_CHIBI_BOUND_IN_Y);
                obj_work.scale.x = (int)(obj_work.scale.x * (double)GMD_BOSS4_CHIBI_BOUND_IN_X);
                obj_work.spd_fall = 0;
                if (obj_work.scale.y < FX_F32_TO_FX32(0.6f))
                    obj_work.scale.y = FX_F32_TO_FX32(0.6f);
                if (obj_work.scale.x > FX_F32_TO_FX32(1.5f))
                    obj_work.scale.x = FX_F32_TO_FX32(1.5f);
            }
        }
        if (w.type != 4)
            GmBoss4UtilLookAtPlayer(w.dir, obj_work, 5);
        if (GmBoss4UtilUpdate1ShotTimer(w.timer))
            SET_FLAG(1073741824U, w);
        if (w.type == 0 && GmBoss4UtilUpdateFlicker(obj_work, w.flk_work))
        {
            int start = (int)(w.timer.timer / 20U) * 3;
            GmBoss4UtilInitFlicker(obj_work, w.flk_work, 1, start, 2, 0, gm_boss4_color_red);
        }
        if (IS_FLAG(1073741824U, w))
        {
            RESET_FLAG(1073741824U, w);
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
            VecFx32 pos = obj_work.pos;
            pos.y += FX_F32_TO_FX32(-16f);
            pos.z = 135168;
            GmBoss4EffCommonInit(id, new VecFx32?(pos));
            GMS_ENEMY_3D_WORK gmsEnemy3DWork2 = (GMS_ENEMY_3D_WORK)obj_work;
            gmsEnemy3DWork2.ene_com.rect_work[1].flag &= 4294967291U;
            gmsEnemy3DWork2.ene_com.rect_work[2].flag &= 4294967291U;
            obj_work.spd_fall = 0;
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
            obj_work.move_flag &= 4294967294U;
            obj_work.move_flag |= 256U;
            obj_work.move_flag |= 256U;
            w.wait = 0;
            T_FUNC(new MPP_VOID_OBS_OBJECT_WORK(gmBoss4ChibiBomb), obj_work);
            GmSoundPlaySE("Boss4_04");
        }
        GmBoss4UtilUpdateDirection(w.dir, obj_work, true);
    }

    private static void gmBoss4ChibiBomb(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_CHIBI_WORK gmsBosS4ChibiWork = (GMS_BOSS4_CHIBI_WORK)obj_work;
        obj_work.pos.x += GmBoss4GetScrollOffset();
        ++gmsBosS4ChibiWork.wait;
        if (gmsBosS4ChibiWork.wait >= 2)
            obj_work.disp_flag |= 32U;
        if (gmsBosS4ChibiWork.wait < 60)
            return;
        GMM_BS_OBJ(gmsBosS4ChibiWork).flag |= 8U;
    }

    private static void gmBoss4ChibiFuncBoost(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_CHIBI_WORK parentObj1 = (GMS_BOSS4_CHIBI_WORK)obj_work.parent_obj;
        OBS_OBJECT_WORK parentObj2 = obj_work.parent_obj;
        MTM_ASSERT(parentObj1.node_work.snm_work.reg_node_max);
        if (((int)obj_work.disp_flag & 8) != 0)
            obj_work.flag |= 4U;
        if (((int)parentObj2.disp_flag & 32) != 0)
            gmBoss4ChibiBoosterDelete(parentObj1);
        obj_work.disp_flag &= 4294967263U;
        if (parentObj1.dir.cur_angle < AKM_DEGtoA16(50) && parentObj1.dir.cur_angle > AKM_DEGtoA16(-50))
            obj_work.disp_flag |= 32U;
        GmBsCmnUpdateObject3DESStuckWithNode(obj_work, parentObj1.node_work.snm_work, parentObj1.node_work.work[0], 1);
        obj_work.disp_flag &= 4294963199U;
        if (((int)g_obj.flag & 1) != 0)
        {
            obj_work.disp_flag |= 4096U;
        }
        else
        {
            obj_work.pos.x += parentObj2.spd.x;
            obj_work.pos.y += parentObj2.spd.y;
            obj_work.pos.x += GmBoss4GetScrollOffset();
        }
    }

    private static void gmBoss4ChibiBoosterCreate(GMS_BOSS4_CHIBI_WORK chibi)
    {
    }

    private static void gmBoss4ChibiBoosterDelete(GMS_BOSS4_CHIBI_WORK chibi)
    {
        if (chibi.boost == null)
            return;
        ObjDrawKillAction3DES((OBS_OBJECT_WORK)chibi.boost);
        chibi.boost = null;
    }

}