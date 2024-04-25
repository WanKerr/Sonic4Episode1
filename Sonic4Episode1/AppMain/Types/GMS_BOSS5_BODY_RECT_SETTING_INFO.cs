public partial class AppMain
{
    public class GMS_BOSS5_BODY_RECT_SETTING_INFO
    {
        public GMS_BOSS5_RECTPOINT_SETTING_INFO[] point_setting_info = New<GMS_BOSS5_RECTPOINT_SETTING_INFO>(3);
        public int is_invincible;
        public int is_leakage;

        public GMS_BOSS5_BODY_RECT_SETTING_INFO(
          int invis,
          int leakage,
          GMS_BOSS5_RECTPOINT_SETTING_INFO[] info)
        {
            this.is_invincible = invis;
            this.is_leakage = leakage;
            this.point_setting_info = info;
        }
    }
}
