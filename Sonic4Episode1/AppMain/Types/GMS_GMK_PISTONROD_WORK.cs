public partial class AppMain
{
    public class GMS_GMK_PISTONROD_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_EFFECT_3DNN_WORK eff_work;
        public uint obj_type;
        public int fulcrum;

        public GMS_GMK_PISTONROD_WORK()
        {
            this.eff_work = new GMS_EFFECT_3DNN_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.eff_work.efct_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(
          GMS_GMK_PISTONROD_WORK work)
        {
            return work.eff_work.efct_com.obj_work;
        }
    }
}
