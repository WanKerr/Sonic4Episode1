public partial class AppMain
{
    public class GMS_PLAYER_WORK : IOBS_OBJECT_WORK
    {
        public readonly OBS_ACTION3D_NN_WORK[] obj_3d = new OBS_ACTION3D_NN_WORK[4];
        public readonly OBS_ACTION3D_NN_WORK[] obj_3d_work = New<OBS_ACTION3D_NN_WORK>(8);
        public readonly OBS_RECT_WORK[] rect_work = New<OBS_RECT_WORK>(3);
        public readonly NNS_MATRIX ex_obj_mtx_r = GlobalPool<NNS_MATRIX>.Alloc();
        public VecFx32 boost_pos1 = new VecFx32();
        public VecFx32 boost_pos2 = new VecFx32();
        public readonly NNS_MATRIX truck_mtx_ply_mtn_pos = GlobalPool<NNS_MATRIX>.Alloc();
        public readonly NNS_VECTOR calc_accel = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly ushort[] key_map = new ushort[8];
        public readonly int[] key_repeat_timer = new int[8];
        public readonly GMS_PLAYER_PACKET[] player_packet = New<GMS_PLAYER_PACKET>(4);
        public readonly OBS_OBJECT_WORK obj_work;
        public byte char_id;
        public byte player_id;
        public byte ctrl_id;
        public byte camera_no;
        public int spin_state;
        public int act_state;
        public int prev_act_state;
        public int seq_state;
        public int prev_seq_state;
        public int timer;
        public uint player_flag;
        public uint gmk_flag;
        public uint gmk_flag2;
        public int dash_power;
        public int prev_walk_roll_spd_max;
        public seq_func_delegate seq_func;
        public seq_func_delegate[] seq_init_tbl;
        public GMS_PLY_SEQ_STATE_DATA[] seq_state_data_tbl;
        public short spin_se_timer;
        public short spin_back_se_timer;
        public short tension;
        public short over_limit_spd;
        public int no_spddown_timer;
        public int spd_add;
        public int spd_max;
        public int spd_dec;
        public int spd_spin;
        public int spd_add_spin;
        public int spd_max_spin;
        public int spd_dec_spin;
        public int spd_max_boost;
        public int spd_add_nitro;
        public int spd_max_nitro;
        public int spd_dec_nitro;
        public int spd_chk_nitro;
        public int spd_max_add_slope;
        public int spd_jump;
        public int spd_work_max;
        public int spd_jump_add;
        public int spd_jump_max;
        public int spd_jump_dec;
        public int spd_add_spin_pinball;
        public int spd_max_spin_pinball;
        public int spd_dec_spin_pinball;
        public int spd_max_add_slope_spin_pinball;
        public int time_air;
        public int time_damage;
        public int spd1;
        public int spd2;
        public int spd3;
        public int spd4;
        public int spd5;
        public short spd_pool;
        public short ring_num;
        public short ring_stage_num;
        public uint score;
        public int invincible_timer;
        public int genocide_timer;
        public int pressure_timer;
        public int disapprove_item_catch_timer;
        public int water_timer;
        public int no_key_timer;
        public int homing_timer;
        public int hi_speed_timer;
        public int homing_boost_timer;
        public int fall_timer;
        public int no_jump_move_timer;
        public int maxdash_timer;
        public int super_sonic_ring_timer;
        public int camera_stop_timer;
        public int camera_ofst_x;
        public int camera_ofst_y;
        public int camera_ofst_tag_x;
        public int camera_ofst_tag_y;
        public int camera_jump_pos_y;
        public OBS_OBJECT_WORK enemy_obj;
        public OBS_OBJECT_WORK cursol_enemy_obj;
        public ushort pgm_turn_dir;
        public ushort pgm_turn_spd;
        public ushort[] pgm_turn_dir_tbl;
        public int pgm_turn_tbl_cnt;
        public int pgm_turn_tbl_num;
        public int fall_act_state;
        public int scroll_spd_x;
        public uint score_combo_cnt;
        public OBS_OBJECT_WORK gmk_obj;
        public short gmk_camera_ofst_x;
        public short gmk_camera_ofst_y;
        public short gmk_camera_center_ofst_x;
        public short gmk_camera_center_ofst_y;
        public short gmk_camera_gmk_center_ofst_x;
        public short gmk_camera_gmk_center_ofst_y;
        public short gmk_map_limit_left;
        public short gmk_map_limit_right;
        public short gmk_map_limit_top;
        public short gmk_map_limit_bottom;
        public int gmk_work0;
        public int gmk_work1;
        public int gmk_work2;
        public int gmk_work3;
        public object opt_anime;
        public ushort prev_dir_fall;
        public ushort prev_dir_fall2;
        public int dir_fall_fix_timer;
        public int ply_pseudofall_dir;
        public ushort jump_pseudofall_dir;
        public ushort jump_pseudofall_eve_id_set;
        public ushort jump_pseudofall_eve_id_cur;
        public ushort jump_pseudofall_eve_id_wait;
        public int truck_left_flip_timer;
        public OBS_OBJECT_WORK truck_obj;
        public ushort truck_prev_dir;
        public ushort truck_prev_dir_fall;
        public ushort truck_stick_prev_dir;
        public OBS_OBJECT_WORK efct_spin_jump_blur;
        public OBS_OBJECT_WORK efct_spin_dash_blur;
        public OBS_OBJECT_WORK efct_spin_dash_cir_blur;
        public OBS_OBJECT_WORK efct_spin_start_blur;
        public OBS_OBJECT_WORK efct_run_spray;
        public float light_rate;
        public int light_anm_flag;
        public short speed_curse;
        public short prev_speed_curse;
        public int warp_pos_x;
        public int warp_pos_y;
        public byte graind_id;
        public byte graind_prev_ride;
        public short nudge_di_timer;
        public short nudge_timer;
        public int nudge_ofst_x;
        public bool is_nudge;
        public ushort key_on;
        public ushort key_push;
        public ushort key_repeat;
        public ushort key_release;
        public int key_rot_z;
        public int key_walk_rot_z;
        public int prev_key_rot_z;
        public int accel_counter;
        public int dir_vec_add;
        public int control_type;
        public ushort[] jump_rect;
        public ushort[] ssonic_rect;
        public int safe_timer;
        public int safe_jump_timer;
        public int safe_spin_timer;
        public uint net_ref_atk_time;
        public int packet_camera_pos_x;
        public int packet_camera_pos_y;
        public short use_packet_buf_no;
        public short key_z_rot;
        public GSS_SND_SE_HANDLE spinHandle = GsSoundAllocSeHandle();

        public static explicit operator OBS_OBJECT_WORK(GMS_PLAYER_WORK work)
        {
            return work?.obj_work;
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public GMS_PLAYER_WORK()
        {
            this.obj_work = OBS_OBJECT_WORK.Create(this);
        }
    }
}
