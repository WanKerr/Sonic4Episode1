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
    public class GMS_PLY_EFCT_TRAIL_SETTING
    {
        public float start_size;
        public float end_size;
        public float life;
        public float vanish_time;

        public GMS_PLY_EFCT_TRAIL_SETTING(
          float start_size,
          float end_size,
          float life,
          float vanish_time)
        {
            this.start_size = start_size;
            this.end_size = end_size;
            this.life = life;
            this.vanish_time = vanish_time;
        }
    }
}
