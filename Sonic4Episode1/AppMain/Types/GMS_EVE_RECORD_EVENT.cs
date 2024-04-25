public partial class AppMain
{
    public class GMS_EVE_RECORD_EVENT
    {
        public byte[] byte_param = new byte[2];
        public byte pos_x;
        public byte pos_y;
        public ushort id;
        public ushort flag;
        public sbyte left;
        public sbyte top;
        public byte width;
        public byte height;

        public ushort word_param
        {
            get => (ushort)((uint)this.byte_param[1] << 8 | this.byte_param[0]);
            set
            {
                this.byte_param[0] = (byte)(value & (uint)byte.MaxValue);
                this.byte_param[1] = (byte)(value >> 8 & byte.MaxValue);
            }
        }
    }
}
