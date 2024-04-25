using mpp;

public partial class AppMain
{
    public class NNS_PRIM3D_PCT_VertexData : OpenGL.GLVertexData
    {
        protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1];
        private NNS_PRIM3D_PCT[] data_;
        private int startIndex_;

        public NNS_PRIM3D_PCT_VertexData()
        {
        }

        public NNS_PRIM3D_PCT_VertexData(NNS_PRIM3D_PCT[] data, int startIndex)
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
                dst[index].Position.X = this.data_[this.startIndex_ + index].Pos.x;
                dst[index].Position.Y = this.data_[this.startIndex_ + index].Pos.y;
                dst[index].Position.Z = this.data_[this.startIndex_ + index].Pos.z;
            }
        }

        public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
        {
            for (int index = 0; index < count; ++index)
            {
                dst[index + dstOffset].Position.X = this.data_[this.startIndex_ + index].Pos.x;
                dst[index + dstOffset].Position.Y = this.data_[this.startIndex_ + index].Pos.y;
                dst[index + dstOffset].Position.Z = this.data_[this.startIndex_ + index].Pos.z;
            }
        }
    }
}
