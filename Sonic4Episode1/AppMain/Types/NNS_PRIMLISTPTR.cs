using System.IO;

public partial class AppMain
{
    public class NNS_PRIMLISTPTR
    {
        public uint fType;
        public object pPrimList;

        public NNS_PRIMLISTPTR()
        {
        }

        public NNS_PRIMLISTPTR(NNS_PRIMLISTPTR primListPtr)
        {
            this.fType = primListPtr.fType;
            this.pPrimList = primListPtr.pPrimList;
        }

        public NNS_PRIMLISTPTR Assign(NNS_PRIMLISTPTR primListPtr)
        {
            this.fType = primListPtr.fType;
            this.pPrimList = primListPtr.pPrimList;
            return this;
        }

        public static NNS_PRIMLISTPTR Read(BinaryReader reader, long data0Pos)
        {
            NNS_PRIMLISTPTR nnsPrimlistptr = new NNS_PRIMLISTPTR();
            nnsPrimlistptr.fType = reader.ReadUInt32();
            uint num = reader.ReadUInt32();
            if (num != 0U)
            {
                long position = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num, SeekOrigin.Begin);
                nnsPrimlistptr.pPrimList = NNS_PRIMLIST_GL_DESC.Read(reader, data0Pos);
                reader.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return nnsPrimlistptr;
        }
    }
}
