public partial class AppMain
{
    private static int gmBoss3MgrCheckSetupComplete(GMS_BOSS3_MGR_WORK mgr_work)
    {
        return ((int)mgr_work.flag & 1) != 0 ? 1 : 0;
    }

    private static GMS_BOSS3_MGR_WORK gmBoss3MgrGetMgrWork(
      OBS_OBJECT_WORK obj_work_parts)
    {
        return (GMS_BOSS3_MGR_WORK)obj_work_parts.user_work_OBJECT;
    }

    private static void gmBoss3MgrAddObject(
      GMS_BOSS3_MGR_WORK mgr_work,
      OBS_OBJECT_WORK obj_work_parts)
    {
        ++mgr_work.obj_create_count;
        obj_work_parts.user_work_OBJECT = mgr_work;
    }

    private static void gmBoss3MgrDeleteObject(OBS_OBJECT_WORK obj_work_parts)
    {
        --gmBoss3MgrGetMgrWork(obj_work_parts).obj_create_count;
        obj_work_parts.user_work = 0U;
    }

    private static void gmBoss3MgrMainFuncWaitLoad(OBS_OBJECT_WORK obj_work)
    {
        if (GmBsCmnIsFinalZoneType(obj_work) != 0 && !GmMainDatLoadBossBattleLoadCheck(2))
            return;
        GMS_BOSS3_MGR_WORK mgr_work = (GMS_BOSS3_MGR_WORK)obj_work;
        GMS_BOSS3_BODY_WORK gmsBosS3BodyWork = (GMS_BOSS3_BODY_WORK)GmEventMgrLocalEventBirth(319, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
        OBS_OBJECT_WORK objWork1 = gmsBosS3BodyWork.ene_3d.ene_com.obj_work;
        objWork1.parent_obj = obj_work;
        gmsBosS3BodyWork.parts_objs[0] = objWork1;
        mgr_work.body_work = gmsBosS3BodyWork;
        gmBoss3MgrAddObject(mgr_work, objWork1);
        mtTaskChangeTcbDestructor(objWork1.tcb, new GSF_TASK_PROCEDURE(gmBoss3BodyExit));
        OBS_OBJECT_WORK objWork2 = ((GMS_BOSS3_EGG_WORK)GmEventMgrLocalEventBirth(320, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0)).ene_3d.ene_com.obj_work;
        objWork2.parent_obj = objWork1;
        gmBoss3MgrAddObject(mgr_work, objWork2);
        mtTaskChangeTcbDestructor(objWork2.tcb, new GSF_TASK_PROCEDURE(gmBoss3ExitFunc));
        gmsBosS3BodyWork.parts_objs[1] = objWork2;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3MgrMainFuncWaitSetup);
    }

    private static void gmBoss3MgrMainFuncWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS3_MGR_WORK gmsBosS3MgrWork = (GMS_BOSS3_MGR_WORK)obj_work;
        GMS_BOSS3_BODY_WORK bodyWork = gmsBosS3MgrWork.body_work;
        for (int index = 0; 2 > index; ++index)
        {
            if (bodyWork.parts_objs[index] == null)
                return;
        }
        gmsBosS3MgrWork.flag |= 1U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3MgrMainFunc);
    }

    private static void gmBoss3MgrMainFunc(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS3_MGR_WORK gmsBosS3MgrWork = (GMS_BOSS3_MGR_WORK)obj_work;
        if (((int)gmsBosS3MgrWork.flag & 2) == 0)
            return;
        GMM_BS_OBJ(gmsBosS3MgrWork.body_work).flag |= 8U;
        gmsBosS3MgrWork.body_work = null;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss3MgrMainFuncWaitRelease);
    }

    private static void gmBoss3MgrMainFuncWaitRelease(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS3_MGR_WORK gmsBosS3MgrWork = (GMS_BOSS3_MGR_WORK)obj_work;
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
        {
            if (gmsBosS3MgrWork.obj_create_count > 0)
                return;
            ((GMS_ENEMY_COM_WORK)obj_work).enemy_flag |= 65536U;
            obj_work.flag |= 4U;
            GmGameDatReleaseBossBattleStart(2);
            GmGmkCamScrLimitRelease(14);
            OBS_OBJECT_WORK obj_work1 = gmBoss3BodyBattleSearchPillar();
            if (obj_work1 != null)
                GmGmkBoss3PillarWallChangeModeReturn(obj_work1);
        }
        obj_work.ppFunc = null;
    }
}