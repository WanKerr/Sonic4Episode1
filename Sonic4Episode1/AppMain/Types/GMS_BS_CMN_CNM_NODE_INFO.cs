public partial class AppMain
{
    public class GMS_BS_CMN_CNM_NODE_INFO
    {
        public readonly NNS_MATRIX node_w_mtx = new NNS_MATRIX();
        public int node_index;
        public int enable;
        public uint mode;
        public uint flag;

        public void Assign(GMS_BS_CMN_CNM_NODE_INFO p)
        {
            this.node_w_mtx.Assign(p.node_w_mtx);
            this.node_index = p.node_index;
            this.enable = p.enable;
            this.mode = p.mode;
            this.flag = p.flag;
        }
    }
}
