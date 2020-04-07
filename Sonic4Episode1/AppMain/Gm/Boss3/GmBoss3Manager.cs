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
    private static int gmBoss3MgrCheckSetupComplete(AppMain.GMS_BOSS3_MGR_WORK mgr_work)
    {
        return ((int)mgr_work.flag & 1) != 0 ? 1 : 0;
    }

    private static AppMain.GMS_BOSS3_MGR_WORK gmBoss3MgrGetMgrWork(
      AppMain.OBS_OBJECT_WORK obj_work_parts)
    {
        return (AppMain.GMS_BOSS3_MGR_WORK)obj_work_parts.user_work_OBJECT;
    }

    private static void gmBoss3MgrAddObject(
      AppMain.GMS_BOSS3_MGR_WORK mgr_work,
      AppMain.OBS_OBJECT_WORK obj_work_parts)
    {
        ++mgr_work.obj_create_count;
        obj_work_parts.user_work_OBJECT = (object)mgr_work;
    }

    private static void gmBoss3MgrDeleteObject(AppMain.OBS_OBJECT_WORK obj_work_parts)
    {
        --AppMain.gmBoss3MgrGetMgrWork(obj_work_parts).obj_create_count;
        obj_work_parts.user_work = 0U;
    }

    private static void gmBoss3MgrMainFuncWaitLoad(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0 && !AppMain.GmMainDatLoadBossBattleLoadCheck(2))
            return;
        AppMain.GMS_BOSS3_MGR_WORK mgr_work = (AppMain.GMS_BOSS3_MGR_WORK)obj_work;
        AppMain.GMS_BOSS3_BODY_WORK gmsBosS3BodyWork = (AppMain.GMS_BOSS3_BODY_WORK)AppMain.GmEventMgrLocalEventBirth((ushort)319, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        AppMain.OBS_OBJECT_WORK objWork1 = gmsBosS3BodyWork.ene_3d.ene_com.obj_work;
        objWork1.parent_obj = obj_work;
        gmsBosS3BodyWork.parts_objs[0] = objWork1;
        mgr_work.body_work = gmsBosS3BodyWork;
        AppMain.gmBoss3MgrAddObject(mgr_work, objWork1);
        AppMain.mtTaskChangeTcbDestructor(objWork1.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss3BodyExit));
        AppMain.OBS_OBJECT_WORK objWork2 = ((AppMain.GMS_BOSS3_EGG_WORK)AppMain.GmEventMgrLocalEventBirth((ushort)320, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0)).ene_3d.ene_com.obj_work;
        objWork2.parent_obj = objWork1;
        AppMain.gmBoss3MgrAddObject(mgr_work, objWork2);
        AppMain.mtTaskChangeTcbDestructor(objWork2.tcb, new AppMain.GSF_TASK_PROCEDURE(AppMain.gmBoss3ExitFunc));
        gmsBosS3BodyWork.parts_objs[1] = objWork2;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3MgrMainFuncWaitSetup);
    }

    private static void gmBoss3MgrMainFuncWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS3_MGR_WORK gmsBosS3MgrWork = (AppMain.GMS_BOSS3_MGR_WORK)obj_work;
        AppMain.GMS_BOSS3_BODY_WORK bodyWork = gmsBosS3MgrWork.body_work;
        for (int index = 0; 2 > index; ++index)
        {
            if (bodyWork.parts_objs[index] == null)
                return;
        }
        gmsBosS3MgrWork.flag |= 1U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3MgrMainFunc);
    }

    private static void gmBoss3MgrMainFunc(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS3_MGR_WORK gmsBosS3MgrWork = (AppMain.GMS_BOSS3_MGR_WORK)obj_work;
        if (((int)gmsBosS3MgrWork.flag & 2) == 0)
            return;
        AppMain.GMM_BS_OBJ((object)gmsBosS3MgrWork.body_work).flag |= 8U;
        gmsBosS3MgrWork.body_work = (AppMain.GMS_BOSS3_BODY_WORK)null;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss3MgrMainFuncWaitRelease);
    }

    private static void gmBoss3MgrMainFuncWaitRelease(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS3_MGR_WORK gmsBosS3MgrWork = (AppMain.GMS_BOSS3_MGR_WORK)obj_work;
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
        {
            if (gmsBosS3MgrWork.obj_create_count > 0)
                return;
            ((AppMain.GMS_ENEMY_COM_WORK)obj_work).enemy_flag |= 65536U;
            obj_work.flag |= 4U;
            AppMain.GmGameDatReleaseBossBattleStart(2);
            AppMain.GmGmkCamScrLimitRelease((byte)14);
            AppMain.OBS_OBJECT_WORK obj_work1 = AppMain.gmBoss3BodyBattleSearchPillar();
            if (obj_work1 != null)
                AppMain.GmGmkBoss3PillarWallChangeModeReturn(obj_work1);
        }
        obj_work.ppFunc = (AppMain.MPP_VOID_OBS_OBJECT_WORK)null;
    }
}