public partial class AppMain
{
    public class GMS_BOSS5_TURRET_SEQ_VUL_SHOT_INFO
    {
        public int life_threshold;
        public uint wait_time;
        public int shot_num;

        public GMS_BOSS5_TURRET_SEQ_VUL_SHOT_INFO(int a, uint b, int c)
        {
            this.life_threshold = a;
            this.wait_time = b;
            this.shot_num = c;
        }
    }
}
