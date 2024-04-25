using mpp;

public partial class AppMain
{
    public struct AOS_TVX_VERTEX
    {
        public float x;
        public float y;
        public float z;
        public uint c;
        public float u;
        public float v;

        public AOS_TVX_VERTEX(byte[] data, int offset)
        {
            this.x = MppBitConverter.ToSingle(data, offset);
            this.y = MppBitConverter.ToSingle(data, offset + 4);
            this.z = MppBitConverter.ToSingle(data, offset + 8);
            this.c = MppBitConverter.ToUInt32(data, offset + 12);
            this.u = MppBitConverter.ToSingle(data, offset + 16);
            this.v = MppBitConverter.ToSingle(data, offset + 20);
        }

        public static uint SizeBytes => 32;
    }
}
