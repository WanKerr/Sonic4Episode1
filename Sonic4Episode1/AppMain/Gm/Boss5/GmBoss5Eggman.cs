public partial class AppMain
{
    public static OBS_OBJECT_WORK GmBoss5EggInit(
         GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS5_EGG_WORK(), "BOSS5_EGG");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS5_EGG_WORK egg_work = (GMS_BOSS5_EGG_WORK)work;
        work.pos.z = GMD_BOSS5_BG_FARSIDE_POS_Z;
        ObjObjectFieldRectSet(work, -16, -16, 16, 0);
        ObjObjectCopyAction3dNNModel(work, GmBoss5GetObject3dList()[2], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(746), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = GMD_BOSS5_DEFAULT_BLEND_SPD;
        work.flag |= 16U;
        work.disp_flag &= 4290772991U;
        work.disp_flag &= 4294967294U;
        work.move_flag |= 1152U;
        work.move_flag &= 4294967039U;
        egg_work.ene_3d.ene_com.enemy_flag |= 32768U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss5EggMain);
        gmBoss5EggProcInit(egg_work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static GMS_BOSS5_EGG_WORK GmBoss5EggCreate(
      GMS_BOSS5_BODY_WORK body_work,
      int pos_x,
      int pos_y)
    {
        OBS_OBJECT_WORK obsObjectWork = GmEventMgrLocalEventBirth(334, pos_x, pos_y, 0, 0, 0, 0, 0, 0);
        obsObjectWork.parent_obj = GMM_BS_OBJ(body_work);
        return (GMS_BOSS5_EGG_WORK)obsObjectWork;
    }

    public static void gmBoss5EggInitEscapeRun(GMS_BOSS5_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        GmBsCmnSetObjSpdZero(obj_work);
        obj_work.pos.x = GmBsCmnGetPlayerObj().pos.x + (OBD_OBJ_CLIP_LCD_X << 12);
    }

    public static void gmBoss5EggUpdateEscapeRun(GMS_BOSS5_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(egg_work);
        if (ObjViewOutCheck(obsObjectWork.pos.x, obsObjectWork.pos.y, 0, GMD_BOSS5_EGG_ESCAPE_RUN_VIEWOUT_OFST_LEFT, 0, GMD_BOSS5_EGG_ESCAPE_RUN_VIEWOUT_OFST_RIGHT, 0) != 0)
            return;
        int num = obsObjectWork.pos.x - GmBsCmnGetPlayerObj().pos.x;
        int v1 = MTM_MATH_CLIP(FX_Div(GMD_BOSS5_EGG_ESCAPE_RUN_DISTANCE_SLOWEST - num, GMD_BOSS5_EGG_ESCAPE_RUN_DISTANCE_SLOWEST - GMD_BOSS5_EGG_ESCAPE_RUN_DISTANCE_FASTEST), 0, 4096);
        obsObjectWork.spd.x = FX_Mul(v1, GmBsCmnGetPlayerObj().spd_m) + FX_Mul(4096 - v1, GMD_BOSS5_EGG_ESCAPE_RUN_SLOWEST_SPD);
    }

    public static void gmBoss5EggInitJump(GMS_BOSS5_EGG_WORK egg_work, int dest_pos_x)
    {
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(egg_work);
        VEC_Set(ref obsObjectWork.spd_add, 0, 0, 0);
        obsObjectWork.spd.x = GMD_BOSS5_EGG_JUMP_INIT_SPD_X;
        obsObjectWork.spd.y = GMD_BOSS5_EGG_JUMP_INIT_SPD_Y;
        obsObjectWork.spd_add.y = -2 * FX_Mul(FX_Div(obsObjectWork.spd.x, dest_pos_x - obsObjectWork.pos.x), obsObjectWork.spd.y);
        egg_work.jump_dest_pos_x = dest_pos_x;
        obsObjectWork.move_flag |= 272U;
        obsObjectWork.move_flag &= 4294967167U;
    }

    public static int gmBoss5EggUpdateJump(GMS_BOSS5_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        if (obj_work.pos.x >= egg_work.jump_dest_pos_x)
            obj_work.spd.x = 0;
        if (obj_work.pos.y <= parentObj.ground_v_pos + GMD_BOSS5_EGG_HIDE_HIGHT)
            return 0;
        GmBsCmnSetObjSpdZero(obj_work);
        return 1;
    }

    public static void gmBoss5EggGetBodyNodePos(
      GMS_BOSS5_EGG_WORK egg_work,
      out VecFx32 pos_out)
    {
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(egg_work).parent_obj;
        NNS_MATRIX snmMtx = GmBsCmnGetSNMMtx(parentObj.snm_work, parentObj.body_snm_reg_id);
        pos_out = new VecFx32(FX_F32_TO_FX32(snmMtx.M03), -FX_F32_TO_FX32(snmMtx.M13), FX_F32_TO_FX32(snmMtx.M23));
    }

    public static void gmBoss5EggMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS5_EGG_WORK wrk = (GMS_BOSS5_EGG_WORK)obj_work;
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    public static void gmBoss5EggProcInit(GMS_BOSS5_EGG_WORK egg_work)
    {
        GmBsCmnSetAction(GMM_BS_OBJ(egg_work), 0, 1, 0);
        GmBoss5EfctStartEggSweat(egg_work);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS5_EGG_WORK(gmBoss5EggProcUpdateStandby);
    }

    public static void gmBoss5EggProcUpdateStandby(GMS_BOSS5_EGG_WORK egg_work)
    {
        if (((int)((GMS_BOSS5_BODY_WORK)GMM_BS_OBJ(egg_work).parent_obj).mgr_work.flag & 4194304) == 0)
            return;
        gmBoss5EggInitEscapeRun(egg_work);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS5_EGG_WORK(gmBoss5EggProcUpdateRun);
    }

    public static void gmBoss5EggProcUpdateRun(GMS_BOSS5_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        VecFx32 pos_out;
        gmBoss5EggGetBodyNodePos(egg_work, out pos_out);
        gmBoss5EggUpdateEscapeRun(egg_work);
        if (obj_work.pos.x < pos_out.x + GMD_BOSS5_EGG_JUMP_START_OFST_POS_X)
            return;
        obj_work.spd.x = GMD_BOSS5_EGG_JUMP_RUNUP_SPD_X;
        GmBsCmnSetAction(obj_work, 1, 0, 1);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS5_EGG_WORK(gmBoss5EggProcUpdateStartJump);
    }

    public static void gmBoss5EggProcUpdateStartJump(GMS_BOSS5_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        ((GMS_BOSS5_BODY_WORK)obj_work.parent_obj).mgr_work.flag |= 524288U;
        GmBoss5EfctEndEggSweat(egg_work);
        VecFx32 pos_out;
        gmBoss5EggGetBodyNodePos(egg_work, out pos_out);
        gmBoss5EggInitJump(egg_work, pos_out.x);
        GmBsCmnSetAction(obj_work, 2, 0, 0);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS5_EGG_WORK(gmBoss5EggProcUpdateJump);
    }

    public static void gmBoss5EggProcUpdateJump(GMS_BOSS5_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        gmBoss5EggUpdateJump(egg_work);
        if (obj_work.spd.y <= 0)
            return;
        GmBsCmnSetAction(obj_work, 3, 0, 0);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS5_EGG_WORK(gmBoss5EggProcUpdateFall);
    }

    public static void gmBoss5EggProcUpdateFall(GMS_BOSS5_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        if (gmBoss5EggUpdateJump(egg_work) == 0)
            return;
        VecFx32 pos_out;
        gmBoss5EggGetBodyNodePos(egg_work, out pos_out);
        obj_work.pos.Assign(pos_out);
        GmBsCmnSetAction(obj_work, 4, 0, 0);
        obj_work.disp_flag |= 1U;
        egg_work.proc_update = new MPP_VOID_GMS_BOSS5_EGG_WORK(gmBoss5EggProcUpdateAnger);
    }

    public static void gmBoss5EggProcUpdateAnger(GMS_BOSS5_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        GMS_BOSS5_BODY_WORK parentObj = (GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        if (GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        parentObj.flag |= 16777216U;
        obj_work.flag |= 4U;
    }


}