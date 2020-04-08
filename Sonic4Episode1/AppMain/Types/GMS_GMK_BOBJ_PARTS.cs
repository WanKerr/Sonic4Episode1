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
    public class GMS_GMK_BOBJ_PARTS : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_EFFECT_3DNN_WORK eff_work;
        public short falltimer;

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.eff_work.efct_com.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_GMK_BOBJ_PARTS work)
        {
            return work.eff_work.efct_com.obj_work;
        }

        public GMS_GMK_BOBJ_PARTS()
        {
            this.eff_work = new AppMain.GMS_EFFECT_3DNN_WORK((object)this);
        }
    }
}
