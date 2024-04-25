public partial class AppMain
{
    public class AMS_AME_NODE_VORTEX : AMS_AME_NODE
    {
        public readonly NNS_VECTOR4D position = GlobalPool<NNS_VECTOR4D>.Alloc();
        public readonly NNS_VECTOR4D axis = GlobalPool<NNS_VECTOR4D>.Alloc();
    }
}
