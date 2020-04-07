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
    private static void GmGmkFlipperBuild()
    {
        AppMain.g_gm_gmk_flipper_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(867), AppMain.GmGameDatGetGimmickData(868), 0U);
    }

    private static void GmGmkFlipperFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(867);
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_flipper_obj_3d_list, gimmickData.file_num);
        AppMain.g_gm_gmk_flipper_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkFlipperInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int num = AppMain.gmGmkFlipperCalcType((int)eve_rec.id);
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkFlipperLoadObj(eve_rec, pos_x, pos_y, num).ene_com.obj_work;
        AppMain.gmGmkFlipperInit(objWork, num);
        return objWork;
    }

    private static uint gmGmkFlipperGameSystemGetSyncTime()
    {
        return AppMain.g_gm_main_system.sync_time;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkFlipperLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = type != 2 ? (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_FLIPPER_WORK()), "GMK_FLIPPER_U") : (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_RIDE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_FLIPPER_WORK()), "GMK_FLIPPER_LR");
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        gmsEnemy3DWork.ene_com.rect_work[1].flag &= 4294967291U;
        return gmsEnemy3DWork;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkFlipperLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkFlipperLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        AppMain.OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index = AppMain.g_gm_gmk_flipper_model_id[type];
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.g_gm_gmk_flipper_obj_3d_list[index], gmsEnemy3DWork.obj_3d);
        AppMain.OBS_DATA_WORK data_work = AppMain.ObjDataGet(869);
        AppMain.ObjObjectAction3dNNMaterialMotionLoad(objWork, 0, data_work, (string)null, 0, (object)null);
        if (type == 2)
        {
            gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
            gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)16;
            gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)8;
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)((int)-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
            gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)-7;
            gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 32U;
        }
        return gmsEnemy3DWork;
    }

    private static void gmGmkFlipperInit(AppMain.OBS_OBJECT_WORK obj_work, int flipper_type)
    {
        AppMain.gmGmkFlipperSetRect((AppMain.GMS_ENEMY_3D_WORK)obj_work, flipper_type);
        obj_work.move_flag = 8448U;
        obj_work.dir.z = AppMain.g_gm_gmk_flipper_angle_z[flipper_type];
        if (flipper_type == 0)
            obj_work.user_flag = 1U;
        obj_work.disp_flag |= 4194304U;
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, AppMain.g_gm_gmk_flipper_mat_motion_id[flipper_type]);
        obj_work.disp_flag |= 4194308U;
        obj_work.pos.z = -122880;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkFlipperDrawFunc);
        AppMain.gmGmkFlipperChangeModeWait(obj_work);
    }

    private static void gmGmkFlipperSetRect(AppMain.GMS_ENEMY_3D_WORK gimmick_work, int flipper_type)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        switch (flipper_type)
        {
            case 0:
                pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkFlipperDefFuncU);
                break;
            case 1:
                pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkFlipperDefFuncU);
                break;
            case 2:
                pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkFlipperDefFuncLR);
                break;
        }
        AppMain.ObjRectWorkZSet(pRec, AppMain.g_gmk_flipper_rect[flipper_type][0], AppMain.g_gmk_flipper_rect[flipper_type][1], (short)-500, AppMain.g_gmk_flipper_rect[flipper_type][2], AppMain.g_gmk_flipper_rect[flipper_type][3], (short)500);
        pRec.flag |= 1024U;
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
    }

    private static void gmGmkFlipperDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (obj3d.motion != null)
        {
            float startFrame = AppMain.amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
            float num = AppMain.amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - startFrame;
            float syncTime = (float)AppMain.gmGmkFlipperGameSystemGetSyncTime();
            obj3d.mat_frame = syncTime % num;
        }
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static int gmGmkFlipperCalcType(int id)
    {
        return id - 169;
    }

    private static void gmGmkFlipperDefPlayer(
      AppMain.OBS_RECT_WORK gimmick_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)parentObj1;
        AppMain.OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)parentObj2;
        if (ply_work.seq_state >= 60)
            return;
        int num1 = AppMain.gmGmkFlipperCalcType((int)gmsEnemy3DWork.ene_com.eve_rec.id);
        gmsEnemy3DWork.ene_com.target_obj = parentObj2;
        int num2 = AppMain.gmGmkFlipperCalcRideOffsetY(parentObj2.pos.x, parentObj1, num1);
        if (parentObj1.pos.y + num2 < parentObj2.pos.y)
        {
            int num3 = parentObj2.pos.x - parentObj1.pos.x;
            if (num1 == 1)
                num3 = -num3;
            if (num3 < 0)
                parentObj2.spd.x = 0;
            bool flag_no_recover_homing = false;
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
                flag_no_recover_homing = true;
            AppMain.GmPlySeqInitPinballAir(ply_work, parentObj2.spd.x, 8192, 5, flag_no_recover_homing);
        }
        else if (AppMain.gmGmkFlipperCheckRect(parentObj1.pos, parentObj2.pos, num1) == 0)
        {
            gimmick_rect.flag &= 4294966271U;
        }
        else
        {
            AppMain.gmGmkFlipperChangeModeReady(parentObj1);
            gimmick_rect.flag |= 1024U;
            AppMain.gmGmkFlipperSetRideSpeed(parentObj2, parentObj1, num1);
            AppMain.GmPlySeqInitFlipper((AppMain.GMS_PLAYER_WORK)parentObj2, parentObj2.spd.x, parentObj2.spd.y, gmsEnemy3DWork.ene_com);
            int num3 = num2;
            int num4 = ((int)ply_work.player_flag & 131072) == 0 ? num3 - 36864 : num3 - 61440;
            parentObj2.pos.y = parentObj1.pos.y + num4;
        }
    }

    private static void gmGmkFlipperDefEnemy(
      AppMain.OBS_RECT_WORK gimmick_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        AppMain.OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (((AppMain.GMS_ENEMY_3D_WORK)parentObj2).ene_com.eve_rec.id != (ushort)316 || parentObj1.pos.y < parentObj2.pos.y)
            return;
        parentObj2.spd.y = -parentObj2.spd.y;
        if (AppMain.MTM_MATH_ABS(parentObj2.spd.x) >= 256)
            return;
        parentObj2.spd.x = 256;
    }

    private static void gmGmkFlipperDefFuncU(
      AppMain.OBS_RECT_WORK gimmick_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj = target_rect.parent_obj;
        if (parentObj.obj_type == (ushort)1)
        {
            AppMain.gmGmkFlipperDefPlayer(gimmick_rect, target_rect);
        }
        else
        {
            if (parentObj.obj_type != (ushort)2)
                return;
            AppMain.gmGmkFlipperDefEnemy(gimmick_rect, target_rect);
        }
    }

    private static void gmGmkFlipperDefFuncLR(
      AppMain.OBS_RECT_WORK gimmick_rect,
      AppMain.OBS_RECT_WORK target_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)parentObj1;
        AppMain.OBS_OBJECT_WORK parentObj2 = target_rect.parent_obj;
        if (parentObj2.obj_type != (ushort)1)
            return;
        gmsEnemy3DWork.ene_com.target_obj = parentObj2;
        int v1_1 = 86016;
        int v1_2 = 0;
        if ((uint)gmsEnemy3DWork.ene_com.eve_rec.width * 1000U == 0U)
        {
            if (parentObj2.pos.x < parentObj1.pos.x)
            {
                parentObj1.user_flag = 0U;
                v1_1 *= -1;
            }
            else
                parentObj1.user_flag = 1U;
        }
        else
        {
            parentObj1.user_flag = 0U;
            v1_1 *= -1;
        }
        int v2_1 = AppMain.FX_F32_TO_FX32((float)((100.0 + (double)gmsEnemy3DWork.ene_com.eve_rec.left) * 0.00999999977648258));
        if (v2_1 < 0)
            v2_1 = 0;
        int v2_2 = AppMain.FX_F32_TO_FX32((float)((100.0 + (double)gmsEnemy3DWork.ene_com.eve_rec.top) * 0.00999999977648258));
        if (v2_2 < 0)
            v2_2 = 0;
        int num1 = AppMain.FX_Mul(v1_1, v2_1);
        int num2 = AppMain.FX_Mul(v1_2, v2_2);
        AppMain.gmGmkFlipperChangeModeHit(parentObj1);
        int no_spddown_timer = 12;
        if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 2) != 0)
            no_spddown_timer = 180;
        AppMain.GmPlySeqInitPinball((AppMain.GMS_PLAYER_WORK)parentObj2, num1, num2, no_spddown_timer);
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate(parentObj1, 16);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj2.pos.x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj2.pos.y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
        gmsEffect3DesWork.efct_com.obj_work.dir.z = (ushort)(AppMain.nnArcTan2((double)AppMain.FX_FX32_TO_F32(num2), (double)AppMain.FX_FX32_TO_F32(num1)) - 16384);
    }

    private static void gmGmkFlipperChangeModeWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int index = AppMain.gmGmkFlipperCalcType((int)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        obj_work.user_work = (uint)AppMain.g_gm_gmk_flipper_angle_z[index];
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkFlipperMainWait);
    }

    private static void gmGmkFlipperChangeModeReady(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int index = AppMain.gmGmkFlipperCalcType((int)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        obj_work.user_work = (uint)AppMain.g_gm_gmk_flipper_angle_z[index];
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkFlipperMainReady);
    }

    private static void gmGmkFlipperChangeModeHit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int index = AppMain.gmGmkFlipperCalcType((int)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        ushort num1 = AppMain.g_gm_gmk_flipper_angle_z[index];
        ushort num2 = obj_work.user_flag == 0U ? (ushort)((uint)num1 + (uint)(ushort)AppMain.NNM_DEGtoA16(70f)) : (ushort)((uint)num1 + (uint)(ushort)AppMain.NNM_DEGtoA16(-70f));
        obj_work.user_work = (uint)num2;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkFlipperMainHit);
        AppMain.GmSoundPlaySE("Casino2");
    }

    private static void gmGmkFlipperChangeModeHook(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int index = AppMain.gmGmkFlipperCalcType((int)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        ushort num1 = AppMain.g_gm_gmk_flipper_angle_z[index];
        ushort num2 = obj_work.user_flag == 0U ? (ushort)((uint)num1 + (uint)(ushort)AppMain.NNM_DEGtoA16(70f)) : (ushort)((uint)num1 + (uint)(ushort)AppMain.NNM_DEGtoA16(-70f));
        obj_work.user_work = (uint)num2;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkFlipperMainHook);
    }

    private static void gmGmkFlipperChangeModeOpen(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        int index = AppMain.gmGmkFlipperCalcType((int)gmsEnemy3DWork.ene_com.eve_rec.id);
        obj_work.user_work = (uint)AppMain.g_gm_gmk_flipper_angle_z[index];
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.dir.z = (ushort)0;
        gmsEnemy3DWork.ene_com.rect_work[2].ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
    }

    private static void gmGmkFlipperMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.gmGmkFlipperUpdateAngle(obj_work);
        if (AppMain.gmGmkFlipperCalcType((int)gmsEnemy3DWork.ene_com.eve_rec.id) != 2 || AppMain.gmGmkFlipperCheckScore(obj_work) == 0)
            return;
        AppMain.gmGmkFlipperChangeModeOpen(obj_work);
    }

    private static void gmGmkFlipperMainReady(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        AppMain.gmGmkFlipperUpdateAngle(obj_work);
        if (true)
        {
            AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
            if (AppMain.gmGmkFlipperCheckControlPlayer() == 0)
            {
                AppMain.gmGmkFlipperChangeModeWait(obj_work);
                return;
            }
            if (AppMain.gmGmkFlipperCheckKeyHit(obj_work, gmsPlayerWork) == 0)
                return;
            if (AppMain.gmGmkFlipperCheckHook(obj_work) != 0)
            {
                targetObj.spd.x = 0;
                targetObj.spd.y = 0;
                AppMain.gmGmkFlipperChangeModeHook(obj_work);
                return;
            }
            int num1 = 12288;
            int v1_1 = -53248;
            if (AppMain.gmGmkFlipperCalcType((int)gmsEnemy3DWork.ene_com.eve_rec.id) == 1)
                num1 = -num1;
            int v1_2 = num1 + (targetObj.pos.x - obj_work.pos.x >> 2);
            int num2 = (102400 - AppMain.MTM_MATH_ABS(targetObj.pos.x - obj_work.pos.x)) / 10;
            if (num2 > 0)
                v1_1 += num2;
            int v2_1 = AppMain.FX_F32_TO_FX32((float)((100.0 + (double)gmsEnemy3DWork.ene_com.eve_rec.left) * 0.00999999977648258));
            if (v2_1 < 0)
                v2_1 = 0;
            int v2_2 = AppMain.FX_F32_TO_FX32((float)((100.0 + (double)gmsEnemy3DWork.ene_com.eve_rec.top) * 0.00999999977648258));
            if (v2_2 < 0)
                v2_2 = 0;
            int num3 = AppMain.FX_Mul(v1_2, v2_1);
            int num4 = AppMain.FX_Mul(v1_1, v2_2);
            int flag_no_recover_homing = 0;
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 1) != 0)
                flag_no_recover_homing = 1;
            int no_spddown_timer = 0;
            if (((int)gmsEnemy3DWork.ene_com.eve_rec.flag & 2) != 0)
                no_spddown_timer = 30;
            AppMain.GmPlayerSetAtk(gmsPlayerWork);
            AppMain.GmPlySeqInitPinballAir(gmsPlayerWork, num3, num4, 5, flag_no_recover_homing, no_spddown_timer);
            AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate(obj_work, 16);
            gmsEffect3DesWork.efct_com.obj_work.pos.x = targetObj.pos.x;
            gmsEffect3DesWork.efct_com.obj_work.pos.y = targetObj.pos.y;
            gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
            gmsEffect3DesWork.efct_com.obj_work.dir.z = (ushort)(AppMain.nnArcTan2((double)AppMain.FX_FX32_TO_F32(num4), (double)AppMain.FX_FX32_TO_F32(num3)) - 16384);
        }
        AppMain.gmGmkFlipperChangeModeHit(obj_work);
    }

    private static void gmGmkFlipperMainHit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.gmGmkFlipperUpdateAngle(obj_work) == 0)
            return;
        AppMain.gmGmkFlipperChangeModeWait(obj_work);
    }

    private static void gmGmkFlipperMainHook(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        AppMain.gmGmkFlipperUpdateAngle(obj_work);
        if (false)
            return;
        if (AppMain.gmGmkFlipperCheckControlPlayer() == 0)
        {
            AppMain.gmGmkFlipperChangeModeWait(obj_work);
        }
        else
        {
            int flipper_type = AppMain.gmGmkFlipperCalcType((int)gmsEnemy3DWork.ene_com.eve_rec.id);
            AppMain.gmGmkFlipperSetRideSpeed(targetObj, obj_work, flipper_type);
            if (((int)AppMain.g_gm_main_system.ply_work[0].key_on & 160) == 0)
            {
                AppMain.gmGmkFlipperChangeModeReady(obj_work);
            }
            else
            {
                int num = targetObj.pos.x - obj_work.pos.x;
                if (flipper_type == 1)
                    num = -num;
                if (num <= 0)
                    return;
                targetObj.spd.x = 0;
                targetObj.spd.y = 0;
            }
        }
    }

    private static int gmGmkFlipperCheckKeyHit(
      AppMain.OBS_OBJECT_WORK gimmick_obj_work,
      AppMain.GMS_PLAYER_WORK player_work)
    {
        return AppMain.GmPlayerKeyCheckJumpKeyPush(player_work) ? 1 : 0;
    }

    private static int gmGmkFlipperCheckControlPlayer()
    {
        return AppMain.g_gm_main_system.ply_work[0].seq_state != 47 ? 0 : 1;
    }

    private static int gmGmkFlipperCheckScore(AppMain.OBS_OBJECT_WORK obj_work)
    {
        uint num = (uint)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.width * 1000U;
        if (num == 0U)
            return 0;
        uint score = AppMain.g_gm_main_system.ply_work[0].score;
        return num > score ? 0 : 1;
    }

    private static int gmGmkFlipperCheckLeft(
      AppMain.VecFx32 line_start,
      AppMain.VecFx32 line_end,
      AppMain.VecFx32 point)
    {
        int v1_1 = line_end.x - line_start.x;
        int v1_2 = line_end.y - line_start.y;
        int v2_1 = point.x - line_start.x;
        int v2_2 = point.y - line_start.y;
        return AppMain.FX_Mul(v1_1, v2_2) - AppMain.FX_Mul(v1_2, v2_1) <= 0 ? 1 : 0;
    }

    private static int gmGmkFlipperCheckRect(
      AppMain.VecFx32 gimmick_pos,
      AppMain.VecFx32 target_pos,
      int type)
    {
        switch (type)
        {
            case 0:
                AppMain.VecFx32 line_start1 = new AppMain.VecFx32(gimmick_pos);
                line_start1.y += AppMain.FX_F32_TO_FX32((float)((int)AppMain.g_gmk_flipper_rect[type][1] - 12));
                AppMain.VecFx32 line_end1 = new AppMain.VecFx32(gimmick_pos);
                line_end1.x += AppMain.FX_F32_TO_FX32((float)AppMain.g_gmk_flipper_rect[type][2]);
                line_end1.y += AppMain.FX_F32_TO_FX32((float)((int)AppMain.g_gmk_flipper_rect[type][3] - 12));
                if (AppMain.gmGmkFlipperCheckLeft(line_start1, line_end1, target_pos) != 0)
                    return 0;
                break;
            case 1:
                AppMain.VecFx32 line_end2 = new AppMain.VecFx32(gimmick_pos);
                line_end2.y += AppMain.FX_F32_TO_FX32((float)((int)AppMain.g_gmk_flipper_rect[type][1] - 12));
                AppMain.VecFx32 line_start2 = new AppMain.VecFx32(gimmick_pos);
                line_start2.x += AppMain.FX_F32_TO_FX32((float)AppMain.g_gmk_flipper_rect[type][0]);
                line_start2.y += AppMain.FX_F32_TO_FX32((float)((int)AppMain.g_gmk_flipper_rect[type][3] - 12));
                if (AppMain.gmGmkFlipperCheckLeft(line_start2, line_end2, target_pos) != 0)
                    return 0;
                break;
        }
        return 1;
    }

    private static int gmGmkFlipperCheckHook(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK targetObj = gmsEnemy3DWork.ene_com.target_obj;
        int num1 = targetObj.pos.x - obj_work.pos.x;
        int num2 = 16384;
        int flipper_type = AppMain.gmGmkFlipperCalcType((int)gmsEnemy3DWork.ene_com.eve_rec.id);
        if (flipper_type == 1)
        {
            num1 = -num1;
            num2 = -num2;
        }
        if (num1 > 16384)
            return 0;
        if (num1 > 0)
        {
            AppMain.GMS_PLAYER_WORK gmsPlayerWork = (AppMain.GMS_PLAYER_WORK)targetObj;
            targetObj.pos.x = obj_work.pos.x;
            int num3 = AppMain.gmGmkFlipperCalcRideOffsetY(obj_work.pos.x + num2, obj_work, flipper_type);
            int num4 = ((int)gmsPlayerWork.player_flag & 131072) == 0 ? num3 - 36864 : num3 - 61440;
            targetObj.pos.y = obj_work.pos.y + num4;
        }
        return 1;
    }

    private static void gmGmkFlipperSetRideSpeed(
      AppMain.OBS_OBJECT_WORK target_obj_work,
      AppMain.OBS_OBJECT_WORK gimmick_obj_work,
      int flipper_type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)gimmick_obj_work);
        int num = AppMain.FX_F32_TO_FX32(0.6857143f);
        int fx32 = AppMain.FX_F32_TO_FX32(0.3714286f);
        if (flipper_type == 1)
            num = -num;
        target_obj_work.spd.x = num;
        target_obj_work.spd.y = fx32;
        target_obj_work.spd.x = AppMain.FX_Div(target_obj_work.spd.x, 12288);
        target_obj_work.spd.y = AppMain.FX_Div(target_obj_work.spd.y, 12288);
    }

    private static int gmGmkFlipperCalcRideOffsetY(
      int x,
      AppMain.OBS_OBJECT_WORK gimmick_obj_work,
      int flipper_type)
    {
        float num = (float)((int)AppMain.g_gmk_flipper_rect[flipper_type][2] - (int)AppMain.g_gmk_flipper_rect[flipper_type][0]);
        if (flipper_type == 1)
            num = -num;
        return AppMain.FX_Mul((int)((double)((float)((int)AppMain.g_gmk_flipper_rect[flipper_type][3] - (int)AppMain.g_gmk_flipper_rect[flipper_type][1]) - 2f) / (double)num * 4096.0), x - gimmick_obj_work.pos.x);
    }

    private static int gmGmkFlipperUpdateAngle(AppMain.OBS_OBJECT_WORK obj_work)
    {
        ++obj_work.user_timer;
        ushort userWork = (ushort)obj_work.user_work;
        ushort num = (ushort)(((int)userWork - (int)obj_work.dir.z) / 6);
        obj_work.dir.z += num;
        if (obj_work.user_timer < 6)
            return 0;
        obj_work.dir.z = userWork;
        obj_work.user_timer = 0;
        return 1;
    }

}