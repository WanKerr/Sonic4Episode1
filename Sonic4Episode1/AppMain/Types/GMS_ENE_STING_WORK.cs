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
    public class GMS_ENE_STING_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.OBS_RECT_WORK search_rect_work = new AppMain.OBS_RECT_WORK();
        public readonly AppMain.NNS_MATRIX jet_r_mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public readonly AppMain.NNS_MATRIX jet_l_mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public readonly AppMain.NNS_MATRIX gun_mtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public readonly AppMain.GMS_ENEMY_3D_WORK ene_3d_work;
        public int spd_dec;
        public int spd_dec_dist;
        public AppMain.GMS_EFFECT_3DES_WORK efct_r_jet;
        public AppMain.GMS_EFFECT_3DES_WORK efct_l_jet;
        public AppMain.GMS_EFFECT_3DES_WORK efct_smoke;
        public int bullet_spd_x;
        public int bullet_spd_y;
        public short bullet_dir;

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d_work.ene_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_ENE_STING_WORK work)
        {
            return work.ene_3d_work.ene_com.obj_work;
        }

        public GMS_ENE_STING_WORK()
        {
            this.ene_3d_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }
    }
}
