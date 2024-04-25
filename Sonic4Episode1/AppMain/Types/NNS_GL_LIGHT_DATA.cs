public partial class AppMain
{
    private class NNS_GL_LIGHT_DATA
    {
        public NNS_RGBA Ambient = new NNS_RGBA();
        public NNS_RGBA Diffuse = new NNS_RGBA();
        public NNS_RGBA Specular = new NNS_RGBA();
        public NNS_VECTOR Direction = GlobalPool<NNS_VECTOR>.Alloc();
        public NNS_VECTOR4D Position = new NNS_VECTOR4D();
        public NNS_VECTOR Target = GlobalPool<NNS_VECTOR>.Alloc();
        public int bEnable;
        public uint fType;
        public float Intensity;
        public int RotType;
        public NNS_ROTATE_A32 Rotation;
        public int InnerAngle;
        public int OuterAngle;
        public float InnerRange;
        public float OuterRange;
        public float FallOffStart;
        public float FallOffEnd;
        public float SpotExponent;
        public float SpotCutoff;
        public float ConstantAttenuation;
        public float LinearAttenuation;
        public float QuadraticAttenuation;
    }
}
