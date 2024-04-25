public partial class AppMain
{
    public class AMS_AME_NODE_MODEL : AMS_AME_NODE_TR_ROT
    {
        public readonly NNS_VECTOR4D rotate_axis = GlobalPool<NNS_VECTOR4D>.Alloc();
        public readonly NNS_VECTOR4D scale_start = GlobalPool<NNS_VECTOR4D>.Alloc();
        public readonly NNS_VECTOR4D scale_end = GlobalPool<NNS_VECTOR4D>.Alloc();
        public char[] model_name = new char[8];
        public float z_bias;
        public float inheritance_rate;
        public float life;
        public float start_time;
        public int lod;
        public AMS_RGBA8888 color_start;
        public AMS_RGBA8888 color_end;
        public int blend;
        public float scroll_u;
        public float scroll_v;
    }
}
