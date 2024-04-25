public partial class AppMain
{
    public class DMS_LOADING_MAIN_WORK
    {
        public AOS_ACTION[] act = new AOS_ACTION[8];
        public AMS_FS arc_amb;
        public byte[] ama;
        public byte[] amb;
        public AOS_TEXTURE tex;
        public _proc_update_ proc_update;
        public _proc_draw_ proc_draw;
        public float timer;
        public uint flag;
        public float efct_timer;
        public float sonic_set_frame;
        public float sonic_pos_x;
        public float sonic_move_spd;
        public bool is_maingame_load;
        public bool is_play_maingame;
        public uint draw_state;
        public int lang_id;

        public delegate void _proc_update_(DMS_LOADING_MAIN_WORK work);

        public delegate void _proc_draw_(DMS_LOADING_MAIN_WORK work);
    }
}
