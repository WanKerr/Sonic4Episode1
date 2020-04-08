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
    public struct AMS_AME_RUNTIME_WORK_MODEL
    {
        private AppMain.Vector4D_Buf rotate_axis_;
        private AppMain.Vector4D_Buf scale_;
        private AppMain.AMS_RGBA8888_Buf color_;
        private AppMain.AMS_AME_RUNTIME_WORK rtm_work_;

        public AMS_AME_RUNTIME_WORK_MODEL(AppMain.AMS_AME_RUNTIME_WORK rtm_work)
        {
            this.rtm_work_ = rtm_work;
            this.rotate_axis_ = new AppMain.Vector4D_Buf(rtm_work.dummy, 0);
            this.scale_ = new AppMain.Vector4D_Buf(rtm_work.dummy, AppMain.Vector4D_Buf.SizeBytes);
            this.color_ = new AppMain.AMS_RGBA8888_Buf(rtm_work.dummy, AppMain.Vector4D_Buf.SizeBytes + AppMain.Vector4D_Buf.SizeBytes);
        }

        public AppMain.AMS_AME_LIST next
        {
            get
            {
                return this.rtm_work_.next;
            }
            set
            {
                this.rtm_work_.next = value;
            }
        }

        public AppMain.AMS_AME_LIST prev
        {
            get
            {
                return this.rtm_work_.prev;
            }
            set
            {
                this.rtm_work_.prev = value;
            }
        }

        public float time
        {
            get
            {
                return this.rtm_work_.time;
            }
            set
            {
                this.rtm_work_.time = value;
            }
        }

        public uint flag
        {
            get
            {
                return this.rtm_work_.flag;
            }
            set
            {
                this.rtm_work_.flag = value;
            }
        }

        public AppMain.NNS_VECTOR4D position
        {
            get
            {
                return this.rtm_work_.position;
            }
            set
            {
                this.rtm_work_.position.Assign(value);
            }
        }

        public AppMain.NNS_VECTOR4D velocity
        {
            get
            {
                return this.rtm_work_.velocity;
            }
            set
            {
                this.rtm_work_.velocity.Assign(value);
            }
        }

        public AppMain.NNS_QUATERNION rotate
        {
            get
            {
                return this.rtm_work_.rotate[0];
            }
            set
            {
                this.rtm_work_.rotate[0] = value;
            }
        }

        public AppMain.Vector4D_Buf rotate_axis
        {
            get
            {
                return this.rotate_axis_;
            }
        }

        public void set_rotate_axis(float x, float y, float z, float w)
        {
            this.rotate_axis_.x = x;
            this.rotate_axis_.y = y;
            this.rotate_axis_.z = z;
            this.rotate_axis_.w = w;
        }

        public AppMain.Vector4D_Buf scale
        {
            get
            {
                return this.scale_;
            }
        }

        public void set_scale(float x, float y, float z, float w)
        {
            this.scale_.x = x;
            this.scale_.y = y;
            this.scale_.z = z;
            this.scale_.w = w;
        }

        public void set_scale(AppMain.NNS_VECTOR4D v)
        {
            this.scale_.Assign(v);
        }

        public void set_scale(AppMain.NNS_VECTOR v)
        {
            this.scale_.Assign(v);
        }

        public AppMain.AMS_RGBA8888_Buf color
        {
            get
            {
                return this.color_;
            }
        }

        public void set_color(byte r, byte g, byte b, byte a)
        {
            this.color_.r = r;
            this.color_.g = g;
            this.color_.b = b;
            this.color_.a = a;
        }

        public void set_color(uint c)
        {
            this.color_.color = c;
        }

        public float scroll_u
        {
            get
            {
                return BitConverter.ToSingle(this.rtm_work_.dummy, AppMain.Vector4D_Buf.SizeBytes + AppMain.Vector4D_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes);
            }
            set
            {
                int offset = AppMain.Vector4D_Buf.SizeBytes + AppMain.Vector4D_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public float scroll_v
        {
            get
            {
                return BitConverter.ToSingle(this.rtm_work_.dummy, AppMain.Vector4D_Buf.SizeBytes + AppMain.Vector4D_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes + 4);
            }
            set
            {
                int offset = AppMain.Vector4D_Buf.SizeBytes + AppMain.Vector4D_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes + 4;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public static explicit operator AppMain.AMS_AME_LIST(
          AppMain.AMS_AME_RUNTIME_WORK_MODEL work)
        {
            return (AppMain.AMS_AME_LIST)work.rtm_work_;
        }

        public static implicit operator AppMain.AMS_AME_RUNTIME_WORK(
          AppMain.AMS_AME_RUNTIME_WORK_MODEL work)
        {
            return work.rtm_work_;
        }
    }
}
