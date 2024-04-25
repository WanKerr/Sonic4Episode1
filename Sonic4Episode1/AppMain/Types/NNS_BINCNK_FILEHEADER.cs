using System.IO;

public partial class AppMain
{
    public class NNS_BINCNK_FILEHEADER
    {
        public uint Id;
        public int OfsNextId;
        public int nChunk;
        public int OfsData;
        public int SizeData;
        public int OfsNOF0;
        public int SizeNOF0;
        public int Version;

        public static NNS_BINCNK_FILEHEADER Read(BinaryReader reader)
        {
            return new NNS_BINCNK_FILEHEADER()
            {
                Id = reader.ReadUInt32(),
                OfsNextId = reader.ReadInt32(),
                nChunk = reader.ReadInt32(),
                OfsData = reader.ReadInt32(),
                SizeData = reader.ReadInt32(),
                OfsNOF0 = reader.ReadInt32(),
                SizeNOF0 = reader.ReadInt32(),
                Version = reader.ReadInt32()
            };
        }
    }
}
