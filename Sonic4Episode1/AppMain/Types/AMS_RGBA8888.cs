using System.Runtime.InteropServices;

public partial class AppMain
{
    [StructLayout(LayoutKind.Explicit)]
    public struct AMS_RGBA8888
    {
        [FieldOffset(0)]
        public byte r;
        [FieldOffset(1)]
        public byte g;
        [FieldOffset(2)]
        public byte b;
        [FieldOffset(3)]
        public byte a;
        [FieldOffset(0)]
        public uint color;
    }
}
