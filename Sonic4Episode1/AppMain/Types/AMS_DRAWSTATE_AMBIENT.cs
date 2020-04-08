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
    public class AMS_DRAWSTATE_AMBIENT
    {
        public int mode;
        public float r;
        public float g;
        public float b;

        public AMS_DRAWSTATE_AMBIENT()
        {
        }

        public AMS_DRAWSTATE_AMBIENT(AppMain.AMS_DRAWSTATE_AMBIENT drawState)
        {
            this.mode = drawState.mode;
            this.r = drawState.r;
            this.g = drawState.g;
            this.b = drawState.b;
        }

        public AppMain.AMS_DRAWSTATE_AMBIENT Assign(AppMain.AMS_DRAWSTATE_AMBIENT drawState)
        {
            this.mode = drawState.mode;
            this.r = drawState.r;
            this.g = drawState.g;
            this.b = drawState.b;
            return this;
        }

        public void Clear()
        {
            this.mode = 0;
            this.r = this.g = this.b = 0.0f;
        }
    }
}
