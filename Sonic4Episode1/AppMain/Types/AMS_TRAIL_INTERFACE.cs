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
    public class AMS_TRAIL_INTERFACE
    {
        public AppMain.AMS_TRAIL_PARTSDATA[] trailData = AppMain.New<AppMain.AMS_TRAIL_PARTSDATA>(8);
        public AppMain.AMS_TRAIL_EFFECT[] trailEffect = new AppMain.AMS_TRAIL_EFFECT[8];
        public short trailId;
        public short trailNum;
        public short trailState;
    }
}
