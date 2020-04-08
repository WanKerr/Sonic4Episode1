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
    public class AMS_AME_RUNTIME_WORK : AppMain.AMS_AME_LIST
    {
        public readonly AppMain.NNS_VECTOR4D position = new AppMain.NNS_VECTOR4D();
        public readonly AppMain.NNS_VECTOR4D velocity = new AppMain.NNS_VECTOR4D();
        public readonly AppMain.NNS_QUATERNION[] rotate = new AppMain.NNS_QUATERNION[1];
        public readonly byte[] dummy = new byte[64];
        public float time;
        public uint flag;

        public override void Clear()
        {
            this.time = 0.0f;
            this.flag = 0U;
            this.position.Clear();
            this.velocity.Clear();
            this.rotate[0].Clear();
            Array.Clear((Array)this.dummy, 0, 64);
            this.next = (AppMain.AMS_AME_LIST)null;
            this.prev = (AppMain.AMS_AME_LIST)null;
        }

        public static explicit operator AppMain.AMS_AME_RUNTIME_WORK_MODEL(
          AppMain.AMS_AME_RUNTIME_WORK work)
        {
            return new AppMain.AMS_AME_RUNTIME_WORK_MODEL(work);
        }

        public static explicit operator AppMain.AMS_AME_RUNTIME_WORK_DIRECTIONAL(
          AppMain.AMS_AME_RUNTIME_WORK work)
        {
            return new AppMain.AMS_AME_RUNTIME_WORK_DIRECTIONAL(work);
        }

        public static explicit operator AppMain.AMS_AME_RUNTIME_WORK_OMNI(
          AppMain.AMS_AME_RUNTIME_WORK work)
        {
            return new AppMain.AMS_AME_RUNTIME_WORK_OMNI(work);
        }

        public static explicit operator AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE(
          AppMain.AMS_AME_RUNTIME_WORK work)
        {
            return new AppMain.AMS_AME_RUNTIME_WORK_SIMPLE_SPRITE(work);
        }

        public static explicit operator AppMain.AMS_AME_RUNTIME_WORK_SPRITE(
          AppMain.AMS_AME_RUNTIME_WORK work)
        {
            return new AppMain.AMS_AME_RUNTIME_WORK_SPRITE(work);
        }

        public static explicit operator AppMain.AMS_AME_RUNTIME_WORK_LINE(
          AppMain.AMS_AME_RUNTIME_WORK work)
        {
            return new AppMain.AMS_AME_RUNTIME_WORK_LINE(work);
        }

        public static explicit operator AppMain.AMS_AME_RUNTIME_WORK_PLANE(
          AppMain.AMS_AME_RUNTIME_WORK work)
        {
            return new AppMain.AMS_AME_RUNTIME_WORK_PLANE(work);
        }

        public static explicit operator AppMain.AMS_AME_RUNTIME_WORK_SURFACE(
          AppMain.AMS_AME_RUNTIME_WORK work)
        {
            return new AppMain.AMS_AME_RUNTIME_WORK_SURFACE(work);
        }

        public static explicit operator AppMain.AMS_AME_RUNTIME_WORK_CIRCLE(
          AppMain.AMS_AME_RUNTIME_WORK work)
        {
            return new AppMain.AMS_AME_RUNTIME_WORK_CIRCLE(work);
        }
    }
}
