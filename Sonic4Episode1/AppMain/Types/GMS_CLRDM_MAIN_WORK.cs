using er;

public partial class AppMain
{
    private class GMS_CLRDM_MAIN_WORK
    {
        public readonly AMS_FS[] ama_fs = new AMS_FS[2];
        public readonly AMS_FS[] amb_fs = new AMS_FS[2];
        public readonly A2S_AMA_HEADER[] ama = new A2S_AMA_HEADER[2];
        public readonly AMS_AMB_HEADER[] amb = new AMS_AMB_HEADER[2];
        public readonly AOS_TEXTURE[] tex = New<AOS_TEXTURE>(2);
        public readonly GMS_COCKPIT_2D_WORK[] tex_up_act = new GMS_COCKPIT_2D_WORK[5];
        public readonly GMS_COCKPIT_2D_WORK[] time_num_act = new GMS_COCKPIT_2D_WORK[5];
        public readonly GMS_COCKPIT_2D_WORK[] ring_num_act = new GMS_COCKPIT_2D_WORK[5];
        public readonly GMS_COCKPIT_2D_WORK[] total_num_act = new GMS_COCKPIT_2D_WORK[9];
        public readonly GMS_COCKPIT_2D_WORK[] line_act = new GMS_COCKPIT_2D_WORK[3];
        public readonly GMS_COCKPIT_2D_WORK[] record_time_num_act = new GMS_COCKPIT_2D_WORK[7];
        public readonly CTrgAoAction trg_retry = new CTrgAoAction();
        public readonly GMS_COCKPIT_2D_WORK[] btn_retry = new GMS_COCKPIT_2D_WORK[3];
        public readonly CTrgAoAction trg_back = new CTrgAoAction();
        public readonly GMS_COCKPIT_2D_WORK[] btn_back = new GMS_COCKPIT_2D_WORK[3];
        public readonly GMS_COCKPIT_2D_WORK[] tex_spst_up_act = new GMS_COCKPIT_2D_WORK[3];
        public readonly GMS_COCKPIT_2D_WORK[] spst_num_act = new GMS_COCKPIT_2D_WORK[7];
        public readonly GMS_COCKPIT_2D_WORK[] icon_emer_up_act = new GMS_COCKPIT_2D_WORK[7];
        public readonly GMS_COCKPIT_2D_WORK[] icon_emer_down_act = new GMS_COCKPIT_2D_WORK[7];
        public readonly uint[] time_score = new uint[2];
        public readonly uint[] ring_score = new uint[2];
        public readonly uint[] total_score = new uint[2];
        public GMS_COCKPIT_2D_WORK tex_time_act;
        public GMS_COCKPIT_2D_WORK tex_ring_act;
        public GMS_COCKPIT_2D_WORK tex_total_act;
        public GMS_COCKPIT_2D_WORK sonic_icon_act;
        public GMS_COCKPIT_2D_WORK sonic_icon_act2;
        public GMS_COCKPIT_2D_WORK tex_big_time_act;
        public GMS_COCKPIT_2D_WORK time_sonic_icon_act;
        public GMS_COCKPIT_2D_WORK tex_new_record_act;
        public GMS_COCKPIT_2D_WORK tex_retry_act;
        public GMS_COCKPIT_2D_WORK tex_back_slct_act;
        public GMS_COCKPIT_2D_WORK bg_retry;
        public GMS_COCKPIT_2D_WORK icon_emer_light_act;
        public GMS_COCKPIT_2D_WORK tex_extend_act;
        public _WorkDelegate proc_input;
        public _WorkDelegate proc_update;
        public _WorkDelegate proc_calc_score;
        public uint clear_time;
        public ushort time_min;
        public ushort time_sec;
        public ushort time_msec;
        public float timer;
        public float flash_timer;
        public uint flag;
        public int idle_time;
        public int count;
        public int game_mode;
        public bool is_clear_spe_stg;
        public bool is_full_eme;
        public bool is_get_eme;
        public int has_eme_num;
        public int get_eme_no;
        public bool is_first_spe_clear;
        public int next_evt;
        public uint cur_retry_slct;
        public ushort stage_id;
        public ushort prev_spe_stage_id;
        public bool nodisp_check;

        public delegate void _WorkDelegate(GMS_CLRDM_MAIN_WORK pArg);
    }
}
