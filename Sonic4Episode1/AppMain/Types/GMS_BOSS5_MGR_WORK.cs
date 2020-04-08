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
    public class GMS_BOSS5_MGR_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly short[] save_camera_offset = new short[2];
        public readonly AppMain.GMS_BOSS5_EXPL_WORK small_expl_work = new AppMain.GMS_BOSS5_EXPL_WORK();
        public readonly AppMain.GMS_BOSS5_EXPL_WORK big_expl_work = new AppMain.GMS_BOSS5_EXPL_WORK();
        public readonly AppMain.GMS_ENEMY_3D_WORK ene_3d;
        public int life;
        public AppMain.MPP_VOID_GMS_BOSS5_MGR_WORK proc_update;
        public uint flag;
        public uint wait_timer;
        public int ply_demo_run_dest_x;
        public int alarm_level;
        public AppMain.GMS_BOSS5_BODY_WORK body_work;

        public GMS_BOSS5_MGR_WORK()
        {
            this.ene_3d = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_BOSS5_MGR_WORK p)
        {
            return p.ene_3d.ene_com.obj_work;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
