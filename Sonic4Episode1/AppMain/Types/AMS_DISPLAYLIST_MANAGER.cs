public partial class AppMain
{
    public class AMS_DISPLAYLIST_MANAGER
    {
        public readonly AMS_DRAW_SORT[] sortlist = New<AMS_DRAW_SORT>(512);
        public readonly AMS_DISPLAYLIST[] displaylist = New<AMS_DISPLAYLIST>(4);
        public readonly AMS_REGISTLIST[] registlist = New<AMS_REGISTLIST>(256);
        public int write_index;
        public int last_index;
        public int read_index;
        public ArrayPointer<object> command_buf_ptr;
        public AMS_COMMAND_BUFFER_HEADER write_header;
        public int reg_write_num;
        public AMS_COMMAND_BUFFER_HEADER read_header;
        public int sort_num;
        public int regist_num;
        public int reg_write_index;
        public int reg_read_index;
        public int reg_end_index;
    }
}
