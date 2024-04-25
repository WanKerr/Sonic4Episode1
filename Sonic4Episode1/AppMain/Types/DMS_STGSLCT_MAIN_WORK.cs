using er;

public partial class AppMain
{
    private class DMS_STGSLCT_MAIN_WORK
    {
        public readonly AMS_FS[] arc_cmn_amb_fs = new AMS_FS[5];
        public readonly AMS_AMB_HEADER[] arc_cmn_amb = new AMS_AMB_HEADER[5];
        public readonly A2S_AMA_HEADER[] cmn_ama = new A2S_AMA_HEADER[5];
        public readonly AMS_AMB_HEADER[] cmn_amb = new AMS_AMB_HEADER[5];
        public readonly AOS_TEXTURE[] cmn_tex = New<AOS_TEXTURE>(5);
        public readonly AMS_FS[] arc_amb_fs = new AMS_FS[2];
        public readonly AMS_AMB_HEADER[] arc_amb = new AMS_AMB_HEADER[2];
        public readonly A2S_AMA_HEADER[] ama = new A2S_AMA_HEADER[2];
        public readonly AMS_AMB_HEADER[] amb = new AMS_AMB_HEADER[2];
        public readonly AOS_TEXTURE[] tex = New<AOS_TEXTURE>(2);
        public readonly AOS_TEXTURE win_tex = new AOS_TEXTURE();
        public readonly AOS_ACTION[] act = new AOS_ACTION[116];
        public readonly CTrgAoAction[] trg_zone = New<CTrgAoAction>(6);
        public readonly CTrgAoAction[] trg_act = New<CTrgAoAction>(7);
        public readonly CTrgAoAction[] trg_act_tab = New<CTrgAoAction>(6);
        public readonly CTrgAoAction[] trg_act_lr = New<CTrgAoAction>(2);
        public readonly CTrgFlick trg_act_move = new CTrgFlick();
        public readonly CTrgAoAction[] trg_mode = New<CTrgAoAction>(2);
        public readonly CTrgAoAction trg_cancel = new CTrgAoAction();
        public readonly CTrgAoAction[] trg_answer = New<CTrgAoAction>(2);
        public readonly int[] n_sonic_hi_score = new int[17];
        public readonly int[] s_sonic_hi_score = new int[17];
        public readonly int[] n_sonic_record_time = new int[17];
        public readonly int[] s_sonic_record_time = new int[17];
        public readonly int[] hi_score = new int[24];
        public readonly int[] record_time = new int[24];
        public readonly int[] is_clear_stage = new int[24];
        public readonly uint[] eme_stage_no = new uint[7];
        public readonly float[,] win_act_pos = new float[13, 2];
        public readonly float[] win_size_rate = new float[2];
        public readonly float[][] zone_pos = New<float>(6, 2);
        public readonly float[] move_spd = new float[2];
        public readonly float[] act_top_pos_x = new float[24];
        public readonly float[] act_top_pos_y = new float[24];
        public readonly float[] act_move_src = new float[2];
        public readonly float[] act_move_dest = new float[2];
        public readonly float[] act_move_pos_src = new float[24];
        public readonly float[] act_move_pos_dst = new float[24];
        public readonly float[] act_tab_state_move_base_pos = new float[2];
        public readonly float[] obi_pos = new float[2];
        public readonly AMS_PARAM_DRAW_PRIMITIVE up_bg_vrtx = new AMS_PARAM_DRAW_PRIMITIVE();
        public readonly float[] tex_u = new float[2];
        public readonly float[] tex_v = new float[2];
        public AMS_FS win_amb_fs;
        public AMS_AMB_HEADER win_amb;
        public _proc_win_input_ proc_win_input;
        public _proc_input_ proc_input;
        public _proc_win_update_ proc_win_update;
        public _proc_menu_update_ proc_menu_update;
        public _proc_draw_ proc_draw;
        public int timer;
        public uint flag;
        public int state;
        public float win_timer;
        public uint disp_flag;
        public int bg_timer;
        public int zone_scr_timer;
        public uint announce_flag;
        public int next_evt;
        public int prev_evt;
        public int is_final_open;
        public uint get_emerald;
        public uint cur_game_mode;
        public uint player_stock;
        public int win_mode;
        public int win_cur_slct;
        public bool win_is_disp_cover;
        public uint cur_zone;
        public uint chng_zone;
        public uint efct_time;
        public uint efct_out_flag;
        public float chaos_eme_pos_y;
        public float mode_tex_pos_y;
        public float mode_tex_frm;
        public int cur_stage;
        public int prev_stage;
        public int cur_vrtcl_stage;
        public int prev_vrtcl_stage;
        public int crsr_idx;
        public int crsr_prev_idx;
        public float crsr_pos_y;
        public float crsr_move_src;
        public float crsr_move_dst;
        public int focus_disp_no;
        public int prev_disp_no;
        public bool is_disp_cover;
        public int decide_zone_efct_dist_x;
        public int decide_zone_efct_dist_y;
        public AOS_ACT_COL bg_fade;
        public uint cur_bg_id;
        public uint next_bg_id;
        public uint zone_scr_id;
        public uint mode_tex_move_frm;
        public uint btn_l_disp_frm;
        public uint btn_r_disp_frm;
        public bool is_jp_region;

        public delegate void _proc_win_input_(DMS_STGSLCT_MAIN_WORK work);

        public delegate void _proc_input_(DMS_STGSLCT_MAIN_WORK work);

        public delegate void _proc_win_update_(DMS_STGSLCT_MAIN_WORK work);

        public delegate void _proc_menu_update_(DMS_STGSLCT_MAIN_WORK work);

        public delegate void _proc_draw_(DMS_STGSLCT_MAIN_WORK work);
    }
}
