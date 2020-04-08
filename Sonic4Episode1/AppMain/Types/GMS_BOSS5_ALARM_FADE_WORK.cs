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
    public class GMS_BOSS5_ALARM_FADE_WORK : AppMain.IOBS_OBJECT_WORK
    {
        public readonly AppMain.GMS_BOSS5_1SHOT_TIMER alert_se_timer = new AppMain.GMS_BOSS5_1SHOT_TIMER();
        public readonly AppMain.GMS_FADE_OBJ_WORK fade_obj;
        public AppMain.GMS_BOSS5_MGR_WORK mgr_work;
        public AppMain.MPP_VOID_GMS_BOSS5_ALARM_FADE_WORK proc_update;
        public int cur_phase;
        public int cur_level;
        public uint wait_timer;
        public int alert_se_ref_level;

        public GMS_BOSS5_ALARM_FADE_WORK()
        {
            this.fade_obj = new AppMain.GMS_FADE_OBJ_WORK((object)this);
        }

        public AppMain.OBS_OBJECT_WORK Cast()
        {
            return this.fade_obj.obj_work;
        }

        public static explicit operator AppMain.OBS_OBJECT_WORK(
          AppMain.GMS_BOSS5_ALARM_FADE_WORK work)
        {
            return work.fade_obj.obj_work;
        }
    }
}
