public partial class AppMain
{
}

public class NNS_RGBA_U8
{
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    public NNS_RGBA_U8(byte _r, byte _g, byte _b, byte _a)
    {
        this.r = _r;
        this.g = _g;
        this.b = _b;
        this.a = _a;
    }

    public NNS_RGBA_U8()
    {
    }

    public static int SizeOf()
    {
        return 4;
    }
}
