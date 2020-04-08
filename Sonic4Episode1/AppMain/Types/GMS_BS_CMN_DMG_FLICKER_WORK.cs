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
    public class GMS_BS_CMN_DMG_FLICKER_WORK : AppMain.IClearable
    {
        public int is_active;
        public uint cycles;
        public uint interval_timer;
        public int cur_angle;
        public float radius;

        public void Clear()
        {
            this.is_active = 0;
            this.cycles = this.interval_timer = 0U;
            this.cur_angle = 0;
            this.radius = 0.0f;
        }
    }
}
