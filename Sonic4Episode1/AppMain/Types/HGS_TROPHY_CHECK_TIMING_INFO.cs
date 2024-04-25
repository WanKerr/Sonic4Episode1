public partial class AppMain
{
    public class HGS_TROPHY_CHECK_TIMING_INFO
    {
        public HGS_TROPHY_CHECK_INFO[] check_info_tbl;
        public int num;

        public HGS_TROPHY_CHECK_TIMING_INFO(HGS_TROPHY_CHECK_INFO[] tbl, int num)
        {
            this.check_info_tbl = tbl;
            this.num = num;
        }

        public HGS_TROPHY_CHECK_TIMING_INFO()
        {
        }
    }
}
