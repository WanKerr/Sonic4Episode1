public partial class AppMain
{
    public class AMS_AME_NODE_RADIAL : AMS_AME_NODE
    {
        public readonly NNS_VECTOR4D position = GlobalPool<NNS_VECTOR4D>.Alloc();
        public float magnitude;
        public float attenuation;
    }
}
