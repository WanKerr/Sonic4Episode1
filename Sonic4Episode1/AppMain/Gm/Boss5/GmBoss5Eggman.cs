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
    public static AppMain.OBS_OBJECT_WORK GmBoss5EggInit(
         AppMain.GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS5_EGG_WORK()), "BOSS5_EGG");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS5_EGG_WORK egg_work = (AppMain.GMS_BOSS5_EGG_WORK)work;
        work.pos.z = AppMain.GMD_BOSS5_BG_FARSIDE_POS_Z;
        AppMain.ObjObjectFieldRectSet(work, (short)-16, (short)-16, (short)16, (short)0);
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss5GetObject3dList()[2], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(746), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = AppMain.GMD_BOSS5_DEFAULT_BLEND_SPD;
        work.flag |= 16U;
        work.disp_flag &= 4290772991U;
        work.disp_flag &= 4294967294U;
        work.move_flag |= 1152U;
        work.move_flag &= 4294967039U;
        egg_work.ene_3d.ene_com.enemy_flag |= 32768U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss5EggMain);
        AppMain.gmBoss5EggProcInit(egg_work);
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    public static AppMain.GMS_BOSS5_EGG_WORK GmBoss5EggCreate(
      AppMain.GMS_BOSS5_BODY_WORK body_work,
      int pos_x,
      int pos_y)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GmEventMgrLocalEventBirth((ushort)334, pos_x, pos_y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        obsObjectWork.parent_obj = AppMain.GMM_BS_OBJ((object)body_work);
        return (AppMain.GMS_BOSS5_EGG_WORK)obsObjectWork;
    }

    public static void gmBoss5EggInitEscapeRun(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        obj_work.pos.x = AppMain.GmBsCmnGetPlayerObj().pos.x + ((int)AppMain.OBD_OBJ_CLIP_LCD_X << 12);
    }

    public static void gmBoss5EggUpdateEscapeRun(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)egg_work);
        if (AppMain.ObjViewOutCheck(obsObjectWork.pos.x, obsObjectWork.pos.y, (short)0, AppMain.GMD_BOSS5_EGG_ESCAPE_RUN_VIEWOUT_OFST_LEFT, (short)0, AppMain.GMD_BOSS5_EGG_ESCAPE_RUN_VIEWOUT_OFST_RIGHT, (short)0) != 0)
            return;
        int num = obsObjectWork.pos.x - AppMain.GmBsCmnGetPlayerObj().pos.x;
        int v1 = AppMain.MTM_MATH_CLIP(AppMain.FX_Div(AppMain.GMD_BOSS5_EGG_ESCAPE_RUN_DISTANCE_SLOWEST - num, AppMain.GMD_BOSS5_EGG_ESCAPE_RUN_DISTANCE_SLOWEST - AppMain.GMD_BOSS5_EGG_ESCAPE_RUN_DISTANCE_FASTEST), 0, 4096);
        obsObjectWork.spd.x = AppMain.FX_Mul(v1, AppMain.GmBsCmnGetPlayerObj().spd_m) + AppMain.FX_Mul(4096 - v1, AppMain.GMD_BOSS5_EGG_ESCAPE_RUN_SLOWEST_SPD);
    }

    public static void gmBoss5EggInitJump(AppMain.GMS_BOSS5_EGG_WORK egg_work, int dest_pos_x)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)egg_work);
        AppMain.VEC_Set(ref obsObjectWork.spd_add, 0, 0, 0);
        obsObjectWork.spd.x = AppMain.GMD_BOSS5_EGG_JUMP_INIT_SPD_X;
        obsObjectWork.spd.y = AppMain.GMD_BOSS5_EGG_JUMP_INIT_SPD_Y;
        obsObjectWork.spd_add.y = -2 * AppMain.FX_Mul(AppMain.FX_Div(obsObjectWork.spd.x, dest_pos_x - obsObjectWork.pos.x), obsObjectWork.spd.y);
        egg_work.jump_dest_pos_x = dest_pos_x;
        obsObjectWork.move_flag |= 272U;
        obsObjectWork.move_flag &= 4294967167U;
    }

    public static int gmBoss5EggUpdateJump(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        if (obj_work.pos.x >= egg_work.jump_dest_pos_x)
            obj_work.spd.x = 0;
        if (obj_work.pos.y <= parentObj.ground_v_pos + AppMain.GMD_BOSS5_EGG_HIDE_HIGHT)
            return 0;
        AppMain.GmBsCmnSetObjSpdZero(obj_work);
        return 1;
    }

    public static void gmBoss5EggGetBodyNodePos(
      AppMain.GMS_BOSS5_EGG_WORK egg_work,
      out AppMain.VecFx32 pos_out)
    {
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)egg_work).parent_obj;
        AppMain.NNS_MATRIX snmMtx = AppMain.GmBsCmnGetSNMMtx(parentObj.snm_work, parentObj.body_snm_reg_id);
        pos_out = new AppMain.VecFx32(AppMain.FX_F32_TO_FX32(snmMtx.M03), -AppMain.FX_F32_TO_FX32(snmMtx.M13), AppMain.FX_F32_TO_FX32(snmMtx.M23));
    }

    public static void gmBoss5EggMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS5_EGG_WORK wrk = (AppMain.GMS_BOSS5_EGG_WORK)obj_work;
        if (wrk.proc_update == null)
            return;
        wrk.proc_update(wrk);
    }

    public static void gmBoss5EggProcInit(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        AppMain.GmBsCmnSetAction(AppMain.GMM_BS_OBJ((object)egg_work), 0, 1, 0);
        AppMain.GmBoss5EfctStartEggSweat(egg_work);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_EGG_WORK(AppMain.gmBoss5EggProcUpdateStandby);
    }

    public static void gmBoss5EggProcUpdateStandby(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        if (((int)((AppMain.GMS_BOSS5_BODY_WORK)AppMain.GMM_BS_OBJ((object)egg_work).parent_obj).mgr_work.flag & 4194304) == 0)
            return;
        AppMain.gmBoss5EggInitEscapeRun(egg_work);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_EGG_WORK(AppMain.gmBoss5EggProcUpdateRun);
    }

    public static void gmBoss5EggProcUpdateRun(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        AppMain.VecFx32 pos_out;
        AppMain.gmBoss5EggGetBodyNodePos(egg_work, out pos_out);
        AppMain.gmBoss5EggUpdateEscapeRun(egg_work);
        if (obj_work.pos.x < pos_out.x + AppMain.GMD_BOSS5_EGG_JUMP_START_OFST_POS_X)
            return;
        obj_work.spd.x = AppMain.GMD_BOSS5_EGG_JUMP_RUNUP_SPD_X;
        AppMain.GmBsCmnSetAction(obj_work, 1, 0, 1);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_EGG_WORK(AppMain.gmBoss5EggProcUpdateStartJump);
    }

    public static void gmBoss5EggProcUpdateStartJump(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        ((AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj).mgr_work.flag |= 524288U;
        AppMain.GmBoss5EfctEndEggSweat(egg_work);
        AppMain.VecFx32 pos_out;
        AppMain.gmBoss5EggGetBodyNodePos(egg_work, out pos_out);
        AppMain.gmBoss5EggInitJump(egg_work, pos_out.x);
        AppMain.GmBsCmnSetAction(obj_work, 2, 0, 0);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_EGG_WORK(AppMain.gmBoss5EggProcUpdateJump);
    }

    public static void gmBoss5EggProcUpdateJump(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        AppMain.gmBoss5EggUpdateJump(egg_work);
        if (obj_work.spd.y <= 0)
            return;
        AppMain.GmBsCmnSetAction(obj_work, 3, 0, 0);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_EGG_WORK(AppMain.gmBoss5EggProcUpdateFall);
    }

    public static void gmBoss5EggProcUpdateFall(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        if (AppMain.gmBoss5EggUpdateJump(egg_work) == 0)
            return;
        AppMain.VecFx32 pos_out;
        AppMain.gmBoss5EggGetBodyNodePos(egg_work, out pos_out);
        obj_work.pos.Assign(pos_out);
        AppMain.GmBsCmnSetAction(obj_work, 4, 0, 0);
        obj_work.disp_flag |= 1U;
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS5_EGG_WORK(AppMain.gmBoss5EggProcUpdateAnger);
    }

    public static void gmBoss5EggProcUpdateAnger(AppMain.GMS_BOSS5_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        AppMain.GMS_BOSS5_BODY_WORK parentObj = (AppMain.GMS_BOSS5_BODY_WORK)obj_work.parent_obj;
        if (AppMain.GmBsCmnIsActionEnd(obj_work) == 0)
            return;
        parentObj.flag |= 16777216U;
        obj_work.flag |= 4U;
    }


}