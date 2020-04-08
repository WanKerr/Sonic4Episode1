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
    public class DMAP_WATER
    {
        public readonly AppMain.DMAP_WATER_OBJ[] _object = AppMain.New<AppMain.DMAP_WATER_OBJ>(2);
        public readonly AppMain.AOS_TEXTURE tex_color = new AppMain.AOS_TEXTURE();
        public AppMain.AMS_AMB_HEADER amb_object;
        public AppMain.AMS_AMB_HEADER amb_texture;
        public int regist_index;
        public float draw_u;
        public float draw_v;
        public float scale;
        public float ofst_u;
        public float ofst_v;
        public float repeat_u;
        public float repeat_v;
        public float speed_u;
        public float speed_v;
        public float speed_surface;
        public float pos_x;
        public float pos_y;
        public float pos_dy;
        public int rot_z;
        public uint color;
        public float repeat_pos_x;
        public AppMain.DMAP_PARAM_WATER draw_param;
    }
}
