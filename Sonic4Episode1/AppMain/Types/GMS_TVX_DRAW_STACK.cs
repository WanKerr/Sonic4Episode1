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
    private class GMS_TVX_DRAW_STACK : AppMain.IClearable
    {
        public AppMain.AOS_TVX_VERTEX[] vtx;
        public AppMain.VecFx32 pos;
        public AppMain.VecFx32 scale;
        public uint disp_flag;
        public uint vtx_num;
        public int rotate_z;
        public AppMain.NNS_TEXCOORD coord;
        public uint color;

        public void Clear()
        {
            this.vtx = (AppMain.AOS_TVX_VERTEX[])null;
            this.pos.Clear();
            this.scale.Clear();
            this.disp_flag = this.vtx_num = 0U;
            this.rotate_z = 0;
            this.coord.Clear();
            this.color = 0U;
        }
    }
}
