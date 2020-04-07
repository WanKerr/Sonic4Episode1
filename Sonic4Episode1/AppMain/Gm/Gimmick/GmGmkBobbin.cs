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
    private static AppMain.OBS_OBJECT_WORK GmGmkBobbinInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkBobbinLoadObj(eve_rec, pos_x, pos_y).ene_com.obj_work;
        AppMain.gmGmkBobbinInit(objWork);
        return objWork;
    }

    public static void GmGmkBobbinBuild()
    {
        AppMain.g_gm_gmk_bobbin_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(863)), AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(864)), 0U);
    }

    public static void GmGmkBobbinFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(863));
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_bobbin_obj_3d_list, amsAmbHeader.file_num);
        AppMain.g_gm_gmk_bobbin_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkBobbinLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_BOBBIN");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkBobbinLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkBobbinLoadObjNoModel(eve_rec, pos_x, pos_y);
        AppMain.OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index = 0;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.g_gm_gmk_bobbin_obj_3d_list[index], gmsEnemy3DWork.obj_3d);
        AppMain.OBS_DATA_WORK data_work1 = AppMain.ObjDataGet(865);
        AppMain.ObjObjectAction3dNNMotionLoad(objWork, 0, false, data_work1, (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.OBS_DATA_WORK data_work2 = AppMain.ObjDataGet(866);
        AppMain.ObjObjectAction3dNNMaterialMotionLoad(objWork, 0, data_work2, (string)null, 0, (object)null);
        return gmsEnemy3DWork;
    }

    private static void gmGmkBobbinInit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.gmGmkBobbinSetRect((AppMain.GMS_ENEMY_3D_WORK)obj_work);
        obj_work.move_flag = 8448U;
        obj_work.disp_flag |= 4194308U;
        obj_work.pos.z = -131072;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
            obj_work.pos.z = -65536;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBobbinDrawFunc);
        AppMain.gmGmkBobbinChangeModeWait(obj_work);
    }

    private static void gmGmkBobbinSetRect(AppMain.GMS_ENEMY_3D_WORK gimmick_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)gimmick_work;
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        short cLeft = -24;
        short cRight = 24;
        short cTop = -24;
        short cBottom = 24;
        AppMain.ObjRectWorkZSet(pRec, cLeft, cTop, (short)-500, cRight, cBottom, (short)500);
        pRec.flag |= 1024U;
        AppMain.ObjRectGroupSet(pRec, (byte)1, (byte)1);
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkBobbinDefFunc);
        if (!AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
            return;
        AppMain.OBS_COLLISION_WORK colWork = ((AppMain.GMS_ENEMY_3D_WORK)obsObjectWork).ene_com.col_work;
        colWork.obj_col.obj = obsObjectWork;
        colWork.obj_col.diff_data = AppMain.g_gm_default_col;
        colWork.obj_col.width = (ushort)16;
        colWork.obj_col.height = (ushort)16;
        colWork.obj_col.ofst_x = (short)-((int)colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)-((int)colWork.obj_col.height / 2);
        colWork.obj_col.attr = (ushort)2;
        colWork.obj_col.flag |= 134217760U;
    }

    private static void gmGmkBobbinDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static AppMain.VecFx32 gmGmkBobbinNormalizeVectorXY(AppMain.VecFx32 vec)
    {
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        int x = AppMain.FX_Mul(vec.x, vec.x) + AppMain.FX_Mul(vec.y, vec.y);
        if (x == 0)
        {
            vecFx32.x = 4096;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = AppMain.FX_Div(4096, AppMain.FX_Sqrt(x));
            vecFx32.x = AppMain.FX_Mul(vec.x, v2);
            vecFx32.y = AppMain.FX_Mul(vec.y, v2);
        }
        vecFx32.z = 0;
        if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE())
        {
            int dest_x = 0;
            int dest_y = 0;
            AppMain.ObjUtilGetRotPosXY(vecFx32.x, vecFx32.y, ref dest_x, ref dest_y, (ushort)-AppMain.g_gm_main_system.pseudofall_dir);
            vecFx32.x = dest_x;
            vecFx32.y = dest_y;
        }
        return vecFx32;
    }

    private static void gmGmkBobbinDefPlayer(
      AppMain.GMS_ENEMY_3D_WORK gimmick_work,
      AppMain.GMS_PLAYER_WORK player_work,
      int speed_x,
      int speed_y)
    {
        bool flag_no_recover_homing = false;
        if (((int)gimmick_work.ene_com.eve_rec.flag & 1) != 0)
            flag_no_recover_homing = true;
        AppMain.GmPlySeqInitPinballAir(player_work, speed_x, speed_y, 5, flag_no_recover_homing);
        if (AppMain.GMM_MAIN_STAGE_IS_SS())
            return;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)gimmick_work;
        AppMain.GmPlayerAddScore(player_work, 10, obsObjectWork.pos.x, obsObjectWork.pos.y);
    }

    private static void gmGmkBobbinDefEnemy(
      AppMain.OBS_OBJECT_WORK obj_work,
      int speed_x,
      int speed_y)
    {
        if (((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.id != (ushort)316)
            return;
        obj_work.spd.x = speed_x;
        obj_work.spd.y = speed_y;
        obj_work.spd_add.x = 0;
        obj_work.spd_add.y = 0;
        if (AppMain.MTM_MATH_ABS(obj_work.spd.x) < 256)
        {
            obj_work.spd.x = 256;
        }
        else
        {
            if (AppMain.MTM_MATH_ABS(obj_work.spd.y) >= 256)
                return;
            obj_work.spd.y = 256;
        }
    }

    private static void gmGmkBobbinDefFunc(
      AppMain.OBS_RECT_WORK gimmick_rect,
      AppMain.OBS_RECT_WORK player_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        AppMain.GMS_ENEMY_3D_WORK gimmick_work = (AppMain.GMS_ENEMY_3D_WORK)parentObj1;
        AppMain.OBS_OBJECT_WORK parentObj2 = player_rect.parent_obj;
        AppMain.VecFx32 vec = new AppMain.VecFx32();
        vec.x = parentObj2.prev_pos.x - parentObj1.pos.x;
        vec.y = (int)((long)parentObj2.prev_pos.y + -12288L - (long)parentObj1.pos.y);
        vec.z = 0;
        if (AppMain.FX_Mul(114688, 114688) < AppMain.FX_Mul(vec.x, vec.x) + AppMain.FX_Mul(vec.y, vec.y))
        {
            gimmick_rect.flag &= 4294966271U;
        }
        else
        {
            gimmick_rect.flag |= 1024U;
            AppMain.VecFx32 vecFx32 = AppMain.gmGmkBobbinNormalizeVectorXY(vec);
            parentObj2.dir.z = (ushort)0;
            int v1_1 = AppMain.FX_Mul(vecFx32.x, 24576);
            int v1_2 = AppMain.FX_Mul(vecFx32.y, 24576);
            int v2_1 = AppMain.FX_F32_TO_FX32((float)((100.0 + (double)gimmick_work.ene_com.eve_rec.left) * 0.00999999977648258));
            if (v2_1 < 0)
                v2_1 = 0;
            int v2_2 = AppMain.FX_F32_TO_FX32((float)((100.0 + (double)gimmick_work.ene_com.eve_rec.top) * 0.00999999977648258));
            if (v2_2 < 0)
                v2_2 = 0;
            int num1 = AppMain.FX_Mul(v1_1, v2_1);
            int num2 = AppMain.FX_Mul(v1_2, v2_2);
            if (parentObj2.obj_type == (ushort)1)
                AppMain.gmGmkBobbinDefPlayer(gimmick_work, (AppMain.GMS_PLAYER_WORK)parentObj2, num1, num2);
            else if (parentObj2.obj_type == (ushort)2)
                AppMain.gmGmkBobbinDefEnemy(parentObj2, num1, num2);
            AppMain.gmGmkBobbinChangeModeHit(parentObj1);
            AppMain.GmSoundPlaySE("Casino1");
            AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate(parentObj1, 16);
            gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj2.pos.x;
            gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj2.pos.y;
            gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
            gmsEffect3DesWork.efct_com.obj_work.dir.z = (ushort)(AppMain.nnArcTan2((double)AppMain.FX_FX32_TO_F32(num2), (double)AppMain.FX_FX32_TO_F32(num1)) - 16384);
            if (AppMain.GSM_MAIN_STAGE_IS_SPSTAGE_NOT_RETRY())
            {
                AppMain.OBS_CAMERA obsCamera = AppMain.ObjCameraGet(AppMain.g_obj.glb_camera_id);
                if (obsCamera != null)
                    gmsEffect3DesWork.efct_com.obj_work.dir.z -= (ushort)obsCamera.roll;
            }
            AppMain.GMM_PAD_VIB_SMALL();
        }
    }

    private static void gmGmkBobbinChangeModeWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, 0, 0);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBobbinMainWait);
    }

    private static void gmGmkBobbinChangeModeHit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, 1, 0);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBobbinMainHit);
    }

    private static void gmGmkBobbinMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
    }

    private static void gmGmkBobbinMainHit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        AppMain.gmGmkBobbinChangeModeWait(obj_work);
    }

}