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
    public class GMS_START_DEMO_WORK
    {
        public AppMain.GMS_COCKPIT_2D_WORK[] action_obj_work_cmn = new AppMain.GMS_COCKPIT_2D_WORK[4];
        public AppMain.GMS_COCKPIT_2D_WORK[] action_obj_work_zone = new AppMain.GMS_COCKPIT_2D_WORK[1];
        public AppMain.GMS_COCKPIT_2D_WORK[] action_obj_work_act = new AppMain.GMS_COCKPIT_2D_WORK[2];
        public uint counter;
        public uint flag;
        public AppMain.GMS_COCKPIT_2D_WORK action_obj_work_message;
        public AppMain.GMS_START_DEMO_WORK._update_ update;

        public delegate void _update_(AppMain.GMS_START_DEMO_WORK cont);
    }
}
