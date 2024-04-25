public partial class AppMain
{
    public class GMS_BOSS5_FLASH_SCREEN_WORK : IOBS_OBJECT_WORK
    {
        public readonly GMS_CMN_FLASH_SCR_WORK flash_work = new GMS_CMN_FLASH_SCR_WORK();
        public readonly GMS_EFFECT_COM_WORK efct_com;

        public GMS_BOSS5_FLASH_SCREEN_WORK()
        {
            this.efct_com = new GMS_EFFECT_COM_WORK(this);
        }

        public OBS_OBJECT_WORK Cast()
        {
            return this.efct_com.Cast();
        }
    }
}
