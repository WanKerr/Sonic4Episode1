public partial class AppMain
{
    public class AMS_AME_NODE_GRAVITY : AMS_AME_NODE
    {
        public readonly NNS_VECTOR4D direction = GlobalPool<NNS_VECTOR4D>.Alloc();
        public float magnitude;
    }
}
