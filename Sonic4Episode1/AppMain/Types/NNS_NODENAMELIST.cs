public partial class AppMain
{
    public class NNS_NODENAMELIST
    {
        public NNE_NODENAME_SORTTYPE SortType;
        public int nNode;
        public NNS_NODENAME[] pNodeNameList;

        public NNS_NODENAMELIST()
        {
        }

        public NNS_NODENAMELIST(NNS_NODENAMELIST nodeNameList)
        {
            this.SortType = nodeNameList.SortType;
            this.nNode = nodeNameList.nNode;
            this.pNodeNameList = nodeNameList.pNodeNameList;
        }

        public NNS_NODENAMELIST Assign(NNS_NODENAMELIST nodeNameList)
        {
            if (this != nodeNameList)
            {
                this.SortType = nodeNameList.SortType;
                this.nNode = nodeNameList.nNode;
                this.pNodeNameList = nodeNameList.pNodeNameList;
            }
            return this;
        }
    }
}
