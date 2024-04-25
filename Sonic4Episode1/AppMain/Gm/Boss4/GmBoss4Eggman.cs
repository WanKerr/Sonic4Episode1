public partial class AppMain
{
    private static OBS_OBJECT_WORK GMM_BOSS4_EGG_CREATE_WORK(
         GMS_EVE_RECORD_EVENT eve_rec,
         int pos_x,
         int pos_y,
         TaskWorkFactoryDelegate work_size,
         string name)
    {
        return GmEnemyCreateWork(eve_rec, pos_x, pos_y, work_size, 5386, name);
    }

    private static void GmBoss4EggmanBuild()
    {
        ObjDataLoadAmbIndex(ObjDataGet(731), 3, GMD_BOSS4_ARC);
    }

    private static void GmBoss4EggmanFlush()
    {
        ObjDataRelease(ObjDataGet(731));
    }

    private static OBS_OBJECT_WORK GmBoss4EggInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_BOSS4_EGG_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS4_EGG_WORK(), "Boss4_EGG");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS4_EGG_WORK gmsBosS4EggWork = (GMS_BOSS4_EGG_WORK)work;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        work.move_flag |= 256U;
        ObjObjectCopyAction3dNNModel(work, GmBoss4GetObj3D(1), gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(731), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = 0.125f;
        work.disp_flag |= 134217728U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EggWaitLoad);
        work.flag |= 16U;
        work.disp_flag |= 4U;
        work.disp_flag |= 4194304U;
        gmsBosS4EggWork.dir_work.direction = 0;
        GmBoss4UtilInitTurnGently(gmsBosS4EggWork.dir_work, 0, 1, false);
        GmBoss4UtilUpdateTurnGently(gmsBosS4EggWork.dir_work);
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss4EggExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss4EggExit(MTS_TASK_TCB tcb)
    {
        GMS_BOSS4_EGG_WORK tcbWork = (GMS_BOSS4_EGG_WORK)mtTaskGetTcbWork(tcb);
        GmBoss4DecObjCreateCount();
        GmBoss4UtilExitNodeMatrix(tcbWork.node_work);
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss4EggSetActionIndependent(
      GMS_BOSS4_EGG_WORK egg_work,
      int act_id)
    {
        gmBoss4EggSetActionIndependent(egg_work, act_id, false);
    }

    private static void gmBoss4EggSetActionIndependent(
      GMS_BOSS4_EGG_WORK egg_work,
      int act_id,
      bool force_change)
    {
        GMS_BOSS4_PART_ACT_INFO bosS4PartActInfo = gm_boss4_egg_act_id_tbl[act_id];
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        if (((int)((GMS_BOSS4_BODY_WORK)obj_work.parent_obj).flag[0] & 2) != 0 || !force_change && ((int)egg_work.flag & 1) != 0 && egg_work.egg_act_id == act_id)
            return;
        egg_work.egg_act_id = act_id;
        egg_work.flag |= 1U;
        if (bosS4PartActInfo.is_maintain == 0)
            GmBsCmnSetAction(obj_work, bosS4PartActInfo.act_id, bosS4PartActInfo.is_repeat, bosS4PartActInfo.is_blend);
        else if (bosS4PartActInfo.is_repeat != 0)
            GMM_BS_OBJ(egg_work).disp_flag |= 4U;
        obj_work.obj_3d.speed[0] = bosS4PartActInfo.mtn_spd;
        obj_work.obj_3d.blend_spd = bosS4PartActInfo.blend_spd;
    }

    private static void gmBoss4EggRevertActionIndependent(GMS_BOSS4_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        GMS_BOSS4_BODY_WORK parentObj = (GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(egg_work.flag & 1U);
        egg_work.flag &= 4294967294U;
        GmBsCmnSetAction(obj_work, GmBoss4GetActInfo(parentObj.egg_revert_mtn_id, 1).act_id, GmBoss4GetActInfo(parentObj.egg_revert_mtn_id, 1).is_repeat, 1);
        obj_work.obj_3d.frame[0] = GMM_BS_OBJ(parentObj).obj_3d.frame[0];
    }

    private static void gmBoss4EggWaitLoad(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK parentObj = (GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS4_EGG_WORK egg_work = (GMS_BOSS4_EGG_WORK)obj_work;
        if (((int)GMM_BOSS4_MGR(parentObj).flag & 1) == 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss4EggMain);
        gmBoss4EggProcIdleInit(egg_work);
        GmBoss4UtilInitNodeMatrix(egg_work.node_work, obj_work, 4);
    }

    private static void gmBoss4EggMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS4_BODY_WORK parentObj = (GMS_BOSS4_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS4_EGG_WORK gmsBosS4EggWork = (GMS_BOSS4_EGG_WORK)obj_work;
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)parentObj;
        NNS_MATRIX nodeMatrix1 = GmBoss4UtilGetNodeMatrix(parentObj.node_work, 2);
        NNS_MATRIX nodeMatrix2 = GmBoss4UtilGetNodeMatrix(parentObj.node_work, 2);
        NNS_MATRIX nnsMatrix = GlobalPool<NNS_MATRIX>.Alloc();
        nnCopyMatrix(nnsMatrix, nodeMatrix1);
        nnsMatrix.M03 = (float)(nodeMatrix1.M03 - (double)nodeMatrix2.M03 + obsObjectWork.pos.x / 4096.0);
        GmBoss4UtilSetMatrixNN(obj_work, nnsMatrix);
        GmBoss4UtilUpdateTurnGently(gmsBosS4EggWork.dir_work);
        GmBoss4UtilUpdateDirection(gmsBosS4EggWork.dir_work, obj_work);
        if (gmsBosS4EggWork.proc_update != null)
            gmsBosS4EggWork.proc_update(gmsBosS4EggWork);
        if (((int)parentObj.flag[0] & 8388608) != 0)
        {
            parentObj.flag[0] &= 4286578687U;
            gmBoss4EggProcEscapeInit(gmsBosS4EggWork);
        }
        if (((int)parentObj.flag[0] & 2097152) != 0)
        {
            parentObj.flag[0] &= 4292870143U;
            gmBoss4EggProcThrowInit(gmsBosS4EggWork);
        }
        if (((int)parentObj.flag[0] & 4194304) != 0)
        {
            parentObj.flag[0] &= 4290772991U;
            gmBoss4EggProcThrowLeftInit(gmsBosS4EggWork);
        }
        if (((int)parentObj.flag[0] & 536870912) != 0)
        {
            parentObj.flag[0] &= 3758096383U;
            gmBoss4EggProcDamageInit(gmsBosS4EggWork);
        }
        if (((int)parentObj.flag[0] & 16777216) != 0)
        {
            parentObj.flag[0] &= 4278190079U;
            gmBoss4SetPartTextureBurnt(obj_work);
        }
        if (((int)GMM_BS_OBJ(parentObj).disp_flag & 16) != 0)
            obj_work.disp_flag |= 16U;
        else
            obj_work.disp_flag &= 4294967279U;
        GlobalPool<NNS_MATRIX>.Release(nnsMatrix);
    }

    private static void gmBoss4EggProcIdleInit(GMS_BOSS4_EGG_WORK egg_work)
    {
        egg_work.proc_update = new MPP_VOID_GMS_BOSS4_EGG_WORK(gmBoss4EggProcIdleUpdateLoop);
    }

    private static void gmBoss4EggProcIdleUpdateLoop(GMS_BOSS4_EGG_WORK egg_work)
    {
        GMS_BOSS4_BODY_WORK parentObj = (GMS_BOSS4_BODY_WORK)GMM_BS_OBJ(egg_work).parent_obj;
        if (((int)parentObj.flag[0] & 268435456) == 0)
            return;
        parentObj.flag[0] &= 4026531839U;
        gmBoss4EggProcLaughInit(egg_work);
    }

    private static void gmBoss4EggProcLaughInit(GMS_BOSS4_EGG_WORK egg_work)
    {
        if (((GMS_BOSS4_BODY_WORK)((OBS_OBJECT_WORK)egg_work).parent_obj).dir.direction == 0)
            gmBoss4EggSetActionIndependent(egg_work, 1);
        else
            gmBoss4EggSetActionIndependent(egg_work, 0);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS4_EGG_WORK(gmBoss4EggProcLaughUpdateLoop);
    }

    private static void gmBoss4EggProcLaughUpdateLoop(GMS_BOSS4_EGG_WORK egg_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(egg_work)) == 0)
            return;
        gmBoss4EggRevertActionIndependent(egg_work);
        gmBoss4EggProcIdleInit(egg_work);
    }

    private static void gmBoss4EggProcThrowInit(GMS_BOSS4_EGG_WORK egg_work)
    {
        gmBoss4EggSetActionIndependent(egg_work, 3);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS4_EGG_WORK(gmBoss4EggProcThrowUpdateLoop);
    }

    private static void gmBoss4EggProcThrowLeftInit(GMS_BOSS4_EGG_WORK egg_work)
    {
        gmBoss4EggSetActionIndependent(egg_work, 4);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS4_EGG_WORK(gmBoss4EggProcThrowUpdateLoop);
    }

    private static void gmBoss4EggProcThrowUpdateLoop(GMS_BOSS4_EGG_WORK egg_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(egg_work)) == 0)
            return;
        gmBoss4EggRevertActionIndependent(egg_work);
        gmBoss4EggProcIdleInit(egg_work);
    }

    private static void gmBoss4EggProcDamageInit(GMS_BOSS4_EGG_WORK egg_work)
    {
        gmBoss4EggSetActionIndependent(egg_work, 2);
        gmBoss4EffSweatInit(egg_work);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS4_EGG_WORK(gmBoss4EggProcDamageUpdateLoop);
    }

    private static void gmBoss4EggProcDamageUpdateLoop(GMS_BOSS4_EGG_WORK egg_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(egg_work)) == 0)
            return;
        egg_work.flag &= 4294967293U;
        gmBoss4EggRevertActionIndependent(egg_work);
        gmBoss4EggProcIdleInit(egg_work);
    }

    private static void gmBoss4EggProcEscapeInit(GMS_BOSS4_EGG_WORK egg_work)
    {
        if (((int)egg_work.flag & 2) == 0)
            gmBoss4EffSweatInit(egg_work);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS4_EGG_WORK(gmBoss4EggProcEscapeUpdateLoop);
    }

    private static void gmBoss4EggProcEscapeUpdateLoop(GMS_BOSS4_EGG_WORK egg_work)
    {
    }


}