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
            float_buf[0] = value;
            Buffer.BlockCopy(float_buf, 0, dst, offset, 4);
        }

        public static void GetBytes(uint value, byte[] dst, int offset)
        {
            UInt32_buf[0] = value;
            Buffer.BlockCopy(UInt32_buf, 0, dst, offset, 4);
        }

        public static void GetBytes(int value, byte[] dst, int offset)
        {
            Int32_buf[0] = value;
            Buffer.BlockCopy(Int32_buf, 0, dst, offset, 4);
        }

        public static float Int32ToSingle(int value)
        {
            Int32_buf[0] = value;
            Buffer.BlockCopy(Int32_buf, 0, float_buf, 0, 4);
            return float_buf[0];
        }

        public static int SingleToInt32(float value)
        {
            float_buf[0] = value;
            Buffer.BlockCopy(float_buf, 0, Int32_buf, 0, 4);
            return Int32_buf[0];
        }

        public static float UInt32ToSingle(uint value)
        {
            UInt32_buf[0] = value;
            Buffer.BlockCopy(UInt32_buf, 0, float_buf, 0, 4);
            return float_buf[0];
        }

        public static uint SingleToUInt32(float value)
        {
            float_buf[0] = value;
            Buffer.BlockCopy(float_buf, 0, UInt32_buf, 0, 4);
            return UInt32_buf[0];
        }

        public static uint ToUInt32(byte[] src, int offset)
        {
            Buffer.BlockCopy(src, offset, UInt32_buf, 0, 4);
            return UInt32_buf[0];
        }

        public static int ToInt32(byte[] src, int offset)
        {
            Buffer.BlockCopy(src, offset, Int32_buf, 0, 4);
            return Int32_buf[0];
        }

        public static float ToSingle(byte[] src, int offset)
        {
            Buffer.BlockCopy(src, offset, float_buf, 0, 4);
            return float_buf[0];
        }
    }
}
