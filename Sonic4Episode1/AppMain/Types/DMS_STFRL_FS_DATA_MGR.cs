using System;

public partial class AppMain
{
    public class DMS_STFRL_FS_DATA_MGR
    {
        public readonly AMS_AMB_HEADER[] arc_cmn_amb_fs = new AMS_AMB_HEADER[2];
        public AMS_AMB_HEADER arc_list_font_amb_fs;
        public AMS_AMB_HEADER arc_scr_amb_fs;
        public AMS_AMB_HEADER arc_end_amb_fs;
        public AMS_AMB_HEADER arc_end_jp_amb_fs;
        public AMS_FS staff_list_fs;

        public void Clear()
        {
            this.arc_list_font_amb_fs = null;
            this.arc_scr_amb_fs = null;
            this.arc_end_amb_fs = null;
            this.arc_end_jp_amb_fs = null;
            Array.Clear(arc_cmn_amb_fs, 0, this.arc_cmn_amb_fs.Length);
            this.staff_list_fs = null;
        }
    }
}
