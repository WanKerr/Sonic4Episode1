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
    private static class nndrawcircumsphere
    {
        public static readonly AppMain.NNS_RGBA[] nnsMsstCircumCol = new AppMain.NNS_RGBA[8]
        {
      new AppMain.NNS_RGBA(0.0f, 1f, 0.0f, 0.3f),
      new AppMain.NNS_RGBA(1f, 0.0f, 1f, 0.3f),
      new AppMain.NNS_RGBA(1f, 1f, 0.0f, 0.3f),
      new AppMain.NNS_RGBA(1f, 1f, 1f, 0.3f),
      new AppMain.NNS_RGBA(0.0f, 1f, 1f, 0.3f),
      new AppMain.NNS_RGBA(1f, 0.0f, 0.0f, 0.3f),
      new AppMain.NNS_RGBA(0.0f, 0.0f, 0.0f, 0.3f),
      new AppMain.NNS_RGBA()
        };
        public static int nnsSubMotIdx;
        public static AppMain.NNS_MATRIX nnsBaseMtx;
        public static AppMain.NNS_OBJECT nnsObj;
        public static AppMain.NNS_NODE nnsNodeList;
        public static AppMain.NNS_MATRIXSTACK nnsMstk;
        public static AppMain.NNS_MOTION nnsMot;
        public static AppMain.NNS_TRS nnsTrsList;
        public static float nnsFrame;
        public static uint nnsDrawCsFlag;
    }
}
