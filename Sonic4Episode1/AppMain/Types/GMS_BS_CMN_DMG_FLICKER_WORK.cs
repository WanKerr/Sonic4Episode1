public partial class AppMain
{
    public class GMS_BS_CMN_DMG_FLICKER_WORK : IClearable
    {
        public int is_active;
        public uint cycles;
        public uint interval_timer;
        public int cur_angle;
        public float radius;

        public void Clear()
        {
            this.is_active = 0;
            this.cycles = this.interval_timer = 0U;
            this.cur_angle = 0;
            this.radius = 0.0f;
        }
    }
}
