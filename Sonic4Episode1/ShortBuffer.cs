﻿// Decompiled with JetBrains decompiler
// Type: ShortBuffer
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

public class ShortBuffer : ByteBuffer
{
    public ShortBuffer(ByteBuffer buffer)
      : base(buffer.data, buffer.offset)
    {
    }

    public short this[int index]
    {
        get => this.GetShort(index * 2);
        set => this.PutShort(value, index * 2);
    }
}
