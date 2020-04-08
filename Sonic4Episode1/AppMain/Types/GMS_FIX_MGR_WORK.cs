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
    public class GMS_FIX_MGR_WORK
    {
        public AppMain.GMS_FIX_PART_WORK[] part_work = new AppMain.GMS_FIX_PART_WORK[5];
        public AppMain.GMS_FIX_PART_RINGCOUNT part_ringcount = new AppMain.GMS_FIX_PART_RINGCOUNT();
        public AppMain.GMS_FIX_PART_SCORE part_score = new AppMain.GMS_FIX_PART_SCORE();
        public AppMain.GMS_FIX_PART_TIMER part_timer = new AppMain.GMS_FIX_PART_TIMER();
        public AppMain.GMS_FIX_PART_CHALLENGE part_challenge = new AppMain.GMS_FIX_PART_CHALLENGE();
        public AppMain.GMS_FIX_PART_VIRTUAL_PAD part_virtual_pad = new AppMain.GMS_FIX_PART_VIRTUAL_PAD();
        public uint flag;
        public uint req_flag;
        public AppMain.MPP_VOID_GMS_FIX_PART_WORK proc_update;
        public readonly object holder;

        public GMS_FIX_MGR_WORK()
        {
        }

        public GMS_FIX_MGR_WORK(AppMain.GMS_FIX_PART_WORK holder)
        {
            this.holder = (object)holder;
        }

        public void Clear()
        {
            this.flag = this.req_flag = 0U;
            this.proc_update = (AppMain.MPP_VOID_GMS_FIX_PART_WORK)null;
            Array.Clear((Array)this.part_work, 0, 5);
            this.part_ringcount.Clear();
            this.part_score.Clear();
            this.part_timer.Clear();
            this.part_challenge.Clear();
            this.part_virtual_pad.Clear();
        }

        public static explicit operator AppMain.GMS_FIX_PART_WORK(AppMain.GMS_FIX_MGR_WORK p)
        {
            return (AppMain.GMS_FIX_PART_WORK)p.holder;
        }
    }
}
