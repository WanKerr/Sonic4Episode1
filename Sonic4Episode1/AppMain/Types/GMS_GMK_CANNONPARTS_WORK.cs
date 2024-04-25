public partial class AppMain
{
    public class GMS_GMK_CANNONPARTS_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_EFFECT_3DNN_WORK eff_work;

        public GMS_GMK_CANNONPARTS_WORK()
        {
            this.eff_work = new GMS_EFFECT_3DNN_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.eff_work.efct_com.obj_work;
        }
    }
}
