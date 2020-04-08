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
    public class GMS_FIX_PART_WORK
    {
        public readonly object holder;
        public AppMain.GMS_FIX_MGR_WORK parent_mgr;
        public int part_type;
        public uint flag;
        public uint blink_timer;
        public uint blink_on_time;
        public uint blink_off_time;
        public AppMain.MPP_VOID_GMS_FIX_PART_WORK proc_update;
        public AppMain.MPP_VOID_GMS_FIX_PART_WORK proc_disp;

        public GMS_FIX_PART_WORK(object _holder)
        {
            this.holder = _holder;
        }

        public GMS_FIX_PART_WORK()
        {
        }

        public static explicit operator AppMain.GMS_FIX_PART_VIRTUAL_PAD(
          AppMain.GMS_FIX_PART_WORK p)
        {
            return (AppMain.GMS_FIX_PART_VIRTUAL_PAD)p.holder;
        }

        public static explicit operator AppMain.GMS_FIX_PART_SCORE(AppMain.GMS_FIX_PART_WORK p)
        {
            return (AppMain.GMS_FIX_PART_SCORE)p.holder;
        }

        public static explicit operator AppMain.GMS_FIX_PART_TIMER(AppMain.GMS_FIX_PART_WORK p)
        {
            return (AppMain.GMS_FIX_PART_TIMER)p.holder;
        }

        public static explicit operator AppMain.GMS_FIX_PART_RINGCOUNT(
          AppMain.GMS_FIX_PART_WORK p)
        {
            return (AppMain.GMS_FIX_PART_RINGCOUNT)p.holder;
        }

        public static explicit operator AppMain.GMS_FIX_PART_CHALLENGE(
          AppMain.GMS_FIX_PART_WORK p)
        {
            return (AppMain.GMS_FIX_PART_CHALLENGE)p.holder;
        }

        public static explicit operator AppMain.GMS_FIX_MGR_WORK(AppMain.GMS_FIX_PART_WORK p)
        {
            return p.parent_mgr;
        }

        public void Clear()
        {
            this.parent_mgr = (AppMain.GMS_FIX_MGR_WORK)null;
            this.part_type = 0;
            this.flag = 0U;
            this.blink_timer = this.blink_on_time = this.blink_off_time = 0U;
            this.proc_update = this.proc_disp = (AppMain.MPP_VOID_GMS_FIX_PART_WORK)null;
        }
    }
}
