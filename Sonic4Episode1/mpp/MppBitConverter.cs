// Decompiled with JetBrains decompiler
// Type: mpp.MppBitConverter
// Assembly: Sonic4 ep I, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 093CE2FC-33E2-4332-B0FE-1EA1E44D3AE7
// Assembly location: C:\Users\wamwo\Documents\GitHub\Sonic4Ep1-WP7-Decompilation\XAP\Sonic4 ep I.dll

using System;

namespace mpp
{
  internal class MppBitConverter
  {
    private static readonly int[] Int32_buf = new int[1];
    private static readonly uint[] UInt32_buf = new uint[1];
    private static readonly float[] float_buf = new float[1];

    public static void GetBytes(float value, byte[] dst, int offset)
    {
      MppBitConverter.float_buf[0] = value;
      Buffer.BlockCopy((Array) MppBitConverter.float_buf, 0, (Array) dst, offset, 4);
    }

    public static void GetBytes(uint value, byte[] dst, int offset)
    {
      MppBitConverter.UInt32_buf[0] = value;
      Buffer.BlockCopy((Array) MppBitConverter.UInt32_buf, 0, (Array) dst, offset, 4);
    }

    public static void GetBytes(int value, byte[] dst, int offset)
    {
      MppBitConverter.Int32_buf[0] = value;
      Buffer.BlockCopy((Array) MppBitConverter.Int32_buf, 0, (Array) dst, offset, 4);
    }

    public static float Int32ToSingle(int value)
    {
      MppBitConverter.Int32_buf[0] = value;
      Buffer.BlockCopy((Array) MppBitConverter.Int32_buf, 0, (Array) MppBitConverter.float_buf, 0, 4);
      return MppBitConverter.float_buf[0];
    }

    public static int SingleToInt32(float value)
    {
      MppBitConverter.float_buf[0] = value;
      Buffer.BlockCopy((Array) MppBitConverter.float_buf, 0, (Array) MppBitConverter.Int32_buf, 0, 4);
      return MppBitConverter.Int32_buf[0];
    }

    public static float UInt32ToSingle(uint value)
    {
      MppBitConverter.UInt32_buf[0] = value;
      Buffer.BlockCopy((Array) MppBitConverter.UInt32_buf, 0, (Array) MppBitConverter.float_buf, 0, 4);
      return MppBitConverter.float_buf[0];
    }

    public static uint SingleToUInt32(float value)
    {
      MppBitConverter.float_buf[0] = value;
      Buffer.BlockCopy((Array) MppBitConverter.float_buf, 0, (Array) MppBitConverter.UInt32_buf, 0, 4);
      return MppBitConverter.UInt32_buf[0];
    }

    public static uint ToUInt32(byte[] src, int offset)
    {
      Buffer.BlockCopy((Array) src, offset, (Array) MppBitConverter.UInt32_buf, 0, 4);
      return MppBitConverter.UInt32_buf[0];
    }

    public static int ToInt32(byte[] src, int offset)
    {
      Buffer.BlockCopy((Array) src, offset, (Array) MppBitConverter.Int32_buf, 0, 4);
      return MppBitConverter.Int32_buf[0];
    }

    public static float ToSingle(byte[] src, int offset)
    {
      Buffer.BlockCopy((Array) src, offset, (Array) MppBitConverter.float_buf, 0, 4);
      return MppBitConverter.float_buf[0];
    }
  }
}
