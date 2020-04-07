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
    private static void GmPlyEfctTrailSysInit()
    {
        if (AppMain.gm_ply_efct_trail_sys_tcb != null)
            return;
        AppMain.gm_ply_efct_trail_sys_tcb = AppMain.MTM_TASK_MAKE_TCB(new AppMain.GSF_TASK_PROCEDURE(AppMain.gmPlyEfctTrailSysMain), (AppMain.GSF_TASK_PROCEDURE)null, 0U, (ushort)0, 8448U, 3, (AppMain.TaskWorkFactoryDelegate)null, "GM_PLY_EF_TRAIL");
    }

    private static void GmPlyEfctTrailSysExit()
    {
        if (AppMain.gm_ply_efct_trail_sys_tcb == null)
            return;
        AppMain.mtTaskClearTcb(AppMain.gm_ply_efct_trail_sys_tcb);
        AppMain.gm_ply_efct_trail_sys_tcb = (AppMain.MTS_TASK_TCB)null;
    }

    private static void GmPlyEfctCreateTrail(AppMain.GMS_PLAYER_WORK ply_work, int efct_type)
    {
    }

    private static void GmPlyEfctCreateBarrier(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_work1 = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 4);
        efct_work1.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctBarrierMain);
        efct_work1.efct_com.obj_work.user_work = 4U;
        AppMain.GmComEfctAddDispOffset(efct_work1, 0, 0, 61440);
        AppMain.GMS_EFFECT_3DES_WORK efct_work2 = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 5);
        efct_work2.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctBarrierMain);
        efct_work2.efct_com.obj_work.user_work = 5U;
        AppMain.GmComEfctAddDispOffset(efct_work2, 0, 0, 61440);
        if (((int)ply_work.gmk_flag2 & 2) == 0)
            return;
        efct_work2.efct_com.obj_work.disp_flag |= 32U;
    }

    private static void GmPlyEfctCreateInvincible(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.gmPlyEfctCreateInvincibleCircle(ply_work);
        AppMain.gmPlyEfctCreateInvincibleTail(ply_work);
        AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_COM_WORK()), ply_work.obj_work, (ushort)0, "GM_PLY_INV_MGR").ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctInvincibleMgrMain);
    }

    private static void GmPlyEfctCreateRollDash(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 147456) != 0)
            return;
        AppMain.GMS_EFFECT_3DES_WORK efct_work;
        if (((int)ply_work.obj_work.disp_flag & 1) != 0)
        {
            efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 52);
            AppMain.GmComEfctSetDispOffsetF(efct_work, 1.5f, 5f, 16f);
            efct_work.obj_3des.ecb.drawObjState = 0U;
        }
        else
        {
            efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 53);
            AppMain.GmComEfctSetDispOffsetF(efct_work, -1.5f, 5f, 16f);
            efct_work.obj_3des.ecb.drawObjState = 0U;
        }
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctRollDashMain);
    }

    private static void GmPlyEfctCreateSweat(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 93);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctSweatMain);
        AppMain.GmComEfctSetDispOffsetF(efct_work, -5f, -10f, 16f);
    }

    private static void GmPlyEfctCreateRunDust(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateDash1Dust(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateDash2Dust(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateDash2Impact(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 147456) != 0)
            return;
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 51);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctDash2ImpactMain);
        AppMain.GmComEfctSetDispOffsetF(efct_work, -8f, 16f, 0.0f);
        efct_work.efct_com.obj_work.parent_ofst.z = AppMain.FXM_FLOAT_TO_FX32(16f);
    }

    private static void GmPlyEfctCreateSpinDust(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GMM_MAIN_GET_ZONE_TYPE() != 2 || ((int)ply_work.player_flag & 67108864) == 0 || (ply_work.obj_work.pos.y >> 12) - 4 < (int)AppMain.g_gm_main_system.water_level ? AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 71) : AppMain.GmEfctZoneEsCreate(ply_work.obj_work, 2, 28);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctSpinDustMain);
        AppMain.GmComEfctSetDispOffsetF(efct_work, -8f, 16f, 0.0f);
        efct_work.efct_com.obj_work.parent_ofst.z = AppMain.FXM_FLOAT_TO_FX32(16f);
    }

    private static void GmPlyEfctCreateSpinAddDust(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateSpinDashImpact(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateSpinDashDust(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static object GmPlyEfctCreateSpinDashCircleBlur(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.efct_spin_dash_cir_blur != null && ((int)ply_work.efct_spin_dash_cir_blur.flag & 12) == 0)
            return (object)null;
        AppMain.GMS_EFFECT_3DES_WORK efct_work;
        if (((int)ply_work.player_flag & 16384) != 0)
        {
            efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 82);
            efct_work.efct_com.obj_work.user_timer = 82;
        }
        else
        {
            efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 73);
            efct_work.efct_com.obj_work.user_timer = 73;
        }
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctSpinDashCircleBlurMain);
        if (((int)ply_work.player_flag & 131072) != 0 || AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            AppMain.GmComEfctSetDispOffset(efct_work, 0, 0, 0);
        else
            AppMain.GmComEfctSetDispOffset(efct_work, 0, 0, 0);
        efct_work.efct_com.obj_work.obj_3des.speed = ply_work.obj_work.obj_3d.speed[0];
        AppMain.mtTaskChangeTcbDestructor(efct_work.efct_com.obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmPlyEfctSpinDashBlurDest));
        ply_work.efct_spin_dash_cir_blur = efct_work.efct_com.obj_work;
        return (object)efct_work;
    }

    private static object GmPlyEfctCreateSpinDashBlur(AppMain.GMS_PLAYER_WORK ply_work, uint type)
    {
        return (object)null;
    }

    private static void GmPlyEfctCreateBrakeImpact(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateBrakeDust(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateJumpDust(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY() || ((int)ply_work.player_flag & 67108864) != 0)
            return;
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 41);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctJumpDustMain);
        AppMain.GmComEfctSetDispOffsetF(efct_work, 0.0f, 16f, 16f);
    }

    private static void GmPlyEfctCreateFootSmoke(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateSpray(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 76).efct_com.obj_work.pos.y = (int)AppMain.g_gm_main_system.water_level << 12;
    }

    private static void GmPlyEfctCreateBubble(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctWaterCount(AppMain.GMS_PLAYER_WORK ply_work, uint no)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 24 + (int)no);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctWaterCountMain);
        efct_work.obj_3des.command_state = 10U;
        efct_work.efct_com.obj_work.user_timer = 487424;
        AppMain.GmComEfctAddDispOffset(efct_work, 0, -65536, 1179648);
    }

    private static void GmPlyEfctWaterDeath(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 31);
    }

    private static void GmPlyEfctCreateRunSpray(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.efct_run_spray != null || AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) < 4096 || ((int)ply_work.obj_work.move_flag & 1) == 0)
            return;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork;
        if (AppMain.MTM_MATH_ABS(ply_work.obj_work.spd_m) >= 16384)
        {
            gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate(ply_work.obj_work, 2, 30);
            gmsEffect3DesWork.efct_com.obj_work.user_work = 1U;
        }
        else
        {
            gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate(ply_work.obj_work, 2, 31);
            gmsEffect3DesWork.efct_com.obj_work.user_work = 0U;
        }
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = (int)AppMain.g_gm_main_system.water_level << 12;
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctRunSprayMain);
        ply_work.efct_run_spray = gmsEffect3DesWork.efct_com.obj_work;
        AppMain.mtTaskChangeTcbDestructor(gmsEffect3DesWork.efct_com.obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmPlyEfctRunSprayDest));
    }

    private static void GmPlyEfctCreateHomingImpact(AppMain.GMS_PLAYER_WORK ply_work)
    {
    }

    private static void GmPlyEfctCreateHomingCursol(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (ply_work.enemy_obj == null)
            return;
        AppMain.GMS_ENEMY_COM_WORK enemyObj = (AppMain.GMS_ENEMY_COM_WORK)ply_work.enemy_obj;
        if (enemyObj.obj_work.obj_type != (ushort)2 && enemyObj.obj_work.obj_type != (ushort)3)
            return;
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(enemyObj.obj_work, 95);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctHomingCursolMain);
        AppMain.OBS_RECT_WORK obsRectWork = enemyObj.rect_work[2];
        efct_work.efct_com.obj_work.user_timer = (int)obsRectWork.rect.left + (int)obsRectWork.rect.right >> 1 << 12;
        efct_work.efct_com.obj_work.user_work = (uint)((int)obsRectWork.rect.top + (int)obsRectWork.rect.bottom >> 1 << 12);
        if (((int)enemyObj.obj_work.disp_flag & 1) != 0)
            efct_work.efct_com.obj_work.user_timer = -efct_work.efct_com.obj_work.user_timer;
        if (((int)enemyObj.obj_work.disp_flag & 2) != 0)
            efct_work.efct_com.obj_work.user_work = (uint)-(int)efct_work.efct_com.obj_work.user_work;
        AppMain.GmComEfctSetDispOffset(efct_work, efct_work.efct_com.obj_work.user_timer, (int)efct_work.efct_com.obj_work.user_work, 524288);
    }

    private static void GmPlyEfctCreateJumpDash(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int num = AppMain.nnArcTan2((double)-ply_work.obj_work.spd.y, (double)ply_work.obj_work.spd.x);
        AppMain.GmComEfctAddDispRotation(AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 36), (ushort)0, (ushort)0, (ushort)num);
    }

    private static object GmPlyEfctCreateSpinStartBlur(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return (object)null;
    }

    private static object GmPlyEfctCreateSpinJumpBlur(AppMain.GMS_PLAYER_WORK ply_work)
    {
        return (object)null;
    }

    private static void GmPlyEfctCreateSuperStart(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GmComEfctSetDispOffset(AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 85), 0, -12288, 0);
    }

    private static void GmPlyEfctCreateSuperEnd(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 80);
        if (((int)ply_work.player_flag & 262144) == 0)
            return;
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
        gmsEffect3DesWork.efct_com.obj_work.pos.x = AppMain.FXM_FLOAT_TO_FX32(ply_work.truck_mtx_ply_mtn_pos.M03);
        gmsEffect3DesWork.efct_com.obj_work.pos.y = AppMain.FXM_FLOAT_TO_FX32(-ply_work.truck_mtx_ply_mtn_pos.M13);
        gmsEffect3DesWork.efct_com.obj_work.pos.z = AppMain.FXM_FLOAT_TO_FX32(ply_work.truck_mtx_ply_mtn_pos.M23);
    }

    private static void GmPlyEfctCreateSuperAuraDeco(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 16384) == 0)
            return;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 0);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctSuperAuraMain);
        if (((int)ply_work.player_flag & 262144) == 0)
            return;
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
    }

    private static void GmPlyEfctCreateSuperAuraBase(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 16384) == 0)
            return;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 1);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctSuperAuraMain);
        if (((int)ply_work.player_flag & 262144) == 0)
            return;
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
    }

    private static void GmPlyEfctCreateSuperAuraSpin(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 16384) == 0)
            return;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 2);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctSuperAuraSpinMain);
        if (((int)ply_work.player_flag & 262144) == 0)
            return;
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
    }

    private static void GmPlyEfctCreateSuperAuraDash(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.player_flag & 16384) == 0)
            return;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 3);
        gmsEffect3DesWork.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctSuperAuraDashMain);
        if (((int)ply_work.player_flag & 262144) == 0)
            return;
        gmsEffect3DesWork.efct_com.obj_work.flag &= 4294966271U;
    }

    private static void GmPlyEfctCreateSteamPipe(AppMain.GMS_PLAYER_WORK ply_work)
    {
        int efct_cmn_idx = 90;
        if (((int)ply_work.player_flag & 16384) != 0)
            efct_cmn_idx = 91;
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, efct_cmn_idx);
        AppMain.GmComEfctSetDispOffset(efct_work, 0, 0, 40960);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctSteamPipeMain);
    }

    private static void gmPlyEfctTrailSysMain(AppMain.MTS_TASK_TCB tcb)
    {
        if (AppMain.ObjObjectPauseCheck(0U) == 0U)
            AppMain.amTrailEFUpdate((ushort)1);
        if (AppMain.g_obj.glb_camera_id == -1 || AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id) == null)
            return;
        AppMain.SNNS_VECTOR disp_pos = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_VECTOR snnsVector = new AppMain.SNNS_VECTOR();
        AppMain.SNNS_MATRIX dst = new AppMain.SNNS_MATRIX();
        AppMain.NNS_RGBA diffuse = new AppMain.NNS_RGBA(1f, 1f, 1f, 1f);
        AppMain.SNNS_RGB ambient = new AppMain.SNNS_RGB(1f, 1f, 1f);
        AppMain.nnMakeUnitMatrix(ref dst);
        AppMain.ObjDraw3DNNSetCameraEx(AppMain.g_obj.glb_camera_id, AppMain.g_obj.glb_camera_type, 0U);
        AppMain.ObjCameraDispPosGet(AppMain.g_obj.glb_camera_id, out disp_pos);
        AppMain.amVectorSet(ref snnsVector, -dst.M03, -dst.M13, -dst.M23);
        AppMain.nnAddVector(ref disp_pos, ref snnsVector, ref disp_pos);
        AppMain.amEffectSetCameraPos(ref disp_pos);
        AppMain.nnSetPrimitive3DMaterial(ref diffuse, ref ambient, 1f);
        AppMain.amTrailEFDraw((ushort)1, (AppMain.NNS_TEXLIST)AppMain.ObjDataGet(18).pData, 0U);
    }

    private static void gmPlyEfctBarrierMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        if (((int)parentObj.player_flag & 268435456) == 0)
        {
            obj_work.flag |= 8U;
            if (obj_work.user_work == 4U)
                AppMain.gmPlyEfctCreateBarrierLost(parentObj);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd);
        }
        if (((int)parentObj.gmk_flag2 & 2) != 0)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
    }

    private static void gmPlyEfctCreateBarrierLost(AppMain.GMS_PLAYER_WORK ply_work)
    {
        if (((int)ply_work.gmk_flag2 & 2) != 0)
            return;
        AppMain.GmComEfctAddDispOffset(AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 6), 0, 0, 61440);
    }

    private static void gmPlyEfctInvincibleMgrMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).genocide_timer == 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            ++obj_work.user_timer;
            if (obj_work.user_timer >= 15)
            {
                obj_work.user_timer = 0;
                AppMain.gmPlyEfctCreateInvincibleTail((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj);
            }
            int num = (int)obj_work.user_work + 1;
            obj_work.user_work = (uint)num;
            if ((int)obj_work.user_work < 70)
                return;
            obj_work.user_work = 0U;
            AppMain.gmPlyEfctCreateInvincibleCircle((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj);
        }
    }

    private static void gmPlyEfctCreateInvincibleCircle(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 42);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctInvincibleCircleMain);
        AppMain.GmComEfctAddDispOffsetF(efct_work, 0.0f, 0.0f, 16f);
        if (ply_work == null || ((int)ply_work.gmk_flag2 & 4) == 0)
            return;
        efct_work.efct_com.obj_work.disp_flag |= 32U;
    }

    private static void gmPlyEfctInvincibleCircleMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GmEfctCmnUpdateInvincibleMainPart((AppMain.GMS_EFFECT_3DES_WORK)obj_work);
        AppMain.gmPlyEfctInvincibleMain(obj_work);
    }

    private static void gmPlyEfctCreateInvincibleTail(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(ply_work.obj_work, 43);
        efct_work.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmPlyEfctInvincibleTailMain);
        AppMain.GmComEfctAddDispOffsetF(efct_work, 0.0f, 0.0f, 16f);
        if (ply_work == null || ((int)ply_work.gmk_flag2 & 4) == 0)
            return;
        efct_work.efct_com.obj_work.disp_flag |= 32U;
    }

    private static void gmPlyEfctInvincibleTailMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GmEfctCmnUpdateInvincibleSubPart((AppMain.GMS_EFFECT_3DES_WORK)obj_work, obj_work.parent_obj);
        AppMain.gmPlyEfctInvincibleMain(obj_work);
    }

    private static void gmPlyEfctInvincibleMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        if (parentObj.genocide_timer == 0)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        if (((int)parentObj.gmk_flag2 & 4) != 0)
            obj_work.disp_flag |= 32U;
        else
            obj_work.disp_flag &= 4294967263U;
        AppMain.GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctRollDashMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).act_state != 22)
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        else
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSweatMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        if (13 > parentObj.seq_state || parentObj.seq_state > 15)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctRunDustMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).act_state != 20)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctDash1DustMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).act_state != 21)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctDash2DustMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).act_state != 22)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctDash2ImpactMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).act_state != 22)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSpinDustMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 12)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSpinAddDustMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 11)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSpinDashImpactMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        if (obj_work.user_timer == 0)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        else if (parentObj.seq_state != 10)
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSpinDashDustMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 10)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctSpinDashCircleBlurMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (AppMain.GMS_EFFECT_3DES_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            obj_work.ofst.Assign(parentObj.obj_work.ofst);
        obj_work.obj_3des.speed = parentObj.obj_work.obj_3d.speed[0];
        obj_work.disp_flag &= 4294967263U;
        obj_work.disp_flag |= parentObj.obj_work.disp_flag & 32U;
        if (parentObj.seq_state != 10 && parentObj.seq_state != 34 && (parentObj.seq_state != 44 && parentObj.seq_state != 37) && !AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        if (((int)obj_work.flag & 12) != 0 || gmsEffect3DesWork.efct_com.obj_work.user_timer != 82 || ((int)parentObj.player_flag & 16384) != 0)
            return;
        obj_work.flag |= 8U;
        AppMain.GMS_EFFECT_3DES_WORK spinDashCircleBlur = (AppMain.GMS_EFFECT_3DES_WORK)AppMain.GmPlyEfctCreateSpinDashCircleBlur(parentObj);
        if (spinDashCircleBlur == null)
            return;
        float unitFrame = AppMain.amEffectGetUnitFrame();
        AppMain.amEffectSetUnitTime(parentObj.obj_work.obj_3d.frame[0], 60);
        AppMain.amEffectUpdate(spinDashCircleBlur.obj_3des.ecb);
        AppMain.amEffectSetUnitTime(unitFrame, 60);
        spinDashCircleBlur.obj_3des.speed = parentObj.obj_work.obj_3d.speed[0];
    }

    private static void gmPlyEfctSpinDashBlurMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (AppMain.GMS_EFFECT_3DES_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            obj_work.ofst.Assign(parentObj.obj_work.ofst);
        obj_work.obj_3des.speed = parentObj.obj_work.obj_3d.speed[0];
        if (parentObj.seq_state != 10 && parentObj.seq_state != 34 && (parentObj.seq_state != 37 && parentObj.seq_state != 44) && (parentObj.seq_state != 51 && parentObj.seq_state != 52 && (parentObj.seq_state != 53 && parentObj.seq_state != 48)) && (parentObj.seq_state != 57 && !AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY()))
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        if (((int)obj_work.flag & 12) != 0 || gmsEffect3DesWork.efct_com.obj_work.user_timer != 83 && gmsEffect3DesWork.efct_com.obj_work.user_timer != 81 || ((int)parentObj.player_flag & 16384) != 0)
            return;
        obj_work.flag |= 8U;
        AppMain.GMS_EFFECT_3DES_WORK spinDashBlur = (AppMain.GMS_EFFECT_3DES_WORK)AppMain.GmPlyEfctCreateSpinDashBlur(parentObj, gmsEffect3DesWork.efct_com.obj_work.user_timer == 81 ? 0U : 1U);
        if (spinDashBlur == null)
            return;
        float unitFrame = AppMain.amEffectGetUnitFrame();
        AppMain.amEffectSetUnitTime(parentObj.obj_work.obj_3d.frame[0], 60);
        AppMain.amEffectUpdate(spinDashBlur.obj_3des.ecb);
        AppMain.amEffectSetUnitTime(unitFrame, 60);
        spinDashBlur.obj_3des.speed = parentObj.obj_work.obj_3d.speed[0];
    }

    private static void gmPlyEfctSpinDashBlurDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = tcbWork.parent_obj == null || tcbWork.parent_obj.obj_type != (ushort)1 ? AppMain.g_gm_main_system.ply_work[0] : (AppMain.GMS_PLAYER_WORK)tcbWork.parent_obj;
        if (gmsPlayerWork.efct_spin_dash_blur == tcbWork)
            gmsPlayerWork.efct_spin_dash_blur = (AppMain.OBS_OBJECT_WORK)null;
        if (gmsPlayerWork.efct_spin_dash_cir_blur == tcbWork)
            gmsPlayerWork.efct_spin_dash_cir_blur = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.ObjObjectExit(tcb);
    }

    private static void gmPlyEfctBrakeDustMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 9)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctJumpDustMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 17)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void gmPlyEfctBubbleMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        bool flag = true;
        if (AppMain.GmMainIsWaterLevel() && obj_work.pos.y >> 12 > (int)AppMain.g_gm_main_system.water_level + 8)
        {
            flag = false;
            obj_work.spd.y += -64;
            if (obj_work.spd.y < -65536)
                obj_work.spd.y = -65536;
            if (obj_work.pos.y + obj_work.spd.y < ((int)AppMain.g_gm_main_system.water_level << 12) + 32768)
                obj_work.spd.y = ((int)AppMain.g_gm_main_system.water_level << 12) + 32768 - obj_work.pos.y;
            obj_work.user_timer = AppMain.ObjTimeCountUp(obj_work.user_timer);
            if ((obj_work.user_timer >> 12 & 3) != 0)
                obj_work.spd.x = ((int)AppMain.mtMathRand() & 4095) - 2048;
        }
        if (flag)
            obj_work.flag |= 4U;
        else
            AppMain.GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctWaterCountMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        if (obj_work.user_timer == 0 || ((int)parentObj.player_flag & 1024) != 0 || AppMain.GmMainIsWaterLevel() && parentObj.obj_work.pos.y >> 12 <= (int)AppMain.g_gm_main_system.water_level || (!AppMain.GmMainIsWaterLevel() || parentObj.time_air - parentObj.water_timer > 2703360))
            obj_work.flag |= 4U;
        else
            AppMain.GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctRunSprayMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        if (parentObj == null)
            return;
        bool flag1 = false;
        bool flag2 = false;
        if (obj_work.user_work == 0U)
        {
            if (AppMain.MTM_MATH_ABS(parentObj.spd_m) < 4096)
                flag1 = true;
            else if (AppMain.MTM_MATH_ABS(parentObj.spd_m) >= 16384)
                flag1 = true;
        }
        else if (AppMain.MTM_MATH_ABS(parentObj.spd_m) < 16384)
            flag1 = true;
        if (((int)parentObj.move_flag & 1) == 0 || (parentObj.pos.y >> 12) - 4 >= (int)AppMain.g_gm_main_system.water_level)
            flag2 = true;
        if (flag1 | flag2)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd);
            if (!flag2 && AppMain.MTM_MATH_ABS(parentObj.spd_m) >= 4096)
            {
                ((AppMain.GMS_PLAYER_WORK)parentObj).efct_run_spray = (AppMain.OBS_OBJECT_WORK)null;
                AppMain.GmPlyEfctCreateRunSpray((AppMain.GMS_PLAYER_WORK)parentObj);
            }
        }
        obj_work.pos.x = parentObj.pos.x;
        obj_work.pos.y = (int)AppMain.g_gm_main_system.water_level << 12;
        obj_work.disp_flag &= 4294967292U;
        obj_work.disp_flag |= parentObj.disp_flag & 3U;
    }

    private static void gmPlyEfctRunSprayDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = tcbWork.parent_obj == null || tcbWork.parent_obj.obj_type != (ushort)1 ? AppMain.g_gm_main_system.ply_work[0] : (AppMain.GMS_PLAYER_WORK)tcbWork.parent_obj;
        if (gmsPlayerWork.efct_run_spray == tcbWork)
            gmsPlayerWork.efct_run_spray = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.ObjObjectExit(tcb);
    }

    private static void gmPlyEfctHomingImpact01Main(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).seq_state != 19)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctHomingCursolMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        if (ply_work.enemy_obj != obj_work.parent_obj || !AppMain.GmPlySeqCheckAcceptHoming(ply_work))
        {
            AppMain.GMS_EFFECT_3DES_WORK efct_work = AppMain.GmEfctCmnEsCreate(obj_work.parent_obj, 94);
            if (((int)obj_work.parent_obj.flag & 12) != 0)
                efct_work.efct_com.obj_work.parent_obj = (AppMain.OBS_OBJECT_WORK)null;
            AppMain.GmComEfctSetDispOffset(efct_work, obj_work.user_timer, (int)Convert.ToUInt32(obj_work.user_work), 131072);
            obj_work.flag |= 8U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd);
        }
        AppMain.GmEffectDefaultMainFuncDeleteAtEnd(obj_work);
    }

    private static void gmPlyEfctSpinStartBlurMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (AppMain.GMS_EFFECT_3DES_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        obj_work.user_timer = AppMain.ObjTimeCountDown(obj_work.user_timer);
        gmsEffect3DesWork.obj_3des.speed += 0.05f;
        if (gmsEffect3DesWork.obj_3des.ecb != null)
        {
            gmsEffect3DesWork.obj_3des.ecb.transparency = AppMain.FX_Div(obj_work.user_timer, 61440) * 256 >> 12;
            if (gmsEffect3DesWork.obj_3des.ecb.transparency > 256)
                gmsEffect3DesWork.obj_3des.ecb.transparency = 256;
        }
        if (obj_work.user_timer == 0 || parentObj.act_state != 28)
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
        }
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            obj_work.ofst.Assign(parentObj.obj_work.ofst);
        AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        if (((int)obj_work.flag & 12) != 0 || gmsEffect3DesWork.efct_com.obj_work.user_work != 84U || ((int)parentObj.player_flag & 16384) != 0)
            return;
        obj_work.flag |= 8U;
        AppMain.GMS_EFFECT_3DES_WORK spinStartBlur = (AppMain.GMS_EFFECT_3DES_WORK)AppMain.GmPlyEfctCreateSpinStartBlur(parentObj);
        if (spinStartBlur == null)
            return;
        float unitFrame = AppMain.amEffectGetUnitFrame();
        AppMain.amEffectSetUnitTime(parentObj.obj_work.obj_3d.frame[0], 60);
        AppMain.amEffectUpdate(spinStartBlur.obj_3des.ecb);
        AppMain.amEffectSetUnitTime(unitFrame, 60);
        spinStartBlur.obj_3des.speed = gmsEffect3DesWork.obj_3des.speed;
        spinStartBlur.efct_com.obj_work.user_timer = obj_work.user_timer;
    }

    private static void gmPlyEfctSpinStartBlurDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = tcbWork.parent_obj == null || tcbWork.parent_obj.obj_type != (ushort)1 ? AppMain.g_gm_main_system.ply_work[0] : (AppMain.GMS_PLAYER_WORK)tcbWork.parent_obj;
        if (gmsPlayerWork.efct_spin_start_blur == tcbWork)
            gmsPlayerWork.efct_spin_start_blur = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.ObjObjectExit(tcb);
    }

    private static void gmPlyEfctSpinJumpBlurMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = (AppMain.GMS_EFFECT_3DES_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            obj_work.ofst.Assign(parentObj.obj_work.ofst);
        if (((int)parentObj.player_flag & 131072) != 0 && parentObj.seq_state != 0 && (parentObj.seq_state != 1 && parentObj.seq_state != 17) && (parentObj.seq_state != 16 && parentObj.seq_state != 21 && (parentObj.seq_state != 19 && parentObj.seq_state != 66)) && (parentObj.seq_state != 45 && parentObj.seq_state != 46 && (parentObj.seq_state != 49 && parentObj.seq_state != 50) && parentObj.seq_state != 47) || ((int)parentObj.player_flag & 131072) == 0 && (parentObj.seq_state != 17 && parentObj.seq_state != 66 && (parentObj.seq_state != 45 && parentObj.seq_state != 46) && (parentObj.seq_state != 49 && parentObj.seq_state != 50 && (parentObj.seq_state != 42 && parentObj.seq_state != 47)) || parentObj.act_state != 39 && parentObj.act_state != 26 && (parentObj.act_state != 67 && parentObj.act_state != 27)) && !AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
        {
            obj_work.flag |= 8U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        }
        else
        {
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
            if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
                obj_work.dir.z += parentObj.obj_work.dir_fall;
            if (((int)obj_work.flag & 12) == 0 && gmsEffect3DesWork.efct_com.obj_work.user_work == 81U && ((int)parentObj.player_flag & 16384) == 0)
            {
                obj_work.flag |= 8U;
                AppMain.GMS_EFFECT_3DES_WORK spinJumpBlur = (AppMain.GMS_EFFECT_3DES_WORK)AppMain.GmPlyEfctCreateSpinJumpBlur(parentObj);
                if (spinJumpBlur != null)
                {
                    float unitFrame = AppMain.amEffectGetUnitFrame();
                    AppMain.amEffectSetUnitTime(parentObj.obj_work.obj_3d.frame[0], 60);
                    AppMain.amEffectUpdate(spinJumpBlur.obj_3des.ecb);
                    AppMain.amEffectSetUnitTime(unitFrame, 60);
                    spinJumpBlur.obj_3des.speed = parentObj.obj_work.obj_3d.speed[0];
                }
            }
            else
            {
                int fx32 = AppMain.FXM_FLOAT_TO_FX32(parentObj.obj_work.obj_3d.frame[0]);
                if (((long)obj_work.user_timer & 4294963200L) != ((long)fx32 & 4294963200L))
                {
                    int a = (int)(((long)fx32 & 4294963200L) - ((long)obj_work.user_timer & 4294963200L)) % 81920;
                    if (a < 0)
                        a = (a + 81920) % 81920;
                    float unitFrame = AppMain.amEffectGetUnitFrame();
                    AppMain.amEffectSetUnitTime(AppMain.FXM_FX32_TO_FLOAT(a), 60);
                    AppMain.amEffectUpdate(gmsEffect3DesWork.obj_3des.ecb);
                    AppMain.amEffectSetUnitTime(unitFrame, 60);
                    obj_work.user_timer = fx32;
                }
            }
            obj_work.user_timer = AppMain.ObjTimeCountUp(obj_work.user_timer);
            if (obj_work.user_timer < 81920)
                return;
            obj_work.user_timer -= 81920;
        }
    }

    private static void gmPlyEfctSpinJumpBlurPosAdj(AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.GMS_EFFECT_3DES_WORK efctSpinJumpBlur = (AppMain.GMS_EFFECT_3DES_WORK)ply_work.efct_spin_jump_blur;
        if (((int)ply_work.player_flag & 131072) != 0 || ply_work.act_state == 26 || AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
            AppMain.GmComEfctSetDispOffset(efctSpinJumpBlur, 0, 4096, 0);
        else
            AppMain.GmComEfctSetDispOffset(efctSpinJumpBlur, 0, -20480, 0);
    }

    private static void gmPlyEfctSpinJumpBlurDest(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = tcbWork.parent_obj == null || tcbWork.parent_obj.obj_type != (ushort)1 ? AppMain.g_gm_main_system.ply_work[0] : (AppMain.GMS_PLAYER_WORK)tcbWork.parent_obj;
        if (gmsPlayerWork.efct_spin_jump_blur == tcbWork)
            gmsPlayerWork.efct_spin_jump_blur = (AppMain.OBS_OBJECT_WORK)null;
        AppMain.ObjObjectExit(tcb);
    }

    private static void gmPlyEfctSuperAuraMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        if (parentObj == null || ((int)parentObj.player_flag & 16384) == 0 || (((int)parentObj.player_flag & 1024) != 0 || parentObj.act_state == 84))
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        }
        else
        {
            if (((int)parentObj.gmk_flag2 & 128) != 0)
                obj_work.disp_flag |= 32U;
            else
                obj_work.disp_flag &= 4294967263U;
            if (((int)parentObj.player_flag & 262144) != 0)
            {
                obj_work.pos.x = AppMain.FXM_FLOAT_TO_FX32(parentObj.truck_mtx_ply_mtn_pos.M03);
                obj_work.pos.y = AppMain.FXM_FLOAT_TO_FX32(-parentObj.truck_mtx_ply_mtn_pos.M13);
                obj_work.pos.z = AppMain.FXM_FLOAT_TO_FX32(parentObj.truck_mtx_ply_mtn_pos.M23);
            }
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
            if (!AppMain.GMM_MAIN_STAGE_IS_ENDING())
                return;
            obj_work.scale.Assign(parentObj.obj_work.scale);
        }
    }

    private static void gmPlyEfctSuperAuraSpinMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        if (parentObj == null || (((int)parentObj.player_flag & 131072) != 0 && parentObj.seq_state != 0 && (parentObj.seq_state != 1 && parentObj.seq_state != 17) && (parentObj.seq_state != 16 && parentObj.seq_state != 21 && (parentObj.seq_state != 19 && parentObj.seq_state != 45)) && (parentObj.seq_state != 46 && parentObj.seq_state != 47) || ((int)parentObj.player_flag & 131072) == 0 && (parentObj.seq_state != 17 && parentObj.seq_state != 45 && (parentObj.seq_state != 46 && parentObj.seq_state != 47) || parentObj.act_state != 39 && parentObj.act_state != 26 && parentObj.act_state != 27)) && !AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        }
        else
        {
            if (((int)parentObj.gmk_flag2 & 128) != 0)
                obj_work.disp_flag |= 32U;
            else
                obj_work.disp_flag &= 4294967263U;
            AppMain.gmPlyEfctSuperAuraMain(obj_work);
        }
    }

    private static void gmPlyEfctSuperAuraDashMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK parentObj = (AppMain.GMS_PLAYER_WORK)obj_work.parent_obj;
        if (parentObj == null || parentObj.act_state != 22)
        {
            AppMain.ObjDrawKillAction3DES(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ);
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
        }
        else
        {
            if (((int)parentObj.gmk_flag2 & 128) != 0)
                obj_work.disp_flag |= 32U;
            else
                obj_work.disp_flag &= 4294967263U;
            AppMain.gmPlyEfctSuperAuraMain(obj_work);
        }
    }

    private static void gmPlyEfctSteamPipeMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_PLAYER_WORK)obj_work.parent_obj).obj_work.spd.x == 0)
            return;
        AppMain.ObjDrawKillAction3DES(obj_work);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.GmEffectDefaultMainFuncDeleteAtEnd);
    }

}