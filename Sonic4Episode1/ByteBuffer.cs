// Decompiled with JetBrains decompiler
// Type: ByteBuffer
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using mpp;

public class ByteBuffer
{
    internal byte[] data;
    internal int offset;

    public byte[] Data => this.data;

    public int Offset
    {
        get => this.offset;
        set
        {
            this.offset = value;
            this.data = null;
        }
    }

    public void SetOffset(int iOffset)
    {
        this.offset = iOffset;
    }

    public static ByteBuffer Wrap(byte[] data)
    {
        return new ByteBuffer(data, 0);
    }

    protected ByteBuffer(byte[] _data, int _offset)
    {
        this.data = _data;
        this.offset = _offset;
    }

    public static ByteBuffer operator +(ByteBuffer buffer, int i)
    {
        return new ByteBuffer(buffer.data, buffer.offset + i);
    }

    public static ByteBuffer operator -(ByteBuffer buffer, int i)
    {
        return new ByteBuffer(buffer.data, buffer.offset - i);
    }

    public static bool operator <(ByteBuffer buffer1, ByteBuffer buffer2)
    {
        if (buffer1.data != buffer2.data)
            throw new InvalidOperationException();
        return buffer1.offset < buffer2.offset;
    }

    public static int operator -(ByteBuffer buffer1, ByteBuffer buffer2)
    {
        if (buffer1.data != buffer2.data)
            throw new InvalidOperationException();
        return buffer1.offset - buffer2.offset;
    }

    public static bool operator >(ByteBuffer buffer1, ByteBuffer buffer2)
    {
        if (buffer1.data != buffer2.data)
            throw new InvalidOperationException();
        return buffer1.offset > buffer2.offset;
    }

    public byte this[int i]
    {
        get => this.data[this.offset + i];
        set => this.data[this.offset + i] = value;
    }

    public int GetInt(int getOffset)
    {
        return BitConverter.ToInt32(this.data, this.offset + getOffset);
    }

    public void PutInt(int n, int putOffset)
    {
        MppBitConverter.GetBytes(n, this.data, this.offset + putOffset);
    }

    public IntBuffer AsIntBuffer()
    {
        return new IntBuffer(this);
    }

    public uint GetUInt(int getOffset)
    {
        return BitConverter.ToUInt32(this.data, this.offset + getOffset);
    }

    public void PutUInt(uint n, int putOffset)
    {
        Array.Copy(BitConverter.GetBytes(n), 0, data, this.offset + putOffset, 4);
    }

    public UIntBuffer AsUIntBuffer()
    {
        return new UIntBuffer(this);
    }

    public short GetShort(int getOffset)
    {
        return BitConverter.ToInt16(this.data, this.offset + getOffset);
    }

    public void PutShort(short n, int putOffset)
    {
        Array.Copy(BitConverter.GetBytes(n), 0, data, this.offset + putOffset, 2);
    }

    public ShortBuffer AsShortBuffer()
    {
        return new ShortBuffer(this);
    }

    public ushort GetUShort(int getOffset)
    {
        return BitConverter.ToUInt16(this.data, this.offset + getOffset);
    }

    public void PutUShort(ushort n, int putOffset)
    {
        Array.Copy(BitConverter.GetBytes(n), 0, data, this.offset + putOffset, 2);
    }

    public UShortBuffer AsUShortBuffer()
    {
        return new UShortBuffer(this);
    }

    public float GetFloat(int getOffset)
    {
        return BitConverter.ToSingle(this.data, this.offset + getOffset);
    }

    public void PutFloat(float n, int putOffset)
    {
        MppBitConverter.GetBytes(n, this.data, this.offset + putOffset);
    }

    public FloatBuffer AsFloatBuffer()
    {
        return new FloatBuffer(this);
    }

    public byte[] GetBytesCopy(int size)
    {
        if (this.offset + size > this.data.Length)
            throw new ArgumentException();
        byte[] numArray = new byte[size];
        Array.Copy(data, this.offset, numArray, 0, size);
        return numArray;
    }
}
