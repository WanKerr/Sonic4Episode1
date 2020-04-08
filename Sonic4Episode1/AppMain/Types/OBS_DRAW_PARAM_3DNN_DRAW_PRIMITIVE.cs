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
    private class OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE
    {
        public readonly AppMain.AMS_PARAM_DRAW_PRIMITIVE dat = new AppMain.AMS_PARAM_DRAW_PRIMITIVE();
        public readonly AppMain.NNS_MATRIX mtx = new AppMain.NNS_MATRIX();
        public int light;
        public int cull;

        public OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE()
        {
        }

        public OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE(AppMain.OBS_DRAW_PARAM_3DNN_DRAW_PRIMITIVE param)
        {
            this.dat.Assign(param.dat);
            this.mtx.Assign(param.mtx);
            this.light = param.light;
            this.cull = param.cull;
        }
    }
}
