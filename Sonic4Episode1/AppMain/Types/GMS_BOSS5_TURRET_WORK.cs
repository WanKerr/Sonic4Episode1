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
    public class GMS_BOSS5_TURRET_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public AppMain.NNS_QUATERNION disp_quat = new AppMain.NNS_QUATERNION();
        public AppMain.VecFx32 vul_fire_pos = new AppMain.VecFx32();
        public AppMain.VecFx32 vul_bullet_pos = new AppMain.VecFx32();
        public readonly AppMain.GMS_ENEMY_3D_WORK ene_3d;
        public AppMain.MPP_VOID_GMS_BOSS5_TURRET_WORK proc_update;
        public uint flag;
        public uint wait_timer;
        public int fire_dir_z;
        public int trt_slide_type;
        public float trt_slide_length;
        public int cvr_slide_type;
        public float cvr_slide_ratio;
        public int vul_shot_remain;
        public int vul_burst_timer;
        public int vul_shot_angle;

        public GMS_BOSS5_TURRET_WORK()
        {
            this.ene_3d = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
