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
    private class OBS_DRAW_PARAM_3DNN_SORT_MODEL
    {
        public readonly AppMain.AMS_COMMAND_HEADER cmd_header = new AppMain.AMS_COMMAND_HEADER();
        public readonly AppMain.AMS_PARAM_SORT_DRAW_OBJECT param = new AppMain.AMS_PARAM_SORT_DRAW_OBJECT();
        public readonly AppMain.AMS_DRAWSTATE state = new AppMain.AMS_DRAWSTATE();
        public AppMain.OBF_DRAW_USER_FUNC user_func;
        public object user_param;
        public AppMain.MPP_BOOL_NNSDRAWCALLBACKVAL_OBJECT_DELEGATE material_cb_func;
        public object material_cb_param;
        public uint use_light_flag;
    }
}
