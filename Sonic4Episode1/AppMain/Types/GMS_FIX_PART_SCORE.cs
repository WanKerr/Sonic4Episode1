using System;

public partial class AppMain
{
    public class GMS_FIX_PART_SCORE
    {
        public GMS_COCKPIT_2D_WORK[] sub_parts = new GMS_COCKPIT_2D_WORK[10];
        public int[] digit_list = new int[9];
        public readonly GMS_FIX_PART_WORK part_work;

        public GMS_FIX_PART_SCORE()
        {
            this.part_work = new GMS_FIX_PART_WORK(this);
        }

        public void Clear()
        {
            this.part_work.Clear();
            Array.Clear(sub_parts, 0, this.sub_parts.Length);
            Array.Clear(digit_list, 0, this.digit_list.Length);
        }

        public static explicit operator GMS_FIX_PART_WORK(
          GMS_FIX_PART_SCORE pScore)
        {
            return pScore.part_work;
        }
    }
}
