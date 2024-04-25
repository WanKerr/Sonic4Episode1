public partial class AppMain
{
    private static OBS_OBJECT_WORK GmGmkBoss5TriggerInit(
      GMS_EVE_RECORD_EVENT eve_rec,
      int pos_x,
      int pos_y,
      byte type)
    {
        OBS_OBJECT_WORK work = GMM_ENEMY_CREATE_WORK(eve_rec, pos_x, pos_y, () => new GMS_GMK_BOSS5_TRIGGER_WORK(), "BOSS5_TRIGGER");
        work.flag |= 16U;
        work.disp_flag &= 4294967263U;
        work.move_flag |= 8448U;
        work.move_flag &= 4294967167U;
        work.ppFunc = new MPP_VOID_OBS_OBJECT_WORK(gmGmkBoss5TriggerMain);
        return work;
    }

    private static void gmGmkBoss5TriggerMain(OBS_OBJECT_WORK obj_work)
    {
        OBS_OBJECT_WORK obsObjectWork = (OBS_OBJECT_WORK)g_gm_main_system.ply_work[0];
        if (obsObjectWork == null || obsObjectWork.pos.x < obj_work.pos.x || !gmGmkBoss5TriggerTryAnnounce())
            return;
        obj_work.flag |= 4U;
    }

    private static bool gmGmkBoss5TriggerTryAnnounce()
    {
        OBS_OBJECT_WORK obsObjectWork = ObjObjectSearchRegistObject(null, 2);
        while (obsObjectWork != null)
        {
            GMS_ENEMY_COM_WORK gmsEnemyComWork = (GMS_ENEMY_COM_WORK)obsObjectWork;
            if (gmsEnemyComWork.eve_rec != null && gmsEnemyComWork.eve_rec.id == 55)
                break;
        }
        if (obsObjectWork == null)
            return false;
        GmBoss5MgrAnnouncePassedTrigger((GMS_BOSS5_MGR_WORK)obsObjectWork);
        return true;
    }

    private static void GmBoss5MgrAnnouncePassedTrigger(GMS_BOSS5_MGR_WORK mgr_work)
    {
        mgr_work.flag |= 4194304U;
    }
}
