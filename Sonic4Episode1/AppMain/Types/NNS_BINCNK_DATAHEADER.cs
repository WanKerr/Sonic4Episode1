using System.IO;

public partial class AppMain
{
    public class NNS_BINCNK_DATAHEADER
    {
        public uint Id;
        public int OfsNextId;
        public int OfsMainData;
        public int Version;

        public static NNS_BINCNK_DATAHEADER Read(BinaryReader reader)
        {
            return new NNS_BINCNK_DATAHEADER()
            {
                Id = reader.ReadUInt32(),
                OfsNextId = reader.ReadInt32(),
                OfsMainData = reader.ReadInt32(),
                Version = reader.ReadInt32()
            };
        }
    }
}
