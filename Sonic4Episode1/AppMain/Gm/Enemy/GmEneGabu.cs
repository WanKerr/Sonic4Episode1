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
    private static void GmEneGabuBuild()
    {
        AppMain.gm_ene_gabu_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(664)), AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(665)), 0U);
    }

    private static void GmEneGabuFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile(AppMain.GmGameDatGetEnemyData(664));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_ene_gabu_obj_3d_list, amsAmbHeader.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmEneGabuInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "ENE_GABU");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_ene_gabu_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(666), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.pos.z = 0;
        AppMain.OBS_RECT_WORK pRec1 = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkSet(pRec1, (short)-11, (short)-16, (short)11, (short)16);
        pRec1.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec2 = gmsEnemy3DWork.ene_com.rect_work[0];
        AppMain.ObjRectWorkSet(pRec2, (short)-19, (short)-24, (short)19, (short)24);
        pRec2.flag |= 4U;
        AppMain.OBS_RECT_WORK pRec3 = gmsEnemy3DWork.ene_com.rect_work[2];
        AppMain.ObjRectWorkSet(pRec3, (short)-19, (short)-24, (short)19, (short)24);
        pRec3.flag &= 4294967291U;
        work.move_flag |= 384U;
        work.view_out_ofst_plus[1] = (short)-256;
        int num1 = (int)-eve_rec.top << 13;
        if (num1 <= 0)
            num1 = 393216;
        int num2 = (int)eve_rec.flag & 7;
        work.user_timer = num2 > 3 ? -24576 - -1024 * (num2 - 3) : -1024 * num2 - 24576;
        int denom = AppMain.FX_Div(num1 * 2, -work.user_timer);
        work.spd_fall = AppMain.FX_Div(-work.user_timer, denom);
        work.user_work = (uint)work.pos.y;
        AppMain.ObjDrawObjectActionSet(work, 1);
        work.disp_flag |= 4U;
        AppMain.gmEneGabuJumpInit(work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmEneGabuJumpInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (obj_work.obj_3d.act_id[0] != 1)
        {
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 1);
            obj_work.disp_flag |= 4U;
        }
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGabuJumpMain);
        obj_work.spd.y = obj_work.user_timer;
    }

    private static void gmEneGabuJumpMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (obj_work.obj_3d.act_id[0] != 2 && obj_work.spd.y < 0 && -obj_work.spd.y / obj_work.spd_fall <= 20)
            AppMain.ObjDrawObjectActionSet(obj_work, 2);
        if (obj_work.obj_3d.act_id[0] == 2 && ((int)obj_work.disp_flag & 8) != 0)
        {
            AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 0);
            obj_work.disp_flag |= 4U;
        }
        if (obj_work.pos.y < (int)obj_work.user_work)
            return;
        if (((int)((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec.flag & 128) != 0)
            AppMain.gmEneGabuJumpWaitInit(obj_work);
        else
            AppMain.gmEneGabuJumpInit(obj_work);
    }

    private static void gmEneGabuJumpWaitInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.ObjDrawObjectActionSet3DNNBlend(obj_work, 1);
        obj_work.disp_flag |= 4U;
        obj_work.user_flag = 61440U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmEneGabuJumpWaitMain);
        obj_work.spd.y = 0;
        obj_work.move_flag &= 4294967167U;
    }

    private static void gmEneGabuJumpWaitMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_flag = (uint)AppMain.ObjTimeCountDown((int)obj_work.user_flag);
        if (obj_work.user_flag != 0U)
            return;
        obj_work.move_flag |= 128U;
        AppMain.gmEneGabuJumpInit(obj_work);
    }


}