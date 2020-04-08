using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using accel;
using dbg;
using er;
using er.web;
using gs;
using gs.backup;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using mpp;
using setting;

public partial class AppMain
{
    public class ITaskLink : AppMain.ITask
    {
        private AppMain.AMS_TCB m_task_tcb;

        public void AttachTask(string name)
        {
            this.AttachTask(name, AppMain.ITask.c_priority_default, AppMain.ITask.c_user_default, AppMain.ITask.c_attribute_default, AppMain.ITask.c_group_default, AppMain.ITask.c_stall_mask_default, AppMain.ITask.c_run_mask_default);
        }

        public void AttachTask(string name, uint priority)
        {
            this.AttachTask(name, priority, AppMain.ITask.c_user_default, AppMain.ITask.c_attribute_default, AppMain.ITask.c_group_default, AppMain.ITask.c_stall_mask_default, AppMain.ITask.c_run_mask_default);
        }

        public void AttachTask(string name, uint priority, uint user)
        {
            this.AttachTask(name, priority, user, AppMain.ITask.c_attribute_default, AppMain.ITask.c_group_default, AppMain.ITask.c_stall_mask_default, AppMain.ITask.c_run_mask_default);
        }

        public void AttachTask(string name, uint priority, uint user, uint attribute)
        {
            this.AttachTask(name, priority, user, attribute, AppMain.ITask.c_group_default, AppMain.ITask.c_stall_mask_default, AppMain.ITask.c_run_mask_default);
        }

        public void AttachTask(string name, uint priority, uint user, uint attribute, int group)
        {
            this.AttachTask(name, priority, user, attribute, group, AppMain.ITask.c_stall_mask_default, AppMain.ITask.c_run_mask_default);
        }

        public void AttachTask(
          string name,
          uint priority,
          uint user,
          uint attribute,
          int group,
          uint stall_mask)
        {
            this.AttachTask(name, priority, user, attribute, group, stall_mask, AppMain.ITask.c_run_mask_default);
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
            this.m_task_tcb = AppMain.amTaskMake(new AppMain.TaskProc(AppMain.ITaskLink.procedure), new AppMain.TaskProc(AppMain.ITaskLink.destructor), priority, user, attribute, name, stall_mask, group, run_mask);
            this.m_task_tcb.work = (object)new AppMain.ITaskLink.SWork();
            ((AppMain.ITaskLink.SWork)AppMain.amTaskGetWork(this.m_task_tcb)).owner = this;
        }

        public void DetachTask()
        {
            if (this.m_task_tcb == null)
                return;
            AppMain.amTaskSetDestructor(this.m_task_tcb, (AppMain.TaskProc)null);
            AppMain.amTaskDelete(this.m_task_tcb);
            this.TaskDestructor(AppMain.ITaskLink.EDestructorCbType.Type.DetachTask);
            this.m_task_tcb = (AppMain.AMS_TCB)null;
        }

        public static AppMain.ITaskLink CastFromTaskTcb(AppMain.AMS_TCB tcb)
        {
            AppMain.mppAssertNotImpl();
            return (AppMain.ITaskLink)null;
        }

        public override AppMain.AMS_TCB GetTaskTcb()
        {
            return this.m_task_tcb;
        }

        public bool IsTask()
        {
            return this.m_task_tcb != null;
        }

        public ITaskLink(AppMain.IFunctor pFunctor)
          : base(pFunctor)
        {
            this.m_task_tcb = (AppMain.AMS_TCB)null;
        }

        ~ITaskLink()
        {
            this.DetachTask();
        }

        protected virtual void TaskDestructor(AppMain.ITaskLink.EDestructorCbType.Type type)
        {
        }

        private void TcbLinkDestructorCb()
        {
            AppMain.mppAssertNotImpl();
        }

        private static void procedure(AppMain.AMS_TCB tcb)
        {
            ((AppMain.ITaskLink.SWork)AppMain.amTaskGetWork(tcb)).owner.operator_brackets();
        }

        private static void destructor(AppMain.AMS_TCB tcb)
        {
            AppMain.mppAssertNotImpl();
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
            public AppMain.ITaskLink owner;
        }
    }
}
