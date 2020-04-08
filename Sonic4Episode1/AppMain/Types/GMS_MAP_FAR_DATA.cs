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
    public class GMS_MAP_FAR_DATA
    {
        public readonly AppMain.OBS_OBJECT_WORK[] obj_work = new AppMain.OBS_OBJECT_WORK[16];
        public readonly AppMain.NNS_VECTOR pos = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public AppMain.AMS_AMB_HEADER amb_header;
        public AppMain.OBS_ACTION3D_NN_WORK[] obj_3d_list;
        public AppMain.OBS_ACTION3D_NN_WORK[] obj_3d_list_render;
        public AppMain.OBS_ACTION3D_NN_WORK nn_work;
        public int nn_work_num;
        public int nn_regist_num;
        public AppMain.MP_HEADER mp_header;
        public AppMain.MD_HEADER md_header;
        public float degSky;
        public float degSky2;

        internal void Clear()
        {
            this.amb_header = (AppMain.AMS_AMB_HEADER)null;
            this.obj_3d_list = (AppMain.OBS_ACTION3D_NN_WORK[])null;
            this.obj_3d_list_render = (AppMain.OBS_ACTION3D_NN_WORK[])null;
            Array.Clear((Array)this.obj_work, 0, 16);
            this.nn_work = (AppMain.OBS_ACTION3D_NN_WORK)null;
            this.nn_work_num = 0;
            this.nn_regist_num = 0;
            this.pos.Clear();
            this.mp_header = (AppMain.MP_HEADER)null;
            this.md_header = (AppMain.MD_HEADER)null;
            this.degSky = 0.0f;
            this.degSky2 = 0.0f;
        }
    }
}
