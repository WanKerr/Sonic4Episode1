public partial class AppMain
{
    public class NNS_TEXLIST
    {
        public int nTex;
        public NNS_TEXINFO[] pTexInfoList;

        public NNS_TEXLIST()
        {
        }

        public NNS_TEXLIST(NNS_TEXLIST pFrom)
        {
            this.nTex = pFrom.nTex;
            this.pTexInfoList = new NNS_TEXINFO[this.nTex];
            for (int index = 0; index < this.nTex; ++index)
                this.pTexInfoList[index] = new NNS_TEXINFO(pFrom.pTexInfoList[index]);
        }

        public void Clear()
        {
            this.nTex = 0;
            this.pTexInfoList = null;
        }
    }
}
