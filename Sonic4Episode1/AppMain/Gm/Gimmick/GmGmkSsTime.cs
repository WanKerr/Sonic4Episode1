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
    private static AppMain.OBS_OBJECT_WORK GmGmkSsTimeInit(
     AppMain.GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SS_TIME");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        work.view_out_ofst -= (short)128;
        uint num = AppMain.MTM_MATH_CLIP((uint)eve_rec.flag & 3U, 0U, 2U);
        work.user_work = num;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_ss_time_obj_3d_list[(int)num], gmsEnemy3DWork.obj_3d);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        work.disp_flag |= 4194304U;
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 2U;
        work.scale.x = work.scale.y = work.scale.z = 6144;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsTimeMain);
        gmsEnemy3DWork.ene_com.rect_work[0].flag &= 4294967291U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkSsTimeDefFunc);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        AppMain.ObjRectWorkSet(pRec, (short)-6, (short)-6, (short)6, (short)6);
        return work;
    }

    public static void GmGmkSsTimeBuild()
    {
        AppMain.gm_gmk_ss_time_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(915), AppMain.GmGameDatGetGimmickData(916), 0U);
    }

    public static void GmGmkSsTimeFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(915);
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_ss_time_obj_3d_list, gimmickData.file_num);
    }

    private static void gmGmkSsTimeMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)AppMain.GmSplStageGetWork().flag & 4) != 0)
            obj_work.flag |= 4U;
        else
            obj_work.dir.z = AppMain.GmMainGetObjectRotation();
    }

    private static void gmGmkSsTimeDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || (parentObj2.obj_work.obj_type != (ushort)1 || parentObj2.gmk_obj == (AppMain.OBS_OBJECT_WORK)parentObj1))
            return;
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork1 = AppMain.GmEfctZoneEsCreate(parentObj1.obj_work, 5, 17);
        gmsEffect3DesWork1.efct_com.obj_work.flag |= 512U;
        gmsEffect3DesWork1.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsTimeEfctMain);
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork2 = AppMain.GmEfctZoneEsCreate(parentObj1.obj_work, 5, AppMain.gm_gmk_ss_time_add_msg[AppMain.GsEnvGetLanguage()]);
        gmsEffect3DesWork2.efct_com.obj_work.flag |= 512U;
        gmsEffect3DesWork2.efct_com.obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSsTimeEfctMain);
        gmsEffect3DesWork2.obj_3des.command_state = 10U;
        parentObj1.enemy_flag |= 65536U;
        AppMain.GmSoundPlaySE("Special6");
        AppMain.GmFixRequestTimerFlash();
        AppMain.g_gm_main_system.game_time += AppMain.gm_gmk_ss_time_add_subtract[(int)parentObj1.obj_work.user_work];
        parentObj1.obj_work.flag |= 4U;
    }

    private static void gmGmkSsTimeEfctMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
        if (obsCamera == null)
            return;
        obj_work.dir.z = (ushort)-obsCamera.roll;
    }

}