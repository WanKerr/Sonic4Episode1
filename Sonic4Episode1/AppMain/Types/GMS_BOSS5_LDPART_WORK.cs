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
    public class GMS_BOSS5_LDPART_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.OBS_COLLISION_WORK col_work = new AppMain.OBS_COLLISION_WORK();
        public readonly int[] vib_ofst = new int[2];
        public readonly int[] pivot_parent_ofst = new int[2];
        public AppMain.NNS_QUATERNION rot_diff_quat = new AppMain.NNS_QUATERNION();
        public AppMain.NNS_QUATERNION cur_rot_quat = new AppMain.NNS_QUATERNION();
        public readonly AppMain.GMS_EFFECT_3DNN_WORK efct_3d;
        public AppMain.MPP_VOID_GMS_BOSS5_LDPART_WORK proc_update;
        public int vib_cnt;
        public int part_index;
        public uint wait_timer;
        public uint brk_glass_cnt;

        public GMS_BOSS5_LDPART_WORK()
        {
            this.efct_3d = new AppMain.GMS_EFFECT_3DNN_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.efct_3d.efct_com.obj_work;
        }
    }
}
