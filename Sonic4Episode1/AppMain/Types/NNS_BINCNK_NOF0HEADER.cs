using System.IO;

public partial class AppMain
{
    public class NNS_BINCNK_NOF0HEADER
    {
        public uint Id;
        public int OfsNextId;
        public int nData;
        public int Pad;

        public static NNS_BINCNK_NOF0HEADER Read(BinaryReader reader)
        {
            return new NNS_BINCNK_NOF0HEADER()
            {
                Id = reader.ReadUInt32(),
                OfsNextId = reader.ReadInt32(),
                nData = reader.ReadInt32(),
                Pad = reader.ReadInt32()
            };
        }
    }
}
