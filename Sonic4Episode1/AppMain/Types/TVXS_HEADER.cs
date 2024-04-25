using mpp;

public partial class AppMain
{
    public struct TVXS_HEADER
    {
        public uint tex_num;
        public uint tex_tbl_ofst;

        public TVXS_HEADER(byte[] data, int offset)
        {
            this.tex_num = MppBitConverter.ToUInt32(data, offset + 4);
            this.tex_tbl_ofst = MppBitConverter.ToUInt32(data, offset + 8);
        }

        public static uint SizeBytes => 16;
    }
}
