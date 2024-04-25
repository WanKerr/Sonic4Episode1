public partial class AppMain
{
    public struct AOS_ACT_COL : IClearable
    {
        public byte a;
        public byte b;
        public byte g;
        public byte r;

        public void Clear()
        {
            this.a = this.b = this.g = this.r = 0;
        }

        public uint c
        {
            get => (uint)(r << 24 | g << 16 | b << 8) | a;
            set
            {
                uint num = value;
                this.r = (byte)(num >> 24 & byte.MaxValue);
                this.g = (byte)(num >> 16 & byte.MaxValue);
                this.b = (byte)(num >> 8 & byte.MaxValue);
                this.a = (byte)(num & byte.MaxValue);
            }
        }
    }
}
