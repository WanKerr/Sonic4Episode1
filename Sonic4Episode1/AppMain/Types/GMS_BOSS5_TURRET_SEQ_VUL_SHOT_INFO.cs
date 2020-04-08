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
    public class GMS_BOSS5_TURRET_SEQ_VUL_SHOT_INFO
    {
        public int life_threshold;
        public uint wait_time;
        public int shot_num;

        public GMS_BOSS5_TURRET_SEQ_VUL_SHOT_INFO(int a, uint b, int c)
        {
            this.life_threshold = a;
            this.wait_time = b;
            this.shot_num = c;
        }
    }
}
