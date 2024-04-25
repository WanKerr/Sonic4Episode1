using System.IO;

public partial class AppMain
{
    public class NNS_TEXFILELIST
    {
        public int nTex;
        public NNS_TEXFILE[] pTexFileList;

        public static NNS_TEXFILELIST Read(BinaryReader reader, long data0Pos)
        {
            NNS_TEXFILELIST nnsTexfilelist = new NNS_TEXFILELIST()
            {
                nTex = reader.ReadInt32()
            };
            nnsTexfilelist.pTexFileList = new NNS_TEXFILE[nnsTexfilelist.nTex];
            uint num = reader.ReadUInt32();
            reader.BaseStream.Seek(data0Pos + num, SeekOrigin.Begin);
            for (int index = 0; index < nnsTexfilelist.nTex; ++index)
                nnsTexfilelist.pTexFileList[index] = NNS_TEXFILE.Read(reader, data0Pos);
            return nnsTexfilelist;
        }
    }
}
