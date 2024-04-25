using System;

public partial class AppMain
{
    public class GMS_FIX_PART_VIRTUAL_PAD
    {
        public GMS_COCKPIT_2D_WORK[] sub_parts = new GMS_COCKPIT_2D_WORK[4];
        public float[] pause_icon_frame = new float[2];
        public GMS_FIX_PART_WORK part_work;

        public GMS_FIX_PART_VIRTUAL_PAD()
        {
            this.part_work = new GMS_FIX_PART_WORK(this);
        }

        public static explicit operator GMS_FIX_PART_WORK(
          GMS_FIX_PART_VIRTUAL_PAD p)
        {
            return p.part_work;
        }

        public void Clear()
        {
            this.part_work.Clear();
            this.pause_icon_frame[0] = this.pause_icon_frame[1] = 0.0f;
            Array.Clear(sub_parts, 0, this.sub_parts.Length);
        }
    }
}
