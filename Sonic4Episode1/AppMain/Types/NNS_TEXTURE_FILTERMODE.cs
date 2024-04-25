using System.IO;

public partial class AppMain
{
    public class NNS_TEXTURE_FILTERMODE
    {
        public ushort MagFilter;
        public ushort MinFilter;
        public float Anisotropy;

        public static NNS_TEXTURE_FILTERMODE Read(BinaryReader reader)
        {
            return new NNS_TEXTURE_FILTERMODE()
            {
                MagFilter = reader.ReadUInt16(),
                MinFilter = reader.ReadUInt16(),
                Anisotropy = reader.ReadSingle()
            };
        }

        public NNS_TEXTURE_FILTERMODE Assign(NNS_TEXTURE_FILTERMODE filterMode)
        {
            this.MagFilter = filterMode.MagFilter;
            this.MinFilter = filterMode.MinFilter;
            this.Anisotropy = filterMode.Anisotropy;
            return this;
        }
    }
}
