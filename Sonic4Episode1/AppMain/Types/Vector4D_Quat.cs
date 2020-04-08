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
    public struct Vector4D_Quat
    {
        private readonly AppMain.NNS_QUATERNION[] quat_;

        public Vector4D_Quat(AppMain.NNS_QUATERNION[] quat)
        {
            this.quat_ = quat;
        }

        public float x
        {
            get
            {
                return this.quat_[0].x;
            }
        }

        public float y
        {
            get
            {
                return this.quat_[0].y;
            }
        }

        public float z
        {
            get
            {
                return this.quat_[0].z;
            }
        }

        public float w
        {
            get
            {
                return this.quat_[0].w;
            }
        }

        public void Assign(float x, float y, float z, float w)
        {
            this.quat_[0] = new AppMain.NNS_QUATERNION(x, y, z, w);
        }

        public void Assign(AppMain.NNS_VECTOR4D v)
        {
            this.quat_[0] = new AppMain.NNS_QUATERNION(v.x, v.y, v.z, v.w);
        }

        public static explicit operator AppMain.NNS_QUATERNION(AppMain.Vector4D_Quat v)
        {
            return v.quat_[0];
        }
    }
}
