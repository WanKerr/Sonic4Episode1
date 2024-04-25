public partial class AppMain
{
    private class GMS_TVX_DRAW_WORK : IClearable
    {
        public GMS_TVX_DRAW_STACK[] stack = New<GMS_TVX_DRAW_STACK>(GMD_TVX_DRAW_STACK_NUM);
        public NNS_TEXLIST tex;
        public int tex_id;
        public uint all_vtx_num;
        public uint stack_num;
        public int u_wrap;
        public int v_wrap;

        public void Clear()
        {
            this.tex = null;
            this.tex_id = 0;
            this.all_vtx_num = this.stack_num = 0U;
            this.u_wrap = this.v_wrap = 0;
            ClearArray(this.stack);
        }
    }
}
