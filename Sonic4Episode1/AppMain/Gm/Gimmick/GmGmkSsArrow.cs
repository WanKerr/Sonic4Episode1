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
    private static AppMain.OBS_OBJECT_WORK GmGmkSsArrowInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SS_ARROW");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= (short)128;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_ss_arrow_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsArrowMain);
        AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, AppMain.readAMBFile(AppMain.ObjDataGet(986).pData));
        AppMain.ObjDrawObjectActionSet3DNNMaterial(work, 0);
        work.dir.z = (ushort)((uint)eve_rec.width << 8);
        int num = (int)(AppMain.g_gm_main_system.sync_time % 24U) - (AppMain.MTM_MATH_CLIP((int)eve_rec.left, 0, 3) << 3);
        if (num < 0)
            num += 24;
        work.user_timer = num;
        gmsEnemy3DWork.obj_3d.mat_frame = (float)num;
        return work;
    }

    public static void GmGmkSsArrowBuild()
    {
        AppMain.gm_gmk_ss_arrow_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(984)), AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(985)), 0U);
    }

    public static void GmGmkSsArrowFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(984));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_ss_arrow_obj_3d_list, amsAmbHeader.file_num);
    }

    private static void gmGmkSsArrowMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE() && ((int)AppMain.GmSplStageGetWork().flag & 4) != 0)
        {
            obj_work.flag |= 4U;
        }
        else
        {
            ++obj_work.user_timer;
            if (obj_work.user_timer < 24)
                return;
            obj_work.user_timer = 0;
            AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        }
    }

}