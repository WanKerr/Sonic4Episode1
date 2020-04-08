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
    private class GMS_TVX_DRAW_WORK : AppMain.IClearable
    {
        public AppMain.GMS_TVX_DRAW_STACK[] stack = AppMain.New<AppMain.GMS_TVX_DRAW_STACK>(AppMain.GMD_TVX_DRAW_STACK_NUM);
        public AppMain.NNS_TEXLIST tex;
        public int tex_id;
        public uint all_vtx_num;
        public uint stack_num;
        public int u_wrap;
        public int v_wrap;

        public void Clear()
        {
            this.tex = (AppMain.NNS_TEXLIST)null;
            this.tex_id = 0;
            this.all_vtx_num = this.stack_num = 0U;
            this.u_wrap = this.v_wrap = 0;
            AppMain.ClearArray<AppMain.GMS_TVX_DRAW_STACK>(this.stack);
        }
    }
}
