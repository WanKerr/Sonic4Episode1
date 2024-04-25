using System.IO;
using mpp;

public partial class AppMain
{
    public class NNS_VTXARRAY_GL
    {
        public uint Type;
        public int Size;
        public uint DataType;
        public int Stride;
        public ByteBuffer Pointer;
        public OpenGL.GLVertexData Data;

        public NNS_VTXARRAY_GL()
        {
        }

        public NNS_VTXARRAY_GL(NNS_VTXARRAY_GL array)
        {
            this.Type = array.Type;
            this.Size = array.Size;
            this.DataType = array.DataType;
            this.Stride = array.Stride;
            this.Pointer = array.Pointer;
            this.Data = array.Data;
        }

        public static NNS_VTXARRAY_GL Read(
          BinaryReader reader,
          ByteBuffer vertexBuffer,
          uint vertexBufferOffset,
          int nVertex)
        {
            NNS_VTXARRAY_GL nnsVtxarrayGl = new NNS_VTXARRAY_GL();
            nnsVtxarrayGl.Type = reader.ReadUInt32();
            nnsVtxarrayGl.Size = reader.ReadInt32();
            nnsVtxarrayGl.DataType = reader.ReadUInt32();
            nnsVtxarrayGl.Stride = reader.ReadInt32();
            uint num = reader.ReadUInt32();
            if (num != 0U)
                nnsVtxarrayGl.Pointer = vertexBuffer + ((int)num - (int)vertexBufferOffset);
            return nnsVtxarrayGl;
        }
    }
}
