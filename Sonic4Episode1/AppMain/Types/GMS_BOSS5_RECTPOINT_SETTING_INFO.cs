public partial class AppMain
{
    public class GMS_BOSS5_RECTPOINT_SETTING_INFO
    {
        public short[][] rect_size = New<short>(3, 4);
        public uint enable_bit_flag;

        public GMS_BOSS5_RECTPOINT_SETTING_INFO()
        {
        }

        public GMS_BOSS5_RECTPOINT_SETTING_INFO(uint bitFlag, short[][] rectsize)
        {
            this.enable_bit_flag = bitFlag;
            this.rect_size = rectsize;
        }
    }
}
