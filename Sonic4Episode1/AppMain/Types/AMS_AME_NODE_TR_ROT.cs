public partial class AppMain
{
    public class AMS_AME_NODE_TR_ROT : AMS_AME_NODE
    {
        public readonly NNS_VECTOR4D translate = GlobalPool<NNS_VECTOR4D>.Alloc();
        public NNS_QUATERNION rotate;
    }
}
