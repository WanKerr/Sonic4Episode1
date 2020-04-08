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
    public class GMS_BOSS4_BODY_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly uint[] flag = new uint[1];
        public readonly ushort[] reserved = new ushort[1];
        public readonly AppMain.GMS_BOSS4_NODE_MATRIX node_work = new AppMain.GMS_BOSS4_NODE_MATRIX();
        public readonly AppMain.GMS_BS_CMN_CNM_MGR_WORK cnm_mgr_work = new AppMain.GMS_BS_CMN_CNM_MGR_WORK();
        public readonly AppMain.GMS_BS_CMN_DMG_FLICKER_WORK flk_work = new AppMain.GMS_BS_CMN_DMG_FLICKER_WORK();
        public readonly AppMain.GMS_BOSS4_1SHOT_TIMER se_timer = new AppMain.GMS_BOSS4_1SHOT_TIMER();
        public readonly AppMain.GMS_BOSS4_MOVE move_work = new AppMain.GMS_BOSS4_MOVE();
        public readonly AppMain.GMS_BOSS4_DIRECTION dir = new AppMain.GMS_BOSS4_DIRECTION();
        public readonly AppMain.GMS_BOSS4_EFF_BOMB_WORK bomb_work = new AppMain.GMS_BOSS4_EFF_BOMB_WORK();
        public readonly AppMain.GMS_BOSS4_EFF_BOMB_WORK bomb_work2 = new AppMain.GMS_BOSS4_EFF_BOMB_WORK();
        public readonly AppMain.OBS_OBJECT_WORK[] parts_objs = new AppMain.OBS_OBJECT_WORK[2];
        public readonly AppMain.GMS_BOSS4_MTN_SUSPEND_WORK[] mtn_suspend = AppMain.New<AppMain.GMS_BOSS4_MTN_SUSPEND_WORK>(2);
        public readonly AppMain.GMS_BOSS4_NOHIT_TIMER nohit_work = new AppMain.GMS_BOSS4_NOHIT_TIMER();
        public readonly AppMain.GMS_CMN_FLASH_SCR_WORK flash_work = new AppMain.GMS_CMN_FLASH_SCR_WORK();
        public readonly AppMain.GMS_ENEMY_3D_WORK ene_3d;
        public int state;
        public int prev_state;
        public AppMain.GMS_BOSS4_MGR_WORK mgr_work;
        public AppMain.MPP_VOID_GMS_BOSS4_BODY_WORK proc_update;
        public int whole_act_id;
        public int egg_revert_mtn_id;
        public uint wait_timer;
        public uint wait_timer2;
        public int chaintop_cnm_reg_id;
        public uint se_cnt;
        public int move_time;
        public int move_cnt;
        public int drift_pivot_x;
        public int drift_angle;
        public int drift_ang_spd;
        public int drift_timer;
        public int atk_nml_alt;
        public AppMain.VecFx32 bash_targ_pos;
        public AppMain.VecFx32 bash_ret_pos;
        public AppMain.VecFx32 bash_orig_pos;
        public int bash_homing_deg;
        public int damage_timer;
        public int avoid_timer;
        public int avoid_yspd;

        public GMS_BOSS4_BODY_WORK()
        {
            this.ene_3d = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_BOSS4_BODY_WORK work)
        {
            return work?.ene_3d.ene_com.obj_work;
        }

        public static explicit operator AppMain.GMS_ENEMY_COM_WORK(
          AppMain.GMS_BOSS4_BODY_WORK work)
        {
            return work.ene_3d.ene_com;
        }

        public static explicit operator AppMain.GMS_ENEMY_3D_WORK(
          AppMain.GMS_BOSS4_BODY_WORK work)
        {
            return work.ene_3d;
        }
    }
}
