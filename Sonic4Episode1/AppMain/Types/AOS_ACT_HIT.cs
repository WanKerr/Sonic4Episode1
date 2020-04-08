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
    public class AOS_ACT_HIT
    {
        public AppMain.AOE_ACT_HIT type;
        public float center_x;
        public float center_y;
        public float rotate;
        public float scale_x;
        public float scale_y;
        public uint[] pad;
        public AppMain.AOS_ACT_RECT rect;
        public AppMain.AOS_ACT_CIRCLE circle;
    }
}
