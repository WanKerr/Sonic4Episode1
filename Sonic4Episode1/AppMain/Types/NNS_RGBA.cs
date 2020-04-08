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
    public struct NNS_RGBA
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public void Clear()
        {
            this.r = this.g = this.b = this.a = 0.0f;
        }

        public NNS_RGBA(float _r, float _g, float _b, float _a)
        {
            this.r = _r;
            this.g = _g;
            this.b = _b;
            this.a = _a;
        }

        public static explicit operator OpenGL.glArray4f(AppMain.NNS_RGBA c)
        {
            return new OpenGL.glArray4f(c.r, c.g, c.b, c.a);
        }
    }
}
