public partial class AppMain
{
    public class GMS_BOSS2_EFFECT_SCATTER_WORK : IOBS_OBJECT_WORK
    {
        public NNS_QUATERNION spin_quat = new NNS_QUATERNION();
        public readonly GMS_BS_CMN_NODE_CTRL_OBJECT control_node_work;

        public GMS_BOSS2_EFFECT_SCATTER_WORK()
        {
            this.control_node_work = new GMS_BS_CMN_NODE_CTRL_OBJECT(this);
        }

        public static explicit operator GMS_BS_CMN_NODE_CTRL_OBJECT(
          GMS_BOSS2_EFFECT_SCATTER_WORK p)
        {
            return p.control_node_work;
        }

        public static explicit operator OBS_OBJECT_WORK(
          GMS_BOSS2_EFFECT_SCATTER_WORK p)
        {
            return p.control_node_work.efct_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.control_node_work.efct_com.obj_work;
        }
    }
}
