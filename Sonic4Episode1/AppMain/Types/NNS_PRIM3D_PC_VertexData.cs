using Microsoft.Xna.Framework;
using mpp;

public partial class AppMain
{
    public class NNS_PRIM3D_PC_VertexData : OpenGL.GLVertexData
    {
        protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1];
        private NNS_PRIM3D_PC[] data_;
        private int startIndex_;

        public NNS_PRIM3D_PC_VertexData()
        {
        }

        public NNS_PRIM3D_PC_VertexData(NNS_PRIM3D_PC[] data, int startIndex)
        {
            this.data_ = data;
            this.startIndex_ = startIndex;
        }

        public void Init(NNS_PRIM3D_PC[] data, int startIndex)
        {
            this.data_ = data;
            this.startIndex_ = startIndex;
        }

        public OpenGL.GLVertexElementType[] DataComponents => this.compType_;

        public int VertexCount => this.data_.Length;

        public void ExtractTo(OpenGL.Vertex[] dst, int count)
        {
            for (int index = 0; index < count; ++index)
                dst[index].Position = (Vector3)this.data_[this.startIndex_ + index].Pos;
        }

        public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
        {
            for (int index = 0; index < count; ++index)
                dst[index + dstOffset].Position = (Vector3)this.data_[this.startIndex_ + index].Pos;
        }
    }
}
