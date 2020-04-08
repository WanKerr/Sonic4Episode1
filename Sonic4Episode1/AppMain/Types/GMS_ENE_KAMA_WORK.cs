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
    public class GMS_ENE_KAMA_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_ENE_NODE_MATRIX node_work = new AppMain.GMS_ENE_NODE_MATRIX();
        public readonly AppMain.GMS_ENEMY_3D_WORK ene_3d_work;
        public int spd_dec;
        public int spd_dec_dist;
        public int hand;
        public int attack;
        public int timer;
        public int rot_z;
        public int rot_z_add;
        public int atk_wait;
        public int walk_s;
        public int ata_futa;
        public AppMain.GMS_ENE_KAMA_FADE_ANIME anime_data;
        public uint anime_pat_no;
        public int anime_frame;

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d_work.ene_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_ENE_KAMA_WORK work)
        {
            return work.ene_3d_work.ene_com.obj_work;
        }

        public GMS_ENE_KAMA_WORK()
        {
            this.ene_3d_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }
    }
}
