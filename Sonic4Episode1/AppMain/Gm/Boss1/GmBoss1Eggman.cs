public partial class AppMain
{
    private static OBS_OBJECT_WORK GmBoss1EggInit(
        GMS_EVE_RECORD_EVENT eve_rec,
        int pos_x,
        int pos_y,
        byte type)
    {
        UNREFERENCED_PARAMETER(type);
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_BOSS1_EGG_WORK(), "BOSS1_EGG");
        GMS_ENEMY_3D_WORK gmsEnemy3DWork = (GMS_ENEMY_3D_WORK)work;
        GMS_BOSS1_EGG_WORK gmsBosS1EggWork = (GMS_BOSS1_EGG_WORK)work;
        work.move_flag |= 256U;
        ObjObjectCopyAction3dNNModel(work, gm_boss1_obj_3d_list[2], gmsEnemy3DWork.obj_3d);
        ObjObjectAction3dNNMotionLoad(work, 0, true, ObjDataGet(705), null, 0, null);
        ObjDrawObjectSetToon(work);
        work.obj_3d.blend_spd = 0.125f;
        work.disp_flag |= 134217728U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1EggWaitSetup);
        work.flag |= 16U;
        work.disp_flag |= 4U;
        work.disp_flag |= 4194304U;
        gmsEnemy3DWork.ene_com.enemy_flag |= 32768U;
        mtTaskChangeTcbDestructor(work.tcb, new GSF_TASK_PROCEDURE(gmBoss1EggExit));
        work.obj_3d.use_light_flag &= 4294967294U;
        work.obj_3d.use_light_flag |= 64U;
        return work;
    }

    private static void gmBoss1EggExit(MTS_TASK_TCB tcb)
    {
        gmBoss1MgrDecObjCreateCount(((GMS_BOSS1_EGG_WORK)mtTaskGetTcbWork(tcb)).mgr_work);
        GmEnemyDefaultExit(tcb);
    }

    private static void gmBoss1EggSetActionIndependent(
      GMS_BOSS1_EGG_WORK egg_work,
      int act_id)
    {
        gmBoss1EggSetActionIndependent(egg_work, act_id, false);
    }

    private static void gmBoss1EggSetActionIndependent(
      GMS_BOSS1_EGG_WORK egg_work,
      int act_id,
      bool force_change)
    {
        GMS_BOSS1_PART_ACT_INFO bosS1PartActInfo = gm_boss1_egg_act_id_tbl[act_id];
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        if (((int)((GMS_BOSS1_BODY_WORK)obj_work.parent_obj).flag & 2) != 0 || !force_change && ((int)egg_work.flag & 1) != 0 && egg_work.egg_act_id == act_id)
            return;
        egg_work.egg_act_id = act_id;
        egg_work.flag |= 1U;
        if (bosS1PartActInfo.is_maintain == 0)
            GmBsCmnSetAction(obj_work, bosS1PartActInfo.act_id, bosS1PartActInfo.is_repeat, bosS1PartActInfo.is_blend ? 1 : 0);
        else if (bosS1PartActInfo.is_repeat != 0)
            GMM_BS_OBJ(egg_work).disp_flag |= 4U;
        obj_work.obj_3d.speed[0] = bosS1PartActInfo.mtn_spd;
        obj_work.obj_3d.blend_spd = bosS1PartActInfo.blend_spd;
    }

    private static void gmBoss1EggRevertActionIndependent(GMS_BOSS1_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        GMS_BOSS1_BODY_WORK parentObj = (GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        MTM_ASSERT(egg_work.flag & 1U);
        egg_work.flag &= 4294967294U;
        GmBsCmnSetAction(obj_work, parentObj.egg_revert_mtn_id, gm_boss1_act_id_tbl[parentObj.whole_act_id][2].is_repeat, 1);
        obj_work.obj_3d.frame[0] = GMM_BS_OBJ(parentObj).obj_3d.frame[0];
    }

    private static void gmBoss1EggWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_BODY_WORK parentObj = (GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS1_EGG_WORK egg_work = (GMS_BOSS1_EGG_WORK)obj_work;
        if (((int)GMM_BOSS1_MGR(parentObj).flag & 1) == 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1EggMain);
        gmBoss1EggProcIdleInit(egg_work);
    }

    private static void gmBoss1EggMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_BODY_WORK parentObj = (GMS_BOSS1_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS1_EGG_WORK gmsBosS1EggWork = (GMS_BOSS1_EGG_WORK)obj_work;
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.egg_snm_reg_id, 1);
        if (gmsBosS1EggWork.proc_update != null)
            gmsBosS1EggWork.proc_update(gmsBosS1EggWork);
        if (((int)parentObj.flag & 8388608) != 0)
        {
            parentObj.flag &= 4286578687U;
            gmBoss1EggProcEscapeInit(gmsBosS1EggWork);
        }
        if (((int)parentObj.flag & 536870912) != 0)
        {
            parentObj.flag &= 3758096383U;
            gmBoss1EggProcDamageInit(gmsBosS1EggWork);
        }
        if (((int)parentObj.flag & 16777216) != 0)
        {
            parentObj.flag &= 4278190079U;
            gmBoss1SetPartTextureBurnt(obj_work);
        }
        if (((int)GMM_BS_OBJ(parentObj).disp_flag & 16) != 0)
            obj_work.disp_flag |= 16U;
        else
            obj_work.disp_flag &= 4294967279U;
    }

    private static void gmBoss1EggProcIdleInit(GMS_BOSS1_EGG_WORK egg_work)
    {
        egg_work.proc_update = new MPP_VOID_GMS_BOSS1_EGG_WORK(gmBoss1EggProcIdleUpdateLoop);
    }

    private static void gmBoss1EggProcIdleUpdateLoop(GMS_BOSS1_EGG_WORK egg_work)
    {
        GMS_BOSS1_BODY_WORK parentObj = (GMS_BOSS1_BODY_WORK)GMM_BS_OBJ(egg_work).parent_obj;
        if (((int)parentObj.flag & 268435456) == 0)
            return;
        parentObj.flag &= 4026531839U;
        gmBoss1EggProcLaughInit(egg_work);
    }

    private static void gmBoss1EggProcLaughInit(GMS_BOSS1_EGG_WORK egg_work)
    {
        gmBoss1EggSetActionIndependent(egg_work, 0);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS1_EGG_WORK(gmBoss1EggProcLaughUpdateLoop);
    }

    private static void gmBoss1EggProcLaughUpdateLoop(GMS_BOSS1_EGG_WORK egg_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(egg_work)) == 0)
            return;
        gmBoss1EggRevertActionIndependent(egg_work);
        gmBoss1EggProcIdleInit(egg_work);
    }

    private static void gmBoss1EggProcDamageInit(GMS_BOSS1_EGG_WORK egg_work)
    {
        gmBoss1EggSetActionIndependent(egg_work, 1);
        gmBoss1EffSweatInit(egg_work);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS1_EGG_WORK(gmBoss1EggProcDamageUpdateLoop);
    }

    private static void gmBoss1EggProcDamageUpdateLoop(GMS_BOSS1_EGG_WORK egg_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(egg_work)) == 0)
            return;
        egg_work.flag &= 4294967293U;
        gmBoss1EggRevertActionIndependent(egg_work);
        gmBoss1EggProcIdleInit(egg_work);
    }

    private static void gmBoss1EggProcEscapeInit(GMS_BOSS1_EGG_WORK egg_work)
    {
        if (((int)egg_work.flag & 2) == 0)
            gmBoss1EffSweatInit(egg_work);
        egg_work.proc_update = new MPP_VOID_GMS_BOSS1_EGG_WORK(gmBoss1EggProcEscapeUpdateLoop);
    }

    private static void gmBoss1EggProcEscapeUpdateLoop(GMS_BOSS1_EGG_WORK egg_work)
    {
        UNREFERENCED_PARAMETER(egg_work);
    }

}