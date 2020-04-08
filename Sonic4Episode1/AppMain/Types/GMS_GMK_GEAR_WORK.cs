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
    public class GMS_GMK_GEAR_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.OBS_ACTION3D_NN_WORK obj_3d_gear_opt = new AppMain.OBS_ACTION3D_NN_WORK();
        public readonly AppMain.OBS_ACTION3D_NN_WORK obj_3d_gear_opt_ashiba = new AppMain.OBS_ACTION3D_NN_WORK();
        public readonly AppMain.GMS_ENEMY_3D_WORK gmk_work;
        public uint col_type;
        public float dir_speed;
        public float dir_temp;
        public ushort prev_dir;
        public ushort move_draw_dir;
        public ushort old_move_draw_dir;
        public short move_draw_dir_spd;
        public short move_draw_dir_ofst;
        public short move_draw_dir_limit;
        public ushort move_stagger_dir_cnt;
        public ushort move_stagger_step;
        public int move_stagger_dir_spd;
        public int stop_timer;
        public int rect_ret_timer;
        public int move_end_x;
        public int move_end_y;
        public int ret_max_speed;
        public bool vib_end;
        public int open_rot_dist;
        public ushort gear_sw_dir_base;
        public int close_rot_spd;
        public AppMain.OBS_OBJECT_WORK gear_end_obj;
        public AppMain.OBS_OBJECT_WORK move_gear_obj;
        public AppMain.OBS_OBJECT_WORK sw_gear_obj;
        public AppMain.GSS_SND_SE_HANDLE h_snd_gear;

        public GMS_GMK_GEAR_WORK()
        {
            this.gmk_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_GMK_GEAR_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }
    }
}
