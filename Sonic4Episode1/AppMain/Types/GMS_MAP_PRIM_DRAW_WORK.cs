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
    public class GMS_MAP_PRIM_DRAW_WORK
    {
        public AppMain.GMS_MAP_PRIM_DRAW_STACK[] stack = AppMain.New<AppMain.GMS_MAP_PRIM_DRAW_STACK>((int)byte.MaxValue);
        public int tex_id;
        public uint all_vtx_num;
        public uint stack_num;
        public uint op;
    }
}
