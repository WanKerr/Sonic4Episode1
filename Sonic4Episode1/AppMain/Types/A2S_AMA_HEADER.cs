public partial class AppMain
{
    public class A2S_AMA_HEADER
    {
        public uint version;
        public uint node_num;
        public uint act_num;
        public int node_tbl_offset;
        public A2S_AMA_NODE[] node_tbl;
        public int act_tbl_offset;
        public A2S_AMA_ACT[] act_tbl;
        public int node_name_tbl_offset;
        public int act_name_tbl_offset;
    }
}
