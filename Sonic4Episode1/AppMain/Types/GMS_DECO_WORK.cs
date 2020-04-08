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
    public class GMS_DECO_WORK : AppMain.IClearable, AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.OBS_ACTION3D_NN_WORK obj_3d = new AppMain.OBS_ACTION3D_NN_WORK();
        public readonly AppMain.OBS_RECT_WORK[] rect_work = AppMain.New<AppMain.OBS_RECT_WORK>(1);
        public readonly AppMain.OBS_OBJECT_WORK obj_work;
        public AppMain.GMS_EVE_RECORD_DECORATE event_record;
        public byte event_x;
        public AppMain.AOS_TEXTURE model_tex;
        public int model_index;
        public readonly object holder;

        public static explicit operator AppMain.GMS_DECO_SUBMODEL_WORK(AppMain.GMS_DECO_WORK p)
        {
            return (AppMain.GMS_DECO_SUBMODEL_WORK)p.holder;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_DECO_WORK work)
        {
            return work.obj_work;
        }

        public void Clear()
        {
            this.obj_work.Clear();
            this.obj_3d.Clear();
            AppMain.ClearArray<AppMain.OBS_RECT_WORK>(this.rect_work);
            this.event_record = (AppMain.GMS_EVE_RECORD_DECORATE)null;
            this.event_x = (byte)0;
            this.model_tex = (AppMain.AOS_TEXTURE)null;
            this.model_index = 0;
        }

        public GMS_DECO_WORK()
        {
            this.obj_work = AppMain.OBS_OBJECT_WORK.Create((object)this);
        }

        public GMS_DECO_WORK(object _holder)
        {
            this.holder = _holder;
            this.obj_work = AppMain.OBS_OBJECT_WORK.Create((object)this);
        }
    }
}
