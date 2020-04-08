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
    public class NNS_VECTOR : AppMain.IClearable
    {
        public float x;
        public float y;
        public float z;

        public NNS_VECTOR()
        {
        }

        public NNS_VECTOR(float _x, float _y, float _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public AppMain.NNS_VECTOR Assign(AppMain.NNS_VECTOR vec)
        {
            this.x = vec.x;
            this.y = vec.y;
            this.z = vec.z;
            return this;
        }

        public AppMain.NNS_VECTOR Assign(ref AppMain.SNNS_VECTOR vec)
        {
            this.x = vec.x;
            this.y = vec.y;
            this.z = vec.z;
            return this;
        }

        public AppMain.NNS_VECTOR Assign(ref AppMain.SNNS_VECTOR4D vec)
        {
            this.x = vec.x;
            this.y = vec.y;
            this.z = vec.z;
            return this;
        }

        public void Clear()
        {
            this.x = this.y = this.z = 0.0f;
        }

        public static explicit operator OpenGL.glArray4f(AppMain.NNS_VECTOR v)
        {
            return new OpenGL.glArray4f(v.x, v.y, v.z, 0.0f);
        }

        public static explicit operator float[] (AppMain.NNS_VECTOR v)
        {
            return new float[3] { v.x, v.y, v.z };
        }

        internal AppMain.NNS_VECTOR Assign(AppMain.VecFx32 vec)
        {
            this.x = (float)vec.x;
            this.y = (float)vec.y;
            this.z = (float)vec.z;
            return this;
        }

        public static AppMain.NNS_VECTOR Read(BinaryReader reader)
        {
            AppMain.NNS_VECTOR nnsVector = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
            nnsVector.x = reader.ReadSingle();
            nnsVector.y = reader.ReadSingle();
            nnsVector.z = reader.ReadSingle();
            return nnsVector;
        }
    }
}
