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
    public class SYS_EVT_DATA
    {
        public AppMain.SYS_EVT_DATA._init_func_ init_func;
        public AppMain.SYS_EVT_DATA._exit_func_ exit_func;
        public AppMain.SYS_EVT_DATA._reset_func_ reset_func;
        public AppMain.SYS_EVT_DATA._init_sys_func_ init_sys_func;
        public AppMain.SYS_EVT_DATA._exit_sys_func_ exit_sys_func;
        public short[] next_evt_id;
        public uint attr;

        public SYS_EVT_DATA(
          AppMain.SYS_EVT_DATA._init_func_ f1,
          AppMain.SYS_EVT_DATA._exit_func_ f2,
          AppMain.SYS_EVT_DATA._reset_func_ f3,
          AppMain.SYS_EVT_DATA._init_sys_func_ f4,
          AppMain.SYS_EVT_DATA._exit_sys_func_ f5,
          short[] ar,
          uint at)
        {
            this.init_func = f1;
            this.exit_func = f2;
            this.reset_func = f3;
            this.init_sys_func = f4;
            this.exit_sys_func = f5;
            this.next_evt_id = ar;
            this.attr = at;
        }

        public SYS_EVT_DATA()
        {
        }

        public delegate void _init_func_(object obj);

        public delegate void _exit_func_();

        public delegate void _reset_func_();

        public delegate void _init_sys_func_();

        public delegate void _exit_sys_func_();
    }
}
