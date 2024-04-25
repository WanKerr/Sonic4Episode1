public partial class AppMain
{
    public class NNS_MORPHTARGETNAMELIST
    {
        public NNE_MORPHTARGETNAME_SORTTYPE SortType;
        public int nMorphTarget;
        public NNS_MORPHTARGETNAME[] pMorphTargetNameList;

        public NNS_MORPHTARGETNAMELIST()
        {
        }

        public NNS_MORPHTARGETNAMELIST(
          NNS_MORPHTARGETNAMELIST morthTargetNameList)
        {
            this.SortType = morthTargetNameList.SortType;
            this.nMorphTarget = morthTargetNameList.nMorphTarget;
            this.pMorphTargetNameList = morthTargetNameList.pMorphTargetNameList;
        }

        public NNS_MORPHTARGETNAMELIST Assign(
          NNS_MORPHTARGETNAMELIST morthTargetNameList)
        {
            this.SortType = morthTargetNameList.SortType;
            this.nMorphTarget = morthTargetNameList.nMorphTarget;
            this.pMorphTargetNameList = morthTargetNameList.pMorphTargetNameList;
            return this;
        }
    }
}
