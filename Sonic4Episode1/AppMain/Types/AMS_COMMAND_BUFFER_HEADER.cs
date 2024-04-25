public partial class AppMain
{
    public class AMS_COMMAND_BUFFER_HEADER
    {
        public readonly uint[] system_flag = new uint[4];
        public readonly uint[] debug_flag = new uint[4];
        public ushort display_flag;
        public ushort regist_flag;
        public float icon_alpha;
    }
}
