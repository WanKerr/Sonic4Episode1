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
    public class DMS_LOGO_SEGA_WORK
    {
        public AppMain.AOS_ACTION[] act = new AppMain.AOS_ACTION[8];
        public uint flag;
        public int timer;
        public AppMain.DMS_LOGO_SEGA_WORK_Delegate func;
        public AppMain.OBS_OBJECT_WORK ply_obj;
        public AppMain.OBS_OBJECT_WORK efct_obj;
        public AppMain.GSS_SND_SE_HANDLE h_se;
    }
}
