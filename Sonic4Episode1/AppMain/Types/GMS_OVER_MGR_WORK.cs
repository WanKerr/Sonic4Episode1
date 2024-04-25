using System;

public partial class AppMain
{
    public class GMS_OVER_MGR_WORK
    {
        public readonly GMS_COCKPIT_2D_WORK[] string_sub_parts = new GMS_COCKPIT_2D_WORK[4];
        public readonly GMS_COCKPIT_2D_WORK[] fadeout_sub_parts = new GMS_COCKPIT_2D_WORK[2];
        public _GMS_OVER_MGR_WORK_UD_ proc_update;
        public _GMS_OVER_MGR_WORK_UD_ proc_disp;
        public uint wait_timer;

        internal void Clear()
        {
            this.proc_update = null;
            this.proc_disp = null;
            this.wait_timer = 0U;
            Array.Clear(string_sub_parts, 0, this.string_sub_parts.Length);
            Array.Clear(fadeout_sub_parts, 0, this.fadeout_sub_parts.Length);
        }
    }
}
