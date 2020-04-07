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
    public static float GMD_BOSS4_CAP_ROTATE_SPD
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_SPEED(6f);
        }
    }

    public static int GMD_BOSS4_CAP_ZOOM_TIME
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_TIME(270f);
        }
    }

    public static int GMD_BOSS4_CAP_ZOOM_TIME_RAND
    {
        get
        {
            return AppMain.GMM_BOSS4_PAL_TIME(60f);
        }
    }

    public static void T_FUNC(AppMain.MPP_VOID_OBS_OBJECT_WORK func, AppMain.OBS_OBJECT_WORK w)
    {
        w.ppFunc = func;
    }

    public static void SET_FLAG(uint f, AppMain.GMS_BOSS4_CAP_WORK w)
    {
        w.flag |= f;
    }

    public static void RESET_FLAG(uint f, AppMain.GMS_BOSS4_CAP_WORK w)
    {
        w.flag &= ~f;
    }

    public static bool IS_FLAG(uint f, AppMain.GMS_BOSS4_CAP_WORK w)
    {
        return 0 != ((int)w.flag & (int)f);
    }

    private static void GmBoss4CapsuleBuild()
    {
        AppMain._cap_no = 0;
        AppMain._cap_count = 0;
        AppMain._cap_inv_flag = 0;
        AppMain._cap_inv_hit = true;
        AppMain._cap_kill_flag = 0;
    }

    private static void GmBoss4CapsuleFlush()
    {
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4CapsuleInit1st(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        return AppMain.GmBoss4CapsuleInit(eve_rec, pos_x, pos_y, 0);
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4CapsuleInit2nd(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        return AppMain.GmBoss4CapsuleInit(eve_rec, pos_x, pos_y, 1);
    }

    private static void GmBoss4CapsuleSetInvincible(int inv)
    {
        AppMain.GmBoss4CapsuleSetInvincible(inv, true);
    }

    private static void GmBoss4CapsuleSetInvincible(int count, bool hit)
    {
        AppMain._cap_inv_flag = count;
        AppMain._cap_inv_hit = hit;
    }

    private static int GmBoss4CapsuleGetCount()
    {
        return AppMain._cap_count;
    }

    private static void GmBoss4CapsuleClear()
    {
        AppMain._cap_count = 0;
    }

    private static void GmBoss4CapsuleExplosion()
    {
        AppMain._cap_kill_flag = 1;
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4CapsuleInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        AppMain._cap_kill_flag = 0;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS4_CAP_WORK()), "BOSS4_CAP");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS4_CAP_WORK gmsBosS4CapWork = (AppMain.GMS_BOSS4_CAP_WORK)work;
        gmsBosS4CapWork.cap_no = AppMain._cap_no++ % 6;
        gmsBosS4CapWork.type = type;
        work.move_flag |= 256U;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss4GetObj3D(2), gmsEnemy3DWork.obj_3d);
        AppMain.ObjDrawObjectSetToon(work);
        work.disp_flag |= 134217728U;
        work.flag |= 16U;
        work.disp_flag |= 4194304U;
        AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[0], (short)-14, (short)-30, (short)14, (short)-2);
        gmsEnemy3DWork.ene_com.rect_work[0].ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss4CapsuleDamageDefFunc);
        AppMain.ObjRectWorkSet(gmsEnemy3DWork.ene_com.rect_work[1], (short)-1, (short)-9, (short)1, (short)-7);
        gmsEnemy3DWork.ene_com.rect_work[1].ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmBoss4CapsuleAtkHitFunc);
        gmsEnemy3DWork.ene_com.rect_work[1].flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        AppMain.T_FUNC(new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4CapsuleWaitLoad), work);
        if (gmsBosS4CapWork.chibi_type == 4)
            work.disp_flag |= 32U;
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss4CapsuleExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void GmBoss4CapsuleUpdateRol(float spd)
    {
        AppMain._cap_rot_y += AppMain.AKM_DEGtoA32(spd);
        AppMain._cap_rot_y %= AppMain.AKM_DEGtoA32(360);
        if (AppMain._cap_rot_z_flag != 0)
        {
            AppMain._cap_rot_z += AppMain.AKM_DEGtoA32(1f);
            if (AppMain._cap_rot_z >= AppMain.AKM_DEGtoA32(45f))
                AppMain._cap_rot_z_flag = 0;
        }
        else
        {
            AppMain._cap_rot_z -= AppMain.AKM_DEGtoA32(1f);
            if (AppMain._cap_rot_z <= AppMain.AKM_DEGtoA32(-45f))
                AppMain._cap_rot_z_flag = 1;
        }
        if (AppMain._cap_rot_x_flag != 0)
        {
            AppMain._cap_rot_x += AppMain.AKM_DEGtoA32(0.5f);
            if (AppMain._cap_rot_x >= AppMain.AKM_DEGtoA32(60f))
                AppMain._cap_rot_x_flag = 0;
        }
        else
        {
            AppMain._cap_rot_x -= AppMain.AKM_DEGtoA32(0.5f);
            if (AppMain._cap_rot_x <= AppMain.AKM_DEGtoA32(-60f))
                AppMain._cap_rot_x_flag = 1;
        }
        if ((double)AppMain._cap_len_time > 0.0)
            --AppMain._cap_len_time;
        else if (0.0 != (double)AppMain._cap_len_flag)
        {
            AppMain._cap_len += 2f;
            if ((double)AppMain._cap_len >= 100.0)
                AppMain._cap_len_flag = 0.0f;
        }
        else
        {
            AppMain._cap_len -= 2f;
            if ((double)AppMain._cap_len <= 65.0)
            {
                AppMain._cap_len_flag = 1f;
                AppMain._cap_len_time = (float)AppMain.GMD_BOSS4_CAP_ZOOM_TIME + (float)AppMain.GMD_BOSS4_CAP_ZOOM_TIME_RAND * ((float)(AppMain.random.Next() % 256) / 256f);
            }
        }
        if (AppMain._cap_inv_flag > 900)
            AppMain._cap_inv_flag = 0;
        if (AppMain._cap_inv_flag > 0)
            --AppMain._cap_inv_flag;
        else
            AppMain._cap_inv_flag = 0;
    }

    private static void gmBoss4CapsuleWaitLoad(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_CAP_WORK gmsBosS4CapWork = (AppMain.GMS_BOSS4_CAP_WORK)obj_work;
        if (!AppMain.GmBoss4IsBuilded())
            return;
        if (gmsBosS4CapWork.type == 0)
        {
            AppMain.T_FUNC(new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4CapsuleMain), obj_work);
        }
        else
        {
            obj_work.move_flag &= 4294963199U;
            obj_work.move_flag |= 128U;
            AppMain.ObjObjectFieldRectSet(obj_work, (short)-20, (short)-40, (short)20, (short)0);
            obj_work.dir.y = (ushort)0;
            gmsBosS4CapWork.chibi_type = AppMain.gmBoss4ChibiGetAttackType(AppMain.GmBoss4GetLife());
            AppMain.T_FUNC(new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4CapsuleMain2nd), obj_work);
            if (gmsBosS4CapWork.chibi_type == 4)
            {
                AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)329, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
                obsObjectWork.spd.x = AppMain.FX_F32_TO_FX32(2f);
                obsObjectWork.spd.y = AppMain.FX_F32_TO_FX32(-3f);
                AppMain.GmBoss4IncObjCreateCount();
                obsObjectWork.parent_obj = obj_work.parent_obj;
                AppMain.GMM_BS_OBJ((object)gmsBosS4CapWork).flag |= 8U;
            }
        }
        ++AppMain._cap_count;
        gmsBosS4CapWork.wait = 0;
    }

    private static void gmBoss4CapsuleExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GmBoss4DecObjCreateCount();
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss4CapsuleMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK parentObj = (AppMain.GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS4_CAP_WORK w = (AppMain.GMS_BOSS4_CAP_WORK)obj_work;
        AppMain.NNS_MATRIX nnsMatrix1 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix2 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.NNS_MATRIX nnsMatrix3 = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        if (w.wait > 0)
        {
            obj_work.pos.z = 131072;
            AppMain.GmBoss4UtilUpdateFlicker(obj_work, w.flk_work);
            if (AppMain.GmBoss4UtilUpdate1ShotTimer(w.timer))
            {
                AppMain.VecFx32 pos = obj_work.pos;
                pos.z = 135168;
                AppMain.GmBoss4EffCommonInit(735, new AppMain.VecFx32?(pos));
                AppMain.T_FUNC(new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4CapsuleBomb), obj_work);
            }
        }
        else
        {
            AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.node_work.snm_work, parentObj.node_work.work[2], 0);
            obj_work.pos.y += AppMain.FX_F32_TO_FX32(20f);
            int ay = (AppMain._cap_rot_y + AppMain.AKM_DEGtoA32(360) / 6 * w.cap_no) % AppMain.AKM_DEGtoA32(360);
            AppMain.nnMakeRotateXMatrix(nnsMatrix1, AppMain._cap_rot_x);
            AppMain.nnRotateZMatrix(nnsMatrix1, nnsMatrix1, AppMain._cap_rot_z);
            AppMain.nnRotateYMatrix(nnsMatrix1, nnsMatrix1, ay);
            AppMain.nnMakeTranslateMatrix(nnsMatrix2, AppMain._cap_len, 0.0f, 0.0f);
            AppMain.nnMultiplyMatrix(nnsMatrix3, nnsMatrix1, nnsMatrix2);
            AppMain.NNS_VECTOR dst = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            AppMain.nnCopyMatrixTranslationVector(dst, nnsMatrix3);
            obj_work.pos.x += AppMain.FX_F32_TO_FX32(dst.x);
            obj_work.pos.y += AppMain.FX_F32_TO_FX32(dst.y);
            obj_work.pos.z += AppMain.FX_F32_TO_FX32(dst.z);
            AppMain.GlobalPool<AppMain.NNS_VECTOR>.Release(dst);
        }
        if (AppMain._cap_kill_flag != 0)
        {
            AppMain.VecFx32 pos = obj_work.pos;
            pos.z = 135168;
            AppMain.GmBoss4EffCommonInit(735, new AppMain.VecFx32?(pos));
            w.wait = 30;
            AppMain.T_FUNC(new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4CapsuleBomb), obj_work);
            AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix3);
            AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix2);
            AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix1);
        }
        else
        {
            AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
            if (AppMain._cap_inv_flag != 0)
            {
                if (!AppMain._cap_inv_hit)
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
            if (AppMain.IS_FLAG(1073741824U, w))
            {
                --AppMain._cap_count;
                gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
                gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
                gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
                AppMain.RESET_FLAG(1073741824U, w);
                AppMain.GmBoss4UtilInitFlicker(obj_work, w.flk_work);
                AppMain.GmBoss4UtilInit1ShotTimer(w.timer, 20U);
                w.wait = 60;
            }
            AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix3);
            AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix2);
            AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix1);
        }
    }

    private static void gmBoss4CapsuleMain2nd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_CAP_WORK gmsBosS4CapWork = (AppMain.GMS_BOSS4_CAP_WORK)obj_work;
        obj_work.move_flag &= 4294443007U;
        obj_work.spd_fall = AppMain.FX_F32_TO_FX32(0.2f);
        obj_work.move_flag |= 128U;
        obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
        if (((int)obj_work.move_flag & 1) == 0)
            return;
        AppMain.VecFx32 pos = obj_work.pos;
        pos.z = 135168;
        AppMain.GmBoss4EffCommonInit(735, new AppMain.VecFx32?(pos));
        gmsBosS4CapWork.wait = 30;
        obj_work.spd.x = AppMain.FX_F32_TO_FX32(0.0f);
        obj_work.spd.y = AppMain.FX_F32_TO_FX32(-1f);
        obj_work.move_flag &= 4294967294U;
        obj_work.move_flag |= 256U;
        ushort id;
        switch (gmsBosS4CapWork.chibi_type)
        {
            case 2:
                id = (ushort)327;
                break;
            case 3:
                id = (ushort)328;
                break;
            case 4:
                id = (ushort)329;
                break;
            default:
                id = (ushort)326;
                break;
        }
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth(id, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        obsObjectWork.parent_obj = obj_work.parent_obj;
        AppMain.GmBoss4IncObjCreateCount();
        obsObjectWork.spd.y = AppMain.FX_F32_TO_FX32(-4f);
        obsObjectWork.spd.x = AppMain.FX_F32_TO_FX32(-1f);
        AppMain.T_FUNC(new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4CapsuleBomb2nd), obj_work);
    }

    private static void gmBoss4CapsuleAtkHitFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        ((AppMain.GMS_BOSS4_BODY_WORK)my_rect.parent_obj.parent_obj).flag[0] |= 268435456U;
        AppMain.GmEnemyDefaultAtkFunc(my_rect, your_rect);
    }

    private static void gmBoss4CapsuleDamageDefFunc(
      AppMain.OBS_RECT_WORK my_rect,
      AppMain.OBS_RECT_WORK your_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        AppMain.GMS_BOSS4_CAP_WORK w = (AppMain.GMS_BOSS4_CAP_WORK)parentObj1;
        AppMain.GMS_BOSS4_BODY_WORK parentObj3 = (AppMain.GMS_BOSS4_BODY_WORK)parentObj1.parent_obj;
        if (parentObj2 == null || (ushort)1 != parentObj2.obj_type || AppMain._cap_inv_flag > 0)
            return;
        AppMain.GmBoss4UtilSetPlayerAttackReaction(parentObj2, parentObj1);
        AppMain.GmSoundPlaySE("Enemy");
        AppMain.GmBoss4CapsuleSetInvincible(30);
        AppMain.GmBoss4UtilInitNoHitTimer(parentObj3.nohit_work, (AppMain.GMS_ENEMY_COM_WORK)parentObj3, 25);
        if (AppMain.IS_FLAG(4U, w))
            return;
        AppMain.SET_FLAG(1073741824U, w);
        if (((int)parentObj3.flag[0] & 4096) != 0)
            return;
        parentObj3.flag[0] |= 2048U;
        parentObj3.avoid_timer = 90;
    }

    private static void gmBoss4CapsuleBomb(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_CAP_WORK gmsBosS4CapWork = (AppMain.GMS_BOSS4_CAP_WORK)obj_work;
        obj_work.disp_flag |= 32U;
        if (gmsBosS4CapWork.wait > 0)
        {
            --gmsBosS4CapWork.wait;
            if (gmsBosS4CapWork.wait != 30 || AppMain._cap_kill_flag != 0)
                return;
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)325, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
            AppMain.GmBoss4IncObjCreateCount();
            obsObjectWork.parent_obj = obj_work.parent_obj;
        }
        else
            AppMain.GMM_BS_OBJ((object)gmsBosS4CapWork).flag |= 8U;
    }

    private static void gmBoss4CapsuleBomb2nd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_CAP_WORK gmsBosS4CapWork = (AppMain.GMS_BOSS4_CAP_WORK)obj_work;
        obj_work.disp_flag &= 4294963199U;
        if (((int)AppMain.g_obj.flag & 1) != 0)
            obj_work.disp_flag |= 4096U;
        else
            obj_work.pos.x += AppMain.GmBoss4GetScrollOffset();
        if (gmsBosS4CapWork.wait > 0)
        {
            --gmsBosS4CapWork.wait;
            if (gmsBosS4CapWork.wait >= 36)
                return;
            obj_work.disp_flag |= 32U;
        }
        else
            AppMain.GMM_BS_OBJ((object)gmsBosS4CapWork).flag |= 8U;
    }


}