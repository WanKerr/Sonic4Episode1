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
    public class IZS_FADE_WORK
    {
        public readonly AppMain.AMS_PARAM_DRAW_PRIMITIVE prim_param = AppMain.GlobalPool<AppMain.AMS_PARAM_DRAW_PRIMITIVE>.Alloc();
        public readonly AppMain.NNS_PRIM2D_PC[][] vtx = AppMain.New<AppMain.NNS_PRIM2D_PC>(2, 4);
        public readonly AppMain.NNS_MATRIX mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public AppMain.NNS_RGBA start_col;
        public AppMain.NNS_RGBA end_col;
        public AppMain.NNS_RGBA now_col;
        public float time;
        public float count;
        public float speed;
        public uint flag;
        public uint draw_state;
        public ushort dt_prio;
        public ushort vtx_no;

        public void Clear()
        {
            this.time = this.count = this.speed = 0.0f;
            this.flag = this.draw_state = 0U;
            this.draw_state = (uint)(this.dt_prio = this.vtx_no = (ushort)0);
            this.start_col.Clear();
            this.end_col.Clear();
            this.now_col.Clear();
            this.mtx.Clear();
            this.prim_param.Clear();
            for (int index1 = 0; index1 < 2; ++index1)
            {
                for (int index2 = 0; index2 < 4; ++index2)
                    this.vtx[index1][index2].Clear();
            }
        }
    }
}
