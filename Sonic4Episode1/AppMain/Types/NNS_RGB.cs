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
    public class NNS_RGB : AppMain.IClearable
    {
        public float r;
        public float g;
        public float b;

        public NNS_RGB()
        {
        }

        public NNS_RGB(float _r, float _g, float _b)
        {
            this.r = _r;
            this.g = _g;
            this.b = _b;
        }

        public AppMain.NNS_RGB Assign(AppMain.NNS_RGB rgb)
        {
            this.r = rgb.r;
            this.g = rgb.g;
            this.b = rgb.b;
            return this;
        }

        public void Clear()
        {
            this.r = this.g = this.b = 0.0f;
        }
    }
}
