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
    public abstract class ITaskAsv
    {
        public const uint c_priority_default = 4096;
        public const uint c_user_default = 0;
        public const uint c_attribute_default = 2;
        public const int c_group_default = 0;
        public const uint c_stall_mask_default = 1;
        public const uint c_run_mask_default = 4294967295;

        public abstract void operator_brackets();

        public static implicit operator AppMain.AMS_TCB(AppMain.ITaskAsv task)
        {
            return task.GetTaskTcb();
        }

        public abstract AppMain.AMS_TCB GetTaskTcb();

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
