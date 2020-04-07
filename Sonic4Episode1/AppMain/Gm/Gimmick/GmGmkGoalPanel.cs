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
    private static AppMain.OBS_OBJECT_WORK GmGmkGoalPanelInit(
         AppMain.GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_GOAL_PANEL");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        gmsEnemy3DWork.ene_com.enemy_flag |= 65536U;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_goal_panel_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 16U;
        work.dir.y = (ushort)32768;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGoalPanelMain);
        AppMain.GmGmkSplRingMake(pos_x + 393216, pos_y - 393216);
        return work;
    }

    public static void GmGmkGoalPanelBuild()
    {
        AppMain.gm_gmk_goal_panel_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(836), AppMain.GmGameDatGetGimmickData(837), 0U);
    }

    public static void GmGmkGoalPanelFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(836));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_goal_panel_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkGoalPanelMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        if (obj_work.pos.x >= ply_work.obj_work.pos.x)
            return;
        SaveState.deleteSave();
        if (((int)ply_work.player_flag & 16384) != 0)
            AppMain.g_gm_main_system.game_flag |= 33554432U;
        else
            AppMain.g_gm_main_system.game_flag &= 4261412863U;
        AppMain.HgTrophyTryAcquisition(1);
        AppMain.GmPlayerSetGoalState(ply_work);
        AppMain.g_gm_main_system.game_flag &= 4294966271U;
        AppMain.g_gm_main_system.game_flag |= 1048576U;
        obj_work.user_work = 4096U;
        obj_work.user_timer = 120;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGoalPanelPass);
        AppMain.GmGmkCamScrLimitSet(new AppMain.GMS_EVE_RECORD_EVENT()
        {
            flag = (ushort)5,
            left = (sbyte)-96,
            top = (sbyte)-104,
            width = (byte)192,
            height = (byte)112
        }, obj_work.pos.x, obj_work.pos.y);
        AppMain.gm_gmk_goal_panel_effct = AppMain.GmEfctCmnEsCreate(obj_work, 32);
        AppMain.GmEffect3DESSetDispOffset(AppMain.gm_gmk_goal_panel_effct, 0.0f, 30f, 15f);
        AppMain.GmEffect3DESSetDispRotation(AppMain.gm_gmk_goal_panel_effct, (short)0, (short)0, (short)0);
        AppMain.GMM_PAD_VIB_SMALL();
        AppMain.GmSoundPlaySE("GoalPanel");
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkCamScrLimitSet(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y)
    {
        return AppMain.GmEventMgrLocalEventBirth((ushort)302, pos_x, pos_y, eve_rec.flag, eve_rec.left, eve_rec.top, eve_rec.width, eve_rec.height, (byte)0);
    }

    private static void gmGmkGoalPanelPass(AppMain.OBS_OBJECT_WORK obj_work)
    {
        --obj_work.user_timer;
        if (obj_work.user_timer <= 0)
        {
            obj_work.user_timer = 0;
            obj_work.user_work = 0U;
            obj_work.dir.y = (ushort)0;
            obj_work.user_timer = 120;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkGoalPanelWait);
            AppMain.gmGmkGoalPanelEfctKill();
            AppMain.GmPlySeqChangeActGoal(AppMain.g_gm_main_system.ply_work[0]);
        }
        obj_work.dir.y += (ushort)obj_work.user_work;
    }

    private static void gmGmkGoalPanelWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (--obj_work.user_timer > 0)
            return;
        AppMain.g_gm_main_system.game_flag |= 4U;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        AppMain.gmGmkGoalPanelEfctKill();
    }

    private static void gmGmkGoalPanelEfctKill()
    {
        if (AppMain.gm_gmk_goal_panel_effct == null)
            return;
        AppMain.ObjDrawKillAction3DES((AppMain.OBS_OBJECT_WORK)AppMain.gm_gmk_goal_panel_effct);
        AppMain.gm_gmk_goal_panel_effct = (AppMain.GMS_EFFECT_3DES_WORK)null;
    }


}