public partial class AppMain
{
    public class GMS_BOSS5_ALARM_FADE_INFO
    {
        public uint fo_frame;
        public uint on_frame;
        public uint fi_frame;
        public uint off_frame;

        public GMS_BOSS5_ALARM_FADE_INFO(uint a, uint b, uint c, uint d)
        {
            this.fo_frame = a;
            this.on_frame = b;
            this.fi_frame = c;
            this.off_frame = d;
        }
    }
}
