using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

public partial class AppMain
{
    private static void GmGmkPressPillarBuild()
    {
        AppMain.gm_gmk_press_pillar_obj_3d_list = AppMain.GmGameDBuildRegBuildModel(AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(951)), AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(952)), 0U);
        AppMain.GmGmkPressPillarClear();
    }

    private static void GmGmkPressPillarFlush()
    {
        AppMain.AMS_AMB_HEADER amsAmbHeader = AppMain.readAMBFile((object)AppMain.GmGameDatGetGimmickData(951));
        AppMain.GmGameDBuildRegFlushModel(AppMain.gm_gmk_press_pillar_obj_3d_list, amsAmbHeader.file_num);
    }

    private static AppMain.OBS_OBJECT_WORK GmGmkPressPillarInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work1 = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_ENEMY_3D_WORK()), "GMK_P_PIL_TOP");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work1;
        uint num = 0;
        if (eve_rec.id == (ushort)285)
            num = 1U;
        AppMain.ObjObjectCopyAction3dNNModel(work1, AppMain.gm_gmk_press_pillar_obj_3d_list[(int)(2U + num)], gmsEnemy3DWork.obj_3d);
        work1.pos.z = -126976;
        work1.disp_flag |= 4194304U;
        work1.move_flag |= 512U;
        work1.move_flag |= 1040U;
        work1.flag |= 1U;
        work1.user_flag = 0U;
        AppMain.OBS_COLLISION_WORK colWork = gmsEnemy3DWork.ene_com.col_work;
        colWork.obj_col.obj = work1;
        colWork.obj_col.width = (ushort)AppMain.GMD_GMK_PPIL_COL_WIDTH;
        colWork.obj_col.height = (ushort)AppMain.GMD_GMK_PPIL_COL_HEIGHT;
        colWork.obj_col.ofst_x = (short)((int)-colWork.obj_col.width / 2);
        colWork.obj_col.ofst_y = (short)0;
        if (eve_rec.id == (ushort)285)
            colWork.obj_col.ofst_y = (short)-colWork.obj_col.height;
        if (eve_rec.id == (ushort)284)
            AppMain.ObjObjectFieldRectSet(work1, (short)((int)-AppMain.GMD_GMK_PPIL_COL_WIDTH / 2 + 2), (short)-1, (short)((int)AppMain.GMD_GMK_PPIL_COL_WIDTH / 2 - 2), AppMain.GMD_GMK_PPIL_COL_HEIGHT);
        else
            AppMain.ObjObjectFieldRectSet(work1, (short)((int)-AppMain.GMD_GMK_PPIL_COL_WIDTH / 2 + 2), (short)-AppMain.GMD_GMK_PPIL_COL_HEIGHT, (short)((int)AppMain.GMD_GMK_PPIL_COL_WIDTH / 2 - 2), (short)-1);
        work1.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPPillarTopWait);
        AppMain.OBS_OBJECT_WORK work2 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), work1, (ushort)0, "GMK_P_PIL_BODY");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork1 = (AppMain.GMS_EFFECT_3DNN_WORK)work2;
        AppMain.ObjObjectCopyAction3dNNModel(work2, AppMain.gm_gmk_press_pillar_obj_3d_list[(int)num], gmsEffect3DnnWork1.obj_3d);
        AppMain.ObjAction3dNNMaterialMotionLoad(gmsEffect3DnnWork1.obj_3d, 0, (AppMain.OBS_DATA_WORK)null, (string)null, (int)num, AppMain.readAMBFile(AppMain.ObjDataGet(953).pData));
        AppMain.ObjDrawObjectActionSet3DNNMaterial(work2, 0);
        work2.pos.z = -131072;
        work2.move_flag |= 256U;
        work2.disp_flag |= 4194308U;
        work2.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPPillarBodyFollow);
        AppMain.OBS_OBJECT_WORK work3 = AppMain.GMM_EFFECT_CREATE_WORK((AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_EFFECT_3DNN_WORK()), work1, (ushort)0, "GMK_P_PIL_SPRING");
        AppMain.GMS_EFFECT_3DNN_WORK gmsEffect3DnnWork2 = (AppMain.GMS_EFFECT_3DNN_WORK)work3;
        AppMain.ObjObjectCopyAction3dNNModel(work3, AppMain.gm_gmk_press_pillar_obj_3d_list[4], gmsEffect3DnnWork2.obj_3d);
        work3.pos.z = -131072;
        work3.move_flag |= 256U;
        work3.disp_flag |= 4194304U;
        work3.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPPillarSpringFollow);
        return work1;
    }

    private static void GmGmkPressPillarStartup(uint id_num)
    {
        id_num &= AppMain.GMD_GMK_PPIL_ID_NUM_MASK;
        AppMain.gm_gmk_press_pillar_sw[(int)id_num] = (byte)1;
    }

    private static void GmGmkPressPillarClear()
    {
        Array.Clear((Array)AppMain.gm_gmk_press_pillar_sw, 0, AppMain.gm_gmk_press_pillar_sw.Length);
    }

    private static void gmGmkPPillarTopWait(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_EVE_RECORD_EVENT eveRec = ((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec;
        byte num1 = (byte)((uint)eveRec.flag & AppMain.GMD_GMK_PPIL_ID_NUM_MASK);
        if (AppMain.gm_gmk_press_pillar_sw[(int)num1] == (byte)0)
            return;
        int num2 = AppMain.MTM_MATH_ABS((int)eveRec.top << 10);
        if (num2 == 0)
            num2 = 4096;
        obj_work.spd.y = eveRec.id != (ushort)284 ? num2 : -num2;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPPillarTopMove);
    }

    private static void gmGmkPPillarTopMove(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_EVE_RECORD_EVENT eveRec = ((AppMain.GMS_ENEMY_COM_WORK)obj_work).eve_rec;
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obj_work;
        int num1 = (int)eveRec.height << 12;
        bool flag = false;
        if (num1 != 0)
        {
            if (eveRec.id == (ushort)284)
            {
                int num2 = gmsEnemyComWork.born_pos_y - num1;
                if (obj_work.pos.y <= num2)
                {
                    obj_work.pos.y = num2;
                    flag = true;
                }
            }
            else
            {
                int num2 = gmsEnemyComWork.born_pos_y + num1;
                if (obj_work.pos.y >= num2)
                {
                    obj_work.pos.y = num2;
                    flag = true;
                }
            }
        }
        if (((int)obj_work.move_flag & 15) != 0)
            flag = true;
        if (!flag)
            return;
        obj_work.spd.y = 0;
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.user_flag |= AppMain.GMD_GMK_PPIL_COLHIT;
        if (((int)eveRec.flag & AppMain.GMD_GMK_PPIL_FLAG_EFFECT) != 0)
            return;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.GmEfctZoneEsCreate(obj_work, 3, 1);
        if (eveRec.id != (ushort)284)
            return;
        obsObjectWork.dir.z = (ushort)32768;
    }

    private static void gmGmkPPillarBodyFollow(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        int y = parentObj.pos.y;
        int num = ((AppMain.GMS_ENEMY_COM_WORK)parentObj).eve_rec.id != (ushort)284 ? y - AppMain.GMD_GMK_PPIL_PIL_OFS_MAX : y + AppMain.GMD_GMK_PPIL_PIL_OFS_MAX;
        obj_work.pos.y = num;
        if (((int)parentObj.user_flag & (int)AppMain.GMD_GMK_PPIL_COLHIT) != 0)
        {
            if (((int)((AppMain.GMS_ENEMY_COM_WORK)parentObj).eve_rec.flag & AppMain.GMD_GMK_PPIL_FLAG_SHOCK_ABS) != 0)
            {
                obj_work.spd.y = 0;
                obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
            }
            else
            {
                obj_work.spd.y = obj_work.user_timer;
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPPillarBodyMove);
                obj_work.user_flag = 0U;
            }
        }
        else
        {
            if (parentObj.spd.y == 0)
                return;
            obj_work.user_timer = parentObj.spd.y;
        }
    }

    private static void gmGmkPPillarBodyMove(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        AppMain.GMS_EVE_RECORD_EVENT eveRec = ((AppMain.GMS_ENEMY_COM_WORK)parentObj).eve_rec;
        int a = obj_work.spd.y * 3 / 4;
        if (16 < AppMain.MTM_MATH_ABS(a))
        {
            obj_work.spd.y = a;
        }
        else
        {
            obj_work.spd.y = eveRec.id != (ushort)284 ? -obj_work.user_timer / 32 : -obj_work.user_timer / 32;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPPillarBodyMoveEx);
        }
        if (eveRec.id == (ushort)284)
        {
            if (obj_work.pos.y <= parentObj.pos.y + AppMain.GMD_GMK_PPIL_PIL_OFS_MIN)
            {
                obj_work.pos.y = parentObj.pos.y + AppMain.GMD_GMK_PPIL_PIL_OFS_MIN;
                obj_work.spd.y = 32;
                obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPPillarBodyMoveEx);
            }
        }
        else if (obj_work.pos.y >= parentObj.pos.y - AppMain.GMD_GMK_PPIL_PIL_OFS_MIN)
        {
            obj_work.pos.y = parentObj.pos.y - AppMain.GMD_GMK_PPIL_PIL_OFS_MIN;
            obj_work.spd.y = -32;
            obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPPillarBodyMoveEx);
        }
        parentObj.user_work = (uint)obj_work.pos.y;
    }

    private static void gmGmkPPillarBodyMoveEx(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        AppMain.GMS_EVE_RECORD_EVENT eveRec = ((AppMain.GMS_ENEMY_COM_WORK)parentObj).eve_rec;
        if (obj_work.user_flag == 0U)
        {
            obj_work.spd.y = obj_work.spd.y * 8 / 7;
            if (2048 <= AppMain.MTM_MATH_ABS(obj_work.spd.y))
                obj_work.user_flag = 1U;
        }
        else
        {
            int num = obj_work.spd.y * 7 / 8;
            if (128 < AppMain.MTM_MATH_ABS(obj_work.spd.y))
                obj_work.spd.y = num;
        }
        if (eveRec.id == (ushort)284)
        {
            if (obj_work.pos.y >= parentObj.pos.y + AppMain.GMD_GMK_PPIL_PIL_OFS_MAX)
            {
                obj_work.pos.y = parentObj.pos.y + AppMain.GMD_GMK_PPIL_PIL_OFS_MAX;
                obj_work.spd.y = 0;
                obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
            }
        }
        else if (obj_work.pos.y <= parentObj.pos.y - AppMain.GMD_GMK_PPIL_PIL_OFS_MAX)
        {
            obj_work.pos.y = parentObj.pos.y - AppMain.GMD_GMK_PPIL_PIL_OFS_MAX;
            obj_work.spd.y = 0;
            obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        }
        parentObj.user_work = (uint)obj_work.pos.y;
    }

    private static void gmGmkPPillarSpringFollow(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        int y = parentObj.pos.y;
        int num = ((AppMain.GMS_ENEMY_COM_WORK)parentObj).eve_rec.id != (ushort)284 ? y - AppMain.GMD_GMK_PPIL_SPR_OFS_MAX : y + AppMain.GMD_GMK_PPIL_SPR_OFS_MAX;
        obj_work.pos.y = num;
        if (((int)parentObj.user_flag & (int)AppMain.GMD_GMK_PPIL_COLHIT) == 0)
            return;
        obj_work.ppFunc = ((int)((AppMain.GMS_ENEMY_COM_WORK)parentObj).eve_rec.flag & AppMain.GMD_GMK_PPIL_FLAG_SHOCK_ABS) == 0 ? new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkPPillarSpringMove) : (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
        obj_work.spd.y = 0;
    }

    private static void gmGmkPPillarSpringMove(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK parentObj = obj_work.parent_obj;
        if (((AppMain.GMS_ENEMY_COM_WORK)parentObj).eve_rec.id == (ushort)284)
        {
            int num = (parentObj.pos.y + AppMain.GMD_GMK_PPIL_PIL_OFS_MIN - (int)parentObj.user_work) / 2;
            obj_work.pos.y = parentObj.pos.y + AppMain.GMD_GMK_PPIL_SPR_OFS_MIN - num;
            float a = (float)AppMain.MTM_MATH_ABS(obj_work.pos.y - (parentObj.pos.y + AppMain.GMD_GMK_PPIL_SPR_OFS_MIN)) / (float)(AppMain.GMD_GMK_PPIL_SPR_OFS_MAX - AppMain.GMD_GMK_PPIL_SPR_OFS_MIN);
            obj_work.scale.y = AppMain.FXM_FLOAT_TO_FX32(a);
        }
        else
        {
            int num = (parentObj.pos.y - AppMain.GMD_GMK_PPIL_PIL_OFS_MIN - (int)parentObj.user_work) / 2;
            obj_work.pos.y = parentObj.pos.y - AppMain.GMD_GMK_PPIL_SPR_OFS_MIN - num;
            float a = (float)AppMain.MTM_MATH_ABS(obj_work.pos.y - (parentObj.pos.y - AppMain.GMD_GMK_PPIL_SPR_OFS_MIN)) / (float)(AppMain.GMD_GMK_PPIL_SPR_OFS_MAX - AppMain.GMD_GMK_PPIL_SPR_OFS_MIN);
            obj_work.scale.y = AppMain.FXM_FLOAT_TO_FX32(a);
        }
    }

}