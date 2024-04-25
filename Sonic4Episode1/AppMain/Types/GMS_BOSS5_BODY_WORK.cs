public partial class AppMain
{
    public class GMS_BOSS5_BODY_WORK : IOBS_OBJECT_WORK
    {
        public OBS_RECT_WORK[][] sub_rect_work = New<OBS_RECT_WORK>(2, 3);
        public readonly GMS_BS_CMN_BMCB_MGR bmcb_mgr = new GMS_BS_CMN_BMCB_MGR();
        public readonly GMS_BS_CMN_SNM_WORK snm_work = new GMS_BS_CMN_SNM_WORK();
        public int[][] armpt_snm_reg_ids = New<int>(2, 3);
        public int[] leg_snm_reg_ids = New<int>(2);
        public int[] groin_snm_reg_ids = New<int>(2);
        public int[] nozzle_snm_reg_ids = New<int>(2);
        public VecFx32 pivot_prev_pos = new VecFx32();
        public readonly GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work = new GMS_BS_CMN_CNM_MGR_WORK();
        public int[][] arm_cnm_reg_id = New<int>(2, 3);
        public int[] scatter_cnm_reg_ids = New<int>(22);
        public readonly NNS_MATRIX[] rkt_ofst_mtx = New<NNS_MATRIX>(2);
        public readonly NNS_QUATERNION[][] arm_part_rot_quat = New<NNS_QUATERNION>(2, 3);
        public readonly GMS_BOSS5_ARM_ANIM_WORK arm_anim_work = new GMS_BOSS5_ARM_ANIM_WORK();
        public NNS_QUATERNION cnpy_close_init_quat = new NNS_QUATERNION();
        public NNS_QUATERNION cnpy_close_dest_quat = new NNS_QUATERNION();
        public readonly GMS_BS_CMN_DMG_FLICKER_WORK flk_work = new GMS_BS_CMN_DMG_FLICKER_WORK();
        public int[] foot_ofst_record_src = new int[2];
        public int[] foot_ofst_record_dest = new int[2];
        public VecFx32 grdmv_pivot_pos = new VecFx32();
        public readonly GMS_BOSS5_GRD_MOVE_WORK grdmv_work = new GMS_BOSS5_GRD_MOVE_WORK();
        public readonly GMS_BOSS5_1SHOT_TIMER se_timer = new GMS_BOSS5_1SHOT_TIMER();
        public readonly GMS_BOSS5_1SHOT_TIMER targ_se_timer = new GMS_BOSS5_1SHOT_TIMER();
        public readonly GMS_BS_CMN_DELAY_SEARCH_WORK dsearch_work = new GMS_BS_CMN_DELAY_SEARCH_WORK();
        public VecFx32[] search_hist_buf = New<VecFx32>(11);
        public readonly GMS_BOSS5_EXPL_WORK expl_work = new GMS_BOSS5_EXPL_WORK();
        public OBS_OBJECT_WORK[] parts_objs = new OBS_OBJECT_WORK[1];
        public readonly GMS_ENEMY_3D_WORK ene_3d;
        public int state;
        public int prev_state;
        public int sub_seq;
        public int strat_state;
        public GMS_BOSS5_MGR_WORK mgr_work;
        public MPP_VOID_GMS_BOSS5_BODY_WORK proc_update;
        public uint flag;
        public int whole_act_id;
        public uint wait_timer;
        public uint no_hit_timer;
        public uint fast_move_timer;
        public uint poke_trg_limit_timer;
        public int ground_v_pos;
        public int crash_pos_ofst_x;
        public uint def_rect_req_flag;
        public int body_snm_reg_id;
        public int lfoot_snm_reg_id;
        public int rfoot_snm_reg_id;
        public int pole_snm_reg_id;
        public int pole_cnm_reg_id;
        public int cover_cnm_reg_id;
        public int neck_cnm_reg_id;
        public int head_cnm_reg_id;
        public int arm_poke_anim_phase;
        public float cnpy_close_ratio;
        public float cnpy_close_ratio_spd;
        public int adj_hgap_is_active;
        public int adj_hgap_act_id;
        public int adj_hgap_leg_type;
        public int cur_move_phase_type;
        public int is_move_reverse;
        public int walk_end_monitor_phase_cnt;
        public int is_player_behind;
        public int cur_walk_grnd_phase_cnt;
        public int run_grnd_runtype;
        public uint run_grnd_delay_timer;
        public uint run_grnd_spawn_remain;
        public int cur_run_type;
        public uint se_cnt;
        public GSS_SND_SE_HANDLE se_hnd_leakage;
        public float targ_se_cur_interval;
        public int ply_search_delay;
        public int turn_src_dir;
        public int turn_tgt_ofst_dir;
        public float turn_ratio;
        public float bsk_shake_acc_ratio;
        public float bsk_shake_acc_ratio_spd;
        public float bsk_shake_init_spd;
        public uint crash_strike_vib_delay_timer;
        public int crash_strike_vib_phase;
        public float crash_strike_vib_ratio;
        public uint start_rise_vib_int_timer;
        public uint sct_land_vib_timer;
        public OBS_OBJECT_WORK part_obj_core;

        public GMS_BOSS5_BODY_WORK()
        {
            this.ene_3d = new GMS_ENEMY_3D_WORK(this);
        }

        public static explicit operator GMS_ENEMY_COM_WORK(GMS_BOSS5_BODY_WORK p)
        {
            return p.ene_3d.ene_com;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
