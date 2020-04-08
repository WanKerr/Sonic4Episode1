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
    public class GMS_EFFECT_3DNN_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public AppMain.OBS_ACTION3D_NN_WORK obj_3d = new AppMain.OBS_ACTION3D_NN_WORK();
        public AppMain.GMS_EFFECT_COM_WORK efct_com;
        public readonly object holder;

        public GMS_EFFECT_3DNN_WORK()
        {
            this.efct_com = new AppMain.GMS_EFFECT_COM_WORK((object)this);
        }

        public GMS_EFFECT_3DNN_WORK(object _holder)
        {
            this.efct_com = new AppMain.GMS_EFFECT_COM_WORK((object)this);
            this.holder = _holder;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.efct_com.obj_work;
        }

        public static explicit operator AppMain.GMS_GMK_PRESSWALL_PARTS(
          AppMain.GMS_EFFECT_3DNN_WORK work)
        {
            return (AppMain.GMS_GMK_PRESSWALL_PARTS)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_SEESAWPARTS_WORK(
          AppMain.GMS_EFFECT_3DNN_WORK work)
        {
            return (AppMain.GMS_GMK_SEESAWPARTS_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_POPSTEAMPARTS_WORK(
          AppMain.GMS_EFFECT_3DNN_WORK work)
        {
            return (AppMain.GMS_GMK_POPSTEAMPARTS_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_PISTONROD_WORK(
          AppMain.GMS_EFFECT_3DNN_WORK work)
        {
            return (AppMain.GMS_GMK_PISTONROD_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_CANNONPARTS_WORK(
          AppMain.GMS_EFFECT_3DNN_WORK work)
        {
            return (AppMain.GMS_GMK_CANNONPARTS_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_LDPART_WORK(
          AppMain.GMS_EFFECT_3DNN_WORK work)
        {
            return (AppMain.GMS_BOSS5_LDPART_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_SLOTPARTS_WORK(
          AppMain.GMS_EFFECT_3DNN_WORK work)
        {
            return (AppMain.GMS_GMK_SLOTPARTS_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_BWALL_PARTS(
          AppMain.GMS_EFFECT_3DNN_WORK work)
        {
            return (AppMain.GMS_GMK_BWALL_PARTS)work.holder;
        }

        public static explicit operator AppMain.GMS_GMK_BOBJ_PARTS(
          AppMain.GMS_EFFECT_3DNN_WORK work)
        {
            return (AppMain.GMS_GMK_BOBJ_PARTS)work.holder;
        }
    }
}
