using System;

public partial class AppMain
{
    public class GMS_FIX_PART_RINGCOUNT
    {
        public GMS_COCKPIT_2D_WORK[] sub_parts = new GMS_COCKPIT_2D_WORK[4];
        public int[] digit_list = new int[3];
        public readonly GMS_FIX_PART_WORK part_work;

        public static explicit operator GMS_FIX_PART_WORK(
          GMS_FIX_PART_RINGCOUNT pObj)
        {
            return pObj.part_work;
        }

        public GMS_FIX_PART_RINGCOUNT()
        {
            this.part_work = new GMS_FIX_PART_WORK(this);
        }

        public void Clear()
        {
            this.part_work.Clear();
            Array.Clear(sub_parts, 0, this.sub_parts.Length);
            Array.Clear(digit_list, 0, this.digit_list.Length);
        }
    }
}
