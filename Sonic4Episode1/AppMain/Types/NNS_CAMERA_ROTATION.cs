public partial class AppMain
{
    public class NNS_CAMERA_ROTATION
    {
        public readonly NNS_VECTOR Position = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_ROTATE_A32 Rotation = new NNS_ROTATE_A32();
        public uint User;
        public int Fovy;
        public float Aspect;
        public float ZNear;
        public float ZFar;
        public readonly int RotType;
    }
}
