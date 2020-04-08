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
    public struct Vector3f
    {
        public float x;
        public float y;
        public float z;

        public Vector3f(float X, float Y, float Z)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
        }

        public void Assign(float X, float Y, float Z)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
        }

        public static explicit operator Vector3(AppMain.Vector3f v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
    }
}
