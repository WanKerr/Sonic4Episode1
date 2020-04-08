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
    public class OBS_ACT_TBL
    {
        public ushort act_id;
        public byte time;
        public byte flag;

        public OBS_ACT_TBL()
        {
        }

        public OBS_ACT_TBL(AppMain.OBS_ACT_TBL obsTbl)
        {
            this.act_id = obsTbl.act_id;
            this.time = obsTbl.time;
            this.flag = obsTbl.flag;
        }

        public AppMain.OBS_ACT_TBL Assign(AppMain.OBS_ACT_TBL obsTbl)
        {
            this.act_id = obsTbl.act_id;
            this.time = obsTbl.time;
            this.flag = obsTbl.flag;
            return this;
        }
    }
}
