// Decompiled with JetBrains decompiler
// Type: GLVector3Data
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using Microsoft.Xna.Framework;
using mpp;

public abstract class GLVector3Data
{
    protected readonly Vector3[] data_;

    public GLVector3Data(ByteBuffer buffer, int size, uint type, int stride, int elCount)
    {
        stride = stride == 0 ? OpenGL.SizeOf(type) * size : stride;
        this.data_ = new Vector3[elCount];
        switch (type)
        {
            case 5121:
                if (size > 2)
                {
                    this.extract3ByteData(buffer, stride);
                    break;
                }
                this.extract2ByteData(buffer, stride);
                break;
            case 5123:
                if (size > 2)
                {
                    this.extract3UShortData(buffer, stride);
                    break;
                }
                this.extract2UShortData(buffer, stride);
                break;
            case 5126:
                if (size > 2)
                {
                    this.extract3FloatData(buffer, stride);
                    break;
                }
                this.extract2FloatData(buffer, stride);
                break;
        }
    }

    public int VertexCount => this.data_.Length;

    private void extract3FloatData(ByteBuffer buffer, int stride)
    {
        int length = this.data_.Length;
        int getOffset = 0;
        for (int index = 0; index < length; ++index)
        {
            float x = buffer.GetFloat(getOffset);
            float y = buffer.GetFloat(getOffset + 4);
            float z = buffer.GetFloat(getOffset + 8);
            this.data_[index] = new Vector3(x, y, z);
            getOffset += stride;
        }
    }

    private void extract2FloatData(ByteBuffer buffer, int stride)
    {
        int length = this.data_.Length;
        int getOffset = 0;
        for (int index = 0; index < length; ++index)
        {
            float x = buffer.GetFloat(getOffset);
            float y = buffer.GetFloat(getOffset + 4);
            this.data_[index] = new Vector3(x, y, 0.0f);
            getOffset += stride;
        }
    }

    private void extract3UShortData(ByteBuffer buffer, int stride)
    {
        int length = this.data_.Length;
        int getOffset = 0;
        for (int index = 0; index < length; ++index)
        {
            ushort num1 = buffer.GetUShort(getOffset);
            ushort num2 = buffer.GetUShort(getOffset + 2);
            ushort num3 = buffer.GetUShort(getOffset + 4);
            this.data_[index] = new Vector3(num1, num2, num3);
            getOffset += stride;
        }
    }

    private void extract2UShortData(ByteBuffer buffer, int stride)
    {
        int length = this.data_.Length;
        int getOffset = 0;
        for (int index = 0; index < length; ++index)
        {
            ushort num1 = buffer.GetUShort(getOffset);
            ushort num2 = buffer.GetUShort(getOffset + 2);
            this.data_[index] = new Vector3(num1, num2, 0.0f);
            getOffset += stride;
        }
    }

    private void extract3ByteData(ByteBuffer buffer, int stride)
    {
        int length = this.data_.Length;
        int index1 = 0;
        for (int index2 = 0; index2 < length; ++index2)
        {
            byte num1 = buffer[index1];
            byte num2 = buffer[index1 + 1];
            byte num3 = buffer[index1 + 2];
            this.data_[index2] = new Vector3(num1, num2, num3);
            index1 += stride;
        }
    }

    private void extract2ByteData(ByteBuffer buffer, int stride)
    {
        int length = this.data_.Length;
        int index1 = 0;
        for (int index2 = 0; index2 < length; ++index2)
        {
            byte num1 = buffer[index1];
            byte num2 = buffer[index1 + 1];
            this.data_[index2] = new Vector3(num1, num2, 0.0f);
            index1 += stride;
        }
    }
}
