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
    public class GMS_GMK_PMARKER_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_ENEMY_3D_WORK gmk_work;
        public int markerdist;
        public int markerdistlast;
        public int hitcounter;
        public ushort marker_prty;

        public GMS_GMK_PMARKER_WORK()
        {
            this.gmk_work = new AppMain.GMS_ENEMY_3D_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.gmk_work.ene_com.obj_work;
        }

        public AppMain.OBS_ACTION3D_NN_WORK OBJ_3D
        {
            get
            {
                return this.gmk_work.obj_3d;
            }
        }

        public AppMain.GMS_ENEMY_COM_WORK COMWORK
        {
            get
            {
                return this.gmk_work.ene_com;
            }
        }

        public AppMain.OBS_OBJECT_WORK OBJWORK
        {
            get
            {
                return this.COMWORK.obj_work;
            }
        }
    }
}
