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
    public class OBS_SCALE_TBL
    {
        public AppMain.VecFx32 scale = new AppMain.VecFx32();
        public byte time;
        public byte flag;

        public OBS_SCALE_TBL()
        {
        }

        public OBS_SCALE_TBL(AppMain.OBS_SCALE_TBL obsTbl)
        {
            this.scale.Assign(obsTbl.scale);
            this.time = obsTbl.time;
            this.flag = obsTbl.flag;
        }

        public AppMain.OBS_SCALE_TBL Assign(AppMain.OBS_SCALE_TBL obsTbl)
        {
            if (this != obsTbl)
            {
                this.scale.Assign(obsTbl.scale);
                this.time = obsTbl.time;
                this.flag = obsTbl.flag;
            }
            return this;
        }
    }
}
