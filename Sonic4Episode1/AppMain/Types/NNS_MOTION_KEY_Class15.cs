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
    public class NNS_MOTION_KEY_Class15
    {
        public readonly AppMain.NNS_MOTION_SI_SPLINE_HANDLE Shandle = new AppMain.NNS_MOTION_SI_SPLINE_HANDLE();
        public short Frame;
        public short Value;

        public NNS_MOTION_KEY_Class15()
        {
        }

        public NNS_MOTION_KEY_Class15(AppMain.NNS_MOTION_KEY_Class15 motionKey)
        {
            this.Frame = motionKey.Frame;
            this.Value = motionKey.Value;
            this.Shandle.Assign(motionKey.Shandle);
        }

        public AppMain.NNS_MOTION_KEY_Class15 Assign(AppMain.NNS_MOTION_KEY_Class15 motionKey)
        {
            if (this != motionKey)
            {
                this.Frame = motionKey.Frame;
                this.Value = motionKey.Value;
                this.Shandle.Assign(motionKey.Shandle);
            }
            return this;
        }
    }
}
