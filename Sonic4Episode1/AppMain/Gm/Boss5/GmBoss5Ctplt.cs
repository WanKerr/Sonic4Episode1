public partial class AppMain
{
    public static OBS_OBJECT_WORK GmBoss5CtpltInit(
         GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS5_CTPLT_WORK(), "BOSS5_CTPLT");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS5_CTPLT_WORK ctplt_work = (GMS_BOSS5_CTPLT_WORK)work;
        ObjObjectCopyAction3dNNModel(work, GmBoss5GetObject3dList()[4], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMaterialMotionLoad(work, 0, ObjDataGet(748), null, 3, null);
        work.flag &= 4294966271U;
        work.flag |= 18U;
        work.disp_flag |= 4194304U;
        work.move_flag |= 256U;
        work.move_flag &= 4294967167U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5CtpltMain);
        gmBoss5CtpltProcInit(ctplt_work);
        return work;
    }

    public static void GmBoss5CtpltCreate(GMS_BOSS5_BODY_WORK body_work)
    {
        OBS_OBJECT_WORK obsObjectWork1 = GMM_BS_OBJ(body_work);
        OBS_OBJECT_WORK obsObjectWork2 = GmEventMgrLocalEventBirth(345, obsObjectWork1.pos.x, obsObjectWork1.pos.y, 0, 0, 0, 0, 0, 0);
        obsObjectWork2.parent_obj = obsObjectWork1;
        obsObjectWork2.pos.x = obsObjectWork1.pos.x;
        obsObjectWork2.pos.y = body_work.ground_v_pos;
        obsObjectWork2.pos.z = GMD_BOSS5_CTPLT_BG_FARSIDE_POS_Z;
    }

    public static void gmBoss5CtpltSetObjCollisionRect(GMS_BOSS5_CTPLT_WORK ctplt_work)
    {
        GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)ctplt_work;
        gmsEnemyComWork.col_work.obj_col.obj = GMM_BS_OBJ(ctplt_work);
        gmsEnemyComWork.col_work.obj_col.width = GMD_BOSS5_CTPLT_OBJ_COL_RECT_WIDTH_INT;
        gmsEnemyComWork.col_work.obj_col.height = GMD_BOSS5_CTPLT_OBJ_COL_RECT_HEIGHT_INT;
        gmsEnemyComWork.col_work.obj_col.ofst_x = GMD_BOSS5_CTPLT_OBJ_COL_RECT_OFST_X_INT;
        gmsEnemyComWork.col_work.obj_col.ofst_y = GMD_BOSS5_CTPLT_OBJ_COL_RECT_OFST_Y_INT;
    }

    public static void gmBoss5CtpltMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_CTPLT_WORK gmsBosS5CtpltWork = (GMS_BOSS5_CTPLT_WORK)obj_work;
        if (((int)((GMS_BOSS5_BODY_WORK)obj_work.parent_obj).mgr_work.flag & 33554432) != 0)
            gmBoss5CtpltSetObjCollisionRect(gmsBosS5CtpltWork);
        if (gmsBosS5CtpltWork.proc_update == null)
            return;
        gmsBosS5CtpltWork.proc_update(gmsBosS5CtpltWork);
    }

    public static void gmBoss5CtpltProcInit(GMS_BOSS5_CTPLT_WORK ctplt_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(ctplt_work);
        ObjDrawObjectActionSet3DNNMaterial(obj_work, 0);
        obj_work.disp_flag |= 4U;
        ctplt_work.proc_update = new MPP_VOID_GMS_BOSS5_CTPLT_WORK(gmBoss5CtpltProcIdle);
    }

    public static void gmBoss5CtpltProcIdle(GMS_BOSS5_CTPLT_WORK ctplt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(ctplt_work);
        if (((int)((GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj).mgr_work.flag & 8388608) == 0)
            return;
        obsObjectWork.spd_add.y = GMD_BOSS5_CTPLT_MOVE_DOWN_ACC;
        ctplt_work.proc_update = new MPP_VOID_GMS_BOSS5_CTPLT_WORK(gmBoss5CtpltProcMoveDown);
    }

    public static void gmBoss5CtpltProcMoveDown(GMS_BOSS5_CTPLT_WORK ctplt_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(ctplt_work);
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obsObjectWork.parent_obj;
        GMS_BOSS5_MGR_WORK mgrWork = parentObj.mgr_work;
        if (obsObjectWork.pos.y <= parentObj.ground_v_pos + GMD_BOSS5_CTPLT_MOVE_DOWN_HIDE_HEIGHT)
            return;
        mgrWork.flag |= 16777216U;
        obsObjectWork.flag |= 4U;
    }


}