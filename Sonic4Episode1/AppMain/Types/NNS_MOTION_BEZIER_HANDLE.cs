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
    public class NNS_MOTION_BEZIER_HANDLE
    {
        public readonly AppMain.NNS_VECTOR2D In = new AppMain.NNS_VECTOR2D();
        public readonly AppMain.NNS_VECTOR2D Out = new AppMain.NNS_VECTOR2D();

        public NNS_MOTION_BEZIER_HANDLE()
        {
        }

        public NNS_MOTION_BEZIER_HANDLE(AppMain.NNS_MOTION_BEZIER_HANDLE bezierHandle)
        {
            this.In.Assign(bezierHandle.In);
            this.Out.Assign(bezierHandle.Out);
        }

        public AppMain.NNS_MOTION_BEZIER_HANDLE Assign(
          AppMain.NNS_MOTION_BEZIER_HANDLE bezierHandle)
        {
            if (this != bezierHandle)
            {
                this.In.Assign(bezierHandle.In);
                this.Out.Assign(bezierHandle.Out);
            }
            return this;
        }

        public static AppMain.NNS_MOTION_BEZIER_HANDLE Read(BinaryReader reader)
        {
            AppMain.NNS_MOTION_BEZIER_HANDLE motionBezierHandle = new AppMain.NNS_MOTION_BEZIER_HANDLE();
            motionBezierHandle.In.Assign(AppMain.NNS_VECTOR2D.Read(reader));
            motionBezierHandle.Out.Assign(AppMain.NNS_VECTOR2D.Read(reader));
            return motionBezierHandle;
        }
    }
}
