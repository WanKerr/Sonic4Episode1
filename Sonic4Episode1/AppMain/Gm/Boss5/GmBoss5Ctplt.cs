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
    public static AppMain.OBS_OBJECT_WORK GmBoss5CtpltInit(
         AppMain.GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_CTPLT_WORK()), "BOSS5_CTPLT");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS5_CTPLT_WORK ctplt_work = (AppMain.GMS_BOSS5_CTPLT_WORK)work;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss5GetObject3dList()[4], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMaterialMotionLoad(work, 0, AppMain.ObjDataGet(748), (string)null, 3, (object)null);
        work.flag &= 4294966271U;
        work.flag |= 18U;
        work.disp_flag |= 4194304U;
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5CtpltMain);
        AppMain.gmBoss5CtpltProcInit(ctplt_work);
        return work;
    }

    public static void GmBoss5CtpltCreate(AppMain.GMS_BOSS5_BODY_WORK body_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork1 = AppMain.GMM_BS_OBJ((object)body_work);
        AppMain.OBS_OBJECT_WORK obsObjectWork2 = AppMain.GmEventMgrLocalEventBirth((ushort)345, obsObjectWork1.pos.x, obsObjectWork1.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        obsObjectWork2.parent_obj = obsObjectWork1;
        obsObjectWork2.pos.x = obsObjectWork1.pos.x;
        obsObjectWork2.pos.y = body_work.ground_v_pos;
        obsObjectWork2.pos.z = AppMain.GMD_BOSS5_CTPLT_BG_FARSIDE_POS_Z;
    }

    public static void gmBoss5CtpltSetObjCollisionRect(AppMain.GMS_BOSS5_CTPLT_WORK ctplt_work)
    {
        AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)ctplt_work;
        gmsEnemyComWork.col_work.obj_col.obj = AppMain.GMM_BS_OBJ((object)ctplt_work);
        gmsEnemyComWork.col_work.obj_col.width = AppMain.GMD_BOSS5_CTPLT_OBJ_COL_RECT_WIDTH_INT;
        gmsEnemyComWork.col_work.obj_col.height = AppMain.GMD_BOSS5_CTPLT_OBJ_COL_RECT_HEIGHT_INT;
        gmsEnemyComWork.col_work.obj_col.ofst_x = AppMain.GMD_BOSS5_CTPLT_OBJ_COL_RECT_OFST_X_INT;
        gmsEnemyComWork.col_work.obj_col.ofst_y = AppMain.GMD_BOSS5_CTPLT_OBJ_COL_RECT_OFST_Y_INT;
    }

    public static void gmBoss5CtpltMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_CTPLT_WORK gmsBosS5CtpltWork = (AppMain.GMS_BOSS5_CTPLT_WORK)obj_work;
        if (((int)((AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj).mgr_work.flag & 33554432) != 0)
            AppMain.gmBoss5CtpltSetObjCollisionRect(gmsBosS5CtpltWork);
        if (gmsBosS5CtpltWork.proc_update == null)
            return;
        gmsBosS5CtpltWork.proc_update(gmsBosS5CtpltWork);
    }

    public static void gmBoss5CtpltProcInit(AppMain.GMS_BOSS5_CTPLT_WORK ctplt_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)ctplt_work);
        AppMain.ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag |= 4U;
        ctplt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_CTPLT_WORK(AppMain.gmBoss5CtpltProcIdle);
    }

    public static void gmBoss5CtpltProcIdle(AppMain.GMS_BOSS5_CTPLT_WORK ctplt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)ctplt_work);
        if (((int)((AppMain.GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj).mgr_work.flag & 8388608) == 0)
            return;
        obsObjectWork.spd_add.y = AppMain.GMD_BOSS5_CTPLT_MOVE_DOWN_ACC;
        ctplt_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_CTPLT_WORK(AppMain.gmBoss5CtpltProcMoveDown);
    }

    public static void gmBoss5CtpltProcMoveDown(AppMain.GMS_BOSS5_CTPLT_WORK ctplt_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)ctplt_work);
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj;
        AppMain.GMS_BOSS5_MGR_WORK mgrWork = parentObj.mgr_work;
        if (obsObjectWork.pos.y <= parentObj.ground_v_pos + AppMain.GMD_BOSS5_CTPLT_MOVE_DOWN_HIDE_HEIGHT)
            return;
        mgrWork.flag |= 16777216U;
        obsObjectWork.flag |= 4U;
    }


}