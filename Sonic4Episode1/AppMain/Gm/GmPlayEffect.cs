using System;

public partial class AppMain
{
    private static MTS_TASK_TCB gm_ply_efct_trail_sys_tcb;
    private static readonly GMS_PLY_EFCT_TRAIL_COLOR gm_ply_efct_trail_color_son
        = new GMS_PLY_EFCT_TRAIL_COLOR(new NNS_RGBA(0.4f, 0.4f, 1f, 1f), new NNS_RGBA(0f, 0f, 1f, 1f));
    private static readonly GMS_PLY_EFCT_TRAIL_COLOR gm_ply_efct_trail_color_sson
        = new GMS_PLY_EFCT_TRAIL_COLOR(new NNS_RGBA(0.9f, 0.9f, 0.2f, 0.8f), new NNS_RGBA(0.5f, 0.5f, 0f, 0.6f));
    private static readonly GMS_PLY_EFCT_TRAIL_SETTING[] gm_ply_efct_trail_setting
        = new GMS_PLY_EFCT_TRAIL_SETTING[]
        {
            new GMS_PLY_EFCT_TRAIL_SETTING(14f, 13f, 50f, 50f),
            new GMS_PLY_EFCT_TRAIL_SETTING(14f, 13f, 50f, 50f),
            new GMS_PLY_EFCT_TRAIL_SETTING(18f, 16f, 20f, 20f)
        };

    private const int GME_PLY_EFCT_TRAIL_TYPE_HOMING = 0;
    private const int GME_PLY_EFCT_TRAIL_TYPE_SPINDASH = 1;
    private const int GME_PLY_EFCT_TRAIL_TYPE_MAX = 2;
    public const float GMD_PLY_EFCT_OFST_FRONT_PLAYER = 16f;
    public const float GMD_PLY_EFCT_OFST_FRONT_A = 160f;
    public const int GMD_PLY_EFCT_BARRIER_ADD_OFST_Z = 15;
    public const int GMD_PLY_EFCT_SPIN_DASH_CIRCLE_BLUR_BASE_OFST_Y = 0;
    public const int GMD_PLY_EFCT_SPIN_DASH_CIRCLE_BLUR_BASE_OFST_Y_PINBALL = 0;
    public const int GMD_PLY_EFCT_SPIN_DASH_CIRCLE_BLUR_BASE_OFST_Z = 0;
    public const int GMD_PLY_EFCT_SPIN_DASH_BLUR_BASE_OFST_Y = 0;
    public const int GMD_PLY_EFCT_SPIN_DASH_BLUR_BASE_OFST_Y_PINBALL = 1;
    public const int GMD_PLY_EFCT_SPIN_START_BLUR_BASE_OFST_Y = 2;
    public const int GMD_PLY_EFCT_SPIN_START_BLUR_BASE_OFST_Y_PINBALL = 1;
    public const int GMD_PLY_EFCT_SPIN_START_BLUR_BASE_OFST_Z = 14;
    public const int GMD_PLY_EFCT_SPIN_START_BLUR_FRAME = 15;
    public const int GMD_PLY_EFCT_SPIN_JUMP_BLUR_BASE_OFST_Y = -5;
    public const int GMD_PLY_EFCT_SPIN_JUMP_BLUR_BASE_OFST_Y_PINBALL = 1;
    public const int GMD_PLY_EFCT_SPIN_JUMP_BLUR_DIST_MAX = 8;
    public const int GMD_PLY_EFCT_SPIN_JUMP_BLUR_DIST_MIN = 1;
    public const int GMD_PLY_EFCT_SPIN_JUMP_BLUR_PLY_MTN_FRAME = 20;
    public const int GMD_PLY_EFCT_BUBBLE_SPD_Y_ACC = -64;
    public const int GMD_PLY_EFCT_BUBBLE_SPD_Y_MAX = -65536;
    public const int GMD_PLY_EFCT_BUBBLE_SURFACE_ADJUST = 32768;
    public const int GMD_PLF_EFCT_RUN_SPRAY_MIN_SPD = 4096;
    public const int GMD_PLF_EFCT_RUN_SPRAY_BIG_SPD = 16384;
    public const int GMD_PLF_EFCT_RUN_SPRAY_OFST_Z = 15;

    private static void GmPlyEfctTrailSysInit()
    {
        if (gm_ply_efct_trail_sys_tcb != null)
            return;
        gm_ply_efct_trail_sys_tcb = MTM_TASK_MAKE_TCB(new GSF_TASK_PROCEDURE(gmPlyEfctTrailSysMain), null, 0U, 0, 8448U, 3, null, "GM_PLY_EF_TRAIL");
    }

    private static void GmPlyEfctTrailSysExit()
    {
        if (gm_ply_efct_trail_sys_tcb == null)
            return;
        mtTaskClearTcb(gm_ply_efct_trail_sys_tcb);
        gm_ply_efct_trail_sys_tcb = null;
    }

    private static void GmPlyEfctCreateTrail(GMS_PLAYER_WORK ply_work, int efct_type)
    {
        var data = (NNS_TEXLIST)ObjDataGet(18).pData;
        var param = new AMS_TRAIL_PARAM();
        param.startSize = gm_ply_efct_trail_setting[efct_type].start_size;
        param.endSize = gm_ply_efct_trail_setting[efct_type].end_size;
        param.life = gm_ply_efct_trail_setting[efct_type].life;
        param.vanish_time = gm_ply_efct_trail_setting[efct_type].vanish_time;

        var color = (ply_work.player_flag & GMD_PLF_SUPER_SONIC) == 0 ? gm_ply_efct_trail_color_son : gm_ply_efct_trail_color_sson;
        param.startColor = color.start_col;
        param.endColor = color.end_col;
        param.trail_obj_work = ply_work.obj_work;
        param.partsNum = 63;
        param.zBias = -393216.0f;
        param.texId = data.nTex - 1;
        param.blendType = 1;
        param.zTest = 1;

        // yes this is actually how it works
        if (efct_type == 2)
        {
            param.endColor.a = 0.0f;
            param.life = 50.0f;
            param.vanish_time = 50.0f;
            amTrailMakeEffect(param, 1, 1);

            param.zBias = -131072.0f;
            param.startColor.r = 1.0f;
            param.startColor.g = 1.0f;
            param.startColor.b = 1.0f;
            param.startColor.a = 1.0f;
            param.endColor.r = 1.0f;
            param.endColor.g = 1.0f;
            param.ptclTexId = 0x21;
            param.endColor.b = 1.0f;
            param.endColor.a = 0.0f;
            amTrailMakeEffect(param, 2, 1);

            param.startColor.a = 0.65f;
            param.ptclTexId = 0x21;
            amTrailMakeEffect(param, 2, 1);

            param.startColor.a = 0.4f;
            param.ptclTexId = 0x21;
            amTrailMakeEffect(param, 2, 1);

            return;
        }

        amTrailMakeEffect(param, 1, 1);
    }

