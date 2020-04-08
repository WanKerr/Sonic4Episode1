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
    public struct VecFx32 : AppMain.IClearable
    {
        public int x;
        public int y;
        public int z;

        public void Clear()
        {
            this.x = this.y = this.z = 0;
        }

        public VecFx32(int _x, int _y, int _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public VecFx32(AppMain.VecFx32 vecFx32)
        {
            this.x = vecFx32.x;
            this.y = vecFx32.y;
            this.z = vecFx32.z;
        }

        public void Assign(AppMain.VecFx32 vecFx32)
        {
            this.x = vecFx32.x;
            this.y = vecFx32.y;
            this.z = vecFx32.z;
        }
    }
}
