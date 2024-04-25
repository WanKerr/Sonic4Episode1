using System;

public partial class AppMain
{
    public class GMS_FIX_PART_TIMER
    {
        public GMS_COCKPIT_2D_WORK[] sub_parts = new GMS_COCKPIT_2D_WORK[8];
        public int[] digit_list = new int[5];
        public ushort[] reserved = new ushort[1];
        public readonly GMS_FIX_PART_WORK part_work;
        public uint flag;
        public float digit_frame_ofst;
        public float deco_char_frame_ofst;
        public ushort cur_sec;
        public float fade_ratio;
        public float scale_ratio;
        public uint flash_act_phase;

        public GMS_FIX_PART_TIMER()
        {
            this.part_work = new GMS_FIX_PART_WORK(this);
        }

        public static explicit operator GMS_FIX_PART_WORK(GMS_FIX_PART_TIMER p)
        {
            return p.part_work;
        }

        public void Clear()
        {
            this.part_work.Clear();
            Array.Clear(sub_parts, 0, this.sub_parts.Length);
            this.flag = 0U;
            Array.Clear(digit_list, 0, this.digit_list.Length);
            this.digit_frame_ofst = this.deco_char_frame_ofst = 0.0f;
            this.cur_sec = 0;
            this.reserved[0] = 0;
            this.fade_ratio = this.scale_ratio = 0.0f;
            this.flash_act_phase = 0U;
        }
    }
}
