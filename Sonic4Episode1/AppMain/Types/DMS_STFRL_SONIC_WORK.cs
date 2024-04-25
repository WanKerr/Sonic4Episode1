public partial class AppMain
{
    public class DMS_STFRL_SONIC_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_OBJECT_WORK obj_work;
        public short timer;
        public ushort flag;
        public float alpha;
        public float alpha_spd;

        public DMS_STFRL_SONIC_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(DMS_STFRL_SONIC_WORK work)
        {
            return work.obj_work;
        }
    }
}
