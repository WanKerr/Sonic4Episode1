public partial class AppMain
{
    private class AMS_IPHONE_TP_DATA
    {
        public int id;
        public ushort touch;
        public ushort validity;
        public ushort x;
        public ushort y;

        internal void Assign(AMS_IPHONE_TP_DATA data)
        {
            this.touch = data.touch;
            this.validity = data.validity;
            this.x = data.x;
            this.y = data.y;
        }
    }
}
