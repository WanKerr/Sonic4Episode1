using er;

public partial class AppMain
{
    public class DMS_MANUAL_MAIN_WORK
    {
        public A2S_AMA_HEADER[] ama = new A2S_AMA_HEADER[2];
        public AMS_AMB_HEADER[] amb = new AMS_AMB_HEADER[2];
        public AOS_ACTION[] act = new AOS_ACTION[179];
        public CTrgAoAction trg_return = new CTrgAoAction();
        public AMS_FS arc_amb;
        public AOS_TEXTURE[] tex;
        public _proc_input_ proc_input;
        public _proc_update_ proc_update;
        public _proc_draw_ proc_draw;
        public float timer;
        public uint flag;
        public float efct_timer;
        public int cur_disp_page;
        public int cur_disp_page_prev;
        public bool is_jp_region;
        public bool is_maingame_load;
        public uint draw_state;
        public GSS_SND_SE_HANDLE se_handle;
        public CTrgAoAction[] trg_btn;

        public DMS_MANUAL_MAIN_WORK()
        {
            this.tex = New<AOS_TEXTURE>(2);
            this.trg_btn = New<CTrgAoAction>(2);
        }

        public delegate void _proc_input_(DMS_MANUAL_MAIN_WORK work);

        public delegate void _proc_update_(DMS_MANUAL_MAIN_WORK work);

        public delegate void _proc_draw_(DMS_MANUAL_MAIN_WORK work);
    }
}
