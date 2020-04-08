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
    public class OBS_LIGHT
    {
        private readonly AppMain.NNS_LIGHT_TARGET_DIRECTIONAL light_data_ = new AppMain.NNS_LIGHT_TARGET_DIRECTIONAL();
        public const int ELT_LIGHT_PARALLEL = 0;
        public const int ELT_LIGHT_POINT = 1;
        public const int ELT_LIGHT_TARGET_SPOT = 2;
        public const int ELT_LIGHT_ROTATION_SPOT = 3;
        public uint light_type;

        public AppMain.NNS_LIGHT_PARALLEL parallel
        {
            get
            {
                return (AppMain.NNS_LIGHT_PARALLEL)this.light_data_;
            }
        }

        public AppMain.NNS_LIGHT_POINT point
        {
            get
            {
                return (AppMain.NNS_LIGHT_POINT)this.light_data_;
            }
        }

        public AppMain.NNS_LIGHT_TARGET_SPOT target_spot
        {
            get
            {
                return (AppMain.NNS_LIGHT_TARGET_SPOT)this.light_data_;
            }
        }

        public AppMain.NNS_LIGHT_ROTATION_SPOT rotation_spot
        {
            get
            {
                return (AppMain.NNS_LIGHT_ROTATION_SPOT)this.light_data_;
            }
        }

        public AppMain.NNS_LIGHT_TARGET_DIRECTIONAL light_param
        {
            get
            {
                return this.light_data_;
            }
        }
    }
}