    private static void GmPlyEfctCreateBarrier(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work1 = GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.BARRIER);
        efct_work1.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctBarrierMain);
        efct_work1.efct_com.obj_work.user_work = 4U;
        GmComEfctAddDispOffset(efct_work1, 0, 0, 61440);
        GMS_EFFECT_3DES_WORK efct_work2 = GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.BARRIER_01);
        efct_work2.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctBarrierMain);
        efct_work2.efct_com.obj_work.user_work = 5U;
        GmComEfctAddDispOffset(efct_work2, 0, 0, 61440);
        if (((int)ply_work.gmk_flag2 & 2) == 0)
            return;
        efct_work2.efct_com.obj_work.disp_flag |= 32U;
    }

    private static void GmPlyEfctCreateInvincible(GMS_PLAYER_WORK ply_work)
    {
        gmPlyEfctCreateInvincibleCircle(ply_work);
        gmPlyEfctCreateInvincibleTail(ply_work);
        GMM_EFFECT_CREATE_WORK(() => new GMS_EFFECT_COM_WORK(), ply_work.obj_work, 0, "GM_PLY_INV_MGR").ppFunc =
            new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctInvincibleMgrMain);
    }

    private static void GmPlyEfctCreateRollDash(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 147456) != 0)
            return;
        GMS_EFFECT_3DES_WORK efct_work;
        if (((int)ply_work.obj_work.disp_flag & 1) != 0)
        {
            efct_work = GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.ROLLDASH_L);
            GmComEfctSetDispOffsetF(efct_work, 1.5f, 5f, GMD_PLY_EFCT_OFST_FRONT_PLAYER);
            efct_work.obj_3des.ecb.drawObjState = 0U;
        }
        else
        {
            efct_work = GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.ROLLDASH_R);
            GmComEfctSetDispOffsetF(efct_work, -1.5f, 5f, GMD_PLY_EFCT_OFST_FRONT_PLAYER);
            efct_work.obj_3des.ecb.drawObjState = 0U;
        }

        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctRollDashMain);
    }

    private static void GmPlyEfctCreateSweat(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.SWEAT);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSweatMain);
        GmComEfctSetDispOffsetF(efct_work, -5f, -10f, GMD_PLY_EFCT_OFST_FRONT_PLAYER);
    }

    private static void GmPlyEfctCreateRunDust(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work =
            GMM_MAIN_GET_ZONE_TYPE() != 2 || (ply_work.player_flag & GMD_PLF_WATER) == 0 ||
            (ply_work.obj_work.pos.y >> 12) - 4 < g_gm_main_system.water_level
                ? GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.RUN)
                : GmEfctZoneEsCreate(ply_work.obj_work, 2, 20);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctRunDustMain);
        GmComEfctSetDispOffsetF(efct_work, -8f, GMD_PLY_EFCT_OFST_FRONT_PLAYER, 0.0f);
        efct_work.efct_com.obj_work.parent_ofst.z = FXM_FLOAT_TO_FX32(GMD_PLY_EFCT_OFST_FRONT_PLAYER);
    }

    private static void GmPlyEfctCreateDash1Dust(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work =
            GMM_MAIN_GET_ZONE_TYPE() != 2 || (ply_work.player_flag & GMD_PLF_WATER) == 0 ||
            (ply_work.obj_work.pos.y >> 12) - 4 < g_gm_main_system.water_level
                ? GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.DASH)
                : GmEfctZoneEsCreate(ply_work.obj_work, 2, 8);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctDash1DustMain);
        GmComEfctSetDispOffsetF(efct_work, -8f, GMD_PLY_EFCT_OFST_FRONT_PLAYER, 0.0f);
        efct_work.efct_com.obj_work.parent_ofst.z = FXM_FLOAT_TO_FX32(GMD_PLY_EFCT_OFST_FRONT_PLAYER);
    }

    private static void GmPlyEfctCreateDash2Dust(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work =
            GMM_MAIN_GET_ZONE_TYPE() != 2 || (ply_work.player_flag & GMD_PLF_WATER) == 0 ||
            (ply_work.obj_work.pos.y >> 12) - 4 < g_gm_main_system.water_level
                ? GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.ROLLDASH_S)
                : GmEfctZoneEsCreate(ply_work.obj_work, 2, 20);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctDash2DustMain);
        GmComEfctSetDispOffsetF(efct_work, -8f, GMD_PLY_EFCT_OFST_FRONT_PLAYER, 0.0f);
        efct_work.efct_com.obj_work.parent_ofst.z = FXM_FLOAT_TO_FX32(GMD_PLY_EFCT_OFST_FRONT_PLAYER);
    }

    private static void GmPlyEfctCreateDash2Impact(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & 147456) != 0)
            return;

        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.ROLLDASH);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctDash2ImpactMain);
        GmComEfctSetDispOffsetF(efct_work, -8f, GMD_PLY_EFCT_OFST_FRONT_PLAYER, 0.0f);
        efct_work.efct_com.obj_work.parent_ofst.z = FXM_FLOAT_TO_FX32(GMD_PLY_EFCT_OFST_FRONT_PLAYER);
    }

    private static void GmPlyEfctCreateSpinDust(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work =
            GMM_MAIN_GET_ZONE_TYPE() != 2 || (ply_work.player_flag & GMD_PLF_WATER) == 0 ||
            (ply_work.obj_work.pos.y >> 12) - 4 < g_gm_main_system.water_level
                ? GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.SPIN_00)
                : GmEfctZoneEsCreate(ply_work.obj_work, 2, 28);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSpinDustMain);
        GmComEfctSetDispOffsetF(efct_work, -8f, GMD_PLY_EFCT_OFST_FRONT_PLAYER, 0.0f);
        efct_work.efct_com.obj_work.parent_ofst.z = FXM_FLOAT_TO_FX32(GMD_PLY_EFCT_OFST_FRONT_PLAYER);
    }

    private static void GmPlyEfctCreateSpinAddDust(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work =
            GMM_MAIN_GET_ZONE_TYPE() != 2 || (ply_work.player_flag & GMD_PLF_WATER) == 0 ||
            (ply_work.obj_work.pos.y >> 12) - 4 < g_gm_main_system.water_level
                ? GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.SPIN_01)
                : GmEfctZoneEsCreate(ply_work.obj_work, 2, 29);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSpinAddDustMain);
        GmComEfctSetDispOffsetF(efct_work, -8f, GMD_PLY_EFCT_OFST_FRONT_PLAYER, 0.0f);
        efct_work.efct_com.obj_work.parent_ofst.z = FXM_FLOAT_TO_FX32(GMD_PLY_EFCT_OFST_FRONT_PLAYER);
    }

    private static void GmPlyEfctCreateSpinDashImpact(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.ROLLDASH);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSpinDashImpactMain);
        GmComEfctSetDispOffsetF(efct_work, -6f, GMD_PLY_EFCT_OFST_FRONT_PLAYER, 0.0f);
        efct_work.efct_com.obj_work.parent_ofst.z = FXM_FLOAT_TO_FX32(GMD_PLY_EFCT_OFST_FRONT_PLAYER);
    }

    private static void GmPlyEfctCreateSpinDashDust(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work =
            GMM_MAIN_GET_ZONE_TYPE() != 2 || (ply_work.player_flag & GMD_PLF_WATER) == 0 ||
            (ply_work.obj_work.pos.y >> 12) - 4 < g_gm_main_system.water_level
                ? GmEfctCmnEsCreate(ply_work.obj_work, 54)
                : GmEfctZoneEsCreate(ply_work.obj_work, 2, 19);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSpinDashDustMain);
        GmComEfctSetDispOffsetF(efct_work, -8f, GMD_PLY_EFCT_OFST_FRONT_PLAYER, 0.0f);
        efct_work.efct_com.obj_work.parent_ofst.z = FXM_FLOAT_TO_FX32(GMD_PLY_EFCT_OFST_FRONT_PLAYER);
    }

    private static object GmPlyEfctCreateSpinDashCircleBlur(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.efct_spin_dash_cir_blur != null && ((int)ply_work.efct_spin_dash_cir_blur.flag & 12) == 0)
            return null;
        GMS_EFFECT_3DES_WORK efct_work;
        if ((ply_work.player_flag & GMD_PLF_SUPER_SONIC) != 0)
        {
            efct_work = GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.SS_SPIN_D);
            efct_work.efct_com.obj_work.user_timer = 82;
        }
        else
        {
            efct_work = GmEfctCmnEsCreate(ply_work.obj_work, GME_EFCT_CMN_IDX.SPIN_D);
            efct_work.efct_com.obj_work.user_timer = 73;
        }

        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSpinDashCircleBlurMain);
        if ((ply_work.player_flag & GMD_PLF_PINBALL_SONIC) != 0 || GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            GmComEfctSetDispOffset(efct_work, 0, 0, 0);
        else
            GmComEfctSetDispOffset(efct_work, 0, 0, 0);
        efct_work.efct_com.obj_work.obj_3des.speed = ply_work.obj_work.obj_3d.speed[0];
        mtTaskChangeTcbDestructor(efct_work.efct_com.obj_work.tcb, new GSF_TASK_PROCEDURE(gmPlyEfctSpinDashBlurDest));
        ply_work.efct_spin_dash_cir_blur = efct_work.efct_com.obj_work;
        return efct_work;
    }

    private static object GmPlyEfctCreateSpinDashBlur(GMS_PLAYER_WORK ply_work, uint type)
    {
        if (ply_work.efct_spin_dash_blur != null && ((int)ply_work.efct_spin_dash_blur.flag & 12) == 0)
            return null;
        GMS_EFFECT_3DES_WORK efct_work;
        if ((ply_work.player_flag & GMD_PLF_SUPER_SONIC) != 0)
        {
            if (type == 0)
            {
                efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 74);
                efct_work.efct_com.obj_work.user_timer = 74;
            }
            else
            {
                efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 83);
                efct_work.efct_com.obj_work.user_timer = 83;
            }
        }
        else
        {
            if (type == 0)
            {
                efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 70);
                efct_work.efct_com.obj_work.user_timer = 70;
            }
            else
            {
                efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 81);
                efct_work.efct_com.obj_work.user_timer = 81;
            }
        }

        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSpinDashBlurMain);
        if ((ply_work.player_flag & GMD_PLF_PINBALL_SONIC) != 0 || GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            GmComEfctSetDispOffset(efct_work, 0, 0, 0);
        else
            GmComEfctSetDispOffset(efct_work, 0, 8192, 0);
        efct_work.efct_com.obj_work.obj_3des.speed = ply_work.obj_work.obj_3d.speed[0];
        mtTaskChangeTcbDestructor(efct_work.efct_com.obj_work.tcb, new GSF_TASK_PROCEDURE(gmPlyEfctSpinDashBlurDest));
        ply_work.efct_spin_dash_blur = efct_work.efct_com.obj_work;
        return efct_work;
    }

    private static void GmPlyEfctCreateBrakeImpact(GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateBrakeDust(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & GMD_PLF_WATER) != 0 || (ply_work.player_flag & GMD_PLF_PINBALL_SONIC) != 0)
            return;

        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 0xC);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctBrakeDustMain);
        GmComEfctAddDispOffsetF(efct_work, -8.0f, 16.0f, 0.0f);
    }

    private static void GmPlyEfctCreateJumpDust(GMS_PLAYER_WORK ply_work)
    {
        if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY() || (ply_work.player_flag & GMD_PLF_WATER) != 0)
            return;
        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 41);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctJumpDustMain);
        GmComEfctSetDispOffsetF(efct_work, 0.0f, GMD_PLY_EFCT_OFST_FRONT_PLAYER, 16f);
    }

    private static void GmPlyEfctCreateFootSmoke(GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateSpray(GMS_PLAYER_WORK ply_work)
    {
        GmEfctCmnEsCreate(ply_work.obj_work, 76).efct_com.obj_work.pos.y = g_gm_main_system.water_level << 12;
    }

    private static void GmPlyEfctCreateBubble(GMS_PLAYER_WORK ply_work)
    {
        return;

        // TODO: x86 is pain.
        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 16);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctBubbleMain);
        GmComEfctAddDispOffsetF(efct_work, 0, 0, 10.0f);
    }

    private static void GmPlyEfctWaterCount(GMS_PLAYER_WORK ply_work, uint no)
    {
        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 24 + (int)no);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctWaterCountMain);
        efct_work.obj_3des.command_state = 10U;
        efct_work.efct_com.obj_work.user_timer = 487424;
        GmComEfctAddDispOffset(efct_work, 0, -65536, 1179648);
    }

    private static void GmPlyEfctWaterDeath(GMS_PLAYER_WORK ply_work)
    {
        GmEfctCmnEsCreate(ply_work.obj_work, 31);
    }

    private static void GmPlyEfctCreateRunSpray(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.efct_run_spray != null || MTM_MATH_ABS(ply_work.obj_work.spd_m) < GMD_PLF_EFCT_RUN_SPRAY_MIN_SPD ||
            ((int)ply_work.obj_work.move_flag & 1) == 0)
            return;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork;
        if (MTM_MATH_ABS(ply_work.obj_work.spd_m) >= GMD_PLF_EFCT_RUN_SPRAY_BIG_SPD)
        {
            gmsEffect3DesWork = GmEfctZoneEsCreate(ply_work.obj_work, 2, 30);
            gmsEffect3DesWork.efct_com.obj_work.user_work = 1U;
        }
        else
        {
            gmsEffect3DesWork = GmEfctZoneEsCreate(ply_work.obj_work, 2, 31);
            gmsEffect3DesWork.efct_com.obj_work.user_work = 0U;
        }

        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = g_gm_main_system.water_level << 12;
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctRunSprayMain);
        ply_work.efct_run_spray = gmsEffect3DesWork.efct_com.obj_work;
        mtTaskChangeTcbDestructor(gmsEffect3DesWork.efct_com.obj_work.tcb,
            new GSF_TASK_PROCEDURE(gmPlyEfctRunSprayDest));
    }

    private static void GmPlyEfctCreateHomingImpact(GMS_PLAYER_WORK ply_work)
    {
        var angle = 0;
        if (ply_work.enemy_obj != null)
        {
            GMS_ENEMY_COM_WORK enemyObj = (GMS_ENEMY_COM_WORK)ply_work.enemy_obj;
            var x = FX_FX32_TO_F32(ply_work.obj_work.pos.x - enemyObj.obj_work.pos.x);
            var y = FX_FX32_TO_F32(ply_work.obj_work.pos.y - enemyObj.obj_work.pos.y);

            angle = (int)nnArcTan2(-(double)y, (double)x);
        }

        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 0x21);
        GmComEfctAddDispRotation(efct_work, 0, 0, (ushort)angle);
        efct_work.efct_com.obj_work.user_timer = 0x10;

        efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 0x23);
        GmComEfctAddDispRotation(efct_work, 0, 0, (ushort)angle);
        efct_work.efct_com.obj_work.user_timer = 0x10;

        efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 0x22);
        efct_work.efct_com.obj_work.user_timer = 0x10;
        GmComEfctSetDispOffset(efct_work, 0, 0, 131072);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctHomingImpact01Main);
    }

    private static void GmPlyEfctCreateHomingCursol(GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.enemy_obj == null)
            return;
        GMS_ENEMY_COM_WORK enemyObj = (GMS_ENEMY_COM_WORK)ply_work.enemy_obj;
        if (enemyObj.obj_work.obj_type != 2 && enemyObj.obj_work.obj_type != 3)
            return;
        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(enemyObj.obj_work, GME_EFCT_CMN_IDX.TARGET_S);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctHomingCursolMain);
        OBS_RECT_WORK obsRectWork = enemyObj.rect_work[2];
        efct_work.efct_com.obj_work.user_timer = obsRectWork.rect.left + obsRectWork.rect.right >> 1 << 12;
        efct_work.efct_com.obj_work.user_work = (uint)(obsRectWork.rect.top + obsRectWork.rect.bottom >> 1 << 12);
        if (((int)enemyObj.obj_work.disp_flag & 1) != 0)
            efct_work.efct_com.obj_work.user_timer = -efct_work.efct_com.obj_work.user_timer;
        if (((int)enemyObj.obj_work.disp_flag & 2) != 0)
            efct_work.efct_com.obj_work.user_work = (uint)-(int)efct_work.efct_com.obj_work.user_work;
        GmComEfctSetDispOffset(efct_work, efct_work.efct_com.obj_work.user_timer,
            (int)efct_work.efct_com.obj_work.user_work, 524288);
    }

    private static void GmPlyEfctCreateJumpDash(GMS_PLAYER_WORK ply_work)
    {
        int num = nnArcTan2(-ply_work.obj_work.spd.y, ply_work.obj_work.spd.x);
        GmComEfctAddDispRotation(GmEfctCmnEsCreate(ply_work.obj_work, 36), 0, 0, (ushort)num);
    }

    private static object GmPlyEfctCreateSpinStartBlur(GMS_PLAYER_WORK ply_work)
    {
        // TODO: this looks fucky
        return null;

        if (ply_work.efct_spin_start_blur != null && ((int)ply_work.efct_spin_start_blur.flag & 12) == 0)
            return null;
        GMS_EFFECT_3DES_WORK efct_work;
        if ((ply_work.player_flag & GMD_PLF_SUPER_SONIC) != 0)
        {
            efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 84);
            efct_work.efct_com.obj_work.user_timer = 84;
        }
        else
        {
            efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 75);
            efct_work.efct_com.obj_work.user_timer = 75;
        }

        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSpinStartBlurMain);
        if ((ply_work.player_flag & GMD_PLF_PINBALL_SONIC) != 0 || GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            GmComEfctSetDispOffset(efct_work, 0, 8192, 0);
        else
            GmComEfctSetDispOffset(efct_work, 0, 4096, 0);
        efct_work.efct_com.obj_work.obj_3des.speed = ply_work.obj_work.obj_3d.speed[0];
        mtTaskChangeTcbDestructor(efct_work.efct_com.obj_work.tcb, new GSF_TASK_PROCEDURE(gmPlyEfctSpinStartBlurDest));
        ply_work.efct_spin_dash_cir_blur = efct_work.efct_com.obj_work;
        return efct_work;
    }

    private static object GmPlyEfctCreateSpinJumpBlur(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work = null;
        // unsure what this flag is
        // if(ply_work.efct_spin_jump_blur == null || (ply_work.efct_spin_jump_blur.flag & 0xc ))
        if (ply_work.efct_spin_jump_blur == null ||
            (((GMS_EFFECT_3DES_WORK)ply_work.efct_spin_jump_blur).efct_com.obj_work.obj_3des == null))
        {
            efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 70);
            efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSpinJumpBlurMain);
            ply_work.efct_spin_jump_blur = (OBS_OBJECT_WORK)efct_work;
            gmPlyEfctSpinJumpBlurPosAdj(ply_work);
        }
        else
        {
            gmPlyEfctSpinJumpBlurPosAdj(ply_work);
        }

        return efct_work;
    }

    private static void GmPlyEfctCreateSuperStart(GMS_PLAYER_WORK ply_work)
    {
        GmComEfctSetDispOffset(GmEfctCmnEsCreate(ply_work.obj_work, 85), 0, -12288, 0);
    }

    private static void GmPlyEfctCreateSuperEnd(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(ply_work.obj_work, 80);
        if ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) == 0)
            return;
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
        gmsEffect3DesWork.efct_com.obj_work.pos.x = FXM_FLOAT_TO_FX32(ply_work.truck_mtx_ply_mtn_pos.M03);
        gmsEffect3DesWork.efct_com.obj_work.pos.y = FXM_FLOAT_TO_FX32(-ply_work.truck_mtx_ply_mtn_pos.M13);
        gmsEffect3DesWork.efct_com.obj_work.pos.z = FXM_FLOAT_TO_FX32(ply_work.truck_mtx_ply_mtn_pos.M23);
    }

    private static void GmPlyEfctCreateSuperAuraDeco(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & GMD_PLF_SUPER_SONIC) == 0)
            return;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(ply_work.obj_work, 0);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSuperAuraMain);
        if ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) == 0)
            return;
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
    }

    private static void GmPlyEfctCreateSuperAuraBase(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & GMD_PLF_SUPER_SONIC) == 0)
            return;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(ply_work.obj_work, 1);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSuperAuraMain);
        if ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) == 0)
            return;
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
    }

    private static void GmPlyEfctCreateSuperAuraSpin(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & GMD_PLF_SUPER_SONIC) == 0)
            return;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(ply_work.obj_work, 2);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSuperAuraSpinMain);
        if ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) == 0)
            return;
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
    }

    private static void GmPlyEfctCreateSuperAuraDash(GMS_PLAYER_WORK ply_work)
    {
        if ((ply_work.player_flag & GMD_PLF_SUPER_SONIC) == 0)
            return;
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = GmEfctCmnEsCreate(ply_work.obj_work, 3);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSuperAuraDashMain);
        if ((ply_work.player_flag & GMD_PLF_TRUCK_RIDE) == 0)
            return;
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
    }

    private static void GmPlyEfctCreateSteamPipe(GMS_PLAYER_WORK ply_work)
    {
        int efct_cmn_idx = 90;
        if ((ply_work.player_flag & GMD_PLF_SUPER_SONIC) != 0)
            efct_cmn_idx = 91;
        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(ply_work.obj_work, efct_cmn_idx);
        GmComEfctSetDispOffset(efct_work, 0, 0, 40960);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctSteamPipeMain);
    }

    private static void gmPlyEfctTrailSysMain(MTS_TASK_TCB tcb)
    {
        if (ObjObjectPauseCheck(0U) == 0U)
            amTrailEFUpdate(1);
        if (g_obj.glb_camera_id == -1 || ObjCameraGet(g_obj.glb_camera_id) == null)
            return;
        SNNS_VECTOR disp_pos = new SNNS_VECTOR();
        SNNS_VECTOR snnsVector = new SNNS_VECTOR();
        SNNS_MATRIX dst = new SNNS_MATRIX();
        NNS_RGBA diffuse = new NNS_RGBA(1f, 1f, 1f, 1f);
        SNNS_RGB ambient = new SNNS_RGB(1f, 1f, 1f);
        nnMakeUnitMatrix(ref dst);
        ObjDraw3DNNSetCameraEx(g_obj.glb_camera_id, g_obj.glb_camera_type, 0U);
        ObjCameraDispPosGet(g_obj.glb_camera_id, out disp_pos);
        amVectorSet(ref snnsVector, -dst.M03, -dst.M13, -dst.M23);
        nnAddVector(ref disp_pos, ref snnsVector, ref disp_pos);
        amEffectSetCameraPos(ref disp_pos);
        nnSetPrimitive3DMaterial(ref diffuse, ref ambient, 1f);
        amTrailEFDraw(1, (NNS_TEXLIST)ObjDataGet(18).pData, 0U);
    }

    private static void gmPlyEfctBarrierMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        if ((parentObj.player_flag & GMD_PLF_BARRIER) == 0)
        {
            obj_work.flag |= 8U;
            if (obj_work.user_work == 4U)
                gmPlyEfctCreateBarrierLost(parentObj);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEnd);
        }

        if (((int)parentObj.gmk_flag2 & 2) != 0)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
    }

    private static void gmPlyEfctCreateBarrierLost(GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.gmk_flag2 & 2) != 0)
            return;
        GmComEfctAddDispOffset(GmEfctCmnEsCreate(ply_work.obj_work, 6), 0, 0, 61440);
    }

    private static void gmPlyEfctInvincibleMgrMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).genocide_timer == 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            ++obj_work.user_timer;
            if (obj_work.user_timer >= 15)
            {
                obj_work.user_timer = 0;
                gmPlyEfctCreateInvincibleTail((GMS_PLAYER_WORK)obj_work.parent_obj);
            }

            int num = (int)obj_work.user_work + 1;
            obj_work.user_work = (uint)num;
            if ((int)obj_work.user_work < 70)
                return;
            obj_work.user_work = 0U;
            gmPlyEfctCreateInvincibleCircle((GMS_PLAYER_WORK)obj_work.parent_obj);
        }
    }

    private static void gmPlyEfctCreateInvincibleCircle(GMS_PLAYER_WORK ply_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 42);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctInvincibleCircleMain);
        AppMain.GmComEfctAddDispOffsetF(efct_work, 0.0f, 0.0f, 16f);
        if (ply_work == null || ((int)ply_work.gmk_flag2 & 4) == 0)
            return;
        efct_work.efct_com.obj_work.disp_flag |= 32U;
    }

    private static void gmPlyEfctInvincibleCircleMain(OBS_OBJECT_WORK obj_work)
    {
        GmEfctCmnUpdateInvincibleMainPart((GMS_EFFECT_3DES_WORK)obj_work);
        gmPlyEfctInvincibleMain(obj_work);
    }

    private static void gmPlyEfctCreateInvincibleTail(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(ply_work.obj_work, 43);
        efct_work.efct_com.obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmPlyEfctInvincibleTailMain);
        GmComEfctAddDispOffsetF(efct_work, 0.0f, 0.0f, 16f);
        if (ply_work == null || ((int)ply_work.gmk_flag2 & 4) == 0)
            return;
        efct_work.efct_com.obj_work.disp_flag |= 32U;
    }

    private static void gmPlyEfctInvincibleTailMain(OBS_OBJECT_WORK obj_work)
    {
        GmEfctCmnUpdateInvincibleSubPart((GMS_EFFECT_3DES_WORK)obj_work, obj_work.parent_obj);
        gmPlyEfctInvincibleMain(obj_work);
    }

    private static void gmPlyEfctInvincibleMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        if (parentObj.genocide_timer == 0)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        if (((int)parentObj.gmk_flag2 & 4) != 0)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
        GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctRollDashMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).act_state != 22)
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        else
            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSweatMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        if (13 > parentObj.seq_state || parentObj.seq_state > 15)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEnd);
        }

        GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctRunDustMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).act_state != 20)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctDash1DustMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).act_state != 21)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctDash2DustMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).act_state != 22)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctDash2ImpactMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).act_state != 22)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSpinDustMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 12)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSpinAddDustMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 11)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSpinDashImpactMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer == 0)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        else if (parentObj.seq_state != 10)
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSpinDashDustMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 10)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSpinDashCircleBlurMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (GMS_EFFECT_3DES_WORK)obj_work;
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            obj_work.ofst.Assign(parentObj.obj_work.ofst);
        obj_work.obj_3des.speed = parentObj.obj_work.obj_3d.speed[0];
        obj_work.disp_flag &= 4294967263U;
        obj_work.disp_flag |= parentObj.obj_work.disp_flag & 32U;
        if (parentObj.seq_state != 10 && parentObj.seq_state != 34 &&
            (parentObj.seq_state != 44 && parentObj.seq_state != 37) && !GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        if (((int)obj_work.flag & 12) != 0 || gmsEffect3DesWork.efct_com.obj_work.user_timer != 82 ||
            ((int)parentObj.player_flag & GMD_PLF_SUPER_SONIC) != 0)
            return;
        obj_work.flag |= 8U;
        GMS_EFFECT_3DES_WORK spinDashCircleBlur = (GMS_EFFECT_3DES_WORK)GmPlyEfctCreateSpinDashCircleBlur(parentObj);
        if (spinDashCircleBlur == null)
            return;
        float unitFrame = amEffectGetUnitFrame();
        amEffectSetUnitTime(parentObj.obj_work.obj_3d.frame[0], 60);
        amEffectUpdate(spinDashCircleBlur.obj_3des.ecb);
        amEffectSetUnitTime(unitFrame, 60);
        spinDashCircleBlur.obj_3des.speed = parentObj.obj_work.obj_3d.speed[0];
    }

    private static void gmPlyEfctSpinDashBlurMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (GMS_EFFECT_3DES_WORK)obj_work;
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            obj_work.ofst.Assign(parentObj.obj_work.ofst);
        obj_work.obj_3des.speed = parentObj.obj_work.obj_3d.speed[0];
        if (parentObj.seq_state != 10 && parentObj.seq_state != 34 &&
            (parentObj.seq_state != 37 && parentObj.seq_state != 44) &&
            (parentObj.seq_state != 51 && parentObj.seq_state != 52 &&
             (parentObj.seq_state != 53 && parentObj.seq_state != 48)) &&
            (parentObj.seq_state != 57 && !GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY()))
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        if (((int)obj_work.flag & 12) != 0 ||
            gmsEffect3DesWork.efct_com.obj_work.user_timer != 83 &&
            gmsEffect3DesWork.efct_com.obj_work.user_timer != 81 || ((int)parentObj.player_flag & 16384) != 0)
            return;
        obj_work.flag |= 8U;
        GMS_EFFECT_3DES_WORK spinDashBlur = (GMS_EFFECT_3DES_WORK)GmPlyEfctCreateSpinDashBlur(parentObj,
            gmsEffect3DesWork.efct_com.obj_work.user_timer == 81 ? 0U : 1U);
        if (spinDashBlur == null)
            return;
        float unitFrame = amEffectGetUnitFrame();
        amEffectSetUnitTime(parentObj.obj_work.obj_3d.frame[0], 60);
        amEffectUpdate(spinDashBlur.obj_3des.ecb);
        amEffectSetUnitTime(unitFrame, 60);
        spinDashBlur.obj_3des.speed = parentObj.obj_work.obj_3d.speed[0];
    }

    private static void gmPlyEfctSpinDashBlurDest(MTS_TASK_TCB tcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        GMS_PLAYER_WORK gmsPlayerWork = tcbWork.parent_obj == null || tcbWork.parent_obj.obj_type != 1
            ? g_gm_main_system.ply_work[0]
            : (GMS_PLAYER_WORK)tcbWork.parent_obj;
        if (gmsPlayerWork.efct_spin_dash_blur == tcbWork)
            gmsPlayerWork.efct_spin_dash_blur = null;
        if (gmsPlayerWork.efct_spin_dash_cir_blur == tcbWork)
            gmsPlayerWork.efct_spin_dash_cir_blur = null;
        ObjObjectExit(tcb);
    }

    private static void gmPlyEfctBrakeDustMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 9)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctJumpDustMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 17)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctBubbleMain(OBS_OBJECT_WORK obj_work)
    {
        bool flag = true;
        if (GmMainIsWaterLevel() && obj_work.pos.y >> 12 > g_gm_main_system.water_level + 8)
        {
            flag = false;
            obj_work.spd.y += GMD_PLY_EFCT_BUBBLE_SPD_Y_ACC;
            if (obj_work.spd.y < GMD_PLY_EFCT_BUBBLE_SPD_Y_MAX)
                obj_work.spd.y = GMD_PLY_EFCT_BUBBLE_SPD_Y_MAX;
            if (obj_work.pos.y + obj_work.spd.y < (g_gm_main_system.water_level << 12) + 32768)
                obj_work.spd.y = (g_gm_main_system.water_level << 12) + 32768 - obj_work.pos.y;
            obj_work.user_timer = ObjTimeCountUp(obj_work.user_timer);
            if ((obj_work.user_timer >> 12 & 3) != 0)
                obj_work.spd.x = (mtMathRand() & 4095) - 2048;
        }

        if (flag)
            obj_work.flag |= 4U;
        else
            GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctWaterCountMain(OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        if (obj_work.user_timer == 0 || ((int)parentObj.player_flag & 1024) != 0 ||
            GmMainIsWaterLevel() && parentObj.obj_work.pos.y >> 12 <= g_gm_main_system.water_level ||
            (!GmMainIsWaterLevel() || parentObj.time_air - parentObj.water_timer > 2703360))
            obj_work.flag |= 4U;
        else
            GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctRunSprayMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        if (parentObj == null)
            return;
        bool flag1 = false;
        bool flag2 = false;
        if (obj_work.user_work == 0U)
        {
            if (MTM_MATH_ABS(parentObj.spd_m) < 4096)
                flag1 = true;
            else if (MTM_MATH_ABS(parentObj.spd_m) >= 16384)
                flag1 = true;
        }
        else if (MTM_MATH_ABS(parentObj.spd_m) < 16384)
            flag1 = true;

        if (((int)parentObj.move_flag & 1) == 0 || (parentObj.pos.y >> 12) - 4 >= g_gm_main_system.water_level)
            flag2 = true;
        if (flag1 | flag2)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEnd);
            if (!flag2 && MTM_MATH_ABS(parentObj.spd_m) >= 4096)
            {
                ((GMS_PLAYER_WORK)parentObj).efct_run_spray = null;
                GmPlyEfctCreateRunSpray((GMS_PLAYER_WORK)parentObj);
            }
        }

        obj_work.pos.x = parentObj.pos.x;
        obj_work.pos.y = g_gm_main_system.water_level << 12;
        obj_work.disp_flag &= 4294967292U;
        obj_work.disp_flag |= parentObj.disp_flag & 3U;
    }

    private static void gmPlyEfctRunSprayDest(MTS_TASK_TCB tcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        GMS_PLAYER_WORK gmsPlayerWork = tcbWork.parent_obj == null || tcbWork.parent_obj.obj_type != 1
            ? g_gm_main_system.ply_work[0]
            : (GMS_PLAYER_WORK)tcbWork.parent_obj;
        if (gmsPlayerWork.efct_run_spray == tcbWork)
            gmsPlayerWork.efct_run_spray = null;
        ObjObjectExit(tcb);
    }

    private static void gmPlyEfctHomingImpact01Main(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 19)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEnd);
        }

        GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctHomingCursolMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        if (ply_work.enemy_obj != obj_work.parent_obj || !GmPlySeqCheckAcceptHoming(ply_work))
        {
            GMS_EFFECT_3DES_WORK efct_work = GmEfctCmnEsCreate(obj_work.parent_obj, 94);
            if (((int)obj_work.parent_obj.flag & 12) != 0)
                efct_work.efct_com.obj_work.parent_obj = null;
            GmComEfctSetDispOffset(efct_work, obj_work.user_timer, (int)Convert.ToUInt32(obj_work.user_work), 131072);
            obj_work.flag |= 8U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEnd);
        }

        GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctSpinStartBlurMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (GMS_EFFECT_3DES_WORK)obj_work;
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        obj_work.user_timer = ObjTimeCountDown(obj_work.user_timer);
        gmsEffect3DesWork.obj_3des.speed += 0.05f;
        if (gmsEffect3DesWork.obj_3des.ecb != null)
        {
            gmsEffect3DesWork.obj_3des.ecb.transparency = FX_Div(obj_work.user_timer, 61440) * 256 >> 12;
            if (gmsEffect3DesWork.obj_3des.ecb.transparency > 256)
                gmsEffect3DesWork.obj_3des.ecb.transparency = 256;
        }

        if (obj_work.user_timer == 0 || parentObj.act_state != 28)
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }

        if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            obj_work.ofst.Assign(parentObj.obj_work.ofst);
        GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        if (((int)obj_work.flag & 12) != 0 || gmsEffect3DesWork.efct_com.obj_work.user_work != 84U ||
            ((int)parentObj.player_flag & 16384) != 0)
            return;
        obj_work.flag |= 8U;
        GMS_EFFECT_3DES_WORK spinStartBlur = (GMS_EFFECT_3DES_WORK)GmPlyEfctCreateSpinStartBlur(parentObj);
        if (spinStartBlur == null)
            return;
        float unitFrame = amEffectGetUnitFrame();
        amEffectSetUnitTime(parentObj.obj_work.obj_3d.frame[0], 60);
        amEffectUpdate(spinStartBlur.obj_3des.ecb);
        amEffectSetUnitTime(unitFrame, 60);
        spinStartBlur.obj_3des.speed = gmsEffect3DesWork.obj_3des.speed;
        spinStartBlur.efct_com.obj_work.user_timer = obj_work.user_timer;
    }

    private static void gmPlyEfctSpinStartBlurDest(MTS_TASK_TCB tcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        GMS_PLAYER_WORK gmsPlayerWork = tcbWork.parent_obj == null || tcbWork.parent_obj.obj_type != 1
            ? g_gm_main_system.ply_work[0]
            : (GMS_PLAYER_WORK)tcbWork.parent_obj;
        if (gmsPlayerWork.efct_spin_start_blur == tcbWork)
            gmsPlayerWork.efct_spin_start_blur = null;
        ObjObjectExit(tcb);
    }

    private static void gmPlyEfctSpinJumpBlurMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (GMS_EFFECT_3DES_WORK)obj_work;
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            obj_work.ofst.Assign(parentObj.obj_work.ofst);
        if (((int)parentObj.player_flag & 131072) != 0 && parentObj.seq_state != 0 &&
            (parentObj.seq_state != 1 && parentObj.seq_state != 17) &&
            (parentObj.seq_state != 16 && parentObj.seq_state != 21 &&
             (parentObj.seq_state != 19 && parentObj.seq_state != 66)) &&
            (parentObj.seq_state != 45 && parentObj.seq_state != 46 &&
             (parentObj.seq_state != 49 && parentObj.seq_state != 50) && parentObj.seq_state != 47) ||
            ((int)parentObj.player_flag & 131072) == 0 &&
            (parentObj.seq_state != 17 && parentObj.seq_state != 66 &&
             (parentObj.seq_state != 45 && parentObj.seq_state != 46) &&
             (parentObj.seq_state != 49 && parentObj.seq_state != 50 &&
              (parentObj.seq_state != 42 && parentObj.seq_state != 47)) ||
             parentObj.act_state != 39 && parentObj.act_state != 26 &&
             (parentObj.act_state != 67 && parentObj.act_state != 27)) && !GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        }
        else
        {
            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
            if (GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
                obj_work.dir.z += parentObj.obj_work.dir_fall;
            if (((int)obj_work.flag & 12) == 0 && gmsEffect3DesWork.efct_com.obj_work.user_work == 81U &&
                ((int)parentObj.player_flag & 16384) == 0)
            {
                obj_work.flag |= 8U;
                GMS_EFFECT_3DES_WORK spinJumpBlur = (GMS_EFFECT_3DES_WORK)GmPlyEfctCreateSpinJumpBlur(parentObj);
                if (spinJumpBlur != null)
                {
                    float unitFrame = amEffectGetUnitFrame();
                    amEffectSetUnitTime(parentObj.obj_work.obj_3d.frame[0], 60);
                    amEffectUpdate(spinJumpBlur.obj_3des.ecb);
                    amEffectSetUnitTime(unitFrame, 60);
                    spinJumpBlur.obj_3des.speed = parentObj.obj_work.obj_3d.speed[0];
                }
            }
            else
            {
                int fx32 = FXM_FLOAT_TO_FX32(parentObj.obj_work.obj_3d.frame[0]);
                if ((obj_work.user_timer & 4294963200L) != (fx32 & 4294963200L))
                {
                    int a = (int)((fx32 & 4294963200L) - (obj_work.user_timer & 4294963200L)) % 81920;
                    if (a < 0)
                        a = (a + 81920) % 81920;
                    float unitFrame = amEffectGetUnitFrame();
                    amEffectSetUnitTime(FXM_FX32_TO_FLOAT(a), 60);
                    amEffectUpdate(gmsEffect3DesWork.obj_3des.ecb);
                    amEffectSetUnitTime(unitFrame, 60);
                    obj_work.user_timer = fx32;
                }
            }

            obj_work.user_timer = ObjTimeCountUp(obj_work.user_timer);
            if (obj_work.user_timer < 81920)
                return;
            obj_work.user_timer -= 81920;
        }
    }

    private static void gmPlyEfctSpinJumpBlurPosAdj(GMS_PLAYER_WORK ply_work)
    {
        GMS_EFFECT_3DES_WORK efctSpinJumpBlur = (GMS_EFFECT_3DES_WORK)ply_work.efct_spin_jump_blur;
        if ((ply_work.player_flag & 131072) != 0 || ply_work.act_state == 26 || GSM_MAIN_STAGE_IS_SPSTAGE())
            GmComEfctSetDispOffset(efctSpinJumpBlur, 0, 4096, 0);
        else
            GmComEfctSetDispOffset(efctSpinJumpBlur, 0, -20480, 0);
    }

    private static void gmPlyEfctSpinJumpBlurDest(MTS_TASK_TCB tcb)
    {
        OBS_OBJECT_WORK tcbWork = mtTaskGetTcbWork(tcb);
        GMS_PLAYER_WORK gmsPlayerWork = tcbWork.parent_obj == null || tcbWork.parent_obj.obj_type != 1
            ? g_gm_main_system.ply_work[0]
            : (GMS_PLAYER_WORK)tcbWork.parent_obj;
        if (gmsPlayerWork.efct_spin_jump_blur == tcbWork)
            gmsPlayerWork.efct_spin_jump_blur = null;
        ObjObjectExit(tcb);
    }

    private static void gmPlyEfctSuperAuraMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        if (parentObj == null || ((int)parentObj.player_flag & 16384) == 0 ||
            (((int)parentObj.player_flag & 1024) != 0 || parentObj.act_state == 84))
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        }
        else
        {
            if (((int)parentObj.gmk_flag2 & 128) != 0)
                obj_work.disp_flag |= 32U;
            else
                obj_work.disp_flag &= 4294967263U;
            if (((int)parentObj.player_flag & 262144) != 0)
            {
                obj_work.pos.x = FXM_FLOAT_TO_FX32(parentObj.truck_mtx_ply_mtn_pos.M03);
                obj_work.pos.y = FXM_FLOAT_TO_FX32(-parentObj.truck_mtx_ply_mtn_pos.M13);
                obj_work.pos.z = FXM_FLOAT_TO_FX32(parentObj.truck_mtx_ply_mtn_pos.M23);
            }

            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
            if (!GMM_MAIN_STAGE_IS_ENDING())
                return;
            obj_work.scale.Assign(parentObj.obj_work.scale);
        }
    }

    private static void gmPlyEfctSuperAuraSpinMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        if (parentObj == null ||
            (((int)parentObj.player_flag & 131072) != 0 && parentObj.seq_state != 0 &&
             (parentObj.seq_state != 1 && parentObj.seq_state != 17) &&
             (parentObj.seq_state != 16 && parentObj.seq_state != 21 &&
              (parentObj.seq_state != 19 && parentObj.seq_state != 45)) &&
             (parentObj.seq_state != 46 && parentObj.seq_state != 47) ||
             ((int)parentObj.player_flag & 131072) == 0 &&
             (parentObj.seq_state != 17 && parentObj.seq_state != 45 &&
              (parentObj.seq_state != 46 && parentObj.seq_state != 47) ||
              parentObj.act_state != 39 && parentObj.act_state != 26 && parentObj.act_state != 27)) &&
            !GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        }
        else
        {
            if (((int)parentObj.gmk_flag2 & 128) != 0)
                obj_work.disp_flag |= 32U;
            else
                obj_work.disp_flag &= 4294967263U;
            gmPlyEfctSuperAuraMain(obj_work);
        }
    }

    private static void gmPlyEfctSuperAuraDashMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK parentObj = (GMS_PLAYER_WORK)obj_work.parent_obj;
        if (parentObj == null || parentObj.act_state != 22)
        {
            ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
            GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        }
        else
        {
            if (((int)parentObj.gmk_flag2 & 128) != 0)
                obj_work.disp_flag |= 32U;
            else
                obj_work.disp_flag &= 4294967263U;
            gmPlyEfctSuperAuraMain(obj_work);
        }
    }

    private static void gmPlyEfctSteamPipeMain(OBS_OBJECT_WORK obj_work)
    {
        if (((GMS_PLAYER_WORK)obj_work.parent_obj).obj_work.spd.x == 0)
            return;
        ObjDrawKillAction3DES(obj_work);
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(GmEffectDefaultMainFuncDeleteAtEnd);
    }
}