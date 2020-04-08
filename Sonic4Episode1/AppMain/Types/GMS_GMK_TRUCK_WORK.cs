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
    public class GMS_GMK_TRUCK_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.OBS_ACTION3D_NN_WORK obj_3d_tire = new AppMain.OBS_ACTION3D_NN_WORK();
        public readonly AppMain.NNS_MATRIX tire_pos_f = new AppMain.NNS_MATRIX();
        public readonly AppMain.NNS_MATRIX tire_pos_b = new AppMain.NNS_MATRIX();
        public readonly AppMain.NNS_MATRIX light_pos = new AppMain.NNS_MATRIX();
        public readonly AppMain.NNS_VECTOR trans_r = new AppMain.NNS_VECTOR();
        public readonly AppMain.GMS_ENEMY_3D_WORK gmk_work;
        public AppMain.GMS_GMK_TRUCK_WORK seq;
        public AppMain.GMS_PLAYER_WORK target_player;
        public int tire_spd_for_dir;
        public int tire_dir_spd;
        public ushort tire_dir;
        public ushort slope_z_dir;
        public ushort slope_f_y_dir;
        public ushort slope_f_z_dir;
        public AppMain.GMS_EFFECT_3DES_WORK efct_f_spark;
        public AppMain.GMS_EFFECT_3DES_WORK efct_b_spark;
        public AppMain.GSS_SND_SE_HANDLE h_snd_lorry;

        public GMS_GMK_TRUCK_WORK()
        {
            this.gmk_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_GMK_TRUCK_WORK work)
        {
            return work.gmk_work.ene_com.obj_work;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }
    }
}
