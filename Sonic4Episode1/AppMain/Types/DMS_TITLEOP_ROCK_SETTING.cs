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
    public class DMS_TITLEOP_ROCK_SETTING
    {
        public AppMain.VecFx32 pos = new AppMain.VecFx32();
        public AppMain.VecFx32 scale = new AppMain.VecFx32();

        public DMS_TITLEOP_ROCK_SETTING(uint x1, uint y1, uint z1, uint x2, uint y2, uint z2)
        {
            this.pos = new AppMain.VecFx32((int)x1, (int)y1, (int)z1);
            this.scale = new AppMain.VecFx32((int)x2, (int)y2, (int)z2);
        }
    }
}
