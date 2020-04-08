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
    public class DMS_TITLEOP_MGR_WORK
    {
        public AppMain.OBS_OBJECT_WORK[] obj_work = new AppMain.OBS_OBJECT_WORK[5];
        public AppMain.AOS_ACTION[] act = new AppMain.AOS_ACTION[7];
        public int frame;
        public uint flag;
        public float finger_frame;

        public void Clear()
        {
            this.frame = 0;
            this.flag = 0U;
            Array.Clear((Array)this.obj_work, 0, this.obj_work.Length);
            Array.Clear((Array)this.act, 0, this.act.Length);
            this.finger_frame = 0.0f;
        }
    }
}
