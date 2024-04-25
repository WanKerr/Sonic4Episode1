using System.IO;

public partial class AppMain
{
    public class NNS_PRIMLIST_GL_DESC
    {
        public uint Mode;
        public int[] pCounts;
        public uint DataType;
        public UShortBuffer[] pIndices;
        public int nPrim;
        public int IndexBufferSize;
        public ByteBuffer pIndexBuffer;
        public uint BufferName;

        public static NNS_PRIMLIST_GL_DESC Read(BinaryReader reader, long data0Pos)
        {
            NNS_PRIMLIST_GL_DESC nnsPrimlistGlDesc = new NNS_PRIMLIST_GL_DESC();
            nnsPrimlistGlDesc.Mode = reader.ReadUInt32();
            uint num1 = reader.ReadUInt32();
            long position1 = reader.BaseStream.Position;
            reader.BaseStream.Seek(data0Pos + num1, SeekOrigin.Begin);
            nnsPrimlistGlDesc.pCounts = new int[1];
            nnsPrimlistGlDesc.pCounts[0] = reader.ReadInt32();
            reader.BaseStream.Seek(position1, SeekOrigin.Begin);
            nnsPrimlistGlDesc.DataType = reader.ReadUInt32();
            uint num2 = reader.ReadUInt32();
            nnsPrimlistGlDesc.nPrim = reader.ReadInt32();
            nnsPrimlistGlDesc.IndexBufferSize = reader.ReadInt32();
            uint num3 = reader.ReadUInt32();
            long position2 = reader.BaseStream.Position;
            reader.BaseStream.Seek(data0Pos + num3, SeekOrigin.Begin);
            byte[] numArray = new byte[nnsPrimlistGlDesc.IndexBufferSize];
            reader.Read(numArray, 0, nnsPrimlistGlDesc.IndexBufferSize);
            nnsPrimlistGlDesc.pIndexBuffer = ByteBuffer.Wrap(numArray);
            reader.BaseStream.Seek(position2, SeekOrigin.Begin);
            nnsPrimlistGlDesc.pIndices = new UShortBuffer[nnsPrimlistGlDesc.nPrim];
            long position3 = reader.BaseStream.Position;
            reader.BaseStream.Seek(data0Pos + num2, SeekOrigin.Begin);
            for (int index = 0; index < nnsPrimlistGlDesc.nPrim; ++index)
            {
                uint num4 = reader.ReadUInt32();
                nnsPrimlistGlDesc.pIndices[index] = (nnsPrimlistGlDesc.pIndexBuffer + ((int)num4 - (int)num3)).AsUShortBuffer();
            }
            reader.BaseStream.Seek(position3, SeekOrigin.Begin);
            nnsPrimlistGlDesc.BufferName = reader.ReadUInt32();
            return nnsPrimlistGlDesc;
        }

        public NNS_PRIMLIST_GL_DESC Assign(NNS_PRIMLIST_GL_DESC desc)
        {
            this.Mode = desc.Mode;
            this.pCounts = desc.pCounts;
            this.DataType = desc.DataType;
            this.pIndices = desc.pIndices;
            this.nPrim = desc.nPrim;
            this.IndexBufferSize = desc.IndexBufferSize;
            this.pIndexBuffer = desc.pIndexBuffer;
            this.BufferName = desc.BufferName;
            return this;
        }
    }
}
