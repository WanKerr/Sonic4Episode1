public partial class AppMain
{
}

public class NNS_RGB : IClearable
{
    public float r;
    public float g;
    public float b;

    public NNS_RGB()
    {
    }

    public NNS_RGB(float _r, float _g, float _b)
    {
        this.r = _r;
        this.g = _g;
        this.b = _b;
    }

    public NNS_RGB Assign(NNS_RGB rgb)
    {
        this.r = rgb.r;
        this.g = rgb.g;
        this.b = rgb.b;
        return this;
    }

    public void Clear()
    {
        this.r = this.g = this.b = 0.0f;
    }
}
