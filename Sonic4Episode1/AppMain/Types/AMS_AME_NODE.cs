public partial class AppMain
{
    public class AMS_AME_NODE
    {
        public readonly char[] name = new char[12];
        public short id;
        public short type;
        public uint flag;
        public int child_offset;
        public AMS_AME_NODE child;
        public int sibling_offset;
        public AMS_AME_NODE sibling;
        public int parent_offset;
        public AMS_AME_NODE parent;
    }
}
