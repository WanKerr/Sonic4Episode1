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
    public class AMS_AME_NODE_MODEL : AppMain.AMS_AME_NODE_TR_ROT
    {
        public readonly AppMain.NNS_VECTOR4D rotate_axis = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        public readonly AppMain.NNS_VECTOR4D scale_start = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        public readonly AppMain.NNS_VECTOR4D scale_end = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        public char[] model_name = new char[8];
        public float z_bias;
        public float inheritance_rate;
        public float life;
        public float start_time;
        public int lod;
        public AppMain.AMS_RGBA8888 color_start;
        public AppMain.AMS_RGBA8888 color_end;
        public int blend;
        public float scroll_u;
        public float scroll_v;
    }
}
