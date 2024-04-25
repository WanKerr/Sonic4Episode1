using System;

public partial class AppMain
{
    private static void GmEneKamaBuild()
    {
        gm_ene_kama_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(699)), readAMBFile(GmGameDatGetEnemyData(700)), 0U);
    }

    private static void GmEneKamaFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(699));
        GmGameDBuildRegFlushModel(gm_ene_kama_obj_3d_list, amsAmbHeader.file_num);
    }

    private static OBS_OBJECT_WORK GmEneKamaInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_KAMA_WORK(), "ENE_KAMA");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_kama_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(701), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -11, -24, 11, 0);
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        ObjRectWorkSet(pRec2, -19, -32, 19, 0);
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -19, -32, 19, 0);
        pRec3.flag &= 4294967291U;
        ObjObjectFieldRectSet(work, -4, -8, 4, 0);
        work.move_flag |= 128U;
        if ((eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        if ((eve_rec.flag & 2) != 0)
        {
            work.disp_flag |= 2U;
            work.move_flag &= 4294967167U;
            work.dir.z = (ushort)AKM_DEGtoA16(180);
            work.disp_flag ^= 1U;
        }
        gmsEneKamaWork.atk_wait = 0;
        if ((eve_rec.flag & 4) != 0)
            gmsEneKamaWork.atk_wait += 10;
        if ((eve_rec.flag & 8) != 0)
            gmsEneKamaWork.atk_wait += 20;
        if ((eve_rec.flag & 16) != 0)
            gmsEneKamaWork.atk_wait += 30;
        gmsEneKamaWork.walk_s = 0;
        if ((eve_rec.flag & 32) != 0)
            gmsEneKamaWork.walk_s = 1;
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        gmsEneKamaWork.spd_dec = 102;
        gmsEneKamaWork.spd_dec_dist = 20480;
        gmEneKamaWalkInit(work);
        gmsEneKamaWork.attack = 0;
        GmEneUtilInitNodeMatrix(gmsEneKamaWork.node_work, work, 32);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmEneKamaExit));
        GmEneUtilGetNodeMatrix(gmsEneKamaWork.node_work, 9);
        GmEneUtilGetNodeMatrix(gmsEneKamaWork.node_work, 6);
        GmEventMgrLocalEventBirth(311, work.pos.x, work.pos.y, 0, 0, 0, 0, 0, 0).parent_obj = work;
        GmEventMgrLocalEventBirth(312, work.pos.x, work.pos.y, 0, 0, 0, 0, 0, 0).parent_obj = work;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        work.flag |= 1073741824U;
        return work;
    }

    private static OBS_OBJECT_WORK GmEneKamaLeftHandInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return gmEneKamaHandInit(eve_rec, pos_x, pos_y, 1);
    }

    private static OBS_OBJECT_WORK GmEneKamaRightHandInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        return gmEneKamaHandInit(eve_rec, pos_x, pos_y, 0);
    }

    private static int gmEneKamaGetLength2N(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & 1024) != 0)
            return int.MaxValue;
        int x1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int x2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        float f32_1 = FX_FX32_TO_F32(x1);
        float f32_2 = FX_FX32_TO_F32(x2);
        return (int)(f32_1 * (double)f32_1 + f32_2 * (double)f32_2);
    }

    private static int gmEneKamaIsPlayerFront(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
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

    private static VecFx32 gmEneKamaGetPlayerVectorFx(OBS_OBJECT_WORK obj_work)
    {
        VecFx32 vecFx32 = new VecFx32();
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        int num1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int num2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        if (num1 > FX_F32_TO_FX32(1000f) || num1 < FX_F32_TO_FX32(-1000f))
            num1 = 1000;
        if (num2 > FX_F32_TO_FX32(1000f) || num2 < FX_F32_TO_FX32(-1000f))
            num2 = 1000;
        int denom = FX_Sqrt(FX_Mul(num1, num1) + FX_Mul(num2, num2));
        if (denom == 0)
        {
            vecFx32.x = 0;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = FX_Div(4096, denom);
            vecFx32.x = FX_Mul(num1, v2);
            vecFx32.y = FX_Mul(num2, v2);
        }
        vecFx32.z = 0;
        return vecFx32;
    }

    private static VecFx32 gmEneKamaGetParentVectorFx(OBS_OBJECT_WORK obj_work)
    {
        VecFx32 vecFx32 = new VecFx32();
        GMS_ENE_KAMA_WORK parentObj = (GMS_ENE_KAMA_WORK)obj_work.parent_obj;
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
        if (num1 > FX_F32_TO_FX32(1000f) || num1 < FX_F32_TO_FX32(-1000f))
            num1 = 1000;
        if (num2 > FX_F32_TO_FX32(1000f) || num2 < FX_F32_TO_FX32(-1000f))
            num2 = 1000;
        int fx32 = FX_F32_TO_FX32(Math.Sqrt(FX_FX32_TO_F32(num1) * (double)FX_FX32_TO_F32(num1) + FX_FX32_TO_F32(num2) + FX_FX32_TO_F32(num2)));
        if (fx32 == 0)
        {
            vecFx32.x = 0;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = FX_Div(4096, fx32);
            vecFx32.x = FX_Mul(num1, v2);
            vecFx32.y = FX_Mul(num2, v2);
        }
        vecFx32.z = 0;
        return vecFx32;
    }

    private static OBS_OBJECT_WORK gmEneKamaHandInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_KAMA_WORK(), "ENE_KAMA");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)work;
        if (type == 1)
            ObjObjectCopyAction3dNNModel(work, gm_ene_kama_obj_3d_list[1], gmsEnemy3DWork.obj_3d);
        else
            ObjObjectCopyAction3dNNModel(work, gm_ene_kama_obj_3d_list[2], gmsEnemy3DWork.obj_3d);
        ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        gmsEneKamaWork.ene_3d_work.ene_com.enemy_flag |= 32768U;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -11, -24, 11, 0);
        pRec1.flag &= 4294967291U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        ObjRectWorkSet(pRec2, -19, -32, 19, 0);
        pRec2.flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -19, -32, 19, 0);
        pRec3.flag &= 4294967291U;
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        work.disp_flag |= 4194304U;
        gmsEneKamaWork.hand = type;
        gmEneKamaHandWaitInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmEneKamaExit(MTS_TASK_TCB tcb)
    {
        GmEneUtilExitNodeMatrix(((GMS_ENE_KAMA_WORK)mtTaskGetTcbWork(tcb)).node_work);
        GmEnemyDefaultExit(tcb);
    }

    private static void gmEneKamaWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 6, 7);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaWalkMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
        if ((int)obj_work.user_flag != (int)obj_work.user_work)
            return;
        obj_work.spd.x = 0;
        GmEneComActionSetDependHFlip(obj_work, 8, 9);
        obj_work.disp_flag |= 4U;
    }

    private static void gmEneKamaWalkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK kama_work = (GMS_ENE_KAMA_WORK)obj_work;
        if (gmEneKamaIsPlayerFront(obj_work) != 0 && gmEneKamaGetLength2N(obj_work) <= 12544)
        {
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaAttackInit);
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
            if ((int)obj_work.user_flag == (int)obj_work.user_work || gmEneKamaSetWalkSpeed(kama_work) == 0)
                return;
            gmEneKamaFlipInit(obj_work);
        }
    }

    private static void gmEneKamaFwMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        gmEneKamaFlipInit(obj_work);
    }

    private static void gmEneKamaFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.obj_3d.blend_spd = 0.1f;
        GmEneComActionSet3DNNBlendDependHFlip(obj_work, 4, 5);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaFlipMain);
        obj_work.spd.x = 0;
    }

    private static void gmEneKamaFlipMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        obj_work.obj_3d.blend_spd = 1f;
        gmEneKamaSetWalkSpeed((GMS_ENE_KAMA_WORK)obj_work);
        gmEneKamaWalkInit(obj_work);
    }

    private static void gmEneKamaAttackInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 2, 3);
        obj_work.disp_flag &= 4294967291U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaAttackPreMain);
        obj_work.move_flag &= 4294967291U;
        obj_work.spd.x = 0;
        gmsEneKamaWork.timer = gmsEneKamaWork.atk_wait;
    }

    private static void gmEneKamaAttackPreMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)obj_work;
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (gmsEneKamaWork.timer > 0)
        {
            --gmsEneKamaWork.timer;
        }
        else
        {
            GmEneComActionSetDependHFlip(obj_work, 0, 1);
            obj_work.disp_flag &= 4294967291U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaAttackMain);
            obj_work.move_flag &= 4294967291U;
            obj_work.spd.x = 0;
            gmsEneKamaWork.timer = 7;
        }
    }

    private static void gmEneKamaAttackMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)obj_work;
        if (gmsEneKamaWork.timer > 0)
        {
            --gmsEneKamaWork.timer;
        }
        else
        {
            gmsEneKamaWork.attack = 1;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaAttackWait);
        }
    }

    private static void gmEneKamaAttackWait(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)obj_work;
        if (gmsEneKamaWork.ata_futa == 0)
            return;
        if (gmsEneKamaWork.timer > 0)
            --gmsEneKamaWork.timer;
        else if (gmEneKamaGetLength2N(obj_work) <= 12544)
        {
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaFlashInit);
        }
        else
        {
            obj_work.obj_3d.speed[0] = 2f;
            GmEneComActionSet3DNNBlendDependHFlip(obj_work, 4, 5);
            obj_work.disp_flag &= 4294967291U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaFlipAtafuta);
            obj_work.spd.x = 0;
            if (gmsEneKamaWork.walk_s != 0)
                gmsEneKamaWork.timer = 15;
            else
                gmsEneKamaWork.timer = 10 + mtMathRand() % 20;
        }
    }

    private static void gmEneKamaFlipAtafuta(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        gmEneKamaSetWalkSpeed((GMS_ENE_KAMA_WORK)obj_work);
        GmEneComActionSetDependHFlip(obj_work, 6, 7);
        obj_work.disp_flag |= 4U;
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaAttackWait);
    }

    private static void gmEneKamaFlashInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK kama_work = (GMS_ENE_KAMA_WORK)obj_work;
        obj_work.spd.x = 0;
        obj_work.obj_3d.speed[0] = 1f;
        GmBsCmnClearObject3DNNFadedColor(obj_work);
        obj_work.disp_flag |= 134217728U;
        gmEneKamaFadeAnimeSet(kama_work, gm_ene_kama_blink_anime);
        obj_work.obj_3d.blend_spd = 0.125f;
        GmEneComActionSet3DNNBlendDependHFlip(obj_work, 10, 11);
        obj_work.disp_flag |= 4U;
        kama_work.timer = 180;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaFlashMain);
    }

    private static void gmEneKamaFlashMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)obj_work;
        gmEneKamaFadeAnimeUpdate((GMS_ENE_KAMA_WORK)obj_work, 4096, 1);
        if (gmsEneKamaWork.timer-- >= 0)
            return;
        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(obj_work, 9);
        if (((int)obj_work.disp_flag & 2) != 0)
            GmComEfctSetDispOffsetF(efct_work, 0.0f, 20f, 0.0f);
        else
            GmComEfctSetDispOffsetF(efct_work, 0.0f, -20f, 0.0f);
        gmsEneKamaWork.ene_3d_work.ene_com.enemy_flag |= 65536U;
        gmsEneKamaWork.timer = 180;
        GmSoundPlaySE(GMD_ENE_KAMA_SE_BOMB);
        obj_work.disp_flag |= 32U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaFlashEnd);
        OBS_RECT_WORK pRec = gmsEneKamaWork.ene_3d_work.ene_com.rect_work[1];
        ObjRectWorkSet(pRec, -30, -30, 30, 10);
        pRec.flag |= 4U;
        GmGmkAnimalInit(obj_work, 0, 0, 0, 0, 0, 0);
        gmsEneKamaWork.ene_3d_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEneKamaWork.ene_3d_work.ene_com.rect_work[2].flag &= 4294967291U;
    }

    private static void gmEneKamaFlashEnd(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)obj_work;
        gmsEneKamaWork.ene_3d_work.ene_com.rect_work[1].flag &= 4294967291U;
        if (gmsEneKamaWork.timer-- >= 0)
            return;
        obj_work.flag |= 8U;
    }

    private static int gmEneKamaSetWalkSpeed(GMS_ENE_KAMA_WORK kama_work)
    {
        int num = 0;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)kama_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 5 && obsObjectWork.obj_3d.frame[0] >= 20.0)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, kama_work.spd_dec, 2048);
            else if (obsObjectWork.pos.x <= (int)obsObjectWork.user_work + kama_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, kama_work.spd_dec);
                num = 1;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > (int)obsObjectWork.user_work)
                {
                    obsObjectWork.spd.x = (int)obsObjectWork.user_work - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -kama_work.spd_dec)
                        obsObjectWork.spd.x = -kama_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -2048)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -kama_work.spd_dec, 2048);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 4 && obsObjectWork.obj_3d.frame[0] >= 20.0)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -kama_work.spd_dec, 2048);
        else if (obsObjectWork.pos.x >= (int)obsObjectWork.user_flag - kama_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, kama_work.spd_dec);
            num = 1;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < (int)obsObjectWork.user_flag)
            {
                obsObjectWork.spd.x = (int)obsObjectWork.user_flag - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > kama_work.spd_dec)
                    obsObjectWork.spd.x = kama_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < 2048)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, kama_work.spd_dec, 2048);
        return num;
    }

    private static void gmEneKamaHandWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)obj_work;
        gmsEneKamaWork.rot_z = 0;
        gmsEneKamaWork.ene_3d_work.ene_com.rect_work[1].flag &= 4294967291U;
        obj_work.ofst.x = 0;
        obj_work.ofst.y = 0;
        obj_work.dir.z = 0;
        obj_work.flag &= 4294966783U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaHandWaitMain);
    }

    private static void gmEneKamaHandWaitMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)obj_work;
        GMS_ENE_KAMA_WORK parentObj = (GMS_ENE_KAMA_WORK)obj_work.parent_obj;
        NNS_MATRIX kamaHandWaitMainMsm = gmEneKamaHandWaitMain_msm;
        if (obj_work.parent_obj == null)
        {
            obj_work.spd.x = 0;
            obj_work.spd_fall = FX_F32_TO_FX32(0.2f);
            obj_work.move_flag |= 128U;
        }
        else
        {
            NNS_MATRIX mtx1 = gmsEneKamaWork.hand != 1 ? GmEneUtilGetNodeMatrix(parentObj.node_work, 6) : GmEneUtilGetNodeMatrix(parentObj.node_work, 9);
            if (mtx1.M03 == 0.0 && mtx1.M13 == 0.0)
                return;
            nnMakeScaleMatrix(kamaHandWaitMainMsm, 1f, 1f, 1f);
            nnMultiplyMatrix(kamaHandWaitMainMsm, mtx1, kamaHandWaitMainMsm);
            GmEneUtilSetMatrixNN(obj_work, kamaHandWaitMainMsm);
            if (parentObj.attack == 0)
                return;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaHandAttackInit);
        }
    }

    private static void gmEneKamaHandAttackInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)obj_work;
        GMS_ENE_KAMA_WORK parentObj = (GMS_ENE_KAMA_WORK)obj_work.parent_obj;
        VecFx32 playerVectorFx = gmEneKamaGetPlayerVectorFx(obj_work);
        obj_work.spd.x = (int)(playerVectorFx.x * 1.75);
        obj_work.spd.y = (int)(playerVectorFx.y * 1.75);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneKamaHandAttackMain);
        gmsEneKamaWork.timer = 120;
        if (((int)parentObj.ene_3d_work.ene_com.obj_work.disp_flag & 2) != 0)
            obj_work.disp_flag |= 2U;
        gmsEneKamaWork.rot_z_add = ((int)parentObj.ene_3d_work.ene_com.obj_work.disp_flag & 1) == 0 ? AKM_DEGtoA32(15) : -AKM_DEGtoA32(15);
        OBS_RECT_WORK pRec = gmsEneKamaWork.ene_3d_work.ene_com.rect_work[1];
        ObjRectWorkSet(pRec, -8, -8, 8, 8);
        pRec.flag |= 4U;
        obj_work.flag |= 512U;
        obj_work.pos.z = 655360;
        GmSoundPlaySE(GMD_ENE_KAMA_SE_KAMA);
    }

    private static void gmEneKamaHandAttackMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_KAMA_WORK gmsEneKamaWork = (GMS_ENE_KAMA_WORK)obj_work;
        gmsEneKamaWork.rot_z += gmsEneKamaWork.rot_z_add;
        NNS_MATRIX handAttackMainRmat = gmEneKamaHandAttackMain_rmat;
        NNS_MATRIX handAttackMainTmat = gmEneKamaHandAttackMain_tmat;
        NNS_MATRIX handAttackMainMat = gmEneKamaHandAttackMain_mat;
        nnMakeRotateZMatrix(handAttackMainRmat, gmsEneKamaWork.rot_z);
        if (((int)obj_work.disp_flag & 2) != 0)
            nnMakeTranslateMatrix(handAttackMainTmat, 10f, 10f, 0.0f);
        else
            nnMakeTranslateMatrix(handAttackMainTmat, 10f, -10f, 0.0f);
        nnMultiplyMatrix(handAttackMainMat, handAttackMainRmat, handAttackMainTmat);
        obj_work.ofst.x = FX_F32_TO_FX32(handAttackMainMat.M03);
        obj_work.ofst.y = FX_F32_TO_FX32(handAttackMainMat.M13);
        obj_work.dir.z = (ushort)gmsEneKamaWork.rot_z;
        if (gmsEneKamaWork.timer > 0)
        {
            VecFx32 playerVectorFx = gmEneKamaGetPlayerVectorFx(obj_work);
            int v2_1 = (int)(obj_work.spd.x / 1.75);
            int v2_2 = (int)(obj_work.spd.y / 1.75);
            int v2_3;
            int num;
            if (FX_Mul(playerVectorFx.x, v2_2) - FX_Mul(playerVectorFx.y, v2_1) < 0)
            {
                v2_3 = FX_Mul(FX_Cos((short)AKM_DEGtoA32(1f)), v2_1) - FX_Mul(FX_Sin((short)AKM_DEGtoA32(1f)), v2_2);
                num = FX_Mul(FX_Sin((short)AKM_DEGtoA32(1f)), v2_3) + FX_Mul(FX_Cos((short)AKM_DEGtoA32(1f)), v2_2);
            }
            else
            {
                v2_3 = FX_Mul(FX_Cos((short)AKM_DEGtoA32(-1f)), v2_1) - FX_Mul(FX_Sin((short)AKM_DEGtoA32(-1f)), v2_2);
                num = FX_Mul(FX_Sin((short)AKM_DEGtoA32(-1f)), v2_3) + FX_Mul(FX_Cos((short)AKM_DEGtoA32(-1f)), v2_2);
            }
            obj_work.spd.x = (int)(v2_3 * 1.75);
            obj_work.spd.y = (int)(num * 1.75);
            --gmsEneKamaWork.timer;
        }
        else
        {
            obj_work.spd.x = 0;
            obj_work.spd_fall = FX_F32_TO_FX32(0.2f);
            obj_work.move_flag |= 128U;
            GMS_ENE_KAMA_WORK parentObj = (GMS_ENE_KAMA_WORK)obj_work.parent_obj;
            if (parentObj == null)
                return;
            parentObj.ata_futa = 1;
        }
    }

    private static void gmEneKamaFadeAnimeSet(
      GMS_ENE_KAMA_WORK kama_work,
      GMS_ENE_KAMA_FADE_ANIME anime_data)
    {
        kama_work.anime_data = anime_data;
        kama_work.anime_pat_no = 0U;
        kama_work.anime_frame = 0;
    }

    private static void gmEneKamaFadeAnimeUpdate(
      GMS_ENE_KAMA_WORK kama_work,
      int speed,
      int repeat)
    {
        GMS_ENE_KAMA_FADE_ANIME animeData = kama_work.anime_data;
        GMS_ENE_KAMA_FADE_ANIME_PAT kamaFadeAnimePat = animeData.anime_pat[(int)kama_work.anime_pat_no];
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
        GmBsCmnSetObject3DNNFadedColor((OBS_OBJECT_WORK)kama_work, kamaFadeAnimePat.col, kamaFadeAnimePat.intensity);
    }


}