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
    private static AppMain.OBS_OBJECT_WORK GmGmkBreakFloorInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.GMS_GMK_BWALL_WORK work = (AppMain.GMS_GMK_BWALL_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BWALL_WORK()), "GMK_BREAK_LAND_MAIN");
        AppMain.OBS_OBJECT_WORK obj_work = (AppMain.OBS_OBJECT_WORK)work;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        ushort num = AppMain.tbl_breakwall_mdl[AppMain.g_gm_gamedat_zone_type_tbl[(int)AppMain.g_gs_main_sys_info.stage_id]][6];
        AppMain.ObjObjectCopyAction3dNNModel(obj_work, AppMain.gm_gmk_breakwall_obj_3d_list[(int)num], gmsEnemy3DWork.obj_3d);
        obj_work.pos.z = -131072;
        if (((int)eve_rec.flag & 2) != 0)
            obj_work.pos.z -= 4096;
        obj_work.move_flag |= 8448U;
        obj_work.disp_flag |= 4194304U;
        work.broketype = (ushort)((uint)eve_rec.flag & 1U);
        if (AppMain.g_gs_main_sys_info.stage_id == (ushort)2 || AppMain.g_gs_main_sys_info.stage_id == (ushort)3)
        {
            gmsEnemy3DWork.obj_3d.use_light_flag &= 4294967294U;
            gmsEnemy3DWork.obj_3d.use_light_flag |= 2U;
        }
        work.obj_type = 2;
        work.wall_type = 6;
        AppMain.gmGmkBreakWallStart(obj_work);
        return obj_work;
    }

    private static void GmGmkBreakWallBuild()
    {
        AppMain.gm_gmk_breakwall_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(797), AppMain.GmGameDatGetGimmickData(798), 0U);
    }

    private static void GmGmkBreakWallFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(797));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_breakwall_obj_3d_list, amsAmbHeader.file_num);
    }
}