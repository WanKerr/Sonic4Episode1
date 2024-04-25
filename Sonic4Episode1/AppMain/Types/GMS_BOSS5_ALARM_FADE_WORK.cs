public partial class AppMain
{
    public class GMS_BOSS5_ALARM_FADE_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_BOSS5_1SHOT_TIMER alert_se_timer = new GMS_BOSS5_1SHOT_TIMER();
        public readonly GMS_FADE_OBJ_WORK fade_obj;
        public GMS_BOSS5_MGR_WORK mgr_work;
        public MPP_VOID_GMS_BOSS5_ALARM_FADE_WORK proc_update;
        public int cur_phase;
        public int cur_level;
        public uint wait_timer;
        public int alert_se_ref_level;

        public GMS_BOSS5_ALARM_FADE_WORK()
        {
            this.fade_obj = new GMS_FADE_OBJ_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.fade_obj.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(
          GMS_BOSS5_ALARM_FADE_WORK work)
        {
            return work.fade_obj.obj_work;
        }
    }
}
