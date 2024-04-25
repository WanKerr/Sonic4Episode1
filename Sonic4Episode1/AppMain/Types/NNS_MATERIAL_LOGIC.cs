public partial class AppMain
{
    public class NNS_MATERIAL_LOGIC
    {
        public uint fFlag;
        public ushort SrcFactorRGB;
        public ushort DstFactorRGB;
        public ushort SrcFactorA;
        public ushort DstFactorA;
        public NNS_RGBA BlendColor;
        public ushort BlendOp;
        public ushort LogicOp;
        public ushort AlphaFunc;
        public ushort DepthFunc;
        public float AlphaRef;

        public NNS_MATERIAL_LOGIC Assign(NNS_MATERIAL_LOGIC matLogic)
        {
            this.fFlag = matLogic.fFlag;
            this.SrcFactorRGB = matLogic.SrcFactorRGB;
            this.DstFactorRGB = matLogic.DstFactorRGB;
            this.SrcFactorA = matLogic.SrcFactorA;
            this.DstFactorA = matLogic.DstFactorA;
            this.BlendColor = matLogic.BlendColor;
            this.BlendOp = matLogic.BlendOp;
            this.LogicOp = matLogic.LogicOp;
            this.AlphaFunc = matLogic.AlphaFunc;
            this.DepthFunc = matLogic.DepthFunc;
            this.AlphaRef = matLogic.AlphaRef;
            return this;
        }
    }
}
