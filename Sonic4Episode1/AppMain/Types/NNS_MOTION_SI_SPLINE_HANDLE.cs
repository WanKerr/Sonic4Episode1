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
    public class NNS_MOTION_SI_SPLINE_HANDLE
    {
        public float In;
        public float Out;

        public NNS_MOTION_SI_SPLINE_HANDLE()
        {
        }

        public NNS_MOTION_SI_SPLINE_HANDLE(AppMain.NNS_MOTION_SI_SPLINE_HANDLE splineHandle)
        {
            this.In = splineHandle.In;
            this.Out = splineHandle.Out;
        }

        public AppMain.NNS_MOTION_SI_SPLINE_HANDLE Assign(
          AppMain.NNS_MOTION_SI_SPLINE_HANDLE splineHandle)
        {
            if (this != splineHandle)
            {
                this.In = splineHandle.In;
                this.Out = splineHandle.Out;
            }
            return this;
        }

        public static AppMain.NNS_MOTION_SI_SPLINE_HANDLE Read(BinaryReader reader)
        {
            return new AppMain.NNS_MOTION_SI_SPLINE_HANDLE()
            {
                In = reader.ReadSingle(),
                Out = reader.ReadSingle()
            };
        }
    }
}
