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
    private static AppMain.OBS_OBJECT_WORK GmGmkDSignInit(
         AppMain.GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_DSIGN");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_dsign_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -917504;
        work.flag |= 2U;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        int num = AppMain.MTM_MATH_CLIP((int)eve_rec.id - 287, 0, 3);
        work.dir.z = (ushort)(num * 16384);
        return work;
    }

    public static void GmGmkDSignBuild()
    {
        AppMain.gm_gmk_dsign_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(954), AppMain.GmGameDatGetGimmickData(955), 0U);
    }

    public static void GmGmkDSignFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(954);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_dsign_obj_3d_list, gimmickData.file_num);
    }

}