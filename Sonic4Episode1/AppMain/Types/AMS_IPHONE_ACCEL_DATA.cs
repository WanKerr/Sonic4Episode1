public partial class AppMain
{
    private class AMS_IPHONE_ACCEL_DATA
    {
        public readonly NNS_VECTOR core = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR sensor = GlobalPool<NNS_VECTOR>.Alloc();
        public int rot_x;
        public int rot_y;
        public int rot_z;
    }
}
