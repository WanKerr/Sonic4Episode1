public partial class AppMain
{
    public class GMS_BS_CMN_SNM_NODE_INFO
    {
        public uint[] reserved = new uint[3];
        public readonly NNS_MATRIX node_w_mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public int node_index;
    }
}
