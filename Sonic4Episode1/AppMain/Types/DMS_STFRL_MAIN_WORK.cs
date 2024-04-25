public partial class AppMain
{
    public class DMS_STFRL_MAIN_WORK
    {
        public readonly DMS_STFRL_DATA_MGR arc_data = new DMS_STFRL_DATA_MGR();
        public readonly AMS_AMB_HEADER[] arc_cmn_amb_fs = new AMS_AMB_HEADER[2];
        public readonly AOS_TEXTURE font_tex = new AOS_TEXTURE();
        public readonly AOS_TEXTURE[] scr_tex = New<AOS_TEXTURE>(3);
        public readonly AOS_TEXTURE end_tex = new AOS_TEXTURE();
        public readonly AOS_TEXTURE end_jp_tex = new AOS_TEXTURE();
        public readonly A2S_AMA_HEADER[] cmn_ama = new A2S_AMA_HEADER[2];
        public readonly AMS_AMB_HEADER[] cmn_amb = new AMS_AMB_HEADER[2];
        public readonly AOS_TEXTURE[] cmn_tex = New<AOS_TEXTURE>(2);
        public readonly AOS_ACTION[] act = new AOS_ACTION[11];
        public readonly float[] win_size_rate = new float[2];
        public readonly bool[] data_disp_yet = new bool[3];
        public readonly bool[] check_file_load = new bool[3];
        public readonly DMS_STFRL_RING_WORK[] ring_work = new DMS_STFRL_RING_WORK[3];
        public AMS_AMB_HEADER arc_list_font_amb;
        public AMS_AMB_HEADER arc_scr_amb_fs;
        public AMS_AMB_HEADER arc_end_amb_fs;
        public AMS_AMB_HEADER arc_end_jp_amb_fs;
        public AMS_FS arc_list_font_amb_fs;
        public object staff_list_fs;
        public A2S_AMA_HEADER end_ama;
        public AMS_AMB_HEADER end_amb;
        public A2S_AMA_HEADER end_jp_ama;
        public AMS_AMB_HEADER end_jp_amb;
        public object stf_list_ysd;
        public _proc_input_ proc_input;
        public _proc_update_ proc_update;
        public _proc_data_load_ proc_data_load;
        public _proc_draw_ proc_draw;
        public float timer;
        public uint flag;
        public float efct_timer;
        public float fade_timer;
        public float win_timer;
        public float disp_frm_time;
        public uint disp_mode;
        public bool is_eme_comp;
        public uint win_mode;
        public uint announce_flag;
        public float sonic_set_frame;
        public float list_disp_pos_x;
        public AOS_ACT_COL list_col;
        public float sonic_move_spd;
        public int end_act_frm;
        public int continue_act_frm;
        public int load_data_num;
        public bool is_full_staffroll;
        public uint draw_state;
        public uint cur_disp_scr_id;
        public uint disp_list_page_num;
        public uint cur_disp_list_page;
        public uint prev_disp_list_page;
        public uint disp_page_time;
        public uint cur_disp_image;
        public uint[] page_line_type;
        public uint cur_page_list_alpha_data;
        public uint cur_page_scr_alpha_data;
        public AOS_ACT_COL question_act_alpha;
        public DMS_STFRL_SONIC_WORK sonic_work;
        public DMS_STFRL_BOSS_BODY_WORK body_work;
        public DMS_STFRL_BOSS_EGG_WORK egg_work;
        public GSS_SND_SCB bgm_scb;
        public GSS_SND_SE_HANDLE se_handle;

        public delegate void _proc_input_(DMS_STFRL_MAIN_WORK work);

        public delegate void _proc_update_(DMS_STFRL_MAIN_WORK work);

        public delegate void _proc_data_load_(DMS_STFRL_MAIN_WORK work);

        public delegate void _proc_draw_(DMS_STFRL_MAIN_WORK work);
    }
}
