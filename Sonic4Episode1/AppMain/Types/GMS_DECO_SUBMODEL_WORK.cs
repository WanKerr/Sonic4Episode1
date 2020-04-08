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
    public class GMS_DECO_SUBMODEL_WORK : AppMain.IClearable, AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.OBS_ACTION3D_NN_WORK obj_3d_sub = new AppMain.OBS_ACTION3D_NN_WORK();
        public readonly AppMain.GMS_DECO_WORK deco_work;
        public int sub_model_index;

        public void Clear()
        {
            this.deco_work.Clear();
            this.obj_3d_sub.Clear();
            this.sub_model_index = 0;
        }

        public static explicit operator AppMain.GMS_DECO_WORK(AppMain.GMS_DECO_SUBMODEL_WORK work)
        {
            return work.deco_work;
        }

        public GMS_DECO_SUBMODEL_WORK()
        {
            this.deco_work = new AppMain.GMS_DECO_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.deco_work.obj_work;
        }
    }
}
