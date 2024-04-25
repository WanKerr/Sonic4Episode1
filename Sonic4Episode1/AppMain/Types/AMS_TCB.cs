public partial class AppMain
{
    public class AMS_TCB : IClearable
    {
        public readonly AMS_TCB_FOOTER footer = new AMS_TCB_FOOTER();
        public string name;
        public uint user_id;
        public uint attribute;
        public uint priority;
        public TaskProc procedure;
        public int proc_addr;
        public TaskProc destructor;
        public AMS_TCB prev;
        public AMS_TCB next;
        public AMS_TASK taskp;
        public int wkbegin;
        public object work;

        public void Clear()
        {
            this.name = string.Empty;
            this.user_id = 0U;
            this.attribute = 0U;
            this.priority = 0U;
            this.procedure = null;
            this.proc_addr = 0;
            this.destructor = null;
            this.prev = null;
            this.next = null;
            this.taskp = null;
            this.footer.wkend = 0;
        }
    }
}
