public partial class AppMain
{
    public class NNS_VTXLIST_COMMON_DESC
    {
        public readonly NNS_VTXLIST_COMMON_ARRAY List0 = new NNS_VTXLIST_COMMON_ARRAY();
        public readonly NNS_VTXLIST_COMMON_ARRAY List1 = new NNS_VTXLIST_COMMON_ARRAY();
        public readonly NNS_VTXLIST_COMMON_ARRAY List2 = new NNS_VTXLIST_COMMON_ARRAY();
        public readonly NNS_VTXLIST_COMMON_ARRAY List3 = new NNS_VTXLIST_COMMON_ARRAY();

        public NNS_VTXLIST_COMMON_DESC Assign(NNS_VTXLIST_COMMON_DESC desc)
        {
            if (this != desc)
            {
                this.List0.Assign(desc.List0);
                this.List1.Assign(desc.List1);
                this.List2.Assign(desc.List2);
                this.List3.Assign(desc.List3);
            }
            return this;
        }
    }
}
