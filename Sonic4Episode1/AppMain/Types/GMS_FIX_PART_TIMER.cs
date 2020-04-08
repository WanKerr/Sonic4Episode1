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
    public class GMS_FIX_PART_TIMER
    {
        public AppMain.GMS_COCKPIT_2D_WORK[] sub_parts = new AppMain.GMS_COCKPIT_2D_WORK[8];
        public int[] digit_list = new int[5];
        public ushort[] reserved = new ushort[1];
        public readonly AppMain.GMS_FIX_PART_WORK part_work;
        public uint flag;
        public float digit_frame_ofst;
        public float deco_char_frame_ofst;
        public ushort cur_sec;
        public float fade_ratio;
        public float scale_ratio;
        public uint flash_act_phase;

        public GMS_FIX_PART_TIMER()
        {
            this.part_work = new AppMain.GMS_FIX_PART_WORK((object)this);
        }

        public static explicit operator AppMain.GMS_FIX_PART_WORK(AppMain.GMS_FIX_PART_TIMER p)
        {
            return p.part_work;
        }

        public void Clear()
        {
            this.part_work.Clear();
            Array.Clear((Array)this.sub_parts, 0, this.sub_parts.Length);
            this.flag = 0U;
            Array.Clear((Array)this.digit_list, 0, this.digit_list.Length);
            this.digit_frame_ofst = this.deco_char_frame_ofst = 0.0f;
            this.cur_sec = (ushort)0;
            this.reserved[0] = (ushort)0;
            this.fade_ratio = this.scale_ratio = 0.0f;
            this.flash_act_phase = 0U;
        }
    }
}
