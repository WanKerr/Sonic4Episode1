using System;

public partial class AppMain
{
    public class GMS_BOSS4_FLICKER_WORK
    {
        public readonly NNS_RGB color = new NNS_RGB();
        public readonly uint[] reserved = new uint[3];
        public bool is_active;
        public uint cycles;
        public uint interval_timer;
        public int cur_angle;
        public int add_timer;
        public float radius;
        public uint interval_flk;

        public void Clear()
        {
            this.is_active = false;
            this.cycles = 0U;
            this.interval_timer = 0U;
            this.cur_angle = 0;
            this.add_timer = 0;
            this.radius = 0.0f;
            this.interval_flk = 0U;
            this.color.Clear();
            Array.Clear(reserved, 0, this.reserved.Length);
        }
    }
}
