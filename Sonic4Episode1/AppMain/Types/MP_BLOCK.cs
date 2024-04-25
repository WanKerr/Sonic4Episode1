public partial class AppMain
{
    public struct MP_BLOCK
    {
        public ushort id;
        public ushort rot;
        public ushort flip_h;
        public ushort flip_v;

        public MP_BLOCK(ushort bitFieldValue)
        {
            this.id = (ushort)(bitFieldValue & 4095U);
            this.rot = (ushort)(bitFieldValue >> 12 & 3);
            this.flip_h = (ushort)(bitFieldValue >> 14 & 1);
            this.flip_v = (ushort)(bitFieldValue >> 15 & 1);
        }
    }
}
