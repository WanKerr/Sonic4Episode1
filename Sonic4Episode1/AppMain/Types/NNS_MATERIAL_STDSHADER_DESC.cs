public partial class AppMain
{
    public class NNS_MATERIAL_STDSHADER_DESC
    {
        public uint fFlag;
        public uint User;
        public NNS_MATERIAL_STDSHADER_COLOR pColor;
        public NNS_MATERIAL_LOGIC pLogic;
        public uint fTexType;
        public int nTex;
        public NNS_MATERIAL_STDSHADER_TEXMAP_DESC[] pTexDesc;

        public NNS_MATERIAL_STDSHADER_DESC()
        {
        }

        public NNS_MATERIAL_STDSHADER_DESC(NNS_MATERIAL_STDSHADER_DESC desc)
        {
            this.fFlag = desc.fFlag;
            this.User = desc.User;
            this.pColor = desc.pColor;
            this.pLogic = desc.pLogic;
            this.fTexType = desc.fTexType;
            this.nTex = desc.nTex;
            this.pTexDesc = desc.pTexDesc;
        }

        public NNS_MATERIAL_STDSHADER_DESC Assign(
          NNS_MATERIAL_STDSHADER_DESC desc)
        {
            this.fFlag = desc.fFlag;
            this.User = desc.User;
            this.pColor = desc.pColor;
            this.pLogic = desc.pLogic;
            this.fTexType = desc.fTexType;
            this.nTex = desc.nTex;
            this.pTexDesc = desc.pTexDesc;
            return this;
        }
    }
}
