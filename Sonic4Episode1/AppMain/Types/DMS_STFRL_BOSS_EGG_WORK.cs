public partial class AppMain
{
    public class DMS_STFRL_BOSS_EGG_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_OBJECT_WORK obj_work;
        public uint flag;
        public int timer;

        public DMS_STFRL_BOSS_EGG_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }

        public static explicit operator OBS_OBJECT_WORK(
          DMS_STFRL_BOSS_EGG_WORK work)
        {
            return work.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }
    }
}
