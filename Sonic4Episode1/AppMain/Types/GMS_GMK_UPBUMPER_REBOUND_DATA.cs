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
    public class GMS_GMK_UPBUMPER_REBOUND_DATA
    {
        public int act_state;
        public int spd_x;
        public int spd_y;

        public GMS_GMK_UPBUMPER_REBOUND_DATA(int act_state, int spd_x, int spd_y)
        {
            this.act_state = act_state;
            this.spd_x = spd_x;
            this.spd_y = spd_y;
        }
    }
}
