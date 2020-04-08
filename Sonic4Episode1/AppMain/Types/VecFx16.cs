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
    public class VecFx16
    {
        public short x;
        public short y;
        public short z;

        public VecFx16()
          : this((short)0, (short)0, (short)0)
        {
        }

        public VecFx16(short _x, short _y, short _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public VecFx16(AppMain.VecFx16 vecFx16)
        {
            this.x = vecFx16.x;
            this.y = vecFx16.y;
            this.z = vecFx16.z;
        }

        public AppMain.VecFx16 Assign(AppMain.VecFx16 vecFx16)
        {
            if (this != vecFx16)
            {
                this.x = vecFx16.x;
                this.y = vecFx16.y;
                this.z = vecFx16.z;
            }
            return this;
        }
    }
}
