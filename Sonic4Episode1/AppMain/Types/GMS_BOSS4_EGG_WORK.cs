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
    public class GMS_BOSS4_EGG_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_BOSS4_DIRECTION dir_work = new AppMain.GMS_BOSS4_DIRECTION();
        public readonly AppMain.GMS_BOSS4_NODE_MATRIX node_work = new AppMain.GMS_BOSS4_NODE_MATRIX();
        public readonly AppMain.GMS_ENEMY_3D_WORK ene_3d;
        public uint flag;
        public int egg_act_id;
        public AppMain.MPP_VOID_GMS_BOSS4_EGG_WORK proc_update;

        public GMS_BOSS4_EGG_WORK()
        {
            this.ene_3d = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_BOSS4_EGG_WORK work)
        {
            return work.ene_3d.ene_com.obj_work;
        }

        public static explicit operator AppMain.GMS_ENEMY_3D_WORK(AppMain.GMS_BOSS4_EGG_WORK work)
        {
            return work.ene_3d;
        }
    }
}
