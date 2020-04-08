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
    public class MTS_TASK_TCB
    {
        public AppMain.AMS_TCB am_tcb;
        public AppMain.GSF_TASK_PROCEDURE proc;
        public AppMain.GSF_TASK_PROCEDURE dest;
        public ushort pause_level;
        public object work;

        public void Clear()
        {
            this.am_tcb = (AppMain.AMS_TCB)null;
            this.proc = (AppMain.GSF_TASK_PROCEDURE)null;
            this.dest = (AppMain.GSF_TASK_PROCEDURE)null;
            this.pause_level = (ushort)0;
            this.work = (object)null;
        }
    }
}
