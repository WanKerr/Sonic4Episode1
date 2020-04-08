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
    private class AMS_PARAM_DRAW_MOTION
    {
        public AppMain.NNS_OBJECT _object;
        public AppMain.NNS_MATRIX mtx;
        public AppMain.NNS_TEXLIST texlist;
        public uint sub_obj_type;
        public uint flag;
        public AppMain.NNS_MATERIALCALLBACK_FUNC material_func;
        public AppMain.NNS_MOTION motion;
        public float frame;
        public readonly object holder;

        public AMS_PARAM_DRAW_MOTION()
        {
        }

        public AMS_PARAM_DRAW_MOTION(object _holder)
        {
            this.holder = _holder;
        }

        public static explicit operator AppMain.OBS_DRAW_PARAM_3DNN_DRAW_MOTION(
          AppMain.AMS_PARAM_DRAW_MOTION mtn)
        {
            return (AppMain.OBS_DRAW_PARAM_3DNN_DRAW_MOTION)mtn.holder;
        }
    }
}
