public partial class AppMain
{
    public class NNS_CAMERA_TARGET_UPTARGET
    {
        public readonly NNS_VECTOR Position = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR Target = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR UpTarget = GlobalPool<NNS_VECTOR>.Alloc();
        public uint User;
        public int Fovy;
        public float Aspect;
        public float ZNear;
        public float ZFar;
    }
}
