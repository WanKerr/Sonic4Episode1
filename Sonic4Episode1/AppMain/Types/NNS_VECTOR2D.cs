using System.IO;

public class NNS_VECTOR2D
{
    public float x;
    public float y;

    public NNS_VECTOR2D Assign(NNS_VECTOR2D vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        return this;
    }

    public void Clear()
    {
        this.x = this.y = 0.0f;
    }

    public static NNS_VECTOR2D Read(BinaryReader reader)
    {
        return new NNS_VECTOR2D()
        {
            x = reader.ReadSingle(),
            y = reader.ReadSingle()
        };
    }
}
