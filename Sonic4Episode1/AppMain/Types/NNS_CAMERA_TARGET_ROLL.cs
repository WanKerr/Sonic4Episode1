public partial class AppMain
{
    public class NNS_CAMERA_TARGET_ROLL : IClearable
    {
        public readonly NNS_VECTOR Position = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR Target = GlobalPool<NNS_VECTOR>.Alloc();
        public uint User;
        public int Fovy;
        public float Aspect;
        public float ZNear;
        public float ZFar;
        public int Roll;

        public void Clear()
        {
            this.User = 0U;
            this.Fovy = 0;
            this.Aspect = 0.0f;
            this.ZNear = 0.0f;
            this.ZFar = 0.0f;
            this.Roll = 0;
            this.Position.Clear();
            this.Target.Clear();
        }
    }
}
