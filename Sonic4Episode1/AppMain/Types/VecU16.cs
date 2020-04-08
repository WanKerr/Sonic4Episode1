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
    public struct VecU16
    {
        public ushort x;
        public ushort y;
        public ushort z;

        public VecU16(ushort _x, ushort _y, ushort _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public VecU16(AppMain.VecU16 vecU16)
        {
            this.x = vecU16.x;
            this.y = vecU16.y;
            this.z = vecU16.z;
        }

        public void Assign(AppMain.VecU16 vecU16)
        {
            this.x = vecU16.x;
            this.y = vecU16.y;
            this.z = vecU16.z;
        }

        public void Clear()
        {
            this.x = this.y = this.z = (ushort)0;
        }
    }
}
