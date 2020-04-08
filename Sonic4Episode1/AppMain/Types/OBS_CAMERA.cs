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
    public class OBS_CAMERA : AppMain.IClearable
    {
        public readonly AppMain.NNS_VECTOR disp_pos = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR prev_disp_pos = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR pos = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR prev_pos = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR ofst = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR disp_ofst = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR target_ofst = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR play_ofst_max = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR allow = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR allow_limit = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR target_pos = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR camup_pos = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR spd = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR spd_add = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly AppMain.NNS_VECTOR spd_max = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly int[] roll_hist = new int[16];
        public readonly AppMain.NNS_VECTOR work = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public readonly int[] limit = new int[6];
        public readonly AppMain.NNS_MATRIX prj_pers_mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public readonly AppMain.NNS_MATRIX prj_ortho_mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public readonly AppMain.NNS_MATRIX view_mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public readonly AppMain.NNS_VECTOR up_vec = AppMain.GlobalPool<AppMain.NNS_VECTOR>.Alloc();
        public int camera_id;
        public AppMain.OBS_OBJECT_WORK target_obj;
        public AppMain.OBS_OBJECT_WORK camup_obj;
        public int roll;
        public ushort roll_ptr;
        public ushort shift;
        public ushort index;
        public uint command_state;
        public AppMain.OBJF_CAMERA_USER_FUNC user_func;
        public object user_work;
        public uint flag;
        public AppMain.OBE_CAMERA_TYPE camera_type;
        public int fovy;
        public float scale;
        public float left;
        public float right;
        public float bottom;
        public float top;
        public float znear;
        public float zfar;
        public float aspect;

        public void Clear()
        {
            this.camera_id = 0;
            this.target_obj = this.camup_obj = (AppMain.OBS_OBJECT_WORK)null;
            this.disp_pos.Clear();
            this.prev_disp_pos.Clear();
            this.pos.Clear();
            this.prev_pos.Clear();
            this.ofst.Clear();
            this.disp_ofst.Clear();
            this.target_ofst.Clear();
            this.play_ofst_max.Clear();
            this.allow.Clear();
            this.allow_limit.Clear();
            this.target_pos.Clear();
            this.camup_pos.Clear();
            this.spd.Clear();
            this.spd_add.Clear();
            this.spd_max.Clear();
            this.roll = 0;
            Array.Clear((Array)this.roll_hist, 0, this.roll_hist.Length);
            this.roll_ptr = (ushort)0;
        }
    }
}
