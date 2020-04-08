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
    public class NNS_LIGHT_TARGET_DIRECTIONAL
    {
        public readonly AppMain.NNS_VECTOR Position = new AppMain.NNS_VECTOR();
        public readonly AppMain.NNS_VECTOR Target = new AppMain.NNS_VECTOR();
        public uint User;
        public AppMain.NNS_RGBA Color;
        public float Intensity;
        public float InnerRange;
        public float OuterRange;
        public float FallOffStart;
        public float FallOffEnd;
        public float dummy;

        public static explicit operator AppMain.NNS_LIGHT_PARALLEL(
          AppMain.NNS_LIGHT_TARGET_DIRECTIONAL light)
        {
            return new AppMain.NNS_LIGHT_PARALLEL(light);
        }

        public static explicit operator AppMain.NNS_LIGHT_POINT(
          AppMain.NNS_LIGHT_TARGET_DIRECTIONAL light)
        {
            return new AppMain.NNS_LIGHT_POINT(light);
        }

        public static explicit operator AppMain.NNS_LIGHT_TARGET_SPOT(
          AppMain.NNS_LIGHT_TARGET_DIRECTIONAL light)
        {
            return new AppMain.NNS_LIGHT_TARGET_SPOT(light);
        }

        public static explicit operator AppMain.NNS_LIGHT_ROTATION_SPOT(
          AppMain.NNS_LIGHT_TARGET_DIRECTIONAL light)
        {
            return new AppMain.NNS_LIGHT_ROTATION_SPOT(light);
        }
    }
}
