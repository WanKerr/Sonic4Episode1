using System.IO;

public partial class AppMain
{
    public class NNS_VTXLIST_GL_DESC
    {
        public uint Type;
        public int nVertex;
        public int nArray;
        public NNS_VTXARRAY_GL[] pArray;
        public int VertexBufferSize;
        public ByteBuffer pVertexBuffer;
        public int nMatrix;
        public ushort[] pMatrixIndices;
        public uint BufferName;

        public static NNS_VTXLIST_GL_DESC Read(BinaryReader reader, long data0Pos)
        {
            NNS_VTXLIST_GL_DESC nnsVtxlistGlDesc = new NNS_VTXLIST_GL_DESC();
            nnsVtxlistGlDesc.Type = reader.ReadUInt32();
            nnsVtxlistGlDesc.nVertex = reader.ReadInt32();
            nnsVtxlistGlDesc.nArray = reader.ReadInt32();
            uint num1 = reader.ReadUInt32();
            nnsVtxlistGlDesc.VertexBufferSize = reader.ReadInt32();
            uint vertexBufferOffset = reader.ReadUInt32();
            long position1 = reader.BaseStream.Position;
            reader.BaseStream.Seek(data0Pos + vertexBufferOffset, SeekOrigin.Begin);
            byte[] numArray = new byte[nnsVtxlistGlDesc.VertexBufferSize];
            reader.Read(numArray, 0, numArray.Length);
            nnsVtxlistGlDesc.pVertexBuffer = ByteBuffer.Wrap(numArray);
            reader.BaseStream.Seek(position1, SeekOrigin.Begin);
            if (num1 != 0U)
            {
                long position2 = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num1, SeekOrigin.Begin);
                nnsVtxlistGlDesc.pArray = new NNS_VTXARRAY_GL[nnsVtxlistGlDesc.nArray];
                for (int index = 0; index < nnsVtxlistGlDesc.nArray; ++index)
                    nnsVtxlistGlDesc.pArray[index] = NNS_VTXARRAY_GL.Read(reader, nnsVtxlistGlDesc.pVertexBuffer, vertexBufferOffset, nnsVtxlistGlDesc.nVertex);
                reader.BaseStream.Seek(position2, SeekOrigin.Begin);
            }
            nnsVtxlistGlDesc.nMatrix = reader.ReadInt32();
            uint num2 = reader.ReadUInt32();
            if (num2 != 0U)
            {
                long position2 = reader.BaseStream.Position;
                reader.BaseStream.Seek(data0Pos + num2, SeekOrigin.Begin);
                nnsVtxlistGlDesc.pMatrixIndices = new ushort[nnsVtxlistGlDesc.nMatrix];
                for (int index = 0; index < nnsVtxlistGlDesc.nMatrix; ++index)
                    nnsVtxlistGlDesc.pMatrixIndices[index] = reader.ReadUInt16();
                reader.BaseStream.Seek(position2, SeekOrigin.Begin);
            }
            nnsVtxlistGlDesc.BufferName = reader.ReadUInt32();
            return nnsVtxlistGlDesc;
        }

        public NNS_VTXLIST_GL_DESC Assign(NNS_VTXLIST_GL_DESC desc)
        {
            this.Type = desc.Type;
            this.nVertex = desc.nVertex;
            this.nArray = desc.nArray;
            this.pArray = desc.pArray;
            this.VertexBufferSize = desc.VertexBufferSize;
            this.pVertexBuffer = desc.pVertexBuffer;
            this.nMatrix = desc.nMatrix;
            this.pMatrixIndices = desc.pMatrixIndices;
            this.BufferName = desc.BufferName;
            return this;
        }
    }
}
