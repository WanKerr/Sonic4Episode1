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
    public struct AMS_AME_RUNTIME_WORK_LINE
    {
        private AppMain.Vector4D_Quat st_;
        private AppMain.AMS_RGBA8888_Buf inside_color_;
        private AppMain.AMS_RGBA8888_Buf outside_color_;
        private AppMain.AMS_AME_RUNTIME_WORK rtm_work_;

        public AMS_AME_RUNTIME_WORK_LINE(AppMain.AMS_AME_RUNTIME_WORK rtm_work)
        {
            this.rtm_work_ = rtm_work;
            this.st_ = new AppMain.Vector4D_Quat(rtm_work.rotate);
            this.inside_color_ = new AppMain.AMS_RGBA8888_Buf(rtm_work.dummy, 0);
            this.outside_color_ = new AppMain.AMS_RGBA8888_Buf(rtm_work.dummy, AppMain.AMS_RGBA8888_Buf.SizeBytes);
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

        public AppMain.Vector4D_Quat st
        {
            get
            {
                return this.st_;
            }
        }

        public void set_st(float x, float y, float z, float w)
        {
            this.st_.Assign(x, y, z, w);
        }

        public AppMain.AMS_RGBA8888_Buf inside_color
        {
            get
            {
                return this.inside_color_;
            }
        }

        public void set_inside_color(byte r, byte g, byte b, byte a)
        {
            this.inside_color_.r = r;
            this.inside_color_.g = g;
            this.inside_color_.b = b;
            this.inside_color_.a = a;
        }

        public void set_inside_color(uint c)
        {
            this.inside_color_.color = c;
        }

        public AppMain.AMS_RGBA8888_Buf outside_color
        {
            get
            {
                return this.outside_color_;
            }
        }

        public void set_outside_color(byte r, byte g, byte b, byte a)
        {
            this.outside_color_.r = r;
            this.outside_color_.g = g;
            this.outside_color_.b = b;
            this.outside_color_.a = a;
        }

        public void set_outside_color(uint c)
        {
            this.outside_color_.color = c;
        }

        public float inside_width
        {
            get
            {
                return BitConverter.ToSingle(this.rtm_work_.dummy, AppMain.AMS_RGBA8888_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes);
            }
            set
            {
                int offset = AppMain.AMS_RGBA8888_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public float outside_width
        {
            get
            {
                return BitConverter.ToSingle(this.rtm_work_.dummy, AppMain.AMS_RGBA8888_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes + 4);
            }
            set
            {
                int offset = AppMain.AMS_RGBA8888_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes + 4;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public float length
        {
            get
            {
                return BitConverter.ToSingle(this.rtm_work_.dummy, AppMain.AMS_RGBA8888_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes + 8);
            }
            set
            {
                int offset = AppMain.AMS_RGBA8888_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes + 8;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public float tex_time
        {
            get
            {
                return BitConverter.ToSingle(this.rtm_work_.dummy, AppMain.Vector4D_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes + 12);
            }
            set
            {
                int offset = AppMain.Vector4D_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes + 12;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public int tex_no
        {
            get
            {
                return BitConverter.ToInt32(this.rtm_work_.dummy, AppMain.Vector4D_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes + 16);
            }
            set
            {
                int offset = AppMain.Vector4D_Buf.SizeBytes + AppMain.AMS_RGBA8888_Buf.SizeBytes + 16;
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, offset);
            }
        }

        public static explicit operator AppMain.AMS_AME_LIST(
          AppMain.AMS_AME_RUNTIME_WORK_LINE work)
        {
            return (AppMain.AMS_AME_LIST)work.rtm_work_;
        }

        public static implicit operator AppMain.AMS_AME_RUNTIME_WORK(
          AppMain.AMS_AME_RUNTIME_WORK_LINE work)
        {
            return work.rtm_work_;
        }
    }
}
