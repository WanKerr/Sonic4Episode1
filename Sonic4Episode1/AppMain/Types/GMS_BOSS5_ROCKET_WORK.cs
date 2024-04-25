public partial class AppMain
{
    public class GMS_BOSS5_ROCKET_WORK : IOBS_OBJECT_WORK
    {
        public VecFx32 launch_pos = new VecFx32();
        public VecFx32 dest_pos = new VecFx32();
        public VecFx32 rvs_acc = new VecFx32();
        public NNS_QUATERNION sct_cur_quat = new NNS_QUATERNION();
        public NNS_QUATERNION sct_spin_quat = new NNS_QUATERNION();
        public readonly GMS_BS_CMN_BMCB_MGR bmcb_mgr = new GMS_BS_CMN_BMCB_MGR();
        public readonly GMS_BS_CMN_SNM_WORK snm_work = new GMS_BS_CMN_SNM_WORK();
        public VecFx32 pivot_prev_pos = new VecFx32();
        public NNS_QUATERNION stuck_lerp_src_quat = new NNS_QUATERNION();
        public readonly GMS_BS_CMN_DELAY_SEARCH_WORK dsearch_work = new GMS_BS_CMN_DELAY_SEARCH_WORK();
        public VecFx32[] search_hist_buf = New<VecFx32>(21U);
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public uint flag;
        public int rkt_type;
        public uint wait_timer;
        public uint hit_count;
        public uint no_hit_timer;
        public uint wfall_atk_toggle_timer;
        public MPP_VOID_GMS_BOSS5_ROCKET_WORK proc_update;
        public int move_dir;
        public int acc;
        public int max_spd;
        public int rot_spd;
        public int stuck_dir;
        public float stuck_lean_ratio;
        public float hit_vib_amp_deg;
        public int hit_vib_sin_angle;
        public int pivot_fall_angle;
        public int wobble_sin_param_angle;
        public int arm_snm_id;
        public int drill_snm_reg_id;
        public float stuck_lerp_ratio;
        public float stuck_lerp_ratio_spd;
        public int ply_search_delay;

        public GMS_BOSS5_ROCKET_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator GMS_ENEMY_COM_WORK(
          GMS_BOSS5_ROCKET_WORK p)
        {
            return p.ene_3d.ene_com;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
