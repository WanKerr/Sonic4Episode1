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
    public static AppMain.OBS_OBJECT_WORK GmGmkUpBumperLInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_UPBUMPER_WORK gmsGmkUpbumperWork = (AppMain.GMS_GMK_UPBUMPER_WORK)AppMain.gmGmkUpBumperInit(eve_rec, pos_x, pos_y, type);
        gmsGmkUpbumperWork.obj_type = 0;
        AppMain.gmGmkUpBumperStart(gmsGmkUpbumperWork.gmk_work.ene_com.obj_work);
        return gmsGmkUpbumperWork.gmk_work.ene_com.obj_work;
    }

    public static AppMain.OBS_OBJECT_WORK GmGmkUpBumperRInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.gmGmkUpBumperInit(eve_rec, pos_x, pos_y, type);
        AppMain.GMS_GMK_UPBUMPER_WORK gmsGmkUpbumperWork = (AppMain.GMS_GMK_UPBUMPER_WORK)obsObjectWork;
        obsObjectWork.disp_flag &= 4290772991U;
        obsObjectWork.obj_3d.drawflag |= 32U;
        obsObjectWork.dir.y = (ushort)16384;
        gmsGmkUpbumperWork.obj_type = 1;
        AppMain.gmGmkUpBumperStart(gmsGmkUpbumperWork.gmk_work.ene_com.obj_work);
        return obsObjectWork;
    }

    public static void GmGmkUpBumperBuild()
    {
        AppMain.gm_gmk_upbumper_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(850), AppMain.GmGameDatGetGimmickData(851), 0U);
    }

    public static void GmGmkUpBumperFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(850);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_upbumper_obj_3d_list, gimmickData.file_num);
    }

    private static void gmGmkUpBumperStart(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_UPBUMPER_WORK gmsGmkUpbumperWork = (AppMain.GMS_GMK_UPBUMPER_WORK)obj_work;
        gmsGmkUpbumperWork.gmk_work.ene_com.rect_work[0].flag &= 4294967291U;
        gmsGmkUpbumperWork.gmk_work.ene_com.rect_work[1].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsGmkUpbumperWork.gmk_work.ene_com.rect_work[2];
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkUpBumperHit);
        pRec.ppHit = (AppMain.OBS_RECT_WORK_Delegate1)null;
        AppMain.ObjRectAtkSet(pRec, (ushort)0, (short)0);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec, AppMain.GmkUpBumperData.tbl_gm_gmk_upbumper_rect[gmsGmkUpbumperWork.obj_type][0], AppMain.GmkUpBumperData.tbl_gm_gmk_upbumper_rect[gmsGmkUpbumperWork.obj_type][1], AppMain.GmkUpBumperData.tbl_gm_gmk_upbumper_rect[gmsGmkUpbumperWork.obj_type][2], AppMain.GmkUpBumperData.tbl_gm_gmk_upbumper_rect[gmsGmkUpbumperWork.obj_type][3]);
        obj_work.flag &= 4294967293U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkUpBumperStay);
    }

    private static void gmGmkUpBumperStay(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_GMK_UPBUMPER_WORK gmsGmkUpbumperWork = (AppMain.GMS_GMK_UPBUMPER_WORK)obj_work;
        if (gmsGmkUpbumperWork.player_spd_keep_timer_mine <= (short)0)
            return;
        if ((int)gmsGmkUpbumperWork.player_spd_keep_timer_mine > (int)AppMain.player_spd_keep_timer)
        {
            gmsGmkUpbumperWork.player_spd_keep_timer_mine = AppMain.player_spd_keep_timer;
            --AppMain.player_spd_keep_timer;
            if (AppMain.player_spd_keep_timer > (short)0)
                return;
            gmsGmkUpbumperWork.player_spd_keep_timer_mine = (short)0;
            AppMain.player_spd_keep_timer = (short)0;
            AppMain.player_spd_x = AppMain.player_spd_y = 0;
        }
        else
            gmsGmkUpbumperWork.player_spd_keep_timer_mine = (short)0;
    }

    private static void gmGmkUpBumperHit(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_GMK_UPBUMPER_WORK parentObj1 = (AppMain.GMS_GMK_UPBUMPER_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj2 == AppMain.g_gm_main_system.ply_work[0])
        {
            int spd_y = 0;
            int spd_x;
            if (AppMain.player_spd_keep_timer <= (short)0)
            {
                spd_x = 0;
                for (uint index = 0; (long)index < (long)AppMain.GMD_GMK_UPBUMPER_REBOUND_DATA_NUM; ++index)
                {
                    if (parentObj2.act_state == AppMain.tbl_upbmper_rebound_data[(int)index].act_state)
                    {
                        spd_x = AppMain.tbl_upbmper_rebound_data[(int)index].spd_x;
                        spd_y = AppMain.tbl_upbmper_rebound_data[(int)index].spd_y;
                        AppMain.player_spd_x = spd_x;
                        AppMain.player_spd_y = spd_y;
                        AppMain.player_spd_keep_timer = (short)60;
                        parentObj1.player_spd_keep_timer_mine = (short)((int)AppMain.player_spd_keep_timer + 1);
                        break;
                    }
                }
                if (spd_x == 0)
                {
                    int num = AppMain.MTM_MATH_ABS(parentObj2.obj_work.spd.x);
                    spd_x = num + (num >> 3);
                    if (spd_x > 32768)
                        spd_x = 32768;
                    if (spd_x < 16384)
                        spd_x = 16384;
                    spd_y = -16384;
                }
            }
            else
            {
                spd_x = AppMain.player_spd_x;
                spd_y = AppMain.player_spd_y;
                AppMain.player_spd_keep_timer = (short)60;
                parentObj1.player_spd_keep_timer_mine = (short)((int)AppMain.player_spd_keep_timer + 1);
            }
            if (parentObj1.obj_type == 1)
                spd_x = -spd_x;
            AppMain.GmPlySeqGmkInitUpBumper(parentObj2, spd_x, spd_y);
            AppMain.GMM_PAD_VIB_SMALL();
        }
        mine_rect.flag &= 4294573823U;
    }

    private static AppMain.OBS_OBJECT_WORK gmGmkUpBumperInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_UPBUMPER_WORK work = (AppMain.GMS_GMK_UPBUMPER_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_UPBUMPER_WORK()), "Gmk_UpBumper");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_upbumper_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -4096;
        obj_work.move_flag |= 256U;
        obj_work.disp_flag |= 4194304U;
        return obj_work;
    }

}