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
    public class GMS_GMK_SLOT_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_GMK_SLOT_REEL_STATUS_WORK[] reel_status = AppMain.New<AppMain.GMS_GMK_SLOT_REEL_STATUS_WORK>(3);
        public readonly short[] prob = new short[5];
        public readonly AppMain.GMS_ENEMY_3D_WORK gmk_work;
        public int current_reel;
        public int slot_id;
        public int timer;
        public int timer_next;
        public int timer_meoshi_wait;
        public int slot_step;
        public int slot_se;
        public int slot_se_timer;
        public int suberi_cnt;
        public int suberi_input;
        public short lotresult;
        public int freestop;
        public int ppos_x;
        public int ppos_y;

        public GMS_GMK_SLOT_WORK()
        {
            this.gmk_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_GMK_SLOT_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public static explicit operator AppMain.GMS_ENEMY_3D_WORK(AppMain.GMS_GMK_SLOT_WORK p)
        {
            return p.gmk_work;
        }
    }
}
