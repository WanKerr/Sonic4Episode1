public partial class AppMain
{
    public class GMS_MAP_PRIM_DRAW_WORK
    {
        public GMS_MAP_PRIM_DRAW_STACK[] stack = New<GMS_MAP_PRIM_DRAW_STACK>(byte.MaxValue);
        public int tex_id;
        public uint all_vtx_num;
        public uint stack_num;
        public uint op;
    }
}
