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
    public class ITask
    {
        public static uint c_priority_default = 4096;
        public static uint c_user_default = 0;
        public static uint c_attribute_default = 2;
        public static int c_group_default = 0;
        public static uint c_stall_mask_default = 1;
        public static uint c_run_mask_default = uint.MaxValue;
        private AppMain.IFunctor m_pFunctor;

        public ITask(AppMain.IFunctor pFunctor)
        {
            this.m_pFunctor = pFunctor;
        }

        public virtual void operator_brackets()
        {
            this.m_pFunctor.operator_brackets();
        }

        public virtual AppMain.AMS_TCB GetTaskTcb()
        {
            AppMain.mppAssertNotImpl();
            return (AppMain.AMS_TCB)null;
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
            return AppMain.ITask.c_group_default;
        }

        public uint GetStallMask()
        {
            return AppMain.ITask.c_stall_mask_default;
        }

        public uint GetRunMask()
        {
            return AppMain.ITask.c_run_mask_default;
        }

        public delegate void FProc();
    }
}
