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
    public class VecU32
    {
        public uint x;
        public uint y;
        public uint z;

        public VecU32()
          : this(0U, 0U, 0U)
        {
        }

        public VecU32(uint _x, uint _y, uint _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public VecU32(AppMain.VecU32 vecU32)
        {
            this.x = vecU32.x;
            this.y = vecU32.y;
            this.z = vecU32.z;
        }

        public AppMain.VecU32 Assign(AppMain.VecU32 vecU32)
        {
            this.x = vecU32.x;
            this.y = vecU32.y;
            this.z = vecU32.z;
            return this;
        }
    }
}
