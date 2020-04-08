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
    public class GMS_OVER_MGR_WORK
    {
        public readonly AppMain.GMS_COCKPIT_2D_WORK[] string_sub_parts = new AppMain.GMS_COCKPIT_2D_WORK[4];
        public readonly AppMain.GMS_COCKPIT_2D_WORK[] fadeout_sub_parts = new AppMain.GMS_COCKPIT_2D_WORK[2];
        public AppMain._GMS_OVER_MGR_WORK_UD_ proc_update;
        public AppMain._GMS_OVER_MGR_WORK_UD_ proc_disp;
        public uint wait_timer;

        internal void Clear()
        {
            this.proc_update = (AppMain._GMS_OVER_MGR_WORK_UD_)null;
            this.proc_disp = (AppMain._GMS_OVER_MGR_WORK_UD_)null;
            this.wait_timer = 0U;
            Array.Clear((Array)this.string_sub_parts, 0, this.string_sub_parts.Length);
            Array.Clear((Array)this.fadeout_sub_parts, 0, this.fadeout_sub_parts.Length);
        }
    }
}
