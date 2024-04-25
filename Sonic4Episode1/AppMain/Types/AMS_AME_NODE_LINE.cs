public partial class AppMain
{
    public class AMS_AME_NODE_LINE : AMS_AME_NODE_TR_ROT
    {
        public float z_bias;
        public float inheritance_rate;
        public float life;
        public float start_time;
        public float length_start;
        public float length_end;
        public float inside_width_start;
        public float inside_width_end;
        public float outside_width_start;
        public float outside_width_end;
        public AMS_RGBA8888 inside_color_start;
        public AMS_RGBA8888 inside_color_end;
        public AMS_RGBA8888 outside_color_start;
        public AMS_RGBA8888 outside_color_end;
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
