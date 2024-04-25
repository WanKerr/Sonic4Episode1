using System;

public partial class AppMain
{
    public class GMS_FIX_MGR_WORK
    {
        public GMS_FIX_PART_WORK[] part_work = new GMS_FIX_PART_WORK[5];
        public GMS_FIX_PART_RINGCOUNT part_ringcount = new GMS_FIX_PART_RINGCOUNT();
        public GMS_FIX_PART_SCORE part_score = new GMS_FIX_PART_SCORE();
        public GMS_FIX_PART_TIMER part_timer = new GMS_FIX_PART_TIMER();
        public GMS_FIX_PART_CHALLENGE part_challenge = new GMS_FIX_PART_CHALLENGE();
        public GMS_FIX_PART_VIRTUAL_PAD part_virtual_pad = new GMS_FIX_PART_VIRTUAL_PAD();
        public uint flag;
        public uint req_flag;
        public MPP_VOID_GMS_FIX_PART_WORK proc_update;
        public readonly object holder;

        public GMS_FIX_MGR_WORK()
        {
        }

        public GMS_FIX_MGR_WORK(GMS_FIX_PART_WORK holder)
        {
            this.holder = holder;
        }

        public void Clear()
        {
            this.flag = this.req_flag = 0U;
            this.proc_update = null;
            Array.Clear(part_work, 0, 5);
            this.part_ringcount.Clear();
            this.part_score.Clear();
            this.part_timer.Clear();
            this.part_challenge.Clear();
            this.part_virtual_pad.Clear();
        }

        public static explicit operator GMS_FIX_PART_WORK(GMS_FIX_MGR_WORK p)
        {
            return (GMS_FIX_PART_WORK)p.holder;
        }
    }
}
