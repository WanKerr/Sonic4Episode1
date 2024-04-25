public partial class AppMain
{
    private static void GmEneGardonBuild()
    {
        gm_ene_gardon_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(677)), readAMBFile(GmGameDatGetEnemyData(678)), 0U);
    }

    private static void GmEneGardonFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(677));
        GmGameDBuildRegFlushModel(gm_ene_gardon_obj_3d_list, amsAmbHeader.file_num);
    }

    private static OBS_OBJECT_WORK GmEneGardonInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_GARDON_WORK(), "ENE_GARDON");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_GARDON_WORK gmsEneGardonWork = (GMS_ENE_GARDON_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_gardon_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(679), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        ObjRectWorkSet(pRec1, -11, -24, 11, 0);
        pRec1.flag |= 1024U;
        pRec1.flag |= 4U;
        OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        pRec2.ppDef = new OBS_RECT_WORK_Delegate1(gmEneGardonDefFunc);
        ObjRectWorkSet(pRec2, -24, -32, 24, 0);
        pRec2.flag |= 1024U;
        pRec2.flag |= 4U;
        gmsEnemy3DWork.ene_com.rect_work[2].flag &= 4294967291U;
        OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        ObjRectWorkSet(pRec3, -19, -32, 19, 0);
        pRec3.flag &= 4294967291U;
        ObjObjectFieldRectSet(work, -4, -8, 4, -2);
        work.move_flag |= 128U;
        if ((eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        gmsEneGardonWork.spd_dec = 51;
        gmsEneGardonWork.spd_dec_dist = 10240;
        gmEneGardonWalkInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static int gmEneGardonGetLength2N(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (((int)gmsPlayerWork.player_flag & GMD_PLF_DIE) != 0)
            return int.MaxValue;
        int x1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int x2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        float f32_1 = FX_FX32_TO_F32(x1);
        float f32_2 = FX_FX32_TO_F32(x2);
        return (int)(f32_1 * (double)f32_1 + f32_2 * (double)f32_2);
    }

    private static int gmEneGardonIsPlayerAttack()
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        return gmsPlayerWork.seq_state == GME_PLY_SEQ_STATE_HOMING || gmsPlayerWork.seq_state == GME_PLY_SEQ_STATE_JUMP || gmsPlayerWork.seq_state == GME_PLY_SEQ_STATE_SPIN ? 1 : 0;
    }

    private static int gmEneGardonIsPlayerFront(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (((int)obj_work.disp_flag & 1) != 0)
        {
            if (obj_work.pos.x > gmsPlayerWork.obj_work.pos.x)
                return 1;
        }
        else if (obj_work.pos.x < gmsPlayerWork.obj_work.pos.x)
            return 1;
        return 0;
    }

    private static void gmEneGardonAtkRectOff(OBS_OBJECT_WORK obj_work)
    {
        ((GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1].flag &= 4294967291U;
    }

    private static void gmEneGardonAtkRectOn(OBS_OBJECT_WORK obj_work)
    {
        ((GMS_ENEMY_3D_WORK)obj_work).ene_com.rect_work[1].flag |= 4U;
    }

    private static void gmEneGardonDefFunc(
      OBS_RECT_WORK my_rect,
      OBS_RECT_WORK your_rect)
    {
        OBS_OBJECT_WORK parentObj1 = my_rect.parent_obj;
        OBS_OBJECT_WORK parentObj2 = your_rect.parent_obj;
        GMS_ENE_GARDON_WORK gmsEneGardonWork = (GMS_ENE_GARDON_WORK)parentObj1;
        GMS_PLAYER_WORK ply_work = (GMS_PLAYER_WORK)parentObj2;
        if (parentObj2 == null || 1 != parentObj2.obj_type)
            return;
        if (ply_work.seq_state == GME_PLY_SEQ_STATE_HOMING || ply_work.seq_state == GME_PLY_SEQ_STATE_HOMING_REF)
        {
            if (gmEneGardonIsPlayerFront(parentObj1) != 0)
            {
                GmEneComActionSetDependHFlip(parentObj1, 8, 9);
                parentObj1.disp_flag &= 4294967291U;
                gmsEneGardonWork.shield = 1;
                GmPlySeqAtkReactionInit(ply_work);
                ply_work.obj_work.spd.y = (int)(ply_work.obj_work.spd.y * 1.5);
                GMS_EFFECT_3DES_WORK efct_3des = GmEfctEneEsCreate(parentObj1, 5);
                efct_3des.efct_com.obj_work.pos.x = parentObj1.pos.x;
                efct_3des.efct_com.obj_work.pos.y = parentObj1.pos.y;
                GmEffect3DESAddDispOffset(efct_3des, 0.0f, 30f, 0.0f);
                GmSoundPlaySE("Casino1");
                gmEneGardonAtkRectOff(parentObj1);
            }
            else
                GmEnemyDefaultDefFunc(my_rect, your_rect);
        }
        else if (parentObj1.pos.y - FX_F32_TO_FX32(20f) > parentObj2.pos.y)
        {
            if (gmEneGardonIsPlayerFront(parentObj1) != 0 || ((int)parentObj1.disp_flag & 1) != ((int)parentObj2.disp_flag & 1))
            {
                GmEneComActionSetDependHFlip(parentObj1, 8, 9);
                parentObj1.disp_flag &= 4294967291U;
                gmsEneGardonWork.shield = 1;
                GmPlySeqAtkReactionInit(ply_work);
                ply_work.obj_work.spd.y = (int)(ply_work.obj_work.spd.y * 1.5);
                GMS_EFFECT_3DES_WORK efct_3des = GmEfctEneEsCreate(parentObj1, 5);
                efct_3des.efct_com.obj_work.pos.x = parentObj1.pos.x;
                efct_3des.efct_com.obj_work.pos.y = parentObj1.pos.y;
                GmEffect3DESAddDispOffset(efct_3des, 0.0f, 30f, 0.0f);
                GmSoundPlaySE("Casino1");
            }
            else
                GmEnemyDefaultDefFunc(my_rect, your_rect);
        }
        else if (gmEneGardonIsPlayerFront(parentObj1) != 0)
        {
            GmEneComActionSetDependHFlip(parentObj1, 4, 5);
            parentObj1.disp_flag &= 4294967291U;
            gmsEneGardonWork.shield = 2;
            ply_work.obj_work.disp_flag ^= 1U;
            GmPlySeqChangeSequence(ply_work, 10);
            if (ply_work.obj_work.spd_m != 0)
            {
                ply_work.obj_work.spd_m = -ply_work.obj_work.spd_m;
                if (MTM_MATH_ABS(ply_work.obj_work.spd_m) < 32768)
                    ply_work.obj_work.spd_m = ((int)ply_work.obj_work.disp_flag & 1) == 0 ? 32768 : short.MinValue;
            }
            else if (parentObj1.pos.x > ply_work.obj_work.pos.x)
            {
                ply_work.obj_work.spd_m = -49152;
                ply_work.obj_work.disp_flag |= 1U;
            }
            else
            {
                ply_work.obj_work.spd_m = 49152;
                ply_work.obj_work.disp_flag &= 4294967294U;
            }
            GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctEneEsCreate(parentObj1, 5);
            gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj1.pos.x;
            gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj1.pos.y;
            GmSoundPlaySE("Casino1");
        }
        else
            GmEnemyDefaultDefFunc(my_rect, your_rect);
    }

    private static void gmEneGardonWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 0, 1);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGardonWalkMain);
        obj_work.move_flag &= 4294967291U;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -1024;
        else
            obj_work.spd.x = 1024;
    }

    private static void gmEneGardonWalkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_GARDON_WORK gmsEneGardonWork = (GMS_ENE_GARDON_WORK)obj_work;
        if (gmsEneGardonWork.shield != 0)
        {
            obj_work.spd.x = 0;
            if (((int)obj_work.disp_flag & 8) == 0)
                return;
            if (gmsEneGardonWork.shield == 1)
                GmEneComActionSetDependHFlip(obj_work, 10, 11);
            else
                GmEneComActionSetDependHFlip(obj_work, 6, 7);
            gmEneGardonAtkRectOn(obj_work);
            gmsEneGardonWork.shield = 0;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGardonWalkWait);
        }
        else
        {
            obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 1024 : -1024;
            if (obj_work.obj_3d.frame[0] >= 40.0 && obj_work.obj_3d.frame[0] <= 60.0)
                obj_work.spd.x = 0;
            if (obj_work.obj_3d.frame[0] >= 100.0 && obj_work.obj_3d.frame[0] <= 120.0)
                obj_work.spd.x = 0;
            if (gmEneGardonGetLength2N(obj_work) <= 4)
            {
                obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGardonWaitToWalkInit);
            }
            else
            {
                if (((int)obj_work.move_flag & 4) == 0 && GmEneComCheckMoveLimit(obj_work, (int)obj_work.user_work, (int)obj_work.user_flag) != 0)
                    return;
                gmEneGardonWaitToFlipInit(obj_work);
            }
        }
    }

    private static void gmEneGardonWalkWait(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGardonWaitToWalkInit);
    }

    private static void gmEneGardonWaitToFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGardonWaitToFlipMain);
        obj_work.spd.x = 0;
        ((GMS_ENE_GARDON_WORK)obj_work).timer = 1;
        GmEneComActionSetDependHFlip(obj_work, 0, 1);
    }

    private static void gmEneGardonWaitToFlipMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_GARDON_WORK gmsEneGardonWork = (GMS_ENE_GARDON_WORK)obj_work;
        if (gmsEneGardonWork.timer > 0)
            --gmsEneGardonWork.timer;
        else
            gmEneGardonFlipInit(obj_work);
    }

    private static void gmEneGardonWaitToWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGardonWaitToWalkMain);
        obj_work.spd.x = 0;
        GMS_ENE_GARDON_WORK gmsEneGardonWork = (GMS_ENE_GARDON_WORK)obj_work;
        gmsEneGardonWork.timer = 60;
        GmEneComActionSetDependHFlip(obj_work, 0, 1);
        obj_work.disp_flag |= 4U;
        gmEneGardonAtkRectOn(obj_work);
        gmsEneGardonWork.shield = 0;
    }

    private static void gmEneGardonWaitToWalkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_GARDON_WORK gmsEneGardonWork = (GMS_ENE_GARDON_WORK)obj_work;
        if (gmsEneGardonWork.shield != 0)
        {
            obj_work.spd.x = 0;
            if (((int)obj_work.disp_flag & 8) == 0)
                return;
            if (gmsEneGardonWork.shield == 1)
                GmEneComActionSetDependHFlip(obj_work, 10, 11);
            else
                GmEneComActionSetDependHFlip(obj_work, 6, 7);
            gmEneGardonAtkRectOn(obj_work);
            gmsEneGardonWork.shield = 0;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGardonWalkWait);
        }
        else
        {
            if (gmEneGardonGetLength2N(obj_work) <= 4)
                return;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGardonWalkInit);
        }
    }

    private static void gmEneGardonFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 2, 3);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGardonFlipMain);
    }

    private static void gmEneGardonFlipMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_ENE_GARDON_WORK)obj_work).shield != 0)
        {
            obj_work.disp_flag ^= 1U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneGardonWalkMain);
        }
        else
        {
            gmEneGardonSetWalkSpeed((GMS_ENE_GARDON_WORK)obj_work);
            if (((int)obj_work.disp_flag & 8) == 0)
                return;
            obj_work.disp_flag ^= 1U;
            gmEneGardonWalkInit(obj_work);
        }
    }

    private static int gmEneGardonSetWalkSpeed(GMS_ENE_GARDON_WORK gardon_work)
    {
        int num = 0;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)gardon_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 2 && obsObjectWork.obj_3d.frame[0] >= 20.0)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, gardon_work.spd_dec, 1024);
            else if (obsObjectWork.pos.x <= (int)obsObjectWork.user_work + gardon_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, gardon_work.spd_dec);
                num = 1;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > (int)obsObjectWork.user_work)
                {
                    obsObjectWork.spd.x = (int)obsObjectWork.user_work - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -gardon_work.spd_dec)
                        obsObjectWork.spd.x = -gardon_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -1024)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -gardon_work.spd_dec, 1024);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 2 && obsObjectWork.obj_3d.frame[0] >= 20.0)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -gardon_work.spd_dec, 1024);
        else if (obsObjectWork.pos.x >= (int)obsObjectWork.user_flag - gardon_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, gardon_work.spd_dec);
            num = 1;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < (int)obsObjectWork.user_flag)
            {
                obsObjectWork.spd.x = (int)obsObjectWork.user_flag - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > gardon_work.spd_dec)
                    obsObjectWork.spd.x = gardon_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < 1024)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, gardon_work.spd_dec, 1024);
        return num;
    }


}