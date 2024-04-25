using System.IO;

public partial class AppMain
{
    public class NNS_TEXTURE_GLES11_COMBINE
    {
        public ushort CombineRGB;
        public ushort Source0RGB;
        public ushort Operand0RGB;
        public ushort Source1RGB;
        public ushort Operand1RGB;
        public ushort Source2RGB;
        public ushort Operand2RGB;
        public ushort CombineAlpha;
        public ushort Source0Alpha;
        public ushort Operand0Alpha;
        public ushort Source1Alpha;
        public ushort Operand1Alpha;
        public ushort Source2Alpha;
        public ushort Operand2Alpha;
        public NNS_RGBA EnvColor;

        public static NNS_TEXTURE_GLES11_COMBINE Read(BinaryReader reader)
        {
            return new NNS_TEXTURE_GLES11_COMBINE()
            {
                CombineRGB = reader.ReadUInt16(),
                Source0RGB = reader.ReadUInt16(),
                Operand0RGB = reader.ReadUInt16(),
                Source1RGB = reader.ReadUInt16(),
                Operand1RGB = reader.ReadUInt16(),
                Source2RGB = reader.ReadUInt16(),
                Operand2RGB = reader.ReadUInt16(),
                CombineAlpha = reader.ReadUInt16(),
                Source0Alpha = reader.ReadUInt16(),
                Operand0Alpha = reader.ReadUInt16(),
                Source1Alpha = reader.ReadUInt16(),
                Operand1Alpha = reader.ReadUInt16(),
                Source2Alpha = reader.ReadUInt16(),
                Operand2Alpha = reader.ReadUInt16(),
                EnvColor = {
          r = reader.ReadSingle(),
          g = reader.ReadSingle(),
          b = reader.ReadSingle(),
          a = reader.ReadSingle()
        }
            };
        }

        public NNS_TEXTURE_GLES11_COMBINE Assign(
          NNS_TEXTURE_GLES11_COMBINE combine)
        {
            this.CombineRGB = combine.CombineRGB;
            this.Source0RGB = combine.Source0RGB;
            this.Operand0RGB = combine.Operand0RGB;
            this.Source1RGB = combine.Source1RGB;
            this.Operand1RGB = combine.Operand1RGB;
            this.Source2RGB = combine.Source2RGB;
            this.Operand2RGB = combine.Operand2RGB;
            this.CombineAlpha = combine.CombineAlpha;
            this.Source0Alpha = combine.Source0Alpha;
            this.Operand0Alpha = combine.Operand0Alpha;
            this.Source1Alpha = combine.Source1Alpha;
            this.Operand1Alpha = combine.Operand1Alpha;
            this.Source2Alpha = combine.Source2Alpha;
            this.Operand2Alpha = combine.Operand2Alpha;
            this.EnvColor = combine.EnvColor;
            return this;
        }
    }
}
