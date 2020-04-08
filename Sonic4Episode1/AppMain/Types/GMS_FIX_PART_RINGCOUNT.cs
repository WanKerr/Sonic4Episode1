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
    public class GMS_FIX_PART_RINGCOUNT
    {
        public AppMain.GMS_COCKPIT_2D_WORK[] sub_parts = new AppMain.GMS_COCKPIT_2D_WORK[4];
        public int[] digit_list = new int[3];
        public readonly AppMain.GMS_FIX_PART_WORK part_work;

        public static explicit operator AppMain.GMS_FIX_PART_WORK(
          AppMain.GMS_FIX_PART_RINGCOUNT pObj)
        {
            return pObj.part_work;
        }

        public GMS_FIX_PART_RINGCOUNT()
        {
            this.part_work = new AppMain.GMS_FIX_PART_WORK((object)this);
        }

        public void Clear()
        {
            this.part_work.Clear();
            Array.Clear((Array)this.sub_parts, 0, this.sub_parts.Length);
            Array.Clear((Array)this.digit_list, 0, this.digit_list.Length);
        }
    }
}
