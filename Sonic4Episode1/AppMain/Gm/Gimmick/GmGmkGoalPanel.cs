using System;

public partial class AppMain
{
    private static int __initial_y;
    private static int __initial_spd_m;

    private static OBS_OBJECT_WORK GmGmkGoalPanelInit(
         GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_ENEMY_3D_WORK(), "GMK_GOAL_PANEL");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        gmsEnemy3DWork.ene_com.enemy_flag |= 65536U;
        ObjObjectCopyAction3dNNModel(work, gm_gmk_goal_panel_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.flag |= 16U;
        work.dir.y = 32768;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGoalPanelMain);
        GmGmkSplRingMake(pos_x + 393216, pos_y - 393216);
        __initial_y = pos_y;

        return work;
    }

    public static void GmGmkGoalPanelBuild()
    {
        gm_gmk_goal_panel_obj_3d_list = GmGameDBuildRegBuildModel(GmGameDatGetGimmickData(836), GmGameDatGetGimmickData(837), 0U);
    }

    public static void GmGmkGoalPanelFlush()
    {
        AMS_AMB_HEADER amsAmbHeader = readAMBFile(GmGameDatGetGimmickData(836));
        GmGameDBuildRegFlushModel(gm_gmk_goal_panel_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkGoalPanelMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
        if (obj_work.pos.x >= ply_work.obj_work.pos.x)
            return;
        SaveState.deleteSave();
        if ((ply_work.player_flag & 16384) != 0)
            g_gm_main_system.game_flag |= 33554432U;
        else
            g_gm_main_system.game_flag &= 4261412863U;
        HgTrophyTryAcquisition(1);
        GmPlayerSetGoalState(ply_work);
        g_gm_main_system.game_flag &= 4294966271U;
        g_gm_main_system.game_flag |= 1048576U;
        obj_work.user_work = 4096U;
        obj_work.user_timer = 120;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGoalPanelPass);

        __initial_spd_m = ply_work.obj_work.spd_m;
        var options = gs.backup.SSave.CreateInstance().GetRemaster();

        GmGmkCamScrLimitSet(new GMS_EVE_RECORD_EVENT()
        {
            flag = 5,
            left = options.BugFixes ? (sbyte)-104 : (sbyte)-112,
            top = options.BugFixes ? (sbyte)-104 : (sbyte)-96, // actually center stuff
            width = 192,
            height = 112
        }, obj_work.pos.x, obj_work.pos.y);
        gm_gmk_goal_panel_effct = GmEfctCmnEsCreate(obj_work, 32);
        GmEffect3DESSetDispOffset(gm_gmk_goal_panel_effct, 0.0f, 30f, 15f);
        GmEffect3DESSetDispRotation(gm_gmk_goal_panel_effct, 0, 0, 0);
        GMM_PAD_VIB_SMALL();
        GmSoundPlaySE("GoalPanel");
    }

    private static OBS_OBJECT_WORK GmGmkCamScrLimitSet(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y)
    {
        return GmEventMgrLocalEventBirth(302, pos_x, pos_y, eve_rec.flag, eve_rec.left, eve_rec.top, eve_rec.width, eve_rec.height, 0);
    }

    private static void gmGmkGoalPanelPass(OBS_OBJECT_WORK obj_work)
    {
        --obj_work.user_timer;
        if (obj_work.user_timer <= 0)
        {
            obj_work.user_timer = 0;
            obj_work.user_work = 0U;
            obj_work.dir.y = 0;
            obj_work.user_timer = 120;
            obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkGoalPanelWait);
            gmGmkGoalPanelEfctKill();
            GmPlySeqChangeActGoal(g_gm_main_system.ply_work[0]);
        }
        obj_work.dir.y += (ushort)obj_work.user_work;

        var options = gs.backup.SSave.CreateInstance().GetRemaster();
        if (options.Misc && gm_gmk_goal_panel_effct != null)
        {
            GMS_PLAYER_WORK ply_work = g_gm_main_system.ply_work[0];
            float multiplier = (__initial_spd_m / (float)ply_work.spd_max);
            float panelMax = (330_000 * multiplier);
            float effectMax = (60 * multiplier); // best guess i've ever made

            if (obj_work.user_timer > 100)
            {
                var progress = easeOutCirc((120 - obj_work.user_timer) / 20f);

                obj_work.pos.y = __initial_y - (int)(panelMax * progress);
                GmEffect3DESSetDispOffset(gm_gmk_goal_panel_effct, 0.0f, 30f + (effectMax * progress), 15f);
            }
            else
            {
                var progress = easeOutBounce(((120 - obj_work.user_timer) - 20) / 100f);

                obj_work.pos.y = __initial_y - (int)panelMax - (int)(-panelMax * progress);
                GmEffect3DESSetDispOffset(gm_gmk_goal_panel_effct, 0.0f, 30f + effectMax - (effectMax * progress), 15f);
            }
        }
    }

    static float easeOutCirc(float x)
    {
        return (float)Math.Sqrt(1f - Math.Pow(x - 1f, 2));
    }

    static float easeOutBounce(float x)
    {
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }

    private static void gmGmkGoalPanelWait(OBS_OBJECT_WORK obj_work)
    {
        if (--obj_work.user_timer > 0)
        {
            return;
        }
        g_gm_main_system.game_flag |= 4U;
        obj_work.ppFunc = null;
        gmGmkGoalPanelEfctKill();
    }

    private static void gmGmkGoalPanelEfctKill()
    {
        if (gm_gmk_goal_panel_effct == null)
            return;
        ObjDrawKillAction3DES((OBS_OBJECT_WORK)gm_gmk_goal_panel_effct);
        gm_gmk_goal_panel_effct = null;
    }


}