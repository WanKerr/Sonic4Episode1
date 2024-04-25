using Microsoft.Xna.Framework;
using mpp;

public partial class AppMain
{
    public class RGBA_U8_ColorData : OpenGL.GLVertexData
    {
        protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1]
        {
      OpenGL.GLVertexElementType.Color
        };
        private RGBA_U8[] data_;
        private int startIndex_;

        public RGBA_U8_ColorData()
        {
        }

        public RGBA_U8_ColorData(RGBA_U8[] data, int startIndex)
        {
            this.data_ = data;
            this.startIndex_ = startIndex;
        }

        public void Init(RGBA_U8[] data, int startIndex)
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
                dst[index].Color.A = this.data_[this.startIndex_ + index].a;
                dst[index].Color.R = this.data_[this.startIndex_ + index].r;
                dst[index].Color.G = this.data_[this.startIndex_ + index].g;
                dst[index].Color.B = this.data_[this.startIndex_ + index].b;
            }
        }

        public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
        {
            for (int index1 = 0; index1 < count; ++index1)
            {
                int index2 = index1 + dstOffset;
                int index3 = this.startIndex_ + index1;
                dst[index2].Color = new Color(data_[index3].r, data_[index3].g, data_[index3].b, (int)this.data_[index3].a);
            }
        }
    }
}
