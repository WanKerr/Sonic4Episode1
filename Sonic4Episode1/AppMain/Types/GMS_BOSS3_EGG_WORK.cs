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
    public class GMS_BOSS3_EGG_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_ENEMY_3D_WORK ene_3d;
        public int egg_action_id;
        public uint flag;
        public AppMain.GMF_BOSS3_EGG_STATE_FUNC proc_update;

        public GMS_BOSS3_EGG_WORK()
        {
            this.ene_3d = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_BOSS3_EGG_WORK p)
        {
            return p.ene_3d.ene_com.obj_work;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.ene_3d.ene_com.obj_work;
        }
    }
}
