public partial class AppMain
{
    public class GMS_BOSS1_CHAIN_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_BS_CMN_BMCB_MGR bmcb_mgr = new GMS_BS_CMN_BMCB_MGR();
        public readonly GMS_BS_CMN_SNM_WORK snm_work = new GMS_BS_CMN_SNM_WORK();
        public readonly int[] sct_snm_reg_ids = new int[9];
        public readonly GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work = new GMS_BS_CMN_CNM_MGR_WORK();
        public readonly int[] sct_cnm_reg_ids = new int[9];
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public GMS_BOSS1_MGR_WORK mgr_work;
        public uint flag;
        public MPP_VOID_GMS_BOSS1_CHAIN_WORK proc_update;
        public int ball_snm_reg_id;

        public GMS_BOSS1_CHAIN_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
