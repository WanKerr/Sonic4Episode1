using System;

public partial class AppMain
{
    public class DMS_STFRL_DATA_MGR
    {
        public readonly AMS_AMB_HEADER[] arc_cmn_amb = new AMS_AMB_HEADER[2];
        public AMS_AMB_HEADER arc_font_amb;
        public AMS_AMB_HEADER arc_scr_amb;
        public AMS_AMB_HEADER arc_end_amb;
        public AMS_AMB_HEADER arc_end_jp_amb;
        public YSDS_HEADER stf_list_ysd;

        public void Clear()
        {
            this.arc_font_amb = null;
            this.arc_scr_amb = null;
            this.arc_end_amb = null;
            this.arc_end_jp_amb = null;
            Array.Clear(arc_cmn_amb, 0, 2);
            this.stf_list_ysd = null;
        }
    }
}
