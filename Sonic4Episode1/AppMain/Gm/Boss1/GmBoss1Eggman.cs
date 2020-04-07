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
    private static AppMain.OBS_OBJECT_WORK GmBoss1EggInit(
        AppMain.GMS_EVE_RECORD_EVENT eve_rec,
        int pos_x,
        int pos_y,
        byte type)
    {
        AppMain.UNREFERENCED_PARAMETER((object)type);
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS1_EGG_WORK()), "BOSS1_EGG");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS1_EGG_WORK gmsBosS1EggWork = (AppMain.GMS_BOSS1_EGG_WORK)work;
        work.move_flag |= 256U;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.gm_boss1_obj_3d_list[2], gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(705), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = 0.125f;
        work.disp_flag |= 134217728U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1EggWaitSetup);
        work.flag |= 16U;
        work.disp_flag |= 4U;
        work.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss1EggExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss1EggExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.gmBoss1MgrDecObjCreateCount(((AppMain.GMS_BOSS1_EGG_WORK)AppMain.mtTaskGetTcbWork(tcb)).mgr_work);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss1EggSetActionIndependent(
      AppMain.GMS_BOSS1_EGG_WORK egg_work,
      int act_id)
    {
        AppMain.gmBoss1EggSetActionIndependent(egg_work, act_id, false);
    }

    private static void gmBoss1EggSetActionIndependent(
      AppMain.GMS_BOSS1_EGG_WORK egg_work,
      int act_id,
      bool force_change)
    {
        AppMain.GMS_BOSS1_PART_ACT_INFO bosS1PartActInfo = AppMain.gm_boss1_egg_act_id_tbl[act_id];
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        if (((int)((AppMain.GMS_BOSS1_BODY_WORK)obj_work.parent_obj).flag & 2) != 0 || !force_change && ((int)egg_work.flag & 1) != 0 && egg_work.egg_act_id == act_id)
            return;
        egg_work.egg_act_id = act_id;
        egg_work.flag |= 1U;
        if (bosS1PartActInfo.is_maintain == (byte)0)
            AppMain.GmBsCmnSetAction(obj_work, (int)bosS1PartActInfo.act_id, (int)bosS1PartActInfo.is_repeat, bosS1PartActInfo.is_blend ? 1 : 0);
        else if (bosS1PartActInfo.is_repeat != (byte)0)
            AppMain.GMM_BS_OBJ((object)egg_work).disp_flag |= 4U;
        obj_work.obj_3d.speed[0] = bosS1PartActInfo.mtn_spd;
        obj_work.obj_3d.blend_spd = bosS1PartActInfo.blend_spd;
    }

    private static void gmBoss1EggRevertActionIndependent(AppMain.GMS_BOSS1_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        AppMain.GMS_BOSS1_BODY_WORK parentObj = (AppMain.GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT(egg_work.flag & 1U);
        egg_work.flag &= 4294967294U;
        AppMain.GmBsCmnSetAction(obj_work, (int)parentObj.egg_revert_mtn_id, (int)AppMain.gm_boss1_act_id_tbl[parentObj.whole_act_id][2].is_repeat, 1);
        obj_work.obj_3d.frame[0] = AppMain.GMM_BS_OBJ((object)parentObj).obj_3d.frame[0];
    }

    private static void gmBoss1EggWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_BODY_WORK parentObj = (AppMain.GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS1_EGG_WORK egg_work = (AppMain.GMS_BOSS1_EGG_WORK)obj_work;
        if (((int)AppMain.GMM_BOSS1_MGR(parentObj).flag & 1) == 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1EggMain);
        AppMain.gmBoss1EggProcIdleInit(egg_work);
    }

    private static void gmBoss1EggMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_BODY_WORK parentObj = (AppMain.GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS1_EGG_WORK gmsBosS1EggWork = (AppMain.GMS_BOSS1_EGG_WORK)obj_work;
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.egg_snm_reg_id, 1);
        if (gmsBosS1EggWork.proc_update != null)
            gmsBosS1EggWork.proc_update(gmsBosS1EggWork);
        if (((int)parentObj.flag & 8388608) != 0)
        {
            parentObj.flag &= 4286578687U;
            AppMain.gmBoss1EggProcEscapeInit(gmsBosS1EggWork);
        }
        if (((int)parentObj.flag & 536870912) != 0)
        {
            parentObj.flag &= 3758096383U;
            AppMain.gmBoss1EggProcDamageInit(gmsBosS1EggWork);
        }
        if (((int)parentObj.flag & 16777216) != 0)
        {
            parentObj.flag &= 4278190079U;
            AppMain.gmBoss1SetPartTextureBurnt(obj_work);
        }
        if (((int)AppMain.GMM_BS_OBJ((object)parentObj).disp_flag & 16) != 0)
            obj_work.disp_flag |= 16U;
        else
            obj_work.disp_flag &= 4294967279U;
    }

    private static void gmBoss1EggProcIdleInit(AppMain.GMS_BOSS1_EGG_WORK egg_work)
    {
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_EGG_WORK(AppMain.gmBoss1EggProcIdleUpdateLoop);
    }

    private static void gmBoss1EggProcIdleUpdateLoop(AppMain.GMS_BOSS1_EGG_WORK egg_work)
    {
        AppMain.GMS_BOSS1_BODY_WORK parentObj = (AppMain.GMS_BOSS1_BODY_WORK)AppMain.GMM_BS_OBJ((object)egg_work).parent_obj;
        if (((int)parentObj.flag & 268435456) == 0)
            return;
        parentObj.flag &= 4026531839U;
        AppMain.gmBoss1EggProcLaughInit(egg_work);
    }

    private static void gmBoss1EggProcLaughInit(AppMain.GMS_BOSS1_EGG_WORK egg_work)
    {
        AppMain.gmBoss1EggSetActionIndependent(egg_work, 0);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_EGG_WORK(AppMain.gmBoss1EggProcLaughUpdateLoop);
    }

    private static void gmBoss1EggProcLaughUpdateLoop(AppMain.GMS_BOSS1_EGG_WORK egg_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)egg_work)) == 0)
            return;
        AppMain.gmBoss1EggRevertActionIndependent(egg_work);
        AppMain.gmBoss1EggProcIdleInit(egg_work);
    }

    private static void gmBoss1EggProcDamageInit(AppMain.GMS_BOSS1_EGG_WORK egg_work)
    {
        AppMain.gmBoss1EggSetActionIndependent(egg_work, 1);
        AppMain.gmBoss1EffSweatInit(egg_work);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_EGG_WORK(AppMain.gmBoss1EggProcDamageUpdateLoop);
    }

    private static void gmBoss1EggProcDamageUpdateLoop(AppMain.GMS_BOSS1_EGG_WORK egg_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)egg_work)) == 0)
            return;
        egg_work.flag &= 4294967293U;
        AppMain.gmBoss1EggRevertActionIndependent(egg_work);
        AppMain.gmBoss1EggProcIdleInit(egg_work);
    }

    private static void gmBoss1EggProcEscapeInit(AppMain.GMS_BOSS1_EGG_WORK egg_work)
    {
        if (((int)egg_work.flag & 2) == 0)
            AppMain.gmBoss1EffSweatInit(egg_work);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS1_EGG_WORK(AppMain.gmBoss1EggProcEscapeUpdateLoop);
    }

    private static void gmBoss1EggProcEscapeUpdateLoop(AppMain.GMS_BOSS1_EGG_WORK egg_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)egg_work);
    }

}