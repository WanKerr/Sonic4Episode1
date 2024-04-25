public partial class AppMain
{
    public class GMS_BOSS5_SCT_PART_NDC_WORK : IOBS_OBJECT_WORK
    {
        public NNS_QUATERNION spin_quat = new NNS_QUATERNION();
        public readonly GMS_BS_CMN_NODE_CTRL_OBJECT ndc_obj;

        public GMS_BOSS5_SCT_PART_NDC_WORK()
        {
            this.ndc_obj = new GMS_BS_CMN_NODE_CTRL_OBJECT(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ndc_obj.Cast();
        }
    }
}
