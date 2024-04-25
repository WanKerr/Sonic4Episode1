public partial class AppMain
{
    public class GMS_MAP_PRIM_DRAW_TVX_UV_WORK
    {
        public int mgr_index_tbl_num;
        public DoubleType<uint[], GMS_MAP_PRIM_DRAW_TVX_MGR_INDEX[]> mgr_index_tbl_addr;
        public int[] mgr_tbl_num;
        public DoubleType<uint[], GMS_MAP_PRIM_DRAW_TVX_MGR[][]> mgr_tbl_addr;
        public DoubleType<uint[], NNS_TEXCOORD[][]> uv_mgr_tbl_addr;
        public uint[] frame_index_tbl;
        public uint[] frame_tbl;
        public int[] tex_uv_index_tbl;
    }
}
