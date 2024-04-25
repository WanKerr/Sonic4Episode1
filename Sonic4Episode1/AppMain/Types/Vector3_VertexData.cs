using Microsoft.Xna.Framework;
using mpp;

public partial class AppMain
{
    public class Vector3_VertexData : OpenGL.GLVertexData
    {
        protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1];
        private Vector3[] data_;

        public Vector3_VertexData(Vector3[] data)
        {
            this.data_ = data;
        }

        public OpenGL.GLVertexElementType[] DataComponents => this.compType_;

        public int VertexCount => this.data_.Length;

        public void ExtractTo(OpenGL.Vertex[] dst, int count)
        {
            for (int index = 0; index < count; ++index)
                dst[index].Position = this.data_[index];
        }

        public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
        {
            for (int index = 0; index < count; ++index)
                dst[index + dstOffset].Position = this.data_[index];
        }
    }
}
