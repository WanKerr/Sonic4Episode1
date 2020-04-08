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
    private class OBS_DRAW_PARAM_3DNN_MODEL
    {
        public readonly AppMain.NNS_MATRIX mtx = new AppMain.NNS_MATRIX();
        public readonly AppMain.AMS_DRAWSTATE draw_state = new AppMain.AMS_DRAWSTATE();
        public readonly AppMain.AMS_PARAM_DRAW_OBJECT param;
        public AppMain.AMS_DRAWSTATE state;
        public AppMain.MPP_VOID_OBJECT_DELEGATE user_func;
        public object user_param;
        public AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func;
        public object material_cb_param;
        public uint use_light_flag;

        public OBS_DRAW_PARAM_3DNN_MODEL()
        {
            this.param = new AppMain.AMS_PARAM_DRAW_OBJECT((object)this);
        }
    }
}
