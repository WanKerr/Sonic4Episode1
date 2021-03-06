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
    public class GMS_GMK_PWALLCTRL_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_ENEMY_3D_WORK gmk_work;
        public int line_top;
        public int line_bottom;
        public AppMain.GMS_PLAYER_WORK ply_work;
        public int last_ply_x;
        public int last_ply_y;

        public GMS_GMK_PWALLCTRL_WORK()
        {
            this.gmk_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public int line_left
        {
            get
            {
                return this.line_top;
            }
            set
            {
                this.line_top = value;
            }
        }

        public int line_right
        {
            get
            {
                return this.line_bottom;
            }
            set
            {
                this.line_bottom = value;
            }
        }
    }
}
