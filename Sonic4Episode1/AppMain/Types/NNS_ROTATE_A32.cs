public partial class AppMain
{
}

public struct NNS_ROTATE_A32
{
    public int x;
    public int y;
    public int z;

    public NNS_ROTATE_A32(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static explicit operator int[](NNS_ROTATE_A32 rv)
    {
        return new int[3] { rv.x, rv.y, rv.z };
    }

    public static explicit operator NNS_ROTATE_A32(int[] array)
    {
        return new NNS_ROTATE_A32()
        {
            x = array[0],
            y = array[1],
            z = array[2]
        };
    }
}
