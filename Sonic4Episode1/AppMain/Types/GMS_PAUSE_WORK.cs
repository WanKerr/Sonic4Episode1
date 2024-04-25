public partial class AppMain
{
    public class GMS_PAUSE_WORK
    {
        public uint flag;
        public _proc_update_ proc_update;
        public uint time_count_flag_save;

        internal void Clear()
        {
            this.flag = 0U;
            this.proc_update = null;
            this.time_count_flag_save = 0U;
        }

        public delegate void _proc_update_(GMS_PAUSE_WORK work);
    }
}
