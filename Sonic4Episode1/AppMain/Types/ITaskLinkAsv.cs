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
    public abstract class ITaskLinkAsv : AppMain.ITaskAsv
    {
        private AppMain.AMS_TCB m_task_tcb;

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
            this.m_task_tcb = AppMain.amTaskMake(new AppMain.TaskProc(AppMain.ITaskLinkAsv.procedure), new AppMain.TaskProc(AppMain.ITaskLinkAsv.destructor), priority, user, attribute, name, stall_mask, group, run_mask);
            this.m_task_tcb.work = (object)new AppMain.ITaskLinkAsv.SWork();
            ((AppMain.ITaskLinkAsv.SWork)AppMain.amTaskGetWork(this.m_task_tcb)).owner = this;
        }

        public void DetachTask()
        {
            if (this.m_task_tcb == null)
                return;
            AppMain.amTaskSetDestructor(this.m_task_tcb, (AppMain.TaskProc)null);
            AppMain.amTaskDelete(this.m_task_tcb);
            this.TaskDestructor(0);
            this.m_task_tcb = (AppMain.AMS_TCB)null;
        }

        public static AppMain.ITaskLinkAsv CastFromTaskTcb(AppMain.AMS_TCB tcb)
        {
            if (new AppMain.TaskProc(AppMain.ITaskLinkAsv.procedure) == tcb.procedure || new AppMain.TaskProc(AppMain.ITaskLinkAsv.destructor) == tcb.destructor)
            {
                AppMain.ITaskLinkAsv.SWork work = (AppMain.ITaskLinkAsv.SWork)AppMain.amTaskGetWork(tcb);
                if (work.owner != null && work.owner.m_task_tcb == tcb)
                    return work.owner;
            }
            return (AppMain.ITaskLinkAsv)null;
        }

        public override AppMain.AMS_TCB GetTaskTcb()
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
            this.m_task_tcb = (AppMain.AMS_TCB)null;
        }

        private static void procedure(AppMain.AMS_TCB tcb)
        {
            ((AppMain.ITaskLinkAsv.SWork)AppMain.amTaskGetWork(tcb)).owner.operator_brackets();
        }

        private static void destructor(AppMain.AMS_TCB tcb)
        {
            ((AppMain.ITaskLinkAsv.SWork)AppMain.amTaskGetWork(tcb)).owner.TcbLinkDestructorCb();
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
            public AppMain.ITaskLinkAsv owner;
        }
    }
}
