
public struct VecFx32 : IClearable
{
    public int x;
    public int y;
    public int z;

    public void Clear()
    {
        this.x = this.y = this.z = 0;
    }

    public VecFx32(int _x, int _y, int _z)
    {
        this.x = _x;
        this.y = _y;
        this.z = _z;
    }

    public VecFx32(VecFx32 vecFx32)
    {
        this.x = vecFx32.x;
        this.y = vecFx32.y;
        this.z = vecFx32.z;
    }

    public void Assign(VecFx32 vecFx32)
    {
        this.x = vecFx32.x;
        this.y = vecFx32.y;
        this.z = vecFx32.z;
    }
}
