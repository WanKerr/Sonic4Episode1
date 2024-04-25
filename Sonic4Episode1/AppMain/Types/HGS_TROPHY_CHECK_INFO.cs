public partial class AppMain
{
    public class HGS_TROPHY_CHECK_INFO
    {
        public int trophy_type;
        public uint trophy_id;
        public HGF_TROPHY_ACQUIRE_CHECK_FUNC acquire_check_func;

        public HGS_TROPHY_CHECK_INFO(
          int trophy,
          uint trophy_id,
          HGF_TROPHY_ACQUIRE_CHECK_FUNC acquire_check_func)
        {
            this.trophy_type = trophy;
            this.trophy_id = trophy_id;
            this.acquire_check_func = acquire_check_func;
        }

        public HGS_TROPHY_CHECK_INFO()
        {
        }
    }
}
