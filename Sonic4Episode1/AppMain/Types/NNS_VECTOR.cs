using System.IO;
using mpp;

public class NNS_VECTOR : IClearable
{
    public float x;
    public float y;
    public float z;

    public NNS_VECTOR()
    {
    }

    public NNS_VECTOR(float _x, float _y, float _z)
    {
        this.x = _x;
        this.y = _y;
        this.z = _z;
    }

    public NNS_VECTOR Assign(NNS_VECTOR vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
        return this;
    }

    public NNS_VECTOR Assign(ref SNNS_VECTOR vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
        return this;
    }

    public NNS_VECTOR Assign(ref SNNS_VECTOR4D vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
        return this;
    }

    public void Clear()
    {
        this.x = this.y = this.z = 0.0f;
    }

    public static explicit operator OpenGL.glArray4f(NNS_VECTOR v)
    {
        return new OpenGL.glArray4f(v.x, v.y, v.z, 0.0f);
    }

    public static explicit operator float[](NNS_VECTOR v)
    {
        return new float[3] { v.x, v.y, v.z };
    }

    internal NNS_VECTOR Assign(VecFx32 vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
        return this;
    }

    public static NNS_VECTOR Read(BinaryReader reader)
    {
        NNS_VECTOR nnsVector = GlobalPool<NNS_VECTOR>.Alloc();
        nnsVector.x = reader.ReadSingle();
        nnsVector.y = reader.ReadSingle();
        nnsVector.z = reader.ReadSingle();
        return nnsVector;
    }
}
