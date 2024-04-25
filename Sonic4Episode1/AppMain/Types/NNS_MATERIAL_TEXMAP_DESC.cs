public partial class AppMain
{
    public class NNS_MATERIAL_TEXMAP_DESC
    {
        public NNS_RGBA? pBorderColor = new NNS_RGBA?();
        public uint fType;
        public int iTexIdx;
        public int EnvMode;
        public NNS_TEXTURE_COMBINE pCombine;
        public NNS_RGBA EnvColor;
        public NNS_TEXCOORD Offset;
        public NNS_TEXCOORD Scale;
        public int WrapS;
        public int WrapT;
        public NNS_TEXTURE_FILTERMODE pFilterMode;
        public NNS_TEXTURE_LOD_PARAM pLODParam;
        public object pTexInfo;
        public uint Reserved1;
        public uint Reserved0;

        public void Assign(NNS_MATERIAL_TEXMAP_DESC pPtr)
        {
            mppAssertNotImpl();
        }

        public NNS_MATERIAL_TEXMAP_DESC()
        {
        }

        public NNS_MATERIAL_TEXMAP_DESC(NNS_MATERIAL_TEXMAP_DESC desc)
        {
            this.fType = desc.fType;
            this.iTexIdx = desc.iTexIdx;
            this.EnvMode = desc.EnvMode;
            this.pCombine = desc.pCombine;
            this.EnvColor = desc.EnvColor;
            this.Offset = desc.Offset;
            this.Scale = desc.Scale;
            this.WrapS = desc.WrapS;
            this.WrapT = desc.WrapT;
            this.pBorderColor = desc.pBorderColor;
            this.pFilterMode = desc.pFilterMode;
            this.pLODParam = desc.pLODParam;
            this.pTexInfo = desc.pTexInfo;
            this.Reserved1 = desc.Reserved1;
            this.Reserved0 = desc.Reserved0;
        }
    }
}
