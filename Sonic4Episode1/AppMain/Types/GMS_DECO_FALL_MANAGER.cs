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
    public class GMS_DECO_FALL_MANAGER : AppMain.IClearable
    {
        public AppMain.GMS_DECO_FALL_REGISTER[] reg = AppMain.New<AppMain.GMS_DECO_FALL_REGISTER>(8);
        public uint dec_id;
        public AppMain.NNS_TEXLIST texlist;
        public ushort all_num;
        public ushort reg_num;
        public float frame;

        public void Clear()
        {
            this.dec_id = 0U;
            this.texlist = (AppMain.NNS_TEXLIST)null;
            this.all_num = this.reg_num = (ushort)0;
            this.frame = 0.0f;
            AppMain.ClearArray<AppMain.GMS_DECO_FALL_REGISTER>(this.reg);
        }
    }
}
