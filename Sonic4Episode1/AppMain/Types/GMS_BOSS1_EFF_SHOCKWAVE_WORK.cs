public partial class AppMain
{
    public class GMS_BOSS1_EFF_SHOCKWAVE_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_EFFECT_3DES_WORK eff_3des;
        public GMS_BOSS1_MGR_WORK mgr_work;
        public uint atk_rect_timer;

        public GMS_BOSS1_EFF_SHOCKWAVE_WORK()
        {
            this.eff_3des = new GMS_EFFECT_3DES_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.eff_3des.efct_com.obj_work;
        }
    }
}
