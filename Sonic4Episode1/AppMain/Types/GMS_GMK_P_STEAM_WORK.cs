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
    public class GMS_GMK_P_STEAM_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public AppMain.OBS_OBJECT_WORK[] opt_steam_int = new AppMain.OBS_OBJECT_WORK[3];
        public const int GMD_GMK_PSTEAM_STAT_HIT = 1;
        public const int GMD_GMK_PSTEAM_STAT_TRUE = 2;
        public readonly AppMain.GMS_ENEMY_3D_WORK gmk_work;
        public int obj_type;
        public short timer;
        public ushort steamvect;
        public short steamsize;
        public short steamwait;
        public int steampower;
        public ushort status;
        public AppMain.GMS_PLAYER_WORK ply_work;
        public AppMain.OBS_OBJECT_WORK opt_timer;
        public AppMain.OBS_OBJECT_WORK opt_steam;
        public int pos_x;
        public int pos_y;

        public GMS_GMK_P_STEAM_WORK()
        {
            this.gmk_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }
    }
}
