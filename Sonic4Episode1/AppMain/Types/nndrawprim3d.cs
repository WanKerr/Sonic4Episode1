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
    private static class nndrawprim3d
    {
        public static readonly float[] nnsDiffuse = new float[4]
        {
      0.8f,
      0.8f,
      0.8f,
      1f
        };
        public static readonly float[] nnsAmbient = new float[4]
        {
      0.2f,
      0.2f,
      0.2f,
      1f
        };
        public static readonly float[] nnsSpecular = new float[4]
        {
      0.0f,
      0.0f,
      0.0f,
      1f
        };
        public static float nnsShininess = 16f;
        public static readonly float[] nnsEmission = new float[4]
        {
      0.0f,
      0.0f,
      0.0f,
      1f
        };
        public static readonly AppMain.NNS_MATRIX nnsPrim3DMatrix = AppMain.GlobalPool<AppMain.NNS_MATRIX>.Alloc();
        public static uint nnsAlphaFunc = 516;
        public static float nnsAlphaFuncRef = 0.0f;
        public static uint nnsDepthFunc = 515;
        public static bool nnsDepthMask = true;
        public static int nnsFormat;
    }
}
