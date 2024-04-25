public partial class AppMain
{
    public abstract class ITaskAsv
    {
        public const uint c_priority_default = 4096;
        public const uint c_user_default = 0;
        public const uint c_attribute_default = 2;
        public const int c_group_default = 0;
        public const uint c_stall_mask_default = 1;
        public const uint c_run_mask_default = 4294967295;

        public abstract void operator_brackets();

        public static implicit operator AMS_TCB(ITaskAsv task)
        {
            return task.GetTaskTcb();
        }

        public abstract AMS_TCB GetTaskTcb();

        public uint GetPriority()
        {
            return this.GetTaskTcb().priority;
        }

        public uint GetUser()
        {
            return this.GetTaskTcb().user_id;
        }

        public uint GetAttribute()
        {
            return this.GetTaskTcb().attribute;
        }

        public int GetGroup()
        {
            return 0;
        }

        public uint GetStallMask()
        {
            return 1;
        }

        public uint GetRunMask()
        {
            return uint.MaxValue;
        }

        public delegate void FProc();
    }
}
