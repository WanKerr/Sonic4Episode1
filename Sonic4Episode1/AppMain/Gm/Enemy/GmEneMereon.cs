public partial class AppMain
{
    private static void GmEneMereonBuild()
    {
        gm_ene_mereon_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(670)), readAMBFile(GmGameDatGetEnemyData(672)), 0U);
        gm_ene_mereon_r_obj_3d_list = GmGameDBuildRegBuildModel(readAMBFile(GmGameDatGetEnemyData(671)), readAMBFile(GmGameDatGetEnemyData(672)), 0U);
    }

    private static void GmEneMereonFlush()
    {
        AMS_AMB_HEADER amsAmbHeader1 = readAMBFile(GmGameDatGetEnemyData(670));
        GmGameDBuildRegFlushModel(gm_ene_mereon_obj_3d_list, amsAmbHeader1.file_num);
        AMS_AMB_HEADER amsAmbHeader2 = readAMBFile(GmGameDatGetEnemyData(671));
        GmGameDBuildRegFlushModel(gm_ene_mereon_r_obj_3d_list, amsAmbHeader2.file_num);
    }

    private static OBS_OBJECT_WORK GmEneMereonInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "ENE_MEREON");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        ObjObjectCopyAction3dNNModel(work, gm_ene_mereon_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(673), null, 0, null);
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
        work.obj_3d.drawflag |= 8388608U;
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        work.flag |= 2U;
        GmEneComActionSetDependHFlip(work, 0, 1);
        if (eve_rec.id == 4)
            gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        gmEneMereonHideSearchInit(work);
        return work;
    }

    private static void gmEneMereonHideSearchInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        obj_work.disp_flag &= 4294967294U;
        if (gmsPlayerWork.obj_work.pos.x < obj_work.pos.x)
            obj_work.disp_flag |= 1U;
        gmEneMereonCheckFwFlip(obj_work);
        obj_work.disp_flag |= 32U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMereonHideSearchMain);
    }

    private static void gmEneMereonHideSearchMain(OBS_OBJECT_WORK obj_work)
    {
        float[] numArray1 = new float[2];
        float[] numArray2 = new float[2];
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        OBS_RECT_WORK obsRectWork1 = gmsPlayerWork.rect_work[2];
        numArray1[0] = FXM_FX32_TO_FLOAT(gmsPlayerWork.obj_work.pos.x + (obsRectWork1.rect.top + obsRectWork1.rect.bottom >> 1));
        numArray1[1] = FXM_FX32_TO_FLOAT(gmsPlayerWork.obj_work.pos.y + (obsRectWork1.rect.left + obsRectWork1.rect.right >> 1));
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
        OBS_RECT_WORK obsRectWork2 = gmsEnemyComWork.rect_work[2];
        numArray2[0] = FXM_FX32_TO_FLOAT(obj_work.pos.x + (obsRectWork2.rect.top + obsRectWork2.rect.bottom >> 1));
        numArray2[1] = FXM_FX32_TO_FLOAT(obj_work.pos.y + (obsRectWork2.rect.left + obsRectWork2.rect.right >> 1));
        float num1 = numArray2[0] - numArray1[0];
        float num2 = numArray2[1] - numArray1[1];
        float num3 = gmsEnemyComWork.eve_rec.id != 4 ? 192f : 96f;
        if (num1 * (double)num1 + num2 * (double)num2 > num3 * (double)num3)
            return;
        obj_work.disp_flag &= 4294967263U;
        gmEneMereonAppearInit(obj_work);
    }

    private static void gmEneMereonAppearInit(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        obj_work.disp_flag &= 4294967294U;
        if (gmsEnemyComWork.eve_rec.id == 4 && gmsPlayerWork.obj_work.pos.x > obj_work.pos.x)
            obj_work.disp_flag |= 1U;
        GmEneComActionSetDependHFlip(obj_work, 0, 1);
        obj_work.disp_flag |= 4U;
        obj_work.disp_flag |= 134217728U;
        obj_work.obj_3d.draw_state.alpha.alpha = 0.0f;
        obj_work.user_timer = 0;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMereonAppearMain);
    }

    private static void gmEneMereonAppearMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obj_work;
        obj_work.user_timer = ObjTimeCountUp(obj_work.user_timer);
        int num = gmsEnemyComWork.eve_rec.id != 4 ? 15 : 30;
        obj_work.obj_3d.draw_state.alpha.alpha = obj_work.user_timer / (float)(num * 4096);
        if (obj_work.user_timer < num * 4096)
            return;
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        obj_work.disp_flag &= 4160749567U;
        if (gmsEnemyComWork.eve_rec.id == 4)
            gmEneMereonAtkInit(obj_work);
        else
            gmEneMereonAtkRocketInit(obj_work);
    }

    private static void gmEneMereonAtkInit(OBS_OBJECT_WORK obj_work)
    {
        GmEneComActionSet3DNNBlendDependHFlip(obj_work, 3, 4);
        obj_work.flag &= 4294967293U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMereonAtkMain);
        obj_work.user_timer = 0;
    }

    private static void gmEneMereonAtkMain(OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) != 0)
        {
            int spd_x = -8192;
            short dir = -16384;
            if (((int)obj_work.disp_flag & 1) != 0)
            {
                spd_x = -spd_x;
                dir += short.MinValue;
            }
            GmEneStingCreateBullet(obj_work, -81920, -49152, 32768, -98304, -49152, 0, spd_x, 0, dir);
            GmSoundPlaySE("Sting");
            GmEneComActionSet3DNNBlendDependHFlip(obj_work, 0, 1);
            obj_work.user_timer = 122880;
        }
        if (obj_work.user_timer == 0)
            return;
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer != 0)
            return;
        gmEneMereonHideInit(obj_work);
    }

    private static void gmEneMereonHideInit(OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMereonHideMain);
        obj_work.disp_flag |= 134217728U;
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        obj_work.user_timer = 122880;
    }

    private static void gmEneMereonHideMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        obj_work.obj_3d.draw_state.alpha.alpha = obj_work.user_timer / 122880f;
        if (obj_work.user_timer > 0)
            return;
        obj_work.obj_3d.draw_state.alpha.alpha = 1f;
        obj_work.disp_flag &= 4160749567U;
        obj_work.disp_flag |= 32U;
        obj_work.flag |= 4U;
    }

    private static void gmEneMereonAtkRocketInit(OBS_OBJECT_WORK obj_work)
    {
        ObjDrawObjectActionSet3DNNBlend(obj_work, 2);
        obj_work.move_flag &= 4294967039U;
        obj_work.move_flag |= 128U;
        obj_work.flag &= 4294967293U;
        obj_work.disp_flag &= 1U;
        obj_work.user_timer = 0;
        if (GmEneComTargetIsLeft(obj_work, g_gm_main_system.ply_work[0].obj_work) == 0)
            obj_work.user_timer = 4096;
        obj_work.user_work = 0U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMereonRocketFallMain);
    }

    private static void gmEneMereonRocketFallMain(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer != 0)
        {
            obj_work.dir.y += (ushort)obj_work.user_timer;
            if (obj_work.dir.y >= 32768)
            {
                obj_work.dir.y = 32768;
                obj_work.user_timer = 0;
            }
        }
        if (obj_work.user_work != 0U)
        {
            obj_work.user_work = (uint)ObjTimeCountDown((int)obj_work.user_work);
            if (obj_work.user_work != 0U)
                return;
            GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)obj_work;
            ObjAction3dNNMotionRelease(obj_work.obj_3d);
            ObjObjectAction3dNNModelReleaseCopy(obj_work);
            ObjObjectCopyAction3dNNModel(obj_work, gm_ene_mereon_r_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            ObjObjectAction3dNNMotionLoad(obj_work, 0, false, ObjDataGet(673), null, 0, null);
            ObjDrawObjectSetToon(obj_work);
            ObjObjectFieldRectSet(obj_work, -4, -4, 4, 4);
            ObjDrawObjectActionSet(obj_work, 5);
            obj_work.disp_flag |= 4U;
            if (obj_work.dir.y != 0 || obj_work.user_timer != 0)
            {
                obj_work.dir.y = 0;
                obj_work.disp_flag &= 4294967294U;
            }
            else
                obj_work.disp_flag |= 1U;
            obj_work.move_flag |= 64U;
            obj_work.spd_m = ((int)obj_work.disp_flag & 1) == 0 ? 8192 : -8192;
            obj_work.move_flag |= 1024U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmEneMereonRocketMain);
            obj_work.user_flag = 0U;
            obj_work.user_timer = 0;
            GMS_EFFECT_3DES_WORK efct_work = GmEfctEneEsCreate(obj_work, 1);
            GmComEfctSetDispOffsetF(efct_work, -38f, -11f, 0.0f);
            efct_work.efct_com.obj_work.flag |= 524288U;
        }
        else
        {
            if (((int)obj_work.disp_flag & 8) == 0 || ((int)obj_work.move_flag & 1) == 0)
                return;
            obj_work.user_work = 4096U;
            GmComEfctAddDispOffsetF(GmEfctEneEsCreate(obj_work, 4), 0.0f, -16f, 16f);
        }
    }

    private static void gmEneMereonRocketMain(OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_flag == 0U && (ushort)(obj_work.dir.z + 8192U) > 16384)
        {
            obj_work.user_flag = 1U;
            obj_work.move_flag &= 4294967231U;
            obj_work.move_flag |= 257U;
            obj_work.spd.x = obj_work.spd_m;
        }
        if (obj_work.user_timer != 0)
        {
            --obj_work.user_timer;
            if (obj_work.user_timer != 0)
                return;
            obj_work.move_flag |= 64U;
            obj_work.user_flag = 0U;
            obj_work.spd.x = 0;
        }
        else
        {
            if (obj_work.user_flag == 0U)
                return;
            obj_work.dir.z = ObjRoopMove16(obj_work.dir.z, 0, 1024);
            if (obj_work.dir.z != 0)
                return;
            obj_work.user_timer = 4;
        }
    }

    private static void gmEneMereonCheckFwFlip(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK gmsPlayerWork = g_gm_main_system.ply_work[0];
        if (gmsPlayerWork.obj_work.pos.x < obj_work.pos.x)
        {
            if (obj_work.obj_3d.act_id[0] != 0)
            {
                ObjDrawObjectActionSet(obj_work, 0);
                obj_work.disp_flag &= 4294967294U;
            }
        }
        else if (gmsPlayerWork.obj_work.pos.x > obj_work.pos.x && obj_work.obj_3d.act_id[0] != 1)
        {
            ObjDrawObjectActionSet(obj_work, 1);
            obj_work.disp_flag |= 1U;
        }
        obj_work.disp_flag |= 4U;
    }


}