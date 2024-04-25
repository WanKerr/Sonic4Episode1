public partial class AppMain
{
    public class GMS_EFCT_ZONE_CREATE_INFO
    {
        public GMS_EFCT_ZONE_CREATE_PARAM[] zone_create_param;
        public int num;

        public GMS_EFCT_ZONE_CREATE_INFO(
          GMS_EFCT_ZONE_CREATE_PARAM[] zone_create_param,
          int num)
        {
            this.zone_create_param = zone_create_param;
            this.num = num;
        }
    }
}
