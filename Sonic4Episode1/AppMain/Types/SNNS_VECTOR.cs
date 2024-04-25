
using Microsoft.Xna.Framework;

public struct SNNS_VECTOR
{
    public float x;
    public float y;
    public float z;

    public SNNS_VECTOR(NNS_VECTOR vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
    }

    public void Assign(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public void Assign(NNS_VECTOR vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
    }

    public void Assign(ref SNNS_VECTOR vec)
    {
        this.x = vec.x;
        this.y = vec.y;
        this.z = vec.z;
    }

    public static explicit operator Vector3(SNNS_VECTOR vec)
    {
        return new Vector3(vec.x, vec.y, vec.z);
    }
}

