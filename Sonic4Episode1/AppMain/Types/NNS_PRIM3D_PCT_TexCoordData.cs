using mpp;

public partial class AppMain
{
    public class NNS_PRIM3D_PCT_TexCoordData : OpenGL.GLVertexData
    {
        protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1]
        {
      OpenGL.GLVertexElementType.TextureCoordinate0
        };
        private NNS_PRIM3D_PCT[] data_;
        private int startIndex_;

        public NNS_PRIM3D_PCT_TexCoordData()
        {
        }

        public NNS_PRIM3D_PCT_TexCoordData(NNS_PRIM3D_PCT[] data, int startIndex)
        {
            this.data_ = data;
            this.startIndex_ = startIndex;
        }

        public void Init(NNS_PRIM3D_PCT[] data, int startIndex)
        {
            this.data_ = data;
            this.startIndex_ = startIndex;
        }

        public OpenGL.GLVertexElementType[] DataComponents => this.compType_;

        public int VertexCount => this.data_.Length;

        public void ExtractTo(OpenGL.Vertex[] dst, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                dst[index].TextureCoordinate.X = this.data_[this.startIndex_ + index].Tex.u;
                dst[index].TextureCoordinate.Y = this.data_[this.startIndex_ + index].Tex.v;
            }
        }

        public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                dst[index + dstOffset].TextureCoordinate.X = this.data_[this.startIndex_ + index].Tex.u;
                dst[index + dstOffset].TextureCoordinate.Y = this.data_[this.startIndex_ + index].Tex.v;
            }
        }
    }
}
