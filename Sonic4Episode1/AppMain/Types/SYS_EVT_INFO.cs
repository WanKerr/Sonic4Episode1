public partial class AppMain
{
    private class SYS_EVT_INFO
    {
        public sbyte[] arg = new sbyte[8];
        public SYS_EVT_DATA[] evt_data;
        public int evt_data_num;
        public SYS_EVT_DATA cur_evt_data;
        public short cur_evt_id;
        public short old_evt_id;
        public SYS_EVT_DATA next_evt_data;
        public short req_evt_id;
        public short req_evt_case;
        public uint flag;
        public uint arg_size;
    }
}
