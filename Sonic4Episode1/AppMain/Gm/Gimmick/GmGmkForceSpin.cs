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
    private static AppMain.OBS_OBJECT_WORK GmGmkForceSpinSetInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_COM_WORK()), "GMK_FORCE_SPIN_SET");
        AppMain.ObjRectSet(((AppMain.GMS_ENEMY_COM_WORK)work).rect_work[2].rect, (short)eve_rec.left, (short)eve_rec.top, (short)((int)eve_rec.width + (int)eve_rec.left), (short)((int)eve_rec.height + (int)eve_rec.top));
        work.move_flag |= 8480U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkForceSpinSetMain);
        return work;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkForceSpinResetInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_COM_WORK()), "GMK_FORCE_SPIN_RESET");
        AppMain.ObjRectSet(((AppMain.GMS_ENEMY_COM_WORK)work).rect_work[2].rect, (short)eve_rec.left, (short)eve_rec.top, (short)((int)eve_rec.width + (int)eve_rec.left), (short)((int)eve_rec.height + (int)eve_rec.top));
        work.move_flag |= 8480U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkForceSpinResetMain);
        return work;
    }

    private static void gmGmkForceSpinSetMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        if (((int)ply_work.player_flag & 1024) != 0 || ((int)ply_work.obj_work.flag & 2) != 0 || (((int)AppMain.g_gm_main_system.game_flag & 262656) != 0 || !AppMain.gmGmkForceSpinRectChk(obj_work, ply_work)) || (ply_work.seq_state == 51 || ply_work.seq_state == 52 || ply_work.seq_state == 53))
            return;
        if (((int)gmsEnemyComWork.eve_rec.flag & 1) != 0)
            AppMain.GmPlySeqGmkInitForceSpinDec(ply_work);
        else
            AppMain.GmPlySeqGmkInitForceSpin(ply_work);
    }

    private static void gmGmkForceSpinResetMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        AppMain.GMS_PLAYER_WORK ply_work = AppMain.g_gm_main_system.ply_work[0];
        if (((int)ply_work.player_flag & 1024) != 0 || ((int)ply_work.obj_work.flag & 2) != 0 || (((int)AppMain.g_gm_main_system.game_flag & 262656) != 0 || !AppMain.gmGmkForceSpinRectChk(obj_work, ply_work)) || ply_work.seq_state != 51 && ply_work.seq_state != 52 && ply_work.seq_state != 53)
            return;
        if (((int)ply_work.obj_work.move_flag & 1) != 0)
        {
            ply_work.no_spddown_timer = 0;
            if (((int)gmsEnemyComWork.eve_rec.flag & 1) != 0)
                AppMain.GmPlySeqChangeSequence(ply_work, 10);
            else
                AppMain.GmPlySeqInitFw(ply_work);
        }
        else
            AppMain.GmPlySeqGmkInitSpinFall(ply_work, ply_work.obj_work.spd.x, ply_work.obj_work.spd.y);
    }

    private static bool gmGmkForceSpinRectChk(
      AppMain.OBS_OBJECT_WORK obj_work,
      AppMain.GMS_PLAYER_WORK ply_work)
    {
        AppMain.OBS_RECT_WORK obsRectWork = ((AppMain.GMS_ENEMY_COM_WORK)obj_work).rect_work[2];
        return ply_work.obj_work.pos.x >= obj_work.pos.x + ((int)obsRectWork.rect.left << 12) && ply_work.obj_work.pos.x <= obj_work.pos.x + ((int)obsRectWork.rect.right << 12) && (ply_work.obj_work.pos.y >= obj_work.pos.y + ((int)obsRectWork.rect.top << 12) && ply_work.obj_work.pos.y <= obj_work.pos.y + ((int)obsRectWork.rect.bottom << 12));
    }

}