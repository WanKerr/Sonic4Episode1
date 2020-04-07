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
    private static AppMain.OBS_OBJECT_WORK GmGmkSplRingInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_SPL_RING");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        gmsEnemy3DWork.ene_com.enemy_flag |= 65536U;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_gmk_splring_obj_3d_list[0], gmsEnemy3DWork.obj_3d);
        AppMain.ObjAction3dNNMaterialMotionLoad(gmsEnemy3DWork.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, 0, (AppMain.AMS_AMB_HEADER)AppMain.ObjDataGet(882).pData);
        AppMain.ObjDrawAction3dActionSet3DNNMaterial(gmsEnemy3DWork.obj_3d, 0);
        work.pos.z = -131072;
        work.move_flag |= 8448U;
        work.disp_flag |= 4194336U;
        work.flag |= 18U;
        gmsEnemy3DWork.ene_com.col_work.obj_col.flag |= 134217728U;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[2];
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkSplRingDefFunc);
        AppMain.ObjRectWorkSet(pRec, (short)-4, (short)-4, (short)4, (short)4);
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSplRingWait);
        return work;
    }

    public static void GmGmkSplRingBuild()
    {
        AppMain.gm_gmk_splring_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(880), AppMain.GmGameDatGetGimmickData(881), 0U);
    }

    public static void GmGmkSplRingFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(880));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_splring_obj_3d_list, amsAmbHeader.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkSplRingMake(int pos_x, int pos_y)
    {
        return AppMain.GmEventMgrLocalEventBirth((ushort)304, pos_x, pos_y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
    }

    private static void gmGmkSplRingWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        if (gmsPlayerWork == null || AppMain.g_gs_main_sys_info.game_mode != 0 || (AppMain.GsTrialIsTrial() || gmsPlayerWork.obj_work.pos.x < obj_work.pos.x - AppMain.FXM_FLOAT_TO_FX32(AppMain.AMD_SCREEN_2D_WIDTH)) || (AppMain.GsMainSysIsSpecialStageClearedAct((int)AppMain.g_gs_main_sys_info.stage_id) || AppMain.GsMainSysIsStageClear(27)))
            return;
        if (AppMain.g_gm_main_system.ply_work[0].ring_num < (short)50)
        {
            obj_work.disp_flag |= 32U;
            obj_work.flag |= 2U;
        }
        else
        {
            obj_work.disp_flag &= 4294967263U;
            obj_work.flag &= 4294967293U;
        }
        uint p_disp_flag = 4;
        AppMain.ObjDrawAction3DNNMaterialUpdate(obj_work.obj_3d, ref p_disp_flag);
    }

    private static void gmGmkSplRingVanishReady(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.obj_3d.mat_frame = 1f;
        uint p_disp_flag = 16;
        AppMain.ObjDrawAction3DNNMaterialUpdate(obj_work.obj_3d, ref p_disp_flag);
        if (((int)obj_work.dir.y & (int)short.MaxValue) != 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSplRingVanish);
        AppMain.GmEffect3DESSetDispOffset(AppMain.GmEfctCmnEsCreate(obj_work, 78), 0.0f, 0.0f, 50f);
        AppMain.GmEfctCmnEsCreate(obj_work, 79);
    }

    private static void gmGmkSplRingVanish(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_PLAYER_WORK gmsPlayerWork = AppMain.g_gm_main_system.ply_work[0];
        obj_work.dir.y -= (ushort)4096;
        if (((int)obj_work.dir.y & (int)short.MaxValue) != 0)
            return;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.disp_flag |= 32U;
        gmsPlayerWork.obj_work.disp_flag |= 32U;
        gmsPlayerWork.obj_work.move_flag |= 8192U;
    }

    private static void gmGmkSplRingDefFunc(
      AppMain.OBS_RECT_WORK mine_rect,
      AppMain.OBS_RECT_WORK match_rect)
    {
        AppMain.GMS_ENEMY_COM_WORK parentObj1 = (AppMain.GMS_ENEMY_COM_WORK)mine_rect.parent_obj;
        AppMain.GMS_PLAYER_WORK parentObj2 = (AppMain.GMS_PLAYER_WORK)match_rect.parent_obj;
        if (parentObj1 == null || parentObj2 == null || parentObj2.obj_work.obj_type != (ushort)1)
            return;
        if (((int)parentObj2.player_flag & 262144) != 0)
            AppMain.GmPlayerSetEndTruckRide(parentObj2);
        AppMain.GmPlySeqInitSplIn(parentObj2, parentObj1.obj_work.pos);
        parentObj2.gmk_flag2 |= 6U;
        ((AppMain.OBS_OBJECT_WORK)parentObj1).ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkSplRingVanishReady);
        parentObj1.obj_work.dir.y &= (ushort)57344;
        parentObj1.obj_work.flag |= 2U;
        AppMain.GMM_PAD_VIB_SMALL();
        AppMain.GmSoundPlaySE("Special1");
    }

}