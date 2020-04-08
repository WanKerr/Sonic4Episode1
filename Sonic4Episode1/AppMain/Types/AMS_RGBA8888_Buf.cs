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
    public struct AMS_RGBA8888_Buf
    {
        private readonly byte[] data_;
        private readonly int offset_;

        public AMS_RGBA8888_Buf(byte[] data, int offset)
        {
            this.data_ = data;
            this.offset_ = offset;
        }

        public byte r
        {
            get
            {
                return this.data_[this.offset_];
            }
            set
            {
                this.data_[this.offset_] = value;
            }
        }

        public byte g
        {
            get
            {
                return this.data_[this.offset_ + 1];
            }
            set
            {
                this.data_[this.offset_ + 1] = value;
            }
        }

        public byte b
        {
            get
            {
                return this.data_[this.offset_ + 2];
            }
            set
            {
                this.data_[this.offset_ + 2] = value;
            }
        }

        public byte a
        {
            get
            {
                return this.data_[this.offset_ + 3];
            }
            set
            {
                this.data_[this.offset_ + 3] = value;
            }
        }

        public uint color
        {
            get
            {
                return BitConverter.ToUInt32(this.data_, this.offset_);
            }
            set
            {
                MppBitConverter.GetBytes(value, this.data_, this.offset_);
            }
        }

        public static int SizeBytes
        {
            get
            {
                return 4;
            }
        }
    }
}
