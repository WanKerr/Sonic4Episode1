using mpp;

public partial class AppMain
{
}

public struct NNS_RGBA
{
    public float r;
    public float g;
    public float b;
    public float a;

    public void Clear()
    {
        this.r = this.g = this.b = this.a = 0.0f;
    }

    public NNS_RGBA(float _r, float _g, float _b, float _a)
    {
        this.r = _r;
        this.g = _g;
        this.b = _b;
        this.a = _a;
    }

    public static explicit operator OpenGL.glArray4f(NNS_RGBA c)
    {
        return new OpenGL.glArray4f(c.r, c.g, c.b, c.a);
    }
}
