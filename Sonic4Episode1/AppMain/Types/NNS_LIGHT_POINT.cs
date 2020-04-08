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
    public struct NNS_LIGHT_POINT
    {
        private AppMain.NNS_LIGHT_TARGET_DIRECTIONAL data_;

        public NNS_LIGHT_POINT(AppMain.NNS_LIGHT_TARGET_DIRECTIONAL data)
        {
            this.data_ = data;
        }

        public uint User
        {
            get
            {
                return this.data_.User;
            }
            set
            {
                this.data_.User = value;
            }
        }

        public AppMain.NNS_RGBA Color
        {
            get
            {
                return this.data_.Color;
            }
            set
            {
                this.data_.Color = value;
            }
        }

        public float Intensity
        {
            get
            {
                return this.data_.Intensity;
            }
            set
            {
                this.data_.Intensity = value;
            }
        }

        public AppMain.NNS_VECTOR Position
        {
            get
            {
                return this.data_.Position;
            }
            set
            {
                this.data_.Position.Assign(value);
            }
        }

        public float FallOffStart
        {
            get
            {
                return this.data_.Target.x;
            }
            set
            {
                this.data_.Target.x = value;
            }
        }

        public float FallOffEnd
        {
            get
            {
                return this.data_.Target.y;
            }
            set
            {
                this.data_.Target.y = value;
            }
        }
    }
}
