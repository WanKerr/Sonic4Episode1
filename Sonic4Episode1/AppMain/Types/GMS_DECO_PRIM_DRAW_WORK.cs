public partial class AppMain
{
    public class GMS_DECO_PRIM_DRAW_WORK
    {
        public readonly GMS_DECO_PRIM_DRAW_STACK[] stack = New<GMS_DECO_PRIM_DRAW_STACK>(128);
        public AOS_TEXTURE tex;
        public int tex_id;
        public uint command;
        public ushort all_vtx_num;
        public ushort stack_num;
    }
}
