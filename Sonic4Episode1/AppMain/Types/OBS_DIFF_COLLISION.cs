public partial class AppMain
{
    public class OBS_DIFF_COLLISION
    {
        public readonly MP_BLOCK[][] block_map_datap = new MP_BLOCK[2][];
        public DF_BLOCK[] cl_diff_datap;
        public DI_BLOCK[] direc_datap;
        public AT_BLOCK[] char_attr_datap;
        public ushort map_block_num_x;
        public ushort map_block_num_y;
        public uint diff_block_num;
        public uint dir_block_num;
        public uint attr_block_num;
        public int left;
        public int top;
        public int right;
        public int bottom;

        internal void Clear()
        {
            this.cl_diff_datap = null;
            this.direc_datap = null;
            this.block_map_datap[0] = null;
            this.block_map_datap[1] = null;
            this.char_attr_datap = null;
            this.map_block_num_x = 0;
            this.map_block_num_y = 0;
            this.diff_block_num = 0U;
            this.dir_block_num = 0U;
            this.attr_block_num = 0U;
            this.left = 0;
            this.top = 0;
            this.right = 0;
            this.bottom = 0;
        }
    }
}
