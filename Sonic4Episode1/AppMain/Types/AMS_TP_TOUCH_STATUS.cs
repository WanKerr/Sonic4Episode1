public partial class AppMain
{
    public class AMS_TP_TOUCH_STATUS
    {
        public AMS_TP_TOUCH_CORE core = new AMS_TP_TOUCH_CORE();
        public ushort[] on = new ushort[2];
        public ushort[] prev = new ushort[2];
        public ushort[] push = new ushort[2];
        public ushort[] pull = new ushort[2];
        public ushort flag;
    }
}
