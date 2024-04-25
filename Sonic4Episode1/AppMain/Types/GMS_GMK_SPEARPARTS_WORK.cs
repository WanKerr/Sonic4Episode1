public partial class AppMain
{
    public class GMS_GMK_SPEARPARTS_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_EFFECT_3DNN_WORK eff_work;
        public uint obj_type;
        public int fulcrum;
        public int connect;
        public OBS_OBJECT_WORK parent_connect;
        public uint connect_type;

        public GMS_GMK_SPEARPARTS_WORK()
        {
            this.eff_work = new GMS_EFFECT_3DNN_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.eff_work.efct_com.obj_work;
        }
    }
}
