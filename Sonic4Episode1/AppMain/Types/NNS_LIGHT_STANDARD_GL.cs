public partial class AppMain
{
    public class NNS_LIGHT_STANDARD_GL
    {
        public readonly NNS_VECTOR4D Position = new NNS_VECTOR4D();
        public readonly NNS_VECTOR SpotDirection = GlobalPool<NNS_VECTOR>.Alloc();
        public uint User;
        public NNS_RGBA Ambient;
        public NNS_RGBA Diffuse;
        public NNS_RGBA Specular;
        public float SpotExponent;
        public float SpotCutoff;
        public float ConstantAttenuation;
        public float LinearAttenuation;
        public float QuadraticAttenuation;
    }
}
