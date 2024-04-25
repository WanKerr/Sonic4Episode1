public partial class AppMain
{
    public class DMS_STFRL_BOSS_BODY_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_BS_CMN_BMCB_MGR bmcb_mgr = new GMS_BS_CMN_BMCB_MGR();
        public readonly GMS_BS_CMN_SNM_WORK snm_work = new GMS_BS_CMN_SNM_WORK();
        public readonly OBS_OBJECT_WORK obj_work;
        public int egg_snm_reg_id;
        public int body_snm_reg_id;
        public uint flag;
        public int timer;

        public DMS_STFRL_BOSS_BODY_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(
          DMS_STFRL_BOSS_BODY_WORK work)
        {
            return work.obj_work;
        }
    }
}
