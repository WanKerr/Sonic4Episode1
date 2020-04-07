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
    private static AppMain.OBS_OBJECT_WORK GmGmkBoss3PillarInitManager(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)AppMain.gmGmkBoss3PillarLoadObjNoModel(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK()));
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)pillarManagerWork;
        AppMain.gmGmkBoss3PillarManagerInit(obj_work);
        for (int index = 0; 2 > index; ++index)
        {
            int num1 = 0;
            int num2 = 0;
            switch (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id])
            {
                case 2:
                    num1 = AppMain.g_gm_gmk_boss3_pillar_wall_default_pos[index][0];
                    num2 = AppMain.g_gm_gmk_boss3_pillar_wall_default_pos[index][1];
                    break;
                case 4:
                    num1 = AppMain.g_gm_gmk_boss3_pillar_f_wall_default_pos[index][0];
                    num2 = AppMain.g_gm_gmk_boss3_pillar_f_wall_default_pos[index][1];
                    break;
            }
            AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)343, obj_work.pos.x + num1 * 4096, obj_work.pos.y + num2 * 4096, eve_rec.flag, eve_rec.left, eve_rec.top, eve_rec.width, eve_rec.height, (byte)index);
            obsObjectWork.parent_obj = obj_work;
            pillarManagerWork.obj_work_wall[index] = obsObjectWork;
            if (((int)pillarManagerWork.gimmick_work.ene_com.eve_rec.byte_param[1] & 1) != 0 && index == 0)
            {
                int num3 = 917504;
                obsObjectWork.pos.y -= num3;
            }
        }
        return obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBoss3PillarInitParts(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        uint pillar_type = AppMain.gmGmkBoss3PillarCalcPillarType((int)eve_rec.id);
        int num;
        int[] numArray;
        switch (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id])
        {
            case 2:
                num = AppMain.g_gm_gmk_boss3_pillar_parts_num[(int)pillar_type];
                numArray = AppMain.g_gm_boss3_pillar_model_id[(int)pillar_type];
                break;
            case 4:
                num = AppMain.g_gm_gmk_boss3_pillar_f_parts_num[(int)pillar_type];
                numArray = AppMain.g_gm_boss3_pillar_f_model_id[(int)pillar_type];
                break;
            default:
                return (AppMain.OBS_OBJECT_WORK)null;
        }
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkBoss3PillarLoadObj(eve_rec, pos_x, pos_y, AppMain.g_gm_gmk_boss3_pillar_obj_3d_list, (uint)numArray[0], (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK()));
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)gmsEnemy3DWork;
        AppMain.gmGmkBoss3PillarPartsInitMain(obj_work, pillar_type);
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)gmsEnemy3DWork;
        for (int index1 = 1; num > index1; ++index1)
        {
            int index2 = numArray[index1];
            AppMain.ObjCopyAction3dNNModel(AppMain.g_gm_gmk_boss3_pillar_obj_3d_list[index2], s3PillarMainWork.obj_3d_parts[index1 - 1]);
            s3PillarMainWork.obj_3d_parts[index1 - 1].drawflag |= 32U;
        }
        return obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkBoss3PillarInitWall(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)AppMain.gmGmkBoss3PillarLoadObj(eve_rec, pos_x, pos_y, AppMain.g_gm_gmk_boss3_wall_obj_3d_list, 0U, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK()));
        AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        int index = AppMain.g_gm_gmk_boss3_wall_obj_3d_list.Length > 1 ? 1 : 0;
        AppMain.ObjCopyAction3dNNModel(AppMain.g_gm_gmk_boss3_wall_obj_3d_list[index], s3PillarWallWork.obj_3d_parts[0]);
        AppMain.gmGmkBoss3PillarWallInit(obj_work);
        return obj_work;
    }

    public static void GmGmkBoss3PillarBuild()
    {
        AppMain.g_gm_gmk_boss3_pillar_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(941), AppMain.GmGameDatGetGimmickData(942), 0U);
        AppMain.g_gm_gmk_boss3_wall_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(945), AppMain.GmGameDatGetGimmickData(946), 0U);
        AppMain.g_gm_gmk_boss3_pillar_obj_tvx_list = AppMain.GmGameDatGetGimmickData(944);
        AppMain.g_gm_gmk_boss3_wall_obj_tvx_list = AppMain.GmGameDatGetGimmickData(948);
        AppMain.gm_gmk_boss3_pillar_se_use_count = 0;
        AppMain.gm_gmk_boss3_pillar_global_flag = 0;
    }

    public static void GmGmkBoss3PillarFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData1 = AppMain.GmGameDatGetGimmickData(941);
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_boss3_pillar_obj_3d_list, gimmickData1.file_num);
        AppMain.g_gm_gmk_boss3_pillar_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        AppMain.AMS_AMB_HEADER gimmickData2 = AppMain.GmGameDatGetGimmickData(945);
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_boss3_wall_obj_3d_list, gimmickData2.file_num);
        AppMain.g_gm_gmk_boss3_wall_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        AppMain.g_gm_gmk_boss3_pillar_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
        AppMain.g_gm_gmk_boss3_wall_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
        AppMain.gm_gmk_boss3_pillar_se_use_count = 0;
        AppMain.gm_gmk_boss3_pillar_global_flag = 0;
    }

    private static void GmGmkBoss3PillarChangeModeActive(
      AppMain.OBS_OBJECT_WORK obj_work,
      int pattern_no)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        float num1 = 80f;
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == 4)
            num1 /= 1.5f;
        for (int index = 0; 26 > index; ++index)
        {
            AppMain.OBS_OBJECT_WORK obj_work1 = pillarManagerWork.obj_work_pillar[index];
            int num2 = AppMain.g_gm_gmk_boss3_pillar_move_distance[pattern_no][index] * 40;
            if (num2 == 0)
            {
                if (obj_work1 != null)
                {
                    obj_work1.flag |= 8U;
                    AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work1;
                    if (s3PillarMainWork.effect_work != null)
                    {
                        AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)s3PillarMainWork.effect_work);
                        s3PillarMainWork.effect_work = (AppMain.GMS_EFFECT_3DES_WORK)null;
                    }
                    pillarManagerWork.obj_work_pillar[index] = (AppMain.OBS_OBJECT_WORK)null;
                }
            }
            else
            {
                if (obj_work1 == null)
                {
                    obj_work1 = AppMain.GmEventMgrLocalEventBirth((ushort)AppMain.g_gm_gmk_boss3_pillar_event_id[index], obj_work.pos.x + AppMain.g_gm_gmk_boss3_pillar_default_pos[index][0] * 40 * 4096, obj_work.pos.y + AppMain.g_gm_gmk_boss3_pillar_default_pos[index][1] * 40 * 4096, gmsEnemy3DWork.ene_com.eve_rec.flag, gmsEnemy3DWork.ene_com.eve_rec.left, gmsEnemy3DWork.ene_com.eve_rec.top, gmsEnemy3DWork.ene_com.eve_rec.width, gmsEnemy3DWork.ene_com.eve_rec.height, (byte)0);
                    obj_work1.parent_obj = obj_work;
                    pillarManagerWork.obj_work_pillar[index] = obj_work1;
                }
                else
                    AppMain.gmGmkBoss3PillarPartsChangeModeWait(obj_work1);
                if (AppMain.g_gm_gmk_boss3_pillar_flag_hit_effect[pattern_no][index] != 0)
                {
                    obj_work1.user_flag |= 9U;
                    AppMain.gm_gmk_boss3_pillar_global_flag |= 8;
                }
                int wait_time = AppMain.g_gm_gmk_boss3_pillar_wait_frame[pattern_no][index] * (int)num1;
                AppMain.gmGmkBoss3PillarPartsChangeModeNormal(obj_work1, (num2 + 16) * 4096, wait_time);
            }
        }
        pillarManagerWork.pattern_no = pattern_no;
        obj_work.user_timer = (int)((double)AppMain.g_gm_gmk_boss3_pillar_frame_change_hurry[pillarManagerWork.pattern_no] * (double)num1);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarManagerMainFuncWaitHurry);
    }

    private static void GmGmkBoss3PillarChangeModeHurry(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        for (int index = 0; 26 > index; ++index)
        {
            AppMain.OBS_OBJECT_WORK obj_work1 = pillarManagerWork.obj_work_pillar[index];
            if (obj_work1 != null)
                AppMain.gmGmkBoss3PillarPartsChangeModeHurry(obj_work1, 50);
        }
    }

    private static void GmGmkBoss3PillarChangeModeReturn(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        for (int index = 0; 26 > index; ++index)
        {
            AppMain.OBS_OBJECT_WORK obj_work1 = pillarManagerWork.obj_work_pillar[index];
            if (obj_work1 != null)
                AppMain.gmGmkBoss3PillarPartsChangeModeReturn(obj_work1, 0, 30);
        }
        obj_work.user_timer = 30;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarManagerMainFuncWaitReturn);
    }

    private static void GmGmkBoss3PillarChangeModeDelete(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        for (int index = 0; 26 > index; ++index)
        {
            AppMain.OBS_OBJECT_WORK obsObjectWork = pillarManagerWork.obj_work_pillar[index];
            if (obsObjectWork != null)
            {
                obsObjectWork.flag |= 8U;
                pillarManagerWork.obj_work_pillar[index] = (AppMain.OBS_OBJECT_WORK)null;
            }
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarManagerMainFuncFw);
    }

    private static int GmGmkBoss3PillarGetActiveTime(int pattern_no)
    {
        float num = 80f;
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == 4)
            num /= 1.5f;
        return (int)((double)AppMain.g_gm_gmk_boss3_pillar_frame_change_hurry[pattern_no] * (double)num + 50.0);
    }

    private static void GmGmkBoss3PillarWallChangeModeActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        for (int index = 0; 2 > index; ++index)
        {
            AppMain.OBS_OBJECT_WORK obj_work1 = pillarManagerWork.obj_work_wall[index];
            obj_work1.user_flag |= 1U;
            obj_work1.user_flag |= 4U;
            AppMain.gmGmkBoss3PillarWallChangeModeActive(obj_work1);
        }
        pillarManagerWork.obj_work_wall[0].user_flag |= 10U;
        AppMain.gm_gmk_boss3_pillar_global_flag |= 12;
    }

    private static void GmGmkBoss3PillarWallChangeModeReturn(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        pillarManagerWork.gimmick_work.ene_com.eve_rec.byte_param[1] |= (byte)1;
        AppMain.gmGmkBoss3PillarWallChangeModeReturn(pillarManagerWork.obj_work_wall[1]);
    }

    private static void GmGmkBoss3PillarWallClearFlagNoPressDie(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK pillarManagerWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)obj_work;
        for (int index = 0; 2 > index; ++index)
            ((AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)pillarManagerWork.obj_work_wall[index]).gimmick_work.ene_com.enemy_flag &= 4294950911U;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkBoss3PillarLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      AppMain.TaskWorkFactoryDelegate work_size)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, work_size, "GMK_BOSS3_PILLAR");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkBoss3PillarLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      AppMain.OBS_ACTION3D_NN_WORK[] data_work_list,
      uint model_id,
      AppMain.TaskWorkFactoryDelegate work_size)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkBoss3PillarLoadObjNoModel(eve_rec, pos_x, pos_y, work_size);
        AppMain.ObjObjectCopyAction3dNNModel(gmsEnemy3DWork.ene_com.obj_work, data_work_list[(int)model_id], gmsEnemy3DWork.obj_3d);
        return gmsEnemy3DWork;
    }

    private static void gmGmkBoss3PillarManagerInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 256U;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppOut = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarManagerMainFuncFw);
    }

    private static void gmGmkBoss3PillarManagerMainFuncWaitHurry(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gm_gmk_boss3_pillar_global_flag = -1;
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.GmGmkBoss3PillarChangeModeHurry(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarManagerMainFuncWaitHurryEnd);
            obj_work.user_timer = 50;
        }
    }

    private static void gmGmkBoss3PillarManagerMainFuncWaitHurryEnd(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gm_gmk_boss3_pillar_global_flag = -1;
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.GmCameraVibrationSet(0, 12288, 0);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarManagerMainFuncFw);
        }
    }

    private static void gmGmkBoss3PillarManagerMainFuncWaitReturn(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gm_gmk_boss3_pillar_global_flag = -1;
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            AppMain.GmGmkBoss3PillarChangeModeDelete(obj_work);
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarManagerMainFuncFw);
        }
    }

    private static void gmGmkBoss3PillarManagerMainFuncFw(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gm_gmk_boss3_pillar_global_flag = -1;
    }

    private static uint gmGmkBoss3PillarCalcPillarType(int event_id)
    {
        return (uint)(event_id - 339);
    }

    private static void gmGmkBoss3PillarSetFieldRect(
      AppMain.OBS_COLLISION_OBJ field_obj,
      uint pillar_type)
    {
        switch (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id])
        {
            case 2:
                field_obj.width = (ushort)AppMain.g_gm_boss3_pillar_field_rect_wh[(int)pillar_type][0];
                field_obj.height = (ushort)AppMain.g_gm_boss3_pillar_field_rect_wh[(int)pillar_type][1];
                field_obj.ofst_x = (short)AppMain.g_gm_boss3_pillar_field_rect_offset[(int)pillar_type][0];
                field_obj.ofst_y = (short)AppMain.g_gm_boss3_pillar_field_rect_offset[(int)pillar_type][1];
                break;
            case 4:
                field_obj.width = (ushort)AppMain.g_gm_boss3_pillar_f_field_rect_wh[(int)pillar_type][0];
                field_obj.height = (ushort)AppMain.g_gm_boss3_pillar_f_field_rect_wh[(int)pillar_type][1];
                field_obj.ofst_x = (short)AppMain.g_gm_boss3_pillar_f_field_rect_offset[(int)pillar_type][0];
                field_obj.ofst_y = (short)AppMain.g_gm_boss3_pillar_f_field_rect_offset[(int)pillar_type][1];
                break;
        }
    }

    private static void gmGmkBoss3PillarSetMoveSpeed(
      AppMain.OBS_OBJECT_WORK obj_work,
      uint pillar_type,
      int speed)
    {
        int num1 = AppMain.FX_Mul(speed, AppMain.g_gm_boss3_pillar_adjust_dir[(int)pillar_type][0] * 4096);
        int num2 = AppMain.FX_Mul(speed, AppMain.g_gm_boss3_pillar_adjust_dir[(int)pillar_type][1] * 4096);
        obj_work.spd.x = num1;
        obj_work.spd.y = num2;
    }

    private static void gmGmkBoss3PillarSetMoveTarget(
      AppMain.OBS_OBJECT_WORK obj_work,
      uint pillar_type,
      int distance)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        int num1 = AppMain.FX_Mul(distance, AppMain.g_gm_boss3_pillar_adjust_dir[(int)pillar_type][0] * 4096);
        int num2 = AppMain.FX_Mul(distance, AppMain.g_gm_boss3_pillar_adjust_dir[(int)pillar_type][1] * 4096);
        s3PillarMainWork.target_pos.x = s3PillarMainWork.default_pos.x + num1;
        s3PillarMainWork.target_pos.y = s3PillarMainWork.default_pos.y + num2;
    }

    private static void gmGmkBoss3PillarSetMoveHurry(AppMain.OBS_OBJECT_WORK obj_work, int frame)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        if (frame <= 0)
        {
            obj_work.pos.Assign(s3PillarMainWork.target_pos);
            obj_work.spd.x = 0;
            obj_work.spd.y = 0;
        }
        else
        {
            int numer1 = s3PillarMainWork.target_pos.x - obj_work.pos.x;
            int numer2 = s3PillarMainWork.target_pos.y - obj_work.pos.y;
            int num1 = AppMain.FX_Div(numer1, frame * 4096);
            int num2 = AppMain.FX_Div(numer2, frame * 4096);
            obj_work.spd.x = num1;
            obj_work.spd.y = num2;
        }
    }

    private static bool gmGmkBoss3PillarCheckMoveEnd(
      AppMain.OBS_OBJECT_WORK obj_work,
      uint pillar_type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)pillar_type);
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        bool flag = false;
        int x = obj_work.spd.x;
        int y = obj_work.spd.y;
        if (x == 0 && y == 0)
        {
            flag = true;
        }
        else
        {
            int num1 = s3PillarMainWork.target_pos.x - obj_work.pos.x;
            int num2 = s3PillarMainWork.target_pos.y - obj_work.pos.y;
            if (x < 0 && num1 >= 0 || x > 0 && num1 <= 0 || (y < 0 && num2 >= 0 || y > 0 && num2 <= 0))
                flag = true;
        }
        return flag;
    }

    private static void gmBoss3PillarDestFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK tcbWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.effect_work != null)
        {
            AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)tcbWork.effect_work);
            tcbWork.effect_work = (AppMain.GMS_EFFECT_3DES_WORK)null;
        }
        if (tcbWork.se_handle != null)
            AppMain.GmSoundStopSE(tcbWork.se_handle);
        AppMain.GMM_PAD_VIB_STOP();
        if (tcbWork.se_handle != null)
        {
            AppMain.GmSoundStopSE(tcbWork.se_handle);
            AppMain.GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss3PillarOutFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        uint num1 = AppMain.gmGmkBoss3PillarCalcPillarType((int)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        int num2 = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        int num3 = 0;
        int[] numArray1 = (int[])null;
        int[] numArray2 = (int[])null;
        AppMain.AMS_AMB_HEADER pillarObjTvxList = AppMain.g_gm_gmk_boss3_pillar_obj_tvx_list;
        switch (num2)
        {
            case 2:
                num3 = AppMain.g_gm_gmk_boss3_pillar_parts_num[(int)num1];
                numArray1 = AppMain.g_gm_boss3_pillar_parts_offset[(int)num1];
                numArray2 = AppMain.g_gm_boss3_pillar_model_id[(int)num1];
                break;
            case 4:
                num3 = AppMain.g_gm_gmk_boss3_pillar_f_parts_num[(int)num1];
                numArray1 = AppMain.g_gm_boss3_pillar_f_parts_offset[(int)num1];
                numArray2 = AppMain.g_gm_boss3_pillar_f_model_id[(int)num1];
                int num4 = AppMain.g_gm_boss3_pillar_f_sub_model_id[(int)num1];
                break;
        }
        uint gmdTvxDispScale = AppMain.GMD_TVX_DISP_SCALE;
        AppMain.VecFx32 scale;
        scale.x = AppMain.FX_F32_TO_FX32(1.25f);
        scale.y = AppMain.FX_F32_TO_FX32(1.25f);
        scale.z = obj_work.scale.z;
        if (((int)obj_work.disp_flag & 1) != 0)
            scale.x *= -1;
        else if (((int)obj_work.disp_flag & 2) != 0)
            scale.y *= -1;
        AppMain.TVX_FILE model_tvx;
        if (pillarObjTvxList.buf[numArray2[0]] == null)
        {
            model_tvx = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(pillarObjTvxList, numArray2[0]));
            pillarObjTvxList.buf[numArray2[0]] = (object)model_tvx;
        }
        else
            model_tvx = (AppMain.TVX_FILE)pillarObjTvxList.buf[numArray2[0]];
        AppMain.GmTvxSetModel(model_tvx, obj_work.obj_3d.texlist, ref obj_work.pos, ref scale, gmdTvxDispScale, (short)0);
        for (int index = 1; num3 > index; ++index)
        {
            int num5 = -numArray1[index];
            int num6 = -numArray1[index];
            int num7 = num5 * (AppMain.g_gm_boss3_pillar_adjust_dir[(int)num1][0] * 40);
            int num8 = num6 * (AppMain.g_gm_boss3_pillar_adjust_dir[(int)num1][1] * 40);
            AppMain.VecFx32 pos = new AppMain.VecFx32(obj_work.pos);
            pos.x += num7 * 4096;
            pos.y += num8 * 4096;
            AppMain.GmTvxSetModel(new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(pillarObjTvxList, numArray2[index])), obj_work.obj_3d.texlist, ref pos, ref scale, gmdTvxDispScale, (short)0);
        }
    }

    private static void gmGmkBoss3PillarPartsInitMain(
      AppMain.OBS_OBJECT_WORK obj_work,
      uint pillar_type)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
        AppMain.gmGmkBoss3PillarSetFieldRect(gmsEnemy3DWork.ene_com.col_work.obj_col, pillar_type);
        obj_work.disp_flag |= 67108864U;
        obj_work.flag |= 16U;
        obj_work.move_flag |= 4112U;
        obj_work.move_flag &= 4294967167U;
        obj_work.move_flag |= 256U;
        switch (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id])
        {
            case 2:
                switch (pillar_type)
                {
                    case 0:
                        obj_work.disp_flag |= 1U;
                        break;
                    case 2:
                        obj_work.disp_flag |= 2U;
                        break;
                }
                gmsEnemy3DWork.obj_3d.drawflag |= 32U;
                obj_work.pos.x += AppMain.FX_F32_TO_FX32(AppMain.g_gm_gmk_boss3_pillar_adjust_default_pos[(int)pillar_type][0]);
                obj_work.pos.y += AppMain.FX_F32_TO_FX32(AppMain.g_gm_gmk_boss3_pillar_adjust_default_pos[(int)pillar_type][1]);
                break;
            case 4:
                obj_work.pos.x += AppMain.FX_F32_TO_FX32(AppMain.g_gm_gmk_boss3_pillar_f_adjust_default_pos[(int)pillar_type][0]);
                obj_work.pos.y += AppMain.FX_F32_TO_FX32(AppMain.g_gm_gmk_boss3_pillar_f_adjust_default_pos[(int)pillar_type][1]);
                break;
        }
        obj_work.pos.z = -655360;
        s3PillarMainWork.default_pos.Assign(obj_work.pos);
        s3PillarMainWork.target_pos.Assign(s3PillarMainWork.default_pos);
        s3PillarMainWork.se_handle = AppMain.GsSoundAllocSeHandle();
        AppMain.gmGmkBoss3PillarPartsChangeModeWait(obj_work);
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3PillarOutFunc);
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss3PillarDestFunc));
    }

    private static void gmGmkBoss3PillarPartsChangeModeWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        obj_work.pos.Assign(s3PillarMainWork.target_pos);
        obj_work.spd.x = 0;
        obj_work.spd.y = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarPartsMainWait);
        if (s3PillarMainWork.effect_work == null)
            return;
        AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)s3PillarMainWork.effect_work);
        s3PillarMainWork.effect_work = (AppMain.GMS_EFFECT_3DES_WORK)null;
    }

    private static void gmGmkBoss3PillarPartsChangeModeNormal(
      AppMain.OBS_OBJECT_WORK obj_work,
      int distance,
      int wait_time)
    {
        uint pillar_type = AppMain.gmGmkBoss3PillarCalcPillarType((int)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        AppMain.gmGmkBoss3PillarSetMoveTarget(obj_work, pillar_type, distance);
        float x = 0.5f;
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == 4)
            x *= 1.5f;
        AppMain.gmGmkBoss3PillarSetMoveSpeed(obj_work, pillar_type, AppMain.FX_F32_TO_FX32(x));
        obj_work.user_flag |= 4U;
        AppMain.gm_gmk_boss3_pillar_global_flag |= 4;
        obj_work.user_timer = wait_time;
        obj_work.move_flag |= 8192U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarPartsMainReady);
    }

    private static void gmGmkBoss3PillarPartsChangeModeHurry(
      AppMain.OBS_OBJECT_WORK obj_work,
      int move_frame)
    {
        AppMain.gmGmkBoss3PillarSetMoveHurry(obj_work, move_frame);
        obj_work.user_timer = 0;
        obj_work.move_flag &= 4294959103U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarPartsMainActive);
    }

    private static void gmGmkBoss3PillarPartsChangeModeReturn(
      AppMain.OBS_OBJECT_WORK obj_work,
      int wait_time,
      int move_frame)
    {
        uint pillar_type = AppMain.gmGmkBoss3PillarCalcPillarType((int)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        AppMain.gmGmkBoss3PillarSetMoveTarget(obj_work, pillar_type, 0);
        AppMain.gmGmkBoss3PillarSetMoveHurry(obj_work, move_frame);
        obj_work.user_flag &= 4294967294U;
        obj_work.user_flag |= 4U;
        obj_work.user_flag &= 4294967287U;
        AppMain.gm_gmk_boss3_pillar_global_flag |= 4;
        AppMain.gm_gmk_boss3_pillar_global_flag = (int)((long)AppMain.gm_gmk_boss3_pillar_global_flag & 4294967287L);
        obj_work.user_timer = wait_time;
        obj_work.move_flag |= 8192U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarPartsMainReady);
    }

    private static void gmGmkBoss3PillarPartsMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkBoss3PillarPartsMainReady(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.user_timer > 0)
        {
            --obj_work.user_timer;
        }
        else
        {
            obj_work.move_flag &= 4294959103U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarPartsMainActive);
            AppMain.gmGmkBoss3PillarEffectCreatePillarAppear(obj_work);
            obj_work.user_timer = 45;
            if (((long)obj_work.user_flag & (long)AppMain.gm_gmk_boss3_pillar_global_flag & 4L) == 0L)
                return;
            AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
            if (s3PillarMainWork.se_handle != null)
                AppMain.GmSoundPlaySE("Boss3_01", s3PillarMainWork.se_handle);
            obj_work.user_flag &= 4294967291U;
            AppMain.gm_gmk_boss3_pillar_global_flag = (int)((long)AppMain.gm_gmk_boss3_pillar_global_flag & 4294967291L);
            AppMain.GMM_PAD_VIB_SMALL_NOEND();
        }
    }

    private static void gmGmkBoss3PillarPartsMainActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        uint pillar_type = AppMain.gmGmkBoss3PillarCalcPillarType((int)((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id);
        if (--obj_work.user_timer == 0)
        {
            AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
            if (s3PillarMainWork.effect_work != null)
            {
                AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)s3PillarMainWork.effect_work);
                s3PillarMainWork.effect_work = (AppMain.GMS_EFFECT_3DES_WORK)null;
            }
        }
        if (!AppMain.gmGmkBoss3PillarCheckMoveEnd(obj_work, pillar_type))
            return;
        if (((int)obj_work.user_flag & 1) != 0)
        {
            AppMain.gmGmkBoss3PillarEffectCreatePillarHit(obj_work);
            obj_work.user_flag &= 4294967294U;
            AppMain.GMM_PAD_VIB_STOP();
        }
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork1 = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        if (s3PillarMainWork1.se_handle != null)
        {
            AppMain.GmSoundStopSE(s3PillarMainWork1.se_handle);
            if (((long)obj_work.user_flag & (long)AppMain.gm_gmk_boss3_pillar_global_flag & 8L) != 0L)
            {
                AppMain.GmSoundPlaySE("Boss3_02", s3PillarMainWork1.se_handle);
                obj_work.user_flag &= 4294967287U;
                AppMain.gm_gmk_boss3_pillar_global_flag = (int)((long)AppMain.gm_gmk_boss3_pillar_global_flag & 4294967287L);
            }
        }
        AppMain.gmGmkBoss3PillarPartsChangeModeWait(obj_work);
    }

    private static AppMain.GSS_SND_SE_HANDLE gmGmkBoss3PillarGetSeHandle()
    {
        AppMain.GSS_SND_SE_HANDLE gssSndSeHandle = (AppMain.GSS_SND_SE_HANDLE)null;
        if (AppMain.gm_gmk_boss3_pillar_se_use_count <= 0)
        {
            gssSndSeHandle = AppMain.GsSoundAllocSeHandle();
            ++AppMain.gm_gmk_boss3_pillar_se_use_count;
        }
        return gssSndSeHandle;
    }

    private static void gmGmkBoss3PillarFreeHandle(AppMain.GSS_SND_SE_HANDLE se_handle)
    {
        if (se_handle == null)
            return;
        AppMain.GmSoundStopSE(se_handle);
        AppMain.GsSoundFreeSeHandle(se_handle);
        --AppMain.gm_gmk_boss3_pillar_se_use_count;
    }

    private static void gmGmkBoss3PillarWallInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        int num = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        switch (num)
        {
            case 2:
                gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
                gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)32;
                gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)192;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)-16;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)-16;
                break;
            case 4:
                gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
                gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)32;
                gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)192;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)-16;
                gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)0;
                break;
        }
        obj_work.disp_flag |= 67108864U;
        obj_work.flag |= 16U;
        obj_work.move_flag |= 272U;
        obj_work.move_flag &= 4294967167U;
        s3PillarWallWork.gimmick_work.ene_com.enemy_flag |= 16384U;
        obj_work.pos.z = -393216;
        s3PillarWallWork.default_pos.Assign(obj_work.pos);
        s3PillarWallWork.target_pos.Assign(s3PillarWallWork.default_pos);
        s3PillarWallWork.se_handle = AppMain.GsSoundAllocSeHandle();
        obj_work.user_flag |= 16U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarWallMainWait);
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss3PillarWallDestFunc));
        if (num == 2)
        {
            obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3PillarWallOutFunc);
        }
        else
        {
            if (num != 4)
                return;
            obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3PillarWallOutFuncForFinalZone);
        }
    }

    private static void gmBoss3PillarWallDestFunc(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK tcbWork = (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)AppMain.mtTaskGetTcbWork(tcb);
        if (tcbWork.effect_work != null)
        {
            AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)tcbWork.effect_work);
            tcbWork.effect_work = (AppMain.GMS_EFFECT_3DES_WORK)null;
        }
        AppMain.GMM_PAD_VIB_STOP();
        if (tcbWork.se_handle != null)
        {
            AppMain.GmSoundStopSE(tcbWork.se_handle);
            AppMain.GsSoundFreeSeHandle(tcbWork.se_handle);
            tcbWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        }
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss3PillarWallOutFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int num1 = 6;
        int num2 = 32;
        if (((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.TVX_FILE model_tvx = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.g_gm_gmk_boss3_wall_obj_tvx_list, 0));
        AppMain.VecFx32 scale = new AppMain.VecFx32(4096, 4096, 4096);
        for (int index = 0; num1 > index; ++index)
        {
            AppMain.VecFx32 pos = new AppMain.VecFx32(obj_work.pos);
            pos.y += num2 * index * 4096;
            AppMain.GmTvxSetModel(model_tvx, obj_work.obj_3d.texlist, ref pos, ref scale, 0U, (short)0);
        }
    }

    private static void gmBoss3PillarWallOutFuncForFinalZone(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int num1 = 3;
        int num2 = 64;
        AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        uint num3 = obj_work.disp_flag | 4U | 4096U;
        obj_work.ofst.y = num2 * 4096;
        AppMain.ObjDrawActionSummary(obj_work);
        for (int index = 1; num1 > index; ++index)
        {
            AppMain.VecFx32 vecFx32 = new AppMain.VecFx32(obj_work.pos);
            vecFx32.y += obj_work.ofst.y + num2 * (index - 1) * 4096;
            AppMain.ObjDrawAction3DNN(s3PillarWallWork.obj_3d_parts[0], new AppMain.VecFx32?(vecFx32), new AppMain.VecU16?(obj_work.dir), obj_work.scale, ref obj_work.disp_flag);
        }
    }

    private static void gmGmkBoss3PillarWallChangeModeWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        obj_work.spd.y = 0;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarWallMainWait);
        if (s3PillarWallWork.effect_work == null)
            return;
        AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)s3PillarWallWork.effect_work);
        s3PillarWallWork.effect_work = (AppMain.GMS_EFFECT_3DES_WORK)null;
    }

    private static void gmGmkBoss3PillarWallChangeModeActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        int num1 = 917504;
        s3PillarWallWork.target_pos.y = s3PillarWallWork.default_pos.y - num1;
        obj_work.spd.y = -12288;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarWallMainActive);
        AppMain.gmGmkBoss3PillarEffectCreateWallAppear(obj_work);
        if (((int)obj_work.user_flag & 4) != 0)
        {
            if (s3PillarWallWork.se_handle != null)
                AppMain.GmSoundPlaySE("Boss3_01", s3PillarWallWork.se_handle);
            obj_work.user_flag &= 4294967291U;
            AppMain.GMM_PAD_VIB_SMALL_NOEND();
        }
        short num2 = -16;
        if ((AppMain.g_gm_main_system.map_fcol.left + (AppMain.g_gm_main_system.map_fcol.right - AppMain.g_gm_main_system.map_fcol.left) / 2) * 4096 > obj_work.pos.x)
            num2 = (short)-32;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = num2;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)48;
    }

    private static void gmGmkBoss3PillarWallChangeModeReturn(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        s3PillarWallWork.target_pos.Assign(s3PillarWallWork.default_pos);
        obj_work.spd.y = 12288;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss3PillarWallMainActive);
        obj_work.user_flag &= 4294967279U;
        AppMain.gmGmkBoss3PillarEffectCreateWallAppear(obj_work);
        if (s3PillarWallWork.se_handle != null)
            AppMain.GmSoundPlaySE("Boss3_01", s3PillarWallWork.se_handle);
        AppMain.GMM_PAD_VIB_SMALL_NOEND();
    }

    private static int gmGmkBoss3PillarWallCheckMoveEnd(
      AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK wall_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)wall_work;
        if (wall_work.target_pos.y == obsObjectWork.pos.y || obsObjectWork.spd.y == 0)
            return 0;
        int num = wall_work.target_pos.y - obsObjectWork.pos.y;
        if (obsObjectWork.spd.y < 0)
        {
            if (num >= 0)
            {
                obsObjectWork.pos.y = wall_work.target_pos.y;
                return 0;
            }
        }
        else if (obsObjectWork.spd.y > 0 && num <= 0)
        {
            obsObjectWork.pos.y = wall_work.target_pos.y;
            return 0;
        }
        return num;
    }

    private static void gmGmkBoss3PillarWallMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkBoss3PillarWallMainActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK wall_work = (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        int a = AppMain.gmGmkBoss3PillarWallCheckMoveEnd(wall_work);
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_main_system.ply_work[0];
        if (obj_work.spd.y < 0 && AppMain.MTM_MATH_ABS(obsObjectWork.pos.x - obj_work.pos.x) < 65536 && (obsObjectWork.pos.y <= obj_work.pos.y && AppMain.MTM_MATH_ABS(a) < 262144))
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
        if (a != 0)
            return;
        obj_work.pos.Assign(wall_work.target_pos);
        AppMain.gmGmkBoss3PillarWallChangeModeWait(obj_work);
        if (((int)obj_work.user_flag & 1) != 0)
        {
            AppMain.gmGmkBoss3PillarEffectCreateWallHit(obj_work);
            obj_work.user_flag &= 4294967294U;
        }
        AppMain.GMM_PAD_VIB_STOP();
        if (((int)obj_work.user_flag & 2) != 0)
        {
            AppMain.GmCameraVibrationSet(0, 12288, 0);
            obj_work.user_flag &= 4294967293U;
        }
        if (wall_work.se_handle == null)
            return;
        AppMain.GmSoundStopSE(wall_work.se_handle);
        if (((int)obj_work.user_flag & 8) == 0)
            return;
        AppMain.GmSoundPlaySE("Boss3_02", wall_work.se_handle);
        obj_work.user_flag &= 4294967287U;
    }

    private static void gmGmkBoss3PillarWallDrawBack(
      float left,
      float top,
      float right,
      float bottom,
      float z)
    {
        AppMain.AMS_PARAM_DRAW_PRIMITIVE setParam = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        setParam.aTest = (short)0;
        setParam.zMask = (short)0;
        setParam.zTest = (short)1;
        setParam.ablend = 0;
        setParam.noSort = (short)1;
        AppMain.NNS_PRIM3D_PC[] nnsPriM3DPcArray = AppMain.amDrawAlloc_NNS_PRIM3D_PC(6);
        AppMain.amVectorSet(nnsPriM3DPcArray[0], left, top, z);
        AppMain.amVectorSet(nnsPriM3DPcArray[1], right, top, z);
        AppMain.amVectorSet(nnsPriM3DPcArray[2], left, bottom, z);
        AppMain.amVectorSet(nnsPriM3DPcArray[5], right, bottom, z);
        uint num = AppMain.AMD_RGBA8888((byte)0, (byte)0, (byte)0, byte.MaxValue);
        nnsPriM3DPcArray[0].Col = num;
        nnsPriM3DPcArray[1].Col = num;
        nnsPriM3DPcArray[2].Col = num;
        nnsPriM3DPcArray[5].Col = num;
        nnsPriM3DPcArray[3] = nnsPriM3DPcArray[1];
        nnsPriM3DPcArray[4] = nnsPriM3DPcArray[2];
        setParam.format3D = 2;
        setParam.type = 0;
        setParam.vtxPC3D = nnsPriM3DPcArray;
        setParam.texlist = (AppMain.NNS_TEXLIST)null;
        setParam.texId = 0;
        setParam.count = 6;
        setParam.sortZ = -1f;
        AppMain.gmGmkBoss3PillarWallMatrixPush(0U);
        AppMain.amDrawPrimitive3D(0U, setParam);
        AppMain.gmGmkBoss3PillarWallMatrixPop(0U);
        AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Release(setParam);
    }

    private static void gmGmkBoss3PillarWallMatrixPush(uint command_state)
    {
        AppMain.ObjDraw3DNNUserFunc(new AppMain.OBF_DRAW_USER_DT_FUNC(AppMain.gmGmkBoss3PillarWallUserFuncMatrixPush), (object)null, 0, command_state);
    }

    private static void gmGmkBoss3PillarWallMatrixPop(uint command_state)
    {
        AppMain.ObjDraw3DNNUserFunc(new AppMain.OBF_DRAW_USER_DT_FUNC(AppMain.gmGmkBoss3PillarWallUserFuncPop), (object)null, 0, command_state);
    }

    private static void gmGmkBoss3PillarWallUserFuncMatrixPush(object param)
    {
        AppMain.UNREFERENCED_PARAMETER(param);
        AppMain.amMatrixPush();
        AppMain.NNS_MATRIX current = AppMain.amMatrixGetCurrent();
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnMultiplyMatrix(nnsMatrix, AppMain.amDrawGetWorldViewMatrix(), current);
        AppMain.nnSetPrimitive3DMatrix(nnsMatrix);
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmGmkBoss3PillarWallUserFuncPop(object param)
    {
        AppMain.amMatrixPop();
    }

    private static void gmGmkBoss3PillarEffectCreatePillarAppear(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        if (s3PillarMainWork.effect_work != null)
            return;
        int x = s3PillarMainWork.default_pos.x;
        int y = s3PillarMainWork.default_pos.y;
        int num1 = 0;
        uint num2 = 0;
        uint num3 = AppMain.gmGmkBoss3PillarCalcPillarType((int)s3PillarMainWork.gimmick_work.ene_com.eve_rec.id);
        int zone_no = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork;
        if (zone_no == 2)
        {
            switch (num3)
            {
                case 0:
                    x += 188416;
                    num1 = 14;
                    num2 = 1U;
                    break;
                case 1:
                    x -= 188416;
                    num1 = 14;
                    break;
                case 2:
                    y += 188416;
                    num1 = 15;
                    break;
                case 3:
                    y -= 188416;
                    num1 = 13;
                    break;
            }
            gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, zone_no, num1);
        }
        else
        {
            switch (num3)
            {
                case 0:
                    x += 229376;
                    num1 = 45;
                    num2 = 1U;
                    break;
                case 1:
                    x -= 229376;
                    num1 = 45;
                    break;
                case 2:
                    y += 229376;
                    num1 = 46;
                    break;
                case 3:
                    y -= 229376;
                    num1 = 44;
                    break;
            }
            gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, num1);
        }
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)gmsEffect3DesWork;
        obsObjectWork.pos.x = x;
        obsObjectWork.pos.y = y;
        obsObjectWork.pos.z = -524288;
        obsObjectWork.disp_flag |= num2;
        s3PillarMainWork.effect_work = gmsEffect3DesWork;
    }

    private static void gmGmkBoss3PillarEffectCreatePillarHit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK s3PillarMainWork = (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)obj_work;
        int x = s3PillarMainWork.target_pos.x;
        int y = s3PillarMainWork.target_pos.y;
        ushort num1 = 0;
        uint num2 = AppMain.gmGmkBoss3PillarCalcPillarType((int)s3PillarMainWork.gimmick_work.ene_com.eve_rec.id);
        int zone_no = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork;
        if (zone_no == 2)
        {
            int efct_zone_idx = 16;
            switch (num2)
            {
                case 0:
                    x += 122880;
                    num1 = (ushort)AppMain.NNM_DEGtoA16(-90f);
                    break;
                case 1:
                    x -= 122880;
                    num1 = (ushort)AppMain.NNM_DEGtoA16(90f);
                    break;
                case 2:
                    y += 122880;
                    break;
                case 3:
                    y -= 122880;
                    num1 = (ushort)AppMain.NNM_DEGtoA16(180f);
                    break;
            }
            gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, zone_no, efct_zone_idx);
        }
        else
        {
            int efct_cmn_idx = 47;
            switch (num2)
            {
                case 0:
                    x += 163840;
                    num1 = (ushort)AppMain.NNM_DEGtoA16(-90f);
                    break;
                case 1:
                    x -= 163840;
                    num1 = (ushort)AppMain.NNM_DEGtoA16(90f);
                    break;
                case 2:
                    y += 163840;
                    break;
                case 3:
                    y -= 163840;
                    num1 = (ushort)AppMain.NNM_DEGtoA16(180f);
                    break;
            }
            gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, efct_cmn_idx);
        }
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)gmsEffect3DesWork;
        obsObjectWork.pos.x = x;
        obsObjectWork.pos.y = y;
        obsObjectWork.pos.z = 655360;
        obsObjectWork.dir.z = num1;
    }

    private static void gmGmkBoss3PillarEffectCreateWallAppear(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        if (s3PillarWallWork.effect_work != null)
            return;
        int x = s3PillarWallWork.default_pos.x;
        int y = s3PillarWallWork.default_pos.y;
        int zone_no = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        int num;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork;
        if (zone_no == 2)
        {
            num = y - 196608;
            gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, zone_no, 13);
        }
        else
        {
            num = y - 131072;
            gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 44);
        }
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)gmsEffect3DesWork;
        obsObjectWork.pos.x = x;
        obsObjectWork.pos.y = num;
        obsObjectWork.pos.z = 393216;
        s3PillarWallWork.effect_work = gmsEffect3DesWork;
    }

    private static void gmGmkBoss3PillarEffectCreateWallHit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK s3PillarWallWork = (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)obj_work;
        int x = s3PillarWallWork.target_pos.x;
        int y = s3PillarWallWork.target_pos.y;
        int zone_no = AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id];
        int num;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork;
        if (zone_no == 2)
        {
            num = y - 65536;
            gmsEffect3DesWork = AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, zone_no, 16);
        }
        else
        {
            num = y;
            gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 47);
        }
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)gmsEffect3DesWork;
        obsObjectWork.pos.x = x;
        obsObjectWork.pos.y = num;
        obsObjectWork.pos.z = 655360;
        obsObjectWork.dir.z = (ushort)AppMain.NNM_DEGtoA16(180f);
    }

}