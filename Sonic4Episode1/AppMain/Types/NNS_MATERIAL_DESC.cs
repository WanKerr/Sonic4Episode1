public partial class AppMain
{
    public class NNS_MATERIAL_DESC
    {
        public uint fFlag;
        public uint User;
        public NNS_MATERIAL_COLOR pColor;
        public NNS_MATERIAL_COLOR pBackColor;
        public NNS_MATERIAL_LOGIC pLogic;
        public int nTex;
        public NNS_MATERIAL_TEXMAP_DESC[] pTexDesc;

        public NNS_MATERIAL_DESC()
        {
        }

        public NNS_MATERIAL_DESC(NNS_MATERIAL_DESC desc)
        {
            this.fFlag = desc.fFlag;
            this.User = desc.User;
            this.pColor = desc.pColor;
            this.pBackColor = desc.pBackColor;
            this.pLogic = desc.pLogic;
            this.nTex = desc.nTex;
            this.pTexDesc = desc.pTexDesc;
        }

        public NNS_MATERIAL_DESC Assign(NNS_MATERIAL_DESC desc)
        {
            this.fFlag = desc.fFlag;
            this.User = desc.User;
            this.pColor = desc.pColor;
            this.pBackColor = desc.pBackColor;
            this.pLogic = desc.pLogic;
            this.nTex = desc.nTex;
            this.pTexDesc = desc.pTexDesc;
            return this;
        }
    }
}
