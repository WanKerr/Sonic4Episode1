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
    public struct AMS_DRAW_VIDEO
    {
        public float draw_width;
        public float draw_height;
        public float disp_width;
        public float disp_height;
        public bool wide_screen;
        public bool squeeze_screen;
        public float refresh_rate;
        public int video_standard;
        public float width_2d;
        public float height_2d;
        public float draw_aspect;
        public float scale_x_2d;
        public float scale_y_2d;
        public float base_x_2d;
        public float base_y_2d;
    }
}
