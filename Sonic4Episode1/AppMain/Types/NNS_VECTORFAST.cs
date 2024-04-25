public struct NNS_VECTORFAST
{
    public float x;
    public float y;
    public float z;
    public float w;

    public NNS_VECTORFAST(NNS_VECTORFAST vector)
    {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
        this.w = vector.w;
    }

    public void Assign(NNS_VECTORFAST vector)
    {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
        this.w = vector.w;
    }
}

