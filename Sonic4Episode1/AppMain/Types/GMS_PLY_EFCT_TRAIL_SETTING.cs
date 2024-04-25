public partial class AppMain
{
    public class GMS_PLY_EFCT_TRAIL_SETTING
    {
        public float start_size;
        public float end_size;
        public float life;
        public float vanish_time;

        public GMS_PLY_EFCT_TRAIL_SETTING(
          float start_size,
          float end_size,
          float life,
          float vanish_time)
        {
            this.start_size = start_size;
            this.end_size = end_size;
            this.life = life;
            this.vanish_time = vanish_time;
        }
    }
}
