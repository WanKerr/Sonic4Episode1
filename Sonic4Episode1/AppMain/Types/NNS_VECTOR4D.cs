using mpp;

public class NNS_VECTOR4D : NNS_VECTOR
{
    public float w;

    public new void Clear()
    {
        base.Clear();
        this.w = 0.0f;
    }

    public void Assign(NNS_VECTOR4D v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
        this.w = v.w;
    }

    public static explicit operator OpenGL.glArray4f(NNS_VECTOR4D v)
    {
        return new OpenGL.glArray4f(v.x, v.y, v.z, v.w);
    }

    public static explicit operator float[](NNS_VECTOR4D v)
    {
        return new float[4] { v.x, v.y, v.z, v.w };
    }
}
