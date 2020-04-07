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
    private static AppMain.OBS_OBJECT_WORK GmGmkNeedleNeonInitStand(
     AppMain.GMS_EVE_RECORD_EVENT eve_rec,
     int pos_x,
     int pos_y,
     byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkNeedleNeonLoadObj(eve_rec, pos_x, pos_y, type, 2U).ene_com.obj_work;
        AppMain.gmGmkNeedleNeonStandInit(objWork);
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)337, objWork.pos.x, objWork.pos.y, eve_rec.flag, eve_rec.left, eve_rec.top, eve_rec.width, eve_rec.height, type);
        obsObjectWork.parent_obj = objWork;
        obsObjectWork.user_work = (uint)(objWork.pos.y + 131072);
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkNeedleNeonInitNeedle(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkNeedleNeonLoadObj(eve_rec, pos_x, pos_y, type, 0U).ene_com.obj_work;
        AppMain.gmGmkNeedleNeonNeedleInit(objWork);
        return objWork;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkNeedleNeonInitGlaer(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)eve_rec);
        AppMain.UNREFERENCED_PARAMETER((object)pos_x);
        AppMain.UNREFERENCED_PARAMETER((object)pos_y);
        AppMain.UNREFERENCED_PARAMETER((object)type);
        return (AppMain.OBS_OBJECT_WORK)null;
    }

    public static void GmGmkNeedleNeonBuild()
    {
        AppMain.g_gm_gmk_needle_neon_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(822), AppMain.GmGameDatGetGimmickData(823), 0U);
        AppMain.g_gm_gmk_needle_neon_obj_tvx_list = AppMain.GmGameDatGetGimmickData(824);
    }

    public static void GmGmkNeedleNeonFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(822);
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_needle_neon_obj_3d_list, gimmickData.file_num);
        AppMain.g_gm_gmk_needle_neon_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
        AppMain.g_gm_gmk_needle_neon_obj_tvx_list = (AppMain.AMS_AMB_HEADER)null;
    }

    private static void GmGmkNeedleNeonChangeModeActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_flag &= 4294967294U;
        if (obj_work.ppFunc == new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonNeedleMainOn) || obj_work.ppFunc == new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonNeedleMainActive))
            return;
        AppMain.gmGmkNeedleNeonNeedleChangeModeOn(obj_work);
    }

    private static void GmGmkNeedleNeonChangeModeWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_flag &= 4294967294U;
        if (obj_work.ppFunc == new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonNeedleMainWait) || obj_work.ppFunc == new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonNeedleMainOff))
            return;
        AppMain.gmGmkNeedleNeonNeedleChangeModeOff(obj_work);
    }

    private static void GmGmkNeedleNeonChangeModeTimer(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.user_flag |= 1U;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkNeedleNeonLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_NEEDLE_NEON");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkNeedleNeonLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type,
      uint model_index)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkNeedleNeonLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        AppMain.ObjObjectCopyAction3dNNModel(gmsEnemy3DWork.ene_com.obj_work, AppMain.g_gm_gmk_needle_neon_obj_3d_list[(int)model_index], gmsEnemy3DWork.obj_3d);
        return gmsEnemy3DWork;
    }

    private static void gmGmkNeedleNeonDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || obj_work.ppFunc == new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonNeedleMainWait) || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.gmGmkNeedleNeonTvxDrawFunc(new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.g_gm_gmk_needle_neon_obj_tvx_list, 0)), obj_work.obj_3d.texlist, obj_work.pos);
    }

    private static void gmGmkNeedleNeonStandDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.GmMainIsDrawEnable() || ((int)obj_work.disp_flag & 32) != 0)
            return;
        AppMain.TVX_FILE tvx;
        if (AppMain.g_gm_gmk_needle_neon_obj_tvx_list.buf[1] == null)
        {
            tvx = new AppMain.TVX_FILE((AppMain.AmbChunk)AppMain.amBindGet(AppMain.g_gm_gmk_needle_neon_obj_tvx_list, 1));
            AppMain.g_gm_gmk_needle_neon_obj_tvx_list.buf[1] = (object)tvx;
        }
        else
            tvx = (AppMain.TVX_FILE)AppMain.g_gm_gmk_needle_neon_obj_tvx_list.buf[1];
        AppMain.gmGmkNeedleNeonTvxDrawFunc(tvx, obj_work.obj_3d.texlist, obj_work.pos);
    }

    private static void gmGmkNeedleNeonTvxDrawFunc(
      AppMain.TVX_FILE tvx,
      AppMain.NNS_TEXLIST texlist,
      AppMain.VecFx32 base_pos)
    {
        AppMain.VecFx32 scale = new AppMain.VecFx32(4096, 4096, 4096);
        for (int index = 0; 5 > index; ++index)
        {
            AppMain.VecFx32 pos;
            pos.x = base_pos.x + AppMain.g_gm_gmk_disp_offset[index].x;
            pos.y = base_pos.y + AppMain.g_gm_gmk_disp_offset[index].y;
            pos.z = base_pos.z + AppMain.g_gm_gmk_disp_offset[index].z;
            AppMain.GmTvxSetModel(tvx, texlist, ref pos, ref scale, 0U, (short)0);
        }
    }

    private static void gmGmkNeedleNeonStandInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.move_flag |= 8449U;
        obj_work.disp_flag |= 4194304U;
        obj_work.pos.z = -655360;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonStandDrawFunc);
    }

    private static void gmGmkNeedleNeonNeedleInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.obj = gmsEnemy3DWork.ene_com.obj_work;
        gmsEnemy3DWork.ene_com.col_work.obj_col.width = (ushort)24;
        gmsEnemy3DWork.ene_com.col_work.obj_col.height = (ushort)30;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_x = (short)-12;
        gmsEnemy3DWork.ene_com.col_work.obj_col.ofst_y = (short)-32;
        AppMain.OBS_RECT_WORK pRec = gmsEnemy3DWork.ene_com.rect_work[1];
        AppMain.ObjRectWorkZSet(pRec, (short)-16, (short)-33, (short)-500, (short)16, (short)-8, (short)500);
        pRec.flag |= 1024U;
        obj_work.flag |= 2U;
        obj_work.move_flag |= 8449U;
        obj_work.disp_flag |= 4194304U;
        obj_work.flag |= 16U;
        obj_work.pos.z = -655360;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonDrawFunc);
        AppMain.gmGmkNeedleNeonNeedleChangeModeWait(obj_work);
    }

    private static void gmGmkNeedleNeonNeedleChangeModeWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonNeedleMainWait);
    }

    private static void gmGmkNeedleNeonNeedleChangeModeOn(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonNeedleMainOn);
        AppMain.GmSoundPlaySE("Boss2_06");
    }

    private static void gmGmkNeedleNeonNeedleChangeModeActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonNeedleMainActive);
    }

    private static void gmGmkNeedleNeonNeedleChangeModeOff(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag |= 2U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkNeedleNeonNeedleMainOff);
    }

    private static void gmGmkNeedleNeonNeedleMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int userWork = (int)obj_work.user_work;
        obj_work.pos.y = userWork;
        if (((int)obj_work.user_flag & 1) == 0)
            return;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 480)
            return;
        obj_work.user_timer = 0;
        AppMain.gmGmkNeedleNeonNeedleChangeModeOn(obj_work);
    }

    private static void gmGmkNeedleNeonNeedleMainOn(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkNeedleNeonNeedleUpdateHitRect(obj_work);
        int userWork = (int)obj_work.user_work;
        int fx32 = AppMain.FX_F32_TO_FX32((float)(32 * obj_work.user_timer / 10));
        obj_work.pos.y = userWork - fx32;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 10)
            return;
        obj_work.user_timer = 0;
        AppMain.gmGmkNeedleNeonNeedleChangeModeActive(obj_work);
    }

    private static void gmGmkNeedleNeonNeedleMainActive(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkNeedleNeonNeedleUpdateHitRect(obj_work);
        int userWork = (int)obj_work.user_work;
        obj_work.pos.y = userWork - 131072;
        if (((int)obj_work.user_flag & 1) == 0)
            return;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 180)
            return;
        obj_work.user_timer = 0;
        AppMain.gmGmkNeedleNeonNeedleChangeModeOff(obj_work);
    }

    private static void gmGmkNeedleNeonNeedleMainOff(AppMain.OBS_OBJECT_WORK obj_work)
    {
        int userWork = (int)obj_work.user_work;
        int fx32 = AppMain.FX_F32_TO_FX32((float)(32 - 32 * obj_work.user_timer / 10));
        obj_work.pos.y = userWork - fx32;
        ++obj_work.user_timer;
        if (obj_work.user_timer < 10)
            return;
        obj_work.user_timer = 0;
        AppMain.gmGmkNeedleNeonNeedleChangeModeWait(obj_work);
    }

    private static void gmGmkNeedleNeonNeedleUpdateHitRect(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.col_work.obj_col.rider_obj != null)
            obj_work.flag &= 4294967293U;
        else
            obj_work.flag |= 2U;
    }

}