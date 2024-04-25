public partial class AppMain
{
    public static GMS_BOSS1_MGR_WORK GMM_BOSS1_MGR(GMS_BOSS1_BODY_WORK work)
    {
        return work.mgr_work;
    }

    private static void gmBoss1MgrIncObjCreateCount(GMS_BOSS1_MGR_WORK mgr_work)
    {
        MTM_ASSERT(mgr_work.obj_create_cnt >= 0);
        ++mgr_work.obj_create_cnt;
    }

    private static void gmBoss1MgrDecObjCreateCount(GMS_BOSS1_MGR_WORK mgr_work)
    {
        MTM_ASSERT(mgr_work.obj_create_cnt > 0);
        --mgr_work.obj_create_cnt;
    }

    private static bool gmBoss1MgrIsAllCreatedObjDeleted(GMS_BOSS1_MGR_WORK mgr_work)
    {
        MTM_ASSERT(mgr_work.obj_create_cnt >= 0);
        return mgr_work.obj_create_cnt <= 0;
    }

    private static void gmBoss1MgrWaitLoad(OBS_OBJECT_WORK obj_work)
    {
        bool flag = false;
        if (GmBsCmnIsFinalZoneType(obj_work) != 0)
        {
            if (GmMainDatLoadBossBattleLoadCheck(0))
                flag = true;
        }
        else
            flag = true;
        if (!flag)
            return;
        GMS_BOSS1_MGR_WORK mgr_work = (GMS_BOSS1_MGR_WORK)obj_work;
        OBS_OBJECT_WORK obsObjectWork1 = GmEventMgrLocalEventBirth(313, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
        gmBoss1MgrIncObjCreateCount(mgr_work);
        OBS_OBJECT_WORK obsObjectWork2 = GmEventMgrLocalEventBirth(314, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
        gmBoss1MgrIncObjCreateCount(mgr_work);
        OBS_OBJECT_WORK obsObjectWork3 = GmEventMgrLocalEventBirth(315, obj_work.pos.x, obj_work.pos.y, 0, 0, 0, 0, 0, 0);
        gmBoss1MgrIncObjCreateCount(mgr_work);
        GMS_BOSS1_BODY_WORK gmsBosS1BodyWork = (GMS_BOSS1_BODY_WORK)obsObjectWork1;
        GMS_BOSS1_CHAIN_WORK gmsBosS1ChainWork = (GMS_BOSS1_CHAIN_WORK)obsObjectWork2;
        GMS_BOSS1_EGG_WORK gmsBosS1EggWork = (GMS_BOSS1_EGG_WORK)obsObjectWork3;
        mgr_work.body_work = gmsBosS1BodyWork;
        gmsBosS1BodyWork.mgr_work = mgr_work;
        gmsBosS1ChainWork.mgr_work = mgr_work;
        gmsBosS1EggWork.mgr_work = mgr_work;
        obsObjectWork1.parent_obj = obj_work;
        obsObjectWork2.parent_obj = obsObjectWork1;
        obsObjectWork3.parent_obj = obsObjectWork1;
        gmsBosS1BodyWork.parts_objs[0] = obsObjectWork1;
        gmsBosS1BodyWork.parts_objs[1] = obsObjectWork2;
        gmsBosS1BodyWork.parts_objs[2] = obsObjectWork3;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1MgrWaitSetup);
    }

    private static void gmBoss1MgrWaitSetup(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_MGR_WORK gmsBosS1MgrWork = (GMS_BOSS1_MGR_WORK)obj_work;
        GMS_BOSS1_BODY_WORK bodyWork = gmsBosS1MgrWork.body_work;
        bool flag = true;
        for (int index = 0; index < 3; ++index)
        {
            if (bodyWork.parts_objs[index] == null)
                flag = false;
        }
        if (!flag)
            return;
        gmsBosS1MgrWork.flag |= 1U;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1MgrMain);
    }

    private static void gmBoss1MgrMain(OBS_OBJECT_WORK obj_work)
    {
        GMS_BOSS1_MGR_WORK gmsBosS1MgrWork = (GMS_BOSS1_MGR_WORK)obj_work;
        if (((int)gmsBosS1MgrWork.flag & 2) == 0)
            return;
        if (gmsBosS1MgrWork.body_work != null)
        {
            GMM_BS_OBJ(gmsBosS1MgrWork.body_work).flag |= 8U;
            gmsBosS1MgrWork.body_work = null;
        }
        if (GmBsCmnIsFinalZoneType(obj_work) == 0)
            return;
        obj_work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmBoss1MgrWaitRelease);
    }

    private static void gmBoss1MgrWaitRelease(OBS_OBJECT_WORK obj_work)
    {
        if (!gmBoss1MgrIsAllCreatedObjDeleted((GMS_BOSS1_MGR_WORK)obj_work))
            return;
        ((GMS_ENEMY_COM_WORK)obj_work).enemy_flag |= 65536U;
        obj_work.flag |= 4U;
        GmGameDatReleaseBossBattleStart(0);
        GmGmkCamScrLimitRelease(14);
    }

}