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
    private static AppMain.OBS_OBJECT_WORK GMM_BOSS4_EGG_CREATE_WORK(
         AppMain.GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         AppMain.TaskWorkFactoryDelegate work_size,
         string name)
    {
        return AppMain.GmEnemyCreateWork(eve_rec, pos_x, pos_y, work_size, (ushort)5386, name);
    }

    private static void GmBoss4EggmanBuild()
    {
        AppMain.ObjDataLoadAmbIndex(AppMain.ObjDataGet(731), 3, AppMain.GMD_BOSS4_ARC);
    }

    private static void GmBoss4EggmanFlush()
    {
        AppMain.ObjDataRelease(AppMain.ObjDataGet(731));
    }

    private static AppMain.OBS_OBJECT_WORK GmBoss4EggInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_BOSS4_EGG_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_BOSS4_EGG_WORK()), "Boss4_EGG");
        AppMain.GMS_ENEMY_3D_WORK gmsEnemy3DWork = (AppMain.GMS_ENEMY_3D_WORK)work;
        AppMain.GMS_BOSS4_EGG_WORK gmsBosS4EggWork = (AppMain.GMS_BOSS4_EGG_WORK)work;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        work.move_flag |= 256U;
        AppMain.ObjObjectCopyAction3dNNModel(work, AppMain.GmBoss4GetObj3D(1), gmsEnemy3DWork.obj_3d);
        AppMain.ObjObjectAction3dNNMotionLoad(work, 0, true, AppMain.ObjDataGet(731), (string)null, 0, (AppMain.AMS_AMB_HEADER)null);
        AppMain.ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = 0.125f;
        work.disp_flag |= 134217728U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EggWaitLoad);
        work.flag |= 16U;
        work.disp_flag |= 4U;
        work.disp_flag |= 4194304U;
        gmsBosS4EggWork.dir_work.direction = 0;
        AppMain.GmBoss4UtilInitTurnGently(gmsBosS4EggWork.dir_work, (short)0, 1, false);
        AppMain.GmBoss4UtilUpdateTurnGently(gmsBosS4EggWork.dir_work);
        AppMain.mtTaskChangeTcbDestructor(work.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss4EggExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss4EggExit(AppMain.MTS_TASK_TCB tcb)
    {
        AppMain.GMS_BOSS4_EGG_WORK tcbWork = (AppMain.GMS_BOSS4_EGG_WORK)AppMain.mtTaskGetTcbWork(tcb);
        AppMain.GmBoss4DecObjCreateCount();
        AppMain.GmBoss4UtilExitNodeMatrix(tcbWork.node_work);
        AppMain.GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss4EggSetActionIndependent(
      AppMain.GMS_BOSS4_EGG_WORK egg_work,
      int act_id)
    {
        AppMain.gmBoss4EggSetActionIndependent(egg_work, act_id, false);
    }

    private static void gmBoss4EggSetActionIndependent(
      AppMain.GMS_BOSS4_EGG_WORK egg_work,
      int act_id,
      bool force_change)
    {
        AppMain.GMS_BOSS4_PART_ACT_INFO bosS4PartActInfo = AppMain.gm_boss4_egg_act_id_tbl[act_id];
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        if (((int)((AppMain.GMS_BOSS4_BODY_WORK)obj_work.parent_obj).flag[0] & 2) != 0 || !force_change && ((int)egg_work.flag & 1) != 0 && egg_work.egg_act_id == act_id)
            return;
        egg_work.egg_act_id = act_id;
        egg_work.flag |= 1U;
        if (bosS4PartActInfo.is_maintain == (byte)0)
            AppMain.GmBsCmnSetAction(obj_work, (int)bosS4PartActInfo.act_id, (int)bosS4PartActInfo.is_repeat, bosS4PartActInfo.is_blend);
        else if (bosS4PartActInfo.is_repeat != (byte)0)
            AppMain.GMM_BS_OBJ((object)egg_work).disp_flag |= 4U;
        obj_work.obj_3d.speed[0] = bosS4PartActInfo.mtn_spd;
        obj_work.obj_3d.blend_spd = bosS4PartActInfo.blend_spd;
    }

    private static void gmBoss4EggRevertActionIndependent(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        AppMain.GMS_BOSS4_BODY_WORK parentObj = (AppMain.GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        AppMain.MTM_ASSERT(egg_work.flag & 1U);
        egg_work.flag &= 4294967294U;
        AppMain.GmBsCmnSetAction(obj_work, (int)AppMain.GmBoss4GetActInfo(parentObj.egg_revert_mtn_id, 1).act_id, (int)AppMain.GmBoss4GetActInfo(parentObj.egg_revert_mtn_id, 1).is_repeat, 1);
        obj_work.obj_3d.frame[0] = AppMain.GMM_BS_OBJ((object)parentObj).obj_3d.frame[0];
    }

    private static void gmBoss4EggWaitLoad(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK parentObj = (AppMain.GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS4_EGG_WORK egg_work = (AppMain.GMS_BOSS4_EGG_WORK)obj_work;
        if (((int)AppMain.GMM_BOSS4_MGR(parentObj).flag & 1) == 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss4EggMain);
        AppMain.gmBoss4EggProcIdleInit(egg_work);
        AppMain.GmBoss4UtilInitNodeMatrix(egg_work.node_work, obj_work, 4);
    }

    private static void gmBoss4EggMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK parentObj = (AppMain.GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS4_EGG_WORK gmsBosS4EggWork = (AppMain.GMS_BOSS4_EGG_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)parentObj;
        AppMain.NNS_MATRIX nodeMatrix1 = AppMain.GmBoss4UtilGetNodeMatrix(parentObj.node_work, 2);
        AppMain.NNS_MATRIX nodeMatrix2 = AppMain.GmBoss4UtilGetNodeMatrix(parentObj.node_work, 2);
        AppMain.NNS_MATRIX nnsMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        AppMain.nnCopyMatrix(nnsMatrix, nodeMatrix1);
        nnsMatrix.M03 = (float)((double)nodeMatrix1.M03 - (double)nodeMatrix2.M03 + (double)obsObjectWork.pos.x / 4096.0);
        AppMain.GmBoss4UtilSetMatrixNN(obj_work, nnsMatrix);
        AppMain.GmBoss4UtilUpdateTurnGently(gmsBosS4EggWork.dir_work);
        AppMain.GmBoss4UtilUpdateDirection(gmsBosS4EggWork.dir_work, obj_work);
        if (gmsBosS4EggWork.proc_update != null)
            gmsBosS4EggWork.proc_update(gmsBosS4EggWork);
        if (((int)parentObj.flag[0] & 8388608) != 0)
        {
            parentObj.flag[0] &= 4286578687U;
            AppMain.gmBoss4EggProcEscapeInit(gmsBosS4EggWork);
        }
        if (((int)parentObj.flag[0] & 2097152) != 0)
        {
            parentObj.flag[0] &= 4292870143U;
            AppMain.gmBoss4EggProcThrowInit(gmsBosS4EggWork);
        }
        if (((int)parentObj.flag[0] & 4194304) != 0)
        {
            parentObj.flag[0] &= 4290772991U;
            AppMain.gmBoss4EggProcThrowLeftInit(gmsBosS4EggWork);
        }
        if (((int)parentObj.flag[0] & 536870912) != 0)
        {
            parentObj.flag[0] &= 3758096383U;
            AppMain.gmBoss4EggProcDamageInit(gmsBosS4EggWork);
        }
        if (((int)parentObj.flag[0] & 16777216) != 0)
        {
            parentObj.flag[0] &= 4278190079U;
            AppMain.gmBoss4SetPartTextureBurnt(obj_work);
        }
        if (((int)AppMain.GMM_BS_OBJ((object)parentObj).disp_flag & 16) != 0)
            obj_work.disp_flag |= 16U;
        else
            obj_work.disp_flag &= 4294967279U;
        AppMain.GlobalPool<AppMain.NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss4EggProcIdleInit(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_EGG_WORK(AppMain.gmBoss4EggProcIdleUpdateLoop);
    }

    private static void gmBoss4EggProcIdleUpdateLoop(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        AppMain.GMS_BOSS4_BODY_WORK parentObj = (AppMain.GMS_BOSS4_BODY_WORK)AppMain.GMM_BS_OBJ((object)egg_work).parent_obj;
        if (((int)parentObj.flag[0] & 268435456) == 0)
            return;
        parentObj.flag[0] &= 4026531839U;
        AppMain.gmBoss4EggProcLaughInit(egg_work);
    }

    private static void gmBoss4EggProcLaughInit(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        if (((AppMain.GMS_BOSS4_BODY_WORK)((AppMain.OBS_OBJECT_WORK)egg_work).parent_obj).dir.direction == 0)
            AppMain.gmBoss4EggSetActionIndependent(egg_work, 1);
        else
            AppMain.gmBoss4EggSetActionIndependent(egg_work, 0);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_EGG_WORK(AppMain.gmBoss4EggProcLaughUpdateLoop);
    }

    private static void gmBoss4EggProcLaughUpdateLoop(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)egg_work)) == 0)
            return;
        AppMain.gmBoss4EggRevertActionIndependent(egg_work);
        AppMain.gmBoss4EggProcIdleInit(egg_work);
    }

    private static void gmBoss4EggProcThrowInit(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        AppMain.gmBoss4EggSetActionIndependent(egg_work, 3);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_EGG_WORK(AppMain.gmBoss4EggProcThrowUpdateLoop);
    }

    private static void gmBoss4EggProcThrowLeftInit(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        AppMain.gmBoss4EggSetActionIndependent(egg_work, 4);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_EGG_WORK(AppMain.gmBoss4EggProcThrowUpdateLoop);
    }

    private static void gmBoss4EggProcThrowUpdateLoop(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)egg_work)) == 0)
            return;
        AppMain.gmBoss4EggRevertActionIndependent(egg_work);
        AppMain.gmBoss4EggProcIdleInit(egg_work);
    }

    private static void gmBoss4EggProcDamageInit(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        AppMain.gmBoss4EggSetActionIndependent(egg_work, 2);
        AppMain.gmBoss4EffSweatInit(egg_work);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_EGG_WORK(AppMain.gmBoss4EggProcDamageUpdateLoop);
    }

    private static void gmBoss4EggProcDamageUpdateLoop(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)egg_work)) == 0)
            return;
        egg_work.flag &= 4294967293U;
        AppMain.gmBoss4EggRevertActionIndependent(egg_work);
        AppMain.gmBoss4EggProcIdleInit(egg_work);
    }

    private static void gmBoss4EggProcEscapeInit(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
        if (((int)egg_work.flag & 2) == 0)
            AppMain.gmBoss4EffSweatInit(egg_work);
        egg_work.proc_update = new AppMain.MPP_VOID_GMS_BOSS4_EGG_WORK(AppMain.gmBoss4EggProcEscapeUpdateLoop);
    }

    private static void gmBoss4EggProcEscapeUpdateLoop(AppMain.GMS_BOSS4_EGG_WORK egg_work)
    {
    }


}