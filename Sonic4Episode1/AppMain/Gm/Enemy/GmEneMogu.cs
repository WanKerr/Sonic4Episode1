public partial class AppMain
{
    public static void GmEneMoguBuild()
    {
        gm_ene_mogu_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(674)), readAMBFile(GmGameDatGetEnemyData(675)), 0U);
    }

    public static void GmEneMoguFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetEnemyData(674));
        GmGameDBuildRegFlushModel(gm_ene_mogu_obj_3d_list, amsAmbHeader.file_num);
    }

    public static OBS_OBJECT_WORK GmEneMoguInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENE_MOGU_WORK(), "ENE_MOGU");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_ENE_MOGU_WORK gmsEneMoguWork = (GMS_ENE_MOGU_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_mogu_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(676), null, 0, null);
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
        ObjObjectFieldRectSet(work, -4, -44, 4, -38);
        work.move_flag |= 128U;
        if ((eve_rec.flag & 1) == 0)
            work.disp_flag |= 1U;
        work.user_work = (uint)(work.pos.x + (eve_rec.left << 12));
        work.user_flag = (uint)(work.pos.x + (eve_rec.left + eve_rec.width << 12));
        gmsEneMoguWork.spd_dec = 102;
        gmsEneMoguWork.spd_dec_dist = 20480;
        gmsEneMoguWork.flag = 0U;
        gmEneMoguWaitInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static int gmEneMoguCheckWater(GMS_ENE_MOGU_WORK mogu_work, short ofst)
    {
        OBS_OBJECT_WORK parent_obj = (OBS_OBJECT_WORK)mogu_work;
        if (!GmMainIsWaterLevel())
            return 0;
        if ((parent_obj.pos.y >> 12) - ofst >= g_gm_main_system.water_level)
        {
            if (((int)mogu_work.flag & 1) == 0 && ((int)mogu_work.flag & 2) != 0)
            {
                GmEfctCmnEsCreate(parent_obj, 76);
                GmSoundPlaySE("Spray");
            }
            mogu_work.flag |= 1U;
            return 1;
        }
        if (((int)mogu_work.flag & 1) == 0)
            return 0;
        if (((int)mogu_work.flag & 2) != 0)
        {
            GmEfctCmnEsCreate(parent_obj, 76);
            GmSoundPlaySE("Spray");
        }
        mogu_work.flag &= 4294967294U;
        return 1;
    }

    public static void gmEneMoguWaitInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        ObjDrawObjectActionSet(obj_work, 4);
        obj_work.disp_flag |= 4U;
        obj_work.pos.z = -655360;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMoguWaitMain);
    }

    public static void gmEneMoguWaitMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_MOGU_WORK mogu_work = (GMS_ENE_MOGU_WORK)obj_work;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        int num1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int num2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        if (FX_Mul(num1, num1) + FX_Mul(num2, num2) > FX_F32_TO_FX32(25600f))
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMoguJumpInit);
        GMS_EFFECT_3DES_WORK efct_work;
        if (gmEneMoguCheckWater(mogu_work, 48) == 0)
        {
            efct_work = GmEfctEneEsCreate(obj_work, 7);
            if (g_gs_main_sys_info.stage_id == 9)
                efct_work.efct_com.obj_work.pos.z = 393216;
        }
        else
            efct_work = GmEfctEneEsCreate(obj_work, 8);
        GmComEfctSetDispOffsetF(efct_work, 0.0f, -30f, 0.0f);
    }

    public static void gmEneMoguJumpInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_MOGU_WORK mogu_work = (GMS_ENE_MOGU_WORK)obj_work;
        ObjDrawObjectActionSet(obj_work, 4);
        obj_work.disp_flag |= 4U;
        obj_work.spd.y = FX_F32_TO_FX32(-6f);
        obj_work.spd_fall = FX_F32_TO_FX32(0.16f);
        obj_work.pos.y -= FX_F32_TO_FX32(4f);
        obj_work.move_flag |= 128U;
        obj_work.move_flag &= 4294967294U;
        obj_work.move_flag &= 4294967291U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMoguJumpMain);
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
        gmEneMoguCheckWater(mogu_work, 0);
        mogu_work.jumpdown = 0;
    }

    public static void gmEneMoguJumpMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENE_MOGU_WORK mogu_work = (GMS_ENE_MOGU_WORK)obj_work;
        obj_work.spd.x = ((int)obj_work.disp_flag & 1) == 0 ? 2048 : -2048;
        if (obj_work.spd.y > 0)
        {
            if (mogu_work.jumpdown == 0)
            {
                mogu_work.jumpdown = 1;
                ObjDrawObjectActionSet(obj_work, 5);
                obj_work.disp_flag |= 4U;
                obj_work.move_flag &= 4294967039U;
                ObjObjectFieldRectSet(obj_work, -4, -8, 4, -2);
                obj_work.pos.z = 0;
                mogu_work.flag |= 2U;
            }
            if (((int)obj_work.move_flag & 4) != 0)
            {
                if (((int)obj_work.disp_flag & 1) != 0)
                    obj_work.disp_flag &= 4294967294U;
                else
                    obj_work.disp_flag |= 1U;
            }
        }
        if (((int)obj_work.move_flag & 1) != 0)
        {
            GmEneComActionSetDependHFlip(obj_work, 0, 1);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMoguJumpEnd);
        }
        gmEneMoguCheckWater(mogu_work, 0);
    }

    public static void gmEneMoguJumpEnd(OBS_OBJECT_WORK obj_work)
    {
        gmEneMoguCheckWater((GMS_ENE_MOGU_WORK)obj_work, 0);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMoguWalkInit);
    }

    public static void gmEneMoguWalkInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSetDependHFlip(obj_work, 6, 7);
        obj_work.disp_flag |= 4U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMoguWalkMain);
        obj_work.move_flag &= 4294967291U;
        if (((int)obj_work.disp_flag & 1) != 0)
            obj_work.spd.x = -2048;
        else
            obj_work.spd.x = 2048;
    }

    public static void gmEneMoguWalkMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        GMS_ENE_MOGU_WORK mogu_work = (GMS_ENE_MOGU_WORK)obj_work;
        gmEneMoguCheckWater(mogu_work, 0);
        int num1 = gmsPlayerWork.obj_work.pos.x - obj_work.pos.x;
        int num2 = gmsPlayerWork.obj_work.pos.y - obj_work.pos.y;
        int num3 = FX_Mul(num1, num1) + FX_Mul(num2, num2);
        if ((((GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 2) != 0)
        {
            num3 = FX_F32_TO_FX32(6400f) + 1;
            mogu_work.wait_time = 216000;
        }
        if (num3 > FX_F32_TO_FX32(6400f))
        {
            if (mogu_work.wait_time > 0)
            {
                --mogu_work.wait_time;
                if (((int)obj_work.move_flag & 4) == 0)
                    return;
                gmEneMoguJumpInit(obj_work);
            }
            else
            {
                if (AkMathRandFx() > FX_F32_TO_FX32(0.5f))
                    gmEneMoguFlipInit(obj_work);
                mogu_work.wait_time = 216000;
            }
        }
        else
        {
            mogu_work.wait_time = 0;
            if (((int)obj_work.disp_flag & 1) != 0)
            {
                if (GmEneComTargetIsLeft(obj_work, gmsPlayerWork.obj_work) == 0)
                    gmEneMoguFlipInit(obj_work);
            }
            else if (GmEneComTargetIsLeft(obj_work, gmsPlayerWork.obj_work) != 0)
                gmEneMoguFlipInit(obj_work);
            if (((int)obj_work.move_flag & 4) == 0)
                return;
            gmEneMoguJumpInit(obj_work);
        }
    }

    public static void gmEneMoguFwMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer > 0)
            return;
        gmEneMoguFlipInit(obj_work);
    }

    public static void gmEneMoguFlipInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GmEneComActionSet3DNNBlendDependHFlip(obj_work, 2, 3);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMoguFlipMain);
    }

    public static void gmEneMoguFlipMain(OBS_OBJECT_WORK obj_work)
    {
        gmEneMoguSetWalkSpeed((GMS_ENE_MOGU_WORK)obj_work);
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        obj_work.disp_flag ^= 1U;
        gmEneMoguWalkInit(obj_work);
    }

    public static int gmEneMoguSetWalkSpeed(GMS_ENE_MOGU_WORK mogu_work)
    {
        int num = 0;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)mogu_work;
        if (((int)obsObjectWork.disp_flag & 1) != 0)
        {
            if (obsObjectWork.obj_3d.act_id[0] == 3 && obsObjectWork.obj_3d.frame[0] >= 20.0)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, mogu_work.spd_dec, 2048);
            else if (obsObjectWork.pos.x <= (int)obsObjectWork.user_work + mogu_work.spd_dec_dist)
            {
                obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, mogu_work.spd_dec);
                num = 1;
                if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x > (int)obsObjectWork.user_work)
                {
                    obsObjectWork.spd.x = (int)obsObjectWork.user_work - obsObjectWork.pos.x;
                    if (obsObjectWork.spd.x < -mogu_work.spd_dec)
                        obsObjectWork.spd.x = -mogu_work.spd_dec;
                }
            }
            else if (obsObjectWork.spd.x > -2048)
                obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -mogu_work.spd_dec, 2048);
        }
        else if (obsObjectWork.obj_3d.act_id[0] == 2 && obsObjectWork.obj_3d.frame[0] >= 20.0)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, -mogu_work.spd_dec, 2048);
        else if (obsObjectWork.pos.x >= (int)obsObjectWork.user_flag - mogu_work.spd_dec_dist)
        {
            obsObjectWork.spd.x = ObjSpdDownSet(obsObjectWork.spd.x, mogu_work.spd_dec);
            num = 1;
            if (obsObjectWork.spd.x == 0 && obsObjectWork.pos.x < (int)obsObjectWork.user_flag)
            {
                obsObjectWork.spd.x = (int)obsObjectWork.user_flag - obsObjectWork.pos.x;
                if (obsObjectWork.spd.x > mogu_work.spd_dec)
                    obsObjectWork.spd.x = mogu_work.spd_dec;
            }
        }
        else if (obsObjectWork.spd.x < 2048)
            obsObjectWork.spd.x = ObjSpdUpSet(obsObjectWork.spd.x, mogu_work.spd_dec, 2048);
        return num;
    }

}