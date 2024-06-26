public partial class AppMain
{
    public class ITask
    {
        public static uint c_priority_default = 4096;
        public static uint c_user_default = 0;
        public static uint c_attribute_default = 2;
        public static int c_group_default = 0;
        public static uint c_stall_mask_default = 1;
        public static uint c_run_mask_default = uint.MaxValue;
        private IFunctor m_pFunctor;

        public ITask(IFunctor pFunctor)
        {
            this.m_pFunctor = pFunctor;
        }

        public virtual void operator_brackets()
        {
            this.m_pFunctor.operator_brackets();
        }

        public virtual AMS_TCB GetTaskTcb()
        {
            mppAssertNotImpl();
            return null;
        }

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
            return c_group_default;
        }

        public uint GetStallMask()
        {
            return c_stall_mask_default;
        }

        public uint GetRunMask()
        {
            return c_run_mask_default;
        }

        public delegate void FProc();
    }
}
