public partial class AppMain
{
    public class MTS_TASK_TCB
    {
        public AMS_TCB am_tcb;
        public GSF_TASK_PROCEDURE proc;
        public GSF_TASK_PROCEDURE dest;
        public ushort pause_level;
        public object work;

        public void Clear()
        {
            this.am_tcb = null;
            this.proc = null;
            this.dest = null;
            this.pause_level = 0;
            this.work = null;
        }
    }
}
