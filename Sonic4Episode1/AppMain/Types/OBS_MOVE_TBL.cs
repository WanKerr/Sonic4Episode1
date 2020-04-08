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
    public class OBS_MOVE_TBL
    {
        public AppMain.VecFx32 spd = new AppMain.VecFx32();
        public AppMain.VecFx32 spd_add = new AppMain.VecFx32();
        public byte time;
        public byte flag;

        public OBS_MOVE_TBL()
        {
        }

        public OBS_MOVE_TBL(AppMain.OBS_MOVE_TBL obsTbl)
        {
            this.spd.Assign(obsTbl.spd);
            this.spd_add.Assign(obsTbl.spd_add);
            this.time = obsTbl.time;
            this.flag = obsTbl.flag;
        }

        public AppMain.OBS_MOVE_TBL Assign(AppMain.OBS_MOVE_TBL obsTbl)
        {
            if (this != obsTbl)
            {
                this.spd.Assign(obsTbl.spd);
                this.spd_add.Assign(obsTbl.spd_add);
                this.time = obsTbl.time;
                this.flag = obsTbl.flag;
            }
            return this;
        }
    }
}
