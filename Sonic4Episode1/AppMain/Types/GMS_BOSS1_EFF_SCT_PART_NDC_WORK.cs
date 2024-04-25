public partial class AppMain
{
    public class GMS_BOSS1_EFF_SCT_PART_NDC_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_BS_CMN_NODE_CTRL_OBJECT ncd_obj = new GMS_BS_CMN_NODE_CTRL_OBJECT();
        public NNS_QUATERNION spin_quat;
        public bool is_ironball;

        public GMS_BOSS1_EFF_SCT_PART_NDC_WORK()
        {
            this.ncd_obj = new GMS_BS_CMN_NODE_CTRL_OBJECT(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ncd_obj.efct_com.obj_work;
        }

        public static explicit operator GMS_BS_CMN_NODE_CTRL_OBJECT(
          GMS_BOSS1_EFF_SCT_PART_NDC_WORK work)
        {
            return work.ncd_obj;
        }
    }
}
