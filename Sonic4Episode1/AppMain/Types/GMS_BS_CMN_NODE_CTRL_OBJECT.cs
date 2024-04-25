public partial class AppMain
{
    public class GMS_BS_CMN_NODE_CTRL_OBJECT : IOBS_OBJECT_WORK
    {
        public readonly NNS_VECTOR user_ofst = new NNS_VECTOR();
        public readonly NNS_MATRIX w_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public object m_pHolder;
        public readonly GMS_EFFECT_COM_WORK efct_com;
        public GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work;
        public int cnm_reg_id;
        public GMS_BS_CMN_SNM_WORK snm_work;
        public int snm_reg_id;
        public NNS_QUATERNION user_quat;
        public uint user_timer;
        public int is_enable;
        public MPP_VOID_OBS_OBJECT_WORK proc_update;

        public GMS_BS_CMN_NODE_CTRL_OBJECT()
        {
            this.efct_com = new GMS_EFFECT_COM_WORK(this);
        }

        public GMS_BS_CMN_NODE_CTRL_OBJECT(object holder)
        {
            this.efct_com = new GMS_EFFECT_COM_WORK(this);
            this.m_pHolder = holder;
        }

        public static explicit operator GMS_BOSS5_SCT_PART_NDC_WORK(
          GMS_BS_CMN_NODE_CTRL_OBJECT p)
        {
            return (GMS_BOSS5_SCT_PART_NDC_WORK)p.m_pHolder;
        }

        public static explicit operator GMS_BOSS2_EFFECT_SCATTER_WORK(
          GMS_BS_CMN_NODE_CTRL_OBJECT p)
        {
            return (GMS_BOSS2_EFFECT_SCATTER_WORK)p.m_pHolder;
        }

        public static explicit operator GMS_BOSS1_EFF_SCT_PART_NDC_WORK(
          GMS_BS_CMN_NODE_CTRL_OBJECT p)
        {
            return (GMS_BOSS1_EFF_SCT_PART_NDC_WORK)p.m_pHolder;
        }

        public static explicit operator GMS_EFFECT_COM_WORK(
          GMS_BS_CMN_NODE_CTRL_OBJECT p)
        {
            return (GMS_EFFECT_COM_WORK)p.efct_com.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.efct_com.obj_work;
        }

        public static explicit operator OBS_OBJECT_WORK(
          GMS_BS_CMN_NODE_CTRL_OBJECT work)
        {
            return work.efct_com.obj_work;
        }
    }
}
