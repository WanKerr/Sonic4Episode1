public partial class AppMain
{
    public class NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST
    {
        public uint fType;
        public int nIndexSetSize;
        public int nTriangle;
        public ushort[] pTriangleList;
        public int nQuad;
        public ushort[] pQuadList;

        public NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST Assign(
          NNS_PRIMLIST_COMMON_TRIANGLE_QUAD_LIST list)
        {
            this.fType = list.fType;
            this.nIndexSetSize = list.nIndexSetSize;
            this.nTriangle = list.nTriangle;
            this.pTriangleList = list.pTriangleList;
            this.nQuad = list.nQuad;
            this.pQuadList = list.pQuadList;
            return this;
        }
    }
}
