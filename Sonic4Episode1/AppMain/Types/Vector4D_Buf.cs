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
    public struct Vector4D_Buf
    {
        private readonly byte[] data_;
        private readonly int offset_;

        public Vector4D_Buf(byte[] data, int offset)
        {
            this.data_ = data;
            this.offset_ = offset;
        }

        public float x
        {
            get
            {
                return BitConverter.ToSingle(this.data_, this.offset_);
            }
            set
            {
                MppBitConverter.GetBytes(value, this.data_, this.offset_);
            }
        }

        public float y
        {
            get
            {
                return BitConverter.ToSingle(this.data_, this.offset_ + 4);
            }
            set
            {
                MppBitConverter.GetBytes(value, this.data_, this.offset_ + 4);
            }
        }

        public float z
        {
            get
            {
                return BitConverter.ToSingle(this.data_, this.offset_ + 8);
            }
            set
            {
                MppBitConverter.GetBytes(value, this.data_, this.offset_ + 8);
            }
        }

        public float w
        {
            get
            {
                return BitConverter.ToSingle(this.data_, this.offset_ + 12);
            }
            set
            {
                MppBitConverter.GetBytes(value, this.data_, this.offset_ + 12);
            }
        }

        public void Assign(AppMain.Vector4D_Buf v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = v.w;
        }

        public void Assign(AppMain.NNS_VECTOR4D v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = v.w;
        }

        public void Assign(AppMain.NNS_VECTOR v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        public static int SizeBytes
        {
            get
            {
                return 16;
            }
        }
    }
}
