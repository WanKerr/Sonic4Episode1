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
    private static class nncalctrsmotion
    {
        public static readonly AppMain.NNS_MATRIX nnsBaseMtx = new AppMain.NNS_MATRIX();
        public static float nnsRootScale = 1f;
        public static AppMain.NNS_OBJECT nnsObj;
        public static AppMain.NNS_MATRIX[] nnsMtxPal;
        public static uint[] nnsNodeStatList;
        public static uint nnsNSFlag;
        public static AppMain.NNS_NODE[] nnsNodeList;
        public static AppMain.NNS_TRS[] nnsTrsList;
        public static AppMain.NNS_MATRIXSTACK nnsMstk;
        public static AppMain.NNS_MOTION nnsMot0;
        public static AppMain.NNS_MOTION nnsMot1;
        public static float nnsFrame0;
        public static float nnsFrame1;
        public static float nnsRatio;
        public static int nnsSubMotIdx0;
        public static int nnsSubMotIdx1;
    }
}
