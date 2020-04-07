using System;
using System.Collections.Generic;
using System.Text;

public partial class AppMain
{
    private static AppMain.OBS_OBJECT_WORK GmGmkBoss5TriggerInit(
      AppMain.GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        AppMain.OBS_OBJECT_WORK work = AppMain.GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, (AppMain.TaskWorkFactoryDelegate)(() => (object)new AppMain.GMS_GMK_BOSS5_TRIGGER_WORK()), "BOSS5_TRIGGER");
        work.flag |= 16U;
        work.disp_flag &= 4294967263U;
        work.move_flag |= 8448U;
        work.move_flag &= 4294967167U;
        work.ppFunc = new AppMain.MPP_VOID_OBS_OBJECT_WORK(AppMain.gmGmkBoss5TriggerMain);
        return work;
    }

    private static void gmGmkBoss5TriggerMain(AppMain.OBS_OBJECT_WORK obj_work)
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = (AppMain.OBS_OBJECT_WORK)AppMain.g_gm_main_system.ply_work[0];
        if (obsObjectWork == null || obsObjectWork.pos.x < obj_work.pos.x || !AppMain.gmGmkBoss5TriggerTryAnnounce())
            return;
        obj_work.flag |= 4U;
    }

    private static bool gmGmkBoss5TriggerTryAnnounce()
    {
        AppMain.OBS_OBJECT_WORK obsObjectWork = AppMain.ObjObjectSearchRegistObject((AppMain.OBS_OBJECT_WORK)null, (ushort)2);
        while (obsObjectWork != null)
        {
            AppMain.GMS_ENEMY_COM_WORK gmsEnemyComWork = (AppMain.GMS_ENEMY_COM_WORK)obsObjectWork;
            if (gmsEnemyComWork.eve_rec != null && gmsEnemyComWork.eve_rec.id == (ushort)55)
                break;
        }
        if (obsObjectWork == null)
            return false;
        AppMain.GmBoss5MgrAnnouncePassedTrigger((AppMain.GMS_BOSS5_MGR_WORK)obsObjectWork);
        return true;
    }

    private static void GmBoss5MgrAnnouncePassedTrigger(AppMain.GMS_BOSS5_MGR_WORK mgr_work)
    {
        mgr_work.flag |= 4194304U;
    }
}
