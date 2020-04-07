// Decompiled with JetBrains decompiler
// Type: GLBlendWeightData
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Microsoft.Xna.Framework;
using mpp;
using System;

public class GLBlendWeightData : OpenGL.GLVertexData
{
  protected OpenGL.GLVertexElementType[] compType_ = new OpenGL.GLVertexElementType[1]
  {
    OpenGL.GLVertexElementType.BlendWeight
  };
  protected readonly Vector4[] data_;

  public GLBlendWeightData(ByteBuffer buffer, int size, uint type, int stride, int elCount)
  {
    stride = stride == 0 ? OpenGL.SizeOf(type) * size : stride;
    this.data_ = new Vector4[elCount];
    int getOffset = 0;
    for (int index = 0; index < elCount; ++index)
    {
      float x = buffer.GetFloat(getOffset);
      float y = size > 1 ? buffer.GetFloat(getOffset + 4) : 0.0f;
      float z = size > 2 ? buffer.GetFloat(getOffset + 8) : 0.0f;
      float w = size > 3 ? buffer.GetFloat(getOffset + 12) : 0.0f;
      this.data_[index] = new Vector4(x, y, z, w);
      getOffset += stride;
    }
  }

  public OpenGL.GLVertexElementType[] DataComponents
  {
    get
    {
      return this.compType_;
    }
  }

  public int VertexCount
  {
    get
    {
      return this.data_.Length;
    }
  }

  public void ExtractTo(OpenGL.Vertex[] dst, int count)
  {
    for (int index = 0; index < count; ++index)
      dst[index].BlendWeight = this.data_[index];
  }

  public void ExtractTo(OpenGL.VertexPosTexColNorm[] dst, int dstOffset, int count)
  {
    throw new InvalidOperationException();
  }
}
