public partial class AppMain
{
    public class GMS_BS_CMN_SNM_WORK
    {
        public readonly GMS_BS_CMN_BMCB_LINK bmcb_link = new GMS_BS_CMN_BMCB_LINK();
        public uint[] reserved = new uint[3];
        public ushort reg_node_cnt;
        public ushort reg_node_max;
        public GMS_BS_CMN_SNM_NODE_INFO[] node_info_list;
    }
}
