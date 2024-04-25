public partial class AppMain
{
    public class NNS_COMMON_PNW4
    {
        public readonly NNS_VECTOR Pos = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_VECTOR Nrm = GlobalPool<NNS_VECTOR>.Alloc();
        public readonly NNS_COMMON_WEIGHT[] Wgt = New<NNS_COMMON_WEIGHT>(4);
    }
}
