// Decompiled with JetBrains decompiler
// Type: GLNormalData
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using mpp;

public class GLNormalData : GLVector3Data, OpenGL.GLVertexData
{
    protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1]
    {
    OpenGL.GLVertexElementType.Normal
    };

    public GLNormalData(ByteBuffer buffer, int size, uint type, int stride, int elCount)
      : base(buffer, size, type, stride, elCount)
    {
    }

    public OpenGL.GLVertexElementType[] DataComponents => this.compType_;

    public virtual void ExtractTo(OpenGL.Vertex[] dst, int count)
    {
        for (int index = 0; index < count; ++index)
            dst[index].Normal = this.data_[index];
    }

    public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
    {
        throw new InvalidOperationException();
    }
}
