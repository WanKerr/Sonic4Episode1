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
    public class GMS_BOSS5_ALARM_FADE_INFO
    {
        public uint fo_frame;
        public uint on_frame;
        public uint fi_frame;
        public uint off_frame;

        public GMS_BOSS5_ALARM_FADE_INFO(uint a, uint b, uint c, uint d)
        {
            this.fo_frame = a;
            this.on_frame = b;
            this.fi_frame = c;
            this.off_frame = d;
        }
    }
}
