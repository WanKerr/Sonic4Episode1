public partial class AppMain
{
    public class AMS_AME_RUNTIME : AMS_AME_LIST
    {
        public readonly AMS_AME_LIST child_head = new AMS_AME_LIST();
        public readonly AMS_AME_LIST child_tail = new AMS_AME_LIST();
        public readonly AMS_AME_LIST work_head = new AMS_AME_RUNTIME_WORK();
        public readonly AMS_AME_LIST work_tail = new AMS_AME_RUNTIME_WORK();
        public readonly AMS_AME_LIST active_head = new AMS_AME_RUNTIME_WORK();
        public readonly AMS_AME_LIST active_tail = new AMS_AME_RUNTIME_WORK();
        public int state;
        public float amount;
        public uint count;
        public AMS_AME_ECB ecb;
        public AMS_AME_NODE node;
        public AMS_AME_RUNTIME parent_runtime;
        public AMS_AME_RUNTIME spawn_runtime;
        public AMS_AME_RUNTIME_WORK work;
        public int child_num;
        public short work_num;
        public short active_num;
        public NNS_TEXLIST texlist;

        public new void Clear()
        {
            this.next = null;
            this.prev = null;
            this.state = 0;
            this.amount = 0.0f;
            this.count = 0U;
            this.ecb = null;
            this.node = null;
            this.parent_runtime = null;
            this.spawn_runtime = null;
            this.work = null;
            this.child_head.Clear();
            this.child_tail.Clear();
            this.child_num = 0;
            this.work_head.Clear();
            this.work_tail.Clear();
            this.active_head.Clear();
            this.active_tail.Clear();
            this.work_num = 0;
            this.active_num = 0;
            this.texlist = null;
        }
    }
}
