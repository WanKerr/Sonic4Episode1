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
    public class AMS_TRAIL_PARTSDATA
    {
        public AppMain.AMS_TRAIL_PARTS[] parts = AppMain.New<AppMain.AMS_TRAIL_PARTS>(64);
        public AppMain.AMS_TRAIL_PARTS trailHead = new AppMain.AMS_TRAIL_PARTS();
        public AppMain.AMS_TRAIL_PARTS trailTail = new AppMain.AMS_TRAIL_PARTS();

        public void Clear()
        {
            foreach (AppMain.AMS_TRAIL_PARTS part in this.parts)
                part.Clear();
            this.trailHead.Clear();
            this.trailTail.Clear();
        }
    }
}
