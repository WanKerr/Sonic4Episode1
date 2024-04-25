using System.IO;

public partial class AppMain
{
    public class NNS_MATERIAL_GLES11_LOGIC
    {
        public uint fFlag;
        public ushort SrcFactor;
        public ushort DstFactor;
        public ushort BlendOp;
        public ushort LogicOp;
        public ushort AlphaFunc;
        public ushort DepthFunc;
        public float AlphaRef;

        public static NNS_MATERIAL_GLES11_LOGIC Read(BinaryReader reader)
        {
            return new NNS_MATERIAL_GLES11_LOGIC()
            {
                fFlag = reader.ReadUInt32(),
                SrcFactor = reader.ReadUInt16(),
                DstFactor = reader.ReadUInt16(),
                BlendOp = reader.ReadUInt16(),
                LogicOp = reader.ReadUInt16(),
                AlphaFunc = reader.ReadUInt16(),
                DepthFunc = reader.ReadUInt16(),
                AlphaRef = reader.ReadSingle()
            };
        }

        public NNS_MATERIAL_GLES11_LOGIC Assign(NNS_MATERIAL_GLES11_LOGIC logic)
        {
            this.fFlag = logic.fFlag;
            this.SrcFactor = logic.SrcFactor;
            this.DstFactor = logic.DstFactor;
            this.BlendOp = logic.BlendOp;
            this.LogicOp = logic.LogicOp;
            this.AlphaFunc = logic.AlphaFunc;
            this.DepthFunc = logic.DepthFunc;
            this.AlphaRef = logic.AlphaRef;
            return this;
        }
    }
}
