// Decompiled with JetBrains decompiler
// Type: StreamExtensions
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;
using System.IO;

internal static class StreamExtensions
{
  private static byte[] buffer = new byte[16384];

  public static void CopyTo(this Stream self, Stream dst)
  {
    for (int count = Math.Min((int) self.Length - (int) self.Position, StreamExtensions.buffer.Length); count > 0; count = Math.Min((int) self.Length - (int) self.Position, StreamExtensions.buffer.Length))
    {
      self.Read(StreamExtensions.buffer, 0, count);
      dst.Write(StreamExtensions.buffer, 0, count);
    }
  }
}
