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
    private static void GmEneKamaBuild()
    {
        AppMain.gm_ene_kama_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(699)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(700)), 0U);
    }

    private static void GmEneKamaFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(699));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_kama_obj_3d_list, amsAmbHeader.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmEneKamaInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_KAMA_WORK()), "ENE_KAMA");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_kama_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(701), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-11, (short)-24, (short)11, (short)0);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        AppMain.ObjRectWorkSet(pRec2, (short)-19, (short)-32, (short)19, (short)0);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-19, (short)-32, (short)19, (short)0);
        pRec3.flag &= 4294967291U;
        AppMain.ObjObjectFieldRectSet(work, (short)-4, (short)-8, (short)4, (short)0);
        work.move_flag |= 128U;
        if (((int)eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        if (((int)eve_rec.flag & 2) != 0)
        {
            work.disp_flag |= 2U;
            work.move_flag &= 4294967167U;
            work.dir.z = (ushort)AppMain.AKM_DEGtoA16(180);
            work.disp_flag ^= 1U;
        }
        gmsEneKamaWork.atk_wait = 0;
        if (((int)eve_rec.flag & 4) != 0)
            gmsEneKamaWork.atk_wait += 10;
        if (((int)eve_rec.flag & 8) != 0)
            gmsEneKamaWork.atk_wait += 20;
        if (((int)eve_rec.flag & 16) != 0)
            gmsEneKamaWork.atk_wait += 30;
        gmsEneKamaWork.walk_s = 0;
        if (((int)eve_rec.flag & 32) != 0)
            gmsEneKamaWork.walk_s = 1;
        work.user_work = (uint)(work.pos.x + ((int)eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + ((int)eve_rec.left + (int)eve_rec.width << 12));
        gmsEneKamaWork.spd_dec = 102;
        gmsEneKamaWork.spd_dec_dist = 20480;
        AppMain.gmEneKamaWalkInit(work);
        gmsEneKamaWork.attack = 0;
        AppMain.GmEneUtilInitNodeMatrix(gmsEneKamaWork.node_work, work, 32);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmEneKamaExit));
        AppMain.GmEneUtilGetNodeMatrix(gmsEneKamaWork.node_work, 9);
        AppMain.GmEneUtilGetNodeMatrix(gmsEneKamaWork.node_work, 6);
        AppMain.GmEventMgrLocalEventBirth((ushort)311, work.pos.x, work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0).parent_obj = work;
        AppMain.GmEventMgrLocalEventBirth((ushort)312, work.pos.x, work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0).parent_obj = work;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        work.flag |= 1073741824U;
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmEneKamaLeftHandInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return AppMain.gmEneKamaHandInit(eve_rec, pos_x, pos_y, 1);
    }

    private static AppMain.OBS_OBJECT_WORK GmEneKamaRightHandInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return AppMain.gmEneKamaHandInit(eve_rec, pos_x, pos_y, 0);
    }

    private static int gmEneKamaGetLength2N(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
            return int.MaxValue;
        int x1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int x2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        float f32_1 = AppMain.FX_FX32_TO_F32(x1);
        float f32_2 = AppMain.FX_FX32_TO_F32(x2);
        return (int)((double)f32_1 * (double)f32_1 + (double)f32_2 * (double)f32_2);
    }

    private static int gmEneKamaIsPlayerFront(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (((int)obj_work.disp_flag & 2) != 0)
        {
            if (((int)obj_work.disp_flag & 1) == 0)
            {
                if (obj_work.pos.x > gmsPlayerWork.obj_work.pos.x)
                    return 1;
            }
            else if (obj_work.pos.x < gmsPlayerWork.obj_work.pos.x)
                return 1;
            return 0;
        }
        if (((int)obj_work.disp_flag & 1) != 0)
        {
            if (obj_work.pos.x > gmsPlayerWork.obj_work.pos.x)
                return 1;
        }
        else if (obj_work.pos.x < gmsPlayerWork.obj_work.pos.x)
            return 1;
        return 0;
    }

    private static AppMain.VecFx32 gmEneKamaGetPlayerVectorFx(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        int num1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int num2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        if (num1 > AppMain.FX_F32_TO_FX32(1000f) || num1 < AppMain.FX_F32_TO_FX32(-1000f))
            num1 = 1000;
        if (num2 > AppMain.FX_F32_TO_FX32(1000f) || num2 < AppMain.FX_F32_TO_FX32(-1000f))
            num2 = 1000;
        int denom = AppMain.FX_Sqrt(AppMain.FX_Mul(num1, num1) + AppMain.FX_Mul(num2, num2));
        if (denom == 0)
        {
            vecFx32.x = 0;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = AppMain.FX_Div(4096, denom);
            vecFx32.x = AppMain.FX_Mul(num1, v2);
            vecFx32.y = AppMain.FX_Mul(num2, v2);
        }
        vecFx32.z = 0;
        return vecFx32;
    }

    private static AppMain.VecFx32 gmEneKamaGetParentVectorFx(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        AppMain.GMS_ENE_KAMA_WORK parentObj = (AppMain.GMS_ENE_KAMA_WORK)obj_work.parent_obj;
        int num1;
        int num2;
        if (parentObj == null)
        {
            num1 = 1000;
            num2 = 1000;
        }
        else
        {
            num1 = parentObj.ene_3d_work.ene_com.obj_work.pos.x - obj_work.pos.x;
            num2 = parentObj.ene_3d_work.ene_com.obj_work.pos.y - obj_work.pos.y;
        }
        if (num1 > AppMain.FX_F32_TO_FX32(1000f) || num1 < AppMain.FX_F32_TO_FX32(-1000f))
            num1 = 1000;
        if (num2 > AppMain.FX_F32_TO_FX32(1000f) || num2 < AppMain.FX_F32_TO_FX32(-1000f))
            num2 = 1000;
        int fx32 = AppMain.FX_F32_TO_FX32(Math.Sqrt((double)AppMain.FX_FX32_TO_F32(num1) * (double)AppMain.FX_FX32_TO_F32(num1) + (double)AppMain.FX_FX32_TO_F32(num2) + (double)AppMain.FX_FX32_TO_F32(num2)));
        if (fx32 == 0)
        {
            vecFx32.x = 0;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = AppMain.FX_Div(4096, fx32);
            vecFx32.x = AppMain.FX_Mul(num1, v2);
            vecFx32.y = AppMain.FX_Mul(num2, v2);
        }
        vecFx32.z = 0;
        return vecFx32;
    }

    private static AppMain.OBS_OBJECT_WORK gmEneKamaHandInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENE_KAMA_WORK()), "ENE_KAMA");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)work;
        if (type == 1)
            AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_kama_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        else
            AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_kama_obj_3d_list[2], gmsEnemy3DWork.obj_3d);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        gmsEneKamaWork.ene_3d_work.ene_com.enemy_flag |= 32768U;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-11, (short)-24, (short)11, (short)0);
        pRec1.flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        AppMain.ObjRectWorkSet(pRec2, (short)-19, (short)-32, (short)19, (short)0);
        pRec2.flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-19, (short)-32, (short)19, (short)0);
        pRec3.flag &= 4294967291U;
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        work.disp_flag |= 4194304U;
        gmsEneKamaWork.hand = type;
        AppMain.gmEneKamaHandWaitInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmEneKamaExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GmEneUtilExitNodeMatrix(((AppMain.GMS_ENE_KAMA_WORK)AppMain.mtTaskGetTcbWork(tcb)).node_work);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmEneKamaWalkInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 6, 7);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaWalkMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
        if ((int)obj_work.user_flag != (int)obj_work.user_work)
            return;
        obj_work.spd.x = 0;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 8, 9);
        obj_work.disp_flag |= 4U;
    }

    private static void gmEneKamaWalkMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK kama_work = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        if (AppMain.gmEneKamaIsPlayerFront(obj_work) != 0 && AppMain.gmEneKamaGetLength2N(obj_work) <= 12544)
        {
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaAttackInit);
        }
        else
        {
            if (((int)obj_work.disp_flag & 2) != 0)
            {
                obj_work.move_flag &= 4294967167U;
                if (((int)obj_work.move_flag & 1) == 0)
                    obj_work.spd.y -= obj_work.spd_fall;
                else
                    obj_work.spd.y = 0;
            }
            if ((int)obj_work.user_flag == (int)obj_work.user_work || AppMain.gmEneKamaSetWalkSpeed(kama_work) == 0)
                return;
            AppMain.gmEneKamaFlipInit(obj_work);
        }
    }

    private static void gmEneKamaFwMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        AppMain.gmEneKamaFlipInit(obj_work);
    }

    private static void gmEneKamaFlipInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        obj_work.obj_3d.blend_spd = 0.1f;
        AppMain.GmEneComActionSet3DNNBlendDependHFlip(obj_work, 4, 5);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaFlipMain);
        obj_work.spd.x = 0;
    }

    private static void gmEneKamaFlipMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        obj_work.obj_3d.blend_spd = 1f;
        AppMain.gmEneKamaSetWalkSpeed((AppMain.GMS_ENE_KAMA_WORK)obj_work);
        AppMain.gmEneKamaWalkInit(obj_work);
    }

    private static void gmEneKamaAttackInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        AppMain.GmEneComActionSetDependHFlip(obj_work, 2, 3);
        obj_work.disp_flag &= 4294967291U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaAttackPreMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        gmsEneKamaWork.timer = gmsEneKamaWork.atk_wait;
    }

    private static void gmEneKamaAttackPreMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (gmsEneKamaWork.timer > 0)
        {
            --gmsEneKamaWork.timer;
        }
        else
        {
            AppMain.GmEneComActionSetDependHFlip(obj_work, 0, 1);
            obj_work.disp_flag &= 4294967291U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaAttackMain);
            obj_work.move_flag &= 4294967291U;
            obj_work.spd.x = 0;
            gmsEneKamaWork.timer = 7;
        }
    }

    private static void gmEneKamaAttackMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        if (gmsEneKamaWork.timer > 0)
        {
            --gmsEneKamaWork.timer;
        }
        else
        {
            gmsEneKamaWork.attack = 1;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaAttackWait);
        }
    }

    private static void gmEneKamaAttackWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        if (gmsEneKamaWork.ata_futa == 0)
            return;
        if (gmsEneKamaWork.timer > 0)
            --gmsEneKamaWork.timer;
        else if (AppMain.gmEneKamaGetLength2N(obj_work) <= 12544)
        {
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaFlashInit);
        }
        else
        {
            obj_work.obj_3d.speed[0] = 2f;
            AppMain.GmEneComActionSet3DNNBlendDependHFlip(obj_work, 4, 5);
            obj_work.disp_flag &= 4294967291U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaFlipAtafuta);
            obj_work.spd.x = 0;
            if (gmsEneKamaWork.walk_s != 0)
                gmsEneKamaWork.timer = 15;
            else
                gmsEneKamaWork.timer = 10 + (int)AppMain.mtMathRand() % 20;
        }
    }

    private static void gmEneKamaFlipAtafuta(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        AppMain.gmEneKamaSetWalkSpeed((AppMain.GMS_ENE_KAMA_WORK)obj_work);
        AppMain.GmEneComActionSetDependHFlip(obj_work, 6, 7);
        obj_work.disp_flag |= 4U;
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaAttackWait);
    }

    private static void gmEneKamaFlashInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK kama_work = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        obj_work.spd.x = 0;
        obj_work.obj_3d.speed[0] = 1f;
        AppMain.GmBsCmnClearObject3DNNFadedColor(obj_work);
        obj_work.disp_flag |= 134217728U;
        AppMain.gmEneKamaFadeAnimeSet(kama_work, AppMain.gm_ene_kama_blink_anime);
        obj_work.obj_3d.blend_spd = 0.125f;
        AppMain.GmEneComActionSet3DNNBlendDependHFlip(obj_work, 10, 11);
        obj_work.disp_flag |= 4U;
        kama_work.timer = 180;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaFlashMain);
    }

    private static void gmEneKamaFlashMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        AppMain.gmEneKamaFadeAnimeUpdate((AppMain.GMS_ENE_KAMA_WORK)obj_work, 4096, 1);
        if (gmsEneKamaWork.timer-- >= 0)
            return;
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(obj_work, 9);
        if (((int)obj_work.disp_flag & 2) != 0)
            AppMain.GmComEfctSetDispOffsetF(efct_work, 0.0f, 20f, 0.0f);
        else
            AppMain.GmComEfctSetDispOffsetF(efct_work, 0.0f, -20f, 0.0f);
        gmsEneKamaWork.ene_3d_work.ene_com.enemy_flag |= 65536U;
        gmsEneKamaWork.timer = 180;
        AppMain.GmSoundPlaySE(AppMain.GMD_ENE_KAMA_SE_BOMB);
        obj_work.disp_flag |= 32U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaFlashEnd);
        AppMain.OBS_RECT_WORK pRec = gmsEneKamaWork.ene_3d_work.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec, (short)-30, (short)-30, (short)30, (short)10);
        pRec.flag |= 4U;
        AppMain.GmGmkAnimalInit(obj_work, 0, 0, 0, (byte)0, (byte)0, (ushort)0);
        gmsEneKamaWork.ene_3d_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEneKamaWork.ene_3d_work.ene_com.rect_work[2].flag &= 4294967291U;
    }

    private static void gmEneKamaFlashEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        gmsEneKamaWork.ene_3d_work.ene_com.rect_work[1].flag &= 4294967291U;
        if (gmsEneKamaWork.timer-- >= 0)
            return;
        obj_work.flag |= 8U;
    }

    private static int gmEneKamaSetWalkSpeed(AppMain.GMS_ENE_KAMA_WORK kama_work)
    {
        int num = 0;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)kama_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 5 && (double)obsObjectWork.obj_3d.frame[0] >= 20.0)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, kama_work.spd_dec, 2048);
            else if (obsObjectWork.pos.x <= (int)obsObjectWork.user_work + kama_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, kama_work.spd_dec);
                num = 1;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > (int)obsObjectWork.user_work)
                {
                    obsObjectWork.spd.x = (int)obsObjectWork.user_work - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -kama_work.spd_dec)
                        obsObjectWork.spd.x = -kama_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -2048)
                obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -kama_work.spd_dec, 2048);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 4 && (double)obsObjectWork.obj_3d.frame[0] >= 20.0)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, -kama_work.spd_dec, 2048);
        else if (obsObjectWork.pos.x >= (int)obsObjectWork.user_flag - kama_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = AppMain.ObjSpdDownSet(obsObjectWork.spd.x, kama_work.spd_dec);
            num = 1;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < (int)obsObjectWork.user_flag)
            {
                obsObjectWork.spd.x = (int)obsObjectWork.user_flag - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > kama_work.spd_dec)
                    obsObjectWork.spd.x = kama_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < 2048)
            obsObjectWork.spd.x = AppMain.ObjSpdUpSet(obsObjectWork.spd.x, kama_work.spd_dec, 2048);
        return num;
    }

    private static void gmEneKamaHandWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        gmsEneKamaWork.rot_z = 0;
        gmsEneKamaWork.ene_3d_work.ene_com.rect_work[1].flag &= 4294967291U;
        obj_work.ofst.x = 0;
        obj_work.ofst.y = 0;
        obj_work.dir.z = (ushort)0;
        obj_work.flag &= 4294966783U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaHandWaitMain);
    }

    private static void gmEneKamaHandWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        AppMain.GMS_ENE_KAMA_WORK parentObj = (AppMain.GMS_ENE_KAMA_WORK)obj_work.parent_obj;
        AppMain.NNS_MATRIX kamaHandWaitMainMsm = AppMain.gmEneKamaHandWaitMain_msm;
        if (obj_work.parent_obj == null)
        {
            obj_work.spd.x = 0;
            obj_work.spd_fall = AppMain.FX_F32_TO_FX32(0.2f);
            obj_work.move_flag |= 128U;
        }
        else
        {
            AppMain.NNS_MATRIX mtx1 = gmsEneKamaWork.hand != 1 ? AppMain.GmEneUtilGetNodeMatrix(parentObj.node_work, 6) : AppMain.GmEneUtilGetNodeMatrix(parentObj.node_work, 9);
            if ((double)mtx1.M03 == 0.0 && (double)mtx1.M13 == 0.0)
                return;
            AppMain.nnMakeScaleMatrix(kamaHandWaitMainMsm, 1f, 1f, 1f);
            AppMain.nnMultiplyMatrix(kamaHandWaitMainMsm, mtx1, kamaHandWaitMainMsm);
            AppMain.GmEneUtilSetMatrixNN(obj_work, kamaHandWaitMainMsm);
            if (parentObj.attack == 0)
                return;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaHandAttackInit);
        }
    }

    private static void gmEneKamaHandAttackInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        AppMain.GMS_ENE_KAMA_WORK parentObj = (AppMain.GMS_ENE_KAMA_WORK)obj_work.parent_obj;
        AppMain.VecFx32 playerVectorFx = AppMain.gmEneKamaGetPlayerVectorFx(obj_work);
        obj_work.spd.x = (int)((double)playerVectorFx.x * 1.75);
        obj_work.spd.y = (int)((double)playerVectorFx.y * 1.75);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneKamaHandAttackMain);
        gmsEneKamaWork.timer = 120;
        if (((int)parentObj.ene_3d_work.ene_com.obj_work.disp_flag & 2) != 0)
            obj_work.disp_flag |= 2U;
        gmsEneKamaWork.rot_z_add = ((int)parentObj.ene_3d_work.ene_com.obj_work.disp_flag & 1) == 0 ? AppMain.AKM_DEGtoA32(15) : -AppMain.AKM_DEGtoA32(15);
        AppMain.OBS_RECT_WORK pRec = gmsEneKamaWork.ene_3d_work.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec, (short)-8, (short)-8, (short)8, (short)8);
        pRec.flag |= 4U;
        obj_work.flag |= 512U;
        obj_work.pos.z = 655360;
        AppMain.GmSoundPlaySE(AppMain.GMD_ENE_KAMA_SE_KAMA);
    }

    private static void gmEneKamaHandAttackMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENE_KAMA_WORK gmsEneKamaWork = (AppMain.GMS_ENE_KAMA_WORK)obj_work;
        gmsEneKamaWork.rot_z += gmsEneKamaWork.rot_z_add;
        AppMain.NNS_MATRIX handAttackMainRmat = AppMain.gmEneKamaHandAttackMain_rmat;
        AppMain.NNS_MATRIX handAttackMainTmat = AppMain.gmEneKamaHandAttackMain_tmat;
        AppMain.NNS_MATRIX handAttackMainMat = AppMain.gmEneKamaHandAttackMain_mat;
        AppMain.nnMakeRotateZMatrix(handAttackMainRmat, gmsEneKamaWork.rot_z);
        if (((int)obj_work.disp_flag & 2) != 0)
            AppMain.nnMakeTranslateMatrix(handAttackMainTmat, 10f, 10f, 0.0f);
        else
            AppMain.nnMakeTranslateMatrix(handAttackMainTmat, 10f, -10f, 0.0f);
        AppMain.nnMultiplyMatrix(handAttackMainMat, handAttackMainRmat, handAttackMainTmat);
        obj_work.ofst.x = AppMain.FX_F32_TO_FX32(handAttackMainMat.M03);
        obj_work.ofst.y = AppMain.FX_F32_TO_FX32(handAttackMainMat.M13);
        obj_work.dir.z = (ushort)gmsEneKamaWork.rot_z;
        if (gmsEneKamaWork.timer > 0)
        {
            AppMain.VecFx32 playerVectorFx = AppMain.gmEneKamaGetPlayerVectorFx(obj_work);
            int v2_1 = (int)((double)obj_work.spd.x / 1.75);
            int v2_2 = (int)((double)obj_work.spd.y / 1.75);
            int v2_3;
            int num;
            if (AppMain.FX_Mul(playerVectorFx.x, v2_2) - AppMain.FX_Mul(playerVectorFx.y, v2_1) < 0)
            {
                v2_3 = AppMain.FX_Mul(AppMain.FX_Cos((int)(short)AppMain.AKM_DEGtoA32(1f)), v2_1) - AppMain.FX_Mul(AppMain.FX_Sin((int)(short)AppMain.AKM_DEGtoA32(1f)), v2_2);
                num = AppMain.FX_Mul(AppMain.FX_Sin((int)(short)AppMain.AKM_DEGtoA32(1f)), v2_3) + AppMain.FX_Mul(AppMain.FX_Cos((int)(short)AppMain.AKM_DEGtoA32(1f)), v2_2);
            }
            else
            {
                v2_3 = AppMain.FX_Mul(AppMain.FX_Cos((int)(short)AppMain.AKM_DEGtoA32(-1f)), v2_1) - AppMain.FX_Mul(AppMain.FX_Sin((int)(short)AppMain.AKM_DEGtoA32(-1f)), v2_2);
                num = AppMain.FX_Mul(AppMain.FX_Sin((int)(short)AppMain.AKM_DEGtoA32(-1f)), v2_3) + AppMain.FX_Mul(AppMain.FX_Cos((int)(short)AppMain.AKM_DEGtoA32(-1f)), v2_2);
            }
            obj_work.spd.x = (int)((double)v2_3 * 1.75);
            obj_work.spd.y = (int)((double)num * 1.75);
            --gmsEneKamaWork.timer;
        }
        else
        {
            obj_work.spd.x = 0;
            obj_work.spd_fall = AppMain.FX_F32_TO_FX32(0.2f);
            obj_work.move_flag |= 128U;
            AppMain.GMS_ENE_KAMA_WORK parentObj = (AppMain.GMS_ENE_KAMA_WORK)obj_work.parent_obj;
            if (parentObj == null)
                return;
            parentObj.ata_futa = 1;
        }
    }

    private static void gmEneKamaFadeAnimeSet(
      AppMain.GMS_ENE_KAMA_WORK kama_work,
      AppMain.GMS_ENE_KAMA_FADE_ANIME anime_data)
    {
        kama_work.anime_data = anime_data;
        kama_work.anime_pat_no = 0U;
        kama_work.anime_frame = 0;
    }

    private static void gmEneKamaFadeAnimeUpdate(
      AppMain.GMS_ENE_KAMA_WORK kama_work,
      int speed,
      int repeat)
    {
        AppMain.GMS_ENE_KAMA_FADE_ANIME animeData = kama_work.anime_data;
        AppMain.GMS_ENE_KAMA_FADE_ANIME_PAT kamaFadeAnimePat = animeData.anime_pat[(int)kama_work.anime_pat_no];
        kama_work.anime_frame += speed;
        while (kama_work.anime_frame >= kamaFadeAnimePat.frame)
        {
            kama_work.anime_frame -= kamaFadeAnimePat.frame;
            ++kama_work.anime_pat_no;
            if (kama_work.anime_pat_no < animeData.pat_num)
                kamaFadeAnimePat = animeData.anime_pat[(int)kama_work.anime_pat_no];
            else if (repeat != 0)
            {
                kama_work.anime_pat_no = 0U;
                kamaFadeAnimePat = animeData.anime_pat[(int)kama_work.anime_pat_no];
            }
            else
            {
                kama_work.anime_pat_no = animeData.pat_num - 1U;
                kama_work.anime_frame = kamaFadeAnimePat.frame - 1;
            }
        }
        AppMain.GmBsCmnSetObject3DNNFadedColor((AppMain.OBS_OBJECT_WORK)kama_work, kamaFadeAnimePat.col, kamaFadeAnimePat.intensity);
    }


}