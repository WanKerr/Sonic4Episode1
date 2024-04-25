using System.Runtime.InteropServices;

public partial class AppMain
{
    public class ITaskLink : ITask
    {
        private AMS_TCB m_task_tcb;

        public void AttachTask(string name)
        {
            this.AttachTask(name, c_priority_default, c_user_default, c_attribute_default, c_group_default, c_stall_mask_default, c_run_mask_default);
        }

        public void AttachTask(string name, uint priority)
        {
            this.AttachTask(name, priority, c_user_default, c_attribute_default, c_group_default, c_stall_mask_default, c_run_mask_default);
        }

        public void AttachTask(string name, uint priority, uint user)
        {
            this.AttachTask(name, priority, user, c_attribute_default, c_group_default, c_stall_mask_default, c_run_mask_default);
        }

        public void AttachTask(string name, uint priority, uint user, uint attribute)
        {
            this.AttachTask(name, priority, user, attribute, c_group_default, c_stall_mask_default, c_run_mask_default);
        }

        public void AttachTask(string name, uint priority, uint user, uint attribute, int group)
        {
            this.AttachTask(name, priority, user, attribute, group, c_stall_mask_default, c_run_mask_default);
        }

        public void AttachTask(
          string name,
          uint priority,
          uint user,
          uint attribute,
          int group,
          uint stall_mask)
        {
            this.AttachTask(name, priority, user, attribute, group, stall_mask, c_run_mask_default);
        }

        public void AttachTask(
          string name,
          uint priority,
          uint user,
          uint attribute,
          int group,
          uint stall_mask,
          uint run_mask)
        {
            this.DetachTask();
            this.m_task_tcb = amTaskMake(new TaskProc(procedure), new TaskProc(destructor), priority, user, attribute, name, stall_mask, group, run_mask);
            this.m_task_tcb.work = new SWork();
            ((SWork)amTaskGetWork(this.m_task_tcb)).owner = this;
        }

        public void DetachTask()
        {
            if (this.m_task_tcb == null)
                return;
            amTaskSetDestructor(this.m_task_tcb, null);
            amTaskDelete(this.m_task_tcb);
            this.TaskDestructor(EDestructorCbType.Type.DetachTask);
            this.m_task_tcb = null;
        }

        public static ITaskLink CastFromTaskTcb(AMS_TCB tcb)
        {
            mppAssertNotImpl();
            return null;
        }

        public override AMS_TCB GetTaskTcb()
        {
            return this.m_task_tcb;
        }

        public bool IsTask()
        {
            return this.m_task_tcb != null;
        }

        public ITaskLink(IFunctor pFunctor)
          : base(pFunctor)
        {
            this.m_task_tcb = null;
        }

        ~ITaskLink()
        {
            this.DetachTask();
        }

        protected virtual void TaskDestructor(EDestructorCbType.Type type)
        {
        }

        private void TcbLinkDestructorCb()
        {
            mppAssertNotImpl();
        }

        private static void procedure(AMS_TCB tcb)
        {
            ((SWork)amTaskGetWork(tcb)).owner.operator_brackets();
        }

        private static void destructor(AMS_TCB tcb)
        {
            mppAssertNotImpl();
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        protected struct EDestructorCbType
        {
            public enum Type
            {
                DetachTask,
                MtTaskClear,
                Max,
                None,
            }
        }

        private class SWork
        {
            public ITaskLink owner;
        }
    }
}
