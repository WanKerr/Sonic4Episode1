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
    public class DMS_TITLEOP_OBJ_3DNN_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public AppMain.OBS_ACTION3D_NN_WORK obj_3d = new AppMain.OBS_ACTION3D_NN_WORK();
        public readonly AppMain.OBS_OBJECT_WORK obj_work;
        public AppMain.DMS_TITLEOP_ROCK_SETTING[] rock_setting;
        public int rock_setting_num;
        public float sky_rot;

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(
          AppMain.DMS_TITLEOP_OBJ_3DNN_WORK work)
        {
            return work.obj_work;
        }

        public static explicit operator AppMain.DMS_TITLEOP_OBJ_3DNN_WORK(
          AppMain.OBS_OBJECT_WORK work)
        {
            return (AppMain.DMS_TITLEOP_OBJ_3DNN_WORK)work.holder;
        }

        public DMS_TITLEOP_OBJ_3DNN_WORK()
        {
            this.obj_work = AppMain.OBS_OBJECT_WORK.Create((object)this);
        }
    }
}
