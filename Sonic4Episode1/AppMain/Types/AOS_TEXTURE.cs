public partial class AppMain
{
    public class AOS_TEXTURE : IClearable
    {
        public NNS_TEXLIST texlist;
        public object texlist_buf;
        public int reg_id;
        public AMS_AMB_HEADER amb;
        public TXB_HEADER txb;

        public void Clear()
        {
            this.texlist = null;
            this.texlist_buf = null;
            this.reg_id = 0;
            this.amb = null;
            this.txb = null;
        }
    }
}
