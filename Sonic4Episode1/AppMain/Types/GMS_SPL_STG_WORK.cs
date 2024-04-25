public partial class AppMain
{
    public class GMS_SPL_STG_WORK
    {
        public readonly NNS_VECTOR light_vec = GlobalPool<NNS_VECTOR>.Alloc();
        public uint counter;
        public uint flag;
        public int roll;
        public int roll_spd;
        public ushort get_ring;
    }
}
