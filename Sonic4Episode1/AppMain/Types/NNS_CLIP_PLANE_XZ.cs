using System.Runtime.InteropServices;

public partial class AppMain
{
    [StructLayout(LayoutKind.Explicit)]
    public class NNS_CLIP_PLANE_XZ
    {
        [FieldOffset(0)]
        public float nx;
        [FieldOffset(0)]
        public float mul;
        [FieldOffset(4)]
        public float nz;
        [FieldOffset(4)]
        public float ofs;
    }
}
