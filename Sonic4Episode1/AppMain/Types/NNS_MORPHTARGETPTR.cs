public partial class AppMain
{
    public class NNS_MORPHTARGETPTR
    {
        public int nVtxList;
        public NNS_VTXLISTPTR[] pMorphTarget;

        public NNS_MORPHTARGETPTR()
        {
        }

        public NNS_MORPHTARGETPTR(NNS_MORPHTARGETPTR morthTargetPtr)
        {
            this.nVtxList = morthTargetPtr.nVtxList;
            this.pMorphTarget = morthTargetPtr.pMorphTarget;
        }

        public NNS_MORPHTARGETPTR Assign(NNS_MORPHTARGETPTR morthTargetPtr)
        {
            this.nVtxList = morthTargetPtr.nVtxList;
            this.pMorphTarget = morthTargetPtr.pMorphTarget;
            return this;
        }
    }
}
