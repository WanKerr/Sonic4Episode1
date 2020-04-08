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
    public class NNS_VECTOR4D : AppMain.NNS_VECTOR
    {
        public float w;

        public new void Clear()
        {
            base.Clear();
            this.w = 0.0f;
        }

        public void Assign(AppMain.NNS_VECTOR4D v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = v.w;
        }

        public static explicit operator OpenGL.glArray4f(AppMain.NNS_VECTOR4D v)
        {
            return new OpenGL.glArray4f(v.x, v.y, v.z, v.w);
        }

        public static explicit operator float[] (AppMain.NNS_VECTOR4D v)
        {
            return new float[4] { v.x, v.y, v.z, v.w };
        }
    }
}
