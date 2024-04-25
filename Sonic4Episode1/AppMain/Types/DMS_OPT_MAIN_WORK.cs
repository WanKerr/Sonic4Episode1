using er;

public partial class AppMain
{
    public class DMS_OPT_MAIN_WORK
    {
        public AMS_FS[] arc_cmn_amb_fs = new AMS_FS[5];
        public AMS_AMB_HEADER[] arc_cmn_amb = new AMS_AMB_HEADER[5];
        public A2S_AMA_HEADER[] cmn_ama = new A2S_AMA_HEADER[5];
        public AMS_AMB_HEADER[] cmn_amb = new AMS_AMB_HEADER[5];
        public AMS_FS[] arc_amb_fs = new AMS_FS[2];
        public AMS_FS[] user_arc_amb_fs = new AMS_FS[2];
        public AMS_FS[] manual_arc_amb_fs = new AMS_FS[2];
        public AMS_AMB_HEADER[] arc_amb = new AMS_AMB_HEADER[2];
        public AMS_AMB_HEADER[] user_arc_amb = new AMS_AMB_HEADER[2];
        public AMS_AMB_HEADER[] manual_arc_amb = new AMS_AMB_HEADER[2];
        public A2S_AMA_HEADER[] ama = new A2S_AMA_HEADER[2];
        public AMS_AMB_HEADER[] amb = new AMS_AMB_HEADER[2];
        public AOS_TEXTURE win_tex = new AOS_TEXTURE();
        public AOS_ACTION[] act = new AOS_ACTION[102];
        public float[] push_efct_timer = new float[4];
        public int[] volume_data = new int[2];
        public CTrgAoAction[] trg_se_btn = new CTrgAoAction[2];
        public CTrgAoAction[] trg_ctrl_btn = new CTrgAoAction[2];
        public CTrgAoAction[] ctrl_win_trg_btn = new CTrgAoAction[2];
        public float[] ctrl_tab_pos_x = new float[2];
        public float[] ctrl_tab_pos_y = new float[2];
        public float[][] ctrl_move_src = New<float>(2, 2);
        public float[][] ctrl_move_dest = New<float>(2, 2);
        public float[] obi_tex_pos = new float[2];
        public float[] win_size_rate = new float[2];
        public AOS_ACT_COL decide_menu_col = new AOS_ACT_COL();
        public AOS_ACT_COL vol_icon_col = new AOS_ACT_COL();
        public AOS_ACT_COL win_col = new AOS_ACT_COL();
        public AOS_TEXTURE[] cmn_tex;
        public AMS_FS[] win_amb_fs;
        public AMS_AMB_HEADER[] win_amb;
        public AOS_TEXTURE[] tex;
        public AOS_ACTION bg_icon_node;
        public _proc_input_ proc_input;
        public _proc_update_ proc_update;
        public _proc_draw_ proc_draw;
        public _proc_menu_draw_ proc_menu_draw;
        public float frm_update_time;
        public float timer;
        public float efct_timer;
        public float win_timer;
        public float vib_timer;
        public uint flag;
        public int state;
        public uint disp_flag;
        public int next_evt;
        public int prev_evt;
        public CTrgAoAction[] trg_slct;
        public CTrgAoAction trg_return;
        public int ctrl_mode;
        public int prev_ctrl_mode;
        public int set_vbrt;
        public CTrgAoAction[] trg_bgm_btn;
        public CTrgRect trg_bgm_slider;
        public CTrgRect trg_se_slider;
        public float ctrl_win_window_prgrs;
        public int cur_slct_top;
        public int cur_slct_set;
        public float top_crsr_pos_y;
        public float src_crsr_pos_y;
        public float dst_crsr_pos_y;
        public float obi_pos_y;
        public float set_icon_dir;
        public float set_icon_shdw_dir;
        public uint draw_state;
        public int nrml_disp_type;
        public int prev_nrml_disp_type;
        public bool is_jp_region;
        public GSS_SND_SCB bgm_scb;
        public GSS_SND_SE_HANDLE se_handle;

        public DMS_OPT_MAIN_WORK()
        {
            this.cmn_tex = New<AOS_TEXTURE>(5);
            this.tex = New<AOS_TEXTURE>(2);
            this.trg_slct = New<CTrgAoAction>(4);
            this.trg_bgm_btn = New<CTrgAoAction>(2);
            this.trg_se_btn = New<CTrgAoAction>(2);
            this.trg_ctrl_btn = New<CTrgAoAction>(2);
            this.ctrl_win_trg_btn = New<CTrgAoAction>(2);
            this.trg_bgm_slider = new CTrgRect();
            this.trg_se_slider = new CTrgRect();
            this.trg_return = new CTrgAoAction();
        }

        public delegate void _proc_input_(DMS_OPT_MAIN_WORK work);

        public delegate void _proc_update_(DMS_OPT_MAIN_WORK work);

        public delegate void _proc_draw_(DMS_OPT_MAIN_WORK work);

        public delegate void _proc_menu_draw_(DMS_OPT_MAIN_WORK work);
    }
}
