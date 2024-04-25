public partial class AppMain
{
    public class AMS_AME_HEADER
    {
        public byte[] file_id = new byte[4];
        public readonly AMS_AME_BOUNDING bounding = new AMS_AME_BOUNDING();
        public int file_version;
        public int node_num;
        public uint node_ofst;
        public AMS_AME_NODE[] node;
    }
}
