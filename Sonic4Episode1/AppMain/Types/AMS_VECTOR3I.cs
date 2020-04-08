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
    public class AMS_VECTOR3I
    {
        public int x;
        public int y;
        public int z;

        public void Assign(AppMain.AMS_VECTOR3I other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
        }

        public void Assign(AppMain.VecFx32 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }
    }
}
