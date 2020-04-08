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
    public class GMS_EFFECT_COM_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public AppMain.OBS_RECT_WORK[] rect_work = AppMain.New<AppMain.OBS_RECT_WORK>(2);
        public AppMain.OBS_OBJECT_WORK obj_work;
        public readonly object holder;

        public void Clear()
        {
            this.obj_work.Clear();
            for (int index = 0; index < 2; ++index)
                this.rect_work[index].Clear();
        }

        public static explicit operator AppMain.GMS_BOSS1_FLASH_SCREEN_WORK(
          AppMain.GMS_EFFECT_COM_WORK p)
        {
            return (AppMain.GMS_BOSS1_FLASH_SCREEN_WORK)p.holder;
        }

        public static explicit operator AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT(
          AppMain.GMS_EFFECT_COM_WORK p)
        {
            return (AppMain.GMS_BS_CMN_NODE_CTRL_OBJECT)p.holder;
        }

        public static explicit operator AppMain.GMS_EFFECT_COM_WORK(AppMain.OBS_OBJECT_WORK work)
        {
            return work.holder is AppMain.GMS_EFFECT_COM_WORK ? (AppMain.GMS_EFFECT_COM_WORK)work.holder : ((AppMain.GMS_EFFECT_3DES_WORK)work.holder).efct_com;
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(AppMain.GMS_EFFECT_COM_WORK work)
        {
            return work.obj_work;
        }

        public GMS_EFFECT_COM_WORK()
        {
            this.obj_work = AppMain.OBS_OBJECT_WORK.Create((object)this);
        }

        public GMS_EFFECT_COM_WORK(object _holder)
        {
            this.obj_work = AppMain.OBS_OBJECT_WORK.Create((object)this);
            this.holder = _holder;
        }

        public static explicit operator AppMain.GMS_GMK_POPSTEAMPARTS_WORK(
          AppMain.GMS_EFFECT_COM_WORK work)
        {
            return (AppMain.GMS_GMK_POPSTEAMPARTS_WORK)(AppMain.GMS_EFFECT_3DNN_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_BOSS5_FLASH_SCREEN_WORK(
          AppMain.GMS_EFFECT_COM_WORK work)
        {
            return (AppMain.GMS_BOSS5_FLASH_SCREEN_WORK)work.holder;
        }

        public static explicit operator AppMain.GMS_EFFECT_3DES_WORK(
          AppMain.GMS_EFFECT_COM_WORK work)
        {
            return (AppMain.GMS_EFFECT_3DES_WORK)work.holder;
        }
    }
}
