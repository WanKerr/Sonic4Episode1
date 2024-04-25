public partial class AppMain
{
    public class IZS_FADE_WORK
    {
        public readonly AMS_PARAM_DRAW_PRIMITIVE prim_param = GlobalPool<AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        public readonly NNS_PRIM2D_PC[][] vtx = New<NNS_PRIM2D_PC>(2, 4);
        public readonly NNS_MATRIX mtx = GlobalPool<NNS_MATRIX>.Alloc();
        public NNS_RGBA start_col;
        public NNS_RGBA end_col;
        public NNS_RGBA now_col;
        public float time;
        public float count;
        public float speed;
        public uint flag;
        public uint draw_state;
        public ushort dt_prio;
        public ushort vtx_no;

        public void Clear()
        {
            this.time = this.count = this.speed = 0.0f;
            this.flag = this.draw_state = 0U;
            this.draw_state = this.dt_prio = this.vtx_no = 0;
            this.start_col.Clear();
            this.end_col.Clear();
            this.now_col.Clear();
            this.mtx.Clear();
            this.prim_param.Clear();
            for (int index1 = 0; index1 < 2; ++index1)
            {
                for (int index2 = 0; index2 < 4; ++index2)
                    this.vtx[index1][index2].Clear();
            }
        }
    }
}
