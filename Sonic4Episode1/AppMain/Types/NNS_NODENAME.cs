public partial class AppMain
{
    public class NNS_NODENAME
    {
        public int iNode;
        public string Name;

        public NNS_NODENAME()
        {
        }

        public NNS_NODENAME(NNS_NODENAME nodeName)
        {
            this.iNode = nodeName.iNode;
            this.Name = nodeName.Name;
        }

        public NNS_NODENAME Assign(NNS_NODENAME nodeName)
        {
            this.iNode = nodeName.iNode;
            this.Name = nodeName.Name;
            return this;
        }
    }
}
