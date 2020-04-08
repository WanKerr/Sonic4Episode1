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
    public class AMS_DRAWSTATE_TEXOFFSET
    {
        public int mode;
        public float u;
        public float v;

        public AMS_DRAWSTATE_TEXOFFSET()
        {
        }

        public AMS_DRAWSTATE_TEXOFFSET(int mode, float u, float v)
        {
            this.mode = mode;
            this.u = u;
            this.v = v;
        }

        public void Assign(AppMain.AMS_DRAWSTATE_TEXOFFSET p)
        {
            this.mode = p.mode;
            this.u = p.u;
            this.v = p.v;
        }

        internal void Clear()
        {
            this.mode = 0;
            this.u = 0.0f;
            this.v = 0.0f;
        }
    }
}
