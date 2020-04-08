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
    public struct AMS_AME_RUNTIME_WORK_CIRCLE
    {
        private AppMain.AMS_AME_RUNTIME_WORK rtm_work_;

        public AMS_AME_RUNTIME_WORK_CIRCLE(AppMain.AMS_AME_RUNTIME_WORK rtm_work)
        {
            this.rtm_work_ = rtm_work;
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

        public float spread
        {
            get
            {
                return BitConverter.ToSingle(this.rtm_work_.dummy, 0);
            }
            set
            {
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, 0);
            }
        }

        public float radius
        {
            get
            {
                return BitConverter.ToSingle(this.rtm_work_.dummy, 4);
            }
            set
            {
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, 4);
            }
        }

        public float offset
        {
            get
            {
                return BitConverter.ToSingle(this.rtm_work_.dummy, 8);
            }
            set
            {
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, 8);
            }
        }

        public float offset_chaos
        {
            get
            {
                return BitConverter.ToSingle(this.rtm_work_.dummy, 12);
            }
            set
            {
                MppBitConverter.GetBytes(value, this.rtm_work_.dummy, 12);
            }
        }

        public static explicit operator AppMain.AMS_AME_LIST(
          AppMain.AMS_AME_RUNTIME_WORK_CIRCLE work)
        {
            return (AppMain.AMS_AME_LIST)work.rtm_work_;
        }
    }
}
