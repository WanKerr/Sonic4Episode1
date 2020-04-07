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
    public static void GmGmkShutterBuild()
    {
        AppMain.g_gm_gmk_shutter_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(938), AppMain.GmGameDatGetGimmickData(939), 0U);
    }

    public static void GmGmkShutterFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(938);
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_shutter_obj_3d_list, gimmickData.file_num);
        AppMain.g_gm_gmk_shutter_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkShutterInInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkShutterLoadObj(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        AppMain.gmGmkShutterInInit(objWork);
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkShutterOutInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkShutterLoadObj(eve_rec, pos_x, pos_y, type).ene_com.obj_work;
        AppMain.gmGmkShutterOutInit(objWork);
        return objWork;
    }

    private static void GmGmkShutterInChangeModeClose(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.pos.y <= (int)obj_work.user_work)
            return;
        obj_work.spd.y = -16384;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkShutterInMainClose);
        obj_work.disp_flag &= 4294967263U;
        AppMain.GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (AppMain.GMS_GMK_SHUTTER_WORK)obj_work;
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] != 4 || gmsGmkShutterWork.effect_work != null)
            return;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 44);
        obsObjectWork.pos.x = obj_work.pos.x + 65536;
        obsObjectWork.pos.y = obj_work.pos.y - 131072;
        obsObjectWork.pos.z = 393216;
        gmsGmkShutterWork.effect_work = (AppMain.GMS_EFFECT_3DES_WORK)obsObjectWork;
    }

    private static void GmGmkShutterOutChangeModeOpen(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.pos.y >= (int)obj_work.user_work)
            return;
        obj_work.spd.y = 16384;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkShutterOutMainOpen);
        AppMain.GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (AppMain.GMS_GMK_SHUTTER_WORK)obj_work;
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] != 4 || gmsGmkShutterWork.effect_work != null)
            return;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 44);
        obsObjectWork.pos.x = obj_work.pos.x - 65536;
        obsObjectWork.pos.y = obj_work.pos.y + 131072;
        obsObjectWork.pos.z = 393216;
        gmsGmkShutterWork.effect_work = (AppMain.GMS_EFFECT_3DES_WORK)obsObjectWork;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkShutterLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_SHUTTER_WORK()), "GMK_SHUTTER");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkShutterLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int num = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        int index1;
        switch (num)
        {
            case 1:
                index1 = 0;
                break;
            case 4:
                index1 = 0;
                break;
            default:
                return (AppMain.GMS_ENEMY_3D_WORK)null;
        }
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkShutterLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        AppMain.OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.g_gm_gmk_shutter_obj_3d_list[index1], gmsEnemy3DWork.obj_3d);
        if (num == 4)
        {
            AppMain.GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (AppMain.GMS_GMK_SHUTTER_WORK)objWork;
            int index2 = 2;
            AppMain.ObjCopyAction3dNNModel(AppMain.g_gm_gmk_shutter_obj_3d_list[index2], gmsGmkShutterWork.obj_3d_parts);
            AppMain.ObjAction3dNNMaterialMotionLoad(gmsGmkShutterWork.obj_3d_parts, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 1, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(940).pData);
        }
        return gmsEnemy3DWork;
    }

    private static void gmGmkShutterDestFuncForFinaleZone(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.ObjAction3dNNMotionRelease(((AppMain.GMS_GMK_SHUTTER_WORK)AppMain.mtTaskGetTcbWork(tcb)).obj_3d_parts);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmGmkShutterInInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)64;
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)64;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)((int)-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)((int)-gmsEnemy3DWork.ene_com.col_work.obj_col.height / 2);
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194336U;
        obj_work.flag |= 16U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        obj_work.pos.z = -655360;
        obj_work.user_work = (uint)(obj_work.pos.y - 262144);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkShutterInMainWaitClose);
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] != 4)
            return;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkShutterInOutFuncForFinalZone);
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkShutterDestFuncForFinaleZone));
    }

    private static void gmGmkShutterInOutFuncForFinalZone(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (AppMain.GMS_GMK_SHUTTER_WORK)obj_work;
        obj_work.ofst.x = AppMain.gm_gmk_shutter_disp_offset_for_final_zone[0] * 4096;
        obj_work.ofst.y = AppMain.gm_gmk_shutter_disp_offset_for_final_zone[1] * 4096;
        AppMain.ObjDrawActionSummary(obj_work);
        AppMain.VecFx32 pos = obj_work.pos;
        pos.x += obj_work.ofst.x;
        pos.y += obj_work.ofst.y;
        uint p_disp_flag = obj_work.disp_flag | 4U;
        if (AppMain.ObjObjectPauseCheck(0U) == 0U)
            AppMain.ObjDrawAction3DNNMaterialUpdate(gmsGmkShutterWork.obj_3d_parts, ref p_disp_flag);
        AppMain.ObjDrawAction3DNN(gmsGmkShutterWork.obj_3d_parts, new AppMain.VecFx32?(pos), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref p_disp_flag);
    }

    private static void gmGmkShutterInMainWaitClose(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.OBS_OBJECT_WORK)AppMain.g_gm_main_system.ply_work[0]).pos.x - obj_work.pos.x < 262144)
            return;
        AppMain.GmGmkShutterInChangeModeClose(obj_work);
    }

    private static void gmGmkShutterInMainClose(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_main_system.ply_work[0];
        int a = (int)((long)obj_work.user_work - (long)obj_work.pos.y);
        if (AppMain.MTM_MATH_ABS(obsObjectWork.pos.x - obj_work.pos.x) < 131072 && obsObjectWork.pos.y <= obj_work.pos.y && AppMain.MTM_MATH_ABS(a) < 262144)
        {
            int num1 = AppMain.g_gm_main_system.map_fcol.left + (AppMain.g_gm_main_system.map_fcol.right - AppMain.g_gm_main_system.map_fcol.left) / 2;
            if (((int)obsObjectWork.move_flag & 1) != 0)
            {
                int num2 = 16384;
                if (num1 * 4096 < obsObjectWork.pos.x)
                    num2 *= -1;
                obsObjectWork.flow.x += num2;
            }
            else
            {
                int spd_x = 4096;
                if (num1 * 4096 < obsObjectWork.pos.x)
                    spd_x *= -1;
                AppMain.GmPlySeqGmkInitGmkJump((AppMain.GMS_PLAYER_WORK)obsObjectWork, spd_x, 0);
                AppMain.GmPlySeqChangeSequenceState((AppMain.GMS_PLAYER_WORK)obsObjectWork, 17);
            }
        }
        if (obj_work.pos.y > (int)obj_work.user_work)
            return;
        obj_work.pos.y = (int)obj_work.user_work;
        obj_work.spd.y = 0;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (AppMain.GMS_GMK_SHUTTER_WORK)obj_work;
        if (gmsGmkShutterWork.effect_work == null)
            return;
        AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)gmsGmkShutterWork.effect_work);
        gmsGmkShutterWork.effect_work = (AppMain.GMS_EFFECT_3DES_WORK)null;
    }

    private static void gmGmkShutterOutInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)64;
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)64;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)((int)-gmsEnemy3DWork.ene_com.col_work.obj_col.width / 2);
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)((int)-gmsEnemy3DWork.ene_com.col_work.obj_col.height / 2);
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        obj_work.flag |= 16U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 16384U;
        obj_work.pos.z = -655360;
        obj_work.user_work = (uint)(obj_work.pos.y + 262144);
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] != 4)
            return;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkShutterOutOutFuncForFinalZone);
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkShutterDestFuncForFinaleZone));
    }

    private static void gmGmkShutterOutOutFuncForFinalZone(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (AppMain.GMS_GMK_SHUTTER_WORK)obj_work;
        obj_work.ofst.x = -AppMain.gm_gmk_shutter_disp_offset_for_final_zone[0] * 4096;
        obj_work.ofst.y = AppMain.gm_gmk_shutter_disp_offset_for_final_zone[1] * 4096;
        AppMain.ObjDrawActionSummary(obj_work);
        AppMain.VecFx32 pos = obj_work.pos;
        pos.x += obj_work.ofst.x;
        pos.y += obj_work.ofst.y;
        uint p_disp_flag = obj_work.disp_flag | 4U;
        if (AppMain.ObjObjectPauseCheck(0U) == 0U)
            AppMain.ObjDrawAction3DNNMaterialUpdate(gmsGmkShutterWork.obj_3d_parts, ref p_disp_flag);
        AppMain.ObjDrawAction3DNN(gmsGmkShutterWork.obj_3d_parts, new AppMain.VecFx32?(pos), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref p_disp_flag);
    }

    private static void gmGmkShutterOutMainOpen(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.pos.y < (int)obj_work.user_work)
            return;
        obj_work.pos.y = (int)obj_work.user_work;
        obj_work.spd.y = 0;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.disp_flag |= 32U;
        AppMain.GMS_GMK_SHUTTER_WORK gmsGmkShutterWork = (AppMain.GMS_GMK_SHUTTER_WORK)obj_work;
        if (gmsGmkShutterWork.effect_work == null)
            return;
        AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)gmsGmkShutterWork.effect_work);
        gmsGmkShutterWork.effect_work = (AppMain.GMS_EFFECT_3DES_WORK)null;
    }

}