public partial class AppMain
{
    public class GMDECO_GLARE_INTERFACE : IClearable
    {
        public AMS_AMB_HEADER amb_header;
        public NNS_TEXFILELIST tex_buf;
        public object texlistbuf;
        public NNS_TEXLIST texlist;
        public int texId;
        public int regId;
        public int drawFlag;

        public void Clear()
        {
            this.amb_header = null;
            this.tex_buf = null;
            this.texlistbuf = null;
            this.texlist = null;
            this.texId = this.regId = this.drawFlag = 0;
        }
    }
}
