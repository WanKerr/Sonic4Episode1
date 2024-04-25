public partial class AppMain
{
    private class GMS_TVX_DRAW_STACK : IClearable
    {
        public AOS_TVX_VERTEX[] vtx;
        public VecFx32 pos;
        public VecFx32 scale;
        public uint disp_flag;
        public uint vtx_num;
        public int rotate_z;
        public NNS_TEXCOORD coord;
        public uint color;

        public void Clear()
        {
            this.vtx = null;
            this.pos.Clear();
            this.scale.Clear();
            this.disp_flag = this.vtx_num = 0U;
            this.rotate_z = 0;
            this.coord.Clear();
            this.color = 0U;
        }
    }
}
