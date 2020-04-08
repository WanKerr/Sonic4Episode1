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
    public struct GMS_PLY_PARAMETER
    {
        public int spd_add;
        public int spd_max;
        public int spd_dec;
        public int spd_spin;
        public int spd_add_spin;
        public int spd_max_spin;
        public int spd_dec_spin;
        public int spd_max_add_slope;
        public short time_air;
        public short time_damage;
        public short pool_max;
        public ushort fall_wait_time;
        public int spd_slope;
        public int spd_slope_max;
        public int spd_slope_spin;
        public int spd_slope_spin_spipe;
        public int spd_slope_spin_pinball;
        public int spd_jump;
        public int spd_fall;
        public int spd_fall_max;
        public int push_max;
        public int spd_jump_add;
        public int spd_jump_max;
        public int spd_jump_dec;
        public int spd_add_spin_pinball;
        public int spd_max_spin_pinball;
        public int spd_dec_spin_pinball;
        public int spd_max_add_slope_spin_pinball;

        public GMS_PLY_PARAMETER(
          int spd_add,
          int spd_max,
          int spd_dec,
          int spd_spin,
          int spd_add_spin,
          int spd_max_spin,
          int spd_dec_spin,
          int spd_max_add_slope,
          short time_air,
          short time_damage,
          short pool_max,
          ushort fall_wait_time,
          int spd_slope,
          int spd_slope_max,
          int spd_slope_spin,
          int spd_slope_spin_spipe,
          int spd_slope_spin_pinball,
          int spd_jump,
          int spd_fall,
          int spd_fall_max,
          int push_max,
          int spd_jump_add,
          int spd_jump_max,
          int spd_jump_dec,
          int spd_add_spin_pinball,
          int spd_max_spin_pinball,
          int spd_dec_spin_pinball,
          int spd_max_add_slope_spin_pinball)
        {
            this.spd_add = spd_add;
            this.spd_max = spd_max;
            this.spd_dec = spd_dec;
            this.spd_spin = spd_spin;
            this.spd_add_spin = spd_add_spin;
            this.spd_max_spin = spd_max_spin;
            this.spd_dec_spin = spd_dec_spin;
            this.spd_max_add_slope = spd_max_add_slope;
            this.time_air = time_air;
            this.time_damage = time_damage;
            this.pool_max = pool_max;
            this.fall_wait_time = fall_wait_time;
            this.spd_slope = spd_slope;
            this.spd_slope_max = spd_slope_max;
            this.spd_slope_spin = spd_slope_spin;
            this.spd_slope_spin_spipe = spd_slope_spin_spipe;
            this.spd_slope_spin_pinball = spd_slope_spin_pinball;
            this.spd_jump = spd_jump;
            this.spd_fall = spd_fall;
            this.spd_fall_max = spd_fall_max;
            this.push_max = push_max;
            this.spd_jump_add = spd_jump_add;
            this.spd_jump_max = spd_jump_max;
            this.spd_jump_dec = spd_jump_dec;
            this.spd_add_spin_pinball = spd_add_spin_pinball;
            this.spd_max_spin_pinball = spd_max_spin_pinball;
            this.spd_dec_spin_pinball = spd_dec_spin_pinball;
            this.spd_max_add_slope_spin_pinball = spd_max_add_slope_spin_pinball;
        }
    }
}
