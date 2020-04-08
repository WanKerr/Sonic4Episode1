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
    public class GMS_BOSS5_ARM_ANIM_INFO
    {
        public AppMain.GMS_BOSS5_ARM_PART_ANIM_INFO[] part_anim_info = AppMain.New<AppMain.GMS_BOSS5_ARM_PART_ANIM_INFO>(3);
        public uint wait_time;
        public float slerp_inc_rate;

        public GMS_BOSS5_ARM_ANIM_INFO()
        {
        }

        public GMS_BOSS5_ARM_ANIM_INFO(
          uint wait,
          float sincrate,
          AppMain.GMS_BOSS5_ARM_PART_ANIM_INFO[] part)
        {
            this.wait_time = wait;
            this.slerp_inc_rate = sincrate;
            this.part_anim_info = part;
        }
    }
}
