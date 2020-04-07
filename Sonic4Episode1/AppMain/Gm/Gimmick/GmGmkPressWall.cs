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
    private static AppMain.OBS_OBJECT_WORK GmGmkPressWallInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.GMS_GMK_PWALL_WORK work = (AppMain.GMS_GMK_PWALL_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_PWALL_WORK()), "Gmk_PressWall");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == 2)
        {
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_presswall_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWall_ppOut);
            if (eve_rec.height == (byte)0)
            {
                work.wall_height = 0;
                obj_work.pos.y -= 524288;
            }
            else
            {
                work.wall_height = (int)eve_rec.height * 64 * 4096;
                obj_work.pos.y -= work.wall_height;
            }
            obj_work.pos.z = 913408;
        }
        else
        {
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_presswall_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
            obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWall_ppOut);
            AppMain.ObjAction3dNNMaterialMotionLoad(obj_work.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 1, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(895).pData);
            AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
            obj_work.disp_flag |= 4U;
            if (eve_rec.height == (byte)0)
            {
                work.wall_height = 0;
                obj_work.pos.y -= 786432;
            }
            else
            {
                work.wall_height = (int)eve_rec.height * 192 * 4096;
                obj_work.pos.y -= work.wall_height;
            }
            obj_work.pos.z = 1044480;
        }
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        obj_work.flag |= 2U;
        work.wall_speed = eve_rec.width == (byte)0 ? 4096 : (int)eve_rec.width * 4096 / 10;
        AppMain.mtTaskChangeTcbDestructor(obj_work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmGmkPressWallExit));
        work.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
        AppMain.gmGmkPressWallStart(obj_work);
        return obj_work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPressWallStopInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == 2 && AppMain.pwall == null)
            AppMain.GmGmkPressWallInit(eve_rec, pos_x, pos_y, type).user_flag = 1U;
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "Gmk_PressWallStopper");
        work.user_flag = 0U;
        if (((int)eve_rec.flag & 1) != 0)
            work.user_flag = 1U;
        AppMain.gmGmkPressWallStopperStart(work);
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPressWallControlerInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_PWALLCTRL_WORK()), "Gmk_PressWallControler");
        AppMain.GMS_GMK_PWALLCTRL_WORK gmkPwallctrlWork = (AppMain.GMS_GMK_PWALLCTRL_WORK)work;
        AppMain.OBS_RECT_WORK pRec = ((AppMain.GMS_ENEMY_3D_WORK)work).ene_com.rect_work[2];
        pRec.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        pRec.flag &= 4294967291U;
        if (eve_rec.left != (sbyte)0)
        {
            AppMain.ObjRectWorkSet(pRec, (short)((int)eve_rec.left * 2), (short)0, (short)((int)eve_rec.width * 2), (short)1);
            work.user_flag = 0U;
            work.user_timer = (int)eve_rec.height * 819;
        }
        else
        {
            pRec.ppDef = (AppMain.OBS_RECT_WORK_Delegate1)null;
            pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
            AppMain.ObjRectWorkSet(pRec, (short)0, (short)((int)eve_rec.top * 2), (short)1, (short)((int)eve_rec.height * 2));
            gmkPwallctrlWork.line_top = (int)eve_rec.top * 2 * 4096 + work.pos.y;
            gmkPwallctrlWork.line_bottom = (int)eve_rec.height * 2 * 4096 + work.pos.y;
            work.user_flag = 1U;
            work.user_timer = (int)eve_rec.width * 819;
        }
        work.flag &= 4294967293U;
        if (((int)eve_rec.flag & 1) != 0)
            work.user_flag |= 2U;
        if (((int)eve_rec.flag & 2) != 0)
            work.user_flag |= 4U;
        AppMain.gmGmkPressWallControlerStart(work);
        return work;
    }

    public static void GmGmkPressWallBuild()
    {
        AppMain.gm_gmk_presswall_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(892), AppMain.GmGameDatGetGimmickData(893), 0U);
        AppMain.pwall = (AppMain.GMS_GMK_PWALL_WORK)null;
    }

    public static void GmGmkPressWallFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(892);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_presswall_obj_3d_list, gimmickData.file_num);
    }

    private static void gmGmkPressWall_ppOut(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALL_WORK gmsGmkPwallWork = (AppMain.GMS_GMK_PWALL_WORK)obj_work;
        obj_work.pos.y = gmsGmkPwallWork.master_posy;
        obj_work.pos.y += obj_work.user_timer;
        if (gmsGmkPwallWork.wall_height == 0)
        {
            while (obj_work.pos.y + 786432 < AppMain.g_obj.camera[0][1])
                obj_work.pos.y += 786432;
            while (obj_work.pos.y > AppMain.g_obj.camera[0][1])
                obj_work.pos.y -= 786432;
            for (int index = obj_work.pos.y - AppMain.g_obj.camera[0][1]; index < 1048576; index += 786432)
            {
                AppMain.ObjDrawActionSummary(obj_work);
                obj_work.pos.y += 786432;
            }
        }
        else
        {
            for (int index = 0; index < gmsGmkPwallWork.wall_height; index += 786432)
            {
                AppMain.ObjDrawActionSummary(obj_work);
                obj_work.pos.y += 786432;
            }
        }
        obj_work.pos.y = AppMain.g_obj.camera[0][1];
    }

    private static void gmGmkPressWallStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.g_obj.camera[0][0] < obj_work.pos.x && (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] != 2 || obj_work.user_flag == 0U))
            return;
        AppMain.GMS_GMK_PWALL_WORK gmsGmkPwallWork = (AppMain.GMS_GMK_PWALL_WORK)obj_work;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.obj = obj_work;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.diff_data = AppMain.g_gm_default_col;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.width = (ushort)192;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.ofst_x = (short)-192;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.height = (ushort)256;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.ofst_y = (short)0;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.flag |= 134217824U;
        gmsGmkPwallWork.gmk_work.ene_com.col_work.obj_col.attr &= (ushort)65534;
        obj_work.disp_flag &= 4294967263U;
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == 2 && gmsGmkPwallWork.wall_height > 0)
            AppMain.gmGmkPressWallCreateRail(obj_work, gmsGmkPwallWork.wall_height, gmsGmkPwallWork.master_posy);
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == 3)
        {
            AppMain.gmGmkPressWallCreateParts(obj_work, gmsGmkPwallWork.master_posy, gmsGmkPwallWork.wall_height);
            gmsGmkPwallWork.gmk_work.ene_com.rect_work[2].flag &= 4294967291U;
            gmsGmkPwallWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
            gmsGmkPwallWork.gmk_work.ene_com.rect_work[1].flag |= 4U;
            AppMain.OBS_RECT_WORK pRec = gmsGmkPwallWork.gmk_work.ene_com.rect_work[1];
            AppMain.ObjRectWorkZSet(pRec, (short)-16, (short)0, (short)-32, (short)0, (short)192, (short)32);
            pRec.flag |= 4U;
            pRec.flag |= 1024U;
            obj_work.flag &= 4294967293U;
            pRec.ppHit = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkPressWallZ4Hit);
        }
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == 2 && obj_work.user_flag != 0U)
        {
            obj_work.user_flag_OBJECT = (object)null;
        }
        else
        {
            AppMain.GMM_PAD_VIB_MID_TIME(60f);
            gmsGmkPwallWork.se_handle = AppMain.GsSoundAllocSeHandle();
            AppMain.GmSoundPlaySEForce("MovingWall", gmsGmkPwallWork.se_handle);
        }
        gmsGmkPwallWork.efct_obj = (AppMain.OBS_OBJECT_WORK)null;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallForce);
        AppMain.gmGmkPressWallForce(obj_work);
    }

    private static void gmGmkPressWallForce(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALL_WORK gmsGmkPwallWork = (AppMain.GMS_GMK_PWALL_WORK)obj_work;
        if (((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
        {
            gmsGmkPwallWork.ply_death = true;
            gmsGmkPwallWork.wall_speed = 0;
            AppMain.gmGmkPressWallSeStop(obj_work);
        }
        if (AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id] == 2)
        {
            if (gmsGmkPwallWork.wall_speed == 0 || gmsGmkPwallWork.ply_death)
            {
                gmsGmkPwallWork.wall_vibration &= (short)3;
                gmsGmkPwallWork.wall_vibration += (short)3;
                gmsGmkPwallWork.wall_brake = gmsGmkPwallWork.wall_speed;
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallForce_100);
                AppMain.gmGmkPressWallForce_100(obj_work);
                return;
            }
            obj_work.pos.x += gmsGmkPwallWork.wall_speed;
            obj_work.user_timer = AppMain.wall_vib[(int)gmsGmkPwallWork.wall_vibration & 7];
            ++gmsGmkPwallWork.wall_vibration;
            if (gmsGmkPwallWork.wall_effect_build_timer == (short)0)
            {
                AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctZoneEsCreate((AppMain.OBS_OBJECT_WORK)null, 2, 32);
                obsObjectWork.pos.x = obj_work.pos.x;
                obsObjectWork.pos.y = AppMain.g_obj.camera[0][1];
                obsObjectWork.pos.z = obj_work.pos.z;
                obsObjectWork.spd.x = gmsGmkPwallWork.wall_speed;
                gmsGmkPwallWork.wall_effect_build_timer = (short)(((int)AppMain.mtMathRand() & 63) + 90);
            }
            --gmsGmkPwallWork.wall_effect_build_timer;
            obj_work.pos.y = AppMain.g_obj.camera[0][1];
        }
        else
        {
            obj_work.pos.x += gmsGmkPwallWork.wall_speed;
            if (gmsGmkPwallWork.ply_death || gmsGmkPwallWork.wall_speed == 0)
            {
                if (gmsGmkPwallWork.efct_obj != null)
                {
                    AppMain.ObjDrawKillAction3DES(gmsGmkPwallWork.efct_obj);
                    gmsGmkPwallWork.efct_obj = (AppMain.OBS_OBJECT_WORK)null;
                    gmsGmkPwallWork.wall_effect_build_timer = (short)0;
                }
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallForceZ4_Stop);
            }
            else if (((int)obj_work.user_flag & 1) != 0 || gmsGmkPwallWork.ply_death)
            {
                gmsGmkPwallWork.wall_brake = gmsGmkPwallWork.wall_speed;
                obj_work.user_flag &= 4294967294U;
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallForceZ4_Hit);
                if (gmsGmkPwallWork.efct_obj != null)
                {
                    AppMain.ObjDrawKillAction3DES(gmsGmkPwallWork.efct_obj);
                    gmsGmkPwallWork.efct_obj = (AppMain.OBS_OBJECT_WORK)null;
                    gmsGmkPwallWork.wall_effect_build_timer = (short)0;
                }
            }
            else
                --gmsGmkPwallWork.wall_effect_build_timer;
        }
        if (gmsGmkPwallWork == AppMain.pwall)
            return;
        obj_work.flag |= 8U;
        gmsGmkPwallWork.gmk_work.ene_com.enemy_flag |= 65536U;
        AppMain.gmGmkPressWallSeStop(obj_work);
    }

    private static void gmGmkPressWallForce_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALL_WORK gmsGmkPwallWork = (AppMain.GMS_GMK_PWALL_WORK)obj_work;
        gmsGmkPwallWork.wall_brake -= gmsGmkPwallWork.wall_speed / 20;
        obj_work.pos.x += gmsGmkPwallWork.wall_brake;
        if (gmsGmkPwallWork.wall_vibration < (short)20)
        {
            obj_work.user_timer = AppMain.wall_vib[(int)gmsGmkPwallWork.wall_vibration];
            ++gmsGmkPwallWork.wall_vibration;
        }
        else
        {
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallForce_200);
            AppMain.gmGmkPressWallSeStop(obj_work);
        }
    }

    private static void gmGmkPressWallForce_200(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALL_WORK gmsGmkPwallWork = (AppMain.GMS_GMK_PWALL_WORK)obj_work;
        if (gmsGmkPwallWork == AppMain.pwall)
            return;
        obj_work.flag |= 8U;
        gmsGmkPwallWork.gmk_work.ene_com.enemy_flag |= 65536U;
    }

    private static void gmGmkPressWallForceZ4_Hit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALL_WORK gmsGmkPwallWork = (AppMain.GMS_GMK_PWALL_WORK)obj_work;
        if (((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
        {
            gmsGmkPwallWork.ply_death = true;
            gmsGmkPwallWork.wall_speed = 0;
            AppMain.gmGmkPressWallSeStop(obj_work);
        }
        gmsGmkPwallWork.wall_brake -= gmsGmkPwallWork.wall_speed / 64;
        if (gmsGmkPwallWork.wall_brake <= 0 || gmsGmkPwallWork.wall_speed == 0)
        {
            gmsGmkPwallWork.wall_brake = 0;
            obj_work.ppFunc = gmsGmkPwallWork.ply_death || gmsGmkPwallWork.wall_speed == 0 ? new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallForceZ4_Stop) : new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallForceZ4_Hit_100);
        }
        if ((double)gmsGmkPwallWork.gmk_work.obj_3d.speed[0] > 0.0)
        {
            gmsGmkPwallWork.gmk_work.obj_3d.speed[0] -= 1f / 64f;
            gmsGmkPwallWork.gmk_work.obj_3d.speed[1] -= 1f / 64f;
            ++gmsGmkPwallWork.mat_timer;
            if (gmsGmkPwallWork.mat_timer > gmsGmkPwallWork.mat_timer_line)
            {
                obj_work.disp_flag &= 4294963199U;
                gmsGmkPwallWork.mat_timer_line = gmsGmkPwallWork.mat_timer;
                gmsGmkPwallWork.mat_timer = 0U;
            }
            else
                obj_work.disp_flag |= 4096U;
        }
        obj_work.pos.x += gmsGmkPwallWork.wall_brake;
    }

    private static void gmGmkPressWallForceZ4_Hit_100(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALL_WORK gmsGmkPwallWork = (AppMain.GMS_GMK_PWALL_WORK)obj_work;
        if (((int)AppMain.g_gm_main_system.ply_work[0].player_flag & 1024) != 0)
        {
            gmsGmkPwallWork.ply_death = true;
            gmsGmkPwallWork.wall_speed = 0;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallForceZ4_Stop);
        }
        else
        {
            gmsGmkPwallWork.wall_brake += gmsGmkPwallWork.wall_speed / 64;
            if (gmsGmkPwallWork.wall_brake >= gmsGmkPwallWork.wall_speed)
            {
                gmsGmkPwallWork.wall_brake = gmsGmkPwallWork.wall_speed;
                gmsGmkPwallWork.gmk_work.obj_3d.speed[0] = 1f;
                gmsGmkPwallWork.gmk_work.obj_3d.speed[1] = 1f;
                gmsGmkPwallWork.mat_timer = 0U;
                gmsGmkPwallWork.mat_timer_line = 0U;
                obj_work.disp_flag &= 4294963199U;
                obj_work.flag &= 4294967293U;
                gmsGmkPwallWork.ply_death = false;
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallForce);
            }
            if ((double)gmsGmkPwallWork.gmk_work.obj_3d.speed[0] < 1.0)
            {
                gmsGmkPwallWork.gmk_work.obj_3d.speed[0] += 1f / 64f;
                gmsGmkPwallWork.gmk_work.obj_3d.speed[1] += 1f / 64f;
                ++gmsGmkPwallWork.mat_timer;
                if (gmsGmkPwallWork.mat_timer > gmsGmkPwallWork.mat_timer_line)
                {
                    obj_work.disp_flag |= 4096U;
                    gmsGmkPwallWork.mat_timer_line = gmsGmkPwallWork.mat_timer;
                    gmsGmkPwallWork.mat_timer = 0U;
                }
                else
                    obj_work.disp_flag &= 4294963199U;
            }
            obj_work.pos.x += gmsGmkPwallWork.wall_brake;
        }
    }

    private static void gmGmkPressWallForceZ4_Stop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALL_WORK gmsGmkPwallWork = (AppMain.GMS_GMK_PWALL_WORK)obj_work;
        if ((double)gmsGmkPwallWork.gmk_work.obj_3d.speed[0] <= 0.0)
            return;
        gmsGmkPwallWork.gmk_work.obj_3d.speed[0] -= 1f / 64f;
        gmsGmkPwallWork.gmk_work.obj_3d.speed[1] -= 1f / 64f;
        ++gmsGmkPwallWork.mat_timer;
        if (gmsGmkPwallWork.mat_timer > gmsGmkPwallWork.mat_timer_line)
        {
            obj_work.disp_flag &= 4294963199U;
            gmsGmkPwallWork.mat_timer_line = gmsGmkPwallWork.mat_timer;
            gmsGmkPwallWork.mat_timer = 0U;
        }
        else
            obj_work.disp_flag |= 4096U;
        if ((double)gmsGmkPwallWork.gmk_work.obj_3d.speed[0] > 0.0)
            return;
        gmsGmkPwallWork.gmk_work.obj_3d.speed[0] = 0.0f;
        gmsGmkPwallWork.gmk_work.obj_3d.speed[1] = 0.0f;
        obj_work.disp_flag |= 4096U;
        AppMain.gmGmkPressWallSeStop(obj_work);
    }

    private static void gmGmkPressWallZ4Hit(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj = mine_rect.parent_obj;
        parentObj.flag |= 2U;
        parentObj.user_flag |= 1U;
        AppMain.GmEnemyDefaultAtkFunc(mine_rect, match_rect);
    }

    private static void gmGmkPressWallSeStop(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALL_WORK gmsGmkPwallWork = (AppMain.GMS_GMK_PWALL_WORK)obj_work;
        if (gmsGmkPwallWork.se_handle == null)
            return;
        AppMain.GsSoundStopSeHandle(gmsGmkPwallWork.se_handle);
        AppMain.GsSoundFreeSeHandle(gmsGmkPwallWork.se_handle);
        gmsGmkPwallWork.se_handle = (AppMain.GSS_SND_SE_HANDLE)null;
    }

    private static void gmGmkPressWallStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALL_WORK gmsGmkPwallWork = (AppMain.GMS_GMK_PWALL_WORK)obj_work;
        AppMain.pwall = gmsGmkPwallWork;
        obj_work.flag |= 16U;
        gmsGmkPwallWork.wall_vibration = (short)0;
        gmsGmkPwallWork.wall_effect_build_timer = (short)0;
        gmsGmkPwallWork.master_posy = obj_work.pos.y;
        AppMain.pwall.stop_wall = false;
        obj_work.disp_flag |= 32U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallStay);
    }

    private static void gmGmkPressWallStopperMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.pwall == null)
        {
            obj_work.flag |= 8U;
        }
        else
        {
            if (obj_work.user_flag != 0U && (obj_work.pos.y < AppMain.g_obj.camera[0][1] - 524288 || obj_work.pos.y > AppMain.g_obj.camera[0][1] + 1572864))
                return;
            if (AppMain.pwall.gmk_work.ene_com.obj_work.pos.x > obj_work.pos.x)
            {
                AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
                AppMain.pwall.gmk_work.ene_com.obj_work.pos.x = obj_work.pos.x;
                AppMain.pwall.wall_speed = 0;
                obj_work.flag |= 8U;
                gmsEnemyComWork.enemy_flag |= 65536U;
            }
            if (obj_work.user_work_OBJECT == AppMain.pwall)
                return;
            AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork1 = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
            obj_work.flag |= 8U;
            gmsEnemyComWork1.enemy_flag |= 65536U;
        }
    }

    private static void gmGmkPressWallStopperStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_work_OBJECT = (object)AppMain.pwall;
        obj_work.disp_flag |= 32U;
        obj_work.move_flag |= 8960U;
        obj_work.flag |= 16U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallStopperMain);
    }

    private static void gmGmkPressWallControler(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALLCTRL_WORK gmkPwallctrlWork = (AppMain.GMS_GMK_PWALLCTRL_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK plyWork = gmkPwallctrlWork.ply_work;
        if (AppMain.pwall != null && ((int)obj_work.user_flag & 1) != 0 && (obj_work.pos.x > gmkPwallctrlWork.last_ply_x && obj_work.pos.x <= plyWork.obj_work.pos.x) && (((int)obj_work.user_flag & 2) == 0 || plyWork.obj_work.pos.y >= gmkPwallctrlWork.line_top && plyWork.obj_work.pos.y <= gmkPwallctrlWork.line_bottom))
        {
            if (((int)obj_work.user_flag & 4) != 0 && AppMain.pwall.gmk_work.ene_com.obj_work.pos.x <= AppMain.g_obj.camera[0][0] - 32768)
                AppMain.pwall.gmk_work.ene_com.obj_work.pos.x = AppMain.g_obj.camera[0][0] - 32768;
            AppMain.pwall.wall_speed = obj_work.user_timer;
            if (AppMain.pwall.wall_speed == 0)
                AppMain.pwall.stop_wall = true;
            obj_work.flag |= 8U;
            gmkPwallctrlWork.gmk_work.ene_com.enemy_flag |= 65536U;
        }
        else
        {
            gmkPwallctrlWork.last_ply_x = plyWork.obj_work.pos.x;
            gmkPwallctrlWork.last_ply_y = plyWork.obj_work.pos.y;
        }
    }

    private static void gmGmkPressWallControlerStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PWALLCTRL_WORK gmkPwallctrlWork = (AppMain.GMS_GMK_PWALLCTRL_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        gmkPwallctrlWork.ply_work = gmsPlayerWork;
        gmkPwallctrlWork.last_ply_x = gmsPlayerWork.obj_work.pos.x;
        gmkPwallctrlWork.last_ply_y = gmsPlayerWork.obj_work.pos.y;
        obj_work.disp_flag |= 32U;
        obj_work.move_flag |= 8960U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallControler);
    }

    private static void gmGmkPressWallParts(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.pos.x = obj_work.parent_obj.pos.x;
    }

    private static void gmGmkPressWallRail(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PRESSWALL_PARTS gmkPresswallParts = (AppMain.GMS_GMK_PRESSWALL_PARTS)obj_work;
        AppMain.GMS_GMK_PWALL_WORK parentObj = (AppMain.GMS_GMK_PWALL_WORK)obj_work.parent_obj;
        obj_work.pos.x = obj_work.parent_obj.pos.x;
        obj_work.pos.y = parentObj.master_posy;
        obj_work.pos.y += obj_work.parent_obj.user_timer;
        obj_work.pos.y += gmkPresswallParts.ofst_y;
    }

    private static void gmGmkPressWallCreateRail(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int height,
      int pos_y)
    {
        AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_PRESSWALL_PARTS()), parent_obj, (ushort)0, "PresswallRail-Top");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork1 = (AppMain.GMS_EFFECT_3DNN_WORK)work1;
        AppMain.ObjObjectCopyAction3dNNModel(work1, AppMain.gm_gmk_presswall_obj_3d_list[2], gmsEffect3DnnWork1.obj_3d);
        work1.flag &= 4294966271U;
        work1.pos.y = pos_y;
        work1.pos.z = parent_obj.pos.z + 4096;
        work1.disp_flag |= 4194304U;
        work1.disp_flag |= 256U;
        work1.disp_flag |= 134217728U;
        ((AppMain.GMS_GMK_PRESSWALL_PARTS)work1).ofst_y = -8192;
        work1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallRail);
        AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_PRESSWALL_PARTS()), parent_obj, (ushort)0, "PresswallRail-Botom");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork2 = (AppMain.GMS_EFFECT_3DNN_WORK)work2;
        AppMain.ObjObjectCopyAction3dNNModel(work2, AppMain.gm_gmk_presswall_obj_3d_list[1], gmsEffect3DnnWork2.obj_3d);
        work2.flag &= 4294966271U;
        work2.pos.y = pos_y + height;
        work2.pos.z = parent_obj.pos.z + 4096;
        work2.disp_flag |= 4194304U;
        work2.disp_flag |= 256U;
        work2.disp_flag |= 134217728U;
        ((AppMain.GMS_GMK_PRESSWALL_PARTS)work2).ofst_y = height - 65536;
        work2.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallRail);
    }

    private static void gmGmkPressWallZ4Parts_ppOut(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_PRESSWALL_PARTS gmkPresswallParts = (AppMain.GMS_GMK_PRESSWALL_PARTS)obj_work;
        obj_work.pos.y = gmkPresswallParts.master_posy;
        while (obj_work.pos.y + 786432 < AppMain.g_obj.camera[0][1])
            obj_work.pos.y += 786432;
        while (obj_work.pos.y > AppMain.g_obj.camera[0][1])
            obj_work.pos.y -= 786432;
        for (int index = obj_work.pos.y - AppMain.g_obj.camera[0][1]; index < 1048576; index += 786432)
        {
            AppMain.ObjDrawActionSummary(obj_work);
            obj_work.pos.y += 786432;
        }
    }

    private static void gmGmkPressWallCreateParts(
      AppMain.OBS_OBJECT_WORK parent_obj,
      int pos_y,
      int height)
    {
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)null;
        for (int index = 0; index < 3; ++index)
        {
            obj_work = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_PRESSWALL_PARTS()), parent_obj, (ushort)0, "PresswallZ4Parts");
            AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork = (AppMain.GMS_EFFECT_3DNN_WORK)obj_work;
            AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_presswall_obj_3d_list[(int)AppMain.tbl_gmk_z4PressWall_model[index]], gmsEffect3DnnWork.obj_3d);
            obj_work.flag &= 4294966271U;
            obj_work.pos.y = pos_y;
            obj_work.pos.z = parent_obj.pos.z + AppMain.tbl_gmk_z4PressWall_ofst_z[index];
            obj_work.disp_flag |= 4194304U;
            obj_work.disp_flag |= 256U;
            obj_work.disp_flag |= 134217728U;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallParts);
            ((AppMain.GMS_GMK_PRESSWALL_PARTS)obj_work).master_posy = pos_y;
            if (height == 0)
                obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPressWallZ4Parts_ppOut);
        }
        AppMain.ObjAction3dNNMaterialMotionLoad(obj_work.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(895).pData);
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.obj_3d.mat_speed = 1f;
        obj_work.disp_flag |= 4U;
    }

    private static void gmGmkPressWallExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.OBS_OBJECT_WORK tcbWork = AppMain.mtTaskGetTcbWork(tcb);
        if (AppMain.pwall == (AppMain.GMS_GMK_PWALL_WORK)tcbWork)
            AppMain.pwall = (AppMain.GMS_GMK_PWALL_WORK)null;
        AppMain.gmGmkPressWallSeStop(tcbWork);
        AppMain.GmEnemyDefaultExit(tcb);
    }
}