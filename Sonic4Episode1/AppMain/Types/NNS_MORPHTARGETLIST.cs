public partial class AppMain
{
    public class NNS_MORPHTARGETLIST
    {
        public int nMorphTarget;
        public NNS_MORPHTARGETPTR[] pMorphTargetPtrList;

        public NNS_MORPHTARGETLIST()
        {
        }

        public NNS_MORPHTARGETLIST(NNS_MORPHTARGETLIST morthTargetList)
        {
            this.nMorphTarget = morthTargetList.nMorphTarget;
            this.pMorphTargetPtrList = morthTargetList.pMorphTargetPtrList;
        }

        public NNS_MORPHTARGETLIST Assign(NNS_MORPHTARGETLIST morthTargetList)
        {
            this.nMorphTarget = morthTargetList.nMorphTarget;
            this.pMorphTargetPtrList = morthTargetList.pMorphTargetPtrList;
            return this;
        }
    }
}
