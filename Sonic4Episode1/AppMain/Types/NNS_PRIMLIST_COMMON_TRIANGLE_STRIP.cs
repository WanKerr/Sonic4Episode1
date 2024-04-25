public partial class AppMain
{
    public class NNS_PRIMLIST_COMMON_TRIANGLE_STRIP
    {
        public uint fType;
        public int nIndexSetSize;
        public int nStrip;
        public ushort[] pLengthList;
        public ushort[] pStripList;

        public NNS_PRIMLIST_COMMON_TRIANGLE_STRIP Assign(
          NNS_PRIMLIST_COMMON_TRIANGLE_STRIP desc)
        {
            this.fType = desc.fType;
            this.nIndexSetSize = desc.nIndexSetSize;
            this.nStrip = desc.nStrip;
            this.pLengthList = desc.pLengthList;
            this.pStripList = desc.pStripList;
            return this;
        }
    }
}
