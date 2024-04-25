public partial class AppMain
{
    public class GMS_GMK_PRESSWALL_PARTS : IOBS_OBJECT_WORK
    {
        public readonly GMS_EFFECT_3DNN_WORK eff_work;
        public int ofst_y;
        public int master_posy;

        public GMS_GMK_PRESSWALL_PARTS()
        {
            this.eff_work = new GMS_EFFECT_3DNN_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.eff_work.efct_com.obj_work;
        }
    }
}
