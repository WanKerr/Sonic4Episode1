using System.IO;
using System.Text;

public partial class AppMain
{
    public class NNS_TEXFILE
    {
        public uint fType;
        public string Filename;
        public ushort MinFilter;
        public ushort MagFilter;
        public uint GlobalIndex;
        public uint Bank;

        public static NNS_TEXFILE Read(BinaryReader reader, long data0Pos)
        {
            NNS_TEXFILE nnsTexfile = new NNS_TEXFILE();
            nnsTexfile.fType = reader.ReadUInt32();
            uint num1 = reader.ReadUInt32();
            nnsTexfile.MinFilter = reader.ReadUInt16();
            nnsTexfile.MagFilter = reader.ReadUInt16();
            nnsTexfile.GlobalIndex = reader.ReadUInt32();
            nnsTexfile.Bank = reader.ReadUInt32();
            if (num1 != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num1, SeekOrigin.Begin);
                StringBuilder stringBuilder = new StringBuilder();
                byte num2;
                while ((num2 = reader.ReadByte()) != 0)
                    stringBuilder.Append((char)num2);
                nnsTexfile.Filename = stringBuilder.ToString().ToUpper();
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return nnsTexfile;
        }
    }
}
