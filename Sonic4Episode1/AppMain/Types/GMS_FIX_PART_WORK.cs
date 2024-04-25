public partial class AppMain
{
    public class GMS_FIX_PART_WORK
    {
        public readonly object holder;
        public GMS_FIX_MGR_WORK parent_mgr;
        public int part_type;
        public uint flag;
        public uint blink_timer;
        public uint blink_on_time;
        public uint blink_off_time;
        public MPP_VOID_GMS_FIX_PART_WORK proc_update;
        public MPP_VOID_GMS_FIX_PART_WORK proc_disp;

        public GMS_FIX_PART_WORK(object _holder)
        {
            this.holder = _holder;
        }

        public GMS_FIX_PART_WORK()
        {
        }

        public static explicit operator GMS_FIX_PART_VIRTUAL_PAD(
          GMS_FIX_PART_WORK p)
        {
            return (GMS_FIX_PART_VIRTUAL_PAD)p.holder;
        }

        public static explicit operator GMS_FIX_PART_SCORE(GMS_FIX_PART_WORK p)
        {
            return (GMS_FIX_PART_SCORE)p.holder;
        }

        public static explicit operator GMS_FIX_PART_TIMER(GMS_FIX_PART_WORK p)
        {
            return (GMS_FIX_PART_TIMER)p.holder;
        }

        public static explicit operator GMS_FIX_PART_RINGCOUNT(
          GMS_FIX_PART_WORK p)
        {
            return (GMS_FIX_PART_RINGCOUNT)p.holder;
        }

        public static explicit operator GMS_FIX_PART_CHALLENGE(
          GMS_FIX_PART_WORK p)
        {
            return (GMS_FIX_PART_CHALLENGE)p.holder;
        }

        public static explicit operator GMS_FIX_MGR_WORK(GMS_FIX_PART_WORK p)
        {
            return p.parent_mgr;
        }

        public void Clear()
        {
            this.parent_mgr = null;
            this.part_type = 0;
            this.flag = 0U;
            this.blink_timer = this.blink_on_time = this.blink_off_time = 0U;
            this.proc_update = this.proc_disp = null;
        }
    }
}
