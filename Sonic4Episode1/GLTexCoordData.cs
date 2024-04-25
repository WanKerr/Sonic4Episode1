// Decompiled with JetBrains decompiler
// Type: GLTexCoordData
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Microsoft.Xna.Framework;
using mpp;

public class GLTexCoordData : OpenGL.GLVertexData
{
    protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1]
    {
    OpenGL.GLVertexElementType.TextureCoordinate0
    };
    protected readonly Vector2[] data_;

    public GLTexCoordData(ByteBuffer buffer, int size, uint type, int stride, int elCount)
    {
        stride = stride == 0 ? OpenGL.SizeOf(type) * size : stride;
        this.data_ = new Vector2[elCount];
        switch (type)
        {
            case 5121:
                this.extractByteData(buffer, stride);
                break;
            case 5123:
                this.extractUShortData(buffer, stride);
                break;
            case 5126:
                this.extractFloatData(buffer, stride);
                break;
        }
    }

    public OpenGL.GLVertexElementType[] DataComponents => this.compType_;

    public int VertexCount => this.data_.Length;

    public void ExtractTo(OpenGL.Vertex[] dst, int count)
    {
        for (int index = 0; index < count; ++index)
            dst[index].TextureCoordinate = this.data_[index];
    }

    public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
    {
        for (int index = 0; index < count; ++index)
            dst[index + dstOffset].TextureCoordinate = this.data_[index];
    }

    private void extractFloatData(ByteBuffer buffer, int stride)
    {
        int length = this.data_.Length;
        int getOffset = 0;
        for (int index = 0; index < length; ++index)
        {
            float x = buffer.GetFloat(getOffset);
            float y = buffer.GetFloat(getOffset + 4);
            this.data_[index] = new Vector2(x, y);
            getOffset += stride;
        }
    }

    private void extractUShortData(ByteBuffer buffer, int stride)
    {
        int length = this.data_.Length;
        int getOffset = 0;
        for (int index = 0; index < length; ++index)
        {
            ushort num1 = buffer.GetUShort(getOffset);
            ushort num2 = buffer.GetUShort(getOffset + 2);
            this.data_[index] = new Vector2(num1, num2);
            getOffset += stride;
        }
    }

    private void extractByteData(ByteBuffer buffer, int stride)
    {
        int length = this.data_.Length;
        int index1 = 0;
        for (int index2 = 0; index2 < length; ++index2)
        {
            byte num1 = buffer[index1];
            byte num2 = buffer[index1 + 1];
            this.data_[index2] = new Vector2(num1, num2);
            index1 += stride;
        }
    }
}
