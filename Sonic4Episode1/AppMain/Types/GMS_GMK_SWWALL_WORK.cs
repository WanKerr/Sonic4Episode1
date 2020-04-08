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
    public class GMS_GMK_SWWALL_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.OBS_ACTION3D_NN_WORK[] obj_3d_opt = AppMain.New<AppMain.OBS_ACTION3D_NN_WORK>(2);
        public byte[] col_dir_buf = new byte[128];
        public readonly AppMain.GMS_ENEMY_3D_WORK gmk_work;
        public uint id;
        public int wall_size;
        public int wall_draw_size;
        public ushort wall_type;
        public ushort gear_dir;
        public ushort gear_base_dir;
        public AppMain.VecFx32 gear_pos;
        public AppMain.VecFx32 gearbase_pos;
        public int wall_spd;
        public AppMain.GSS_SND_SE_HANDLE h_snd;

        public GMS_GMK_SWWALL_WORK()
        {
            this.gmk_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_GMK_SWWALL_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }
    }
}
