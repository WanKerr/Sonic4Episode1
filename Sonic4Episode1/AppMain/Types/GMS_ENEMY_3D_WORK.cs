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
    public class GMS_ENEMY_3D_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.OBS_ACTION3D_NN_WORK obj_3d = new AppMain.OBS_ACTION3D_NN_WORK();
        public readonly AppMain.GMS_ENEMY_COM_WORK ene_com;
        public readonly object holder;

        public GMS_ENEMY_3D_WORK()
        {
            this.ene_com = new AppMain.GMS_ENEMY_COM_WORK((object)this);
        }

        public GMS_ENEMY_3D_WORK(object _holder)
        {
            this.ene_com = new AppMain.GMS_ENEMY_COM_WORK((object)this);
            this.holder = _holder;
        }

        public static explicit operator AppMain.GMS_GMK_GEAR_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_GEAR_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_WATER_SLIDER_WORK(
          AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_WATER_SLIDER_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_UPBUMPER_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_UPBUMPER_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_STEAMP_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_STEAMP_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_SEESAW_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_SEESAW_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_PWALL_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_PWALL_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_PWALLCTRL_WORK(
          AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_PWALLCTRL_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_SHUTTER_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_SHUTTER_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_P_STEAM_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_P_STEAM_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_PISTON_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_PISTON_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK(
          AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_DRAIN_TANK_OUT_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_CANNON_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_CANNON_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK(
          AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_BOSS3_PILLAR_MAIN_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK(
          AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_BOSS3_PILLAR_MANAGER_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK(
          AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_BOSS3_PILLAR_WALL_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_LAND_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS5_LAND_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_CTPLT_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS5_CTPLT_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_TURRET_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS5_TURRET_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_EGG_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS5_EGG_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_ROCKET_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS5_ROCKET_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_MGR_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS5_MGR_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_BODY_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS5_BODY_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS4_CHIBI_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS4_CHIBI_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS4_BODY_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS4_BODY_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS4_CAP_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS4_CAP_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS4_EGG_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS4_EGG_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS4_MGR_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS4_MGR_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_TRUCK_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_TRUCK_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS2_BALL_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS2_BALL_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS2_EGG_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS2_EGG_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS2_BODY_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS2_BODY_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS2_MGR_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS2_MGR_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS3_EGG_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS3_EGG_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS3_BODY_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS3_BODY_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS3_MGR_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS3_MGR_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENE_UNIUNI_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_ENE_UNIUNI_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENE_UNIDES_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_ENE_UNIDES_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENE_BUKU_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_ENE_BUKU_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENE_T_STAR_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_ENE_T_STAR_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENE_KANI_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_ENE_KANI_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENE_KAMA_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return p == null ? (AppMain.GMS_ENE_KAMA_WORK)null : (AppMain.GMS_ENE_KAMA_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENE_MOGU_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_ENE_MOGU_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_SPCTPLT_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_SPCTPLT_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_SLOT_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_SLOT_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_STOPPER_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_STOPPER_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENE_HARO_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_ENE_HARO_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENE_GARDON_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_ENE_GARDON_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_BUMPER_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_BUMPER_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS1_CHAIN_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS1_CHAIN_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS1_MGR_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS1_MGR_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS1_EGG_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS1_EGG_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BOSS1_BODY_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_BOSS1_BODY_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_PULLEY_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_PULLEY_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_ENE_STING_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_ENE_STING_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_BLAND_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_BLAND_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_PMARKER_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_PMARKER_WORK)p.holder;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ene_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_ENEMY_3D_WORK work)
        {
            return work.ene_com.obj_work;
        }

        public static explicit operator AppMain.GMS_GMK_NEEDLE_WORK(
          AppMain.GMS_ENEMY_3D_WORK work)
        {
            return (AppMain.GMS_GMK_NEEDLE_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_ENE_HARI_WORK(AppMain.GMS_ENEMY_3D_WORK work)
        {
            return (AppMain.GMS_ENE_HARI_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_ENE_MOTORA_WORK(
          AppMain.GMS_ENEMY_3D_WORK work)
        {
            return (AppMain.GMS_ENE_MOTORA_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_BOBJ_WORK(AppMain.GMS_ENEMY_3D_WORK work)
        {
            return (AppMain.GMS_GMK_BOBJ_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_BOBJ_PARTS(AppMain.GMS_ENEMY_3D_WORK work)
        {
            return (AppMain.GMS_GMK_BOBJ_PARTS)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_BELTC_WORK(AppMain.GMS_ENEMY_3D_WORK p)
        {
            return (AppMain.GMS_GMK_BELTC_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_GMK_ROCK_FALL_WORK(
          AppMain.GMS_ENEMY_3D_WORK work)
        {
            return (AppMain.GMS_GMK_ROCK_FALL_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_ROCK_FALL_MGR_WORK(
          AppMain.GMS_ENEMY_3D_WORK work)
        {
            return (AppMain.GMS_GMK_ROCK_FALL_MGR_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_ROCK_CHASE_WORK(
          AppMain.GMS_ENEMY_3D_WORK work)
        {
            return (AppMain.GMS_GMK_ROCK_CHASE_WORK)work.holder;
        }
    }
}
