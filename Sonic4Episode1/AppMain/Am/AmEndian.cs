public partial class AppMain
{
    private static uint _amConvertEndian32(uint data)
    {
        return (uint)((int)((data & 0xFF000000U) >> 24) | (int)((data & 16711680U) >> 8) | ((int)data & 65280) << 8 | ((int)data & byte.MaxValue) << 24);
    }

    private static ushort _amConvertEndian16(ushort data)
    {
        return (ushort)(data >> 8 & byte.MaxValue | data << 8);
    }

    private static ulong _amConvertEndian64(ulong data)
    {
        return (ulong)((long)(data >> 56) & byte.MaxValue | ((long)(data >> 48) & byte.MaxValue) << 8 | ((long)(data >> 40) & byte.MaxValue) << 16 | ((long)(data >> 32) & byte.MaxValue) << 24 | ((long)(data >> 24) & byte.MaxValue) << 32 | ((long)(data >> 16) & byte.MaxValue) << 40 | ((long)(data >> 8) & byte.MaxValue) << 48 | ((long)data & byte.MaxValue) << 56);
    }

    private static uint _amConvertEndian(uint data)
    {
        return _amConvertEndian32(data);
    }

    private static int _amConvertEndian(int data)
    {
        return (int)_amConvertEndian32((uint)data);
    }

    private static ushort _amConvertEndian(ushort data)
    {
        return _amConvertEndian16(data);
    }

    private static short _amConvertEndian(short data)
    {
        mppAssertNotImpl();
        return (short)_amConvertEndian16((ushort)data);
    }

    private static ulong _amConvertEndian(ulong data)
    {
        return _amConvertEndian64(data);
    }

    private static long _amConvertEndian(long data)
    {
        return (long)_amConvertEndian64((ulong)data);
    }

    private static void _amConvertEndian(ref uint data)
    {
        data = _amConvertEndian32(data);
    }

    private static void _amConvertEndian(ref int data)
    {
        data = (int)_amConvertEndian32((uint)data);
    }

    private static void _amConvertEndian(ref ushort data)
    {
        data = _amConvertEndian16(data);
    }

    private static void _amConvertEndian(ref short data)
    {
        data = (short)_amConvertEndian16((ushort)data);
    }

    private static void _amConvertEndian(ref ulong data)
    {
        data = _amConvertEndian64(data);
    }

    private static void _amConvertEndian(ref long data)
    {
        data = (long)_amConvertEndian64((ulong)data);
    }
}