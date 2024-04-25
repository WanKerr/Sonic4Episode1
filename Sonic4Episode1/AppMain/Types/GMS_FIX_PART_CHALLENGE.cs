using System;

public partial class AppMain
{
    public class GMS_FIX_PART_CHALLENGE
    {
        public GMS_COCKPIT_2D_WORK[] sub_parts = new GMS_COCKPIT_2D_WORK[5];
        public int[] digit_list = new int[3];
        public GMS_FIX_PART_WORK part_work;

        public GMS_FIX_PART_CHALLENGE()
        {
            this.part_work = new GMS_FIX_PART_WORK(this);
        }

        public static explicit operator GMS_FIX_PART_WORK(
          GMS_FIX_PART_CHALLENGE p)
        {
            return p.part_work;
        }

        public void Clear()
        {
            this.part_work.Clear();
            Array.Clear(sub_parts, 0, this.sub_parts.Length);
            Array.Clear(digit_list, 0, this.digit_list.Length);
        }
    }
}
