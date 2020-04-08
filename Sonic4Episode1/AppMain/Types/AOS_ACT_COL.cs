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
    public struct AOS_ACT_COL : AppMain.IClearable
    {
        public byte a;
        public byte b;
        public byte g;
        public byte r;

        public void Clear()
        {
            this.a = this.b = this.g = this.r = (byte)0;
        }

        public uint c
        {
            get
            {
                return (uint)((int)this.r << 24 | (int)this.g << 16 | (int)this.b << 8) | (uint)this.a;
            }
            set
            {
                uint num = value;
                this.r = (byte)(num >> 24 & (uint)byte.MaxValue);
                this.g = (byte)(num >> 16 & (uint)byte.MaxValue);
                this.b = (byte)(num >> 8 & (uint)byte.MaxValue);
                this.a = (byte)(num & (uint)byte.MaxValue);
            }
        }
    }
}
