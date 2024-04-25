// Decompiled with JetBrains decompiler
// Type: FloatBuffer
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

public class FloatBuffer : ByteBuffer
{
    public FloatBuffer(ByteBuffer buffer)
      : base(buffer.data, buffer.offset)
    {
    }

    public new float this[int index]
    {
        get => this.GetFloat(index * 4);
        set => this.PutFloat(value, index * 4);
    }
}
