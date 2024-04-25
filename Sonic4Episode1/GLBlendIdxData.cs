// Decompiled with JetBrains decompiler
// Type: GLBlendIdxData
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using mpp;

public class GLBlendIdxData : OpenGL.GLVertexData
{
    protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1]
    {
    OpenGL.GLVertexElementType.BlendIndex
    };
    protected readonly Byte4[] data_;

    public GLBlendIdxData(ByteBuffer buffer, int size, uint type, int stride, int elCount)
    {
        stride = stride == 0 ? OpenGL.SizeOf(type) * size : stride;
        this.data_ = new Byte4[elCount];
        int index1 = 0;
        for (int index2 = 0; index2 < elCount; ++index2)
        {
            byte num1 = buffer[index1];
            byte num2 = size > 1 ? buffer[index1 + 1] : (byte)0;
            byte num3 = size > 2 ? buffer[index1 + 2] : (byte)0;
            byte num4 = size > 3 ? buffer[index1 + 3] : (byte)0;
            this.data_[index2] = new Byte4(num1, num2, num3, num4);
            index1 += stride;
        }
    }

    public OpenGL.GLVertexElementType[] DataComponents => this.compType_;

    public int VertexCount => this.data_.Length;

    public void ExtractTo(OpenGL.Vertex[] dst, int count)
    {
        for (int index = 0; index < count; ++index)
            dst[index].BlendIndices = this.data_[index];
    }

    public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
    {
        throw new InvalidOperationException();
    }
}
