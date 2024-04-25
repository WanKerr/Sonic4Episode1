public partial class AppMain
{
    public class DMS_SAVE_MAIN_WORK
    {
        public readonly AMS_FS[] arc_cmn_amb_fs = new AMS_FS[2];
        public readonly AMS_AMB_HEADER[] arc_cmn_amb = new AMS_AMB_HEADER[2];
        public readonly A2S_AMA_HEADER[] cmn_ama = new A2S_AMA_HEADER[2];
        public readonly AMS_AMB_HEADER[] cmn_amb = new AMS_AMB_HEADER[2];
        public readonly AOS_TEXTURE[] cmn_tex = New<AOS_TEXTURE>(2);
        public readonly AOS_ACTION[] act = new AOS_ACTION[6];
        public readonly float[][] win_act_pos = New<float>(5, 2);
        public readonly float[] win_size_rate = new float[2];
        public _saveproc_input_update proc_input;
        public _saveproc_input_update proc_menu_update;
        public _saveproc_draw proc_draw;
        public uint flag;
        public uint announce_flag;
        public uint disp_flag;
        public int state;
        public int timer;
        public int win_timer;
        public int win_mode;
        public int win_cur_slct;
        public uint draw_state;
        public GSS_SND_SCB bgm_scb;
    }
}
