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
    private static void gmGmkWaterSliderEffectDestFunc(AppMain.MTS_TASK_TCB tcb)
    {
        if (AppMain.g_gm_gmk_water_slider_se_handle != null)
        {
            AppMain.GmSoundStopSE(AppMain.g_gm_gmk_water_slider_se_handle);
            AppMain.GsSoundFreeSeHandle(AppMain.g_gm_gmk_water_slider_se_handle);
            AppMain.g_gm_gmk_water_slider_se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.GMM_PAD_VIB_STOP();
        AppMain.GmEffectDefaultExit(tcb);
        AppMain.g_gm_gmk_water_slider_effct_player = (AppMain.GMS_EFFECT_3DES_WORK)null;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkWaterSliderCreateEffect()
    {
        if (AppMain.g_gm_gmk_water_slider_effct_player == null)
        {
            AppMain.g_gm_gmk_water_slider_effct_player = AppMain.GmEfctZoneEsCreate(AppMain.g_gm_main_system.ply_work[0].obj_work, 2, 23);
            AppMain.OBS_OBJECT_WORK sliderEffctPlayer = (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_gmk_water_slider_effct_player;
            sliderEffctPlayer.parent_ofst.z = 131072;
            sliderEffctPlayer.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkWaterSliderEffectMainFunc);
            AppMain.mtTaskChangeTcbDestructor(sliderEffctPlayer.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkWaterSliderEffectDestFunc));
        }
        if (AppMain.g_gm_gmk_water_slider_se_handle == null)
        {
            AppMain.g_gm_gmk_water_slider_se_handle = AppMain.GsSoundAllocSeHandle();
            AppMain.GmSoundPlaySE("WaterSlider", AppMain.g_gm_gmk_water_slider_se_handle);
        }
        AppMain.GMM_PAD_VIB_SMALL_TIME(30f);
        return (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_gmk_water_slider_effct_player;
    }

    private static void gmGmkWaterSliderEffectMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.g_gm_main_system.ply_work[0].seq_state != 36)
            AppMain.GmGmkWaterSliderDeleteEffect();
        else
            AppMain.GmEffectDefaultMainFuncDeleteAtEndCopyDirZ(obj_work);
    }

    private static void GmGmkWaterSliderDeleteEffect()
    {
        if (AppMain.g_gm_gmk_water_slider_effct_player == null)
            return;
        AppMain.g_gm_gmk_water_slider_effct_player.efct_com.obj_work.flag |= 8U;
        AppMain.g_gm_gmk_water_slider_effct_player = (AppMain.GMS_EFFECT_3DES_WORK)null;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkWaterSliderInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        uint num;
        switch (eve_rec.id)
        {
            case 116:
                num = 1U;
                break;
            case 117:
                num = 2U;
                break;
            case 118:
                num = 3U;
                break;
            case 120:
                num = 5U;
                break;
            case 121:
                num = 6U;
                break;
            case 122:
                num = 7U;
                break;
            default:
                return (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkWaterSliderLoadObj(eve_rec, pos_x, pos_y, num).ene_com.obj_work;
        AppMain.gmGmkWaterSliderInit(objWork, num);
        return objWork;
    }

    public static void GmGmkWaterSliderBuild()
    {
        AppMain.g_gm_gmk_water_slider_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(832), AppMain.GmGameDatGetGimmickData(833), 0U);
    }

    public static void GmGmkWaterSliderFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(832);
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_water_slider_obj_3d_list, gimmickData.file_num);
        AppMain.g_gm_gmk_water_slider_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    public static AppMain.OBS_ACTION3D_NN_WORK[] GmGmkWaterSliderGetObj3DList()
    {
        return AppMain.g_gm_gmk_water_slider_obj_3d_list;
    }

    private static uint gmGmkWaterSlidereGameSystemGetSyncTime()
    {
        return AppMain.g_gm_main_system.sync_time;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkWaterSliderLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      uint type)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_WATER_SLIDER_WORK()), "GMK_WATER_SLIDER");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkWaterSliderLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      uint type)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkWaterSliderLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        AppMain.OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index1 = AppMain.g_gm_gmk_water_slider_model_id_main[(int)type];
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.g_gm_gmk_water_slider_obj_3d_list[index1], gmsEnemy3DWork.obj_3d);
        int index2 = AppMain.g_gm_gmk_water_slider_material_id_main[(int)type];
        object pData1 = AppMain.ObjDataGet(835).pData;
        AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, index2, (AppMain.AMS_AMB_HEADER)pData1);
        AppMain.GMS_GMK_WATER_SLIDER_WORK gmkWaterSliderWork = (AppMain.GMS_GMK_WATER_SLIDER_WORK)objWork;
        int index3 = AppMain.g_gm_gmk_water_slider_model_id_sub[(int)type];
        if (index3 != -1)
            AppMain.ObjCopyAction3dNNModel(AppMain.g_gm_gmk_water_slider_obj_3d_list[index3], gmkWaterSliderWork.obj_3d_parts);
        gmkWaterSliderWork.obj_3d_parts.drawflag |= 32U;
        int index4 = AppMain.g_gm_gmk_water_slider_motion_id_sub[(int)type];
        if (index4 != -1)
        {
            object pData2 = AppMain.ObjDataGet(834).pData;
            AppMain.ObjAction3dNNMotionLoad(gmkWaterSliderWork.obj_3d_parts, 0, false, (AppMain.OBS_DATA_WORK)null, (string)null, index4, (AppMain.AMS_AMB_HEADER)pData2);
        }
        int index5 = AppMain.g_gm_gmk_water_slider_material_id_sub[(int)type];
        if (index5 != -1)
            AppMain.ObjAction3dNNMaterialMotionLoad(gmkWaterSliderWork.obj_3d_parts, 0, (AppMain.OBS_DATA_WORK)null, (string)null, index5, (AppMain.AMS_AMB_HEADER)pData1);
        objWork.disp_flag |= 268435456U;
        gmkWaterSliderWork.obj_3d_parts.command_state = 17U;
        return gmsEnemy3DWork;
    }

    private static bool gmGmkWaterSliderCheckHFlip(uint type)
    {
        switch (type)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return false;
            case 4:
            case 5:
            case 6:
            case 7:
                return true;
            default:
                return false;
        }
    }

    private static void gmGmkWaterSliderInit(AppMain.OBS_OBJECT_WORK obj_work, uint slider_type)
    {
        AppMain.gmGmkWaterSliderSetRect((AppMain.GMS_ENEMY_3D_WORK)obj_work, slider_type);
        AppMain.gmGmkWaterSliderSetUserWorkSlideType(obj_work, slider_type);
        obj_work.move_flag = 8448U;
        int speed = -61440;
        obj_work.dir.y = (ushort)49152;
        if (AppMain.gmGmkWaterSliderCheckHFlip(slider_type))
        {
            obj_work.disp_flag |= 1U;
            speed = -speed;
        }
        AppMain.gmGmkWaterSliderSetUserTimerSlideSpeed(obj_work, speed);
        obj_work.obj_3d.drawflag |= 32U;
        obj_work.pos.z = 131072;
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag |= 20U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkWaterSliderDrawFunc);
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkWaterSliderDestFunc));
    }

    private static void gmGmkWaterSliderSetRect(
      AppMain.GMS_ENEMY_3D_WORK gimmick_work,
      uint slider_type)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        short num1 = 0;
        short num2 = 0;
        short num3 = 0;
        switch (slider_type)
        {
            case 1:
            case 5:
                num1 = (short)-64;
                num3 = (short)32;
                break;
            case 2:
            case 6:
                num1 = (short)-64;
                num3 = (short)64;
                break;
            case 3:
            case 7:
                num1 = (short)-64;
                num3 = (short)128;
                break;
        }
        AppMain.ObjRectWorkZSet(pRec, (short)((int)num1 - 8), (short)-8, (short)-500, (short)((int)num2 + 8), (short)((int)num3 + 8), (short)500);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkWaterSliderDefFunc);
    }

    private static void gmGmkWaterSliderDestFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.ObjAction3dNNMotionRelease(((AppMain.GMS_GMK_WATER_SLIDER_WORK)AppMain.mtTaskGetTcbWork(tcb)).obj_3d_parts);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkWaterSliderDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (obj3d.motion != null)
        {
            float startFrame = AppMain.amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
            float num = AppMain.amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - startFrame;
            float syncTime = (float)AppMain.gmGmkWaterSlidereGameSystemGetSyncTime();
            obj3d.mat_frame = syncTime % num;
        }
        AppMain.ObjDrawActionSummary(obj_work);
        uint p_disp_flag = (obj_work.disp_flag | 4U) & 4294967279U;
        if (AppMain.ObjObjectPauseCheck(0U) != 0U)
            p_disp_flag |= 4096U;
        AppMain.GMS_GMK_WATER_SLIDER_WORK gmkWaterSliderWork = (AppMain.GMS_GMK_WATER_SLIDER_WORK)obj_work;
        AppMain.VecFx32 pos = obj_work.pos;
        pos.z += 131072;
        AppMain.ObjDrawAction3DNN(gmkWaterSliderWork.obj_3d_parts, new AppMain.VecFx32?(pos), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref p_disp_flag);
    }

    private static void gmGmkWaterSliderDefFunc(
      AppMain.OBS_RECT_WORK gimmick_rect,
      AppMain.OBS_RECT_WORK player_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)parentObj1;
        AppMain.OBS_OBJECT_WORK parentObj2 = player_rect.parent_obj;
        if (parentObj2.obj_type != (ushort)1)
            return;
        AppMain.GMS_PLAYER_WORK ply_work = (AppMain.GMS_PLAYER_WORK)parentObj2;
        if (ply_work.seq_state == 36)
        {
            ply_work.gmk_obj = parentObj1;
        }
        else
        {
            if (((int)parentObj2.move_flag & 1) == 0)
                return;
            if (AppMain.gmGmkWaterSliderCheckHFlip(AppMain.gmGmkWaterSliderGetUserWorkSlideType(parentObj1)))
                parentObj2.disp_flag &= 4294967294U;
            else
                parentObj2.disp_flag |= 1U;
            parentObj2.spd_m = AppMain.gmGmkWaterSliderGetUserTimerSlideSpeed(parentObj1);
            parentObj2.spd.x = 0;
            parentObj2.spd.y = 0;
            parentObj2.spd_add.x = 0;
            parentObj2.spd_add.y = 0;
            AppMain.GmPlySeqInitWaterSlider(ply_work, gmsEnemy3DWork.ene_com);
            gmsEnemy3DWork.ene_com.target_obj = parentObj2;
            gimmick_rect.flag |= 1024U;
            parentObj1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkWaterSliderMainActive);
            AppMain.GmGmkWaterSliderCreateEffect();
            AppMain.nnMakeUnitMatrix(ply_work.ex_obj_mtx_r);
            int ay = -6144;
            if (((int)parentObj2.disp_flag & 1) != 0)
                ay = -ay;
            AppMain.nnRotateYMatrix(ply_work.ex_obj_mtx_r, ply_work.ex_obj_mtx_r, ay);
            ply_work.ex_obj_mtx_r.M13 = -2f;
            ply_work.gmk_flag |= 32768U;
        }
    }

    private static void gmGmkWaterSliderMainActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK targetObj = (AppMain.GMS_PLAYER_WORK)gmsEnemy3DWork.ene_com.target_obj;
        AppMain.OBS_RECT_WORK obsRectWork = gmsEnemy3DWork.ene_com.rect_work[2];
        if (targetObj.seq_state == 36)
            return;
        gmsEnemy3DWork.ene_com.target_obj = (AppMain.OBS_OBJECT_WORK)null;
        obsRectWork.flag &= 4294966271U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }

    private static void gmGmkWaterSliderSetUserWorkSlideType(
      AppMain.OBS_OBJECT_WORK obj_work,
      uint type)
    {
        obj_work.user_work = type;
    }

    private static uint gmGmkWaterSliderGetUserWorkSlideType(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_work;
    }

    private static void gmGmkWaterSliderSetUserTimerSlideSpeed(
      AppMain.OBS_OBJECT_WORK obj_work,
      int speed)
    {
        obj_work.user_timer = speed;
    }

    private static int gmGmkWaterSliderGetUserTimerSlideSpeed(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return obj_work.user_timer;
    }

}