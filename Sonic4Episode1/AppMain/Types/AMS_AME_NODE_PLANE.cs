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
    public class AMS_AME_NODE_PLANE : AppMain.AMS_AME_NODE_TR_ROT
    {
        public readonly AppMain.NNS_VECTOR4D rotate_axis = AppMain.GlobalPool<AppMain.NNS_VECTOR4D>.Alloc();
        public float z_bias;
        public float inheritance_rate;
        public float life;
        public float start_time;
        public float size;
        public float size_chaos;
        public float scale_x_start;
        public float scale_x_end;
        public float scale_y_start;
        public float scale_y_end;
        public AppMain.AMS_RGBA8888 color_start;
        public AppMain.AMS_RGBA8888 color_end;
        public int blend;
        public short texture_slot;
        public short texture_id;
        public float cropping_l;
        public float cropping_t;
        public float cropping_r;
        public float cropping_b;
        public float scroll_u;
        public float scroll_v;
        public AppMain.AMS_AME_TEX_ANIM tex_anim;
    }
}
