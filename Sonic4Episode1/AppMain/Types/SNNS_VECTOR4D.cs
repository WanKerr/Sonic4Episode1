
public struct SNNS_VECTOR4D
{
    public float x;
    public float y;
    public float z;
    public float w;

    public SNNS_VECTOR4D(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }


    public void Assign(ref SNNS_VECTOR4D val)
    {
        this.x = val.x;
        this.y = val.y;
        this.z = val.z;
        this.w = val.w;
    }

    public void Clear()
    {
        this.x = this.y = this.z = this.w = 0.0f;
    }
}

