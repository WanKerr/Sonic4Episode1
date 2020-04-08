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
    public class GMS_GMK_SEESAW_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_ENEMY_3D_WORK gmk_work;
        public ushort seesaw_id;
        public short initial_tilt;
        public short tilt;
        public short tilt_d;
        public short tilt_acc;
        public short tilt_timer;
        public short tilt_se_timer;
        public int hold_x;
        public int hold_y;
        public long player_distance;
        public int player_speed;
        public AppMain.GMS_PLAYER_WORK ply_work;

        public GMS_GMK_SEESAW_WORK()
        {
            this.gmk_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_GMK_SEESAW_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public static explicit operator AppMain.GMS_ENEMY_3D_WORK(AppMain.GMS_GMK_SEESAW_WORK p)
        {
            return p.gmk_work;
        }
    }
}
