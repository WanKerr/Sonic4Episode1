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
    public class GMS_START_DEMO_DATA : AppMain.IClearable
    {
        public readonly AppMain.AOS_TEXTURE[] aos_texture = AppMain.New<AppMain.AOS_TEXTURE>(2);
        public readonly object[] demo_amb = new object[2];
        public bool flag_regist;

        public void Clear()
        {
            Array.Clear((Array)this.demo_amb, 0, this.demo_amb.Length);
            AppMain.ClearArray<AppMain.AOS_TEXTURE>(this.aos_texture);
            this.flag_regist = false;
        }
    }
}
