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
    public class GMS_RING_WORK : AppMain.IClearable
    {
        public AppMain.VecFx32 pos = new AppMain.VecFx32();
        public AppMain.VecFx32 scale = new AppMain.VecFx32();
        public int spd_x;
        public int spd_y;
        public short timer;
        public ushort flag;
        public AppMain.GMS_EVE_RECORD_RING eve_rec;
        public AppMain.GMS_RING_WORK pre_ring;
        public AppMain.GMS_RING_WORK post_ring;
        public AppMain.OBS_OBJECT_WORK duct_obj;

        public void Clear()
        {
            this.pos.Clear();
            this.scale.Clear();
            this.spd_x = 0;
            this.spd_y = 0;
            this.timer = (short)0;
            this.flag = (ushort)0;
            this.eve_rec = (AppMain.GMS_EVE_RECORD_RING)null;
            this.pre_ring = (AppMain.GMS_RING_WORK)null;
            this.post_ring = (AppMain.GMS_RING_WORK)null;
            this.duct_obj = (AppMain.OBS_OBJECT_WORK)null;
        }
    }
}
