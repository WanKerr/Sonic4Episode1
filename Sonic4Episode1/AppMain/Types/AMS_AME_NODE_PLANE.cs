public partial class AppMain
{
    public class AMS_AME_NODE_PLANE : AMS_AME_NODE_TR_ROT
    {
        public readonly NNS_VECTOR4D rotate_axis = GlobalPool<NNS_VECTOR4D>.Alloc();
        public float z_bias;
        public float inheritance_rate;
        public float life;
        public float start_time;
        public float size;
        public float size_chaos;
        public float scale_x_start;
        public float scale_x_end;
        public float scale_y_start;
        public float scale_y_end;
        public AMS_RGBA8888 color_start;
        public AMS_RGBA8888 color_end;
        public int blend;
        public short texture_slot;
        public short texture_id;
        public float cropping_l;
        public float cropping_t;
        public float cropping_r;
        public float cropping_b;
        public float scroll_u;
        public float scroll_v;
        public AMS_AME_TEX_ANIM tex_anim;
    }
}
