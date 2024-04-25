public partial class AppMain
{
    public class DMAP_WATER
    {
        public readonly DMAP_WATER_OBJ[] _object = New<DMAP_WATER_OBJ>(2);
        public readonly AOS_TEXTURE tex_color = new AOS_TEXTURE();
        public AMS_AMB_HEADER amb_object;
        public AMS_AMB_HEADER amb_texture;
        public int regist_index;
        public float draw_u;
        public float draw_v;
        public float scale;
        public float ofst_u;
        public float ofst_v;
        public float repeat_u;
        public float repeat_v;
        public float speed_u;
        public float speed_v;
        public float speed_surface;
        public float pos_x;
        public float pos_y;
        public float pos_dy;
        public int rot_z;
        public uint color;
        public float repeat_pos_x;
        public DMAP_PARAM_WATER draw_param;
    }
}
