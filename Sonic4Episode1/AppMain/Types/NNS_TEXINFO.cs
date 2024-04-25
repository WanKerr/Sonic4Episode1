using System.IO;

public partial class AppMain
{
    public class NNS_TEXINFO
    {
        public uint TexName;
        public uint GlobalIndex;
        public uint Bank;
        public uint Flag;

        public NNS_TEXINFO()
        {
        }

        public NNS_TEXINFO(NNS_TEXINFO pFrom)
        {
            this.TexName = pFrom.TexName;
            this.GlobalIndex = pFrom.GlobalIndex;
            this.Bank = pFrom.Bank;
            this.Flag = pFrom.Flag;
        }

        public static NNS_TEXINFO Read(BinaryReader reader)
        {
            return new NNS_TEXINFO()
            {
                TexName = reader.ReadUInt32(),
                GlobalIndex = reader.ReadUInt32(),
                Bank = reader.ReadUInt32(),
                Flag = reader.ReadUInt32()
            };
        }
    }
}
