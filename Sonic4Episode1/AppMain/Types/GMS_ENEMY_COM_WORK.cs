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
    public class GMS_ENEMY_COM_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.OBS_RECT_WORK[] rect_work = AppMain.New<AppMain.OBS_RECT_WORK>(3);
        public readonly AppMain.OBS_COLLISION_WORK col_work = new AppMain.OBS_COLLISION_WORK();
        public AppMain.VecU16 target_dp_dir = new AppMain.VecU16();
        public AppMain.VecFx32 target_dp_pos = new AppMain.VecFx32();
        public readonly AppMain.OBS_OBJECT_WORK obj_work;
        public AppMain.GMS_EVE_RECORD_EVENT eve_rec;
        public byte eve_x;
        public byte vit;
        public int born_pos_x;
        public int born_pos_y;
        public int invincible_timer;
        public uint enemy_flag;
        public ushort act_state;
        public AppMain.OBS_OBJECT_WORK target_obj;
        public int target_dp_dist;
        public readonly object holder;

        public static explicit operator AppMain.GMS_GMK_TRUCK_WORK(AppMain.GMS_ENEMY_COM_WORK p)
        {
            return (AppMain.GMS_GMK_TRUCK_WORK)(AppMain.GMS_ENEMY_3D_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_CORE_WORK(AppMain.GMS_ENEMY_COM_WORK p)
        {
            return (AppMain.GMS_BOSS5_CORE_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENEMY_3D_WORK(AppMain.GMS_ENEMY_COM_WORK p)
        {
            return p == null ? (AppMain.GMS_ENEMY_3D_WORK)null : (AppMain.GMS_ENEMY_3D_WORK)p.holder;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_ENEMY_COM_WORK work)
        {
            return work.obj_work;
        }

        public GMS_ENEMY_COM_WORK()
        {
            this.obj_work = AppMain.OBS_OBJECT_WORK.Create((object)this, (object)null);
        }

        public GMS_ENEMY_COM_WORK(object p)
        {
            this.holder = p;
            this.obj_work = AppMain.OBS_OBJECT_WORK.Create((object)this, p);
        }
    }
}
