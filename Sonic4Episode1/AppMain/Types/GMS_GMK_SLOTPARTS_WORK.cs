public partial class AppMain
{
    public class GMS_GMK_SLOTPARTS_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_EFFECT_3DNN_WORK eff_work;
        public GMS_GMK_SLOT_WORK slot_work;
        public int reel_id;
        public float tex_v;

        public GMS_GMK_SLOTPARTS_WORK()
        {
            this.eff_work = new GMS_EFFECT_3DNN_WORK(this);
        }

        public static explicit operator OBS_OBJECT_WORK(
          GMS_GMK_SLOTPARTS_WORK work)
        {
            return work.eff_work.efct_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.eff_work.efct_com.obj_work;
        }
    }
}
