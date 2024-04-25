public partial class AppMain
{
    public abstract class ITaskLinkAsv : ITaskAsv
    {
        private AMS_TCB m_task_tcb;

        public void AttachTask()
        {
            this.AttachTask("", 4096U, 0U, 2U, 0, 1U, uint.MaxValue);
        }

        public void AttachTask(string name)
        {
            this.AttachTask(name, 4096U, 0U, 2U, 0, 1U, uint.MaxValue);
        }

        public void AttachTask(string name, uint priority)
        {
            this.AttachTask(name, priority, 0U, 2U, 0, 1U, uint.MaxValue);
        }

        public void AttachTask(string name, uint priority, uint user)
        {
            this.AttachTask(name, priority, user, 2U, 0, 1U, uint.MaxValue);
        }

        public void AttachTask(string name, uint priority, uint user, uint attribute)
        {
            this.AttachTask(name, priority, user, attribute, 0, 1U, uint.MaxValue);
        }

        public void AttachTask(string name, uint priority, uint user, uint attribute, int group)
        {
            this.AttachTask(name, priority, user, attribute, group, 1U, uint.MaxValue);
        }

        public void AttachTask(
          string name,
          uint priority,
          uint user,
          uint attribute,
          int group,
          uint stall_mask)
        {
            this.AttachTask(name, priority, user, attribute, group, stall_mask, uint.MaxValue);
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
            this.TaskDestructor(0);
            this.m_task_tcb = null;
        }

        public static ITaskLinkAsv CastFromTaskTcb(AMS_TCB tcb)
        {
            if (new TaskProc(procedure) == tcb.procedure || new TaskProc(destructor) == tcb.destructor)
            {
                SWork work = (SWork)amTaskGetWork(tcb);
                if (work.owner != null && work.owner.m_task_tcb == tcb)
                    return work.owner;
            }
            return null;
        }

        public override AMS_TCB GetTaskTcb()
        {
            return this.m_task_tcb;
        }

        public bool IsTask()
        {
            return null != this.m_task_tcb;
        }

        protected virtual void TaskDestructor(int type)
        {
        }

        private void TcbLinkDestructorCb()
        {
            this.TaskDestructor(1);
            this.m_task_tcb = null;
        }

        private static void procedure(AMS_TCB tcb)
        {
            ((SWork)amTaskGetWork(tcb)).owner.operator_brackets();
        }

        private static void destructor(AMS_TCB tcb)
        {
            ((SWork)amTaskGetWork(tcb)).owner.TcbLinkDestructorCb();
        }

        protected class EDestructorCbType
        {
            public const int DetachTask = 0;
            public const int MtTaskClear = 1;
            public const int Max = 2;
            public const int None = 3;
        }

        private class SWork
        {
            public ITaskLinkAsv owner;
        }
    }
}
