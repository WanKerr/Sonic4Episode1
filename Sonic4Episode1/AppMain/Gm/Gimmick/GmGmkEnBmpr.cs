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
    private static void GmGmkEnBmprBuild()
    {
        AppMain.g_gm_gmk_en_bmpr_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.GmGameDatGetGimmickData(852), AppMain.GmGameDatGetGimmickData(853), 0U);
    }

    private static void GmGmkEnBmprFlush()
    {
        AppMain.AMS_AMB_HEADER gimmickData = AppMain.GmGameDatGetGimmickData(852);
        AppMain.GmGameDBuildRegFlushModel(AppMain.g_gm_gmk_en_bmpr_obj_3d_list, gimmickData.file_num);
        AppMain.g_gm_gmk_en_bmpr_obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkEnBmprInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        int life_max = 3;
        if (eve_rec.left > (sbyte)0 && eve_rec.left < (sbyte)3)
            life_max = (int)eve_rec.left;
        if (life_max <= (int)eve_rec.byte_param[1])
            return (AppMain.OBS_OBJECT_WORK)null;
        int num = AppMain.gmGmkEnBmprCalcType((int)eve_rec.id);
        AppMain.OBS_OBJECT_WORK objWork = AppMain.gmGmkEnBmprLoadObj(eve_rec, pos_x, pos_y, num).ene_com.obj_work;
        AppMain.gmGmkEnBmprInit(objWork, num, life_max);
        return objWork;
    }

    private static uint gmGmkEnBmpreGameSystemGetSyncTime()
    {
        return AppMain.g_gm_main_system.sync_time;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkEnBmprLoadObjNoModel(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        AppMain.GMS_ENEMY_3D_WORK work = (AppMain.GMS_ENEMY_3D_WORK)AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_EN_BMPR");
        work.ene_com.rect_work[0].flag &= 4294967291U;
        work.ene_com.rect_work[1].flag &= 4294967291U;
        return work;
    }

    private static AppMain.GMS_ENEMY_3D_WORK gmGmkEnBmprLoadObj(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      int type)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = AppMain.gmGmkEnBmprLoadObjNoModel(eve_rec, pos_x, pos_y, type);
        AppMain.OBS_OBJECT_WORK objWork = gmsEnemy3DWork.ene_com.obj_work;
        int index = 3;
        AppMain.ObjObjectCopyAction3dNNModel(objWork, AppMain.g_gm_gmk_en_bmpr_obj_3d_list[index], gmsEnemy3DWork.obj_3d);
        AppMain.OBS_DATA_WORK data_work1 = AppMain.ObjDataGet(854);
        AppMain.ObjObjectAction3dNNMotionLoad(objWork, 0, false, data_work1, (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.OBS_DATA_WORK data_work2 = AppMain.ObjDataGet(855);
        AppMain.ObjObjectAction3dNNMaterialMotionLoad(objWork, 0, data_work2, (string)null, 0, (object)null);
        return gmsEnemy3DWork;
    }

    private static void gmGmkEnBmprInit(
      AppMain.OBS_OBJECT_WORK obj_work,
      int en_bmpr_type,
      int life_max)
    {
        AppMain.GMS_ENEMY_3D_WORK gimmick_work = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        AppMain.gmGmkEnBmprSetRect(gimmick_work, en_bmpr_type);
        obj_work.move_flag = 8448U;
        obj_work.user_flag |= 1U;
        obj_work.dir.z = AppMain.g_gm_gmk_en_bmpr_angle_z[en_bmpr_type];
        int life = life_max - (int)gimmick_work.ene_com.eve_rec.byte_param[1];
        AppMain.gmGmkEnBmperSetUserWorkLife(obj_work, life);
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, AppMain.g_gm_gmk_en_bmpr_mat_motion_id[life]);
        obj_work.disp_flag |= 4194308U;
        obj_work.pos.z = -122880;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppMove = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.ppOut = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkEnBmprDrawFunc);
        AppMain.gmGmkEnBmprChangeModeWait(obj_work);
    }

    private static void gmGmkEnBmprSetRect(AppMain.GMS_ENEMY_3D_WORK gimmick_work, int en_bmpr_type)
    {
        AppMain.OBS_RECT_WORK pRec = gimmick_work.ene_com.rect_work[2];
        short cLeft = AppMain.g_gmk_en_bmpr_rect[en_bmpr_type][0];
        short cRight = AppMain.g_gmk_en_bmpr_rect[en_bmpr_type][2];
        short cTop = AppMain.g_gmk_en_bmpr_rect[en_bmpr_type][1];
        short cBottom = AppMain.g_gmk_en_bmpr_rect[en_bmpr_type][3];
        AppMain.ObjRectWorkZSet(pRec, cLeft, cTop, (short)-500, cRight, cBottom, (short)500);
        pRec.flag |= 1024U;
        AppMain.ObjRectDefSet(pRec, (ushort)65534, (short)0);
        pRec.ppDef = new AppMain.OBS_RECT_WORK_Delegate1(AppMain.gmGmkEnBmprDefFunc);
    }

    private static void gmGmkEnBmprDrawFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_ACTION3D_NN_WORK obj3d = obj_work.obj_3d;
        if (obj3d.motion != null)
        {
            float startFrame = AppMain.amMotionMaterialGetStartFrame(obj3d.motion, obj3d.mat_act_id);
            float num = AppMain.amMotionMaterialGetEndFrame(obj3d.motion, obj3d.mat_act_id) - startFrame;
            float syncTime = (float)AppMain.gmGmkEnBmpreGameSystemGetSyncTime();
            obj3d.mat_frame = syncTime % num;
        }
        AppMain.ObjDrawActionSummary(obj_work);
    }

    private static int gmGmkEnBmprCalcType(int id)
    {
        return id - 164;
    }

    private static AppMain.VecFx32 gmGmkEnBmprNormalizeVectorXY(AppMain.VecFx32 vec)
    {
        AppMain.VecFx32 vecFx32 = new AppMain.VecFx32();
        int denom = AppMain.FX_Sqrt(AppMain.FX_Mul(vec.x, vec.x) + AppMain.FX_Mul(vec.y, vec.y));
        if (denom == 0)
        {
            vecFx32.x = 4096;
            vecFx32.y = 0;
        }
        else
        {
            int v2 = AppMain.FX_Div(4096, denom);
            vecFx32.x = AppMain.FX_Mul(vec.x, v2);
            vecFx32.y = AppMain.FX_Mul(vec.y, v2);
        }
        vecFx32.z = 0;
        return vecFx32;
    }

    private static void gmGmkEnBmprDefFunc(
      AppMain.OBS_RECT_WORK gimmick_rect,
      AppMain.OBS_RECT_WORK player_rect)
    {
        AppMain.OBS_OBJECT_WORK parentObj1 = gimmick_rect.parent_obj;
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)parentObj1;
        AppMain.OBS_OBJECT_WORK parentObj2 = player_rect.parent_obj;
        if (parentObj2.obj_type != (ushort)1)
            return;
        int index = AppMain.gmGmkEnBmprCalcType((int)gmsEnemy3DWork.ene_com.eve_rec.id);
        parentObj1.dir.z = AppMain.g_gm_gmk_en_bmpr_angle_z[index];
        parentObj2.dir.z = (ushort)0;
        int num1 = parentObj2.spd.x;
        int num2 = parentObj2.spd.y;
        if (((int)parentObj2.move_flag & 32768) != 0)
        {
            if (parentObj2.spd_m != 0)
            {
                num1 = AppMain.FX_Mul(parentObj2.spd_m, AppMain.mtMathCos((int)parentObj2.dir.z));
                num2 = AppMain.FX_Mul(parentObj2.spd_m, AppMain.mtMathSin((int)parentObj2.dir.z));
            }
            else
            {
                AppMain.VecFx32 vec = new AppMain.VecFx32();
                vec.x = parentObj2.pos.x - parentObj1.pos.x;
                vec.y = parentObj2.pos.y - parentObj1.pos.y;
                vec.z = 0;
                vec = AppMain.gmGmkEnBmprNormalizeVectorXY(vec);
                num1 = AppMain.FX_Mul(vec.x, 98304);
                num2 = AppMain.FX_Mul(vec.y, 98304);
            }
        }
        int num3 = -12288;
        int num4 = parentObj2.pos.x - parentObj1.pos.x;
        int num5 = parentObj2.pos.y + num3 - parentObj1.pos.y;
        switch (index)
        {
            case 0:
                num1 = 0;
                if (num5 < 0)
                {
                    num2 = -24576;
                    break;
                }
                num2 = 24576;
                parentObj1.dir.z += (ushort)32768;
                break;
            case 1:
                int num6 = AppMain.FX_Mul(24576, 2896);
                if (num5 < 0)
                {
                    num1 = -num6;
                    num2 = -num6;
                    break;
                }
                num1 = num6;
                num2 = num6;
                parentObj1.dir.z += (ushort)32768;
                break;
            case 2:
                num2 = 0;
                if (num4 < 0)
                {
                    num1 = -24576;
                    break;
                }
                num1 = 24576;
                parentObj1.dir.z += (ushort)32768;
                break;
            case 3:
                int num7 = AppMain.FX_Mul(24576, 2896);
                if (num5 > 0)
                {
                    num1 = -num7;
                    num2 = num7;
                    break;
                }
                num1 = num7;
                num2 = -num7;
                parentObj1.dir.z += (ushort)32768;
                break;
        }
        AppMain.GmPlySeqInitPinballAir((AppMain.GMS_PLAYER_WORK)parentObj2, num1, num2, 5);
        AppMain.gmGmkEnBmprChangeModeHit(parentObj1);
        if (AppMain.gmGmkEnBmperGetUserWorkLife(parentObj1) <= 0)
            parentObj1.user_flag = (uint)((ulong)parentObj1.user_flag & 18446744073709551614UL);
        int score = 10;
        if (AppMain.gmGmkEnBmprCheckGroupBonus(parentObj1) != 0)
            score *= 50;
        AppMain.GmPlayerAddScore((AppMain.GMS_PLAYER_WORK)parentObj2, score, parentObj1.pos.x, parentObj1.pos.y);
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate(parentObj1, 16);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = parentObj2.pos.x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = parentObj2.pos.y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
        gmsEffect3DesWork.efct_com.obj_work.dir.z = (ushort)(AppMain.nnArcTan2((double)AppMain.FX_FX32_TO_F32(num2), (double)AppMain.FX_FX32_TO_F32(num1)) - 16384);
        AppMain.GMM_PAD_VIB_SMALL();
    }

    private static int gmGmkEnBmprCheckGroupBonus(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)obj_work;
        if (AppMain.gmGmkEnBmperGetUserWorkLife(obj_work) > 0)
            return 0;
        sbyte top = gmsEnemy3DWork.ene_com.eve_rec.top;
        if (top == (sbyte)0)
            return 0;
        for (AppMain.OBS_OBJECT_WORK obj_work1 = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)3); obj_work1 != null; obj_work1 = AppMain.ObjObjectSearchRegistObject(obj_work1, (ushort)3))
        {
            AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work1;
            if (obj_work1 != obj_work && (gmsEnemyComWork.eve_rec.id == (ushort)164 || gmsEnemyComWork.eve_rec.id == (ushort)165 || (gmsEnemyComWork.eve_rec.id == (ushort)166 || gmsEnemyComWork.eve_rec.id == (ushort)167)) && ((int)gmsEnemyComWork.eve_rec.top == (int)top && (Convert.ToInt32(obj_work1.user_flag) & 1) != 0))
                return 0;
        }
        return 1;
    }

    private static void gmGmkEnBmprChangeModeWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, 0, 0);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkEnBmprMainWait);
    }

    private static void gmGmkEnBmprChangeModeHit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GmSoundPlaySE("Casino7");
        byte num = 1;
        ((AppMain.GMS_ENEMY_3D_WORK)obj_work).ene_com.eve_rec.byte_param[1] += num;
        int index = AppMain.gmGmkEnBmperAddUserWorkLife(obj_work, (int)-num);
        if (index < 0)
            return;
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, AppMain.g_gm_gmk_en_bmpr_mat_motion_id[index]);
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, 1, 0);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkEnBmprMainHit);
    }

    private static void gmGmkEnBmprChangeModeLost(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.ObjDrawObjectActionSet3DNN(obj_work, 0, 0);
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkEnBmprMainLost);
        AppMain.GMS_EFFECT_3DES_WORK gmsEffect3DesWork = AppMain.GmEfctCmnEsCreate((AppMain.OBS_OBJECT_WORK)null, 19);
        gmsEffect3DesWork.efct_com.obj_work.pos.x = obj_work.pos.x;
        gmsEffect3DesWork.efct_com.obj_work.pos.y = obj_work.pos.y;
        gmsEffect3DesWork.efct_com.obj_work.pos.z = 131072;
    }

    private static void gmGmkEnBmprMainWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)obj_work);
    }

    private static void gmGmkEnBmprMainHit(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (((int)obj_work.disp_flag & 8) == 0)
            return;
        if (AppMain.gmGmkEnBmperGetUserWorkLife(obj_work) > 0)
            AppMain.gmGmkEnBmprChangeModeWait(obj_work);
        else
            AppMain.gmGmkEnBmprChangeModeLost(obj_work);
    }

    private static void gmGmkEnBmprMainLost(AppMain.OBS_OBJECT_WORK obj_work)
    {
        obj_work.flag |= 4U;
    }

    private static void gmGmkEnBmperSetUserWorkLife(AppMain.OBS_OBJECT_WORK obj_work, int life)
    {
        obj_work.user_work = (uint)life;
    }

    private static int gmGmkEnBmperGetUserWorkLife(AppMain.OBS_OBJECT_WORK obj_work)
    {
        return (int)obj_work.user_work;
    }

    private static int gmGmkEnBmperAddUserWorkLife(AppMain.OBS_OBJECT_WORK obj_work, int add)
    {
        obj_work.user_work += (uint)add;
        return (int)obj_work.user_work;
    }


}