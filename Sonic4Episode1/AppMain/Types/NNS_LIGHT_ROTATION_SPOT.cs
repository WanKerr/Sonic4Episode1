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
    public struct NNS_LIGHT_ROTATION_SPOT
    {
        private AppMain.NNS_LIGHT_TARGET_DIRECTIONAL data_;

        public NNS_LIGHT_ROTATION_SPOT(AppMain.NNS_LIGHT_TARGET_DIRECTIONAL data)
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

        public int RotType
        {
            get
            {
                return MppBitConverter.SingleToInt32(this.data_.Target.x);
            }
            set
            {
                this.data_.Target.x = MppBitConverter.Int32ToSingle(value);
            }
        }

        public AppMain.NNS_ROTATE_A32 Rotation
        {
            get
            {
                return new AppMain.NNS_ROTATE_A32()
                {
                    x = MppBitConverter.SingleToInt32(this.data_.Target.y),
                    y = MppBitConverter.SingleToInt32(this.data_.Target.z),
                    z = MppBitConverter.SingleToInt32(this.data_.InnerRange)
                };
            }
            set
            {
                this.data_.Target.y = MppBitConverter.Int32ToSingle(value.x);
                this.data_.Target.z = MppBitConverter.Int32ToSingle(value.y);
                this.data_.InnerRange = MppBitConverter.Int32ToSingle(value.z);
            }
        }

        public int InnerAngle
        {
            get
            {
                return MppBitConverter.SingleToInt32(this.data_.OuterRange);
            }
            set
            {
                this.data_.OuterRange = MppBitConverter.Int32ToSingle(value);
            }
        }

        public int OuterAngle
        {
            get
            {
                return MppBitConverter.SingleToInt32(this.data_.FallOffStart);
            }
            set
            {
                this.data_.FallOffStart = MppBitConverter.Int32ToSingle(value);
            }
        }

        public float FallOffStart
        {
            get
            {
                return this.data_.FallOffEnd;
            }
            set
            {
                this.data_.FallOffEnd = value;
            }
        }

        public float FallOffEnd
        {
            get
            {
                return this.data_.dummy;
            }
            set
            {
                this.data_.dummy = value;
            }
        }
    }
}
