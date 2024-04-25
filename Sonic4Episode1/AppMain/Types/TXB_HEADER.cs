public partial class AppMain
{
    public class TXB_HEADER
    {
        public byte[] file_id = new byte[4];
        public TXB_TEXFILELIST texfilelist = new TXB_TEXFILELIST();
        public byte[] pad = new byte[8];
        public int texfilelist_offset;
    }
}
