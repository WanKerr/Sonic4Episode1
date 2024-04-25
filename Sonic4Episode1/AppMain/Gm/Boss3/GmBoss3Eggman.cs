public partial class AppMain
{
    private static void gmBoss3EggChangeAction(GMS_BOSS3_EGG_WORK egg_work, int action_id)
    {
        gmBoss3EggChangeAction(egg_work, action_id, 0);
    }

    private static void gmBoss3EggChangeAction(
      GMS_BOSS3_EGG_WORK egg_work,
      int action_id,
      int force_change)
    {
        GMS_BOSS3_PART_ACT_INFO bosS3PartActInfo = gm_boss3_egg_act_info_tbl[action_id];
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        if (force_change == 0 && egg_work.egg_action_id == action_id && ((int)egg_work.flag & 1) != 0)
            return;
        egg_work.egg_action_id = action_id;
        egg_work.flag |= 1U;
        if (bosS3PartActInfo.is_maintain != 0)
        {
            if (bosS3PartActInfo.is_repeat != 0)
                obj_work.disp_flag |= 4U;
        }
        else
            GmBsCmnSetAction(obj_work, bosS3PartActInfo.mtn_id, bosS3PartActInfo.is_repeat, bosS3PartActInfo.is_blend);
        obj_work.obj_3d.speed[0] = bosS3PartActInfo.mtn_spd;
        obj_work.obj_3d.blend_spd = bosS3PartActInfo.blend_spd;
    }

    private static void gmBoss3EggRevertAction(GMS_BOSS3_EGG_WORK egg_work)
    {
        OBS_OBJECT_WORK obj_work = GMM_BS_OBJ(egg_work);
        GMS_BOSS3_BODY_WORK parentObj = (GMS_BOSS3_BODY_WORK)obj_work.parent_obj;
        OBS_OBJECT_WORK obsObjectWork = GMM_BS_OBJ(parentObj);
        egg_work.flag &= 4294967294U;
        GMS_BOSS3_PART_ACT_INFO bosS3PartActInfo = gm_boss3_act_info_tbl[parentObj.action_id][1];
        GmBsCmnSetAction(obj_work, bosS3PartActInfo.mtn_id, bosS3PartActInfo.is_repeat, 1);
        obj_work.obj_3d.frame[0] = obsObjectWork.obj_3d.frame[0];
    }

    private static void gmBoss3EggStateIdleInit(GMS_BOSS3_EGG_WORK egg_work)
    {
        egg_work.proc_update = new GMF_BOSS3_EGG_STATE_FUNC(gmBoss3EggStateIdleUpdate);
    }

    private static void gmBoss3EggStateIdleUpdate(GMS_BOSS3_EGG_WORK egg_work)
    {
        GMS_BOSS3_BODY_WORK parentObj = (GMS_BOSS3_BODY_WORK)GMM_BS_OBJ(egg_work).parent_obj;
        if (((int)parentObj.flag & 268435456) == 0)
            return;
        parentObj.flag &= 4026531839U;
        gmBoss3EggStateLaughInit(egg_work);
    }

    private static void gmBoss3EggStateLaughInit(GMS_BOSS3_EGG_WORK egg_work)
    {
        if (((int)GMM_BS_OBJ(egg_work).parent_obj.disp_flag & 1) != 0)
            gmBoss3EggChangeAction(egg_work, 0);
        else
            gmBoss3EggChangeAction(egg_work, 1);
        egg_work.proc_update = new GMF_BOSS3_EGG_STATE_FUNC(gmBoss3EggStateLaughUpdate);
    }

    private static void gmBoss3EggStateLaughUpdate(GMS_BOSS3_EGG_WORK egg_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(egg_work)) == 0)
            return;
        gmBoss3EggRevertAction(egg_work);
        gmBoss3EggStateIdleInit(egg_work);
    }

    private static void gmBoss3EggStateDamageInit(GMS_BOSS3_EGG_WORK egg_work)
    {
        gmBoss3EggChangeAction(egg_work, 2);
        gmBoss3EffSweatInit(egg_work);
        egg_work.proc_update = new GMF_BOSS3_EGG_STATE_FUNC(gmBoss3EggStateDamageUpdate);
    }

    private static void gmBoss3EggStateDamageUpdate(GMS_BOSS3_EGG_WORK egg_work)
    {
        if (GmBsCmnIsActionEnd(GMM_BS_OBJ(egg_work)) == 0)
            return;
        egg_work.flag &= 4294967293U;
        gmBoss3EggRevertAction(egg_work);
        gmBoss3EggStateIdleInit(egg_work);
    }

    private static void gmBoss3EggStateEscapeInit(GMS_BOSS3_EGG_WORK egg_work)
    {
        if (((int)egg_work.flag & 2) == 0)
            gmBoss3EffSweatInit(egg_work);
        egg_work.proc_update = new GMF_BOSS3_EGG_STATE_FUNC(gmBoss3EggStateEscapeUpdate);
    }

    private static void gmBoss3EggStateEscapeUpdate(GMS_BOSS3_EGG_WORK egg_work)
    {
        UNREFERENCED_PARAMETER(egg_work);
    }

    private static void gmBoss3EggmanMainFuncWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        if (gmBoss3MgrCheckSetupComplete(gmBoss3MgrGetMgrWork(GMM_BS_OBJ((GMS_BOSS3_BODY_WORK)obj_work.parent_obj))) == 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3EggmanMainFunc);
        gmBoss3EggStateIdleInit((GMS_BOSS3_EGG_WORK)obj_work);
    }

    private static void gmBoss3EggmanMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS3_BODY_WORK parentObj = (GMS_BOSS3_BODY_WORK)obj_work.parent_obj;
        GMS_BOSS3_EGG_WORK egg_work = (GMS_BOSS3_EGG_WORK)obj_work;
        GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
        if (egg_work.proc_update != null)
            egg_work.proc_update(egg_work);
        if (((int)parentObj.flag & 8388608) != 0)
        {
            parentObj.flag &= 4286578687U;
            gmBoss3EggStateEscapeInit(egg_work);
        }
        if (((int)parentObj.flag & 536870912) != 0)
        {
            parentObj.flag &= 3758096383U;
            gmBoss3EggStateDamageInit(egg_work);
        }
        if (((int)parentObj.flag & 16777216) != 0)
        {
            parentObj.flag &= 4278190079U;
            gmBoss3ChangeTextureBurnt(obj_work);
        }
        if (((int)GMM_BS_OBJ(parentObj).disp_flag & 16) != 0)
            obj_work.disp_flag |= 16U;
        else
            obj_work.disp_flag &= 4294967279U;
    }

}