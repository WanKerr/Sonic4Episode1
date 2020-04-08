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
    private class nnlight
    {
        public static AppMain.NNS_MATRIX nngLightMtx = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public static AppMain.NNS_GL_LIGHT nngLight = new AppMain.NNS_GL_LIGHT();
        public static float[] nngPointLightFallOffEnd = new float[4];
        public static float[] nngPointLightFallOffScale = new float[4];
        public static float[] nngSpotLightFallOffEnd = new float[4];
        public static float[] nngSpotLightFallOffScale = new float[4];
        public static float[] nngSpotLightAngleScale = new float[4];
        public static int nngNumParallelLight;
        public static int nngNumPointLight;
        public static int nngNumSpotLight;
    }
}
