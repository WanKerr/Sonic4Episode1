public partial class AppMain
{
    public class GMS_GMK_BOBJ_PARTS : IOBS_OBJECT_WORK
    {
        public readonly GMS_EFFECT_3DNN_WORK eff_work;
        public short falltimer;

        public OBS_OBJECT_WORK Cast()
        {
            return this.eff_work.efct_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(GMS_GMK_BOBJ_PARTS work)
        {
            return work.eff_work.efct_com.obj_work;
        }

        public GMS_GMK_BOBJ_PARTS()
        {
            this.eff_work = new GMS_EFFECT_3DNN_WORK(this);
        }
    }
}
