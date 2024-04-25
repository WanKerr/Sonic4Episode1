using mpp;

public partial class AppMain
{
    public struct TVXS_TEXTURE
    {
        public int tex_id;
        public uint vtx_num;
        public uint vtx_tbl_ofst;
        public uint prim_type;

        public TVXS_TEXTURE(byte[] data, int offset)
        {
            this.tex_id = MppBitConverter.ToInt32(data, offset);
            this.vtx_num = MppBitConverter.ToUInt32(data, offset + 4);
            this.vtx_tbl_ofst = MppBitConverter.ToUInt32(data, offset + 8);
            this.prim_type = MppBitConverter.ToUInt32(data, offset + 12);
        }

        public static int SizeBytes => 16;
    }
}
