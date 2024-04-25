public partial class AppMain
{
    public class NNS_CAMERA_TARGET_UPVECTOR
    {
        public readonly NNS_VECTOR Position = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR Target = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR UpVector = GlobalPool<NNS_VECTOR>.Alloc();
        public uint User;
        public int Fovy;
        public float Aspect;
        public float ZNear;
        public float ZFar;
    }
}
