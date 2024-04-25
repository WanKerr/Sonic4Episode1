public partial class AppMain
{
    public class NNS_PRIMLIST_COMMON_TRIANGLE_LIST
    {
        public uint fType;
        public int nIndexSetSize;
        public int nTriangle;
        public ushort[] pTriangleList;

        public NNS_PRIMLIST_COMMON_TRIANGLE_LIST Assign(
          NNS_PRIMLIST_COMMON_TRIANGLE_LIST desc)
        {
            this.fType = desc.fType;
            this.nIndexSetSize = desc.nIndexSetSize;
            this.nTriangle = desc.nTriangle;
            this.pTriangleList = desc.pTriangleList;
            return this;
        }
    }
}
