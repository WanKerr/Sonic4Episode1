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
    public static AppMain.GMS_BOSS1_MGR_WORK GMM_BOSS1_MGR(AppMain.GMS_BOSS1_BODY_WORK work)
    {
        return work.mgr_work;
    }

    private static void gmBoss1MgrIncObjCreateCount(AppMain.GMS_BOSS1_MGR_WORK mgr_work)
    {
        AppMain.MTM_ASSERT(mgr_work.obj_create_cnt >= 0);
        ++mgr_work.obj_create_cnt;
    }

    private static void gmBoss1MgrDecObjCreateCount(AppMain.GMS_BOSS1_MGR_WORK mgr_work)
    {
        AppMain.MTM_ASSERT(mgr_work.obj_create_cnt > 0);
        --mgr_work.obj_create_cnt;
    }

    private static bool gmBoss1MgrIsAllCreatedObjDeleted(AppMain.GMS_BOSS1_MGR_WORK mgr_work)
    {
        AppMain.MTM_ASSERT(mgr_work.obj_create_cnt >= 0);
        return mgr_work.obj_create_cnt <= 0;
    }

    private static void gmBoss1MgrWaitLoad(AppMain.OBS_OBJECT_WORK obj_work)
    {
        bool flag = false;
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) != 0)
        {
            if (AppMain.GmMainDatLoadBossBattleLoadCheck(0))
                flag = true;
        }
        else
            flag = true;
        if (!flag)
            return;
        AppMain.GMS_BOSS1_MGR_WORK mgr_work = (AppMain.GMS_BOSS1_MGR_WORK)obj_work;
        AppMain.OBS_OBJECT_WORK obsObjectWork1 = AppMain.GmEventMgrLocalEventBirth((ushort)313, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        AppMain.gmBoss1MgrIncObjCreateCount(mgr_work);
        AppMain.OBS_OBJECT_WORK obsObjectWork2 = AppMain.GmEventMgrLocalEventBirth((ushort)314, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        AppMain.gmBoss1MgrIncObjCreateCount(mgr_work);
        AppMain.OBS_OBJECT_WORK obsObjectWork3 = AppMain.GmEventMgrLocalEventBirth((ushort)315, obj_work.pos.x, obj_work.pos.y, (ushort)0, (sbyte)0, (sbyte)0, (byte)0, (byte)0, (byte)0);
        AppMain.gmBoss1MgrIncObjCreateCount(mgr_work);
        AppMain.GMS_BOSS1_BODY_WORK gmsBosS1BodyWork = (AppMain.GMS_BOSS1_BODY_WORK)obsObjectWork1;
        AppMain.GMS_BOSS1_CHAIN_WORK gmsBosS1ChainWork = (AppMain.GMS_BOSS1_CHAIN_WORK)obsObjectWork2;
        AppMain.GMS_BOSS1_EGG_WORK gmsBosS1EggWork = (AppMain.GMS_BOSS1_EGG_WORK)obsObjectWork3;
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
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1MgrWaitSetup);
    }

    private static void gmBoss1MgrWaitSetup(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_MGR_WORK gmsBosS1MgrWork = (AppMain.GMS_BOSS1_MGR_WORK)obj_work;
        AppMain.GMS_BOSS1_BODY_WORK bodyWork = gmsBosS1MgrWork.body_work;
        bool flag = true;
        for (int index = 0; index < 3; ++index)
        {
            if (bodyWork.parts_objs[index] == null)
                flag = false;
        }
        if (!flag)
            return;
        gmsBosS1MgrWork.flag |= 1U;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1MgrMain);
    }

    private static void gmBoss1MgrMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.GMS_BOSS1_MGR_WORK gmsBosS1MgrWork = (AppMain.GMS_BOSS1_MGR_WORK)obj_work;
        if (((int)gmsBosS1MgrWork.flag & 2) == 0)
            return;
        if (gmsBosS1MgrWork.body_work != null)
        {
            AppMain.GMM_BS_OBJ((object)gmsBosS1MgrWork.body_work).flag |= 8U;
            gmsBosS1MgrWork.body_work = (AppMain.GMS_BOSS1_BODY_WORK)null;
        }
        if (AppMain.GmBsCmnIsFinalZoneType(obj_work) == 0)
            return;
        obj_work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmBoss1MgrWaitRelease);
    }

    private static void gmBoss1MgrWaitRelease(AppMain.OBS_OBJECT_WORK obj_work)
    {
        if (!AppMain.gmBoss1MgrIsAllCreatedObjDeleted((AppMain.GMS_BOSS1_MGR_WORK)obj_work))
            return;
        ((AppMain.GMS_ENEMY_COM_WORK)obj_work).enemy_flag |= 65536U;
        obj_work.flag |= 4U;
        AppMain.GmGameDatReleaseBossBattleStart(0);
        AppMain.GmGmkCamScrLimitRelease((byte)14);
    }

}