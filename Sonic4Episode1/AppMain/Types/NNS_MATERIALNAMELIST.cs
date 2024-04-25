public partial class AppMain
{
    public class NNS_MATERIALNAMELIST
    {
        public NNE_MATERIALNAME_SORTTYPE SortType;
        public int nMaterial;
        public NNS_MATERIALNAME[] pMaterialNameList;

        public NNS_MATERIALNAMELIST()
        {
        }

        public NNS_MATERIALNAMELIST(NNS_MATERIALNAMELIST materialNameList)
        {
            this.SortType = materialNameList.SortType;
            this.nMaterial = materialNameList.nMaterial;
            this.pMaterialNameList = materialNameList.pMaterialNameList;
        }

        public NNS_MATERIALNAMELIST Assign(NNS_MATERIALNAMELIST materialNameList)
        {
            if (this != materialNameList)
            {
                this.SortType = materialNameList.SortType;
                this.nMaterial = materialNameList.nMaterial;
                this.pMaterialNameList = materialNameList.pMaterialNameList;
            }
            return this;
        }
    }
}
