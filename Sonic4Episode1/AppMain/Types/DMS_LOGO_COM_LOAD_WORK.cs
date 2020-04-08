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
    public class DMS_LOGO_COM_LOAD_WORK
    {
        public readonly AppMain.DMS_LOGO_COM_LOAD_CONTEXT[] context = new AppMain.DMS_LOGO_COM_LOAD_CONTEXT[10];
        public AppMain.Reference<AppMain.MTS_TASK_TCB> load_tcb_addr;
        public int context_num;

        public DMS_LOGO_COM_LOAD_WORK()
        {
            for (int index = 0; index < this.context.Length; ++index)
                this.context[index] = new AppMain.DMS_LOGO_COM_LOAD_CONTEXT();
        }
    }
}
