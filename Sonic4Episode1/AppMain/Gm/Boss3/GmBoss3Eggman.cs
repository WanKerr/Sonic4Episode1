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
    private static void gmBoss3EggChangeAction(AppMain.GMS_BOSS3_EGG_WORK egg_work, int action_id)
    {
        AppMain.gmBoss3EggChangeAction(egg_work, action_id, 0);
    }

    private static void gmBoss3EggChangeAction(
      AppMain.GMS_BOSS3_EGG_WORK egg_work,
      int action_id,
      int force_change)
    {
        AppMain.GMS_BOSS3_PART_ACT_INFO bosS3PartActInfo = AppMain.gm_boss3_egg_act_info_tbl[action_id];
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        if (force_change == 0 && egg_work.egg_action_id == action_id && ((int)egg_work.flag & 1) != 0)
            return;
        egg_work.egg_action_id = action_id;
        egg_work.flag |= 1U;
        if (bosS3PartActInfo.is_maintain != (byte)0)
        {
            if (bosS3PartActInfo.is_repeat != (byte)0)
                obj_work.disp_flag |= 4U;
        }
        else
            AppMain.GmBsCmnSetAction(obj_work, (int)bosS3PartActInfo.mtn_id, (int)bosS3PartActInfo.is_repeat, bosS3PartActInfo.is_blend);
        obj_work.obj_3d.speed[0] = bosS3PartActInfo.mtn_spd;
        obj_work.obj_3d.blend_spd = bosS3PartActInfo.blend_spd;
    }

    private static void gmBoss3EggRevertAction(AppMain.GMS_BOSS3_EGG_WORK egg_work)
    {
        AppMain.OBS_OBJECT_WORK obj_work = AppMain.GMM_BS_OBJ((object)egg_work);
        AppMain.GMS_BOSS3_BODY_WORK parentObj = (AppMain.GMS_BOSS3_BODY_WORK)obj_work.parent_obj;
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.GMM_BS_OBJ((object)parentObj);
        egg_work.flag &= 4294967294U;
        AppMain.GMS_BOSS3_PART_ACT_INFO bosS3PartActInfo = AppMain.gm_boss3_act_info_tbl[parentObj.action_id][1];
        AppMain.GmBsCmnSetAction(obj_work, (int)bosS3PartActInfo.mtn_id, (int)bosS3PartActInfo.is_repeat, 1);
        obj_work.obj_3d.frame[0] = obsObjectWork.obj_3d.frame[0];
    }

    private static void gmBoss3EggStateIdleInit(AppMain.GMS_BOSS3_EGG_WORK egg_work)
    {
        egg_work.proc_update = new AppMain.GMF_BOSS3_EGG_STATE_FUNC(AppMain.gmBoss3EggStateIdleUpdate);
    }

    private static void gmBoss3EggStateIdleUpdate(AppMain.GMS_BOSS3_EGG_WORK egg_work)
    {
        AppMain.GMS_BOSS3_BODY_WORK parentObj = (AppMain.GMS_BOSS3_BODY_WORK)AppMain.GMM_BS_OBJ((object)egg_work).parent_obj;
        if (((int)parentObj.flag & 268435456) == 0)
            return;
        parentObj.flag &= 4026531839U;
        AppMain.gmBoss3EggStateLaughInit(egg_work);
    }

    private static void gmBoss3EggStateLaughInit(AppMain.GMS_BOSS3_EGG_WORK egg_work)
    {
        if (((int)AppMain.GMM_BS_OBJ((object)egg_work).parent_obj.disp_flag & 1) != 0)
            AppMain.gmBoss3EggChangeAction(egg_work, 0);
        else
            AppMain.gmBoss3EggChangeAction(egg_work, 1);
        egg_work.proc_update = new AppMain.GMF_BOSS3_EGG_STATE_FUNC(AppMain.gmBoss3EggStateLaughUpdate);
    }

    private static void gmBoss3EggStateLaughUpdate(AppMain.GMS_BOSS3_EGG_WORK egg_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)egg_work)) == 0)
            return;
        AppMain.gmBoss3EggRevertAction(egg_work);
        AppMain.gmBoss3EggStateIdleInit(egg_work);
    }

    private static void gmBoss3EggStateDamageInit(AppMain.GMS_BOSS3_EGG_WORK egg_work)
    {
        AppMain.gmBoss3EggChangeAction(egg_work, 2);
        AppMain.gmBoss3EffSweatInit(egg_work);
        egg_work.proc_update = new AppMain.GMF_BOSS3_EGG_STATE_FUNC(AppMain.gmBoss3EggStateDamageUpdate);
    }

    private static void gmBoss3EggStateDamageUpdate(AppMain.GMS_BOSS3_EGG_WORK egg_work)
    {
        if (AppMain.GmBsCmnIsActionEnd(AppMain.GMM_BS_OBJ((object)egg_work)) == 0)
            return;
        egg_work.flag &= 4294967293U;
        AppMain.gmBoss3EggRevertAction(egg_work);
        AppMain.gmBoss3EggStateIdleInit(egg_work);
    }

    private static void gmBoss3EggStateEscapeInit(AppMain.GMS_BOSS3_EGG_WORK egg_work)
    {
        if (((int)egg_work.flag & 2) == 0)
            AppMain.gmBoss3EffSweatInit(egg_work);
        egg_work.proc_update = new AppMain.GMF_BOSS3_EGG_STATE_FUNC(AppMain.gmBoss3EggStateEscapeUpdate);
    }

    private static void gmBoss3EggStateEscapeUpdate(AppMain.GMS_BOSS3_EGG_WORK egg_work)
    {
        AppMain.UNREFERENCED_PARAMETER((object)egg_work);
    }

    private static void gmBoss3EggmanMainFuncWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.gmBoss3MgrCheckSetupComplete(AppMain.gmBoss3MgrGetMgrWork(AppMain.GMM_BS_OBJ((object)(AppMain.GMS_BOSS3_BODY_WORK)obj_work.parent_obj))) == 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3EggmanMainFunc);
        AppMain.gmBoss3EggStateIdleInit((AppMain.GMS_BOSS3_EGG_WORK)obj_work);
    }

    private static void gmBoss3EggmanMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS3_BODY_WORK parentObj = (AppMain.GMS_BOSS3_BODY_WORK)obj_work.parent_obj;
        AppMain.GMS_BOSS3_EGG_WORK egg_work = (AppMain.GMS_BOSS3_EGG_WORK)obj_work;
        AppMain.GmBsCmnUpdateObject3DNNStuckWithNode(obj_work, parentObj.snm_work, parentObj.snm_reg_id[0], 1);
        if (egg_work.proc_update != null)
            egg_work.proc_update(egg_work);
        if (((int)parentObj.flag & 8388608) != 0)
        {
            parentObj.flag &= 4286578687U;
            AppMain.gmBoss3EggStateEscapeInit(egg_work);
        }
        if (((int)parentObj.flag & 536870912) != 0)
        {
            parentObj.flag &= 3758096383U;
            AppMain.gmBoss3EggStateDamageInit(egg_work);
        }
        if (((int)parentObj.flag & 16777216) != 0)
        {
            parentObj.flag &= 4278190079U;
            AppMain.gmBoss3ChangeTextureBurnt(obj_work);
        }
        if (((int)AppMain.GMM_BS_OBJ((object)parentObj).disp_flag & 16) != 0)
            obj_work.disp_flag |= 16U;
        else
            obj_work.disp_flag &= 4294967279U;
    }

}