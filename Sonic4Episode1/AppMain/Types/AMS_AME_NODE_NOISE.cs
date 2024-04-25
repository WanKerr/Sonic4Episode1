public partial class AppMain
{
    public class AMS_AME_NODE_NOISE : AMS_AME_NODE
    {
        public readonly NNS_VECTOR4D axis = GlobalPool<NNS_VECTOR4D>.Alloc();
        public float magnitude;
    }
}
