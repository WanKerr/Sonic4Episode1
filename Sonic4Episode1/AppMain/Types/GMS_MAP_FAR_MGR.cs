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
    public class GMS_MAP_FAR_MGR
    {
        public readonly AppMain.GMS_MAP_FAR_CAMERA camera = new AppMain.GMS_MAP_FAR_CAMERA();
        public AppMain.MTS_TASK_TCB tcb_pre_draw;
        public AppMain.MTS_TASK_TCB tcb_draw;
        public AppMain.MTS_TASK_TCB tcb_post_draw;

        internal void Clear()
        {
            this.tcb_pre_draw = (AppMain.MTS_TASK_TCB)null;
            this.tcb_draw = (AppMain.MTS_TASK_TCB)null;
            this.tcb_post_draw = (AppMain.MTS_TASK_TCB)null;
            this.camera.Clear();
        }
    }
}
